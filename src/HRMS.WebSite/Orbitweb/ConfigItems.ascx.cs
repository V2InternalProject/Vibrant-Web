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
using V2.Orbit.Model;
using V2.Orbit.BusinessLayer;
using V2.CommonServices.FileLogger;
using V2.CommonServices.Exceptions;
using System.Text.RegularExpressions;

namespace HRMS.Orbitweb
{
    public partial class ConfigItems1 : System.Web.UI.UserControl
    {
        ConfigItemBOL objConfigItemBOL = new ConfigItemBOL();
        ConfigItemModel objConfigItemModel = new ConfigItemModel();
        DataSet dsConfigItem = new DataSet();


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblError.Visible = false;
                lblSuccess.Visible = false;
                if (!IsPostBack)
                {
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItemMaster.cs", "Page_Load", ex.StackTrace);
                throw new V2Exceptions();
            }

        }
        public void bindData()
        {
            try
            {
                dsConfigItem = objConfigItemBOL.bindData();
                grdConfigItem.DataSource = dsConfigItem;
                grdConfigItem.DataBind();
                if (grdConfigItem.PageCount > 1)
                {
                    grdConfigItem.PagerStyle.Visible = true;
                }
                else
                {
                    grdConfigItem.PagerStyle.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItem.cs", "bindData", ex.StackTrace);
                throw new V2Exceptions();
            }

        }

        protected void grdConfigItem_EditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                grdConfigItem.EditItemIndex = e.Item.ItemIndex;
                bindData();
                //grdConfigItem.ShowFooter = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItem.cs", "grdConfigItem_EditCommand", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        protected void grdConfigItem_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                #region Commented code for Add
                //if (e.CommandName == "btnAdd")
                //{

                //    grdConfigItem.EditItemIndex = -1;
                //    grdConfigItem.ShowFooter = true;
                //    // TextBox lblHolidayID = ((TextBox)(e.Item.FindControl("txtHolidayID")));
                //    TextBox txtConfigItemName = ((TextBox)(e.Item.FindControl("txtConfigItemName")));
                //    TextBox txtConfigItemValue = ((TextBox)(e.Item.FindControl("txtConfigItemValue")));
                //    // objHolidayMastermodel.HolidayID =Convert.ToInt32( lblHolidayID.Text);
                //    objConfigItemModel.ConfigItemName = txtConfigItemName.Text;
                //    objConfigItemModel.ConfigItemValue = txtConfigItemValue.Text;

                //    AddNewConfigItems();
                //    //Reset Page Controls
                //    //dgrddepartment.CurrentPageIndex=0;
                //    grdConfigItem.EditItemIndex = -1;
                //    bindData();
                //    grdConfigItem.ShowFooter = true;
                //}
                #endregion

                if (e.CommandName == "btnCancel")
                {
                    grdConfigItem.EditItemIndex = -1;
                    bindData();
                    // grdConfigItem.ShowFooter = true;
                }

                if (e.CommandName == "Update")
                {
                    Label lblConfigItemID = ((Label)(e.Item.FindControl("lblConfigItemID2")));
                    Label lblConfigItemName = ((Label)(e.Item.FindControl("lblConfigItemName")));
                    TextBox txtConfigItemValue = ((TextBox)(e.Item.FindControl("txtConfigItemValue1")));
                    TextBox txtConfigItemDescription = ((TextBox)(e.Item.FindControl("txtConfigItemDescription1")));
                    DropDownList ddlHours = ((DropDownList)(e.Item.FindControl("ddlHours")));
                    DropDownList ddlMins = ((DropDownList)(e.Item.FindControl("ddlMins")));
                    if (lblConfigItemName.Text == "Check Attendance Time")
                    {
                        objConfigItemModel.ConfigItemValue = ddlHours.SelectedItem.Value + ":" + ddlMins.SelectedItem.Value;
                        ddlHours.Visible = false;
                        ddlMins.Visible = false;
                    }
                    else
                    {
                        objConfigItemModel.ConfigItemValue = txtConfigItemValue.Text;
                    }
                    objConfigItemModel.ConfigItemIdID = Convert.ToInt32(lblConfigItemID.Text);
                    objConfigItemModel.ConfigItemName = lblConfigItemName.Text;
                    objConfigItemModel.ConfigItemDescription = txtConfigItemDescription.Text;
                    if (lblConfigItemName.Text == "Freezing Date")
                    {
                        if (Convert.ToDateTime(txtConfigItemValue.Text) < DateTime.Now)
                        {
                            UpdateConfigItems();
                            //  grdConfigItem.ShowFooter = true;
                            grdConfigItem.EditItemIndex = -1;
                            bindData();
                        }
                        else
                        {
                            // grdConfigItem.ShowFooter = true;
                            grdConfigItem.EditItemIndex = -1;
                            lblError.Visible = true;
                            lblError.Text = "Freezing Date can not Greater than Current Date";
                            lblSuccess.Visible = false;
                            bindData();
                        }
                    }
                    else if (lblConfigItemName.Text == "SISOLimitDays")
                    {

                        if (!CheckIfNumberic(txtConfigItemValue.Text))
                        {
                            lblError.Visible = true;
                            lblError.Text = "SISOLimitDays Must be Numeric";
                            lblSuccess.Visible = false;
                        }
                        else if (Convert.ToInt32(txtConfigItemValue.Text) > 100)
                        {
                            lblError.Visible = true;
                            lblError.Text = "SISOLimitDays not more than 100";
                            lblSuccess.Visible = false;
                        }
                        else
                        {
                            UpdateConfigItems();
                            // grdConfigItem.ShowFooter = true;
                            grdConfigItem.EditItemIndex = -1;
                            bindData();
                        }

                    }
                    else if (lblConfigItemName.Text == "From EmailID")
                    {
                        if (!isEmail(txtConfigItemValue.Text))
                        {
                            lblError.Visible = true;
                            lblError.Text = "Please enter valid Email Address";
                            lblSuccess.Visible = false;
                        }
                        else
                        {
                            UpdateConfigItems();
                            //grdConfigItem.ShowFooter = true;
                            grdConfigItem.EditItemIndex = -1;
                            bindData();
                        }
                    }
                    else
                    {
                        UpdateConfigItems();
                        // grdConfigItem.ShowFooter = true;
                        grdConfigItem.EditItemIndex = -1;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItemMaster.cs", "dgrddepartment_ItemCommand", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        bool CheckIfNumberic(string myNumber)
        {
            try
            {
                bool IsNum = true;
                for (int index = 0; index < myNumber.Length; index++)
                {
                    if (!Char.IsNumber(myNumber[index]))
                    {
                        IsNum = false;
                        break;
                    }
                }
                return IsNum;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItemMaster.cs", "CheckIfNumberic", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        bool isEmail(string inputEmail)
        {
            try
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                if (re.IsMatch(inputEmail))
                    return (true);
                else
                    return (false);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItemMaster.cs", "isEmail", ex.StackTrace);
                throw new V2Exceptions();
            }
        }


        #region AddNewConfigItems
        //public void AddNewConfigItems()
        //{
        //    try
        //    {

        //        int rowsAffected = objConfigItemBOL.AddNewConfigItems(objConfigItemModel);
        //        if (rowsAffected > 0)
        //        {
        //            lblSuccess.Visible = true;
        //            lblSuccess.Text = "Record added successfully";
        //            lblError.Visible = false;
        //        }
        //        else
        //        {
        //            lblError.Visible = true;
        //            lblError.Text = "ConfigItem Name already exists";
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (V2Exceptions ex)
        //    {
        //        throw;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItem.cs", "AddNewConfigItems", ex.StackTrace);
        //        throw new V2Exceptions();
        //    }
        //}
        #endregion

        #region UpdateConfigItems
        public void UpdateConfigItems()
        {
            try
            {
                int rowsAffected = objConfigItemBOL.UpdateConfigItems(objConfigItemModel);
                if (rowsAffected > 0)
                {
                    lblSuccess.Visible = true;
                    lblSuccess.Text = "Record updated successfully";
                    lblError.Visible = false;
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "ConfigItem Name already exists";
                    lblSuccess.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                lblError.Visible = true;
                lblError.Text = "ConfigItem Name already exists";
                lblSuccess.Visible = false;
                throw;
            }
            catch (System.Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "ConfigItem Name already exists";
                lblSuccess.Visible = false;
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItem.cs", "UpdateConfigItems", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        #endregion
        protected void grdConfigItem_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            try
            {
                grdConfigItem.CurrentPageIndex = e.NewPageIndex;
                grdConfigItem.EditItemIndex = -1;
                bindData();
                //grdConfigItem.ShowFooter = true;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItems.cs", "grdConfigItem__ItemCommand", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        protected void grdConfigItem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item)
                {
                    Label lblConfigItemValue = ((Label)(e.Item.FindControl("lblConfigItemValue")));
                    Label lblConfigItemName = ((Label)(e.Item.FindControl("lblConfigItemName")));
                    if (lblConfigItemName.Text == "Check Attendance Time")
                    {
                        string[] time = new string[2];
                        time = lblConfigItemValue.Text.Split(':');
                        if (Convert.ToInt32(time[0]) < 10)
                            time[0] = "0" + time[0];
                        if (Convert.ToInt32(time[1]) < 10)
                            time[1] = "0" + time[1];
                        lblConfigItemValue.Text = time[0] + ":" + time[1];
                    }

                }
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    DropDownList ddlHours = ((DropDownList)(e.Item.FindControl("ddlHours")));
                    DropDownList ddlMins = ((DropDownList)(e.Item.FindControl("ddlMins")));
                    LinkButton lnkbutUpdate = (LinkButton)e.Item.FindControl("lnkbutUpdate");
                    TextBox txtConfigItemValue = ((TextBox)(e.Item.FindControl("txtConfigItemValue1")));
                    Label lblConfigItemName = ((Label)(e.Item.FindControl("lblConfigItemName")));
                    ImageButton imgbtnSearchToDate = ((ImageButton)(e.Item.FindControl("imgbtnSearchToDate1")));
                    AjaxControlToolkit.CalendarExtender CalendarExtenderSearchToDate1 = ((AjaxControlToolkit.CalendarExtender)(e.Item.FindControl("CalendarExtenderSearchToDate1")));
                    if (lblConfigItemName.Text == "Check Attendance Time")
                    {
                        string[] time = new string[2];
                        ddlHours.Visible = true;
                        ddlMins.Visible = true;
                        txtConfigItemValue.Visible = false;
                        time = txtConfigItemValue.Text.Split(':');
                        ddlHours.SelectedIndex = Convert.ToInt32(time[0]);
                        ddlMins.SelectedIndex = Convert.ToInt32(time[1]);
                        e.Item.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                        //grdConfigItem.Items[e.Item.ItemIndex].Cells[1].Controls.Add(ddlHours);
                        //grdConfigItem.Items[e.Item.ItemIndex].Cells[1].Controls.Add(ddlMins);
                    }
                    if (lblConfigItemName.Text == "Freezing Date" || lblConfigItemName.Text == "Lock Upload Date")
                    {
                        imgbtnSearchToDate.Visible = true;
                        // txtConfigItemValue.ReadOnly = true;
                        txtConfigItemValue.Attributes.Add("onkeydown", "return false");
                    }
                    else
                    {
                        CalendarExtenderSearchToDate1.Enabled = false;
                    }

                    lnkbutUpdate.Attributes.Add("onClick", "return validation(" + txtConfigItemValue.ClientID + ");");

                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItems.cs", "grdConfigItem_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
    }
}