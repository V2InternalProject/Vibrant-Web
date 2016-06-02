using BLL;
using BOL;
using MailActivity;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SelectedCandidate : System.Web.UI.Page
{
    private string RRFID = string.Empty;
    private string CandidateID = string.Empty;
    private string offerdPosition = string.Empty;
    private string desigation = string.Empty;
    private static string candidateemailid = string.Empty;
    private static string recruiterid = string.Empty;
    private DataSet DsSelectedCandidate = new DataSet();
    private SelectedCandidateBLL objSelectedCandidateBLL = new SelectedCandidateBLL();
    private SelectedCandidateBOL objSelectedCandidateBOL = new SelectedCandidateBOL();
    private int mailFlag;
    private RRFApproverBLL objRRFApproverBLL = new RRFApproverBLL();
    private RRFApproverBOL objRRFApproverBOL = new RRFApproverBOL();
    private EmailActivityBLL objEmailActivityBLL = new EmailActivityBLL();
    private EmailActivityBOL objEmailActivityBOL = new EmailActivityBOL();
    private EmailActivity objEmailActivity = new EmailActivity();
    private DataSet dsGetMailInfo = new DataSet();
    private DataSet dsGetEmployeeFromRole = new DataSet();
    private DataSet dsGetFinalScore = new DataSet();

    private string ScheduleID = string.Empty;
    private string MainScheduleID = string.Empty;
    private int StageID;
    private string mode = string.Empty;
    private string Flag = string.Empty;
    private int FeedBackBy;
    private int i = 0;
    private int RoundNumber;
    private string RRFShowNumber = string.Empty;
    private string Message = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        FeedBackBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
        //RRFShowNumber = (Request.QueryString["RRFNumber"] ?? "").Trim();
        //CandidateID = Request.QueryString["CandidateID"].ToString();
        //RoundNumber = Convert.ToInt32(Request.QueryString["CandidateID"].ToString());
        //RRFID = Request.QueryString["RRFID"].ToString();
        //string StageName = (Request.QueryString["StageName"] ?? "").Trim();
        //string flag = (Request.QueryString["flag"] ?? "").Trim();
        //offerdPosition = (Request.QueryString["DesignationName"] ?? "").Trim();
        //mode = (Request.QueryString["Mode"] ?? "").Trim();
        //ScheduleID = (Request.QueryString["ScheduleID"] ?? "").Trim();
        //StageID = Convert.ToInt32((Request.QueryString["StageID"] ?? ""));
        //Flag = (Request.QueryString["Flag"] ?? "").Trim();
        //Message = (Request.QueryString["Message"] ?? "").Trim();
        string StageName = "", flag, Res;
        try
        {
            RRFShowNumber = (Convert.ToString(Session["RRFNumber"]) ?? "").Trim();
            CandidateID = Convert.ToString(Session["CandidateID"]);
            RoundNumber = Convert.ToInt32(CandidateID);
            RRFID = Convert.ToString(Session["RRFID"]);
            StageName = (Convert.ToString(Session["StageName"]) ?? "").Trim();
            flag = (Convert.ToString(Session["flag"]) ?? "").Trim();
            offerdPosition = (Convert.ToString(Session["DesignationName"]) ?? "").Trim();
            mode = (Convert.ToString(Session["Mode"]) ?? "").Trim();
            if (Session["MainScheduleID"] == null)
            {
                Session["MainScheduleID"] = (Convert.ToString(Session["ScheduleID"]) ?? "").Trim();
            }
            ScheduleID = (Convert.ToString(Session["ScheduleID"]) ?? "").Trim();

            object Test = Session["StageID"];
            Res = Test.GetType().ToString();
            if (Res.Contains("System.Web.UI.WebControls.Label"))
            {
                object newobj = Session["StageID"];
                var Testing = newobj as System.Web.UI.WebControls.Label;
                Session["StageID"] = Testing.Text;
            }
            StageID = Convert.ToInt32((Convert.ToString(Session["StageID"]) ?? "0"));
            Flag = (Convert.ToString(Session["Flag"]) ?? "").Trim();
            Message = (Convert.ToString(Session["Message"]) ?? "").Trim();
        }
        catch (Exception errObj)
        {
            if (errObj.Message.Contains("Input string was not in a correct format"))
                Response.Redirect("http://" + Request.Url.Authority.ToLower().ToString());
        }
        btnPrint.Visible = false;

        if (!IsPostBack)
        {
            ddlAction.Focus();
            lblStage.Text = StageName;
            lblRRFNo.Text = RRFShowNumber;
            lblPosition.Text = offerdPosition;
            lblCandidate.Text = Session["CandidateName"].ToString();

            objSelectedCandidateBOL.CandidateID = RoundNumber;
            objSelectedCandidateBOL.RRFNo = Convert.ToInt32(RRFID);

            dsGetFinalScore = objSelectedCandidateBLL.GetCandidateScore(objSelectedCandidateBOL);
            lblFinalScore.Text = Convert.ToString(dsGetFinalScore.Tables[0].Rows[0][0].ToString());

            if (string.IsNullOrEmpty(Flag))
            {
                if (!string.IsNullOrEmpty(mode))
                {
                    btnSave.Visible = false;
                    HRMView.Visible = false;
                    btnPrint.Visible = true;
                }
                lblStage.Text = StageName;

                lblRRFNo.Text = Session["RRFNo"].ToString();
                lblPosition.Text = Session["Position"].ToString();
                lblCandidate.Text = Session["CandidateName"].ToString();

                //lblRRFNo.Text = RRFShowNumber;//Session["RRFNo"].ToString();
                lblPosition.Text = offerdPosition;
                lblCandidate.Text = Session["CandidateName"].ToString();

                BindInterviewFeedBack();
                BindGrade();
                BindEmploymentType();
                BindOfferedPosition();
            }
            else
            {
                ShowReadOnlyData();
                //if (string.IsNullOrEmpty(Message))
                //{
                //    lblErrorMsg.Visible = false;
                //}
                //else
                //{
                //    lblErrorMsg.Text = "Feedback entered successfully,but e-mails could not be sent to the candidate.";

                //    lblErrorMsg.Visible = true;
                //}
            }
        }

        //else
        //{
        //    ShowReadOnlyData();

        //}
        // BindInterviewFeedBack();
    }

    public void ShowReadOnlyData()
    {
        //BindPeriod();
        BindGrade();
        BindEmploymentType();
        BindOfferedPosition();
        BindInterviewFeedBack();

        if (!string.IsNullOrEmpty(RRFID) && !string.IsNullOrEmpty(CandidateID))
        {
            objSelectedCandidateBOL.CandidateID = Convert.ToInt32(CandidateID);
            objSelectedCandidateBOL.RRFNo = Convert.ToInt32(RRFID);
            DsSelectedCandidate = objSelectedCandidateBLL.GetSelectedCandidate(objSelectedCandidateBOL);

            int FinalStageRow = DsSelectedCandidate.Tables[0].Rows.Count - 1;

            if (DsSelectedCandidate != null)
            {
                if (DsSelectedCandidate.Tables.Count > 0)
                {
                    if (DsSelectedCandidate.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < DsSelectedCandidate.Tables[0].Rows.Count; j++)
                        {
                            if (Convert.ToInt32(DsSelectedCandidate.Tables[0].Rows[j]["StageID"]) == 17)
                            {
                                //lblRRFNo.Text = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["RRFNo"]);
                                //lblPosition.Text = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["OfferedPosition"]);
                                //lblCandidate.Text = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["CandidateID"]);
                                //lblStage.Text = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["StageID"]);

                                if (Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["Action"]) != null && Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["Action"]) != "")
                                {
                                    if ((DsSelectedCandidate.Tables[0].Rows[j]["Action"].ToString() == "9") || (DsSelectedCandidate.Tables[0].Rows[j]["Action"].ToString() == "15"))
                                    {
                                        ShowHideRow.Visible = false;
                                        HideDate.Visible = false;
                                        //   ddlAction.SelectedItem.Value = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["Action"]);
                                        ddlAction.SelectedIndex = ddlAction.Items.IndexOf(ddlAction.Items.FindByValue(DsSelectedCandidate.Tables[0].Rows[j]["Action"].ToString()));
                                    }
                                    else
                                    {
                                        ShowHideRow.Visible = true;
                                        HideDate.Visible = true;
                                        //ddlAction.SelectedItem.Value = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["Action"]);
                                        ddlAction.SelectedIndex = ddlAction.Items.IndexOf(ddlAction.Items.FindByValue(DsSelectedCandidate.Tables[0].Rows[j]["Action"].ToString()));
                                    }
                                }

                                if (Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["OfferedEmployementType"]) != null && Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["OfferedEmployementType"]) != "")
                                    // ddlEmploymentType.SelectedItem.Value = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["OfferedEmployementType"]);
                                    ddlEmploymentType.SelectedIndex = ddlEmploymentType.Items.IndexOf(ddlEmploymentType.Items.FindByValue(DsSelectedCandidate.Tables[0].Rows[j]["OfferedEmployementType"].ToString()));

                                if (Convert.ToString(DsSelectedCandidate.Tables[0].Rows[0]["OfferedPosition"]) != null && Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["OfferedPosition"]) != "")
                                    ddlOfferedPosition.SelectedItem.Text = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["OfferedPosition"]);

                                if (Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["Grade"]) != null && Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["Grade"]) != "")
                                    ddlGrade.SelectedItem.Text = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["Grade"]);
                                dsGetFinalScore = objSelectedCandidateBLL.GetCandidateScore(objSelectedCandidateBOL);
                                lblFinalScore.Text = Convert.ToString(dsGetFinalScore.Tables[0].Rows[0][0].ToString());
                                txtCTC.Text = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["CTC"]);
                                //txtOther1.Text = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["Other1"]);
                                //txtOther2.Text = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["Other2"]);
                                txtJoiningDate.Text = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["JoiningDate"]);
                                //ddlProbationPeriod.SelectedValue = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[0]["ProbationPeriod"]);

                                if (Convert.ToString(DsSelectedCandidate.Tables[0].Rows[0]["ProbationPeriod"]) != null && Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["ProbationPeriod"]) != "")
                                    txtProbationPeriod.Text = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["ProbationPeriod"]);
                                //ddlProbationPeriod.SelectedValue = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["ProbationPeriod"]);

                                txtComment.Text = Convert.ToString(DsSelectedCandidate.Tables[0].Rows[j]["SelectedComment"]);
                                ddlAction.Enabled = false;
                                ddlEmploymentType.Enabled = false;
                                ddlGrade.Enabled = false;
                                txtCTC.Enabled = false;
                                txtJoiningDate.Enabled = false;
                                txtProbationPeriod.Enabled = false;
                                txtComment.Enabled = false;
                                ddlOfferedPosition.Enabled = false;
                                imgbtnDate.Enabled = false;

                                btnSave.Visible = false;
                                btnPrint.Visible = true;
                            }
                        }
                    }
                }
            }
        }
    }

    public void BindOfferedPosition()
    {
        DataSet dsOfferedPosition = new DataSet();
        dsOfferedPosition = objSelectedCandidateBLL.GetDesignationDetails();
        if (dsOfferedPosition != null)
        {
            ddlOfferedPosition.Items.Clear();
            if (dsOfferedPosition.Tables[0].Rows.Count > 0)
            {
                ddlOfferedPosition.DataSource = dsOfferedPosition.Tables[0];

                ddlOfferedPosition.DataTextField = "Designationname";
                ddlOfferedPosition.DataValueField = "DesignationID";
                ddlOfferedPosition.DataBind();
                ddlOfferedPosition.SelectedIndex = ddlOfferedPosition.Items.IndexOf(ddlOfferedPosition.Items.FindByText(offerdPosition.Trim()));
                //ddlOfferedPosition.SelectedItem.Text = offerdPosition.Trim();
            }
        }
    }

    public void BindEmploymentType()
    {
        DataSet dsEmploymentType = new DataSet();
        dsEmploymentType = objSelectedCandidateBLL.GetEmploymentType();
        if (dsEmploymentType != null)
        {
            if (dsEmploymentType.Tables[0].Rows.Count > 0)
            {
                ddlEmploymentType.DataSource = dsEmploymentType.Tables[0];
                ddlEmploymentType.DataTextField = "EmploymentType";
                ddlEmploymentType.DataValueField = "ID";
                ddlEmploymentType.DataBind();
            }
        }
    }

    public void BindGrade()
    {
        DataSet dsGradeName = new DataSet();
        dsGradeName = objSelectedCandidateBLL.GetGradeName();
        if (dsGradeName != null)
        {
            if (dsGradeName.Tables[0].Rows.Count > 0)
            {
                ddlGrade.DataSource = dsGradeName.Tables[0];
                ddlGrade.DataTextField = "Grade";
                ddlGrade.DataValueField = "GradeID";
                ddlGrade.DataBind();
            }
        }
    }

    public void BindInterviewFeedBack()
    {
        if (!string.IsNullOrEmpty(RRFID) && !string.IsNullOrEmpty(CandidateID))
        {
            if (Session["HRMRole"] != null)
            {
                objSelectedCandidateBOL.FeedBackBy = 0;
            }
            else
            {
                objSelectedCandidateBOL.FeedBackBy = Convert.ToInt32(User.Identity.Name);
            }
            objSelectedCandidateBOL.CandidateID = Convert.ToInt32(CandidateID);
            objSelectedCandidateBOL.RRFNo = Convert.ToInt32(RRFID);
            objSelectedCandidateBOL.ScheduleId = Convert.ToInt32(Session["MainScheduleID"]);
            DsSelectedCandidate = objSelectedCandidateBLL.GetSelectedCandidate(objSelectedCandidateBOL);

            if (DsSelectedCandidate != null)
            {
                if (DsSelectedCandidate.Tables.Count > 0)
                {
                    candidateemailid = DsSelectedCandidate.Tables[1].Rows[0]["Email"].ToString();
                    recruiterid = DsSelectedCandidate.Tables[2].Rows[0]["Recruiter"].ToString();
                }
                if (DsSelectedCandidate.Tables[0].Rows.Count > 0)
                {
                    grdSelectedcandidate.DataSource = DsSelectedCandidate.Tables[0];
                    grdSelectedcandidate.DataBind();
                }
                else
                {
                    grdSelectedcandidate.ShowHeaderWhenEmpty = true;
                    lblMessage.Visible = true;
                    btnShowFeedBack.Visible = false;
                    if (mode == "Read")
                    {
                        lblMessage.Text = "No previous feedback(s) Found";
                    }
                }
            }
        }
    }

    //public void BindPeriod()
    //{
    //    List<int> probationPeriod = new List<int>();

    //    for (int i = 1; i < 100; i++)
    //    {
    //        ddlProbationPeriod.Items.Add(Convert.ToString(i));
    //    }

    //}

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string CTCForMail, CommentforMail;

        objSelectedCandidateBOL.CandidateID = Convert.ToInt32(CandidateID);
        objSelectedCandidateBOL.RRFNo = Convert.ToInt32(RRFID);
        objSelectedCandidateBOL.StageID = StageID;
        objSelectedCandidateBOL.Action = Convert.ToInt32(ddlAction.SelectedItem.Value);
        objSelectedCandidateBOL.FeedBackBy = Convert.ToInt32(User.Identity.Name);

        if (ddlAction.SelectedItem.Value.ToString() == "7")
        {
            decimal CTC = Convert.ToDecimal(txtCTC.Text);

            if (CTC != null)
            {
                objSelectedCandidateBOL.CTC = CTC;
            }
            CTCForMail = Convert.ToString(CTC);
        }
        else
        {
            objSelectedCandidateBOL.CTC = 0;
            CTCForMail = "0";
        }

        string expireOnDate = txtJoiningDate.Text;

        if (!string.IsNullOrEmpty(expireOnDate))
        {
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
            objSelectedCandidateBOL.JoiningDate = expectedClosureDate;
        }
        else
        {
            objSelectedCandidateBOL.JoiningDate = System.DateTime.Now;
        }

        if (string.IsNullOrEmpty(txtProbationPeriod.Text))
        {
            objSelectedCandidateBOL.ProbationPeriod = 0;
        }
        else
        {
            objSelectedCandidateBOL.ProbationPeriod = Convert.ToInt32(txtProbationPeriod.Text);
        }

        objSelectedCandidateBOL.SelectedComment = txtComment.Text;
        CommentforMail = txtComment.Text;
        objSelectedCandidateBOL.Grade = ddlGrade.SelectedItem.ToString();

        objSelectedCandidateBOL.OfferedPosition = Convert.ToInt32(ddlOfferedPosition.SelectedItem.Value);

        objSelectedCandidateBOL.OfferedEmployementType = Convert.ToInt32(ddlEmploymentType.SelectedItem.Value);
        objSelectedCandidateBOL.ScheduleId = Convert.ToInt32(Session["MainScheduleID"]);
        objSelectedCandidateBLL.SetSelectedCandidate(objSelectedCandidateBOL);
        ClearValues();
        int i = 1;

        //Mailing Activity

        objRRFApproverBOL.Role = "HRM";
        dsGetEmployeeFromRole = objRRFApproverBLL.GetEmployeeFromRole(objRRFApproverBOL);
        string toID = dsGetEmployeeFromRole.Tables[0].Rows[0]["UserId"].ToString();
        for (int j = 1; j < dsGetEmployeeFromRole.Tables[0].Rows.Count; j++)
            toID = toID + ';' + dsGetEmployeeFromRole.Tables[0].Rows[j]["UserId"].ToString();
        objEmailActivityBOL.ToID = Convert.ToString(toID) + ";";//candidate

        //objEmailActivityBOL.CCID = Convert.ToString(recruiterid) + ";" + Convert.ToString(toID) + ";";//recruiter,HRM
        objEmailActivityBOL.CCID = Convert.ToString(recruiterid) + ";" + Convert.ToString(HttpContext.Current.User.Identity.Name) + ";";//recruiter,HRM
        objEmailActivityBOL.FromID = Convert.ToInt32(HttpContext.Current.User.Identity.Name);//Interviewer

        if (ddlAction.SelectedItem.Text == "Generate offer")
        {
            objEmailActivityBOL.EmailTemplateName = "Offer Issued";
        }
        else if (ddlAction.SelectedItem.Text == "On Hold")
        {
            objEmailActivityBOL.EmailTemplateName = "On Hold";
        }
        else if (ddlAction.SelectedItem.Text == "Rejected")
        {
            objEmailActivityBOL.EmailTemplateName = "Rejected";
        }

        dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);

        DataSet dsmaildetails = new DataSet();
        dsmaildetails = objSelectedCandidateBLL.GetDetailsformail(objSelectedCandidateBOL);

        string body;

        body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
        //body = body.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
        //body = body.Replace("##skills##", (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString()));
        body = body.Replace("##amount##", CTCForMail);
        //body = body.Replace("##designation##", ddlOfferedPosition.SelectedItem.ToString());
        body = body.Replace("##Candidate Name##", lblCandidate.Text);
        body = body.Replace("##InterviewDate##", (dsmaildetails.Tables[0].Rows[0]["ScheduledDatetime"].ToString()));
        body = body.Replace("##comment##", CommentforMail);

        char[] separator = new char[] { ';' };

        try
        {
            //candidateemailid = candidateemailid + ";";
            //objEmailActivityBOL.ToAddress = Convert.ToString(candidateemailid).Split(separator);
            objEmailActivityBOL.CandidateName = Session["CandidateName"].ToString();
            objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString().Split(separator));
            objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
            objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
            objEmailActivityBOL.Body = body; // (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
            objEmailActivityBOL.RRFNo = (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString());
            objEmailActivityBOL.skills = (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString());
            objEmailActivityBOL.Position = ddlOfferedPosition.SelectedItem.ToString();
            objEmailActivity.SendMail(objEmailActivityBOL);
            ScriptManager.RegisterStartupScript(this, typeof(string), "SelectedCandidate", "javascript:SelectedCandidateLoader();", true);
        }
        catch (System.Exception ex)
        {
            mailFlag = 1;
            lblMessage.Text = "Feedback entered successfully,but e-mails could not be sent.";
            lblMessage.Visible = true;
        }
        finally
        {
            Session["Mode"] = "Read";
            Session["StageID"] = StageID;
            Session["CandidateID"] = CandidateID;
            Session["RRFID"] = RRFID;
            Session["Flag"] = Convert.ToString(i);
            Session["ScheduleID"] = Session["MainScheduleID"];
            Session["StageName"] = lblStage.Text;
            Session["DesignationName"] = offerdPosition;
            Session["RRFNumber"] = lblRRFNo.Text;

            if (mailFlag == 1)
            {
                Session["Message"] = "Feedback entered successfully,but e-mails could not be sent to the candidate";
                Response.Redirect("SelectedCandidate.aspx");
                //Response.Redirect("SelectedCandidate.aspx?Mode=Read&StageID=" + StageID + "&CandidateID=" + CandidateID + "&RRFID=" + RRFID + "&Flag=" + i + "&ScheduleID=" + ScheduleID + "&StageName=" + lblStage.Text + "&DesignationName=" + offerdPosition + "&RRFNumber=" + lblRRFNo.Text + "&Message=Feedback entered successfully,but e-mails could not be sent to the candidate");
            }
            else
            {
                Session["Message"] = "";
                // Response.Redirect("SelectedCandidate.aspx?Mode=Read&StageID=" + StageID + "&CandidateID=" + CandidateID + "&RRFID=" + RRFID + "&Flag=" + i + "&ScheduleID=" + ScheduleID + "&StageName=" + lblStage.Text + "&DesignationName=" + offerdPosition + "&RRFNumber=" + lblRRFNo.Text);
                Response.Redirect("SelectedCandidate.aspx");
            }
        }
    }

    public void ClearValues()
    {
        txtCTC.Text = "";
        //txtOther1.Text = "";
        //txtOther2.Text = "";
        txtJoiningDate.Text = "";
        txtComment.Text = "";
        ddlGrade.SelectedItem.Value = "";
    }

    public void btnShowFeedBack_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdSelectedcandidate.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");

            Label StageID = (Label)gvRow.FindControl("lblStageID");
            Label CandidateID = (Label)gvRow.FindControl("lblCandidateID");
            Label RRFID = (Label)gvRow.FindControl("lblRRFNO");
            Label StageName = (Label)gvRow.FindControl("lblStageName");
            Label InterViewerScheduleID = (Label)gvRow.FindControl("lblScheduleID");

            if (chkSelect.Checked == true)
            {
                Session["CandidateID"] = CandidateID.Text;
                Session["DesignationName"] = offerdPosition;
                Session["StageName"] = StageName.Text;
                Session["RRFID"] = RRFID.Text;
                Session["StageID"] = StageID.Text;

                //if (StageID == "17")
                //    Response.Redirect("SelectedCandidate.aspx?StageName=" + Stagename + "&CandidateID=" + CandidateID + "&RRFID=" + RRFID + "&DesignationName=" + Position + "&Mode=Read");
                if ((StageID.Text == "17"))
                {
                    //  window.open('InterviewFeedback.aspx?CandidateID=' + CandidateID + '&RRFID=' + RRFID + '&ScheduleID=' + ScheduleID + '&StageID=' + StageID + '', null, 'height=700, width=700,status= no, resizable= no, scrollbars=no, toolbar=no,location=no,menubar=no ')

                    //offerdPosition = (Request.QueryString["DesignationName"] ?? "").Trim();
                    //string mode = (Request.QueryString["Mode"] ?? "").Trim();
                    Session["ScheduleID"] = InterViewerScheduleID.Text;
                    //string flag = "1";
                    Session["Mode"] = "";
                    Session["flag"] = "1";

                    //ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", "SelectedCandidate.aspx,null,height=700, width=700,status= no, resizable= yes, scrollbars=no, toolbar=no,location=no,menubar=no"));
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'SelectedCandidate.aspx', null, 'height=900, width=1200,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no' );", true);
                    return;
                }
                else if ((StageID.Text == "6") || (StageID.Text == "18"))
                {
                    Session["Mode"] = "Read";
                    Session["flag"] = "0";
                    //  window.open('InterviewFeedback.aspx?CandidateID=' + CandidateID + '&RRFID=' + RRFID + '&ScheduleID=' + ScheduleID + '&StageID=' + StageID + '', null, 'height=700, width=700,status= no, resizable= no, scrollbars=no, toolbar=no,location=no,menubar=no ')
                    //ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", "HRInterviewAssessment.aspx?CandidateID=" + CandidateID.Text + "&RRFID=" + RRFID.Text + "&StageID=" + StageID.Text + ",height=700, width=700,status= no, resizable= no, scrollbars=no, toolbar=no,location=no,menubar=no "));
                    Session["ScheduleID"] = InterViewerScheduleID.Text;

                    //ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", "'HRInterviewAssessment.aspx',null,'height=700, width=700,status= no, resizable= no, scrollbars=no, toolbar=no,location=no,menubar=no'"));
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'HRInterviewAssessment.aspx', null, 'height=900, width=1200,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no' );", true);
                    return;
                }
                else
                {
                    Session["Mode"] = "Read";
                    Session["flag"] = "0";
                    Session["ScheduleID"] = InterViewerScheduleID.Text;
                    //ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", "InterviewFeedback.aspx?CandidateID=" + CandidateID + "&RRFID=" + RRFID.Text + "&StageID=" + StageID.Text + ",height=700, width=700,status= no, resizable= no, scrollbars=no, toolbar=no,location=no,menubar=no "));
                    //-----------------------------------------------------------------//
                    //ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", "InterviewFeedback.aspx,null,height=700, width=700,status= no, resizable= no, scrollbars=no, toolbar=no,location=no,menubar=no"));
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('InterviewFeedback.aspx?CandidateID=" + CandidateID + "&RRFID=" + RRFID + "&ScheduleID=" + ScheduleID + "&StageID=" + StageID + "&Mode=Read&height=700, width=1000,status= no, resizable= no, scrollbars=yes, toolbar=no,location=no,menubar=no', null, 'status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no' );", true);
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'InterviewFeedback.aspx', null, 'height=900, width=1200,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no' );", true);
                    return;
                }
            }
        }
    }

    protected void grdSelectedcandidate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        CheckBox Feedbacklink = (CheckBox)e.Row.FindControl("chkSelect");

        if (DsSelectedCandidate != null)
        {
            if (DsSelectedCandidate.Tables[0].Rows.Count >= 1 && DsSelectedCandidate.Tables.Count > 0)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string stage = DsSelectedCandidate.Tables[0].Rows[i]["stageID"].ToString();
                    if (!string.IsNullOrEmpty(stage))
                    {
                        if ((Convert.ToInt32(stage) == 17))
                        {
                            Feedbacklink.Enabled = false;
                            i++;
                        }
                        else
                        {
                            Feedbacklink.Visible = true;

                            i++;
                        }
                    }
                }
            }
        }
    }

    protected void grdSelectedcandidate_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdSelectedcandidate.PageIndex = e.NewPageIndex;
        ////BindInterviewFeedBack();
        //grdSelectedcandidate.DataSource = DsSelectedCandidate;

        //grdSelectedcandidate.DataBind();
    }

    protected void confirmPrint(object sender, EventArgs e)
    {
        // ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>var mywindow = window.open();mywindow.document.write('<html><head><title>my div</title>');mywindow.document.write('</head><body >');mywindow.document.write($('#MainContent_tblSelectedCandidate').html());mywindow.document.write('</body></html>');mywindow.print();mywindow.close();</script>");
    }
}