using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class ManagerViewPostModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int projectnameId { get; set; }
        public List<ProjectAppList> ProjectApprovedList = new List<ProjectAppList>();

        public int? HelpDeskTicketID { get; set; }
        public int? EmployeeCode { get; set; }

        public string EmployeeName { get; set; }
        public string ReportingTo { get; set; }
        public string ResourcePool { get; set; }
        public string Designation { get; set; }
        public string ProjectRole { get; set; }
        public string ResourceType { get; set; }
        public string EmploymentStatus { get; set; }
        public string Allocated { get; set; }
        public string ReleaseResource { get; set; }
        public string ProjectEndAppraisalForm { get; set; }
        public string RMGComments { get; set; }

        public DateTime? ReleaseDate { get; set; }
        public DateTime? AllocationStartDate { get; set; }
        public DateTime? AllocationEndDate { get; set; }

        public class ProjectAppList
        {
            public int? Projectids { get; set; }
            public string ProjectName { get; set; }
            public int projectIdList { get; set; }
        }
    }
}