//using System;
//using System.Data;
//using System.Configuration;
//using System.Collections;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
//using Microsoft.Reporting.WebForms;
//using V2.CommonServices.Exceptions;
//using V2.CommonServices.FileLogger;
//using V2.Orbit.BusinessLayer;
//using V2.Orbit.Model;
//using System.Data.SqlClient;
//using Microsoft.ApplicationBlocks.Data;
//using V2.CommonServices;

//public partial class AttendanceReport : System.Web.UI.Page
//{
//    AttendanceReportModel objAttendanceReportModel = new AttendanceReportModel();
//    AttendanceReportBOL objAttendanceReportBOL = new AttendanceReportBOL();
//    RosterPlanningBOL objRosterPlanningBOL = new RosterPlanningBOL();
//    RosterPlanningModel objRosterPlanningModel = new RosterPlanningModel();
//    OutOfOfficeModel objOutOfOfficeModel = new OutOfOfficeModel();
//    OutOfOfficeBOL objOutOfOfficeBOL = new OutOfOfficeBOL();

//    DataSet dsEmployeeNames = new DataSet();
//    DataSet dsGetShiftData = new DataSet();
//    Boolean IsAdmin = false;
//    Boolean AllTeammembers = false;
//    protected void Page_Load(object sender, EventArgs e)
//    {
//        try
//        {
//            if (!IsPostBack)
//            {
//                getEmployeeName();
//                BindShift();
//                bindDropDownMonthYear();
//                ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
//                //ddlYear.SelectedItem.Text = DateTime.Now.Year.ToString();
//                ddlYear.SelectedValue = DateTime.Now.Year.ToString();
//            }

//            AttendanceReportViewer.Visible = false;
//        }
//        catch (V2Exceptions ex)
//        {
//            throw;
//        }
//        catch (System.Exception ex)
//        {
//            FileLog objFileLog = FileLog.GetLogger();
//            objFileLog.WriteLine(LogType.Error, ex.Message, "AttendanceReport.cs", "Page_Load", ex.StackTrace);
//            throw new V2Exceptions(ex.ToString(),ex);
//        }
//    }

//    private void bindDropDownMonthYear()
//    {
//        try
//        {
//            ddlMonth.Items.Add(new ListItem("January", "1"));
//            ddlMonth.Items.Add(new ListItem("February", "2"));
//            ddlMonth.Items.Add(new ListItem("March", "3"));
//            ddlMonth.Items.Add(new ListItem("April", "4"));
//            ddlMonth.Items.Add(new ListItem("May", "5"));
//            ddlMonth.Items.Add(new ListItem("June", "6"));
//            ddlMonth.Items.Add(new ListItem("July", "7"));
//            ddlMonth.Items.Add(new ListItem("August", "8"));
//            ddlMonth.Items.Add(new ListItem("September", "9"));
//            ddlMonth.Items.Add(new ListItem("October", "10"));
//            ddlMonth.Items.Add(new ListItem("November", "11"));
//            ddlMonth.Items.Add(new ListItem("December", "12"));

//            //for (int year = 0; year < 10; year++)
//            //{
//            //    ddlYear.Items.Add(new ListItem("Year", "0"));
//            //    ddlYear.Items.Add(new ListItem((year + 2005).ToString(), (year).ToString()));
//            //}
//            for (int year = 2006; year <= DateTime.Now.Year + 1; year++)
//            {
//                ddlYear.Items.Add(year.ToString());
//            }
//        }
//        catch (V2Exceptions ex)
//        {
//            throw;
//        }
//        catch (System.Exception ex)
//        {
//            FileLog objFileLog = FileLog.GetLogger();
//            objFileLog.WriteLine(LogType.Error, ex.Message, "AttendanceReport.cs", "bindDropDownMonthYear", ex.StackTrace);
//            throw new V2Exceptions(ex.ToString(),ex);
//        }

//    }

//    public void getEmployeeName()
//    {
//        try
//        {
//            objAttendanceReportModel.UserId = Convert.ToInt32(User.Identity.Name);
//            dsEmployeeNames = objAttendanceReportBOL.BindEmployee(objAttendanceReportModel);
//            ddlEmployeeName.Items.Clear();
//            ddlEmployeeName.Items.Add(new ListItem("All", "0"));
//            if (Roles.IsUserInRole("Admin"))
//            {
//                for (int i = 0; i < dsEmployeeNames.Tables[1].Rows.Count; i++)
//                {
//                    ddlEmployeeName.Items.Add(new ListItem(dsEmployeeNames.Tables[1].Rows[i]["EmployeeName"].ToString(), dsEmployeeNames.Tables[1].Rows[i]["UserId"].ToString()));
//                }
//            }
//            else if (Roles.IsUserInRole("Team Leads") || Roles.IsUserInRole("Manager"))
//            {
//                for (int j = 0; j < dsEmployeeNames.Tables[4].Rows.Count; j++)
//                {
//                    ddlEmployeeName.Items.Add(new ListItem(dsEmployeeNames.Tables[4].Rows[j]["EmployeeName"].ToString(), dsEmployeeNames.Tables[4].Rows[j]["UserId"].ToString()));
//                }

