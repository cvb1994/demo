using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System;

#nullable disable

namespace EnvironmentSurvey.WebAPI.DataAccess.Domains
{
    public partial class Result : BaseEntity
    {
        public int Point { get; set; }
        public double SubmitTime { get; set; }
        public int UserId { get; set; }
        public int SurveyId { get; set; }

        public virtual Survey Survey { get; set; }
        public virtual User User { get; set; }
    }
}
