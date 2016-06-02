using System;
using System.Data;
using System.Threading;
using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Workflow.Runtime;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;
using V2.Orbit.Workflow.OutOfOfficeWF;

namespace HRMS.Orbitweb
{
    public partial class OutOfOfficeApproval : Page
    {
        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblSuccess.Text = "";
                if (!IsPostBack)
                {
                    getStatus();
                    getOutofOfficeApproval();
                }
                txtSearchFromDate.Attributes.Add("onkeydown", "return false");
                txtSearchToDate.Attributes.Add("onkeydown", "return false");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs", "page_load",
                    ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Page Load

        #region getStatus

        public void getStatus()
        {
            try
            {
                dsGetStatus = objOutOfOfficeBOL.GetStatus();
                for (var i = 0; i < dsGetStatus.Tables[0].Rows.Count; i++)
                {
                    ddlStatus.Items.Add(new ListItem(dsGetStatus.Tables[0].Rows[i]["StatusName"].ToString(),
                        dsGetStatus.Tables[0].Rows[i]["StatusID"].ToString()));
                }
                ddlStatus.Items.Add(new ListItem("All", "0"));
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs", "getStatus",
                    ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion getStatus

        #region getOutofOfficeApproval

        public void getOutofOfficeApproval()
        {
            try
            {
                if (User.Identity.Name != null)
                {
                    objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name);
                }
                objOutOfOfficeModel.StatusId = Convert.ToInt32(ddlStatus.SelectedValue);
                dsgetOutofOfficeApproval = objOutOfOfficeBOL.GetOutOfOfficeApproval(objOutOfOfficeModel);
                if (dsgetOutofOfficeApproval.Tables[0].Rows.Count <= 0)
                {
                    grvOutOfOfficeApproval.Visible = false;
                    lblSuccess.Text = "No records Found";
                }
                else
                {
                    lblSuccess.Text = "";
                    grvOutOfOfficeApproval.Visible = true;
                    grvOutOfOfficeApproval.DataSource = dsgetOutofOfficeApproval;
                    grvOutOfOfficeApproval.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs", "getOutofOfficeApproval",
                    ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion getOutofOfficeApproval

        #region grvOutOfOfficeApproval_RowDataBound

        protected void grvOutOfOfficeApproval_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name);

                    var lblStatus = ((Label)e.Row.FindControl("lblStatus"));
                    var lblApproved = ((Label)e.Row.FindControl("lblApproved"));
                    var lnkbtnEdit = ((LinkButton)e.Row.FindControl("lnkEdit"));
                    var lblOutDate = ((Label)e.Row.FindControl("lblDate"));

                    var dsConfigItem = objOutOfOfficeBOL.GetOutOfOfficeApproval(objOutOfOfficeModel);

                    var ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                    if (ConfigdateTime.Date >= Convert.ToDateTime(lblOutDate.Text).Date)
                    {
                        //   string strStatus = lblStatus.Text;
                        if (lblStatus.Text == "Approved")
                        {
                            lblApproved.Text = "Approved";
                            lnkbtnEdit.Visible = false;
                            //lnkbtnEdit.Enabled = false;
                        }
                        if (lblStatus.Text == "Pending")
                        {
                            //lnkbtnEdit.Enabled = false;
                            lblApproved.Text = "Pending";
                            lnkbtnEdit.Visible = false;
                        }
                        if (lblStatus.Text == "Rejected")
                        {
                            //lnkbtnEdit.Enabled = false;
                            lblApproved.Text = "Rejected";
                            lnkbtnEdit.Visible = false;
                        }
                        if (lblStatus.Text == "Cancelled")
                        {
                            lblApproved.Text = "Cancelled";
                            //lnkbtnEdit.Enabled = false;
                            lnkbtnEdit.Visible = false;
                        }
                    }
                    else
                    {
                        if (lnkbtnEdit != null)
                        {
                            if (lblStatus.Text == "Rejected")
                            {
                                lblApproved.Text = "Rejected";
                                lnkbtnEdit.Visible = false;
                                //lnkbtnEdit.Enabled = false;
                            }
                            else if (lblStatus.Text == "Cancelled")
                            {
                                lblApproved.Text = "Cancelled";
                                lnkbtnEdit.Visible = false;
                                //lnkbtnEdit.Enabled = false;
                            }
                        }
                    }
                }

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    dsGetReasonName = objOutOfOfficeBOL.GetReasonName();
                    // DropDownList ddlReason = ((DropDownList)e.Row.FindControl("ddlReason"));
                    //Label lblTypeid = ((Label)e.Row.FindControl("lblTypeid"));

                    //for (int i = 0; i < dsGetReasonName.Tables[0].Rows.Count; i++)
                    //{
                    //    ddlReason.Items.Add(new ListItem(dsGetReasonName.Tables[0].Rows[i]["Reason"].ToString(), dsGetReasonName.Tables[0].Rows[i]["TypeId"].ToString()));

                    //}
                    //ddlReason.SelectedValue = lblTypeid.Text.ToString();

                    dsGetStatusName = objOutOfOfficeBOL.GetStatus();
                    var ddlApproved = ((DropDownList)e.Row.FindControl("ddlApproved"));
                    var lblStatusid = ((Label)e.Row.FindControl("lblStatusid"));
                    for (var i = 0; i < dsGetStatusName.Tables[0].Rows.Count; i++)
                    {
                        if (dsGetStatusName.Tables[0].Rows[i]["StatusName"].ToString() != "Cancelled")
                        {
                            ddlApproved.Items.Add(
                                new ListItem(dsGetStatusName.Tables[0].Rows[i]["StatusName"].ToString(),
                                    dsGetStatusName.Tables[0].Rows[i]["StatusID"].ToString()));
                        }
                    }
                    ddlApproved.SelectedValue = lblStatusid.Text;

                    var ddlOutTimeHrs = ((DropDownList)e.Row.FindControl("ddlOutTimeHrs"));
                    var ddlInTimeHrs = ((DropDownList)e.Row.FindControl("ddlInTimeHrs"));

                    for (var i = 1; i <= 23; i++)
                    {
                        ddlOutTimeHrs.Items.Add(i.ToString());
                        ddlInTimeHrs.Items.Add(i.ToString());
                    }
                    var lblOutTime = ((Label)e.Row.FindControl("lblOutTime1"));
                    // ddlOutTimeHrs.SelectedValue = lblOutTime.Text.ToString();
                    var strOutTime = lblOutTime.Text;
                    var strarOutTime = strOutTime.Split(':');
                    ddlOutTimeHrs.SelectedIndex = Convert.ToInt32(strarOutTime[0]) - 1;

                    var ddlOutTimeMins = ((DropDownList)e.Row.FindControl("ddlOutTimeMins"));
                    for (var i = 0; i < ddlOutTimeMins.Items.Count; i++)
                    {
                        if (ddlOutTimeMins.Items[i].Text.Trim() == strarOutTime[1].Trim())
                        {
                            ddlOutTimeMins.SelectedIndex = i;
                            break;
                        }
                    }
                    var lblInTime = ((Label)e.Row.FindControl("lblInTime1"));
                    var strInTime = lblInTime.Text;
                    var strarInTime = strInTime.Split(':');
                    ddlInTimeHrs.SelectedIndex = Convert.ToInt32(strarInTime[0]) - 1;
                    var ddlInTimeMins = ((DropDownList)e.Row.FindControl("ddlInTimeMins"));
                    for (var i = 0; i < ddlInTimeMins.Items.Count; i++)
                    {
                        if (ddlInTimeMins.Items[i].Text.Trim() == strarInTime[1].Trim())
                        {
                            ddlInTimeMins.SelectedIndex = i;
                            break;
                        }
                    }
                    var statusApproved = ((Label)e.Row.FindControl("lblstatusApproved"));
                    if (ddlApproved.SelectedItem.Text == "Approved")
                    {
                        ddlApproved.Visible = false;
                        statusApproved.Visible = true;
                        statusApproved.Text = "Approved";
                    }
                    if (ddlApproved.SelectedItem.Text == "Pending")
                    {
                        statusApproved.Visible = false;
                        ddlApproved.Visible = true;
                    }

                    //LinkButton lnkbtnEdit = ((LinkButton)e.Row.FindControl("lnkEdit"));

                    //if (ddlApproved.SelectedItem.Text == "Approved")
                    //{
                    //    lnkbtnEdit.Visible = true;
                    //}
                    //else if (ddlApproved.SelectedItem.Text == "Pending")
                    //{
                    //    lnkbtnEdit.Visible = true;

                    //}
                    //else
                    //{
                    //    lnkbtnEdit.Visible = false;
                    //}

                    var txtApproversComment = ((TextBox)e.Row.FindControl("txtApproversComment"));
                    ((LinkButton)(e.Row.FindControl("btnUpdate"))).Attributes.Add("onClick",
                        "return Validation(" + ddlOutTimeHrs.ClientID + "," + ddlOutTimeMins.ClientID + ", " +
                        ddlInTimeHrs.ClientID + ", " + ddlInTimeMins.ClientID + "," + txtApproversComment.ClientID +
                        " );");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs",
                    "grvOutOfOfficeApproval_RowDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion grvOutOfOfficeApproval_RowDataBound

        #region grvOutOfOfficeApproval_RowEditing

        protected void grvOutOfOfficeApproval_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                grvOutOfOfficeApproval.EditIndex = e.NewEditIndex;
                getOutofOfficeApproval();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs",
                    "grvOutOfOfficeApproval_RowEditing", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion grvOutOfOfficeApproval_RowEditing

        #region grvOutOfOfficeApproval_RowCancelingEdit

        protected void grvOutOfOfficeApproval_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                grvOutOfOfficeApproval.EditIndex = -1;
                getOutofOfficeApproval();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs",
                    "grvOutOfOfficeApproval_RowCancelingEdit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion grvOutOfOfficeApproval_RowCancelingEdit

        #region grvOutOfOfficeApproval_RowUpdating

        protected void grvOutOfOfficeApproval_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var CheckWorkFlowStatus = false;
            try
            {
                var lblOutofOfficeid = ((Label)grvOutOfOfficeApproval.Rows[e.RowIndex].FindControl("lblOutofOFficeid"));
                objOutOfOfficeModel.OutOfOfficeID = Convert.ToInt32(lblOutofOfficeid.Text);

                var lblWorkflowID = ((Label)grvOutOfOfficeApproval.Rows[e.RowIndex].FindControl("lblOutofOfficeWFID"));
                objOutOfOfficeModel.WorkflowId = new Guid(lblWorkflowID.Text);

                var lblUserId = ((Label)grvOutOfOfficeApproval.Rows[e.RowIndex].FindControl("lblUserId"));
                objOutOfOfficeModel.UserId = Convert.ToInt32(lblUserId.Text);

                var strDate = ((Label)grvOutOfOfficeApproval.Rows[e.RowIndex].FindControl("lblDate")).Text;

                var ddlOutTimeHrs =
                    ((DropDownList)grvOutOfOfficeApproval.Rows[e.RowIndex].FindControl("ddlOutTimeHrs"));
                var ddlOutTimeMins =
                    ((DropDownList)grvOutOfOfficeApproval.Rows[e.RowIndex].FindControl("ddlOutTimeMins"));

                var strOutTime = strDate + " " + ddlOutTimeHrs.SelectedValue + ":" + ddlOutTimeMins.SelectedItem.Text +
                                 ":00";
                objOutOfOfficeModel.OutTime = Convert.ToDateTime(strOutTime.Trim());

                var ddlInTimeHrs = ((DropDownList)grvOutOfOfficeApproval.Rows[e.RowIndex].FindControl("ddlInTimeHrs"));
                var ddlInTimeMins =
                    ((DropDownList)grvOutOfOfficeApproval.Rows[e.RowIndex].FindControl("ddlInTimeMins"));

                var strInTime = strDate + " " + ddlInTimeHrs.SelectedValue + ":" + ddlInTimeMins.SelectedItem.Text +
                                ":00";
                objOutOfOfficeModel.InTime = Convert.ToDateTime(strInTime.Trim());

                //TextBox txtComment = ((TextBox)grvOutOfOfficeApproval.Rows[e.RowIndex].FindControl("txtComment"));
                //objOutOfOfficeModel.Comments = txtComment.Text;

                var lblApproverID = ((Label)grvOutOfOfficeApproval.Rows[e.RowIndex].FindControl("lblApproverID"));
                objOutOfOfficeModel.ApproverId = Convert.ToInt32(lblApproverID.Text.Trim());

                var txtApproversComment =
                    ((TextBox)grvOutOfOfficeApproval.Rows[e.RowIndex].FindControl("txtApproversComment"));
                objOutOfOfficeModel.ApproverComments = txtApproversComment.Text.Trim();

                //DropDownList ddlReason = ((DropDownList)grvOutOfOfficeApproval.Rows[e.RowIndex].FindControl("ddlReason"));
                //objOutOfOfficeModel.Type = Convert.ToInt32(ddlReason.SelectedValue);

                var ddlApproved = ((DropDownList)grvOutOfOfficeApproval.Rows[e.RowIndex].FindControl("ddlApproved"));
                objOutOfOfficeModel.StatusId = Convert.ToInt32(ddlApproved.SelectedValue);

                if (ddlApproved.Visible == false)
                {
                    CheckWorkFlowStatus = true;
                }

                objOutOfOfficeBOL.UpdateOutOfficeApproval(objOutOfOfficeModel);
                lblSuccess.Text = "Record Updated Successfully";

                // StartWorkflow
                if (!CheckWorkFlowStatus)
                {
                    if (objOutOfOfficeModel.StatusId == 2)
                    {
                        try
                        {
                            var wr = (WorkflowRuntime)Application["WokflowRuntime"];
                            if (objOutOfOfficeModel.WorkflowId != null ||
                                objOutOfOfficeModel.WorkflowId.ToString() != "")
                            {
                                var wi = wr.GetWorkflow(objOutOfOfficeModel.WorkflowId);
                                wi.Resume();
                                var objOutOfOfficeService =
                                    (OutOfOfficeService)wr.GetService(typeof(OutOfOfficeService));
                                objOutOfOfficeService.RaiseApproveEvent(wi.InstanceId);
                            }
                        }
                        catch (V2Exceptions)
                        {
                            try
                            {
                                Response.Redirect("OutOfOfficeApproval.aspx");
                            }
                            catch (ThreadAbortException ex)
                            {
                                throw;
                            }
                        }
                        catch (Exception)
                        {
                            try
                            {
                                Response.Redirect("OutOfOfficeApproval.aspx");
                            }
                            catch (ThreadAbortException ex)
                            {
                                throw;
                            }
                        }
                    }
                    if (objOutOfOfficeModel.StatusId == 3)
                    {
                        try
                        {
                            var wr = (WorkflowRuntime)Application["WokflowRuntime"];
                            if (objOutOfOfficeModel.WorkflowId != null ||
                                objOutOfOfficeModel.WorkflowId.ToString() != "")
                            {
                                var wi = wr.GetWorkflow(objOutOfOfficeModel.WorkflowId);
                                wi.Resume();
                                var objOutOfOfficeService =
                                    (OutOfOfficeService)wr.GetService(typeof(OutOfOfficeService));
                                objOutOfOfficeService.RaiseRejectEvent(wi.InstanceId);
                            }
                        }
                        catch (V2Exceptions)
                        {
                            try
                            {
                                Response.Redirect("OutOfOfficeApproval.aspx");
                            }
                            catch (ThreadAbortException ex)
                            {
                                throw;
                            }
                        }
                        catch (Exception)
                        {
                            try
                            {
                                Response.Redirect("OutOfOfficeApproval.aspx");
                            }
                            catch (ThreadAbortException ex)
                            {
                                throw;
                            }
                        }
                    }
                }
                // End WorkFlow

                if (objOutOfOfficeModel.StatusId == 2)
                {
                    if (CheckWorkFlowStatus)
                    {
                        createMailMessage();
                    }
                }

                grvOutOfOfficeApproval.EditIndex = -1;
                //getOutofOfficeApproval();
                Response.Redirect("OutOfOfficeApproval.aspx");
            }
            catch (ThreadAbortException ex)
            {
                throw;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (ex.Message.CompareTo("An entry for the selected Date and Time already exists") != 0)
                {
                    var objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs",
                        "grvOutOfOfficeApproval_RowUpdating", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                lblError.Text = ex.Message;
                lblSuccess.Text = "";
            }
        }

        #endregion grvOutOfOfficeApproval_RowUpdating

        #region searchOutOfOfficeApproval

        public void searchOutOfOfficeApproval()
        {
            try
            {
                dsgetOutofOfficeApproval = objOutOfOfficeBOL.SearchOutOfOfficeApproval(objOutOfOfficeModel);
                if (dsgetOutofOfficeApproval.Tables[0].Rows.Count <= 0)
                {
                    lblError.Text = "Records Not Found";
                    lblSuccess.Text = "";
                    grvOutOfOfficeApproval.Visible = false;
                }
                else
                {
                    lblError.Text = "";
                    grvOutOfOfficeApproval.Visible = true;
                    grvOutOfOfficeApproval.DataSource = dsgetOutofOfficeApproval;
                    grvOutOfOfficeApproval.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs",
                    "searchOutOfOfficeApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion searchOutOfOfficeApproval

        #region searchOutOfOfficeDatewise

        public void searchOutOfOfficeDatewise()
        {
            try
            {
                dssearchOutOfOfficeDatewise = objOutOfOfficeBOL.SearchOutOfOfficeApprovalDateWise(objOutOfOfficeModel);
                if (dssearchOutOfOfficeDatewise.Tables[0].Rows.Count <= 0)
                {
                    lblError.Text = "Records Not Found";
                    lblSuccess.Text = "";
                    grvOutOfOfficeApproval.Visible = false;
                }
                else
                {
                    lblError.Text = "";
                    grvOutOfOfficeApproval.Visible = true;
                    grvOutOfOfficeApproval.DataSource = dssearchOutOfOfficeDatewise.Tables[0];
                    grvOutOfOfficeApproval.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs",
                    "searchOutOfOfficeApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion searchOutOfOfficeDatewise

        #region btnSearch_Click

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name);
                objOutOfOfficeModel.StatusId = Convert.ToInt32(ddlStatus.SelectedValue);
                objOutOfOfficeModel.FromDate = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                objOutOfOfficeModel.ToDate = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                searchOutOfOfficeDatewise();
                //txtSearchFromDate.Text = "";
                //txtSearchToDate.Text = "";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs", "btnSearch_Click",
                    ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion btnSearch_Click

        #region grvOutOfOfficeApproval_PageIndexChanging

        protected void grvOutOfOfficeApproval_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grvOutOfOfficeApproval.PageIndex = e.NewPageIndex;
                grvOutOfOfficeApproval.EditIndex = -1;
                getOutofOfficeApproval();

                objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name);
                objOutOfOfficeModel.StatusId = Convert.ToInt32(ddlStatus.SelectedValue);

                if (Convert.ToInt32(ddlStatus.SelectedValue) != 0)
                {
                    searchOutOfOfficeApproval();
                }
                if (txtSearchFromDate.Text != "" && txtSearchToDate.Text != "")
                {
                    objOutOfOfficeModel.FromDate = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objOutOfOfficeModel.ToDate = Convert.ToDateTime(txtSearchToDate.Text.Trim());

                    searchOutOfOfficeDatewise();
                }
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs",
                    "grvOutOfOfficeApproval_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion grvOutOfOfficeApproval_PageIndexChanging

        #region ddlStatus_SelectedIndexChanged

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name);
                if (ddlStatus.SelectedItem.Text == "All")
                {
                    objOutOfOfficeModel.StatusId = 0;
                    objOutOfOfficeModel.FromDate = Convert.ToDateTime(null);
                    objOutOfOfficeModel.ToDate = Convert.ToDateTime(null);
                }
                else if (Convert.ToInt32(ddlStatus.SelectedValue) >= 1)
                {
                    objOutOfOfficeModel.StatusId = Convert.ToInt32(ddlStatus.SelectedValue);
                    objOutOfOfficeModel.FromDate = Convert.ToDateTime(null);
                    objOutOfOfficeModel.ToDate = Convert.ToDateTime(null);
                }
                grvOutOfOfficeApproval.EditIndex = -1;
                searchOutOfOfficeApproval();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs",
                    "ddlStatus_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion ddlStatus_SelectedIndexChanged

        #region grvOutOfOfficeApproval_Sorting

        protected void grvOutOfOfficeApproval_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name);
                dsgetOutofOfficeApproval = objOutOfOfficeBOL.GetOutOfOfficeApproval(objOutOfOfficeModel);
                grvOutOfOfficeApproval.DataSource = dsgetOutofOfficeApproval.Tables[0];
                grvOutOfOfficeApproval.DataBind();
                var dt = dsgetOutofOfficeApproval.Tables[0];
                var dv = new DataView(dt);

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
                var strOrder = ViewState["Order"].ToString();
                dv.Sort = e.SortExpression + " " + strOrder;
                grvOutOfOfficeApproval.DataSource = dv;
                grvOutOfOfficeApproval.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs",
                    "grvOutOfOfficeApproval_Sorting", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion grvOutOfOfficeApproval_Sorting

        #region createMailMessage

        public void createMailMessage()
        {
            try
            {
                dsReportingTo = objOutOfOfficeBOL.GetReportingTo(objOutOfOfficeModel);

                if (dsReportingTo.Tables[0].Rows.Count > 0)
                {
                    //Send mail to Approved Out of office cancelled report.
                    var objMailMessage = new MailMessage();
                    string Reason, Date, OutTime, InTime;

                    //EmployeeName = dsReportingTo.Tables[0].Rows[0]["EmployeeName"].ToString();
                    //ApproverName = dsReportingTo.Tables[0].Rows[0]["ReporterName"].ToString();
                    objMailMessage.To = dsReportingTo.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
                    dsCancelOutOfOffice = objOutOfOfficeBOL.CancelOutOfOffice(objOutOfOfficeModel);

                    Date = Convert.ToDateTime(dsCancelOutOfOffice.Tables[0].Rows[0]["Date"]).ToShortDateString();
                    OutTime = dsCancelOutOfOffice.Tables[0].Rows[0]["OutTime"].ToString();
                    InTime = dsCancelOutOfOffice.Tables[0].Rows[0]["InTime"].ToString();
                    Reason = dsCancelOutOfOffice.Tables[0].Rows[0]["Comment"].ToString();
                    for (var k = 0; k < dsReportingTo.Tables[1].Rows.Count; k++)
                    {
                        if (dsReportingTo.Tables[1].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                        {
                            objMailMessage.From = dsReportingTo.Tables[1].Rows[k]["ConfigItemValue"].ToString();
                        }
                        if (dsReportingTo.Tables[1].Rows[k]["ConfigItemName"].ToString() == "Mail Server Name")
                        {
                            break;
                        }
                    }

                    objMailMessage.Subject = "Approved Out Of Office entry was updated.";
                    objMailMessage.BodyFormat = MailFormat.Html;
                    objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi" + " " +
                                          dsReportingTo.Tables[0].Rows[0]["EmployeeName"] + " , " + "<br><br>" +
                                          "Out Of Office Date :" + Date + "<br>" + "OutTime : " + OutTime + "<br>" +
                                          "InTime :" + InTime + "<Br>" + "Reason :" + Reason + "<br><br>" +
                                          " Your Out of office entry was updated by " + " " +
                                          dsReportingTo.Tables[0].Rows[0]["ReporterName"] +
                                          ", the required updates are made in the system.";
                    SmtpMail.Send(objMailMessage);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs",
                    "grvOutOfOfficeApproval_Sorting", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion createMailMessage

        #region btnReset_Click

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeApproval.aspx.cs",
                    "grvOutOfOfficeApproval_Sorting", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion btnReset_Click

        #region Variable Declaration

        private readonly OutOfOfficeBOL objOutOfOfficeBOL = new OutOfOfficeBOL();
        private readonly OutOfOfficeModel objOutOfOfficeModel = new OutOfOfficeModel();
        private DataSet dsGetStatus = new DataSet();
        private DataSet dsgetOutofOfficeApproval = new DataSet();
        private DataSet dsGetReasonName = new DataSet();
        private DataSet dsGetStatusName = new DataSet();
        private DataSet dsSearchOutOfOfficApproval = new DataSet();
        private DataSet dssearchOutOfOfficeDatewise = new DataSet();
        private DataSet dsReportingTo = new DataSet();
        private DataSet dsCancelOutOfOffice = new DataSet();

        #endregion Variable Declaration
    }
}