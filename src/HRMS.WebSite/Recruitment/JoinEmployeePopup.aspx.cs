using BLL;
using BOL;
using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;

namespace HRMS.Recruitment
{
    public partial class JoinEmployeePopup : System.Web.UI.Page
    {
        private static Int64 employeeCode;
        private JoinEmployeeBLL objJoinEmployeeBLL = new JoinEmployeeBLL();
        private JoinEmployeeBOL objJoinEmployeeBOL = new JoinEmployeeBOL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DateTime currentdate = new DateTime();
                currentdate = System.DateTime.Now;
                txtJoiningDate.Text = currentdate.ToShortDateString();

                DataSet CheckUserNameEmail = new DataSet();
                objJoinEmployeeBOL.EmailId = Convert.ToString(Session["UserName"] + "@v2solutions.com");
                objJoinEmployeeBOL.UserName = Convert.ToString(Session["UserName"]);
                CheckUserNameEmail = objJoinEmployeeBLL.CheckUserNameEmail(objJoinEmployeeBOL);

                if (CheckUserNameEmail.Tables[0].Rows.Count != 0)
                {
                    if (CheckUserNameEmail.Tables[0].Rows[0][0].ToString() != "0")
                        lblUserNameError.Visible = true;
                }
                //
                if (CheckUserNameEmail.Tables[1].Rows.Count != 0)
                {
                    if (CheckUserNameEmail.Tables[1].Rows[0][0].ToString() != "0")
                        lblEmailError.Visible = true;
                }

                txtUserName.Text = Convert.ToString(Session["UserName"]);
                txtEmailID.Text = Convert.ToString(Session["UserName"] + "@v2solutions.com");
                objJoinEmployeeBOL.IsContract = 1;
                employeeCode = objJoinEmployeeBLL.GetLatestEmployeeCode(objJoinEmployeeBOL);
                txtEmployeeCode.Text = Convert.ToString(employeeCode);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtEmployeeCode.Text != string.Empty && txtJoiningDate.Text != string.Empty && txtUserName.Text != string.Empty && txtEmailID.Text != string.Empty)
            {
                DataSet CheckUserNameEmail = new DataSet();
                objJoinEmployeeBOL.EmailId = Convert.ToString(txtEmailID.Text);
                objJoinEmployeeBOL.UserName = Convert.ToString(txtUserName.Text);
                CheckUserNameEmail = objJoinEmployeeBLL.CheckUserNameEmail(objJoinEmployeeBOL);

                if (CheckUserNameEmail.Tables[0].Rows.Count != 0)
                {
                    if (CheckUserNameEmail.Tables[0].Rows[0][0].ToString() != "0")
                    {
                        lblUserNameError.ForeColor = System.Drawing.Color.Red;
                        lblUserNameError.Visible = true;
                    }
                }
                //
                if (CheckUserNameEmail.Tables[1].Rows.Count != 0)
                {
                    if (CheckUserNameEmail.Tables[1].Rows[0][0].ToString() != "0")
                    {
                        lblEmailError.ForeColor = System.Drawing.Color.Red;
                        lblEmailError.Visible = true;
                    }
                }

                if (CheckUserNameEmail.Tables[0].Rows[0][0].ToString() == "0" && CheckUserNameEmail.Tables[1].Rows[0][0].ToString() == "0")
                {
                    lblUserNameError.Visible = false;
                    lblEmailError.Visible = false;
                    objJoinEmployeeBOL.Employeecode = Convert.ToInt32(txtEmployeeCode.Text);
                    objJoinEmployeeBOL.UserName = Convert.ToString(txtUserName.Text);
                    objJoinEmployeeBOL.EmailId = Convert.ToString(txtEmailID.Text);
                    objJoinEmployeeBOL.CandidateId = Convert.ToInt32(Session["CandidateID"]);
                    objJoinEmployeeBOL.JoiningDate = Convert.ToDateTime(txtJoiningDate.Text);
                    DataSet dsEmployeeCreated = new DataSet();
                    dsEmployeeCreated = objJoinEmployeeBLL.CreaetNewEmployee(objJoinEmployeeBOL);

                    MembershipCreateStatus objMembershipCreateStatus = new MembershipCreateStatus();
                    if (dsEmployeeCreated.Tables != null)
                    {
                        if (dsEmployeeCreated.Tables[0].Rows.Count != 0)
                        {
                            if (Convert.ToString(dsEmployeeCreated.Tables[0].Rows[0][0]) == "0")
                                Membership.CreateUser(Convert.ToString(objJoinEmployeeBOL.Employeecode), "mail_123", objJoinEmployeeBOL.EmailId, "Question", "Answer", true, out objMembershipCreateStatus);
                        }
                    }
                    string strSuccess = "";
                    strSuccess = objMembershipCreateStatus.ToString();
                    if (objMembershipCreateStatus.ToString() == "Success")
                    {
                        lblSuccess.Visible = true;
                        lblSuccess.ForeColor = System.Drawing.Color.Green;
                        lblSuccess.Text = "Employee created successfully";
                    }
                    else
                    {
                        lblSuccess.Visible = true;
                        lblSuccess.ForeColor = System.Drawing.Color.Red;
                        lblSuccess.Text = "Employee not created";
                    }

                    Session["EmployeeCode"] = Convert.ToString(txtEmployeeCode.Text);
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('SendMailPopUP.aspx',null,'height=500, width=600,status= no, resizable= no, scrollbars=yes, toolbar=no,location=no,menubar=no');", true);
                }
            }
            else
            {
                lblEmailError.Visible = true;
                lblUserNameError.Visible = true;
                lblEmailError.ForeColor = System.Drawing.Color.Red;
                lblUserNameError.ForeColor = System.Drawing.Color.Red;
                lblEmailError.Text = "Please enter proper Email Id";
                lblUserNameError.Text = "Please enter proper UserName";
            }

            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('SendMailPopUP.aspx?height=250, width=1200,status= no, resizable= no, scrollbars=yes, toolbar=no,location=no,menubar=no', null, 'status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no' );", true);

            // Response.Redirect("SendMailPopUP.aspx");
        }

        protected void chkIscontract_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIscontract.Checked == true)
            {
                objJoinEmployeeBOL.IsContract = 0;
            }
            else
            {
                objJoinEmployeeBOL.IsContract = 1;
            }

            employeeCode = objJoinEmployeeBLL.GetLatestEmployeeCode(objJoinEmployeeBOL);
            txtEmployeeCode.Text = Convert.ToString(employeeCode);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.close();", true);
        }
    }
}