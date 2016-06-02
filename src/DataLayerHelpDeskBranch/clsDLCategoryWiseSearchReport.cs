using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using V2.Helpdesk.Model;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Helpdesk.DataLayer
{
	/// <summary>
	/// Summary description for clsDLCategoryWiseSearchReport.
	/// </summary>
	public class clsDLCategoryWiseSearchReport
	{
		string sqlConn = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();

		public clsDLCategoryWiseSearchReport()
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
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLCategoryWiseSearchReport.cs", "clsDLCategoryWiseSearchReport", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

		public DataSet getAllStatus()
		{
			try
			{
			DataSet dsAllStatus = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetAllStatus");
			return dsAllStatus;
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLCategoryWiseSearchReport.cs", "getAllStatus", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}
        /// <summary>
        /// get years from dates of logged issue
        /// </summary>
        /// <returns></returns>
        public DataSet getYears()
        {
            try
            {
                DataSet dsYears = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "sp_GetYears");
                return dsYears;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLMemberWiseSearchReport.cs", "getYears", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

		public DataSet GetCategoryWiseReport(clsCategoryWiseSearchReport objCategoryWiseSearchReport)
		{
			try
			{
			SqlDateTime sqlNullValue;
			sqlNullValue = SqlDateTime.Null;
			DataSet dsCategoryWiseList;
			dsCategoryWiseList = new DataSet();

			SqlParameter[] objParam = new SqlParameter[9];

			//objParam[0] = new SqlParameter("@EmployeeID",SqlDbType.Int);
			//objParam[0].Value = objCategoryWiseSearchReport.EmployeeID;

			objParam[0] = new SqlParameter("@CategoryID",SqlDbType.Int);
			objParam[0].Value = objCategoryWiseSearchReport.CategoryID;

			objParam[1] = new SqlParameter("@StatusID",SqlDbType.Int);
			objParam[1].Value = objCategoryWiseSearchReport.StatusID;

			objParam[2] = new SqlParameter("@Date",SqlDbType.DateTime,20);
			if(objCategoryWiseSearchReport.Date == "")
				objParam[2].Value = sqlNullValue;
			else
				objParam[2].Value = objCategoryWiseSearchReport.Date;

			objParam[3] = new SqlParameter("@Month", SqlDbType.VarChar,20);
			objParam[3].Value = objCategoryWiseSearchReport.Month;

			objParam[4] = new SqlParameter("@Year",SqlDbType.VarChar,20);
			objParam[4].Value = objCategoryWiseSearchReport.Year;

			objParam[5] = new SqlParameter("@FromDate",SqlDbType.DateTime,20);
			if(objCategoryWiseSearchReport.FromDate == "")
				objParam[5].Value = sqlNullValue;
			else
				objParam[5].Value = objCategoryWiseSearchReport.FromDate;

			objParam[6] = new SqlParameter("@ToDate", SqlDbType.DateTime,20);
			if(objCategoryWiseSearchReport.ToDate == "")
				objParam[6].Value = sqlNullValue;
			else
				objParam[6].Value = objCategoryWiseSearchReport.ToDate;

			objParam[7] = new SqlParameter("@Period",SqlDbType.VarChar,50);
			objParam[7].Value = objCategoryWiseSearchReport.Period;

            objParam[8] = new SqlParameter("@SubCategoryID", SqlDbType.Int);
            objParam[8].Value = objCategoryWiseSearchReport.SubCategoryID;

			try
			{
				dsCategoryWiseList = SqlHelper.ExecuteDataset(sqlConn,CommandType.StoredProcedure,"sp_GetCategoryWiseReport",objParam);
				return dsCategoryWiseList;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
			catch(V2Exceptions ex)
			{
				throw ;
			}

			catch(System.Exception ex)
			{
                
				FileLog objFileLog = FileLog.GetLogger();
				objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLCategoryWiseSearchReport.cs", "GetCategoryWiseReport", ex.StackTrace);
				throw new V2Exceptions(ex.ToString(),ex);
			}
		}

	}
}
