using System.Collections.Generic;

namespace HRMS.Models
{
    public class ResourceIndexModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int HelpdeskTicketID { get; set; }

        public int RMGprojectnameId { get; set; }
        public List<ProjectAppListApproved> ProjectApprovedListdata = new List<ProjectAppListApproved>();

        public long? HelpDeskTicketID { get; set; }
        public long? EmployeeCode { get; set; }

        public string EmployeeName { get; set; }
        public string ReportingTo { get; set; }
        public string ResourcePool { get; set; }
        public string Designation { get; set; }
        public string ProjectRole { get; set; }
        public string ResourceType { get; set; }
        public string EmploymentStatus { get; set; }
        public decimal? Allocated { get; set; }
        public string ReleaseResource { get; set; }
        public string ProjectEndAppraisalForm { get; set; }
        public string RMGComments { get; set; }

        public string ReleaseDate { get; set; }
        public string AllocationStartDate { get; set; }
        public string AllocationEndDate { get; set; }

        public class ProjectAppListApproved
        {
            public int? Projectids { get; set; }
            public string ProjectName { get; set; }
            public int projectIdList { get; set; }
        }
    }
}