using System;
using System.Data;
using ModelHelpdesk;
using DataLayerHelpDesk;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLLogin.
	/// </summary>
	public class clsBLLogin
	{
		clsDLLogin objDLLogin = new clsDLLogin();

		public clsBLLogin()
		{
			try
			{
			objDLLogin = new clsDLLogin();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLLogin.cs", "clsBLLogin", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public int DoesEmployeeIDExist(clsLogin objLogin)
		{
			try
			{
				return objDLLogin.DoesEmployeeIDExist(objLogin);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLLogin.cs", "DoesEmployeeIDExist", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet IsEmployeeIDValid(clsLogin objLogin)
		{
			try
			{
				return objDLLogin.IsEmployeeIDValid(objLogin);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLLogin.cs", "IsEmployeeIDValid", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


        public int isEmployeeSuperAdmin(clsLogin objLogin)
		{
			try
			{
                return objDLLogin.isEmployeeSuperAdmin(objLogin);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLLogin.cs", "isEmployeeSuperAdmin", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

	}
}
