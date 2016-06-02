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
	/// Summary description for clsBLProblemSeverity.
	/// </summary>
	public class clsBLProblemSeverity
	{
		clsDLProblemSeverity objDLProblemSeverity= new clsDLProblemSeverity();

		public clsBLProblemSeverity()
		{
			try
			{
			objDLProblemSeverity = new clsDLProblemSeverity();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLProblemSeverity.cs", "clsBLProblemSeverity", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetProblemSeverityList()
		{
			try
			{
				return objDLProblemSeverity.GetProblemSeverityList();
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLProblemSeverity.cs", "GetProblemSeverityList", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public bool AddNewProblemSeverity(clsProblemSeverity objProblemSeverity)
		{
			try
			{
				if (objDLProblemSeverity.AddNewProblemSeverity(objProblemSeverity))
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLProblemSeverity.cs", "AddNewProblemSeverity", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		public int UpdateProblemSeverity(clsProblemSeverity objProblemSeverity)
		{
			try
			{
				return objDLProblemSeverity.UpdateProblemSeverity(objProblemSeverity);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLProblemSeverity.cs", "UpdateProblemSeverity", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		public int DeleteProblemSeverity(clsProblemSeverity objProblemSeverity)
		{
			try
			{
			return	objDLProblemSeverity.DeleteProblemSeverity(objProblemSeverity);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLProblemSeverity.cs", "DeleteProblemSeverity", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		public DataSet IsDuplicateProblemSeverity(clsProblemSeverity objProblemSeverity)
		{
			try
			{
				return objDLProblemSeverity.IsDuplicateProblemSeverity(objProblemSeverity);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLProblemSeverity.cs", "IsDuplicateProblemSeverity", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


	}
}
