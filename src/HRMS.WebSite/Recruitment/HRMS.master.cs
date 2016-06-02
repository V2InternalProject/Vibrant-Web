using BLL;
using BOL;
using System;
using System.Data;
using System.Web;
using System.Web.UI;

public partial class HRMSMaster : System.Web.UI.MasterPage
{
    private UtilityBLL objUtilityBLL = new UtilityBLL();
    private UtilityBOL objUtilityBOL = new UtilityBOL();
    private DataSet dsEmployeeInfo = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if ((Session["HRMRole"] != null) || (Session["RequestorRole"] != null) || (Session["ApproverRole"] != null) || (Session["InterviewerRole"] != null) || (Session["RecruiterRole"] != null))
            {
                objUtilityBOL.UserID = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                Session["userID"] = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                Page.RegisterClientScriptBlock("MyScript", "<SCRIPT Language='JavaScript'> alert('You are not authorized to access this site.');  window.location = 'http://myvibrantweb.v2solutions.com/'</SCRIPT>");
            }
        }
    }
}