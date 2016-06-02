using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HRMS.Models
{
    public class ConfirmationDetailsViewModel
    {
        public string Field { get; set; }

        public string hiddenid { get; set; }

        public string encryptedEmployeeId { get; set; }

        public string FieldChild { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<FieldChildList> FieldchildList { get; set; }

        public List<ShowStatus> ShowStatusList { get; set; }

        public int? EmployeeId { get; set; }

        public int? ConfirmationID { get; set; }

        public string EmployeeCode { get; set; }

        public int? UserId { get; set; }

        public string EmployeeName { get; set; }

        public DateTime? JoiningDate { get; set; }

        public DateTime? ProbationReviewDate { get; set; }

        public DateTime? InitiatedDate { get; set; }

        public int? StageID { get; set; }

        public string Stage { get; set; }

        public bool IsManager { get; set; }

        public bool IsFurtherApprover { get; set; }

        public bool? IsFurtherApproverPresent { get; set; }

        public bool? IsFurtherApproverCleared { get; set; }

        public bool IsAdmin { get; set; }

        public List<SelectListItem> GetField()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                 new SelectListItem { Selected = true, Text = "Select", Value = "Select" },
                new SelectListItem { Selected = true, Text = "Buisness Group", Value = "Buisness Group" },
                new SelectListItem { Selected = true, Text = "Organization Unit", Value = "Organization Unit" },
                new SelectListItem { Selected = true, Text = "Stage Name", Value = "Stage Name" },
            };
            return list;
        }
    }

    public class FieldChildList
    {
        public int ID { get; set; }

        public string Discription { get; set; }
    }

    public class ShowStatus
    {
        public int? ShowstatusEmployeeId { get; set; }

        public int? stagecheck { get; set; }

        public string stagecheckname { get; set; }

        public string ShowstatusEmployeeCode { get; set; }

        public string ShowstatusEmployeeName { get; set; }

        public int? ShowstatusStageID { get; set; }

        public string ShowstatusCurrentStage { get; set; }

        public string showStatus { get; set; }

        public DateTime? ShowstatusTime { get; set; }

        public string ShowstatusActor { get; set; }

        public string ShowstatusAction { get; set; }

        public string ShowstatusComments { get; set; }
    }
}