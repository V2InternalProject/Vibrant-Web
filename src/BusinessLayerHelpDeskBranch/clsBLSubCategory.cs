using System;
using V2.Helpdesk.DataLayer;
using V2.Helpdesk.Model;
using System.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Helpdesk.BusinessLayer
{
	/// <summary>
	/// Summary description for clsBLSubCategory.
	/// </summary>
	public class clsBLSubCategory
	{
		public clsBLSubCategory()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategory.cs", "clsBLSubCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet getSubCategories()
		{
			try
			{
			clsDLSubCategory objclsDLSubCategory = new clsDLSubCategory();
			return objclsDLSubCategory.getSubCategories();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategory.cs", "getSubCategories", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet getCategoryID()
		{
			try
			{
			clsDLSubCategory objClsDLSubCategory = new clsDLSubCategory();
			return objClsDLSubCategory.getCategoryID();
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategory.cs", "getCategoryID", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet getAllCategoryID(clsCategoryWiseSearchReport objCategorywiseSearchReport)
		{
			try
			{
			clsDLSubCategory objClsDLSubCategory = new clsDLSubCategory();
			return objClsDLSubCategory.getAllCategoryID(objCategorywiseSearchReport);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategory.cs", "getAllCategoryID", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public int updateSubCategory(clsSubCategory objClsSubCategory)
		{
			try
			{
			clsDLSubCategory objClsDLSubCategory = new clsDLSubCategory();
			return objClsDLSubCategory.upDateSubCategory(objClsSubCategory);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategory.cs", "updateSubCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public int InsertSubCategory(clsSubCategory objClsSubCategory)
		{
			try
			{
			clsDLSubCategory objClsDLSubCategory = new clsDLSubCategory();
			return objClsDLSubCategory.InsertSubCategory(objClsSubCategory);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategory.cs", "InsertSubCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public int DeleteSubCategory(clsSubCategory objClsSubCategory)
		{
			try
			{
			clsDLSubCategory objClsDLSubCategory = new clsDLSubCategory();
			return objClsDLSubCategory.DeleteSubCategory(objClsSubCategory);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategory.cs", "DeleteSubCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public int DoesExist(clsSubCategory objClsSubCategory)
		{
			try
			{
			clsDLSubCategory objClsDLSubCategory = new clsDLSubCategory();
			return objClsDLSubCategory.DoesExist(objClsSubCategory);
		}	
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategory.cs", "DoesExist", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet DoesExistWhenEdited(clsSubCategory objClsSubCategory)
		{
			try
			{
			clsDLSubCategory objClsDLSubCategory = new clsDLSubCategory();
			return objClsDLSubCategory.DoesExistWhenEdited(objClsSubCategory);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategory.cs", "DoesExistWhenEdited", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet GetEmployeeName(clsSubCategory objClsSubCategory)
		{
			try
			{
			clsDLSubCategory objClsDLSubCategory = new clsDLSubCategory();
			return objClsDLSubCategory.GetEmployeeName(objClsSubCategory);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsBLSubCategory.cs", "GetEmployeeName", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
        public DataSet GetSubCategory(int category)
        {
            DataSet dsGetSubCategory = new DataSet();
            clsDLSubCategory objClsDLSubCategory = new clsDLSubCategory();
            dsGetSubCategory = objClsDLSubCategory.GetSubCategory(category);
            return dsGetSubCategory;
        }


	}
}
