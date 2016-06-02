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
    /// Summary description for subCategoryMaster.
    /// </summary>
    public partial class subCategoryMaster : System.Web.UI.Page
    {
        #region Variable declaration

        private DataSet dsSubCategories = new DataSet();
        private DataSet dsCategoryID = new DataSet();
        private string strSubCategoryStatus = "", subCategory = "";
        private string strCategoryName = "", strSubCategoryName = "";
        public int EmployeeID, SAEmployeeID, SuperAdmin;

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
                SuperAdmin = Convert.ToInt32(Session["SuperAdmin"]);
                // OnlySuperAdmin = Convert.ToInt32(Session["OnlySuperAdmin"]);
                //			pnlAddSubCategory.Visible = false;
                //if (EmployeeID.ToString() == "" || EmployeeID == 0 || SuperAdmin != 0 || OnlySuperAdmin != 0)
                //{
                //    if ((SAEmployeeID.ToString() == "" || SAEmployeeID == 0) && SuperAdmin == 0 && OnlySuperAdmin == 0)
                //    {
                //        Response.Redirect("Login.aspx");
                //    }
                //}
                if (SuperAdmin == 0)
                {
                    if ((SAEmployeeID.ToString() != "" || SAEmployeeID != 0))
                        Response.Redirect("AuthorizationErrorMessage.aspx");
                    else
                        Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
                }
                //{
                //    Response.Redirect("Login.aspx");
                //}
                //else
                //{
                //    Response.Redirect("AuthorizationErrorMessage.aspx");
                //}
                if (!Page.IsPostBack)
                {
                    getSubCategories();
                    clsBLSubCategory objClsBLSubCategory = new clsBLSubCategory();
                    dsCategoryID = objClsBLSubCategory.getCategoryID();
                    ddlAddCategory.DataSource = dsCategoryID.Tables[0];
                    ddlAddCategory.DataValueField = dsCategoryID.Tables[0].Columns["CategoryID"].ToString();
                    ddlAddCategory.DataTextField = dsCategoryID.Tables[0].Columns["Category"].ToString();
                    ddlAddCategory.DataBind();
                    EmployeePanel.Visible = false;
                }

                btnSubmit.Attributes.Add("onClick", "return isRequire('txtAddSubCategory','Category');");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "Page_Load", ex.StackTrace);
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
        }

        #endregion Web Form Designer generated code

        public void getSubCategories()
        {
            try
            {
                clsBLSubCategory objclsBLSubCategory = new clsBLSubCategory();
                clsCategory objClsCategory = new clsCategory();
                dsSubCategories = objclsBLSubCategory.getSubCategories();
                if (dsSubCategories.Tables[0].Rows.Count > 0)
                {
                    dgSubCategories.DataSource = dsSubCategories.Tables[0];
                    dgSubCategories.DataBind();
                    if (dgSubCategories.PageCount > 1)
                    {
                        dgSubCategories.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgSubCategories.PagerStyle.Visible = false;
                    }

                    lblRecordMsg.Visible = false;
                }
                else if (dsSubCategories.Tables[0].Rows.Count <= 0)
                {
                    lblRecordMsg.Visible = true;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "getSubCategories", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgSubCategories_Edit(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                //strSubCategoryStatus = ((DropDownList)e.Item.Cells[2].FindControl("ddlCategory")).SelectedItem.Value.ToString();
                //DropDownList ddl;
                //ddl = ((DropDownList)e.Item.FindControl("ddlCategory"));
                //ddl.Attributes.Add("readonly", "readonly");

                // //DropDownList ddl= new DropDownList();
                // //ddl = (e.Item.FindControl("ddlCategory") as DropDownList);
                // ddl.Attributes.Add("readonly", "readonly");
                lblSuccessMsgs.Text = "";
                lblEmployeeList.Text = "";
                lblEmployeename.Text = "";
                lblError.Text = "";
                int i = dgSubCategories.CurrentPageIndex;
                int itemIndex = e.Item.ItemIndex;
                if (i < 0)
                {
                    itemIndex = i * 10 + itemIndex;
                }

                pnlAddSubCategory.Visible = false;
                clsBLSubCategory objclsBLSubCategory = new clsBLSubCategory();
                dsSubCategories = objclsBLSubCategory.getSubCategories();

                strSubCategoryStatus = dsSubCategories.Tables[0].Rows[e.Item.DataSetIndex][4].ToString();
                ViewState["SubCategoryName"] = dsSubCategories.Tables[0].Rows[e.Item.DataSetIndex][3].ToString();

                strCategoryName = dsSubCategories.Tables[0].Rows[e.Item.DataSetIndex][2].ToString();
                dgSubCategories.EditItemIndex = (int)e.Item.ItemIndex;
                getSubCategories();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "dgSubCategories_Edit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgSubCategories_Update(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                lblEmployeeList.Text = "";
                lblEmployeename.Text = "";
                clsBLSubCategory objclsBLSubCategory = new clsBLSubCategory();
                dsSubCategories = objclsBLSubCategory.getSubCategories();

                strSubCategoryStatus = ((DropDownList)e.Item.Cells[4].FindControl("ddlStatus")).SelectedItem.Value.ToString();//dsSubCategories.Tables[0].Rows[e.Item.ItemIndex][4].ToString();
                int subCategoryID = Convert.ToInt32(dgSubCategories.DataKeys[e.Item.ItemIndex]);
                string newSubCategory = Server.HtmlEncode(((TextBox)e.Item.FindControl("txtSubCategory")).Text.Trim());
                clsSubCategory objClsSubCategory = new clsSubCategory();
                if (newSubCategory == "")
                {
                    lblError.Visible = true;
                    lblError.Text = "Please fill the Category.";
                    getSubCategories();
                }
                else
                {
                    lblError.Text = "";
                    DataSet dsNoOfRowsReturned = new DataSet();

                    int newCategory = Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlCategory")).SelectedItem.Value);
                    objClsSubCategory.NewCategory = newCategory;
                    objClsSubCategory.NewSubCategory = newSubCategory;
                    dsNoOfRowsReturned = objclsBLSubCategory.DoesExistWhenEdited(objClsSubCategory);
                    //int subCatID = Convert.ToInt32(dsSubCategories.Tables[0].Rows[e.Item.ItemIndex][0]);
                    int subCatID = Convert.ToInt32(e.Item.Cells[0].Text);
                    if (dsNoOfRowsReturned.Tables[0].Rows.Count > 0 && Convert.ToInt32(dsNoOfRowsReturned.Tables[0].Rows[0][1]) == newCategory)// && objClsSubCategory.SubCategoryID != subCatID)
                    {
                        if (subCatID != Convert.ToInt32(dsNoOfRowsReturned.Tables[0].Rows[0][0]))
                        {
                            lblError.Visible = true;
                            lblError.Text = "This Department/Category is already Existing in the Table.";
                            getSubCategories();
                        }
                        else
                        {
                            objClsSubCategory.NewSubCategory = newSubCategory;
                            objClsSubCategory.SubCategoryID = subCategoryID;
                            objClsSubCategory.NewCategory = Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlCategory")).SelectedItem.Value);
                            int status = Convert.ToInt32(((DropDownList)e.Item.Cells[4].FindControl("ddlStatus")).SelectedItem.Value);//Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlStatus")).SelectedItem.Value);
                            objClsSubCategory.IsActive = status;
                            int noOfRecordsReturned = objclsBLSubCategory.updateSubCategory(objClsSubCategory);
                            if (noOfRecordsReturned > 0)
                            {
                                //if (status == 0)
                                //{
                                lblError.Visible = true;
                                //lblError.Text = "Cannot Delete this Department as some issues under this Department are not yet resolved or closed.";
                                Page.RegisterStartupScript("key", "<script>alert('Cannot update the details, as some issues under this Department are not yet resolved or closed.');</script>");
                                ViewState["SubCategoryName"] = null;
                                //}
                            }
                            else
                            {
                                dgSubCategories.EditItemIndex = -1;
                                lblError.Visible = false;
                                getSubCategories();
                                lblSuccessMsgs.Visible = true;
                                lblSuccessMsgs.Text = "Details updated successfully";
                                ViewState["SubCategoryName"] = null;
                            }
                            dgSubCategories.EditItemIndex = -1;
                            lblError.Visible = false;
                            getSubCategories();
                        }
                    }
                    else
                    {
                        objClsSubCategory.NewSubCategory = newSubCategory;
                        objClsSubCategory.SubCategoryID = subCategoryID;
                        objClsSubCategory.NewCategory = Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlCategory")).SelectedItem.Value);
                        int status = ((DropDownList)e.Item.Cells[4].FindControl("ddlStatus")).SelectedIndex;//Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlStatus")).SelectedItem.Value);
                        objClsSubCategory.IsActive = status;
                        int noOfRecordsReturned = objclsBLSubCategory.updateSubCategory(objClsSubCategory);
                        if (noOfRecordsReturned > 0)
                        {
                            //if (status == 0)
                            //{
                            lblError.Visible = true;
                            //lblError.Text = "Cannot Delete this Department as some issues under this Department are not yet resolved or closed.";
                            Page.RegisterStartupScript("key", "<script>alert('Cannot update the details as some issues under this Department are not yet resolved or closed.');</script>");
                            ViewState["SubCategoryName"] = null;
                            //}
                        }
                        else
                        {
                            dgSubCategories.EditItemIndex = -1;
                            lblError.Visible = false;
                            getSubCategories();
                            lblSuccessMsgs.Visible = true;
                            lblSuccessMsgs.Text = "Details updated successfully";
                            ViewState["SubCategoryName"] = null;
                        }
                        dgSubCategories.EditItemIndex = -1;
                        lblError.Visible = false;
                        getSubCategories();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "dgSubCategories_Update", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgSubCategories_Cancel(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblSuccessMsgs.Text = "";
                lblEmployeeList.Text = "";
                lblEmployeename.Text = "";
                pnlAddSubCategory.Visible = false;
                dgSubCategories.EditItemIndex = -1;
                getSubCategories();
                ViewState["SubCategoryName"] = null;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "dgSubCategories_Cancel", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgSubCategories_Delete(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                pnlAddSubCategory.Visible = false;
                int subCategoryID = Convert.ToInt16(dgSubCategories.DataKeys[e.Item.ItemIndex]);
                clsSubCategory objClsSubCategory = new clsSubCategory();
                objClsSubCategory.SubCategoryID = subCategoryID;
                clsBLSubCategory objClsBLSubCategory = new clsBLSubCategory();
                int noOfRowsReturned = objClsBLSubCategory.DeleteSubCategory(objClsSubCategory);
                if (noOfRowsReturned <= 0)
                {
                    Page.RegisterStartupScript("key", "<script>alert('Cannot delete this Department as some issues under this Department are not yet resolved or closed.');</script>");
                }
                dgSubCategories.EditItemIndex = -1;
                getSubCategories();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "dgSubCategories_Delete", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgSubCategories_Status(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblStatus = (Label)e.Item.FindControl("lblstatus");

                    if (lblStatus.Text == "True")
                    {
                        lblStatus.Text = "Active";
                    }
                    else if (lblStatus.Text == "False")
                    {
                        lblStatus.Text = "Inactive";
                    }
                }
                else if (e.Item.ItemType == ListItemType.EditItem)
                {
                    clsBLSubCategory objClsBLSubCategory = new clsBLSubCategory();
                    dsCategoryID = objClsBLSubCategory.getCategoryID();
                    DropDownList ddlCategory = (DropDownList)e.Item.Cells[3].FindControl("ddlCategory");
                    ddlCategory.DataSource = dsCategoryID.Tables[0];
                    ddlCategory.DataValueField = dsCategoryID.Tables[0].Columns["CategoryID"].ToString();
                    ddlCategory.DataTextField = dsCategoryID.Tables[0].Columns["Category"].ToString();
                    ddlCategory.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "dgSubCategories_Status", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblSuccessMsgs.Text = "";
                lblEmployeeList.Text = "";
                lblEmployeename.Text = "";
                resetPageControls();
                getSubCategories();
                this.setfocus(txtAddSubCategory);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                clsBLSubCategory objClsBLSubCategory = new clsBLSubCategory();
                clsSubCategory objClsSubCategory = new clsSubCategory();
                objClsSubCategory.AddSubCategories = Server.HtmlEncode(txtAddSubCategory.Text);
                objClsSubCategory.AddCategory = Convert.ToInt32(ddlAddCategory.SelectedItem.Value);
                int noOfRowsReturned = 0;
                noOfRowsReturned = objClsBLSubCategory.DoesExist(objClsSubCategory);
                if (txtAddSubCategory.Text == "")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "please enter Category";
                    //lblmessage.visible = true;
                    //pnladdsubcategory.visible = true;
                    //getsubcategories();
                }
                else
                {
                    if (noOfRowsReturned >= 1)
                    {
                        lblMessage.Text = "This Category is already existing in the Table.";
                        lblMessage.Visible = true;
                        //					pnlAddSubCategory.Visible = true;
                        getSubCategories();
                    }
                    else
                    {
                        //clsSubCategory objClsSubCategory = new clsSubCategory();
                        objClsSubCategory.AddCategory = Convert.ToInt32(ddlAddCategory.SelectedItem.Value);
                        objClsSubCategory.AddSubCategories = Server.HtmlEncode(txtAddSubCategory.Text.Trim());
                        objClsSubCategory.AddIsActive = Convert.ToInt32(ddlAddStatus.SelectedIndex);
                        //clsBLSubCategory objClsBLSubCategory = new clsBLSubCategory();
                        objClsBLSubCategory.InsertSubCategory(objClsSubCategory);
                        pnlAddSubCategory.Visible = false;
                        txtAddSubCategory.Text = "";
                        ddlAddStatus.SelectedIndex = 0;
                        getSubCategories();
                        lblSuccessMsgs.Visible = true;
                        lblSuccessMsgs.Text = "Category created successfully";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void lbtnAddSubCategory_Click(object sender, System.EventArgs e)
        {
            try
            {
                resetPageControls();
                pnlAddSubCategory.Visible = true;
                getSubCategories();
                this.setfocus(txtAddSubCategory);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "lbtnAddSubCategory_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgSubCategories_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lbtnDelete1 = (LinkButton)e.Item.Cells[6].FindControl("lbtnDelete");
                    lbtnDelete1.Attributes.Add("onClick", "return confirm('Are you sure you want to Delete this Record?')");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "dgSubCategories_ItemCreated", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void resetPageControls()
        {
            try
            {
                txtAddSubCategory.Text = "";
                ddlAddCategory.SelectedIndex = 0;
                //ddlAddStatus.SelectedIndex = 0;
                lblMessage.Text = "";
                lblError.Text = "";
                lblCategory.Text = "";
                lblSuccessMsgs.Text = "";
                lblEmployeename.Text = "";
                lblEmployeeList.Text = "";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "resetPageControls", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void PreRenderddlStatus(object sender, System.EventArgs e)
        {
            try
            {
                DropDownList ddlSubCategoryStatus = new DropDownList();
                ddlSubCategoryStatus = (DropDownList)sender;
                if (strSubCategoryStatus == "False" || strSubCategoryStatus == "0")
                    ddlSubCategoryStatus.SelectedIndex = ddlSubCategoryStatus.Items.IndexOf(ddlSubCategoryStatus.Items.FindByText("InActive"));
                else if (strSubCategoryStatus == "True" || strSubCategoryStatus == "1")
                    ddlSubCategoryStatus.SelectedIndex = ddlSubCategoryStatus.Items.IndexOf(ddlSubCategoryStatus.Items.FindByText("Active"));
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "PreRenderddlStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void PreRenderddlCategory(object sender, System.EventArgs e)
        {
            try
            {
                DropDownList ddlCategoryName = (DropDownList)sender;
                ddlCategoryName.SelectedIndex = ddlCategoryName.Items.IndexOf(ddlCategoryName.Items.FindByText(strCategoryName));
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "PreRenderddlCategory", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void setfocus(System.Web.UI.Control ctrl)
        {
            try
            {
                string s = "<SCRIPT language='javascript'>if(document.getElementById('" + ctrl.ID + "')!=null) document.getElementById('" +
                    ctrl.ID + "').focus();</SCRIPT>";
                RegisterStartupScript("focus", s);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "setfocus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgSubCategories_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
        {
            try
            {
                dgSubCategories.CurrentPageIndex = e.NewPageIndex;
                dgSubCategories.EditItemIndex = -1;
                getSubCategories();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "dgSubCategories_PageIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void dgSubCategories_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                DataSet dsEmployeeName = new DataSet();
                clsBLSubCategory objClsBLSubCategory = new clsBLSubCategory();
                clsSubCategory objClsSubCategory = new clsSubCategory();
                Label lblCategory = new Label();
                if (e.CommandName == "View")
                {
                    lblError.Text = "";
                    lblSuccessMsgs.Text = "";
                    lblEmployeeList.Text = "";
                    lblEmployeename.Text = "";
                    pnlAddSubCategory.Visible = false;
                    objClsSubCategory.SubCategoryID = Convert.ToInt32(dgSubCategories.DataKeys[e.Item.ItemIndex]);
                    if (ViewState["SubCategoryName"] == null)
                    {
                        lblCategory = (Label)dgSubCategories.Items[e.Item.ItemIndex].FindControl("SubCategory");
                        lblEmployeeList.Text = "List of Employee for Category <b>" + lblCategory.Text + "</b>";
                        dsEmployeeName = objClsBLSubCategory.GetEmployeeName(objClsSubCategory);
                        if (dsEmployeeName.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsEmployeeName.Tables[0].Rows.Count; i++)
                            {
                                subCategory = subCategory + "" + dsEmployeeName.Tables[0].Rows[i][0].ToString();
                                subCategory = subCategory + "</br>";
                                lblEmployeename.Text = subCategory.Replace("<br>", "/r/n");
                            }
                        }
                        else
                        {
                            lblEmployeename.Text = "";
                        }
                        EmployeePanel.Visible = true;
                    } //dgSubCategories.Columns[4]. . [e.Item.ItemIndex].FindControl("SubCategory");
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Cant view details while Editing, Complete Edit operation first.";
                        ViewState["SubCategoryName"] = null;
                        //lblCategory = ((TextBox)dgSubCategories.Items[e.Item.ItemIndex].FindControl("txtSubCategory"));
                    }
                }
                getSubCategories();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "subCategoryMaster.aspx", "dgSubCategories_ItemCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}