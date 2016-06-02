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

namespace V2.Helpdesk.web
{
    /// <summary>
    /// Summary description for CategoryWiseSearchReport.
    /// </summary>
    public partial class CategoryWiseSearchReport : System.Web.UI.Page
    {
        #region Variable declaration

        private static DataSet dsGetReport;
        private DataSet dsYear;
        private clsBLCategoryWiseSearchReport objClsBLCategoryWiseSearchReport = new clsBLCategoryWiseSearchReport();
        public int EmployeeID, SAEmployeeID, OnlySuperAdmin;

        #endregion Variable declaration

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Put user code to initialize the page here
                //code to clear all the cache so that after logout, previous page cannot be revisited.
                //--- starts here---------

                //--- ends here---------
                //			if(ddlCategory.SelectedIndex==0)
                //			{
                //				btnSubmit.Attributes.Add("onclick","return validateCategory();");
                //			}
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
                if (!IsPostBack)
                {
                    BindActiveCategory();
                    //BindActiveSubCategory();
                    BindStatus();
                    BindYear();
                    lnkbtnPrint.Visible = false;
                    lnkbtnSendToExcel.Visible = false;
                    lblSeperator.Visible = false;
                }
                btnSubmit.Attributes.Add("onClick", "javascript:if(validateCategory() != false){ return CheckDate();}else{return false;}");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CategoryWiseSearchReport.aspx", "Page_Load", ex.StackTrace);
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

        private void BindYear()
        {
            dsYear = objClsBLCategoryWiseSearchReport.getYears();

            ddlYears.Items.Clear();
            for (int i = 0; i < dsYear.Tables[0].Rows.Count; i++)
            {
                ddlYears.Items.Add(new ListItem(dsYear.Tables[0].Rows[i]["Year"].ToString()));
            }
        }

