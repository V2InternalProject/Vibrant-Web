using System;
using System.Data;
using V2.Helpdesk.DataLayer;
using V2.Helpdesk.Model;
using V2.CommonServices.Exceptions;
using V2.CommonServices .FileLogger;

namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLCategory.
	/// </summary>
	public class clsBLCategory
	{
		public clsBLCategory()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategory.cs", "clsBLCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


		public DataSet getCategories()
		{
			try
			{
			clsDLCategory objclsDLCategory = new clsDLCategory();
			return objclsDLCategory.getCategories();
						  
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategory.cs", "getCategories", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet GetEmployeeName()
		{
			try
			{
			clsDLCategory objclsDLCategory = new clsDLCategory();
			return objclsDLCategory.GetEmployeeName();
						  
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategory.cs", "GetEmployeeName", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet GetCategoryEmployeeName(string CategoryName)
		{
			try
			{
			clsDLCategory objclsDLCategory = new clsDLCategory();
			return objclsDLCategory.GetCategoryEmployeeName(CategoryName);
						  
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategory.cs", "GetCategoryEmployeeName", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet GetCategoryEmployeeNames(clsCategory objClsCategory)
		{
			try
			{
			clsDLCategory objclsDLCategory = new clsDLCategory();
			return objclsDLCategory.GetCategoryEmployeeNames(objClsCategory);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategory.cs", "GetCategoryEmployeeNames", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		
		
		public int updateCategory(clsCategory objClsCategory)
		{
			try
			{
			clsDLCategory objclsDLCategory = new clsDLCategory();
			return objclsDLCategory.upDateCategory(objClsCategory);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategory.cs", "updateCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public int updateCategories(clsCategory objClsCategories)
		{
			try
			{
			clsDLCategory objclsDLCategories = new clsDLCategory();
			return objclsDLCategories.upDateCategories(objClsCategories);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategory.cs", "updateCategories", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public int InsertCategory(clsCategory objClsCategory)
		{
			try
			{
			clsDLCategory objClsDLCategory = new clsDLCategory();
			return objClsDLCategory.InsertCategory(objClsCategory);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategory.cs", "InsertCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public int DeleteCategory(clsCategory objClsCategory)
		{
			try
			{
			clsDLCategory objClsDLCategory = new clsDLCategory();
			return objClsDLCategory.DeleteCategory(objClsCategory);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategory.cs", "DeleteCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet DoesExistWhenEdited(clsCategory objClsCategory)
		{
			try
			{
			clsDLCategory objClsDLCategory = new clsDLCategory();
			return objClsDLCategory.DoesExistWhenEdited(objClsCategory);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategory.cs", "DoesExistWhenEdited", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public int DoesExist(clsCategory objClsCategory)
		{
			try
			{
			clsDLCategory objClsDLCategory = new clsDLCategory();
			return objClsDLCategory.DoesExist(objClsCategory);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLCategory.cs", "DoesExist", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
	}
}
