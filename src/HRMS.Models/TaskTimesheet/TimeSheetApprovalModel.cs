using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class TimeSheetApprovalModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? SelectedStartDate { get; set; }
        public DateTime? SelectedEndDate { get; set; }
        public int? SelectedProjectID { get; set; }
        public int? SelectedResourceID { get; set; }
        public int? SelectedStatus { get; set; }
        public List<ProjectList> ProjectList = new List<ProjectList>();
        public List<ResourceList> ResourceList = new List<ResourceList>();
        public List<StatusList> StatusList = new List<StatusList>();
    }

    public class TimeSheetApprovalDetailsModel
    {
        public int TimeSheetID { get; set; }
        public int? ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int? ResourceID { get; set; }
        public string ResourceName { get; set; }
        public DateTime? Date { get; set; }
        public string Task { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? Hours { get; set; }
        public int? Units { get; set; }
        public int? IsApproved { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string ApproverComments { get; set; }
    }

    public class ApproverData
    {
        public int TimeSheetID { get; set; }
        public string ApproverComments { get; set; }
        public int? ResourceID { get; set; }
    }

    //public class ProjectList
    //{
    //    public int ProjectID { get; set; }
    //    public string ProjectName { get; set; }
    //}
    public class ResourceList
    {
        public int ResourceID { get; set; }
        public string ResourceName { get; set; }
    }

    public class StatusList
    {
        public int StatusID { get; set; }
        public string Status { get; set; }
    }

    public class TemplateHandling
    {
        public TemplateHandling(string Key, string value)
        {
            this.Key = Key;
            this.Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class TimeSheetMailTemplate
    {
        public int? EmployeeId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public string Note { get; set; }
        public string Message { get; set; }
    }
}