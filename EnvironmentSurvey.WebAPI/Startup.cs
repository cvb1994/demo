using EnvironmentSurvey.WebAPI.BusinessLogic;
using EnvironmentSurvey.WebAPI.ClientSide.Common;
using EnvironmentSurvey.WebAPI.DataAccess;
using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using Polly;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
               options.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddOptions();
            var mailsettings = Configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailsettings);
            services.AddScoped<ISendMailService, SendMailService>();
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", op => op.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            //services.AddDbContext<ESContext>(options => options.UseMySQL(Configuration.GetConnectionString("EnvironmentSurvey")));
            services.AddDbContext<ESContext>(options => options.UseMySQL(StartupExtensions.GetMySqlConnectionString().ToString()));
            services.AddSingleton(sp => StartupExtensions.GetMySqlConnectionString());
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISeminarService, SeminarService>();
            services.AddScoped<IUserSeminarService, UserSeminarService>();
            services.AddScoped<IUserAnswerService, UserAnswerService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<ISurveyQuestionService, SurveyQuestionService>();
            services.AddScoped<ISurveyService, SurveyService>();
            services.AddScoped<IFAQsService, FAQsService>();
            services.AddScoped<ISupportInfoService, SupportInfoService>();
            services.AddScoped<IReportSevice, ReportService>();
            services.AddScoped<IResultService, ResultService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddControllers();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"].ToString())),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EnvironmentSurvey.WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EnvironmentSurvey.WebAPI v1"));
            }
            app.UseAuthentication();

            //app.UseCors(op => op.WithOrigins(Configuration["Client_URL"]).AllowAnyMethod().AllowAnyHeader());
            //app.UseCors(op => op.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    static class StartupExtensions
    {
        public static void OpenWithRetry(this DbConnection connection) =>
            // [START cloud_sql_mysql_dotnet_ado_backoff]
            Policy
                .Handle<MySqlException>()
                .WaitAndRetry(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(5)
                })
                .Execute(() => connection.Open());
        // [END cloud_sql_mysql_dotnet_ado_backoff]

        public static void InitializeDatabase()
        {
            var connectionString = GetMySqlConnectionString();
            using (DbConnection connection = new MySqlConnection(connectionString.ConnectionString))
            {
                connection.OpenWithRetry();
                using (var createTableCommand = connection.CreateCommand())
                {
                    createTableCommand.CommandText = @"
                        CREATE TABLE IF NOT EXISTS
                        votes(
                            vote_id SERIAL NOT NULL,
                            time_cast timestamp NOT NULL,
                            candidate CHAR(6) NOT NULL,
                            PRIMARY KEY (vote_id)
                        )";
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }

        public static MySqlConnectionStringBuilder GetMySqlConnectionString()
        {
            MySqlConnectionStringBuilder connectionString;
            if (Environment.GetEnvironmentVariable("DB_HOST") != null)
            {
                connectionString = NewMysqlTCPConnectionString();
            }
            else
            {
                connectionString = NewMysqlUnixSocketConnectionString();
            }
            // The values set here are for demonstration purposes only. You 
            // should set these values to what works best for your application.
            // [START cloud_sql_mysql_dotnet_ado_limit]
            // MaximumPoolSize sets maximum number of connections allowed in the pool.
            connectionString.MaximumPoolSize = 5;
            // MinimumPoolSize sets the minimum number of connections in the pool.
            connectionString.MinimumPoolSize = 0;
            // [END cloud_sql_mysql_dotnet_ado_limit]
            // [START cloud_sql_mysql_dotnet_ado_timeout]
            // ConnectionTimeout sets the time to wait (in seconds) while
            // trying to establish a connection before terminating the attempt.
            connectionString.ConnectionTimeout = 15;
            // [END cloud_sql_mysql_dotnet_ado_timeout]
            // [START cloud_sql_mysql_dotnet_ado_lifetime]
            // ConnectionLifeTime sets the lifetime of a pooled connection
            // (in seconds) that a connection lives before it is destroyed
            // and recreated. Connections that are returned to the pool are
            // destroyed if it's been more than the number of seconds
            // specified by ConnectionLifeTime since the connection was
            // created. The default value is zero (0) which means the
            // connection always returns to pool.
            connectionString.ConnectionLifeTime = 1800; // 30 minutes
            // [END cloud_sql_mysql_dotnet_ado_lifetime]
            return connectionString;
        }

        public static MySqlConnectionStringBuilder NewMysqlTCPConnectionString()
        {
            // [START cloud_sql_mysql_dotnet_ado_connection_tcp]
            // Equivalent connection string:
            // "Uid=<DB_USER>;Pwd=<DB_PASS>;Host=<DB_HOST>;Database=<DB_NAME>;"
            var connectionString = new MySqlConnectionStringBuilder()
            {
                // The Cloud SQL proxy provides encryption between the proxy and instance.
                SslMode = MySqlSslMode.None,

                // Remember - storing secrets in plain text is potentially unsafe. Consider using
                // something like https://cloud.google.com/secret-manager/docs/overview to help keep
                // secrets secret.
                Server = Environment.GetEnvironmentVariable("DB_HOST"),   // e.g. '127.0.0.1'
                // Set Host to 'cloudsql' when deploying to App Engine Flexible environment
                UserID = Environment.GetEnvironmentVariable("DB_USER"),   // e.g. 'my-db-user'
                Password = Environment.GetEnvironmentVariable("DB_PASS"), // e.g. 'my-db-password'
                Database = Environment.GetEnvironmentVariable("DB_NAME"), // e.g. 'my-database'
            };
            connectionString.Pooling = true;
            // Specify additional properties here.
            return connectionString;
            // [END cloud_sql_mysql_dotnet_ado_connection_tcp]
        }

        public static MySqlConnectionStringBuilder NewMysqlUnixSocketConnectionString()
        {
            // [START cloud_sql_mysql_dotnet_ado_connection_socket]
            // Equivalent connection string:
            // "Server=<dbSocketDir>/<INSTANCE_CONNECTION_NAME>;Uid=<DB_USER>;Pwd=<DB_PASS>;Database=<DB_NAME>;Protocol=unix"
            String dbSocketDir = Environment.GetEnvironmentVariable("DB_SOCKET_PATH") ?? "/cloudsql";
            String instanceConnectionName = Environment.GetEnvironmentVariable("INSTANCE_CONNECTION_NAME");
            var connectionString = new MySqlConnectionStringBuilder()
            {
                // The Cloud SQL proxy provides encryption between the proxy and instance.
                SslMode = MySqlSslMode.None,
                // Remember - storing secrets in plain text is potentially unsafe. Consider using
                // something like https://cloud.google.com/secret-manager/docs/overview to help keep
                // secrets secret.
                Server = String.Format("{0}/{1}", dbSocketDir, instanceConnectionName),
                UserID = Environment.GetEnvironmentVariable("DB_USER"),   // e.g. 'my-db-user
                Password = Environment.GetEnvironmentVariable("DB_PASS"), // e.g. 'my-db-password'
                Database = Environment.GetEnvironmentVariable("DB_NAME"), // e.g. 'my-database'
                ConnectionProtocol = MySqlConnectionProtocol.UnixSocket
            };
            connectionString.Pooling = true;
            // Specify additional properties here.
            return connectionString;
            // [END cloud_sql_mysql_dotnet_ado_connection_socket]
        }
    }
}
