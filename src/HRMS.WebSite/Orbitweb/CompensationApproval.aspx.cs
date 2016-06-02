using HRMS.Notification;
using System;
using System.Data;

//using System.Web.Mail;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Workflow.Runtime;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;
using V2.Orbit.Workflow.CompensationWF;

namespace HRMS.Orbitweb
{
    public partial class CompensationApproval : System.Web.UI.Page
    {
        #region Variable Declaration

        private CompensationDetailsModel objCompensationModel = new CompensationDetailsModel();
        private CompensationDetailsBOL objCompensationBOL = new CompensationDetailsBOL();
        private LeaveDetailsBOL objLeaveDeatilsBOL = new LeaveDetailsBOL();
        private LeaveDetailsModel objLeaveDeatilsModel = new LeaveDetailsModel();
        private LeaveDetailsModel objLeaveDetailsModel = new LeaveDetailsModel();

        private HolidayMasterBOL objHolidayBOL = new HolidayMasterBOL();
        private HolidayMasterModel objHolidayModel = new HolidayMasterModel();
        private LeaveTransactionBOL objLeaveTransDetailsBOL = new LeaveTransactionBOL();
        private LeaveTransactionModel objLeaveTransDetailsModel = new LeaveTransactionModel();
        private DataSet dsGetStatus, dsCompensation, dsHolidaysList, dsSearchDetails, dsGetLeaveStatus, dsSorting, dsConfigItem, dsReportingTo, dsCancelDetails;
        private DataTable dtGetLeaveStatus;
        private string UserID = string.Empty;
        private DateTime FromDate, ToDate, ConfigDate;
        private double TotalLeaves = 0, count = 0;
        private int j = 0, TotalLeavesApplyedFor = 0;
        private double CorrectionLeaves, BalanceLeaves;
        private String[] strLeaves;
        private String[] strHoliDays;
        private int rowsAffected;
        private bool WfApprovedLocked = false;
        private RosterPlanningBOL ObjRosterPlanningBOL = new RosterPlanningBOL();
        private RosterPlanningModel objRosterPlanningModel = new RosterPlanningModel();
        private DataSet dsEmployeeRole = null;
        private DataSet dsEmployeeShiftDetail = null;
        private Boolean isShiftEmployee = false;
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        #endregion Variable Declaration

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string PageName = "Compensatory Leave";
                objpagelevel.PageLevelAccess(PageName);

                UserID = User.Identity.Name.ToString();
                lblError.Text = "";
                lblSuccess.Text = "";
                if (!IsPostBack)
                {
                    FillddlStatus();
                    BindData();
                }

                txtSearchFromDate.Attributes.Add("onkeydown", "return false");
                txtSearchToDate.Attributes.Add("onkeydown", "return false");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Page Load

        #region BindData

