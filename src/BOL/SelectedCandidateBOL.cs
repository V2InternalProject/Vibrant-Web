using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOL
{
    public class SelectedCandidateBOL
    {
        public int  RRFNo { get; set; }
        public int CandidateID { get; set; }

        public int StageID {get;set;}
		public int Designation {get;set;}
		public int Action {get;set;}
		public decimal CTC  {get;set;}
		public string Other1  {get;set;}
		public string Other2  {get;set;}
		public DateTime  JoiningDate  {get;set;}
		public int ProbationPeriod  {get;set;}
		public string SelectedComment  {get;set;}
		public string Grade  {get;set;}
		public int OfferedPosition  {get;set;}
        public int OfferedEmployementType { get; set; }
        public int FeedBackBy { set; get; }
        public int ScheduleId { set; get; }
        

    }
}
