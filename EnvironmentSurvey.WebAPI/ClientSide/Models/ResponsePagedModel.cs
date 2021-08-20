using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class ResponsePagedModel
    {
        public List<SeminarModel> ListData { get; set; }
        public int PageNumber { get; set; }
        public int TotalPage { get; set; }
        public List<UserModel> ListUser { get; set; }
        public List<ResUserSemiModel> ListUserSeminar { get; set; }
        public List<ResultModel>  listResult { get; set; }
    }
}
