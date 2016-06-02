using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class BondDetailsViewModel
    {
        [Display(Name = "Bond Id")]
        public int BondId { get; set; }

        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }

        [Required]
        [Display(Name = "Bond Status")]
        public string BondStatus { get; set; }

        public string BondStatusHidden { get; set; }

        [Required]
        [Display(Name = "Bond Type")]
        public string BondType { get; set; }

        //[Display(Name = "Bond Status")]
        [Required]
        public int BondTypeID { get; set; }

        [Required]
        [Display(Name = "Bond Amount")]
        [StringLength(50, ErrorMessage = "Bond Amount can not be greater than 50 characters.")]
        public string BondAmount { get; set; }

        [Required]
        [Display(Name = "Bond End Date")]
        public DateTime? BondOverDate { get; set; }

        public Dictionary<string, string> BondStatusList { get; set; }

        public string UserRole { get; set; }

        public int EmpStatusMasterID { get; set; }
    }
}