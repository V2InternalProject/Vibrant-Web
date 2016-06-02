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
using V2.Orbit.Model;
using V2.Orbit.BusinessLayer;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

public partial class Orbit : System.Web.UI.MasterPage
{

    OutOfOfficeModel objOutOfOfficeModel = new OutOfOfficeModel();
    OutOfOfficeBOL objOutOfOfficeBOL = new OutOfOfficeBOL();
    DataSet dsEmployeeName;

    protected void Page_Load(object sender, EventArgs e)
    {
        //try
        //{
        //    lblUser.Text = HttpContext.Current.User.Identity.Name;
        //    objOutOfOfficeModel.UserId = Convert.ToInt32(lblUser.Text);
        //    //dsEmployeeName = objOutOfOfficeBOL.GetEmployeeNameDetails(objOutOfOfficeModel);
        //    dsEmployeeName = objOutOfOfficeBOL.GetEmployeeNameRpt(objOutOfOfficeModel);
        //    if (dsEmployeeName.Tables[0].Rows.Count > 0)
        //    {
        //        lblUser.Text = dsEmployeeName.Tables[3].Rows[0]["EmployeeName"].ToString();
        //    }
        //    else
        //    {
        //        lblUser.Text = HttpContext.Current.User.Identity.Name;
        //    }
        //}
        //catch (V2Exceptions ex)
        //{
        //    throw;
        //}
        //catch (System.Exception ex)
        //{
        //    FileLog objFileLog = FileLog.GetLogger();
        //    objFileLog.WriteLine(LogType.Error, ex.Message, "OrbitMaster.aspx.cs", "Page_Load", ex.StackTrace);
        //    throw new V2Exceptions();
        //}
    }
    protected void lgstOrbitMaster_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        try
        {            
            FormsAuthentication.SignOut();
	    //Code Added For External Users Access the Application through Dwarpal.
            //From here
	        
	      string str = Request.Url.ToString();
            if (str.Contains("dwarpal"))
            {
                Response.Redirect("/V2voice/Tools.html");
            }
            else 
            {
                //Response.Redirect("http://v2tools.v2solutions.com/");
                Response.Redirect("http://myv2.v2solutions.com/");  
                //Response.Redirect("/tools/indexLeftMenu_Test.htm");
                //  Response.Redirect("/V2voice/index.htm");
            }     
		
	    //Till here
            //Response.Redirect("http://Intranet/V2voice/index.htm");
            //Response.Write("<script>window.open('Blank.htm');</script>");

        }
            catch(System.Threading.ThreadAbortException ex)
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
            objFileLog.WriteLine(LogType.Error, ex.Message, "OrbitMaster.aspx.cs", "lgstOrbitMaster_LoggingOut", ex.StackTrace);
            throw new V2Exceptions();
        }


    }
    //protected void ddlSwitchApplication_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {            
    //        if (ddlSwitchApplication.SelectedValue != "0")
    //        {
    //            if(ddlSwitchApplication.SelectedValue=="1")
    //            {
    //                Response.Redirect("/PMS2/PMS2web/Manage Project/ResourceAllocationHistory.aspx");
    //            }
    //            else if(ddlSwitchApplication.SelectedValue=="2")
    //            {
    //                Response.Redirect("/HelpDesk/helpdesk.web/index.aspx");
    //                //Code Added For External Users Access the Application through Dwarpal.
    //               //From here
    //               string str = Request.Url.ToString();
    //              //if (str.Contains("dwarpal"))
    //              //{
    //              //  Response.Write("<script language='javascript'>window.open('http://dwarpal.in.v2solutions.com:63819/helpdesk/');</script>");                   
    //              //}
    //              //else
    //              //{
    //              //  Response.Write("<script language='javascript'>window.open('http://intranettools/helpdesk/');</script>");
    //              //}
    //            //Till here 
    //            }               
    //        }


    //    }
    //    catch(System.Threading.ThreadAbortException ex)
    //    {
    //        throw;
    //    }
    //    catch (V2Exceptions ex)
    //    {
    //        throw;
    //    }
    //    catch (System.Exception ex)
    //    {
    //        FileLog objFileLog = FileLog.GetLogger();
    //        objFileLog.WriteLine(LogType.Error, ex.Message, "OrbitMaster.aspx.cs", "Page_Load", ex.StackTrace);
    //        throw new V2Exceptions();
    //    }
    //}
}
