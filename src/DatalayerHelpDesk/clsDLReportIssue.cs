using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using HRMS.ModelHelpdesk;


namespace DataLayerHelpDesk
{
    /// <summary>
    /// Summary description for clsDLReportIssue.
    /// </summary>
    public class clsDLReportIssue
    {
        string strConnectionString = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
        public clsDLReportIssue()
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLReportIssue.cs", "clsDLReportIssue", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        public DataSet GetEmailID(int EmployeeID)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.NVarChar, 10);
                sqlParams[0].Value = EmployeeID;
                DataSet dsEmail = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetEmailID", sqlParams);
                return dsEmail;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLReportIssue.cs", "GetEmailID", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        public DataSet GetSeverity()
        {
            try
            {
                DataSet dsSeverity = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetSeverity");
                return dsSeverity;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLReportIssue.cs", "GetSeverity", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        public DataSet GetSubCategory()
        {
            try
            {
                DataSet dsSubCategory = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_BindSubCategory");
                return dsSubCategory;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLReportIssue.cs", "GetSubCategory", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        public DataSet GetType()
        {
            try
            {
                DataSet dsType = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetRequestType");
                return dsType;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLReportIssue.cs", "GetType", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        public string GetCategoryName(clsReportIssue objclsReportIssue)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@SubCatgoryId", SqlDbType.Int, 4);
                sqlParams[0].Value = objclsReportIssue.SubCategoryID;
                sqlParams[0].Direction = ParameterDirection.Input;
                string Category = (SqlHelper.ExecuteScalar(strConnectionString, CommandType.StoredProcedure, "sp_GetCategoryNames", sqlParams)).ToString();
                return Category;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLReportIssue.cs", "GetCategoryName", ex.StackTrace);
                throw new V2Exceptions();
            }
        }


        public string GetAssignedToEmailID(clsReportIssue objclsReportIssue)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
                sqlParams[0].Value = objclsReportIssue.EmployeeID;
                sqlParams[0].Direction = ParameterDirection.Input;
                string AssignedToEmailID = (SqlHelper.ExecuteScalar(strConnectionString, CommandType.StoredProcedure, "GetAssignedToEmailID", sqlParams)).ToString();
                return AssignedToEmailID;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLReportIssue.cs", "GetAssignedToEmailID", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        public DataSet GetPriority()
        {
            try
            {
                DataSet dsPriority = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetPriority");
                return dsPriority;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLReportIssue.cs", "GetPriority", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        public DataSet InsertIssueDetails(clsReportIssue objClsReportIssue)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[12];


                //sqlParams[0] = new SqlParameter("@ReportIssueID", SqlDbType.Int, 4);
                //sqlParams[0].Value = objClsReportIssue.ReportIssueID;
                //sqlParams[0].Direction = ParameterDirection.Input;

                sqlParams[0] = new SqlParameter("@Name", SqlDbType.VarChar, 100);
                sqlParams[0].Value = objClsReportIssue.Name;
                sqlParams[0].Direction = ParameterDirection.Input;

                sqlParams[1] = new SqlParameter("@EmailID", SqlDbType.VarChar, 100);
                sqlParams[1].Value = objClsReportIssue.EmailID;
                sqlParams[1].Direction = ParameterDirection.Input;

                sqlParams[2] = new SqlParameter("@CCEmailID", SqlDbType.VarChar, 100);
                sqlParams[2].Value = objClsReportIssue.CCEmailID;
                sqlParams[2].Direction = ParameterDirection.Input;

                sqlParams[3] = new SqlParameter("@PhoneExtension", SqlDbType.VarChar, 100);
                sqlParams[3].Value = objClsReportIssue.PhoneExtension;
                sqlParams[3].Direction = ParameterDirection.Input;

                sqlParams[4] = new SqlParameter("@SeatingLocation", SqlDbType.VarChar, 100);
                sqlParams[4].Value = objClsReportIssue.SeatingLocation;
                sqlParams[4].Direction = ParameterDirection.Input;

                sqlParams[5] = new SqlParameter("@SubCategoryID", SqlDbType.Int, 4);
                sqlParams[5].Value = objClsReportIssue.SubCategoryID;
                sqlParams[5].Direction = ParameterDirection.Input;

                sqlParams[6] = new SqlParameter("@SeverityID", SqlDbType.Int, 4);
                sqlParams[6].Value = objClsReportIssue.SeverityID;
                sqlParams[6].Direction = ParameterDirection.Input;

                //	sqlParams[8] = new SqlParameter("@PriorityID", SqlDbType.Int, 4);
                //	sqlParams[8].Value = objClsReportIssue.PriorityID;
                //	sqlParams[8].Direction = ParameterDirection.Input;

                sqlParams[7] = new SqlParameter("@UploadedFileName", SqlDbType.VarChar, 100);
                sqlParams[8] = new SqlParameter("@UploadedFileExtension", SqlDbType.VarChar, 50);
                if (objClsReportIssue.UploadedFileName == null && objClsReportIssue.UploadedFileExtension == null)
                {
                    sqlParams[7].Value = 0;
                    sqlParams[8].Value = 0;
                }
                else if (objClsReportIssue.UploadedFileName != "")
                {
                    sqlParams[7].Value = objClsReportIssue.UploadedFileName;
                    sqlParams[8].Value = objClsReportIssue.UploadedFileExtension;
                }
                sqlParams[7].Direction = ParameterDirection.Input;
                sqlParams[8].Direction = ParameterDirection.Input;

                sqlParams[9] = new SqlParameter("@Description", SqlDbType.VarChar, 1000);
                sqlParams[9].Value = objClsReportIssue.Description;
                sqlParams[9].Direction = ParameterDirection.Input;

                sqlParams[10] = new SqlParameter("@StatusID", SqlDbType.Int, 4);
                sqlParams[10].Value = objClsReportIssue.StatusID;
                sqlParams[10].Direction = ParameterDirection.Input;

                sqlParams[11] = new SqlParameter("@TypeID", SqlDbType.Int, 4);
                sqlParams[11].Value = objClsReportIssue.Type;
                sqlParams[11].Direction = ParameterDirection.Input;

                DataSet IssueSubmitResult;
                IssueSubmitResult = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_InsertIssueDetails", sqlParams);
                return IssueSubmitResult;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLReportIssue.cs", "InsertIssueDetails", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        public DataSet InsertFile(clsReportIssue objClsReportIssue)
        {

            try
            {
                SqlParameter[] sqlParams = new SqlParameter[3];
                sqlParams[0] = new SqlParameter("@ReportIssueID", SqlDbType.Int, 4);
                sqlParams[0].Value = objClsReportIssue.ReportIssueID;
                sqlParams[0].Direction = ParameterDirection.Input;

                sqlParams[1] = new SqlParameter("@UploadedFileName", SqlDbType.VarChar, 100);
                sqlParams[2] = new SqlParameter("@UploadedFileExtension", SqlDbType.VarChar, 50);
                if (objClsReportIssue.UploadedFileName == null && objClsReportIssue.UploadedFileExtension == null)
                {
                    sqlParams[1].Value = 0;
                    sqlParams[2].Value = 0;
                }
                else if (objClsReportIssue.UploadedFileName != "")
                {
                    sqlParams[1].Value = objClsReportIssue.UploadedFileName;
                    sqlParams[2].Value = objClsReportIssue.UploadedFileExtension;
                }
                sqlParams[1].Direction = ParameterDirection.Input;
                sqlParams[2].Direction = ParameterDirection.Input;

                DataSet InsertFile;
                InsertFile = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "InsertFile", sqlParams);
                return InsertFile;

            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLReportIssue.cs", "InsertFile", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        public bool IsCCEmailExists(string CCEmail)
        {
            SqlParameter[] sqlParams = new SqlParameter[1];
            try
            {
                sqlParams[0] = new SqlParameter("@CCEmail", SqlDbType.VarChar, 100);
                sqlParams[0].Value = CCEmail;
                sqlParams[0].Direction = ParameterDirection.Input;

                DataSet result = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetCcEmailId", sqlParams);

                if (result.Tables[0].Rows.Count > 0) return true;

                else return false;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public DataSet getCategorySummary(clsReportIssue objclsReportIssue)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@SubCategoryID", SqlDbType.Int, 4);
                sqlParams[0].Value = objclsReportIssue.SubCategoryID;
                sqlParams[0].Direction = ParameterDirection.Input;
                DataSet dsGetData = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetCategorySummary", sqlParams);
                return dsGetData;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLReportIssue.cs", "getCategorySummary", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

    }
}