        public void BindData()
        {
            try
            {
                dsCompensation = new DataSet();
                objCompensationModel.UserID = Convert.ToInt32(UserID);
                objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                dsConfigItem = objCompensationBOL.GetCompensationDetails(objCompensationModel);

                dsCompensation = objCompensationBOL.GetTMCompensationDetails(objCompensationModel);
                if (dsCompensation.Tables[0].Rows.Count != 0)
                {
                    gvCompensationApprovals.DataSource = dsCompensation.Tables[0];
                    gvCompensationApprovals.DataBind();
                }
                else if (dsCompensation.Tables[0].Rows.Count == 0)
                {
                    gvCompensationApprovals.DataSource = dsCompensation;
                    gvCompensationApprovals.DataBind();

                    lblError.Visible = true;
                    lblError.Text = "No records found";
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No records found";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "BindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion BindData

        #region FillddlStatus

        public void FillddlStatus()
        {
            try
            {
                dsGetStatus = objLeaveDeatilsBOL.GetStatusDetails();

                for (int i = 0; i < dsGetStatus.Tables[0].Rows.Count; i++)
                {
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "FillddlStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion FillddlStatus

        #region Search

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                objCompensationModel.UserID = Convert.ToInt32(UserID);
                objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                objCompensationModel.CompensationFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                objCompensationModel.CompensationTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                SearchCompensationDetails();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "btnSearch_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Search

        #region SearchCompensationDetails

        public void SearchCompensationDetails()
        {
            try
            {
                dsSearchDetails = objCompensationBOL.SearchTMCompensationDetails(objCompensationModel);
                dsConfigItem = objCompensationBOL.SearchTMCompensationDetails(objCompensationModel);
                if (dsSearchDetails.Tables[0].Rows.Count <= 0)
                {
                    lblError.Text = "No records found";
                    lblSuccess.Text = "";
                    gvCompensationApprovals.Visible = false;
                }
                else
                {
                    lblError.Text = "";
                    gvCompensationApprovals.Visible = true;
                    gvCompensationApprovals.DataSource = dsSearchDetails.Tables[0];
                    gvCompensationApprovals.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "SearchCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion SearchCompensationDetails

        #region UpdateCompenstionDetails

        public void UpdateCompenstionDetails()
        {
            try
            {
                rowsAffected = objCompensationBOL.UpdateApprovalCompenstionDetails(objCompensationModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("Already Compensation applied for this date.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "UpdateCompenstionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion UpdateCompenstionDetails

        #region AddCompensationTransactionDetails

        public void AddCompensationTransactionDetails()
        {
            try
            {
                rowsAffected = objLeaveTransDetailsBOL.AddCompensationTransactionDetails(objLeaveTransDetailsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("Already leave applied for this dates.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "AddCompensationTransactionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion AddCompensationTransactionDetails

        #region ddlStatus_SelectedIndexChanged

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                objCompensationModel.UserID = Convert.ToInt32(UserID);

                dsSearchDetails = objCompensationBOL.SearchAllTMCompensationDetails(objCompensationModel);
                dsConfigItem = objCompensationBOL.SearchAllTMCompensationDetails(objCompensationModel);
                if (dsSearchDetails.Tables[0].Rows.Count <= 0)
                {
                    lblError.Text = "No records found";
                    lblSuccess.Text = "";
                    gvCompensationApprovals.Visible = false;
                }
                else
                {
                    lblError.Text = "";
                    gvCompensationApprovals.Visible = true;
                    gvCompensationApprovals.DataSource = dsSearchDetails.Tables[0];
                    gvCompensationApprovals.DataBind();
                }
                gvCompensationApprovals.EditIndex = -1;
                BindData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "ddlStatus_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion ddlStatus_SelectedIndexChanged

        #region DeleteCompensationTransactionDetails

        public void DeleteCompensationTransactionDetails()
        {
            try
            {
                rowsAffected = objLeaveTransDetailsBOL.DeleteCompensationTransactionDetails(objLeaveTransDetailsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "DeleteCompensationTransactionDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion DeleteCompensationTransactionDetails

        #region RowUpdating

        protected void gvCompensationApprovals_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvCompensationApprovals.Rows[e.RowIndex];

                Label lblTMmember = row.FindControl("lblEditUserID") as Label;

                objCompensationModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());

                Label lblEditCompensationID = row.FindControl("lblEditCompensationID") as Label;
                objCompensationModel.CompensationID = Convert.ToInt32(lblEditCompensationID.Text);

                Label lblCompensationWFID = row.FindControl("lblCompensationWFID") as Label;
                if (lblCompensationWFID.Text != "")
                    objCompensationModel.WorkFlowID = new Guid(lblCompensationWFID.Text);

                Label lblgrvEditUserName = row.FindControl("lblgrvEditUserName") as Label;

                TextBox txtEditAppliedFor = row.FindControl("txtgrvAppliedFor") as TextBox;

                DropDownList ddlStatusID = row.FindControl("ddlgrvStatusName") as DropDownList;
                objCompensationModel.StatusID = Convert.ToInt32(ddlStatusID.SelectedValue);
                if (ddlStatusID.Visible == false)
                {
                    WfApprovedLocked = true;
                }

                Label lblEditgrvReason = row.FindControl("lblEditgrvReason") as Label;

                objCompensationModel.Resason = lblEditgrvReason.Text.Trim();

                TextBox ApproverComments = row.FindControl("txtgrvApproverComments") as TextBox;
                objCompensationModel.ApproverComments = Convert.ToString(ApproverComments.Text.ToString());

                objCompensationModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                objCompensationModel.CompensationTo = Convert.ToDateTime(txtEditAppliedFor.Text.Trim());

                objLeaveDeatilsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                //Getting All the Holidays Details
                //dsHolidaysList = objHolidayBOL.bindData();
                objHolidayModel.UserID = objCompensationModel.UserID;
                objHolidayModel.StartDate = Convert.ToDateTime(txtEditAppliedFor.Text.Trim());
                objHolidayModel.EndDate = Convert.ToDateTime(txtEditAppliedFor.Text.Trim());
                dsHolidaysList = objHolidayBOL.bindHolidaysForLeaveApprovals(objHolidayModel);

                //For Getting ConfigDate from Database
                dsConfigItem = objCompensationBOL.GetCompensationDetails(objCompensationModel);
                ConfigDate = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                FromDate = Convert.ToDateTime(txtEditAppliedFor.Text.Trim());
                Boolean isCompLeaveApproved = false;
                rowsAffected = objCompensationBOL.CheckSignInForCompensation(objCompensationModel);

                objRosterPlanningModel.UserId = Convert.ToInt32(lblTMmember.Text.ToString());
                dsEmployeeRole = ObjRosterPlanningBOL.GetEmployeeRole(objRosterPlanningModel);
                foreach (DataRow rowRole in dsEmployeeRole.Tables[0].Rows)
                {
                    if (rowRole["RoleName"].ToString().Equals("Shift"))
                    {
                        isShiftEmployee = true;
                        break;
                    }
                }
                if (rowsAffected > 0)
                {
                    if (!isShiftEmployee)
                    {
                        // This is checking the total list of holiday in current year provided by V2.
                        for (int k = 0; k < dsHolidaysList.Tables[0].Rows.Count; k++)
                        {
                            if (FromDate.ToString() == dsHolidaysList.Tables[0].Rows[k]["HolidayDate"].ToString())
                            {
                                isCompLeaveApproved = true;
                                break;
                            }
                        }

                        // This is to check whether the working day is a WeekDay.
                        if (FromDate.DayOfWeek.ToString() == "Saturday" || FromDate.DayOfWeek.ToString() == "Sunday")
                        {
                            isCompLeaveApproved = true;
                        }
                    }
                    else
                    {
                        objRosterPlanningModel.FromDate = Convert.ToDateTime(FromDate);
                        objRosterPlanningModel.ToDate = Convert.ToDateTime(FromDate);

                        dsEmployeeShiftDetail = ObjRosterPlanningBOL.GetEmployeeShiftDetail(objRosterPlanningModel);
                        for (int k = 0; k < dsEmployeeShiftDetail.Tables[1].Rows.Count; k++)
                        {
                            if (FromDate.ToString() == dsEmployeeShiftDetail.Tables[1].Rows[k]["HolidayDate"].ToString())
                            {
                                isCompLeaveApproved = true;
                                break;
                            }
                        }
                        for (int i = 0; i < dsEmployeeShiftDetail.Tables[0].Rows.Count; i++)
                        {
                            if (Convert.ToDateTime(FromDate.ToString()) == Convert.ToDateTime(dsEmployeeShiftDetail.Tables[0].Rows[i]["WeekOff1"].ToString()) || (Convert.ToDateTime(FromDate.ToString()) == Convert.ToDateTime(dsEmployeeShiftDetail.Tables[0].Rows[i]["WeekOff2"].ToString())))
                            {
                                isCompLeaveApproved = true;
                                break;
                            }
                        }
                    }
                    if (isCompLeaveApproved)
                    {
                        // Compensatory Leave Approved Successfully.
                        objCompensationModel.AppliedFor = Convert.ToDateTime(FromDate.ToString());
                        UpdateCompenstionDetails();
                        gvCompensationApprovals.EditIndex = -1;
                        lblError.Visible = true;
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "Compensatory Leave details are updated successfully";
                        lblError.Text = "";

                        if (ddlStatusID.SelectedValue == Convert.ToString("2"))
                        {
                            //Here delete the all records of related CompensationID
                            objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                            objLeaveTransDetailsModel.CompensationID = Convert.ToInt32(lblEditCompensationID.Text);

                            //Here Adding Leave records to LeaveTransaction table
                            objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                            objLeaveTransDetailsModel.CompensationID = Convert.ToInt32(lblEditCompensationID.Text);

                            objLeaveTransDetailsModel.TransactionDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                            objLeaveTransDetailsModel.Description = "Compensatory Leave:" + lblEditgrvReason.Text.ToString();
                            objLeaveTransDetailsModel.Quantity = Convert.ToDecimal(1);
                            objLeaveTransDetailsModel.LeaveType = Convert.ToBoolean(0);

                            AddCompensationTransactionDetails();
                            UpdateEmployeeLeaveAndComp();
                        }
                        if (ddlStatusID.SelectedValue == Convert.ToString("3"))
                        {
                            //Here delete the all records of related CompensationID
                            objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                            objLeaveTransDetailsModel.CompensationID = Convert.ToInt32(lblEditCompensationID.Text);
                            DeleteCompensationTransactionDetails();
                            UpdateEmployeeLeaveAndComp();

                            //Here Adding Leave records to Compensation table
                            objCompensationModel.AppliedFor = Convert.ToDateTime(txtEditAppliedFor.Text.Trim());
                            objCompensationModel.StatusID = Convert.ToInt32(3);
                            UpdateCompenstionDetails();
                            lblSuccess.Visible = true;
                            lblSuccess.Text = "Compensatory Leave Details are Rejected";
                        }
                    }
                }
                else
                {
                    // Compensatory Leave Approved Unsuccessfully.
                    lblError.Visible = true;
                    lblError.Text = "The entry for the day you are approving Compensatory Leave for is in Pending Status.Kindly approve that";
                    lblSuccess.Text = "";
                    return;
                }

                if (WfApprovedLocked == false)
                {
                    if (objCompensationModel.StatusID == 2)
                    {
                        try
                        {
                            WorkflowRuntime wr = (WorkflowRuntime)Application["WokflowRuntime"];
                            if (objCompensationModel.WorkFlowID != null || objCompensationModel.WorkFlowID.ToString() != "")
                            {
                                WorkflowInstance wi = wr.GetWorkflow(objCompensationModel.WorkFlowID);
                                wi.Resume();
                                CompensationService objCompensationService = (CompensationService)wr.GetService(typeof(CompensationService));
                                objCompensationService.RaiseApproveEvent(wi.InstanceId);
                            }
                        }
                        catch (V2Exceptions)
                        {
                            try
                            {
                                Response.Redirect("CompensationApproval.aspx");
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
                                Response.Redirect("CompensationApproval.aspx");
                            }
                            catch (System.Threading.ThreadAbortException ex)
                            {
                                throw;
                            }
                        }
                    }
                    if (objCompensationModel.StatusID == 3)
                    {
                        try
                        {
                            WorkflowRuntime wr = (WorkflowRuntime)Application["WokflowRuntime"];
                            if (objCompensationModel.WorkFlowID != null || objCompensationModel.WorkFlowID.ToString() != "")
                            {
                                WorkflowInstance wi = wr.GetWorkflow(objCompensationModel.WorkFlowID);
                                wi.Resume();
                                CompensationService objCompensationService = (CompensationService)wr.GetService(typeof(CompensationService));
                                objCompensationService.RaiseRejectEvent(wi.InstanceId);
                            }
                        }
                        catch (V2Exceptions)
                        {
                            try
                            {
                                Response.Redirect("CompensationApproval.aspx");
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
                                Response.Redirect("CompensationApproval.aspx");
                            }
                            catch (System.Threading.ThreadAbortException ex)
                            {
                                throw;
                            }
                        }
                    }
                    if (objCompensationModel.StatusID == 4)
                    {
                        try
                        {
                            WorkflowRuntime wr = (WorkflowRuntime)Application["WokflowRuntime"];
                            if (objCompensationModel.WorkFlowID != null || objCompensationModel.WorkFlowID.ToString() != "")
                            {
                                WorkflowInstance wi = wr.GetWorkflow(objCompensationModel.WorkFlowID);
                                wi.Resume();
                                CompensationService objCompensationService = (CompensationService)wr.GetService(typeof(CompensationService));
                                objCompensationService.RaiseCancelEvent(wi.InstanceId);
                            }
                        }
                        catch (V2Exceptions)
                        {
                            try
                            {
                                Response.Redirect("CompensationApproval.aspx");
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
                                Response.Redirect("CompensationApproval.aspx");
                            }
                            catch (System.Threading.ThreadAbortException ex)
                            {
                                throw;
                            }
                        }
                    }
                }
                else
                {
                    if (objCompensationModel.StatusID == 2)
                    {
                        //Sending a Mail to Employee after Updating compensatory Leaves
                        if (WfApprovedLocked == true)
                        {
                            objLeaveDeatilsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                            SendingMailToReportingPerson();
                        }
                    }
                }

                Response.Redirect("CompensationApproval.aspx");
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
                if (ex.Message.CompareTo("Already Compensation applied for this date.") != 0)
                {
                    lblError.Visible = false;
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "btnSubmit", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    lblError.Visible = true;

                    lblError.Text = "User has already applied for compensatory leave for this date";
                    lblSuccess.Text = "";
                }
            }
        }

        #endregion RowUpdating

        #region RowEditing

        protected void gvCompensationApprovals_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvCompensationApprovals.EditIndex = e.NewEditIndex;

                BindData();
                if (txtSearchFromDate.Text != "" && txtSearchToDate.Text != "")
                {
                    objCompensationModel.UserID = Convert.ToInt32(UserID);
                    objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    objCompensationModel.CompensationFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objCompensationModel.CompensationTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    SearchCompensationDetails();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "gvCompensationApprovals_RowEditing", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion RowEditing

        #region RowDataBound

        protected void gvCompensationApprovals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblstatus = ((Label)(e.Row.FindControl("lblgrvStatusName")));
                    Label lblgrvAppliedFor = ((Label)(e.Row.FindControl("lblgrvAppliedFor")));
                    Label lblApproved = ((Label)(e.Row.FindControl("lblApprove")));
                    LinkButton lnkEdit = ((LinkButton)(e.Row.FindControl("lnkbutEdit")));
                    objCompensationModel.UserID = Convert.ToInt32(UserID);
                    objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    dsConfigItem = objCompensationBOL.GetCompensationDetails(objCompensationModel);

                    DateTime dtCheckDate = DateTime.Now.AddDays(-60);
                    if (dtCheckDate.Date >= Convert.ToDateTime(lblgrvAppliedFor.Text).Date)
                    {
                        if (lblstatus.Text == "Approved")
                        {
                            lblApproved.Visible = true;
                            lblApproved.Text = "CompOff Aprroved";
                            lnkEdit.Visible = false;
                        }
                        else if (lblstatus.Text == "Cancelled")
                        {
                            lblApproved.Visible = true;
                            lblApproved.Text = "CompOff Cancelled";
                            lnkEdit.Visible = false;
                        }
                        else if (lblstatus.Text == "Rejected")
                        {
                            lblApproved.Visible = true;
                            lblApproved.Text = "CompOff Rejected";
                            lnkEdit.Visible = false;
                        }
                        else if (lblstatus.Text == "Pending")
                        {
                            lblApproved.Visible = true;
                            lblApproved.Text = "CompOff Pending";
                            lnkEdit.Visible = false;
                        }
                    }
                    else
                    {
                        if (lnkEdit != null)
                        {
                            if (lblstatus.Text == "Cancelled")
                            {
                                lblApproved.Visible = true;
                                lblApproved.Text = "CompOff Cancelled";
                                lnkEdit.Visible = false;
                            }
                            else if (lblstatus.Text == "Rejected")
                            {
                                lblApproved.Visible = true;
                                lblApproved.Text = "CompOff Rejected";
                                lnkEdit.Visible = false;
                            }
                        }
                    }
                }

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    TextBox txtApproversComment = ((TextBox)e.Row.FindControl("txtgrvApproverComments"));
                    LinkButton lnkUpdate = ((LinkButton)e.Row.FindControl("lbnUpdate"));
                    lnkUpdate.Attributes.Add("onClick", "return Validation(" + txtApproversComment.ClientID + ");");

                    DropDownList ddlgrvStatusName = (DropDownList)e.Row.FindControl("ddlgrvStatusName");
                    Label lblgrvStatusID = (Label)e.Row.FindControl("lblgrvStatusID");

                    dsGetLeaveStatus = objLeaveDeatilsBOL.GetStatusDetails();
                    dtGetLeaveStatus = dsGetLeaveStatus.Tables[0];
                    if (dtGetLeaveStatus.Rows.Count > 0)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            ddlgrvStatusName.Items.Add(new ListItem(dtGetLeaveStatus.Rows[i]["StatusName"].ToString(), dtGetLeaveStatus.Rows[i]["StatusID"].ToString()));
                        }

                        ddlgrvStatusName.SelectedValue = lblgrvStatusID.Text.ToString();
                    }

                    TextBox txtgrvAppliedFor = (TextBox)e.Row.FindControl("txtgrvAppliedFor");

                    txtgrvAppliedFor.Attributes.Add("onkeydown", "return false");

                    if (ddlgrvStatusName.SelectedValue == Convert.ToString("2"))
                    {
                        Label lblgrvStatus = (Label)(e.Row.FindControl("lblStatus"));
                        lblgrvStatus.Visible = true;
                        ddlgrvStatusName.Visible = false;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "gvCompensationApprovals_RowDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion RowDataBound

        #region PageIndexChanging

        protected void gvCompensationApprovals_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCompensationApprovals.PageIndex = e.NewPageIndex;
                gvCompensationApprovals.EditIndex = -1;
                BindData();
                if (txtSearchFromDate.Text != "" && txtSearchToDate.Text != "")
                {
                    objCompensationModel.UserID = Convert.ToInt32(UserID);
                    objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    objCompensationModel.CompensationFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objCompensationModel.CompensationTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    SearchCompensationDetails();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "gvCompensationApprovals_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion PageIndexChanging

        #region RowCommand

        protected void gvCompensationApprovals_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "lbnCancel")
                {
                    gvCompensationApprovals.EditIndex = -1;
                    BindData();
                    if (txtSearchFromDate.Text != "" && txtSearchToDate.Text != "")
                    {
                        objCompensationModel.UserID = Convert.ToInt32(UserID);
                        objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                        objCompensationModel.CompensationFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                        objCompensationModel.CompensationTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                        SearchCompensationDetails();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "gvCompensationApprovals_RowCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion RowCommand

        #region RowCancelingEdit

        protected void gvCompensationApprovals_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvCompensationApprovals.EditIndex = -1;
                BindData();
                if (txtSearchFromDate.Text != "" && txtSearchToDate.Text != "")
                {
                    objCompensationModel.UserID = Convert.ToInt32(UserID);
                    objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    objCompensationModel.CompensationFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objCompensationModel.CompensationTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    SearchCompensationDetails();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "gvCompensationApprovals_RowCancelingEdit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion RowCancelingEdit

        #region Cancel

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ddlStatus.SelectedValue = Convert.ToString("1");
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
                lblSuccess.Text = "";
                lblError.Text = "";

                if (txtSearchFromDate.Text != "" && txtSearchToDate.Text != "")
                {
                    objCompensationModel.UserID = Convert.ToInt32(UserID);
                    objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    objCompensationModel.CompensationFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objCompensationModel.CompensationTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    SearchCompensationDetails();
                }
                else
                {
                    objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    objCompensationModel.UserID = Convert.ToInt32(UserID);
                    dsSearchDetails = objCompensationBOL.SearchAllTMCompensationDetails(objCompensationModel);
                    dsConfigItem = objCompensationBOL.SearchAllTMCompensationDetails(objCompensationModel);
                    if (dsSearchDetails.Tables[0].Rows.Count <= 0)
                    {
                        lblError.Text = "No records found";
                        lblSuccess.Text = "";
                        gvCompensationApprovals.Visible = false;
                    }
                    else
                    {
                        lblError.Text = "";
                        gvCompensationApprovals.Visible = true;
                        gvCompensationApprovals.DataSource = dsSearchDetails.Tables[0];
                        gvCompensationApprovals.DataBind();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Cancel

        #region Sorting

        protected void gvCompensationApprovals_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                objCompensationModel.UserID = Convert.ToInt32(UserID);
                objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);

                if (txtSearchFromDate.Text != "" && txtSearchToDate.Text != "")
                {
                    objCompensationModel.CompensationFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objCompensationModel.CompensationTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    dsSorting = objCompensationBOL.SearchTMCompensationDetails(objCompensationModel);
                }
                else
                {
                    dsSorting = objCompensationBOL.GetTMCompensationDetails(objCompensationModel);
                    if (dsSorting.Tables[0].Rows.Count <= 0)
                    {
                        lblError.Text = "No records found";
                        lblSuccess.Text = "";
                        gvCompensationApprovals.Visible = false;
                    }
                    else
                    {
                        lblError.Text = "";
                        gvCompensationApprovals.Visible = true;
                        gvCompensationApprovals.DataSource = dsSorting;
                        gvCompensationApprovals.DataBind();
                    }
                }
                gvCompensationApprovals.DataSource = dsSorting.Tables[0];
                gvCompensationApprovals.DataBind();
                DataTable dt = dsSorting.Tables[0];
                DataView dv = new DataView(dt);

                if ((ViewState["Order"] == null))
                {
                    ViewState["Order"] = "DESC";
                }
                else if (ViewState["Order"].ToString() == "DESC")
                {
                    ViewState["Order"] = "ASC";
                }
                else if (ViewState["Order"].ToString() == "ASC")
                {
                    ViewState["Order"] = "DESC";
                }

                string strOrder = this.ViewState["Order"].ToString();

                dv.Sort = e.SortExpression + " " + strOrder;

                gvCompensationApprovals.DataSource = dv;
                gvCompensationApprovals.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "gvCompensationApprovals_Sorting", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Sorting

        #region UpdateEmployeeLeaveAndComp

        public void UpdateEmployeeLeaveAndComp()
        {
            try
            {
                rowsAffected = objLeaveDeatilsBOL.UpdateEmployeeLeaveAndComp(objLeaveDeatilsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "UpdateEmployeeLeaveAndComp", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion UpdateEmployeeLeaveAndComp

        #region SendingMailToReportingPerson

        public void SendingMailToReportingPerson()
        {
            try
            {
                dsReportingTo = objLeaveDeatilsBOL.GetReportingTo(objLeaveDeatilsModel);
                if (dsReportingTo.Tables[0].Rows.Count > 0)
                {
                    //Send  new password to the employee through Email.
                    MailMessage objMailMessage = new MailMessage();

                    objMailMessage.To.Add(new MailAddress(dsReportingTo.Tables[0].Rows[0]["EmployeeEmailID"].ToString()));

                    string strBody;
                    dsCancelDetails = objCompensationBOL.GetCancelCompOffDetails(objCompensationModel);

                    for (int k = 0; k < dsReportingTo.Tables[1].Rows.Count; k++)
                    {
                        if (dsReportingTo.Tables[1].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                        {
                            objMailMessage.From = new MailAddress(dsReportingTo.Tables[1].Rows[k]["ConfigItemValue"].ToString(), dsReportingTo.Tables[1].Rows[k]["ConfigItemValue"].ToString());
                        }
                        if (dsReportingTo.Tables[1].Rows[k]["ConfigItemName"].ToString() == "Mail Server Name")
                        {
                            break;
                        }
                    }

                    objMailMessage.Subject = "Updated Compensatory Leave Details";

                    strBody = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi ##EmployeeName##," + " \n\n" + " Updated Compensatory Leave Application Details: " + " \n\n" + "  Applied For: ##AppliedFor##" + " \n" + "  Reason: ##Reason##" + "\n\n" + "Update the Approved Compensatory Leave Details, the required updates are made in the system.";
                    strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);
                    strBody = Regex.Replace(strBody, "##ApproverName##", dsReportingTo.Tables[0].Rows[0]["ReporterName"].ToString());
                    strBody = Regex.Replace(strBody, "##EmployeeName##", dsReportingTo.Tables[0].Rows[0]["EmployeeName"].ToString());
                    strBody = Regex.Replace(strBody, "##AppliedFor##", Convert.ToDateTime(dsCancelDetails.Tables[0].Rows[0]["AppliedFor"].ToString()).ToShortDateString());
                    strBody = Regex.Replace(strBody, "##Reason##", dsCancelDetails.Tables[0].Rows[0]["Reason"].ToString());

                    objMailMessage.Body = strBody;

                    SMTPHelper objhelper = new SMTPHelper();
                    objhelper.SendMail(objMailMessage.From.ToString(), objMailMessage.To.ToString(), objMailMessage.Subject, objMailMessage.Body, null, null, 1);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApproval.aspx.cs", "SendingMailToReportingPerson", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion SendingMailToReportingPerson
    }
}