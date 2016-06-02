using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using V2.CommonServices.FileLogger;
using V2.CommonServices.Exceptions;

namespace HRMS.Views.Reports
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        DataSet dsGetEmployeeName = new DataSet();
        DataSet dsGetStatus = new DataSet();
        DataSet dsSearchLeaveRpt = new DataSet();
        DataSet dsGetShiftData = new DataSet();
        bool IsAdmin = false;
        bool AllTeammembers = false;
        protected string ConnectionString = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                DataSet dsSearchLeaveRpt;
                SqlParameter[] objSqlParam = new SqlParameter[10];

                objSqlParam[0] = new SqlParameter("@UserId", SqlDbType.Int);
                objSqlParam[0].Value = 3493;

                objSqlParam[1] = new SqlParameter("@period", SqlDbType.NVarChar);
                objSqlParam[1].Value = "Month";

                objSqlParam[2] = new SqlParameter("@StatusId", SqlDbType.Int);
                objSqlParam[2].Value = 0;

                objSqlParam[3] = new SqlParameter("@LeaveDateFrom", SqlDbType.DateTime);
                objSqlParam[3].Value = null;

                objSqlParam[4] = new SqlParameter("@LeaveDateTo", SqlDbType.DateTime);
                objSqlParam[4].Value = null;

                objSqlParam[5] = new SqlParameter("@Month", SqlDbType.NVarChar);
                objSqlParam[5].Value = 4;

                objSqlParam[6] = new SqlParameter("@Year", SqlDbType.NVarChar);
                objSqlParam[6].Value = 2014;

                objSqlParam[7] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                objSqlParam[7].Value = true;

                objSqlParam[8] = new SqlParameter("@AllTeammembers", SqlDbType.Bit);
                objSqlParam[8].Value = true;

                objSqlParam[9] = new SqlParameter("@ShiftID", SqlDbType.Int);
                objSqlParam[9].Value = 0;

                dsSearchLeaveRpt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchLeaveRpt", objSqlParam);
                ReportDataSource source = new ReportDataSource("LeaveReport_SearchLeaveRpt", dsSearchLeaveRpt.Tables[0]);
                rptLeaveReport.LocalReport.DataSources.Clear();
                rptLeaveReport.LocalReport.DataSources.Add(source);
                rptLeaveReport.LocalReport.Refresh();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "SearchOutOfOfficeRpt", ex.StackTrace);
                throw new V2Exceptions();
            }

        }
    }
}