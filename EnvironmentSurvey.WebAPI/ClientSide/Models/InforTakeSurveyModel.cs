using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class InforTakeSurveyModel
    {
        public int SurveyId { get; set; }
        public string SeminarName { get; set; }
        public string SurveyName { get; set; }
        public int TotalRegister { get; set; }
        public int TotalTakeSurvey { get; set; }
    }
}
