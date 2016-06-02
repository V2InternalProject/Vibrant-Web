using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ModelHelpdesk;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace DataLayerHelpDesk
{
	/// <summary>
	/// Summary description for clsDLIssueAssignment.
	/// </summary>
	public class clsDLIssueAssignment
	{
		string sqlConn = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();

		public clsDLIssueAssignment()
		{
		}


        public bool UpdateIssueByLoginUser(clsIssueAssignment objIssueAssignment,string name1)
		{
			SqlParameter[] objParam = new SqlParameter[7];

			objParam[0]= new SqlParameter("@StatusID", SqlDbType.Int);
			objParam[0].Value = objIssueAssignment.StatusID;

			objParam[1] = new SqlParameter("@IssueAssignmentID", SqlDbType.Int);
			objParam[1].Value = objIssueAssignment.IssueAssignmentID;

			objParam[2] = new SqlParameter("@ReportIssueID", SqlDbType.Int);
			objParam[2].Value = objIssueAssignment.ReportIssueID;

			objParam[3] = new SqlParameter("@Cause", SqlDbType.VarChar, 255);
			objParam[3].Value = objIssueAssignment.Cause;

			objParam[4] = new SqlParameter("@Fix", SqlDbType.VarChar, 255);
			objParam[4].Value = objIssueAssignment.Fix;
			
			objParam[5] = new SqlParameter ("@Comments",SqlDbType.NVarChar,2000);
			objParam[5].Value = objIssueAssignment.AddComment;

            objParam[6] = new SqlParameter("@EmployeeID", SqlDbType.Int);
            objParam[6].Value = name1;

			int recaffected1;
			try
			{
				recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, "sp_UpdateIssueByLoginUser_New11", objParam);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLIssueAssignment.cs", "UpdateIssueByLoginUser", ex.StackTrace);
				throw new V2Exceptions();
			}
		
			return true;
		}


		public DataSet FileName(clsIssueAssignment objIssueAssignment)
		{
			try
			{
				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@ReportIssueID", SqlDbType.Int, 4);
				sqlParams[0].Value = objIssueAssignment.ReportIssueID;
                //sqlParams[0].Direction = ParameterDirection.Input;

				DataSet dsFromEmailID = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_FileName",sqlParams);
                return dsFromEmailID;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLIssueAssignment.cs", "FileName", ex.StackTrace);
				throw new V2Exceptions();
			}
		}
		

		public bool IssueUpdateBySuperAdmin(clsIssueAssignment objIssueAssignment)
		{
			SqlParameter[] objParam = new SqlParameter[8];

			objParam[0]= new SqlParameter("@StatusID", SqlDbType.Int);
			objParam[0].Value = objIssueAssignment.StatusID;

			objParam[1] = new SqlParameter("@IssueAssignmentID", SqlDbType.Int);
			objParam[1].Value = objIssueAssignment.IssueAssignmentID;

			objParam[2] = new SqlParameter("@ReportIssueID", SqlDbType.Int);
			objParam[2].Value = objIssueAssignment.ReportIssueID;

			objParam[3] = new SqlParameter("@Cause", SqlDbType.VarChar, 255);
			objParam[3].Value = objIssueAssignment.Cause;

			objParam[4] = new SqlParameter("@Fix", SqlDbType.VarChar, 255);
			objParam[4].Value = objIssueAssignment.Fix;

			objParam[5] = new SqlParameter("@EmployeeID", SqlDbType.Int);
			objParam[5].Value = objIssueAssignment.EmployeeID;
			
			objParam[6] = new SqlParameter("@SubCategory", SqlDbType.Int);
			objParam[6].Value = objIssueAssignment.SubCategory;

            objParam[7] = new SqlParameter("@TypeID", SqlDbType.Int);
            objParam[7].Value = objIssueAssignment.TypeID;

			int recaffected1;
			try
			{

                //recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, "sp_IssueUpdateBySuperAdmin", objParam);
                recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, "sp_IssueAssignmentBySuperAdmin", objParam);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLIssueAssignment.cs", "IssueUpdateBySuperAdmin", ex.StackTrace);
				throw new V2Exceptions();
			}
		
			return true;
		}


		public bool IssueAssignmentBySuperAdmin(clsIssueAssignment objIssueAssignment)
		{
			SqlParameter[] objParam = new SqlParameter[8];

			objParam[0]= new SqlParameter("@StatusID", SqlDbType.Int);
			objParam[0].Value = objIssueAssignment.StatusID;

			objParam[1] = new SqlParameter("@IssueAssignmentID", SqlDbType.Int);
			objParam[1].Value = objIssueAssignment.IssueAssignmentID;

			objParam[2] = new SqlParameter("@ReportIssueID", SqlDbType.Int);
			objParam[2].Value = objIssueAssignment.ReportIssueID;

			objParam[3] = new SqlParameter("@Cause", SqlDbType.VarChar, 255);
			objParam[3].Value = objIssueAssignment.Cause;

			objParam[4] = new SqlParameter("@Fix", SqlDbType.VarChar, 255);
			objParam[4].Value = objIssueAssignment.Fix;

			objParam[5] = new SqlParameter("@EmployeeID", SqlDbType.Int);
			objParam[5].Value = objIssueAssignment.EmployeeID;

			objParam[6] = new SqlParameter("@SubCategory", SqlDbType.Int);
			objParam[6].Value = objIssueAssignment.SubCategory;

            objParam[7] = new SqlParameter("@TypeID", SqlDbType.Int);
            objParam[7].Value = objIssueAssignment.TypeID;
            

			int recaffected1;
			try
			{
				recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, "sp_IssueAssignmentBySuperAdmin", objParam);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLIssueAssignment.cs", "IssueAssignmentBySuperAdmin", ex.StackTrace);
				throw new V2Exceptions();
			}
		
			return true;
		}
        public DataSet ChangeCategoryOfIssue(clsIssueAssignment objIssueAssignment)
        {
            SqlParameter[] objParam = new SqlParameter[3];

            objParam[0] = new SqlParameter("@ReportIssueID", SqlDbType.Int);
            objParam[0].Value = objIssueAssignment.ReportIssueID;

            objParam[1] = new SqlParameter("@SubCategoryID", SqlDbType.Int, 4);
            objParam[1].Value = objIssueAssignment.SubCategoryID;

            objParam[2] = new SqlParameter("@StatusID", SqlDbType.Int, 4);
            objParam[2].Value = objIssueAssignment.StatusID;
     
            try
            {
              DataSet dsCategory=  SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "ChangeCategoryOfIssue", objParam);
              return dsCategory;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLIssueAssignment.cs", "ChangeCategoryOfIssue", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

      
		public string getFromEmailID(clsIssueAssignment objIssueAssignment)
		{
			try
			{
				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@IssueAssignmentID", SqlDbType.Int, 4);
				sqlParams[0].Value = objIssueAssignment.IssueAssignmentID;
				sqlParams[0].Direction = ParameterDirection.Input;

				string strFromEmailID = (SqlHelper.ExecuteScalar(sqlConn, CommandType.StoredProcedure, "sp_GetFromEmailID",sqlParams)).ToString();
				return strFromEmailID;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLIssueAssignment.cs", "getFromEmailID", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public string getToEmailID(clsIssueAssignment objIssueAssignment)
		{
			try
			{
				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@IssueID", SqlDbType.Int, 4);
				sqlParams[0].Value = objIssueAssignment.ReportIssueID;
				sqlParams[0].Direction = ParameterDirection.Input;

				string strFromEmailID = (SqlHelper.ExecuteScalar(sqlConn, CommandType.StoredProcedure, "sp_GetEmailID",sqlParams)).ToString();
				return strFromEmailID;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLIssueAssignment.cs", "getToEmailID", ex.StackTrace);
				throw new V2Exceptions();
			}
		}

		public string getEmployeeEmailID(clsIssueAssignment objIssueAssignment)
		{
			try
			{
				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 4);
				sqlParams[0].Value = objIssueAssignment.EmployeeID;
				sqlParams[0].Direction = ParameterDirection.Input;

				string strEmployeeEmailID = (SqlHelper.ExecuteScalar(sqlConn, CommandType.StoredProcedure, "sp_getEmployeeEmailID",sqlParams)).ToString();
				return strEmployeeEmailID;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLIssueAssignment.cs", "getEmployeeEmailID", ex.StackTrace);
				throw new V2Exceptions();
			}
		}


        public string GetIssueRaiserEmailID(int intReportIssueID)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@ReportIssueId", SqlDbType.Int, 4);
                sqlParams[0].Value = intReportIssueID;
                sqlParams[0].Direction = ParameterDirection.Input;

                string strEmployeeEmailID = (SqlHelper.ExecuteScalar(sqlConn, CommandType.StoredProcedure, "sp_GetIssueRaiserEmailID", sqlParams)).ToString();
                return strEmployeeEmailID;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLIssueAssignment.cs", "GetIssueRaiserEmailID", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

	}
}
