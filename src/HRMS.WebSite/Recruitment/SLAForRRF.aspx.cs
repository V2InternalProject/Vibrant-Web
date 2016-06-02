using BLL;
using BOL;
using System;
using System.Data;

public partial class SLAForRRF : System.Web.UI.Page
{
    private DataSet dsSLAForRRF = new DataSet();
    private DataSet dsGetRFFNo = new DataSet();

    private SLAForRRFBOL objSLAForRRFBOL = new SLAForRRFBOL();
    private SLAForRRFBLL objSLAForRRFBLL = new SLAForRRFBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //string RRFNO = Request.QueryString["RRFNO"].ToString();
            string RRFNO = Convert.ToString(Session["RRFID"]);
            objSLAForRRFBOL.RRFNo = Convert.ToInt32(RRFNO);
            if (!string.IsNullOrEmpty(RRFNO))
            {
                GetRRFNo(RRFNO);
            }

            dsSLAForRRF = objSLAForRRFBLL.GetDataForSLA(objSLAForRRFBOL);

            if (!string.IsNullOrEmpty(dsSLAForRRF.Tables[0].Rows[0]["ApprovedDate"].ToString()))
            {
                lblRRFApprove.Visible = true;
                lblRRFApproveDate.Visible = true;
                lblRRFApproveDate.Text = dsSLAForRRF.Tables[0].Rows[0]["ApprovedDate"].ToString();
                lblRRFApproveDate.ToolTip = "MM/dd/yyyy";
            }
            else
            {
                lblRRFApprove.Visible = false;
                lblRRFApproveDate.Visible = false;
            }

            for (int i = 0; i < 4; i++)
            {
                if (dsSLAForRRF.Tables[i].Rows[0]["Status"].ToString() == "Green")
                {
                    if (i == 0)
                    {
                        imgRRFAccepted.ImageUrl = "~/Images/New Design/cleared.png";
                        //imgRRFAccepted.ToolTip = dsSLAForRRF.Tables[i].Rows[0]["RRFAcceptedDate"].ToString();
                        lblStage1LapseDate.Text = dsSLAForRRF.Tables[i].Rows[0]["Stage1LapseDate"].ToString();
                    }
                    else if (i == 1)
                    {
                        imgInterviewScheduled.ImageUrl = "~/Images/New Design/cleared.png";
                        imgInterviewScheduled.ToolTip = dsSLAForRRF.Tables[i].Rows[0]["Count"].ToString() + "  Interview Scheduled";
                        lblStage2LapseDate.Text = dsSLAForRRF.Tables[i].Rows[0]["Stage2LapseDate"].ToString();
                    }
                    else if (i == 2)
                    {
                        imgCandidateSelected.ImageUrl = "~/Images/New Design/cleared.png";
                        imgCandidateSelected.ToolTip = dsSLAForRRF.Tables[i].Rows[0]["Count"].ToString() + " Candidate";
                        lblStage3LapseDate.Text = dsSLAForRRF.Tables[i].Rows[0]["Stage3LapseDate"].ToString();
                    }
                    else if (i == 3)
                    {
                        imgOfferGenerated.ImageUrl = "~/Images/New Design/cleared.png";
                        imgOfferGenerated.ToolTip = "Offer Issued to " + dsSLAForRRF.Tables[i].Rows[0]["Count"].ToString() + " Candidate";
                        lblStage4LapseDate.Text = dsSLAForRRF.Tables[i].Rows[0]["Stage4LapseDate"].ToString();
                    }
                }
                if (dsSLAForRRF.Tables[i].Rows[0]["Status"].ToString() == "Gray")
                {
                    if (i == 0)
                    {
                        imgRRFAccepted.ImageUrl = "~/Images/New Design/not-proceeded.png";
                        lblStage1LapseDate.Text = dsSLAForRRF.Tables[i].Rows[0]["Stage1LapseDate"].ToString();
                    }
                    else if (i == 1)
                    {
                        imgInterviewScheduled.ImageUrl = "~/Images/New Design/not-proceeded.png";
                        imgInterviewScheduled.ToolTip = dsSLAForRRF.Tables[i].Rows[0]["Count"].ToString() + "  Interview Scheduled";
                        lblStage2LapseDate.Text = dsSLAForRRF.Tables[i].Rows[0]["Stage2LapseDate"].ToString();
                    }
                    else if (i == 2)
                    {
                        imgCandidateSelected.ImageUrl = "~/Images/New Design/not-proceeded.png";
                        imgCandidateSelected.ToolTip = dsSLAForRRF.Tables[i].Rows[0]["Count"].ToString() + " Candidate Selected";
                        lblStage3LapseDate.Text = dsSLAForRRF.Tables[i].Rows[0]["Stage3LapseDate"].ToString();
                    }
                    else if (i == 3)
                    {
                        imgOfferGenerated.ImageUrl = "~/Images/New Design/not-proceeded.png";
                        imgOfferGenerated.ToolTip = "Offer Issued to " + dsSLAForRRF.Tables[i].Rows[0]["Count"].ToString() + " Candidate";
                        lblStage4LapseDate.Text = dsSLAForRRF.Tables[i].Rows[0]["Stage4LapseDate"].ToString();
                    }
                }
                if (dsSLAForRRF.Tables[i].Rows[0]["Status"].ToString() == "Red")
                {
                    if (i == 0)
                    {
                        imgRRFAccepted.ImageUrl = "~/Images/New Design/rejected.png";
                        lblStage1LapseDate.Text = dsSLAForRRF.Tables[i].Rows[0]["Stage1LapseDate"].ToString();
                    }
                    else if (i == 1)
                    {
                        imgInterviewScheduled.ImageUrl = "~/Images/New Design/rejected.png";
                        imgInterviewScheduled.ToolTip = dsSLAForRRF.Tables[i].Rows[0]["Count"].ToString() + "  Interview Scheduled";
                        lblStage2LapseDate.Text = dsSLAForRRF.Tables[i].Rows[0]["Stage2LapseDate"].ToString();
                    }
                    else if (i == 2)
                    {
                        imgCandidateSelected.ImageUrl = "~/Images/New Design/rejected.png";
                        imgCandidateSelected.ToolTip = dsSLAForRRF.Tables[i].Rows[0]["Count"].ToString() + "  Candidate Selected";
                        lblStage3LapseDate.Text = dsSLAForRRF.Tables[i].Rows[0]["Stage3LapseDate"].ToString();
                    }
                    else if (i == 3)
                    {
                        imgOfferGenerated.ImageUrl = "~/Images/New Design/rejected.png";
                        imgOfferGenerated.ToolTip = "Offer Issued to " + dsSLAForRRF.Tables[i].Rows[0]["Count"].ToString() + " Candidate";
                        lblStage4LapseDate.Text = dsSLAForRRF.Tables[i].Rows[0]["Stage4LapseDate"].ToString();
                    }
                }
            }
        }
        catch (MyException ex)
        {
            throw new MyException("", ex.InnerException);
        }
    }

    public void GetRRFNo(string rrfNo)
    {
        objSLAForRRFBOL.RRFNo = Convert.ToInt32(rrfNo);

        dsGetRFFNo = objSLAForRRFBLL.GetRRFNo(objSLAForRRFBOL);

        if (dsGetRFFNo != null)
        {
            lblRRFNO.Text = (dsGetRFFNo.Tables[0].Rows[0]["RRFNo"].ToString());
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Recruitment/Recruiter.aspx");
    }
}