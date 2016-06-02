using BLL;
using BOL;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RRFStatus : System.Web.UI.Page
{
    private static DataSet dsRRfStatus = new DataSet();
    private static DataTable dtRRfStatus = new DataTable();
    private static DataView dvRRfStatus = new DataView();

    private DataSet dsGetTooltipInfo = new DataSet();
    private RRFStatusBLL objRRFStatusBLL = new RRFStatusBLL();
    private RRFStatusBOL objRRFStatusBOL = new RRFStatusBOL();
    private int k = -1;
    private int l = 0;
    private string RRFID;
    private string ShowButtons;
    private string candidateID = string.Empty;
    private int Candidatestausid = 0;
    private int noOfCandidatewithOfferedIssue = 0;
    private DataSet dsRRFListToReassign = new DataSet();
    private DataSet dsGetRFFNo = new DataSet();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    private bool flag = false;
    private int pageRowCount = 0;
    private string rrfNo = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            rrfNo = Convert.ToString(Session["RRFID"]);
            // rrfNo = (Request.QueryString["RRFID"] ?? "").Trim();

            if (!string.IsNullOrEmpty(rrfNo))
            {
                GetRRFNo(rrfNo);
            }
            BindData();
        }
    }

    public void GetRRFNo(string rrfNo)
    {
        objRRFStatusBOL.RRFNo = Convert.ToInt32(rrfNo);

        dsGetRFFNo = objRRFStatusBLL.GetRRFNo(objRRFStatusBOL);

        if (dsGetRFFNo != null)
        {
            lblRRFNO.Text = (dsGetRFFNo.Tables[0].Rows[0]["RRFNo"].ToString());
        }
    }

    public void BindRRFListtoReassign()
    {
        if (Session["HRMRole"] != null)
        {
            objRRFStatusBOL.RecruiterID = 0;
        }
        else
        {
            objRRFStatusBOL.RecruiterID = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
        }
        objRRFStatusBOL.RRFNo = Convert.ToInt32(Convert.ToString(Session["RRFID"]));
        dsRRFListToReassign = objRRFStatusBLL.GetRRFListToReassign(objRRFStatusBOL);
        ddlRRFReassign.Items.Clear();
        ddlRRFReassign.Items.Add("Select");
        for (int i = 0; i < dsRRFListToReassign.Tables[0].Rows.Count; i++)
        {
            ddlRRFReassign.Items.Add(new ListItem(dsRRFListToReassign.Tables[0].Rows[i]["RRFNo"].ToString(), dsRRFListToReassign.Tables[0].Rows[i]["ID"].ToString()));
        }
    }

    public void BindData()
    {
        //RRFID = Request.QueryString["RRFID"];
        //ShowButtons = Request.QueryString["ShowButtons"];
        RRFID = Convert.ToString(Session["RRFID"]);
        ShowButtons = Convert.ToString(Session["ShowButtons"]);
        //ShowButtons = "hello";

        if (!string.IsNullOrEmpty(RRFID))
        {
            lblMessage.Visible = false;
            objRRFStatusBOL.RRFNo = Convert.ToInt32(RRFID.ToString());

            dsRRfStatus = objRRFStatusBLL.GetRRFStatus(objRRFStatusBOL);
            if (dsRRfStatus.Tables[0].Rows.Count != 0)
            {
                grdCandidateProgress.DataSource = dsRRfStatus;
                grdCandidateProgress.DataBind();
                if (!string.IsNullOrEmpty(ShowButtons))
                {
                    BindRRFListtoReassign();
                    grdCandidateProgress.Columns[2].Visible = true;
                    btnSchedule.Visible = true;
                    btnReassign.Visible = true;
                    lblReassign.Visible = true;
                    ddlRRFReassign.Visible = true;
                }
                else
                {
                    grdCandidateProgress.Columns[2].Visible = false;
                }
            }
            else
            {
                lblMessage.Visible = true;
                btnReassign.Visible = false;
                btnSchedule.Visible = false;
            }
        }
        else
        {
            lblMessage.Visible = true;
        }
    }

    protected void grdCandidateProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (flag == false)
        {
            if (grdCandidateProgress.PageIndex > 0)
            {
                // logic added for paging.
                k = (grdCandidateProgress.PageIndex * 10) - 1;
                flag = true;
            }
        }

        if (l > 0)
        {
            int numberofStages = 0;
            string imgURL = null;
            int status = 0;
            string toolTipInfo = string.Empty;
            pageRowCount = (grdCandidateProgress.PageIndex + 1) * 10;

            k = k + 1;
            int numberofCandidates = dsRRfStatus.Tables[0].Rows.Count;

            if (pageRowCount < numberofCandidates)
                numberofCandidates = pageRowCount;

            if (k < (numberofCandidates))
            {
                numberofStages = Convert.ToInt32(dsRRfStatus.Tables[0].Rows[k]["NumberOfStages"]);

                if (numberofStages > 0)
                {
                    status = Convert.ToInt32(dsRRfStatus.Tables[0].Rows[k]["CurrentStageStatus"]);
                    //if (status == 7)
                    //    noOfCandidatewithOfferedIssue++;
                }
                objRRFStatusBOL.CandidateID = Convert.ToInt32(dsRRfStatus.Tables[0].Rows[k]["CandidateID"]);
                // objRRFStatusBOL.RRFNo = Convert.ToInt32(RRFID);
                objRRFStatusBOL.RRFNo = Convert.ToInt32(Convert.ToString(Session["RRFID"]));
                if (numberofStages > 0)
                {
                    switch (status)
                    {
                        case 9:
                            imgURL = "~/Images/New Design/on-hold.png";
                            break;

                        case 15:
                            imgURL = "~/Images/New Design/rejected.png";
                            break;

                        case 14:
                            imgURL = "~/Images/New Design/cleared.png";
                            break;

                        case 7:
                            imgURL = "~/Images/New Design/offer-issued.png";
                            break;

                        case 16:
                            imgURL = "~/Images/New Design/not-conducted.png";
                            break;

                        default:
                            break;
                    }

                    GridViewRow grd = e.Row;
                    if (grd.RowIndex > -1)
                    {
                        Table t1 = new Table();
                        t1.CellPadding = 10;
                        TableRow tr = new TableRow();
                        TableCell[] tc = new TableCell[numberofStages];
                        dsGetTooltipInfo = objRRFStatusBLL.GetTooltipInfo(objRRFStatusBOL);
                        if (dsGetTooltipInfo.Tables[0].Rows.Count > 0)
                        {
                            int i = 0;
                            for (i = 1; i < numberofStages; i++)
                            {
                                tc[i - 1] = new TableCell();
                                Image imgPreviousStages = new Image();
                                //imgPreviousStages.Height = 10;
                                //imgPreviousStages.Width = 10;

                                toolTipInfo = "Interviewer Name : " + Convert.ToString(dsGetTooltipInfo.Tables[0].Rows[i - 1]["InterviewerName"]) + "\r\nStage Name : " + Convert.ToString(dsGetTooltipInfo.Tables[0].Rows[i - 1]["StageName"]);
                                imgPreviousStages.ImageUrl = "~/Images/New Design/cleared.png";
                                imgPreviousStages.ToolTip = toolTipInfo;
                                tc[i - 1].Controls.Add(imgPreviousStages);
                                tr.Controls.AddAt((i - 1), tc[i - 1]);
                            }

                            TableCell tc1 = new TableCell();
                            grd = e.Row;
                            Image imgCurrentStage = new Image();
                            toolTipInfo = "Interviewer Name : " + Convert.ToString(dsGetTooltipInfo.Tables[0].Rows[i - 1]["InterviewerName"]) + "\nStage Name : " + Convert.ToString(dsGetTooltipInfo.Tables[0].Rows[i - 1]["StageName"]);
                            imgCurrentStage.ImageUrl = imgURL;
                            imgCurrentStage.ToolTip = toolTipInfo;
                            //imgCurrentStage.Height = 10;
                            //imgCurrentStage.Width = 10;

                            tc1.Controls.Add(imgCurrentStage);
                            tr.Controls.AddAt((numberofStages - 1), tc1);
                            t1.Controls.Add(tr);
                            grd.Cells[1].Controls.Add(t1);
                        }
                    }
                }
                else if (numberofStages == 0)
                {
                    Label lbl1 = new Label();
                    TableCell tc = new TableCell();
                    GridViewRow grd = e.Row;
                    lbl1.Text = "Initiated";
                    tc.Controls.Add(lbl1);
                    Table t1 = new Table();
                    TableRow tr = new TableRow();
                    tr.Controls.AddAt(0, tc);
                    t1.Controls.Add(tr);
                    grd.Cells[1].Controls.Add(t1);
                }
            }
        }
        else
            l = l + 1;
    }

    //protected void grdCandidateProgress_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "Schedule")
    //    {
    //        RRFID = Request.QueryString["RRFID"];
    //        GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
    //        Label lblID = (Label)grdCandidateProgress.Rows[gvr.RowIndex].FindControl("lblCandidateID");
    //        Response.Redirect("~/CandidateInterviewSchedule.aspx?CandidateID=" + lblID.Text + "&RRFID=" + RRFID);
    //    }
    //}
    protected void btnSchedule_Click(object sender, EventArgs e)
    {
        RRFID = Convert.ToString(Session["RRFID"]);

        foreach (GridViewRow gvRow in grdCandidateProgress.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblCandidateID = (Label)gvRow.FindControl("lblCandidateID");
                candidateID = Convert.ToString(lblCandidateID.Text).Trim();
                objRRFStatusBOL.CandidateID = Convert.ToInt32(candidateID);
                Label lblCandidateStageStatus = (Label)gvRow.FindControl("lblStage");
                Candidatestausid = Convert.ToInt32(lblCandidateStageStatus.Text);

                break;
            }
        }
        //if candidate status is 'Offer issued'  or 'Rejected' or 'On hold'
        //if (Candidatestausid == 7 || Candidatestausid == 15 || Candidatestausid == 9)
        //if ( Candidatestausid == 15 || Candidatestausid == 9)
        //{
        //    lblMessage.Visible = true;
        //    lblMessage.Text = "Cannot Schedule this candidate.";
        //}
        //else

        // above logic comment because if the candidate status is  'Rejected' or 'On hold' then we need to see his  feedback why candidate is on Hold or rejected state.

        if (!string.IsNullOrEmpty(candidateID))
        {
            lblMessage.Visible = false;
            Session["CandidateID"] = Convert.ToString(candidateID);
            Session["RRFID"] = Convert.ToString(RRFID);
            //Response.Redirect("~/CandidateInterviewSchedule.aspx?CandidateID=" + candidateID + "&RRFID=" + RRFID);
            Response.Redirect("CandidateInterviewSchedule.aspx");
        }
    }

    protected void btnReassign_Click(object sender, EventArgs e)
    {
        //offer issued to single candidate
        try
        {
            noOfCandidatewithOfferedIssue = Convert.ToInt32(dsRRfStatus.Tables[1].Rows[0]["noOfCandidatewithOfferedIssue"]);

            if (ddlRRFReassign.SelectedValue != "Select")
            {
                string stageStatus = string.Empty;
                foreach (GridViewRow gvRow in grdCandidateProgress.Rows)
                {
                    CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        Label lblCandidateID = (Label)gvRow.FindControl("lblCandidateID");
                        candidateID = Convert.ToString(lblCandidateID.Text).Trim();
                        objRRFStatusBOL.CandidateID = Convert.ToInt32(candidateID);

                        Label lblStage = (Label)gvRow.FindControl("lblStage");
                        stageStatus = Convert.ToString(lblStage.Text).Trim();

                        break;
                    }
                }
                switch (Convert.ToInt32(stageStatus))
                {
                    case 1:
                    case 2:
                    case 5:
                    case 6:
                    case 7:
                    case 9:
                    case 10:
                    case 14:
                    case 16:
                        objRRFStatusBOL.RRFNo = Convert.ToInt32(ddlRRFReassign.SelectedValue);
                        objRRFStatusBLL.ReassignRRF(objRRFStatusBOL);
                        BindData();
                        lblMessage.Visible = true;
                        //lblMessage.SkinID = "lblSuccess";
                        lblMessage.Text = "Reassigned Candidate to New RRF successfully.";
                        break;

                    default:
                        BindData();
                        lblMessage.Visible = true;
                        lblMessage.Text = "Cannot reassign the candidate as his stage is one of Black Listed/Cancelled/Offer Rejected/Withdrawn/Rejected/Joined.";
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        //    if (stageStatus == "7" || 1,2,5,6,7,9,10,14,16 )
        //    {
        //        objRRFStatusBOL.RRFNo = Convert.ToInt32(ddlRRFReassign.SelectedValue);
        //        objRRFStatusBLL.ReassignRRF(objRRFStatusBOL);
        //        BindData();
        //        lblMessage.Visible = true;
        //        //lblMessage.SkinID = "lblSuccess";
        //        lblMessage.Text = "Reassigned Candidate to New RRF successfully.";
        //    }
        //    else
        //    {
        //        BindData();
        //        lblMessage.Visible = true;
        //        lblMessage.Text = "Cannot reassign the candidate whose status is not Offer Issued.";
        //    }
        //}
        //else
        //{
        //    BindData();
        //    lblMessage.Visible = true;
        //    if (ddlRRFReassign.Items.Count > 1)
        //        lblMessage.Text = "First select RRF from List to Reassign.";
        //    else
        //        lblMessage.Text = "No RRFs are available to reassign the candidate.";

        //}
        //}
        ////if offer issued to none candidate
        //else
        //{
        //    BindData();
        //    lblMessage.Visible = true;
        //    lblMessage.Text = "Offer is not issued to any candidate, so can not reassign the candidate.";
        //}
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Descending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    private void SortGridView(string sortExpression, string direction)
    {
        dtRRfStatus = dsRRfStatus.Tables[0] as DataTable;

        dvRRfStatus = new DataView(dtRRfStatus);
        dvRRfStatus.Sort = sortExpression + direction;

        dtRRfStatus = dvRRfStatus.ToTable();
        DataSet sortedDs = new DataSet();
        sortedDs.Tables.Add(dtRRfStatus.Copy());
        dsRRfStatus = sortedDs;

        grdCandidateProgress.DataSource = dvRRfStatus;
        grdCandidateProgress.DataBind();
    }

    protected void grdCandidateProgress_Sorting(object sender, GridViewSortEventArgs e)
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

    protected void grdCandidateProgress_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCandidateProgress.PageIndex = e.NewPageIndex;
        grdCandidateProgress.DataSource = dsRRfStatus;
        grdCandidateProgress.DataBind();
    }
}