using System;
using System.Data;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;
using V2.Helpdesk.Model;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for EditEmployeeDetail.
    /// </summary>
    public partial class EditEmployeeDetail : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Label lblSystemAdmin;
        protected System.Web.UI.WebControls.ListBox lbAvailableAdminSubCategories;
        private int EmployeeId;
        private clsBLSubCategoryAssignment objClsBLSubCategoryAssignment = new clsBLSubCategoryAssignment();
        private clsSubCategoryAssignment objSubCategoryAssignment = new clsSubCategoryAssignment();
        private DataSet dsBindEmployeeNames = new DataSet();
        protected System.Web.UI.WebControls.Button btnRightShift;
        protected System.Web.UI.WebControls.CheckBox chkSuperAdmin;
        protected System.Web.UI.WebControls.RadioButton SysAdminTrue;
        protected System.Web.UI.WebControls.RadioButton SysAdminFalse;
        protected System.Web.UI.WebControls.Button btnLeftShift;
        protected System.Web.UI.WebControls.ListBox lblSelectedAdminSubCategories;

        //SubCategoryAssignment objSubCategoryAssignment1=new SubCategoryAssignment();
        private int tableCount = 0;

        private int tableCount1 = 0;
        private int Flag = 0;
        private string subCategoryList = "";
        private DataSet dsBindEmployeeNames1 = new DataSet();
        protected int noOfCategory = 0;
        protected int EmployeeID, SAEmployeeID, SuperAdmin;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                //code to clear all the cache so that after logout, previous page cannot be revisited.
                //--- starts here---------

                //--- ends here---------

                //EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                SuperAdmin = Convert.ToInt32(Session["SuperAdmin"]);
                //  OnlySuperAdmin = Convert.ToInt32(Session["OnlySuperAdmin"]);
                objSubCategoryAssignment.SuerAdminEmployeeId = SAEmployeeID;
                EmployeeId = Convert.ToInt32(Request.QueryString["EmpId"]);
                objSubCategoryAssignment.EmployeeID = EmployeeId;
                // if (EmployeeID.ToString() == "" || EmployeeID == 0 || SuperAdmin != 0 )
                // {
                //     if ((SAEmployeeID.ToString() == "" || SAEmployeeID == 0) && SuperAdmin == 0)
                //     {
                //         Response.Redirect("Login.aspx");
                //    }
                //}
                if (SuperAdmin == 0)
                // {
                // Response.Redirect("Login.aspx");
                //   }
                // else
                {
                    Response.Redirect("AuthorizationErrorMessage.aspx");
                }
                if (!IsPostBack)
                {
                    CountNoOfCategory(objSubCategoryAssignment);
                    bindEmployeeNames(objSubCategoryAssignment);
                    //	FetchRecordForEdit1(objSubCategoryAssignment);
                    FetchRecordForEdit1(objSubCategoryAssignment);
                }
                //btnUpdate.Attributes.Add("onClick","return CheckCategory();");    -- Category not compulsory for selected Department
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "EditEmployeeDetail.aspx", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
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

        #region Code for Binding Text box

        public void bindEmployeeNames(clsSubCategoryAssignment objSubCategoryAssignment)
        {
            try
            {
                dsBindEmployeeNames = objClsBLSubCategoryAssignment.getEmployeeName(objSubCategoryAssignment);
                if (dsBindEmployeeNames.Tables[0].Rows.Count > 0)
                {
                    txtEmployeeName.Text = dsBindEmployeeNames.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "EditEmployeeDetail.aspx", "bindEmployeeNames", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        # endregion

        #region Code for Count no of Category

        public void CountNoOfCategory(clsSubCategoryAssignment objSubCategoryAssignment)
        {
            try
            {
                dsBindEmployeeNames = objClsBLSubCategoryAssignment.CountNoOfCategory(objSubCategoryAssignment);
                if (dsBindEmployeeNames.Tables[0].Rows.Count > 0)
                {
                    noOfCategory = Convert.ToInt32(dsBindEmployeeNames.Tables[0].Rows[0][0]);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "EditEmployeeDetail.aspx", "CountNoOfCategory", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        # endregion

        #region Commented Code

        //		public void FindNoOfCategory(clsSubCategoryAssignment objSubCategoryAssignment)
        //		{
        //			dsBindEmployeeNames = objClsBLSubCategoryAssignment.FindNoOfCategory(objSubCategoryAssignment);
        //			if(dsBindEmployeeNames.Tables[0].Rows.Count>0)
        //			{
        //				for(int i=0;i<noOfCategory;i++)
        //				{
        //					//NoOfActiveCategory[i]=Convert.ToInt32(dsBindEmployeeNames.Tables[0].Rows[0][i]);
        //				}
        //
        //			}
        //		}

        #endregion Commented Code

        # region Fetch the Record to bind List box

        public void FetchRecordForEdit1(clsSubCategoryAssignment objSubCategoryAssignment)
        {
            try
            {
                dsBindEmployeeNames1 = objClsBLSubCategoryAssignment.FetchRecordForEdit1(objSubCategoryAssignment);
                ViewState["dsBindEmployeeNames1"] = dsBindEmployeeNames1;
                RepSubCategory.DataSource = dsBindEmployeeNames1.Tables[tableCount1];
                FetchRecordForEdit(objSubCategoryAssignment);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "EditEmployeeDetail.aspx", "FetchRecordForEdit1", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void FetchRecordForEdit(clsSubCategoryAssignment objSubCategoryAssignment)
        {
            try
            {
                dsBindEmployeeNames = objClsBLSubCategoryAssignment.FetchRecordForEdit(objSubCategoryAssignment);
                RepSubCategory.DataSource = dsBindEmployeeNames.Tables[0];
                RepSubCategory.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "EditEmployeeDetail.aspx", "FetchRecordForEdit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        # endregion

        protected void btnUpdate_Click(object sender, System.EventArgs e)
        {
            try
            {
                clsBLSubCategoryAssignment objClsBLSubCategoryAssignment = new clsBLSubCategoryAssignment();
                clsSubCategoryAssignment objClsSubCategoryAssignment = new clsSubCategoryAssignment();
                for (int i = 0; i < RepSubCategory.Items.Count; i++)
                {
                    Panel pnlforSubCategory = (Panel)(RepSubCategory.Items[i].FindControl("pnlforSubCategory"));
                    ListBox lblSelectedAdminSubCategories = (ListBox)(RepSubCategory.Items[i].FindControl("lblSelectedAdminSubCategories"));
                    ListBox lbAvailableAdminSubCategories = (ListBox)(RepSubCategory.Items[i].FindControl("lbAvailableAdminSubCategories"));
                    RadioButtonList SysAdminTrue = (RadioButtonList)(RepSubCategory.Items[i].FindControl("IsAdmin"));
                    RadioButtonList SysAdminFalse = (RadioButtonList)(RepSubCategory.Items[i].FindControl("IsAdmin"));
                    CheckBox chkSuperAdmin = (CheckBox)(RepSubCategory.Items[i].FindControl("chkSuperAdmin"));
                    Button btnLeftShift = (Button)(RepSubCategory.Items[i].FindControl("btnLeftShift"));
                    Button btnRightShift = (Button)(RepSubCategory.Items[i].FindControl("btnRightShift"));
                    Response.Write(lblSelectedAdminSubCategories.Items.Count);
                    if (lblSelectedAdminSubCategories.Items.Count > 0)
                    {
                        foreach (ListItem item in lblSelectedAdminSubCategories.Items)
                        {
                            subCategoryList = subCategoryList + item.Value + ",";
                        }
                    }
                    if (chkSuperAdmin.Checked == true)
                    {
                        Flag = 1;
                        if (SysAdminTrue.SelectedValue == "true")
                        {
                            dsBindEmployeeNames1 = (DataSet)ViewState["dsBindEmployeeNames1"];
                            objClsSubCategoryAssignment.CategoryID = Convert.ToInt32(dsBindEmployeeNames1.Tables[0].Rows[i][0]);
                            objClsSubCategoryAssignment.EmployeeID = EmployeeId;
                            objClsBLSubCategoryAssignment.CheckForSuperAdmin1(objClsSubCategoryAssignment);
                            SysAdminTrue.SelectedValue = "true";
                            SysAdminFalse.SelectedValue = "false";
                            chkSuperAdmin.Checked = true;
                        }
                        else
                        {
                            dsBindEmployeeNames1 = (DataSet)ViewState["dsBindEmployeeNames1"];
                            objClsSubCategoryAssignment.CategoryID = Convert.ToInt32(dsBindEmployeeNames1.Tables[0].Rows[i][0]);
                            objClsSubCategoryAssignment.EmployeeID = EmployeeId;
                            objClsBLSubCategoryAssignment.CheckForSuperAdmin(objClsSubCategoryAssignment);
                            SysAdminTrue.SelectedValue = "false";
                            SysAdminFalse.SelectedValue = "true";
                            chkSuperAdmin.Checked = true;
                        }
                    }
                    //}
                    else
                    {
                        dsBindEmployeeNames1 = (DataSet)ViewState["dsBindEmployeeNames1"];
                        objClsSubCategoryAssignment.CategoryID = Convert.ToInt32(dsBindEmployeeNames1.Tables[0].Rows[i][0]);
                        objClsSubCategoryAssignment.EmployeeID = EmployeeId;
                        objClsBLSubCategoryAssignment.deleteSubCategory(objClsSubCategoryAssignment);
                    }
                }
                if (Flag == 1)
                {
                    subCategoryList = subCategoryList.Trim(',');
                    objClsSubCategoryAssignment.SubCategoryID = subCategoryList;
                    objClsSubCategoryAssignment.EmployeeID = EmployeeId;
                    objClsSubCategoryAssignment.SuerAdminEmployeeId = SAEmployeeID;
                    objClsBLSubCategoryAssignment.UpdateSubCategoryAssignment(objClsSubCategoryAssignment);
                    lblMessage.Visible = true;
                    lblMessage.Text = "Record Updated Successfully ";
                    Response.Redirect("ViewEmployeeDetails.aspx");
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Please Select The Check box to Update the Category ";
                }
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "EditEmployeeDetail.aspx", "btnUpdate_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void RepSubCategory_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    ListBox lbAvailableAdminSubCategories = (ListBox)(e.Item.FindControl("lbAvailableAdminSubCategories"));
                    ListBox lblSelectedAdminSubCategories = (ListBox)(e.Item.FindControl("lblSelectedAdminSubCategories"));
                    lblSelectedAdminSubCategories.DataSource = dsBindEmployeeNames.Tables[tableCount + 1];
                    lblSelectedAdminSubCategories.DataTextField = "Subcategory";
                    lblSelectedAdminSubCategories.DataValueField = "SubCategoryID";
                    lblSelectedAdminSubCategories.DataBind();
                    lbAvailableAdminSubCategories.DataSource = dsBindEmployeeNames1.Tables[tableCount1 + 1];
                    lbAvailableAdminSubCategories.DataTextField = "Subcategory";
                    lbAvailableAdminSubCategories.DataValueField = "SubCategoryID";
                    lbAvailableAdminSubCategories.DataBind();

                    RadioButtonList SysAdminTrue = (RadioButtonList)(e.Item.FindControl("IsAdmin"));
                    RadioButtonList SysAdminFalse = (RadioButtonList)(e.Item.FindControl("IsAdmin"));
                    Label lblSystemAdmin = (Label)(e.Item.FindControl("lblSystemAdmin"));
                    CheckBox chkSuperAdmin = (CheckBox)(e.Item.FindControl("chkSuperAdmin"));
                    Label forchkSuperAdmin = (Label)(e.Item.FindControl("lblforchkSuperAdmin"));
                    forchkSuperAdmin.Text = dsBindEmployeeNames.Tables[0].Rows[tableCount][1].ToString();
                    //chkSuperAdmin.Checked = true;
                    if (dsBindEmployeeNames1.Tables[0].Rows[tableCount1][4] == System.DBNull.Value)
                    {
                        chkSuperAdmin.Checked = false;
                    }
                    else
                    {
                        chkSuperAdmin.Checked = true;
                    }

                    if (dsBindEmployeeNames1.Tables[0].Rows[tableCount][4] == System.DBNull.Value)
                    {
                        SysAdminTrue.SelectedValue = "false";
                    }
                    else
                    {
                        if (dsBindEmployeeNames1.Tables[0].Rows[tableCount][4].ToString() == "False")
                        {
                            SysAdminTrue.SelectedValue = "false";
                        }
                        else
                        {
                            SysAdminTrue.SelectedValue = "true";
                        }
                    }

                    //				if(dsBindEmployeeNames.Tables[0].Rows[tableCount1][4]!=System.DBNull.Value)
                    //				{
                    //					if(Convert.ToInt32(dsBindEmployeeNames.Tables[0].Rows[tableCount1][4])==1)
                    //					{
                    //						SysAdminTrue.SelectedValue="true";
                    //					}
                    //					else
                    //					{
                    //						SysAdminTrue.SelectedValue="false";
                    //					}
                    //				}
                    //				else
                    //				{
                    //					SysAdminTrue.SelectedValue="false";
                    //				}

                    tableCount++;
                    tableCount1++;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "EditEmployeeDetail.aspx", "RepSubCategory_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void RepSubCategory_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            try
            {
                ListBox lbAvailableAdminSubCategories = (ListBox)(e.Item.FindControl("lbAvailableAdminSubCategories"));
                ListBox lblSelectedAdminSubCategories = (ListBox)(e.Item.FindControl("lblSelectedAdminSubCategories"));
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
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "EditEmployeeDetail.aspx", "RepSubCategory_ItemCommand", ex.StackTrace);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "EditEmployeeDetail.aspx", "OnChangeHandler", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        # endregion

        #region Code for super Admin in Radio button Checked  Event.
        //		private void SysAdminTrue_CheckedChanged(object sender, System.EventArgs e)
        //		{
        //
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

        private void RepSubCategory_ItemCreated(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            try
            {
                CheckBox chkSuperAdmin = (CheckBox)(e.Item.FindControl("chkSuperAdmin"));
                RadioButtonList SysAdmin = (RadioButtonList)(e.Item.FindControl("IsAdmin"));
                //SysAdmin.SelectedIndexChanged += new System.EventHandler(this.SysAdminTrue_CheckedChanged);
                chkSuperAdmin.CheckedChanged += new EventHandler(OnChangeHandler);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "EditEmployeeDetail.aspx", "RepSubCategory_ItemCreated", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnCancle_Click(object sender, System.EventArgs e)
        {
            try
            {
                Response.Redirect("ViewEmployeeDetails.aspx");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "EditEmployeeDetail.aspx", "btnCancle_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}