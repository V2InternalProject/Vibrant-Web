using BLL;
using BOL;
using DocumentFormat.OpenXml.Packaging;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using V2.Helpdesk.BusinessLayer;

public partial class Candidate : System.Web.UI.Page
{
    # region declarations

    private FileUpload fileUpload = new FileUpload();
    private ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private DataSet dsExperience = new DataSet();
    private DataSet maxcandidatejoined = new DataSet();
    private DataSet dsEducation = new DataSet();
    private DataSet dsCertification = new DataSet();
    private DataSet dsCertificationName = new DataSet();
    private DataSet dsCourses = new DataSet();
    private DataSet dsCountry = new DataSet();
    private DataSet dsDegree = new DataSet();
    private DataSet dsCourseTypes = new DataSet();
    private DataSet dsExpTypes = new DataSet();
    private DataSet dsSkills = new DataSet();
    private DataSet dsCandidateStatus = new DataSet();

    //DataSet dsCoursesByPGUG = new DataSet();
    private DataSet dsCandidate = new DataSet();

    private DataSet dsEstablishment = new DataSet();
    private DataSet dsOrganization = new DataSet();
    private DataSet dsCandidateHistory = new DataSet();
    private DataSet addhistrory = new DataSet();
    private static DataSet dsTemp = new DataSet();
    private CandidateBOL candidateBOL = new CandidateBOL();
    private CandidateBLL candidateBLL = new CandidateBLL();

    private DataTable dtTempExperience = new DataTable();
    private DataTable dtTempCertification = new DataTable();
    private DataTable dtTempEducation = new DataTable();

    private string candidateID = string.Empty;
    private string fileName = string.Empty;
    private string completeFilePath = string.Empty;
    private string modeForSkills = string.Empty;
    //  static string CurrentDate = string.Empty;

    private List<CandidateBOL> listOfActiveExperienceRecords = new List<CandidateBOL>();
    private List<CandidateBOL> listOfInsertedExperienceRecords = new List<CandidateBOL>();
    private List<CandidateBOL> listOfUpdatedExperienceRecords = new List<CandidateBOL>();
    private List<CandidateBOL> listOfDeletedExperienceRecords = new List<CandidateBOL>();

    private List<CandidateBOL> listOfActiveCertificationRecords = new List<CandidateBOL>();
    private List<CandidateBOL> listOfInsertedCertificationRecords = new List<CandidateBOL>();
    private List<CandidateBOL> listOfUpdatedCertificationRecords = new List<CandidateBOL>();
    private List<CandidateBOL> listOfDeletedCertificationRecords = new List<CandidateBOL>();

    private List<CandidateBOL> listOfActiveEducationRecords = new List<CandidateBOL>();
    private List<CandidateBOL> listOfInsertedEducationRecords = new List<CandidateBOL>();
    private List<CandidateBOL> listOfUpdatedEducationRecords = new List<CandidateBOL>();
    private List<CandidateBOL> listOfDeletedEducationRecords = new List<CandidateBOL>();

    protected System.Web.UI.HtmlControls.HtmlInputFile uploadFiles;

    #endregion

    #region properties

    #region Experience properties

    public int ExpIndex
    {
        get
        {
            if (ViewState["ExpIndex"] != null)
            {
                return Convert.ToInt32(ViewState["ExpIndex"]);
            }
            else
                return 0;
        }
        set { ViewState["ExpIndex"] = value; }
    }

    private List<CandidateBOL> ListOfActiveExperienceRecords
    {
        get
        {
            if (ViewState["ListOfActiveExperienceRecords"] != null)
                return ViewState["ListOfActiveExperienceRecords"] as List<CandidateBOL>;
            else
                return null;
        }
        set
        {
            ViewState["ListOfActiveExperienceRecords"] = value;
        }
    }

    private List<CandidateBOL> ListOfInsertedExperienceRecords
    {
        get
        {
            if (ViewState["ListOfInsertedExperienceRecords"] != null)
                return ViewState["ListOfInsertedExperienceRecords"] as List<CandidateBOL>;
            else
                return null;
        }
        set
        {
            ViewState["ListOfInsertedExperienceRecords"] = value;
        }
    }

    private List<CandidateBOL> ListOfUpdatedExperienceRecords
    {
        get
        {
            if (ViewState["ListOfUpdatedExperienceRecords"] != null)
                return ViewState["ListOfUpdatedExperienceRecords"] as List<CandidateBOL>;
            else
                return null;
        }
        set
        {
            ViewState["ListOfUpdatedExperienceRecords"] = value;
        }
    }

    private List<CandidateBOL> ListOfDeletedExperienceRecords
    {
        get
        {
            if (ViewState["ListOfDeletedExperienceRecords"] != null)
                return ViewState["ListOfDeletedExperienceRecords"] as List<CandidateBOL>;
            else
                return null;
        }
        set
        {
            ViewState["ListOfDeletedExperienceRecords"] = value;
        }
    }

    #endregion

    #region Education

    public int EducationIndex
    {
        get
        {
            if (ViewState["EducationIndex"] != null)
            {
                return Convert.ToInt32(ViewState["EducationIndex"]);
            }
            else
                return 0;
        }
        set { ViewState["EducationIndex"] = value; }
    }

    private List<CandidateBOL> ListOfActiveEducationRecords
    {
        get
        {
            if (ViewState["ListOfActiveEducationRecords"] != null)
                return ViewState["ListOfActiveEducationRecords"] as List<CandidateBOL>;
            else
                return null;
        }
        set
        {
            ViewState["ListOfActiveEducationRecords"] = value;
        }
    }

    private List<CandidateBOL> ListOfInsertedEducationRecords
    {
        get
        {
            if (ViewState["ListOfInsertedEducationRecords"] != null)
                return ViewState["ListOfInsertedEducationRecords"] as List<CandidateBOL>;
            else
                return null;
        }
        set
        {
            ViewState["ListOfInsertedEducationRecords"] = value;
        }
    }

    private List<CandidateBOL> ListOfUpdatedEducationRecords
    {
        get
        {
            if (ViewState["ListOfUpdatedEducationRecords"] != null)
                return ViewState["ListOfUpdatedEducationRecords"] as List<CandidateBOL>;
            else
                return null;
        }
        set
        {
            ViewState["ListOfUpdatedEducationRecords"] = value;
        }
    }

    private List<CandidateBOL> ListOfDeletedEducationRecords
    {
        get
        {
            if (ViewState["ListOfDeletedEducationRecords"] != null)
                return ViewState["ListOfDeletedEducationRecords"] as List<CandidateBOL>;
            else
                return null;
        }
        set
        {
            ViewState["ListOfDeletedEducationRecords"] = value;
        }
    }

    #endregion

    #region Certification

    public int CertificationIndex
    {
        get
        {
            if (ViewState["CertificationIndex"] != null)
            {
                return Convert.ToInt32(ViewState["CertificationIndex"]);
            }
            else
                return 0;
        }
        set { ViewState["CertificationIndex"] = value; }
    }

    private List<CandidateBOL> ListOfActiveCertificationRecords
    {
        get
        {
            if (ViewState["ListOfActiveCertificationRecords"] != null)
                return ViewState["ListOfActiveCertificationRecords"] as List<CandidateBOL>;
            else
                return null;
        }
        set
        {
            ViewState["ListOfActiveCertificationRecords"] = value;
        }
    }

    private List<CandidateBOL> ListOfInsertedCertificationRecords
    {
        get
        {
            if (ViewState["ListOfInsertedCertificationRecords"] != null)
                return ViewState["ListOfInsertedCertificationRecords"] as List<CandidateBOL>;
            else
                return null;
        }
        set
        {
            ViewState["ListOfInsertedCertificationRecords"] = value;
        }
    }

    private List<CandidateBOL> ListOfUpdatedCertificationRecords
    {
        get
        {
            if (ViewState["ListOfUpdatedCertificationRecords"] != null)
                return ViewState["ListOfUpdatedCertificationRecords"] as List<CandidateBOL>;
            else
                return null;
        }
        set
        {
            ViewState["ListOfUpdatedCertificationRecords"] = value;
        }
    }

    private List<CandidateBOL> ListOfDeletedCertificationRecords
    {
        get
        {
            if (ViewState["ListOfDeletedCertificationRecords"] != null)
                return ViewState["ListOfDeletedCertificationRecords"] as List<CandidateBOL>;
            else
                return null;
        }
        set
        {
            ViewState["ListOfDeletedCertificationRecords"] = value;
        }
    }

