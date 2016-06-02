using System;
using System.Collections.Generic;
using V2.Orbit.Model;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Orbit.DataLayer
{
    public class MonthlyLeaveUploadDAL:DBBaseClass
    {
        public DataSet BindData(MonthlyLeaveUploadModel objMonthlyLeaveUploadModel)
        {
            {
                try
                {
                    SqlParameter[] paramAdd = new SqlParameter[1];

                    paramAdd[0] = new SqlParameter("@Year", SqlDbType.Int);
                    paramAdd[0].Value = objMonthlyLeaveUploadModel.LeaveYear;
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetMonthlyLeaveUpload",paramAdd);
                }
                catch (V2Exceptions ex)
                {
                    throw;
                }
                catch (System.Exception ex)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUploadDAL.cs", "BindData", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
            }
        }

        #region AddNewMonthlyLeaveDetails
        public int AddNewMonthlyLeaveDetails(MonthlyLeaveUploadModel objMonthlyLeaveUploadModel)
        {

            SqlParameter[] paramAdd = new SqlParameter[3];

            paramAdd[0] = new SqlParameter("@Year", SqlDbType.Int);
            paramAdd[0].Value = objMonthlyLeaveUploadModel.LeaveYear;
            paramAdd[1] = new SqlParameter("@Month", SqlDbType.VarChar,15);
            paramAdd[1].Value = objMonthlyLeaveUploadModel.LeaveMonth;
            paramAdd[2] = new SqlParameter("@Days", SqlDbType.Float);
            paramAdd[2].Value = objMonthlyLeaveUploadModel.LeaveDays;
            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "AddMonthlyLeaveUpload", paramAdd);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUploadDAL.cs", "AddNewMonthlyLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }
        #endregion

        #region UpdateNewMonthlyLeaveDetails
        public int UpdateNewMonthlyLeaveDetails(MonthlyLeaveUploadModel objMonthlyLeaveUploadModel)
        {
            SqlParameter[] paramAdd = new SqlParameter[4];
            paramAdd[0] = new SqlParameter("@UploadYearID", SqlDbType.Int);
            paramAdd[0].Value = objMonthlyLeaveUploadModel.MonthlyLeaveUploadId;
            paramAdd[1] = new SqlParameter("@Year", SqlDbType.Int);
            paramAdd[1].Value = objMonthlyLeaveUploadModel.LeaveYear;
            paramAdd[2] = new SqlParameter("@Month", SqlDbType.VarChar,15);
            paramAdd[2].Value = objMonthlyLeaveUploadModel.LeaveMonth;
            paramAdd[3] = new SqlParameter("@Days", SqlDbType.Float);
            paramAdd[3].Value = objMonthlyLeaveUploadModel.LeaveDays;
            
            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateMonthlyLeaveUpload", paramAdd);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUploadDAL.cs", "UpdateNewMonthlyLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

    }
}
