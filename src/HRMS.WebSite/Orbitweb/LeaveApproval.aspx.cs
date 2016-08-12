using HRMS.Notification;
using System;
using System.Data;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Workflow.Runtime;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;
using V2.Orbit.Workflow.LeaveDetailsWF;

namespace HRMS.Orbitweb
{
    public partial class LeaveApproval : System.Web.UI.Page
    {
        #region Variables Declaration

        private LeaveDetailsBOL objLeaveDeatilsBOL = new LeaveDetailsBOL();
        private LeaveDetailsModel objLeaveDetailsModel = new LeaveDetailsModel();
        private LeaveTransactionBOL objLeaveTransDetailsBOL = new LeaveTransactionBOL();
        private LeaveTransactionModel objLeaveTransDetailsModel = new LeaveTransactionModel();
        private HolidayMasterModel objHolidayModel = new HolidayMasterModel();
        private HolidayMasterBOL objHolidayBOL = new HolidayMasterBOL();
        private DataSet dsGetLeaveDetails;
        private DataSet dsGetLeaveStatus, dsSearchLeaveDetails, dsTotalLeaves, dsHolidaysList, dsAllDetails, dsSorting, dsConfigItem, dsCheckInSignIn, dsReportingTo, dsCancelDetails;
        private DataTable dtGetLeaveStatus;
        private string UserID;
        private string leaveuserid;
        private DateTime leaveDate;
        private string lblgrvStatusNameText = string.Empty, strTotalLeaveBalance = string.Empty;
        private DateTime FromDate, ToDate;
        private double TotalLeaves = 0;
        private int j = 0, TotalLeavesApplyedFor = 0, leaveStatusId = 0;
        private int BalanceLeaves, absent;
        private double CorrectionLeaves;
        private String[] strLeaves, TotalLeavesBalance;
        private bool oneLeaveWeekend = false;
        private int rowsAffected;
        private RosterPlanningBOL ObjRosterPlanningBOL = new RosterPlanningBOL();
        private RosterPlanningModel objRosterPlanningModel = new RosterPlanningModel();
        private DataSet dsEmployeeRole = null;
        private DataSet dsEmployeeShiftDetail = null;
        private Boolean isShiftEmployee = false;
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        #endregion Variables Declaration

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string PageName = "Leave Approval";
                objpagelevel.PageLevelAccess(PageName);

                UserID = User.Identity.Name.ToString();
                leaveuserid = null;
                if (!IsPostBack)
                {
                    if (Request.QueryString["userid"] != null)
                        leaveuserid = Request.QueryString["userid"];
                    if (Request.QueryString["leavedate"] != null)
                        leaveDate = Convert.ToDateTime(Request.QueryString["leavedate"]);
                }

