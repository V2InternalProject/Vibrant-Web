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
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace HRMS.Orbitweb
{
    public partial class Status : System.Web.UI.UserControl
    {
        StatusMasterModel objStatusMasterModel = new StatusMasterModel();
        StatusMasterBOL objStatusMasterBOL = new StatusMasterBOL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblError.Visible = false;
                lblSuccess.Visible = false;
                if (!IsPostBack)
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.cs", "Page_Load", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        public void BindData()
        {
            try
            {
                DataSet dsGetStatus = new DataSet();
                dsGetStatus = objStatusMasterBOL.BindData();
                grdStatus.DataSource = dsGetStatus.Tables[2];
                grdStatus.DataBind();
                if (grdStatus.PageCount > 1)
                {
                    grdStatus.PagerStyle.Visible = true;
                }
                else
                {
                    grdStatus.PagerStyle.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.cs", "BindData", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        protected void grdConfigItem_ItemCommand(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "AddStatus")
            {
                TextBox txtFSBUName = (TextBox)e.Item.FindControl("txtStatusName");
                objStatusMasterModel.StatusName = txtFSBUName.Text.Trim();
                DropDownList ddlIsActive = (DropDownList)e.Item.FindControl("ddlFIsActive");
                objStatusMasterModel.IsActive = Convert.ToInt32(ddlIsActive.SelectedValue);
                try
                {
                    int rowUpdated = objStatusMasterBOL.AddStatus(objStatusMasterModel);

                    if (rowUpdated > 0)
                    {
                        BindData();
                        lblError.Visible = false;
                        lblSuccess.Text = "Status name Added successfully";
                        lblSuccess.Visible = true;
                    }
                    else
                    {
                        BindData();
                        lblError.Visible = true;
                        lblError.Text = "Status name already exists";
                        lblSuccess.Visible = false;
                    }

                }
                catch (V2Exceptions ex)
                {
                    lblError.Visible = true;
                    lblError.Text = "Status name already exists";
                    lblSuccess.Visible = false;
                    throw;

                }
                catch (System.Exception ex)
                {
                    lblError.Visible = true;
                    lblError.Text = "Status name already exists";
                    lblSuccess.Visible = false;
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.cs", "dgSBUMaster_ItemCommand", ex.StackTrace);
                    throw new V2Exceptions();
                }
            }
            if (e.CommandName == "CancelAdd")
            {
                TextBox txtStatusName = (TextBox)e.Item.FindControl("txtStatusName");
                txtStatusName.Text = "";
            }

            #region Edit
            if (e.CommandName == "EditStatus")
            {
                grdStatus.EditItemIndex = e.Item.ItemIndex;

                BindData();
                //footer = false for validation
                // grdStatus.ShowFooter = false;
            }
            #endregion

            #region  Update SBU
            if (e.CommandName == "UpdateStatus")
            {
                TextBox txtStatusName = (TextBox)e.Item.FindControl("txtStatusName1");
                objStatusMasterModel.StatusName = txtStatusName.Text.Trim();
                DropDownList ddlIsActive1 = (DropDownList)e.Item.FindControl("ddlIsActive");
                objStatusMasterModel.IsActive = Convert.ToInt32(ddlIsActive1.SelectedValue);
                Label lblStatusID = (Label)e.Item.FindControl("lblStatusID1");
                objStatusMasterModel.StatusId = Convert.ToInt32(lblStatusID.Text);
                try
                {
                    int rowUpdated = objStatusMasterBOL.UpdateStatus(objStatusMasterModel);
                    if (rowUpdated > 0)
                    {
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "Status name updated successfully";
                        lblError.Visible = false;
                        // grdStatus.ShowFooter = true;
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Status name already exists";
                        lblSuccess.Visible = false;
                        // grdStatus.ShowFooter = true;
                    }
                    grdStatus.EditItemIndex = -1;
                    BindData();

                    // }
                }

                catch (V2Exceptions ex)
                {
                    lblError.Text = "Status name already exists";
                    lblSuccess.Visible = false;
                    lblError.Visible = true;
                    throw;

                }
                catch (System.Exception ex)
                {
                    lblError.Text = "Status name already exists";
                    lblSuccess.Visible = false;
                    lblError.Visible = true;
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.cs", "dgSBUMaster_ItemCommand", ex.StackTrace);
                    throw new V2Exceptions();
                }
            }

            #endregion

            #region Cancel Update
            if (e.CommandName == "CancelUpdate")
            {
                grdStatus.EditItemIndex = -1;
                BindData();
                lblError.Text = "";
                //  grdStatus.ShowFooter = true;
            }
            #endregion

        }
        protected void grdConfigItem_EditCommand(object source, DataGridCommandEventArgs e)
        {

        }

        protected void grdConfigItem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblIsActive = (Label)e.Item.FindControl("lblIsActive");
                    if (lblIsActive.Text == "True")
                    {
                        lblIsActive.Text = "Active";
                    }
                    else if (lblIsActive.Text == "False")
                    {
                        lblIsActive.Text = "InActive";
                    }
                }
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    TextBox txtStatusName = (TextBox)e.Item.FindControl("txtStatusName");
                    LinkButton btnAdd = (LinkButton)e.Item.FindControl("btnAdd");
                    //LinkButton btnUpdate = (LinkButton)e.Item.FindControl("btnUpdate");
                    btnAdd.Attributes.Add("onClick", "return validation(" + txtStatusName.ClientID + ");");

                    //btnAdd.Attributes.Add("onkeyup", "return spcharacter(" + txtFSBUName.ClientID + ");");
                }
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    TextBox txtStatusName = (TextBox)e.Item.FindControl("txtStatusName1");

                    LinkButton btnUpdate = (LinkButton)e.Item.FindControl("btnUpdate");
                    btnUpdate.Attributes.Add("onClick", "return validation(" + txtStatusName.ClientID + ");");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.cs", "dgSBUMaster_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        protected void grdStatus_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            try
            {
                grdStatus.CurrentPageIndex = e.NewPageIndex;

                grdStatus.EditItemIndex = -1;
                BindData();
                // grdStatus.ShowFooter = true;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.cs", "grdStatus_PageIndexChanged", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
    }
}