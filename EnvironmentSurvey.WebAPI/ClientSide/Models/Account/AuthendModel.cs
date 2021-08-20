using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models.Account
{
    public class AuthendModel
    {
        public string Role { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
    }
}
