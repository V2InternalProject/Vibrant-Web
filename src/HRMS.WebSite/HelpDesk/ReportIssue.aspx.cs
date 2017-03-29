using HRMS.DAL;
using ModelHelpDeskBranch;
using System;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Helpdesk.BusinessLayer;
using V2.Helpdesk.web;

namespace HRMS.HelpDesk
{
    public partial class ReportIssue : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.TextBox txtEmailID;
        protected System.Web.UI.WebControls.TextBox txtCCEmailID;
        protected System.Web.UI.WebControls.TextBox txtPhoneExtension;
        protected System.Web.UI.WebControls.TextBox txtSeatingLocation;
        protected System.Web.UI.WebControls.DropDownList ddlSeverity;
        protected System.Web.UI.HtmlControls.HtmlTextArea txtDescription;
        protected System.Web.UI.WebControls.Button btnReset;
        protected System.Web.UI.WebControls.Button btnSubmit;
        protected System.Web.UI.WebControls.DropDownList ddltype;
        protected System.Web.UI.WebControls.DropDownList ddlProjectName;
        protected System.Web.UI.WebControls.DropDownList ddlProjectRole;
        protected System.Web.UI.WebControls.DropDownList ddlResourcePool;
        protected System.Web.UI.WebControls.DropDownList ddlReportingTo;
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.Label lblCcEmailError;
        protected System.Web.UI.WebControls.TextBox txtName;
        protected System.Web.UI.WebControls.Label lblMailError;
        protected System.Web.UI.WebControls.TextBox txtWorkHours;
        protected System.Web.UI.WebControls.TextBox txtNoOfResources;
        protected System.Web.UI.WebControls.TextBox txtFromDate;
        protected System.Web.UI.WebControls.TextBox txtEndDate;
        private DataSet dsCategoryID = new DataSet();
        public string strMemberEmailID;
        private string strDeptEmailID, strDeptCCEmailID;
        public int intIssueID, EmployeeID;
        public DataSet dsIssueIDAndMemberEmailID, IssueSubmitResult;
        protected System.Web.UI.HtmlControls.HtmlInputFile uploadFiles;
        public int ShouldSendMail, ShouldUploadFile;
        public string strAssignedTo;
        private string fn, fnExt;
        protected System.Web.UI.HtmlControls.HtmlSelect ddlSubCategories;
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();
        private CommonMethods common = new CommonMethods();

        public class ReportingTo
        {
            public int EmployeeId { get; set; }

            public string EmployeeName { get; set; }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            lblError.Text = ""; lblMessage.Text = "";
            string PageName = "HelpDesk";
            objpagelevel.PageLevelAccess(PageName);