    #endregion

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["RecruiterRole"] != null)
        {
            txtCurrentCTC.Visible = false;
            lblCurrentCTC.Visible = false;
            lblCurrentCTCPA.Visible = false;
            lblRupeeSymbolCurrentCTC.Visible = false;
        }
        else if (Session["RecruiterRole"] == null && Session["HRMRole"] == null)
        {
            fileUpload.Visible = false;
            lblUploadResume.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            pnlCandidate.Enabled = false;
        }

        ////For checking role of Current User
        //if (HttpContext.Current.User.IsInRole("Recruiter"))
        //{
        //    txtCurrentCTC.Visible = false;
        //    lblCurrentCTC.Visible = false;
        //    lblCurrentCTCPA.Visible = false;
        //    lblRupeeSymbolCurrentCTC.Visible = false;
        //}
        //if (!HttpContext.Current.User.IsInRole("Recruiter") && !HttpContext.Current.User.IsInRole("HRM"))
        //{
        //    fileUpload.Visible = false;
        //    lblUploadResume.Visible = false;
        //    btnSave.Visible = false;
        //    btnCancel.Visible = false;
        //    pnlCandidate.Enabled = false;
        //}

        //
        candidateBOL.IsFileUploaded = false;
        string ID = Request.QueryString["ID"];
        string mode = Request.QueryString["mode"];
        if (Session["ID"] != null)
            ID = Convert.ToString(Session["ID"]);
        if (Session["mode"] != null)
            mode = Convert.ToString(Session["mode"]);

        Session["ID"] = null;

        btnSave.Attributes.Add("onClick", "return validateForm(" + txtFirstName.ClientID + " , " + txtLastName.ClientID + "," + txtDateOfBirth.ClientID + "," + ddlCandidateStatus.ClientID + "," + ddlSalutation.ClientID + "," + ddlMaritalStatus.ClientID + "," + ddlGender.ClientID + "," + txtAlternateContactNumber.ClientID + "," + txtMobileNumber.ClientID + "," + txtEmailID.ClientID + "," + ddlHighestQualification.ClientID + "," + txtPresentAddress.ClientID + "," + txtCity.ClientID + "," + txtPinCode.ClientID + "," + txtState.ClientID + "," + ddlCountry.ClientID + "," + ddlSource.ClientID + "," + txtSourceName.ClientID + "," + ddlTotalWorkExpYears.ClientID + "," + ddlTotalWorkExpMonths.ClientID + "," + ddlRelevantWorkExpYears.ClientID + "," + ddlRelevantWorkExpMonths.ClientID + ");");  // Attribute added for update using javascript validation
        btnSaveAndAddMore.Attributes.Add("onClick", "return validateForm(" + txtFirstName.ClientID + " , " + txtLastName.ClientID + "," + txtDateOfBirth.ClientID + "," + ddlCandidateStatus.ClientID + "," + ddlSalutation.ClientID + "," + ddlMaritalStatus.ClientID + "," + ddlGender.ClientID + "," + txtAlternateContactNumber.ClientID + "," + txtMobileNumber.ClientID + "," + txtEmailID.ClientID + "," + ddlHighestQualification.ClientID + "," + txtPresentAddress.ClientID + "," + txtCity.ClientID + "," + txtPinCode.ClientID + "," + txtState.ClientID + "," + ddlCountry.ClientID + "," + ddlSource.ClientID + "," + txtSourceName.ClientID + "," + ddlTotalWorkExpYears.ClientID + "," + ddlTotalWorkExpMonths.ClientID + "," + ddlRelevantWorkExpYears.ClientID + "," + ddlRelevantWorkExpMonths.ClientID + ");");  // Attribute added for update using javascript validation
        btnAlert.Style.Add("display", "none");
        btnAlertCancel.Style.Add("display", "none");
        btnRedirect.Style.Add("display", "none");

        btnSaveAndAddMoreRedirect.Style.Add("display", "none");

        if (!string.IsNullOrEmpty(ID))
        {
            lblCandidateID.Text = ID;
            candidateID = ID;
            btnSave.Text = "Update"; // for Edit Mode
            btnSaveAndAddMore.Visible = false;
            ddlCandidateStatus.Focus();

            GetCandidateDetailsInEditMode(ID);

            //To Remove Querystring

            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            isreadonly.SetValue(this.Request.QueryString, false, null); //make collection editable
            this.Request.QueryString.Remove("ID");  //remove

            if (ListOfActiveExperienceRecords == null)
                ListOfActiveExperienceRecords = GetExperienceRecordsInList(dsExperience);

            if (ListOfInsertedExperienceRecords != null)
                listOfInsertedExperienceRecords = ListOfInsertedExperienceRecords;

            if (ListOfUpdatedExperienceRecords != null)
                listOfUpdatedExperienceRecords = ListOfUpdatedExperienceRecords;

            if (ListOfDeletedExperienceRecords != null)
                listOfDeletedExperienceRecords = ListOfDeletedExperienceRecords;

            if (ListOfActiveCertificationRecords == null)
                ListOfActiveCertificationRecords = GetCertificationRecordsInList(dsCertification);

            if (ListOfInsertedCertificationRecords != null)
                listOfInsertedCertificationRecords = ListOfInsertedCertificationRecords;

            if (ListOfUpdatedCertificationRecords != null)
                listOfUpdatedCertificationRecords = ListOfUpdatedCertificationRecords;

            if (ListOfDeletedCertificationRecords != null)
                listOfDeletedCertificationRecords = ListOfDeletedCertificationRecords;

            if (ListOfActiveEducationRecords == null)
                ListOfActiveEducationRecords = GetEducationRecordsInList(dsEducation);

            if (ListOfInsertedEducationRecords != null)
                listOfInsertedEducationRecords = ListOfInsertedEducationRecords;

            if (ListOfUpdatedEducationRecords != null)
                listOfUpdatedEducationRecords = ListOfUpdatedEducationRecords;

            if (ListOfDeletedEducationRecords != null)
                listOfDeletedEducationRecords = ListOfDeletedEducationRecords;

            //     For getting latest Experience ID
            if (ExpIndex == 0)
            {
                string expIndex = candidateBLL.GetLatestExperienceID(candidateBOL);
                if (!string.IsNullOrEmpty(expIndex))
                    ExpIndex = Convert.ToInt32(expIndex);
            }

            //   For getting latest Certification ID
            if (CertificationIndex == 0)
            {
                string certificationIndex = candidateBLL.GetLatestCertificationID(candidateBOL);
                if (!string.IsNullOrEmpty(certificationIndex))
                    CertificationIndex = Convert.ToInt32(certificationIndex);
            }
            //

            //   For getting latest Education ID

            if (EducationIndex == 0)
            {
                string educationIndex = candidateBLL.GetLatestEducationID(candidateBOL);
                if (!string.IsNullOrEmpty(educationIndex))
                    EducationIndex = Convert.ToInt32(educationIndex);
            }

            if (ddlCandidateStatus.SelectedValue == "17")
            {
                fileUpload.Visible = false;
                lblUploadResume.Visible = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                pnlCandidate.Enabled = false;
            }
        }
        else
        {
            lblErrorMsg.Text = string.Empty;
            lblSuccessMsg.Text = string.Empty;
            lblMsg.Text = string.Empty;

            dsExperience = candidateBLL.GetExperienceDetails(candidateBOL);
            dsEducation = candidateBLL.GetEducationDetails(candidateBOL);
            dsCertification = candidateBLL.GetCertificationDetails(candidateBOL);

            if (ListOfActiveExperienceRecords == null)
                ListOfActiveExperienceRecords = GetExperienceRecordsInList(dsExperience);

            if (ListOfInsertedExperienceRecords != null)
                listOfInsertedExperienceRecords = ListOfInsertedExperienceRecords;

            if (ListOfUpdatedExperienceRecords != null)
                listOfUpdatedExperienceRecords = ListOfUpdatedExperienceRecords;

            if (ListOfDeletedExperienceRecords != null)
                listOfDeletedExperienceRecords = ListOfDeletedExperienceRecords;

            if (ListOfActiveCertificationRecords == null)
                ListOfActiveCertificationRecords = GetCertificationRecordsInList(dsCertification);

            if (ListOfInsertedCertificationRecords != null)
                listOfInsertedCertificationRecords = ListOfInsertedCertificationRecords;

            if (ListOfUpdatedCertificationRecords != null)
                listOfUpdatedCertificationRecords = ListOfUpdatedCertificationRecords;

            if (ListOfDeletedCertificationRecords != null)
                listOfDeletedCertificationRecords = ListOfDeletedCertificationRecords;

            if (ListOfActiveEducationRecords == null)
                ListOfActiveEducationRecords = GetEducationRecordsInList(dsEducation);

            if (ListOfInsertedEducationRecords != null)
                listOfInsertedEducationRecords = ListOfInsertedEducationRecords;

            if (ListOfUpdatedEducationRecords != null)
                listOfUpdatedEducationRecords = ListOfUpdatedEducationRecords;

            if (ListOfDeletedEducationRecords != null)
                listOfDeletedEducationRecords = ListOfDeletedEducationRecords;

            if (ViewState["mode"] != null && Convert.ToString(ViewState["mode"]) == "Edit" && Convert.ToBoolean(ViewState["DuplicateEntryLoaded"]) && ViewState["DuplicateEntryLoaded"] != null)
            {
                chkList.Items.Clear();
                List<KeyValuePair<int, string>> listOfSkills = GetAllSkills(Convert.ToString(ViewState["mode"]));
                AddCheckBoxesToCheckBoxList(listOfSkills);

                candidateID = dsTemp.Tables[0].Rows[0]["ID"].ToString();
                candidateBOL.CandidateID = Convert.ToInt32(candidateID);

                dsCandidate = candidateBLL.GetCandidateSkills(candidateBOL);
                foreach (DataRow row in dsCandidate.Tables[0].Rows)
                {
                    chkList.Items.FindByValue(row["ID"].ToString()).Selected = true;
                }
            }

            if (ViewState["mode"] != null && Convert.ToString(ViewState["mode"]) == "Edit" && ViewState["DuplicateEntryLoaded"] == null)
            {
                chkList.Items.Clear();
                List<KeyValuePair<int, string>> listOfSkills = GetAllSkills(Convert.ToString(ViewState["mode"]));
                AddCheckBoxesToCheckBoxList(listOfSkills);
            }

            if (!Page.IsPostBack)
            {
                modeForSkills = "Add";
                ddlSalutation.Focus();
                ddlCandidateStatus.Enabled = true;  //changed recently
                lblCandidateIDDisplay.Visible = false;
                lblTotalWorkExp_Months.Visible = false;
                lblTotalWorkExp_Years.Visible = false;
                lblRelevantWorkExp_Months.Visible = false;
                lblRelevantWorkExp_Years.Visible = false;

                txtDateOfBirth.Attributes.Add("onkeydown", "return false"); // doesnt not allow user to add date manually

                //     For getting latest Experience ID
                if (ExpIndex == 0)
                {
                    string expIndex = candidateBLL.GetLatestExperienceID(candidateBOL);
                    if (!string.IsNullOrEmpty(expIndex))
                        ExpIndex = Convert.ToInt32(expIndex);
                }

                //   For getting latest Certification ID
                if (CertificationIndex == 0)
                {
                    string certificationIndex = candidateBLL.GetLatestCertificationID(candidateBOL);
                    if (!string.IsNullOrEmpty(certificationIndex))
                        CertificationIndex = Convert.ToInt32(certificationIndex);
                }

                //   For getting latest Education ID
                if (EducationIndex == 0)
                {
                    string educationIndex = candidateBLL.GetLatestEducationID(candidateBOL);
                    if (!string.IsNullOrEmpty(educationIndex))
                        EducationIndex = Convert.ToInt32(educationIndex);
                }

                BindOnPageLoad(dsExperience, grdExperienceDetails);

                BindOnPageLoad(dsEducation, grdEducationDetails);

                BindOnPageLoad(dsCertification, grdCertificationDetails);

                //End working code

                BindDropDownList(ddlTotalWorkExpYears, 0, 50);
                BindDropDownList(ddlTotalWorkExpMonths, 0, 12);
                BindDropDownList(ddlRelevantWorkExpYears, 0, 50);
                BindDropDownList(ddlRelevantWorkExpMonths, 0, 12);

                List<KeyValuePair<int, string>> listOfCandidateStatus = GetAllStatus();
                List<KeyValuePair<int, string>> listOfQualifications = GetAllCourses();
                List<KeyValuePair<int, string>> listOfCountryNames = GetAllCountryNames();

                BindDropDownListWithValuesFromDB(ddlCandidateStatus, listOfCandidateStatus);
                BindDropDownListWithValuesFromDB(ddlHighestQualification, listOfQualifications);
                BindDropDownListWithValuesFromDB(ddlCountry, listOfCountryNames);

                ddlCandidateStatus.SelectedIndex = ddlCandidateStatus.Items.IndexOf(ddlCandidateStatus.Items.FindByText("New"));
                // ddlCandidateStatus.SelectedIndex = ddlCandidateStatus.Items.IndexOf(ddlCandidateStatus.Items.FindByValue(dsCandidate.Tables[0].Rows[0]["CandidateStatus"].ToString()));
                List<KeyValuePair<int, string>> listOfSkills = GetAllSkills(modeForSkills);
                AddCheckBoxesToCheckBoxList(listOfSkills);
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "javascript:ApplyClass();", true);
                //txtCandidateHistory.Visible = false;
                //Label1.Visible = false;
                //Label2.Visible = false;
            }
        }
        if (Session["viewProfileClick"] == "true")
        {
            Session["viewProfileClick"] = null;
            txtFirstName.Enabled = false;

            grdExperienceDetails.Enabled = false;
            grdEducationDetails.Enabled = false;
            grdCertificationDetails.Enabled = false;
            chkList.Enabled = false;
        }

        if (Session["editClick"] == "true")
        {
            Session["editClick"] = null;
            lblTotalWorkExp_Months.Visible = false;
            lblTotalWorkExp_Years.Visible = false;
            lblRelevantWorkExp_Months.Visible = false;
            lblRelevantWorkExp_Years.Visible = false;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Save")
            {
                //  start For adding candidate  // region required

                candidateID = AddCandidate();
                candidateBOL.CandidateID = Convert.ToInt32(candidateID);

                List<int> listOfSelectedCheckBoxes = GetSelectedCandidateSkills(chkList);

                //// Test code for Error Cannot have multiple items selected in a DropDownList.

                AddCandidateSkills(listOfSelectedCheckBoxes);

                //end for adding candidate //end required region

                AddCandidateExperienceDetails();
                AddCandidateCertificationDetails();
                AddCandidateEducationDetails();

                if (!string.IsNullOrEmpty(candidateID))
                    UploadFiles(candidateID);
                else
                    lblErrorMsg.Text = "File name not set";

                ListOfActiveExperienceRecords = null;
                ListOfInsertedExperienceRecords = null;
                ListOfUpdatedExperienceRecords = null;
                ListOfDeletedExperienceRecords = null;

                ListOfActiveCertificationRecords = null;
                ListOfInsertedCertificationRecords = null;
                ListOfUpdatedCertificationRecords = null;
                ListOfDeletedCertificationRecords = null;

                ListOfActiveEducationRecords = null;
                ListOfInsertedEducationRecords = null;
                ListOfUpdatedEducationRecords = null;
                ListOfDeletedEducationRecords = null;

                ExpIndex = 0;
                CertificationIndex = 0;
                EducationIndex = 0;

                dsExperience = candidateBLL.GetExperienceDetails(candidateID);
                BindOnPageLoad(dsExperience, grdExperienceDetails);

                dsCertification = candidateBLL.GetCertificationDetails(candidateID);
                BindOnPageLoad(dsCertification, grdCertificationDetails);

                dsEducation = candidateBLL.GetEducationDetails(candidateID);
                BindOnPageLoad(dsEducation, grdEducationDetails);
                string successMessage = "Candidate Details have been saved successfully.";
                string msg = string.Empty;
                msg = successMessage + lblErrorMsg.Text + lblSuccessMsg.Text;
                log.Info("Candidate details are saved.");
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "javascript:SaveAlert('" + msg + "');", true);
            }
            else if (btnSave.Text == "Update")
            {
                UpdateCandidate();

                dsCandidate = candidateBLL.GetCandidateSkills(candidateBOL);

                List<int> listOfSelectedCheckBoxes = GetSelectedCandidateSkills(chkList);

                // Test code for Error Cannot have multiple items selected in a DropDownList.

                List<int> listOfSkillIDsToBeUpdated = new List<int>();
                List<int> listOfSkillIDsToBeDeleted = new List<int>();
                List<int> listOfEsixtingSkillIDs = new List<int>();

                foreach (DataRow row in dsCandidate.Tables[0].Rows)
                {
                    listOfEsixtingSkillIDs.Add(Convert.ToInt32(row["ID"]));
                }

                foreach (var item in listOfSelectedCheckBoxes)
                {
                    if (!listOfEsixtingSkillIDs.Contains(item))
                        listOfSkillIDsToBeUpdated.Add(item);
                }
                foreach (var item in listOfEsixtingSkillIDs)
                {
                    if (!listOfSelectedCheckBoxes.Contains(item))
                        listOfSkillIDsToBeDeleted.Add(item);
                }

                if (listOfSkillIDsToBeUpdated != null && listOfSkillIDsToBeUpdated.Count > 0)
                    AddCandidateSkills(listOfSkillIDsToBeUpdated);

                if (listOfSkillIDsToBeDeleted != null && listOfSkillIDsToBeDeleted.Count > 0)
                    DeleteCandidateSkills(listOfSkillIDsToBeDeleted);

                candidateID = Convert.ToString(candidateBOL.CandidateID);
                AddCandidateCertificationDetails();
                AddCandidateEducationDetails();

                if (!string.IsNullOrEmpty(candidateID))
                {
                    UploadFiles(candidateID);
                }
                else
                    lblErrorMsg.Text = "File name not set";

                ListOfActiveExperienceRecords = null;
                ListOfInsertedExperienceRecords = null;
                ListOfUpdatedExperienceRecords = null;
                ListOfDeletedExperienceRecords = null;

                ListOfActiveCertificationRecords = null;
                ListOfInsertedCertificationRecords = null;
                ListOfUpdatedCertificationRecords = null;
                ListOfDeletedCertificationRecords = null;

                ListOfActiveEducationRecords = null;
                ListOfInsertedEducationRecords = null;
                ListOfUpdatedEducationRecords = null;
                ListOfDeletedEducationRecords = null;

                ViewState["DuplicateEntryLoaded"] = null;
                ViewState["mode"] = null;

                ExpIndex = 0;
                CertificationIndex = 0;
                EducationIndex = 0;

                dsCertification = candidateBLL.GetCertificationDetails(candidateID);
                BindOnPageLoad(dsCertification, grdCertificationDetails);

                dsEducation = candidateBLL.GetEducationDetails(candidateID);
                BindOnPageLoad(dsEducation, grdEducationDetails);

                string successMessage = "Candidate Details have been updated successfully.";
                string msg = string.Empty;
                msg = successMessage + lblErrorMsg.Text + lblSuccessMsg.Text;

                if (ddlCandidateStatus.SelectedValue == "17")
                {
                    dsExperience = candidateBLL.GetExperienceDetails(candidateID);
                    var count = 0;
                    foreach (DataRow row in dsExperience.Tables[0].Rows)
                    {
                        var temp = row["WorkedTill"].ToString();
                        if (temp == "")
                        {
                            string AlertMsg = "All Fields in Experience Details are Mandatory.";
                            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "javascript:UpdateAlert('" + AlertMsg + "');", true);
                            log.Info("Validation error while updating candidate details.");
                            count = 1;
                        }
                    }
                    if (count == 0)
                    {
                        AddCandidateExperienceDetails();
                        dsExperience = candidateBLL.GetExperienceDetails(candidateID);
                        maxcandidatejoined = candidateBLL.GetMaxjoinedcandidate(candidateID);
                        foreach (DataRow row in maxcandidatejoined.Tables[0].Rows)
                        {
                            var temp = Convert.ToInt32(row["Position"].ToString());
                            var cnt = Convert.ToInt32(row["Count"].ToString());
                            if (Convert.ToInt32(cnt) >= Convert.ToInt32(temp))
                            {
                                lblMsg.Text = "as per the RRF no of positions are Filled" + cnt ;
                                log.Info("Validation error while updating candidate details.");
                            }
                            else
                            {
                                BindOnPageLoad(dsExperience, grdExperienceDetails);
                                Session["UserName"] = txtFirstName.Text.ToLower() + "." + txtLastName.Text.ToLower();
                                Session["CandidateID"] = Convert.ToString(candidateID);
                                log.Info(Convert.ToString(Session["UserName"]) + " details are saved and joining pop-up window opened.");
                                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('JoinEmployeePopup.aspx',null,'height=550, width=1200,status= no, resizable= no, scrollbars=yes, toolbar=no,location=no,menubar=no');", true);
                            }
                        }
                    }
                }
                else
                {
                    AddCandidateExperienceDetails();
                    dsExperience = candidateBLL.GetExperienceDetails(candidateID);
                    BindOnPageLoad(dsExperience, grdExperienceDetails);
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "javascript:UpdateAlert('" + msg + "');", true);
                }
            }
        }
        catch (System.Exception ex)
        {
            string msg = string.Empty;
            msg = lblErrorMsg.Text + lblSuccessMsg.Text;
            log.Error("Error occured while saving candidate", ex);
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "javascript:SaveAlert('" + msg + "');", true);
        }
    }

    protected void btnSaveAndAddMore_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSaveAndAddMore.Text == "Save And Add More")
            {
                candidateID = AddCandidate();

                List<int> listOfSelectedCheckBoxes = GetSelectedCandidateSkills(chkList);
                AddCandidateSkills(listOfSelectedCheckBoxes);

                AddCandidateExperienceDetails();
                AddCandidateCertificationDetails();
                AddCandidateEducationDetails();

                if (!string.IsNullOrEmpty(candidateID))
                    UploadFiles(candidateID);
                else
                    lblErrorMsg.Text = "File name not set";

                ListOfActiveExperienceRecords = null;
                ListOfInsertedExperienceRecords = null;
                ListOfUpdatedExperienceRecords = null;
                ListOfDeletedExperienceRecords = null;

                ListOfActiveCertificationRecords = null;
                ListOfInsertedCertificationRecords = null;
                ListOfUpdatedCertificationRecords = null;
                ListOfDeletedCertificationRecords = null;

                ListOfActiveEducationRecords = null;
                ListOfInsertedEducationRecords = null;
                ListOfUpdatedEducationRecords = null;
                ListOfDeletedEducationRecords = null;

                ExpIndex = 0;
                CertificationIndex = 0;
                EducationIndex = 0;

                dsExperience = candidateBLL.GetExperienceDetails(candidateID);
                BindOnPageLoad(dsExperience, grdExperienceDetails);

                dsCertification = candidateBLL.GetCertificationDetails(candidateID);
                BindOnPageLoad(dsCertification, grdCertificationDetails);

                dsEducation = candidateBLL.GetEducationDetails(candidateID);
                BindOnPageLoad(dsEducation, grdEducationDetails);

                string successMessage = "Candidate Details have been saved successfully.";
                string msg = string.Empty;
                msg = successMessage + lblErrorMsg.Text + lblSuccessMsg.Text;

                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "javascript:SaveAndAddMoreAlert('" + msg + "');", true);
            }
            else if (btnSave.Text == "Update And Add More")
            {
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException("Exception in btnSaveAndAddMore_Click. Message:" + ex.Message, ex.InnerException);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Save")
            {
                ClearAllControls();
                ClearGridViews();
                ddlSalutation.Focus();
            }
            else if (btnSave.Text == "Update")
            {
                chkList.Items.Clear();
                GetCandidateDetailsInEditMode(lblCandidateID.Text);
                ddlSalutation.Focus();
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException("Exception in btnCancel_Click. Message:" + ex.Message, ex.InnerException);
        }
    }

    protected void grdExperienceDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Add")
            {
                ExpIndex += 1;
                DropDownList ddlFooterExpType = grdExperienceDetails.FooterRow.FindControl("ddlFooterExpType") as DropDownList;
                TextBox txtFooterOrganisation = grdExperienceDetails.FooterRow.FindControl("txtFooterOrganisation") as TextBox;

                TextBox txtFooterFromDate = grdExperienceDetails.FooterRow.FindControl("txtFooterFromDate") as TextBox;
                TextBox txtFooterToDate = grdExperienceDetails.FooterRow.FindControl("txtFooterToDate") as TextBox;
                TextBox txtFooterDesignation = grdExperienceDetails.FooterRow.FindControl("txtFooterDesignation") as TextBox;
                TextBox txtFooterReportingManager = grdExperienceDetails.FooterRow.FindControl("txtFooterReportingManager") as TextBox;
                TextBox txtFooterLastDrawnCTCInLacs = grdExperienceDetails.FooterRow.FindControl("txtFooterLastDrawnCTCInLacs") as TextBox;

                string lastDrawnCTCInLacs = "";

                if (!string.IsNullOrEmpty(txtFooterLastDrawnCTCInLacs.Text))
                    lastDrawnCTCInLacs = txtFooterLastDrawnCTCInLacs.Text;

                listOfActiveExperienceRecords = ListOfActiveExperienceRecords;
                int count = ListOfActiveExperienceRecords.Count;

                if (txtFooterToDate.Text != "")
                {
                    listOfActiveExperienceRecords.Add(new CandidateBOL()
                    {
                        ExpID = ExpIndex,
                        ExpType = Convert.ToInt32(ddlFooterExpType.SelectedItem.Value),
                        OrganisationName = txtFooterOrganisation.Text,

                        WorkedFrom = DateTime.ParseExact(txtFooterFromDate.Text, "MM/dd/yyyy", null),   // To DO  handle Empty string for DateTime Field
                        WorkedTill = DateTime.ParseExact(txtFooterToDate.Text, "MM/dd/yyyy", null),
                        PositionHeld = txtFooterDesignation.Text,
                        ReportingManager = txtFooterReportingManager.Text,
                        CTC = lastDrawnCTCInLacs
                    });
                }
                else
                {
                    listOfActiveExperienceRecords.Add(new CandidateBOL()
                    {
                        ExpID = ExpIndex,
                        ExpType = Convert.ToInt32(ddlFooterExpType.SelectedItem.Value),
                        OrganisationName = txtFooterOrganisation.Text,

                        WorkedFrom = DateTime.ParseExact(txtFooterFromDate.Text, "MM/dd/yyyy", null),   // To DO  handle Empty string for DateTime Field
                        WorkedTill = Convert.ToDateTime(null),
                        PositionHeld = txtFooterDesignation.Text,
                        ReportingManager = txtFooterReportingManager.Text,
                        CTC = lastDrawnCTCInLacs
                    });
                }
                if (txtFooterToDate.Text != "")
                {
                    listOfInsertedExperienceRecords.Add(new CandidateBOL()
                    {
                        ExpID = ExpIndex,
                        ExpType = Convert.ToInt32(ddlFooterExpType.SelectedItem.Value),
                        OrganisationName = txtFooterOrganisation.Text,

                        WorkedFrom = DateTime.ParseExact(txtFooterFromDate.Text, "MM/dd/yyyy", null),
                        WorkedTill = DateTime.ParseExact(txtFooterToDate.Text, "MM/dd/yyyy", null),
                        PositionHeld = txtFooterDesignation.Text,
                        ReportingManager = txtFooterReportingManager.Text,
                        CTC = lastDrawnCTCInLacs
                    });
                }
                else
                {
                    listOfInsertedExperienceRecords.Add(new CandidateBOL()
                    {
                        ExpID = ExpIndex,
                        ExpType = Convert.ToInt32(ddlFooterExpType.SelectedItem.Value),
                        OrganisationName = txtFooterOrganisation.Text,

                        WorkedFrom = DateTime.ParseExact(txtFooterFromDate.Text, "MM/dd/yyyy", null),
                        WorkedTill = Convert.ToDateTime(null),
                        PositionHeld = txtFooterDesignation.Text,
                        ReportingManager = txtFooterReportingManager.Text,
                        CTC = lastDrawnCTCInLacs
                    });
                }
                ListOfActiveExperienceRecords = listOfActiveExperienceRecords;
                ListOfInsertedExperienceRecords = listOfInsertedExperienceRecords;

                if (ListOfDeletedExperienceRecords != null && ListOfDeletedExperienceRecords.Count > 0)
                {
                    List<CandidateBOL> tempDelete = new List<CandidateBOL>();

                    foreach (CandidateBOL emp in ListOfDeletedExperienceRecords)
                    {
                        if (emp.ExpID == ExpIndex)
                        {
                            tempDelete.Add(emp);
                        }
                    }

                    if (tempDelete != null)
                    {
                        foreach (CandidateBOL emp in tempDelete)
                        {
                            if (ListOfDeletedExperienceRecords.Contains(emp))
                                ListOfDeletedExperienceRecords.Remove(emp);
                        }
                    }
                }

                grdExperienceDetails.DataSource = ListOfActiveExperienceRecords;
                grdExperienceDetails.DataBind();
            }
            else if (e.CommandName == "Delete")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grdExperienceDetails.Rows[index];

                Label lblExpID = row.FindControl("lblExpID") as Label;

                if (ListOfActiveExperienceRecords.Count > 0)  //Checks whether list contains items
                {
                    CandidateBOL itemToBeDeleted = ListOfActiveExperienceRecords.Where(l => l.ExpID == Convert.ToInt32(lblExpID.Text)).ToList<CandidateBOL>().SingleOrDefault(); // item which is to be deleted

                    if (itemToBeDeleted != null)
                    {
                        ListOfActiveExperienceRecords.Remove(itemToBeDeleted);
                    }
                }

                if (listOfInsertedExperienceRecords.Count > 0)  //Checks whether list contains items
                {
                    CandidateBOL itemToBeDeleted = listOfInsertedExperienceRecords.Where(l => l.ExpID == Convert.ToInt32(lblExpID.Text)).ToList<CandidateBOL>().SingleOrDefault(); // item which is to be deleted

                    if (itemToBeDeleted != null)
                    {
                        listOfInsertedExperienceRecords.Remove(itemToBeDeleted);

                        ListOfInsertedExperienceRecords = listOfInsertedExperienceRecords;
                    }
                }

                if (listOfUpdatedExperienceRecords.Count > 0)  //Checks whether list contains items
                {
                    CandidateBOL itemToBeDeleted = listOfUpdatedExperienceRecords.Where(l => l.ExpID == Convert.ToInt32(lblExpID.Text)).ToList<CandidateBOL>().SingleOrDefault(); // item which is to be deleted

                    if (itemToBeDeleted != null)
                    {
                        listOfUpdatedExperienceRecords.Remove(itemToBeDeleted);
                        grdExperienceDetails.DataSource = null;
                        grdExperienceDetails.DataBind();
                        ListOfUpdatedExperienceRecords = listOfUpdatedExperienceRecords;
                    }
                }

                listOfDeletedExperienceRecords.Add(new CandidateBOL() { ExpID = Convert.ToInt32(lblExpID.Text) });

                ListOfDeletedExperienceRecords = listOfDeletedExperienceRecords;

                if (ListOfActiveExperienceRecords != null && ListOfActiveExperienceRecords.Count > 0)
                {
                    grdExperienceDetails.DataSource = ListOfActiveExperienceRecords;
                    grdExperienceDetails.DataBind();
                }
                else if (dsExperience.Tables[0].Rows.Count == 1)
                {
                    BindBlankGridForExperienceDetails();
                }
                else
                {
                    BindBlankGridForExperienceDetails();
                }
            }
            ScriptManager.RegisterStartupScript(this, typeof(string), "", "javascript:ApplyClass();", true);
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    protected void grdExperienceDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void grdExperienceDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdExperienceDetails.EditIndex = e.NewEditIndex;
        grdExperienceDetails.DataSource = ListOfActiveExperienceRecords;
        grdExperienceDetails.DataBind();

        grdExperienceDetails.FooterRow.Visible = true;
    }

    protected void grdExperienceDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Label lblExpID = grdExperienceDetails.Rows[e.RowIndex].FindControl("lblExpID") as Label;
        TextBox txtOrganisation = grdExperienceDetails.Rows[e.RowIndex].FindControl("txtEditOrganisation") as TextBox;

        DropDownList ddlExpType = grdExperienceDetails.Rows[e.RowIndex].FindControl("ddlExpType") as DropDownList;

        TextBox txtFromDate = grdExperienceDetails.Rows[e.RowIndex].FindControl("txtEditFromDate") as TextBox;
        TextBox txtToDate = grdExperienceDetails.Rows[e.RowIndex].FindControl("txtEditToDate") as TextBox;
        TextBox txtDesignation = grdExperienceDetails.Rows[e.RowIndex].FindControl("txtEditDesignation") as TextBox;
        TextBox txtReportingManager = grdExperienceDetails.Rows[e.RowIndex].FindControl("txtEditReportingManager") as TextBox;
        TextBox txtLastDrawnCTCInLacs = grdExperienceDetails.Rows[e.RowIndex].FindControl("txtEditLastDrawnCTCInLacs") as TextBox;
        string lastDrawnCTCInLacs = "";

        if (!string.IsNullOrEmpty(txtLastDrawnCTCInLacs.Text))
            lastDrawnCTCInLacs = txtLastDrawnCTCInLacs.Text;

        if (txtToDate.Text != "" || txtToDate.Text != null)
        {
            if (ListOfActiveExperienceRecords.Count > 0) //Checks whether list contains items
            {
                foreach (CandidateBOL emp in ListOfActiveExperienceRecords)
                {
                    if (emp.ExpID == Convert.ToInt32(lblExpID.Text))
                    {
                        emp.OrganisationName = txtOrganisation.Text;
                        emp.ExpType = Convert.ToInt32(ddlExpType.SelectedItem.Value);
                        emp.WorkedFrom = DateTime.ParseExact(txtFromDate.Text, "MM/dd/yyyy", null);
                        emp.WorkedTill = DateTime.ParseExact(txtToDate.Text, "MM/dd/yyyy", null);
                        emp.PositionHeld = txtDesignation.Text;
                        emp.ReportingManager = txtReportingManager.Text;
                        emp.CTC = lastDrawnCTCInLacs;
                    }
                }
            }
        }
        else
        {
            if (ListOfActiveExperienceRecords.Count > 0) //Checks whether list contains items
            {
                foreach (CandidateBOL emp in ListOfActiveExperienceRecords)
                {
                    if (emp.ExpID == Convert.ToInt32(lblExpID.Text))
                    {
                        emp.OrganisationName = txtOrganisation.Text;
                        emp.ExpType = Convert.ToInt32(ddlExpType.SelectedItem.Value);
                        emp.WorkedFrom = DateTime.ParseExact(txtFromDate.Text, "MM/dd/yyyy", null);
                        emp.WorkedTill = Convert.ToDateTime(null);
                        emp.PositionHeld = txtDesignation.Text;
                        emp.ReportingManager = txtReportingManager.Text;
                        emp.CTC = lastDrawnCTCInLacs;
                    }
                }
            }
        }

        bool insertedItem = false;
        if (txtToDate.Text != "" || txtToDate.Text != null)
        {
            if (listOfInsertedExperienceRecords.Count > 0) //Checks whether list contains items
            {
                foreach (CandidateBOL emp in listOfInsertedExperienceRecords)
                {
                    if (emp.ExpID == Convert.ToInt32(lblExpID.Text))
                    {
                        insertedItem = true;
                        emp.OrganisationName = txtOrganisation.Text;
                        emp.ExpType = Convert.ToInt32(ddlExpType.SelectedItem.Value);
                        emp.WorkedFrom = DateTime.ParseExact(txtFromDate.Text, "MM/dd/yyyy", null);
                        emp.WorkedTill = DateTime.ParseExact(txtToDate.Text, "MM/dd/yyyy", null);
                        emp.PositionHeld = txtDesignation.Text;
                        emp.ReportingManager = txtReportingManager.Text;
                        emp.CTC = lastDrawnCTCInLacs;
                    }
                }

                ListOfInsertedExperienceRecords = listOfInsertedExperienceRecords;
            }
        }
        else
        {
            if (listOfInsertedExperienceRecords.Count > 0) //Checks whether list contains items
            {
                foreach (CandidateBOL emp in listOfInsertedExperienceRecords)
                {
                    if (emp.ExpID == Convert.ToInt32(lblExpID.Text))
                    {
                        insertedItem = true;
                        emp.OrganisationName = txtOrganisation.Text;
                        emp.ExpType = Convert.ToInt32(ddlExpType.SelectedItem.Value);
                        emp.WorkedFrom = DateTime.ParseExact(txtFromDate.Text, "MM/dd/yyyy", null);
                        emp.WorkedTill = Convert.ToDateTime(null);
                        emp.PositionHeld = txtDesignation.Text;
                        emp.ReportingManager = txtReportingManager.Text;
                        emp.CTC = lastDrawnCTCInLacs;
                    }
                }

                ListOfInsertedExperienceRecords = listOfInsertedExperienceRecords;
            }
        }
        bool isUpdateItemAdded = false;
        if (txtToDate.Text != "" || txtToDate.Text != null)
        {
            if (!insertedItem && (listOfUpdatedExperienceRecords.Count > 0))
            {
                foreach (CandidateBOL emp in listOfUpdatedExperienceRecords)
                {
                    if (emp.ExpID == Convert.ToInt32(lblExpID.Text))
                    {
                        emp.OrganisationName = txtOrganisation.Text;
                        emp.ExpType = Convert.ToInt32(ddlExpType.SelectedItem.Value);
                        emp.WorkedFrom = DateTime.ParseExact(txtFromDate.Text, "MM/dd/yyyy", null);
                        emp.WorkedTill = DateTime.ParseExact(txtToDate.Text, "MM/dd/yyyy", null);
                        emp.PositionHeld = txtDesignation.Text;
                        emp.ReportingManager = txtReportingManager.Text;
                        emp.CTC = lastDrawnCTCInLacs;

                        isUpdateItemAdded = true;
                    }
                }
                ListOfUpdatedExperienceRecords = listOfUpdatedExperienceRecords;
            }
        }
        else
        {
            if (!insertedItem && (listOfUpdatedExperienceRecords.Count > 0))
            {
                foreach (CandidateBOL emp in listOfUpdatedExperienceRecords)
                {
                    if (emp.ExpID == Convert.ToInt32(lblExpID.Text))
                    {
                        emp.OrganisationName = txtOrganisation.Text;
                        emp.ExpType = Convert.ToInt32(ddlExpType.SelectedItem.Value);
                        emp.WorkedFrom = DateTime.ParseExact(txtFromDate.Text, "MM/dd/yyyy", null);
                        emp.WorkedTill = Convert.ToDateTime(null);
                        emp.PositionHeld = txtDesignation.Text;
                        emp.ReportingManager = txtReportingManager.Text;
                        emp.CTC = lastDrawnCTCInLacs;

                        isUpdateItemAdded = true;
                    }
                }
                ListOfUpdatedExperienceRecords = listOfUpdatedExperienceRecords;
            }
        }
        if (txtToDate.Text != "" || txtToDate.Text != null)
        {
            if (!insertedItem && !isUpdateItemAdded)
            {
                listOfUpdatedExperienceRecords.Add(new CandidateBOL()
                {
                    ExpID = Convert.ToInt32(lblExpID.Text),
                    OrganisationName = txtOrganisation.Text,
                    ExpType = Convert.ToInt32(ddlExpType.SelectedItem.Value),
                    WorkedFrom = DateTime.ParseExact(txtFromDate.Text, "MM/dd/yyyy", null),
                    WorkedTill = DateTime.ParseExact(txtToDate.Text, "MM/dd/yyyy", null),
                    PositionHeld = txtDesignation.Text,
                    ReportingManager = txtReportingManager.Text,
                    CTC = lastDrawnCTCInLacs
                });

                ListOfUpdatedExperienceRecords = listOfUpdatedExperienceRecords;
            }
        }
        else
        {
            if (!insertedItem && !isUpdateItemAdded)
            {
                listOfUpdatedExperienceRecords.Add(new CandidateBOL()
                {
                    ExpID = Convert.ToInt32(lblExpID.Text),
                    OrganisationName = txtOrganisation.Text,
                    ExpType = Convert.ToInt32(ddlExpType.SelectedItem.Value),
                    WorkedFrom = DateTime.ParseExact(txtFromDate.Text, "MM/dd/yyyy", null),
                    WorkedTill = Convert.ToDateTime(null),
                    PositionHeld = txtDesignation.Text,
                    ReportingManager = txtReportingManager.Text,
                    CTC = lastDrawnCTCInLacs
                });

                ListOfUpdatedExperienceRecords = listOfUpdatedExperienceRecords;
            }
        }
        grdExperienceDetails.EditIndex = -1;
        grdExperienceDetails.DataSource = ListOfActiveExperienceRecords;
        grdExperienceDetails.DataBind();
        grdExperienceDetails.FooterRow.Visible = true;
    }

    protected void grdExperienceDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdExperienceDetails.EditIndex = -1;
        grdExperienceDetails.DataSource = ListOfActiveExperienceRecords;
        grdExperienceDetails.DataBind();
        grdExperienceDetails.FooterRow.Visible = true;
    }

    protected void grdExperienceDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ListItem item = new ListItem() { Text = "select", Value = "none" };
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox txtFooterOrganisation = e.Row.FindControl("txtFooterOrganisation") as TextBox;
            TextBox txtFooterFromDate = e.Row.FindControl("txtFooterFromDate") as TextBox;
            TextBox txtFooterToDate = e.Row.FindControl("txtFooterToDate") as TextBox;
            TextBox txtFooterDesignation = e.Row.FindControl("txtFooterDesignation") as TextBox;
            TextBox txtFooterReportingManager = e.Row.FindControl("txtFooterReportingManager") as TextBox;
            TextBox txtFooterLastDrawnCTCInLacs = e.Row.FindControl("txtFooterLastDrawnCTCInLacs") as TextBox;
            DropDownList ddlFooterExpType = e.Row.FindControl("ddlFooterExpType") as DropDownList;

            ddlFooterExpType.DataSource = GetAllExpTypes();  // To Bind Footer Row of Experience Grid
            ddlFooterExpType.DataTextField = "Value";
            ddlFooterExpType.DataValueField = "Key";
            ddlFooterExpType.DataBind();
            ddlFooterExpType.Items.Insert(0, item);

            Button btnAddMoreExperience = e.Row.FindControl("btnAddMoreExperience") as Button;
            if (btnAddMoreExperience != null)
            {
                btnAddMoreExperience.Attributes.Add("onClick", "return validateExperience(" + txtFooterOrganisation.ClientID + "," + txtFooterFromDate.ClientID + "," + txtFooterToDate.ClientID + "," + ddlFooterExpType.ClientID + " , " + txtFooterDesignation.ClientID + "," + txtFooterReportingManager.ClientID + "," + txtFooterLastDrawnCTCInLacs.ClientID + ");");  // Attribute added for insert using javascript validation
            }
            txtFooterFromDate.Attributes.Add("onkeydown", "return false");
            txtFooterToDate.Attributes.Add("onkeydown", "return false");
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblExpType = e.Row.FindControl("lblExpType") as Label;
            Label lblExpType3 = e.Row.FindControl("lblExpType3") as Label;

            DropDownList ddlExpType = e.Row.FindControl("ddlExpType") as DropDownList;

            TextBox txtEditOrganisation = e.Row.FindControl("txtEditOrganisation") as TextBox;

            TextBox txtEditFromDate = e.Row.FindControl("txtEditFromDate") as TextBox;
            TextBox txtEditToDate = e.Row.FindControl("txtEditToDate") as TextBox;
            TextBox txtEditDesignation = e.Row.FindControl("txtEditDesignation") as TextBox;
            TextBox txtEditReportingManager = e.Row.FindControl("txtEditReportingManager") as TextBox;
            TextBox txtEditLastDrawnCTCInLacs = e.Row.FindControl("txtEditLastDrawnCTCInLacs") as TextBox;

            if (lblExpType != null)
            {
                if (string.IsNullOrEmpty(lblExpType.Text))
                    lblExpType3.Text = string.Empty;
                else
                    lblExpType3.Text = GetExpTypeByID(Convert.ToInt32(lblExpType.Text));
            }

            if (ddlExpType != null)
            {
                ddlExpType.DataSource = GetAllExpTypes();  // To Bind Footer Row of Education Grid
                ddlExpType.DataTextField = "Value";
                ddlExpType.DataValueField = "Key";
                ddlExpType.DataBind();
                ddlExpType.Items.Insert(0, item);
            }

            Button btnUpdate = e.Row.FindControl("btnUpdate") as Button;
            if (btnUpdate != null)

                btnUpdate.Attributes.Add("onClick", "return validateExperience(" + txtEditOrganisation.ClientID + "," + txtEditFromDate.ClientID + "," + txtEditToDate.ClientID + "," + ddlExpType.ClientID + " , " + txtEditDesignation.ClientID + "," + txtEditReportingManager.ClientID + "," + txtEditLastDrawnCTCInLacs.ClientID + ");");  // Attribute added for insert using javascript validation
        }
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
        {
            Label lblExpType2 = e.Row.FindControl("lblExpType2") as Label;

            DropDownList ddlExpType = e.Row.FindControl("ddlExpType") as DropDownList;

            if (lblExpType2 != null)
            {
                ddlExpType.SelectedIndex = ddlExpType.Items.IndexOf(ddlExpType.Items.FindByValue(lblExpType2.Text));
            }
        }
    }

    protected void grdCertificationDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Add")
            {
                CertificationIndex += 1;

                DropDownList ddlFooterCertificationName = grdCertificationDetails.FooterRow.FindControl("ddlFooterCertificationName") as DropDownList;

                TextBox txtFooterCertificationNo = grdCertificationDetails.FooterRow.FindControl("txtFooterCertificationNo") as TextBox;
                TextBox txtFooterInstitution = grdCertificationDetails.FooterRow.FindControl("txtFooterInstitution") as TextBox;
                TextBox txtFooterCertifiedOnDate = grdCertificationDetails.FooterRow.FindControl("txtFooterCertifiedOnDate") as TextBox;
                TextBox txtFooterCertificationScore = grdCertificationDetails.FooterRow.FindControl("txtFooterCertificationScore") as TextBox;
                TextBox txtFooterCertificationGrade = grdCertificationDetails.FooterRow.FindControl("txtFooterCertificationGrade") as TextBox;

                string certificationScore = "";

                if (!string.IsNullOrEmpty(txtFooterCertificationScore.Text))
                    certificationScore = txtFooterCertificationScore.Text;

                listOfActiveCertificationRecords = ListOfActiveCertificationRecords;
                int count = ListOfActiveCertificationRecords.Count;

                listOfActiveCertificationRecords.Add(new CandidateBOL()
                {
                    CertificationID = CertificationIndex,

                    CertificationName = Convert.ToInt32(ddlFooterCertificationName.SelectedItem.Value),

                    CertificationNo = txtFooterCertificationNo.Text,
                    Institution = txtFooterInstitution.Text,
                    CertificationDate = DateTime.ParseExact(txtFooterCertifiedOnDate.Text, "MM/dd/yyyy", null),
                    CertificationScore = certificationScore,
                    CertificationGrade = txtFooterCertificationGrade.Text
                });

                listOfInsertedCertificationRecords.Add(new CandidateBOL()
                {
                    CertificationID = CertificationIndex,

                    CertificationName = Convert.ToInt32(ddlFooterCertificationName.SelectedItem.Value),

                    CertificationNo = txtFooterCertificationNo.Text,
                    Institution = txtFooterInstitution.Text,
                    CertificationDate = DateTime.ParseExact(txtFooterCertifiedOnDate.Text, "MM/dd/yyyy", null),
                    CertificationScore = certificationScore,
                    CertificationGrade = txtFooterCertificationGrade.Text
                });

                ListOfActiveCertificationRecords = listOfActiveCertificationRecords;
                ListOfInsertedCertificationRecords = listOfInsertedCertificationRecords;

                if (ListOfDeletedCertificationRecords != null && ListOfDeletedCertificationRecords.Count > 0)
                {
                    List<CandidateBOL> tempDelete = new List<CandidateBOL>();

                    foreach (CandidateBOL emp in ListOfDeletedCertificationRecords)
                    {
                        if (emp.CertificationID == CertificationIndex)
                        {
                            tempDelete.Add(emp);
                        }
                    }

                    if (tempDelete != null)
                    {
                        foreach (CandidateBOL emp in tempDelete)
                        {
                            if (ListOfDeletedCertificationRecords.Contains(emp))
                                ListOfDeletedCertificationRecords.Remove(emp);
                        }
                    }
                }

                grdCertificationDetails.DataSource = ListOfActiveCertificationRecords;
                grdCertificationDetails.DataBind();
            }
            else if (e.CommandName == "Delete")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grdCertificationDetails.Rows[index];

                Label lblCertificationSrNo = row.FindControl("lblCertificationSrNo") as Label;
                Label lblCertificationID = row.FindControl("lblCertificationID") as Label;

                if (ListOfActiveCertificationRecords.Count > 0)  //Checks whether list contains items
                {
                    CandidateBOL itemToBeDeleted = ListOfActiveCertificationRecords.Where(l => l.CertificationID == Convert.ToInt32(lblCertificationID.Text)).ToList<CandidateBOL>().SingleOrDefault(); // item which is to be deleted

                    if (itemToBeDeleted != null)
                    {
                        ListOfActiveCertificationRecords.Remove(itemToBeDeleted);
                    }
                }

                if (listOfInsertedCertificationRecords.Count > 0)  //Checks whether list contains items
                {
                    CandidateBOL itemToBeDeleted = listOfInsertedCertificationRecords.Where(l => l.CertificationID == Convert.ToInt32(lblCertificationID.Text)).ToList<CandidateBOL>().SingleOrDefault(); // item which is to be deleted

                    if (itemToBeDeleted != null)
                    {
                        listOfInsertedCertificationRecords.Remove(itemToBeDeleted);

                        ListOfInsertedCertificationRecords = listOfInsertedCertificationRecords;
                    }
                }

                if (listOfUpdatedCertificationRecords.Count > 0)  //Checks whether list contains items
                {
                    CandidateBOL itemToBeDeleted = listOfUpdatedCertificationRecords.Where(l => l.CertificationID == Convert.ToInt32(lblCertificationID.Text)).ToList<CandidateBOL>().SingleOrDefault(); // item which is to be deleted

                    if (itemToBeDeleted != null)
                    {
                        listOfUpdatedCertificationRecords.Remove(itemToBeDeleted);
                        grdCertificationDetails.DataSource = null;
                        grdCertificationDetails.DataBind();
                        ListOfUpdatedCertificationRecords = listOfUpdatedCertificationRecords;
                    }
                }

                listOfDeletedCertificationRecords.Add(new CandidateBOL() { CertificationID = Convert.ToInt32(lblCertificationID.Text) });

                ListOfDeletedCertificationRecords = listOfDeletedCertificationRecords;

                if (ListOfActiveCertificationRecords != null && ListOfActiveCertificationRecords.Count > 0)
                {
                    grdCertificationDetails.DataSource = ListOfActiveCertificationRecords;
                    grdCertificationDetails.DataBind();
                }
                else if (dsCertification.Tables[0].Rows.Count == 1)
                {
                    BindBlankGridForCertificationDetails();
                }
                else
                {
                    BindBlankGridForCertificationDetails();
                }
            }
            ScriptManager.RegisterStartupScript(this, typeof(string), "", "javascript:ApplyClass();", true);
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    protected void grdCertificationDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdCertificationDetails.EditIndex = e.NewEditIndex;
        grdCertificationDetails.DataSource = ListOfActiveCertificationRecords;
        grdCertificationDetails.DataBind();
        grdCertificationDetails.FooterRow.Visible = true;
    }

    protected void grdCertificationDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void grdCertificationDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ListItem item = new ListItem() { Text = "select", Value = "none" };
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlFooterCertificationName = e.Row.FindControl("ddlFooterCertificationName") as DropDownList;
            TextBox txtFooterCertificationNo = e.Row.FindControl("txtFooterCertificationNo") as TextBox;
            TextBox txtFooterInstitution = e.Row.FindControl("txtFooterInstitution") as TextBox;
            TextBox txtFooterCertifiedOnDate = e.Row.FindControl("txtFooterCertifiedOnDate") as TextBox;
            TextBox txtFooterCertificationScore = e.Row.FindControl("txtFooterCertificationScore") as TextBox;
            TextBox txtFooterCertificationGrade = e.Row.FindControl("txtFooterCertificationGrade") as TextBox;

            ddlFooterCertificationName.DataSource = GetAllCertificationNames();  // To Bind Footer Row of Certification Grid
            ddlFooterCertificationName.DataTextField = "Value";
            ddlFooterCertificationName.DataValueField = "Key";
            ddlFooterCertificationName.DataBind();
            ddlFooterCertificationName.Items.Insert(0, item);

            Button btnAddMoreCertification = e.Row.FindControl("btnAddMoreCertification") as Button;
            btnAddMoreCertification.Attributes.Add("onClick", "return validateCertification(" + ddlFooterCertificationName.ClientID + " , " + txtFooterCertificationNo.ClientID + "," + txtFooterInstitution.ClientID + "," + txtFooterCertifiedOnDate.ClientID + "," + txtFooterCertificationScore.ClientID + "," + txtFooterCertificationGrade.ClientID + ");");  // Attribute added for insert using javascript validation
            txtFooterCertifiedOnDate.Attributes.Add("onkeydown", "return false");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblCertificationName = e.Row.FindControl("lblCertificationName") as Label;
            Label lblCertificationNameName = e.Row.FindControl("lblCertificationNameName") as Label;

            DropDownList ddlCertificationName = e.Row.FindControl("ddlCertificationName") as DropDownList;
            TextBox txtCertificationNo = e.Row.FindControl("txtEditCertificationNo") as TextBox;
            TextBox txtInstitution = e.Row.FindControl("txtEditInstitution") as TextBox;
            TextBox txtCertifiedOnDate = e.Row.FindControl("txtEditCertifiedOnDate") as TextBox;
            TextBox txtCertificationScore = e.Row.FindControl("txtEditCertificationScore") as TextBox;
            TextBox txtCertificationGrade = e.Row.FindControl("txtEditCertificationGrade") as TextBox;

            if (lblCertificationName != null)
            {
                if (string.IsNullOrEmpty(lblCertificationName.Text))
                    lblCertificationNameName.Text = string.Empty;
                else
                    lblCertificationNameName.Text = GetCertificationNameID(Convert.ToInt32(lblCertificationName.Text));
            }

            if (ddlCertificationName != null)
            {
                ddlCertificationName.DataSource = GetAllCertificationNames();  // To Bind Footer Row of Education Grid
                ddlCertificationName.DataTextField = "Value";
                ddlCertificationName.DataValueField = "Key";
                ddlCertificationName.DataBind();

                ddlCertificationName.Items.Insert(0, item);
            }

            Button btnUpdateCertification = e.Row.FindControl("btnUpdateCertification") as Button;
            if (btnUpdateCertification != null)
                btnUpdateCertification.Attributes.Add("onClick", "return validateCertification(" + ddlCertificationName.ClientID + " , " + txtCertificationNo.ClientID + "," + txtInstitution.ClientID + "," + txtCertifiedOnDate.ClientID + "," + txtCertificationScore.ClientID + "," + txtCertificationGrade.ClientID + ");"); // Attribute added for update using javascript validation
        }

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
        {
            Label lblCertificationName2 = e.Row.FindControl("lblCertificationName2") as Label;
            DropDownList ddlCertificationName = e.Row.FindControl("ddlCertificationName") as DropDownList;
            if (lblCertificationName2 != null)
            {
                ddlCertificationName.SelectedIndex = ddlCertificationName.Items.IndexOf(ddlCertificationName.Items.FindByValue(lblCertificationName2.Text));
            }
        }
    }

    protected void grdCertificationDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Label lblCertificationID = grdCertificationDetails.Rows[e.RowIndex].FindControl("lblCertificationID") as Label;
        DropDownList ddlCertificationName = grdCertificationDetails.Rows[e.RowIndex].FindControl("ddlCertificationName") as DropDownList;
        TextBox txtCertificationNo = grdCertificationDetails.Rows[e.RowIndex].FindControl("txtEditCertificationNo") as TextBox;
        TextBox txtInstitution = grdCertificationDetails.Rows[e.RowIndex].FindControl("txtEditInstitution") as TextBox;
        TextBox txtCertifiedOnDate = grdCertificationDetails.Rows[e.RowIndex].FindControl("txtEditCertifiedOnDate") as TextBox;
        TextBox txtCertificationScore = grdCertificationDetails.Rows[e.RowIndex].FindControl("txtEditCertificationScore") as TextBox;
        TextBox txtCertificationGrade = grdCertificationDetails.Rows[e.RowIndex].FindControl("txtEditCertificationGrade") as TextBox;

        string certificationScore = "";

        if (!string.IsNullOrEmpty(txtCertificationScore.Text))
            certificationScore = txtCertificationScore.Text;

        if (ListOfActiveCertificationRecords.Count > 0) //Checks whether list contains items
        {
            foreach (CandidateBOL emp in ListOfActiveCertificationRecords)
            {
                if (emp.CertificationID == Convert.ToInt32(lblCertificationID.Text))
                {
                    emp.CertificationName = Convert.ToInt32(ddlCertificationName.SelectedItem.Value);
                    emp.CertificationNo = txtCertificationNo.Text;
                    emp.Institution = txtInstitution.Text;
                    emp.CertificationDate = DateTime.ParseExact(txtCertifiedOnDate.Text, "MM/dd/yyyy", null);
                    emp.CertificationScore = certificationScore;
                    emp.CertificationGrade = txtCertificationGrade.Text;
                }
            }
        }

        bool insertedItem = false;

        if (listOfInsertedCertificationRecords.Count > 0) //Checks whether list contains items
        {
            foreach (CandidateBOL emp in listOfInsertedCertificationRecords)
            {
                if (emp.CertificationID == Convert.ToInt32(lblCertificationID.Text))
                {
                    insertedItem = true;
                    emp.CertificationName = Convert.ToInt32(ddlCertificationName.SelectedItem.Value);
                    emp.CertificationNo = txtCertificationNo.Text;
                    emp.Institution = txtInstitution.Text;
                    emp.CertificationDate = DateTime.ParseExact(txtCertifiedOnDate.Text, "MM/dd/yyyy", null);
                    emp.CertificationScore = certificationScore;
                    emp.CertificationGrade = txtCertificationGrade.Text;
                }
            }

            ListOfInsertedCertificationRecords = listOfInsertedCertificationRecords;
        }

        bool isUpdateItemAdded = false;

        if (!insertedItem && (listOfUpdatedCertificationRecords.Count > 0))
        {
            foreach (CandidateBOL emp in listOfUpdatedCertificationRecords)
            {
                if (emp.CertificationID == Convert.ToInt32(lblCertificationID.Text))
                {
                    emp.CertificationName = Convert.ToInt32(ddlCertificationName.SelectedItem.Value);
                    emp.CertificationNo = txtCertificationNo.Text;
                    emp.Institution = txtInstitution.Text;
                    emp.CertificationDate = DateTime.ParseExact(txtCertifiedOnDate.Text, "MM/dd/yyyy", null);
                    emp.CertificationScore = certificationScore;
                    emp.CertificationGrade = txtCertificationGrade.Text;
                    isUpdateItemAdded = true;
                }
            }
            ListOfUpdatedCertificationRecords = listOfUpdatedCertificationRecords;
        }

        if (!insertedItem && !isUpdateItemAdded)
        {
            listOfUpdatedCertificationRecords.Add(new CandidateBOL()
            {
                CertificationID = Convert.ToInt32(lblCertificationID.Text),
                CertificationName = Convert.ToInt32(ddlCertificationName.SelectedItem.Value),
                //CertificationName = txtCertificationName.Text,
                CertificationNo = txtCertificationNo.Text,
                Institution = txtInstitution.Text,
                CertificationDate = DateTime.ParseExact(txtCertifiedOnDate.Text, "MM/dd/yyyy", null),
                CertificationScore = certificationScore,
                CertificationGrade = txtCertificationGrade.Text
            });

            ListOfUpdatedCertificationRecords = listOfUpdatedCertificationRecords;
        }

        grdCertificationDetails.DataSource = ListOfActiveCertificationRecords;
        grdCertificationDetails.DataBind();
        //btnRemove1.Visible = true;
        grdCertificationDetails.FooterRow.Visible = true;

        // code to solve row updating issue

        grdCertificationDetails.EditIndex = -1;
        grdCertificationDetails.DataSource = ListOfActiveCertificationRecords;
        grdCertificationDetails.DataBind();
        grdCertificationDetails.FooterRow.Visible = true;

        //
    }

    protected void grdCertificationDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdCertificationDetails.EditIndex = -1;
        grdCertificationDetails.DataSource = ListOfActiveCertificationRecords;
        grdCertificationDetails.DataBind();
        //btnRemove1.Visible = true;
        grdCertificationDetails.FooterRow.Visible = true;
    }

    protected void grdEducationDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlFooterCourse = e.Row.FindControl("ddlFooterCourse") as DropDownList;
            DropDownList ddlFooterYear = e.Row.FindControl("ddlFooterYear") as DropDownList;
            DropDownList ddlFooterType = e.Row.FindControl("ddlFooterType") as DropDownList;
            DropDownList ddlFooterDegree = e.Row.FindControl("ddlFooterDegree") as DropDownList;
            //DropDownList ddlFooterPGUG = e.Row.FindControl("ddlFooterPGUG") as DropDownList;
            //DropDownList ddlFooterEstablishment = e.Row.FindControl("ddlFooterEstablishment") as DropDownList;

            //TextBox txtFooterInstitute = e.Row.FindControl("txtFooterInstitute") as TextBox;
            TextBox txtFooterSpecialization = e.Row.FindControl("txtFooterSpecialization") as TextBox;
            //   TextBox txtFooterEstablishment = e.Row.FindControl("txtFooterEstablishment") as TextBox;
            TextBox txtFooterUniversity = e.Row.FindControl("txtFooterUniversity") as TextBox;
            TextBox txtFooterPercentage = e.Row.FindControl("txtFooterPercentage") as TextBox;

            ListItem item = new ListItem() { Text = "select", Value = "none" };

            ddlFooterYear.DataSource = GetYearsAndMonths(1950, 2200); // To Bind Footer Row of Education Grid
            ddlFooterYear.DataBind();
            ddlFooterYear.Items.Insert(0, item);

            //List<KeyValuePair<int, string>> list = GetAllCourseTypes();

            ddlFooterType.DataSource = GetAllCourseTypes();  // To Bind Footer Row of Education Grid
            ddlFooterType.DataTextField = "Value";
            ddlFooterType.DataValueField = "Key";
            ddlFooterType.DataBind();
            ddlFooterType.Items.Insert(0, item);

            ddlFooterDegree.DataSource = GetAllDegree();  // To Bind Footer Row of Education Grid
            ddlFooterDegree.DataTextField = "Value";
            ddlFooterDegree.DataValueField = "Key";
            ddlFooterDegree.DataBind();
            ddlFooterDegree.Items.Insert(0, item);

            ddlFooterCourse.DataSource = GetAllCourses();  // To Bind Footer Row of Education Grid
            ddlFooterCourse.DataTextField = "Value";
            ddlFooterCourse.DataValueField = "Key";
            ddlFooterCourse.DataBind();
            ddlFooterCourse.Items.Insert(0, item);

            Button btnAddMoreEducation = e.Row.FindControl("btnAddMoreEducation") as Button;
            //btnAddMoreEducation.Attributes.Add("onClick", "return validateEducation(" + ddlFooterPGUG.ClientID + " , " + ddlFooterCourse.ClientID + "," + txtFooterSpecialization.ClientID + "," + ddlFooterEstablishment.ClientID + "," + txtFooterUniversity.ClientID + "," + ddlFooterYear.ClientID + "," + ddlFooterType.ClientID + "," + txtFooterPercentage.ClientID + ");");  // Attribute added for insert using javascript validation
            btnAddMoreEducation.Attributes.Add("onClick", "return validateEducation(" + ddlFooterDegree.ClientID + " , " + ddlFooterCourse.ClientID + "," + txtFooterSpecialization.ClientID + "," + txtFooterUniversity.ClientID + "," + ddlFooterYear.ClientID + "," + ddlFooterType.ClientID + "," + txtFooterPercentage.ClientID + ");");  // Attribute added for insert using javascript validation
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDegree = e.Row.FindControl("lblDegree") as Label;
            Label lblDegreeName = e.Row.FindControl("lblDegreeName") as Label;

            Label lblCourse = e.Row.FindControl("lblCourse") as Label;
            Label lblCourseName = e.Row.FindControl("lblCourseName") as Label;

            Label lblType = e.Row.FindControl("lblType") as Label;
            Label lblTypeCourse = e.Row.FindControl("lblTypeCourse") as Label;

            DropDownList ddlDegree = e.Row.FindControl("ddlDegree") as DropDownList;
            DropDownList ddlCourse = e.Row.FindControl("ddlCourse") as DropDownList;
            DropDownList ddlYear = e.Row.FindControl("ddlYear") as DropDownList;
            DropDownList ddlType = e.Row.FindControl("ddlType") as DropDownList;

            TextBox txtEditSpecialization = e.Row.FindControl("txtEditSpecialization") as TextBox;
            // TextBox txtEditInstitute = e.Row.FindControl("txtEditInstitute") as TextBox;
            TextBox txtEditUniversity = e.Row.FindControl("txtEditUniversity") as TextBox;
            TextBox txtEditPercentage = e.Row.FindControl("txtEditPercentage") as TextBox;

            if (lblCourse != null)
            {
                if (string.IsNullOrEmpty(lblCourse.Text))
                    lblCourseName.Text = string.Empty;
                else
                    lblCourseName.Text = GetCourseByID(Convert.ToInt32(lblCourse.Text));
            }

            if (lblDegree != null)
            {
                if (string.IsNullOrEmpty(lblDegree.Text))
                    lblDegreeName.Text = string.Empty;
                else
                    lblDegreeName.Text = GetDegreeID(Convert.ToInt32(lblDegree.Text));
            }

            if (lblType != null)
            {
                if (string.IsNullOrEmpty(lblType.Text))
                    lblTypeCourse.Text = string.Empty;
                else
                    lblTypeCourse.Text = GetCourseTypeByID(Convert.ToInt32(lblType.Text));
            }

            ListItem item = new ListItem() { Text = "select", Value = "none" };

            if (ddlYear != null)
            {
                ddlYear.DataSource = GetYearsAndMonths(1950, 2200);
                ddlYear.DataBind();

                ddlYear.Items.Insert(0, item);
            }
            if (ddlType != null)
            {
                ddlType.DataSource = GetAllCourseTypes();  // To Bind Footer Row of Education Grid
                ddlType.DataTextField = "Value";
                ddlType.DataValueField = "Key";
                ddlType.DataBind();

                ddlType.Items.Insert(0, item);
            }

            if (ddlDegree != null)
            {
                ddlDegree.DataSource = GetAllDegree();  // To Bind Footer Row of Education Grid
                ddlDegree.DataTextField = "Value";
                ddlDegree.DataValueField = "Key";
                ddlDegree.DataBind();

                ddlDegree.Items.Insert(0, item);
            }

            if (ddlCourse != null)
            {
                ddlCourse.DataSource = GetAllCourses();  // To Bind Footer Row of Education Grid
                ddlCourse.DataTextField = "Value";
                ddlCourse.DataValueField = "Key";
                ddlCourse.DataBind();

                ddlDegree.Items.Insert(0, item);
            }

            Button btnUpdateEducation = e.Row.FindControl("btnUpdateEducation") as Button;

            if (btnUpdateEducation != null)
                //btnUpdateEducation.Attributes.Add("onClick", "return validateEducation(" + ddlPGUG.ClientID + " , " + ddlCourse.ClientID + "," + txtEditSpecialization.ClientID + "," + ddlEstablishment.ClientID + "," + txtEditUniversity.ClientID + "," + ddlYear.ClientID + "," + ddlType.ClientID + "," + txtEditPercentage.ClientID + ");"); // Attribute added for update using javascript validation
                //btnUpdateEducation.Attributes.Add("onClick", "return validateEducation(" + ddlDegree.ClientID + " , " + ddlCourse.ClientID + "," + txtEditSpecialization.ClientID + "," + txtEditInstitute.ClientID + "," + txtEditUniversity.ClientID + "," + ddlYear.ClientID + "," + ddlType.ClientID + "," + txtEditPercentage.ClientID + ");"); // Attribute added for update using javascript validation
                btnUpdateEducation.Attributes.Add("onClick", "return validateEducation(" + ddlDegree.ClientID + " , " + ddlCourse.ClientID + "," + txtEditSpecialization.ClientID + "," + txtEditUniversity.ClientID + "," + ddlYear.ClientID + "," + ddlType.ClientID + "," + txtEditPercentage.ClientID + ");"); // Attribute added for update using javascript validation
        }

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
        {
            Label lblDegree2 = e.Row.FindControl("lblDegree2") as Label;
            //Label lblPGUG2 = e.Row.FindControl("lblPGUG2") as Label;
            Label lblType2 = e.Row.FindControl("lblType2") as Label;
            Label lblYear2 = e.Row.FindControl("lblYear2") as Label;
            Label lblCourse2 = e.Row.FindControl("lblCourse2") as Label;
            //Label lblEstatblishment2 = e.Row.FindControl("lblEstatblishment2") as Label;

            DropDownList ddlDegree = e.Row.FindControl("ddlDegree") as DropDownList;
            //DropDownList ddlPGUG = e.Row.FindControl("ddlPGUG") as DropDownList;
            DropDownList ddlYear = e.Row.FindControl("ddlYear") as DropDownList;
            DropDownList ddlType = e.Row.FindControl("ddlType") as DropDownList;
            DropDownList ddlCourse = e.Row.FindControl("ddlCourse") as DropDownList;
            //DropDownList ddlEstatblishment = e.Row.FindControl("ddlEstatblishment") as DropDownList;

            if (lblType2 != null && lblYear2 != null && lblDegree2 != null && lblCourse2 != null && lblDegree2 != null)
            {
                ddlDegree.SelectedIndex = ddlDegree.Items.IndexOf(ddlDegree.Items.FindByValue(lblDegree2.Text));
                ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue(lblType2.Text));
                ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByText(lblYear2.Text));
                //ddlPGUG.SelectedIndex = ddlPGUG.Items.IndexOf(ddlPGUG.Items.FindByText(lblPGUG2.Text));
                //ddlPGUG_SelectedIndexChanged(ddlPGUG, null);
                ddlCourse.SelectedIndex = ddlCourse.Items.IndexOf(ddlCourse.Items.FindByValue(lblCourse2.Text));
                //ddlEstatblishment.SelectedIndex = ddlEstatblishment.Items.IndexOf(ddlEstatblishment.Items.FindByValue(lblEstatblishment2.Text));
            }
        }
    }

    protected void grdEducationDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdEducationDetails.EditIndex = e.NewEditIndex;
        grdEducationDetails.DataSource = ListOfActiveEducationRecords;
        grdEducationDetails.DataBind();
        //btnRemove1.Visible = true;
        grdEducationDetails.FooterRow.Visible = true;
    }

    protected void grdEducationDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void grdEducationDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Add")
        {
            EducationIndex += 1;

            DropDownList ddlFooterDegree = grdEducationDetails.FooterRow.FindControl("ddlFooterDegree") as DropDownList;
            //DropDownList ddlFooterPGUG = grdEducationDetails.FooterRow.FindControl("ddlFooterPGUG") as DropDownList;
            DropDownList ddlFooterCourse = grdEducationDetails.FooterRow.FindControl("ddlFooterCourse") as DropDownList;
            DropDownList ddlFooterYear = grdEducationDetails.FooterRow.FindControl("ddlFooterYear") as DropDownList;
            DropDownList ddlFooterType = grdEducationDetails.FooterRow.FindControl("ddlFooterType") as DropDownList;
            //DropDownList ddlFooterEstablishment = grdEducationDetails.FooterRow.FindControl("ddlFooterEstablishment") as DropDownList;

            TextBox txtFooterSpecialization = grdEducationDetails.FooterRow.FindControl("txtFooterSpecialization") as TextBox;
            //TextBox txtFooterEstablishment = grdEducationDetails.FooterRow.FindControl("txtFooterEstablishment") as TextBox;
            //TextBox txtFooterInstitute = grdEducationDetails.FooterRow.FindControl("txtFooterInstitute") as TextBox;
            TextBox txtFooterUniversity = grdEducationDetails.FooterRow.FindControl("txtFooterUniversity") as TextBox;
            TextBox txtFooterPercentage = grdEducationDetails.FooterRow.FindControl("txtFooterPercentage") as TextBox;

            listOfActiveEducationRecords = ListOfActiveEducationRecords;
            int count = ListOfActiveEducationRecords.Count;

            string percentage = "";

            if (!string.IsNullOrEmpty(txtFooterPercentage.Text))
                percentage = txtFooterPercentage.Text;

            listOfActiveEducationRecords.Add(new CandidateBOL()
            {
                EducationID = EducationIndex,
                //EducationID = count + 1,
                Degree = Convert.ToInt32(ddlFooterDegree.SelectedItem.Value),
                //PGUG = ddlFooterPGUG.SelectedItem.Text,
                Course = Convert.ToInt32(ddlFooterCourse.SelectedItem.Value),
                Specialization = txtFooterSpecialization.Text,
                // Establishment = txtFooterEstablishment.Text,
                //Establishment = Convert.ToInt32(ddlFooterEstablishment.SelectedItem.Value),
                University = txtFooterUniversity.Text,
                //Institute = txtFooterInstitute.Text,
                Year = Convert.ToInt32(ddlFooterYear.SelectedItem.Text),
                Type = Convert.ToInt32(ddlFooterType.SelectedItem.Value),
                Percentage = percentage,
            });

            listOfInsertedEducationRecords.Add(new CandidateBOL()
            {
                EducationID = EducationIndex,
                //EducationID = count + 1,
                Degree = Convert.ToInt32(ddlFooterDegree.SelectedItem.Value),
                //PGUG = ddlFooterPGUG.SelectedItem.Text,
                Course = Convert.ToInt32(ddlFooterCourse.SelectedItem.Value),
                Specialization = txtFooterSpecialization.Text,
                //Establishment = Convert.ToInt32(ddlFooterEstablishment.SelectedItem.Value),
                University = txtFooterUniversity.Text,
                //Institute = txtFooterInstitute.Text,
                Year = Convert.ToInt32(ddlFooterYear.SelectedItem.Text),
                Type = Convert.ToInt32(ddlFooterType.SelectedItem.Value),
                Percentage = percentage,
            });

            ListOfActiveEducationRecords = listOfActiveEducationRecords;
            ListOfInsertedEducationRecords = listOfInsertedEducationRecords;

            if (ListOfDeletedEducationRecords != null && ListOfDeletedEducationRecords.Count > 0)
            {
                List<CandidateBOL> tempDelete = new List<CandidateBOL>();

                foreach (CandidateBOL emp in ListOfDeletedEducationRecords)
                {
                    if (emp.ExpID == EducationIndex)
                    {
                        tempDelete.Add(emp);
                    }
                }

                if (tempDelete != null)
                {
                    foreach (CandidateBOL emp in tempDelete)
                    {
                        if (ListOfDeletedEducationRecords.Contains(emp))
                            ListOfDeletedEducationRecords.Remove(emp);
                    }
                }
            }

            grdEducationDetails.DataSource = ListOfActiveEducationRecords;
            grdEducationDetails.DataBind();
        }
        else if (e.CommandName == "Delete")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdEducationDetails.Rows[index];

            Label lblEducationSrNo = row.FindControl("lblEducationSrNo") as Label;
            Label lblEducationID = row.FindControl("lblEducationID") as Label;

            if (ListOfActiveEducationRecords.Count > 0)  //Checks whether list contains items
            {
                CandidateBOL itemToBeDeleted = ListOfActiveEducationRecords.Where(l => l.EducationID == Convert.ToInt32(lblEducationID.Text)).ToList<CandidateBOL>().SingleOrDefault(); // item which is to be deleted

                if (itemToBeDeleted != null)
                {
                    ListOfActiveEducationRecords.Remove(itemToBeDeleted);

                    //ListOfActiveRecords = listOfActiveRecords;
                }
            }

            if (listOfInsertedEducationRecords.Count > 0)  //Checks whether list contains items
            {
                CandidateBOL itemToBeDeleted = listOfInsertedEducationRecords.Where(l => l.EducationID == Convert.ToInt32(lblEducationID.Text)).ToList<CandidateBOL>().SingleOrDefault(); // item which is to be deleted

                if (itemToBeDeleted != null)
                {
                    listOfInsertedEducationRecords.Remove(itemToBeDeleted);

                    ListOfInsertedEducationRecords = listOfInsertedEducationRecords;
                }
            }

            if (listOfUpdatedEducationRecords.Count > 0)  //Checks whether list contains items
            {
                CandidateBOL itemToBeDeleted = listOfUpdatedEducationRecords.Where(l => l.EducationID == Convert.ToInt32(lblEducationID.Text)).ToList<CandidateBOL>().SingleOrDefault(); // item which is to be deleted

                if (itemToBeDeleted != null)
                {
                    listOfUpdatedEducationRecords.Remove(itemToBeDeleted);
                    grdEducationDetails.DataSource = null;
                    grdEducationDetails.DataBind();
                    ListOfUpdatedEducationRecords = listOfUpdatedEducationRecords;
                }
            }

            listOfDeletedEducationRecords.Add(new CandidateBOL() { EducationID = Convert.ToInt32(lblEducationID.Text) });

            ListOfDeletedEducationRecords = listOfDeletedEducationRecords;

            if (ListOfActiveEducationRecords != null && ListOfActiveEducationRecords.Count > 0)
            {
                grdEducationDetails.DataSource = ListOfActiveEducationRecords;
                grdEducationDetails.DataBind();
            }
            else if (dsEducation.Tables[0].Rows.Count == 1)
            {
                BindBlankGridForEducationDetails();
            }
            else
            {
                BindBlankGridForEducationDetails();
            }
        }
        ScriptManager.RegisterStartupScript(this, typeof(string), "", "javascript:ApplyClass();", true);
    }

    protected void grdEducationDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Label lblEducationID = grdEducationDetails.Rows[e.RowIndex].FindControl("lblEducationID") as Label;

        DropDownList ddlDegree = grdEducationDetails.Rows[e.RowIndex].FindControl("ddlDegree") as DropDownList;
        DropDownList ddlCourse = grdEducationDetails.Rows[e.RowIndex].FindControl("ddlCourse") as DropDownList;
        DropDownList ddlYear = grdEducationDetails.Rows[e.RowIndex].FindControl("ddlYear") as DropDownList;
        DropDownList ddlType = grdEducationDetails.Rows[e.RowIndex].FindControl("ddlType") as DropDownList;

        TextBox txtSpecialization = grdEducationDetails.Rows[e.RowIndex].FindControl("txtEditSpecialization") as TextBox;
        TextBox txtUniversity = grdEducationDetails.Rows[e.RowIndex].FindControl("txtEditUniversity") as TextBox;
        //TextBox txtInstitute = grdEducationDetails.Rows[e.RowIndex].FindControl("txtEditInstitute") as TextBox;
        TextBox txtPercentage = grdEducationDetails.Rows[e.RowIndex].FindControl("txtEditPercentage") as TextBox;

        string percentage = "";

        if (!string.IsNullOrEmpty(txtPercentage.Text))
            percentage = txtPercentage.Text;

        if (ListOfActiveEducationRecords.Count > 0) //Checks whether list contains items
        {
            foreach (CandidateBOL emp in ListOfActiveEducationRecords)
            {
                if (emp.EducationID == Convert.ToInt32(lblEducationID.Text))
                {
                    emp.Degree = Convert.ToInt32(ddlDegree.SelectedItem.Value);
                    emp.Course = Convert.ToInt32(ddlCourse.SelectedItem.Value);
                    emp.Year = Convert.ToInt32(ddlYear.SelectedItem.Text);
                    emp.Type = Convert.ToInt32(ddlType.SelectedItem.Value);
                    emp.Specialization = txtSpecialization.Text;
                    emp.University = txtUniversity.Text;
                    //emp.Institute = txtInstitute.Text;
                    emp.Percentage = percentage;
                }
            }
        }

        bool insertedItem = false;

        if (listOfInsertedEducationRecords.Count > 0) //Checks whether list contains items
        {
            foreach (CandidateBOL emp in listOfInsertedEducationRecords)
            {
                if (emp.EducationID == Convert.ToInt32(lblEducationID.Text))
                {
                    insertedItem = true;
                    emp.Degree = Convert.ToInt32(ddlDegree.SelectedItem.Value);
                    emp.Course = Convert.ToInt32(ddlCourse.SelectedItem.Value);
                    emp.Year = Convert.ToInt32(ddlYear.SelectedItem.Text);
                    emp.Type = Convert.ToInt32(ddlType.SelectedItem.Value);
                    emp.Specialization = txtSpecialization.Text;
                    emp.University = txtUniversity.Text;
                    //emp.Institute = txtInstitute.Text;
                    emp.Percentage = percentage;
                }
            }
            ListOfInsertedEducationRecords = listOfInsertedEducationRecords;
        }

        bool isUpdateItemAdded = false;

        if (!insertedItem && (listOfUpdatedEducationRecords.Count > 0))
        {
            foreach (CandidateBOL emp in listOfUpdatedEducationRecords)
            {
                if (emp.EducationID == Convert.ToInt32(lblEducationID.Text))
                {
                    emp.Degree = Convert.ToInt32(ddlDegree.SelectedItem.Value);
                    emp.Course = Convert.ToInt32(ddlCourse.SelectedItem.Value);
                    emp.Year = Convert.ToInt32(ddlYear.SelectedItem.Text);
                    emp.Type = Convert.ToInt32(ddlType.SelectedItem.Value);
                    emp.Specialization = txtSpecialization.Text;
                    emp.University = txtUniversity.Text;
                    //emp.Institute = txtInstitute.Text;
                    emp.Percentage = percentage;
                    isUpdateItemAdded = true;
                }
            }
            ListOfUpdatedEducationRecords = listOfUpdatedEducationRecords;
        }

        if (!insertedItem && !isUpdateItemAdded)
        {
            listOfUpdatedEducationRecords.Add(new CandidateBOL()
            {
                EducationID = Convert.ToInt32(lblEducationID.Text),
                Degree = Convert.ToInt32(ddlDegree.SelectedItem.Value),
                Course = Convert.ToInt32(ddlCourse.SelectedItem.Value),
                Year = Convert.ToInt32(ddlYear.SelectedItem.Text),
                Type = Convert.ToInt32(ddlType.SelectedItem.Value),
                Specialization = txtSpecialization.Text,
                University = txtUniversity.Text,
                //Institute = txtInstitute.Text,
                Percentage = percentage
            });
            ListOfUpdatedEducationRecords = listOfUpdatedEducationRecords;
        }

        grdEducationDetails.DataSource = ListOfActiveEducationRecords;
        grdEducationDetails.DataBind();
        grdEducationDetails.FooterRow.Visible = true;

        // Code to solve row updating issue

        grdEducationDetails.EditIndex = -1;
        grdEducationDetails.DataSource = ListOfActiveEducationRecords;
        grdEducationDetails.DataBind();
        grdEducationDetails.FooterRow.Visible = true;
    }

    protected void grdEducationDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdEducationDetails.EditIndex = -1;
        grdEducationDetails.DataSource = ListOfActiveEducationRecords;
        grdEducationDetails.DataBind();
        //btnRemove1.Visible = true;
        grdEducationDetails.FooterRow.Visible = true;
    }

    protected void txtFirstName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Save")
            {
                if (string.IsNullOrEmpty(txtFirstName.Text))
                    txtFirstName.Focus();
                else
                    txtMiddleName.Focus();

                if (!string.IsNullOrEmpty(txtFirstName.Text) && !string.IsNullOrEmpty(txtLastName.Text) && !string.IsNullOrEmpty(txtDateOfBirth.Text))
                {
                    dsTemp = GetCandidateDetailsByFirstNameAndLastNameAndDOB(txtFirstName.Text, txtLastName.Text, txtDateOfBirth.Text);

                    if (dsTemp.Tables[0].Rows.Count == 1)  // Always count == 1 as it is unique record and should be one
                    {
                        ViewState["mode"] = "Edit";
                        this.ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript: show_confirm(); ", true);
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    protected void txtDateOfBirth_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Save")
            {
                if (string.IsNullOrEmpty(txtDateOfBirth.Text))
                    txtDateOfBirth.Focus();
                else
                    ddlMaritalStatus.Focus();

                if (!string.IsNullOrEmpty(txtFirstName.Text) && !string.IsNullOrEmpty(txtLastName.Text) && !string.IsNullOrEmpty(txtDateOfBirth.Text))
                {
                    dsTemp = GetCandidateDetailsByFirstNameAndLastNameAndDOB(txtFirstName.Text, txtLastName.Text, txtDateOfBirth.Text);

                    if (dsTemp.Tables[0].Rows.Count == 1)  // Always count == 1 as it is unique record and should be one
                    {
                        ViewState["mode"] = "Edit";
                        this.ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript: show_confirm(); ", true);
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    protected void txtLastName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Save")
            {
                if (string.IsNullOrEmpty(txtLastName.Text))
                    txtLastName.Focus();
                else
                    txtDateOfBirth.Focus();

                if (!string.IsNullOrEmpty(txtFirstName.Text) && !string.IsNullOrEmpty(txtLastName.Text) && !string.IsNullOrEmpty(txtDateOfBirth.Text))
                {
                    dsTemp = GetCandidateDetailsByFirstNameAndLastNameAndDOB(txtFirstName.Text, txtLastName.Text, txtDateOfBirth.Text);

                    if (dsTemp.Tables[0].Rows.Count == 1)  // Always count == 1 as it is unique record and should be one
                    {
                        ViewState["mode"] = "Edit";
                        this.ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript: show_confirm(); ", true);
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    protected void btnAlert_Click(object sender, EventArgs e)
    {
        try
        {
            candidateID = dsTemp.Tables[0].Rows[0]["ID"].ToString();
            GetCandidateDetailsWithoutCheckBox(candidateID);

            candidateBOL.CandidateID = Convert.ToInt32(candidateID);
            dsCandidate = candidateBLL.GetCandidateSkills(candidateBOL);

            chkList.ClearSelection();
            foreach (DataRow row in dsCandidate.Tables[0].Rows)
            {
                chkList.Items.FindByValue(row["ID"].ToString()).Selected = true;
            }

            // New code for filling viestate for Experience,education and certification
            // candidateBOL.CandidateID = Convert.ToInt32(candidateID);

            if (ListOfActiveExperienceRecords.Count == 0)
            {
                dsExperience = candidateBLL.GetExperienceDetails(candidateBOL);
                ListOfActiveExperienceRecords = GetExperienceRecordsInList(dsExperience);
            }

            if (ListOfActiveEducationRecords.Count == 0)
            {
                dsEducation = candidateBLL.GetEducationDetails(candidateBOL);
                ListOfActiveEducationRecords = GetEducationRecordsInList(dsEducation);
            }

            if (ListOfActiveCertificationRecords.Count == 0)
            {
                dsCertification = candidateBLL.GetCertificationDetails(candidateBOL);
                ListOfActiveCertificationRecords = GetCertificationRecordsInList(dsCertification);
            }
            ViewState["DuplicateEntryLoaded"] = true;
            btnSave.Text = "Update";
            btnSaveAndAddMore.Visible = false;
            ddlCandidateStatus.Enabled = true;
            lblCandidateIDDisplay.Visible = true;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    public void DisableControls()
    {
        if (Session["viewProfileClick"] == "true")
        {
            txtFirstName.Enabled = false;
        }
    }

    protected void btnAlertCancel_Click(object sender, EventArgs e)
    {
        try
        {
            string modeForSkills = "Add";
            chkList.Items.Clear();
            List<KeyValuePair<int, string>> listOfSkills = GetAllSkills(modeForSkills);
            AddCheckBoxesToCheckBoxList(listOfSkills);

            ClearAllControls();
            ClearGridViews();

            ddlSalutation.Focus();
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    protected void btnRedirect_Click(object sender, EventArgs e)
    {
        ClearAllControls();
        ClearGridViews();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("CandidateDataBank.aspx", false);
    }

    protected void btnSaveAndAddMoreRedirect_Click(object sender, EventArgs e)
    {
        ClearAllControls();
        ClearGridViews();
        ddlSalutation.Focus();
    }

    protected void ddlSalutation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(ddlSalutation.SelectedItem.Text))
            {
                if (ddlSalutation.SelectedItem.Text == "Mr.")
                {
                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByText("Male"));
                }
                else if (ddlSalutation.SelectedItem.Text == "Ms.")
                {
                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByText("Female"));
                }
                else if (ddlSalutation.SelectedItem.Text == "Mrs.")
                {
                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByText("Female"));
                }
            }
            txtFirstName.Focus();
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    protected void ddlGender_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(ddlGender.SelectedItem.Text))
            {
                if (ddlGender.SelectedItem.Text == "Male")
                {
                    ddlSalutation.SelectedIndex = ddlSalutation.Items.IndexOf(ddlSalutation.Items.FindByText("Mr."));
                }
                else if (ddlGender.SelectedItem.Text == "Female")
                {
                    ddlSalutation.SelectedIndex = ddlSalutation.Items.IndexOf(ddlSalutation.Items.FindByText("Ms."));
                }
            }
            txtAlternateContactNumber.Focus();
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    #endregion

    #region custom methods

    #region Backup

    private void GetAllMaritalStatus()
    {
        try
        {
            Dictionary<int, string> dictmaritalStatus = new Dictionary<int, string>();

            dictmaritalStatus.Add(0, "select");
            dictmaritalStatus.Add(1, "Single");
            dictmaritalStatus.Add(2, "Married");
            ddlMaritalStatus.DataSource = dictmaritalStatus;
            ddlMaritalStatus.DataBind();
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    private void GetAllSalutations()
    {
        try
        {
            Dictionary<int, string> dictSalutations = new Dictionary<int, string>();
            dictSalutations.Add(0, "select");
            dictSalutations.Add(1, "Mr.");
            dictSalutations.Add(2, "Mrs.");
            ddlSalutation.DataSource = dictSalutations;
            ddlSalutation.DataBind();
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    private void GetGenders()
    {
        try
        {
            Dictionary<int, string> dictGenders = new Dictionary<int, string>();
            dictGenders.Add(0, "select");
            dictGenders.Add(1, "Male");
            dictGenders.Add(2, "Female");
            ddlGender.DataSource = dictGenders;
            ddlGender.DataBind();
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    private void GetAllSource()
    {
        try
        {
            Dictionary<int, string> dictSources = new Dictionary<int, string>();
            dictSources.Add(0, "select");
            dictSources.Add(1, "Website");
            dictSources.Add(2, "Agency");
            dictSources.Add(3, "Walk-in");
            dictSources.Add(4, "Advertisement");
            dictSources.Add(5, "Referral");
            dictSources.Add(6, "V2 Online career section");
            ddlSource.DataSource = dictSources;
            ddlSource.DataBind();
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    #endregion

    /// <summary>
    /// Gets Records for Experience in List
    /// </summary>
    /// <param name="dataSet"></param>
    /// <returns></returns>
    private List<CandidateBOL> GetExperienceRecordsInList(DataSet dataSet)
    {
        try
        {
            List<CandidateBOL> listRecords = new List<CandidateBOL>();

            if (dataSet.Tables.Count > 0)
            {
                DataTable table = dataSet.Tables[0];

                foreach (DataRow record in table.Rows)
                {
                    if (record["WorkedTill"].ToString() != "")
                    {
                        listRecords.Add(new CandidateBOL()
                        {
                            ExpID = Convert.ToInt32(record["ExpID"]),
                            OrganisationName = Convert.ToString(record["OrganisationName"]),
                            //Location = Convert.ToString(record["Location"]),
                            WorkedFrom = Convert.ToDateTime(record["WorkedFrom"]),
                            WorkedTill = Convert.ToDateTime(record["WorkedTill"]),
                            PositionHeld = Convert.ToString(record["PositionHeld"]),
                            ReportingManager = Convert.ToString(record["ReportingManager"]),
                            ExpType = Convert.ToInt32(record["ExpType"]),
                            CTC = record["CTC"].ToString()
                        });
                    }
                    else
                    {
                        listRecords.Add(new CandidateBOL()
                        {
                            ExpID = Convert.ToInt32(record["ExpID"]),
                            OrganisationName = Convert.ToString(record["OrganisationName"]),
                            //Location = Convert.ToString(record["Location"]),
                            WorkedFrom = Convert.ToDateTime(record["WorkedFrom"]),
                            //WorkedTill = Convert.ToDateTime(null),
                            PositionHeld = Convert.ToString(record["PositionHeld"]),
                            ReportingManager = Convert.ToString(record["ReportingManager"]),
                            ExpType = Convert.ToInt32(record["ExpType"]),
                            CTC = record["CTC"].ToString()
                        });
                    }
                }
            }
            return listRecords;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets Records for Experience in List
    /// </summary>
    /// <param name="dataSet"></param>
    /// <returns></returns>
    private List<CandidateBOL> GetExperienceRecordsInList(DataTable dataTable)
    {
        try
        {
            List<CandidateBOL> listRecords = new List<CandidateBOL>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow record in dataTable.Rows)
                {
                    if (record["WorkedTill"].ToString() != "")
                    {
                        listRecords.Add(new CandidateBOL()
                        {
                            ExpID = Convert.ToInt32(record["ExpID"]),
                            OrganisationName = Convert.ToString(record["OrganisationName"]),
                            //OrganisationName = Convert.ToInt32(record["OrganisationName"]),
                            //Location = Convert.ToString(record["Location"]),
                            WorkedFrom = Convert.ToDateTime(record["WorkedFrom"]),
                            WorkedTill = Convert.ToDateTime(record["WorkedTill"]),
                            ExpType = Convert.ToInt32(record["ExpType"]),
                            PositionHeld = Convert.ToString(record["PositionHeld"]),
                            ReportingManager = Convert.ToString(record["ReportingManager"]),
                            CTC = record["CTC"].ToString()
                        });
                    }
                    else
                    {
                        listRecords.Add(new CandidateBOL()
                        {
                            ExpID = Convert.ToInt32(record["ExpID"]),
                            OrganisationName = Convert.ToString(record["OrganisationName"]),
                            //OrganisationName = Convert.ToInt32(record["OrganisationName"]),
                            //Location = Convert.ToString(record["Location"]),
                            WorkedFrom = Convert.ToDateTime(record["WorkedFrom"]),
                            //WorkedTill = Convert.ToDateTime(null),
                            ExpType = Convert.ToInt32(record["ExpType"]),
                            PositionHeld = Convert.ToString(record["PositionHeld"]),
                            ReportingManager = Convert.ToString(record["ReportingManager"]),
                            CTC = record["CTC"].ToString()
                        });
                    }
                }
            }
            return listRecords;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets Records for Education in List
    /// </summary>
    /// <param name="dataSet"></param>
    /// <returns></returns>
    private List<CandidateBOL> GetEducationRecordsInList(DataSet dataSet)
    {
        try
        {
            List<CandidateBOL> listRecords = new List<CandidateBOL>();

            if (dataSet.Tables.Count > 0)
            {
                DataTable table = dataSet.Tables[0];

                foreach (DataRow record in table.Rows)
                {
                    listRecords.Add(new CandidateBOL()
                    {
                        EducationID = Convert.ToInt32(record["EducationID"]),
                        Degree = Convert.ToInt32(record["Degree"]),
                        Specialization = Convert.ToString(record["Specialization"]),
                        University = Convert.ToString(record["University"]),
                        //Institute = Convert.ToString(record["Institute"]),
                        Course = Convert.ToInt32(record["Course"]),
                        Year = Convert.ToInt32(record["Year"]),
                        Type = Convert.ToInt32(record["Type"]),
                        Percentage = record["Percentage"].ToString()
                    });
                }
            }
            return listRecords;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets Records for Education in List
    /// </summary>
    /// <param name="dataSet"></param>
    /// <returns></returns>
    private List<CandidateBOL> GetEducationRecordsInList(DataTable dataTable)
    {
        try
        {
            List<CandidateBOL> listRecords = new List<CandidateBOL>();

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow record in dataTable.Rows)
                {
                    listRecords.Add(new CandidateBOL()
                    {
                        EducationID = Convert.ToInt32(record["EducationID"]),
                        Degree = Convert.ToInt32(record["Degree"]),
                        //PGUG = Convert.ToString(record["PGUG"]),
                        Specialization = Convert.ToString(record["Specialization"]),
                        //Establishment = Convert.ToInt32(record["Establishment"]),
                        University = Convert.ToString(record["University"]),
                        //Institute = Convert.ToString(record["Institute"]),
                        Course = Convert.ToInt32(record["Course"]),
                        Year = Convert.ToInt32(record["Year"]),
                        Type = Convert.ToInt32(record["Type"]),
                        Percentage = record["Percentage"].ToString()
                    });
                }
            }
            return listRecords;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets Records for Certification in List
    /// </summary>
    /// <param name="dataSet"></param>
    /// <returns></returns>
    private List<CandidateBOL> GetCertificationRecordsInList(DataSet dataSet)
    {
        try
        {
            List<CandidateBOL> listRecords = new List<CandidateBOL>();

            if (dataSet.Tables.Count > 0)
            {
                DataTable table = dataSet.Tables[0];

                foreach (DataRow record in table.Rows)
                {
                    listRecords.Add(new CandidateBOL()
                    {
                        CertificationID = Convert.ToInt32(record["CertificationID"]),
                        CertificationName = Convert.ToInt32(record["CertificationName"]),
                        CertificationNo = Convert.ToString(record["CertificationNo"]),
                        Institution = Convert.ToString(record["Institution"]),
                        CertificationDate = Convert.ToDateTime(record["CertificationDate"]),
                        CertificationGrade = Convert.ToString(record["CertificationGrade"]),
                        CertificationScore = Convert.ToString(record["CertificationScore"])
                    });
                }
            }
            return listRecords;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets Records for Certification in List
    /// </summary>
    /// <param name="dataSet"></param>
    /// <returns></returns>
    private List<CandidateBOL> GetCertificationRecordsInList(DataTable dataTable)
    {
        try
        {
            List<CandidateBOL> listRecords = new List<CandidateBOL>();

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow record in dataTable.Rows)
                {
                    listRecords.Add(new CandidateBOL()
                    {
                        CertificationID = Convert.ToInt32(record["CertificationID"]),
                        CertificationName = Convert.ToInt32(record["CertificationName"]),
                        CertificationNo = Convert.ToString(record["CertificationNo"]),
                        Institution = Convert.ToString(record["Institution"]),
                        CertificationDate = Convert.ToDateTime(record["CertificationDate"]),
                        CertificationGrade = Convert.ToString(record["CertificationGrade"]),
                        CertificationScore = Convert.ToString(record["CertificationScore"])
                    });
                }
            }
            return listRecords;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Adds CheckBoxes To CheckBoxList
    /// </summary>
    /// <param name="listOfSkills"></param>
    private void AddCheckBoxesToCheckBoxList(List<KeyValuePair<int, string>> listOfSkills)
    {
        try
        {
            foreach (KeyValuePair<int, string> item in listOfSkills)
            {
                chkList.Items.Add(new ListItem() { Text = item.Value, Value = Convert.ToString(item.Key) });
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Binds DropDownList with Values from DataBase
    /// </summary>
    /// <param name="dropDownList"></param>
    /// <param name="listOfCandidateStatus"></param>
    private void BindDropDownListWithValuesFromDB(DropDownList dropDownList, List<KeyValuePair<int, string>> listOfCandidateStatus)
    {
        try
        {
            dropDownList.DataSource = listOfCandidateStatus;
            dropDownList.DataTextField = "Value";
            dropDownList.DataValueField = "Key";
            dropDownList.DataBind();

            ListItem item = new ListItem() { Text = "select", Value = "none" };
            dropDownList.Items.Insert(0, item);
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets All Skills
    /// </summary>
    /// <returns></returns>
    private List<KeyValuePair<int, string>> GetAllSkills(string mode)
    {
        try
        {
            List<KeyValuePair<int, string>> listOfSkills = new List<KeyValuePair<int, string>>();

            dsSkills = candidateBLL.GetAllSkills(mode);

            foreach (DataRow course in dsSkills.Tables[0].Rows)
            {
                listOfSkills.Add(new KeyValuePair<int, string>(Convert.ToInt32(course[0].ToString()), Convert.ToString(course[1])));
            }
            return listOfSkills;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Generates List of Years and Months
    /// </summary>
    /// <returns></returns>
    private static List<int> GetYearsAndMonths(int min, int max)
    {
        try
        {
            List<int> listOfYears = new List<int>();

            for (int i = min; i <= max; i++)
            //for (int i = 1950; i <= 2200; i++)
            {
                listOfYears.Add(i);
            }
            return listOfYears;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Binds DropDownList
    /// </summary>
    /// <param name="dropDownList"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    private static void BindDropDownList(DropDownList dropDownList, int min, int max)
    {
        try
        {
            dropDownList.DataSource = GetYearsAndMonths(min, max);
            dropDownList.DataBind();
            //ListItem item = new ListItem() { Text = "select", Value = "none" };
            //dropDownList.Items.Insert(0, item);
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets All Courses
    /// </summary>
    /// <returns></returns>
    private List<KeyValuePair<int, string>> GetAllCourses()
    {
        try
        {
            List<KeyValuePair<int, string>> listOfCourses = new List<KeyValuePair<int, string>>();

            dsCourses = candidateBLL.GetAllCourses();

            foreach (DataRow course in dsCourses.Tables[0].Rows)
            {
                listOfCourses.Add(new KeyValuePair<int, string>(Convert.ToInt32(course[0].ToString()), Convert.ToString(course[1])));
            }
            return listOfCourses;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    private List<KeyValuePair<int, string>> GetAllCountryNames()
    {
        try
        {
            List<KeyValuePair<int, string>> listOfCountryNames = new List<KeyValuePair<int, string>>();

            dsCountry = candidateBLL.GetAllCountryNames();

            foreach (DataRow country in dsCountry.Tables[0].Rows)
            {
                listOfCountryNames.Add(new KeyValuePair<int, string>(Convert.ToInt32(country[0].ToString()), Convert.ToString(country[1])));
            }
            return listOfCountryNames;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets All Degree Names
    /// </summary>
    /// <returns></returns>
    private List<KeyValuePair<int, string>> GetAllDegree()
    {
        try
        {
            List<KeyValuePair<int, string>> listOfDegree = new List<KeyValuePair<int, string>>();

            dsDegree = candidateBLL.GetAllDegree();

            foreach (DataRow Degree in dsDegree.Tables[0].Rows)
            {
                listOfDegree.Add(new KeyValuePair<int, string>(Convert.ToInt32(Degree[0].ToString()), Convert.ToString(Degree[1])));
            }
            return listOfDegree;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    private List<KeyValuePair<int, string>> GetAllCertificationNames()
    {
        try
        {
            List<KeyValuePair<int, string>> listOfCertificationName = new List<KeyValuePair<int, string>>();

            dsCertificationName = candidateBLL.GetAllCertificationNames();

            foreach (DataRow CertificationName in dsCertificationName.Tables[0].Rows)
            {
                listOfCertificationName.Add(new KeyValuePair<int, string>(Convert.ToInt32(CertificationName[0].ToString()), Convert.ToString(CertificationName[1])));
            }
            return listOfCertificationName;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets All Establishment
    /// </summary>
    /// <returns></returns>
    private List<KeyValuePair<int, string>> GetAllOrganization()
    {
        try
        {
            List<KeyValuePair<int, string>> listOfOrganization = new List<KeyValuePair<int, string>>();

            dsOrganization = candidateBLL.GetAllOrganization();

            foreach (DataRow Organization in dsOrganization.Tables[0].Rows)
            {
                listOfOrganization.Add(new KeyValuePair<int, string>(Convert.ToInt32(Organization[0].ToString()), Convert.ToString(Organization[1])));
            }
            return listOfOrganization;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets Course By ID
    /// </summary>
    /// <returns></returns>
    private string GetOrganizationID(int organizationID)
    {
        try
        {
            string Organization = string.Empty;

            dsOrganization = candidateBLL.GetAllOrganization();

            foreach (DataRow row in dsOrganization.Tables[0].Rows)
            {
                if (Convert.ToInt32(row[0].ToString()) == organizationID)
                    Organization = Convert.ToString(row[1]);
                // listOfCourses.Add(new KeyValuePair<int, string>(Convert.ToInt32(row[0].ToString()), Convert.ToString(row[1])));
            }
            return Organization;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets Course By ID
    /// </summary>
    /// <returns></returns>
    private string GetCourseByID(int courseID)
    {
        try
        {
            string course = string.Empty;

            dsCourses = candidateBLL.GetAllCourses();

            foreach (DataRow row in dsCourses.Tables[0].Rows)
            {
                if (Convert.ToInt32(row[0].ToString()) == courseID)
                    course = Convert.ToString(row[1]);
                // listOfCourses.Add(new KeyValuePair<int, string>(Convert.ToInt32(row[0].ToString()), Convert.ToString(row[1])));
            }
            return course;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    private string GetCountryByID(int countryID)
    {
        try
        {
            string country = string.Empty;

            dsCountry = candidateBLL.GetAllCountryNames();

            foreach (DataRow row in dsCountry.Tables[0].Rows)
            {
                if (Convert.ToInt32(row[0].ToString()) == countryID)
                    country = Convert.ToString(row[1]);
            }
            return country;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    private string GetDegreeID(int degreeID)
    {
        try
        {
            string degree = string.Empty;

            dsDegree = candidateBLL.GetAllDegree();

            foreach (DataRow row in dsDegree.Tables[0].Rows)
            {
                if (Convert.ToInt32(row[0].ToString()) == degreeID)
                    degree = Convert.ToString(row[1]);
            }
            return degree;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    private string GetCertificationNameID(int certificationNameID)
    {
        try
        {
            string certificationName = string.Empty;

            dsCertificationName = candidateBLL.GetAllCertificationNames();

            foreach (DataRow row in dsCertificationName.Tables[0].Rows)
            {
                if (Convert.ToInt32(row[0].ToString()) == certificationNameID)
                    certificationName = Convert.ToString(row[1]);
            }
            return certificationName;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets All Course Types
    /// </summary>
    /// <returns></returns>
    private List<KeyValuePair<int, string>> GetAllCourseTypes()
    {
        try
        {
            List<KeyValuePair<int, string>> listOfCourseTypes = new List<KeyValuePair<int, string>>();

            dsCourseTypes = candidateBLL.GetAllCourseTypes();

            foreach (DataRow courseType in dsCourseTypes.Tables[0].Rows)
            {
                listOfCourseTypes.Add(new KeyValuePair<int, string>(Convert.ToInt32(courseType[0].ToString()), Convert.ToString(courseType[1])));
            }
            return listOfCourseTypes;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    private List<KeyValuePair<int, string>> GetAllExpTypes()
    {
        try
        {
            List<KeyValuePair<int, string>> listOfExpTypes = new List<KeyValuePair<int, string>>();

            dsExpTypes = candidateBLL.GetAllExpTypes();

            foreach (DataRow expType in dsExpTypes.Tables[0].Rows)
            {
                listOfExpTypes.Add(new KeyValuePair<int, string>(Convert.ToInt32(expType[0].ToString()), Convert.ToString(expType[1])));
            }
            return listOfExpTypes;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets Course Type By ID
    /// </summary>
    /// <returns></returns>
    private string GetCourseTypeByID(int courseTypeID)
    {
        try
        {
            string courseType = string.Empty;

            dsCourseTypes = candidateBLL.GetAllCourseTypes();

            foreach (DataRow row in dsCourseTypes.Tables[0].Rows)
            {
                if (Convert.ToInt32(row[0].ToString()) == courseTypeID)
                    courseType = Convert.ToString(row[1]);
                //listOfCourseTypes.Add(new KeyValuePair<int, string>(Convert.ToInt32(row[0].ToString()), Convert.ToString(row[1])));
            }
            return courseType;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    private string GetExpTypeByID(int expTypeID)
    {
        try
        {
            string expType = string.Empty;

            dsExpTypes = candidateBLL.GetAllExpTypes();

            foreach (DataRow row in dsExpTypes.Tables[0].Rows)
            {
                if (Convert.ToInt32(row[0].ToString()) == expTypeID)
                    expType = Convert.ToString(row[1]);
            }
            return expType;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets All Status
    /// </summary>
    /// <returns></returns>
    private List<KeyValuePair<int, string>> GetAllStatus()
    {
        try
        {
            List<KeyValuePair<int, string>> listOfCandidateStatus = new List<KeyValuePair<int, string>>();

            dsCandidateStatus = candidateBLL.GetAllStaus();

            foreach (DataRow courseType in dsCandidateStatus.Tables[0].Rows)
            {
                listOfCandidateStatus.Add(new KeyValuePair<int, string>(Convert.ToInt32(courseType[0].ToString()), Convert.ToString(courseType[1])));
            }
            return listOfCandidateStatus;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    private List<KeyValuePair<int, string>> GetHistory()
    {
        try
        {
            clsBLViewMyStatus objClsBLViewMyStatus = new clsBLViewMyStatus();
            List<KeyValuePair<int, string>> listOfCandidateHistory = new List<KeyValuePair<int, string>>();
            string name = "";
            dsCandidateHistory = candidateBLL.GetCandidateHistory(candidateBOL);
            addhistrory = candidateBLL.GetCandidateDetails(candidateBOL);
            DataSet dsEmpName = objClsBLViewMyStatus.GetEmployeeName(addhistrory.Tables[0].Rows[0]["CreatedBy"].ToString());
            if (dsEmpName.Tables[0].Rows.Count > 0)
            {
                name = dsEmpName.Tables[0].Rows[0]["EmployeeName"].ToString();
            }
            listOfCandidateHistory.Add(new KeyValuePair<int, string>(Convert.ToInt32(addhistrory.Tables[0].Rows[0]["ID"].ToString()), Convert.ToString(Convert.ToDateTime(addhistrory.Tables[0].Rows[0]["CreatedDate"]).ToString("dd/MM/yyyy") + " " + name + " " + "Candidate info Added")));

            foreach (DataRow courseType in dsCandidateHistory.Tables[0].Rows)
            {
                DateTime dt = Convert.ToDateTime(courseType[13]);
                listOfCandidateHistory.Add(new KeyValuePair<int, string>(Convert.ToInt32(courseType[0].ToString()), Convert.ToString(Convert.ToDateTime(courseType[13]).ToString("dd/MM/yyyy") + " " + (courseType[14]).ToString().Trim() + " " + courseType[6].ToString().Trim() + " " + courseType[8].ToString().Trim())));
            }
            return listOfCandidateHistory;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Adds Candidate
    /// </summary>
    private string AddCandidate()
    {
        try
        {
            DataSet dsTemp = new DataSet();
            // candidateBOL.CandidateID = Convert.ToInt32(lblCandidateID.Text);
            candidateBOL.HighestQualification = Convert.ToInt32(ddlHighestQualification.SelectedItem.Value);
            candidateBOL.CandidateStatus = Convert.ToInt32(ddlCandidateStatus.SelectedItem.Value);
            candidateBOL.Salutation = ddlSalutation.SelectedItem.Text;
            candidateBOL.FirstName = txtFirstName.Text;
            candidateBOL.Middlename = txtMiddleName.Text;
            candidateBOL.LastName = txtLastName.Text;
            candidateBOL.DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text, "MM/dd/yyyy", null);    //TO DO
            //candidateBOL.DateOfBirth = Convert.ToDateTime(txtDateOfBirth.Text);
            candidateBOL.MaritalStatus = ddlMaritalStatus.SelectedItem.Text;
            candidateBOL.Gender = ddlGender.SelectedItem.Text;
            candidateBOL.AlternateContactNumber = txtAlternateContactNumber.Text;
            candidateBOL.MobileNumber = txtMobileNumber.Text;
            candidateBOL.Email = txtEmailID.Text;
            candidateBOL.AlternateEmailID = txtAlternateEmailID.Text;

            candidateBOL.TotalWorkExperienceInYear = Convert.ToInt32(ddlTotalWorkExpYears.SelectedItem.Text);
            candidateBOL.TotalWorkExperienceInMonths = Convert.ToInt32(ddlTotalWorkExpMonths.SelectedItem.Text);
            candidateBOL.RelevantWorkExperienceInYear = Convert.ToInt32(ddlRelevantWorkExpYears.SelectedItem.Text);
            candidateBOL.RelevantWorkExperienceInMonths = Convert.ToInt32(ddlRelevantWorkExpMonths.SelectedItem.Text);

            candidateBOL.WorkExperienceInMonths = (Convert.ToInt32(ddlTotalWorkExpYears.SelectedItem.Text) * 12) + Convert.ToInt32(ddlTotalWorkExpMonths.SelectedItem.Text);
            candidateBOL.RelevantlWorkExperienceInMonths = (Convert.ToInt32(ddlRelevantWorkExpYears.SelectedItem.Text) * 12) + Convert.ToInt32(ddlRelevantWorkExpMonths.SelectedItem.Text);

            //if (optValidPassportYes.Checked)
            if (rdobtnValidPassport.Items[0].Selected == true)
                candidateBOL.ValidPassPort = true;
            else
                candidateBOL.ValidPassPort = false;

            //if (optUSVisaYes.Checked)
            if (rdobtnUSVisa.Items[0].Selected == true)
                candidateBOL.USVisa = true;
            else
                candidateBOL.USVisa = false;

            //if (optWillingToRelocateYes.Checked)
            if (rdobtnWillingToRelocate.Items[0].Selected == true)
                candidateBOL.WillingToRelocate = true;
            else
                candidateBOL.WillingToRelocate = false;

            candidateBOL.PresentAddress = txtPresentAddress.Text;
            candidateBOL.State = txtState.Text;
            candidateBOL.City = txtCity.Text;
            candidateBOL.Country = Convert.ToInt32(ddlCountry.SelectedItem.Value);
            candidateBOL.PinCode = txtPinCode.Text;

            if (!string.IsNullOrEmpty(txtCurrentNoticePeriod.Text))
                candidateBOL.CurrrentNoticePeriod = Convert.ToInt32(txtCurrentNoticePeriod.Text);
            else
                candidateBOL.CurrrentNoticePeriod = 0;

            if (!string.IsNullOrEmpty(txtCurrentCTC.Text))
                candidateBOL.CurrentCTC = Convert.ToDecimal(txtCurrentCTC.Text);
            else
                candidateBOL.CurrentCTC = 0;

            candidateBOL.AreasOfInterest = txtAreasOfInterest.Text;
            candidateBOL.Source = ddlSource.SelectedItem.Text;
            candidateBOL.SourceName = txtSourceName.Text;
            candidateBOL.CurrentJobSummary = txtCurrentJobSummary.Text;
            candidateBOL.RewardsAndRecognition = txtRewardsAndRecognition.Text;
            candidateBOL.SpecialAchievements = txtSpecialAchievements.Text;
            candidateBOL.OtherSkills = txtOtherSkills.Text;
            candidateBOL.CurrentRRF = 0;
            candidateBOL.CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            candidateBOL.CreatedDate = DateTime.Now;

            //  candidateBLL.AddCandidate(candidateBOL);

            dsTemp = candidateBLL.AddCandidate(candidateBOL);
            string candidateID = dsTemp.Tables[0].Rows[0]["ID"].ToString();
            return candidateID;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Updates Candidate
    /// </summary>
    private void UpdateCandidate()
    {
        try
        {
            DataSet dsTemp = new DataSet();
            candidateBOL.CandidateID = Convert.ToInt32(lblCandidateID.Text);
            candidateBOL.HighestQualification = Convert.ToInt32(ddlHighestQualification.SelectedItem.Value);
            // if (Convert.ToInt32(ddlCandidateStatus.SelectedItem.Value) != 17)
            candidateBOL.CandidateStatus = Convert.ToInt32(ddlCandidateStatus.SelectedItem.Value);

            candidateBOL.Salutation = ddlSalutation.SelectedItem.Text;
            candidateBOL.FirstName = txtFirstName.Text;
            candidateBOL.Middlename = txtMiddleName.Text;
            candidateBOL.LastName = txtLastName.Text;
            candidateBOL.DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text, "MM/dd/yyyy", null);    //TO DO
            //candidateBOL.DateOfBirth = Convert.ToDateTime(txtDateOfBirth.Text);
            candidateBOL.MaritalStatus = ddlMaritalStatus.SelectedItem.Text;
            candidateBOL.Gender = ddlGender.SelectedItem.Text;
            candidateBOL.AlternateContactNumber = txtAlternateContactNumber.Text;
            candidateBOL.MobileNumber = txtMobileNumber.Text;
            candidateBOL.Email = txtEmailID.Text;
            candidateBOL.AlternateEmailID = txtAlternateEmailID.Text;

            candidateBOL.TotalWorkExperienceInYear = Convert.ToInt32(ddlTotalWorkExpYears.SelectedItem.Text);
            candidateBOL.TotalWorkExperienceInMonths = Convert.ToInt32(ddlTotalWorkExpMonths.SelectedItem.Text);
            candidateBOL.RelevantWorkExperienceInYear = Convert.ToInt32(ddlRelevantWorkExpYears.SelectedItem.Text);
            candidateBOL.RelevantWorkExperienceInMonths = Convert.ToInt32(ddlRelevantWorkExpMonths.SelectedItem.Text);

            candidateBOL.WorkExperienceInMonths = (Convert.ToInt32(ddlTotalWorkExpYears.SelectedItem.Text) * 12) + Convert.ToInt32(ddlTotalWorkExpMonths.SelectedItem.Text);
            candidateBOL.RelevantlWorkExperienceInMonths = (Convert.ToInt32(ddlRelevantWorkExpYears.SelectedItem.Text) * 12) + Convert.ToInt32(ddlRelevantWorkExpMonths.SelectedItem.Text);

            //if (optValidPassportYes.Checked)
            if (rdobtnValidPassport.Items[0].Selected == true)
                candidateBOL.ValidPassPort = true;
            else
                candidateBOL.ValidPassPort = false;

            //if (optUSVisaYes.Checked)
            if (rdobtnUSVisa.Items[0].Selected == true)
                candidateBOL.USVisa = true;
            else
                candidateBOL.USVisa = false;

            //if (optWillingToRelocateYes.Checked)
            if (rdobtnWillingToRelocate.Items[0].Selected == true)
                candidateBOL.WillingToRelocate = true;
            else
                candidateBOL.WillingToRelocate = false;

            candidateBOL.PresentAddress = txtPresentAddress.Text;
            candidateBOL.State = txtState.Text;
            candidateBOL.City = txtCity.Text;
            candidateBOL.Country = Convert.ToInt32(ddlCountry.SelectedItem.Value);
            candidateBOL.PinCode = txtPinCode.Text;

            if (!string.IsNullOrEmpty(txtCurrentNoticePeriod.Text))
                candidateBOL.CurrrentNoticePeriod = Convert.ToInt32(txtCurrentNoticePeriod.Text);
            else
                candidateBOL.CurrrentNoticePeriod = 0;

            candidateBOL.JoiningDate = Convert.ToDateTime("1900-01-01 00:00:00.000");

            if (!string.IsNullOrEmpty(txtCurrentCTC.Text))
                candidateBOL.CurrentCTC = Convert.ToDecimal(txtCurrentCTC.Text);
            else
                candidateBOL.CurrentCTC = 0;

            candidateBOL.AreasOfInterest = txtAreasOfInterest.Text;
            candidateBOL.Source = ddlSource.SelectedItem.Text;
            candidateBOL.SourceName = txtSourceName.Text;
            candidateBOL.CurrentJobSummary = txtCurrentJobSummary.Text;
            candidateBOL.RewardsAndRecognition = txtRewardsAndRecognition.Text;
            candidateBOL.SpecialAchievements = txtSpecialAchievements.Text;
            candidateBOL.OtherSkills = txtOtherSkills.Text;
            //candidateBOL.CurrentRRF = 0;
            candidateBOL.CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            candidateBOL.CreatedDate = DateTime.Now;

            dsTemp = candidateBLL.UpdateCandidate(candidateBOL);
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Adds Skills for Candidate
    /// </summary>
    /// <param name="listOfSelectedCheckBoxes"></param>
    private void AddCandidateSkills(List<int> listOfSelectedCheckBoxes)
    {
        try
        {
            foreach (int selectedCheckBox in listOfSelectedCheckBoxes)
            {
                candidateBOL.SkillID = selectedCheckBox;
                candidateBLL.AddCandidateSkills(candidateBOL);
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Deletes Skills for Candidate
    /// </summary>
    /// <param name="listOfSelectedCheckBoxes"></param>
    private void DeleteCandidateSkills(List<int> listOfSelectedCheckBoxes)
    {
        try
        {
            foreach (int selectedCheckBox in listOfSelectedCheckBoxes)
            {
                candidateBOL.SkillID = selectedCheckBox;
                candidateBLL.DeleteCandidateSkills(candidateBOL);
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets Selected Skills from CheckBoxList
    /// </summary>
    /// <param name="chkList"></param>
    /// <returns></returns>
    private List<int> GetSelectedCandidateSkills(CheckBoxList chkList)
    {
        try
        {
            List<int> listOfSelectedCheckBoxes = new List<int>();

            foreach (ListItem chkItem in chkList.Items)
            {
                if (chkItem.Selected)
                {
                    listOfSelectedCheckBoxes.Add(Convert.ToInt32(chkItem.Value));
                }
            }
            return listOfSelectedCheckBoxes;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Saves Experience Details in DataBase
    /// </summary>
    /// <param name="listOfEmployeeRecords"></param>
    private void SaveExperienceDetails(List<CandidateBOL> listOfInsertedRecords, List<CandidateBOL> listOfUpdatedRecords, List<CandidateBOL> listOfDeletedRecords, string candidateID)
    {
        try
        {
            DataSet dsTemp = candidateBLL.GetCandidateExpID(candidateBOL); // Get all ExpIDs for candidate before inserting record

            if (listOfInsertedRecords != null && listOfInsertedRecords.Count > 0)
            {
                //var listOfCandidateExperienceWithoutIDColumn = listOfInsertedRecords.Where(l => l.ExpID != null).Select(l => new { candidateID = l.CandidateID, l.OrganisationName, l.Location, l.WorkedFrom, l.WorkedTill, l.ExpType, l.PositionHeld, l.ReportingManager, l.CTC, l.LastModifiedBy, l.LastModifiedDate });
                var listOfCandidateExperienceWithoutIDColumn = listOfInsertedRecords.Where(l => l.ExpID != null).Select(l => new { candidateID = l.CandidateID, l.OrganisationName, l.WorkedFrom, l.WorkedTill, l.ExpType, l.PositionHeld, l.ReportingManager, l.CTC, l.LastModifiedBy, l.LastModifiedDate });
                foreach (CandidateBOL employee in listOfInsertedRecords)
                {
                    employee.CandidateID = Convert.ToInt32(candidateID);
                    employee.LastModifiedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                    employee.LastModifiedDate = DateTime.Now;
                    candidateBLL.AddCandidateExperienceDetails(employee);

                    //InsertRecord(employee.EmpID, employee.EmployeeName, employee.Department);
                }
            }

            if (listOfUpdatedRecords != null && listOfUpdatedRecords.Count > 0)
            {
                foreach (CandidateBOL employee in listOfUpdatedRecords)
                {
                    employee.CandidateID = Convert.ToInt32(candidateID);
                    employee.LastModifiedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                    employee.LastModifiedDate = DateTime.Now;
                    candidateBLL.UpdateCandidateExperienceDetails(employee.ExpID, employee);

                    //UpdateRecord(employee.EmpID, employee.EmployeeName, employee.Department);
                }
            }

            if (listOfDeletedRecords != null && listOfDeletedRecords.Count > 0)
            {
                foreach (CandidateBOL employee in listOfDeletedRecords)
                {
                    foreach (DataRow row in dsTemp.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(row["ExpID"]) == employee.ExpID)
                        {
                            candidateBLL.DeleteCandidateExperienceDetails(employee.ExpID);
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Saves Certification Details in DataBase
    /// </summary>
    /// <param name="listOfInsertedRecords"></param>
    /// <param name="listOfUpdatedRecords"></param>
    /// <param name="listOfDeletedRecords"></param>
    /// <param name="candidateID"></param>
    private void SaveCertificationDetails(List<CandidateBOL> listOfInsertedRecords, List<CandidateBOL> listOfUpdatedRecords, List<CandidateBOL> listOfDeletedRecords, string candidateID)
    {
        try
        {
            DataSet dsTemp = candidateBLL.GetCandidateCertificationID(candidateBOL); // Get all CertificationIDs for candidate before inserting record

            if (listOfInsertedRecords != null && listOfInsertedRecords.Count > 0)
            {
                var listOfCandidateExperienceWithoutIDColumn = listOfInsertedRecords.Where(l => l.CertificationID != null).Select(l => new { candidateID = l.CandidateID, l.CertificationName, l.CertificationNo, l.Institution, l.CertificationDate, l.CertificationScore, l.CertificationGrade });

                foreach (CandidateBOL employee in listOfInsertedRecords)
                {
                    employee.CandidateID = Convert.ToInt32(candidateID);
                    candidateBLL.AddCandidateCertificationDetails(employee);
                }
            }

            if (listOfUpdatedRecords != null && listOfUpdatedRecords.Count > 0)
            {
                foreach (CandidateBOL employee in listOfUpdatedRecords)
                {
                    employee.CandidateID = Convert.ToInt32(candidateID);
                    candidateBLL.UpdateCandidateCertificationDetails(employee.CertificationID, employee);
                }
            }

            if (listOfDeletedRecords != null && listOfDeletedRecords.Count > 0)
            {
                foreach (CandidateBOL employee in listOfDeletedRecords)
                {
                    foreach (DataRow row in dsTemp.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(row["CertificationID"]) == employee.CertificationID)
                        {
                            candidateBLL.DeleteCandidateCertificationDetails(employee.CertificationID);
                        }
                    }

                    //var tt=  dsTemp.Tables[0].Rows.Cast<DataRow>().Where(f => Convert.ToInt32(f["CertificationID"]) == employee.CertificationID);
                }
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Saves Education Details in DataBase
    /// </summary>
    /// <param name="listOfInsertedRecords"></param>
    /// <param name="listOfUpdatedRecords"></param>
    /// <param name="listOfDeletedRecords"></param>
    /// <param name="candidateID"></param>
    private void SaveEducationDetails(List<CandidateBOL> listOfInsertedRecords, List<CandidateBOL> listOfUpdatedRecords, List<CandidateBOL> listOfDeletedRecords, string candidateID)
    {
        try
        {
            DataSet dsTemp = candidateBLL.GetCandidateEducationID(candidateBOL); // Get all EducationIDs for candidate before inserting record

            if (listOfInsertedRecords != null && listOfInsertedRecords.Count > 0)
            {
                //var listOfCandidateExperienceWithoutIDColumn = listOfInsertedRecords.Where(l => l.EducationID != null).Select(l => new { candidateID = l.CandidateID, l.Degree, l.Specialization, l.Institute, l.University, l.Course, l.Year, l.Type, l.Percentage });
                var listOfCandidateExperienceWithoutIDColumn = listOfInsertedRecords.Where(l => l.EducationID != null).Select(l => new { candidateID = l.CandidateID, l.Degree, l.Specialization, l.University, l.Course, l.Year, l.Type, l.Percentage });

                foreach (CandidateBOL employee in listOfInsertedRecords)
                {
                    employee.CandidateID = Convert.ToInt32(candidateID);
                    candidateBLL.AddCandidateEducationDetails(employee);
                }
            }

            if (listOfUpdatedRecords != null && listOfUpdatedRecords.Count > 0)
            {
                foreach (CandidateBOL employee in listOfUpdatedRecords)
                {
                    employee.CandidateID = Convert.ToInt32(candidateID);
                    candidateBLL.UpdateCandidateEducationDetails(employee.EducationID, employee);
                }
            }

            if (listOfDeletedRecords != null && listOfDeletedRecords.Count > 0)
            {
                foreach (CandidateBOL employee in listOfDeletedRecords)
                {
                    foreach (DataRow row in dsTemp.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(row["EducationID"]) == employee.EducationID)
                        {
                            candidateBLL.DeleteCandidateEducationDetails(employee.EducationID);
                        }
                    }
                    //if (CheckCandidateExpID(candidateBOL))  //new code to check Education ID
                    //    candidateBLL.DeleteCandidateEducationDetails(employee.EducationID);
                }
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Adds Experience Details of Candidate in DataBase
    /// </summary>
    private void AddCandidateExperienceDetails()
    {
        try
        {
            SaveExperienceDetails(listOfInsertedExperienceRecords, listOfUpdatedExperienceRecords, listOfDeletedExperienceRecords, candidateID);
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Adds Education Details of Candidate in DataBase
    /// </summary>
    private void AddCandidateEducationDetails()
    {
        try
        {
            SaveEducationDetails(listOfInsertedEducationRecords, listOfUpdatedEducationRecords, listOfDeletedEducationRecords, candidateID);
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Adds Certification Details of Candidate in DataBase
    /// </summary>
    private void AddCandidateCertificationDetails()
    {
        try
        {
            SaveCertificationDetails(listOfInsertedCertificationRecords, listOfUpdatedCertificationRecords, listOfDeletedCertificationRecords, candidateID);
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Binds GridView on page Load
    /// </summary>
    private void BindOnPageLoad(DataTable dt, GridView grdCandidate)
    {
        try
        {
            //ds = candidateBLL.GetExperienceDetails(candidateBOL);
            dsExperience = candidateBLL.GetExperienceDetails(candidateBOL);

            dtTempExperience = dsExperience.Tables[0].Clone();
            DataRow drTempExperience = dtTempExperience.NewRow();
            dtTempExperience.Rows.Add(drTempExperience);

            dsEducation = candidateBLL.GetEducationDetails(candidateBOL);

            // begin region new code for blank grid
            dtTempEducation = dsEducation.Tables[0].Clone();
            DataRow drTempEducation = dtTempEducation.NewRow();
            dtTempEducation.Rows.Add(drTempEducation);
            // end region

            dsCertification = candidateBLL.GetCertificationDetails(candidateBOL);

            // begin region new code for blank grid
            dtTempCertification = dsCertification.Tables[0].Clone();
            DataRow drTempCertification = dtTempCertification.NewRow();
            dtTempCertification.Rows.Add(drTempCertification);
            // end region

            //Bind(dtTempExperience, grdExperienceDetails);
            //Bind(dtTempEducation, grdEducationDetails);
            //Bind(dtTempCertification, grdCertificationDetails);
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Binds GridView on page Load
    /// </summary>
    private void BindOnPageLoad(DataSet ds, GridView grdCandidate)
    {
        try
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                grdCandidate.DataSource = ds.Tables[0];
                grdCandidate.DataBind();
                ListOfActiveEducationRecords = GetEducationRecordsInList(dsEducation);
                ListOfActiveCertificationRecords = GetCertificationRecordsInList(dsCertification);
                ListOfActiveExperienceRecords = GetExperienceRecordsInList(dsExperience);
            }
            else
            {
                DataTable dt = ds.Tables[0].Clone();
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);

                grdCandidate.DataSource = dt;
                grdCandidate.DataBind();
                grdCandidate.Rows[0].Visible = false;
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Binds Experience GridView and makes Rows in GridView Disappear
    /// </summary>
    private void BindBlankGridForExperienceDetails()
    {
        try
        {
            foreach (GridViewRow row in grdExperienceDetails.Rows)
            {
                // Label lblExpSrNo = row.FindControl("lblExpSrNo") as Label;
                Label lblExpID = row.FindControl("lblExpID") as Label;
                //Label lblPA = row.FindControl("lblPA") as Label;
                Label lblOrganisation = row.FindControl("lblOrganisation") as Label;
                // Label lblOfficeLocation = row.FindControl("lblOfficeLocation") as Label;
                Label lblFromDate = row.FindControl("lblFromDate") as Label;
                Label lblToDate = row.FindControl("lblToDate") as Label;
                Label lblDesignation = row.FindControl("lblDesignation") as Label;
                Label lblReportingManager = row.FindControl("lblReportingManager") as Label;
                Label lblLastDrawnCTCInLacs = row.FindControl("lblLastDrawnCTCInLacs") as Label;
                Label lblExpType = row.FindControl("lblExpType") as Label;
                ImageButton imgbtnFromDate = row.FindControl("imgbtnFromDate") as ImageButton;

                ImageButton imgbtnToDate = row.FindControl("imgbtnToDate") as ImageButton;
                DropDownList ddlExpType = row.FindControl("ddlExpType") as DropDownList;
                Button btnRemoveExperience = row.FindControl("btnRemoveExperience") as Button;
                Button btnEditExperience = row.FindControl("btnEditExperience") as Button;

                // lblExpSrNo.Visible = false;
                lblExpID.Visible = false;
                //lblPA.Visible = false;
                lblOrganisation.Visible = false;
                //lblOfficeLocation.Visible = false;
                lblFromDate.Visible = false;
                lblToDate.Visible = false;
                lblDesignation.Visible = false;
                lblReportingManager.Visible = false;
                lblLastDrawnCTCInLacs.Visible = false;
                lblExpType.Visible = false;
                imgbtnFromDate.Visible = false;
                imgbtnToDate.Visible = false;

                btnEditExperience.Visible = false;
                btnRemoveExperience.Visible = false;

                grdExperienceDetails.Rows[0].Visible = false;
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Binds Certification GridView and makes Rows in GridView Disappear
    /// </summary>
    private void BindBlankGridForCertificationDetails()
    {
        try
        {
            foreach (GridViewRow row in grdCertificationDetails.Rows)
            {
                Label lblCertificationSrNo = row.FindControl("lblCertificationSrNo") as Label;
                Label lblCertificationID = row.FindControl("lblCertificationID") as Label;
                Label lblCertificationName = row.FindControl("lblCertificationName") as Label;
                Label lblCertificationNo = row.FindControl("lblCertificationNo") as Label;
                Label lblInstitution = row.FindControl("lblInstitution") as Label;
                Label lblCertifiedOnDate = row.FindControl("lblCertifiedOnDate") as Label;
                Label lblCertificationScore = row.FindControl("lblCertificationScore") as Label;
                Label lblCertificationGrade = row.FindControl("lblCertificationGrade") as Label;

                DropDownList ddlCertificationName = row.FindControl("ddlCertificationName") as DropDownList;

                ImageButton imgbtnCertifiedOnDate = row.FindControl("imgbtnCertifiedOnDate") as ImageButton;

                Button btnRemoveCertification = row.FindControl("btnRemoveCertification") as Button;
                Button btnEditCertification = row.FindControl("btnEditCertification") as Button;

                lblCertificationSrNo.Visible = false;
                lblCertificationID.Visible = false;
                lblCertificationName.Visible = false;
                lblCertificationNo.Visible = false;
                lblInstitution.Visible = false;
                lblCertifiedOnDate.Visible = false;
                lblCertificationScore.Visible = false;
                lblCertificationGrade.Visible = false;
                imgbtnCertifiedOnDate.Visible = false;
                btnEditCertification.Visible = false;
                btnRemoveCertification.Visible = false;
                grdCertificationDetails.Rows[0].Visible = false;
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    ///  Binds Education GridView and makes Rows in GridView Disappear
    /// </summary>
    private void BindBlankGridForEducationDetails()
    {
        try
        {
            foreach (GridViewRow row in grdEducationDetails.Rows)
            {
                Label lblEducationSrNo = row.FindControl("lblEducationSrNo") as Label;
                Label lblEducationID = row.FindControl("lblEducationID") as Label;
                Label lblDegree = row.FindControl("lblDegree") as Label;
                Label lblCourse = row.FindControl("lblCourse") as Label;
                Label lblSpecialization = row.FindControl("lblSpecialization") as Label;
                Label lblUniversity = row.FindControl("lblUniversity") as Label;
                //Label lblInstitute = row.FindControl("lblInstitute") as Label;
                Label lblYear = row.FindControl("lblYear") as Label;
                Label lblType = row.FindControl("lblType") as Label;
                Label lblTypeCourse = row.FindControl("lblTypeCourse") as Label;
                Label lblPercentage = row.FindControl("lblPercentage") as Label;
                Label lblCourseName = row.FindControl("lblCourseName") as Label;

                DropDownList ddlDegree = row.FindControl("ddlDegree") as DropDownList;
                DropDownList ddlCourse = row.FindControl("ddlCourse") as DropDownList;
                DropDownList ddlYear = row.FindControl("ddlYear") as DropDownList;
                DropDownList ddlType = row.FindControl("ddlType") as DropDownList;
                TextBox txtSpecialization = row.FindControl("txtSpecialization") as TextBox;
                TextBox txtUniversity = row.FindControl("txtUniversity") as TextBox;
                //TextBox txtInstitute = row.FindControl("txtInstitute") as TextBox;
                TextBox txtPercentage = row.FindControl("txtPercentage") as TextBox;

                Button btnRemoveEducation = row.FindControl("btnRemoveEducation") as Button;
                Button btnEditEducation = row.FindControl("btnEditEducation") as Button;

                lblEducationSrNo.Visible = false;
                lblEducationID.Visible = false;
                lblDegree.Visible = false;
                lblCourse.Visible = false;
                lblSpecialization.Visible = false;
                //lblInstitute.Visible = false;
                lblUniversity.Visible = false;
                lblYear.Visible = false;
                lblType.Visible = false;
                lblTypeCourse.Visible = false;
                lblCourseName.Visible = false;
                lblPercentage.Visible = false;
                btnRemoveEducation.Visible = false;
                btnEditEducation.Visible = false;

                grdEducationDetails.Rows[0].Visible = false;
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Clears all Controls
    /// </summary>
    private void ClearAllControls()
    {
        try
        {
            ddlCandidateStatus.SelectedIndex = ddlCandidateStatus.Items.IndexOf(ddlCandidateStatus.Items.FindByText("New"));
            //ddlCandidateStatus.SelectedIndex = 0; //old code
            ddlSalutation.SelectedIndex = 0;
            ddlMaritalStatus.SelectedIndex = 0;
            ddlGender.SelectedIndex = 0;
            ddlTotalWorkExpYears.SelectedIndex = 0;
            ddlTotalWorkExpMonths.SelectedIndex = 0;
            ddlRelevantWorkExpYears.SelectedIndex = 0;
            ddlRelevantWorkExpMonths.SelectedIndex = 0;
            ddlHighestQualification.SelectedIndex = 0;
            ddlSource.SelectedIndex = 0;
            ddlCountry.SelectedIndex = 0;

            chkList.ClearSelection();

            lblCandidateID.Text = string.Empty;

            txtFirstName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtDateOfBirth.Text = string.Empty;
            txtAlternateContactNumber.Text = string.Empty;
            txtMobileNumber.Text = string.Empty;
            txtEmailID.Text = string.Empty;
            txtAlternateEmailID.Text = string.Empty;
            txtOtherSkills.Text = string.Empty;
            txtPresentAddress.Text = string.Empty;
            txtState.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtPinCode.Text = string.Empty;
            txtCurrentNoticePeriod.Text = string.Empty;
            txtAreasOfInterest.Text = string.Empty;
            txtCurrentCTC.Text = string.Empty;
            txtSourceName.Text = string.Empty;
            txtCurrentJobSummary.Text = string.Empty;
            txtSpecialAchievements.Text = string.Empty;
            txtRewardsAndRecognition.Text = string.Empty;

            rdobtnUSVisa.Items[1].Selected = true;
            //optUSVisaNo.Checked = true;

            rdobtnValidPassport.Items[1].Selected = true;
            //optValidPassportNo.Checked = true;

            rdobtnWillingToRelocate.Items[1].Selected = true;
            //optWillingToRelocateNo.Checked = true;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Clears GridViews
    /// </summary>
    private void ClearGridViews()
    {
        try
        {
            dsExperience = candidateBLL.GetExperienceDetails(candidateBOL);
            dsEducation = candidateBLL.GetEducationDetails(candidateBOL);
            dsCertification = candidateBLL.GetCertificationDetails(candidateBOL);
            BindOnPageLoad(dsExperience, grdExperienceDetails);
            BindOnPageLoad(dsEducation, grdEducationDetails);
            BindOnPageLoad(dsCertification, grdCertificationDetails);
            #region backup

            #endregion
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets Candidate Detail In Edit Mode
    /// </summary>
    /// <param name="ID"></param>
    private void GetCandidateDetailsInEditMode(string ID)
    {
        try
        {
            if (ID != "")
            {
                //chkList.Items.Clear();

                rdobtnValidPassport.Items[1].Selected = false;
                //optValidPassportNo.Checked = false;

                rdobtnUSVisa.Items[1].Selected = false;
                //optUSVisaNo.Checked = false;

                rdobtnWillingToRelocate.Items[1].Selected = false;
                //optWillingToRelocateNo.Checked = false;

                lblCandidateID.Text = ID;
                modeForSkills = "Edit";
                candidateBOL.CandidateID = Convert.ToInt32(ID);
                dsExperience = candidateBLL.GetExperienceDetails(candidateBOL);
                dsEducation = candidateBLL.GetEducationDetails(candidateBOL);
                dsCertification = candidateBLL.GetCertificationDetails(candidateBOL);

                BindOnPageLoad(dsExperience, grdExperienceDetails);
                BindOnPageLoad(dsEducation, grdEducationDetails);
                BindOnPageLoad(dsCertification, grdCertificationDetails);

                BindDropDownList(ddlTotalWorkExpYears, 0, 50);
                BindDropDownList(ddlTotalWorkExpMonths, 0, 12);
                BindDropDownList(ddlRelevantWorkExpYears, 0, 50);
                BindDropDownList(ddlRelevantWorkExpMonths, 0, 12);

                List<KeyValuePair<int, string>> listOfCandidateStatus = GetAllStatus();
                List<KeyValuePair<int, string>> listOfQualifications = GetAllCourses();
                List<KeyValuePair<int, string>> listofCountryNames = GetAllCountryNames();

                BindDropDownListWithValuesFromDB(ddlCandidateStatus, listOfCandidateStatus);
                BindDropDownListWithValuesFromDB(ddlHighestQualification, listOfQualifications);
                BindDropDownListWithValuesFromDB(ddlCountry, listofCountryNames);

                List<KeyValuePair<int, string>> listOfSkills = GetAllSkills(modeForSkills);
                AddCheckBoxesToCheckBoxList(listOfSkills);

                dsCandidate = candidateBLL.GetCandidateDetails(candidateBOL);

                txtFirstName.Text = dsCandidate.Tables[0].Rows[0]["FirstName"].ToString();
                txtMiddleName.Text = dsCandidate.Tables[0].Rows[0]["MiddleName"].ToString();
                txtLastName.Text = dsCandidate.Tables[0].Rows[0]["LastName"].ToString();
                txtAlternateContactNumber.Text = dsCandidate.Tables[0].Rows[0]["AlternateContactNumber"].ToString();
                txtMobileNumber.Text = dsCandidate.Tables[0].Rows[0]["MobileNumber"].ToString();
                txtEmailID.Text = dsCandidate.Tables[0].Rows[0]["Email"].ToString();
                txtAlternateEmailID.Text = dsCandidate.Tables[0].Rows[0]["AlternateEmailID"].ToString();
                txtOtherSkills.Text = dsCandidate.Tables[0].Rows[0]["OtherSkills"].ToString();
                txtPresentAddress.Text = dsCandidate.Tables[0].Rows[0]["CurrentAddress"].ToString();
                txtState.Text = dsCandidate.Tables[0].Rows[0]["State"].ToString();
                txtCity.Text = dsCandidate.Tables[0].Rows[0]["City"].ToString();
                txtPinCode.Text = dsCandidate.Tables[0].Rows[0]["ZipCode"].ToString();
                txtCurrentNoticePeriod.Text = dsCandidate.Tables[0].Rows[0]["NoticePeriod"].ToString();
                txtAreasOfInterest.Text = dsCandidate.Tables[0].Rows[0]["AreaOfInterest"].ToString();
                txtCurrentCTC.Text = dsCandidate.Tables[0].Rows[0]["CurrentCTC"].ToString();
                txtSourceName.Text = dsCandidate.Tables[0].Rows[0]["SourceName"].ToString();
                txtCurrentJobSummary.Text = dsCandidate.Tables[0].Rows[0]["CurrentJobSummary"].ToString();
                txtRewardsAndRecognition.Text = dsCandidate.Tables[0].Rows[0]["Recognition"].ToString();
                txtSpecialAchievements.Text = dsCandidate.Tables[0].Rows[0]["Achievements"].ToString();

                ddlCandidateStatus.SelectedIndex =
                    ddlCandidateStatus.Items.IndexOf(
                        ddlCandidateStatus.Items.FindByValue(dsCandidate.Tables[0].Rows[0]["CandidateStatus"].ToString()));
                Session["CandidateStatus"] = dsCandidate.Tables[0].Rows[0]["CandidateStatus"].ToString();
                ddlMaritalStatus.SelectedIndex =
                    ddlMaritalStatus.Items.IndexOf(
                        ddlMaritalStatus.Items.FindByText(dsCandidate.Tables[0].Rows[0]["MaritalStatus"].ToString()));
                ddlSalutation.SelectedIndex =
                    ddlSalutation.Items.IndexOf(
                        ddlSalutation.Items.FindByText(dsCandidate.Tables[0].Rows[0]["Salutation"].ToString()));
                ddlHighestQualification.SelectedIndex =
                    ddlHighestQualification.Items.IndexOf(
                        ddlHighestQualification.Items.FindByValue(
                            dsCandidate.Tables[0].Rows[0]["HighestQualification"].ToString()));
                ddlCountry.SelectedIndex =
                    ddlCountry.Items.IndexOf(
                        ddlCountry.Items.FindByValue(dsCandidate.Tables[0].Rows[0]["Country"].ToString()));
                ddlGender.SelectedIndex =
                    ddlGender.Items.IndexOf(
                        ddlGender.Items.FindByText(dsCandidate.Tables[0].Rows[0]["Gender"].ToString()));
                ddlSource.SelectedIndex =
                    ddlSource.Items.IndexOf(
                        ddlSource.Items.FindByText(dsCandidate.Tables[0].Rows[0]["Source"].ToString()));

                ddlTotalWorkExpYears.SelectedIndex =
                    ddlTotalWorkExpYears.Items.IndexOf(
                        ddlTotalWorkExpYears.Items.FindByText(
                            dsCandidate.Tables[0].Rows[0]["TotalWorkExperienceInYear"].ToString()));
                ddlTotalWorkExpMonths.SelectedIndex =
                    ddlTotalWorkExpMonths.Items.IndexOf(
                        ddlTotalWorkExpMonths.Items.FindByText(
                            dsCandidate.Tables[0].Rows[0]["TotalWorkExperienceInMonths"].ToString()));
                ddlRelevantWorkExpYears.SelectedIndex =
                    ddlRelevantWorkExpYears.Items.IndexOf(
                        ddlRelevantWorkExpYears.Items.FindByText(
                            dsCandidate.Tables[0].Rows[0]["RelevantWorkExperienceInYear"].ToString()));
                ddlRelevantWorkExpMonths.SelectedIndex =
                    ddlRelevantWorkExpMonths.Items.IndexOf(
                        ddlRelevantWorkExpMonths.Items.FindByText(
                            dsCandidate.Tables[0].Rows[0]["RelevantWorkExperienceInMonths"].ToString()));

                DateTime dateOfBirth = new DateTime();
                dateOfBirth = Convert.ToDateTime(dsCandidate.Tables[0].Rows[0]["DateOfBirth"]);
                txtDateOfBirth.Text = dateOfBirth.ToString("MM/dd/yyyy");

                if (Convert.ToBoolean(dsCandidate.Tables[0].Rows[0]["ValidPassport"]))
                    rdobtnValidPassport.Items[0].Selected = true;
                //optValidPassportYes.Checked = true;
                else
                    rdobtnValidPassport.Items[1].Selected = true;
                //optValidPassportNo.Checked = true;

                if (Convert.ToBoolean(dsCandidate.Tables[0].Rows[0]["USVisa"]))
                    rdobtnUSVisa.Items[0].Selected = true;
                //optUSVisaYes.Checked = true;
                else
                    rdobtnUSVisa.Items[1].Selected = true;
                //optUSVisaNo.Checked = true;

                if (Convert.ToBoolean(dsCandidate.Tables[0].Rows[0]["WillingToRelocate"]))
                    rdobtnWillingToRelocate.Items[0].Selected = true;
                //optWillingToRelocateYes.Checked = true;
                else
                    rdobtnWillingToRelocate.Items[1].Selected = true;
                //optWillingToRelocateNo.Checked = true;

                txtOtherSkills.ToolTip = dsCandidate.Tables[0].Rows[0]["OtherSkills"].ToString();
                txtCurrentJobSummary.ToolTip = dsCandidate.Tables[0].Rows[0]["CurrentJobSummary"].ToString();
                txtRewardsAndRecognition.ToolTip = dsCandidate.Tables[0].Rows[0]["Recognition"].ToString();
                txtSpecialAchievements.ToolTip = dsCandidate.Tables[0].Rows[0]["Achievements"].ToString();
                txtPresentAddress.ToolTip = dsCandidate.Tables[0].Rows[0]["CurrentAddress"].ToString();
                txtAreasOfInterest.ToolTip = dsCandidate.Tables[0].Rows[0]["AreaOfInterest"].ToString();

                dsCandidate = candidateBLL.GetCandidateSkills(candidateBOL);
                //  chkList.ClearSelection();                                                            // Test code for Error Cannot have multiple items selected in a DropDownList.
                foreach (DataRow row in dsCandidate.Tables[0].Rows)
                {
                    chkList.Items.FindByValue(row["ID"].ToString()).Selected = true;
                }
                //txtCandidateHistory.Attributes.Add("onkeydown", "return false");
                //List<KeyValuePair<int, string>> listOfCandidateHistory = GetHistory();
                ////AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
                //foreach (var k in listOfCandidateHistory)
                //{
                //    txtCandidateHistory.Text += k.Value + Environment.NewLine;

                //}
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Gets Candidate Detail By FirstName LastName and  DOB
    /// </summary>
    private DataSet GetCandidateDetailsByFirstNameAndLastNameAndDOB(string firstName, string lastname, string DOB)
    {
        try
        {
            DataSet dsTemp = candidateBLL.GetCandidateDetailsByFirstNameAndLastNameAndDOB(firstName, lastname, DOB);
            return dsTemp;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Checks Experience Ids for particular candidate
    /// </summary>
    /// <param name="candidateBOL"></param>
    /// <returns></returns>
    private bool CheckCandidateExpID(CandidateBOL candidateBOL)
    {
        try
        {
            DataSet dsTemp = candidateBLL.GetCandidateExpID(candidateBOL);

            foreach (DataRow row in dsTemp.Tables[0].Rows)
            {
                if (Convert.ToInt32(row["ExpID"]) == ExpIndex)
                {
                    return true;
                }
            }
            return false;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Checks Certification Ids for particular candidate
    /// </summary>
    /// <param name="candidateBOL"></param>
    /// <returns></returns>
    private bool CheckCandidateCertificationID(CandidateBOL candidateBOL)
    {
        try
        {
            DataSet dsTemp = candidateBLL.GetCandidateCertificationID(candidateBOL);

            foreach (DataRow row in dsTemp.Tables[0].Rows)
            {
                if (Convert.ToInt32(row["CertificationID"]) == CertificationIndex)
                {
                    return true;
                }
            }
            return false;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Checks Education Ids for particular candidate
    /// </summary>
    /// </summary>
    /// <param name="candidateBOL"></param>
    /// <returns></returns>
    private bool CheckCandidateEducationID(CandidateBOL candidateBOL)
    {
        try
        {
            DataSet dsTemp = candidateBLL.GetCandidateEducationID(candidateBOL);

            foreach (DataRow row in dsTemp.Tables[0].Rows)
            {
                if (Convert.ToInt32(row["EducationID"]) == EducationIndex)
                {
                    return true;
                }
            }
            return false;
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="ID"></param>
    private void GetCandidateDetailsWithoutCheckBox(string ID)
    {
        try
        {
            //chkList.Items.Clear();

            rdobtnValidPassport.Items[1].Selected = false;
            //optValidPassportNo.Checked = false;

            rdobtnUSVisa.Items[1].Selected = false;
            //optUSVisaNo.Checked = false;

            rdobtnWillingToRelocate.Items[1].Selected = false;
            //optWillingToRelocateNo.Checked = false;

            lblCandidateID.Text = ID;
            candidateBOL.CandidateID = Convert.ToInt32(ID);
            dsExperience = candidateBLL.GetExperienceDetails(candidateBOL);
            dsEducation = candidateBLL.GetEducationDetails(candidateBOL);
            dsCertification = candidateBLL.GetCertificationDetails(candidateBOL);

            BindOnPageLoad(dsExperience, grdExperienceDetails);
            BindOnPageLoad(dsEducation, grdEducationDetails);
            BindOnPageLoad(dsCertification, grdCertificationDetails);

            dsCandidate = candidateBLL.GetCandidateDetails(candidateBOL);

            //txtFirstName.Text = dsCandidate.Tables[0].Rows[0]["FirstName"].ToString();
            txtMiddleName.Text = dsCandidate.Tables[0].Rows[0]["MiddleName"].ToString();
            //txtLastName.Text = dsCandidate.Tables[0].Rows[0]["LastName"].ToString();
            txtAlternateContactNumber.Text = dsCandidate.Tables[0].Rows[0]["AlternateContactNumber"].ToString();
            txtMobileNumber.Text = dsCandidate.Tables[0].Rows[0]["MobileNumber"].ToString();
            txtEmailID.Text = dsCandidate.Tables[0].Rows[0]["Email"].ToString();
            txtAlternateEmailID.Text = dsCandidate.Tables[0].Rows[0]["AlternateEmailID"].ToString();
            txtOtherSkills.Text = dsCandidate.Tables[0].Rows[0]["OtherSkills"].ToString();
            txtPresentAddress.Text = dsCandidate.Tables[0].Rows[0]["CurrentAddress"].ToString();
            txtState.Text = dsCandidate.Tables[0].Rows[0]["State"].ToString();
            txtCity.Text = dsCandidate.Tables[0].Rows[0]["City"].ToString();
            txtPinCode.Text = dsCandidate.Tables[0].Rows[0]["ZipCode"].ToString();
            txtCurrentNoticePeriod.Text = dsCandidate.Tables[0].Rows[0]["NoticePeriod"].ToString();
            txtAreasOfInterest.Text = dsCandidate.Tables[0].Rows[0]["AreaOfInterest"].ToString();
            txtCurrentCTC.Text = dsCandidate.Tables[0].Rows[0]["CurrentCTC"].ToString();
            txtSourceName.Text = dsCandidate.Tables[0].Rows[0]["SourceName"].ToString();
            txtCurrentJobSummary.Text = dsCandidate.Tables[0].Rows[0]["CurrentJobSummary"].ToString();
            txtRewardsAndRecognition.Text = dsCandidate.Tables[0].Rows[0]["Recognition"].ToString();
            txtSpecialAchievements.Text = dsCandidate.Tables[0].Rows[0]["Achievements"].ToString();

            ddlCandidateStatus.SelectedIndex = ddlCandidateStatus.Items.IndexOf(ddlCandidateStatus.Items.FindByValue(dsCandidate.Tables[0].Rows[0]["CandidateStatus"].ToString()));
            ddlMaritalStatus.SelectedIndex = ddlMaritalStatus.Items.IndexOf(ddlMaritalStatus.Items.FindByText(dsCandidate.Tables[0].Rows[0]["MaritalStatus"].ToString()));
            ddlSalutation.SelectedIndex = ddlSalutation.Items.IndexOf(ddlSalutation.Items.FindByText(dsCandidate.Tables[0].Rows[0]["Salutation"].ToString()));
            ddlHighestQualification.SelectedIndex = ddlHighestQualification.Items.IndexOf(ddlHighestQualification.Items.FindByValue(dsCandidate.Tables[0].Rows[0]["HighestQualification"].ToString()));
            ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByValue(dsCandidate.Tables[0].Rows[0]["Country"].ToString()));
            ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByText(dsCandidate.Tables[0].Rows[0]["Gender"].ToString()));
            ddlSource.SelectedIndex = ddlSource.Items.IndexOf(ddlSource.Items.FindByText(dsCandidate.Tables[0].Rows[0]["Source"].ToString()));

            ddlTotalWorkExpYears.SelectedIndex = ddlTotalWorkExpYears.Items.IndexOf(ddlTotalWorkExpYears.Items.FindByText(dsCandidate.Tables[0].Rows[0]["TotalWorkExperienceInYear"].ToString()));
            ddlTotalWorkExpMonths.SelectedIndex = ddlTotalWorkExpMonths.Items.IndexOf(ddlTotalWorkExpMonths.Items.FindByText(dsCandidate.Tables[0].Rows[0]["TotalWorkExperienceInMonths"].ToString()));
            ddlRelevantWorkExpYears.SelectedIndex = ddlRelevantWorkExpYears.Items.IndexOf(ddlRelevantWorkExpYears.Items.FindByText(dsCandidate.Tables[0].Rows[0]["RelevantWorkExperienceInYear"].ToString()));
            ddlRelevantWorkExpMonths.SelectedIndex = ddlRelevantWorkExpMonths.Items.IndexOf(ddlRelevantWorkExpMonths.Items.FindByText(dsCandidate.Tables[0].Rows[0]["RelevantWorkExperienceInMonths"].ToString()));

            if (Convert.ToBoolean(dsCandidate.Tables[0].Rows[0]["ValidPassport"]))
                rdobtnValidPassport.Items[0].Selected = true;
            //optValidPassportYes.Checked = true;
            else
                rdobtnValidPassport.Items[1].Selected = true;
            //optValidPassportNo.Checked = true;

            if (Convert.ToBoolean(dsCandidate.Tables[0].Rows[0]["USVisa"]))
                rdobtnUSVisa.Items[0].Selected = true;
            //optUSVisaYes.Checked = true;
            else
                rdobtnUSVisa.Items[1].Selected = true;
            //optUSVisaNo.Checked = true;

            if (Convert.ToBoolean(dsCandidate.Tables[0].Rows[0]["WillingToRelocate"]))
                rdobtnWillingToRelocate.Items[0].Selected = true;
            //optWillingToRelocateYes.Checked = true;
            else
                rdobtnWillingToRelocate.Items[1].Selected = true;
            //optWillingToRelocateNo.Checked = true;

            txtOtherSkills.ToolTip = dsCandidate.Tables[0].Rows[0]["OtherSkills"].ToString();
            txtCurrentJobSummary.ToolTip = dsCandidate.Tables[0].Rows[0]["CurrentJobSummary"].ToString();
            txtRewardsAndRecognition.ToolTip = dsCandidate.Tables[0].Rows[0]["Recognition"].ToString();
            txtSpecialAchievements.ToolTip = dsCandidate.Tables[0].Rows[0]["Achievements"].ToString();
            txtPresentAddress.ToolTip = dsCandidate.Tables[0].Rows[0]["CurrentAddress"].ToString();
            txtAreasOfInterest.ToolTip = dsCandidate.Tables[0].Rows[0]["AreaOfInterest"].ToString();
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    private void UploadFiles(string candidateID)
    {
        try
        {
            //string filePath = @"D:\Recruitment ResumesDemo\";
            string filePath = ConfigurationSettings.AppSettings["SmarttrackUploadedfilePath"].ToString();

            if (!Directory.Exists((filePath)))
            {
                Directory.CreateDirectory((filePath));
            }
            string fname = uploadFiles.PostedFile.FileName;

            if (fname != "")
            {
                string extension = Path.GetExtension(fname);
                completeFilePath = filePath + candidateID + extension;

                if ((extension == ".doc") || (extension == ".docx"))
                {
                    DataSet dsCandidateFileUploaded = new DataSet();

                    if (File.Exists(filePath + candidateID + ".docx"))
                        File.Delete(filePath + candidateID + ".docx");

                    if (File.Exists(filePath + candidateID + ".doc"))
                        File.Delete(filePath + candidateID + ".doc");

                    //uploadFiles.PostedFile.SaveAs(completeFilePath);
                    uploadFiles.PostedFile.SaveAs((completeFilePath));
                    lblSuccessMsg.Text = "Your file was uploaded successfully.";
                    candidateBOL.CandidateID = Convert.ToInt32(candidateID);
                    candidateBOL.IsFileUploaded = true;
                    dsCandidateFileUploaded = candidateBLL.UpdateCandidateFileUploadStatus(candidateBOL);

                    //}
                }
                //else
                //{
                //    lblErrorMsg.Text = "Please upload file with .doc or .docx extension";
                //}
                //}
            }
            else
            {
                lblErrorMsg.Text = "Please select file to upload";
            }
        }
        catch (System.Exception ex)
        {
            throw new MyException(" Message:" + ex.Message, ex.InnerException);
        }
    }

    private bool CheckFileForCorruptData(string file)
    {
        try
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(file, true))
            {
                return false;
            }

            //  wordDoc.Dispose();
        }
        catch (System.Exception ex)
        {
            lblErrorMsg.Text = ex.Message;
            return true;
        }
    }

    #endregion
}