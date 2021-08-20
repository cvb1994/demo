using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.DataAccess.Domains
{
    public class Subject : BaseEntity
    {
        public string Subject1 { get; set; }

        public virtual ICollection<Seminar> Seminars { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
