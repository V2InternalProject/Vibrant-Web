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
    /// Summary description for categoryMaster.
    /// </summary>
    public partial class categoryMaster : System.Web.UI.Page
    {
        #region Variable declaration

        private DataSet dsCategories;
        private clsBLCategory objclsBLCategory = new clsBLCategory();
        private string strCategories = "";
        private DataSet dsGetEmployeeName = new DataSet();
        public int EmployeeID, SAEmployeeID;
        private clsCategory objClsCategory = new clsCategory();
        private static int strCatagoriesID;
        private string strCatagory = "";
        private DataSet dsEmployeeName = new DataSet();
        private clsCategory objClsCategories = new clsCategory();
        private int gridIndex = 0;

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
                if (!IsPostBack)
                {
                    //if(EmployeeID.ToString() == "" || EmployeeID == 0)
                    //{
                    if (SAEmployeeID.ToString() == "" || SAEmployeeID == 0)
                    {
                        //Response.Redirect("Login.aspx");
                        Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
                    }
                    //}
                    //else
                    //{
                    //    Response.Redirect("AuthorizationErrorMessage.aspx");
                    //}
                    getCategories();
                    lblError.Text = "";
                    lblError.CssClass = "error";
                    //GetCategoryEmployeeName();
                    //lbtnAddCategory.Attributes.Add("onClick", "return setFocus();");
                }
                btnSubmit.Attributes.Add("onClick", "return isRequire('txtAddCategory','Category',this.enabled)");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void getCategories()
        {
            try
            {
                clsBLCategory objclsBLCategory = new clsBLCategory();
                dsCategories = new DataSet();
                dsCategories = objclsBLCategory.getCategories();
                if (dsCategories.Tables[0].Rows.Count > 0)
                {
                    dgCategories.DataSource = dsCategories.Tables[0];
                    dgCategories.DataBind();
                    if (dgCategories.PageCount > 1)
                    {
                        dgCategories.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgCategories.PagerStyle.Visible = false;
                    }

                    lblRecordMsg.Visible = false;
                }
                else if (dsCategories.Tables[0].Rows.Count <= 0)
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "getCategories", ex.StackTrace);
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
            this.dgCategories.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgCategories_ItemCreated);
            this.dgCategories.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgCategories_ItemCommand);
            this.dgCategories.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgCategories_PageIndexChanged);
        }

        #endregion Web Form Designer generated code

        protected void lbtnAddCategory_Click(object sender, System.EventArgs e)
        {
            try
            {
                resetPageControls();
                pnlAddCategory.Visible = true;
                btnUpdate.Visible = false;
                lblMessage.Visible = false;
                btnSubmit.Visible = true;
                lblEmployeeName.Visible = true;
                ddlEmployeeName.Visible = true;
                EmployeePanel.Visible = false;
                //ddlEmployeeName.Items.Insert(0,"select");
                GetEmployeeName();
                getCategories();
                this.setfocus(txtAddCategory);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "lbtnAddCategory_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void GetEmployeeName()
        {
            try
            {
                clsBLCategory objclsBLCategory = new clsBLCategory();
                dsGetEmployeeName = objclsBLCategory.GetEmployeeName();
                ddlEmployeeName.Items.Add(new ListItem("Select Administrator", "0"));
                for (int i = 0; i < dsGetEmployeeName.Tables[0].Rows.Count; i++)
                {
                    ddlEmployeeName.Items.Add(new ListItem(dsGetEmployeeName.Tables[0].Rows[i]["Emp_Name"].ToString(), dsGetEmployeeName.Tables[0].Rows[i]["Emp_User_Name"].ToString())); ;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "GetEmployeeName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void GetCategoryEmployeeName(string CategoryName)
        {
            try
            {
                clsBLCategory objclsBLCategory = new clsBLCategory();
                dsEmployeeName = objclsBLCategory.GetCategoryEmployeeName(CategoryName);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "GetCategoryEmployeeName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (txtAddCategory.Text == "")
                {
                    getCategories();
                    //				pnlAddCategory.Visible = true;
                    lblMessage.Visible = true;
                }
                else
                {
                    clsCategory objClsCategory = new clsCategory();
                    clsBLCategory objClsBLCategory = new clsBLCategory();
                    objClsCategory.AddCategories = Server.HtmlEncode(txtAddCategory.Text.Trim());
                    int noOfRowsReturned = 0;
                    noOfRowsReturned = objClsBLCategory.DoesExist(objClsCategory);
                    if (noOfRowsReturned >= 1)
                    {
                        lblMessage.Text = "This DepartMent is already existing in the Table.";
                        lblMessage.Visible = true;
                        //					pnlAddCategory.Visible = true;
                        getCategories();
                    }
                    else
                    {
                        objClsCategory.AddCategories = Server.HtmlEncode(txtAddCategory.Text.Trim());
                        objClsCategory.AddIdActive = Convert.ToInt32(ddlAddStatus.SelectedValue);
                        objClsCategory.EmployeeId = ddlEmployeeName.SelectedValue.ToString();
                        objClsBLCategory.InsertCategory(objClsCategory);
                        pnlAddCategory.Visible = false;
                        txtAddCategory.Text = "";
                        ddlAddStatus.SelectedIndex = 0;
                        getCategories();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (btnUpdate.Visible == false)
                {
                    resetPageControls();
                    //			pnlAddCategory.Visible = true;

                    this.setfocus(txtAddCategory);
                }
                else if (btnUpdate.Visible == true)
                {
                    lblError.Text = "";
                    pnlAddCategory.Visible = false;
                }
                getCategories();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgCategories_Edit(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                pnlAddCategory.Visible = true;
                btnSubmit.Visible = false;
                btnUpdate.Visible = true;
                lblEmployeeName.Visible = true;
                EmployeePanel.Visible = false;
                getCategories();

                string strCategoryID = dgCategories.DataKeys[e.Item.ItemIndex].ToString();

                Label lblCategory = (Label)dgCategories.Items[e.Item.ItemIndex].FindControl("lblCategory");
                strCatagoriesID = Convert.ToInt32(strCategoryID.ToString());

                txtAddCategory.Text = lblCategory.Text;
                GetCategoryEmployeeName(strCategoryID);
                if (dsEmployeeName.Tables[0].Rows.Count > 0)
                {
                    lblEmployeeName.Visible = false;
                    ddlEmployeeName.Visible = false;
                }
                else
                {
                    GetEmployeeName();
                    lblEmployeeName.Visible = true;
                    ddlEmployeeName.Visible = true;

                    if (dsEmployeeName.Tables[0].Rows.Count == 1)
                    {
                        ddlEmployeeName.SelectedValue = dsEmployeeName.Tables[0].Rows[0][0].ToString();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "dgCategories_Edit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgCategories_Cancel(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                dgCategories.EditItemIndex = -1;
                lblError.Text = "";
                getCategories();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "dgCategories_Cancel", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgCategories_Delete(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                pnlAddCategory.Visible = false;
                int categoryID = Convert.ToInt16(dgCategories.DataKeys[e.Item.ItemIndex]);
                clsCategory objClsCategory = new clsCategory();
                objClsCategory.CategoryID = categoryID;
                clsBLCategory objClsBLCategory = new clsBLCategory();
                int NoOfRowsReturned = objClsBLCategory.DeleteCategory(objClsCategory);
                if (NoOfRowsReturned <= 0)
                {
                    //lblError.Visible = true;
                    //lblError.Text = "Cannot delete this department as some issues under this department are not yet resolved or closed.";
                    Page.RegisterStartupScript("key", "<script>alert('Cannot delete this department as some issues under this department are not yet resolved or closed.');</script>");
                }
                else
                {
                    lblError.Visible = false;
                    lblError.Text = "";
                }
                dgCategories.EditItemIndex = -1;
                getCategories();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "dgCategories_Delete", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgCategories_Status(object sender, DataGridItemEventArgs e)
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
                    //	gridIndex++;*/
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "dgCategories_Status", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgCategories_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lbtnDelete1 = (LinkButton)e.Item.Cells[4].FindControl("lbtnDelete");
                    lbtnDelete1.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this record?')");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "dgCategories_ItemCreated", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void resetPageControls()
        {
            try
            {
                txtAddCategory.Text = "";
                ddlAddStatus.SelectedIndex = 0;
                lblMessage.Text = "";
                ddlEmployeeName.SelectedIndex = 0;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "resetPageControls", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void PreRenderDropdownList(object sender, System.EventArgs e)
        {
            try
            {
                DropDownList ddlCategoryStatus = new DropDownList();
                ddlCategoryStatus = (DropDownList)sender;
                if (strCategories == "False" || strCategories == "0")
                    ddlCategoryStatus.SelectedIndex = ddlCategoryStatus.Items.IndexOf(ddlCategoryStatus.Items.FindByText("InActive"));
                else if (strCategories == "True" || strCategories == "1")
                    ddlCategoryStatus.SelectedIndex = ddlCategoryStatus.Items.IndexOf(ddlCategoryStatus.Items.FindByText("Active"));
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "PreRenderDropdownList", ex.StackTrace);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "setfocus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void ddlAddStatus_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        private void dgCategories_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            try
            {
                dgCategories.CurrentPageIndex = e.NewPageIndex;
                dgCategories.EditItemIndex = -1;
                getCategories();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "dgCategories_PageIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void dgCategories_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                pnlAddCategory.Visible = false;
                EmployeePanel.Visible = true;
                DataSet dsEmployee = new DataSet();
                clsBLCategory objClsBLCategory = new clsBLCategory();
                clsCategory objClsCategory = new clsCategory();
                if (e.CommandName == "View")
                {
                    strCatagory = null;
                    objClsCategory.CategoryID = Convert.ToInt32(dgCategories.DataKeys[e.Item.ItemIndex]);

                    Label lblCategory = (Label)dgCategories.Items[e.Item.ItemIndex].FindControl("lblCategory");
                    lblEmployeeList.Text = "List of Administrator for Department <b> " + lblCategory.Text + " </b> ";

                    dsEmployee = objClsBLCategory.GetCategoryEmployeeNames(objClsCategory);

                    for (int i = 0; i < dsEmployee.Tables[0].Rows.Count; i++)
                    {
                        strCatagory = strCatagory + "" + dsEmployee.Tables[0].Rows[i][0].ToString();
                        strCatagory = strCatagory + "</br>";
                        lblEmployeename1.Text = strCatagory.Replace("<br>", "/r/n");
                    }
                    if (dsEmployee.Tables[0].Rows.Count == 0)
                    {
                        lblEmployeeList.Text = " Administrator Not available for <b>" + lblCategory.Text + " </b> Department";
                        lblEmployeename1.Text = "";
                        //lblEmployeename1.Text = dsEmployee.Tables[0].Rows[0][0].ToString();
                    }
                    EmployeePanel.Visible = true;
                }
                getCategories();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "dgCategories_ItemCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnUpdate_Click(object sender, System.EventArgs e)
        {
            try
            {
                EmployeePanel.Visible = false;
                objClsCategories.Category = txtAddCategory.Text; ;
                objClsCategories.CategoryID = strCatagoriesID;
                objClsCategories.EmployeeId = ddlEmployeeName.SelectedValue;
                objClsCategories.isActive = Convert.ToInt32(ddlAddStatus.SelectedValue);
                updateCategories(objClsCategories);
                getCategories();
                pnlAddCategory.Visible = false;
                lblError.Text = "Department Details updated successfully";
                lblError.CssClass = "success";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "btnUpdate_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public int updateCategories(clsCategory objClsCategories)
        {
            try
            {
                return objclsBLCategory.updateCategories(objClsCategories);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "categoryMaster.aspx", "updateCategories", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void dgCategories_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        /*public void dgCategories_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
        {
            dgCategories.CurrentPageIndex = e.NewPageIndex;
            getCategories();
        }*/
    }
}