using EnvironmentSurvey.WebAPI.DataAccess.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class ResUserSemiModel
    {
        public int UserSeminarId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserNumberId { get; set; }
        public int SeminarId { get; set; }
        public string seminarName { get; set; }
        public int Status { get; set; }
        public virtual SeminarModel Seminar { get; set; }
        public string FullName { get; set; }
    }
}
