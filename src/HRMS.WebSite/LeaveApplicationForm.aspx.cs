using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.Workflow.LeaveDetailsWF;
using System.Drawing;
using System.Web.Mail;
using System.Text.RegularExpressions;


public partial class LeaveApplicationForm : System.Web.UI.Page
{
    #region Variables Declaration
    LeaveDetailsBOL objLeaveDeatilsBOL = new LeaveDetailsBOL();
    LeaveDetailsModel objLeaveDetailsModel = new LeaveDetailsModel();
    LeaveTransactionBOL objLeaveTransDetailsBOL = new LeaveTransactionBOL();
    LeaveTransactionModel objLeaveTransDetailsModel = new LeaveTransactionModel();
    HolidayMasterModel objHolidayModel = new HolidayMasterModel();
    HolidayMasterBOL objHolidayBOL = new HolidayMasterBOL();
    RosterPlanningBOL ObjRosterPlanningBOL = new RosterPlanningBOL();
    RosterPlanningModel objRosterPlanningModel = new RosterPlanningModel();
    DataSet dsLeave, dsHolidaysList;
    DataSet dsGetLeaveStatus, dsReportingTo, dsSearchLeaveDetails, dsTotalLeaves, dsLeaveBalance, dsAllDetails, dsSorting, dsConfigItem, dsCheckInSignIn, dsCancelDetails, dsEmployeeShiftDetail, dsEmployeeRole;
    DateTime FromDate, ToDate;
    double TotalLeaves = 0;
    int j = 0, TotalLeavesApplyedFor = 0;
    double CorrectionLeaves;
    int BalanceLeaves, absent;
    String[] strLeaves, TotalLeavesBalance;
    bool oneLeaveWeekend = false;
    string LoginID = string.Empty;
    bool isShiftEmployee = false;
    int rowsAffected;

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";
            lblSuccess.Text = "";
            LoginID = User.Identity.Name.ToString();
            if (!IsPostBack)
            {
                FillddlLeaveStatus();
                BindData();
                tdAddLeave.Visible = true;
                spanAddLeave.Visible = true;
                spanSearch.Visible = false;
                spanEdit.Visible = false;
                tdSearch.Visible = false;
                lnkAddLeaves.Visible = false;
                lnkLeavePolicy.Visible = false;
                //searchDetails.Visible = false;
                txtSearchFromDate.Visible = false;
                txtSearchToDate.Visible = false;

            }
            objHolidayModel.Year = DateTime.Now.Year;
            objHolidayModel.UserID = Convert.ToInt32(User.Identity.Name);
            dsHolidaysList = objHolidayBOL.searchHolidayList(objHolidayModel);
            for (int l = 0; l < dsHolidaysList.Tables[1].Rows.Count; l++)
            {
                //TableRow tr = new TableRow();
                //TableCell tc=new TableCell();
                //tc.Text=dsHolidaysList.Tables[1].Rows[l]["Holidays"].ToString();
                //tr.Cells.Add(tc);
                //HolidayTable.Rows.Add(tr);

                TableRow tr = new TableRow();
                TableCell tc = new TableCell();
                TableCell tc1 = new TableCell();
                TableCell tc2 = new TableCell();

                tc.Text = dsHolidaysList.Tables[2].Rows[l]["Holiday"].ToString();
                tr.Cells.Add(tc);
                tc2.Text = " ";
                tr.Cells.Add(tc2);
                tc1.Text = dsHolidaysList.Tables[2].Rows[l]["Holidaydescription"].ToString();
                tr.Cells.Add(tc1);

                HolidayTable.Rows.Add(tr);
            }
            mainTab_Selected.Value = "Leave";

