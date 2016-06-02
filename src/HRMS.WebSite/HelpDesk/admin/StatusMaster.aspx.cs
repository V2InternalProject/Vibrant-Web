using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for StatusMaster.
    /// </summary>
    public partial class StatusMaster : System.Web.UI.Page
    {
        #region variable declarations

        private Model.clsStatus objStatus;
        private BusinessLayer.clsBLStatus objBLStatus;
        private DataSet dsStatusList, dsIsDuplicateStatus;
        private Boolean recordcount;
        public int EmployeeID;

        #endregion variable declarations

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Put user code to initialize the page here
                //code to clear all the cache so that after logout, previous page cannot be revisited.
                //--- starts here---------

                //--- ends here---------
                int EmployeeID, SAEmployeeID;
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
                if (!IsPostBack)
                {
                    GetStatusList();
                }
                btnSubmit.Attributes.Add("onClick", "return isRequired('txtStatus','Status')");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.aspx", "Page_Load", ex.StackTrace);
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
            //this.dgCategories.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgCategories_ItemCreated);
            this.dgStatus.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgStatus_ItemCreated);
        }

        #endregion Web Form Designer generated code

        public void GetStatusList()
        {
            try
            {
                clsBLStatus objBLStatus = new clsBLStatus();
                dsStatusList = objBLStatus.GetStatusList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.aspx", "GetStatusList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }

            ShowStatusList();
        }

        public void ShowStatusList()
        {
            try
            {
                dgStatus.DataSource = dsStatusList.Tables[0];
                if (dsStatusList.Tables[0].Rows.Count > 0)
                {
                    lblMsg.Visible = false;
                    dgStatus.DataBind();
                    if (dgStatus.PageCount > 1)
                    {
                        dgStatus.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgStatus.PagerStyle.Visible = false;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.aspx", "ShowStatusList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                objStatus = new Model.clsStatus();
                objBLStatus = new BusinessLayer.clsBLStatus();

                objStatus.StatusName = Convert.ToString(txtStatus.Text.Trim());

                //				if(txtStatus.Text.Trim()=="")
                //				{
                //					lblDuplicateStatus.Visible=true;
                //					lblDuplicateStatus.Text="Please enter a status";
                //				}
                //				else
                //				{
                dsIsDuplicateStatus = objBLStatus.IsDuplicateStatus(objStatus);
                if (dsIsDuplicateStatus.Tables[0].Rows.Count > 0)
                {
                    lblDuplicateStatus.Text = "This status already exists";
                }
                else
                {
                    lblDuplicateStatus.Text = "";
                    recordcount = objBLStatus.AddNewStatus(objStatus);
                    if (recordcount)
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Record inserted successfully";
                        pnlAddEditStatus.Visible = false;
                        GetStatusList();
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Error while inserting record";
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void lnkAddNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                pnlAddEditStatus.Visible = true;
                ResetPageControls();
                lblMsg.Visible = false;
                GetStatusList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.aspx", "lnkAddNew_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgStatus_Edit(object Sender, DataGridCommandEventArgs e)
        {
            try
            {
                pnlAddEditStatus.Visible = false;
                dgStatus.EditItemIndex = (int)e.Item.ItemIndex;
                GetStatusList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.aspx", "dgStatus_Edit", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgStatus_Update(object Sender, DataGridCommandEventArgs e)
        {
            try
            {
                objStatus = new Model.clsStatus();
                objBLStatus = new BusinessLayer.clsBLStatus();
                DataSet dsIsDuplicateStatus = new DataSet();
                int intStatusID = Convert.ToInt32((dgStatus.DataKeys[e.Item.ItemIndex]).ToString());
                TextBox strNewStatus = (TextBox)e.Item.FindControl("txtNewStatus");
                DropDownList strIsActive = (DropDownList)e.Item.FindControl("ddlisActive1");
                objStatus.StatusID = intStatusID;
                objStatus.StatusName = Convert.ToString(strNewStatus.Text.Trim());

                if (objStatus.StatusName == "")
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Please enter a status";
                }
                else
                {
                    dsIsDuplicateStatus = objBLStatus.IsDuplicateStatus(objStatus);
                    if (dsIsDuplicateStatus.Tables[0].Rows.Count > 0)
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "This status already exists.";
                    }
                    else
                    {
                        lblDuplicateStatus.Text = "";
                        recordcount = objBLStatus.UpdateStatus(objStatus);

                        dgStatus.EditItemIndex = -1;
                        GetStatusList();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.aspx", "dgStatus_Update", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgStatus_Cancel(object Sender, DataGridCommandEventArgs e)
        {
            try
            {
                dgStatus.EditItemIndex = -1;
                lblDuplicateStatus.Text = "";
                GetStatusList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.aspx", "dgStatus_Cancel", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgStatus_Delete(object Sender, DataGridCommandEventArgs e)
        {
            try
            {
                objStatus = new Model.clsStatus();
                objBLStatus = new BusinessLayer.clsBLStatus();

                int intStatusID = Convert.ToInt32((dgStatus.DataKeys[e.Item.ItemIndex]).ToString());
                objStatus.StatusID = intStatusID;

                recordcount = objBLStatus.DeleteStatus(objStatus);
                dgStatus.EditItemIndex = -1;

                GetStatusList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.aspx", "dgStatus_Delete", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void dgStatus_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                //			pnlAddEditStatus.Visible=false;
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lnkDel = (LinkButton)(e.Item.Cells[0].FindControl("lnkDelete"));
                    lnkDel.Attributes.Add("onClick", "return confirm('Do you want to delete this record?')");
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.aspx", "dgStatus_ItemCreated", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                ResetPageControls();
                lblDuplicateStatus.Text = "";
                GetStatusList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.aspx", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void ResetPageControls()
        {
            try
            {
                txtStatus.Text = "";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.aspx", "ResetPageControls", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}