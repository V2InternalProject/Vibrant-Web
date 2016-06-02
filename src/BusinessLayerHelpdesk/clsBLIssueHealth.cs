using System;
using System.Data;
using ModelHelpdesk;
using DataLayerHelpDesk;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLIssueHealth.
	/// </summary>
	public class clsBLIssueHealth
	{
		clsDLIssueHealth objDLIssueHealth = new clsDLIssueHealth();

		public clsBLIssueHealth()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLIssueHealth.cs", "clsBLIssueHealth", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet bindData(clsIssueHealth objIssueHealth)
		{
			try
			{
          return objDLIssueHealth.bindData(objIssueHealth);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLIssueHealth.cs", "bindData", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

	}
}
