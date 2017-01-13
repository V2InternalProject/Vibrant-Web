using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class EmployeeDetailsViewModel
    {
        [Display(Name = "Employee Id")]
        public int? EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Display(Name = "Probation Duration(Months)")]
        [RegularExpression("^[0-9 .]+$", ErrorMessage = "Months can not contain alphabates or special characters.")]
        public int? Months { get; set; }

        [Required]
        [Display(Name = "Joining Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? JoiningDate { get; set; }

        [Display(Name = "Exit Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExitDate { get; set; }

        //[Required]
        [Display(Name = "Probation Review Date")]
        //[DataType(DataType.Date)]
        [RegularExpression(@"(0[1-9]|1[012])/(0[1-9]|[12][0-9]|3[01])/(19|20)\d\d", ErrorMessage = "Please enter valid Date")]
        public DateTime? ProbationReviewDate { get; set; }

        [Display(Name = "Confirmation Date")]
        //[DataType(DataType.Date)]
        [RegularExpression(@"(0[1-9]|1[012])/(0[1-9]|[12][0-9]|3[01])/(19|20)\d\d", ErrorMessage = "Please enter valid Date")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ConfirmationDate { get; set; }

        [Display(Name = "Billable")]
        public bool BillableStatus { get; set; }

        public string EmailID { get; set; }

        [Required]
        [Display(Name = "Employee Status")]
        public string EmployeeStatus { get; set; }

        [Required]
        [Display(Name = "Employment Status")]
        public string EmployeeStatusMaster { get; set; }

        [Display(Name = "Last year’s promotion status")]
        [RegularExpression("^[a-zA-Z .]+$", ErrorMessage = "Last year's promotion status can not contain numbers or special characters.")]
        [StringLength(50, ErrorMessage = "Last year’s promotion status can not be more than 50 characters.")]
        public string LastYearPromotion { get; set; }

        [Display(Name = "Last year’s Appraisal Score")]
        [StringLength(50, ErrorMessage = "Last year’s Appraisal Score can not be more than 50 characters.")]
        public string LastYearAppraisal { get; set; }

        [Display(Name = "Last year’s Increment %")]
        [StringLength(50, ErrorMessage = "Last year’s Increment % can not be more than 50 characters.")]
        public string LastYearIncrement { get; set; }

        [Required(ErrorMessage = "The Office Location field is required.")]
        [Display(Name = "Office Location")]
        public string OfficeLocation { get; set; }

        //[Required]
        [Display(Name = "Organization Unit")]
        public string OrganizationUnit { get; set; }

        [StringLength(50, ErrorMessage = "Additional Location Details can not be more than 50 characters.")]
        public string Region { get; set; }

        //[Required]
        [Display(Name = "Group")]
        public string Group { get; set; }

        [Display(Name = "Resource Pool Name")]
        public string ResourcePoolName { get; set; }

        // [Required]
        [Display(Name = "Parent DU")]
        public string ParentDU { get; set; }

        [Display(Name = "Delivery Team")]
        public string DT { get; set; }

        //[Required]
        [Display(Name = "Current DU")]
        public int? CurrentDU { get; set; }

        [Required]
        [Display(Name = "Recruiter Name")]
        //[RegularExpression("^[a-zA-Z .]+$", ErrorMessage = "Recruiter Name can not contain numbers or special characters.")]
        //[StringLength(100, ErrorMessage = "Recruiter Name can not be more than 100 characters.")]
        public string RecruiterName { get; set; }

        [Required]
        [Display(Name = "Commitments Made")]
        [StringLength(1000, ErrorMessage = "Commitments Made can not be more than 1000 characters.")]
        public string CommitmentsMade { get; set; }

        public string Designation { get; set; }

        public string UserRole { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Rejoined Within One Year")]
        public bool RejoinedWithingOneYear { get; set; }

        [Required(ErrorMessage = "The Holiday Calendar field is required.")]
        [Display(Name = "Holiday Calendar")]
        public string CalenderName { get; set; }

        //[Required]
        [Display(Name = "Shift")]
        public string Shift { get; set; }

        [Display(Name = "Login Role")]
        public string LoginRole { get; set; }

        public List<EmployeeStatusListDetails> EmployeeStatusList { get; set; }

        public List<EmployeeStatusMsterListDetails> EmployeeStatusMasterList { get; set; }

        //[Required]
        public int ReportingToId_Emp { get; set; }

        public string ReportingToName_Emp { get; set; }

        //[Required]
        public int CompetencyManagerId_Emp { get; set; }

        public string CompetencyManagerName_Emp { get; set; }

        public string ExitConfirmationManagerName_Emp { get; set; }

        //[Required]
        public int ExitConfirmationManagerId_Emp { get; set; }

        [Required(ErrorMessage = "Please select Organization Role")]
        public int? OrgRoleID { get; set; }

        //public List<RoleList> RoleList { get; set; }

        public string OrgRoleDescription { get; set; }

        public List<LoginRolesDetails> LoginRolesList { get; set; }

        public List<LoginRolesDetails> HdLoginRolesList { get; set; }

        [Required]
        public string SelectedRolesList { get; set; }

        [Display(Name = "ESIC No.")]
        [StringLength(20, ErrorMessage = "ESIC No. can not be more than 20 characters.")]
        public string ESICNo { get; set; }

        [Display(Name = "PF No.")]
        [StringLength(20, ErrorMessage = "PF No. can not be more than 20 characters.")]
        public string PFNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? TentativeReleaseDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ResignedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? AgreedReleaseDate { get; set; }

        public int EmpStatusMasterID { get; set; }

        public List<ReportingToList_Emp> ReportingToList_Emp { get; set; }

        [Display(Name = "PAN Number")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "PAN Numbaer can not contain special characters.")]
        [StringLength(20, ErrorMessage = "PAN Number can not be greater that 20 characters.")]
        [Required]
        public string IncomeTaxNo { get; set; }

        public List<Business_Group> BusinessGroups { get; set; }
    }

    public class ReportingToList_Emp
    {
        public int? EmployeeId { get; set; }

        public string EmployeeName { get; set; }
    }

    public class Business_Group
    {
        public int BusinessGroupID { get; set; }

        public string BusinessGroup { get; set; }
    }

    public class AccessRightMapping
    {
        public string UserName { get; set; }

        public string RoleName { get; set; }

        public string ActionKey { get; set; }

        public int MenuId { get; set; }

        public string ControllerName { get; set; }

        public string Action { get; set; }

        public string Section { get; set; }

        public string Area { get; set; }

        public int? CanAdd { get; set; }
    }
}