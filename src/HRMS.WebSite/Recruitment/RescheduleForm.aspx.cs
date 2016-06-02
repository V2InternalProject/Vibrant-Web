using BLL;
using BOL;
using MailActivity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CandidateInterviewScheduleForm
{
    public partial class RescheduleForm : System.Web.UI.Page
    {
        private RescheduledBLL objRescheduledBLL = new RescheduledBLL();
        private RescheduledBOL objRescheduledBOL = new RescheduledBOL();
        private int RoundNumber;
        private static string stageName = string.Empty;
        private static DataSet dsInterViewer = new DataSet();
        private static int employeeId = 0;
        private static int InterviewerId;

        private DataSet dsGetMailInfo = new DataSet();
        private DataSet dsGetEmployeeFromRole = new DataSet();
        private int mailFlag;
        private int redirectClickFlag;

        private RRFApproverBLL objRRFApproverBLL = new RRFApproverBLL();
        private RRFApproverBOL objRRFApproverBOL = new RRFApproverBOL();
        private EmailActivityBLL objEmailActivityBLL = new EmailActivityBLL();
        private EmailActivityBOL objEmailActivityBOL = new EmailActivityBOL();
        private EmailActivity objEmailActivity = new EmailActivity();

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["InterviewerID"] != null || Session["RecruiterId"] != null)
            //{
            //    string a = Convert.ToString(Session["InterviewerID"]);
            //    string b = Convert.ToString(Session["RecruiterId"]);
            //}

            try
            {
                // txtNewDate.Attributes.Add("OnKeyPress", "GetKeyPress()");
                if (!IsPostBack)
                {
                    //txtRRFNo.Text = Session["RRFNo"].ToString();
                    //txtPosition.Text = Session["Position"].ToString();
                    //txtCandidateName.Text = Session["CandidateName"].ToString();
                    //txtStage.Text = Session["StageName"].ToString();

                    //stageName = txtStage.Text;
                    txtNewInterviewer.Focus();
                    //InterviewerId = Convert.ToInt32(Request.QueryString["InterviewerId"]);
                    InterviewerId = Convert.ToInt32(Convert.ToString(Session["InterviewerEmployeeId"]));
                    lblRRFNO.Text = Session["RRFNo"].ToString();
                    lblPosition.Text = Session["Position"].ToString();
                    lblCandidateName.Text = Session["CandidateName"].ToString();
                    lblStage.Text = Session["StageName"].ToString();

                    stageName = lblStage.Text;

                    lblScheduledDate.Text = Session["ScheduledDate"].ToString();
                    lblScheduledTime.Text = Session["ScheduledTime"].ToString();
                    lblInterviewerold.Text = Session["InterviewerName"].ToString();
                    RoundNumber = Convert.ToInt32(Session["RoundNumber"].ToString());
                    redirectClickFlag = 1;
                    Session["redirectClickFlag"] = redirectClickFlag;
                }
                else
                {
                    RoundNumber = Convert.ToInt32(Session["RoundNumber"].ToString());
                }
            }
            catch (Exception errObj)
            {
                if (errObj.Message.Contains("Input string was not in a correct format"))
                    Response.Redirect("http://" + Request.Url.Authority.ToLower().ToString());
            }
        }

        protected void ddlNewTimeHours_Load(object sender, EventArgs e)
        {
            DropDownList ddlNewTimeHours = (DropDownList)sender;

            if (ddlNewTimeHours.Items.Count > 0)
            {
                int s = ddlNewTimeHours.SelectedIndex;
                ddlNewTimeHours.Items.Clear();
                for (int i = 1; i <= 24; i++)
                {
                    string hrs = string.Empty;
                    if (i < 10)
                        hrs = "0" + Convert.ToString(i);
                    else
                        hrs = Convert.ToString(i);

                    ddlNewTimeHours.Items.Add(hrs);
                }
                ddlNewTimeHours.SelectedIndex = s;
            }
            else
            {
                for (int i = 1; i <= 24; i++)
                {
                    string l = string.Empty;
                    if (i < 10)
                        l = "0" + Convert.ToString(i);
                    else
                        l = Convert.ToString(i);

                    ddlNewTimeHours.Items.Add(l);
                }
            }
        }

        protected void ddlNewTimeHours_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = " ";
            int i = 0;
            DropDownList ddlNewTimeHours = (DropDownList)sender;
            if (lblNewTime.Text == "")
                lblNewTime.Text = ddlNewTimeHours.SelectedItem.Value.ToString();
            else if (lblNewTime.Text.Contains(":"))
            {
                i = lblNewTime.Text.IndexOf(":");
                s = lblNewTime.Text.Substring(i, 3);
                lblNewTime.Text = ddlNewTimeHours.SelectedItem.Value.ToString() + s;
            }
        }

        protected void ddlNewTimeMinutes_Load(object sender, EventArgs e)
        {
            DropDownList ddlNewTimeMin = (DropDownList)sender;
            if (ddlNewTimeMin.Items.Count > 0)
            {
                int s = ddlNewTimeMin.SelectedIndex;
                ddlNewTimeMin.Items.Clear();
                for (int i = 0; i <= 59; i++)
                {
                    string min = string.Empty;

                    if (i < 10)
                        min = "0" + Convert.ToString(i);
                    else
                        min = Convert.ToString(i);

                    ddlNewTimeMin.Items.Add(min);
                }
                ddlNewTimeMin.SelectedIndex = s;
            }
            else
            {
                for (int i = 0; i <= 59; i++)
                {
                    string min = string.Empty;

                    if (i < 10)
                        min = "0" + Convert.ToString(i);
                    else
                        min = Convert.ToString(i);

                    ddlNewTimeMin.Items.Add(min);
                }
            }
        }

        protected void ddlNewTimeMinutes_SelectedIndexChanged(object sender, EventArgs e)
        {
            String s = " ";
            DropDownList ddlNewTimeMin = (DropDownList)sender;
            if (lblNewTime.Text == "")
                lblNewTime.Text = ":" + ddlNewTimeMin.SelectedItem.Value.ToString();
            else if (lblNewTime.Text.Contains(":"))
            {
                s = lblNewTime.Text.Substring(0, 2);
                lblNewTime.Text = s + ":" + ddlNewTimeMin.SelectedItem.Value.ToString();
            }
            else
            {
                s = lblNewTime.Text.Substring(0, 2);
                lblNewTime.Text = s + ":" + ddlNewTimeMin.SelectedItem.Value.ToString();
            }
        }

        private static DataSet ds = new DataSet();
        private static RescheduledBLL objReschedulebll = new RescheduledBLL();

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetEmployeeName(string prefixText)
        {
            List<string> IndPanNames = new List<string>();
            string ddlStageName = stageName;
            string RoleName = string.Empty;
            if (!string.IsNullOrEmpty(ddlStageName))
            {
                if (ddlStageName.Trim() == "Final Stage")
                {
                    RoleName = "HRM";
                }
                else if (stageName.Trim() == "HR Interview")
                {
                    RoleName = "HRM,Interviewer";
                }
                else
                {
                    RoleName = "Interviewer";
                }

                ds = objReschedulebll.GetEmployeeName(prefixText, RoleName);
                dsInterViewer = ds;

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        IndPanNames.Add(ds.Tables[0].Rows[i][1].ToString());
                    }
                }
            }
            return IndPanNames;
        }

        protected void btnReschedule_Click(object sender, EventArgs e)
        {
            //if (Convert.ToInt32(Session["redirectClickFlag"]) > 1)
            //{
            //    Response.Redirect("~/CandidateInterviewSchedule.aspx");
            //}
            //else
            //{
            Session["redirectClickFlag"] = Convert.ToInt32(Session["redirectClickFlag"]) + 1;

            System.Data.DataSet ds = new DataSet();
            objRescheduledBOL.CandidateID = Convert.ToInt32(Session["CandidateID"]);
            objRescheduledBOL.RRFNo = Convert.ToInt32(Session["RRFID"]);
            objRescheduledBOL.Stage = Convert.ToString(lblStage.Text);

            string expireOnDate = txtNewDate.Text;
            int pos1 = expireOnDate.IndexOf("/");
            int pos2 = expireOnDate.IndexOf("/", pos1 + 1);

            int strDay;
            int strMonth;
            if (Convert.ToInt32(expireOnDate.Substring(0, (pos1 - 1))) == 0)
                strMonth = Convert.ToInt32(expireOnDate.Substring(1, (pos1 - 1)));
            else
                strMonth = Convert.ToInt32(expireOnDate.Substring(0, (pos1)));

            if (Convert.ToInt32(expireOnDate.Substring((pos1 + 1), 1)) == 0)
                strDay = Convert.ToInt32(expireOnDate.Substring((pos1 + 2), 1));
            else
                strDay = Convert.ToInt32(expireOnDate.Substring((pos1 + 1), 2));

            int strYear = Convert.ToInt32(expireOnDate.Substring((pos2 + 1)));

            DateTime expectedClosureDate = new DateTime(strYear, strMonth, strDay);

            objRescheduledBOL.ScheduledDateTime = expectedClosureDate.AddHours(Convert.ToDouble(ddlNewTimeHours.SelectedValue.ToString())).AddMinutes(Convert.ToDouble(ddlNewTimeMinutes.SelectedItem.Value.ToString()));

            objRescheduledBOL.Position = Convert.ToString(lblPosition.Text);

            objRescheduledBOL.Requestor = Convert.ToString(Session["Requestor"]);

            int isInterviewerNameEmpty = 1;
            if (txtNewInterviewer.Text != "")
            {
                if (dsInterViewer != null)
                {
                    if (dsInterViewer.Tables[0].Rows.Count > 0 && dsInterViewer.Tables.Count > 0)
                    {
                        for (int i = 0; i < dsInterViewer.Tables[0].Rows.Count; i++)
                        {
                            if (txtNewInterviewer.Text == dsInterViewer.Tables[0].Rows[i]["EmployeeName"].ToString())
                            {
                                employeeId = Convert.ToInt32(dsInterViewer.Tables[0].Rows[i]["UserID"]);
                                isInterviewerNameEmpty = 1;
                                break;
                            }
                            else
                            {
                                isInterviewerNameEmpty = 0;
                            }
                        }
                        if (isInterviewerNameEmpty == 1)
                            objRescheduledBOL.InterviewerName = employeeId;
                        else
                        {
                            lblInterviewer.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lblInterviewer.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                objRescheduledBOL.InterviewerName = InterviewerId;
            }

            //objRescheduledBOL.Schdu
            //  objRescheduledBOL.ScheduledBy = Convert.ToInt32(Session["UserId"]);
            objRescheduledBOL.ScheduledBy = Convert.ToInt32(Session["RecruiterId"]);
            objRescheduledBOL.RescheduledBy = Convert.ToString(ddlRescheduledby.SelectedItem.Text);
            objRescheduledBOL.RescheduledReason = Convert.ToString(txtReason.Text);

            objRescheduledBLL.SetCandidateSchecule(objRescheduledBOL, RoundNumber);

            //Mailing Activity

            objRRFApproverBOL.Role = "HRM";
            dsGetEmployeeFromRole = objRRFApproverBLL.GetEmployeeFromRole(objRRFApproverBOL);
            string toID = dsGetEmployeeFromRole.Tables[0].Rows[0]["UserId"].ToString();
            for (int i = 1; i < dsGetEmployeeFromRole.Tables[0].Rows.Count; i++)
                toID = toID + ';' + dsGetEmployeeFromRole.Tables[0].Rows[i]["UserId"].ToString();
            if (employeeId == Convert.ToInt32(InterviewerId))
                objEmailActivityBOL.ToID = Convert.ToString(employeeId) + ";";//new interviewer
            else
                objEmailActivityBOL.ToID = Convert.ToString(InterviewerId) + ";";//old interviewer

            //objEmailActivityBOL.CCID = Convert.ToString(Session["RecruiterId"]) + ";" + Convert.ToString(toID) + ";" + Convert.ToString(InterviewerId) + ";";//recruiter,HRM,old interviewer
            objEmailActivityBOL.CCID = Convert.ToString(toID);
            objEmailActivityBOL.FromID = Convert.ToInt32(Session["RecruiterId"]);//recruiter
            objEmailActivityBOL.EmailTemplateName = "Reschedule Interview";
            dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);

            string interviewTime, interviewDate, body, bodyForCandidate;
            interviewDate = String.Format("{0:dddd, MMMM d, yyyy}", expectedClosureDate);
            interviewTime = String.Format("{0:t}", objRescheduledBOL.ScheduledDateTime);

            //string scheduleDate = lblScheduledDate.Text;
            //int pos1 = expireOnDate.IndexOf("/");
            //int pos2 = expireOnDate.IndexOf("/", pos1 + 1);

            //int strDay;
            //int strMonth;
            //if (Convert.ToInt32(expireOnDate.Substring(0, (pos1 - 1))) == 0)
            //    strMonth = Convert.ToInt32(expireOnDate.Substring(1, (pos1 - 1)));
            //else
            //    strMonth = Convert.ToInt32(expireOnDate.Substring(0, (pos1)));

            //if (Convert.ToInt32(expireOnDate.Substring((pos1 + 1), 1)) == 0)
            //    strDay = Convert.ToInt32(expireOnDate.Substring((pos1 + 2), 1));
            //else
            //    strDay = Convert.ToInt32(expireOnDate.Substring((pos1 + 1), 2));

            //int strYear = Convert.ToInt32(expireOnDate.Substring((pos2 + 1)));

            //DateTime expectedClosureDate = new DateTime(strYear, strMonth, strDay);

            DataSet dsmaildetails = new DataSet();
            dsmaildetails = objRescheduledBLL.GetDetailsformail(objRescheduledBOL);

            body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            body = body.Replace("##InterviewTime##", interviewTime);
            body = body.Replace("##InterviewDate##", interviewDate);
            body = body.Replace("##OriginalDate##", String.Format("{0:dddd, MMMM d, yyyy}", Convert.ToDateTime(lblScheduledDate.Text)));
            body = body.Replace("##OriginalTime##", String.Format("{0:t}", Convert.ToDateTime(lblScheduledTime.Text)));
            //body = body.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
            //body = body.Replace("##skills##", (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString()));
            body = body.Replace("##comment##", txtReason.Text);
            bodyForCandidate = body;
            //String.Format("{0:dddd, MMMM d, yyyy}", dt);

            char[] separator = new char[] { ';' };
            objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
            objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
            objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
            //body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            //body = body.Replace("##date##", String.Format("{0:f}", objRescheduledBOL.ScheduledDateTime));

            objEmailActivityBOL.Body = body;
            objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
            try
            {
                body = body.Replace("##CandidateName##", lblCandidateName.Text);
                objEmailActivityBOL.Body = body;
                //objEmailActivity.SendMail(objEmailActivityBOL);
                for (int i = 0; i < objEmailActivityBOL.ToAddress.Length; i++)
                    objEmailActivityBOL.ToAddress[i] = string.Empty;
                objEmailActivityBOL.ToAddress[0] = Convert.ToString(Session["EmailId"].ToString());
                objEmailActivityBOL.CandidateName = Convert.ToString(Session["CandidateName"].ToString());
                objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
                objEmailActivityBOL.RRFNo = (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString());
                objEmailActivityBOL.skills = (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString());
                objEmailActivityBOL.Position = (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString());
                objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
                objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##skills##", (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString()));
                objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##CandidateName##", lblCandidateName.Text);
                bodyForCandidate = bodyForCandidate.Replace("##CandidateName##", lblCandidateName.Text);
                objEmailActivityBOL.Body = bodyForCandidate;
                objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
                objEmailActivity.SendMail(objEmailActivityBOL);
                ScriptManager.RegisterStartupScript(this, typeof(string), "CandidateReSchedule", "javascript:CandidateRescheduleLoader();", true);
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("Mailbox unavailable. The server response was: 5.1.1 User unknown"))
                {
                    //This Is used to ignore error related to invalid Email Address.But Emails Will be send to other valid users.
                }
                else
                {
                    mailFlag = 1;
                    lblMessage.Text = "Interview Rescheduled,but e-mails could not be sent.";
                    lblMessage.Visible = true;
                }
            }
            finally
            {
                if (mailFlag == 1)
                {
                    Session["Message"] = "Email could not be send to the candidate";
                    Response.Redirect("~/Recruitment/CandidateInterviewSchedule.aspx");
                    //Response.Redirect("~/CandidateInterviewSchedule.aspx?Message=Email could not be send to the candidate");
                }
                else
                {
                    Response.Redirect("~/Recruitment/CandidateInterviewSchedule.aspx");
                }
            }
        }

        // }
    }
}