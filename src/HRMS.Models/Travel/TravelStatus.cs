using System.Collections.Generic;
using System.Web.Mvc;

namespace HRMS.Models
{
    public class TravelStatus
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<TravelFieldChildDetails> FieldChildList { get; set; }

        public string TRFNo { get; set; }

        public string TravelFormCode { get; set; }

        public string TravelRequestNumber { get; set; }

        public string Employeename { get; set; }

        public string EncryptedTravelId { get; set; }

        public string EncryptedEmployeeId { get; set; }

        public int? TravelStageOrder { get; set; }

        public int TravelId { get; set; }

        public int? StageId { get; set; }

        public string stageName { get; set; }

        public string ProjectName { get; set; }

        public int? EmployeeId { get; set; }

        public int? ReportingTo { get; set; }

        public int? ProjectManagerApprover { get; set; }

        public int? GroupHeadApprover { get; set; }

        public int? AdminApprover { get; set; }

        public string Field { get; set; }

        public string FieldChild { get; set; }

        public EmployeeMailTemplate Mail { get; set; }

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

    public class TravelFieldChildDetails
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }
}