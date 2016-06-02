using System;
using System.Configuration;
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
    /// Summary description for SubCategoryAssignment.
    /// </summary>
    public partial class SubCategoryAssignment : System.Web.UI.Page
    {
        #region Variable declaration

        protected System.Web.UI.HtmlControls.HtmlGenericControl divAdmin;

        //protected System.Web.UI.WebControls.CheckBox chkBoxSysAdmin;
        private DataTable dtNewTable;

        public int EmployeeID, SAEmployeeID;

        #endregion Variable declaration

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Put user code to initialize the page here
                //code to clear all the cache so that after logout, previous page cannot be revisited.
                //--- starts here---------

                //--- ends here---------
                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                //if(EmployeeID.ToString() == "" || EmployeeID == 0)
                //{
                if (SAEmployeeID.ToString() == "" || SAEmployeeID == 0)
                {
                    Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
                }
                //}
                //else
                //{
                //    Response.Redirect("AuthorizationErrorMessage.aspx");
                //}
                if (!Page.IsPostBack)
                {
                    /*if(EmployeeID.ToString() == "" || EmployeeID == 0)
                    {
                        if(SAEmployeeID.ToString() == "" || SAEmployeeID == 0)
                        {
                            Response.Redirect("Login.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("AuthorizationErrorMessage.aspx");
                    }*/
                    bindDataGrid();
                    bindEmployeeNames();
                    bindSubCategories();
                }
                btnSubmit.Attributes.Add("OnClick", "return isSubCategorySelected();");
                ReadFromViewState();
                //lblMessage.Visible = false;
                lblSuccessMessage.Visible = false;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        //To Bind the ListBoxes with the SubCategories fetched form the Databse
        private void bindSubCategories()
        {
            try
            {
                clsBLSubCategoryAssignment objClsBLSubCategoryAssignment = new clsBLSubCategoryAssignment();
                clsSubCategoryAssignment objClsSubCategoryAssignment = new clsSubCategoryAssignment();
                DataSet dsAdminSubCategories = objClsBLSubCategoryAssignment.getSubCategories();
                if (dsAdminSubCategories.Tables[0].Rows.Count >= 1)
                {
                    lbAvailableAdminSubCategories.DataSource = dsAdminSubCategories.Tables[0];
                    lbAvailableAdminSubCategories.DataValueField = dsAdminSubCategories.Tables[0].Columns["SubCategoryID"].ToString();
                    lbAvailableAdminSubCategories.DataTextField = dsAdminSubCategories.Tables[0].Columns["SubCategory"].ToString();
                    lbAvailableAdminSubCategories.DataBind();
                }
                if (dsAdminSubCategories.Tables[1].Rows.Count >= 1)
                {
                    lbAvailableITSubCategories.DataSource = dsAdminSubCategories.Tables[1];
                    lbAvailableITSubCategories.DataValueField = dsAdminSubCategories.Tables[1].Columns["SubCategoryID"].ToString();
                    lbAvailableITSubCategories.DataTextField = dsAdminSubCategories.Tables[1].Columns["SubCategory"].ToString();
                    lbAvailableITSubCategories.DataBind();
                }
                if (dsAdminSubCategories.Tables[2].Rows.Count >= 1)
                {
                    lbAvailableHRSubCategories.DataSource = dsAdminSubCategories.Tables[2];
                    lbAvailableHRSubCategories.DataValueField = dsAdminSubCategories.Tables[2].Columns["SubCategoryID"].ToString();
                    lbAvailableHRSubCategories.DataTextField = dsAdminSubCategories.Tables[2].Columns["SubCategory"].ToString();
                    lbAvailableHRSubCategories.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "bindSubCategories", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void InsertSubcategoryAssignment()
        {
            try
            {
                string subCategoryList = "";
                if (lbSelectedAdminSubCategories != null)
                {
                    //Response.Write(lbSelectedAdminSubCategories.Items.ToString());
                    /*for (int i=0; i <= lbSelectedAdminSubCategories.Items.Count-1; i++)
                    {
                        subCategoryList = subCategoryList + lbSelectedAdminSubCategories.Items + ",";
                    }*/
                    foreach (ListItem item in lbSelectedAdminSubCategories.Items)
                    {
                        subCategoryList = subCategoryList + item.Value + ",";
                    }
                }
                if (lbSelectedHRSubCategories != null)
                {
                    foreach (ListItem item in lbSelectedHRSubCategories.Items)
                    {
                        subCategoryList = subCategoryList + item.Value + ",";
                    }
                }
                if (lbSelectedITSubCategories != null)
                {
                    foreach (ListItem item in lbSelectedITSubCategories.Items)
                    {
                        subCategoryList = subCategoryList + item.Value + ",";
                    }
                }
                subCategoryList = subCategoryList.Trim(',');
                clsSubCategoryAssignment objClsSubCategoryAssignment = new clsSubCategoryAssignment();
                objClsSubCategoryAssignment.SubCategoryID = subCategoryList;
                objClsSubCategoryAssignment.EmployeeID = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);
                clsBLSubCategoryAssignment objClsBLSubCategoryAssignment = new clsBLSubCategoryAssignment();
                objClsBLSubCategoryAssignment.InsertSubCategoryAssignment(objClsSubCategoryAssignment);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "InsertSubcategoryAssignment", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void bindEmployeeNames()
        {
            try
            {
                clsBLSubCategoryAssignment objClsBLSubCategoryAssignment = new clsBLSubCategoryAssignment();
                DataSet dsBindEmployeeNames = objClsBLSubCategoryAssignment.getEmployeeNames();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "bindEmployeeNames", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void bindDataGrid()
        {
            try
            {
                clsBLSubCategoryAssignment objClsBLSubCategoryAssignment = new clsBLSubCategoryAssignment();
                DataSet dsGetData = objClsBLSubCategoryAssignment.getData();
                dgCategories.DataSource = dsGetData.Tables[0];
                dgCategories.DataBind();
                if (dgCategories.PageCount > 1)
                {
                    dgCategories.PagerStyle.Visible = true;
                }
                else
                {
                    dgCategories.PagerStyle.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "bindDataGrid", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void fillNewTable()
        {
            try
            {
                DataRow drNewRow;
                dtNewTable = new DataTable();
                dtNewTable.Columns.Add("EmployeeID");
                dtNewTable.Columns.Add("CategoryID");
                dtNewTable.Columns.Add("IsSysAdmin");
                foreach (DataGridItem dgCategoriesItem in dgCategories.Items)
                {
                    if (dgCategoriesItem.ItemType == ListItemType.AlternatingItem || dgCategoriesItem.ItemType == ListItemType.Item)
                    {
                        CheckBox chkBoxCategory = new CheckBox();
                        chkBoxCategory = (CheckBox)(dgCategoriesItem.FindControl("chkBoxCategory"));
                        if (chkBoxCategory.Checked)
                        {
                            drNewRow = dtNewTable.NewRow();
                            string checkedCategoryID = ((Label)dgCategoriesItem.FindControl("lblCategoryID")).Text;
                            drNewRow["CategoryID"] = Convert.ToInt32(checkedCategoryID);//Convert.ToInt32(lblNewLabel.Text);
                            if (((CheckBox)dgCategoriesItem.FindControl("chkBoxSysAdmin")).Checked == true)
                            {
                                drNewRow["IsSysAdmin"] = 1;
                            }
                            else if (((CheckBox)dgCategoriesItem.FindControl("chkBoxSysAdmin")).Checked == false)
                            {
                                drNewRow["IsSysAdmin"] = 0;
                            }
                            drNewRow["EmployeeID"] = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);
                            dtNewTable.Rows.Add(drNewRow);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "fillNewTable", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void InsertEmployeeRoles()
        {
            try
            {
                fillNewTable();
                InsertEmployee();
                InsertSubcategoryAssignment();
                lblSuccessMessage.Visible = true;
                lblSuccessMessage.Text = "Sub-Categories have been successfully assigned to " + ddlEmployeeName.SelectedItem.Text + ".";

                for (int recordCount = 0; recordCount <= dtNewTable.Rows.Count - 1; recordCount++)
                {
                    clsSubCategoryAssignment objClsSubCategoryAssignment = new clsSubCategoryAssignment();
                    objClsSubCategoryAssignment.EmployeeID = Convert.ToInt32(dtNewTable.Rows[recordCount][0]);
                    objClsSubCategoryAssignment.CategoryID = Convert.ToInt32(dtNewTable.Rows[recordCount][1]);
                    objClsSubCategoryAssignment.IsSysAdmin = Convert.ToInt32(dtNewTable.Rows[recordCount][2]);
                    clsBLSubCategoryAssignment objClsBLSubCategoryAssignment = new clsBLSubCategoryAssignment();
                    objClsBLSubCategoryAssignment.InsertEmployeeRoles(objClsSubCategoryAssignment);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "InsertEmployeeRoles", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void Page_Prerender()
        {
            try
            {
                WriteViewState();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "Page_Prerender", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void WriteViewState()
        {
            try
            {
                ViewState["dtNewTable"] = dtNewTable;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "WriteViewState", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void ReadFromViewState()
        {
            try
            {
                if (ViewState["dtNewTable"] != null)
                {
                    dtNewTable = ViewState["dtNewTable"] as DataTable;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "ReadFromViewState", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        //		public void IsCategorySelected(object sender, DataGridItemEventArgs e)
        //		{
        //			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        //			{
        //				dgCategories.FindControl("chkCategory");
        //			}
        //		}

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
        }

        #endregion Web Form Designer generated code

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                //InsertEmployee();
                //InsertSubcategoryAssignment();
                clsSubCategoryAssignment objClsSubCategoryAssignment = new clsSubCategoryAssignment();
                clsBLSubCategoryAssignment objClsBLSubCategoryAssignment = new clsBLSubCategoryAssignment();
                objClsSubCategoryAssignment.EmployeeID = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);
                int noOfRowsReturned = objClsBLSubCategoryAssignment.DoesEmployeeExist(objClsSubCategoryAssignment);
                if (noOfRowsReturned == 0)
                {
                    lblEmployee.Visible = false;
                    InsertEmployeeRoles();
                }
                else
                {
                    lblEmployee.Visible = true;
                    lblEmployee.Text = "EmployeeID " + objClsSubCategoryAssignment.EmployeeID + " already exists";
                    lblSuccessMessage.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void InsertEmployee()
        {
            try
            {
                clsSubCategoryAssignment objClsSubCategoryAssignment = new clsSubCategoryAssignment();
                clsBLSubCategoryAssignment objClsBLSubCategoryAssignment = new clsBLSubCategoryAssignment();
                objClsSubCategoryAssignment.EmployeeID = Convert.ToInt32(ddlEmployeeName.SelectedItem.Value);
                string strEmployeeEmailID = objClsBLSubCategoryAssignment.getEmployeeEmailID(objClsSubCategoryAssignment);
                objClsSubCategoryAssignment.EmployeeEmailID = strEmployeeEmailID;
                objClsSubCategoryAssignment.EmployeeName = ddlEmployeeName.SelectedItem.Text;
                objClsSubCategoryAssignment.IsActive = 1;
                objClsBLSubCategoryAssignment.InsertEmployee(objClsSubCategoryAssignment);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "InsertEmployee", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnAdminCopy_Click(object sender, System.EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbAvailableAdminSubCategories.Items)
                {
                    if (item.Selected == true)
                    {
                        //lblAvailableAdminSubcategories.Visible = false;
                        if (!lbSelectedAdminSubCategories.Items.Contains(item))
                        {
                            lbSelectedAdminSubCategories.Items.Add(item);
                        }
                        //lbAvailableAdminSubCategories.Items.Remove(item);
                    }
                }
                foreach (ListItem item in lbSelectedAdminSubCategories.Items)
                {
                    lbAvailableAdminSubCategories.Items.Remove(item);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "btnAdminCopy_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnAdminRemove_Click(object sender, System.EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbSelectedAdminSubCategories.Items)
                {
                    if (item.Selected == true)
                    {
                        lbAvailableAdminSubCategories.Items.Add(item);
                    }
                }
                foreach (ListItem item in lbAvailableAdminSubCategories.Items)
                {
                    lbSelectedAdminSubCategories.Items.Remove(item);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "btnAdminRemove_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnAdminCopyAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                SelectAllAdmin();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "btnAdminCopyAll_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnAdminRemoveAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                RemoveAllAdmin();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "btnAdminRemoveAll_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnITCopy_Click(object sender, System.EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbAvailableITSubCategories.Items)
                {
                    if (item.Selected == true)
                    {
                        if (!lbSelectedITSubCategories.Items.Contains(item))
                        {
                            lbSelectedITSubCategories.Items.Add(item);
                        }
                        //lbAvailableAdminSubCategories.Items.Remove(item);
                    }
                }
                foreach (ListItem item in lbSelectedITSubCategories.Items)
                {
                    lbAvailableITSubCategories.Items.Remove(item);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "btnITCopy_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnITRemove_Click(object sender, System.EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbSelectedITSubCategories.Items)
                {
                    if (item.Selected == true)
                    {
                        lbAvailableITSubCategories.Items.Add(item);
                    }
                }
                foreach (ListItem item in lbAvailableITSubCategories.Items)
                {
                    lbSelectedITSubCategories.Items.Remove(item);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "btnITRemove_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnITCopyAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                SelectAllIT();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "btnITCopyAll_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnITRemoveAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                RemoveAllIT();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "btnITRemoveAll_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnHRCopy_Click(object sender, System.EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbAvailableHRSubCategories.Items)
                {
                    if (item.Selected == true)
                    {
                        if (!lbSelectedHRSubCategories.Items.Contains(item))
                        {
                            lbSelectedHRSubCategories.Items.Add(item);
                        }
                        //lbAvailableAdminSubCategories.Items.Remove(item);
                    }
                }
                foreach (ListItem item in lbSelectedHRSubCategories.Items)
                {
                    lbAvailableHRSubCategories.Items.Remove(item);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnHRRemove_Click(object sender, System.EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbSelectedHRSubCategories.Items)
                {
                    if (item.Selected == true)
                    {
                        lbAvailableHRSubCategories.Items.Add(item);
                    }
                }
                foreach (ListItem item in lbAvailableHRSubCategories.Items)
                {
                    lbSelectedHRSubCategories.Items.Remove(item);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "btnHRRemove_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnHRCopyAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                SelectAllHR();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "btnHRCopyAll_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnHRRemoveAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                RemoveAllHR();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "btnHRRemoveAll_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgCategories_IsCateogrySelected(object sender, System.EventArgs e)
        {
            try
            {
                CheckBox chkIsCategorySelected = (CheckBox)sender;
                DataGridItem item = (DataGridItem)chkIsCategorySelected.Parent.Parent;
                clsBLSubCategoryAssignment objClsBLSubCategoryAssignment = new clsBLSubCategoryAssignment();
                clsSubCategoryAssignment objClsSubCategoryAssignment = new clsSubCategoryAssignment();
                DataSet dsAdminSubCategories = objClsBLSubCategoryAssignment.getSubCategories();
                if (((CheckBox)item.FindControl("chkBoxCategory")).Text == "Admin")
                {
                    if (chkIsCategorySelected.Checked == false)
                    {
                        RemoveAllAdmin();
                    }
                }
                else if (((CheckBox)item.FindControl("chkBoxCategory")).Text == "IT")
                {
                    if (chkIsCategorySelected.Checked == false)
                    {
                        RemoveAllIT();
                    }
                }
                else if (((CheckBox)item.FindControl("chkBoxCategory")).Text == "HR")
                {
                    if (chkIsCategorySelected.Checked == false)
                    {
                        RemoveAllHR();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "dgCategories_IsCateogrySelected", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /*public void IsSubCateogrySelected(object sender, System.EventArgs e)
        {
            CheckBox chkIsCategorySelected = (CheckBox)sender;
            DataGridItem item = (DataGridItem)chkIsCategorySelected.Parent.Parent;
            clsBLSubCategoryAssignment objClsBLSubCategoryAssignment = new clsBLSubCategoryAssignment();
            clsSubCategoryAssignment objClsSubCategoryAssignment = new clsSubCategoryAssignment();
            DataSet dsAdminSubCategories = objClsBLSubCategoryAssignment.getSubCategories();
            if(((CheckBox)item.FindControl("chkBoxCategory")).Text == "Admin" && lbSelectedAdminSubCategories.Items.Count == 0)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please enter a Category on un-check the Admin Department.";
            }
            else if(((CheckBox)item.FindControl("chkBoxCategory")).Text == "IT" && lbSelectedITSubCategories.Items.Count == 0)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please enter a Category on un-check the IT Department.";
            }
            else if(((CheckBox)item.FindControl("chkBoxCategory")).Text == "HR" && lbSelectedHRSubCategories.Items.Count == 0)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please enter a Category on un-check the HR Department.";
            }
        }*/

        public void dgCategories_isSysAdmin(object sender, System.EventArgs e)
        {
            try
            {
                CheckBox chkIsSysAdmin = (CheckBox)sender;
                DataGridItem dgi = (DataGridItem)chkIsSysAdmin.Parent.Parent;
                if (((CheckBox)dgi.FindControl("chkBoxCategory")).Text == "Admin")
                {
                    if (chkIsSysAdmin.Checked == true)
                    {
                        SelectAllAdmin();
                    }
                    else if (chkIsSysAdmin.Checked == false)
                    {
                        RemoveAllAdmin();
                    }
                }
                else if (((CheckBox)dgi.FindControl("chkBoxCategory")).Text == "IT")
                {
                    if (chkIsSysAdmin.Checked == true)
                    {
                        SelectAllIT();
                    }
                    else if (chkIsSysAdmin.Checked == false)
                    {
                        RemoveAllIT();
                    }
                }
                else if (((CheckBox)dgi.FindControl("chkBoxCategory")).Text == "HR")
                {
                    if (chkIsSysAdmin.Checked == true)
                    {
                        SelectAllHR();
                    }
                    else if (chkIsSysAdmin.Checked == false)
                    {
                        RemoveAllHR();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void SelectAllAdmin()
        {
            try
            {
                foreach (ListItem item in lbAvailableAdminSubCategories.Items)
                {
                    lbSelectedAdminSubCategories.Items.Add(item);
                }
                foreach (ListItem item in lbSelectedAdminSubCategories.Items)
                {
                    lbAvailableAdminSubCategories.Items.Remove(item);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "SelectAllAdmin", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void SelectAllIT()
        {
            try
            {
                foreach (ListItem item in lbAvailableITSubCategories.Items)
                {
                    lbSelectedITSubCategories.Items.Add(item);
                }
                foreach (ListItem item in lbSelectedITSubCategories.Items)
                {
                    lbAvailableITSubCategories.Items.Remove(item);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "SelectAllIT", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void SelectAllHR()
        {
            try
            {
                foreach (ListItem item in lbAvailableHRSubCategories.Items)
                {
                    lbSelectedHRSubCategories.Items.Add(item);
                }
                foreach (ListItem item in lbSelectedHRSubCategories.Items)
                {
                    lbAvailableHRSubCategories.Items.Remove(item);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "SelectAllHR", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void RemoveAllAdmin()
        {
            try
            {
                foreach (ListItem item in lbSelectedAdminSubCategories.Items)
                {
                    lbAvailableAdminSubCategories.Items.Add(item);
                }
                foreach (ListItem item in lbAvailableAdminSubCategories.Items)
                {
                    lbSelectedAdminSubCategories.Items.Remove(item);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "RemoveAllAdmin", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void RemoveAllIT()
        {
            try
            {
                foreach (ListItem item in lbSelectedITSubCategories.Items)
                {
                    lbAvailableITSubCategories.Items.Add(item);
                }
                foreach (ListItem item in lbAvailableITSubCategories.Items)
                {
                    lbSelectedITSubCategories.Items.Remove(item);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "RemoveAllIT", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void RemoveAllHR()
        {
            try
            {
                foreach (ListItem item in lbSelectedHRSubCategories.Items)
                {
                    lbAvailableHRSubCategories.Items.Add(item);
                }
                foreach (ListItem item in lbAvailableHRSubCategories.Items)
                {
                    lbSelectedHRSubCategories.Items.Remove(item);
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "RemoveAllHR", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnReset_Click(object sender, System.EventArgs e)
        {
            try
            {
                ddlEmployeeName.SelectedIndex = 0;
                RemoveAllAdmin();
                RemoveAllHR();
                RemoveAllIT();
                foreach (DataGridItem dgCategoriesItem in dgCategories.Items)
                {
                    if (dgCategoriesItem.ItemType == ListItemType.AlternatingItem || dgCategoriesItem.ItemType == ListItemType.Item)
                    {
                        if (((CheckBox)dgCategoriesItem.FindControl("chkBoxSysAdmin")).Checked == true)
                        {
                            ((CheckBox)dgCategoriesItem.FindControl("chkBoxSysAdmin")).Checked = false;
                        }
                        if (((CheckBox)dgCategoriesItem.FindControl("chkBoxCategory")).Checked == true)
                        {
                            ((CheckBox)dgCategoriesItem.FindControl("chkBoxCategory")).Checked = false;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "btnReset_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgCategories_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                foreach (DataGridItem item in dgCategories.Items)
                {
                    if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                    {
                        if (((Label)item.FindControl("lblIsActive")).Text == "False")
                        {
                            ((CheckBox)item.FindControl("chkBoxCategory")).Enabled = false;
                            ((CheckBox)item.FindControl("chkBoxSysAdmin")).Enabled = false;
                        }
                        else if (((Label)item.FindControl("lblIsActive")).Text == "True")
                        {
                            ((CheckBox)item.FindControl("chkBoxCategory")).Enabled = true;
                            ((CheckBox)item.FindControl("chkBoxSysAdmin")).Enabled = true;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SubCategoryAssignment.aspx", "dgCategories_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void dgCategories_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }
    }
}