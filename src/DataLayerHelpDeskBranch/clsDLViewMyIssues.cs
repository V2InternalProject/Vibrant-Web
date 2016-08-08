using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using V2.Helpdesk.Model;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Helpdesk.DataLayer
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
				throw new V2Exceptions(ex.ToString(),ex);
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
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }


        


		public DataSet GetMyIssueList(Model.clsViewMyIssues objViewMyIssues)
		{
			
			DataSet dsIssueList;
			dsIssueList = new DataSet();

            //SqlParameter[] objParam = new SqlParameter[2];
            //objParam[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
            //objParam[0].Value = objViewMyIssues.EmployeeID;
            //objParam[1] = new SqlParameter("@StatusID", SqlDbType.NVarChar);
            //objParam[1].Value = objViewMyIssues.SelectedStatus;
			try
			{
                //dsIssueList = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"sp_GetIssueListNew3",objParam);
                //return dsIssueList;
                using (SqlConnection con = new SqlConnection(sqlConn))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_GetIssueListNew3";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 5000;
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("@EmployeeID", objViewMyIssues.EmployeeID);
                    cmd.Parameters.AddWithValue("@StatusID", objViewMyIssues.SelectedStatus);


                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dsIssueList);
                }
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
				throw new V2Exceptions(ex.ToString(),ex);
			}

		}

		public DataSet GetSelectedIssue(Model.clsViewMyIssues objViewIssue,int userid)
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
				throw new V2Exceptions(ex.ToString(),ex);
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
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }



		public DataSet GetReportIssueHistory(Model.clsViewMyIssues objViewIssue)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet GetSuperAdminIssueList(Model.clsViewMyIssues objViewIssue)
		{
			DataSet dsSelectedIssue;
			dsSelectedIssue = new DataSet();

            //SqlParameter[] objParam = new SqlParameter[4];
			
            //objParam[0] = new SqlParameter("@EmployeeID",SqlDbType.Int);
            //objParam[0].Value = objViewIssue.EmployeeID;
            //objParam[1] = new SqlParameter("@Status",SqlDbType.NVarChar);
            //objParam[1].Value = objViewIssue.SelectedStatus;
            //objParam[2] = new SqlParameter("@Name",SqlDbType.VarChar);
            //objParam[2].Value = objViewIssue.SelectedEmployee;
            //objParam[3] = new SqlParameter("@Category",SqlDbType.VarChar);
            //objParam[3].Value = objViewIssue.Category;

            SqlConnection con = new SqlConnection(sqlConn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GetSuperAdminIssueListNew1";
            cmd.CommandTimeout = 6000;

            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters["@EmployeeID"].Value = objViewIssue.EmployeeID;

            cmd.Parameters.Add("@Status", SqlDbType.NVarChar);
            cmd.Parameters["@Status"].Value = objViewIssue.SelectedStatus;

            cmd.Parameters.Add("@Name", SqlDbType.VarChar);
            cmd.Parameters["@Name"].Value = objViewIssue.SelectedEmployee;

            cmd.Parameters.Add("@Category", SqlDbType.VarChar);
            cmd.Parameters["@Category"].Value = objViewIssue.Category;

			try
			{
                //dsSelectedIssue = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure,"sp_GetSuperAdminIssueListNew1",objParam);
                cmd.Connection = con;

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dsSelectedIssue);
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet GetSuperAdminIssue(Model.clsViewMyIssues objViewIssue)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet MoveIssue(Model.clsViewMyIssues objViewIssue)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet GetSuperAdminEmployees(Model.clsViewMyIssues objViewIssue)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet BindCategory(Model.clsViewMyIssues objViewIssue)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		
		public DataSet bindCategories(Model.clsViewMyIssues objViewIssue)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet LoadDepartment(Model.clsViewMyIssues objViewIssue)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet GetSelectedIssueforSuperAdmin(Model.clsViewMyIssues objViewIssue,int userid)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet bindCategory(Model.clsViewMyIssues objViewIssue)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet GetSuperAdminReportIssueHistory(Model.clsViewMyIssues objViewIssue)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


        public DataSet SearchIssueIDData(Model.clsViewMyIssues objViewIssue)
        {
            DataSet dsSelectedIssue;
            dsSelectedIssue = new DataSet();

            SqlParameter[] objParam = new SqlParameter[1];

            objParam[0] = new SqlParameter("@ReportIssueID", SqlDbType.Int);
            objParam[0].Value = objViewIssue.ReportIssueID;
            try
            {
                dsSelectedIssue = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_IssueIDSearch", objParam);
                return dsSelectedIssue;
            }
            catch (V2Exceptions ex)
            {
                throw new V2Exceptions(ex.ToString(),ex);
            }


        }
    }
}
