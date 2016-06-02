using BLL;
using BOL;
using MailActivity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

public partial class InterviewFeedback : System.Web.UI.Page
{
    private DataSet dsSkills = new DataSet();
    private static DataSet dsInterviewFeedback = new DataSet();
    private static DataSet dsCandidateInfo = new DataSet();
    private DataSet dsFeedback = new DataSet();
    private static DataSet dsMail = new DataSet();
    private InterviewFeedbackBOL InterviewFeedbackBOL = new InterviewFeedbackBOL();
    private InterviewFeedbackBLL InterviewFeedbackBLL = new InterviewFeedbackBLL();

    private DataSet dsGetMailInfo = new DataSet();
    private DataSet dsGetEmployeeFromRole = new DataSet();
    private RRFApproverBLL objRRFApproverBLL = new RRFApproverBLL();
    private RRFApproverBOL objRRFApproverBOL = new RRFApproverBOL();
    private EmailActivityBLL objEmailActivityBLL = new EmailActivityBLL();
    private EmailActivityBOL objEmailActivityBOL = new EmailActivityBOL();
    private EmailActivity objEmailActivity = new EmailActivity();

    private int candidateID, RRFID, ScheduleID, StageID;

    private string Mode = string.Empty;
    private int RoundNumber, SrNo;
    private static string EmailId = string.Empty;
    private DataSet DsForBlankDataRow = new DataSet();

    //string candidateID = string.Empty;

    private List<InterviewFeedbackBOL> listOfActiveSkillsRecords = new List<InterviewFeedbackBOL>();
    private List<InterviewFeedbackBOL> listOfInsertedSkillsRecords = new List<InterviewFeedbackBOL>();
    private List<InterviewFeedbackBOL> listOfUpdatedSkillsRecords = new List<InterviewFeedbackBOL>();
    private List<InterviewFeedbackBOL> listOfDeletedSkillsRecords = new List<InterviewFeedbackBOL>();
    private HRInterviewAssessmentBOL objHRInterviewAssessmentBOL = new HRInterviewAssessmentBOL();
    private HRInterviewAssessmentBLL objHRInterviewAssessmentBLL = new HRInterviewAssessmentBLL();

    #region properties

    #region Skills properties

    public int SkillIndex
    {
        get
        {
            if (ViewState["SkillIndex"] != null)
            {
                return Convert.ToInt32(ViewState["SkillIndex"]);
            }
            else
                return 0;
        }
        set { ViewState["SkillIndex"] = value; }
    }

    private List<InterviewFeedbackBOL> ListOfActiveSkillsRecords
    {
        get
        {
            if (ViewState["listOfActiveSkillsRecords"] != null)
                return ViewState["listOfActiveSkillsRecords"] as List<InterviewFeedbackBOL>;
            else
                return null;
        }
        set
        {
            ViewState["listOfActiveSkillsRecords"] = value;
        }
    }

    private List<InterviewFeedbackBOL> ListOfInsertedSkillsRecords
    {
        get
        {
            if (ViewState["listOfInsertedSkillsRecords"] != null)
                return ViewState["listOfInsertedSkillsRecords"] as List<InterviewFeedbackBOL>;
            else
                return null;
        }
        set
        {
            ViewState["listOfInsertedSkillsRecords"] = value;
        }
    }

    private List<InterviewFeedbackBOL> ListOfUpdatedSkillsRecords
    {
        get
        {
            if (ViewState["listOfUpdatedSkillsRecords"] != null)
                return ViewState["listOfUpdatedSkillsRecords"] as List<InterviewFeedbackBOL>;
            else
                return null;
        }
        set
        {
            ViewState["listOfUpdatedSkillsRecords"] = value;
        }
    }

    private List<InterviewFeedbackBOL> ListOfDeletedSkillsRecords
    {
        get
        {
            if (ViewState["listOfDeletedSkillsRecords"] != null)
                return ViewState["listOfDeletedSkillsRecords"] as List<InterviewFeedbackBOL>;
            else
                return null;
        }
        set
        {
            ViewState["listOfDeletedSkillsRecords"] = value;
        }
    }

    public int Index
    {
        get
        {
            if (ViewState["Index"] != null)
            {
                return Convert.ToInt32(ViewState["Index"]);
            }
            else
                return 0;
        }
        set { ViewState["Index"] = value; }
    }

    #endregion Skills properties

    #endregion properties

