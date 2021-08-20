using EnvironmentSurvey.WebAPI.ClientSide.Common;
using System;
using System.Collections.Generic;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class SurveyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public SurveyStatus Status { get; set; }
        public int SeminarId { get; set; }
        public string Description { get; set; }
        public int NumberOfQuestion { get; set; }
        public List<ResultModel> Results { get; set; }
        public List<SurveyQuestionModel> SurveyQuestions { get; set; }
    }
}
