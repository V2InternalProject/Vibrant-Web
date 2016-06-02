using System;
using System.Collections.Generic;
using System.Text;
using V2.Orbit.Model;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;

namespace V2.Orbit.DataLayer
{
    public class AttendanceReportDAL : DBBaseClass
    {


        public DataSet BindEmployee(AttendanceReportModel objAttendanceReportModel)
        {
            DataSet dsEmployeeName;
            SqlParameter[] sqlParams = new SqlParameter[1];

            sqlParams[0] = new SqlParameter("@UserID", SqlDbType.Int);
            sqlParams[0].Value = objAttendanceReportModel.UserId;

            dsEmployeeName = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetEmployeeNameRpt", sqlParams);
            return dsEmployeeName;
        }

        public DataSet GetAttendanceReport(AttendanceReportModel objAttendanceReportModel, Boolean IsAdmin, Boolean AllTeammembers)
        {
            DataSet dsGetAttendanceReport = new DataSet();
           // SqlParameter[] sqlParams = new SqlParameter[5];
            //sqlParams[0] = new SqlParameter("@EmployeeId", SqlDbType.Int);
            //sqlParams[0].Value = objAttendanceReportModel.UserId;


            //sqlParams[1] = new SqlParameter("@month", SqlDbType.VarChar, 15);
            //sqlParams[1].Value = objAttendanceReportModel.Month;


            //sqlParams[2] = new SqlParameter("@year", SqlDbType.VarChar, 10);
            //sqlParams[2].Value = objAttendanceReportModel.Year;

            //sqlParams[3] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
            //sqlParams[3].Value = IsAdmin;


            //sqlParams[4] = new SqlParameter("@AllTeammembers", SqlDbType.Bit);
            //sqlParams[4].Value = AllTeammembers ;

            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "AttendanceReport";
            cmd.CommandTimeout = 6000;
            //cmd.Parameters.Add(sqlParams);
            cmd.Parameters.Add("@EmployeeId", SqlDbType.Int);
            cmd.Parameters["@EmployeeId"].Value = objAttendanceReportModel.UserId;

            cmd.Parameters.Add("@ShiftId", SqlDbType.Int);
            cmd.Parameters["@ShiftId"].Value = objAttendanceReportModel.ShiftID;

            cmd.Parameters.Add("@month", SqlDbType.VarChar);
            cmd.Parameters["@month"].Value = objAttendanceReportModel.Month;

            cmd.Parameters.Add("@year", SqlDbType.VarChar);
            cmd.Parameters["@year"].Value = objAttendanceReportModel.Year;

            cmd.Parameters.Add("@IsAdmin", SqlDbType.Bit);
            cmd.Parameters["@IsAdmin"].Value = IsAdmin;

            cmd.Parameters.Add("@AllTeammembers", SqlDbType.Bit);
            cmd.Parameters["@AllTeammembers"].Value =AllTeammembers;

            cmd.Connection = con;
            
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dsGetAttendanceReport);
            
            return dsGetAttendanceReport;
           

           // dsGetAttendanceReport = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "AttendanceReport", sqlParams);
            //return dsGetAttendanceReport;
        }
    }
}
