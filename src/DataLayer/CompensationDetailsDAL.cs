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
    public class CompensationDetailsDAL : DBBaseClass
    {
        DataSet dsWeeklyOffDAL = new DataSet();

        #region weekly off for Shift Employee
        public DataSet GetWeeklyOff(CompensationDetailsModel objCompensationDetailsModel)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
            param[0].Value = objCompensationDetailsModel.UserID;
            DateTime nullDate = new DateTime();
            if (objCompensationDetailsModel.CompensationFrom  == nullDate)
                param[1] = new SqlParameter("@StartDate", DBNull.Value);
            else
                param[1] = new SqlParameter("@StartDate", objCompensationDetailsModel.CompensationFrom);

            if (objCompensationDetailsModel.CompensationTo  == nullDate)
                param[2] = new SqlParameter("@EndDate", DBNull.Value);
            else
                param[2] = new SqlParameter("@EndDate", objCompensationDetailsModel.CompensationTo);

            dsWeeklyOffDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetWeekOffDate", param);
            return dsWeeklyOffDAL;
        }
        #endregion

        #region GetCompensationDetails
        public DataSet GetCompensationDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                DataSet GetCompensationDetails;
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@UserID", objCompensationModel.UserID);
                objParam[1] = new SqlParameter("@StatusID", objCompensationModel.StatusID);
                return GetCompensationDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetCompensationDetails", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "GetCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region GetTMCompensationDetails
        public DataSet GetTMCompensationDetails(CompensationDetailsModel objCompensationModel)
        {
           try
            {
               DataSet GetCompensationDetails;
               SqlParameter[] objParam = new SqlParameter[2];
               objParam[0] = new SqlParameter("@UserID", objCompensationModel.UserID);
               objParam[1] = new SqlParameter("@StatusID", objCompensationModel.StatusID);
               return GetCompensationDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetTMCompensationDetails", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "GetTMCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region AddCompenstionDetails
        public SqlDataReader AddCompenstionDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[7];
                objParam[0] = new SqlParameter("@UserID", objCompensationModel.UserID);
                objParam[1] = new SqlParameter("@AppliedFor", objCompensationModel.AppliedFor);           
                objParam[2] = new SqlParameter("@Reason", objCompensationModel.Resason);            
                objParam[3] = new SqlParameter("@StatusID", objCompensationModel.StatusID);
                objParam[4] = new SqlParameter("@ApproverID", objCompensationModel.ApproverID);
                objParam[5] = new SqlParameter("@ApproverComments", objCompensationModel.ApproverComments);
                objParam[6] = new SqlParameter("@RequestedOn", objCompensationModel.RequestedOn);
                return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "AddCompensationDetails", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("Already Compensation applied for this date.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "CompensationDetailsDAL", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region GetMaxCompensationID
        public DataSet GetMaxCompensationID()
        {
            DataSet GetMaxCompensationID;


            try
            {

                return GetMaxCompensationID = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetMaxCompensationID");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "GetMaxCompensationID", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region UpdateCompenstionDetails
        public int UpdateCompenstionDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[6];
                objParam[0] = new SqlParameter("@UserID", objCompensationModel.UserID);
                objParam[1] = new SqlParameter("@AppliedFor", objCompensationModel.AppliedFor);
                objParam[2] = new SqlParameter("@Reason", objCompensationModel.Resason);
                objParam[3] = new SqlParameter("@StatusID", objCompensationModel.StatusID);
                //objParam[4] = new SqlParameter("@ApproverID", objCompensationModel.ApproverID);
                //objParam[5] = new SqlParameter("@ApproverComments", objCompensationModel.ApproverComments);
                objParam[4] = new SqlParameter("@RequestedOn", objCompensationModel.RequestedOn);
                objParam[5] = new SqlParameter("@CompensationID", objCompensationModel.CompensationID);            
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateCompenstionDetails", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("Already Compensation applied for this date.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "UpdateCompenstionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region UpdateApprovalCompenstionDetails
        public int UpdateApprovalCompenstionDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[7];
                objParam[0] = new SqlParameter("@UserID", objCompensationModel.UserID);
                objParam[1] = new SqlParameter("@AppliedFor", objCompensationModel.AppliedFor);
                objParam[2] = new SqlParameter("@Reason", objCompensationModel.Resason);
                objParam[3] = new SqlParameter("@StatusID", objCompensationModel.StatusID);
                //objParam[4] = new SqlParameter("@ApproverID", objCompensationModel.ApproverID);
                objParam[4] = new SqlParameter("@ApproverComments", objCompensationModel.ApproverComments);
                objParam[5] = new SqlParameter("@RequestedOn", objCompensationModel.RequestedOn);
                objParam[6] = new SqlParameter("@CompensationID", objCompensationModel.CompensationID);

                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateApprovalCompenstionDetails", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("Already Compensation applied for this date.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "UpdateCompenstionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region UpdateCancelCompenstionDetails
        public int UpdateCancelCompenstionDetails(CompensationDetailsModel objCompensationModel)
        {
             try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@CompensationID", objCompensationModel.CompensationID);
                objParam[1] = new SqlParameter("@UserID", objCompensationModel.UserID);
                objParam[2] = new SqlParameter("@StatusID", objCompensationModel.StatusID);
                objParam[3] = new SqlParameter("@RequestedOn", objCompensationModel.RequestedOn);
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateCancelCompenstionDetails", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "UpdateCancelCompenstionDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region SearchAllCompensationDetails
        public DataSet SearchAllCompensationDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                DataSet dsSearchLeaveDetails;
                SqlParameter[] objSqlparam = new SqlParameter[2];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objCompensationModel.UserID;

                objSqlparam[1] = new SqlParameter("@StatusID", SqlDbType.Int);
                objSqlparam[1].Value = objCompensationModel.StatusID;
            
                dsSearchLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchAllCompensationDetails", objSqlparam);
                return dsSearchLeaveDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "SearchAllCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchCompensationDetails
        public DataSet SearchCompensationDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                DataSet dsSearchLeaveDetails;
                SqlParameter[] objSqlparam = new SqlParameter[4];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objCompensationModel.UserID;

                objSqlparam[1] = new SqlParameter("@StatusID", SqlDbType.Int);
                objSqlparam[1].Value = objCompensationModel.StatusID;

                objSqlparam[2] = new SqlParameter("@CompensationFrom", SqlDbType.DateTime);
                objSqlparam[2].Value = objCompensationModel.CompensationFrom;

                objSqlparam[3] = new SqlParameter("@CompensationTo", SqlDbType.DateTime);
                objSqlparam[3].Value = objCompensationModel.CompensationTo;
            
                dsSearchLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchCompensationDetails", objSqlparam);
                return dsSearchLeaveDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "SearchAllCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchAllTMCompensationDetails
        public DataSet SearchAllTMCompensationDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                DataSet dsSearchLeaveDetails;
                SqlParameter[] objSqlparam = new SqlParameter[2];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objCompensationModel.UserID;

                objSqlparam[1] = new SqlParameter("@StatusID", SqlDbType.Int);
                objSqlparam[1].Value = objCompensationModel.StatusID;
                dsSearchLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchAllTMCompensationDetails", objSqlparam);
                return dsSearchLeaveDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "SearchAllTMCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchTMCompensationDetails
        public DataSet SearchTMCompensationDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                DataSet dsSearchLeaveDetails;
                SqlParameter[] objSqlparam = new SqlParameter[4];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objCompensationModel.UserID;

                objSqlparam[1] = new SqlParameter("@StatusID", SqlDbType.Int);
                objSqlparam[1].Value = objCompensationModel.StatusID;

                objSqlparam[2] = new SqlParameter("@CompensationFrom", SqlDbType.DateTime);
                objSqlparam[2].Value = objCompensationModel.CompensationFrom;

                objSqlparam[3] = new SqlParameter("@CompensationTo", SqlDbType.DateTime);
                objSqlparam[3].Value = objCompensationModel.CompensationTo;

            
                dsSearchLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchTMCompensationDetails", objSqlparam);
                return dsSearchLeaveDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "SearchTMCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region CheckSignInForCompensation
        public int CheckSignInForCompensation(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                SqlParameter[] objSqlparam = new SqlParameter[2];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objCompensationModel.UserID;                     

                objSqlparam[1] = new SqlParameter("@CompensationTo", SqlDbType.DateTime);
                objSqlparam[1].Value = objCompensationModel.CompensationTo;

            
                int x= Convert.ToInt32( SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckSignInForCompensation", objSqlparam));
                return x; 
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "CheckSignInForCompensation", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchCompensationRpt
        public DataSet SearchCompensationRpt(CompensationDetailsModel objCompensationDetailsModel, bool IsAdmin, bool AllTeammembers)
        {
             try
            {
                DataSet dsSearchCompensationRpt;
                SqlParameter[] objSqlParam = new SqlParameter[10];

                objSqlParam[0] = new SqlParameter("@UserId", SqlDbType.Int);
                objSqlParam[0].Value = objCompensationDetailsModel.UserID;

                objSqlParam[1] = new SqlParameter("@StatusId", SqlDbType.Int);
                objSqlParam[1].Value = objCompensationDetailsModel.StatusID;

                objSqlParam[2] = new SqlParameter("@period", SqlDbType.NVarChar);
                objSqlParam[2].Value = objCompensationDetailsModel.Period;

                if (objCompensationDetailsModel.CompensationFrom.ToString() != "1/1/0001 12:00:00 AM")
                {
                    objSqlParam[3] = new SqlParameter("@CompensationFrom", SqlDbType.DateTime);
                    objSqlParam[3].Value = objCompensationDetailsModel.CompensationFrom;
                }
                else
                {
                    objSqlParam[3] = new SqlParameter("@CompensationFrom", SqlDbType.DateTime);
                    objSqlParam[3].Value = null;
                }

                if (objCompensationDetailsModel.CompensationTo.ToString() != "1/1/0001 12:00:00 AM")
                {
                    objSqlParam[4] = new SqlParameter("@CompensationTo", SqlDbType.DateTime);
                    objSqlParam[4].Value = objCompensationDetailsModel.CompensationTo;
                }
                else
                {
                    objSqlParam[4] = new SqlParameter("@CompensationTo", SqlDbType.DateTime);
                    objSqlParam[4].Value = null;
                }

                objSqlParam[5] = new SqlParameter("@Month", SqlDbType.NVarChar);
                objSqlParam[5].Value = objCompensationDetailsModel.Month;

                objSqlParam[6] = new SqlParameter("@Year", SqlDbType.NVarChar);
                objSqlParam[6].Value = objCompensationDetailsModel.Year;

                objSqlParam[7] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                objSqlParam[7].Value = IsAdmin;

                objSqlParam[8] = new SqlParameter("@AllTeammembers", SqlDbType.Bit);
                objSqlParam[8].Value = AllTeammembers;

                objSqlParam[9] = new SqlParameter("@ShiftID", SqlDbType.Int);
                objSqlParam[9].Value = objCompensationDetailsModel.ShiftId;
           
                dsSearchCompensationRpt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchCompensationRpt", objSqlParam);
                return dsSearchCompensationRpt;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "SearchTMCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }        
        #endregion

        //wf


        #region WFGetCompensationDetails

        public DataSet WFGetCompensationDetails(int compensationID)
         {
             try
            {
                  DataSet GetCompensationDetails;
            SqlParameter[] objParam = new SqlParameter[1];
            objParam[0] = new SqlParameter("@CompensationID",compensationID );

               GetCompensationDetails=SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "WFGetCompensationDetails",objParam);
                 return GetCompensationDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "WFGetCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

         }
        
        #endregion

        #region GetEmploymentStatus
         public int GetEmploymentStatus(CompensationDetailsModel objCompensationModel)
        {    try
            {        
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@UserID", objCompensationModel.UserID);           
                int y = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetEmploymentStatus", objParam));
                return y;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "GetEmploymentStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region GetCancelCompOffDetails
        public DataSet GetCancelCompOffDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                DataSet GetCancelCompOffDetails;
                SqlParameter[] objSqlparam = new SqlParameter[2];

                objSqlparam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[0].Value = objCompensationModel.UserID;
                objSqlparam[1] = new SqlParameter("@CompensationID", SqlDbType.Int);
                objSqlparam[1].Value = objCompensationModel.CompensationID;

                return GetCancelCompOffDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetCancelCompOffDetails", objSqlparam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "GetCancelCompOffDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region Admin Approval

        // get the records of all the employee for admin approval of compensatory leave
        public DataSet GetCompensatoryLeaveForAdminApproval(int StatusID, string FromDate, string ToDate)
        {
            try
            {
                DataSet dsCompensatorydetails = new DataSet();

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@StatusID", StatusID);
                param[1] = new SqlParameter("@CompLeaveDateFrom", FromDate);
                param[2] = new SqlParameter("@CompLeaveDateTo", ToDate);

               return dsCompensatorydetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetCompensatoryLeaveForAdminApproval", param);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "GetCompensatoryLeaveForAdminApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        
        #endregion

        //to Check the eligibility of the login user for applying the Comp-off
        public Boolean GetCompOffEligibility(int UserId, string DesignationID)
        {
            try
            {
                Boolean isEligible;
                DataSet dsIsEligible = new DataSet();
                                
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@UserId", UserId);
                param[1] = new SqlParameter("@DesignationID", DesignationID);
                
                dsIsEligible = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetComp-OffEligibility", param);
                return isEligible = Convert.ToBoolean(dsIsEligible.Tables[0].Rows[0]["IsEligible"]);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsDAL.cs", "GetCompOffEligibility", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

    }
}
