using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;

namespace HRMS.Orbitweb
{
    public partial class LeaveTransaction : Page
    {
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var PageName = "Leave Transaction";
                objpagelevel.PageLevelAccess(PageName);

                if (!IsPostBack)
                {
                }

                lblError.Text = "";
                lblSuccess.Text = "";
                trTotalLeave.Visible = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransaction.aspx.cs", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Page_Load

        #region BindLeaveTransaction

        public void bindLeaveTransaction()
        {
            try
            {
                objLeaveTransactionModel.UserID = Convert.ToInt32(txtempid.Text);
                dsGetLeaveTransaction = objLeaveTransactionBOL.dsGetLeaveTransaction(objLeaveTransactionModel);

                // If the dataset has no data ,to display gridview empty data.
                if (dsGetLeaveTransaction.Tables[0].Rows.Count == 0)
                {
                    dsGetLeaveTransaction.Tables[0].Rows.Add(dsGetLeaveTransaction.Tables[0].NewRow());
                    gvLeaveTransaction.DataSource = dsGetLeaveTransaction.Tables[0];
                    gvLeaveTransaction.DataBind();
                    gvLeaveTransaction.Rows[0].Visible = false;
                }
                else
                {
                    gvLeaveTransaction.Visible = true;
                    gvLeaveTransaction.DataSource = dsGetLeaveTransaction;
                    gvLeaveTransaction.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransaction.aspx.cs", "bindLeaveTransaction",
                    ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion BindLeaveTransaction

        #region gvLeaveTransaction_RowDataBound

        protected void gvLeaveTransaction_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    var txtDescription = ((TextBox)e.Row.FindControl("txtDescription"));
                    var strRemove = txtDescription.Text;
                    var SplitRemoveValue = strRemove.Split(':');
                    txtDescription.Text = Convert.ToString(SplitRemoveValue[1]);
                    objLeaveTransactionModel.Description = txtDescription.Text.Trim();
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var LeaveType = ((Label)e.Row.FindControl("lblLeaveType"));
                    if (LeaveType != null)
                    {
                        if (LeaveType.Text == "True")
                        {
                            LeaveType.Text = "Leave";
                        }
                        else if (LeaveType.Text == "False")
                        {
                            LeaveType.Text = "Compensatory";
                        }
                    }

                    var TransactionMode = ((Label)e.Row.FindControl("lblMode"));
                    if (TransactionMode.Text == "True")
                    {
                        TransactionMode.Text = "Manual";
                    }
                    else if (TransactionMode.Text == "False")
                    {
                        TransactionMode.Text = "Auto";
                    }

                    var lblAuto = ((Label)e.Row.FindControl("lblAuto"));
                    var lblMode = ((Label)e.Row.FindControl("lblMode"));
                    var lblTransDate = ((Label)e.Row.FindControl("lblTransDate"));

                    // To freeze the data
                    dsConfigItem = objLeaveTransactionBOL.dsGetLeaveTransaction(objLeaveTransactionModel);
                    var ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());
                    if (lblTransDate.Text != "")
                    {
                        if (ConfigdateTime.Date >= Convert.ToDateTime(lblTransDate.Text).Date)
                        {
                            if (lblMode.Text == "Auto")
                            {
                                lblAuto.Text = "Auto";
                                var lnkEdit = ((LinkButton)e.Row.FindControl("lnkEdit"));
                                var lnkDelete = ((LinkButton)e.Row.FindControl("lnkDelete"));
                                lnkDelete.Visible = false;
                                lnkEdit.Visible = false;
                            }
                            if (lblMode.Text == "Manual")
                            {
                                lblAuto.Text = "Manual";
                                var lnkEdit = ((LinkButton)e.Row.FindControl("lnkEdit"));
                                var lnkDelete = ((LinkButton)e.Row.FindControl("lnkDelete"));
                                lnkDelete.Visible = false;
                                lnkEdit.Visible = false;
                            }
                        }
                        else
                        {
                            if (lblMode.Text == "Auto")
                            {
                                lblAuto.Text = "Auto";
                                var lnkEdit = ((LinkButton)e.Row.FindControl("lnkEdit"));
                                var lnkDelete = ((LinkButton)e.Row.FindControl("lnkDelete"));
                                lnkDelete.Visible = false;
                                lnkEdit.Visible = false;
                            }
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    // using javasctipt to check the validataion

                    var txtTransDateAdd = ((TextBox)e.Row.FindControl("txtTransDateAdd"));
                    var txtDescriptionAdd = ((TextBox)e.Row.FindControl("txtDescriptionAdd"));
                    var txtQuantityAdd = ((TextBox)e.Row.FindControl("txtQuantityAdd"));
                    var btnAdd = ((LinkButton)e.Row.FindControl("lnkAdd"));
                    txtTransDateAdd.Attributes.Add("onkeydown", "return false");
                    btnAdd.Attributes.Add("onClick",
                        "return doValue('" + txtTransDateAdd.ClientID + "','" + txtDescriptionAdd.ClientID + "' ,'" +
                        txtQuantityAdd.ClientID + "');");
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    // to set textbox is readonly.
                    var txtTransDate = ((TextBox)e.Row.FindControl("txtTransDate"));
                    txtTransDate.Attributes.Add("onKeydown", "return false");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransaction.aspx.cs",
                    "gvLeaveTransaction_RowDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion gvLeaveTransaction_RowDataBound

        #region gvLeaveTransaction_RowCommand

        protected void gvLeaveTransaction_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    gvLeaveTransaction.PageIndex = 0;
                    var txtTransDateAdd = ((TextBox)gvLeaveTransaction.FooterRow.FindControl("txtTransDateAdd"));
                    var txtDescriptionAdd = ((TextBox)gvLeaveTransaction.FooterRow.FindControl("txtDescriptionAdd"));
                    var txtQuantityAdd = ((TextBox)gvLeaveTransaction.FooterRow.FindControl("txtQuantityAdd"));
                    var ddlLeaveTypeAdd = ((DropDownList)gvLeaveTransaction.FooterRow.FindControl("ddlLeaveTypeAdd"));

                    objLeaveTransactionModel.TransactionDate = Convert.ToDateTime(txtTransDateAdd.Text).Date;
                    objLeaveTransactionModel.Description = txtDescriptionAdd.Text.Trim();
                    objLeaveTransactionModel.Quantity = Convert.ToDecimal(txtQuantityAdd.Text.Trim());
                    objLeaveTransactionModel.UserID = Convert.ToInt32(txtempid.Text);

                    if (ddlLeaveTypeAdd.SelectedValue == "0")
                        objLeaveTransactionModel.LeaveType = false;
                    else if (ddlLeaveTypeAdd.SelectedValue == "1")
                        objLeaveTransactionModel.LeaveType = true;

                    objLeaveTransactionModel.TransactionMode = Convert.ToBoolean(1);

                    dsConfigItem = objLeaveTransactionBOL.dsGetLeaveTransaction(objLeaveTransactionModel);
                    var ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                    if (ConfigdateTime.Date >= Convert.ToDateTime(txtTransDateAdd.Text).Date)
                    {
                        lblError.Text = "Administrator has frozen the data from " +
                                        ConfigdateTime.Date.ToShortDateString() +
                                        " . It was freezed.Please Select another date.";
                    }
                    else
                    {
                        if (objLeaveTransactionModel.Quantity < 0)
                        {
                            objLeaveTransactionModel.Description = "Manually Debited:" + txtDescriptionAdd.Text.Trim();
                            objLeaveTransactionBOL.AddLeaveTransactionAdmin(objLeaveTransactionModel);
                            objLeaveTransactionBOL.UpdateLeaveBalance(objLeaveTransactionModel);
                        }
                        else
                        {
                            objLeaveTransactionModel.Description = " Manually Credited:" + txtDescriptionAdd.Text.Trim();
                            objLeaveTransactionBOL.AddLeaveTransactionAdmin(objLeaveTransactionModel);
                            objLeaveTransactionBOL.UpdateLeaveBalance(objLeaveTransactionModel);
                        }

                        lblSuccess.Text = "Record added successfully";
                        gvLeaveTransaction.EditIndex = -1;

                        bindLeaveTransaction();

                        getTotalLeave();
                        trTotalLeave.Visible = true;
                    }
                }
                else if (e.CommandName == "Cancel")
                {
                    gvLeaveTransaction.EditIndex = -1;
                    bindLeaveTransaction();
                    getTotalLeave();
                    trTotalLeave.Visible = true;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (ex.Message.CompareTo(" have no leave balance.So not allowed to enter the Negative values.") != 0)
                {
                    var objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransaction.aspx.cs",
                        "gvLeaveTransaction_RowCommand", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                lblError.Text = txtempname.Text + ex.Message;
            }
        }

        #endregion gvLeaveTransaction_RowCommand

        #region gvLeaveTransaction_RowDeleting

        protected void gvLeaveTransaction_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                var lblleavetransactionID =
                    ((Label)gvLeaveTransaction.Rows[e.RowIndex].FindControl("lblleavetransactionID"));
                objLeaveTransactionModel.LeaveTransactionID = Convert.ToInt32(lblleavetransactionID.Text);
                objLeaveTransactionBOL.DeleteLeaveTransactionAdmin(objLeaveTransactionModel);
                gvLeaveTransaction.ShowFooter = true;
                bindLeaveTransaction();
                getTotalLeave();
                trTotalLeave.Visible = true;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (ex.Message.CompareTo("As Leave balance going negative not allowed to delete this entry.") != 0)
                {
                    var objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransaction.aspx.cs",
                        "gvLeaveTransaction_RowDeleting", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                lblError.Text = ex.Message;
                getTotalLeave();
            }
        }

        #endregion gvLeaveTransaction_RowDeleting

        #region gvLeaveTransaction_RowEditing

        protected void gvLeaveTransaction_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvLeaveTransaction.EditIndex = e.NewEditIndex;
                gvLeaveTransaction.ShowFooter = false;
                bindLeaveTransaction();
                trTotalLeave.Visible = true;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransaction.aspx.cs",
                    "gvLeaveTransaction_RowEditing", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion gvLeaveTransaction_RowEditing

        #region gvLeaveTransaction_RowUpdating

        protected void gvLeaveTransaction_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var lblleavetransactionID =
                    ((Label)gvLeaveTransaction.Rows[e.RowIndex].FindControl("lblleavetransactionID"));
                objLeaveTransactionModel.LeaveTransactionID = Convert.ToInt32(lblleavetransactionID.Text);
                var txtTransDate = ((TextBox)gvLeaveTransaction.Rows[e.RowIndex].FindControl("txtTransDate"));
                objLeaveTransactionModel.TransactionDate = Convert.ToDateTime(txtTransDate.Text).Date;
                var txtDescription = ((TextBox)gvLeaveTransaction.Rows[e.RowIndex].FindControl("txtDescription"));
                objLeaveTransactionModel.Description = txtDescription.Text.Trim();
                var lblQuantity = ((Label)gvLeaveTransaction.Rows[e.RowIndex].FindControl("lblQuantity"));
                objLeaveTransactionModel.Quantity = Convert.ToDecimal(lblQuantity.Text);

                dsConfigItem = objLeaveTransactionBOL.dsGetLeaveTransaction(objLeaveTransactionModel);
                var ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                if (ConfigdateTime.Date >= Convert.ToDateTime(txtTransDate.Text).Date)
                {
                    lblError.Text = "Administrator has frozen the data from " + txtTransDate.Text +
                                    " .Select another date.";
                    txtTransDate.Text = "";
                }
                else
                {
                    if (objLeaveTransactionModel.Quantity < 0)
                    {
                        objLeaveTransactionModel.Description = "Manually Debited : " + txtDescription.Text.Trim();
                        objLeaveTransactionBOL.UpdateLeaveTransactionAdmin(objLeaveTransactionModel);
                        lblSuccess.Text = "Record updated sucessfully";
                        gvLeaveTransaction.ShowFooter = true;
                        gvLeaveTransaction.EditIndex = -1;
                        bindLeaveTransaction();
                    }
                    else
                    {
                        objLeaveTransactionModel.Description = " Manually Credited : " + txtDescription.Text.Trim();
                        objLeaveTransactionBOL.UpdateLeaveTransactionAdmin(objLeaveTransactionModel);
                        lblSuccess.Text = "Record updated sucessfully";
                        gvLeaveTransaction.ShowFooter = true;
                        gvLeaveTransaction.EditIndex = -1;
                        bindLeaveTransaction();
                        trTotalLeave.Visible = true;
                    }
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (ex.Message.CompareTo(" have no leave balance.So not allowed to enter the Negative values.") != 0)
                {
                    var objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransaction.aspx.cs",
                        "gvLeaveTransaction_RowUpdating", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
                lblError.Text = txtempname.Text + ex.Message;
            }
        }

        #endregion gvLeaveTransaction_RowUpdating

        #region gvLeaveTransaction_Sorting

        protected void gvLeaveTransaction_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                objLeaveTransactionModel.UserID = Convert.ToInt32(User.Identity.Name);
                bindLeaveTransaction();

                var dt = dsGetLeaveTransaction.Tables[0];
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
                gvLeaveTransaction.DataSource = dv;
                gvLeaveTransaction.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransaction.aspx.cs", "gvLeaveTransaction_Sorting",
                    ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion gvLeaveTransaction_Sorting

        #region gvLeaveTransaction_PageIndexChanging

        protected void gvLeaveTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvLeaveTransaction.PageIndex = e.NewPageIndex;
                gvLeaveTransaction.EditIndex = -1;
                bindLeaveTransaction();

                trTotalLeave.Visible = true;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransaction.aspx.cs",
                    "gvLeaveTransaction_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion gvLeaveTransaction_PageIndexChanging

        #region gvLeaveTransaction_RowCancelingEdit

        protected void gvLeaveTransaction_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvLeaveTransaction.EditIndex = -1;
                gvLeaveTransaction.ShowFooter = true;
                bindLeaveTransaction();

                trTotalLeave.Visible = true;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransaction.aspx.cs",
                    "gvLeaveTransaction_RowCancelingEdit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion gvLeaveTransaction_RowCancelingEdit

        #region getTotalLeave

        public void getTotalLeave()
        {
            try
            {
                objLeaveTransactionModel.UserID = Convert.ToInt32(txtempid.Text);
                dsGetTotalLeave = objLeaveTransactionBOL.GetTotalLeave(objLeaveTransactionModel);
                if (dsGetTotalLeave.Tables[0].Rows.Count <= 0)
                {
                    trTotalLeave.Visible = true;
                    lblTotalDisplay.Text = "0.0";
                }
                else
                {
                    lblTotalDisplay.Text = Convert.ToString(dsGetTotalLeave.Tables[0].Rows[0]["TotalLeave"].ToString());
                    var TotalLeaves = Convert.ToString(lblTotalDisplay.Text.StartsWith("-"));
                    if (TotalLeaves == "True")
                    {
                        lblTotalDisplay.Text = "0.0";
                    }
                    trTotalLeave.Visible = true;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransaction.aspx.cs", "getTotalLeave",
                    ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion getTotalLeave

        #region Autocompleteextender Textbox

        protected void txtempname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                gvLeaveTransaction.PageIndex = 0;
                var txtempname = (TextBox)sender;
                var txtempid = txtempname.Parent.FindControl("txtempid") as TextBox;

                if (txtempid != null)
                {
                    empid = txtempname.Text;
                    var str = txtempid.Text;
                    // split userid and employee name
                    var SplitEmpName = empid.Split(' ');

                    if (SplitEmpName.Length == 1)
                    {
                        lblError.Text = "Please Enter proper Employee Name";
                        gvLeaveTransaction.Visible = false;
                        return;
                    }
                    txtempid.Text = Convert.ToString(SplitEmpName[0]);
                    txtempname.Text = SplitEmpName[1];
                    for (var i = 2; i < SplitEmpName.Length; i++)
                    {
                        txtempname.Text += " " + SplitEmpName[i];
                    }
                    // To check validataion for employee name
                    objLeaveTransactionModel.EmployeeName = txtempname.Text;
                    var rowsAffected = objLeaveTransactionBOL.CheckEmployeeNameValidation(objLeaveTransactionModel);
                    if (rowsAffected <= 0)
                    {
                        lblError.Text = "Please Enter proper Employee Name";
                        gvLeaveTransaction.Visible = false;
                    }
                    //txtempname is empty
                    else if (txtempname.Text.Trim() == "")
                    {
                        lblError.Text = "Please Enter proper Employee Name";

                        gvLeaveTransaction.Visible = false;
                    }
                    else
                    {
                        gvLeaveTransaction.Visible = true;
                        objLeaveTransactionModel.UserID = Convert.ToInt32(txtempid.Text);
                        gvLeaveTransaction.EditIndex = -1;
                        bindLeaveTransaction();
                        getTotalLeave();
                    }
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveTransaction.aspx.cs", "txtempname_TextChanged",
                    ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Autocompleteextender Textbox

        protected void gvLeaveTransaction_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        #region Variable declaration

        private readonly LeaveTransactionBOL objLeaveTransactionBOL = new LeaveTransactionBOL();
        private readonly LeaveTransactionModel objLeaveTransactionModel = new LeaveTransactionModel();
        private OutOfOfficeModel objOutOfOfficeModel = new OutOfOfficeModel();
        private OutOfOfficeBOL objOutOfOfficeBOL = new OutOfOfficeBOL();

        private DataSet dsGetLeaveTransaction = new DataSet();
        private DataSet dsGetTotalLeave = new DataSet();
        private DataSet dsGetEmployeeName = new DataSet();
        private DataSet dsConfigItem = new DataSet();
        private DataSet dsCheckEmployeeName = new DataSet();
        private string empid = "";
        private readonly HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        #endregion Variable declaration
    }
}