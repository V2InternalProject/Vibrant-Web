using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ExpenseReimbursementViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int? FormCode { get; set; }

        public int ReimbursementEmployeeId { get; set; }

        public int ReimbursementEmployeeCode { get; set; }

        public string ReimbursementEmployeeName { get; set; }

        public string EncryptedExpenseId { get; set; }

        public string EncryptedEmployeeId { get; set; }

        //[Required]
        //[StringLength(20, ErrorMessage = "Form Name can not be greater than 20 characters.")]
        public string ReimbursementFormName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Code can not be greater than 20 characters.")]
        public string ReimbursementFormCode { get; set; }

        [Required(ErrorMessage = "Please enter Cheque Details.")]
        [StringLength(200, ErrorMessage = "Cheque Details can not be greater than 200 characters.")]
        public string ChequeDetails { get; set; }

        public string DeliveryTeam { get; set; }

        public string Location { get; set; }

        [Required(ErrorMessage = "Cleint Name is required")]
        public int? ProjectName { get; set; }

        [Required(ErrorMessage = "Client-Reimbursment is required")]
        public int? ClientReimbursement { get; set; }

        [Required(ErrorMessage = "Client-Name is required")]
        [StringLength(50, ErrorMessage = "Client Name can not be greater than 50 characters.")]
        public string clientName { get; set; }

        public DateTime? DateOfSubmission { get; set; }

        [Required(ErrorMessage = "Primary Approver is required")]
        public int? PrimaryApprover { get; set; }

        [Required(ErrorMessage = "Secondary Approver is required")]
        public int? SecondaryApprover { get; set; }

        public int? FinanceApprover { get; set; }

        [Required(ErrorMessage = "Cost-Center is required")]
        public int? CostCentre { get; set; }

        public int? Currency { get; set; }

        public int? StageID { get; set; }

        public int ExpenseId { get; set; }

        public bool IsApprove { get; set; }

        public bool IsTotalApprove { get; set; }

        public bool IsAdvanceApproved { get; set; }

        public bool IsBalanceApproved { get; set; }

        public int ExpenseDetailsId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Receipt number should be less that 50 digits.")]
        public string ReceiptNo { get; set; }

        [Required]
        public DateTime? DateOfExpense { get; set; }

        [Required]
        public int? NatureOfExpense { get; set; }

        [StringLength(500, ErrorMessage = "Details can not be greater than 500 characters.")]
        public string Details { get; set; }

        public string AmountInWords { get; set; }

        [Required]
        // [Range(0, 100)]
        // [DisplayFormat(DataFormatString = "{10:00}", ApplyFormatInEditMode = true)]
        //[StringLength(10, ErrorMessage = "Amount should be less that 10 digits.")]
        // [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} must be a Number.")]
        [Range(0.01, 9999999.00, ErrorMessage = "Amount must be between 0.01 and 9999999.00")]
        public decimal? Amount { get; set; }

        public decimal? Total { get; set; }

        public string ExpenseDetails { get; set; }

        //[Required]
        //[RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Price must can't have more than 2 decimal places")]
        [RegularExpression(@"^\+?[0-9]*\.?[0-9]+$", ErrorMessage = "Please enter numeric value only.")]

        public decimal? Advances { get; set; }

        public decimal? Balance { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "Comments can not be greater than 300 characters.")]
        public string Comments { get; set; }

        public string RejectComments { get; set; }

        public List<ClientReimbursementList> ClientReimbursementList { get; set; }

        public List<PrimaryApproverList> PrimaryApproverList { get; set; }

        public List<SecondaryApproverList> SecondaryApproverList { get; set; }

        public List<FinanceApproverList> FinanceApproverList { get; set; }

        public List<NatureOfExpenseList> NatureOfExpenseList { get; set; }

        public List<ExpenseReimbursementDetails> FormDataList { get; set; }

        public List<ExpenseDetail> ExpenseDetailList { get; set; }

        public List<CostCentreList> CostCentreList { get; set; }

        public List<CurrencyList> CurrencyList { get; set; }

        public List<ProjectNameList> ProjectNameList { get; set; }

        public string uploadedFileName { get; set; }

        public string uploadedFilePath { get; set; }
    }

    public class ProjectNameList
    {
        public int ProjectNameID { get; set; }
        public string ProjectName { get; set; }
    }

    public class CostCentreList
    {
        public int CostCentreID { get; set; }
        public string CostCentreName { get; set; }
    }

    public class CurrencyList
    {
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
    }

    public class ExpenseDetail
    {
        public int ExpenseDetailID { get; set; }

        public bool Verify { get; set; }
    }

    public class ClientReimbursementList
    {
        public int ClientReimbursementId { get; set; }

        public string ClientReimbursementValue { get; set; }
    }

    public class PrimaryApproverList
    {
        public int PrimaryApproverId { get; set; }

        public string PrimaryApproverName { get; set; }
    }

    public class SecondaryApproverList
    {
        public int SecondaryApproverId { get; set; }

        public string SecondaryApproverName { get; set; }
    }

    public class FinanceApproverList
    {
        public int FinanceApproverId { get; set; }

        public string FinanceApproverName { get; set; }
    }

    public class NatureOfExpenseList
    {
        public int NatureOfExpenseId { get; set; }

        public string NatureOfExpensevalue { get; set; }
    }

    public class ExpenseProjectNamesModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Required(ErrorMessage = "Please Enter Project Name.")]
        [StringLength(50, ErrorMessage = "Project Name can not be greater that 50 characters.")]
        public string NewProjectName { get; set; }

        [Required(ErrorMessage = "Please Enter Project Description.")]
        [StringLength(200, ErrorMessage = "Project Description can not be greater that 200 characters.")]
        public string NewExpProjectDescription { get; set; }

        public List<ExpenseReimbProjects> ExpProjectNamesList { get; set; }

        [Display(Name = "Total Projects :")]
        public int TotalExpProjects { get; set; }

        public int ProjectNameID { get; set; }

        public string ExistingExpProjectName { get; set; }
    }

    public class ExpenseReimbProjects
    {
        public int ProjectNameID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
    }

    public class ExpenseReimbProcessResponse
    {
        public bool isAdded { get; set; }
        public bool isExisted { get; set; }
        public bool isDeleted { get; set; }
        public int latestExpenseID { get; set; }
    }
}