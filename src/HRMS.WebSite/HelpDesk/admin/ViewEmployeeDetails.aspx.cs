using HRMS;
using System;
using System.Configuration;
using System.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;
using V2.Helpdesk.Model;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for ViewEmployeeDetails.
    /// </summary>
    public partial class ViewEmployeeDetails : System.Web.UI.Page
    {
        private DataSet dsGetData = new DataSet();
        private string EmpId;
        private int EmployeeID, SAEmployeeID, SuperAdmin;
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                string PageName = "HelpDesk Admin Masters";
                objpagelevel.PageLevelAccess(PageName);

                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                SuperAdmin = Convert.ToInt32(Session["SuperAdmin"]);

                if (SuperAdmin == 0)
                {
                    if ((SAEmployeeID.ToString() != "" || SAEmployeeID != 0))
                        Response.Redirect("AuthorizationErrorMessage.aspx?PageSource=EmpDetails");
                    else
                        Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
                }
                if (!IsPostBack)
                {
                    BindData();
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                Response.Redirect("AuthorizationErrorMessage.aspx?PageSource=EmpDetails");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewEmployeeDetails.aspx", "Page_Load", ex.StackTrace);
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

        #	region for Binding DataGrid

        public void BindData()
        {
            try
            {
                clsBLSubCategoryAssignment objClsBLSubCategoryAssignment = new clsBLSubCategoryAssignment();
                clsSubCategoryAssignment objClsSubCategoryAssignment = new clsSubCategoryAssignment();
                objClsSubCategoryAssignment.EmployeeID = SuperAdmin;
                dsGetData = objClsBLSubCategoryAssignment.getEmployeedetails(objClsSubCategoryAssignment);
                if (dsGetData.Tables[0].Rows.Count > 0)
                {
                    dgEmployeedetails.DataSource = dsGetData.Tables[0];
                    dgEmployeedetails.DataBind();
                    if (dgEmployeedetails.PageCount > 1)
                    {
                        dgEmployeedetails.PagerStyle.Visible = true;
                    }
                    else
                    {
                        dgEmployeedetails.PagerStyle.Visible = false;
                    }
                    dgEmployeedetails.Visible = true;
                    Session[EmpId] = dsGetData.Tables[0].Rows[0];
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewEmployeeDetails.aspx", "BindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        # endregion

        public void dgEmployeedetails_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Detail")
                {
                    EmpId = dgEmployeedetails.DataKeys[e.Item.ItemIndex].ToString();
                    Response.Redirect("ViewCategoryEmpDetail.aspx?source=Detail&EmpId=" + EmpId);
                }
                else if (e.CommandName == "Edit")
                {
                    EmpId = dgEmployeedetails.DataKeys[e.Item.ItemIndex].ToString();
                    Response.Redirect("EditEmployeeDetail.aspx?source=Edit&EmpId=" + EmpId);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewEmployeeDetails.aspx", "dgEmployeedetails_ItemCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void lnkAddNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                Response.Redirect("AddSubCategoryAssignment.aspx", false);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewEmployeeDetails.aspx", "lnkAddNew_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void dgEmployeedetails_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            try
            {
                dgEmployeedetails.CurrentPageIndex = e.NewPageIndex;
                BindData();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ViewEmployeeDetails.aspx", "dgEmployeedetails_PageIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}