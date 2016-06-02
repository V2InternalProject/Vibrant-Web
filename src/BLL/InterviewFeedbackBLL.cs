using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOL;
using DAL;

namespace BLL
{
    public class InterviewFeedbackBLL
    {

        InterviewFeedbackDAL InterviewFeedbackDAL = new InterviewFeedbackDAL();
        DataSet dsInterviewerfeedback = new DataSet();

        public DataSet GetLatestInterviewCoreSkillID()
        {
            dsInterviewerfeedback = InterviewFeedbackDAL.GetLatestInterviewCoreSkillID();
            return dsInterviewerfeedback;

        }

        public DataSet GetInterviewCoreSkillsDetails(InterviewFeedbackBOL objInterviewFeedbackBOL)
        {
            dsInterviewerfeedback = InterviewFeedbackDAL.GetInterviewCoreSkillsDetails(objInterviewFeedbackBOL);
            return dsInterviewerfeedback;
        }
        
        public DataSet GetDetails(InterviewFeedbackBOL objInterviewFeedbackBOL)
        {
            dsInterviewerfeedback = InterviewFeedbackDAL.Getdetails(objInterviewFeedbackBOL);
            return dsInterviewerfeedback;
        }

        public DataSet AddInterviewFeedbackDetails(InterviewFeedbackBOL objInterviewFeedbackBOL)
        {
            dsInterviewerfeedback = InterviewFeedbackDAL.AddInterviewFeedbackDetails(objInterviewFeedbackBOL);
            return dsInterviewerfeedback;
        }

        public DataSet GetInterviewFeedbackID(InterviewFeedbackBOL objInterviewFeedbackBOL)
        {
            dsInterviewerfeedback = InterviewFeedbackDAL.GetInterviewFeedbackID(objInterviewFeedbackBOL);
            return dsInterviewerfeedback;
        }

        public DataSet AddInterviewCoreSkillsDetails(InterviewFeedbackBOL objInterviewFeedbackBOL)
        {

            dsInterviewerfeedback = InterviewFeedbackDAL.AddInterviewCoreSkillsDetails(objInterviewFeedbackBOL);
            return dsInterviewerfeedback;
        
        }

        public DataSet UpdateCoreSkillsDetails(int ID, InterviewFeedbackBOL objInterviewFeedbackBOL)
        {

            dsInterviewerfeedback = InterviewFeedbackDAL.UpdateCoreSkillsDetails(ID, objInterviewFeedbackBOL);
            return dsInterviewerfeedback;
       
        }

        public DataSet DeleteCoreSkillsDetails(int ID)
        {
            dsInterviewerfeedback = InterviewFeedbackDAL.DeleteCoreSkillsDetails(ID);
            return dsInterviewerfeedback;
       
        }

        public DataSet GetSkills(string prefixText)
        {
            dsInterviewerfeedback = InterviewFeedbackDAL.GetSkills(prefixText);
            return dsInterviewerfeedback;
        }

        public DataSet GetinterviewFeedbackDetailsForCandidate(InterviewFeedbackBOL objInterviewFeedbackBOL)
        {
            dsInterviewerfeedback = InterviewFeedbackDAL.GetinterviewFeedbackDetailsForCandidate(objInterviewFeedbackBOL);
            return dsInterviewerfeedback;
        }

        public DataSet UpdateRejectStatus(InterviewFeedbackBOL objInterviewFeedbackBOL)
        {
            dsInterviewerfeedback = InterviewFeedbackDAL.UpdateRejectStatus(objInterviewFeedbackBOL);
            return dsInterviewerfeedback;
        }

        public  DataSet GetDetailsformail(InterviewFeedbackBOL InterviewFeedbackBOL)
        {
            dsInterviewerfeedback = InterviewFeedbackDAL.GetDetailsformail(InterviewFeedbackBOL);
            return dsInterviewerfeedback;
        }
    }
}
