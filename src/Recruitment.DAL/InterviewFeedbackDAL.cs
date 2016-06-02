using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BOL;
using System.Data.SqlClient;


namespace DAL
{
    public class InterviewFeedbackDAL
    {
        DataSet dsInterviewFeedBack = new DataSet();

        public DataSet GetLatestInterviewCoreSkillID()
        {
            return dsInterviewFeedBack = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetLatestInterview_CoreSkillsID");
        }

        public DataSet GetInterviewCoreSkillsDetails(InterviewFeedbackBOL objInterviewFeedbackBOL)
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.Int);
            param[0].Value = objInterviewFeedbackBOL.CandidateID;

            param[1] = new SqlParameter("@RRFNo", SqlDbType.Int);
            param[1].Value = objInterviewFeedbackBOL.RRFNo;

            param[2] = new SqlParameter("@StageID", SqlDbType.Int);
            param[2].Value = objInterviewFeedbackBOL.StageID;

            param[3] = new SqlParameter("@ScheduleID", SqlDbType.Int);
            param[3].Value = objInterviewFeedbackBOL.ScheduleID;

            try
            {
                return dsInterviewFeedBack = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetInterviewCoreSkillsDetails", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public DataSet Getdetails(InterviewFeedbackBOL objInterviewFeedbackBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@ScheduleID", SqlDbType.Int);
            param[0].Value = objInterviewFeedbackBOL.ScheduleID;

