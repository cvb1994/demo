#nullable disable

using EnvironmentSurvey.WebAPI.DataAccess.Extensions;

namespace EnvironmentSurvey.WebAPI.DataAccess.Domains
{
    public partial class UserAnswer : BaseEntity
    {
        public int SurveyQuestionId { get; set; }
        public int? AnswerId { get; set; }
        public int UserId { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual SurveyQuestion SurveyQuestion { get; set; }
        public virtual User User { get; set; }
    }
}
