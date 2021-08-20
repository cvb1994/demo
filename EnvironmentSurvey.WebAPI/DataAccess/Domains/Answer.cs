using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System.Collections.Generic;

#nullable disable

namespace EnvironmentSurvey.WebAPI.DataAccess.Domains
{
    public partial class Answer : BaseEntity
    {
        public string Answer1 { get; set; }
        public int IsCorrect { get; set; }
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