//            }
//            else if (Roles.IsUserInRole("Developers"))
//            {
//                // ddlEmployeeName.Visible = false;
//                trEmployee.Visible = false;
//            }
//        }
//        catch (V2Exceptions ex)
//        {
//            throw;
//        }
//        catch (System.Exception ex)
//        {
//            FileLog objFileLog = FileLog.GetLogger();
//            objFileLog.WriteLine(LogType.Error, ex.Message, "AttendanceReport.cs", "BindEmployee", ex.StackTrace);
//            throw new V2Exceptions(ex.ToString(),ex);
//        }
//        //ddlEmployeeName.DataBind();
//    }

//    #region GetEmployeeNameShift
//    public void getEmployeeNameForShift()
//    {
//        try
//        {
//            objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name);
//            objOutOfOfficeModel.ShiftID = Convert.ToInt32(ddlShift.SelectedValue);
//            dsEmployeeNames = objOutOfOfficeBOL.GetEmployeeNameRptShift(objOutOfOfficeModel);
//            ddlEmployeeName.Items.Clear();
//            ddlEmployeeName.Items.Add(new ListItem("All", "0"));

//            for (int j = 0; j < dsEmployeeNames.Tables[0].Rows.Count; j++)
//            {
//                ddlEmployeeName.Items.Add(new ListItem(dsEmployeeNames.Tables[0].Rows[j]["EmployeeName"].ToString(), dsEmployeeNames.Tables[0].Rows[j]["UserID"].ToString()));
//            }

//        }
//        catch (V2Exceptions ex)
//        {
//            throw;
//        }
//        catch (System.Exception ex)
//        {
//            FileLog objFileLog = FileLog.GetLogger();
//            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveReport.aspx.cs", "getEmployeeNameShift", ex.StackTrace);
//            throw new V2Exceptions(ex.ToString(),ex);
//        }

//    }
//    #endregion

//    public void BindShift()
//    {
//        try
//        {
//            objRosterPlanningModel.UserId = Convert.ToInt32(User.Identity.Name);
//            dsGetShiftData = objRosterPlanningBOL.GetShift(objRosterPlanningModel);
//            ddlShift.Items.Clear();
//            ddlShift.Items.Add(new ListItem("All", "0"));
//            for (int i = 0; i < dsGetShiftData.Tables[0].Rows.Count; i++)
//            {
//                ddlShift.Items.Add(new ListItem(dsGetShiftData.Tables[0].Rows[i]["Description"].ToString(), dsGetShiftData.Tables[0].Rows[i]["ShiftID"].ToString()));
//            }

//        }
//        catch (V2Exceptions ex)
//        {
//            throw;
//        }
//        catch (System.Exception ex)
//        {
//            FileLog objFileLog = FileLog.GetLogger();
//            objFileLog.WriteLine(LogType.Error, ex.Message, "AttendanceReport.cs", "BindEmployee", ex.StackTrace);
//            throw new V2Exceptions(ex.ToString(),ex);
//        }
//        //ddlEmployeeName.DataBind();
//    }

//    private void getAttendanceReport(AttendanceReportModel objAttendanceReportModel, Boolean IsAdmin, Boolean AllTeammembers)
//    {
//        try
//        {
//            DataSet dsReport = new DataSet();
//            dsReport = objAttendanceReportBOL.GetAttendanceReport(objAttendanceReportModel, IsAdmin, AllTeammembers);

//            //ReportDataSource source = new ReportDataSource("AttendanceReport_AttendanceReport", dsReport.Tables[0]);
//            ReportDataSource source = new ReportDataSource("AttendanceReport_AttendanceReport", dsReport.Tables[0]);

//            AttendanceReportViewer.LocalReport.DataSources.Clear();
//            AttendanceReportViewer.LocalReport.DataSources.Add(source);
//            if (dsReport.Tables[0].Rows.Count <= 0)
//            {
//                lblError.Text = "No record found";
//                AttendanceReportViewer.Visible = false;
//            }
//            else
//            {
//                lblError.Text = "";
//                AttendanceReportViewer.Visible = true;
//                AttendanceReportViewer.LocalReport.Refresh();
//            }
//        }
//        catch (V2Exceptions ex)
//        {
//            throw;
//        }
//        catch (System.Exception ex)
//        {
//            FileLog objFileLog = FileLog.GetLogger();
//            objFileLog.WriteLine(LogType.Error, ex.Message, "AttendanceReport.cs", "getAttendanceReport", ex.StackTrace);
//            throw new V2Exceptions(ex.ToString(),ex);
//        }

