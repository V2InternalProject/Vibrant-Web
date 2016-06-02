using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.Workflow.CompensationWF;
using System.Workflow.Runtime;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Mail;
using System.Text.RegularExpressions;

public partial class CompensationApplicationForm : System.Web.UI.Page
{
    #region Variable declaration
    CompensationDetailsModel objCompensationModel = new CompensationDetailsModel();
    CompensationDetailsBOL objCompensationBOL = new CompensationDetailsBOL();
    LeaveDetailsBOL objLeaveDeatilsBOL = new LeaveDetailsBOL();
    LeaveDetailsModel objLeaveDeatilsModel = new LeaveDetailsModel();
    HolidayMasterBOL objHolidayBOL = new HolidayMasterBOL();
    HolidayMasterModel objHolidayModel = new HolidayMasterModel();
    LeaveTransactionBOL objLeaveTransDetailsBOL = new LeaveTransactionBOL();
    LeaveTransactionModel objLeaveTransDetailsModel = new LeaveTransactionModel();

    DataSet dsGetStatus, dsCompensation, dsReportingTo, dsHolidaysList, dsSearchDetails, dsSorting, dsConfigItem, dsCancelDetails;
    string UserID = string.Empty;
    DateTime FromDate, ConfigDate;
    double TotalLeaves = 0, count = 0;
    int j = 0, TotalLeavesApplyedFor = 0;
    double CorrectionLeaves, BalanceLeaves;
    String[] strLeaves;
    String[] strHoliDays;
    int rowsAffected, EmploymentStatus;
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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
            throw new V2Exceptions();
        }
    }
    #endregion

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
            throw new V2Exceptions();
        }
    }
    #endregion

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
            //if (dsCompensation.Tables[4].Rows[0]["CompensationLeaveBalance"].ToString() == "")
            //{
            //    lblAvailableLeaves.Text = Convert.ToString("0");
            //}
            //else
            //{
            //    lblAvailableLeaves.Text = Convert.ToString(dsCompensation.Tables[4].Rows[0]["CompensationLeaveBalance"].ToString());                
            //}

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
            throw new V2Exceptions();
        }

    }
    #endregion

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
                throw new V2Exceptions();
            }
            else
            {
                throw ex;
            }
        }

    }
    #endregion

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
                throw new V2Exceptions();
            }
            else
            {
                throw ex;
            }
        }

    }
    #endregion

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
                throw new V2Exceptions();
            }
            else
            {
                throw ex;
            }
        }
    }
    #endregion

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
                throw new V2Exceptions();
            }
            else
            {
                throw ex;
            }
        }
    }
    #endregion

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
            throw new V2Exceptions();
        }

    }
    #endregion

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
            //if (dtAppliedFor < dtCheckDate)
            //{
            //    lblError.Text = "Compensatory leave can be applied with in 60 days";
            //    return;
            //}

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
                lblError.Text = "your Reporting To is not set. Please set it to appropriate person";
                return;
                // Prvious code	
                //objCompensationModel.ApproverID = Convert.ToInt32("");

            }

            //Getting All the Holidays Details
            dsHolidaysList = objHolidayBOL.bindData();

            FromDate = Convert.ToDateTime(txtAppliedFor.Text.Trim());
            Boolean flag = false;

            objCompensationModel.CompensationTo = Convert.ToDateTime(txtAppliedFor.Text.Trim());

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

                DataSet dsweeklyOff = new DataSet();
                dsweeklyOff = objCompensationBOL.GetWeeklyOff(objCompensationModel);
                bool flagWeekOff = false;
                for (int i = 0; i < dsweeklyOff.Tables[0].Rows.Count; i++)
                {
                    DateTime weekoff1 = Convert.ToDateTime(dsweeklyOff.Tables[0].Rows[i]["Weekoff1"]);
                    DateTime weekoff2 = Convert.ToDateTime(dsweeklyOff.Tables[0].Rows[i]["Weekoff2"]);

                    if (FromDate.ToString("MM/dd/yy") == weekoff1.ToString("MM/dd/yy") || FromDate.ToString("MM/dd/yy") == weekoff2.ToString("MM/dd/yy"))
                    {
                        flagWeekOff = true;
                        break;
                    }

                }

                if (flag)
                {
                    #region Config Date Check Code I
                    //if (ConfigDate.Date >= Convert.ToDateTime(txtAppliedFor.Text).Date)
                    //{
                    //    //lblError.Text = "You are selected " + txtAppliedFor.Text + " . It was freezed.Please Select another date.";
                    //    lblError.Text = "Administrator has frozen the data for the selected date";
                    //    txtAppliedFor.Text = "";
                    //    txtReason.Text = "";
                    //    return;
                    //}
                    //else
                    //{ 
                    #endregion

                    objCompensationModel.AppliedFor = Convert.ToDateTime(FromDate.ToString());
                    AddCompenstionDetails();
                    lblSuccess.Text = "Compensatory Leave successfully submitted";

                    #region Config Date Check Code II
                    //} 
                    #endregion
                }
                else
                {
                    if (flagWeekOff)
                    {
                        objCompensationModel.AppliedFor = Convert.ToDateTime(FromDate.ToString());
                        AddCompenstionDetails();
                        lblSuccess.Text = "Compensatory Leave successfully submitted";
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Compensatory leave cannot be applied for a working day";
                    }

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
                throw new V2Exceptions();
            }
            else
            {
                lblError.Visible = true;
                //lblError.Text = ex.Message;
                lblError.Text = "You have already applied for compensatory leave for this date";
            }
        }

    }
    #endregion

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
            throw new V2Exceptions();
        }
    }
    #endregion

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
            throw new V2Exceptions();
        }
    }
    #endregion

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
            dsHolidaysList = objHolidayBOL.bindData();

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
            //Here Updating LeaveBalance at EmployeeMaster table
            //UpdateLeaveBalance();
            //BindData();


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
                throw new V2Exceptions();
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "You have already applied for compensatory leave for this date";
            }
        }

    }
    #endregion

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
            throw new V2Exceptions();
        }
    }
    #endregion

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
                        //lblApproved.Visible = true;
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
                        //lblApproved.Visible = true;
                        //lblApproved.Text = "Aprroved";

                        lnkEdit.Visible = false;
                        //lnkCancel.Visible = false;

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
            throw new V2Exceptions();
        }
    }
    #endregion

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
            throw new V2Exceptions();
        }
    }
    #endregion

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
            throw new V2Exceptions();
        }
    }
    #endregion

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
            //ddlStatus.SelectedValue = Convert.ToString("0");

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
            throw new V2Exceptions();
        }
    }
    #endregion

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
            //BindData();
            //ddlStatus.SelectedValue = Convert.ToString("0");
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
            throw new V2Exceptions();
        }
    }
    #endregion

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
                //tdSearch.Visible = true;
                //tdAddLeave.Visible = false;
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
            throw new V2Exceptions();
        }

    }
    #endregion

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
                //tdSearch.Visible = true;
                //tdAddLeave.Visible = false;
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
            throw new V2Exceptions();
        }
    }
    #endregion

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
            throw new V2Exceptions();
        }
    }
    #endregion

    #region Sorting
    protected void gvCampensation_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            objCompensationModel.UserID = Convert.ToInt32(UserID);
            objCompensationModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
            //dsSorting = objCompensationBOL.GetCompensationDetails(objCompensationModel);
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
            throw new V2Exceptions();
        }
    }
    #endregion

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
            throw new V2Exceptions();
        }
    }
    #endregion

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

                objMailMessage.To = dsReportingTo.Tables[0].Rows[0]["ReporterMailID"].ToString();

                string UserName, ApproverName, Reason, AppliedFor;

                //string strBody;
                dsCancelDetails = objCompensationBOL.GetCancelCompOffDetails(objCompensationModel);

                UserName = dsReportingTo.Tables[0].Rows[0]["EmployeeName"].ToString();
                ApproverName = dsReportingTo.Tables[0].Rows[0]["ReporterName"].ToString();
                objMailMessage.To = dsReportingTo.Tables[0].Rows[0]["ReporterMailID"].ToString();
                objMailMessage.Cc = dsReportingTo.Tables[0].Rows[0]["CompetencyMailID"].ToString();
                AppliedFor = Convert.ToDateTime(dsCancelDetails.Tables[0].Rows[0]["AppliedFor"].ToString()).ToShortDateString();

                Reason = dsCancelDetails.Tables[0].Rows[0]["Reason"].ToString();

                for (int k = 0; k < dsReportingTo.Tables[1].Rows.Count; k++)
                {
                    if (dsReportingTo.Tables[1].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                    {

                        objMailMessage.From = dsReportingTo.Tables[1].Rows[k]["ConfigItemValue"].ToString();

                    }
                    if (dsReportingTo.Tables[1].Rows[k]["ConfigItemName"].ToString() == "Mail Server Name")
                    {

                        SmtpMail.SmtpServer = dsReportingTo.Tables[1].Rows[k]["ConfigItemValue"].ToString();
                        break;

                    }
                }

                objMailMessage.Subject = "Cancelling Compensatory Leave";
                objMailMessage.BodyFormat = MailFormat.Html;

                objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi " + ApproverName + " ," + "<br>" + "<br>" + " Compensatory Leave Application Details: " + "<br>" + "<br>" + "User Name: " + "<b>" + UserName + "</b>" + " <br> " + "AppliedFor: " + AppliedFor + " <br> " + " Reason: " + Reason + " <br> " + " <br> " + " Cancelled the Approved Compensatory Leave Details, the required updates are made in the system.";

                /* strBody = "\n" + "Hi ##ApproverName##," + " \n\n" + " Compensatory Leave Application Details: " + " \n\n" + "  Applied For: ##AppliedFor##" + " \n" + "  Reason: ##Reason##" + "\n\n" + "Cancelled the Approved Compensatory Leave Details, the required updates are made in the system.";
                 strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);
                 strBody = Regex.Replace(strBody, "##ApproverName##", dsReportingTo.Tables[0].Rows[0]["ReporterName"].ToString());
                 strBody = Regex.Replace(strBody, "##EmployeeName##", dsReportingTo.Tables[0].Rows[0]["EmployeeName"].ToString());
                 strBody = Regex.Replace(strBody, "##AppliedFor##", Convert.ToDateTime(dsCancelDetails.Tables[0].Rows[0]["AppliedFor"].ToString()).ToShortDateString());
                 strBody = Regex.Replace(strBody, "##Reason##", dsCancelDetails.Tables[0].Rows[0]["Reason"].ToString());

                 //objMailMessage.Body = strBody;    */

                objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "0");
                objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "v2system");
                objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "mail_123");


                SmtpMail.Send(objMailMessage);

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
            throw new V2Exceptions();
        }
    }
    #endregion

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
            throw new V2Exceptions();
        }

    }
    #endregion

}
