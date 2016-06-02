//using ModelHelpdeskBranch;
using ModelHelpDeskBranch;
using System;
using System.Configuration;
using System.Data;

//using System.Web.Mail;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

//using HRMS.ModelHelpdeskBranch;
//using ModelHelpDesk;
using V2.Helpdesk.BusinessLayer;
using V2.Helpdesk.Model;

//using BusinessLayerHelpdeskBranch;
using V2.Helpdesk.web;

namespace HRMS.HelpDesk
{
    public partial class ViewMyStatus : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.DataGrid dgMyIssueList;
        protected System.Web.UI.WebControls.Button btnViewStatus;
        protected System.Web.UI.WebControls.Panel pnlMain;
        protected System.Web.UI.WebControls.Panel pnlAddComments;
        protected System.Web.UI.WebControls.Panel pnlDataGrid;
        protected System.Web.UI.HtmlControls.HtmlTextArea txtComments;
        protected System.Web.UI.WebControls.Button btnReOpenIssue;
        protected System.Web.UI.WebControls.Label lblTypeID;

        //protected System.Web.UI.WebControls.DropDownList ddlStatus;
        public DataSet dsIssueDetails;

        private static int SubCategoryID, StatusID;
        public int LoginID, ReportIssueID, maxLimit, EmployeeID, counter;
        public int IssueID;
        public string EmailID, Status, UserName;
        public string Password;
        protected System.Web.UI.WebControls.Button btnAddComments;
        protected System.Web.UI.WebControls.Button btnUpload;
        protected System.Web.UI.WebControls.Label lblError;
        private static string strReportStatus;
        protected System.Web.UI.HtmlControls.HtmlInputText txtDescCount;
        protected System.Web.UI.HtmlControls.HtmlInputHidden txtMaxLimit;
        protected System.Web.UI.WebControls.TextBox txtLoginID;
        protected System.Web.UI.WebControls.TextBox txtPassword;
        public string strEmailID;
        protected System.Web.UI.WebControls.Label lblMessage1;
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.Label lblIssueID;
        protected System.Web.UI.WebControls.Label lblReportedBy;
        protected System.Web.UI.WebControls.Label lblReportedOn;
        protected System.Web.UI.WebControls.Label lblSubCategory_Category;
        protected System.Web.UI.WebControls.Label lblSeverity;
        protected System.Web.UI.WebControls.Label lblDescription;
        protected System.Web.UI.HtmlControls.HtmlInputFile uploadFiles;
        protected System.Web.UI.WebControls.Label lblDepartment;
        protected System.Web.UI.WebControls.Label lblReportStatus;
        protected System.Web.UI.WebControls.DataGrid dgIssueDetails;
        protected System.Web.UI.WebControls.Panel pnlIssueDetails;
        protected System.Web.UI.WebControls.Label lblComment;
        protected System.Web.UI.WebControls.Label lblCommentDesc;
        protected System.Web.UI.WebControls.Label lblProjectName;
        protected System.Web.UI.WebControls.Label lblProjectRole;
        protected System.Web.UI.WebControls.Label lblWorkHours;
        protected System.Web.UI.WebControls.Label lblFromDate;
        protected System.Web.UI.WebControls.Label lblToDate;
        protected System.Web.UI.WebControls.Label lblNoOfResources;
        protected System.Web.UI.WebControls.Label lblResourcePool;
        protected System.Web.UI.WebControls.Label lblReportingTo;
        private DataSet dsIssueDetails1, dsUploadFile;
        public int ShouldSendMail, ShouldUploadFile;
        public int intIssueID, isuploaded, intReportIssueID;
        public string strMemberEmailID, strItDeptCCEmailID;
        private string strItDeptEmailID;
        private string fnInsert, fnUpload, fnExt;
        private static DataSet dsIssueDetailsNew;
        private static DataSet dsfileName;
        private static LinkButton lnkFileName;
        public int id;

