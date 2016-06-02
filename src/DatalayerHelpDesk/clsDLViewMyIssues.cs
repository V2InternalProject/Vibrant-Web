using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using ModelHelpdesk;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;



namespace DataLayerHelpDesk
{
	/// <summary>
	/// Summary description for clsDLViewMyIssues.
	/// </summary>
	public class clsDLViewMyIssues
	{
		string sqlConn = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();

		public clsDLViewMyIssues()
		{
			try
			{
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "clsDLViewMyIssues", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


        public DataSet GetStatus()
        {
            DataSet dsStatus = new DataSet();
            try
            {
                dsStatus = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetStatusForIssues");
                return dsStatus;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "GetStatusForIssues", ex.StackTrace);
                throw new V2Exceptions();
            }
        }


        


		public DataSet GetMyIssueList(ModelHelpdesk.clsViewMyIssues objViewMyIssues)
		{
			
			DataSet dsIssueList;
			dsIssueList = new DataSet();

			SqlParameter[] objParam = new SqlParameter[2];
			objParam[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
			objParam[0].Value = objViewMyIssues.EmployeeID;
			objParam[1] = new SqlParameter("@StatusID", SqlDbType.NVarChar);
			objParam[1].Value = objViewMyIssues.SelectedStatus;
			try
			{
				dsIssueList = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"sp_GetIssueListNew1",objParam);
				return dsIssueList;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "GetMyIssueList", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetSelectedIssue(clsViewMyIssues objViewIssue,int userid)
		{
			DataSet dsSelectedIssue;
			dsSelectedIssue = new DataSet();

			SqlParameter[] objParam = new SqlParameter[2];
			
			objParam[0] = new SqlParameter("@ReportIssueID",SqlDbType.Int);
			objParam[0].Value = objViewIssue.ReportIssueID;
			objParam[0].Direction = ParameterDirection.Input;

            objParam[1] = new SqlParameter("@UserID", SqlDbType.Int);
            objParam[1].Value = userid;
            objParam[1].Direction = ParameterDirection.Input;
			try
			{
				dsSelectedIssue = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"sp_GetSelectedIssue",objParam);
				return dsSelectedIssue;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "GetSelectedIssue", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


        public DataSet GetStatusAccToRole(int EmployeeID, int Status)
        {

            DataSet dsIssueList;
            dsIssueList = new DataSet();

            SqlParameter[] objParam = new SqlParameter[2];
            objParam[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
            objParam[0].Value = EmployeeID;
            objParam[1] = new SqlParameter("@Status", SqlDbType.Int);
            objParam[1].Value = Status;
            //objParam[1] = new SqlParameter("@StatusID", SqlDbType.NVarChar);
            //objParam[1].Value = objViewMyIssues.SelectedStatus;
            try
            {
                dsIssueList = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetStatusAccToRole", objParam);
                return dsIssueList;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "GetStatusAccToRole", ex.StackTrace);
                throw new V2Exceptions();
            }
        }



		public DataSet GetReportIssueHistory(clsViewMyIssues objViewIssue)
		{
			DataSet dsReportIssueHistory;
			dsReportIssueHistory = new DataSet();

			SqlParameter[] objParam = new SqlParameter[1];
			objParam[0] = new SqlParameter("@ReportIssueID",SqlDbType.Int);
			objParam[0].Value = objViewIssue.ReportIssueID;
			objParam[0].Direction = ParameterDirection.Input;

//			objParam[1] = new SqlParameter("@StatusID", SqlDbType.Int);
//			objParam[1].Value = objViewIssue.StatusID;
//			objParam[1].Direction = ParameterDirection.Input;

			try
			{
				dsReportIssueHistory = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"sp_GetReportIssueHistory",objParam);
				return dsReportIssueHistory;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "GetReportIssueHistory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet GetSuperAdminIssueList(clsViewMyIssues objViewIssue)
		{
			DataSet dsSelectedIssue;
			dsSelectedIssue = new DataSet();

			SqlParameter[] objParam = new SqlParameter[4];
			
			objParam[0] = new SqlParameter("@EmployeeID",SqlDbType.Int);
			objParam[0].Value = objViewIssue.EmployeeID;
			objParam[1] = new SqlParameter("@Status",SqlDbType.NVarChar);
			objParam[1].Value = objViewIssue.SelectedStatus;
			objParam[2] = new SqlParameter("@Name",SqlDbType.VarChar);
			objParam[2].Value = objViewIssue.SelectedEmployee;
			objParam[3] = new SqlParameter("@Category",SqlDbType.VarChar);
			objParam[3].Value = objViewIssue.Category;
			try
			{
				dsSelectedIssue = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"sp_GetSuperAdminIssueListNew1",objParam);
				return dsSelectedIssue;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "GetSuperAdminIssueList", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet GetSuperAdminIssue(clsViewMyIssues objViewIssue)
		{
			DataSet dsSelectedIssue;
			dsSelectedIssue = new DataSet();

			SqlParameter[] objParam = new SqlParameter[2];
			
			objParam[0] = new SqlParameter("@EmployeeID",SqlDbType.Int);
			objParam[0].Value = objViewIssue.EmployeeID;

			objParam[1] = new SqlParameter("@Category",SqlDbType.Int);
			objParam[1].Value = objViewIssue.SubCategoryId;

			try
			{
				dsSelectedIssue = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"sp_GetAllIssueforsuperAdmin",objParam);
				return dsSelectedIssue;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "GetSuperAdminIssue", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet MoveIssue(clsViewMyIssues objViewIssue)
		{
			DataSet dsSelectedIssue;
			dsSelectedIssue = new DataSet();

			SqlParameter[] objParam = new SqlParameter[2];
			
			objParam[0] = new SqlParameter("@EmployeeID",SqlDbType.Int);
			objParam[0].Value = objViewIssue.EmployeeID;
			objParam[1] = new SqlParameter("@IssueAssignmentId",SqlDbType.VarChar);
			objParam[1].Value = objViewIssue.ReportIssueIDStr;

			try
			{
				dsSelectedIssue = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"sp_MoveIssue",objParam);
				return dsSelectedIssue;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "MoveIssue", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetSuperAdminEmployees(clsViewMyIssues objViewIssue)
		{
			DataSet dsSelectedIssue;
			dsSelectedIssue = new DataSet();

			SqlParameter[] objParam = new SqlParameter[1];
			
			objParam[0] = new SqlParameter("@EmployeeID",SqlDbType.Int);
			objParam[0].Value = objViewIssue.EmployeeID;
			try
			{
				dsSelectedIssue = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,".sp_Admin_Employees",objParam);
				return dsSelectedIssue;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "GetSuperAdminEmployees", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet BindCategory(clsViewMyIssues objViewIssue)
		{
			DataSet dsSelectedIssue = new DataSet();

			SqlParameter[] objParam = new SqlParameter[1];
			objParam[0] = new SqlParameter("@EmployeeID",SqlDbType.Int);
			objParam[0].Value = objViewIssue.EmployeeID;
			try
			{
				dsSelectedIssue = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"sp_getCategory",objParam);
				return dsSelectedIssue;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "BindCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		
		public DataSet bindCategories(clsViewMyIssues objViewIssue)
		{
			DataSet dsSelectedIssue = new DataSet();

			SqlParameter[] objParam = new SqlParameter[1];
			objParam[0] = new SqlParameter("@EmployeeID",SqlDbType.Int);
			objParam[0].Value = objViewIssue.EmployeeID;
			try
			{
				dsSelectedIssue = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"sp_BindCategory",objParam);
				return dsSelectedIssue;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "bindCategories", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet LoadDepartment(clsViewMyIssues objViewIssue)
		{
			DataSet dsSelectedIssue;
			dsSelectedIssue = new DataSet();

			SqlParameter[] objParam = new SqlParameter[1];
			
			objParam[0] = new SqlParameter("@EmployeeID",SqlDbType.Int);
			objParam[0].Value = objViewIssue.EmployeeID;
			try
			{
				dsSelectedIssue = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"getDepartment",objParam);
				return dsSelectedIssue;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "LoadDepartment", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet GetSelectedIssueforSuperAdmin(clsViewMyIssues objViewIssue,int userid)
		{
			DataSet dsSelectedIssue;
			dsSelectedIssue = new DataSet();

			SqlParameter[] objParam = new SqlParameter[2];
			
//			objParam[0] = new SqlParameter("@ReportIssueID",SqlDbType.Int);
//			objParam[0].Value = objViewIssue.ReportIssueID;

			objParam[0] = new SqlParameter("@IssueAssignmentID", SqlDbType.Int);
			objParam[0].Value = objViewIssue.IssueAssignmentID;

            objParam[1] = new SqlParameter("@UserID", SqlDbType.Int);
            objParam[1].Value = userid;

			try
			{
				dsSelectedIssue = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"sp_GetSelectedIssueForSuperAdmin",objParam);
				return dsSelectedIssue;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "GetSelectedIssueforSuperAdmin", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		public DataSet bindCategory(clsViewMyIssues objViewIssue)
		{
			DataSet dsSelectedIssue;
			dsSelectedIssue = new DataSet();

			SqlParameter[] objParam = new SqlParameter[1];

			objParam[0] = new SqlParameter("@CategoryId", SqlDbType.Int);
			objParam[0].Value = objViewIssue.SubCategoryId;

			try
			{
				dsSelectedIssue = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"sp_geSubCategory",objParam);
				return dsSelectedIssue;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "bindCategory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public DataSet GetSuperAdminReportIssueHistory(clsViewMyIssues objViewIssue)
		{
			DataSet dsSuperAdminReportIssueHistory;
			dsSuperAdminReportIssueHistory = new DataSet();

			SqlParameter[] objParam = new SqlParameter[1];
			objParam[0] = new SqlParameter("@ReportIssueID",SqlDbType.Int);
			objParam[0].Value = objViewIssue.ReportIssueID;

			try
			{
				dsSuperAdminReportIssueHistory = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"sp_GetSuperAdminReportIssueHistory",objParam);
				return dsSuperAdminReportIssueHistory;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyIssues.cs", "GetSuperAdminReportIssueHistory", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

	}
}