        private void BindStatus()
        {
            try
            {
                clsMemberWiseSearchReport objClsMemberWiseSearchReport = new clsMemberWiseSearchReport();
                clsBLMemberWiseSearchReport objClsBLMemberWiseSearchReport = new clsBLMemberWiseSearchReport();

                objClsMemberWiseSearchReport = new clsMemberWiseSearchReport();
                objClsBLMemberWiseSearchReport = new clsBLMemberWiseSearchReport();
                //objViewMyIssues = new Model.clsViewMyIssues();
                //objBLViewMyIssues = new BusinessLayer.clsBLViewMyIssues();
                DataSet dsStatus = new DataSet();
                dsStatus = objClsBLMemberWiseSearchReport.GetStatus();
                // ddlStatus.Items.Add(new ListItem(dsStatus.Tables[0].Rows[i]["GroupName"].ToString(), dsStatus.Tables[0].Rows[i]["GroupID"].ToString()))

                for (int i = 0; i < dsStatus.Tables[0].Rows.Count; i++)
                {
                    ddlStatus.Items.Add(new ListItem(dsStatus.Tables[0].Rows[i]["StatusDesc"].ToString(), dsStatus.Tables[0].Rows[i]["StatusID"].ToString()));
                }
                ddlStatus.Items.Insert(0, "All");

                //ddlStatus.DataSource = Status.BindStatusEnum(typeof(IssueStatus),"All");
                //ddlStatus.DataTextField="Key";
                //ddlStatus.DataValueField="Value";
                //ddlStatus.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CategoryWiseSearchReport.aspx", "BindStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        //private void BindActiveSubCategory()
        //{
        //    try
        //    {
        //        clsBLSubCategory objclsBLSubCategory = new clsBLSubCategory();

        //        DataSet dsSubCategory = new DataSet();
        //        dsSubCategory = objclsBLSubCategory.getSubCategories();

        //        for (int i = 0; i < dsSubCategory.Tables[0].Rows.Count; i++)
        //        {
        //            ddlSubCategory.Items.Add(new ListItem(dsSubCategory.Tables[0].Rows[i]["SubCategory"].ToString(), dsSubCategory.Tables[0].Rows[i]["SubCategoryID"].ToString()));
        //        }
        //        ddlSubCategory.Items.Insert(0, "All");

        //    }
        //    catch (V2Exceptions ex)
        //    {
        //        throw;

        //    }
        //    catch (System.Exception ex)
        //    {
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "CategoryWiseSearchReport.aspx", "BindActiveSubCategory", ex.StackTrace);
        //        throw new V2Exceptions(ex.ToString(),ex);
        //    }
        //}
        private void BindActiveCategory()
        {
            try
            {
                ddlCategory.Items.Add(new ListItem("Select Department", "0"));
                clsBLSubCategory objClsBLSubCategory = new clsBLSubCategory();
                clsCategoryWiseSearchReport objCategorywiseSearchReport = new clsCategoryWiseSearchReport();
                objCategorywiseSearchReport.EmployeeName = SAEmployeeID.ToString();
                DataSet dsCategoryID = objClsBLSubCategory.getAllCategoryID(objCategorywiseSearchReport);
                for (int i = 0; i < dsCategoryID.Tables[0].Rows.Count; i++)
                {
                    if (dsCategoryID.Tables[0].Rows.Count > 0)
                    {
                        ddlCategory.Items.Add(new ListItem(dsCategoryID.Tables[0].Rows[i]["Category"].ToString(), dsCategoryID.Tables[0].Rows[i]["CategoryID"].ToString())); ;
                        //				ddlCategory.DataSource = dsCategoryID.Tables[0];
                        //				ddlCategory.DataValueField = dsCategoryID.Tables[0].Columns["CategoryID"].ToString();
                        //				ddlCategory.DataTextField = dsCategoryID.Tables[0].Columns["Category"].ToString();
                        //				ddlCategory.DataBind();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CategoryWiseSearchReport.aspx", "BindActiveCategory", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                dgReport.CurrentPageIndex = 0;
                GetCategorywiseReport();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CategoryWiseSearchReport.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void GetCategorywiseReport()
        {
            int StatusID = 0;
            int SubCategoryID = 0;
            try
            {
                clsCategoryWiseSearchReport objCategorywiseSearchReport = new clsCategoryWiseSearchReport();
                clsBLCategoryWiseSearchReport objBLCategorywiseSearchReport = new clsBLCategoryWiseSearchReport();
                objCategorywiseSearchReport.EmployeeID = SAEmployeeID;
                clsIssueStatus objIssueStatus = new clsIssueStatus();
                int CategoryID = Convert.ToInt32(ddlCategory.SelectedItem.Value);
                objCategorywiseSearchReport.CategoryID = CategoryID;
                //			int StatusID = Convert.ToInt32(ddlStatus.SelectedItem.Value);
                if (ddlStatus.SelectedItem.Value != "All")
                {
                    StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    objCategorywiseSearchReport.StatusID = StatusID;
                }
                else
                {
                    objCategorywiseSearchReport.StatusID = -1;
                }
                if (ddlSubCategory.SelectedItem.Value != "All")
                {
                    SubCategoryID = Convert.ToInt32(ddlSubCategory.SelectedValue);
                    objCategorywiseSearchReport.SubCategoryID = SubCategoryID;
                }
                else
                {
                    objCategorywiseSearchReport.SubCategoryID = -1;
                }
                objCategorywiseSearchReport.Period = ddlPeriod.SelectedItem.Value;
                if (ddlPeriod.SelectedItem.Value.ToLower() == "day")
                {
                    objCategorywiseSearchReport.Date = txtDay.Text.Trim();
                    objCategorywiseSearchReport.Month = "";
                    objCategorywiseSearchReport.Year = "";
                    objCategorywiseSearchReport.FromDate = "";
                    objCategorywiseSearchReport.ToDate = "";
                }
                else if (ddlPeriod.SelectedItem.Value.ToLower() == "week")
                {
                    objCategorywiseSearchReport.Date = txtDay.Text.Trim();
                    objCategorywiseSearchReport.Month = "";
                    objCategorywiseSearchReport.Year = "";
                    objCategorywiseSearchReport.FromDate = "";
                    objCategorywiseSearchReport.ToDate = "";
                }
                else if (ddlPeriod.SelectedItem.Value.ToLower() == "month")
                {
                    objCategorywiseSearchReport.Date = "";
                    objCategorywiseSearchReport.Month = ddlMonths.SelectedValue;
                    objCategorywiseSearchReport.Year = ddlYears.SelectedValue;
                    objCategorywiseSearchReport.FromDate = "";
                    objCategorywiseSearchReport.ToDate = "";
                }
                else if (ddlPeriod.SelectedItem.Value.ToLower() == "year")
                {
                    objCategorywiseSearchReport.Date = "";
                    objCategorywiseSearchReport.Month = "";
                    objCategorywiseSearchReport.Year = ddlYears.SelectedValue;
                    objCategorywiseSearchReport.FromDate = "";
                    objCategorywiseSearchReport.ToDate = "";
                }
                else if (ddlPeriod.SelectedItem.Value.ToLower() == "range")
                {
                    objCategorywiseSearchReport.Date = "";
                    objCategorywiseSearchReport.Month = "";
                    objCategorywiseSearchReport.Year = "";
                    objCategorywiseSearchReport.FromDate = txtFromDate.Text.Trim();
                    objCategorywiseSearchReport.ToDate = txtToDate.Text.Trim();
                }
                dsGetReport = new DataSet();
                dsGetReport = objBLCategorywiseSearchReport.GetCategoryWiseReport(objCategorywiseSearchReport);
                if (dsGetReport.Tables[0].Rows.Count > 0)
                {
                    dgReport.DataSource = dsGetReport.Tables[0];
                    dgReport.DataBind();
                    if (StatusID != -1)
                    {
                        //dgReport.Columns[8].Visible = false;
                    }
                    else
                    {
                        //dgReport.Columns[8].Visible = true;
                    }
                    if (CategoryID != 0)
                    {
                        dgReport.Columns[4].Visible = false;
                    }
                    else
                    {
                        dgReport.Columns[4].Visible = true;
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
                    lnkbtnPrint.Visible = true;
                    lnkbtnSendToExcel.Visible = true;
                    lblSeperator.Visible = true;
                }
                else
                {
                    lnkbtnPrint.Visible = false;
                    lnkbtnSendToExcel.Visible = false;
                    lblSeperator.Visible = false;
                    dgReport.Visible = false;
                    lblError.Visible = true;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CategoryWiseSearchReport.aspx", "GetCategorywiseReport", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgReport_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetails")
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CategoryWiseSearchReport.aspx", "dgReport_ItemCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void lnkbtnPrint_Click(object sender, System.EventArgs e)
        {
            try
            {
                Session["dsResults"] = dsGetReport;
                string Heading = "Department wise report";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CategoryWiseSearchReport.aspx", "lnkbtnPrint_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void lnkbtnSendToExcel_Click(object sender, System.EventArgs e)
        {
            try
            {
                string strFileNameCust = "";
                strFileNameCust = "CategoryWiseIssueReport.xls";
                //the string strFileName initializing ends here.

                DataGrid dgForCSVFile = new DataGrid();
                if (dsGetReport.Tables[0].Rows.Count > 0)
                {
                    dgForCSVFile.AutoGenerateColumns = false;
                    BoundColumn ReportIssueID = new BoundColumn();
                    ReportIssueID.HeaderText = "Issue ID";
                    ReportIssueID.DataField = "ReportIssueID";
                    dgForCSVFile.Columns.Add(ReportIssueID);

                    BoundColumn Name = new BoundColumn();
                    Name.HeaderText = "Reported By";
                    Name.DataField = "Name";
                    dgForCSVFile.Columns.Add(Name);

                    BoundColumn ReportIssueDate = new BoundColumn();
                    ReportIssueDate.HeaderText = "ReportIssueDate";
                    ReportIssueDate.DataField = "ReportIssueDate";
                    dgForCSVFile.Columns.Add(ReportIssueDate);

                    BoundColumn ProblemSeverity = new BoundColumn();
                    ProblemSeverity.HeaderText = "ProblemSeverity";
                    ProblemSeverity.DataField = "ProblemSeverity";
                    dgForCSVFile.Columns.Add(ProblemSeverity);

                    BoundColumn Category = new BoundColumn();
                    Category.HeaderText = "Category";
                    Category.DataField = "Category";
                    dgForCSVFile.Columns.Add(Category);

                    BoundColumn SubCategory = new BoundColumn();
                    SubCategory.HeaderText = "SubCategory";
                    SubCategory.DataField = "SubCategory";
                    dgForCSVFile.Columns.Add(SubCategory);

                    BoundColumn EmployeeName = new BoundColumn();
                    EmployeeName.HeaderText = "Assigned To";
                    EmployeeName.DataField = "EmployeeName";
                    dgForCSVFile.Columns.Add(EmployeeName);

                    BoundColumn Status = new BoundColumn();
                    Status.HeaderText = "Status";
                    Status.DataField = "Status";
                    dgForCSVFile.Columns.Add(Status);

                    dgForCSVFile.DataSource = dsGetReport;//.Tables[0];
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CategoryWiseSearchReport.aspx", "lnkbtnSendToExcel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgReport_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
        {
            try
            {
                dgReport.CurrentPageIndex = e.NewPageIndex;
                GetCategorywiseReport();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CategoryWiseSearchReport.aspx", "dgReport_PageIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgReport_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            //			foreach(DataGridItem dgi in dgReport.Items)
            //			{
            //				if(dgi.ItemType == ListItemType.Item || dgi.ItemType == ListItemType.AlternatingItem)
            //				{
            //					int intStatusID = Convert.ToInt32(((Label)dgi.FindControl("lblStatusID")).Text);
            //					Label lblStatusName = (Label)dgi.FindControl("lblStatusName");
            //
            //					if(intStatusID == 1)
            //					{
            //						lblStatusName.Text =IssueStatus.New.ToString ();
            //					}
            //					else if (intStatusID == 2)
            //					{
            //						lblStatusName.Text = IssueStatus.Resolved.ToString ();
            //
            //					}
            //					else if (intStatusID == 3)
            //					{
            //						lblStatusName.Text = IssueStatus.Moved.ToString ();
            //
            //					}
            //					else if (intStatusID == 4)
            //					{
            //						lblStatusName.Text = IssueStatus.Reopen.ToString ();
            //
            //					}
            //				}
            //			}
        }

        //protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //}

        protected void ddlCategory_SelectedIndexChanged1(object sender, EventArgs e)
        {
            DataSet dsGetSubCategory = new DataSet();
            clsBLSubCategory objclsBLSubCategory = new clsBLSubCategory();
            ddlSubCategory.Items.Clear();
            if (ddlCategory.SelectedIndex == 0)
            {
                lblError.Text = "Please select Category."; return;
            }
            int category = Convert.ToInt32(ddlCategory.SelectedItem.Value);
            dsGetSubCategory = objclsBLSubCategory.GetSubCategory(category);

            for (int i = 0; i < dsGetSubCategory.Tables[0].Rows.Count; i++)
            {
                ddlSubCategory.Items.Add(new ListItem(dsGetSubCategory.Tables[0].Rows[i]["SubCategory"].ToString(), dsGetSubCategory.Tables[0].Rows[i]["SubCategoryID"].ToString()));
            }
        }
    }
}