using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.DataAccess.Domains
{
    public class SupportInformation : BaseEntity
    {
        public string Company { get; set; }
        public string Address { get; set; }
        public string CompanyTel { get; set; }
        public string Supporter { get; set; }
        public string SupporterTel { get; set; }
    }
}
