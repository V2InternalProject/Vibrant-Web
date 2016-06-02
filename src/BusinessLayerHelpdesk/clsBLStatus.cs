using System;
using ModelHelpdesk;
using DataLayerHelpDesk;
using System.Data;
using System.Data.SqlClient;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for StatusBusinessLogic.
	/// </summary>
	public class clsBLStatus
	{
		clsDLStatus objDLStatus= new clsDLStatus();

		public clsBLStatus()
		{
			try
			{
			objDLStatus = new clsDLStatus();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLStatus.cs", "clsBLStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetStatusList()
		{
			try
			{
				return objDLStatus.GetStatusList();
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLStatus.cs", "GetStatusList", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		public bool AddNewStatus(clsStatus objStatus)
		{
			try
			{
				if (objDLStatus.AddNewStatus(objStatus))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLStatus.cs", "AddNewStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		public bool UpdateStatus(clsStatus objStatus)
		{
			try
			{
				if (objDLStatus.UpdateStatus(objStatus))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLStatus.cs", "UpdateStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public bool DeleteStatus(clsStatus objStatus)
		{
			try
			{
				if (objDLStatus.DeleteStatus(objStatus))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLStatus.cs", "DeleteStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		public DataSet IsDuplicateStatus(clsStatus objStatus)
		{
			try
			{
				return objDLStatus.IsDuplicateStatus(objStatus);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLStatus.cs", "IsDuplicateStatus", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


	}
}
