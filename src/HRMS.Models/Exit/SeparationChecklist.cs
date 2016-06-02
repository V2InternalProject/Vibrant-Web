using System.Collections.Generic;

namespace HRMS.Models
{
    public class SeparationChecklist
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<SeperationChecklistRecord> seperationChecklist { get; set; }
        public int? OrderNo { get; set; }
        public string Stage { get; set; }
        public string Approver { get; set; }
    }

    public class SeperationChecklistRecord
    {
        public int? OrderNo { get; set; }
        public string Stage { get; set; }
        public string Approver { get; set; }
    }
}