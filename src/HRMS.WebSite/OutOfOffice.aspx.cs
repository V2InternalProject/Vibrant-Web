using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using V2.CommonServices;
using Microsoft.ApplicationBlocks.Data;
using V2.Orbit.Model;
using V2.Orbit.BusinessLayer;
using System.Data.SqlClient;
using V2.CommonServices.FileLogger;
using V2.CommonServices.Exceptions;
using System.Workflow.Runtime;
using V2.Orbit.Workflow.OutOfOfficeWF;
using System.Web.Mail;
//using System.Net.Mail;
public partial class OutOfOffice : System.Web.UI.Page
{

    #region Variable Declartion
    OutOfOfficeModel objOutOfOfficeModel = new OutOfOfficeModel();
    OutOfOfficeBOL objOutOfOfficeBOL = new OutOfOfficeBOL();

    DataSet dsGetResonName = new DataSet();
    DataSet dsGetOutofOffice = new DataSet();
    DataSet dsGetStatus = new DataSet();
    DataSet dsSearchOutOfOffice = new DataSet();
    DataSet dssearchDatewise = new DataSet();
    DataSet dsReportingTo = new DataSet();
    DataSet dsConfigItem = new DataSet();
    DataSet dsCancelOutOfOffice = new DataSet();

    string strDate;
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                fillHrs();
                getOutOfOffice();
                getReason();
                getStatus();
                resetControls();
                txtDate.Text = DateTime.Now.ToShortDateString();
                pnlAddDetails.Visible = true;
                tdbutton.Visible = true;
                pnlSearch.Visible = false;
                tdAddDetails.Visible = false;
                td1.Visible = false;
                tdSearch.Visible = true;


                //prasad
                txtDate.Visible = true;
                ddlHrsOut.Visible = true;
                ddlHrsIn.Visible = true;
                ddlMinsOut.Visible = true;
                ddlMinsIn.Visible = true;
                txtComments.Visible = true;

