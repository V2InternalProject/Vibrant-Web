using HRMS.DAL;
using HRMS.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;

namespace HRMS.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            //return View();
            //return Redirect(System.Configuration.ConfigurationManager.AppSettings["Log-OffURL"]);

            ////GoLive Code for redirecting to Login Page
            bool isDebug = false;
            if (System.Configuration.ConfigurationManager.AppSettings["IsDebug"] != null)
                isDebug = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsDebug"]);

            if (isDebug)
                return View();
            else
                return Redirect(System.Configuration.ConfigurationManager.AppSettings["Log-OffURL"]);
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)        {
            if (ModelState.IsValid)
            {
                model.UserName = model.UserName.Trim();
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        string[] role = Roles.GetRolesForUser(model.UserName);
                        if (role == null || role.Count() <= 0)
                        {
                            FormsAuthentication.SignOut();
                            return RedirectToAction("Index", "Error", new { errorCode = "Error403" });
                        }
                        EmployeeDAL employeeDAL = new EmployeeDAL();
                        SemDAL SEMdal = new SemDAL();
                        int employeeID = employeeDAL.GetEmployeeID(model.UserName);
                        int semEmployeeId = SEMdal.geteEmployeeIDFromSEMDatabase(model.UserName);
                        Guid globalID = Guid.NewGuid();
                        Session["LoggedInEmployee"] = model.UserName;
                        Session["LoggedInEmployeeSEMID"] = semEmployeeId;
                        Session["SecurityKey"] = globalID.ToString();
                        string encryptedEmployeeid = Encrypt(Convert.ToString(Session["SecurityKey"].ToString() + employeeID), true);
                        Session["encryptedLoggedinEmployeeID"] = encryptedEmployeeid;
                        string current_DU = employeeDAL.getCurrentDU(model.UserName);
                        Session["Current_DU"] = current_DU;
                        Session["ViewNode"] = employeeDAL.GetViewableNodesForEmployee(Convert.ToInt32(model.UserName));
                        CommonMethodsDAL Commondal = new CommonMethodsDAL();
                        string maxRole = Commondal.GetMaxRoleForUser(role);
                        Session["MaxUserRole"] = maxRole;

                        //for Help desk Tab Access
                        //if (User.IsInRole("Super Admin") == false)
                        //    Session

                        // return RedirectToAction("Index", "PersonalDetails", new { employeeId = encryptedEmployeeid });
                        //return RedirectToAction("Index", "Orbit");
                        if (HttpContext.User.IsInRole("Super Admin"))
                        {
                            Session["SuperAdmin"] = model.UserName;
                        }
                        else
                            Session["SuperAdmin"] = 0;
                        Session["EmployeeID"] = model.UserName;
                        HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsByEmployeeCode(model.UserName);
                        Session["LoggedInEmployeeName"] = employee.EmployeeName;
                        if (employee != null)
                        {
                            Session["UserName"] = employee.FirstName;
                        }
                        //added code for access mapping
                        Session["AccessRights"] = employeeDAL.GetPageAccessMapping(model.UserName);

                        XmlDocument doc = new XmlDocument();

                        string data = employeeDAL.GetPageAccessMapping_xmlData(model.UserName);

                        doc.LoadXml(data);

                        Session["MenuDataList"] = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, true);

                        return RedirectToAction("PersonalDetails", "PersonalDetails", new { employeeId = encryptedEmployeeid });

                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogIn()
        {
            try
            {
                LogOnModel model = new LogOnModel();
                string userName = Convert.ToString(Request.Form["UserName"]);
                userName = userName.Trim();
                string password = Convert.ToString(Request.Form["pwd"]);
                string employeeCode = string.Empty;
                //if (Session["LoggedInEmployee"] != null)
                //    employeeCode = Session["LoggedInEmployee"].ToString();
                //if (employeeCode == "")
                //{
                if (Membership.ValidateUser(userName, password))
                {
                    FormsAuthentication.SetAuthCookie(userName, true);
                    string[] role = Roles.GetRolesForUser(userName);
                    if (role == null || role.Count() <= 0)
                    {
                        FormsAuthentication.SignOut();
                        return RedirectToAction("Index", "Error", new { errorCode = "Error403" });
                    }
                    EmployeeDAL employeeDAL = new EmployeeDAL();
                    SemDAL SEMdal = new SemDAL();
                    int employeeID = employeeDAL.GetEmployeeID(userName);
                    int semEmployeeId = SEMdal.geteEmployeeIDFromSEMDatabase(userName);
                    Session["LoggedInEmployee"] = userName;
                    Session["LoggedInEmployeeSEMID"] = semEmployeeId;
                    Guid globalID = Guid.NewGuid();
                    Session["SecurityKey"] = globalID.ToString();
                    string encryptedEmployeeid = Encrypt(Convert.ToString(Session["SecurityKey"].ToString() + employeeID), true);
                    Session["encryptedLoggedinEmployeeID"] = encryptedEmployeeid;
                    string current_DU = employeeDAL.getCurrentDU(userName);
                    if (string.IsNullOrEmpty(current_DU))
                        current_DU = "0";
                    Session["Current_DU"] = current_DU;
                    //return RedirectToAction("Index", "PersonalDetails", new { employeeId = encryptedEmployeeid });
                    ////GoLive code for redirecting to orbit
                    if (HttpContext.User.IsInRole("Super Admin"))
                    {
                        Session["SuperAdmin"] = userName;
                    }
                    else
                        Session["SuperAdmin"] = 0;
                    Session["EmployeeID"] = userName;
                    HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsByEmployeeCode(userName);
                    if (employee != null)
                    {
                        Session["UserName"] = employee.FirstName;
                    }
                    //added code for access mapping
                    Session["AccessRights"] = employeeDAL.GetPageAccessMapping(userName);

                    XmlDocument doc = new XmlDocument();

                    string data = employeeDAL.GetPageAccessMapping_xmlData(userName);

                    doc.LoadXml(data);

                    Session["MenuDataList"] = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, true);

                    return RedirectToAction("PersonalDetails", "PersonalDetails", new { employeeId = encryptedEmployeeid });
                }
                else
                {
                    model.LogOffURL = System.Configuration.ConfigurationManager.AppSettings["Log-OffURL"];
                    return View(model);
                }
                //}
                //else
                //{
                //    model.IsValidSession = false;
                //    return View(model);
                //}
            }
            catch
            {
                throw;
            }
        }

        //Encrypt ID
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new System.Configuration.AppSettingsReader();
            // Get the key from config file
            string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            HttpContext.Session.Abandon();
            //return RedirectToAction("LogOn", "Account");
            return Redirect(System.Configuration.ConfigurationManager.AppSettings["Log-OffURL"]);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    // MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    MembershipUser currentUser = Membership.GetUser(model.UserName);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult NotAuthorized()
        {
            return View();
        }

        public ActionResult GetUploadNameFromUploadById(string empcode)
        {
            try
            {
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                int employeeID = 0;
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetailsByEmployeeCode(empcode);
                if (employeeDetails != null)
                    employeeID = employeeDetails.EmployeeID;
                string EmpName = personalDAL.GetDisplayName(employeeID);
                return Json(EmpName, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ForgotPassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool ResetPasswordSucceeded = false;
                string newPassword = string.Empty;
                try
                {
                    // MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    MembershipUser currentUser = Membership.GetUser(model.EmployeeId.ToString());
                    newPassword = currentUser.ResetPassword();   //..

                    if (newPassword.Trim() != string.Empty)
                        ResetPasswordSucceeded = true;

                    //m.ChangePassword(strPassword, "mail_123"); //..
                    //ResetPasswordSucceeded = currentUser.ChangePassword(newPassword, "mail_123");
                    EmployeeDAL employeeDAL = new EmployeeDAL();
                    var empDetails = employeeDAL.GetUserDetailByEmployeeCode(model.EmployeeId.ToString());
                    string empName = empDetails.EmployeeName;
                    string empEmailId = empDetails.EmailID;
                    //Send  new password to the employee through Email.
                    MailMessage objMailMessage = new MailMessage();

                    objMailMessage.From = "pmo@in.v2solutions.com";
                    objMailMessage.To = empEmailId.Trim().ToString();
                    objMailMessage.Subject = "PMS Password Reset";
                    objMailMessage.BodyFormat = MailFormat.Html;

                    objMailMessage.Body = "Hi " + empName.Trim() + "," + "<br>" + "</br>" + "Your password has been Reset, the new PMS password is " + "<b>" + newPassword + "</b>" + "</br>" + "Please change the password as soon as you receive this mail." + "<br>" + "</br>" +
                        //    "<A HREF='http://intranet/'>http://intranet/</a>" + "<br>" + "</br>" + "Best Regards" + "</br>" + "PMO Department.";
                    "<A HREF='http://myvibrantweb.v2solutions.com'>http://myvibrantweb.v2solutions.com</a>" + "<br>" + "</br>" + "Best Regards" + "</br>" + "PMO Department.";

                    SmtpMail.SmtpServer = ConfigurationManager.AppSettings.Get("SMTPServerName").ToString();

                    objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "0");
                    objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "v2system");
                    objMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "mail_123");

                    SmtpMail.Send(objMailMessage);

                    //txtUserID.Text = "";
                    //lblError.Text = "";
                    //lblMessage.Text = "Password reset successfully.";
                    //lblMessage.Visible = true;
                    //lblUserID.Text = "";
                    //lblUserName.Text = "";
                    //lblEmail.Text = "";
                    //pnlEnterUser.Visible = true;
                    //pnlUserInfo.Visible = false;
                }
                catch (Exception)
                {
                    ResetPasswordSucceeded = false;
                }

                if (ResetPasswordSucceeded)
                {
                    return RedirectToAction("ForgotPasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to reset password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ForgotPasswordSuccess()
        {
            return View();
        }

        #region Status Codes

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion Status Codes
    }
}