    protected void Page_Load(object sender, EventArgs e)
    {
        lblSuccess.Visible = false;

        //candidateID = Convert.ToInt32(Request.QueryString["CandidateID"]);
        //RRFID = Convert.ToInt32(Request.QueryString["RRFID"]);
        //ScheduleID = Convert.ToInt32(Request.QueryString["ScheduleID"]);
        //StageID = Convert.ToInt32(Request.QueryString["StageID"]);
        //Mode = Convert.ToString(Request.QueryString["Mode"]);
        //RoundNumber = Convert.ToInt32(Request.QueryString["RoundNumber"]);
        //SrNo = Convert.ToInt32(Request.QueryString["SrNo"]);

        if (Session["CandidateID"] != null)
            candidateID = Convert.ToInt32(Convert.ToString(Session["CandidateID"]));

        if (Session["RRFID"] != null)
            RRFID = Convert.ToInt32(Convert.ToString(Session["RRFID"]));

        if (Session["ScheduleID"] != null)
            ScheduleID = Convert.ToInt32(Convert.ToString(Session["ScheduleID"]));

        if (Session["StageID"] != null)
            StageID = Convert.ToInt32(Convert.ToString(Session["StageID"]));

        if (Session["Mode"] != null)
            Mode = Convert.ToString(Session["Mode"]);

        if (Session["RoundNumber"] != null)
            RoundNumber = Convert.ToInt32(Convert.ToString(Session["RoundNumber"]));

        if (Session["SrNo"] != null)
            SrNo = Convert.ToInt32(Convert.ToString(Session["SrNo"]));

        if (Mode == "Read")
        {
            pnlInterviewFeedBack.Enabled = false;
            btnNextStage.Visible = false;
            btnReject.Visible = false;
            //btnPrint.Visible = true;
        }
        else
        {
            pnlInterviewFeedBack.Enabled = true;
            btnNextStage.Visible = true;
            btnReject.Visible = true;
            //btnPrint.Visible = false;
        }

        InterviewFeedbackBOL.ScheduleID = ScheduleID;

        dsInterviewFeedback = InterviewFeedbackBLL.GetDetails(InterviewFeedbackBOL);
        dsMail = dsInterviewFeedback;
        if (dsInterviewFeedback.Tables[0].Rows.Count > 0)
        {
            lblCandidateName.Text = dsInterviewFeedback.Tables[0].Rows[0]["CandidateName"].ToString();
            lblCompetency.Text = dsInterviewFeedback.Tables[0].Rows[0]["Competency"].ToString();
            lblInterviewDate.Text = dsInterviewFeedback.Tables[0].Rows[0]["InterviewDate"].ToString();
            lblInterviewer.Text = dsInterviewFeedback.Tables[0].Rows[0]["InterviewerName"].ToString();
        }

        InterviewFeedbackBOL.CandidateID = candidateID;
        InterviewFeedbackBOL.RRFNo = RRFID;
        InterviewFeedbackBOL.StageID = StageID;
        InterviewFeedbackBOL.ScheduleID = ScheduleID;
        dsFeedback = InterviewFeedbackBLL.GetinterviewFeedbackDetailsForCandidate(InterviewFeedbackBOL);

        if (dsFeedback.Tables[0].Rows.Count > 0)
        {
            txtLanguage.Text = dsFeedback.Tables[0].Rows[0]["LanguageProficiency"].ToString();
            txtCompliance.Text = dsFeedback.Tables[0].Rows[0]["Compliance"].ToString();
            txtOverallComments.Text = dsFeedback.Tables[0].Rows[0]["OverallComments"].ToString();
            txtProjectKnowledge.Text = dsFeedback.Tables[0].Rows[0]["CurrentProjectKnowledge"].ToString();
            string risk = (dsFeedback.Tables[0].Rows[0]["CandidateRiskProfile"].ToString()).Trim();

            if (Mode == "Read")
            {
                //if (risk == "Marginal Candidate - relatively high risk")
                //    rbtnMarginal.Checked = true;
                //else if (risk == "Average Candidate - normal risk")
                //    rbtnAverage.Checked = true;
                //else if (risk == "Strong Candidate - minimal risk")
                //    rbtnStrong.Checked = true;
                if (risk == ConfigurationSettings.AppSettings["MarginalCandidate"].ToString())
                    rbtnMarginal.Checked = true;
                else if (risk == ConfigurationSettings.AppSettings["AverageCandidate"].ToString())
                    rbtnAverage.Checked = true;
                else if (risk == ConfigurationSettings.AppSettings["StrongCandidate"].ToString())
                    rbtnStrong.Checked = true;
            }
        }

        dsSkills = InterviewFeedbackBLL.GetInterviewCoreSkillsDetails(InterviewFeedbackBOL);

        if (ListOfActiveSkillsRecords == null)
            ListOfActiveSkillsRecords = GetSkillsInList(dsSkills);

        if (ListOfInsertedSkillsRecords != null)
            listOfInsertedSkillsRecords = ListOfInsertedSkillsRecords;

        if (ListOfUpdatedSkillsRecords != null)
            listOfUpdatedSkillsRecords = ListOfUpdatedSkillsRecords;

        if (ListOfDeletedSkillsRecords != null)
            listOfDeletedSkillsRecords = ListOfDeletedSkillsRecords;

        if (!Page.IsPostBack)
        {
            //     For getting latest Experience ID
            if (SkillIndex == 0)
            {
                DataSet dsSkillIndex = InterviewFeedbackBLL.GetLatestInterviewCoreSkillID();
                if (dsSkillIndex.Tables.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dsSkillIndex.Tables[0].Rows[0][0].ToString()))
                        SkillIndex = Convert.ToInt32(dsSkillIndex.Tables[0].Rows[0][0]);
                }
            }
            else
                SkillIndex = 1;

