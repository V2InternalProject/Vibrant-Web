using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration ;
using Microsoft.ApplicationBlocks.Data ;
using V2.Helpdesk.Model;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Helpdesk.DataLayer
{
	/// <summary>
	/// Summary description for clsDlIssueStatus.
	/// </summary>
	public class clsDlIssueStatus
	{
		string connectionString = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();

		public clsDlIssueStatus()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDlIssueStatus.cs", "clsDlIssueStatus", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet getDsIssueStatus(clsIssueStatus objclsIssueStatus)
		
        {
			DataSet dsIssueStatus;

			SqlParameter []objparam = new SqlParameter [3];

			objparam [0] = new SqlParameter ("@Category", System.Data.SqlDbType.Int );
			objparam [0].Value = objclsIssueStatus.Category ;

			objparam [1] = new SqlParameter ("@startMonth", System.Data.SqlDbType.DateTime );
			objparam [1].Value = objclsIssueStatus.StartMonth ;

			objparam [2] = new SqlParameter ("@endMonth",System.Data.SqlDbType.DateTime );
			objparam [2].Value = objclsIssueStatus.EndMonth ;
			
			try
			{
				dsIssueStatus = SqlHelper.ExecuteDataset (connectionString ,CommandType.StoredProcedure ,"sp_GetIssueStatusforDepartment",objparam );
				return dsIssueStatus;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDlIssueStatus.cs", "getDsIssueStatus", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

        public DataSet getYears()
        {
            DataSet dsYears;           

            try
            {
                dsYears = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "sp_GetYears");
                return dsYears;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDlIssueStatus.cs", "getDsIssueStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
		
		public DataSet BindData(clsIssueStatus objclsIssueStatus)
		{
			DataSet dsIssueStatus = new DataSet();

			SqlParameter []objparam = new SqlParameter [1];

			objparam [0] = new SqlParameter ("@EmployeeID", System.Data.SqlDbType.Int );
			objparam [0].Value = objclsIssueStatus.EmployeeId ;

			try
			{
				dsIssueStatus = SqlHelper.ExecuteDataset (connectionString ,CommandType.StoredProcedure ,"getDepartment",objparam );
				return dsIssueStatus;
			}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDlIssueStatus.cs", "BindData", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		
		
	}
}
