using System;

namespace HRMS.Models
{
    public class TravelShowStatus
    {
        public int? TravelShowstatusEmployeeId { get; set; }

        public int? ShowstatusTravelId { get; set; }

        public int? Travelstagecheck { get; set; }

        public string TravelstagecheckName { get; set; }

        public string TravelShowstatusEmployeeCode { get; set; }

        public string TravelShowstatusEmployeeName { get; set; }

        public int? TravelShowstatusStageID { get; set; }

        public string TravelShowstatusCurrentStage { get; set; }

        public string showStatus { get; set; }

        public DateTime? TravelShowstatusTime { get; set; }

        public string TravelShowstatusActor { get; set; }

        public string TravelShowstatusAction { get; set; }

        public string TravelShowstatusComments { get; set; }
    }
}