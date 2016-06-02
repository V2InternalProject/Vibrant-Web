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
    /// Summary description for MoveIssue.
    /// </summary>
    public partial class MoveIssue : System.Web.UI.Page
    {
        private clsViewMyIssues objViewMyIssues = new clsViewMyIssues();
        private clsBLViewMyIssues objBLViewMyIssues = new clsBLViewMyIssues();
        private int SAEmployeeID, EmployeeId, varEmployeeID;
        private DataSet dsEmployees = new DataSet();
        private string ReportId = "";
        private int f = 0;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Put user code to initialize the page here
                //code to clear all the cache so that after logout, previous page cannot be revisited.
                //--- starts here---------

                //--- ends here---------

                EmployeeId = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                //if (EmployeeId.ToString() == "" || EmployeeId == 0)
                //{
                if (SAEmployeeID.ToString() == "" || SAEmployeeID == 0)
                {
                    Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
                }
                else
                {
                    varEmployeeID = SAEmployeeID;
                }
                //}
                //else
                //{
                //    Response.Redirect("AuthorizationErrorMessage.aspx");
                //}
                if (!IsPostBack)
                {
                    LoadEmployees();
                    GetSuperAdminIssue();
                    ddlIssueMoveto.Visible = false;
                    lblIssuemoveTo.Visible = false;
                    lblCategory.Visible = false;
                    ddlCategory.Visible = false;
                    lblSuccessMessage.Visible = false;
                    btnGo.Visible = false;
                    btnMove.Visible = false;
                }
                btnMove.Attributes.Add("onclick", "return validateEmployee()");
                btnGo.Attributes.Add("onclick", "return validateCategory()");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "MoveIssue.aspx", "Page_Load", ex.StackTrace);
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
            this.dgViewIssues.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgViewIssues_ItemDataBound);
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
                    // ddlIssueMoveto.Items.Add(new ListItem("All","0"));
                    ddlIssueMoveFrom.Items.Add(new ListItem("Select Employee Name", "0"));
                    for (int i = 0; i < dsEmployees.Tables[0].Rows.Count; i++)
                    {
                        //	ddlIssueMoveto.Items.Add(new ListItem(dsEmployees.Tables[0].Rows[i]["EmployeeName"].ToString(),dsEmployees.Tables[0].Rows[i]["EmployeeID"].ToString()));
                        ddlIssueMoveFrom.Items.Add(new ListItem(dsEmployees.Tables[0].Rows[i]["EmployeeName"].ToString(), dsEmployees.Tables[0].Rows[i]["EmployeeID"].ToString()));
                    }

                    //				ddlAssignedEmployeees.DataSource=dsEmployees;
                    //				ddlAssignedEmployeees.DataValueField=dsEmployees.Tables[0].Columns["EmployeeName"].ColumnName.ToString();;
                    //				ddlAssignedEmployeees.DataTextField=dsEmployees.Tables[0].Columns["EmployeeName"].ColumnName.ToString();
                    //				ddlAssignedEmployeees.DataBind();
                    //	ddlIssueMoveto.SelectedIndex=1;
                    ddlIssueMoveFrom.SelectedIndex = 0;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MoveIssue.aspx", "LoadEmployees", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void GetSuperAdminIssue()
        {
            try
            {
                objViewMyIssues.EmployeeID = Convert.ToInt32(ddlIssueMoveFrom.SelectedValue);
                if (ddlCategory.SelectedValue == "")
                {
                    objViewMyIssues.SubCategoryId = 0;
                }
                else
                {
                    objViewMyIssues.SubCategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                }
                objViewMyIssues.LoginId = varEmployeeID;
                //objViewMyIssues.SelectedEmployee="";
                dsEmployees = objBLViewMyIssues.GetSuperAdminIssue(objViewMyIssues);
                dgViewIssues.DataSource = dsEmployees.Tables[0];
                if (dsEmployees.Tables[0].Rows.Count > 0)
                {
                    lblMsg.Visible = false;
                    lblSuccessMessage.Visible = false;
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
                    if (f == 1)
                    {
                        lblMsg.Text = "No records found";
                        lblMsg.Visible = true;
                        dgViewIssues.Visible = false;
                    }
                    else
                    {
                        f = 1;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "MoveIssue.aspx", "GetSuperAdminIssue", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void dgViewIssues_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            try
            {
                dgViewIssues.CurrentPageIndex = e.NewPageIndex;
                GetSuperAdminIssue();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MoveIssue.aspx", "dgViewIssues_PageIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void dgViewIssues_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                dgViewIssues.EditItemIndex = e.Item.ItemIndex;
                if (e.CommandName == "ShowIssueDetails")
                {
                    string IssueID = dgViewIssues.DataKeys[e.Item.ItemIndex].ToString();
                    int StatusID = Convert.ToInt32(((Label)e.Item.FindControl("lblIssueAssignmentID")).Text);
                    Session["ReportIssueID"] = IssueID;
                    Session["StatusID"] = StatusID;
                    Response.Redirect("ViewSelectedIssue.aspx");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MoveIssue.aspx", "dgViewIssues_ItemCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnMove_Click(object sender, System.EventArgs e)
        {
            try
            {
                foreach (DataGridItem dgi in dgViewIssues.Items)
                {
                    //CheckBox chkIsuue  = (CheckBox)dgi.Cells[0].FindControl("chkIsuue"));
                    CheckBox chkIssue = (CheckBox)dgi.Cells[0].FindControl("chkIsuue");

                    if (chkIssue.Checked == true)
                    {
                        Label IssueId = (Label)(dgi.Cells[0].FindControl("IssueId"));
                        ReportId = ReportId + IssueId.Text + ";";
                    }
                }
                objViewMyIssues.EmployeeID = Convert.ToInt32(ddlIssueMoveto.SelectedItem.Value);
                objViewMyIssues.ReportIssueIDStr = ReportId;
                objBLViewMyIssues.MoveIssue(objViewMyIssues);
                lblSuccessMessage.Text = "Issue moved Succesfully";
                GetSuperAdminIssue();
                lblSuccessMessage.Visible = true;
                //dgViewIssues.Visible=false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MoveIssue.aspx", "btnMove_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void ddlIssueMoveFrom_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                lblCategory.Visible = true;
                ddlCategory.Visible = true;
                lblSuccessMessage.Visible = false;
                btnGo.Visible = true;
                lblIssuemoveTo.Visible = false;
                btnMove.Visible = false;
                ddlIssueMoveto.Visible = false;
                dgViewIssues.Visible = false;
                ddlCategory.Items.Clear();
                objViewMyIssues.EmployeeID = Convert.ToInt32(ddlIssueMoveFrom.SelectedValue);
                dsEmployees = objBLViewMyIssues.BindCategory(objViewMyIssues);

                ddlCategory.Items.Add(new ListItem("Select", "0"));
                if (dsEmployees.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsEmployees.Tables[0].Rows.Count; i++)
                    {
                        //					ddlCategory.DataSource = dsEmployees.Tables[0];
                        //					ddlCategory.DataTextField = dsEmployees.Tables[0].Columns["SubCategory"].ToString();
                        //					ddlCategory.DataValueField = dsEmployees.Tables[0].Columns["SubCategoryID"].ToString();
                        //					ddlCategory.DataBind();
                        ddlCategory.Items.Add(new ListItem(dsEmployees.Tables[0].Rows[i]["SubCategory"].ToString(), dsEmployees.Tables[0].Rows[i]["SubCategoryID"].ToString()));
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "MoveIssue.aspx", "ddlIssueMoveFrom_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnGo_Click(object sender, System.EventArgs e)
        {
            try
            {
                lblSuccessMessage.Visible = false;
                lblIssuemoveTo.Visible = true;
                btnMove.Visible = true;
                ddlIssueMoveto.Visible = true;
                ddlIssueMoveto.Items.Clear();
                dgViewIssues.CurrentPageIndex = 0;
                objViewMyIssues.EmployeeID = Convert.ToInt32(ddlIssueMoveFrom.SelectedValue);
                objViewMyIssues.LoginId = varEmployeeID;
                if (ddlCategory.SelectedValue == "All")
                {
                    objViewMyIssues.Category = 0;
                }
                else

                    objViewMyIssues.SubCategoryId = Convert.ToInt32(ddlCategory.SelectedItem.Value);
                dsEmployees = objBLViewMyIssues.GetSuperAdminIssue(objViewMyIssues);
                dgViewIssues.DataSource = dsEmployees.Tables[0];
                ddlIssueMoveto.Items.Add(new ListItem("Select", "0"));
                if (dsEmployees.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < dsEmployees.Tables[1].Rows.Count; i++)
                    {
                        ddlIssueMoveto.Items.Add(new ListItem(dsEmployees.Tables[1].Rows[i]["EmployeeName"].ToString(), dsEmployees.Tables[1].Rows[i]["EmployeeID"].ToString()));
                    }
                }
                if (dsEmployees.Tables[0].Rows.Count > 0)
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
                    lblSuccessMessage.Visible = false;
                    dgViewIssues.Visible = false;
                    ddlIssueMoveto.Visible = false;
                    lblIssuemoveTo.Visible = false;
                    btnMove.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MoveIssue.aspx", "btnGo_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void dgViewIssues_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblStatusId = (Label)e.Item.FindControl("lblStatusId");
                    if (lblStatusId.Text == "1")
                    {
                        lblStatusId.Text = "New";
                    }
                    else if (lblStatusId.Text == "2")
                    {
                        lblStatusId.Text = "Resolved";
                    }
                    else if (lblStatusId.Text == "3")
                    {
                        lblStatusId.Text = "Moved";
                    }
                    else if (lblStatusId.Text == "4")
                    {
                        lblStatusId.Text = "Reopen";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "MoveIssue.aspx", "dgViewIssues_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}