using System;
using System.Collections.Generic;
using System.Text;
using V2.CommonServices.FileLogger;
using V2.CommonServices.Exceptions;
using System.Data.SqlClient;
using System.Data;
using V2.Orbit.Model;
using V2.Orbit.DataLayer;

namespace V2.Orbit.BusinessLayer
{
    [Serializable]
    public class OutOfOfficeBOL
    {
        OutOfOfficeDAL objOutOfOfficeDAL = new OutOfOfficeDAL();

        #region AddOutOfOffice
        public SqlDataReader AddOutOfOffice(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                return objOutOfOfficeDAL.AddOutOfOffice(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("An entry for the selected Date and Time already exists") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "AddOutOfOffice", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else {
                    throw ex;
                }
            }


        } 
        #endregion

        #region UpdateOutOffice
        public int UpdateOutOffice(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                return objOutOfOfficeDAL.UpdateOutOffice(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("An entry for the selected Date and Time already exists") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "updateOutOfOffice", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else
                {
                    throw ex;
                }
            }

        } 
        #endregion

        #region DeleteOutOfOffice
        public int DeleteOutOfOffice(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                return objOutOfOfficeDAL.DeleteOutOfOffice(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "updateOutOfOffice", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        } 
        #endregion

        #region CancelOutOfOffice
        public DataSet CancelOutOfOffice(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                return objOutOfOfficeDAL.CancelOutOfOffice(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "CancelOutOfOffice", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        
        #endregion
        #region GetReasonName
        public DataSet GetReasonName()
        {
            try
            {
                return objOutOfOfficeDAL.GetReasonName();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "GetReasonName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }


        } 
        #endregion

        #region GetOutOfOffice
        public DataSet GetOutOfOffice(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                return objOutOfOfficeDAL.GetOutOfOffice(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "GetOutOfOffice", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion

        #region GetStatus
        public DataSet GetStatus()
        {
            try
            {
                return objOutOfOfficeDAL.GetStatus();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "GetOutOfOffice", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion


        public DataSet GetReportingTo(OutOfOfficeModel objOutOfOfficeModel)
         {
             try
            {
                return objOutOfOfficeDAL.GetReportingTo(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "objOutOfOfficeBOL.cs", "GetReportingTo", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #region SearchOutOfOffice
        public DataSet SearchOutOfOffice(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                return objOutOfOfficeDAL.SearchOutOfOffice(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "SearchOutOfOffice", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion
        
        #region SearchOutOfOfficeDatewise
        public DataSet SearchOutOfOfficeDatewise(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                return objOutOfOfficeDAL.SearchOutOfOfficeDatewise(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "SearchOutOfOfficeDatewise", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        
        #endregion

        #region SearchOutOfOfficeApprovalDateWise
        public DataSet SearchOutOfOfficeApprovalDateWise(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                return objOutOfOfficeDAL.SearchOutOfOfficeApprovalDateWise(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "SearchOutOfOfficeApprovalDateWise", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion

        #region GetOutOfOfficeApproval
        public DataSet GetOutOfOfficeApproval(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                return objOutOfOfficeDAL.GetOutOfOfficeApproval(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "GetOutOfOfficeApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion

        #region UpdateOutOfficeApproval
        public int UpdateOutOfficeApproval(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                return objOutOfOfficeDAL.UpdateOutOfficeApproval(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("An entry for the selected Date and Time already exists") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "UpdateOutOfficeApproval", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else
                {
                    throw ex;
                }
            }

        } 
        #endregion

        #region SearchOutOfOfficeApproval
        public DataSet SearchOutOfOfficeApproval(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                return objOutOfOfficeDAL.SearchOutOfOfficeApproval(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "SearchOutOfOfficeApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion
         // Out Of Office Report

        #region GetEmployeeNameRpt
        public DataSet GetEmployeeNameRpt(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                return objOutOfOfficeDAL.GetEmployeeNameRpt(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "GetEmployeeNameRpt", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }


        } 
        #endregion
        #region GetEmployeeNameRptShift
        public DataSet GetEmployeeNameRptShift(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                return objOutOfOfficeDAL.GetEmployeeNameRptShift(objOutOfOfficeModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "GetEmployeeNameRptShift", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }


        }
        #endregion

        #region SearchOutOfOfficeRpt
        public DataSet SearchOutOfOfficeRpt(OutOfOfficeModel objOutOfOfficeModel, bool IsAdmin, bool AllTeammembers)
        {
            try
            {
                return objOutOfOfficeDAL.SearchOutOfOfficeRpt(objOutOfOfficeModel, IsAdmin, AllTeammembers);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL", "SearchOutOfOfficeRpt", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        } 
        #endregion

        // WorkFlow
        #region WFGetOutOfOfficeDetails
        public DataSet WFGetOutOfOfficeDetails(int outOfOfficeID)
        {
            try
            {
                return objOutOfOfficeDAL.WFGetOutOfOfficeDetails(outOfOfficeID);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeBOL.cs", "WFGetOutOfOfficeDetailss", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion



        
    }
}
