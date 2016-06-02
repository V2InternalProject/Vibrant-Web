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
    public partial class SLAReport : System.Web.UI.Page
    {
        #region Declare Variables

        protected System.Web.UI.WebControls.Button btnSubmit;
        protected System.Web.UI.WebControls.DropDownList ddlDepartment;
        protected System.Web.UI.WebControls.TextBox txtFromDate;
        protected System.Web.UI.WebControls.ImageButton ibtnFromDateCalendar;
        protected System.Web.UI.WebControls.TextBox txtToDate;
        protected System.Web.UI.WebControls.ImageButton ibtnFromToCalendar;
        protected System.Web.UI.WebControls.GridView dgSLAReport;
        protected System.Web.UI.WebControls.Label lblMsg;
        protected System.Web.UI.WebControls.LinkButton lnkbtnPrint;
        protected System.Web.UI.WebControls.LinkButton lnkbtnSendToExcel;
        protected System.Web.UI.WebControls.Label lblSeperator;
        private clsSLAReport objSLAReport = new clsSLAReport();
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvTodate;
        protected System.Web.UI.WebControls.ValidationSummary SLAValidationSummary1;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvfromDate;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvDept;
        protected System.Web.UI.WebControls.CompareValidator CmpDate;
        private clsBLSLAReport objBLSLAReport = new clsBLSLAReport();
        private static DataSet dsSLAReport;
        private int EmployeeID, SAEmployeeID, OnlySuperAdmin;

        #endregion Declare Variables

        #region Page Load

        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                OnlySuperAdmin = Convert.ToInt32(Session["OnlySuperAdmin"]);
                if (OnlySuperAdmin != 0)
                {
                    Response.Redirect("AuthorizationErrorMessage.aspx");
                }
                //if(EmployeeID.ToString() == "" || EmployeeID == 0)
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
                if (!IsPostBack)
                {
                    DepartmentDetails();
                    lnkbtnPrint.Visible = false;
                    lnkbtnSendToExcel.Visible = false;
                    lblSeperator.Visible = false;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SLAReport.aspx", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Page Load

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
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.lnkbtnPrint.Click += new System.EventHandler(this.lnkbtnPrint_Click);
            this.lnkbtnSendToExcel.Click += new System.EventHandler(this.lnkbtnSendToExcel_Click);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        #endregion Web Form Designer generated code

        #region DepartmentDetails

        public void DepartmentDetails()
        {
            try
            {
                ddlDepartment.Items.Clear();
                DataSet dsDepartment = new DataSet();
                objSLAReport.EmployeeID = Convert.ToInt32(SAEmployeeID.ToString());
                //clsBLSubCategory objClsBLSubCategory = new clsBLSubCategory();
                //clsCategoryWiseSearchReport objCategorywiseSearchReport = new clsCategoryWiseSearchReport();
                //objCategorywiseSearchReport.EmployeeName = SAEmployeeID.ToString();
                //DataSet dsCategoryID = objClsBLSubCategory.getAllCategoryID(objCategorywiseSearchReport);

                dsDepartment = objBLSLAReport.DepartmentDeatils(objSLAReport);
                if (dsDepartment.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsDepartment.Tables[0].Rows.Count; i++)
                    {
                        ddlDepartment.Items.Add(new ListItem(dsDepartment.Tables[0].Rows[i]["Category"].ToString(), dsDepartment.Tables[0].Rows[i]["CategoryID"].ToString()));
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SLAReport.aspx", "DepartmentDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion DepartmentDetails

        #region DeptSLADetails

        public void DeptSummaryDetails()
        {
            try
            {
                //DataSet dsSLAReport = new DataSet();
                objSLAReport.CategoryID = Convert.ToInt32(ddlDepartment.SelectedValue);

                objSLAReport.ReportIssueDate = Convert.ToDateTime(txtFromDate.Text.ToString());
                objSLAReport.ReportCloseDate = Convert.ToDateTime(txtToDate.Text.ToString());
                dsSLAReport = objBLSLAReport.DeptSLADetails(objSLAReport);
                if (dsSLAReport.Tables[0].Rows.Count < 1)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "No Records Found";
                    dgSLAReport.DataSource = null;
                    dgSLAReport.DataBind();
                    lnkbtnPrint.Visible = false;
                    lnkbtnSendToExcel.Visible = false;
                    lblSeperator.Visible = false;
                }
                else
                {
                    lblMsg.Visible = false;
                    dgSLAReport.DataSource = dsSLAReport;
                    dgSLAReport.DataBind();
                    lnkbtnPrint.Visible = true;
                    lnkbtnSendToExcel.Visible = true;
                    lblSeperator.Visible = true;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SLAReport.aspx", "DeptSummaryDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion DeptSLADetails

        #region Display the Grid Details

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                DeptSummaryDetails();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SLAReport.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Display the Grid Details

        protected void dgSLAReport_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgSLAReport.PageIndex = e.NewPageIndex;
                DeptSummaryDetails();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SLAReport.aspx", "dgSLAReport_PageIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void lnkbtnPrint_Click(object sender, System.EventArgs e)
        {
            try
            {
                Session["dsResults"] = dsSLAReport;
                string Heading = "SLA Report";
                Session["Heading"] = Heading;
                string strJScript = "<Script Language='javascript'>";
                strJScript += "popup('printPage.aspx')";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SLAReport.aspx", "lnkbtnPrint_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void lnkbtnSendToExcel_Click(object sender, System.EventArgs e)
        {
            try
            {
                string strFileNameCust = "";
                strFileNameCust = "SLAReport.xls";
                //the string strFileName initializing ends here.

                DataGrid dgForCSVFile = new DataGrid();
                if (dsSLAReport.Tables[0].Rows.Count > 0)
                {
                    dgForCSVFile.AutoGenerateColumns = false;

                    BoundColumn ReportIssueID = new BoundColumn();
                    ReportIssueID.HeaderText = "Issue ID";
                    ReportIssueID.DataField = "IssueID";
                    dgForCSVFile.Columns.Add(ReportIssueID);

                    BoundColumn Name = new BoundColumn();
                    Name.HeaderText = "HelpDeskName";
                    Name.DataField = "HelpDeskName";
                    dgForCSVFile.Columns.Add(Name);

                    BoundColumn Category = new BoundColumn();
                    Category.HeaderText = "Category";
                    Category.DataField = "Category";
                    dgForCSVFile.Columns.Add(Category);

                    BoundColumn Requester = new BoundColumn();
                    Requester.HeaderText = "Requester";
                    Requester.DataField = "Requester";
                    dgForCSVFile.Columns.Add(Requester);

                    BoundColumn Department = new BoundColumn();
                    Department.HeaderText = "Department";
                    Department.DataField = "Department";
                    dgForCSVFile.Columns.Add(Department);

                    BoundColumn Status = new BoundColumn();
                    Status.HeaderText = "Status";
                    Status.DataField = "Status";
                    dgForCSVFile.Columns.Add(Status);

                    BoundColumn Type = new BoundColumn();
                    Type.HeaderText = "Type";
                    Type.DataField = "Type";
                    dgForCSVFile.Columns.Add(Type);

                    BoundColumn ProblemSeverity = new BoundColumn();
                    ProblemSeverity.HeaderText = "ProblemSeverity";
                    ProblemSeverity.DataField = "ProblemSeverity";
                    dgForCSVFile.Columns.Add(ProblemSeverity);

                    BoundColumn AssignedTo = new BoundColumn();
                    AssignedTo.HeaderText = "AssignedTo";
                    AssignedTo.DataField = "AssignedTo";
                    dgForCSVFile.Columns.Add(AssignedTo);

                    BoundColumn SubmittedDate = new BoundColumn();
                    SubmittedDate.HeaderText = "SubmittedDate";
                    SubmittedDate.DataField = "SubmittedDate";
                    dgForCSVFile.Columns.Add(SubmittedDate);

                    BoundColumn ResolvedOn = new BoundColumn();
                    ResolvedOn.HeaderText = "ResolvedOn";
                    ResolvedOn.DataField = "ResolvedOn";
                    dgForCSVFile.Columns.Add(ResolvedOn);

                    BoundColumn ResolutionHealth = new BoundColumn();
                    ResolutionHealth.HeaderText = "ResolutionHealth";
                    ResolutionHealth.DataField = "ResolutionHealth";
                    dgForCSVFile.Columns.Add(ResolutionHealth);

                    BoundColumn Month = new BoundColumn();
                    Month.HeaderText = "Month";
                    Month.DataField = "Month";
                    dgForCSVFile.Columns.Add(Month);

                    BoundColumn Year = new BoundColumn();
                    Year.HeaderText = "Year";
                    Year.DataField = "Year";
                    dgForCSVFile.Columns.Add(Year);

                    dgForCSVFile.DataSource = dsSLAReport;//.Tables[0];
                    dgForCSVFile.DataBind();
                    Response.Clear();
                    //				Response.Charset = "";
                    Response.AddHeader("content-disposition", "attachment;FileName = " + strFileNameCust);
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter objStringWriter = new StringWriter();
                    System.Web.UI.HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                    //dgForCSVFile.DataBind();
                    dgForCSVFile.RenderControl(objHtmlTextWriter);
                    Response.Write(objStringWriter.ToString());
                    Response.End();
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "";
                    lblError.Text = "No Records to generate a excel file";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SLAReport.aspx", "lnkbtnSendToExcel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}