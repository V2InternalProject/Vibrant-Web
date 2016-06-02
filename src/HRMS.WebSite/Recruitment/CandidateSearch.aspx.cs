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

public partial class CandidateSearch : System.Web.UI.Page
{
    private DataSet dsCandidateStatus = new DataSet();
    private DataSet dsCandidateQualification = new DataSet();
    private static DataSet dsCandidateSearchResults = new DataSet();
    private DataSet dsKeywordSearch = new DataSet();
    private DropDownList ddlCandidateStatus;
    private DropDownList ddlCandidateQualification;

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    private CandidateSearchBLL objCandidateSearchBLL = new CandidateSearchBLL();
    private CandidateSearchBOL objCandidateSearchBOL = new CandidateSearchBOL();

    private RRFStatusBLL objRRFStatusBLL = new RRFStatusBLL();
    private RRFStatusBOL objRRFStatusBOL = new RRFStatusBOL();
    private DataSet dsGetRFFNo = new DataSet();

    //useRRFNo field to Read RRFNO. from response.redirect
    private int RRFID;

    private string path = System.Configuration.ConfigurationManager.AppSettings["SmarttrackUploadedfilePath"].ToString();
    private string searchKeyWord = "--";
    private string rrfNo = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        rrfNo = (Convert.ToString(Session["RRFID"]) ?? "").Trim();

        if ((Session["RRFNo"] != null) && (Session["RRFNo"].ToString() != ""))
        {
            lblRRFNO.Text = "RRF NO   :  " + Session["RRFNo"].ToString();

            objRRFStatusBOL.RRFNo = Convert.ToInt32(rrfNo);

            dsGetRFFNo = objRRFStatusBLL.GetRRFNo(objRRFStatusBOL);

            if (dsGetRFFNo != null)
            {
                // lblRRFNO.Text = (dsGetRFFNo.Tables[0].Rows[0]["RRFNo"].ToString());
                lblRRFNO.Text = Session["RRFNo"].ToString();
                lblRequestor.Text = Convert.ToString(dsGetRFFNo.Tables[0].Rows[0]["EmployeeName"]);

                lblPosition.Text = Convert.ToString(dsGetRFFNo.Tables[0].Rows[0]["Designationname"]);
                DateTime posteddate = Convert.ToDateTime(dsGetRFFNo.Tables[0].Rows[0]["PostedDate"]);
                lblPostedDate.Text = posteddate.Date.ToString("MM/dd/yyyy");
                lblResourcePoolName.Text = Convert.ToString(dsGetRFFNo.Tables[0].Rows[0]["ResourcePoolName"]);
            }
        }
        else
        {
            lblRRFNO.Visible = false;
        }
        lblError.Visible = false;
        // path = Server.MapPath("~/Resumes/");
        if (Session["RRFID"] != null)
            RRFID = Convert.ToInt32(Convert.ToString(Session["RRFID"]));
        else
            RRFID = 1000;
        // RRFID = Convert.ToInt32(Request.QueryString["RRFID"]);

