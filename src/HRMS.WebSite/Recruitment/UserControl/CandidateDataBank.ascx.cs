using BLL;
using BOL;
using DocumentFormat.OpenXml.Packaging;
using HRMS.Recruitment.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS.Recruitment.UserControl
{
    public partial class CandidateDataBank : System.Web.UI.UserControl
    {
        #region Declaration

        private DataSet dsCandidateStatus = new DataSet();
        private DataSet dsCandidateQualification = new DataSet();
        private static DataSet dsCandidateSearchResults = new DataSet();
        private DataSet dsKeywordSearch = new DataSet();
        private DropDownList ddlCandidateStatus;
        private DropDownList ddlCandidateQualification;

        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC";

        private CandidateDataBankBLL objCandidateDataBankBLL = new CandidateDataBankBLL();
        private CandidateDataBankBOL objCandidateDataBankBOL = new CandidateDataBankBOL();

        private string path = System.Configuration.ConfigurationManager.AppSettings["SmarttrackUploadedfilePath"].ToString();

        private string searchKeyWord = "--";

        #endregion Declaration

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnOpenResume);

            lblErrorMessage.Text = string.Empty;
            lblErrorMessage.Visible = false;
            lblSuccessMessage.Text = string.Empty;

            //For checking role of Current User
            if (Session["RecruiterRole"] != null)
            {
                btnDelete.Visible = false;
            }

            if (!Page.IsPostBack)
                BindData();
        }

        protected void grdCandidateSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //Get Search Data from the textboxes.
                TextBox txtName = (TextBox)grdCandidateSearch.FooterRow.FindControl("txtName");

                TextBox txtFromYears = (TextBox)grdCandidateSearch.FooterRow.FindControl("txtFromYears");
                TextBox txtUptoYears = (TextBox)grdCandidateSearch.FooterRow.FindControl("txtUptoYears");
                DropDownList ddlQualification = (DropDownList)grdCandidateSearch.FooterRow.FindControl("ddlQualification");
                TextBox txtNotice = (TextBox)grdCandidateSearch.FooterRow.FindControl("txtNoticePeriod");
                DropDownList ddlStatus = (DropDownList)grdCandidateSearch.FooterRow.FindControl("ddlStatus");
                TextBox txtKeyword = (TextBox)grdCandidateSearch.FooterRow.FindControl("txtKeyword");

                if (txtName.Text != String.Empty)
                    objCandidateDataBankBOL.FirstName = Convert.ToString(txtName.Text);
                else
                    objCandidateDataBankBOL.FirstName = null;

                if (txtFromYears.Text != String.Empty)
                    objCandidateDataBankBOL.Years = Convert.ToInt32(txtFromYears.Text);
                else
                    objCandidateDataBankBOL.Years = -1;

                if (txtUptoYears.Text != String.Empty)
                    objCandidateDataBankBOL.UptoYears = Convert.ToInt32(txtUptoYears.Text);
                else
                    objCandidateDataBankBOL.UptoYears = -1;

                if (ddlQualification.Text != "Select")
                    objCandidateDataBankBOL.Qualifications = Convert.ToInt32(ddlQualification.SelectedItem.Value);
                else
                    objCandidateDataBankBOL.Qualifications = -1;

                if (txtNotice.Text != String.Empty)
                    objCandidateDataBankBOL.NoticePeriod = Convert.ToInt32(txtNotice.Text);
                else
                    objCandidateDataBankBOL.NoticePeriod = -1;

                if (ddlStatus.Text != "Select")
                    objCandidateDataBankBOL.Status = Convert.ToInt32(ddlStatus.SelectedItem.Value);
                else
                    objCandidateDataBankBOL.Status = -1;

                dsCandidateSearchResults = objCandidateDataBankBLL.GetCandidateSearchResults(objCandidateDataBankBOL);
                CandidateSearchBindData(dsCandidateSearchResults);

                string keywordsearch = txtKeyword.Text;
                DataTable dtTempSerchResults = new DataTable();
                dtTempSerchResults = dsCandidateSearchResults.Tables[0].Clone();

                if (txtKeyword.Text != string.Empty)
                {
                    //Spilt input string for multiword search
                    string[] keys = keywordsearch.Split(',');
                    for (int i = 0; i < keys.Length; i++)
                    {
                        keys[i] = keys[i].Trim();
                        if (keys[i].Equals(string.Empty))
                        { continue; }
                        searchKeyWord = @keys[i];
                        SearchValidCandidates();
                        for (int j = 0; j < dsKeywordSearch.Tables[0].Rows.Count; j++)
                        {
                            dtTempSerchResults.ImportRow(dsKeywordSearch.Tables[0].Rows[j]);
                        }
                    }

                    try
                    {
                        dsKeywordSearch.Tables.RemoveAt(0);
                        dsKeywordSearch.Tables.Add(dtTempSerchResults);
                    }
                    catch { }
                    CandidateSearchBindData(dsKeywordSearch);
                }
            }
            catch (System.Exception ex)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = ex.Message;
                grdCandidates.Visible = false;
            }
        }

        protected void grdCandidates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void grdCandidates_Sorting(object sender, GridViewSortEventArgs e)
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

        protected void grdCandidates_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCandidates.PageIndex = e.NewPageIndex;
            grdCandidates.DataSource = dsCandidateSearchResults;
            grdCandidates.DataBind();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            bool deleted = false;
            foreach (GridViewRow gvRow in grdCandidates.Rows)
            {
                Label lblCandidateStatus = (Label)gvRow.FindControl("lblCandidateStatus");
                CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
                Label lblCandidateID = (Label)gvRow.FindControl("lblCandidateID");
                bool allowToDelete = false;

                if (chkSelect.Checked && (lblCandidateStatus.Text.ToLower() == "new" || lblCandidateStatus.Text.ToLower() == "active" || lblCandidateStatus.Text.ToLower() == "blacklisted" || lblCandidateStatus.Text.ToLower() == "cancelled" || lblCandidateStatus.Text.ToLower() == "offer rejected" || lblCandidateStatus.Text.ToLower() == "withdrawn" || lblCandidateStatus.Text.ToLower() == "rejected"))
                {
                    allowToDelete = true;
                }

                if (allowToDelete)
                {
                    DataSet dsTemp = new DataSet();

                    objCandidateDataBankBOL.CandidateID = Convert.ToInt32(lblCandidateID.Text.Trim());
                    dsTemp = objCandidateDataBankBLL.DeleteCandidateSkillsByCandidateID(objCandidateDataBankBOL);
                    dsTemp = objCandidateDataBankBLL.DeleteCandidateExperienceDetailsByCandidateID(objCandidateDataBankBOL);
                    dsTemp = objCandidateDataBankBLL.DeleteCandidateEducationDetailsByCandidateID(objCandidateDataBankBOL);
                    dsTemp = objCandidateDataBankBLL.DeleteCandidateCertificationDetailsByCandidateID(objCandidateDataBankBOL);
                    dsTemp = objCandidateDataBankBLL.DeleteCandidate(objCandidateDataBankBOL);

                    deleted = true;

                    GridViewRow row = grdCandidateSearch.FooterRow;
                    TextBox txtName = (TextBox)row.FindControl("txtName");
                    TextBox txtFromYears = (TextBox)row.FindControl("txtFromYears");
                    TextBox txtUptoYears = (TextBox)row.FindControl("txtUptoYears");
                    DropDownList ddlQualification = (DropDownList)row.FindControl("ddlQualification");
                    TextBox txtNotice = (TextBox)row.FindControl("txtNoticePeriod");
                    DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");
                    TextBox txtKeyword = (TextBox)row.FindControl("txtKeyword");

                    if (txtName.Text != String.Empty)
                        objCandidateDataBankBOL.FirstName = Convert.ToString(txtName.Text);
                    else
                        objCandidateDataBankBOL.FirstName = null;

                    if (txtFromYears.Text != String.Empty)
                        objCandidateDataBankBOL.Years = Convert.ToInt32(txtFromYears.Text);
                    else
                        objCandidateDataBankBOL.Years = -1;

                    if (txtUptoYears.Text != String.Empty)
                        objCandidateDataBankBOL.UptoYears = Convert.ToInt32(txtUptoYears.Text);
                    else
                        objCandidateDataBankBOL.UptoYears = -1;

                    if (ddlQualification.Text != "Select")
                        objCandidateDataBankBOL.Qualifications = Convert.ToInt32(ddlQualification.SelectedItem.Value);
                    else
                        objCandidateDataBankBOL.Qualifications = -1;

                    if (txtNotice.Text != String.Empty)
                        objCandidateDataBankBOL.NoticePeriod = Convert.ToInt32(txtNotice.Text);
                    else
                        objCandidateDataBankBOL.NoticePeriod = -1;

                    if (ddlStatus.Text != "Select")
                        objCandidateDataBankBOL.Status = Convert.ToInt32(ddlStatus.SelectedItem.Value);
                    else
                        objCandidateDataBankBOL.Status = -1;

                    lblSuccessMessage.Text = "Candidate has been deleted successfully!";
                    lblSuccessMessage.Visible = true;
                    lblErrorMessage.Visible = false;

                    dsCandidateSearchResults = objCandidateDataBankBLL.GetCandidateSearchResults(objCandidateDataBankBOL);
                    CandidateSearchBindData(dsCandidateSearchResults);

                    string keywordsearch = txtKeyword.Text;
                    DataTable dtTempSerchResults = new DataTable();
                    dtTempSerchResults = dsCandidateSearchResults.Tables[0].Clone();

                    if (txtKeyword.Text != string.Empty)
                    {
                        //Spilt input string for multiword search
                        string[] keys = keywordsearch.Split(',');
                        for (int i = 0; i < keys.Length; i++)
                        {
                            keys[i] = keys[i].Trim();
                            if (keys[i].Equals(string.Empty))
                            { continue; }
                            searchKeyWord = @keys[i];
                            SearchValidCandidates();
                            for (int j = 0; j < dsKeywordSearch.Tables[0].Rows.Count; j++)
                            {
                                dtTempSerchResults.ImportRow(dsKeywordSearch.Tables[0].Rows[j]);
                            }
                        }

                        try
                        {
                            dsKeywordSearch.Tables.RemoveAt(0);
                            dsKeywordSearch.Tables.Add(dtTempSerchResults);
                        }
                        catch { }
                        CandidateSearchBindData(dsKeywordSearch);
                    }
                }

                if (!deleted)
                {
                    lblErrorMessage.Text = "The selected candidate cannot be deleted as candidate is assigned to RRF.";
                    lblErrorMessage.Visible = true;
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in grdCandidates.Rows)
            {
                CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Session["editClick"] = "true";
                    Label lblCandidateID = (Label)gvRow.FindControl("lblCandidateID");

                    Session["ID"] = lblCandidateID.Text;
                    Response.Redirect("Candidate.aspx");
                }
            }
        }

        protected void btnOpenResume_Click(object sender, EventArgs e)
        {
            Label lblCandidateID = null;
            foreach (GridViewRow gvRow in grdCandidates.Rows)
            {
                CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    try
                    {
                        lblCandidateID = (Label)gvRow.FindControl("lblCandidateID");
                    }
                    catch (FileNotFoundException ex)
                    {
                        throw ex;
                    }
                }
            }
            if (lblCandidateID != null)
            {
                string filename1 = lblCandidateID.Text + ".docx";
                string filename2 = lblCandidateID.Text + ".doc";

                FileInfo myDoc = new FileInfo((path + filename1));
                FileInfo myDoc2 = new FileInfo((path + filename2));

                if (!myDoc.Exists)
                {
                    myDoc = myDoc2;
                }

                if (myDoc.Exists)
                {
                    Response.Clear();
                    Response.AppendHeader("content-disposition", "attachment;filename=" + myDoc.Name);
                    Response.AppendHeader("Content-Length", myDoc.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.BufferOutput = true;
                    Response.WriteFile(myDoc.FullName);
                    Response.End();
                }
                else
                {
                    string msg = "No Resume for this candidate was found.";
                    ScriptManager.RegisterStartupScript(pnlCandidates, pnlCandidates.GetType(), "alert", "javascript:show_confirm('" + msg + "');", true);
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("Candidate.aspx", false);
        }

        protected void btnViewProfile_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in grdCandidates.Rows)
            {
                CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Session["viewProfileClick"] = "true";
                    Label lblCandidateID = (Label)gvRow.FindControl("lblCandidateID");
                    Session["ID"] = lblCandidateID.Text.Trim();
                    Session["mode"] = btnViewProfile.Text.Trim();
                    Response.Redirect("Candidate.aspx");
                }
            }
        }

        #endregion Events

        #region Custom Methods

        public void BindData()
        {
            DataTable dtTemp = new DataTable();

            //Add a blank row into the grid and set visibility as false.
            //This will show the footer row.
            dtTemp.Rows.Add(dtTemp.NewRow());
            grdCandidateSearch.DataSource = dtTemp;
            grdCandidateSearch.DataBind();
            grdCandidateSearch.Rows[0].Visible = false;

            //Populate ddlCandidateStatus
            dsCandidateStatus = objCandidateDataBankBLL.GetCandidateStatus();
            ddlCandidateStatus = (DropDownList)grdCandidateSearch.FooterRow.Cells[5].Controls[1];
            ddlCandidateStatus.Items.Add("Select");

            for (int i = 0; i < dsCandidateStatus.Tables[0].Rows.Count; i++)
            {
                ddlCandidateStatus.Items.Add(new ListItem(dsCandidateStatus.Tables[0].Rows[i]["CandidateStatus"].ToString(), dsCandidateStatus.Tables[0].Rows[i]["ID"].ToString()));
            }

            //Populate ddlCandidateQualification
            dsCandidateQualification = objCandidateDataBankBLL.GetCandidateQualification();
            ddlCandidateQualification = (DropDownList)grdCandidateSearch.FooterRow.Cells[2].Controls[1];
            ddlCandidateQualification.Items.Add("Select");
            for (int i = 0; i < dsCandidateQualification.Tables[0].Rows.Count; i++)
            {
                ddlCandidateQualification.Items.Add(new ListItem(dsCandidateQualification.Tables[0].Rows[i]["QualificationName"].ToString(), dsCandidateQualification.Tables[0].Rows[i]["QualificationID"].ToString()));
            }
        }

        public void CandidateSearchBindData(DataSet dsCandidateSearchResults)
        {
            //Check if search results are empty or not
            if (dsCandidateSearchResults.Tables[0].Rows.Count == 0)
            {
                //Show lblError if no entries found
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "No Matching Entries";
                grdCandidates.Visible = false;
                pnlAction1.Visible = false;
            }
            else
            {
                lblErrorMessage.Visible = false;
                grdCandidates.Visible = true;
                pnlAction1.Visible = true;
                grdCandidates.DataSource = dsCandidateSearchResults;
                grdCandidates.DataBind();
            }
        }

        private void SearchValidCandidates()
        {
            string candidate_id;
            DataTable dtSearchResults = new DataTable();

            dtSearchResults = dsCandidateSearchResults.Tables[0].Clone();

            List<string> fileNames = GetMatchedFilesWithSearchKeyWord();
            if (fileNames != null && fileNames.Count > 0)
            {
                for (int check = 0; check < (dsCandidateSearchResults.Tables[0].Rows.Count); check++)
                {
                    candidate_id = dsCandidateSearchResults.Tables[0].Rows[check]["ID"].ToString();
                    foreach (var file in fileNames)
                    {
                        if (file == candidate_id)
                        {
                            dtSearchResults.ImportRow(dsCandidateSearchResults.Tables[0].Rows[check]);
                        }
                    }
                }

                try
                {
                    if (dsKeywordSearch.Tables[0].TableName == "temp")
                        dsKeywordSearch.Tables.Remove("temp");
                }
                catch
                {
                    //Continue with execution without handling the error if unable to remove the table.
                }

                dsKeywordSearch.Tables.Add(dtSearchResults);
                dsKeywordSearch.Tables[0].TableName = "temp";
            }
            else
            {
                try
                {
                    if (dsKeywordSearch.Tables[0].TableName == "temp")
                        dsKeywordSearch.Tables.Remove("temp");
                }
                catch
                {
                    //Continue with execution without handling the error if unable to remove the table.
                }

                dsKeywordSearch.Tables.Add();
                dsKeywordSearch.Tables[0].TableName = "temp";
            }
        }

        /// <summary>
        /// Gets Matched files with particular searchKeyWord
        /// </summary>
        /// <returns></returns>
        private List<string> GetMatchedFilesWithSearchKeyWord() //ToDo
        {
            try
            {
                List<string> listOfSearchPatterns = new List<string>();
                listOfSearchPatterns.Add("*.doc");
                List<string> matchingFiles = GetFilesUsingLINQ(path, listOfSearchPatterns);

                List<string> matchedFilesForSearchedkeyWord = new List<string>();

                foreach (var files in matchingFiles)
                {
                    string extension = Path.GetExtension(files);
                    if (extension == ".doc")
                    {
                        if (SearchWordIsMatchedInDocFile(files, searchKeyWord))
                        {
                            string fileNameWithKeyWord = files.Split('\\').Last().Split('.').First();
                            matchedFilesForSearchedkeyWord.Add(fileNameWithKeyWord);
                        }
                    }
                    else if (extension == ".docx")
                    {
                        if (SearchWordIsMatchedInDocxFile(files, searchKeyWord))
                        {
                            string fileNameWithKeyWord = files.Split('\\').Last().Split('.').First();
                            matchedFilesForSearchedkeyWord.Add(fileNameWithKeyWord);
                        }
                    }
                }
                return matchedFilesForSearchedkeyWord;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  Searches for specified searchKeyWord in Doc file in specified path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchKeyWord"></param>
        /// <returns></returns>
        private bool SearchWordIsMatchedInDocFile(string path, string searchKeyWord)
        {
            try
            {
                OfficeFileReaderNew objOFR = new OfficeFileReaderNew();
                string text = string.Empty;
                objOFR.GetText(path, ref text);
                if (text.ToLower().Contains(searchKeyWord.ToLower()))
                {
                    return true;
                }
                else
                    return false;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Searches for specified searchKeyWord in Docx file in specified path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchKeyWord"></param>
        /// <returns></returns>
        private bool SearchWordIsMatchedInDocxFile(string path, string searchKeyWord)
        {
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, true))
                {
                    var text = wordDoc.MainDocumentPart.Document.InnerText;
                    if (text.ToLower().Contains(searchKeyWord.ToLower()))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets files for specific extension name
        /// </summary>
        /// <param name="path"></param>
        /// <param name="listOfSearchPatterns"></param>
        /// <returns></returns>
        public static List<string> GetFilesUsingLINQ(string path, List<string> listOfSearchPatterns)
        {
            try
            {
                List<string> matchingFiles = new List<string>();

                foreach (string s in listOfSearchPatterns)
                {
                    var files = from f in Directory.GetFiles(path, s)
                                select f;

                    //add the files to our list
                    matchingFiles.AddRange(files);
                }

                return matchingFiles;
            }
            catch (System.Exception ex)
            {
                throw new MyException("Exception in  btnSave_Click(). Message:" + ex.Message, ex.InnerException);
            }
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
            //  You can cache the DataTable for improving performance

            DataTable dt = dsCandidateSearchResults.Tables[0] as DataTable;

            DataView dv = new DataView(dt);
            dv.Sort = sortExpression + direction;

            dt = dv.ToTable();
            DataSet sortedDs = new DataSet();
            sortedDs.Tables.Add(dt.Copy());

            dsCandidateSearchResults = sortedDs;
            grdCandidates.DataSource = dv;
            grdCandidates.DataBind();
        }

        #endregion Custom Methods
    }
}