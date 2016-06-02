using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class EmployeeDisciplinaryDetailsViewModel
    {
        public int DisciplineId { get; set; }

        public int EmployeeId { get; set; }

        public int ManagerId { get; set; }

        public string CreatedByUserName { get; set; }

        public string CreatedByUserId { get; set; }

        public string LoginUserId { get; set; }

        [Required]
        public DateTime? AddedDate { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [Display(Name = "Subject")]
        [StringLength(50, ErrorMessage = "Maxium 50 characters are allowed")]
        public string DisciplineSubject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Maxium 500 characters are allowed")]
        [Display(Name = "Message")]
        public string DisciplineMessage { get; set; }

        [Required(ErrorMessage = "Manager is required")]
        // [RegularExpression("^[a-zA-Z .]+$", ErrorMessage = "Manager can not contain numbers or special characters.")]
        public string Manager { get; set; }

        public string UserRole { get; set; }

        public int EmpStatusMasterID { get; set; }

        public List<EmployeeListDetails> EmployeeManagerList { get; set; }
    }

    //public class EmployeeManagerList
    //{
    //   public int ManagerId { get; set; }
    //   public string ManagerName { get; set; }
    //}
}