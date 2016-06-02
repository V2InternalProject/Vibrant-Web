using BLL;
using DAL;
using System;

//using System.Linq;
using System.Web.Security;

public partial class _Default : System.Web.UI.Page
{
    private MasterTableDAL masterDal = new MasterTableDAL();
    private string strUserName, strPassword, strApp;

    private int userId;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UtilityBLL common = new UtilityBLL();
            // strApp=Convert.ToString(Request.Form["IntranetApplication"]);

            string UserName = common.Decrypt(Request.QueryString["employeeCode"].ToString(), true);
            UserName = UserName.Replace(Request.QueryString["AsciiKey"].ToString(), "");
            strUserName = UserName;

            //Admin

            //strUserName = "3553";
            //strPassword = "password";
            //@movement13

            //if (strUserName == "" || strPassword == "")
            //{
            //   // Response.Redirect("http://www.google.com", false);

            //}
            //else
            {
                //if (Membership.ValidateUser(strUserName, strPassword))
                //{
                //FormsAuthentication.RedirectFromLoginPage(strUserName,false);
                FormsAuthentication.SetAuthCookie(strUserName, false);
                //string temp = strUserName;

                //string CustomIdentity = strUserName;

                Response.Redirect("welcome.aspx");
                //}

                //}
                //else
                //{
                //}
            }
        }
        catch (System.Threading.ThreadAbortException ex)
        {
        }
    }

    //DataSet Ds = new DataSet();

    //       Ds = masterDal.getUserRole(strUserName);

    //       int RoleId =Convert.ToInt32(Ds.Tables[0].Rows[0]["RoleID"].ToString());
    //string RoleName = Ds.Tables[0].Rows[0]["RoleName"].ToString();

    //Session
    //       //Response.Redirect("~/MastersTable.aspx");

    //       FormsAuthentication.SetAuthCookie(strUserName.ToString() , false);
    //        {
    //           // Response.Redirect("/ORBIT_PHASE_II/OrbitWeb/SignInSignOut.aspx");
    //           Response.Redirect("~/MastersTable.aspx");
    //       }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("http://v2tools.v2solutions.com/");
    }
}