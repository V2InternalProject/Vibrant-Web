
using System.Collections.Generic;
using System.Text;
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using V2.Helpdesk.Model;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.DataLayer
{

    public class clsDLSLAReport
    {

        string sqlConn = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
        public clsDLSLAReport()
        {
            try
            {
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSLAReport.cs", "clsDLSLAReport", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet DepartmentDetails(clsSLAReport objSLAReport)
        {
            DataSet dsDept;
            dsDept = new DataSet();

            try
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
                sqlParams[0].Value = objSLAReport.EmployeeID;

                dsDept = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "GetCategoriesProc", sqlParams);
                return dsDept;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSLAReport.cs", "DepartmentDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet EmployeeDetails(clsSLAReport objSLAReport)
        {
            try
            {
                /*SqlParameter[] sqlParams = new SqlParameter[1];
			
                sqlParams[0] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
                sqlParams[0].Value = objSumaryReport.CategoryID;
                sqlParams[0].Direction = ParameterDirection.Input;


                DataSet dsRowsReturned = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetCategoryWiseEmployees", sqlParams);*/
                DataSet dsRowsReturned = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetEmployeeDetail");
                return dsRowsReturned;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSLAReport.cs", "EmployeeDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet EmployeeDetails()
        {
            try
            {
                DataSet dsRowsReturned = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetEmployeeDetailProc");
                return dsRowsReturned;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSLAReport.cs", "EmployeeDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet DeptSLADetails(clsSLAReport objSLAReport)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[3];
                //sqlParams[0].Value = new SqlParameter("@IssueResolvedDate", SqlDbType.DateTime);
                //sqlParams[0].Value = objSLAReport.IssueResolvedDate;
                //sqlParams[1].Value = new SqlParameter("@StatusID", SqlDbType.Int, 4);
                //sqlParams[1].Value = objSLAReport.StatusID;
                //sqlParams[2].Value = new SqlParameter("@IssueHelth", SqlDbType.Int, 4);
                //sqlParams[2].Value = objSLAReport.IssueHealth;

                //sqlParams[0] = new SqlParameter("@DepartmentID", SqlDbType.Int, 4);
                //sqlParams[0].Value = objSLAReport.CategoryID;
                //sqlParams[1] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                //sqlParams[1].Value = objSLAReport.ReportIssueDate;
                //sqlParams[2] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                //sqlParams[2].Value = objSLAReport.ReportCloseDate;

                DataSet dsSummary = new DataSet();

                SqlConnection con = new SqlConnection(sqlConn);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetSLAReport";
                cmd.CommandTimeout = 6000;

                cmd.Parameters.Add("@DepartmentID", SqlDbType.Int, 4);
                cmd.Parameters["@DepartmentID"].Value = objSLAReport.CategoryID;

                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime);
                cmd.Parameters["@FromDate"].Value = objSLAReport.ReportIssueDate;

                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime);
                cmd.Parameters["@ToDate"].Value = objSLAReport.ReportCloseDate;


                cmd.Connection = con;

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dsSummary);

                return dsSummary;


                //DataSet dsSummary = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetSLAReport", sqlParams);
                //return dsSummary;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSLAReport.cs", "DeptSummaryDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet EmpSummaryDetails(clsSLAReport objSLAReport)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[3];
                sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
                sqlParams[0].Value = objSLAReport.EmployeeID;
                sqlParams[1] = new SqlParameter("@ReportIssueDate", SqlDbType.DateTime);
                sqlParams[1].Value = objSLAReport.ReportIssueDate;
                sqlParams[2] = new SqlParameter("@ReportCloseDate", SqlDbType.DateTime);
                sqlParams[2].Value = objSLAReport.ReportCloseDate;

                DataSet dsSummary = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetEmpSummary", sqlParams);
                return dsSummary;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSLAReport.cs", "EmpSummaryDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
    }
}
