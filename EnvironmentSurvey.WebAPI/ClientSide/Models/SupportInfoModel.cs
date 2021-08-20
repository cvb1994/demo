using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class SupportInfoModel
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string CompanyTel { get; set; }
        public string Supporter { get; set; }
        public string SupporterTel { get; set; }
    }
}
