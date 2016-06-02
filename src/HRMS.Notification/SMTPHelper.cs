using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace HRMS.Notification
{
    public class SMTPHelper
    {
        // C:\Windows\Microsoft.NET\Framework\v4.0.30319>aspnet_regiis -ga v2solutions\rpostuser
        // Start granting v2solutions\rpostuser access to the IIS configuration and other d
        // irectories used by ASP.NET.
        // Finished granting v2solutions\rpostuser access to the IIS configuration and othe
        // directories used by ASP.NET.

        /// <summary>
        /// Gets the log file name and location
        /// </summary>
        private string LogFileName
        {
            get
            {
                if (ConfigurationManager.AppSettings["LogFileName"] == null)
                    return string.Empty;
                else
                    return ConfigurationManager.AppSettings["LogFileName"].ToString();
            }
        }

        /// <summary>
        /// SMTPHelper Constructor to help create new instance
        /// </summary>
        public SMTPHelper()
        {
        }

        /// <summary>
        /// send mail with mail message as parameter
        /// </summary>
        /// <param name="mailMessage">MailMessage instance</param>
        public void SendMail(MailMessage mailMessage)
        {
            SmtpClient smtpClient = new SmtpClient();
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                this.AddAnEntryToLogFile("SMTPHelper.SendMail()", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        public void SendMail(string from, string to, string subject, string messageBody, string cc, string bcc, int isOrbitmail = 0)
        {
            SendMail(new string[] { to }, new string[] { to }, new string[] { cc }, new string[] { cc }, new string[] { bcc }, new string[] { bcc }, from, null, subject, messageBody, null, null,isOrbitmail);
           
        }

        /// <summary>
        /// send email message
        /// </summary>
        /// <param name="toAddress">To recipient list</param>
        /// <param name="toDisplayName">To recipient list display name</param>
        /// <param name="ccAddress">CC recipient list</param>
        /// <param name="ccDisplayName">CC recipient list display name</param>
        /// <param name="bccAddress">BCC recipient list</param>
        /// <param name="bccDisplayName">BCC recipient list display name</param>
        /// <param name="fromAddress">sender address</param>
        /// <param name="fromDisplayName">sender display name</param>
        /// <param name="subject">mail subject</param>
        /// <param name="messageBody">message body</param>
        /// <param name="attachments">list of attachements</param>
        /// <param name="fileName">list of file names</param>
        /// <returns>true, if mail sent successfully, false otherwise</returns>
        public bool SendMail(string[] toAddress, string[] toDisplayName,
                                    string[] ccAddress, string[] ccDisplayName,
                                    string[] bccAddress, string[] bccDisplayName,
                                    string fromAddress, string fromDisplayName,
                                    string subject, string messageBody,
                                    List<byte[]> attachments, List<string> fileName, int isOrbitmail = 0)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage mailMessage = new MailMessage();
            try
            {
                smtpClient.UseDefaultCredentials = false;

                smtpClient.Host = System.Configuration.ConfigurationManager.AppSettings["SMTPServerName"].ToString();
                //smtpClient.Host = "v2mailserver.in.v2solutions.com";
                string UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"].ToString();
                string Password = System.Configuration.ConfigurationManager.AppSettings["Password"].ToString();
                smtpClient.Credentials = new System.Net.NetworkCredential(UserName, Password);
                smtpClient.Port = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["PortNumber"].ToString());
                //  smtpClient.EnableSsl = true;

                //smtpClient.Credentials.GetCredential(smtpClient.Host, smtpClient.Port,  

                //Assign recipients
                if (toAddress != null && toAddress.Length > 0)
                {
                    for (int i = 0; i < toAddress.Length; i++)
                    {
                        string emailAddress = string.Empty;
                        string emailDisplayName = string.Empty;

                        if (toAddress[i] != null && toAddress[i].Trim() != string.Empty)
                            emailAddress = toAddress[i].Trim();
                        if (toDisplayName != null)
                            if (toDisplayName[i] != null && toDisplayName[i].Trim() != string.Empty)
                                emailDisplayName = toDisplayName[i].Trim();

                        if (emailAddress != string.Empty)
                        {
                            MailAddress mailAddress = new MailAddress(emailAddress, emailDisplayName);
                            mailMessage.To.Add(mailAddress);
                        }
                    }
                }
                else
                    throw new Exception("Recipients address is not specified.");

                //Assign CC
                if (ccAddress != null && ccAddress.Length > 0)
                {
                    for (int i = 0; i < ccAddress.Length; i++)
                    {
                        string emailAddress = string.Empty;
                        string emailDisplayName = string.Empty;

                        if (ccAddress[i] != null && ccAddress[i].Trim() != string.Empty)
                            emailAddress = ccAddress[i].Trim();
                        if (ccDisplayName != null)
                            if (ccDisplayName[i] != null && ccDisplayName[i].Trim() != string.Empty)
                                emailDisplayName = ccDisplayName[i].Trim();

                        if (emailDisplayName != null && emailDisplayName != "")
                        {
                            MailAddress mailAddress = new MailAddress(emailAddress, emailDisplayName);
                            mailMessage.CC.Add(mailAddress);
                        }
                        else
                        {
                            if (emailAddress != null && emailAddress != "")
                            {
                                MailAddress mailAddress = new MailAddress(emailAddress);
                                mailMessage.CC.Add(mailAddress);
                            }
                        }
                    }
                }

               

                //Assign From
                if (fromAddress != null && fromAddress != string.Empty)
                {
                    MailAddress fromMailAddress = new MailAddress(fromAddress, fromAddress);
                    mailMessage.From = fromMailAddress;
                }
                else
                    throw new Exception("From address is not specified.");

                mailMessage.Subject = subject;
                mailMessage.Body = messageBody;
                if(isOrbitmail == 1)
                   mailMessage.IsBodyHtml = true;

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (attachments != null)
                {
                    Attachment attachment = null;
                    for (int counter = 0; counter < attachments.Count; counter++)
                    {
                        attachment = new Attachment(new MemoryStream(attachments[counter]), fileName[counter]);
                        mailMessage.Attachments.Add(attachment);
                    }
                }
                smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSLRequiredForEmail"].ToString());
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (SmtpFailedRecipientException e)
            {
                throw new SmtpFailedRecipientException(e.FailedRecipient);
            }
            catch (Exception ex)
            {
                this.AddAnEntryToLogFile("SMTPHelper.SendMail()", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Add an entry to log file
        /// </summary>
        /// <param name="functionName">function name</param>
        /// <param name="message">message to log</param>
        public void AddAnEntryToLogFile(string functionName, string message)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                string logMessage = dateTime.ToString() + ", [" + functionName + "], " +
                    System.Environment.NewLine +
                    "======================================================================" +
                    System.Environment.NewLine +
                    message +
                    System.Environment.NewLine + System.Environment.NewLine;
                //string logPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.System), LogFileName);
                StreamWriter sw = new StreamWriter(this.LogFileName, true, Encoding.ASCII);

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
                EventLog.WriteEntry("ManageEnvelopeDocumentService", message + ex.ToString(), EventLogEntryType.Error);
            }
        }
    }
}
