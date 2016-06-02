using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOL;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Configuration;

namespace MailActivity
{
    public class EmailActivity
    {
        public void SendMail(EmailActivityBOL objEmailActivityBOL)
        {

            for (int i = 0; i < objEmailActivityBOL.ToAddress.Length; i++)
            {
                if (objEmailActivityBOL.ToAddress[i] != "")
                {
                    String body = string.Empty;
                    int pos = objEmailActivityBOL.ToAddress[i].IndexOf(".");
                    string name = objEmailActivityBOL.ToAddress[i].Substring(0, pos);
                    name = name.ToUpper();
                    //if mail not raised by RRF Requestor
                    if (objEmailActivityBOL.Subject != "Aprrove RRF")
                        body = objEmailActivityBOL.Body.Replace("##EmployeeName##", name);

                    MailMessage objMailMessage = new MailMessage();
                    objMailMessage.From = new MailAddress(objEmailActivityBOL.FromAddress);
                    objMailMessage.To.Add(new MailAddress(objEmailActivityBOL.ToAddress[i]));
                    objMailMessage.Subject = objEmailActivityBOL.Subject;

                    //objMailMessage.IsBodyHtml = true;
                    //for (int k = 0; k < objEmailActivityBOL.CCAddress.Length; k++)
                    //    objMailMessage.CC.Add(new MailAddress(objEmailActivityBOL.CCAddress[k]));
                    SmtpClient smtpClient = new SmtpClient(ConfigurationSettings.AppSettings["SMTPServerName"].ToString());
                    smtpClient.UseDefaultCredentials = false;
                    //SmtpClient smtpClient = new SmtpClient("webmail.in.v2solutions.com");

                    if (!objEmailActivityBOL.ToAddress[i].ToString().Contains("@v2solutions.com"))
                    {
                        //objMailMessage.From = new MailAddress("v2system@in.v2solutions.com");
                        objMailMessage.From = new MailAddress(ConfigurationSettings.AppSettings["MailAddress"].ToString());
                        body = body.Replace("##RRFNO##", objEmailActivityBOL.RRFNo.ToString());
                        body = body.Replace("##skills##", objEmailActivityBOL.skills.ToString());
                        body = body.Replace("##designation##", objEmailActivityBOL.Position.ToString());
                        body = body.Replace("##EmployeeName##", objEmailActivityBOL.CandidateName);
                        smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), ConfigurationSettings.AppSettings["SMTPPassword"].ToString());//("v2system", "mail_123");
                    }
                    else
                    {
                        objMailMessage.From = new MailAddress(objEmailActivityBOL.FromAddress);
                        body = objEmailActivityBOL.Body.Replace("##RRFNO##", objEmailActivityBOL.RRFNo.ToString());

                        body = body.Replace("##skills##", objEmailActivityBOL.skills.ToString());
                        body = body.Replace("##designation##", objEmailActivityBOL.Position.ToString());
                        body = body.Replace("##Candidate name##", objEmailActivityBOL.CandidateName);
                        if (objEmailActivityBOL.EmailTemplateName != "Accept RRF")
                        {
                            for (int k = 0; k < objEmailActivityBOL.CCAddress.Length; k++)
                                objMailMessage.CC.Add(new MailAddress(objEmailActivityBOL.CCAddress[k]));
                        }
                        smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), ConfigurationSettings.AppSettings["SMTPPassword"].ToString());//("v2system", "mail_123"); 
                    }
                    objMailMessage.IsBodyHtml = true;
                    objMailMessage.Body = body;
                    smtpClient.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["PortNumber"].ToString());
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(objMailMessage);
                }
            }
        }
    }
}