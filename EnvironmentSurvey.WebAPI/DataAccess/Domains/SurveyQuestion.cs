using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System.Collections.Generic;

#nullable disable

namespace EnvironmentSurvey.WebAPI.DataAccess.Domains
{
    public partial class SurveyQuestion : BaseEntity
    {
        public int SurveyId { get; set; }
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
