using System;
using System.Collections.Generic;
using System.Text;
using V2.Orbit.DataLayer;
using V2.Orbit.Model;
using System.Data;
using System.Data.SqlClient;
using V2.CommonServices.FileLogger;
using V2.CommonServices.Exceptions;

namespace V2.Orbit.BusinessLayer
{
    [Serializable]
    public class LeaveDetailsBOL 
    {
        LeaveDeatilsDAL objLeaveDetailsDAL = new LeaveDeatilsDAL();

        #region AddLeaveDetails
        public SqlDataReader AddLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.AddLeaveDetails(objLeaveDeatilsModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "AddLeaveDetails", ex.StackTrace);
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
        public int UpdateLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.UpdateLeaveDetails(objLeaveDeatilsModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "UpdateLeaveDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region UpdateTMLeaveDetails
        public int UpdateTMLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.UpdateTMLeaveDetails(objLeaveDeatilsModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "UpdateTMLeaveDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        public int UpdateLeaveDetailsForFuture(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.UpdateLeaveDetailsForFuture(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                 FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "UpdateLeaveDetailsForFuture", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);              
               
            }
        }
        #endregion

        #region GetLeaveDetails
        public DataSet GetLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.GetLeaveDetails(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "GetLeaveDetailsProc", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet GetLeaveDetailsForFuture(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.GetLeaveDetailsForFuture(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "GetLeaveDetailsProc", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        
         public DataSet WFGetLeaveDetails(int leaveDetailsId)
         {
             try
            {

                return objLeaveDetailsDAL.WFGetLeaveDetails(leaveDetailsId);
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
        #endregion

        public void wfUpdateLeaveBalance()
        {           
            try
            {
              objLeaveDetailsDAL.wfUpdateLeaveBalance();
                 
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
        #region GetTeamMembersLeaveDetails
        public DataSet GetTeamMembersLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.GetTeamMembersLeaveDetails(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "GetTeamMembersLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region GetReportingTo
        public DataSet GetReportingTo(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.GetReportingTo(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "GetReportingTo", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region GetStatusDetails
        public DataSet GetStatusDetails()
        {
            try
            {
                return objLeaveDetailsDAL.GetStatusDetails();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "GetStatusDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion  

        #region SearchLeaveDetails
        public DataSet SearchLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.SearchLeaveDetails(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "SearchLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchAllLeaveDetails
        public DataSet SearchAllLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.SearchAllLeaveDetails(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "SearchAllLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchAllTMLeaveDetails
        public DataSet SearchAllTMLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.SearchAllTMLeaveDetails(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "SearchAllTMLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchTeamMembersLeaveDetails
        public DataSet SearchTeamMembersLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.SearchTeamMembersLeaveDetails(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "SearchTeamMembersLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region TotalLeaves
        public DataSet TotalLeaves(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.TotalLeaves(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "TotalLeaves", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region GetLeaveBalance
        public DataSet GetLeaveBalance(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.GetLeaveBalance(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "GetLeaveBalance", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region GetTotalLeaveBalance
        public DataSet TotalLeaveBalance(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.TotalLeaveBalance(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "TotalLeaveBalance", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region UpdateCancelLeaveDetails
        public int UpdateCancelLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.UpdateCancelLeaveDetails(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "UpdateCancelLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region UpdateLeaveBalance
        public int UpdateLeaveBalance(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.UpdateLeaveBalance(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "UpdateLeaveBalance", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion
        // Search Leave Report

        #region SearchLeaveRpt
        public DataSet SearchLeaveRpt(LeaveDetailsModel objLeaveDetailsModel, bool IsAdmin, bool AllTeammembers)
        {
            try
            {
                return objLeaveDetailsDAL.SearchLeaveRpt(objLeaveDetailsModel,IsAdmin,AllTeammembers);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "SearchLeaveRpt", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        
        #endregion

        #region SummaryReport
        public DataSet SummaryReport(LeaveDetailsModel objLeaveDetailsModel, bool IsAdmin, bool AllTeammembers)
        {
            try
            {
                return objLeaveDetailsDAL.SummaryReport(objLeaveDetailsModel, IsAdmin, AllTeammembers);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "SummaryReport", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        
        #endregion        

        #region CheckLeaveDetails
        public int CheckLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.CheckLeaveDetails(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "CheckLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region AddingBonusLeaves
        public DataSet AddingBonusLeaves(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.AddingBonusLeaves(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "AddingBonusLeaves", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region GetBulkLeaveDetails
        public DataSet GetBulkLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.GetBulkLeaveDetails(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "GetBulkLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
       
        #endregion

        #region SearchBulkLeaveDetails
        public DataSet SearchBulkLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.SearchBulkLeaveDetails(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "SearchBulkLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region CheckLeavesinSignIn
        public DataSet CheckLeavesinSignIn(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.CheckLeavesinSignIn(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "CheckLeavesinSignIn", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region GetCancelLeaveDetails
        public DataSet GetCancelLeaveDetails(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.GetCancelLeaveDetails(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "GetCancelLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region UpdateEmployeeLeaveAndComp
        public int UpdateEmployeeLeaveAndComp(LeaveDetailsModel objLeaveDeatilsModel)
        {
            try
            {
                return objLeaveDetailsDAL.UpdateEmployeeLeaveAndComp(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "UpdateEmployeeLeaveAndComp", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                
            }
        }
        #endregion
        
        #region LeaveAnomalyReport

        public DataSet LeaveAnomalyReport(string month,string year)
        {
            try
            {
                return objLeaveDetailsDAL.LeaveAnomalyReport(month, year);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "LeaveAnomalyReport", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region Admin Approval

        public DataSet GetLeaveForAdminApproval(int StatusID, string FromDate, string ToDate)
        {
            try
            {
                return objLeaveDetailsDAL.GetLeaveForAdminApproval(StatusID, FromDate, ToDate);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "GetLeaveForAdminApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }

        #endregion

        #region Get Leave Balance For Given Date

        public DataSet GetLeaveBalanceForGivenDate(int EmployeeCode, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return objLeaveDetailsDAL.CheckLeaveBalanceForGivenDate(EmployeeCode, FromDate, ToDate);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsBOL.cs", "GetLeaveBalanceForGivenDate", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }

        }

        #endregion
    }
}
