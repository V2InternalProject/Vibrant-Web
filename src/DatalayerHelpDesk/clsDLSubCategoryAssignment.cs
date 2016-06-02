using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using ModelHelpdesk;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace DataLayerHelpDesk
{
	/// <summary>
	/// Summary description for clsDLSubCategoryAssignment.
	/// </summary>
	public class clsDLSubCategoryAssignment
	{
		string strConnectionString = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
		//clsSubCategoryAssignment objSubCategoryAssignment = new clsSubCategoryAssignment();
		public clsDLSubCategoryAssignment()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "clsDLSubCategoryAssignment", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet getSubCategories()
		{
			try
			{
			DataSet dsSubCategories = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_BindSubCategoryForAssignMent");
			return dsSubCategories;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "getSubCategories", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet getAddSubCategory(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
            //SqlParameter[] sqlParams = new SqlParameter[1];
            //sqlParams[0] = new SqlParameter("@EmployeeId", SqlDbType.Int, 4);
            //sqlParams[0].Value = objClsSubCategoryAssignment.SuerAdminEmployeeId;
            //sqlParams[0].Direction = ParameterDirection.Input;
			DataSet dsSubCategories = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_BindSubCategoryForAssignMentNew");
			return dsSubCategories;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "getAddSubCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet getEmployeeNames()
		{
			try
			{
			DataSet dsEmployeeNames = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_BindEmployeeNames1");
			return dsEmployeeNames;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "getEmployeeNames", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet getData()
		{	
			try
			{
			DataSet dsGetData = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetCategoryID");
			return dsGetData;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "getData", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet getEmployeeName(clsSubCategoryAssignment objSubCategoryAssignment)
		{	
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@EmployeeId", SqlDbType.Int, 4);
			sqlParams[0].Value = objSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			DataSet dsGetData = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetCategoryName",sqlParams);
			return dsGetData;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "getEmployeeName", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		
		public DataSet CountNoOfCategory(clsSubCategoryAssignment objSubCategoryAssignment)
			{	
			try
			{
				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@EmployeeId", SqlDbType.Int, 4);
				sqlParams[0].Value = objSubCategoryAssignment.EmployeeID;
				sqlParams[0].Direction = ParameterDirection.Input;
				DataSet dsGetData = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_CountNoOfCategory",sqlParams);
				return dsGetData;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "CountNoOfCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet FindNoOfCategory(clsSubCategoryAssignment objSubCategoryAssignment)
		{	
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@EmployeeId", SqlDbType.Int, 4);
			sqlParams[0].Value = objSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			DataSet dsGetData = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_FindtNoOfActiveCategory",sqlParams);
			return dsGetData;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "FindNoOfCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet FetchRecordForEdit(clsSubCategoryAssignment objSubCategoryAssignment)
		{	
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@EmployeeId", SqlDbType.Int, 4);
			sqlParams[0].Value = objSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
            //sqlParams[1] = new SqlParameter("@SuperAdminEmployeeId", SqlDbType.Int, 4);
            //sqlParams[1].Value = objSubCategoryAssignment.SuerAdminEmployeeId;
            //sqlParams[1].Direction = ParameterDirection.Input;

			DataSet dsGetData = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetDetailOfEmployeeToSelected2",sqlParams);
			return dsGetData;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "FetchRecordForEdit", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet FetchRecordForEdit1(clsSubCategoryAssignment objSubCategoryAssignment)
		{	
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@EmployeeId", SqlDbType.Int, 4);
			sqlParams[0].Value = objSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
            //sqlParams[1] = new SqlParameter("@SuperAdminEmployeeId", SqlDbType.Int, 4);
            //sqlParams[1].Value = objSubCategoryAssignment.SuerAdminEmployeeId;
            //sqlParams[1].Direction = ParameterDirection.Input;

			DataSet dsGetData = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetDetailOfEmployeeToAvailable2",sqlParams);
			return dsGetData;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "FetchRecordForEdit1", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		
		public DataSet getEmployeedetails()
		{	
			try
			{
			DataSet dsGetData = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetEmployeeDetail");
			return dsGetData;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "getEmployeedetails", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet getDetialOfEmployee(clsSubCategoryAssignment objSubCategoryAssignment)
		{	
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@EmployeeId", SqlDbType.Int, 4);
			sqlParams[0].Value = objSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			DataSet dsGetData = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetDetailOfEmployee",sqlParams);
			return dsGetData;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "getDetialOfEmployee", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet CheckForSuperUser(clsSubCategoryAssignment objSubCategoryAssignment)
		{	
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@EmployeeId", SqlDbType.Int, 4);
			sqlParams[0].Value = objSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			DataSet dsGetData = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetDetailOfEmpStatus",sqlParams);
			return dsGetData;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "CheckForSuperUser", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public void InsertEmployee(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[4];
			sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			
			sqlParams[1] = new SqlParameter("@EmployeeEmailID", SqlDbType.VarChar, 100);
			sqlParams[1].Value = objClsSubCategoryAssignment.EmployeeEmailID;
			sqlParams[1].Direction = ParameterDirection.Input;
			
			sqlParams[2] = new SqlParameter("@EmployeeName", SqlDbType.VarChar, 100);
			sqlParams[2].Value = objClsSubCategoryAssignment.EmployeeName;
			sqlParams[2].Direction = ParameterDirection.Input;
			
			sqlParams[3] = new SqlParameter("@IsActive", SqlDbType.Bit, 1);
			sqlParams[3].Value = objClsSubCategoryAssignment.IsActive;
			sqlParams[3].Direction = ParameterDirection.Input;
			
			SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_InsertEmployee", sqlParams);
        }	
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "InsertEmployee", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public string getEmployeeEmailID(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			string strEmployeeEmailID = (SqlHelper.ExecuteScalar(strConnectionString, CommandType.StoredProcedure, "sp_getEmployeeEmailID", sqlParams)).ToString();
			return strEmployeeEmailID;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "getEmployeeEmailID", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public void AddEmpRole(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			SqlHelper.ExecuteScalar(strConnectionString, CommandType.StoredProcedure, "sp_AddEmpRole", sqlParams);
			
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "AddEmpRole", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public void CheckForSuperAdmin(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[2];
			sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			sqlParams[1] = new SqlParameter("@CategoryId", SqlDbType.Int, 4);
			sqlParams[1].Value = objClsSubCategoryAssignment.CategoryID;
			sqlParams[1].Direction = ParameterDirection.Input;
			SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_CheckSuperAdmin", sqlParams);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "CheckForSuperAdmin", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public void CheckForSuperAdmin1(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[2];
			sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			sqlParams[1] = new SqlParameter("@CategoryId", SqlDbType.Int, 4);
			sqlParams[1].Value = objClsSubCategoryAssignment.CategoryID;
			sqlParams[1].Direction = ParameterDirection.Input;
			SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_CheckSuperAdmin1", sqlParams);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "CheckForSuperAdmin1", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public void InsertEmployeeRoles(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[3];
			sqlParams[0] = new SqlParameter("@IsAdmin", SqlDbType.Int, 1);
			sqlParams[0].Value = objClsSubCategoryAssignment.IsSysAdmin;
			sqlParams[0].Direction = ParameterDirection.Input;

			sqlParams[1] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
			sqlParams[1].Value = objClsSubCategoryAssignment.CategoryID;
			sqlParams[1].Direction = ParameterDirection.Input;

			sqlParams[2] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[2].Value = objClsSubCategoryAssignment.EmployeeID;
			sqlParams[2].Direction = ParameterDirection.Input;

			SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_InsertEmployeeRoles", sqlParams);
        }
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "InsertEmployeeRoles", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public void InsertSubCategoryAssignment(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[2];

			sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;

			sqlParams[1] = new SqlParameter("@subCategoryID", SqlDbType.VarChar, 100);
			sqlParams[1].Value = objClsSubCategoryAssignment.SubCategoryID;
			sqlParams[1].Direction = ParameterDirection.Input;

			SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_InsertSubCategoryAssignment", sqlParams);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "InsertSubCategoryAssignment", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public void UpdateSubCategoryAssignment(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[2];

			sqlParams[0] = new SqlParameter("@subCategoryID", SqlDbType.VarChar, 5000);
			sqlParams[0].Value = objClsSubCategoryAssignment.SubCategoryID;
			sqlParams[0].Direction = ParameterDirection.Input;

			sqlParams[1] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[1].Value = objClsSubCategoryAssignment.EmployeeID;
			sqlParams[1].Direction = ParameterDirection.Input;

            //sqlParams[2] = new SqlParameter("@SuperAdminEmployeeID", SqlDbType.Int, 4);
            //sqlParams[2].Value = objClsSubCategoryAssignment.SuerAdminEmployeeId;
            //sqlParams[2].Direction = ParameterDirection.Input;

			SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_UpdateSubCategoryAssignmentNew", sqlParams);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "UpdateSubCategoryAssignment", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public void AddSubCategoryAssignment(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[2];

			sqlParams[0] = new SqlParameter("@subCategoryID", SqlDbType.VarChar, 100);
			sqlParams[0].Value = objClsSubCategoryAssignment.SubCategoryID;
			sqlParams[0].Direction = ParameterDirection.Input;


			sqlParams[1] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[1].Value = objClsSubCategoryAssignment.EmployeeID;
			sqlParams[1].Direction = ParameterDirection.Input;

			SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_AddSubCategoryAssignment", sqlParams);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "AddSubCategoryAssignment", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		
		public void deleteSubCategory(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[2];

			sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			sqlParams[1] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
			sqlParams[1].Value = objClsSubCategoryAssignment.CategoryID;
			sqlParams[1].Direction = ParameterDirection.Input;

			SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_DeleteAssignSubCategory", sqlParams);
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "deleteSubCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public int DoesEmployeeExist(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			int noOfRowsReturned = 0;
			SqlParameter[] sqlParams = new SqlParameter[1];

			sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;

			noOfRowsReturned = Convert.ToInt32(SqlHelper.ExecuteScalar(strConnectionString, CommandType.StoredProcedure, "sp_DoesEmployeeExist", sqlParams));
			return noOfRowsReturned;
			
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "DoesEmployeeExist", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public void CheckSuperAdmin(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			
			SqlParameter[] sqlParams = new SqlParameter[2];

			sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			sqlParams[1] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
			sqlParams[1].Value = objClsSubCategoryAssignment.CategoryID;
			sqlParams[1].Direction = ParameterDirection.Input;
			SqlHelper.ExecuteScalar(strConnectionString, CommandType.StoredProcedure, "sp_CheckSuperAdmin", sqlParams);
		
			
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "CheckSuperAdmin", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public void CheckSuperAdmin1(clsSubCategoryAssignment objClsSubCategoryAssignment)
		{
			try
			{
			
			SqlParameter[] sqlParams = new SqlParameter[2];

			sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsSubCategoryAssignment.EmployeeID;
			sqlParams[0].Direction = ParameterDirection.Input;
			sqlParams[1] = new SqlParameter("@CategoryID", SqlDbType.Int, 4);
			sqlParams[1].Value = objClsSubCategoryAssignment.CategoryID;
			sqlParams[1].Direction = ParameterDirection.Input;
			SqlHelper.ExecuteScalar(strConnectionString, CommandType.StoredProcedure, "sp_CheckSuperAdmin1", sqlParams);
		
			
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "CheckSuperAdmin1", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetLoginUserList(clsSubCategoryAssignment objSubCategoryAssignment)
		{
			try
			{
			DataSet dsLoginUserList = new DataSet();
			SqlParameter[] objParam = new SqlParameter[1];

			objParam[0] = new SqlParameter("@SubCategoryID", SqlDbType.Int);
			objParam[0].Value = objSubCategoryAssignment.SubCategoryID;

			dsLoginUserList = SqlHelper.ExecuteDataset(strConnectionString,CommandType.StoredProcedure,"sp_GetLoginUserList", objParam);
			return dsLoginUserList;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "GetLoginUserList", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetAssignedSubCategories(clsSubCategoryAssignment objSubCategoryAssignment)
		{
			try
			{
			DataSet dsAssignedSubCategories = new DataSet();
			SqlParameter[] objParam = new SqlParameter[1];

			objParam[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
			objParam[0].Value = objSubCategoryAssignment.EmployeeID;

			dsAssignedSubCategories = SqlHelper.ExecuteDataset(strConnectionString,CommandType.StoredProcedure,"sp_GetAssignedSubCategories",objParam);
			return dsAssignedSubCategories;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLSubCategoryAssignment.cs", "GetAssignedSubCategories", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

	}
}