using System;
using System.Collections.Generic;
using System.Text;
using V2.Orbit.Model;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Orbit.DataLayer
{
    [Serializable]
    public class LeaveTransactionDAL : DBBaseClass
    {
        #region AddLeaveTransactionDetails
        public int AddLeaveTransactionDetails(LeaveTransactionModel objLeaveTransDetailsModel)
        {
            SqlParameter[] objParam = new SqlParameter[6];
            objParam[0] = new SqlParameter("@UserID", objLeaveTransDetailsModel.UserID);
            objParam[1] = new SqlParameter("@TransactionDate", objLeaveTransDetailsModel.TransactionDate);
            objParam[2] = new SqlParameter("@Description", objLeaveTransDetailsModel.Description);
            objParam[3] = new SqlParameter("@Quantity", objLeaveTransDetailsModel.Quantity);
            objParam[4] = new SqlParameter("@LeaveDetailID", objLeaveTransDetailsModel.LeaveDetailsID);
            objParam[5] = new SqlParameter("@LeaveType", objLeaveTransDetailsModel.LeaveType);

            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "AddLeaveTransactionDeatils", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("Already leave applied for this dates.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "AddLeaveTransactionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region AddCompensationTransactionDetails
        public int AddCompensationTransactionDetails(LeaveTransactionModel objLeaveTransDetailsModel)
        {
            SqlParameter[] objParam = new SqlParameter[6];
            objParam[0] = new SqlParameter("@UserID", objLeaveTransDetailsModel.UserID);
            objParam[1] = new SqlParameter("@TransactionDate", objLeaveTransDetailsModel.TransactionDate);
            objParam[2] = new SqlParameter("@Description", objLeaveTransDetailsModel.Description);
            objParam[3] = new SqlParameter("@Quantity", objLeaveTransDetailsModel.Quantity);
            objParam[4] = new SqlParameter("@CompensationID", objLeaveTransDetailsModel.CompensationID);
            objParam[5] = new SqlParameter("@LeaveType", objLeaveTransDetailsModel.LeaveType);

            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "AddLeaveTransactionDeatilsProc", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("Already leave applied for this dates.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "AddCompensationTransactionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region DeleteLeaveTransactionDetails
        public int DeleteLeaveTransactionDetails(LeaveTransactionModel objLeaveTransDetailsModel)
        {
            SqlParameter[] objParam = new SqlParameter[1];

            objParam[0] = new SqlParameter("@LeaveDetailID", objLeaveTransDetailsModel.LeaveDetailsID);

            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeleteLeaveTransaction", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "DeleteLeaveTransactionDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion


        #region DeleteCompensationTransactionDetails
        public int DeleteCompensationTransactionDetails(LeaveTransactionModel objLeaveTransDetailsModel)
        {
            SqlParameter[] objParam = new SqlParameter[2];

            objParam[0] = new SqlParameter("@CompensationID", objLeaveTransDetailsModel.CompensationID);
            objParam[1] = new SqlParameter("@UserID", objLeaveTransDetailsModel.UserID);

            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeleteCompensationTransaction", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "DeleteCompensationTransactionDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion

        #region GetMaxLeaveDetailID
        public DataSet GetMaxLeaveDetailID()
        {
            DataSet GetMaxLeaveDetailID;


            try
            {

                return GetMaxLeaveDetailID = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetMAxLeaveDetailID");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "GetMaxLeaveDetailID", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion

        #region UpdateLeaveTransactionDetails
        public int UpdateLeaveTransactionDetails(LeaveTransactionModel objLeaveTransDetailsModel)
        {
            SqlParameter[] objParam = new SqlParameter[6];
            objParam[0] = new SqlParameter("@UserID", objLeaveTransDetailsModel.UserID);
            objParam[1] = new SqlParameter("@TransactionDate", objLeaveTransDetailsModel.TransactionDate);
            objParam[2] = new SqlParameter("@Description", objLeaveTransDetailsModel.Description);
            objParam[3] = new SqlParameter("@Quantity", objLeaveTransDetailsModel.Quantity);
            objParam[4] = new SqlParameter("@LeaveDetailID", objLeaveTransDetailsModel.LeaveDetailsID);
            objParam[5] = new SqlParameter("@LeaveType", objLeaveTransDetailsModel.LeaveType);
            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateLeaveTransactionDeatils", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "UpdateLeaveTransactionDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
        public int UpdateLeaveTransactionDetailsForFuture(LeaveTransactionModel objLeaveTransDetailsModel)
        {
            SqlParameter[] objParam = new SqlParameter[7];
            objParam[0] = new SqlParameter("@UserID", objLeaveTransDetailsModel.UserID);
            objParam[1] = new SqlParameter("@TransactionDate", objLeaveTransDetailsModel.TransactionDate);
            objParam[2] = new SqlParameter("@Description", objLeaveTransDetailsModel.Description);
            objParam[3] = new SqlParameter("@Quantity", objLeaveTransDetailsModel.Quantity);
            objParam[4] = new SqlParameter("@LeaveDetailID", objLeaveTransDetailsModel.LeaveDetailsID);
            objParam[5] = new SqlParameter("@LeaveType", objLeaveTransDetailsModel.LeaveType);
            objParam[6] = new SqlParameter("@LeaveTransactionID", objLeaveTransDetailsModel.LeaveTransactionID);
            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateLeaveTransactionDetailsForFuture", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "UpdateLeaveTransactionDetailsForFuture", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
        #endregion
        // Leave Transaction Report


        // LeaveTransactionReport
        #region SearchLeaveTransactionRpt
        public DataSet SearchLeaveTransactionRpt(LeaveTransactionModel objLeaveTransactionModel, bool IsAdmin, bool AllTeammembers)
        {
            DataSet dsSearchLeaveTransactionRpt;
            SqlParameter[] objSqlParam = new SqlParameter[10];


            objSqlParam[0] = new SqlParameter("@UserId", SqlDbType.Int);
            objSqlParam[0].Value = objLeaveTransactionModel.UserID;

            objSqlParam[1] = new SqlParameter("@period", SqlDbType.NVarChar);
            objSqlParam[1].Value = objLeaveTransactionModel.Period;

            objSqlParam[2] = new SqlParameter("@LeaveTypeID", SqlDbType.Int);
            objSqlParam[2].Value = objLeaveTransactionModel.LeaveTypeID;

            if (objLeaveTransactionModel.FromDate.ToString() != "1/1/0001 12:00:00 AM")
            {

                objSqlParam[3] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                objSqlParam[3].Value = objLeaveTransactionModel.FromDate;
            }
            else
            {
                objSqlParam[3] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                objSqlParam[3].Value = null;
            }
            if (objLeaveTransactionModel.ToDate.ToString() != "1/1/0001 12:00:00 AM")
            {
                objSqlParam[4] = new SqlParameter("@Todate", SqlDbType.DateTime);
                objSqlParam[4].Value = objLeaveTransactionModel.ToDate;
            }
            else
            {
                objSqlParam[4] = new SqlParameter("@Todate", SqlDbType.DateTime);
                objSqlParam[4].Value = null;
            }

            objSqlParam[5] = new SqlParameter("@Month", SqlDbType.NVarChar);
            objSqlParam[5].Value = objLeaveTransactionModel.Month;

            objSqlParam[6] = new SqlParameter("@Year", SqlDbType.NVarChar);
            objSqlParam[6].Value = objLeaveTransactionModel.Year;

            objSqlParam[7] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
            objSqlParam[7].Value = IsAdmin;

            objSqlParam[8] = new SqlParameter("@AllTeammembers", SqlDbType.Bit);
            objSqlParam[8].Value = AllTeammembers;

            objSqlParam[9] = new SqlParameter("@ShiftID", SqlDbType.Int);
            objSqlParam[9].Value = objLeaveTransactionModel.ShiftID;


            try
            {
                dsSearchLeaveTransactionRpt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchLeaveTransactionRpt", objSqlParam);
                return dsSearchLeaveTransactionRpt;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "SearchLeaveTransactionRpt", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }

        }
        #endregion

        // Leave Transaction Admin

        #region dsGetLeaveTransaction
        public DataSet dsGetLeaveTransaction(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                SqlParameter[] objSqlParam = new SqlParameter[1];
                objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlParam[0].Value = objLeaveTransactionModel.UserID;

                DataSet dsGetLeaveTransaction = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetLeaveTransaction", objSqlParam);
                return dsGetLeaveTransaction;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "drGetLeaveTransaction", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public DataSet GetLeaveTransactionForspecificLeave(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                SqlParameter[] objSqlParam = new SqlParameter[2];
                objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlParam[0].Value = objLeaveTransactionModel.UserID;
                objSqlParam[1] = new SqlParameter("@LeaveDetailsID", SqlDbType.Int);
                objSqlParam[1].Value = objLeaveTransactionModel.LeaveDetailsID;

                DataSet dsGetLeaveTransaction = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetLeaveTransactionForspecificLeave", objSqlParam);
                return dsGetLeaveTransaction;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "GetLeaveTransactionForspecificLeave", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public DataSet GetLeaveTransactionForFuture(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                SqlParameter[] objSqlParam = new SqlParameter[2];
                objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlParam[0].Value = objLeaveDetailsModel.UserID;
                objSqlParam[1] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                objSqlParam[1].Value = objLeaveDetailsModel.LeaveDateTo;

                DataSet dsGetLeaveTransactionForFuture = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetLeaveTransactionForFuture", objSqlParam);
                return dsGetLeaveTransactionForFuture;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "drGetLeaveTransaction", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
        #endregion

        #region AddLeaveTransactionAdmin
        public int AddLeaveTransactionAdmin(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "AddLeaveTransactionAdmin";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 20000;
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("@UserID", objLeaveTransactionModel.UserID);
                    cmd.Parameters.AddWithValue("@TransactionDate", objLeaveTransactionModel.TransactionDate);
                    cmd.Parameters.AddWithValue("@Description", objLeaveTransactionModel.Description);
                    cmd.Parameters.AddWithValue("@Quantity", objLeaveTransactionModel.Quantity);
                    cmd.Parameters.AddWithValue("@LeaveType", objLeaveTransactionModel.LeaveType);
                    cmd.Parameters.AddWithValue("@TransactionMode", objLeaveTransactionModel.TransactionMode);
                    //cmd.ExecuteNonQuery();
                    int retVal = cmd.ExecuteNonQuery();
                    return retVal;

                }
                //SqlParameter[] objParam = new SqlParameter[6];
                //objParam[0] = new SqlParameter("@UserID", SqlDbType.BigInt);
                //objParam[0].Value = objLeaveTransactionModel.UserID;

                //objParam[1] = new SqlParameter("@TransactionDate", SqlDbType.DateTime);
                //objParam[1].Value = objLeaveTransactionModel.TransactionDate;

                //objParam[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                //objParam[2].Value = objLeaveTransactionModel.Description;

                //objParam[3] = new SqlParameter("@Quantity", SqlDbType.Decimal);
                //objParam[3].Value = objLeaveTransactionModel.Quantity;

                //objParam[4] = new SqlParameter("@LeaveType", SqlDbType.Bit);
                //objParam[4].Value = objLeaveTransactionModel.LeaveType;

                //objParam[5] = new SqlParameter("@TransactionMode", SqlDbType.Bit);
                //objParam[5].Value = objLeaveTransactionModel.TransactionMode;

                //    return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "AddLeaveTransactionAdmin", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo(" have no leave balance.So not allowed to enter the Negative values.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "AddLeaveTransactionAdmin", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region UpdateLeaveTransactionAdmin
        public int UpdateLeaveTransactionAdmin(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "UpdateLeaveTransactionAdmin";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 20000;
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("@LeaveTransactionID", objLeaveTransactionModel.UserID);
                    cmd.Parameters.AddWithValue("@TransactionDate", objLeaveTransactionModel.TransactionDate);
                    cmd.Parameters.AddWithValue("@Description", objLeaveTransactionModel.Description);
                    cmd.Parameters.AddWithValue("@Quantity", objLeaveTransactionModel.Quantity);
                    //cmd.ExecuteNonQuery();
                    int retVal = cmd.ExecuteNonQuery();
                    return retVal;

                }
            }

            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "UpdateLeaveTransactionAdmin", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);

            }

        }
        #endregion

        #region DeleteLeaveTransactionAdmin
        public int DeleteLeaveTransactionAdmin(LeaveTransactionModel objLeaveTransactionModel)
        {
            SqlParameter[] objParam = new SqlParameter[1];
            objParam[0] = new SqlParameter("@LeaveTransactionID", SqlDbType.Int);
            objParam[0].Value = objLeaveTransactionModel.LeaveTransactionID;
            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeleteLeaveTransactionAdmin", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("As Leave balance going negative not allowed to delete this entry.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "DeleteLeaveTransactionAdmin", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }

            }


        }
        #endregion

        #region SearchLeaveTransactionAdmin
        public DataSet SearchLeaveTransactionAdmin(LeaveTransactionModel objLeaveTransactionModel)
        {
            DataSet dsSearchLeaveTransactionAdmin;
            SqlParameter[] objParam = new SqlParameter[2];
            objParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            objParam[0].Value = objLeaveTransactionModel.UserID;

            objParam[1] = new SqlParameter("@LeaveTypeID", SqlDbType.Int);
            objParam[1].Value = objLeaveTransactionModel.LeaveTypeID;

            try
            {
                dsSearchLeaveTransactionAdmin = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchLeaveTransactionAdmin", objParam);
                return dsSearchLeaveTransactionAdmin;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "DeleteLeaveTransactionAdmin", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }

        }
        #endregion

        #region SearchLeaveTransactiondatewise

        public DataSet SearchLeaveTransactiondatewise(LeaveTransactionModel objLeaveTransactionModel)
        {
            DataSet dsSearchLeaveTransactiondatewise;
            SqlParameter[] objParam = new SqlParameter[4];

            objParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            objParam[0].Value = objLeaveTransactionModel.UserID;

            objParam[1] = new SqlParameter("@LeaveTypeID", SqlDbType.Int);
            objParam[1].Value = objLeaveTransactionModel.LeaveTypeID;

            if (objLeaveTransactionModel.FromDate.ToString() != "1/1/0001 12:00:00 AM")
            {

                objParam[2] = new SqlParameter("@Fromdate", SqlDbType.DateTime);
                objParam[2].Value = objLeaveTransactionModel.FromDate;
            }
            else
            {
                objParam[2] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                objParam[2].Value = null;
            }

            if (objLeaveTransactionModel.ToDate.ToString() != "1/1/0001 12:00:00 AM")
            {
                objParam[3] = new SqlParameter("@Todate", SqlDbType.DateTime);
                objParam[3].Value = objLeaveTransactionModel.ToDate;
            }
            else
            {
                objParam[3] = new SqlParameter("@Todate", SqlDbType.DateTime);
                objParam[3].Value = null;
            }

            try
            {
                dsSearchLeaveTransactiondatewise = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchLeaveTransactiondatewise", objParam);
                return dsSearchLeaveTransactiondatewise;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "DeleteLeaveTransactionAdmin", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }

        }
        #endregion

        #region GetTotalLeave
        public DataSet GetTotalLeave(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                DataSet dsGetTotalLeave;
                SqlParameter[] objSqlParam = new SqlParameter[1];
                objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlParam[0].Value = objLeaveTransactionModel.UserID;

                dsGetTotalLeave = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "TotalLeave", objSqlParam);

                return dsGetTotalLeave;
            }

            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "GetTotalLeave", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }

        }
        #endregion

        #region CheckEmployeeNameValidation
        public int CheckEmployeeNameValidation(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                int rowsReturned;
                SqlParameter[] objSqlParam = new SqlParameter[1];

                objSqlParam[0] = new SqlParameter("@EmployeeName", SqlDbType.VarChar);
                objSqlParam[0].Value = objLeaveTransactionModel.EmployeeName;

                rowsReturned = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckEmployeeName", objSqlParam));
                return rowsReturned;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "CheckEmployeeNameValidation", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }

        }


        #endregion

        public int UpdateLeaveBalance(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                int rowsreturned;
                SqlParameter[] objSqlParam = new SqlParameter[1];
                objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.BigInt);
                objSqlParam[0].Value = objLeaveTransactionModel.UserID;

                rowsreturned = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateLeaveCompBalance", objSqlParam);
                return rowsreturned;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionDAL.cs", "UpdateLeaveBalance", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }

        }
    }
}
