using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;
using V2.Helpdesk.Model;

//using ComponentArt.Charting;
//using ComponentArt.Charting.Design ;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for IssueStatusReport.
    /// </summary>
    public partial class IssueStatusReport : System.Web.UI.Page
    {
        #region Variable Declaration

        private BusinessLayer.clsBLIssueStaus objclsBLIssueStaus;
        private DataSet dsIssueStatus = new DataSet();

        //protected ComponentArt.Charting.WebChart Chart1;
        private Model.clsIssueStatus objclsIssueStatus;

        public int EmployeeID, SAEmployeeID, OnlySuperAdmin;
        private DataSet dsYear;
        //Model.clsLine2D objclsLine2D;
        #endregion Variable Declaration

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                //code to clear all the cache so that after logout, previous page cannot be revisited.
                //--- starts here---------

                //--- ends here---------
                objclsIssueStatus = new clsIssueStatus();
                objclsBLIssueStaus = new clsBLIssueStaus();
                //			objclsIssueStatus.connectionString = ConfigurationSettings.AppSettings["sql_Helpdesk_connection"].ToString();
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
                    objclsIssueStatus.EmployeeId = SAEmployeeID;
                    BindData();
                    BindYear();
                }
                btnshow.Attributes.Add("onclick", "return validateCategory()");
                btnshow.Attributes.Add("onclick", "return DateRequired()");
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "IssueStatusReport.aspx", "Page_Load", ex.StackTrace);
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

        public void BindData()
        {
            try
            {
                ddlDepartment.Items.Add(new ListItem("Select Department", "0"));
                dsIssueStatus = objclsBLIssueStaus.BindData(objclsIssueStatus);
                for (int i = 0; i < dsIssueStatus.Tables[0].Rows.Count; i++)
                {
                    if (dsIssueStatus.Tables[0].Rows.Count > 0)
                    {
                        ddlDepartment.Items.Add(new ListItem(dsIssueStatus.Tables[0].Rows[i]["Category"].ToString(), dsIssueStatus.Tables[0].Rows[i]["CategoryID"].ToString())); ;
                    }
                }
                //			dsIssueStatus=objclsBLIssueStaus.BindData(objclsIssueStatus);
                //			ddlDepartment.DataSource = dsIssueStatus.Tables[0];
                //			ddlDepartment.DataValueField = dsIssueStatus.Tables[0].Columns["CategoryID"].ToString();
                //			ddlDepartment.DataTextField = dsIssueStatus.Tables[0].Columns["Category"].ToString();
                //			ddlDepartment.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "IssueStatusReport.aspx", "BindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// bind years for dropdown list
        /// </summary>
        private void BindYear()
        {
            dsYear = objclsBLIssueStaus.getYears();

            ddlFromyear.Items.Clear();
            for (int i = 0; i < dsYear.Tables[0].Rows.Count; i++)
            {
                ddlFromyear.Items.Add(new ListItem(dsYear.Tables[0].Rows[i]["Year"].ToString()));
            }

            ddlToyear.Items.Clear();
            for (int i = 0; i < dsYear.Tables[0].Rows.Count; i++)
            {
                ddlToyear.Items.Add(new ListItem(dsYear.Tables[0].Rows[i]["Year"].ToString()));
            }
        }

        # region Get Issue Details

        public void getIssueStatusdetails(clsIssueStatus objclsIssueStatus)
        {
            try
            {
                //dsIssueStatus = objclsBLIssueStaus.getDsIssueStatus (startDate,endDate);

                dsIssueStatus = objclsBLIssueStaus.getDsIssueStatus(objclsIssueStatus);

                if (dsIssueStatus.Tables[0].Rows.Count > 0)
                {
                    lblError.Visible = false;
                    dgIssuestatus.Visible = true;
                    dgIssuestatus.DataSource = dsIssueStatus.Tables[0];
                    dgIssuestatus.DataBind();
                    ViewState["IssueStatus"] = dsIssueStatus;
                }
                else
                {
                    lblError.Visible = true;
                    dgIssuestatus.Visible = false;
                    lblError.Text = "NO Records Found";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "IssueStatusReport.aspx", "getIssueStatusdetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion

        #region ShowData

        protected void btnshow_Click(object sender, System.EventArgs e)
        {
            try
            {
                string startMonth = ddlFrommonth.SelectedItem.Value.ToString() + "-" + ddlFromyear.SelectedItem.Text;
                string endMonth = ddlTomonth.SelectedItem.Value.ToString() + "-" + ddlToyear.SelectedItem.Text;

                objclsIssueStatus.Category = Convert.ToInt32(ddlDepartment.SelectedValue);
                objclsIssueStatus.StartMonth = Convert.ToDateTime(startMonth);
                //objclsIssueStatus.startMonth = Convert.ToDateTime (ddlFromyear.SelectedIndex.ToString ());
                objclsIssueStatus.EndMonth = getEndDate(Convert.ToInt32(ddlTomonth.SelectedItem.Value), Convert.ToInt32(ddlToyear.SelectedItem.Text));
                //objclsIssueStatus.endMonth = Convert.ToDateTime (ddlToyear.SelectedItem.Text );
                getIssueStatusdetails(objclsIssueStatus);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "IssueStatusReport.aspx", "btnshow_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion

        #region checkmonthenddate

        private DateTime getEndDate(int monthValue, int YearValue)
        {
            try
            {
                if (monthValue == 1 || monthValue == 3 || monthValue == 5 || monthValue == 7 || monthValue == 8 || monthValue == 10 || monthValue == 12)
                {
                    return Convert.ToDateTime(monthValue + "/31/" + YearValue);
                }
                else if (monthValue == 4 || monthValue == 6 || monthValue == 9 || monthValue == 11)
                {
                    return Convert.ToDateTime(monthValue + "/30/" + YearValue);
                }
                else
                {
                    if (DateTime.IsLeapYear(YearValue))
                    {
                        return Convert.ToDateTime(monthValue + "/29/" + YearValue);
                    }
                    else
                    {
                        return Convert.ToDateTime(monthValue + "/28/" + YearValue);
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "IssueStatusReport.aspx", "getEndDate", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion

        #region redirecttoanotherpage

        private void btngraph_Click(object sender, System.EventArgs e)
        {
            try
            {
                Response.Redirect("GraphicalIssueStatusReport.aspx");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "IssueStatusReport.aspx", "btngraph_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion
    }
}