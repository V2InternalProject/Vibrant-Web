using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;
using V2.Helpdesk.Model;

/*using ComponentArt.Charting;
using ComponentArt.Charting.Design;*/

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for ResolutionTimeReport.
    /// </summary>
    public partial class ResolutionTimeReport : System.Web.UI.Page
    {
        //protected System.Web.UI.WebControls.DropDownList ddlFrommonth;
        public DataSet dsResolutionTimeReport;

        public DataView dv;
        public clsResolutionTimeReport objClsResolutionTimeReport;
        private clsBLResolutionTimeReport objClsBLResolutionTimeReport = new clsBLResolutionTimeReport();
        public int EmployeeID, SAEmployeeID, OnlySuperAdmin;
        public string selectedToMonth, selectedFromMonth;
        private int IssueHealthId = 0;
        private string strFileNameCust = String.Empty;
        public DataSet dsYear;
        //protected ComponentArt.Charting.WebChart Chart1;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Put user code to initialize the page here
                //code to clear all the cache so that after logout, previous page cannot be revisited.
                //--- starts here---------

                //--- ends here---------
                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                OnlySuperAdmin = Convert.ToInt32(Session["OnlySuperAdmin"]);
                if (OnlySuperAdmin != 0)
                {
                    Response.Redirect("AuthorizationErrorMessage.aspx");
                }
                //if (EmployeeID.ToString() == "" || EmployeeID == 0)
                //{
                if (SAEmployeeID.ToString() == "" || SAEmployeeID == 0)
                {
                    Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
                }
                //}
                //else
                //{
                //    Response.Redirect("AuthorizationErrorMessage.aspx");
                //}
                if (!Page.IsPostBack)
                {
                    //Chart1.Visible = false;

                    BindEmployeeNames();
                    //BindProblemPriority();
                    BindProblemSeverity();
                    BindYear();
                }
                //btnSubmit.Attributes.Add("OnClick","return CompareMonths('selectedFromMonth', 'selectedToMonth');");
                btnSubmit.Attributes.Add("OnClick", "return DateRequired();");
                btnGraphicalRepresentation.Attributes.Add("onClick", "return DateRequired();");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void BindYear()
        {
            dsYear = objClsBLResolutionTimeReport.getYears();

            ddlFromYear.Items.Clear();
            for (int i = 0; i < dsYear.Tables[0].Rows.Count; i++)
            {
                ddlFromYear.Items.Add(new ListItem(dsYear.Tables[0].Rows[i]["Year"].ToString()));
            }

            ddlToYear.Items.Clear();
            for (int i = 0; i < dsYear.Tables[0].Rows.Count; i++)
            {
                ddlToYear.Items.Add(new ListItem(dsYear.Tables[0].Rows[i]["Year"].ToString()));
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
            this.Datagrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.Datagrid1_ItemDataBound);
        }

        #endregion Web Form Designer generated code

        private void BindEmployeeNames()
        {
            try
            {
                ddlEmployeeName.Items.Add(new ListItem("All", "0"));
                clsBLMemberWiseSearchReport objClsBLMemberWiseSearchReport = new clsBLMemberWiseSearchReport();
                clsMemberWiseSearchReport objclsMemberWiseSearchReport = new clsMemberWiseSearchReport();
                objclsMemberWiseSearchReport.EmployeeID = SAEmployeeID;
                DataSet dsBindEmployeeNames = objClsBLMemberWiseSearchReport.getAllEmployee(objclsMemberWiseSearchReport);
                for (int i = 0; i < dsBindEmployeeNames.Tables[0].Rows.Count; i++)
                {
                    if (dsBindEmployeeNames.Tables[0].Rows.Count > 0)
                    {
                        ddlEmployeeName.Items.Add(new ListItem(dsBindEmployeeNames.Tables[0].Rows[i]["EmployeeName"].ToString(), dsBindEmployeeNames.Tables[0].Rows[i]["EmployeeID"].ToString())); ;
                        //				ddlEmployeeName.DataSource = dsBindEmployeeNames.Tables[0];
                        //				ddlEmployeeName.DataTextField = dsBindEmployeeNames.Tables[0].Columns["EmployeeName"].ToString();
                        //				ddlEmployeeName.DataValueField = dsBindEmployeeNames.Tables[0].Columns["EmployeeID"].ToString();
                        //				ddlEmployeeName.DataBind();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "BindEmployeeNames", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        //		private void BindProblemPriority()
        //		{
        //
        //			clsBLResolutionTimeReport objClsBLResolutionTimeReport = new clsBLResolutionTimeReport();
        //			DataSet dsBindProblemPriority = objClsBLResolutionTimeReport.getProblemPriority();
        //			if(dsBindProblemPriority.Tables[0].Rows.Count > 0)
        //			{
        //				ddlPriority.DataSource = dsBindProblemPriority.Tables[0];
        //				ddlPriority.DataTextField = dsBindProblemPriority.Tables[0].Columns["ProblemPriority"].ToString();
        //				ddlPriority.DataValueField = dsBindProblemPriority.Tables[0].Columns["ProblemPriorityID"].ToString();
        //				ddlPriority.DataBind();
        //			}
        //		}

        private void BindProblemSeverity()
        {
            try
            {
                clsBLResolutionTimeReport objClsBLResolutionTimeReport = new clsBLResolutionTimeReport();
                DataSet dsBindProblemSeverity = objClsBLResolutionTimeReport.getProblemSeverity();
                if (dsBindProblemSeverity.Tables[0].Rows.Count > 0)
                {
                    ddlSeverity.DataSource = dsBindProblemSeverity.Tables[0];
                    ddlSeverity.DataTextField = dsBindProblemSeverity.Tables[0].Columns["ProblemSeverity"].ToString();
                    ddlSeverity.DataValueField = dsBindProblemSeverity.Tables[0].Columns["ProblemSeverityID"].ToString();
                    ddlSeverity.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "BindProblemSeverity", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void getResolutionTimeReport(string ReportType)
        {
            try
            {
                clsBLResolutionTimeReport objClsBLResolutionTimeReport = new clsBLResolutionTimeReport();
                clsResolutionTimeReport objClsResolutionTimeReport = new clsResolutionTimeReport();
                //DataSet dsResolutionTimeReport = new DataSet();
                objClsResolutionTimeReport.EmployeeID = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);
                selectedToMonth = getEndDate(Convert.ToInt32(ddlToMonth.SelectedItem.Value), Convert.ToInt32(ddlToYear.SelectedItem.Value)).ToString();
                selectedFromMonth = Convert.ToDateTime(ddlFromMonth.SelectedItem.Value + "-" + ddlFromYear.SelectedItem.Value).ToString();
                objClsResolutionTimeReport.FromMonth = Convert.ToDateTime(selectedFromMonth);//Convert.ToDateTime(ddlFromMonth.SelectedItem.Value + "-" + ddlFromYear.SelectedItem.Value);
                objClsResolutionTimeReport.ToMonth = Convert.ToDateTime(selectedToMonth);//getEndDate(Convert.ToInt32(ddlToMonth.SelectedItem.Value), Convert.ToInt32(ddlToYear.SelectedItem.Value));
                /*objClsResolutionTimeReport.FromMonth = Convert.ToInt32(ddlFromMonth.SelectedItem.Value);
                objClsResolutionTimeReport.FromYear =  Convert.ToInt32(ddlFromYear.SelectedItem.Value);
                objClsResolutionTimeReport.ToMonth =  Convert.ToInt32(ddlToMonth.SelectedItem.Value);
                objClsResolutionTimeReport.ToYear =  Convert.ToInt32(ddlToYear.SelectedItem.Value);*/
                //						ddlIssueHealth
                IssueHealthId = Convert.ToInt32(ddlIssueHealth.Items[ddlIssueHealth.SelectedIndex].Value);
                ////objClsResolutionTimeReport.PriorityID = Convert.ToInt32(ddlPriority.SelectedItem.Value);
                objClsResolutionTimeReport.SeverityID = Convert.ToInt32(ddlSeverity.SelectedItem.Value);
                int superAdmin = Convert.ToInt32(Session["SAEmployeeID"]);
                dsResolutionTimeReport = objClsBLResolutionTimeReport.getResolutionTimeDetails(objClsResolutionTimeReport, superAdmin);
                if (ReportType == "Graph")
                {
                    Session["Graph"] = objClsResolutionTimeReport;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "getResolutionTimeReport", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private DateTime getEndDate(int monthValue, int YearValue)
        {
            try
            {
                if (monthValue == 1 || monthValue == 3 || monthValue == 5 || monthValue == 7 || monthValue == 8 || monthValue == 10 || monthValue == 12)
                {
                    return Convert.ToDateTime(monthValue + "/31/" + YearValue);
                }
                else if (monthValue == 4 || monthValue == 6 || monthValue == 9 || monthValue == 11)
                {
                    return Convert.ToDateTime(monthValue + "/30/" + YearValue);
                }
                else
                {
                    if (DateTime.IsLeapYear(YearValue))
                    {
                        return Convert.ToDateTime(monthValue + "/29/" + YearValue);
                    }
                    else
                    {
                        return Convert.ToDateTime(monthValue + "/28/" + YearValue);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "getEndDate", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void showResolutionTimeReport()
        {
            try
            {
                getResolutionTimeReport("Tabular");

                clsBLResolutionTimeReport objClsBLResolutionTimeReport = new clsBLResolutionTimeReport();
                clsResolutionTimeReport objClsResolutionTimeReport = new clsResolutionTimeReport();
                if (dsResolutionTimeReport.Tables[0].Rows.Count > 0)
                {
                    lblError.Visible = false;
                    dgReport.Visible = true;
                    lnkbtnGenrateToExcel.Visible = true;
                    //DataView
                    //added
                    dv = new DataView(dsResolutionTimeReport.Tables[0]);
                    if (IssueHealthId > 0)
                    {
                        dv.RowFilter = "ResolutionHealth = " + IssueHealthId;
                    }
                    if (dv.Count > 0)
                    {
                        dgReport.DataSource = dv;
                        dgReport.DataBind();
                        //added
                        //Session["FilteredReport"] = dv;
                        if (dgReport.PageCount > 1)
                        {
                            dgReport.PagerStyle.Visible = true;
                        }
                        else
                        {
                            dgReport.PagerStyle.Visible = false;
                        }
                    }
                    else
                    {
                        lblError.Visible = true;
                        dgReport.Visible = false;
                        lnkbtnGenrateToExcel.Visible = false;
                        lblError.Text = "No Records Found.";
                    }
                }
                else
                {
                    lblError.Visible = true;
                    dgReport.Visible = false;
                    lnkbtnGenrateToExcel.Visible = false;
                    lblError.Text = "No Records Found.";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "showResolutionTimeReport", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgReport_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblResolutionTime = (Label)(e.Item.FindControl("lblResolutionTime"));
                    Label lblIssueHealth = (Label)(e.Item.FindControl("lblIssueHealth"));
                    Label lblResolvedOn = (Label)(e.Item.FindControl("lblResolvedOn"));
                    // Added Tooltip for Issue ID:1516 By Mahesh F
                    if (lblIssueHealth.Text.Trim() == "1")
                    {
                        lblIssueHealth.Text = "Green";
                        lblIssueHealth.ForeColor = Color.Green;
                        lblIssueHealth.ToolTip = "Met SLA";
                        //lblIssueHealth.BackColor = Color.Green;
                    }
                    else if (lblIssueHealth.Text.Trim() == "2")
                    {
                        lblIssueHealth.Text = "Amber";
                        lblIssueHealth.ForeColor = Color.DarkOrange;
                        lblIssueHealth.ToolTip = "Met";
                    }
                    //added by Anushree if resolved date is null
                    else if ((lblIssueHealth.Text.Trim() == "3"))
                    //|| (lblIssueHealth.Text.Trim() == "0"))
                    {
                        lblIssueHealth.Text = "Red";
                        lblIssueHealth.ForeColor = Color.Red;
                        lblIssueHealth.ToolTip = "Not Met";
                    }
                    else if (lblIssueHealth.Text.Trim() == "0")
                    {
                        lblIssueHealth.Text = "";
                    }
                    int totalMintutes = Convert.ToInt32(lblResolutionTime.Text.Trim());
                    int hrs, mins; string displaytime;
                    hrs = totalMintutes / 60;
                    //Label lblPriority = (Label)(e.Item.FindControl("lblPriority"));
                    //Label lblIssueHealth = (Label)(e.Item.FindControl("lblIssueHealth"));
                    //if(hrs > Convert.ToInt32(((Label)e.Item.FindControl("lblGreenResolutionHours")).Text.Trim()))
                    //{
                    //if(hrs < Convert.ToInt32(((Label)e.Item.FindControl("lblAmberResolutionHours")).Text.Trim()))

                    mins = totalMintutes % 60;
                    if (mins < 10)
                    {
                        displaytime = Convert.ToString(hrs) + ":" + "0" + Convert.ToString(mins);
                    }
                    else
                    {
                        displaytime = Convert.ToString(hrs) + ":" + Convert.ToString(mins);
                    }
                    lblResolutionTime.Text = displaytime;
                    //added by Anushree for null Resolved date
                    if (lblResolvedOn.Text.Trim() == "1/1/1900 12:00:00 AM")
                        lblResolvedOn.Text = "";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "dgReport_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                //			Chart1.Visible = false;
                lnkbtnGenrateToExcel.Visible = true;
                dgReport.CurrentPageIndex = 0;
                showResolutionTimeReport();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgReport_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "viewDetails")
                {
                    int IssueID = Convert.ToInt32(dgReport.DataKeys[e.Item.ItemIndex]);
                    Session["IssueID"] = IssueID;
                    Response.Redirect("ViewResolutionTimeReportDetails.aspx");
                }
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "dgReport_ItemCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgReport_PageChange(object sender, DataGridPageChangedEventArgs e)
        {
            try
            {
                dgReport.CurrentPageIndex = e.NewPageIndex;
                showResolutionTimeReport();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "dgReport_PageChange", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void btnGraphicalRepresentation_Click(object sender, System.EventArgs e)
        {
            try
            {
                viewGraph();
                GetGraphDetails();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "btnGraphicalRepresentation_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void viewGraph()
        {
            try
            {
                getResolutionTimeReport("Graph");
                if (dsResolutionTimeReport.Tables[0].Rows.Count > 0)
                {
                    lblError.Visible = false;
                    GetGraphDetails();
                    PlotGraph();
                    //Response.Redirect("GraphicalResolutionTimeReport.aspx");
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "There are no records to be represented Graphically.";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "viewGraph", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void GetGraphDetails()
        {
            try
            {
                clsResolutionTimeReport objClsResolutionTimeReport = new clsResolutionTimeReport();
                objClsResolutionTimeReport = (clsResolutionTimeReport)Session["Graph"];
                clsBLResolutionTimeReport objClsBLResolutionTimeReport = new clsBLResolutionTimeReport();
                dsResolutionTimeReport = objClsBLResolutionTimeReport.getGraphDetails(objClsResolutionTimeReport);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "GetGraphDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void PlotGraph()
        {
            try
            {
                /*ArrayList MonthYear = new ArrayList();
                ArrayList Green = new ArrayList();
                ArrayList Red = new ArrayList();
                ArrayList Amber = new ArrayList();

                 MonthYear.Add(dtViewGraph.Columns["MonthYear"]);
                    Green.Add(dtViewGraph.Columns["GeenIssues"]);
                    Amber.Add(dtViewGraph.Columns["AmberIssues"]);
                    Red.Add(dtViewGraph.Columns["RedIssues"]);
                 string[] Months = new string[] { "Jan", "Feb", "Mar", "April", "May", "June", "July", "August" };
                double[] Red = new double[] { 15, 20, 9,10 };
                double[] Green = new double[] { 20, 15, 8,25 };
                double[] DarkOrange = new double[] { 10, 12, 9, 50 };*/
                DataTable dtViewGraph = dsResolutionTimeReport.Tables[0];
                if (dsResolutionTimeReport.Tables[0].Rows.Count > 1)
                {
                    lblError.Visible = false;
                    /*				Chart1.Visible = true;

                                    Chart1.Series.Clear();
                                    Series series1 = new Series("Red");
                                    Series series2 = new Series("Green");
                                    Series series3 = new Series("Amber");

                                    Chart1.Series.Add(series1);
                                    Chart1.Series.Add(series2);
                                    Chart1.Series.Add(series3);

                                    Chart1.DefineValue("x", "MonthYear");
                                    Chart1.DefineValue("Red:y", "RedIssue");
                                    Chart1.DefineValue("Green:y","GreenIssue");
                                    Chart1.DefineValue("Amber:y", "AmberIssue");

                                    Chart1.MainStyle = "Line";
                                    //Chart1.CompositionKind = CompositionKind.Sections;
                                    Chart1.SeriesStyles["Line"].LineStyleName = "StripLine";

                                    Chart1.DataSource = dtViewGraph;
                                    Chart1.DataBind();
                        */
                }
                else
                {
                    //			Chart1.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "No enough Records to display the Graph.";
                }

                /*//Chart1.DefineValue("Issue Health_Red:y", PrB);
                    series1.Transparency = 10;
                    series2.Transparency = 18;
                    series3.Transparency = 25;

                    //Chart1.MainStyle = "line";
                    Chart1.DataBind();
                    for (int i = 0; i < Chart1.Series.Count; i++)
                    {
                        Series cs = (Series)Chart1.Series[i];
                        for (int j = 0; j < cs.DataPoints.Count; j++)
                        {
                            if ((double)cs.DataPoints[j].Parameters["y"] == 0)
                            {
                                cs.DataPoints[j].Visible = false;
                            }
                        }
                    }*/
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "PlotGraph", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void lnkbtnGenrateToExcel_Click(object sender, System.EventArgs e)
        {
            try
            {
                dgReport.CurrentPageIndex = 0;
                showResolutionTimeReport();
                clsResolutionTimeReport objClsResolutionTimeReport = new clsResolutionTimeReport();
                objClsResolutionTimeReport.EmployeeID = SAEmployeeID;
                strFileNameCust = "Report_" + objClsResolutionTimeReport.EmployeeID + ".xls";
                //			DataGrid dgForCSVFile = new DataGrid();
                //  DataView dv = new DataView();

                //if(dsResolutionTimeReport.Tables[0].Rows.Count>0)
                //	 {

                #region Commented code

                //				dgForCSVFile.AutoGenerateColumns = false;
                //				BoundColumn ReportIssueID = new BoundColumn();
                //				ReportIssueID.HeaderText = "Issue ID";
                //				ReportIssueID.DataField = "ReportIssueID";
                //				dgForCSVFile.Columns.Add(ReportIssueID);
                //
                //				dgForCSVFile.AutoGenerateColumns = false;
                //				BoundColumn ReportedBy = new BoundColumn();
                //				ReportedBy.HeaderText = "Reported By";
                //				ReportedBy.DataField = "ReportedBy";
                //				dgForCSVFile.Columns.Add(ReportedBy);
                //
                //				dgForCSVFile.AutoGenerateColumns = false;
                //				BoundColumn ReportedOn = new BoundColumn();
                //				ReportedOn.HeaderText = "Reported On";
                //				ReportedOn.DataField = "ReportedOn";
                //				dgForCSVFile.Columns.Add(ReportedOn);
                //
                //				dgForCSVFile.AutoGenerateColumns = false;
                //				BoundColumn AssignedTo = new BoundColumn();
                //				AssignedTo.HeaderText = "Issue ID";
                //				AssignedTo.DataField = "AssignedTo";
                //				dgForCSVFile.Columns.Add(AssignedTo);
                //
                //				dgForCSVFile.AutoGenerateColumns = false;
                //				BoundColumn ResolvedOn = new BoundColumn();
                //				ResolvedOn.HeaderText = "Resolved On";
                //				ResolvedOn.DataField = "ResolvedOn";
                //				dgForCSVFile.Columns.Add(ResolvedOn);
                //
                //				dgForCSVFile.AutoGenerateColumns = false;
                //				BoundColumn StatusID = new BoundColumn();
                //				StatusID.HeaderText = "Status ID";
                //				StatusID.DataField = "StatusID";
                //				dgForCSVFile.Columns.Add(StatusID);
                //
                //				dgForCSVFile.AutoGenerateColumns = false;
                //				BoundColumn ResolutionTime = new BoundColumn();
                //				ResolutionTime.HeaderText = "Resolution Time";
                //				ResolutionTime.DataField = "ResolutionTime";
                //				dgForCSVFile.Columns.Add(ResolutionTime);
                //
                //				dgForCSVFile.AutoGenerateColumns = false;
                //				TemplateColumn ResolutionHealth = new TemplateColumn();
                //				Label lblResolutionHealth = new Label();
                //				//BoundColumn ResolutionHealth = new BoundColumn();
                //				ResolutionHealth.HeaderText = "Resolution Health";
                //				lblResolutionHealth.Text = DataBinder.Eval(Container.DataItem,"ResolutionHealth");
                //
                //				dgForCSVFile.Columns.Add(ResolutionHealth);
                //

                #endregion Commented code

                //	Datagrid1.DataSource = dsResolutionTimeReport;//.Tables[0];
                //	Datagrid1.DataBind();
                //	}
                if (dv.Count > 0)
                {
                    Datagrid1.DataSource = dv;//.Tables[0];
                    Datagrid1.DataBind();
                }

                Response.Clear();
                Response.Charset = "";
                Response.AddHeader("content-disposition", "attachment;FileName = " + strFileNameCust);
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter objStringWriter = new StringWriter();
                System.Web.UI.HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                Datagrid1.RenderControl(objHtmlTextWriter);
                Response.Write(objStringWriter.ToString());
                Response.End();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "lnkbtnGenrateToExcel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void dgForCSVFile_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblResolutionHealth = (Label)e.Item.FindControl("lblResolutionHealth");
                    if (lblResolutionHealth.Text == "1")
                    {
                        lblResolutionHealth.Text = "Green";
                    }
                    else if (lblResolutionHealth.Text == "2")
                    {
                        lblResolutionHealth.Text = "Amber";
                    }
                    else if (lblResolutionHealth.Text == "3" || lblResolutionHealth.Text == "0")
                    {
                        lblResolutionHealth.Text = "Red";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "dgForCSVFile_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void Datagrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblResolutionHealth = (Label)e.Item.FindControl("lblResolutionHealth1");
                    Label lblResolutionTime = (Label)e.Item.FindControl("lblResolutionTime1");
                    if (lblResolutionHealth.Text == "1")
                    {
                        lblResolutionHealth.Text = "Green";
                    }
                    else if (lblResolutionHealth.Text == "2")
                    {
                        lblResolutionHealth.Text = "Amber";
                    }
                    else if (lblResolutionHealth.Text == "3" || lblResolutionHealth.Text == "0")
                    {
                        lblResolutionHealth.Text = "Red";
                    }

                    int totalMintutes = Convert.ToInt32(lblResolutionTime.Text.Trim());
                    int hrs, mins; string displaytime;
                    hrs = totalMintutes / 60;
                    //Label lblPriority = (Label)(e.Item.FindControl("lblPriority"));
                    //Label lblIssueHealth = (Label)(e.Item.FindControl("lblIssueHealth"));
                    //if(hrs > Convert.ToInt32(((Label)e.Item.FindControl("lblGreenResolutionHours")).Text.Trim()))
                    //{
                    //if(hrs < Convert.ToInt32(((Label)e.Item.FindControl("lblAmberResolutionHours")).Text.Trim()))

                    mins = totalMintutes % 60;
                    if (mins < 10)
                    {
                        displaytime = Convert.ToString(hrs) + ":" + "0" + Convert.ToString(mins);
                    }
                    else
                    {
                        displaytime = Convert.ToString(hrs) + ":" + Convert.ToString(mins);
                    }
                    lblResolutionTime.Text = displaytime;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeReport.aspx", "Datagrid1_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}