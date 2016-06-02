namespace V2.Helpdesk.web.controls
{
    using System;
    using System.Configuration;
    using V2.CommonServices.Exceptions;
    using V2.CommonServices.FileLogger;

    /// <summary>
    ///		Summary description for AdminHeader.
    /// </summary>
    public partial class AdminHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (Request.QueryString["PageSource"] != null)
                    Session["PageSource"] = Request.QueryString["PageSource"];
                hdnTabClick.Text = Convert.ToString(Session["PageSource"]);
                if (((Session["SAEmployeeID"] != null) && (Session["SAEmployeeID"].ToString() != "0")) || ((Session["EmployeeID"] != null) && (Session["EmployeeID"].ToString() != "0")) || ((Session["OnlySuperAdmin"] != null) && (Session["OnlySuperAdmin"].ToString() != "0")) || ((Session["SuperAdmin"] != null) && (Session["SuperAdmin"].ToString() != "0")))
                {
                    //lnkBtnLogin.Visible = true;
                    //lnkBtnLogin.Text = "Logout";
                }
                else
                {
                    //lnkBtnLogin.Visible = false;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminHeader.ascx", "Page_Load", ex.StackTrace);
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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion Web Form Designer generated code

        protected void lnkBtnLogin_Click(object sender, System.EventArgs e)
        {
            try
            {
                Response.Redirect(ConfigurationManager.AppSettings["Log-OffURL"].ToString());
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "AdminHeader.ascx", "lnkBtnLogin_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}