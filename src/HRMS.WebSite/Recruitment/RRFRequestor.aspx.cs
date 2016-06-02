using BLL;
using BOL;
using MailActivity;
using System;
using System.Collections.Generic;
using System.Data;

//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RRFRequestor : System.Web.UI.Page
{
    private DataSet dsGetDeliveryUnit = new DataSet();
    private DataSet dsGetDesignation = new DataSet();
    private DataSet dsGetResourcePool = new DataSet();
    private DataSet dsGetEmploymentType = new DataSet();
    private DataSet dsGetDeliveryTeam = new DataSet();
    private DataSet dsGetApprover = new DataSet();
    private DataSet dsGetMailInfo = new DataSet();
    private DataSet dsSLAForTechnology = new DataSet();

    private RRFRequestorBLL objRRFRequestorBLL = new RRFRequestorBLL();
    private RRFRequestorBOL objRRFRequestorBOL = new RRFRequestorBOL();
    private EmailActivityBLL objEmailActivityBLL = new EmailActivityBLL();
    private EmailActivityBOL objEmailActivityBOL = new EmailActivityBOL();
    private EmailActivity objEmailActivity = new EmailActivity();

    private static DataSet dsIndicativePanel1 = new DataSet();
    private static RRFRequestorBLL objRRFRequestorBLLForIndicativePanel1 = new RRFRequestorBLL();

    private static DataSet dsIndicativePanel2 = new DataSet();
    private static RRFRequestorBLL objRRFRequestorBLLForIndicativePanel2 = new RRFRequestorBLL();

    private static DataSet dsIndicativePanel3 = new DataSet();
    private static RRFRequestorBLL objRRFRequestorBLLForIndicativePanel3 = new RRFRequestorBLL();

    private static DataSet dsReplacementFor = new DataSet();
    private static RRFRequestorBLL objRRFRequestorBLLForReplacement = new RRFRequestorBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindData();
            txtExpectedClosureDate.Focus();
        }
    }

    public void BindData()
    {
        DateTime currentDate = DateTime.Now;
        txtRequestDate.Text = currentDate.ToString("MM/dd/yyyy");

        dsGetApprover = objRRFRequestorBLL.GetApprover();
        ddlApproverName.Items.Clear();
        ddlApproverName.Items.Add("Select");
        for (int i = 0; i < dsGetApprover.Tables[0].Rows.Count; i++)
        {
            ddlApproverName.Items.Add(new ListItem(dsGetApprover.Tables[0].Rows[i]["EmployeeName"].ToString(), dsGetApprover.Tables[0].Rows[i]["UserID"].ToString()));
        }

        dsGetDeliveryUnit = objRRFRequestorBLL.GetDeliveryUnit();
        ddlForDU.Items.Clear();
        ddlForDU.Items.Add("Select");
        for (int i = 0; i < dsGetDeliveryUnit.Tables[0].Rows.Count; i++)
        {
            ddlForDU.Items.Add(new ListItem(dsGetDeliveryUnit.Tables[0].Rows[i]["ResourcePoolName"].ToString(), dsGetDeliveryUnit.Tables[0].Rows[i]["ResourcePoolID"].ToString()));
        }

        dsGetDesignation = objRRFRequestorBLL.GetDesignation();
        ddlDesignation.Items.Clear();
        ddlDesignation.Items.Add("Select");
        for (int i = 0; i < dsGetDesignation.Tables[0].Rows.Count; i++)
        {
            ddlDesignation.Items.Add(new ListItem(dsGetDesignation.Tables[0].Rows[i]["DesignationName"].ToString(), dsGetDesignation.Tables[0].Rows[i]["DesignationID"].ToString()));
        }

        dsGetResourcePool = objRRFRequestorBLL.GetResourcePool();
        ddlResourcePool.Items.Clear();
        ddlResourcePool.Items.Add("");
        for (int i = 0; i < dsGetResourcePool.Tables[0].Rows.Count; i++)
        {
            ddlResourcePool.Items.Add(new ListItem(dsGetResourcePool.Tables[0].Rows[i]["ResourcePoolName"].ToString(), dsGetResourcePool.Tables[0].Rows[i]["ResourcePoolID"].ToString()));
        }

        dsGetEmploymentType = objRRFRequestorBLL.GetEmploymentType();
        ddlEmploymentType.Items.Clear();
        ddlEmploymentType.Items.Add("");
        for (int i = 0; i < dsGetEmploymentType.Tables[0].Rows.Count; i++)
        {
            ddlEmploymentType.Items.Add(new ListItem(dsGetEmploymentType.Tables[0].Rows[i]["EmploymentType"].ToString(), dsGetEmploymentType.Tables[0].Rows[i]["ID"].ToString()));
        }

        dsSLAForTechnology = objRRFRequestorBLL.GetSkillsForSLA();
        ddlSLAForTechnology.Items.Clear();
        ddlSLAForTechnology.Items.Add("Select");
        for (int i = 0; i < dsSLAForTechnology.Tables[0].Rows.Count; i++)
        {
            ddlSLAForTechnology.Items.Add(new ListItem(dsSLAForTechnology.Tables[0].Rows[i]["Skill"].ToString(), dsSLAForTechnology.Tables[0].Rows[i]["ID"].ToString()));
        }
    }

    protected void rdobtnIsReplacement_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdobtnIsReplacement.SelectedItem.Text == "Yes")
        {
            lblReplacementFor.Visible = true;
            txtReplacementFor.Visible = true;
            txtReplacementFor.Focus();
            lblReplacementMandatory.Visible = true;
            txtReplacementFor.Text = "";
        }
        else if (rdobtnIsReplacement.SelectedItem.Text == "No")
        {
            lblReplacementFor.Visible = false;
            txtReplacementFor.Visible = false;
            lblReplacementMandatory.Visible = false;
        }
    }

    //[System.Web.Script.Services.ScriptService]

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetEmployeeName1(string prefixText)
    {
        dsIndicativePanel1 = objRRFRequestorBLLForIndicativePanel1.GetEmployeeName(prefixText);
        List<string> IndicativePanelNames = new List<string>();

        for (int i = 0; i < dsIndicativePanel1.Tables[0].Rows.Count; i++)
        {
            IndicativePanelNames.Add(dsIndicativePanel1.Tables[0].Rows[i]["EmployeeName"].ToString());
        }
        return IndicativePanelNames;
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetEmployeeName2(string prefixText)
    {
        dsIndicativePanel2 = objRRFRequestorBLLForIndicativePanel2.GetEmployeeName(prefixText);
        List<string> IndicativePanelNames = new List<string>();

        for (int i = 0; i < dsIndicativePanel2.Tables[0].Rows.Count; i++)
        {
            IndicativePanelNames.Add(dsIndicativePanel2.Tables[0].Rows[i]["EmployeeName"].ToString());
        }
        return IndicativePanelNames;
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetEmployeeName3(string prefixText)
    {
        dsIndicativePanel3 = objRRFRequestorBLLForIndicativePanel3.GetEmployeeName(prefixText);
        List<string> IndicativePanelNames = new List<string>();

        for (int i = 0; i < dsIndicativePanel3.Tables[0].Rows.Count; i++)
        {
            IndicativePanelNames.Add(dsIndicativePanel3.Tables[0].Rows[i]["EmployeeName"].ToString());
        }
        return IndicativePanelNames;
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetEmployeeName4(string prefixText)
    {
        dsReplacementFor = objRRFRequestorBLLForReplacement.GetEmployeeName(prefixText);
        List<string> ReplacementForNames = new List<string>();

        for (int i = 0; i < dsReplacementFor.Tables[0].Rows.Count; i++)
        {
            ReplacementForNames.Add(dsReplacementFor.Tables[0].Rows[i]["EmployeeName"].ToString());
        }
        return ReplacementForNames;
    }

    protected void btnSendForApproval_Click(object sender, EventArgs e)
    {
        lblIndicativePanel1.Visible = false;
        lblIndicativePanel2.Visible = false;
        lblIndicativePanel3.Visible = false;
        lblReplacement.Visible = false;

        // get login Id of current user.
        objRRFRequestorBOL.RequestedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
        objRRFRequestorBOL.RequestDate = DateTime.Now;
        if (txtExpectedClosureDate.Text == "")
            objRRFRequestorBOL.ExpectedClosureDate = Convert.ToDateTime("1900-01-01 00:00:00.000");
        else
        {
            string expireOnDate = txtExpectedClosureDate.Text;
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
            objRRFRequestorBOL.ExpectedClosureDate = expectedClosureDate;
        }

        objRRFRequestorBOL.RRFForDU = Convert.ToInt32(ddlForDU.SelectedItem.Value);
        objRRFRequestorBOL.DUName = Convert.ToString(ddlForDU.SelectedItem.Text);

        if (ddlForDT.SelectedItem.Value == "")
            objRRFRequestorBOL.RRFForDT = 0;
        else
            objRRFRequestorBOL.RRFForDT = Convert.ToInt32(ddlForDT.SelectedItem.Value);

        if (txtProjectName.Text == "")
            objRRFRequestorBOL.ProjectName = "";
        else
            objRRFRequestorBOL.ProjectName = Convert.ToString(txtProjectName.Text);

        objRRFRequestorBOL.Designation = Convert.ToInt32(ddlDesignation.SelectedItem.Value);

        if (txtIndicativePanel1.Text == "")
            objRRFRequestorBOL.IndicativePanel1 = 0;
        else
        {
            int isIndicativePanel1Empty = 1;
            int employeeID1 = 0;
            if (dsIndicativePanel1 != null)
            {
                if (dsIndicativePanel1.Tables.Count > 0 && dsIndicativePanel1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsIndicativePanel1.Tables[0].Rows.Count; i++)
                    {
                        if (txtIndicativePanel1.Text == dsIndicativePanel1.Tables[0].Rows[i]["EmployeeName"].ToString())
                        {
                            employeeID1 = Convert.ToInt32(dsIndicativePanel1.Tables[0].Rows[i]["UserID"]);
                            isIndicativePanel1Empty = 1;
                            break;
                        }
                        else
                        {
                            isIndicativePanel1Empty = 0;
                        }
                    }
                }
                else
                {
                    lblIndicativePanel1.Visible = true;
                    return;
                }
            }

            if (isIndicativePanel1Empty == 1)
                objRRFRequestorBOL.IndicativePanel1 = employeeID1;
            else
            {
                lblIndicativePanel1.Visible = true;
                return;
            }
        }

        if (txtIndicativePanel2.Text == "")
            objRRFRequestorBOL.IndicativePanel2 = 0;
        else
        {
            int isIndicativePanel2Empty = 1;
            int employeeID2 = 0;
            if (dsIndicativePanel2 != null)
            {
                if (dsIndicativePanel2.Tables.Count > 0 && dsIndicativePanel2.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsIndicativePanel2.Tables[0].Rows.Count; i++)
                    {
                        if (txtIndicativePanel2.Text == dsIndicativePanel2.Tables[0].Rows[i]["EmployeeName"].ToString())
                        {
                            employeeID2 = Convert.ToInt32(dsIndicativePanel2.Tables[0].Rows[i]["UserID"]);
                            isIndicativePanel2Empty = 1;
                            break;
                        }
                        else
                        {
                            isIndicativePanel2Empty = 0;
                        }
                    }
                }
                else
                {
                    lblIndicativePanel2.Visible = true;
                    return;
                }
            }

            if (isIndicativePanel2Empty == 1)
                objRRFRequestorBOL.IndicativePanel2 = employeeID2;
            else
            {
                lblIndicativePanel2.Visible = true;
                return;
            }
        }

        if (txtIndicativePanel3.Text == "")
            objRRFRequestorBOL.IndicativePanel3 = 0;
        else
        {
            int isIndicativePanel3Empty = 1;
            int employeeID3 = 0;
            if (dsIndicativePanel3 != null)
            {
                if (dsIndicativePanel3.Tables.Count > 0 && dsIndicativePanel3.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsIndicativePanel3.Tables[0].Rows.Count; i++)
                    {
                        if (txtIndicativePanel3.Text == dsIndicativePanel3.Tables[0].Rows[i]["EmployeeName"].ToString())
                        {
                            employeeID3 = Convert.ToInt32(dsIndicativePanel3.Tables[0].Rows[i]["UserID"]);
                            isIndicativePanel3Empty = 1;
                            break;
                        }
                        else
                        {
                            isIndicativePanel3Empty = 0;
                        }
                    }
                }
                else
                {
                    lblIndicativePanel3.Visible = true;
                    return;
                }
            }

            if (isIndicativePanel3Empty == 1)
                objRRFRequestorBOL.IndicativePanel3 = employeeID3;
            else
            {
                lblIndicativePanel3.Visible = true;
                return;
            }
        }

        if (ddlResourcePool.SelectedItem.Value == "")
            objRRFRequestorBOL.ResourcePool = 0;
        else
            objRRFRequestorBOL.ResourcePool = Convert.ToInt32(ddlResourcePool.SelectedItem.Value);

        objRRFRequestorBOL.PositionsRequired = Convert.ToInt32(txtPositionsRequired.Text);

        if (ddlEmploymentType.SelectedItem.Value == "")
            objRRFRequestorBOL.EmployeementType = 0;
        else
            objRRFRequestorBOL.EmployeementType = Convert.ToInt32(ddlEmploymentType.SelectedItem.Value);

        if (rdobtnIsReplacement.SelectedItem.Text == "Yes")
        {
            objRRFRequestorBOL.IsReplacement = true;
            int isReplacementForEmpty = 1;
            int employeeID4 = 0;

            if (dsReplacementFor != null)
            {
                if (dsReplacementFor.Tables.Count > 0 && dsReplacementFor.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReplacementFor.Tables[0].Rows.Count; i++)
                    {
                        if (txtReplacementFor.Text == dsReplacementFor.Tables[0].Rows[i]["EmployeeName"].ToString())
                        {
                            employeeID4 = Convert.ToInt32(dsReplacementFor.Tables[0].Rows[i]["UserID"]);
                            isReplacementForEmpty = 1;
                            break;
                        }
                        else
                        {
                            isReplacementForEmpty = 0;
                        }
                    }
                }
                else
                {
                    lblReplacement.Visible = true;
                    return;
                }
            }

            if (isReplacementForEmpty == 1)
                objRRFRequestorBOL.ReplacementFor = employeeID4;
            else
            {
                lblReplacement.Visible = true;
                return;
            }
        }
        else if (rdobtnIsReplacement.SelectedItem.Text == "No")
        {
            objRRFRequestorBOL.IsReplacement = false;
            objRRFRequestorBOL.ReplacementFor = 0;
        }

        if (rdobtnIsBillable.SelectedItem.Text == "Yes")
        {
            objRRFRequestorBOL.IsBillable = true;
        }
        else if (rdobtnIsBillable.SelectedItem.Text == "No")
        {
            objRRFRequestorBOL.IsBillable = false;
        }

        objRRFRequestorBOL.KeySkills = Convert.ToString(txtKeySkills.Text);

        objRRFRequestorBOL.Experience = Convert.ToInt32(txtExperience.Text);

        objRRFRequestorBOL.BusinessJustification = Convert.ToString(txtBuisnessJustification.Text);

        if (txtAdditionalInformation.Text == "")
            objRRFRequestorBOL.AdditionalInfo = "";
        else
            objRRFRequestorBOL.AdditionalInfo = Convert.ToString(txtAdditionalInformation.Text);
        objRRFRequestorBOL.ApprovedBy = Convert.ToInt32(ddlApproverName.SelectedItem.Value);
        objRRFRequestorBOL.SLAForSkill = Convert.ToInt32(ddlSLAForTechnology.SelectedItem.Value);
        DataSet dsmaildetails = new DataSet();
        dsmaildetails = objRRFRequestorBLL.AddRRF(objRRFRequestorBOL);

        //Mailing Activity
        string body;

        objEmailActivityBOL.ToID = Convert.ToString(ddlApproverName.SelectedItem.Value) + ";";
        objEmailActivityBOL.CCID = Convert.ToString(HttpContext.Current.User.Identity.Name) + ";";
        objEmailActivityBOL.FromID = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

        objEmailActivityBOL.EmailTemplateName = "Send For Approval";
        dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);

        MasterPage objMaster = new MasterPage();

        body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
        //get name of Requestor
        body = body.Replace("##EmployeeName##", (dsGetMailInfo.Tables[1].Rows[0]["firstname"].ToString()));
        //body = body.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
        //body = body.Replace("##skills##", txtKeySkills.Text.Trim());
        //body = body.Replace("##designation##", (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString()));
        body = body.Replace("##ProjectName##", txtProjectName.Text.Trim());

        char[] separator = new char[] { ';' };
        objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
        objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
        objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
        objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##RRFNO##", (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString()));
        objEmailActivityBOL.Body = body;
        objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
        objEmailActivityBOL.RRFNo = (dsmaildetails.Tables[0].Rows[0]["RRFNo"].ToString());
        objEmailActivityBOL.skills = txtKeySkills.Text.Trim();
        objEmailActivityBOL.Position = (dsmaildetails.Tables[0].Rows[0]["DesignationName"].ToString());

        try
        {
            objEmailActivity.SendMail(objEmailActivityBOL);
            lblSuccessMessage.Text = "RRF sent for Approval Successfully";
            // lblSuccessMessage.Visible = true;
        }
        catch (System.Exception ex)
        {
            lblSuccessMessage.Text = "RRF sent for Approval Successfully ,but e-mails could not be sent.";
        }

        btnBack.Enabled = true;
        btnCancel.Enabled = true;
        btnRedirect.Enabled = true;
        btnSendForApproval.Enabled = true;

        lblSuccessMessage.Visible = true;
        pnlRRFRequestor.Visible = false;
        btnBack.Visible = false;
        btnRedirect.Visible = true;
        btnSendForApproval.Visible = false;
        btnCancel.Visible = false;

        //System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "javascript:MessageTextResult();", true);
    }

    protected void ddlForDU_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlForDT.Items.Clear();
        ddlForDT.Items.Add("");
        if (ddlForDU.Items.FindByText("Select") != null)
            ddlForDU.Items.RemoveAt(0);
        int deliveryUnit = Convert.ToInt32(ddlForDU.SelectedItem.Value);
        dsGetDeliveryTeam = objRRFRequestorBLL.GetDeliveryTeam(deliveryUnit);
        // ddlForDT.Items.Add("");
        //  ddlForDT.Items.Add("Select");
        for (int i = 0; i < dsGetDeliveryTeam.Tables[0].Rows.Count; i++)
        {
            ddlForDT.Items.Add(new ListItem(dsGetDeliveryTeam.Tables[0].Rows[i]["GroupName"].ToString(), dsGetDeliveryTeam.Tables[0].Rows[i]["GroupID"].ToString()));
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtExpectedClosureDate.Text = "";
        txtAdditionalInformation.Text = "";
        txtBuisnessJustification.Text = "";
        txtExperience.Text = "";
        txtIndicativePanel1.Text = "";
        txtIndicativePanel2.Text = "";
        txtIndicativePanel3.Text = "";
        txtKeySkills.Text = "";
        txtPositionsRequired.Text = "";
        txtProjectName.Text = "";
        txtReplacementFor.Text = "";
        ddlDesignation.SelectedIndex = 0;
        ddlEmploymentType.SelectedIndex = 0;
        ddlForDU.SelectedIndex = 0;
        ddlForDT.Items.Clear();
        ddlResourcePool.SelectedIndex = 0;
        ddlSLAForTechnology.SelectedIndex = 0;
        ddlApproverName.SelectedIndex = 0;
        btnBack.Visible = false;
        btnRedirect.Visible = true;
    }

    protected void btnRedirect_Click(object sender, EventArgs e)
    {
        //string value1 = "RRF List";
        Session["Title"] = "RRF List";
        //Response.Redirect(string.Format("RRFList.aspx?Title={0}", HttpUtility.UrlEncode(value1)));
        Response.Redirect("RRFList.aspx");
    }

    //protected void ddlSLAForTechnology_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    lblTotalSLADays.Visible = true;
    //    int SkillForSLA;
    //    DataSet dsTotalSLADaysForTech;
    //    if (ddlSLAForTechnology.SelectedValue != "Select" || ddlSLAForTechnology.SelectedValue == null)
    //    {
    //        lblSLADays.Visible = true;
    //        SkillForSLA = Convert.ToInt32(ddlSLAForTechnology.SelectedValue);
    //        dsTotalSLADaysForTech = objRRFRequestorBLL.GetSLADaysForSelectTechnology(SkillForSLA);
    //        lblSLADays.Text = dsTotalSLADaysForTech.Tables[0].Rows[0]["Days"].ToString();

    //    }
    //    else
    //    {
    //        lblSLADays.Visible = true;
    //        lblSLADays.Text = string.Empty;
    //        lblTotalSLADays.Visible = false;
    //    }
    //}
}