            BindOnPageLoad(dsSkills, grdSkills);

            if (Mode == "Read")
            {
                if (dsFeedback.Tables[0].Rows.Count > 0)
                {
                    string risk = (dsFeedback.Tables[0].Rows[0]["CandidateRiskProfile"].ToString()).Trim();
                    rbtnMarginal.Checked = false;
                    rbtnAverage.Checked = false;
                    rbtnStrong.Checked = false;

                    if (risk == "Marginal Candidate - relatively high risk")
                        rbtnMarginal.Checked = true;
                    else if (risk == "Average Candidate - normal risk")
                        rbtnAverage.Checked = true;
                    else if (risk == "Strong Candidate - minimal risk")
                        rbtnStrong.Checked = true;
                }
            }
        }

        //btnNextStage.Attributes.Add("onclick", "CloseAndRefresh()");
    }

    private void BindOnPageLoad(DataSet dsSkills, GridView grdSkills)
    {
        try
        {
            DsForBlankDataRow = dsSkills;

            if (dsSkills.Tables[0].Rows.Count != 0)
            {
                grdSkills.DataSource = dsSkills.Tables[0];
                grdSkills.DataBind();
            }
            else
            {
                DataTable dt = dsSkills.Tables[0].Clone();
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);

                grdSkills.DataSource = dt;
                grdSkills.DataBind();
                grdSkills.Rows[0].Visible = false;
            }
        }
        catch (System.Exception ex)
        {
            throw new Exception("Exception in BindOnPageLoad(). Message:" + ex.Message);
        }
    }

    private List<InterviewFeedbackBOL> GetSkillsInList(DataSet dataSet)
    {
        try
        {
            List<InterviewFeedbackBOL> listRecords = new List<InterviewFeedbackBOL>();
            if (dataSet.Tables.Count > 0)
            {
                DataTable table = dataSet.Tables[0];

                foreach (DataRow record in table.Rows)
                {
                    listRecords.Add(new InterviewFeedbackBOL()
                    {
                        InterviewID = Convert.ToInt32(record["InterviewID"]),
                        Skills = Convert.ToString(record["Skills"]),
                        Rating = Convert.ToString(record["Rating"])
                    });
                }
            }
            return listRecords;
        }
        catch (Exception ex)
        {
            throw new Exception("Exception in GetSkillsInList(). Message:" + ex.Message);
        }
    }

    protected void grdSkills_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //GridView1.Rows[e.NewEditIndex].FindControl("txtEdit").Focus();
        //Button btnAddMore1 =  grdSkills.FooterRow.FindControl("btnAddMore1") as Button;
        //btnAddMore1.
        //TextBox txtSkills2 = grdSkills.FooterRow.FindControl("txtSkills1") as TextBox;

        //txtSkills2.Attributes.Add("onkeypress", "Focus();");

        if (e.CommandName == "Add")
        {
            //TextBox txtSkills1 = grdSkills.FooterRow.FindControl("txtSkills1") as TextBox;
            //TextBox txtRating1 = grdSkills.FooterRow.FindControl("txtRating1") as TextBox;
            //string SkillsTemp
            //this.ClientScript.RegisterStartupScript(GetType(), "Alert", "<script language='javascript'> Function Validate (" + txtSkills1 + "); </script>");

            {
                Index += 1;
                TextBox txtSkills1 = grdSkills.FooterRow.FindControl("txtSkills1") as TextBox;
                TextBox txtRating1 = grdSkills.FooterRow.FindControl("txtRating1") as TextBox;

                listOfActiveSkillsRecords = ListOfActiveSkillsRecords;

                listOfActiveSkillsRecords.Add(new InterviewFeedbackBOL()
                {
                    ID = Index,
                    Skills = txtSkills1.Text,
                    Rating = txtRating1.Text
                });

                listOfInsertedSkillsRecords.Add(new InterviewFeedbackBOL()
                {
                    ID = Index,
                    Skills = txtSkills1.Text,
                    Rating = txtRating1.Text
                });

                ListOfInsertedSkillsRecords = listOfInsertedSkillsRecords;
                ListOfActiveSkillsRecords = listOfActiveSkillsRecords;

                if (ListOfDeletedSkillsRecords != null && ListOfDeletedSkillsRecords.Count > 0)
                {
                    List<InterviewFeedbackBOL> tempDelete = new List<InterviewFeedbackBOL>();

                    foreach (InterviewFeedbackBOL emp in ListOfDeletedSkillsRecords)
                    {
                        if (emp.ID == Index)
                        {
                            tempDelete.Add(emp);
                        }
                    }

                    if (tempDelete != null)
                    {
                        foreach (InterviewFeedbackBOL emp in tempDelete)
                        {
                            if (ListOfDeletedSkillsRecords.Contains(emp))
                                ListOfDeletedSkillsRecords.Remove(emp);
                        }
                    }
                }

                grdSkills.DataSource = ListOfActiveSkillsRecords;
                grdSkills.DataBind();
            }
        }
        else if (e.CommandName == "Delete")
        {
            int c = grdSkills.Rows.Count;
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdSkills.Rows[index];

            Label lblID = row.FindControl("lblID") as Label;

            if (ListOfActiveSkillsRecords.Count > 0)  //Checks whether list contains items
            {
                InterviewFeedbackBOL itemToBeDeleted = ListOfActiveSkillsRecords.Where(l => l.ID == Convert.ToInt32(lblID.Text)).ToList<InterviewFeedbackBOL>().SingleOrDefault(); // item which is to be deleted

                if (itemToBeDeleted != null)
                {
                    ListOfActiveSkillsRecords.Remove(itemToBeDeleted);

                    //ListOfActiveRecords = listOfActiveRecords;
                }
            }

            if (listOfInsertedSkillsRecords.Count > 0)  //Checks whether list contains items
            {
                InterviewFeedbackBOL itemToBeDeleted = listOfInsertedSkillsRecords.Where(l => l.ID == Convert.ToInt32(lblID.Text)).ToList<InterviewFeedbackBOL>().SingleOrDefault(); // item which is to be deleted

                if (itemToBeDeleted != null)
                {
                    listOfInsertedSkillsRecords.Remove(itemToBeDeleted);

                    ListOfInsertedSkillsRecords = listOfInsertedSkillsRecords;
                }
            }

            if (listOfUpdatedSkillsRecords.Count > 0)  //Checks whether list contains items
            {
                InterviewFeedbackBOL itemToBeDeleted = listOfUpdatedSkillsRecords.Where(l => l.ID == Convert.ToInt32(lblID.Text)).ToList<InterviewFeedbackBOL>().SingleOrDefault(); // item which is to be deleted

                if (itemToBeDeleted != null)
                {
                    listOfUpdatedSkillsRecords.Remove(itemToBeDeleted);

                    ListOfUpdatedSkillsRecords = listOfUpdatedSkillsRecords;
                }
            }

            listOfDeletedSkillsRecords.Add(new InterviewFeedbackBOL() { ID = Convert.ToInt32(lblID.Text) });

            ListOfDeletedSkillsRecords = listOfDeletedSkillsRecords;

            if (ListOfActiveSkillsRecords != null && ListOfActiveSkillsRecords.Count > 0)
            {
                grdSkills.DataSource = ListOfActiveSkillsRecords;
                grdSkills.DataBind();
            }
            else if (dsSkills.Tables[0].Rows.Count == 1)
            {
                BindBlankGrid();
            }
            else
            {
                BindBlankGrid();
            }
        }
        //else if (e.CommandName == "Edit")
        //{
        //    int index = Convert.ToInt32(e.CommandArgument);
        //    GridViewRow row = grdSkills.Rows[index];

        //    Button btnRemove1 = row.FindControl("btnRemove1") as Button;
        //    Button btnEdit = row.FindControl("btnEdit") as Button;

        //    if (btnEdit.Text == "Edit")
        //    {
        //        btnEdit.Text = "Update";
        //        Label lblID = row.FindControl("lblID") as Label;
        //        Label lblExpSrNo = row.FindControl("lblExpSrNo") as Label;
        //        Label lblSkills = row.FindControl("lblSkills") as Label;
        //        Label lblRating = row.FindControl("lblRating") as Label;

        //        TextBox txtSkills = row.FindControl("txtSkills") as TextBox;
        //        TextBox txtRating = row.FindControl("txtRating") as TextBox;

        //        lblID.Visible = false;
        //        lblExpSrNo.Visible = false;
        //        lblSkills.Visible = false;
        //        lblRating.Visible = false;

        //        txtSkills.Visible = true;
        //        txtRating.Visible = true;
        //    }
        //    else if (btnEdit.Text == "Update")
        //    {
        //        Label lblID = row.FindControl("lblID") as Label;
        //        TextBox txtSkills = row.FindControl("txtSkills") as TextBox;
        //        TextBox txtRating = row.FindControl("txtRating") as TextBox;

        //        if (ListOfActiveSkillsRecords.Count > 0) //Checks whether list contains items
        //        {
        //            foreach (InterviewFeedbackBOL emp in ListOfActiveSkillsRecords)
        //            {
        //                if (emp.ID == Convert.ToInt32(lblID.Text))
        //                {
        //                    emp.Skills = txtSkills.Text;
        //                    emp.Rating = txtRating.Text;
        //                }
        //            }
        //        }

        //        bool insertedItem = false;

        //        if (listOfInsertedSkillsRecords.Count > 0) //Checks whether list contains items
        //        {
        //            foreach (InterviewFeedbackBOL emp in listOfInsertedSkillsRecords)
        //            {
        //                if (emp.ID == Convert.ToInt32(lblID.Text))
        //                {
        //                    insertedItem = true;

        //                    emp.Skills = txtSkills.Text;
        //                    emp.Rating = txtRating.Text;
        //                }
        //            }

        //            ListOfInsertedSkillsRecords = listOfInsertedSkillsRecords;
        //        }

        //        bool isUpdateItemAdded = false;

        //        if (!insertedItem && (listOfUpdatedSkillsRecords.Count > 0))
        //        {
        //            foreach (InterviewFeedbackBOL emp in listOfUpdatedSkillsRecords)
        //            {
        //                if (emp.ID == Convert.ToInt32(lblID.Text))
        //                {
        //                    emp.Skills = txtSkills.Text;
        //                    emp.Rating = txtRating.Text;

        //                    isUpdateItemAdded = true;
        //                }
        //            }
        //            ListOfUpdatedSkillsRecords = listOfUpdatedSkillsRecords;
        //        }

        //        if (!insertedItem && !isUpdateItemAdded)
        //        {
        //            listOfUpdatedSkillsRecords.Add(new InterviewFeedbackBOL()
        //            {
        //                ID = Convert.ToInt32(lblID.Text),
        //                Skills = txtSkills.Text,
        //                Rating = txtRating.Text
        //            });

        //            ListOfUpdatedSkillsRecords = listOfUpdatedSkillsRecords;
        //        }

        //        grdSkills.DataSource = ListOfActiveSkillsRecords;
        //        grdSkills.DataBind();
        //        //btnRemove1.Visible = true;
        //        grdSkills.FooterRow.Visible = true;
        //    }
        //}
    }

    protected void grdSkills_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Label lblID = grdSkills.Rows[e.RowIndex].FindControl("lblID") as Label;
        TextBox txtSkills = grdSkills.Rows[e.RowIndex].FindControl("txtSkills") as TextBox;
        TextBox txtRating = grdSkills.Rows[e.RowIndex].FindControl("txtRating") as TextBox;

        if (ListOfActiveSkillsRecords.Count > 0) //Checks whether list contains items
        {
            foreach (InterviewFeedbackBOL emp in ListOfActiveSkillsRecords)
            {
                if (emp.ID == Convert.ToInt32(lblID.Text))
                {
                    emp.Skills = txtSkills.Text;
                    emp.Rating = txtRating.Text;
                }
            }
        }

        bool insertedItem = false;

        if (listOfInsertedSkillsRecords.Count > 0) //Checks whether list contains items
        {
            foreach (InterviewFeedbackBOL emp in listOfInsertedSkillsRecords)
            {
                if (emp.ID == Convert.ToInt32(lblID.Text))
                {
                    insertedItem = true;

                    emp.Skills = txtSkills.Text;
                    emp.Rating = txtRating.Text;
                }
            }

            ListOfInsertedSkillsRecords = listOfInsertedSkillsRecords;
        }

        bool isUpdateItemAdded = false;

        if (!insertedItem && (listOfUpdatedSkillsRecords.Count > 0))
        {
            foreach (InterviewFeedbackBOL emp in listOfUpdatedSkillsRecords)
            {
                if (emp.ID == Convert.ToInt32(lblID.Text))
                {
                    emp.Skills = txtSkills.Text;
                    emp.Rating = txtRating.Text;

                    isUpdateItemAdded = true;
                }
            }
            ListOfUpdatedSkillsRecords = listOfUpdatedSkillsRecords;
        }

        if (!insertedItem && !isUpdateItemAdded)
        {
            listOfUpdatedSkillsRecords.Add(new InterviewFeedbackBOL()
            {
                ID = Convert.ToInt32(lblID.Text),
                Skills = txtSkills.Text,
                Rating = txtRating.Text
            });

            ListOfUpdatedSkillsRecords = listOfUpdatedSkillsRecords;
        }

        grdSkills.DataSource = ListOfActiveSkillsRecords;
        grdSkills.DataBind();
        //btnRemove1.Visible = true;
        grdSkills.FooterRow.Visible = true;

        //code to solve rowupdating issue

        grdSkills.EditIndex = -1;
        grdSkills.DataSource = ListOfActiveSkillsRecords;
        grdSkills.DataBind();
        //btnRemove1.Visible = true;
        grdSkills.FooterRow.Visible = true;

        //
    }

    protected void grdSkills_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdSkills.EditIndex = -1;
        grdSkills.DataSource = ListOfActiveSkillsRecords;
        grdSkills.DataBind();
        //btnRemove1.Visible = true;
        grdSkills.FooterRow.Visible = true;
    }

    private void BindBlankGrid()
    {
        try
        {
            foreach (GridViewRow row in grdSkills.Rows)
            {
                Label lblID = row.FindControl("lblID") as Label;
                Label lblExpSrNo = row.FindControl("lblExpSrNo") as Label;
                Label lblSkills = row.FindControl("lblSkills") as Label;
                Label lblRating = row.FindControl("lblRating") as Label;

                Button btnRemove1 = row.FindControl("btnRemove1") as Button;
                Button btnEdit = row.FindControl("btnEdit") as Button;

                lblID.Visible = false;
                lblExpSrNo.Visible = false;
                lblSkills.Visible = false;
                lblRating.Visible = false;

                btnRemove1.Visible = false;
                btnEdit.Visible = false;

                grdSkills.Rows[0].Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Exception in BindBlankGrid(). Message:" + ex.Message);
        }
    }

    protected void grdSkills_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //if (grdSkills.Rows.Count == 1)
        //    grdSkills.Rows[0].Visible = false;
    }

    protected void grdSkills_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdSkills.EditIndex = e.NewEditIndex;
        grdSkills.DataSource = ListOfActiveSkillsRecords;
        grdSkills.DataBind();
        //btnRemove1.Visible = true;
        grdSkills.FooterRow.Visible = true;
    }

    protected void btnNextStage_Click(object sender, EventArgs e)
    {
        btnNextStage.Enabled = false;
        btnReject.Enabled = false;

        int LatestInterviewID;
        CommonMethodForAddAndRejectCandidate();

        dsInterviewFeedback = InterviewFeedbackBLL.AddInterviewFeedbackDetails(InterviewFeedbackBOL);
        LatestInterviewID = Convert.ToInt32(dsInterviewFeedback.Tables[0].Rows[0]["InterviewID"].ToString());

        AddCoreSkillsRating(LatestInterviewID);

        objHRInterviewAssessmentBOL.RRFID = Convert.ToInt32(RRFID);
        objHRInterviewAssessmentBOL.CandidateID = Convert.ToInt32(candidateID);
        objHRInterviewAssessmentBOL.StageID = Convert.ToInt32(StageID);
        objHRInterviewAssessmentBOL.RoundNo = RoundNumber;
        objHRInterviewAssessmentBOL.SrNo = SrNo;
        objHRInterviewAssessmentBLL.UpdateCandidateScheduleDate(objHRInterviewAssessmentBOL);

        //btnNextStage.Enabled = false;
        //btnReject.Enabled = false;

        //Mailing Activity

        objRRFApproverBOL.Role = "HRM";
        dsGetEmployeeFromRole = objRRFApproverBLL.GetEmployeeFromRole(objRRFApproverBOL);
        string toID = dsGetEmployeeFromRole.Tables[0].Rows[0]["UserId"].ToString();
        for (int i = 1; i < dsGetEmployeeFromRole.Tables[0].Rows.Count; i++)
            toID = toID + ';' + dsGetEmployeeFromRole.Tables[0].Rows[i]["UserId"].ToString();
        objEmailActivityBOL.ToID = dsMail.Tables[0].Rows[0]["Recruiter"].ToString() + ";";//recruiter

        objEmailActivityBOL.CCID = toID + ";" + Convert.ToString(HttpContext.Current.User.Identity.Name) + ";";//HRM,Interviewer

        objEmailActivityBOL.FromID = Convert.ToInt32(HttpContext.Current.User.Identity.Name);//Interviewer
        objEmailActivityBOL.EmailTemplateName = "Interview Feedback";
        dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);
        DataSet dsmaildetails = new DataSet();
        dsmaildetails = InterviewFeedbackBLL.GetDetailsformail(InterviewFeedbackBOL);

        string body;

        body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
        //body = body.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
        //body = body.Replace("##skills##", (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString()));
        body = body.Replace("##comment##", txtOverallComments.Text);
        body = body.Replace("##Candidate Name##", lblCandidateName.Text);
        body = body.Replace("##InterviewDate##", (dsmaildetails.Tables[0].Rows[0]["ScheduledDatetime"].ToString()));

        char[] separator = new char[] { ';' };
        objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);

        objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
        objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
        objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
        objEmailActivityBOL.Body = body; //(dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
        objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
        objEmailActivityBOL.RRFNo = (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString());
        objEmailActivityBOL.skills = (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString());
        objEmailActivityBOL.Position = (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString());

        try
        {
            objEmailActivity.SendMail(objEmailActivityBOL);
            lblSuccess.Text = "Candidate approved and Interview Feedback submitted successfully.";
            lblSuccess.Visible = true;
        }
        catch (System.Exception ex)
        {
            lblSuccess.Text = "Candidate approved and Interview Feedback submitted successfully,but e-mails could not be sent.";
            lblSuccess.Visible = true;
        }

        //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script language=javascript>if (window.opener && !window.opener.closed) { parent.location.reload(true); }  </script>");
        //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
        this.ClientScript.RegisterStartupScript(GetType(), "CLOSE", "<script language='javascript'> opener.location.href = 'Interviewer.aspx'; window.close(); </script>");
    }

    private void CommonMethodForAddAndRejectCandidate()
    {
        InterviewFeedbackBOL.CandidateID = Convert.ToInt32(candidateID);
        InterviewFeedbackBOL.RRFNo = Convert.ToInt32(RRFID);
        InterviewFeedbackBOL.StageID = Convert.ToInt32(StageID);
        InterviewFeedbackBOL.FeedbackBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
        if (txtLanguage.Text.ToString() != "")
            InterviewFeedbackBOL.LanguageProficiency = Convert.ToInt32(txtLanguage.Text.ToString().Trim());
        else
            InterviewFeedbackBOL.LanguageProficiency = int.MinValue;
        if (txtCompliance.Text.ToString() != "")
            InterviewFeedbackBOL.Compliance = Convert.ToInt32(txtCompliance.Text.ToString().Trim());
        else
            InterviewFeedbackBOL.Compliance = int.MinValue;

        InterviewFeedbackBOL.CurrentProjectKnowledge = txtProjectKnowledge.Text;
        InterviewFeedbackBOL.OverallComments = txtOverallComments.Text;
        InterviewFeedbackBOL.ScheduleID = Convert.ToInt32(ScheduleID);
        if (rbtnMarginal.Checked == true)
        {
            //InterviewFeedbackBOL.CandidateRiskProfile = rbtnMarginal.Text;
            InterviewFeedbackBOL.CandidateRiskProfile = ConfigurationSettings.AppSettings["MarginalCandidate"].ToString();
        }
        else if (rbtnAverage.Checked == true)
        {
            //InterviewFeedbackBOL.CandidateRiskProfile = rbtnAverage.Text;
            InterviewFeedbackBOL.CandidateRiskProfile = ConfigurationSettings.AppSettings["AverageCandidate"].ToString();
        }
        else if (rbtnStrong.Checked == true)
        {
            //InterviewFeedbackBOL.CandidateRiskProfile = rbtnStrong.Text;
            InterviewFeedbackBOL.CandidateRiskProfile = ConfigurationSettings.AppSettings["StrongCandidate"].ToString();
        }
    }

    private void AddCoreSkillsRating(int LatestInterviewID)
    {
        try
        {
            SaveCoreSkillsRating(listOfInsertedSkillsRecords, listOfUpdatedSkillsRecords, listOfDeletedSkillsRecords, LatestInterviewID);
        }
        catch (Exception ex)
        {
            throw new Exception("Error in AddCoreSkillsRating(). Message:" + ex.Message);
        }
    }

    private void SaveCoreSkillsRating(List<BOL.InterviewFeedbackBOL> listOfInsertedSkillsRecords, List<BOL.InterviewFeedbackBOL> listOfUpdatedSkillsRecords, List<BOL.InterviewFeedbackBOL> listOfDeletedSkillsRecords, int LatestInterviewID)
    {
        try
        {
            InterviewFeedbackBOL.InterviewID = LatestInterviewID;
            DataSet dsTemp = InterviewFeedbackBLL.GetInterviewFeedbackID(InterviewFeedbackBOL); // Get all ExpIDs for candidate before inserting record

            if (listOfInsertedSkillsRecords != null && listOfInsertedSkillsRecords.Count > 0)
            {
                var listOfCandidateExperienceWithoutIDColumn = listOfInsertedSkillsRecords.Where(l => l.ID != null).Select(l => new { InterviewID = l.InterviewID, l.Skills, l.Rating });

                foreach (InterviewFeedbackBOL InterviewFeedBack in listOfInsertedSkillsRecords)
                {
                    InterviewFeedBack.InterviewID = Convert.ToInt32(LatestInterviewID);
                    InterviewFeedbackBLL.AddInterviewCoreSkillsDetails(InterviewFeedBack);
                }
            }

            if (listOfUpdatedSkillsRecords != null && listOfUpdatedSkillsRecords.Count > 0)
            {
                foreach (InterviewFeedbackBOL InterviewFeedBack in listOfUpdatedSkillsRecords)
                {
                    InterviewFeedBack.InterviewID = Convert.ToInt32(LatestInterviewID);
                    InterviewFeedbackBLL.UpdateCoreSkillsDetails(InterviewFeedBack.ID, InterviewFeedBack);
                }
            }

            if (listOfDeletedSkillsRecords != null && listOfDeletedSkillsRecords.Count > 0)
            {
                foreach (InterviewFeedbackBOL InterviewFeedBack in listOfDeletedSkillsRecords)
                {
                    foreach (DataRow row in dsTemp.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(row["ID"]) == InterviewFeedBack.ID)
                        {
                            InterviewFeedbackBLL.DeleteCoreSkillsDetails(InterviewFeedBack.ID);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Exception in SaveCoreSkillsRating(). Message:" + ex.Message);
        }
    }

    private static DataSet ds = new DataSet();
    private static InterviewFeedbackBLL objInterviewFeedbackBLL = new InterviewFeedbackBLL();

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetSkills(string prefixText)
    {
        ds = objInterviewFeedbackBLL.GetSkills(prefixText);
        List<string> IndPanNames = new List<string>();

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            IndPanNames.Add(ds.Tables[0].Rows[i][1].ToString());
        }
        return IndPanNames;
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        btnNextStage.Enabled = false;
        btnReject.Enabled = false;

        int LatestInterviewID;
        CommonMethodForAddAndRejectCandidate();

        dsInterviewFeedback = InterviewFeedbackBLL.UpdateRejectStatus(InterviewFeedbackBOL);
        LatestInterviewID = Convert.ToInt32(dsInterviewFeedback.Tables[0].Rows[0]["InterviewID"].ToString());

        AddCoreSkillsRating(LatestInterviewID);
        objHRInterviewAssessmentBOL.RRFID = Convert.ToInt32(RRFID);
        objHRInterviewAssessmentBOL.CandidateID = Convert.ToInt32(candidateID);
        objHRInterviewAssessmentBOL.StageID = Convert.ToInt32(StageID);
        objHRInterviewAssessmentBOL.RoundNo = RoundNumber;
        objHRInterviewAssessmentBOL.SrNo = SrNo;
        objHRInterviewAssessmentBLL.UpdateCandidateScheduleDate(objHRInterviewAssessmentBOL);

        //btnNextStage.Enabled = false;
        //btnReject.Enabled = false;

        //Mailing Activity

        objRRFApproverBOL.Role = "HRM";
        dsGetEmployeeFromRole = objRRFApproverBLL.GetEmployeeFromRole(objRRFApproverBOL);
        string toID = dsGetEmployeeFromRole.Tables[0].Rows[0]["UserId"].ToString();
        for (int i = 1; i < dsGetEmployeeFromRole.Tables[0].Rows.Count; i++)
            toID = toID + ';' + dsGetEmployeeFromRole.Tables[0].Rows[i]["UserId"].ToString();
        objEmailActivityBOL.ToID = dsMail.Tables[0].Rows[0]["Recruiter"].ToString() + ";";//recruiter

        objEmailActivityBOL.CCID = toID + ";" + Convert.ToString(HttpContext.Current.User.Identity.Name) + ";";//HRM,Interviewer

        objEmailActivityBOL.FromID = Convert.ToInt32(HttpContext.Current.User.Identity.Name);//Interviewer
        objEmailActivityBOL.EmailTemplateName = "Reject Candidate"; //"Interview Feedback";
        dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);

        DataSet dsmaildetails = new DataSet();
        dsmaildetails = InterviewFeedbackBLL.GetDetailsformail(InterviewFeedbackBOL);

        string body, rrfnomail;
        rrfnomail = Convert.ToString(InterviewFeedbackBOL.RRFNo);
        body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
        //body = body.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
        //body = body.Replace("##skills##", (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString()));
        body = body.Replace("##comment##", txtOverallComments.Text);
        body = body.Replace("##Candidate Name##", lblCandidateName.Text);
        body = body.Replace("##InterviewDate##", (dsmaildetails.Tables[0].Rows[0]["ScheduledDatetime"].ToString()));

        char[] separator = new char[] { ';' };
        objEmailActivityBOL.CandidateName = dsMail.Tables[0].Rows[0]["CandidateFirstName"].ToString();
        objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
        objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
        objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
        objEmailActivityBOL.RRFNo = (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString());
        objEmailActivityBOL.skills = (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString());
        objEmailActivityBOL.Position = (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString());
        objEmailActivityBOL.Body = body;//(dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
        objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
        try
        {
            objEmailActivity.SendMail(objEmailActivityBOL);
            lblSuccess.Text = "Candidate rejected and Interview Feedback submitted successfully.";
            lblSuccess.Visible = true;
        }
        catch (System.Exception ex)
        {
            lblSuccess.Text = "Candidate rejected and Interview Feedback submitted successfully,but e-mails could not be sent.";
            lblSuccess.Visible = true;
            throw new Exception("Exception in btnReject_Click(). Message:" + ex.Message);
        }

        this.ClientScript.RegisterStartupScript(GetType(), "CLOSE", "<script language='javascript'> opener.location.href = 'Interviewer.aspx'; window.close(); </script>");
    }

    protected void grdSkills_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox txtRating1 = e.Row.FindControl("txtRating1") as TextBox;
            TextBox txtSkills1 = e.Row.FindControl("txtSkills1") as TextBox;

            Button btnAddMore1 = e.Row.FindControl("btnAddMore1") as Button;
            btnAddMore1.Attributes.Add("onClick", "javascript:return Validate(" + txtSkills1.ClientID + " , " + txtRating1.ClientID + ");"); // Attribute added for insert using javascript validation
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtRating = e.Row.FindControl("txtRating") as TextBox;
            TextBox txtSkills = e.Row.FindControl("txtSkills") as TextBox;

            Button btnUpdate = e.Row.FindControl("btnUpdate") as Button;
            if (btnUpdate != null)
                btnUpdate.Attributes.Add("onClick", "javascript:return Validate(" + txtSkills.ClientID + " , " + txtRating.ClientID + ");");   // Attribute added for insert using javascript validation
        }
    }

    protected void grdSkills_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (Mode == "Read")
        {
            int rowCount = e.Row.Cells.Count;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[4].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Visible = false;
            }
        }
    }

    protected void grdSkills_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
    }

    //protected void grdSkills_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    grdSkills.PageIndex = e.NewPageIndex;
    //    grdSkills.DataSource = dsInterviewFeedback;
    //    grdSkills.DataBind();

    //}

    protected void confirmPrint(object sender, EventArgs e)
    {
        //ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>var mywindow = window.open();mywindow.document.write('<html><head><title>Technical Interview Assessment Form</title>');mywindow.document.write('</head><body >');mywindow.document.write($('#tblMain').html());mywindow.document.write('</body></html>');mywindow.print();mywindow.close();</script>");
    }
}