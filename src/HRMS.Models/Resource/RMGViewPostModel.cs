using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class RMGViewPostModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int RMGprojectnameId { get; set; }

        public int ResourceId { get; set; }

        public int ResourcePoolId { get; set; }

        public List<ResourcePoolList> resourcePoolList = new List<ResourcePoolList>();

        public List<ResourceList> ResourcesList = new List<ResourceList>();

        public List<ProjectAppListApproved> ProjectApprovedListdata = new List<ProjectAppListApproved>();
        public List<Role> ProjectRolesList = new List<Role>();

        public int? ProjectEndAppraisalStausID { get; set; }
        public int? ProjectSkillMatrixStausID { get; set; }
        public int? ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ProjectEmployeeRoleID { get; set; }
        public long? HelpDeskTicketID { get; set; }
        public string EmployeeCode { get; set; }
        public int? EmployeeId { get; set; }
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
        public string PresentAbsent { get; set; }

        public string ReleaseDate { get; set; }
        public string AllocationStartDate { get; set; }
        public string AllocationStartDate1 { get; set; }
        public string AllocationEndDate { get; set; }
        public string AllocationEndDate1 { get; set; }
        public string UnallocatedFrom { get; set; }
        public string PrimarySkills { get; set; }
        public string DesignationName { get; set; }
        public DateTime? AsOnDate { get; set; }
        public string SearchEmployeeName { get; set; }
        public int SearchEmployeeId { get; set; }
        public int? SearchEmployeeCode { get; set; }
        public List<ResourcePoolList> ResourcePoolListData = new List<ResourcePoolList>();
        public List<ResourceList> ResourceListData = new List<ResourceList>();

        public class ProjectAppListApproved
        {
            public int? Projectids { get; set; }
            public string ProjectName { get; set; }
            public int projectIdList { get; set; }
        }

        public class ResourcePoolList
        {
            public int ResourcePoolID { get; set; }
            public string ResourcePoolName { get; set; }
        }

        public class ResourceList
        {
            public int ResourceID { get; set; }
            public string ResourceName { get; set; }
        }
    }
}