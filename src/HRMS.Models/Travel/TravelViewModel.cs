using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class TravelViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int TravelEmployeeId { get; set; }

        public int TravelId { get; set; }

        public bool IsNewForm { get; set; }

        public int? StageID { get; set; }

        public int TravelEmployeeCode { get; set; }

        public string EncryptedTravelId { get; set; }

        public string EncryptedEmployeeId { get; set; }

        public string TravelEmployeeName { get; set; }

        public bool IsShowAlert { get; set; }

        public int? ClientTravelListType { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Form Name can not be greater than 20 characters.")]
        public string TravelTRFNo { get; set; }

        public DateTime? RequestDate { get; set; }

        public int? RequesterId { get; set; }

        public int ProjectNameId { get; set; }

        [Required(ErrorMessage = "Project Name is required")]
        public string ProjectName { get; set; }

        public string DeliveryTeam { get; set; }

        public DateTime? DateOfSubmission { get; set; }

        public int? TravelToCountry { get; set; }

        public List<CountryDetailsList> CurrentCountryList { get; set; }

        [Required]
        public int? TravelType { get; set; }

        [Required]
        public DateTime? TravelStartDate { get; set; }

        [Required]
        public DateTime? TravelEndDate { get; set; }

        [Required]
        public DateTime? ImmigrationDate { get; set; }

        [Required]
        public DateTime? TravelExtexsionStartDate { get; set; }

        [Required]
        public DateTime? TravelExtensionEndDate { get; set; }

        public ContactViewModel ContactDeatails { get; set; }

        public PassportViewModel PassportDetails { get; set; }

        public JourneyViewModel JourneyDetails { get; set; }

        public ClientViewModel ClientDetails { get; set; }

        public VisaViewModel VisaDetails { get; set; }

        public AccomodationViewModel AccomodationDetails { get; set; }

        public AccomodationAdminViewModel AccomodationAdminDetails { get; set; }

        public ConveyanceAdminViewModel ConveyanceDetails { get; set; }

        public OtherAdminViewModel OtherAdminDetails { get; set; }

        //[Required]
        //public int? IsValidVisa { get; set; }

        //[Required]
        //public int? TravellingAbroadForFirstTime { get; set; }

        [Required]
        public int? TravelReadyOnly { get; set; }

        [Required]
        public int? ExpenseReimbursedByClient { get; set; }

        //[Required]
        //public int? IsPointOfContact { get; set; }

        //[Required]
        //public string POCDetail { get; set; }

        public string AdditionalInfo { get; set; }

        public string TravelExtDetails { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public int IsParent { get; set; }

        public string OrganizationUnit { get; set; }

        [Required]
        public int? ProjectManagerApprover { get; set; }

        [Required]
        public int? AdminApprover { get; set; }

        [Required]
        public int? GroupheadApprover { get; set; }

        public List<ProjectManagerList> ProjectManagerList { get; set; }

        public List<AdminAppoverList> AdminAppoverList { get; set; }
        public CustomerDetails CustomerDetailsList { get; set; }
        public List<GroupHeadList> GroupHeadList { get; set; }

        public List<TravelTypeList> TravelTypeList { get; set; }

        public List<ClientTravelList> ClientReimbursementList { get; set; }

        public TravelMailTemplate Mail { get; set; }

        public List<ProjectNameList> ProjectNameList { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.]*@[a-zA-Z0-9_.]*\.([a-zA-Z]{2,4})$", ErrorMessage = "Please enter valid email id.")]
        [StringLength(200, ErrorMessage = "Email ID can not be greater that 200 characters.")]
        public string userPersonalEmailId { get; set; }

        [RegularExpression(@"^[0-9.\+\(\)\-\s]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [StringLength(100, ErrorMessage = "Contact Number India can not be greater than 100 characters.")]
        public string ContactNoIndia { get; set; }

        [Required]
        [RegularExpression(@"^[0-9.\+\(\)\-\s]+$", ErrorMessage = "Phone Number can not contain alphabets.")]
        [StringLength(100, ErrorMessage = "Contact Number Abroad can not be greater than 100 characters.")]
        public string ContactNoAbroad { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.]*@[a-zA-Z0-9_.]*\.([a-zA-Z]{2,4})$", ErrorMessage = "Please enter valid email id.")]
        [StringLength(200, ErrorMessage = "Email ID can not be greater that 200 characters.")]
        public string PersonalEmailId { get; set; }
    }

    public class CustomerDetails
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }
    }

    public class ProjectManagerList
    {
        public int ProjectManagerId { get; set; }

        public string ProjectManagerName { get; set; }
    }

    public class AdminAppoverList
    {
        public int AdminAppoverId { get; set; }

        public string AdminAppoverName { get; set; }
    }

    public class GroupHeadList
    {
        public int GroupHeadId { get; set; }

        public string GroupHeadrName { get; set; }
    }

    public class TravelTypeList
    {
        public int TravelTypeId { get; set; }

        public string TravelTypes { get; set; }
    }

    public class ClientTravelList
    {
        public int ClientTravelsId { get; set; }

        public string ClientTravelsValue { get; set; }
    }

    public class RetriveTravelID
    {
        public bool IsAdded { get; set; }

        public int TravelID { get; set; }

        public int EmployeeID { get; set; }
    }

    public class CountryDetailsList
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CC_FIPS { get; set; }
    }

    public class ContactViewModel2
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
    }
}