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
using System.Data;

using V2.Orbit.BusinessLayer;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using System.Text.RegularExpressions;


namespace LeaveDetailsWF
{
    public sealed partial class LeaveDetailsWF : StateMachineWorkflowActivity
    {
        private int leaveDetailID;
        public string To, From, Subject, Body, strFrom, SMTPServer, Cc;

        public int LeaveDetailID
        {
            get { return leaveDetailID; }
            set { leaveDetailID = value; }
        }

        public LeaveDetailsWF()
        {
            InitializeComponent();
        }

        private void caGetLeaveDetailsInfo_ExecuteCode(object sender, EventArgs e)
        {

            LeaveDetailsBOL objLeaveDetailsBOL = new LeaveDetailsBOL();

            try
            {
                DataSet dsLeaveDetails = objLeaveDetailsBOL.WFGetLeaveDetails(LeaveDetailID);


                for (int k = 0; k < dsLeaveDetails.Tables[2].Rows.Count; k++)
                {
                    if (dsLeaveDetails.Tables[2].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                    {

                        strFrom = dsLeaveDetails.Tables[2].Rows[k]["ConfigItemValue"].ToString();

                    }
                    if (dsLeaveDetails.Tables[2].Rows[k]["ConfigItemName"].ToString() == "Mail Server Name")
                    {

                        SMTPServer = dsLeaveDetails.Tables[2].Rows[k]["ConfigItemValue"].ToString();

                    }
                }

                To = dsLeaveDetails.Tables[0].Rows[0]["ApproverEmailID"].ToString();
                Cc = dsLeaveDetails.Tables[0].Rows[0]["CompetencyMailID"].ToString();
                //From=dsLeaveDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
                From = strFrom;


                for (int i = 0; i < dsLeaveDetails.Tables[1].Rows.Count; i++)
                {
                    if (dsLeaveDetails.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Leave Request")
                    {
                        string strSubject = dsLeaveDetails.Tables[1].Rows[i]["EmailSubject"].ToString();
                        string strBody = dsLeaveDetails.Tables[1].Rows[i]["EmailBody"].ToString();

                        strSubject = Regex.Replace(strSubject, "##EmployeeName##", dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        Subject = strSubject;

                        //strBody = "\n" + "Hi ##ApproverName##," + " \n\n" + " Leave Application Details: " + " \n\n" + "  Name: ##EmployeeName##" + " \n" + "  From Date: ##LeaveDateFrom##" + " \n" + "  To Date: ##LeaveDateTo##" + " \n" + "  Reason: ##LeaveReason##" + " \n" + "  Total Applied Leaves: ##TotalAppliedLeaves##" + " \n" + "  Available Leaves: ##TotalLeaveDays##" + " \n" + "  Marked as Abesent: ##LeaveCorrectionDays##" + "\n\n" + "  Kindly do the needful at the earliest, So that the salary won’t be affected as the salary is processed on the leave details it is mandatory to do it on time.";


                        strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);

                        strBody = Regex.Replace(strBody, "##ApproverName##", dsLeaveDetails.Tables[0].Rows[0]["ApproverName"].ToString());
                        strBody = Regex.Replace(strBody, "##EmployeeName##", dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        strBody = Regex.Replace(strBody, "##LeaveDateFrom##", Convert.ToDateTime(dsLeaveDetails.Tables[0].Rows[0]["LeaveDateFrom"].ToString()).ToShortDateString());
                        strBody = Regex.Replace(strBody, "##LeaveDateTo##", Convert.ToDateTime(dsLeaveDetails.Tables[0].Rows[0]["LeaveDateTo"].ToString()).ToShortDateString());
                        strBody = Regex.Replace(strBody, "##LeaveReason##", dsLeaveDetails.Tables[0].Rows[0]["LeaveReason"].ToString());
                        strBody = Regex.Replace(strBody, "##TotalLeaveDays##", dsLeaveDetails.Tables[0].Rows[0]["TotalLeaveDays"].ToString());
                        float TotalLeaves;
                        TotalLeaves = (float)(Convert.ToDouble(dsLeaveDetails.Tables[0].Rows[0]["TotalLeaveDays"].ToString()) + Convert.ToDouble(dsLeaveDetails.Tables[0].Rows[0]["LeaveCorrectionDays"].ToString()));
                        strBody = Regex.Replace(strBody, "##LeaveCorrectionDays##", dsLeaveDetails.Tables[0].Rows[0]["LeaveCorrectionDays"].ToString());
                        strBody = Regex.Replace(strBody, "##TotalAppliedLeaves##", Convert.ToString(TotalLeaves).ToString());

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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsWF.cs", "caGetLeaveDetailsInfo_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }


        }



        private void caGetApprovalRequiredData_ExecuteCode(object sender, EventArgs e)
        {
            try
            {
                LeaveDetailsBOL objLeaveDetailsBOL = new LeaveDetailsBOL();

                DataSet dsLeaveDetails = objLeaveDetailsBOL.WFGetLeaveDetails(LeaveDetailID);


                for (int k = 0; k < dsLeaveDetails.Tables[2].Rows.Count; k++)
                {
                    if (dsLeaveDetails.Tables[2].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                    {

                        strFrom = dsLeaveDetails.Tables[2].Rows[k]["ConfigItemValue"].ToString();
                        break;

                    }
                }
                To = dsLeaveDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
                //From = dsLeaveDetails.Tables[0].Rows[0]["ApproverEmailID"].ToString();
                Cc = dsLeaveDetails.Tables[0].Rows[0]["CompetencyMailID"].ToString();
                From = strFrom;

                for (int i = 0; i < dsLeaveDetails.Tables[1].Rows.Count; i++)
                {
                    if (dsLeaveDetails.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Leave Application on Approved")
                    {
                        string strSubject = dsLeaveDetails.Tables[1].Rows[i]["EmailSubject"].ToString();
                        string strBody = dsLeaveDetails.Tables[1].Rows[i]["EmailBody"].ToString();

                        strSubject = Regex.Replace(strSubject, "##EmployeeName##", dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        Subject = strSubject;

                        //strBody = "\n" + "Hi ##EmployeeName##," + " \n\n" + " The leave is approved and recorded in the system." + " \n\n" + " Leave Application Details: " + " \n\n" + "  From Date: ##LeaveDateFrom##" + " \n" + "  To Date: ##LeaveDateTo##" + " \n" + "  Reason: ##LeaveReason##" + " \n" + "  Total Applied Leaves: ##TotalAppliedLeaves##" + " \n" + "  Available Leaves: ##TotalLeaveDays##" + " \n" + "  Marked as Abesent: ##LeaveCorrectionDays##" +  " \n\n" + "  Approved By: ##ApproverName##";

                        strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);
                        strBody = Regex.Replace(strBody, "##ApproverName##", dsLeaveDetails.Tables[0].Rows[0]["ApproverName"].ToString());
                        strBody = Regex.Replace(strBody, "##EmployeeName##", dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        strBody = Regex.Replace(strBody, "##LeaveDateFrom##", Convert.ToDateTime(dsLeaveDetails.Tables[0].Rows[0]["LeaveDateFrom"].ToString()).ToShortDateString());
                        strBody = Regex.Replace(strBody, "##LeaveDateTo##", Convert.ToDateTime(dsLeaveDetails.Tables[0].Rows[0]["LeaveDateTo"].ToString()).ToShortDateString());
                        strBody = Regex.Replace(strBody, "##LeaveReason##", dsLeaveDetails.Tables[0].Rows[0]["LeaveReason"].ToString());
                        strBody = Regex.Replace(strBody, "##TotalLeaveDays##", dsLeaveDetails.Tables[0].Rows[0]["TotalLeaveDays"].ToString());
                        strBody = Regex.Replace(strBody, "##LeaveCorrectionDays##", dsLeaveDetails.Tables[0].Rows[0]["LeaveCorrectionDays"].ToString());
                        float TotalLeaves;
                        TotalLeaves = (float)(Convert.ToDouble(dsLeaveDetails.Tables[0].Rows[0]["TotalLeaveDays"].ToString()) + Convert.ToDouble(dsLeaveDetails.Tables[0].Rows[0]["LeaveCorrectionDays"].ToString()));
                        strBody = Regex.Replace(strBody, "##TotalAppliedLeaves##", Convert.ToString(TotalLeaves).ToString());


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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsWF.cs", "caGetApprovalRequiredData_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

            //To=dsLeaveDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
            //From=dsLeaveDetails.Tables[0].Rows[0]["ApproverEmailID"].ToString();

            //string strSubject= dsLeaveDetails.Tables[1].Rows[0]["EmailSubject"].ToString();
            //Subject= Regex.Replace(strSubject,"##User##",dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());

            //Body=dsLeaveDetails.Tables[1].Rows[0]["EmailBody"].ToString();
            //Subject= Regex.Replace(strSubject,"##User##",dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());


        }



        private void caRejected_ExecuteCode(object sender, EventArgs e)
        {
            try
            {
                LeaveDetailsBOL objLeaveDetailsBOL = new LeaveDetailsBOL();

                DataSet dsLeaveDetails = objLeaveDetailsBOL.WFGetLeaveDetails(LeaveDetailID);

                for (int k = 0; k < dsLeaveDetails.Tables[2].Rows.Count; k++)
                {
                    if (dsLeaveDetails.Tables[2].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                    {

                        strFrom = dsLeaveDetails.Tables[2].Rows[k]["ConfigItemValue"].ToString();
                        break;

                    }
                }

                To = dsLeaveDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
                //From = dsLeaveDetails.Tables[0].Rows[0]["ApproverEmailID"].ToString();
                Cc = dsLeaveDetails.Tables[0].Rows[0]["CompetencyMailID"].ToString();
                From = strFrom;

                for (int i = 0; i < dsLeaveDetails.Tables[1].Rows.Count; i++)
                {
                    if (dsLeaveDetails.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Leave Application on Rejected")
                    {
                        string strSubject = dsLeaveDetails.Tables[1].Rows[i]["EmailSubject"].ToString();
                        string strBody = dsLeaveDetails.Tables[1].Rows[i]["EmailBody"].ToString();

                        strSubject = Regex.Replace(strSubject, "##EmployeeName##", dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        Subject = strSubject;

                        //strBody = "\n" + "Hi ##EmployeeName##," + " \n\n" + " The leave is Rejected for ##ApproverComments##." + " \n\n" + " Approved By: ##ApproverName##";

                        strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);
                        strBody = Regex.Replace(strBody, "##ApproverName##", dsLeaveDetails.Tables[0].Rows[0]["ApproverName"].ToString());
                        strBody = Regex.Replace(strBody, "##EmployeeName##", dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        strBody = Regex.Replace(strBody, "##LeaveDateFrom##", Convert.ToDateTime(dsLeaveDetails.Tables[0].Rows[0]["LeaveDateFrom"].ToString()).ToShortDateString());
                        strBody = Regex.Replace(strBody, "##LeaveDateTo##", Convert.ToDateTime(dsLeaveDetails.Tables[0].Rows[0]["LeaveDateTo"].ToString()).ToShortDateString());
                        strBody = Regex.Replace(strBody, "##LeaveReason##", dsLeaveDetails.Tables[0].Rows[0]["LeaveReason"].ToString());
                        strBody = Regex.Replace(strBody, "##ApproverComments##", dsLeaveDetails.Tables[0].Rows[0]["ApproverComments"].ToString());
                        strBody = Regex.Replace(strBody, "##TotalLeaveDays##", dsLeaveDetails.Tables[0].Rows[0]["TotalLeaveDays"].ToString());
                        strBody = Regex.Replace(strBody, "##LeaveCorrectionDays##", dsLeaveDetails.Tables[0].Rows[0]["LeaveCorrectionDays"].ToString());
                        float TotalLeaves;
                        TotalLeaves = (float)(Convert.ToDouble(dsLeaveDetails.Tables[0].Rows[0]["TotalLeaveDays"].ToString()) + Convert.ToDouble(dsLeaveDetails.Tables[0].Rows[0]["LeaveCorrectionDays"].ToString()));
                        strBody = Regex.Replace(strBody, "##TotalAppliedLeaves##", Convert.ToString(TotalLeaves).ToString());


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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsWF.cs", "caRejected_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }


            //To=dsLeaveDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
            // From=dsLeaveDetails.Tables[0].Rows[0]["ApproverEmailID"].ToString();

            // string strSubject= dsLeaveDetails.Tables[1].Rows[2]["EmailSubject"].ToString();
            // Subject= Regex.Replace(strSubject,"##User##",dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());

            // Body=dsLeaveDetails.Tables[1].Rows[2]["EmailBody"].ToString();
            // Subject= Regex.Replace(strSubject,"##User##",dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());



        }

        private void caLeaveCancel_ExecuteCode(object sender, EventArgs e)
        {
            LeaveDetailsBOL objLeaveDetailsBOL = new LeaveDetailsBOL();
            try
            {

                DataSet dsLeaveDetails = objLeaveDetailsBOL.WFGetLeaveDetails(LeaveDetailID);

                for (int k = 0; k < dsLeaveDetails.Tables[2].Rows.Count; k++)
                {
                    if (dsLeaveDetails.Tables[2].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                    {

                        strFrom = dsLeaveDetails.Tables[2].Rows[k]["ConfigItemValue"].ToString();
                        break;

                    }
                }

                To = dsLeaveDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
                //From = dsLeaveDetails.Tables[0].Rows[0]["ApproverEmailID"].ToString();
                Cc = dsLeaveDetails.Tables[0].Rows[0]["CompetencyMailID"].ToString();
                From = strFrom;

                for (int i = 0; i < dsLeaveDetails.Tables[1].Rows.Count; i++)
                {
                    if (dsLeaveDetails.Tables[1].Rows[i]["EmailTemplateName"].ToString() == "Leave Application on Cancelled")
                    {
                        string strSubject = dsLeaveDetails.Tables[1].Rows[i]["EmailSubject"].ToString();
                        string strBody = dsLeaveDetails.Tables[1].Rows[i]["EmailBody"].ToString();

                        strSubject = Regex.Replace(strSubject, "##EmployeeName##", dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        Subject = strSubject;

                        //strBody = "\n" + "Hi ##EmployeeName##," + " \n\n" + " As requested your leave is canceled and the required updates are made in the system." + " \n\n" + "  Approved By: ##ApproverName##";

                        strBody = Regex.Replace(strBody, "##Environment##", Environment.NewLine);
                        strBody = Regex.Replace(strBody, "##ApproverName##", dsLeaveDetails.Tables[0].Rows[0]["ApproverName"].ToString());
                        strBody = Regex.Replace(strBody, "##EmployeeName##", dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());
                        strBody = Regex.Replace(strBody, "##LeaveDateFrom##", Convert.ToDateTime(dsLeaveDetails.Tables[0].Rows[0]["LeaveDateFrom"].ToString()).ToShortDateString());
                        strBody = Regex.Replace(strBody, "##LeaveDateTo##", Convert.ToDateTime(dsLeaveDetails.Tables[0].Rows[0]["LeaveDateTo"].ToString()).ToShortDateString());
                        strBody = Regex.Replace(strBody, "##LeaveReason##", dsLeaveDetails.Tables[0].Rows[0]["LeaveReason"].ToString());
                        strBody = Regex.Replace(strBody, "##ApproverComments##", dsLeaveDetails.Tables[0].Rows[0]["ApproverComments"].ToString());
                        strBody = Regex.Replace(strBody, "##TotalLeaveDays##", dsLeaveDetails.Tables[0].Rows[0]["TotalLeaveDays"].ToString());
                        strBody = Regex.Replace(strBody, "##LeaveCorrectionDays##", dsLeaveDetails.Tables[0].Rows[0]["LeaveCorrectionDays"].ToString());
                        float TotalLeaves;
                        TotalLeaves = (float)(Convert.ToDouble(dsLeaveDetails.Tables[0].Rows[0]["TotalLeaveDays"].ToString()) + Convert.ToDouble(dsLeaveDetails.Tables[0].Rows[0]["LeaveCorrectionDays"].ToString()));
                        strBody = Regex.Replace(strBody, "##TotalAppliedLeaves##", Convert.ToString(TotalLeaves).ToString());


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
                objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDetailsWF.cs", "caLeaveCancel_ExecuteCode", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
            //To=dsLeaveDetails.Tables[0].Rows[0]["EmployeeEmailID"].ToString();
            // From=dsLeaveDetails.Tables[0].Rows[0]["ApproverEmailID"].ToString();

            // string strSubject= dsLeaveDetails.Tables[1].Rows[3]["EmailSubject"].ToString();
            // Subject= Regex.Replace(strSubject,"##User##",dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());

            // Body=dsLeaveDetails.Tables[1].Rows[3]["EmailBody"].ToString();
            // Subject= Regex.Replace(strSubject,"##User##",dsLeaveDetails.Tables[0].Rows[0]["EmployeeName"].ToString());


        }
    }

}
