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
	/// Summary description for ProblemPriorityBusinessLogic.
	/// </summary>
	public class clsBLProblemPriority
	{
		clsDLProblemPriority objDLProblemPriority= new clsDLProblemPriority();

		public clsBLProblemPriority()
		{
			try
			{
			objDLProblemPriority = new clsDLProblemPriority();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLProblemPriority.cs", "clsBLProblemPriority", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetProblemPriorityList()
		{
			try
			{
				return objDLProblemPriority.GetProblemPriorityList();
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLProblemPriority.cs", "GetProblemPriorityList", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		
		public bool AddNewProblemPriority(clsProblemPriority objProblemPriority)
		{
			try
			{
				if (objDLProblemPriority.AddNewProblemPriority(objProblemPriority))
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLProblemPriority.cs", "AddNewProblemPriority", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		public int UpdateProblemPriority(clsProblemPriority objProblemPriority)
		{
			try
			{
			return objDLProblemPriority.UpdateProblemPriority(objProblemPriority);
//			try
//			{
//				if (objDLProblemPriority.UpdateProblemPriority(objProblemPriority))
//				{
//					return true;
//				}
//				else
//				{
//					return false;
//				}
//			}
//			catch (Exception ex)
//			{
//				throw ex;
//			}
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLProblemPriority.cs", "UpdateProblemPriority", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		public int DeleteProblemPriority(clsProblemPriority objProblemPriority)
		{
			try
			{
			return objDLProblemPriority.DeleteProblemPriority(objProblemPriority);
//			try
//			{
//				if (objDLProblemPriority.DeleteProblemPriority(objProblemPriority))
//				{
//					return true;
//				}
//				else
//				{
//					return false;
//				}
//			}
//			catch (Exception ex)
//			{
//				throw ex;
//			}
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLProblemPriority.cs", "DeleteProblemPriority", ex.StackTrace);
				throw new V2Exceptions();
			}
		}



		public DataSet IsDuplicateProblemPriority(clsProblemPriority objProblemPriority)
		{
			try
			{
				return objDLProblemPriority.IsDuplicateProblemPriority(objProblemPriority);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLProblemPriority.cs", "IsDuplicateProblemPriority", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


		public int CheckBeforeDeletingProblemPriority(clsProblemPriority objProblemPriority)
		{
			try
			{
				return objDLProblemPriority.CheckBeforeDeletingProblemPriority(objProblemPriority);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLProblemPriority.cs", "CheckBeforeDeletingProblemPriority", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

	}
}
