namespace V2.Helpdesk.web.controls
{
    using System;
    using System.Web;

    /// <summary>
    ///		Summary description for header.
    /// </summary>
    public partial class header : System.Web.UI.UserControl
    {
        public HttpCookie cookie = null;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.Cache.SetExpires(DateTime.Now);
            cookie = (HttpCookie)Session["UserCookie"];
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

        //protected void ddlAppSwitch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        ////PMS
        //  if(ddlAppSwitch.SelectedValue=="1")
        //      Response.Redirect("/PMS2/Manage Project/ResourceAllocationHistory.aspx");
        //      //Orbit
        //  else if (ddlAppSwitch.SelectedValue=="2")
        //      Response.Redirect("/OrbitWeb/SignInSignOut.aspx");
        //}

        //protected void lnkLogout_Click(object sender, EventArgs e)
        //{
        //    HttpCookie cookie= null;
        //    string cookieName = "";

        //    //cookieName = cookie.Name;
        //    Context.Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
        //    Session.Abandon();
        //    Session["UName"] = 0;
        //    Session.Clear();
        //    HttpCookieCollection cookieCollection = new HttpCookieCollection();
        //    cookieCollection.Remove(cookieName);
        //    FormsAuthentication.SignOut();
        //    // cookie = null;
        //    Response.Redirect("http://myv2.v2solutions.com/");
        //}
    }
}