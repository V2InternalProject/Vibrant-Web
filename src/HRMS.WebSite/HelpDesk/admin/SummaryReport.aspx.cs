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
    /// Summary description for SummaryReport.
    /// </summary>
    public partial class SummaryReport : System.Web.UI.Page
    {
        #region Declare Variables

        private clsSummaryReport objSummaryReport = new clsSummaryReport();
        private clsBLSummaryReport objBLSummaryReport = new clsBLSummaryReport();
        private int EmployeeID, SAEmployeeID, OnlySuperAdmin;

        #endregion Declare Variables

        #region Page Load

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                SAEmployeeID = Convert.ToInt32(Session["SAEmployeeID"]);
                OnlySuperAdmin = Convert.ToInt32(Session["OnlySuperAdmin"]);
                if (OnlySuperAdmin != 0)
                {
                    Response.Redirect("AuthorizationErrorMessage.aspx");
                }
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
                    DepartmentDetails();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SummaryReport.aspx", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Page Load

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

        #region DepartmentDetails

        public void DepartmentDetails()
        {
            try
            {
                ddlDepartment.Items.Clear();
                DataSet dsDepartment = new DataSet();
                objSummaryReport.EmployeeID = Convert.ToInt32(SAEmployeeID.ToString());
                //clsBLSubCategory objClsBLSubCategory = new clsBLSubCategory();
                //clsCategoryWiseSearchReport objCategorywiseSearchReport = new clsCategoryWiseSearchReport();
                //objCategorywiseSearchReport.EmployeeName = SAEmployeeID.ToString();
                //DataSet dsCategoryID = objClsBLSubCategory.getAllCategoryID(objCategorywiseSearchReport);

                dsDepartment = objBLSummaryReport.DepartmentDeatils(objSummaryReport);
                if (dsDepartment.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsDepartment.Tables[0].Rows.Count; i++)
                    {
                        ddlDepartment.Items.Add(new ListItem(dsDepartment.Tables[0].Rows[i]["Category"].ToString(), dsDepartment.Tables[0].Rows[i]["CategoryID"].ToString()));
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SummaryReport.aspx", "DepartmentDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion DepartmentDetails

        #region DeptSummaryDetails

        public void DeptSummaryDetails()
        {
            try
            {
                DataSet dsSummaryReport = new DataSet();
                objSummaryReport.CategoryID = Convert.ToInt32(ddlDepartment.SelectedValue);
                objSummaryReport.ReportIssueDate = Convert.ToDateTime(txtFromDate.Text.ToString());
                objSummaryReport.ReportCloseDate = Convert.ToDateTime(txtToDate.Text.ToString());
                dsSummaryReport = objBLSummaryReport.DeptSummaryDetails(objSummaryReport);
                if (dsSummaryReport.Tables[0].Rows.Count < 1)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "No Records Found";
                    dgSummaryReport.DataSource = dsSummaryReport;
                    dgSummaryReport.DataBind();
                }
                else
                {
                    lblMsg.Visible = false;
                    dgSummaryReport.DataSource = dsSummaryReport;
                    dgSummaryReport.DataBind();
                }
                if (dgSummaryReport.PageCount <= 1)
                {
                    dgSummaryReport.PagerStyle.Visible = false;
                }
                else
                {
                    dgSummaryReport.PagerStyle.Visible = true;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SummaryReport.aspx", "DeptSummaryDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion DeptSummaryDetails

        #region Display the Grid Details

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                DeptSummaryDetails();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SummaryReport.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Display the Grid Details
    }
}