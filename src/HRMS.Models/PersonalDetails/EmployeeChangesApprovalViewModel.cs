using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class EmployeeChangesApprovalViewModel
    {
        public int? EmployeeID { get; set; }
        public string FieldDiscription { get; set; }
        public string Module { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int? ApprovalStatusMasterID { get; set; }
        public string Comments { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string FieldDbColumnName { get; set; }
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<EmployeeChangeDetails> ChangeDetailsList { get; set; }
        public List<EmployeeMailTemplate> mailList { get; set; }
        public EmployeeMailTemplate Mail { get; set; }
    }

    public class EmployeeChangeDetails
    {
        public int? ChildEmployeeID { get; set; }
        public string ChildFieldDiscription { get; set; }
        public string ChildModule { get; set; }
        public string ChildOldValue { get; set; }
        public string ChildNewValue { get; set; }
        public string ChildNewValueAdmin { get; set; }
        public int? ChildApprovalStatusMasterID { get; set; }
        public string ChildComments { get; set; }
        public DateTime? ChildCreatedDateTime { get; set; }
        public string ChildCreatedBy { get; set; }
        public string ChildFieldDbColumnName { get; set; }
        public int ChildRadioSelect { get; set; }
    }
}