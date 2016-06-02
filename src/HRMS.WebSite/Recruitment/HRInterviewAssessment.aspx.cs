using BLL;
using BOL;
using MailActivity;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HRInterviewAssessment : System.Web.UI.Page
{
    private static DataSet dsCandidateDetails = new DataSet();
    private HRInterviewAssessmentBLL objHRInterviewAssessmentBLL = new HRInterviewAssessmentBLL();
    private HRInterviewAssessmentBOL objHRInterviewAssessmentBOL = new HRInterviewAssessmentBOL();

    private RRFApproverBLL objRRFApproverBLL = new RRFApproverBLL();
    private RRFApproverBOL objRRFApproverBOL = new RRFApproverBOL();
    private EmailActivityBLL objEmailActivityBLL = new EmailActivityBLL();
    private EmailActivityBOL objEmailActivityBOL = new EmailActivityBOL();
    private EmailActivity objEmailActivity = new EmailActivity();

    private DataSet dsGetMailInfo = new DataSet();
    private DataSet dsGetEmployeeFromRole = new DataSet();

    private int RRFID, CandidateID, ScheduleID, RoundNumber, SrNo, stageID;
    private string Feedback, Mode;
    private static string EmailId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString.Count > 0)
        {
            RRFID = Convert.ToInt32(Request.QueryString["RRFID"]);
            CandidateID = Convert.ToInt32(Request.QueryString["CandidateID"]);
            ScheduleID = Convert.ToInt32(Request.QueryString["ScheduleID"]);
            Mode = Convert.ToString(Request.QueryString["Mode"]);
            RoundNumber = Convert.ToInt32(Request.QueryString["RoundNumber"]);
            stageID = Convert.ToInt32(Request.QueryString["StageID"]);
        }
        else
        {
            try
            {
                if (Session["RRFID"] != null)
                    RRFID = Convert.ToInt32(Convert.ToString(Session["RRFID"]));
                if (Session["CandidateID"] != null)
                    CandidateID = Convert.ToInt32(Convert.ToString(Session["CandidateID"]));
                if (Session["ScheduleID"] != null)
                    ScheduleID = Convert.ToInt32(Convert.ToString(Session["ScheduleID"]));
                if (Session["Mode"] != null)
                    Mode = Convert.ToString(Session["Mode"]);
                if (Session["RoundNumber"] != null)
                    RoundNumber = Convert.ToInt32(Convert.ToString(Session["RoundNumber"]));
                if (Session["StageID"] != null)
                {
                    object Test = Session["StageID"];
                    string Res = Test.GetType().ToString();
                    if (Res.Contains("System.Web.UI.WebControls.Label"))
                    {
                        object newobj = Session["StageID"];
                        var Testing = newobj as System.Web.UI.WebControls.Label;
                        Session["StageID"] = Testing.Text;
                    }
                    stageID = Convert.ToInt32(Convert.ToString(Session["StageID"]));
                }
            }
            catch (Exception errObj)
            {
                //Used To Handle Null Exception But Session Gets Expired So  We Direct it to the Login Page. [PJ]
                if (errObj.Message.Contains("Input string was not in a correct format"))
                    Response.Redirect("http://" + Request.Url.Authority.ToString());
            }
        }
        if (!Page.IsPostBack)
            BindData();
    }

    public void BindData()
    {
        try
        {
            if (stageID == 18)
            {
                lblWelcome.Text = "Group Head Form";
                lblComments.Text = "Group Head Comments";
            }
            else if (stageID == 6)
            {
                lblWelcome.Text = "HR Interview Assessment Form";
                lblComments.Text = "HR Assessment Comments";
            }

            objHRInterviewAssessmentBOL.RRFID = RRFID;
            objHRInterviewAssessmentBOL.CandidateID = CandidateID;
            objHRInterviewAssessmentBOL.ScheduleID = ScheduleID;
            if (Mode == "Read")
                objHRInterviewAssessmentBOL.Mode = Mode;
            else
                objHRInterviewAssessmentBOL.Mode = "Write";

            //Mode = "Read";
            //objHRInterviewAssessmentBOL.Mode = "Read";

            dsCandidateDetails = objHRInterviewAssessmentBLL.GetCandidateDetails(objHRInterviewAssessmentBOL);
            if (dsCandidateDetails != null && dsCandidateDetails.Tables[0].Rows.Count > 0)
            {
                if (dsCandidateDetails.Tables[0].Rows.Count > 0)
                {
                    //lblInterviewedBy.Text =  Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                    lblCandidateName.Text = dsCandidateDetails.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsCandidateDetails.Tables[0].Rows[0]["LastName"].ToString();
                    lblRecruiterName.Text = dsCandidateDetails.Tables[0].Rows[0]["Recruiter"].ToString();
                    lblDepartment.Text = dsCandidateDetails.Tables[0].Rows[0]["Department"].ToString();
                    lblPosition.Text = dsCandidateDetails.Tables[0].Rows[0]["Designation"].ToString();
                    lblTotalExp.Text = dsCandidateDetails.Tables[0].Rows[0]["TotalExpYrs"].ToString() + " Years " + dsCandidateDetails.Tables[0].Rows[0]["TotalExpMonths"].ToString() + " months ";
                    lblRelevantExp.Text = dsCandidateDetails.Tables[0].Rows[0]["RelevantExpYrs"].ToString() + " Years " + dsCandidateDetails.Tables[0].Rows[0]["RelevantExpMonths"].ToString() + " months ";
                    lblNoticePeriod.Text = dsCandidateDetails.Tables[0].Rows[0]["NoticePeriod"].ToString();
                    lblInterviewedBy.Text = dsCandidateDetails.Tables[0].Rows[0]["Interviewer"].ToString();

                    if (Mode == "Read")
                    {
                        //Set all fields as enabled = false and hide buttons
                        btnApprove.Visible = false;
                        btnReject.Visible = false;
                        txtComments.Enabled = false;
                        //btnPrint.Visible = true;
                        int[] rating = new int[7];

                        var otPersonlity = dsCandidateDetails.Tables[0].Rows[0]["Personality"];
                        if (!(otPersonlity is DBNull)) { rating[0] = Convert.ToInt32(dsCandidateDetails.Tables[0].Rows[0]["Personality"]); }
                        var otClarity = dsCandidateDetails.Tables[0].Rows[0]["Clarity"];
                        if (!(otClarity is DBNull)) { rating[1] = Convert.ToInt32(dsCandidateDetails.Tables[0].Rows[0]["Clarity"]); }
                        var otLeadership = dsCandidateDetails.Tables[0].Rows[0]["Leadership"];
                        if (!(otLeadership is DBNull)) { rating[2] = Convert.ToInt32(dsCandidateDetails.Tables[0].Rows[0]["Leadership"]); }
                        var otInterPersonal = dsCandidateDetails.Tables[0].Rows[0]["Interpersonal"];
                        if (!(otInterPersonal is DBNull)) { rating[3] = Convert.ToInt32(dsCandidateDetails.Tables[0].Rows[0]["Interpersonal"]); }
                        var otCommunication = dsCandidateDetails.Tables[0].Rows[0]["Communication"];
                        if (!(otCommunication is DBNull)) { rating[4] = Convert.ToInt32(dsCandidateDetails.Tables[0].Rows[0]["Communication"]); }
                        var otInitiative = dsCandidateDetails.Tables[0].Rows[0]["Initiative"];
                        if (!(otInitiative is DBNull)) { rating[5] = Convert.ToInt32(dsCandidateDetails.Tables[0].Rows[0]["Initiative"]); }
                        var otCarrer = dsCandidateDetails.Tables[0].Rows[0]["Career"];
                        if (!(otCarrer is DBNull)) { rating[6] = Convert.ToInt32(dsCandidateDetails.Tables[0].Rows[0]["Career"]); }

                        for (int i = 1; i <= 7; i++)
                        {
                            for (int j = 1; j <= 5; j++)
                            {
                                string RadioButtonID = "RadioButton" + (i).ToString() + (j).ToString();
                                RadioButton rb = new RadioButton();
                                rb = (RadioButton)form1.FindControl(RadioButtonID);
                                rb.Checked = false;
                                rb.Enabled = false;
                            }
                        }

                        for (int i = 1; i <= 7; i++)
                        {
                            for (int j = 1; j <= 5; j++)
                            {
                                if (rating[i - 1] == j)
                                {
                                    string RadioButtonID = "RadioButton" + (i).ToString() + (j).ToString();
                                    RadioButton rb = new RadioButton();
                                    rb = (RadioButton)form1.FindControl(RadioButtonID);
                                    rb.Checked = true;
                                }
                            }
                        }

                        txtComments.Text = dsCandidateDetails.Tables[0].Rows[0]["Comments"].ToString();
                    }
                    else
                    {
                        btnPrint.Visible = false;
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException("Exception in BindData()." + ex.Message, ex.InnerException);
        }
    }

    protected void btnSubmitAssessment_Click(object sender, EventArgs e)
    {
        Button btnApprove = (Button)sender;
        Feedback = "Approved";

        btnApprove.Enabled = false;
        btnReject.Enabled = false;
        btnPrint.Enabled = true;
        InsertFeedbackData(btnApprove, Feedback);

        objHRInterviewAssessmentBOL.RRFID = RRFID;
        objHRInterviewAssessmentBOL.CandidateID = CandidateID;
        objHRInterviewAssessmentBOL.StageID = stageID;
        objHRInterviewAssessmentBOL.RoundNo = RoundNumber;
        objHRInterviewAssessmentBOL.SrNo = SrNo;
        objHRInterviewAssessmentBLL.UpdateCandidateScheduleDate(objHRInterviewAssessmentBOL);

        this.ClientScript.RegisterStartupScript(GetType(), "CLOSE", "<script language='javascript'> opener.location.href = 'Interviewer.aspx'; window.close(); </script>");
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        Button btnReject = (Button)sender;
        Feedback = "Rejected";

        btnApprove.Enabled = false;
        btnReject.Enabled = false;
        btnPrint.Enabled = true;
        InsertFeedbackData(btnReject, Feedback);

        objHRInterviewAssessmentBOL.RRFID = RRFID;
        objHRInterviewAssessmentBOL.CandidateID = CandidateID;
        objHRInterviewAssessmentBOL.StageID = stageID;
        objHRInterviewAssessmentBOL.RoundNo = RoundNumber;
        objHRInterviewAssessmentBOL.SrNo = SrNo;
        objHRInterviewAssessmentBLL.UpdateCandidateScheduleDate(objHRInterviewAssessmentBOL);

        this.ClientScript.RegisterStartupScript(GetType(), "CLOSE", "<script language='javascript'> opener.location.href = 'Interviewer.aspx'; window.close(); </script>");
    }

    protected void InsertFeedbackData(Button btn, string Feedback)
    {
        Button btnTemp = btn;
        string RadioButtonID = "";

        for (int i = 1; i <= 7; i++)
        {
            for (int j = 1; j <= 5; j++)
            {
                RadioButtonID = "RadioButton" + (i).ToString() + (j).ToString();

                RadioButton rb = (RadioButton)btnTemp.Parent.FindControl(RadioButtonID);

                if (rb.Checked)
                {
                    string grp = rb.GroupName;
                    switch (grp)
                    {
                        case "rdoPersonalityGroup": objHRInterviewAssessmentBOL.Personality = j;
                            break;

                        case "rdoClarityGroup": objHRInterviewAssessmentBOL.Clarity = j;
                            break;

                        case "rdoLeadershipGroup": objHRInterviewAssessmentBOL.Leadership = j;
                            break;

                        case "rdoInterPersonalGroup": objHRInterviewAssessmentBOL.Interpersonal = j;
                            break;

                        case "rdoCommunicationGroup": objHRInterviewAssessmentBOL.Communication = j;
                            break;

                        case "rdoInitiativeGroup": objHRInterviewAssessmentBOL.Initiative = j;
                            break;

                        case "rdoCareerGroup": objHRInterviewAssessmentBOL.Career = j;
                            break;
                    }
                }
            }
        }

        objHRInterviewAssessmentBOL.StageID = stageID; //HR round
        objHRInterviewAssessmentBOL.ScheduleID = ScheduleID;
        objHRInterviewAssessmentBOL.CandidateID = CandidateID;
        objHRInterviewAssessmentBOL.RRFID = RRFID;
        objHRInterviewAssessmentBOL.SrNo = SrNo;
        objHRInterviewAssessmentBOL.FeedbackBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

        objHRInterviewAssessmentBOL.Mode = "Write";
        dsCandidateDetails = objHRInterviewAssessmentBLL.GetCandidateDetails(objHRInterviewAssessmentBOL);

        if (dsCandidateDetails != null && dsCandidateDetails.Tables[0].Rows.Count > 0)
        {
            objHRInterviewAssessmentBOL.Department = Convert.ToInt32(dsCandidateDetails.Tables[0].Rows[0]["DepartmentID"]);
            objHRInterviewAssessmentBOL.Position = Convert.ToInt32(dsCandidateDetails.Tables[0].Rows[0]["DesignationID"]);
        }
        objHRInterviewAssessmentBOL.Feedback = Feedback;
        objHRInterviewAssessmentBOL.HRMComments = txtComments.Text;

        objHRInterviewAssessmentBLL.HRInterviewAssessment(objHRInterviewAssessmentBOL);

        btnApprove.Visible = false;
        btnReject.Visible = false;
        //btnPrint.Visible = true;
        //Mailing Activity

        objRRFApproverBOL.Role = "HRM";
        dsGetEmployeeFromRole = objRRFApproverBLL.GetEmployeeFromRole(objRRFApproverBOL);
        string toID = dsGetEmployeeFromRole.Tables[0].Rows[0]["UserId"].ToString();
        for (int i = 1; i < dsGetEmployeeFromRole.Tables[0].Rows.Count; i++)
            toID = toID + ';' + dsGetEmployeeFromRole.Tables[0].Rows[i]["UserId"].ToString();
        if (dsCandidateDetails.Tables[0].Rows.Count > 0 && dsCandidateDetails != null)
            objEmailActivityBOL.ToID = dsCandidateDetails.Tables[0].Rows[0]["RecruiterID"].ToString() + ";";//recruiter

        objEmailActivityBOL.CCID = toID + ";" + Convert.ToString(HttpContext.Current.User.Identity.Name) + ";";//HRM,Interviewer

        objEmailActivityBOL.FromID = Convert.ToInt32(HttpContext.Current.User.Identity.Name);//Interviewer
        objEmailActivityBOL.EmailTemplateName = "Interview Feedback";
        dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);
        DataSet dsmaildetails = new DataSet();
        dsmaildetails = objHRInterviewAssessmentBLL.GetDetailsformail(objHRInterviewAssessmentBOL);

        string body, RRFNomail;

        RRFNomail = Convert.ToString(objHRInterviewAssessmentBOL.RRFID);

        body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
        //body = body.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
        //body = body.Replace("##skills##", (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString()));
        body = body.Replace("##comment##", txtComments.Text);
        body = body.Replace("##Candidate Name##", lblCandidateName.Text);
        body = body.Replace("##InterviewDate##", (dsmaildetails.Tables[0].Rows[0]["ScheduledDatetime"].ToString()));

        char[] separator = new char[] { ';' };
        objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
        objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
        objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
        objEmailActivityBOL.Body = body; // (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
        objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
        objEmailActivityBOL.RRFNo = (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString());
        objEmailActivityBOL.skills = (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString());
        objEmailActivityBOL.Position = (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString());
        try
        {
            objEmailActivity.SendMail(objEmailActivityBOL);

            lblSuccess.Text = "Interview Feedback submitted successfully.";
            lblSuccess.Visible = true;
        }
        catch (System.Exception ex)
        {
            lblSuccess.Text = "Interview Feedback submitted successfully,but e-mails could not be sent.";
            lblSuccess.Visible = true;
        }
    }

    protected void confirmPrint(object sender, EventArgs e)
    {
        //ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript type='text/javascript'>var mywindow = window.open();mywindow.document.write('<html><head><title>Group Head Form</title>');mywindow.document.write('<link type='text/css' rel='stylesheet' href=''></link>'); mywindow.document.write('</head><body >');mywindow.document.write($('#tblMain').html());mywindow.document.write('</body></html>');mywindow.print();mywindow.close();</script>");
        // ClientScript.RegisterStartupScript(this.GetType(), "onclick", "PrintDocument();");
    }
}