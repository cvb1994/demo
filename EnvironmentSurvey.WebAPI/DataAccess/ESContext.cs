using System;
using EnvironmentSurvey.WebAPI.DataAccess.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EnvironmentSurvey.WebAPI.DataAccess
{
    public partial class ESContext : DbContext
    {
        public ESContext()
        {
        }

        public ESContext(DbContextOptions<ESContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Result> Results { get; set; }
        public virtual DbSet<Seminar> Seminars { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAnswer> UserAnswers { get; set; }
        public virtual DbSet<UserSeminar> UserSeminars { get; set; }
        public virtual DbSet<SupportInformation> SupportInformations { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("Server=35.247.129.160;Database=ES;Uid=group1;Pwd=123456789;");
            }*/

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.Answer1)
                    .HasColumnType("text")
                    .HasColumnName("Answer");

                entity.Property(e => e.IsCorrect).HasColumnName("isCorrect");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Answers__Questio__30F848ED");
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("FAQs");

                entity.Property(e => e.Issue)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Solution)
                    .IsRequired()
                    .HasColumnType("text");
            });

            modelBuilder.Entity<SupportInformation>(entity =>
            {
                entity.ToTable("SupportInfo");

                entity.Property(e => e.Company)
                    .IsRequired()
                    .HasColumnType("text");
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnType("text");
                entity.Property(e => e.CompanyTel)
                    .HasColumnType("text");
                entity.Property(e => e.Supporter)
                    .HasColumnType("text");
                entity.Property(e => e.SupporterTel)
                    .HasColumnType("text");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Question1)
                    .HasColumnType("text")
                    .HasColumnName("Question");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Questions__Subject__36B17696");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subjects");

                /*entity.Property(e => e.Subject1)
                    .HasColumnType("text")
                    .HasColumnName("Subject");*/
            });

            modelBuilder.Entity<Result>(entity =>
            {
                //entity.Property(e => e.SubmitTime).HasColumnType("double");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Results__SurveyI__36B12243");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Results__UserId__35BCFE0A");
            });

            modelBuilder.Entity<UserSeminar>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValueSql("('1')");

                entity.HasOne(d => d.Seminar)
                    .WithMany(p => p.UserSeminars)
                    .HasForeignKey(d => d.SeminarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserSeminars__SurveyI__36B12243");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSeminars)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserSeminars__UserId__35BCFE0A");
            });

            modelBuilder.Entity<Seminar>(entity =>
            {
                entity.Property(e => e.Author).HasMaxLength(255);

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(255);

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Seminars)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Seminars__Subject__36B57510");

            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Serminar)
                    .WithMany(p => p.Surveys)
                    .HasForeignKey(d => d.SerminarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Surveys__Sermina__2C3393D0");
            });

            modelBuilder.Entity<SurveyQuestion>(entity =>
            {
                entity.ToTable("Survey_Questions");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.SurveyQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Survey_Qu__Quest__3A81B327");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyQuestions)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Survey_Qu__Surve__398D8EEE");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Username);

                entity.Property(e => e.Email);

                entity.Property(e => e.Tel);

                entity.Property(e => e.Address).HasColumnType("text");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(255);

                entity.Property(e => e.LastName).HasMaxLength(255);

                entity.Property(e => e.NumberId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("('1')");

                entity.Property(e => e.Tel).HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserAnswer>(entity =>
            {
                entity.ToTable("User_Answers");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.UserAnswers)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("FK__User_Answ__Answe__3E52440B");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.UserAnswers)
                    .HasForeignKey(d => d.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_Answ__Surve__3D5E1FD2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAnswers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_Answ__UserI__3F466844");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
