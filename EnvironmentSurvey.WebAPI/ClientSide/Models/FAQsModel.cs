using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class FAQsModel
    {
        public int ID { get; set; }
        public string Issue { get; set; }
        public string Solution { get; set; }
    }
}
