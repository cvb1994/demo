using EnvironmentSurvey.WebAPI.ClientSide.Models;
using EnvironmentSurvey.WebAPI.DataAccess;
using EnvironmentSurvey.WebAPI.DataAccess.Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.BusinessLogic
{
    public interface IReportSevice
    {
        public Task<DashboardModel> DataDashboard();
    }

    public class ReportService : IReportSevice
    {
        private readonly ESContext _context;
        public ReportService(ESContext context)
        {
            _context = context;

        }
        public async Task<DashboardModel> DataDashboard()
        {
            DateTime dt = DateTime.Now;
            var seminars = await _context.Seminars.ToListAsync();
            var totalseminars = seminars.Count();
            var users = await _context.Users.Where(u=> u.Status.Equals(1)).ToListAsync();
            var totalUser = users.Count();
            var newUsers = await _context.Users.Where(u => u.Status.Equals(1) && u.CreatedDate.Year == dt.Year && u.CreatedDate.Month == dt.Month && u.CreatedDate.Day == dt.Day)
                            .ToListAsync();
            var totalNewUser = newUsers.Count();
            var surveys = await _context.Surveys.ToListAsync();
            var totalSurvey = surveys.Count();
            var requestSeminars = await _context.UserSeminars.ToListAsync();
            var totalRquestSeminars = requestSeminars.Count();
            var newRquestSeminars = await _context.UserSeminars.Where(u => u.Status.Equals(1) && u.CreatedDate.Year == dt.Year && u.CreatedDate.Month == dt.Month && u.CreatedDate.Day == dt.Day)
                                  .ToListAsync();
            var totalNewRequestSemianr = newRquestSeminars.Count();
            var top1Senimar = await _context.UserSeminars.GroupBy(s => s.SeminarId).OrderByDescending(g => g.Count()).Select(g => new {SeminarId = g.Key, count = g.Count() }).FirstOrDefaultAsync();
            Seminar seminar = new Seminar();
            if(top1Senimar != null)
            {
                seminar = await _context.Seminars.Where(s => s.Id.Equals(top1Senimar.SeminarId)).FirstAsync();
            }
            
            var top1SenimarPerDay = await _context.UserSeminars
                .Where(u=>u.CreatedDate.Year == dt.Year && u.CreatedDate.Month == dt.Month && u.CreatedDate.Day == dt.Day)
                .GroupBy(s => s.SeminarId).OrderByDescending(g => g.Count()).Select(g => new { SeminarId = g.Key, count = g.Count() }).FirstOrDefaultAsync();
            Seminar seminarPerDay = new Seminar();
            if (top1SenimarPerDay != null)
            {
                seminarPerDay = await _context.Seminars.Where(s => s.Id.Equals(top1SenimarPerDay.SeminarId)).FirstAsync();
            }
            
            var responseModel = new DashboardModel
            {
                TotalSeminars = totalseminars,
                TotalSurveys = totalSurvey,
                TotalUsers = totalUser,
                TotalNewUsers = totalNewUser,
                TotalRequestSeminars = totalRquestSeminars,
                TotalNewRequestSeminars = totalNewRequestSemianr,
                Top1Seminar = seminar.Name != null ? seminar.Name : "",
                Top1SeminarCount = top1Senimar!= null ? top1Senimar.count : 0,
                Top1SeminarPerDay = seminarPerDay.Name != null ? seminarPerDay.Name : "",
                Top1SeminarPerDayCount = top1SenimarPerDay != null ? top1SenimarPerDay.count : 0
            };
            return responseModel;
        }
    }
}
