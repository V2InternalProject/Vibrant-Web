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
    /// Summary description for ProblemPriorityMaster.
    /// </summary>
    public partial class ProblemPriorityMaster : System.Web.UI.Page
    {
        #region variable declarations

        private Model.clsProblemPriority objProblemPriority;
        private BusinessLayer.clsBLProblemPriority objBLProblemPriority;
        private DataSet dsProblemPriorityList, dsDuplicateProblemPriority, dsCheckProblemPriority;
        private Boolean recordfound;
        private int recordcount;
        private string strIsActive1;
        public int strProblemPriorityID, EmployeeID, SAEmployeeID;
        //protected System.Web.UI.WebControls.TextBox txtNewProblemPriority;

        #endregion variable declarations

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                //code to clear all the cache so that after logout, previous page cannot be revisited.
                //--- starts here---------

                //--- ends here---------
                // Put user code to initialize the page here
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
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
                if (!IsPostBack)
                {
                    GetProblemPriorityList();
                }
                btnSubmit.Attributes.Add("onClick", "return ParametersRequired();");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "Page_Load", ex.StackTrace);
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
            this.dgProblemPriority.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgProblemPriority_ItemCreated);
        }

        #endregion Web Form Designer generated code

        #region User defined functions

        public void GetProblemPriorityList()
        {
            try
            {
                clsBLProblemPriority objBLProblemPriority = new clsBLProblemPriority();
                dsProblemPriorityList = objBLProblemPriority.GetProblemPriorityList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                ShowProblemPriorityList();
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "GetProblemPriorityList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void ShowProblemPriorityList()
        {
            dgProblemPriority.DataSource = dsProblemPriorityList.Tables[0];
            if (dsProblemPriorityList.Tables[0].Rows.Count > 0)
            {
                dgProblemPriority.DataBind();
                if (dgProblemPriority.PageCount > 1)
                {
                    dgProblemPriority.PagerStyle.Visible = true;
                }
                else
                {
                    dgProblemPriority.PagerStyle.Visible = false;
                }
            }
            else
            {
                lblMsg.Visible = true;
            }
        }

        #endregion User defined functions

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            objProblemPriority = new Model.clsProblemPriority();
            objBLProblemPriority = new BusinessLayer.clsBLProblemPriority();

            objProblemPriority.ProblemPriorityName = Server.HtmlEncode(Convert.ToString(txtProblemPriority.Text.Trim()));
            objProblemPriority.isActive = Convert.ToInt32(ddlisActive.SelectedItem.Value);
            objProblemPriority.GreenResolutionHours = Convert.ToInt32(txtGreenResolutionHours.Text.Trim());
            objProblemPriority.AmberResolutionHours = Convert.ToInt32(txtAmberResolutionHours.Text.Trim());
            //			DataSet dsDuplicateProblemPriority = new DataSet();
            try
            {
                //				if(txtProblemPriority.Text.Trim()=="")
                //				{
                //					lblDuplicateProblemPriority.Visible=true;
                //					lblDuplicateProblemPriority.Text="Please enter a problem priority";
                //					GetProblemPriorityList();
                //				}
                //				else
                //				{
                //					int dsDuplicateProblemPriority;
                dsDuplicateProblemPriority = objBLProblemPriority.IsDuplicateProblemPriority(objProblemPriority);
                if (dsDuplicateProblemPriority.Tables[0].Rows.Count > 0)
                {
                    //						lblDuplicateProblemPriority.Visible=true;
                    lblDuplicateProblemPriority.Text = "Duplicate problem priority";
                    GetProblemPriorityList();
                }
                else
                {
                    lblDuplicateProblemPriority.Text = "";
                    pnlAddEditProblemPriority.Visible = false;
                    recordfound = objBLProblemPriority.AddNewProblemPriority(objProblemPriority);
                    if (recordfound)
                    {
                        lblMsg.Visible = true;
                        lblMsg.CssClass = "success";
                        lblMsg.Text = "Record inserted successfully";
                        GetProblemPriorityList();
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Error while inserting record";
                        GetProblemPriorityList();
                    }
                }
                //				}
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void lnkAddNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                pnlAddEditProblemPriority.Visible = true;
                lblDuplicateProblemPriority.Text = "";
                lblMsg.Text = "";
                ResetPageControls();
                GetProblemPriorityList();
                this.setfocus(txtProblemPriority);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "lnkAddNew_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgProblemPriority_Edit(object Sender, DataGridCommandEventArgs e)
        {
            try
            {
                pnlAddEditProblemPriority.Visible = false;
                objBLProblemPriority = new BusinessLayer.clsBLProblemPriority();
                dsProblemPriorityList = objBLProblemPriority.GetProblemPriorityList();
                strIsActive1 = dsProblemPriorityList.Tables[0].Rows[e.Item.ItemIndex][4].ToString();
                dgProblemPriority.EditItemIndex = (int)e.Item.ItemIndex;
                GetProblemPriorityList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "dgProblemPriority_Edit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgProblemPriority_Update(object Sender, DataGridCommandEventArgs e)
        {
            try
            {
                objProblemPriority = new Model.clsProblemPriority();
                objBLProblemPriority = new BusinessLayer.clsBLProblemPriority();
                int intProblemPriorityID = Convert.ToInt32((dgProblemPriority.DataKeys[e.Item.ItemIndex]).ToString());
                TextBox strNewProblemPriority = (TextBox)e.Item.FindControl("txtNewProblemPriority");
                TextBox strNewGreenResolutionHours = (TextBox)e.Item.FindControl("txtNewGreenResolutionHours");
                TextBox strNewAmberResolutionHours = (TextBox)e.Item.FindControl("txtNewAmberResolutionHours");
                int strIsActive = ((DropDownList)e.Item.FindControl("ddlisActive1")).SelectedIndex;
                objProblemPriority.ProblemPriorityID = intProblemPriorityID;
                objProblemPriority.isActive = strIsActive;

                if (strNewProblemPriority.Text.Trim() == "")
                {
                    lblMsg.Text = "Please enter a problem priority";
                    GetProblemPriorityList();
                }
                else if (strNewProblemPriority.Text.Trim() != "")
                {
                    objProblemPriority.ProblemPriorityName = Server.HtmlEncode(Convert.ToString(strNewProblemPriority.Text.Trim().Replace("&amp;", "&")));
                    dsDuplicateProblemPriority = objBLProblemPriority.IsDuplicateProblemPriority(objProblemPriority);
                    if (dsDuplicateProblemPriority.Tables[0].Rows.Count > 0)
                    {
                        lblMsg.Text = "Duplicate problem priority";
                        GetProblemPriorityList();
                    }
                    else
                    {
                        objProblemPriority.ProblemPriorityName = Server.HtmlEncode(Convert.ToString(strNewProblemPriority.Text.Trim()));
                        if (strNewGreenResolutionHours.Text.Trim() == "")
                        {
                            lblMsg.Text = "Please enter an integer value for Green Resolution Hours";
                            GetProblemPriorityList();
                        }
                        else if (strNewGreenResolutionHours.Text.Trim() != "")
                        {
                            string strNewGreenResolution = strNewGreenResolutionHours.Text.Trim();
                            if (strNewGreenResolution.StartsWith("0") || strNewGreenResolution.StartsWith("1") ||
                                strNewGreenResolution.StartsWith("2") || strNewGreenResolution.StartsWith("3") ||
                                strNewGreenResolution.StartsWith("4") || strNewGreenResolution.StartsWith("5") ||
                                strNewGreenResolution.StartsWith("6") || strNewGreenResolution.StartsWith("7") ||
                                strNewGreenResolution.StartsWith("8") || strNewGreenResolution.StartsWith("9"))
                            {
                                objProblemPriority.GreenResolutionHours = Convert.ToInt32(strNewGreenResolution);

                                if (strNewAmberResolutionHours.Text.Trim() == "")
                                {
                                    lblMsg.Text = "Please enter an integer value for Amber Resolution Hours";
                                    GetProblemPriorityList();
                                }
                                else if (strNewAmberResolutionHours.Text.Trim() != "")
                                {
                                    string strNewAmberResolution = strNewAmberResolutionHours.Text.Trim();
                                    if (strNewAmberResolution.StartsWith("0") || strNewAmberResolution.StartsWith("1") ||
                                        strNewAmberResolution.StartsWith("2") || strNewAmberResolution.StartsWith("3") ||
                                        strNewAmberResolution.StartsWith("4") || strNewAmberResolution.StartsWith("5") ||
                                        strNewAmberResolution.StartsWith("6") || strNewAmberResolution.StartsWith("7") ||
                                        strNewAmberResolution.StartsWith("8") || strNewAmberResolution.StartsWith("9"))
                                    {
                                        objProblemPriority.AmberResolutionHours = Convert.ToInt32(strNewAmberResolution);
                                        recordcount = objBLProblemPriority.UpdateProblemPriority(objProblemPriority);
                                        if (recordcount > 0)
                                        {
                                            if (strIsActive == 0)
                                            {
                                                Page.RegisterStartupScript("key", "<script>alert('Cannot edit this Problem Priority as issues found.');</script>");
                                            }
                                            else
                                            {
                                                dgProblemPriority.EditItemIndex = -1;
                                                lblMsg.Text = "";
                                                GetProblemPriorityList();
                                            }
                                        }
                                        else
                                        {
                                            dgProblemPriority.EditItemIndex = -1;
                                            lblMsg.Text = "";
                                            GetProblemPriorityList();
                                        }
                                    }
                                    else
                                    {
                                        lblMsg.Text = "Please enter an integer value for Amber Resolution Hours";
                                        GetProblemPriorityList();
                                    }
                                }

                                /*dgProblemPriority.EditItemIndex = -1;
                                lblMsg.Text="";
                                GetProblemPriorityList();*/
                            }
                            else
                            {
                                lblMsg.Text = "Please enter an integer value for Green Resolution Hours";
                                GetProblemPriorityList();
                            }
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "dgProblemPriority_Update", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgProblemPriority_Cancel(object Sender, DataGridCommandEventArgs e)
        {
            try
            {
                dgProblemPriority.EditItemIndex = -1;
                lblDuplicateProblemPriority.Text = "";
                GetProblemPriorityList();
                lblMsg.Text = "";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "dgProblemPriority_Cancel", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgProblemPriority_Delete(object Sender, DataGridCommandEventArgs e)
        {
            try
            {
                objProblemPriority = new Model.clsProblemPriority();
                objBLProblemPriority = new BusinessLayer.clsBLProblemPriority();

                int intProblemPriorityID = Convert.ToInt32((dgProblemPriority.DataKeys[e.Item.ItemIndex]).ToString());
                objProblemPriority.ProblemPriorityID = intProblemPriorityID;

                recordcount = objBLProblemPriority.DeleteProblemPriority(objProblemPriority);
                if (recordcount <= 0)
                {
                    Page.RegisterStartupScript("key", "<script>alert('Cannot delete this Problem Priority as issues found.');</script>");
                }
                else
                {
                    dgProblemPriority.EditItemIndex = -1;
                }
                GetProblemPriorityList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "dgProblemPriority_Delete", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgProblemPriority_Status(object Sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (dsProblemPriorityList.Tables[0].Rows[e.Item.ItemIndex][4].ToString() == "False")
                    {
                        e.Item.Cells[4].Text = "Inactive";
                    }
                    else
                    {
                        e.Item.Cells[4].Text = "Active";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "dgProblemPriority_Status", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void dgProblemPriority_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lnkDel = (LinkButton)(e.Item.Cells[0].FindControl("lnkDelete"));
                    lnkDel.Attributes.Add("onClick", "return confirm('Do you want to delete this Problem Priority?')");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "dgProblemPriority_ItemCreated", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                ResetPageControls();
                lblDuplicateProblemPriority.Text = "";
                GetProblemPriorityList();
                this.setfocus(txtProblemPriority);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void ResetPageControls()
        {
            try
            {
                txtProblemPriority.Text = "";
                txtGreenResolutionHours.Text = "";
                txtAmberResolutionHours.Text = "";
                ddlisActive.SelectedIndex = 1;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "ResetPageControls", ex.StackTrace);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "SetDropDownIndex", ex.StackTrace);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ProblemPriorityMaster.aspx", "setfocus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}