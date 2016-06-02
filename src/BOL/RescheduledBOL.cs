using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOL
{
    public class RescheduledBOL
    {

        public string CandidateName { get; set; }
        public int RRFNo { get; set; }
        public string Stage { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public string Position { get; set; }
        public DateTime PostedDate { get; set; }
        public string Requestor { get; set; }
        public int InterviewerName { get; set; }
        public int ScheduledBy { get; set; }
        public string RescheduledBy { get; set; }
        public DateTime RescheduledDate { get; set; }
        public string RescheduledReason { get; set; }
        public int CandidateID { get; set; }

    }
}
