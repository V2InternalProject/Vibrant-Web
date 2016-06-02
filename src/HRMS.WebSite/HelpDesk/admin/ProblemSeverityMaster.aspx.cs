using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for ProblemSeverityMaster.
    /// </summary>
    public partial class ProblemSeverityMaster : System.Web.UI.Page
    {
        #region variable declarations

        private Model.clsProblemSeverity objProblemSeverity;
        private BusinessLayer.clsBLProblemSeverity objBLProblemSeverity;
        private DataSet dsProblemSeverityList;
        private Boolean recordfound;
        private int recordcount;
        private string strIsActive1;
        public int EmployeeID, SAEmployeeID, SuperAdmin;
        protected System.Web.UI.WebControls.Label lblMessage;

        #endregion variable declarations

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
                    GetProblemSeverityList();
                }
                btnSubmit.Attributes.Add("onClick", "return isRequire('txtProblemSeverity','Problem Severity',this.disabled);");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "Page_Load", ex.StackTrace);
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
            this.dgProblemSeverity.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgProblemSeverity_ItemCreated);
        }

        #endregion Web Form Designer generated code

        public void GetProblemSeverityList()
        {
            try
            {
                clsBLProblemSeverity objBLProblemSeverity = new clsBLProblemSeverity();
                dsProblemSeverityList = objBLProblemSeverity.GetProblemSeverityList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "GetProblemSeverityList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }

            ShowProblemSeverityList();
        }

        public void ShowProblemSeverityList()
        {
            try
            {
                dgProblemSeverity.DataSource = dsProblemSeverityList.Tables[0];
                if (dsProblemSeverityList.Tables[0].Rows.Count > 0)
                {
                    //lblMsg.Visible=false;
                    dgProblemSeverity.DataBind();
                    if (dgProblemSeverity.PageCount > 1)
                    {
                        dgProblemSeverity.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgProblemSeverity.PagerStyle.Visible = false;
                    }
                }
                else
                {
                    lblMsg.Visible = true;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "ShowProblemSeverityList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                objProblemSeverity = new Model.clsProblemSeverity();
                objBLProblemSeverity = new BusinessLayer.clsBLProblemSeverity();
                DataSet dsDuplicateProblemSeverity = new DataSet();
                objProblemSeverity.ProblemSeverityName = Server.HtmlEncode(Convert.ToString(txtProblemSeverity.Text.Trim()));
                objProblemSeverity.isActive = Convert.ToInt32(ddlisActive.SelectedItem.Value);

                dsDuplicateProblemSeverity = objBLProblemSeverity.IsDuplicateProblemSeverity(objProblemSeverity);
                if (dsDuplicateProblemSeverity.Tables[0].Rows.Count > 0)
                {
                    lblDuplicateProblemSeverity.Text = "Duplicate problem severity";
                    GetProblemSeverityList();
                }
                else
                {
                    lblDuplicateProblemSeverity.Text = "";
                    recordfound = objBLProblemSeverity.AddNewProblemSeverity(objProblemSeverity);
                    if (recordfound)
                    {
                        lblMsg.Visible = true;
                        lblMsg.CssClass = "success";
                        lblMsg.Text = "Record inserted successfully";
                        pnlAddEditProblemSeverity.Visible = false;
                        GetProblemSeverityList();
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Error while inserting record";
                        GetProblemSeverityList();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void lnkAddNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                pnlAddEditProblemSeverity.Visible = true;
                lblDuplicateProblemSeverity.Text = "";
                lblMsg.Text = "";
                ResetPageControls();
                GetProblemSeverityList();
                this.setfocus(txtProblemSeverity);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "lnkAddNew_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgProblemSeverity_Edit(object Sender, DataGridCommandEventArgs e)
        {
            try
            {
                lblSuccessMsgs.Text = "";
                pnlAddEditProblemSeverity.Visible = false;
                objBLProblemSeverity = new BusinessLayer.clsBLProblemSeverity();
                dsProblemSeverityList = objBLProblemSeverity.GetProblemSeverityList();

                strIsActive1 = dsProblemSeverityList.Tables[0].Rows[e.Item.ItemIndex][2].ToString();

                dgProblemSeverity.EditItemIndex = (int)e.Item.ItemIndex;
                GetProblemSeverityList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "dgProblemSeverity_Edit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgProblemSeverity_Update(object Sender, DataGridCommandEventArgs e)
        {
            try
            {
                objProblemSeverity = new Model.clsProblemSeverity();
                objBLProblemSeverity = new BusinessLayer.clsBLProblemSeverity();
                DataSet dsDuplicateProblemSeverity = new DataSet();
                int intProblemSeverityID = Convert.ToInt32((dgProblemSeverity.DataKeys[e.Item.ItemIndex]).ToString());
                TextBox strNewProblemSeverity = (TextBox)e.Item.FindControl("txtNewProblemSeverity");
                int strIsActive = ((DropDownList)e.Item.FindControl("ddlisActive1")).SelectedIndex;
                objProblemSeverity.ProblemSeverityID = intProblemSeverityID;
                objProblemSeverity.ProblemSeverityName = Server.HtmlEncode(Convert.ToString(strNewProblemSeverity.Text.Trim()));
                objProblemSeverity.isActive = strIsActive;

                if (objProblemSeverity.ProblemSeverityName == "")
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Please enter a problem severity";
                    GetProblemSeverityList();
                }
                else
                {
                    dsDuplicateProblemSeverity = objBLProblemSeverity.IsDuplicateProblemSeverity(objProblemSeverity);
                    if (dsDuplicateProblemSeverity.Tables[0].Rows.Count > 0)
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Duplicate problem severity";
                        GetProblemSeverityList();
                    }
                    else
                    {
                        recordcount = objBLProblemSeverity.UpdateProblemSeverity(objProblemSeverity);
                        if (recordcount > 0)
                        {
                            if (strIsActive == 0)
                            {
                                Page.RegisterStartupScript("key", "<script>alert('Cannot edit this Problem Severity Status as issues found');</script>");
                            }
                        }
                        else
                        {
                            dgProblemSeverity.EditItemIndex = -1;
                            GetProblemSeverityList();
                        }
                    }
                    dgProblemSeverity.EditItemIndex = -1;
                    GetProblemSeverityList();
                    lblSuccessMsgs.Visible = true;
                    lblSuccessMsgs.Text = "Details updated successfully";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "dgProblemSeverity_Update", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgProblemSeverity_Cancel(object Sender, DataGridCommandEventArgs e)
        {
            try
            {
                dgProblemSeverity.EditItemIndex = -1;
                lblDuplicateProblemSeverity.Text = "";
                lblSuccessMsgs.Text = "";
                GetProblemSeverityList();
                lblMsg.Visible = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "dgProblemSeverity_Cancel", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgProblemSeverity_Delete(object Sender, DataGridCommandEventArgs e)
        {
            try
            {
                lblSuccessMsgs.Text = "";
                pnlAddEditProblemSeverity.Visible = false;
                objProblemSeverity = new Model.clsProblemSeverity();
                objBLProblemSeverity = new BusinessLayer.clsBLProblemSeverity();

                int intProblemSeverityID = Convert.ToInt32((dgProblemSeverity.DataKeys[e.Item.ItemIndex]).ToString());
                objProblemSeverity.ProblemSeverityID = intProblemSeverityID;

                recordcount = objBLProblemSeverity.DeleteProblemSeverity(objProblemSeverity);
                if (recordcount <= 0)
                {
                    Page.RegisterStartupScript("key", "<script>alert('Cannot delete this Problem Severity as issues found');</script>");
                }
                else
                {
                    dgProblemSeverity.EditItemIndex = -1;
                }
                GetProblemSeverityList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "dgProblemSeverity_Delete", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgProblemSeverity_Status(object Sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (dsProblemSeverityList.Tables[0].Rows[e.Item.ItemIndex][2].ToString() == "False")
                    {
                        e.Item.Cells[2].Text = "InActive";
                    }
                    else
                    {
                        e.Item.Cells[2].Text = "Active";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "dgProblemSeverity_Status", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void dgProblemSeverity_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lnkDel = (LinkButton)(e.Item.Cells[0].FindControl("lnkDelete"));
                    lnkDel.Attributes.Add("onClick", "return confirm('Do you want to delete this Problem Severity?')");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "dgProblemSeverity_ItemCreated", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                ResetPageControls();
                lblDuplicateProblemSeverity.Text = "";
                lblSuccessMsgs.Text = "";
                GetProblemSeverityList();
                this.setfocus(txtProblemSeverity);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void ResetPageControls()
        {
            try
            {
                txtProblemSeverity.Text = "";
                ddlisActive.SelectedIndex = 1;
                lblSuccessMsgs.Text = "";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "ResetPageControls", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void SetDropDownIndex(object sender, System.EventArgs e)
        {
            try
            {
                DropDownList ddlActive1 = new DropDownList();
                ddlActive1 = (DropDownList)sender;
                if (strIsActive1 == "False" || strIsActive1 == "0")
                    ddlActive1.SelectedIndex = ddlActive1.Items.IndexOf(ddlActive1.Items.FindByText("Inactive"));
                else if (strIsActive1 == "True" || strIsActive1 == "1")
                    ddlActive1.SelectedIndex = ddlActive1.Items.IndexOf(ddlActive1.Items.FindByText("Active"));
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "SetDropDownIndex", ex.StackTrace);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemSeverityMaster.aspx", "setfocus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}