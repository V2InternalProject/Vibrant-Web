using BLL;
using BOL;
using MailActivity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

public partial class RRFApprover : System.Web.UI.Page
{
    private DataSet dsGetDeliveryUnit = new DataSet();
    private DataSet dsGetDesignation = new DataSet();
    private DataSet dsGetResourcePool = new DataSet();
    private DataSet dsGetEmploymentType = new DataSet();
    private DataSet dsGetDeliveryTeam = new DataSet();
    private DataSet dsGetRRFValuesToApprove = new DataSet();
    private DataSet dsGetMailInfo = new DataSet();
    private DataSet dsGetEmployeeFromRole = new DataSet();
    private DataSet dsSLAForTechnology = new DataSet();

    private RRFApproverBLL objRRFApproverBLL = new RRFApproverBLL();
    private RRFApproverBOL objRRFApproverBOL = new RRFApproverBOL();
    private EmailActivityBLL objEmailActivityBLL = new EmailActivityBLL();
    private EmailActivityBOL objEmailActivityBOL = new EmailActivityBOL();
    private EmailActivity objEmailActivity = new EmailActivity();

    private static String value1 = "";
    private static String value2 = "";
    private static string expectedClosureDate = string.Empty;

    private static DataSet dsIndicativePanel1 = new DataSet();
    private static RRFApproverBLL objRRFApproverBLLForIndicativePanel1 = new RRFApproverBLL();

    private static DataSet dsIndicativePanel2 = new DataSet();
    private static RRFApproverBLL objRRFApproverBLLForIndicativePanel2 = new RRFApproverBLL();

    private static DataSet dsIndicativePanel3 = new DataSet();
    private static RRFApproverBLL objRRFApproverBLLForIndicativePanel3 = new RRFApproverBLL();

    private static DataSet dsReplacementFor = new DataSet();
    private static RRFApproverBLL objRRFApproverBLLForReplacement = new RRFApproverBLL();