                lblError.Text = "";
                lblSuccess.Text = "";
                if (!IsPostBack)
                {
                    FillddlLeaveStatus();
                    GetLeaveDetails();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Page Load

        #region GetLeaveDetails

        public void GetLeaveDetails()
        {
            try
            {
                dsGetLeaveDetails = new DataSet();
                objLeaveDetailsModel.UserID = Convert.ToInt32(UserID.ToString());
                objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlLeaveStatus.SelectedValue);

                dsGetLeaveDetails = objLeaveDeatilsBOL.GetTeamMembersLeaveDetails(objLeaveDetailsModel);
                dsConfigItem = objLeaveDeatilsBOL.GetTeamMembersLeaveDetails(objLeaveDetailsModel);
                if (dsGetLeaveDetails.Tables[0].Rows.Count > 0)
                {
                    gvLeaveApprovals.DataSource = dsGetLeaveDetails.Tables[0];
                    gvLeaveApprovals.DataBind();
                    if (leaveDate != null && leaveuserid != null)
                    {
                        for (int i = 0; i < dsGetLeaveDetails.Tables[0].Rows.Count; i++)
                        {
                            if (dsGetLeaveDetails.Tables[0].Rows[i]["UserID"].ToString() == Convert.ToString(leaveuserid))
                            {
                                DateTime leaveDateFrom = Convert.ToDateTime(dsGetLeaveDetails.Tables[0].Rows[i]["LeaveDateFrom"].ToString());
                                DateTime LeaveDateTo = Convert.ToDateTime(dsGetLeaveDetails.Tables[0].Rows[i]["LeaveDateTo"].ToString());
                                if ((leaveDateFrom <= leaveDate) && (LeaveDateTo >= leaveDate))
                                {
                                    gvLeaveApprovals.EditIndex = i;
                                    gvLeaveApprovals.DataBind();
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (dsGetLeaveDetails.Tables[0].Rows.Count == 0)
                {
                    gvLeaveApprovals.DataSource = dsGetLeaveDetails;
                    gvLeaveApprovals.DataBind();
                    lblError.Visible = true;
                    lblError.Text = "No records found";
                }
                else
                {
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "No records found";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "GetLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion GetLeaveDetails

        #region GetLeaveStatus

        public void FillddlLeaveStatus()
        {
            try
            {
                dsGetLeaveStatus = objLeaveDeatilsBOL.GetStatusDetails();

                for (int i = 0; i < dsGetLeaveStatus.Tables[0].Rows.Count; i++)
                {
                    ddlLeaveStatus.Items.Add(new ListItem(dsGetLeaveStatus.Tables[0].Rows[i]["StatusName"].ToString(), dsGetLeaveStatus.Tables[0].Rows[i]["StatusID"].ToString()));
                }
                ddlLeaveStatus.Items.Add(new ListItem("All", "0"));
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "FillddlLeaveStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion GetLeaveStatus

        #region DeleteLeaveTransDetails

        public void DeleteLeaveTransDetails()
        {
            try
            {
                rowsAffected = objLeaveTransDetailsBOL.DeleteLeaveTransactionDetails(objLeaveTransDetailsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "DeleteLeaveTransDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion DeleteLeaveTransDetails

        #region UpdateTMLeaveDetails

        public void UpdateTMLeaveDetails()
        {
            try
            {
                rowsAffected = objLeaveDeatilsBOL.UpdateTMLeaveDetails(objLeaveDetailsModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "UpdateTMLeaveDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion UpdateTMLeaveDetails

        #region UpdateLeaveDetailsForFuture

        public void UpdateLeaveDetailsForFuture()
        {
            try
            {
                rowsAffected = objLeaveDeatilsBOL.UpdateLeaveDetailsForFuture(objLeaveDetailsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "UpdateLeaveDetailsForFuture", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion UpdateLeaveDetailsForFuture

        #region UpdateLeaveTransDetails

        public void UpdateLeaveTransDetails()
        {
            try
            {
                rowsAffected = objLeaveTransDetailsBOL.UpdateLeaveTransactionDetails(objLeaveTransDetailsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "UpdateLeaveTransDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion UpdateLeaveTransDetails

        #region UpdateFutureLeaves

        protected void UpdateFutureLeaves()
        {
            try
            {
                //Correction done by Anushree Tirwadkar on 9/9/2011

                string strTotalLeaveBalance = string.Empty;

                String[] TotalLeavesBalance;
                int BalanceLeaves;

                DataSet dsTotalLeaves = objLeaveDeatilsBOL.TotalLeaveBalance(objLeaveDetailsModel);
                strTotalLeaveBalance = Convert.ToString(dsTotalLeaves.Tables[0].Rows[0]["Leave_Balance"].ToString());
                TotalLeavesBalance = strTotalLeaveBalance.Split('.');
                BalanceLeaves = Convert.ToInt32(TotalLeavesBalance[0].ToString());

                int BalanceLeavesForCalculation = BalanceLeaves;
                DataSet dsLeaveTransaction = objLeaveTransDetailsBOL.dsGetLeaveTransactionForFuture(objLeaveDetailsModel);

                String strDescription = String.Empty;
                if (dsLeaveTransaction.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < dsLeaveTransaction.Tables[1].Rows.Count; i++)
                    {
                        if (dsLeaveTransaction.Tables[1].Rows.Count > 1)
                        {
                            i = dsLeaveTransaction.Tables[1].Rows.Count - 1;
                        }
                        for (int k = 0; k < dsLeaveTransaction.Tables[0].Rows.Count; k++)
                        {
                            if (dsLeaveTransaction.Tables[1].Rows[i]["LeaveDetailId"].ToString() != objLeaveDetailsModel.LeaveDetailsID.ToString())
                            {
                                strDescription = dsLeaveTransaction.Tables[0].Rows[k]["Description"].ToString();
                                strDescription = strDescription.Replace("Absent To Leave:", "");
                                strDescription = strDescription.Replace("Absent To Leave", "");
                                strDescription = strDescription.Replace("Leave:", "");
                                strDescription = strDescription.Replace("Absent:", "");

                                //Here Adding Leave records to LeaveTransaction table
                                objLeaveTransDetailsModel.UserID = Convert.ToInt32(dsLeaveTransaction.Tables[0].Rows[k]["UserID"].ToString());
                                objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(dsLeaveTransaction.Tables[0].Rows[k]["LeaveDetailId"].ToString());
                                objLeaveTransDetailsModel.TransactionDate = Convert.ToDateTime(dsLeaveTransaction.Tables[0].Rows[k]["TransactionDate"].ToString());
                                objLeaveTransDetailsModel.LeaveType = Convert.ToBoolean(dsLeaveTransaction.Tables[0].Rows[k]["LeaveType"].ToString());
                                objLeaveTransDetailsModel.LeaveTransactionID = Convert.ToInt32(dsLeaveTransaction.Tables[0].Rows[k]["LeaveTransactionID"].ToString());

                                int totalleavesappliedfor = TotalLeavesApplyedFor;
                                int CurrentBalance = 0;
                                int OnDayBalance = 0;
                                bool leaveFlag = false;
                                if (BalanceLeaves > 0)
                                {
                                    objLeaveTransDetailsModel.Quantity = Convert.ToDecimal("-" + 1);
                                    objLeaveTransDetailsModel.Description = "Leave:" + strDescription;

                                    BalanceLeaves--;
                                }
                                else
                                {
                                    objLeaveTransDetailsModel.Quantity = Convert.ToDecimal(0);
                                    objLeaveTransDetailsModel.Description = "Absent:" + strDescription;
                                }
                                objLeaveTransDetailsBOL.UpdateLeaveTransactionDetailsForFuture(objLeaveTransDetailsModel);
                            }
                        }
                    }
                }

                for (int m = 0; m < dsLeaveTransaction.Tables[1].Rows.Count; m++)
                {
                    objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(dsLeaveTransaction.Tables[1].Rows[m]["LeaveDetailId"].ToString());
                    objLeaveDetailsModel.UserID = Convert.ToInt32(dsLeaveTransaction.Tables[1].Rows[m]["UserId"].ToString());
                    DataSet dsLeaveTransactionForspecificLeave = objLeaveTransDetailsBOL.GetLeaveTransactionForspecificLeave(objLeaveTransDetailsModel);
                    int TotalLeaveDays = 0;
                    int LeaveCorrectionDays = 0;
                    for (int i = 0; i < dsLeaveTransactionForspecificLeave.Tables[0].Rows.Count; i++)
                    {
                        if (dsLeaveTransactionForspecificLeave.Tables[0].Rows[i]["Quantity"].ToString() == "-1.0")
                        {
                            TotalLeaveDays = TotalLeaveDays + 1;
                        }
                        if (dsLeaveTransactionForspecificLeave.Tables[0].Rows[i]["Quantity"].ToString() == "0.0")
                        {
                            LeaveCorrectionDays = LeaveCorrectionDays + 1;
                        }
                    }

                    objLeaveDetailsModel.LeaveDetailsID = Convert.ToInt32(dsLeaveTransactionForspecificLeave.Tables[0].Rows[0]["ReasonID"].ToString());
                    objLeaveDetailsModel.TotalLeaveDays = TotalLeaveDays;
                    objLeaveDetailsModel.LeaveCorrectionDays = LeaveCorrectionDays;

                    UpdateLeaveDetailsForFuture();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval", "UpdateFutureLeaves", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion UpdateFutureLeaves

        #region AddLeaveTransDetails

        public void AddLeaveTransDetails()
        {
            try
            {
                rowsAffected = objLeaveTransDetailsBOL.AddLeaveTransactionDetails(objLeaveTransDetailsModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "AddLeaveTransDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion AddLeaveTransDetails

        #region Search

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlLeaveStatus.SelectedValue);
                objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.ToString());
                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.ToString());
                objLeaveDetailsModel.UserID = Convert.ToInt32(UserID.ToString());

                SearchTeamMembersLeaveDetails();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "btnSearch_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Search

        #region SearchTeamMembersLeaveDetails

        public void SearchTeamMembersLeaveDetails()
        {
            try
            {
                dsSearchLeaveDetails = objLeaveDeatilsBOL.SearchTeamMembersLeaveDetails(objLeaveDetailsModel);
                dsConfigItem = objLeaveDeatilsBOL.SearchTeamMembersLeaveDetails(objLeaveDetailsModel);
                if (dsSearchLeaveDetails.Tables[0].Rows.Count <= 0)
                {
                    lblSuccess.Visible = false;
                    lblError.Text = "No records found";
                    gvLeaveApprovals.Visible = false;
                }
                else
                {
                    lblError.Text = "";
                    gvLeaveApprovals.Visible = true;
                    gvLeaveApprovals.DataSource = dsSearchLeaveDetails.Tables[0];
                    gvLeaveApprovals.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "SearchTeamMembersLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion SearchTeamMembersLeaveDetails

        #region RowCommand

        protected void gvLeaveApprovals_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "lbnCancel")
                {
                    gvLeaveApprovals.EditIndex = -1;
                    gvLeaveApprovals.ShowFooter = true;
                    GetLeaveDetails();
                    if (txtSearchFromDate.Text != "" && txtSearchFromDate.Text != "")
                    {
                        objLeaveDetailsModel.UserID = Convert.ToInt32(UserID);
                        objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlLeaveStatus.SelectedValue);
                        objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                        objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                        gvLeaveApprovals.EditIndex = -1;
                        SearchTeamMembersLeaveDetails();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "gvLeaveApprovals_RowCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion RowCommand

        #region RowCancelingEdit

        protected void gvLeaveApprovals_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvLeaveApprovals.EditIndex = -1;
                gvLeaveApprovals.ShowFooter = true;
                GetLeaveDetails();
                if (txtSearchFromDate.Text != "" && txtSearchFromDate.Text != "")
                {
                    objLeaveDetailsModel.UserID = Convert.ToInt32(UserID);
                    objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlLeaveStatus.SelectedValue);
                    objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    gvLeaveApprovals.EditIndex = -1;
                    SearchTeamMembersLeaveDetails();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "gvLeaveApprovals_RowCancelingEdit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion RowCancelingEdit

        #region RowEditing

        protected void gvLeaveApprovals_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvLeaveApprovals.EditIndex = e.NewEditIndex;

                GetLeaveDetails();
                if (txtSearchFromDate.Text != "" && txtSearchFromDate.Text != "")
                {
                    objLeaveDetailsModel.UserID = Convert.ToInt32(UserID);
                    objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlLeaveStatus.SelectedValue);
                    objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    SearchTeamMembersLeaveDetails();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "gvLeaveApprovals_RowEditing", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion RowEditing

        #region RowUpdating

        protected void gvLeaveApprovals_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                bool WfApprovedLocked = false;
                GridViewRow row = gvLeaveApprovals.Rows[e.RowIndex];

                Label lblLeaveDetailID = row.FindControl("lblLeaveDetailID1") as Label;
                objLeaveDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text.Trim());

                Label lblLeaveDetailWFID = row.FindControl("lblLeaveDetailWFID1") as Label;
                objLeaveDetailsModel.WorkFlowID = new Guid(lblLeaveDetailWFID.Text.Trim());

                Label lblgrvEditUserName = row.FindControl("lblgrvEditUserName") as Label;

                TextBox txtgrvFormDate = row.FindControl("txtgrvFormDate") as TextBox;
                objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtgrvFormDate.Text.Trim());

                TextBox txtgrvToDate = row.FindControl("txtgrvToDate") as TextBox;
                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtgrvToDate.Text.Trim());

                Label lblgrvLeaveReason = row.FindControl("lblgrvLeaveReason") as Label;
                objLeaveDetailsModel.LeaveResason = lblgrvLeaveReason.Text.Trim();

                DropDownList ddlStatusID = row.FindControl("ddlgrvStatusName") as DropDownList;
                objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatusID.SelectedValue);
                if (ddlStatusID.Visible == false)
                {
                    WfApprovedLocked = true;
                }

                objLeaveDetailsModel.ApproverID = Convert.ToInt32(UserID.ToString());

                TextBox txtgrvApproverComments = row.FindControl("txtgrvApproverComments") as TextBox;
                objLeaveDetailsModel.ApproverComments = txtgrvApproverComments.Text.Trim();

                Label lblTotalLeaves = row.FindControl("lblTotalLeaves") as Label;
                Label lblTotalAbsentLeaves = row.FindControl("lblTotalAbsentLeaves") as Label;
                Label lblEditUserID = row.FindControl("lblEditUserID") as Label;

                objLeaveDetailsModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                Label lblTMmember = row.FindControl("lblEditUserID") as Label;
                objLeaveDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());

                //Getting Total Leaves
                objLeaveDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());

                //Getting All the Holidays Details
                //dsHolidaysList = objHolidayBOL.bindData();
                objHolidayModel.UserID = objLeaveDetailsModel.UserID;
                objHolidayModel.StartDate = objLeaveDetailsModel.LeaveDateFrom;
                objHolidayModel.EndDate = objLeaveDetailsModel.LeaveDateTo;
                dsHolidaysList = objHolidayBOL.bindHolidaysForLeaveApprovals(objHolidayModel);

                //Check the leaves dates are in SignInSignOut Table
                dsCheckInSignIn = objLeaveDeatilsBOL.CheckLeavesinSignIn(objLeaveDetailsModel);

                //For Getting ConfigDate from Database
                dsConfigItem = objLeaveDeatilsBOL.GetLeaveDetails(objLeaveDetailsModel);
                DateTime ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                FromDate = Convert.ToDateTime(txtgrvFormDate.Text.Trim());
                ToDate = Convert.ToDateTime(txtgrvToDate.Text.Trim());
                TimeSpan ts = ToDate - FromDate;

                TotalLeaves = ts.TotalDays + 1;
                strLeaves = new String[Convert.ToInt32(TotalLeaves)];
                leaveStatusId = Convert.ToInt32(ddlStatusID.SelectedValue);

                if (ddlStatusID.SelectedValue == Convert.ToString("3"))
                {
                    //Here delete the all records of related leavedetailsID
                    objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                    objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text);
                    DeleteLeaveTransDetails();
                    UpdateEmployeeLeaveAndComp();

                    objLeaveDetailsModel.TotalLeaveDays = Convert.ToInt32(lblTotalLeaves.Text);
                    objLeaveDetailsModel.LeaveCorrectionDays = Convert.ToInt32(lblTotalAbsentLeaves.Text);
                    objLeaveDetailsModel.StatusID = Convert.ToInt32(3);
                    UpdateTMLeaveDetails();

                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Leave Details are Rejected";
                    lblError.Text = "";
                    gvLeaveApprovals.EditIndex = -1;

                    SendingMailToReportingPerson();

                    GetLeaveDetails();
                    return;
                }

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
                //general employee
                if (isShiftEmployee == false)
                {
                    for (int i = 0; i < TotalLeaves; i++)
                    {
                        if (FromDate.DayOfWeek.ToString() == "Saturday" || FromDate.DayOfWeek.ToString() == "Sunday")
                        {
                            if (TotalLeaves == 1)
                            {
                                oneLeaveWeekend = true;
                                break;
                            }
                            if (TotalLeaves == 2)
                            {
                                if (FromDate.DayOfWeek.ToString() == "Saturday" && ToDate.DayOfWeek.ToString() == "Sunday")
                                {
                                    oneLeaveWeekend = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Boolean flag = false;

                            for (int k = 0; k < dsHolidaysList.Tables[0].Rows.Count; k++)
                            {
                                if (FromDate.ToString() == dsHolidaysList.Tables[0].Rows[k]["HolidayDate"].ToString())
                                {
                                    flag = true;
                                    break;
                                }
                            }

                            for (int l = 0; l < dsCheckInSignIn.Tables[0].Rows.Count; l++)
                            {
                                DateTime dtSignIn = Convert.ToDateTime(dsCheckInSignIn.Tables[0].Rows[l]["SignInTime"].ToString());
                                DateTime dtFrom = Convert.ToDateTime(FromDate.ToString());

                                if (dtFrom.ToShortDateString() == dtSignIn.ToShortDateString())
                                {
                                    flag = true;
                                    break;
                                }
                                //Here i need to add one message
                            }

                            if (!flag)
                            {
                                strLeaves[j] = FromDate.ToString();
                                j++;
                            }
                        }
                        FromDate = FromDate.AddDays(1);
                    }
                }
                //if shift employee
                else
                {
                    objRosterPlanningModel.FromDate = Convert.ToDateTime(txtgrvFormDate.Text.Trim());
                    objRosterPlanningModel.ToDate = Convert.ToDateTime(txtgrvToDate.Text.Trim());
                    Boolean onweeklyoff = false;
                    dsEmployeeShiftDetail = ObjRosterPlanningBOL.GetEmployeeShiftDetail(objRosterPlanningModel);
                    for (int i = 0; i < TotalLeaves; i++)
                    {
                        if (dsEmployeeShiftDetail.Tables[0].Rows.Count != 0)
                        {
                            for (int k = 0; k < dsEmployeeShiftDetail.Tables[0].Rows.Count; k++)
                            {
                                if (Convert.ToDateTime(dsEmployeeShiftDetail.Tables[0].Rows[k]["WeekOff1"].ToString()) == Convert.ToDateTime(FromDate.ToString()) || (Convert.ToDateTime(dsEmployeeShiftDetail.Tables[0].Rows[k]["WeekOff2"].ToString()) == Convert.ToDateTime(FromDate.ToString())))
                                {
                                    onweeklyoff = true;
                                    break;
                                }
                                else
                                {
                                    onweeklyoff = false;
                                }
                            }
                        }
                        if (onweeklyoff == true)
                        {
                            if (TotalLeaves == 1)
                            {
                                oneLeaveWeekend = true;
                                break;
                            }

                            if (TotalLeaves == 2)
                            {
                                if (Convert.ToDateTime(dsEmployeeShiftDetail.Tables[0].Rows[0]["WeekOff1"].ToString()) == Convert.ToDateTime(txtgrvFormDate.Text.Trim()) && (Convert.ToDateTime(dsEmployeeShiftDetail.Tables[0].Rows[0]["WeekOff2"].ToString()) == Convert.ToDateTime(txtgrvToDate.Text.Trim())))
                                {
                                    oneLeaveWeekend = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Boolean flag = false;

                            for (int k = 0; k < dsEmployeeShiftDetail.Tables[1].Rows.Count; k++)
                            {
                                if (FromDate.ToString() == dsEmployeeShiftDetail.Tables[1].Rows[k]["HolidayDate"].ToString())
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            for (int l = 0; l < dsCheckInSignIn.Tables[0].Rows.Count; l++)
                            {
                                DateTime dtSignIn = Convert.ToDateTime(dsCheckInSignIn.Tables[0].Rows[l]["SignInTime"].ToString());
                                DateTime dtFrom = Convert.ToDateTime(FromDate.ToString());

                                if (dtFrom.ToShortDateString() == dtSignIn.ToShortDateString())
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                            {
                                strLeaves[j] = FromDate.ToString();
                                j++;
                            }
                        }
                        FromDate = FromDate.AddDays(1);
                    }
                }

                if (oneLeaveWeekend == true)
                {
                    lblError.Visible = true;
                    lblError.Text = lblgrvEditUserName.Text + " has  already Signed In for these dates / It is in Holiday List / Sat/Sun, So you can't approve leaves for these dates";
                    gvLeaveApprovals.EditIndex = -1;
                    GetLeaveDetails();
                    return;
                }
                TotalLeavesApplyedFor = j++;
                int totalleavesappliedfor = TotalLeavesApplyedFor;
                int CurrentBalance = 0;
                int OnDayBalance = 0;
                bool leaveFlag = false;

                if (TotalLeavesApplyedFor == 0)
                {
                    lblError.Text = lblgrvEditUserName.Text + " has already Signed In for these dates / It is in Holiday List / Sat/Sun, So you can't approve leaves for these dates";
                    gvLeaveApprovals.EditIndex = -1;
                    GetLeaveDetails();
                    return;
                }
                else
                {
                    if (Convert.ToDateTime(txtgrvFormDate.Text.Trim()) < Convert.ToDateTime(DateTime.Now))
                    {
                        DataSet dsLeavebalanceforGivenDate = objLeaveDeatilsBOL.GetLeaveBalanceForGivenDate(objLeaveDetailsModel.UserID, objLeaveDetailsModel.LeaveDateFrom, objLeaveDetailsModel.LeaveDateTo);
                        CurrentBalance = Convert.ToInt32(dsLeavebalanceforGivenDate.Tables[0].Rows[0]["CurrentBalance"]);
                        OnDayBalance = Convert.ToInt32(dsLeavebalanceforGivenDate.Tables[0].Rows[0]["OnDayBalance"]);
                        if (CurrentBalance > 0)
                        {
                            if (TotalLeavesApplyedFor > OnDayBalance)
                            {
                                int diff = TotalLeavesApplyedFor - OnDayBalance;
                                TotalLeavesApplyedFor = TotalLeavesApplyedFor - diff;
                                leaveFlag = true;
                            }
                        }
                        else
                        {
                            TotalLeavesApplyedFor = 0;
                            leaveFlag = true;
                        }
                    }
                }

                if (ddlStatusID.SelectedValue == Convert.ToString("2"))
                {
                    objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                    objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text);
                }

                if (Convert.ToDateTime(txtgrvToDate.Text.Trim()) < Convert.ToDateTime(DateTime.Now))
                    objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(DateTime.Now);
                else
                    objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtgrvToDate.Text.Trim());

                dsTotalLeaves = objLeaveDeatilsBOL.TotalLeaveBalance(objLeaveDetailsModel);
                if (dsTotalLeaves.Tables[0].Rows[0]["Leave_Balance"].ToString() == "")
                {
                    strTotalLeaveBalance = Convert.ToString("0");
                }
                else
                {
                    strTotalLeaveBalance = Convert.ToString(dsTotalLeaves.Tables[0].Rows[0]["Leave_Balance"].ToString());
                    string Error = Convert.ToString(strTotalLeaveBalance.ToString().StartsWith("-"));
                    if (Error.ToString() == "True")
                    {
                        strTotalLeaveBalance = Convert.ToString("0");
                    }
                }

                TotalLeavesBalance = strTotalLeaveBalance.Split('.');
                BalanceLeaves = Convert.ToInt32(TotalLeavesBalance[0].ToString());

                CorrectionLeaves = BalanceLeaves - TotalLeavesApplyedFor;

                string error = Convert.ToString(CorrectionLeaves.ToString().StartsWith("-"));

                if (error.ToString() == "True")
                {
                    if (BalanceLeaves <= 0)
                    {
                        BalanceLeaves = Convert.ToInt32(0);
                        objLeaveDetailsModel.TotalLeaveDays = Convert.ToInt32(BalanceLeaves);
                        objLeaveDetailsModel.LeaveCorrectionDays = Convert.ToInt32(TotalLeavesApplyedFor.ToString());
                        absent = (Convert.ToInt32(TotalLeavesApplyedFor.ToString()));
                    }
                    else
                    {
                        objLeaveDetailsModel.LeaveCorrectionDays = -(float)(Convert.ToDecimal(CorrectionLeaves.ToString())); ;
                        objLeaveDetailsModel.TotalLeaveDays = (float)(Convert.ToDecimal(BalanceLeaves));
                        absent = -(Convert.ToInt32(CorrectionLeaves.ToString()));
                    }
                    if (ConfigdateTime.Date >= Convert.ToDateTime(txtgrvFormDate.Text).Date)
                    {
                        lblError.Text = "Administrator has frozen the data for the selected date";
                        gvLeaveApprovals.EditIndex = -1;
                        GetLeaveDetails();
                        return;
                    }
                    else
                    {
                        objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtgrvToDate.Text.Trim());
                        UpdateTMLeaveDetails();
                        lblError.Text = lblgrvEditUserName.Text + " has applied for " + TotalLeavesApplyedFor + " leaves, but his/her Leave balance is " + BalanceLeaves + ", So " + absent + " days will be marked as absent.";
                    }

                    for (int k = 0; k < TotalLeavesApplyedFor; k++)
                    {
                        if (strLeaves[k] == null)
                        {
                            break;
                        }
                        else
                        {
                            if (ddlStatusID.SelectedValue == Convert.ToString("2"))
                            {
                                //Here Adding Leave records to LeaveTransaction table
                                objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                                objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text);
                                objLeaveTransDetailsModel.TransactionDate = Convert.ToDateTime(strLeaves[k].ToString());
                                objLeaveTransDetailsModel.LeaveType = Convert.ToBoolean(1);

                                if (BalanceLeaves > 0)
                                {
                                    objLeaveTransDetailsModel.Quantity = Convert.ToDecimal("-" + 1);
                                    objLeaveTransDetailsModel.Description = "Leave:" + lblgrvLeaveReason.Text.ToString();
                                    AddLeaveTransDetails();
                                    UpdateEmployeeLeaveAndComp();
                                    BalanceLeaves--;
                                }
                                else
                                {
                                    objLeaveTransDetailsModel.Quantity = Convert.ToDecimal(0);
                                    objLeaveTransDetailsModel.Description = "Absent:" + lblgrvLeaveReason.Text.ToString();
                                    AddLeaveTransDetails();
                                    UpdateEmployeeLeaveAndComp();
                                }
                            }
                        }
                    }
                    if (ddlStatusID.SelectedValue == Convert.ToString("2"))
                    {
                        if (Convert.ToDateTime(txtgrvToDate.Text.Trim()) < Convert.ToDateTime(DateTime.Now))
                            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(DateTime.Now);
                        else
                            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtgrvToDate.Text.Trim());

                        UpdateFutureLeaves();
                    }
                }
                else
                {
                    if (ConfigdateTime.Date >= Convert.ToDateTime(txtgrvFormDate.Text).Date)
                    {
                        lblError.Text = "Administrator has frozen the data for the selected date";

                        gvLeaveApprovals.EditIndex = -1;
                        GetLeaveDetails();
                        return;
                    }
                    else
                    {
                        if (leaveFlag)
                        {
                            absent = totalleavesappliedfor - TotalLeavesApplyedFor;
                            objLeaveDetailsModel.TotalLeaveDays = Convert.ToInt32(TotalLeavesApplyedFor);
                            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtgrvToDate.Text.Trim());
                            objLeaveDetailsModel.LeaveCorrectionDays = absent;
                            UpdateTMLeaveDetails();
                        }
                        else
                        {


                            objLeaveDetailsModel.TotalLeaveDays = TotalLeavesApplyedFor;
                            objLeaveDetailsModel.LeaveCorrectionDays = Convert.ToInt32(0);

                            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtgrvToDate.Text.Trim());
                            UpdateTMLeaveDetails();

                        }

                    }


                    for (int k = 0; k < strLeaves.Length; k++)
                    {
                        if (strLeaves[k] == null)
                        {
                            break;
                        }
                        else
                        {
                            if (leaveFlag)
                            {
                                if (ddlStatusID.SelectedValue == Convert.ToString("2"))
                                {
                                    // Here Adding Leave records to LeaveTransaction table
                                    objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                                    objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text);
                                    objLeaveTransDetailsModel.TransactionDate = Convert.ToDateTime(strLeaves[k].ToString());
                                    objLeaveTransDetailsModel.LeaveType = Convert.ToBoolean(1);

                                    if (TotalLeavesApplyedFor > 0)
                                    {
                                        objLeaveTransDetailsModel.Quantity = Convert.ToDecimal("-" + 1);
                                        objLeaveTransDetailsModel.Description = "Leave:" + lblgrvLeaveReason.Text.ToString();
                                    }
                                    else
                                    {
                                        objLeaveTransDetailsModel.Quantity = Convert.ToDecimal(0);
                                        objLeaveTransDetailsModel.Description = "Absent:" + lblgrvLeaveReason.Text.ToString();
                                    }

                                    AddLeaveTransDetails();
                                    UpdateEmployeeLeaveAndComp();
                                    TotalLeavesApplyedFor--;
                                }
                            }
                            else
                            {
                                if (ddlStatusID.SelectedValue == Convert.ToString("2"))
                                {
                                    //Here Adding Leave records to LeaveTransaction table
                                    objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                                    objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text);
                                    objLeaveTransDetailsModel.TransactionDate = Convert.ToDateTime(strLeaves[k].ToString());
                                    objLeaveTransDetailsModel.Description = "Leave:" + lblgrvLeaveReason.Text.ToString();
                                    objLeaveTransDetailsModel.Quantity = Convert.ToDecimal("-" + 1);
                                    objLeaveTransDetailsModel.LeaveType = Convert.ToBoolean(1);

                                    AddLeaveTransDetails();
                                    UpdateEmployeeLeaveAndComp();
                                }
                            }

                        }
                    }
                    if (ddlStatusID.SelectedValue == Convert.ToString("2"))
                    {
                        if (Convert.ToDateTime(txtgrvToDate.Text.Trim()) < Convert.ToDateTime(DateTime.Now))
                            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(DateTime.Now);
                        else
                            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtgrvToDate.Text.Trim());

                        UpdateFutureLeaves();
                    }
                }

                if (WfApprovedLocked == false)
                {
                    if (ddlStatusID.SelectedValue == Convert.ToString("2"))
                    {
                        SendingMailToReportingPerson();
                    }
                    if (ddlStatusID.SelectedValue == Convert.ToString("3"))
                    {
                        SendingMailToReportingPerson();
                    }
                }
                else
                {
                    if (ddlStatusID.SelectedValue == Convert.ToString("2"))
                    {
                        //Sending a Mail to Employee after Updating Leaves
                        if (WfApprovedLocked == false)
                        {
                            objLeaveDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                            SendingMailToReportingPerson();
                        }
                    }
                }
                gvLeaveApprovals.EditIndex = -1;

                GetLeaveDetails();

                lblSuccess.Visible = true;
                lblSuccess.Text = "Leave details are updated successfully";

                if (ddlStatusID.SelectedValue == Convert.ToString("4"))
                {
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Leave Details are Cancelled";
                    lblError.Text = "";
                }
                if (ddlStatusID.SelectedValue == Convert.ToString("3"))
                {
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Leave Details are Rejected";
                    lblError.Text = "";
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
                if (ex.Message.CompareTo("Already leave applied for this dates.") != 0)
                {
                    lblError.Visible = false;
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "btnSubmit", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "User has already applied for leaves for these dates.";
                }
            }
        }

        public void StartWorkflow(int LeaveDetails, Guid gLeaveDetailsWFID)
        {
            try
            {
                WorkflowRuntime wr = (WorkflowRuntime)Application["WokflowRuntime"];

                WorkflowInstance wi = wr.GetWorkflow(gLeaveDetailsWFID);
                wi.Resume();
                LeaveDetailsService objLeaveDetailsService = (LeaveDetailsService)wr.GetService(typeof(LeaveDetailsService));
                objLeaveDetailsService.RaiseApproveEvent(wi.InstanceId);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "StartWorkflow", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion RowUpdating

        #region RowDataBound

        protected void gvLeaveApprovals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblstatus = ((Label)(e.Row.FindControl("lblgrvStatusName")));
                    Label lblgrvFromDate = ((Label)(e.Row.FindControl("lblgrvFromDate")));
                    Label lblgrvToDate = ((Label)(e.Row.FindControl("lblgrvToDate")));
                    Label lblApproved = ((Label)(e.Row.FindControl("lblApproved")));
                    LinkButton lnkEdit = ((LinkButton)(e.Row.FindControl("lnkbutEdit")));

                    DateTime ConfigDate = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());
                    if (ConfigDate.Date >= Convert.ToDateTime(lblgrvFromDate.Text).Date)
                    {
                        if (lblstatus.Text == "Approved")
                        {
                            lblApproved.Visible = true;
                            lblApproved.Text = "Aprroved";
                            lnkEdit.Visible = false;
                        }
                        else if (lblstatus.Text == "Cancelled")
                        {
                            lblApproved.Visible = true;
                            lblApproved.Text = "Leave Cancelled";
                            lnkEdit.Visible = false;
                        }
                        else if (lblstatus.Text == "Rejected")
                        {
                            lblApproved.Visible = true;
                            lblApproved.Text = "Leave Rejected";
                            lnkEdit.Visible = false;
                        }
                        else if (lblstatus.Text == "Pending")
                        {
                            lblApproved.Visible = true;
                            lblApproved.Text = "Leave Pending";
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
                                lblApproved.Text = "Leave Cancelled";
                                lnkEdit.Visible = false;
                            }
                            else if (lblstatus.Text == "Rejected")
                            {
                                lblApproved.Visible = true;
                                lblApproved.Text = "Leave Rejected";
                                lnkEdit.Visible = false;
                            }
                        }
                    }
                }

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    TextBox txtApproversComment = ((TextBox)e.Row.FindControl("txtgrvApproverComments"));
                    TextBox txtFromDate = ((TextBox)e.Row.FindControl("txtgrvFormDate"));
                    TextBox txtToDate = ((TextBox)e.Row.FindControl("txtgrvToDate"));
                    LinkButton lnkUpdate = ((LinkButton)e.Row.FindControl("lbnUpdate"));

                    lnkUpdate.Attributes.Add("onClick", "return Validation(" + txtFromDate.ClientID + "," + txtToDate.ClientID + "," + txtApproversComment.ClientID + ");");
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

                    TextBox txtgrvToDate = (TextBox)e.Row.FindControl("txtgrvToDate");
                    TextBox txtgrvFormDate = (TextBox)e.Row.FindControl("txtgrvFormDate");

                    txtgrvToDate.Attributes.Add("onkeydown", "return false");
                    txtgrvFormDate.Attributes.Add("onkeydown", "return false");

                    if (ddlgrvStatusName.SelectedValue == Convert.ToString("2"))
                    {
                        Label lblgrvStatus = (Label)(e.Row.FindControl("lblgrvStatus"));
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "gvLeaveApprovals_RowDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion RowDataBound

        #region Reset

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ddlLeaveStatus.SelectedValue = Convert.ToString("1");
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
                lblSuccess.Text = "";
                lblError.Text = "";

                if (txtSearchFromDate.Text != "" && txtSearchFromDate.Text != "")
                {
                    objLeaveDetailsModel.UserID = Convert.ToInt32(UserID);
                    objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlLeaveStatus.SelectedValue);
                    objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    SearchTeamMembersLeaveDetails();
                }
                else
                {
                    txtSearchFromDate.Text = "";
                    txtSearchToDate.Text = "";
                    objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlLeaveStatus.SelectedValue);
                    objLeaveDetailsModel.UserID = Convert.ToInt32(UserID);
                    dsAllDetails = objLeaveDeatilsBOL.SearchAllTMLeaveDetails(objLeaveDetailsModel);
                    dsConfigItem = objLeaveDeatilsBOL.SearchAllTMLeaveDetails(objLeaveDetailsModel);
                    if (dsAllDetails.Tables[0].Rows.Count <= 0)
                    {
                        lblError.Text = "No records found";
                        lblSuccess.Text = "";
                        gvLeaveApprovals.Visible = false;
                    }
                    else
                    {
                        lblError.Text = "";
                        gvLeaveApprovals.Visible = true;
                        gvLeaveApprovals.DataSource = dsAllDetails.Tables[0];
                        gvLeaveApprovals.DataBind();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Reset

        #region PageIndexChanging

        protected void gvLeaveApprovals_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvLeaveApprovals.PageIndex = e.NewPageIndex;
                gvLeaveApprovals.EditIndex = -1;
                GetLeaveDetails();
                if (txtSearchFromDate.Text != "" && txtSearchFromDate.Text != "")
                {
                    objLeaveDetailsModel.UserID = Convert.ToInt32(UserID);
                    objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlLeaveStatus.SelectedValue);
                    objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    SearchTeamMembersLeaveDetails();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "gvLeaveApprovals_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion PageIndexChanging

        #region Sorting

        protected void gvLeaveApprovals_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                gvLeaveApprovals.EditIndex = -1;
                objLeaveDetailsModel.UserID = Convert.ToInt32(UserID);
                objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlLeaveStatus.SelectedValue);

                dsConfigItem = objLeaveDeatilsBOL.GetTeamMembersLeaveDetails(objLeaveDetailsModel);
                if (txtSearchFromDate.Text != "" && txtSearchFromDate.Text != "")
                {
                    objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    SearchTeamMembersLeaveDetails();
                }
                else
                {
                    dsSearchLeaveDetails = objLeaveDeatilsBOL.GetTeamMembersLeaveDetails(objLeaveDetailsModel);
                    if (dsSearchLeaveDetails.Tables[0].Rows.Count <= 0)
                    {
                        lblError.Text = "No records found";
                        gvLeaveApprovals.Visible = false;
                    }
                    else
                    {
                        lblError.Text = "";
                        gvLeaveApprovals.Visible = true;
                        gvLeaveApprovals.DataSource = dsSearchLeaveDetails.Tables[0];
                        gvLeaveApprovals.DataBind();
                    }
                }
                gvLeaveApprovals.DataSource = dsSearchLeaveDetails.Tables[0];
                gvLeaveApprovals.DataBind();
                DataTable dt = dsSearchLeaveDetails.Tables[0];
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

                gvLeaveApprovals.DataSource = dv;
                gvLeaveApprovals.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "gvLeaveApprovals_Sorting", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Sorting

        #region ddlLeaveStatus_SelectedIndexChanged

        protected void ddlLeaveStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
                objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlLeaveStatus.SelectedValue);
                objLeaveDetailsModel.UserID = Convert.ToInt32(UserID);
                dsAllDetails = objLeaveDeatilsBOL.SearchAllTMLeaveDetails(objLeaveDetailsModel);
                dsConfigItem = objLeaveDeatilsBOL.SearchAllTMLeaveDetails(objLeaveDetailsModel);
                if (dsAllDetails.Tables[0].Rows.Count <= 0)
                {
                    lblError.Text = "No records found";
                    gvLeaveApprovals.Visible = false;
                }
                else
                {
                    lblError.Text = "";
                    gvLeaveApprovals.Visible = true;
                    gvLeaveApprovals.DataSource = dsAllDetails.Tables[0];
                    gvLeaveApprovals.DataBind();
                }
                gvLeaveApprovals.EditIndex = -1;
                GetLeaveDetails();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "ddlLeaveStatus_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion ddlLeaveStatus_SelectedIndexChanged

        #region UpdateEmployeeLeaveAndComp

        public void UpdateEmployeeLeaveAndComp()
        {
            try
            {
                rowsAffected = objLeaveDeatilsBOL.UpdateEmployeeLeaveAndComp(objLeaveDetailsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "UpdateEmployeeLeaveAndComp", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion UpdateEmployeeLeaveAndComp

        #region SendingMailToReportingPerson

        public void SendingMailToReportingPerson()
        {
            try
            {
                dsReportingTo = objLeaveDeatilsBOL.GetReportingTo(objLeaveDetailsModel);
                if (dsReportingTo.Tables[0].Rows.Count > 0)
                {
                    //Send new password to the employee through Email.
                    MailMessage objMailMessage = new MailMessage();
                    SmtpClient smtpClient = new SmtpClient();
                    objMailMessage.Subject = "Updated Leave Details";
                    objMailMessage.To.Add(new MailAddress(dsReportingTo.Tables[0].Rows[0]["EmployeeEmailID"].ToString()));
                    string UserName, ApproverName, Reason, ToDate, FromDate;

                    int Applyleaves;

                    UserName = dsReportingTo.Tables[0].Rows[0]["EmployeeName"].ToString();
                    ApproverName = dsReportingTo.Tables[0].Rows[0]["ReporterName"].ToString();

                    dsCancelDetails = objLeaveDeatilsBOL.GetCancelLeaveDetails(objLeaveDetailsModel);

                    ToDate = Convert.ToDateTime(dsCancelDetails.Tables[0].Rows[0]["LeaveDateTo"].ToString()).ToShortDateString();
                    FromDate = Convert.ToDateTime(dsCancelDetails.Tables[0].Rows[0]["LeaveDateFrom"].ToString()).ToShortDateString();
                    Reason = dsCancelDetails.Tables[0].Rows[0]["LeaveReason"].ToString();
                    Applyleaves = Convert.ToInt32(dsCancelDetails.Tables[0].Rows[0]["TotalLeaveDays"].ToString()) + Convert.ToInt32(dsCancelDetails.Tables[0].Rows[0]["LeaveCorrectionDays"].ToString());

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

                    switch (leaveStatusId)
                    {
                        case 2:
                            objMailMessage.Subject = "Leave Approved Details";
                            objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi " + "<b>" + UserName + "</b>" + " ," + "<br>" + "<br>" + "Leave Approved Details: " + "<br>" + "<br>" + " <br> " + "FromDate: " + FromDate + " <br> " + "ToDate: " + ToDate + " <br> " + "Leave Reason: " + Reason + " <br> " + "Leave Applied For: " + Applyleaves + " <br> " + "Approval Reason: " + objLeaveDetailsModel.ApproverComments + " <br> " + " <br> " + " Update the Leave Approved Details, the required updates are made in the system.";
                            break;

                        case 3:
                            objMailMessage.Subject = "Leave Rejected Details";
                            objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi " + "<b>" + UserName + "</b>" + " ," + "<br>" + "<br>" + "Leave Rejection Details: " + "<br>" + "<br>" + " <br> " + "FromDate: " + FromDate + " <br> " + "ToDate: " + ToDate + " <br> " + "Leave Reason: " + Reason + " <br> " + "Leave Applied For: " + Applyleaves + " <br> " + "Rejection Reason: " + objLeaveDetailsModel.ApproverComments + " <br> " + " <br> " + " Update the Leave Rejected Details, the required updates are made in the system.";
                            break;

                        default:
                            objMailMessage.Subject = "Updated Leave Details";
                            objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi " + "<b>" + UserName + "</b>" + " ," + "<br>" + "<br>" + "Updated Leave Application Details: " + "<br>" + "<br>" + " <br> " + "FromDate: " + FromDate + " <br> " + "ToDate: " + ToDate + " <br> " + "Leave Reason: " + Reason + " <br> " + "Leave Applied For: " + Applyleaves + " <br> " + " <br> " + " Update the Approved Leave Details, the required updates are made in the system.";
                            break;
                    }

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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "SendingMailToReportingPerson", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion SendingMailToReportingPerson
    }
}