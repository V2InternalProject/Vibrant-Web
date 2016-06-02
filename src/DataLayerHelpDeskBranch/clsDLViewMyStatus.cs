using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using ModelHelpDeskBranch;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace DataLayerHelpDeskBranch
{
	/// <summary>
	/// Summary description for clsDLViewMyStatus.
	/// </summary>
	public class clsDLViewMyStatus
	{
		string strConnectionString  = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
        //public clsDLViewMyStatus()
        //{
        //    try
        //    {
        //    }
        //    catch(V2Exceptions ex)
        //    {
        //        throw ;
        //    }

        //    catch(System.Exception ex)
        //    {
                
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyStatus.cs", "clsDLViewMyStatus", ex.StackTrace);
        //        throw new V2Exceptions(ex.ToString(),ex);
        //    }
        //}
		
		public DataSet GetIssues(clsViewMyStatus objClsViewMyStatus)
		{
			
    		DataSet dsGetIssues = new DataSet();
//			SqlParameter[] sqlParams = new SqlParameter[1];
//			sqlParams[0] = new SqlParameter("@issuesId", SqlDbType.VarChar, 100);
//			sqlParams[0].Value = objClsViewMyStatus.IssueID;
//			sqlParams[0].Direction = ParameterDirection.Input;

			SqlParameter[] sqlParams = new SqlParameter[1];
            //sqlParams[0] = new SqlParameter("@Password", SqlDbType.VarChar, 100);
            //sqlParams[0].Value = objClsViewMyStatus.Password;
            //sqlParams[0].Direction = ParameterDirection.Input;

			sqlParams[0] = new SqlParameter("@LoginID", SqlDbType.Int, 10);
			sqlParams[0].Value = objClsViewMyStatus.LoginID;
			sqlParams[0].Direction = ParameterDirection.Input;
	  	//sp_GetIssues
			try
			{
				dsGetIssues = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetIssues", sqlParams);
				return dsGetIssues;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyStatus.cs", "GetIssues", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet GetIssuesReport(clsViewMyStatus objClsViewMyStatus)
		{
			DataSet dsGetIssues = new DataSet();
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0]=new SqlParameter("@issuesId",SqlDbType.Int,10);
			sqlParams[0].Value=objClsViewMyStatus.IssueID;
			try
			{
				dsGetIssues = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetIssuesReport", sqlParams);
				return dsGetIssues;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyStatus.cs", "GetIssuesReport", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet GetIssueDetails(clsViewMyStatus objClsViewMyStatus)
		{
			try
			{
			DataSet dsGetIssueDetails = new DataSet();
			SqlParameter[] sqlParams = new SqlParameter[1];

			/*sqlParams[0] = new SqlParameter("@EmailID", SqlDbType.VarChar, 100);
			sqlParams[0].Value = objClsViewMyStatus.EmailID;
			sqlParams[0].Direction = ParameterDirection.Input;*/

			sqlParams[0] = new SqlParameter("@IssueID", SqlDbType.Int);
			sqlParams[0].Value = objClsViewMyStatus.IssueID;
			sqlParams[0].Direction = ParameterDirection.Input;

			dsGetIssueDetails = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetIssueDetails", sqlParams);
			return dsGetIssueDetails;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyStatus.cs", "GetIssueDetails", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		/*public DataSet GetStatusList()
		{
			DataSet dsGetStatusList = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetStatusList");
			return dsGetStatusList;
		}*/
		
		public int updateIssue(clsViewMyStatus objClsViewMyStatus)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[4];
			sqlParams[0] = new SqlParameter("@Comments", SqlDbType.VarChar, 1000);
			sqlParams[0].Value = objClsViewMyStatus.Comments;
			sqlParams[0].Direction = ParameterDirection.Input;

			/*sqlParams[1] = new SqlParameter("@statusID", SqlDbType.Int);
			sqlParams[1].Value = objClsViewMyStatus.StatusID;
			sqlParams[1].Direction = ParameterDirection.Input;*/

			sqlParams[1] = new SqlParameter("@SubCategoryID", SqlDbType.Int);
			sqlParams[1].Value = objClsViewMyStatus.SubCategoryID;
			sqlParams[1].Direction = ParameterDirection.Input;

			sqlParams[2] = new SqlParameter("@reportIssueID", SqlDbType.Int);
			sqlParams[2].Value = objClsViewMyStatus.IssueID;
			sqlParams[2].Direction = ParameterDirection.Input;

			sqlParams[3] = new SqlParameter("@StatusID", SqlDbType.Int, 10);
			sqlParams[3].Value = objClsViewMyStatus.StatusID;
			sqlParams[3].Direction = ParameterDirection.Input;


            //sqlParams[4] = new SqlParameter("@UserName", SqlDbType.VarChar, 200);
            //sqlParams[4].Value = name;
            //sqlParams[4].Direction = ParameterDirection.Input;


			int noOfrecordsAffected = 0;
			//[sp_UpdateIssueStatus]
            noOfrecordsAffected = SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_UpdateIssueStatus", sqlParams);
			//noOfrecordsAffected = SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_UpdateIssue", sqlParams);
			return noOfrecordsAffected;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyStatus.cs", "updateIssue", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

        public DataSet GetEmployeeName(string name1)
        {
            try
            {
                DataSet dsGetEmpName = new DataSet();
                SqlParameter[] sqlParams = new SqlParameter[1];

                /*sqlParams[0] = new SqlParameter("@EmailID", SqlDbType.VarChar, 100);
                sqlParams[0].Value = objClsViewMyStatus.EmailID;
                sqlParams[0].Direction = ParameterDirection.Input;*/

                sqlParams[0] = new SqlParameter("@EmpCode", SqlDbType.NVarChar,50);
                sqlParams[0].Value = name1;
                sqlParams[0].Direction = ParameterDirection.Input;

                dsGetEmpName = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "GetEmployeeName", sqlParams);
                return dsGetEmpName;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyStatus.cs", "GetIssueDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }


        public int updateIssue1(clsViewMyStatus objClsViewMyStatus, int counter,string userloginid)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[6];
                sqlParams[0] = new SqlParameter("@Comments", SqlDbType.VarChar, 1000);
                sqlParams[0].Value = objClsViewMyStatus.Comments;
                sqlParams[0].Direction = ParameterDirection.Input;

                /*sqlParams[1] = new SqlParameter("@statusID", SqlDbType.Int);
                sqlParams[1].Value = objClsViewMyStatus.StatusID;
                sqlParams[1].Direction = ParameterDirection.Input;*/

                sqlParams[1] = new SqlParameter("@SubCategoryID", SqlDbType.Int);
                sqlParams[1].Value = objClsViewMyStatus.SubCategoryID;
                sqlParams[1].Direction = ParameterDirection.Input;

                sqlParams[2] = new SqlParameter("@reportIssueID", SqlDbType.Int);
                sqlParams[2].Value = objClsViewMyStatus.IssueID;
                sqlParams[2].Direction = ParameterDirection.Input;

                sqlParams[3] = new SqlParameter("@StatusID", SqlDbType.Int);
                sqlParams[3].Value = objClsViewMyStatus.StatusID;
                sqlParams[3].Direction = ParameterDirection.Input;

                sqlParams[4] = new SqlParameter("@counter", SqlDbType.Int);
                sqlParams[4].Value = counter;
                sqlParams[4].Direction = ParameterDirection.Input;


                //sqlParams[4] = new SqlParameter("@UserName", SqlDbType.VarChar, 200);
                //sqlParams[4].Value = name;
                //sqlParams[4].Direction = ParameterDirection.Input;

                sqlParams[5] = new SqlParameter("@EmployeeID", SqlDbType.VarChar, 100);
                sqlParams[5].Value = userloginid;
                sqlParams[5].Direction = ParameterDirection.Input;


                int noOfrecordsAffected = 0;
                //[sp_UpdateIssueStatus]
                noOfrecordsAffected = SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_UpdateIssueStatus_New", sqlParams);
                //noOfrecordsAffected = SqlHelper.ExecuteNonQuery(strConnectionString, CommandType.StoredProcedure, "sp_UpdateIssue", sqlParams);
                return noOfrecordsAffected;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyStatus.cs", "updateIssue", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

		public string getEmailID(clsViewMyStatus objClsViewMyStatus)
		{
			try
			{
			SqlParameter[] sqlParams = new SqlParameter[1];
			sqlParams[0] = new SqlParameter("@IssueID", SqlDbType.Int, 4);
			sqlParams[0].Value = objClsViewMyStatus.IssueID;
			sqlParams[0].Direction = ParameterDirection.Input;

			string strEmailID = (SqlHelper.ExecuteScalar(strConnectionString, CommandType.StoredProcedure, "sp_GetEmailID",sqlParams)).ToString();
			return strEmailID;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLViewMyStatus.cs", "getEmailID", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

	}
}