        private void Page_Load(object sender, System.EventArgs e)
        {
            clsIssueAssignment objIssueAssignment = new clsIssueAssignment();
            clsBLIssueAssignment objBLIssueAssignment = new clsBLIssueAssignment();

            try
            {
                // Put user code to initialize the page here
                EmployeeID = Convert.ToInt32(User.Identity.Name);
                if (EmployeeID.ToString() == "" || EmployeeID == 0)
                {
                    Response.Redirect("http://192.168.30.15/intranet/");
                }
                else
                {
                    lblMessage1.Visible = false;
                    if (!Page.IsPostBack)
                    {
                        pnlIssueDetails.Visible = false;
                        pnlAddComments.Visible = false;
                        BindDataGrid();

                        //lblError.Visible=true;
                        //lblError.Text ="Login Id and Password are same as PMS";
                    }
                    uploadFiles.Disabled = false;
                    btnUpload.Enabled = true;
                    //btnViewStatus.Attributes.Add("onClick", "return validate();");
                    btnReOpenIssue.Attributes.Add("onClick", "return isRequire('txtComments','Comments');");
                }

                if (Convert.ToInt32(Session["IssueId"]) != 0)
                {
                    //objIssueAssignment.ReportIssueID = Convert.ToInt32(Session["IssueId"]);
                    //dsfileName = objBLIssueAssignment.FileName(objIssueAssignment);
                    //if (dsfileName.Tables[0].Rows.Count > 0)
                    //{
                    //    for (int i = 0, j = 0; i < dsfileName.Tables[0].Rows.Count; i++, j++)
                    //    {
                    //LinkButton lnk1 = new LinkButton();
                    pnlFileName.Controls.Clear();
                    lnkFileName = new LinkButton();
                    //lnkFileName.ID = "lnk2FileName1";
                    lnkFileName.Attributes.Add("runat", "server");
                    //this.lnk2.Click += new System.EventHandler(this.lnk2_Click);
                    //Session["FileNames"] = dsfileName.Tables[0].Rows[j]["FileName"].ToString();
                    if (Session["FileNames"] != "" && Session["FileNames"] != null)
                        lnkFileName.Text = Session["FileNames"].ToString();
                    //lnkFileName.Text = dsfileName.Tables[0].Rows[j]["FileName"].ToString();
                    //lnkFileName.Text = "</br>";
                    lnkFileName.Click += new System.EventHandler(this.lnkFileName_Click);

                    //if (pnlFileName.Controls.Contains(lnkFileName) == false)
                    //{
                    pnlFileName.Controls.Add(lnkFileName);
                    pnlFileName.Controls.Add(new LiteralControl("<br/>"));
                    //}

                    // Add control to container:
                    //pnlFileName.Controls.Add(lnk1);
                    //lblFileName.Text = dsfileName.Tables[0].Rows[0]["FileName"].ToString();
                    //    }
                    //}
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }

            //dsfileName = objClsBLReportIssue.InsertFile(objClsReportIssue);

            //pnlFileName.Visible = true;
            //pnlFileName.Attributes.Add("onclick", "this.disabled=false;");
            //lnkFileName.Text = Session["FileNames"].ToString();
            // lnkFileName.Visible = true;

            // btnSubmit.Attributes.Add("onclick", "return validate();");
            // btnMove.Attributes.Add("onClick", "return checkSubCategorySelection();");
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
            //this.btnViewStatus.Click += new System.EventHandler(this.btnViewStatus_Click);
            var lnkFileName = new LinkButton();
            pnlFileName.Controls.Add(lnkFileName);
            lnkFileName.ID = "lnk2FileName";
            lnkFileName.Attributes.Add("runat", "server");
            lnkFileName.Text = null;
            lnkFileName.Click += new System.EventHandler(this.lnkFileName_Click);

            this.Load += new System.EventHandler(this.Page_Load);
        }

        #endregion Web Form Designer generated code

        public void BindDataGrid()
        {
            clsBLViewMyStatus objClsBLViewMyStatus = new clsBLViewMyStatus();
            clsViewMyStatus objClsViewMyStatus = new clsViewMyStatus();
            try
            {
                objClsViewMyStatus.LoginID = Convert.ToInt32(User.Identity.Name);
                Session["LoginID"] = User.Identity.Name;
                // objClsViewMyStatus.Password = Password;
                // Session["Password"] = Password;

                dsIssueDetailsNew = objClsBLViewMyStatus.GetIssues(objClsViewMyStatus);
                int tableconunt = dsIssueDetailsNew.Tables.Count;
                if (dsIssueDetailsNew.Tables[tableconunt - 1].Rows.Count > 0)
                {
                    Session["Name"] = dsIssueDetailsNew.Tables[tableconunt - 1].Rows[0]["UserName"].ToString();
                }
                if (dsIssueDetailsNew.Tables[0].Rows.Count == 0)
                {
                    lblError.Visible = true;
                    lblError.Text = "No Record found.";
                    return;
                }
                else if (dsIssueDetailsNew.Tables[1].Rows.Count == 0)
                {
                    lblMessage1.Visible = true;
                    lblMessage1.Text = "No Record found.";
                }
                if (dsIssueDetailsNew.Tables[1].Rows.Count > 0)
                {
                    pnlDataGrid.Visible = true;
                    //Session["AssignedToEmailId"] = dsIssueDetailsNew.Tables[1].Rows[0]["AssignedToEmailId"].ToString();

                    Session["UserEmail"] = dsIssueDetailsNew.Tables[0].Rows[0]["Emp_Email"].ToString();
                    //pnlMain.Visible=false;
                    dgMyIssueList.DataSource = dsIssueDetailsNew.Tables[1];
                    dgMyIssueList.DataBind();
                    if (dgMyIssueList.PageCount > 1)
                    {
                        dgMyIssueList.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgMyIssueList.PagerStyle.Visible = false;
                    }
                }
                else
                {
                    pnlDataGrid.Visible = false;
                    //pnlMain.Visible = false;
                    lblMessage1.Visible = true;
                    lblMessage1.Text = "No Record found.";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "CheckParameterValues", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgMyIssueList_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            clsIssueAssignment objIssueAssignment = new clsIssueAssignment();
            clsBLIssueAssignment objBLIssueAssignment = new clsBLIssueAssignment();
            intReportIssueID = Convert.ToInt32(Session["ReportIssueID"]);
            try
            {
                if (e.CommandName == "viewDetails")
                {
                    Session["AssignedTo"] = "";
                    clsBLViewMyStatus objClsBLViewMyStatus = new clsBLViewMyStatus();
                    clsViewMyStatus objClsViewMyStatus = new clsViewMyStatus();
                    dgMyIssueList.EditItemIndex = e.Item.ItemIndex;
                    IssueID = Convert.ToInt32(dgMyIssueList.DataKeys[e.Item.ItemIndex]);
                    Session["IssueId"] = IssueID;
                    id = Convert.ToInt32(Session["IssueId"]);
                    objClsViewMyStatus.IssueID = IssueID;

                    pnlAddComments.Visible = false;
                    btnReOpenIssue.Visible = true;
                    btnAddComments.Visible = true;
                    uploadFiles.Disabled = false;
                    btnUpload.Visible = true;
                    lblMessage.Text = "";

                    BindIssueDetails();

                    if (dsIssueDetails1.Tables[1].Rows.Count > 0)
                    {
                        objIssueAssignment.ReportIssueID = Convert.ToInt32(Session["IssueId"]);
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
                        //lblTypeID.Text = dsIssueDetails1.Tables[1].Rows[0]["FileName"].ToString();
                        lblTypeID.Text = dsIssueDetails1.Tables[1].Rows[0]["Type"].ToString();
                        lblIssueID.Text = dsIssueDetails1.Tables[1].Rows[0]["ReportIssueID"].ToString();
                        lblReportedBy.Text = Server.HtmlEncode(dsIssueDetails1.Tables[1].Rows[0]["Name"].ToString());
                        lblReportedOn.Text = dsIssueDetails1.Tables[1].Rows[0]["ReportIssueDate"].ToString();
                        lblDepartment.Text = dsIssueDetails1.Tables[1].Rows[0]["Category"].ToString();
                        lblSubCategory_Category.Text = dsIssueDetails1.Tables[1].Rows[0]["SubCategory"].ToString();
                        lblSeverity.Text = dsIssueDetails1.Tables[1].Rows[0]["ProblemSeverity"].ToString();
                        //lblPriority.Text = dsIssueDetails1.Tables[1].Rows[0]["ProblemPriority"].ToString();
                        lblDescription.Text = Server.HtmlEncode(dsIssueDetails1.Tables[1].Rows[0]["Description"].ToString());
                        lblComment.Text = dsIssueDetails1.Tables[1].Rows[0]["Comments"].ToString();
                        lblCommentDesc.Text = dsIssueDetails1.Tables[1].Rows[0]["DescriptionAndComments"].ToString();
                        maxLimit = 1500 - Convert.ToInt32(lblDescription.Text.Length);
                        txtMaxLimit.Value = maxLimit.ToString();
                        txtDescCount.Value = maxLimit.ToString();
                        lblProjectName.Text = dsIssueDetails1.Tables[1].Rows[0]["projectName"].ToString();
                        lblProjectRole.Text = dsIssueDetails1.Tables[1].Rows[0]["RoleDescription"].ToString();
                        lblWorkHours.Text = dsIssueDetails1.Tables[1].Rows[0]["WorkHours"].ToString();
                        lblFromDate.Text = dsIssueDetails1.Tables[1].Rows[0]["FromDate"].ToString();
                        lblToDate.Text = dsIssueDetails1.Tables[1].Rows[0]["ToDate"].ToString();
                        lblNoOfResources.Text = dsIssueDetails1.Tables[1].Rows[0]["NumberOfResources"].ToString();
                        lblResourcePool.Text = dsIssueDetails1.Tables[1].Rows[0]["ResourcePoolName"].ToString();
                        lblReportingTo.Text = dsIssueDetails1.Tables[1].Rows[0]["employeename"].ToString();
                        //lblReportStatus.Text = dsIssueDetails.Tables[0].Rows[0]["Status"].ToString();
                        //IssueID = Convert.ToInt32(dsIssueDetails.Tables[0].Rows[0]["IssueAssignmentID"]);
                        ReportIssueID = Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["ReportIssueID"]);
                        StatusID = Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]);
                        lblReportStatus.Text = dsIssueDetails1.Tables[1].Rows[0]["Status"].ToString();
                        SubCategoryID = Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["SubCategoryID"]);

                        Session["SubCategoryId"] = dsIssueDetails1.Tables[1].Rows[0]["SubCategoryID"];

                        if (Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]) == 8)
                        {
                            btnAddComments.Visible = false;
                            pnlAddComments.Visible = false;
                            uploadFiles.Disabled = true;
                            btnUpload.Visible = false;
                        }
                        //Added by Rahul Ramachandran : Issue ID 3875
                        //Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]) == 3
                        if (lblReportStatus.Text.Trim() == "Closed")
                        {
                            btnAddComments.Visible = false;
                            btnUpload.Visible = false;
                            btnCloseIssue.Visible = false;
                            uploadFiles.Disabled = true;
                        }
                        else
                        {
                            btnAddComments.Visible = true;
                            btnUpload.Visible = true;
                            btnCloseIssue.Visible = true;
                            uploadFiles.Disabled = false;
                        }
                        if (Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]) == 1 ||
                         Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]) == 2 ||
                         Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]) == 5 ||
                         Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]) == 9 ||
                         Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]) == 4)
                        {
                            btnReOpenIssue.Visible = false;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "dgMyIssueList_ItemCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        //public void dgMyIssueList_ItemCommand(object sender, DataGridCommandEventArgs e)
        //{
        //    clsIssueAssignment objIssueAssignment = new clsIssueAssignment();
        //    clsBLIssueAssignment objBLIssueAssignment = new clsBLIssueAssignment();
        //    intReportIssueID = Convert.ToInt32(Session["ReportIssueID"]);
        //    try
        //    {
        //        if (e.CommandName == "viewDetails")
        //        {
        //            Session["AssignedTo"] = "";
        //            clsBLViewMyStatus objClsBLViewMyStatus = new clsBLViewMyStatus();
        //            clsViewMyStatus objClsViewMyStatus = new clsViewMyStatus();
        //            dgMyIssueList.EditItemIndex = e.Item.ItemIndex;
        //            IssueID = Convert.ToInt32(dgMyIssueList.DataKeys[e.Item.ItemIndex]);
        //            Session["IssueId"] = IssueID;
        //            id = Convert.ToInt32(Session["IssueId"]);
        //            objClsViewMyStatus.IssueID = IssueID;

        //            pnlAddComments.Visible = false;
        //            btnReOpenIssue.Visible = true;
        //            btnAddComments.Visible = true;
        //            uploadFiles.Disabled = false;
        //            btnUpload.Visible = true;
        //            lblMessage.Text = "";

        //            BindIssueDetails();

        //            if (dsIssueDetails1.Tables[1].Rows.Count > 0)
        //            {
        //                objIssueAssignment.ReportIssueID = Convert.ToInt32(Session["IssueId"]);
        //                dsfileName = objBLIssueAssignment.FileName(objIssueAssignment);
        //                if (dsfileName.Tables[0].Rows.Count > 0)
        //                {
        //                    for (int i = 0, j = 0; i < dsfileName.Tables[0].Rows.Count; i++, j++)
        //                    {
        //                        //LinkButton lnk1 = new LinkButton();
        //                        lnkFileName = new LinkButton();
        //                        //lnkFileName.ID = "lnk2FileName1";
        //                        lnkFileName.Attributes.Add("runat", "server");
        //                        //this.lnk2.Click += new System.EventHandler(this.lnk2_Click);
        //                        Session["FileNames"] = dsfileName.Tables[0].Rows[j]["FileName"].ToString();
        //                        lnkFileName.Text = dsfileName.Tables[0].Rows[j]["FileName"].ToString();
        //                        //lnkFileName.Text = "</br>";

        //                        lnkFileName.Click += new System.EventHandler(this.lnkFileName_Click);
        //                        pnlFileName.Controls.Add(lnkFileName);
        //                        pnlFileName.Controls.Add(new LiteralControl("<br/>"));

        //                        // Add control to container:
        //                        //pnlFileName.Controls.Add(lnk1);
        //                        //lblFileName.Text = dsfileName.Tables[0].Rows[0]["FileName"].ToString();
        //                    }
        //                }
        //                //lblTypeID.Text = dsIssueDetails1.Tables[1].Rows[0]["FileName"].ToString();
        //                lblTypeID.Text = dsIssueDetails1.Tables[1].Rows[0]["Type"].ToString();
        //                lblIssueID.Text = dsIssueDetails1.Tables[1].Rows[0]["ReportIssueID"].ToString();
        //                lblReportedBy.Text = Server.HtmlEncode(dsIssueDetails1.Tables[1].Rows[0]["Name"].ToString());
        //                lblReportedOn.Text = dsIssueDetails1.Tables[1].Rows[0]["ReportIssueDate"].ToString();
        //                lblDepartment.Text = dsIssueDetails1.Tables[1].Rows[0]["Category"].ToString();
        //                lblSubCategory_Category.Text = dsIssueDetails1.Tables[1].Rows[0]["SubCategory"].ToString();
        //                lblSeverity.Text = dsIssueDetails1.Tables[1].Rows[0]["ProblemSeverity"].ToString();
        //                //lblPriority.Text = dsIssueDetails1.Tables[1].Rows[0]["ProblemPriority"].ToString();
        //                lblDescription.Text = Server.HtmlEncode(dsIssueDetails1.Tables[1].Rows[0]["Description"].ToString());
        //                lblComment.Text = dsIssueDetails1.Tables[1].Rows[0]["Comments"].ToString();
        //                lblCommentDesc.Text = dsIssueDetails1.Tables[1].Rows[0]["DescriptionAndComments"].ToString();
        //                maxLimit = 1500 - Convert.ToInt32(lblDescription.Text.Length);
        //                txtMaxLimit.Value = maxLimit.ToString();
        //                txtDescCount.Value = maxLimit.ToString();
        //                lblProjectName.Text = dsIssueDetails1.Tables[1].Rows[0]["projectName"].ToString();
        //                lblProjectRole.Text = dsIssueDetails1.Tables[1].Rows[0]["RoleDescription"].ToString();
        //                lblWorkHours.Text = dsIssueDetails1.Tables[1].Rows[0]["WorkHours"].ToString();
        //                lblFromDate.Text = dsIssueDetails1.Tables[1].Rows[0]["FromDate"].ToString();
        //                lblToDate.Text = dsIssueDetails1.Tables[1].Rows[0]["ToDate"].ToString();
        //                lblNoOfResources.Text = dsIssueDetails1.Tables[1].Rows[0]["NumberOfResources"].ToString();
        //                lblResourcePool.Text = dsIssueDetails1.Tables[1].Rows[0]["ResourcePoolName"].ToString();
        //                lblReportingTo.Text = dsIssueDetails1.Tables[1].Rows[0]["employeename"].ToString();
        //                //lblReportStatus.Text = dsIssueDetails.Tables[0].Rows[0]["Status"].ToString();
        //                //IssueID = Convert.ToInt32(dsIssueDetails.Tables[0].Rows[0]["IssueAssignmentID"]);
        //                ReportIssueID = Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["ReportIssueID"]);
        //                StatusID = Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]);
        //                lblReportStatus.Text = dsIssueDetails1.Tables[1].Rows[0]["Status"].ToString();
        //                SubCategoryID = Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["SubCategoryID"]);

        //                Session["SubCategoryId"] = dsIssueDetails1.Tables[1].Rows[0]["SubCategoryID"];

        //                if (Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]) == 8)
        //                {
        //                    btnAddComments.Visible = false;
        //                    pnlAddComments.Visible = false;
        //                    uploadFiles.Disabled = true;
        //                    btnUpload.Visible = false;
        //                }

        //                if (Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]) == 1 ||
        //                 Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]) == 2 ||
        //                 Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]) == 5 ||
        //                 Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]) == 9 ||
        //                 Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]) == 4)
        //                {
        //                    btnReOpenIssue.Visible = false;
        //                }
        //            }

        //        }
        //    }

        //    catch (V2Exceptions ex)
        //    {
        //        throw;

        //    }
        //    catch (System.Exception ex)
        //    {
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "dgMyIssueList_ItemCommand", ex.StackTrace);
        //        throw new V2Exceptions(ex.ToString(),ex);
        //    }
        //}

        private void BindIssueDetails()
        {
            try
            {
                clsBLViewMyStatus objClsBLViewMyStatus = new clsBLViewMyStatus();
                clsViewMyStatus objClsViewMyStatus = new clsViewMyStatus();
                objClsViewMyStatus.IssueID = IssueID;
                dsIssueDetails1 = objClsBLViewMyStatus.GetIssueDetails(objClsViewMyStatus);
                pnlIssueDetails.Visible = true;
                if (dsIssueDetails1.Tables[0].Rows.Count > 0)
                {
                    //pnlDataGrid.Visible = true;
                    dgIssueDetails.Visible = true;
                    dgIssueDetails.DataSource = dsIssueDetails1.Tables[0];
                    dgIssueDetails.DataBind();
                    Session["AssignedTo"] = dsIssueDetails1.Tables[0].Rows[0]["EmployeeID"].ToString();
                    if (dgIssueDetails.PageCount > 1)
                    {
                        dgIssueDetails.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgIssueDetails.PagerStyle.Visible = false;
                    }
                }
                else
                {
                    dgIssueDetails.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "BindIssueDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void updateIssue()
        {
            try
            {
                clsBLViewMyStatus objClsBLViewMyStatus = new clsBLViewMyStatus();
                clsViewMyStatus objClsViewMyStatus = new clsViewMyStatus();
                // objIssueAssignment.AddComment = " [" + DateTime.Now + " ]" + txtAddcomment.Text + ".</br>";
                string name1 = User.Identity.Name.ToString();
                string name = "";
                DataSet dsEmpName = objClsBLViewMyStatus.GetEmployeeName(name1);
                var userloginid = dsEmpName.Tables[0].Rows[0]["EmployeeCode"].ToString();
                if (dsEmpName.Tables[0].Rows.Count > 0)
                {
                    name = dsEmpName.Tables[0].Rows[0]["EmployeeName"].ToString();
                }
                if (txtComments.Value.ToString() == "")
                {
                    objClsViewMyStatus.Comments = " ";
                }
                else
                {
                    objClsViewMyStatus.Comments = " [" + DateTime.Now + " ]" + ' ' + name + ':' + ' ' + txtComments.Value.ToString() + ".</br>";
                }
                //string name2 = HttpContext.Current.User.Identity.Name.ToString();
                objClsViewMyStatus.IssueID = Convert.ToInt32(lblIssueID.Text);
                objClsViewMyStatus.SubCategoryID = SubCategoryID;
                objClsViewMyStatus.StatusID = StatusID;
                int noOfRecordsUpdated = 0;

                //Session["UName"].ToString();
                noOfRecordsUpdated = objClsBLViewMyStatus.updateIssue1(objClsViewMyStatus, counter, userloginid);
                //if(noOfRecordsUpdated > 0)
                //{
                DataSet dsIssueDetails1 = objClsBLViewMyStatus.GetIssueDetails(objClsViewMyStatus);

                BindDataGrid();
                pnlIssueDetails.Visible = true;
                if (dsIssueDetails1.Tables[0].Rows.Count > 0)
                {
                    dgIssueDetails.DataSource = dsIssueDetails1.Tables[0];
                    dgIssueDetails.DataBind();
                    if (dgIssueDetails.PageCount > 1)
                    {
                        dgIssueDetails.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgIssueDetails.PagerStyle.Visible = false;
                    }

                    pnlAddComments.Visible = false;
                    //lblMessage.Text = "Issue is Reopened.";
                    lblDescription.Text = dsIssueDetails1.Tables[0].Rows[0]["Description"].ToString();
                    lblCommentDesc.Text = dsIssueDetails1.Tables[0].Rows[0]["descriptionAndComments"].ToString();
                    strReportStatus = lblReportStatus.Text.ToString().Trim();
                    StatusID = Convert.ToInt32(dsIssueDetails1.Tables[0].Rows[0]["StatusID"]);
                    Status = dsIssueDetails1.Tables[0].Rows[0]["Status"].ToString();
                    lblReportStatus.Text = Status;
                    //counter = 0;

                    if (Status == "Closed")
                    {
                        btnAddComments.Visible = false;
                        btnUpload.Visible = false;
                    }

                    if (Status == "ReOpened")
                    {
                        btnReOpenIssue.Visible = false;
                    }
                }
                else
                {
                    pnlAddComments.Visible = false;
                    //lblMessage.Text = "Issue is Reopened.";
                    lblDescription.Text = dsIssueDetails1.Tables[1].Rows[0]["Description"].ToString();
                    lblCommentDesc.Text = dsIssueDetails1.Tables[1].Rows[0]["DescriptionAndComments"].ToString();
                    strReportStatus = lblReportStatus.Text.ToString().Trim();
                    StatusID = Convert.ToInt32(dsIssueDetails1.Tables[1].Rows[0]["StatusID"]);
                    Status = dsIssueDetails1.Tables[1].Rows[0]["Status"].ToString();
                    lblReportStatus.Text = Status;
                    //counter = 0;
                    if (Status == "Closed")
                    {
                        btnAddComments.Visible = false;
                        btnUpload.Visible = false;
                    }

                    if (Status == "ReOpened")
                    {
                        btnReOpenIssue.Visible = false;
                    }
                }
                //	}
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "updateIssue", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnAddComments_ServerClick(object sender, System.EventArgs e)
        {
            try
            {
                txtComments.Value = "";
                pnlAddComments.Visible = true;
                lblMessage.Text = "";
                if (lblReportStatus.Text == "Reopen" || lblReportStatus.Text == "New")
                {
                    btnReOpenIssue.Text = "Submit";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "btnAddComments_ServerClick", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgMyIssueList_PageChange(object sender, DataGridPageChangedEventArgs e)
        {
            try
            {
                pnlIssueDetails.Visible = false;
                pnlAddComments.Visible = false;
                dgMyIssueList.CurrentPageIndex = e.NewPageIndex;
                // CheckParameterValues();
                BindDataGrid();

                lblMessage.Visible = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "dgMyIssueList_PageChange", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgIssueDetails_pageChange(object sender, DataGridPageChangedEventArgs e)
        {
            try
            {
                clsBLViewMyStatus objClsBLViewMyStatus = new clsBLViewMyStatus();
                clsViewMyStatus objClsViewMyStatus = new clsViewMyStatus();
                dgIssueDetails.CurrentPageIndex = e.NewPageIndex;
                objClsViewMyStatus.IssueID = Convert.ToInt32(lblIssueID.Text);
                DataSet dsIssueDetails1 = objClsBLViewMyStatus.GetIssueDetails(objClsViewMyStatus);
                pnlIssueDetails.Visible = true;
                if (dsIssueDetails1.Tables[0].Rows.Count > 0)
                {
                    dgIssueDetails.DataSource = dsIssueDetails1.Tables[0];
                    dgIssueDetails.DataBind();
                    if (dgIssueDetails.PageCount > 1)
                    {
                        dgIssueDetails.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgIssueDetails.PagerStyle.Visible = false;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "dgIssueDetails_pageChange", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void dgIssuDetails_ItemBound(object sender, DataGridItemEventArgs e)
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
                throw;

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "dgIssuDetails_ItemBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void dgMyIssueList_ItemBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.EditItem)
                {
                    string intStatusID = ((Label)e.Item.FindControl("lblCurrentStatusID")).Text;
                    string lblIssueDescription = ((Label)e.Item.FindControl("lblIssueDescription")).Text;

                    Label RemainingTimeToGoTOAmberOrRed = (Label)(e.Item.FindControl("RemainingTimeToGoTOAmberOrRed"));

                    int totalMintutes = Convert.ToInt32(RemainingTimeToGoTOAmberOrRed.Text.Trim());
                    int hrs, mins; string displaytime;
                    hrs = totalMintutes / 60;

                    mins = totalMintutes % 60;
                    if (mins < 10)
                    {
                        displaytime = Convert.ToString(hrs) + ":" + "0" + Convert.ToString(mins);
                    }
                    else
                    {
                        displaytime = Convert.ToString(hrs) + ":" + Convert.ToString(mins);
                    }
                    if (hrs < 0)
                    {
                        displaytime = "0:00";
                    }
                    RemainingTimeToGoTOAmberOrRed.Text = displaytime;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "dgMyIssueList_ItemBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void sendMail()
        {
            try
            {
                if (isuploaded == 1)
                {
                    SmtpClient SmtpMail = new SmtpClient(ConfigurationSettings.AppSettings["SMTPServerName"].ToString());
                    clsReportIssue objClsReportIssue = new clsReportIssue();
                    objClsReportIssue.SubCategoryID = Convert.ToInt32(Session["SubCategoryId"]);
                    Session["OldStatus"] = lblReportStatus.Text.ToString();
                    // Session["AssignedToEmailId"] = dsIssueDetailsNew.Tables[1].Rows[0]["AssignedToEmailId"].ToString();
                    //Session["AssignedToEmailId"] = dsIssueDetailsNew.Tables[0].Rows[0]["EmailID"].ToString();
                    string Category = GetCategoryName(objClsReportIssue).TrimStart(' ').TrimEnd(' ');
                    string AssignedToEmailID = "";

                    if (Category == "")
                    {
                        strItDeptEmailID = ConfigurationSettings.AppSettings["IT_Dept_EmailID"].ToString();
                    }
                    else
                    {
                        //Category= Category.ToUpper();
                        strItDeptEmailID = ConfigurationSettings.AppSettings[Category].ToString();
                        string CCCateory = Category + "CC";
                        strItDeptCCEmailID = ConfigurationSettings.AppSettings[CCCateory].ToString();
                    }

                    if (Session["AssignedTo"].ToString() != "0" && Session["AssignedTo"].ToString() != "")
                    {
                        objClsReportIssue.EmployeeID = Convert.ToInt32(Session["AssignedTo"]);
                        AssignedToEmailID = GetAssignedToEmailID(objClsReportIssue);
                    }

                    //= Convert.ToString(Session["AssignedTo"]);

                    MailMessage reopenSentToMember = new MailMessage();

                    string fromEmailId = Convert.ToString(Session["UserEmail"]);
                    if (fromEmailId != string.Empty)
                        reopenSentToMember.From = new MailAddress(fromEmailId);
                    //}
                    if (AssignedToEmailID != string.Empty)
                    {
                        //send mail to assigned person
                        reopenSentToMember.To.Add(new MailAddress(AssignedToEmailID));
                        //reopenSentToMember.CC.Add(strItDeptEmailID);
                    }
                    else
                    {
                        //sendmail to department head
                        reopenSentToMember.To.Add(new MailAddress(strItDeptEmailID));
                    }

                    if (strItDeptCCEmailID != string.Empty)
                    {
                        strItDeptCCEmailID = strItDeptCCEmailID + "," + fromEmailId;

                        if (strItDeptCCEmailID.Contains(","))
                        {
                            string[] CCEmailId = strItDeptCCEmailID.Split(',');
                            foreach (string email in CCEmailId)
                            {
                                reopenSentToMember.CC.Add(new MailAddress(email));
                            }
                        }
                    }

                    //if File is uploaded,then mail body  & subject will be as follow.
                    reopenSentToMember.Subject = "HelpDesk : Issue " + lblIssueID.Text + " under " + lblDepartment.Text.ToString() + " : "
                         + lblSubCategory_Category.Text.ToString() + " has a new File.";

                    reopenSentToMember.Body = "Hi, " + "<br>" + "<br>" +
                      "This is to inform you that, a new File has been Uploaded." + "<br>" + " Issue details are as follows :" + "<br>" + "<br>" +

                       "<b>" + "Issue ID : " + "</b>" + "<b>" + Server.HtmlEncode(lblIssueID.Text.ToString()) + "</b>" + "<br>" + "<br>" +
                       "<b>" + "Department Name : " + "</b>" + Server.HtmlEncode(lblDepartment.Text.ToString()) + "<br>" + "<br>" +
                       "<b>" + "Category Name : " + "</b>" + Server.HtmlEncode(lblSubCategory_Category.Text.ToString()) + "<br>" + "<br>" +
                       "<b>" + "Issue Severity : " + "</b>" + Server.HtmlEncode(lblSeverity.Text.ToString()) + "<br>" + "<br>" +
                       "<b>" + "Current Status : " + "</b>" + Session["OldStatus"].ToString() + "<br>" + "<br>" +
                       "<b>" + "Description : " + "</b>" + Server.HtmlEncode(lblDescription.Text.ToString()) + "." + "<br>" + "<br>" +
                       "Please take the necessary action." + "<br>" + "<br>" + "<br>" +
                        "Regards," + "<br>" + Session["Name"].ToString();

                    reopenSentToMember.IsBodyHtml = true;
                    ////reopenSentToMember.Priority = MailPriority.Normal;
                    //SmtpMail.UseDefaultCredentials = false;
                    // SmtpMail.Credentials = new System.Net.NetworkCredential("v2system", "mail_123");
                    //SmtpMail.Credentials = new System.Net.NetworkCredential("username", "password");

                    SmtpMail.UseDefaultCredentials = false;
                    SmtpMail.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), ConfigurationSettings.AppSettings["SMTPPassword"].ToString());
                    SmtpMail.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["PortNumber"].ToString());
                    SmtpMail.EnableSsl = true;
                    SmtpMail.Send(reopenSentToMember);

                    //SmtpMail.Send(reopenSentToMember);
                    Session["OldStatus"] = "";
                }
                else
                {
                    string currentIssueStatus = string.Empty;
                    string AssignedToEmailId = string.Empty;

                    if (counter == 1)
                        currentIssueStatus = "ReOpened";
                    else
                        currentIssueStatus = "Closed";

                    //  MailMessage reopenSentToUser = new MailMessage();
                    SmtpClient SmtpMail = new SmtpClient(ConfigurationSettings.AppSettings["SMTPServerName"].ToString());
                    clsReportIssue objClsReportIssue = new clsReportIssue();
                    objClsReportIssue.SubCategoryID = Convert.ToInt32(Session["SubCategoryId"]);
                    string Category = GetCategoryName(objClsReportIssue).TrimStart(' ').TrimEnd(' ');
                    if (Category == "")
                    {
                        strItDeptEmailID = ConfigurationSettings.AppSettings["IT_Dept_EmailID"].ToString();
                    }
                    else
                    {
                        //Category= Category.ToUpper();
                        strItDeptEmailID = ConfigurationSettings.AppSettings[Category].ToString();
                        string CCCateory = Category + "CC";
                        strItDeptCCEmailID = ConfigurationSettings.AppSettings[CCCateory].ToString();
                    }

                    if (Session["AssignedTo"].ToString() != "0" && Session["AssignedTo"].ToString() != "")
                    {
                        objClsReportIssue.EmployeeID = Convert.ToInt32(Session["AssignedTo"]);
                        AssignedToEmailId = GetAssignedToEmailID(objClsReportIssue);
                    }

                    //= Convert.ToString(Session["AssignedTo"]);

                    MailMessage reopenSentToMember = new MailMessage();

                    string fromEmailId = Convert.ToString(Session["UserEmail"]);
                    if (fromEmailId != string.Empty)
                        reopenSentToMember.From = new MailAddress(fromEmailId);
                    //}
                    if (AssignedToEmailId != string.Empty)
                    {
                        //send mail to assigned person
                        reopenSentToMember.To.Add(new MailAddress(AssignedToEmailId));
                        //reopenSentToMember.CC.Add(strItDeptEmailID);
                    }
                    else
                    {
                        //sendmail to department head
                        reopenSentToMember.To.Add(new MailAddress(strItDeptEmailID));
                    }

                    if (strItDeptCCEmailID != string.Empty)
                    {
                        strItDeptCCEmailID = strItDeptCCEmailID + "," + fromEmailId;

                        if (strItDeptCCEmailID.Contains(","))
                        {
                            string[] CCEmailId = strItDeptCCEmailID.Split(',');
                            foreach (string email in CCEmailId)
                            {
                                reopenSentToMember.CC.Add(new MailAddress(email));
                            }
                        }
                    }

                    //if only comment is added , then mail body  & subject will be as follow.
                    if (counter == 3)
                    {
                        reopenSentToMember.Subject = "HelpDesk : Issue " + lblIssueID.Text + " under " + lblDepartment.Text.ToString() + " : "
                         + lblSubCategory_Category.Text.ToString() + " has a new comment.";

                        reopenSentToMember.Body = "Hi, " + "<br>" + "<br>" +
                      "This is to inform you that, a new comment has been added." + "<br>" + " Issue details are as follows :" + "<br>" + "<br>" +

                       "<b>" + "Issue ID : " + "</b>" + "<b>" + Server.HtmlEncode(lblIssueID.Text.ToString()) + "</b>" + "<br>" + "<br>" +
                       "<b>" + "Department Name : " + "</b>" + Server.HtmlEncode(lblDepartment.Text.ToString()) + "<br>" + "<br>" +
                       "<b>" + "Category Name : " + "</b>" + Server.HtmlEncode(lblSubCategory_Category.Text.ToString()) + "<br>" + "<br>" +
                       "<b>" + "Issue Severity : " + "</b>" + Server.HtmlEncode(lblSeverity.Text.ToString()) + "<br>" + "<br>" +
                       "<b>" + "Current Status : " + "</b>" + Session["OldStatus"].ToString() + "<br>" + "<br>" +
                       "<b>" + "Description : " + "</b>" + Server.HtmlEncode(lblDescription.Text.ToString()) + "." + "<br>" + "<br>" +
                       "<b>" + "Comment : " + "</b>" + Server.HtmlEncode(txtComments.InnerText.ToString()) + "." + "<br>" + "<br>" +
                       "Please take the necessary action." + "<br>" + "<br>" + "<br>" +
                        "Regards," + "<br>" + Session["Name"].ToString();
                    }
                    //if the issue is reopend or closed, then following will bw the mail body & subject.
                    else
                    {
                        reopenSentToMember.Subject = "HelpDesk : Status of Issue " + lblIssueID.Text + " under " + lblDepartment.Text.ToString() + " : "
                       + lblSubCategory_Category.Text.ToString() + " has been changed to " + currentIssueStatus + ".";

                        reopenSentToMember.Body = "Hi, " + "<br>" + "<br>" +
                        "This is to inform you that, the issue status has been changed from " + "<b>" + Session["OldStatus"].ToString() +
                         "</b>" + " to " + "<b>" + currentIssueStatus + "</b>" + "<br>" + " Issue details are as follows :" + "<br>" + "<br>" +

                         "<b>" + "Issue ID : " + "</b>" + "<b>" + Server.HtmlEncode(lblIssueID.Text.ToString()) + "</b>" + "<br>" + "<br>" +
                         "<b>" + "Department Name : " + "</b>" + Server.HtmlEncode(lblDepartment.Text.ToString()) + "<br>" + "<br>" +
                         "<b>" + "Category Name : " + "</b>" + Server.HtmlEncode(lblSubCategory_Category.Text.ToString()) + "<br>" + "<br>" +
                         "<b>" + "Issue Severity : " + "</b>" + Server.HtmlEncode(lblSeverity.Text.ToString()) + "<br>" + "<br>" +
                         "<b>" + "Current Status : " + "</b>" + currentIssueStatus + "<br>" + "<br>" +
                         "<b>" + "Description : " + "</b>" + Server.HtmlEncode(lblDescription.Text.ToString()) + "." + "<br>" + "<br>" +
                         "Please take the necessary action." + "<br>" + "<br>" + "<br>" +
                          "Regards," + "<br>" + Session["Name"].ToString();
                    }

                    reopenSentToMember.IsBodyHtml = true;
                    ////reopenSentToMember.Priority = MailPriority.Normal;
                    //SmtpMail.UseDefaultCredentials = false;
                    //// SmtpMail.Credentials = new System.Net.NetworkCredential("v2system", "mail_123");
                    //SmtpMail.Credentials = new System.Net.NetworkCredential("username", "password");
                    //SmtpMail.Send(reopenSentToMember);

                    SmtpMail.UseDefaultCredentials = false;
                    SmtpMail.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), ConfigurationSettings.AppSettings["SMTPPassword"].ToString());
                    SmtpMail.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["PortNumber"].ToString());
                    SmtpMail.EnableSsl = true;
                    SmtpMail.Send(reopenSentToMember);

                    Session["OldStatus"] = "";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = ex.Message;
                lblMessage.Visible = false;
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "sendMail", ex.StackTrace);
                //throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public string GetCategoryName(clsReportIssue objclsReportIssue)
        {
            try
            {
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                return objClsBLReportIssue.GetCategoryName(objclsReportIssue);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "GetCategoryName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public string GetAssignedToEmailID(clsReportIssue objClsReportIssue)
        {
            try
            {
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                return objClsBLReportIssue.GetAssignedToEmailID(objClsReportIssue);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "GetAssignedToEmailID", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private string getEmailID()
        {
            try
            {
                clsBLViewMyStatus objClsBLViewMyStatus = new clsBLViewMyStatus();
                clsViewMyStatus objClsViewMyStatus = new clsViewMyStatus();
                objClsViewMyStatus.IssueID = Convert.ToInt32(lblIssueID.Text.ToString().Trim());
                strEmailID = objClsBLViewMyStatus.getEmailID(objClsViewMyStatus);
                return strEmailID;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "getEmailID", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnAddComments_Click(object sender, System.EventArgs e)
        {
            try
            {
                uploadFiles.Disabled = true;
                btnUpload.Enabled = false;

                txtComments.Value = "";
                pnlAddComments.Visible = true;
                lblMessage.Text = "";
                if (lblReportStatus.Text == "Reopen" || lblReportStatus.Text == "New")
                {
                    btnReOpenIssue.Text = "Submit";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "btnAddComments_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnReOpenIssue_Click(object sender, System.EventArgs e)
        {
            try
            {
                Session["OldStatus"] = lblReportStatus.Text.ToString();
                counter = 1;
                updateIssue();
                if (Status == "ReOpened" || Status == "Closed")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Your issue has been Re-Opened.";
                    sendMail();
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Your Comment is added.";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "btnReOpenIssue_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnCloseIssue_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtComments.InnerText == "")
                {
                    lblMessage1.Text = "Please enter Comments before closing issue.";
                    lblMessage1.Visible = true;
                }
                else
                {
                    Session["OldStatus"] = lblReportStatus.Text.ToString();
                    counter = 2;
                    updateIssue();

                    if (Status == "ReOpened" || Status == "Closed")
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Your issue has been Closed.";
                        sendMail();
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Your Comment is added.";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "btnReOpenIssue_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSaveComments_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtComments.InnerText == "")
                {
                    lblMessage1.Text = "Please enter Comments before saving";
                    lblMessage1.Visible = true;
                }
                else
                {
                    Session["OldStatus"] = lblReportStatus.Text.ToString();
                    counter = 3;
                    updateIssue();
                    if (Status != "ReOpened" || Status != "Closed")
                    {
                        lblMessage.Text = "Your Comment is added.";
                        lblMessage.Visible = true;
                        sendMail();
                    }
                    else
                    {
                        lblMessage1.Text = "Error occured while adding comment";
                        lblMessage1.Visible = true;
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (uploadFiles.Value == "")
            {
                lblMessage1.Visible = true;
                lblMessage1.Text = "Please Select File to Upload";
            }
            else
            {
                isuploaded = 1;
                InsertFile();
                if (ShouldUploadFile == 1)
                    UploadFiles();

                if (ShouldSendMail == 1)
                    sendMail();

                lblMessage.Text = "File Successfully Uploaded. An email will be sent to you confirming the Uploading of the File.";
            }
        }

        private void InsertFile()
        {
            try
            {
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                clsReportIssue objClsReportIssue = new clsReportIssue();
                IssueStatus objIssueStatus = new IssueStatus();
                clsViewMyStatus objClsViewMyStatus = new clsViewMyStatus();
                intIssueID = 0;
                IssueID = Convert.ToInt32(Session["IssueId"].ToString());
                objClsReportIssue.ReportIssueID = IssueID;

                if ((uploadFiles.PostedFile != null) && (uploadFiles.PostedFile.ContentLength > 0))
                {
                    //fnUpload=dsUploadFile.Tables[1].Rows[0]["filename"].ToString();
                    fnInsert = System.IO.Path.GetFileNameWithoutExtension(uploadFiles.PostedFile.FileName);
                    fnExt = System.IO.Path.GetExtension(uploadFiles.PostedFile.FileName);
                    objClsReportIssue.UploadedFileName = fnInsert.ToString();
                    objClsReportIssue.UploadedFileExtension = fnExt.ToString();
                }
                dsUploadFile = objClsBLReportIssue.InsertFile(objClsReportIssue);
                fnUpload = dsUploadFile.Tables[1].Rows[0]["filename"].ToString();

                if (dsUploadFile.Tables[0].Rows.Count > 0)
                {
                    intIssueID = Convert.ToInt32(dsUploadFile.Tables[0].Rows[0]["ReportIssueID"].ToString());
                    lblMessage.Visible = true;
                    lblMessage.CssClass = "success";
                    lblMessage.Text = "File Successfully Uploaded.";
                    ShouldSendMail = 1;
                    ShouldUploadFile = 1;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.CssClass = "Error";
                    lblMessage.Text = "Error occured in uploading the File ,Please try again";
                    ShouldSendMail = 0;
                    ShouldUploadFile = 0;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "InsertIssueDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void UploadFiles()
        {
            try
            {
                if ((uploadFiles.PostedFile != null) && (uploadFiles.PostedFile.ContentLength > 0))
                {
                    string SaveLocation = ConfigurationSettings.AppSettings["UploadedfilePath"].ToString();
                    try
                    {
                        uploadFiles.PostedFile.SaveAs(SaveLocation + "\\" + fnUpload + fnExt);
                        lblMessage.Visible = true;
                        lblMessage.CssClass = "success";
                        lblMessage.Text = "The file has been uploaded.";
                    }
                    catch (V2Exceptions ex)
                    {
                        throw;
                    }
                    catch (System.Exception ex)
                    {
                        lblMessage.Visible = true;
                        lblMessage.CssClass = "success";
                        lblMessage.Text = "Error: " + ex.Message;

                        FileLog objFileLog = FileLog.GetLogger();
                        objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "UploadFiles", ex.StackTrace);
                        throw new V2Exceptions(ex.ToString(), ex);
                    }
                }
                /*else
                {
                    //Response.Write("Please select a file to upload.");
                }*/
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "UploadFiles", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}