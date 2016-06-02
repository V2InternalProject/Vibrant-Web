using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Workflow.Runtime;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using V2.Orbit.BusinessLayer;
using V2.Orbit.Model;

namespace HRMS.Orbitweb
{
    public partial class ManualSignin : System.Web.UI.Page
    {
        //Declaration of local variables

        private ManualSignInSignOutBOL objManualSignInSignOutBOL = new ManualSignInSignOutBOL();
        private SignInSignOutBOL objSignInSignOutBOL = new SignInSignOutBOL();
        private DataSet dsGetOldData = new DataSet();

        //added by Anushree Tirwadkar on 19_4_2010 to solve signin before joining date issue.
        private DataSet dsEmployeeJoiningDate = new DataSet();

        private DateTime JoiningDate;
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();
        private SignInSignOutModel objSignInSignOutModel = new SignInSignOutModel();
        private string strID = "";
        private string strCommandName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string PageName = "SignInSignOutManual";
                objpagelevel.PageLevelAccess(PageName);

                if (!IsPostBack)
                {
                    txtSignInDate.Attributes.Add("onkeydown", "return false");
                    txtSignOutDate.Attributes.Add("onkeydown", "return false");
                    ViewState["CommandName"] = "";
                    if (Session["CommandName"] != "" && Session["CommandName"] != null)
                    {
                        ViewState["CommandName"] = Session["CommandName"].ToString();
                        Session["CommandName"] = "";
                        txtSignInDate.ReadOnly = true;
                        imgbtnSignInDate.Enabled = false;
                    }
                    else
                    {
                        tblSignInSignOut.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Right;
                        tblSignInSignOut.Rows[0].Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    }
                    lblErrorMess.Text = "";
                    FillDropDowns();
                    SettingRangeValidaters();
                }
                MainTabSelected.Value = "Manual";

                objSignInSignOutModel.EmployeeID = Convert.ToInt32(User.Identity.Name);

