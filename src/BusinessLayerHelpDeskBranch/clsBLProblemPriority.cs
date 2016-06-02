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
	/// Summary description for ProblemPriorityBusinessLogic.
	/// </summary>
	public class clsBLProblemPriority
	{
		DataLayer.clsDLProblemPriority objDLProblemPriority= new clsDLProblemPriority();

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
				throw new V2Exceptions(ex.ToString(),ex);
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


		
		public bool AddNewProblemPriority(Model.clsProblemPriority objProblemPriority)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


		public int UpdateProblemPriority(Model.clsProblemPriority objProblemPriority)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


		public int DeleteProblemPriority(Model.clsProblemPriority objProblemPriority)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}



		public DataSet IsDuplicateProblemPriority(Model.clsProblemPriority objProblemPriority)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


		public int CheckBeforeDeletingProblemPriority(Model.clsProblemPriority objProblemPriority)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

	}
}
