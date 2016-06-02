using System;
using System.Data;
using V2.Helpdesk.DataLayer;
using V2.Helpdesk.Model;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLResolutionTime.
	/// </summary>
	public class clsBLResolutionTime
	{
		clsDLResolutionTime  objclsDLResolutionTime = new clsDLResolutionTime ();
		public clsBLResolutionTime()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTime.cs", "clsBLResolutionTime", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet getCategory()
		{
			try
			{
			 return objclsDLResolutionTime.getCategory ();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTime.cs", "getCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet getSubCategory(clsResolutionTime objclsResolutionTime)
		{
			try
			{
			return objclsDLResolutionTime.getSubCategory (objclsResolutionTime);
			//return objclsDLResolutionTime.getSubCategory();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTime.cs", "getSubCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet getProblemSeverity()
		{
			try
			{
			return objclsDLResolutionTime.getProblemSeverity ();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTime.cs", "getProblemSeverity", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public int addResolutionTime(clsResolutionTime objclsResolutionTime)
		{
			try
			{
			return objclsDLResolutionTime.addResolutionTime (objclsResolutionTime);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTime.cs", "addResolutionTime", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public int updateResolutionTime(clsResolutionTime objclsResolutionTime)
		{
			try
			{
			return objclsDLResolutionTime.updateResolutionTime (objclsResolutionTime);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTime.cs", "updateResolutionTime", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public int deleteResolutionTime(clsResolutionTime objclsResolutionTime)
		{
			try
			{
			return objclsDLResolutionTime.deleteResolutionTime(objclsResolutionTime);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTime.cs", "deleteResolutionTime", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet IsDuplicateResolution (clsResolutionTime  objclsResolutionTime )
		{
			try
			{
			return objclsDLResolutionTime.IsDuplicateResolution(objclsResolutionTime);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTime.cs", "IsDuplicateResolution", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet getResolutionTime()
		{
			try
			{
			return objclsDLResolutionTime.getResolutionTime ();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLResolutionTime.cs", "getResolutionTime", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
	}
}
