using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class TimesheetModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public int TimeSheetId { get; set; }
        public int? EmployeeId { get; set; }
        public int? ProjectTaskTypeId { get; set; }

        public List<ProjectList> ProjectListdata { get; set; }
        public List<TaskList> TaskListdata { get; set; }

        public List<TimesheetStatusList> StatusListdata { get; set; }

        [Required(ErrorMessage = "Please select the project first.")]
        public int? ProjectID { get; set; }

        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public bool NewTaskCheckbox { get; set; }
        public string NewTask { get; set; }

        public double? Hours { get; set; }
        public int? Minutes { get; set; }

        public string Description { get; set; }

        public int? Units { get; set; }

        public int? AvgUnitTime { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime? Date { get; set; }

        public int? TagID { get; set; }

        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }

        public string Status { get; set; }
        public int? StatusFilter { get; set; }

        [StringLength(1500, ErrorMessage = "Comments cannot be greater than 1500 characters.")]
        public string Comments { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ApproverComments { get; set; }

        public string selectedProjectIDFilter { get; set; }
        public string selectedTaskDDFilter { get; set; }
        public string selectedStatusDDFilter { get; set; }
        public string selectedFromDateFilter { get; set; }
        public string selectedToDateFilter { get; set; }

        public class ProjectList
        {
            public int? ProjectID { get; set; }
            public string ProjectName { get; set; }
        }

        public class TimesheetStatusList
        {
            public int? StatusID { get; set; }
            public string StatusName { get; set; }
        }

        public class TaskList
        {
            public int ProjectTaskTypeId { get; set; }
            public int? ProjectID { get; set; }
            public string TaskName { get; set; }
            public int? AssignedTo { get; set; }
            public int? AvgUnitTime { get; set; }
            public string Description { get; set; }
            public int TagId { get; set; }
            public string TagName { get; set; }
            public string TagType { get; set; }
        }

        public string HrUnit { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public double ActualHours { get; set; }
        public double PlannedHours { get; set; }
    }
}