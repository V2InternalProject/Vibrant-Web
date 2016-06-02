using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOL
{
   public class HRInterviewAssessmentBOL
    {
        public int  RRFID { get; set; }
        public int CandidateID { get; set; }
        public int ScheduleID { get; set; }
        public int FeedbackBy { get; set; }
        public string DU { get; set; }
        public string Feedback { get; set; }
        public int StageID { get; set; }

       // public int CandidateName { get; set; }
        public int RecruiterName { get; set; }
        public int Department { get; set; }
        public int Position { get; set; }
        public int Experience { get; set; }
        public int RelevantExperience { get; set; }
        public int NoticePeriod { get; set; }
        public int InterviewedBy { get; set; }

        public int Personality { get; set; }
        public int Clarity { get; set; }
        public int Leadership { get; set; }
        public int Interpersonal { get; set; }
        public int Communication { get; set; }
        public int Initiative { get; set; }
        public int Career { get; set; }
        public int RoundNo { get; set; }
        public int SrNo { get; set; }

        public string HRMComments { get; set; }
        public string Mode { get; set; }
   
        //public DateTime  FeedBackDatetime { get; set; } 
   }
}
