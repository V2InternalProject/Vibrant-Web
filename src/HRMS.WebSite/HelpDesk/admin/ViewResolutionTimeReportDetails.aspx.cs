using ModelHelpDeskBranch;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for ViewResolutionTimeReportDetails.
    /// </summary>
    public partial class ViewResolutionTimeReportDetails : System.Web.UI.Page
    {
        //protected System.Web.UI.WebControls.Label lblStatus;
        public int ReportIssueID, EmployeeID, SAEmployeeID;

        private static int IssueID, SubCategoryID;

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
                    viewDetails();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewResolutionTimeReportDetails.aspx", "Page_Load", ex.StackTrace);
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
                    lblSubCategory.Text = dsIssueDetails.Tables[0].Rows[0]["SubCategory"].ToString();
                    lblSeverity.Text = dsIssueDetails.Tables[0].Rows[0]["ProblemSeverity"].ToString();
                    //// lblPriority.Text = dsIssueDetails.Tables[0].Rows[0]["ProblemPriority"].ToString();
                    lblDescription.Text = dsIssueDetails.Tables[0].Rows[0]["Description"].ToString();
                    int intStatusID = Convert.ToInt32(dsIssueDetails.Tables[0].Rows[0]["StatusID"]);
                    lblReportStatus.Text = dsIssueDetails.Tables[0].Rows[0]["Status"].ToString();
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
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewResolutionTimeReportDetails.aspx", "viewDetails", ex.StackTrace);
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
                //pnlIssueDetails.Visible = true;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewResolutionTimeReportDetails.aspx", "BindIssueDetails", ex.StackTrace);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewResolutionTimeReportDetails.aspx", "dgIssueDetails_PageChange", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgIssueDetails_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                //foreach (DataGridItem dgi in dgIssueDetails.Items)
                //{
                //    if (dgi.ItemType == ListItemType.Item || dgi.ItemType == ListItemType.AlternatingItem)
                //    {
                //        int intStatusID = Convert.ToInt32(((Label)dgi.FindControl("lblStatusID")).Text);
                //        Label lblStatus = (Label)dgi.FindControl("lblStatus");
                //        if (intStatusID == 1)
                //        {
                //            lblStatus.Text = IssueStatus.New.ToString();
                //        }
                //        else if (intStatusID == 2)
                //        {
                //            lblStatus.Text = IssueStatus.Resolved.ToString();
                //        }
                //        else if (intStatusID == 3)
                //        {
                //            lblStatus.Text = IssueStatus.Moved.ToString();
                //        }
                //        else if (intStatusID == 4)
                //        {
                //            lblStatus.Text = IssueStatus.Reopen.ToString();
                //        }
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewResolutionTimeReportDetails.aspx", "dgIssueDetails_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}