            return dsInterviewFeedBack = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetInterviewFeedbackHeaderDetails", param);
        }

        public DataSet AddInterviewFeedbackDetails(InterviewFeedbackBOL objInterviewFeedbackBOL)
        { 
            SqlParameter[] param = new SqlParameter[10];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.Int);
            param[0].Value = objInterviewFeedbackBOL.CandidateID;

            param[1] = new SqlParameter("@RRFNo", SqlDbType.Int);
            param[1].Value = objInterviewFeedbackBOL.RRFNo;

            param[2] = new SqlParameter("@StageID", SqlDbType.Int);
            param[2].Value = objInterviewFeedbackBOL.StageID ;

            param[3] = new SqlParameter("@LanguageProficiency", SqlDbType.Int);
            if (objInterviewFeedbackBOL.LanguageProficiency != -2147483648)
                param[3].Value = objInterviewFeedbackBOL.LanguageProficiency;
            else
                param[3].Value = DBNull.Value;
           
            param[4] = new SqlParameter("@Compliance", SqlDbType.Int);
            if (objInterviewFeedbackBOL.Compliance != -2147483648)
                param[4].Value = objInterviewFeedbackBOL.Compliance;
            else
                param[4].Value = DBNull.Value;

            param[5] = new SqlParameter("@CurrentProjectKnowledge", SqlDbType.VarChar);
            param[5].Value = objInterviewFeedbackBOL.CurrentProjectKnowledge;

            param[6] = new SqlParameter("@OverallComments", SqlDbType.VarChar);
            param[6].Value = objInterviewFeedbackBOL.OverallComments;

            param[7] = new SqlParameter("@CandidateRiskProfile", SqlDbType.VarChar);
            param[7].Value = objInterviewFeedbackBOL.CandidateRiskProfile;

            param[8] = new SqlParameter("@ScheduleID", SqlDbType.Int);
            param[8].Value = objInterviewFeedbackBOL.ScheduleID;

            param[9] = new SqlParameter("@FeedBackBy", SqlDbType.Int);
            param[9].Value = objInterviewFeedbackBOL.FeedbackBy;

   
            return dsInterviewFeedBack = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_AddInterviewFeedBack", param);
        }

        public DataSet GetInterviewFeedbackID(InterviewFeedbackBOL objInterviewFeedbackBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@InterviewFeedbackID", SqlDbType.Int);
            param[0].Value = objInterviewFeedbackBOL.ID;

            return dsInterviewFeedBack = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetInterviewFeedbackID", param);
        }

        public DataSet AddInterviewCoreSkillsDetails(InterviewFeedbackBOL objInterviewFeedbackBOL)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@InterviewID", SqlDbType.BigInt);
            param[0].Value = objInterviewFeedbackBOL.InterviewID;

            param[1] = new SqlParameter("@Skills", SqlDbType.VarChar);
            param[1].Value = objInterviewFeedbackBOL.Skills;

            param[2] = new SqlParameter("@Rating", SqlDbType.VarChar);
            param[2].Value = objInterviewFeedbackBOL.Rating;

            return dsInterviewFeedBack = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_AddInterviewCoreSkillsDetails", param);
           
        }

        public DataSet UpdateCoreSkillsDetails(int ID, InterviewFeedbackBOL objInterviewFeedbackBOL)
        {
            SqlParameter[] param = new SqlParameter[4];

            param[0] = new SqlParameter("@ID", SqlDbType.Int);
            param[0].Value = ID;

            param[1] = new SqlParameter("@InterviewID", SqlDbType.Int);
            param[1].Value = objInterviewFeedbackBOL.InterviewID;

            param[2] = new SqlParameter("@Skills", SqlDbType.VarChar);
            param[2].Value = objInterviewFeedbackBOL.Skills;

            param[3] = new SqlParameter("@Rating", SqlDbType.VarChar);
            param[3].Value = objInterviewFeedbackBOL.Rating;

            return dsInterviewFeedBack = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_UpdateCoreSkillsDetails", param);

        }

        public DataSet DeleteCoreSkillsDetails(int ID)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@ID", SqlDbType.Int);
            param[0].Value = ID;

            return dsInterviewFeedBack = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_DeleteCoreSkillsDetails", param);

        }

        public DataSet GetSkills(string prefixText)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@prefixText", SqlDbType.VarChar);
            param[0].Value = prefixText;
            return dsInterviewFeedBack = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetskillsForAutosuggest", param);
        }

        public DataSet GetinterviewFeedbackDetailsForCandidate(InterviewFeedbackBOL objInterviewFeedbackBOL)
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.Int);
            param[0].Value = objInterviewFeedbackBOL.CandidateID;

            param[1] = new SqlParameter("@RRFNo", SqlDbType.Int);
            param[1].Value = objInterviewFeedbackBOL.RRFNo;

            param[2] = new SqlParameter("@StageID", SqlDbType.Int);
            param[2].Value = objInterviewFeedbackBOL.StageID;

            param[3] = new SqlParameter("@ScheduleID", SqlDbType.Int);
            param[3].Value = objInterviewFeedbackBOL.ScheduleID;

            return dsInterviewFeedBack = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetinterviewFeedbackDetailsForCandidate", param);
        }

        public DataSet UpdateRejectStatus(InterviewFeedbackBOL objInterviewFeedbackBOL)
        {
            SqlParameter[] param = new SqlParameter[10];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.Int);
            param[0].Value = objInterviewFeedbackBOL.CandidateID;

            param[1] = new SqlParameter("@RRFNo", SqlDbType.Int);
            param[1].Value = objInterviewFeedbackBOL.RRFNo;

            param[2] = new SqlParameter("@StageID", SqlDbType.Int);
            param[2].Value = objInterviewFeedbackBOL.StageID;

            param[3] = new SqlParameter("@LanguageProficiency", SqlDbType.Int);
            if (objInterviewFeedbackBOL.LanguageProficiency != -2147483648)
                param[3].Value = objInterviewFeedbackBOL.LanguageProficiency;
            else
                param[3].Value = DBNull.Value;

            param[4] = new SqlParameter("@Compliance", SqlDbType.Int);
            if (objInterviewFeedbackBOL.Compliance != -2147483648)
                param[4].Value = objInterviewFeedbackBOL.Compliance;
            else
                param[4].Value = DBNull.Value;

            param[5] = new SqlParameter("@CurrentProjectKnowledge", SqlDbType.VarChar);
            param[5].Value = objInterviewFeedbackBOL.CurrentProjectKnowledge;

            param[6] = new SqlParameter("@OverallComments", SqlDbType.VarChar);
            param[6].Value = objInterviewFeedbackBOL.OverallComments;

            param[7] = new SqlParameter("@CandidateRiskProfile", SqlDbType.VarChar);
            param[7].Value = objInterviewFeedbackBOL.CandidateRiskProfile;

            param[8] = new SqlParameter("@ScheduleID", SqlDbType.Int);
            param[8].Value = objInterviewFeedbackBOL.ScheduleID;

            param[9] = new SqlParameter("@FeedBackBy", SqlDbType.Int);
            param[9].Value = objInterviewFeedbackBOL.FeedbackBy;

            return dsInterviewFeedBack = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_UpdateRejectStatus", param);

        }

        public DataSet GetDetailsformail(InterviewFeedbackBOL InterviewFeedbackBOL)
        {
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[0].Value = InterviewFeedbackBOL.RRFNo;

            param[1] = new SqlParameter("@ScheduleID", SqlDbType.Int);
            param[1].Value = InterviewFeedbackBOL.ScheduleID;

            return dsInterviewFeedBack = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_getdetailsformailForFeedbackPage", param);
        }
    }
}
