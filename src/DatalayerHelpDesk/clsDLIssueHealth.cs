using System;
using ModelHelpdesk;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Configuration ;
using Microsoft.ApplicationBlocks.Data ;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.DataLayer;

namespace DataLayerHelpDesk 
{
	/// <summary>
	/// Summary description for clsDLIssueHealth.
	/// </summary>
	public class clsDLIssueHealth
	{
		string connectionString = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
		public clsDLIssueHealth()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLIssueHealth.cs", "clsDLIssueHealth", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet bindData(clsIssueHealth objIssueHealth)
		{
			try
			{
			DataSet dsIssueStatus;

			SqlParameter []objparam = new SqlParameter [1];

			objparam [0] = new SqlParameter ("@EmployeeId", System.Data.SqlDbType.Int );
			objparam [0].Value = objIssueHealth.EmployeeID ;
			dsIssueStatus=SqlHelper.ExecuteDataset(connectionString ,CommandType.StoredProcedure ,"sp_getIssueHealth_test" ,objparam);
		   return dsIssueStatus;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLIssueHealth.cs", "bindData", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
	}
}
