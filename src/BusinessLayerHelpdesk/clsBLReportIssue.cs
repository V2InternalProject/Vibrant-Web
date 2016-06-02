using System;
using System.Data;
using System.Data.SqlClient;

using HRMS;
using DataLayerHelpDesk;
using HRMS.ModelHelpdesk;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace BusinessLayerHelpdesk
{
    /// <summary>
    /// Summary description for clsBLReportIssue.
    /// </summary>
    public class clsBLReportIssue
    {
        public clsBLReportIssue()
        {
            try
            {
                //
                // TODO: Add constructor logic here
                //
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "clsBLReportIssue", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        public DataSet GetEmailID(int EmployeeID)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue1 = new clsDLReportIssue();
                return objClsDLReportIssue1.GetEmailID(EmployeeID);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetEmailID", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        public DataSet GetSeverity()
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetSeverity();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetSeverity", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        public DataSet GetSubCategory()
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetSubCategory();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetSubCategory", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        public string GetCategoryName(clsReportIssue objclsReportIssue)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetCategoryName(objclsReportIssue);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetCategoryName", ex.StackTrace);
                throw new V2Exceptions();
            }
        }



        public string GetAssignedToEmailID(clsReportIssue objclsReportIssue)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetAssignedToEmailID(objclsReportIssue);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetAssignedToEmailID", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        public DataSet GetPriority()
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetPriority();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetPriority", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        public DataSet GetType()
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.GetType();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "GetType", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        public DataSet InsertIssueDetails(clsReportIssue objClsReportIssue)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.InsertIssueDetails(objClsReportIssue);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "InsertIssueDetails", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        public DataSet InsertFile(clsReportIssue objClsReportIssue)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.InsertFile(objClsReportIssue);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "InsertIssueDetails", ex.StackTrace);
                throw new V2Exceptions();
            }
        }


        public bool IsCCEmailExists(string CCEmailId)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.IsCCEmailExists(CCEmailId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataSet getCategorySummary(clsReportIssue objClsReportIssue)
        {
            try
            {
                clsDLReportIssue objClsDLReportIssue = new clsDLReportIssue();
                return objClsDLReportIssue.getCategorySummary(objClsReportIssue);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLReportIssue.cs", "getCategorySummary", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

    }
}
