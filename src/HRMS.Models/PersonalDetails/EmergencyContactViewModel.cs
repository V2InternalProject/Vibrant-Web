using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class EmergencyContactViewModel
    {
        [Required]
        [Display(Name = "Employee Id")]
        public int? EmployeeId { get; set; }

        public string UserRole { get; set; }

        public int TravelID { get; set; }

        [Display(Name = "Employee Emergency Contact Id")]
        public int EmployeeEmergencyContactId { get; set; }

        [Required]
        [Display(Name = "Name")]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only")]
        [StringLength(1000, ErrorMessage = "Name can not be greater that 1000 characters.")]
        public string Name { get; set; }

        [Display(Name = "Address")]
        [StringLength(4000, ErrorMessage = "Name can not be greater that 4000 characters.")]
        public string EmgAddress { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
        //[RegularExpression(@"^[0-9\\\-\[\]\""._^%$#!~@,&*()+={}|:;'<>/`? ]+$", ErrorMessage = "Contact Number can not contain alphabets.")]
        [RegularExpression(@"^[0-9.\+\(\)\-\s]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [StringLength(100, ErrorMessage = "Contact Number can not be greater that 100 characters.")]
        public string ContactNo { get; set; }

        [Display(Name = "Email ID")]
        [RegularExpression(@"^[a-zA-Z0-9_.]*@[a-zA-Z0-9_.]*\.([a-zA-Z]{2,4})$", ErrorMessage = "Please enter valid email id.")]
        //[RegularExpression(@"^[a-zA-Z0-9_\+-]+(\.[a-zA-Z0-9_\+-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.([a-zA-Z]{2,4})$", ErrorMessage = "Please enter valid email id.")]
        [StringLength(50, ErrorMessage = "Email ID can not be greater that 50 characters.")]
        public string EmailId { get; set; }

        [Required]
        public int? uniqueID { get; set; }

        [Required]
        public string Relation { get; set; }

        public string ConcateReportField { get; set; }

        public bool IsAddedFromVB { get; set; }
    }
}