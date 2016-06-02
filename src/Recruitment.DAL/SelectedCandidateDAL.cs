using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOL;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace DAL
{
    public class SelectedCandidateDAL
    {

        DataSet dsGetSelectedCandidateRoundDetails = new DataSet();


        public DataSet GetSelectedCandidateRoundDetails(SelectedCandidateBOL objSelectedCandidate)
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.Int);
            param[0].Value = objSelectedCandidate.CandidateID;

            param[1] = new SqlParameter("@RRFNo", SqlDbType.Int );
            param[1].Value = objSelectedCandidate.RRFNo;

            param[2] = new SqlParameter("@FeedBackBy", SqlDbType.Int);
            param[2].Value = objSelectedCandidate.FeedBackBy;

            param[3] = new SqlParameter("@ScheduleId", SqlDbType.Int);
            param[3].Value = objSelectedCandidate.ScheduleId;


            return dsGetSelectedCandidateRoundDetails = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetSelectedCandidateRoundDetails", param);
        }

        public DataSet SetSelectedCandidateDetails(SelectedCandidateBOL objSelectedCandidate)
        {


            SqlParameter[] param = new SqlParameter[13];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.Int);
            param[0].Value = objSelectedCandidate.CandidateID;

            param[1] = new SqlParameter("@RRFNo", SqlDbType.Int);
            param[1].Value = objSelectedCandidate.RRFNo;


            param[2] = new SqlParameter("@StageID", SqlDbType.Int);
            param[2].Value = objSelectedCandidate.StageID;

            


            param[3] = new SqlParameter("@Action", SqlDbType.Int);
            param[3].Value = objSelectedCandidate.Action;

            param[4] = new SqlParameter("@CTC", SqlDbType.Decimal);
            param[4].Value = objSelectedCandidate.CTC;

            //param[5] = new SqlParameter("@other1", SqlDbType.VarChar);
            //param[5].Value = objSelectedCandidate.Other1;

            //param[6] = new SqlParameter("@other2", SqlDbType.VarChar);
            //param[6].Value = objSelectedCandidate.Other2;

            param[5] = new SqlParameter("@JoiningDate", SqlDbType.DateTime);
            param[5].Value = objSelectedCandidate.JoiningDate;

            param[6] = new SqlParameter("@ProbationPeriod", SqlDbType.Int);
            param[6].Value = objSelectedCandidate.ProbationPeriod;



            param[7] = new SqlParameter("@SelectedComment", SqlDbType.VarChar);
            param[7].Value = objSelectedCandidate.SelectedComment;

            param[8] = new SqlParameter("@Grade", SqlDbType.VarChar);
            param[8].Value = objSelectedCandidate.Grade;

            param[9] = new SqlParameter("@OfferedPosition", SqlDbType.Int);
            param[9].Value = objSelectedCandidate.OfferedPosition;


            param[10] = new SqlParameter("@OfferedEmployementType", SqlDbType.Int);
            param[10].Value = objSelectedCandidate.OfferedEmployementType;

            param[11] = new SqlParameter("@FeedBackBy", SqlDbType.Int);
            param[11].Value = objSelectedCandidate.FeedBackBy;

            param[12] = new SqlParameter("@ScheduleId", SqlDbType.Int);
            param[12].Value = objSelectedCandidate.ScheduleId;

            return dsGetSelectedCandidateRoundDetails = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_setSelectedCandidate", param);
        }

        public DataSet GetCandidateScore(SelectedCandidateBOL objSelectedCandidate)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.Int);
            param[0].Value = objSelectedCandidate.CandidateID;

            param[1] = new SqlParameter("@RRFNo", SqlDbType.Int);
            param[1].Value = objSelectedCandidate.RRFNo;

            DataSet dscandidateScore = new DataSet();
            return dscandidateScore =SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateScore", param);
        }

        public DataSet GetGradeDetails()
        {
            DataSet dsGradeName = new DataSet();
            return dsGradeName = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetGradeName");
        }

        public DataSet GetGetEmploymentTypeDetails()
        {
            DataSet dsEmploymentType = new DataSet();
            return dsEmploymentType = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetEmploymentType");
        }

        public DataSet GetDesignation()
        {
            DataSet dsGetDesignation = new DataSet();
            return dsGetDesignation = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetDesignation");
        }


        public DataSet GetDetailsformail(SelectedCandidateBOL objSelectedCandidateBOL)
        {
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[0].Value = objSelectedCandidateBOL.RRFNo;

            param[1] = new SqlParameter("@ScheduleID", SqlDbType.Int);
            param[1].Value = objSelectedCandidateBOL.ScheduleId;

            return dsGetSelectedCandidateRoundDetails = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_getdetailsformailForFeedbackPage", param);
        }
    }
}
