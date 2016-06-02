using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ApprovalEmployeeDetails
    {
        [Display(Name = "Employee Id")]
        public int? EmployeeId { get; set; }

        public string EncryptedEmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "User Id")]
        public int? UserId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Post Id")]
        public int? PostID { get; set; }

        public string Address { get; set; }

        public string LoggedInUserRole { get; set; }

        public string Module { get; set; }

        public string CreatedBy { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }
    }
}