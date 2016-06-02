using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOL
{
    [Serializable]
    public class InterviewFeedbackBOL
    {
        private int interviewID;
        private string skills;
        private string rating;
        private int id;
        private int scheduleID;
        private int languageProficiency;
        private int compliance;
        private string currentProjectKnowledge;
        private string overallComments;
        private string candidateRiskProfile;
        private int candidateID;
        private int rrfno;
        private int stageID;
        private int feedbackby;
    

        public int StageID
        {
            get { return stageID; }
            set { stageID = value; }
        }

        public int RRFNo
        {
            get { return rrfno; }
            set { rrfno = value; }
        }
        
        public int CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
        }

        public string CandidateRiskProfile
        {
            get { return candidateRiskProfile; }
            set { candidateRiskProfile = value; }
        }
        
        public string OverallComments
        {
            get { return overallComments; }
            set { overallComments = value; }
        }
               
        public string CurrentProjectKnowledge
        {
            get { return currentProjectKnowledge; }
            set { currentProjectKnowledge = value; }
        }
        
        public int Compliance
        {
            get { return compliance; }
            set { compliance = value; }
        }
        
        public int LanguageProficiency
        {
            get { return languageProficiency; }
            set { languageProficiency = value; }
        }
       
        public int ScheduleID
        {
            get { return scheduleID; }
            set { scheduleID = value; }
        }

        public int InterviewID
        {
            get { return interviewID; }
            set { interviewID = value; }
        }

        public string Skills
        {
            get { return skills; }
            set { skills = value; }
        }

        public string Rating
        {
            get { return rating; }
            set { rating = value; }
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public int FeedbackBy
        {
            get { return feedbackby; }
            set { feedbackby = value; }
        }
    }
}
