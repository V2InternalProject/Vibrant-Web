using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Workflow.Runtime;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;
using V2.Orbit.Workflow.SignInSignOutWF;

namespace HRMS.Orbitweb
{
    public partial class SignInSignOutApproval : System.Web.UI.Page
    {
        // declaring local variables
        private SignInSignOutModel objSignInSignOutModel = new SignInSignOutModel();

        private SignInSignOutBOL objSignInSignOutBOL = new SignInSignOutBOL();
        private DataSet dsSignInSignOutApproval = new DataSet();
        private bool strDropdown = false;
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string PageName = "SignInOut Approval";
                objpagelevel.PageLevelAccess(PageName);

                if (!IsPostBack)
                {
                    ViewState["ColumnName"] = "date";
                    ViewState["Order"] = "DESC";
                    objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                    objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();
                    txtSearchFromDate.Attributes.Add("onkeydown", "return false");
                    txtSearchToDate.Attributes.Add("onkeydown", "return false");
                    GetStatus();

                    BindData();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.aspx.cs", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void BindData()
        {
            try
            {
                objSignInSignOutModel.EmployeeID = Convert.ToInt32(User.Identity.Name);
                objSignInSignOutModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);//pending
                if (!strDropdown)
                {
                    objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                    objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();
                }
                else
                {
                    ViewState["ColumnName"] = "date";
                    ViewState["Order"] = "DESC";

                    objSignInSignOutModel.ColumnName = "date";
                    objSignInSignOutModel.SortOrder = "DESC";
                }

                dsSignInSignOutApproval = objSignInSignOutBOL.BindApprovalData(objSignInSignOutModel);
                if (dsSignInSignOutApproval.Tables[0].Rows.Count == 0)
                {
                    lblErrorMess.Text = "No records found.";
                    grvSISOApproval.Visible = false;
                    txtSearchFromDate.Text = "";
                    txtSearchToDate.Text = "";
                }
                else
                {
                    lblErrorMess.Text = "";
                    grvSISOApproval.Visible = true;
                    grvSISOApproval.DataSource = dsSignInSignOutApproval.Tables[0];
                    grvSISOApproval.DataBind();
                    txtSearchFromDate.Text = "";
                    txtSearchToDate.Text = "";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.aspx.cs", "BindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #region getStatus

        public void GetStatus()
        {
            try
            {
                DataSet dsGetStatus = new DataSet();
                dsGetStatus = objSignInSignOutBOL.GetStatus();
                for (int i = 0; i < dsGetStatus.Tables[0].Rows.Count; i++)
                {
                    if (dsGetStatus.Tables[0].Rows[i]["StatusID"].ToString() != "4")
                        ddlStatus.Items.Add(new ListItem(dsGetStatus.Tables[0].Rows[i]["StatusName"].ToString(), dsGetStatus.Tables[0].Rows[i]["StatusID"].ToString()));
                }
                ddlStatus.Items.Add(new ListItem("All", "0"));
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.aspx.cs", "getStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion getStatus

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
        }

        protected void grvSISOApproval_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                lblErrorMess.Text = "";
                lblSuccess.Text = "";
                grvSISOApproval.EditIndex = e.NewEditIndex;
                BindData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.aspx.cs", "grvSISOApproval_RowEditing", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grvSISOApproval.PageIndex = 0;
                lblSuccess.Text = "";
                lblErrorMess.Text = "";
                strDropdown = true;
                BindData();
                strDropdown = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.aspx.cs", "ddlStatus_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void grvSISOApproval_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Label lblDate = (Label)grvSISOApproval.Rows[e.RowIndex].FindControl("lblDate");
                string strDate = lblDate.Text.ToString();
                //for workflow...
                Label lblWorkflowID = ((Label)grvSISOApproval.Rows[e.RowIndex].FindControl("lblSignInSignOutWFID"));
                // To write code if "" then dont fill value in model for WorkflowID
                if (lblWorkflowID.Text != "")
                    objSignInSignOutModel.WorkflowID = new Guid(lblWorkflowID.Text);

                //Status
                DropDownList ddlStatus1 = (DropDownList)grvSISOApproval.Rows[e.RowIndex].FindControl("ddlStatusdEdit");
                objSignInSignOutModel.StatusID = Convert.ToInt32(ddlStatus1.SelectedValue);
                //InTime
                DropDownList ddlSignInTimeH1 = (DropDownList)grvSISOApproval.Rows[e.RowIndex].FindControl("ddlInTimeHours");
                DropDownList ddlSignInTimeM1 = (DropDownList)grvSISOApproval.Rows[e.RowIndex].FindControl("ddlInTimeMins");
                string strInTime = ddlSignInTimeH1.SelectedValue.ToString() + ":" + ddlSignInTimeM1.SelectedValue.ToString() + ":00";

                objSignInSignOutModel.SignInTime = Convert.ToDateTime(strInTime + " " + strDate.ToString());

                //Out Time
                Label lblOutDate = (Label)grvSISOApproval.Rows[e.RowIndex].FindControl("lblOutDate");
                string strOutDate = lblOutDate.Text.ToString();
                Label lblOutTimeH1 = (Label)grvSISOApproval.Rows[e.RowIndex].FindControl("lblSignOutTime");
                if (lblOutTimeH1.Text.ToString() != "")
                {
                    DropDownList ddlOutTimeHours = (DropDownList)grvSISOApproval.Rows[e.RowIndex].FindControl("ddlOutTimeHours");
                    DropDownList ddlOutTimeMinutes = (DropDownList)grvSISOApproval.Rows[e.RowIndex].FindControl("ddlOutTimeMins");
                    string strOutTime = ddlOutTimeHours.SelectedValue.ToString() + ":" + ddlOutTimeMinutes.SelectedValue.ToString() + ":00";
                    objSignInSignOutModel.SignOutTime = Convert.ToDateTime(strOutTime + " " + strOutDate.ToString());
                }
                else
                {
                    objSignInSignOutModel.SignOutTime = Convert.ToDateTime(null);
                }

                //Approver's comments
                TextBox txtApproversComments = (TextBox)grvSISOApproval.Rows[e.RowIndex].FindControl("txtApproversComments");
                objSignInSignOutModel.ApproverComments = txtApproversComments.Text.ToString();
                //Id
                Label lblID = (Label)grvSISOApproval.Rows[e.RowIndex].FindControl("lblID");
                objSignInSignOutModel.SignInSignOutID = Convert.ToInt32(lblID.Text);
                if ((objSignInSignOutModel.SignOutTime < objSignInSignOutModel.SignInTime) && (lblOutTimeH1.Text.ToString() != ""))
                {
                    lblErrorMess.Text = "Sign Out Time cannot be smaller than the Sign In Time";
                }
                else
                {
                    lblErrorMess.Text = "";
                    UpdateStatus(objSignInSignOutModel);

                    // StartWorkflow

                    if (objSignInSignOutModel.StatusID == 2)
                    {
                        lblSuccess.Text = "Record updated successfully.";
                        UpdateEmployeeLeaveAndComp();
                        try
                        {
                            WorkflowRuntime wr = (WorkflowRuntime)Application["WokflowRuntime"];
                            if (objSignInSignOutModel.WorkflowID != null || objSignInSignOutModel.WorkflowID.ToString() != "")
                            {
                                WorkflowInstance wi = wr.GetWorkflow(objSignInSignOutModel.WorkflowID);
                                wi.Resume();
                                SignInSignOutService objSignInSignOutService = (SignInSignOutService)wr.GetService(typeof(SignInSignOutService));
                                objSignInSignOutService.RaiseApproveEvent(wi.InstanceId);
                            }
                        }
                        catch (V2Exceptions)
                        {
                            try
                            {
                                Response.Redirect("SignInSignOutApproval.aspx");
                            }
                            catch (System.Threading.ThreadAbortException ex)
                            {
                                throw;
                            }
                        }
                        catch (System.Exception)
                        {
                            try
                            {
                                Response.Redirect("SignInSignOutApproval.aspx");
                            }
                            catch (System.Threading.ThreadAbortException ex)
                            {
                                throw;
                            }
                        }
                    }
                    if (objSignInSignOutModel.StatusID == 3)
                    {
                        lblSuccess.Text = "Record updated successfully.";
                        try
                        {
                            WorkflowRuntime wr = (WorkflowRuntime)Application["WokflowRuntime"];
                            if (objSignInSignOutModel.WorkflowID != null || objSignInSignOutModel.WorkflowID.ToString() != "")
                            {
                                WorkflowInstance wi = wr.GetWorkflow(objSignInSignOutModel.WorkflowID);
                                wi.Resume();
                                SignInSignOutService objSignInSignOutService = (SignInSignOutService)wr.GetService(typeof(SignInSignOutService));
                                objSignInSignOutService.RaiseRejectEvent(wi.InstanceId);
                            }
                        }
                        catch (V2Exceptions)
                        {
                            try
                            {
                                Response.Redirect("SignInSignOutApproval.aspx");
                            }
                            catch (System.Threading.ThreadAbortException ex)
                            {
                                throw;
                            }
                        }
                        catch (System.Exception)
                        {
                            try
                            {
                                Response.Redirect("SignInSignOutApproval.aspx");
                            }
                            catch (System.Threading.ThreadAbortException ex)
                            {
                                throw;
                            }
                        }
                    }
                    // End WorkFlow

                    grvSISOApproval.EditIndex = -1;
                    Response.Redirect("SignInSignOutApproval.aspx");
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                throw;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.aspx.cs", "grvSISOApproval_RowUpdating", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void UpdateStatus(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                objSignInSignOutBOL.UpdateStatus(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.cs", "UpdateStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void grvSISOApproval_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //hiding Edit link for records with date < config date
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DateTime dtTable = Convert.ToDateTime(dsSignInSignOutApproval.Tables[0].Rows[e.Row.DataItemIndex]["date"].ToString());
                    DateTime dtConfig = Convert.ToDateTime(dsSignInSignOutApproval.Tables[1].Rows[0]["ConfigItemValue"].ToString());
                    if (dtConfig > dtTable)
                    {
                        Label lblStatus1 = (Label)e.Row.FindControl("lblStatus1");
                        LinkButton lnkEdit = (LinkButton)e.Row.FindControl("LinkButton1");
                        lnkEdit.Visible = false;
                        Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                        lblStatus.Visible = true;
                        lblStatus.Text = lblStatus1.Text.ToString();
                    }
                    //hiding Edit link for approved records
                    if ((dsSignInSignOutApproval.Tables[0].Rows[e.Row.RowIndex]["Status"].ToString() == "Approved") || (dsSignInSignOutApproval.Tables[0].Rows[e.Row.RowIndex]["Status"].ToString() == "Rejected"))
                    {
                        Label lblStatus1 = (Label)e.Row.FindControl("lblStatus1");
                        LinkButton lnkEdit = (LinkButton)e.Row.FindControl("LinkButton1");
                        lnkEdit.Visible = false;
                        Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                        lblStatus.Visible = true;
                        lblStatus.Text = lblStatus1.Text.ToString();
                    }
                    Label lblUserName = (Label)e.Row.FindControl("lblUserName");
                    if (lblUserName != null)
                    {
                        //saturday/sunday and public holiday - BOLD
                        if (dsSignInSignOutApproval.Tables[0].Rows[e.Row.RowIndex]["date"] != null)
                        {
                            DateTime date1 = Convert.ToDateTime(dsSignInSignOutApproval.Tables[0].Rows[e.Row.DataItemIndex]["date"]);
                            string strDate = date1.ToShortDateString();
                            Label lblDate2 = (Label)e.Row.FindControl("lblDate1");
                            lblDate2.Text = strDate;

                            if ((date1.DayOfWeek.ToString() == "Sunday") || (date1.DayOfWeek.ToString() == "Saturday"))
                            {
                                lblUserName.Text = "<b>" + lblUserName.Text + "</b>";
                                Label lblDate1 = (Label)e.Row.FindControl("lblDate1");
                                lblDate1.Text = "<b>" + lblDate1.Text + "</b>";
                                Label lblSignInTime1 = (Label)e.Row.FindControl("lblSignInTime1");
                                lblSignInTime1.Text = "<b>" + lblSignInTime1.Text + "</b>";
                                Label lblSignOutTime1 = (Label)e.Row.FindControl("lblSignOutTime1");
                                lblSignOutTime1.Text = "<b>" + lblSignOutTime1.Text + "</b>";
                                Label lblTotalHours = (Label)e.Row.FindControl("lblTotalHours");
                                lblTotalHours.Text = "<b>" + lblTotalHours.Text + "</b>";
                                Label lblMode = (Label)e.Row.FindControl("lblMode");
                                lblMode.Text = "<b>" + lblMode.Text + "</b>";
                                Label lblSignInComment = (Label)e.Row.FindControl("lblSignInComment");
                                lblSignInComment.Text = "<b>" + lblSignInComment.Text + "</b>";
                                Label lblSignOutComment = (Label)e.Row.FindControl("lblSignOutComment");
                                lblSignOutComment.Text = "<b>" + lblSignOutComment.Text + "</b>";
                                Label lblStatus1 = (Label)e.Row.FindControl("lblStatus1");
                                lblStatus1.Text = "<b>" + lblStatus1.Text + "</b>";
                                Label lblApproversComments = (Label)e.Row.FindControl("lblApproversComments");
                                lblApproversComments.Text = "<b>" + lblApproversComments.Text + "</b>";
                                Label lblLastModified1 = (Label)e.Row.FindControl("lblLastModified");
                                lblLastModified1.Text = "<b>" + lblLastModified1.Text + "</b>";
                            }
                            else
                            {
                                for (int i = 0; i < dsSignInSignOutApproval.Tables[2].Rows.Count; i++)
                                {
                                    DateTime date2 = Convert.ToDateTime(dsSignInSignOutApproval.Tables[2].Rows[i]["HolidayDate"].ToString());
                                    if (date1.Date == date2.Date)
                                    {
                                        lblUserName.Text = "<b>" + lblUserName.Text + "</b>";
                                        Label lblDate1 = (Label)e.Row.FindControl("lblDate1");
                                        lblDate1.Text = "<b>" + lblDate1.Text + "</b>";
                                        Label lblSignInTime1 = (Label)e.Row.FindControl("lblSignInTime1");
                                        lblSignInTime1.Text = "<b>" + lblSignInTime1.Text + "</b>";
                                        Label lblSignOutTime1 = (Label)e.Row.FindControl("lblSignOutTime1");
                                        lblSignOutTime1.Text = "<b>" + lblSignOutTime1.Text + "</b>";
                                        Label lblTotalHours = (Label)e.Row.FindControl("lblTotalHours");
                                        lblTotalHours.Text = "<b>" + lblTotalHours.Text + "</b>";
                                        Label lblMode = (Label)e.Row.FindControl("lblMode");
                                        lblMode.Text = "<b>" + lblMode.Text + "</b>";
                                        Label lblSignInComment = (Label)e.Row.FindControl("lblSignInComment");
                                        lblSignInComment.Text = "<b>" + lblSignInComment.Text + "</b>";
                                        Label lblSignOutComment = (Label)e.Row.FindControl("lblSignOutComment");
                                        lblSignOutComment.Text = "<b>" + lblSignOutComment.Text + "</b>";
                                        Label lblStatus1 = (Label)e.Row.FindControl("lblStatus1");
                                        lblStatus1.Text = "<b>" + lblStatus1.Text + "</b>";
                                        Label lblApproversComments = (Label)e.Row.FindControl("lblApproversComments");
                                        lblApproversComments.Text = "<b>" + lblApproversComments.Text + "</b>";
                                    }
                                }
                            }
                        }
                    }
                }

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    if (dsSignInSignOutApproval.Tables[0].Rows[e.Row.RowIndex]["date"] != null)
                    {
                        DateTime date1 = Convert.ToDateTime(dsSignInSignOutApproval.Tables[0].Rows[e.Row.DataItemIndex]["date"]);
                        string strDate = date1.ToShortDateString();
                        Label lblDate2 = (Label)e.Row.FindControl("lblDate");
                        lblDate2.Text = strDate;
                    }

                    //In Time hours

                    Label lblSignInTime = (Label)e.Row.FindControl("lblSignInTime");
                    DropDownList ddlSignInTimeH1 = (DropDownList)e.Row.FindControl("ddlInTimeHours");
                    for (int i = 00; i <= 23; i++)
                    {
                        if (i.ToString().Length == 1)
                        {
                            ddlSignInTimeH1.Items.Add(new ListItem("0" + i.ToString(), "0" + i.ToString()));
                        }
                        else
                        {
                            ddlSignInTimeH1.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                    }
                    string strInTime = lblSignInTime.Text.ToString();
                    string[] strArrayInTime = strInTime.Split(':');
                    ddlSignInTimeH1.SelectedValue = strArrayInTime[0].ToString();

                    //In time mins
                    DropDownList ddlSignInTimeM1 = (DropDownList)e.Row.FindControl("ddlInTimeMins");
                    for (int i = 00; i <= 59; i++)
                    {
                        if (i.ToString().Length == 1)
                        {
                            ddlSignInTimeM1.Items.Add(new ListItem("0" + i.ToString(), "0" + i.ToString()));
                        }
                        else
                        {
                            ddlSignInTimeM1.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                    }
                    ddlSignInTimeM1.SelectedValue = strArrayInTime[1].ToString();

                    //out time hours
                    Label lblOutTimeH1 = (Label)e.Row.FindControl("lblSignOutTime");
                    string strOutTime = lblOutTimeH1.Text.ToString();
                    string[] strArrayOutTime = strOutTime.Split(':');

                    //Out time Hours Dropdown
                    DropDownList ddlOutTimeHours = (DropDownList)e.Row.FindControl("ddlOutTimeHours");
                    for (int i = 00; i <= 23; i++)
                    {
                        if (i.ToString().Length == 1)
                        {
                            ddlOutTimeHours.Items.Add(new ListItem("0" + i.ToString(), "0" + i.ToString()));
                        }
                        else
                        {
                            ddlOutTimeHours.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                    }

                    //out time minutes
                    DropDownList ddlOutTimeMinutes = (DropDownList)e.Row.FindControl("ddlOutTimeMins");
                    for (int i = 00; i <= 59; i++)
                    {
                        if (i.ToString().Length == 1)
                        {
                            ddlOutTimeMinutes.Items.Add(new ListItem("0" + i.ToString(), "0" + i.ToString()));
                        }
                        else
                        {
                            ddlOutTimeMinutes.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                    }
                    if (lblOutTimeH1.Text.ToString() != "")
                    {
                        ddlOutTimeMinutes.SelectedValue = strArrayOutTime[1].ToString();
                        ddlOutTimeHours.SelectedValue = strArrayOutTime[0].ToString();
                    }
                    else
                    {
                        ddlOutTimeHours.Visible = false;
                        ddlOutTimeMinutes.Visible = false;
                    }

                    //status dropdown
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    DropDownList ddlStatus1 = (DropDownList)e.Row.FindControl("ddlStatusdEdit");
                    DataSet dsSignInSignOutApproval1 = objSignInSignOutBOL.GetStatus();
                    for (int i = 0; i < dsSignInSignOutApproval1.Tables[0].Rows.Count; i++)
                    {
                        if (dsSignInSignOutApproval1.Tables[0].Rows[i]["StatusName"].ToString() != "Cancelled")
                        {
                            ddlStatus1.Items.Add(new ListItem(dsSignInSignOutApproval1.Tables[0].Rows[i]["StatusName"].ToString(), dsSignInSignOutApproval1.Tables[0].Rows[i]["StatusID"].ToString()));
                        }
                    }

                    ddlStatus1.SelectedValue = lblStatus.Text.ToString();
                    TextBox txtApproversComments = (TextBox)e.Row.FindControl("txtApproversComments");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.aspx.cs", "grvSISOApproval_RowDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                grvSISOApproval.PageIndex = 0;
                objSignInSignOutModel.EmployeeID = Convert.ToInt32(User.Identity.Name);
                objSignInSignOutModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                objSignInSignOutModel.Todate = Convert.ToDateTime(txtSearchToDate.Text);
                objSignInSignOutModel.FromDate = Convert.ToDateTime(txtSearchFromDate.Text);

                ViewState["ColumnName"] = "date";
                ViewState["Order"] = "DESC";

                objSignInSignOutModel.ColumnName = "date";
                objSignInSignOutModel.SortOrder = "DESC";

                dsSignInSignOutApproval = SearchApproval(objSignInSignOutModel);
                if (dsSignInSignOutApproval.Tables[0].Rows.Count == 0)
                {
                    lblErrorMess.Text = "No records Found.";
                    grvSISOApproval.Visible = false;
                }
                else
                {
                    lblErrorMess.Text = "";
                    grvSISOApproval.Visible = true;
                    grvSISOApproval.DataSource = dsSignInSignOutApproval;
                    grvSISOApproval.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.aspx.cs", "btnSearch_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public DataSet SearchApproval(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                return objSignInSignOutBOL.SearchApproval(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval", "SearchApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void grvSISOApproval_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lblSuccess.Text = "";
            lblErrorMess.Text = "";
            try
            {
                grvSISOApproval.EditIndex = -1;
                BindData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.aspx.cs", "grvSISOApproval_RowCancelingEdit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void grvSISOApproval_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                lblSuccess.Text = "";
                lblErrorMess.Text = "";
                grvSISOApproval.PageIndex = e.NewPageIndex;
                objSignInSignOutModel.EmployeeID = Convert.ToInt32(User.Identity.Name);
                objSignInSignOutModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);

                if ((ddlStatus.SelectedValue.ToString() == "1") && (txtSearchToDate.Text == ""))
                {
                    objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                    objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();
                    BindData();
                }
                else
                {
                    objSignInSignOutModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    DateTime dt = DateTime.Now;

                    if (txtSearchToDate.Text != "")
                    {
                        objSignInSignOutModel.Todate = Convert.ToDateTime(txtSearchToDate.Text);
                    }
                    else
                    {
                        objSignInSignOutModel.Todate = Convert.ToDateTime(dt.Date.AddMonths(12));
                    }
                    if (txtSearchFromDate.Text != "")
                    {
                        objSignInSignOutModel.FromDate = Convert.ToDateTime(txtSearchFromDate.Text);
                    }
                    else
                    {
                        objSignInSignOutModel.FromDate = Convert.ToDateTime(dt.AddYears(-3));
                    }

                    objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                    objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();
                    dsSignInSignOutApproval = SearchApproval(objSignInSignOutModel);
                    grvSISOApproval.DataSource = dsSignInSignOutApproval;
                    grvSISOApproval.DataBind();
                    grvSISOApproval.PageIndex = e.NewPageIndex;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.aspx.cs", "grvSISOApproval_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void grvSISOApproval_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                lblSuccess.Text = "";
                lblErrorMess.Text = "";

                if ((ViewState["Order"] == null))
                {
                    ViewState["Order"] = "ASC";
                }
                else if (ViewState["Order"].ToString() == "ASC")
                {
                    ViewState["Order"] = "DESC";
                }
                else if (ViewState["Order"].ToString() == "DESC")
                {
                    ViewState["Order"] = "ASC";
                }

                ViewState["ColumnName"] = e.SortExpression;

                objSignInSignOutModel.EmployeeID = Convert.ToInt32(User.Identity.Name);
                objSignInSignOutModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);

                if ((ddlStatus.SelectedValue.ToString() == "1") && (txtSearchToDate.Text == ""))
                {
                    objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                    objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();
                    BindData();
                }
                else
                {
                    objSignInSignOutModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    DateTime dt = DateTime.Now;

                    if (txtSearchToDate.Text != "")
                    {
                        objSignInSignOutModel.Todate = Convert.ToDateTime(txtSearchToDate.Text);
                    }
                    else
                    {
                        objSignInSignOutModel.Todate = Convert.ToDateTime(dt.Date.AddMonths(12));
                    }
                    if (txtSearchFromDate.Text != "")
                    {
                        objSignInSignOutModel.FromDate = Convert.ToDateTime(txtSearchFromDate.Text);
                    }
                    else
                    {
                        objSignInSignOutModel.FromDate = Convert.ToDateTime(dt.AddYears(-3));
                    }

                    objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                    objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();
                    dsSignInSignOutApproval = SearchApproval(objSignInSignOutModel);
                    grvSISOApproval.DataSource = dsSignInSignOutApproval;
                    grvSISOApproval.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.aspx.cs", "grvSISOApproval_Sorting", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #region UpdateEmployeeLeaveAndComp

        public void UpdateEmployeeLeaveAndComp()
        {
            try
            {
                int rowsAffected = objSignInSignOutBOL.UpdateEmployeeLeaveAndComp(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.aspx.cs", "UpdateEmployeeLeaveAndComp", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion UpdateEmployeeLeaveAndComp

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
                ddlStatus.SelectedValue = "1";
                BindData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutApproval.aspx.cs", "btnReset_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}