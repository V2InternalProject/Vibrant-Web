using HRMS;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;
using V2.Helpdesk.Model;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for ReportSearch.
    /// </summary>
    public partial class MemberWiseSearchReport : System.Web.UI.Page
    {
        //protected System.Web.UI.WebControls.Literal Literal1;
        private static DataSet dsGetReport;

        public int EmployeeID, SAEmployeeID, OnlySuperAdmin, onlyExecutive;
        private DataSet dsYear;
        private clsMemberWiseSearchReport objClsMemberWiseSearchReport = new clsMemberWiseSearchReport();
        private clsBLMemberWiseSearchReport objClsBLMemberWiseSearchReport = new clsBLMemberWiseSearchReport();
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                string PageName = "Reports";
                objpagelevel.PageLevelAccess(PageName);

                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                OnlySuperAdmin = Convert.ToInt32(Session["OnlySuperAdmin"]);
                onlyExecutive = Convert.ToInt32(Session["IsExecutive"]);
                if (OnlySuperAdmin != 0 || onlyExecutive == 1)
                {
                    Response.Redirect("AuthorizationErrorMessage.aspx?PageSource=SearchReport");
                }

                if (SAEmployeeID.ToString() == "" || SAEmployeeID == 0)
                {
                    Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
                }

                if (!IsPostBack)
                {
                    BindEmployeeNames();
                    GetStatus();
                    lBtnPrint.Visible = false;
                    lblSeparator.Visible = false;
                    lBtnExcel.Visible = false;
                    BindYear();
                }
                if ((Page.FindControl("Literal1")) != null)
                {
                    ((Literal)(Page.FindControl("Literal1"))).Text = "";
                    Controls.Remove(Page.FindControl("Literal1"));
                }

                btnSubmit.Attributes.Add("onClick", "return CheckDate();");
            }
            catch (System.Threading.ThreadAbortException ex)
            {
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MemberWiseSearchReport.aspx", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void BindYear()
        {
            dsYear = objClsBLMemberWiseSearchReport.getYears();

            ddlYears.Items.Clear();
            for (int i = 0; i < dsYear.Tables[0].Rows.Count; i++)
            {
                ddlYears.Items.Add(new ListItem(dsYear.Tables[0].Rows[i]["Year"].ToString()));
            }
        }

        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion Web Form Designer generated code

        private void BindEmployeeNames()
        {
            try
            {
                clsBLMemberWiseSearchReport objClsBLMemberWiseSearchReport = new clsBLMemberWiseSearchReport();
                objClsMemberWiseSearchReport.EmployeeID = SAEmployeeID;
                DataSet dsBindEmployeeNames = objClsBLMemberWiseSearchReport.getAllEmployee(objClsMemberWiseSearchReport);
                ddlEmployeeName.Items.Add(new ListItem("All", "0"));
                for (int i = 0; i < dsBindEmployeeNames.Tables[0].Rows.Count; i++)
                {
                    if (dsBindEmployeeNames.Tables[0].Rows.Count > 0)
                    {
                        ddlEmployeeName.Items.Add(new ListItem(dsBindEmployeeNames.Tables[0].Rows[i]["EmployeeName"].ToString(), dsBindEmployeeNames.Tables[0].Rows[i]["EmployeeID"].ToString())); ;
                    }
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MemberWiseSearchReport.aspx", "BindEmployeeNames", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void BindStatus()
        {
            try
            {
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MemberWiseSearchReport.aspx", "BindStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                dgReport.CurrentPageIndex = 0;
                GetResults();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MemberWiseSearchReport.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void GetStatus()
        {
            objClsMemberWiseSearchReport = new clsMemberWiseSearchReport();
            objClsBLMemberWiseSearchReport = new clsBLMemberWiseSearchReport();

            DataSet dsStatus = new DataSet();
            dsStatus = objClsBLMemberWiseSearchReport.GetStatus();

            for (int i = 0; i < dsStatus.Tables[0].Rows.Count; i++)
            {
                ddlStatus.Items.Add(new ListItem(dsStatus.Tables[0].Rows[i]["StatusDesc"].ToString(), dsStatus.Tables[0].Rows[i]["StatusID"].ToString()));
            }
            ddlStatus.Items.Insert(0, "All");
        }

        private void GetResults()
        {
            int StatusID = 0;
            try
            {
                int EmployeeID;
                clsMemberWiseSearchReport objClsMemberWiseSearchReport = new clsMemberWiseSearchReport();
                clsBLMemberWiseSearchReport objClsBLMemberWiseSearchReport = new clsBLMemberWiseSearchReport();
                //added here
                int superAdmin;
                superAdmin = Convert.ToInt32(Session["SAEmployeeID"]);
                //till here

                EmployeeID = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);
                objClsMemberWiseSearchReport.EmployeeID = EmployeeID;

                if (ddlStatus.SelectedItem.Value != "All")
                {
                    StatusID = Convert.ToInt32(ddlStatus.SelectedItem.Value);
                    objClsMemberWiseSearchReport.StatusID = StatusID;
                }
                else
                    objClsMemberWiseSearchReport.StatusID = -1;
                objClsMemberWiseSearchReport.Period = ddlPeriod.SelectedItem.Value;
                if (ddlPeriod.SelectedItem.Value.ToLower() == "day" || ddlPeriod.SelectedItem.Value.ToLower() == "week")
                {
                    objClsMemberWiseSearchReport.Date = txtDay.Text.Trim();
                    objClsMemberWiseSearchReport.Month = "";
                    objClsMemberWiseSearchReport.Year = "";
                    objClsMemberWiseSearchReport.FromDate = "";
                    objClsMemberWiseSearchReport.ToDate = "";
                }
                else if (ddlPeriod.SelectedItem.Value.ToLower() == "month")
                {
                    objClsMemberWiseSearchReport.Date = "";
                    objClsMemberWiseSearchReport.Month = ddlMonths.SelectedValue;
                    objClsMemberWiseSearchReport.Year = ddlYears.SelectedValue;
                    objClsMemberWiseSearchReport.FromDate = "";
                    objClsMemberWiseSearchReport.ToDate = "";
                }
                else if (ddlPeriod.SelectedItem.Value.ToLower() == "year")
                {
                    objClsMemberWiseSearchReport.Date = "";
                    objClsMemberWiseSearchReport.Month = "";
                    objClsMemberWiseSearchReport.Year = ddlYears.SelectedValue;
                    objClsMemberWiseSearchReport.FromDate = "";
                    objClsMemberWiseSearchReport.ToDate = "";
                }
                else if (ddlPeriod.SelectedItem.Value.ToLower() == "range")
                {
                    objClsMemberWiseSearchReport.Date = "";
                    objClsMemberWiseSearchReport.Month = "";
                    objClsMemberWiseSearchReport.Year = "";
                    objClsMemberWiseSearchReport.FromDate = txtFromDate.Text.Trim();
                    objClsMemberWiseSearchReport.ToDate = txtToDate.Text.Trim();
                }
                dsGetReport = new DataSet();
                dsGetReport = objClsBLMemberWiseSearchReport.GetMemberWiseReport(objClsMemberWiseSearchReport, superAdmin);
                if (dsGetReport.Tables[0].Rows.Count > 0)
                {
                    dgReport.DataSource = dsGetReport.Tables[0];
                    dgReport.DataBind();
                    if (dgReport.PageCount > 1)
                    {
                        dgReport.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgReport.PagerStyle.Visible = false;
                    }

                    if (StatusID != -1)
                    {
                        dgReport.Columns[7].Visible = false;
                    }
                    else
                    {
                        dgReport.Columns[7].Visible = true;
                    }
                    if (EmployeeID != 0)
                    {
                        dgReport.Columns[5].Visible = false;
                    }
                    else
                    {
                        dgReport.Columns[5].Visible = true;
                    }
                    if (ddlPeriod.SelectedItem.Value == "day")
                    {
                        dgReport.Columns[2].Visible = false;
                    }
                    else
                    {
                        dgReport.Columns[2].Visible = true;
                    }
                    dgReport.Visible = true;
                    lblError.Visible = false;
                    lBtnPrint.Visible = true;
                    lblSeparator.Visible = true;
                    lBtnExcel.Visible = true;
                }
                else
                {
                    dgReport.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "No Records Found.";
                    lBtnPrint.Visible = false;
                    lblSeparator.Visible = false;
                    lBtnExcel.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MemberWiseSearchReport.aspx", "GetResults", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgReport_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "viewDetails")
                {
                    dgReport.EditItemIndex = e.Item.ItemIndex;
                    int IssueID = Convert.ToInt32(dgReport.DataKeys[e.Item.ItemIndex]);
                    Session["IssueID"] = IssueID;
                    Response.Redirect("ViewIssueStatus.aspx");
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MemberWiseSearchReport.aspx", "dgReport_ItemCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgReport_Pagination(object sencer, DataGridPageChangedEventArgs e)
        {
            try
            {
                dgReport.CurrentPageIndex = e.NewPageIndex;
                GetResults();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MemberWiseSearchReport.aspx", "dgReport_Pagination", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void lBtnExcel_Click(object sender, System.EventArgs e)
        {
            try
            {
                string strFileNameCust = "Membser-Wise Issue Report.xls";
                DataGrid dgForExcel = new DataGrid();
                if (dsGetReport.Tables[0].Rows.Count > 0)
                {
                    dgForExcel.AutoGenerateColumns = false;
                    BoundColumn IssueID = new BoundColumn();
                    IssueID.HeaderText = "Issue ID";
                    IssueID.DataField = "ReportIssueID";
                    dgForExcel.Columns.Add(IssueID);

                    BoundColumn ReportedBy = new BoundColumn();
                    ReportedBy.HeaderText = "Report By";
                    ReportedBy.DataField = "Name";
                    dgForExcel.Columns.Add(ReportedBy);

                    BoundColumn ReportedOn = new BoundColumn();
                    ReportedOn.HeaderText = "Reported On";
                    ReportedOn.DataField = "ReportIssueDate";
                    dgForExcel.Columns.Add(ReportedOn);

                    BoundColumn Severity = new BoundColumn();
                    Severity.HeaderText = "Severity";
                    Severity.DataField = "ProblemSeverity";
                    dgForExcel.Columns.Add(Severity);

                    BoundColumn SubCategory = new BoundColumn();
                    SubCategory.HeaderText = "Category";
                    SubCategory.DataField = "SubCategoryID";
                    dgForExcel.Columns.Add(SubCategory);

                    BoundColumn AssignedTo = new BoundColumn();
                    AssignedTo.HeaderText = "Assigned To";
                    AssignedTo.DataField = "EmployeeName";
                    dgForExcel.Columns.Add(AssignedTo);

                    BoundColumn Status = new BoundColumn();
                    Status.HeaderText = "Status";
                    Status.DataField = "Status";
                    dgForExcel.Columns.Add(Status);

                    dgForExcel.DataSource = dsGetReport;
                    dgForExcel.DataBind();
                    if (dgForExcel.PageCount > 1)
                    {
                        dgForExcel.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgForExcel.PagerStyle.Visible = false;
                    }

                    Response.Clear();

                    Response.AddHeader("content-disposition", "attachment;FileName = " + strFileNameCust);
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter objStringWriter = new StringWriter();
                    HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                    dgForExcel.RenderControl(objHtmlTextWriter);
                    Response.Write(objStringWriter.ToString());
                    Response.End();
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No records available to generate Excel File";
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MemberWiseSearchReport.aspx", "lBtnExcel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void lBtnPrint_Click(object sender, System.EventArgs e)
        {
            try
            {
                Session["dsResults"] = dsGetReport;
                string Heading = "Issue Status wise report by each Members";
                Session["Heading"] = Heading;
                string strJScript = "<Script Language='javascript'>";

                strJScript += "popup('printPage.aspx');";
                strJScript += "</script>";
                Literal Literal1 = new Literal();
                Literal1.ID = "Literal1";
                Controls.Add(Literal1);
                Literal1.Text = strJScript;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MemberWiseSearchReport.aspx", "lBtnPrint_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgReport_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                foreach (DataGridItem dgi in dgReport.Items)
                {
                    if (dgi.ItemType == ListItemType.Item || dgi.ItemType == ListItemType.AlternatingItem)
                    {
                        int intStatusID = Convert.ToInt32(((Label)dgi.FindControl("lblStatusID")).Text);
                        Label lblStatusName = (Label)dgi.FindControl("lblStatusName");

                        if (intStatusID == 1)
                        {
                            lblStatusName.Text = IssueStatus.New.ToString();
                        }
                        else if (intStatusID == 2)
                        {
                            lblStatusName.Text = IssueStatus.Resolved.ToString();
                        }
                        else if (intStatusID == 3)
                        {
                            lblStatusName.Text = IssueStatus.Moved.ToString();
                        }
                        else if (intStatusID == 4)
                        {
                            lblStatusName.Text = IssueStatus.Reopen.ToString();
                        }
                    }
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MemberWiseSearchReport.aspx", "dgReport_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}