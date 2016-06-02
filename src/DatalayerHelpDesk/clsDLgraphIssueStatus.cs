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
	/// Summary description for ClsgraphIssueStatus.
	/// </summary>
	public class clsDLgraphIssueStatus
	{
		string strConnectionString = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();

		public clsDLgraphIssueStatus()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLgraphIssueStatus.cs", "clsDLgraphIssueStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet getProblemSeverity()
		{
			try
			{
				DataSet dsProblemSeverity = SqlHelper.ExecuteDataset (strConnectionString,CommandType.StoredProcedure ,"sp_ProblemSeverity");
				return dsProblemSeverity;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLgraphIssueStatus.cs", "getProblemSeverity", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet getChartDetails(clsIssueStatus objclsIssueStatus )
		{
			
			DataSet dsgetChartDetails;
			SqlParameter [] objparam = new SqlParameter [3];

			objparam [0] = new SqlParameter ("@startMonth", System.Data.SqlDbType.DateTime );
			objparam [0].Value = objclsIssueStatus.StartMonth ;

			objparam [1] = new SqlParameter ("@endMonth",System.Data.SqlDbType.DateTime );
			objparam [1].Value = objclsIssueStatus.EndMonth ;

			objparam[2] = new SqlParameter ("@problemseverityid",System.Data.SqlDbType.Int );
			objparam[2].Value = objclsIssueStatus.problemSeverityId ;
			try
			{
				dsgetChartDetails = SqlHelper.ExecuteDataset (strConnectionString,CommandType.StoredProcedure,"sp_GetGraphicalIssues",objparam);
				
				return dsgetChartDetails;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLgraphIssueStatus.cs", "getChartDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
	}
}
