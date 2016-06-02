using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ModelHelpDesk;
//using BusinessLayerHelpDesk;
using V2.Helpdesk;
using System.Web.Services;
using System.Net.Mail;
using System.Configuration;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;

using V2.Helpdesk.web;

namespace Helpdesk.web
{
    /// <summary>
    /// Summary description for WebForm1.
    /// </summary>
    public class WebForm1 : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.TextBox txtEmailID;
        protected System.Web.UI.WebControls.TextBox txtCCEmailID;
        protected System.Web.UI.WebControls.TextBox txtPhoneExtension;
        protected System.Web.UI.WebControls.TextBox txtSeatingLocation;
        protected System.Web.UI.WebControls.DropDownList ddlSeverity;
        protected System.Web.UI.HtmlControls.HtmlTextArea txtDescription;
        protected System.Web.UI.WebControls.Button btnReset;
        protected System.Web.UI.WebControls.Button btnSubmit;
        protected System.Web.UI.WebControls.DropDownList ddltype;
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.Label lblCcEmailError;
        protected System.Web.UI.WebControls.TextBox txtName;
        protected System.Web.UI.WebControls.Label lblMailError;
        public string strMemberEmailID;
        string strDeptEmailID, strDeptCCEmailID;
        public int intIssueID, EmployeeID;
        public DataSet dsIssueIDAndMemberEmailID, IssueSubmitResult;
        protected System.Web.UI.HtmlControls.HtmlInputFile uploadFiles;
        public int ShouldSendMail, ShouldUploadFile;
        public string strAssignedTo;
        string fn, fnExt;
        protected System.Web.UI.HtmlControls.HtmlSelect ddlCategories;

