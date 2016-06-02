using ModelHelpDeskBranch;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for ViewIssueStatus.
    /// </summary>
    public partial class ViewIssueStatus : System.Web.UI.Page
    {
        public int ReportIssueID;
        public int EmployeeID;
        private static int IssueID, SubCategoryID;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Put user code to initialize the page here
                //code to clear all the cache so that after logout, previous page cannot be revisited.
                //--- starts here---------

                //--- ends here---------
                int SAEmployeeID;
                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
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
                if (!Page.IsPostBack)
                {
                    //if(EmployeeID.ToString() == "" || EmployeeID == 0)
                    if (SAEmployeeID.ToString() == "" || SAEmployeeID == 0)
                    {
                        Response.Redirect("AuthorizationErrorMessage.aspx");
                    }
                    pnlIssueDetails.Visible = true;
                    pnlAddComments.Visible = false;
                    viewDetails();
                }
                btnReOpenIssue.Attributes.Add("onClick", "return isRequired('txtComments','Comments');");
                //lBtnBackToReports.Attributes.Add("onClick","history.back();");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewIssueStatus.aspx", "Page_Load", ex.StackTrace);
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

        protected void btnAddComments_ServerClick(object sender, System.EventArgs e)
        {
            try
            {
                txtComments.Text = "";
                pnlAddComments.Visible = true;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewIssueStatus.aspx", "btnAddComments_ServerClick", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void viewDetails()
        {
            try
            {
                clsBLViewMyStatus objClsBLViewMyStatus = new clsBLViewMyStatus();
                clsViewMyStatus objClsViewMyStatus = new clsViewMyStatus();
                IssueID = Convert.ToInt32(Session["IssueID"]);
                objClsViewMyStatus.IssueID = IssueID;
                objClsViewMyStatus.EmailID = "";
                BindIssueDetails();
                DataSet dsIssueDetails = objClsBLViewMyStatus.GetIssuesReport(objClsViewMyStatus);
                if (dsIssueDetails.Tables[0].Rows.Count > 0)
                {
                    lblIssueID.Text = dsIssueDetails.Tables[0].Rows[0]["ReportIssueID"].ToString();
                    lblReportedBy.Text = dsIssueDetails.Tables[0].Rows[0]["Name"].ToString();
                    lblReportedOn.Text = dsIssueDetails.Tables[0].Rows[0]["ReportIssueDate"].ToString();
                    lblSubCategory_Category.Text = dsIssueDetails.Tables[0].Rows[0]["SubCategory"].ToString();
                    lblSeverity.Text = dsIssueDetails.Tables[0].Rows[0]["ProblemSeverity"].ToString();
                    ////lblPriority.Text = dsIssueDetails.Tables[0].Rows[0]["ProblemPriority"].ToString();
                    lblDescription.Text = dsIssueDetails.Tables[0].Rows[0]["Description"].ToString();
                    lblReportStatus.Text = dsIssueDetails.Tables[0].Rows[0]["Status"].ToString();
                    // int intStatusID = Convert.ToInt32(dsIssueDetails.Tables[0].Rows[0]["Status"].ToString());
                    //if(intStatusID == 1)
                    //{
                    //    lblReportStatus.Text = IssueStatus.New.ToString();
                    //}
                    //else if(intStatusID == 2)
                    //{
                    //    lblReportStatus.Text = IssueStatus.Resolved.ToString();
                    //}
                    //else if(intStatusID == 3)
                    //{
                    //    lblReportStatus.Text = IssueStatus.Moved.ToString();
                    //}
                    //else if(intStatusID == 4)
                    //{
                    //    lblReportStatus.Text = IssueStatus.Reopen.ToString();
                    //}

                    //IssueID = Convert.ToInt32(dsIssueDetails.Tables[0].Rows[0]["IssueAssignmentID"]);
                    ReportIssueID = Convert.ToInt32(dsIssueDetails.Tables[0].Rows[0]["ReportIssueID"]);
                    //StatusID = Convert.ToInt32(dsIssueDetails.Tables[0].Rows[0]["StatusID"]);
                    SubCategoryID = Convert.ToInt32(dsIssueDetails.Tables[0].Rows[0]["SubCategoryID"]);
                    btnAddComments.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewIssueStatus.aspx", "viewDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void BindIssueDetails()
        {
            try
            {
                clsBLViewMyStatus objClsBLViewMyStatus = new clsBLViewMyStatus();
                clsViewMyStatus objClsViewMyStatus = new clsViewMyStatus();
                objClsViewMyStatus.IssueID = IssueID;
                DataSet dsIssueDetails1 = objClsBLViewMyStatus.GetIssueDetails(objClsViewMyStatus);
                pnlIssueDetails.Visible = true;
                if (dsIssueDetails1.Tables[0].Rows.Count > 0)
                {
                    //pnlDataGrid.Visible = true;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewIssueStatus.aspx", "BindIssueDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnReOpenIssue_Click(object sender, System.EventArgs e)
        {
            try
            {
                updateIssue();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewIssueStatus.aspx", "btnReOpenIssue_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void updateIssue()
        {
            try
            {
                clsBLViewMyStatus objClsBLViewMyStatus = new clsBLViewMyStatus();
                clsViewMyStatus objClsViewMyStatus = new clsViewMyStatus();
                objClsViewMyStatus.Comments = txtComments.Text;
                objClsViewMyStatus.IssueID = Convert.ToInt32(lblIssueID.Text);
                objClsViewMyStatus.SubCategoryID = SubCategoryID;
                int noOfRecordsUpdated = 0;
                noOfRecordsUpdated = objClsBLViewMyStatus.updateIssue(objClsViewMyStatus);
                if (noOfRecordsUpdated > 0)
                {
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

                        pnlAddComments.Visible = false;
                        lblMessage.Text = "Issue is Reopened.";
                        lblDescription.Text = dsIssueDetails1.Tables[0].Rows[0]["Description"].ToString();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewIssueStatus.aspx", "updateIssue", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgIssueDetails_PageChange(object sender, DataGridPageChangedEventArgs e)
        {
            try
            {
                dgIssueDetails.CurrentPageIndex = e.NewPageIndex;
                BindIssueDetails();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewIssueStatus.aspx", "dgIssueDetails_PageChange", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgIssueDetails_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                //foreach(DataGridItem dgi in dgIssueDetails.Items)
                //{
                //    if(dgi.ItemType == ListItemType.Item || dgi.ItemType == ListItemType.AlternatingItem)
                //    {
                //        //int intStatusID = Convert.ToInt32(((Label)dgi.FindControl("lblStatusID")).Text);
                //        //if(intStatusID == 1)
                //        //{
                //        //    ((Label)dgi.FindControl("lblStatus")).Text = IssueStatus.New.ToString();
                //        //}
                //        //else if(intStatusID == 2)
                //        //{
                //        //    ((Label)dgi.FindControl("lblStatus")).Text = IssueStatus.Resolved.ToString();
                //        //}
                //        //else if(intStatusID == 3)
                //        //{
                //        //    ((Label)dgi.FindControl("lblStatus")).Text = IssueStatus.Moved.ToString();
                //        //}
                //        //else if(intStatusID == 4)
                //        //{
                //        //    ((Label)dgi.FindControl("lblStatus")).Text = IssueStatus.Reopen.ToString();
                //        //}
                //    }
                //}
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewIssueStatus.aspx", "dgIssueDetails_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}