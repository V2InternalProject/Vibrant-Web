using HRMS.Notification;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;

namespace HRMS.Orbitweb
{
    public partial class AdminLeaveApplicationForm : System.Web.UI.Page
    {
        #region Variables Declaration

        private LeaveDetailsBOL objLeaveDeatilsBOL = new LeaveDetailsBOL();
        private LeaveDetailsModel objLeaveDetailsModel = new LeaveDetailsModel();
        private LeaveTransactionBOL objLeaveTransDetailsBOL = new LeaveTransactionBOL();
        private LeaveTransactionModel objLeaveTransDetailsModel = new LeaveTransactionModel();
        private OutOfOfficeBOL objOutOfOfficeBOL = new OutOfOfficeBOL();
        private OutOfOfficeModel objOutOfOfficeModel = new OutOfOfficeModel();
        private HolidayMasterModel objHolidayModel = new HolidayMasterModel();
        private HolidayMasterBOL objHolidayBOL = new HolidayMasterBOL();
        private BulkEntriesBOL objBulkEntriesBOL = new BulkEntriesBOL();
        private SignInSignOutModel objSignInSignOutModel = new SignInSignOutModel();
        private DataSet dsLeave, dsHolidaysList;
        private DataSet dsSearchLeaveDetails, dsTotalLeaves, dsMaxLeaveDetailID, dsLeaveBalance, dsEmployee, dsConfigItem, dsCheckInSignIn, dsReportingTo, dsCancelDetails;
        private DateTime FromDate, ToDate;
        private double TotalLeaves = 0, count = 0;
        private int j = 0, TotalLeavesApplyedFor = 0;
        private double CorrectionLeaves;
        private int BalanceLeaves, absent;
        private String[] strLeaves, TotalLeavesBalance;
        private bool oneLeaveWeekend = false;
        private string empid = "";
        private string LoginID = string.Empty;
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
                string PageName = "Admin Leave Approval";
                objpagelevel.PageLevelAccess(PageName);

                lblError.Text = "";
                lblSuccess.Text = "";
                LoginID = User.Identity.Name.ToString();
                if (!IsPostBack)
                {
                    BindData();
                    FillSearchEmployee();
                    tdAddLeave.Visible = true;
                    spanAddLeave.Visible = true;
                    spanSearch.Visible = false;
                    spanEdit.Visible = false;

                    ViewState["Search"] = 0;
                }

