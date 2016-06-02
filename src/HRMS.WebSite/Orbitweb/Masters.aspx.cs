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
    public partial class Masters : System.Web.UI.Page
    {
        HolidayMasterBOL objHolidayMasterBOL = new HolidayMasterBOL();
        HolidayMasterModel objHolidayMastermodel = new HolidayMasterModel();
        DataSet dsHoliday = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // grdHolidayList.Visible = false;
                lblError.Text = "";
                lblSuccess.Text = "";

                if (!IsPostBack)
                {
                    BindYear();

                    bindData();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayList.cs", "Page_Load", ex.StackTrace);
                throw new V2Exceptions();
            }

        }

        #region BindYear
        public void BindYear()
        {
            try
            {
                DDlYear.Items.Clear();
                DDlYear.Items.Add(new ListItem("-- Select --", "0"));
                for (int i = 2008; i <= System.DateTime.Now.Year + 1; i++)
                {
                    DDlYear.Items.Add(i.ToString());
                }
                for (int count = 0; count < DDlYear.Items.Count; count++)
                {
                    if (DDlYear.Items[count].Text.CompareTo("-- Select --") != 0)
                    {
                        if (Convert.ToInt32(DDlYear.Items[count].Text) == DateTime.Now.Year)
                        {
                            DDlYear.Items[count].Selected = true;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayList.cs", "bindYear", ex.StackTrace);
                throw new V2Exceptions();
            }

        }
        #endregion
        public void bindData()
        {
            try
            {
                if (DDlYear.SelectedIndex != 0)
                {
                    objHolidayMastermodel.Year = Convert.ToInt32(DDlYear.SelectedItem.Text);
                }
                else
                {
                    //objHolidayMastermodel.Year = 0;
                    lblError.Text = "Please select the Year";
                    lblSuccess.Text = "";
                }
                dsHoliday = objHolidayMasterBOL.searchHolidayList(objHolidayMastermodel);
                grdHolidayList.DataSource = dsHoliday;
                if (dsHoliday.Tables[0].Rows.Count > 0)
                {
                    grdHolidayList.DataBind();

                }
                else
                {
                    grdHolidayList.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayList.cs", "bindData", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        protected void grdHolidayList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "btnAdd")
                {

                    grdHolidayList.EditItemIndex = -1;
                    grdHolidayList.ShowFooter = true;
                    // TextBox lblHolidayID = ((TextBox)(e.Item.FindControl("txtHolidayID")));
                    TextBox txtHolidayDate = ((TextBox)(e.Item.FindControl("txtHolidayDate")));
                    TextBox txtHolidayDescription = ((TextBox)(e.Item.FindControl("txtHolidayDescription")));
                    CheckBox chkIsForShift = ((CheckBox)(e.Item.FindControl("chkIsForShiftFooter")));
                    // objHolidayMastermodel.HolidayID =Convert.ToInt32( lblHolidayID.Text);
                    objHolidayMastermodel.HolidayDate = Convert.ToDateTime(txtHolidayDate.Text);
                    objHolidayMastermodel.HolidayDescription = txtHolidayDescription.Text;
                    if (chkIsForShift.Checked == true)
                        objHolidayMastermodel.isHolidayForShift = true;
                    else
                        objHolidayMastermodel.isHolidayForShift = false;

                    if (Convert.ToDateTime(txtHolidayDate.Text).DayOfWeek.ToString() == "Saturday" || Convert.ToDateTime(txtHolidayDate.Text).DayOfWeek.ToString() == "Sunday")
                    {
                        lblError.Text = "Holidays  are not allowed on Saturday and Sunday  ";
                        lblSuccess.Text = "";
                        txtHolidayDate.Text = "";
                        txtHolidayDescription.Text = "";
                    }
                    else if (Convert.ToDateTime(txtHolidayDate.Text) > DateTime.Now)
                    {
                        AddNewDepartmentDetails();
                        //Reset Page Controls
                        //dgrddepartment.CurrentPageIndex=0;
                        grdHolidayList.EditItemIndex = -1;
                        bindData();
                        grdHolidayList.ShowFooter = true;
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Holidays  are not allowed for Previous date ";
                        lblSuccess.Text = "";
                        txtHolidayDate.Text = "";
                        txtHolidayDescription.Text = "";
                    }
                }
                if (e.CommandName == "btnCancel")
                {
                    grdHolidayList.EditItemIndex = -1;
                    bindData();
                    grdHolidayList.ShowFooter = true;
                }

                if (e.CommandName == "Update")
                {

                    Label lblHolidayID = ((Label)(e.Item.FindControl("lblHolidayID2")));
                    TextBox txtHolidayDate = ((TextBox)(e.Item.FindControl("txtHolidayDate1")));
                    TextBox txtHolidayDescription = ((TextBox)(e.Item.FindControl("txtHolidayDescription1")));
                    CheckBox chkIsForShift = ((CheckBox)(e.Item.FindControl("chkIsForShiftEdit")));
                    objHolidayMastermodel.HolidayID = Convert.ToInt32(lblHolidayID.Text);
                    objHolidayMastermodel.HolidayDate = Convert.ToDateTime(txtHolidayDate.Text);
                    objHolidayMastermodel.HolidayDescription = txtHolidayDescription.Text;
                    if (chkIsForShift.Checked == true)
                        objHolidayMastermodel.isHolidayForShift = true;
                    else
                        objHolidayMastermodel.isHolidayForShift = false;

                    if (Convert.ToDateTime(txtHolidayDate.Text).DayOfWeek.ToString() == "Saturday" || Convert.ToDateTime(txtHolidayDate.Text).DayOfWeek.ToString() == "Sunday")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Holidays  are not allowed in Saturday and Sunday  ";
                        lblSuccess.Text = "";
                        txtHolidayDate.Text = "";
                        txtHolidayDescription.Text = "";
                        grdHolidayList.ShowFooter = true;
                        grdHolidayList.EditItemIndex = -1;
                        bindData();
                    }
                    else
                    {
                        UpdateDepartmentDetails();
                        grdHolidayList.ShowFooter = true;
                        grdHolidayList.EditItemIndex = -1;
                        bindData();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayList.cs", "dgrddepartment_ItemCommand", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        #region AddNewDepartmentDetails
        public void AddNewDepartmentDetails()
        {
            try
            {

                int rowsAffected = objHolidayMasterBOL.AddNewDepartmentDetails(objHolidayMastermodel);
                if (rowsAffected > 0)
                {
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Record added successfully";
                    lblError.Text = "";
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Holidays date already exists ";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidaysList.cs", "AddNewDepartmentDetails", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        #endregion

        #region UpdateDepartmentDetails
        public void UpdateDepartmentDetails()
        {
            try
            {
                int rowsAffected = objHolidayMasterBOL.UpdateDepartmentDetails(objHolidayMastermodel);
                if (rowsAffected > 0)
                {
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Record updated successfully";
                    lblError.Text = "";
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Holidays date already exists";
                    lblSuccess.Text = "";
                }
            }
            catch (V2Exceptions ex)
            {
                lblError.Visible = true;
                lblError.Text = "Holidays date already exists";
                lblSuccess.Text = "";
                throw;
            }
            catch (System.Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Holidays date already exists";
                lblSuccess.Text = "";
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidaysList.cs", "UpdateDepartmentDetails", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        #endregion

        protected void grdHolidayList_EditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                grdHolidayList.EditItemIndex = e.Item.ItemIndex;
                bindData();
                grdHolidayList.ShowFooter = false;
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

        protected void grdHolidayList_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {

                    Label lblHolidayDate = (Label)e.Item.FindControl("lblHolidayDate");
                    lblHolidayDate.Text = lblHolidayDate.Text.Replace("12:00:00 AM", " ");// Substring(0, 13);// Replace("12:00:00 AM ", "");
                    if (Convert.ToDateTime(lblHolidayDate.Text) < DateTime.Now)
                    {
                        LinkButton lnkbutEdit = (LinkButton)e.Item.FindControl("lnkbutEdit");
                        lnkbutEdit.Enabled = false;
                    }
                }
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    TextBox txtHolidayDate = ((TextBox)(e.Item.FindControl("txtHolidayDate")));
                    LinkButton btnAdd = (LinkButton)e.Item.FindControl("btnAdd");
                    TextBox txtHolidayDescription = ((TextBox)(e.Item.FindControl("txtHolidayDescription")));
                    btnAdd.Attributes.Add("onClick", "return validation(" + txtHolidayDescription.ClientID + " , " + txtHolidayDate.ClientID + ");");
                    txtHolidayDate.Attributes.Add("onkeydown", "return false");
                }
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    TextBox txtHolidayDate = ((TextBox)(e.Item.FindControl("txtHolidayDate1")));
                    LinkButton lnkbutUpdate = (LinkButton)e.Item.FindControl("lnkbutUpdate");
                    TextBox txtHolidayDescription = ((TextBox)(e.Item.FindControl("txtHolidayDescription1")));
                    txtHolidayDate.Text = txtHolidayDate.Text.Replace("12:00:00 AM", " ");
                    lnkbutUpdate.Attributes.Add("onClick", "return validation(" + txtHolidayDescription.ClientID + "," + txtHolidayDate.ClientID + ");");
                    txtHolidayDate.Attributes.Add("onkeydown", "return false");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayList.cs", "grdHolidayList_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions();
            }

        }


        protected void DDlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grdHolidayList.CurrentPageIndex = 0;
                objHolidayMastermodel.Year = Convert.ToInt32(DDlYear.SelectedItem.Text);
                dsHoliday = objHolidayMasterBOL.searchHolidayList(objHolidayMastermodel);
                if (Convert.ToInt32(DDlYear.SelectedItem.Text) < DateTime.Now.Year)
                {
                    grdHolidayList.ShowFooter = false;
                }
                else
                {
                    grdHolidayList.ShowFooter = true;
                }

                if (dsHoliday.Tables[0].Rows.Count > 0)
                {
                    grdHolidayList.Visible = true;
                    grdHolidayList.DataSource = dsHoliday;
                    grdHolidayList.DataBind();

                }
                else
                {
                    grdHolidayList.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "No holidays in this Year";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayList.cs", "DDlYear_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
    }
}