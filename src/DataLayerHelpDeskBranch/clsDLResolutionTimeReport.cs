using System;
using System.Data;
using System.Data.SqlClient;
using V2.Helpdesk.Model;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.DataLayer
{
    /// <summary>
    /// Summary description for clsDLResolutionTimeReport.
    /// </summary>
    public class clsDLResolutionTimeReport
    {
        string strConnectionString = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
        public clsDLResolutionTimeReport()
        {
            try
            {
                //
                // TODO: Add constructor logic here
                //
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTimeReport.cs", "clsDLResolutionTimeReport", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        public DataSet getProblemPriority()
        {
            try
            {
                DataSet dsProblemPriority = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetProblemPriority");
                return dsProblemPriority;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTimeReport.cs", "getProblemPriority", ex.StackTrace);
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
                DataSet dsYears = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetYears");
                return dsYears;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTimeReport.cs", "getYears", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet getProblemSeverity()
        {
            try
            {
                DataSet dsProblemSeverity = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetProblemSeverity");
                return dsProblemSeverity;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTimeReport.cs", "getProblemSeverity", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet getResolutionTimeDetails(clsResolutionTimeReport objClsResolutionTimeReport, int superAdmin)
        {
            try
            {
                //SqlParameter[] sqlParams = new SqlParameter[5];

                //sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int,100);
                //sqlParams[0].Value = objClsResolutionTimeReport.EmployeeID;
                //sqlParams[0].Direction = ParameterDirection.Input;


                //sqlParams[1] = new SqlParameter("@SeverityID", SqlDbType.Int,10);
                //sqlParams[1].Value = objClsResolutionTimeReport.SeverityID;
                //sqlParams[1].Direction = ParameterDirection.Input;


                //sqlParams[2] = new SqlParameter("@FromMonth", SqlDbType.VarChar,10);
                //sqlParams[2].Value = objClsResolutionTimeReport.FromMonth.ToShortDateString();
                //sqlParams[2].Direction = ParameterDirection.Input;

                //sqlParams[3] = new SqlParameter("@ToMonth", SqlDbType.VarChar,10);
                //sqlParams[3].Value = objClsResolutionTimeReport.ToMonth.ToShortDateString();
                //sqlParams[3].Direction = ParameterDirection.Input;

                //sqlParams[4] = new SqlParameter("@superAdmin", SqlDbType.Int);
                //sqlParams[4].Value = superAdmin;
                //sqlParams[4].Direction = ParameterDirection.Input;


                DataSet dsResolutionTimeDetails = new DataSet();

                SqlConnection con = new SqlConnection(strConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetResolutionTimeDetails_New1";
                cmd.CommandTimeout = 6000;

                cmd.Parameters.Add("@EmployeeID", SqlDbType.Int, 100);
                cmd.Parameters["@EmployeeID"].Value = objClsResolutionTimeReport.EmployeeID;

                cmd.Parameters.Add("@SeverityID", SqlDbType.Int, 10);
                cmd.Parameters["@SeverityID"].Value = objClsResolutionTimeReport.SeverityID;

                cmd.Parameters.Add("@FromMonth", SqlDbType.VarChar, 10);
                cmd.Parameters["@FromMonth"].Value = objClsResolutionTimeReport.FromMonth.ToShortDateString();

                cmd.Parameters.Add("@ToMonth", SqlDbType.VarChar, 10);
                cmd.Parameters["@ToMonth"].Value = objClsResolutionTimeReport.ToMonth.ToShortDateString();

                cmd.Parameters.Add("@superAdmin", SqlDbType.Int);
                cmd.Parameters["@superAdmin"].Value = superAdmin;

                cmd.Connection = con;

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dsResolutionTimeDetails);

                return dsResolutionTimeDetails;

                //DataSet dsResolutionTimeDetails = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetResolutionTimeDetails_New1", sqlParams);
                //return dsResolutionTimeDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTimeReport.cs", "getResolutionTimeDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet getGraphDetails(clsResolutionTimeReport objClsResolutionTimeReport)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[5];

                sqlParams[0] = new SqlParameter("@EmployeeID", SqlDbType.Int, 100);
                sqlParams[0].Value = objClsResolutionTimeReport.EmployeeID;
                sqlParams[0].Direction = ParameterDirection.Input;

                sqlParams[1] = new SqlParameter("@PriorityID", SqlDbType.Int, 10);
                sqlParams[1].Value = objClsResolutionTimeReport.PriorityID;
                sqlParams[1].Direction = ParameterDirection.Input;

                sqlParams[2] = new SqlParameter("@SeverityID", SqlDbType.Int, 10);
                sqlParams[2].Value = objClsResolutionTimeReport.SeverityID;
                sqlParams[2].Direction = ParameterDirection.Input;

                sqlParams[3] = new SqlParameter("@FromMonth", SqlDbType.VarChar, 12);
                sqlParams[3].Value = objClsResolutionTimeReport.FromMonth.ToShortDateString();
                sqlParams[3].Direction = ParameterDirection.Input;

                sqlParams[4] = new SqlParameter("@ToMonth", SqlDbType.VarChar, 12);
                sqlParams[4].Value = objClsResolutionTimeReport.ToMonth.ToShortDateString();
                sqlParams[4].Direction = ParameterDirection.Input;

                DataSet dsResolutionTimeDetails = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "sp_GetResolutionTimeGraph", sqlParams);
                return dsResolutionTimeDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "clsDLResolutionTimeReport.cs", "getGraphDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

    }
}
