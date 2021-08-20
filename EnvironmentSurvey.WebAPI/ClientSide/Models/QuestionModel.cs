using System.Collections.Generic;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public int SubjectId { get; set; }
        public List<AnswerModel> Answers { get; set; }
        public List<SurveyQuestionModel> SurveyQuestions { get; set; }
        public int? SurveyQuestionId { get; set; }
    }
}
