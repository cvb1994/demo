using EnvironmentSurvey.WebAPI.BusinessLogic;
using EnvironmentSurvey.WebAPI.ClientSide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly IReportSevice _reportSevice;
        public ReportController(IReportSevice reportSevice)
        {
            _reportSevice = reportSevice;
        }
        [HttpGet]
        [Route("getDataReport")]
        [Authorize(Roles = "ADMIN")]
        public async Task<DashboardModel> getDataReport()
        {
            return await _reportSevice.DataDashboard();
        }

    }
}
