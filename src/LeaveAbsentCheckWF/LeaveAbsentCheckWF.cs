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
using V2.Orbit.BusinessLayer;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using System.Web.Mail;
using System.Text.RegularExpressions;

namespace V2.Orbit.Workflow.LeaveAbsentCheckWF
{
    public sealed partial class LeaveAbsentCheckWF : StateMachineWorkflowActivity
    {
        public LeaveAbsentCheckWF()
        {
            InitializeComponent();
        }

        private void caCheckandSendMail_ExecuteCode(object sender, EventArgs e)
        {

            AbsentListBOL objAbsentListBOL = new AbsentListBOL();
            try
            {
                DataSet dsAbsentListBOL= objAbsentListBOL.GetAbsentList();

                if(dsAbsentListBOL!=null)
                {
                    
                    for(int i=0;i<dsAbsentListBOL.Tables[0].Rows.Count;i++)
                    {
                        DateTime dtFrom=Convert.ToDateTime(dsAbsentListBOL.Tables[0].Rows[i]["SignInDate"].ToString());
                      if (dtFrom.DayOfWeek.ToString() != "Saturday" && dtFrom.DayOfWeek.ToString() != "Sunday")
                      {
                          bool IsHoliday=false;
                        for(int j=0;j<dsAbsentListBOL.Tables[2].Rows.Count;j++)
                        {
                            if(dsAbsentListBOL.Tables[2].Rows[j]["HolidayDate"].ToString()==dtFrom.Date.ToString())
                            {
                                IsHoliday=true;
                                break;
                            }

                        }
                          if(!IsHoliday)
                          {
                              MailMessage objMailMessage = new MailMessage();
                                objMailMessage.From ="pmo@in.v2solutions.com" ;
                                objMailMessage.To =  dsAbsentListBOL.Tables[0].Rows[i]["EmployeeEmailID"].ToString();
                               // objMailMessage.To="chandrashekar.v@in.v2solutions.com";
                                //string strSubject=

                                objMailMessage.Subject =dsAbsentListBOL.Tables[1].Rows[0]["EmailSubject"].ToString() ;

                               // objMailMessage.BodyFormat = MailFormat.Html;
                                //objMailMessage.Body=dsAbsentListBOL.Tables["EmailTemplates"].Rows[0]["EmployeeEmailID"].ToString();
                               // objMailMessage.BodyFormat = MailFormat.Html;

                                //objMailMessage.Body = mailMessage;

                                string strBody = "Hi ##EmployeeName##" + " \n\n" +
                                                      "It is observed that there was no Orbit Sign-In / Sign-Out for you on ##SignInDate##," + " \n" +
                                                       "so if you are present for the day and forgot to login, please do the manual entry for the same." + " \n\n" +
                                                       "It is absorved that you have Leave Balance so apply for Leave" + " \n\n" +
                                                       "If you were on approved leave please fill the leave application form and send it to your reporting head." +" \n\n" +
                                                       "As the salary is being processed based on the Orbit SignIn Approved entries it is necessary to do the same on daily basis.";

                                strBody=Regex.Replace(strBody,"##EmployeeName##",dsAbsentListBOL.Tables[0].Rows[i]["EmployeeName"].ToString());

                                objMailMessage.Body=Regex.Replace(strBody,"##SignInDate##",dsAbsentListBOL.Tables[0].Rows[i]["SignInDate"].ToString());
                                    
              
                                //SmtpMail.SmtpServer="192.168.8.10";     //System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString();
                                SmtpMail.Send(objMailMessage);
                          }
                        
                       
                      }
                        break;
                    }
                    //DateTime dttomorrow=DateTime.Now.
                     
                    daOnTimeAbsentLeave.TimeoutDuration=new TimeSpan(0,5,0);
           
                }


                
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveAbsentCheckWF.cs", "caCheckandSendMail_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
           


        }


        }
}

