//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

using HRMS.Notification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Workflow.Runtime;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;
using V2.Orbit.Workflow.CompensationWF;

namespace HRMS.Orbitweb
{
    public partial class CompensationApplicationForm : System.Web.UI.Page
    {
        #region Variable declaration

        private CompensationDetailsModel objCompensationModel = new CompensationDetailsModel();
        private CompensationDetailsBOL objCompensationBOL = new CompensationDetailsBOL();
        private LeaveDetailsBOL objLeaveDeatilsBOL = new LeaveDetailsBOL();
        private LeaveDetailsModel objLeaveDeatilsModel = new LeaveDetailsModel();
        private HolidayMasterBOL objHolidayBOL = new HolidayMasterBOL();
        private HolidayMasterModel objHolidayModel = new HolidayMasterModel();
        private LeaveTransactionBOL objLeaveTransDetailsBOL = new LeaveTransactionBOL();
        private LeaveTransactionModel objLeaveTransDetailsModel = new LeaveTransactionModel();
        private LeaveDetailsModel objLeaveDetailsModel = new LeaveDetailsModel();

        private DataSet dsGetStatus, dsCompensation, dsReportingTo, dsHolidaysList, dsSearchDetails, dsSorting, dsConfigItem, dsCancelDetails;
        private string UserID = string.Empty;
        private DateTime FromDate, ConfigDate;
        private double TotalLeaves = 0, count = 0;
        private int j = 0, TotalLeavesApplyedFor = 0;
        private double CorrectionLeaves, BalanceLeaves;
        private String[] strLeaves;
        private String[] strHoliDays;
        private int rowsAffected, EmploymentStatus;

        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        #endregion Variable declaration

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string PageName = "Compensatory Leave Application";
                objpagelevel.PageLevelAccess(PageName);

                lblError.Text = "";
                lblSuccess.Text = "";
                UserID = User.Identity.Name.ToString();
                objCompensationModel.UserID = Convert.ToInt32(UserID.ToString());
                EmploymentStatus = objCompensationBOL.GetEmploymentStatus(objCompensationModel);

                if (!IsPostBack)
                {
                    if (EmploymentStatus > 0)
                    {
                        lblError.Text = "You are in Notice period, thus you are not eligible to apply for a Compensatory Leave";

                        tdAddLeave.Visible = false;
                        spanAddLeave.Visible = true;
                        spanSearch.Visible = false;
                        tdSearch.Visible = false;
                        lnkAddLeaves.Visible = false;
                        lnkSearch.Visible = false;
                        spanEdit.Visible = false;
                        travailableCompOff.Visible = false;
                        tddiff.Visible = false;

                        selected_tab.Value = "Add";

                        return;
                    }
                    else
                    {
                        FillddlStatus();
                        BindData();
                        tdAddLeave.Visible = true;
                        spanAddLeave.Visible = true;
                        spanSearch.Visible = false;
                        tdSearch.Visible = false;
                        lnkAddLeaves.Visible = false;
                        spanEdit.Visible = false;
                    }
                    ddlStatus.Visible = false;
                    txtSearchFromDate.Visible = false;
                    txtSearchToDate.Visible = false;
                }
                GridEditModel.Value = "";

                MainTab_Selected.Value = "CompOff";
                txtAppliedFor.Attributes.Add("onkeydown", "return false");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Page Load

        #region FillddlStatus

