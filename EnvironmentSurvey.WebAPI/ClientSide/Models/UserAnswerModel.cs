using EnvironmentSurvey.WebAPI.ClientSide.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class UserAnswerModel
    {
        public int Id { get; set; }
        public int SurveyQuestionId { get; set; }
        public int? AnswerId { get; set; }
        public int UserId { get; set; }

    }
}
