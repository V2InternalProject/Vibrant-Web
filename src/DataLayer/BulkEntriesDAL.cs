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
    public class BulkEntriesDAL : DBBaseClass
    {
        #region GetBulkEntries
        public DataSet GetBulkEntries(SignInSignOutModel objSignInSignOutModel)
        {
            DataSet GetBulkEntries;
            SqlParameter[] objParam = new SqlParameter[1];
            objParam[0] = new SqlParameter("@EmployeeID", objSignInSignOutModel.EmployeeID);
            try
            {

                return GetBulkEntries = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetBulkEntries", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntriesDAL.cs", "GetBulkEntries", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }       

        #endregion

        #region AddBulkEntriesDetails
        //public SqlDataReader AddBulkEntriesDetails(SignInSignOutModel objSignInSignOutModel)
        //{
        //    SqlParameter[] objParam = new SqlParameter[11];
        //    objParam[0] = new SqlParameter("@UserID", objSignInSignOutModel.EmployeeID);
        //    objParam[1] = new SqlParameter("@SignInTime", objSignInSignOutModel.SignInTime);
        //    objParam[2] = new SqlParameter("@SignOutTime", objSignInSignOutModel.SignOutTime);
        //    objParam[3] = new SqlParameter("@TotalWorkedHours", objSignInSignOutModel.TotalHoursWorked);
        //    objParam[4] = new SqlParameter("@IsSignInManual", objSignInSignOutModel.IsSignInManual);
        //    objParam[5] = new SqlParameter("@IsSignOutManual", objSignInSignOutModel.IsSignOutManual);
        //    objParam[6] = new SqlParameter("@SignInComments", objSignInSignOutModel.SignInComment);
        //    objParam[7] = new SqlParameter("@ApproverID", objSignInSignOutModel.ApproverID);
        //    objParam[8] = new SqlParameter("@ApproverComments", objSignInSignOutModel.ApproverComments);
        //    //objParam[9] = new SqlParameter("@ReportingTime", objSignInSignOutModel.ReportingTime);
        //    objParam[9] = new SqlParameter("@StatusID", objSignInSignOutModel.StatusID);
        //    objParam[10] = new SqlParameter("@IsBulk", objSignInSignOutModel.IsBulk);


        //    try
        //    {
        //        return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "AddBulkEntriesDetails", objParam);
        //    }
        //    catch (V2Exceptions ex)
        //    {
        //        throw;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        if (ex.Message.CompareTo("Already leave applied for this dates.") != 0)
        //        {
        //            FileLog objFileLog = FileLog.GetLogger();
        //            objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntriesDAL.cs", "AddBulkEntriesDetails", ex.StackTrace);
        //            throw new V2Exceptions(ex.ToString(),ex);
        //        }
        //        else
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        public int AddBulkEntriesDetails(SignInSignOutModel objSignInSignOutModel)
        {
            SqlParameter[] objParam = new SqlParameter[11];
            objParam[0] = new SqlParameter("@UserID", objSignInSignOutModel.EmployeeID);
            objParam[1] = new SqlParameter("@SignInTime", objSignInSignOutModel.SignInTime);
            objParam[2] = new SqlParameter("@SignOutTime", objSignInSignOutModel.SignOutTime);
            objParam[3] = new SqlParameter("@TotalWorkedHours", objSignInSignOutModel.TotalHoursWorked);
            objParam[4] = new SqlParameter("@IsSignInManual", objSignInSignOutModel.IsSignInManual);
            objParam[5] = new SqlParameter("@IsSignOutManual", objSignInSignOutModel.IsSignOutManual);
            objParam[6] = new SqlParameter("@SignInComments", objSignInSignOutModel.SignInComment);
            objParam[7] = new SqlParameter("@ApproverID", objSignInSignOutModel.ApproverID);
            objParam[8] = new SqlParameter("@ApproverComments", objSignInSignOutModel.ApproverComments);
            //objParam[9] = new SqlParameter("@ReportingTime", objSignInSignOutModel.ReportingTime);
            objParam[9] = new SqlParameter("@StatusID", objSignInSignOutModel.StatusID);
            objParam[10] = new SqlParameter("@IsBulk", objSignInSignOutModel.IsBulk);


            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "AddBulkEntriesDetails", objParam);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntriesDAL.cs", "AddBulkEntriesDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region GetBulkEntriesForEmployees
        public DataSet GetBulkEntriesForEmployees(SignInSignOutModel objSignInSignOutModel)
        {
            DataSet GetBulkEntries;
            SqlParameter[] objParam = new SqlParameter[1];
            objParam[0] = new SqlParameter("@EmployeeID", objSignInSignOutModel.EmployeeID);
            try
            {

                return GetBulkEntries = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetBulkEntriesForEmployees", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntriesDAL.cs", "GetBulkEntriesForEmployees", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region DeleteSignInSignOutDetails
        public int DeleteSignInSignOutDetails(SignInSignOutModel objSignInSignOutModel)
        {
            SqlParameter[] ParamDelete = new SqlParameter[1];

            ParamDelete[0] = new SqlParameter("@SignInSignOutID", Convert.ToInt32(objSignInSignOutModel.SignInSignOutID.ToString()));


            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeleteSignInSignOutDetails", ParamDelete);

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntriesDAL.cs", "DeleteSignInSignOutDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchBulkDetails
        public DataSet SearchBulkDetails(SignInSignOutModel objSignInSignOutModel)
        {
            DataSet dsSearchBulkDetails;
            SqlParameter[] objSqlparam = new SqlParameter[3];

            objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            objSqlparam[0].Value = objSignInSignOutModel.EmployeeID;

            objSqlparam[1] = new SqlParameter("@SignIn", SqlDbType.DateTime);
            objSqlparam[1].Value = objSignInSignOutModel.SignInTime;

            objSqlparam[2] = new SqlParameter("@SignOut", SqlDbType.DateTime);
            objSqlparam[2].Value = objSignInSignOutModel.SignOutTime;
           

            try
            {
                dsSearchBulkDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchBulkDetails", objSqlparam);
                return dsSearchBulkDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "SearchTeamMembersLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

    }
}
