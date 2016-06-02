using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using V2.Orbit.Model;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Orbit.DataLayer
{
    public class AbsenteeismReportDAL : DBBaseClass
    {
        #region AbsenteeismReport
        public DataSet AbsenteeismReport(AbsenteeismReportModel objAbsenteeismReportModel, bool IsAdmin, bool AllTeammembers)
        {
            DataSet dsAbsenteeismReport=new DataSet();
            //SqlParameter[] objSqlParam = new SqlParameter[8];

            //objSqlParam[0] = new SqlParameter("@UserId", SqlDbType.Int);
            //objSqlParam[0].Value = objAbsenteeismReportModel.UserId;

            //objSqlParam[1] = new SqlParameter("@period", SqlDbType.NVarChar);
            //objSqlParam[1].Value = objAbsenteeismReportModel.Period;

            //if (objAbsenteeismReportModel.FromDate.ToString() != "1/1/0001 12:00:00 AM")
            //{

            //    objSqlParam[2] = new SqlParameter("@FromDate", SqlDbType.DateTime);
            //    objSqlParam[2].Value = objAbsenteeismReportModel.FromDate;
            //}
            //else
            //{
            //    objSqlParam[2] = new SqlParameter("@FromDate", SqlDbType.DateTime);
            //    objSqlParam[2].Value = null;
            //}
            //if (objAbsenteeismReportModel.ToDate.ToString() != "1/1/0001 12:00:00 AM")
            //{
            //    objSqlParam[3] = new SqlParameter("@Todate", SqlDbType.DateTime);
            //    objSqlParam[3].Value = objAbsenteeismReportModel.ToDate;
            //}
            //else
            //{
            //    objSqlParam[3] = new SqlParameter("@Todate", SqlDbType.DateTime);
            //    objSqlParam[3].Value = null;
            //}

            //objSqlParam[4] = new SqlParameter("@Month", SqlDbType.NVarChar);
            //objSqlParam[4].Value = objAbsenteeismReportModel.Month;

            //objSqlParam[5] = new SqlParameter("@Year", SqlDbType.NVarChar);
            //objSqlParam[5].Value = objAbsenteeismReportModel.Year;

            //objSqlParam[6] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
            //objSqlParam[6].Value = IsAdmin;

            //objSqlParam[7] = new SqlParameter("@AllTeammembers", SqlDbType.Bit);
            //objSqlParam[7].Value = AllTeammembers;


            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "AbsenteeismReport";
            cmd.CommandTimeout = 6000;
            //cmd.Parameters.Add(sqlParams);

            cmd.Parameters.Add("@UserId", SqlDbType.Int);
            cmd.Parameters["@UserId"].Value = objAbsenteeismReportModel.UserId;

            cmd.Parameters.Add("@ShiftID", SqlDbType.Int);
            cmd.Parameters["@ShiftID"].Value = objAbsenteeismReportModel.ShiftID;


            cmd.Parameters.Add("@Period", SqlDbType.NVarChar);
            cmd.Parameters["@Period"].Value = objAbsenteeismReportModel.Period;

            if (objAbsenteeismReportModel.FromDate.ToString() != "1/1/0001 12:00:00 AM")
            {

                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime);
                cmd.Parameters["@FromDate"].Value = objAbsenteeismReportModel.FromDate;
            }
            else
            {
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime);
                cmd.Parameters["@FromDate"].Value = null;
            }
            if (objAbsenteeismReportModel.ToDate.ToString() != "1/1/0001 12:00:00 AM")
            {
                cmd.Parameters.Add("@Todate", SqlDbType.DateTime);
                cmd.Parameters["@Todate"].Value = objAbsenteeismReportModel.ToDate;
            }
            else
            {
                cmd.Parameters.Add("@Todate", SqlDbType.DateTime);
                cmd.Parameters["@Todate"].Value = null;
            }

          

            

            //objSqlParam[7] = new SqlParameter("@AllTeammembers", SqlDbType.Bit);
            //objSqlParam[7].Value = AllTeammembers;

            cmd.Parameters.Add("@month", SqlDbType.NVarChar);
            cmd.Parameters["@month"].Value = objAbsenteeismReportModel.Month;

            cmd.Parameters.Add("@year", SqlDbType.NVarChar);
            cmd.Parameters["@year"].Value = objAbsenteeismReportModel.Year;

            cmd.Parameters.Add("@IsAdmin", SqlDbType.Bit);
            cmd.Parameters["@IsAdmin"].Value = IsAdmin;

            cmd.Parameters.Add("@AllTeammembers", SqlDbType.Bit);
            cmd.Parameters["@AllTeammembers"].Value = AllTeammembers;

           

            cmd.Connection = con;

            SqlDataAdapter da = new SqlDataAdapter(cmd); 

            try
            {
                da.Fill(dsAbsenteeismReport);
                return dsAbsenteeismReport;

                //dsAbsenteeismReport = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "AbsenteeismReport", objSqlParam);
                //return dsAbsenteeismReport;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AbsenteeismReportDAL.cs", "AbsenteeismReport", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion

        #region GetEmployeeNameRpt
        public DataSet GetEmployeeNameRpt(AbsenteeismReportModel objAbsenteeismReportModelModel)
        {
            DataSet dsGetEmployeeNameRpt;
            SqlParameter[] objSqlParam = new SqlParameter[1];
            objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            objSqlParam[0].Value = objAbsenteeismReportModelModel.UserId;

            try
            {
                dsGetEmployeeNameRpt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetEmployeeNameRpt", objSqlParam);
                return dsGetEmployeeNameRpt;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AbsentismReportDAL.cs", "GetEmployeeNameRpt", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion
        
    }
}
