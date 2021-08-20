using EnvironmentSurvey.WebAPI.ClientSide.Common;
using EnvironmentSurvey.WebAPI.ClientSide.Models;
using EnvironmentSurvey.WebAPI.ClientSide.Models.Account;
using EnvironmentSurvey.WebAPI.DataAccess;
using EnvironmentSurvey.WebAPI.DataAccess.Domains;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace EnvironmentSurvey.WebAPI.BusinessLogic
{
    public interface IAccountService
    {
        //Task<string> Register(Dictionary<string, string> dict, string imagePath);
        Task<string> Register(RegisterModel model);
        Task<AuthendModel> Login(LoginModel model);
        //Task<Object> GetUserProfile();
        Task<string> changePassword(ChangePasswordModel model);
    }
    public class AccountService : IAccountService
    {
        private readonly ESContext _context;
        private IConfiguration _configuration;
        private readonly Regex regexPassword = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");

        public AccountService(IConfiguration config, ESContext context)
        {
            _configuration = config;
            _context = context;

        }

        private async Task<User> GetUser(string UserName, string Password)
        {
            var account = _context.Users.SingleOrDefault(u => u.Username.Equals(UserName) && !u.DeletedDate.HasValue);
            if (account == null || !BC.Verify(Password, account.Password))
                return await Task.FromResult<User>(null);
            else
                return await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(UserName));
        }

        //public async Task<string> Register(Dictionary<string,string> dict, string imagePath)
        //{
        //    User user = new User();
        //    foreach (string key in dict.Keys)
        //    {
        //        if (key.Equals("username")) { user.Username = dict[key]; continue; }
        //        if (key.Equals("userPass")) { user.Password = BC.HashPassword(dict[key]); continue; }
        //        if (key.Equals("idNumber")) { user.NumberId = dict[key]; continue; }
        //        if (key.Equals("userRole")) { user.Role = dict[key]; continue; }
        //        if (key.Equals("userLastname")) { user.LastName = dict[key]; continue; }
        //        if (key.Equals("userFirstname")) { user.FirstName = dict[key]; continue; }
        //        if (key.Equals("userEmail")) { user.Email = dict[key]; continue; }
        //        if (key.Equals("userTel")) { user.Tel = dict[key]; continue; }
        //        if (key.Equals("userAddress")) { user.Address = dict[key]; continue; }
        //        if (key.Equals("userGender")) { user.Gender = Int32.Parse(dict[key]); continue; }
        //    }
        //    user.Image = imagePath;
        //    user.Status = (int)Status.PENDING;
        //    user.CreatedDate = DateTime.UtcNow;

        //    /*var duplicate = _context.Users.FirstOrDefault(u => u.Username.Equals(user.Username));
        //    if (duplicate != null)
        //        return "Duplicate";*/

        //    try
        //    {
        //        _context.Users.Add(user);
        //        var result = await _context.SaveChangesAsync();
        //        //await _sendMailService.SendWelcomeEmailAsync(user.Email, "Verify Account", user.Username);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return "Success";
        //}

        public async Task<string> Register(RegisterModel model)
        {
            if ((!model.Role.Equals("STUDENT") && !model.Role.Equals("EMPLOYEE")) || !regexPassword.IsMatch(model.Password))
            {
                return "Failed";
            }
            try
            {
                User user = new User
                {
                    Username = model.Username,
                    Password = BC.HashPassword(model.Password),
                    NumberId = model.NumberId,
                    Role = model.Role,
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    Email = model.Email,
                    Tel = model.Tel,
                    Gender = model.Gender,
                    Image = "default-avatar.jpg",
                    Status = (int)Status.PENDING,
                    CreatedDate = DateTime.UtcNow,
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<AuthendModel> Login(LoginModel model)
        {
            if (model != null && model.Username != null && model.Password != null)
            {
                var user = await GetUser(model.Username, model.Password);
                if (user != null)
                {
                    if (user.Status != (int)Status.ACCEPTED)
                    {
                        return null;
                    }
                    else
                    {
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role),
                        }),
                            Expires = DateTime.UtcNow.AddDays(1),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                        var token = tokenHandler.WriteToken(securityToken);
                        var authenModel = new AuthendModel
                        {
                            Token = token,
                            Username = user.Username,
                            Role = user.Role,
                            UserId = user.Id
                        };
                        return authenModel;
                    }
                }
                else
                    return null;
            }
            return null;
        }

        public async Task<string> changePassword(ChangePasswordModel model)
        {
            if (!regexPassword.IsMatch(model.NewPassword))
                return "Failed";
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(model.Username));
            if (user != null)
            {
                string userPass = user.Password;
                if (BC.Verify(model.OldPassword, userPass))
                {
                    string passwordHash = BC.HashPassword(model.NewPassword);
                    user.Password = passwordHash;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                    return "Success";
                }
            }
            else
                return "User not found";
            return "Failed";
        }
    }
}
