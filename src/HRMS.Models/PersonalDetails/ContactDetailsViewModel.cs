using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ContactDetailsViewModel
    {
        [Required]
        [Display(Name = "Employee Id")]
        public int? EmployeeId { get; set; }

        public int? TravelId { get; set; }
        //public int EmployeeEmergencyContactId { get; set; }

        public List<EmergencyContactViewModel> EmergencyContactList { get; set; }

        public EmergencyContactViewModel EmergencyContactModel { get; set; }

        [Required]
        [Display(Name = "Personal Email ID")]
        [RegularExpression(@"^[a-zA-Z0-9_.]*@[a-zA-Z0-9_.]*\.([a-zA-Z]{2,4})$", ErrorMessage = "Please enter valid email id.")]
        [StringLength(200, ErrorMessage = "Email ID can not be greater that 200 characters.")]
        public string PersonalEmailId { get; set; }

        [Display(Name = "Alternate Email ID")]
        [RegularExpression(@"^[a-zA-Z0-9_.]*@[a-zA-Z0-9_.]*\.([a-zA-Z]{2,4})$", ErrorMessage = "Please enter valid email id.")]
        [StringLength(200, ErrorMessage = "Email ID can not be greater that 200 characters.")]
        public string AlternateEmailId { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        //[RegularExpression(@"^[0-9\\\-\[\]\""._^%$#!~@,&*()+={}|:;'<>/`? ]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [RegularExpression(@"^[0-9.\+\(\)\-\s]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [StringLength(100, ErrorMessage = "Mobile Number can not be greater than 100 characters.")]
        public string MobileNumber { get; set; }

        [Display(Name = "Alternate Contact Number")]
        //[RegularExpression(@"^[0-9\\\-\[\]\""._^%$#!~@,&*()+={}|:;'<>/`? ]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [RegularExpression(@"^[0-9.\+\(\)\-\s]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [StringLength(100, ErrorMessage = "Alternate Contact Number can not be greater that 100 characters.")]
        public string AlternateContactNumber { get; set; }

        [Required]
        [Display(Name = "Residence Number")]
        [RegularExpression(@"^[0-9.\+\(\)\-\s]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        //[RegularExpression(@"^[0-9\\\-\[\]\""._^%$#!~@,&*()+={}|:;'<>/`? ]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [StringLength(50, ErrorMessage = "Residence Number can not be greater that 50 characters.")]
        public string ResidenceNumber { get; set; }

        [Display(Name = "Residence Voip")]
        //[RegularExpression(@"^[0-9\\\-\[\]\""._^%$#!~@,&*()+={}|:;'<>/`? ]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [RegularExpression(@"^[0-9.\+\(\)\-\s]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [StringLength(50, ErrorMessage = "Residence Voip can not be greater that 50 characters.")]
        public string ResidenceVoip { get; set; }

        //[Required]
        //[Display(Name = "Emergency Contact Number")]
        //[RegularExpression("^([0-9]+)$", ErrorMessage = "Phone Number can not contain characters.")]
        //[StringLength(11, MinimumLength = 10, ErrorMessage = "Emergency Contact Number can not be lesser than 10 digits and can not be greater that 11 digits.")]
        //public string EmergencyContactNumber { get; set; }

        [Display(Name = "Skype ID")]
        //[StringLength(50, ErrorMessage = "Skype ID can not be greater that 50 characters.")]
        public string SkypeId { get; set; }

        public int EmpStatusMasterID { get; set; }

        [Display(Name = "YIM ID")]
        //[StringLength(50, ErrorMessage = "YIM ID can not be greater that 50 characters.")]
        public string YIMId { get; set; }

        [Required]
        [Display(Name = "Office Email ID")]
        [RegularExpression(@"^[a-zA-Z0-9_\+-]+(\.[a-zA-Z0-9_\+-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.([a-zA-Z]{2,4})$", ErrorMessage = "Please enter valid email id.")]
        [StringLength(500, ErrorMessage = "Office Email ID can not be greater that 500 characters.")]
        public string OfficeEmailId { get; set; }

        [Display(Name = "Office Extention")]
        //[RegularExpression(@"^[0-9\\\-\[\]\""._^%$#!~@,&*()+={}|:;'<>/`? ]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [RegularExpression(@"^[0-9.\+\(\)\-\s]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [StringLength(50, ErrorMessage = "Office Extention can not be greater that 50 characters.")]
        public string OfficeVoip { get; set; }

        [Display(Name = "Gtalk ID")]
        //[StringLength(50, ErrorMessage = "Gtalk ID can not be greater that 50 characters.")]
        public string GtalkId { get; set; }

        public string UserRole { get; set; }

        [Display(Name = "Seating Location")]
        [StringLength(50, ErrorMessage = "Seating Location can not be greater that 50 characters.")]
        public string SeatingLocation { get; set; }

        public int? uniqueID { get; set; }
    }
}