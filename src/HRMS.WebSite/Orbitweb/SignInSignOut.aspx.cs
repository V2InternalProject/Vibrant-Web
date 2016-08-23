using HRMS.DAL;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Workflow.Runtime;
using System.Xml;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;

namespace HRMS.Orbitweb
{
    public partial class SignInSignOut : System.Web.UI.Page
    {
        //Declaration of local variables
        private DataSet dsLoadSignInSigOutData = new DataSet();

        private SignInSignOutBOL objSignInSignOutBOL = new SignInSignOutBOL();
        private string strSignInSignOutID = "";
        private SignInSignOutModel objSignInSignOutModel = new SignInSignOutModel();
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();
        private Boolean hideManual = false;

        public string UserName
        {
            get
            {
                //return the object from session
                return (string)Session["UserName"];
            }

            set
            {
                Session["UserName"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            strSignInSignOutID = "";

            this.UserName = (string)Session["UserName"];

            string PageName = "SignInSignOutAuto";
            objpagelevel.PageLevelAccess(PageName);

            //LoadSignInSignOutData
            //the above function will load data from SignInSignOut Table.

            /*
             *Added by Rahul regarding the inclusion of the US employees in the system.
             *US Employees will not be authenticated with whitelistedIp address instead they would be given access to the signInsignOut page irrespective of their IP's.
            */
            this.ClientScript.RegisterStartupScript(this.GetType(), "setTime", "setTime();", true);//calls the setTime clientside function to set the currnet browser's time to the hidden field
            HRMS_tbl_PM_Employee empDetails = new HRMS_tbl_PM_Employee();
            EmployeeDAL empDal = new EmployeeDAL();
            empDetails = empDal.GetEmployeeDetailsByEmployeeCode(User.Identity.Name);
            FileLog objFileLog = FileLog.GetLogger();
            objSignInSignOutModel.EmployeeID = Convert.ToInt32(User.Identity.Name);
            int LoggedInUserEmployeeID = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
            objSignInSignOutModel.IsSignInManual = 0;
            objSignInSignOutModel.IsSignOutManual = 0;

            if (empDetails.OfficeLocation != 2 && empDetails.OfficeLocation != 3)
            {
                string whiteListedEmpCode = System.Configuration.ConfigurationManager.AppSettings["WhiteListedEmpCode"];
                string[] empCodes = whiteListedEmpCode.Split(',');
                bool whiteListed = empCodes.Contains(User.Identity.Name);
                if (whiteListed)
                {
                    btnSignIn.Visible = true;
                    btnSignOut.Visible = true;
                    OpenSignInSignOutPage();
                    ClientScript.RegisterStartupScript(GetType(), "ChangeVisibilityofManualTab", "ChangeVisibility(true)", true);
                    //ScriptManager.RegisterStartupScript(this, GetType(), "ChangeVisibilityofManualTab", "ChangeVisibility(true)", true);
                }
                else
                {
                    Response.Redirect("../PersonalDetails/PersonalDetails?employeeId=" + Session["encryptedLoggedinEmployeeID"]);
                }
            }
            else
            {
                try
                {
                    IPAddressDetailsModel ipDetails = new IPAddressDetailsModel();
                    string UnitedStatesCountryCode = System.Configuration.ConfigurationManager.AppSettings["UnitedStatesCountryCode"];
                    string IndiaCountryCode = System.Configuration.ConfigurationManager.AppSettings["IndiaCountryCode"];
                    string LocalNetworkIP = System.Configuration.ConfigurationManager.AppSettings["LocalNetworkIP"];
                    string IPAddressWebServiceURL = System.Configuration.ConfigurationManager.AppSettings["IPAddressWebServiceURL"];
                    string[] LocalNetworkIPList = LocalNetworkIP.Split(',');
                    string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    objFileLog.WriteLine(LogType.Info, "", "SignInSignOut.aspx.cs", "", "User IP: " + ip);

                    var islocal = LocalNetworkIPList.Where(t => ip.StartsWith(t)).Any();
                    if (!islocal)
                    {
                        string resulturl = string.Format("{0}&ip={1}&format=xml", IPAddressWebServiceURL, ip);

                        objFileLog.WriteLine(LogType.Info, "", "signinsignout.aspx.cs", "", "dynamic url: " + resulturl);
                        string responsedata = "";

                        var webrequest = WebRequest.Create(resulturl) as HttpWebRequest;
                        if (webrequest != null)
                        {
                            webrequest.UserAgent = "mozilla/5.0 (macintosh; intel mac os x 10_9_2) applewebkit/537.36 (khtml, like gecko) chrome/36.0.1944.0 safari/537.36";
                            using (var response = webrequest.GetResponse())
                            {
                                objFileLog.WriteLine(LogType.Info, "", "signinsignout.aspx.cs", "", "response: " + response);
                                var stream = response.GetResponseStream();

                                if (stream != null)
                                {
                                    using (var reader = new StreamReader(stream))
                                    {
                                        responsedata = reader.ReadToEnd();
                                        objFileLog.WriteLine(LogType.Info, "", "signinsignout.aspx.cs", "", "responsedata: " + responsedata);
                                        ipDetails = FilterData(responsedata);
                                    }
                                }
                            }
                        }

                        if (ipDetails.countryCode == UnitedStatesCountryCode)
                        {
                            btnSignIn.Visible = true;
                            btnSignOut.Visible = true;
                            OpenSignInSignOutPage();
                            ClientScript.RegisterStartupScript(GetType(), "ChangeVisibilityofManualTab", "ChangeVisibility(true)", true);
                            //ScriptManager.RegisterStartupScript(this, GetType(), "ChangeVisibilityofManualTab", "ChangeVisibility(true)", true);
                        }
                        else
                        {
                            Response.Redirect("../PersonalDetails/PersonalDetails?employeeId=" + Session["encryptedLoggedinEmployeeID"]);
                        }
                    }
                    else
                    {
                        hideManual = true;
                        btnSignIn.Visible = false;
                        btnSignOut.Visible = false;
                        OpenSignInSignOutPage();
                        ClientScript.RegisterStartupScript(GetType(), "ChangeVisibilityofManualTab", "ChangeVisibility(false)", true);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "ChangeVisibilityofManualTab", "ChangeVisibility(false)", true);
                    }
                }
                catch (V2Exceptions ex)
                {
                    // throw;
                    Response.Redirect("../PersonalDetails/PersonalDetails?employeeId=" + Session["encryptedLoggedinEmployeeID"]);
                }
                catch (System.Exception ex)
                {
                    objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.cs", "Page_Load", ex.StackTrace);
                    //throw;
                    Response.Redirect("../PersonalDetails/PersonalDetails?employeeId=" + Session["encryptedLoggedinEmployeeID"]);
                }
            }
        }

        private void OpenSignInSignOutPage()
        {
            if (!IsPostBack)
            {
                lblErrorMess.Text = "";
                lblSuccess.Text = "";
                txtSearchFromDate.Attributes.Add("onkeydown", "return false");
                txtSearchToDate.Attributes.Add("onkeydown", "return false");
                ViewState["ColumnName"] = "date";
                ViewState["Order"] = "DESC";
                objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();
                BindSignInSignOut(objSignInSignOutModel);
                grdSignInSignOut.Visible = true;
            }
            MainTabSelected.Value = "Auto";
        }

        public IPAddressDetailsModel FilterData(string Data)
        {
            IPAddressDetailsModel fh = new IPAddressDetailsModel();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(Data);
            XmlNodeList xmlnode;
            string str = null;
            xmlnode = xmldoc.GetElementsByTagName("Response");
            var originalXml = xmldoc.InnerXml;

            Regex exp1 = new Regex(@"<countryCode>(.*?)<\/countryCode>");
            var result1 = exp1.Matches(originalXml);
            foreach (Match match in result1)
            {
                fh.countryCode = match.Groups[1].Value;
            }

            Regex exp2 = new Regex(@"<countryName>(.*?)<\/countryName>");
            var result2 = exp2.Matches(originalXml);
            foreach (Match match in result2)
            {
                fh.countryName = match.Groups[1].Value;
            }

            Regex exp3 = new Regex(@"<ipAddress>(.*?)<\/ipAddress>");
            var result3 = exp3.Matches(originalXml);
            foreach (Match match in result3)
            {
                fh.ipAddress = match.Groups[1].Value;
            }
            return fh;
        }

        public void BindSignInSignOut(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                dsLoadSignInSigOutData = objSignInSignOutBOL.BindSignInSignOut(objSignInSignOutModel);
                grdSignInSignOut.DataSource = dsLoadSignInSigOutData.Tables[0];
                grdSignInSignOut.DataBind();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.aspx.cs", "BindSignInSignOut", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            lblErrorMess.Text = "";
            lblSuccess.Text = "";
            objSignInSignOutModel.SignInTime = Convert.ToDateTime(Convert.ToString(hd_browserTime.Value));
            try
            {
                grdSignInSignOut.PageIndex = 0;
                //1>Check for Leave Details
                //2>Check For Multiple Sign-Ins
                //3>check For Missing SignOut entries
                //4>If the above 3 are false call the "signIn" Stored Procedure and Sign-In the user
                DataSet dsSignIn = new DataSet();

                dsSignIn.Clear();
                //1>Multiple sign ins
                dsSignIn = CheckForMultipleSignIn(objSignInSignOutModel);

                if (dsSignIn.Tables[0].Rows.Count != 0) //Multiple sign ins "IF"
                {
                    lblErrorMess.Text = "You have already Signed-In for the Day!";
                    dsSignIn.Clear();
                }
                else  //Multiple sign ins "IF"--"Else"
                {
                    dsSignIn.Clear();
                    //2>Missing Sign-Outs
                    dsSignIn = CheckMissingSignOut(objSignInSignOutModel);
                    if (dsSignIn.Tables[0].Rows.Count != 0) // Missing Sign-Outs--"IF"
                    {
                        string Dates = "";
                        for (int i = 0; i < dsSignIn.Tables[0].Rows.Count; i++)
                        {
                            if (i == 0)
                            {
                                Dates = dsSignIn.Tables[0].Rows[i]["Dates"].ToString();
                            }
                            else
                            {
                                Dates = Dates + "," + dsSignIn.Tables[0].Rows[i]["Dates"].ToString();
                            }
                        }
                        //Dates = Dates.Substring(1, Dates.Length);

                        lblErrorMess.Text = "You have not signed out on " + Dates;

                        dsSignIn.Clear();

                        dsSignIn.Clear();
                    }
                    else // Missing Sign-Outs--"IF"-"Else"
                    {
                        dsSignIn.Clear();
                        dsSignIn = CheckLeaveDetails(objSignInSignOutModel);
                        if (dsSignIn.Tables[0].Rows.Count != 0)
                        {
                            objSignInSignOutModel.EmployeeID = Convert.ToInt32(User.Identity.Name);
                            objSignInSignOutModel.IsSignInManual = 0;
                            objSignInSignOutModel.IsSignOutManual = 0;
                            SignIn(objSignInSignOutModel);
                            lblErrorMess.Text = "You have applied for leave for this day. So your Sign In Entry is marked PENDING";
                        }
                        else
                        {
                            objSignInSignOutModel.IsSignInManual = 0;
                            objSignInSignOutModel.IsSignOutManual = 0;
                            SignIn(objSignInSignOutModel);
                        }
                    }
                }
                if (lblErrorMess.Text == "")
                {
                    lblSuccess.Text = "You have successfully Signed-in for the day. Have a productive day ahead!";
                }
                ViewState["ColumnName"] = "date";
                ViewState["Order"] = "DESC";
                objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();
                ddlType.SelectedValue = "7";
                BindSignInSignOut(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.cs", "btnSignIn_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #region SignIn

        //check if the employee is on leave -- Employee will not be allowed to sign-in if he has an approved leave
        public DataSet CheckLeaveDetails(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                DataSet dsCheckLeaveDetails = new DataSet();
                dsCheckLeaveDetails = objSignInSignOutBOL.CheckLeaveDetails(objSignInSignOutModel);
                return dsCheckLeaveDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.cs", "CheckLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        // Check if the Employee has already Signed_in for the day
        public DataSet CheckForMultipleSignIn(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                DataSet dsCheckForMultipleSignIn = new DataSet();
                dsCheckForMultipleSignIn = objSignInSignOutBOL.CheckForMultipleSignIn(objSignInSignOutModel);
                return dsCheckForMultipleSignIn;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.cs", "CheckForMultipleSignIn", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        //check for the missing Sign out entries. Employee will not be allowed to sign in unless he has singed out for the corr sign-ins.
        public DataSet CheckMissingSignOut(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                DataSet dsCheckMissingSignOut = new DataSet();
                dsCheckMissingSignOut = objSignInSignOutBOL.CheckMissingSignOut(objSignInSignOutModel);
                return dsCheckMissingSignOut;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.cs", "CheckMissingSignOut", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        // after the above 3 functions(conditions) are satisfied(return false) the employee is allowed to sign-in.
        public void SignIn(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlDataReader sdrSignIn = objSignInSignOutBOL.SignIn(objSignInSignOutModel);
                Guid gSignInSignOutWFID = new Guid("00000000-0000-0000-0000-000000000000");
                int SignInSignOutID = 0;

                while (sdrSignIn.Read())
                {
                    if (sdrSignIn[1].ToString() != "")
                    {
                        SignInSignOutID = Convert.ToInt32(sdrSignIn[0].ToString());
                        gSignInSignOutWFID = new Guid(sdrSignIn[1].ToString());
                        System.Workflow.Runtime.WorkflowRuntime wr = (System.Workflow.Runtime.WorkflowRuntime)Application["WokflowRuntime"];
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("SignInSignOutID", SignInSignOutID);
                        WorkflowInstance wi = wr.CreateWorkflow(typeof(SignInSignOutWF.SignInSignOutWF), parameters, gSignInSignOutWFID);
                        wi.Start();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.cs", "SignIn", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion SignIn

        #region SignOut

        //Auto Sign Out
        public void SignOut(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                grdSignInSignOut.PageIndex = 0;
                //Checking if the user has already Signed-Out
                string SignInSignOutID = objSignInSignOutBOL.MultipleSignOuts(objSignInSignOutModel);
                if (SignInSignOutID == "")
                {
                    lblErrorMess.Text = "You have already Signed out";
                }
                //if (SignInSignOutID == "1")
                //{
                //    lblErrorMess.Text = "You are signing out from different time zone";
                //}
                //else if (SignInSignOutID == "2")
                //{
                //    lblErrorMess.Text = "You have already Signed out";
                //}
                else
                {
                    objSignInSignOutModel.SignInSignOutID = Convert.ToInt32(SignInSignOutID);
                    SqlDataReader sdrSignOut = objSignInSignOutBOL.SignOut(objSignInSignOutModel);

                    //workflow

                    Guid gMSignInSignOutWFID = new Guid("00000000-0000-0000-0000-000000000000");
                    int SignInSignOutID1 = 0;
                    while (sdrSignOut.Read())
                    {
                        if (sdrSignOut.FieldCount.ToString() == "2")
                        {
                            if (sdrSignOut[1].ToString() != "")
                            {
                                SignInSignOutID1 = Convert.ToInt32(sdrSignOut[0].ToString());
                                gMSignInSignOutWFID = new Guid(sdrSignOut[1].ToString());
                                WorkflowRuntime wr = (WorkflowRuntime)Application["WokflowRuntime"];
                                wr = new WorkflowRuntime();
                                Dictionary<string, object> parameters = new Dictionary<string, object>();
                                parameters.Add("SignInSignOutID", SignInSignOutID1);

                                WorkflowInstance wi = wr.CreateWorkflow(typeof(SignInSignOutWF.SignInSignOutWF), parameters, gMSignInSignOutWFID);
                                wi.Start();
                            }
                        }
                    }

                    //workflow
                    lblSuccess.Text = "You have successfully Signed Out for the day";
                }
                ViewState["ColumnName"] = "date";
                ViewState["Order"] = "DESC";
                objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();
                BindSignInSignOut(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.cs", "SignOut", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion SignOut

        protected void lnkModifyTime_Click(object sender, EventArgs e)
        {
            try
            {
                // redirects to manual sign in sign out page where the user can modify the in/out time for a selected date.
                Response.Redirect("ManualSignin.aspx");
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.cs", "lnkModifyTime_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            try
            {
                objSignInSignOutModel.SignOutTime = Convert.ToDateTime(Convert.ToString(hd_browserTime.Value));
                ddlType.SelectedValue = "7";
                lblErrorMess.Text = "";
                lblSuccess.Text = "";
                grdSignInSignOut.PageIndex = 0;
                SignOut(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.cs", "btnSignOut_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void grdSignInSignOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ManualSignin.aspx");
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.cs", "grdSignInSignOut_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void grdSignInSignOut_DataBound(object sender, EventArgs e)
        {
        }

        protected void grdSignInSignOut_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            bool location = hideManual;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Condition Added By Rahul Ramachandran so that the manual links should be disabled for Mumbai and Bengaluru Employees.
                    if (location)
                    {
                        DateTime dt = Convert.ToDateTime(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"].ToString());
                        string strDate = dt.ToShortDateString();

                        Label lblDate1 = (Label)e.Row.FindControl("lblDate");
                        lblDate1.Text = strDate;
                        lblDate1.Visible = true;
                        LinkButton lnkDate1 = (LinkButton)e.Row.FindControl("lnkDate");
                        lnkDate1.Visible = false;

                        Label lblInTime1 = (Label)e.Row.FindControl("lblInTime");
                        lblInTime1.Visible = true;
                        LinkButton lnkInTime1 = (LinkButton)e.Row.FindControl("lnkInTime");
                        lnkInTime1.Visible = false;

                        Label lblOutTime1 = (Label)e.Row.FindControl("lblOutTime");
                        lblOutTime1.Visible = true;
                        LinkButton lnkOutTime1 = (LinkButton)e.Row.FindControl("lnkOutTime");
                        lnkOutTime1.Visible = false;
                    }
                    if (ddlType.SelectedValue == "2")
                    {
                        DateTime dt = Convert.ToDateTime(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"].ToString());
                        string strDate = dt.ToShortDateString();
                        Label lblDate1 = (Label)e.Row.FindControl("lblDate");
                        lblDate1.Text = strDate;
                        lblDate1.Visible = true;
                        LinkButton lnkDate1 = (LinkButton)e.Row.FindControl("lnkDate");
                        lnkDate1.Visible = false;

                        Label lblInTime1 = (Label)e.Row.FindControl("lblInTime");
                        lblInTime1.Visible = true;
                        LinkButton lnkInTime1 = (LinkButton)e.Row.FindControl("lnkInTime");
                        lnkInTime1.Visible = false;

                        Label lblOutTime1 = (Label)e.Row.FindControl("lblOutTime");
                        lblOutTime1.Visible = true;
                        LinkButton lnkOutTime1 = (LinkButton)e.Row.FindControl("lnkOutTime");
                        lnkOutTime1.Visible = false;
                    }
                    if (ddlType.SelectedValue != "2")
                    {
                        if (dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["Mode"].ToString() == "Bulk")
                        {
                            DateTime dt;
                            string strDate;
                            if (!string.IsNullOrEmpty(Convert.ToString(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"])))
                            {
                                dt = Convert.ToDateTime(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"].ToString());
                                strDate = dt.ToShortDateString();
                            }
                            else
                            {
                                strDate = Convert.ToString(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"]);
                            }

                            Label lblDate1 = (Label)e.Row.FindControl("lblDate");
                            lblDate1.Text = strDate;
                            lblDate1.Visible = true;
                            LinkButton lnkDate1 = (LinkButton)e.Row.FindControl("lnkDate");
                            lnkDate1.Visible = false;

                            Label lblInTime1 = (Label)e.Row.FindControl("lblInTime");
                            lblInTime1.Visible = true;
                            LinkButton lnkInTime1 = (LinkButton)e.Row.FindControl("lnkInTime");
                            lnkInTime1.Visible = false;

                            Label lblOutTime1 = (Label)e.Row.FindControl("lblOutTime");
                            lblOutTime1.Visible = true;
                            LinkButton lnkOutTime1 = (LinkButton)e.Row.FindControl("lnkOutTime");
                            lnkOutTime1.Visible = false;
                        }
                        DateTime dtTable;
                        if (!string.IsNullOrEmpty(Convert.ToString(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"])))
                        {
                            dtTable = Convert.ToDateTime(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"].ToString());
                        }
                        else
                        {
                            dtTable = Convert.ToDateTime(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"].ToString());
                        }

                        DateTime dtConfig = Convert.ToDateTime(dsLoadSignInSigOutData.Tables[2].Rows[0]["ConfigItemValue"].ToString());

                        if (dtConfig >= dtTable)
                        {
                            DateTime dt;
                            string strDate;
                            if (!string.IsNullOrEmpty(Convert.ToString(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"])))
                            {
                                dt = Convert.ToDateTime(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"].ToString());
                                strDate = dt.ToShortDateString();
                            }
                            else
                            {
                                strDate = Convert.ToString(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"]);
                            }

                            Label lblDate1 = (Label)e.Row.FindControl("lblDate");
                            lblDate1.Text = strDate;
                            lblDate1.Visible = true;
                            LinkButton lnkDate1 = (LinkButton)e.Row.FindControl("lnkDate");
                            lnkDate1.Visible = false;

                            Label lblInTime1 = (Label)e.Row.FindControl("lblInTime");
                            lblInTime1.Visible = true;
                            LinkButton lnkInTime1 = (LinkButton)e.Row.FindControl("lnkInTime");
                            lnkInTime1.Visible = false;

                            Label lblOutTime1 = (Label)e.Row.FindControl("lblOutTime");
                            lblOutTime1.Visible = true;
                            LinkButton lnkOutTime1 = (LinkButton)e.Row.FindControl("lnkOutTime");
                            lnkOutTime1.Visible = false;
                        }

                        if (!location && dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["SignOutTime"].ToString() == "")
                        {
                            LinkButton lnkOutTime = (LinkButton)e.Row.FindControl("lnkOutTime");
                            lnkOutTime.Text = "Enter Out Time";
                            lnkOutTime.Visible = true;
                        }
                        if (dtTable.Date == DateTime.Now.Date && dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["SignOutTime"].ToString() == "")
                        {
                            DateTime dt;
                            string strDate;
                            if (!string.IsNullOrEmpty(Convert.ToString(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"])))
                            {
                                dt = Convert.ToDateTime(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"].ToString());
                                strDate = dt.ToShortDateString();
                            }
                            else
                            {
                                strDate = Convert.ToString(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"]);
                            }
                            Label lblDate1 = (Label)e.Row.FindControl("lblDate");
                            lblDate1.Text = strDate;
                            lblDate1.Visible = true;
                            LinkButton lnkDate1 = (LinkButton)e.Row.FindControl("lnkDate");
                            lnkDate1.Visible = false;

                            Label lblOutTime1 = (Label)e.Row.FindControl("lblOutTime");
                            lblOutTime1.Visible = false;
                            LinkButton lnkOutTime1 = (LinkButton)e.Row.FindControl("lnkOutTime");
                            lnkOutTime1.Visible = false;
                        }

                        if (dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"] != null)
                        {
                            DateTime date1 = Convert.ToDateTime(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"].ToString());
                            string strDate;
                            if (string.IsNullOrEmpty(Convert.ToString(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"])))
                            {
                                strDate = Convert.ToString(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"]);
                            }
                            else
                            {
                                date1 = Convert.ToDateTime(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["date"].ToString());
                                strDate = date1.ToShortDateString();
                            }
                            LinkButton lnkDate1 = (LinkButton)e.Row.FindControl("lnkDate");
                            lnkDate1.Text = strDate;
                            if (dsLoadSignInSigOutData.Tables[3].Rows[0]["ShiftName"].ToString() == "General")
                            {
                                if ((date1.DayOfWeek.ToString() == "Sunday") || (date1.DayOfWeek.ToString() == "Saturday"))
                                {
                                    e.Row.Font.Bold = true;

                                    Label lblDate1 = (Label)e.Row.FindControl("lblDate");
                                    lblDate1.Text = strDate;
                                    lblDate1.Text = "<b>" + lblDate1.Text + "</b>";
                                    lnkDate1.Text = "<font style='color:Black'>" + lnkDate1.Text + "<font>";

                                    Label lblInTime1 = (Label)e.Row.FindControl("lblInTime");
                                    lblInTime1.Text = "<b>" + lblInTime1.Text + "</b>";
                                    LinkButton lnkInTime1 = (LinkButton)e.Row.FindControl("lnkInTime");
                                    lnkInTime1.Text = "<font style='color:Black'>" + lnkInTime1.Text + "<font>";

                                    Label lblOutTime1 = (Label)e.Row.FindControl("lblOutTime");
                                    lblOutTime1.Text = "<b>" + lblOutTime1.Text + "</b>";
                                    LinkButton lnkOutTime1 = (LinkButton)e.Row.FindControl("lnkOutTime");
                                    lnkOutTime1.Text = "<font style='color:Black'>" + lnkOutTime1.Text + "<font>";
                                    Label lblStatus = (Label)e.Row.FindControl("lblStatus1");
                                    lblStatus.Text = "<b>" + lblStatus.Text + "</b>";
                                }
                                else
                                {
                                    for (int i = 0; i < dsLoadSignInSigOutData.Tables[1].Rows.Count; i++)
                                    {
                                        DateTime date2 = Convert.ToDateTime(dsLoadSignInSigOutData.Tables[1].Rows[i]["HolidayDate"].ToString());
                                        if (date1.Date == date2.Date)
                                        {
                                            e.Row.Font.Bold = true;

                                            Label lblDate1 = (Label)e.Row.FindControl("lblDate");
                                            lblDate1.Text = strDate;
                                            lblDate1.Text = "<b>" + lblDate1.Text + "</b>";

                                            lnkDate1.Text = "<font style='color:Black'>" + lnkDate1.Text + "<font>";

                                            Label lblInTime1 = (Label)e.Row.FindControl("lblInTime");
                                            lblInTime1.Text = "<b>" + lblInTime1.Text + "</b>";
                                            LinkButton lnkInTime1 = (LinkButton)e.Row.FindControl("lnkInTime");
                                            lnkInTime1.Text = "<font style='color:Black'>" + lnkInTime1.Text + "<font>";

                                            Label lblOutTime1 = (Label)e.Row.FindControl("lblOutTime");
                                            lblOutTime1.Text = "<b>" + lblOutTime1.Text + "</b>";
                                            LinkButton lnkOutTime1 = (LinkButton)e.Row.FindControl("lnkOutTime");
                                            lnkOutTime1.Text = "<font style='color:Black'>" + lnkOutTime1.Text + "<font>";
                                            Label lblStatus = (Label)e.Row.FindControl("lblStatus1");
                                            lblStatus.Text = "<b>" + lblStatus.Text + "</b>";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataSet dsweeklyOff = new DataSet();
                                dsweeklyOff = objSignInSignOutBOL.GetWeeklyOff(objSignInSignOutModel);
                                bool flagWeekOff = false;
                                for (int i = 0; i < dsweeklyOff.Tables[0].Rows.Count; i++)
                                {
                                    DateTime weekoff1 = Convert.ToDateTime(dsweeklyOff.Tables[0].Rows[i]["Weekoff1"]);
                                    DateTime weekoff2 = Convert.ToDateTime(dsweeklyOff.Tables[0].Rows[i]["Weekoff2"]);

                                    if ((weekoff1.ToString("MM/dd/yy") == date1.ToString("MM/dd/yy")) || (weekoff2.ToString("MM/dd/yy")) == date1.ToString("MM/dd/yy"))
                                    {
                                        flagWeekOff = true;
                                        break;
                                    }
                                }
                                if (flagWeekOff == true)
                                {
                                    e.Row.Font.Bold = true;

                                    Label lblDate1 = (Label)e.Row.FindControl("lblDate");
                                    lblDate1.Text = strDate;
                                    lblDate1.Text = "<b>" + lblDate1.Text + "</b>";
                                    lnkDate1.Text = "<font style='color:Black'>" + lnkDate1.Text + "<font>";

                                    Label lblInTime1 = (Label)e.Row.FindControl("lblInTime");
                                    lblInTime1.Text = "<b>" + lblInTime1.Text + "</b>";
                                    LinkButton lnkInTime1 = (LinkButton)e.Row.FindControl("lnkInTime");
                                    lnkInTime1.Text = "<font style='color:Black'>" + lnkInTime1.Text + "<font>";

                                    Label lblOutTime1 = (Label)e.Row.FindControl("lblOutTime");
                                    lblOutTime1.Text = "<b>" + lblOutTime1.Text + "</b>";
                                    LinkButton lnkOutTime1 = (LinkButton)e.Row.FindControl("lnkOutTime");
                                    lnkOutTime1.Text = "<font style='color:Black'>" + lnkOutTime1.Text + "<font>";
                                    Label lblStatus = (Label)e.Row.FindControl("lblStatus1");
                                    lblStatus.Text = "<b>" + lblStatus.Text + "</b>";
                                }
                                else
                                {
                                    for (int i = 0; i < dsLoadSignInSigOutData.Tables[4].Rows.Count; i++)
                                    {
                                        DateTime date2 = Convert.ToDateTime(dsLoadSignInSigOutData.Tables[4].Rows[i]["HolidayDate"].ToString());
                                        if (date1.Date == date2.Date)
                                        {
                                            e.Row.Font.Bold = true;

                                            Label lblDate1 = (Label)e.Row.FindControl("lblDate");
                                            lblDate1.Text = strDate;
                                            lblDate1.Text = "<b>" + lblDate1.Text + "</b>";

                                            lnkDate1.Text = "<font style='color:Black'>" + lnkDate1.Text + "<font>";

                                            Label lblInTime1 = (Label)e.Row.FindControl("lblInTime");
                                            lblInTime1.Text = "<b>" + lblInTime1.Text + "</b>";
                                            LinkButton lnkInTime1 = (LinkButton)e.Row.FindControl("lnkInTime");
                                            lnkInTime1.Text = "<font style='color:Black'>" + lnkInTime1.Text + "<font>";

                                            Label lblOutTime1 = (Label)e.Row.FindControl("lblOutTime");
                                            lblOutTime1.Text = "<b>" + lblOutTime1.Text + "</b>";
                                            LinkButton lnkOutTime1 = (LinkButton)e.Row.FindControl("lnkOutTime");
                                            lnkOutTime1.Text = "<font style='color:Black'>" + lnkOutTime1.Text + "<font>";
                                            Label lblStatus = (Label)e.Row.FindControl("lblStatus1");
                                            lblStatus.Text = "<b>" + lblStatus.Text + "</b>";
                                        }
                                    }
                                }
                            }
                        }
                        //absenteesim dataset doesnt have any config or holiday table
                        if (dsLoadSignInSigOutData.Tables[1].Rows.Count == 0)
                        {
                            grdSignInSignOut.Columns[11].Visible = false;
                        }
                        if (Convert.ToInt32(dsLoadSignInSigOutData.Tables[0].Rows[e.Row.DataItemIndex]["IsBulk"]) == 1)
                        {
                            LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkbtnEdit");
                            lnkEdit.Visible = false;
                            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                            lblStatus.Visible = true;
                            lblStatus.Text = "Bulk";
                        }
                    }
                }
            }
            //}
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.aspx.cs", "grdSignInSignOut_RowDataBound", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void grdSignInSignOut_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                //Response.Redirect("ManualSignInSignOut.aspx");
                Response.Redirect("ManualSignin.aspx");
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.cs", "grdSignInSignOut_RowEditing", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                lblSuccess.Text = "";
                lblErrorMess.Text = "";
                grdSignInSignOut.PageIndex = e.NewPageIndex;

                if ((ddlType.SelectedValue.ToString() == "7") && (txtSearchToDate.Text == ""))
                {
                    objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                    objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();
                    BindSignInSignOut(objSignInSignOutModel);
                }
                else
                {
                    objSignInSignOutModel.StatusID = Convert.ToInt32(ddlType.SelectedValue);
                    DateTime dt = DateTime.Now;

                    if (txtSearchToDate.Text != "")
                    {
                        objSignInSignOutModel.Todate = Convert.ToDateTime(txtSearchToDate.Text);
                    }
                    else
                    {
                        if (ddlType.SelectedValue.ToString() == "2") // absent selected.. show only 1 yr's recs
                        {
                            objSignInSignOutModel.Todate = Convert.ToDateTime(dt.Date);
                        }
                        else
                        {
                            objSignInSignOutModel.Todate = Convert.ToDateTime(dt.Date.AddMonths(12));
                        }
                    }
                    if (txtSearchFromDate.Text != "")
                    {
                        objSignInSignOutModel.FromDate = Convert.ToDateTime(txtSearchFromDate.Text);
                    }
                    else
                    {
                        if (ddlType.SelectedValue.ToString() == "2")// absent selected.. show only 1 yr's recs
                        {
                            objSignInSignOutModel.FromDate = Convert.ToDateTime(dt.AddMonths(-12));
                        }
                        else
                        {
                            objSignInSignOutModel.FromDate = Convert.ToDateTime(dt.AddYears(-3));
                        }
                    }
                    objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                    objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();
                    dsLoadSignInSigOutData = Search(objSignInSignOutModel);
                    grdSignInSignOut.DataSource = dsLoadSignInSigOutData;
                    grdSignInSignOut.DataBind();
                    grdSignInSignOut.PageIndex = e.NewPageIndex;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.aspx.cs", "gridView_PageIndexChanging", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                lblSuccess.Text = "";
                lblErrorMess.Text = "";
                //  dsLoadSignInSigOutData = objSignInSignOutBOL.BindSignInSignOut(objSignInSignOutModel);
                // //grdSignInSignOut.DataSource = dsLoadSignInSigOutData.Tables[0];
                //// grdSignInSignOut.DataBind();
                // DataTable dt = dsLoadSignInSigOutData.Tables[0];

                // DataView dv = new DataView(dt);

                if ((ViewState["Order"] == null))
                {
                    ViewState["Order"] = "ASC";
                }
                else if (ViewState["Order"].ToString() == "ASC")
                {
                    ViewState["Order"] = "DESC";
                }
                else if (ViewState["Order"].ToString() == "DESC")
                {
                    ViewState["Order"] = "ASC";
                }
                //string strOrder = this.ViewState["Order"].ToString();
                // dv.Sort =  e.SortExpression + " " + strOrder;
                ViewState["ColumnName"] = e.SortExpression;
                objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();
                if ((ddlType.SelectedValue.ToString() == "7") && (txtSearchToDate.Text == ""))
                {
                    objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                    objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();
                    BindSignInSignOut(objSignInSignOutModel);
                }
                else
                {
                    objSignInSignOutModel.StatusID = Convert.ToInt32(ddlType.SelectedValue);
                    DateTime dt = DateTime.Now;

                    if (txtSearchToDate.Text != "")
                    {
                        objSignInSignOutModel.Todate = Convert.ToDateTime(txtSearchToDate.Text);
                    }
                    else
                    {
                        if (ddlType.SelectedValue.ToString() == "2") // absent selected.. show only 1 yr's recs
                        {
                            objSignInSignOutModel.Todate = Convert.ToDateTime(dt.Date);
                        }
                        else
                        {
                            objSignInSignOutModel.Todate = Convert.ToDateTime(dt.Date.AddMonths(12));
                        }
                    }
                    if (txtSearchFromDate.Text != "")
                    {
                        objSignInSignOutModel.FromDate = Convert.ToDateTime(txtSearchFromDate.Text);
                    }
                    else
                    {
                        if (ddlType.SelectedValue.ToString() == "2")// absent selected.. show only 1 yr's recs
                        {
                            objSignInSignOutModel.FromDate = Convert.ToDateTime(dt.AddMonths(-12));
                        }
                        else
                        {
                            objSignInSignOutModel.FromDate = Convert.ToDateTime(dt.AddYears(-3));
                        }
                    }
                    objSignInSignOutModel.ColumnName = ViewState["ColumnName"].ToString();
                    objSignInSignOutModel.SortOrder = ViewState["Order"].ToString();

                    dsLoadSignInSigOutData = Search(objSignInSignOutModel);
                    grdSignInSignOut.DataSource = dsLoadSignInSigOutData;
                    grdSignInSignOut.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.aspx.cs", "gridView_Sorting", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void grdSignInSignOut_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditSignInSignOut")
                {
                    //Label lblSignInSignOutID = (Label) grdSignInSignOut.FindControl("lblSignInSignOutID");
                    strSignInSignOutID = e.CommandArgument.ToString();
                    //Response.Redirect("ManualSignInSignOut.aspx?ID=" + strSignInSignOutID);
                    Response.Redirect("ManualSignin.aspx?ID=" + strSignInSignOutID);
                }
                if (e.CommandName == "Date")
                {
                    strSignInSignOutID = e.CommandArgument.ToString();
                    Session["CommandName"] = "Date;" + strSignInSignOutID;

                    //Response.Redirect("ManualSignInSignOut.aspx");
                    Response.Redirect("ManualSignin.aspx");
                }
                if (e.CommandName == "InTime")
                {
                    strSignInSignOutID = e.CommandArgument.ToString();
                    Session["CommandName"] = "InTime;" + strSignInSignOutID;
                    Response.Redirect("ManualSignin.aspx");
                }

                if (e.CommandName == "OutTime")
                {
                    strSignInSignOutID = e.CommandArgument.ToString();
                    Session["CommandName"] = "OutTime;" + strSignInSignOutID;
                    Response.Redirect("ManualSignin.aspx");
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.aspx.cs", "grdSignInSignOut_RowCommand", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #region Search

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                lblSuccess.Text = "";
                lblErrorMess.Text = "";
                grdSignInSignOut.PageIndex = 0;
                objSignInSignOutModel.StatusID = Convert.ToInt32(ddlType.SelectedValue);
                objSignInSignOutModel.Todate = Convert.ToDateTime(txtSearchToDate.Text);
                objSignInSignOutModel.FromDate = Convert.ToDateTime(txtSearchFromDate.Text);
                objSignInSignOutModel.ColumnName = "date";
                objSignInSignOutModel.SortOrder = "DESC";
                ViewState["ColumnName"] = "date";
                ViewState["Order"] = "DESC";
                dsLoadSignInSigOutData = Search(objSignInSignOutModel);
                if (dsLoadSignInSigOutData.Tables[0].Rows.Count == 0)
                {
                    lblErrorMess.Text = "No records found!";
                    grdSignInSignOut.Visible = false;
                }
                else
                {
                    lblErrorMess.Text = "";
                    grdSignInSignOut.Visible = true;
                    grdSignInSignOut.DataSource = dsLoadSignInSigOutData;
                    grdSignInSignOut.DataBind();
                    //txtSearchFromDate.Text = "";
                    //txtSearchToDate.Text = "";
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.aspx.cs", "btnSearch_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public DataSet Search(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                return objSignInSignOutBOL.Search(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.aspx.cs", "Search", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #endregion Search

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
                grdSignInSignOut.PageIndex = 0;
                lblErrorMess.Text = "";
                lblSuccess.Text = "";
                if (ddlType.SelectedValue.ToString() != "-1")
                {
                    objSignInSignOutModel.SignInSignOutID = Convert.ToInt32(User.Identity.Name);
                    objSignInSignOutModel.StatusID = Convert.ToInt32(ddlType.SelectedValue);
                    DateTime dt = DateTime.Now;
                    if (ddlType.SelectedValue.ToString() == "2")
                    {
                        objSignInSignOutModel.Todate = Convert.ToDateTime(dt.Date);
                        objSignInSignOutModel.FromDate = Convert.ToDateTime(dt.AddMonths(-6));
                    }
                    else
                    {
                        objSignInSignOutModel.Todate = Convert.ToDateTime(dt.Date.AddYears(1));
                        objSignInSignOutModel.FromDate = Convert.ToDateTime(dt.AddYears(-3));
                    }
                    ViewState["ColumnName"] = "date";
                    ViewState["Order"] = "DESC";
                    objSignInSignOutModel.ColumnName = "date";
                    objSignInSignOutModel.SortOrder = "DESC";
                    dsLoadSignInSigOutData = Search(objSignInSignOutModel);
                    if (dsLoadSignInSigOutData.Tables[0].Rows.Count == 0)
                    {
                        lblErrorMess.Text = "No records Found.";
                        grdSignInSignOut.Visible = false;
                    }
                    else
                    {
                        lblErrorMess.Text = "";
                        grdSignInSignOut.Visible = true;
                        grdSignInSignOut.DataSource = dsLoadSignInSigOutData;
                        grdSignInSignOut.DataBind();
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.aspx.cs", "ddlType_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearchFromDate.Text = "";
                txtSearchToDate.Text = "";
                ddlType.SelectedValue = "7";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOut.aspx.cs", "btnReset_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}