            EmployeeID = Convert.ToInt32(User.Identity.Name);
            clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
            DataSet dsEmail = objClsBLReportIssue.GetEmailID(EmployeeID);
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtFromDate.Text = (DateTime.Now).ToShortDateString();
            txtEndDate.Attributes.Add("readonly", "readonly");
            if (dsEmail.Tables[0].Rows.Count > 0)
            {
                txtEmailID.Text = dsEmail.Tables[0].Rows[0][0].ToString();
                txtName.Text = dsEmail.Tables[0].Rows[0]["UserName"].ToString();
            }
            if (EmployeeID.ToString() == "" || EmployeeID == 0)
            {
                //Response.Redirect("http://192.168.30.15/intranet/");
                Response.Redirect("http://myvibrantweb.v2solutions.com");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    try
                    {
                        lblError.Text = "";
                        BindType();
                        BindSeverity();
                        BindCategory();
                        BindProjectName();
                        BindProjectRole();
                        BindResourcePool();
                    }
                    catch (V2Exceptions ex)
                    {
                        throw;
                    }
                    catch (System.Exception ex)
                    {
                        FileLog objFileLog = FileLog.GetLogger();
                        objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "Page_Load", ex.StackTrace);
                        throw new V2Exceptions(ex.ToString(), ex);
                    }
                }
            }
            //btnSubmit.Attributes.Add("onClick", "return validateAndCheck();");
        }

        private void BindSeverity()
        {
            try
            {
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                DataSet dsSeverity = objClsBLReportIssue.GetSeverity();
                if (dsSeverity.Tables[0].Rows.Count > 0)
                {
                    ddlSeverity.DataSource = dsSeverity.Tables[0];
                    ddlSeverity.DataValueField = dsSeverity.Tables[0].Columns[0].ToString();
                    ddlSeverity.DataTextField = dsSeverity.Tables[0].Columns[1].ToString();
                    ddlSeverity.DataBind();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "BindSeverity", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void BindType()
        {
            try
            {
                ddltype.Items.Clear();

                ddltype.Items.Insert(0, new ListItem("Select", "0"));

                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                DataSet dsType = objClsBLReportIssue.GetType();

                for (int i = dsType.Tables[0].Rows.Count-1; i >= 0; i--)
                {
                    ddltype.Items.Add(new ListItem(dsType.Tables[0].Rows[i]["RequestType"].ToString(), dsType.Tables[0].Rows[i]["TypeID"].ToString()));
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "BindType", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void BindType(int catagoryID)
        {
            try
            {
                ddltype.Items.Clear();

                ddltype.Items.Insert(0, new ListItem("Select", "0"));
                ddltype.Items.Insert(1, new ListItem("Incidents", "1"));
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "BindType", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void BindProjectName()
        {
            try
            {
                string EmployeeCode = User.Identity.Name.ToString();
                ddlProjectName.Items.Clear();

                ddlProjectName.Items.Insert(0, new ListItem("Select", "0"));
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                DataSet dsType = objClsBLReportIssue.GetProjectName(EmployeeCode);

                for (int i = 0; i < dsType.Tables[0].Rows.Count; i++)
                {
                    ddlProjectName.Items.Add(new ListItem(dsType.Tables[0].Rows[i]["ProjectName"].ToString(), dsType.Tables[0].Rows[i]["ProjectId"].ToString()));
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "BindProjectName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void BindProjectRole()
        {
            try
            {
                ddlProjectRole.Items.Clear();

                ddlProjectRole.Items.Insert(0, new ListItem("Select", "0"));
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                DataSet dsRole = objClsBLReportIssue.GetProjectRole();

                for (int i = 0; i < dsRole.Tables[0].Rows.Count; i++)
                {
                    ddlProjectRole.Items.Add(new ListItem(dsRole.Tables[0].Rows[i]["RoleDescription"].ToString(), dsRole.Tables[0].Rows[i]["RoleID"].ToString()));
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "BindProjectRole", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void BindResourcePool()
        {
            try
            {
                ddlResourcePool.Items.Clear();

                ddlResourcePool.Items.Insert(0, new ListItem("Select", "0"));
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                DataSet dsResourcePool = objClsBLReportIssue.GetResourcePool();

                for (int i = 0; i < dsResourcePool.Tables[0].Rows.Count; i++)
                {
                    ddlResourcePool.Items.Add(new ListItem(dsResourcePool.Tables[0].Rows[i]["ResourcePoolName"].ToString(), dsResourcePool.Tables[0].Rows[i]["ResourcePoolID"].ToString()));
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "BindProjectRole", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void BindReportingTo(int stringParam)
        {
            clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
            DataSet dsReportingTo = objClsBLReportIssue.GetReportingTo(stringParam);

            ddlReportingTo.Items.Clear();
            ddlReportingTo.Items.Insert(0, new ListItem("Select", "0"));
            for (int i = 0; i < dsReportingTo.Tables[0].Rows.Count; i++)
            {
                ddlReportingTo.Items.Add(new ListItem(dsReportingTo.Tables[0].Rows[i]["EmployeeName"].ToString(), dsReportingTo.Tables[0].Rows[i]["EmployeeId"].ToString()));
            }
        }

        private void BindSubCategory(int categoryID)
        {
            try
            {
                ddlSubCategories.Items.Clear();
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                DataSet dsSubCategory = objClsBLReportIssue.GetSubCategoryByCategoryID(categoryID);

                for (int i = 0; i <= dsSubCategory.Tables[0].Rows.Count - 1; i++)
                {
                    ListItem li = new ListItem(dsSubCategory.Tables[0].Rows[i][1].ToString(), dsSubCategory.Tables[0].Rows[i][0].ToString());

                    ddlSubCategories.Items.Add(li);
                    if (ddlSubCategories.Items[i].Value.Equals("0"))
                    {
                        ddlSubCategories.Items[i].Attributes.Add("class", "tableheader");
                    }
                    else
                    {
                        ddlSubCategories.Items[i].Text = ddlSubCategories.Items[i].Text.Replace("--", "");
                    }
                }
                ddlSubCategories.Items.Insert(0, new ListItem("Select SubCategory", "0"));
                ddlSubCategories.SelectedIndex = 0;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "BindSubCategory", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void BindPriority()
        {
            try
            {
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "BindPriority", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            try
            {
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                lblMessage.Text = "";
                lblCcEmailError.Text = "";
                lblError.Text = "";
                if (ddlCategories.SelectedIndex == 0)
                {
                    lblError.Text = "Please enter Categories." + "</br>";
                    lblError.Visible = true;
                }
                if (ddlCategories.Text == "12")
                {
                    ddlSubCategories.SelectedIndex = 1;
                    if (ddltype.SelectedIndex == 0)
                    {
                        lblError.Text = "Please enter Type." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && ddlProjectName.SelectedIndex == 0)
                    {
                        lblError.Text = "Please enter Project Name." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && ddlSubCategories.SelectedIndex != 3 && txtWorkHours.Text == "")
                    {
                        lblError.Text = "Please enter Work Hours." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && ddlSubCategories.SelectedIndex != 3 && txtFromDate.Text == "")
                    {
                        lblError.Text = "Please enter From Date." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && ddlSubCategories.SelectedIndex != 3 && ddlResourcePool.SelectedIndex == 0)
                    {
                        lblError.Text = "Please enter Resource Pool." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && ddlSubCategories.SelectedIndex != 3 && ddlProjectRole.SelectedIndex == 0)
                    {
                        lblError.Text = "Please enter Project Role." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && txtNoOfResources.Text == "")
                    {
                        lblError.Text = "Please enter No Of Resources." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && txtEndDate.Text == "")
                    {
                        lblError.Text = "Please enter End Date." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && ddlSubCategories.SelectedIndex != 3 && ddlReportingTo.SelectedIndex == 0)
                    {
                        lblError.Text = "Please enter Reporting To." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (txtDescription.InnerText == "")
                    {
                        lblError.Text = "Please enter Description." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (txtPhoneExtension.Text == "" && ddlCategories.SelectedItem.Text != "RMG")
                    {
                        lblError.Text = "Please enter Phone Extension." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (txtSeatingLocation.Text == "" && ddlCategories.SelectedItem.Text != "RMG")
                    {
                        lblError.Text = "Please enter Seating Location." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (txtCCEmailID.Text != "")
                    {
                        bool result = objClsBLReportIssue.IsCCEmailExists(txtCCEmailID.Text);

                        if (result == false)
                        {
                            lblCcEmailError.Text = "Please enter valid 'CC to Email' address";
                        }
                        else
                        {
                            InsertIssueDetails();
                            if (ShouldUploadFile == 1)
                                UploadFiles();

                            if (ShouldSendMail == 1)
                                SendMail();

                            lblMessage.Text = "Issue Successfully recorded." + "</br>";
                            lblMessage.Visible = true;
                            lblMailError.Visible = true;
                        }
                    }
                    else
                    {
                        InsertIssueDetails();
                        if (ShouldUploadFile == 1)
                            UploadFiles();

                        if (ShouldSendMail == 1)
                            SendMail();

                        lblMessage.Text = "Issue Successfully recorded." + "</br>";
                        lblMailError.Visible = true;
                    }
                }
                else if (ddlCategories.SelectedItem.Text == "RMG" && ddlSubCategories.SelectedIndex == 4)
                {
                    if (ddlSubCategories.SelectedIndex == 0)
                    {
                        lblError.Text = "Please enter Subcategories." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddltype.SelectedIndex == 0)
                    {
                        lblError.Text = "Please enter Type." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (txtDescription.InnerText == "")
                    {
                        lblError.Text = "Please enter Description." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (txtPhoneExtension.Text == "" && ddlCategories.SelectedItem.Text != "RMG")
                    {
                        lblError.Text = "Please enter Phone Extension." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (txtSeatingLocation.Text == "" && ddlCategories.SelectedItem.Text != "RMG")
                    {
                        lblError.Text = "Please enter Seating Location." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (txtCCEmailID.Text != "")
                    {
                        bool result = objClsBLReportIssue.IsCCEmailExists(txtCCEmailID.Text);

                        if (result == false)
                        {
                            lblCcEmailError.Text = "Please enter valid 'CC to Email' address";
                        }
                        else
                        {
                            InsertIssueDetails();
                            if (ShouldUploadFile == 1)
                                UploadFiles();

                            if (ShouldSendMail == 1)
                                SendMail();

                            lblMessage.Text = "Issue Successfully recorded." + "</br>";
                            lblMessage.Visible = true;
                            lblMailError.Visible = true;
                        }
                    }
                    else
                    {
                        InsertIssueDetails();
                        if (ShouldUploadFile == 1)
                            UploadFiles();

                        if (ShouldSendMail == 1)
                            SendMail();

                        lblMessage.Text = "Issue Successfully recorded." + "</br>";
                        lblMailError.Visible = true;
                    }
                }
                else
                {
                    if (ddlSubCategories.SelectedIndex == 0)
                    {
                        lblError.Text = "Please enter Subcategories." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddltype.SelectedIndex == 0)
                    {
                        lblError.Text = "Please enter Type." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && ddlProjectName.SelectedIndex == 0)
                    {
                        lblError.Text = "Please enter Project Name." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && ddlSubCategories.SelectedIndex != 3 && txtWorkHours.Text == "")
                    {
                        lblError.Text = "Please enter Work Hours." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && ddlSubCategories.SelectedIndex != 3 && txtFromDate.Text == "")
                    {
                        lblError.Text = "Please enter From Date." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && ddlSubCategories.SelectedIndex != 3 && ddlResourcePool.SelectedIndex == 0)
                    {
                        lblError.Text = "Please enter Resource Pool." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && ddlSubCategories.SelectedIndex != 3 && ddlProjectRole.SelectedIndex == 0)
                    {
                        lblError.Text = "Please enter Project Role." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && txtNoOfResources.Text == "")
                    {
                        lblError.Text = "Please enter No Of Resources." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && txtEndDate.Text == "")
                    {
                        lblError.Text = "Please enter End Date." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (ddlCategories.SelectedItem.Text == "RMG" && ddlSubCategories.SelectedIndex != 3 && ddlReportingTo.SelectedIndex == 0)
                    {
                        lblError.Text = "Please enter Reporting To." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (txtDescription.InnerText == "")
                    {
                        lblError.Text = "Please enter Description." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (txtPhoneExtension.Text == "" && ddlCategories.SelectedItem.Text != "RMG")
                    {
                        lblError.Text = "Please enter Phone Extension." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (txtSeatingLocation.Text == "" && ddlCategories.SelectedItem.Text != "RMG")
                    {
                        lblError.Text = "Please enter Seating Location." + "</br>";
                        lblError.Visible = true;
                    }
                    else if (txtCCEmailID.Text != "")
                    {
                        bool result = objClsBLReportIssue.IsCCEmailExists(txtCCEmailID.Text);

                        if (result == false)
                        {
                            lblCcEmailError.Text = "Please enter valid 'CC to Email' address";
                        }
                        else
                        {
                            InsertIssueDetails();
                            if (ShouldUploadFile == 1)
                                UploadFiles();

                            if (ShouldSendMail == 1)
                                SendMail();

                            lblMessage.Text = "Issue Successfully recorded." + "</br>";
                            lblMessage.Visible = true;
                            lblMailError.Visible = true;
                        }
                    }
                    else
                    {
                        InsertIssueDetails();
                        if (ShouldUploadFile == 1)
                            UploadFiles();

                        if (ShouldSendMail == 1)
                            SendMail();

                        lblMessage.Text = "Issue Successfully recorded." + "</br>";
                        lblMailError.Visible = true;
                    }
                }

                /// <summary>
                /// To send emails to the user, the TL and the HelpDesk Member
                /// </summary>
                ///
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void SendMail()
        {
            HRMS_tbl_PM_Employee emp = new HRMS_tbl_PM_Employee();
            //Added by Rahul Ramachandran for finding the Employee office Location to differentiate issues on the basis of Employees Office Location.
            EmployeeDAL empdet = new EmployeeDAL();
            emp = empdet.GetEmployeeDetailsByEmployeeCode(Convert.ToString(EmployeeID));
            string USMailBody = string.Empty;
            USMailBody = "<br><b><i>**This issue is logged by US Employee**</i></b><br><br>";
            try
            {
                //To send mail to the Dept under which issue falls.
                clsReportIssue objClsReportIssue = new clsReportIssue();
                string subCategoryName = string.Empty;
                string Category = string.Empty;
                string severityStat = string.Empty;
                string severity = ddlSeverity.Items[ddlSeverity.SelectedIndex].Text;
                if (ddlCategories.Text == "12")
                {
                    subCategoryName = ddltype.Items[ddltype.SelectedIndex].Text;
                    Category = ddlCategories.Items[ddlCategories.SelectedIndex].Text;
                    severityStat = string.Empty;
                }
                else
                {
                    objClsReportIssue.SubCategoryID = Convert.ToInt32(ddlSubCategories.Items[ddlSubCategories.SelectedIndex].Value);
                    subCategoryName = ddlSubCategories.Items[ddlSubCategories.SelectedIndex].Text;
                    Category = GetCategoryName(objClsReportIssue).TrimStart(' ').TrimEnd(' ');
                    severityStat = "<b>" + "Issue Severity : " + "</b>" + severity + "<br>" + "<br>";
                }

                SmtpClient SmtpMail = new SmtpClient(ConfigurationSettings.AppSettings["SMTPServerName"].ToString());
                if (Category == "")
                {
                    strDeptEmailID = ConfigurationSettings.AppSettings["IT_Dept_EmailID"].ToString();
                }
                else
                {
                    strDeptEmailID = ConfigurationSettings.AppSettings[Category].ToString();
                    string CCCateory = Category + "CC";
                    strDeptCCEmailID = ConfigurationSettings.AppSettings[CCCateory].ToString();
                }

                string strPath = ConfigurationSettings.AppSettings["Mail_Path"].ToString();
                string arrow = "&rarr;";

                MailMessage sendMailToDept = new MailMessage();
                //Loop to seperate email id's of CC peoples
                if (strDeptCCEmailID.Contains(","))
                {
                    string[] CCEmailId = strDeptCCEmailID.Split(',');
                    foreach (string email in CCEmailId)
                    {
                        sendMailToDept.To.Add(new MailAddress(email));
                    }
                }
                else
                    sendMailToDept.To.Add(new MailAddress(strDeptCCEmailID));

                //If there is a manager in CC while logging an issue , then in mail CC part there will be user & manager
                if (txtCCEmailID.Text != "")
                {
                    string User_ManagerEmail = txtEmailID.Text + "," + txtCCEmailID.Text;

                    if (User_ManagerEmail.Contains(","))
                    {
                        string[] CCEmailId = User_ManagerEmail.Split(',');
                        foreach (string email in CCEmailId)
                        {
                            sendMailToDept.CC.Add(new MailAddress(email));
                        }
                    }
                }

                //else only user will be there.
                else
                {
                    sendMailToDept.CC.Add(new MailAddress(txtEmailID.Text));
                }

                sendMailToDept.From = new MailAddress(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), txtEmailID.Text);
                sendMailToDept.Subject = "HelpDesk : Issue " + intIssueID + " under " + Category +
                                         " : " + subCategoryName + " has been logged.";

                //OfficeLocation 3 is of Bengaluru and 2 is of Mumbai
                if (emp.OfficeLocation != 3 && emp.OfficeLocation != 2)
                {
                    sendMailToDept.Body = USMailBody + "Hi, " + "<br>" + "<br>" +
                    "This is to inform you that, a new issue has been logged. " + "<br>" + " Issue details are as follows :" + "<br>" + "<br>" +
                     "<b>" + "Issue ID : " + "</b>" + "<b>" + intIssueID + "</b>" + "<br>" + "<br>" +
                     "<b>" + "Department Name : " + "</b>" + Category + "<br>" + "<br>" +
                     "<b>" + "Category Name : " + "</b>" + subCategoryName + "<br>" + "<br>" +
                     severityStat +
                     "<b>" + "Current Status : " + "</b>" + "Open" + "<br>" + "<br>" +
                     "<b>" + "Description : " + "</b>" + Server.HtmlEncode(txtDescription.Value.ToString()) + "." + "<br>" + "<br>" +
                     "Please take the necessary action." + "<br>" + "<br>" +
                      "Regards," + "<br>" +
                      Server.HtmlEncode(txtName.Text.ToString());
                }
                else
                {
                    sendMailToDept.Body = "Hi, " + "<br>" + "<br>" +
                    "This is to inform you that, a new issue has been logged. " + "<br>" + " Issue details are as follows :" + "<br>" + "<br>" +
                     "<b>" + "Issue ID : " + "</b>" + "<b>" + intIssueID + "</b>" + "<br>" + "<br>" +
                     "<b>" + "Department Name : " + "</b>" + Category + "<br>" + "<br>" +
                     "<b>" + "Category Name : " + "</b>" + subCategoryName + "<br>" + "<br>" +
                     severityStat +
                     "<b>" + "Current Status : " + "</b>" + "Open" + "<br>" + "<br>" +
                     "<b>" + "Description : " + "</b>" + Server.HtmlEncode(txtDescription.Value.ToString()) + "." + "<br>" + "<br>" +
                     "Please take the necessary action." + "<br>" + "<br>" +
                      "Regards," + "<br>" +
                      Server.HtmlEncode(txtName.Text.ToString());
                }

                sendMailToDept.IsBodyHtml = true;

                SmtpMail.UseDefaultCredentials = false;
                SmtpMail.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), ConfigurationSettings.AppSettings["SMTPPassword"].ToString());
                SmtpMail.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["PortNumber"].ToString());
                SmtpMail.EnableSsl = true;
                SmtpMail.Send(sendMailToDept);

                resetAll();
                lblMessage.Visible = true;
                lblMessage.Text = "An email will be sent to you confirming the recording of the issue.";
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                lblMailError.Text = ex.Message;
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "SendMail", ex.StackTrace);
            }
        }

        //private void SendMail()
        //{
        //    try
        //    {
        //        //To send mail to the Dept under which issue falls.
        //        clsReportIssue objClsReportIssue = new clsReportIssue();
        //        objClsReportIssue.SubCategoryID = Convert.ToInt32(ddlSubCategories.Items[ddlSubCategories.SelectedIndex].Value);
        //        string subCategoryName = ddlSubCategories.Items[ddlSubCategories.SelectedIndex].Text;
        //        string severity = ddlSeverity.Items[ddlSeverity.SelectedIndex].Text;
        //        string Category = GetCategoryName(objClsReportIssue).TrimStart(' ').TrimEnd(' ');
        //        SmtpClient SmtpMail = new SmtpClient(ConfigurationSettings.AppSettings["SMTPServerName"].ToString());
        //        if (Category == "")
        //        {
        //            strDeptEmailID = ConfigurationSettings.AppSettings["IT_Dept_EmailID"].ToString();
        //        }
        //        else
        //        {
        //            strDeptEmailID = ConfigurationSettings.AppSettings[Category].ToString();
        //            string CCCateory = Category + "CC";
        //            strDeptCCEmailID = ConfigurationSettings.AppSettings[CCCateory].ToString();
        //        }

        //        string strPath = ConfigurationSettings.AppSettings["Mail_Path"].ToString();
        //        string arrow = "&rarr;";

        //        MailMessage sendMailToDept = new MailMessage();
        //        //Loop to seperate email id's of CC peoples
        //        if (strDeptCCEmailID.Contains(","))
        //        {
        //            string[] CCEmailId = strDeptCCEmailID.Split(',');
        //            foreach (string email in CCEmailId)
        //            {
        //                sendMailToDept.To.Add(new MailAddress(email));
        //            }
        //        }
        //        else
        //            sendMailToDept.To.Add(new MailAddress(strDeptCCEmailID));

        //        //If there is a manager in CC while logging an issue , then in mail CC part there will be user & manager
        //        if (txtCCEmailID.Text != "")
        //        {
        //            string User_ManagerEmail = txtEmailID.Text + "," + txtCCEmailID.Text;

        //            if (User_ManagerEmail.Contains(","))
        //            {
        //                string[] CCEmailId = User_ManagerEmail.Split(',');
        //                foreach (string email in CCEmailId)
        //                {
        //                    sendMailToDept.CC.Add(new MailAddress(email));
        //                }

        //            }

        //        }

        //        //else only user will be there.
        //        else
        //        {
        //            sendMailToDept.CC.Add(new MailAddress(txtEmailID.Text));
        //        }

        //        sendMailToDept.From = new MailAddress(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), txtEmailID.Text);
        //        sendMailToDept.Subject = "HelpDesk : Issue " + intIssueID + " under " + Category +
        //                                 " : " + subCategoryName + " has been logged.";

        //        sendMailToDept.Body = "Hi, " + "<br>" + "<br>" +
        //            "This is to inform you that, a new issue has been logged. " + "<br>" + " Issue details are as follows :" + "<br>" + "<br>" +
        //             "<b>" + "Issue ID : " + "</b>" + "<b>" + intIssueID + "</b>" + "<br>" + "<br>" +
        //             "<b>" + "Department Name : " + "</b>" + Category + "<br>" + "<br>" +
        //             "<b>" + "Category Name : " + "</b>" + subCategoryName + "<br>" + "<br>" +
        //             "<b>" + "Issue Severity : " + "</b>" + severity + "<br>" + "<br>" +
        //             "<b>" + "Current Status : " + "</b>" + "Open" + "<br>" + "<br>" +
        //             "<b>" + "Description : " + "</b>" + Server.HtmlEncode(txtDescription.Value.ToString()) + "." + "<br>" + "<br>" +
        //             "Please take the necessary action." + "<br>" + "<br>" + "<br>" +
        //              "Regards," + "<br>" +
        //              Server.HtmlEncode(txtName.Text.ToString());

        //        sendMailToDept.IsBodyHtml = true;

        //        SmtpMail.UseDefaultCredentials = false;
        //        SmtpMail.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), ConfigurationSettings.AppSettings["SMTPPassword"].ToString());
        //        SmtpMail.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["PortNumber"].ToString());
        //        SmtpMail.EnableSsl = true;
        //        SmtpMail.Send(sendMailToDept);

        //        resetAll();
        //        lblMessage.Visible = true;
        //        lblMessage.Text = "An email will be sent to you confirming the recording of the issue.";
        //    }

        //    catch (V2Exceptions ex)
        //    {
        //        throw;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        lblMailError.Text = ex.Message;
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "SendMail", ex.StackTrace);

        //    }
        //}
        public string GetCategoryName(clsReportIssue objclsReportIssue)
        {
            try
            {
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                return objClsBLReportIssue.GetCategoryName(objclsReportIssue);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "GetCategoryName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void InsertIssueDetails()
        {
            try
            {
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                clsReportIssue objClsReportIssue = new clsReportIssue();
                IssueStatus objIssueStatus = new IssueStatus();
                intIssueID = 0;
                objClsReportIssue.Name = txtName.Text.ToString();
                if (objClsReportIssue.Name == "")
                {
                    objClsReportIssue.Name = null;
                }
                objClsReportIssue.EmailID = txtEmailID.Text.ToString();
                objClsReportIssue.CCEmailID = txtCCEmailID.Text.ToString();
                objClsReportIssue.PhoneExtension = txtPhoneExtension.Text.ToString();
                objClsReportIssue.Type = Convert.ToInt32(ddltype.SelectedItem.Value);
                objClsReportIssue.SeatingLocation = txtSeatingLocation.Text.ToString();
                if (ddlCategories.Text == "12")
                {
                    objClsReportIssue.SubCategoryID = Convert.ToInt32(ddlSubCategories.Items[ddlSubCategories.SelectedIndex].Value);
                    objClsReportIssue.SeverityID = Convert.ToInt32(ddlSeverity.SelectedItem.Value);
                }
                else
                {
                    objClsReportIssue.SubCategoryID = Convert.ToInt32(ddlSubCategories.Items[ddlSubCategories.SelectedIndex].Value);
                    objClsReportIssue.SeverityID = Convert.ToInt32(ddlSeverity.SelectedItem.Value);
                }
                int categoryId = objClsReportIssue.SubCategoryID;

                objClsReportIssue.Description = txtDescription.Value.ToString();

                //Added By Nikhil
                objClsReportIssue.ProjectNameId = Convert.ToInt32(ddlProjectName.SelectedItem.Value);
                objClsReportIssue.ProjectRoleId = Convert.ToInt32(ddlProjectRole.SelectedItem.Value);
                objClsReportIssue.ResourcePoolId = Convert.ToInt32(ddlResourcePool.SelectedItem.Value);

                //Commented by Rahul:#141306597(Report Manager is not Displayed in Admin view)
                //if (objClsReportIssue.ReportingToId != 0)
                {
                    if (ddlReportingTo.SelectedIndex > 0)
                        objClsReportIssue.ReportingToId = Convert.ToInt32(ddlReportingTo.SelectedItem.Value);
                }
                objClsReportIssue.FromDate = null;
                objClsReportIssue.ToDate = null;
                if (categoryId == Convert.ToInt32(ConfigurationSettings.AppSettings["NewResource"].ToString())
                    || categoryId == Convert.ToInt32(ConfigurationSettings.AppSettings["UpdateCurrentAllocation"].ToString())
                    || categoryId == Convert.ToInt32(ConfigurationSettings.AppSettings["SingleOrBulkExtension"].ToString()))
                {
                    if (categoryId != Convert.ToInt32(ConfigurationSettings.AppSettings["SingleOrBulkExtension"].ToString()))
                    {
                        objClsReportIssue.WorkHours = Convert.ToInt32(txtWorkHours.Text);
                        objClsReportIssue.FromDate = Convert.ToDateTime(txtFromDate.Text.ToString());
                    }
                    objClsReportIssue.NumberOfResources = Convert.ToInt32(txtNoOfResources.Text);
                    objClsReportIssue.ToDate = Convert.ToDateTime(txtEndDate.Text.ToString());
                }

                objClsReportIssue.StatusID = 1;
                if ((uploadFiles.PostedFile != null) && (uploadFiles.PostedFile.ContentLength > 0))
                {
                    fn = System.IO.Path.GetFileNameWithoutExtension(uploadFiles.PostedFile.FileName);
                    fnExt = System.IO.Path.GetExtension(uploadFiles.PostedFile.FileName);
                    objClsReportIssue.UploadedFileName = fn.ToString();
                    objClsReportIssue.UploadedFileExtension = fnExt.ToString();
                }
                IssueSubmitResult = objClsBLReportIssue.InsertIssueDetails(objClsReportIssue);

                if (IssueSubmitResult.Tables[0].Rows.Count > 0)
                {
                    intIssueID = Convert.ToInt32(IssueSubmitResult.Tables[0].Rows[0]["IssueId"].ToString());
                    lblMessage.Visible = true;
                    lblMessage.CssClass = "success";
                    lblMessage.Text = "Issue Successfully recorded.";
                    ShouldSendMail = 1;
                    ShouldUploadFile = 1;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.CssClass = "Error";
                    lblMessage.Text = "Error occured in saving the Issue ,Please try again";
                    ShouldSendMail = 0;
                    ShouldUploadFile = 0;
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "InsertIssueDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ddlSubCategories.Attributes.Add("runat", "server");

            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        #endregion Web Form Designer generated code

        private void ddlSubCategories_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                clsReportIssue objClsReportIssue = new clsReportIssue();
                objClsBLReportIssue.GetSelectedProjectDetails(objClsReportIssue);
                lblMessage.Text = "";
                CheckSelectedItem();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "ddlSubCategories_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void itemSelected()
        {
            try
            {
                lblMessage.Text = "";
                CheckSelectedItem();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "ddlProjectName_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private void CheckSelectedItem()
        {
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            try
            {
                Response.Redirect("ReportIssue.aspx", false);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "resetAll", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void resetAll()
        {
            try
            {
                txtCCEmailID.Text = "";
                txtPhoneExtension.Text = "";
                txtSeatingLocation.Text = "";
                BindCategory();

                ddlSeverity.SelectedIndex = 0;
                ddltype.SelectedIndex = 0;
                txtDescription.Value = "";
                ddlProjectName.SelectedIndex = 0;
                ddlProjectRole.SelectedIndex = 0;
                ddlResourcePool.SelectedIndex = 0;
                ddlReportingTo.SelectedIndex = 0;
                txtWorkHours.Text = "";
                txtFromDate.Text = "";
                txtNoOfResources.Text = "";
                txtEndDate.Text = "";
                lblMessage.Visible = false;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "resetAll", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void UploadFiles()
        {
            try
            {
                if ((uploadFiles.PostedFile != null) && (uploadFiles.PostedFile.ContentLength > 0))
                {
                    string SaveLocation = ConfigurationSettings.AppSettings["UploadedfilePath"].ToString();

                    try
                    {
                        uploadFiles.PostedFile.SaveAs(SaveLocation + "\\" + fn + "_" + intIssueID + fnExt);

                        lblMessage.Visible = true;
                        lblMessage.CssClass = "success";
                        lblMessage.Text = "The file has been uploaded.";
                    }
                    catch (V2Exceptions ex)
                    {
                        throw;
                    }
                    catch (System.Exception ex)
                    {
                        lblMessage.Visible = true;
                        lblMessage.CssClass = "success";
                        lblMessage.Text = "Error: " + ex.Message;

                        FileLog objFileLog = FileLog.GetLogger();
                        objFileLog.WriteLine(LogType.Error, ex.Message, "ViewMyStatus.aspx", "UploadFiles", ex.StackTrace);
                        throw new V2Exceptions(ex.ToString(), ex);
                    }
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "UploadFiles", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        [WebMethod]
        public static string getDropdomdata(string stringParam)
        {
            DataSet dsGetData = new DataSet();
            string val = "";

            clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
            clsReportIssue objClsReportIssue = new clsReportIssue();
            var categoryissue = stringParam;
            objClsReportIssue.SubCategoryID = Convert.ToInt32(categoryissue);
            dsGetData = objClsBLReportIssue.getCategorySummary(objClsReportIssue);
            if (dsGetData.Tables[0].Rows.Count > 0)
            {
                val = dsGetData.Tables[0].Rows[0]["IssueSummary"].ToString();
            }
            else
            {
                val = "";
            }
            return val;
        }

        [WebMethod]
        public static string GetProjectDates(string stringParam)
        {
            try
            {
                DataSet dsGetData = new DataSet();
                string startdate = "";
                string enddate = "";

                clsBLReportIssue objClsBLReportIssue = new clsBLReportIssue();
                clsReportIssue objClsReportIssue = new clsReportIssue();
                var categoryissue = stringParam;
                objClsReportIssue.ProjectNameId = Convert.ToInt32(categoryissue);
                dsGetData = objClsBLReportIssue.GetProjectDates(objClsReportIssue);
                if (dsGetData.Tables[0].Rows.Count > 0)
                {
                    startdate = dsGetData.Tables[0].Rows[0]["ActualStartDate"].ToString();
                    enddate = dsGetData.Tables[0].Rows[0]["ActualEndDate"].ToString();
                    startdate = startdate + enddate;
                }
                else
                {
                    startdate = "";
                    enddate = "";
                }

                return startdate;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "GetProjectDates", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        private ActionResult Json(object p, JsonRequestBehavior jsonRequestBehavior)
        {
            throw new NotImplementedException();
        }

        protected void ddlProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedProjectId = Convert.ToInt32(ddlProjectName.SelectedValue);
            BindReportingTo(selectedProjectId);
        }

        private void BindCategory()
        {
            ddlCategories.Items.Clear();
            clsBLSubCategory objClsBLSubCategory = new clsBLSubCategory();
            dsCategoryID = objClsBLSubCategory.getCategoryID();
            ddlCategories.DataSource = dsCategoryID.Tables[0];
            ddlCategories.DataValueField = dsCategoryID.Tables[0].Columns["CategoryID"].ToString();
            ddlCategories.DataTextField = dsCategoryID.Tables[0].Columns["Category"].ToString();
            ddlCategories.DataBind();
            ddlCategories.Items.Insert(0, new ListItem("Select Category", "0"));
            ddlCategories.SelectedIndex = 0;
            subCategoryId.Visible = false;
        }

        protected void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCategories.SelectedIndex == 0)
                {
                    ddlSubCategories.Items.Clear();
                    subCategoryId.Visible = false;
                }
                else
                {
                    int categoryID = Convert.ToInt32(ddlCategories.SelectedValue);
                    if (categoryID == 12)
                    {
                        BindType(categoryID);
                        BindSubCategory(categoryID);
                        subCategoryId.Visible = false;
                        ddlSubCategories.Visible = false;
                        ddlSeverity.Visible = false;
                        severity.Visible = false;
                    }
                    else
                    {
                        BindType();
                        BindSubCategory(categoryID);
                        subCategoryId.Visible = true;
                        ddlSubCategories.Visible = true;
                        ddlSeverity.Visible = true;
                        severity.Visible = true;
                    }
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ReportIssue.aspx", "ddlSubCategories_SelectedIndexChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}