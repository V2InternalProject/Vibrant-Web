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
	/// Summary description for clsBLViewMyStatus.
	/// </summary>
	public class clsBLViewMyStatus
	{
        clsDLViewMyStatus objClsDLViewMyStatus;
		public clsBLViewMyStatus()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyStatus.cs", "clsBLViewMyStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet GetIssues(clsViewMyStatus objClsViewMyStatus)
		{
			try
			{
                clsDLViewMyStatus objClsDLViewMyStatus = new clsDLViewMyStatus();
                return objClsDLViewMyStatus.GetIssues(objClsViewMyStatus);
                
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyStatus.cs", "GetIssues", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet GetIssuesReport(clsViewMyStatus objClsViewMyStatus)
		{
			try
			{
			clsDLViewMyStatus objClsDLViewMyStatus = new clsDLViewMyStatus();
			return objClsDLViewMyStatus.GetIssuesReport(objClsViewMyStatus);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyStatus.cs", "GetIssuesReport", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetIssueDetails(clsViewMyStatus objClsViewMyStatus)
		{
			try
			{
			clsDLViewMyStatus objClsDLViewMyStatus = new clsDLViewMyStatus();
			return objClsDLViewMyStatus.GetIssueDetails(objClsViewMyStatus);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyStatus.cs", "GetIssueDetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		/*public DataSet GetStatusList()
		{
			clsDLViewMyStatus objClsDLViewMyStatus = new clsDLViewMyStatus();
			return objClsDLViewMyStatus.GetStatusList();
		}*/

		public int updateIssue(clsViewMyStatus objClsViewMyStatus)
		{
			try
			{
			clsDLViewMyStatus objClsDLViewMyStatus = new clsDLViewMyStatus();
			return objClsDLViewMyStatus.updateIssue(objClsViewMyStatus);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyStatus.cs", "updateIssue", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

        public DataSet GetEmployeeName(string name1)
        {
            try
            {
                clsDLViewMyStatus objClsDLViewMyStatus = new clsDLViewMyStatus();
                return objClsDLViewMyStatus.GetEmployeeName(name1);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyStatus.cs", "GetEmployeeName", ex.StackTrace);
                throw new V2Exceptions();
            }
        }


        public int updateIssue1(clsViewMyStatus objClsViewMyStatus, int counter,string userloginid)
        {
            try
            {
                clsDLViewMyStatus objClsDLViewMyStatus = new clsDLViewMyStatus();
                return objClsDLViewMyStatus.updateIssue1(objClsViewMyStatus,counter,userloginid);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyStatus.cs", "updateIssue", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

		public string getEmailID(clsViewMyStatus objClsViewMyStatus)
		{
			try
			{
			clsDLViewMyStatus objClsDLViewMyStatus = new clsDLViewMyStatus();
			return objClsDLViewMyStatus.getEmailID(objClsViewMyStatus);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyStatus.cs", "getEmailID", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
	}
}
