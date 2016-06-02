using HRMS.DAL;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;

namespace HRMS.Orbitweb
{
    public partial class HolidayList : System.Web.UI.Page
    {
        private HolidayMasterBOL objHolidayMasterBOL = new HolidayMasterBOL();
        private HolidayMasterModel objHolidayMastermodel = new HolidayMasterModel();
        private DataSet dsHoliday = new DataSet();
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string PageName = "Masters";
                objpagelevel.PageLevelAccess(PageName);

                lblError.Text = "";
                lblSuccess.Text = "";
                ViewState["AdminMaster"] = "HolidayList";
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
                throw new V2Exceptions(ex.ToString(), ex);
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
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion BindYear

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
                throw new V2Exceptions(ex.ToString(), ex);
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

                    TextBox txtHolidayDate = ((TextBox)(e.Item.FindControl("txtHolidayDate")));
                    TextBox txtHolidayDescription = ((TextBox)(e.Item.FindControl("txtHolidayDescription")));
                    CheckBox chkIsForShift = ((CheckBox)(e.Item.FindControl("chkIsForShiftFooter")));
                    //Added by Rahul Ramachandran
                    DropDownList ddlLocation = (DropDownList)e.Item.FindControl("ddlHolidayLocation1");
                    objHolidayMastermodel.OfficeLocation = Convert.ToInt16(ddlLocation.SelectedValue);
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

