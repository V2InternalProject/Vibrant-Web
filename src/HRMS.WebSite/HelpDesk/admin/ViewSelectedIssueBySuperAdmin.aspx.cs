using HRMS.DAL;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;
using V2.Helpdesk.Model;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for ViewSelectedIssueBySuperAdmin.
    /// </summary>
    public partial class ViewSelectedIssueBySuperAdmin : System.Web.UI.Page
    {
        #region Variable declaration

        private Model.clsIssueAssignment objIssueAssignment;
        private Model.clsSubCategoryAssignment objSubCategoryAssignment;
        private Model.clsViewMyIssues objViewIssue;
        private BusinessLayer.clsBLIssueAssignment objBLIssueAssignment;

        //		BusinessLayer.clsBLStatus objBLStatus;
        private BusinessLayer.clsBLSubCategoryAssignment objBLSubCategoryAssignment;

        private BusinessLayer.clsBLViewMyIssues objBLViewIssue;
        private clsMemberWiseSearchReport objClsMemberWiseSearchReport = new clsMemberWiseSearchReport();
        private clsBLMemberWiseSearchReport objClsBLMemberWiseSearchReport = new clsBLMemberWiseSearchReport();

        public DataSet dsStatusList, dsLoginUserList, dsSelectedIssue, dsSuperAdminReportIssueHistory, dsDepartment;
        public int EmployeeID, SAEmployeeID, intReportIssueID, intIssueAssignmentID;
        private Boolean IsRecordUpdated, IsRecordInserted, IsCategoryChanged;
        protected System.Web.UI.WebControls.Label lblFileName;
        private static int PrevEmployeeID, intSubCategoryID;
        public static int CategoryChangeFlag = 0;
        public string strFromEmailID, strToEmailID, strEmployeeEmailID, strDeptCCEmailID;
        public string strMailSubForUser, strEmailBodyForMember, strMailSubForITDept, strEMailBodyForITDept;
        protected System.Web.UI.WebControls.LinkButton lnkbtnFileName;

        //  protected System.Web.UI.WebControls.LinkButton lnkFileName;
        protected System.Web.UI.WebControls.RangeValidator RangeValidatorEndDate;

        protected System.Web.UI.WebControls.RangeValidator RangeValidatorFromDate;
        private int Flag = 0;
        public string Category = string.Empty;
        private static DataSet dsfileName;
        private static LinkButton lnkFileName;

        #endregion Variable declaration

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Put user code to initialize the page here
                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                intReportIssueID = Convert.ToInt32(Session["ReportIssueID"]);
                //intIssueAssignmentID = Convert.ToInt32(Session["IssueAssignmentID"]);
                //lnkbtnFileName.Text="Introduction.doc";
                intIssueAssignmentID = intReportIssueID;
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtEndDate.Attributes.Add("readonly", "readonly");
                clsIssueAssignment objIssueAssignment = new clsIssueAssignment();
                clsBLIssueAssignment objBLIssueAssignment = new clsBLIssueAssignment();
                pnlMessage.Visible = false;

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
                    lblCheckHistory.Visible = false;
                    dgIssueDetails.Visible = false;
                    BindType();
                    //Modified by Mahesh F For Issue ID:22449
                    BindProblemSeverity();
                    GetSelectedIssue();

                    GetSuperAdminReportIssueHistory();
                    //objIssueAssignment.ReportIssueID = intReportIssueID;
                    //dsfileName = objBLIssueAssignment.FileName(objIssueAssignment);
                    //if (dsfileName.Tables[0].Rows.Count > 0)
                    //{
                    //    for (int i = 0, j = 0; i < dsfileName.Tables[0].Rows.Count; i++, j++)
                    //    {
                    //        //LinkButton lnk1 = new LinkButton();
                    //        lnkFileName = new LinkButton();
                    //        lnkFileName.ID = "lnk2FileName";
                    //        lnkFileName.Attributes.Add("runat", "server");
                    //        //this.lnk2.Click += new System.EventHandler(this.lnk2_Click);
                    //        Session["FileNames"] = dsfileName.Tables[0].Rows[j]["FileName"].ToString();
                    //        lnkFileName.Text = dsfileName.Tables[0].Rows[j]["FileName"].ToString();
                    //        //lnkFileName.Text = "</br>";
                    //        lnkFileName.Click += new System.EventHandler(this.lnkFileName_Click);
                    //        pnlFileName.Controls.Add(lnkFileName);
                    //        pnlFileName.Controls.Add(new LiteralControl("<br/>"));

                    //        // Add control to container:
                    //        //pnlFileName.Controls.Add(lnk1);
                    //        //lblFileName.Text = dsfileName.Tables[0].Rows[0]["FileName"].ToString();
                    //    }
                    //}
                }
                objIssueAssignment.ReportIssueID = intReportIssueID;
                dsfileName = objBLIssueAssignment.FileName(objIssueAssignment);
                if (dsfileName.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0, j = 0; i < dsfileName.Tables[0].Rows.Count; i++, j++)
                    {
                        //LinkButton lnk1 = new LinkButton();
                        lnkFileName = new LinkButton();
                        //lnkFileName.ID = "lnk2FileName1";
                        lnkFileName.Attributes.Add("runat", "server");
                        //this.lnk2.Click += new System.EventHandler(this.lnk2_Click);
                        Session["FileNames"] = dsfileName.Tables[0].Rows[j]["FileName"].ToString();
                        lnkFileName.Text = dsfileName.Tables[0].Rows[j]["FileName"].ToString();
                        //lnkFileName.Text = "</br>";
                        lnkFileName.Click += new System.EventHandler(this.lnkFileName_Click);
                        pnlFileName.Controls.Add(lnkFileName);
                        pnlFileName.Controls.Add(new LiteralControl("<br/>"));

                        // Add control to container:
                        //pnlFileName.Controls.Add(lnk1);
                        //lblFileName.Text = dsfileName.Tables[0].Rows[0]["FileName"].ToString();
                    }
                }
                //pnlFileName.Visible = true;
                //pnlFileName.Attributes.Add("onclick", "this.disabled=false;");
                //lnkFileName.Text = Session["FileNames"].ToString();
                // lnkFileName.Visible = true;

                btnSubmit.Attributes.Add("onclick", "return validate();");
                btnMove.Attributes.Add("onClick", "return checkSubCategorySelection();");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "Page_Load", ex.StackTrace);
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
            //for (int i = 0, j = 0; i < dsfileName.Tables[0].Rows.Count; i++, j++)
            //{
            this.ddlCatagory.SelectedIndexChanged += new System.EventHandler(this.ddlCatagory_SelectedIndexChanged);
            var lnkFileName = new LinkButton();
            pnlFileName.Controls.Add(lnkFileName);
            lnkFileName.ID = "lnk2FileName";
            lnkFileName.Attributes.Add("runat", "server");
            lnkFileName.Text = null;
            //lnk2.Text = dsfileName.Tables[0].Rows[j]["FileName"].ToString();
            lnkFileName.Click += new System.EventHandler(this.lnkFileName_Click);
            pnlFileName.Controls.Add(lnkFileName);
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        #endregion Web Form Designer generated code

        #region User defined functions

        public void GetSelectedIssue()
        {
            try
            {
                objViewIssue = new Model.clsViewMyIssues();
                objBLViewIssue = new BusinessLayer.clsBLViewMyIssues();

                //				objViewIssue.ReportIssueID = intReportIssueID;
                objViewIssue.IssueAssignmentID = intIssueAssignmentID;
                int userid = SAEmployeeID;

                dsSelectedIssue = objBLViewIssue.GetSelectedIssueforSuperAdmin(objViewIssue, userid);
                if (dsSelectedIssue.Tables[1].Rows.Count > 0)
                {
                    Session["Username"] = dsSelectedIssue.Tables[1].Rows[0]["UserName"].ToString();
                    Session["UserEmailId"] = dsSelectedIssue.Tables[1].Rows[0]["EmailID"].ToString();
                }
                if (dsSelectedIssue.Tables[0].Rows.Count > 0)
                {
                    FillIssueDetails();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "GetSelectedIssue", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void FillIssueDetails()
        {
            try
            {
                btnSubmit.Visible = true;
                btnCancel.Visible = true;
                if (dsSelectedIssue.Tables[0].Rows.Count > 0)
                {
                    //lblIssueType.Text = dsSelectedIssue.Tables[0].Rows[0]["Type"].ToString();
                    ddlIssueType.SelectedValue = dsSelectedIssue.Tables[0].Rows[0]["TypeID"].ToString();
                    lblPhoneExtension.Text = dsSelectedIssue.Tables[0].Rows[0]["PhoneExt"].ToString();
                    lblSeatingLocation.Text = dsSelectedIssue.Tables[0].Rows[0]["SeatingLocation"].ToString();
                    lblIssueID.Text = dsSelectedIssue.Tables[0].Rows[0]["ReportIssueID"].ToString();
                    lblcategory.Text = dsSelectedIssue.Tables[0].Rows[0]["SubCategory"].ToString();
                    lblIssueReportedBy.Text = dsSelectedIssue.Tables[0].Rows[0]["Name"].ToString();
                    lblIssueReportedOn.Text = dsSelectedIssue.Tables[0].Rows[0]["ReportIssueDate"].ToString();
                    lblProblemType.Text = dsSelectedIssue.Tables[0].Rows[0]["SubCategory"].ToString();
                    //Modified by Mahesh F For Issue ID:22449
                    ddlProblemSeverity.SelectedValue = dsSelectedIssue.Tables[0].Rows[0]["ProblemSeverityID"].ToString();
                    // lblProblemSeverity.Text = dsSelectedIssue.Tables[0].Rows[0]["ProblemSeverity"].ToString();
                    ////lblPriority.Text = dsSelectedIssue.Tables[0].Rows[0]["ProblemPriority"].ToString();
                    lblDescription.Text = dsSelectedIssue.Tables[0].Rows[0]["Description"].ToString();
                    lblComments.Text = dsSelectedIssue.Tables[0].Rows[0]["comments"].ToString();
                    hdnTxtCategory.Text = lblcategory.Text;
                    lblCurrentIssueStatus.Text = dsSelectedIssue.Tables[0].Rows[0]["CurrentStatus"].ToString();

                    lblDescComments.Text = dsSelectedIssue.Tables[0].Rows[0]["DescriptionAndComments"].ToString();

                    //txtCause.Text = dsSelectedIssue.Tables[0].Rows[0]["Cause"].ToString();
                    //txtFix.Text = dsSelectedIssue.Tables[0].Rows[0]["Fix"].ToString();

                    //Added By Nikhil
                    lblProjectName.Text = dsSelectedIssue.Tables[0].Rows[0]["ProjectName"].ToString();
                    lblProjectRole.Text = dsSelectedIssue.Tables[0].Rows[0]["RoleDescription"].ToString();
                    lblReportingTo.Text = dsSelectedIssue.Tables[0].Rows[0]["EmployeeName"].ToString();
                    lblResourcePool.Text = dsSelectedIssue.Tables[0].Rows[0]["ResourcePoolName"].ToString();
                    txtWorkHours.Text = dsSelectedIssue.Tables[0].Rows[0]["WorkHours"].ToString();
                    txtFromDate.Text = dsSelectedIssue.Tables[0].Rows[0]["FromDate"].ToString();
                    txtEndDate.Text = dsSelectedIssue.Tables[0].Rows[0]["ToDate"].ToString();
                    txtNoOfResources.Text = dsSelectedIssue.Tables[0].Rows[0]["NumberOfResources"].ToString();
                    hdnFromDate.Text = dsSelectedIssue.Tables[0].Rows[0]["actualStartDate"].ToString();
                    hdnToDate.Text = dsSelectedIssue.Tables[0].Rows[0]["actualEndDate"].ToString();
                    hdnReportedByEmpId.Text = dsSelectedIssue.Tables[0].Rows[0]["employeeid1"].ToString();
                    hdnProjectNameId.Text = dsSelectedIssue.Tables[0].Rows[0]["projectnameid"].ToString();
                    //Added Code End
                    intSubCategoryID = Convert.ToInt32(dsSelectedIssue.Tables[0].Rows[0]["SubCategoryID"].ToString());
                    Session["Department"] = dsSelectedIssue.Tables[0].Rows[0]["Category"].ToString();
                    //Department=Department.Substring(0,5);
                    FillStatusList();
                    FillLoginUserList();
                    ddlStatus.SelectedValue = dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString();
                    Session["OldStatus"] = ddlStatus.SelectedItem.Text.ToString();
                    if (Convert.ToInt32(ddlStatus.SelectedValue) == 2 || Convert.ToInt32(ddlStatus.SelectedValue) == 4)
                    {
                        btnChangeCategory.Visible = false;
                    }
                    if (Convert.ToInt32(ddlStatus.SelectedValue) == 1 || Convert.ToInt32(ddlStatus.SelectedValue) == 8)
                    {
                        btnMoveIssue.Visible = true;
                    }

                    if (!string.IsNullOrEmpty(dsSelectedIssue.Tables[0].Rows[0]["EmployeeID"].ToString()))
                    {
                        ddlLoginUser.SelectedValue = dsSelectedIssue.Tables[0].Rows[0]["EmployeeID"].ToString();
                        PrevEmployeeID = Convert.ToInt32(dsSelectedIssue.Tables[0].Rows[0]["EmployeeID"].ToString());
                        Session["IssueAssignmentID"] = Convert.ToInt32(dsSelectedIssue.Tables[0].Rows[0]["IssueAssignmentID"].ToString());
                    }
                    else
                    {
                        ddlLoginUser.SelectedValue = ddlLoginUser.Items.FindByText("Select").ToString();
                        PrevEmployeeID = (ddlLoginUser.SelectedIndex);
                        Session["IssueAssignmentID"] = 0;
                    }

                    if (dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "1")
                    {
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Open"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("ReOpened"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("InProgress"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Assigned"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Resolved"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Closed"));
                    }
                    if (dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "2")
                    {
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Open"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("ReOpened"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Closed"));
                    }

                    if (dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "5")
                    {
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Open"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("ReOpened"));
                        //ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Resolved"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Closed"));
                    }

                    if (dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "4")
                    {
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Open"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("InProgress"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Resolved"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Closed"));
                    }

                    if (dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "6" || dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "7")
                    {
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Open"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("InProgress"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Resolved"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Closed"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("ReOpened"));
                    }

                    if (dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "3" || dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "8" ||
                    dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "9")
                    {
                        btnSubmit.Visible = false;
                        btnCancel.Visible = false;
                    }
                    if (dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "8")
                    {
                        btnChangeCategory.Visible = false;
                        btnMoveIssue.Visible = false;
                    }

                    //Session["IssueAssignmentID"] = Convert.ToInt32(dsSelectedIssue.Tables[0].Rows[0]["IssueAssignmentID"].ToString());
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "FillIssueDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void FillStatusList()
        {
            try
            {
                //ddlStatus.DataSource = Status.BindStatusEnum(typeof(IssueStatus), "");
                //ddlStatus.DataTextField="Key";
                //ddlStatus.DataValueField="Value";
                //ddlStatus.DataBind();

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
                //ddlStatus.Items.Insert(0, "All");
                //ddlStatus.Items.Remove(ddlStatus.Items.FindByText("ReOpened"));
                //ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Resolved"));
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "FillStatusList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void FillLoginUserList()
        {
            objBLSubCategoryAssignment = new BusinessLayer.clsBLSubCategoryAssignment();
            objSubCategoryAssignment = new Model.clsSubCategoryAssignment();
            try
            {
                objSubCategoryAssignment.SubCategoryID = intSubCategoryID.ToString();
                dsLoginUserList = objBLSubCategoryAssignment.GetLoginUserList(objSubCategoryAssignment);
                ddlLoginUser.Items.Clear();
                //ddlLoginUser.Items.Insert(0,"Select");

                if (dsLoginUserList.Tables[0].Rows.Count >= 0)
                {
                    ddlLoginUser.DataSource = dsLoginUserList.Tables[0];
                    ddlLoginUser.DataTextField = dsLoginUserList.Tables[0].Columns["EmployeeName"].ToString();
                    ddlLoginUser.DataValueField = dsLoginUserList.Tables[0].Columns["EmployeeID"].ToString();
                    ddlLoginUser.DataBind();
                    ddlLoginUser.Items.Insert(0, "Select");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "FillLoginUserList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void FillLoginUserList1()
        {
            objBLSubCategoryAssignment = new BusinessLayer.clsBLSubCategoryAssignment();
            objSubCategoryAssignment = new Model.clsSubCategoryAssignment();
            try
            {
                objSubCategoryAssignment.SubCategoryID = ddlCatagory.SelectedValue.ToString();
                dsLoginUserList = objBLSubCategoryAssignment.GetLoginUserList(objSubCategoryAssignment);

                if (dsLoginUserList.Tables[0].Rows.Count > 0)
                {
                    ddlLoginUser.Items.Clear();
                    ddlLoginUser.Items.Add(new ListItem("Select", "0"));
                    for (int i = 0; i < dsLoginUserList.Tables[0].Rows.Count; i++)
                    {
                        ddlLoginUser.Items.Add(new ListItem(dsLoginUserList.Tables[0].Rows[i]["EmployeeName"].ToString(), dsLoginUserList.Tables[0].Rows[i]["EmployeeID"].ToString()));
                    }

                    //ddlLoginUser.DataSource = dsLoginUserList.Tables[0];
                    //ddlLoginUser.DataTextField = dsLoginUserList.Tables[0].Columns["EmployeeName"].ToString();
                    //ddlLoginUser.DataValueField = dsLoginUserList.Tables[0].Columns["EmployeeID"].ToString();
                    //ddlLoginUser.DataBind();
                    //ddlLoginUser.Items.Add(new ListItem("select", "0"));
                }
                else
                {
                    ddlLoginUser.Items.Clear();
                    ddlLoginUser.Items.Add(new ListItem("Select", "0"));
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "FillLoginUserList1", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void GetSuperAdminReportIssueHistory()
        {
            try
            {
                objViewIssue = new Model.clsViewMyIssues();
                objBLViewIssue = new BusinessLayer.clsBLViewMyIssues();

                objViewIssue.ReportIssueID = intReportIssueID;
                dsSuperAdminReportIssueHistory = objBLViewIssue.GetSuperAdminReportIssueHistory(objViewIssue);
                if (dsSuperAdminReportIssueHistory.Tables[0].Rows.Count > 0)
                {
                    dgIssueDetails.Visible = true;
                    dgIssueDetails.DataSource = dsSuperAdminReportIssueHistory;
                    dgIssueDetails.DataBind();
                    lblCheckHistory.Visible = false;
                }
                else
                {
                    dgIssueDetails.Visible = false;
                    lblCheckHistory.Visible = true;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "GetSuperAdminReportIssueHistory", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion User defined functions

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                //GetSelectedIssue();
                //GetSuperAdminReportIssueHistory();
                Response.Redirect("ViewSuperAdminIssues.aspx", false);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            int StatusID, EmployeeID;
            StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
            string categoryName = lblProblemType.Text.ToString();
            objIssueAssignment = new Model.clsIssueAssignment();
            objBLIssueAssignment = new BusinessLayer.clsBLIssueAssignment();
            if ((CategoryChangeFlag == 1 && ddlStatus.SelectedItem.Text != "Assigned") || (CategoryChangeFlag == 0 && lblcategory.Visible == false && ddlStatus.SelectedItem.Text != "Assigned"))
            {
                if (ddlCatagory.SelectedItem.Text == "Select" || (CategoryChangeFlag == 0 && ddlCatagory.SelectedItem.Text == "Select"))
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Please select Category";
                    CategoryChangeFlag = 0;
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "While Changing the Category, you can't change issue status to '" + ddlStatus.SelectedItem.Text + "', Please select Executive to assign issue";
                    CategoryChangeFlag = 0;
                }
            }
            else
            {
                if (ddlStatus.SelectedItem.Text == "ReOpened" || ddlStatus.SelectedItem.Text == "Closed")
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "You can not change the issue status to " + "'" + ddlStatus.SelectedItem.Text + "'";
                }
                else
                {
                    if ((ddlLoginUser.SelectedItem.Text == "Select") && (ddlStatus.SelectedItem.Text != "OnHold" && ddlStatus.SelectedItem.Text != "UserEscalated" && ddlStatus.SelectedItem.Text != "Cancelled"))
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Please select Employee To Assign issue";
                    }
                    else
                    {
                        if (categoryName == ConfigurationSettings.AppSettings["NewResourceText"].ToString()
                              || categoryName == ConfigurationSettings.AppSettings["UpdateCurrentAllocationText"].ToString()
                              || categoryName == ConfigurationSettings.AppSettings["SingleOrBulkExtensionText"].ToString())
                        {
                            if (categoryName != ConfigurationSettings.AppSettings["SingleOrBulkExtensionText"].ToString())
                            {
                                objIssueAssignment.WorkHours = Convert.ToInt32(txtWorkHours.Text);
                                objIssueAssignment.FromDate = Convert.ToDateTime(txtFromDate.Text.ToString());
                            }
                            objIssueAssignment.NumberOfResources = Convert.ToInt32(txtNoOfResources.Text);
                            objIssueAssignment.ToDate = Convert.ToDateTime(txtEndDate.Text.ToString());
                        }

                        if (ddlStatus.SelectedItem.Text == "OnHold" || ddlStatus.SelectedItem.Text == "Resolved" || ddlStatus.SelectedItem.Text == "InProgress" || ddlStatus.SelectedItem.Text == "UserEscalated" || ddlStatus.SelectedItem.Text == "Cancelled")
                        {
                            objIssueAssignment.IssueAssignmentID = Convert.ToInt32(Session["IssueAssignmentID"]);
                            objIssueAssignment.ReportIssueID = intReportIssueID;
                            objIssueAssignment.StatusID = StatusID;
                            if (ddlLoginUser.SelectedItem.Text != "Select")
                            {
                                EmployeeID = Convert.ToInt32(ddlLoginUser.SelectedValue);
                                objIssueAssignment.EmployeeID = EmployeeID;
                                objIssueAssignment.TypeID = Convert.ToInt32(ddlIssueType.SelectedValue);
                                objIssueAssignment.ProblemSeverity = Convert.ToInt32(ddlProblemSeverity.SelectedValue);
                            }
                            if (ddlStatus.SelectedItem.Text != "Resolved")
                            {
                                objIssueAssignment.TypeID = Convert.ToInt32(ddlIssueType.SelectedValue);
                                objIssueAssignment.ProblemSeverity = Convert.ToInt32(ddlProblemSeverity.SelectedValue);
                            }
                            objIssueAssignment.Cause = txtCause.Text;
                            objIssueAssignment.Fix = txtFix.Text;

                            //ddlLoginUser.SelectedValue ="25";
                        }
                        else
                        {
                            EmployeeID = Convert.ToInt32(ddlLoginUser.SelectedValue);

                            objIssueAssignment.IssueAssignmentID = Convert.ToInt32(Session["IssueAssignmentID"]);
                            objIssueAssignment.ReportIssueID = intReportIssueID;
                            objIssueAssignment.StatusID = StatusID;
                            objIssueAssignment.TypeID = Convert.ToInt32(ddlIssueType.SelectedValue);
                            objIssueAssignment.ProblemSeverity = Convert.ToInt32(ddlProblemSeverity.SelectedValue);
                            objIssueAssignment.EmployeeID = EmployeeID;
                            objIssueAssignment.Cause = txtCause.Text;
                            objIssueAssignment.Fix = txtFix.Text;
                        }

                        if (ddlCatagory.SelectedValue == "")
                        {
                            objIssueAssignment.SubCategory = intSubCategoryID;
                        }
                        else
                        {
                            objIssueAssignment.SubCategory = Convert.ToInt32(ddlCatagory.SelectedValue);
                        }

                        try
                        {
                            if (ddlLoginUser.SelectedItem.Text == "Select")
                            {
                                IsRecordInserted = objBLIssueAssignment.IssueAssignmentBySuperAdmin(objIssueAssignment);
                                if (IsRecordInserted)
                                {
                                    SendMail();
                                    lblMsg.Text = "";
                                    pnlMessage.Visible = true;
                                    //pnlIssueDetails.Visible = false;
                                }
                            }
                            else
                            {
                                if (PrevEmployeeID != Convert.ToInt32(ddlLoginUser.SelectedValue))
                                {
                                    IsRecordInserted = objBLIssueAssignment.IssueAssignmentBySuperAdmin(objIssueAssignment);
                                    if (IsRecordInserted)
                                    {
                                        SendMail();
                                        lblMsg.Text = "";
                                        pnlMessage.Visible = true;
                                        //pnlIssueDetails.Visible = false;
                                    }
                                }
                                else
                                {
                                    IsRecordUpdated = objBLIssueAssignment.IssueUpdateBySuperAdmin(objIssueAssignment);
                                    if (IsRecordUpdated)
                                    {
                                        SendMail();
                                        lblMsg.Text = "";
                                        pnlMessage.Visible = true;
                                        //pnlIssueDetails.Visible = false;
                                    }
                                    else
                                    {
                                        lblMsg.Text = "Error while updating issue";
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
                            objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "btnSubmit_Click", ex.StackTrace);
                            throw new V2Exceptions(ex.ToString(), ex);
                        }
                    }
                }
                CategoryChangeFlag = 0;
            }
        }

        //private void btnMove_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        objIssueAssignment = new Model.clsIssueAssignment();
        //        objBLIssueAssignment = new BusinessLayer.clsBLIssueAssignment();
        //        objIssueAssignment.ReportIssueID = intReportIssueID;
        //        objIssueAssignment.SubCategoryID = Convert.ToInt32(ddlCatagory.Items[ddlCatagory.SelectedIndex].Value);

        //        dsDepartment = objBLIssueAssignment.ChangeCategoryOfIssue(objIssueAssignment);

        //        if (dsDepartment.Tables[0].Rows.Count != 0)
        //        {
        //            Session["NewDepartment"] = dsDepartment.Tables[0].Rows[0]["category"].ToString();
        //            SendMail();
        //            lblMsg.Text = "";
        //            pnlMessage.Visible = true;
        //            pnlIssueDetails.Visible = false;
        //        }

        //    }
        //    catch (V2Exceptions ex)
        //    {
        //        throw;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "btnMove_Click", ex.StackTrace);
        //        throw new V2Exceptions(ex.ToString(),ex);
        //    }
        //}

        public void dgIssueDetails_PageChange(object sender, DataGridPageChangedEventArgs e)
        {
            try
            {
                dgIssueDetails.CurrentPageIndex = e.NewPageIndex;
                GetSuperAdminReportIssueHistory();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "dgIssueDetails_PageChange", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgIssueDetails_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                //foreach(DataGridItem dgi in dgIssueDetails.Items)
                //{
                //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                //{
                //    int intStatusID = Convert.ToInt32(((Label)e.Item.FindControl("lblStatusID")).Text);
                //    if (intStatusID == 1)
                //    {
                //        ((Label)e.Item.FindControl("lblStatus")).Text = IssueStatus.New.ToString();
                //    }
                //    else if (intStatusID == 2)
                //    {
                //        ((Label)e.Item.FindControl("lblStatus")).Text = IssueStatus.Resolved.ToString();
                //    }
                //    else if (intStatusID == 3)
                //    {
                //        ((Label)e.Item.FindControl("lblStatus")).Text = IssueStatus.Moved.ToString();
                //    }
                //    else if (intStatusID == 4)
                //    {
                //        ((Label)e.Item.FindControl("lblStatus")).Text = IssueStatus.Reopen.ToString();
                //    }
                //}
                //}
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "dgIssueDetails_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private string getFromEmailID()
        {
            try
            {
                objIssueAssignment = new Model.clsIssueAssignment();
                objBLIssueAssignment = new BusinessLayer.clsBLIssueAssignment();
                objIssueAssignment.EmployeeID = SAEmployeeID;
                strFromEmailID = objBLIssueAssignment.getEmployeeEmailID(objIssueAssignment);
                return strFromEmailID;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "getFromEmailID", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private string getEmployeeEmailID()
        {
            try
            {
                objIssueAssignment = new Model.clsIssueAssignment();
                objBLIssueAssignment = new BusinessLayer.clsBLIssueAssignment();
                objIssueAssignment.EmployeeID = Convert.ToInt32(ddlLoginUser.SelectedValue);
                strEmployeeEmailID = objBLIssueAssignment.getEmployeeEmailID(objIssueAssignment);
                return strEmployeeEmailID;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "getEmployeeEmailID", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private string GetIssueRaiserEmailID()
        {
            try
            {
                strEmployeeEmailID = objBLIssueAssignment.GetIssueRaiserEmailID(intReportIssueID);
                return strEmployeeEmailID;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private string getEmailBodyForMember()
        {
            try
            {
                objClsBLMemberWiseSearchReport = new clsBLMemberWiseSearchReport();
                DataSet dsStatus = new DataSet();
                dsStatus = objClsBLMemberWiseSearchReport.GetStatus();

                strEmailBodyForMember = "Hi " + ddlLoginUser.SelectedItem.Text + ",";

                for (int i = 0; i < dsStatus.Tables[0].Rows.Count; i++)
                {
                    if (ddlStatus.SelectedValue == dsStatus.Tables[0].Rows[i]["StatusID"].ToString())
                    {
                        strEmailBodyForMember += "<br>" + "<br>" + "An issue " + lblIssueID.Text + " has been " + "<b>" + dsStatus.Tables[0].Rows[i]["StatusDesc"].ToString() + "</b>";
                    }
                }

                if (ddlStatus.SelectedValue == "5")
                {
                    strEmailBodyForMember += " to you.";
                }
                strEmailBodyForMember += "<br>" + "<b>" + "Problem Description: " + "</b>" + lblDescription.Text;
                return strEmailBodyForMember;

                //if (ddlStatus.SelectedValue == "1")
                //{
                //    strEmailBodyForMember += "<br>" + "The Issue ID is " + "<b>" + lblIssueID.Text + "</b>" + "<br>" + " and issue status is " + "<b>" + "New" + "</b>";
                //}
                //else if (ddlStatus.SelectedValue == "2")
                //{
                //    strEmailBodyForMember += "<br>" + "The Issue ID is " + "<b>" + lblIssueID.Text + "</b>" + "<br>" + " and issue status is " + "<b>" + "Resolved" + "</b>";
                //}
                //else if (ddlStatus.SelectedValue == "3")
                //{
                //    strEmailBodyForMember += "<br>" + "The Issue ID is " + "<b>" + lblIssueID.Text + "</b>" + "<br>" + " and issue status is " + "<b>" + "Closed" + "</b>";
                //}
                //else if (ddlStatus.SelectedValue == "4")
                //{
                //    strEmailBodyForMember += "<br>" + "The Issue ID is " + "<b>" + lblIssueID.Text + "</b>" + "<br>" + " and issue status is " + "<b>" + "Reopened" + "</b>";
                //}
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "getEmailBodyForMember", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private string getEmailBodyForITDept()
        {
            try
            {
                objClsBLMemberWiseSearchReport = new clsBLMemberWiseSearchReport();
                DataSet dsStatus = new DataSet();
                dsStatus = objClsBLMemberWiseSearchReport.GetStatus();

                strEMailBodyForITDept = "Hi,";
                for (int i = 0; i < dsStatus.Tables[0].Rows.Count; i++)
                {
                    if (ddlStatus.SelectedValue == dsStatus.Tables[0].Rows[i]["StatusID"].ToString())
                    {
                        strEMailBodyForITDept += "<br>" + "<br>" + "An issue " + lblIssueID.Text + " has been " + "<b>" + dsStatus.Tables[0].Rows[i]["StatusDesc"].ToString() + "</b>";
                    }
                }

                if (ddlStatus.SelectedValue == "5")
                {
                    strEMailBodyForITDept += " to " + ddlLoginUser.SelectedItem.Text + ".";
                }
                strEMailBodyForITDept += "<br>" + "<b>" + "Problem Description: " + "</b>" + lblDescription.Text;
                return strEMailBodyForITDept;

                //strEMailBodyForITDept += "<br>" + "<br>" + "An issue " + lblIssueID.Text + " has been assigned to " + ddlLoginUser.SelectedItem.Text + ".";

                //if (ddlStatus.SelectedValue == "1")
                //{
                //    strEMailBodyForITDept += "<br>" + "The Issue ID is " + "<b>" + lblIssueID.Text + "</b>" + "<br>" + " and issue status is " + "<b>" + "New" + "</b>";
                //}
                //else if (ddlStatus.SelectedValue == "2")
                //{
                //    strEMailBodyForITDept += "<br>" + "The Issue ID is " + "<b>" + lblIssueID.Text + "</b>" + "<br>" + " and issue status is " + "<b>" + "Resolved" + "</b>";
                //}
                //else if (ddlStatus.SelectedValue == "3")
                //{
                //    strEMailBodyForITDept += "<br>" + "The Issue ID is " + "<b>" + lblIssueID.Text + "</b>" + "<br>" + " and issue status is " + "<b>" + "Closed" + "</b>";
                //}
                //else if (ddlStatus.SelectedValue == "4")
                //{
                //    strEMailBodyForITDept += "<br>" + "The Issue ID is " + "<b>" + lblIssueID.Text + "</b>" + "<br>" + " and issue status is " + "<b>" + "Reopened" + "</b>";
                //}
                //strEMailBodyForITDept += "<br>" + "<b>" + "Problem Description: " + "</b>" + lblDescription.Text;

                //return strEMailBodyForITDept;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "getEmailBodyForITDept", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        //Modified by Mahesh F for Issue ID:22449
        private void SendMail()
        {
            try
            {
                objIssueAssignment = new Model.clsIssueAssignment();
                Category = Session["Department"].ToString();
                SmtpClient SmtpMail = new SmtpClient(ConfigurationSettings.AppSettings["SMTPServerName"].ToString());
                string strDeptEmailID = ConfigurationSettings.AppSettings[Category].ToString();
                MailMessage objSendMailToMember = new MailMessage();
                MailMessage objSendMailToUser = new MailMessage();

                string CCCateory = Category + "CC";
                strDeptCCEmailID = ConfigurationSettings.AppSettings[CCCateory].ToString();

                // when there is change in Department, then follwowing mail will trigger.
                if (ddlStatus.SelectedItem.Text == "Open")
                {
                    string newcategory = Session["NewDepartment"].ToString();
                    string strNewDeptEmailID = ConfigurationSettings.AppSettings[newcategory].ToString();
                    string CCCateoryNew = newcategory + "CC";
                    string strDeptCCEmailIDNew = ConfigurationSettings.AppSettings[CCCateoryNew].ToString();

                    if (strDeptCCEmailIDNew.Contains(","))
                    {
                        string[] CCEmailIdNew = strDeptCCEmailIDNew.Split(',');
                        foreach (string email in CCEmailIdNew)
                        {
                            objSendMailToUser.To.Add(new MailAddress(email));
                        }
                    }
                    //objSendMailToUser.To.Add(new MailAddress(strDeptCCEmailIDNew));
                    objSendMailToUser.From = new MailAddress(Session["UserEmailId"].ToString());

                    objSendMailToUser.Subject = "HelpDesk : Department of Issue " + lblIssueID.Text + " under " + Session["Department"].ToString() + " : "
                      + lblcategory.Text.ToString() + " has been changed to " + Session["NewDepartment"].ToString() + ".";

                    objSendMailToUser.Body = "Hi, " + "<br>" + "<br>" +
                    "This is to inform you that, the issue department has been changed from " + "<b>" + Session["Department"].ToString() +
                     "</b>" + " to " + "<b>" + Session["NewDepartment"].ToString() + "</b>" + "<br>" + " Issue details are as follows :" + "<br>" + "<br>" +

                     "<b>" + "Issue ID : " + "</b>" + "<b>" + Server.HtmlEncode(lblIssueID.Text.ToString()) + "</b>" + "<br>" + "<br>" +
                     "<b>" + "Department Name : " + "</b>" + Server.HtmlEncode(Session["NewDepartment"].ToString()) + "<br>" + "<br>" +
                     "<b>" + "Category Name : " + "</b>" + Server.HtmlEncode(ddlCatagory.SelectedItem.Text.ToString()) + "<br>" + "<br>" +
                     "<b>" + "Issue Severity : " + "</b>" + Server.HtmlEncode(ddlProblemSeverity.SelectedItem.Text.ToString()) + "<br>" + "<br>" +
                     "<b>" + "Current Status : " + "</b>" + Server.HtmlEncode(ddlStatus.SelectedItem.Text.ToString()) + "<br>" + "<br>" +
                     "<b>" + "Description : " + "</b>" + Server.HtmlEncode(lblDescription.Text.ToString()) + "." + "<br>" + "<br>" +
                     "<b>" + "Cause : " + "</b>" + Server.HtmlEncode(txtCause.Text.ToString()) + "." + "<br>" + "<br>" +
                     "<b>" + "Fix : " + "</b>" + Server.HtmlEncode(txtFix.Text.ToString()) + "." + "<br>" + "<br>" +
                     "Please take the necessary action." + "<br>" + "<br>" + "<br>" +
                      "Regards," + "<br>" + Session["Username"].ToString();

                    objSendMailToUser.IsBodyHtml = true;

                    strDeptCCEmailID = strDeptCCEmailID + "," + GetIssueRaiserEmailID();

                    if (strDeptCCEmailID.Contains(","))
                    {
                        string[] CCEmailId = strDeptCCEmailID.Split(',');
                        foreach (string email in CCEmailId)
                        {
                            objSendMailToUser.CC.Add(email);
                        }
                    }

                    SmtpMail.UseDefaultCredentials = false;
                    SmtpMail.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), ConfigurationSettings.AppSettings["SMTPPassword"].ToString());
                    SmtpMail.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["PortNumber"].ToString());
                    SmtpMail.EnableSsl = true;
                    SmtpMail.Send(objSendMailToUser);
                    lblStatusUpdateMsg.Text = "An email will be sent to you confirming the moving of the issue.";
                    Session["NewDepartment"] = "";
                }
                // when there is change in other statuses, then follwowing mail will trigger.
                else
                {
                    if (ddlStatus.SelectedItem.Text != "Assigned")
                    {
                        objSendMailToUser.To.Add(new MailAddress(GetIssueRaiserEmailID()));
                        objSendMailToUser.From = new MailAddress(Session["UserEmailId"].ToString());

                        objSendMailToUser.Subject = "HelpDesk : Status of Issue " + lblIssueID.Text + " under " + Session["Department"].ToString() + " : "
                          + lblcategory.Text.ToString() + " has been changed to " + ddlStatus.SelectedItem.Text.ToString() + ".";

                        objSendMailToUser.Body = "Hi, " + "<br>" + "<br>" +
                        "This is to inform you that, the issue status has been changed from " + "<b>" + Session["OldStatus"].ToString() +
                         "</b>" + " to " + "<b>" + ddlStatus.SelectedItem.Text.ToString() + "</b>" + "<br>" + " Issue details are as follows :" + "<br>" + "<br>" +

                         "<b>" + "Issue ID : " + "</b>" + "<b>" + Server.HtmlEncode(lblIssueID.Text.ToString()) + "</b>" + "<br>" + "<br>" +
                         "<b>" + "Department Name : " + "</b>" + Server.HtmlEncode(Session["Department"].ToString()) + "<br>" + "<br>" +
                         "<b>" + "Category Name : " + "</b>" + Server.HtmlEncode(lblcategory.Text.ToString()) + "<br>" + "<br>" +
                         "<b>" + "Issue Severity : " + "</b>" + Server.HtmlEncode(ddlProblemSeverity.SelectedItem.Text.ToString()) + "<br>" + "<br>" +
                         "<b>" + "Current Status : " + "</b>" + Server.HtmlEncode(ddlStatus.SelectedItem.Text.ToString()) + "<br>" + "<br>" +
                         "<b>" + "Description : " + "</b>" + Server.HtmlEncode(lblDescription.Text.ToString()) + "." + "<br>" + "<br>" +
                         "<b>" + "Cause : " + "</b>" + Server.HtmlEncode(txtCause.Text.ToString()) + "." + "<br>" + "<br>" +
                         "<b>" + "Fix : " + "</b>" + Server.HtmlEncode(txtFix.Text.ToString()) + "." + "<br>" + "<br>" +
                         "Please take the necessary action." + "<br>" + "<br>" + "<br>" +
                          "Regards," + "<br>" + Session["Username"].ToString();

                        objSendMailToUser.IsBodyHtml = true;

                        if (ddlLoginUser.SelectedValue != "Select")
                        {
                            strDeptCCEmailID = strDeptCCEmailID + "," + getEmployeeEmailID();
                        }

                        if (strDeptCCEmailID.Contains(","))
                        {
                            string[] CCEmailId = strDeptCCEmailID.Split(',');
                            foreach (string email in CCEmailId)
                            {
                                objSendMailToUser.CC.Add(email);
                            }
                        }
                        SmtpMail.UseDefaultCredentials = false;
                        SmtpMail.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), ConfigurationSettings.AppSettings["SMTPPassword"].ToString());
                        SmtpMail.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["PortNumber"].ToString());
                        SmtpMail.EnableSsl = true;
                        SmtpMail.Send(objSendMailToUser);
                        lblStatusUpdateMsg.Text = "An email will be sent to you confirming the recording of the issue.";
                    }

                    //If admin assigns the issue to some executive then follwing mail will trigger,
                    // adrresing him as TO
                    else
                    {
                        if (ddlLoginUser.SelectedValue != "Select")
                        {
                            objSendMailToMember.To.Add(new MailAddress(getEmployeeEmailID()));
                            objSendMailToMember.From = new MailAddress(Session["UserEmailId"].ToString());

                            objSendMailToMember.Subject = "HelpDesk : Issue " + lblIssueID.Text + " under " + Session["Department"].ToString() + " : "
                          + lblcategory.Text.ToString() + " has been assigned to you.";

                            objSendMailToMember.Body = "Hi, " + "<br>" + "<br>" +
                            "This is to inform you that, an issue has been assigned to you. " + "<br>" + " Issue details are as follows :" + "<br>" + "<br>" +

                             "<b>" + "Issue ID : " + "</b>" + "<b>" + Server.HtmlEncode(lblIssueID.Text.ToString()) + "</b>" + "<br>" + "<br>" +
                             "<b>" + "Department Name : " + "</b>" + Server.HtmlEncode(Session["Department"].ToString()) + "<br>" + "<br>" +
                             "<b>" + "Category Name : " + "</b>" + Server.HtmlEncode(lblcategory.Text.ToString()) + "<br>" + "<br>" +
                             "<b>" + "Issue Severity : " + "</b>" + Server.HtmlEncode(ddlProblemSeverity.SelectedItem.Text.ToString()) + "<br>" + "<br>" +
                             "<b>" + "Current Status : " + "</b>" + Server.HtmlEncode(ddlStatus.SelectedItem.Text.ToString()) + "<br>" + "<br>" +
                             "<b>" + "Description : " + "</b>" + Server.HtmlEncode(lblDescription.Text.ToString()) + "." + "<br>" + "<br>" +
                              "<b>" + "Cause : " + "</b>" + Server.HtmlEncode(txtCause.Text.ToString()) + "." + "<br>" + "<br>" +
                             "<b>" + "Fix : " + "</b>" + Server.HtmlEncode(txtFix.Text.ToString()) + "." + "<br>" + "<br>" +
                             "Please take the necessary action." + "<br>" + "<br>" + "<br>" +
                              "Regards," + "<br>" + Session["Username"].ToString();

                            objSendMailToMember.IsBodyHtml = true;

                            strDeptCCEmailID = strDeptCCEmailID + "," + GetIssueRaiserEmailID();

                            if (strDeptCCEmailID.Contains(","))
                            {
                                string[] CCEmailId = strDeptCCEmailID.Split(',');
                                foreach (string email in CCEmailId)
                                {
                                    objSendMailToMember.CC.Add(email);
                                }
                            }
                            SmtpMail.UseDefaultCredentials = false;
                            SmtpMail.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), ConfigurationSettings.AppSettings["SMTPPassword"].ToString());
                            SmtpMail.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["PortNumber"].ToString());
                            SmtpMail.EnableSsl = true;
                            SmtpMail.Send(objSendMailToMember);
                            lblStatusUpdateMsg.Text = "An email will be sent to you confirming the recording of the issue.";
                        }
                    }

                    Session["UserEmailId"] = "";
                    Session["Username"] = "";
                    Session["OldStatus"] = "";
                    Session["Department"] = "";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "Page_Load", ex.StackTrace);
                lblStatusUpdateMsg.Text = "Issue has been logged but an Email could not be sent. Please check the HelpDesk for Issue ID.";
            }
        }

        private void lnkFileName_Click(object sender, System.EventArgs e)
        {
            try
            {
                string fileName = "";

                LinkButton myButton = sender as LinkButton;
                if (myButton != null)
                {
                    fileName = myButton.Text;
                }
                string remoteHostName = Request.Headers["Host"].ToString();
                //string SaveLocation = ConfigurationSettings.AppSettings["UploadedfilePath"].ToString();
                //string applicationPath = Request.ApplicationPath.ToString();
                string resumePath = "http://" + remoteHostName + "/Uploads/" + fileName;
                //string resumePath2 = "http://" + SaveLocation + "/helpdeskadmin/Uploads/" + fileName;
                //string resumePath = fileName;

                Response.Write("<script language='JavaScript'>" + '\n');
                Response.Write("val = window.open('" + resumePath + "')" + '\n');

                Response.Write("</script>" + '\n');
                Session["FileNames"] = "";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "lnkbtnFileName_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnRedirectToPms_click(object sender, System.EventArgs e)
        {
            try
            {
                // string remoteHostName = Request.Headers["Host"].ToString();
                //string resumePath = "http://" + remoteHostName + "/Appraisal/AppraisalProcessIndex/";
                //Response.Write("<script language='JavaScript'>" + '\n');
                //Response.Write("val = window.open('" + resumePath + "')" + '\n');
                // string url = "http://" + remoteHostName + "/Appraisal/AppraisalProcessIndex";
                //Session["fromdate"] = txtFromDate.Text;
                // ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( url, null, 'height=550, width=1200,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no' );", true);
                //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( url, '_blank');", true);
                //Response.Write("</script>" + '\n');
                //Response.RedirectPermanent("http://" + remoteHostName + "/Appraisal/AppraisalProcessIndex", false);
                // Response.Redirect("http://192.168.30.15/intranet/");

                // ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( url, '_blank');", true);
                //ClientScript.RegisterStartupScript(GetType(), "openwindow", "<script type=text/javascript> window.open(url); </script>");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "btnRedirectToPms_click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnChangeCategory_Click(object sender, System.EventArgs e)
        {
            try
            {
                CategoryChangeFlag = 1;
                Flag = 1;
                ddlCatagory.Items.Clear();
                ddlCatagory.Visible = true;
                lblcategory.Visible = false;
                bindCategory();
                btnMove.Visible = false;
                btnSubmit.Visible = true;
                btnChangeCategory.Visible = false;
                btnMoveIssue.Visible = true;
                ddlStatus.Items.Clear();
                FillStatusList();
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Open"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("ReOpened"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("InProgress"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("OnHold"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Resolved"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Closed"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("UserEscalated"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Cancelled"));

                ddlStatus.Items.FindByText("Assigned").Selected = true;
                ddlStatus.Enabled = false;
                ddlLoginUser.Enabled = true;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "btnChangeCategory_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void bindCategory()
        {
            try
            {
                objViewIssue = new Model.clsViewMyIssues();
                objBLViewIssue = new BusinessLayer.clsBLViewMyIssues();
                ddlCatagory.Items.Add(new ListItem("Select", "0"));
                objViewIssue.SubCategoryId = intSubCategoryID;
                dsSelectedIssue = objBLViewIssue.bindCategory(objViewIssue);
                if (dsSelectedIssue.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsSelectedIssue.Tables[0].Rows.Count; i++)
                        ddlCatagory.Items.Add(new ListItem(dsSelectedIssue.Tables[0].Rows[i][1].ToString(), dsSelectedIssue.Tables[0].Rows[i][0].ToString()));
                }
                ddlCatagory.SelectedValue = intSubCategoryID.ToString();
                foreach (ListItem li in ddlCatagory.Items)
                {
                    li.Attributes["ToolTip"] = li.Text;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "bindCategory", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void ddlCatagory_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                FillLoginUserList1();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "ddlCatagory_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnMoveIssue_Click(object sender, System.EventArgs e)
        {
            try
            {
                CategoryChangeFlag = 1;
                Flag = 1;
                ddlCatagory.Items.Clear();
                ddlCatagory.Visible = true;
                lblcategory.Visible = false;
                BindSubCategory();
                btnMove.Visible = true;
                btnSubmit.Visible = false;
                btnChangeCategory.Visible = true;
                btnMoveIssue.Visible = false;
                ddlStatus.Items.Clear();
                FillStatusList();
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Assigned"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("ReOpened"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("InProgress"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("OnHold"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Resolved"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Closed"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("UserEscalated"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Cancelled"));

                ddlStatus.Items.FindByText("Open").Selected = true;
                ddlStatus.Enabled = false;

                ddlLoginUser.Enabled = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "btnMoveISsue_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void BindSubCategory()
        {
            try
            {
                ddlCatagory.Items.Clear();
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                DataSet dsSubCategory = objClsBLReportIssue.GetSubCategory();

                for (int i = 0; i <= dsSubCategory.Tables[0].Rows.Count - 1; i++)
                {
                    ListItem li = new ListItem(dsSubCategory.Tables[0].Rows[i][1].ToString(), dsSubCategory.Tables[0].Rows[i][0].ToString());
                    //li.Attributes.Add("style","BACKGROUND-COLOR:red");
                    ddlCatagory.Items.Add(li);
                    if (ddlCatagory.Items[i].Value.Equals("0"))
                    {
                        ddlCatagory.Items[i].Attributes.Add("class", "tableheader");
                    }
                    else
                    {
                        ddlCatagory.Items[i].Text = ddlCatagory.Items[i].Text.Replace("--", "");
                    }
                }
                ddlCatagory.SelectedValue = intSubCategoryID.ToString();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "BindSubCategory", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void ddlLoginUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlStatus.Enabled = true;
                ddlStatus.Items.Clear();
                FillStatusList();

                if (ddlLoginUser.SelectedValue == "Select")
                {
                    ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Open"));
                    ddlStatus.Items.Remove(ddlStatus.Items.FindByText("ReOpened"));
                    //ddlStatus.Items.Remove(ddlStatus.Items.FindByText("InProgress"));
                    //ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Assigned"));
                    //ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Resolved"));
                    ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Closed"));
                }
                else
                {
                    ddlStatus.Items.FindByText("Assigned").Selected = true;
                    ddlStatus.Enabled = false;
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private void BindType()
        {
            try
            {
                ddlIssueType.Items.Clear();

                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                DataSet dsType = objClsBLReportIssue.GetType();

                for (int i = 0; i < dsType.Tables[0].Rows.Count; i++)
                {
                    ddlIssueType.Items.Add(new ListItem(dsType.Tables[0].Rows[i]["RequestType"].ToString(), dsType.Tables[0].Rows[i]["TypeID"].ToString()));
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "BindType", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        //Modified by Mahesh F For Issue ID:22449
        private void BindProblemSeverity()
        {
            try
            {
                ddlProblemSeverity.Items.Clear();

                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                DataSet dsProblemSeverity = objClsBLReportIssue.GetProblemSeverity();

                for (int i = 0; i < dsProblemSeverity.Tables[0].Rows.Count; i++)
                {
                    ddlProblemSeverity.Items.Add(new ListItem(dsProblemSeverity.Tables[0].Rows[i]["ProblemSeverity"].ToString(), dsProblemSeverity.Tables[0].Rows[i]["ProblemSeverityID"].ToString()));
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "BindProblemSeverity", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnMove_Click1(object sender, EventArgs e)
        {
            try
            {
                objIssueAssignment = new Model.clsIssueAssignment();
                objBLIssueAssignment = new BusinessLayer.clsBLIssueAssignment();
                objIssueAssignment.ReportIssueID = intReportIssueID;
                objIssueAssignment.SubCategoryID = Convert.ToInt32(ddlCatagory.Items[ddlCatagory.SelectedIndex].Value);
                objIssueAssignment.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);

                dsDepartment = objBLIssueAssignment.ChangeCategoryOfIssue(objIssueAssignment);

                if (dsDepartment.Tables[0].Rows.Count != 0)
                {
                    Session["NewDepartment"] = dsDepartment.Tables[0].Rows[0]["category"].ToString();
                    SendMail();
                    lblMsg.Text = "";
                    pnlMessage.Visible = true;
                    //pnlIssueDetails.Visible = false;
                }
                else
                {
                    lblMsg.Text = "Error while moving the Issue";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssueBySuperAdmin.aspx", "btnMove_Click1", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        //protected void lnkReallocation_Click(object sender, EventArgs e)
        //{
        //    string encryptedHelpdeskTicketID = string.Empty;

        //}

        [WebMethod]
        public static string GetIssueDetails(string helpdeskid, string projectid, string hdnreportedbyEmpid)
        {
            // instantiate a serializer

            JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            ArrayList list = new ArrayList();
            list.Add(helpdeskid);
            list.Add(projectid);
            list.Add(hdnreportedbyEmpid);
            ArrayList list1 = new ArrayList();
            foreach (string value in list)
            {
                list1.Add(Commondal.Encrypt(Convert.ToString(value), true));
            }
            var TheJson = TheSerializer.Serialize(list1);

            return TheJson;
        }

        [WebMethod]
        public static string GetUpdateCurrentAllocation(string helpdeskid, string projectid, string projectrole, string workhours, string fromdate, string enddate, string hdnreportedbyEmpid)
        {
            // instantiate a serializer

            JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            ArrayList list = new ArrayList();
            list.Add(helpdeskid);
            list.Add(projectid);
            list.Add(projectrole);
            list.Add(workhours);
            list.Add(fromdate);
            list.Add(enddate);
            list.Add(hdnreportedbyEmpid);
            ArrayList list1 = new ArrayList();
            foreach (string value in list)
            {
                list1.Add(Commondal.Encrypt(Convert.ToString(value), true));
            }
            var TheJson = TheSerializer.Serialize(list1);

            return TheJson;
        }

        [WebMethod]
        public static string GetNewAllocation(string helpdeskid, string projectid, string projectrole, string workhours, string fromdate, string enddate, string hdnreportedbyEmpid)
        {
            // instantiate a serializer

            JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            ArrayList list = new ArrayList();
            list.Add(helpdeskid);
            list.Add(projectid);
            list.Add(projectrole);
            list.Add(workhours);
            list.Add(fromdate);
            list.Add(enddate);
            list.Add(hdnreportedbyEmpid);
            ArrayList encrptedList1 = new ArrayList();
            foreach (string value in list)
            {
                encrptedList1.Add(Commondal.Encrypt(Convert.ToString(value), true));
            }
            var TheJson = TheSerializer.Serialize(encrptedList1);

            return TheJson;
        }
    }
}