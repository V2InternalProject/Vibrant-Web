using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class DesignationDetailsViewModel
    {
        [Display(Name = "Employee Id")]
        public int? EmployeeId { get; set; }

        [Display(Name = "Designation Details")]
        public List<DesignationDetails> DesignationDetailsList { get; set; }

        [Display(Name = "New Designation")]
        public DesignationDetails NewDesignation { get; set; }

        public string UserRole { get; set; }

        public int EmpStatusMasterID { get; set; }

        public int? Year { get; set; }

        [Required]
        [StringLength(9, ErrorMessage = "Month can not be greater than 9 digits")]
        public string Month { get; set; }

        public string Grade { get; set; }

        public int? GradeId { get; set; }

        public bool? isDefaultRecord { get; set; }

        public string Level { get; set; }

        public int? UniqueId { get; set; }
    }
}