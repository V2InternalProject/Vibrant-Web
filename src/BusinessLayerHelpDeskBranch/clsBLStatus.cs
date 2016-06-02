using System;
using V2.Helpdesk.Model;
using V2.Helpdesk.DataLayer;
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
		DataLayer.clsDLStatus objDLStatus= new clsDLStatus();

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
				throw new V2Exceptions(ex.ToString(),ex);
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


		public bool AddNewStatus(Model.clsStatus objStatus)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


		public bool UpdateStatus(Model.clsStatus objStatus)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public bool DeleteStatus(Model.clsStatus objStatus)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


		public DataSet IsDuplicateStatus(Model.clsStatus objStatus)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


	}
}
