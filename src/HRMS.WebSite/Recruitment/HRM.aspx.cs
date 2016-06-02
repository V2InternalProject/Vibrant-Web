using BLL;
using BOL;
using MailActivity;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HRM : System.Web.UI.Page
{
    private static DataSet dsRRFDetails = new DataSet();
    private DataSet dsRecruiterNames = new DataSet();
    private DataSet dsGetMailInfo = new DataSet();
    private DataSet dsSLAType = new DataSet();

    private HRMBLL objHRMBLL = new HRMBLL();
    private HRMBOL objHRMBOL = new HRMBOL();
    private EmailActivityBLL objEmailActivityBLL = new EmailActivityBLL();
    private EmailActivityBOL objEmailActivityBOL = new EmailActivityBOL();
    private EmailActivity objEmailActivity = new EmailActivity();

    private static int RRFStatus;
    private static int ApprovalStatus;
    private string RRFID;

    protected void Page_Load(object sender, EventArgs e)
    {
        //values of RRFID(To be picked up from URL Query String) and RecruiterID (To be picked up from LogIn Details)
        // RRFID = Request.QueryString["RRFID"];

        RRFID = Convert.ToString(Session["RRFID"]);

        if (!string.IsNullOrEmpty(RRFID))
        {
            objHRMBOL.RRFID = Convert.ToInt32(RRFID);
            objHRMBOL.AssignedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name.ToString());
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }
    }

    public void BindData()
    {
        dsRRFDetails = objHRMBLL.GetRRFToApprove(objHRMBOL);
        dsRecruiterNames = objHRMBLL.GetRecruiterNames();

        if (dsRRFDetails.Tables[0].Rows.Count == 0)
        {
            lblError.Text = "No Records to Show";
            lblError.Visible = true;
            pnlHRM.Visible = false;
            trSendForApproval.Visible = false;
        }
        else
        {
            lblError.Visible = false;
            lblSuccess.Visible = true;
            pnlHRM.Visible = true;

            txtRequestor.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["EmpName"]);
            txtRRFNo.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["RRFNo"]);
            txtForDU.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["DU"]);

            if (Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["DT"]) != null)
                txtForDT.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["DT"]);
            txtRequestDate.Text = Convert.ToDateTime((dsRRFDetails.Tables[0].Rows[0]["RequestDate"])).ToString("MM/dd/yyyy");
            if (Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["ExpectedClosureDate"]) != "")
            {
                DateTime expclosuredate = new DateTime();
                expclosuredate = Convert.ToDateTime(dsRRFDetails.Tables[0].Rows[0]["ExpectedClosureDate"]);
                txtExpectedClosureDate.Text = expclosuredate.ToString("MM/dd/yyyy");
            }

            if (Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["ProjectName"]) != null)
                txtProjectName.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["ProjectName"]);

            if (Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["ResourcePool"]) != null)
                txtResourcePool.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["ResourcePool"]);

            txtDesignation.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Designation"]);
            txtPositionsRequired.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["PositionsRequired"]);

            if (Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Ind1"]) != null)
                txtIndicativePanel1.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Ind1"]);

            if (Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Ind2"]) != null)
                txtIndicativePanel2.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Ind2"]);

            if (Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Ind3"]) != null)
                txtIndicativePanel3.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Ind3"]);

            if (Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["EType"]) != null)
                txtEmployementType.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["EType"]);
            txtKeySkills.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["KeySkills"]);
            txtExperience.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Experience"]);
            txtBuisnessJustification.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["BusinessJustification"]);
            txtBudget.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Budget"]);

            if (Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["AdditionalInfo"]) != null)
                txtAdditionalInformation.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["AdditionalInfo"]);
            txtComments.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Comments"]);
            txtApproverName.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Approver"]);
            txtSLAForTechnology.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["SLAForSkill"]);
            //txtTotalSLADays.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Days"]);
            int i = 0;
            //for (i = 0; i < dsRecruiterNames.Tables[0].Rows.Count; i++)
            //{
            //    lstRecruiterName.Items.Add(new ListItem(dsRecruiterNames.Tables[0].Rows[i]["EmployeeName"].ToString(), dsRecruiterNames.Tables[0].Rows[i]["UserId"].ToString()));
            //}
            //lstRecruiterName.Items.Add(dsRecruiterNames.Tables[0].Rows[i]["EmployeeName"].ToString());

            //if (dsRRFDetails.Tables[0].Rows[0]["Recruiter"].ToString() != "")
            //{
            //    for (int k = 1; k < lstRecruiterName.Items.Count ; k++)
            //    {
            //        if (lstRecruiterName.Items[k].Value.ToString() == dsRRFDetails.Tables[0].Rows[0]["Recruiter"].ToString())
            //        {
            //            lstRecruiterName.Items[k].Selected = true;
            //            lstRecruiterName.Items[k].Enabled = true;
            //        }
            //        else
            //        {
            //            lstRecruiterName.Items[k].Enabled = false;
            //        }

            //    }

            //}

            string RecruiterName = dsRRFDetails.Tables[0].Rows[0]["Recruiter"].ToString();
            for (i = 0; i < dsRecruiterNames.Tables[0].Rows.Count; i++)
            {
                lstRecruiterName.Items.Add(new ListItem(dsRecruiterNames.Tables[0].Rows[i]["EmployeeName"].ToString(), dsRecruiterNames.Tables[0].Rows[i]["UserId"].ToString()));
            }
            for (int k = 0; k < lstRecruiterName.Items.Count; k++)
            {
                //foreach (ListItem  item in lstRecruiterName.Items )
                //{
                if (lstRecruiterName.Items[k].Value.Equals(RecruiterName))
                {
                    lstRecruiterName.Items[k].Selected = true;
                    lstRecruiterName.Items[k].Enabled = true;
                    Session["OldRecruiterID"] = lstRecruiterName.Items[k].Value.ToString();
                }
                else
                {
                    if (Session["HRMRole"] != null)
                    {
                        if ((dsRRFDetails.Tables[0].Rows[0]["RRFStatus"].ToString() == "3") || ((dsRRFDetails.Tables[0].Rows[0]["RRFStatus"].ToString() == "4")))
                        {
                            lstRecruiterName.Items[k].Enabled = false;
                        }
                        else if ((dsRRFDetails.Tables[0].Rows[0]["RRFStatus"].ToString() == "1") || ((dsRRFDetails.Tables[0].Rows[0]["RRFStatus"].ToString() == "2")))
                        {
                            lstRecruiterName.Items[k].Enabled = true;
                        }
                    }
                }
                //}
            }

            dsSLAType = objHRMBLL.GetSLAType();
            if (dsRRFDetails.Tables[0].Rows[0]["SLAType"] != DBNull.Value)
            {
                int SLAType = Convert.ToInt32(dsRRFDetails.Tables[0].Rows[0]["SLAType"]);

                for (int k = 0; k < dsSLAType.Tables[0].Rows.Count; k++)
                {
                    if (dsSLAType.Tables[0].Rows[k]["ID"].Equals(SLAType))
                    {
                        string a = Convert.ToString(dsSLAType.Tables[0].Rows[0]["SLAType"]);
                        lbl_ddlSLATypeName.Text = a;
                    }
                }
            }
            else
                lbl_ddlSLATypeName.Text = "";
            ddlSLATypeName.Items.Clear();
            ddlSLATypeName.Items.Add("Select");
            for (int s = 0; s < dsSLAType.Tables[0].Rows.Count; s++)
            {
                ddlSLATypeName.Items.Add(new ListItem(dsSLAType.Tables[0].Rows[s]["SLAType"].ToString(), dsSLAType.Tables[0].Rows[s]["ID"].ToString()));
            }

            if (Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["SLAType"]) != null && Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["SLAType"]) != "")
                ddlSLATypeName.Items.FindByValue(Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["SLAType"])).Selected = true;

            bool checkRplacement = Convert.ToBoolean(dsRRFDetails.Tables[0].Rows[0]["IsReplacement"]);
            if (checkRplacement == true)
            {
                rdobtnIsReplacement.SelectedValue = "Yes";
                lblReplacementFor.Visible = true;
                txtReplacementFor.Visible = true;
                txtReplacementFor.Text = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["ReplacementFor"]);
            }
            else
                rdobtnIsReplacement.SelectedValue = "No";

            bool checkIsBillable = Convert.ToBoolean(dsRRFDetails.Tables[0].Rows[0]["IsBillable"]);
            if (checkIsBillable == true)
                rdobtnIsBillable.SelectedValue = "Yes";
            else
                rdobtnIsBillable.SelectedValue = "No";

            //Get Approval status and RRF Status and Role
            RRFStatus = Convert.ToInt32(dsRRFDetails.Tables[0].Rows[0]["RRFStatus"]);
            ApprovalStatus = Convert.ToInt32(dsRRFDetails.Tables[0].Rows[0]["ApprovalStatus"]);

            //Show welcome note according to the role
            string Role = Session["Role"].ToString();
            lblWelcome.Text = Role + " View";

            //Make Certain data hidden and make them visible as per the RRF Status later.
            trBudget.Visible = false;
            trSendForApproval.Visible = false;
            trRecruiter.Visible = false;
            lblComments.Visible = false;
            txtComments.Visible = false;

            if (ApprovalStatus == 1)  //Approval status == Pending
            {
                trBudget.Visible = false;
                trSendForApproval.Visible = false;
                trRecruiter.Visible = false;
                lblComments.Visible = false;
                txtComments.Visible = false;
            }
            else if (ApprovalStatus == 3)  //Approval status == Needs Clarification
            {
                trBudget.Visible = false;
                trSendForApproval.Visible = false;
                trRecruiter.Visible = false;
                lblComments.Visible = true;
                txtComments.Visible = true;
            }
            else if (ApprovalStatus == 4)  //Approval status == Rejected
            {
                trBudget.Visible = false;
                trSendForApproval.Visible = false;
                trRecruiter.Visible = false;
                lblComments.Visible = true;
                txtComments.Visible = true;
            }
            else if (ApprovalStatus == 2)  //Approval status == Approved
            {
                if ((RRFStatus == 1 || RRFStatus == 2 || RRFStatus == 3) && (Role == "Requestor" || Role == "Recruiter" || Role == "Interviewer")) //For a Requestor,Recruiter,Interviewer with Approval status == Approved AND RRFStatus == Yet to begin OR InProgress  OR Closed
                {
                    lbl_ddlSLATypeName.Visible = true;
                    ddlSLATypeName.Visible = false;
                    trBudget.Visible = false;
                    trSendForApproval.Visible = false;
                    trRecruiter.Visible = false;
                    lblComments.Visible = false;
                    txtComments.Visible = false;
                    lblSuccess.Visible = false;
                }
                else if ((RRFStatus == 1 || RRFStatus == 2 || RRFStatus == 3) && Role == "Approver") //For an Approver with Approval status == Approved AND RRFStatus == Yet to begin OR InProgress  OR Closed
                {
                    lbl_ddlSLATypeName.Visible = true;
                    ddlSLATypeName.Visible = false;
                    trBudget.Visible = true;
                    trSendForApproval.Visible = false;
                    trRecruiter.Visible = false;
                    lblComments.Visible = false;
                    txtComments.Visible = false;
                }
                else if ((RRFStatus == 1 || RRFStatus == 2) && Role == "HRM")  //For a HRM with Approval status == Approved AND RRFStatus == Yet to begin OR InProgress
                {
                    lbl_ddlSLATypeName.Visible = false;
                    ddlSLATypeName.Visible = true;
                    trBudget.Visible = true;
                    trSendForApproval.Visible = true;
                    trRecruiter.Visible = true;
                    lblComments.Visible = false;
                    txtComments.Visible = false;
                    if (!string.IsNullOrEmpty(Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Recruiter"])))
                        btnConfirmAndAssign.Text = "Reassign Recruiter";
                }
                else if (RRFStatus == 3 && Role == "HRM")  //For a HRM with Approval status == Closed
                {
                    lbl_ddlSLATypeName.Visible = true;
                    ddlSLATypeName.Visible = false;
                    trBudget.Visible = true;
                    trSendForApproval.Visible = false;
                    trRecruiter.Visible = true;
                    lblComments.Visible = false;

                    txtComments.Visible = false;
                }
                else if (RRFStatus == 4 && Role == "Requestor")   //For a RRFRequestor with Approval status == Cancelled
                {
                    trBudget.Visible = false;
                    trSendForApproval.Visible = false;
                    trRecruiter.Visible = false;
                    lblComments.Visible = true;
                    txtComments.Visible = true;
                }
                else if (RRFStatus == 4 && Role == "Approver")  //For a RRFApprover with Approval status == Cancelled
                {
                    trBudget.Visible = true;
                    trSendForApproval.Visible = false;
                    trRecruiter.Visible = false;
                    lblComments.Visible = true;
                    txtComments.Visible = true;
                }
                else if (RRFStatus == 4 && Role == "HRM")  //For a RRFHRM with Approval status == Cancelled
                {
                    trBudget.Visible = true;
                    trSendForApproval.Visible = false;
                    trRecruiter.Visible = true;
                    lblComments.Visible = true;
                    txtComments.Visible = true;
                }

                if ((Role == "Recruiter" || Role == "Interviewer") && RRFStatus == 2)
                {
                    if (ddlSLATypeName.SelectedItem.Value == "Select")
                    {
                        trSLAType.Visible = false;
                    }
                    else
                    {
                        trSLAType.Visible = true;
                        ddlSLATypeName.Enabled = false;
                    }
                }
                if (Role == "Recruiter" && RRFStatus == 4)
                {
                    if (ddlSLATypeName.SelectedItem.Value == "Select")
                    {
                        trSLAType.Visible = false;
                    }
                    else
                    {
                        trSLAType.Visible = true;
                        ddlSLATypeName.Enabled = false;
                    }
                }
            }
            if (Role == "Requestor" || Role == "Approver")
            {
                trSLAType.Visible = false;
            }
            //txtRRFhistory.Attributes.Add("onkeydown", "return false");
            //List<KeyValuePair<int, string>> listOfCandidateHistory = GetHistory();
            ////AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
            //foreach (var k in listOfCandidateHistory)
            //{
            //    txtRRFhistory.Text += k.Value + Environment.NewLine;

            //}
        }
    }

    //private List<KeyValuePair<int, string>> GetHistory()
    //{
    //    try
    //    {
    //        dsRRFDetails = objHRMBLL.GetRRFToApprove(objHRMBOL);
    //        dsRecruiterNames = objHRMBLL.GetRecruiterNames();
    //        string RecruiterName = dsRRFDetails.Tables[0].Rows[0]["Recruiter"].ToString();
    //        List<KeyValuePair<int, string>> listRRFHistory = new List<KeyValuePair<int, string>>();
    //        listRRFHistory.Add(new KeyValuePair<int, string>(Convert.ToInt32("0"), Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["Approver"].ToString() + " " + "RRF Approved")));
    //        foreach (DataRow rs in dsRecruiterNames.Tables[0].Rows)
    //        {
    //            if (rs[0].ToString() == RecruiterName)
    //            {
    //                listRRFHistory.Add(new KeyValuePair<int, string>(Convert.ToInt32("1"), Convert.ToString("RRF Assigned To" + " " + rs[1].ToString())));
    //            }

    //        }

    //        //foreach (DataRow courseType in dsCandidateHistory.Tables[0].Rows)
    //        //{
    //        //    DateTime dt = Convert.ToDateTime(courseType[13]);
    //        //    listOfCandidateHistory.Add(new KeyValuePair<int, string>(Convert.ToInt32(courseType[0].ToString()), Convert.ToString(Convert.ToDateTime(courseType[13]).ToString("dd/MM/yyyy") + " " + (courseType[14]).ToString().Trim() + " " + courseType[6].ToString().Trim() + " " + courseType[8].ToString().Trim())));
    //        //}
    //        return listRRFHistory;
    //    }
    //    catch (System.Exception ex)
    //    {
    //        throw new MyException(" Message:" + ex.Message, ex.InnerException);
    //    }

    //}
    protected void btnConfirmAndAssign_Click(object sender, EventArgs e)
    {
        objHRMBOL.RecruiterID = Convert.ToInt32(lstRecruiterName.SelectedItem.Value);
        //DateTime currentTime = DateTime.Now;
        //String date = currentTime.ToString("MM/dd/yyyy");
        objHRMBOL.SLAType = Convert.ToInt32(ddlSLATypeName.SelectedItem.Value);
        objHRMBOL.AssignedDate = DateTime.Now;
        objHRMBOL.ModifiedDate = DateTime.Now;
        objHRMBOL.ModifiedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name.ToString());

        int TimesReassigned = Convert.ToInt32(dsRRFDetails.Tables[0].Rows[0]["TimesReassigned"]);
        if (TimesReassigned == 1)
        {
            objHRMBLL.SetRecruiterToRRF(objHRMBOL);

            lblSuccess.Visible = true;
            lblSuccess.Text = "This RRF has been successfully assigned.";
            pnlHRM.Visible = false;
            btnBack.Visible = false;
            btnRedirect.Visible = true;
            btnConfirmAndAssign.Visible = false;
            btnCancel.Visible = false;
            //Mailing Activity

            objEmailActivityBOL.ToID = Convert.ToString(lstRecruiterName.SelectedItem.Value) + ";";//recruiter
            objEmailActivityBOL.CCID = Convert.ToString(HttpContext.Current.User.Identity.Name) + ";";//hrm

            objEmailActivityBOL.FromID = Convert.ToInt32(HttpContext.Current.User.Identity.Name);//hrm
            objEmailActivityBOL.EmailTemplateName = "Confirm And Assign Recruiter";
            dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);

            string body;

            body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            //body = body.Replace("##RRFNO##", txtRRFNo.Text);
            //body = body.Replace("##skills##", txtKeySkills.Text);
            //body = body.Replace("##designation##", txtDesignation.Text);
            body = body.Replace("##ProjectName##", txtProjectName.Text.Trim());
            body = body.Replace("##Budget##", txtBudget.Text.Trim());

            char[] separator = new char[] { ';' };
            objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
            objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
            objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
            objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##RRFNO##", txtRRFNo.Text);
            objEmailActivityBOL.Body = body; // (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
            objEmailActivityBOL.RRFNo = txtRRFNo.Text;
            objEmailActivityBOL.skills = txtKeySkills.Text;
            objEmailActivityBOL.Position = txtDesignation.Text;
            try
            {
                objEmailActivity.SendMail(objEmailActivityBOL);
            }
            catch (System.Exception ex)
            {
                lblSuccess.Text = "RRF assigned Successfully ,but e-mails could not be sent.";
            }
        }
        else if (TimesReassigned < 6)
        {
            objHRMBLL.SetRecruiterToRRF(objHRMBOL);

            lblSuccess.Visible = true;
            lblSuccess.Text = "This RRF has been successfully re-assigned.";
            pnlHRM.Visible = false;
            btnBack.Visible = false;
            btnRedirect.Visible = true;
            btnConfirmAndAssign.Visible = false;
            btnCancel.Visible = false;
            //Mailing Activity

            objEmailActivityBOL.ToID = Convert.ToString(lstRecruiterName.SelectedItem.Value) + ";";//recruiter

            objEmailActivityBOL.CCID = Convert.ToString(HttpContext.Current.User.Identity.Name) + ";" + Session["OldRecruiterID"].ToString() + ";";//hrm

            objEmailActivityBOL.FromID = Convert.ToInt32(HttpContext.Current.User.Identity.Name);//hrm
            objEmailActivityBOL.EmailTemplateName = "Reassign Recruiter";
            dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);
            string body;

            body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            //body = body.Replace("##RRFNO##", txtRRFNo.Text);
            //body = body.Replace("##skills##", txtKeySkills.Text);
            //body = body.Replace("##designation##", txtDesignation.Text);
            body = body.Replace("##ProjectName##", txtProjectName.Text.Trim());
            body = body.Replace("##Budget##", txtBudget.Text.Trim());

            char[] separator = new char[] { ';' };
            objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
            objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
            objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
            objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##RRFNO##", txtRRFNo.Text);
            objEmailActivityBOL.Body = body;//(dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
            objEmailActivityBOL.RRFNo = txtRRFNo.Text;
            objEmailActivityBOL.skills = txtKeySkills.Text;
            objEmailActivityBOL.Position = txtDesignation.Text;
            try
            {
                objEmailActivity.SendMail(objEmailActivityBOL);
            }
            catch (System.Exception ex)
            {
                lblSuccess.Text = "RRF re-assigned Successfully ,but e-mails could not be sent.";
            }
        }
        else
        {
            lblError.Visible = true;
            lblError.Text = "This RRF cannot be re-assigned as it has already been re-assigned 5 times.";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (RRFStatus == 1 || RRFStatus == 2)  //RRFStatus == YetToBegin OR InProgress
        {
            objHRMBOL.RRFID = Convert.ToInt32(RRFID);
            int checkOfferIssedToAnyCandidate = objHRMBLL.CheckOfferIssedToAnyCandidate(objHRMBOL);

            //first check whether offer is issued to any candidate
            if (checkOfferIssedToAnyCandidate != 0)
            {
                lblError.Visible = true;
                lblError.Text = "Offer is issued to Candidate against this RRF, So can not Cancel this RRF.";
            }
            else
            {
                Session["RRFID"] = Convert.ToString(RRFID);
                //String value1 = "Cancel";
                //String value2 =
                //String toid = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["ApprovedBy"]);//approver
                String fromid = Convert.ToString(HttpContext.Current.User.Identity.Name);
                String ccid = String.Empty;
                if (lstRecruiterName.SelectedIndex == -1)
                    ccid = fromid;
                else
                    ccid = fromid + ";" + Convert.ToString(lstRecruiterName.SelectedItem.Value);

                Session["value1"] = "Cancel";
                Session["value2"] = "Reason for Cancellation";
                Session["toid"] = Convert.ToString(dsRRFDetails.Tables[0].Rows[0]["ApprovedBy"]);//approver
                Session["fromid"] = fromid;
                Session["ccid"] = ccid;

                Response.Redirect("RRFApproverComment.aspx");

                //  Response.Redirect(string.Format("RRFApproverComment.aspx?value1={0}&value2={1}&toid={2}&fromid={3}&ccid={4}", HttpUtility.UrlEncode(value1), HttpUtility.UrlEncode(value2), HttpUtility.UrlEncode(toid), HttpUtility.UrlEncode(fromid), HttpUtility.UrlEncode(ccid)));
            }
        }
        else
        {
            lblError.Visible = true;
            lblError.Text = "This action is not allowed";
        }
    }

    protected void btnRedirect_Click(object sender, EventArgs e)
    {
        //  string value1 = "HRM RRFList";
        Session["Title"] = "HRM RRFList";
        //Response.Redirect(string.Format("RRFList.aspx?Title={0}", HttpUtility.UrlEncode(value1)));
        Response.Redirect("RRFList.aspx");
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (Session["HeaderCheck"].ToString() == "Interview")
        {
            Response.Redirect("~/Recruitment/Interviewer.aspx");
        }
        else if (Session["HeaderCheck"].ToString() == "HRM RRFList" || (Session["HeaderCheck"].ToString() == "RRF Approver List"))
        {
            Response.Redirect("~/Recruitment/RRFList.aspx");
        }
        else if (Session["HeaderCheck"].ToString() == "Recruiter")
        {
            Response.Redirect("~/Recruitment/Recruiter.aspx");
        }
        else if (Session["HeaderCheck"].ToString() == "RRF List")
        {
            Response.Redirect("~/Recruitment/RRFList.aspx");
        }
    }
}