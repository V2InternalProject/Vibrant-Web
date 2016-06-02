using HRMS.Models.Enums;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HRMS.Models
{
    public class AppraisalProcessStatus
    {
        public int? FormCode { get; set; }
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public string Field { get; set; }
        public string FieldChild { get; set; }
        public List<AppraisalFieldChildDetails> FieldChildList { get; set; }
        public string Employeename { get; set; }
        public int AppraisalId { get; set; }

        public int? Appriser1Id { get; set; }
        public int? Appriser2Id { get; set; }
        public int? Reviwer1Id { get; set; }
        public int? Reviwer2Id { get; set; }
        public int? GroupHeadId { get; set; }

        public int? ReportingTo { get; set; }
        public int? StageId { get; set; }
        public string stageName { get; set; }
        public int? EmployeeId { get; set; }
        public string EncryptedAppraisalId { get; set; }
        public string EncryptedEmployeeId { get; set; }
        public int? AppraisalStageOrder { get; set; }
        public string ProjectName { get; set; }
        public DateTime? DateOfSubmission { get; set; }
        public int AppraisalYearId { get; set; }
        public EmployeeMailTemplate Mail { get; set; }
        public string TextLink { get; set; }
        public AppraisalProcessViewDetails ViewDetails { get; set; }
        public DateTime? IDFFrozenOnDate { get; set; }
        public bool? UnFreezedByAdmin { get; set; }

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

    public class AppraisalFieldChildDetails
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }
}