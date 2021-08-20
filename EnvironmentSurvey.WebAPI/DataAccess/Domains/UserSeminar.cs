using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System;

namespace EnvironmentSurvey.WebAPI.DataAccess.Domains
{
    public class UserSeminar : BaseEntity
    {
        public int UserId { get; set; }
        public int SeminarId { get; set; }
        public int Status { get; set; }

        public virtual User User { get; set; }
        public virtual Seminar Seminar { get; set; }
    }
}