        private void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
            EmployeeID = Convert.ToInt32(Session["UName"]);
            clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
            DataSet dsEmail = objClsBLReportIssue.GetEmailID(EmployeeID);
            if (dsEmail.Tables[0].Rows.Count > 0)
            {
                txtEmailID.Text = dsEmail.Tables[0].Rows[0][0].ToString();
                txtName.Text = dsEmail.Tables[0].Rows[0]["UserName"].ToString();
            }
            if (EmployeeID.ToString() == "" || EmployeeID == 0)
            {
                //Response.Redirect("http://192.168.30.15/intranet/");
                Response.Redirect("http://myv2.v2solutions.com/");
            }
            else
            {

                if (!Page.IsPostBack)
                {
                    try
                    {
                        BindType();
                        BindSeverity();
                        BindSubCategory();

                    }

                    catch (V2Exceptions ex)
                    {
                        throw;

                    }
                    catch (System.Exception ex)
                    {
                        FileLog objFileLog = FileLog.GetLogger();
                        objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "Page_Load", ex.StackTrace);
                        throw new V2Exceptions();
                    }
                    ////	BindPriority();
                }
            }
            btnSubmit.Attributes.Add("onClick", "return validateAndCheck();");
            //lblMessage.Visible = false;
            //btnSubmit.Attributes.Add("onClick","return checkSubCategorySelection();");
        }

        private void BindSeverity()
        {
            try
            {
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                DataSet dsSeverity = objClsBLReportIssue.GetSeverity();
                if (dsSeverity.Tables[0].Rows.Count > 0)
                {
                    ddlSeverity.DataSource = dsSeverity.Tables[0];
                    ddlSeverity.DataValueField = dsSeverity.Tables[0].Columns[0].ToString();
                    ddlSeverity.DataTextField = dsSeverity.Tables[0].Columns[1].ToString();
                    ddlSeverity.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;

            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "BindSeverity", ex.StackTrace);
                throw new V2Exceptions();
            }
        }
        private void BindType()
        {
            try
            {
                ddltype.Items.Clear();

                ddltype.Items.Insert(0, new ListItem("Select", "0"));
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                DataSet dsType = objClsBLReportIssue.GetType();

                for (int i = 0; i < dsType.Tables[0].Rows.Count; i++)
                {
                    ddltype.Items.Add(new ListItem(dsType.Tables[0].Rows[i]["RequestType"].ToString(), dsType.Tables[0].Rows[i]["TypeID"].ToString()));
                }


            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "BindType", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        private void BindSubCategory()
        {
            try
            {
                ddlCategories.Items.Clear();
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                DataSet dsSubCategory = objClsBLReportIssue.GetSubCategory();

                for (int i = 0; i <= dsSubCategory.Tables[0].Rows.Count - 1; i++)
                {


                    ListItem li = new ListItem(dsSubCategory.Tables[0].Rows[i][1].ToString(), dsSubCategory.Tables[0].Rows[i][0].ToString());
                    //li.Attributes.Add("style","BACKGROUND-COLOR:red");
                    ddlCategories.Items.Add(li);
                    if (ddlCategories.Items[i].Value.Equals("0"))
                    {
                        ddlCategories.Items[i].Attributes.Add("class", "tableheader");
                    }
                    else
                    {
                        ddlCategories.Items[i].Text = ddlCategories.Items[i].Text.Replace("--", "");
                    }


                    //ddlCategories.Items[i].Attributes.Add("BackColor", "red");
                    //   ddlCategories.BackColor=Color.Yellow;
                }

                /*ddlCategories.DataSource = dsSubCategory.Tables[0];
                ddlCategories.DataValueField = dsSubCategory.Tables[0].Columns[0].ToString();
                ddlCategories.DataTextField = dsSubCategory.Tables[0].Columns[1].ToString();
                ddlCategories.DataBind();*/
                /*	for(int i =0;i<=ddlCategories.Items.Count-1;i++)
                    {
                        ListItem li;
                        li = (ListItem)ddlCategories.Items[i];
                        li.Attributes.Add("style","background: #333333");
                        //ddlCategories.Items.Add(li)	;
                    //	ddlCategories.Items[i].Attributes.Add("style","background: yellow");
                    }*/
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "BindSubCategory", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        private void BindPriority()
        {
            try
            {

                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                //DataSet dsPriority = objClsBLReportIssue.GetPriority();
                /*if(dsPriority.Tables[0].Rows.Count > 0)
                {
                    ddlPriority.DataSource = dsPriority.Tables[0];
                    ddlPriority.DataValueField = dsPriority.Tables[0].Columns[0].ToString();
                    ddlPriority.DataTextField = dsPriority.Tables[0].Columns[1].ToString();
                    ddlPriority.DataBind();
                }*/
            }
            catch (V2Exceptions ex)
            {
                throw;

            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "BindPriority", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                lblMessage.Text = "";
                lblCcEmailError.Text = "";
                if (txtCCEmailID.Text != "")
                {
                    bool result = objClsBLReportIssue.IsCCEmailExists(txtCCEmailID.Text);

                    if (result == false)
                    {
                        lblCcEmailError.Text = "Please enter valid 'CC to Email' address";
                    }
                    else
                    {
                        InsertIssueDetails();
                        if (ShouldUploadFile == 1)
                            UploadFiles();

                        if (ShouldSendMail == 1)
                            SendMail();



                        lblMessage.Text = "Issue Successfully recorded." + "</br>";
                        lblMailError.Visible = true;
                    }
                }
                else
                {
                    InsertIssueDetails();
                    if (ShouldUploadFile == 1)
                        UploadFiles();

                    if (ShouldSendMail == 1)
                        SendMail();

                    lblMessage.Text = "Issue Successfully recorded." + "</br>";
                    lblMailError.Visible = true;
                }


                /// <summary>
                /// To send emails to the user, the TL and the HelpDesk Member
                /// </summary>
                /// 
            }
            catch (V2Exceptions ex)
            {
                throw;

            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        //private void SendMail()
        //{

        //    try
        //    {
        //        //To send mail to the user
        //        clsReportIssue objClsReportIssue = new clsReportIssue();
        //        objClsReportIssue.SubCategoryID = Convert.ToInt32(ddlCategories.Items[ddlCategories.SelectedIndex].Value);
        //        string Category = GetCategoryName(objClsReportIssue);
        //        SmtpClient SmtpMail = new SmtpClient(ConfigurationSettings.AppSettings["SMTPServerName"].ToString());
        //        if (Category == "")
        //        {
        //            strDeptEmailID = ConfigurationSettings.AppSettings["IT_Dept_EmailID"].ToString();
        //        }
        //        else
        //        {
        //            //Category= Category.ToUpper();
        //            strDeptEmailID = ConfigurationSettings.AppSettings[Category].ToString();
        //        }
        //        //string strDeptEmailID = ConfigurationSettings.AppSettings["IT_Dept_EmailID"].ToString();
        //        string strPath = ConfigurationSettings.AppSettings["Mail_Path"].ToString();
        //        MailMessage sendMailToDept = new MailMessage();
        //        sendMailToDept.To.Add(new MailAddress(txtEmailID.Text));
        //        if (txtCCEmailID.Text != "")
        //        {
        //            sendMailToDept.CC.Add(txtCCEmailID.Text);
        //        }
        //        sendMailToDept.From = new MailAddress(strDeptEmailID);//"vruddhi.vasoo@in.v2solutions.com";
        //        //sendMailToDept.From = new MailAddress("v2system@in.v2solutions.com");
        //        sendMailToDept.Subject = "HelpDesk : IssueRecorded";
        //        sendMailToDept.Body = "Hi " + Server.HtmlEncode(txtName.Text) + "<br>" + "<br>" +
        //            "Your issue has been recorded successfully with the following contents:" + "<br>" +
        //            "Your Issue ID is " + "<b>" + intIssueID + "</b>" + "<br>" +
        //            //"Assigned To: " + strAssignedTo + "<br>" +  // Code written by Mohan on 16/04/07
        //            "<b>" + "Description: " + "</b>" + Server.HtmlEncode(txtDescription.Value.ToString()) + ".";

        //        ///sendMailToDept.Priority = MailPriority.Normal;
        //        sendMailToDept.IsBodyHtml = true;
        //        if (txtCCEmailID.Text != "")
        //        {
        //            MailMessage sentToTL = new MailMessage();
        //            sentToTL.To.Add(new MailAddress(txtCCEmailID.Text));
        //            sentToTL.From = new MailAddress(strDeptEmailID);//"vruddhi.vasoo@in.v2solutions.com";
        //            sentToTL.Subject = "HelpDesk : Issue Recorded";
        //            sentToTL.Body = "This mail is regarding the issue of:" + "<br>" +
        //                "Name: " + Server.HtmlEncode(txtName.Text) + " sent to HelpDesk." + "<br>" +
        //                "The contents of the issue are as follows:" + "<br>" +
        //                "Problem Severity: " + ddlSeverity.SelectedItem.Text + "<br>" +
        //                ////	"Priority:"+ddlPriority.SelectedItem.Text+"<br>"+
        //                "Description: " + Server.HtmlEncode(txtDescription.Value.ToString()) + "<br>" +
        //                "Issue Id: " + "<b>" + intIssueID + "</b>" + "<br>";

        //            ////	sentToTL.Priority = MailPriority.Normal;
        //            sentToTL.IsBodyHtml = true;




        //            // SmtpMai.SmtpServer = ConfigurationSettings.AppSettings["SMTPServerName"].ToString();
        //            //Followin Code is Commented by Amit Thakkar on 11/july/2007
        //            //SmtpMail.Send(sentToTL); 
        //        }

        //        //Following is the code to send the mail to employee whom issue has been asigned (AutoAssign)
        //        //MailMessage sentToHelpDeskMember = new MailMessage();
        //        //sentToHelpDeskMember.To.Add(new MailAddress(strMemberEmailID));
        //        //if (txtCCEmailID.Text != "")
        //        //{
        //        //    sentToHelpDeskMember.CC.Add(txtCCEmailID.Text);
        //        //}
        //        //sentToHelpDeskMember.From = new MailAddress(txtEmailID.Text);
        //        //sentToHelpDeskMember.Subject = "HelpDesk : New Issue Recorded.";
        //        //sentToHelpDeskMember.Body = "A new issue has been recorded with the following content:" + "<br>" +
        //        //    "Name: " + Server.HtmlEncode(txtName.Text) + "<br>" +
        //        //    "Phone Extension: " + txtPhoneExtension.Text + "<br>" +
        //        //    "Seating Location: " + txtSeatingLocation.Text + "<br>" +
        //        //    "ProblemSeverity: " + ddlSeverity.SelectedItem.Text + "<br>" +
        //        //  "Issue ID: " + intIssueID;
        //        //sentToHelpDeskMember.IsBodyHtml = true;




        //        //Old Code
        //        //Remove the Test part from the message sent to the Help Desk Member.
        //        // MailMessage sentToHelpDeskMember = new MailMessage();
        //        // sentToHelpDeskMember.To.Add(new MailAddress(strMemberEmailID));
        //        // if (txtCCEmailID.Text != "")
        //        // {
        //        //     sentToHelpDeskMember.CC.Add(txtCCEmailID.Text);
        //        // }
        //        //// sentToHelpDeskMember.CC.Add(new MailAddress(txtCCEmailID.Text));
        //        // sentToHelpDeskMember.From = new MailAddress(txtEmailID.Text);
        //        // //sentToHelpDeskMember.From = new MailAddress("v2system@in.v2solutions.com"); 

        //        // sentToHelpDeskMember.Subject = "HelpDesk : New Issue Recorded.";
        //        // //			sentToHelpDeskMember.Subject = "TEST";
        //        // //			sentToHelpDeskMember.Body = "This is a Test Mail. Please Ignore it."+"<br><br>"+
        //        // sentToHelpDeskMember.Body = "A new issue has been recorded with the following content:" + "<br>" +
        //        //     "Name: " + Server.HtmlEncode(txtName.Text) + "<br>" +
        //        //     "Phone Extension: " + txtPhoneExtension.Text + "<br>" +
        //        //     "Seating Location: " + txtSeatingLocation.Text + "<br>" +
        //        //     "ProblemSeverity: " + ddlSeverity.SelectedItem.Text + "<br>" +
        //        //     ////	"Priority: "+ddlPriority.SelectedItem.Value+"<br>"+
        //        //     "Description: " + Server.HtmlEncode(txtDescription.Value.ToString()) + "<br>" +
        //        //     //"Path: <a href='http://intranet/HelpDesk/admin/AssignTask.aspx?id='"+ intIssueID +">"+intIssueID+"</a> <br>" +
        //        //     //"Path: Path "+ intIssueID +""+intIssueID+" <br>" +
        //        //     //"Issue id: "+intIssueID;
        //        //     //"Issue ID: <a href= '"+Path="'"+ intIssueID +">"+intIssueID;
        //        //     "Issue ID: " + intIssueID;
        //        //// sentToHelpDeskMember.Priority = MailPriority.Normal;





        //        SmtpMail.UseDefaultCredentials = false;
        //        // SmtpMail.Credentials = new System.Net.NetworkCredential("v2system", "mail_123");
        //        SmtpMail.Credentials = new System.Net.NetworkCredential("username", "password");
        //        //sentToHelpDeskMember.IsBodyHtml = true;
        //        // Make sure you have appropriate replying permissions from your local system
        //        //SmtpMail.SmtpServer = "localhost";
        //        //SmtpMail.SmtpServer = ConfigurationSettings.AppSettings["SMTPServerName"].ToString();
        //        //sendMailToDept.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
        //        //sendMailToDept.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "v2system");
        //        //sendMailToDept.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "mail_123");

        //        //sentToHelpDeskMember.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
        //        //sentToHelpDeskMember.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "v2system");
        //        //sentToHelpDeskMember.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "mail_123");

        //        SmtpMail.Send(sendMailToDept);
        //        //SmtpMail.Send(sentToHelpDeskMember);
        //        //				lblMessage.CssClass
        //        resetAll();
        //        lblMessage.Visible = true;
        //        lblMessage.Text = "An email will be sent to you confirming the recording of the issue.";
        //        //Response.Write("Your Email has been sent sucessfully - Thank You");				
        //    }
        //    catch (V2Exceptions ex)
        //    {
        //        throw;
        //    }
        //    catch (System.Exception ex)
        //    {

        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "SendMail", ex.StackTrace);
        //        throw new V2Exceptions();
        //    }
        //}

        private void SendMail()
        {

            try
            {
                //To send mail to the Dept under which issue falls.
                clsReportIssue objClsReportIssue = new clsReportIssue();
                objClsReportIssue.SubCategoryID = Convert.ToInt32(ddlCategories.Items[ddlCategories.SelectedIndex].Value);
                string subCategoryName = ddlCategories.Items[ddlCategories.SelectedIndex].Text;
                string severity = ddlSeverity.Items[ddlSeverity.SelectedIndex].Text;
                string Category = GetCategoryName(objClsReportIssue).TrimStart(' ').TrimEnd(' ');
                SmtpClient SmtpMail = new SmtpClient(ConfigurationSettings.AppSettings["SMTPServerName"].ToString());
                if (Category == "")
                {
                    strDeptEmailID = ConfigurationSettings.AppSettings["IT_Dept_EmailID"].ToString();
                }
                else
                {
                    //Category= Category.ToUpper();
                    strDeptEmailID = ConfigurationSettings.AppSettings[Category].ToString();
                    string CCCateory = Category + "CC";
                    strDeptCCEmailID = ConfigurationSettings.AppSettings[CCCateory].ToString();
                }
                //string strDeptEmailID = ConfigurationSettings.AppSettings["IT_Dept_EmailID"].ToString();


                string strPath = ConfigurationSettings.AppSettings["Mail_Path"].ToString();
                string arrow = "&rarr;";

                MailMessage sendMailToDept = new MailMessage();
                //Loop to seperate email id's of CC peoples
                if (strDeptCCEmailID.Contains(","))
                {
                    string[] CCEmailId = strDeptCCEmailID.Split(',');
                    foreach (string email in CCEmailId)
                    {
                        sendMailToDept.To.Add(new MailAddress(email));
                    }


                }

                //If there is a manager in CC while logging an issue , then in mail CC part there will be user & manager 
                if (txtCCEmailID.Text != "")
                {
                    string User_ManagerEmail = txtEmailID.Text + "," + txtCCEmailID.Text;

                    if (User_ManagerEmail.Contains(","))
                    {
                        string[] CCEmailId = User_ManagerEmail.Split(',');
                        foreach (string email in CCEmailId)
                        {
                            sendMailToDept.CC.Add(new MailAddress(email));
                        }

                    }


                }

                //else only user will be there.
                else
                {
                    sendMailToDept.CC.Add(new MailAddress(txtEmailID.Text));
                }

                sendMailToDept.From = new MailAddress(txtEmailID.Text);
                sendMailToDept.Subject = "HelpDesk : Issue " + intIssueID + " under " + Category +
                                         " : " + subCategoryName + " has been logged.";

                sendMailToDept.Body = "Hi, " + "<br>" + "<br>" +
                    "This is to inform you that, a new issue has been logged. " + "<br>" + " Issue details are as follows :" + "<br>" + "<br>" +
                     "<b>" + "Issue ID : " + "</b>" + "<b>" + intIssueID + "</b>" + "<br>" + "<br>" +
                     "<b>" + "Department Name : " + "</b>" + Category + "<br>" + "<br>" +
                     "<b>" + "Category Name : " + "</b>" + subCategoryName + "<br>" + "<br>" +
                     "<b>" + "Issue Severity : " + "</b>" + severity + "<br>" + "<br>" +
                     "<b>" + "Current Status : " + "</b>" + "Open" + "<br>" + "<br>" +
                     "<b>" + "Description : " + "</b>" + Server.HtmlEncode(txtDescription.Value.ToString()) + "." + "<br>" + "<br>" +
                     "Please take the necessary action." + "<br>" + "<br>" + "<br>" +
                      "Regards," + "<br>" +
                      Server.HtmlEncode(txtName.Text.ToString());

                sendMailToDept.IsBodyHtml = true;

                SmtpMail.UseDefaultCredentials = false;
                SmtpMail.Credentials = new System.Net.NetworkCredential("username", "password");
                SmtpMail.Send(sendMailToDept);

                resetAll();
                lblMessage.Visible = true;
                lblMessage.Text = "An email will be sent to you confirming the recording of the issue.";
            }


            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                lblMailError.Text = ex.Message;
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "SendMail", ex.StackTrace);
                //throw new V2Exceptions();
            }
        }
        public string GetCategoryName(clsReportIssue objclsReportIssue)
        {
            try
            {

                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                return objClsBLReportIssue.GetCategoryName(objclsReportIssue);
            }
            catch (V2Exceptions ex)
            {
                throw;

            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "GetCategoryName", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        private void InsertIssueDetails()
        {
            try
            {

                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                clsReportIssue objClsReportIssue = new clsReportIssue();
                IssueStatus objIssueStatus = new IssueStatus();
                intIssueID = 0;
                objClsReportIssue.Name = txtName.Text.ToString();
                if (objClsReportIssue.Name == "")
                {
                    objClsReportIssue.Name = null;
                }
                objClsReportIssue.EmailID = txtEmailID.Text.ToString();
                objClsReportIssue.CCEmailID = txtCCEmailID.Text.ToString();
                objClsReportIssue.PhoneExtension = txtPhoneExtension.Text.ToString();
                objClsReportIssue.Type = Convert.ToInt32(ddltype.SelectedItem.Value);
                objClsReportIssue.SeatingLocation = txtSeatingLocation.Text.ToString();
                objClsReportIssue.SubCategoryID = Convert.ToInt32(ddlCategories.Items[ddlCategories.SelectedIndex].Value);
                objClsReportIssue.SeverityID = Convert.ToInt32(ddlSeverity.SelectedItem.Value);
                //objClsReportIssue.PriorityID = Convert.ToInt32(ddlPriority.SelectedItem.Value);
                objClsReportIssue.Description = txtDescription.Value.ToString();
                //objClsReportIssue.StatusID = Convert.ToInt32(IssueStatus.New);
                objClsReportIssue.StatusID = 1;
                if ((uploadFiles.PostedFile != null) && (uploadFiles.PostedFile.ContentLength > 0))
                {
                    fn = System.IO.Path.GetFileNameWithoutExtension(uploadFiles.PostedFile.FileName);
                    fnExt = System.IO.Path.GetExtension(uploadFiles.PostedFile.FileName);
                    objClsReportIssue.UploadedFileName = fn.ToString();
                    objClsReportIssue.UploadedFileExtension = fnExt.ToString();
                }
                IssueSubmitResult = objClsBLReportIssue.InsertIssueDetails(objClsReportIssue);

                if (IssueSubmitResult.Tables[0].Rows.Count > 0)
                {

                    intIssueID = Convert.ToInt32(IssueSubmitResult.Tables[0].Rows[0]["IssueId"].ToString());
                    lblMessage.Visible = true;
                    lblMessage.CssClass = "success";
                    lblMessage.Text = "Issue Successfully recorded.";
                    ShouldSendMail = 1;
                    ShouldUploadFile = 1;

                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.CssClass = "Error";
                    lblMessage.Text = "Error occured in saving the Issue ,Please try again";
                    ShouldSendMail = 0;
                    ShouldUploadFile = 0;
                }


                //if (dsIssueIDAndMemberEmailID.Tables[0].Rows.Count > 0)
                //{
                //    if (dsIssueIDAndMemberEmailID.Tables[0].Rows[0][0].ToString() == "0")
                //    {
                //        lblMessage.Visible = true;
                //        lblMessage.CssClass = "Error";
                //        lblMessage.Text = "This Category is not yet assigned to any of the members of HelpDesk.";
                //        ShouldSendMail = 0;
                //        ShouldUploadFile = 0;
                //    }
                //    else
                //    {
                //        lblMessage.Visible = true;
                //        lblMessage.CssClass = "success";
                //        lblMessage.Text = "Issue Successfully recorded.";
                //        ShouldSendMail = 1;
                //        ShouldUploadFile = 1;
                //        // To get the IssueID and Email ID of the helpdesk member to whom the issue is assigned; and mail is to be sent.
                //        intIssueID = Convert.ToInt32(dsIssueIDAndMemberEmailID.Tables[0].Rows[0]["ReportIssueID"]);
                //        strMemberEmailID = dsIssueIDAndMemberEmailID.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
                //        strAssignedTo = dsIssueIDAndMemberEmailID.Tables[0].Rows[0]["EmployeeName"].ToString();
                //    }
                //}
            }
            catch (V2Exceptions ex)
            {
                throw;

            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "InsertIssueDetails", ex.StackTrace);
                throw new V2Exceptions();
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
            ddlCategories.Attributes.Add("runat", "server");
            //ddlCategories.Attributes.Add("onchange", "DisplayCategorySummary();");

            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        private void ddlCategories_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                lblMessage.Text = "";
                CheckSelectedItem();
            }
            catch (V2Exceptions ex)
            {
                throw;

            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "ddlCategories_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        private void CheckSelectedItem()
        {
            /*if(ddlCategories.Items[ddlCategories.SelectedIndex].Text == "Admin" || ddlCategories.SelectedItem.Text == "IT" || ddlCategories.SelectedItem.Text == "HR" ||  ddlCategories.SelectedItem.Text == "--Select SubCategory--")
            {
                //lblDisplayCategoryError.Text = "You are not allowed to Select a Category. Please Select a SubCategory.";
                //lblDisplayCategoryError.Text = "";
            }*/
            //else lblDisplayCategoryError.Text = "";
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            try
            {
                resetAll();
            }
            catch (V2Exceptions ex)
            {
                throw;

            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "btnReset_Click", ex.StackTrace);
                throw new V2Exceptions();
            }
        }


        public void resetAll()
        {
            try
            {

                //txtName.Text = "";
                //txtEmailID.Text = "";
                txtCCEmailID.Text = "";
                txtPhoneExtension.Text = "";
                txtSeatingLocation.Text = "";
                BindSubCategory();
                //ddlCategories.SelectedIndex = 0;
                //// ddlPriority.SelectedIndex = 0;
                ddlSeverity.SelectedIndex = 0;
                ddltype.SelectedIndex = 0;
                txtDescription.Value = "";
                lblMessage.Visible = false;
            }
            catch (V2Exceptions ex)
            {
                throw;

            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "resetAll", ex.StackTrace);
                throw new V2Exceptions();
            }
        }


        public void UploadFiles()
        {
            try
            {
                if ((uploadFiles.PostedFile != null) && (uploadFiles.PostedFile.ContentLength > 0))
                {
                    //clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                    //clsReportIssue objClsReportIssue = new clsReportIssue();
                    //string fn = System.IO.Path.GetFileName(uploadFiles.PostedFile.FileName);


                    string SaveLocation = ConfigurationSettings.AppSettings["UploadedfilePath"].ToString();
                    //string SaveLocation = Server.MapPath("Uploads");
                    try
                    {

                        //  SaveLocation = SaveLocation.Replace("\\PublishHelpDesk\\", "\\HelpdeskAdmin\\");
                        uploadFiles.PostedFile.SaveAs(SaveLocation + "\\" + fn + "_" + intIssueID + fnExt);

                        lblMessage.Visible = true;
                        lblMessage.CssClass = "success";
                        lblMessage.Text = "The file has been uploaded.";
                    }

                    catch (V2Exceptions ex)
                    {
                        throw;

                    }
                    catch (System.Exception ex)
                    {
                        lblMessage.Visible = true;
                        lblMessage.CssClass = "success";
                        lblMessage.Text = "Error: " + ex.Message;

                        FileLog objFileLog = FileLog.GetLogger();
                        objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "UploadFiles", ex.StackTrace);
                        throw new V2Exceptions();

                    }

                }
                /*else
                {
                    //Response.Write("Please select a file to upload.");
                }*/
            }
            catch (V2Exceptions ex)
            {
                throw;

            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "UploadFiles", ex.StackTrace);
                throw new V2Exceptions();
            }
        }

        [WebMethod]
        public static string getDropdomdata(string stringParam)
        {
            DataSet dsGetData = new DataSet();
            string val = "";

            clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
            clsReportIssue objClsReportIssue = new clsReportIssue();
            var categoryissue = stringParam;
            objClsReportIssue.SubCategoryID = Convert.ToInt32(categoryissue);
            dsGetData = objClsBLReportIssue.getCategorySummary(objClsReportIssue);
            if (dsGetData.Tables[0].Rows.Count > 0)
            {
                val = dsGetData.Tables[0].Rows[0]["IssueSummary"].ToString();
            }
            else
            {
                val = "";
            }
            return val;
        }

    }
}
