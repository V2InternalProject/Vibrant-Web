using System;
using System.Data;
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
using System.Web.Mail;
//using System.Net.Mail;
using System.Text.RegularExpressions;
using V2.Orbit.BusinessLayer;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Orbit.Workflow.LeaveUploadWF
{
    public sealed partial class LeaveUpLoadWF : StateMachineWorkflowActivity
    {
        public LeaveUpLoadWF()
        {
            InitializeComponent();
        }

        private void caUploadandSendingMail_ExecuteCode(object sender, EventArgs e)
        {
             AbsentListBOL objAbsentListBOL=new  AbsentListBOL();
            string strFrom="",SMTPServer="";
            DateTime dtLastUsed=DateTime.Now;
            try
            {
                DataSet dsAbsentListBOL=  objAbsentListBOL.GetLeaveUploadEmployeeList();

                for(int k=0;k<dsAbsentListBOL.Tables[0].Rows.Count;k++)
                   {
                       if(dsAbsentListBOL.Tables[0].Rows[k]["ConfigItemName"].ToString()=="From EmailID")
                       {
                           
                           strFrom=dsAbsentListBOL.Tables[0].Rows[k]["ConfigItemValue"].ToString();
                           
                       }
                       if(dsAbsentListBOL.Tables[0].Rows[k]["ConfigItemName"].ToString()=="Lock Upload Date")
                       {
                           dtLastUsed= Convert.ToDateTime(dsAbsentListBOL.Tables[0].Rows[k]["ConfigItemValue"].ToString());
                          
                         
                       }
                       if(dsAbsentListBOL.Tables[0].Rows[k]["ConfigItemName"].ToString()=="Mail Server Name")
                       {
                          SMTPServer= dsAbsentListBOL.Tables[0].Rows[k]["ConfigItemValue"].ToString();
                          
                         
                       }
                   }

               if(dsAbsentListBOL.Tables.Count>1)
               {
                   for(int j=0;j<dsAbsentListBOL.Tables[1].Rows.Count;j++)
                {
                  if(dsAbsentListBOL.Tables[1].Rows[j]["EmailTemplateName"].ToString()=="Leave Upload")
                  {
                     for(int i=0;i<dsAbsentListBOL.Tables[3].Rows.Count;i++)
                     {    
                        MailMessage objMailMessage = new MailMessage();

                             objMailMessage.From =strFrom;           //new MailAddress(strFrom) ;
                             objMailMessage.To=  dsAbsentListBOL.Tables[3].Rows[i]["EmployeeEmailID"].ToString();   //.Add(new MailAddress(dsAbsentListBOL.Tables[0].Rows[i]["EmployeeEmailID"].ToString()));
                             objMailMessage.Subject =dsAbsentListBOL.Tables[1].Rows[j]["EmailSubject"].ToString() ;
                         objMailMessage.Body =dsAbsentListBOL.Tables[1].Rows[j]["EmailBody"].ToString() ;
                             objMailMessage.Subject=Regex.Replace(objMailMessage.Subject,"##Month##",dsAbsentListBOL.Tables[2].Rows[0]["Month"].ToString());
                             objMailMessage.Subject=Regex.Replace(objMailMessage.Subject,"##Year##",dsAbsentListBOL.Tables[2].Rows[0]["Year"].ToString());
                             objMailMessage.Body=Regex.Replace(objMailMessage.Body," ##EmployeeName##",dsAbsentListBOL.Tables[3].Rows[i]["EmployeeName"].ToString());
                            
                             objMailMessage.Body=Regex.Replace(objMailMessage.Body,"##Month##",dsAbsentListBOL.Tables[2].Rows[0]["Month"].ToString());
                             objMailMessage.Body=Regex.Replace(objMailMessage.Body,"##Year##",dsAbsentListBOL.Tables[2].Rows[0]["Year"].ToString());

                        objMailMessage.BodyFormat=MailFormat.Html;
                                   
                             //SmtpClient sc=new SmtpClient();
                             //sc.Host=SMTPServer; 
                             // sc.Send(objMailMessage);
                           objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "0");    
                            objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "v2system"); 
                            objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "mail_123");
                           SmtpMail.SmtpServer=SMTPServer;     //System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString();
                             SmtpMail.Send(objMailMessage);

                 
                 
                    }

                }
             
            }

                   

               }

                     dtLastUsed=dtLastUsed.AddMonths(1);
                     int day=dtLastUsed.Day;
                     dtLastUsed=dtLastUsed.AddDays(-(day-1));
                    string  strdate=dtLastUsed.ToShortDateString() + " 00:00:00";
                    dtLastUsed=Convert.ToDateTime(strdate);
                     TimeSpan tsDelay=dtLastUsed.Subtract(DateTime.Now);
                    daTimeOut.TimeoutDuration=tsDelay;
            }
            catch(V2Exceptions ex)
            {
                throw ;
            }

            catch(System.Exception ex)
            {
                
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveUpLoadWF.cs", "caUploadandSendingMail_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }



            #region Not Required Code

            //catch(System.Data.SqlClient.SqlException ex)
            //{
            //    FileLog objFileLog = FileLog.GetLogger();
            //    objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveUpLoadWF.cs", "caUploadandSendingMail_ExecuteCode", ex.StackTrace);
            //    throw new V2Exceptions(ex.ToString(),ex);


            //}

           //for(int k=0;k<dsAbsentListBOL.Tables[3].Rows.Count;k++)
           //{
           //    if(dsAbsentListBOL.Tables[3].Rows[k]["ConfigItemName"].ToString()=="Lock Upload Date")
           //    {
           //        DateTime dtLock=Convert.ToDateTime( dsAbsentListBOL.Tables[3].Rows[k]["ConfigItemValue"].ToString());
                   
           //        if(dtLock.Month==DateTime.Now.Month)
           //        {
           //            booLock=true;
           //            break;
           //        }
                   
           //    }
           //}

             #endregion

            

            
         
        }
    }
}
