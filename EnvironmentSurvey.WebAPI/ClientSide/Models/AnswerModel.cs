using EnvironmentSurvey.WebAPI.ClientSide.Common;
using System.Collections.Generic;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class AnswerModel
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public IsCorrect? IsCorrect { get; set; }
        public int QuestionId { get; set; }
        public List<UserAnswerModel> UserAnswers { get; set; }
    }
}
