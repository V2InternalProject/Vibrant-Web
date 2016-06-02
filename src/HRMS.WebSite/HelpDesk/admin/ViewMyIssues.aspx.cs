using HRMS;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for ViewMyIssues.
    /// </summary>
    public partial class ViewMyIssues : System.Web.UI.Page
    {
        #region Variable declaration

        private DataSet dsIssueList;
        private Model.clsViewMyIssues objViewMyIssues;
        private BusinessLayer.clsBLViewMyIssues objBLViewMyIssues;
        public int EmployeeID, SAEmployeeID, varEmployeeID, SelectedStatus, OnlySuperAdmin;
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        #endregion Variable declaration

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                string PageName = "My Issues";
                objpagelevel.PageLevelAccess(PageName);

                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                OnlySuperAdmin = Convert.ToInt32(Session["OnlySuperAdmin"]);

                if (OnlySuperAdmin != 0)
                {
                    Response.Redirect("AuthorizationErrorMessage.aspx?PageSource=ViewMyIssue");
                }

                if ((SAEmployeeID.ToString() == "" || SAEmployeeID == 0) && OnlySuperAdmin == 0)
                {
                    Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
                }
                else
                {
                    if (OnlySuperAdmin == 0)
                        varEmployeeID = SAEmployeeID;
                    else
                        Response.Redirect("AuthorizationErrorMessage.aspx?PageSource=ViewMyIssue");
                }

                if (!IsPostBack)
                {
                    GetStatus();
                    GetMyIssueList();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyIssues.aspx", "Page_Load", ex.StackTrace);
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

        public void GetStatus()
        {
            objViewMyIssues = new Model.clsViewMyIssues();
            objBLViewMyIssues = new BusinessLayer.clsBLViewMyIssues();
            DataSet dsStatus = new DataSet();

            dsStatus = objBLViewMyIssues.GetStatus();

            for (int i = 0; i < dsStatus.Tables[0].Rows.Count; i++)
            {
                ddlStatus.Items.Add(new ListItem(dsStatus.Tables[0].Rows[i]["StatusDesc"].ToString(), dsStatus.Tables[0].Rows[i]["StatusID"].ToString()));
            }
            ddlStatus.Items.Insert(0, "All");
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Assigned"));
        }

        public void GetMyIssueList()
        {
            try
            {
                objViewMyIssues = new Model.clsViewMyIssues();
                objBLViewMyIssues = new BusinessLayer.clsBLViewMyIssues();

                objViewMyIssues.EmployeeID = varEmployeeID;
                objViewMyIssues.SelectedStatus = Convert.ToString(ddlStatus.SelectedValue.ToString());
                dsIssueList = objBLViewMyIssues.GetMyIssueList(objViewMyIssues);
                dgViewIssues.DataSource = dsIssueList.Tables[0];
                if (dsIssueList.Tables[0].Rows.Count > 0)
                {
                    lblMsg.Visible = false;
                    dgViewIssues.Visible = true;
                    dgViewIssues.DataBind();
                    if (dgViewIssues.PageCount > 1)
                    {
                        dgViewIssues.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgViewIssues.PagerStyle.Visible = false;
                    }

                    if (objViewMyIssues.SelectedStatus == "All" || objViewMyIssues.SelectedStatus == "1" || (objViewMyIssues.SelectedStatus == "2") || (objViewMyIssues.SelectedStatus == "3") || (objViewMyIssues.SelectedStatus == "4") || (objViewMyIssues.SelectedStatus == "5") || (objViewMyIssues.SelectedStatus == "6") || (objViewMyIssues.SelectedStatus == "7") || (objViewMyIssues.SelectedStatus == "8") || (objViewMyIssues.SelectedStatus == "9"))
                    {
                        dgViewIssues.Columns[1].Visible = false;
                        dgViewIssues.Columns[0].Visible = true;
                    }

                    if (dgViewIssues.PageCount > 1)
                    {
                        dgViewIssues.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgViewIssues.PagerStyle.Visible = false;
                    }
                }
                else
                {
                    dgViewIssues.Visible = false;
                    lblMsg.Visible = true;
                    lblMsg.Text = "No records found";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyIssues.aspx", "GetMyIssueList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgViewIssues_ItemCommand(object Sender, DataGridCommandEventArgs e)
        {
            try
            {
                dgViewIssues.EditItemIndex = e.Item.ItemIndex;
                if (e.CommandName == "ShowIssueDetails")
                {
                    string IssueID = dgViewIssues.DataKeys[e.Item.ItemIndex].ToString();
                    int StatusID = Convert.ToInt32(((Label)e.Item.FindControl("lblStatusID")).Text);
                    Session["ReportIssueID"] = IssueID; Session["StatusID"] = StatusID;
                    Response.Redirect("ViewSelectedIssue.aspx");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyIssues.aspx", "dgViewIssues_ItemCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgViewIssues_PageIndexChanged(object Sender, DataGridPageChangedEventArgs e)
        {
            try
            {
                dgViewIssues.CurrentPageIndex = e.NewPageIndex;
                GetMyIssueList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyIssues.aspx", "dgViewIssues_PageIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                GetMyIssueList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyIssues.aspx", "ddlStatus_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgViewIssues_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label RemainingTimeToGoTOAmberOrRed = (Label)(e.Item.FindControl("RemainingTimeToGoTOAmberOrRed"));
                    int totalMintutes = 0;
                    if (RemainingTimeToGoTOAmberOrRed.Text != "")
                        totalMintutes = Convert.ToInt32(RemainingTimeToGoTOAmberOrRed.Text.Trim());
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyIssues.aspx", "dgViewIssues_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}