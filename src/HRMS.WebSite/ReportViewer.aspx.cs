using HRMS.DAL;
using Microsoft.Reporting.WebForms;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace HRMS.Views.Reports
{
    public partial class ReportViewer : Page
    {
        private bool AllTeammembers = false;
        protected string ConnectionString = "";
        private DataSet dsGetEmployeeName = new DataSet();
        private DataSet dsGetShiftData = new DataSet();
        private DataSet dsGetStatus = new DataSet();
        private DataSet dsSearchLeaveRpt = new DataSet();
        private bool IsAdmin = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            // var selectedGuid=null;

            var selectedGuid = Guid.Parse(Convert.ToString(Request.QueryString["selectedGuid"]));

            var reportID = Request.QueryString["reportID"];

            if (!IsPostBack)
            {
                LoadReport(selectedGuid, Convert.ToInt32(reportID));
            }
        }

        public void LoadReport(Guid selectedGuid, int reportID)
        {
            try
            {
                rptLeaveReport.LocalReport.DataSources.Clear();
                ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
                var dsSearchLeaveRpt = new DataSet();

                var context = new ReportDAL();
                var reporetSchema = context.GetReportSchema(selectedGuid, reportID);
                var reportMasterData = context.GetReportMasterData(reportID);

                var con = new SqlConnection(ConnectionString);
                var cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = reporetSchema.ProcName;
                cmd.CommandTimeout = 6000;
                cmd.Connection = con;
                if (reportID == 23)
                {
                    cmd.Parameters.AddWithValue(reporetSchema.ProcInputItemList[0].ParameterName,
                        reporetSchema.ProcInputItemList[0].Value);
                }
                else
                {
                    for (var i = 0; i < reporetSchema.ProcInputItemList.Count; i++)
                    {
                        cmd.Parameters.AddWithValue(reporetSchema.ProcInputItemList[i].ParameterName,
                            reporetSchema.ProcInputItemList[i].Value);
                    }
                }

                var da = new SqlDataAdapter(cmd);
                da.Fill(dsSearchLeaveRpt);

                var reportDatasetName = reportMasterData.ReportDatasetName.Split(',');

                for (var i = 0; i < dsSearchLeaveRpt.Tables.Count; i++)
                {
                    rptLeaveReport.LocalReport.DataSources.Add(
                        new ReportDataSource(Convert.ToString(reportDatasetName[i]), dsSearchLeaveRpt.Tables[i]));
                }
                rptLeaveReport.LocalReport.ReportPath =
                    Server.MapPath(string.Format("./Reports/{0}", reporetSchema.FileName));
                rptLeaveReport.LocalReport.Refresh();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportViewer.aspx.cs", "LoadReport", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}