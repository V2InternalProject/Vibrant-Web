using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;

namespace HRMS.Orbitweb
{
    public partial class BulkEntries : System.Web.UI.Page
    {
        #region Variable declarations

        private LeaveDetailsBOL objLeaveDeatilsBOL = new LeaveDetailsBOL();
        private LeaveDetailsModel objLeaveDetailsModel = new LeaveDetailsModel();
        private BulkEntriesBOL objBulkBOL = new BulkEntriesBOL();
        private SignInSignOutBOL objSignInSignOutBOL = new SignInSignOutBOL();
        private SignInSignOutModel objSignInSignOutModel = new SignInSignOutModel();
        private string UserID = string.Empty;
        private DataSet dsLoadSignInSigOutData, dsGetLeaveStatus, dsEmployee, dsHolidaysList, dsSearchBulkDetails, dsConfigItem;
        private OutOfOfficeBOL objOutOfOfficeBOL = new OutOfOfficeBOL();
        private OutOfOfficeModel objOutOfOfficeModel = new OutOfOfficeModel();
        private HolidayMasterModel objHolidayModel = new HolidayMasterModel();
        private HolidayMasterBOL objHolidayBOL = new HolidayMasterBOL();
        private LeaveTransactionBOL objLeaveTransactionBOL = new LeaveTransactionBOL();
        private LeaveTransactionModel objLeaveTransactionModel = new LeaveTransactionModel();
        private DateTime FromDate, ToDate, ConfigDate;
        private double TotalLeaves = 0, count = 0;
        private int j = 0, TotalLeavesApplyedFor = 0;
        private double CorrectionLeaves, BalanceLeaves;
        private String[] strLeaves;
        private String[] strHoliDays;
        private bool oneLeaveWeekend = false;
        private int rowsAffected;
        private string empid = "";
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        #endregion Variable declarations

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string PageName = "Bulk Entries";
                objpagelevel.PageLevelAccess(PageName);

                lblError.Text = "";
                lblSuccess.Text = "";
                UserID = User.Identity.Name.ToString();
                if (!IsPostBack)
                {
                    //TODO : Fill resource listbox
                    FillEmployee();
                    //set date value fromt to for one month from current date
                    //bind blank grid

                    BindData();

                    tdAddBulk.Visible = true;
                    spanAddBulk.Visible = true;
                    spanSearch.Visible = false;
                    spanEdit.Visible = false;

                    ViewState["Search"] = 0;
                    tdGenerate.Visible = false;
                }

                txtFromDate.Attributes.Add("onkeydown", "return false");
                txtToDate.Attributes.Add("onkeydown", "return false");
                txtSearchFromDate.Attributes.Add("onkeydown", "return false");
                txtSearchToDate.Attributes.Add("onkeydown", "return false");

                btnSearch.Attributes.Add("onClick", "return SearchValidation();");
                btnSubmit.Attributes.Add("onClick", "return ValidationButton();");
                lnkbtnGenerateDates.Attributes.Add("onClick", "return Validation();");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Page Load

        #region BindData

