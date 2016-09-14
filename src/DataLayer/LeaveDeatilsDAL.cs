using System;
using System.Collections.Generic;
using System.Text;
using V2.Orbit.Model;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
//using V2.ApplicationBlocks.Data;

namespace V2.Orbit.DataLayer
{
     [Serializable]
    public class LeaveDeatilsDAL : DBBaseClass
    {
        #region AddLeaveDetails
        public SqlDataReader AddLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[10];
                objParam[0] = new SqlParameter("@UserID", objLeaveDetailsModel.UserID);
                objParam[1] = new SqlParameter("@LeaveDateFrom", objLeaveDetailsModel.LeaveDateFrom);
                objParam[2] = new SqlParameter("@LeaveDateTo", objLeaveDetailsModel.LeaveDateTo);
                objParam[3] = new SqlParameter("@LeaveReason", objLeaveDetailsModel.LeaveResason);
                objParam[4]=  new SqlParameter("@TotalLeaveDays",objLeaveDetailsModel.TotalLeaveDays);
                objParam[5]=  new SqlParameter("@StatusID",objLeaveDetailsModel.StatusID);
                objParam[6]=  new SqlParameter("@ApproverID",objLeaveDetailsModel.ApproverID);
                objParam[7]=  new SqlParameter("@ApproverComments",objLeaveDetailsModel.ApproverComments);
                objParam[8]=  new SqlParameter("@RequestedOn",objLeaveDetailsModel.RequestedOn);
                objParam[9] = new SqlParameter("@LeaveCorrectionDays", objLeaveDetailsModel.LeaveCorrectionDays);
                
            
                return  SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "AddLeaveDeatils", objParam);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "AddLeaveDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region UpdateLeaveDetails
        public int UpdateLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[10];
                objParam[0] = new SqlParameter("@LeaveDetailsID", objLeaveDetailsModel.LeaveDetailsID);
                objParam[1] = new SqlParameter("@LeaveDateFrom", objLeaveDetailsModel.LeaveDateFrom);
                objParam[2] = new SqlParameter("@LeaveDateTo", objLeaveDetailsModel.LeaveDateTo);
                objParam[3] = new SqlParameter("@LeaveReason", objLeaveDetailsModel.LeaveResason);
                objParam[4] = new SqlParameter("@TotalLeaveDays", objLeaveDetailsModel.TotalLeaveDays);
                objParam[5] = new SqlParameter("@LeaveCorrectionDays", objLeaveDetailsModel.LeaveCorrectionDays);
                objParam[6] = new SqlParameter("@UserID", objLeaveDetailsModel.UserID);
                objParam[7] = new SqlParameter("@StatusID", objLeaveDetailsModel.StatusID);
                objParam[8] = new SqlParameter("@ApproverComments", objLeaveDetailsModel.ApproverComments);
                objParam[9] = new SqlParameter("@RequestedOn", objLeaveDetailsModel.RequestedOn);
            //objParam[9] = new SqlParameter("@WorkFlowID", objLeaveDetailsModel.WorkFlowID);

                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateLeaveDetails", objParam);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "UpdateLeaveDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        //public int UpdateLeaveDetailsForFuture(LeaveDetailsModel objLeaveDetailsModel)
        //{
        //    try
        //    {               
        //        SqlParameter[] objParam = new SqlParameter[4];
        //        objParam[0] = new SqlParameter("@UserID", objLeaveDetailsModel.UserID);
        //        objParam[1] = new SqlParameter("@@LeaveDetailsID", objLeaveDetailsModel.LeaveDetailsID);
        //        objParam[2] = new SqlParameter("@TotalLeaveDays", objLeaveDetailsModel.TotalLeaveDays);
        //        objParam[3] = new SqlParameter("@LeaveCorrectionDays", objLeaveDetailsModel.LeaveCorrectionDays);
        //        //objParam[1] = new SqlParameter("@LeaveDateFrom", objLeaveDetailsModel.LeaveDateFrom);
        //        //objParam[2] = new SqlParameter("@LeaveDateTo", objLeaveDetailsModel.LeaveDateTo);
        //        //objParam[1] = new SqlParameter("@SatusID", objLeaveDetailsModel.StatusID);
        //        return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateLeaveDetailsForFuture", objParam);
              
