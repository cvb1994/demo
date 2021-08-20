using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System.Collections.Generic;

#nullable disable

namespace EnvironmentSurvey.WebAPI.DataAccess.Domains
{
    public partial class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string NumberId { get; set; }
        public string Role { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public int? Gender { get; set; }
        public int? Status { get; set; }
        public string Image { get; set; }

        public virtual ICollection<Result> Results { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
        public virtual ICollection<UserSeminar> UserSeminars { get; set; }
    }
}
