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
   public class RescheduledDAL
    {
       RescheduledBOL objRescheduleBOL = new RescheduledBOL();
        DataSet ds = new DataSet();
        public DataSet GetEmployeeName(string prefixText, string RoleName)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@RoleName", SqlDbType.VarChar);
            param[0].Value = RoleName;
            param[1] = new SqlParameter("@prefixText", SqlDbType.VarChar);
            param[1].Value = prefixText;
            return ds = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetRoleBasedEmployeeList", param);
        }

        public DataSet SetCandidateSchecule(RescheduledBOL mod,int RoundNumber)
        {
            SqlParameter[] param = new SqlParameter[9];

            param[0] = new SqlParameter("@CandidateId", SqlDbType.VarChar);
            param[0].Value = mod.CandidateID;

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

            //param[5] = new SqlParameter("@Requestor", SqlDbType.VarChar);
            //param[5].Value = mod.Requestor;

            param[4] = new SqlParameter("@InterviewerName", SqlDbType.VarChar);
            param[4].Value = mod.InterviewerName;

            param[5] = new SqlParameter("@ScheduledBy", SqlDbType.Int);
            param[5].Value = mod.ScheduledBy ;


            param[6] = new SqlParameter("@Reschedule", SqlDbType.VarChar);
            param[6].Value = mod.RescheduledBy;

            param[7] = new SqlParameter("@RescheduleReason", SqlDbType.VarChar);
            param[7].Value = mod.RescheduledReason;

            param[8] = new SqlParameter("@RowNumberForReschedule", SqlDbType.Int);
            param[8].Value = RoundNumber ;

            return ds = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_SetCandidateSchedule", param);
        }

        public DataSet GetDetailsformail(RescheduledBOL objRescheduledBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[0].Value = objRescheduledBOL.RRFNo;

            return ds = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_getdetailsformail", param);

        }
    }
}


