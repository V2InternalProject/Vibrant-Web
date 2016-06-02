using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using V2.Helpdesk.Model;
using System.Configuration;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.DataLayer
{
	/// <summary>
	/// Summary description for clsDLProblemSeverity.
	/// </summary>
	public class clsDLProblemSeverity
	{
		string sqlConn = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();

		public clsDLProblemSeverity()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLProblemSeverity.cs", "clsDLProblemSeverity", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet GetProblemSeverityList()
		{
			DataSet dsProblemSeverityList;
			dsProblemSeverityList = new DataSet();
			try
			{
				dsProblemSeverityList = SqlHelper.ExecuteDataset(sqlConn,CommandType.StoredProcedure,"sp_GetProblemSeverityList");
				return dsProblemSeverityList;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLProblemSeverity.cs", "GetProblemSeverityList", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public bool AddNewProblemSeverity(Model.clsProblemSeverity objProblemSeverity)
		{
			DataSet dsProblemSeverityList;
			dsProblemSeverityList = new DataSet();

			SqlParameter[] objParam = new SqlParameter[2];

			objParam[0] = new SqlParameter("@ProblemSeverity",SqlDbType.VarChar,100);
			objParam[0].Value = objProblemSeverity.ProblemSeverityName;

			objParam[1] = new SqlParameter("@isActive",SqlDbType.Bit,1);
			objParam[1].Value = objProblemSeverity.isActive;

			int recaffected1;
			try
			{
				recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn,CommandType.StoredProcedure,"sp_AddNewProblemSeverity",objParam);
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLProblemSeverity.cs", "AddNewProblemSeverity", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		
			return true;
		}

		public int UpdateProblemSeverity(Model.clsProblemSeverity objProblemSeverity)
		{
			SqlParameter[] objParam = new SqlParameter[3];

			objParam[0] = new SqlParameter("@ProblemSeverityID",SqlDbType.Int);
			objParam[0].Value = objProblemSeverity.ProblemSeverityID;

			objParam[1] = new SqlParameter("@ProblemSeverity",SqlDbType.VarChar,100);
			objParam[1].Value = objProblemSeverity.ProblemSeverityName;

			objParam[2] = new SqlParameter("@isActive",SqlDbType.Bit,1);
			objParam[2].Value = objProblemSeverity.isActive;

			int recaffected1;
			try
			{
				recaffected1 = Convert.ToInt32(SqlHelper.ExecuteScalar(sqlConn,CommandType.StoredProcedure,"sp_UpdateProblemSeverity",objParam));
				return recaffected1;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLProblemSeverity.cs", "UpdateProblemSeverity", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public int DeleteProblemSeverity(Model.clsProblemSeverity objProblemSeverity)
		{
			SqlParameter[] objParam = new SqlParameter[1];

			objParam[0] = new SqlParameter("@ProblemSeverityID",SqlDbType.Int);
			objParam[0].Value = objProblemSeverity.ProblemSeverityID;

			int recaffected1;
			try
			{
				recaffected1 = SqlHelper.ExecuteNonQuery(sqlConn,CommandType.StoredProcedure,"sp_DeleteProblemSeverity",objParam);
				return recaffected1;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLProblemSeverity.cs", "DeleteProblemSeverity", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
		public DataSet IsDuplicateProblemSeverity(Model.clsProblemSeverity objProblemSeverity)
		{
			DataSet dsDuplicateProblemSeverity;

			SqlParameter[] objParam = new SqlParameter[2];

			objParam[0] = new SqlParameter("@ProblemSeverityID",SqlDbType.Int);
			objParam[0].Value = objProblemSeverity.ProblemSeverityID;

			objParam[1] = new SqlParameter("@ProblemSeverity",SqlDbType.VarChar,100);
			objParam[1].Value = objProblemSeverity.ProblemSeverityName;
			
			try
			{
				dsDuplicateProblemSeverity = SqlHelper.ExecuteDataset(sqlConn,CommandType.StoredProcedure,"sp_IsDuplicateProblemSeverity",objParam);
				return dsDuplicateProblemSeverity;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLProblemSeverity.cs", "IsDuplicateProblemSeverity", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

	}
}
