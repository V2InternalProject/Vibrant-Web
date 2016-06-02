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
    public class LeaveTransactionBOL
    {
        LeaveTransactionDAL objLeaveTransDetailsDAL = new LeaveTransactionDAL();

        #region AddLeaveTransactionDetails
        public int AddLeaveTransactionDetails(LeaveTransactionModel objLeaveTransDetailsModel)
        {
            try
            {
                return objLeaveTransDetailsDAL.AddLeaveTransactionDetails(objLeaveTransDetailsModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL.cs", "AddLeaveTransactionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
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
            try
            {
                return objLeaveTransDetailsDAL.AddCompensationTransactionDetails(objLeaveTransDetailsModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL.cs", "AddCompensationTransactionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
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
            try
            {
                return objLeaveTransDetailsDAL.DeleteLeaveTransactionDetails(objLeaveTransDetailsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL.cs", "DeleteLeaveTransactionDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region DeleteCompensationTransactionDetails
        public int DeleteCompensationTransactionDetails(LeaveTransactionModel objLeaveTransDetailsModel)
        {
            try
            {
                return objLeaveTransDetailsDAL.DeleteCompensationTransactionDetails(objLeaveTransDetailsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL.cs", "DeleteCompensationTransactionDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region GetMaxLeaveDetailID
        public DataSet GetMaxLeaveDetailID()
        {
            try
            {
                return objLeaveTransDetailsDAL.GetMaxLeaveDetailID();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL.cs", "GetMaxLeaveDetailID", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region UpdateLeaveTransactionDetails
        public int UpdateLeaveTransactionDetails(LeaveTransactionModel objLeaveTransDetailsModel)
        {
            try
            {
                return objLeaveTransDetailsDAL.UpdateLeaveTransactionDetails(objLeaveTransDetailsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL.cs", "UpdateLeaveTransactionDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        public int UpdateLeaveTransactionDetailsForFuture(LeaveTransactionModel objLeaveTransDetailsModel)
        {
            try
            {
                return objLeaveTransDetailsDAL.UpdateLeaveTransactionDetailsForFuture(objLeaveTransDetailsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL.cs", "UpdateLeaveTransactionDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion
        // Leave Transaction Report

        #region SearchLeaveTransactionRpt
        public DataSet SearchLeaveTransactionRpt(LeaveTransactionModel objLeaveTransactionModel, bool @IsAdmin, bool @AllTeammembers)
        {
            try
            {
                return objLeaveTransDetailsDAL.SearchLeaveTransactionRpt(objLeaveTransactionModel, IsAdmin, AllTeammembers);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL", "SearchLeaveTransactionRpt", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        } 
        #endregion
        // Leave Transaction Admin

        #region dsGetLeaveTransaction
        public DataSet dsGetLeaveTransaction(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                return objLeaveTransDetailsDAL.dsGetLeaveTransaction(objLeaveTransactionModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL", "drGetLeaveTransaction", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet GetLeaveTransactionForspecificLeave(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                return objLeaveTransDetailsDAL.GetLeaveTransactionForspecificLeave(objLeaveTransactionModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL", "drGetLeaveTransaction", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        public DataSet dsGetLeaveTransactionForFuture(LeaveDetailsModel objLeaveDetailsModel)
        {
            try
            {
                return objLeaveTransDetailsDAL.GetLeaveTransactionForFuture(objLeaveDetailsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL", "dsGetLeaveTransactionForFuture", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion
        #region AddLeaveTransactionAdmin
        public int AddLeaveTransactionAdmin(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                return objLeaveTransDetailsDAL.AddLeaveTransactionAdmin(objLeaveTransactionModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL", "AddLeaveTransactionAdmin", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
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
                return objLeaveTransDetailsDAL.UpdateLeaveTransactionAdmin(objLeaveTransactionModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL", "AddLeaveTransactionAdmin", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);


            }
        } 
        #endregion

        #region DeleteLeaveTransactionAdmin
        public int DeleteLeaveTransactionAdmin(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                return objLeaveTransDetailsDAL.DeleteLeaveTransactionAdmin(objLeaveTransactionModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL", "DeleteLeaveTransactionAdmin", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
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
            try
            {
                return objLeaveTransDetailsDAL.SearchLeaveTransactionAdmin(objLeaveTransactionModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL", "SearchLeaveTransactionAdmin", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        } 
        #endregion

        #region SearchLeaveTransactiondatewise
        public DataSet SearchLeaveTransactiondatewise(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                return objLeaveTransDetailsDAL.SearchLeaveTransactiondatewise(objLeaveTransactionModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL", "SearchLeaveTransactiondatewise", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion

        #region GetTotalLeave
        public DataSet GetTotalLeave(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                return objLeaveTransDetailsDAL.GetTotalLeave(objLeaveTransactionModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL", "SearchLeaveTransactiondatewise", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion


        #region CheckEmployeeNameValidation
        public int CheckEmployeeNameValidation(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                return objLeaveTransDetailsDAL.CheckEmployeeNameValidation(objLeaveTransactionModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL", "CheckEmployeeNameValidation", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion

        #region UpdateLeaveBalance
        public int UpdateLeaveBalance(LeaveTransactionModel objLeaveTransactionModel)
        {
            try
            {
                return objLeaveTransDetailsDAL.UpdateLeaveBalance(objLeaveTransactionModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransactionBOL", "CheckEmployeeNameValidation", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }
        
        #endregion
    }
}
