using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using ModelHelpdesk;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace DataLayerHelpDesk
{
	/// <summary>
	/// Summary description for clsDLCategory.
	/// </summary>
	public class clsDLCategory
	{
		string strConnectionString  = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
		public clsDLCategory()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLCategory.cs", "clsDLCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		clsCategory objClsCategories = new clsCategory();
		clsCategory objClsCategory = new clsCategory();

		public DataSet getCategories()
		{
			try
			{
			DataSet dsCategories = new DataSet();
			dsCategories = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetCategories");
			return dsCategories;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLCategory.cs", "getCategories", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet GetEmployeeName()
		{
			try
			{
			DataSet dsCategories = new DataSet();
			dsCategories = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "getEmployeeName");
			return dsCategories;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLCategory.cs", "GetEmployeeName", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetCategoryEmployeeNames(clsCategory objClsCategory)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			
			sqlParams[0] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsCategory.CategoryID;
			sqlParams[0].Direction = ParameterDirection.Input;


			DataSet dsRowsReturned = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "GetCategoryEmployeeName", sqlParams);
			return dsRowsReturned;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLCategory.cs", "GetCategoryEmployeeNames", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetCategoryEmployeeName(string CategoryName)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
			sqlParams[0].Value = CategoryName;
			sqlParams[0].Direction = ParameterDirection.Input;

			DataSet dsCategories = new DataSet();
			//dsCategories = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "GetEmployeeNames",sqlParams);
			dsCategories = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_getEmployeeName1",sqlParams);
			return dsCategories;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLCategory.cs", "GetCategoryEmployeeName", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public int upDateCategory(clsCategory objClsCategory)
		{
			try
			{
			//clsCategory objClsCategory = new clsCategory();
			int noOfRowsReturned = 0;
			SqlParameter[] sqlParams = new SqlParameter[3];
			sqlParams[0] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsCategory.CategoryID;
			sqlParams[0].Direction = ParameterDirection.Input;

			sqlParams[1] = new SqlParameter("@Category", SqlDbType.VarChar, 100);
			sqlParams[1].Value = objClsCategory.NewCategory;
			sqlParams[1].Direction = ParameterDirection.Input;

			sqlParams[2] = new SqlParameter("@IsActive", SqlDbType.Bit, 1);
			sqlParams[2].Value = objClsCategory.isActive;
			sqlParams[2].Direction = ParameterDirection.Input;
			noOfRowsReturned = Convert.ToInt32(SqlHelper.ExecuteScalar(strConnectionString, CommandType.StoredProcedure, "sp_UpdateCategory", sqlParams));
			return noOfRowsReturned;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLCategory.cs", "upDateCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public int upDateCategories(clsCategory objClsCategories)
		{
			int noOfRowsReturned = 0;
			int employeeID = 0;
			try
			{
				
				SqlParameter[] sqlParams = new SqlParameter[4];
				sqlParams[0] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
				sqlParams[0].Value = objClsCategories.CategoryID;
				sqlParams[0].Direction = ParameterDirection.Input;

				sqlParams[1] = new SqlParameter("@Category", SqlDbType.VarChar, 100);
				sqlParams[1].Value = objClsCategories.Category;
				sqlParams[1].Direction = ParameterDirection.Input;

				sqlParams[2] = new SqlParameter("@IsActive", SqlDbType.Bit, 1);
				sqlParams[2].Value = objClsCategories.isActive;
				sqlParams[2].Direction = ParameterDirection.Input;
				
				if(objClsCategories.EmployeeId != "")
				{
					employeeID = Convert.ToInt32(objClsCategories.EmployeeId);
				}
				sqlParams[3] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
				sqlParams[3].Value = employeeID;
				sqlParams[3].Direction = ParameterDirection.Input;

				noOfRowsReturned = Convert.ToInt32(SqlHelper.ExecuteScalar(strConnectionString, CommandType.StoredProcedure, "sp_UpdateCategories", sqlParams));
			}
			catch(SqlException sqlexec)
			{
				throw sqlexec;
			}
			catch(NullReferenceException nullexec)
			{
				throw nullexec;
			}
			catch(Exception exec)
			{
				throw exec;
			}
			return noOfRowsReturned;
		}

		public int InsertCategory(clsCategory objClsCategory)
		{
			try
			{
			int noOfRowsReturned = 0;
			SqlParameter[] sqlParams = new SqlParameter[3];
			sqlParams[0] = new SqlParameter("@Category", SqlDbType.VarChar, 100);
			sqlParams[0].Value = objClsCategory.AddCategories;
			sqlParams[0].Direction = ParameterDirection.Input;
            
			sqlParams[1] = new SqlParameter("@IsActive", SqlDbType.Bit, 1);
			sqlParams[1].Value = objClsCategory.AddIdActive;
			sqlParams[1].Direction = ParameterDirection.Input;
			
			sqlParams[2] = new SqlParameter("@EmployeeId", SqlDbType.VarChar, 100);
			sqlParams[2].Value = objClsCategory.EmployeeId;
			sqlParams[2].Direction = ParameterDirection.Input;
            
			noOfRowsReturned = SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_InsertCategory", sqlParams);
			return noOfRowsReturned;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLCategory.cs", "InsertCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public int DeleteCategory(clsCategory objClsCategory)
		{
			try
			{
			int noOfRowsDeleted = 0;
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsCategory.CategoryID;
			sqlParams[0].Direction = ParameterDirection.Input;
			noOfRowsDeleted = SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_DeleteCategory", sqlParams);
			return noOfRowsDeleted;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLCategory.cs", "DeleteCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet DoesExistWhenEdited(clsCategory objClsCategory)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@Category", SqlDbType.VarChar, 100);
			sqlParams[0].Value = objClsCategory.NewCategory;
			sqlParams[0].Direction = ParameterDirection.Input;
			DataSet dsNoOfRowsReturned = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_DoesCategoryExist", sqlParams);
			return dsNoOfRowsReturned;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLCategory.cs", "DoesExistWhenEdited", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public int DoesExist(clsCategory objClsCategory)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@Category", SqlDbType.VarChar, 100);
			sqlParams[0].Value = objClsCategory.AddCategories;
			sqlParams[0].Direction = ParameterDirection.Input;
			int NoOfRowsReturned = Convert.ToInt32(SqlHelper.ExecuteScalar(strConnectionString, CommandType.StoredProcedure, "sp_DoesCategoryExist", sqlParams));
			return NoOfRowsReturned;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLCategory.cs", "DoesExist", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
	}
}
