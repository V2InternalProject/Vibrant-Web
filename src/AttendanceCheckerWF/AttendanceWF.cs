using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using System.Web.Mail;
//using System.Net.Mail;
using System.Text.RegularExpressions;

namespace AttendanceCheckerWF
{
    public sealed partial class AttendanceWF : StateMachineWorkflowActivity
    {
        public string To,From,Body,Subject,SMTPServer;
        string strFrom="";
        string strDelayTime="";
            
        public AttendanceWF()
        {
            InitializeComponent();
        }

       

        private void caCheckAttendance_ExecuteCode(object sender, EventArgs e)
        {
            AbsentListBOL objAbsentListBOL = new AbsentListBOL();
            //string strFrom="";
            //string strDelayTime="";
            //string SMTPServer="";
            try
            {
                DataSet dsAbsentListBOL= objAbsentListBOL.GetAbsentList();



                if(dsAbsentListBOL!=null)
                {

                    
                    for(int k=0;k<dsAbsentListBOL.Tables[3].Rows.Count;k++)
                   {
                        if(dsAbsentListBOL.Tables[3].Rows[k]["ConfigItemName"].ToString()=="Check Attendance Time")
                        {
                            strDelayTime=dsAbsentListBOL.Tables[3].Rows[k]["ConfigItemValue"].ToString();
                        }
                       if(dsAbsentListBOL.Tables[3].Rows[k]["ConfigItemName"].ToString()=="From EmailID")
                       {
                           strFrom=dsAbsentListBOL.Tables[3].Rows[k]["ConfigItemValue"].ToString();
                          
                         
                       }
                        if(dsAbsentListBOL.Tables[3].Rows[k]["ConfigItemName"].ToString()=="Mail Server Name")
                       {
                           SMTPServer=dsAbsentListBOL.Tables[3].Rows[k]["ConfigItemValue"].ToString();
                          
                         
                       }
                   }

                    bool IsHoliday=false;
                        for(int j=0;j<dsAbsentListBOL.Tables[2].Rows.Count;j++)
                        {
                            DateTime dtHoliday=Convert.ToDateTime(dsAbsentListBOL.Tables[2].Rows[j]["HolidayDate"].ToString());
                            if(dtHoliday.Date.ToShortDateString() ==DateTime.Now.ToShortDateString())
                            {
                                IsHoliday=true;
                                break;
                            }

                        }
                    if(!IsHoliday)
                    {
                        if (DateTime.Now.DayOfWeek.ToString() != "Saturday" && DateTime.Now.DayOfWeek.ToString() != "Sunday")
                        {
                            for(int i=0;i<dsAbsentListBOL.Tables[0].Rows.Count;i++)
                            {
                                //DateTime dtFrom=Convert.ToDateTime(dsAbsentListBOL.Tables[0].Rows[i]["SignInDate"].ToString());
                      
                          
                          
                              MailMessage objMailMessage = new MailMessage();   
                                objMailMessage.BodyFormat=MailFormat.Html;
                                objMailMessage.From =strFrom;                  //"pmo@in.v2solutions.com"
                                objMailMessage.To= dsAbsentListBOL.Tables[0].Rows[i]["EmployeeEmailID"].ToString();  //objMailMessage.To="chandrashekar.v@in.v2solutions.com";
                                objMailMessage.Subject =dsAbsentListBOL.Tables[1].Rows[0]["EmailSubject"].ToString() ;
                                
                                string strBody = dsAbsentListBOL.Tables[1].Rows[0]["EmailBody"].ToString();
                                strBody=Regex.Replace(strBody,"##EmployeeName##",dsAbsentListBOL.Tables[0].Rows[i]["EmployeeName"].ToString());
                                strBody=Regex.Replace(strBody,"##Environment##",Environment.NewLine);
                                objMailMessage.Body=Regex.Replace(strBody,"##SignInDate##",dsAbsentListBOL.Tables[0].Rows[i]["SignInDate"].ToString());
                                    
                              //SmtpClient sc=new SmtpClient();
                             // sc.Host=SMTPServer;
                              //sc.Send(objMailMessage);
              
                                  objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");    
            objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "v2system"); 
            objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "mail_123");

                               SmtpMail.SmtpServer=SMTPServer;     //System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString();
                                SmtpMail.Send(objMailMessage);
                          

                            }
                   
                                        

                        }

                    }
                    
                    
                    
                    
                    string strdate="";//=DateTime.Now.ToShortDateString() +" " +strDelayTime+":00"
                    
                    DateTime dttemp=DateTime.Now;
                    dttemp=dttemp.AddDays(1);
                    strdate=dttemp.ToShortDateString() + " " + strDelayTime+":00";
                    dttemp=Convert.ToDateTime(strdate);
                    TimeSpan tsDelay= dttemp.Subtract(DateTime.Now);

                     daCheckAttendance.TimeoutDuration=tsDelay;
                  
                    
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AttendanceWF.cs", "caCheckAttendance_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
           


        }

        private void caFHSendMailFromIsaAttendance(object sender, EventArgs e)
        {
                To="pmo@in.v2solutions.com";     //strFrom;
                From="pmo@in.v2solutions.com";
                Subject="Check Attendance Process Raised Exception";
                Body="Hi PMO," +Environment.NewLine + "Check Attendance Process Raised Exception and Stopped Please Restart the Process and Contact Intranet Development team to solve the issue if any.";
                //caCheckAttendance

           

        }

        private void caFHCheckAttendence_ExecuteCode(object sender, EventArgs e)
        {
           
                To="pmo@in.v2solutions.com";     //strFrom;
                From="pmo@in.v2solutions.com";
                Subject="Check Attendance Process Raised Exception";
                Body="Hi PMO," +Environment.NewLine + "Check Attendance Process Raised Exception and Stopped Please Restart the Process and Contact Intranet Development team to solve the issue if any.";
                

            

        }

        private void cafhDelay_ExecuteCode(object sender, EventArgs e)
        {
            
                To="pmo@in.v2solutions.com";     //strFrom;
                From="pmo@in.v2solutions.com";
                Subject="Check Attendance Process Raised Exception";
                Body="Hi PMO," +Environment.NewLine + "Check Attendance Process Raised Exception and Stopped Please Restart the Process and Contact Intranet Development team to solve the issue if any.";
                

           

        }

        private void cafhOnDemand_ExecuteCode(object sender, EventArgs e)
        {
            
            
                To="pmo@in.v2solutions.com";     //strFrom;
                From="pmo@in.v2solutions.com";
                Subject="Check Attendance Process Raised Exception";
                Body="Hi PMO," +Environment.NewLine + "Check Attendance Process Raised Exception and Stopped Please Restart the Process and Contact Intranet Development team to solve the issue if any.";
                

            

        }

        private void cafhOnStop_ExecuteCode(object sender, EventArgs e)
        {
            
               
                To="pmo@in.v2solutions.com";     //strFrom;
                From="pmo@in.v2solutions.com";
                Subject="Check Attendance Process Raised Exception";
                Body="Hi PMO," +Environment.NewLine + "Check Attendance Process Raised Exception and Stopped Please Restart the Process and Contact Intranet Development team to solve the issue if any.";
                

            
        }

        

        
    }
}
