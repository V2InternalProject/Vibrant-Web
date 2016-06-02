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
using System.Text.RegularExpressions;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Orbit.Workflow.CompensationWF
{
    public sealed partial class Compensation : StateMachineWorkflowActivity
    {
        private int compensationID;
        public string To, From, Subject, Body, strFrom, SMTPServer;

        public int CompensationID
        {
            get { return compensationID; }
            set { compensationID = value; }
        }
        public Compensation()
        {
            InitializeComponent();
        }

        private void caRepotingToDetails_ExecuteCode(object sender, EventArgs e)
        {
            CompensationDetailsBOL objCompensationDetailsBOL = new CompensationDetailsBOL();
            try
            {
                DataSet dsCompensationDetails = objCompensationDetailsBOL.WFGetCompensationDetails(CompensationID);

                for (int k = 0; k < dsCompensationDetails.Tables[2].Rows.Count; k++)
                {
                    if (dsCompensationDetails.Tables[2].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                    {

                        strFrom = dsCompensationDetails.Tables[2].Rows[k]["ConfigItemValue"].ToString();

                    }
                    if (dsCompensationDetails.Tables[2].Rows[k]["ConfigItemName"].ToString() == "Mail Server Name")
                    {

                        SMTPServer = dsCompensationDetails.Tables[2].Rows[k]["ConfigItemValue"].ToString();

                    }
                }

                To = dsCompensationDetails.Tables[0].Rows[0]["ApproverEmailID"].ToString();
                From = strFrom;


                for (int i = 0; i < dsCompensationDetails.Tables[1].Rows.Count; i++)
                {
                    if (dsCompensationDetails.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Compensatory Leave Request")
                    {
                        string strSubject = dsCompensationDetails.Tables[1].Rows[i]["EmailSubject"].ToString();
                        string strBody = dsCompensationDetails.Tables[1].Rows[i]["EmailBody"].ToString();

                        strSubject = Regex.Replace(strSubject, "##EmployeeName##", dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        Subject = strSubject;

                        //strBody = "\n" + "Hi ##ApproverName##," + " \n\n" + " Compensation Application Details: " + " \n\n" + "  Name: ##EmployeeName##" + " \n" + "  Applied For: ##AppliedFor##" + " \n" + "  Reason: ##Reason##" + "\n\n" + "  Compensation Application submitted.";

                        strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);
                        strBody = Regex.Replace(strBody, "##ApproverName##", dsCompensationDetails.Tables[0].Rows[0]["ApproverName"].ToString());
                        strBody = Regex.Replace(strBody, "##EmployeeName##", dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        strBody = Regex.Replace(strBody, "##AppliedFor##", Convert.ToDateTime(dsCompensationDetails.Tables[0].Rows[0]["AppliedFor"].ToString()).ToShortDateString());
                        strBody = Regex.Replace(strBody, "##Reason##", dsCompensationDetails.Tables[0].Rows[0]["Reason"].ToString());


                        Body = strBody;
                        break;
                    }
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "Compensation.cs", "caRepotingToDetails_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }



            //To= dsCompensationDetails.Tables[0].Rows[0]["ApproverEmailID"].ToString();
            //To="mohan.p@in.v2solutions.com";
            //From=dsCompensationDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();

            //string strSubject= dsCompensationDetails.Tables[1].Rows[0]["EmailSubject"].ToString();
            //Subject= Regex.Replace(strSubject,"##User##",dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());


            //Body=Regex.Replace(dsCompensationDetails.Tables[1].Rows[0]["EmailBody"].ToString(),"##User##",dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());


        }

        private void caApproval_ExecuteCode(object sender, EventArgs e)
        {
            CompensationDetailsBOL objCompensationDetailsBOL = new CompensationDetailsBOL();
            try
            {
                DataSet dsCompensationDetails = objCompensationDetailsBOL.WFGetCompensationDetails(CompensationID);

                for (int k = 0; k < dsCompensationDetails.Tables[2].Rows.Count; k++)
                {
                    if (dsCompensationDetails.Tables[2].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                    {

                        strFrom = dsCompensationDetails.Tables[2].Rows[k]["ConfigItemValue"].ToString();
                        break;

                    }
                }

                To = To = dsCompensationDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
                From = strFrom;


                for (int i = 0; i < dsCompensationDetails.Tables[1].Rows.Count; i++)
                {
                    if (dsCompensationDetails.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Compensatory Leave Application on Approved")
                    {
                        string strSubject = dsCompensationDetails.Tables[1].Rows[i]["EmailSubject"].ToString();
                        string strBody = dsCompensationDetails.Tables[1].Rows[i]["EmailBody"].ToString();

                        strSubject = Regex.Replace(strSubject, "##EmployeeName##", dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        Subject = strSubject;

                        //strBody = "\n" + "Hi ##EmployeeName##," + " \n\n" + " The Compensation is approved and recorded in the system." + " \n\n" + " Compensation Application Details: " + " \n\n" + "  Applied For: ##AppliedFor##" + " \n" + "  Reason: ##Reason##" + "\n\n" + "  Approved By: ##ApproverName##";
                        //strBody = "\n" + "Hi ##ApproverName##," + " \n\n" + " Compensation Application Details: " + " \n\n" + "  Name: ##EmployeeName##" + " \n" + "  Applied For: ##AppliedFor##" + " \n" + "  Reason: ##Reason##" + "\n\n" + "  Compensation Application submitted.";

                        strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);
                        strBody = Regex.Replace(strBody, "##ApproverName##", dsCompensationDetails.Tables[0].Rows[0]["ApproverName"].ToString());
                        strBody = Regex.Replace(strBody, "##EmployeeName##", dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        strBody = Regex.Replace(strBody, "##AppliedFor##", Convert.ToDateTime(dsCompensationDetails.Tables[0].Rows[0]["AppliedFor"].ToString()).ToShortDateString());
                        strBody = Regex.Replace(strBody, "##Reason##", dsCompensationDetails.Tables[0].Rows[0]["Reason"].ToString());


                        Body = strBody;
                        break;
                    }
                }

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "Compensation.cs", "caApproval_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }



            //To=dsCompensationDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
            //From=dsCompensationDetails.Tables[0].Rows[0]["ApproverEmailID"].ToString();

            //string strSubject= dsCompensationDetails.Tables[1].Rows[0]["EmailSubject"].ToString();
            //Subject= Regex.Replace(strSubject,"##User##",dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());

            //Body=dsCompensationDetails.Tables[1].Rows[0]["EmailBody"].ToString();
            //Subject= Regex.Replace(strSubject,"##User##",dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());



        }

        private void caRejected_ExecuteCode(object sender, EventArgs e)
        {
            CompensationDetailsBOL objCompensationDetailsBOL = new CompensationDetailsBOL();
            try
            {
                DataSet dsCompensationDetails = objCompensationDetailsBOL.WFGetCompensationDetails(CompensationID);

                for (int k = 0; k < dsCompensationDetails.Tables[2].Rows.Count; k++)
                {
                    if (dsCompensationDetails.Tables[2].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                    {

                        strFrom = dsCompensationDetails.Tables[2].Rows[k]["ConfigItemValue"].ToString();
                        break;

                    }
                }

                To = To = dsCompensationDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
                From = strFrom;


                for (int i = 0; i < dsCompensationDetails.Tables[1].Rows.Count; i++)
                {
                    if (dsCompensationDetails.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Compensatory Leave Application on Rejected")
                    {
                        string strSubject = dsCompensationDetails.Tables[1].Rows[i]["EmailSubject"].ToString();
                        string strBody = dsCompensationDetails.Tables[1].Rows[i]["EmailBody"].ToString();

                        strSubject = Regex.Replace(strSubject, "##EmployeeName##", dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        Subject = strSubject;

                        //strBody = "\n" + "Hi ##EmployeeName##," + " \n\n" + " The Compensation is Rejected for ##ApproverComments##." + " \n\n\n" + "  Approved By: ##ApproverName##.";
                        //strBody = "\n" + "Hi ##ApproverName##," + " \n\n" + " Compensation Application Details: " + " \n\n" + "  Name: ##EmployeeName##" + " \n" + "  Applied For: ##AppliedFor##" + " \n" + "  Reason: ##Reason##" + "\n\n" + "  Compensation Application submitted.";

                        strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);
                        strBody = Regex.Replace(strBody, "##ApproverName##", dsCompensationDetails.Tables[0].Rows[0]["ApproverName"].ToString());
                        strBody = Regex.Replace(strBody, "##EmployeeName##", dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        strBody = Regex.Replace(strBody, "##AppliedFor##", Convert.ToDateTime(dsCompensationDetails.Tables[0].Rows[0]["AppliedFor"].ToString()).ToShortDateString());
                        strBody = Regex.Replace(strBody, "##Reason##", dsCompensationDetails.Tables[0].Rows[0]["Reason"].ToString());
                        strBody = Regex.Replace(strBody, "##ApproverComments##", dsCompensationDetails.Tables[0].Rows[0]["ApproverComments"].ToString());


                        Body = strBody;
                        break;
                    }
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "Compensation.cs", "caRejected_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }



            //To=dsCompensationDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
            //From=dsCompensationDetails.Tables[0].Rows[0]["ApproverEmailID"].ToString();

            //string strSubject= dsCompensationDetails.Tables[1].Rows[2]["EmailSubject"].ToString();
            //Subject= Regex.Replace(strSubject,"##User##",dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());

            //Body=dsCompensationDetails.Tables[1].Rows[2]["EmailBody"].ToString();
            //Subject= Regex.Replace(strSubject,"##User##",dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());

        }

        private void caCancel_ExecuteCode(object sender, EventArgs e)
        {

            CompensationDetailsBOL objCompensationDetailsBOL = new CompensationDetailsBOL();
            try
            {
                DataSet dsCompensationDetails = objCompensationDetailsBOL.WFGetCompensationDetails(CompensationID);

                for (int k = 0; k < dsCompensationDetails.Tables[2].Rows.Count; k++)
                {
                    if (dsCompensationDetails.Tables[2].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                    {

                        strFrom = dsCompensationDetails.Tables[2].Rows[k]["ConfigItemValue"].ToString();
                        break;

                    }
                }

                To = To = dsCompensationDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
                From = strFrom;


                for (int i = 0; i < dsCompensationDetails.Tables[1].Rows.Count; i++)
                {
                    if (dsCompensationDetails.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Compensatory Leave Application on Cancelled")
                    {
                        string strSubject = dsCompensationDetails.Tables[1].Rows[i]["EmailSubject"].ToString();
                        string strBody = dsCompensationDetails.Tables[1].Rows[i]["EmailBody"].ToString();

                        strSubject = Regex.Replace(strSubject, "##EmployeeName##", dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        Subject = strSubject;

                        //strBody = "\n" + "Hi ##EmployeeName##," + " \n\n" + " The Compensation is Rejected for ##ApproverComments##." + " \n\n\n" + "  Approved By: ##ApproverName##.";
                        //strBody = "\n" + "Hi ##EmployeeName##," + " \n\n" + " As requested your leave is canceled and the required updates are made in the system." + " \n\n" + "  Approved By: ##ApproverName##";

                        strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);
                        strBody = Regex.Replace(strBody, "##ApproverName##", dsCompensationDetails.Tables[0].Rows[0]["ApproverName"].ToString());
                        strBody = Regex.Replace(strBody, "##EmployeeName##", dsCompensationDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        strBody = Regex.Replace(strBody, "##AppliedFor##", Convert.ToDateTime(dsCompensationDetails.Tables[0].Rows[0]["AppliedFor"].ToString()).ToShortDateString());
                        strBody = Regex.Replace(strBody, "##Reason##", dsCompensationDetails.Tables[0].Rows[0]["Reason"].ToString());
                        strBody = Regex.Replace(strBody, "##ApproverComments##", dsCompensationDetails.Tables[0].Rows[0]["ApproverComments"].ToString());


                        Body = strBody;
                        break;
                    }
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "Compensation.cs", "caRejected_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }



        }
    }
}
