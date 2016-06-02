using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.BusinessLayer;
using System.IO;
using System.Text;
using System.Diagnostics;

public partial class Logon : System.Web.UI.Page
{
    string strUserName, strPassword, strApp;

    public string LogFileName
    {
        get
        {
            return @"D:\TestApplication\VW Integration\Orbit_Phase_II\OrbitWeb\Log\TempLog\LogFile.txt";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CommanMethodsBOL common = new CommanMethodsBOL();
            strApp = Convert.ToString(Request.Form["IntranetApplication"]);
            //strUserName = Convert.ToString(Request.QueryString["employeeID"]);
            string UserName = "";

            //UserName = common.Decrypt(Request.QueryString["employeeCode"].ToString(), true);
            //UserName = UserName.Replace(Request.QueryString["AsciiKey"].ToString(), "");
            //strUserName = UserName;

            //strUserName =Convert.ToString(Request.Form["UserName"]);
            //  strPassword=Convert.ToString(Request.Form["pwd"]);

            // strUserName = "3704";
            strUserName = "1878";
            // strApp = "ORBIT";
            //  strUserName = employeeID.ToString();
            // strPassword = "password";
            //if (strUserName == string.Empty || strPassword == string.Empty)
            //{
            //    Response.Redirect("http://www.google.com");

            //}
            //else
            //{
            //if(Membership.ValidateUser(strUserName,strPassword))
            //{
            //FormsAuthentication.RedirectFromLoginPage(strUserName,false);
            FormsAuthentication.SetAuthCookie(strUserName, false);
            Session["UName"] = strUserName;
            //if(strApp=="PMS")
            //    Response.Redirect("/PMS2/PMS2web/Manage Project/ResourceAllocationHistory.aspx");
            //else if (strApp == "ORBIT")
            Response.Redirect("/SignInSignOut.aspx");
            //Response.Redirect("/testOrbitWeb2/SignInSignOut.aspx");
            //else if (strApp == "HELPDESK")
            //    Response.Redirect("/HelpDesk/helpdesk.web/index.aspx");

            //}
            //else
            //{


            //    Response.Redirect("http://www.gmail.com?Username="+ strUserName +"&Password="+strPassword);

            //}
            //}
        }
        catch (System.Threading.ThreadAbortException ex)
        {
            // AddAnEntryToLogFile("Catch", ex.Message);
            throw;

        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "btnSubmit", ex.StackTrace);
            throw new V2Exceptions();
        }

    }


    protected void lnkHomePage_Click(object sender, EventArgs e)
    {
        try
        {

            //Code Added For External Users Access the Application through Dwarpal.
            //From here

            string str = Request.Url.ToString();
            if (str.Contains("dwarpal"))
            {
                Response.Redirect("/V2voice/Tools.html");
            }
            else
            {
                Response.Redirect("http://myv2.v2solutions.com/");
            }

            //Till here

            //Response.Redirect("http://Intranet/v2voice/index.htm");
        }
        catch (System.Threading.ThreadAbortException ex)
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
            objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveApproval.aspx.cs", "lnkHomePage_Click", ex.StackTrace);
            throw new V2Exceptions();
        }

    }

    /// <summary>
    /// AddAnEntryToLogFile() method adds an entry to a log file.
    /// </summary>
    /// <param name="FunctionName">Name of the function in which exception occured.</param>
    /// <param name="Message">The message to be displayed.</param>
    private void AddAnEntryToLogFile(string FunctionName, string Message)
    {
        try
        {
            DateTime dateTime = DateTime.Now;
            string logMessage = dateTime.ToString() + ", [" + FunctionName + "], " +
                System.Environment.NewLine +
                "======================================================================" +
                System.Environment.NewLine +
                Message +
                System.Environment.NewLine + System.Environment.NewLine;
            //string logPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.System), LogFileName);
            StreamWriter sw = new StreamWriter(LogFileName, true, Encoding.ASCII);

            try
            {
                sw.Write(logMessage);
            }
            finally
            {
                sw.Close();
            }
        }
        catch (Exception ex)
        {
            //EventLog.WriteEntry("ReminderService", Message + ex.ToString(), EventLogEntryType.Error);
        }
    }
}
