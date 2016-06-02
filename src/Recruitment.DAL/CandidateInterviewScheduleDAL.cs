using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using BOL;

namespace DAL
{
    public class CandidateInterviewScheduleDAL
    {

         DataSet dsSDALDataset = new DataSet();

        public DataSet GetRRFValues(CandidateInterviewScheduleBOL mod,int CandidateID)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@RRFNo", SqlDbType.Int);
            param[0].Value = mod.RRFNo;

            param[1] = new SqlParameter("@CandidateID", SqlDbType.Int);
            param[1].Value = CandidateID;

            return dsSDALDataset = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetRRFValues",param);
        }

        public DataSet GetStage(CandidateInterviewScheduleBOL mod)
        {
            return dsSDALDataset = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetStage");
        }

        public DataSet GetCandidateSchedule(int CandidateID, int RRFNo)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.Int);
            param[0].Value = CandidateID;
            param[1] = new SqlParameter("@RRFNo", SqlDbType.Int);
            param[1].Value = @RRFNo;
            return dsSDALDataset = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateSchedule",param);
        }
        public DataSet GetCandidateReScheduleRoundNumber(int CandidateID, int RRFNo, int RoundNo)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.Int);
            param[0].Value = CandidateID;
            param[1] = new SqlParameter("@RRFNo", SqlDbType.Int);
            param[1].Value = @RRFNo;
            param[2] = new SqlParameter("@RoundNo", SqlDbType.Int);
            param[2].Value = @RoundNo;

            return dsSDALDataset = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetRescheduleRoundDetails", param);
        }

        public DataSet GetRoleBasedInterViewer(string prefixText, string RoleName)
        {
            SqlParameter[] param = new SqlParameter[2];
            
            param[0] = new SqlParameter("@RoleName", SqlDbType.VarChar);
            param[0].Value = RoleName;
            param[1] = new SqlParameter("@prefixText", SqlDbType.VarChar);
            param[1].Value = prefixText;


            return dsSDALDataset = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetRoleBasedEmployeeList", param);

        }

        public void SetCandidateSchecule(CandidateInterviewScheduleBOL mod, int RowNumberForReschedule)
        {
            SqlParameter[] param = new SqlParameter[9];

            param[0] = new SqlParameter("@CandidateID", SqlDbType.Int);
            param[0].Value = mod.CandidateID ;

            param[1] = new SqlParameter("@RRFNo", SqlDbType.Int);
            param[1].Value = mod.RRFNo;

            param[2] = new SqlParameter("@Stage", SqlDbType.VarChar);
            param[2].Value = mod.Stage;

            param[3] = new SqlParameter("@ScheduledDateTime", SqlDbType.DateTime);
            param[3].Value = mod.ScheduledDateTime;

            //param[4] = new SqlParameter("@Position", SqlDbType.VarChar);
            //param[4].Value = mod.Position;

            //param[5] = new SqlParameter("@PostedDate", SqlDbType.DateTime);
            //param[5].Value = mod.PostedDate;

            //param[6] = new SqlParameter("@Requestor", SqlDbType.VarChar);
            //param[6].Value = mod.Requestor;

            param[4] = new SqlParameter("@InterviewerName", SqlDbType.VarChar);
            param[4].Value = mod.InterviewerName;

            param[5] = new SqlParameter("@ScheduledBy", SqlDbType.Int);
            param[5].Value = mod.ScheduledBy;


            param[6] = new SqlParameter("@Reschedule", SqlDbType.VarChar);
            param[6].Value = DBNull.Value;

            param[7] = new SqlParameter("@RescheduleReason", SqlDbType.VarChar);
            param[7].Value = DBNull.Value;

            param[8] = new SqlParameter("@RowNumberForReschedule", SqlDbType.Int);
            param[8].Value = RowNumberForReschedule;

            

           SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_SetCandidateSchedule", param);
        }

        //public DataSet Update(Model mod)
        //{
        //    SqlParameter[] param = new SqlParameter[4];

        //    param[0] = new SqlParameter("@Stage", SqlDbType.Int);
        //    param[0].Value = mod.Stage;

        //    param[1] = new SqlParameter("@ScheduledDate", SqlDbType.Date);
        //    param[1].Value = mod.ScheduledDate;

        //    param[2] = new SqlParameter("@ScheduledTime", SqlDbType.VarChar);
        //    param[2].Value = mod.ScheduledTime;

        //    param[3] = new SqlParameter("@InterviewerName", SqlDbType.VarChar);
        //    param[3].Value = mod.InterviewerName;

        //    return dsSDALDataset = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "sp_update_Schedule", param);
        //}



        public DataSet GetDetailsformail(CandidateInterviewScheduleBOL objCandidateInterviewScheduleBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[0].Value = objCandidateInterviewScheduleBOL.RRFNo;

            return dsSDALDataset = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_getdetailsformail", param);

        }
    }

    }
