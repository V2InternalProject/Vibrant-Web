using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using ModelHelpdesk;
using System.Configuration;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace DataLayerHelpDesk
{
	/// <summary>
	/// Summary description for clsDLStatus.
	/// </summary>
	public class clsDLStatus
	{
		string sqlConn = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();

		public clsDLStatus()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLStatus.cs", "clsDLStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetStatusList()
		{
			DataSet dsStatusList;
			dsStatusList = new DataSet();
			try
			{
				dsStatusList = SqlHelper.ExecuteDataset(sqlConn,CommandType.StoredProcedure,"sp_GetStatusList");
				return dsStatusList;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLStatus.cs", "GetStatusList", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		public bool AddNewStatus(clsStatus objStatus)
		{
			DataSet dsStatusList;
			dsStatusList = new DataSet();

			SqlParameter[] objParam = new SqlParameter[1];

			objParam[0] = new SqlParameter("@Status",SqlDbType.VarChar,100);
			objParam[0].Value = objStatus.StatusName;

			int recaffected1;
			try
			{
				recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn,CommandType.StoredProcedure,"sp_AddNewStatus",objParam);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLStatus.cs", "AddNewStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		

			return true;
		}

		public bool UpdateStatus(clsStatus objStatus)
		{
			DataSet dsStatusList;
			dsStatusList = new DataSet();

			SqlParameter[] objParam = new SqlParameter[2];

			objParam[0] = new SqlParameter("@StatusID",SqlDbType.Int);
			objParam[0].Value = objStatus.StatusID;

			objParam[1] = new SqlParameter("@Status",SqlDbType.VarChar,100);
			objParam[1].Value = objStatus.StatusName;

			int recaffected1;
			try
			{
				recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn,CommandType.StoredProcedure,"sp_UpdateStatus",objParam);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLStatus.cs", "UpdateStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		

			return true;
		}

		public bool DeleteStatus(clsStatus objStatus)
		{
			DataSet dsStatusList;
			dsStatusList = new DataSet();

			SqlParameter[] objParam = new SqlParameter[1];

			objParam[0] = new SqlParameter("@StatusID",SqlDbType.Int);
			objParam[0].Value = objStatus.StatusID;

			int recaffected1;
			try
			{
				recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn,CommandType.StoredProcedure,"sp_DeleteStatus",objParam);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLStatus.cs", "DeleteStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		
			return true;
		}

		public DataSet IsDuplicateStatus(clsStatus objStatus)
		{
			DataSet dsIsDuplicateStatus;

			SqlParameter[] objParam = new SqlParameter[2];

			objParam[0] = new SqlParameter("@StatusID",SqlDbType.Int);
			objParam[0].Value = objStatus.StatusID;

			objParam[1] = new SqlParameter("@Status",SqlDbType.VarChar,100);
			objParam[1].Value = objStatus.StatusName;

			try
			{
				dsIsDuplicateStatus = SqlHelper.ExecuteDataset(sqlConn,CommandType.StoredProcedure,"sp_IsDuplicateStatus",objParam);
				return dsIsDuplicateStatus;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLStatus.cs", "IsDuplicateStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

	}
}
