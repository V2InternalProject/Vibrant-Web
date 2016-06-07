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
    /// Summary description for ViewSelectedIssue.
    /// </summary>
    public partial class ViewSelectedIssue : System.Web.UI.Page
    {
        #region Variable declaration

        private Model.clsViewMyIssues objViewIssue;
        private Model.clsIssueAssignment objIssueAssignment;
        private BusinessLayer.clsBLViewMyIssues objBLViewIssue;
        private BusinessLayer.clsBLStatus objBLStatus;
        private BusinessLayer.clsBLIssueAssignment objBLIssueAssignment;
        private DataSet dsSelectedIssue, dsStatusList, dsReportIssueHistory;
        private Boolean IsRecordUpdated;
        public int intStatusID, intReportIssueID, EmployeeID, SAEmployeeID, varEmployeeID;
        public string strFromEmailID, strToEmailID, strgetUserName;
        public string strMailSubForUser, strMailBodyForUser, strMailSubForITDept, strMailBodyForITDept;

        #endregion Variable declaration

        protected System.Web.UI.WebControls.LinkButton lnkbtnFileName;
        protected System.Web.UI.WebControls.RangeValidator RangeValidatorEndDate;
        protected System.Web.UI.WebControls.RangeValidator RangeValidatorFromDate;
        public string Department = string.Empty, strDeptCCEmailID = string.Empty, strEmployeeEmailID = string.Empty;
        private static DataSet dsfileName;
        private static LinkButton lnkFileName;

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
                intReportIssueID = Convert.ToInt32(Session["ReportIssueID"]);
                intStatusID = Convert.ToInt32(Session["StatusID"]);
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtEndDate.Attributes.Add("readonly", "readonly");
                if (SAEmployeeID.ToString() == "" || SAEmployeeID == 0)
                {
                    Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
                }
                pnlMessage.Visible = false;
                clsIssueAssignment objIssueAssignment = new clsIssueAssignment();
                clsBLIssueAssignment objBLIssueAssignment = new clsBLIssueAssignment();
                if (!IsPostBack)
                {
                    if (SAEmployeeID.ToString() == "" || SAEmployeeID == 0)
                    {
                        Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
                    }
                    else
                    {
                        varEmployeeID = SAEmployeeID;
                        Session["varEmployeeID"] = varEmployeeID;
                    }

                    lblCheckHistory.Visible = false;
                    dgIssueDetails.Visible = false;
                    GetSelectedIssue();
                    GetReportIssueHistory();
                }

                objIssueAssignment.ReportIssueID = intReportIssueID;
                dsfileName = objBLIssueAssignment.FileName(objIssueAssignment);
                if (dsfileName.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0, j = 0; i < dsfileName.Tables[0].Rows.Count; i++, j++)
                    {
                        lnkFileName = new LinkButton();
                        lnkFileName.Attributes.Add("runat", "server");
                        Session["FileNames"] = dsfileName.Tables[0].Rows[j]["FileName"].ToString();
                        lnkFileName.Text = dsfileName.Tables[0].Rows[j]["FileName"].ToString();

                        lnkFileName.Click += new EventHandler(lnkFileName_Click);
                        pnlFileName.Controls.Add(lnkFileName);
                        pnlFileName.Controls.Add(new LiteralControl("<br/>"));
                    }
                }
                btnSubmit.Attributes.Add("onclick", "return validate();");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "Page_Load", ex.StackTrace);
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
            var lnkFileName = new LinkButton();
            pnlFileName.Controls.Add(lnkFileName);
            lnkFileName.ID = "lnk2FileName";
            lnkFileName.Attributes.Add("runat", "server");
            lnkFileName.Text = null;
            //lnk2.Text = dsfileName.Tables[0].Rows[j]["FileName"].ToString();
            lnkFileName.Click += new System.EventHandler(this.lnkFileName_Click);
            pnlFileName.Controls.Add(lnkFileName);
        }

        #endregion Web Form Designer generated code

        #region User defined functions

        public void GetSelectedIssue()
        {
            try
            {
                objViewIssue = new Model.clsViewMyIssues();
                objBLViewIssue = new BusinessLayer.clsBLViewMyIssues();
                objViewIssue.StatusID = intStatusID;
                objViewIssue.ReportIssueID = intReportIssueID;
                int userid = EmployeeID;
                Session["status"] = (objViewIssue.StatusID).ToString();
                dsSelectedIssue = objBLViewIssue.GetSelectedIssue(objViewIssue, userid);

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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "GetSelectedIssue", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void FillIssueDetails()
        {
            try
            {
                if (dsSelectedIssue.Tables[0].Rows.Count > 0)
                {
                    lblIssueType.Text = dsSelectedIssue.Tables[0].Rows[0]["Type"].ToString();
                    lblPhoneExtension.Text = dsSelectedIssue.Tables[0].Rows[0]["PhoneExt"].ToString();
                    lblSeatingLocation.Text = dsSelectedIssue.Tables[0].Rows[0]["SeatingLocation"].ToString();
                    lblIssueID.Text = dsSelectedIssue.Tables[0].Rows[0]["ReportIssueID"].ToString();
                    lblIssueReportedBy.Text = dsSelectedIssue.Tables[0].Rows[0]["Name"].ToString();
                    lblIssueReportedOn.Text = dsSelectedIssue.Tables[0].Rows[0]["ReportIssueDate"].ToString();
                    lblProblemType.Text = dsSelectedIssue.Tables[0].Rows[0]["SubCategory"].ToString();
                    lblProblemSeverity.Text = dsSelectedIssue.Tables[0].Rows[0]["ProblemSeverity"].ToString();
                    lblDescription.Text = dsSelectedIssue.Tables[0].Rows[0]["Description"].ToString();

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
                    hdnReportedByEmpId.Text = dsSelectedIssue.Tables[0].Rows[0]["employeeid"].ToString();
                    hdnProjectNameId.Text = dsSelectedIssue.Tables[0].Rows[0]["projectnameid"].ToString();
                    hdnTxtCategory.Text = lblProblemType.Text;
                    //Added Code End

                    lblPreviousComment.Text = dsSelectedIssue.Tables[0].Rows[0]["comments"].ToString();
                    lblCommentDesc.Text = dsSelectedIssue.Tables[0].Rows[0]["descriptionAndComments"].ToString();
                    Session["Department"] = dsSelectedIssue.Tables[0].Rows[0]["Category"].ToString();

                    if (lblPreviousComment.Text == "")
                    {
                        lblComment.Visible = false;
                    }
                    FillStatusList();
                    ddlStatus.SelectedValue = dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString();
                    Session["OldStatus"] = ddlStatus.SelectedItem.Text.ToString();
                    Session["IssueAssignmentID"] = Convert.ToInt32(dsSelectedIssue.Tables[0].Rows[0]["IssueAssignmentID"].ToString());

                    if (dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "1" || dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "2" ||
                        dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "3" || dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "4" ||
                        dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "5" || dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "6" ||
                        dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "7")
                    {
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Closed"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Assigned"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("ReOpened"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Cancelled"));
                    }
                    if (dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "8" || dsSelectedIssue.Tables[0].Rows[0]["StatusID"].ToString() == "9")
                    {
                        hdnStatusOfIssue.Text = "Closed";
                        btnSubmit.Visible = false;
                        btnCancel.Visible = false;
                        ddlStatus.Enabled = false;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "FillIssueDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        //have to change here
        public void FillStatusList()
        {
            try
            {
                objViewIssue = new Model.clsViewMyIssues();
                objBLViewIssue = new BusinessLayer.clsBLViewMyIssues();
                DataSet dsStatus = new DataSet();
                int EmployeeID = Convert.ToInt32(Session["varEmployeeID"]);
                int Status = Convert.ToInt32(Session["status"]);
                dsStatus = objBLViewIssue.GetStatusAccToRole(EmployeeID, Status);
                ddlStatus.Items.Clear();
                for (int i = 0; i < dsStatus.Tables[0].Rows.Count; i++)
                {
                    ddlStatus.Items.Add(new ListItem(dsStatus.Tables[0].Rows[i]["StatusDesc"].ToString(), dsStatus.Tables[0].Rows[i]["StatusID"].ToString()));
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "FillStatusList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
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
                string resumePath = "http://" + remoteHostName + "/Uploads/" + fileName;
                //D:\NEWHRMSPUBLISH\Uploads
                Response.Write("<script language='JavaScript'>" + '\n');
                Response.Write("val = window.open('" + resumePath + "')" + '\n');

                Response.Write("</script>" + '\n');
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

        public void GetReportIssueHistory()
        {
            try
            {
                objViewIssue = new Model.clsViewMyIssues();
                objBLViewIssue = new BusinessLayer.clsBLViewMyIssues();

                objViewIssue.ReportIssueID = intReportIssueID;
                dsReportIssueHistory = objBLViewIssue.GetReportIssueHistory(objViewIssue);
                if (dsReportIssueHistory.Tables[0].Rows.Count > 0)
                {
                    dgIssueDetails.Visible = true;
                    dgIssueDetails.DataSource = dsReportIssueHistory;
                    dgIssueDetails.DataBind();
                    lblCheckHistory.Visible = false;
                    pnlFileName.Visible = true;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "GetReportIssueHistory", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion User defined functions

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            clsBLViewMyStatus objClsBLViewMyStatus = new clsBLViewMyStatus();
            string categoryName = lblProblemType.Text.ToString();
            int StatusID;
            StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
            if (StatusID != 5)
            {
                objIssueAssignment = new Model.clsIssueAssignment();
                objBLIssueAssignment = new BusinessLayer.clsBLIssueAssignment();

                objIssueAssignment.StatusID = StatusID;
                objIssueAssignment.IssueAssignmentID = Convert.ToInt32(Session["IssueAssignmentID"]);
                objIssueAssignment.ReportIssueID = intReportIssueID;
                objIssueAssignment.Cause = txtCause.Text;
                objIssueAssignment.Fix = txtFix.Text;
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
                string name1 = null;

                if (Convert.ToInt32(Session["SAEmployeeID"]) != 0)
                {
                    name1 = Session["SAEmployeeID"].ToString();
                }
                else
                {
                    name1 = Session["EmployeeID"].ToString();
                }
                string name = "";
                DataSet dsEmpName = objClsBLViewMyStatus.GetEmployeeName(name1);
                if (dsEmpName.Tables[0].Rows.Count > 0)
                {
                    name = dsEmpName.Tables[0].Rows[0]["EmployeeName"].ToString();
                }
                if (txtAddcomment.Text != string.Empty)
                {
                    objIssueAssignment.AddComment = " [" + DateTime.Now + " ]" + ' ' + name + ':' + ' ' + txtAddcomment.Text + ".</br>";
                }
                else
                {
                    objIssueAssignment.AddComment = "";
                }
                try
                {
                    IsRecordUpdated = objBLIssueAssignment.UpdateIssueByLoginUser(objIssueAssignment, name1);
                    lblComment.Text = txtAddcomment.Text;
                    if (IsRecordUpdated)
                    {
                        GetReportIssueHistory();
                        if (ddlStatus.SelectedItem.Value != "1")
                        {
                            SendMail();
                        }
                        lblMsg.Text = "";
                        pnlMessage.Visible = true;
                    }
                    else
                    {
                        lblMsg.Text = "Error while updating issue";
                    }
                }
                catch (V2Exceptions ex)
                {
                    throw;
                }
                catch (System.Exception ex)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "btnSubmit_Click", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Please select 'Inprogress' status";
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                Response.Redirect("ViewMyIssues.aspx");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void lnkbtnCheckHistory_Click(object sender, System.EventArgs e)
        {
        }

        public void dgIssueDetails_PageChange(object sender, DataGridPageChangedEventArgs e)
        {
            try
            {
                dgIssueDetails.CurrentPageIndex = e.NewPageIndex;
                GetReportIssueHistory();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "dgIssueDetails_PageChange", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgIssueDetails_ItemDataBound(object sender, DataGridItemEventArgs e)
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "dgIssueDetails_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private string getFromEmailID()
        {
            try
            {
                objIssueAssignment = new Model.clsIssueAssignment();
                objBLIssueAssignment = new BusinessLayer.clsBLIssueAssignment();
                objIssueAssignment.IssueAssignmentID = Convert.ToInt32(Session["IssueAssignmentID"]);
                strFromEmailID = objBLIssueAssignment.getFromEmailID(objIssueAssignment);
                return strFromEmailID;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "getFromEmailID", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private string getToEmailID()
        {
            try
            {
                objIssueAssignment = new Model.clsIssueAssignment();
                objBLIssueAssignment = new BusinessLayer.clsBLIssueAssignment();
                objIssueAssignment.ReportIssueID = Convert.ToInt32(lblIssueID.Text.Trim());
                strFromEmailID = objBLIssueAssignment.getToEmailID(objIssueAssignment);
                return strFromEmailID;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "getToEmailID", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private string getEmailSubForUser()
        {
            try
            {
                if (ddlStatus.SelectedValue == "2")
                {
                    strMailSubForUser = "Issue ID " + lblIssueID.Text + " In Progress.";
                }
                else if (ddlStatus.SelectedValue == "3")
                {
                    strMailSubForUser = "Issue ID " + lblIssueID.Text + " Resolved.";
                }
                else if (ddlStatus.SelectedValue == "4")
                {
                    strMailSubForUser = "Issue ID " + lblIssueID.Text + " Reopened.";
                }
                else if (ddlStatus.SelectedValue == "5")
                {
                    strMailSubForUser = "Issue ID " + lblIssueID.Text + " Assigned.";
                }
                else if (ddlStatus.SelectedValue == "6")
                {
                    strMailSubForUser = "Issue ID " + lblIssueID.Text + " on Hold.";
                }
                else if (ddlStatus.SelectedValue == "7")
                {
                    strMailSubForUser = "Issue ID " + lblIssueID.Text + " User Escalated.";
                }
                else if (ddlStatus.SelectedValue == "8")
                {
                    strMailSubForUser = "Issue ID " + lblIssueID.Text + " Closed.";
                }
                else if (ddlStatus.SelectedValue == "9")
                {
                    strMailSubForUser = "Issue ID " + lblIssueID.Text + " Cancelled.";
                }
                return strMailSubForUser;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "getEmailSubForUser", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private string getEmailBodyForUser()
        {
            try
            {
                strMailBodyForUser = "Hi " + lblIssueReportedBy.Text;
                if (ddlStatus.SelectedValue == "3")
                {
                    strMailBodyForUser += "<br>" + "<br>" + "The issue status has been changed to " + "<b>" + "Resolved" + "</b>" + " for the Issue ID " + "<b>" + lblIssueID.Text + "</b>";
                }
                else if (ddlStatus.SelectedValue == "4")
                {
                    strMailBodyForUser += "<br>" + "<br>" + "The issue status has been changed to " + "<b>" + "Reopened" + "</b>" + " for the Issue ID " + "<b>" + lblIssueID.Text + "</b>";
                }
                else if (ddlStatus.SelectedValue == "2")
                {
                    strMailBodyForUser += "<br>" + "<br>" + "The issue status has been changed to " + "<b>" + "In Progress" + "</b>" + " for the Issue ID " + "<b>" + lblIssueID.Text + "</b>";
                }
                else if (ddlStatus.SelectedValue == "5")
                {
                    strMailBodyForUser += "<br>" + "<br>" + "The issue status has been changed to " + "<b>" + "Assigned" + "</b>" + " for the Issue ID " + "<b>" + lblIssueID.Text + "</b>";
                }
                else if (ddlStatus.SelectedValue == "6")
                {
                    strMailBodyForUser += "<br>" + "<br>" + "The issue status has been changed to " + "<b>" + " on Hold" + "</b>" + " for the Issue ID " + "<b>" + lblIssueID.Text + "</b>";
                }
                else if (ddlStatus.SelectedValue == "7")
                {
                    strMailBodyForUser += "<br>" + "<br>" + "The issue status has been changed to " + "<b>" + "User Escalated" + "</b>" + " for the Issue ID " + "<b>" + lblIssueID.Text + "</b>";
                }
                else if (ddlStatus.SelectedValue == "8")
                {
                    strMailBodyForUser += "<br>" + "<br>" + "The issue status has been changed to " + "<b>" + "Closed" + "</b>" + " for the Issue ID " + "<b>" + lblIssueID.Text + "</b>";
                }
                else if (ddlStatus.SelectedValue == "9")
                {
                    strMailBodyForUser += "<br>" + "<br>" + "The issue status has been changed to " + "<b>" + "Cancelled" + "</b>" + " for the Issue ID " + "<b>" + lblIssueID.Text + "</b>";
                }

                strMailBodyForUser += "<br>" + "<b>" + "Problem Description: " + "</b>" + lblDescription.Text;
                if (txtFix.Text.Trim() != "")
                {
                    strMailBodyForUser += "<br>" + "<b>" + "Fix: " + "</b>" + txtFix.Text;
                }
                return strMailBodyForUser;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "getEmailBodyForUser", ex.StackTrace);
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

        private void SendMail()
        {
            try
            {
                objIssueAssignment = new Model.clsIssueAssignment();
                Department = Session["Department"].ToString();
                SmtpClient SmtpMail = new SmtpClient(ConfigurationSettings.AppSettings["SMTPServerName"].ToString());
                string strDeptEmailID = ConfigurationSettings.AppSettings[Department].ToString();

                MailMessage objSendMailToUser = new MailMessage();

                string CCCateory = Department + "CC";
                strDeptCCEmailID = ConfigurationSettings.AppSettings[CCCateory].ToString();
                string FromEmailID = getFromEmailID();
                string username = getUserName(FromEmailID);
                if (Department != "")
                {
                    //If there is change in status , then mail will triffer with follpwing body & subject.
                    if (Session["status"] != ddlStatus.SelectedValue)
                    {
                        objSendMailToUser.To.Add(new MailAddress(GetIssueRaiserEmailID()));
                        //objSendMailToUser.From = new MailAddress(getFromEmailID());
                        objSendMailToUser.From = new MailAddress(FromEmailID);

                        objSendMailToUser.Subject = "HelpDesk : Status of Issue " + lblIssueID.Text + " under " + Session["Department"].ToString() + " : "
                             + lblProblemType.Text.ToString() + " has been changed to " + ddlStatus.SelectedItem.Text.ToString() + ".";

                        objSendMailToUser.Body = "Hi, " + "<br>" + "<br>" +
                        "This is to inform you that, the issue status has been changed from " + "<b>" + Session["OldStatus"].ToString() +
                         "</b>" + " to " + "<b>" + ddlStatus.SelectedItem.Text.ToString() + "</b>" + "<br>" + " Issue details are as follows :" + "<br>" + "<br>" +

                         "<b>" + "Issue ID : " + "</b>" + "<b>" + Server.HtmlEncode(lblIssueID.Text.ToString()) + "</b>" + "<br>" + "<br>" +
                         "<b>" + "Department Name : " + "</b>" + Server.HtmlEncode(Session["Department"].ToString()) + "<br>" + "<br>" +
                         "<b>" + "Category Name : " + "</b>" + Server.HtmlEncode(lblProblemType.Text.ToString()) + "<br>" + "<br>" +
                         "<b>" + "Issue Severity : " + "</b>" + Server.HtmlEncode(lblProblemSeverity.Text.ToString()) + "<br>" + "<br>" +
                         "<b>" + "Current Status : " + "</b>" + Server.HtmlEncode(ddlStatus.SelectedItem.Text.ToString()) + "<br>" + "<br>" +
                         "<b>" + "Description : " + "</b>" + Server.HtmlEncode(lblDescription.Text.ToString()) + "." + "<br>" + "<br>" +
                          "<b>" + "Cause : " + "</b>" + Server.HtmlEncode(txtCause.Text.ToString()) + "." + "<br>" + "<br>" +
                         "<b>" + "Fix : " + "</b>" + Server.HtmlEncode(txtFix.Text.ToString()) + "." + "<br>" + "<br>" +
                         "<b>" + "Comment And Description : " + "</b>" + Server.HtmlEncode(txtAddcomment.Text.ToString()) + "." + "<br>" + "<br>" +
                         "Please take the necessary action." + "<br>" + "<br>" + "<br>" +
                          "Regards," + "<br>" + username.ToString();

                        objSendMailToUser.IsBodyHtml = true;

                        strDeptCCEmailID = strDeptCCEmailID + "," + FromEmailID.ToString();

                        if (strDeptCCEmailID.Contains(","))
                        {
                            string[] CCEmailId = strDeptCCEmailID.Split(',');
                            foreach (string email in CCEmailId)
                            {
                                objSendMailToUser.CC.Add(email);
                            }
                        }
                    }

                    // If there is only change in comment, then following mail body will trigger.
                    else
                    {
                        objSendMailToUser.To.Add(new MailAddress(GetIssueRaiserEmailID()));
                        objSendMailToUser.From = new MailAddress(FromEmailID);

                        objSendMailToUser.Subject = "HelpDesk : Issue " + lblIssueID.Text + " under " + Session["Department"].ToString() + " : "
                           + lblProblemType.Text.ToString() + " has a new comment. ";

                        objSendMailToUser.Body = "Hi, " + "<br>" + "<br>" +
                        "This is to inform you that, a new comment has been added. " + "<br>" + " Issue details are as follows :" + "<br>" + "<br>" +

                         "<b>" + "Issue ID : " + "</b>" + "<b>" + Server.HtmlEncode(lblIssueID.Text.ToString()) + "</b>" + "<br>" + "<br>" +
                         "<b>" + "Department Name : " + "</b>" + Server.HtmlEncode(Session["Department"].ToString()) + "<br>" + "<br>" +
                         "<b>" + "Category Name : " + "</b>" + Server.HtmlEncode(lblProblemType.Text.ToString()) + "<br>" + "<br>" +
                         "<b>" + "Issue Severity : " + "</b>" + Server.HtmlEncode(lblProblemSeverity.Text.ToString()) + "<br>" + "<br>" +
                         "<b>" + "Current Status : " + "</b>" + Server.HtmlEncode(ddlStatus.SelectedItem.Text.ToString()) + "<br>" + "<br>" +
                         "<b>" + "Description : " + "</b>" + Server.HtmlEncode(lblDescription.Text.ToString()) + "." + "<br>" + "<br>" +
                          "<b>" + "Cause : " + "</b>" + Server.HtmlEncode(txtCause.Text.ToString()) + "." + "<br>" + "<br>" +
                         "<b>" + "Fix : " + "</b>" + Server.HtmlEncode(txtFix.Text.ToString()) + "." + "<br>" + "<br>" +
                         "<b>" + "Comment And Description : " + "</b>" + Server.HtmlEncode(txtAddcomment.Text.ToString()) + "." + "<br>" + "<br>" +
                         "Please take the necessary action." + "<br>" + "<br>" + "<br>" +
                          "Regards," + "<br>" + username.ToString();

                        objSendMailToUser.IsBodyHtml = true;

                        strDeptCCEmailID = strDeptCCEmailID + "," + FromEmailID.ToString();

                        if (strDeptCCEmailID.Contains(","))
                        {
                            string[] CCEmailId = strDeptCCEmailID.Split(',');
                            foreach (string email in CCEmailId)
                            {
                                objSendMailToUser.CC.Add(email);
                            }
                        }
                    }

                    SmtpMail.UseDefaultCredentials = false;
                    SmtpMail.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), ConfigurationSettings.AppSettings["SMTPPassword"].ToString());
                    SmtpMail.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["PortNumber"].ToString());
                    SmtpMail.EnableSsl = true;
                    SmtpMail.Send(objSendMailToUser);
                }
                Session["UserEmailId"] = "";
                Session["Username"] = "";
                Session["OldStatus"] = "";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "SendMail", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void lnkbtnFileName_Click(object sender, System.EventArgs e)
        {
            try
            {
                string fileName = lnkbtnFileName.Text;
                string remoteHostName = Request.Headers["Host"].ToString();
                string resumePath = "http://" + remoteHostName + "/Uploads/" + fileName;
                //string resumePath = fileName;
                Response.Write("<script language='JavaScript'>" + '\n');
                Response.Write("val = window.open('" + resumePath + "')" + '\n');
                Response.Write("</script>" + '\n');
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "lnkbtnFileName_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnAddComment_Click(object sender, System.EventArgs e)
        {
            try
            {
                //lblColon1.Visible = true;
                lblComment1.Visible = true;
                txtAddcomment.Visible = true;
                btnAddComment.Visible = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "btnAddComment_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private string getUserName(string FromEmailID)
        {
            try
            {
                objIssueAssignment = new Model.clsIssueAssignment();
                objBLIssueAssignment = new BusinessLayer.clsBLIssueAssignment();
                objIssueAssignment.IssueAssignmentID = Convert.ToInt32(Session["IssueAssignmentID"]);
                strgetUserName = objBLIssueAssignment.getUserName(objIssueAssignment);
                return strgetUserName;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSelectedIssue.aspx", "getUserName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
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
    }
}