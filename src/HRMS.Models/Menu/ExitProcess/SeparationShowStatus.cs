using System;

namespace HRMS.Models
{
    public class SeparationShowStatus
    {
        public int? ShowstatusEmployeeId { get; set; }

        public int? stagecheck { get; set; }

        public string stagecheckname { get; set; }

        public string ShowstatusEmployeeCode { get; set; }

        public string ShowstatusEmployeeName { get; set; }

        public int? ShowstatusStageID { get; set; }

        public string ShowstatusCurrentStage { get; set; }

        public string showStatus { get; set; }

        public DateTime? ShowstatusTime { get; set; }

        public string ShowstatusActor { get; set; }

        public string ShowstatusAction { get; set; }

        public string ShowstatusComments { get; set; }
    }
}