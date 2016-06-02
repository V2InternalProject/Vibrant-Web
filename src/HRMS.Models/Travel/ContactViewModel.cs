using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ContactViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<TravelEmergencyContactViewModel> TravelEmergencyContactList { get; set; }
        public TravelEmergencyContactViewModel TravelEmergencyContactModel { get; set; }

        [Required]
        [Display(Name = "Travel Id")]
        public int? TravelId { get; set; }

        [Required]
        [Display(Name = "Personal Email ID")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)(((\.(\w){2,2})+(\.(\w){2,3}))|((\.(\w){3,3})+))$", ErrorMessage = "Please enter valid email id.")]
        [StringLength(200, ErrorMessage = "Email ID can not be greater that 200 characters.")]
        public string PersonalEmailId { get; set; }

        [Required]
        [Display(Name = "Personal Email ID")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)(((\.(\w){2,2})+(\.(\w){2,3}))|((\.(\w){3,3})+))$", ErrorMessage = "Please enter valid email id.")]
        [StringLength(200, ErrorMessage = "Email ID can not be greater that 200 characters.")]
        public string userPersonalEmailId { get; set; }

        //[Required]
        [Display(Name = "Contact No India")]
        [RegularExpression(@"^[0-9.\+\(\)\-\s]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [StringLength(100, ErrorMessage = "Contact Number India can not be greater than 100 characters.")]
        public string ContactNoIndia { get; set; }

        [Required]
        [Display(Name = "Contact No Abroad")]
        [RegularExpression(@"^[0-9.\+\(\)\-\s]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [StringLength(100, ErrorMessage = "Contact Number Abroad can not be greater than 100 characters.")]
        public string ContactNoAbroad { get; set; }

        public string UserRole { get; set; }

        public int StageID { get; set; }

        public int UniqueID { get; set; }
    }
}