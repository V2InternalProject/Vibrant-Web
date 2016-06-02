using System;
using System.Data;
using V2.Helpdesk.DataLayer;
using V2.Helpdesk.Model;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLSubCategoryAssignment.
	/// </summary>
	public class clsBLSubCategoryAssignment
	{
		public clsBLSubCategoryAssignment()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "clsBLSubCategoryAssignment", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet getAddSubCategory(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			return objClsDLSubCategoryAssignment.getAddSubCategory(objClsSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "getAddSubCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet getSubCategories()
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			return objClsDLSubCategoryAssignment.getSubCategories();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "getSubCategories", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet getEmployeeNames()
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			return objClsDLSubCategoryAssignment.getEmployeeNames();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "getEmployeeNames", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet CountNoOfCategory(clsSubCategoryAssignment objSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			return objClsDLSubCategoryAssignment.CountNoOfCategory(objSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "CountNoOfCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet FindNoOfCategory(clsSubCategoryAssignment objSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			return objClsDLSubCategoryAssignment.FindNoOfCategory(objSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "FindNoOfCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet FetchRecordForEdit(clsSubCategoryAssignment objSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			return objClsDLSubCategoryAssignment.FetchRecordForEdit(objSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "FetchRecordForEdit", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet FetchRecordForEdit1(clsSubCategoryAssignment objSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			return objClsDLSubCategoryAssignment.FetchRecordForEdit1(objSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "FetchRecordForEdit1", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet getData()
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			return objClsDLSubCategoryAssignment.getData();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "getData", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

        public DataSet getEmployeedetails(clsSubCategoryAssignment objSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
            return objClsDLSubCategoryAssignment.getEmployeedetails(objSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "getEmployeedetails", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet getEmployeeName(clsSubCategoryAssignment objSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			return objClsDLSubCategoryAssignment.getEmployeeName(objSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "getEmployeeName", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		
		public DataSet getDetialOfEmployee(clsSubCategoryAssignment objSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			return objClsDLSubCategoryAssignment.getDetialOfEmployee(objSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "getDetialOfEmployee", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet CheckForSuperUser(clsSubCategoryAssignment objSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			return objClsDLSubCategoryAssignment.CheckForSuperUser(objSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "CheckForSuperUser", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public void InsertEmployee(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			objClsDLSubCategoryAssignment.InsertEmployee(objClsSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "InsertEmployee", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public string getEmployeeEmailID(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			return objClsDLSubCategoryAssignment.getEmployeeEmailID(objClsSubCategoryAssignment);
		
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "getEmployeeEmailID", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public void InsertEmployeeRoles(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			objClsDLSubCategoryAssignment.InsertEmployeeRoles(objClsSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "InsertEmployeeRoles", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public void InsertSubCategoryAssignment(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			objClsDLSubCategoryAssignment.InsertSubCategoryAssignment(objClsSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "InsertSubCategoryAssignment", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public void CheckForSuperAdmin(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			objClsDLSubCategoryAssignment.CheckForSuperAdmin(objClsSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "CheckForSuperAdmin", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public void CheckForSuperAdmin1(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			objClsDLSubCategoryAssignment.CheckForSuperAdmin1(objClsSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "CheckForSuperAdmin1", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public void UpdateSubCategoryAssignment(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			objClsDLSubCategoryAssignment.UpdateSubCategoryAssignment(objClsSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "UpdateSubCategoryAssignment", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public void AddSubCategoryAssignment(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			objClsDLSubCategoryAssignment.AddSubCategoryAssignment(objClsSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "AddSubCategoryAssignment", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public void AddEmpRole(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			objClsDLSubCategoryAssignment.AddEmpRole(objClsSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "AddEmpRole", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public void CheckSuperAdmin(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			objClsDLSubCategoryAssignment.CheckSuperAdmin(objClsSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "CheckSuperAdmin", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		
		public void CheckSuperAdmin1(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			objClsDLSubCategoryAssignment.CheckSuperAdmin1(objClsSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "CheckSuperAdmin1", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		
		public void deleteSubCategory(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			objClsDLSubCategoryAssignment.deleteSubCategory(objClsSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "deleteSubCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		

		public int DoesEmployeeExist(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
			return objClsDLSubCategoryAssignment.DoesEmployeeExist(objClsSubCategoryAssignment);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "DoesEmployeeExist", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet GetLoginUserList(Model.clsSubCategoryAssignment objSubCategoryAssignment)
		{
			try
			{
				clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
				return objClsDLSubCategoryAssignment.GetLoginUserList(objSubCategoryAssignment);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "GetLoginUserList", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet GetAssignedSubCategories(Model.clsSubCategoryAssignment objSubCategoryAssignment)
		{
			try
			{
				clsDLSubCategoryAssignment objClsDLSubCategoryAssignment = new clsDLSubCategoryAssignment();
				return objClsDLSubCategoryAssignment.GetAssignedSubCategories(objSubCategoryAssignment);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategoryAssignment.cs", "GetAssignedSubCategories", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

	}
}
