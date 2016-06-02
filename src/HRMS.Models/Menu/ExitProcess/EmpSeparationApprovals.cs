using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HRMS.Models
{
    public class EmpSeparationApprovals
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public string UserRole { get; set; }

        public string LoggedInUser { get; set; }

        public int? EmployeeId { get; set; }

        public string WatchListEmployeeName { get; set; }

        public int? StageId { get; set; }

        public int? ReportingTo { get; set; }

        public int? hdnReportingTo { get; set; }

        public int? ExitStageOrder { get; set; }

        public string stageName { get; set; }

        public int ExitInstanceId { get; set; }

        public string status { get; set; }

        public string SearchEmployeeName { get; set; }

        public string SearchEmployeeId { get; set; }

        public string Field { get; set; }

        public string FieldChild { get; set; }

        public string loginUsersDepartment { get; set; }

        public string IsProjectStageCleared { get; set; }

        public bool? IsWithdrawn { get; set; }

        public List<FieldChildDetails> FieldChildList { get; set; }

        public FinanceClearance FinanceClearance { get; set; }

        public SeparationShowDetails ShowDetails { get; set; }

        public SeparationShowStatus ShowStatus { get; set; }

        public SeparationMailTemplate Mail { get; set; }

        public ExitInterviewViewModel ExitInterviewForm { get; set; }

        public string EncryptedExitInstanceId { get; set; }

        public DateTime? ResignedDate { get; set; }

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

    public class FieldChildDetails
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }
}