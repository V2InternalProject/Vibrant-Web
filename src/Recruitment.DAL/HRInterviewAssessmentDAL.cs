using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOL;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;

namespace DAL
{        
    public class HRInterviewAssessmentDAL
    {
        DataSet dsCandidateDetails = new DataSet();
        //HRInterviewAssessmentBOL objHRInterviewAssessmentBOL = new HRInterviewAssessmentBOL();


        public DataSet GetCandidateDetails(HRInterviewAssessmentBOL objHRInterviewAssessmentBOL)
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[0].Value = objHRInterviewAssessmentBOL.RRFID;

            param[1] = new SqlParameter("@CandidateID", SqlDbType.Int);
            param[1].Value = objHRInterviewAssessmentBOL.CandidateID;

            param[2] = new SqlParameter("@Mode", SqlDbType.VarChar);
            param[2].Value = objHRInterviewAssessmentBOL.Mode;

            param[3] = new SqlParameter("@ScheduleID", SqlDbType.Int);
            param[3].Value = objHRInterviewAssessmentBOL.ScheduleID;

            dsCandidateDetails = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateInterviewDetails",param);
            return dsCandidateDetails;

        }

        public void HRInterviewAssessment(HRInterviewAssessmentBOL objHRInterviewAssessmentBOL)
        {
          SqlParameter[] param = new SqlParameter[14];

             param[0] = new SqlParameter("@RRFID", SqlDbType.Int);
             param[0].Value = objHRInterviewAssessmentBOL.RRFID;

             param[1] = new SqlParameter("@CandidateID", SqlDbType.Int);
             param[1].Value = objHRInterviewAssessmentBOL.CandidateID;

             param[2] = new SqlParameter("@FeedBackBy", SqlDbType.Int);
             param[2].Value = objHRInterviewAssessmentBOL.FeedbackBy;

             param[3] = new SqlParameter("@Personality", SqlDbType.Int);
             param[3].Value = objHRInterviewAssessmentBOL.Personality;

             param[4] = new SqlParameter("@Clarity", SqlDbType.Int);
             param[4].Value = objHRInterviewAssessmentBOL.Clarity;

             param[5] = new SqlParameter("@Leadership", SqlDbType.Int);
             param[5].Value = objHRInterviewAssessmentBOL.Leadership;

             param[6] = new SqlParameter("@Interpersonal", SqlDbType.Int);
             param[6].Value = objHRInterviewAssessmentBOL.Interpersonal;

             param[7] = new SqlParameter("@Communication", SqlDbType.Int);
             param[7].Value = objHRInterviewAssessmentBOL.Communication ;

             param[8] = new SqlParameter("@Initiative", SqlDbType.Int);
             param[8].Value = objHRInterviewAssessmentBOL.Initiative ;

             param[9] = new SqlParameter("@Career", SqlDbType.Int);
             param[9].Value = objHRInterviewAssessmentBOL.Career;

             param[10] = new SqlParameter("@HRComments", SqlDbType.VarChar);
             param[10].Value = objHRInterviewAssessmentBOL.HRMComments;

             param[11] = new SqlParameter("@ScheduleID", SqlDbType.Int);
             param[11].Value = objHRInterviewAssessmentBOL.ScheduleID;
            
             param[12] = new SqlParameter("@FeedbackMode", SqlDbType.VarChar);
             param[12].Value = objHRInterviewAssessmentBOL.Feedback;

             param[13] = new SqlParameter("@StageID", SqlDbType.Int);
             param[13].Value = objHRInterviewAssessmentBOL.StageID;

             SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_SetHRAssessment", param);

        }
        public void GetUpdateCandidateScheduleDate(HRInterviewAssessmentBOL objHRInterviewAssessmentBOL)
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@RRFNo", SqlDbType.Int);
            param[0].Value = objHRInterviewAssessmentBOL.RRFID;

            param[1] = new SqlParameter("@CandidateID", SqlDbType.Int);
            param[1].Value = objHRInterviewAssessmentBOL.CandidateID;

            param[2] = new SqlParameter("@StageID", SqlDbType.Int);
            param[2].Value = objHRInterviewAssessmentBOL.StageID;

            param[3] = new SqlParameter("@RoundNumber", SqlDbType.Int);
            param[3].Value = objHRInterviewAssessmentBOL.RoundNo;

            //param[3] = new SqlParameter("@RoundNumber", SqlDbType.Int);
            //param[3].Value = objHRInterviewAssessmentBOL.RoundNo;



            param[4] = new SqlParameter("@SrNo", SqlDbType.Int);
            param[4].Value = objHRInterviewAssessmentBOL.SrNo ;
            SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetUpdateCandidateScheduleDate", param);
          //  return dsCandidateDetails;

        }


        public DataSet GetDetailsformail(HRInterviewAssessmentBOL objHRInterviewAssessmentBOL)
        {
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[0].Value = objHRInterviewAssessmentBOL.RRFID;

            param[1] = new SqlParameter("@ScheduleID", SqlDbType.Int);
            param[1].Value = objHRInterviewAssessmentBOL.ScheduleID ;

            return dsCandidateDetails = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_getdetailsformailForFeedbackPage", param);
        }
    }
}