//        #region Commented
//        //DataSet dsReport = new DataSet();
//        //SqlDataReader drReport;
//        //DataTable dtReport = new DataTable();
//        //drReport = objAttendanceReportBOL.GetAttendanceReport(objAttendanceReportModel);
//        //dsReport.Tables.Add(dtReport);
//        //dsReport.Tables[0].Reset();
//        //dsReport.Tables[0].Load(drReport);
//        ////ReportDataSource rds = new ReportDataSource("AttendanceReport_AttendanceReport", dsReport.Tables[0]);
//        ////AttendanceReportViewer.LocalReport.ReportPath = Server.MapPath("AttendanceReportTemp.rdlc");
//        //ReportDataSource rds = new ReportDataSource("DataSet1_AttendanceReport", dsReport.Tables[0]);
//        //AttendanceReportViewer.LocalReport.ReportPath = Server.MapPath("AttendanceReportTemp.rdlc");
//        ////rds.Name = "Dataset1_AttendanceReport";
//        ////rds.Value = dsReport.Tables[0];
//        //AttendanceReportViewer.LocalReport.DataSources.Clear();
//        //AttendanceReportViewer.LocalReport.DataSources.Add(rds);
//        //AttendanceReportViewer.LocalReport.Refresh();

//        ////string reportPath = Server.MapPath("AttendanceReport.rdlc");
//        ////ReportViewer rView = new ReportViewer();
//        //////rView.Dock = DockStyle.Fill;
//        ////this.Controls.Add(rView);
//        ////rView.LocalReport.DataSources.Add(new ReportDataSource("Dataset1_AttendanceReport", dsReport.Tables[0]));

//        ////// Set the active report path of the ReportViewer object
//        ////rView.LocalReport.ReportPath = reportPath;
//        #endregion

//    }

//    protected void ddlShift_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        if (ddlShift.SelectedIndex == 0)
//            getEmployeeName();
//        else
//        {
//            getEmployeeNameForShift();
//        }

//    }

//    protected void btnSubmit_Click(object sender, EventArgs e)
//    {
//        try
//        {
//            //if (Roles.IsUserInRole("Developers"))
//            //{
//            //   // ddlEmployeeName.Visible = false;
//            //    objAttendanceReportModel.UserId = Convert.ToInt32(User.Identity.Name);
//            //}
//            //else
//            //{
//            //    objAttendanceReportModel.UserId = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);
//            //}
//            if (Roles.IsUserInRole("Admin"))
//            {
//                IsAdmin = true;
//                AllTeammembers = true;
//            }
//            if (Roles.IsUserInRole("Team Leads") || Roles.IsUserInRole("Manager"))
//            {
//                AllTeammembers = true;
//            }
//            if (ddlEmployeeName.SelectedItem.Value == "0")
//            {
//                objAttendanceReportModel.UserId = Convert.ToInt32(User.Identity.Name);
//            }
//            else
//            {
//                objAttendanceReportModel.UserId = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);
//                IsAdmin = false;
//                AllTeammembers = false;
//            }
//            objAttendanceReportModel.Month = ddlMonth.SelectedItem.Value;

//            if (ddlShift.SelectedIndex == 0)
//                objAttendanceReportModel.ShiftID = 0;
//            else
//                objAttendanceReportModel.ShiftID = Convert.ToInt16(ddlShift.SelectedItem.Value);

//            objAttendanceReportModel.Year = ddlYear.SelectedItem.Text;
//            if (Convert.ToInt32(ddlYear.SelectedValue) == System.DateTime.Now.Year)
//            {
//                if (Convert.ToInt32(ddlMonth.SelectedItem.Value) <= System.DateTime.Now.Month)
//                {
//                    lblError.Visible = false;
//                    AttendanceReportViewer.Visible = true;
//                    getAttendanceReport(objAttendanceReportModel, IsAdmin, AllTeammembers);
//                }
//                else
//                {
//                    AttendanceReportViewer.Visible = false;
//                    lblError.Visible = true;
//                    lblError.Text = "Attendance Report can not generated for future date ";
//                }
//            }
//            else if (Convert.ToInt32(ddlYear.SelectedValue) < System.DateTime.Now.Year)
//            {
//                lblError.Visible = false;
//                AttendanceReportViewer.Visible = true;
//                getAttendanceReport(objAttendanceReportModel, IsAdmin, AllTeammembers);
//            }
//            else
//            {
//                AttendanceReportViewer.Visible = false;
//                lblError.Visible = true;
//                lblError.Text = "Attendance Report can not generated for future date ";
//            }
//            //getAttendanceReport(objAttendanceReportModel, IsAdmin, AllTeammembers);
//        }
//        catch (V2Exceptions ex)
//        {
//            throw;
//        }
//        catch (System.Exception ex)
//        {
//            FileLog objFileLog = FileLog.GetLogger();
//            objFileLog.WriteLine(LogType.Error, ex.Message, "AttendanceReport.cs", "btnSubmit_Click", ex.StackTrace);
//            throw new V2Exceptions(ex.ToString(),ex);
//        }
//    }

//}