using BLL;
using BOL;
using MailActivity;
using System;
using System.Data;
using System.Web;

public partial class RRFApproverComment : System.Web.UI.Page
{
    private DataSet dsGetMailInfo = new DataSet();

    private RRFApproverBLL objRRFApproverBLL = new RRFApproverBLL();
    private RRFListBLL objRRFListBLL = new RRFListBLL();
    private RRFApproverBOL objRRFApproverBOL = new RRFApproverBOL();

    private EmailActivityBLL objEmailActivityBLL = new EmailActivityBLL();
    private EmailActivityBOL objEmailActivityBOL = new EmailActivityBOL();
    private EmailActivity objEmailActivity = new EmailActivity();

    private static String value1 = "";
    private static String value2 = "";
    private static String toid = "";
    private static String fromid = "";
    private static String ccid = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    public void BindData()
    {
        //value1 = Request.QueryString["value1"];
        //value2 = Request.QueryString["value2"];

        value1 = Convert.ToString(Session["value1"]);
        value2 = Convert.ToString(Session["value2"]);

        lblTitle.Text = value1 + " RRF";
        lblReasonFor.Text = value2;
    }

    protected void btnSendToAddComment_Click(object sender, EventArgs e)
    {
        int RRFID = Convert.ToInt32(Session["RRFID"]);
        objRRFApproverBOL.RRFID = RRFID;
        objRRFApproverBOL.ModifiedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
        objRRFApproverBOL.ModifiedDate = DateTime.Now;
        DataSet dsmaildetails = new DataSet();
        dsmaildetails = objRRFApproverBLL.getmaildetails(RRFID);

        if (lblTitle.Text == "Reject RRF")
        {
            //toid = Request.QueryString["toid"];
            //fromid = Request.QueryString["fromid"];
            //ccid = Request.QueryString["ccid"];

            toid = Convert.ToString(Session["toid"]);
            fromid = Convert.ToString(Session["fromid"]);
            ccid = Convert.ToString(Session["ccid"]);

            objRRFApproverBOL.RRFStatus = 0;
            objRRFApproverBOL.ApprovalStatus = 4;
            lblSuccess.Text = "RRF Rejected Successfully";
            objRRFApproverBOL.Comments = txtReasonFor.Text;
            objRRFApproverBLL.UdateRRFValuesToApprove(objRRFApproverBOL);

            //Mailing Activity

            objEmailActivityBOL.ToID = Convert.ToString(toid) + ";";//requestor

            objEmailActivityBOL.CCID = Convert.ToString(ccid) + ";";//approver

            objEmailActivityBOL.FromID = Convert.ToInt32(fromid);//approver
            objEmailActivityBOL.EmailTemplateName = "Reject RRF";
            dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);
            string body;

            body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            //body = body.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
            //body = body.Replace("##skills##", (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString()));
            //body = body.Replace("##designation##", (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString()));
            body = body.Replace("##ProjectName##", (dsmaildetails.Tables[0].Rows[0]["ProjectName"].ToString()));
            body = body.Replace("##comment##", txtReasonFor.Text);

            char[] separator = new char[] { ';' };
            objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
            objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
            objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
            objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
            objEmailActivityBOL.Body = body;//(dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
            objEmailActivityBOL.RRFNo = (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString());
            objEmailActivityBOL.skills = (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString());
            objEmailActivityBOL.Position = (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString());
            try
            {
                objEmailActivity.SendMail(objEmailActivityBOL);
            }
            catch (System.Exception ex)
            {
                lblSuccess.Text = "RRF rejected successfully,but e-mails could not be sent.";
            }
        }
        else if (lblTitle.Text == "Push Back RRF")
        {
            //toid = Request.QueryString["toid"];
            //fromid = Request.QueryString["fromid"];
            //ccid = Request.QueryString["ccid"];

            toid = Convert.ToString(Session["toid"]);
            fromid = Convert.ToString(Session["fromid"]);
            ccid = Convert.ToString(Session["ccid"]);

            objRRFApproverBOL.RRFStatus = 0;
            objRRFApproverBOL.ApprovalStatus = 3;
            lblSuccess.Text = "RRF sent for Clarification";
            objRRFApproverBOL.Comments = txtReasonFor.Text;
            objRRFApproverBLL.UdateRRFValuesToApprove(objRRFApproverBOL);

            //Mailing Activity
            string role;
            role = Session["Role"].ToString();

            if (role == "Approver")
            {
                objEmailActivityBOL.ToID = Convert.ToString(toid) + ";";//requestor

                objEmailActivityBOL.CCID = Convert.ToString(ccid) + ";";//approver

                objEmailActivityBOL.FromID = Convert.ToInt32(fromid);//approver
                //objEmailActivityBOL.EmailTemplateName = "Resend For Approval Approver";
                objEmailActivityBOL.EmailTemplateName = "Pushback RRF";
            }
            else
            {
                objEmailActivityBOL.ToID = Convert.ToString(toid) + ";";//requestor
                objEmailActivityBOL.CCID = Convert.ToString(ccid) + ";";//approver
                objEmailActivityBOL.FromID = Convert.ToInt32(fromid);//approver
                objEmailActivityBOL.EmailTemplateName = "Resend For Approval";
            }
            dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);