        public void FillddlStatus()
        {
            try
            {
                dsGetStatus = objLeaveDeatilsBOL.GetStatusDetails();
                ddlStatus.Items.Add(new ListItem("All", "0"));
                for (int i = 0; i < dsGetStatus.Tables[0].Rows.Count; i++)
                {
                    ddlStatus.Items.Add(new ListItem(dsGetStatus.Tables[0].Rows[i]["StatusName"].ToString(), dsGetStatus.Tables[0].Rows[i]["StatusID"].ToString()));
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "FillddlStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion FillddlStatus

        #region BindData

        public void BindData()
        {
            try
            {
                dsCompensation = new DataSet();
                objCompensationModel.UserID = Convert.ToInt32(UserID);
                objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);

                dsConfigItem = objCompensationBOL.GetCompensationDetails(objCompensationModel);
                ConfigDate = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                dsCompensation = objCompensationBOL.GetCompensationDetails(objCompensationModel);

                if (dsCompensation.Tables[0].Rows.Count > 0)
                {
                    gvCampensation.DataSource = dsCompensation.Tables[0];
                    gvCampensation.DataBind();
                }
                else if (dsCompensation.Tables[0].Rows.Count == 0)
                {
                    gvCampensation.DataSource = dsCompensation;
                    gvCampensation.DataBind();
                    lblError.Visible = true;
                    lblError.Text = "No Compensatory Leave details.";
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No Compensatory Leave details.";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "BindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion BindData

        #region AddCompenstionDetails

        public void AddCompenstionDetails()
        {
            try
            {
                SqlDataReader drCompenstionDetails = objCompensationBOL.AddCompenstionDetails(objCompensationModel);

                Guid gCompenstionWFID = new Guid("00000000-0000-0000-0000-000000000000");
                int CompenstionID = 0;

                while (drCompenstionDetails.Read())
                {
                    CompenstionID = Convert.ToInt32(drCompenstionDetails[0].ToString());
                    gCompenstionWFID = new Guid(drCompenstionDetails[1].ToString());
                    WorkflowRuntime wr = (WorkflowRuntime)Application["WokflowRuntime"];
                    wr = new WorkflowRuntime();
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("CompensationID", CompenstionID);
                    WorkflowInstance wi = wr.CreateWorkflow(typeof(Compensation), parameters, gCompenstionWFID);
                    wi.Start();
                }
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "AddCompenstionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion AddCompenstionDetails

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
                if (ex.Message.CompareTo("Already Compensation applied for this date.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "AddCompensationTransactionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion AddCompensationTransactionDetails

        #region UpdateCompenstionDetails

        public void UpdateCompenstionDetails()
        {
            try
            {
                rowsAffected = objCompensationBOL.UpdateCompenstionDetails(objCompensationModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "UpdateCompenstionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion UpdateCompenstionDetails

        #region UpdateCancelCompenstionDetails

        public void UpdateCancelCompenstionDetails()
        {
            try
            {
                rowsAffected = objCompensationBOL.UpdateCancelCompenstionDetails(objCompensationModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "UpdateCancelCompenstionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion UpdateCancelCompenstionDetails

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

        #region Submit

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //Code to Check the Designation level of the employee if it is above TL level
                //a message will be displayed to the user that the Application for the designation is not allowed.
                Boolean isEligible;
                int UserIdForCompOff = Convert.ToInt32(User.Identity.Name);
                string DesignationID = ConfigurationManager.AppSettings["DesignationID"].ToString();
                isEligible = objCompensationBOL.GetCompOffEligibility(UserIdForCompOff, DesignationID);
                if (!isEligible) //not Eligible
                {
                    lblError.Text = "You are not eligible to apply for Compensatory Leave";
                    lblError.Visible = true;
                    return;
                }

                //Code to Check whether the date of Comp-Off application is with in 60days of comp-off or not.
                DateTime dtCheckDate = DateTime.Now.AddDays(-60);
                DateTime dtAppliedFor = Convert.ToDateTime(txtAppliedFor.Text.Trim());

                objCompensationModel.UserID = Convert.ToInt32(UserID);
                objCompensationModel.AppliedFor = Convert.ToDateTime(txtAppliedFor.Text.Trim());
                objCompensationModel.Resason = txtReason.Text.ToString();
                objCompensationModel.StatusID = Convert.ToInt32(1);
                objCompensationModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                dsConfigItem = objCompensationBOL.GetCompensationDetails(objCompensationModel);
                ConfigDate = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                // Getting Reporting Person
                objLeaveDeatilsModel.UserID = Convert.ToInt32(UserID);
                dsReportingTo = objLeaveDeatilsBOL.GetReportingTo(objLeaveDeatilsModel);
                objCompensationModel.ApproverComments = "";

                if (dsReportingTo.Tables[0].Rows.Count > 0)
                {
                    objCompensationModel.ApproverID = Convert.ToInt32(dsReportingTo.Tables[0].Rows[0]["ReporterID"].ToString());
                }
                else
                {
                    // If Reporting to is not assigned
                    lblSuccess.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "Your Reporting To is not set. Please set it to the appropriate person";
                    return;
                }

                //Getting All the Holidays Details
                //   dsHolidaysList = objHolidayBOL.bindData();
                objHolidayModel.UserID = objCompensationModel.UserID;
                objHolidayModel.StartDate = objCompensationModel.AppliedFor;
                objHolidayModel.EndDate = objCompensationModel.AppliedFor;
                DataSet dsHolidaysList = objHolidayBOL.bindHolidaysForLeaveApprovals(objHolidayModel);

                FromDate = Convert.ToDateTime(txtAppliedFor.Text.Trim());
                Boolean isCompLeaveOk = false;

                objCompensationModel.CompensationTo = Convert.ToDateTime(txtAppliedFor.Text.Trim());

                rowsAffected = objCompensationBOL.CheckSignInForCompensation(objCompensationModel);
                if (rowsAffected > 0)
                {
                    // This is checking the total list of holiday in current year provided by V2.
                    for (int k = 0; k < dsHolidaysList.Tables[0].Rows.Count; k++)
                    {
                        if (FromDate.ToString() == dsHolidaysList.Tables[0].Rows[k]["HolidayDate"].ToString())
                        {
                            isCompLeaveOk = true;
                            break;
                        }
                    }

                    // This is to check whether the working day is a WeekDay.
                    if (FromDate.DayOfWeek.ToString() == "Saturday" || FromDate.DayOfWeek.ToString() == "Sunday")
                    {
                        isCompLeaveOk = true;
                    }

                    if (isCompLeaveOk)
                    {
                        // Compensatory Leave Submitted Successfully.
                        objCompensationModel.AppliedFor = Convert.ToDateTime(FromDate.ToString());
                        AddCompenstionDetails();
                        lblSuccess.Text = "Compensatory Leave successfully submitted";
                    }
                    else
                    {
                        // Compensatory Leave Submitted Unsuccessfully.
                        lblError.Visible = true;
                        lblError.Text = "Compensatory leave cannot be applied for a working day";
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "You have not signed-in on the day you are applying compensatory off for, Select Valid date";
                }

                BindData();
                txtAppliedFor.Text = "";
                txtReason.Text = "";
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "btnSubmit", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    lblError.Visible = true;

                    lblError.Text = "You have already applied for compensatory leave for this date";
                }
            }
        }

        #endregion Submit

        #region Reset

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtAppliedFor.Text = "";
                txtReason.Text = "";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "btnReset_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Reset

        #region gvCampensation_RowEditing

        protected void gvCampensation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                tdAddLeave.Visible = false;
                tdSearch.Visible = false;
                lnkAddLeaves.Visible = true;
                lnkSearch.Visible = true;
                spanAddLeave.Visible = false;
                spanEdit.Visible = true;
                spanSearch.Visible = false;
                tddiff.Visible = true;
                tdSearchLink.Visible = true;
                gvCampensation.EditIndex = e.NewEditIndex;

                LinkButton lbnCancel = ((LinkButton)(gvCampensation.Rows[e.NewEditIndex].FindControl("lnkButCancel")));
                lbnCancel.Visible = false;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "gvCampensation_RowEditing", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion gvCampensation_RowEditing

        #region gvCampensation_RowUpdating

        protected void gvCampensation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                txtAppliedFor.Text = "";
                txtReason.Text = "";

                GridViewRow row = gvCampensation.Rows[e.RowIndex];

                objCompensationModel.UserID = Convert.ToInt32(UserID);

                Label lblEditCompensationID = row.FindControl("lblEditCompensationID") as Label;
                objCompensationModel.CompensationID = Convert.ToInt32(lblEditCompensationID.Text);

                TextBox txtEditAppliedFor = row.FindControl("txtEditAppliedFor") as TextBox;

                Label lblgrvStatusID = row.FindControl("lblgrvStatusID") as Label;
                objCompensationModel.StatusID = Convert.ToInt32(lblgrvStatusID.Text.Trim());

                TextBox txtgrvLeaveReason = row.FindControl("txtgrvLeaveReason") as TextBox;
                objCompensationModel.Resason = txtgrvLeaveReason.Text.Trim();

                objCompensationModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                //Getting All the Holidays Details
                //  dsHolidaysList = objHolidayBOL.bindData();
                objHolidayModel.UserID = objCompensationModel.UserID;
                objHolidayModel.StartDate = objLeaveDetailsModel.LeaveDateFrom;
                objHolidayModel.EndDate = objCompensationModel.AppliedFor;
                dsHolidaysList = objHolidayBOL.bindHolidaysForLeaveApprovals(objHolidayModel);

                FromDate = Convert.ToDateTime(txtEditAppliedFor.Text.Trim());
                Boolean flag = false;
                objCompensationModel.CompensationTo = Convert.ToDateTime(txtEditAppliedFor.Text.Trim());
                rowsAffected = objCompensationBOL.CheckSignInForCompensation(objCompensationModel);
                if (rowsAffected > 0)
                {
                    for (int k = 0; k < dsHolidaysList.Tables[0].Rows.Count; k++)
                    {
                        if (FromDate.ToString() == dsHolidaysList.Tables[0].Rows[k]["HolidayDate"].ToString())
                        {
                            flag = true;
                            break;
                        }
                        if (FromDate.DayOfWeek.ToString() == "Saturday" || FromDate.DayOfWeek.ToString() == "Sunday")
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (flag)
                    {
                        if (ConfigDate.Date >= Convert.ToDateTime(txtEditAppliedFor.Text).Date)
                        {
                            lblError.Text = "Administrator has frozen the data for the selected date";
                            txtEditAppliedFor.Text = "";
                            gvCampensation.EditIndex = -1;
                            return;
                        }
                        else
                        {
                            objCompensationModel.AppliedFor = Convert.ToDateTime(FromDate.ToString());
                            UpdateCompenstionDetails();
                            gvCampensation.EditIndex = -1;
                            lblError.Visible = true;
                            lblSuccess.Visible = true;
                            lblSuccess.Text = "Compensatory Leave Updated Successfully";
                        }
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Compensatory leave cannot be applied for a working day";
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "You have not signed-in on the day you are applying compensatory off for, Select Valid date";
                }

                BindData();
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "gvLeaveApplication_RowUpdating", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "You have already applied for compensatory leave for this date";
                }
            }
        }

        #endregion gvCampensation_RowUpdating

        #region gvCampensation_RowCommand

        protected void gvCampensation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                txtAppliedFor.Text = "";
                txtReason.Text = "";

                if (e.CommandName == "CampensationCancel")
                {
                    gvCampensation.EditIndex = -1;

                    Label lblStatusName = new Label();
                    for (int i = 0; i < gvCampensation.Rows.Count; i++)
                    {
                        Label lblId = (Label)((gvCampensation.Rows[i].FindControl("lblCompensationID")));
                        if (lblId.Text == e.CommandArgument.ToString())
                        {
                            lblStatusName = (Label)((gvCampensation.Rows[i].FindControl("lblgrvStatusName")));
                            break;
                        }
                    }

                    objCompensationModel.CompensationID = Convert.ToInt32(e.CommandArgument);
                    objCompensationModel.UserID = Convert.ToInt32(UserID);

                    if (lblStatusName.Text == "Approved")
                    {
                        //Here delete the all records of related CompensationID
                        objLeaveTransDetailsModel.UserID = Convert.ToInt32(UserID);
                        objLeaveTransDetailsModel.CompensationID = Convert.ToInt32(e.CommandArgument);
                        DeleteCompensationTransactionDetails();
                        UpdateEmployeeLeaveAndComp();
                        //Sending a Mail to Reporting person
                        objLeaveDeatilsModel.UserID = Convert.ToInt32(UserID);
                        SendingMailToReportingPerson();
                    }

                    objCompensationModel.StatusID = Convert.ToInt32(4);
                    objCompensationModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    UpdateCancelCompenstionDetails();
                    BindData();

                    lblError.Visible = false;
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Compensatory Leave Cancelled successfully";
                }

                if (e.CommandName == "lnkCancel")
                {
                    gvCampensation.EditIndex = -1;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "gvCampensation_RowCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion gvCampensation_RowCommand

        #region gvCampensation_RowDataBound

        protected void gvCampensation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblstatus = ((Label)(e.Row.FindControl("lblgrvStatusName")));
                    Label lblApproved = ((Label)(e.Row.FindControl("lblApprove")));
                    Label lblAppliedFor = ((Label)(e.Row.FindControl("lblAppliedFor")));

                    objCompensationModel.UserID = Convert.ToInt32(UserID);
                    objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    dsConfigItem = objCompensationBOL.GetCompensationDetails(objCompensationModel);
                    ConfigDate = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());
                    LinkButton lnkCancel = ((LinkButton)(e.Row.FindControl("lnkButCancel")));
                    LinkButton lnkEdit = ((LinkButton)(e.Row.FindControl("lnkbutEdit")));

                    if (ConfigDate.Date >= Convert.ToDateTime(lblAppliedFor.Text).Date)
                    {
                        if (lblstatus.Text == "Approved")
                        {
                            lblApproved.Text = "CompOff Aprroved";

                            lnkEdit.Visible = false;
                            lnkCancel.Visible = false;
                        }
                        else if (lblstatus.Text == "Cancelled")
                        {
                            lblApproved.Visible = true;
                            lblApproved.Text = "CompOff Cancelled";
                            lnkEdit.Visible = false;
                            lnkCancel.Visible = false;
                        }
                        else if (lblstatus.Text == "Rejected")
                        {
                            lblApproved.Visible = true;
                            lblApproved.Text = "CompOff Rejected";
                            lnkEdit.Visible = false;
                            lnkCancel.Visible = false;
                        }
                        else if (lblstatus.Text == "Pending")
                        {
                            lblApproved.Visible = true;
                            lblApproved.Text = "CompOff Pending";

                            lnkEdit.Visible = false;
                            lnkCancel.Visible = false;
                        }
                    }
                    else
                    {
                        if (lblstatus.Text == "Approved")
                        {
                            lnkEdit.Visible = false;
                        }
                        else if (lblstatus.Text == "Cancelled")
                        {
                            lblApproved.Visible = true;
                            lblApproved.Text = "CompOff Cancelled";

                            lnkEdit.Visible = false;
                            lnkCancel.Visible = false;
                        }
                        else if (lblstatus.Text == "Rejected")
                        {
                            lblApproved.Visible = true;
                            lblApproved.Text = "CompOff Rejected";

                            lnkEdit.Visible = false;
                            lnkCancel.Visible = false;
                        }
                    }
                }

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    TextBox txtEditAppliedFor = (TextBox)e.Row.FindControl("txtEditAppliedFor");
                    TextBox txtgrvFormDate = (TextBox)e.Row.FindControl("txtgrvFormDate");

                    txtEditAppliedFor.Attributes.Add("onkeydown", "return false");

                    GridEditModel.Value = "Edit";

                    if (selected_tab.Value == "Search")
                    {
                    }
                    else if (selected_tab.Value == "Add" || selected_tab.Value == "")
                    {
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "gvCampensation_RowDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion gvCampensation_RowDataBound

        #region gvCampensation_RowCancelingEdit

        protected void gvCampensation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvCampensation.EditIndex = -1;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "gvCampensation_RowCancelingEdit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion gvCampensation_RowCancelingEdit

        #region AddLeaves Link

        protected void lnkAddLeaves_Click(object sender, EventArgs e)
        {
            try
            {
                lnkAddLeaves.Visible = false;
                lnkSearch.Visible = true;
                tdSearch.Visible = false;
                tdAddLeave.Visible = true;
                gvCampensation.Visible = true;
                lblSuccess.Text = "";
                lblError.Text = "";
                spanAddLeave.Visible = true;
                spanSearch.Visible = false;
                spanEdit.Visible = false;
                tddiff.Visible = false;
                tdSearchLink.Visible = true;
                gvCampensation.EditIndex = -1;
                ddlStatus.SelectedValue = Convert.ToString("0");

                AddCompOffDetails.Visible = true;

                ddlStatus.Visible = false;
                txtSearchFromDate.Visible = false;
                txtSearchToDate.Visible = false;

                txtAppliedFor.Visible = true;
                txtReason.Visible = true;

                selected_tab.Value = "Add";

                BindData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "lnkAddLeaves_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion AddLeaves Link

        #region Search Link

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
                lnkAddLeaves.Visible = true;
                lnkSearch.Visible = false;
                tdSearch.Visible = true;
                tdSearchLink.Visible = false;
                tdAddLeave.Visible = false;
                gvCampensation.Visible = true;
                lblSuccess.Text = "";
                lblError.Text = "";
                spanSearch.Visible = true;
                spanAddLeave.Visible = false;
                spanEdit.Visible = false;
                gvCampensation.EditIndex = -1;
                tddiff.Visible = false;
                BindData();
                ddlStatus.SelectedValue = Convert.ToString("0");
                txtAppliedFor.Text = "";
                txtReason.Text = "";

                SearchCompOff.Visible = true;

                txtAppliedFor.Visible = false;
                txtReason.Visible = false;

                ddlStatus.Visible = true;
                txtSearchFromDate.Visible = true;
                txtSearchToDate.Visible = true;

                selected_tab.Value = "Search";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "lnkSearch_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Search Link

        #region Cancel

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                objCompensationModel.UserID = Convert.ToInt32(UserID);
                tdSearch.Visible = true;
                tdAddLeave.Visible = false;
                ddlStatus.SelectedValue = Convert.ToString("0");
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
                lblSuccess.Text = "";
                lblError.Text = "";
                lnkAddLeaves.Visible = true;

                if (ddlStatus.SelectedValue == "0")
                {
                    objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    objCompensationModel.UserID = Convert.ToInt32(UserID);
                    dsSearchDetails = objCompensationBOL.SearchAllCompensationDetails(objCompensationModel);
                    dsConfigItem = objCompensationBOL.SearchAllCompensationDetails(objCompensationModel);
                    gvCampensation.Visible = true;
                    gvCampensation.DataSource = dsSearchDetails.Tables[0];
                    gvCampensation.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Cancel

        #region SearchCompensationDetails

        public void SearchCompensationDetails()
        {
            try
            {
                dsSearchDetails = objCompensationBOL.SearchCompensationDetails(objCompensationModel);
                if (dsSearchDetails.Tables[0].Rows.Count <= 0)
                {
                    lblError.Text = "Records Not Found";
                    gvCampensation.Visible = false;
                }
                else
                {
                    lblError.Text = "";
                    gvCampensation.Visible = true;
                    gvCampensation.DataSource = dsSearchDetails.Tables[0];
                    gvCampensation.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "SearchCompensationDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion SearchCompensationDetails

        #region ddlStatus_SelectedIndexChanged

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
                objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                objCompensationModel.UserID = Convert.ToInt32(UserID);

                dsSearchDetails = objCompensationBOL.SearchAllCompensationDetails(objCompensationModel);
                dsConfigItem = objCompensationBOL.SearchAllCompensationDetails(objCompensationModel);
                if (dsSearchDetails.Tables[0].Rows.Count <= 0)
                {
                    lblError.Text = "Records Not Found";
                    gvCampensation.Visible = false;
                }
                else
                {
                    lblError.Text = "";
                    gvCampensation.Visible = true;
                    gvCampensation.DataSource = dsSearchDetails.Tables[0];
                    gvCampensation.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "ddlStatus_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion ddlStatus_SelectedIndexChanged

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
                tdSearch.Visible = true;
                tdAddLeave.Visible = false;
                spanSearch.Visible = true;

                spanAddLeave.Visible = false;
                lnkAddLeaves.Visible = true;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "btnSearch_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Search

        #region Sorting

        protected void gvCampensation_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                objCompensationModel.UserID = Convert.ToInt32(UserID);
                objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);

                if (txtSearchFromDate.Text != "" && txtSearchToDate.Text != "")
                {
                    objCompensationModel.UserID = Convert.ToInt32(UserID);
                    objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    objCompensationModel.CompensationFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objCompensationModel.CompensationTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    dsSorting = objCompensationBOL.SearchCompensationDetails(objCompensationModel);
                }
                else
                {
                    dsSorting = objCompensationBOL.GetCompensationDetails(objCompensationModel);
                    if (dsSorting.Tables[0].Rows.Count <= 0)
                    {
                        lblError.Text = "Records Not Found";
                        gvCampensation.Visible = false;
                    }
                    else
                    {
                        lblError.Text = "";
                        gvCampensation.Visible = true;
                        gvCampensation.DataSource = dsSorting;
                        gvCampensation.DataBind();
                    }
                }
                gvCampensation.DataSource = dsSorting.Tables[0];
                gvCampensation.DataBind();
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

                gvCampensation.DataSource = dv;
                gvCampensation.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "gvCampensation_Sorting", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Sorting

        #region Paging

        protected void gvCampensation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCampensation.PageIndex = e.NewPageIndex;
                gvCampensation.EditIndex = -1;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "gvCampensation_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Paging

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
                    SmtpClient smtpClient = new SmtpClient();
                    objMailMessage.To.Add(new MailAddress(dsReportingTo.Tables[0].Rows[0]["ReporterMailID"].ToString()));

                    string UserName, ApproverName, Reason, AppliedFor;

                    dsCancelDetails = objCompensationBOL.GetCancelCompOffDetails(objCompensationModel);

                    UserName = dsReportingTo.Tables[0].Rows[0]["EmployeeName"].ToString();
                    ApproverName = dsReportingTo.Tables[0].Rows[0]["ReporterName"].ToString();
                    objMailMessage.To.Add(new MailAddress(dsReportingTo.Tables[0].Rows[0]["ReporterMailID"].ToString()));
                    objMailMessage.CC.Add(new MailAddress(dsReportingTo.Tables[0].Rows[0]["CompetencyMailID"].ToString()));
                    AppliedFor = Convert.ToDateTime(dsCancelDetails.Tables[0].Rows[0]["AppliedFor"].ToString()).ToShortDateString();

                    Reason = dsCancelDetails.Tables[0].Rows[0]["Reason"].ToString();

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

                    objMailMessage.Subject = "Cancelling Compensatory Leave";

                    objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi " + ApproverName + " ," + "<br>" + "<br>" + " Compensatory Leave Application Details: " + "<br>" + "<br>" + "User Name: " + "<b>" + UserName + "</b>" + " <br> " + "AppliedFor: " + AppliedFor + " <br> " + " Reason: " + Reason + " <br> " + " <br> " + " Cancelled the Approved Compensatory Leave Details, the required updates are made in the system.";

                    SMTPHelper objhelper = new SMTPHelper();
                    objhelper.SendMail(objMailMessage.From.ToString(), objMailMessage.To.ToString(), objMailMessage.Subject, objMailMessage.Body, objMailMessage.CC.ToString(), null, 1);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "CompensationApplicationForm.aspx.cs", "SendingMailToReportingPerson", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion SendingMailToReportingPerson

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
    }
}