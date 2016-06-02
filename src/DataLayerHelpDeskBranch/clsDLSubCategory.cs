using System;
using V2.Helpdesk.Model;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Helpdesk.DataLayer
{
	/// <summary>
	/// Summary description for clsDLSubCategory.
	/// </summary>
	public class clsDLSubCategory
	{
		string strConnectionString  = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
		public clsDLSubCategory()
		{
			try
			{
			}

			//
			// TODO: Add constructor logic here
			//
			
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategory.cs", "clsDLSubCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet getSubCategories()
		{
			try
			{
//			SqlParameter[] sqlParams = new SqlParameter[1];
//
//			sqlParams[0] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
//			sqlParams[0].Value = objClsCategory.CategoryID;
//			sqlParams[0].Direction = ParameterDirection.Input;
			DataSet dsSubCategories = new DataSet();
			dsSubCategories = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetSubCategories");
			return dsSubCategories;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategory.cs", "getSubCategories", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet getCategoryID()
		{
			try
			{
			DataSet dsCategoryID = new DataSet();
			dsCategoryID = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetActiveCategory");
			return dsCategoryID;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategory.cs", "getCategoryID", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		
		public DataSet getAllCategoryID(clsCategoryWiseSearchReport objCategorywiseSearchReport)
		{
			try
			{
			DataSet dsCategoryID = new DataSet();
			SqlParameter[] objParam = new SqlParameter[1];
			
			objParam[0] = new SqlParameter("@EmployeeID",SqlDbType.Int);
			objParam[0].Value = objCategorywiseSearchReport.EmployeeName;

			dsCategoryID = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "getDepartment",objParam);
			return dsCategoryID;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategory.cs", "getAllCategoryID", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public int upDateSubCategory(clsSubCategory objClsSubCategory)
		{
			try
			{
			//clsCategory objClsCategory = new clsCategory();
			int noOfRowsReturned = 0;
			SqlParameter[] sqlParams = new SqlParameter[4];
			sqlParams[0] = new SqlParameter("@SubCategoryID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsSubCategory.SubCategoryID;
			sqlParams[0].Direction = ParameterDirection.Input;

			sqlParams[1] = new SqlParameter("@CategoryID", SqlDbType.Int, 100);
			sqlParams[1].Value = objClsSubCategory.NewCategory;
			sqlParams[1].Direction = ParameterDirection.Input;

			sqlParams[2] = new SqlParameter("@SubCategory", SqlDbType.VarChar, 100);
			sqlParams[2].Value = objClsSubCategory.NewSubCategory;
			sqlParams[2].Direction = ParameterDirection.Input;

			sqlParams[3] = new SqlParameter("@IsActive", SqlDbType.Bit, 1);
			sqlParams[3].Value = objClsSubCategory.IsActive;
			sqlParams[3].Direction = ParameterDirection.Input;
			noOfRowsReturned = Convert.ToInt32(SqlHelper.ExecuteScalar(strConnectionString, CommandType.StoredProcedure, "sp_UpdateSubCategory", sqlParams));
			return noOfRowsReturned;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategory.cs", "upDateSubCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public int InsertSubCategory(clsSubCategory objClsSubCategory)
		{
			try
			{
			int noOfRowsAdded = 0;
			SqlParameter[] sqlParams = new SqlParameter[3];
			sqlParams[0] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsSubCategory.AddCategory;
			sqlParams[0].Direction = ParameterDirection.Input;
			
			sqlParams[1] = new SqlParameter("@SubCategory", SqlDbType.VarChar, 100);
			sqlParams[1].Value = objClsSubCategory.AddSubCategories;
			sqlParams[1].Direction = ParameterDirection.Input;

			sqlParams[2] = new SqlParameter("@IsActive", SqlDbType.Bit, 1);
			sqlParams[2].Value = objClsSubCategory.AddIsActive;
			sqlParams[2].Direction = ParameterDirection.Input;
			
			noOfRowsAdded = SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_InsertSubCategory", sqlParams);
			return noOfRowsAdded;

		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategory.cs", "InsertSubCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public int DeleteSubCategory(clsSubCategory objClsSubCategory)
		{
			try
			{
			int noOfRowsDeleted = 0;
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@SubCategoryID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsSubCategory.SubCategoryID;
			sqlParams[0].Direction = ParameterDirection.Input;
			noOfRowsDeleted = SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_DeleteSubCategory", sqlParams);
			return noOfRowsDeleted;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategory.cs", "DeleteSubCategory", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public int DoesExist(clsSubCategory objClsSubCategory)
		{
			try
			{
			int noOfRowsReturned = 0;
			SqlParameter[] sqlParams = new SqlParameter[2];

			sqlParams[0] = new SqlParameter("@SubCategory", SqlDbType.VarChar, 100);
			sqlParams[0].Value = objClsSubCategory.AddSubCategories;
			sqlParams[0].Direction = ParameterDirection.Input;

			sqlParams[1] = new SqlParameter("@CategoryID", SqlDbType.Int);
			sqlParams[1].Value = objClsSubCategory.AddCategory;
			sqlParams[1].Direction = ParameterDirection.Input;

			noOfRowsReturned = Convert.ToInt32(SqlHelper.ExecuteScalar(strConnectionString, CommandType.StoredProcedure, "sp_DoesSubCategoryExist", sqlParams));
			return noOfRowsReturned;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategory.cs", "DoesExist", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet DoesExistWhenEdited(clsSubCategory objClsSubCategory)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[2];
			
			sqlParams[0] = new SqlParameter("@SubCategory", SqlDbType.VarChar, 100);
			sqlParams[0].Value = objClsSubCategory.NewSubCategory;
			sqlParams[0].Direction = ParameterDirection.Input;

			sqlParams[1] = new SqlParameter("@CategoryID", SqlDbType.Int);
			sqlParams[1].Value = objClsSubCategory.NewCategory;
			sqlParams[1].Direction = ParameterDirection.Input;

			DataSet dsRowsReturned = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_DoesSubCategoryExist", sqlParams);
			return dsRowsReturned;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategory.cs", "DoesExistWhenEdited", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet GetEmployeeName(clsSubCategory objClsSubCategory)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			
			sqlParams[0] = new SqlParameter("@SubCategoryId", SqlDbType.VarChar, 100);
			sqlParams[0].Value = objClsSubCategory.SubCategoryID;
			sqlParams[0].Direction = ParameterDirection.Input;


			DataSet dsRowsReturned = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "GetEmployeeNames", sqlParams);
			return dsRowsReturned;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategory.cs", "GetEmployeeName", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
        public DataSet GetSubCategory(int category)
        {
            DataSet dsGetSubCategory = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@CategoryID", SqlDbType.Int);
            param[0].Value = category;
            return dsGetSubCategory = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetSubCategoryDynamic", param);
        }

	}
}
