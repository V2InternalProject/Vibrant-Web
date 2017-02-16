using HRMS.DAL;
using HRMS.Helper;
using HRMS.Models;
using HRMS.Notification;
using MvcApplication3.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class ExitController : Controller
    {
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();
        private ExitProcessDAL objExitDal = new ExitProcessDAL();
        private EmployeeDAL employeeDAL = new EmployeeDAL();

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ExitProcessViewModel model = new ExitProcessViewModel();
                model.SearchedUserDetails = new SearchedUserDetails();

                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("LogOn", "Account");  //replace the null with a viewModel as needed.
                }

                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                //added here
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                model.SearchedUserDetails.UserRole = user;
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                model.EmployeeId = employeeID;
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult ExitProcess()
        {
            try
            {
                ExitProcessViewModel model = new ExitProcessViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.SearchedUserDetails = new SearchedUserDetails();

                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                //added here
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                model.UserRole = user;
                model.SearchedUserDetails.UserRole = user;
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                model.EmployeeId = employeeID;
                model.SearchedUserDetails.EmployeeId = employeeID;

                tbl_HR_ExitInstance exit = objExitDal.GetExitfromEmpIdForResignLinkHiding(employeeID);
                if (exit != null)
                    ViewBag.IsSeparationLinkVisible = false;
                else
                    ViewBag.IsSeparationLinkVisible = true;

                int page = 1;
                int rows = 5;
                string term = "";
                string Field = "0";
                string FieldChild = "0";
                int totalCount;
                List<EmpSeparationApprovals> searchResultInbox = new List<EmpSeparationApprovals>();
                searchResultInbox = objExitDal.GetInboxListDetails(term, Field, FieldChild, page, rows, employeeID, out totalCount);
                ViewBag.InboxCount = totalCount;
                return PartialView("_ExitProcess", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        [PageAccess(PageName = "Fill My Seperation")]
        public ActionResult SeparationForm(string Istermination, string employeeid)
        {
            try
            {
                Session["SearchEmpFullName"] = null;  // to hide emp search
                Session["SearchEmpCode"] = null;
                Session["SearchEmpID"] = null;

                ExitProcessViewModel model = new ExitProcessViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                ExitProcessDAL exitdal = new ExitProcessDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                int? decryptedemployeeId = 0;
                if (employeeid != null)
                {
                    bool isAuthorize;
                    string decryptedEmployeeID = HRMSHelper.Decrypt(employeeid, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                    decryptedemployeeId = Convert.ToInt32(decryptedEmployeeID);
                }
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);

                // model.TentativeReleaseDate = DateTime.Now.Date;//added by nikhil
                //added here

                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                model.UserRole = user;
                model.SearchedUserDetails.UserRole = user;
                //  model.ResignedDate = DateTime.Today;
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                int terminatedEmpID = 0;
                if (decryptedemployeeId != 0)
                {
                    terminatedEmpID = Convert.ToInt32(decryptedemployeeId);
                }
                if (decryptedemployeeId == 0)
                    decryptedemployeeId = employeeID;

                model.EmployeeId = employeeID;
                model.SearchedUserDetails.EmployeeId = employeeID;
                model.SeparationReasonList = objExitDal.GetSeparationReasonList();
                if (Istermination != null)
                    model.Isterminate = Istermination;
                if (decryptedemployeeId != null)
                {
                    HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetails(decryptedemployeeId.HasValue ? decryptedemployeeId.Value : 0);
                    model.EmpName = employee.EmployeeName;
                    ViewBag.EmpJoiningDate = employee.JoiningDate;
                    if (employee.ReportingTo != null && employee.ReportingTo != 0)
                    {
                        ViewBag.IsExitConfManagerSet = true;
                    }
                    else
                    {
                        ViewBag.IsExitConfManagerSet = false;
                    }
                }
                model.TerminatedEmpId = decryptedemployeeId.HasValue ? decryptedemployeeId.Value : 0;
                ViewBag.Separation = new SelectList(exitdal.GetModeOfSeperationList(), "SeperationId", "SeperationName");
                int probationNoticePeriod = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["ProbNoticePeriodDuration"]);
                int confirmedNoticePeriod = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["CnfNoticePeriodDuration"]);
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToBoolean(employeeID) ? employeeID : 0);
                HRMS_tbl_PM_Employee terminatedEmpDetails = new HRMS_tbl_PM_Employee();
                if (terminatedEmpID != 0)
                {
                    terminatedEmpDetails = employeeDAL.GetEmployeeDetails(terminatedEmpID);
                }
                if (employeeDetails != null)
                {
                    if (Istermination == "yes")
                    {
                        if (terminatedEmpDetails.EmployeeStatusID == 5)
                        {
                            model.NoticePeriod = probationNoticePeriod;
                            model.TentativeReleaseDate = DateTime.Now.AddDays((probationNoticePeriod - 1));
                        }
                        else
                        {
                            model.NoticePeriod = confirmedNoticePeriod;
                            model.TentativeReleaseDate = DateTime.Now.AddDays((confirmedNoticePeriod - 1));
                        }
                    }
                    else
                    {
                        DateTime? checkDay;
                        if (employeeDetails.EmployeeStatusID == 5)
                        {
                            model.NoticePeriod = probationNoticePeriod;

                            checkDay = DateTime.Now.AddDays((probationNoticePeriod));
                            if (checkDay.Value.DayOfWeek == DayOfWeek.Saturday)
                            {
                                checkDay = checkDay.Value.AddDays(-1);
                            }
                            if (checkDay.Value.DayOfWeek == DayOfWeek.Sunday)
                            {
                                checkDay = checkDay.Value.AddDays(-2);
                            }
                            model.SystemReleavingDate = checkDay;
                            model.TentativeReleaseDate = checkDay;
                        }
                        else
                        {
                            model.NoticePeriod = confirmedNoticePeriod;

                            checkDay = DateTime.Now.AddDays((confirmedNoticePeriod));
                            if (checkDay.Value.DayOfWeek == DayOfWeek.Saturday)
                            {
                                checkDay = checkDay.Value.AddDays(-1);
                            }
                            if (checkDay.Value.DayOfWeek == DayOfWeek.Sunday)
                            {
                                checkDay = checkDay.Value.AddDays(-2);
                            }
                            model.SystemReleavingDate = checkDay;
                            model.TentativeReleaseDate = checkDay;
                        }
                    }
                }
                model.ResignedDate = DateTime.Now.Date;
                model.Mail = new SeparationMailTemplate();
                model.Mail.Message = model.Mail.Message ?? "";

                tbl_HR_ExitInstance exit = objExitDal.GetExitfromEmpIdForResignLinkHiding(employeeID);
                if (exit != null)
                    ViewBag.IsSeparationLinkVisible = false;
                else
                    ViewBag.IsSeparationLinkVisible = true;

                return PartialView("_SeparationForm", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SaveSeparationForm(ExitProcessViewModel model)
        {
            try
            {
                bool result = false;
                int exitInstanceId = 0;

                result = objExitDal.SaveSeparationFormDetails(model, out exitInstanceId);
                string ExitId = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(exitInstanceId), true);
                if (result == true)
                    return Json(new { status = result, exitId = ExitId }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { status = result, exitId = ExitId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [PageAccess(PageName = "Termination")]
        public ActionResult EmpSeparationTermination()
        {
            try
            {
                EmpSeperationTerminationViewModel extmodel = new EmpSeperationTerminationViewModel();
                ConfirmationDAL dal = new ConfirmationDAL();

                EmployeeDAL employeeDAL = new EmployeeDAL();
                extmodel.SearchedUserDetails = new SearchedUserDetails();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                extmodel.SearchedUserDetails.UserRole = user;
                extmodel.SearchedUserDetails.EmployeeId = employeeID;
                string userRole = user;
                extmodel.MailDetail = new MailTemplateViewModel();
                extmodel.MailDetail.Message = "";

                ViewBag.Location = new SelectList(employeeDAL.GetLocationList(), "LocationId", "LocationName");
                return PartialView("_InitiateSeperationTermination", extmodel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult InitiateTerminationLoadGrid(string term, string OrganizationUnit, int page, int rows)
        {
            try
            {
                EmpSeperationTerminationViewModel model = new EmpSeperationTerminationViewModel();
                ConfirmationProcess confrmDal = new ConfirmationProcess();
                ExitProcessDAL dal = new ExitProcessDAL();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                confrmDal.ConfigurationDetailsList = new List<ConfigurationDetailsViewModel>();

                int totalCount;
                List<EmpSeperationTerminationViewModel> searchResult = new List<EmpSeperationTerminationViewModel>();
                searchResult = dal.SearchEmployeeForTerminationLoadGrid(term, OrganizationUnit, page, rows, out totalCount);

                if ((searchResult == null || searchResult.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    searchResult = dal.SearchEmployeeForTerminationLoadGrid(term, OrganizationUnit, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = searchResult,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult MailTemplate(string exitInstanceId, bool isApproveCall, bool IsRejectCall, string Isterminated)
        {
            try
            {
                ExitProcessViewModel model = new ExitProcessViewModel();
                model.Mail = new SeparationMailTemplate();
                HRMS_tbl_PM_Employee CompetencyMgr = new HRMS_tbl_PM_Employee();
                int EmployeeID = 0;
                int terminatorID = 0;
                string mailBody = null;
                string subject = null;
                int templateId = 0;
                List<EmployeeMailTemplate> template = new List<EmployeeMailTemplate>();
                bool isAuthorize;
                string decryptedExitInstanceId = HRMSHelper.Decrypt(exitInstanceId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                int userApproverStageId = objExitDal.GetApproverStageIdFromEmpId(employeeDAL.GetEmployeeID(Membership.GetUser().UserName));
                bool result = objExitDal.CheckLoginUserIsEmpOrManager(employeeDAL.GetEmployeeID(Membership.GetUser().UserName));
                EmployeeDetailsViewModel empExit = objExitDal.GetEmpSeparationShowDetails(Convert.ToInt32(decryptedExitInstanceId));
                tbl_HR_ExitInstance empexitTermination = objExitDal.GetEmpExitTermination(Convert.ToInt32(decryptedExitInstanceId));//terminated employee information from exit instance table
                Tbl_HR_ExitStageEvent emptermination = objExitDal.GetEmpTerminationDetails(Convert.ToInt32(decryptedExitInstanceId));//terminated employee information from exit stage event table
                ExitProcessViewModel obj = objExitDal.GetSeparationDetails(Convert.ToInt32(decryptedExitInstanceId));

                if (empExit != null)
                {
                    EmployeeID = empExit.EmployeeId.Value;
                    terminatorID = emptermination.StageActorEmployeeId.Value;
                    //HR admin who has done termination
                    if (empExit.CompetencyManagerId_Emp != 0)
                        CompetencyMgr = employeeDAL.GetEmployeeDetails(empExit.CompetencyManagerId_Emp);
                }
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(EmployeeID);
                HRMS_tbl_PM_Employee terminationDetails = employeeDAL.GetEmployeeDetails(terminatorID);//info of HR admin who has done termination
                int competencymanger = Convert.ToInt32(employeeDetails.CompetencyManager);
                int exitManager = Convert.ToInt32(employeeDetails.ReportingTo);
                int reportingManager = Convert.ToInt32(employeeDetails.CostCenterID);
                HRMS_tbl_PM_Employee CompetencyManager = new HRMS_tbl_PM_Employee();
                HRMS_tbl_PM_Employee ExitManager = new HRMS_tbl_PM_Employee();
                HRMS_tbl_PM_Employee ReportingManager = new HRMS_tbl_PM_Employee();
                if (competencymanger != 0 || competencymanger != null)
                {
                    CompetencyManager = employeeDAL.GetEmployeeDetails(competencymanger);//terminated employee's competency manager
                }
                if (exitManager != 0 || exitManager != null)
                {
                    ExitManager = employeeDAL.GetEmployeeDetails(exitManager);//terminated employee's exit manager
                }
                if (reportingManager != 0 || reportingManager != null)
                {
                    ReportingManager = employeeDAL.GetEmployeeDetails(reportingManager);
                }
                string[] StageApproverEmail = objExitDal.GetToFieldEmailsRMG_HRAdmin(Convert.ToInt32(decryptedExitInstanceId));

                if (employeeDetails != null)
                {
                    if (obj != null)
                    {
                        //Termination Process
                        if (Isterminated == "yes")
                        {
                            model.Mail.From = terminationDetails.EmailID;
                            model.Mail.To = employeeDetails.EmailID;
                            templateId = 33;
                            template = Commondal.GetEmailTemplate(templateId);
                            foreach (var emailTemplate in template)
                            {
                                subject = emailTemplate.Subject;
                                mailBody = emailTemplate.Message;
                            }
                            subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                            model.Mail.Subject = subject;
                            mailBody = mailBody.Replace("##employee name##", employeeDetails.EmployeeName);
                            mailBody = mailBody.Replace("##date selected by HR##", empexitTermination.TentativeReleavingDate.Value.Date.ToString("d"));
                            mailBody = mailBody.Replace("##comments enter by HR##", empexitTermination.HRComment);
                            mailBody = mailBody.Replace("##HR Admin##", Server.HtmlEncode(terminationDetails.EmployeeName));
                            model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                            //Removed RMG Roles to skip their names in seperation mail:Changes by Rahul R
                            //Adding RMG roles so they can recieve the seperation Mails:Rahul R
                            string[] Loginroles = { "HR Admin", "RMG" };
                            foreach (string r in Loginroles)
                            {
                                string[] users = Roles.GetUsersInRole(r);
                                foreach (string userR in users)
                                {
                                    HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
                                    if (employee == null)
                                        model.Mail.Cc = model.Mail.Cc + string.Empty;
                                    else
                                        model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                                }
                            }
                            char[] symbols = new char[] { ';' };
                            string CC = model.Mail.Cc.TrimEnd(symbols);
                            if (CompetencyManager != null)
                            {
                                model.Mail.Cc = CC + ";" + CompetencyManager.EmailID + ";" + ExitManager.EmailID;
                            }
                            else
                            {
                                if (CompetencyManager == null)
                                {
                                    model.Mail.Cc = CC + ";" + ExitManager.EmailID;
                                }
                                else
                                {
                                    model.Mail.Cc = CC + ";" + CompetencyManager.EmailID;
                                }
                            }
                        }
                        else
                        {
                            if (empexitTermination.stageID == 7) //added by nikhil for hr closure
                            {
                                HRMS_tbl_PM_Employee employeeD = employeeDAL.GetEmployeeDetails(employeeDAL.GetEmployeeID(Membership.GetUser().UserName));
                                model.Mail.From = employeeD.EmailID;
                                model.Mail.To = employeeDetails.EmailID;
                                templateId = 32;
                                template = Commondal.GetEmailTemplate(templateId);
                                foreach (var emailTemplate in template)
                                {
                                    model.Mail.Subject = emailTemplate.Subject;
                                    mailBody = emailTemplate.Message;
                                }
                                mailBody = mailBody.Replace("##Employee name##", employeeDetails.EmployeeName);
                                mailBody = mailBody.Replace("##HR Closure stakeholder##", Server.HtmlEncode(employeeD.EmployeeName));
                                model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                                //Removed RMG Roles to skip their names in seperation mail:Changes by Rahul R
                                //Adding RMG roles so they can recieve the seperation Mails:Rahul R
                                string[] Loginroles = { "HR Admin", "RMG" };
                                foreach (string r in Loginroles)
                                {
                                    string[] users = Roles.GetUsersInRole(r);
                                    foreach (string userR in users)
                                    {
                                        HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
                                        if (employee == null)
                                            model.Mail.Cc = model.Mail.Cc + string.Empty;
                                        else
                                            model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                                    }
                                }
                            }
                            //end of hr closure
                            else
                            {
                                //employee withdraw resignation
                                model.Mail.From = employeeDetails.EmailID;
                                HRMS_tbl_PM_Employee manager = employeeDAL.GetEmployeeDetails(employeeDetails.ReportingTo.Value);

                                if (manager != null)
                                    model.Mail.To = manager.EmailID;

                                if (obj.IsWithdraw == true)
                                {
                                    templateId = 34;
                                    template = Commondal.GetEmailTemplate(templateId);
                                    foreach (var emailTemplate in template)
                                    {
                                        subject = emailTemplate.Subject;
                                        mailBody = emailTemplate.Message;
                                    }
                                    subject = subject.Replace("##EmployeeName##", employeeDetails.EmployeeName);
                                    model.Mail.Subject = subject;
                                    mailBody = mailBody.Replace("##employee name##", employeeDetails.EmployeeName);
                                    mailBody = mailBody.Replace("##employee code##", employeeDetails.EmployeeCode);
                                    model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                                    ViewBag.Body = mailBody;
                                    model.Mail.Cc = model.Mail.Cc + ReportingManager.EmailID + ";";
                                    model.Mail.Cc = model.Mail.Cc + CompetencyManager.EmailID + ";";
                                    //Removed RMG Roles to skip their names in seperation mail:Changes by Rahul R 
                                    //Adding RMG roles so they can recieve the seperation Mails:Rahul R
                                    string[] Loginroles = { "HR Admin", "RMG" };
                                    foreach (string r in Loginroles)
                                    {
                                        string[] users = Roles.GetUsersInRole(r);
                                        foreach (string userR in users)
                                        {
                                            HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
                                            if (employee == null)
                                                model.Mail.Cc = model.Mail.Cc + string.Empty;
                                            else
                                                model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                                        }
                                    }
                                }
                                else
                                {
                                    if (isApproveCall == false && IsRejectCall == false)//Employee initiated seperation process
                                    {
                                        ViewBag.employeeSeparationForm = true;
                                        templateId = 22;
                                        template = Commondal.GetEmailTemplate(templateId);
                                        foreach (var emailTemplate in template)
                                        {
                                            subject = emailTemplate.Subject;
                                            mailBody = emailTemplate.Message;
                                        }
                                        subject = subject.Replace("##employeename##", employeeDetails.EmployeeName);
                                        model.Mail.Subject = subject;
                                        mailBody = mailBody.Replace("##resigned Date##", obj.ResignedDate.Value.Date.ToString("d"));
                                        mailBody = mailBody.Replace("##NP value##", obj.NoticePeriod.ToString());
                                        mailBody = mailBody.Replace("##reason selected##", obj.ReasonForSeparartion);
                                        mailBody = mailBody.Replace("##tentative release date##", obj.TentativeReleaseDate.Value.Date.ToString("d"));
                                        mailBody = mailBody.Replace("##System Generated Relieving Date##", obj.SystemReleavingDate.Value.Date.ToString("d"));
                                        mailBody = mailBody.Replace("##Comments entered by Employee##", obj.EmpComment);
                                        mailBody = mailBody.Replace("##employee name##", Server.HtmlEncode(employeeDetails.EmployeeName));
                                        model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                                        ViewBag.Body = mailBody;
                                        if (ReportingManager != null)
                                            model.Mail.Cc = model.Mail.Cc + ReportingManager.EmailID + ";";
                                        if (CompetencyManager != null)
                                            model.Mail.Cc = model.Mail.Cc + CompetencyManager.EmailID + ";";
                                        //Removed RMG Roles to skip their names in seperation mail:Changes by Rahul R
                                        //Adding RMG roles so they can recieve the seperation Mails:Rahul R
                                        string[] Loginroles = { "HR Admin", "RMG" };
                                        foreach (string r in Loginroles)
                                        {
                                            string[] users = Roles.GetUsersInRole(r);
                                            foreach (string userR in users)
                                            {
                                                HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
                                                if (employee == null)
                                                    model.Mail.Cc = model.Mail.Cc + string.Empty;
                                                else
                                                    model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (isApproveCall == true)
                                        {
                                            //Removed RMG Roles to skip their names in seperation mail:Changes by Rahul R

                                            //if (userApproverStageId == 8 || user == "RMG")//RMG approver stage
                                            //{
                                            //    HRMS_tbl_PM_Employee employeeD = employeeDAL.GetEmployeeDetails(employeeDAL.GetEmployeeID(Membership.GetUser().UserName));
                                            //    model.Mail.From = employeeD.EmailID;
                                            //    model.Mail.To = employeeDetails.EmailID;
                                            //    templateId = 25;
                                            //    template = Commondal.GetEmailTemplate(templateId);
                                            //    foreach (var emailTemplate in template)
                                            //    {
                                            //        subject = emailTemplate.Subject;
                                            //        mailBody = emailTemplate.Message;
                                            //    }
                                            //    subject = subject.Replace("##RMG manager name##", employeeD.EmployeeName);
                                            //    subject = subject.Replace("##employee name##", Server.HtmlEncode(employeeDetails.EmployeeName));
                                            //    model.Mail.Subject = subject;
                                            //    mailBody = mailBody.Replace("##employee name##", Server.HtmlEncode(employeeDetails.EmployeeName));
                                            //    //mailBody = mailBody.Replace("##Tentative Release Date##", obj.TentativeReleaseDate.Value.Date.ToString("d"));
                                            //    mailBody = mailBody.Replace("##Agreed release date##", obj.AgreedReleaseDate.Value.Date.ToString("d"));
                                            //    mailBody = mailBody.Replace("##RMG manager name##", Server.HtmlEncode(employeeD.EmployeeName));
                                            //    model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                                            //    ViewBag.Body = mailBody;
                                            //    model.Mail.Cc = model.Mail.Cc + manager.EmailID + ";";
                                            //    model.Mail.Cc = model.Mail.Cc + CompetencyManager.EmailID + ";";
                                            //    model.Mail.Cc = model.Mail.Cc + ReportingManager.EmailID + ";";
                                            //    model.Mail.Cc = model.Mail.Cc + employeeDetails.EmailID + ";";
                                            //    string[] Loginroles = { "HR Admin", "RMG" };
                                            //    foreach (string r in Loginroles)
                                            //    {
                                            //        string[] users = Roles.GetUsersInRole(r);
                                            //        foreach (string userR in users)
                                            //        {
                                            //            HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
                                            //            if (employee == null)
                                            //                model.Mail.Cc = model.Mail.Cc + string.Empty;
                                            //            else
                                            //                model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                                            //        }
                                            //    }
                                            //}
                                            //else                                            
                                            {
                                                if (userApproverStageId == 3 || user == "HR Admin")
                                                {
                                                    if (obj.StageId == 6 || obj.StageId == 5)
                                                    {
                                                        HRMS_tbl_PM_Employee employeeD = employeeDAL.GetEmployeeDetails(employeeDAL.GetEmployeeID(Membership.GetUser().UserName));
                                                        model.Mail.From = employeeD.EmailID;

                                                        string[] LoginrolesHRAdmin = { "HR Admin" };
                                                        foreach (string r in LoginrolesHRAdmin)
                                                        {
                                                            string[] users = Roles.GetUsersInRole(r);
                                                            foreach (string userR in users)
                                                            {
                                                                HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
                                                                if (employee == null)
                                                                    model.Mail.To = model.Mail.To + string.Empty;
                                                                else
                                                                    model.Mail.To = model.Mail.To + employee.EmailID + ";";
                                                            }
                                                        }
                                                        templateId = 31;
                                                        template = Commondal.GetEmailTemplate(templateId);
                                                        foreach (var emailTemplate in template)
                                                        {
                                                            model.Mail.Subject = emailTemplate.Subject;
                                                            mailBody = emailTemplate.Message;
                                                        }
                                                        mailBody = mailBody.Replace("##employeename##", employeeDetails.EmployeeName);
                                                        mailBody = mailBody.Replace("##Exit Interview stakeholder##", Server.HtmlEncode(employeeD.EmployeeName));
                                                        model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                                                        ViewBag.Body = mailBody;
                                                        model.Mail.Cc = model.Mail.Cc + employeeDetails.EmailID + ";";
                                                        //Removed RMG Roles to skip their names in seperation mail:Changes by Rahul R

                                                        //UnCommented RMG roles cc section so RMG can recieve the seperation Mails:Rahul R                                                      
                                                        string[] Loginroles = { "RMG" };
                                                        foreach (string r in Loginroles)
                                                        {
                                                            string[] users = Roles.GetUsersInRole(r);
                                                            foreach (string userR in users)
                                                            {
                                                                HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
                                                                if (employee == null)
                                                                    model.Mail.Cc = model.Mail.Cc + string.Empty;
                                                                else
                                                                    model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        HRMS_tbl_PM_Employee employeeD = employeeDAL.GetEmployeeDetails(employeeDAL.GetEmployeeID(Membership.GetUser().UserName));
                                                        model.Mail.From = employeeD.EmailID;
                                                        model.Mail.To = StageApproverEmail[0];
                                                        templateId = 27;
                                                        template = Commondal.GetEmailTemplate(templateId);
                                                        foreach (var emailTemplate in template)
                                                        {
                                                            subject = emailTemplate.Subject;
                                                            mailBody = emailTemplate.Message;
                                                        }
                                                        subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                                                        model.Mail.Subject = subject;
                                                        //mailBody = mailBody.Replace("##Submitted date##", empexitTermination.ResignedDate.Value.Date.ToString("d"));   by modified email template requirement
                                                        mailBody = mailBody.Replace("##employee name##", employeeDetails.EmployeeName);
                                                        mailBody = mailBody.Replace("##agreed release date##", obj.AgreedReleaseDate.Value.Date.ToString("d"));
                                                        mailBody = mailBody.Replace("##HRAdmin##", Server.HtmlEncode(employeeD.EmployeeName));
                                                        model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                                                        ViewBag.Body = mailBody;
                                                        model.Mail.Cc = model.Mail.Cc + manager.EmailID + ";" + CompetencyManager.EmailID + ";"
                                                                        + ReportingManager.EmailID + ";" + employeeDetails.EmailID + ";";
                                                        //Removed RMG Roles to skip their names in seperation mail:Changes by Rahul R

                                                        //UnCommented RMG roles cc section so RMG can recieve the seperation Mails:Rahul R  

                                                        string[] Loginroles = { "RMG" };
                                                        foreach (string r in Loginroles)
                                                        {
                                                            string[] users = Roles.GetUsersInRole(r);
                                                            foreach (string userR in users)
                                                            {
                                                                HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
                                                                if (employee == null)
                                                                    model.Mail.Cc = model.Mail.Cc + string.Empty;
                                                                else
                                                                    model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (result == true)//Line manager clearance
                                                    {
                                                        HRMS_tbl_PM_Employee employeeD = employeeDAL.GetEmployeeDetails(employeeDAL.GetEmployeeID(Membership.GetUser().UserName));
                                                        model.Mail.From = employeeD.EmailID;
                                                        model.Mail.To = StageApproverEmail[0];
                                                        templateId = 23;
                                                        template = Commondal.GetEmailTemplate(templateId);
                                                        foreach (var emailTemplate in template)
                                                        {
                                                            subject = emailTemplate.Subject;
                                                            mailBody = emailTemplate.Message;
                                                        }
                                                        subject = subject.Replace("##reporting manager name##", Server.HtmlEncode(employeeD.EmployeeName));
                                                        subject = subject.Replace("##employee name##", Server.HtmlEncode(employeeDetails.EmployeeName));
                                                        model.Mail.Subject = subject;
                                                        mailBody = mailBody.Replace("##employee name##", Server.HtmlEncode(employeeDetails.EmployeeName));
                                                        // mailBody = mailBody.Replace("##Tentative Release Date##", obj.TentativeReleaseDate.Value.Date.ToString("d"));
                                                        mailBody = mailBody.Replace("##Agreed Release Date##", obj.AgreedReleaseDate.Value.Date.ToString("d"));
                                                        mailBody = mailBody.Replace("##reporting manager name##", Server.HtmlEncode(employeeD.EmployeeName));
                                                        model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                                                        ViewBag.Body = mailBody;
                                                        model.Mail.Cc = model.Mail.Cc + manager.EmailID +
                                                        ";" + CompetencyManager.EmailID + ";" + employeeDetails.EmailID + ";";

                                                        string[] Loginroles = { "HR Admin" };
                                                        foreach (string r in Loginroles)
                                                        {
                                                            string[] users = Roles.GetUsersInRole(r);
                                                            foreach (string userR in users)
                                                            {
                                                                HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
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
                                            if (IsRejectCall == true)
                                            {
                                                //Removed RMG Roles to skip their names in seperation mail:Changes by Rahul R

                                                //if (userApproverStageId == 8 || user == "RMG")//RMG reject
                                                //{
                                                //    HRMS_tbl_PM_Employee employeeD = employeeDAL.GetEmployeeDetails(employeeDAL.GetEmployeeID(Membership.GetUser().UserName));
                                                //    model.Mail.From = employeeD.EmailID;
                                                //    model.Mail.To = manager.EmailID;
                                                //    templateId = 26;
                                                //    template = Commondal.GetEmailTemplate(templateId);
                                                //    foreach (var emailTemplate in template)
                                                //    {
                                                //        subject = emailTemplate.Subject;
                                                //        mailBody = emailTemplate.Message;
                                                //    }
                                                //    subject = subject.Replace("##RMG manager name##", employeeD.EmployeeName);
                                                //    subject = subject.Replace("##employee name##", Server.HtmlEncode(employeeDetails.EmployeeName));
                                                //    model.Mail.Subject = subject;
                                                //    mailBody = mailBody.Replace("##employee name##", Server.HtmlEncode(employeeDetails.EmployeeName));
                                                //    mailBody = mailBody.Replace("##RMG manager name##", Server.HtmlEncode(employeeD.EmployeeName));
                                                //    model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                                                //    ViewBag.Body = mailBody;
                                                //    model.Mail.Cc = model.Mail.Cc + CompetencyManager.EmailID + ";";
                                                //    model.Mail.Cc = model.Mail.Cc + ReportingManager.EmailID + ";";
                                                //    model.Mail.Cc = model.Mail.Cc + employeeDetails.EmailID + ";";

                                                //    string[] Loginroles = { "HR Admin", "RMG" };
                                                //    foreach (string r in Loginroles)
                                                //    {
                                                //        string[] users = Roles.GetUsersInRole(r);
                                                //        foreach (string userR in users)
                                                //        {
                                                //            HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
                                                //            if (employee == null)
                                                //                model.Mail.Cc = model.Mail.Cc + string.Empty;
                                                //            else
                                                //                model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                                                //        }
                                                //    }
                                                //}
                                                //else
                                                {
                                                    if (userApproverStageId == 3 || user == "HR Admin")
                                                    {
                                                        HRMS_tbl_PM_Employee employeeD = employeeDAL.GetEmployeeDetails(employeeDAL.GetEmployeeID(Membership.GetUser().UserName));
                                                        model.Mail.From = employeeD.EmailID;
                                                        model.Mail.To = StageApproverEmail[0];
                                                        templateId = 28;
                                                        template = Commondal.GetEmailTemplate(templateId);
                                                        foreach (var emailTemplate in template)
                                                        {
                                                            subject = emailTemplate.Subject;
                                                            mailBody = emailTemplate.Message;
                                                        }
                                                        subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                                                        model.Mail.Subject = subject;
                                                        mailBody = mailBody.Replace("##employee name##", Server.HtmlEncode(employeeDetails.EmployeeName));
                                                        mailBody = mailBody.Replace("##HR admin comment##", obj.HRComment);
                                                        mailBody = mailBody.Replace("##HR Admin name##", Server.HtmlEncode(employeeD.EmployeeName));
                                                        model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                                                        ViewBag.Body = mailBody;
                                                        model.Mail.Cc = model.Mail.Cc + manager.EmailID + ";" + CompetencyManager.EmailID + ";"
                                                                         + ReportingManager.EmailID + ";" + employeeDetails.EmailID + ";";
                                                    }
                                                    else
                                                    {
                                                        if (result == true)//Line manager reject
                                                        {
                                                            HRMS_tbl_PM_Employee employeeD = employeeDAL.GetEmployeeDetails(employeeDAL.GetEmployeeID(Membership.GetUser().UserName));
                                                            model.Mail.From = employeeD.EmailID;

                                                            model.Mail.To = employeeDetails.EmailID;
                                                            templateId = 24;
                                                            template = Commondal.GetEmailTemplate(templateId);
                                                            foreach (var emailTemplate in template)
                                                            {
                                                                subject = emailTemplate.Subject;
                                                                mailBody = emailTemplate.Message;
                                                            }
                                                            subject = subject.Replace("##reporting manager name##", Server.HtmlEncode(employeeD.EmployeeName));
                                                            subject = subject.Replace("##employee name##", Server.HtmlEncode(employeeDetails.EmployeeName));
                                                            model.Mail.Subject = subject;
                                                            mailBody = mailBody.Replace("##employee name##", Server.HtmlEncode(employeeDetails.EmployeeName));
                                                            mailBody = mailBody.Replace("##reporting manager name##", Server.HtmlEncode(employeeD.EmployeeName));
                                                            model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                                                            ViewBag.Body = mailBody;
                                                            model.Mail.Cc = model.Mail.Cc + CompetencyManager.EmailID + ";";
                                                            //Removed RMG Roles to skip their names in seperation mail:Changes by Rahul R
                                                            //Adding RMG roles so they can recieve the seperation Mails:Rahul R
                                                            string[] Loginroles = { "HR Admin", "RMG" };
                                                            foreach (string r in Loginroles)
                                                            {
                                                                string[] users = Roles.GetUsersInRole(r);
                                                                foreach (string userR in users)
                                                                {
                                                                    HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
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
                                        }
                                    }
                                }
                            }
                            char[] symbols = new char[] { ';' };

                            string CC = model.Mail.Cc.TrimEnd(symbols);
                            model.Mail.Cc = CC + ";" + model.Mail.From;
                        }
                    }
                }
                return PartialView("_MailTemplateSeparation", model.Mail);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SendEmail(SeparationMailTemplate model)
        {
            try
            {
                bool result = false;
                char[] symbols = new char[] { ';', ' ', ',', '\r', '\n' };
                int CcCounter = 0;
                int ToCounter = 0;
                if (model.Cc != "" && model.Cc != null)
                {
                    string CcMailIds = model.Cc.TrimEnd(symbols);
                    model.Cc = CcMailIds;
                    string[] EmailId = CcMailIds.Split(symbols);
                    string[] EmailIds = EmailId.Where(s => !String.IsNullOrEmpty(s)).ToArray();
                    foreach (string id in EmailIds)
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

        [HttpGet]
        [PageAccess(PageName = "Employee Seperation Approvals")]
        public ActionResult EmpSeparationApprovals()
        {
            try
            {
                EmpSeparationApprovals model = new EmpSeparationApprovals();
                ExitProcessDAL dal = new ExitProcessDAL();
                model.SearchedUserDetails = new SearchedUserDetails();

                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                model.UserRole = user;
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                EmployeeDAL empdal = new EmployeeDAL();
                //HRMS_tbl_PM_Employee emp = empdal.GetEmployeeDetailsByEmployeeCode(loginName);
                //int loginEmployeeID = emp.EmployeeID;
                int loginEmployeeID = empdal.GetEmployeeID(loginName);
                model.LoggedInUser = Convert.ToString(loginEmployeeID);
                ViewBag.UserRole = user;
                model.SearchedUserDetails.UserRole = user;
                model.SearchedUserDetails.EmployeeId = loginEmployeeID;
                model.FieldChildList = new List<FieldChildDetails>();
                ViewBag.FieldChildListBG = new SelectList(GetFieldChildDetailsList("Business Group"), "Id", "Description");
                ViewBag.FieldChildListOU = new SelectList(GetFieldChildDetailsList("Organization Unit"), "Id", "Description");
                ViewBag.FieldChildListSN = new SelectList(GetFieldChildDetailsList("Stage Name"), "Id", "Description");
                tbl_Q_Questionnaire department = dal.GetLoginUserDepartment(loginEmployeeID);
                if (department != null)
                {
                    model.loginUsersDepartment = department.QuestionnaireName;
                }
                else
                {
                    model.loginUsersDepartment = "";
                }
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                bool result = objExitDal.CheckLoginUserIsEmpOrManager(employeeID);
                List<tbl_HR_ExitInstance> exitDetails = objExitDal.GetExitDetailsfromEmpId(employeeID);

                if (exitDetails != null)
                    ViewBag.IsResignationExists = true;
                else
                    ViewBag.IsResignationExists = false;

                if (result == true)
                    ViewBag.IsManagerOrEMployee = "Manager";
                else
                    ViewBag.IsManagerOrEMployee = "Employee";

                model.EmployeeId = employeeID;
                ViewBag.EncryptedEmployeeId = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(employeeID), true);

                model.ShowDetails = new SeparationShowDetails();
                model.ShowStatus = new SeparationShowStatus();
                model.FinanceClearance = new FinanceClearance();
                model.ExitInterviewForm = new ExitInterviewViewModel();
                model.ShowDetails.EmployeeDetails = new EmployeeDetailsViewModel();
                model.ShowDetails.SearchedUserDetails = new SearchedUserDetails();
                model.ShowDetails.SeparationFormDetails = new ExitProcessViewModel();
                model.ExitInterviewForm.ListExitInterviewItems = new List<ExitInterview>();
                model.ExitInterviewForm.SearchedUserDetails = new SearchedUserDetails();
                model.ExitInterviewForm.SeparationFormDetails = new ExitProcessViewModel();
                model.ExitInterviewForm.EmployeeDetails = new EmployeeDetailsViewModel();

                model.ShowDetails.SeparationFormDetails.SeparationReasonList = objExitDal.GetSeparationReasonList();
                model.ShowDetails.WaiveOffList = objExitDal.GetWaiveOffList();

                model.Mail = new SeparationMailTemplate();
                model.Mail.Message = model.Mail.Message ?? "";
                return PartialView("_EmpSeparationApprovalsScreen", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public List<FieldChildDetails> GetFieldChildDetailsList(string Field)
        {
            try
            {
                List<FieldChildDetails> childs = objExitDal.GetFieldChildDetailsList(Field);
                childs.ToList();
                return childs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult LoadWatchListGrid(string term, string Field, string FieldChild, int page, int rows, string employeeId)
        {
            List<EmpSeparationApprovals> FinalWatchListDetails = new List<EmpSeparationApprovals>();
            try
            {
                int totalCount;
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                List<EmpSeparationApprovals> WatchListDetails = objExitDal.GetWatchListDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                if ((WatchListDetails == null || WatchListDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    WatchListDetails = objExitDal.GetWatchListDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                }
                foreach (var item in WatchListDetails)
                {
                    string ExitId = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.ExitInstanceId), true);
                    item.EncryptedExitInstanceId = ExitId;
                    FinalWatchListDetails.Add(item);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = FinalWatchListDetails
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult LoadInboxListGrid(string term, string Field, string FieldChild, int page, int rows, string employeeId)
        {
            List<EmpSeparationApprovals> FinalInboxListDetails = new List<EmpSeparationApprovals>();
            try
            {
                int totalCount;
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                List<EmpSeparationApprovals> InboxListDetails = objExitDal.GetInboxListDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);

                if ((InboxListDetails == null || InboxListDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    InboxListDetails = objExitDal.GetInboxListDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                }

                foreach (var item in InboxListDetails)
                {
                    string ExitId = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.ExitInstanceId), true);
                    item.EncryptedExitInstanceId = ExitId;
                    FinalInboxListDetails.Add(item);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = FinalInboxListDetails
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SeparationStatusDetailsLoadGrid(int page, int rows, string exitInstanceId)
        {
            try
            {
                int totalCount;
                bool isAuthorize;
                string decryptedExitInstanceId = HRMSHelper.Decrypt(exitInstanceId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                List<SeparationShowStatus> InboxListDetails = objExitDal.GetSeparationStatusDetails(page, rows, Convert.ToInt32(decryptedExitInstanceId), out totalCount);

                if ((InboxListDetails == null || InboxListDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    InboxListDetails = objExitDal.GetSeparationStatusDetails(page, rows, Convert.ToInt32(decryptedExitInstanceId), out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = InboxListDetails
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult GetSeparationShowDetails(string exitInstanceId)
        {
            try
            {
                SeparationShowDetails model = new SeparationShowDetails();
                model.EmployeeDetails = new EmployeeDetailsViewModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SeparationFormDetails = new ExitProcessViewModel();
                model.EmpdesignationDetails = new DesignationDetails();

                bool isAuthorize;
                string decryptedExitInstanceId = HRMSHelper.Decrypt(exitInstanceId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                model.SearchedUserDetails.UserRole = user;

                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);

                ViewBag.DoActionAsRMGOrHRAdmin = objExitDal.GetApproverStageIdFromEmpId(employeeID);
                ViewBag.LoggedInEmpId = employeeID;
                bool result = objExitDal.CheckLoginUserIsEmpOrManager(employeeID);
                ExitProcessViewModel exitDetails = objExitDal.GetEmpSeparationDetails(Convert.ToInt32(decryptedExitInstanceId));
                model.SeparationFormDetails.StageId = exitDetails.StageId;
                if (exitDetails != null)
                    ViewBag.IsResignationExists = true;
                else
                    ViewBag.IsResignationExists = false;

                if (result == true)
                    ViewBag.IsManagerOrEMployee = "Manager";
                else
                    ViewBag.IsManagerOrEMployee = "Employee";

                EmployeeDetailsViewModel employee = objExitDal.GetEmpSeparationShowDetails(Convert.ToInt32(decryptedExitInstanceId));
                ExitProcessViewModel exit = objExitDal.GetEmpSeparationDetails(Convert.ToInt32(decryptedExitInstanceId));
                DesignationDetails designation = objExitDal.GetEmpSeparationDesignationDetails(Convert.ToInt32(decryptedExitInstanceId));

                if (employee != null)
                {
                    model.EmployeeDetails = employee;
                }

                if (exit != null)
                {
                    model.SeparationFormDetails = exit;
                }

                if (designation != null)
                {
                    model.EmpdesignationDetails = designation;
                }

                model.EmployeeCode = Convert.ToInt32(employee.EmployeeCode);
                model.EmployeeId = employee.EmployeeId;
                model.SeparationFormDetails.SeparationId = exit.SeparationId;
                model.SeparationFormDetails.SeparationReasonList = objExitDal.GetSeparationReasonList();
                model.SeparationFormDetails.NoticePeriod = exit.NoticePeriod;
                model.SeparationFormDetails.SystemReleavingDate = exit.SystemReleavingDate;
                model.WaiveOffList = objExitDal.GetWaiveOffList();
                model.SeparationFormDetails.WaiveOff = exit.WaiveOff;
                return PartialView("_SeparationShowDetails", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult WithdrawEmployeeResignation(string exitInstanceId)
        {
            try
            {
                bool Status = false;
                bool isAuthorize;
                string decryptedExitInstanceId = HRMSHelper.Decrypt(exitInstanceId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                Status = objExitDal.WithdrawEmployeeResignation(Convert.ToInt32(decryptedExitInstanceId));
                return Json(new { status = Status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveShowDetailsData(SeparationShowDetails model)
        {
            try
            {
                bool status = objExitDal.SaveShowDetailsData(model);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ApproveShowDetailsData(SeparationShowDetails model)
        {
            try
            {
                bool status = objExitDal.ApproveShowDetailsData(model);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RejectShowDetailsData(SeparationShowDetails model)
        {
            try
            {
                bool status = objExitDal.RejectShowDetailsData(model);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SubmitShowDetailsData(SeparationShowDetails model)
        {
            try
            {
                bool status = objExitDal.SubmitShowDetailsData(model);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetExitInterviewFormDetails(string exitInstanceId)
        {
            try
            {
                ExitInterviewViewModel model = new ExitInterviewViewModel();
                model.EmployeeDetails = new EmployeeDetailsViewModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SeparationFormDetails = new ExitProcessViewModel();
                string hrclosurecomments = string.Empty;

                bool isAuthorize;
                string decryptedExitInstanceId = HRMSHelper.Decrypt(exitInstanceId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                bool result = objExitDal.CheckLoginUserIsEmpOrManager(employeeID);
                ExitProcessViewModel exitDetails = objExitDal.GetEmpSeparationDetails(Convert.ToInt32(decryptedExitInstanceId));

                if (exitDetails != null)
                    ViewBag.IsResignationExists = true;
                else
                    ViewBag.IsResignationExists = false;

                if (result == true)
                    ViewBag.IsManagerOrEMployee = "Manager";
                else
                    ViewBag.IsManagerOrEMployee = "Employee";

                EmployeeDetailsViewModel employee = objExitDal.GetEmpSeparationShowDetails(Convert.ToInt32(decryptedExitInstanceId));

                if (employee != null)
                    model.EmployeeDetails = employee;

                if (exitDetails != null)
                    model.SeparationFormDetails = exitDetails;

                model.EmployeeId = employee.EmployeeId;
                model.HiddenRadioId = 0;
                model.SeparationFormDetails.SeparationId = exitDetails.SeparationId;
                model.ListExitInterviewItems = objExitDal.GetExitInterviewForm(Convert.ToInt32(decryptedExitInstanceId), employeeID, out hrclosurecomments);
                if (hrclosurecomments != null)
                    model.HRClosureComments = hrclosurecomments;
                else
                    hrclosurecomments = "";

                //stage id for HR closure\
                tbl_HR_ExitInstance stageInfo = objExitDal.GetEmpExitTermination(Convert.ToInt32(decryptedExitInstanceId));
                model.HiddenStageId = stageInfo.stageID;

                return PartialView("_ExitInterviewForm", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SaveExitInterviewFormData(List<ExitInterview> model)
        {
            try
            {
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                bool status = objExitDal.SaveExitInterviewFormData(model, employeeID);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ApproveExitInterViewFormData(List<ExitInterview> model)
        {
            try
            {
                bool status = objExitDal.ApproveExitInterViewFormData(model);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool SendMail(SeparationMailTemplate model)
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

        //Department Approver for HR Admin
        [HttpGet]
        public ActionResult DepartmentFormForAdmin(string ExitInstanceId)
        {
            try
            {
                FinanceClearance fcmodel = new FinanceClearance();
                ExitProcessDAL ExitDAL = new ExitProcessDAL();

                bool isAuthorize;
                string decryptedExitInstanceId = HRMSHelper.Decrypt(ExitInstanceId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                ViewBag.EmployeeIdAdmin = ExitInstanceId;
                ViewBag.EmployeeIdProject = ExitInstanceId;
                ViewBag.EmployeeIdIT = ExitInstanceId;
                ViewBag.EmployeeIdHR = ExitInstanceId;
                ViewBag.EmployeeIdFinance = ExitInstanceId;
                ViewBag.EmployeeIdAsset = ExitInstanceId;
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                fcmodel.UserRole = user;
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                List<QuestionnaireQuestion> question = ExitDAL.GetAdminQuestionnaireQuestionDetails();
                fcmodel.QuestionnaireQuestions = question;
                tbl_Q_QuestionnaireQuestion FinanceRevision = ExitDAL.GetFinanceRevisionId();
                fcmodel.FinanceRevisionID = FinanceRevision.RevisionId;
                fcmodel.FinanceQuestionnaireID = FinanceRevision.QuestionnaireID;
                tbl_Q_QuestionnaireQuestion ITRevision = ExitDAL.GetITRevisionId();
                fcmodel.ITRevisionID = ITRevision.RevisionId;
                fcmodel.ITQuestionnaireID = ITRevision.QuestionnaireID;
                tbl_Q_QuestionnaireQuestion HRRevision = ExitDAL.GetHRRevisionId();
                fcmodel.HRRevisionID = HRRevision.RevisionId;
                fcmodel.HRQuestionnaireID = HRRevision.QuestionnaireID;
                tbl_Q_QuestionnaireQuestion AdminRevision = ExitDAL.GetAdminRevisionId();
                fcmodel.AdminRevisionID = AdminRevision.RevisionId;
                fcmodel.AdminQuestionnaireID = AdminRevision.QuestionnaireID;
                tbl_Q_QuestionnaireQuestion projectRevision = ExitDAL.GetProjectRevisionId();
                fcmodel.ProjectRevisionID = projectRevision.RevisionId;
                fcmodel.ProjectQuestionnaireID = projectRevision.QuestionnaireID;
                tbl_Q_QuestionnaireQuestion assetRevision = ExitDAL.GetAssetRevisionId();
                fcmodel.AssetRevisionID = assetRevision.RevisionId;
                fcmodel.AssetQuestionnaireID = assetRevision.QuestionnaireID;
                int employeeID = ExitDAL.GetjqinboxEmployeeID(Convert.ToInt32(decryptedExitInstanceId));
                fcmodel.EmployeeId = employeeID;
                List<DepartmentResponce> Responces = ExitDAL.GetResponceList(Convert.ToInt32(decryptedExitInstanceId));
                fcmodel.ResponceList = Responces;
                List<QuestionnaireOption> option1 = ExitDAL.GetAdminQuestionnaireOptionDetails();
                fcmodel.QuestionnaireOptions = option1;
                fcmodel.Mail = new SeparationMailTemplate();
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToBoolean(employeeID) ? employeeID : 0);
                FinanceClearance obj = ExitDAL.GettentativeDetails(Convert.ToInt32(decryptedExitInstanceId));
                fcmodel.EmployeeName = obj.EmployeeName;
                fcmodel.EmployeeCode = employeeDetails.EmployeeCode;
                fcmodel.TentativeReleaseDate = obj.TentativeReleaseDate;
                fcmodel.location = obj.location;
                fcmodel.ExitInstanceId = Convert.ToInt32(decryptedExitInstanceId);
                List<Tbl_HR_ExitStageEvent> departments = ExitDAL.LatestDepartmentEntry(Convert.ToInt32(decryptedExitInstanceId));
                foreach (var item in departments)
                {
                    if (item.QuestionnaireID == 9)
                    {
                        fcmodel.IsProjectStageCleared = "true";
                        break;
                    }
                    else
                        fcmodel.IsProjectStageCleared = "false";
                }
                foreach (var item in departments)
                {
                    if (item.QuestionnaireID == 4)
                    {
                        fcmodel.IsFinanceStageCleared = "true";
                        break;
                    }
                    else
                        fcmodel.IsFinanceStageCleared = "false";
                }
                foreach (var item in departments)
                {
                    if (item.QuestionnaireID == 2)
                    {
                        fcmodel.IsITStageCleared = "true";
                        break;
                    }
                    else
                        fcmodel.IsITStageCleared = "false";
                }
                foreach (var item in departments)
                {
                    if (item.QuestionnaireID == 5)
                    {
                        fcmodel.IsHRStageCleared = "true";
                        break;
                    }
                    else
                        fcmodel.IsHRStageCleared = "false";
                }
                foreach (var item in departments)
                {
                    if (item.QuestionnaireID == 3)
                    {
                        fcmodel.IsAdminStageCleared = "true";
                        break;
                    }
                    else
                        fcmodel.IsAdminStageCleared = "false";
                }
                foreach (var item in departments)
                {
                    if (item.QuestionnaireID == 20)
                    {
                        fcmodel.IsAssetStageCleared = "true";
                        break;
                    }
                    else
                        fcmodel.IsAssetStageCleared = "false";
                }
                List<ApproverList> GetApprovers = ExitDAL.GetDepartmentList(Convert.ToInt32(decryptedExitInstanceId));
                fcmodel.Approvers = GetApprovers;
                foreach (var item in GetApprovers)
                {
                    if (item.QuestionnaireID == 4)
                        ViewBag.Finance = (item.ApproverID).ToString();
                    else if (item.QuestionnaireID == 5)
                        ViewBag.HR = (item.ApproverID).ToString();
                    else if (item.QuestionnaireID == 3)
                        ViewBag.Admin = (item.ApproverID).ToString();
                    else if (item.QuestionnaireID == 2)
                        ViewBag.IT = (item.ApproverID).ToString();
                    else if (item.QuestionnaireID == 20)
                        ViewBag.Asset = (item.ApproverID).ToString();
                    else
                        ViewBag.Project = (item.ApproverID).ToString();
                }
                return PartialView("_WatchList_SeparationFinanceClearanceForm", fcmodel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult FinanceClearance(string ExitInstanceId)
        {
            try
            {
                FinanceClearance fcmodel = new FinanceClearance();
                ExitProcessDAL ExitDAL = new ExitProcessDAL();

                bool isAuthorize;
                string decryptedExitInstanceId = HRMSHelper.Decrypt(ExitInstanceId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                List<QuestionnaireQuestion> question = ExitDAL.GetFinanceQuestionnaireQuestionDetails();
                tbl_Q_QuestionnaireQuestion FinanceRevision = ExitDAL.GetFinanceRevisionId();
                fcmodel.FinanceRevisionID = FinanceRevision.RevisionId;
                fcmodel.FinanceQuestionnaireID = FinanceRevision.QuestionnaireID;
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                fcmodel.QuestionnaireQuestions = question;
                int employeeID = ExitDAL.GetjqinboxEmployeeID(Convert.ToInt32(decryptedExitInstanceId));
                fcmodel.EmployeeId = employeeID;
                ViewBag.EmployeeIdFinance = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(employeeID), true);

                List<DepartmentResponce> Responces = ExitDAL.GetResponceList(Convert.ToInt32(decryptedExitInstanceId));
                fcmodel.ResponceList = Responces;
                List<Tbl_HR_ExitStageEvent> departments = ExitDAL.LatestDepartmentEntry(Convert.ToInt32(decryptedExitInstanceId));
                foreach (var item in departments)
                {
                    if (item.QuestionnaireID == 4)
                    {
                        fcmodel.IsFinanceStageCleared = "true";
                        break;
                    }
                    else
                    {
                        fcmodel.IsFinanceStageCleared = "false";
                    }
                }
                List<QuestionnaireOption> option1 = ExitDAL.GetFinanceQuestionnaireOptionDetails();
                fcmodel.QuestionnaireOptions = option1;
                fcmodel.Mail = new SeparationMailTemplate();
                fcmodel.Mail.Message = fcmodel.Mail.Message ?? "";
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToBoolean(employeeID) ? employeeID : 0);
                FinanceClearance obj = ExitDAL.GettentativeDetails(Convert.ToInt32(decryptedExitInstanceId));
                fcmodel.EmployeeName = obj.EmployeeName;
                fcmodel.EmployeeCode = employeeDetails.EmployeeCode;
                fcmodel.TentativeReleaseDate = obj.TentativeReleaseDate;
                fcmodel.location = obj.location;
                fcmodel.ExitInstanceId = Convert.ToInt32(decryptedExitInstanceId);
                return PartialView("_SeparationFinanceClearanceForm", fcmodel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult savefinanceseparationDetails(FinanceClearance model)
        {
            try
            {
                string result = string.Empty;
                ExitProcessDAL dal = new ExitProcessDAL();

                var status = dal.savefinanceseparationDetails(model);

                if (status)
                    result = "Saved";
                else
                    result = "Error";

                return Json(new { resultMesssage = result, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult saveAssetseparationDetails(FinanceClearance model)
        {
            try
            {
                string result = string.Empty;
                ExitProcessDAL dal = new ExitProcessDAL();

                var status = dal.saveAssetseparationDetails(model);

                if (status)
                    result = "Saved";
                else
                    result = "Error";

                return Json(new { resultMesssage = result, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ITClearance(string ExitInstanceId)
        {
            try
            {
                FinanceClearance ITCmodel = new FinanceClearance();
                ExitProcessDAL ExitDAL = new ExitProcessDAL();

                bool isAuthorize;
                string decryptedExitInstanceId = HRMSHelper.Decrypt(ExitInstanceId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                List<QuestionnaireQuestion> question = ExitDAL.GetITQuestionnaireQuestionDetails();
                tbl_Q_QuestionnaireQuestion ITRevision = ExitDAL.GetITRevisionId();
                ITCmodel.ITRevisionID = ITRevision.RevisionId;
                ITCmodel.ITQuestionnaireID = ITRevision.QuestionnaireID;
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                ITCmodel.QuestionnaireQuestions = question;
                int employeeID = ExitDAL.GetjqinboxEmployeeID(Convert.ToInt32(decryptedExitInstanceId));
                ITCmodel.EmployeeId = employeeID;
                ViewBag.EmployeeIdIT = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(employeeID), true);

                List<DepartmentResponce> Responces = ExitDAL.GetResponceList(Convert.ToInt32(decryptedExitInstanceId));
                ITCmodel.ResponceList = Responces;
                List<QuestionnaireOption> option1 = ExitDAL.GetITQuestionnaireOptionDetails();
                ITCmodel.QuestionnaireOptions = option1;
                List<Tbl_HR_ExitStageEvent> departments = ExitDAL.LatestDepartmentEntry(Convert.ToInt32(decryptedExitInstanceId));
                foreach (var item in departments)
                {
                    if (item.QuestionnaireID == 2)
                    {
                        ITCmodel.IsITStageCleared = "true";
                        break;
                    }
                    else
                    {
                        ITCmodel.IsITStageCleared = "false";
                    }
                }
                ITCmodel.Mail = new SeparationMailTemplate();
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToBoolean(employeeID) ? employeeID : 0);
                FinanceClearance obj = ExitDAL.GettentativeDetails(Convert.ToInt32(decryptedExitInstanceId));
                ITCmodel.EmployeeName = obj.EmployeeName;
                ITCmodel.EmployeeCode = employeeDetails.EmployeeCode;
                ITCmodel.TentativeReleaseDate = obj.TentativeReleaseDate;
                ITCmodel.location = obj.location;
                ITCmodel.ExitInstanceId = obj.ExitInstanceId;
                return PartialView("_SeparationITClearanceForm", ITCmodel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        //Asset Department
        [HttpGet]
        public ActionResult AssetClearance(string ExitInstanceId)
        {
            try
            {
                FinanceClearance AssetCmodel = new FinanceClearance();
                ExitProcessDAL ExitDAL = new ExitProcessDAL();

                bool isAuthorize;
                string decryptedExitInstanceId = HRMSHelper.Decrypt(ExitInstanceId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                List<QuestionnaireQuestion> question = ExitDAL.GetAssetQuestionnaireQuestionDetails();
                tbl_Q_QuestionnaireQuestion AssetRevision = ExitDAL.GetAssetRevisionId();
                AssetCmodel.AssetRevisionID = AssetRevision.RevisionId;
                AssetCmodel.AssetQuestionnaireID = AssetRevision.QuestionnaireID;
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                AssetCmodel.QuestionnaireQuestions = question;
                int employeeID = ExitDAL.GetjqinboxEmployeeID(Convert.ToInt32(decryptedExitInstanceId));
                AssetCmodel.EmployeeId = employeeID;
                ViewBag.EmployeeIdAsset = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(employeeID), true);

                List<DepartmentResponce> Responces = ExitDAL.GetResponceList(Convert.ToInt32(decryptedExitInstanceId));
                AssetCmodel.ResponceList = Responces;
                List<QuestionnaireOption> option1 = ExitDAL.GetAssetQuestionnaireOptionDetails();
                AssetCmodel.QuestionnaireOptions = option1;
                List<Tbl_HR_ExitStageEvent> departments = ExitDAL.LatestDepartmentEntry(Convert.ToInt32(decryptedExitInstanceId));
                foreach (var item in departments)
                {
                    if (item.QuestionnaireID == 20)
                    {
                        AssetCmodel.IsAssetStageCleared = "true";
                        break;
                    }
                    else
                    {
                        AssetCmodel.IsAssetStageCleared = "false";
                    }
                }
                AssetCmodel.Mail = new SeparationMailTemplate();
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToBoolean(employeeID) ? employeeID : 0);
                FinanceClearance obj = ExitDAL.GettentativeDetails(Convert.ToInt32(decryptedExitInstanceId));
                AssetCmodel.EmployeeName = obj.EmployeeName;
                AssetCmodel.EmployeeCode = employeeDetails.EmployeeCode;
                AssetCmodel.TentativeReleaseDate = obj.TentativeReleaseDate;
                AssetCmodel.location = obj.location;
                AssetCmodel.ExitInstanceId = obj.ExitInstanceId;
                return PartialView("_SeparationAssetClearanceForm", AssetCmodel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult ProjectClearance(string ExitInstanceId)
        {
            try
            {
                FinanceClearance projectModel = new FinanceClearance();
                ExitProcessDAL ExitDAL = new ExitProcessDAL();

                bool isAuthorize;
                string decryptedExitInstanceId = HRMSHelper.Decrypt(ExitInstanceId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                List<QuestionnaireQuestion> question = ExitDAL.GetProjectQuestionnaireQuestionDetails();
                tbl_Q_QuestionnaireQuestion projectRevision = ExitDAL.GetProjectRevisionId();
                projectModel.ProjectRevisionID = projectRevision.RevisionId;
                projectModel.ProjectQuestionnaireID = projectRevision.QuestionnaireID;
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                projectModel.QuestionnaireQuestions = question;
                int employeeID = ExitDAL.GetjqinboxEmployeeID(Convert.ToInt32(decryptedExitInstanceId));
                projectModel.EmployeeId = employeeID;
                ViewBag.EmployeeIdProject = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(employeeID), true);

                List<DepartmentResponce> Responces = ExitDAL.GetResponceList(Convert.ToInt32(decryptedExitInstanceId));
                projectModel.ResponceList = Responces;
                List<QuestionnaireOption> option1 = ExitDAL.GetProjectQuestionnaireOptionDetails();
                projectModel.QuestionnaireOptions = option1;
                List<Tbl_HR_ExitStageEvent> departments = ExitDAL.LatestDepartmentEntry(Convert.ToInt32(decryptedExitInstanceId));
                foreach (var item in departments)
                {
                    if (item.QuestionnaireID == 9)
                    {
                        projectModel.IsProjectStageCleared = "true";
                        break;
                    }
                    else
                        projectModel.IsProjectStageCleared = "false";
                }
                projectModel.Mail = new SeparationMailTemplate();
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToBoolean(employeeID) ? employeeID : 0);
                FinanceClearance obj = ExitDAL.GettentativeDetails(Convert.ToInt32(decryptedExitInstanceId));
                projectModel.EmployeeName = obj.EmployeeName;
                projectModel.EmployeeCode = employeeDetails.EmployeeCode;
                projectModel.TentativeReleaseDate = obj.TentativeReleaseDate;
                projectModel.location = obj.location;
                projectModel.ExitInstanceId = obj.ExitInstanceId;
                return PartialView("_SeparationProjectClearanceForm", projectModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult HRClearance(string ExitInstanceId)
        {
            try
            {
                FinanceClearance HRCmodel = new FinanceClearance();
                ExitProcessDAL ExitDAL = new ExitProcessDAL();

                bool isAuthorize;
                string decryptedExitInstanceId = HRMSHelper.Decrypt(ExitInstanceId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                List<QuestionnaireQuestion> question = ExitDAL.GetHRQuestionnaireQuestionDetails();
                tbl_Q_QuestionnaireQuestion HRRevision = ExitDAL.GetHRRevisionId();
                HRCmodel.HRRevisionID = HRRevision.RevisionId;
                HRCmodel.HRQuestionnaireID = HRRevision.QuestionnaireID;
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                HRCmodel.QuestionnaireQuestions = question;
                int employeeID = ExitDAL.GetjqinboxEmployeeID(Convert.ToInt32(decryptedExitInstanceId));
                HRCmodel.EmployeeId = employeeID;
                ViewBag.EmployeeIdHR = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(employeeID), true);

                List<DepartmentResponce> Responces = ExitDAL.GetResponceList(Convert.ToInt32(decryptedExitInstanceId));
                HRCmodel.ResponceList = Responces;
                List<QuestionnaireOption> option1 = ExitDAL.GetHRQuestionnaireOptionDetails();
                HRCmodel.QuestionnaireOptions = option1;
                List<Tbl_HR_ExitStageEvent> departments = ExitDAL.LatestDepartmentEntry(Convert.ToInt32(decryptedExitInstanceId));
                foreach (var item in departments)
                {
                    if (item.QuestionnaireID == 5)
                    {
                        HRCmodel.IsHRStageCleared = "true";
                        break;
                    }
                    else
                        HRCmodel.IsHRStageCleared = "false";
                }
                HRCmodel.Mail = new SeparationMailTemplate();
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToBoolean(employeeID) ? employeeID : 0);
                FinanceClearance obj = ExitDAL.GettentativeDetails(Convert.ToInt32(decryptedExitInstanceId));
                HRCmodel.EmployeeName = obj.EmployeeName;
                HRCmodel.EmployeeCode = employeeDetails.EmployeeCode;
                HRCmodel.TentativeReleaseDate = obj.TentativeReleaseDate;
                HRCmodel.location = obj.location;
                HRCmodel.ExitInstanceId = obj.ExitInstanceId;
                return PartialView("_SeparationHRClearanceForm", HRCmodel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult ADMINClearance(string ExitInstanceId)
        {
            try
            {
                FinanceClearance ADMINCmodel = new FinanceClearance();
                ExitProcessDAL ExitDAL = new ExitProcessDAL();

                bool isAuthorize;
                string decryptedExitInstanceId = HRMSHelper.Decrypt(ExitInstanceId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                List<QuestionnaireQuestion> question = ExitDAL.GetADMINQuestionnaireQuestionDetails();
                ADMINCmodel.QuestionnaireQuestions = question;
                tbl_Q_QuestionnaireQuestion AdminRevision = ExitDAL.GetAdminRevisionId();
                ADMINCmodel.AdminRevisionID = AdminRevision.RevisionId;
                ADMINCmodel.AdminQuestionnaireID = AdminRevision.QuestionnaireID;
                int employeeID = ExitDAL.GetjqinboxEmployeeID(Convert.ToInt32(decryptedExitInstanceId));
                ADMINCmodel.EmployeeId = employeeID;
                ViewBag.EmployeeIdAdmin = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(employeeID), true);

                List<DepartmentResponce> Responces = ExitDAL.GetResponceList(Convert.ToInt32(decryptedExitInstanceId));
                ADMINCmodel.ResponceList = Responces;
                List<QuestionnaireOption> option1 = ExitDAL.GetADMINQuestionnaireOptionDetails();
                ADMINCmodel.QuestionnaireOptions = option1;
                List<Tbl_HR_ExitStageEvent> departments = ExitDAL.LatestDepartmentEntry(Convert.ToInt32(decryptedExitInstanceId));
                foreach (var item in departments)
                {
                    if (item.QuestionnaireID == 3)
                    {
                        ADMINCmodel.IsAdminStageCleared = "true";
                        break;
                    }
                    else
                        ADMINCmodel.IsAdminStageCleared = "false";
                }
                ADMINCmodel.Mail = new SeparationMailTemplate();
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToBoolean(employeeID) ? employeeID : 0);
                FinanceClearance obj = ExitDAL.GettentativeDetails(Convert.ToInt32(decryptedExitInstanceId));
                ADMINCmodel.EmployeeName = obj.EmployeeName;
                ADMINCmodel.EmployeeCode = employeeDetails.EmployeeCode;
                ADMINCmodel.TentativeReleaseDate = obj.TentativeReleaseDate;
                ADMINCmodel.location = obj.location;
                ADMINCmodel.ExitInstanceId = obj.ExitInstanceId;
                return PartialView("_SeparationADMINClearanceForm", ADMINCmodel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult PendingClearanceMailTemplate(string employeeId, string collection)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                string collectionwithComma = collection.TrimEnd(';');
                string collectionstring = collectionwithComma.Replace(";", Environment.NewLine);
                FinanceClearance model = new FinanceClearance();
                model.Mail = new SeparationMailTemplate();
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(decryptedEmployeeId));
                if (employeeDetails != null)
                {
                    model.Mail.From = loginuser.EmailID;

                    model.Mail.To = employeeDetails.EmailID;

                    string mailBody = ("Hello, " + "<br>" + "<br>" +
                        "This is to notify that following are the items pending during your Separation process." + "<br>" + "<br>" +
                         collectionstring + "<br>" + "<br>" +
                         "Please do the needful." + "<br>" + "<br>" +
                          "Regards," + "<br>" + "<br>" +
                          Server.HtmlEncode(loginuser.EmployeeName));
                    model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);

                    ViewBag.Body = mailBody;

                    model.Mail.Subject = "Due pendings during your Separation process";

                    string[] roles = { "HR Admin", "RMG" };

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

                return PartialView("_MailTemplateSeparation", model.Mail);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult MoveAheadSendMail(string employeeId)
        {
            try
            {
                FinanceClearance model = new FinanceClearance();

                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                model.Mail = new SeparationMailTemplate();
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToBoolean(Convert.ToInt32(decryptedEmployeeId)) ? Convert.ToInt32(decryptedEmployeeId) : 0);
                if (employeeDetails != null)
                {
                    model.Mail.From = loginuser.EmailID;
                    model.Mail.To = employeeDetails.EmailID;
                    string mailBody = null;
                    string subject = null;
                    int templateId = 0;
                    List<EmployeeMailTemplate> template = new List<EmployeeMailTemplate>();
                    templateId = 29;
                    template = Commondal.GetEmailTemplate(templateId);
                    foreach (var emailTemplate in template)
                    {
                        subject = emailTemplate.Subject;
                        mailBody = emailTemplate.Message;
                    }
                    subject = subject.Replace("##Department Clearance Stakeholder##", loginuser.EmployeeName);
                    model.Mail.Subject = subject;
                    mailBody = mailBody.Replace("##employee name##", employeeDetails.EmployeeName);
                    mailBody = mailBody.Replace("##Department Clearance Stakeholder##", loginuser.EmployeeName);
                    mailBody = mailBody.Replace("##Stakeholder employee code##", loginuser.EmployeeCode);
                    model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                    ViewBag.Body = mailBody;
                    string[] roles = { "HR Admin", "RMG" };

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

                return PartialView("_MailTemplateSeparation", model.Mail);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult PushBackDetailsData(int ExitInstanceId)
        {
            try
            {
                bool status = objExitDal.PushBackDetailsData(ExitInstanceId);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult MoveAheadDetailsData(FinanceClearance model)
        {
            try
            {
                bool status = objExitDal.MoveAheadDetailsData(model);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult PushBackHRClosure(string exitInstanceId)
        {
            try
            {
                bool isAuthorize;
                string decryptedExitInstanceId = HRMSHelper.Decrypt(exitInstanceId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                bool status = objExitDal.PushBackHRClosure(Convert.ToInt32(decryptedExitInstanceId));
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}