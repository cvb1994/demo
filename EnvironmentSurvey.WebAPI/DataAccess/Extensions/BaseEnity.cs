using System;
using System.ComponentModel.DataAnnotations;

namespace EnvironmentSurvey.WebAPI.DataAccess.Extensions
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
