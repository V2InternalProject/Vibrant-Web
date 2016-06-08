using HRMS.Notification;
using System;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;

namespace HRMS.Orbitweb
{
    public partial class AdminApproval : System.Web.UI.Page
    {
        private SignInSignOutModel objSignInSignOutModel = new SignInSignOutModel();
        private SignInSignOutBOL objSignInSignOutBOL = new SignInSignOutBOL();
        private LeaveDetailsModel objLeaveDetailsModel = new LeaveDetailsModel();
        private LeaveDetailsBOL objLeaveDetailsBOL = new LeaveDetailsBOL();
        private CompensationDetailsModel objCompensationDetailsModel = new CompensationDetailsModel();
        private CompensationDetailsBOL objCompensationDetailsBOL = new CompensationDetailsBOL();
        private LeaveTransactionBOL objLeaveTransactionBOL = new LeaveTransactionBOL();
        private LeaveTransactionModel objLeaveTransDetailsModel = new LeaveTransactionModel();
        private HolidayMasterBOL objHolidayMasterBOL = new HolidayMasterBOL();
        private ConfigItemBOL objConfigItemBOL = new ConfigItemBOL();
        private HolidayMasterModel objHolidayModel = new HolidayMasterModel();
        private HolidayMasterBOL objHolidayBOL = new HolidayMasterBOL();
        private DataSet dsAdminApproval = new DataSet();
        private string strSignInDateForMail, strApproverCommentForMail, strFromDate, strToDate;
        private string strLeaveToDate;
        private Guid WorkflowId;
        private int intUserIDForMail;
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string PageName = "Admin Approval";
                objpagelevel.PageLevelAccess(PageName);

