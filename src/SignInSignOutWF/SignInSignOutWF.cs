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
using V2.CommonServices;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using System.Text.RegularExpressions;


namespace SignInSignOutWF
{
    public sealed partial class SignInSignOutWF : StateMachineWorkflowActivity
    {
        private int signInSignOutID;
        public string To, From, Body, Subject, strFrom;
        public string SMTPServer ;   //= "192.168.8.10"
        public int SignInSignOutID
        {
            get { return signInSignOutID; }
            set { signInSignOutID = value; }
        }

        public SignInSignOutWF()
        {
            InitializeComponent();
        }

        private void caGetReportingToInfo_ExecuteCode(object sender, EventArgs e)
        { 
            SignInSignOutBOL objSignInSignOutBOL = new SignInSignOutBOL();
            DataSet dsSignInSignOut = objSignInSignOutBOL.WFGetSignInSignOutDetails(SignInSignOutID);

            for (int k = 0; k < dsSignInSignOut.Tables[2].Rows.Count; k++)
            {
                if (dsSignInSignOut.Tables[2].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                {

                    strFrom = dsSignInSignOut.Tables[2].Rows[k]["ConfigItemValue"].ToString();

                }
                if (dsSignInSignOut.Tables[2].Rows[k]["ConfigItemName"].ToString() == "Mail Server Name")
                {

                    SMTPServer = dsSignInSignOut.Tables[2].Rows[k]["ConfigItemValue"].ToString();

                }
            }

           
            //To = dsSignInSignOut.Tables[0].Rows[0]["ApproverEmailID"].ToString();
            To = dsSignInSignOut.Tables[0].Rows[0]["ApproverEmailID"].ToString();//  "kajal.shah@in.v2solutions.com";
            From = strFrom;
           
           
            
            string strBody = "";
            if (dsSignInSignOut.Tables[0].Rows[0]["SignInComment"] != null)
            {
                string str = dsSignInSignOut.Tables[0].Rows[0]["SignInComment"].ToString();
                char[] chrSplitter = { ':' };
                string[] strArray = new string[2];
                strArray = str.Split(chrSplitter);
                

                if (strArray[0].ToString() == "##Leave")
                {
                    for (int i = 0; i < dsSignInSignOut.Tables[1].Rows.Count; i++)
                    {
                        if (dsSignInSignOut.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Pending Sign In Sign Out Entry Waiting Approval due to Leave")
                        {

                            strBody = dsSignInSignOut.Tables[1].Rows[i]["EmailBody"].ToString();
                            Subject= dsSignInSignOut.Tables[1].Rows[i]["EmailSubject"].ToString();
                             
                            break;

                             

                        }
                    }
                }

                else if (dsSignInSignOut.Tables[0].Rows[0]["TotalHoursWorked"] != null)
                {
                    string str1 = dsSignInSignOut.Tables[0].Rows[0]["TotalHoursWorked"].ToString();
                    char[] chrSplitter1 = { ':' };
                    string[] strArray1 = new string[2];
                    strArray1 = str1.Split(chrSplitter1);
                    int intHours = Convert.ToInt32(strArray1[0].ToString());
                    if (intHours >= 24)
                    {

                        for (int i = 0; i < dsSignInSignOut.Tables[1].Rows.Count; i++)
                        {
                            if (dsSignInSignOut.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Pending Sign In Sign Out Entry Waiting Approval due to 24 hrs")
                            {

                                strBody = dsSignInSignOut.Tables[1].Rows[i]["EmailBody"].ToString();
                                Subject= dsSignInSignOut.Tables[1].Rows[i]["EmailSubject"].ToString();
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dsSignInSignOut.Tables[1].Rows.Count; i++)
                        {
                            if (dsSignInSignOut.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Pending Sign In Sign Out Entry Waiting Approval due to 5 day cross")
                            {

                                strBody = dsSignInSignOut.Tables[1].Rows[i]["EmailBody"].ToString();
                                Subject= dsSignInSignOut.Tables[1].Rows[i]["EmailSubject"].ToString();
                                break;
                            }
                        }
                    }
                }

                Subject= Regex.Replace(Subject, "##EmployeeName##", dsSignInSignOut.Tables[0].Rows[0]["EmployeeName"].ToString());
                strBody = Regex.Replace(strBody, "##ApproverName##", dsSignInSignOut.Tables[0].Rows[0]["ApproverName"].ToString());
                strBody = Regex.Replace(strBody, "##EmployeeName##", dsSignInSignOut.Tables[0].Rows[0]["EmployeeName"].ToString());
		        strBody = Regex.Replace(strBody, "##Date##", dsSignInSignOut.Tables[0].Rows[0]["Date"].ToString());
                strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);

            }
            Body = strBody;
            



        }

        private void caApproval_ExecuteCode(object sender, EventArgs e)
        {
            SignInSignOutBOL objSignInSignOutBOL = new SignInSignOutBOL();
            DataSet dsSignInSignOut = objSignInSignOutBOL.WFGetSignInSignOutDetails(SignInSignOutID);

            //To = dsSignInSignOut.Tables[0].Rows[0]["EmployeeEmail"].ToString();
            To = dsSignInSignOut.Tables[0].Rows[0]["EmployeeEmail"].ToString();

            for (int k = 0; k < dsSignInSignOut.Tables[2].Rows.Count; k++)
            {
                if (dsSignInSignOut.Tables[2].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                {

                    strFrom = dsSignInSignOut.Tables[2].Rows[k]["ConfigItemValue"].ToString();

                }
            }
            From = strFrom;
            //Subject = "Orbit Entry Approved for ##SignInDate##";
           
            //strSubject = Regex.Replace(strSubject, "##EmployeeName##", dsSignInSignOut.Tables[0].Rows[0]["date"].ToString());
            
            string strBody = "";
            for (int i = 0; i < dsSignInSignOut.Tables[1].Rows.Count; i++)
            {
                if (dsSignInSignOut.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Approved Orbit Entry")
                {

                    strBody = dsSignInSignOut.Tables[1].Rows[i]["EmailBody"].ToString();
                    Subject= dsSignInSignOut.Tables[1].Rows[i]["EmailSubject"].ToString();
                                break;
                }
            }
           
            Subject = Regex.Replace(Subject, "##SignInDate##", dsSignInSignOut.Tables[0].Rows[0]["date"].ToString());
            strBody = Regex.Replace(strBody, "##SignInDate##", dsSignInSignOut.Tables[0].Rows[0]["date"].ToString());
            strBody = Regex.Replace(strBody, "##EmployeeName##", dsSignInSignOut.Tables[0].Rows[0]["EmployeeName"].ToString());
            strBody = Regex.Replace(strBody, "##Date##", dsSignInSignOut.Tables[0].Rows[0]["Date"].ToString());
            strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);
            Body = strBody;
        }

        private void caRejected_ExecuteCode(object sender, EventArgs e)
        {
            SignInSignOutBOL objSignInSignOutBOL = new SignInSignOutBOL();
            DataSet dsSignInSignOut = objSignInSignOutBOL.WFGetSignInSignOutDetails(SignInSignOutID);
            //To = dsSignInSignOut.Tables[0].Rows[0]["ApproverEmailID"].ToString();
            To = dsSignInSignOut.Tables[0].Rows[0]["EmployeeEmail"].ToString();
            for (int k = 0; k < dsSignInSignOut.Tables[2].Rows.Count; k++)
            {
                if (dsSignInSignOut.Tables[2].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                {

                    strFrom = dsSignInSignOut.Tables[2].Rows[k]["ConfigItemValue"].ToString();

                }
            }
            From = strFrom;

            //Subject = "Orbit Entry Rejected for ##SignInDate##";
            
            

            string strBody = "";
            for (int i = 0; i < dsSignInSignOut.Tables[1].Rows.Count; i++)
            {
                if (dsSignInSignOut.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Rejected Orbit Entry ")
                {

                    strBody = dsSignInSignOut.Tables[1].Rows[i]["EmailBody"].ToString();
                     Subject = Subject= dsSignInSignOut.Tables[1].Rows[i]["EmailSubject"].ToString();
                    break;
                }
            }
           Subject = Regex.Replace(Subject, "##SignInDate##", dsSignInSignOut.Tables[0].Rows[0]["date"].ToString());
            strBody = Regex.Replace(strBody, "##ApproverComments##", dsSignInSignOut.Tables[0].Rows[0]["ApproverComments"].ToString());
            strBody = Regex.Replace(strBody, "##SignInDate##", dsSignInSignOut.Tables[0].Rows[0]["date"].ToString());
            strBody = Regex.Replace(strBody, "##EmployeeName##", dsSignInSignOut.Tables[0].Rows[0]["EmployeeName"].ToString());
            strBody = Regex.Replace(strBody, "##date##", dsSignInSignOut.Tables[0].Rows[0]["date"].ToString());
            strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);
            Body = strBody;
        }

        private void caCanecl_ExecuteCode(object sender, EventArgs e)
        {
            SignInSignOutBOL objSignInSignOutBOL = new SignInSignOutBOL();
            DataSet dsSignInSignOut = objSignInSignOutBOL.WFGetSignInSignOutDetails(SignInSignOutID);
            To = dsSignInSignOut.Tables[0].Rows[0]["ApproverEmailID"].ToString();


            for (int k = 0; k < dsSignInSignOut.Tables[2].Rows.Count; k++)
            {
                if (dsSignInSignOut.Tables[2].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                {

                    strFrom = dsSignInSignOut.Tables[2].Rows[k]["ConfigItemValue"].ToString();

                }
            }
            From = strFrom;
            string strSubject = dsSignInSignOut.Tables[1].Rows[8]["EmailSubject"].ToString();
            Subject = Regex.Replace(strSubject, "##User##", dsSignInSignOut.Tables[0].Rows[0]["EmployeeName"].ToString());
            //Body = Regex.Replace(dsSignInSignOut.Tables[1].Rows[8]["EmailBody"].ToString(), "##User##", dsSignInSignOut.Tables[0].Rows[0]["EmployeeName"].ToString());

            string strBody = "Hi ##EmployeeName##," + " \n\n" + " Your pending Orbit entry for ##SignInTime## was Cancelled and the following were the Approver's comment ##ApproverComments##";
            strBody = Regex.Replace(strBody, "##ApproverName##", dsSignInSignOut.Tables[0].Rows[0]["ApproverName"].ToString());
            strBody = Regex.Replace(strBody, "##SignInTime##", dsSignInSignOut.Tables[0].Rows[0]["SignInTime"].ToString());
            strBody = Regex.Replace(strBody, "##EmployeeName##", dsSignInSignOut.Tables[0].Rows[0]["EmployeeName"].ToString());
            strBody = Regex.Replace(strBody, "###SignInDate##", dsSignInSignOut.Tables[0].Rows[0]["date"].ToString());
            Body = strBody;
        }
    }

}
