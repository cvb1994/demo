using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class DashboardModel
    {
        public int TotalSeminars { get; set; }
        public int TotalSurveys { get; set; }
        public int TotalUsers { get; set; }
        public int TotalNewUsers { get; set; }
        public int TotalRequestSeminars { get; set; }
        public int TotalNewRequestSeminars { get; set; }
        public string  Top1Seminar { get; set; }
        public int Top1SeminarCount { get; set; }
        public string Top1SeminarPerDay { get; set; }
        public int Top1SeminarPerDayCount { get; set; }
        public int TotalAwards { get; set; }

    }
}
