using BLL;
using BOL;
using MailActivity;
using System;
using System.Data;

//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Recruiter : System.Web.UI.Page
{
    private static DataSet dsRRFDetails = new DataSet();
    private RecruiterBLL objRecruiterBLL = new RecruiterBLL();
    private RecruiterBOL objRecruiterBOL = new RecruiterBOL();
    private int UserID;
    private int mailFlag;
    private DataSet dsGetMailInfo = new DataSet();
    private DataSet dsGetEmployeeFromRole = new DataSet();
    private EmailActivityBLL objEmailActivityBLL = new EmailActivityBLL();
    private EmailActivityBOL objEmailActivityBOL = new EmailActivityBOL();
    private EmailActivity objEmailActivity = new EmailActivity();
    private RRFApproverBLL objRRFApproverBLL = new RRFApproverBLL();
    private RRFApproverBOL objRRFApproverBOL = new RRFApproverBOL();
    private RescheduledBLL objRescheduledBLL = new RescheduledBLL();
    private RescheduledBOL objRescheduledBOL = new RescheduledBOL();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    public void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            BindData();
    }

    public void BindData()
    {
        // UserID = 3300;
        lblMessage.Visible = false;
        if (Session["HRMRole"] != null)
        {
            UserID = Convert.ToInt32(User.Identity.Name);
        }
        else
        {
            UserID = 0;
        }

        dsRRFDetails = objRecruiterBLL.GetRRFDetails(UserID);
        if (dsRRFDetails.Tables[0].Rows.Count != 0)
        {
            grdRecruiter.DataSource = dsRRFDetails;
            grdRecruiter.DataBind();
        }
        else
        {
            btnAcceptRRF.Visible = false;
            btnSearchCandidate.Visible = false;
            btnScheduleInterview.Visible = false;
            btnViewRRF.Visible = false;
            btnViewProgress.Visible = false;
            lblMessage.Text = "No Records Found";
            lblMessage.Visible = true;
        }
    }

    protected void grdRecruiter_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName == "SelectCandidate")
        //{
        //    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //    Label RRFID = (Label)grdRecruiter.Rows[gvr.RowIndex].FindControl("lblRRFID");

        //    Response.Redirect("~/CandidateSearch.aspx?RRFID=" + RRFID.Text);

        //}
    }

    protected void grdRecruiter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        // {
        //     CheckBox chk = (CheckBox)e.Row.FindControl("chkSelect");
        //     string CountryId = (chk.ClientID).ToString();
        //     Label lbl = (Label)e.Row.FindControl("lblSelect");
        //    lbl.for
        // }
    }

    protected void grdRecruiter_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdRecruiter.PageIndex = e.NewPageIndex;
        grdRecruiter.DataSource = dsRRFDetails;
        grdRecruiter.DataBind();
    }

    protected void btnSearchCandidate_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdRecruiter.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                string RRFID = Convert.ToString(lblRRFID.Text).Trim();
                //string RRFID = "1000";

                Label lblRRFcode = (Label)gvRow.FindControl("lblRRFcode");
                string RRFNo = Convert.ToString(lblRRFcode.Text).Trim();
                //string RRFNo = "Di20131000.1";

                Session["RRFNo"] = RRFNo;
                Session["RRFID"] = RRFID;
                // Response.Redirect("CandidateSearch.aspx?RRFID=" + RRFID);
                Response.Redirect("CandidateSearch.aspx");
            }
        }
    }

    protected void btnScheduleInterview_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdRecruiter.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                string RRFID = Convert.ToString(lblRRFID.Text).Trim();
                //string RRFID = "999";
                Session["RRFID"] = RRFID.ToString();
                Session["ShowButtons"] = "true";
                // Response.Redirect("RRFStatus.aspx?RRFID=" + RRFID + "&ShowButtons=true");
                Response.Redirect("RRFStatus.aspx");
            }
        }
    }

    protected void btnViewProgress_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdRecruiter.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                string RRFID = Convert.ToString(lblRRFID.Text).Trim();
                Session["RRFID"] = RRFID;
                //Response.Redirect("RRFStatus.aspx?RRFID=" + RRFID);
                Response.Redirect("RRFStatus.aspx");
            }
        }
    }

    protected void btnAcceptRRF_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdRecruiter.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                string RRFID = Convert.ToString(lblRRFID.Text).Trim();
                Session["RRFID"] = RRFID;
                HiddenField hiddenStatus = (HiddenField)gvRow.FindControl("hdnRRFStatus");
                string Status = Convert.ToString(hiddenStatus.Value);

                objRecruiterBOL.RRFID = RRFID;
                objRecruiterBOL.ModifiedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name.ToString());
                objRecruiterBOL.ModifiedDate = DateTime.Now;
                objRecruiterBOL.RRFAcceptedDate = DateTime.Now;

                if (Status == "Yet To Begin")
                {
                    objRecruiterBLL.ChangeStatus(objRecruiterBOL);
                    BindData();

                    objRRFApproverBOL.Role = "HRM";
                    dsGetEmployeeFromRole = objRRFApproverBLL.GetEmployeeFromRole(objRRFApproverBOL);
                    string toID = dsGetEmployeeFromRole.Tables[0].Rows[0]["UserId"].ToString();
                    for (int i = 1; i < dsGetEmployeeFromRole.Tables[0].Rows.Count; i++)
                        toID = toID + ';' + dsGetEmployeeFromRole.Tables[0].Rows[i]["UserId"].ToString();
                    objEmailActivityBOL.ToID = Convert.ToString(toID);
                    objEmailActivityBOL.FromID = objRecruiterBOL.ModifiedBy;//recruiter
                    objEmailActivityBOL.CCID = objRecruiterBOL.ModifiedBy.ToString();//recruiter
                    objEmailActivityBOL.EmailTemplateName = "Accept RRF";
                    dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);
                    string body, bodyForCandidate;
                    objRescheduledBOL.RRFNo = Convert.ToInt32(RRFID);
                    DataSet dsmaildetails = new DataSet();
                    dsmaildetails = objRescheduledBLL.GetDetailsformail(objRescheduledBOL);

                    body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
                    //body = body.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
                    //body = body.Replace("##skills##", (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString()));

                    //body = body.Replace("##comment##", txtReason.Text);
                    bodyForCandidate = body;
                    //String.Format("{0:dddd, MMMM d, yyyy}", dt);

                    char[] separator = new char[] { ';' };
                    objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
                    objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
                    //body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
                    //body = body.Replace("##date##", String.Format("{0:f}", objRescheduledBOL.ScheduledDateTime));

                    objEmailActivityBOL.Body = body;
                    try
                    {
                        objEmailActivityBOL.RRFNo = (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString());
                        objEmailActivityBOL.skills = (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString());
                        objEmailActivityBOL.Position = (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString());
                        objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
                        objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
                        objEmailActivityBOL.Body = bodyForCandidate;
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
                            Response.Redirect("SLAForRRF.aspx");
                            //Response.Redirect("~/CandidateInterviewSchedule.aspx?Message=Email could not be send to the candidate");
                        }
                        else
                        {
                            Response.Redirect("SLAForRRF.aspx");
                        }
                    }
                }
            }
        }
    }

    protected void grdRecruiter_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    private void SortGridView(string sortExpression, string direction)
    {
        //  You can cache the DataTable for improving performance

        DataTable dt = dsRRFDetails.Tables[0] as DataTable;

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        dt = dv.ToTable();
        DataSet sortedDs = new DataSet();
        sortedDs.Tables.Add(dt.Copy());
        dsRRFDetails = sortedDs;

        grdRecruiter.DataSource = dv;
        grdRecruiter.DataBind();
    }

    protected void btnViewRRF_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdRecruiter.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Session["Role"] = "Recruiter"; //Sets the role based on which the HRM View screen is opened

                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                string RRFID = Convert.ToString(lblRRFID.Text).Trim();
                //string RRFID = "1000";
                Session["RRFID"] = Convert.ToString(RRFID);
                Response.Redirect("HRM.aspx");
            }
        }
    }

    protected void btnViewSLA_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdRecruiter.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Session["Role"] = "Recruiter"; //Sets the role based on which the HRM View screen is opened

                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                string RRFID = Convert.ToString(lblRRFID.Text).Trim();
                Session["RRFID"] = RRFID;
                // Response.Redirect("SLAForRRF.aspx?RRFNO=" + RRFID);
                Response.Redirect("SLAForRRF.aspx");
            }
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtRRFCodeSearch.Text = string.Empty;
        btnAcceptRRF.Visible = true;
        btnSearchCandidate.Visible = true;
        btnScheduleInterview.Visible = true;
        btnViewRRF.Visible = true;
        btnViewProgress.Visible = true;
        btnViewSLA.Visible = true;
        grdRecruiter.Visible = true;
        BindData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtRRFCodeSearch.Text == "")
        {
            BindData();
        }
        else
        {
            BindDataForRRFSearch();
        }
    }

    public void BindDataForRRFSearch()
    {
        lblMessage.Visible = false;

        objRecruiterBOL.UserID = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
        if (Session["HRMRole"] != null)
            objRecruiterBOL.RoleType = "HRM";
        else
            objRecruiterBOL.RoleType = "Recruiter";
        btnAcceptRRF.Visible = true;
        btnSearchCandidate.Visible = true;
        btnScheduleInterview.Visible = true;
        btnViewRRF.Visible = true;
        btnViewProgress.Visible = true;
        btnViewSLA.Visible = true;
        objRecruiterBOL.RRFCode = txtRRFCodeSearch.Text.Trim();

        dsRRFDetails = objRecruiterBLL.SearchRRFCodeData(objRecruiterBOL);
        if (dsRRFDetails.Tables[0].Rows.Count != 0)
        {
            grdRecruiter.DataSource = dsRRFDetails;
            grdRecruiter.DataBind();
        }
        else
        {
            btnAcceptRRF.Visible = false;
            btnSearchCandidate.Visible = false;
            btnScheduleInterview.Visible = false;
            btnViewRRF.Visible = false;
            btnViewProgress.Visible = false;
            btnViewSLA.Visible = false;
            grdRecruiter.Visible = false;
            lblMessage.Text = "No Records Found";
            lblMessage.Visible = true;
        }
    }
}