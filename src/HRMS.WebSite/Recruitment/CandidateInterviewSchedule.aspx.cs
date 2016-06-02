using BLL;
using BOL;
using MailActivity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CandidateInterviewSchedule : System.Web.UI.Page
{
    private int candidateId, RRFID;
    private static string recruiterid = string.Empty;
    private static string interviewerid = string.Empty;
    private DataSet dsInterviewScheduleDetails = new DataSet();
    private int i = 0;
    private static string stageName;
    private CandidateInterviewScheduleBLL objCandidateInterviewScheduleBLL = new CandidateInterviewScheduleBLL();
    private CandidateInterviewScheduleBOL objCandidateInterviewScheduleBOL = new CandidateInterviewScheduleBOL();
    private static DataSet dsCandidate = new DataSet();
    private int rrfIdValue;
    private DataSet dsGetMailInfo = new DataSet();
    private DataSet dsGetEmployeeFromRole = new DataSet();
    private static int employeeId = 0;
    private string Message = string.Empty;
    private int mailFlag;
    private RRFApproverBLL objRRFApproverBLL = new RRFApproverBLL();
    private RRFApproverBOL objRRFApproverBOL = new RRFApproverBOL();
    private EmailActivityBLL objEmailActivityBLL = new EmailActivityBLL();
    private EmailActivityBOL objEmailActivityBOL = new EmailActivityBOL();
    private EmailActivity objEmailActivity = new EmailActivity();

    protected void Page_Load(object sender, EventArgs e)
    {
        // RRFID = Convert.ToInt32(Request.QueryString["RRFID"]);
        // candidateId = Convert.ToInt32(Request.QueryString["CandidateID"]);
        // Message = (Request.QueryString["Message"] ?? "").Trim();

        try
        {
            candidateId = Convert.ToInt32(Convert.ToString(Session["CandidateID"]));
            RRFID = Convert.ToInt32(Convert.ToString(Session["RRFID"]));
            Message = Convert.ToString(Session["Message"]).Trim();

            if (!Page.IsPostBack)
            {
                if (candidateId != 0)
                {
                    Session["CandidateID"] = candidateId;
                    Session["RRFID"] = RRFID;
                }
                else
                {
                    candidateId = Convert.ToInt32(Session["CandidateID"]);
                    RRFID = Convert.ToInt32(Session["RRFID"]);
                }
                BindData(candidateId, RRFID);
                //GetStageDetails();
                BindStageDetails();

                //if (string.IsNullOrEmpty(Message))
                //{
                //    lblErrorMsg.Visible = false;
                //}
                //else
                //{
                //    lblErrorMsg.Text = "Email could not be send to the candidate.";

                //    lblErrorMsg.Visible = true;
                //}
            }
        }
        catch (Exception errObj)
        {
            if (errObj.Message.Contains("Input string was not in a correct format"))
                Response.Redirect("http://" + Request.Url.Authority.ToLower().ToString());
        }
    }

    public void BindData(int CandidateID, int RRFID)
    {
        DataSet Ds = new DataSet();
        dsInterviewScheduleDetails = objCandidateInterviewScheduleBLL.GetCandidateSchedule(CandidateID, RRFID);

        if (dsInterviewScheduleDetails != null)
        {
            if (dsInterviewScheduleDetails.Tables[0].Rows.Count == 0)
            {
                Session["StageStatus"] = "";
                dsInterviewScheduleDetails.Tables[0].Rows.Add(dsInterviewScheduleDetails.Tables[0].NewRow());
                grdCandidateSchedule.DataSource = dsInterviewScheduleDetails.Tables[0];
                grdCandidateSchedule.DataBind();
                grdCandidateSchedule.Rows[0].Visible = false;
                objCandidateInterviewScheduleBOL.RRFNo = RRFID;
                dsInterviewScheduleDetails = objCandidateInterviewScheduleBLL.GetRRFValues(objCandidateInterviewScheduleBOL, CandidateID);
                recruiterid = dsInterviewScheduleDetails.Tables[0].Rows[0]["Recruiter"].ToString();

                if (dsInterviewScheduleDetails.Tables[0].Rows[0]["RequestDate"] != null)
                {
                    DateTime requestdate = Convert.ToDateTime(dsInterviewScheduleDetails.Tables[0].Rows[0]["RequestDate"]);
                    lblPostedDate.Text = requestdate.Date.ToString("MM/dd/yyyy");
                }
                lblRRFNo.Text = Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["RRFNo"]);
                Session["RRFNo"] = lblRRFNo.Text;
                Session["RRFID"] = Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["RRFID"]);
                // rrfIdValue = Convert.ToInt32(dsInterviewScheduleDetails.Tables[0].Rows[0]["RRFID"]);

                lblRequestor.Text = Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["EmployeeName"]);

                lblPosition.Text = Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["Designationname"]);
                lblCandidateName.Text = Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["FirstName"]) + " " + Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["lastName"]);

                Session["Position"] = lblPosition.Text;
                Session["CandidateName"] = lblCandidateName.Text;
            }
            else
            {
                int Count = dsInterviewScheduleDetails.Tables[0].Rows.Count;
                string StageStatus = Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[Count - 1]["Status"]);
                if (!string.IsNullOrEmpty(StageStatus))
                {
                    Session["StageStatus"] = Convert.ToString(StageStatus);
                }
                recruiterid = dsInterviewScheduleDetails.Tables[0].Rows[(dsInterviewScheduleDetails.Tables[0].Rows.Count) - 1]["Recruiter"].ToString();
                employeeId = Convert.ToInt32(dsInterviewScheduleDetails.Tables[0].Rows[(dsInterviewScheduleDetails.Tables[0].Rows.Count) - 1]["InterviewerId"].ToString());

                Session["EmailId"] = Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["Email"]);
                Session["CandidateName"] = Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["FirstName"]);
                DateTime posteddate = Convert.ToDateTime(dsInterviewScheduleDetails.Tables[0].Rows[0]["PostedDate"]);
                lblRRFNo.Text = Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["RRFNo"]);
                Session["RRFNo"] = lblRRFNo.Text;
                Session["RRFID"] = Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["RRFID"]);
                //txtRRFNo.Text = Convert.ToString(RRFID);
                lblRequestor.Text = Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["Requestor"]);
                lblPostedDate.Text = posteddate.Date.ToString("MM/dd/yyyy");
                lblPosition.Text = Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["DesignationName"]);
                lblCandidateName.Text = Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["FirstName"]) + " " + Convert.ToString(dsInterviewScheduleDetails.Tables[0].Rows[0]["lastName"]);
                //   rrfIdValue = Convert.ToInt32(dsInterviewScheduleDetails.Tables[0].Rows[0]["RRFID"]);

                Session["Position"] = lblPosition.Text;
                Session["CandidateName"] = lblCandidateName.Text;

                if (Session["StageStatus"].ToString() == "16" || Session["StageStatus"].ToString() == "15")
                {
                    grdCandidateSchedule.ShowFooter = false;
                    grdCandidateSchedule.DataSource = dsInterviewScheduleDetails.Tables[0];
                    grdCandidateSchedule.DataBind();
                }
                else
                {
                    grdCandidateSchedule.DataSource = dsInterviewScheduleDetails.Tables[0];
                    grdCandidateSchedule.DataBind();
                }
            }
        }
    }

    public void BindStageDetails()
    {
        DropDownList ddlStage = (DropDownList)grdCandidateSchedule.FooterRow.Cells[0].Controls[1];
        ddlStage.Items.Clear();
        DataSet ds = new DataSet();
        string stageName = string.Empty;
        int StageId;
        ds = objCandidateInterviewScheduleBLL.GetStage(objCandidateInterviewScheduleBOL);
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlStage.DataSource = ds.Tables[0];
                ddlStage.DataTextField = "StageName";
                ddlStage.DataValueField = "ID";
                ddlStage.DataBind();
                ddlStage.Items.Insert(0, "Select");
            }
        }
    }

    public DataSet GetStageDetails()
    {
        DropDownList ddlStage = (DropDownList)grdCandidateSchedule.FooterRow.Cells[0].Controls[1];
        ddlStage.Items.Clear();
        DataSet ds = new DataSet();
        string stageName = string.Empty;
        int StageId;
        ds = objCandidateInterviewScheduleBLL.GetStage(objCandidateInterviewScheduleBOL);

        if (ds != null)
        {
            ddlStage.DataSource = ds.Tables[0];
            ddlStage.DataTextField = "StageName";
            ddlStage.DataValueField = "ID";
            ddlStage.DataBind();
        }
        return ds;
    }

    protected void ddlTimeHours_Load(object sender, EventArgs e)
    {
        DropDownList ddlHours = (DropDownList)sender;

        if (ddlHours.Items.Count > 0)
        {
            int s = ddlHours.SelectedIndex;
            ddlHours.Items.Clear();
            for (int i = 1; i <= 24; i++)
            {
                string hrs = string.Empty;
                if (i < 10)
                    hrs = "0" + Convert.ToString(i);
                else
                    hrs = Convert.ToString(i);

                ddlHours.Items.Add(hrs);
            }
            ddlHours.SelectedIndex = s;
        }
        else
        {
            for (int i = 1; i <= 24; i++)
            {
                //string l = Convert.ToString(i);
                string l = string.Empty;
                if (i < 10)
                    l = "0" + Convert.ToString(i);
                else
                    l = Convert.ToString(i);
                ddlHours.Items.Add(l);
            }
        }
    }

    protected void ddlTimeHours_SelectedIndexChanged(object sender, EventArgs e)
    {
        string s = " ";
        int i = 0;
        DropDownList ddlHours = (DropDownList)sender;
        Label lbl = (Label)((GridViewRow)ddlHours.Parent.Parent).FindControl("lblTime");
        if (lbl.Text == "")
            lbl.Text = ddlHours.SelectedItem.Value.ToString();
        else if (lbl.Text.Contains(":"))
        {
            i = lbl.Text.IndexOf(":");
            s = lbl.Text.Substring(i, 3);
            lbl.Text = ddlHours.SelectedItem.Value.ToString() + s;
        }
    }

    protected void ddlTimeMinutes_Load(object sender, EventArgs e)
    {
        DropDownList ddlMin = (DropDownList)sender;
        if (ddlMin.Items.Count > 0)
        {
            int s = ddlMin.SelectedIndex;
            ddlMin.Items.Clear();
            for (int i = 0; i <= 59; i++)
            {
                string min = string.Empty;

                if (i < 10)
                    min = "0" + Convert.ToString(i);
                else
                    min = Convert.ToString(i);

                ddlMin.Items.Add(min);
            }
            ddlMin.SelectedIndex = s;
        }
        else
        {
            for (int i = 0; i <= 59; i++)
            {
                // string min = Convert.ToString(i);
                string min = string.Empty;

                if (i < 10)
                    min = "0" + Convert.ToString(i);
                else
                    min = Convert.ToString(i);

                ddlMin.Items.Add(min);
            }
        }
    }

    protected void ddlTimeMinutes_SelectedIndexChanged(object sender, EventArgs e)
    {
        String s = " ";
        DropDownList ddl = (DropDownList)sender;
        Label lbl = (Label)((GridViewRow)ddl.Parent.Parent).FindControl("lblTime");
        if (lbl.Text == "")
            lbl.Text = ":" + ddl.SelectedItem.Value.ToString();
        else if (lbl.Text.Contains(":"))
        {
            s = lbl.Text.Substring(0, 2);
            lbl.Text = s + ":" + ddl.SelectedItem.Value.ToString();
        }
        else
        {
            if (lbl.Text.Length > 1)
            {
                s = lbl.Text.Substring(0, 2);
                lbl.Text = s + ":" + ddl.SelectedItem.Value.ToString();
            }
            else
            {
                lbl.Text = s + ":" + ddl.SelectedItem.Value.ToString();
            }
        }
    }

    private static DataSet ds = new DataSet();
    private static CandidateInterviewScheduleBLL objCandidateInterviewSchedulebol = new CandidateInterviewScheduleBLL();

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetEmployeeName(string prefixText)
    {
        //DropDownList ddlStage = grdCandidateSchedule.FooterRow.FindControl("ddlStage") as DropDownList;
        string ddlStageName = stageName;
        List<string> IndPanNames = new List<string>();
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
            else if (stageName.Trim() == "Group Head")
            {
                RoleName = "Group Head";
            }
            else
            {
                RoleName = "Interviewer";
            }

            ds = objCandidateInterviewSchedulebol.GetEmployeeName(prefixText, RoleName);

            dsCandidate = ds;

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

    protected void grdCandidateSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, typeof(string), "CandidateReSchedule", "javascript:CandidatescheduleLoader();", true);
        // ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOWNew", "javascript:CandidatescheduleLoader();", true);

        candidateId = Convert.ToInt32(Session["CandidateID"].ToString());
        RRFID = Convert.ToInt32(Session["RRFID"].ToString());

        DropDownList ddlStage = grdCandidateSchedule.FooterRow.FindControl("ddlStage") as DropDownList;
        LinkButton lnkbtnInsert = grdCandidateSchedule.FooterRow.FindControl("lnkbtnInsert") as LinkButton;

        string stageName = ddlStage.SelectedItem.Value;
        Session["stageName"] = stageName;

        TextBox txtInterviewerName = grdCandidateSchedule.FooterRow.FindControl("txtInterviewerName") as TextBox;
        Label lblInterviewerName = grdCandidateSchedule.FooterRow.FindControl("lblInterviewerName") as Label;
        if (txtInterviewerName.Text == "")
            objCandidateInterviewScheduleBOL.InterviewerName = 0;
        else
        {
            int isInterviewerNameEmpty = 1;

            if (dsCandidate != null)
            {
                if (dsCandidate.Tables.Count > 0 && dsCandidate.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsCandidate.Tables[0].Rows.Count; i++)
                    {
                        if (txtInterviewerName.Text == dsCandidate.Tables[0].Rows[i]["EmployeeName"].ToString())
                        {
                            employeeId = Convert.ToInt32(dsCandidate.Tables[0].Rows[i]["UserID"]);
                            Session["InterviewerName"] = employeeId;
                            Session["InterviewerID"] = employeeId;
                            isInterviewerNameEmpty = 1;

                            break;
                        }
                        else
                        {
                            isInterviewerNameEmpty = 0;
                        }
                    }
                }
                else
                {
                    lblInterviewerName.Visible = true;
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOWNew", "javascript:HideImageloader();", true);
                    return;
                }
            }

            if (isInterviewerNameEmpty == 1)
                objCandidateInterviewScheduleBOL.InterviewerName = employeeId;
            else
            {
                lblInterviewerName.Visible = true;
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOWNew", "javascript:HideImageloader();", true);
                lblInterviewerName.Visible = true;
                loadImage.Visible = false;

                return;
            }
        }

        if (ddlStage.SelectedItem.Text != "Final Stage")
        {
            if (e.CommandName == "Insert")
            {
                int RowNumberForReschedule = 0;
                objCandidateInterviewScheduleBOL.CandidateID = Convert.ToInt32(Session["CandidateID"]);
                objCandidateInterviewScheduleBOL.RRFNo = Convert.ToInt32(Session["RRFID"]);
                objCandidateInterviewScheduleBOL.Stage = Convert.ToString(ddlStage.SelectedItem.Text);
                Session["StageName"] = Convert.ToString(ddlStage.SelectedItem.Value);
                DropDownList ddlTimeHours = grdCandidateSchedule.FooterRow.FindControl("ddlTimeHours") as DropDownList;
                DropDownList ddlTimeMinutes = grdCandidateSchedule.FooterRow.FindControl("ddlTimeMinutes") as DropDownList;
                TextBox txtDate = grdCandidateSchedule.FooterRow.FindControl("txtDate") as TextBox;
                string expireOnDate = txtDate.Text;
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
                objCandidateInterviewScheduleBOL.ScheduledDateTime = expectedClosureDate.AddHours(Convert.ToDouble(ddlTimeHours.SelectedValue.ToString())).AddMinutes(Convert.ToDouble(ddlTimeMinutes.SelectedItem.Value.ToString()));
                objCandidateInterviewScheduleBOL.ScheduledBy = Convert.ToInt32(recruiterid);
                interviewerid = employeeId.ToString();
                objCandidateInterviewScheduleBOL.RescheduleReason = "";
                objCandidateInterviewScheduleBLL.SetCandidateSchecule(objCandidateInterviewScheduleBOL, RowNumberForReschedule);
                grdCandidateSchedule.EditIndex = -1;
                BindData(candidateId, RRFID);
                GetStageDetails();
                lnkbtnInsert.Enabled = false;
                ////Mailing Activity
                objRRFApproverBOL.Role = "HRM";
                dsGetEmployeeFromRole = objRRFApproverBLL.GetEmployeeFromRole(objRRFApproverBOL);
                string toID = dsGetEmployeeFromRole.Tables[0].Rows[0]["UserId"].ToString();
                for (int i = 1; i < dsGetEmployeeFromRole.Tables[0].Rows.Count; i++)
                    toID = toID + ';' + dsGetEmployeeFromRole.Tables[0].Rows[i]["UserId"].ToString();
                objEmailActivityBOL.ToID = Convert.ToString(interviewerid) + ";";//interviewer

                objEmailActivityBOL.CCID = Convert.ToString(recruiterid) + ";" + Convert.ToString(toID) + ";";//recruiter,HRM

                objEmailActivityBOL.FromID = Convert.ToInt32(recruiterid);//recruiter
                objEmailActivityBOL.EmailTemplateName = "Schedule Interview";
                dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);

                string interviewTime, interviewDate, body, bodyForCandidate;
                interviewDate = String.Format("{0:dddd, MMMM d, yyyy}", expectedClosureDate);
                interviewTime = String.Format("{0:t}", objCandidateInterviewScheduleBOL.ScheduledDateTime);
                DataSet dsmaildetails = new DataSet();
                dsmaildetails = objCandidateInterviewScheduleBLL.GetDetailsformail(objCandidateInterviewScheduleBOL);

                body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
                //body = body.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
                //body = body.Replace("##skills##", (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString()));
                body = body.Replace("##InterviewTime##", interviewTime);
                body = body.Replace("##InterviewDate##", interviewDate);
                // body = body.Replace("##comment##", txtReason.Text);
                bodyForCandidate = body;
                //String.Format("{0:dddd, MMMM d, yyyy}", dt);

                char[] separator = new char[] { ';' };
                objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
                objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
                objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
                objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##skills##", (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString()));
                objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##CandidateName##", lblCandidateName.Text);
                objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("\r\n", " ");
                objEmailActivityBOL.Body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
                objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
                objEmailActivityBOL.RRFNo = (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString());
                objEmailActivityBOL.skills = (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString());
                objEmailActivityBOL.Position = (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString());
                objEmailActivityBOL.CandidateName = Convert.ToString(Session["CandidateName"].ToString());
                try
                {
                    body = body.Replace("##Candidate Name##", lblCandidateName.Text);
                    objEmailActivityBOL.Body = body;
                    objEmailActivity.SendMail(objEmailActivityBOL);
                    //for (int i = 0; i < objEmailActivityBOL.ToAddress.Length; i++)
                    //    objEmailActivityBOL.ToAddress[i] = string.Empty;
                    //objEmailActivityBOL.ToAddress[0] = Convert.ToString(Session["EmailId"].ToString());
                    //objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
                    ////objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
                    //bodyForCandidate = bodyForCandidate.Replace(" for <b>##CandidateName##</b>", string.Empty);
                    //objEmailActivityBOL.Body = bodyForCandidate;
                    //objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
                    //objEmailActivity.SendMail(objEmailActivityBOL);
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOWNew", "javascript:CandidatescheduleLoader();", true);
                }
                catch (System.Exception ex)
                {
                    lblMessage.Text = "Interview Scheduled,but e-mails could not be sent.";
                    lblMessage.Visible = true;
                    mailFlag = 1;
                }
                finally
                {
                    if (mailFlag == 1)
                    {
                        lblErrorMsg.Text = "Interview Scheduled,but e-mails could not be sent.";
                        lblErrorMsg.Visible = true;
                        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOWNew", "javascript:CandidatescheduleLoader();", true);
                        Session["CandidateID"] = Convert.ToString(candidateId);
                        Session["RRFID"] = Convert.ToString(RRFID);
                        Session["Message"] = "Email could not be send to the candidate";
                        //Response.Redirect("~/CandidateInterviewSchedule.aspx?CandidateID=" + candidateId + "&RRFID=" + RRFID + "&Message=Email could not be send to the candidate");
                        Response.Redirect("~/Recruitment/CandidateInterviewSchedule.aspx");
                    }
                }
            }
            else if (e.CommandName == "Reschedule")
            {
                DataSet DS = new DataSet();
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdrow = grdCandidateSchedule.Rows[index];
                GridViewRow grdView = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                TextBox txtStage = (TextBox)grdrow.FindControl("txtEditStageName");
                //TextBox txtInterviewerName = (TextBox)grdrow.FindControl("txtEditInterviewer");
                TextBox txtDate = (TextBox)grdrow.FindControl("txtEditScheduledDate");
                TextBox txtTime = (TextBox)grdrow.FindControl("txtEditddlTimeHours");

                Label lblStageName = (Label)grdrow.FindControl("lblStageName");
                Label lblScheduleDate = (Label)grdrow.FindControl("lblScheduleDate");
                Label lblTime = (Label)grdrow.FindControl("lblTime");
                Label lblInterviewer = (Label)grdrow.FindControl("lblInterviewer");
                Label lblCandidateID = (Label)grdrow.FindControl("lblCandidateID");

                Label lblroundNumber = (Label)grdrow.FindControl("lblroundNumber");
                DS = objCandidateInterviewScheduleBLL.GetCandidateReScheduleRoundNumber(Convert.ToInt32(Session["CandidateID"]), Convert.ToInt32(Session["RRFID"]), Convert.ToInt32(lblroundNumber.Text));
                if (DS != null)
                {
                    if (DS.Tables.Count > 0)
                    {
                        int MaxRescheduleCount = Convert.ToInt32(DS.Tables[0].Rows[0]["MaxRescheduleCount"]);
                        if (MaxRescheduleCount > 5)
                        {
                            // this.ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript: show_confirm(); ", true);
                            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "javascript:show_confirm();", true);
                        }
                        else
                        {
                            Session["RRFNo"] = Convert.ToString(lblRRFNo.Text);
                            Session["Position"] = lblPosition.Text;
                            Session["CandidateName"] = lblCandidateName.Text;
                            Session["PostedDate"] = lblPostedDate.Text;
                            Session["Requestor"] = lblRequestor.Text;
                            Session["StageName"] = lblStageName.Text;
                            Session["ScheduledDate"] = lblScheduleDate.Text;
                            Session["ScheduledTime"] = lblTime.Text;
                            Session["InterviewerName"] = lblInterviewer.Text;
                            Session["RoundNumber"] = grdrow.RowIndex + 1;
                            Session["RecruiterId"] = recruiterid;
                            Session["InterviewerID"] = interviewerid;
                            Session["InterviewerEmployeeId"] = employeeId;

                            Response.Redirect("~/Recruitment/RescheduleForm.aspx");
                        }
                    }
                }
            }
            else if (e.CommandName == "ViewFeedBack")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdrow = grdCandidateSchedule.Rows[index];
                GridViewRow grdView = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                Label lblCandidateID = (Label)grdrow.FindControl("lblCandidateID");
                Label lblDesignationName = (Label)grdrow.FindControl("lblDesignationName");
                Label lblScheduleID = (Label)grdrow.FindControl("lblScheduleID");
                Label lblStageID = (Label)grdrow.FindControl("lblStageID");
                Label lblStageNameGrid = (Label)grdrow.FindControl("lblStageName");
                Label lblRRFID = (Label)grdrow.FindControl("lblRRFID");
                Label RRFNumber = (Label)grdrow.FindControl("lblRRFNumber");
                Session["ScheduleID"] = lblScheduleID.Text;
                Session["StageName"] = lblStageNameGrid.Text;
                Session["DesignationName"] = lblDesignationName.Text;
                Session["CandidateID"] = lblCandidateID.Text;
                Session["RRFNumber"] = RRFNumber.Text;
                Session["RRFID"] = lblRRFID.Text;
                Session["StageID"] = lblStageID.Text;

                if (lblStageID.Text == "17")
                {
                    // string flag = "1";
                    Session["flag"] = "1";
                    Session["Mode"] = "";
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'SelectedCandidate.aspx?ScheduleID=" + lblScheduleID.Text + "&StageName=" + lblStageNameGrid.Text + "&DesignationName=" + lblDesignationName.Text + "&CandidateID=" + lblCandidateID.Text + "&RRFNumber=" + RRFNumber.Text + "&RRFID=" + lblRRFID.Text + "&StageID=" + lblStageID.Text + "&Mode=&Flag=1&height=700, width=700,status= no, resizable= no, scrollbars=no, toolbar=no,location=no,menubar=no ' );", true);
                    //  ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('JoinEmployeePopup.aspx',null,'height=250, width=600,status= no, resizable= no, scrollbars=yes, toolbar=no,location=no,menubar=no');", true);

                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'SelectedCandidate.aspx',null,'height=900, width=1800,status= no, resizable= no, scrollbars=no, toolbar=no,location=no,menubar=no ' );", true);
                }
                // HR stage or Group head stage
                else if (lblStageID.Text == "6" || lblStageID.Text == "18")
                {
                    Session["flag"] = "0";
                    Session["Mode"] = "Read";
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'HRInterviewAssessment.aspx?ScheduleID=" + lblScheduleID.Text + "&StageName=" + lblStageNameGrid.Text + "&DesignationName=" + lblDesignationName.Text + "&CandidateID=" + lblCandidateID.Text + "&RRFNumber=" + RRFNumber.Text + "&RRFID=" + lblRRFID.Text + "&StageID=" + lblStageID.Text + "&flag=0&Mode=Read&height=550, width=1200,status= no, resizable= no, scrollbars=yes, toolbar=no,location=no,menubar=no', null, 'status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no' );", true);
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'HRInterviewAssessment.aspx', null, 'height=1000, width=1200,status=yes,toolbar=no,titlebar=yes,scrollbars=yes,menubar=no,location=no' );", true);
                }
                //else if(lblStageID.Text=="6" && lblStageID.Text=="17" && lblStageID.Text=="18")
                else
                {
                    Session["flag"] = "0";
                    Session["Mode"] = "Read";
                    //  ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'InterviewFeedback.aspx?ScheduleID=" + lblScheduleID.Text + "&StageName=" + lblStageNameGrid.Text + "&DesignationName=" + lblDesignationName.Text + "&CandidateID=" + lblCandidateID.Text + "&RRFNumber=" + RRFNumber.Text + "&RRFID=" + lblRRFID.Text + "&StageID=" + lblStageID.Text + "&flag=0&Mode=Read&height=700, width=1000,status= no, resizable= no, scrollbars=yes, toolbar=no,location=no,menubar=no', null, 'status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no' );", true);
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( 'InterviewFeedback.aspx', null, 'height=1000, width=1200,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no' );", true);
                }
            }
        }
        else
        {
            //Response.Redirect("SelectedCandidate.aspx?StageName=" + stageName+"&Mode=Read");
            int RowNumberForReschedule = 0;
            //objCandidateInterviewScheduleBOL.CandidateName = lblCandidateName.Text;
            objCandidateInterviewScheduleBOL.CandidateID = Convert.ToInt32(Session["CandidateID"]);
            objCandidateInterviewScheduleBOL.RRFNo = Convert.ToInt32(Session["RRFID"]);
            objCandidateInterviewScheduleBOL.Stage = Convert.ToString(ddlStage.SelectedItem.Text);
            Session["StageName"] = Convert.ToString(ddlStage.SelectedItem.Value);
            DropDownList ddlTimeHours = grdCandidateSchedule.FooterRow.FindControl("ddlTimeHours") as DropDownList;
            DropDownList ddlTimeMinutes = grdCandidateSchedule.FooterRow.FindControl("ddlTimeMinutes") as DropDownList;
            objCandidateInterviewScheduleBOL.ScheduledDateTime = DateTime.Now;
            //objCandidateInterviewScheduleBOL.ScheduledBy = Convert.ToInt32(Session["userID"]);
            interviewerid = employeeId.ToString();
            objCandidateInterviewScheduleBOL.ScheduledBy = Convert.ToInt32(interviewerid);
            objCandidateInterviewScheduleBOL.RescheduleReason = "";
            objCandidateInterviewScheduleBLL.SetCandidateSchecule(objCandidateInterviewScheduleBOL, RowNumberForReschedule);
            grdCandidateSchedule.EditIndex = -1;
            BindData(candidateId, RRFID);
            GetStageDetails();

            //Mailing Activity

            objRRFApproverBOL.Role = "HRM";
            dsGetEmployeeFromRole = objRRFApproverBLL.GetEmployeeFromRole(objRRFApproverBOL);
            string toID = dsGetEmployeeFromRole.Tables[0].Rows[0]["UserId"].ToString();
            for (int i = 1; i < dsGetEmployeeFromRole.Tables[0].Rows.Count; i++)
                toID = toID + ';' + dsGetEmployeeFromRole.Tables[0].Rows[i]["UserId"].ToString();
            objEmailActivityBOL.ToID = Convert.ToString(interviewerid) + ";";//interviewer

            objEmailActivityBOL.CCID = Convert.ToString(recruiterid) + ";" + Convert.ToString(toID) + ";";//recruiter,HRM

            objEmailActivityBOL.FromID = Convert.ToInt32(recruiterid);//recruiter
            objEmailActivityBOL.EmailTemplateName = "Final Stage Schedule";
            dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);

            string body, bodyForCandidate;
            body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            bodyForCandidate = body;

            char[] separator = new char[] { ';' };
            objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
            objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
            objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
            objEmailActivityBOL.Body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
            objEmailActivityBOL.RRFNo = "";
            objEmailActivityBOL.skills = "";
            objEmailActivityBOL.Position = "";
            objEmailActivityBOL.CandidateName = Convert.ToString(Session["CandidateName"].ToString());
            try
            {
                body = body.Replace("##CandidateName##", lblCandidateName.Text);
                body = body.Replace("##EmployeeName##", txtInterviewerName.Text);
                objEmailActivityBOL.Body = body;
                objEmailActivity.SendMail(objEmailActivityBOL);
                //for (int i = 0; i < objEmailActivityBOL.ToAddress.Length; i++)
                //    objEmailActivityBOL.ToAddress[i] = string.Empty;
                //objEmailActivityBOL.ToAddress[0] = Convert.ToString(Session["EmailId"].ToString());
                //objEmailActivityBOL.CandidateName = Convert.ToString(Session["CandidateName"].ToString());
                //objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
                //objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
                //bodyForCandidate = bodyForCandidate.Replace(" for <b>##CandidateName##</b>", string.Empty);
                //objEmailActivityBOL.Body = bodyForCandidate;
                //objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
                //objEmailActivity.SendMail(objEmailActivityBOL);

                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOWNew", "javascript:CandidatescheduleLoader();", true);
            }
            //catch (System.Exception ex)
            //{
            //    lblMessage.Text = "Interview Scheduled,but e-mails could not be sent.";
            //    lblMessage.Visible = true;
            //}
            //finally
            //{
            //    lblMessage.Text = "Interview Scheduled,but e-mails could not be sent.";
            //    lblMessage.Visible = true;
            //    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOWNew", "javascript:CandidatescheduleLoader();", true);
            //    Response.Redirect("~/CandidateInterviewSchedule.aspx?CandidateID=" + candidateId + "&RRFID=" + RRFID + "&Message=Email could not be send to the candidate");
            //}
            catch (System.Exception ex)
            {
                lblMessage.Text = "Interview Scheduled,but e-mails could not be sent.";
                lblMessage.Visible = true;
                mailFlag = 1;
            }
            finally
            {
                if (mailFlag == 1)
                {
                    lblErrorMsg.Text = "Interview Scheduled,but e-mails could not be sent.";
                    lblErrorMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOWNew", "javascript:CandidatescheduleLoader();", true);

                    Session["CandidateID"] = Convert.ToString(candidateId);
                    Session["RRFID"] = Convert.ToString(RRFID);
                    Session["Message"] = "Email could not be send to the candidate";

                    Response.Redirect("~/Recruitment/CandidateInterviewSchedule.aspx");
                }
            }
        }
    }

    protected void grdCandidateSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCandidateSchedule.PageIndex = e.NewPageIndex;
        BindData(candidateId, RRFID);
        GetStageDetails();
    }

    protected void grdCandidateSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        LinkButton Reschedulelink = (LinkButton)e.Row.FindControl("lnkReschedule");
        LinkButton lnkViewFeedBack = (LinkButton)e.Row.FindControl("lnkViewStageDetails");

        if (dsInterviewScheduleDetails != null)
        {
            if (dsInterviewScheduleDetails.Tables.Count > 0)
            {
                if (dsInterviewScheduleDetails.Tables[0].Rows.Count >= 1 && dsInterviewScheduleDetails.Tables.Count > 0)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        string Status = dsInterviewScheduleDetails.Tables[0].Rows[i]["Status"].ToString();
                        string stage = dsInterviewScheduleDetails.Tables[0].Rows[i]["stage"].ToString();

                        string RRFStatus = dsInterviewScheduleDetails.Tables[0].Rows[i]["RRFStatus"].ToString();
                        if (!string.IsNullOrEmpty(Status))
                        {
                            if ((Convert.ToInt32(Status) == 14)) //Stage Cleared
                            {
                                Reschedulelink.Visible = false;
                                i++;
                            }
                            else
                            {
                                Reschedulelink.Visible = true;
                                i++;
                            }
                            if (Convert.ToInt32(Status) == 16)//Not Conducted
                            {
                                lnkViewFeedBack.Visible = false;
                            }
                            else
                            {
                                lnkViewFeedBack.Visible = true;
                            }

                            if ((Convert.ToInt32(Status) == 15) || (Convert.ToInt32(Status) == 9))//'On Hold && Rejected'
                            {
                                lnkViewFeedBack.Visible = true;
                                Reschedulelink.Visible = false;
                                grdCandidateSchedule.ShowFooter = false;
                            }
                            // Group head stage
                            if (Session["HRMRole"] == null)
                            {
                                if ((Convert.ToInt32(stage) == 18))
                                {
                                    lnkViewFeedBack.Visible = false;
                                }
                            }

                            if ((Convert.ToInt32(stage) == 17))
                            {
                                lnkViewFeedBack.Visible = false;
                                Reschedulelink.Visible = false;
                                grdCandidateSchedule.ShowFooter = false;
                            }
                        }
                        if (!string.IsNullOrEmpty(RRFStatus))
                        {
                            if ((Convert.ToInt32(RRFStatus) == 3) || (Convert.ToInt32(RRFStatus) == 4))
                            {
                                lnkViewFeedBack.Visible = true;
                                Reschedulelink.Visible = false;
                                grdCandidateSchedule.ShowFooter = false;
                            }
                        }
                    }
                }

                if (dsInterviewScheduleDetails.Tables[1].Rows.Count > 0)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        string stage = dsInterviewScheduleDetails.Tables[1].Rows[0]["StageID"].ToString();

                        if (Session["HRMRole"] != null)
                        {
                            if (Convert.ToInt32(stage) == 17 || Convert.ToInt32(stage) == 18)
                            {
                                lnkViewFeedBack.Visible = true;
                                Reschedulelink.Visible = false;
                                grdCandidateSchedule.ShowFooter = false;
                            }
                        }
                    }
                }
            }
        }
    }

    protected void ddlStage_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DropDownList ddlStage = grdCandidateSchedule.FooterRow.FindControl("ddlStage") as DropDownList;\
        TextBox txtInterviewerName = grdCandidateSchedule.FooterRow.FindControl("txtInterviewerName") as TextBox;

        DropDownList ddlTimeHours = grdCandidateSchedule.FooterRow.FindControl("ddlTimeHours") as DropDownList;

        DropDownList ddlTimeMinutes = grdCandidateSchedule.FooterRow.FindControl("ddlTimeMinutes") as DropDownList;

        TextBox txtDate = grdCandidateSchedule.FooterRow.FindControl("txtDate") as TextBox;
        ImageButton imgbtnDate = grdCandidateSchedule.FooterRow.FindControl("imgbtnDate") as ImageButton;

        txtDate.Text = "";
        DropDownList ddlStage = sender as DropDownList;
        string stage = ddlStage.SelectedItem.Text;
        if (!string.IsNullOrEmpty(stage))
        {
            Session["stageName"] = stage;
            stageName = Session["stageName"].ToString();
            if (stageName.Trim() == "Final Stage")
            {
                ddlTimeHours.Visible = false;
                ddlTimeMinutes.Visible = false;
                txtDate.Visible = false;
                imgbtnDate.Visible = false;
            }
            else
            {
                ddlTimeHours.Visible = true;
                ddlTimeMinutes.Visible = true;
                txtDate.Visible = true;
                imgbtnDate.Visible = true;
            }
        }

        //ClientScriptManager.RegisterClientScriptBlock(this.GetType, "myFunction", "$(document).ready(myFunction);", true);
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Recruitment/Recruiter.aspx");
    }
}