                        grdHolidayList.EditItemIndex = -1;
                        bindData();
                        grdHolidayList.ShowFooter = true;
                    }
                    else
                    {
                        lblError.Visible = true;
                        this.lblError.Style.Add("margin-bottom", "3px");
                        lblError.Text = "Holidays  are not allowed for Previous date ";
                        lblSuccess.Text = "";
                        txtHolidayDate.Text = "";
                        txtHolidayDescription.Text = "";
                    }
                }
                else if (e.CommandName == "btnCancel")
                {
                    grdHolidayList.EditItemIndex = -1;
                    bindData();
                    grdHolidayList.ShowFooter = true;
                }
                else if (e.CommandName == "Update")
                {
                    Label lblHolidayID = ((Label)(e.Item.FindControl("lblHolidayID2")));
                    TextBox txtHolidayDate = ((TextBox)(e.Item.FindControl("txtHolidayDate1")));
                    TextBox txtHolidayDescription = ((TextBox)(e.Item.FindControl("txtHolidayDescription1")));
                    CheckBox chkIsForShift = ((CheckBox)(e.Item.FindControl("chkIsForShiftEdit")));

                    objHolidayMastermodel.HolidayID = Convert.ToInt32(lblHolidayID.Text);
                    objHolidayMastermodel.HolidayDate = Convert.ToDateTime(txtHolidayDate.Text);
                    objHolidayMastermodel.HolidayDescription = txtHolidayDescription.Text;
                    //Added by Rahul Ramachandran
                    DropDownList ddlLocation = (DropDownList)e.Item.FindControl("ddlHolidayLocation2");
                    objHolidayMastermodel.OfficeLocation = Convert.ToInt16(ddlLocation.SelectedValue);
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
                //Added by Rahul Ramachandran
                else if (e.CommandName == "Edit")
                {
                    Label lblHolidayID = ((Label)(e.Item.FindControl("lblHolidayID")));
                    Label lblHolidayDate = ((Label)(e.Item.FindControl("lblHolidayDate")));
                    Label lblHolidayDescription = ((Label)(e.Item.FindControl("lblHolidayDescriptio")));
                    CheckBox chkIsForShift = ((CheckBox)(e.Item.FindControl("chkIsForShift")));
                    objHolidayMastermodel.HolidayID = Convert.ToInt32(lblHolidayID.Text);
                    objHolidayMastermodel.HolidayDate = Convert.ToDateTime(lblHolidayDate.Text);
                    objHolidayMastermodel.HolidayDescription = lblHolidayDescription.Text;
                    Label lblLocationID = (Label)e.Item.FindControl("lblHolidayLocationID");
                    objHolidayMastermodel.OfficeLocation = Convert.ToInt16(lblLocationID.Text);
                    if (chkIsForShift.Checked == true)
                        objHolidayMastermodel.isHolidayForShift = true;
                    else
                        objHolidayMastermodel.isHolidayForShift = false;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayList.cs", "dgrddepartment_ItemCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
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
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion AddNewDepartmentDetails

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
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion UpdateDepartmentDetails

        protected void grdHolidayList_EditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                grdHolidayList.EditItemIndex = e.Item.ItemIndex;
                bindData();
                grdHolidayList.ShowFooter = false;
                if (e.Item.FindControl("ddlHolidayLocation2") != null)
                {
                    EmployeeDAL dl = new EmployeeDAL();
                    List<OfficeLocationListDetails> holidayLocation = dl.GetOfficeLocationList();
                    DropDownList ddlLocation = (DropDownList)e.Item.FindControl("ddlHolidayLocation2");
                    ddlLocation.DataTextField = "OfficeLocation";
                    ddlLocation.DataValueField = "OfficeLocationID";
                    ddlLocation.DataSource = holidayLocation;
                    ddlLocation.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayList.aspx", "grdHolidayList_EditCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void grdHolidayList_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                //Added by Rahul Ramachandran
                EmployeeDAL dl = new EmployeeDAL();
                List<OfficeLocationListDetails> holidayLocation = dl.GetOfficeLocationList();
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblHolidayDate = (Label)e.Item.FindControl("lblHolidayDate");
                    lblHolidayDate.Text = lblHolidayDate.Text.Replace("12:00:00 AM", " ");// Substring(0, 13);// Replace("12:00:00 AM ", "");
                    if (Convert.ToDateTime(lblHolidayDate.Text) < DateTime.Now)
                    {
                        LinkButton lnkbutEdit = (LinkButton)e.Item.FindControl("lnkbutEdit");
                        lnkbutEdit.Enabled = false;
                    }
                    //Added by Rahul Ramachandran
                    Label lblLocation = (Label)e.Item.FindControl("lblHolidayLocation");
                    Label lblLocationID = (Label)e.Item.FindControl("lblHolidayLocationID");
                    //DropDownList ddlLocation = (DropDownList)e.Item.FindControl("ddlHolidayLocation");
                    //ddlLocation.DataTextField = "OfficeLocation";
                    //ddlLocation.DataValueField = "OfficeLocationID";
                    //ddlLocation.DataSource = holidayLocation;
                    //ddlLocation.DataBind();
                    //ddlLocation.Text = "";
                }
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    TextBox txtHolidayDate = ((TextBox)(e.Item.FindControl("txtHolidayDate")));
                    LinkButton btnAdd = (LinkButton)e.Item.FindControl("btnAdd");
                    TextBox txtHolidayDescription = ((TextBox)(e.Item.FindControl("txtHolidayDescription")));
                    btnAdd.Attributes.Add("onClick", "return validation('" + txtHolidayDescription.ClientID + "','" + txtHolidayDate.ClientID + "');");
                    txtHolidayDate.Attributes.Add("onkeydown", "return false");
                    //Added by Rahul Ramachandran
                    DropDownList ddlLocation = (DropDownList)e.Item.FindControl("ddlHolidayLocation1");
                    ddlLocation.DataTextField = "OfficeLocation";
                    ddlLocation.DataValueField = "OfficeLocationID";
                    ddlLocation.DataSource = holidayLocation;
                    ddlLocation.DataBind();
                }
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    TextBox txtHolidayDate = ((TextBox)(e.Item.FindControl("txtHolidayDate1")));
                    LinkButton lnkbutUpdate = (LinkButton)e.Item.FindControl("lnkbutUpdate");
                    TextBox txtHolidayDescription = ((TextBox)(e.Item.FindControl("txtHolidayDescription1")));
                    txtHolidayDate.Text = txtHolidayDate.Text.Replace("12:00:00 AM", " ");
                    lnkbutUpdate.Attributes.Add("onClick", "return validation('" + txtHolidayDescription.ClientID + "','" + txtHolidayDate.ClientID + "');");
                    txtHolidayDate.Attributes.Add("onkeydown", "return false");
                    //Added by Rahul Ramachandran
                    DropDownList ddlLocation = (DropDownList)e.Item.FindControl("ddlHolidayLocation2");
                    ddlLocation.DataTextField = "OfficeLocation";
                    ddlLocation.DataValueField = "OfficeLocationID";
                    ddlLocation.DataSource = holidayLocation;
                    ddlLocation.DataBind();
                    ddlLocation.SelectedValue = Convert.ToString(objHolidayMastermodel.OfficeLocation);
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
                throw new V2Exceptions(ex.ToString(), ex);
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
                    grdHolidayList.Visible = true;
                    grdHolidayList.DataSource = dsHoliday;
                    grdHolidayList.DataBind();
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
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        //public DataSet GetHolidayLocation()
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        //    SqlConnection con = new SqlConnection(connectionString);
        //    DataSet dsholidayloc = new DataSet();
        //    try
        //    {
        //        string cmd = "select * from tbl_pm_OfficeLocation";
        //        SqlDataAdapter adp = new SqlDataAdapter(cmd, con);
        //        adp.Fill(dsholidayloc.Tables["tbl_pm_OfficeLocation"]);

        //    }
        //    catch(Exception ex)
        //    {
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayList.cs", "grdHolidayList_ItemDataBound", ex.StackTrace);
        //        throw new V2Exceptions(ex.ToString(), ex);
        //    }
        //    return dsholidayloc;
        //}
    }
}