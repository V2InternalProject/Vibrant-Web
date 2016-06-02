using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class PMSProjectDetailsViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public RevisionDetails RevisionDetail { get; set; }

        public ApproveRevision ApproveRevision { get; set; }
        public ShowHistoryPMS ShowHistory { get; set; }
        public RevisionDetails RevisionDetails { get; set; }
        public CustomerAddress ProjectOwners { get; set; }
        public CustomerContact ProjectReviewers { get; set; }
        public CustomerContract ProjectDocuments { get; set; }

        public List<RevisionList> RevisionFeilds { get; set; }

        public ProjectReviewers ReviewerList { get; set; }

        //public AddCustomerAddress AddCustomerAddressinvoice { get; set; }
        //public AddContact AddContact { get; set; }
        //public AddContract AddContract { get; set; }
        public CustomerContract IRApprovers { get; set; }

        public CustomerContract IRGenerators { get; set; }
        public TravelMailTemplate Mail { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Abbreviated Name can not be greater than 10 characters.")]
        [Display(Name = "Abbreviated Name")]
        public string AbbreviatedName { get; set; }

        public DateTime Holidaydate { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Project Name can not be greater than 50 characters.")]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }             // This is used for Project Name Text in Add Project Details View

        public string ApproverDecisionComment { get; set; }
        public int? ProjectID { get; set; }
        public bool IsProjectApprover { get; set; }
        public bool IsProjectApproverPresent { get; set; }
        public int? userId { get; set; }
        public int? ApprovalStatusID { get; set; }
        public string HrAdminEMailIds { get; set; }

        public int? RoleId { get; set; }
        public int? IRApproverRoleId { get; set; }
        public int? IRFinanceApproverRoleId { get; set; }

        public string RoleDescription { get; set; }

        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [StringLength(2000, ErrorMessage = "Description can not be greater than 2000 characters.")]
        [Display(Name = "Description")]
        public string PMSProjectDescription { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectNames { get; set; }            // This is used for Project Name DropDown which contains All Project Names

        [Display(Name = "Sort by Approval Status")]
        public string PMSApprovalStatus { get; set; }

        public string PMSRevisionStatus { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime? PMSProjectStartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime? PMSProjectEndDate { get; set; }

        [Display(Name = "Duration(Days)")]
        public int? PMSProjectDurationDays { get; set; }

        [Display(Name = "Work(Hours)")]
        public string PMSProjectWorkHours { get; set; }

        //public Double? PMSProjectWorkHours { get; set; }

        [Display(Name = "Billable")]
        public bool PMSProjectBillableStatus { get; set; }

        [Required]
        [Display(Name = "Delivery Unit")]
        public int? PMSDeliveryUnit { get; set; }

        public string DeliveryUnitName { get; set; }

        [Required]
        [Display(Name = "Delivery Team")]
        public int? PMSDeliveryTeam { get; set; }

        public string DeliveryTeamName { get; set; }

        [Required]
        [Display(Name = "Organization Unit")]
        public int? PMSOrganizationUnit { get; set; }

        public string OrganizationUnitName { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int? PMSCustomer { get; set; }

        [Display(Name = "Project Group")]
        public int? PMSProjectGroup { get; set; }

        [Required]
        [Display(Name = "Project Status")]
        public string PMSProjectStatus { get; set; }

        public int? PMSProjectStatusID { get; set; }

        [Display(Name = "Project Currency")]
        public int? PMSProjectCurrency { get; set; }

        [Display(Name = "Practice")]
        public int? PMSPractice { get; set; }

        public string PracticeName { get; set; }

        [Display(Name = "Life Cycle")]
        public int? PMSLifeCycle { get; set; }

        [Required]
        [Display(Name = "Commercial Details")]
        public int? PMSCommercialDetailsType { get; set; }

        [Required]
        [Display(Name = "Business Group")]
        public int? PMSBusinessGroup { get; set; }

        public string FeildName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public int ApprovalStaus { get; set; }

        public int RevisionStaus { get; set; }

        public List<string> fieldlabellist { get; set; }

        public List<string> approvalMessageList { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int ProjectReviewerId { get; set; }

        public string loggedInUserEmployeeCode { get; set; }

        public Int32 AuditId { get; set; }
        public string FieldName { get; set; }
        public string OldValueProjectHistory { get; set; }
        public string NewValueProjectHistory { get; set; }
        public string ApproverDescription { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime? ApprovedRejectedOn { get; set; }
        public string ApprovedRejectedBy { get; set; }
        // public string ApproverDescription { get; set; }

        public List<ProjectNamesListDetails> ProjectNamesList { get; set; }

        public List<PMSApprovalStatusListDetails> PMSApprovalStatusList { get; set; }

        public List<PMSDeliveryUnitListDetails> PMSDeliveryUnitList { get; set; }

        public List<PMSDeliveryTeamListDetails> PMSDeliveryTeamList { get; set; }

        public List<PMSOrganizationUnitListDetails> PMSOrganizationUnitList { get; set; }

        public List<PMSCustomerListDetails> PMSCustomerList { get; set; }

        public List<PMSProjectGroupListDetails> PMSProjectGroupList { get; set; }

        public List<PMSProjectStatusListDetails> PMSProjectStatusList { get; set; }

        public List<PMSProjectCurrencyListDetails> PMSProjectCurrencyList { get; set; }

        public List<PMSPracticeListDetails> PMSPracticeList { get; set; }

        public List<PMSLifeCycleListDetails> PMSLifeCycleList { get; set; }

        public List<PMSCommercialDetailsTypeListDetails> PMSCommercialDetailsTypeList { get; set; }

        public List<PMSBusinessGroupListDetails> PMSBusinessGroupList { get; set; }

        [Required]
        public string QuestionOne { get; set; }

        [Required]
        public string QuestionTwo { get; set; }

        [Required]
        public string QuestionThree { get; set; }

        [Required]
        public string QuestionFour { get; set; }

        [Required]
        public string QuestionFive { get; set; }

        public string ManagerRevisionComment { get; set; }

        public bool IsEndDateChanged { get; set; }

        public bool IsWorkHourChanged { get; set; }

        public int ProjectIRApproverId { get; set; }
        public int? IRApproverEmployeeId { get; set; }
        public string IRApproverEmployeeName { get; set; }
        public string IRApproverRoleDescription { get; set; }
        public int? IRApproverProjectID { get; set; }

        public int ProjectIRFinanceApproverId { get; set; }
        public int? IRFinanceApproverEmployeeId { get; set; }
        public string IRFinanceApproverEmployeeName { get; set; }
        public string IRFinanceApproverRoleDescription { get; set; }
        public int? IRFinanceApproverProjectID { get; set; }

        public string CustomerStartDate { get; set; }
        public string CustomerEndDate { get; set; }
        public DateTime? OriginalDateTime { get; set; }
    }

    public class RevisionList
    {
        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string FeildName { get; set; }
    }

    public class RevisionDetailsModel
    {
        public string WorkHours { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ProjectID { get; set; }

        public string QuestionOne { get; set; }

        public string QuestionTwo { get; set; }

        public string QuestionThree { get; set; }

        public string QuestionFour { get; set; }

        public string QuestionFive { get; set; }

        public List<ShowHistoryPMS> ShowHistoryList { get; set; }
    }

    public class ShowHistoryPMS
    {
        public int ProjectId { get; set; }
    }

    public class ProjectNamesListDetails
    {
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
    }

    public class PMSApprovalStatusListDetails
    {
        public int PMSApprovalStatusID { get; set; }
        public string PMSApprovalStatus { get; set; }
    }

    public class PMSDeliveryUnitListDetails
    {
        public int PMSDeliveryUnitID { get; set; }
        public string PMSDeliveryUnit { get; set; }
    }

    public class PMSDeliveryTeamListDetails
    {
        public int PMSDeliveryTeamID { get; set; }
        public string PMSDeliveryTeam { get; set; }
    }

    public class PMSOrganizationUnitListDetails
    {
        public int PMSOrganizationUnitID { get; set; }
        public string PMSOrganizationUnit { get; set; }
    }

    public class PMSCustomerListDetails
    {
        public int PMSCustomerID { get; set; }
        public string PMSCustomer { get; set; }
    }

    public class PMSProjectGroupListDetails
    {
        public int PMSProjectGroupID { get; set; }
        public string PMSProjectGroup { get; set; }
    }

    public class PMSProjectStatusListDetails
    {
        public int PMSProjectStatusID { get; set; }
        public string PMSProjectStatus { get; set; }
    }

    public class PMSProjectCurrencyListDetails
    {
        public int PMSProjectCurrencyID { get; set; }
        public string PMSProjectCurrency { get; set; }
    }

    public class PMSPracticeListDetails
    {
        public int PMSPracticeID { get; set; }
        public string PMSPractice { get; set; }
    }

    public class PMSLifeCycleListDetails
    {
        public int PMSLifeCycleID { get; set; }
        public string PMSLifeCycle { get; set; }
    }

    public class PMSCommercialDetailsTypeListDetails
    {
        public int PMSCommercialDetailsTypeID { get; set; }
        public string PMSCommercialDetailsType { get; set; }
    }

    public class PMSBusinessGroupListDetails
    {
        public int PMSBusinessGroupID { get; set; }
        public string PMSBusinessGroup { get; set; }
    }

    public class ProjectReviewers
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }
    }
}