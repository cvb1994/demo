using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System.Collections.Generic;

#nullable disable

namespace EnvironmentSurvey.WebAPI.DataAccess.Domains
{
    public partial class Question : BaseEntity
    {
        public string Question1 { get; set; }
        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<SurveyQuestion> SurveyQuestions { get; set; }
    }
}
