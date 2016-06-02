using System;
using ModelHelpdesk;
using DataLayerHelpDesk;
using System.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLViewMyIssues.
	/// </summary>
	public class clsBLViewMyIssues
	{
		clsDLViewMyIssues objDLViewMyIssues = new clsDLViewMyIssues();

		public clsBLViewMyIssues()
		{
			try
			{
			objDLViewMyIssues = new clsDLViewMyIssues();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "clsBLViewMyIssues", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

        public DataSet GetStatus()
        {
            try
            {
                return objDLViewMyIssues.GetStatus();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "GetStatus", ex.StackTrace);
                throw new V2Exceptions();
            }
 
        }



       

		public DataSet GetMyIssueList(clsViewMyIssues objViewMyIssues)
		{
			try
			{
				return objDLViewMyIssues.GetMyIssueList(objViewMyIssues);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "GetMyIssueList", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet GetSelectedIssue(clsViewMyIssues objViewIssue,int userid)
		{
			try
			{
				return objDLViewMyIssues.GetSelectedIssue(objViewIssue,userid);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "GetSelectedIssue", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

        public DataSet GetStatusAccToRole(int EmployeeID, int Status)
        {
            try
            {
                return objDLViewMyIssues.GetStatusAccToRole(EmployeeID, Status);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "GetStatusAccToRole", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

		public DataSet GetReportIssueHistory(clsViewMyIssues objViewIssue)
		{
			try
			{
				return objDLViewMyIssues.GetReportIssueHistory(objViewIssue);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "GetReportIssueHistory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetSuperAdminIssueList(clsViewMyIssues objViewMyIssues)
		{
			try
			{
				return objDLViewMyIssues.GetSuperAdminIssueList(objViewMyIssues);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "GetSuperAdminIssueList", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet GetSuperAdminIssue(clsViewMyIssues objViewMyIssues)
		{
			try
			{
				return objDLViewMyIssues.GetSuperAdminIssue(objViewMyIssues);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "GetSuperAdminIssue", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet MoveIssue(clsViewMyIssues objViewMyIssues)
		{
			try
			{
				return objDLViewMyIssues.MoveIssue(objViewMyIssues);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "MoveIssue", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetSuperAdminEmployees(clsViewMyIssues objViewMyIssues)
		{
			try
			{
				return objDLViewMyIssues.GetSuperAdminEmployees(objViewMyIssues);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "GetSuperAdminEmployees", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet BindCategory(clsViewMyIssues objViewMyIssues)
		{
			try
			{
				return objDLViewMyIssues.BindCategory(objViewMyIssues);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "BindCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet bindCategories(clsViewMyIssues objViewMyIssues)
		{
			try
			{
				return objDLViewMyIssues.bindCategories(objViewMyIssues);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "bindCategories", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet LoadDepartment(clsViewMyIssues objViewMyIssues)
		{
			try
			{
				return objDLViewMyIssues.LoadDepartment(objViewMyIssues);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "LoadDepartment", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet GetSelectedIssueforSuperAdmin(clsViewMyIssues objViewIssue,int userid)
		{
			try
			{
				return objDLViewMyIssues.GetSelectedIssueforSuperAdmin(objViewIssue,userid);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "GetSelectedIssueforSuperAdmin", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet bindCategory(clsViewMyIssues objViewIssue)
		{
			try
			{
				return objDLViewMyIssues.bindCategory(objViewIssue);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "bindCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet GetSuperAdminReportIssueHistory(clsViewMyIssues objViewIssue)
		{
			try
			{
				return objDLViewMyIssues.GetSuperAdminReportIssueHistory(objViewIssue);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLViewMyIssues.cs", "GetSuperAdminReportIssueHistory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

	}
}
