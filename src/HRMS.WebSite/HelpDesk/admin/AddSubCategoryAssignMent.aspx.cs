using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;
using V2.Helpdesk.Model;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for AddSubCategoryAssignMent.
    /// </summary>
    public partial class AddSubCategoryAssignMent : System.Web.UI.Page
    {
        private int tableCount = 0;
        private string subCategoryList;
        private int i = 0, flag = 0;
        protected int EmployeeID, SAEmployeeID, SuperAdmin;
        private clsBLSubCategoryAssignment objClsBLSubCategoryAssignment = new clsBLSubCategoryAssignment();
        private clsSubCategoryAssignment objClsSubCategoryAssignment = new clsSubCategoryAssignment();
        private DataSet dsBindEmployeeNames = new DataSet();

        /// <summary>
        /// Page Load
        /// </summary>

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                //code to clear all the cache so that after logout, previous page cannot be revisited.
                //--- starts here---------

                //--- ends here---------
                int EmployeeId;
                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                EmployeeId = Convert.ToInt32(Session["EmployeeID"]);
                SuperAdmin = Convert.ToInt32(Session["SuperAdmin"]);
                //  OnlySuperAdmin = Convert.ToInt32(Session["OnlySuperAdmin"]);
                objClsSubCategoryAssignment.SuerAdminEmployeeId = SAEmployeeID;
                //if (EmployeeId.ToString() == "" || EmployeeID == 0 || SuperAdmin != 0)
                //{
                //    if ((SAEmployeeID.ToString() == "" || SAEmployeeID == 0) && SuperAdmin == 0)
                //    {
                //        Response.Redirect("Login.aspx");
                //    }
                //}
                if (SuperAdmin == 0)
                //{
                //    Response.Redirect("Login.aspx");
                //}
                //  else
                {
                    Response.Redirect("AuthorizationErrorMessage.aspx");
                }
                if (!Page.IsPostBack)
                {
                    bindEmployeeNames();
                    bindSubCategories();
                }
                //// btnAdd.Attributes.Add("onClick","return CheckCategory();");  -- category not compulsory for selected Department
            }
            catch (System.Threading.ThreadAbortException ex)
            {
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AddSubCategoryAssignMent.aspx", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            //LoadControlEvents(this.Controls);
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RepSubCategory.ItemCreated += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.RepSubCategory_ItemCreated);
            this.RepSubCategory.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.RepSubCategory_ItemDataBound);
            this.RepSubCategory.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.RepSubCategory_ItemCommand);
        }

        #endregion Web Form Designer generated code

        private void dgCategories_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        #region Code for Binding Employe Name

        public void bindEmployeeNames()
        {
            try
            {
                dsBindEmployeeNames = objClsBLSubCategoryAssignment.getEmployeeNames();
                if (dsBindEmployeeNames.Tables[0].Rows.Count >= 1)
                {
                    ddlEmployeeName.DataSource = dsBindEmployeeNames.Tables[0];
                    ddlEmployeeName.DataTextField = dsBindEmployeeNames.Tables[0].Columns["Emp_Name"].ToString();
                    ddlEmployeeName.DataValueField = dsBindEmployeeNames.Tables[0].Columns["Emp_User_Name"].ToString();
                    ddlEmployeeName.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AddSubCategoryAssignMent.aspx", "bindEmployeeNames", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        # endregion

        # region Code for Binding Category

        private void bindSubCategories()
        {
            try
            {
                //clsSubCategoryAssignment objClsSubCategoryAssignment = new clsSubCategoryAssignment();
                objClsSubCategoryAssignment.SuerAdminEmployeeId = SAEmployeeID;
                dsBindEmployeeNames = objClsBLSubCategoryAssignment.getAddSubCategory(objClsSubCategoryAssignment);
                ViewState["dsBindEmployeeNames"] = dsBindEmployeeNames;
                RepSubCategory.DataSource = dsBindEmployeeNames.Tables[tableCount];
                RepSubCategory.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AddSubCategoryAssignMent.aspx", "bindSubCategories", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        # endregion

        #region Code for  Selected Super admin
        //		private void SysAdminTrue_CheckedChanged(object sender, System.EventArgs e)
        //		{
        //			for(int i=0;i<RepSubCategory.Items.Count;i++)
        //			{
        //				ListBox lbAvailableAdminSubCategories = (ListBox)(RepSubCategory.Items[i].FindControl("lbAvailableAdminSubCategories"));
        //				ListBox lblSelectedAdminSubCategories = (ListBox)(RepSubCategory.Items[i].FindControl("lblSelectedAdminSubCategories"));
        //				RadioButtonList SysAdminTrue=(RadioButtonList)(RepSubCategory.Items[i].FindControl("IsAdmin"));
        //				CheckBox chkSuperAdmin=(CheckBox)(RepSubCategory.Items[i].FindControl("chkSuperAdmin"));
        //				if(chkSuperAdmin.Checked==true)
        //				{
        //					if(SysAdminTrue.SelectedValue=="true")
        //					{
        //						foreach(ListItem item in lbAvailableAdminSubCategories.Items)
        //						{
        //							lblSelectedAdminSubCategories.Items.Add(item);
        //						}
        //						foreach(ListItem item in lblSelectedAdminSubCategories.Items)
        //						{
        //							lbAvailableAdminSubCategories.Items.Remove(item);
        //						}
        //					}
        //				}
        //			}
        //		}
        # endregion

        private void RepSubCategory_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (dsBindEmployeeNames.Tables.Count > tableCount)
                    {
                        ListBox lbAvailableAdminSubCategories = (ListBox)(e.Item.FindControl("lbAvailableAdminSubCategories"));
                        ListBox lblSelectedAdminSubCategories = (ListBox)(e.Item.FindControl("lblSelectedAdminSubCategories"));
                        CheckBox chkSuperAdmin = (CheckBox)(e.Item.FindControl("chkSuperAdmin"));
                        Label forchkSuperAdmin = (Label)(e.Item.FindControl("lblforchkSuperAdmin"));
                        forchkSuperAdmin.Text = dsBindEmployeeNames.Tables[0].Rows[tableCount][1].ToString();
                        RadioButtonList SysAdminTrue = (RadioButtonList)(e.Item.FindControl("IsAdmin"));
                        lbAvailableAdminSubCategories.DataSource = dsBindEmployeeNames.Tables[++tableCount];
                        lbAvailableAdminSubCategories.DataTextField = "Subcategory";
                        lbAvailableAdminSubCategories.DataValueField = "SubCategoryId";
                        lbAvailableAdminSubCategories.DataBind();
                        SysAdminTrue.SelectedValue = "false";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AddSubCategoryAssignMent.aspx", "RepSubCategory_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void RepSubCategory_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            try
            {
                ListBox lbAvailableAdminSubCategories = (ListBox)(e.Item.FindControl("lbAvailableAdminSubCategories"));
                ListBox lblSelectedAdminSubCategories = (ListBox)(e.Item.FindControl("lblSelectedAdminSubCategories"));
                RadioButtonList SysAdminTrue = (RadioButtonList)(RepSubCategory.Items[i].FindControl("IsAdmin"));
                CheckBox chkSuperAdmin = (CheckBox)(e.Item.FindControl("chkSuperAdmin"));
                if (chkSuperAdmin.Checked == true)
                {
                    if (e.CommandName == "rightshift")
                    {
                        foreach (ListItem item in lbAvailableAdminSubCategories.Items)
                        {
                            if (item.Selected == true)
                            {
                                if (!lblSelectedAdminSubCategories.Items.Contains(item))
                                {
                                    //new ListItem(dsEmployees.Tables[0].Rows[i]["EmployeeName"].ToString(),dsEmployees.Tables[0].Rows[i]["EmployeeID"].ToString())
                                    lblSelectedAdminSubCategories.Items.Add(item);
                                }
                            }
                        }
                        foreach (ListItem item in lblSelectedAdminSubCategories.Items)
                        {
                            lbAvailableAdminSubCategories.Items.Remove(item);
                        }
                    }
                    else if (e.CommandName == "leftshift")
                    {
                        foreach (ListItem item in lblSelectedAdminSubCategories.Items)
                        {
                            if (item.Selected == true)
                            {
                                lbAvailableAdminSubCategories.Items.Add(item);
                            }
                        }
                        foreach (ListItem item in lbAvailableAdminSubCategories.Items)
                        {
                            lblSelectedAdminSubCategories.Items.Remove(item);
                        }
                    }
                    else if (e.CommandName == "RemoveAll")
                    {
                        foreach (ListItem item in lblSelectedAdminSubCategories.Items)
                        {
                            lbAvailableAdminSubCategories.Items.Add(item);
                        }
                        foreach (ListItem item in lbAvailableAdminSubCategories.Items)
                        {
                            lblSelectedAdminSubCategories.Items.Remove(item);
                        }
                    }
                    else if (e.CommandName == "CopyAll")
                    {
                        foreach (ListItem item in lbAvailableAdminSubCategories.Items)
                        {
                            lblSelectedAdminSubCategories.Items.Add(item);
                        }
                        foreach (ListItem item in lblSelectedAdminSubCategories.Items)
                        {
                            lbAvailableAdminSubCategories.Items.Remove(item);
                        }
                    }
                    else if (e.CommandName == "SysAdmin")
                    {
                        foreach (ListItem item in lbAvailableAdminSubCategories.Items)
                        {
                            lblSelectedAdminSubCategories.Items.Add(item);
                        }
                        foreach (ListItem item in lblSelectedAdminSubCategories.Items)
                        {
                            lbAvailableAdminSubCategories.Items.Remove(item);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AddSubCategoryAssignMent.aspx", "RepSubCategory_ItemCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #region Code for Department CheckBox Checked  Event.

        protected void OnChangeHandler(object sender, System.EventArgs e)
        {
            try
            {
                for (int i = 0; i < RepSubCategory.Items.Count; i++)
                {
                    ListBox lbAvailableAdminSubCategories = (ListBox)(RepSubCategory.Items[i].FindControl("lbAvailableAdminSubCategories"));
                    ListBox lblSelectedAdminSubCategories = (ListBox)(RepSubCategory.Items[i].FindControl("lblSelectedAdminSubCategories"));
                    RadioButtonList SysAdminTrue = (RadioButtonList)(RepSubCategory.Items[i].FindControl("IsAdmin"));
                    CheckBox chkSuperAdmin = (CheckBox)(RepSubCategory.Items[i].FindControl("chkSuperAdmin"));
                    if (chkSuperAdmin.Checked == false)
                    {
                        SysAdminTrue.SelectedValue = "false";
                        foreach (ListItem item in lblSelectedAdminSubCategories.Items)
                        {
                            lbAvailableAdminSubCategories.Items.Add(item);
                        }
                        foreach (ListItem item in lbAvailableAdminSubCategories.Items)
                        {
                            lblSelectedAdminSubCategories.Items.Remove(item);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AddSubCategoryAssignMent.aspx", "OnChangeHandler", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        # endregion

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                for (int i = 0; i < RepSubCategory.Items.Count; i++)
                {
                    ListBox lblSelectedAdminSubCategories = (ListBox)(RepSubCategory.Items[i].FindControl("lblSelectedAdminSubCategories"));
                    ListBox lbAvailableAdminSubCategories = (ListBox)(RepSubCategory.Items[i].FindControl("lbAvailableAdminSubCategories"));
                    CheckBox chkSuperAdmin = (CheckBox)(RepSubCategory.Items[i].FindControl("chkSuperAdmin"));
                    if (chkSuperAdmin.Checked == true)
                    {
                        if (lblSelectedAdminSubCategories.Items.Count > 0)
                        {
                            foreach (ListItem item in lblSelectedAdminSubCategories.Items)
                            {
                                subCategoryList = subCategoryList + item.Value + ",";
                            }
                        }
                    }
                }
                if (subCategoryList != null)
                {
                    flag = 1;
                    subCategoryList = subCategoryList.Trim(',');
                    objClsSubCategoryAssignment.SubCategoryID = subCategoryList;
                    objClsSubCategoryAssignment.EmployeeID = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);
                    objClsSubCategoryAssignment.SuerAdminEmployeeId = SAEmployeeID;
                    objClsBLSubCategoryAssignment.AddEmpRole(objClsSubCategoryAssignment);
                    objClsBLSubCategoryAssignment.AddSubCategoryAssignment(objClsSubCategoryAssignment);
                    for (int i = 0; i < RepSubCategory.Items.Count; i++)
                    {
                        CheckBox chkSuperAdmin = (CheckBox)(RepSubCategory.Items[i].FindControl("chkSuperAdmin"));
                        RadioButtonList SysAdminTrue = (RadioButtonList)(RepSubCategory.Items[i].FindControl("IsAdmin"));
                        if (chkSuperAdmin.Checked == true)
                        {
                            if (SysAdminTrue.SelectedValue == "true")
                            {
                                dsBindEmployeeNames = (DataSet)ViewState["dsBindEmployeeNames"];
                                objClsSubCategoryAssignment.CategoryID = Convert.ToInt32(dsBindEmployeeNames.Tables[0].Rows[i][0]);
                                objClsSubCategoryAssignment.EmployeeID = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);
                                objClsBLSubCategoryAssignment.CheckSuperAdmin1(objClsSubCategoryAssignment);
                            }
                            else
                            {
                                dsBindEmployeeNames = (DataSet)ViewState["dsBindEmployeeNames"];
                                objClsSubCategoryAssignment.CategoryID = Convert.ToInt32(dsBindEmployeeNames.Tables[0].Rows[i][0]);
                                objClsSubCategoryAssignment.EmployeeID = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);
                                objClsBLSubCategoryAssignment.CheckSuperAdmin(objClsSubCategoryAssignment);
                            }
                        }
                    }
                }
                else
                {
                    objClsSubCategoryAssignment.EmployeeID = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);
                    objClsSubCategoryAssignment.SuerAdminEmployeeId = SAEmployeeID;
                    objClsBLSubCategoryAssignment.AddEmpRole(objClsSubCategoryAssignment);
                    for (int i = 0; i < RepSubCategory.Items.Count; i++)
                    {
                        RadioButtonList SysAdminTrue = (RadioButtonList)(RepSubCategory.Items[i].FindControl("IsAdmin"));
                        if (SysAdminTrue.SelectedValue == "true")
                        {
                            dsBindEmployeeNames = (DataSet)ViewState["dsBindEmployeeNames"];
                            objClsSubCategoryAssignment.CategoryID = Convert.ToInt32(dsBindEmployeeNames.Tables[0].Rows[i][0]);
                            objClsSubCategoryAssignment.EmployeeID = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);
                            objClsBLSubCategoryAssignment.CheckSuperAdmin1(objClsSubCategoryAssignment);
                        }
                    }
                }

                lblMessage.Visible = true;
                lblMessage.Text = " Record Added SuccessFully ";
                Response.Redirect("ViewEmployeeDetails.aspx");
                //			if(flag==0)
                //			{
                //				lblMessage.Visible=true;
                //				lblMessage.Text="Please Select The SubCategory";
                //
                //			}
            }
            catch (System.Threading.ThreadAbortException ex)
            {
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AddSubCategoryAssignMent.aspx", "btnAdd_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void RepSubCategory_ItemCreated(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            try
            {
                RadioButtonList SysAdmin = (RadioButtonList)(e.Item.FindControl("IsAdmin"));
                //SysAdmin.SelectedIndexChanged += new System.EventHandler(this.SysAdminTrue_CheckedChanged);
                CheckBox chkSuperAdmin = (CheckBox)(e.Item.FindControl("chkSuperAdmin"));
                chkSuperAdmin.CheckedChanged += new EventHandler(OnChangeHandler);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AddSubCategoryAssignMent.aspx", "RepSubCategory_ItemCreated", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                bindEmployeeNames();
                bindSubCategories();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AddSubCategoryAssignMent.aspx", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}