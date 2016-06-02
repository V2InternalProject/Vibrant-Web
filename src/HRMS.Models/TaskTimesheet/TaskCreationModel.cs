using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class TaskCreationModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public int LoggedInEmployeeId { get; set; }
        public int ProjectID { get; set; }
        public List<ProjectList> TaskProjectList { get; set; }
        public int SelectedProjectId { get; set; }
        public int SelectedMileStoneId { get; set; }
        public int SelectedAssignedEmployeeId { get; set; }
        public int SelectedStatusId { get; set; }
        //public AddProjectTask AddProjectTaskDetails { get; set; }

        public int ProjectTaskTypeID { get; set; }

        [Required]
        public string TaskName { get; set; }

        public string ProjectName { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }

        public double? PlannedHours { get; set; }
        public int? AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public string TagID { get; set; }
        public string TagName { get; set; }

        [Required(ErrorMessage = "Status field is required.")]
        public int? StatusID { get; set; }

        public int? StatusIDFilter { get; set; }
        public string StatusValue { get; set; }

        //[Required(ErrorMessage = "Hot Time field is required.")]
        [RegularExpression("^([0-9]+)$", ErrorMessage = "Enter Whole Number only.")]
        public int? AvgUnitTime { get; set; }

        [Required(ErrorMessage = "Task Type field is required.")]
        public int? TaskTypeID { get; set; }

        public string TaskTypeName { get; set; }

        [Required]
        public string Description { get; set; }

        public List<MasterDataModel> TaskStatusList { get; set; }
        public List<MasterDataModel> TaskTypeList { get; set; }
        public List<EmployeeList> AssignedToList { get; set; }
        public List<TagListClass> TagList { get; set; }
        public string AssignedEmployeeName { get; set; }
        public string AddedTagName { get; set; }
        public string SelectedEmployeeList { get; set; }
        public string SelectedTagList { get; set; }
        public string SelectedTaskName { get; set; }
        public int? MileStoneId { get; set; }
        public List<MileStoneListClass> MileStoneList { get; set; }
        public int hdnAssignedEmployeeId { get; set; }
        public int hdnStatusId { get; set; }
        public int? PlannedUnits { get; set; }
        public bool ProjectTaskType { get; set; }
    }

    public class ProjectList
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
    }

    public class AddProjectTask
    {
        public int ProjectTaskTypeID { get; set; }
        public string TaskName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? PlannedHours { get; set; }
        public int? AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public string TagID { get; set; }
        public string TagName { get; set; }
        public int? StatusID { get; set; }
        public string StatusValue { get; set; }
        public int? AvgUnitTime { get; set; }
        public int? TaskTypeID { get; set; }
        public string TaskTypeName { get; set; }
        public string Description { get; set; }
        public decimal? ActualHours { get; set; }
        public int? PlannedUnits { get; set; }
        public bool? ProjectTaskType { get; set; }
        public string ProjectTaskTypeValue { get; set; }
        public List<MasterDataModel> TaskStatusList { get; set; }
        public List<MasterDataModel> TaskTypeList { get; set; }
    }

    public class MasterDataModel
    {
        public int LookUpTypeId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class EmployeeList
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
    }

    public class TagListClass
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
    }

    public class ProjectTaskRespose
    {
        public bool status { get; set; }
        public bool isTaskNameExist { get; set; }
    }

    public class MileStoneListClass
    {
        public int MileStoneId { get; set; }
        public string MileStoneName { get; set; }
        public DateTime? ProjStartDate { get; set; }
        public DateTime? ProjEndDate { get; set; }
    }
}