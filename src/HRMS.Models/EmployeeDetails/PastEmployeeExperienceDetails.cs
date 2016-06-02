using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class PastEmployeeExperienceDetails
    {
        public int? EmployeeHistoryId { get; set; }

        public int? EmployeeId { get; set; }

        [Required]
        [Display(Name = "Organization Name")]
        [StringLength(100, ErrorMessage = "Organization Name can not be greater than 100 characters.")]
        public string OrganizationName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Location can not be greater than 100 characters.")]
        public string Location { get; set; }

        [Required]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? WorkedFrom { get; set; }

        [Required]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date)]
        public DateTime? WorkedTill { get; set; }

        [Required]
        //[RegularExpression("^([0-9]+)$", ErrorMessage = "Employee Type Id can not contain characters.")]
        public int EmployeeTypeId { get; set; }

        [Required]
        [Display(Name = "Last Designation")]
        [StringLength(100, ErrorMessage = "Last Designation can not be greater than 100 characters.")]
        public string LastDesignation { get; set; }

        //[Required]
        [Display(Name = "Reporting Manager")]
        [StringLength(100, ErrorMessage = "Reporting Manager can not be greater than 100 characters.")]
        public string ReportingManager { get; set; }

        //[Required]
        [Display(Name = "Last Salary Drawn (lacs per annum)")]
        [StringLength(50, ErrorMessage = "Last Salary Drawn can not be greater than 50 characters.")]
        public string LastSalaryDrawn { get; set; }

        public string EmployeeWorkingType { get; set; }

        public List<EmployeeWorkingType> EmployeeWorkingTypeList { get; set; }

        public bool IsEdit { get; set; }

        public string UserRole { get; set; }
    }
}