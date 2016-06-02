using HRMS.Attributes;
using HRMS.DAL;
using HRMS.Models;
using HRMS.Notification;
using MvcApplication3.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    [CustomAuthorize(Roles = "HR Admin,RMG")]
    public class ApprovalsController : Controller
    {
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();
        private EmployeeDAL employeeDAL = new EmployeeDAL();
        private int[] collection;

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                EmployeeChangesApprovalViewModel model = new EmployeeChangesApprovalViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                string employeeCode = Membership.GetUser().UserName;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                string[] role = Roles.GetRolesForUser(employeeCode);
                string maxRole = Commondal.GetMaxRoleForUser(role);
                if (employeeCode != null)
                {
                    model.EmployeeID = employeeId;
                    model.SearchedUserDetails.EmployeeId = employeeId;
                    model.SearchedUserDetails.EmployeeCode = employeeCode;
                    model.SearchedUserDetails.UserRole = maxRole;
                    ViewBag.UserRole = maxRole;
                }
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        [HttpGet]
        [PageAccess(PageName = "Approvals")]
        public ActionResult EmployeeApprovals(int? employeeId)
        {
            Session["SearchEmpFullName"] = null;  // to hide emp search
            Session["SearchEmpCode"] = null;
            Session["SearchEmpID"] = null;

            if (employeeId == null)
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                employeeId = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
            }
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "PersonalDetails");
            }
            try
            {
                EmployeeChangesApprovalViewModel changesApprovalModel = new EmployeeChangesApprovalViewModel();
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                tbl_ApprovalChanges approvalChanges = dal.GetChangedFields(employeeId);

                if (approvalChanges != null)
                {
                    changesApprovalModel.EmployeeID = approvalChanges.EmployeeID;
                    changesApprovalModel.FieldDiscription = approvalChanges.FieldDiscription;
                    changesApprovalModel.Module = approvalChanges.Module;
                    changesApprovalModel.FieldDbColumnName = approvalChanges.FieldDbColumnName;
                    changesApprovalModel.OldValue = approvalChanges.OldValue;
                    changesApprovalModel.NewValue = approvalChanges.NewValue;
                    changesApprovalModel.ApprovalStatusMasterID = approvalChanges.ApprovalStatusMasterID;
                    changesApprovalModel.Comments = approvalChanges.Comments;
                    changesApprovalModel.CreatedBy = approvalChanges.CreatedBy;
                    changesApprovalModel.CreatedDateTime = approvalChanges.CreatedDateTime;
                }
                changesApprovalModel.ChangeDetailsList = new List<EmployeeChangeDetails>();
                changesApprovalModel.Mail = new EmployeeMailTemplate();
                changesApprovalModel.EmployeeID = employeeId;
                return PartialView("_EmployeeApprovals", changesApprovalModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult SearchEmployeeAutoSuggest(string term)
        {
            PersonalDetailsDAL dal = new PersonalDetailsDAL();
            List<ApprovalEmployeeDetails> searchResult = new List<ApprovalEmployeeDetails>();
            searchResult = dal.SearchEmployeeNameModule(term, 1, 20);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EmployeeApprovalsLoadGrid(int page, int rows, string term, string Module)
        {
            try
            {
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                EmployeeDAL empdal = new EmployeeDAL();
                EmployeeChangesApprovalViewModel objChangesApprovalModel = new Models.EmployeeChangesApprovalViewModel();
                int totalCount;
                int LoggedInEmployeeId = empdal.GetEmployeeID(Membership.GetUser().UserName);

                List<EmployeeChangesApprovalViewModel> ChangesApprovalList = dal.GetEmployeeChangeDetails(page, rows, term, Module, LoggedInEmployeeId, out totalCount);
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);

                if ((ChangesApprovalList == null || ChangesApprovalList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    ChangesApprovalList = dal.GetEmployeeChangeDetails(page, rows, term, Module, LoggedInEmployeeId, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = ChangesApprovalList,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult LoadCertificationGrid(int? employeeId, string module, int page, int rows)
        {
            try
            {
                CertificationDetailsDAL dal = new CertificationDetailsDAL();
                CertificationDetailsViewModel model = new CertificationDetailsViewModel();

                int totalCount;
                model.CertificationsList = dal.GetEmpCertificateDetailAndHistory(page, rows, employeeId.GetValueOrDefault(), out totalCount, module);

                if ((model.CertificationsList == null || model.CertificationsList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model.CertificationsList = dal.GetEmployeeCertificationHistoryDetails(page, rows, employeeId.GetValueOrDefault(), out totalCount);
                }

                var jsonData = new
                {
                    total = (int)Math.Ceiling((double)totalCount / (double)rows),
                    page = page,
                    records = totalCount,
                    rows = model.CertificationsList
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SelectedModuleDetails(int employeeId, string module, int page, int rows)
        {
            try
            {
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                EmployeeChangesApprovalViewModel changesApprovalModel = new EmployeeChangesApprovalViewModel();
                int totalCount;
                changesApprovalModel.ChangeDetailsList = new List<EmployeeChangeDetails>();
                changesApprovalModel.ChangeDetailsList = dal.SelectedModuleDetailsList(employeeId, module, page, rows, out totalCount);
                if ((changesApprovalModel.ChangeDetailsList == null || changesApprovalModel.ChangeDetailsList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    changesApprovalModel.ChangeDetailsList = dal.SelectedModuleDetailsList(employeeId, module, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = changesApprovalModel.ChangeDetailsList,
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult AdminApprovalQualificationsLoadGrid(int employeeId, string module, int page, int rows)
        {
            try
            {
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                EmployeeQualificationsViewModel model = new EmployeeQualificationsViewModel();

                int totalCount;
                model.EmployeeQualificationsList = dal.GetAdminEmployeeQualificationsApprovalDetails(page, rows, employeeId, out totalCount, module);

                if ((model.EmployeeQualificationsList == null || model.EmployeeQualificationsList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model.EmployeeQualificationsList = dal.GetAdminEmployeeQualificationsApprovalDetails(page, rows, employeeId, out totalCount, module);
                }

                var jsonData = new
                {
                    total = (int)Math.Ceiling((double)totalCount / (double)rows),
                    page = page,
                    records = totalCount,
                    rows = model.EmployeeQualificationsList
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SaveEmployeeQualificationStatus(List<EmployeeQualifications> model, string comments)
        {
            try
            {
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                var saveStatus = dal.SaveQualificationMatrixHistory(model, comments);
                return Json(new { status = saveStatus }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveApprovalStatus(EmployeeChangesApprovalViewModel model, FormCollection frmColl)
        {
            try
            {
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                bool success = false;
                string result = null;
                if (model != null)
                {
                    success = dal.saveApprovalStatus(model);
                    if (success)
                        result = "saved";
                    else
                        result = "error";
                    return Json(new { status = success, resultmessage = result }, JsonRequestBehavior.AllowGet);
                }
                else
                    result = "ErrorInSave";
                return Json(new { status = success, resultmessage = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveCertificateDetailStatus(List<CertificationDetails> model, string CertHrComment)
        {
            try
            {
                CertificationDetailsDAL dal = new CertificationDetailsDAL();
                var saveStatus = dal.SaveCertificationMatrixHistory(model, CertHrComment);
                return Json(new { status = saveStatus }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult LoadSkillGrid(int? employeeId, string module, int page, int rows)
        {
            try
            {
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                SkillDetailsViewModel model = new SkillDetailsViewModel();

                int totalCount;

                model.EmployeeSkillDetails = dal.GetEmpSkillDetailsAndHistory(page, rows, employeeId.GetValueOrDefault(), out totalCount, module);

                if ((model.EmployeeSkillDetails == null || model.EmployeeSkillDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model.EmployeeSkillDetails = dal.GetEmployeeSkillHistoryDetails(page, rows, employeeId.GetValueOrDefault(), out totalCount);
                }

                var jsonData = new
                {
                    total = (int)Math.Ceiling((double)totalCount / (double)rows),
                    page = page,
                    records = totalCount,
                    rows = model.EmployeeSkillDetails
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SaveSkillDetailStatus(List<SkillDetails> model, string SkillHrComment)
        {
            try
            {
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                var saveStatus = dal.SaveEmployeeSkillMatrixHistory(model, SkillHrComment);
                return Json(new { status = saveStatus }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AdminSendMail(EmployeeChangesApprovalViewModel model)
        {
            try
            {
                if (model.Module == "New Personal Details" || model.Module == "New Residential Details" || model.Module == "OnHold Personal Details" || model.Module == "OnHold Residential Details")
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (var item in model.ChangeDetailsList)
                    {
                        if (item.ChildApprovalStatusMasterID == 1 || item.ChildApprovalStatusMasterID == 2 || item.ChildApprovalStatusMasterID == 3)
                        {
                            model.Mail = new EmployeeMailTemplate();
                            int empid = Convert.ToInt32(model.EmployeeID);
                            int childemployeeid = Convert.ToInt32(item.ChildEmployeeID);
                            HRMS_tbl_PM_Employee ChildemployeeDetails = employeeDAL.GetEmployeeDetails(childemployeeid);
                            HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(empid);
                            if (employeeDetails != null)
                            {
                                model.Mail.From = employeeDetails.EmailID;

                                model.Mail.To = ChildemployeeDetails.EmailID;

                                string ChildMasterstatus;
                                if (item.ChildApprovalStatusMasterID == 1)
                                {
                                    ChildMasterstatus = "On Hold";
                                }
                                else if (item.ChildApprovalStatusMasterID == 2)
                                {
                                    ChildMasterstatus = "Approved";
                                }
                                else
                                {
                                    ChildMasterstatus = "Rejected";
                                }

                                string approvalStatus = (builder.Append(item.ChildFieldDiscription).Append(" : ").Append(ChildMasterstatus).Append("<br>")
                                     + "Comments: " + model.Comments);

                                int templateId = 0;
                                if (model.Module == "New Personal Details" || model.Module == "OnHold Personal Details")
                                    templateId = 1;
                                if (model.Module == "New Residential Details" || model.Module == "OnHold Residential Details")
                                    templateId = 2;

                                string mailBody = null;
                                List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                                foreach (var emailTemplate in template)
                                {
                                    model.Mail.Subject = emailTemplate.Subject;
                                    mailBody = emailTemplate.Message;
                                }

                                mailBody = mailBody.Replace("##Approval Status##", approvalStatus);
                                mailBody = mailBody.Replace("##HR Admin##", Server.HtmlEncode(employeeDetails.EmployeeName));
                                model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                                ViewBag.Body = mailBody;

                                string[] roles = { "HR Admin" };

                                foreach (string r in roles)
                                {
                                    string[] users = Roles.GetUsersInRole(r);

                                    foreach (string user in users)
                                    {
                                        HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(user));
                                        if (employee == null)
                                            model.Mail.Cc = model.Mail.Cc + string.Empty;
                                        else
                                            model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                }
                return PartialView("_MailTemplateApprovalStatus", model.Mail);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult CertificateSendMail(int EmpID, EmployeeChangesApprovalViewModel model)
        {
            var EmployeeID = EmpID;
            var ChildEmployeeID = model.EmployeeID;
            try
            {
                model.Mail = new EmployeeMailTemplate();
                int empid = Convert.ToInt32(EmployeeID);
                int childemployeeid = Convert.ToInt32(ChildEmployeeID);
                HRMS_tbl_PM_Employee ChildemployeeDetails = employeeDAL.GetEmployeeDetails(childemployeeid);
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(empid);
                if (employeeDetails != null)
                {
                    model.Mail.From = employeeDetails.EmailID;

                    model.Mail.To = ChildemployeeDetails.EmailID;

                    string mailBody = null;
                    int templateId = 4;
                    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                    foreach (var emailTemplate in template)
                    {
                        model.Mail.Subject = emailTemplate.Subject;
                        mailBody = emailTemplate.Message;
                    }
                    mailBody = mailBody.Replace("##HR Admin##", Server.HtmlEncode(employeeDetails.EmployeeName));
                    model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                    ViewBag.Body = mailBody;
                    string[] roles = { "HR Admin" };
                    foreach (string r in roles)
                    {
                        string[] users = Roles.GetUsersInRole(r);

                        foreach (string user in users)
                        {
                            HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(user));
                            if (employee == null)
                                model.Mail.Cc = model.Mail.Cc + string.Empty;
                            else
                                model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                        }
                    }
                }

                return PartialView("_MailTemplateApprovalStatus", model.Mail);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult QualificationSendMail(int EmpID, EmployeeChangesApprovalViewModel model)
        {
            try
            {
                var EmployeeID = EmpID;
                var ChildEmployeeID = model.EmployeeID;

                model.Mail = new EmployeeMailTemplate();
                int empid = Convert.ToInt32(EmployeeID);
                int childemployeeid = Convert.ToInt32(ChildEmployeeID);
                HRMS_tbl_PM_Employee ChildemployeeDetails = employeeDAL.GetEmployeeDetails(childemployeeid);
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(empid);
                if (employeeDetails != null)
                {
                    model.Mail.From = employeeDetails.EmailID;
                    model.Mail.To = ChildemployeeDetails.EmailID;
                    string mailBody = null;
                    int templateId = 3;
                    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                    foreach (var emailTemplate in template)
                    {
                        model.Mail.Subject = emailTemplate.Subject;
                        mailBody = emailTemplate.Message;
                    }
                    mailBody = mailBody.Replace("##HR Admin##", Server.HtmlEncode(employeeDetails.EmployeeName));
                    model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                    ViewBag.Body = mailBody;
                    string[] roles = { "HR Admin" };

                    foreach (string r in roles)
                    {
                        string[] users = Roles.GetUsersInRole(r);

                        foreach (string user in users)
                        {
                            HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(user));
                            if (employee == null)
                                model.Mail.Cc = model.Mail.Cc + string.Empty;
                            else
                                model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                        }
                    }
                }

                return PartialView("_MailTemplateApprovalStatus", model.Mail);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SkillSendMail(int EmpID, EmployeeChangesApprovalViewModel model)
        {
            try
            {
                var EmployeeID = EmpID;
                var ChildEmployeeID = model.EmployeeID;

                model.Mail = new EmployeeMailTemplate();
                int empid = Convert.ToInt32(EmployeeID);
                int childemployeeid = Convert.ToInt32(ChildEmployeeID);
                HRMS_tbl_PM_Employee ChildemployeeDetails = employeeDAL.GetEmployeeDetails(childemployeeid);
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(empid);
                if (employeeDetails != null)
                {
                    model.Mail.From = employeeDetails.EmailID;
                    model.Mail.To = ChildemployeeDetails.EmailID;
                    string mailBody = null;
                    int templateId = 5;
                    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                    foreach (var emailTemplate in template)
                    {
                        model.Mail.Subject = emailTemplate.Subject;
                        mailBody = emailTemplate.Message;
                    }
                    mailBody = mailBody.Replace("##RMG##", Server.HtmlEncode(employeeDetails.EmployeeName));
                    model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                    ViewBag.Body = mailBody;

                    string[] roles = { "RMG" };
                    foreach (string r in roles)
                    {
                        string[] users = Roles.GetUsersInRole(r);

                        foreach (string user in users)
                        {
                            HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(user));
                            if (employee == null)
                                model.Mail.Cc = model.Mail.Cc + string.Empty;
                            else
                                model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                        }
                    }
                }

                return PartialView("_MailTemplateApprovalStatus", model.Mail);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult SendEmail(EmployeeMailTemplate model)
        {
            bool result = false;

            try
            {
                char[] symbols = new char[] { ';', ' ', ',', '\r', '\n' };
                int CcCounter = 0;
                int ToCounter = 0;

                if (model.Cc != "" && model.Cc != null)
                {
                    string CcMailIds = model.Cc.TrimEnd(symbols);
                    model.Cc = CcMailIds;
                    string[] EmailIds = CcMailIds.Split(symbols);

                    string[] CCEmailId = EmailIds.Where(s => !String.IsNullOrEmpty(s)).ToArray();

                    foreach (string id in CCEmailId)
                    {
                        HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetailsFromEmailId(id);

                        if (employeeDetails != null)
                            CcCounter = 1;
                        else
                        {
                            CcCounter = 0;
                            break;
                        }
                    }

                    string[] EmailToId = model.To.Split(symbols);
                    string[] EmailToIds = EmailToId.Where(s => !String.IsNullOrEmpty(s)).ToArray();
                    foreach (string email in EmailToIds)
                    {
                        HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetailsFromEmailId(email);
                        if (employeeDetails != null)

                            ToCounter = 1;
                        else
                        {
                            ToCounter = 0;
                            break;
                        }
                    }
                }
                else
                {
                    CcCounter = 1;
                    string[] EmailToId = model.To.Split(symbols);
                    string[] EmailToIds = EmailToId.Where(s => !String.IsNullOrEmpty(s)).ToArray();
                    foreach (string email in EmailToIds)
                    {
                        HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetailsFromEmailId(email);
                        if (employeeDetails != null)

                            ToCounter = 1;
                        else
                        {
                            ToCounter = 0;
                            break;
                        }
                    }
                }

                if (CcCounter == 1 && ToCounter == 1)
                {
                    result = SendMail(model);
                    if (result == true)
                        return Json(new { status = true, validCcId = true, validtoId = true });
                    else
                        return Json(new { status = false, validCcId = true, validtoId = true });
                }
                else
                {
                    if (CcCounter == 1 && ToCounter == 0)
                        return Json(new { status = false, validCcId = true, validtoId = false });
                    else
                    {
                        if (CcCounter == 0 && ToCounter == 1)
                            return Json(new { status = false, validCcId = false, validtoId = true });
                        else
                        {
                            return Json(new { status = false, validCcId = false, validtoId = false });
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool SendMail(EmployeeMailTemplate model)
        {
            try
            {
                SMTPHelper smtpHelper = new SMTPHelper();
                char[] symbols = new char[] { ';', ' ', ',', '\r', '\n' };
                if (model != null)
                {
                    string[] ToEmailId = model.To.Split(symbols);

                    //Loop to seperate email id's of CC peoples
                    string[] CCEmailId = null;
                    if (model.Cc != "" && model.Cc != null)
                    {
                        CCEmailId = model.Cc.Split(symbols);
                        string[] CCEmailIds = CCEmailId.Where(s => !String.IsNullOrEmpty(s)).ToArray();
                        return smtpHelper.SendMail(ToEmailId, null, CCEmailIds, null, null, null, model.From, null, model.Subject, model.Message, null, null);
                    }
                    else
                        return smtpHelper.SendMail(ToEmailId, null, null, null, null, null, model.From, null, model.Subject, model.Message, null, null);
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}