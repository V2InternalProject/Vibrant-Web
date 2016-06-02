using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using ModelHelpdesk;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace DataLayerHelpDesk
{
	/// <summary>
	/// Summary description for clsDLMemberWiseSearchReport.
	/// </summary>
	public class clsDLMemberWiseSearchReport
	{
		clsMemberWiseSearchReport objClsMemberWiseSearchReport = new clsMemberWiseSearchReport();
        string sqlConn = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
        //string strConnectionString = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
		public clsDLMemberWiseSearchReport()
		{
			try
			{
			//
			// TODO: Add constructor logic here
			//
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLMemberWiseSearchReport.cs", "clsDLMemberWiseSearchReport", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet getAllEmployee(clsMemberWiseSearchReport objclsMemberWiseSearchReport)
		{
			DataSet dsAllEmployees = new DataSet();
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter ("@EmployeeID",SqlDbType.Int);
			sqlParams[0].Value = objclsMemberWiseSearchReport.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			try
			{
                dsAllEmployees = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_Admin_Employees", sqlParams);
				return dsAllEmployees;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLMemberWiseSearchReport.cs", "getAllEmployee", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet getAllEmployees()
		{
			try
			{
                DataSet dsAllEmployees = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetAllEmployees");
			return dsAllEmployees;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLMemberWiseSearchReport.cs", "getAllEmployees", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		public DataSet getAllStatus()
		{
			try
			{
                DataSet dsAllStatus = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetAllStatus");
			return dsAllStatus;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLMemberWiseSearchReport.cs", "getAllStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

        /// <summary>
        /// get years from dates of logged issue
        /// </summary>
        /// <returns></returns>
        public DataSet getYears()
        {
            try
            {
                DataSet dsYears = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetYears");
                return dsYears;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLMemberWiseSearchReport.cs", "getYears", ex.StackTrace);
                throw new V2Exceptions();
            }
        }


        public DataSet GetStatus()
        {
            DataSet dsStatus = new DataSet();
            try
            {
                dsStatus = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetStatus");
                return dsStatus;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLMemberWiseSearchReport.cs", "GetStatus", ex.StackTrace);
                throw new V2Exceptions();
            }
        }


		public DataSet GetMemberWiseReport(clsMemberWiseSearchReport objClsMemberWiseSearchReport, int superAdmin)
		{
			try
			{
			SqlDateTime sqlNullValue;
			sqlNullValue = SqlDateTime.Null;
			//clsMemberWiseSearchReport objClsMemberWiseSearchReport = new clsMemberWiseSearchReport();
			SqlParameter[] sqlParams = new SqlParameter[9];
			sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 100);
			sqlParams[0].Value = objClsMemberWiseSearchReport.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			
			sqlParams[1] = new SqlParameter("@StatusID", SqlDbType.VarChar, 50);
			sqlParams[1].Value = objClsMemberWiseSearchReport.StatusID;
			sqlParams[1].Direction = ParameterDirection.Input;

			sqlParams[2] = new SqlParameter("@Date", SqlDbType.DateTime, 20);
			if(objClsMemberWiseSearchReport.Date == "")
				sqlParams[2].Value = sqlNullValue;
			else
				sqlParams[2].Value = objClsMemberWiseSearchReport.Date;
			sqlParams[2].Direction = ParameterDirection.Input;

			sqlParams[3] = new SqlParameter("@Month", SqlDbType.VarChar, 20);
			sqlParams[3].Value = objClsMemberWiseSearchReport.Month;
			sqlParams[3].Direction = ParameterDirection.Input;

			sqlParams[4] = new SqlParameter("@Year", SqlDbType.VarChar, 20);
			sqlParams[4].Value = objClsMemberWiseSearchReport.Year;
			sqlParams[4].Direction = ParameterDirection.Input;

			sqlParams[5] = new SqlParameter("@FromDate", SqlDbType.DateTime, 20);
			if(objClsMemberWiseSearchReport.FromDate == "")
				sqlParams[5].Value = sqlNullValue;
			else
				sqlParams[5].Value = objClsMemberWiseSearchReport.FromDate;
			sqlParams[5].Direction = ParameterDirection.Input;

			sqlParams[6] = new SqlParameter("@ToDate", SqlDbType.DateTime, 20);
			if(objClsMemberWiseSearchReport.ToDate == "")
                sqlParams[6].Value = sqlNullValue;
			else
				sqlParams[6].Value = objClsMemberWiseSearchReport.ToDate;
			sqlParams[6].Direction = ParameterDirection.Input;

			sqlParams[7] = new SqlParameter("@Period", SqlDbType.VarChar, 50);
			sqlParams[7].Value = objClsMemberWiseSearchReport.Period;
			sqlParams[7].Direction = ParameterDirection.Input;

            sqlParams[8] = new SqlParameter("@superAdmin", SqlDbType.Int);
			sqlParams[8].Value = superAdmin;
			sqlParams[8].Direction = ParameterDirection.Input;
                //superAdmin
            DataSet dsMemberWiseReport = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetMemberWiseReport_New", sqlParams);
			return dsMemberWiseReport;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLMemberWiseSearchReport.cs", "GetMemberWiseReport", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


	}
}