        if (!Page.IsPostBack)
        {
            BindData();
        }
    }

    public void BindData()
    {
        DataTable dtTemp = new DataTable();
        //String status;
        //String qualification;

        //Add a blank row into the grid and set visibility as false.
        //This will show the footer row.
        dtTemp.Rows.Add(dtTemp.NewRow());
        grdCandidateSearch.DataSource = dtTemp;
        grdCandidateSearch.DataBind();
        grdCandidateSearch.Rows[0].Visible = false;

        pnlAction.Visible = false;

        //Populate ddlCandidateStatus
        dsCandidateStatus = objCandidateSearchBLL.GetCandidateStatus();
        ddlCandidateStatus = (DropDownList)grdCandidateSearch.FooterRow.Cells[6].Controls[1];
        ddlCandidateStatus.Items.Add("Select");

        for (int i = 0; i < dsCandidateStatus.Tables[0].Rows.Count; i++)
        {
            int a = Convert.ToInt32(dsCandidateStatus.Tables[0].Rows[i]["ID"]);
            if ((a == 1) || (a == 2) || (a == 8) || (a == 11) || (a == 15))     //Candidate Status == New OR Active OR OfferRejected OR Withdrawn OR Rejected
            {
                ddlCandidateStatus.Items.Add(new ListItem(dsCandidateStatus.Tables[0].Rows[i]["CandidateStatus"].ToString(), dsCandidateStatus.Tables[0].Rows[i]["ID"].ToString()));
            }
            //status = dsCandidateStatus.Tables[0].Rows[i][dsCandidateStatus.Tables[0].Columns[1].ColumnName].ToString();
            //ddlCandidateStatus.Items.Add(status);
        }

        //Populate ddlCandidateQualification
        dsCandidateQualification = objCandidateSearchBLL.GetCandidateQualification();
        ddlCandidateQualification = (DropDownList)grdCandidateSearch.FooterRow.Cells[3].Controls[1];
        ddlCandidateQualification.Items.Add("Select");
        for (int i = 0; i < dsCandidateQualification.Tables[0].Rows.Count; i++)
        {
            ddlCandidateQualification.Items.Add(new ListItem(dsCandidateQualification.Tables[0].Rows[i]["QualificationName"].ToString(), dsCandidateQualification.Tables[0].Rows[i]["QualificationID"].ToString()));
            //qualification = dsCandidateQualification.Tables[0].Rows[i][dsCandidateQualification.Tables[0].Columns[1].ColumnName].ToString();
            //ddlCandidateQualification.Items.Add(qualification);
        }
    }

    public void CandidateSearchBindData(DataSet dsCandidateSearchResults)
    {
        //Check if search results are empty or not
        if (dsCandidateSearchResults.Tables[0].Rows.Count == 0)
        {
            //Show lblError if no entries found
            lblError.Visible = true;
            lblError.Text = "No Matching Entries";
            grdSearchResults.Visible = false;
            pnlAction.Visible = false;
            btnBack.Visible = false;
            btnRedirect.Visible = true;
        }
        else
        {
            lblError.Visible = false;
            grdSearchResults.Visible = true;
            grdSearchResults.DataSource = dsCandidateSearchResults;
            grdSearchResults.DataBind();
            pnlAction.Visible = true;
            btnBack.Visible = false;
            btnRedirect.Visible = true;
        }
    }

    protected void grdCandidateSearch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //Get Search Data from the textboxes.
            lblError.Visible = false;
            TextBox name = (TextBox)grdCandidateSearch.FooterRow.FindControl("txtName");
            string name1 = name.Text;
            string[] sepereted = name1.Split(' ');
            foreach (var i in sepereted)
            {
                Label lb = new Label();
                lb.Text = i;
            }

            TextBox fromyears = (TextBox)grdCandidateSearch.FooterRow.FindControl("txtFromYears");
            TextBox uptoyears = (TextBox)grdCandidateSearch.FooterRow.FindControl("txtUptoYears");
            DropDownList qualification = (DropDownList)grdCandidateSearch.FooterRow.FindControl("ddlQualification");
            TextBox notice = (TextBox)grdCandidateSearch.FooterRow.FindControl("txtNoticePeriod");
            DropDownList status = (DropDownList)grdCandidateSearch.FooterRow.FindControl("ddlStatus");
            TextBox keyword = (TextBox)grdCandidateSearch.FooterRow.FindControl("txtKeyword");

            if (name.Text != String.Empty)
                objCandidateSearchBOL.Name = Convert.ToString(name.Text);
            else
                objCandidateSearchBOL.Name = null;

            if (fromyears.Text != String.Empty)
                objCandidateSearchBOL.Years = Convert.ToInt32(fromyears.Text);
            else
                objCandidateSearchBOL.Years = -1;

            if (uptoyears.Text != String.Empty)
                objCandidateSearchBOL.UptoYears = Convert.ToInt32(uptoyears.Text);
            else
                objCandidateSearchBOL.UptoYears = -1;

            if (qualification.Text != "Select")
                objCandidateSearchBOL.Qualifications = Convert.ToInt32(qualification.SelectedItem.Value);
            else
                objCandidateSearchBOL.Qualifications = -1;

            if (notice.Text != String.Empty)
                objCandidateSearchBOL.NoticePeriod = Convert.ToInt32(notice.Text);
            else
                objCandidateSearchBOL.NoticePeriod = -1;

            if (status.Text != "Select")
                objCandidateSearchBOL.Status = Convert.ToInt32(status.SelectedItem.Value);
            else
                objCandidateSearchBOL.Status = -1;

            objCandidateSearchBOL.RRFID = RRFID;

            //if ((fromyears.Text != string.Empty && uptoyears.Text != string.Empty) || (fromyears.Text == string.Empty && uptoyears.Text == string.Empty))
            //{
            dsCandidateSearchResults = objCandidateSearchBLL.GetCandidateSearchResults(objCandidateSearchBOL);
            CandidateSearchBindData(dsCandidateSearchResults);

            string keywordsearch = keyword.Text;
            DataTable dtTempSerchResults = new DataTable();
            dtTempSerchResults = dsCandidateSearchResults.Tables[0].Clone();

            if (keyword.Text != string.Empty)
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

            //}
            //else
            //{
            //    global::System.Windows.Forms.MessageBox.Show("Please enter the correct range for work experience.\n Kindly enter both TO and FROM values.", "Recruitment module");
            //    if (fromyears.Text == string.Empty)
            //        fromyears.Focus();
            //    else
            //        uptoyears.Focus();

            //}
        }
        catch (System.Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.Message;
            btnBack.Visible = false;
            btnRedirect.Visible = true;
            throw new MyException("Exception in  btnSave_Click(). Message:" + ex.Message, ex.InnerException);
        }
        //catch (System.Exception ex)
        //{
        //    lblError.Visible = true;
        //    lblError.Text = ex.Message;
        //    btnBack.Visible = false;
        //    btnRedirect.Visible = true;
        //}
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
    private List<string> GetMatchedFilesWithSearchKeyWord()
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
            throw new MyException("Exception in  btnSave_Click(). Message:" + ex.Message, ex.InnerException);
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

    protected void grdSearchResults_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //lblError.Visible = false;
        //if (e.CommandName == "OpenResume")
        //{
        //    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //    Label lblID = (Label)grdSearchResults.Rows[gvr.RowIndex].FindControl("lblID");
        //    string filename = lblID.Text + ".docx";

        //    FileInfo myDoc = new FileInfo(path + filename);
        //    if (myDoc.Exists)
        //    {
        //        Response.Clear();
        //        Response.ContentType = "Application/msword";
        //        Response.AddHeader("content-disposition", "attachment;filename=" + myDoc.Name);
        //        Response.AddHeader("Content-Length", myDoc.Length.ToString());
        //        Response.ContentType = "application/octet-stream";
        //        Response.WriteFile(myDoc.FullName);
        //        Response.End();
        //    }
        //    else
        //    {
        //        //System.Windows.Forms.MessageBox.Show("No such File Exists");
        //        lblError.Visible = true;
        //        lblError.Text = "No such File Exists";
        //    }
        //}
        //else if (e.CommandName == "InitiateRecruitment")
        //{
        //    lblError.Visible = false;
        //    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //    Label lblID = (Label)grdSearchResults.Rows[gvr.RowIndex].FindControl("lblID");
        //    LinkButton init = (LinkButton)grdSearchResults.Rows[gvr.RowIndex].FindControl("lnkInitiateRecruitment");

        //    Response.Redirect("~/CandidateInterviewSchedule.aspx?CandidateID=" + lblID.Text + "&RRFID=" + RRFID);

        //}
    }

    protected void grdSearchResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSearchResults.PageIndex = e.NewPageIndex;
        grdSearchResults.DataSource = dsCandidateSearchResults;
        grdSearchResults.DataBind();
        btnBack.Visible = false;
        btnRedirect.Visible = true;
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

        grdSearchResults.DataSource = dv;
        grdSearchResults.DataBind();
    }

    protected void grdSearchResults_Sorting(object sender, GridViewSortEventArgs e)
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

    protected void btnOpenResume_Click(object sender, EventArgs e)
    {
        // lblError.Visible = false;
        foreach (GridViewRow gvRow in grdSearchResults.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                try
                {
                    //  path = Server.MapPath("~/Resumes/");
                    Label lblCandidateID = (Label)gvRow.FindControl("lblID");

                    string filename1 = lblCandidateID.Text + ".docx";
                    string filename2 = lblCandidateID.Text + ".doc";

                    FileInfo myDoc = new FileInfo(path + filename1);
                    FileInfo myDoc2 = new FileInfo(path + filename2);

                    if (myDoc.Exists)
                    {
                    }
                    else
                    {
                        myDoc = myDoc2;
                    }

                    if (myDoc.Exists)
                    {
                        try
                        {
                            Response.Clear();

                            Response.ContentType = "Application/msword";
                            Response.AddHeader("content-disposition", "attachment;filename=" + myDoc.Name);
                            Response.AddHeader("Content-Length", myDoc.Length.ToString());
                            Response.ContentType = "application/octet-stream";
                            Response.BufferOutput = true;
                            Response.WriteFile(myDoc.FullName);
                            Response.End();
                        }
                        catch (System.Exception ex)
                        {
                            throw new MyException("Exception in  btnSave_Click(). Message:" + ex.Message, ex.InnerException);
                        }
                        finally
                        {
                            lblError.Visible = false;
                        }
                    }
                    else
                    {
                        //lblError.Visible = true;
                        //lblError.Text = "No Resume for this candidate was found.";
                        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "javascript:show_confirm();", true);
                        btnBack.Visible = false;
                        btnRedirect.Visible = true;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    //throw ex;
                }
            }
        }
    }

    protected void btnInitiateRecruitment_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in grdSearchResults.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                try
                {
                    lblError.Visible = false;
                    //GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblCandidateID = (Label)gvRow.FindControl("lblID");
                    int CandidateID = Convert.ToInt32(lblCandidateID.Text);
                    objCandidateSearchBLL.ChangeCandidateStatus(CandidateID, RRFID);
                    Session["CandidateID"] = lblCandidateID.Text;
                    Session["RRFID"] = RRFID;

                    //Response.Redirect("~/CandidateInterviewSchedule.aspx?CandidateID=" + lblCandidateID.Text + "&RRFID=" + RRFID);
                    Response.Redirect("~/Recruitment/CandidateInterviewSchedule.aspx");
                }
                catch (FileNotFoundException ex)
                {
                    //  throw ex;
                }
            }
        }
    }

    protected void btnRedirect_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Recruitment/Recruiter.aspx");
    }

    protected void grdCandidateSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}