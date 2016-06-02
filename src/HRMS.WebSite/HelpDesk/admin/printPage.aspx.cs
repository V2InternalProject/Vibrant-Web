using System;
using System.Data;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for printPage.
    /// </summary>
    public partial class printPage : System.Web.UI.Page
    {
        public int ReportIssueID;

        //static int IssueID, SubCategoryID;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Literal1.Text = "";
                Literal2.Text = "";
                // Put user code to initialize the page here
                getName();
                getData();
                printThisPage();
                //closeWindow();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "printPage.aspx", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
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

        public void getName()
        {
            try
            {
                lblSubHeading.CssClass = "header";
                lblSubHeading.Text = Session["Heading"].ToString();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "printPage.aspx", "getName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void getData()
        {
            try
            {
                DataSet dsPrint = (DataSet)Session["dsResults"];
                dgPrint.AutoGenerateColumns = false;
                if (dsPrint.Tables[0].Rows.Count > 0)
                {
                    if (Session["Heading"].ToString() == "SLA Report")
                    {
                        BoundColumn ReportIssueID = new BoundColumn();
                        ReportIssueID.HeaderText = "Issue ID";
                        ReportIssueID.DataField = "IssueID";
                        dgPrint.Columns.Add(ReportIssueID);

                        BoundColumn Name = new BoundColumn();
                        Name.HeaderText = "HelpDeskName";
                        Name.DataField = "HelpDeskName";
                        dgPrint.Columns.Add(Name);

                        BoundColumn Category = new BoundColumn();
                        Category.HeaderText = "Category";
                        Category.DataField = "Category";
                        dgPrint.Columns.Add(Category);

                        BoundColumn Requester = new BoundColumn();
                        Requester.HeaderText = "Requester";
                        Requester.DataField = "Requester";
                        dgPrint.Columns.Add(Requester);

                        BoundColumn Department = new BoundColumn();
                        Department.HeaderText = "Department";
                        Department.DataField = "Department";
                        dgPrint.Columns.Add(Department);

                        BoundColumn Status = new BoundColumn();
                        Status.HeaderText = "Status";
                        Status.DataField = "Status";
                        dgPrint.Columns.Add(Status);

                        BoundColumn Type = new BoundColumn();
                        Type.HeaderText = "Type";
                        Type.DataField = "Type";
                        dgPrint.Columns.Add(Type);

                        BoundColumn ProblemSeverity = new BoundColumn();
                        ProblemSeverity.HeaderText = "ProblemSeverity";
                        ProblemSeverity.DataField = "ProblemSeverity";
                        dgPrint.Columns.Add(ProblemSeverity);

                        BoundColumn AssignedTo = new BoundColumn();
                        AssignedTo.HeaderText = "AssignedTo";
                        AssignedTo.DataField = "AssignedTo";
                        dgPrint.Columns.Add(AssignedTo);

                        BoundColumn SubmittedDate = new BoundColumn();
                        SubmittedDate.HeaderText = "SubmittedDate";
                        SubmittedDate.DataField = "SubmittedDate";
                        dgPrint.Columns.Add(SubmittedDate);

                        BoundColumn ResolvedOn = new BoundColumn();
                        ResolvedOn.HeaderText = "ResolvedOn";
                        ResolvedOn.DataField = "ResolvedOn";
                        dgPrint.Columns.Add(ResolvedOn);

                        BoundColumn ResolutionHealth = new BoundColumn();
                        ResolutionHealth.HeaderText = "ResolutionHealth";
                        ResolutionHealth.DataField = "ResolutionHealth";
                        dgPrint.Columns.Add(ResolutionHealth);

                        BoundColumn Month = new BoundColumn();
                        Month.HeaderText = "Month";
                        Month.DataField = "Month";
                        dgPrint.Columns.Add(Month);

                        BoundColumn Year = new BoundColumn();
                        Year.HeaderText = "Year";
                        Year.DataField = "Year";
                        dgPrint.Columns.Add(Year);
                    }
                    else
                    {
                        dgPrint.AutoGenerateColumns = false;
                        BoundColumn IssueID = new BoundColumn();
                        IssueID.HeaderText = "Issue ID";
                        IssueID.DataField = "ReportIssueID";
                        dgPrint.Columns.Add(IssueID);

                        BoundColumn ReportedBy = new BoundColumn();
                        ReportedBy.HeaderText = "Report By";
                        ReportedBy.DataField = "Name";
                        dgPrint.Columns.Add(ReportedBy);

                        BoundColumn ReportedOn = new BoundColumn();
                        ReportedOn.HeaderText = "Reported On";
                        ReportedOn.DataField = "ReportIssueDate";
                        dgPrint.Columns.Add(ReportedOn);

                        BoundColumn Severity = new BoundColumn();
                        Severity.HeaderText = "Severity";
                        Severity.DataField = "ProblemSeverity";
                        dgPrint.Columns.Add(Severity);

                        if (Session["Heading"].ToString() == "Issue Status wise report by each Members")
                        {
                            BoundColumn SubCategory = new BoundColumn();
                            SubCategory.HeaderText = "Category";
                            SubCategory.DataField = "SubCategoryID";
                            dgPrint.Columns.Add(SubCategory);
                        }

                        BoundColumn AssignedTo = new BoundColumn();
                        AssignedTo.HeaderText = "Assigned To";
                        AssignedTo.DataField = "EmployeeName";
                        dgPrint.Columns.Add(AssignedTo);

                        BoundColumn Status = new BoundColumn();
                        Status.HeaderText = "Status";
                        Status.DataField = "Status";
                        dgPrint.Columns.Add(Status);
                    }
                    dgPrint.DataSource = dsPrint;
                    dgPrint.DataBind();
                    if (dgPrint.PageCount > 1)
                    {
                        dgPrint.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgPrint.PagerStyle.Visible = false;
                    }

                    Controls.Add(dgPrint);
                    dgPrint.Visible = true;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "printPage.aspx", "getData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void printThisPage()
        {
            try
            {
                string strJScript = "<Script Language='javascript'>";
                strJScript += "window.opener = self;";
                strJScript += "window.print();";
                strJScript += " setTimeout('self.close()',1000);";
                strJScript += "</script>";
                Literal Literal1 = new Literal();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "printPage.aspx", "printThisPage", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void closeWindow()
        {
            try
            {
                string strJScript = "<Script Language='javascript'>";
                strJScript += "window.opener = self;";
                strJScript += "window.close();";
                strJScript += "</Script>";
                Literal Litereal2 = new Literal();
                Controls.Add(Litereal2);
                Litereal2.Text = strJScript;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "printPage.aspx", "closeWindow", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}