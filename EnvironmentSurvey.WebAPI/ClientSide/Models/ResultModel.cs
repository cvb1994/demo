using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class ResultModel
    {
        public int Id { get; set; }
        public int surveyId { get; set; }
        public string surveyName { get; set; }
        public int point { get; set; }
        public double SubmitTime { get; set; }
        public int UserId { get; set; }
        public int Ranked { get; set; }
        public string FullName { get; set; }
        public string NameSeminar { get; set; }

    }
}
