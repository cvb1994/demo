using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System;
using System.Collections.Generic;

#nullable disable

namespace EnvironmentSurvey.WebAPI.DataAccess.Domains
{
    public partial class Seminar : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        public string Author { get; set; }
        public int SubjectId { get; set; }
        public int forUser { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual ICollection<Survey> Surveys { get; set; }
        public virtual ICollection<UserSeminar> UserSeminars { get; set; }
    }
}
