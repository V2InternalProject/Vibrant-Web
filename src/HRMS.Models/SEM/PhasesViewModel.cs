using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class PhasesViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int ProjectPhaseId { get; set; }

        public int? ProjectId { get; set; }

        [Required(ErrorMessage = "The Order Number field is required.")]
        public int? OrderNumber { get; set; }

        public string Phases { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public double? WorkHours { get; set; }

        public int? PeakTeamSize { get; set; }

        public string ResponsiblePerson { get; set; }

        public bool Currentphase { get; set; }

        [Required(ErrorMessage = "The Percentage Effort field is required.")]
        public float? PercentageEfforts { get; set; }

        [Required(ErrorMessage = "Responsible Person is required.")]
        public int? ResponsiblePerId { get; set; }

        public List<ResponsiblePesons> ResposiblePersoneList { get; set; }

        public string WorkHrs { get; set; }
        public double ActualHrs { get; set; }

        public List<IBPhaseList> IBManagePhasesList { get; set; }

        [Required]
        public int? IBPhaseManageId { get; set; }
    }

    public class IBPhaseList
    {
        public int? PhaseID { get; set; }
        public string PhaseName { get; set; }
    }

    public class ResponsiblePesons
    {
        public int? PersoneID { get; set; }
        public string PersonName { get; set; }
    }
}