using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ModelHelpdesk;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace DataLayerHelpDesk
{
	/// <summary>
	/// Summary description for clsDLLogin.
	/// </summary>
	public class clsDLLogin
	{
		String sqlConn = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();

		public clsDLLogin()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLLogin.cs", "clsDLLogin", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public int DoesEmployeeIDExist(clsLogin objLogin)
		{
			int recordcount;

			SqlParameter[] objParam = new SqlParameter[1];

			objParam[0] = new SqlParameter("@EmployeeID",SqlDbType.Int);
			objParam[0].Value = objLogin.EmployeeID;

			try
			{
				recordcount = Convert.ToInt32(SqlHelper.ExecuteScalar(sqlConn,CommandType.StoredProcedure,"sp_DoesEmployeeExist",objParam));
				return recordcount;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLLogin.cs", "DoesEmployeeIDExist", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


        public int isEmployeeSuperAdmin(clsLogin objLogin)
		{
			int recordcount;

			SqlParameter[] objParam = new SqlParameter[1];

			objParam[0] = new SqlParameter("@EmployeeID",SqlDbType.Int);
			objParam[0].Value = objLogin.EmployeeID;

			try
			{
                recordcount = Convert.ToInt32(SqlHelper.ExecuteScalar(sqlConn, CommandType.StoredProcedure, "IsEmployeeSuperAdmin", objParam));
				return recordcount;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLLogin.cs", "isEmployeeSuperAdmin", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		public DataSet IsEmployeeIDValid(clsLogin objLogin)
		{
			SqlParameter[] objParam = new SqlParameter[2];

			objParam[0] = new SqlParameter("@EmployeeID",SqlDbType.Int);
			objParam[0].Value = objLogin.EmployeeID;

			objParam[1] = new SqlParameter("@Password",SqlDbType.VarChar, 15);
			objParam[1].Value = objLogin.Password;

			DataSet dsEmployeeExists = new DataSet();
			try
			{
				dsEmployeeExists = SqlHelper.ExecuteDataset(sqlConn,CommandType.StoredProcedure,"sp_IsEmployeeIDValid",objParam);
				return dsEmployeeExists;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLLogin.cs", "IsEmployeeIDValid", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
	}
}