        //    }
        //    catch (V2Exceptions ex)
        //    {
        //        throw;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "UpdateLeaveDetailsForFuture", ex.StackTrace);
        //        throw new V2Exceptions(ex.ToString(),ex);
        //    }
        //}
        #endregion

        #region GetLeaveDetails

         public DataSet WFGetLeaveDetails(int leaveDetailsId)
         {
             try
            {
                  DataSet GetLeaveDetails;
            SqlParameter[] objParam = new SqlParameter[1];
            objParam[0] = new SqlParameter("@LeaveDetailID",leaveDetailsId );

               GetLeaveDetails=SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "WFGetLeaveDetails",objParam);
                 return GetLeaveDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "WFGetLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

         }


        public DataSet GetLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                DataSet GetLeaveDetails;
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@UserID", objLeaveDetailsModel.UserID);
                //objParam[1] = new SqlParameter("@LeaveDateFrom", objLeaveDetailsModel.LeaveDateFrom);
                //objParam[2] = new SqlParameter("@LeaveDateTo", objLeaveDetailsModel.LeaveDateTo);
                //objParam[1] = new SqlParameter("@SatusID", objLeaveDetailsModel.StatusID);
                 
                return GetLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetLeaveDetailsProc", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "GetLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet GetLeaveDetailsForFuture(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                DataSet GetLeaveDetailsForFuture;
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@UserID", objLeaveDetailsModel.UserID);
                objParam[1] = new SqlParameter("@ToDate", objLeaveDetailsModel.LeaveDateTo);
                //objParam[1] = new SqlParameter("@LeaveDateFrom", objLeaveDetailsModel.LeaveDateFrom);
                //objParam[2] = new SqlParameter("@LeaveDateTo", objLeaveDetailsModel.LeaveDateTo);
                //objParam[1] = new SqlParameter("@SatusID", objLeaveDetailsModel.StatusID);

                return GetLeaveDetailsForFuture = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetLeaveDetailsForFuture", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "GetLeaveDetailsForFuture", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        public void wfUpdateLeaveBalance()
        {           
            try
            {

                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "WFGetAndSetLeaveBalance");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "GetLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }


        #endregion

        #region GetTeamMembersLeaveDetails
        public DataSet GetTeamMembersLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                DataSet GetTeamMembersLeaveDetails;
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@UserID", objLeaveDetailsModel.UserID);
                objParam[1] = new SqlParameter("@StatusID", objLeaveDetailsModel.StatusID);
                            
                return GetTeamMembersLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetLeaveDetails", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "GetLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region UpdateTMLeaveDetails
        public int UpdateTMLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[10];
                objParam[0] = new SqlParameter("@LeaveDetailsID", objLeaveDetailsModel.LeaveDetailsID);
                objParam[1] = new SqlParameter("@LeaveDateFrom", objLeaveDetailsModel.LeaveDateFrom);
                objParam[2] = new SqlParameter("@LeaveDateTo", objLeaveDetailsModel.LeaveDateTo);
                objParam[3] = new SqlParameter("@LeaveReason", objLeaveDetailsModel.LeaveResason);
                objParam[4] = new SqlParameter("@TotalLeaveDays", objLeaveDetailsModel.TotalLeaveDays);
                objParam[5] = new SqlParameter("@LeaveCorrectionDays", objLeaveDetailsModel.LeaveCorrectionDays);
                objParam[6] = new SqlParameter("@ApproverID", objLeaveDetailsModel.ApproverID);
                objParam[7] = new SqlParameter("@ApproverComments", objLeaveDetailsModel.ApproverComments);
                //objParam[8] = new SqlParameter("@RequestedOn", objLeaveDetailsModel.RequestedOn);
                objParam[8] = new SqlParameter("@StatusID", objLeaveDetailsModel.StatusID);
                objParam[9] = new SqlParameter("@UserID", objLeaveDetailsModel.UserID);

                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateTMLeaveDetails", objParam);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "UpdateLeaveDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region GetReportingTo
        public DataSet GetReportingTo(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                DataSet GetReportingTo;
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@UserID", objLeaveDetailsModel.UserID);
                return GetReportingTo = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetReportingToProc", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "GetReportingToProc", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region GetStatusDetails
        public DataSet GetStatusDetails()
        {
            DataSet GetStatusDetails;
            //SqlParameter[] objParam = new SqlParameter[1];

            //objParam[0] = new SqlParameter("@StatusID", objLeaveDetailsModel.StatusID);

            try
            {

                return GetStatusDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetStatus");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "GetStatusDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion
         



        #region SearchLeaveDetails
        public DataSet SearchLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                DataSet dsSearchLeaveDetails;
                SqlParameter[] objSqlparam = new SqlParameter[4];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objLeaveDetailsModel.UserID;

                objSqlparam[1] = new SqlParameter("@LeaveDateFrom", SqlDbType.DateTime);
                objSqlparam[1].Value = objLeaveDetailsModel.LeaveDateFrom;

                objSqlparam[2] = new SqlParameter("@LeaveDateTo", SqlDbType.DateTime);
                objSqlparam[2].Value = objLeaveDetailsModel.LeaveDateTo;

                objSqlparam[3] = new SqlParameter("@StatusID", SqlDbType.Int);
                objSqlparam[3].Value = objLeaveDetailsModel.StatusID;

           
                dsSearchLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchLeaveDetails", objSqlparam);
                return dsSearchLeaveDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "SearchLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchAllLeaveDetails
        public DataSet SearchAllLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                DataSet dsSearchLeaveDetails;
                SqlParameter[] objSqlparam = new SqlParameter[2];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objLeaveDetailsModel.UserID;
               
                objSqlparam[1] = new SqlParameter("@StatusID", SqlDbType.Int);
                objSqlparam[1].Value = objLeaveDetailsModel.StatusID;

           
                dsSearchLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchLeaveDetailsProc", objSqlparam);
                return dsSearchLeaveDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "SearchAllLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchAllTMLeaveDetails
        public DataSet SearchAllTMLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        {

            try
            {
                DataSet dsSearchLeaveDetails;
                SqlParameter[] objSqlparam = new SqlParameter[2];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objLeaveDetailsModel.UserID;

                objSqlparam[1] = new SqlParameter("@StatusID", SqlDbType.Int);
                objSqlparam[1].Value = objLeaveDetailsModel.StatusID;

                dsSearchLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchTMDetailsProc", objSqlparam);
                return dsSearchLeaveDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "SearchAllTMLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchTeamMembersLeaveDetails
        public DataSet SearchTeamMembersLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        { 
            try
            {

                DataSet dsSearchLeaveDetails;
                SqlParameter[] objSqlparam = new SqlParameter[4];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objLeaveDetailsModel.UserID;

                objSqlparam[1] = new SqlParameter("@LeaveDateFrom", SqlDbType.DateTime);
                objSqlparam[1].Value = objLeaveDetailsModel.LeaveDateFrom;

                objSqlparam[2] = new SqlParameter("@LeaveDateTo", SqlDbType.DateTime);
                objSqlparam[2].Value = objLeaveDetailsModel.LeaveDateTo;

                objSqlparam[3] = new SqlParameter("@StatusID", SqlDbType.Int);
                objSqlparam[3].Value = objLeaveDetailsModel.StatusID;

           
                dsSearchLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchTMLeaveDetails", objSqlparam);
                return dsSearchLeaveDetails;
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

        #region TotalLeaves
        public DataSet TotalLeaves(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                DataSet TotalLeaves;
                SqlParameter[] objSqlparam = new SqlParameter[2];
                            
                objSqlparam[0] = new SqlParameter("@LeaveDateFrom", SqlDbType.DateTime);
                objSqlparam[0].Value = objLeaveDetailsModel.LeaveDateFrom;

                objSqlparam[1] = new SqlParameter("@LeaveDateTo", SqlDbType.DateTime);
                objSqlparam[1].Value = objLeaveDetailsModel.LeaveDateTo;

           
            
                TotalLeaves = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "TotalLeaves", objSqlparam);
                return TotalLeaves;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "SearchLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region GetLeaveBalance
        public DataSet GetLeaveBalance(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                DataSet GetLeaveBalance;
                SqlParameter[] objSqlparam = new SqlParameter[1];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objLeaveDetailsModel.UserID;
                //objSqlparam[1] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                //objSqlparam[1].Value = objLeaveDetailsModel.LeaveDateTo;
                return GetLeaveBalance = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetAvailableLeaveBalance", objSqlparam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "GetLeaveBalance", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region UpdateCancelLeaveDetails
        public int UpdateCancelLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@LeaveDetailsID", objLeaveDetailsModel.LeaveDetailsID);
                objParam[1] = new SqlParameter("@UserID", objLeaveDetailsModel.UserID);
                objParam[2] = new SqlParameter("@StatusID", objLeaveDetailsModel.StatusID);
                objParam[3] = new SqlParameter("@RequestedOn", objLeaveDetailsModel.RequestedOn);
           
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateCancelLeaveDetails", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "UpdateCancelLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region UpdateLeaveBalance
        public int UpdateLeaveBalance(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
               
                objParam[0] = new SqlParameter("@UserID", objLeaveDetailsModel.UserID);
                objParam[1] = new SqlParameter("@TotalLeaveDays", objLeaveDetailsModel.TotalLeaveDays);
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateLeaveBalance", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "UpdateLeaveBalance", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public int UpdateLeaveDetailsForFuture(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@UserID", objLeaveDetailsModel.UserID);
                objParam[1] = new SqlParameter("@LeaveDetailsID", objLeaveDetailsModel.LeaveDetailsID);
                objParam[2] = new SqlParameter("@TotalLeaveDays", objLeaveDetailsModel.TotalLeaveDays);
                objParam[3] = new SqlParameter("@LeaveCorrectionDays", objLeaveDetailsModel.LeaveCorrectionDays);
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateLeaveDetailsForFuture", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "UpdateLeaveDetailsForFuture", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region GetTotalLeaveBalance
        public DataSet TotalLeaveBalance(LeaveDetailsModel objLeaveDetailsModel)
        {

            try
            {
                DataSet TotalLeaveBalance;
                SqlParameter[] objSqlparam = new SqlParameter[2];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objLeaveDetailsModel.UserID;
                objSqlparam[1] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                objSqlparam[1].Value = objLeaveDetailsModel.LeaveDateTo;

                return TotalLeaveBalance = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetLeaveBalance", objSqlparam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "LeaveBalance", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

         // Leave Report

        public DataSet SearchLeaveRpt(LeaveDetailsModel objLeaveDetailsModel, bool IsAdmin, bool AllTeammembers)

        {
             try
            {
                DataSet dsSearchLeaveRpt;
                SqlParameter[] objSqlParam = new SqlParameter[10];

                objSqlParam[0] = new SqlParameter("@UserId", SqlDbType.Int);
                objSqlParam[0].Value = objLeaveDetailsModel.UserID;

                objSqlParam[1] = new SqlParameter("@period", SqlDbType.NVarChar);
                objSqlParam[1].Value = objLeaveDetailsModel.Period;


                objSqlParam[2] = new SqlParameter("@StatusId", SqlDbType.Int);
                objSqlParam[2].Value = objLeaveDetailsModel.StatusID ;

                if (objLeaveDetailsModel.LeaveDateFrom.ToString() != "1/1/0001 12:00:00 AM")
                {

                    objSqlParam[3] = new SqlParameter("@LeaveDateFrom", SqlDbType.DateTime);
                    objSqlParam[3].Value = objLeaveDetailsModel.LeaveDateFrom;
                }
                else
                {
                    objSqlParam[3] = new SqlParameter("@LeaveDateFrom", SqlDbType.DateTime);
                    objSqlParam[3].Value = null;
                }
                if (objLeaveDetailsModel.LeaveDateTo.ToString() != "1/1/0001 12:00:00 AM")
                {
                    objSqlParam[4] = new SqlParameter("@LeaveDateTo", SqlDbType.DateTime);
                    objSqlParam[4].Value = objLeaveDetailsModel.LeaveDateTo;
                }
                else
                {
                    objSqlParam[4] = new SqlParameter("@LeaveDateTo", SqlDbType.DateTime);
                    objSqlParam[4].Value = null;
                }

                objSqlParam[5] = new SqlParameter("@Month", SqlDbType.NVarChar);
                objSqlParam[5].Value = objLeaveDetailsModel.Month;

                objSqlParam[6] = new SqlParameter("@Year", SqlDbType.NVarChar);
                objSqlParam[6].Value = objLeaveDetailsModel.Year;

                objSqlParam[7] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                objSqlParam[7].Value = IsAdmin;

                objSqlParam[8] = new SqlParameter("@AllTeammembers", SqlDbType.Bit);
                objSqlParam[8].Value = AllTeammembers;

                objSqlParam[9] = new SqlParameter("@ShiftID", SqlDbType.Int);
                objSqlParam[9].Value = objLeaveDetailsModel.ShiftID;
           
                dsSearchLeaveRpt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchLeaveRpt", objSqlParam);
                return dsSearchLeaveRpt;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "SearchOutOfOfficeRpt", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }

        #region SummaryReport

        public DataSet SummaryReport(LeaveDetailsModel objLeaveDetailsModel, bool IsAdmin, bool AllTeammembers)
        {
            try
            {
                DataSet dsSearchLeaveRpt=new DataSet();
               // SqlParameter[] objSqlParam = new SqlParameter[8];

               // objSqlParam[0] = new SqlParameter("@UserId", SqlDbType.Int);
               // objSqlParam[0].Value = objLeaveDetailsModel.UserID;

               // objSqlParam[1] = new SqlParameter("@period", SqlDbType.NVarChar);
               // objSqlParam[1].Value = objLeaveDetailsModel.Period;

               // if (objLeaveDetailsModel.LeaveDateFrom.ToString() != "1/1/0001 12:00:00 AM")
               // {

               //     objSqlParam[2] = new SqlParameter("@SummaryDateFrom", SqlDbType.DateTime);
               //     objSqlParam[2].Value = objLeaveDetailsModel.LeaveDateFrom;
               // }
               // else
               // {
               //     objSqlParam[2] = new SqlParameter("@SummaryDateFrom", SqlDbType.DateTime);
               //     objSqlParam[2].Value = null;
               // }
               // if (objLeaveDetailsModel.LeaveDateTo.ToString() != "1/1/0001 12:00:00 AM")
               // {
               //     objSqlParam[3] = new SqlParameter("@SummarDateTo", SqlDbType.DateTime);
               //     objSqlParam[3].Value = objLeaveDetailsModel.LeaveDateTo;
               // }
               // else
               // {
               //     objSqlParam[3] = new SqlParameter("@SummarDateTo", SqlDbType.DateTime);
               //     objSqlParam[3].Value = null;
               // }

               // objSqlParam[4] = new SqlParameter("@Month", SqlDbType.NVarChar);
               // objSqlParam[4].Value = objLeaveDetailsModel.Month;

               // objSqlParam[5] = new SqlParameter("@Year", SqlDbType.NVarChar);
               // objSqlParam[5].Value = objLeaveDetailsModel.Year;

               // objSqlParam[6] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
               // objSqlParam[6].Value = IsAdmin;

               // objSqlParam[7] = new SqlParameter("@AllTeammembers", SqlDbType.Bit);
               // objSqlParam[7].Value = AllTeammembers;

            
               //dsSearchLeaveRpt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SummaryReport", objSqlParam);
               //// dsSearchLeaveRpt = V2.ApplicationBlocks.Data.V2SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SummaryReport", objSqlParam);
//-----------------------------------------------------------------
               SqlConnection con = new SqlConnection(ConnectionString);
               SqlCommand cmd = new SqlCommand();
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.CommandText = "SummaryReport";
               cmd.CommandTimeout = 6000;
               //cmd.Parameters.Add(sqlParams);
               cmd.Parameters.Add("@UserID", SqlDbType.Int);
               cmd.Parameters["@UserID"].Value = objLeaveDetailsModel.UserID;

             
             
               if (objLeaveDetailsModel.LeaveDateFrom.ToString() != "1/1/0001 12:00:00 AM")
               {

                   cmd.Parameters.Add("@SummaryDateFrom", SqlDbType.DateTime);
                   cmd.Parameters["@SummaryDateFrom"].Value = objLeaveDetailsModel.LeaveDateFrom;
               }
               else
               {
                   cmd.Parameters.Add("@SummaryDateFrom", SqlDbType.DateTime);
                   cmd.Parameters["@SummaryDateFrom"].Value = null;
               }

                if (objLeaveDetailsModel.LeaveDateTo.ToString() != "1/1/0001 12:00:00 AM")
                {
                    cmd.Parameters.Add("@SummarDateTo", SqlDbType.DateTime);
                    cmd.Parameters["@SummarDateTo"].Value = objLeaveDetailsModel.LeaveDateTo;
                    
                }
                else
                {
                    cmd.Parameters.Add("@SummarDateTo", SqlDbType.DateTime);
                    cmd.Parameters["@SummarDateTo"].Value = null;                    
                }
                cmd.Parameters.Add("@period", SqlDbType.NVarChar);
                cmd.Parameters["@period"].Value = objLeaveDetailsModel.Period;

                cmd.Parameters.Add("@Month", SqlDbType.NVarChar);
                cmd.Parameters["@Month"].Value = objLeaveDetailsModel.Month;

                cmd.Parameters.Add("@Year", SqlDbType.NVarChar);
                cmd.Parameters["@Year"].Value = objLeaveDetailsModel.Year;

               cmd.Parameters.Add("@IsAdmin", SqlDbType.Bit);
               cmd.Parameters["@IsAdmin"].Value = IsAdmin;

               cmd.Parameters.Add("@AllTeammembers", SqlDbType.Bit);
               cmd.Parameters["@AllTeammembers"].Value = AllTeammembers;

               cmd.Connection = con;

               SqlDataAdapter da = new SqlDataAdapter(cmd);

               da.Fill(dsSearchLeaveRpt);

            // ------------------------
                return dsSearchLeaveRpt;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "SummaryReport", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }
        #endregion

        #region CheckLeaveDetails

        public int CheckLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                SqlParameter[] objSqlparam = new SqlParameter[3];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objLeaveDetailsModel.UserID;

                objSqlparam[1] = new SqlParameter("@LeaveDateFrom", SqlDbType.DateTime);
                objSqlparam[1].Value = objLeaveDetailsModel.LeaveDateFrom;

                objSqlparam[2] = new SqlParameter("@LeaveDateTo", SqlDbType.DateTime);
                objSqlparam[2].Value = objLeaveDetailsModel.LeaveDateTo;
            
                int x = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckLeaveDetails", objSqlparam));
                return x;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "CheckSignInForCompensation", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region AddingBonusLeaves
        public DataSet AddingBonusLeaves(LeaveDetailsModel objLeaveDetailsModel)
        {
             try
            {
                DataSet AddingBonusLeaves;
                SqlParameter[] objSqlparam = new SqlParameter[2];

                objSqlparam[0] = new SqlParameter("@Month", SqlDbType.NVarChar);
                objSqlparam[0].Value = objLeaveDetailsModel.Month;

                objSqlparam[1] = new SqlParameter("@Totalleaves", SqlDbType.Float);
                objSqlparam[1].Value = objLeaveDetailsModel.TotalLeaveDays;

                return AddingBonusLeaves = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "AddingBonusLeaves", objSqlparam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "AddingBonusLeaves", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region GetBulkLeaveDetails
        public DataSet GetBulkLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                DataSet GetBulkLeaveDetails;
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@UserID", objLeaveDetailsModel.UserID);
                 return GetBulkLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetBulkLeaveDetails", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "GetLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchBulkLeaveDetails
        public DataSet SearchBulkLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                DataSet SearchBulkLeaveDetails;
                SqlParameter[] objSqlparam = new SqlParameter[3];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objLeaveDetailsModel.UserID;

                objSqlparam[1] = new SqlParameter("@LeaveDateFrom", SqlDbType.DateTime);
                objSqlparam[1].Value = objLeaveDetailsModel.LeaveDateFrom;

                objSqlparam[2] = new SqlParameter("@LeaveDateTo", SqlDbType.DateTime);
                objSqlparam[2].Value = objLeaveDetailsModel.LeaveDateTo;
            
                SearchBulkLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchBulkLeaveDetails", objSqlparam);
                return SearchBulkLeaveDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "SearchLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region CheckLeavesinSignIn
        public DataSet CheckLeavesinSignIn(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                DataSet CheckLeavesinSignIn;
                SqlParameter[] objSqlparam = new SqlParameter[3];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objLeaveDetailsModel.UserID;

                objSqlparam[1] = new SqlParameter("@LeaveDateFrom", SqlDbType.DateTime);
                objSqlparam[1].Value = objLeaveDetailsModel.LeaveDateFrom;

                objSqlparam[2] = new SqlParameter("@LeaveDateTo", SqlDbType.DateTime);
                objSqlparam[2].Value = objLeaveDetailsModel.LeaveDateTo;
            
                CheckLeavesinSignIn = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "CheckLeavesinSignIn", objSqlparam);
                return CheckLeavesinSignIn;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "SearchLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region GetCancelLeaveDetails
        public DataSet GetCancelLeaveDetails(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                DataSet GetCancelLeaveDetails;
                SqlParameter[] objSqlparam = new SqlParameter[2];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objLeaveDetailsModel.UserID;
                objSqlparam[1] = new SqlParameter("@LeaveDetailsID", SqlDbType.Int);
                objSqlparam[1].Value = objLeaveDetailsModel.LeaveDetailsID;

                return GetCancelLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetCancelLeaveDetails", objSqlparam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "GetCancelLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region UpdateEmployeeLeaveAndComp
        public int UpdateEmployeeLeaveAndComp(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@UserID", objLeaveDetailsModel.UserID);
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateLeaveCompBalance", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {


                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "UpdateEmployeeLeaveAndComp", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
               
            }
        }

        #endregion


        #region  Leave Anomaly Report

        public DataSet LeaveAnomalyReport(string Month, string Year)
        {

            try
            {
                DataSet LeaveAnomalyDetails = new DataSet();
               // SqlParameter[] sqlParam = new SqlParameter[2];

                //sqlParam[0] = new SqlParameter("@Month", Month);
                //sqlParam[1] = new SqlParameter("@Year", Year);

                ////As the SP  may take more time to give the output so using the V2SqlHelper
                ////In V2SqlHelper the command time out is mentined to 3600 sec (1hr).
                ////return LeaveAnomalyDetails = V2.ApplicationBlocks.Data.V2SqlHelper.ExecuteDataset(ConnectionString,CommandType.StoredProcedure, "LeaveAnomalyReport", sqlParam);
                //return LeaveAnomalyDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "LeaveAnomalyReport", sqlParam);
                DataSet dsGetAttendanceReport = new DataSet();
                // SqlParameter[] sqlParams = new SqlParameter[5];
                //sqlParams[0] = new SqlParameter("@EmployeeId", SqlDbType.Int);
                //sqlParams[0].Value = objAttendanceReportModel.UserId;


                //sqlParams[1] = new SqlParameter("@month", SqlDbType.VarChar, 15);
                //sqlParams[1].Value = objAttendanceReportModel.Month;


                //sqlParams[2] = new SqlParameter("@year", SqlDbType.VarChar, 10);
                //sqlParams[2].Value = objAttendanceReportModel.Year;

                //sqlParams[3] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                //sqlParams[3].Value = IsAdmin;


                //sqlParams[4] = new SqlParameter("@AllTeammembers", SqlDbType.Bit);
                //sqlParams[4].Value = AllTeammembers ;

                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "LeaveAnomalyReport";
                cmd.CommandTimeout = 6000;
                //cmd.Parameters.Add(sqlParams);
              
                cmd.Parameters.Add("@month", SqlDbType.VarChar);
                cmd.Parameters["@month"].Value = Month;

                cmd.Parameters.Add("@year", SqlDbType.VarChar);
                cmd.Parameters["@year"].Value = Year;

               

                cmd.Connection = con;
               
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(LeaveAnomalyDetails);

                return LeaveAnomalyDetails;

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "LeaveAnomalyReport", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region Admin Approval

        public DataSet GetLeaveForAdminApproval(int StatusID, string FromDate, string ToDate)
        {
            try
            {
                DataSet dsCompensatorydetails = new DataSet();

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@StatusID", StatusID);
                param[1] = new SqlParameter("@LeaveDateFrom", FromDate);
                param[2] = new SqlParameter("@LeaveDateTo", ToDate);

                return dsCompensatorydetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetLeaveForAdminApproval", param);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "GetLeaveForAdminApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }



        #endregion

        #region Check Leave Balance For Given Date
        /// <summary>
        /// Added by Rahul
        /// Description: Not to allow leaves if there is no enough leave balance.
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        /// 
        public DataSet CheckLeaveBalanceForGivenDate(int EmployeeCode, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                DataSet dsLeaveBalanceForGivenDate = new DataSet();
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "CheckLeaveBalanceForGivenDate";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 5000;
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                    cmd.Parameters.AddWithValue("@FromDate", FromDate);
                    cmd.Parameters.AddWithValue("@ToDate", ToDate);

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dsLeaveBalanceForGivenDate);
                }

                return dsLeaveBalanceForGivenDate;
                //SqlParameter[] param = new SqlParameter[3];
                //param[0] = new SqlParameter("@EmployeeCode", EmployeeCode);
                //param[1] = new SqlParameter("@FromDate", FromDate);
                //param[2] = new SqlParameter("@ToDate", ToDate);

                //return dsLeaveBalanceForGivenDate = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "CheckLeaveBalanceForGivenDate", param);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "CheckLeaveBalanceForGivenDate", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion
     
     }
}
