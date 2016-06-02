using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using ModelHelpdesk;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace DataLayerHelpDesk
{
	/// <summary>
	/// Summary description for clsDLSummaryReport.
	/// </summary>
	public class clsDLSummaryReport
	{

		string sqlConn = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
		public clsDLSummaryReport()
		{
			try
			{
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSummaryReport.cs", "clsDLSummaryReport", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

        public DataSet DepartmentDetails(clsSummaryReport objSumaryReport)
		{
			DataSet dsDept;
			dsDept = new DataSet();

			try
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
                sqlParams[0].Value = objSumaryReport.EmployeeID;

                dsDept = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "GetCategoriesProc", sqlParams);
				return dsDept;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSummaryReport.cs", "DepartmentDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet EmployeeDetails(clsSummaryReport objSumaryReport)
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
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSummaryReport.cs", "EmployeeDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet EmployeeDetails()
		{	
			try
			{
			DataSet dsRowsReturned = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetEmployeeDetailProc");
			return dsRowsReturned;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSummaryReport.cs", "EmployeeDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet DeptSummaryDetails(clsSummaryReport objSumaryReport)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
			sqlParams[0].Value = objSumaryReport.CategoryID;
            sqlParams[1] = new SqlParameter("@ReportIssueDate", SqlDbType.DateTime);
			sqlParams[1].Value = objSumaryReport.ReportIssueDate;
            sqlParams[2] = new SqlParameter("@ReportCloseDate", SqlDbType.DateTime);
			sqlParams[2].Value = objSumaryReport.ReportCloseDate;

			DataSet dsSummary = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "GetSummaryDetailsProc",sqlParams);
			return dsSummary;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSummaryReport.cs", "DeptSummaryDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		
		public DataSet EmpSummaryDetails(clsSummaryReport objSumaryReport)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[3];
			sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[0].Value = objSumaryReport.EmployeeID;
			sqlParams[1] = new SqlParameter("@ReportIssueDate", SqlDbType.DateTime);
			sqlParams[1].Value = objSumaryReport.ReportIssueDate;
			sqlParams[2] = new SqlParameter("@ReportCloseDate", SqlDbType.DateTime);
			sqlParams[2].Value = objSumaryReport.ReportCloseDate;

			DataSet dsSummary = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetEmpSummary",sqlParams);
			return dsSummary;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSummaryReport.cs", "EmpSummaryDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
	}
}
