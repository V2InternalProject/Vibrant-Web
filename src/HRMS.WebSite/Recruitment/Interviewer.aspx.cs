using BLL;
using BOL;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Interviewer : System.Web.UI.Page
{
    private static DataSet dsInterviewer = new DataSet();
    private static DataSet dsFilteredInterviewer = new DataSet();
    private static string ShowFilteredData = "false";
    private InterviewerBLL objInterviewerBLL = new InterviewerBLL();
    private InterviewerBOL objInterviewerBOL = new InterviewerBOL();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    // string path = @"D:\Recruitment Resumes\";
    private string path = System.Configuration.ConfigurationManager.AppSettings["SmarttrackUploadedfilePath"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            BindData();
    }

    private void BindData()
    {
        objInterviewerBOL.Interviewer = Convert.ToInt32(User.Identity.Name);

        dsInterviewer = objInterviewerBLL.GetInterviewDetails(objInterviewerBOL);
        ShowFilteredData = "false";
        if (dsInterviewer.Tables.Count > 0)
        {
            if (dsInterviewer.Tables[0].Rows.Count > 0)
            {
                btnUpdateFeedback.Visible = true;
                btnViewRRF.Visible = true;
                btnViewFeedback.Visible = true;
                lblMsg.Visible = false;
                grdInterviwer.Visible = true;
                btnOpenResume.Visible = true;
                grdInterviwer.DataSource = dsInterviewer;
                grdInterviwer.DataBind();
            }
            else
            {
                btnUpdateFeedback.Visible = false;
                btnViewRRF.Visible = false;
                btnViewFeedback.Visible = false;
                lblMsg.Visible = true;
                lblMsg.Text = "No records found";
                grdInterviwer.Visible = false;
                btnOpenResume.Visible = false;
            }
        }
        else
        {
            btnUpdateFeedback.Visible = false;
            btnViewRRF.Visible = false;
            lblMsg.Visible = true;
            lblMsg.Text = "No records found";
            grdInterviwer.Visible = false;
            btnOpenResume.Visible = false;
        }
    }

    public void btnUpdateFeedback_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdInterviwer.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblCandidateID = (Label)gvRow.FindControl("lblCandidateID");
                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                Label lblStageID = (Label)gvRow.FindControl("lblStageID");
                Label lblPosition = (Label)gvRow.FindControl("lblPosition");
                Label lblStagename = (Label)gvRow.FindControl("lblStagename");
                Label lblRRFCode = (Label)gvRow.FindControl("lblRRFCode");
                Label lblCandidateName = (Label)gvRow.FindControl("lblCandidateName");
                Label lblScheduleID = (Label)gvRow.FindControl("lblScheduleID");
                Label lblRRFNo = (Label)gvRow.FindControl("lblRRFNo");

                Label lblRoundNo = (Label)gvRow.FindControl("lblRoundNo");
                Label lblSrNo = (Label)gvRow.FindControl("lblSrNo");

                string CandidateID = Convert.ToString(lblCandidateID.Text).Trim();
                string RRFID = Convert.ToString(lblRRFID.Text).Trim();
                string StageID = Convert.ToString(lblStageID.Text).Trim();
                string Position = Convert.ToString(lblPosition.Text).Trim();
                string Stagename = Convert.ToString(lblStagename.Text).Trim();
                string RRFNO = Convert.ToString(lblRRFCode.Text).Trim();
                string CandidateName = Convert.ToString(lblCandidateName.Text).Trim();
                string ScheduleID = Convert.ToString(lblScheduleID.Text).Trim();

                // Session["Role"] = "Interviewer";
                Session["RRFID"] = RRFID;
                Session["RRFNumber"] = RRFNO;
                Session["CandidateID"] = CandidateID;
                Session["RRFNo"] = RRFNO;
                Session["CandidateName"] = CandidateName;
                Session["StageName"] = Stagename;
                Session["RoundNo"] = lblRoundNo.Text;
                Session["DesignationName"] = Position;
                Session["ScheduleID"] = ScheduleID;
                Session["StageID"] = StageID;
                Session["SrNo"] = lblSrNo.Text;
                Session["Position"] = Position;
                if (StageID == "17")
                {
                    Response.Redirect("SelectedCandidate.aspx");
                    // Response.Redirect("SelectedCandidate.aspx?StageName=" + Stagename + "&CandidateID=" + CandidateID + "&RRFID=" + RRFID + "&DesignationName=" + Position + "&ScheduleID=" + ScheduleID + "&StageID=" + StageID + "&RRFNumber=" + lblRRFCode.Text + "&RoundNo=" + lblRoundNo.Text + "&SrNo=" + lblSrNo.Text);
                }
                break;
            }
        }
        //Added by Rahul Ramachandran: Issue ID 952
        //Call the client side event Validate() after the above process is done.
        ClientScript.RegisterStartupScript(GetType(), "CallFeedBack", "validate(1)", true);
    }

    //public void btnUpdateFeedback_Click(object sender, EventArgs e)
    // {
    //     foreach (GridViewRow gvRow in grdInterviwer.Rows)
    //     {
    //         CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
    //         if (chkSelect.Checked == true)
    //         {
    //             Label lblCandidateID = (Label)gvRow.FindControl("lblCandidateID");
    //             Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
    //             Label lblStageID = (Label)gvRow.FindControl("lblStageID");
    //             Label lblPosition = (Label)gvRow.FindControl("lblPosition");
    //             Label lblStagename = (Label)gvRow.FindControl("lblStagename");
    //             Label lblRRFCode = (Label)gvRow.FindControl("lblRRFCode");
    //             Label lblCandidateName = (Label)gvRow.FindControl("lblCandidateName");
    //             Label lblScheduleID = (Label)gvRow.FindControl("lblScheduleID");
    //             Label lblRRFNo = (Label)gvRow.FindControl("lblRRFNo");

    //             Label lblRoundNo = (Label)gvRow.FindControl("lblRoundNo");
    //             Label lblSrNo = (Label)gvRow.FindControl("lblSrNo");

    //             string CandidateID = Convert.ToString(lblCandidateID.Text).Trim();
    //             string RRFID = Convert.ToString(lblRRFID.Text).Trim();
    //             string StageID = Convert.ToString(lblStageID.Text).Trim();
    //             string Position = Convert.ToString(lblPosition.Text).Trim();
    //             string Stagename = Convert.ToString(lblStagename.Text).Trim();
    //             string RRFNO = Convert.ToString(lblRRFCode.Text).Trim();
    //             string CandidateName = Convert.ToString(lblCandidateName.Text).Trim();
    //             string ScheduleID = Convert.ToString(lblScheduleID.Text).Trim();

    //            // Session["Role"] = "Interviewer";
    //             Session["RRFID"] = RRFID;
    //             Session["RRFNumber"] = RRFNO;
    //             Session["CandidateID"] = CandidateID;
    //             Session["RRFNo"] = RRFNO;
    //             Session["CandidateName"] = CandidateName;
    //             Session["StageName"] = Stagename;
    //             Session["RoundNo"]= lblRoundNo.Text;
    //             Session["DesignationName"] = Position;
    //             Session["ScheduleID"] = ScheduleID;
    //              Session["StageID"] = StageID;
    //              Session["SrNo"]=lblSrNo.Text;
    //              Session["Position"] = Position;
    //              if (StageID == "17")
    //              {
    //                  Response.Redirect("SelectedCandidate.aspx");
    //                   // Response.Redirect("SelectedCandidate.aspx?StageName=" + Stagename + "&CandidateID=" + CandidateID + "&RRFID=" + RRFID + "&DesignationName=" + Position + "&ScheduleID=" + ScheduleID + "&StageID=" + StageID + "&RRFNumber=" + lblRRFCode.Text + "&RoundNo=" + lblRoundNo.Text + "&SrNo=" + lblSrNo.Text);
    //              }

    //         }
    //     }

    // }

    private void BindSearchData(DataSet dsFilteredInterviewer)
    {
        objInterviewerBOL.Interviewer = Convert.ToInt32(User.Identity.Name);
        ShowFilteredData = "true";

        if (dsFilteredInterviewer.Tables.Count > 0)
        {
            if (dsFilteredInterviewer.Tables[0].Rows.Count > 0)
            {
                btnUpdateFeedback.Visible = true;
                btnViewRRF.Visible = true;
                btnViewFeedback.Visible = true;
                lblMsg.Visible = false;
                grdInterviwer.Visible = true;
                grdInterviwer.DataSource = dsFilteredInterviewer;
                grdInterviwer.DataBind();
                btnOpenResume.Visible = true;
            }
            else
            {
                btnUpdateFeedback.Visible = false;
                btnViewRRF.Visible = false;
                btnViewFeedback.Visible = false;
                lblMsg.Visible = true;
                lblMsg.Text = "No records found";
                grdInterviwer.Visible = false;
                btnOpenResume.Visible = false;
            }
        }
        else
        {
            btnUpdateFeedback.Visible = false;
            btnViewRRF.Visible = false;
            lblMsg.Visible = true;
            lblMsg.Text = "No records found";
            grdInterviwer.Visible = false;
            btnOpenResume.Visible = false;
        }
    }

    protected void btnViewRRF_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdInterviwer.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                string RRFID = Convert.ToString(lblRRFID.Text).Trim();
                Session["Role"] = "Interviewer";
                Session["RRFID"] = RRFID;
                Response.Redirect("HRM.aspx");
            }
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

    protected void grdInterviwer_Sorting(object sender, GridViewSortEventArgs e)
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

    private void SortGridView(string sortExpression, string direction)
    {
        //  You can cache the DataTable for improving performance
        DataTable dt = new DataTable();
        if (ShowFilteredData == "true")
            dt = dsFilteredInterviewer.Tables[0];
        else if (ShowFilteredData == "false")
            dt = dsInterviewer.Tables[0];

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        dt = dv.ToTable();
        DataSet sortedDs = new DataSet();
        sortedDs.Tables.Add(dt.Copy());

        if (ShowFilteredData == "true")
            dsFilteredInterviewer = sortedDs;
        else if (ShowFilteredData == "false")
            dsInterviewer = sortedDs;

        grdInterviwer.DataSource = dv;
        grdInterviwer.DataBind();
    }

    protected void grdInterviwer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdInterviwer.PageIndex = e.NewPageIndex;
        if (ShowFilteredData == "true")
            grdInterviwer.DataSource = dsFilteredInterviewer;
        else if (ShowFilteredData == "false")
            grdInterviwer.DataSource = dsInterviewer;

        grdInterviwer.DataBind();
    }

    protected void btnViewFeedback_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdInterviwer.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblInterviewConducted = (Label)gvRow.FindControl("lblInterviewConducted");
                Label lblCandidateID = (Label)gvRow.FindControl("lblCandidateID");
                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                Label lblStageID = (Label)gvRow.FindControl("lblStageID");
                Label lblPosition = (Label)gvRow.FindControl("lblPosition");
                Label lblStagename = (Label)gvRow.FindControl("lblStagename");
                Label lblRRFCode = (Label)gvRow.FindControl("lblRRFCode");
                Label lblCandidateName = (Label)gvRow.FindControl("lblCandidateName");
                Label lblScheduleID = (Label)gvRow.FindControl("lblScheduleID");
                Label lblRRFNo = (Label)gvRow.FindControl("lblRRFNo");

                Label lblRoundNo = (Label)gvRow.FindControl("lblRoundNo");
                Label lblSrNo = (Label)gvRow.FindControl("lblSrNo");

                string SrNo = string.Empty;
                string roundNumber = string.Empty;

                if (!string.IsNullOrEmpty(lblSrNo.Text))
                {
                    SrNo = lblSrNo.Text.Trim();
                }

                if (!string.IsNullOrEmpty(lblRoundNo.Text))
                {
                    roundNumber = lblRoundNo.Text.Trim();
                }

                string interviewConducted = Convert.ToString(lblInterviewConducted.Text).Trim();
                string CandidateID = Convert.ToString(lblCandidateID.Text).Trim();
                string RRFID = Convert.ToString(lblRRFID.Text).Trim();
                string StageID = Convert.ToString(lblStageID.Text).Trim();
                string Position = Convert.ToString(lblPosition.Text).Trim();
                string Stagename = Convert.ToString(lblStagename.Text).Trim();
                string RRFNO = Convert.ToString(lblRRFCode.Text).Trim();
                string CandidateName = Convert.ToString(lblCandidateName.Text).Trim();
                string ScheduleID = Convert.ToString(lblScheduleID.Text).Trim();
                Session["CandidateName"] = CandidateName;

                //Session["Role"] = "Interviewer";
                //Session["RRFID"] = RRFID;
                Session["Position"] = Position;
                //Session["CandidateID"] = CandidateID;
                Session["RRFNo"] = RRFNO;
                Session["CandidateID"] = CandidateID;
                Session["DesignationName"] = Position;
                Session["ScheduleID"] = ScheduleID;
                Session["StageName"] = Stagename;
                Session["RRFID"] = RRFID;
                Session["StageID"] = StageID;
                Session["SrNo"] = SrNo;
                Session["RoundNumber"] = roundNumber;

                if (!string.IsNullOrEmpty(interviewConducted))
                {
                    if (StageID != "6" && StageID != "17" && StageID != "18")
                    {
                        lblMsg.Visible = false;
                        Session["Mode"] = "Read";
                        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('InterviewFeedback.aspx', null, 'height=700, width=1000,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no' );", true);

                        //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('InterviewFeedback.aspx?SrNo=" + SrNo + "&RoundNumber=" + roundNumber + "&CandidateID=" + CandidateID + "&RRFID=" + RRFID + "&ScheduleID=" + ScheduleID + "&StageID=" + StageID + "&Mode=Read&height=700, width=1000,status= no, resizable= no, scrollbars=yes, toolbar=no,location=no,menubar=no', null, 'status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no' );", true);
                        //window.open('InterviewFeedback.aspx?SrNo=' + SrNo + '&RoundNumber=' + RoundNumber + '&CandidateID=' + CandidateID + '&RRFID=' + RRFID + '&ScheduleID=' + ScheduleID + '&StageID=' + StageID + '&Mode=Read' + '', null, 'height=700, width=700,status= no, resizable= no, scrollbars=no, toolbar=no,location=no,menubar=no ');
                    }
                    else if (StageID == "6" || StageID == "18")
                    {
                        lblMsg.Visible = false;
                        Session["Mode"] = "Read";
                        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('HRInterviewAssessment.aspx', null, 'height=550, width=1200,status=yes,scrollbars=yes,toolbar=no,titlebar=yes,menubar=no,location=no' );", true);

                        //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('HRInterviewAssessment.aspx?SrNo=" + SrNo + "&RoundNumber=" + roundNumber + "&CandidateID=" + CandidateID + "&RRFID=" + RRFID + "&ScheduleID=" + ScheduleID + "&StageID=" + StageID + "&Mode=Read&height=550, width=1200,status= no, resizable= no, scrollbars=yes, toolbar=no,location=no,menubar=no', null, 'status=yes,scrollbars=yes,toolbar=no,menubar=no,location=no' );", true);
                        //window.open('HRInterviewAssessment.aspx?SrNo=' + SrNo + '&RoundNumber=' + RoundNumber + '&CandidateID=' + CandidateID + '&RRFID=' + RRFID + '&ScheduleID=' + ScheduleID + '&StageID=' + StageID + '&Mode=Read' + '', null, 'height=450, width=1000,status= no, resizable= no, scrollbars=no, toolbar=no,location=no,menubar=no ');
                    }
                    else
                    {
                        lblMsg.Visible = false;
                        Response.Redirect("SelectedCandidate.aspx");

                        //[PJ]//Response.Redirect("SelectedCandidate.aspx?StageName=" + Stagename + "&CandidateID=" + CandidateID + "&RRFID=" + RRFID + "&DesignationName=" + Position + "&Mode=Read&ScheduleID=" + ScheduleID + "&StageID=" + StageID + "&RRFNumber=" + lblRRFCode.Text + "&RoundNo =" + lblRoundNo.Text + "&SrNo=" + lblSrNo.Text);
                    }
                }
                else
                {
                    lblMsg.Visible = false;
                    Response.Redirect("SelectedCandidate.aspx");

                    //Response.Redirect("SelectedCandidate.aspx?StageName=" + Stagename + "&CandidateID=" + CandidateID + "&RRFID=" + RRFID + "&DesignationName=" + Position + "&Mode=Read&ScheduleID=" + ScheduleID + "&StageID=" + StageID + "&RRFNumber=" + lblRRFCode.Text + "&RoundNo =" + lblRoundNo.Text + "&SrNo=" + lblSrNo.Text);
                    //lblMsg.Visible = true;
                    //lblMsg.Text = "This action is not allowed.";
                }
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ShowFilteredData = "true";
        DataTable dtTempSerchResults = new DataTable();
        dtTempSerchResults = dsInterviewer.Tables[0].Clone();

        string fname = txtFirstName.Text.ToLower();
        string lname = txtLastName.Text.ToLower();

        for (int i = 0; i < dsInterviewer.Tables[0].Rows.Count; i++)
        {
            if (fname == string.Empty && lname == string.Empty)
            {
                dtTempSerchResults.ImportRow(dsInterviewer.Tables[0].Rows[i]);
            }
            else if ((dsInterviewer.Tables[0].Rows[i]["Firstname"].ToString().ToLower().Contains(fname)) && lname == string.Empty)
            {
                dtTempSerchResults.ImportRow(dsInterviewer.Tables[0].Rows[i]);
            }
            else if (fname == string.Empty && (dsInterviewer.Tables[0].Rows[i]["Lastname"].ToString().ToLower().Contains(lname)))
            {
                dtTempSerchResults.ImportRow(dsInterviewer.Tables[0].Rows[i]);
            }
            else if ((dsInterviewer.Tables[0].Rows[i]["Firstname"].ToString().ToLower().Contains(fname)) && (dsInterviewer.Tables[0].Rows[i]["Lastname"].ToString().ToLower().Contains(lname)))
            {
                dtTempSerchResults.ImportRow(dsInterviewer.Tables[0].Rows[i]);
            }

            //else
            //{
            //    lblMsg.Visible = true;
            //    lblMsg.Text = "No such record was found.";
            //}
        }

        if (dsFilteredInterviewer.Tables.Count == 1)
        {
            dsFilteredInterviewer.Tables.RemoveAt(0);
        }
        else
        {
            btnReset_Click(sender, e);
        }
        dsFilteredInterviewer.Tables.Add(dtTempSerchResults);
        BindSearchData(dsFilteredInterviewer);
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtFirstName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        BindData();
    }

    protected void btnOpenResume_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdInterviwer.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                try
                {
                    // path = Server.MapPath("~/Resumes/");
                    //GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblCandidateID = (Label)gvRow.FindControl("lblCandidateID");

                    //string sourcePath = path + "\\" + lblCandidateID.Text;

                    //sourcePath = Path.GetExtension(sourcePath);

                    string filename1 = lblCandidateID.Text + ".docx";
                    string filename2 = lblCandidateID.Text + ".doc";

                    FileInfo myDoc = new FileInfo(path + filename1);
                    FileInfo myDoc2 = new FileInfo(path + filename2);

                    if (myDoc.Exists)
                    {
                    }
                    else
                    {
                        myDoc = myDoc2;
                    }

                    if (myDoc.Exists)
                    {
                        Response.Clear();
                        Response.ContentType = "Application/msword";
                        Response.AddHeader("content-disposition", "attachment;filename=" + myDoc.Name);
                        Response.AddHeader("Content-Length", myDoc.Length.ToString());
                        Response.ContentType = "application/octet-stream";
                        Response.BufferOutput = true;
                        Response.WriteFile(myDoc.FullName);
                        Response.End();
                    }
                    else
                    {
                        //lblErrorMessage.Visible = true;
                        //lblErrorMessage.Text = "No Resume for this candidate was found.";
                        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "javascript:show_confirm();", true);

                        //System.Windows.Forms.MessageBox.Show("No Resume for this candidate was found.");
                    }
                }
                catch (FileNotFoundException ex)
                {
                    throw ex;
                }
            }
        }
    }
}