    private static DateTime requestDate = new DateTime();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
            txtExpectedClosureDate.Focus();
        }
    }

    public void BindData()
    {
        //objRRFApproverBOL.RRFID = 56;
        // objRRFApproverBOL.RRFID = Convert.ToInt32(Request.QueryString["RRFID"].ToString());
        objRRFApproverBOL.RRFID = Convert.ToInt32(Session["RRFID"].ToString());
        Session["RRFID"] = objRRFApproverBOL.RRFID;

        dsGetRRFValuesToApprove = objRRFApproverBLL.GetRRFValuesToApprove(objRRFApproverBOL);

        txtRequestor.Text = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["EmpName"]);

        Session["Requestor"] = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["RequestedByID"]);

        Session["Approver"] = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["ApprovedBy"]);

        txtRRFNo.Text = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["RRFNo"]);

        DateTime reqdate = new DateTime();
        requestDate = Convert.ToDateTime(dsGetRRFValuesToApprove.Tables[0].Rows[0]["RequestDate"]);
        reqdate = Convert.ToDateTime(dsGetRRFValuesToApprove.Tables[0].Rows[0]["RequestDate"]);
        txtRequestDate.Text = reqdate.ToString("MM/dd/yyyy");

        if (Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["ExpectedClosureDate"]) != "")
        {
            DateTime expclosuredate = new DateTime();
            expclosuredate = Convert.ToDateTime(dsGetRRFValuesToApprove.Tables[0].Rows[0]["ExpectedClosureDate"]);
            txtExpectedClosureDate.Text = expclosuredate.ToString("MM/dd/yyyy");
        }

        if (Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["ProjectName"]) != null)
            txtProjectName.Text = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["ProjectName"]);

        txtPositionsRequired.Text = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["PositionsRequired"]);

        if (Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind1"]) != null)
            txtIndicativePanel1.Text = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind1"]);

        if (Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind2"]) != null)
            txtIndicativePanel2.Text = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind2"]);

        if (Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind3"]) != null)
            txtIndicativePanel3.Text = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind3"]);

        txtKeySkills.Text = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["KeySkills"]);

        txtExperience.Text = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Experience"]);

        txtBuisnessJustification.Text = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["BusinessJustification"]);

        if (Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["AdditionalInfo"]) != null)
            txtAdditionalInformation.Text = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["AdditionalInfo"]);

        bool checkRplacement = Convert.ToBoolean(dsGetRRFValuesToApprove.Tables[0].Rows[0]["IsReplacement"]);
        if (checkRplacement == true)
        {
            rdobtnIsReplacement.SelectedValue = "Yes";
            lblReplacementFor.Visible = true;
            txtReplacementFor.Visible = true;
            txtReplacementFor.Text = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["ReplacementFor"]);
        }
        else
            rdobtnIsReplacement.SelectedValue = "No";

        bool checkBillable = Convert.ToBoolean(dsGetRRFValuesToApprove.Tables[0].Rows[0]["IsBillable"]);
        if (checkBillable == true)
            rdobtnIsBillable.SelectedValue = "Yes";
        else
            rdobtnIsBillable.SelectedValue = "No";

        dsGetDeliveryUnit = objRRFApproverBLL.GetDeliveryUnit(objRRFApproverBOL);
        int deliveryUnit;
        ddlForDU.Items.Add("Select");
        for (int i = 0; i < dsGetDeliveryUnit.Tables[0].Rows.Count; i++)
        {
            ddlForDU.Items.Add(new ListItem(dsGetDeliveryUnit.Tables[0].Rows[i]["ResourcePoolName"].ToString(), dsGetDeliveryUnit.Tables[0].Rows[i]["ResourcePoolID"].ToString()));
        }
        ddlForDU.Items.FindByText(Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["DU"])).Selected = true;

        deliveryUnit = Convert.ToInt32(ddlForDU.SelectedItem.Value);
        dsGetDeliveryTeam = objRRFApproverBLL.GetDeliveryTeam(deliveryUnit);
        ddlForDT.Items.Add("");
        for (int i = 0; i < dsGetDeliveryTeam.Tables[0].Rows.Count; i++)
        {
            ddlForDT.Items.Add(new ListItem(dsGetDeliveryTeam.Tables[0].Rows[i]["GroupName"].ToString(), dsGetDeliveryTeam.Tables[0].Rows[i]["GroupID"].ToString()));
        }
        if (Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["DT"]) != null)
        {
            if (!string.IsNullOrEmpty(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Designation"].ToString()))
            {
                ddlForDT.Items.FindByText(Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["DT"])).Selected = true;
            }
        }

        dsGetDesignation = objRRFApproverBLL.GetDesignation(objRRFApproverBOL);
        ddlDesignation.Items.Add("Select");
        for (int i = 0; i < dsGetDesignation.Tables[0].Rows.Count; i++)
        {
            ddlDesignation.Items.Add(new ListItem(dsGetDesignation.Tables[0].Rows[i]["DesignationName"].ToString(), dsGetDesignation.Tables[0].Rows[i]["DesignationID"].ToString()));
        }
        //ddlDesignation.Items.FindByText(Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Designation"])).Selected = true;
        if (!string.IsNullOrEmpty(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Designation"].ToString()))
        {
            ddlDesignation.Items.FindByText(Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Designation"])).Selected = true;
        }

        dsGetResourcePool = objRRFApproverBLL.GetResourcePool(objRRFApproverBOL);
        ddlResourcePool.Items.Add("");
        String resPoolName;
        for (int i = 0; i < dsGetResourcePool.Tables[0].Rows.Count; i++)
        {
            resPoolName = dsGetResourcePool.Tables[0].Rows[i]["ResourcePoolName"].ToString();
            ddlResourcePool.Items.Add(new ListItem(dsGetResourcePool.Tables[0].Rows[i]["ResourcePoolName"].ToString(), dsGetResourcePool.Tables[0].Rows[i]["ResourcePoolID"].ToString()));
        }
        if (Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["ResourcePool"]) != null)
            ddlResourcePool.Items.FindByText(Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["ResourcePool"])).Selected = true;

        dsGetEmploymentType = objRRFApproverBLL.GetEmploymentType(objRRFApproverBOL);
        ddlEmploymentType.Items.Add("");
        for (int i = 0; i < dsGetEmploymentType.Tables[0].Rows.Count; i++)
        {
            ddlEmploymentType.Items.Add(new ListItem(dsGetEmploymentType.Tables[0].Rows[i]["EmploymentType"].ToString(), dsGetEmploymentType.Tables[0].Rows[i]["ID"].ToString()));
        }
        if (Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["EType"]) != null)
            ddlEmploymentType.Items.FindByText(Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["EType"])).Selected = true;

        if (Convert.ToInt32(dsGetRRFValuesToApprove.Tables[0].Rows[0]["ApprovalStatus"]) == 3)
        {
            lblTitle.Text = "RRF Requestor Screen [Push Back]";
            btnApproveandSendtoHR.Visible = false;
            btnPushbackRRF.Visible = false;
            btnRejectRRF.Visible = false;
            txtBudgetPerVacancy.Visible = false;
            lblBudgetPerVacancy.Visible = false;
            Label7.Visible = false;
            lblComments.Visible = true;
            txtComments.Visible = true;
            txtComments.Enabled = false;
            txtComments.Text = dsGetRRFValuesToApprove.Tables[0].Rows[0]["Comments"].ToString();
            btnResendForApproval.Visible = true;
        }

        dsSLAForTechnology = objRRFApproverBLL.GetSkillsForSLA(objRRFApproverBOL);
        ddlSLAForTechnology.Items.Add("Select");
        for (int i = 0; i < dsSLAForTechnology.Tables[0].Rows.Count; i++)
        {
            ddlSLAForTechnology.Items.Add(new ListItem(dsSLAForTechnology.Tables[0].Rows[i]["Skill"].ToString(), dsSLAForTechnology.Tables[0].Rows[i]["ID"].ToString()));
        }
        if (!string.IsNullOrEmpty(dsGetRRFValuesToApprove.Tables[0].Rows[0]["SLAForSkill"].ToString()))
            ddlSLAForTechnology.Items.FindByText(Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["SLAForSkill"])).Selected = true;

        //lblTotalSLADays.Visible = true;
        //lblSLADays.Text = Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Days"]);
    }

    protected void rdobtnIsReplacement_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdobtnIsReplacement.SelectedItem.Text == "Yes")
        {
            lblReplacementFor.Visible = true;
            txtReplacementFor.Visible = true;
            lblReplacementMandatory.Visible = true;
            txtReplacementFor.Text = "";
            txtReplacementFor.Focus();
        }
        else if (rdobtnIsReplacement.SelectedItem.Text == "No")
        {
            lblReplacementFor.Visible = false;
            txtReplacementFor.Visible = false;
            lblReplacementMandatory.Visible = false;
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetEmployeeName1(string prefixText)
    {
        dsIndicativePanel1 = objRRFApproverBLLForIndicativePanel1.GetEmployeeName(prefixText);
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
        dsIndicativePanel2 = objRRFApproverBLLForIndicativePanel2.GetEmployeeName(prefixText);
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
        dsIndicativePanel3 = objRRFApproverBLLForIndicativePanel3.GetEmployeeName(prefixText);
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
        dsReplacementFor = objRRFApproverBLLForReplacement.GetEmployeeName(prefixText);
        List<string> ReplacementForNames = new List<string>();

        for (int i = 0; i < dsReplacementFor.Tables[0].Rows.Count; i++)
        {
            ReplacementForNames.Add(dsReplacementFor.Tables[0].Rows[i]["EmployeeName"].ToString());
        }
        return ReplacementForNames;
    }

    protected void btnApproveandSendtoHR_Click(object sender, EventArgs e)
    {
        // for mail activity.. variables
        string designationmail;
        //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOWNew", "javascript:HideImageloader();", true);
        // for mail activity.. variables

        lblIndicativePanel1.Visible = false;
        lblIndicativePanel2.Visible = false;
        lblIndicativePanel3.Visible = false;
        lblReplacement.Visible = false;

        //objRRFApproverBOL.RRFID = Convert.ToInt32(Request.QueryString["RRFID"].ToString());
        objRRFApproverBOL.RRFID = Convert.ToInt32(Session["RRFID"].ToString());
        dsGetRRFValuesToApprove = objRRFApproverBLL.GetRRFValuesToApprove(objRRFApproverBOL);

        objRRFApproverBOL.RequestedBy = Convert.ToInt32(dsGetRRFValuesToApprove.Tables[0].Rows[0]["RequestedByID"]);
        objRRFApproverBOL.RRFNo = txtRRFNo.Text;
        objRRFApproverBOL.RequestDate = requestDate;

        if (txtExpectedClosureDate.Text == "")
            objRRFApproverBOL.ExpectedClosureDate = Convert.ToDateTime("1900-01-01 00:00:00.000");
        else
        {
            string expireOnDate = txtExpectedClosureDate.Text;
            int pos1 = 0;
            int pos2 = 0;
            if (expireOnDate.Contains("/"))
            {
                pos1 = expireOnDate.IndexOf("/");
                pos2 = expireOnDate.IndexOf("/", pos1 + 1);
            }
            else if (expireOnDate.Contains("-"))
            {
                pos1 = expireOnDate.IndexOf("-");
                pos2 = expireOnDate.IndexOf("-", pos1 + 1);
            }

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
            objRRFApproverBOL.ExpectedClosureDate = expectedClosureDate;
        }

        objRRFApproverBOL.RRFForDU = Convert.ToInt32(ddlForDU.SelectedItem.Value);

        if (ddlForDT.SelectedItem.Value == "")
            objRRFApproverBOL.RRFForDT = 0;
        else
            objRRFApproverBOL.RRFForDT = Convert.ToInt32(ddlForDT.SelectedItem.Value);

        if (txtProjectName.Text == "")
            objRRFApproverBOL.ProjectName = "";
        else
            objRRFApproverBOL.ProjectName = Convert.ToString(txtProjectName.Text);

        objRRFApproverBOL.Designation = Convert.ToInt32(ddlDesignation.SelectedItem.Value);
        designationmail = ddlDesignation.SelectedItem.ToString();

        if (txtIndicativePanel1.Text == "")
            objRRFApproverBOL.IndicativePanel1 = 0;
        else
        {
            if (txtIndicativePanel1.Text == Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind1"]))
                objRRFApproverBOL.IndicativePanel1 = Convert.ToInt32(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind1ID"]);
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
                    objRRFApproverBOL.IndicativePanel1 = employeeID1;
                else
                {
                    lblIndicativePanel1.Visible = true;
                    return;
                }
            }
        }

        if (txtIndicativePanel2.Text == "")
            objRRFApproverBOL.IndicativePanel2 = 0;
        else
        {
            if (txtIndicativePanel2.Text == Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind2"]))
                objRRFApproverBOL.IndicativePanel2 = Convert.ToInt32(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind2ID"]);
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
                    objRRFApproverBOL.IndicativePanel2 = employeeID2;
                else
                {
                    lblIndicativePanel2.Visible = true;
                    return;
                }
            }
        }

        if (txtIndicativePanel3.Text == "")
            objRRFApproverBOL.IndicativePanel3 = 0;
        else
        {
            if (txtIndicativePanel3.Text == Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind3"]))
                objRRFApproverBOL.IndicativePanel3 = Convert.ToInt32(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind3ID"]);
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
                    objRRFApproverBOL.IndicativePanel3 = employeeID3;
                else
                {
                    lblIndicativePanel3.Visible = true;
                    return;
                }
            }
        }

        if (ddlResourcePool.SelectedItem.Value == "")
            objRRFApproverBOL.ResourcePool = 0;
        else
            objRRFApproverBOL.ResourcePool = Convert.ToInt32(ddlResourcePool.SelectedItem.Value);

        objRRFApproverBOL.PositionsRequired = Convert.ToInt32(txtPositionsRequired.Text);

        if (ddlEmploymentType.SelectedItem.Value == "")
            objRRFApproverBOL.EmployeementType = 0;
        else
            objRRFApproverBOL.EmployeementType = Convert.ToInt32(ddlEmploymentType.SelectedItem.Value);

        if (rdobtnIsReplacement.SelectedItem.Text == "Yes")
        {
            if (txtReplacementFor.Text == Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["ReplacementFor"]))
            {
                objRRFApproverBOL.ReplacementFor = Convert.ToInt32(dsGetRRFValuesToApprove.Tables[0].Rows[0]["ReplacementForID"]);
                objRRFApproverBOL.IsReplacement = true;
            }
            else
            {
                objRRFApproverBOL.IsReplacement = true;
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
                    objRRFApproverBOL.ReplacementFor = employeeID4;
                else
                {
                    lblReplacement.Visible = true;
                    return;
                }
            }
        }
        else if (rdobtnIsReplacement.SelectedItem.Text == "No")
        {
            objRRFApproverBOL.IsReplacement = false;
            objRRFApproverBOL.ReplacementFor = 0;
        }

        if (rdobtnIsBillable.SelectedItem.Text == "Yes")
        {
            objRRFApproverBOL.IsBillable = true;
        }
        else if (rdobtnIsBillable.SelectedItem.Text == "No")
        {
            objRRFApproverBOL.IsBillable = false;
        }

        objRRFApproverBOL.KeySkills = Convert.ToString(txtKeySkills.Text);

        objRRFApproverBOL.Experience = Convert.ToInt32(txtExperience.Text);

        objRRFApproverBOL.BusinessJustification = Convert.ToString(txtBuisnessJustification.Text);

        if (txtAdditionalInformation.Text == "")
            objRRFApproverBOL.AdditionalInfo = "";
        else
            objRRFApproverBOL.AdditionalInfo = Convert.ToString(txtAdditionalInformation.Text);

        objRRFApproverBOL.BudgetPerVacancy = Convert.ToDecimal(txtBudgetPerVacancy.Text);

        DateTime currentDate = DateTime.Now;
        objRRFApproverBOL.ApproveDate = currentDate;
        objRRFApproverBOL.ApprovedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

        objRRFApproverBOL.SLAForSkill = Convert.ToInt32(ddlSLAForTechnology.SelectedItem.Value);

        objRRFApproverBLL.AddRRF(objRRFApproverBOL);

        //Mailing Activity

        objRRFApproverBOL.Role = "HRM";
        dsGetEmployeeFromRole = objRRFApproverBLL.GetEmployeeFromRole(objRRFApproverBOL);
        string toID = dsGetEmployeeFromRole.Tables[0].Rows[0]["UserId"].ToString();
        for (int i = 1; i < dsGetEmployeeFromRole.Tables[0].Rows.Count; i++)
            toID = toID + ';' + dsGetEmployeeFromRole.Tables[0].Rows[i]["UserId"].ToString();
        objEmailActivityBOL.ToID = Convert.ToString(toID) + ";";//hrm

        objEmailActivityBOL.CCID = Convert.ToString(Session["Requestor"]) + ";" + Convert.ToString(HttpContext.Current.User.Identity.Name) + ";";//requestor,approver

        objEmailActivityBOL.FromID = Convert.ToInt32(HttpContext.Current.User.Identity.Name);//approver
        objEmailActivityBOL.EmailTemplateName = "Approve RRF";
        dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);

        string body;

        body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
        //body = body.Replace("##RRFNO##", objRRFApproverBOL.RRFNo);
        //body = body.Replace("##skills##", objRRFApproverBOL.KeySkills);
        //body = body.Replace("##designation##", designationmail);
        body = body.Replace("##ProjectName##", txtProjectName.Text);
        body = body.Replace("##comment##", txtAdditionalInformation.Text);

        char[] separator = new char[] { ';' };
        objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
        objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
        objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
        objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##RRFNO##", objRRFApproverBOL.RRFNo);
        objEmailActivityBOL.Body = body;//(dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
        objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
        objEmailActivityBOL.RRFNo = objRRFApproverBOL.RRFNo;
        objEmailActivityBOL.skills = objRRFApproverBOL.KeySkills;
        objEmailActivityBOL.Position = designationmail;
        try
        {
            objEmailActivity.SendMail(objEmailActivityBOL);
        }
        catch (System.Exception ex)
        {
            lblSuccessMessage.Text = "RRF successfully sent,but e-mails could not be sent.";
        }

        btnApproveandSendtoHR.Enabled = true;
        btnBack.Enabled = true;
        btnPushbackRRF.Enabled = true;
        btnRedirect.Enabled = true;
        btnRejectRRF.Enabled = true;
        btnResendForApproval.Enabled = true;

        lblSuccessMessage.Visible = true;
        pnlRRFApprover.Visible = false;
        btnBack.Visible = false;
        lblTitle.Visible = false;
        btnRedirect.Visible = true;
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
        //BindData();

        //ddlDesignation.SelectedIndex = 0;
        //ddlEmploymentType.SelectedIndex = 0;
        //ddlForDU.SelectedIndex = 0;
        //ddlResourcePool.SelectedIndex = 0;
    }

    protected void ddlForDU_SelectedIndexChanged(object sender, EventArgs e)
    {
        expectedClosureDate = txtExpectedClosureDate.Text;
        ddlForDT.Items.Clear();
        ddlForDT.Items.Add("");
        if (ddlForDU.Items.FindByText("Select") != null)
            ddlForDU.Items.RemoveAt(0);
        int deliveryUnit = Convert.ToInt32(ddlForDU.SelectedItem.Value);
        dsGetDeliveryTeam = objRRFApproverBLL.GetDeliveryTeam(deliveryUnit);
        for (int i = 0; i < dsGetDeliveryTeam.Tables[0].Rows.Count; i++)
        {
            ddlForDT.Items.Add(new ListItem(dsGetDeliveryTeam.Tables[0].Rows[i]["GroupName"].ToString(), dsGetDeliveryTeam.Tables[0].Rows[i]["GroupID"].ToString()));
        }
        ddlForDT.Focus();
    }

    protected void btnRejectRRF_Click(object sender, EventArgs e)
    {
        //String requestor = Convert.ToString(Session["Requestor"]);
        //String approver = Convert.ToString(HttpContext.Current.User.Identity.Name);
        //String value1 = "Reject";
        //String value2 = "Reason for Rejection";

        Session["value1"] = "Reject";
        Session["value2"] = "Reason for Rejection";
        Session["toid"] = Convert.ToString(Session["Requestor"]);
        Session["fromid"] = Convert.ToString(HttpContext.Current.User.Identity.Name);
        Session["ccid"] = Convert.ToString(HttpContext.Current.User.Identity.Name);

        //Response.Redirect(string.Format("RRFApproverComment.aspx?value1={0}&value2={1}&toid={2}&fromid={3}&ccid={4}", HttpUtility.UrlEncode(value1), HttpUtility.UrlEncode(value2), HttpUtility.UrlEncode(requestor), HttpUtility.UrlEncode(approver), HttpUtility.UrlEncode(approver)));
        Response.Redirect("RRFApproverComment.aspx");
    }

    protected void btnPushbackRRF_Click(object sender, EventArgs e)
    {
        //String requestor = Convert.ToString(Session["Requestor"]);
        //String approver = Convert.ToString(HttpContext.Current.User.Identity.Name);
        //String value1 = "Push Back";
        //String value2 = "Reason for Push Back";
        Session["value1"] = "Push Back";
        Session["value2"] = "Reason for Push Back";
        Session["toid"] = Convert.ToString(Session["Requestor"]);
        Session["fromid"] = Convert.ToString(HttpContext.Current.User.Identity.Name);
        Session["ccid"] = Convert.ToString(HttpContext.Current.User.Identity.Name);

        Response.Redirect("RRFApproverComment.aspx");
    }

    protected void btnResendForApproval_Click(object sender, EventArgs e)
    {
        lblIndicativePanel1.Visible = false;
        lblIndicativePanel2.Visible = false;
        lblIndicativePanel3.Visible = false;
        lblReplacement.Visible = false;

        // variables for mails
        string body, designationmail;

        // variables for mails

        //objRRFApproverBOL.RRFID = 56;
        //objRRFApproverBOL.RRFID = Convert.ToInt32(Request.QueryString["RRFID"].ToString());
        objRRFApproverBOL.RRFID = Convert.ToInt32(Session["RRFID"].ToString());
        dsGetRRFValuesToApprove = objRRFApproverBLL.GetRRFValuesToApprove(objRRFApproverBOL);

        objRRFApproverBOL.RequestedBy = Convert.ToInt32(dsGetRRFValuesToApprove.Tables[0].Rows[0]["RequestedByID"]);
        objRRFApproverBOL.RequestDate = DateTime.Now;

        objRRFApproverBOL.RRFNo = txtRRFNo.Text;

        if (txtExpectedClosureDate.Text == "")
            objRRFApproverBOL.ExpectedClosureDate = Convert.ToDateTime("1900-01-01 00:00:00.000");
        else
        {
            string expireOnDate = txtExpectedClosureDate.Text;
            int pos1 = 0;
            int pos2 = 0;
            if (expireOnDate.Contains("/"))
            {
                pos1 = expireOnDate.IndexOf("/");
                pos2 = expireOnDate.IndexOf("/", pos1 + 1);
            }
            else if (expireOnDate.Contains("-"))
            {
                pos1 = expireOnDate.IndexOf("-");
                pos2 = expireOnDate.IndexOf("-", pos1 + 1);
            }

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
            objRRFApproverBOL.ExpectedClosureDate = expectedClosureDate;
        }

        objRRFApproverBOL.RRFForDU = Convert.ToInt32(ddlForDU.SelectedItem.Value);

        if (ddlForDT.SelectedItem.Value == "")
            objRRFApproverBOL.RRFForDT = 0;
        else
            objRRFApproverBOL.RRFForDT = Convert.ToInt32(ddlForDT.SelectedItem.Value);

        if (txtProjectName.Text == "")
            objRRFApproverBOL.ProjectName = "";
        else
            objRRFApproverBOL.ProjectName = Convert.ToString(txtProjectName.Text);

        objRRFApproverBOL.Designation = Convert.ToInt32(ddlDesignation.SelectedItem.Value);

        designationmail = ddlDesignation.SelectedItem.ToString();

        if (txtIndicativePanel1.Text == "")
            objRRFApproverBOL.IndicativePanel1 = 0;
        else
        {
            if (txtIndicativePanel1.Text == Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind1"]))
                objRRFApproverBOL.IndicativePanel1 = Convert.ToInt32(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind1ID"]);
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
                    objRRFApproverBOL.IndicativePanel1 = employeeID1;
                else
                {
                    lblIndicativePanel1.Visible = true;
                    return;
                }
            }
        }

        if (txtIndicativePanel2.Text == "")
            objRRFApproverBOL.IndicativePanel2 = 0;
        else
        {
            if (txtIndicativePanel2.Text == Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind2"]))
                objRRFApproverBOL.IndicativePanel2 = Convert.ToInt32(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind2ID"]);
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
                    objRRFApproverBOL.IndicativePanel2 = employeeID2;
                else
                {
                    lblIndicativePanel2.Visible = true;
                    return;
                }
            }
        }

        if (txtIndicativePanel3.Text == "")
            objRRFApproverBOL.IndicativePanel3 = 0;
        else
        {
            if (txtIndicativePanel3.Text == Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind3"]))
                objRRFApproverBOL.IndicativePanel3 = Convert.ToInt32(dsGetRRFValuesToApprove.Tables[0].Rows[0]["Ind3ID"]);
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
                    objRRFApproverBOL.IndicativePanel3 = employeeID3;
                else
                {
                    lblIndicativePanel3.Visible = true;
                    return;
                }
            }
        }

        if (ddlResourcePool.SelectedItem.Value == "")
            objRRFApproverBOL.ResourcePool = 0;
        else
            objRRFApproverBOL.ResourcePool = Convert.ToInt32(ddlResourcePool.SelectedItem.Value);

        objRRFApproverBOL.PositionsRequired = Convert.ToInt32(txtPositionsRequired.Text);

        if (ddlEmploymentType.SelectedItem.Value == "")
            objRRFApproverBOL.EmployeementType = 0;
        else
            objRRFApproverBOL.EmployeementType = Convert.ToInt32(ddlEmploymentType.SelectedItem.Value);

        if (rdobtnIsReplacement.SelectedItem.Text == "Yes")
        {
            if (txtReplacementFor.Text == Convert.ToString(dsGetRRFValuesToApprove.Tables[0].Rows[0]["ReplacementFor"]))
            {
                objRRFApproverBOL.ReplacementFor = Convert.ToInt32(dsGetRRFValuesToApprove.Tables[0].Rows[0]["ReplacementForID"]);
                objRRFApproverBOL.IsReplacement = true;
            }
            else
            {
                objRRFApproverBOL.IsReplacement = true;
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
                    objRRFApproverBOL.ReplacementFor = employeeID4;
                else
                {
                    lblReplacement.Visible = true;
                    return;
                }
            }
        }
        else if (rdobtnIsReplacement.SelectedItem.Text == "No")
        {
            objRRFApproverBOL.IsReplacement = false;
            objRRFApproverBOL.ReplacementFor = 0;
        }

        if (rdobtnIsBillable.SelectedItem.Text == "Yes")
        {
            objRRFApproverBOL.IsBillable = true;
        }
        else if (rdobtnIsBillable.SelectedItem.Text == "No")
        {
            objRRFApproverBOL.IsBillable = false;
        }

        objRRFApproverBOL.KeySkills = Convert.ToString(txtKeySkills.Text);

        objRRFApproverBOL.Experience = Convert.ToInt32(txtExperience.Text);

        objRRFApproverBOL.BusinessJustification = Convert.ToString(txtBuisnessJustification.Text);

        if (txtAdditionalInformation.Text == "")
            objRRFApproverBOL.AdditionalInfo = "";
        else
            objRRFApproverBOL.AdditionalInfo = Convert.ToString(txtAdditionalInformation.Text);
        objRRFApproverBOL.Comments = txtComments.Text;

        objRRFApproverBLL.UpdateRRFForResend(objRRFApproverBOL);

        //Mailing Activity

        objEmailActivityBOL.ToID = Convert.ToString(Session["Approver"]) + ";";//approver

        objEmailActivityBOL.CCID = Convert.ToString(HttpContext.Current.User.Identity.Name) + ";";//approver

        objEmailActivityBOL.FromID = Convert.ToInt32(Session["Requestor"]);//requestor
        objEmailActivityBOL.EmailTemplateName = "Resend For Approval";
        dsGetMailInfo = objEmailActivityBLL.GetMailInfo(objEmailActivityBOL);

        body = (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
        //body = body.Replace("##RRFNO##", objRRFApproverBOL.RRFNo);
        //body = body.Replace("##skills##", objRRFApproverBOL.KeySkills);
        //body = body.Replace("##designation##", designationmail );
        body = body.Replace("##ProjectName##", txtProjectName.Text.Trim());

        char[] separator = new char[] { ';' };
        objEmailActivityBOL.ToAddress = (dsGetMailInfo.Tables[0].Rows[0]["ToAddress"].ToString()).Split(separator);
        objEmailActivityBOL.FromAddress = (dsGetMailInfo.Tables[0].Rows[0]["FromAddress"].ToString());
        objEmailActivityBOL.Subject = (dsGetMailInfo.Tables[0].Rows[0]["EmailSubject"].ToString());
        objEmailActivityBOL.Subject = objEmailActivityBOL.Subject.Replace("##RRFNO##", objRRFApproverBOL.RRFNo);
        objEmailActivityBOL.Body = body;// (dsGetMailInfo.Tables[0].Rows[0]["EmailBody"].ToString());
        objEmailActivityBOL.CCAddress = (dsGetMailInfo.Tables[0].Rows[0]["CCAddress"].ToString()).Split(separator);
        objEmailActivityBOL.RRFNo = objRRFApproverBOL.RRFNo;
        objEmailActivityBOL.skills = objRRFApproverBOL.KeySkills;
        objEmailActivityBOL.Position = designationmail;
        try
        {
            objEmailActivity.SendMail(objEmailActivityBOL);
        }
        catch (System.Exception ex)
        {
            lblSuccessMessage.Text = "RRF resent successfully, but e-mails could not be sent.";
        }

        btnApproveandSendtoHR.Enabled = true;
        btnBack.Enabled = true;
        btnPushbackRRF.Enabled = true;
        btnRedirect.Enabled = true;
        btnRejectRRF.Enabled = true;
        btnResendForApproval.Enabled = true;

        lblSuccessMessage.Text = "RRF Resent Successfully";
        value1 = lblSuccessMessage.Text;
        lblSuccessMessage.Visible = true;

        //txtExpectedClosureDate.Text = "";
        //txtAdditionalInformation.Text = "";
        //txtBuisnessJustification.Text = "";
        //txtExperience.Text = "";
        //txtIndicativePanel1.Text = "";
        //txtIndicativePanel2.Text = "";
        //txtIndicativePanel3.Text = "";
        //txtKeySkills.Text = "";
        //txtPositionsRequired.Text = "";
        //txtProjectName.Text = "";
        //txtReplacementFor.Text = "";
        //BindData();
        pnlRRFApprover.Visible = false;
        btnBack.Visible = false;
        btnRedirect.Visible = true;
    }

    protected void btnRedirect_Click(object sender, EventArgs e)
    {
        if (value1 == "RRF Resent Successfully")
            value1 = "RRF List";
        else
            value1 = "RRF Approver List";
        Session["Title"] = value1;

        //Response.Redirect(string.Format("RRFList.aspx?Title={0}", HttpUtility.UrlEncode(value1)));
        Response.Redirect("RRFList.aspx");
    }

    //protected void ddlSLAForTechnology_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    int SkillForSLA;
    //    DataSet dsTotalSLADaysForTech;
    //    if (ddlSLAForTechnology.SelectedValue != "0" || ddlSLAForTechnology.SelectedValue == null)
    //    {
    //        lblTotalSLADays.Visible = true;
    //        lblSLADays.Visible = true;
    //        SkillForSLA = Convert.ToInt32(ddlSLAForTechnology.SelectedValue);
    //        dsTotalSLADaysForTech = objRRFApproverBLL.GetSLADaysForSelectTechnology(SkillForSLA);
    //        lblSLADays.Text = dsTotalSLADaysForTech.Tables[0].Rows[0]["Days"].ToString();

    //    }
    //    else
    //    {
    //        lblTotalSLADays.Visible = true;
    //        lblSLADays.Visible = false;
    //    }
    //}
}