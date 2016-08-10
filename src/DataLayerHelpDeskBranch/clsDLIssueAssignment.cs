using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using V2.Helpdesk.Model;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.DataLayer
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


        public bool UpdateIssueByLoginUser(Model.clsIssueAssignment objIssueAssignment,string name1)
		{
			SqlParameter[] objParam = new SqlParameter[11];

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

            objParam[7] = new SqlParameter("@WorkHours", SqlDbType.Int, 8);
            objParam[7].Value = objIssueAssignment.WorkHours;
            objParam[7].Direction = ParameterDirection.Input;

            objParam[8] = new SqlParameter("@FromDate", SqlDbType.DateTime);
            if (objIssueAssignment.FromDate.ToString() == "")
                objParam[8].Value = null;
            else
                objParam[8].Value = objIssueAssignment.FromDate;
            objParam[8].Direction = ParameterDirection.Input;

            objParam[9] = new SqlParameter("@ToDate", SqlDbType.DateTime);
            if (objIssueAssignment.ToDate.ToString() == "")
                objParam[9].Value = null;
            else
                objParam[9].Value = objIssueAssignment.ToDate;
            objParam[9].Direction = ParameterDirection.Input;

            objParam[10] = new SqlParameter("@NumberOfResources", SqlDbType.Int, 8);
            objParam[10].Value = objIssueAssignment.NumberOfResources;
            objParam[10].Direction = ParameterDirection.Input;

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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		
			return true;
		}


		public DataSet FileName(Model.clsIssueAssignment objIssueAssignment)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


        public bool IssueUpdateBySuperAdmin(Model.clsIssueAssignment objIssueAssignment)
        {
            SqlParameter[] objParam = new SqlParameter[14];

            objParam[0] = new SqlParameter("@StatusID", SqlDbType.Int);
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

            //Modified by Mahesh F For Issue ID:22449
            objParam[8] = new SqlParameter("@ProblemSeverity", SqlDbType.Int);
            objParam[8].Value = objIssueAssignment.ProblemSeverity;


            objParam[9] = new SqlParameter("@WorkHours", SqlDbType.Int, 8);
            objParam[9].Value = objIssueAssignment.WorkHours;
            objParam[9].Direction = ParameterDirection.Input;


            objParam[10] = new SqlParameter("@FromDate", SqlDbType.DateTime);
            if (objIssueAssignment.FromDate.ToString() == "")
                objParam[10].Value = null;
            else
                objParam[10].Value = objIssueAssignment.FromDate;
            objParam[10].Direction = ParameterDirection.Input;

            objParam[11] = new SqlParameter("@ToDate", SqlDbType.DateTime);
            if (objIssueAssignment.ToDate.ToString() == "")
                objParam[11].Value = null;
            else
                objParam[11].Value = objIssueAssignment.ToDate;
            objParam[11].Direction = ParameterDirection.Input;

            objParam[12] = new SqlParameter("@IssueReportDateTime", SqlDbType.DateTime);
            if (objIssueAssignment.IssueReportDateTime.ToString() == "")
                objParam[12].Value = null;
            else
                objParam[12].Value = objIssueAssignment.IssueReportDateTime;
            objParam[12].Direction = ParameterDirection.Input;


            objParam[13] = new SqlParameter("@NumberOfResources", SqlDbType.Int, 8);
            objParam[13].Value = objIssueAssignment.NumberOfResources;
            objParam[13].Direction = ParameterDirection.Input;

            int recaffected1;
            try
            {

                //recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, "sp_IssueUpdateBySuperAdmin", objParam);
                recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, "sp_IssueAssignmentBySuperAdmin", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLIssueAssignment.cs", "IssueUpdateBySuperAdmin", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }

            return true;
        }


		public bool IssueAssignmentBySuperAdmin(Model.clsIssueAssignment objIssueAssignment)
		{
			SqlParameter[] objParam = new SqlParameter[14];

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
            //Modified by Mahesh F For Issue ID:22449
            objParam[8] = new SqlParameter("@ProblemSeverity", SqlDbType.Int);
            objParam[8].Value = objIssueAssignment.ProblemSeverity;

            objParam[9] = new SqlParameter("@WorkHours", SqlDbType.Int, 8);
            objParam[9].Value = objIssueAssignment.WorkHours;
            objParam[9].Direction = ParameterDirection.Input;

            objParam[10] = new SqlParameter("@FromDate", SqlDbType.DateTime);
            if (objIssueAssignment.FromDate.ToString() == "")
                objParam[10].Value = null;
            else
                objParam[10].Value = objIssueAssignment.FromDate;
            objParam[10].Direction = ParameterDirection.Input;

            objParam[11] = new SqlParameter("@ToDate", SqlDbType.DateTime);
            if (objIssueAssignment.ToDate.ToString() == "")
                objParam[11].Value = null;
            else
                objParam[11].Value = objIssueAssignment.ToDate;
            objParam[11].Direction = ParameterDirection.Input;

            objParam[12] = new SqlParameter("@IssueReportDateTime", SqlDbType.DateTime);
            if (objIssueAssignment.IssueReportDateTime.ToString() == "")
                objParam[12].Value = null;
            else
                objParam[12].Value = objIssueAssignment.IssueReportDateTime;
            objParam[12].Direction = ParameterDirection.Input;

            objParam[13] = new SqlParameter("@NumberOfResources", SqlDbType.Int, 8);
            objParam[13].Value = objIssueAssignment.NumberOfResources;
            objParam[13].Direction = ParameterDirection.Input;

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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		
			return true;
		}
        public DataSet ChangeCategoryOfIssue(Model.clsIssueAssignment objIssueAssignment)
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
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

      
		public string getFromEmailID(Model.clsIssueAssignment objIssueAssignment)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public string getToEmailID(Model.clsIssueAssignment objIssueAssignment)
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
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public string getEmployeeEmailID(Model.clsIssueAssignment objIssueAssignment)
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
				throw new V2Exceptions(ex.ToString(),ex);
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
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public string getUserName(Model.clsIssueAssignment objIssueAssignment)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@IssueAssignmentID", SqlDbType.Int, 4);
                sqlParams[0].Value = objIssueAssignment.IssueAssignmentID;
                sqlParams[0].Direction = ParameterDirection.Input;

                string strgetUserName = (SqlHelper.ExecuteScalar(sqlConn, CommandType.StoredProcedure, "sp_GetUserName", sqlParams)).ToString();
                return strgetUserName;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLIssueAssignment.cs", "getUserName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

	}
}
