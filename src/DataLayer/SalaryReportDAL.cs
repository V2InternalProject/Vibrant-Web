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
    public class SalaryReportDAL : DBBaseClass
    {
        public DataSet getSalaryReport(SalaryReportModel objSalaryReportModel)
        {
            DataSet dsGetAttendanceReport = new DataSet();
            SqlParameter[] sqlParams = new SqlParameter[2];

            //sqlParams[0] = new SqlParameter("@month", SqlDbType.VarChar, 15);
            //sqlParams[0].Value = objSalaryReportModel.Month;


            //sqlParams[1] = new SqlParameter("@year", SqlDbType.VarChar, 10);
            //sqlParams[1].Value = objSalaryReportModel.Year;

            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SalaryReport";
            cmd.CommandTimeout = 6000;
            //cmd.Parameters.Add(sqlParams);
            cmd.Parameters.Add("@month", SqlDbType.VarChar);
            cmd.Parameters["@month"].Value = objSalaryReportModel.Month;

            cmd.Parameters.Add("@year", SqlDbType.VarChar);
            cmd.Parameters["@year"].Value = objSalaryReportModel.Year;
            cmd.Connection= con;
            
              SqlDataAdapter da=new SqlDataAdapter(cmd);
            
              da.Fill(dsGetAttendanceReport);
              
            //dsGetAttendanceReport = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SalaryReport", sqlParams);
            return dsGetAttendanceReport;
        }
    }
}
