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
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using System.Text.RegularExpressions;


namespace OutOfOfficeWF
{
    public sealed partial class OutOfOfficeWF : StateMachineWorkflowActivity
    {
        private int outOfOfficeID;
        public string To,From,Body,Subject,SMTPServer;

        public int OutOfOfficeID
        {
            get { return outOfOfficeID; }
            set { outOfOfficeID = value; }
        }
        

        public OutOfOfficeWF()
        {
            InitializeComponent();
        }

        private void caGetReportingToInfo_ExecuteCode(object sender, EventArgs e)
        {
            OutOfOfficeBOL objOutOfOfficeBOL=new OutOfOfficeBOL();
            try
            {
            DataSet dsOutOfOfficeDetails= objOutOfOfficeBOL.WFGetOutOfOfficeDetails(outOfOfficeID);
            To = dsOutOfOfficeDetails.Tables[0].Rows[0]["ApproverEmailID"].ToString();
            for (int j = 0; j<dsOutOfOfficeDetails.Tables[2].Rows.Count ;j++)
            {
                if (dsOutOfOfficeDetails.Tables[2].Rows[j]["ConfigItemName"].ToString() == "From EmailID")
                {
                    From = dsOutOfOfficeDetails.Tables[2].Rows[j]["ConfigItemValue"].ToString();
                }
                if(dsOutOfOfficeDetails.Tables[2].Rows[j]["ConfigItemName"].ToString() == "Mail Server Name")
                {
                    SMTPServer=dsOutOfOfficeDetails.Tables[2].Rows[j]["ConfigItemValue"].ToString();
                }
            }
            for (int i = 0; i < dsOutOfOfficeDetails.Tables[1].Rows.Count; i++)
            {
                if (dsOutOfOfficeDetails.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Out Of Office Request")
                {
                    string strSubject = dsOutOfOfficeDetails.Tables[1].Rows[i]["EmailSubject"].ToString();
                    strSubject = Regex.Replace(strSubject, "##EmployeeName##", dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                    Subject = strSubject;

                    string strBody = dsOutOfOfficeDetails.Tables[1].Rows[i]["EmailBody"].ToString();
                    //strBody = "\n" + "Hi ##EmployeeName##," + "\n\n" + "   Out Of Office entry is recorded in the system." + " \n\n" + "Out Of Office Details: " + " \n\n" + "  Date : ##Date##" + " \n" + "  Out Time : ##OutTime##" +  "\n" + "  In Time : ##InTime## " + " \n" + "  Reason : ##Comment##" ;
                    strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);
                    strBody = Regex.Replace(strBody, "##ApproverName##", dsOutOfOfficeDetails.Tables[0].Rows[0]["ApproverName"].ToString());
                    strBody = Regex.Replace(strBody, "##EmployeeName##", dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                    strBody = Regex.Replace(strBody,"##Date##",dsOutOfOfficeDetails.Tables[0].Rows[0]["Date"].ToString());
                    strBody = Regex.Replace(strBody,"##OutTime##",dsOutOfOfficeDetails.Tables[0].Rows[0]["OutTime"].ToString());
                    strBody = Regex.Replace(strBody,"##InTime##",dsOutOfOfficeDetails.Tables[0].Rows[0]["InTime"].ToString());
                    strBody = Regex.Replace(strBody,"##Comment##",dsOutOfOfficeDetails.Tables[0].Rows[0]["Comment"].ToString());
                    Body = strBody;                    
                }
            }
            }
            catch(V2Exceptions ex)
            {
                throw ;
            }

            catch(System.Exception ex)
            {
                
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeWF.cs", "caGetReportingToInfo_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        private void caApproval_ExecuteCode(object sender, EventArgs e)
        {

            OutOfOfficeBOL objOutOfOfficeBOL=new OutOfOfficeBOL();
            try
            {
                 DataSet dsOutOfOfficeDetails= objOutOfOfficeBOL.WFGetOutOfOfficeDetails(outOfOfficeID);            

            /*To = dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
            string strSubject= dsOutOfOfficeDetails.Tables[1].Rows[6]["EmailSubject"].ToString();
            Subject= Regex.Replace(strSubject,"##User##",dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeName"].ToString());                        
            Body=dsOutOfOfficeDetails.Tables[1].Rows[6]["EmailBody"].ToString();
            Subject= Regex.Replace(strSubject,"##User##",dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeName"].ToString());*/

            To = dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
            for (int j = 0; j < dsOutOfOfficeDetails.Tables[2].Rows.Count; j++)
                if (dsOutOfOfficeDetails.Tables[2].Rows[j]["ConfigItemName"].ToString() == "From EmailID")
                {
                    From = dsOutOfOfficeDetails.Tables[2].Rows[j]["ConfigItemValue"].ToString();
                }
            for (int i = 0; i < dsOutOfOfficeDetails.Tables[1].Rows.Count; i++)
            {
                if (dsOutOfOfficeDetails.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Out Of Office Application on Approved")
                {
                    string strSubject = dsOutOfOfficeDetails.Tables[1].Rows[i]["EmailSubject"].ToString();
                    strSubject = Regex.Replace(strSubject, "##EmployeeName##", dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                    Subject = strSubject;

                    string strBody = dsOutOfOfficeDetails.Tables[1].Rows[i]["EmailBody"].ToString();
                          //strBody = "<font style="color:Navy;font-size:smaller;font-family:Arial"> Hi <b> ##EmployeeName##,  </b>  <br> <br>   Out Of Office entry is approved.  <br>  <br> Out Of Office Details:  <br>  <br>  Date : ##Date## <br>  <br> Out Time : ##OutTime## <br> In Time : ##InTime##  <br>  Reason : ##Comment##  <br>  <br>  Approved By: ##ApproverName##";

                    strBody = Regex.Replace(strBody, "##ApproverName##", dsOutOfOfficeDetails.Tables[0].Rows[0]["ApproverName"].ToString());
                    strBody = Regex.Replace(strBody, "##EmployeeName##", dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                    strBody = Regex.Replace(strBody, "##Date##", dsOutOfOfficeDetails.Tables[0].Rows[0]["Date"].ToString());
                    strBody = Regex.Replace(strBody, "##OutTime##", dsOutOfOfficeDetails.Tables[0].Rows[0]["OutTime"].ToString());
                    strBody = Regex.Replace(strBody, "##InTime##", dsOutOfOfficeDetails.Tables[0].Rows[0]["InTime"].ToString());
                    strBody = Regex.Replace(strBody, "##Comment##", dsOutOfOfficeDetails.Tables[0].Rows[0]["Comment"].ToString());
                    Body = strBody;
                }
            }

            }
            catch(V2Exceptions ex)
            {
                throw ;
            }

            catch(System.Exception ex)
            {
                
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeWF.cs", "caApproval_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
           



        }

        private void caRejected_ExecuteCode(object sender, EventArgs e)
        {
            try
            {
                OutOfOfficeBOL objOutOfOfficeBOL=new OutOfOfficeBOL();
            DataSet dsOutOfOfficeDetails= objOutOfOfficeBOL.WFGetOutOfOfficeDetails(outOfOfficeID);
           
            /*To=dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
            From="chandrashekar.v@in.v2solutions.com";

            string strSubject= dsOutOfOfficeDetails.Tables[1].Rows[7]["EmailSubject"].ToString();
            Subject= Regex.Replace(strSubject,"##User##",dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        
            Body=dsOutOfOfficeDetails.Tables[1].Rows[7]["EmailBody"].ToString();
            Subject= Regex.Replace(strSubject,"##User##",dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeName"].ToString());*/

            To = dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
            for (int j = 0; j < dsOutOfOfficeDetails.Tables[2].Rows.Count; j++)
                if (dsOutOfOfficeDetails.Tables[2].Rows[j]["ConfigItemName"].ToString() == "From EmailID")
                {
                    From = dsOutOfOfficeDetails.Tables[2].Rows[j]["ConfigItemValue"].ToString();
                }
            for (int i = 0; i < dsOutOfOfficeDetails.Tables[1].Rows.Count; i++)
            {
                if (dsOutOfOfficeDetails.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "OutOfOffice Application on Rejected")
                {
                    string strSubject = dsOutOfOfficeDetails.Tables[1].Rows[i]["EmailSubject"].ToString();
                    strSubject = Regex.Replace(strSubject, "##EmployeeName##", dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                    Subject = strSubject;

                    string strBody = dsOutOfOfficeDetails.Tables[1].Rows[i]["EmailBody"].ToString();
                    //strBody = "\n" + "Hi ##EmployeeName##," + "\n\n" + "   Out Of Office entry is Rejected." + " \n\n" + "Out Of Office Details: " + " \n\n" + "  Date : ##Date##" + " \n" + "  Out Time : ##OutTime##" + "\n" + "  In Time : ##InTime## " + " \n" + "  Reason : ##Comment##" + " \n\n" + "  Approved By: ##ApproverName##";
                    strBody = Regex.Replace(strBody, "##ApproverName##", dsOutOfOfficeDetails.Tables[0].Rows[0]["ApproverName"].ToString());
                    strBody = Regex.Replace(strBody, "##EmployeeName##", dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                    strBody = Regex.Replace(strBody, "##Date##", dsOutOfOfficeDetails.Tables[0].Rows[0]["Date"].ToString());
                    strBody = Regex.Replace(strBody, "##OutTime##", dsOutOfOfficeDetails.Tables[0].Rows[0]["OutTime"].ToString());
                    strBody = Regex.Replace(strBody, "##InTime##", dsOutOfOfficeDetails.Tables[0].Rows[0]["InTime"].ToString());
                    strBody = Regex.Replace(strBody, "##Comment##", dsOutOfOfficeDetails.Tables[0].Rows[0]["Comment"].ToString());
                    Body = strBody;
                }
            }
            }
            catch(V2Exceptions ex)
            {
                throw ;
            }

            catch(System.Exception ex)
            {
                
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeWF.cs", "caRejected_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
            

            
        }

        private void caCancelled_ExecuteCode(object sender, EventArgs e)
        {
            try
            {
                 OutOfOfficeBOL objOutOfOfficeBOL=new OutOfOfficeBOL();
            DataSet dsOutOfOfficeDetails= objOutOfOfficeBOL.WFGetOutOfOfficeDetails(outOfOfficeID);
           
            To=dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
            

            string strSubject= dsOutOfOfficeDetails.Tables[1].Rows[8]["EmailSubject"].ToString();
            Subject= Regex.Replace(strSubject,"##User##",dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        
            Body=dsOutOfOfficeDetails.Tables[1].Rows[8]["EmailBody"].ToString();
            Subject= Regex.Replace(strSubject,"##User##",dsOutOfOfficeDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
            }
            catch(V2Exceptions ex)
            {
                throw ;
            }

            catch(System.Exception ex)
            {
                
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeWF.cs", "caRejected_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
           
        }
    }
}
