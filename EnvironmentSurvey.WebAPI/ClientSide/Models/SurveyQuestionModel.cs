using System.Collections.Generic;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class SurveyQuestionModel
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int QuestionId { get; set; }
        public List<UserAnswerModel> UserAnswers { get; set; }
    }
}
