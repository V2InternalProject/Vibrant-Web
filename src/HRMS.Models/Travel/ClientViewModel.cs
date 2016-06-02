using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ClientViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public ClientViewModel ClientDetails { get; set; }

        public List<ClientViewModel> ClientDetailsList { get; set; }

        public int ClientDetailsEmployeeId { get; set; }

        public int ClientId { get; set; }

        public int? ClientNameId { get; set; }

        public int TravelId { get; set; }

        public List<TravellingLocationList> TravellingLocationList { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Maxium 1000 characters are allowed")]
        public string ClientName { get; set; }

        public int? ProjectNameId { get; set; }

        public string ProjectName { get; set; }

        public List<ProjectNameList> ProjectNameList { get; set; }

        //[Required]
        //[StringLength(1000, ErrorMessage = "Maxium 1000 characters are allowed")]
        //public string Compony { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Maxium 1000 characters are allowed")]
        public string ClientContact { get; set; }

        [Required]
        [RegularExpression(@"^[0-9.\+\(\)\-\s]+$", ErrorMessage = "Client Contact Number can not contain alphabets.")]
        [StringLength(1000, ErrorMessage = "Client Contact Number India can not be greater than 1000 characters.")]
        public string ClientContactNumber { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Maxium 1000 characters are allowed")]
        public string ClientAddress { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)(((\.(\w){2,2})+(\.(\w){2,3}))|((\.(\w){3,3})+))$", ErrorMessage = "Please enter valid email id.")]
        [StringLength(200, ErrorMessage = "Email ID can not be greater that 200 characters.")]
        public string ClientEmailId { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Purpose of visit can not be greater that 1000 characters.")]
        public string PurposeOfVisit { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Maxium 1000 characters are allowed")]
        public string AdditionalInfo { get; set; }

        [Required(ErrorMessage = "Prospect Name is required")]
        public string ProspectName { get; set; }

        [Required(ErrorMessage = "Travel Location is required")]
        public string TravellingLocName { get; set; }

        [Required]
        public int? TravellingLocId { get; set; }

        public string ClientInviteLetterName { get; set; }

        public string ClientIviteLetterFilePath { get; set; }

        public int selectedLocationID { get; set; }

        public int selectedClientID { get; set; }

        public string TravellingLocNameHidden { get; set; }

        public string ClientInviteLetterNameUpload { get; set; }
    }

    public class TravellingLocationList
    {
        public int TravellingLocationId { get; set; }
        public string TravellingLocationName { get; set; }
    }
}