                if (ViewState["CommandName"].ToString() != "")
                {
                    string[] strArray = new string[2];
                    string str = ViewState["CommandName"].ToString();
                    char[] chr = { ';' };
                    strArray = str.Split(chr);
                    strID = strArray[1];
                    strCommandName = strArray[0];

                    //txtSignInDate.ReadOnly = true;
                    //txtSignOutDate.ReadOnly = true;
                    objSignInSignOutModel.SignInSignOutID = Convert.ToInt32(strID);
                    GetOldData();

                    if (!IsPostBack)
                    {
                        txtSignInDate.Text = dsGetOldData.Tables[0].Rows[0]["date"].ToString();
                        txtInComments.Text = dsGetOldData.Tables[0].Rows[0]["SignInComment"].ToString();
                        DateTime dtInTime = Convert.ToDateTime(dsGetOldData.Tables[0].Rows[0]["SignInTime"].ToString());

                        ddlInHrs.SelectedValue = dtInTime.Hour.ToString();
                        ddlInMins.SelectedValue = dtInTime.Minute.ToString();
                        if (dsGetOldData.Tables[0].Rows[0]["SignOutTime1"].ToString() == "")
                        {
                            txtSignOutDate.Text = txtSignInDate.Text;
                        }
                        else
                        {
                            txtSignOutDate.Text = dsGetOldData.Tables[0].Rows[0]["SignOutTime1"].ToString();
                        }
                        txtOutComments.Text = dsGetOldData.Tables[0].Rows[0]["SignOutComment"].ToString();

                        if (dsGetOldData.Tables[0].Rows[0]["SignOutTime"].ToString() != "")
                        {
                            DateTime dtOutTime = Convert.ToDateTime(dsGetOldData.Tables[0].Rows[0]["SignOutTime"].ToString());

                            ddlOutHrs.SelectedValue = dtOutTime.Hour.ToString();
                            ddlOutMins.SelectedValue = dtOutTime.Minute.ToString();
                        }
                        if (strCommandName == "Date")
                        {
                            tblSignInSignOut.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Right;
                            tblSignInSignOut.Rows[0].Cells[1].HorizontalAlign = HorizontalAlign.Left;

                            PanelSignIn.Visible = true;
                            PanelSignOut.Visible = true;
                        }
                        if (strCommandName == "InTime")
                        {
                            tblSignInSignOut.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            tblSignInSignOut.Rows[0].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            PanelSignOut.Visible = false;
                        }
                        if (strCommandName == "OutTime")
                        {
                            tblSignInSignOut.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            tblSignInSignOut.Rows[0].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            PanelSignIn.Visible = false;
                        }
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOut.cs", "Page_Load", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void FillDropDowns()
        {
            try
            {
                //filling the Hours Dropdown
                for (int i = 00; i <= 23; i++)
                {
                    if (i.ToString().Length == 1)
                    {
                        ddlInHrs.Items.Add(new ListItem("0" + i.ToString(), i.ToString()));
                        ddlOutHrs.Items.Add(new ListItem("0" + i.ToString(), i.ToString()));
                    }
                    else
                    {
                        ddlInHrs.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        ddlOutHrs.Items.Add(new ListItem(i.ToString()));
                    }
                }
                // filling the mins dropdown
                for (int i = 00; i <= 59; i++)
                {
                    if (i.ToString().Length == 1)
                    {
                        ddlInMins.Items.Add(new ListItem("0" + i.ToString(), i.ToString()));
                        ddlOutMins.Items.Add(new ListItem("0" + i.ToString(), i.ToString()));
                    }
                    else
                    {
                        ddlInMins.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        ddlOutMins.Items.Add(new ListItem(i.ToString(), i.ToString()));
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
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOut.cs", "FillDropDowns", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void GetOldData()
        {
            try
            {
                //dsGetOldData =  objManualSignInSignOutBOL.GetOldData(Convert.ToInt32(strSignInSignOutID));
                dsGetOldData = objManualSignInSignOutBOL.GetOldData(objSignInSignOutModel);
                objSignInSignOutModel.Date1 = Convert.ToDateTime(dsGetOldData.Tables[0].Rows[0]["date"]);

                objSignInSignOutModel.SignInComment = dsGetOldData.Tables[0].Rows[0]["SignInComment"].ToString();
                objSignInSignOutModel.SignOutComment = dsGetOldData.Tables[0].Rows[0]["SignOutComment"].ToString();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOut.cs", "GetOldData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                #region Date

                if (strCommandName != "InTime")
                {
                    string strOutDateTime = txtSignOutDate.Text + " " + ddlOutHrs.SelectedValue + ":" + ddlOutMins.SelectedValue + ":00";
                    objSignInSignOutModel.SignOutTime = Convert.ToDateTime(strOutDateTime);
                    string strDateTime = "";
                    strDateTime = txtSignInDate.Text + " " + ddlInHrs.SelectedValue + ":" + ddlInMins.SelectedValue + ":00";
                    objSignInSignOutModel.SignInTime = Convert.ToDateTime(strDateTime);
                    DateTime tempJoinging;
                    string strJoingingDatetime = "";

                    dsEmployeeJoiningDate = objManualSignInSignOutBOL.GetEmployeeJoiningData(objSignInSignOutModel);

                    if (dsEmployeeJoiningDate != null && dsEmployeeJoiningDate.Tables.Count > 0 && dsEmployeeJoiningDate.Tables[0].Rows.Count > 0)
                        strJoingingDatetime = dsEmployeeJoiningDate.Tables[0].Rows[0]["DateOfJoining"].ToString();

                    tempJoinging = Convert.ToDateTime(strJoingingDatetime);

                    if (objSignInSignOutModel.SignOutTime > DateTime.Now)
                    {
                        lblErrorMess.Text = "Manually entered sign-in time occurs in the future. Please enter the correct time.";
                    }
                    else if (objSignInSignOutModel.SignOutTime < objSignInSignOutModel.SignInTime)
                    {
                        lblErrorMess.Text = "Manually entered sign-out time occurs in the before the sign-in time. Please enter the correct time.";
                    }
                    else if (objSignInSignOutModel.SignInTime <= tempJoinging)
                    {
                        lblErrorMess.Text = "Manually entered sign-in time occurs before joining date . Please enter the correct time.";
                    }
                    else if (objSignInSignOutModel.SignOutTime <= tempJoinging)
                    {
                        lblErrorMess.Text = "Manually entered sign-out time occurs before joining date . Please enter the correct time.";
                    }
                    else
                    {
                        lblErrorMess.Text = "";
                        objSignInSignOutModel.EmployeeID = Convert.ToInt32(User.Identity.Name);

                        objSignInSignOutModel.SignInComment = txtInComments.Text;

                        objSignInSignOutModel.SignOutComment = txtOutComments.Text;
                        if (strCommandName == "OutTime")
                        {
                            objSignInSignOutModel.IsSignInManual = 0;
                        }
                        else
                        {
                            objSignInSignOutModel.IsSignInManual = 1;
                        }
                        objSignInSignOutModel.IsSignOutManual = 1;
                        if (objSignInSignOutModel.SignInSignOutID == 0)//add new record
                        {
                            SqlDataReader sdrMsignIn = objManualSignInSignOutBOL.SignIn(objSignInSignOutModel);

                            Guid gMSignInSignOutWFID = new Guid("00000000-0000-0000-0000-000000000000");
                            int SignInSignOutID = 0;
                            while (sdrMsignIn.Read())
                            {
                                if (sdrMsignIn.FieldCount.ToString() == "2")
                                {
                                    if (sdrMsignIn[1].ToString() != "")
                                    {
                                        SignInSignOutID = Convert.ToInt32(sdrMsignIn[0].ToString());
                                        gMSignInSignOutWFID = new Guid(sdrMsignIn[1].ToString());
                                        WorkflowRuntime wr = (WorkflowRuntime)Application["WokflowRuntime"];
                                        wr = new WorkflowRuntime();
                                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                                        parameters.Add("SignInSignOutID", SignInSignOutID);

                                        WorkflowInstance wi = wr.CreateWorkflow(typeof(SignInSignOutWF.SignInSignOutWF), parameters, gMSignInSignOutWFID);
                                        wi.Start();
                                    }
                                }
                            }
                            Response.Redirect("SignInSignOut.aspx");
                        }
                        else // modify existing record
                        {
                            SqlDataReader sdrUpdate = ModifyAddRecordsInsignInSignOut(objSignInSignOutModel);

                            Guid gMSignInSignOutWFID = new Guid("00000000-0000-0000-0000-000000000000");
                            int SignInSignOutID = 0;
                            while (sdrUpdate.Read())
                            {
                                if (sdrUpdate.FieldCount.ToString() == "2")
                                {
                                    if (sdrUpdate[1].ToString() != "")
                                    {
                                        SignInSignOutID = Convert.ToInt32(sdrUpdate[0].ToString());

                                        gMSignInSignOutWFID = new Guid(sdrUpdate[1].ToString());
                                        WorkflowRuntime wr = (WorkflowRuntime)Application["WokflowRuntime"];
                                        wr = new WorkflowRuntime();
                                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                                        parameters.Add("SignInSignOutID", SignInSignOutID);
                                        WorkflowInstance wi = wr.CreateWorkflow(typeof(SignInSignOutWF.SignInSignOutWF), parameters, gMSignInSignOutWFID);
                                        wi.Start();
                                    }
                                }
                            }
                            ViewState["CommandName"] = null;
                            Response.Redirect("SignInSignOut.aspx");
                        }
                    }
                }

                #endregion Date

                #region InTime

                if (strCommandName == "InTime")
                {
                    if (ddlOutHrs.SelectedValue != "-1")
                    {
                        string strOutDateTime = txtSignOutDate.Text + " " + ddlOutHrs.SelectedValue + ":" + ddlOutMins.SelectedValue + ":00";
                        objSignInSignOutModel.SignOutTime = Convert.ToDateTime(strOutDateTime);
                    }
                    string strDateTime = "";
                    strDateTime = txtSignInDate.Text + " " + ddlInHrs.SelectedValue + ":" + ddlInMins.SelectedValue + ":00";
                    objSignInSignOutModel.SignInTime = Convert.ToDateTime(strDateTime);

                    if ((objSignInSignOutModel.SignInTime > DateTime.Now))
                    {
                        lblErrorMess.Text = "Manually entered sign-in time occurs in the future. Please enter the correct time.";
                    }
                    else if ((ddlOutHrs.SelectedValue != "-1") && (objSignInSignOutModel.SignOutTime < objSignInSignOutModel.SignInTime))
                    {
                        lblErrorMess.Text = "Manually entered sign-out time occurs in the before the sign-in time. Please enter the correct time.";
                    }
                    else
                    {
                        objSignInSignOutModel.IsSignInManual = 1;
                        lblErrorMess.Text = "";
                        objSignInSignOutModel.EmployeeID = Convert.ToInt32(User.Identity.Name);
                        objSignInSignOutModel.SignInComment = txtInComments.Text;
                        SqlDataReader sdrModifyInTime = objManualSignInSignOutBOL.ModifyInTime(objSignInSignOutModel);
                        Guid gMSignInSignOutWFID = new Guid("00000000-0000-0000-0000-000000000000");
                        int SignInSignOutID = 0;
                        while (sdrModifyInTime.Read())
                        {
                            if (sdrModifyInTime.FieldCount.ToString() == "2")
                            {
                                if (sdrModifyInTime[1].ToString() != "")
                                {
                                    SignInSignOutID = Convert.ToInt32(sdrModifyInTime[0].ToString());

                                    gMSignInSignOutWFID = new Guid(sdrModifyInTime[1].ToString());
                                    WorkflowRuntime wr = (WorkflowRuntime)Application["WokflowRuntime"];
                                    wr = new WorkflowRuntime();
                                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                                    parameters.Add("SignInSignOutID", SignInSignOutID);
                                    WorkflowInstance wi = wr.CreateWorkflow(typeof(SignInSignOutWF.SignInSignOutWF), parameters, gMSignInSignOutWFID);
                                    wi.Start();
                                }
                            }
                        }
                        ViewState["CommandName"] = null;
                        Response.Redirect("SignInSignOut.aspx");
                    }

                #endregion InTime
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            { }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOut.cs", "btnSubmit_Click", ex.StackTrace);

                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void CheckAndGetDateData()
        {
            try
            {
                objSignInSignOutModel.SignInTime = Convert.ToDateTime(txtSignInDate.Text);
                objSignInSignOutModel.EmployeeID = Convert.ToInt16(User.Identity.Name);
                dsGetOldData = objManualSignInSignOutBOL.CheckAndGetDateData(objSignInSignOutModel);
                DataSet dsCheckMissingOutTime = objSignInSignOutBOL.CheckMissingSignOut(objSignInSignOutModel); //Missing Sign-Outs
                DateTime dt1 = Convert.ToDateTime(dsGetOldData.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                if (dsGetOldData.Tables[0].Rows.Count != 0) // entry already exist
                {
                    lblErrorMess.Text = "Record for this date already exist. Please click the Date link on the Home Page to modify this record or Enter a new date";
                    txtSignInDate.Text = "";
                }
                else if (dt1 >= objSignInSignOutModel.SignInTime)// freez date
                {
                    lblErrorMess.Text = "Administrator has frozen the data for the selected date";
                    txtSignInDate.Text = "";
                }
                else if (dsCheckMissingOutTime.Tables[0].Rows.Count != 0) // Missing Sign-Outs
                {
                    string Dates = "";
                    for (int i = 0; i < dsCheckMissingOutTime.Tables[0].Rows.Count; i++)
                    {
                        string strdate = DateTime.Today.ToString("MM/dd/yyyy");
                        if (dsCheckMissingOutTime.Tables[0].Rows[i]["Dates"].ToString() != strdate)
                        {
                            if (i == 0)
                            {
                                Dates = dsCheckMissingOutTime.Tables[0].Rows[i]["Dates"].ToString();
                            }
                            else
                            {
                                Dates = Dates + "," + dsCheckMissingOutTime.Tables[0].Rows[i]["Dates"].ToString();
                            }
                        }
                    }
                    if (Dates != "")
                    {
                        lblErrorMess.Text = "You have not signed out on " + Dates;
                        txtSignInDate.Text = "";
                        txtSignOutDate.Text = "";
                    }
                    else
                    {
                        lblErrorMess.Text = "";
                        txtSignOutDate.Text = txtSignInDate.Text;
                    }
                }
                else
                {
                    lblErrorMess.Text = "";
                    txtSignOutDate.Text = txtSignInDate.Text;
                }

                #region Commented

                //DateTime dtInTime = Convert.ToDateTime(dsGetOldData.Tables[0].Rows[0]["SignInTime"].ToString());
                //txtSignInDate.Text = dtInTime.ToShortDateString();
                //txtInComments.Text = dsGetOldData.Tables[0].Rows[0]["SignInComment"].ToString();

                //ddlInHrs.SelectedValue = dtInTime.Hour.ToString();
                //ddlInMins.SelectedValue = dtInTime.Minute.ToString();

                //if(dsGetOldData.Tables[0].Rows[0]["SignOutTime"].ToString()!="")
                //{
                //    DateTime dtOutTime = Convert.ToDateTime(dsGetOldData.Tables[0].Rows[0]["SignOutTime"].ToString());
                //    txtSignOutDate.Text=dtOutTime.ToShortDateString();
                //    ddlOutHrs.SelectedValue = dtOutTime.Hour.ToString();
                //    ddlOutMins.SelectedValue = dtOutTime.Minute.ToString();
                //   txtOutComments.Text=dsGetOldData.Tables[0].Rows[0]["SignOutComment"].ToString();
                //}

                #endregion Commented
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOut.cs", "CheckAndGetDateData", ex.StackTrace);

                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public SqlDataReader ModifyAddRecordsInsignInSignOut(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                return objManualSignInSignOutBOL.ModifyAddRecordsInsignInSignOut(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOut.cs", "ModifyAddRecordsInsignInSignOut", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        //check if the record for selected date already exists
        protected void txtSignInDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(txtSignInDate.Text);
                //dt = dt.ToShortDateString();
                DateTime dt1 = DateTime.Now;
                //dt1 = dt1.ToShortDateString();
                if (dt > dt1)
                {
                    lblErrorMess.Text = "Please dont select future date";
                }
                else
                {
                    CheckAndGetDateData();
                }
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOut.cs", "txtSignInDate_TextChanged", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public void SettingRangeValidaters()
        {
            try
            {
                rvInDate.MaximumValue = DateTime.Now.ToShortDateString();
                rvInDate.MinimumValue = DateTime.Now.AddMonths(-12).ToShortDateString();

                rvOutDate.MaximumValue = DateTime.Now.ToShortDateString();
                rvOutDate.MinimumValue = DateTime.Now.AddMonths(-12).ToShortDateString();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOut.cs", "SettingRangeValidaters", ex.StackTrace);

                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["CommandName"] = null;
                Response.Redirect("SignInSignOut.aspx");
            }
            catch (System.Threading.ThreadAbortException ex)
            { }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOut.cs", "btnSubmit_Click", ex.StackTrace);

                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}