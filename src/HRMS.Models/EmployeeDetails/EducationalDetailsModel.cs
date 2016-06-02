using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class EducationalDetailsModel
    {
        public int EmployeeId { get; set; }

        [Required]
        public string Qualification { get; set; }

        [Required]
        public string Degree { get; set; }

        [Required]
        public string Specialization { get; set; }

        [Required]
        public string Institute { get; set; }

        [Required]
        public string University { get; set; }

        //[Required]
        //public string Course { get; set; }
        [Required]
        public string Year { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Percentage { get; set; }
    }
}