using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AddEdirResourseModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int ProjectEmployeeRoleID { get; set; }

        public int HelpdeskTicketID { get; set; }

        public string ProjectName { get; set; }

        public int? ProjectID { get; set; }

        public int? RequesterId { get; set; }

        public DateTime? ProjectStartDate { get; set; }

        public DateTime? ProjectEndDate { get; set; }

        [Required(ErrorMessage = "The Employee Name field is required.")]
        public string EmployeeName { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeCode { get; set; }

        public string ReportingTo { get; set; }

        [Required(ErrorMessage = "The Project Role field is required.")]
        public string ProjectRole { get; set; }

        public int RoleID { get; set; }

        [Required(ErrorMessage = "The Resource Type field is required.")]
        public string ResourceType { get; set; }

        public int ResourceTypesID { get; set; }

        public string ResourcePoolName { get; set; }

        [Required(ErrorMessage = "The Allocation Start Date field is required.")]
        public DateTime? AllocationStartDate { get; set; }

        [Required(ErrorMessage = "The Allocation End Date field is required.")]
        public DateTime? AllocationEndDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public DateTime? AllocationOldEndDate { get; set; }

        [Required(ErrorMessage = "The Allocated Percentage field is required.")]
        [Range(0, 100, ErrorMessage = "Allocated Percentage should be in between 0 and 100")]
        public double? AllocatedPercentage { get; set; }

        public string Comments { get; set; }

        public List<Role> EmployeeRole { get; set; }

        public List<ResourseType> ResourseTypes { get; set; }
        public List<EmployeeListDetails> EmployeeList { get; set; }

        public double? totalAllocation { get; set; }

        public SaveAddEditResources SaveStatus { get; set; }

        public DateTime? BulkReallocationDate { get; set; }

        public string ProjectCode { get; set; }

        public int SkillId { get; set; }

        public string Skills { get; set; }

        public int DesignationId { get; set; }

        public string DesignationName { get; set; }

        public int ResourceTypeId { get; set; }

        public int EmploymentStatusId { get; set; }

        public string EmployementStatus { get; set; }

        public string Action { get; set; }

        public string ButtonClick { get; set; }
    }

    public class ResourseType
    {
        public int ResourseTypeID { get; set; }

        public string ResourseTypeDescription { get; set; }
    }

    public class SaveAddEditResources
    {
        public bool CanAllocationDone { get; set; }

        public int RemainingAllocationPercentage { get; set; }

        public bool isAllocationDone { get; set; }

        public string ErrorMessage { get; set; }
    }
}