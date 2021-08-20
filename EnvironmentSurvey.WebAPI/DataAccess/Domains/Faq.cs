using EnvironmentSurvey.WebAPI.DataAccess.Extensions;

#nullable disable

namespace EnvironmentSurvey.WebAPI.DataAccess.Domains
{
    public partial class Faq : BaseEntity
    {
        public string Issue { get; set; }
        public string Solution { get; set; }
    }
}
