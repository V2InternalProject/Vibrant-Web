using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;
using V2.Helpdesk.Model;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for ResolutionTimeMaster.
    /// </summary>
    public partial class ResolutionTimeMaster : System.Web.UI.Page
    {
        private clsResolutionTime objclsResolutionTime = new clsResolutionTime();
        private clsBLResolutionTime objclsBLResolutionTime = new clsBLResolutionTime();

        private DataSet dsCategory = new DataSet();
        private DataSet dsSubCategory = new DataSet();
        private DataSet dsSeverity = new DataSet();

        //protected System.Web.UI.WebControls.Label lblerror;
        private DataSet dsGetResolutionTime = new DataSet();

        private string strCategory = "";
        private string strSubCategory = "";
        private string strSeverity = "";
        private bool isEdit = false;
        private int EmployeeID, SAEmployeeID, SuperAdmin;
        private DataSet dsDuplicateResolution = new DataSet();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                SuperAdmin = Convert.ToInt32(Session["SuperAdmin"]);
                //  OnlySuperAdmin = Convert.ToInt32(Session["OnlySuperAdmin"]);
                //if (EmployeeID.ToString() == "" || EmployeeID == 0 || SuperAdmin != 0 || OnlySuperAdmin != 0)
                //{
                //    if ((SAEmployeeID.ToString() == "" || SAEmployeeID == 0) && SuperAdmin == 0 && OnlySuperAdmin == 0)
                //    {
                //        Response.Redirect("Login.aspx");
                //    }
                //}
                if (SuperAdmin == 0)
                //{
                //    Response.Redirect("Login.aspx");
                //}
                //else
                //{
                //    Response.Redirect("AuthorizationErrorMessage.aspx");
                //}
                {
                    if ((SAEmployeeID.ToString() != "" || SAEmployeeID != 0))
                        Response.Redirect("AuthorizationErrorMessage.aspx");
                    else
                        Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
                }
                if (!IsPostBack)
                {
                    //added
                    // ddlcategory.Items.Clear();
                    // ddlcategory.Items.Add("Select");
                    // ddlcategory.SelectedValue = "Select";
                    //till here
                    bindCategory();
                    bindSubCategory();
                    bindSeverity();
                    bindResolutionTime();
                }
                lblError.Text = "";
                lblRecordMsg.Text = "";

                btnsubmit.Attributes.Add("onClick", "return ParametersRequired();");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "Page_Load", ex.StackTrace);
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
            this.dgResolutionTime.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgResolutionTime_ItemCreated);
        }

        #endregion Web Form Designer generated code

        #region User defind Function

        public void bindCategory()
        {
            try
            {
                //added
                //  ddlcategory.Items.Clear();
                //  ddlcategory.Items.Add("Select");
                // ddlsubcategory.Items.Add("Select");

                //till here
                dsCategory = objclsBLResolutionTime.getCategory();
                ddlcategory.DataSource = dsCategory.Tables[0];
                ddlcategory.DataTextField = dsCategory.Tables[0].Columns["Category"].ToString();
                ddlcategory.DataValueField = dsCategory.Tables[0].Columns["CategoryID"].ToString();
                ddlcategory.DataBind();
                ddlcategory.Items.Insert(0, "Select");
                //added here
                ViewState["select"] = ddlcategory.SelectedItem.Text;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "bindCategory", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void bindSubCategory()
        {
            try
            {
                //added
                ddlsubcategory.Items.Clear();
                //  ddlseverity.Items.Add("Select");
                //   ddlsubcategory.Items.Add("Select");

                //till here
                if (ViewState["select"].ToString() != "Select")
                {
                    if (ddlcategory.Items.FindByText("Select") != null)
                        ddlcategory.Items.RemoveAt(0);
                    objclsResolutionTime.CategoryID = Convert.ToInt32(ddlcategory.SelectedValue.ToString());
                    dsSubCategory = objclsBLResolutionTime.getSubCategory(objclsResolutionTime);
                    //ddlsubcategory.Items.Clear ();
                    for (int i = 0; i < dsSubCategory.Tables[0].Rows.Count; i++)
                    {
                        ddlsubcategory.Items.Add(new ListItem(dsSubCategory.Tables[0].Rows[i]["SubCategory"].ToString(), dsSubCategory.Tables[0].Rows[i]["SubcategoryID"].ToString()));
                    }
                    ddlcategory.Items.Insert(0, "Select");
                    ddlsubcategory.Items.Insert(0, "Select");
                }
                else
                {
                    ddlseverity.Items.Insert(0, "Select");
                    ddlsubcategory.Items.Insert(0, "Select");
                    //ddlseverity.Items.Add("Select");
                    //ddlsubcategory.Items.Add("Select");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "bindSubCategory", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void bindSeverity()
        {
            try
            {
                //added
                ddlseverity.Items.Clear();
                //  ddlseverity.Items.Add("Select");
                //till here
                dsSeverity = objclsBLResolutionTime.getProblemSeverity();
                ddlseverity.DataSource = dsSeverity.Tables[0];
                ddlseverity.DataTextField = dsSeverity.Tables[0].Columns["ProblemSeverity"].ToString();
                ddlseverity.DataValueField = dsSeverity.Tables[0].Columns["ProblemSeverityID"].ToString();
                ddlseverity.DataBind();
                ddlseverity.Items.Insert(0, "Select");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "bindSeverity", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void bindResolutionTime()
        {
            try
            {
                dsGetResolutionTime = objclsBLResolutionTime.getResolutionTime();
                if (dsGetResolutionTime.Tables[0].Rows.Count > 0)
                {
                    dgResolutionTime.DataSource = dsGetResolutionTime.Tables[0];
                    dgResolutionTime.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "bindResolutionTime", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void resetPageControls()
        {
            try
            {
                //added
                // bindCategory();
                //bindSubCategory();
                //bindSeverity();
                //  ddlcategory.SelectedValue = "Select";
                ddlcategory.SelectedValue = "Select";
                ddlsubcategory.SelectedValue = "Select";
                ddlseverity.SelectedValue = "Select";
                //till here
                //   ddlcategory.Items.Insert(0, "Select");

                // ddlsubcategory.SelectedItem.Text = "Select";
                //  ddlseverity.SelectedItem.Text = "Select";
                txtamber.Text = "";
                txtgreen.Text = "";
                lblError.Text = "";
                //lblRecordMsg.Text ="";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "resetPageControls", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion User defind Function

        #region submit button

        protected void btnsubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (txtgreen.Text.Trim() == "")
                {
                    lblError.Text = "Please enter resolution for green";
                }
                else if (txtamber.Text.Trim() == "")
                {
                    lblError.Text = "Please enter resolution for amber";
                }
                else if (ddlcategory.SelectedValue == "Select")
                {
                    lblError.Text = "please select Department";
                }
                else if (ddlsubcategory.SelectedValue == "Select")
                {
                    lblError.Text = "please select Category";
                }
                else if (ddlseverity.SelectedValue == "Select")
                {
                    lblError.Text = "please select Problem severity";
                }
                else
                {
                    //changes done here
                    if (ddlcategory.Items.FindByText("Select") != null && ddlcategory.SelectedValue != "Select")
                    {
                        ddlcategory.Items.RemoveAt(0);
                        objclsResolutionTime.CategoryID = Convert.ToInt32(ddlcategory.SelectedItem.Value);
                    }
                    else
                    {
                        lblError.Text = "please select Department";
                    }
                    if (ddlsubcategory.Items.FindByText("Select") != null && ddlsubcategory.SelectedValue != "Select")
                    {
                        ddlsubcategory.Items.RemoveAt(0);
                        objclsResolutionTime.SubCategoryID = Convert.ToInt32(ddlsubcategory.SelectedItem.Value);
                    }
                    else
                    {
                        lblError.Text = "please select Category";
                    }
                    if (ddlseverity.Items.FindByText("Select") != null && ddlseverity.SelectedValue != "Select")
                    {
                        ddlseverity.Items.RemoveAt(0);
                        objclsResolutionTime.ProblemSeverityId = Convert.ToInt32(ddlseverity.SelectedItem.Value);
                    }
                    else
                    {
                        lblError.Text = "please select Problem severity";
                    }
                    objclsResolutionTime.ResolutionForGreen = txtgreen.Text.Trim();
                    objclsResolutionTime.ResolutionForAmber = txtamber.Text.Trim();

                    dsDuplicateResolution = objclsBLResolutionTime.IsDuplicateResolution(objclsResolutionTime);
                    if (dsDuplicateResolution.Tables[0].Rows.Count > 0)
                    {
                        bindCategory();
                        bindSubCategory();
                        bindSeverity();
                        resetPageControls();
                        bindResolutionTime();
                        lblError.Text = "Don't Enter Duplicate Data";
                    }
                    else if (Convert.ToInt32(txtgreen.Text.Trim()) >= Convert.ToInt32(txtamber.Text.Trim()))
                    {
                        lblError.Text = "Your value is less than/same Resolution for Green. Please enter correct value";
                        txtamber.Text = "";
                    }
                    else
                    {
                        objclsBLResolutionTime.addResolutionTime(objclsResolutionTime);
                        lblRecordMsg.Text = "Records added successfully";
                        bindCategory();
                        bindSubCategory();
                        bindSeverity();
                        resetPageControls();
                        bindResolutionTime();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "btnsubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion submit button

        #region linkbutton

        protected void lbtnAddResolutionTime_Click(object sender, System.EventArgs e)
        {
            try
            {
                resetPageControls();
                pnlAddResolutionMaster.Visible = true;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "lbtnAddResolutionTime_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion linkbutton

        #region Edit,Update,Delete,Cancel,PageIndex,Itemdatabound

        public void dgResolutionTime_Edit(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                pnlAddResolutionMaster.Visible = false;
                dgResolutionTime.EditItemIndex = e.Item.ItemIndex;
                dsGetResolutionTime = objclsBLResolutionTime.getResolutionTime();

                /*strCategory=dsGetResolutionTime.Tables[0].Rows[e.Item.ItemIndex][6].ToString();
                strSubCategory = dsGetResolutionTime.Tables[0].Rows[e.Item.ItemIndex][7].ToString();
                strSeverity = dsGetResolutionTime.Tables[0].Rows[e.Item.ItemIndex][8].ToString();*/
                bindResolutionTime();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "dgResolutionTime_Edit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgResolutionTime_Update(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                Label lblresolutionID = ((Label)e.Item.FindControl("lblresolutionID"));
                objclsResolutionTime.ResolutionID = Convert.ToInt32(lblresolutionID.Text.Trim());

                DropDownList ddlcategory1 = ((DropDownList)e.Item.FindControl("ddlcategory1"));
                objclsResolutionTime.CategoryID = Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlcategory1")).SelectedValue);

                DropDownList ddlsubCategorymaster = ((DropDownList)e.Item.FindControl("ddlsubCategorymaster"));
                objclsResolutionTime.SubCategoryID = Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlsubCategorymaster")).SelectedValue);

                DropDownList ddlseveritymaster = ((DropDownList)e.Item.FindControl("ddlseveritymaster"));
                objclsResolutionTime.ProblemSeverityId = Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlseveritymaster")).SelectedValue);

                TextBox txtgreen1 = ((TextBox)e.Item.FindControl("txtgreen1"));
                objclsResolutionTime.ResolutionForGreen = txtgreen1.Text.Trim();

                TextBox txtamber1 = ((TextBox)e.Item.FindControl("txtamber1"));
                objclsResolutionTime.ResolutionForAmber = txtamber1.Text.Trim();

                dsDuplicateResolution = objclsBLResolutionTime.IsDuplicateResolution(objclsResolutionTime);
                // Validation for enter Duplcate values
                if (dsDuplicateResolution.Tables[0].Rows.Count > 0)
                {
                    lblError.Text = "Don't Enter Duplicate Data";
                }
                //validation for Text Boxes
                else if (txtgreen1.Text.Trim() == "")
                {
                    lblError.Text = "Please enter an integer value for green resolution";
                }
                else if (txtgreen1.Text.Trim() != "")
                {
                    string txtgreen2 = txtgreen1.Text.Trim();
                    for (int i = 0; i < txtgreen2.Length; i++)
                    {
                        if (!char.IsNumber(txtgreen2[i]))
                        {
                            lblError.Text = "Enter Interger value for green resolution";
                            return;
                            //break;
                        }
                    }

                    string strGreen = txtgreen1.Text.Trim();
                    if (strGreen.StartsWith("0") || strGreen.StartsWith("1") ||
                        strGreen.StartsWith("2") || strGreen.StartsWith("3") ||
                        strGreen.StartsWith("4") || strGreen.StartsWith("5") ||
                        strGreen.StartsWith("6") || strGreen.StartsWith("7") ||
                        strGreen.StartsWith("8") || strGreen.StartsWith("9"))
                    {
                        objclsResolutionTime.ResolutionForGreen = strGreen;
                        if (txtamber1.Text.Trim() == "")
                        {
                            lblError.Text = "Please enter resolution for amber";
                            bindResolutionTime();
                        }
                        else if (txtamber1.Text.Trim() != "")
                        {
                            //if (Convert.ToInt32(txtamber1.Text) < 0 && Convert.ToInt32(txtamber1.Text) > 9)

                            //{
                            //}

                            //else
                            // {
                            string txtamber2 = txtamber1.Text.Trim();
                            for (int i = 0; i < txtamber2.Length; i++)
                            {
                                if (!char.IsNumber(txtamber2[i]))
                                {
                                    lblError.Text = "Enter Interger value for amber resolution";
                                    return;
                                    //break;
                                }
                            }
                            string strAmber = txtamber1.Text.Trim();
                            if (strAmber.StartsWith("0") || strAmber.StartsWith("1") ||
                                strAmber.StartsWith("2") || strAmber.StartsWith("3") ||
                                strAmber.StartsWith("4") || strAmber.StartsWith("5") ||
                                strAmber.StartsWith("6") || strAmber.StartsWith("7") ||
                                strAmber.StartsWith("8") || strAmber.StartsWith("9"))
                            {
                                objclsResolutionTime.ResolutionForAmber = strAmber;

                                if (Convert.ToInt32(txtgreen1.Text.Trim()) >= Convert.ToInt32(txtamber1.Text.Trim()))
                                {
                                    lblError.Text = "Your value is less than/same Resolution for Green. Please enter correct value ";
                                }
                                else
                                {
                                    lblError.Text = "";
                                    objclsBLResolutionTime.updateResolutionTime(objclsResolutionTime);
                                    dgResolutionTime.EditItemIndex = -1;
                                    bindResolutionTime();
                                }
                            }
                            else
                            {
                                lblError.Text = "Enter Interger value for amber resolution";
                            }
                        }
                        //}
                    }
                    else
                    {
                        lblError.Text = "Enter Interger value for green resolution";
                    }
                }

                /*else
                {
                    lblError.Text="";
                    //objclsBLResolutionTime.updateResolutionTime (objclsResolutionTime);
                    dgResolutionTime.EditItemIndex=-1;
                    bindResolutionTime();
                }*/

                isEdit = true;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "dgResolutionTime_Update", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgResolutionTime_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    dsCategory = objclsBLResolutionTime.getCategory();
                    DropDownList ddlcategory1 = ((DropDownList)e.Item.FindControl("ddlcategory1"));
                    Label lblcategoryID = ((Label)e.Item.FindControl("lblcategoryID"));
                    for (int i = 0; i < dsCategory.Tables[0].Rows.Count; i++)
                    {
                        ddlcategory1.Items.Add(new ListItem(dsCategory.Tables[0].Rows[i]["Category"].ToString(), dsCategory.Tables[0].Rows[i]["CategoryID"].ToString()));
                    }
                    ddlcategory1.SelectedValue = lblcategoryID.Text.ToString();

                    objclsResolutionTime.CategoryID = Convert.ToInt32(ddlcategory1.SelectedValue.ToString());
                    DropDownList ddlsubcategorymaster = ((DropDownList)e.Item.FindControl("ddlsubcategorymaster"));
                    dsSubCategory = objclsBLResolutionTime.getSubCategory(objclsResolutionTime);
                    //	ddlsubcategorymaster.Items.Clear();
                    Label lblsubcategoryID = ((Label)e.Item.FindControl("lblsubcategoryID"));
                    for (int i = 0; i < dsSubCategory.Tables[0].Rows.Count; i++)
                    {
                        ddlsubcategorymaster.Items.Add(new ListItem(dsSubCategory.Tables[0].Rows[i]["SubCategory"].ToString(), dsSubCategory.Tables[0].Rows[i]["SubcategoryID"].ToString()));
                    }
                    ddlsubcategorymaster.SelectedValue = lblsubcategoryID.Text.ToString();

                    dsSeverity = objclsBLResolutionTime.getProblemSeverity();
                    DropDownList ddlseveritymaster = ((DropDownList)e.Item.FindControl("ddlseveritymaster"));
                    for (int i = 0; i < dsSeverity.Tables[0].Rows.Count; i++)
                    {
                        ddlseveritymaster.Items.Add(new ListItem(dsSeverity.Tables[0].Rows[i]["problemseverity"].ToString(), dsSeverity.Tables[0].Rows[i]["problemseverityID"].ToString()));
                    }
                    Label lblProblemseverityID = ((Label)e.Item.FindControl("lblProblemseverityID"));
                    ddlseveritymaster.SelectedValue = lblProblemseverityID.Text.ToString();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "dgResolutionTime_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgResolutionTime_Delete(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                pnlAddResolutionMaster.Visible = false;
                Label lblresolutionID = ((Label)e.Item.FindControl("lblresolutionID"));
                objclsResolutionTime.ResolutionID = Convert.ToInt32(lblresolutionID.Text.Trim());
                objclsBLResolutionTime.deleteResolutionTime(objclsResolutionTime);
                //dgResolutionTime.EditItemIndex=-1;
                bindResolutionTime();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "dgResolutionTime_Delete", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgResolutionTime_cancel(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                dgResolutionTime.EditItemIndex = -1;
                bindResolutionTime();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "dgResolutionTime_cancel", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgResolutionTime_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
        {
            try
            {
                dgResolutionTime.CurrentPageIndex = e.NewPageIndex;

                dgResolutionTime.EditItemIndex = -1;
                bindResolutionTime();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "dgResolutionTime_PageIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Edit,Update,Delete,Cancel,PageIndex,Itemdatabound

        #region Cancel button

        protected void btncancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                resetPageControls();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "btncancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Cancel button

        #region PreRender for category,Sub Category,Problem Severity

        //commented
        public void PreRendercategory(object sender, System.EventArgs e)
        {
            /*if (!isEdit)
            {
                DropDownList ddlcategory1= (DropDownList)sender;
                ddlcategory1.SelectedIndex = ddlcategory1.Items.IndexOf(ddlcategory1.Items.FindByText (strCategory));
                isEdit = false;
            }*/
        }

        public void PreRenderSubCategory(object sender, System.EventArgs e)
        {
            /*	if (!isEdit)
                {
                    DropDownList ddlsubCategorymaster =(DropDownList)sender;
                    ddlsubCategorymaster.SelectedIndex=ddlsubCategorymaster.Items.IndexOf(ddlsubCategorymaster.Items.FindByText (strSubCategory));
                    isEdit = false;
                }*/
        }

        public void PreRenderSeverity(object sender, System.EventArgs e)
        {
            /*if (!isEdit)
            {
                DropDownList ddlseveritymaster =(DropDownList)sender;
                ddlseveritymaster.SelectedIndex=ddlseveritymaster.Items.IndexOf(ddlseveritymaster.Items.FindByText(strSeverity));
                isEdit = false;
            }*/
        }

        #endregion PreRender for category,Sub Category,Problem Severity

        #region dropdownlist selectedIndexChanged

        protected void ddlcategory_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                ViewState["select"] = ddlcategory.SelectedItem.Text;

                bindSubCategory();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "ddlcategory_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void ddlcategory1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                foreach (DataGridItem dgi in dgResolutionTime.Items)
                {
                    if (dgi.ItemType == ListItemType.EditItem)
                    {
                        DropDownList ddlcategory1 = ((DropDownList)dgi.FindControl("ddlcategory1"));
                        objclsResolutionTime.CategoryID = Convert.ToInt32(ddlcategory1.SelectedValue.ToString());
                        dsSubCategory = objclsBLResolutionTime.getSubCategory(objclsResolutionTime);

                        DropDownList ddlsubcategorymaster = ((DropDownList)dgi.FindControl("ddlsubcategorymaster"));
                        ddlsubcategorymaster.Items.Clear();
                        for (int i = 0; i < dsSubCategory.Tables[0].Rows.Count; i++)
                        {
                            ddlsubcategorymaster.Items.Add(new ListItem(dsSubCategory.Tables[0].Rows[i]["SubCategory"].ToString(), dsSubCategory.Tables[0].Rows[i]["SubcategoryID"].ToString()));
                        }
                        isEdit = true;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "ddlcategory1_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion dropdownlist selectedIndexChanged

        #region Delete Conformation

        private void dgResolutionTime_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lbtnDelete = (LinkButton)(e.Item.Cells[0].FindControl("lbtnDelete"));
                    lbtnDelete.Attributes.Add("onClick", "return confirm('Do you want to delete this row?')");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ResolutionTimeMaster.aspx", "dgResolutionTime_ItemCreated", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Delete Conformation

        protected void ddlsubcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}