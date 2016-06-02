using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ResidentialDetailsViewModel
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Current address is required")]
        [Display(Name = "Current Address")]
        [StringLength(255, ErrorMessage = "Current address can not be more than 255 characters.")]
        public string CurrentAddress { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        [StringLength(255, ErrorMessage = "Address can not be more than 255 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Current Zip Code is required")]
        [StringLength(30, ErrorMessage = "Current Zip Code can not be more than 30 characters.")]
        //[RegularExpression(@"^\d+$", ErrorMessage = "Please enter only numbers")]
        [Display(Name = "Current Zip Code")]
        public string CurrentZipCode { get; set; }

        [Required(ErrorMessage = "Zip Code is required")]
        [StringLength(30, ErrorMessage = "Permanent Zip Code can not be more than 30 characters.")]
        //[RegularExpression(@"^\d+$", ErrorMessage = "Please enter only numbers")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Current City is required")]
        [Display(Name = "Current City")]
        [StringLength(255, ErrorMessage = "Current City can not be more than 255 characters.")]
        //[RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        public string CurrentCity { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        [StringLength(255, ErrorMessage = "City can not be more than 255 characters.")]
        //[RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        public string City { get; set; }

        [Required(ErrorMessage = "Current State is required")]
        [Display(Name = "Current State")]
        [StringLength(255, ErrorMessage = "Current State can not be more than 255 characters.")]
        //[RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        public string CurrentState { get; set; }

        [Required(ErrorMessage = "State is required")]
        [Display(Name = "State")]
        [StringLength(255, ErrorMessage = "State can not be more than 255 characters.")]
        //[RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        public string State { get; set; }

        public string UserRole { get; set; }

        [Required(ErrorMessage = "Please select Country")]
        public string country { get; set; }

        public int? countryId { get; set; }

        public int SearchedUserID { get; set; }

        public int? CurrentcountryId { get; set; }

        [Required(ErrorMessage = "Please select Country")]
        public string CurrentCountry { get; set; }

        public List<CountryDetails> CountryList { get; set; }

        public List<CountryDetails> CurrentCountryList { get; set; }

        public List<CityDetails> CityList { get; set; }

        public List<CityDetails> CurrentCityList { get; set; }

        public int EmpStatusMasterID { get; set; }

        public string lblCurrentAddress { get; set; }

        public string lblCurrentCountry { get; set; }

        public string lblCurrentState { get; set; }

        public string lblCurrentCity { get; set; }

        public string lblCurrentZipCode { get; set; }

        public string lblAddress { get; set; }

        public string lblCountry { get; set; }

        public string lblState { get; set; }

        public string lblCity { get; set; }

        public string lblZipCode { get; set; }
    }
}