            string body;

            body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            //body = body.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
            //body = body.Replace("##skills##", (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString()));
            //body = body.Replace("##designation##", (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString()));
            body = body.Replace("##ProjectName##", (dsmaildetails.Tables[0].Rows[0]["ProjectName"].ToString()));
            body = body.Replace("##comment##", txtReasonFor.Text);

            char[] separator = new char[] { ';' };
            objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
            objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
            objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
            objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
            objEmailActivityBOL.Body = body; // (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
            objEmailActivityBOL.RRFNo = (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString());
            objEmailActivityBOL.skills = (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString());
            objEmailActivityBOL.Position = (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString());
            try
            {
                objEmailActivity.SendMail(objEmailActivityBOL);
            }
            catch (System.Exception ex)
            {
                lblSuccess.Text = "RRF sent for Clarification ,but e-mails could not be sent.";
            }
        }
        else if (lblTitle.Text == "Cancel RRF")
        {
            //toid = Request.QueryString["toid"];
            //fromid = Request.QueryString["fromid"];
            //ccid = Request.QueryString["ccid"];

            toid = Convert.ToString(Session["toid"]);
            fromid = Convert.ToString(Session["fromid"]);
            ccid = Convert.ToString(Session["ccid"]);

            objRRFApproverBOL.RRFStatus = 4;
            objRRFApproverBOL.ApprovalStatus = 0;
            objRRFListBLL.CancelRRF(objRRFApproverBOL);
            lblSuccess.Text = "RRF Cancelled Successfully";
            objRRFApproverBOL.Comments = txtReasonFor.Text;
            objRRFApproverBLL.UdateRRFValuesToApprove(objRRFApproverBOL);
            objEmailActivityBOL.ToID = Convert.ToString(toid) + ";";//hrm

            objEmailActivityBOL.CCID = Convert.ToString(ccid) + ";";//hrm,recruiter

            objEmailActivityBOL.FromID = Convert.ToInt32(fromid);//approver
            objEmailActivityBOL.EmailTemplateName = "Cancel RRF";
            dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);

            string body;

            body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            //body = body.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
            //body = body.Replace("##skills##", (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString()));
            //body = body.Replace("##designation##", (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString()));
            body = body.Replace("##ProjectName##", (dsmaildetails.Tables[0].Rows[0]["ProjectName"].ToString()));
            body = body.Replace("##comment##", txtReasonFor.Text);

            char[] separator = new char[] { ';' };
            objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
            objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
            objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
            objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
            objEmailActivityBOL.Body = body; // (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
            objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
            objEmailActivityBOL.RRFNo = (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString());
            objEmailActivityBOL.skills = (dsmaildetails.Tables[0].Rows[0]["keyskills"].ToString());
            objEmailActivityBOL.Position = (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString());
            try
            {
                objEmailActivity.SendMail(objEmailActivityBOL);
            }
            catch (System.Exception ex)
            {
                lblSuccess.Text = "RRF Cancelled Successfully ,but e-mails could not be sent.";
            }
        }

        lblSuccess.Visible = true;
        btnRedirect.Visible = true;
        btnBack.Visible = false;
        pnlRRFComments.Visible = false;
    }

    protected void btnRedirect_Click(object sender, EventArgs e)
    {
        if (value1 == "Reject")
            value1 = "RRF Approver List";
        else if (value1 == "Cancel")
            value1 = "HRM RRFList";
        else if (value1 == "Push Back")
            value1 = "RRF Approver List";

        Session["Title"] = value1;

        Response.Redirect("RRFList.aspx");
        //Response.Redirect(string.Format("RRFList.aspx?Title={0}", HttpUtility.UrlEncode(value1)));
    }
}