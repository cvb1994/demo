using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class SubjectModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public List<SurveyModel> Surveys { get; set; }
        public List<SeminarModel> Seminars { get; set; }
    }
}
