using System;

namespace V2.Helpdesk.web.admin
{
    /// <summary>
    /// Summary description for AuthorizationErrorMessage.
    /// </summary>
    public partial class AuthorizationErrorMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request.QueryString["PageSource"] != null)
                Session["PageSource"] = Request.QueryString["PageSource"];
            // Put user code to initialize the page here
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
    }
}