                txtFromDate.Attributes.Add("onkeydown", "return false");
                txtToDate.Attributes.Add("onkeydown", "return false");
                btnSearch.Attributes.Add("onClick", "return SearchValidation();");
                btnSubmit.Attributes.Add("onClick", "return Validation();");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Page Load

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
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (ex.Message.CompareTo("Already leave applied for this dates.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "AddLeaveDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion AddLeaveDetails

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
            catch (Exception ex)
            {
                if (ex.Message.CompareTo("Already leave applied for this dates.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "AddLeaveTransDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion AddLeaveTransDetails

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
            catch (Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "DeleteLeaveTransDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion DeleteLeaveTransDetails

        #region Submit

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                gvLeaveApplication.PageIndex = 0;
                objLeaveDetailsModel.UserID = Convert.ToInt32(txtempid.Text);
                objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtFromDate.Text.Trim());
                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtToDate.Text.Trim());
                objLeaveDetailsModel.LeaveResason = "Bulk Leave";
                objLeaveDetailsModel.StatusID = Convert.ToInt32(2);
                objLeaveDetailsModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                objLeaveDetailsModel.ApproverComments = txtReason.Text.ToString();

                objLeaveDetailsModel.ApproverID = Convert.ToInt32(LoginID);

                dsConfigItem = objLeaveDeatilsBOL.GetLeaveDetails(objLeaveDetailsModel);
                DateTime ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                //Getting Total Leaves
                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtToDate.Text.Trim());
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
                // dsHolidaysList = objHolidayBOL.bindData();
                objHolidayModel.UserID = objLeaveDetailsModel.UserID;
                objHolidayModel.StartDate = objLeaveDetailsModel.LeaveDateFrom;
                objHolidayModel.EndDate = objLeaveDetailsModel.LeaveDateTo;
                dsHolidaysList = objHolidayBOL.bindHolidaysForLeaveApprovals(objHolidayModel);

                FromDate = Convert.ToDateTime(txtFromDate.Text.Trim());
                ToDate = Convert.ToDateTime(txtToDate.Text.Trim());
                TimeSpan ts = ToDate - FromDate;

                TotalLeaves = ts.TotalDays + 1;
                strLeaves = new String[Convert.ToInt32(TotalLeaves)];

                objRosterPlanningModel.UserId = Convert.ToInt32(txtempid.Text);
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
                        if (onweeklyoff == true)
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
                    lblError.Text = txtempname.Text + " has already Signed In for these dates / It is in Holiday List / Sat/Sun, So you can't apply leaves for these dates";
                    return;
                }

                count++;
                TotalLeavesApplyedFor = j++;

                int totalleavesappliedfor = TotalLeavesApplyedFor;
                int CurrentBalance = 0;
                int OnDayBalance = 0;
                bool leaveFlag = false;

                if (TotalLeavesApplyedFor == 0)
                {
                    lblError.Text = txtempname.Text + " has already Signed In for these dates / It is in Holiday List / Sat/Sun, So you can't apply leaves for these dates";
                    return;
                }
                else
                {
                    if (Convert.ToDateTime(txtFromDate.Text.Trim()) < Convert.ToDateTime(DateTime.Now))
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
                        lblError.Text = "Administrator has frozen the data from " + ConfigdateTime.Date.ToShortDateString() + ". Select another date";
                        return;
                    }
                    else
                    {
                        lblError.Text = txtempname.Text + " has applied for " + TotalLeavesApplyedFor + " leaves, but his/her Leave balance is " + BalanceLeaves + ", So " + absent + " days will be marked as absent.";

                        AddLeaveDetails();
                    }
                    for (int k = 0; k < TotalLeavesApplyedFor; k++)
                    {
                        if (strLeaves[k] == null)
                        {
                            break;
                        }
                        else
                        {
                            objLeaveTransDetailsModel.UserID = Convert.ToInt32(txtempid.Text);

                            dsMaxLeaveDetailID = objLeaveTransDetailsBOL.GetMaxLeaveDetailID();
                            objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(dsMaxLeaveDetailID.Tables[0].Rows[0]["LeaveDetailsID"].ToString());

                            objLeaveTransDetailsModel.TransactionDate = Convert.ToDateTime(strLeaves[k].ToString());
                            objLeaveTransDetailsModel.LeaveType = Convert.ToBoolean(1);

                            if (BalanceLeaves > 0)
                            {
                                objLeaveTransDetailsModel.Quantity = Convert.ToDecimal("-" + 1);
                                objLeaveTransDetailsModel.Description = "Leave:" + txtReason.Text.ToString();
                                AddLeaveTransDetails();
                                UpdateEmployeeLeaveAndComp();
                                BalanceLeaves--;
                            }
                            else
                            {
                                objLeaveTransDetailsModel.Quantity = Convert.ToDecimal(0);
                                objLeaveTransDetailsModel.Description = "Absent:" + txtReason.Text.ToString();
                                AddLeaveTransDetails();
                                UpdateEmployeeLeaveAndComp();
                            }
                        }
                    }
                }
                else
                {
                    if (ConfigdateTime.Date >= Convert.ToDateTime(txtFromDate.Text).Date)
                    {
                        lblError.Text = "Administrator has frozen the data from " + ConfigdateTime.Date.ToShortDateString() + ". Select another date";
                        return;
                    }
                    else
                    {
                        if (leaveFlag)
                        {
                            absent = totalleavesappliedfor - TotalLeavesApplyedFor;
                            objLeaveDetailsModel.TotalLeaveDays = Convert.ToInt32(TotalLeavesApplyedFor);
                            objLeaveDetailsModel.LeaveCorrectionDays = Convert.ToInt32(absent);
                            AddLeaveDetails();
                            lblError.Text = "You have applied for " + totalleavesappliedfor + " leaves, but your Leave balance till that day is " + CurrentBalance + ", So actual leave days will be " + TotalLeavesApplyedFor + " and " + absent + " days will be marked as absent.";
                        }
                        else
                        {
                            objLeaveDetailsModel.TotalLeaveDays = Convert.ToInt32(TotalLeavesApplyedFor);
                            objLeaveDetailsModel.LeaveCorrectionDays = Convert.ToInt32(0);
                            AddLeaveDetails();
                        }
                    }

                    objLeaveTransDetailsModel.UserID = Convert.ToInt32(txtempid.Text);
                    dsMaxLeaveDetailID = objLeaveTransDetailsBOL.GetMaxLeaveDetailID();
                    objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(dsMaxLeaveDetailID.Tables[0].Rows[0]["LeaveDetailsID"].ToString());
                    DeleteLeaveTransDetails();
                    UpdateEmployeeLeaveAndComp();

                    for (int k = 0; k < strLeaves.Length; k++)
                    {
                        if (strLeaves[k] == null)
                        {
                            break;
                        }
                        else
                        {
                            objLeaveTransDetailsModel.UserID = Convert.ToInt32(txtempid.Text);
                            dsMaxLeaveDetailID = objLeaveTransDetailsBOL.GetMaxLeaveDetailID();
                            objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(dsMaxLeaveDetailID.Tables[0].Rows[0]["LeaveDetailsID"].ToString());
                            objLeaveTransDetailsModel.TransactionDate = Convert.ToDateTime(strLeaves[k].ToString());

                            if (leaveFlag)
                            {
                                if (TotalLeavesApplyedFor > 0)
                                {
                                    objLeaveTransDetailsModel.Description = "Leave:" + txtReason.Text.ToString();
                                    objLeaveTransDetailsModel.Quantity = Convert.ToDecimal("-" + 1);
                                    TotalLeavesApplyedFor--;
                                }
                                else
                                {
                                    objLeaveTransDetailsModel.Description = "Absent:" + txtReason.Text.ToString();
                                    objLeaveTransDetailsModel.Quantity = Convert.ToInt32(0);
                                }
                            }
                            else
                            {
                                objLeaveTransDetailsModel.Description = "Leave:" + txtReason.Text.ToString();
                                objLeaveTransDetailsModel.Quantity = Convert.ToDecimal("-" + 1);
                            }

                            AddLeaveTransDetails();
                            UpdateEmployeeLeaveAndComp();
                        }
                    }
                }
                lblError.Visible = true;
                lblSuccess.Text = "Leave application Successfully submitted";
                BindData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (ex.Message.CompareTo("Already leave applied for this dates.") != 0)
                {
                    lblError.Visible = false;
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "btnSubmit", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    lblError.Visible = true;

                    lblError.Text = txtempname.Text + " already applied for leaves for these dates.";
                }
            }
        }

