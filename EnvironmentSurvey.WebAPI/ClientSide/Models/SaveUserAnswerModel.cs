using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class SaveUserAnswerModel
    {
        public List<UserAnswerModel> ListUserAnserModel { get; set; }
        public double SubmitTime { get; set;}
    }
}
