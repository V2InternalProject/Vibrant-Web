using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class MedicalHistoryDetails
    {
        public int? EmployeeId { get; set; }

        [Display(Name = "Medical Description Id")]
        public int MedicalDescId { get; set; }

        [Required]
        [Display(Name = "Medical Description")]
        [StringLength(500, ErrorMessage = "Medical Description can not be more than 500 characters.")]
        public string MedicalDescription { get; set; }

        [Required]
        //[Range(typeof(string), "1900", "2099")]
        public string Year { get; set; }
    }

    public class BloodGroupModel
    {
        [Required]
        public int BloodGroupId { get; set; }

        [Required]
        public string BloodGroupName { get; set; }
    }
}