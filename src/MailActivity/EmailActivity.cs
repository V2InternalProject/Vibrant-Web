using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using System.Net.Mail;

namespace V2.Orbit.Workflow.Activities.MailActivity
{
    [ToolboxItemAttribute(typeof(ActivityToolboxItem))]
    public partial class EmailActivity : System.Workflow.ComponentModel.Activity
    {
        public EmailActivity()
        {
            InitializeComponent();
        }
        public static DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(System.String), typeof(EmailActivity));
        [DescriptionAttribute("Please specify the email address of the receipent ")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        public string To
        {
            get
            {
                return ((String)(base.GetValue(EmailActivity.ToProperty)));
            }
            set
            {
                base.SetValue(EmailActivity.ToProperty, value);
            }
        }

        public static DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(System.String), typeof(EmailActivity));

        [DescriptionAttribute("Please specify the email address of the sender")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        public string From
        {
            get
            {
                return ((String)(base.GetValue(EmailActivity.FromProperty)));
            }
            set
            {
                base.SetValue(EmailActivity.FromProperty, value);
            }
        }
        public static DependencyProperty CcProperty = DependencyProperty.Register("Cc", typeof(System.String), typeof(EmailActivity));
        [DescriptionAttribute("Please specify the email address of the (CC) receipent ")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        public string Cc
        {
            get
            {
                return ((String)(base.GetValue(EmailActivity.CcProperty)));
            }
            set
            {
                base.SetValue(EmailActivity.CcProperty, value);
            }
        }

        public static DependencyProperty SubjectProperty = DependencyProperty.Register("Subject", typeof(System.String), typeof(EmailActivity));
        [DescriptionAttribute("Please specify the Subject for the receipent ")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        public string Subject
        {
            get
            {
                return ((String)(base.GetValue(EmailActivity.SubjectProperty)));
            }
            set
            {
                base.SetValue(EmailActivity.SubjectProperty, value);
            }
        }

        public static DependencyProperty BodyProperty = DependencyProperty.Register("Body", typeof(System.String), typeof(EmailActivity));
        [DescriptionAttribute("Please specify the Body for the receipent ")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        public string Body
        {
            get
            {
                return ((String)(base.GetValue(EmailActivity.BodyProperty)));
            }
            set
            {
                base.SetValue(EmailActivity.BodyProperty, value);
            }
        }
        public static DependencyProperty SMTPServerProperty = DependencyProperty.Register("SMTPServer", typeof(System.String), typeof(EmailActivity));
        [DescriptionAttribute("Please specify the SMTP Server IP for the receipent ")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        public string SMTPServer
        {
            get
            {
                return ((String)(base.GetValue(EmailActivity.SMTPServerProperty)));
            }
            set
            {
                base.SetValue(EmailActivity.SMTPServerProperty, value);
            }
        }




        protected override ActivityExecutionStatus Execute(ActivityExecutionContext context)
        {
            try
            {
                //MailMessage objMailMessage = new MailMessage();

                SmtpClient smtpClient = new SmtpClient();
                MailMessage objMailMessage = new MailMessage();


                objMailMessage.From = new MailAddress(From, From);
                objMailMessage.To.Add(new MailAddress(To));
                objMailMessage.CC.Add(new MailAddress(Cc));
                objMailMessage.Subject = Subject;
                objMailMessage.IsBodyHtml = true;
                objMailMessage.Body = Body;

                smtpClient.Host = System.Configuration.ConfigurationManager.AppSettings["SMTPServerName"].ToString();
                string UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"].ToString();
                string Password = System.Configuration.ConfigurationManager.AppSettings["Password"].ToString();
                smtpClient.Port = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["PortNumber"].ToString());
                smtpClient.EnableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsSSLRequiredForEmail"]);
                smtpClient.Credentials = new System.Net.NetworkCredential(UserName, Password);
                //SmtpClient sc=new SmtpClient();
                //    sc.Host=SMTPServer;
                //     sc.Send(objMailMessage);



                //objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "0");    
                //objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "v2system"); 
                //objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "mail_123");

                smtpClient.Send(objMailMessage);
                // SmtpMail.Send(objMailMessage);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }

            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "EmailActivity.cs", "ActivityExecutionStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }



            return ActivityExecutionStatus.Closed;
        }

    }








}
