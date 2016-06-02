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
using System.IO;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace HRMS.Orbitweb
{
    public partial class ShiftMaster : System.Web.UI.UserControl
    {
        ShiftMasterBOL objShiftMasterBOL = new ShiftMasterBOL();
        ShiftMasterModel objShiftMasterModel = new ShiftMasterModel();
        DataSet dsShift = new DataSet();
        bool inEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "Shiftmaster.cs", "Page_Load", ex.StackTrace);
                throw new V2Exceptions();
            }


        }

        private void BindData()
        {
            try
            {
                dsShift = objShiftMasterBOL.bindData();
                grdShiftMaster.DataSource = dsShift;
                if (dsShift.Tables[0].Rows.Count > 0)
                {
                    grdShiftMaster.DataBind();


                    int i = 0;


                    foreach (DataGridItem rowitem in grdShiftMaster.Items)
                    {

                        DropDownList ddlReportingHourIN = (DropDownList)rowitem.Cells[0].FindControl("ddlReportingHourIN");
                        DropDownList ddlReportingMinuteIN = (DropDownList)rowitem.Cells[0].FindControl("ddlReportingMinuteIN");
                        DropDownList ddlReportingHourOUT = (DropDownList)rowitem.Cells[0].FindControl("ddlReportingHourOUT");
                        DropDownList ddlReportingMinuteOUT = (DropDownList)rowitem.Cells[0].FindControl("ddlReportingMinuteOUT");

                        if (ddlReportingHourIN != null)
                        {
                            //ddlReportingHourIN1.DataSource = dsShift;
                            DateTime RprtInTime = new DateTime();
                            RprtInTime = Convert.ToDateTime(dsShift.Tables[0].Rows[i]["ShiftInTime"].ToString());
                            int rpInTmHr = RprtInTime.Hour;
                            int rpInTmMin = RprtInTime.Minute;

                            ddlReportingHourIN.SelectedValue = Convert.ToString(rpInTmHr);
                            ddlReportingMinuteIN.SelectedValue = Convert.ToString(rpInTmMin);

                            DateTime RprtOutTime = new DateTime();
                            RprtOutTime = Convert.ToDateTime(dsShift.Tables[0].Rows[i]["ShiftOutTime"].ToString());
                            int rpOutTmHr = RprtOutTime.Hour;
                            int rpOutTmMin = RprtOutTime.Minute;

                            ddlReportingHourOUT.SelectedValue = Convert.ToString(rpOutTmHr);
                            ddlReportingMinuteOUT.SelectedValue = Convert.ToString(rpOutTmMin);
                            i++;
                        }
                        if (ddlReportingHourIN == null)
                        {

                            DropDownList ddlReportingHourIN1 = (DropDownList)rowitem.Cells[0].FindControl("ddlReportingHourIN2");
                            DropDownList ddlReportingMinuteIN1 = (DropDownList)rowitem.Cells[0].FindControl("ddlReportingMinuteIN2");
                            DropDownList ddlReportingHourOUT1 = (DropDownList)rowitem.Cells[0].FindControl("ddlReportingHourOUT2");
                            DropDownList ddlReportingMinuteOUT1 = (DropDownList)rowitem.Cells[0].FindControl("ddlReportingMinuteOUT2");
                            DateTime RprtInTime = new DateTime();
                            RprtInTime = Convert.ToDateTime(dsShift.Tables[0].Rows[i]["ShiftInTime"].ToString());
                            int rpInTmHr = RprtInTime.Hour;
                            int rpInTmMin = RprtInTime.Minute;

                            ddlReportingHourIN1.SelectedValue = Convert.ToString(rpInTmHr);
                            ddlReportingMinuteIN1.SelectedValue = Convert.ToString(rpInTmMin);

                            DateTime RprtOutTime = new DateTime();
                            RprtOutTime = Convert.ToDateTime(dsShift.Tables[0].Rows[i]["ShiftOutTime"].ToString());
                            int rpOutTmHr = RprtOutTime.Hour;
                            int rpOutTmMin = RprtOutTime.Minute;

                            ddlReportingHourOUT1.SelectedValue = Convert.ToString(rpOutTmHr);
                            ddlReportingMinuteOUT1.SelectedValue = Convert.ToString(rpOutTmMin);
                        }

                    }


                }
                else
                {
                    grdShiftMaster.Visible = false;
                }

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ShiftMaster.cs", "Bindata", ex.StackTrace);
                throw new V2Exceptions();
            }


        }
        protected void grdShiftMaster_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "btnAdd")
                {
                    grdShiftMaster.EditItemIndex = -1;
                    grdShiftMaster.ShowFooter = true;

                    TextBox txtShiftName = ((TextBox)(e.Item.FindControl("txtShiftName")));
                    //TextBox txtShiftDescription = ((TextBox)(e.Item.FindControl("txtShiftDescription")));
                    DropDownList ddlReportingHourIN1 = ((DropDownList)(e.Item.FindControl("ddlReportingHourIN1")));
                    DropDownList ddlReportingMinuteIN1 = ((DropDownList)(e.Item.FindControl("ddlReportingMinuteIN1")));
                    DropDownList ddlReportingHourOUT1 = ((DropDownList)(e.Item.FindControl("ddlReportingHourOUT1")));
                    DropDownList ddlReportingMinuteOUT1 = ((DropDownList)(e.Item.FindControl("ddlReportingMinuteOUT1")));
                    CheckBox chkIsactive1 = ((CheckBox)(e.Item.FindControl("chkIsactive1")));
                    objShiftMasterModel.shiftName = Convert.ToString(txtShiftName.Text);
                    //objShiftMasterModel.description = Convert.ToString(txtShiftDescription.Text);
                    string shiftInTime = DateTime.Now.ToShortDateString() + " " + ddlReportingHourIN1.SelectedValue.ToString() + ":" + ddlReportingMinuteIN1.SelectedValue.ToString() + ":" + "00";
                    objShiftMasterModel.shiftInTime = Convert.ToDateTime(shiftInTime);
                    string shiftOutTime = DateTime.Now.ToShortDateString() + " " + ddlReportingHourOUT1.SelectedValue.ToString() + ":" + ddlReportingMinuteOUT1.SelectedValue.ToString() + ":" + "00";
                    objShiftMasterModel.shiftOutTime = Convert.ToDateTime(shiftOutTime);
                    if (chkIsactive1.Checked == true)
                        objShiftMasterModel.isActive = true;
                    else
                        objShiftMasterModel.isActive = false;

                    AddShiftMaster();
                    grdShiftMaster.ShowFooter = true;
                    grdShiftMaster.EditItemIndex = -1;
                    BindData();
                }
                if (e.CommandName == "btnCancel")
                {
                    grdShiftMaster.EditItemIndex = -1;
                    BindData();
                    grdShiftMaster.ShowFooter = true;
                }


                if (e.CommandName == "Update")
                {
                    grdShiftMaster.EditItemIndex = -1;
                    grdShiftMaster.ShowFooter = true;

                    Label lblShiftID1 = ((Label)(e.Item.FindControl("lblShiftID2")));
                    TextBox txtShiftName = ((TextBox)(e.Item.FindControl("txtShiftName1")));
                    //TextBox txtShiftDescription = ((TextBox)(e.Item.FindControl("txtShiftDescription1")));
                    DropDownList ddlReportingHourIN1 = ((DropDownList)(e.Item.FindControl("ddlReportingHourIN2")));
                    DropDownList ddlReportingMinuteIN1 = ((DropDownList)(e.Item.FindControl("ddlReportingMinuteIN2")));
                    DropDownList ddlReportingHourOUT1 = ((DropDownList)(e.Item.FindControl("ddlReportingHourOUT2")));
                    DropDownList ddlReportingMinuteOUT1 = ((DropDownList)(e.Item.FindControl("ddlReportingMinuteOUT2")));
                    CheckBox chkIsactive1 = ((CheckBox)(e.Item.FindControl("chkIsactive2")));
                    objShiftMasterModel.shiftID = Convert.ToInt32(lblShiftID1.Text);
                    objShiftMasterModel.shiftName = Convert.ToString(txtShiftName.Text);
                    //objShiftMasterModel.description = Convert.ToString(txtShiftDescription.Text);
                    string shiftInTime = DateTime.Now.ToShortDateString() + " " + ddlReportingHourIN1.SelectedValue.ToString() + ":" + ddlReportingMinuteIN1.SelectedValue.ToString() + ":" + "00";
                    objShiftMasterModel.shiftInTime = Convert.ToDateTime(shiftInTime);
                    string shiftOutTime = DateTime.Now.ToShortDateString() + " " + ddlReportingHourOUT1.SelectedValue.ToString() + ":" + ddlReportingMinuteOUT1.SelectedValue.ToString() + ":" + "00";
                    objShiftMasterModel.shiftOutTime = Convert.ToDateTime(shiftOutTime);
                    if (chkIsactive1.Checked == true)
                        objShiftMasterModel.isActive = true;
                    else
                        objShiftMasterModel.isActive = false;

                    UpdateShiftDetails();
                    grdShiftMaster.ShowFooter = true;
                    grdShiftMaster.EditItemIndex = -1;
                    BindData();
                }

                if (e.CommandName == "Edit")
                {
                    inEdit = true;
                }

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ShiftMaster.cs", "grdShiftMaster_ItemCommand", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        private void UpdateShiftDetails()
        {
            try
            {
                int rowsAffected = objShiftMasterBOL.UpdateShiftMaster(objShiftMasterModel);
                if (rowsAffected > 0)
                {
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Record updated successfully";
                    lblError.Text = "";
                }
                else
                {
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Shift already exist";
                    lblError.Text = "";
                }

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "";
                lblSuccess.Text = "";
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidaysList.cs", "UpdateDepartmentDetails", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        private void AddShiftMaster()
        {
            try
            {
                int rowsAffected = objShiftMasterBOL.AddShiftMaster(objShiftMasterModel);
                if (rowsAffected > 0)
                {
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Record Added successfully";
                    lblError.Text = "";
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Record already exists";
                    lblSuccess.Text = "";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ShiftMaster.cs", "AddShiftMaster", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        protected void grdShiftMaster_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    TextBox txtShiftName = ((TextBox)(e.Item.FindControl("txtShiftName")));
                    LinkButton btnAdd = (LinkButton)e.Item.FindControl("btnAdd");
                    //TextBox txtShiftDescription = ((TextBox)(e.Item.FindControl("txtShiftDescription")));
                    btnAdd.Attributes.Add("onClick", "return validation(" + txtShiftName.ClientID + " );");

                }
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    TextBox txtShiftName = ((TextBox)(e.Item.FindControl("txtShiftName1")));
                    LinkButton lnkbutUpdate = (LinkButton)e.Item.FindControl("lnkbutUpdate");
                    //TextBox txtShiftDescription = ((TextBox)(e.Item.FindControl("txtShiftDescription1")));
                    lnkbutUpdate.Attributes.Add("onClick", "return validation(" + txtShiftName.ClientID + ");");

                }


            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ShiftMaster.cs", "grdShiftMaster_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        protected void grdShiftMaster_EditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                grdShiftMaster.EditItemIndex = e.Item.ItemIndex;

                BindData();
                grdShiftMaster.ShowFooter = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayList.aspx", "grdHolidayList_EditCommand", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        protected void grdShiftMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {


        }
        protected void grdShiftMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {


        }
        protected void grdShiftMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}