        #endregion Submit

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

                if (txtempname.Text == "")
                {
                    gvLeaveApplication.Visible = false;
                }
                else
                {
                    objLeaveDetailsModel.UserID = Convert.ToInt32(txtempid.Text);
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

                    dsLeave = objLeaveDeatilsBOL.GetBulkLeaveDetails(objLeaveDetailsModel);
                    if (dsLeave.Tables[0].Rows.Count <= 0)
                    {
                        lblError.Text = "Records Not Found";
                        gvLeaveApplication.Visible = false;
                    }
                    else
                    {
                        gvLeaveApplication.Visible = true;
                        gvLeaveApplication.DataSource = dsLeave.Tables[0];
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "BindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion BindData

        #region FillSearchEmployee

        public void FillSearchEmployee()
        {
            try
            {
                objOutOfOfficeModel.UserId = Convert.ToInt32(LoginID);
                dsEmployee = objOutOfOfficeBOL.GetEmployeeNameRpt(objOutOfOfficeModel);

                if (Roles.IsUserInRole("Admin"))
                {
                    for (int i = 0; i < dsEmployee.Tables[1].Rows.Count; i++)
                    {
                        ddlEmployeeName.Items.Add(new ListItem(dsEmployee.Tables[1].Rows[i]["EmployeeName"].ToString(), dsEmployee.Tables[1].Rows[i]["UserID"].ToString()));
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "FillEmployeeNames", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion FillSearchEmployee

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
            catch (Exception ex)
            {
                if (ex.Message.CompareTo("Already leave applied for this dates.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "UpdateLeaveDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion UpdateLeaveDetails

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
            catch (Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx", "UpdateLeaveBalance", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion UpdateLeaveBalance

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
            catch (Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx", "UpdateCancelLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion UpdateCancelLeaveDetails

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
            catch (Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "UpdateLeaveTransDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion UpdateLeaveTransDetails

        #region RowEditing

        protected void gvLeaveApplication_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                tdAddLeave.Visible = false;

                spanAddLeave.Visible = false;
                spanEdit.Visible = true;
                spanSearch.Visible = false;

                gvLeaveApplication.EditIndex = e.NewEditIndex;

                LinkButton lbnCancel = ((LinkButton)(gvLeaveApplication.Rows[e.NewEditIndex].FindControl("lnkButCancel")));
                lbnCancel.Visible = false;
                BindData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "gvLeaveApplication_RowEditing", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion RowEditing

        #region Search

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                objLeaveDetailsModel.UserID = Convert.ToInt32(ddlEmployeeName.SelectedValue);
                objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                SearchLeaveDetails();
                tdAddLeave.Visible = false;
                spanSearch.Visible = true;
                spanAddLeave.Visible = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "btnSearch_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Search

        #region SearchLeaveDetails

        public void SearchLeaveDetails()
        {
            try
            {
                dsSearchLeaveDetails = objLeaveDeatilsBOL.SearchBulkLeaveDetails(objLeaveDetailsModel);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "SearchLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion SearchLeaveDetails

        #region Search Link

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
                tdAddLeave.Visible = false;
                gvLeaveApplication.Visible = true;
                lblSuccess.Text = "";
                lblError.Text = "";
                spanSearch.Visible = true;
                spanAddLeave.Visible = false;
                spanEdit.Visible = false;
                gvLeaveApplication.EditIndex = -1;

                SearchData();
                ViewState["Search"] = 1;
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtReason.Text = "";

                txtempname.Text = "";
                lblAvailable.Text = "";
                lblAvailableLeaves.Text = "";
                lblAleave.Text = "";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "lnkSearch_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Search Link

        #region Addleaves Link

        protected void lnkAddLeaves_Click(object sender, EventArgs e)
        {
            try
            {
                tdAddLeave.Visible = true;
                gvLeaveApplication.Visible = true;
                ViewState["Search"] = 0;
                lblSuccess.Text = "";
                lblError.Text = "";
                spanSearch.Visible = false;
                spanEdit.Visible = false;
                spanAddLeave.Visible = true;
                gvLeaveApplication.EditIndex = -1;
                BindData();
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtReason.Text = "";
                ddlEmployeeName.SelectedValue = Convert.ToString("0");
                lblAvailable.Text = "";
                lblAvailableLeaves.Text = "";
                lblAleave.Text = "";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "lnkAddLeaves_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Addleaves Link

        #region RowDatabound

        protected void gvLeaveApplication_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblstatus = ((Label)(e.Row.FindControl("lblgrvStatusName")));
                    Label lblgrvToDate = ((Label)(e.Row.FindControl("lblgrvToDate")));
                    Label lblApproved = ((Label)(e.Row.FindControl("lblApprove")));
                    LinkButton lnkEdit = ((LinkButton)(e.Row.FindControl("lnkbutEdit")));
                    LinkButton lnkCancel = ((LinkButton)(e.Row.FindControl("lnkButCancel")));

                    objOutOfOfficeModel.UserId = Convert.ToInt32(LoginID);
                    dsConfigItem = objOutOfOfficeBOL.GetEmployeeNameRpt(objOutOfOfficeModel);

                    DateTime ConfigDate = Convert.ToDateTime(dsConfigItem.Tables[2].Rows[0]["ConfigItemValue"].ToString());

                    if (lblstatus.Text == "Approved")
                    {
                        if (ConfigDate.Date >= Convert.ToDateTime(lblgrvToDate.Text).Date)
                        {
                            lnkCancel.Visible = false;
                            lblApproved.Text = "Leave Approved";
                        }
                        lnkEdit.Visible = false;
                    }
                    else if (lblstatus.Text == "Cancel")
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "gvLeaveApplication_RowDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion RowDatabound

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
                    Label lblUserID = new Label();
                    for (int i = 0; i < gvLeaveApplication.Rows.Count; i++)
                    {
                        Label lblId = (Label)((gvLeaveApplication.Rows[i].FindControl("lblLeaveDetailID")));

                        if (lblId.Text == e.CommandArgument.ToString())
                        {
                            lblStatusName = (Label)((gvLeaveApplication.Rows[i].FindControl("lblgrvStatusName")));
                            lblUserID = (Label)((gvLeaveApplication.Rows[i].FindControl("lblUserID")));
                            break;
                        }
                    }

                    objLeaveDetailsModel.LeaveDetailsID = Convert.ToInt32(e.CommandArgument);
                    objLeaveDetailsModel.UserID = Convert.ToInt32(lblUserID.Text.ToString());
                    if (lblStatusName.Text == "Approved")
                    {
                        //Here delete the all records of related leavedetailsID
                        objLeaveTransDetailsModel.UserID = Convert.ToInt32(lblUserID.Text.ToString());
                        objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(e.CommandArgument);
                        DeleteLeaveTransDetails();
                        UpdateEmployeeLeaveAndComp();

                        //Sending a Mail to Reporting person
                        objLeaveDetailsModel.UserID = Convert.ToInt32(lblUserID.Text.ToString());
                        SendingMailToReportingPerson();
                    }

                    objLeaveDetailsModel.StatusID = Convert.ToInt32(4);
                    objLeaveDetailsModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    UpdateCancelLeaveDetails();

                    if (ViewState["Search"].ToString() == "0")
                    {
                        BindData();
                    }
                    else
                    {
                        SearchData();
                    }

                    lblError.Visible = false;
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Leave details are Cancelled successfully";
                }

                if (e.CommandName == "lnkCancel")
                {
                    gvLeaveApplication.EditIndex = -1;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "gvLeaveApplication_RowCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion RowCommand

        #region RowCancelingEdit

        protected void gvLeaveApplication_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvLeaveApplication.EditIndex = -1;
            BindData();
        }

        #endregion RowCancelingEdit

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

                Label lblEmployee = row.FindControl("lblApprover") as Label;

                TextBox txtgrvFormDate = row.FindControl("txtgrvFormDate") as TextBox;
                objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtgrvFormDate.Text.Trim());

                TextBox txtgrvToDate = row.FindControl("txtgrvToDate") as TextBox;
                objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtgrvToDate.Text.Trim());

                objLeaveDetailsModel.LeaveResason = Convert.ToString("");

                Label lblgrvStatusName = row.FindControl("lblgrvStatusName") as Label;
                objLeaveDetailsModel.ApproverComments = Convert.ToString(lblgrvStatusName.Text);

                TextBox txtApproverComments = row.FindControl("txtgrvApproverComments") as TextBox;
                objLeaveDetailsModel.ApproverComments = Convert.ToString(txtApproverComments.Text);

                Label lblgrvStatusID = row.FindControl("lblgrvStatusID") as Label;
                objLeaveDetailsModel.StatusID = Convert.ToInt32(lblgrvStatusID.Text);

                objLeaveDetailsModel.RequestedOn = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                dsConfigItem = objLeaveDeatilsBOL.GetLeaveDetails(objLeaveDetailsModel);
                DateTime ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

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
                        lblAvailableLeaves.Text = Convert.ToString("0");
                    }
                }

                //Getting All the Holidays Details
                objHolidayModel.UserID = objLeaveDetailsModel.UserID;
                objHolidayModel.StartDate = objLeaveDetailsModel.LeaveDateFrom;
                objHolidayModel.EndDate = objLeaveDetailsModel.LeaveDateTo;
                dsHolidaysList = objHolidayBOL.bindHolidaysForLeaveApprovals(objHolidayModel);
                // dsHolidaysList = objHolidayBOL.bindData();

                //Check the leaves dates are in SignInSignOut Table
                dsCheckInSignIn = objLeaveDeatilsBOL.CheckLeavesinSignIn(objLeaveDetailsModel);

                FromDate = Convert.ToDateTime(txtgrvFormDate.Text.Trim());
                ToDate = Convert.ToDateTime(txtgrvToDate.Text.Trim());
                TimeSpan ts = ToDate - FromDate;

                TotalLeaves = ts.TotalDays + 1;
                strLeaves = new String[Convert.ToInt32(TotalLeaves)];

                for (int i = 0; i < TotalLeaves; i++)
                {
                    if (FromDate.DayOfWeek.ToString() == "Saturday" || FromDate.DayOfWeek.ToString() == "Sunday")
                    {
                        count++;
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
                                count++;
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
                count++;
                if (oneLeaveWeekend == true)
                {
                    lblError.Visible = true;
                    lblError.Text = lblEmployee.Text + " has already Signed In for these dates / It is in Holiday List / Sat/Sun, So you can't apply leaves for these dates";
                    gvLeaveApplication.EditIndex = -1;
                    BindData();
                    return;
                }

                TotalLeavesApplyedFor = j++;
                int totalleavesappliedfor = TotalLeavesApplyedFor;
                int CurrentBalance = 0;
                int OnDayBalance = 0;
                bool leaveFlag = false;

                if (TotalLeavesApplyedFor == 0)
                {
                    lblError.Text = lblEmployee.Text + " has  already Signed In for these dates / It is in Holiday List / Sat/Sun, So you can't apply leaves for these dates";
                    gvLeaveApplication.EditIndex = -1;
                    BindData();
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

                objLeaveTransDetailsModel.UserID = Convert.ToInt32(txtempid.Text);
                objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text);
                DeleteLeaveTransDetails();
                UpdateEmployeeLeaveAndComp();

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
                        lblAvailableLeaves.Text = Convert.ToString("0");
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

                    if (ConfigdateTime.Date >= Convert.ToDateTime(txtgrvFormDate.Text).Date)
                    {
                        lblError.Text = "Administrator has frozen the data from " + ConfigdateTime.Date.ToShortDateString() + ". Select another date";
                        gvLeaveApplication.EditIndex = -1;
                        BindData();
                        return;
                    }
                    else
                    {
                        UpdateLeaveDetails();
                        lblError.Text = lblEmployee.Text + " has applied for " + TotalLeavesApplyedFor + " leaves, but his/her Leave balance is " + BalanceLeaves + ", So " + absent + " days will be marked as absent.";
                    }

                    for (int k = 0; k < TotalLeavesApplyedFor; k++)
                    {
                        if (strLeaves[k] == null)
                        {
                            break;
                        }
                        else
                        {
                            objLeaveTransDetailsModel.UserID = Convert.ToInt32(txtempid.Text);
                            objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text);
                            objLeaveTransDetailsModel.TransactionDate = Convert.ToDateTime(strLeaves[k].ToString());
                            objLeaveTransDetailsModel.LeaveType = Convert.ToBoolean(1);

                            if (BalanceLeaves > 0)
                            {
                                objLeaveTransDetailsModel.Quantity = Convert.ToDecimal("-" + 1);
                                objLeaveTransDetailsModel.Description = "Leave:" + txtApproverComments.Text.ToString();
                                AddLeaveTransDetails();
                                UpdateEmployeeLeaveAndComp();
                                BalanceLeaves--;
                            }
                            else
                            {
                                objLeaveTransDetailsModel.Quantity = Convert.ToDecimal(0);
                                objLeaveTransDetailsModel.Description = "Absent:" + txtApproverComments.Text.ToString();
                                AddLeaveTransDetails();
                                UpdateEmployeeLeaveAndComp();
                            }
                        }
                    }
                }
                else
                {

                    if (ConfigdateTime.Date >= Convert.ToDateTime(txtgrvFormDate.Text).Date)
                    {
                        lblError.Text = "Administrator has frozen the data from " + ConfigdateTime.Date.ToShortDateString() + ". Select another date";
                        txtgrvFormDate.Text = "";
                        gvLeaveApplication.EditIndex = -1;
                        return;
                    }
                    else
                    {
                        if (leaveFlag)
                        {
                            absent = totalleavesappliedfor - TotalLeavesApplyedFor;
                            objLeaveDetailsModel.TotalLeaveDays = Convert.ToInt32(TotalLeavesApplyedFor);
                            objLeaveDetailsModel.LeaveCorrectionDays = Convert.ToInt32(absent);
                            UpdateLeaveDetails();
                            lblError.Text = "You have applied for " + totalleavesappliedfor + " leaves, but your Leave balance till that day is " + CurrentBalance + ", So actual leave days will be " + TotalLeavesApplyedFor + " and " + absent + " days will be marked as absent.";
                        }
                        else
                        {
                            objLeaveDetailsModel.TotalLeaveDays = TotalLeavesApplyedFor;
                            objLeaveDetailsModel.LeaveCorrectionDays = Convert.ToInt32(0);
                            UpdateLeaveDetails();
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
                            objLeaveTransDetailsModel.UserID = Convert.ToInt32(txtempid.Text);
                            objLeaveTransDetailsModel.LeaveDetailsID = Convert.ToInt32(lblLeaveDetailID.Text);
                            objLeaveTransDetailsModel.TransactionDate = Convert.ToDateTime(strLeaves[k].ToString());
                            objLeaveTransDetailsModel.LeaveType = Convert.ToBoolean(1);

                            if (leaveFlag)
                            {
                                if (TotalLeavesApplyedFor > 0)
                                {
                                    objLeaveTransDetailsModel.Description = "Leave:" + txtApproverComments.Text.ToString();
                                    objLeaveTransDetailsModel.Quantity = Convert.ToDecimal("-" + 1);
                                    TotalLeavesApplyedFor--;
                                }
                                else
                                {
                                    objLeaveTransDetailsModel.Description = "Absent:" + txtApproverComments.Text.ToString();
                                    objLeaveTransDetailsModel.Quantity = Convert.ToInt32(0);
                                }
                            }
                            else
                            {
                                objLeaveTransDetailsModel.Description = "Leave:" + txtApproverComments.Text.ToString();
                                objLeaveTransDetailsModel.Quantity = Convert.ToDecimal("-" + 1);
                            }

                            AddLeaveTransDetails();
                            UpdateEmployeeLeaveAndComp();
                        }
                    }
                }

                gvLeaveApplication.EditIndex = -1;
                BindData();
                UpdateEmployeeLeaveAndComp();
                lblError.Visible = true;
                lblSuccess.Visible = true;
                lblSuccess.Text = "Leave details are updated successfully";
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "gvLeaveApplication_RowUpdating", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = lblEmployee.Text + " has already applied for leaves for these dates.";
                }
            }
        }

        #endregion RowUpdating

        #region Cancelling Leave

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                objLeaveDetailsModel.UserID = Convert.ToInt32(LoginID);
                tdAddLeave.Visible = false;
                ddlEmployeeName.SelectedValue = Convert.ToString("0");
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
                lblSuccess.Text = "";

                SearchData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Cancelling Leave

        #region Paging

        protected void gvLeaveApplication_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvLeaveApplication.PageIndex = e.NewPageIndex;
                gvLeaveApplication.EditIndex = -1;
                if (ViewState["Search"].ToString() == "0")
                {
                    BindData();
                }
                else
                {
                    SearchData();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "gvLeaveApplication_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Paging

        #region Sorting

        protected void gvLeaveApplication_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (ViewState["Search"].ToString() == "0")
                {
                    BindData();
                }
                else
                {
                    SearchData();
                }
                if (txtSearchFromDate.Text != "" && txtSearchToDate.Text != "")
                {
                    objLeaveDetailsModel.UserID = Convert.ToInt32(ddlEmployeeName.SelectedValue);
                    objLeaveDetailsModel.LeaveDateFrom = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objLeaveDetailsModel.LeaveDateTo = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    dsLeave = objLeaveDeatilsBOL.SearchBulkLeaveDetails(objLeaveDetailsModel);
                }
                gvLeaveApplication.DataSource = dsLeave.Tables[0];
                gvLeaveApplication.DataBind();
                DataTable dt = dsLeave.Tables[0];
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "gvLeaveApplication_Sorting", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Sorting

        #region EmployeeNameChanged

        public void EmployeeNameChanged()
        {
            try
            {
                SearchData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "EmployeeNameChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion EmployeeNameChanged

        #region Search

        protected void ddlEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
                SearchData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "ddlEmployeeName_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void SearchData()
        {
            try
            {
                if (ddlEmployeeName.SelectedValue == "" || ddlEmployeeName.SelectedValue == "0")
                {
                    objLeaveDetailsModel.UserID = Convert.ToInt32(ddlEmployeeName.SelectedValue);
                    objLeaveDetailsModel.StatusID = Convert.ToInt32(0);

                    dsLeave = objLeaveDeatilsBOL.GetBulkLeaveDetails(objLeaveDetailsModel);
                    if (dsLeave.Tables[0].Rows.Count > 0)
                    {
                        gvLeaveApplication.DataSource = dsLeave.Tables[0];
                        gvLeaveApplication.DataBind();
                        gvLeaveApplication.Visible = true;
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
                else
                {
                    objLeaveDetailsModel.UserID = Convert.ToInt32(ddlEmployeeName.SelectedValue);

                    dsLeave = objLeaveDeatilsBOL.GetBulkLeaveDetails(objLeaveDetailsModel);
                    if (dsLeave.Tables[0].Rows.Count <= 0)
                    {
                        lblError.Text = "Records Not Found";
                        gvLeaveApplication.Visible = false;
                    }
                    else
                    {
                        lblError.Text = "";
                        gvLeaveApplication.Visible = true;
                        gvLeaveApplication.DataSource = dsLeave.Tables[0];
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "SearchData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Search

        #region SendingMailToReportingPerson

        public void SendingMailToReportingPerson()
        {
            try
            {
                dsReportingTo = objLeaveDeatilsBOL.GetReportingTo(objLeaveDetailsModel);
                if (dsReportingTo.Tables[0].Rows.Count > 0)
                {
                    //Send  new password to the employee through Email.
                    MailMessage objMailMessage = new MailMessage();
                    SmtpClient smtpClient = new SmtpClient();
                    string UserName, ApproverName, Reason, ToDate, FromDate;

                    int Applyleaves;

                    UserName = dsReportingTo.Tables[0].Rows[0]["EmployeeName"].ToString();
                    ApproverName = dsReportingTo.Tables[0].Rows[0]["ReporterName"].ToString();
                    objMailMessage.To.Add(new MailAddress(dsReportingTo.Tables[0].Rows[0]["ReporterMailID"].ToString()));

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

                    objMailMessage.Subject = "Cancelling Leave";

                    objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi " + ApproverName + " ," + "<br>" + "<br>" + "User Name: " + "<b>" + UserName + "</b>" + " <br> " + "FromDate: " + FromDate + " <br> " + "ToDate: " + ToDate + " <br> " + "Leave Reason: " + Reason + " <br> " + "Leave Applied For: " + Applyleaves + " <br> " + " <br> " + " Cancelled the Approved Leave Details, the required updates are made in the system.";

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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "SendingMailToReportingPerson", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion SendingMailToReportingPerson

        #region AutoComplete

        protected void txtempname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                gvLeaveApplication.PageIndex = 0;
                TextBox txtempname = (TextBox)sender;
                TextBox txtempid = txtempname.Parent.FindControl("txtempid") as TextBox;
                if (txtempid != null)
                {
                    empid = txtempname.Text.ToString();
                    string str = txtempid.Text;
                    string[] SplitEmpName = empid.Split(new Char[] { ' ' });

                    if (SplitEmpName.Length == 1)
                    {
                        lblError.Text = "Please Enter proper Employee Name";
                        gvLeaveApplication.Visible = false;
                        return;
                    }
                    txtempid.Text = Convert.ToString(SplitEmpName[0]);
                    txtempname.Text = SplitEmpName[1];
                    for (int i = 2; i < SplitEmpName.Length; i++)
                    {
                        txtempname.Text += " " + SplitEmpName[i];
                    }
                    txtFromDate.Text = "";
                    txtToDate.Text = "";
                    txtReason.Text = "";

                    // To check validataion for employee name
                    objLeaveTransDetailsModel.EmployeeName = txtempname.Text;
                    rowsAffected = objLeaveTransDetailsBOL.CheckEmployeeNameValidation(objLeaveTransDetailsModel);
                    if (rowsAffected <= 0)
                    {
                        lblError.Text = "Please Enter proper Employee Name";
                        txtempname.Text = "";
                        gvLeaveApplication.Visible = false;
                    }
                    //txtempname is empty
                    else if (txtempname.Text.Trim() == "")
                    {
                        lblError.Text = "Please Enter proper Employee Name";
                        gvLeaveApplication.Visible = false;
                    }
                    else
                    {
                        gvLeaveApplication.Visible = true;
                        objLeaveDetailsModel.UserID = Convert.ToInt32(txtempid.Text);
                        gvLeaveApplication.EditIndex = -1;
                        BindData();
                    }

                    if (txtempname.Text == "")
                    {
                        trLeaveBalance.Visible = false;
                    }
                    else
                    {
                        lblAvailable.Visible = true;
                        lblAvailableLeaves.Visible = true;
                        lblAleave.Visible = true;
                        trLeaveBalance.Visible = true;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminLeaveApplicationForm.aspx.cs", "txtempname_TextChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion AutoComplete

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
    }
}