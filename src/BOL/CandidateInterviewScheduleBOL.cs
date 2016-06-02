using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOL
{
    public class CandidateInterviewScheduleBOL
    {
        public string Position { get; set; }
        public int RRFNo { get; set; }
        
        public DateTime PostedDate { get; set; }
        public string Requestor { get; set; }
        public string InitiateFor { get; set; }
        public string CandidateName { get; set; }
        public string Stage { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public int InterviewerName { get; set; }
        public string RecruiterName { get; set; }
        public int CandidateID { get; set; }
        public int ScheduledBy { get; set; }
        public string RescheduleReason { get; set; }
        public int RoundNo { get; set; }
        
    }
}