        public void BindData()
        {
            try
            {
                objOutOfOfficeModel.UserId = Convert.ToInt32(UserID);
                dsConfigItem = objOutOfOfficeBOL.GetEmployeeNameRpt(objOutOfOfficeModel);

                ConfigDate = Convert.ToDateTime(dsConfigItem.Tables[2].Rows[0]["ConfigItemValue"].ToString());

                if (txtempname.Text == "")
                {
                    gvBulkEntries.Visible = false;
                }
                else
                {
                    objSignInSignOutModel.EmployeeID = Convert.ToInt32(txtempid.Text);
                    dsLoadSignInSigOutData = objBulkBOL.GetBulkEntriesForEmployees(objSignInSignOutModel);

                    if (dsLoadSignInSigOutData.Tables[0].Rows.Count > 0)
                    {
                        gvBulkEntries.Visible = true;
                        gvBulkEntries.DataSource = dsLoadSignInSigOutData.Tables[0];
                        gvBulkEntries.DataBind();
                    }
                    else if (dsLoadSignInSigOutData.Tables[0].Rows.Count == 0)
                    {
                        gvBulkEntries.DataSource = dsLoadSignInSigOutData.Tables[0];
                        gvBulkEntries.DataBind();

                        lblSuccess.Visible = true;
                        lblSuccess.Text = "No Bulk Entries details.";
                    }
                    else
                    {
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "No Bulk Entries details.";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "BindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion BindData

        #region FillEmployeeNames

        public void FillEmployeeNames()
        {
        }

        #endregion FillEmployeeNames

        #region AddBulkEntriesDetails

        public void AddBulkEntriesDetails()
        {
            try
            {
                rowsAffected = objBulkBOL.AddBulkEntriesDetails(objSignInSignOutModel);
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "AddBulkEntriesDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion AddBulkEntriesDetails

        #region Generates Dates

        protected void lnkbtnGenerateDates_Click(object sender, EventArgs e)
        {
            try
            {
                lblGenarate.Visible = false;
                tdGenerate.Visible = true;
                cblGenerateDates.Items.Clear();
                FromDate = Convert.ToDateTime(txtFromDate.Text.Trim());
                ToDate = Convert.ToDateTime(txtToDate.Text.Trim());
                TimeSpan ts = ToDate - FromDate;

                TotalLeaves = ts.TotalDays + 1;
                strLeaves = new String[Convert.ToInt32(TotalLeaves)];

                objHolidayModel.UserID = Convert.ToInt32(txtempid.Text);
                objHolidayModel.StartDate = Convert.ToDateTime(txtFromDate.Text.Trim());
                objHolidayModel.EndDate = Convert.ToDateTime(txtToDate.Text.Trim());
                dsHolidaysList = objHolidayBOL.bindHolidaysForLeaveApprovals(objHolidayModel);

                for (int i = 0; i < TotalLeaves; i++)
                {
                    if (FromDate.DayOfWeek.ToString() == "Saturday" || FromDate.DayOfWeek.ToString() == "Sunday")
                    {
                        count++;
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
                        if (!flag)
                        {
                            strLeaves[j] = FromDate.ToString();
                            cblGenerateDates.Items.Add(FromDate.Date.ToShortDateString());
                            j++;
                        }
                    }
                    FromDate = FromDate.AddDays(1);
                }
                count++;
                TotalLeavesApplyedFor = j++;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "lnkbtnGenerateDates_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Generates Dates

        #region Search link

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";

                lnkSearch.Visible = true;
                tdAddBulk.Visible = false;
                gvBulkEntries.Visible = true;
                lblSuccess.Text = "";
                lblError.Text = "";
                spanSearch.Visible = true;
                spanAddBulk.Visible = false;
                gvBulkEntries.EditIndex = -1;

                SearchData();
                ViewState["Search"] = 1;
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtReason.Text = "";

                tdGenerate.Visible = false;
                lblGenarate.Visible = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "lnkSearch_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Search link

        #region Submit

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                objSignInSignOutModel.EmployeeID = Convert.ToInt32(txtempid.Text);
                objSignInSignOutModel.SignInComment = "";
                objSignInSignOutModel.StatusID = Convert.ToInt32(2);
                objSignInSignOutModel.ApproverID = Convert.ToInt32(UserID);
                objSignInSignOutModel.ApproverComments = txtReason.Text.ToString();
                objSignInSignOutModel.IsSignInManual = Convert.ToInt32(1);
                objSignInSignOutModel.IsSignOutManual = Convert.ToInt32(1);
                objSignInSignOutModel.TotalHoursWorked = Convert.ToString("9:00:00");

                //getting frozen the date

                objLeaveDetailsModel.UserID = Convert.ToInt32(txtempid.Text);
                dsConfigItem = objLeaveDeatilsBOL.GetLeaveDetails(objLeaveDetailsModel);
                DateTime ConfigDate = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                if (cblGenerateDates.Items.Count == 0)
                {
                    lblGenarate.Visible = true;
                    return;
                }

                string date = " ";
                for (int k = 0; k < cblGenerateDates.Items.Count; k++)
                {
                    if (cblGenerateDates.Items[k].Selected)
                    {
                        if (ConfigDate.Date >= Convert.ToDateTime(txtFromDate.Text).Date)
                        {
                            lblError.Text = "Administrator has frozen the data from " + ConfigDate.Date.ToShortDateString() + ". Select another date";
                            return;
                        }
                        else
                        {
                            objSignInSignOutModel.SignInTime = Convert.ToDateTime(cblGenerateDates.Items[k].ToString());
                            objSignInSignOutModel.SignOutTime = Convert.ToDateTime(cblGenerateDates.Items[k].ToString());
                            objSignInSignOutModel.IsBulk = Convert.ToBoolean(1);
                            AddBulkEntriesDetails();
                            if (rowsAffected < 0)
                            {
                                lblError.Visible = true;
                                date += cblGenerateDates.Items[k].ToString() + ", ";
                                lblError.Text = date + "  dates are already added.";
                                lblSuccess.Text = "";
                            }
                            else
                            {
                                lblSuccess.Text = "Bulk Entries details Successfully submitted";
                            }
                        }
                    }
                }

                BindData();
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "btnSubmit", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "User has already applied for leaves for these dates.";
                }
            }
        }

        #endregion Submit

        #region Delete

        protected void gvBulkEntries_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string lblSignInSignOutID = ((Label)gvBulkEntries.Rows[e.RowIndex].FindControl("lblSignInSignOutID")).Text;
            objSignInSignOutModel.SignInSignOutID = Convert.ToInt32(lblSignInSignOutID);
            try
            {
                objBulkBOL.DeleteSignInSignOutDetails(objSignInSignOutModel);
                lblSuccess.Text = "Record Deleted Successfully";
                BindData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "gvBulkEntries_RowDeleting", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Delete

        #region Pageing

        protected void gvBulkEntries_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvBulkEntries.PageIndex = e.NewPageIndex;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "gvBulkEntries_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Pageing

        #region FillEmployee for Search

        public void FillEmployee()
        {
            try
            {
                objOutOfOfficeModel.UserId = Convert.ToInt32(UserID);
                dsEmployee = objOutOfOfficeBOL.GetEmployeeNameRpt(objOutOfOfficeModel);

                if (Roles.IsUserInRole("Admin"))
                {
                    for (int i = 0; i < dsEmployee.Tables[1].Rows.Count; i++)
                    {
                        ddlEmployeeNames.Items.Add(new ListItem(dsEmployee.Tables[1].Rows[i]["EmployeeName"].ToString(), dsEmployee.Tables[1].Rows[i]["UserID"].ToString()));
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "FillEmployeeNames", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion FillEmployee for Search

        #region Search

        protected void ddlEmployeeNames_SelectedIndexChanged(object sender, EventArgs e)
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "ddlEmployeeNames_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                objSignInSignOutModel.EmployeeID = Convert.ToInt32(ddlEmployeeNames.SelectedValue);
                objSignInSignOutModel.SignInTime = Convert.ToDateTime(txtSearchFromDate.Text);
                objSignInSignOutModel.SignOutTime = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                SearchBulkDetails();
                tdAddBulk.Visible = false;
                spanSearch.Visible = true;
                spanAddBulk.Visible = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "btnSearch_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Search

        #region SearchBulkDetails

        public void SearchBulkDetails()
        {
            try
            {
                dsSearchBulkDetails = objBulkBOL.SearchBulkDetails(objSignInSignOutModel);
                if (dsSearchBulkDetails.Tables[0].Rows.Count <= 0)
                {
                    lblError.Text = "Records Not Found";
                    gvBulkEntries.Visible = false;
                }
                else
                {
                    lblError.Text = "";
                    gvBulkEntries.Visible = true;
                    gvBulkEntries.DataSource = dsSearchBulkDetails.Tables[0];
                    gvBulkEntries.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "SearchLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion SearchBulkDetails

        #region SearchData

        public void SearchData()
        {
            try
            {
                if (ddlEmployeeNames.SelectedValue == "" || ddlEmployeeNames.SelectedValue == "0")
                {
                    objSignInSignOutModel.EmployeeID = Convert.ToInt32(UserID);

                    dsLoadSignInSigOutData = objBulkBOL.GetBulkEntries(objSignInSignOutModel);
                    if (dsLoadSignInSigOutData.Tables[0].Rows.Count > 0)
                    {
                        gvBulkEntries.Visible = true;
                    }
                    else if (dsLoadSignInSigOutData.Tables[0].Rows.Count == 0)
                    {
                        gvBulkEntries.DataSource = dsLoadSignInSigOutData.Tables[0];
                        gvBulkEntries.DataBind();
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "No Bulk Entries details.";
                    }
                    else
                    {
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "No Bulk Entries details.";
                    }
                }
                else
                {
                    objSignInSignOutModel.EmployeeID = Convert.ToInt32(ddlEmployeeNames.SelectedValue);
                    dsLoadSignInSigOutData = objBulkBOL.GetBulkEntriesForEmployees(objSignInSignOutModel);

                    if (dsLoadSignInSigOutData.Tables[0].Rows.Count > 0)
                    {
                        gvBulkEntries.DataSource = dsLoadSignInSigOutData.Tables[0];
                        gvBulkEntries.DataBind();
                    }
                    else if (dsLoadSignInSigOutData.Tables[0].Rows.Count == 0)
                    {
                        gvBulkEntries.DataSource = dsLoadSignInSigOutData.Tables[0];
                        gvBulkEntries.DataBind();
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "No Bulk Entries details.";
                    }
                    else
                    {
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "No Bulk Entries details.";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "SearchData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion SearchData

        #region Sorting

        protected void gvBulkEntries_Sorting(object sender, GridViewSortEventArgs e)
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
                    objSignInSignOutModel.EmployeeID = Convert.ToInt32(ddlEmployeeNames.SelectedValue);
                    objSignInSignOutModel.SignInTime = Convert.ToDateTime(txtSearchFromDate.Text);
                    objSignInSignOutModel.SignOutTime = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    dsLoadSignInSigOutData = objBulkBOL.SearchBulkDetails(objSignInSignOutModel);
                }
                gvBulkEntries.DataSource = dsLoadSignInSigOutData.Tables[0];
                gvBulkEntries.DataBind();
                DataTable dt = dsLoadSignInSigOutData.Tables[0];
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

                gvBulkEntries.DataSource = dv;
                gvBulkEntries.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "gvBulkEntries_Sorting", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Sorting

        #region RowDataBound

        protected void gvBulkEntries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblstatus = ((Label)(e.Row.FindControl("lblgrvStatusName")));
                    Label lblDate = ((Label)(e.Row.FindControl("lblDate")));
                    Label lblApproved = ((Label)(e.Row.FindControl("lblApproved")));
                    LinkButton lnkButDelete = ((LinkButton)(e.Row.FindControl("lnkButDelete")));
                    objOutOfOfficeModel.UserId = Convert.ToInt32(UserID);
                    dsConfigItem = objOutOfOfficeBOL.GetEmployeeNameRpt(objOutOfOfficeModel);

                    DateTime ConfigDate = Convert.ToDateTime(dsConfigItem.Tables[2].Rows[0]["ConfigItemValue"].ToString());
                    if (lblDate.Text != "")
                    {
                        if (ConfigDate.Date >= Convert.ToDateTime(lblDate.Text).Date)
                        {
                            if (lblstatus.Text == "Approved")
                            {
                                lblApproved.Visible = true;
                                lblApproved.Text = "Aprroved";
                                lnkButDelete.Visible = false;
                            }
                            else if (lblstatus.Text == "Cancel")
                            {
                                lblApproved.Visible = true;
                                lblApproved.Text = "Bulk Entry Cancelled";
                                lnkButDelete.Visible = false;
                            }
                            else if (lblstatus.Text == "Rejected")
                            {
                                lblApproved.Visible = true;
                                lblApproved.Text = "Bulk Entry Rejected";
                                lnkButDelete.Visible = false;
                            }
                            else if (lblstatus.Text == "Pending")
                            {
                                lblApproved.Visible = true;
                                lblApproved.Text = "Bulk Entry Pending";
                                lnkButDelete.Visible = false;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "gvBulkEntries_RowDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion RowDataBound

        #region Cancel

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //tdSearch.Visible = true;
                tdAddBulk.Visible = false;
                ddlEmployeeNames.SelectedValue = Convert.ToString("0");
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
                lblSuccess.Text = "";
                lblError.Text = "";
                //lnkAdd.Visible = true;
                SearchData();
                gvBulkEntries.Visible = true;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Cancel

        #region Link Add

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["Search"] = 0;

                tdAddBulk.Visible = true;
                lblSuccess.Text = "";
                lblError.Text = "";
                spanSearch.Visible = false;

                spanAddBulk.Visible = true;
                gvBulkEntries.EditIndex = -1;
                BindData();

                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtReason.Text = "";
                ddlEmployeeNames.SelectedValue = Convert.ToString("0");
                tdGenerate.Visible = false;
                lblGenarate.Visible = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "lnkAdd_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Link Add

        #region Todate

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cblGenerateDates.Items.Clear();
                tdGenerate.Visible = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "txtToDate_TextChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Todate

        #region FormDate

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cblGenerateDates.Items.Clear();
                tdGenerate.Visible = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "txtFromDate_TextChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion FormDate

        #region TextName changed

        protected void txtempname_TextChanged(object sender, EventArgs e)
        {
            try
            {
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
                        gvBulkEntries.Visible = false;
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
                    objLeaveTransactionModel.EmployeeName = txtempname.Text;
                    rowsAffected = objLeaveTransactionBOL.CheckEmployeeNameValidation(objLeaveTransactionModel);
                    if (rowsAffected <= 0)
                    {
                        lblError.Text = "Please Enter proper Employee Name";
                        txtempname.Text = "";
                        gvBulkEntries.Visible = false;
                    }
                    //txtempname is empty
                    else if (txtempname.Text.Trim() == "")
                    {
                        lblError.Text = "Please Enter proper Employee Name";
                        gvBulkEntries.Visible = false;
                    }
                    else
                    {
                        gvBulkEntries.Visible = true;

                        ViewState["Search"] = 0;
                        BindData();
                    }
                }

                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtReason.Text = "";
                tdGenerate.Visible = false;
                lblGenarate.Visible = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntries.aspx.cs", "txtempname_TextChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion TextName changed
    }
}