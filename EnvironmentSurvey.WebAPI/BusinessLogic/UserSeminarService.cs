using EnvironmentSurvey.WebAPI.ClientSide.Common;
using EnvironmentSurvey.WebAPI.ClientSide.Models;
using EnvironmentSurvey.WebAPI.DataAccess;
using EnvironmentSurvey.WebAPI.DataAccess.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.BusinessLogic
{
    public interface IUserSeminarService
    {
        Task<string> SeminarRegistration(UserSeminarModel model);
        Task<List<ResUserSemiModel>> getUserSeminarByUser(UserSeminarModel model);

        Task<ResponsePagedModel> getAllSeminarPending(SearchModel model, PaginationClientModel paginationClientModel);

        Task<string> changeUserSeminarStatus(int option, int userSeminarId);
    }
    public class UserSeminarService : IUserSeminarService
    {
        private readonly ESContext _context;
        private readonly ISeminarService _seminarService;
        private readonly ISendMailService _sendMailService;

        public UserSeminarService(ESContext context, ISeminarService seminarService, ISendMailService sendMailService)
        {
            _context = context;
            _seminarService = seminarService;
            _sendMailService = sendMailService;
        }

        public async Task<string> changeUserSeminarStatus(int option, int userSeminarId)
        {
            try
            {
                UserSeminar userSerminar = await _context.UserSeminars.FindAsync(userSeminarId);
                User user = await _context.Users.FindAsync(userSerminar.UserId);
                Seminar seminar = await _context.Seminars.FindAsync(userSerminar.SeminarId);
                var subject = "Confirm Register Seminar";
                var message = "";
                if (option == 1)
                {                  
                    userSerminar.Status = 1;
                    message = "Thank you for your interest in the seminars at The Educenter. Your seminar registration <b>" + seminar.Name + "</b> has been accepted. <a href='http://127.0.0.1:5500/Client/my_seminar.html'>Click here</a>  to see detail. Thanks you !";
                } else
                {
                    userSerminar.Status = 3;
                    message = "Thank you for your interest in the seminars at The Educenter. Your seminar registration <b>" + seminar.Name + "</b> has been declined because you are not yet eligible to participate in this seminar. <a href='http://127.0.0.1:5500/Client/fags.html'>Click here</a>  to see detail. Thanks you !";
                }
                await _context.SaveChangesAsync();
                await _sendMailService.SendEmailConfirm(user.Email, subject, user.Username, message);
                return "success";
            } catch (Exception ex)
            {
                return "failed";
            }
        }

        public async Task<ResponsePagedModel> getAllSeminarPending(SearchModel model, PaginationClientModel paginationClientModel)
        {
            var query = _context.UserSeminars.Where(s => s.Status == 2).OrderByDescending(s=> s.CreatedDate);
            if(model.Search_key == "")
            {

            }
            /*if(model.FromDate != "")
            {
                DateTime fromDate = Convert.ToDateTime(model.FromDate);
                query = (IOrderedQueryable<UserSeminar>)query.Where(s => s.CreatedDate >= fromDate);
            }
            if(model.ToDate != "")
            {
                DateTime toDate = Convert.ToDateTime(model.ToDate);
                query = (IOrderedQueryable<UserSeminar>)query.Where(s => s.CreatedDate <= toDate);
            }*/
            if(model.Search_key != "")
            {
                var key = model.Search_key;
                query = (IOrderedQueryable<UserSeminar>)query.Where(s => s.Seminar.Name.Contains(key) || s.User.FirstName.Contains(key) || s.User.LastName.Contains(key));
            }
            var listUserSeminar = await query.ToListAsync();
            int totalPage = (int)Math.Ceiling(listUserSeminar.Count() / (double)paginationClientModel.PageSize);

            var list  = await query
                            .Skip((paginationClientModel.PageNumber - 1) * paginationClientModel.PageSize)
                            .Take(paginationClientModel.PageSize)
                            .ToListAsync();
            List<ResUserSemiModel> returnList = new List<ResUserSemiModel>();
            foreach (var userSeminar in list)
            {
                User user = _context.Users.Find(userSeminar.UserId);
                Seminar seminar = _context.Seminars.Find(userSeminar.SeminarId);
                var obj = new ResUserSemiModel
                {
                    UserSeminarId = userSeminar.Id,
                    UserName = user.Username,
                    UserNumberId = userSeminar.User.NumberId,
                    seminarName = seminar.Name,
                    FullName =  user.FirstName + " " + user.LastName
                };
                returnList.Add(obj);
            }
            var responsePagedModel = new ResponsePagedModel
            {
                ListUserSeminar = returnList.ToList(),
                PageNumber = paginationClientModel.PageNumber,
                TotalPage = totalPage
            };
            return responsePagedModel;
        }

        public async Task<List<ResUserSemiModel>> getUserSeminarByUser(UserSeminarModel model)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(model.Username));
            if(user != null)
            {
                var listUserSemnar = await _context.UserSeminars.Where(us => us.UserId.Equals(user.Id)).ToListAsync();
                List<ResUserSemiModel> lsUserSeminarModal = new List<ResUserSemiModel>();
                foreach(var US in listUserSemnar)
                {
                    var user_seminar = new ResUserSemiModel
                    {
                        UserId = US.UserId,
                        SeminarId = US.SeminarId,
                        Seminar = await _seminarService.GetByID(US.SeminarId),
                        Status = US.Status
                    };
                    lsUserSeminarModal.Add(user_seminar);
                }
                return lsUserSeminarModal.ToList();
            }
            else
            {
                return null;
            }
        }

        public async Task<string> SeminarRegistration(UserSeminarModel model)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(model.Username));
            Seminar seminar = await _context.Seminars.FindAsync(model.SeminarId);
            if (user != null && seminar != null )
            {
                var userSeminar = new UserSeminar
                {
                    UserId = user.Id,
                    User = user,
                    SeminarId = model.SeminarId,
                    Seminar = seminar,
                    Status = (int)Status.PENDING,
                    CreatedDate = DateTime.UtcNow
                };
                try
                {
                    _context.UserSeminars.Add(userSeminar);
                    await _context.SaveChangesAsync();
                    return "Register success";
                }
                catch (Exception ex)
                {
                    return "Register error";
                }
               
            }
            else 
            {
                return "User not found";
            }
            
        }
    }
}
