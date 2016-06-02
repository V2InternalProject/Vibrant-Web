using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ConfirmationProcess
    {
        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "User Id")]
        public int? UserId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Post Id")]
        public int? PostID { get; set; }

        public bool Checked { get; set; }

        public string Address { get; set; }

        public int ApprovalsCount { get; set; }

        public string LoggedInUserRole { get; set; }

        public List<ConfigurationDetailsViewModel> ConfigurationDetailsList { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }
    }
}