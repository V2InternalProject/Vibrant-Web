using System;
using System.Data;
using System.Data.SqlClient;
using DataLayerHelpDesk;
using ModelHelpdesk;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLGraphIssueStatus.
	/// </summary>
	public class clsBLGraphIssueStatus
	{
		public clsBLGraphIssueStatus()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLGraphIssueStatus.cs", "clsBLGraphIssueStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet getProblemSeverity()
		{
			try
			{
			clsDLgraphIssueStatus objclsDLgraphIssueStatus = new clsDLgraphIssueStatus ();
			return objclsDLgraphIssueStatus.getProblemSeverity ();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLGraphIssueStatus.cs", "getProblemSeverity", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet getChartDetails(clsIssueStatus objclsIssueStatus)
		{
			try
			{
			clsDLgraphIssueStatus objclsDLgraphIssueStatus = new clsDLgraphIssueStatus ();
			return objclsDLgraphIssueStatus.getChartDetails (objclsIssueStatus);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLGraphIssueStatus.cs", "getChartDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		
	}
}