                if (!IsPostBack)
                {
                    txtSearchFromDate.Attributes.Add("onkeydown", "return false");
                    txtSearchToDate.Attributes.Add("onkeydown", "return false");

                    FillEmployeeName();
                }
            }
            catch (V2Exceptions ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void ddlModuleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSISO.PageIndex = 0;
            gvLeave.PageIndex = 0;
            gvCompensatory.PageIndex = 0;
            gvOutOfOffice.PageIndex = 0;

            try
            {
                if (ddlModuleType.SelectedValue == "10")
                {
                    #region Show Hide Panel for selected Module Type

                    lblError.Text = "Please select the Module type";
                    lblSuccess.Text = "";

                    pnlSearch.Visible = false;
                    pnlSISO.Visible = false;
                    pnlLeave.Visible = false;
                    pnlCompensdatory.Visible = false;
                    pnlOutOfOffice.Visible = false;

                    txtSearchFromDate.Text = "";
                    txtSearchToDate.Text = "";
                    lblSelectUser.Visible = false;
                    ddlEmployeeName.Visible = false;

                    #endregion Show Hide Panel for selected Module Type
                }
                else if (ddlModuleType.SelectedValue == "0")
                {
                    //Code to show the SISO pending records

                    #region Show Hide Panel for Selected Module type

                    lblError.Text = "";
                    lblSuccess.Text = "";
                    pnlSearch.Visible = true;
                    pnlSISO.Visible = true;
                    pnlLeave.Visible = false;
                    pnlCompensdatory.Visible = false;
                    pnlOutOfOffice.Visible = false;

                    txtSearchFromDate.Text = "";
                    txtSearchToDate.Text = "";
                    lblSelectUser.Visible = false;
                    ddlEmployeeName.Visible = false;

                    #endregion Show Hide Panel for Selected Module type

                    #region Fill Status DropDown

                    DataSet dsGetStatus = new DataSet();
                    dsGetStatus = objSignInSignOutBOL.GetStatus();
                    ddlStatus.Items.Clear();
                    for (int i = 0; i < dsGetStatus.Tables[0].Rows.Count; i++)
                    {
                        if (dsGetStatus.Tables[0].Rows[i]["StatusID"].ToString() != "4")
                            ddlStatus.Items.Add(new ListItem(dsGetStatus.Tables[0].Rows[i]["StatusName"].ToString(), dsGetStatus.Tables[0].Rows[i]["StatusID"].ToString()));
                    }
                    ddlStatus.Items.Add(new ListItem("Auto-Approved", "5"));
                    ddlStatus.Items.Add(new ListItem("All", "0"));

                    #endregion Fill Status DropDown

                    BindSISOData();
                }
                else if (ddlModuleType.SelectedValue == "1")
                {
                    //Code to show the Leave pending records

                    #region Show Hide Panel for Selected Module type

                    lblError.Text = "";
                    lblSuccess.Text = "";
                    pnlSearch.Visible = true;
                    pnlSISO.Visible = false;
                    pnlLeave.Visible = true;
                    pnlCompensdatory.Visible = false;
                    pnlOutOfOffice.Visible = false;

                    txtSearchFromDate.Text = "";
                    txtSearchToDate.Text = "";
                    lblSelectUser.Visible = false;
                    ddlEmployeeName.Visible = false;

                    #endregion Show Hide Panel for Selected Module type

                    #region Fill Status DropDown

                    DataSet dsGetStatus = new DataSet();
                    dsGetStatus = objSignInSignOutBOL.GetStatus();
                    ddlStatus.Items.Clear();
                    for (int i = 0; i < dsGetStatus.Tables[0].Rows.Count; i++)
                    {
                        ddlStatus.Items.Add(new ListItem(dsGetStatus.Tables[0].Rows[i]["StatusName"].ToString(), dsGetStatus.Tables[0].Rows[i]["StatusID"].ToString()));
                    }

                    ddlStatus.Items.Add(new ListItem("All", "0"));

                    #endregion Fill Status DropDown

                    BindLeaveData();
                }
                else if (ddlModuleType.SelectedValue == "2")
                {
                    //Code to show the Compensatory Leave pending records

                    #region Show Hide Panel for Selected Module type

                    lblError.Text = "";
                    lblSuccess.Text = "";
                    pnlSearch.Visible = true;
                    pnlSISO.Visible = false;
                    pnlLeave.Visible = false;
                    pnlCompensdatory.Visible = true;
                    pnlOutOfOffice.Visible = false;

                    txtSearchFromDate.Text = "";
                    txtSearchToDate.Text = "";
                    lblSelectUser.Visible = false;
                    ddlEmployeeName.Visible = false;

                    #endregion Show Hide Panel for Selected Module type

                    #region Fill Status DropDown

                    DataSet dsGetStatus = new DataSet();
                    dsGetStatus = objSignInSignOutBOL.GetStatus();
                    ddlStatus.Items.Clear();
                    for (int i = 0; i < dsGetStatus.Tables[0].Rows.Count; i++)
                    {
                        ddlStatus.Items.Add(new ListItem(dsGetStatus.Tables[0].Rows[i]["StatusName"].ToString(), dsGetStatus.Tables[0].Rows[i]["StatusID"].ToString()));
                    }

                    ddlStatus.Items.Add(new ListItem("All", "0"));

                    #endregion Fill Status DropDown

                    BindCompensatoryData();
                }
                else if (ddlModuleType.SelectedValue == "3")
                {
                    //Code to show the Out Of Office pending records

                    #region Show Hide Panel for Selected Module type

                    lblError.Text = "";
                    lblSuccess.Text = "";
                    pnlSearch.Visible = true;
                    pnlSISO.Visible = false;
                    pnlLeave.Visible = false;
                    pnlCompensdatory.Visible = false;
                    pnlOutOfOffice.Visible = true;

                    txtSearchFromDate.Text = "";
                    txtSearchToDate.Text = "";
                    lblSelectUser.Visible = false;
                    ddlEmployeeName.Visible = false;

                    #endregion Show Hide Panel for Selected Module type

                    #region Fill Status DropDown

                    DataSet dsGetStatus = new DataSet();
                    dsGetStatus = objSignInSignOutBOL.GetStatus();
                    ddlStatus.Items.Clear();
                    for (int i = 0; i < dsGetStatus.Tables[0].Rows.Count; i++)
                    {
                        ddlStatus.Items.Add(new ListItem(dsGetStatus.Tables[0].Rows[i]["StatusName"].ToString(), dsGetStatus.Tables[0].Rows[i]["StatusID"].ToString()));
                    }
                    ddlStatus.Items.Add(new ListItem("All", "0"));

                    #endregion Fill Status DropDown

                    BindOutOfOfficeData();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "ddlModuleType_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSISO.PageIndex = 0;
            gvLeave.PageIndex = 0;
            gvCompensatory.PageIndex = 0;
            gvOutOfOffice.PageIndex = 0;

            try
            {
                if (ddlStatus.SelectedItem.Text == "Pending")
                {
                    if (ddlModuleType.SelectedValue == "0")// SISO Pending
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = true;
                        pnlLeave.Visible = false;
                        pnlLeave.Visible = false;
                        pnlLeave.Visible = false;
                        imgbtnSearchToDate.Enabled = true;
                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindSISOData();
                    }
                    else if (ddlModuleType.SelectedValue == "1")//Leave Pending
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = false;
                        pnlLeave.Visible = true;
                        pnlCompensdatory.Visible = false;
                        pnlOutOfOffice.Visible = false;

                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindLeaveData();
                    }
                    else if (ddlModuleType.SelectedValue == "2")//Compensatory Pending
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = false;
                        pnlLeave.Visible = false;
                        pnlCompensdatory.Visible = true;
                        pnlOutOfOffice.Visible = false;

                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindCompensatoryData();
                    }
                    else if (ddlModuleType.SelectedValue == "3")//Out Of Office Pending
                    { }
                }
                else if (ddlStatus.SelectedItem.Text == "Approved")
                {
                    if (ddlModuleType.SelectedValue == "0")// SISO Approved
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = true;
                        pnlLeave.Visible = false;
                        pnlLeave.Visible = false;
                        pnlLeave.Visible = false;
                        imgbtnSearchToDate.Enabled = true;
                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindSISOData();
                    }
                    else if (ddlModuleType.SelectedValue == "1")//Leave Approved
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = false;
                        pnlLeave.Visible = true;
                        pnlCompensdatory.Visible = false;
                        pnlOutOfOffice.Visible = false;

                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindLeaveData();
                    }
                    else if (ddlModuleType.SelectedValue == "2")//Compensatory Approved
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = false;
                        pnlLeave.Visible = false;
                        pnlCompensdatory.Visible = true;
                        pnlOutOfOffice.Visible = false;

                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindCompensatoryData();
                    }
                    else if (ddlModuleType.SelectedValue == "3")//Out Of Office Approved
                    { }
                }
                else if (ddlStatus.SelectedItem.Text == "Rejected")
                {
                    if (ddlModuleType.SelectedValue == "0")// SISO Rejected
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = true;
                        pnlLeave.Visible = false;
                        pnlLeave.Visible = false;
                        pnlLeave.Visible = false;
                        imgbtnSearchToDate.Enabled = true;
                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindSISOData();
                    }
                    else if (ddlModuleType.SelectedValue == "1")//Leave Rejected
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = false;
                        pnlLeave.Visible = true;
                        pnlCompensdatory.Visible = false;
                        pnlOutOfOffice.Visible = false;

                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindLeaveData();
                    }
                    else if (ddlModuleType.SelectedValue == "2")//Compensatory Rejected
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = false;
                        pnlLeave.Visible = false;
                        pnlCompensdatory.Visible = true;
                        pnlOutOfOffice.Visible = false;

                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindCompensatoryData();
                    }
                    else if (ddlModuleType.SelectedValue == "3")//Out Of Office Rejected
                    { }
                }
                else if (ddlStatus.SelectedItem.Text == "Cancelled")
                {
                    if (ddlModuleType.SelectedValue == "1")//Leave Cancelled Cancelled
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = false;
                        pnlLeave.Visible = true;
                        pnlCompensdatory.Visible = false;
                        pnlOutOfOffice.Visible = false;

                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindLeaveData();
                    }
                    else if (ddlModuleType.SelectedValue == "2")//Compensatory Cancelled
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = false;
                        pnlLeave.Visible = false;
                        pnlCompensdatory.Visible = true;
                        pnlOutOfOffice.Visible = false;

                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindCompensatoryData();
                    }
                    else if (ddlModuleType.SelectedValue == "3")//Out Of Office Cancelled
                    { }
                }
                else if (ddlStatus.SelectedItem.Text == "Auto-Approved")
                {
                    if (ddlModuleType.SelectedValue == "0")// SISO Auto-Approved
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = true;
                        pnlLeave.Visible = false;
                        pnlLeave.Visible = false;
                        pnlLeave.Visible = false;
                        txtSearchFromDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                        txtSearchToDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                        lblSelectUser.Visible = true;
                        ddlEmployeeName.Visible = true;

                        #endregion Show Hide Panel for Selected Module type

                        BindSISOData();
                    }
                }
                else if (ddlStatus.SelectedItem.Text == "All")
                {
                    if (ddlModuleType.SelectedValue == "0")// SISO All
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = true;
                        pnlLeave.Visible = false;
                        pnlLeave.Visible = false;
                        pnlLeave.Visible = false;
                        imgbtnSearchToDate.Enabled = true;
                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindSISOData();
                    }
                    else if (ddlModuleType.SelectedValue == "1")//Leave All
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = false;
                        pnlLeave.Visible = true;
                        pnlCompensdatory.Visible = false;
                        pnlOutOfOffice.Visible = false;

                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindLeaveData();
                    }
                    else if (ddlModuleType.SelectedValue == "2")//Compensatory All
                    {
                        #region Show Hide Panel for Selected Module type

                        lblError.Text = "";
                        lblSuccess.Text = "";
                        pnlSearch.Visible = true;
                        pnlSISO.Visible = false;
                        pnlLeave.Visible = false;
                        pnlCompensdatory.Visible = true;
                        pnlOutOfOffice.Visible = false;

                        txtSearchFromDate.Text = "";
                        txtSearchToDate.Text = "";
                        lblSelectUser.Visible = false;
                        ddlEmployeeName.Visible = false;

                        #endregion Show Hide Panel for Selected Module type

                        BindCompensatoryData();
                    }
                    else if (ddlModuleType.SelectedValue == "3")//Out Of Office All
                    { }
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "ddlStatus_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlModuleType.SelectedValue == "10")
                {
                    #region Show Hide Panel for selected Module Type

                    lblError.Text = "Please select the Module type";
                    lblSuccess.Text = "";

                    pnlSearch.Visible = false;
                    pnlSISO.Visible = false;
                    pnlLeave.Visible = false;
                    pnlCompensdatory.Visible = false;
                    pnlOutOfOffice.Visible = false;

                    #endregion Show Hide Panel for selected Module Type
                }
                else if (ddlModuleType.SelectedValue == "0")
                {
                    //Code to show the SISO pending records

                    #region Show Hide Panel for Selected Module type

                    lblError.Text = "";
                    lblSuccess.Text = "";
                    pnlSearch.Visible = true;
                    pnlSISO.Visible = true;
                    pnlLeave.Visible = false;
                    pnlCompensdatory.Visible = false;
                    pnlOutOfOffice.Visible = false;

                    #endregion Show Hide Panel for Selected Module type

                    BindSISOData();
                }
                else if (ddlModuleType.SelectedValue == "1")
                {
                    //Code to show the Leave pending records

                    #region Show Hide Panel for Selected Module type

                    lblError.Text = "";
                    lblSuccess.Text = "";
                    pnlSearch.Visible = true;
                    pnlSISO.Visible = false;
                    pnlLeave.Visible = true;
                    pnlCompensdatory.Visible = false;
                    pnlOutOfOffice.Visible = false;

                    #endregion Show Hide Panel for Selected Module type

                    BindLeaveData();
                }
                else if (ddlModuleType.SelectedValue == "2")
                {
                    //Code to show the Compensatory Leave pending records

                    #region Show Hide Panel for Selected Module type

                    lblError.Text = "";
                    lblSuccess.Text = "";
                    pnlSearch.Visible = true;
                    pnlSISO.Visible = false;
                    pnlLeave.Visible = false;
                    pnlCompensdatory.Visible = true;
                    pnlOutOfOffice.Visible = false;

                    #endregion Show Hide Panel for Selected Module type

                    BindCompensatoryData();
                }
                else if (ddlModuleType.SelectedValue == "3")
                {
                    //Code to show the Out Of Office pending records

                    #region Show Hide Panel for Selected Module type

                    lblError.Text = "";
                    lblSuccess.Text = "";
                    pnlSearch.Visible = true;
                    pnlSISO.Visible = false;
                    pnlLeave.Visible = false;
                    pnlCompensdatory.Visible = false;
                    pnlOutOfOffice.Visible = true;

                    #endregion Show Hide Panel for Selected Module type

                    BindOutOfOfficeData();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "btnSearch_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #region SISO Module

        public void BindSISOData()
        {
            try
            {
                int StatusID = Convert.ToInt32(ddlStatus.SelectedValue);//Default pending is selected
                int UserID = 0;
                string FromDate = string.Empty;
                DateTime strFromDate = new DateTime();

                if (txtSearchFromDate.Text != "")
                    strFromDate = Convert.ToDateTime(txtSearchFromDate.Text);

                if (txtSearchFromDate.Text == "")
                    FromDate = "Empty";
                else
                    FromDate = strFromDate.ToString("dd/MMMM/yyyy");

                string ToDate = string.Empty;
                DateTime strToDate = new DateTime();

                if (txtSearchToDate.Text != "")
                    strToDate = Convert.ToDateTime(txtSearchToDate.Text);

                if (txtSearchToDate.Text == "")
                    ToDate = "Empty";
                else
                    ToDate = strToDate.AddDays(1).ToString("dd/MMMM/yyyy");

                if (ddlStatus.SelectedItem.Text == "Auto-Approved")
                    UserID = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);

                dsAdminApproval = objSignInSignOutBOL.GetSISOForAdminApproval(StatusID, FromDate, ToDate, UserID);
                if (dsAdminApproval.Tables[0].Rows.Count == 0)
                {
                    lblError.Text = "No records found";
                    gvSISO.DataSource = null;

                    if (ddlStatus.SelectedValue.ToString() == "1")
                        gvSISO.Columns[16].Visible = true;
                    else
                        gvSISO.Columns[16].Visible = false;

                    gvSISO.DataBind();
                    gvLeave.DataSource = null;
                    gvLeave.DataBind();
                    gvCompensatory.DataSource = null;
                    gvCompensatory.DataBind();
                    gvOutOfOffice.DataSource = null;
                    gvOutOfOffice.DataBind();
                }
                else
                {
                    lblError.Text = "";

                    gvLeave.DataSource = null;
                    gvLeave.DataBind();
                    gvCompensatory.DataSource = null;
                    gvCompensatory.DataBind();
                    gvOutOfOffice.DataSource = null;
                    gvOutOfOffice.DataBind();

                    gvSISO.Visible = true;
                    gvSISO.DataSource = dsAdminApproval.Tables[0];
                    if (ddlStatus.SelectedValue.ToString() == "1")
                        gvSISO.Columns[16].Visible = true;
                    else
                        gvSISO.Columns[16].Visible = false;

                    gvSISO.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "BindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvSISO_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSISO.PageIndex = e.NewPageIndex;
                gvSISO.EditIndex = -1;
                BindSISOData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "gvSISO_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvSISO_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblSuccess.Text = "";
                gvSISO.EditIndex = e.NewEditIndex;
                BindSISOData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "gvSISO_RowEditing", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvSISO_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                lblSuccess.Text = "";
                lblError.Text = "";

                gvSISO.EditIndex = -1;
                BindSISOData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "gvSISO_RowCancelingEdit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvSISO_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Label lblDate = (Label)gvSISO.Rows[e.RowIndex].FindControl("lblDate");
                string strDate = lblDate.Text.ToString();
                //for workflow...
                Label lblWorkflowID = ((Label)gvSISO.Rows[e.RowIndex].FindControl("lblSignInSignOutWFID"));
                // To write code if "" then dont fill value in model for WorkflowID
                if (lblWorkflowID.Text != "")
                {
                    objSignInSignOutModel.WorkflowID = new Guid(lblWorkflowID.Text);
                    WorkflowId = new Guid(lblWorkflowID.Text);//For Sending mail
                }

                //Status
                DropDownList ddlStatus1 = (DropDownList)gvSISO.Rows[e.RowIndex].FindControl("ddlStatusdEdit");
                if (ddlStatus1.SelectedItem.Text == "Pending")
                {
                    lblError.Text = "Please change the status";
                    return;
                }
                objSignInSignOutModel.StatusID = Convert.ToInt32(ddlStatus1.SelectedValue);
                //InTime
                DropDownList ddlSignInTimeH1 = (DropDownList)gvSISO.Rows[e.RowIndex].FindControl("ddlInTimeHours");
                DropDownList ddlSignInTimeM1 = (DropDownList)gvSISO.Rows[e.RowIndex].FindControl("ddlInTimeMins");
                string strInTime = ddlSignInTimeH1.SelectedValue.ToString() + ":" + ddlSignInTimeM1.SelectedValue.ToString() + ":00";

                objSignInSignOutModel.SignInTime = Convert.ToDateTime(strInTime + " " + strDate.ToString());
                strSignInDateForMail = lblDate.Text.Trim(); //For Sending mail

                Label lblEditUserID = (Label)gvSISO.Rows[e.RowIndex].FindControl("lblEditUserID");
                objLeaveDetailsModel.UserID = Convert.ToInt32(lblEditUserID.Text.ToString());
                intUserIDForMail = Convert.ToInt32(lblEditUserID.Text.ToString());// For Sending mail

                //Out Time
                Label lblOutDate = (Label)gvSISO.Rows[e.RowIndex].FindControl("lblOutDate");
                string strOutDate = lblOutDate.Text.ToString();
                Label lblOutTimeH1 = (Label)gvSISO.Rows[e.RowIndex].FindControl("lblSignOutTime");
                if (lblOutTimeH1.Text.ToString() != "")
                {
                    DropDownList ddlOutTimeHours = (DropDownList)gvSISO.Rows[e.RowIndex].FindControl("ddlOutTimeHours");
                    DropDownList ddlOutTimeMinutes = (DropDownList)gvSISO.Rows[e.RowIndex].FindControl("ddlOutTimeMins");
                    string strOutTime = ddlOutTimeHours.SelectedValue.ToString() + ":" + ddlOutTimeMinutes.SelectedValue.ToString() + ":00";
                    objSignInSignOutModel.SignOutTime = Convert.ToDateTime(strOutTime + " " + strOutDate.ToString());
                }
                else
                {
                    objSignInSignOutModel.SignOutTime = Convert.ToDateTime(null);
                }

                //Approver's comments
                TextBox txtApproversComments = (TextBox)gvSISO.Rows[e.RowIndex].FindControl("txtApproversComments");
                strApproverCommentForMail = txtApproversComments.Text.Trim(); //For Sending mail
                if (ddlStatus1.SelectedItem.Text == "Approved")
                    objSignInSignOutModel.ApproverComments = "Approved by Administrator on behalf of approver: " + txtApproversComments.Text.ToString();
                else if (ddlStatus1.SelectedItem.Text == "Rejected")
                    objSignInSignOutModel.ApproverComments = "Rejected by Administrator on behalf of approver: " + txtApproversComments.Text.ToString();

                //Id
                Label lblID = (Label)gvSISO.Rows[e.RowIndex].FindControl("lblID");
                objSignInSignOutModel.SignInSignOutID = Convert.ToInt32(lblID.Text);
                if ((objSignInSignOutModel.SignOutTime < objSignInSignOutModel.SignInTime) && (lblOutTimeH1.Text.ToString() != ""))
                {
                    lblError.Text = "Sign Out Time cannot be smaller than the Sign In Time";
                }
                else
                {
                    lblError.Text = "";
                    UpdateStatus(objSignInSignOutModel);

                    // StartWorkflow
                    if (objSignInSignOutModel.StatusID == 2)
                    {
                        lblSuccess.Text = "Record updated successfully.";
                        UpdateEmployeeLeaveAndComp();

                        int rowEffected;
                        if (WorkflowId != null || WorkflowId.ToString() != "")
                            rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                        SendMail();
                    }
                    if (objSignInSignOutModel.StatusID == 3)
                    {
                        lblSuccess.Text = "Record updated successfully.";

                        int rowEffected;
                        if (WorkflowId != null || WorkflowId.ToString() != "")
                            rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                        SendMail();
                    }
                    // End WorkFlow

                    gvSISO.EditIndex = -1;
                    BindSISOData();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "gvSISO_RowUpdating", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvSISO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblUserName = (Label)e.Row.FindControl("lblUserName");
                    if (lblUserName != null)
                    {
                        //saturday/sunday and public holiday - BOLD
                        if (dsAdminApproval.Tables[0].Rows[e.Row.RowIndex]["date"] != null)
                        {
                            DateTime date1 = Convert.ToDateTime(dsAdminApproval.Tables[0].Rows[e.Row.DataItemIndex]["date"]);
                            string strDate = date1.ToShortDateString();
                            Label lblDate2 = (Label)e.Row.FindControl("lblDate1");
                            if (lblDate2 != null)
                                lblDate2.Text = strDate;

                            if ((date1.DayOfWeek.ToString() == "Sunday") || (date1.DayOfWeek.ToString() == "Saturday"))
                            {
                                lblUserName.Text = "<b>" + lblUserName.Text + "</b>";
                                Label lblDate1 = (Label)e.Row.FindControl("lblDate1");
                                if (lblDate1 != null)
                                    lblDate1.Text = lblDate1.Text;

                                Label lblSignInTime1 = (Label)e.Row.FindControl("lblSignInTime1");
                                if (lblSignInTime1 != null)
                                    lblSignInTime1.Text = "<b>" + lblSignInTime1.Text + "</b>";

                                Label lblSignOutTime1 = (Label)e.Row.FindControl("lblSignOutTime1");
                                if (lblSignOutTime1 != null)
                                    lblSignOutTime1.Text = lblSignOutTime1.Text;

                                Label lblTotalHours = (Label)e.Row.FindControl("lblTotalHours");
                                if (lblTotalHours != null)
                                    lblTotalHours.Text = "<b>" + lblTotalHours.Text + "</b>";

                                Label lblMode = (Label)e.Row.FindControl("lblMode");
                                if (lblMode != null)
                                    lblMode.Text = "<b>" + lblMode.Text + "</b>";

                                Label lblSignInComment = (Label)e.Row.FindControl("lblSignInComment");
                                if (lblSignInComment != null)
                                    lblSignInComment.Text = "<b>" + lblSignInComment.Text + "</b>";

                                Label lblSignOutComment = (Label)e.Row.FindControl("lblSignOutComment");
                                if (lblSignOutComment != null)
                                    lblSignOutComment.Text = "<b>" + lblSignOutComment.Text + "</b>";

                                Label lblStatus1 = (Label)e.Row.FindControl("lblStatus1");
                                if (lblStatus1 != null)
                                    lblStatus1.Text = "<b>" + lblStatus1.Text + "</b>";

                                Label lblApproversComments = (Label)e.Row.FindControl("lblApproversComments");
                                if (lblApproversComments != null)
                                    lblApproversComments.Text = "<b>" + lblApproversComments.Text + "</b>";

                                Label lblLastModified1 = (Label)e.Row.FindControl("lblLastModified");
                                if (lblLastModified1 != null)
                                    lblLastModified1.Text = "<b>" + lblLastModified1.Text + "</b>";
                            }
                            else
                            {
                                for (int i = 0; i < dsAdminApproval.Tables[2].Rows.Count; i++)
                                {
                                    DateTime date2 = Convert.ToDateTime(dsAdminApproval.Tables[2].Rows[i]["HolidayDate"].ToString());
                                    if (date1.Date == date2.Date)
                                    {
                                        lblUserName.Text = "<b>" + lblUserName.Text + "</b>";

                                        Label lblDate1 = (Label)e.Row.FindControl("lblDate1");
                                        if (lblDate1 != null)
                                            lblDate1.Text = lblDate1.Text;

                                        Label lblSignInTime1 = (Label)e.Row.FindControl("lblSignInTime1");
                                        if (lblSignInTime1 != null)
                                            lblSignInTime1.Text = "<b>" + lblSignInTime1.Text + "</b>";

                                        Label lblSignOutTime1 = (Label)e.Row.FindControl("lblSignOutTime1");
                                        if (lblSignOutTime1 != null)
                                            lblSignOutTime1.Text = lblSignOutTime1.Text;

                                        Label lblTotalHours = (Label)e.Row.FindControl("lblTotalHours");
                                        if (lblTotalHours != null)
                                            lblTotalHours.Text = "<b>" + lblTotalHours.Text + "</b>";

                                        Label lblMode = (Label)e.Row.FindControl("lblMode");
                                        if (lblMode != null)
                                            lblMode.Text = "<b>" + lblMode.Text + "</b>";

                                        Label lblSignInComment = (Label)e.Row.FindControl("lblSignInComment");
                                        if (lblSignInComment != null)
                                            lblSignInComment.Text = "<b>" + lblSignInComment.Text + "</b>";

                                        Label lblSignOutComment = (Label)e.Row.FindControl("lblSignOutComment");
                                        if (lblSignOutComment != null)
                                            lblSignOutComment.Text = "<b>" + lblSignOutComment.Text + "</b>";

                                        Label lblStatus1 = (Label)e.Row.FindControl("lblStatus1");
                                        if (lblStatus1 != null)
                                            lblStatus1.Text = "<b>" + lblStatus1.Text + "</b>";

                                        Label lblApproversComments = (Label)e.Row.FindControl("lblApproversComments");
                                        if (lblApproversComments != null)
                                            lblApproversComments.Text = "<b>" + lblApproversComments.Text + "</b>";
                                    }
                                }
                            }
                        }
                    }
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    if (dsAdminApproval.Tables[0].Rows[e.Row.RowIndex]["date"] != null)
                    {
                        DateTime date1 = Convert.ToDateTime(dsAdminApproval.Tables[0].Rows[e.Row.DataItemIndex]["date"]);
                        string strDate = date1.ToShortDateString();
                        Label lblDate2 = (Label)e.Row.FindControl("lblDate");
                        if (lblDate2 != null)
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
                    DataSet dsAdminApproval1 = objSignInSignOutBOL.GetStatus();
                    for (int i = 0; i < dsAdminApproval1.Tables[0].Rows.Count; i++)
                    {
                        if (dsAdminApproval1.Tables[0].Rows[i]["StatusName"].ToString() != "Cancelled")
                        {
                            ddlStatus1.Items.Add(new ListItem(dsAdminApproval1.Tables[0].Rows[i]["StatusName"].ToString(), dsAdminApproval1.Tables[0].Rows[i]["StatusID"].ToString()));
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "gvSISO_RowUpdating", ex.StackTrace);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "UpdateStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "UpdateEmployeeLeaveAndComp", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void FillEmployeeName()
        {
            try
            {
                //To fill the User DropDown
                ddlEmployeeName.Items.Clear();
                DataSet dsUserList = objConfigItemBOL.GetEmployeeListForAdminApproval();
                if (dsUserList.Tables[0].Rows.Count != 0)
                {
                    for (int rowCount = 0; rowCount < dsUserList.Tables[0].Rows.Count; rowCount++)
                    {
                        ddlEmployeeName.Items.Add(new ListItem(dsUserList.Tables[0].Rows[rowCount]["EmployeeName"].ToString(), dsUserList.Tables[0].Rows[rowCount]["UserID"].ToString()));
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "FillEmployeeName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion SISO Module

        #region Leave Module

        public void BindLeaveData()
        {
            try
            {
                dsAdminApproval = new DataSet();
                int StatusID = Convert.ToInt32(ddlStatus.SelectedValue);//pending

                string FromDate = string.Empty;

                if (txtSearchFromDate.Text == "")
                    FromDate = "Empty";
                else
                    FromDate = txtSearchFromDate.Text.ToString();

                string ToDate;
                if (txtSearchToDate.Text == "")
                    ToDate = "Empty";
                else
                    ToDate = txtSearchToDate.Text.ToString();

                dsAdminApproval = objLeaveDetailsBOL.GetLeaveForAdminApproval(StatusID, FromDate, ToDate);

                if (dsAdminApproval.Tables[0].Rows.Count > 0)
                {
                    gvLeave.DataSource = dsAdminApproval.Tables[0];
                    if (ddlStatus.SelectedValue.ToString() == "1")
                        gvLeave.Columns[15].Visible = true;
                    else
                        gvLeave.Columns[15].Visible = false;

                    gvLeave.DataBind();
                }
                else if (dsAdminApproval.Tables[0].Rows.Count == 0)
                {
                    gvLeave.DataSource = dsAdminApproval;
                    if (ddlStatus.SelectedValue.ToString() == "1")
                        gvLeave.Columns[15].Visible = true;
                    else
                        gvLeave.Columns[15].Visible = false;

                    gvLeave.DataBind();
                    lblError.Visible = true;
                    lblError.Text = "No records found";
                }
                else
                {
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "No records found";
                }

                gvSISO.DataSource = null;
                gvSISO.DataBind();
                gvCompensatory.DataSource = null;
                gvCompensatory.DataBind();
                gvOutOfOffice.DataSource = null;
                gvOutOfOffice.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "BindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvLeave_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvLeave.PageIndex = e.NewPageIndex;
                gvLeave.EditIndex = -1;
                BindLeaveData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "gvLeave_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvLeave_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    TextBox txtApproversComment = ((TextBox)e.Row.FindControl("txtgrvApproverComments"));
                    TextBox txtFromDate = ((TextBox)e.Row.FindControl("txtgrvFormDate"));
                    TextBox txtToDate = ((TextBox)e.Row.FindControl("txtgrvToDate"));
                    LinkButton lnkUpdate = ((LinkButton)e.Row.FindControl("lbnUpdate"));

                    lnkUpdate.Attributes.Add("onClick", "return Validation(" + txtFromDate.ClientID + "," + txtToDate.ClientID + "," + txtApproversComment.ClientID + ");");
                    DropDownList ddlgrvStatusName = (DropDownList)e.Row.FindControl("ddlgrvStatusName");
                    Label lblgrvStatusID = (Label)e.Row.FindControl("lblgrvStatusID");

                    DataSet dsGetStatus = new DataSet();
                    dsGetStatus = objLeaveDetailsBOL.GetStatusDetails();
                    if (dsGetStatus.Tables[0].Rows.Count > 0)
                    {
                        for (int rowCount = 0; rowCount < dsGetStatus.Tables[0].Rows.Count; rowCount++)
                        {
                            ddlgrvStatusName.Items.Add(new ListItem(dsGetStatus.Tables[0].Rows[rowCount]["StatusName"].ToString(), dsGetStatus.Tables[0].Rows[rowCount]["StatusID"].ToString()));
                        }
                        ddlgrvStatusName.SelectedValue = lblgrvStatusID.Text.ToString();
                    }

                    TextBox txtgrvToDate = (TextBox)e.Row.FindControl("txtgrvToDate");
                    TextBox txtgrvFormDate = (TextBox)e.Row.FindControl("txtgrvFormDate");

                    txtgrvToDate.Attributes.Add("onkeydown", "return false");
                    txtgrvFormDate.Attributes.Add("onkeydown", "return false");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval.aspx.cs", "gvLeave_RowDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvLeave_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblSuccess.Text = "";
                gvLeave.EditIndex = e.NewEditIndex;

                BindLeaveData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval.aspx.cs", "gvLeave_RowEditing", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvLeave_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                lblSuccess.Text = "";
                lblError.Text = "";

                gvLeave.EditIndex = -1;
                BindLeaveData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "gvLeave_RowCancelingEdit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #region UpdateFutureLeaves

        protected void UpdateFutureLeaves()
        {
            try
            {
                //Correction done by Anushree Tirwadkar on 9/9/2011

                string strTotalLeaveBalance = string.Empty;

                String[] TotalLeavesBalance;
                int BalanceLeaves;

                DataSet dsTotalLeaves = objLeaveDetailsBOL.TotalLeaveBalance(objLeaveDetailsModel);
                strTotalLeaveBalance = Convert.ToString(dsTotalLeaves.Tables[0].Rows[0]["Leave_Balance"].ToString());
                TotalLeavesBalance = strTotalLeaveBalance.Split('.');
                BalanceLeaves = Convert.ToInt32(TotalLeavesBalance[0].ToString());

                DataSet dsLeaveTransaction = objLeaveTransactionBOL.dsGetLeaveTransactionForFuture(objLeaveDetailsModel);
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
                                objLeaveTransactionBOL.UpdateLeaveTransactionDetailsForFuture(objLeaveTransDetailsModel);
                            }
                        }
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval", "UpdateFutureLeaves", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion UpdateFutureLeaves

        protected void gvLeave_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                bool WfApprovedLocked = false, oneLeaveWeekend = false;
                double TotalLeaves = 0, CorrectionLeaves;
                int BalanceLeaves, absent;
                String[] strLeaves, TotalLeavesBalance;
                DateTime FromDate, ToDate;
                string strTotalLeaveBalance = string.Empty;
                WorkflowId = new Guid();

                GridViewRow row = gvLeave.Rows[e.RowIndex];

                Label lblLeaveDetailID = row.FindControl("lblLeaveDetailID1") as Label;
                objLeaveDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text.Trim());
                objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text.Trim());

                Label lblLeaveDetailWFID = row.FindControl("lblLeaveDetailWFID1") as Label;
                objLeaveDetailsModel.WorkFlowID = new Guid(lblLeaveDetailWFID.Text.Trim());
                WorkflowId = new Guid(lblLeaveDetailWFID.Text.Trim()); //For Sending mail

                TextBox txtgrvFormDate = row.FindControl("txtgrvFormDate") as TextBox;
                objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtgrvFormDate.Text.Trim());
                strFromDate = txtgrvFormDate.Text.Trim();//For Sending mail

                TextBox txtgrvToDate = row.FindControl("txtgrvToDate") as TextBox;
                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtgrvToDate.Text.Trim());
                strToDate = txtgrvToDate.Text.Trim();//For Sending mail

                Label lblgrvLeaveReason = row.FindControl("lblgrvLeaveReason") as Label;
                objLeaveDetailsModel.LeaveResason = lblgrvLeaveReason.Text.Trim();

                DropDownList ddlStatusID = row.FindControl("ddlgrvStatusName") as DropDownList;
                objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatusID.SelectedValue);
                if (ddlStatusID.SelectedItem.Text == "Pending")
                {
                    lblError.Text = "Please change the status";
                    lblSuccess.Text = "";
                    return;
                }
                if (ddlStatusID.Visible == false)
                {
                    WfApprovedLocked = true;
                }

                Label lblgrvApproverEdit = row.FindControl("lblgrvApproverEdit") as Label;
                objLeaveDetailsModel.ApproverID = Convert.ToInt32(lblgrvApproverEdit.Text.ToString());

                objLeaveDetailsModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                //Getting Total Leaves
                Label lblEditUserID = row.FindControl("lblEditUserID") as Label;
                objLeaveDetailsModel.UserID = Convert.ToInt32(lblEditUserID.Text.ToString());
                intUserIDForMail = Convert.ToInt32(lblEditUserID.Text.ToString());// For Sending mail

                //Getting All the Holidays Details
                objHolidayModel.UserID = objLeaveDetailsModel.UserID;
                objHolidayModel.StartDate = objLeaveDetailsModel.LeaveDateFrom;
                objHolidayModel.EndDate = objLeaveDetailsModel.LeaveDateTo;
                DataSet dsHolidaysList = objHolidayBOL.bindHolidaysForLeaveApprovals(objHolidayModel);

                //  DataSet dsHolidaysList = objHolidayMasterBOL.bindData();

                //Check the leaves dates are in SignInSignOut Table
                DataSet dsCheckInSignIn = objLeaveDetailsBOL.CheckLeavesinSignIn(objLeaveDetailsModel);

                //For Getting ConfigDate from Database
                DataSet dsConfigItem = objLeaveDetailsBOL.GetLeaveDetails(objLeaveDetailsModel);
                DateTime ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                FromDate = Convert.ToDateTime(txtgrvFormDate.Text.Trim());
                ToDate = Convert.ToDateTime(txtgrvToDate.Text.Trim());
                TimeSpan ts = ToDate - FromDate;

                TotalLeaves = ts.TotalDays + 1;
                strLeaves = new String[Convert.ToInt32(TotalLeaves)];

                Label lblTotalLeaves = row.FindControl("lblTotalLeaves") as Label;
                Label lblTotalAbsentLeaves = row.FindControl("lblTotalAbsentLeaves") as Label;
                Label lblgrvEditUserName = row.FindControl("lblgrvEditUserName") as Label;

                TextBox txtgrvApproverComments = row.FindControl("txtgrvApproverComments") as TextBox;
                strApproverCommentForMail = txtgrvApproverComments.Text.Trim();//For Sending mail
                if (ddlStatusID.SelectedItem.Text == "Approved")
                    objLeaveDetailsModel.ApproverComments = "Approved by Administrator on behalf of Approver: " + txtgrvApproverComments.Text.Trim();
                else if (ddlStatusID.SelectedItem.Text == "Rejected")
                    objLeaveDetailsModel.ApproverComments = "Rejected by Administrator on behalf of Approver: " + txtgrvApproverComments.Text.Trim();
                else if (ddlStatusID.SelectedItem.Text == "Cancelled")
                    objLeaveDetailsModel.ApproverComments = "Cancelled by Administrator on behalf of Approver: " + txtgrvApproverComments.Text.Trim();
                if (ddlStatusID.SelectedValue == "3")//Rejected
                {
                    // if the Leave is Rejected by Administrator the code comes here
                    //Here delete the all records of related leavedetailsID
                    DeleteLeaveTransDetails();
                    UpdateEmployeeLeaveAndComp();

                    objLeaveDetailsModel.TotalLeaveDays = Convert.ToInt32(lblTotalLeaves.Text);
                    objLeaveDetailsModel.LeaveCorrectionDays = Convert.ToInt32(lblTotalAbsentLeaves.Text);
                    objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatusID.SelectedValue.ToString());

                    strLeaveToDate = txtgrvToDate.Text.Trim();
                    UpdateLeaveDetails(strLeaveToDate);

                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Leave Details are Rejected";
                    lblError.Text = "";
                    gvLeave.EditIndex = -1;
                    BindLeaveData();
                    if (WfApprovedLocked == false)
                    {
                        if (ddlStatusID.SelectedValue == "3")
                        {
                            int rowEffected;
                            if (WorkflowId != null || WorkflowId.ToString() != "")
                                rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                            SendMail();
                        }
                    }
                    return;
                }
                else if (ddlStatusID.SelectedValue == "4")//Cancelled
                {
                    // if the Leave is Cancelled by Administrator the code comes here
                    //Here delete the all records of related leavedetailsID
                    DeleteLeaveTransDetails();
                    UpdateEmployeeLeaveAndComp();

                    objLeaveDetailsModel.TotalLeaveDays = Convert.ToInt32(lblTotalLeaves.Text);
                    objLeaveDetailsModel.LeaveCorrectionDays = Convert.ToInt32(lblTotalAbsentLeaves.Text);
                    objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatusID.SelectedValue.ToString());
                    strLeaveToDate = txtgrvToDate.Text.Trim();
                    UpdateLeaveDetails(strLeaveToDate);

                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Leave Details are Cancelled";
                    lblError.Text = "";
                    gvLeave.EditIndex = -1;
                    BindLeaveData();
                    if (WfApprovedLocked == false)
                    {
                        if (ddlStatusID.SelectedValue == "4")
                        {
                            int rowEffected;
                            if (WorkflowId != null || WorkflowId.ToString() != "")
                                rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                            SendMail();
                        }
                    }
                    return;
                }

                int j = 0, TotalLeavesApplyedFor = 0;
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
                if (oneLeaveWeekend == true)
                {
                    lblError.Visible = true;
                    lblError.Text = lblgrvEditUserName.Text + " has  already Signed In for these dates / It is in Holiday List / Sat/Sun, So you can't approve leaves for these dates";
                    lblSuccess.Text = "";
                    gvLeave.EditIndex = -1;
                    BindLeaveData();
                    return;
                }
                TotalLeavesApplyedFor = j++;

                if (TotalLeavesApplyedFor == 0)
                {
                    lblError.Text = lblgrvEditUserName.Text + " has already Signed In for these dates / It is in Holiday List / Sat/Sun, So you can't approve leaves for these dates";
                    lblSuccess.Text = "";
                    gvLeave.EditIndex = -1;
                    BindLeaveData();
                    return;
                }

                if (ddlStatusID.SelectedValue == "2")
                {
                    //Here delete the all records of related leavedetailsID
                    objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblEditUserID.Text.ToString());
                    objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text.ToString());
                }

                if (Convert.ToDateTime(txtgrvToDate.Text.Trim()) < Convert.ToDateTime(DateTime.Now))
                    objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(DateTime.Now);
                else
                    objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtgrvToDate.Text.Trim());

                DataSet dsTotalLeaves = objLeaveDetailsBOL.TotalLeaveBalance(objLeaveDetailsModel);
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

                    strLeaveToDate = txtgrvToDate.Text.Trim();
                    UpdateLeaveDetails(strLeaveToDate);
                    lblError.Text = lblgrvEditUserName.Text + " has applied for " + TotalLeavesApplyedFor + " leaves, but his/her Leave balance is " + BalanceLeaves + ", So " + absent + " days will be marked as absent.";
                    lblSuccess.Text = "";

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
                                objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblEditUserID.Text.ToString());
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
                    objLeaveDetailsModel.TotalLeaveDays = TotalLeavesApplyedFor;
                    objLeaveDetailsModel.LeaveCorrectionDays = Convert.ToInt32(0);
                    strLeaveToDate = txtgrvToDate.Text.Trim();
                    UpdateLeaveDetails(strLeaveToDate);

                    for (int k = 0; k < strLeaves.Length; k++)
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
                                objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblEditUserID.Text.ToString());
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
                    if (ddlStatusID.SelectedValue == "2")//Approved
                    {
                        int rowEffected;
                        if (WorkflowId != null || WorkflowId.ToString() != "")
                            rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                        SendMail();
                    }
                    if (ddlStatusID.SelectedValue == "3")//Rejected
                    {
                        int rowEffected;
                        if (WorkflowId != null || WorkflowId.ToString() != "")
                            rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                        SendMail();
                    }
                    if (ddlStatusID.SelectedValue == "4")//Cancelled
                    {
                        int rowEffected;
                        if (WorkflowId != null || WorkflowId.ToString() != "")
                            rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                        SendMail();
                    }
                }
                else
                {
                    if (ddlStatusID.SelectedValue == Convert.ToString("2"))
                    {
                        //Sending a Mail to Employee after Updating Leaves
                        if (WfApprovedLocked == true)
                        {
                            objLeaveDetailsModel.UserID = Convert.ToInt32(lblEditUserID.Text.ToString());
                            SendingMailToReportingPerson();
                        }
                    }
                }
                gvLeave.EditIndex = -1;

                BindLeaveData();

                lblSuccess.Visible = true;
                lblSuccess.Text = "Leave details are updated successfully";
                lblError.Text = "";

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
                throw ex;
            }
            catch (V2Exceptions ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("Already leave applied for this dates.") != 0)
                {
                    lblError.Visible = false;
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval.aspx.cs", "gvLeave_RowUpdating", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "User has already applied for leaves for these dates.";
                    lblSuccess.Text = "";
                }
            }
        }

        public void DeleteLeaveTransDetails()
        {
            try
            {
                int rowsAffected = objLeaveTransactionBOL.DeleteLeaveTransactionDetails(objLeaveTransDetailsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval.aspx.cs", "DeleteLeaveTransDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void UpdateLeaveDetails(string strLeaveToDate)
        {
            try
            {
                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(strLeaveToDate.Trim());
                int rowsAffected = objLeaveDetailsBOL.UpdateTMLeaveDetails(objLeaveDetailsModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval.aspx.cs", "UpdateLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void AddLeaveTransDetails()
        {
            try
            {
                int rowsAffected = objLeaveTransactionBOL.AddLeaveTransactionDetails(objLeaveTransDetailsModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval.aspx.cs", "AddLeaveTransDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion Leave Module

        #region Compensatory Module

        public void BindCompensatoryData()
        {
            try
            {
                dsAdminApproval = new DataSet();
                int StatusID = Convert.ToInt32(ddlStatus.SelectedValue);//pending

                string FromDate = string.Empty;

                if (txtSearchFromDate.Text == "")
                    FromDate = "Empty";
                else
                    FromDate = txtSearchFromDate.Text.ToString();

                string ToDate;
                if (txtSearchToDate.Text == "")
                    ToDate = "Empty";
                else
                    ToDate = txtSearchToDate.Text.ToString();

                dsAdminApproval = objCompensationDetailsBOL.GetCompensatoryLeaveForAdminApproval(StatusID, FromDate, ToDate);

                if (dsAdminApproval.Tables[0].Rows.Count > 0)
                {
                    gvCompensatory.DataSource = dsAdminApproval.Tables[0];
                    if (ddlStatus.SelectedValue.ToString() == "1")
                        gvCompensatory.Columns[12].Visible = true;
                    else
                        gvCompensatory.Columns[12].Visible = false;

                    gvCompensatory.DataBind();
                }
                else if (dsAdminApproval.Tables[0].Rows.Count == 0)
                {
                    gvCompensatory.DataSource = dsAdminApproval;
                    if (ddlStatus.SelectedValue.ToString() == "1")
                        gvCompensatory.Columns[12].Visible = true;
                    else
                        gvCompensatory.Columns[12].Visible = false;

                    gvCompensatory.DataBind();
                    lblError.Visible = true;
                    lblError.Text = "No records found";
                }
                else
                {
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "No records found";
                }

                gvSISO.DataSource = null;
                gvSISO.DataBind();
                gvLeave.DataSource = null;
                gvLeave.DataBind();
                gvOutOfOffice.DataSource = null;
                gvOutOfOffice.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "BindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvCompensatory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    TextBox txtApproversComment = ((TextBox)e.Row.FindControl("txtgrvApproverComments"));
                    LinkButton lnkUpdate = ((LinkButton)e.Row.FindControl("lbnUpdate"));
                    lnkUpdate.Attributes.Add("onClick", "return ValidationForCompOff(" + txtApproversComment.ClientID + ");");

                    DropDownList ddlgrvStatusName = (DropDownList)e.Row.FindControl("ddlgrvStatusName");
                    Label lblgrvStatusID = (Label)e.Row.FindControl("lblgrvStatusID");

                    DataSet dsGetLeaveStatus = objLeaveDetailsBOL.GetStatusDetails();
                    if (dsGetLeaveStatus.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            ddlgrvStatusName.Items.Add(new ListItem(dsGetLeaveStatus.Tables[0].Rows[i]["StatusName"].ToString(), dsGetLeaveStatus.Tables[0].Rows[i]["StatusID"].ToString()));
                        }
                        ddlgrvStatusName.SelectedValue = lblgrvStatusID.Text.ToString();
                    }

                    TextBox txtgrvAppliedFor = (TextBox)e.Row.FindControl("txtgrvAppliedFor");
                    txtgrvAppliedFor.Attributes.Add("onkeydown", "return false");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "gvCompensatory_RowDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvCompensatory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCompensatory.PageIndex = e.NewPageIndex;
                gvCompensatory.EditIndex = -1;
                BindCompensatoryData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "gvCompensatory_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvCompensatory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                lblSuccess.Text = "";
                lblError.Text = "";

                gvCompensatory.EditIndex = -1;
                BindCompensatoryData();
            }
            catch (V2Exceptions ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "gvCompensatory_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvCompensatory_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblSuccess.Text = "";
                gvCompensatory.EditIndex = e.NewEditIndex;
                BindCompensatoryData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "gvCompensatory_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gvCompensatory_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                bool WfApprovedLocked = false;
                DateTime FromDate, ConfigDate;
                int rowsAffected;

                GridViewRow row = gvCompensatory.Rows[e.RowIndex];

                Label lblTMmember = row.FindControl("lblEditUserID") as Label;

                objCompensationDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                intUserIDForMail = Convert.ToInt32(lblTMmember.Text.ToString());

                Label lblEditCompensationID = row.FindControl("lblEditCompensationID") as Label;
                objCompensationDetailsModel.CompensationID = Convert.ToInt32(lblEditCompensationID.Text);

                Label lblCompensationWFID = row.FindControl("lblCompensationWFID") as Label;
                if (lblCompensationWFID.Text != "")
                {
                    objCompensationDetailsModel.WorkFlowID = new Guid(lblCompensationWFID.Text);
                    WorkflowId = new Guid(lblCompensationWFID.Text.Trim());//For Sending mail
                }

                Label lblgrvEditUserName = row.FindControl("lblgrvEditUserName") as Label;

                TextBox txtEditAppliedFor = row.FindControl("txtgrvAppliedFor") as TextBox;
                strFromDate = txtEditAppliedFor.Text.Trim();//For Sending mail

                DropDownList ddlStatusID = row.FindControl("ddlgrvStatusName") as DropDownList;
                if (ddlStatusID.SelectedItem.Text == "Pending")
                {
                    lblError.Text = "Please change the status";
                    return;
                }

                objCompensationDetailsModel.StatusID = Convert.ToInt32(ddlStatusID.SelectedValue);
                if (ddlStatusID.Visible == false)
                {
                    WfApprovedLocked = true;
                }

                Label lblEditgrvReason = row.FindControl("lblEditgrvReason") as Label;

                objCompensationDetailsModel.Resason = lblEditgrvReason.Text.Trim();

                TextBox ApproverComments = row.FindControl("txtgrvApproverComments") as TextBox;
                strApproverCommentForMail = ApproverComments.Text.Trim();//For Sending mail
                if (ddlStatusID.SelectedItem.Text == "Approved")
                    objCompensationDetailsModel.ApproverComments = "Approved by Administrator on behalf of Approver: " + Convert.ToString(ApproverComments.Text.ToString());
                else if (ddlStatusID.SelectedItem.Text == "Rejected")
                    objCompensationDetailsModel.ApproverComments = "Rejected by Administrator on behalf of Approver: " + Convert.ToString(ApproverComments.Text.ToString());
                else if (ddlStatusID.SelectedItem.Text == "Cancelled")
                    objCompensationDetailsModel.ApproverComments = "Cancelled by Administrator on behalf of Approver: " + Convert.ToString(ApproverComments.Text.ToString());

                objCompensationDetailsModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                objCompensationDetailsModel.CompensationTo = Convert.ToDateTime(txtEditAppliedFor.Text.Trim());

                objCompensationDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                intUserIDForMail = Convert.ToInt32(lblTMmember.Text.ToString()); //For Sending mail to user
                //Getting All the Holidays Details
                // DataSet dsHolidaysList = objHolidayMasterBOL.bindData();
                objHolidayModel.UserID = objCompensationDetailsModel.UserID;
                objHolidayModel.StartDate = Convert.ToDateTime(strFromDate);
                objHolidayModel.EndDate = Convert.ToDateTime(strFromDate);
                DataSet dsHolidaysList = objHolidayBOL.bindHolidaysForLeaveApprovals(objHolidayModel);
                //For Getting ConfigDate from Database
                DataSet dsConfigItem = objCompensationDetailsBOL.GetCompensationDetails(objCompensationDetailsModel);
                ConfigDate = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                FromDate = Convert.ToDateTime(txtEditAppliedFor.Text.Trim());
                Boolean flag = false;
                rowsAffected = objCompensationDetailsBOL.CheckSignInForCompensation(objCompensationDetailsModel);
                if (rowsAffected > 0)
                {
                    for (int k = 0; k < dsHolidaysList.Tables[0].Rows.Count; k++)
                    {
                        if (FromDate.ToString() == dsHolidaysList.Tables[0].Rows[k]["HolidayDate"].ToString())
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (FromDate.DayOfWeek.ToString() == "Saturday" || FromDate.DayOfWeek.ToString() == "Sunday")
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        objCompensationDetailsModel.AppliedFor = Convert.ToDateTime(FromDate.ToString());

                        UpdateCompenstionDetails();
                        gvCompensatory.EditIndex = -1;
                        lblError.Visible = true;
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "Compensatory Leave details are updated successfully";
                        lblError.Text = "";

                        if (ddlStatusID.SelectedValue == Convert.ToString("2"))
                        {
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
                            objCompensationDetailsModel.AppliedFor = Convert.ToDateTime(FromDate.ToString());
                            objCompensationDetailsModel.StatusID = Convert.ToInt32(3);
                            UpdateCompenstionDetails();
                            lblSuccess.Visible = true;
                            lblSuccess.Text = "Compensatory Leave Details are Rejected";
                        }
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = lblgrvEditUserName.Text + " has not signed-in on the day you are approving compensatory off for, Select Valid date";
                    lblSuccess.Text = "";
                }

                if (WfApprovedLocked == false)
                {
                    if (objCompensationDetailsModel.StatusID == 2)//Approved
                    {
                        int rowEffected;
                        if (WorkflowId != null || WorkflowId.ToString() != "")
                            rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                        SendMail();
                    }
                    if (objCompensationDetailsModel.StatusID == 3)//Rejected
                    {
                        int rowEffected;
                        if (WorkflowId != null || WorkflowId.ToString() != "")
                            rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                        SendMail();
                    }
                    if (objCompensationDetailsModel.StatusID == 4)//Cancelled
                    {
                        int rowEffected;
                        if (WorkflowId != null || WorkflowId.ToString() != "")
                            rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                        SendMail();
                    }
                }
                else
                {
                    if (objCompensationDetailsModel.StatusID == 2)
                    {
                        //Sending a Mail to Employee after Updating compensatory Leaves
                        if (WfApprovedLocked == true)
                        {
                            objLeaveDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                            SendingMailToReportingPersonForCompOff();
                        }
                    }
                }
                BindCompensatoryData();
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "gvCompensatory_RowUpdating", ex.StackTrace);
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

        public void AddCompensationTransactionDetails()
        {
            try
            {
                int rowsAffected = objLeaveTransactionBOL.AddCompensationTransactionDetails(objLeaveTransDetailsModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "AddCompensationTransactionDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        public void DeleteCompensationTransactionDetails()
        {
            try
            {
                int rowsAffected = objLeaveTransactionBOL.DeleteCompensationTransactionDetails(objLeaveTransDetailsModel);
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

        public void UpdateCompenstionDetails()
        {
            try
            {
                int rowsAffected = objCompensationDetailsBOL.UpdateApprovalCompenstionDetails(objCompensationDetailsModel);
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

        public void SendingMailToReportingPersonForCompOff()
        {
            try
            {
                DataSet dsReportingTo = objLeaveDetailsBOL.GetReportingTo(objLeaveDetailsModel);
                if (dsReportingTo.Tables[0].Rows.Count > 0)
                {
                    //Send  new password to the employee through Email.
                    MailMessage objMailMessage = new MailMessage();
                    SmtpClient smtpClient = new SmtpClient();
                    objMailMessage.To.Add(new MailAddress(dsReportingTo.Tables[0].Rows[0]["EmployeeEmailID"].ToString()));

                    string strBody;
                    DataSet dsCancelDetails = objCompensationDetailsBOL.GetCancelCompOffDetails(objCompensationDetailsModel);

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

                    strBody = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi ##EmployeeName##," + " \n\n" + " Updated Compensatory Leave Application Details: " + " \n\n" + "  Applied For: ##AppliedFor##" + " \n" + "  Reason: ##Reason##" + "\n\n" + "Updated the Approved Compensatory Leave Details, the required updates are made in the system.";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval.aspx.cs", "SendingMailToReportingPersonForCompOff", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Compensatory Module

        #region Out Of Office

        public void BindOutOfOfficeData()
        {
            try
            {
                gvSISO.DataSource = null;
                gvSISO.DataBind();
                gvLeave.DataSource = null;
                gvLeave.DataBind();
                gvCompensatory.DataSource = null;
                gvCompensatory.DataBind();
                gvOutOfOffice.DataSource = null;
                gvOutOfOffice.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "BindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Out Of Office

        public void SendMail()
        {
            try
            {
                if (intUserIDForMail != 0)
                {
                    objLeaveDetailsModel.UserID = intUserIDForMail;
                    DataSet dsReportingTo = objLeaveDetailsBOL.GetReportingTo(objLeaveDetailsModel);

                    if (dsReportingTo.Tables[0].Rows.Count > 0)
                    {
                        //Send  new password to the employee through Email.
                        MailMessage objMailMessage = new MailMessage();
                        SmtpClient smtpClient = new SmtpClient();
                        string UserName, ApproverName, Reason, ToDate, FromDate;

                        objMailMessage.To.Add(new MailAddress(dsReportingTo.Tables[0].Rows[0]["EmployeeEmailID"].ToString()));
                        objMailMessage.CC.Add(new MailAddress(dsReportingTo.Tables[0].Rows[0]["ReporterMailid"].ToString()));

                        UserName = dsReportingTo.Tables[0].Rows[0]["EmployeeName"].ToString();
                        ApproverName = dsReportingTo.Tables[0].Rows[0]["ReporterName"].ToString();

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

                        if (ddlModuleType.SelectedItem.Text == "Sign-In Sign-Out")
                        {
                            objMailMessage.Subject = "Orbit Entry Updated for " + strSignInDateForMail;
                            objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi " + "<b>" + UserName + "," + "</b>" + "<br>" + "<br>" + "Your Orbit entry for " + "<b>" + strSignInDateForMail + "</b>" + " was updated by Administrator on behalf of " + "<b>" + ApproverName + "</b>" + ". " + "<br>" + "Approver's comment: " + strApproverCommentForMail + "<br>" + "<br>" + "<br>" + "------------------------------------------------" + "<br>" + "Auto generated by ORBIT System" + "</font>";
                        }
                        else if (ddlModuleType.SelectedItem.Text == "Leave")
                        {
                            objMailMessage.Subject = "Leave Details Updated for period" + strFromDate + " - " + strToDate;
                            objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi " + "<b>" + UserName + "," + "</b>" + "<br>" + "<br>" + "Your Leave application for period " + "<b>" + strFromDate + " - " + strToDate + "</b>" + " was updated by Administrator on behalf of " + "<b>" + ApproverName + "</b>" + ". " + "<br>" + "Approver's comment: " + strApproverCommentForMail + "<br>" + "<br>" + "<br>" + "------------------------------------------------" + "<br>" + "Auto generated by ORBIT System" + "</font>";
                        }
                        else if (ddlModuleType.SelectedItem.Text == "Compensatory")
                        {
                            objMailMessage.Subject = "Compensatory Leave Details Updated for " + strFromDate;
                            objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi " + "<b>" + UserName + "," + "</b>" + "<br>" + "<br>" + "Your Compensatory Leave application for " + "<b>" + strFromDate + "</b>" + " was updated by Administrator on behalf of " + "<b>" + ApproverName + "</b>" + ". " + "<br>" + "Approver's comment: " + strApproverCommentForMail + "<br>" + "<br>" + "<br>" + "------------------------------------------------" + "<br>" + "Auto generated by ORBIT System" + "</font>";
                        }
                        else if (ddlModuleType.SelectedItem.Text == "Out Of Office")
                        {
                            objMailMessage.Subject = "Out Of Office Details Updated for " + strFromDate;
                            objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi " + "<b>" + UserName + "," + "</b>" + "<br>" + "<br>" + "Your Out Of Office for " + "<b>" + strFromDate + "</b>" + " was updated by Administrator on behalf of " + "<b>" + ApproverName + "</b>" + " and the following were the approver's comment " + "<br>" + "Approver's comment: " + strApproverCommentForMail + "</font>";
                        }

                        SMTPHelper objhelper = new SMTPHelper();
                        objhelper.SendMail(objMailMessage.From.ToString(), objMailMessage.To.ToString(), objMailMessage.Subject, objMailMessage.Body, objMailMessage.CC.ToString(), null, 1);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "SendingMailToReportingPerson", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void SendingMailToReportingPerson()
        {
            try
            {
                DataSet dsReportingTo = objLeaveDetailsBOL.GetReportingTo(objLeaveDetailsModel);
                if (dsReportingTo.Tables[0].Rows.Count > 0)
                {
                    //Send  new password to the employee through Email.
                    MailMessage objMailMessage = new MailMessage();
                    SmtpClient smtpClient = new SmtpClient();
                    objMailMessage.Subject = "Updated Leave Details";
                    objMailMessage.To.Add(new MailAddress(dsReportingTo.Tables[0].Rows[0]["EmployeeEmailID"].ToString()));

                    string UserName, ApproverName, Reason, ToDate, FromDate;

                    int Applyleaves;

                    UserName = dsReportingTo.Tables[0].Rows[0]["EmployeeName"].ToString();
                    ApproverName = dsReportingTo.Tables[0].Rows[0]["ReporterName"].ToString();

                    DataSet dsCancelDetails = objLeaveDetailsBOL.GetCancelLeaveDetails(objLeaveDetailsModel);

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

                    objMailMessage.Subject = "Updated Leave Details";

                    objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi " + "<b>" + UserName + "</b>" + " ," + "<br>" + "<br>" + "Updated Leave Application Details: " + "<br>" + "<br>" + " <br> " + "FromDate: " + FromDate + " <br> " + "ToDate: " + ToDate + " <br> " + "Leave Reason: " + Reason + " <br> " + "Leave Applied For: " + Applyleaves + " <br> " + " <br> " + " Updated the Approved Leave Details, the required updates are made in the system.";

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

        protected void ChkSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTest = (CheckBox)sender;
            GridViewRow grdRow = (GridViewRow)chkTest.NamingContainer;
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkAll = (CheckBox)gvSISO.HeaderRow.FindControl("chkSelectAll");
            if (chkAll.Checked == true)
            {
                foreach (GridViewRow gvRow in gvSISO.Rows)
                {
                    CheckBox chkSel = (CheckBox)gvRow.FindControl("chkSelect");
                    chkSel.Checked = true;
                }
            }
            else
            {
                foreach (GridViewRow gvRow in gvSISO.Rows)
                {
                    CheckBox chkSel = (CheckBox)gvRow.FindControl("chkSelect");
                    chkSel.Checked = false;
                }
            }
        }

        protected void btnupdateselect_click(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in gvSISO.Rows)
            {
                CheckBox chkSel = (CheckBox)gvRow.FindControl("chkSelect");
                if (chkSel.Checked == true)
                {
                    try
                    {
                        Label lblDate = ((Label)gvRow.FindControl("lblDate1"));
                        string strDate = lblDate.Text.ToString().Replace("<b>", "").Trim();
                        strDate = strDate.Replace("</b>", "").Trim();

                        //for workflow...
                        Label lblWorkflowID = ((Label)gvRow.FindControl("lblSignInSignOutWFID"));
                        // To write code if "" then dont fill value in model for WorkflowID
                        if (lblWorkflowID.Text != "")
                        {
                            objSignInSignOutModel.WorkflowID = new Guid(lblWorkflowID.Text);
                            WorkflowId = new Guid(lblWorkflowID.Text);//For Sending mail
                        }

                        //Status
                        DropDownList ddlStatus1 = (DropDownList)gvRow.FindControl("ddlStatusdEdit");

                        objSignInSignOutModel.StatusID = Convert.ToInt32(2);
                        //InTime
                        DropDownList ddlSignInTimeH1 = (DropDownList)gvRow.FindControl("ddlInTimeHours");
                        DropDownList ddlSignInTimeM1 = (DropDownList)gvRow.FindControl("ddlInTimeMins");
                        string strInTime = "09:30:00";

                        objSignInSignOutModel.SignInTime = Convert.ToDateTime(strInTime + " " + strDate.ToString());
                        strSignInDateForMail = lblDate.Text.Trim(); //For Sending mail

                        Label lblEditUserID = ((Label)gvRow.FindControl("lblEditUserID1"));
                        objLeaveDetailsModel.UserID = Convert.ToInt32(lblEditUserID.Text.ToString());
                        intUserIDForMail = Convert.ToInt32(lblEditUserID.Text.ToString());// For Sending mail

                        //Out Time
                        Label lblOutDate = (Label)gvRow.FindControl("lblOutDate1");
                        string strOutDate = lblOutDate.Text.ToString();
                        Label lblOutTimeH1 = (Label)gvRow.FindControl("lblSignOutTime1");
                        if (lblOutTimeH1.Text.ToString() != "")
                        {
                            DropDownList ddlOutTimeHours = (DropDownList)gvRow.FindControl("ddlOutTimeHours");
                            DropDownList ddlOutTimeMinutes = (DropDownList)gvRow.FindControl("ddlOutTimeMins");
                            string strOutTime = "18:30:00";
                            objSignInSignOutModel.SignOutTime = Convert.ToDateTime(strOutTime + " " + strOutDate.ToString());
                        }
                        else
                        {
                            objSignInSignOutModel.SignOutTime = Convert.ToDateTime(null);
                        }

                        objSignInSignOutModel.ApproverComments = "Approved by :Admin ";

                        //Id
                        Label lblID = (Label)gvRow.FindControl("lblID1");
                        objSignInSignOutModel.SignInSignOutID = Convert.ToInt32(lblID.Text);
                        if ((objSignInSignOutModel.SignOutTime < objSignInSignOutModel.SignInTime) && (lblOutTimeH1.Text.ToString() != ""))
                        {
                            lblError.Text = "Sign Out Time cannot be smaller than the Sign In Time";
                        }
                        else
                        {
                            lblError.Text = "";
                            UpdateStatus(objSignInSignOutModel);

                            // StartWorkflow
                            if (objSignInSignOutModel.StatusID == 2)
                            {
                                lblSuccess.Text = "Record updated successfully.";
                                UpdateEmployeeLeaveAndComp();

                                int rowEffected;
                                if (WorkflowId != null || WorkflowId.ToString() != "")
                                    rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                                SendMail();
                            }
                            if (objSignInSignOutModel.StatusID == 3)
                            {
                                lblSuccess.Text = "Record updated successfully.";

                                int rowEffected;
                                if (WorkflowId != null || WorkflowId.ToString() != "")
                                    rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                                SendMail();
                            }
                            // End WorkFlow

                            gvSISO.EditIndex = -1;
                            BindSISOData();
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
                        objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "btnupdateselect_click", ex.StackTrace);
                        throw new V2Exceptions(ex.ToString(), ex);
                    }
                }
            }
        }

        protected void ChkSelectLeave_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTest = (CheckBox)sender;
            GridViewRow grdRow = (GridViewRow)chkTest.NamingContainer;
        }

        protected void chkSelectAllLeave_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkAll = (CheckBox)gvLeave.HeaderRow.FindControl("chkSelectAllLeave");
            if (chkAll.Checked == true)
            {
                foreach (GridViewRow gvRow in gvLeave.Rows)
                {
                    CheckBox chkSel = (CheckBox)gvRow.FindControl("ChkSelectLeave");
                    chkSel.Checked = true;
                }
            }
            else
            {
                foreach (GridViewRow gvRow in gvLeave.Rows)
                {
                    CheckBox chkSel = (CheckBox)gvRow.FindControl("ChkSelectLeave");
                    chkSel.Checked = false;
                }
            }
        }

        protected void btnupdateselectLeave_click(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in gvLeave.Rows)
            {
                CheckBox chkSel = (CheckBox)gvRow.FindControl("ChkSelectLeave");
                if (chkSel.Checked == true)
                {
                    try
                    {
                        bool WfApprovedLocked = false, oneLeaveWeekend = false;
                        double TotalLeaves = 0, CorrectionLeaves;
                        int BalanceLeaves, absent;
                        String[] strLeaves, TotalLeavesBalance;
                        DateTime FromDate, ToDate;
                        string strTotalLeaveBalance = string.Empty;
                        WorkflowId = new Guid();

                        Label lblLeaveDetailID = gvRow.FindControl("lblLeaveDetailID") as Label;
                        objLeaveDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text.Trim());
                        objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text.Trim());

                        Label lblLeaveDetailWFID = gvRow.FindControl("lblLeaveDetailWFID") as Label;
                        objLeaveDetailsModel.WorkFlowID = new Guid(lblLeaveDetailWFID.Text.Trim());
                        WorkflowId = new Guid(lblLeaveDetailWFID.Text.Trim()); //For Sending mail

                        Label lblgrvFormDate = gvRow.FindControl("lblgrvFromDate") as Label;
                        objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(lblgrvFormDate.Text.Trim());
                        strFromDate = lblgrvFormDate.Text.Trim();//For Sending mail

                        Label lblgrvToDate = gvRow.FindControl("lblgrvToDate") as Label;
                        objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(lblgrvToDate.Text.Trim());
                        strFromDate = lblgrvToDate.Text.Trim();//For Sending mail

                        Label lblgrvLeaveReason = gvRow.FindControl("lblgrvLeaveReason") as Label;
                        objLeaveDetailsModel.LeaveResason = lblgrvLeaveReason.Text.Trim();

                        DropDownList ddlStatusID = gvRow.FindControl("ddlgrvStatusName") as DropDownList;

                        objLeaveDetailsModel.StatusID = Convert.ToInt32(2);

                        Label lblgrvApproverEdit = gvRow.FindControl("lblgrvApprover") as Label;
                        objLeaveDetailsModel.ApproverID = Convert.ToInt32(lblgrvApproverEdit.Text.ToString());

                        objLeaveDetailsModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                        //Getting Total Leaves
                        Label lblEditUserID = gvRow.FindControl("lblEditUserID1") as Label;
                        objLeaveDetailsModel.UserID = Convert.ToInt32(lblEditUserID.Text.ToString());
                        intUserIDForMail = Convert.ToInt32(lblEditUserID.Text.ToString());// For Sending mail

                        //Getting All the Holidays Details
                        // DataSet dsHolidaysList = objHolidayMasterBOL.bindData();
                        objHolidayModel.UserID = objLeaveDetailsModel.UserID;
                        objHolidayModel.StartDate = objLeaveDetailsModel.LeaveDateFrom;
                        objHolidayModel.EndDate = objLeaveDetailsModel.LeaveDateTo;
                        DataSet dsHolidaysList = objHolidayBOL.bindHolidaysForLeaveApprovals(objHolidayModel);

                        //Check the leaves dates are in SignInSignOut Table
                        DataSet dsCheckInSignIn = objLeaveDetailsBOL.CheckLeavesinSignIn(objLeaveDetailsModel);

                        //For Getting ConfigDate from Database
                        DataSet dsConfigItem = objLeaveDetailsBOL.GetLeaveDetails(objLeaveDetailsModel);
                        DateTime ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                        FromDate = Convert.ToDateTime(lblgrvFormDate.Text.Trim());
                        ToDate = Convert.ToDateTime(lblgrvToDate.Text.Trim());
                        TimeSpan ts = ToDate - FromDate;

                        TotalLeaves = ts.TotalDays + 1;
                        strLeaves = new String[Convert.ToInt32(TotalLeaves)];

                        Label lblTotalLeaves = gvRow.FindControl("lblgrvTotalLeaves") as Label;
                        Label lblTotalAbsentLeaves = gvRow.FindControl("lblgrvTotalAbsentLeaves") as Label;
                        Label lblgrvEditUserName = gvRow.FindControl("lblgrvUserName") as Label;

                        TextBox txtgrvApproverComments = gvRow.FindControl("txtgrvApproverComments") as TextBox;
                        strApproverCommentForMail = "Approved";//For Sending mail

                        objLeaveDetailsModel.ApproverComments = "Approved by :Admin ";

                        string ddlstautsHardCoded = "2";

                        int j = 0, TotalLeavesApplyedFor = 0;
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
                        if (oneLeaveWeekend == true)
                        {
                            lblError.Visible = true;
                            lblError.Text = lblgrvEditUserName.Text + " has  already Signed In for these dates / It is in Holiday List / Sat/Sun, So you can't approve leaves for these dates";
                            lblSuccess.Text = "";
                            gvLeave.EditIndex = -1;
                            BindLeaveData();
                            return;
                        }
                        TotalLeavesApplyedFor = j++;

                        if (TotalLeavesApplyedFor == 0)
                        {
                            lblError.Text = lblgrvEditUserName.Text + " has already Signed In for these dates / It is in Holiday List / Sat/Sun, So you can't approve leaves for these dates";
                            lblSuccess.Text = "";
                            gvLeave.EditIndex = -1;
                            BindLeaveData();
                            return;
                        }

                        if (ddlstautsHardCoded == "2")
                        {
                            //Here delete the all records of related leavedetailsID
                            objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblEditUserID.Text.ToString());
                            objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text.ToString());
                        }

                        //get Total Leaves: Correction done by Anushree Tirwadkar on 9/9/2011

                        if (Convert.ToDateTime(lblgrvToDate.Text.Trim()) < Convert.ToDateTime(DateTime.Now))
                            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(DateTime.Now);
                        else
                            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(lblgrvToDate.Text.Trim());
                        DataSet dsTotalLeaves = objLeaveDetailsBOL.TotalLeaveBalance(objLeaveDetailsModel);
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
                            strLeaveToDate = lblgrvToDate.Text.Trim();
                            UpdateLeaveDetails(strLeaveToDate);
                            lblError.Text = lblgrvEditUserName.Text + " has applied for " + TotalLeavesApplyedFor + " leaves, but his/her Leave balance is " + BalanceLeaves + ", So " + absent + " days will be marked as absent.";
                            lblSuccess.Text = "";

                            for (int k = 0; k < TotalLeavesApplyedFor; k++)
                            {
                                if (strLeaves[k] == null)
                                {
                                    break;
                                }
                                else
                                {
                                    if (ddlstautsHardCoded == Convert.ToString("2"))
                                    {
                                        //Here Adding Leave records to LeaveTransaction table
                                        objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblEditUserID.Text.ToString());
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

                            if (Convert.ToDateTime(lblgrvToDate.Text.Trim()) < Convert.ToDateTime(DateTime.Now))
                                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(DateTime.Now);
                            else
                                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(lblgrvToDate.Text.Trim());

                            UpdateFutureLeaves();
                        }
                        else
                        {
                            objLeaveDetailsModel.TotalLeaveDays = TotalLeavesApplyedFor;
                            objLeaveDetailsModel.LeaveCorrectionDays = Convert.ToInt32(0);
                            strLeaveToDate = lblgrvToDate.Text.Trim();
                            UpdateLeaveDetails(strLeaveToDate);

                            for (int k = 0; k < strLeaves.Length; k++)
                            {
                                if (strLeaves[k] == null)
                                {
                                    break;
                                }
                                else
                                {
                                    if (ddlstautsHardCoded == Convert.ToString("2"))
                                    {
                                        //Here Adding Leave records to LeaveTransaction table
                                        objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblEditUserID.Text.ToString());
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

                            if (Convert.ToDateTime(lblgrvToDate.Text.Trim()) < Convert.ToDateTime(DateTime.Now))
                                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(DateTime.Now);
                            else
                                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(lblgrvToDate.Text.Trim());

                            UpdateFutureLeaves();
                        }

                        if (WfApprovedLocked == false)
                        {
                            if (ddlstautsHardCoded == "2")//Approved
                            {
                                int rowEffected;
                                if (WorkflowId != null || WorkflowId.ToString() != "")
                                    rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                                SendMail();
                            }
                        }
                        else
                        {
                            if (ddlstautsHardCoded == Convert.ToString("2"))
                            {
                                //Sending a Mail to Employee after Updating Leaves
                                if (WfApprovedLocked == true)
                                {
                                    objLeaveDetailsModel.UserID = Convert.ToInt32(lblEditUserID.Text.ToString());
                                    SendingMailToReportingPerson();
                                }
                            }
                        }
                        gvLeave.EditIndex = -1;

                        BindLeaveData();

                        lblSuccess.Visible = true;
                        lblSuccess.Text = "Leave details are updated successfully";
                        lblError.Text = "";
                    }
                    catch (System.Threading.ThreadAbortException ex)
                    {
                        throw ex;
                    }
                    catch (V2Exceptions ex)
                    {
                        throw ex;
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.Message.CompareTo("Already leave applied for this dates.") != 0)
                        {
                            lblError.Visible = false;
                            FileLog objFileLog = FileLog.GetLogger();
                            objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval.aspx.cs", "gvLeave_RowUpdating", ex.StackTrace);
                            throw new V2Exceptions(ex.ToString(), ex);
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "User has already applied for leaves for these dates.";
                            lblSuccess.Text = "";
                        }
                    }
                }
            }
        }

        protected void ChkSelectCompensatory_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTest = (CheckBox)sender;
            GridViewRow grdRow = (GridViewRow)chkTest.NamingContainer;
        }

        protected void chkSelectAllCompensatory_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkAll = (CheckBox)gvCompensatory.HeaderRow.FindControl("chkSelectAllCompensatory");
            if (chkAll.Checked == true)
            {
                foreach (GridViewRow gvRow in gvCompensatory.Rows)
                {
                    CheckBox chkSel = (CheckBox)gvRow.FindControl("ChkSelectCompensatory");
                    chkSel.Checked = true;
                }
            }
            else
            {
                foreach (GridViewRow gvRow in gvCompensatory.Rows)
                {
                    CheckBox chkSel = (CheckBox)gvRow.FindControl("ChkSelectCompensatory");
                    chkSel.Checked = false;
                }
            }
        }

        protected void btnupdateselectCompensatory_click(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in gvCompensatory.Rows)
            {
                CheckBox chkSel = (CheckBox)gvRow.FindControl("ChkSelectCompensatory");
                if (chkSel.Checked == true)
                {
                    {
                        try
                        {
                            bool WfApprovedLocked = false;
                            DateTime FromDate, ConfigDate;
                            int rowsAffected;

                            Label lblTMmember = gvRow.FindControl("lblEditUserIDTemp") as Label;

                            objCompensationDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                            intUserIDForMail = Convert.ToInt32(lblTMmember.Text.ToString());

                            Label lblEditCompensationID = gvRow.FindControl("lblCompensationID") as Label;
                            objCompensationDetailsModel.CompensationID = Convert.ToInt32(lblEditCompensationID.Text);

                            Label lblCompensationWFID = gvRow.FindControl("lblCompensationWFID") as Label;
                            if (lblCompensationWFID.Text != "")
                            {
                                objCompensationDetailsModel.WorkFlowID = new Guid(lblCompensationWFID.Text);
                                WorkflowId = new Guid(lblCompensationWFID.Text.Trim());//For Sending mail
                            }

                            Label lblgrvEditUserName = gvRow.FindControl("lblgrvUserName") as Label;

                            Label txtEditAppliedFor = gvRow.FindControl("lblgrvAppliedFor") as Label;
                            strFromDate = txtEditAppliedFor.Text.Trim();//For Sending mail

                            DropDownList ddlStatusID = gvRow.FindControl("ddlgrvStatusName") as DropDownList;

                            objCompensationDetailsModel.StatusID = Convert.ToInt32(2);

                            Label lblEditgrvReason = gvRow.FindControl("lblgrvReason") as Label;

                            objCompensationDetailsModel.Resason = lblEditgrvReason.Text.Trim();

                            Label ApproverComments = gvRow.FindControl("lblgrvApproverComments") as Label;
                            strApproverCommentForMail = "Approved"; //For Sending mail

                            objCompensationDetailsModel.ApproverComments = "Approved by :Admin ";

                            objCompensationDetailsModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                            objCompensationDetailsModel.CompensationTo = Convert.ToDateTime(txtEditAppliedFor.Text.Trim());

                            objCompensationDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                            intUserIDForMail = Convert.ToInt32(lblTMmember.Text.ToString()); //For Sending mail to user
                            //Getting All the Holidays Details
                            //DataSet dsHolidaysList = objHolidayMasterBOL.bindData();
                            objHolidayModel.UserID = objLeaveDetailsModel.UserID;
                            objHolidayModel.StartDate = objLeaveDetailsModel.LeaveDateFrom;
                            objHolidayModel.EndDate = objLeaveDetailsModel.LeaveDateTo;
                            DataSet dsHolidaysList = objHolidayBOL.bindHolidaysForLeaveApprovals(objHolidayModel);
                            //For Getting ConfigDate from Database
                            DataSet dsConfigItem = objCompensationDetailsBOL.GetCompensationDetails(objCompensationDetailsModel);
                            ConfigDate = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                            FromDate = Convert.ToDateTime(txtEditAppliedFor.Text.Trim());
                            Boolean flag = false;
                            rowsAffected = objCompensationDetailsBOL.CheckSignInForCompensation(objCompensationDetailsModel);

                            string ddlStatusHardCoded = "2";
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
                                    objCompensationDetailsModel.AppliedFor = Convert.ToDateTime(FromDate.ToString());

                                    UpdateCompenstionDetails();
                                    gvCompensatory.EditIndex = -1;
                                    lblError.Visible = true;
                                    lblSuccess.Visible = true;
                                    lblSuccess.Text = "Compensatory Leave details are updated successfully";
                                    lblError.Text = "";

                                    if (ddlStatusHardCoded == Convert.ToString("2"))
                                    {
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
                                    if (ddlStatusHardCoded == Convert.ToString("3"))
                                    {
                                        //Here delete the all records of related CompensationID
                                        objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                                        objLeaveTransDetailsModel.CompensationID = Convert.ToInt32(lblEditCompensationID.Text);
                                        DeleteCompensationTransactionDetails();
                                        UpdateEmployeeLeaveAndComp();

                                        //Here Adding Leave records to Compensation table
                                        objCompensationDetailsModel.AppliedFor = Convert.ToDateTime(FromDate.ToString());
                                        objCompensationDetailsModel.StatusID = Convert.ToInt32(3);
                                        UpdateCompenstionDetails();
                                        lblSuccess.Visible = true;
                                        lblSuccess.Text = "Compensatory Leave Details are Rejected";
                                    }
                                }
                            }
                            else
                            {
                                lblError.Visible = true;
                                lblError.Text = lblgrvEditUserName.Text + " has not signed-in on the day you are approving compensatory off for, Select Valid date";
                                lblSuccess.Text = "";
                            }

                            if (WfApprovedLocked == false)
                            {
                                if (objCompensationDetailsModel.StatusID == 2)//Approved
                                {
                                    int rowEffected;
                                    if (WorkflowId != null || WorkflowId.ToString() != "")
                                        rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                                    SendMail();
                                }
                                if (objCompensationDetailsModel.StatusID == 3)//Rejected
                                {
                                    int rowEffected;
                                    if (WorkflowId != null || WorkflowId.ToString() != "")
                                        rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                                    SendMail();
                                }
                                if (objCompensationDetailsModel.StatusID == 4)//Cancelled
                                {
                                    int rowEffected;
                                    if (WorkflowId != null || WorkflowId.ToString() != "")
                                        rowEffected = objConfigItemBOL.DeleteWorkflowInstanceManually(WorkflowId);
                                    SendMail();
                                }
                            }
                            else
                            {
                                if (objCompensationDetailsModel.StatusID == 2)
                                {
                                    //Sending a Mail to Employee after Updating compensatory Leaves
                                    if (WfApprovedLocked == true)
                                    {
                                        objLeaveDetailsModel.UserID = Convert.ToInt32(lblTMmember.Text.ToString());
                                        SendingMailToReportingPersonForCompOff();
                                    }
                                }
                            }
                            BindCompensatoryData();
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
                                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminApproval", "gvCompensatory_RowUpdating", ex.StackTrace);
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
                }
            }
        }
    }
}