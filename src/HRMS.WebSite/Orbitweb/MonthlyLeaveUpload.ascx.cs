using System;
using System.Data;
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


namespace HRMS.Orbitweb
{
    public partial class MonthlyLeaveUpload : System.Web.UI.UserControl
    {
        MonthlyLeaveUploadModel objMonthlyLeaveUploadModel = new MonthlyLeaveUploadModel();
        MonthlyLeaveUploadBOL objMonthlyLeaveUploadBOL = new MonthlyLeaveUploadBOL();
        DataSet dsMonthlyLeaveUpload = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblSuccess.Text = "";
                if (!IsPostBack)
                {
                    BindYear();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUpload.cs", "Page_load", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        #region BindYear
        public void BindYear()
        {
            try
            {
                for (int i = 2006; i <= System.DateTime.Now.Year + 1; i++)
                {
                    DDlYear.Items.Add(i.ToString());

                }
                for (int count = 0; count < DDlYear.Items.Count; count++)
                {
                    if (Convert.ToInt32(DDlYear.Items[count].Text) == DateTime.Now.Year)
                    {
                        DDlYear.Items[count].Selected = true;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUpload.cs", "bindYear", ex.StackTrace);
                throw new V2Exceptions();
            }

        }
        #endregion

        public void BindData()
        {
            try
            {
                if (DDlYear.SelectedIndex != 0)
                {
                    objMonthlyLeaveUploadModel.LeaveYear = Convert.ToInt32(DDlYear.SelectedItem.Text);
                }
                else
                {
                    objMonthlyLeaveUploadModel.LeaveYear = 0;
                }
                dsMonthlyLeaveUpload = objMonthlyLeaveUploadBOL.BindData(objMonthlyLeaveUploadModel);
                if (dsMonthlyLeaveUpload.Tables[0].Rows.Count > 0)
                {
                    grdMonthlyLeaveUpload.DataSource = dsMonthlyLeaveUpload;
                    grdMonthlyLeaveUpload.DataBind();
                    if (grdMonthlyLeaveUpload.PageCount > 1)
                    {
                        //grdMonthlyLeaveUpload.PagerStyle.Visible = true;
                    }
                    else
                    {
                        //grdMonthlyLeaveUpload.PagerStyle.Visible = false;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUpload.cs", "BindData", ex.StackTrace);
                throw new V2Exceptions();
            }
        }


        protected void grdMonthlyLeaveUpload_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Update")
                {

                }
                if (e.CommandName == "lnkAdd")
                {
                    grdMonthlyLeaveUpload.EditIndex = -1;
                    grdMonthlyLeaveUpload.ShowFooter = true;
                    DropDownList ddlMonth = ((DropDownList)(grdMonthlyLeaveUpload.FooterRow.FindControl("ddlMonth1")));
                    DropDownList ddlYear = ((DropDownList)(grdMonthlyLeaveUpload.FooterRow.FindControl("ddlYear1")));
                    TextBox txtdays = ((TextBox)(grdMonthlyLeaveUpload.FooterRow.FindControl("txtdays1")));
                    objMonthlyLeaveUploadModel.LeaveMonth = ddlMonth.SelectedItem.Text;
                    objMonthlyLeaveUploadModel.LeaveYear = Convert.ToInt32(ddlYear.SelectedItem.Text);
                    objMonthlyLeaveUploadModel.LeaveDays = Convert.ToDouble(txtdays.Text);
                    DateTime date;
                    date = Convert.ToDateTime(ddlMonth.SelectedItem.Text + '-' + ddlYear.SelectedItem.Text);
                    if (date > System.DateTime.Now)
                    {
                        AddNewMonthlyLeaveDetails();
                        grdMonthlyLeaveUpload.EditIndex = -1;
                        BindData();
                        grdMonthlyLeaveUpload.ShowFooter = true;
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "MonthlyLeaveUpload  is not allowed for Previous date ";
                        lblSuccess.Visible = false;
                        ddlMonth.SelectedIndex = 0;
                        ddlYear.SelectedIndex = 0;
                        txtdays.Text = "";
                    }
                    //grdMonthlyLeaveUpload.ShowFooter = true;
                }
                if (e.CommandName == "lnkCancel")
                {
                    grdMonthlyLeaveUpload.EditIndex = -1;
                    BindData();
                    grdMonthlyLeaveUpload.ShowFooter = true;
                }
                if (e.CommandName == "Edit")
                {
                    //grdMonthlyLeaveUpload.EditIndex =  e.NewEditIndex;
                    //grdMonthlyLeaveUpload.ShowFo oter = false;
                    //BindData();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUpload.cs", "grdMonthlyLeaveUpload_RowCommand", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        protected void grdMonthlyLeaveUpload_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //LinkButton lnkEdit = ((LinkButton)(e.Row.FindControl("lnkEdit")));
                    Label lblLeaveYear = ((Label)e.Row.FindControl("lblLeaveYear"));
                    Label lblLeaveMonth = ((Label)e.Row.FindControl("lblLeaveMonth"));
                    DateTime date = Convert.ToDateTime(lblLeaveMonth.Text + "-" + lblLeaveYear.Text);
                    if (date < DateTime.Now)
                    {
                        LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
                        lnkEdit.Enabled = false;
                    }
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    LinkButton lnkAdd1 = ((LinkButton)(e.Row.FindControl("lnkAdd")));
                    TextBox txtDays = ((TextBox)e.Row.FindControl("txtdays1"));
                    DropDownList ddlyear = ((DropDownList)e.Row.FindControl("ddlYear1"));
                    DropDownList ddlmonth = ((DropDownList)e.Row.FindControl("ddlMonth1"));
                    for (int i = 2006; i <= System.DateTime.Now.Year + 1; i++)
                    {
                        ddlyear.Items.Add(i.ToString());

                    }
                    for (int count = 0; count < ddlyear.Items.Count; count++)
                    {
                        if (Convert.ToInt32(DDlYear.Items[count].Text) == DateTime.Now.Year)
                        {
                            ddlyear.Items[count].Selected = true;
                        }
                    }
                    lnkAdd1.Attributes.Add("onClick", "return ValidateAdd(" + txtDays.ClientID + " , " + ddlyear.ClientID + " , " + ddlmonth.ClientID + ");");

                }


                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    TextBox txtdays = ((TextBox)e.Row.FindControl("txtdays"));
                    LinkButton lnkUpdate = ((LinkButton)(e.Row.FindControl("lnkUpdate")));
                    lnkUpdate.Attributes.Add("onClick", "return Validate(" + txtdays.ClientID + ");");

                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUpload.cs", "grdMonthlyLeaveUpload_RowDataBound", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        protected void grdMonthlyLeaveUpload_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdMonthlyLeaveUpload.PageIndex = e.NewPageIndex;
                grdMonthlyLeaveUpload.EditIndex = -1;
                BindData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUpload.cs", "grdMonthlyLeaveUpload_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        protected void grdMonthlyLeaveUpload_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = grdMonthlyLeaveUpload.Rows[e.RowIndex];
                Label lblUploadYearID = row.FindControl("lblUploadYearID") as Label;
                Label lblMonth = row.FindControl("lblLeaveMonth") as Label;
                Label lblYear = row.FindControl("lblLeaveYear") as Label;
                TextBox txtdays = row.FindControl("txtdays") as TextBox;
                objMonthlyLeaveUploadModel.MonthlyLeaveUploadId = Convert.ToInt32(lblUploadYearID.Text);
                objMonthlyLeaveUploadModel.LeaveMonth = lblMonth.Text;
                objMonthlyLeaveUploadModel.LeaveYear = Convert.ToInt32(lblYear.Text);
                objMonthlyLeaveUploadModel.LeaveDays = Convert.ToDouble(txtdays.Text);
                UpdateNewMonthlyLeaveDetails();
                grdMonthlyLeaveUpload.ShowFooter = true;
                grdMonthlyLeaveUpload.EditIndex = -1;
                BindData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUpload.cs", "grdMonthlyLeaveUpload_RowUpdating", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        protected void grdMonthlyLeaveUpload_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                grdMonthlyLeaveUpload.EditIndex = -1;
                grdMonthlyLeaveUpload.ShowFooter = true;
                BindData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUpload.cs", "grdMonthlyLeaveUpload_RowCancelingEdit", ex.StackTrace);
                throw new V2Exceptions();
            }
        }


        #region AddNewMonthlyLeaveDetails
        public void AddNewMonthlyLeaveDetails()
        {
            try
            {

                int rowsAffected = objMonthlyLeaveUploadBOL.AddNewMonthlyLeaveDetails(objMonthlyLeaveUploadModel);
                if (rowsAffected > 0)
                {
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Record added successfully";
                    lblError.Visible = false;
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "MonthlyLeaveUpload already exists or is not allowed for Previous date";
                    lblSuccess.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUpload.cs", "AddNewMonthlyLeaveDetails", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        #endregion

        #region UpdateNewMonthlyLeaveDetails
        public void UpdateNewMonthlyLeaveDetails()
        {
            try
            {
                int rowsAffected = objMonthlyLeaveUploadBOL.UpdateNewMonthlyLeaveDetails(objMonthlyLeaveUploadModel);
                if (rowsAffected > 0)
                {
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Record updated successfully";
                    lblError.Visible = false;
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "MonthlyLeaveUpload already exists";
                    lblSuccess.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                lblError.Visible = true;
                lblError.Text = "MonthlyLeaveUpload already exists";
                lblSuccess.Visible = false;
                throw;
            }
            catch (System.Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "MonthlyLeaveUpload already exists";
                lblSuccess.Visible = false;
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUpload.cs", "UpdateDepartmentDetails", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        #endregion


        public void grdMonthlyLeaveUpload_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                grdMonthlyLeaveUpload.EditIndex = e.NewEditIndex;
                grdMonthlyLeaveUpload.ShowFooter = false;
                BindData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUpload.cs", "grdMonthlyLeaveUpload_RowEditing", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        protected void DDlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(DDlYear.SelectedValue) < System.DateTime.Now.Year)
                {
                    //grdMonthlyLeaveUpload.ShowFooter = false;
                }
                else
                {
                    grdMonthlyLeaveUpload.ShowFooter = true;
                }
                objMonthlyLeaveUploadModel.LeaveYear = Convert.ToInt32(DDlYear.SelectedItem.Text);
                dsMonthlyLeaveUpload = objMonthlyLeaveUploadBOL.BindData(objMonthlyLeaveUploadModel);
                if (dsMonthlyLeaveUpload.Tables[0].Rows.Count > 0)
                {
                    grdMonthlyLeaveUpload.Visible = true;
                    grdMonthlyLeaveUpload.DataSource = dsMonthlyLeaveUpload;
                    grdMonthlyLeaveUpload.DataBind();

                }
                else
                {
                    grdMonthlyLeaveUpload.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "No holidays in this Year";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUpload.cs", "DDlYear_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
    }
}