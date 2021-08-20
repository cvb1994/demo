using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System;
using System.Collections.Generic;

#nullable disable

namespace EnvironmentSurvey.WebAPI.DataAccess.Domains
{
    public partial class Survey : BaseEntity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
        public int SerminarId { get; set; }
        public string  Description { get; set; }
        public virtual Seminar Serminar { get; set; }
        public virtual ICollection<Result> Results { get; set; }
        public virtual ICollection<SurveyQuestion> SurveyQuestions { get; set; }
    }
}
