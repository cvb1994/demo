using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class SearchModel
    {
        public string Search_key { get; set; }
        public string Username { get; set; }
        public int Status { get; set; }
        public string Role { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