            txtFromDate.Attributes.Add("onkeydown", "return false");
            txtToDate.Attributes.Add("onkeydown", "return false");
            txtSearchFromDate.Attributes.Add("onkeydown", "return false");
            txtSearchToDate.Attributes.Add("onkeydown", "return false");
            //selected_tab.Value = Request.Form[selected_tab.UniqueID];
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "Page_Load", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region AddLeaveDetails
    public void AddLeaveDetails()
    {
        try
        {
        Guid gLeaveDetailsWFID = new Guid("00000000-0000-0000-0000-000000000000");
        int LeavedetailId = 0;
        
            SqlDataReader dr = objLeaveDeatilsBOL.AddLeaveDetails(objLeaveDetailsModel);
            while (dr.Read())
            {
                gLeaveDetailsWFID = new Guid(dr[1].ToString());
                LeavedetailId = Convert.ToInt32(dr[0].ToString());

            }
            if (gLeaveDetailsWFID != null || LeavedetailId != 0)
                StartWorkflow(LeavedetailId, gLeaveDetailsWFID);

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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "AddLeaveDetails", ex.StackTrace);
                throw new V2Exceptions();
            }
            else
            {

                throw ex;
            }
        }

    }
    public void StartWorkflow(int LeaveDetails, Guid gLeaveDetailsWFID)
    {
        try
        {
            WorkflowRuntime wr = (WorkflowRuntime)Application["WokflowRuntime"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("LeaveDetailID", LeaveDetails);
            WorkflowInstance wi = wr.CreateWorkflow(typeof(LeaveDetailsWF.LeaveDetailsWF), parameters, gLeaveDetailsWFID);
            wi.Start();
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {

            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "StartWorkflow", ex.StackTrace);
            throw new V2Exceptions();

        }


    }

    #endregion

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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "AddLeaveTransDetails", ex.StackTrace);
                throw new V2Exceptions();
            }
            else
            {
                throw ex;
            }
        }

    }
    #endregion

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
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "DeleteLeaveTransDetails", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region Submit
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
            objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtFromDate.Text.Trim());
            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtToDate.Text.Trim());
            objLeaveDetailsModel.LeaveResason = txtReason.Text.ToString();
            objLeaveDetailsModel.StatusID = Convert.ToInt32(1);
            objLeaveDetailsModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            dsReportingTo = objLeaveDeatilsBOL.GetReportingTo(objLeaveDetailsModel);
            objLeaveDetailsModel.ApproverComments = "";
            dsConfigItem = objLeaveDeatilsBOL.GetLeaveDetails(objLeaveDetailsModel);
            DateTime ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

            if (dsReportingTo.Tables[0].Rows.Count > 0)
            {
                objLeaveDetailsModel.ApproverID = Convert.ToInt32(dsReportingTo.Tables[0].Rows[0]["ReporterID"].ToString());
            }
            else
            {
		        //If Reporting to is not assigned 
		          lblError.Visible = true;
                  lblError.Text = "your Reporting To is not set. Please set it to appropriate person";
                  return;
     	       // Prvious code	
               // objLeaveDetailsModel.ApproverID = Convert.ToInt32("");
            }

            //Check the leaves dates are in SignInSignOut Table
            dsCheckInSignIn = objLeaveDeatilsBOL.CheckLeavesinSignIn(objLeaveDetailsModel);

            //Getting All the Holidays Details
            dsHolidaysList = objHolidayBOL.bindData();            
            FromDate = Convert.ToDateTime(txtFromDate.Text.Trim());
            ToDate = Convert.ToDateTime(txtToDate.Text.Trim());
            TimeSpan ts = ToDate - FromDate;

            TotalLeaves = ts.TotalDays + 1;
            strLeaves = new String[Convert.ToInt32(TotalLeaves)];

	        SignInSignOutModel objSignInSignOutModel = new SignInSignOutModel();
		    objSignInSignOutModel.EmployeeID = Convert.ToInt32(HttpContext.Current.User.Identity.Name.ToString());
           //objSignInSignOutModel.EmployeeID = Convert.ToInt32(User.Identity.Name);
		     //objSignInSignOutModel.EmployeeID = 332;
            ManualSignInSignOutBOL objManualSignInSignOutBOL = new ManualSignInSignOutBOL();
            DataSet dsEmployeeJoiningDate = new DataSet();
            DateTime JoiningDate; 
            dsEmployeeJoiningDate = objManualSignInSignOutBOL.GetEmployeeJoiningData(objSignInSignOutModel);
            JoiningDate = Convert.ToDateTime(dsEmployeeJoiningDate.Tables[0].Rows[0]["DateOfJoining"]);

            if (FromDate < JoiningDate)
            {
                lblError.Visible = true;
                lblError.Text = "You are applying for Leave before your Joining Date. Enter Valid Date ";
                return;
            
            }
            objRosterPlanningModel.UserId = Convert.ToInt32(HttpContext.Current.User.Identity.Name.ToString());
            dsEmployeeRole = ObjRosterPlanningBOL.GetEmployeeRole(objRosterPlanningModel);
            foreach (DataRow row in dsEmployeeRole.Tables[0].Rows)
            {
                if (row["RoleName"].ToString().Equals("Shift"))
                {
                    isShiftEmployee = true;
                    break;
                }
            }
            //if Not ShiftEmployee
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
            //if Shift employee
            else
            {
                objRosterPlanningModel.FromDate = Convert.ToDateTime(txtFromDate.Text.Trim());
                objRosterPlanningModel.ToDate = Convert.ToDateTime(txtToDate.Text.Trim());
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
                    if (onweeklyoff==true)
                    {
                        
                            if (TotalLeaves == 1)
                            {
                                oneLeaveWeekend = true;
                                break;
                            }
                        
                            if (TotalLeaves == 2)
                            {
                                if (Convert.ToDateTime(dsEmployeeShiftDetail.Tables[0].Rows[0]["WeekOff1"].ToString()) == Convert.ToDateTime(txtFromDate.Text.Trim()) && (Convert.ToDateTime(dsEmployeeShiftDetail.Tables[0].Rows[0]["WeekOff2"].ToString()) == Convert.ToDateTime(txtToDate.Text.Trim())))
                                    {   
                                        oneLeaveWeekend = true;
                                        break;                                           
                                     }
                            }
                            
                    }
                    else
                    {
                        Boolean flag = false;
                        foreach (DataRow row in dsEmployeeShiftDetail.Tables[1].Rows)
                        {
                            //if (row["isHolidayForShift"].ToString() == "0")
                            //{
                            //    row.Delete();
                            //    dsHolidaysList.Tables[0].AcceptChanges();
                            //}
                        }
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
                ///////////////
            }
            if (oneLeaveWeekend == true)
            {
                lblError.Visible = true;
                lblError.Text = "You have  already Signed In for these dates / It is in Holiday List / Weekly Off, So you can't apply leaves for these dates";
                return;
            }

            TotalLeavesApplyedFor = j++;

            if (TotalLeavesApplyedFor == 0)
            {
                lblError.Text = "You have  already Signed In for these dates / It is in Holiday List / Weekly Off, So you can't apply leaves for these dates";
                return;
            }
            //Getting Total Leaves
            // Done changes 
            //objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtToDate.Text.Trim());
            if (Convert.ToDateTime(txtToDate.Text.Trim()) < Convert.ToDateTime(DateTime.Now))
                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(DateTime.Now);
            else
                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtToDate.Text.Trim());
            dsTotalLeaves = objLeaveDeatilsBOL.TotalLeaveBalance(objLeaveDetailsModel);
            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtToDate.Text.Trim());
            if (dsTotalLeaves.Tables[0].Rows[0]["Leave_Balance"].ToString() == "")
            {
                lblAvailableLeaves.Text = Convert.ToString("0");
            }
            else
            {
                lblAvailableLeaves.Text = Convert.ToString(dsTotalLeaves.Tables[0].Rows[0]["Leave_Balance"].ToString());
                string Error = Convert.ToString(lblAvailableLeaves.Text.ToString().StartsWith("-"));
                if (Error.ToString() == "True")
                {
                    lblAvailableLeaves.Text = "0";
                }
            }

            TotalLeavesBalance = lblAvailableLeaves.Text.Split('.');
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

                if (ConfigdateTime.Date >= Convert.ToDateTime(txtFromDate.Text).Date)
                {
                    lblError.Text = "Administrator has frozen the data for the selected date";
                    return;
                }
                else
                {

                    lblError.Text = "You have applied for " + TotalLeavesApplyedFor + " leaves, but your Leave balance is " + BalanceLeaves + ", So " + absent + " days will be marked as absent.";                    
                    AddLeaveDetails();
                }
            }
            else
            {
                if (ConfigdateTime.Date >= Convert.ToDateTime(txtToDate.Text).Date)
                {
                    lblError.Text = "Administrator has frozen the data for the selected date";
                    return;
                }
                else
                {

                    objLeaveDetailsModel.TotalLeaveDays = Convert.ToInt32(TotalLeavesApplyedFor);
                    objLeaveDetailsModel.LeaveCorrectionDays = Convert.ToInt32(0);
                    AddLeaveDetails();
                }

            }

            lblSuccess.Text = "Leave application successfully submitted";
            BindData();
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtReason.Text = "";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "btnSubmit", ex.StackTrace);
                throw new V2Exceptions();
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "User has already applied for leaves for these dates.";
            }
        }
    }
    #endregion

    #region BindData
    public void BindData()
    {
        try
        {
        dsLeave = new DataSet();
        objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
        
            dsLeaveBalance = objLeaveDeatilsBOL.GetLeaveBalance(objLeaveDetailsModel);

            if (dsLeaveBalance.Tables[0].Rows[0]["Leave_Balance"].ToString() == "")
            {
                lblAvailableLeaves.Text = Convert.ToString("0");
            }
            else
            {
                lblAvailableLeaves.Text = Convert.ToString(dsLeaveBalance.Tables[0].Rows[0]["Leave_Balance"].ToString());
                string Error = Convert.ToString(lblAvailableLeaves.Text.ToString().StartsWith("-"));
                if (Error.ToString() == "True")
                {
                    lblAvailableLeaves.Text = "0";
                }
            }

            dsLeave = objLeaveDeatilsBOL.GetLeaveDetails(objLeaveDetailsModel);
            if (dsLeave.Tables[0].Rows.Count > 0)
            {
                gvLeaveApplication.DataSource = dsLeave.Tables[0];
                gvLeaveApplication.DataBind();
            }
            else if (dsLeave.Tables[0].Rows.Count == 0)
            {
                gvLeaveApplication.DataSource = dsLeave;
                gvLeaveApplication.DataBind();
                lblSuccess.Visible = true;
                lblSuccess.Text = "No Leave details.";
            }
            else
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "No Leave details.";
            }
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "BindData", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region GetLeaveStatus
    public void FillddlLeaveStatus()
    {
        try
        {
            dsGetLeaveStatus = objLeaveDeatilsBOL.GetStatusDetails();

            for (int i = 0; i < dsGetLeaveStatus.Tables[0].Rows.Count; i++)
            {
                ddlStatus.Items.Add(new ListItem(dsGetLeaveStatus.Tables[0].Rows[i]["StatusName"].ToString(), dsGetLeaveStatus.Tables[0].Rows[i]["StatusID"].ToString()));
            }

        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "FillddlLeaveStatus", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region UpdateLeaveDetails
    public void UpdateLeaveDetails()
    {
        try
        {
            rowsAffected = objLeaveDeatilsBOL.UpdateLeaveDetails(objLeaveDetailsModel);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "UpdateLeaveDetails", ex.StackTrace);
                throw new V2Exceptions();
            }
            else
            {
                throw ex;
            }
        }
    }
    #endregion

    #region UpdateLeaveBalance
    public void UpdateLeaveBalance()
    {
        try
        {
            rowsAffected = objLeaveDeatilsBOL.UpdateLeaveBalance(objLeaveDetailsModel);
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx", "UpdateLeaveBalance", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region UpdateCancelLeaveDetails
    public void UpdateCancelLeaveDetails()
    {
        try
        {
            rowsAffected = objLeaveDeatilsBOL.UpdateCancelLeaveDetails(objLeaveDetailsModel);
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx", "UpdateCancelLeaveDetails", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

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
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "UpdateLeaveTransDetails", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region RowEditing
    protected void gvLeaveApplication_RowEditing(object sender, GridViewEditEventArgs e)
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

            gvLeaveApplication.EditIndex = e.NewEditIndex;

            LinkButton lbnCancel = ((LinkButton)(gvLeaveApplication.Rows[e.NewEditIndex].FindControl("lnkButCancel")));
            lbnCancel.Visible = false;
            BindData();

            if (txtSearchFromDate.Text != "" && txtSearchFromDate.Text != "")
            {
                objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
                objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                SearchLeaveDetails();
            }
            else
            {
                objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
                objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                dsSearchLeaveDetails = objLeaveDeatilsBOL.SearchAllLeaveDetails(objLeaveDetailsModel);
                if (dsSearchLeaveDetails.Tables[0].Rows.Count <= 0)
                {
                    lblError.Text = "Records Not Found";
                    gvLeaveApplication.Visible = false;
                }
                else
                {
                    lblError.Text = "";
                    gvLeaveApplication.Visible = true;
                    gvLeaveApplication.DataSource = dsSearchLeaveDetails.Tables[0];
                    gvLeaveApplication.DataBind();
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
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "gvLeaveApplication_RowEditing", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region Search
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
            objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
            objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
            SearchLeaveDetails();
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
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "btnSearch_Click", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region SearchLeaveDetails
    public void SearchLeaveDetails()
    {
        try
        {
            dsSearchLeaveDetails = objLeaveDeatilsBOL.SearchLeaveDetails(objLeaveDetailsModel);
            if (dsSearchLeaveDetails.Tables[0].Rows.Count <= 0)
            {
                lblError.Text = "Records Not Found";
                gvLeaveApplication.Visible = false;
            }
            else
            {
                lblError.Text = "";
                gvLeaveApplication.Visible = true;
                gvLeaveApplication.DataSource = dsSearchLeaveDetails.Tables[0];
                gvLeaveApplication.DataBind();
            }
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "SearchLeaveDetails", ex.StackTrace);
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
            tdAddLeave.Visible = false;
            gvLeaveApplication.Visible = true;
            lblSuccess.Text = "";
            lblError.Text = "";
            spanSearch.Visible = true;
            spanAddLeave.Visible = false;
            spanEdit.Visible = false;
            gvLeaveApplication.EditIndex = -1;
            BindData();
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtReason.Text = "";


            txtFromDate.Visible = false;
            txtToDate.Visible = false;
            txtReason.Visible = false;

            PanelAddDetails.Visible = false;
            PanelSearchDetails.Visible = true;

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
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "lnkSearch_Click", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region Addleaves Link
    protected void lnkAddLeaves_Click(object sender, EventArgs e)
    {
        try
        {
            lnkAddLeaves.Visible = false;
            lnkSearch.Visible = true;
            tdSearch.Visible = false;
            tdAddLeave.Visible = true;
            gvLeaveApplication.Visible = true;
            lblSuccess.Text = "";
            lblError.Text = "";
            spanSearch.Visible = false;
            spanEdit.Visible = false;
            spanAddLeave.Visible = true;
            gvLeaveApplication.EditIndex = -1;
            ddlStatus.SelectedValue = Convert.ToString("0");
            BindData();

            txtSearchFromDate.Text = "";
            txtSearchToDate.Text = "";


            txtFromDate.Visible = true;
            txtToDate.Visible = true;
            txtReason.Visible = true;

            PanelAddDetails.Visible = true;
            PanelSearchDetails.Visible = false;

            txtSearchFromDate.Visible = false;
            txtSearchToDate.Visible = false;

            selected_tab.Value = "Add";

        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "lnkAddLeaves_Click.aspx.cs", "lnkSearch_Click", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region RowDatabound
    protected void gvLeaveApplication_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblstatus = ((Label)(e.Row.FindControl("lblgrvStatusName")));
                Label lblgrvFromDate = ((Label)(e.Row.FindControl("lblgrvFromDate")));
				TextBox txtgrvFormDate = ((TextBox)(e.Row.FindControl("txtgrvFormDate")));
                Label lblgrvToDate = ((Label)(e.Row.FindControl("lblgrvToDate")));
                Label lblApproved = ((Label)(e.Row.FindControl("lblApprove")));
                LinkButton lnkEdit = ((LinkButton)(e.Row.FindControl("lnkbutEdit")));
                LinkButton lnkCancel = ((LinkButton)(e.Row.FindControl("lnkButCancel")));

                objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
                dsConfigItem = objLeaveDeatilsBOL.GetLeaveDetails(objLeaveDetailsModel);
                DateTime ConfigDate = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

				// if Application start date is less than or equal to Todays date then disable the link to edit or cancel.
				DateTime dtAppFromDate;
                if(lblgrvFromDate != null)
                    dtAppFromDate =Convert.ToDateTime(lblgrvFromDate.Text).Date;
                else
                    dtAppFromDate = Convert.ToDateTime(txtgrvFormDate.Text).Date;
                if ((dtAppFromDate <= DateTime.Now) || (ConfigDate.Date >= Convert.ToDateTime(lblgrvToDate.Text).Date))
                //if ( ConfigDate.Date >= Convert.ToDateTime(lblgrvToDate.Text).Date)
                {
                    if (lblstatus.Text == "Approved")
                    {
                        lnkCancel.Visible = false;
                        lblApproved.Text = "Leave Approved"; 
                        lnkEdit.Visible = false;                    
                    }
                    else if (lblstatus.Text == "Cancelled")
                    {
                        lblApproved.Visible = true;
                        lblApproved.Text = "Leave Cancelled"; 
                        lnkEdit.Visible = false;
                        lnkCancel.Visible = false;

                    }
                    else if (lblstatus.Text == "Rejected")
                    {
                        lblApproved.Visible = true;
                        lblApproved.Text = "Leave Rejected";                       
                        lnkEdit.Visible = false;
                        lnkCancel.Visible = false;

                    }
                    else if (lblstatus.Text == "Pending")
                    {
                        lblApproved.Visible = true;
                        lblApproved.Text = "Leave Pending";                      
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
                        lblApproved.Text = "Leave Cancelled";                       
                        lnkEdit.Visible = false;
                        lnkCancel.Visible = false;

                    }
                    else if (lblstatus.Text == "Rejected")
                    {
                        lblApproved.Visible = true;
                        lblApproved.Text = "Leave Rejected";                       
                        lnkEdit.Visible = false;
                        lnkCancel.Visible = false;

                    }
                }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
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
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "gvLeaveApplication_RowDataBound", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region Reset
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtReason.Text = "";
        }
         catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "btnReset_Click", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region RowCommand
    protected void gvLeaveApplication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtReason.Text = "";

            if (e.CommandName == "LeaveCancel")
            {
                gvLeaveApplication.EditIndex = -1;

                Label lblStatusName = new Label();
                for (int i = 0; i < gvLeaveApplication.Rows.Count; i++)
                {
                    Label lblId = (Label)((gvLeaveApplication.Rows[i].FindControl("lblLeaveDetailID")));
                    if (lblId.Text == e.CommandArgument.ToString())
                    {
                        lblStatusName = (Label)((gvLeaveApplication.Rows[i].FindControl("lblgrvStatusName")));
                        break;
                    }
                }

                objLeaveDetailsModel.LeaveDetailsID = Convert.ToInt32(e.CommandArgument);
                objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
                
                if (lblStatusName.Text == "Approved")
                {
                    //Here delete the all records of related leavedetailsID
                    objLeaveTransDetailsModel.UserID = Convert.ToInt32(LoginID.ToString());
                    objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(e.CommandArgument);
                    DeleteLeaveTransDetails();                   
                    UpdateEmployeeLeaveAndComp();

                    //Sending a Mail to Reporting person
                    objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID.ToString());
                    SendingMailToReportingPerson();
                }

                

                objLeaveDetailsModel.StatusID = Convert.ToInt32(4);
                objLeaveDetailsModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());


                UpdateCancelLeaveDetails();
                BindData();


                //Label lblStatusName = (Label)((gvLeaveApplication.Rows[gvLeaveApplication.SelectedIndex].FindControl("lblgrvStatusName")));

                lblError.Visible = false;
                lblSuccess.Visible = true;
                lblSuccess.Text = "Leave details are Cancelled successfully";

            }

            if (e.CommandName == "lnkCancel")
            {
                gvLeaveApplication.EditIndex = -1;
                BindData();

                if (txtSearchFromDate.Text != "" && txtSearchFromDate.Text != "")
                {

                    objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
                    objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    gvLeaveApplication.EditIndex = -1;
                    SearchLeaveDetails();

                }
                else
                {
                    //gvLeaveApplication.EditIndex = -1;
                    //BindData();
                    objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
                    objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    dsSearchLeaveDetails = objLeaveDeatilsBOL.SearchAllLeaveDetails(objLeaveDetailsModel);
                    if (dsSearchLeaveDetails.Tables[0].Rows.Count <= 0)
                    {
                        lblError.Text = "Records Not Found";
                        gvLeaveApplication.Visible = false;
                    }
                    else
                    {
                        lblError.Text = "";
                        gvLeaveApplication.Visible = true;
                        gvLeaveApplication.DataSource = dsSearchLeaveDetails.Tables[0];
                        gvLeaveApplication.DataBind();
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
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "gvLeaveApplication_RowCommand", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region RowCancelingEdit
    protected void gvLeaveApplication_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
        gvLeaveApplication.EditIndex = -1;
        BindData();
        }
        
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "gvLeaveApplication_RowCancelingEdit", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region RowUpdating
    protected void gvLeaveApplication_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtReason.Text = "";

            GridViewRow row = gvLeaveApplication.Rows[e.RowIndex];

            objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);

            Label lblLeaveDetailID = row.FindControl("lblLeaveDetailID1") as Label;
            objLeaveDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text);

            TextBox txtgrvFormDate = row.FindControl("txtgrvFormDate") as TextBox;
            objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtgrvFormDate.Text.Trim());

            TextBox txtgrvToDate = row.FindControl("txtgrvToDate") as TextBox;
            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtgrvToDate.Text.Trim());

            TextBox txtgrvLeaveReason = row.FindControl("txtgrvLeaveReason") as TextBox;
            objLeaveDetailsModel.LeaveResason = txtgrvLeaveReason.Text.Trim();

            Label lblgrvStatusName = row.FindControl("lblgrvStatusName") as Label;
            objLeaveDetailsModel.ApproverComments = Convert.ToString(lblgrvStatusName.Text);

            Label lblApproverComments = row.FindControl("lblApproverComments") as Label;
            objLeaveDetailsModel.ApproverComments = Convert.ToString(lblApproverComments.Text);

            Label lblgrvStatusID = row.FindControl("lblgrvStatusID") as Label;
            objLeaveDetailsModel.StatusID = Convert.ToInt32(lblgrvStatusID.Text);

            dsConfigItem = objLeaveDeatilsBOL.GetLeaveDetails(objLeaveDetailsModel);
            DateTime ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

            if (lblgrvStatusName.Text == "Rejected")
            {
                objLeaveDetailsModel.ApproverComments = "";
            }
            objLeaveDetailsModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            //Getting Total Leaves
            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtgrvToDate.Text.Trim());
            dsTotalLeaves = objLeaveDeatilsBOL.TotalLeaveBalance(objLeaveDetailsModel);
            if (dsTotalLeaves.Tables[0].Rows[0]["Leave_Balance"].ToString() == "")
            {
                lblAvailableLeaves.Text = Convert.ToString("0");
            }
            else
            {
                lblAvailableLeaves.Text = Convert.ToString(dsTotalLeaves.Tables[0].Rows[0]["Leave_Balance"].ToString());
                string Error = Convert.ToString(lblAvailableLeaves.Text.ToString().StartsWith("-"));
                if (Error.ToString() == "True")
                {
                    lblAvailableLeaves.Text = "0";
                }
            }

            //Check the leaves dates are in SignInSignOut Table
            dsCheckInSignIn = objLeaveDeatilsBOL.CheckLeavesinSignIn(objLeaveDetailsModel);

            //Getting All the Holidays Details
            dsHolidaysList = objHolidayBOL.bindData();

            FromDate = Convert.ToDateTime(txtgrvFormDate.Text.Trim());
            ToDate = Convert.ToDateTime(txtgrvToDate.Text.Trim());
            TimeSpan ts = ToDate - FromDate;

            TotalLeaves = ts.TotalDays + 1;
            strLeaves = new String[Convert.ToInt32(TotalLeaves)];


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
                lblError.Text = "You have  already Signed In for these dates / It is in Holiday List / Weekly Off, So you can't apply leaves for these dates";
                gvLeaveApplication.EditIndex = -1;
                BindData();
                return;
            }
            TotalLeavesApplyedFor = j++;
            if (TotalLeavesApplyedFor == 0)
            {
                lblError.Text = ("You have  already Signed In for these dates / It is in Holiday List / Weekly Off, So you can't apply leaves for these dates");
                gvLeaveApplication.EditIndex = -1;
                BindData();
                return;
            }
            TotalLeavesBalance = lblAvailableLeaves.Text.Split('.');
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
                    gvLeaveApplication.EditIndex = -1;
                    BindData();
                    return;
                }
                else
                {
                    UpdateLeaveDetails();

                    //lblError.Text = "You have applied for " + TotalLeavesApplyedFor + " leaves But your's Available Leaves are " + BalanceLeaves + ", So" + absent + " days marked as absent.";
                    lblError.Text = "You have applied for " + TotalLeavesApplyedFor + " leaves, but your Leave balance is " + BalanceLeaves + ", So " + absent + " days will be marked as absent.";
                }
            }
            else
            {
                if (ConfigdateTime.Date >= Convert.ToDateTime(txtgrvFormDate.Text).Date)
                {
                    lblError.Text = "Administrator has frozen the data for the selected date";
                    gvLeaveApplication.EditIndex = -1;
                    BindData();
                    return;
                }
                else
                {
                    objLeaveDetailsModel.TotalLeaveDays = TotalLeavesApplyedFor;
                    objLeaveDetailsModel.LeaveCorrectionDays = Convert.ToInt32(0);
                    UpdateLeaveDetails();
                }

            }
            gvLeaveApplication.EditIndex = -1;
            BindData();
            lblError.Visible = true;
            lblSuccess.Visible = true;
            lblSuccess.Text = "Leave details are updated successfully.";

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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "gvLeaveApplication_RowUpdating", ex.StackTrace);
                throw new V2Exceptions();
            }
            else
            {
                lblError.Visible = true;
                //lblError.Text = ex.Message;
                lblError.Text = "User has already applied for leaves for these dates.";               
            }
        }
    }
    #endregion

    #region Reset Search
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
            tdSearch.Visible = true;
            tdAddLeave.Visible = false;

            txtSearchFromDate.Text = "";
            txtSearchToDate.Text = "";
            lblSuccess.Text = "";
            lblError.Text = "";
            lnkAddLeaves.Visible = true;
            BindData();
            ddlStatus.SelectedValue = Convert.ToString("0");
            if (ddlStatus.SelectedValue == "0")
            {
                objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
                dsAllDetails = objLeaveDeatilsBOL.SearchAllLeaveDetails(objLeaveDetailsModel);
                gvLeaveApplication.Visible = true;
                gvLeaveApplication.DataSource = dsAllDetails.Tables[0];
                gvLeaveApplication.DataBind();
            }
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "btnCancel_Click", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region ddlstatus changed
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try{
        txtSearchFromDate.Text = "";
        txtSearchToDate.Text = "";
        objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
        objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
        dsAllDetails = objLeaveDeatilsBOL.SearchAllLeaveDetails(objLeaveDetailsModel);
        if (dsAllDetails.Tables[0].Rows.Count <= 0)
        {
            lblError.Text = "Records Not Found";
            gvLeaveApplication.Visible = false;
        }
        else
        {
            lblError.Text = "";
            gvLeaveApplication.Visible = true;
            gvLeaveApplication.DataSource = dsAllDetails.Tables[0];
            gvLeaveApplication.DataBind();
        }
    }
    catch (V2Exceptions ex)
    {
        throw;
    }
    catch (System.Exception ex)
    {
        FileLog objFileLog = FileLog.GetLogger();
        objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "ddlStatus_SelectedIndexChanged", ex.StackTrace);
        throw new V2Exceptions();
    }
    }
    #endregion

    #region Paging
    protected void gvLeaveApplication_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try{
        gvLeaveApplication.PageIndex = e.NewPageIndex;
        gvLeaveApplication.EditIndex = -1;
        BindData();

        if (txtSearchFromDate.Text != "" && txtSearchFromDate.Text != "")
        {

            objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
            objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
            objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
            gvLeaveApplication.EditIndex = -1;
            SearchLeaveDetails();

        }
        else
        {
            objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
            objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
            dsSearchLeaveDetails = objLeaveDeatilsBOL.SearchAllLeaveDetails(objLeaveDetailsModel);
            if (dsSearchLeaveDetails.Tables[0].Rows.Count <= 0)
            {
                lblError.Text = "Records Not Found";
                gvLeaveApplication.Visible = false;
            }
            else
            {
                lblError.Text = "";
                gvLeaveApplication.Visible = true;
                gvLeaveApplication.DataSource = dsSearchLeaveDetails.Tables[0];
                gvLeaveApplication.DataBind();
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
        objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "gvLeaveApplication_PageIndexChanging", ex.StackTrace);
        throw new V2Exceptions();
    }
    }
    #endregion

    #region Sorting
    protected void gvLeaveApplication_Sorting(object sender, GridViewSortEventArgs e)
    {
        try{
        objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
        //dsSorting = objLeaveDeatilsBOL.GetLeaveDetails(objLeaveDetailsModel);
        if (txtSearchFromDate.Text != "" && txtSearchFromDate.Text != "")
        {

            //objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
            objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
            objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
            objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
            gvLeaveApplication.EditIndex = -1;
            SearchLeaveDetails();

        }
        else
        {
            //objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
            objLeaveDetailsModel.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
            dsSearchLeaveDetails = objLeaveDeatilsBOL.SearchAllLeaveDetails(objLeaveDetailsModel);
            if (dsSearchLeaveDetails.Tables[0].Rows.Count <= 0)
            {
                lblError.Text = "Records Not Found";
                gvLeaveApplication.Visible = false;
            }
            else
            {
                lblError.Text = "";
                gvLeaveApplication.Visible = true;
                gvLeaveApplication.DataSource = dsSearchLeaveDetails.Tables[0];
                gvLeaveApplication.DataBind();
            }
        }
        gvLeaveApplication.DataSource = dsSearchLeaveDetails.Tables[0];
        gvLeaveApplication.DataBind();
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

        gvLeaveApplication.DataSource = dv;
        gvLeaveApplication.DataBind();
    }
    catch (V2Exceptions ex)
    {
        throw;
    }
    catch (System.Exception ex)
    {
        FileLog objFileLog = FileLog.GetLogger();
        objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "gvLeaveApplication_Sorting", ex.StackTrace);
        throw new V2Exceptions();
    }
    }
    #endregion

    #region SendingMailToReportingPerson
    public void SendingMailToReportingPerson()
    {
        try{
        dsReportingTo = objLeaveDeatilsBOL.GetReportingTo(objLeaveDetailsModel);
        if (dsReportingTo.Tables[0].Rows.Count > 0)
        {
            //Send  new password to the employee through Email.
            MailMessage objMailMessage = new MailMessage();

            string UserName, ApproverName, Reason, ToDate, FromDate;            
        
            int Applyleaves;

            UserName = dsReportingTo.Tables[0].Rows[0]["EmployeeName"].ToString();
            ApproverName = dsReportingTo.Tables[0].Rows[0]["ReporterName"].ToString();
            objMailMessage.To = dsReportingTo.Tables[0].Rows[0]["ReporterMailID"].ToString();
            objMailMessage.Cc = dsReportingTo.Tables[0].Rows[0]["CompetencyMailID"].ToString();
            dsCancelDetails = objLeaveDeatilsBOL.GetCancelLeaveDetails(objLeaveDetailsModel);

            ToDate = Convert.ToDateTime(dsCancelDetails.Tables[0].Rows[0]["LeaveDateTo"].ToString()).ToShortDateString();
            FromDate = Convert.ToDateTime(dsCancelDetails.Tables[0].Rows[0]["LeaveDateFrom"].ToString()).ToShortDateString();
            Reason = dsCancelDetails.Tables[0].Rows[0]["LeaveReason"].ToString();
            Applyleaves = Convert.ToInt32(dsCancelDetails.Tables[0].Rows[0]["TotalLeaveDays"].ToString()) + Convert.ToInt32(dsCancelDetails.Tables[0].Rows[0]["LeaveCorrectionDays"].ToString());
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

            objMailMessage.Subject = "Cancelling Leave";
            objMailMessage.BodyFormat = MailFormat.Html;

            objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi " + ApproverName + " ," + "<br>" + "<br>" + "User Name: " + "<b>" + UserName + "</b>" + " <br> " + "FromDate: " + FromDate + " <br> " + "ToDate: " + ToDate + " <br> " + "Leave Reason: " + Reason + " <br> " + "Leave Applied For: " + Applyleaves + " <br> " + " <br> " + " Cancelled the Approved Leave Details, the required updates are made in the system.";
                        
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
        objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplicationForm.aspx.cs", "SendingMailToReportingPerson", ex.StackTrace);
        throw new V2Exceptions();
    }
    }
    #endregion

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
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApplication.aspx.cs", "UpdateEmployeeLeaveAndComp", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion
}
