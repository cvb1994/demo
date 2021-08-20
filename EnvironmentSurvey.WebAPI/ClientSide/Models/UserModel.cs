using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string NumberId { get; set; }
        public string Role { get; set; }
        public string Image { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public int? Gender { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class ChangeAvatarModel
    {
        public int ID { get; set; }
        public IFormFile File { get; set; }
    }
}
