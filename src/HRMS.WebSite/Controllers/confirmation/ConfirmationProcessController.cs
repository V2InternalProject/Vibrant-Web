using HRMS.DAL;
using HRMS.Helper;
using HRMS.Models;
using HRMS.Notification;
using MvcApplication3.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class ConfirmationProcessController : Controller
    {
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();
        private EmployeeDAL employeeDAL = new EmployeeDAL();

        /// <summary>
        /// Depending on the list of pending to take action(confirmation process,intitated) link is displayed.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConfigureParameter()
        {
            try
            {
                ConfirmationProcess cnfmodel = new ConfirmationProcess();
                ConfirmationDAL dal = new ConfirmationDAL();
                cnfmodel.SearchedUserDetails = new SearchedUserDetails();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                cnfmodel.LoggedInUserRole = user;
                ViewBag.UserRole = user;
                cnfmodel.SearchedUserDetails.UserRole = user;
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                cnfmodel.SearchedUserDetails.EmployeeId = employeeID;
                string userRole = user;
                cnfmodel.ConfigurationDetailsList = new List<ConfigurationDetailsViewModel>();
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                int loginUserId = Convert.ToInt32(loginName);
                int page = 1;
                int rows = 5;
                string term = "";
                string Field = "0";
                string FieldChild = "0";
                int totalCount;
                List<ConfirmationDetailsViewModel> searchResultInbox = new List<ConfirmationDetailsViewModel>();
                searchResultInbox = dal.InboxSearchEmployeeForConfirmationLoadGrid(term, Field, FieldChild, page, rows, loginUserId, out totalCount);
                totalCount = searchResultInbox.Count();
                cnfmodel.ApprovalsCount = totalCount;
                return PartialView("InboxWatchlistGrid", cnfmodel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        /// <summary>
        /// Retrives the details of inbox and watchlist results.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConfirmatioProcess()
        {
            try
            {
                ConfirmationDetailsViewModel model = new ConfirmationDetailsViewModel();
                ConfirmationDAL dal = new ConfirmationDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                model.SearchedUserDetails.UserRole = user;
                //model.hiddenid = null;
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                model.SearchedUserDetails.EmployeeId = employeeID;
                string userRole = user;
                model.FieldchildList = new List<FieldChildList>();
                return PartialView("_ConfirmationProcessDetails", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public JsonResult GetFieldDropdownDetails(string FieldName)
        {
            try
            {
                var objStatus = new JsonResult();
                ConfirmationDAL cnfdal = new ConfirmationDAL();
                List<FieldChildList> childList = new List<FieldChildList>();
                childList = cnfdal.GetFieldChildDetails(FieldName);
                objStatus.Data = childList;
                objStatus.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return objStatus;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InitiateConfirmationProcess()
        {
            try
            {
                InitiatConfirmationProcess cnfmodel = new InitiatConfirmationProcess();
                ConfirmationDAL dal = new ConfirmationDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                cnfmodel.SearchedUserDetails = new SearchedUserDetails();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                cnfmodel.SearchedUserDetails.UserRole = user;
                string userRole = user;
                cnfmodel.MailDetail = new MailTemplateViewModel();
                cnfmodel.MailDetail.Message = "";
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                cnfmodel.SearchedUserDetails.EmployeeId = employeeID;
                return PartialView("_ConfigurationDetailRecord", cnfmodel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="initiateEmpID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConfirmInitiate(string initiateEmpID)
        {
            try
            {
                InitiatConfirmationProcess cnfmodel = new InitiatConfirmationProcess();
                ConfirmationDAL dal = new ConfirmationDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();

                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(initiateEmpID, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                ViewBag.encryptedEmployeeId = initiateEmpID;

                cnfmodel.SearchedUserDetails = new SearchedUserDetails();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                cnfmodel.SearchedUserDetails.UserRole = user;
                string userRole = user;
                cnfmodel.MailDetail = new MailTemplateViewModel();
                cnfmodel.MailDetail.Message = "";
                string employeeId = decryptedEmployeeId;
                HRMS_tbl_PM_Employee Empinfo = employeeDAL.GetEmployeeDetails(Convert.ToInt32(employeeId));
                cnfmodel.EmployeeCode = Empinfo.EmployeeCode;
                cnfmodel.EmployeeId = Empinfo.EmployeeID;
                cnfmodel.EmployeeName = Empinfo.EmployeeName;
                cnfmodel.InitiationDate = DateTime.Now.Date;
                HRMS_tbl_PM_Employee comfirmationManager = employeeDAL.GetEmployeeDetails(Convert.ToInt32(Empinfo.ReportingTo));
                if (comfirmationManager != null)
                    cnfmodel.ReportingManager = comfirmationManager.EmployeeName;
                HRMS_tbl_PM_Employee Reviewer = employeeDAL.GetEmployeeDetails(Convert.ToInt32(Empinfo.CompetencyManager));
                if (Reviewer != null)
                    cnfmodel.Reviewer = Reviewer.EmployeeName;
                ViewBag.ReportingTo = new SelectList(dal.GetManagerList_Emp(Empinfo.EmployeeID), "EmployeeId", "EmployeeName");
                string[] roles = { "HR Admin" };
                Dictionary<int, string> HrAdmins = new Dictionary<int, string>();
                foreach (string r in roles)
                {
                    string[] users = Roles.GetUsersInRole(r);
                    foreach (string userlist in users)
                    {
                        HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsByEmployeeCode(userlist);
                        if (employee != null)
                            HrAdmins.Add(employee.EmployeeID, employee.EmployeeName);
                    }
                }
                ViewBag.admin = new SelectList(HrAdmins, "Key", "Value");
                return PartialView("_ConfirmInitiate", cnfmodel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        /// <summary>
        /// Retrive the list of records depending on the search term.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult SearchEmployeeAutoSuggestCnfrm(string term)
        {
            try
            {
                ConfirmationDAL dal = new ConfirmationDAL();
                InitiatConfirmationProcess model = new InitiatConfirmationProcess();
                List<InitiatConfirmationProcess> searchResult = new List<InitiatConfirmationProcess>();
                searchResult = dal.SearchEmployee(term, 1, 80);

                ViewBag.result = searchResult;
                return Json(searchResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrive all the records which are pending to get intitated and the records which are already initiated.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InitiateConfirmationLoadGrid(string term, int page, int rows)
        {
            try
            {
                InitiatConfirmationProcess model = new InitiatConfirmationProcess();
                ConfirmationProcess confrmDal = new ConfirmationProcess();
                ConfirmationDAL dal = new ConfirmationDAL();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                confrmDal.ConfigurationDetailsList = new List<ConfigurationDetailsViewModel>();
                int totalCount;
                List<InitiatConfirmationProcess> searchResult = new List<InitiatConfirmationProcess>();
                searchResult = dal.SearchEmployeeForLoadGrid(term, page, rows, out totalCount);
                if ((searchResult == null || searchResult.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    searchResult = dal.SearchEmployeeForLoadGrid(term, page, rows, out totalCount);
                }
                List<InitiatConfirmationProcess> finalSearchResult = new List<InitiatConfirmationProcess>();
                foreach (var item in searchResult)
                {
                    string initiateEmployeeId = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.EmployeeId), true);
                    item.encryptedEmployeeId = initiateEmployeeId;
                    finalSearchResult.Add(item);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = finalSearchResult,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        /// <summary>
        /// Retrives records store in the inbox of the user.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="Field"></param>
        /// <param name="FieldChild"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InboxConfirmationProcessLoadGrid(string term, string Field, string FieldChild, int page, int rows)
        {
            try
            {
                ConfirmationDetailsViewModel model = new ConfirmationDetailsViewModel();
                ConfirmationProcess confrmDal = new ConfirmationProcess();
                ConfirmationDAL dal = new ConfirmationDAL();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                confrmDal.ConfigurationDetailsList = new List<ConfigurationDetailsViewModel>();
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                int loginUserId = Convert.ToInt32(loginName);
                int totalCount;
                List<ConfirmationDetailsViewModel> searchResultInbox = new List<ConfirmationDetailsViewModel>();
                searchResultInbox = dal.InboxSearchEmployeeForConfirmationLoadGrid(term, Field, FieldChild, page, rows, loginUserId, out totalCount);
                if ((searchResultInbox == null || searchResultInbox.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    searchResultInbox = dal.InboxSearchEmployeeForConfirmationLoadGrid(term, Field, FieldChild, page, rows, loginUserId, out totalCount);
                }
                List<ConfirmationDetailsViewModel> finalSearchResultInbox = new List<ConfirmationDetailsViewModel>();
                foreach (var item in searchResultInbox)
                {
                    string inboxEmployeeId = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.EmployeeId), true);
                    item.encryptedEmployeeId = inboxEmployeeId;
                    finalSearchResultInbox.Add(item);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = finalSearchResultInbox,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        /// <summary>
        /// Retrive records store in the watchlist of the user.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="Field"></param>
        /// <param name="FieldChild"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WatchListConfirmationProcessLoadGrid(string term, string Field, string FieldChild, int page, int rows)
        {
            try
            {
                ConfirmationDetailsViewModel model = new ConfirmationDetailsViewModel();
                ConfirmationProcess confrmDal = new ConfirmationProcess();
                ConfirmationDAL dal = new ConfirmationDAL();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                confrmDal.ConfigurationDetailsList = new List<ConfigurationDetailsViewModel>();
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                int loginUserId = Convert.ToInt32(loginName);
                int totalCount;
                List<ConfirmationDetailsViewModel> searchResultWatchList = new List<ConfirmationDetailsViewModel>();
                searchResultWatchList = dal.WatchListSearchEmployeeForConfirmationLoadGrid(term, Field, FieldChild, page, rows, loginUserId, out totalCount);
                if ((searchResultWatchList == null || searchResultWatchList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    searchResultWatchList = dal.WatchListSearchEmployeeForConfirmationLoadGrid(term, Field, FieldChild, page, rows, loginUserId, out totalCount);
                }
                List<ConfirmationDetailsViewModel> finalSearchResultInbox = new List<ConfirmationDetailsViewModel>();
                foreach (var item in searchResultWatchList)
                {
                    string watchlistEmployeeId = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.EmployeeId), true);
                    item.encryptedEmployeeId = watchlistEmployeeId;
                    finalSearchResultInbox.Add(item);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = finalSearchResultInbox,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        /// <summary>
        /// Retrive the status related details of the user.
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ShowStatusDetails(string EmployeeId, string ConfirmationId, int page, int rows)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(EmployeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                ConfirmationDetailsViewModel model = new ConfirmationDetailsViewModel();
                ConfirmationProcess confrmDal = new ConfirmationProcess();
                ConfirmationDAL dal = new ConfirmationDAL();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                confrmDal.ConfigurationDetailsList = new List<ConfigurationDetailsViewModel>();
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                int loginUserId = Convert.ToInt32(loginName);

                int totalCount;
                List<ShowStatus> ShowStatusResult = new List<ShowStatus>();
                ShowStatusResult = dal.GetShowStatusResult(decryptedEmployeeId, ConfirmationId, page, rows);
                totalCount = ShowStatusResult.Count();
                if ((ShowStatusResult == null || ShowStatusResult.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    ShowStatusResult = dal.GetShowStatusResult(decryptedEmployeeId, ConfirmationId, page, rows);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = ShowStatusResult,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        /// <summary>
        /// Retrive list of manager.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult GetManagerList(string term)
        {
            try
            {
                List<InitiatConfirmationProcess> getmanagers = new List<InitiatConfirmationProcess>();
                ConfirmationDAL dal = new ConfirmationDAL();
                getmanagers = dal.GetManagersList();
                var jsonData = new
                {
                    rows = getmanagers,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Stores intitated confirmation details record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveInitiateConfirmationDetails(InitiatConfirmationProcess model)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                ConfirmationDAL CnfDal = new ConfirmationDAL();
                bool resultMessage = false;
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                tbl_CF_Confirmation initiateProcess = new tbl_CF_Confirmation()
                {
                    EmployeeID = model.EmployeeId,
                    ConfirmationCoordinator = Convert.ToInt32(model.ConfirmationCoordinator),
                    ReportingManager = Convert.ToInt32(model.ReportingManager),
                    ReportingManager2 = Convert.ToInt32(model.ReportingManager2),
                    Reviewer = Convert.ToInt32(model.Reviewer),
                    HRReviewer = Convert.ToInt32(model.HRReviewer),
                    Comments = model.Comments,
                    ConfirmationID = model.confirmationid,
                    stageID = 3,
                    ConfirmationInitiationDate = DateTime.Now
                };
                resultMessage = CnfDal.InitiateConfirmationDetail(initiateProcess, loginuser.EmployeeID);
                return Json(new { status = resultMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Create the template of the mail.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="IsApproveOrReject"></param>
        /// <param name="IsAcceptExtendPIP"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEmailTemplate(string employeeId, string IsApproveOrReject = "", string IsAcceptExtendPIP = "")
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                EmployeeDAL employeeDAL = new EmployeeDAL();
                ConfirmationDAL CnfDal = new ConfirmationDAL();
                DateTime formFillingDate = DateTime.Now.Date;
                var startDate = DateTime.Now.DayOfWeek;
                DateTime formFillDate, mangerDate, reviewrDate;

                if (startDate.ToString() == "Monday")
                {
                    formFillDate = DateTime.Now.AddDays(2);
                    mangerDate = DateTime.Now.AddDays(4);
                    reviewrDate = DateTime.Now.AddDays(7);
                }
                else
                {
                    if (startDate.ToString() == "Wednesday")
                    {
                        formFillDate = DateTime.Now.AddDays(2);
                        mangerDate = DateTime.Now.AddDays(6);
                        reviewrDate = DateTime.Now.AddDays(8);
                    }
                    else
                    {
                        if (startDate.ToString() == "Friday")
                        {
                            formFillDate = DateTime.Now.AddDays(3);
                            mangerDate = DateTime.Now.AddDays(5);
                            reviewrDate = DateTime.Now.AddDays(7);
                        }
                        else
                        {
                            if (startDate.ToString() == "Tuesday")
                            {
                                formFillDate = DateTime.Now.AddDays(2);
                                mangerDate = DateTime.Now.AddDays(6);
                                reviewrDate = DateTime.Now.AddDays(8);
                            }
                            else
                            {
                                if (startDate.ToString() == "Thursday")
                                {
                                    formFillDate = DateTime.Now.AddDays(4);
                                    mangerDate = DateTime.Now.AddDays(6);
                                    reviewrDate = DateTime.Now.AddDays(8);
                                }
                                else
                                {
                                    formFillDate = DateTime.Now.AddDays(2);
                                    mangerDate = DateTime.Now.AddDays(4);
                                    reviewrDate = DateTime.Now.AddDays(8);
                                }
                            }
                        }
                    }
                }

                InitiatConfirmationProcess model = new InitiatConfirmationProcess();

                model.MailDetail = new MailTemplateViewModel();

                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(decryptedEmployeeId));

                if (employeeDetails != null)
                {
                    tbl_CF_Confirmation confirmationDetails = CnfDal.getConfirmationId(Convert.ToInt32(decryptedEmployeeId));
                    tbl_CF_TempConfirmation extendConfirmation = CnfDal.GetTempConfirmation(confirmationDetails.ConfirmationID);
                    if (confirmationDetails != null)
                    {
                        string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                        string loginUserId = loginName;
                        string mailbody = null;
                        string subject = null;
                        int templateId = 0;
                        List<EmployeeMailTemplate> template = new List<EmployeeMailTemplate>();
                        HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                        model.MailDetail.From = loginuser.EmailID;

                        HRMS_tbl_PM_Employee managerID = new HRMS_tbl_PM_Employee();
                        HRMS_tbl_PM_Employee managerID2 = new HRMS_tbl_PM_Employee();
                        HRMS_tbl_PM_Employee reviewerID = new HRMS_tbl_PM_Employee();
                        HRMS_tbl_PM_Employee hrID = new HRMS_tbl_PM_Employee();
                        HRMS_tbl_PM_Employee cordinatorID = new HRMS_tbl_PM_Employee();
                        managerID = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.ReportingManager));
                        managerID2 = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.ReportingManager2));
                        reviewerID = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.Reviewer));
                        hrID = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.HRReviewer));
                        cordinatorID = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.ConfirmationCoordinator));
                        model.MailDetail.From = loginuser.EmailID;
                        if (confirmationDetails.ReportingManager2 != null)
                        {
                            managerID2 = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.ReportingManager2));
                        }

                        if (confirmationDetails.stageID == 1)
                        {
                            if (IsAcceptExtendPIP == "extend" || IsAcceptExtendPIP == "terminate")
                            {
                                model.MailDetail.To = employeeDetails.EmailID;
                                //model.MailDetail.From = hrID.EmailID;
                                //if (managerID2 != null)
                                //    model.MailDetail.Cc = loginuser.EmailID + ";" + managerID2.EmailID + ";" + reviewerID.EmailID + ";" + hrID.EmailID + ";" + managerID.EmailID + ";" + cordinatorID.EmailID + ";";
                                //else
                                //model.MailDetail.Cc = loginuser.EmailID + ";" + managerID.EmailID + ";" + hrID.EmailID + ";" + managerID.EmailID + ";" + cordinatorID.EmailID + ";";
                                model.MailDetail.Cc = loginuser.EmailID + ";" + managerID.EmailID + ";" + hrID.EmailID + ";";
                                string[] Loginroles = { "HR Admin", "RMG" };
                                foreach (string r in Loginroles)
                                {
                                    string[] users = Roles.GetUsersInRole(r);
                                    foreach (string userR in users)
                                    {
                                        HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
                                        if (employee == null)
                                            model.MailDetail.Cc = model.MailDetail.Cc + string.Empty;
                                        else
                                            model.MailDetail.Cc = model.MailDetail.Cc + employee.EmailID + ";";
                                    }
                                }

                                if (IsAcceptExtendPIP == "extend")
                                {
                                    templateId = 20;
                                    template = Commondal.GetEmailTemplate(templateId);
                                    foreach (var emailTemplate in template)
                                    {
                                        subject = emailTemplate.Subject;
                                        mailbody = emailTemplate.Message;
                                    }
                                    subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                                    model.MailDetail.Subject = subject;
                                    mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                                    mailbody = mailbody.Replace("##new probation review date##", employeeDetails.Probation_Review_Date.Value.Date.ToString("d"));
                                    mailbody = mailbody.Replace("##comments entered by HR coordinator##", extendConfirmation.ExtensionComments);
                                    mailbody = mailbody.Replace("##Confirmation coordinator##", cordinatorID.EmployeeName);
                                    model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                                    ViewBag.body = mailbody;
                                }
                                if (IsAcceptExtendPIP == "terminate")
                                {
                                    templateId = 21;
                                    template = Commondal.GetEmailTemplate(templateId);
                                    foreach (var emailTemplate in template)
                                    {
                                        subject = emailTemplate.Subject;
                                        mailbody = emailTemplate.Message;
                                    }
                                    subject = subject.Replace("##Employee name##", employeeDetails.EmployeeName);
                                    model.MailDetail.Subject = subject;
                                    mailbody = mailbody.Replace("##Confirmation coordinator##", cordinatorID.EmployeeName);
                                    model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                                    ViewBag.body = mailbody;
                                }
                            }
                            else if (IsApproveOrReject == "Approved")
                            {
                                // if (loginuser.EmployeeID == confirmationDetails.Reviewer)
                                model.MailDetail.To = hrID.EmailID;
                                //if (loginuser.EmployeeID == confirmationDetails.HRReviewer)
                                //    model.MailDetail.To = cordinatorID.EmailID;
                                //if (managerID2 != null)
                                //    model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + managerID2.EmailID + ";" + managerID.EmailID + ";" + employeeDetails.EmailID + ";" + cordinatorID.EmailID + ";";
                                //else
                                //    model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + managerID.EmailID + ";" + employeeDetails.EmailID + ";" + cordinatorID.EmailID + ";";

                                model.MailDetail.Cc = loginuser.EmailID + ";" + managerID.EmailID + ";" + hrID.EmailID + ";";
                                templateId = 16;
                                template = Commondal.GetEmailTemplate(templateId);
                                foreach (var emailTemplate in template)
                                {
                                    subject = emailTemplate.Subject;
                                    mailbody = emailTemplate.Message;
                                }
                                subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                                model.MailDetail.Subject = subject;
                                mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                                mailbody = mailbody.Replace("##Reviewer name##", Server.HtmlEncode(loginuser.EmployeeName));
                                model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                                ViewBag.body = mailbody;
                            }
                        }
                        else if (confirmationDetails.stageID == 3 && IsApproveOrReject == "Reject")//manager reject
                        {
                            model.MailDetail.To = employeeDetails.EmailID;
                            if (managerID2 != null)
                            {
                                if (managerID2.EmployeeID != loginuser.EmployeeID)
                                    model.MailDetail.Cc = loginuser.EmailID + ";" + managerID2.EmailID + ";" + reviewerID.EmailID + ";" + hrID.EmailID + ";" + cordinatorID.EmailID + ";";
                                else
                                    model.MailDetail.Cc = loginuser.EmailID + ";" + managerID.EmailID + ";" + reviewerID.EmailID + ";" + hrID.EmailID + ";" + cordinatorID.EmailID + ";";
                            }
                            else
                                model.MailDetail.Cc = loginuser.EmailID + ";" + reviewerID.EmailID + ";" + hrID.EmailID + ";" + cordinatorID.EmailID + ";";
                            templateId = 13;
                            template = Commondal.GetEmailTemplate(templateId);
                            foreach (var emailTemplate in template)
                            {
                                subject = emailTemplate.Subject;
                                mailbody = emailTemplate.Message;
                            }
                            subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                            model.MailDetail.Subject = subject;
                            mailbody = mailbody.Replace("##manager name##", Server.HtmlEncode(loginuser.EmployeeName));
                            model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                            ViewBag.body = mailbody;
                        }
                        else if (confirmationDetails.stageID == 4)
                        {
                            if (IsApproveOrReject == "Approved") // employee approve
                            {
                                // if there is second mngr in the process stage id wont get changed and mail will be send to reviewer
                                if (confirmationDetails.ReportingManager2 == loginuser.EmployeeID || confirmationDetails.ReportingManager == loginuser.EmployeeID)
                                {
                                    model.MailDetail.To = reviewerID.EmailID;
                                    if (managerID2 != null)
                                    {
                                        if (managerID2.EmployeeID != loginuser.EmployeeID)
                                            model.MailDetail.Cc = loginuser.EmailID + ";" + managerID2.EmailID + ";" + hrID.EmailID + ";" + cordinatorID.EmailID + ";";
                                        else
                                            model.MailDetail.Cc = loginuser.EmailID + ";" + managerID.EmailID + ";" + hrID.EmailID + ";" + cordinatorID.EmailID + ";";
                                    }
                                    else
                                        model.MailDetail.Cc = loginuser.EmailID + ";" + reviewerID.EmailID + ";" + hrID.EmailID + ";" + cordinatorID.EmailID + ";";
                                    templateId = 14;
                                    template = Commondal.GetEmailTemplate(templateId);
                                    foreach (var emailTemplate in template)
                                    {
                                        subject = emailTemplate.Subject;
                                        mailbody = emailTemplate.Message;
                                    }
                                    subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                                    model.MailDetail.Subject = subject;
                                    mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                                    mailbody = mailbody.Replace("##Confirmation coordinator##", cordinatorID.EmployeeName);
                                    mailbody = mailbody.Replace("##probation review date##", employeeDetails.Probation_Review_Date.Value.Date.ToString("d"));
                                    mailbody = mailbody.Replace("##manager name##", Server.HtmlEncode(loginuser.EmployeeName));
                                    model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                                    ViewBag.body = mailbody;
                                }
                                else  // employee approval mail
                                {
                                    model.MailDetail.To = managerID.EmailID;
                                    if (managerID2 != null)
                                        model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + managerID2.EmailID + ";" + reviewerID.EmailID + ";" + hrID.EmailID + ";" + cordinatorID.EmailID + ";";
                                    else
                                        model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + reviewerID.EmailID + ";" + hrID.EmailID + ";" + cordinatorID.EmailID + ";";
                                    templateId = 12;
                                    template = Commondal.GetEmailTemplate(templateId);
                                    foreach (var emailTemplate in template)
                                    {
                                        subject = emailTemplate.Subject;
                                        mailbody = emailTemplate.Message;
                                    }
                                    subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                                    model.MailDetail.Subject = subject;
                                    mailbody = mailbody.Replace("##employee name##", Server.HtmlEncode(loginuser.EmployeeName));
                                    model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                                    ViewBag.body = mailbody;
                                }
                            }
                            else
                            {
                                // reviewer reject

                                if (managerID2 != null)
                                {
                                    model.MailDetail.To = managerID.EmailID + ";" + managerID2.EmailID;
                                    model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + managerID2.EmailID + ";" + managerID.EmailID + ";" + hrID.EmailID + ";" + cordinatorID.EmailID + ";";
                                }
                                else
                                {
                                    model.MailDetail.To = managerID.EmailID;
                                    model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + managerID.EmailID + ";" + hrID.EmailID + ";" + cordinatorID.EmailID + ";";
                                }
                                templateId = 15;
                                template = Commondal.GetEmailTemplate(templateId);
                                foreach (var emailTemplate in template)
                                {
                                    subject = emailTemplate.Subject;
                                    mailbody = emailTemplate.Message;
                                }
                                subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                                model.MailDetail.Subject = subject;
                                mailbody = mailbody.Replace("##Reviewer name##", Server.HtmlEncode(loginuser.EmployeeName));
                                model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                                ViewBag.body = mailbody;
                            }
                        }
                        else if (confirmationDetails.stageID == 5) //manager approve
                        {
                            if (IsApproveOrReject == "Approved")
                            {
                                // if there is second mngr in the process stage id wont get changed and mail will be send to reviewer
                                if (confirmationDetails.Reviewer == loginuser.EmployeeID || confirmationDetails.HRReviewer == loginuser.EmployeeID)
                                {
                                    if (loginuser.EmployeeID == confirmationDetails.Reviewer)
                                        model.MailDetail.To = hrID.EmailID;
                                    if (loginuser.EmployeeID == confirmationDetails.HRReviewer)
                                        model.MailDetail.To = cordinatorID.EmailID;
                                    if (managerID2 != null)
                                        model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + managerID2.EmailID + ";" + managerID.EmailID + ";" + employeeDetails.EmailID + ";" + cordinatorID.EmailID + ";";
                                    else
                                        model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + managerID.EmailID + ";" + employeeDetails.EmailID + ";" + cordinatorID.EmailID + ";";
                                    templateId = 16;
                                    template = Commondal.GetEmailTemplate(templateId);
                                    foreach (var emailTemplate in template)
                                    {
                                        subject = emailTemplate.Subject;
                                        mailbody = emailTemplate.Message;
                                    }
                                    subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                                    model.MailDetail.Subject = subject;
                                    mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                                    mailbody = mailbody.Replace("##Reviewer name##", Server.HtmlEncode(loginuser.EmployeeName));
                                    model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                                    ViewBag.body = mailbody;
                                }
                                else
                                {
                                    model.MailDetail.Subject = employeeDetails.EmployeeName + " Confirmation form for approval";
                                    model.MailDetail.To = reviewerID.EmailID;
                                    if (managerID2 != null)
                                        model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + managerID2.EmailID + ";" + hrID.EmailID + ";" + managerID.EmailID + ";" + cordinatorID.EmailID + ";";
                                    else
                                        model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + hrID.EmailID + ";" + managerID.EmailID + ";" + cordinatorID.EmailID + ";";
                                    templateId = 14;
                                    template = Commondal.GetEmailTemplate(templateId);
                                    foreach (var emailTemplate in template)
                                    {
                                        subject = emailTemplate.Subject;
                                        mailbody = emailTemplate.Message;
                                    }
                                    subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                                    model.MailDetail.Subject = subject;
                                    mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                                    mailbody = mailbody.Replace("##Confirmation coordinator##", cordinatorID.EmployeeName);
                                    mailbody = mailbody.Replace("##probation review date##", employeeDetails.Probation_Review_Date.Value.Date.ToString("d"));
                                    mailbody = mailbody.Replace("##manager name##", Server.HtmlEncode(loginuser.EmployeeName));
                                    model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                                    ViewBag.body = mailbody;
                                }
                            }
                        }
                        else if (confirmationDetails.stageID == 6)   //reviewer Or HRreviewer approves
                        {
                            if (IsApproveOrReject == "Approved")
                            {
                                model.MailDetail.To = cordinatorID.EmailID;
                                if (managerID2 != null)
                                    model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + managerID2.EmailID + ";" + managerID.EmailID + ";" + cordinatorID.EmailID + ";";
                                else
                                    model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + managerID.EmailID + ";" + cordinatorID.EmailID + ";";
                                templateId = 18;
                                template = Commondal.GetEmailTemplate(templateId);
                                foreach (var emailTemplate in template)
                                {
                                    subject = emailTemplate.Subject;
                                    mailbody = emailTemplate.Message;
                                }
                                subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                                model.MailDetail.Subject = subject;
                                mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                                mailbody = mailbody.Replace("##Reviewer name##", Server.HtmlEncode(loginuser.EmployeeName));
                                model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                                ViewBag.body = mailbody;
                            }
                            else
                            {
                                // hr reject
                                model.MailDetail.Subject = "Confirmation Process Initiated";
                                model.MailDetail.To = employeeDetails.EmailID;
                                model.MailDetail.Cc = reviewerID.EmailID + ";";
                            }
                        }
                        else if (confirmationDetails.stageID == 7)   //hr closure
                        {
                            model.MailDetail.To = employeeDetails.EmailID;
                            //model.MailDetail.From = hrID.EmailID;
                            if (managerID2 != null)
                                model.MailDetail.Cc = loginuser.EmailID + ";" + managerID2.EmailID + ";" + reviewerID.EmailID + ";" + hrID.EmailID + ";" + managerID.EmailID + ";" + cordinatorID.EmailID + ";";
                            else
                                model.MailDetail.Cc = loginuser.EmailID + ";" + reviewerID.EmailID + ";" + hrID.EmailID + ";" + managerID.EmailID + ";" + cordinatorID.EmailID + ";";
                            string[] Loginroles = { "HR Admin", "RMG" };
                            foreach (string r in Loginroles)
                            {
                                string[] users = Roles.GetUsersInRole(r);
                                foreach (string userR in users)
                                {
                                    HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
                                    if (employee == null)
                                        model.MailDetail.Cc = model.MailDetail.Cc + string.Empty;
                                    else
                                        model.MailDetail.Cc = model.MailDetail.Cc + employee.EmailID + ";";
                                }
                            }
                            if (IsApproveOrReject == "Approved" && IsAcceptExtendPIP == "accept")
                            {
                                templateId = 19;
                                template = Commondal.GetEmailTemplate(templateId);
                                foreach (var emailTemplate in template)
                                {
                                    model.MailDetail.Subject = emailTemplate.Subject;
                                    mailbody = emailTemplate.Message;
                                }
                                mailbody = mailbody.Replace("##Confirmation coordinator##", Server.HtmlEncode(loginuser.EmployeeName));
                                model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                                ViewBag.body = mailbody;
                            }
                        }
                        else
                        {
                            //Confirmation Initiated Mail
                            model.MailDetail.To = employeeDetails.EmailID;
                            templateId = 11;
                            template = Commondal.GetEmailTemplate(templateId);
                            foreach (var emailTemplate in template)
                            {
                                model.MailDetail.Subject = emailTemplate.Subject;
                                mailbody = emailTemplate.Message;
                            }
                            mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                            mailbody = mailbody.Replace("##current date##", formFillDate.Date.ToString("d"));
                            mailbody = mailbody.Replace("##introspection date##", mangerDate.Date.ToString("d"));
                            mailbody = mailbody.Replace("##manager date##", reviewrDate.Date.ToString("d"));
                            mailbody = mailbody.Replace("##HR Admin##", Server.HtmlEncode(loginuser.EmployeeName));
                            model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                            ViewBag.body = mailbody;
                            if (managerID2 != null)
                            {
                                model.MailDetail.Cc = managerID2.EmailID + ";" + loginuser.EmailID + ";" + managerID.EmailID + ";" + reviewerID.EmailID + ";" + hrID.EmailID + ";" + cordinatorID.EmailID + ";";
                            }
                            else
                            {
                                model.MailDetail.Cc = loginuser.EmailID + ";" + managerID.EmailID + ";" + reviewerID.EmailID + ";" + hrID.EmailID + ";" + cordinatorID.EmailID + ";";
                            }
                        }
                    }
                }
                return PartialView("_MailTemplate", model.MailDetail);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        public ActionResult SendEmail(MailTemplateViewModel model)
        {
            try
            {
                bool result = false;
                EmployeeDAL employeeDAL = new EmployeeDAL();
                char[] symbols = new char[] { ';', ' ', ',' };
                int CcCounter = 0;
                int ToCounter = 0;
                if (model.Cc != null)
                {
                    string CcMailIds = model.Cc.TrimEnd(symbols);
                    model.Cc = CcMailIds;
                    char[] delimiters = new char[] { '\r', '\n', ';', ' ', ',' };
                    string[] EmailIds = CcMailIds.Split(delimiters,
                                     StringSplitOptions.RemoveEmptyEntries);
                    //string[] EmailIds = CcMailIds.Split(symbols);
                    string[] EmailId = EmailIds.Where(s => !String.IsNullOrEmpty(s)).ToArray();

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
                    result = true;
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

        /// <summary>
        /// Send mail to all the recipent specifiec in Cc,To,Form.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool SendMail(MailTemplateViewModel model)
        {
            try
            {
                SMTPHelper smtpHelper = new SMTPHelper();
                char[] symbols = new char[] { ';', ' ', ',', '\r', '\n' };
                if (model != null)
                {
                    string[] ToEmailId = model.To.Split(symbols);

                    //Loop to seperate email id's of CC peoples
                    string[] CCEmailIds = null;
                    if (model.Cc != "" && model.Cc != null)
                    {
                        CCEmailIds = model.Cc.Split(symbols);
                        string[] CCEmailId = CCEmailIds.Where(s => !String.IsNullOrEmpty(s)).ToArray();
                        return smtpHelper.SendMail(ToEmailId, null, CCEmailId, null, null, null, model.From, null, model.Subject, model.Message, null, null);
                    }

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

        /// <summary>
        /// Confirmation form related data is retrived.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="viewDetailsBtn"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConfirmationDetails(string employeeId, string viewDetailsBtn = "no")
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("LogOn", "Account");  //replace the null with a viewModel as needed.
                }
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                ViewBag.confirmationEmployeeId = employeeId;
                AppraisalDAL dalApp = new AppraisalDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                ConfirmationDAL dal = new ConfirmationDAL();
                ConfirmationFormViewModel model = new ConfirmationFormViewModel();
                tbl_CF_Confirmation confirmationDetails = dal.getConfirmationId(Convert.ToInt32(decryptedEmployeeId));
                HRMS_tbl_PM_Employee managerDetails = new HRMS_tbl_PM_Employee();
                HRMS_tbl_PM_Employee manager2Details = new HRMS_tbl_PM_Employee();
                HRMS_tbl_PM_Employee reviewerDetails = new HRMS_tbl_PM_Employee();
                HRMS_tbl_PM_Employee hrDetails = new HRMS_tbl_PM_Employee();
                HRMS_tbl_PM_Employee cordinatorDetails = new HRMS_tbl_PM_Employee();
                managerDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.ReportingManager));
                manager2Details = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.ReportingManager2));
                reviewerDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.Reviewer));
                hrDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.HRReviewer));
                cordinatorDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.ConfirmationCoordinator));
                DateTime joiningdt = employeeDAL.GetEmployeeJoiningDate(Convert.ToInt32(Convert.ToInt32(decryptedEmployeeId)));
                ViewBag.JoiningDate = joiningdt;
                DateTime Dateonly = joiningdt.Date;
                ViewBag.JoiningDate = Dateonly.ToString("MM/dd/yy");
                int? confirmationID = confirmationDetails.ConfirmationID;
                model.SearchedUserDetails = new SearchedUserDetails();
                int loginemployeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                model.SearchedUserDetails.EmployeeId = loginemployeeID;
                model.ManagerName = managerDetails.EmployeeName.Trim();
                if (manager2Details != null)
                    model.ManagerNameSecond = manager2Details.EmployeeName.Trim();
                else
                    model.ManagerNameSecond = "noManager2";

                if (reviewerDetails != null)
                    model.ReviewerName = reviewerDetails.EmployeeName.Trim();
                if (hrDetails != null)
                    model.HRReviewerName = hrDetails.EmployeeName.Trim();

                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                model.ViewButtonClicked = viewDetailsBtn;   // added for view details popUp click button
                if (confirmationDetails != null)
                    model.confirmationID = confirmationDetails.ConfirmationID;
                else
                    model.confirmationID = 0;
                if (confirmationDetails.ReportingManager2 == loginuser.EmployeeID)
                {
                    ViewBag.IsManagerOrEMployee = "Manager2";
                    model.IsManagerOrEmployee = "Manager2";
                }
                else if (confirmationDetails.ReportingManager == loginuser.EmployeeID)
                {
                    ViewBag.IsManagerOrEMployee = "Manager";
                    model.IsManagerOrEmployee = "Manager";
                }
                else if (confirmationDetails.Reviewer == loginuser.EmployeeID)
                {
                    ViewBag.IsManagerOrEMployee = "Reviewer";
                    model.IsManagerOrEmployee = "Reviewer";
                }
                else if (confirmationDetails.HRReviewer == loginuser.EmployeeID)
                {
                    ViewBag.IsManagerOrEMployee = "HR";
                    model.IsManagerOrEmployee = "HR";
                }
                else if (confirmationDetails.ConfirmationCoordinator == loginuser.EmployeeID)
                {
                    ViewBag.IsManagerOrEMployee = "HR";
                    model.IsManagerOrEmployee = "HR";
                }
                else
                {
                    ViewBag.IsManagerOrEMployee = "Employee";
                    model.IsManagerOrEmployee = "Employee";
                }
                model.rating = new RatingMinMax();
                model.rating = dal.GetRating();
                ViewBag.minRating = model.rating.min;
                ViewBag.maxRating = model.rating.max;
                List<int> ratingList = new List<int>();
                for (int i = model.rating.min; i <= model.rating.max; i++)
                {
                    ratingList.Add(i);
                }
                ViewBag.sectionTwoRatingList = ratingList;

                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                model.SearchedUserDetails.UserRole = user;
                ViewBag.UserRole = user;
                model.UserRole = user;
                model.EmployeeID = Convert.ToInt32(decryptedEmployeeId);
                model.CorporateEmployeeID = Convert.ToInt32(decryptedEmployeeId);
                int? employeeID = Convert.ToInt32(decryptedEmployeeId);
                model.MailDetail = new MailTemplateViewModel();
                model.MailDetail.Message = "";

                tbl_CF_PerformanceHinders perfHinderTable = dal.GetPerformanceHinderTable(employeeID, confirmationID);

                if (perfHinderTable != null)
                {
                    model.PerfHinderListTable = new PerformanceHinderTable()
                    {
                        perfHinderID = perfHinderTable.PerformanceHinderID,
                        empID = perfHinderTable.EmployeeID,
                        confID = perfHinderTable.ConfirmationID,
                        EmpCommentsFFEnvi = perfHinderTable.EmployeeCommentsFFEnvi,
                        EmpCommentsFFSelf = perfHinderTable.EmployeeCommentsFFSelf,
                        EmpCommentsIFEnvi = perfHinderTable.EmployeeCommentsIFEnvi,
                        EmpCommentsIFSelf = perfHinderTable.EmployeeCommentsIFSelf,
                        EmpCommentsSupport = perfHinderTable.EmployeeCommentsSupport,
                        MngrCommentsFFEnvi = perfHinderTable.ManagerCommentsFFEnvi,
                        MngrCommentsIFEnvi = perfHinderTable.ManagerCommentsIFEnvi,
                        MngrCommentsIFSelf = perfHinderTable.ManagerCommentsIFSelf,
                        MngrCommentsFFSelf = perfHinderTable.ManagerCommentsFFSelf,
                        MngrCommentsSupport = perfHinderTable.ManagerCommentsSupport,
                        MngrCommentsFFEnviSecond = perfHinderTable.ManagerCommentsFFEnviSecond,
                        MngrCommentsIFEnviSecond = perfHinderTable.ManagerCommentsIFEnviSecond,
                        MngrCommentsIFSelfSecond = perfHinderTable.ManagerCommentsIFSelfSecond,
                        MngrCommentsFFSelfSecond = perfHinderTable.ManagerCommentsFFSelfSecond,
                        MngrCommentsSupportSecond = perfHinderTable.ManagerCommentsSupportSecond,
                        ReviewerCommentsFFEnvi = perfHinderTable.ReviewerCommentsFFEnvi,
                        ReviewerCommentsFFSelf = perfHinderTable.ReviewerCommentsFFSelf,
                        ReviewerCommentsIFEnvi = perfHinderTable.ReviewerCommentsIFEnvi,
                        ReviewerCommentsIFSelf = perfHinderTable.ReviewerCommentsIFSelf,
                        ReviewerCommentsSupport = perfHinderTable.ReviewerCommentsSupport,
                        HrCommentsFFEnvi = perfHinderTable.HrCommentsFFEnvi,
                        HrCommentsFFSelf = perfHinderTable.HrCommentsFFSelf,
                        HrCommentsIFEnvi = perfHinderTable.HrCommentsIFEnvi,
                        HrCommentsIFSelf = perfHinderTable.HrCommentsIFSelf,
                        HrCommentsSupport = perfHinderTable.HrCommentsSupport,
                        MgrName = model.ManagerName,
                        MgrNameSecond = model.ManagerNameSecond,
                        RevName = model.ReviewerName,
                        HRName = model.HRReviewerName,
                        IsManagerOrEmployee = model.IsManagerOrEmployee,
                    };
                }
                else
                {
                    model.PerfHinderListTable = new PerformanceHinderTable()
                    {
                        perfHinderID = 0,
                        confID = confirmationID,
                        IsManagerOrEmployee = model.IsManagerOrEmployee,
                        MgrName = model.ManagerName,
                        MgrNameSecond = model.ManagerNameSecond,
                        RevName = model.ReviewerName,
                        HRName = model.HRReviewerName,
                        empID = employeeID
                    };
                }
                model.projAchievement = new ProjectAchievement();
                model.projAchievement.EmpID = employeeID;
                model.projAchievement.ConfirmationID = confirmationID;
                model.skillAquired = new SkillsAquired();
                model.skillAquired.SkillEmployeeID = employeeID;
                model.skillAquired.ConfirmationID = confirmationID;
                model.additionalQualification = new AdditionalQualification();
                model.additionalQualification.QualifEmployeeID = Convert.ToInt32(decryptedEmployeeId);
                model.additionalQualification.ConfirmationID = confirmationID;
                LoadQualificationDropDown(model.additionalQualification);
                List<tbl_CF_ValueDrivers> paramList = dal.GetParameters(Convert.ToInt32(decryptedEmployeeId), confirmationID);
                model.confParameterList = new List<ConfirmationParameter>();
                ConfirmationParameter confParam;
                if (paramList == null)
                {
                    confParam = new ConfirmationParameter();
                    confParam.confirmationID = Convert.ToInt32(confirmationID);
                    confParam.employeeID = Convert.ToInt32(employeeID);
                    confParam.IsManagerOrEmployee = model.IsManagerOrEmployee;
                    confParam.MgrName = model.ManagerName.Trim();
                    confParam.MgrNameSecond = model.ManagerNameSecond.Trim();
                    confParam.RevName = model.ReviewerName.Trim();
                    confParam.HRName = model.HRReviewerName.Trim();
                    model.confParameterList.Add(confParam);
                }
                else
                {
                    foreach (var data in paramList)
                    {
                        confParam = new ConfirmationParameter();
                        confParam.confirmationID = data.ConfirmationID;
                        confParam.competencyID = Convert.ToInt32(data.CompetencyID);
                        confParam.ParameterDescription = data.ParameterDescription;
                        confParam.SelfRating = data.SelfRating;
                        confParam.EmpComments = data.EmployeeComments;
                        confParam.employeeID = data.EmployeeID;
                        confParam.ManagerRating1 = data.ManagerRating1;
                        confParam.MngrComments1 = data.ManagerComments1;
                        confParam.ManagerRating2 = data.ManagerRating2;
                        confParam.MngrComments2 = data.ManagerComments2;
                        confParam.ReviewerComments = data.ReviewerComments;
                        confParam.ReviewerRating = data.ReviewerRating;
                        confParam.HRrRating = data.HRrRating;
                        confParam.HrComments = data.HRComments;
                        confParam.IsManagerOrEmployee = model.IsManagerOrEmployee;
                        confParam.MgrName = model.ManagerName;
                        confParam.MgrNameSecond = model.ManagerNameSecond;
                        if (data.OverallHRComments != null)
                            confParam.OverallReviewHRComments = data.OverallHRComments.Trim();
                        if (data.OverallHRReview != null)
                            confParam.OverallReviewHRRating = data.OverallHRReview;
                        if (data.OverallReviewRating != null)
                            confParam.OverallReviewRating = data.OverallReviewRating;
                        if (data.OverallReviewComments != null)
                            confParam.OverallReviewRatingComments = data.OverallReviewComments.Trim();

                        confParam.RevName = model.ReviewerName;
                        confParam.HRName = model.HRReviewerName;
                        model.confParameterList.Add(confParam);
                    }
                }
                tbl_CF_GoalAspire goalAspire = dal.GetGoalAspire(Convert.ToInt32(decryptedEmployeeId), confirmationID);
                if (goalAspire != null)
                {
                    model.goalAquire = new GoalAquire
                    {
                        EmployeID = goalAspire.EmployeeID,
                        ConfirmID = goalAspire.ConfirmationID,
                        LongTerm = goalAspire.LongTermGoal,
                        ShortTerm = goalAspire.ShortTermGoal,
                        SkillDevPrgm = goalAspire.SkillDevPrgm,
                    };
                }
                else
                {
                    model.goalAquire = new GoalAquire
                    {
                        EmployeID = employeeID,
                        ConfirmID = Convert.ToInt32(confirmationID),
                    };
                }
                tbl_CF_TempConfirmation tempConfirmation = dal.GetTempConfirmation(Convert.ToInt32(confirmationID));
                if (tempConfirmation != null)
                {
                    if (tempConfirmation.ConfirmationStatus == 4)
                    {
                        model.roleName = Convert.ToString(tempConfirmation.RoleID);
                        model.empStatus = Convert.ToString(tempConfirmation.EmployeeStatusID);
                        model.gradeName = Convert.ToString(tempConfirmation.GradeID);
                        //model.empType = Convert.ToString(tempConfirmation.EmployeeType);
                        model.ConfirmationComments = tempConfirmation.ConfirmationComments;
                        model.ConfirmationDate = Convert.ToDateTime(tempConfirmation.ConfirmationDate);
                        model.PIPDate = DateTime.Now.AddDays(1);
                        model.ExtendProbationDate = DateTime.Now.AddDays(1);
                    }
                    else if (tempConfirmation.ConfirmationStatus == 3)
                    {
                        model.ExtendProbationDate = Convert.ToDateTime(tempConfirmation.ExtendedProbationDate);
                        model.ProbationComments = tempConfirmation.ExtensionComments.Trim();
                        model.ConfirmationDate = DateTime.Now;
                        model.PIPDate = DateTime.Now.AddDays(1);
                    }
                    else if (tempConfirmation.ConfirmationStatus == 2)
                    {
                        model.ConfirmationDate = DateTime.Now;
                        model.ExtendProbationDate = DateTime.Now.AddDays(1);
                        model.PIPDate = Convert.ToDateTime(tempConfirmation.PIPDate);
                        model.PIPComments = tempConfirmation.PIPComments.Trim();
                    }
                }
                else
                {
                    model.ConfirmationDate = DateTime.Now;
                    model.ExtendProbationDate = DateTime.Now.AddDays(1);
                    model.PIPDate = DateTime.Now.AddDays(1);
                }

                model.Grade = dal.getGradeList();
                model.EmployeeStatus = dal.getEmployeeStatusList();
                model.EmployeeType = dal.getEmployeementType();
                model.Role = dal.getRoleList();
                ViewBag.Role = new SelectList(employeeDAL.GetEmployeeRole(), "RoleID", "RoleDescription");

                model.StageID = Convert.ToInt32(confirmationDetails.stageID);
                ViewBag.stageid = model.StageID;
                ViewBag.clickedViewDetails = model.ViewButtonClicked;
                ViewBag.hasManager2 = model.ManagerNameSecond;
                SemDAL semDal = new SemDAL();
                List<DateTime> HolidayDates = semDal.getHolidayDateList();
                ViewBag.Holidaydates = HolidayDates;
                return PartialView("ConfirmationForm", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        /// <summary>
        /// In section 2 of confirmation form,corporate contribution grid related data is retrived .
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CorporateDetailsLoadGrid(string employeeId, int confirmationID, int page, int rows)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                ConfirmationDAL dal = new ConfirmationDAL();
                ConfirmationFormViewModel objDependentDetailModel = new Models.ConfirmationFormViewModel();
                objDependentDetailModel.CorporateContributionList = new List<Models.ConfirmationFormViewModel>();
                int totalCount = 0;

                objDependentDetailModel.CorporateContributionList = dal.GetCorporateDetails(Convert.ToInt32(decryptedEmployeeId), confirmationID, page, rows, out totalCount);

                if ((objDependentDetailModel.CorporateContributionList == null || objDependentDetailModel.CorporateContributionList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    objDependentDetailModel.CorporateContributionList = dal.GetCorporateDetails(Convert.ToInt32(decryptedEmployeeId), confirmationID, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = objDependentDetailModel.CorporateContributionList,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        /// <summary>
        /// In section 1 of confirmation form,project achievement grid related data is retrived .
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProjectAchievementDetailsLoadGrid(string employeeId, int? confirmationID, int page, int rows)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                ConfirmationDAL dal = new ConfirmationDAL();
                EmployeeDAL EMPdal = new EmployeeDAL();
                ProjectAchievement objDependentDetailModel = new Models.ProjectAchievement();
                objDependentDetailModel.projectAchievementList = new List<Models.ProjectAchievement>();
                int totalCount = 0;
                objDependentDetailModel.projectAchievementList = dal.GetProjectAchievementDetails(Convert.ToInt32(decryptedEmployeeId), confirmationID, page, rows, out totalCount);
                if ((objDependentDetailModel.projectAchievementList == null || objDependentDetailModel.projectAchievementList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    objDependentDetailModel.projectAchievementList = dal.GetProjectAchievementDetails(Convert.ToInt32(decryptedEmployeeId), confirmationID, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = objDependentDetailModel.projectAchievementList,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        /// <summary>
        /// In section 4 of confirmation form,skills aquired grid related data is retrived .
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SkillAquiredDetailsLoadGrid(string employeeId, int ConfirmID, int page, int rows)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                ConfirmationDAL dal = new ConfirmationDAL();
                SkillsAquired objSkillAquiredDetailModel = new Models.SkillsAquired();
                objSkillAquiredDetailModel.skillsAquiredList = new List<Models.SkillsAquired>();
                int totalCount = 0;

                objSkillAquiredDetailModel.skillsAquiredList = dal.GetSkillAquiredDetails(Convert.ToInt32(decryptedEmployeeId), ConfirmID, page, rows, out totalCount);

                if ((objSkillAquiredDetailModel.skillsAquiredList == null || objSkillAquiredDetailModel.skillsAquiredList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    objSkillAquiredDetailModel.skillsAquiredList = dal.GetSkillAquiredDetails(Convert.ToInt32(decryptedEmployeeId), ConfirmID, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = objSkillAquiredDetailModel.skillsAquiredList,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        /// <summary>
        /// In section 5 of confirmation form,additional qualification grid related data is retrived .
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddQualificationDetailsLoadGrid(string employeeId, int ConfirmationID, int page, int rows)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                ConfirmationDAL dal = new ConfirmationDAL();
                AdditionalQualification objAddQualificationDetailModel = new Models.AdditionalQualification();
                objAddQualificationDetailModel.additionalQualificationList = new List<Models.AdditionalQualification>();
                int totalCount = 0;

                objAddQualificationDetailModel.additionalQualificationList = dal.GetAddQualificationDetails(Convert.ToInt32(decryptedEmployeeId), ConfirmationID, page, rows, out totalCount);

                if ((objAddQualificationDetailModel.additionalQualificationList == null || objAddQualificationDetailModel.additionalQualificationList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    objAddQualificationDetailModel.additionalQualificationList = dal.GetAddQualificationDetails(Convert.ToInt32(decryptedEmployeeId), ConfirmationID, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = objAddQualificationDetailModel.additionalQualificationList,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        /// <summary>
        /// Save corporate contribution details record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveCorporateInfo(ConfirmationFormViewModel model)
        {
            try
            {
                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.SaveCorporateDetails(model);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Save project achievement details record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveProjectAchievementInfo(ProjectAchievement model)
        {
            try
            {
                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.SaveProjectAchievementDetails(model);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Save additional qualification details record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveAddQualificationDetails(AdditionalQualification model)
        {
            try
            {
                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.SaveAddQualificationDetails(model);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Save skill aquired details record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveSkillAquiredInfo(SkillsAquired model)
        {
            try
            {
                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.SaveSkillAquiredDetails(model);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Save performance hinder details record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SavePerformanceHinderInfo(PerformanceHinderTable model)
        {
            try
            {
                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.SavePerformanceHinderDetails(model);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Save value driver details record.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveValueDriverInfo(List<ConfirmationParameter> item)
        {
            try
            {
                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.SaveParameterDetails(item);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Save goal aspire details record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveGoalAspire(GoalAquire model)
        {
            try
            {
                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.SaveGoalAspireDetails(model);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Save Hr Closure related details.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveHrConfirmation(ConfirmationFormViewModel model)
        {
            try
            {
                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.saveHrConfiramtionDetails(model);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Updated the stage and other employee details depending whether the user has approved or reject the confirmation form.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="approveReject"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveConfirmation(string employeeId, string approveReject, int confirmationID)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.ApproveConfirmation(Convert.ToInt32(decryptedEmployeeId), approveReject, confirmationID);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Delete corporate contribution record.
        /// </summary>
        /// <param name="CorporateID"></param>
        /// <returns></returns>
        public ActionResult DeleteCorporateDetails(int CorporateID)
        {
            try
            {
                ConfirmationDAL dal = new ConfirmationDAL();
                tbl_CF_CorporateContribution corporateDetails = new tbl_CF_CorporateContribution();
                bool eq = dal.DeletecorporateDetails(CorporateID);
                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Delete project achievement record.
        /// </summary>
        /// <param name="ProjectAchievementID"></param>
        /// <returns></returns>
        public ActionResult DeleteProjectAchievementDetails(int ProjectAchievementID)
        {
            try
            {
                ConfirmationDAL dal = new ConfirmationDAL();
                tbl_CF_ProjectAchievement projectAchievementDetails = new tbl_CF_ProjectAchievement();
                bool eq = dal.DeleteprojectAchievementDetails(ProjectAchievementID);
                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Delete skill aquired record.
        /// </summary>
        /// <param name="SkillAquiredID"></param>
        /// <returns></returns>
        public ActionResult DeleteSkillAquiredDetails(int SkillAquiredID)
        {
            try
            {
                ConfirmationDAL dal = new ConfirmationDAL();
                tbl_CF_SkillAquired skillAquiredDetails = new tbl_CF_SkillAquired();
                bool eq = dal.DeleteskillAquiredDetails(SkillAquiredID);
                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Delete additional qualification record.
        /// </summary>
        /// <param name="AddQualificationID"></param>
        /// <returns></returns>
        public ActionResult DeleteAddQualificationDetails(int AddQualificationID)
        {
            try
            {
                ConfirmationDAL dal = new ConfirmationDAL();
                tbl_CF_AdditionalQualification addQualificationDetails = new tbl_CF_AdditionalQualification();
                bool eq = dal.DeleteQualificationDetails(AddQualificationID);
                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets qualification type.
        /// </summary>
        /// <param name="model"></param>
        private void LoadQualificationDropDown(AdditionalQualification model)
        {
            QualificationDetailsDAL dal = new QualificationDetailsDAL();
            model.QualificationList = new List<AddQualificationListClass>();
            List<tbl_PM_QualificationType> qualifDetailsList = dal.GetQualificationTypeList();
            if (qualifDetailsList != null)
            {
                foreach (tbl_PM_QualificationType eachqualificationDetail in qualifDetailsList)
                {
                    model.QualificationList.Add(new AddQualificationListClass()
                    {
                        AddQualificationID = eachqualificationDetail.QualificationTypeID,
                        AddQualification = eachqualificationDetail.QualificationTypeName.Trim(),
                    });
                }
            }
        }

        /// <summary>
        /// Encryption of employeeID.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EmployeeEncryption(string employeeId)
        {
            try
            {
                string resultmsg = string.Empty;
                string empid = employeeId.ToString();
                resultmsg = Commondal.Encrypt(Session["SecurityKey"].ToString() + employeeId, true);
                return Json(new { result = resultmsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #region ConfirmationProcessChanges

        //New Confirmation Changes Started

        //Auto triggered mail
        public System.Threading.Timer myTimer;

        public void SetTimerValue()
        {
            // trigger the event at 9 AM. For 7 PM use 21 i.e. 24 hour format
            DateTime requiredTime = DateTime.Today.AddHours(12).AddMinutes(00);
            if (DateTime.Now > requiredTime)
            {
                requiredTime = requiredTime.AddDays(1);
            }
            // initialize timer only, do not specify the start time or the interval
            myTimer = new System.Threading.Timer(new TimerCallback(TimerAction));
            // first parameter is the start time and the second parameter is the interval
            // Timeout.Infinite means do not repeat the interval, only start the timer
            myTimer.Change((int)(requiredTime - DateTime.Now).TotalMilliseconds, Timeout.Infinite);
        }

        public void TimerAction(object e)
        {
            // do some work
            AutoSendMail();
            // now, call the set timer method to reset its next call time
            SetTimerValue();
        }

        public void AutoSendMail()
        {
            ConfirmationDAL dal = new ConfirmationDAL();

            //After Probation Review Date
            DataSet dsConfirmationDetails = dal.GetAutoTriggerMailDetailsForConfirmation();
            var values = new List<Tuple<string, string, string, string>>();
            var todaysDay = DateTime.Now.DayOfWeek;
            DateTime formFillingDate = new DateTime();
            if (todaysDay.ToString() == "Thursday")
                formFillingDate = DateTime.Now.AddDays(4);
            else if (todaysDay.ToString() == "Friday")
                formFillingDate = DateTime.Now.AddDays(3);
            else
                formFillingDate = DateTime.Now.AddDays(2);
            foreach (DataTable t in dsConfirmationDetails.Tables)
            {
                foreach (DataRow row in t.Rows)
                {
                    string ReportingToName = row["EmployeeName1"].ToString();
                    string EmployeeName = row["EmployeeName"].ToString();

                    string EmpEmailId = row["EmailId"].ToString();
                    string ManagerEmailId = row["EmailId1"].ToString();
                    values.Add(new Tuple<string, string, string, string>(EmployeeName, EmpEmailId, ReportingToName, ManagerEmailId));
                }
            }
            MailMessage mail = new MailMessage();

            string[] Loginroles = { "HR Admin" };
            foreach (string r in Loginroles)
            {
                string[] users = Roles.GetUsersInRole(r);
                foreach (string userR in users)
                {
                    HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(userR));
                    if (employee == null)
                        continue;
                    else
                        mail.CC.Add(employee.EmailID);
                }
            }
            for (int i = 0; i < values.Count; i++)
            {
                mail.CC.Add(values[i].Item2);
                mail.To.Add(values[i].Item4);
                //string RMGEmail = System.Configuration.ConfigurationManager.AppSettings["RMGEmailId"].ToString();
                //string Email = string.Empty;
                //Email = RMGEmail;
                //mail.CC.Add(RMGEmail);
                mail.From = new MailAddress(values[i].Item2, "HR Admin");

                TravelViewModel model = new TravelViewModel();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.Mail = new TravelMailTemplate();
                int templateId = 11;
                List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                foreach (var emailTemplate in template)
                {
                    model.Mail.Subject = emailTemplate.Subject.Replace("##employeename##", values[i].Item1);
                    model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                    model.Mail.Message = model.Mail.Message.Replace("##employeename##", values[i].Item1);
                    model.Mail.Message = model.Mail.Message.Replace("##reportingmanage##", values[i].Item3);
                    model.Mail.Message = model.Mail.Message.Replace("##date##", formFillingDate.ToShortDateString());
                    model.Mail.Message = model.Mail.Message.Replace("##logged in user##", "HR Admin");
                }
                mail.Subject = model.Mail.Subject;
                mail.Body = model.Mail.Message;
                mail.Priority = MailPriority.High;
                SmtpClient myMailClient = new SmtpClient();
                myMailClient.Host = System.Configuration.ConfigurationManager.AppSettings["SMTPServerName"].ToString();
                myMailClient.Port = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["PortNumber"].ToString());
                myMailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                myMailClient.Send(mail);
                // mail.Dispose();
            }

            //Befor Probation Review date
            DataSet dsConfirmationDetailsBeforProbation = dal.GetAutoTriggerMailDetailsForConfirmationBeforProbation();
            var valuesBeforeProbation = new List<Tuple<string, string, string, string, DateTime>>();
            foreach (DataTable t in dsConfirmationDetailsBeforProbation.Tables)
            {
                foreach (DataRow row in t.Rows)
                {
                    string ReportingToName = row["EmployeeName1"].ToString();
                    string EmployeeName = row["EmployeeName"].ToString();

                    string EmpEmailId = row["EmailId"].ToString();
                    string ManagerEmailId = row["EmailId1"].ToString();
                    DateTime ProbationDate = Convert.ToDateTime(row["probation_review_date"].ToString());
                    valuesBeforeProbation.Add(new Tuple<string, string, string, string, DateTime>(EmployeeName, EmpEmailId, ReportingToName, ManagerEmailId, ProbationDate));
                }
            }

            for (int i = 0; i < valuesBeforeProbation.Count; i++)
            {
                SmtpClient smtpClient = new SmtpClient();
                mail.CC.Add(valuesBeforeProbation[i].Item2);
                mail.To.Add(valuesBeforeProbation[i].Item4);
                mail.From = new MailAddress(valuesBeforeProbation[i].Item2, "HR Admin");

                TravelViewModel model = new TravelViewModel();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.Mail = new TravelMailTemplate();
                int templateId = 79;
                List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                foreach (var emailTemplate in template)
                {
                    model.Mail.Subject = emailTemplate.Subject.Replace("##employeename##", valuesBeforeProbation[i].Item1);
                    model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                    model.Mail.Message = model.Mail.Message.Replace("##employeename##", valuesBeforeProbation[i].Item1);
                    model.Mail.Message = model.Mail.Message.Replace("##reportingmanage##", valuesBeforeProbation[i].Item3);
                    model.Mail.Message = model.Mail.Message.Replace("##probationdate##", valuesBeforeProbation[i].Item5.ToShortDateString());
                    model.Mail.Message = model.Mail.Message.Replace("##logged in user##", "HR Admin");
                }
                mail.Subject = model.Mail.Subject;
                mail.Body = model.Mail.Message;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.Host = System.Configuration.ConfigurationManager.AppSettings["SMTPServerName"].ToString();
                //smtpClient.Host = "v2mailserver.in.v2solutions.com";
                string UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"].ToString();
                string Password = System.Configuration.ConfigurationManager.AppSettings["Password"].ToString();
                smtpClient.Credentials = new System.Net.NetworkCredential(UserName, Password);
                smtpClient.Port = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["PortNumber"].ToString());
                smtpClient.Send(mail);
                // mail.Dispose();
            }
        }

        //Auto triggered mail end

        /// <summary>
        /// Retrives records store in the inbox of the user.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="Field"></param>
        /// <param name="FieldChild"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadGridConfirmationDetailsList(string term, string Field, string FieldChild, int page, int rows)
        {
            try
            {
                ConfirmationDetailsViewModel model = new ConfirmationDetailsViewModel();
                ConfirmationProcess confrmDal = new ConfirmationProcess();
                ConfirmationDAL dal = new ConfirmationDAL();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                confrmDal.ConfigurationDetailsList = new List<ConfigurationDetailsViewModel>();
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                int loginUserId = Convert.ToInt32(loginName);
                int totalCount;
                List<ConfirmationDetailsViewModel> searchResultInbox = new List<ConfirmationDetailsViewModel>();
                searchResultInbox = dal.LoadGridConfirmationDetailsList(term, Field, FieldChild, page, rows, loginUserId, out totalCount);
                if ((searchResultInbox == null || searchResultInbox.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    searchResultInbox = dal.LoadGridConfirmationDetailsList(term, Field, FieldChild, page, rows, loginUserId, out totalCount);
                }
                List<ConfirmationDetailsViewModel> finalSearchResultInbox = new List<ConfirmationDetailsViewModel>();
                foreach (var item in searchResultInbox)
                {
                    string inboxEmployeeId = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.EmployeeId), true);
                    item.encryptedEmployeeId = inboxEmployeeId;
                    finalSearchResultInbox.Add(item);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = finalSearchResultInbox,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        /// <summary>
        /// Retrive records store in the watchlist of the user.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="Field"></param>
        /// <param name="FieldChild"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>

        [HttpGet]
        [PageAccess(PageName = "Confirmation")]
        public ActionResult ConfirmationDetailList()
        {
            try
            {
                Session["SearchEmpFullName"] = null;  // to hide emp search
                Session["SearchEmpCode"] = null;
                Session["SearchEmpID"] = null;
                ConfirmationDetailsViewModel model = new ConfirmationDetailsViewModel();
                ConfirmationDAL dal = new ConfirmationDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                model.SearchedUserDetails.UserRole = user;
                //model.hiddenid = null;
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                model.SearchedUserDetails.EmployeeId = employeeID;
                string userRole = user;
                model.FieldchildList = new List<FieldChildList>();
                ViewBag.FieldChildListBG = new SelectList(GetFieldChildDetailsList("Business Group"), "Id", "Description");
                ViewBag.FieldChildListOU = new SelectList(GetFieldChildDetailsList("Organization Unit"), "Id", "Description");
                ViewBag.FieldChildListSN = new SelectList(GetFieldChildDetailsList("Stage Name"), "Id", "Description");
                return PartialView("InboxWatchlistGrid", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        // Added By Mahesh Issue Id:25339 and 25049

        [HttpPost]
        public ActionResult AddWorkingDays(string empcode, DateTime extendedDate, bool isPIPDate)
        {
            /* modified By Suraj For Extended Probation Date Calculation*/
            ConfirmationDAL dal = new ConfirmationDAL();
            int EmpId = int.Parse(empcode);
            ////DateTime start = DateTime.Now;
            SemDAL semDal = new SemDAL();
            EmployeeDAL EmpDal = new EmployeeDAL();
            tbl_CF_Confirmation confirmationDetails = dal.getConfirmationId(Convert.ToInt32(EmpId));
            HRMS_tbl_PM_Employee Empdetails = new HRMS_tbl_PM_Employee();
            HRMSDBEntities dbContext = new HRMSDBEntities();

            var Empcode = (from resource in dbContext.HRMS_tbl_PM_Employee
                           where resource.EmployeeID == EmpId
                           select resource.Probation_Review_Date).FirstOrDefault();
            DateTime oldDate = new DateTime();
            if (oldDate == DateTime.Parse("01/01/0001"))
            {
                //oldDate = DateTime.Now;
                if (confirmationDetails.ExtendedProbationDate == null || confirmationDetails.PIPDate == null)
                {
                    oldDate = Convert.ToDateTime(Empcode);
                }
                else
                {
                    oldDate = Convert.ToDateTime(isPIPDate ? confirmationDetails.PIPDate : confirmationDetails.ExtendedProbationDate);
                }
            }
            //DateTime oldDate = DateTime.Now;
            List<DateTime> holidays = semDal.getHolidayDateList();//get holidays count from sp

            ViewBag.Holidaydates = holidays;

            TimeSpan interval = extendedDate.Date - oldDate.Date;//counting difference between date
            int totalWeek = interval.Days / 7;//counting weeks in between dates
            int totalWorkingDays = 7 * totalWeek;
            int remainingDays = interval.Days % 7;
            for (int i = 0; i < remainingDays; i++)
            {
                DayOfWeek test = (DayOfWeek)(((int)oldDate.Date.DayOfWeek + i) % 7);
                totalWorkingDays++;
            }
            //foreach (var dt in holidays)
            //{
            //    if (dt >= oldDate.Date && dt <= extendedDate.Date)
            //    {
            //        totalWorkingDays--;
            //    }
            //}

            return Json(new { results = totalWorkingDays }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ConfirmationFormDetails(string employeeId, string viewDetailsBtn = "no")
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("LogOn", "Account");  //replace the null with a viewModel as needed.
                }
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                ViewBag.confirmationEmployeeId = employeeId;

                EmployeeDAL employeeDAL = new EmployeeDAL();
                ConfirmationDAL dal = new ConfirmationDAL();
                ConfirmationFormViewModel model = new ConfirmationFormViewModel();
                tbl_CF_Confirmation confirmationDetails = dal.getConfirmationId(Convert.ToInt32(decryptedEmployeeId));
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(decryptedEmployeeId));
                HRMS_tbl_PM_Employee managerDetails = new HRMS_tbl_PM_Employee();
                HRMS_tbl_PM_Role EmployeeRoleDetails = new HRMS_tbl_PM_Role();
                EmployeeRoleDetails = employeeDAL.GetEmployeeOrganizationRole(employeeDetails.PostID);
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                tbl_PM_ResourcePool_Managers furtherApproverId = dal.GetFurtherApproverName(Convert.ToInt32(employeeDetails.ResourcePoolID));
                if (furtherApproverId != null)
                {
                    //Older Code
                    tbl_PM_Employee_SEM furtherApproverName = employeeDAL.GetEmployeeDetailsForConfirmation(Convert.ToInt32(furtherApproverId.ManagerID));
                    //Modified By Mahesh For DU Stage As it Reffers Diffrent Tables
                    //HRMS_tbl_PM_Employee furtherApproverName = employeeDAL.GetEmployeeDetails(Convert.ToInt32(furtherApproverId.ManagerID));
                    model.FurtherApproverName = furtherApproverName.EmployeeName;
                    model.FurtherApproverId = Convert.ToInt32(furtherApproverId.ManagerID);
                }
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                if (employeeDetails != null)
                    managerDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(employeeDetails.ReportingTo));
                model.EmployeeCode = employeeDetails.EmployeeCode;
                model.EmployeeName = employeeDetails.EmployeeName;
                model.ProbationReviewDate = employeeDetails.Probation_Review_Date.Value.ToShortDateString();
                model.ReportingManagerName = managerDetails.EmployeeName;
                if (EmployeeRoleDetails != null)
                    model.EmpRole = EmployeeRoleDetails.RoleDescription;
                model.projAchievement = new ProjectAchievement();
                model.projAchievement.EmpID = Convert.ToInt32(decryptedEmployeeId);
                model.projAchievement.ConfirmationID = confirmationDetails.ConfirmationID;
                if (confirmationDetails.FurtherApproverId != null)
                    model.IsFurtherApproverNeeded = true;
                if (confirmationDetails.IsFurtherApprovalStageCleared == true)
                    model.IsFurtherApproverStageCleared = true;
                if (confirmationDetails != null)
                    model.confirmationID = confirmationDetails.ConfirmationID;
                else
                    model.confirmationID = 0;
                model.EmployeeIdConfirmation = Convert.ToInt32(decryptedEmployeeId);
                if (confirmationDetails.ReportingManager == loginuser.EmployeeID)
                {
                    ViewBag.IsManagerOrEMployee = "Manager";
                    model.IsManagerOrEmployee = "Manager";
                }
                else if (confirmationDetails.FurtherApproverId == loginuser.EmployeeID)
                    model.IsManagerOrEmployee = "FurtherApprover";
                else
                    model.IsManagerOrEmployee = "HR";
                model.PerfHinderListTable = new PerformanceHinderTable();
                model.confParameterList = new List<ConfirmationParameter>();

                tbl_cf_ConfirmationFormQuetionsComments quetionList = dal.GetQuetionList(Convert.ToInt32(decryptedEmployeeId), confirmationDetails.ConfirmationID);
                if (quetionList != null)
                {
                    model.QuestionId = quetionList.QuetionId;
                    model.AreaOfContributionComments = quetionList.AreaOfContributionComments;
                    model.TrainingProgramComments = quetionList.TrainingProgramComments;
                    model.BehaviourComments = quetionList.BehaviorComments;
                }

                tbl_cf_ApproverComments approverList = dal.GetApproverComments(Convert.ToInt32(decryptedEmployeeId), confirmationDetails.ConfirmationID);
                if (approverList != null)
                {
                    model.ApproverDetailId = approverList.Id;
                    model.ReportingManagerComments = approverList.ReportingManagerComments;
                    model.HRComments = approverList.HRComments;
                    model.DUManagerComments = approverList.AdditionalApproverComments;
                }

                List<tbl_CF_ValueDrivers> paramList = dal.GetParameters(Convert.ToInt32(decryptedEmployeeId), confirmationDetails.ConfirmationID);
                model.confParameterList = new List<ConfirmationParameter>();
                ConfirmationParameter confParam;
                if (paramList == null)
                {
                    confParam = new ConfirmationParameter();
                    confParam.confirmationID = Convert.ToInt32(confirmationDetails.ConfirmationID);
                    confParam.employeeID = Convert.ToInt32(decryptedEmployeeId);
                    confParam.IsManagerOrEmployee = model.IsManagerOrEmployee;
                    confParam.MgrName = model.ReportingManagerName.Trim();
                    model.confParameterList.Add(confParam);
                }
                else
                {
                    foreach (var data in paramList)
                    {
                        confParam = new ConfirmationParameter();

                        confParam.confirmationID = data.ConfirmationID;
                        confParam.competencyID = Convert.ToInt32(data.CompetencyID);
                        confParam.ParameterDescription = data.ParameterDescription;
                        confParam.employeeID = data.EmployeeID;
                        confParam.ManagerRating1 = data.ManagerRating1;
                        confParam.MngrComments1 = data.ManagerComments1;
                        confParam.HRrRating = data.HRrRating;
                        confParam.HrComments = data.HRComments;
                        confParam.IsManagerOrEmployee = model.IsManagerOrEmployee;
                        confParam.MgrName = model.ReportingManagerName;
                        if (data.OverallManagerComments != null)
                            confParam.OverallManagerRatingComments = data.OverallManagerComments.Trim();
                        if (data.OverallManagerRating != null)
                            confParam.OverallManagerRating = Convert.ToInt32(data.OverallManagerRating);
                        model.confParameterList.Add(confParam);
                    }
                }
                model.IsAcceptedOrExtended = "accept";
                tbl_CF_TempConfirmation tempConfirmation = dal.GetTempConfirmation(confirmationDetails.ConfirmationID);
                if (tempConfirmation != null)
                {
                    if (tempConfirmation.ConfirmationStatus == 4)
                    {
                        if (model.IsManagerOrEmployee != "Manager")
                        {
                            // model.roleName = Convert.ToString(tempConfirmation.RoleID);
                            model.empStatus = Convert.ToString(tempConfirmation.EmployeeStatusID);
                            //model.gradeName = Convert.ToString(tempConfirmation.GradeID);
                            //model.empType = Convert.ToString(tempConfirmation.EmployeeType);
                            if (tempConfirmation.ConfirmationComments != null)
                                model.ConfirmationComments = tempConfirmation.ConfirmationComments;
                            if (tempConfirmation.ConfirmationDate != null)
                                model.ConfirmationDate = Convert.ToDateTime(tempConfirmation.ConfirmationDate);
                            else
                                model.ConfirmationDate = DateTime.Now;
                            model.PIPDate = DateTime.Now.AddDays(1);
                            model.ExtendProbationDate = DateTime.Now.AddDays(1);
                        }
                        model.IsAcceptedOrExtended = "accept";
                    }
                    else if (tempConfirmation.ConfirmationStatus == 3)
                    {
                        if (model.IsManagerOrEmployee != "Manager")
                        {
                            if (tempConfirmation.ExtendedProbationDate == null)
                            {
                                model.ExtendProbationDate = DateTime.Parse(model.ProbationReviewDate);
                            }
                            if (tempConfirmation.ExtendedProbationDate != null)
                                model.ExtendProbationDate = Convert.ToDateTime(tempConfirmation.ExtendedProbationDate);
                            if (tempConfirmation.ExtensionComments != null)
                                model.ProbationComments = tempConfirmation.ExtensionComments.Trim();
                            model.ConfirmationDate = DateTime.Now;
                            model.PIPDate = DateTime.Now.AddDays(1);
                        }
                        model.IsAcceptedOrExtended = "extend";
                    }
                    else if (tempConfirmation.ConfirmationStatus == 2)
                    {
                        if (model.IsManagerOrEmployee != "Manager")
                        {
                            model.ConfirmationDate = DateTime.Now;
                            model.ExtendProbationDate = DateTime.Now.AddDays(1);
                            if (tempConfirmation.PIPDate == null)
                            {
                                model.PIPDate = DateTime.Parse(model.ProbationReviewDate);
                            }
                            if (tempConfirmation.PIPDate != null)
                                model.PIPDate = Convert.ToDateTime(tempConfirmation.PIPDate);
                            if (tempConfirmation.PIPComments != null)
                                model.PIPComments = tempConfirmation.PIPComments.Trim();
                        }
                        model.IsAcceptedOrExtended = "sendPIP";
                    }
                    model.NumberOfDaysProbation = tempConfirmation.NumberOfDaysExtension.HasValue ? tempConfirmation.NumberOfDaysExtension.Value : 0;
                    model.NumberOfDaysPIP = tempConfirmation.NumberOfDaysExtension.HasValue ? tempConfirmation.NumberOfDaysExtension.Value : 0;
                }
                else
                {
                    model.ConfirmationDate = DateTime.Now;
                    model.ExtendProbationDate = DateTime.Now.AddDays(1);
                    model.PIPDate = DateTime.Now.AddDays(1);
                }

                model.MailDetail = new MailTemplateViewModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = Convert.ToInt32(decryptedEmployeeId);

                model.GuideLinesList = new List<GuideLines>();
                model.GuideLinesList = dal.getGuileLines();

                model.Grade = dal.getGradeList();
                model.EmployeeStatus = dal.getEmployeeStatusList();
                model.EmployeeType = dal.getEmployeementType();
                model.Role = dal.getRoleList();
                ViewBag.Role = new SelectList(employeeDAL.GetEmployeeRole(), "RoleID", "RoleDescription");
                model.rating = new RatingMinMax();
                model.rating = dal.GetRating();
                ViewBag.minRating = model.rating.min;
                ViewBag.maxRating = model.rating.max;
                List<int> ratingList = new List<int>();
                for (int i = model.rating.min; i <= model.rating.max; i++)
                {
                    ratingList.Add(i);
                }
                ViewBag.sectionTwoRatingList = ratingList;
                model.StageID = Convert.ToInt32(confirmationDetails.stageID);
                ViewBag.stageid = model.StageID;
                model.ViewButtonClicked = viewDetailsBtn;
                //ViewBag.clickedViewDetails = model.ViewButtonClicked;
                ViewBag.clickedViewDetails = model.ViewButtonClicked;
                ViewBag.hasManager2 = model.ManagerNameSecond;
                return PartialView("ConfirmationForm", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        [HttpPost]
        public ActionResult LoadGridProjectDetailsConfirmation(string employeeId, int page, int rows)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                ConfirmationDAL dal = new ConfirmationDAL();
                EmployeeDAL EMPdal = new EmployeeDAL();
                ProjectAchievement projectDetails = new Models.ProjectAchievement();
                projectDetails.projectAchievementList = new List<Models.ProjectAchievement>();
                int totalCount = 0;
                projectDetails.projectAchievementList = dal.GetProjectDetailsConfirmation(Convert.ToInt32(decryptedEmployeeId), page, rows, out totalCount);
                if ((projectDetails.projectAchievementList == null || projectDetails.projectAchievementList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    projectDetails.projectAchievementList = dal.GetProjectDetailsConfirmation(Convert.ToInt32(decryptedEmployeeId), page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = projectDetails.projectAchievementList,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        /// <summary>
        /// Save value driver details record.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveValueDriverConfirmationInfo(List<ConfirmationParameter> item)
        {
            try
            {
                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.SaveParameterDetailsConfirmation(item);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Save goal aspire details record.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ///

        [HttpPost]
        public ActionResult SaveConfirmationFormQuetionsInfo(ConfirmationFormViewModel model)
        {
            try
            {
                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.SaveConfirmationFormQuetionsDetails(model);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveApproverComments(ConfirmationFormViewModel model)
        {
            try
            {
                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.SaveApproverCommentsDetails(model);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveHrClosureFormDetails(ConfirmationFormViewModel model)
        {
            try
            {
                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.saveHrClosureFormDetails(model);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Updated the stage and other employee details depending whether the user has approved or reject the confirmation form.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="approveReject"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveConfirmationFormDetails(string employeeId, int confirmationID, string IsManagerOrEmployee, string ReportingMangerComment, string HrComments)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.ApproveConfirmationFormDetails(Convert.ToInt32(decryptedEmployeeId), confirmationID, IsManagerOrEmployee, ReportingMangerComment, HrComments);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Delete corporate contribution record.
        /// </summary>
        /// <param name="CorporateID"></param>
        /// <returns></returns>
        ///

        [HttpPost]
        public ActionResult RejectConfirmation(string employeeId, int confirmationID, string IsManagerOrEmployee, string RejectComments)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                DAL.ConfirmationDAL dal = new DAL.ConfirmationDAL();
                string resultMessage = string.Empty;
                var status = dal.RejectConfirmationFormDetails(Convert.ToInt32(decryptedEmployeeId), confirmationID, IsManagerOrEmployee, RejectComments);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion ConfirmationProcessChanges

        [HttpGet]
        public ActionResult GetEmailTemplateNewComfirmation(string employeeId, string IsApproveOrReject = "", string IsAcceptExtendPIP = "", string IsManagerOrEmployee = "")
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                EmployeeDAL employeeDAL = new EmployeeDAL();
                ConfirmationDAL CnfDal = new ConfirmationDAL();
                DateTime formFillingDate = DateTime.Now.Date;
                var startDate = DateTime.Now.DayOfWeek;
                DateTime formFillDate, mangerDate, reviewrDate;

                tbl_CF_Confirmation confirmationDetail = CnfDal.getConfirmationDetails(Convert.ToInt32(decryptedEmployeeId));
                //string AppraisalCoorcinator = null;
                HRMS_tbl_PM_Employee empDetail = new HRMS_tbl_PM_Employee();
                if (confirmationDetail != null)
                    empDetail = employeeDAL.GetEmployeeDetails(confirmationDetail.ReportingManager.HasValue ? confirmationDetail.ReportingManager.Value : 0);

                if (startDate.ToString() == "Monday")
                {
                    formFillDate = DateTime.Now.AddDays(2);
                    mangerDate = DateTime.Now.AddDays(4);
                    reviewrDate = DateTime.Now.AddDays(7);
                }
                else
                {
                    if (startDate.ToString() == "Wednesday")
                    {
                        formFillDate = DateTime.Now.AddDays(2);
                        mangerDate = DateTime.Now.AddDays(6);
                        reviewrDate = DateTime.Now.AddDays(8);
                    }
                    else
                    {
                        if (startDate.ToString() == "Friday")
                        {
                            formFillDate = DateTime.Now.AddDays(3);
                            mangerDate = DateTime.Now.AddDays(5);
                            reviewrDate = DateTime.Now.AddDays(7);
                        }
                        else
                        {
                            if (startDate.ToString() == "Tuesday")
                            {
                                formFillDate = DateTime.Now.AddDays(2);
                                mangerDate = DateTime.Now.AddDays(6);
                                reviewrDate = DateTime.Now.AddDays(8);
                            }
                            else
                            {
                                if (startDate.ToString() == "Thursday")
                                {
                                    formFillDate = DateTime.Now.AddDays(4);
                                    mangerDate = DateTime.Now.AddDays(6);
                                    reviewrDate = DateTime.Now.AddDays(8);
                                }
                                else
                                {
                                    formFillDate = DateTime.Now.AddDays(2);
                                    mangerDate = DateTime.Now.AddDays(4);
                                    reviewrDate = DateTime.Now.AddDays(8);
                                }
                            }
                        }
                    }
                }

                InitiatConfirmationProcess model = new InitiatConfirmationProcess();
                model.MailDetail = new MailTemplateViewModel();
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(decryptedEmployeeId));
                if (employeeDetails != null)
                {
                    tbl_CF_Confirmation confirmationDetails = CnfDal.getConfirmationId(Convert.ToInt32(decryptedEmployeeId));
                    tbl_CF_TempConfirmation extendConfirmation = CnfDal.GetTempConfirmation(confirmationDetails.ConfirmationID);
                    if (confirmationDetails != null)
                    {
                        string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                        string loginUserId = loginName;
                        string mailbody = null;
                        string subject = null;
                        int templateId = 0;
                        List<EmployeeMailTemplate> template = new List<EmployeeMailTemplate>();
                        HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                        model.MailDetail.From = loginuser.EmailID;

                        string manager =
                            //HRMS_tbl_PM_Employee managerID = new HRMS_tbl_PM_Employee();
                            //HRMS_tbl_PM_Employee managerID2 = new HRMS_tbl_PM_Employee();
                            //HRMS_tbl_PM_Employee reviewerID = new HRMS_tbl_PM_Employee();
                            //List<HRMS_tbl_PM_Employee> hrID = new List<HRMS_tbl_PM_Employee>();
                            //HRMS_tbl_PM_Employee cordinatorID = new HRMS_tbl_PM_Employee();
                            //managerID = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.ReportingManager));
                            //managerID2 = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.ReportingManager2));
                            //reviewerID = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.Reviewer));

                        //cordinatorID = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.ConfirmationCoordinator));

                        model.MailDetail.From = loginuser.EmailID;
                        //if (confirmationDetails.ReportingManager2 != null)
                        //{
                        //    managerID2 = employeeDAL.GetEmployeeDetails(Convert.ToInt32(confirmationDetails.ReportingManager2));
                        //}
                        if (IsAcceptExtendPIP == "")
                            IsAcceptExtendPIP = "No";
                        if (confirmationDetails.stageID == 1 && confirmationDetails.IsFurtherApprovalStageCleared != true && IsApproveOrReject == "Approved") // manager accept
                        {
                            string[] HRAdmin = { };
                            HRAdmin = Roles.GetUsersInRole("HR Admin");
                            foreach (var item in HRAdmin)
                            {
                                HRMS_tbl_PM_Employee empdetails = employeeDAL.GetEmployeeDetailsByEmployeeCode(item);
                                if (empdetails != null)
                                    model.MailDetail.To = empdetails.EmailID + ";" + model.MailDetail.To;
                            }
                            model.MailDetail.Cc = loginuser.EmailID;

                            templateId = 80;
                            template = Commondal.GetEmailTemplate(templateId);
                            foreach (var emailTemplate in template)
                            {
                                subject = emailTemplate.Subject;
                                mailbody = emailTemplate.Message;
                            }
                            subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                            model.MailDetail.Subject = subject;
                            mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                            mailbody = mailbody.Replace("##logged in user##", loginuser.EmployeeName);
                            model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                            ViewBag.body = mailbody;
                        }
                        else if (confirmationDetails.stageID == 0 && confirmationDetail.IsFurtherApprovalStagePresent != true && IsApproveOrReject == "Reject")//HR reject
                        {
                            model.MailDetail.To = empDetail.EmailID;
                            //model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + hrID.EmailID + ";" + managerID.EmailID + ";";

                            Tbl_HR_ConfirmationStageEvent corporate = CnfDal.getStageDetails(confirmationDetails.ConfirmationID);
                            string comments = null;
                            if (corporate != null)
                                comments = corporate.Comments;
                            string[] HRAdmin = { };
                            HRAdmin = Roles.GetUsersInRole("HR Admin");
                            foreach (var item in HRAdmin)
                            {
                                HRMS_tbl_PM_Employee empdetails = employeeDAL.GetEmployeeDetailsByEmployeeCode(item);
                                if (empdetails != null)
                                    model.MailDetail.Cc = empdetails.EmailID + ";" + model.MailDetail.Cc;
                            }

                            model.MailDetail.Cc = loginuser.EmailID + ";" + model.MailDetail.Cc;

                            templateId = 85;
                            template = Commondal.GetEmailTemplate(templateId);
                            foreach (var emailTemplate in template)
                            {
                                subject = emailTemplate.Subject;
                                mailbody = emailTemplate.Message;
                            }
                            subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                            model.MailDetail.Subject = subject;

                            mailbody = mailbody.Replace("##reporting manager##", empDetail.EmployeeName);
                            mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                            mailbody = mailbody.Replace("##rejection comments##", comments);
                            mailbody = mailbody.Replace("##date: today + 1 day##", DateTime.Today.AddDays(1).ToShortDateString());
                            mailbody = mailbody.Replace("##logged in user##", loginuser.EmployeeName);
                            model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                            ViewBag.body = mailbody;
                        }
                        else if (confirmationDetails.stageID == 4 && IsApproveOrReject == "Approved" && IsAcceptExtendPIP == "accept")
                        {
                            model.MailDetail.To = empDetail.EmailID + ";" + employeeDetails.EmailID;
                            //model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + hrID.EmailID + ";" + managerID.EmailID + ";";

                            string[] HRAdmin = { };
                            HRAdmin = Roles.GetUsersInRole("HR Admin");
                            foreach (var item in HRAdmin)
                            {
                                HRMS_tbl_PM_Employee empdetails = employeeDAL.GetEmployeeDetailsByEmployeeCode(item);
                                if (empdetails != null)
                                    model.MailDetail.Cc = empdetails.EmailID + ";" + model.MailDetail.Cc;
                            }

                            model.MailDetail.Cc = loginuser.EmailID + ";" + model.MailDetail.Cc;

                            templateId = 81;
                            template = Commondal.GetEmailTemplate(templateId);
                            foreach (var emailTemplate in template)
                            {
                                subject = emailTemplate.Subject;
                                mailbody = emailTemplate.Message;
                            }
                            subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                            model.MailDetail.Subject = subject;
                            mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                            mailbody = mailbody.Replace("##logged in user##", loginuser.EmployeeName);
                            model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                            ViewBag.body = mailbody;
                        }
                        else if (confirmationDetails.stageID == 0 && IsApproveOrReject == "Approved" && IsAcceptExtendPIP == "extend")
                        {
                            model.MailDetail.To = empDetail.EmailID + ";" + employeeDetails.EmailID;
                            //model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + hrID.EmailID + ";" + managerID.EmailID + ";";

                            string[] HRAdmin = { };
                            HRAdmin = Roles.GetUsersInRole("HR Admin");
                            foreach (var item in HRAdmin)
                            {
                                HRMS_tbl_PM_Employee empdetails = employeeDAL.GetEmployeeDetailsByEmployeeCode(item);
                                if (empdetails != null)
                                    model.MailDetail.Cc = empdetails.EmailID + ";" + model.MailDetail.Cc;
                            }

                            model.MailDetail.Cc = loginuser.EmailID + ";" + model.MailDetail.Cc;

                            templateId = 82;
                            template = Commondal.GetEmailTemplate(templateId);
                            foreach (var emailTemplate in template)
                            {
                                subject = emailTemplate.Subject;
                                mailbody = emailTemplate.Message;
                            }
                            subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                            model.MailDetail.Subject = subject;
                            mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                            mailbody = mailbody.Replace("##new probation review date##", Convert.ToString(confirmationDetail.ExtendedProbationDate));
                            mailbody = mailbody.Replace("##logged in user##", loginuser.EmployeeName);
                            model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                            ViewBag.body = mailbody;
                        }
                        else if (confirmationDetails.stageID == 0 && IsApproveOrReject == "Approved" && IsAcceptExtendPIP == "sendPIP")
                        {
                            model.MailDetail.To = empDetail.EmailID + ";" + employeeDetails.EmailID;
                            //model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + hrID.EmailID + ";" + managerID.EmailID + ";";

                            string[] HRAdmin = { };
                            HRAdmin = Roles.GetUsersInRole("HR Admin");
                            foreach (var item in HRAdmin)
                            {
                                HRMS_tbl_PM_Employee empdetails = employeeDAL.GetEmployeeDetailsByEmployeeCode(item);
                                if (empdetails != null)
                                    model.MailDetail.Cc = empdetails.EmailID + ";" + model.MailDetail.Cc;
                            }

                            model.MailDetail.Cc = loginuser.EmailID + ";" + model.MailDetail.Cc;

                            templateId = 83;
                            template = Commondal.GetEmailTemplate(templateId);
                            foreach (var emailTemplate in template)
                            {
                                subject = emailTemplate.Subject;
                                mailbody = emailTemplate.Message;
                            }
                            subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                            model.MailDetail.Subject = subject;
                            mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                            mailbody = mailbody.Replace("##new probation review date##", Convert.ToString(confirmationDetail.ExtendedProbationDate));
                            mailbody = mailbody.Replace("##logged in user##", loginuser.EmployeeName);
                            model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                            ViewBag.body = mailbody;
                        }
                        else if (confirmationDetails.stageID == 2 && confirmationDetail.IsFurtherApprovalStagePresent == true && IsApproveOrReject == "Approved" && confirmationDetail.IsFurtherApprovalStageCleared != true)
                        {
                            HRMS_tbl_PM_Employee empDetails = new HRMS_tbl_PM_Employee();
                            if (confirmationDetail != null)
                                empDetails = employeeDAL.GetEmployeeDetails(confirmationDetail.FurtherApproverId.HasValue ? confirmationDetail.FurtherApproverId.Value : 0);

                            model.MailDetail.To = empDetails.EmailID;
                            //model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + hrID.EmailID + ";" + managerID.EmailID + ";";

                            string[] HRAdmin = { };
                            HRAdmin = Roles.GetUsersInRole("HR Admin");
                            foreach (var item in HRAdmin)
                            {
                                HRMS_tbl_PM_Employee empdetails = employeeDAL.GetEmployeeDetailsByEmployeeCode(item);
                                if (empdetails != null)
                                    model.MailDetail.Cc = empdetails.EmailID + ";" + model.MailDetail.Cc;
                            }

                            model.MailDetail.Cc = loginuser.EmailID + ";" + model.MailDetail.Cc + ";" + empDetail.EmailID;

                            templateId = 84;
                            template = Commondal.GetEmailTemplate(templateId);
                            foreach (var emailTemplate in template)
                            {
                                subject = emailTemplate.Subject;
                                mailbody = emailTemplate.Message;
                            }
                            subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                            model.MailDetail.Subject = subject;
                            mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                            mailbody = mailbody.Replace("##DU manager##", empDetails.EmployeeName);
                            mailbody = mailbody.Replace("##reporting manager name##", empDetail.EmployeeName);
                            mailbody = mailbody.Replace("##date: today + 2 days##", DateTime.Today.AddDays(2).ToShortDateString());
                            mailbody = mailbody.Replace("##logged in user##", loginuser.EmployeeName);
                            model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                            ViewBag.body = mailbody;
                        }
                        else if (confirmationDetails.stageID == 1 && confirmationDetail.IsFurtherApprovalStagePresent == true && IsApproveOrReject == "Approved" && confirmationDetail.IsFurtherApprovalStageCleared == true)
                        {
                            // Confirmation Process Re-submitted by Manager for employee
                            string[] HRAdmin = { };
                            HRAdmin = Roles.GetUsersInRole("HR Admin");
                            foreach (var item in HRAdmin)
                            {
                                HRMS_tbl_PM_Employee empdetails = employeeDAL.GetEmployeeDetailsByEmployeeCode(item);
                                if (empdetails != null)
                                    model.MailDetail.To = empdetails.EmailID + ";" + model.MailDetail.To;
                            }

                            model.MailDetail.Cc = loginuser.EmailID + ";" + model.MailDetail.Cc + ";" + empDetail.EmailID;

                            templateId = 87;
                            template = Commondal.GetEmailTemplate(templateId);
                            foreach (var emailTemplate in template)
                            {
                                subject = emailTemplate.Subject;
                                mailbody = emailTemplate.Message;
                            }
                            subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                            subject = subject.Replace("##button clicked by DU manager##", "Agree with Manager");
                            model.MailDetail.Subject = subject;
                            mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                            mailbody = mailbody.Replace("##button clicked by DU manager##", "Agree with Manager");
                            mailbody = mailbody.Replace("##agree with the manager / disagree with the manager##", "agree with Manager");
                            mailbody = mailbody.Replace("##logged in user##", loginuser.EmployeeName);
                            model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                            ViewBag.body = mailbody;
                        }
                        else if (confirmationDetails.stageID == 0 && confirmationDetail.IsFurtherApprovalStagePresent == true && IsApproveOrReject == "Reject" && confirmationDetail.IsFurtherApprovalStageCleared == true)
                        {//HR admin rejecting after further approval stage.
                            model.MailDetail.To = empDetail.EmailID;
                            //model.MailDetail.Cc = employeeDetails.EmailID + ";" + loginuser.EmailID + ";" + hrID.EmailID + ";" + managerID.EmailID + ";";

                            Tbl_HR_ConfirmationStageEvent corporate = CnfDal.getStageDetails(confirmationDetails.ConfirmationID);
                            string comments = null;
                            if (corporate != null)
                                comments = corporate.Comments;
                            string[] HRAdmin = { };
                            HRAdmin = Roles.GetUsersInRole("HR Admin");
                            foreach (var item in HRAdmin)
                            {
                                HRMS_tbl_PM_Employee empdetails = employeeDAL.GetEmployeeDetailsByEmployeeCode(item);
                                if (empdetails != null)
                                    model.MailDetail.Cc = empdetails.EmailID + ";" + model.MailDetail.Cc;
                            }

                            model.MailDetail.Cc = loginuser.EmailID + ";" + model.MailDetail.Cc;

                            templateId = 85;
                            template = Commondal.GetEmailTemplate(templateId);
                            foreach (var emailTemplate in template)
                            {
                                subject = emailTemplate.Subject;
                                mailbody = emailTemplate.Message;
                            }
                            subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                            model.MailDetail.Subject = subject;

                            mailbody = mailbody.Replace("##reporting manager##", empDetail.EmployeeName);
                            mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                            mailbody = mailbody.Replace("##rejection comments##", comments);
                            mailbody = mailbody.Replace("##date: today + 1 day##", DateTime.Today.AddDays(1).ToShortDateString());
                            mailbody = mailbody.Replace("##logged in user##", loginuser.EmployeeName);
                            model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                            ViewBag.body = mailbody;
                        }
                        else if (confirmationDetails.stageID == 1 && confirmationDetails.IsFurtherApprovalStageCleared == true && IsApproveOrReject == "Approved" && IsManagerOrEmployee == "Manager")
                        {
                            // manager accept
                            string[] HRAdmin = { };
                            HRAdmin = Roles.GetUsersInRole("HR Admin");
                            foreach (var item in HRAdmin)
                            {
                                HRMS_tbl_PM_Employee empdetails = employeeDAL.GetEmployeeDetailsByEmployeeCode(item);
                                if (empdetails != null)
                                    model.MailDetail.To = empdetails.EmailID + ";" + model.MailDetail.To;
                            }

                            model.MailDetail.Cc = loginuser.EmailID + ";" + model.MailDetail.Cc;

                            templateId = 87;
                            template = Commondal.GetEmailTemplate(templateId);
                            foreach (var emailTemplate in template)
                            {
                                subject = emailTemplate.Subject;
                                mailbody = emailTemplate.Message;
                            }
                            subject = subject.Replace("##employee name##", employeeDetails.EmployeeName);
                            model.MailDetail.Subject = subject;
                            mailbody = mailbody.Replace("##employee name##", employeeDetails.EmployeeName);
                            mailbody = mailbody.Replace("##logged in user##", loginuser.EmployeeName);
                            model.MailDetail.Message = mailbody.Replace("<br>", Environment.NewLine);
                            ViewBag.body = mailbody;
                        }
                    }
                }
                return PartialView("_MailTemplate", model.MailDetail);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        public List<FieldChildDetails> GetFieldChildDetailsList(string travelField)
        {
            try
            {
                ConfirmationDAL dal = new ConfirmationDAL();

                List<FieldChildDetails> childs = dal.GetFieldChildDetailsList(travelField);
                childs.ToList();
                return childs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrive the status related details of the user.
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ShowStatusDetailsConfirmation(string EmployeeId, string ConfirmationId, int page, int rows)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(EmployeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                ConfirmationDetailsViewModel model = new ConfirmationDetailsViewModel();
                ConfirmationProcess confrmDal = new ConfirmationProcess();
                ConfirmationDAL dal = new ConfirmationDAL();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                confrmDal.ConfigurationDetailsList = new List<ConfigurationDetailsViewModel>();
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                int loginUserId = Convert.ToInt32(loginName);

                int totalCount;
                List<ShowStatus> ShowStatusResult = new List<ShowStatus>();
                ShowStatusResult = dal.GetShowStatusResultConfirmation(decryptedEmployeeId, ConfirmationId, page, rows);
                totalCount = ShowStatusResult.Count();
                if ((ShowStatusResult == null || ShowStatusResult.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    ShowStatusResult = dal.GetShowStatusResultConfirmation(decryptedEmployeeId, ConfirmationId, page, rows);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = ShowStatusResult,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult ProjectEndAppraisalFormView(int employeeId, int? ProjectID, int ProjectEmployeeRoleID, int? ProjectEndAppraisalStatusID)
        {
            try
            {
                ProjectEndAppraisalFormModel model = new ProjectEndAppraisalFormModel();
                SemDAL dal = new SemDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                ConfigurationDAL configDAL = new ConfigurationDAL();
                ViewBag.AsciiKey = Session["SecurityKey"].ToString();

                HRMS_tbl_PM_Employee employeedetail = employeeDAL.GetEmployeeDetails(employeeId);
                string EmployeeCode = null;
                if (employeedetail != null)
                    EmployeeCode = employeedetail.EmployeeCode;

                string LoggedinUserEmployeeCode = Membership.GetUser().UserName;
                int LoggedinUserEmployeeId = dal.GetEmployeeID(LoggedinUserEmployeeCode);
                ViewBag.loginUserId = LoggedinUserEmployeeId;

                int EmployeeID = dal.GetEmployeeID(EmployeeCode);

                tbl_PM_Employee_SEM details = dal.GetEmployeeDetailsFromEmployeeID(EmployeeID);
                model.EmployeeName = details.EmployeeName;

                model.LoggedinUserEmployeeCode = LoggedinUserEmployeeCode;
                model.ProjectEmployeeRoleID = ProjectEmployeeRoleID;
                int proID = 0;
                tbl_PM_ProjectEmployeeRole EmpRoleDetails = dal.GetProjectEmployeeRoleAllocationDetails(ProjectEmployeeRoleID);
                if (EmpRoleDetails != null)
                {
                    model.AllocationStartDate = EmpRoleDetails.ExpectedStartDate;
                    if (EmpRoleDetails.ActualEndDate == null)
                    {
                        model.ReleaseDate = EmpRoleDetails.ExpectedEndDate;
                    }
                    else
                    {
                        model.ReleaseDate = EmpRoleDetails.ActualEndDate;
                    }
                    proID = EmpRoleDetails.ProjectID;
                }
                tbl_PM_Employee_SEM loggedinUserEmpDetails = dal.GetEmployeeDetailsFromEmployeeID(Convert.ToInt32(details.ReportingTo));
                model.ProjectManager = loggedinUserEmpDetails.EmployeeName;

                tbl_PM_Project projDetails = dal.GetProjectDetails(proID);
                if (projDetails != null)
                {
                    model.ProjectName = projDetails.ProjectName;
                }

                List<RatingScales> ratingScale = configDAL.GetRatingScales();
                model.RatingScale = ratingScale;
                if (EmpRoleDetails != null)
                    model.JoiningDate = EmpRoleDetails.ExpectedStartDate;
                for (int i = 0; i < model.RatingScale.Count; i++)
                {
                    model.RatingScale[i].Percentage = Convert.ToInt32(model.RatingScale[i].Percentage);
                }

                List<ProjectEndAppraisalParameters> parameterlist = dal.GetProjectEndAppraisalParameters(EmployeeID, ProjectID, ProjectEndAppraisalStatusID);
                if (parameterlist.Count > 0)
                {
                    model.ProjectEndAppraisalParameterList = parameterlist;
                }

                ParameterRating Rating = dal.GetMinMaxRating();
                ViewBag.minRating = Rating.MinValue;
                ViewBag.maxRating = Rating.MaxValue;
                List<int> ratingList = new List<int>();
                for (int i = Rating.MinValue; i <= Rating.MaxValue; i++)
                {
                    ratingList.Add(i);
                }
                model.RatingsList = ratingList;

                model.EmployeeID = EmployeeID;
                model.ProjectID = ProjectID;
                model.ProjectEndAppraisalFormStatus = ProjectEndAppraisalStatusID;
                foreach (var l in parameterlist)
                {
                    if (l.ProjectLead != null)
                    {
                        model.ProjectLead = l.ProjectLead;
                        break;
                    }
                }

                return PartialView("_ProjectEndAppraisalForm", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "Data is not available " });
            }
        }
    }
}