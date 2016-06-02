using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HRMS.Models
{
    public class ExpenseReimbursementStatus
    {
        public int? FormCode { get; set; }
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public string Field { get; set; }
        public string FieldChild { get; set; }
        public List<ReimbursementFieldChildDetails> FieldChildList { get; set; }
        public string FormName { get; set; }
        public string Employeename { get; set; }
        public int ExpenseId { get; set; }

        public int? ReportingTo { get; set; }
        public int? StageId { get; set; }
        public string stageName { get; set; }
        public int? EmployeeId { get; set; }
        public string EncryptedExpenseId { get; set; }
        public string EncryptedEmployeeId { get; set; }
        public string ExpenseFormName { get; set; }
        public string ExpenseFormCode { get; set; }
        public int? ExpenseStageOrder { get; set; }
        public int? ProjectName { get; set; }
        public int IsClientReimbursement { get; set; }
        public string ClientName { get; set; }
        public DateTime? DateOfSubmission { get; set; }
        public int? PrimaryApprover { get; set; }
        public int? SecondaryApprover { get; set; }
        public int? FinanceApprover { get; set; }
        public decimal? ToalAmount { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public EmployeeMailTemplate Mail { get; set; }

        public ExpenseReimbursementDetails ExpenseReimbursementDetails { get; set; }

        public List<ExpenseReimbursementDetails> ExpenseReimbursementDetailsList { get; set; }

        public List<SelectListItem> GetFieldList()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Business Group", Value = "Business Group" },
                new SelectListItem { Selected = true, Text = "Organization Unit", Value = "Organization Unit" },
                new SelectListItem { Selected = true, Text = "Stage Name", Value = "Stage Name" },
            };
            return list;
        }
    }

    public class ReimbursementFieldChildDetails
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }

    public class ExpenseReimbursementDetails
    {
        public int ExpenseId { get; set; }

        public int ExpenseDetailsId { get; set; }

        public string ReceiptNo { get; set; }

        public DateTime? DateOfExpense { get; set; }

        public string NatureOfExpense { get; set; }

        public string Details { get; set; }

        public decimal? Amount { get; set; }

        public string Comments { get; set; }

        public bool Verify { get; set; }

        public string EncryptedEmployeeId { get; set; }

        public List<ExpenseReimbursementViewModel> FormDetailsList { get; set; }

        public List<ExpenseReimbursementDetails> MyProperty { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }
    }
}