                ddlStatus.Visible = false;
                txtSearchFromDate.Visible = false;
                txtSearchToDate.Visible = false;
            }

            //Page.RegisterStartupScript("SetFocus", "<script>document.getElementById('" + rdbType.ClientID + "').focus();</script>");

            lblError.Text = "";
            //lblSuccess.Text = "";       
            txtSearchFromDate.Attributes.Add("onkeydown", "return false");
            txtSearchToDate.Attributes.Add("onkeydown", "return false");
            txtDate.Attributes.Add("onkeydown", "return false");
            btnSubmit.Attributes.Add("onClick", "return ButtonValidation();");
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "page_load", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region fillHrs
    public void fillHrs()
    {
        try
        {
            for (int i = 01; i <= 23; i++)
            {
                ddlHrsIn.Items.Add(i.ToString());
                ddlHrsOut.Items.Add(i.ToString());
            }
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "fillHrs", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region getReason
    public void getReason()
    {
        try
        {
            dsGetResonName = objOutOfOfficeBOL.GetReasonName();
            for (int i = 0; i < dsGetResonName.Tables[0].Rows.Count; i++)
            {
                rdbType.Items.Add(new ListItem(dsGetResonName.Tables[0].Rows[i]["Reason"].ToString(), dsGetResonName.Tables[0].Rows[i]["TypeId"].ToString()));
                rdbType.SelectedValue = "2";
            }

        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "getReason", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region resetControls
    public void resetControls()
    {
        try
        {
            txtDate.Text = "";
            txtComments.Text = "";
            ddlHrsIn.SelectedIndex = 0;
            ddlHrsIn.SelectedIndex = 0;
            ddlHrsOut.SelectedIndex = 0;
            ddlMinsIn.SelectedIndex = 0;
            ddlMinsOut.SelectedIndex = 0;
            txtSearchFromDate.Text = "";
            txtSearchToDate.Text = "";
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "resetControls", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region getOutOfOffice
    public void getOutOfOffice()
    {
        try
        {
          
            if (User.Identity.Name != null)
            {
                objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name);
            }           
            
            dsGetOutofOffice = objOutOfOfficeBOL.GetOutOfOffice(objOutOfOfficeModel);
            if (dsGetOutofOffice.Tables[0].Rows.Count <= 0)
            {
                lblSuccess.Text = "No Records Found";
                grdSignInSignOut.Visible = false;
            }
            else
            {
               // lblSuccess.Text = "";
                grdSignInSignOut.Visible = true;
                grdSignInSignOut.DataSource = dsGetOutofOffice.Tables[0];
                grdSignInSignOut.DataBind();
            }
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "getOutOfOffice", ex.StackTrace);
            throw new V2Exceptions();
        }


    }
    #endregion

    #region getStatus
    public void getStatus()
    {
        try
        {
            dsGetStatus = objOutOfOfficeBOL.GetStatus();
            ddlStatus.Items.Add(new ListItem("All", "0"));
            for (int i = 0; i < dsGetStatus.Tables[0].Rows.Count; i++)
            {
                ddlStatus.Items.Add(new ListItem(dsGetStatus.Tables[0].Rows[i]["StatusName"].ToString(), dsGetStatus.Tables[0].Rows[i]["StatusID"].ToString()));
            }
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "getStatus", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            SqlDataReader drOutOfOffice;
            if (User.Identity.Name != null)
            {
                objOutOfOfficeModel.UserId = Convert.ToInt32 (User.Identity.Name );
            }
            string strOutTime = txtDate.Text + " " + ddlHrsOut.SelectedValue + ":" + ddlMinsOut.SelectedItem.ToString();
            string strInTime = txtDate.Text + " " + ddlHrsIn.SelectedValue + ":" + ddlMinsIn.SelectedItem.ToString();

            objOutOfOfficeModel.OutTime = Convert.ToDateTime(strOutTime);
            objOutOfOfficeModel.InTime = Convert.ToDateTime(strInTime);
            objOutOfOfficeModel.Type = Convert.ToInt32(rdbType.SelectedValue);
            objOutOfOfficeModel.Comments = txtComments.Text.Trim();
            dsReportingTo = objOutOfOfficeBOL.GetReportingTo(objOutOfOfficeModel);
            if (dsReportingTo.Tables[0].Rows.Count > 0)
            {
                objOutOfOfficeModel.ApproverId = Convert.ToInt32(dsReportingTo.Tables[0].Rows[0]["ReporterID"].ToString());
            }
            else
            {
		 // If Reporting to is not assigned
		lblSuccess.Visible = false; 
                lblError.Visible = true;
                lblError.Text = "your Reporting To is not set. Please set it to appropriate person";
                return;
                // Prvious code	
                //objOutOfOfficeModel.ApproverId = Convert.ToInt32("");
               
            }

	     SignInSignOutModel objSignInSignOutModel = new SignInSignOutModel();
            objSignInSignOutModel.EmployeeID = Convert.ToInt32(HttpContext.Current.User.Identity.Name.ToString());
            ManualSignInSignOutBOL objManualSignInSignOutBOL = new ManualSignInSignOutBOL();
            DataSet dsEmployeeJoiningDate = new DataSet();
            DateTime JoiningDate;
            dsEmployeeJoiningDate = objManualSignInSignOutBOL.GetEmployeeJoiningData(objSignInSignOutModel);
            JoiningDate = Convert.ToDateTime(dsEmployeeJoiningDate.Tables[0].Rows[0]["DateOfJoining"]);

            if (Convert.ToDateTime(txtDate.Text).Date < JoiningDate)
            {
		lblSuccess.Visible = false;
                lblError.Visible = true;
                lblError.Text = "You are applying for Out Of Office before your Joining Date. Enter Valid Date ";
                return;

            }	    

            dsConfigItem = objOutOfOfficeBOL.GetOutOfOffice(objOutOfOfficeModel);
            DateTime ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());
            if (ConfigdateTime.Date >= Convert.ToDateTime(txtDate.Text).Date)
            {
                lblError.Text = "Administrator has frozen the data from " + ConfigdateTime.Date.ToShortDateString () + " . Select another date.";                           
            }
            else
            {
                lblSuccess.Text = "Record Added Successfully";
                drOutOfOffice = objOutOfOfficeBOL.AddOutOfOffice(objOutOfOfficeModel);
                


                Guid gOutOfOfficeWFID = new Guid("00000000-0000-0000-0000-000000000000");
                int OutOfOfficeID = 0;

                while (drOutOfOffice.Read())
                {
                    OutOfOfficeID = Convert.ToInt32(drOutOfOffice[0].ToString());
                    gOutOfOfficeWFID = new Guid(drOutOfOffice[1].ToString());
                    WorkflowRuntime wr = (WorkflowRuntime)Application["WokflowRuntime"];
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("OutOfOfficeID", OutOfOfficeID);
                    WorkflowInstance wi = wr.CreateWorkflow(typeof(OutOfOfficeWF.OutOfOfficeWF), parameters, gOutOfOfficeWFID);
                    wi.Start();

                }
                getOutOfOffice();
                resetControls();
            }
        }

        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            if (ex.Message.CompareTo("An entry for the selected Date and Time already exists") != 0)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "btnSubmit_Click", ex.StackTrace);
                throw new V2Exceptions();
            }
            else
            {
                lblSuccess.Text="";
                lblError.Text = ex.Message;
                getOutOfOffice();
                resetControls();
            }
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            resetControls();
            getOutOfOffice();
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "btnUpdate_Click", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region grdSignInSignOut_RowEditing
    protected void grdSignInSignOut_RowEditing(object sender, GridViewEditEventArgs e)
    {
        #region Old Code
        /*try
        {
            btnSubmit.Visible = false;           
            btnCancel.Visible = true;

            string strOutOfOFficeId = ((Label)grdSignInSignOut.Rows[e.NewEditIndex].FindControl("lblOutOfOFficeId")).Text;
            lblHidden.Text = strOutOfOFficeId;
            strDate = ((Label)(grdSignInSignOut.Rows[e.NewEditIndex].FindControl("lblOutDate"))).Text;
            txtDate.Text = strDate;

            strOutTime = ((Label)(grdSignInSignOut.Rows[e.NewEditIndex].FindControl("lblOuttime"))).Text;
            string[] strarOutTime = strOutTime.Split(':');
            ddlHrsOut.SelectedIndex = Convert.ToInt32(strarOutTime[0]) + 1;

            for (int i = 0; i < ddlMinsOut.Items.Count; i++)
            {
                if (ddlMinsOut.Items[i].Text.Trim() == strarOutTime[1].Trim())
                {
                    ddlMinsOut.SelectedIndex = i;
                    break;
                }
            }

            strInTime = ((Label)(grdSignInSignOut.Rows[e.NewEditIndex].FindControl("lblIntime"))).Text;
            string[] strarInTime = strInTime.Split(':');
            ddlHrsIn.SelectedIndex = Convert.ToInt32(strarInTime[0]) + 1;
            for (int i = 0; i < ddlMinsIn.Items.Count; i++)
            {
                if (ddlMinsIn.Items[i].Text.Trim() == strarInTime[1].Trim())
                {
                    ddlMinsIn.SelectedIndex = i;
                    break;
                }
            }
            strComments = ((Label)grdSignInSignOut.Rows[e.NewEditIndex].FindControl("lblComment")).Text;
            txtComments.Text = strComments;
            strReason = ((Label)grdSignInSignOut.Rows[e.NewEditIndex].FindControl("lblresonID")).Text;
            for (int i = 0; i < rdbType.Items.Count; i++)
            {
                if (rdbType.Items[i].Value == strReason)
                {
                    rdbType.Items[i].Selected = true;
                    break;
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
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "grdSignInSignOut_RowEditing", ex.StackTrace);
            throw new V2Exceptions();
        }*/

        #endregion

        try
        {
            grdSignInSignOut.EditIndex = e.NewEditIndex;
            getOutOfOffice();
            //tdbutton.Visible = false;
            //pnlAddDetails.Visible = false;
            //tdAddDetails.Visible = true;

            objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name);
            objOutOfOfficeModel.StatusId = Convert.ToInt32(ddlStatus.SelectedValue);

            if (Convert.ToInt32(ddlStatus.SelectedValue) != 0)
            {

                SearchOutOfOffice();
            }

            if (txtSearchFromDate.Text != "" && txtSearchToDate.Text != "")
            {

                objOutOfOfficeModel.FromDate = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                objOutOfOfficeModel.ToDate = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                SearchOutOfOfficeDateWise();
            }

        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "grdSignInSignOut_RowEditing", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region grdSignInSignOut_RowDeleting
    protected void grdSignInSignOut_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string strOutOfOFficeId = ((Label)grdSignInSignOut.Rows[e.RowIndex].FindControl("lblOutOfOFficeId")).Text;
        Label lblStatus = ((Label)grdSignInSignOut.Rows[e.RowIndex].FindControl("lblStatus"));
        objOutOfOfficeModel.OutOfOfficeID = Convert.ToInt32(strOutOfOFficeId);
        objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name);
        // objOutOfOfficeModel.StatusId = Convert.ToInt32(4);
        try
        {
            if (lblStatus.Text == "Approved")
            {
                lblSuccess.Text = "Approved out of office record cancelled Successfully";
                objOutOfOfficeBOL.DeleteOutOfOffice(objOutOfOfficeModel);
                createMailMessage();
                getOutOfOffice();
                td1.Visible = false;
                tdAddDetails.Visible = true;
                tdAddDetails.Visible = false;
                tdSearch.Visible = true;
                return;
            }
            //if (lblStatus.Text == "Pending")
            //{

            //}

            objOutOfOfficeBOL.DeleteOutOfOffice(objOutOfOfficeModel);
            lblSuccess.Text = "Record Cancelled Successfully";
            getOutOfOffice();
            /*td1.Visible = false;
            tdAddDetails.Visible = true;
            tdSearch.Visible = false;*/

        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "grdSignInSignOut_RowDeleting", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region grdSignInSignOut_PageIndexChanging
    protected void grdSignInSignOut_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdSignInSignOut.PageIndex = e.NewPageIndex;
            grdSignInSignOut.EditIndex = -1;
            getOutOfOffice();
            // get details statuswise and datewise.
            getSearchDetails();
            grdSignInSignOut.PageIndex = e.NewPageIndex;
            grdSignInSignOut.EditIndex = -1;
        }


        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "grdSignInSignOut_PageIndexChanging", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region btnSearch_Click
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            grdSignInSignOut.PageIndex = 0;
            objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name );
            objOutOfOfficeModel.StatusId = Convert.ToInt32(ddlStatus.SelectedValue);
            objOutOfOfficeModel.FromDate = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
            objOutOfOfficeModel.ToDate = Convert.ToDateTime(txtSearchToDate.Text.Trim());

            pnlSearch.Visible = true;
            grdSignInSignOut.EditIndex = -1;
            SearchOutOfOfficeDateWise();
            lnkAdddetails.Visible = true;

        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "btnSearch_Click", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region SearchOutOfOffice
    public void SearchOutOfOffice()
    {
        try
        {
            dsSearchOutOfOffice = objOutOfOfficeBOL.SearchOutOfOffice(objOutOfOfficeModel);
            if (dsSearchOutOfOffice.Tables[0].Rows.Count <= 0)
            {
                lblError.Text = "Records Not Found";
                lblSuccess.Text = "";
                grdSignInSignOut.Visible = false;
                pnlSearch.Visible = true;
                //btnSubmit.Visible = false;
                tdbutton.Visible = false;
            }
            else
            {
                lblError.Text = "";
                grdSignInSignOut.Visible = true;
                grdSignInSignOut.DataSource = dsSearchOutOfOffice;
                grdSignInSignOut.DataBind();
                // btnSubmit.Visible = false;
                tdbutton.Visible = false;
            }
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "SearchOutOfOffice", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region SearchOutOfOfficeDateWise
    public void SearchOutOfOfficeDateWise()
    {
        try
        {
            dssearchDatewise = objOutOfOfficeBOL.SearchOutOfOfficeDatewise(objOutOfOfficeModel);
            if (dssearchDatewise.Tables[0].Rows.Count <= 0)
            {
                lblError.Text = "Records Not Found";
                grdSignInSignOut.Visible = false;
                lblSuccess.Text = "";
                pnlSearch.Visible = true;
                //btnSubmit.Visible = false;
                tdbutton.Visible = false;
            }
            else
            {
                lblError.Text = "";
                grdSignInSignOut.Visible = true;
                grdSignInSignOut.DataSource = dssearchDatewise.Tables[0];
                grdSignInSignOut.DataBind();
                // btnSubmit.Visible = false;
                tdbutton.Visible = false;
            }
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "SearchOutOfOffice", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region grdSignInSignOut_RowDataBound
    protected void grdSignInSignOut_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                TextBox txtDate1 = ((TextBox)e.Row.FindControl("txtDate1"));
                TextBox txtComments = ((TextBox)e.Row.FindControl("txtComments"));
                txtDate1.Attributes.Add("onkeydown", "return false");
                DropDownList ddlOutTimeHrs = ((DropDownList)e.Row.FindControl("ddlOutTimeHrs"));
                DropDownList ddlInTimeHrs = ((DropDownList)e.Row.FindControl("ddlInTimeHrs"));
                DropDownList ddlOutTimeMins = ((DropDownList)e.Row.FindControl("ddlOutTimeMins"));
                DropDownList ddlInTimeMins = ((DropDownList)e.Row.FindControl("ddlInTimeMins"));

                Label lblOutTime = ((Label)e.Row.FindControl("lblOuttime1"));
                Label lblInTime = ((Label)e.Row.FindControl("lblIntime1"));
                for (int i = 01; i <= 23; i++)
                {
                    ddlOutTimeHrs.Items.Add(i.ToString());
                    ddlInTimeHrs.Items.Add(i.ToString());
                }

                string strOutTime = lblOutTime.Text.ToString();
                string[] strarOutTime = strOutTime.Split(':');
                ddlOutTimeHrs.SelectedIndex = Convert.ToInt32(strarOutTime[0]) - 1;

                for (int i = 0; i < ddlOutTimeMins.Items.Count; i++)
                {
                    if (ddlOutTimeMins.Items[i].Text.Trim() == strarOutTime[1].Trim())
                    {
                        ddlOutTimeMins.SelectedIndex = i;
                        break;
                    }
                }

                string strInTime = lblInTime.Text.ToString();
                string[] strarInTime = strInTime.Split(':');
                ddlInTimeHrs.SelectedIndex = Convert.ToInt32(strarInTime[0]) - 1;
                for (int i = 0; i < ddlInTimeMins.Items.Count; i++)
                {
                    if (ddlInTimeMins.Items[i].Text.Trim() == strarInTime[1].Trim())
                    {
                        ddlInTimeMins.SelectedIndex = i;
                        break;
                    }
                }

                dsGetResonName = objOutOfOfficeBOL.GetReasonName();
                DropDownList ddlReason = ((DropDownList)e.Row.FindControl("ddlReason"));
                Label lblresonID = ((Label)e.Row.FindControl("lblresonID"));
                for (int i = 0; i < dsGetResonName.Tables[0].Rows.Count; i++)
                {
                    ddlReason.Items.Add(new ListItem(dsGetResonName.Tables[0].Rows[i]["Reason"].ToString(), dsGetResonName.Tables[0].Rows[i]["TypeId"].ToString()));
                }
                ddlReason.SelectedValue = lblresonID.Text.ToString().Trim();
                ((LinkButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("onClick", "return UpdateValidation( " + txtDate1.ClientID + "," + ddlOutTimeHrs.ClientID + "," + ddlOutTimeMins.ClientID + "," + ddlInTimeHrs.ClientID + "," + ddlInTimeMins.ClientID + "," + txtComments.ClientID + " );");
                
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name);

                Label lblStatus = ((Label)e.Row.FindControl("lblStatus"));
                Label lblApproved = ((Label)e.Row.FindControl("lblApproved"));
                Label lblOutDate = ((Label)e.Row.FindControl("lblOutDate"));
                LinkButton lnkbtnEdit = ((LinkButton)e.Row.FindControl("lnkbtnEdit"));
                LinkButton lnkbtnDelete = ((LinkButton)e.Row.FindControl("lnkbtnCancel"));
                Label lblOuttime1 = ((Label)e.Row.FindControl("lblOuttime1"));


                dsConfigItem = objOutOfOfficeBOL.GetOutOfOffice(objOutOfOfficeModel);
                DateTime ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

                if (ConfigdateTime.Date >= Convert.ToDateTime(lblOutDate.Text).Date)
                {
                    if (lblStatus.Text == "Approved")
                    {
                        lblApproved.Text = "Approved";
                        lnkbtnEdit.Visible = false;
                        lnkbtnDelete.Visible = false;

                    }
                    if (lblStatus.Text == "Pending")
                    {
                        lblApproved.Text = "Pending";
                        lnkbtnEdit.Visible = false;
                        lnkbtnDelete.Visible = false;
                    }
                    if (lblStatus.Text == "Rejected")
                    {
                        lblApproved.Text = "Rejected";
                        lnkbtnEdit.Visible = false;
                        lnkbtnDelete.Visible = false;
                    }
                    if (lblStatus.Text == "Cancelled")
                    {
                        lblApproved.Text = "Cancelled ";
                        lnkbtnEdit.Visible = false;
                        lnkbtnDelete.Visible = false;
                    }
                }
                else
                {

                    if (lblStatus.Text == "Approved")
                    {
                        lnkbtnEdit.Visible = false;
                        lnkbtnDelete.Visible = true ;
                    }
                    if (lblStatus.Text == "Rejected")
                    {
                        lblApproved.Text = "Rejected";
                        lnkbtnEdit.Visible = false;
                        lnkbtnDelete.Visible = false;
                    }
                    if (lblStatus.Text == "Cancelled")
                    {
                        lblApproved.Text = "Cancelled";
                        lnkbtnEdit.Visible = false;
                        lnkbtnDelete.Visible = false;
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
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "grdSignInSignOut_RowDataBound", ex.StackTrace);
            throw new V2Exceptions();
        }

    }
    #endregion

    #region lnkSearch_Click
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        try
        {
            // Page.RegisterStartupScript("SetFocus", "<script>document.getElementById('" + ddlStatus.ClientID + "').focus();</script>");
            tdAddDetails.Visible = true;
            pnlSearch.Visible = true;
            tdSearch.Visible = false;
            td1.Visible = false;
            pnlAddDetails.Visible = false;
            tdbutton.Visible = false;
            ddlStatus.SelectedIndex = 0;
            grdSignInSignOut.PageIndex = 0;
            grdSignInSignOut.EditIndex = -1;
            lblSuccess.Text ="";
            lblError.Text="";

            txtDate.Visible = false;
            ddlHrsOut.Visible = false;
            ddlHrsIn.Visible = false;
            ddlMinsOut.Visible = false;
            ddlMinsIn.Visible = false;
            txtComments.Visible = false;

            ddlStatus.Visible = true;
            txtSearchFromDate.Visible = true;
            txtSearchToDate.Visible = true;

            selected_tab.Value = "Search";

            getOutOfOffice();
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "lnkSearch_Click", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region lnkAdddetails_Click
    protected void lnkAdddetails_Click(object sender, EventArgs e)
    {
        try
        {
            txtSearchFromDate.Text = "";
            txtSearchToDate.Text = "";
            tdSearch.Visible = true;
            td1.Visible = false;
            tdAddDetails.Visible = false;
            pnlAddDetails.Visible = true;
            pnlSearch.Visible = false;
            tdbutton.Visible = true;
            grdSignInSignOut.PageIndex = 0;
            grdSignInSignOut.EditIndex = -1;
            lblError.Text = "";
            lblSuccess.Text="";

            txtDate.Visible = true;
            ddlHrsOut.Visible = true;
            ddlHrsIn.Visible = true;
            ddlMinsOut.Visible = true;
            ddlMinsIn.Visible = true;
            txtComments.Visible = true;

            ddlStatus.Visible = false;
            txtSearchFromDate.Visible = false;
            txtSearchToDate.Visible = false;

            selected_tab.Value = "Add";

            getOutOfOffice();

        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "lnkAdddetails_Click", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region grdSignInSignOut_Sorting
    protected void grdSignInSignOut_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            pnlAddDetails.Visible = false;
            tdbutton.Visible = false;
            tdAddDetails.Visible = true;
            tdSearch.Visible = false;
            pnlSearch.Visible = true;
            objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name);
            dsGetOutofOffice = objOutOfOfficeBOL.GetOutOfOffice(objOutOfOfficeModel);
            grdSignInSignOut.DataSource = dsGetOutofOffice.Tables[0];
            grdSignInSignOut.DataBind();

            DataTable dtGetOutofOffice = dsGetOutofOffice.Tables[0];
            DataView dvGetOutofOffice = new DataView(dtGetOutofOffice);

            if ((ViewState["Order"] == null))
            {
                ViewState["Order"] = "ASC";
            }
            else if (ViewState["Order"].ToString() == "ASC")
            {
                ViewState["Order"] = "DESC";
            }
            else if (ViewState["Order"].ToString() == "DESC")
            {
                ViewState["Order"] = "ASC";
            }
            string strGetOutofOffice = this.ViewState["Order"].ToString();
            dvGetOutofOffice.Sort = e.SortExpression + " " + strGetOutofOffice;
            grdSignInSignOut.DataSource = dvGetOutofOffice;
            grdSignInSignOut.DataBind();

            objOutOfOfficeModel.StatusId = Convert.ToInt32(ddlStatus.SelectedValue);

            if (Convert.ToInt32(ddlStatus.SelectedValue) != 0)
            {
                dsSearchOutOfOffice = objOutOfOfficeBOL.SearchOutOfOffice(objOutOfOfficeModel);
                grdSignInSignOut.DataSource = dsSearchOutOfOffice.Tables[0];
                grdSignInSignOut.DataBind();
                DataTable dtSearchOutOfOffice = dsSearchOutOfOffice.Tables[0];
                DataView dvSearchOutOfOffice = new DataView(dtSearchOutOfOffice);

                if ((ViewState["Order"] == null))
                {
                    ViewState["Order"] = "ASC";
                }
                else if (ViewState["Order"].ToString() == "ASC")
                {
                    ViewState["Order"] = "DESC";
                }
                else if (ViewState["Order"].ToString() == "DESC")
                {
                    ViewState["Order"] = "ASC";
                }
                string strSearchOutOfOffice = this.ViewState["Order"].ToString();
                dvSearchOutOfOffice.Sort = e.SortExpression + " " + strSearchOutOfOffice;
                grdSignInSignOut.DataSource = dvSearchOutOfOffice;
                grdSignInSignOut.DataBind();


                if (txtSearchFromDate.Text != "" && txtSearchToDate.Text != "")
                {

                    objOutOfOfficeModel.FromDate = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                    objOutOfOfficeModel.ToDate = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                    dssearchDatewise = objOutOfOfficeBOL.SearchOutOfOfficeDatewise(objOutOfOfficeModel);

                    grdSignInSignOut.DataSource = dssearchDatewise.Tables[0];
                    grdSignInSignOut.DataBind();
                    DataTable dtsearchDatewise = dssearchDatewise.Tables[0];
                    DataView dvsearchDatewise = new DataView(dtsearchDatewise);

                    if ((ViewState["Order"] == null))
                    {
                        ViewState["Order"] = "ASC";
                    }
                    else if (ViewState["Order"].ToString() == "ASC")
                    {
                        ViewState["Order"] = "DESC";
                    }
                    else if (ViewState["Order"].ToString() == "DESC")
                    {
                        ViewState["Order"] = "ASC";
                    }
                    string strsearchDatewise = this.ViewState["Order"].ToString();
                    dvsearchDatewise.Sort = e.SortExpression + " " + strsearchDatewise;
                    grdSignInSignOut.DataSource = dvsearchDatewise;
                    grdSignInSignOut.DataBind();

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
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "grdSignInSignOut_Sorting", ex.StackTrace);
            throw new V2Exceptions();
        }

    } 
    #endregion

    #region grdSignInSignOut_RowUpdating
    protected void grdSignInSignOut_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Label lblOutOfOFficeId = ((Label)grdSignInSignOut.Rows[e.RowIndex].FindControl("lblOutOfOFficeId"));
            objOutOfOfficeModel.OutOfOfficeID = Convert.ToInt32(lblOutOfOFficeId.Text.ToString().Trim());
            Label lblUserid = ((Label)grdSignInSignOut.Rows[e.RowIndex].FindControl("lblUserid"));
            objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name) ;

            strDate = ((TextBox)grdSignInSignOut.Rows[e.RowIndex].FindControl("txtDate1")).Text.Trim();
            DropDownList ddlOutTimeHrs = ((DropDownList)grdSignInSignOut.Rows[e.RowIndex].FindControl("ddlOutTimeHrs"));
            DropDownList ddlOutTimeMins = ((DropDownList)grdSignInSignOut.Rows[e.RowIndex].FindControl("ddlOutTimeMins"));
            DropDownList ddlInTimeHrs = ((DropDownList)grdSignInSignOut.Rows[e.RowIndex].FindControl("ddlInTimeHrs"));
            DropDownList ddlInTimeMins = ((DropDownList)grdSignInSignOut.Rows[e.RowIndex].FindControl("ddlInTimeMins"));

            string strOutTime = strDate + " " + ddlOutTimeHrs.SelectedValue + ":" + ddlOutTimeMins.SelectedItem.Text.ToString();
            objOutOfOfficeModel.OutTime = Convert.ToDateTime(strOutTime.ToString().Trim());
            string strInTime = strDate + " " + ddlInTimeHrs.SelectedValue + ":" + ddlInTimeMins.SelectedItem.Text.ToString();
            objOutOfOfficeModel.InTime = Convert.ToDateTime(strInTime.ToString().Trim());
            DropDownList ddlReason = ((DropDownList)grdSignInSignOut.Rows[e.RowIndex].FindControl("ddlReason"));
            objOutOfOfficeModel.Type = Convert.ToInt32(ddlReason.SelectedValue);
            TextBox txtComments = ((TextBox)grdSignInSignOut.Rows[e.RowIndex].FindControl("txtComments"));
            objOutOfOfficeModel.Comments = txtComments.Text.Trim();


            TextBox txtDate1 = ((TextBox)grdSignInSignOut.Rows[e.RowIndex].FindControl("txtDate1"));
            dsConfigItem = objOutOfOfficeBOL.GetOutOfOffice(objOutOfOfficeModel);
            DateTime ConfigdateTime = Convert.ToDateTime(dsConfigItem.Tables[1].Rows[0]["ConfigItemValue"].ToString());

            if (ConfigdateTime.Date >= Convert.ToDateTime(txtDate1.Text).Date)
            {
                lblSuccess.Text = "";
                lblError.Text = "Administrator has frozen the data from  " + ConfigdateTime.Date.ToShortDateString() + " .Select another date.";
            }
            else
            {

                objOutOfOfficeBOL.UpdateOutOffice(objOutOfOfficeModel);
                lblSuccess.Text = "Record Updated Successfully";
                grdSignInSignOut.EditIndex = -1;
                getOutOfOffice();
                /*tdAddDetails.Visible = true;
                tdSearch.Visible = false;
                td1.Visible = false;*/
                getSearchDetails();
            }
        }

        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            if (ex.Message.CompareTo("An entry for the selected Date and Time already exists") != 0)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "btnUpdate_Click", ex.StackTrace);
                throw new V2Exceptions();
            }
            else
            {
                lblSuccess.Text="";
                lblError.Text = ex.Message;
            }

        }

    }
    #endregion

    #region grdSignInSignOut_RowCancelingEdit
    protected void grdSignInSignOut_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            grdSignInSignOut.EditIndex = -1;
            getOutOfOffice();
            //tdAddDetails.Visible = true;
            //tdSearch.Visible = false;
            //td1.Visible = false;
            getSearchDetails();
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "btnUpdate_Click", ex.StackTrace);
            throw new V2Exceptions();
        }


    }
    #endregion

    #region getSearchDetails
    public void getSearchDetails()
    {

        try
        {
            objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name);
            objOutOfOfficeModel.StatusId = Convert.ToInt32(ddlStatus.SelectedValue);
            if (Convert.ToInt32(ddlStatus.SelectedValue) != 0)
            {

                SearchOutOfOffice();
            }

            if (txtSearchFromDate.Text != "" && txtSearchToDate.Text != "")
            {

                objOutOfOfficeModel.FromDate = Convert.ToDateTime(txtSearchFromDate.Text.Trim());
                objOutOfOfficeModel.ToDate = Convert.ToDateTime(txtSearchToDate.Text.Trim());
                SearchOutOfOfficeDateWise();
            }
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "getSearchDetails", ex.StackTrace);
            throw new V2Exceptions();
        }

    } 
    #endregion

    #region ddlStatus_SelectedIndexChanged
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdSignInSignOut.PageIndex = 0;
            objOutOfOfficeModel.UserId = Convert.ToInt32(User.Identity.Name );
            if (ddlStatus.SelectedItem.Text == "All")
            {
                objOutOfOfficeModel.StatusId = 0;
                objOutOfOfficeModel.FromDate = Convert.ToDateTime(null);
                objOutOfOfficeModel.ToDate = Convert.ToDateTime(null);
            }
            else if (Convert.ToInt32(ddlStatus.SelectedValue) >= 1)
            {

                objOutOfOfficeModel.StatusId = Convert.ToInt32(ddlStatus.SelectedValue);
                objOutOfOfficeModel.FromDate = Convert.ToDateTime(null);
                objOutOfOfficeModel.ToDate = Convert.ToDateTime(null);
            }
            pnlSearch.Visible = true;

            SearchOutOfOffice();
            lnkAdddetails.Visible = true;
            txtSearchFromDate.Text = "";
            txtSearchToDate.Text = "";
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "ddlStatus_SelectedIndexChanged", ex.StackTrace);
            throw new V2Exceptions();
        }
    }
    #endregion

    #region getSearchDetails
    public void createMailMessage()
    {
        try
        {
            dsReportingTo = objOutOfOfficeBOL.GetReportingTo(objOutOfOfficeModel);

            if (dsReportingTo.Tables[0].Rows.Count > 0)
            {
                //Send mail to Approved Out of office cancelled report.
                MailMessage objMailMessage = new MailMessage();
                string Reason, Date, OutTime, InTime;

                //EmployeeName = dsReportingTo.Tables[0].Rows[0]["EmployeeName"].ToString();
                //ApproverName = dsReportingTo.Tables[0].Rows[0]["ReporterName"].ToString();
                objMailMessage.To = dsReportingTo.Tables[0].Rows[0]["ReporterMailID"].ToString();
                dsCancelOutOfOffice = objOutOfOfficeBOL.CancelOutOfOffice(objOutOfOfficeModel);

                Date = Convert.ToDateTime(dsCancelOutOfOffice.Tables[0].Rows[0]["Date"]).ToShortDateString();
                OutTime = dsCancelOutOfOffice.Tables[0].Rows[0]["OutTime"].ToString();
                InTime = dsCancelOutOfOffice.Tables[0].Rows[0]["InTime"].ToString();
                Reason = dsCancelOutOfOffice.Tables[0].Rows[0]["Comment"].ToString();
                for (int k = 0; k < dsReportingTo.Tables[1].Rows.Count; k++)
                {
                    if (dsReportingTo.Tables[1].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                    {
                        objMailMessage.From = dsReportingTo.Tables[1].Rows[k]["ConfigItemValue"].ToString();
                    }
                    if (dsReportingTo.Tables[1].Rows[k]["ConfigItemName"].ToString() == "Mail Server Name")
                    {
                        //SmtpMail.SmtpServer = dsReportingTo.Tables[1].Rows[k]["ConfigItemValue"].ToString();
                        break;
                    }
                }

                objMailMessage.Subject = "Approved Out of office Entry was cancelled. ";
                objMailMessage.BodyFormat = MailFormat.Html;
                objMailMessage.Body = "<font style= color:Navy;font-size:smaller;font-family:Arial>" + "Hi" + " " + "<b>" + dsReportingTo.Tables[0].Rows[0]["ReporterName"].ToString() + "</b>" + " ," + "<br> <br>" + "Employee Name : " + "<b>" +  dsReportingTo.Tables[0].Rows[0]["EmployeeName"].ToString() + "</b>" + " " + "<br><br>" + "Date :" + Date + "<br><br>" + "OutTime : " + OutTime + "<br><br>" + "InTime :" + InTime + "<Br><br>" + "Reason :" + Reason + "<br> <br>"+ "<b>" + dsReportingTo.Tables[0].Rows[0]["EmployeeName"].ToString() + "</b>" + " was Cancelled the Approved Out of Office Entry, the required updates are made in the system.";


                //objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                //objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "v2system");
                //objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "mail_123");

                SmtpMail.Send(objMailMessage);
            }
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "createMailMessage", ex.StackTrace);
            throw new V2Exceptions();
        }


        /* This is old Mail format
        if (dsReportingTo.Tables[0].Rows.Count > 0)
        {
            //Send mail to Approved Out of office cancelled repot.
            MailMessage objMailMessage = new MailMessage();
            objMailMessage.To = dsReportingTo.Tables[0].Rows[0]["ReporterMailID"].ToString();
            for (int k = 0; k < dsReportingTo.Tables[1].Rows.Count; k++)
            {
                if (dsReportingTo.Tables[1].Rows[k]["ConfigItemName"].ToString() == "From EmailID")
                {
                    objMailMessage.From = dsReportingTo.Tables[1].Rows[k]["ConfigItemValue"].ToString();
                }
                if (dsReportingTo.Tables[1].Rows[k]["ConfigItemName"].ToString() == "Mail Server Name")
                {
                    //SmtpMail.SmtpServer = dsReportingTo.Tables[1].Rows[k]["ConfigItemValue"].ToString();
                }
            }

            objMailMessage.Subject = "Approved Out of office Entry was cancelled. ";
            objMailMessage.BodyFormat = MailFormat.Html;

            for (int i = 0; i < dsReportingTo.Tables[0].Rows.Count; i++)
            {
                objMailMessage.Body = "Hi " + dsReportingTo.Tables[0].Rows[i]["ReporterName"].ToString() + " ," + "<br>" + dsReportingTo.Tables[0].Rows[i]["EmployeeName"].ToString() + " " + " was Cancelled the Approved Out of Office Entry, the required updates are made in the system.";
            }
            //SmtpClient objSmtpClient = new SmtpClient();
            SmtpMail.Send(objMailMessage);
            //objSmtpClient.Send(objMailMessage);
         
        }*/

    } 
    #endregion

    #region btnReset_Click
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try 
        {
            txtSearchFromDate.Text = "";
            txtSearchToDate.Text = "";
        }
        catch (V2Exceptions ex)
        {
            throw;
        }
        catch (System.Exception ex)
        {
            FileLog objFileLog = FileLog.GetLogger();
            objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOffice.aspx.cs", "getSearchDetails", ex.StackTrace);
            throw new V2Exceptions();
        }
    } 
    #endregion
}