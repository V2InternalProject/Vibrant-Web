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
    public class CompensationDetailsBOL
    {
        CompensationDetailsDAL objCompensationDAL = new CompensationDetailsDAL();
        DataSet dsCompensationDetails = new DataSet();

        #region Get Weekly Off 
        public DataSet GetWeeklyOff(CompensationDetailsModel objCompensationDetailsModel)
        {
            try
            {
                dsCompensationDetails = objCompensationDAL.GetWeeklyOff(objCompensationDetailsModel);
                return dsCompensationDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL.cs", "GetWeeklyOff", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion
        

        #region GetCompensationDetails
        public DataSet GetCompensationDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                return objCompensationDAL.GetCompensationDetails(objCompensationModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "GetCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
      
#endregion

        #region GetTMCompensationDetails
        public DataSet GetTMCompensationDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                return objCompensationDAL.GetTMCompensationDetails(objCompensationModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "GetTMCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region AddCompenstionDetails
        public SqlDataReader AddCompenstionDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                return objCompensationDAL.AddCompenstionDetails(objCompensationModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "AddCompenstionDetails", ex.StackTrace);
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
            try
            {
                return objCompensationDAL.GetMaxCompensationID();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "GetMaxCompensationID", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region UpdateCompenstionDetails
        public int UpdateCompenstionDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                return objCompensationDAL.UpdateCompenstionDetails(objCompensationModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "UpdateCompenstionDetails", ex.StackTrace);
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
                return objCompensationDAL.UpdateCancelCompenstionDetails(objCompensationModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "UpdateCancelCompenstionDetails", ex.StackTrace);
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
                return objCompensationDAL.UpdateApprovalCompenstionDetails(objCompensationModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "UpdateApprovalCompenstionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region SearchAllCompensationDetails
        public DataSet SearchAllCompensationDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                return objCompensationDAL.SearchAllCompensationDetails(objCompensationModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "SearchAllCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchCompensationDetails
        public DataSet SearchCompensationDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                return objCompensationDAL.SearchCompensationDetails(objCompensationModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "SearchCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region CheckSignInForCompensation
        public int CheckSignInForCompensation(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                return objCompensationDAL.CheckSignInForCompensation(objCompensationModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "CheckSignInForCompensation", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchAllTMCompensationDetails
        public DataSet SearchAllTMCompensationDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                return objCompensationDAL.SearchAllTMCompensationDetails(objCompensationModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "SearchAllTMCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region SearchTMCompensationDetails
        public DataSet SearchTMCompensationDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                return objCompensationDAL.SearchTMCompensationDetails(objCompensationModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "SearchTMCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion
        // Compensation Report

        //Workflow

          #region WFGetCompensationDetails
        public DataSet WFGetCompensationDetails(int compensationID)
        {
            try
            {
                return objCompensationDAL.WFGetCompensationDetails(compensationID);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "WFGetCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion

        #region SearchCompensationRpt
        public DataSet SearchCompensationRpt(CompensationDetailsModel objCompensationDetailsModel, bool IsAdmin, bool AllTeammembers)
        {
            try
            {
                return objCompensationDAL.SearchCompensationRpt(objCompensationDetailsModel, IsAdmin, AllTeammembers);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "SearchCompensationRpt", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }
        
        #endregion

        #region GetEmploymentStatus
        public int GetEmploymentStatus(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                return objCompensationDAL.GetEmploymentStatus(objCompensationModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {                
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "UpdateCompenstionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
               
            }
        }
        #endregion

        #region GetCancelCompOffDetails
        public DataSet GetCancelCompOffDetails(CompensationDetailsModel objCompensationModel)
        {
            try
            {
                return objCompensationDAL.GetCancelCompOffDetails(objCompensationModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "GetCancelCompOffDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region Admin Approval

        public DataSet GetCompensatoryLeaveForAdminApproval(int StatusID, string FromDate, string ToDate)
        {
            try
            {
                return objCompensationDAL.GetCompensatoryLeaveForAdminApproval(StatusID, FromDate, ToDate);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "GetCompensatoryLeaveForAdminApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }

        #endregion

        public Boolean GetCompOffEligibility(int UserId, string DesignationID)
        {
            try
            {
                Boolean isEligible;
                return isEligible = objCompensationDAL.GetCompOffEligibility(UserId, DesignationID);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationDetailsBOL.cs", "UpdateApprovalCompenstionDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);              
            }
        }
    }
}
