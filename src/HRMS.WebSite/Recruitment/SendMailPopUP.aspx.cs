using BLL;
using BOL;
using System;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Web.UI;

public partial class SendMailPopUP : System.Web.UI.Page
{
    private DataSet EmployeeInfo = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblNote.Text = "The valid separators for the Email IDs are space(' '), comma(',') and semi-colon(';').";
            txtSubject.Text = "New Employee Code : " + Convert.ToString(Session["EmployeeCode"]);

            GetEmailAdress();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SmtpClient SmtpMail = new SmtpClient(ConfigurationSettings.AppSettings["SMTPServerName"].ToString());

            MailMessage sendMail = new MailMessage();
            //Loop to seperate email id's of CC peoples

            if (txtTo.Text != "")
            {
                if (txtTo.Text.Contains(","))
                {
                    string[] ToEmailId = txtTo.Text.Split(',');
                    foreach (string email in ToEmailId)
                    {
                        if (Convert.ToString(email) != string.Empty)
                            sendMail.To.Add(new MailAddress(email));
                    }
                }
                if (txtTo.Text.Contains(" "))
                {
                    string[] ToEmailId = txtTo.Text.Split(' ');
                    foreach (string email in ToEmailId)
                    {
                        if (Convert.ToString(email) != string.Empty)
                            sendMail.To.Add(new MailAddress(email));
                    }
                }
                if (txtTo.Text.Contains(";"))
                {
                    string[] ToEmailId = txtTo.Text.Split(';');
                    foreach (string email in ToEmailId)
                    {
                        if (Convert.ToString(email) != string.Empty)
                            sendMail.To.Add(new MailAddress(email));
                    }
                }

                if (!txtTo.Text.Contains(";") && (!txtTo.Text.Contains(" ")) && (!txtTo.Text.Contains(",")))
                {
                    //if (txtTo.Text.Contains(";"))
                    //   txtTo.Text = txtTo.Text.Replace(";","");
                    //if (txtTo.Text.Contains(" "))
                    //  txtTo.Text =  txtTo.Text.Trim();
                    //if (txtTo.Text.Contains(","))
                    //   txtTo.Text = txtTo.Text.Replace(",", "");
                    sendMail.To.Add(new MailAddress(txtTo.Text));
                    //throw new FormatException();
                }
            }

            //If there is a manager in CC while logging an issue , then in mail CC part there will be user & manager
            if (txtCC.Text != "")
            {
                if (txtCC.Text.Contains(","))
                {
                    string[] CCEmailId = txtCC.Text.Split(',');
                    foreach (string email in CCEmailId)
                    {
                        if (Convert.ToString(email) != string.Empty)
                            sendMail.CC.Add(new MailAddress(email));
                    }
                }
                if (txtCC.Text.Contains(" "))
                {
                    string[] CCEmailId = txtCC.Text.Split(' ');
                    foreach (string email in CCEmailId)
                    {
                        if (Convert.ToString(email) != string.Empty)
                            sendMail.CC.Add(new MailAddress(email));
                    }
                }
                if (txtCC.Text.Contains(";"))
                {
                    string[] CCEmailId = txtCC.Text.Split(';');
                    foreach (string email in CCEmailId)
                    {
                        if (Convert.ToString(email) != string.Empty)
                            sendMail.CC.Add(new MailAddress(email));
                    }
                }

                if (!txtCC.Text.Contains(";") && (!txtCC.Text.Contains(" ")) && (!txtCC.Text.Contains(",")))
                {
                    //if (txtCC.Text.Contains(";"))
                    //    txtCC.Text = txtCC.Text.Replace(";", "");
                    //if (txtCC.Text.Contains(" "))
                    //    txtCC.Text = txtCC.Text.Trim();
                    //if (txtCC.Text.Contains(","))
                    //    txtCC.Text = txtCC.Text.Replace(",", "");
                    sendMail.CC.Add(new MailAddress(txtCC.Text));
                    //throw new FormatException();
                }
            }

            //else only user will be there.
            //Changed by Rahul for issue ID #142667129.
            sendMail.From = new MailAddress(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), txtFrom.Text);
            sendMail.Subject = "New Employeecode " + Convert.ToString(Session["EmployeeCode"]);

            sendMail.Body = txtMessage.Text;
            sendMail.Body.Replace("<br>", Environment.NewLine);

            //sendMail.IsBodyHtml = true;

            SmtpMail.UseDefaultCredentials = Convert.ToBoolean(ConfigurationSettings.AppSettings["IsSSLRequiredForEmail"]);
            SmtpMail.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), ConfigurationSettings.AppSettings["SMTPPassword"].ToString());
            SmtpMail.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["PortNumber"].ToString());
            SmtpMail.EnableSsl = true;
            SmtpMail.Send(sendMail);
            lblError.Visible = true;
            lblError.Text = "";
            lblError.ForeColor = System.Drawing.Color.Green;
            lblError.Text = "Mail send successfully";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.close();", true);
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = "Invalid Email ID. Mail could not be send.";
            //lblError.Text = "There is some error in sending mail. Mail could not be send due to \r\n"+ex.Message;
        }
    }

    private void GetEmailAdress()
    {
        UtilityBLL objUtilityBLL = new UtilityBLL();
        UtilityBOL objUtilityBOL = new UtilityBOL();

        objUtilityBOL.UserID = Convert.ToInt32(User.Identity.Name);
        EmployeeInfo = objUtilityBLL.GetEmployeeInfo(objUtilityBOL);
        txtFrom.Text = Convert.ToString(EmployeeInfo.Tables[0].Rows[0]["EmailId"]);
        txtCC.Text = Convert.ToString(EmployeeInfo.Tables[0].Rows[0]["EmailId"]);
        DataTable tableEmployeeInfo = EmployeeInfo.Tables[1]; // Get the data table.
        foreach (DataRow row in EmployeeInfo.Tables[1].Rows) // Loop over the rows.
        {
            if (Convert.ToString(row["EmailId"]) != string.Empty)
                txtTo.Text = txtTo.Text + Convert.ToString(row["EmailId"]) + ";";
        }

        txtMessage.Text = Server.HtmlDecode("Hello, " + "<br>" + "<br>" +
           "This is to notify that, a new employee code has been generated in Vibrantweb as " + Convert.ToString(Session["EmployeeCode"]) + "<br>" + "<br>" + "Regards," + "<br>" +
             Convert.ToString(EmployeeInfo.Tables[0].Rows[0]["EmployeeName"]));
        txtMessage.Text = txtMessage.Text.Replace("<br>", Environment.NewLine);
    }
}