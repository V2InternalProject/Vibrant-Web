using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class OrganizationStructure
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<BusinessGroup> BusinessGroups { get; set; }

        public List<OrganizationUnit> OrganizationUnits { get; set; }

        public List<ExistingOrganizationUnit> ExistingOrganizationUnits { get; set; }

        public List<ExistingDeliveryUnit> ExistingDeliveryUnits { get; set; }

        public List<ExistingDeliveryTeam> ExistingDeliveryTeams { get; set; }

        public List<DeliveryUnit> DeliveryUnits { get; set; }

        public List<DeliveryTeam> DeliveryTeams { get; set; }

        public List<BusinessGroup> InActiveBusinessGroups { get; set; }

        public List<OrganizationUnit> InActiveOrganizationUnits { get; set; }

        public List<DeliveryUnit> InActiveDeliveryUnits { get; set; }

        public List<DeliveryTeam> InActiveDeliveryTeams { get; set; }

        public List<Object> ExtremFinalList { get; set; }

        public List<MiddleLevelResources> MiddleLevelResourcesList { get; set; }

        public List<ManagerList> OUManagerList { get; set; }

        [Display(Name = "Total Records : ")]
        public int TotalOrganizationUnits { get; set; }

        [Display(Name = "Total Records : ")]
        public int TotalBusinessGroupManagers { get; set; }

        [Display(Name = "Total Records : ")]
        public int TotalMiddleLevelResources { get; set; }

        [Display(Name = "Total Records : ")]
        public int TotalDeliveryTeams { get; set; }

        [Display(Name = "Total Records : ")]
        public int TotalDeliveryUnitsManagers { get; set; }

        [Display(Name = "Total Records : ")]
        public int TotalDeliveryUnits { get; set; }

        [Display(Name = "Total Records : ")]
        public int TotalDocumentCategories { get; set; }

        [Display(Name = "Total Records : ")]
        public int TotalOUManagers { get; set; }

        public List<ManagerList> BusinessGroupManagerList { get; set; }

        public List<ManagerList> DeleveryTeamHeadList { get; set; }

        public List<ManagerList> DeleveryUnitManagerList { get; set; }

        public List<DocumentCategory> OrganizationUnitDocumentCategory { get; set; }
        public List<OrganizationUnitDocumentList> CategoryList { get; set; }

        [Required(ErrorMessage = "'Category' should not be left blank.")]
        public string ddlCategory { get; set; }

        public int CategoryID { get; set; }

        public int OldEmployeeID { get; set; }

        [Required(ErrorMessage = "'Delivery Unit Code' should not be left blank.")]
        public string newResourcePoolCode { get; set; }

        [Required(ErrorMessage = "'Delivery Unit Name' should not be left blank.")]
        public string newresourcePoolName { get; set; }

        [Required]
        [Display(Name = "Manager ")]
        public string Manager { get; set; }

        public int Old_Manager { get; set; }

        [Display(Name = "Is Primary Responsible ")]
        public bool IsPrimaryResponsible { get; set; }

        public int BusinessGroupID { get; set; }

        [Required]
        [Display(Name = "Unit Code")]
        public string LocationCode { get; set; }

        [Required(ErrorMessage = "'Organization Unit' should not be left blank.")]
        [Display(Name = "Unit Name")]
        public string Location { get; set; }

        public int LocationID { get; set; }

        public int UniqueID { get; set; }

        public int OUPoolID { get; set; }

        [Display(Name = "Short Code")]
        public string ShortCode { get; set; }

        [Display(Name = "Address1")]
        public string Address { get; set; }

        [Display(Name = "Address2")]
        public string Address1 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Zip")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter only numbers")]
        [StringLength(6, MinimumLength = 5, ErrorMessage = "Zip code can not be lesser than 5 digits and can not be greater than 6 digits.")]
        public string Zip { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Country")]
        public List<OrganizationCountryDetails> CountryList { get; set; }

        public string Country { get; set; }

        [Display(Name = "Phone Number 1")]
        [RegularExpression("^(0|[1-9][0-9]*)$", ErrorMessage = "Phone Number 1 can not contain characters.")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Phone Number 1 can not be lesser than 10 digits and can not be greater that 11 digits.")]
        public string Phone1 { get; set; }

        [Display(Name = "Phone Number 2")]
        [RegularExpression("^(0|[1-9][0-9]*)$", ErrorMessage = "Phone Number 2 can not contain characters.")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Phone Number 2 can not be lesser than 10 digits and can not be greater that 11 digits.")]
        public string Phone2 { get; set; }

        [Display(Name = "Fax")]
        [RegularExpression("^(0|[1-9][0-9]*)$", ErrorMessage = "Fax number can not contain characters.")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Fax number can not be lesser than 10 digits and can not be greater that 11 digits.")]
        public string Fax { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9_\+-]+(\.[a-zA-Z0-9_\+-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.([a-zA-Z]{2,4})$", ErrorMessage = "Please enter valid email id.")]
        [StringLength(50, ErrorMessage = "Email ID can not be greater that 50 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "'Working Hours' should not be left blank.")]
        [Range(1, 24, ErrorMessage = "The value of 'Working Hours' should be in range of (1-24).")]
        [Display(Name = "Working Hours")]
        public double? WorkingHours { get; set; }

        [Required(ErrorMessage = "'Working Days' should not be left blank.")]
        [Range(1, 7, ErrorMessage = "The value of 'Working Days' should be in range of (1-7).")]
        [Display(Name = "Working Days")]
        public double? WorkingDays { get; set; }

        [Required]
        [Display(Name = "Business Group Name")]
        public string businessgroup { get; set; }

        [Required]
        [Display(Name = "Business Group Code")]
        public string BusinessGroupCode { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        [Display(Name = "Timesheet Required to be filled")]
        public bool TimesheetRequired { get; set; }

        [Required]
        [Display(Name = "Delivery Team Code ")]
        public string GroupCode { get; set; }

        [Required]
        [Display(Name = "Delivery Team Name")]
        public string GroupName { get; set; }

        public int GroupID { get; set; }

        [Display(Name = "Delivery Team Head")]
        public int ResourceHeadID { get; set; }

        [Display(Name = "Delivery Unit Code")]
        public string ResourcePoolCode { get; set; }

        [Required]
        [Display(Name = "Delivery Unit Name")]
        public string ResourcePoolName { get; set; }

        public int ResourcePoolID { get; set; }

        [Required]
        [Display(Name = "Organization Unit")]
        public string ExistingOU { get; set; }

        [Required]
        [Display(Name = "Delivery Unit")]
        public string ExistingDU { get; set; }

        [Required]
        [Display(Name = "Delivery Team")]
        public string ExistingDT { get; set; }
    }

    public class DocumentCategory
    {
        public int LocationID { get; set; }
        public string Location { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Checked { get; set; }
    }

    public class OrganizationUnitDocumentList
    {
        public int CategoryID { get; set; }
        public string Category { get; set; }
    }

    public class OrganizationCountryDetails
    {
        public int CountryId { get; set; }

        public string CountryName { get; set; }
    }

    public class BusinessGroup
    {
        private List<ManagerList> _employeeList = null;

        public int BusinessGroupID { get; set; }

        public string businessgroup { get; set; }

        public string BusinessGroupCode { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool Active { get; set; }

        public int? LastSequence { get; set; }

        public List<ManagerList> EmployeeList
        {
            get
            {
                if (_employeeList == null)
                {
                    _employeeList = new List<ManagerList>();
                }
                return _employeeList;
            }
            set
            {
                _employeeList = value;
            }
        }

        public bool Checked { get; set; }
    }

    public class ManagerList
    {
        public string UserName { get; set; }

        public string EmployeeName { get; set; }

        public int EmployeeID { get; set; }

        public int ResourcePoolID { get; set; }

        public int BusinessGroupID { get; set; }

        public bool IsPrimaryResponsible { get; set; }

        public int LocationID { get; set; }

        public bool Checked { get; set; }
    }

    public class MiddleLevelResources
    {
        public int EmpoloyeeID { get; set; }

        public int LocationID { get; set; }

        public string EmployeeName { get; set; }

        public string Role { get; set; }

        public string EmailID { get; set; }

        public int BusinessGroupID { get; set; }

        public bool Checked { get; set; }
    }

    public class OrganizationUnit
    {
        public int LocationID { get; set; }

        public string Location { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string LocationCode { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool Active { get; set; }

        public int UniqueID { get; set; }

        public int? BusinessGroupID { get; set; }

        public int? OUPoolID { get; set; }

        public List<string> EmployeeName { get; set; }

        public bool Checked { get; set; }
    }

    public class ExistingDeliveryUnit
    {
        public int ResourcePoolID { get; set; }

        public string ResourcePoolName { get; set; }

        public int LocationID { get; set; }
    }

    public class ExistingOrganizationUnit
    {
        public int LocationID { get; set; }

        public string Location { get; set; }

        public int BusinessGroupID { get; set; }
    }

    public class ExistingDeliveryTeam
    {
        public int ResourcePoolID { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
    }

    public class DeliveryUnit
    {
        public int LocationID { get; set; }

        public string Location { get; set; }

        public bool Checked { get; set; }

        public int ResourcePoolID { get; set; }

        public string ResourcePoolCode { get; set; }

        public string ResourcePoolName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool Active { get; set; }

        public int UniqueID { get; set; }

        public int? OUPoolID { get; set; }

        public List<string> EmployeeName { get; set; }

        public int BusinessGroupID { get; set; }
    }

    public class DeliveryTeam
    {
        public int GroupID { get; set; }

        public string GroupCode { get; set; }

        public string GroupName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ResourceHeadID { get; set; }

        public bool Active { get; set; }

        public int UniqueID { get; set; }

        public int? ResourcePoolID { get; set; }

        public string ResourcePoolName { get; set; }

        public string EmployeeCode { get; set; }

        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public int BusinessGroupID { get; set; }

        public bool Checked { get; set; }
    }

    public class OrganizationStructureResponse
    {
        public bool Isadded { get; set; }

        public bool IsExisted { get; set; }

        public bool IsActive { get; set; }
    }
}