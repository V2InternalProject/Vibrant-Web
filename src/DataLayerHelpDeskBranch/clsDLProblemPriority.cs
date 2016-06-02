using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using V2.Helpdesk.Model;
using System.Configuration;
using V2.CommonServices.FileLogger;
using V2.CommonServices.Exceptions;


namespace V2.Helpdesk.DataLayer
{
	/// <summary>
	/// Summary description for clsProblemPriorityDataAccess.
	/// </summary>
	public class clsDLProblemPriority
	{
        string sqlConn = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
           // ConfigurationManager.AppSettings["sql_Helpdesk_connection"].ToString();
         //  ConfigurationManager.ConnectionStrings["sql_Helpdesk_connection"].ConnectionString;
		public clsDLProblemPriority()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLProblemPriority.cs", "clsDLProblemPriority", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet GetProblemPriorityList()
		{
			DataSet dsProblemPriorityList;
			dsProblemPriorityList = new DataSet();
			try
			{
				dsProblemPriorityList = SqlHelper.ExecuteDataset(sqlConn,CommandType.StoredProcedure,"sp_GetProblemPriorityList");
				return dsProblemPriorityList;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLProblemPriority.cs", "GetProblemPriorityList", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


		public bool AddNewProblemPriority(Model.clsProblemPriority objProblemPriority)
		{
			DataSet dsProblemPriorityList;
			dsProblemPriorityList = new DataSet();

			SqlParameter[] objParam = new SqlParameter[4];

			objParam[0] = new SqlParameter("@ProblemPriority",SqlDbType.VarChar,100);
			objParam[0].Value = objProblemPriority.ProblemPriorityName;

			objParam[1] = new SqlParameter("@isActive",SqlDbType.Bit,1);
			objParam[1].Value = objProblemPriority.isActive;

			objParam[2] = new SqlParameter("@GreenResolutionHours",SqlDbType.Int);
			objParam[2].Value = objProblemPriority.GreenResolutionHours;

			objParam[3] = new SqlParameter("@AmberResolutionHours",SqlDbType.Int);
			objParam[3].Value = objProblemPriority.AmberResolutionHours;
			
			int recaffected1;
			try
			{
				recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn,CommandType.StoredProcedure,"sp_AddNewProblemPriority",objParam);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLProblemPriority.cs", "AddNewProblemPriority", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}

			return true;
		}

		public int UpdateProblemPriority(Model.clsProblemPriority objProblemPriority)
		{
			SqlParameter[] objParam = new SqlParameter[5];

			objParam[0] = new SqlParameter("@ProblemPriorityID",SqlDbType.Int);
			objParam[0].Value = objProblemPriority.ProblemPriorityID;

			objParam[1] = new SqlParameter("@ProblemPriority",SqlDbType.VarChar,100);
			objParam[1].Value = objProblemPriority.ProblemPriorityName;

			objParam[2] = new SqlParameter("@isActive",SqlDbType.Bit,1);
			objParam[2].Value = objProblemPriority.isActive;

			objParam[3] = new SqlParameter("@GreenResolutionHours",SqlDbType.Int);
			objParam[3].Value = objProblemPriority.GreenResolutionHours;

			objParam[4] = new SqlParameter("@AmberResolutionHours",SqlDbType.Int);
			objParam[4].Value = objProblemPriority.AmberResolutionHours;
			
			int recaffected1;
			try
			{
				recaffected1 = Convert.ToInt32(SqlHelper.ExecuteScalar(sqlConn,CommandType.StoredProcedure,"sp_UpdateProblemPriority",objParam));
				return recaffected1;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLProblemPriority.cs", "UpdateProblemPriority", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		

		public int DeleteProblemPriority(Model.clsProblemPriority objProblemPriority)
		{
			DataSet dsProblemPriorityList;
			dsProblemPriorityList = new DataSet();

			SqlParameter[] objParam = new SqlParameter[1];

			objParam[0] = new SqlParameter("@ProblemPriorityID",SqlDbType.Int);
			objParam[0].Value = objProblemPriority.ProblemPriorityID;

			int recaffected1;
			try
			{
				recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn,CommandType.StoredProcedure,"sp_DeleteProblemPriority",objParam);
				return recaffected1;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLProblemPriority.cs", "DeleteProblemPriority", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


		public DataSet IsDuplicateProblemPriority(Model.clsProblemPriority objProblemPriority)
		{
			DataSet dsDuplicateProblemPriority;

			SqlParameter[] objParam = new SqlParameter[2];

			objParam[0] = new SqlParameter("@ProblemPriorityID",SqlDbType.Int);
			objParam[0].Value = objProblemPriority.ProblemPriorityID;
			
			objParam[1] = new SqlParameter("@ProblemPriority",SqlDbType.VarChar,100);
			objParam[1].Value = objProblemPriority.ProblemPriorityName;
			try
			{
				dsDuplicateProblemPriority = SqlHelper.ExecuteDataset(sqlConn,CommandType.StoredProcedure,"sp_IsDuplicateProblemPriority",objParam);
				return dsDuplicateProblemPriority;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLProblemPriority.cs", "IsDuplicateProblemPriority", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public int CheckBeforeDeletingProblemPriority(Model.clsProblemPriority objProblemPriority)
		{
			SqlParameter[] objParam = new SqlParameter[1];

			objParam[0] = new SqlParameter("ProblemPriorityID", SqlDbType.Int);
			objParam[0].Value = objProblemPriority.ProblemPriorityID;

			int recaffected1;
			try
			{
				recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn,CommandType.StoredProcedure,"sp_CheckBeforeDeletingProblemPriority",objParam);
				return recaffected1;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLProblemPriority.cs", "CheckBeforeDeletingProblemPriority", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}


	}
}
