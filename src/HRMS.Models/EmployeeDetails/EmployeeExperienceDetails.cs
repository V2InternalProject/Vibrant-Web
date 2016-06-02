using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class EmployeeExperienceDetails
    {
        public int? EmployeeId { get; set; }

        public int V2ExperienceYears { get; set; }

        public int V2ExperienceMonths { get; set; }

        public int PastExperienceYears { get; set; }

        public int PastExperienceMonths { get; set; }

        [Display(Name = "Relevant Experience Years")]
        [Range(0, 99)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Please enter valid years")]
        public int RelevantExperienceYears { get; set; }

        [Display(Name = "Relevant Experience Months")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Please enter valid months")]
        [Range(0, 11)]
        public int RelevantExperienceMonths { get; set; }

        public int TotalExperienceYears { get; set; }

        public int TotalExperienceMonths { get; set; }

        public PastEmployeeExperienceViewModel PastExperienceDetails { get; set; }

        //public GapEmployeeExperienceViewModel GapExperienceDetails { get; set; }

        public string UserRole { get; set; }

        public int EmployeeGapExpId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Reason can not be greater than 500 characters.")]
        public string Reason { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        public string GapDuration { get; set; }

        [StringLength(500, ErrorMessage = "Description can not be greater than 500 characters.")]
        public string Description { get; set; }

        public int EmpStatusMasterID { get; set; }
    }
}