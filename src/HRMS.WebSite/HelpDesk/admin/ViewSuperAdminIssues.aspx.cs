using HRMS;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;
using V2.Helpdesk.Model;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for ViewSuperAdminIssues.
    /// </summary>
    public partial class ViewSuperAdminIssues : System.Web.UI.Page
    {
        #region Variable declaration

        private DataSet dsIssueList;
        private DataSet dsEmployees;
        private clsViewMyIssues objViewMyIssues = new clsViewMyIssues();
        private clsBLViewMyIssues objBLViewMyIssues = new clsBLViewMyIssues();
        private clsMemberWiseSearchReport objClsMemberWiseSearchReport = new clsMemberWiseSearchReport();

        private clsBLMemberWiseSearchReport objClsBLMemberWiseSearchReport = new clsBLMemberWiseSearchReport();
        public int EmployeeID, SAEmployeeID, varEmployeeID, StatusSelected, OnlySuperAdmin, onlyExecutive;
        public string SelectedEmployee;
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        #endregion Variable declaration

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                string PageName = "Assign Issues";
                objpagelevel.PageLevelAccess(PageName);

                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                OnlySuperAdmin = Convert.ToInt32(Session["OnlySuperAdmin"]);
                onlyExecutive = Convert.ToInt32(Session["IsExecutive"]);
                if (OnlySuperAdmin != 0)
                {
                    Response.Redirect("AuthorizationErrorMessage.aspx?PageSource=SuperAdminIssue");
                }

                if ((SAEmployeeID.ToString() == "" || SAEmployeeID == 0) && OnlySuperAdmin == 0)
                {
                    Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
                }
                else
                {
                    if (OnlySuperAdmin == 0 && onlyExecutive != 1)
                        varEmployeeID = SAEmployeeID;
                    else
                        Response.Redirect("AuthorizationErrorMessage.aspx?PageSource=SuperAdminIssue");
                }

                if (!IsPostBack)
                {
                    GetStatus();
                    LoadEmployees();
                    LoadDepartment();
                    GetSuperAdminIssueList();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSuperAdminIssues.aspx", "Page_Load", ex.StackTrace);
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

        public void LoadEmployees()
        {
            try
            {
                objViewMyIssues.EmployeeID = varEmployeeID;
                dsEmployees = objBLViewMyIssues.GetSuperAdminEmployees(objViewMyIssues);
                if (dsEmployees.Tables[0].Rows.Count > 0)
                {
                    ddlAssignedEmployeees.Items.Add(new ListItem("All", ""));
                    for (int i = 0; i < dsEmployees.Tables[0].Rows.Count; i++)
                    {
                        ddlAssignedEmployeees.Items.Add(new ListItem(dsEmployees.Tables[0].Rows[i]["EmployeeName"].ToString(), dsEmployees.Tables[0].Rows[i]["EmployeeID"].ToString()));
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSuperAdminIssues.aspx", "LoadEmployees", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void LoadDepartment()
        {
            try
            {
                ddlDepartment.Items.Clear();
                DataSet dsDepartment = new DataSet();

                objViewMyIssues.EmployeeID = varEmployeeID;
                dsDepartment = objBLViewMyIssues.LoadDepartment(objViewMyIssues);
                if (dsDepartment.Tables[0].Rows.Count > 0)
                {
                    ddlDepartment.Items.Add(new ListItem("All", "0"));
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSuperAdminIssues.aspx", "LoadDepartment", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void GetStatus()
        {
            objClsMemberWiseSearchReport = new clsMemberWiseSearchReport();
            objClsBLMemberWiseSearchReport = new clsBLMemberWiseSearchReport();

            DataSet dsStatus = new DataSet();
            dsStatus = objClsBLMemberWiseSearchReport.GetStatus();

            for (int i = 0; i < dsStatus.Tables[0].Rows.Count; i++)
            {
                ddlStatus.Items.Add(new ListItem(dsStatus.Tables[0].Rows[i]["StatusDesc"].ToString(), dsStatus.Tables[0].Rows[i]["StatusID"].ToString()));
            }
            ddlStatus.Items.Insert(0, "All");
            ddlStatus.SelectedIndex = 1;
        }

        public void GetSuperAdminIssueList()
        {
            try
            {
                objViewMyIssues.EmployeeID = varEmployeeID;
                objViewMyIssues.SelectedStatus = ddlStatus.SelectedValue.ToString();
                objViewMyIssues.Category = Convert.ToInt32(ddlDepartment.SelectedValue);

                objViewMyIssues.SelectedEmployee = ddlAssignedEmployeees.SelectedValue;
                dsIssueList = objBLViewMyIssues.GetSuperAdminIssueList(objViewMyIssues);
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
                }
                else
                {
                    lblMsg.Text = "No records found";
                    lblMsg.Visible = true;
                    dgViewIssues.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSuperAdminIssues.aspx", "GetSuperAdminIssueList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgViewIssues_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                dgViewIssues.EditItemIndex = e.Item.ItemIndex;
                Label lblIssueAssignmentID;
                if (e.CommandName == "ShowIssueDetails")
                {
                    string IssueID = dgViewIssues.DataKeys[e.Item.ItemIndex].ToString();
                    Session["ReportIssueID"] = IssueID;
                    lblIssueAssignmentID = (Label)(e.Item.Cells[0].FindControl("lblIssueAssignmentID"));
                    Session["IssueAssignmentID"] = lblIssueAssignmentID.Text;

                    Response.Redirect("ViewSelectedIssueBySuperAdmin.aspx");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSuperAdminIssues.aspx", "dgViewIssues_ItemCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgViewIssues_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
        {
            try
            {
                dgViewIssues.CurrentPageIndex = e.NewPageIndex;
                GetSuperAdminIssueList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSuperAdminIssues.aspx", "dgViewIssues_PageIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            submitData();
        }

        public void submitData()
        {
            try
            {
                objViewMyIssues.EmployeeID = varEmployeeID;
                objViewMyIssues.SelectedStatus = ddlStatus.SelectedValue.ToString();
                objViewMyIssues.SelectedEmployee = ddlAssignedEmployeees.SelectedValue.ToString();
                objViewMyIssues.Category = Convert.ToInt32(ddlDepartment.SelectedValue);
                dsIssueList = objBLViewMyIssues.GetSuperAdminIssueList(objViewMyIssues);
                dgViewIssues.DataSource = dsIssueList.Tables[0];
                if (dsIssueList.Tables[0].Rows.Count > 0)
                {
                    txtIssueIDSearch.Text = "";
                    lblMsg.Visible = false;
                    dgViewIssues.Visible = true;
                    dgViewIssues.CurrentPageIndex = 0;
                    dgViewIssues.DataBind();
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
                    lblMsg.Text = "No records found";
                    lblMsg.Visible = true;
                    dgViewIssues.CurrentPageIndex = 0;
                    dgViewIssues.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSuperAdminIssues.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void lbtnMoveIssue_Click(object sender, System.EventArgs e)
        {
            try
            {
                Response.Redirect("MoveIssue.aspx");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSuperAdminIssues.aspx", "lbtnMoveIssue_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            submitData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtIssueIDSearch.Text == "")
            {
                submitData();
            }
            else
            {
                BindDataForSearch();
            }
        }

        public void BindDataForSearch()
        {
            int userVal = int.Parse(txtIssueIDSearch.Text);
            objViewMyIssues.ReportIssueID = userVal;

            dsIssueList = objBLViewMyIssues.SearchIssueIDData(objViewMyIssues);
            dgViewIssues.DataSource = dsIssueList.Tables[0];
            if (dsIssueList.Tables[0].Rows.Count > 0)
            {
                lblMsg.Visible = false;
                dgViewIssues.Visible = true;
                dgViewIssues.CurrentPageIndex = 0;
                dgViewIssues.DataBind();
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
                lblMsg.Text = "No records found";
                lblMsg.Visible = true;
                dgViewIssues.CurrentPageIndex = 0;
                dgViewIssues.Visible = false;
            }
        }

        public void dgViewIssues_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewSuperAdminIssues.aspx", "dgViewIssues_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}