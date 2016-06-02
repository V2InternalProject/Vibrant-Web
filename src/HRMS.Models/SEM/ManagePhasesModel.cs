using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class ManagePhasesModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int ProjectPhaseId { get; set; }

        public int? ProjectId { get; set; }

        public int? OrderNumber { get; set; }

        public string Phases { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public double? WorkHours { get; set; }

        public int? PeakTeamSize { get; set; }

        public int? ResponsiblePerson { get; set; }

        public string ResponsiblePersonGridName { get; set; }

        public bool Currentphase { get; set; }

        public double? PercentageEfforts { get; set; }

        public int projectnameId { get; set; }
        public List<ProjectAppList> ProjectApprovedList = new List<ProjectAppList>();
    }

    public class ProjectAppList
    {
        public int? Projectids { get; set; }
        public string ProjectName { get; set; }
        public int projectIdList { get; set; }
        public string ProjectStatus { get; set; }
    }
}