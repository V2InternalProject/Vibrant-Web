using HRMS;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;
using V2.Helpdesk.Model;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for IssueHealth.
    /// </summary>
    public partial class lblIssueHealth : System.Web.UI.Page
    {
        private clsIssueHealth objIssueHealth = new clsIssueHealth();
        private clsBLIssueHealth objBIssueHealth = new clsBLIssueHealth();
        private int EmployeeID, SAEmployeeID, OnlySuperAdmin;
        private DataSet dsIssueHealth = new DataSet();
        private DataView dv;
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        private int DvRowCount = 0;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                string PageName = "Issue Health";
                objpagelevel.PageLevelAccess(PageName);

                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                OnlySuperAdmin = Convert.ToInt32(Session["OnlySuperAdmin"]);
                if (OnlySuperAdmin != 0)
                {
                    Response.Redirect("AuthorizationErrorMessage.aspx?PageSource=IssueHealth");
                }

                if ((SAEmployeeID.ToString() == "" || SAEmployeeID == 0))
                {
                    Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
                }

                if (SAEmployeeID != 0 && EmployeeID == 0)
                {
                    EmployeeID = SAEmployeeID;
                }

                if (!IsPostBack)
                {
                    bindData();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "IssueHealth.aspx", "Page_Load", ex.StackTrace);
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
            this.ID = "lblIssueHealth";
        }

        #endregion Web Form Designer generated code

        public void bindData()
        {
            try
            {
                objIssueHealth.EmployeeID = EmployeeID;
                dsIssueHealth = objBIssueHealth.bindData(objIssueHealth);
                dgViewIssuesHealth.DataSource = dsIssueHealth.Tables[0];
                if (dsIssueHealth.Tables[0].Rows.Count > 0)
                {
                    dv = new DataView(dsIssueHealth.Tables[1]);
                    dgViewIssuesHealth.DataBind();
                }
                else
                {
                    lblmsg.Text = "No Issue Reported ";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "IssueHealth.aspx", "bindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void dgViewIssues_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblIssueHealth = (Label)(e.Item.FindControl("lblIssueHealth"));
                    Label lblCategoryID = (Label)(e.Item.FindControl("lblCategoryID"));
                    Label lblSeverityID = (Label)(e.Item.FindControl("lblSeverityID"));

                    dv.RowFilter = "Categoryid = " + lblCategoryID.Text + " and  SeverityId=" + lblSeverityID.Text + "";
                    if (dv.Count >= 1)
                    {
                        if (Convert.ToDouble(lblIssueHealth.Text) <= Convert.ToDouble(dv.Table.Rows[DvRowCount]["Green"].ToString()))
                        {
                            lblIssueHealth.BackColor = Color.Green;
                        }
                        else if (Convert.ToDouble(lblIssueHealth.Text) > Convert.ToDouble(dv.Table.Rows[DvRowCount]["Green"].ToString()) && Convert.ToDouble(lblIssueHealth.Text) <= Convert.ToDouble(dv.Table.Rows[DvRowCount]["Amber"].ToString()))
                        {
                            lblIssueHealth.BackColor = Color.FromName("#FF7E00");
                        }
                        else if (Convert.ToDouble(lblIssueHealth.Text) > Convert.ToDouble(dv.Table.Rows[DvRowCount]["Amber"].ToString()) && Convert.ToDouble(lblIssueHealth.Text) <= Convert.ToDouble(dv.Table.Rows[DvRowCount]["Red"].ToString()))
                        {
                            lblIssueHealth.BackColor = Color.Red;
                        }
                        else
                        {
                            lblIssueHealth.BackColor = Color.White;
                        }
                        lblIssueHealth.Text = Convert.ToString(100 - Convert.ToInt32(lblIssueHealth.Text));
                        DvRowCount++;
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "IssueHealth.aspx", "dgViewIssues_ItemDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}