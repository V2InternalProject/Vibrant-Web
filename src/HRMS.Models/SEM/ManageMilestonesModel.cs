using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ManageMilestonesModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public TaskClosureComplition TaskClosureComplition { get; set; }
        public List<TaskClosureComplition> TaskClosureComplitionList { get; set; }

        public TaskClosureVoid TaskClosureVoid { get; set; }
        public List<TaskClosureVoid> TaskClosureVoidList { get; set; }

        public List<ProjectAppList> ProjectList { get; set; }

        public string Prj { get; set; }

        public int? MilestoneID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Milestone Name can not be greater than 100 characters.")]
        public string MilestoneName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Milestone Description can not be greater than 100 characters.")]
        public string MilestoneDescription { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        // [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "Milestone Status can not be greater than 100 characters.")]
        public string MilestoneStatus { get; set; }

        public int? HiddenResponsiblePerson { get; set; }

        public int ProjectID { get; set; }

        public string ResponsiblePerson { get; set; }
    }

    public class TaskClosureComplition
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public int? MileStoneID { get; set; }
        public int? TaskID { get; set; }
        public string TaskName { get; set; }
        public int? ProjectId { get; set; }
        public string ResponsiblePerson { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public double? PlannedWork { get; set; }
        public double? ActualWork { get; set; }
        public double? ActualPercentComplete { get; set; }

        public bool TaskClosureComplitionChecked { get; set; }
    }

    public class TaskClosureVoid
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public int? MileStoneID { get; set; }
        public int? TaskID { get; set; }
        public string TaskName { get; set; }
        public int? ProjectId { get; set; }
        public string ResponsiblePerson { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public double? PlannedWork { get; set; }
        public double? ActualWork { get; set; }
        public double? ActualPercentComplete { get; set; }
        public bool TaskClosureVoidChecked { get; set; }
    }
}