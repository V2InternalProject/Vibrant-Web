using BLL;
using BOL;
using MailActivity;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

public partial class RRFList : System.Web.UI.Page
{
    private static DataSet dsRRfList = new DataSet();
    private static DataTable dtRRfList = new DataTable();
    private static DataView dvRRfList = new DataView();
    private DataSet dsGetMailInfo = new DataSet();
    private DataSet OfferIssedToAnyCandidate = new DataSet();

    private RRFListBLL objRRFListBLL = new RRFListBLL();
    private RRFListBOL objRRFListBOL = new RRFListBOL();

    private EmailActivityBLL objEmailActivityBLL = new EmailActivityBLL();
    private EmailActivityBOL objEmailActivityBOL = new EmailActivityBOL();
    private EmailActivity objEmailActivity = new EmailActivity();

    // static string rrfId = string.Empty;
    // static string approvalStatus = string.Empty;
    private int checkOfferIssedToAnyCandidate = 0;

    private string title = string.Empty;
    private string rrfStatus = string.Empty;
    private string approvalStatus = string.Empty;
    private string requestor = string.Empty;
    private string approver = string.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        //lblMessage.Visible = false;
        if (!IsPostBack)
        {
            BindData();
            BindStatus();
        }
    }

    public void BindStatus()
    {
        ddStatusFilter.DataSource = objRRFListBLL.RRFStatus();
        ddStatusFilter.DataTextField = "RRFStatus";
        ddStatusFilter.DataValueField = "ID";
        ddStatusFilter.DataBind();
    }

    public void BindData()
    {
        lblMessage.Visible = false;
        lblSuccessMessage.Visible = false;

        title = Convert.ToString(Session["Title"]);

        string userId = Convert.ToString(HttpContext.Current.User.Identity.Name);
        objRRFListBOL.UserID = Convert.ToInt32(userId);

        if (Session["HRMRole"] != null)
        {
            btnCancelRRF.Visible = true;
        }
        else
        {
            btnCancelRRF.Visible = false;
        }

        //Approver
        if (title == "RRF Approver List")
        {
            if (Session["ApproverRole"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            Session["Role"] = "Approver";
            btnRejectRRF.Visible = true;
            btnAddNew.Visible = false;
            btnCancelRRF.Visible = false;
            btnCloseRRRF.Visible = false;
            objRRFListBOL.RoleType = "Approver";
            dsRRfList = objRRFListBLL.GetRRFForList(objRRFListBOL);
        }
        //Requestor
        if (title == "RRF List")
        {
            if (Session["RequestorRole"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            Session["Role"] = "Requestor";
            btnRejectRRF.Visible = false;
            btnAddNew.Visible = true;
            objRRFListBOL.RoleType = "Requestor";
            btnCancelRRF.Visible = false;
            btnCloseRRRF.Visible = false;
            dsRRfList = objRRFListBLL.GetRRFForList(objRRFListBOL);
        }
        //HRM
        if (title == "HRM RRFList")
        {
            if (Session["HRMRole"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            Session["Role"] = "HRM";
            btnRejectRRF.Visible = false;
            btnAddNew.Visible = false;
            btnCancelRRF.Visible = true;
            btnCloseRRRF.Visible = true;
            objRRFListBOL.RoleType = "HRM";
            dsRRfList = objRRFListBLL.GetRRFForList(objRRFListBOL);
        }

        if (dsRRfList.Tables[0].Rows.Count != 0)
        {
            objRRFListBOL.RoleType = Session["Role"].ToString();
            dsRRfList = objRRFListBLL.GetRRFForList(objRRFListBOL);
            grdRRF.DataSource = dsRRfList;
            grdRRF.DataBind();
        }
        else
        {
            lblSuccessMessage.Visible = false;
            lblMessage.Visible = true;
            lblMessage.Text = "No Records Found";
            if (title != "RRF List")
            {
                btnAddNew.Visible = false;
            }
            else
            {
                btnAddNew.Visible = true;
            }
            btnViewRRF.Visible = false;
            btnCandidateStatus.Visible = false;
            btnCancelRRF.Visible = false;
            btnRejectRRF.Visible = false;
            btnCloseRRRF.Visible = false;
            btnViewSLA.Visible = false;
        }
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("RRFRequestor.aspx");
    }

    protected void btnViewRRF_Click(object sender, EventArgs e)
    {
        title = Convert.ToString(Session["Title"]);

        foreach (GridViewRow gvRow in grdRRF.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                string RRFID = Convert.ToString(lblRRFID.Text).Trim();

                Label lblApprovalStatus = (Label)gvRow.FindControl("lblApprovalStatus");
                approvalStatus = Convert.ToString(lblApprovalStatus.Text).Trim();

                Session["RRFID"] = RRFID;

                if (title == "RRF Approver List" && approvalStatus == "Pending")
                    Response.Redirect("RRFApprover.aspx");
                else if (title == "RRF List" && approvalStatus == "Need Clarification")
                    Response.Redirect("RRFApprover.aspx");
                else
                    Response.Redirect("HRM.aspx");
            }
        }
    }

    protected void btnCandidateStatus_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdRRF.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                string RRFID = Convert.ToString(lblRRFID.Text).Trim();
                Session["RRFID"] = RRFID;
                Server.Transfer("RRFStatus.aspx");
            }
        }
    }

    protected void grdRRF_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }

    protected void btnCancelRRF_Click(object sender, EventArgs e)
    {
        string RRFID = string.Empty;
        lblMessage.Visible = false;
        foreach (GridViewRow gvRow in grdRRF.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                RRFID = Convert.ToString(lblRRFID.Text).Trim();

                Label lblRRFStatus = (Label)gvRow.FindControl("lblRRFStatus");
                rrfStatus = Convert.ToString(lblRRFStatus.Text).Trim();
            }
        }

        if (RRFID != string.Empty)
        {
            if (rrfStatus == "Yet To Begin" || rrfStatus == "In Progress")
            {
                objRRFListBOL.RRFID = Convert.ToInt32(RRFID);
                OfferIssedToAnyCandidate = objRRFListBLL.CheckOfferIssedToAnyCandidate(objRRFListBOL);

                if (checkOfferIssedToAnyCandidate != 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Offer is issued to Candidate against this RRF, So can not Cancel this RRF.";
                }
                else
                {
                    Session["RRFID"] = Convert.ToString(RRFID);

                    String fromid = Convert.ToString(HttpContext.Current.User.Identity.Name);
                    String ccid = String.Empty;
                    if (string.IsNullOrEmpty(dsRRfList.Tables[0].Rows[0]["RecruiterId"].ToString()))
                        ccid = fromid;
                    else
                        ccid = fromid + ";" + Convert.ToString(dsRRfList.Tables[0].Rows[0]["RecruiterId"].ToString());

                    Session["value1"] = "Cancel";
                    Session["value2"] = "Reason for Cancellation";
                    Session["toid"] = Convert.ToString(dsRRfList.Tables[0].Rows[0]["ApprovedById"]);//approver
                    Session["fromid"] = fromid;
                    Session["ccid"] = ccid;

                    Response.Redirect("RRFApproverComment.aspx");
                }
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "This action is not allowed";
            }
        }
    }

    protected void btnRejectRRF_Click(object sender, EventArgs e)
    {
        string RRFID = string.Empty;
        lblMessage.Visible = false;
        foreach (GridViewRow gvRow in grdRRF.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                RRFID = Convert.ToString(lblRRFID.Text).Trim();

                Label lblapprovalStatus = (Label)gvRow.FindControl("lblApprovalStatus");
                approvalStatus = Convert.ToString(lblapprovalStatus.Text).Trim();
            }
        }

        if (RRFID != string.Empty)
        {
            if (approvalStatus == "Need Clarification" || approvalStatus == "Pending")
            {
                objRRFListBOL.RRFID = Convert.ToInt32(RRFID);

                Session["RRFID"] = Convert.ToString(RRFID);

                Session["value1"] = "Reject";
                Session["value2"] = "Reason for Rejection";
                Session["toid"] = Convert.ToString(dsRRfList.Tables[0].Rows[0]["ApprovedById"]);//approver
                Session["fromid"] = Convert.ToString(HttpContext.Current.User.Identity.Name);
                Session["ccid"] = Convert.ToString(HttpContext.Current.User.Identity.Name);

                Response.Redirect("RRFApproverComment.aspx");
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "This action is not allowed";
            }
        }
    }

    protected void btnCloseRRRF_Click(object sender, EventArgs e)
    {
        string RRFID = string.Empty;
        lblMessage.Visible = false;
        foreach (GridViewRow gvRow in grdRRF.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                RRFID = Convert.ToString(lblRRFID.Text).Trim();

                Label lblRRFStatus = (Label)gvRow.FindControl("lblRRFStatus");
                rrfStatus = Convert.ToString(lblRRFStatus.Text).Trim();
            }
        }

        if (RRFID != string.Empty)
        {
            if (rrfStatus == "Yet To Begin" || rrfStatus == "In Progress")
            {
                objRRFListBOL.RRFID = Convert.ToInt32(RRFID);
                OfferIssedToAnyCandidate = objRRFListBLL.CheckOfferIssedToAnyCandidate(objRRFListBOL);

                //first check whether offer is issued to any candidate
                if (!string.IsNullOrEmpty(OfferIssedToAnyCandidate.Tables[0].Rows[0]["countCandidate"].ToString()) && (OfferIssedToAnyCandidate.Tables[0].Rows[0]["countCandidate"].ToString() != "0"))
                {
                    Session["RRFID"] = Convert.ToString(RRFID);
                    objRRFListBLL.CloseRRF(objRRFListBOL);
                    objRRFListBOL.RRFStatus = 3;
                    objRRFListBOL.ApprovalStatus = 0;
                    objRRFListBOL.ModifiedDate = DateTime.Now;
                    objRRFListBLL.UdateRRFValuesToApprove(objRRFListBOL);

                    String toid = Convert.ToString(dsRRfList.Tables[0].Rows[0]["ApprovedById"]);//approver
                    String fromid = Convert.ToString(HttpContext.Current.User.Identity.Name);
                    String ccid = String.Empty;
                    if (string.IsNullOrEmpty(dsRRfList.Tables[0].Rows[0]["RecruiterId"].ToString()))
                        ccid = fromid;
                    else
                        ccid = fromid + ";" + Convert.ToString(dsRRfList.Tables[0].Rows[0]["RecruiterId"].ToString());

                    //Mailing Activity

                    objEmailActivityBOL.ToID = Convert.ToString(toid) + ";";//requestor

                    objEmailActivityBOL.CCID = Convert.ToString(ccid) + ";";//approver

                    objEmailActivityBOL.FromID = Convert.ToInt32(fromid);//approver
                    objEmailActivityBOL.EmailTemplateName = "Close RRF";
                    dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);

                    char[] separator = new char[] { ';' };

                    objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
                    objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
                    objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
                    objEmailActivityBOL.Body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
                    objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);

                    if (OfferIssedToAnyCandidate.Tables[1].Rows.Count != 0)
                    {
                        objEmailActivityBOL.skills = (OfferIssedToAnyCandidate.Tables[1].Rows[0]["keyskills"].ToString());
                        objEmailActivityBOL.Position = (OfferIssedToAnyCandidate.Tables[1].Rows[0]["Designationname"].ToString());
                        objEmailActivityBOL.CandidateName = (OfferIssedToAnyCandidate.Tables[1].Rows[0]["FirstName"].ToString());
                        objEmailActivityBOL.RRFNo = (OfferIssedToAnyCandidate.Tables[1].Rows[0]["RRFNo"].ToString());
                    }
                    try
                    {
                        objEmailActivity.SendMail(objEmailActivityBOL);
                    }
                    catch (System.Exception ex)
                    {
                        lblSuccessMessage.Text = "RRF Closed Successfully ,but e-mails could not be sent.";
                    }

                    BindData();
                    lblSuccessMessage.Visible = true;
                    lblSuccessMessage.Text = "RRF Closed Successfully";
                }
                else if (checkOfferIssedToAnyCandidate > 1)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Offer is issued to more than one Candidates against this RRF, so can not Close this RRF.\nView Candidate Status for this RRF to view the Candidate.";
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Offer is not issued to a single candidate on this RRF, so can not Close this RRF.";
                }
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "This action is not allowed";
            }
        }
    }

    protected void grdRRF_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdRRF.PageIndex = e.NewPageIndex;
        grdRRF.DataSource = dsRRfList;
        grdRRF.DataBind();
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

    protected void grdRRF_Sorting(object sender, GridViewSortEventArgs e)
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

        dtRRfList = dsRRfList.Tables[0] as DataTable;

        dvRRfList = new DataView(dtRRfList);
        dvRRfList.Sort = sortExpression + direction;

        dtRRfList = dvRRfList.ToTable();
        DataSet sortedDs = new DataSet();
        sortedDs.Tables.Add(dtRRfList.Copy());
        dsRRfList = sortedDs;

        grdRRF.DataSource = dvRRfList;
        grdRRF.DataBind();
    }

    protected void btnViewSLA_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdRRF.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblRRFID = (Label)gvRow.FindControl("lblRRFID");
                string RRFID = Convert.ToString(lblRRFID.Text).Trim();

                Session["RRFID"] = RRFID;

                Response.Redirect("SLAForRRF.aspx");
            }
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtRRFNOSearch.Text = string.Empty;
        btnAddNew.Visible = true;
        btnViewRRF.Visible = true;
        btnCandidateStatus.Visible = true;
        btnCancelRRF.Visible = true;
        btnRejectRRF.Visible = true;
        btnCloseRRRF.Visible = true;
        btnViewSLA.Visible = true;
        grdRRF.Visible = true;
        BindData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //if (txtRRFNOSearch.Text == "" && )
        //{
        //    BindData();
        //}
        //else
        {
            BindDataForRRFSearch();
        }
    }

    public void BindDataForRRFSearch()
    {
        lblMessage.Visible = false;
        lblSuccessMessage.Visible = false;

        title = Convert.ToString(Session["Title"]);

        objRRFListBOL.UserID = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
        objRRFListBOL.RRFNo = txtRRFNOSearch.Text.Trim();
        objRRFListBOL.RRFStatus = int.Parse(ddStatusFilter.SelectedValue);
        //Approver
        if (title == "RRF Approver List")
        {
            Session["Role"] = "Approver";
            btnRejectRRF.Visible = true;
            btnAddNew.Visible = false;
            btnCancelRRF.Visible = false;
            btnCloseRRRF.Visible = false;
            objRRFListBOL.RoleType = "Approver";
            dsRRfList = objRRFListBLL.SearchRRFNoData(objRRFListBOL);
        }
        //Requestor
        if (title == "RRF List")
        {
            Session["Role"] = "Requestor";
            btnRejectRRF.Visible = false;
            btnAddNew.Visible = true;
            objRRFListBOL.RoleType = "Requestor";
            btnCancelRRF.Visible = false;
            btnCloseRRRF.Visible = false;
            dsRRfList = objRRFListBLL.SearchRRFNoData(objRRFListBOL);
        }
        //HRM
        if (title == "HRM RRFList")
        {
            Session["Role"] = "HRM";
            btnRejectRRF.Visible = false;
            btnAddNew.Visible = false;
            btnCancelRRF.Visible = true;
            btnCloseRRRF.Visible = true;
            objRRFListBOL.RoleType = "HRM";
            dsRRfList = objRRFListBLL.SearchRRFNoData(objRRFListBOL);
        }

        if (dsRRfList.Tables[0].Rows.Count != 0)
        {
            grdRRF.DataSource = dsRRfList;
            grdRRF.DataBind();
        }
        else
        {
            lblSuccessMessage.Visible = false;
            lblMessage.Visible = true;
            lblMessage.Text = "No Records Found";
            if (title != "RRF List")
            {
                btnAddNew.Visible = false;
            }
            else
            {
                btnAddNew.Visible = true;
            }
            btnViewRRF.Visible = false;
            btnCandidateStatus.Visible = false;
            btnCancelRRF.Visible = false;
            btnRejectRRF.Visible = false;
            btnCloseRRRF.Visible = false;
            btnViewSLA.Visible = false;
            grdRRF.Visible = false;
        }
    }
}