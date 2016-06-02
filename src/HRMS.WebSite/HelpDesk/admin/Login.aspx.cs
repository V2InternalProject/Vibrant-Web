using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for Login.
    /// </summary>
    public partial class Login : System.Web.UI.Page
    {
        #region Variable declaration

        private Model.clsLogin objLogin;
        private BusinessLayer.clsBLLogin objBLLogin;
        private int recordcount;
        protected System.Web.UI.WebControls.Label lblBlankUserID;
        protected System.Web.UI.WebControls.Label lblBlankPassword;

        #endregion Variable declaration

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Put user code to initialize the page here
                //Session["EmployeeID"] = 0;
                Session["SAEmployeeID"] = 0;
                Session["OnlySuperAdmin"] = 0;
                //Session["SuperAdmin"] = 0;
                loginAccess();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "Login.aspx", "Page_Load", ex.StackTrace);
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

        //To verify loginid and password
        //protected void btnSubmit_Click(object sender, System.EventArgs e)
        public void loginAccess()
        {
            if ((Session["SAEmployeeID"].ToString() == "0"))
            {
                DataSet dsEmployeeExists = new DataSet();
                // DataSet isEmployeeSuperAdmin = new DataSet();
                int isEmployeeSuperAdmin = 0;
                objLogin = new Model.clsLogin();
                objBLLogin = new BusinessLayer.clsBLLogin();
                // int user =
                try
                {
                    objLogin.EmployeeID = Convert.ToInt32(Session["EmployeeID"]);
                    //objLogin.Password = txtPassword.Text;

                    //if (Membership.ValidateUser(txtUserID.Text, txtPassword.Text))
                    {
                        recordcount = objBLLogin.DoesEmployeeIDExist(objLogin);
                        if (recordcount > 0)
                        {
                            dsEmployeeExists = objBLLogin.IsEmployeeIDValid(objLogin);
                            if (dsEmployeeExists.Tables[0].Rows.Count > 0)
                            {
                                if (dsEmployeeExists.Tables[0].Rows[0]["isAdmin"].ToString() == "1" || dsEmployeeExists.Tables[0].Rows[0]["isAdmin"].ToString() == "True")
                                {
                                    Session["SAEmployeeID"] = Convert.ToInt32(Session["EmployeeID"]);
                                    if (Roles.IsUserInRole(Convert.ToString(Session["EmployeeID"]), "Super Admin"))
                                    {
                                        Session["SuperAdmin"] = Convert.ToInt32(Session["EmployeeID"]);
                                    }
                                    //Response.Redirect("ViewSuperAdminIssues.aspx");
                                    Response.Redirect("IssueHealth.aspx", false);
                                }
                                else if (dsEmployeeExists.Tables[0].Rows[0]["isAdmin"].ToString() == "0" || dsEmployeeExists.Tables[0].Rows[0]["isAdmin"].ToString() == "False")
                                {
                                    //check if he has super admin role
                                    //then  give him roles for masters and other roles.
                                    isEmployeeSuperAdmin = objBLLogin.isEmployeeSuperAdmin(objLogin);
                                    if (isEmployeeSuperAdmin > 0)
                                    {
                                        Session["SuperAdmin"] = Convert.ToInt32(Session["EmployeeID"]);
                                    }
                                    Session["IsExecutive"] = 1;
                                    Session["EmployeeID"] = Convert.ToInt32(Session["EmployeeID"]);
                                    Session["SAEmployeeID"] = Convert.ToInt32(Session["EmployeeID"]);
                                    Response.Redirect("IssueHealth.aspx", false);
                                }
                                else
                                {
                                    lblMsg.Text = "UserID does not exist";
                                }
                            }
                            else if (Roles.IsUserInRole(Convert.ToString(Session["EmployeeID"]), "Super Admin"))
                            {
                                Session["SuperAdmin"] = Convert.ToInt32(Session["EmployeeID"]);
                                Session["OnlySuperAdmin"] = Convert.ToInt32(Session["EmployeeID"]);
                                Response.Redirect("ViewEmployeeDetails.aspx", false);
                            }
                            else
                            {
                                lblMsg.Text = "Sorry, you are not authorize to access the system";
                            }
                        }
                        else if (Roles.IsUserInRole(Convert.ToString(Session["EmployeeID"]), "Super Admin"))
                        {
                            Session["SuperAdmin"] = Convert.ToInt32(Session["EmployeeID"]);
                            Session["OnlySuperAdmin"] = Convert.ToInt32(Session["EmployeeID"]);
                            Response.Redirect("ViewEmployeeDetails.aspx");
                        }
                        else
                        {
                            lblMsg.Text = "Sorry, you are not authorize to access the system";
                        }
                    }

                    //else
                    //{
                    //    lblMsg.Text = "Not a Valid User, Please check the credentials";
                    //}
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
                    objFileLog.WriteLine(LogType.Error, ex.Message, "Login.aspx", "btnSubmit_Click", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(), ex);
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                txtUserID.Text = "";
                txtPassword.Text = "";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "Login.aspx", "btnCancel_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}