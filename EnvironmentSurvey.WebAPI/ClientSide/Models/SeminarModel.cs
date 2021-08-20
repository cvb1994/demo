using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.ClientSide.Models
{
    public class SeminarModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        public string Author { get; set; }
        public int SubjectId { get; set; }
        public int forUser { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public IFormFile File { get; set; }
        public SubjectModel Subject { get; set; }


    }
}
