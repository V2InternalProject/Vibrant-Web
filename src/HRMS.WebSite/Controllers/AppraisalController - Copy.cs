//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using HRMS.DAL;
//using HRMS.Models;
//using System.Web.Security;
//using HRMS.Attributes;
//using HRMS.Helper;
//using System.IO;
//using HRMS.Models.Enums;
//using System.Data.OleDb;
//using System.Data;

//namespace HRMS.Controllers
//{
//    public class AppraisalController : Controller
//    {
//        //
//        // GET: /Appraisal/
//        CommonMethodsDAL Commondal = new CommonMethodsDAL();
//        AppraisalDAL dal = new AppraisalDAL();
//        [CustomAuthorize(Roles = "HR Admin,HR Executive,RMG")]
//        public ActionResult Index()
//        {
//            AppraisalViewModel model = new AppraisalViewModel();
//            if (!HttpContext.User.Identity.IsAuthenticated)
//            {
//                return RedirectToAction("LogOn", "Account");  //replace the null with a viewModel as needed.
//            }

//            EmployeeDAL employeeDAL = new EmployeeDAL();

//            model.SearchedUserDetails = new SearchedUserDetails();

//            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
//            string user = Commondal.GetMaxRoleForUser(role);

//            model.SearchedUserDetails.UserRole = user;

//            ViewBag.UserRole = user;

//            var empDetails = employeeDAL.GetUserDetailByEmployeeCode(Membership.GetUser().UserName);

//            var confirmationYear = empDetails.ConfirmationDate.Value.Year;
//            var confirmationMonth = empDetails.ConfirmationDate.Value.Month;

//            if (empDetails.ConfirmationDate > new DateTime(2013, 1, 1))
//            {
//                return PartialView("_AppraisalNotStarted");
//            }

//            model.EmployeeId = empDetails.EmployeeID;
//            return View(model);
//        }

//        [HttpGet]
//        public ActionResult SelfRating(int? employeeId)
//        {
//            return PartialView("_SelfRating");
//        }

//        public ActionResult ProjectAssignment(int employeeId)
//        {
//            AppraisalViewModel model = new AppraisalViewModel();
//            model.EmployeeId = employeeId;
//            return PartialView("_ProjectAssignment", model);
//        }

//        /// <summary>
//        /// Loads the grid view with the Project Assignment
//        /// </summary>
//        /// <param name="page"></param>
//        /// <param name="rows"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public ActionResult ProjectAssignmentLoadGrid(int page, int rows, int employeeId)
//        {
//            AppraisalDAL dal = new AppraisalDAL();
//            AppraisalViewModel model = new AppraisalViewModel();
//            employeeId = 1577;
//            int totalCount;
//            List<AppraisalViewModel> listAppraisal = dal.ProjectAssignmentLoadGrid(page, rows, employeeId, out totalCount);

//            if ((listAppraisal == null || listAppraisal.Count <= 0) && page - 1 > 0)
//            {
//                page = page - 1;

//                listAppraisal = dal.ProjectAssignmentLoadGrid(page, rows, employeeId, out totalCount);
//            }

//            var jsonData = new
//            {
//                total = (int)Math.Ceiling((double)totalCount / (double)rows),
//                page = page,
//                records = totalCount,
//                rows = listAppraisal
//            };

//            return Json(jsonData);
//        }

//        [HttpGet]
//        public ActionResult AppraisalProcessStatus(string TextLink)
//        {
//            try
//            {
//                Session["SearchEmpFullName"] = null;  // to hide emp search
//                Session["SearchEmpCode"] = null;
//                Session["SearchEmpID"] = null;

//                AppraisalProcessStatus model = new AppraisalProcessStatus();
//                EmployeeDAL DAL = new EmployeeDAL();

//                string EmployeeCode = Membership.GetUser().UserName;
//                string[] role = Roles.GetRolesForUser(EmployeeCode);
//                CommonMethodsDAL Commondal = new CommonMethodsDAL();
//                if (TextLink == null)
//                {
//                    TextLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "SelfAppraisal", true);
//                }
//                bool isAuthorizeTextLink;
//                string decryptedTextLink = HRMSHelper.Decrypt(TextLink, out isAuthorizeTextLink);
//                if (!isAuthorizeTextLink)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
//                if (decryptedTextLink == "AppraisalCoordinator" && !role.Contains("HR Admin"))
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
//                model.SearchedUserDetails = new SearchedUserDetails();
//                model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
//                ViewBag.LoggedInUserRole = model.SearchedUserDetails.UserRole;
//                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
//                model.SearchedUserDetails.EmployeeCode = EmployeeCode;
//                model.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
//                model.FieldChildList = new List<AppraisalFieldChildDetails>();
//                model.TextLink = decryptedTextLink;
//                ViewBag.EncryptedTextLink = TextLink;
//                model.Mail = new EmployeeMailTemplate();
//                ViewBag.EmployeeId = model.SearchedUserDetails.EmployeeId;
//                ViewBag.EncryptedEmployeeId = Commondal.Encrypt(this.Session["SecurityKey"].ToString() + Convert.ToString(model.SearchedUserDetails.EmployeeId), true);

//                int CountSelfAppraisal = 0, CountGroupHead = 0;
//                int totalCount;
//                List<ActionCount> Count = dal.GetCounts(Convert.ToInt32(model.SearchedUserDetails.EmployeeId));
//                int appraiserCount = dal.GetAsAnAppraiserCount(Convert.ToInt32(model.SearchedUserDetails.EmployeeId), out totalCount);
//                int reviewerCount = dal.GetAsAnReviewerCount(Convert.ToInt32(model.SearchedUserDetails.EmployeeId), out totalCount);
//                int currentAppraisalYearId = dal.GetCurrentYear();
//                string encryptedAppraisalYearId = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(currentAppraisalYearId), true);

//                foreach (var item in Count)
//                {
//                    if ((item.EmployeeId == model.SearchedUserDetails.EmployeeId) && (item.StageId == 0 || item.StageId == 5))
//                    {
//                        CountSelfAppraisal += 1;
//                    }
//                    if (item.GroupHead == model.SearchedUserDetails.EmployeeId && (item.StageId == 3 && item.AppraisalYearFrozenOn <= DateTime.Now))
//                    {
//                        CountGroupHead += 1;
//                    }
//                }
//                ViewBag.SelfAppraisal = CountSelfAppraisal;
//                ViewBag.Appraiser = appraiserCount;
//                ViewBag.Reviewer = reviewerCount;
//                ViewBag.GroupHead = CountGroupHead;
//                ViewBag.AppraisalCordinator = 0;
//                ViewBag.CurrentYear = encryptedAppraisalYearId;

//                ViewBag.EncryptedCoordinatorLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "AppraisalCoordinator", true);
//                ViewBag.EncryptedGroupHeadLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "GroupHead", true);
//                ViewBag.EncryptedReviewerLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "Reviewer", true);
//                ViewBag.EncryptedAppraiserLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "Appraiser", true);
//                ViewBag.EncryptedSelfAppraisalLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "SelfAppraisal", true);

//                ViewBag.FieldChildListBG = new SelectList(GetFieldChildDetailsList("Business Group"), "Id", "Description");
//                ViewBag.FieldChildListOU = new SelectList(GetFieldChildDetailsList("Organization Unit"), "Id", "Description");
//                ViewBag.FieldChildListSN = new SelectList(GetFieldChildDetailsList("Stage Name"), "Id", "Description");

//                return View("_AppraisalProcessStatus", model);
//            }
//            catch
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
//            }
//        }

//        [HttpGet]
//        public ActionResult AppraisalProcessIndex()
//        {
//            try
//            {
//                Session["SearchEmpFullName"] = null;  // to hide emp search
//                Session["SearchEmpCode"] = null;
//                AppraisalProcessIndexModel model = new AppraisalProcessIndexModel();
//                string EmployeeCode = Membership.GetUser().UserName;
//                string[] role = Roles.GetRolesForUser(EmployeeCode);
//                model.SearchedUserDetails = new SearchedUserDetails();
//                CommonMethodsDAL Commondal = new CommonMethodsDAL();
//                model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
//                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
//                model.SearchedUserDetails.EmployeeCode = EmployeeCode;
//                EmployeeDAL DAL = new EmployeeDAL();
//                model.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
//                int CountSelfAppraisal = 0, CountGroupHead = 0;
//                int totalCount;
//                List<ActionCount> Count = dal.GetCounts(Convert.ToInt32(model.SearchedUserDetails.EmployeeId));
//                int appraiserCount = dal.GetAsAnAppraiserCount(Convert.ToInt32(model.SearchedUserDetails.EmployeeId), out totalCount);
//                int reviewerCount = dal.GetAsAnReviewerCount(Convert.ToInt32(model.SearchedUserDetails.EmployeeId), out totalCount);
//                int currentAppraisalYearId = dal.GetCurrentYear();
//                string encryptedAppraisalYearId = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(currentAppraisalYearId), true);

//                foreach (var item in Count)
//                {
//                    if ((item.EmployeeId == model.SearchedUserDetails.EmployeeId) && (item.StageId == 0 || item.StageId == 5))
//                    {
//                        CountSelfAppraisal += 1;
//                    }
//                    if (item.GroupHead == model.SearchedUserDetails.EmployeeId && (item.StageId == 3 && item.AppraisalYearFrozenOn <= DateTime.Now))
//                    {
//                        CountGroupHead += 1;
//                    }
//                }
//                ViewBag.SelfAppraisal = CountSelfAppraisal;
//                ViewBag.Appraiser = appraiserCount;
//                ViewBag.Reviewer = reviewerCount;
//                ViewBag.GroupHead = CountGroupHead;
//                ViewBag.AppraisalCordinator = 0;
//                ViewBag.CurrentYear = encryptedAppraisalYearId;

//                ViewBag.EncryptedCoordinatorLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "AppraisalCoordinator", true);
//                ViewBag.EncryptedGroupHeadLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "GroupHead", true);
//                ViewBag.EncryptedReviewerLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "Reviewer", true);
//                ViewBag.EncryptedAppraiserLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "Appraiser", true);
//                ViewBag.EncryptedSelfAppraisalLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "SelfAppraisal", true);
//                //ViewBag.EncryptedLink = encryptedCoordinatorLink;

//                return View("AppraisalProcessIndex", model);
//            }
//            catch
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
//            }
//        }

//        [HttpGet]
//        public ActionResult AppraisalProcess(string encryptedAppraisalId, string encryptedEmployeeId, string encryptedViewDetails, string encryptedTextLink)
//        {
//            try
//            {
//                bool isAuthorize;
//                string decryptedAppraisalId = HRMSHelper.Decrypt(encryptedAppraisalId, out isAuthorize);
//                if (!isAuthorize)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
//                bool isAuthorizeEmployee;
//                string decryptedEmployeeId = HRMSHelper.Decrypt(encryptedEmployeeId, out isAuthorizeEmployee);
//                if (!isAuthorizeEmployee)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
//                bool isAuthorizeViewDetails;
//                string viewDetails = HRMSHelper.Decrypt(encryptedViewDetails, out isAuthorizeViewDetails);
//                if (!isAuthorizeViewDetails)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
//                bool isAuthorizeTextLink;
//                string textLink = HRMSHelper.Decrypt(encryptedTextLink, out isAuthorizeTextLink);
//                if (!isAuthorizeTextLink)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
//                ViewBag.EncryptedTextLink = encryptedTextLink;
//                int appraisalId = Convert.ToInt16(decryptedAppraisalId);
//                int employeeId = Convert.ToInt16(decryptedEmployeeId);
//                //this part has to be reomoved

//                AppraisalProcessModel model = new AppraisalProcessModel { rating = new RatingApprMinMax() };
//                string EmployeeCode = Membership.GetUser().UserName;
//                string[] role = Roles.GetRolesForUser(EmployeeCode);
//                model.SearchedUserDetails = new SearchedUserDetails();
//                CommonMethodsDAL Commondal = new CommonMethodsDAL();
//                model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
//                ViewBag.LoggedInUserRole = model.SearchedUserDetails.UserRole;
//                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
//                model.SearchedUserDetails.EmployeeCode = EmployeeCode;
//                EmployeeDAL DAL = new EmployeeDAL();
//                model.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
//                HRMS_tbl_PM_Employee EmpDetails = DAL.GetEmployeeDetails(employeeId);
//                model.Employeename = EmpDetails.EmployeeName;
//                ViewBag.LoggedInEmployeeId = model.SearchedUserDetails.EmployeeId;
//                ConfigurationDAL configDAL = new ConfigurationDAL();
//                List<CompetencyMaster> competencyMaster = configDAL.GetCompetencyMaster();
//                ViewBag.TextLink = textLink;
//                ViewBag.EncryptedTextLink = encryptedTextLink;
//                model.LinkClicked = textLink;
//                model.appraisalId = appraisalId;
//                model.projAchievementtAppraisal = new ProjectAchievementAppraisal();
//                model.projAchievementtAppraisal.AppraisalID = appraisalId;
//                model.projAchievementtAppraisal.ProjAchvmntEmpID = employeeId;
//                model.PerfHinderListAppraisal = new PerformanceHinderAppraisal();
//                model.skillAquiredAppraisal = new SkillsAquiredAppraisal();
//                model.skillAquiredAppraisal.AppraisalID = appraisalId;
//                model.skillAquiredAppraisal.SkillEmployeeID = employeeId;
//                model.goalAquireAppraisal = new GoalAquireAppraisal();
//                model.additionalQualificationAppraisal = new AdditionalQualificationAppraisal();
//                model.additionalQualificationAppraisal.AppraisalID = appraisalId;
//                model.additionalQualificationAppraisal.QualifEmployeeID = employeeId;
//                model.AppraisalParameterList = new List<AppraisalParameter>();
//                model.EmployeeID = employeeId;
//                model.IsViewDetails = viewDetails;
//                ViewBag.EncryptedViewDetails = encryptedViewDetails;
//                ViewBag.EncryptedAppraisalId = encryptedAppraisalId;
//                ViewBag.EncryptedEmployeeId = encryptedEmployeeId;
//                AppraisalDAL appraisalDal = new AppraisalDAL();
//                ConfigurationAppraisalDAL configAppraisalDal = new ConfigurationAppraisalDAL();
//                /////new code
//                tbl_Appraisal_AppraisalMaster appraisalDetails = dal.GetAppraisalDetails(appraisalId);
//                ViewBag.isUnfreezedByAdmin = appraisalDetails.UnFreezedByAdmin;
//                string encryptedAppraisalYearId = Commondal.Encrypt(this.Session["SecurityKey"].ToString() + Convert.ToString(appraisalDetails.AppraisalYearID), true);
//                ViewBag.encryptedAppraisalYearId = encryptedAppraisalYearId;
//                ApraiserName appraisalNameDetails = new ApraiserName();
//                appraisalNameDetails = dal.GetAprraiserName(employeeId, appraisalId);
//                DateTime joiningdt = DAL.GetEmployeeJoiningDate(Convert.ToInt32(Convert.ToInt32(decryptedEmployeeId)));
//                ViewBag.JoiningDate = joiningdt;
//                DateTime Dateonly = joiningdt.Date;
//                ViewBag.JoiningDate = Dateonly.ToString("MM/dd/yy");

//                tbl_Appraisal_YearMaster performancePeriod = configAppraisalDal.getAppraisalYearDetails(appraisalDetails.AppraisalYearID);
//                if (performancePeriod != null)
//                {
//                    if (DateTime.Now > performancePeriod.AppraisalYearFrozenOn)
//                        ViewBag.IsPerformanceYearFrozen = true;

//                    if (DateTime.Now > performancePeriod.IDFFrozenOn)
//                        ViewBag.IsIDFFrozen = true;
//                }

//                model.Appraiser1Name = appraisalNameDetails.Appraiser1;
//                if (appraisalNameDetails.Appraiser2 != null)
//                    model.Appraiser2Name = appraisalNameDetails.Appraiser2;
//                else
//                    model.Appraiser2Name = "noAppraiser2";
//                if (appraisalNameDetails.Reviewer2 != null)
//                    model.Reviewer2Name = appraisalNameDetails.Reviewer2;
//                else
//                    model.Reviewer2Name = "noReviewer2";
//                ViewBag.Reviewer2Name = model.Reviewer2Name;
//                model.Reviewer1Name = appraisalNameDetails.Reviewer1;

//                model.GroupHeadName = appraisalNameDetails.GroupHead;
//                model.StageID = Convert.ToInt16(appraisalDetails.AppraisalStageID);
//                ViewBag.StageID = model.StageID;
//                string hrAdmin = UserInRole.HRAdmin.ToString().Insert(2, " ");

//                HRMS_tbl_PM_Employee loginuser = DAL.GetEmployeeDetailsByEmployeeCode(EmployeeCode);
//                if (appraisalDetails != null)
//                    model.appraisalId = appraisalDetails.AppraisalID;
//                else
//                    model.appraisalId = 0;
//                if (appraisalDetails.Appraiser2 == loginuser.EmployeeID)
//                {
//                    ViewBag.IsManagerOrEMployee = "Appraiser2";
//                    model.IsManagerOrEmployee = "Appraiser2";
//                }
//                else if (appraisalDetails.Appraiser1 == loginuser.EmployeeID)
//                {
//                    ViewBag.IsManagerOrEMployee = "Appraiser1";
//                    model.IsManagerOrEmployee = "Appraiser1";
//                }
//                else if (appraisalDetails.Reviewer1 == loginuser.EmployeeID)
//                {
//                    ViewBag.IsManagerOrEMployee = "Reviewer1";
//                    model.IsManagerOrEmployee = "Reviewer1";
//                }
//                else if (appraisalDetails.Reviewer2 == loginuser.EmployeeID)
//                {
//                    ViewBag.IsManagerOrEMployee = "Reviewer2";
//                    model.IsManagerOrEmployee = "Reviewer2";
//                }
//                else if (appraisalDetails.GroupHead == loginuser.EmployeeID)
//                {
//                    ViewBag.IsManagerOrEMployee = "GroupHead";
//                    model.IsManagerOrEmployee = "GroupHead";
//                }

//                else
//                {
//                    ViewBag.IsManagerOrEMployee = "Employee";
//                    model.IsManagerOrEmployee = "Employee";
//                }

//                if (model.StageID == 5 && textLink == "SelfAppraisal" && loginuser.EmployeeID == appraisalDetails.EmployeeID)
//                {
//                    return RedirectToAction("AppraisalProcessIDFForm", "Appraisal", new { encryptedAppraisalId, encryptedEmployeeId, encryptedViewDetails, encryptedTextLink });
//                }

//                tbl_Appraisal_StageEvents _appraisalLatestEntry = appraisalDal.getStageEventlatestEntry(appraisalId);
//                if (_appraisalLatestEntry != null)
//                {
//                    ViewBag.FromStageID = _appraisalLatestEntry.FromStageId;
//                    ViewBag.ToStageID = _appraisalLatestEntry.ToStageId;
//                    ViewBag.ApproverId = _appraisalLatestEntry.USerId;
//                    ViewBag.Emp1 = appraisalDetails.EmployeeID;
//                    ViewBag.App1 = appraisalDetails.Appraiser1;
//                    ViewBag.App2 = appraisalDetails.Appraiser2;
//                    ViewBag.Rew1 = appraisalDetails.Reviewer1;
//                    ViewBag.Rew2 = appraisalDetails.Reviewer2;

//                }
//                tbl_Appraisal_PerformanceHinders perfHinderTable =
//                    appraisalDal.GetAppraisalPerformanceHinder(appraisalId);

//                if (perfHinderTable != null)
//                {
//                    model.PerfHinderListAppraisal = new PerformanceHinderAppraisal
//                    {
//                        AppraisalID = perfHinderTable.AppraisalID,
//                        EmpID = perfHinderTable.EmployeeID,
//                        EmployeeName = model.Employeename,
//                        EmployeeCommentsFFEnvi = perfHinderTable.EmployeeCommentsFFEnvi,
//                        EmployeeCommentsFFSelf = perfHinderTable.EmployeeCommentsFFSelf,
//                        EmployeeCommentsIFEnvi = perfHinderTable.EmployeeCommentsIFEnvi,
//                        EmployeeCommentsIFSelf = perfHinderTable.EmployeeCommentsIFSelf,
//                        EmployeeCommentsSupport = perfHinderTable.EmployeeCommentsSupport,

//                        Appraiser1CommentsFFEnvi = perfHinderTable.Appraiser1CommentsFFEnvi,
//                        Appraiser1CommentsIFEnvi = perfHinderTable.Appraiser1CommentsIFEnvi,
//                        Appraiser1CommentsIFSelf = perfHinderTable.Appraiser1CommentsIFSelf,
//                        Appraiser1CommentsFFSelf = perfHinderTable.Appraiser1CommentsFFSelf,
//                        Appraiser1CommentsSupport = perfHinderTable.Appraiser1CommentsSupport,

//                        Appraiser2CommentsFFEnvi = perfHinderTable.Appraiser2CommentsFFEnvi,
//                        Appraiser2CommentsIFEnvi = perfHinderTable.Appraiser2CommentsIFEnvi,
//                        Appraiser2CommentsIFSelf = perfHinderTable.Appraiser2CommentsIFSelf,
//                        Appraiser2CommentsFFSelf = perfHinderTable.Appraiser2CommentsFFSelf,
//                        Appraiser2CommentsSupport = perfHinderTable.Appraiser2CommentsSupport,

//                        Reviewer1CommentsFFEnvi = perfHinderTable.Reviewer1CommentsFFEnvi,
//                        Reviewer1CommentsIFEnvi = perfHinderTable.Reviewer1CommentsIFEnvi,
//                        Reviewer1CommentsIFSelf = perfHinderTable.Reviewer1CommentsIFSelf,
//                        Reviewer1CommentsFFSelf = perfHinderTable.Reviewer1CommentsFFSelf,
//                        Reviewer1CommentsSupport = perfHinderTable.Reviewer1CommentsSupport,

//                        Reviewer2CommentsFFEnvi = perfHinderTable.Reviewer2CommentsFFEnvi,
//                        Reviewer2CommentsIFEnvi = perfHinderTable.Reviewer2CommentsIFEnvi,
//                        Reviewer2CommentsIFSelf = perfHinderTable.Reviewer2CommentsIFSelf,
//                        Reviewer2CommentsFFSelf = perfHinderTable.Reviewer2CommentsFFSelf,
//                        Reviewer2CommentsSupport = perfHinderTable.Reviewer2CommentsSupport,

//                        GroupHeadCommentsFFEnvi = perfHinderTable.GroupHeadCommentsFFEnvi,
//                        GroupHeadCommentsIFEnvi = perfHinderTable.GroupHeadCommentsIFEnvi,
//                        GroupHeadCommentsIFSelf = perfHinderTable.GroupHeadCommentsIFSelf,
//                        GroupHeadCommentsFFSelf = perfHinderTable.GroupHeadCommentsFFSelf,
//                        GroupHeadCommentsSupport = perfHinderTable.GroupHeadCommentsSupport,

//                        Appraisal1Name = model.Appraiser1Name,
//                        Appraisal2Name = model.Appraiser2Name,
//                        Reviewer1Name = model.Reviewer1Name,
//                        Reviewer2Name = model.Reviewer2Name,
//                        GroupHeadName = model.GroupHeadName,
//                        IsManagerOrEmployee = model.IsManagerOrEmployee,
//                    };
//                }
//                else
//                {
//                    model.PerfHinderListAppraisal = new PerformanceHinderAppraisal
//                    {
//                        EmpID = model.EmployeeID,
//                        AppraisalID = appraisalId,
//                        EmployeeName = model.Employeename,
//                        IsManagerOrEmployee = model.IsManagerOrEmployee,
//                        Appraisal1Name = model.Appraiser1Name,
//                        Appraisal2Name = model.Appraiser2Name,
//                        Reviewer1Name = model.Reviewer1Name,
//                        Reviewer2Name = model.Reviewer2Name,
//                        GroupHeadName = model.GroupHeadName
//                    };
//                }

//                tbl_Appraisal_GoalAspire goalAspire = appraisalDal.GetGoalAspire(appraisalId);
//                if (goalAspire != null)
//                {
//                    model.goalAquireAppraisal = new GoalAquireAppraisal
//                    {
//                        EmployeIDGoal = goalAspire.EmployeeID,
//                        AppraisalIDGoal = goalAspire.AppraisalID,
//                        LongTerm = goalAspire.LongTermGoal,
//                        ShortTerm = goalAspire.ShortTermGoal,
//                        SkillDevPrgm = goalAspire.SkillDevPrgm,
//                    };
//                }
//                else
//                {
//                    model.goalAquireAppraisal = new GoalAquireAppraisal
//                    {
//                        EmployeIDGoal = employeeId,
//                        AppraisalIDGoal = appraisalId,
//                    };
//                }

//                List<AppraisalParameter> paramListAppraisal = appraisalDal.GetParametersDetails(appraisalId, employeeId, appraisalDetails.AppraisalYearID);
//                model.AppraisalParameterList = new List<AppraisalParameter>();
//                AppraisalParameter apprParam;
//                if (paramListAppraisal == null)
//                {
//                    apprParam = new AppraisalParameter();
//                    apprParam.appraisalID = appraisalId;
//                    apprParam.employeeID = employeeId;
//                    apprParam.emplyeeName = model.Employeename.Trim();
//                    apprParam.IsManagerOrEmployee = model.IsManagerOrEmployee;
//                    apprParam.App1Name = model.Appraiser1Name.Trim();
//                    apprParam.App2Name = model.Appraiser2Name.Trim();
//                    apprParam.Rev1Name = model.Reviewer1Name.Trim();
//                    apprParam.Rev2Name = model.Reviewer1Name.Trim();
//                    apprParam.GrpHeadName = model.GroupHeadName.Trim();
//                    model.AppraisalParameterList.Add(apprParam);
//                }
//                else
//                {
//                    model.AppraisalParameterList = paramListAppraisal;
//                    foreach (var appraisalParameter in paramListAppraisal)
//                    {
//                        appraisalParameter.IsManagerOrEmployee = model.IsManagerOrEmployee;
//                        appraisalParameter.emplyeeName = model.Employeename;
//                        appraisalParameter.App1Name = model.Appraiser1Name;
//                        appraisalParameter.App2Name = model.Appraiser2Name;
//                        appraisalParameter.Rev1Name = model.Reviewer1Name;
//                        appraisalParameter.Rev2Name = model.Reviewer2Name;
//                        appraisalParameter.GrpHeadName = model.GroupHeadName;
//                    }
//                }

//                model.certifications = appraisalDal.GetEmployeeCertificationDetails(employeeId, appraisalDetails.AppraisalID);
//                model.rating = dal.GetRating(appraisalDetails.AppraisalYearID);
//                model.GuideLinesList = new List<GuideLines>();
//                model.GuideLinesList = dal.getGuileLines(appraisalDetails.AppraisalYearID);

//                ViewBag.clickedViewDetails = model.IsViewDetails;
//                ViewBag.minRating = model.rating.min;
//                ViewBag.maxRating = model.rating.max;

//                List<int> ratingList = new List<int>();
//                for (int i = model.rating.min; i <= model.rating.max; i++)
//                {
//                    ratingList.Add(i);
//                }
//                model.Mail = new EmployeeMailTemplate();
//                ViewBag.sectionTwoRatingList = ratingList;
//                ViewBag.EmployeeID = model.EmployeeID;
//                ViewBag.appraisalId = model.appraisalId;
//                ViewBag.IsManagerOrEmployee = model.IsManagerOrEmployee;
//                ViewBag.StageID = model.StageID;
//                ViewBag.Appraisar2 = model.Appraiser2Name;
//                ViewBag.Reviwer2 = model.Reviewer2Name;

//                LoadQualificationDropDown(model.additionalQualificationAppraisal);
//                return PartialView("_AppraisalProcess", model);
//            }
//            catch
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
//            }

//        }

//        [HttpGet]
//        public ActionResult AppraiseeDetails(string AppraisalYearID)
//        {
//            try
//            {
//                bool isAuthorize;
//                string decryptedAppraisalYearId = HRMSHelper.Decrypt(AppraisalYearID, out isAuthorize);
//                int appraisalYearID = 0;
//                if (!string.IsNullOrEmpty(decryptedAppraisalYearId))
//                    appraisalYearID = Convert.ToInt32(decryptedAppraisalYearId);
//                AppraiseeDetailsModel model = new AppraiseeDetailsModel();
//                string EmployeeCode = Membership.GetUser().UserName;
//                string[] role = Roles.GetRolesForUser(EmployeeCode);
//                model.SearchedUserDetails = new SearchedUserDetails();
//                CommonMethodsDAL Commondal = new CommonMethodsDAL();
//                model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
//                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
//                model.SearchedUserDetails.EmployeeCode = EmployeeCode;
//                EmployeeDAL DAL = new EmployeeDAL();
//                model.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
//                AppraisalDAL dal = new AppraisalDAL();
//                model.SelectedAppraisalYear = appraisalYearID;
//                model.AppraisalYearList = dal.GetAppraisalYears();
//                model.AppraiserDetailsList = dal.GetAppraiserDetails(model.SearchedUserDetails.EmployeeId, model.SelectedAppraisalYear);
//                model.ReviewerDetailsList = dal.GetReviewerDetails(model.SearchedUserDetails.EmployeeId, model.SelectedAppraisalYear);
//                model.GroupHeadDetailsList = dal.GetGroupHeadDetails(model.SearchedUserDetails.EmployeeId, model.SelectedAppraisalYear);

//                int CountSelfAppraisal = 0, CountGroupHead = 0;
//                int totalCount;
//                List<ActionCount> Count = dal.GetCounts(Convert.ToInt32(model.SearchedUserDetails.EmployeeId));
//                int appraiserCount = dal.GetAsAnAppraiserCount(Convert.ToInt32(model.SearchedUserDetails.EmployeeId), out totalCount);
//                int reviewerCount = dal.GetAsAnReviewerCount(Convert.ToInt32(model.SearchedUserDetails.EmployeeId), out totalCount);
//                int currentAppraisalYearId = dal.GetCurrentYear();
//                string encryptedAppraisalYearId = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(currentAppraisalYearId), true);

//                foreach (var item in Count)
//                {
//                    if ((item.EmployeeId == model.SearchedUserDetails.EmployeeId) && (item.StageId == 0 || item.StageId == 5))
//                    {
//                        CountSelfAppraisal += 1;
//                    }
//                    if (item.GroupHead == model.SearchedUserDetails.EmployeeId && (item.StageId == 3 && item.AppraisalYearFrozenOn <= DateTime.Now))
//                    {
//                        CountGroupHead += 1;
//                    }
//                }
//                ViewBag.SelfAppraisal = CountSelfAppraisal;
//                ViewBag.Appraiser = appraiserCount;
//                ViewBag.Reviewer = reviewerCount;
//                ViewBag.GroupHead = CountGroupHead;
//                ViewBag.AppraisalCordinator = 0;
//                ViewBag.CurrentYear = encryptedAppraisalYearId;

//                ViewBag.EncryptedCoordinatorLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "AppraisalCoordinator", true);
//                ViewBag.EncryptedGroupHeadLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "GroupHead", true);
//                ViewBag.EncryptedReviewerLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "Reviewer", true);
//                ViewBag.EncryptedAppraiserLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "Appraiser", true);
//                ViewBag.EncryptedSelfAppraisalLink = Commondal.Encrypt(Session["SecurityKey"].ToString() + "SelfAppraisal", true);
//                return PartialView("_AppraiseeDetails", model);
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        [HttpGet]
//        public ActionResult EncryptAppraisalYearId(int AppraisalYearId)
//        {
//            try
//            {
//                string encryptedAppraisalYearId = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(AppraisalYearId), true);
//                return Json(new { status = true, encryptedAppraisalYearId = encryptedAppraisalYearId }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpGet]
//        public ActionResult AppraisalProcessThree(string appraisalId, string encryptedEmployeeId, string encryptedViewDetails, string encryptedTextLink)
//        {
//            try
//            {
//                bool isAuthorize;
//                string decryptedAppraisalId = HRMSHelper.Decrypt(appraisalId, out isAuthorize);
//                if (!isAuthorize)
//                    return RedirectToAction("Index", "Error",
//                        new { errorCode = "You are not authorised to do this action." });

//                bool isAuthorizeEmployee;
//                string decryptedEmployeeId = HRMSHelper.Decrypt(encryptedEmployeeId, out isAuthorizeEmployee);
//                if (!isAuthorizeEmployee)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

//                bool isAuthorizeViewDetails;
//                string viewDetails = HRMSHelper.Decrypt(encryptedViewDetails, out isAuthorizeViewDetails);
//                if (!isAuthorizeViewDetails)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

//                bool isAuthorizeTextLink;
//                string textLink = HRMSHelper.Decrypt(encryptedTextLink, out isAuthorizeTextLink);
//                if (!isAuthorizeTextLink)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
//                ViewBag.EncryptedTextLink = encryptedTextLink;

//                EmployeeDAL DAL = new EmployeeDAL();
//                int appraiseeEmployeeId = Convert.ToInt32(decryptedEmployeeId);
//                HRMS_tbl_PM_Employee EmpDetails = DAL.GetEmployeeDetails(appraiseeEmployeeId);
//                ViewBag.AppProcessThreeEmpName = EmpDetails.EmployeeName;

//                AppraisalProcessThreeModel appraisalProcessThreeModel = new AppraisalProcessThreeModel();
//                appraisalProcessThreeModel.AppraisalEmpGrowthSummary = new AppraisalEmployeeGrowthSummary();
//                string EmployeeCode = Membership.GetUser().UserName;
//                string[] role = Roles.GetRolesForUser(EmployeeCode);
//                appraisalProcessThreeModel.SearchedUserDetails = new SearchedUserDetails();
//                CommonMethodsDAL Commondal = new CommonMethodsDAL();
//                appraisalProcessThreeModel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
//                appraisalProcessThreeModel.SearchedUserDetails.EmployeeCode = EmployeeCode;

//                EmployeeDAL employeeDal = new EmployeeDAL();
//                appraisalProcessThreeModel.EmployeeID = employeeDal.GetEmployeeID(EmployeeCode);
//                ViewBag.LoggedInEmployeeId = appraisalProcessThreeModel.EmployeeID;
//                appraisalProcessThreeModel.AppraisalID = Convert.ToInt32(decryptedAppraisalId);
//                AppraisalDAL dal = new AppraisalDAL();
//                appraisalProcessThreeModel.DesignationList = dal.GetDesignationList();

//                ConfigurationAppraisalDAL configAppraisalDal = new ConfigurationAppraisalDAL();
//                tbl_Appraisal_AppraisalMaster appraisalDetails = dal.GetAppraisalDetails(Convert.ToInt32(decryptedAppraisalId));
//                tbl_Appraisal_YearMaster performancePeriod = configAppraisalDal.getAppraisalYearDetails(appraisalDetails.AppraisalYearID);
//                if (performancePeriod != null)
//                {
//                    if (DateTime.Now > performancePeriod.AppraisalYearFrozenOn)
//                        ViewBag.IsPerformanceYearFrozen = true;
//                    if (DateTime.Now > performancePeriod.IDFInitiatedOn)
//                        ViewBag.IsIDFInitiated = true;
//                }
//                int? designationId = employeeDal.GetDesignationIdOfEmployee(appraisalDetails.EmployeeID);
//                appraisalProcessThreeModel.Designation =
//                    appraisalProcessThreeModel.DesignationList.Where(d => d.DesignationID == designationId)
//                        .Select(ds => ds.DesignationDesc)
//                        .FirstOrDefault();
//                appraisalProcessThreeModel.DesignationID = designationId.HasValue ? designationId.Value : 0;
//                appraisalProcessThreeModel.EmployeeID = appraisalDetails.EmployeeID;
//                appraisalProcessThreeModel.StageID = Convert.ToInt16(appraisalDetails.AppraisalStageID);
//                string hrAdmin = UserInRole.HRAdmin.ToString().Insert(2, " ");
//                int employeeId = employeeDal.GetEmployeeID(EmployeeCode);
//                if (appraisalDetails.Reviewer1 == employeeId)
//                {
//                    appraisalProcessThreeModel.UserInRole = UserInRole.Reviewer1;
//                    appraisalProcessThreeModel.AppraisalEmpGrowthSummary.UserInRole = UserInRole.Reviewer1;
//                    ViewBag.IsManagerOrEMployee = "Reviewer1";
//                }
//                else if (appraisalDetails.Reviewer2 == employeeId)
//                {
//                    appraisalProcessThreeModel.UserInRole = UserInRole.Reviewer2;
//                    appraisalProcessThreeModel.AppraisalEmpGrowthSummary.UserInRole = UserInRole.Reviewer2;
//                    ViewBag.IsManagerOrEMployee = "Reviewer2";
//                }
//                else if (appraisalDetails.GroupHead == employeeId)
//                {
//                    appraisalProcessThreeModel.UserInRole = UserInRole.GroupHead;
//                    appraisalProcessThreeModel.AppraisalEmpGrowthSummary.UserInRole = UserInRole.GroupHead;
//                    ViewBag.IsManagerOrEMployee = "GroupHead";
//                }
//                else if (appraisalDetails.Appraiser1 == employeeId)
//                {
//                    appraisalProcessThreeModel.UserInRole = UserInRole.Appraiser1;
//                    appraisalProcessThreeModel.AppraisalEmpGrowthSummary.UserInRole = UserInRole.Appraiser1;
//                    ViewBag.IsManagerOrEMployee = "Appraiser1";
//                }
//                else if (appraisalProcessThreeModel.SearchedUserDetails.UserRole == hrAdmin &&
//                         appraisalDetails.Reviewer1 != employeeId && appraisalDetails.Reviewer2 != employeeId &&
//                         appraisalDetails.GroupHead != employeeId)
//                {
//                    appraisalProcessThreeModel.UserInRole = UserInRole.HRAdmin;
//                    appraisalProcessThreeModel.AppraisalEmpGrowthSummary.UserInRole = UserInRole.HRAdmin;
//                }

//                if (appraisalDetails.Reviewer2 == null)
//                {
//                    ViewBag.Reviewer2 = "noReviewer2";
//                }
//                appraisalProcessThreeModel.AppraisalEmpGrowthSummary =
//                    dal.GetAppProcessDetails(appraisalDetails.EmployeeID, appraisalDetails.AppraisalID);

//                tbl_Appraisal_PromotionRecommendation promotionRecommendation =
//                    dal.getAppraisalPromotionDetails(appraisalDetails.AppraisalID);
//                if (promotionRecommendation != null)
//                {
//                    appraisalProcessThreeModel.Reviewer1Recomendation = promotionRecommendation.PromoRecombyReviewer1.HasValue ? promotionRecommendation.PromoRecombyReviewer1.Value : false;
//                    appraisalProcessThreeModel.Reviewer2Recomendation = promotionRecommendation.PromoRecombyReviewer2.HasValue ? promotionRecommendation.PromoRecombyReviewer2.Value : false;
//                    appraisalProcessThreeModel.GroupHeadRecomendation = promotionRecommendation.PromoRecombyGroupHead.HasValue ? promotionRecommendation.PromoRecombyGroupHead.Value : false;
//                    if (promotionRecommendation.NextDesignationIDFromReviewer1 != null)
//                        appraisalProcessThreeModel.NextDesignationIDFromReviewer1 =
//                            promotionRecommendation.NextDesignationIDFromReviewer1;
//                    if (promotionRecommendation.NextDesignationIDfromReviewer2 != null)
//                        appraisalProcessThreeModel.NextDesignationIDFromReviewer2 =
//                            promotionRecommendation.NextDesignationIDfromReviewer2;
//                    if (promotionRecommendation.NextDesignationIDfromGroupHead != null)
//                        appraisalProcessThreeModel.NextDesignationIDFromGropHead =
//                            promotionRecommendation.NextDesignationIDfromGroupHead;
//                }
//                List<Parameters> parameterForReviewerOne =
//                    dal.GetPromotionRecommendationDetails(appraisalDetails.EmployeeID, appraisalDetails.AppraisalID,
//                        Convert.ToString(UserInRole.Reviewer1),
//                        appraisalProcessThreeModel.NextDesignationIDFromReviewer1);
//                Parameters parameterReviewerOne = new Parameters();
//                if (parameterForReviewerOne.Count > 0)
//                {
//                    appraisalProcessThreeModel.parameterListForReviewerOne = parameterForReviewerOne;
//                    parameterReviewerOne = parameterForReviewerOne.FirstOrDefault();
//                }
//                if (parameterReviewerOne != null)
//                {
//                    appraisalProcessThreeModel.Reviewer1OverallRating = parameterReviewerOne.Reviewer1OverallRating;
//                    appraisalProcessThreeModel.Reviewer1OverallRatingComments =
//                        parameterReviewerOne.Reviewer1OverallRatingComments;
//                    appraisalProcessThreeModel.Reviewer1RecomendationComments =
//                        parameterReviewerOne.Reviewer1RecomendationComments;
//                }
//                if (appraisalDetails.Reviewer2 != null)
//                {
//                    List<Parameters> parameterForReviewerTwo =
//                        dal.GetPromotionRecommendationDetails(appraisalDetails.EmployeeID, appraisalDetails.AppraisalID,
//                            Convert.ToString(UserInRole.Reviewer2),
//                            appraisalProcessThreeModel.NextDesignationIDFromReviewer2);
//                    Parameters parameterReviewerTwo = new Parameters();
//                    if (parameterForReviewerTwo.Count > 0)
//                    {
//                        appraisalProcessThreeModel.parameterListForReviewerTwo = parameterForReviewerTwo;
//                        parameterReviewerTwo = parameterForReviewerTwo.FirstOrDefault();
//                    }
//                    if (parameterReviewerTwo != null)
//                    {
//                        appraisalProcessThreeModel.Reviewer2OverallRating = parameterReviewerTwo.Reviewer2OverallRating;
//                        appraisalProcessThreeModel.Reviewer2OverallRatingComments =
//                            parameterReviewerTwo.Reviewer2OverallRatingComments;
//                        appraisalProcessThreeModel.Reviewer2RecomendationComments =
//                            parameterReviewerTwo.Reviewer2RecomendationComments;
//                    }
//                }
//                List<Parameters> parameterFoeGroupHead =
//                    dal.GetPromotionRecommendationDetails(appraisalDetails.EmployeeID, appraisalDetails.AppraisalID,
//                        Convert.ToString(UserInRole.GroupHead), appraisalProcessThreeModel.NextDesignationIDFromGropHead);
//                Parameters parameterGroupHead = new Parameters();
//                if (parameterFoeGroupHead.Count > 0)
//                {
//                    appraisalProcessThreeModel.parameterListForGroupHead = parameterFoeGroupHead;
//                    parameterGroupHead = parameterFoeGroupHead.FirstOrDefault();
//                }
//                if (parameterGroupHead != null)
//                {
//                    appraisalProcessThreeModel.GroupHeadOverallRating = parameterGroupHead.GroupHeadOverallRating;
//                    appraisalProcessThreeModel.GroupHeadOverallRatingComments =
//                        parameterGroupHead.GroupHeadOverallRatingComments;
//                    appraisalProcessThreeModel.GroupHeadRecomendationComments =
//                        parameterGroupHead.GroupHeadRecomendationComments;
//                }
//                if (appraisalProcessThreeModel.UserInRole == UserInRole.Reviewer1 ||
//                    appraisalProcessThreeModel.UserInRole == UserInRole.Reviewer2 ||
//                    appraisalProcessThreeModel.UserInRole == UserInRole.GroupHead ||
//                    appraisalProcessThreeModel.UserInRole == UserInRole.HRAdmin)
//                {
//                    AppraisalProcessThreeModel modelForReviewerOne =
//                        dal.GetSuccessionPlanningDetails(appraisalDetails.EmployeeID, appraisalDetails.AppraisalID,
//                            Convert.ToString(UserInRole.Reviewer1));
//                    appraisalProcessThreeModel.ReadyComments1to2YearsByReviewer1 =
//                        modelForReviewerOne.ReadyComments1to2YearsByReviewer1;
//                    appraisalProcessThreeModel.ReadyComments3to5YearsByReviewer1 =
//                        modelForReviewerOne.ReadyComments3to5YearsByReviewer1;
//                    appraisalProcessThreeModel.ReadyCommentsByReviewer1 = modelForReviewerOne.ReadyCommentsByReviewer1;

//                    AppraisalProcessThreeModel modelForReviewerTwo =
//                        dal.GetSuccessionPlanningDetails(appraisalDetails.EmployeeID, appraisalDetails.AppraisalID,
//                            Convert.ToString(UserInRole.Reviewer2));
//                    appraisalProcessThreeModel.ReadyComments1to2YearsByReviewer2 =
//                        modelForReviewerTwo.ReadyComments1to2YearsByReviewer2;
//                    appraisalProcessThreeModel.ReadyComments3to5YearsByReviewer2 =
//                        modelForReviewerTwo.ReadyComments3to5YearsByReviewer2;
//                    appraisalProcessThreeModel.ReadyCommentsByReviewer2 = modelForReviewerTwo.ReadyCommentsByReviewer2;
//                }
//                UpraisalRating uprasRating = dal.GetMinMaxRating(appraisalDetails.AppraisalYearID);
//                ViewBag.minRating = uprasRating.MinValue;
//                ViewBag.maxRating = uprasRating.MaxValue;
//                List<int> ratingList = new List<int>();
//                for (int i = uprasRating.MinValue; i <= uprasRating.MaxValue; i++)
//                {
//                    ratingList.Add(i);
//                }
//                appraisalProcessThreeModel.IsViewDetails = viewDetails;
//                appraisalProcessThreeModel.RatingsList = ratingList;
//                ApraiserName appraisalNameDetails = new ApraiserName();
//                appraisalNameDetails = dal.GetAprraiserName(appraisalDetails.EmployeeID, Convert.ToInt32(decryptedAppraisalId));
//                appraisalProcessThreeModel.Reviewer1Name = appraisalNameDetails.Reviewer1;
//                if (appraisalNameDetails.Reviewer2 != null)
//                    appraisalProcessThreeModel.Reviewer2Name = appraisalNameDetails.Reviewer2;
//                else
//                    appraisalProcessThreeModel.Reviewer2Name = "noReviewer2";
//                appraisalProcessThreeModel.GroupHeadName = appraisalNameDetails.GroupHead;

//                appraisalProcessThreeModel.AppraisalEmpGrowthSummary.Reviewer1Name = appraisalNameDetails.Reviewer1;
//                if (appraisalNameDetails.Reviewer2 != null)
//                    appraisalProcessThreeModel.AppraisalEmpGrowthSummary.Reviewer2Name = appraisalNameDetails.Reviewer2;
//                else
//                    appraisalProcessThreeModel.AppraisalEmpGrowthSummary.Reviewer2Name = "noReviewer2";
//                appraisalProcessThreeModel.AppraisalEmpGrowthSummary.GroupHeadName = appraisalNameDetails.GroupHead;
//                ViewBag.EncryptedAppraisalId = appraisalId;
//                ViewBag.EncryptedEmployeeId = encryptedEmployeeId;
//                ViewBag.appraisalId = decryptedAppraisalId;
//                ViewBag.StageID = appraisalProcessThreeModel.StageID;
//                appraisalProcessThreeModel.Mail = new EmployeeMailTemplate();
//                ViewBag.TextLink = textLink;
//                ViewBag.EncryptedViewDetails = encryptedViewDetails;
//                return PartialView("_AppraisalProcessThree", appraisalProcessThreeModel);
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        [HttpGet]
//        public ActionResult GetParameterForReviewerOne(int appraisalId, int? designationId)
//        {
//            AppraisalProcessThreeModel appraisalProcessThreeModel = new AppraisalProcessThreeModel();
//            AppraisalDAL dal = new AppraisalDAL();
//            tbl_Appraisal_AppraisalMaster appraisalDetails = dal.GetAppraisalDetails(appraisalId);
//            appraisalProcessThreeModel.DesignationList = dal.GetDesignationList();
//            if (designationId == null)
//                appraisalProcessThreeModel.NextDesignationIDFromReviewer1 = null;
//            else
//                appraisalProcessThreeModel.NextDesignationIDFromReviewer1 = designationId;
//            List<Parameters> parameterList = dal.GetPromotionRecommendationDetails(appraisalDetails.EmployeeID, appraisalDetails.AppraisalID, UserInRole.Reviewer1.ToString(), appraisalProcessThreeModel.NextDesignationIDFromReviewer1);
//            Parameters parameter = new Parameters();
//            if (parameterList != null)
//            {
//                appraisalProcessThreeModel.parameterListForReviewerOne = parameterList;
//                parameter = parameterList.FirstOrDefault();
//            }
//            if (parameter != null)
//            {
//                appraisalProcessThreeModel.Reviewer1OverallRating = parameter.Reviewer1OverallRating;
//                appraisalProcessThreeModel.Reviewer1OverallRatingComments = parameter.Reviewer1OverallRatingComments;
//                appraisalProcessThreeModel.Reviewer1RecomendationComments = parameter.Reviewer1RecomendationComments;
//            }
//            UpraisalRating uprasRating = dal.GetMinMaxRating(appraisalDetails.AppraisalYearID);
//            ViewBag.minRating = uprasRating.MinValue;
//            ViewBag.maxRating = uprasRating.MaxValue;
//            List<int> ratingList = new List<int>();
//            for (int i = uprasRating.MinValue; i <= uprasRating.MaxValue; i++)
//            {
//                ratingList.Add(i);
//            }
//            appraisalProcessThreeModel.RatingsList = ratingList;
//            appraisalProcessThreeModel.AppraisalID = appraisalId;
//            ApraiserName appraisalNameDetails = new ApraiserName();
//            appraisalNameDetails = dal.GetAprraiserName(appraisalDetails.EmployeeID, appraisalId);
//            appraisalProcessThreeModel.Reviewer1Name = appraisalNameDetails.Reviewer1;
//            appraisalProcessThreeModel.Reviewer2Name = appraisalNameDetails.Reviewer2;
//            appraisalProcessThreeModel.GroupHeadName = appraisalNameDetails.GroupHead;
//            return PartialView("_promotionRecommendationForReviewerOne", appraisalProcessThreeModel);
//        }

//        [HttpGet]
//        public ActionResult GetParameterForReviewerTwo(int appraisalId, int? designationId)
//        {
//            AppraisalProcessThreeModel appraisalProcessThreeModel = new AppraisalProcessThreeModel();
//            AppraisalDAL dal = new AppraisalDAL();
//            tbl_Appraisal_AppraisalMaster appraisalDetails = dal.GetAppraisalDetails(appraisalId);
//            appraisalProcessThreeModel.DesignationList = dal.GetDesignationList();
//            if (designationId == null)
//                appraisalProcessThreeModel.NextDesignationIDFromReviewer2 = null;
//            else
//                appraisalProcessThreeModel.NextDesignationIDFromReviewer2 = designationId;
//            List<Parameters> parameterList = dal.GetPromotionRecommendationDetails(appraisalDetails.EmployeeID, appraisalDetails.AppraisalID, UserInRole.Reviewer2.ToString(), appraisalProcessThreeModel.NextDesignationIDFromReviewer2);
//            Parameters parameter = new Parameters();
//            if (parameterList != null)
//            {
//                appraisalProcessThreeModel.parameterListForReviewerTwo = parameterList;
//                parameter = parameterList.FirstOrDefault();
//            }
//            if (parameter != null)
//            {
//                appraisalProcessThreeModel.Reviewer2OverallRating = parameter.Reviewer2OverallRating;
//                appraisalProcessThreeModel.Reviewer2OverallRatingComments = parameter.Reviewer2OverallRatingComments;
//                appraisalProcessThreeModel.Reviewer2RecomendationComments = parameter.Reviewer2RecomendationComments;
//            }
//            UpraisalRating uprasRating = dal.GetMinMaxRating(appraisalDetails.AppraisalYearID);
//            ViewBag.minRating = uprasRating.MinValue;
//            ViewBag.maxRating = uprasRating.MaxValue;
//            List<int> ratingList = new List<int>();
//            for (int i = uprasRating.MinValue; i <= uprasRating.MaxValue; i++)
//            {
//                ratingList.Add(i);
//            }
//            appraisalProcessThreeModel.RatingsList = ratingList;
//            appraisalProcessThreeModel.AppraisalID = appraisalId;

//            ApraiserName appraisalNameDetails = new ApraiserName();
//            appraisalNameDetails = dal.GetAprraiserName(appraisalDetails.EmployeeID, appraisalId);

//            appraisalProcessThreeModel.Reviewer1Name = appraisalNameDetails.Reviewer1;
//            appraisalProcessThreeModel.Reviewer2Name = appraisalNameDetails.Reviewer2;
//            appraisalProcessThreeModel.GroupHeadName = appraisalNameDetails.GroupHead;

//            return PartialView("_promotionRecommendationForReviewerTwo", appraisalProcessThreeModel);
//        }

//        [HttpGet]
//        public ActionResult GetParameterForGroupHead(int appraisalId, int? designationId)
//        {
//            AppraisalProcessThreeModel appraisalProcessThreeModel = new AppraisalProcessThreeModel();
//            AppraisalDAL dal = new AppraisalDAL();
//            tbl_Appraisal_AppraisalMaster appraisalDetails = dal.GetAppraisalDetails(appraisalId);
//            appraisalProcessThreeModel.DesignationList = dal.GetDesignationList();
//            if (designationId == null)
//                appraisalProcessThreeModel.NextDesignationIDFromGropHead = null;
//            else
//                appraisalProcessThreeModel.NextDesignationIDFromGropHead = designationId;
//            List<Parameters> parameterList = dal.GetPromotionRecommendationDetails(appraisalDetails.EmployeeID, appraisalDetails.AppraisalID, UserInRole.GroupHead.ToString(), appraisalProcessThreeModel.NextDesignationIDFromGropHead);
//            Parameters parameter = new Parameters();
//            if (parameterList != null)
//            {
//                appraisalProcessThreeModel.parameterListForGroupHead = parameterList;
//                parameter = parameterList.FirstOrDefault();
//            }
//            if (parameter != null)
//            {
//                appraisalProcessThreeModel.GroupHeadOverallRating = parameter.GroupHeadOverallRating;
//                appraisalProcessThreeModel.GroupHeadOverallRatingComments = parameter.GroupHeadOverallRatingComments;
//                appraisalProcessThreeModel.GroupHeadRecomendationComments = parameter.GroupHeadRecomendationComments;
//            }
//            UpraisalRating uprasRating = dal.GetMinMaxRating(appraisalDetails.AppraisalYearID);
//            ViewBag.minRating = uprasRating.MinValue;
//            ViewBag.maxRating = uprasRating.MaxValue;
//            List<int> ratingList = new List<int>();
//            for (int i = uprasRating.MinValue; i <= uprasRating.MaxValue; i++)
//            {
//                ratingList.Add(i);
//            }
//            appraisalProcessThreeModel.RatingsList = ratingList;
//            appraisalProcessThreeModel.AppraisalID = appraisalId;

//            ApraiserName appraisalNameDetails = new ApraiserName();
//            appraisalNameDetails = dal.GetAprraiserName(appraisalDetails.EmployeeID, appraisalId);

//            appraisalProcessThreeModel.Reviewer1Name = appraisalNameDetails.Reviewer1;
//            appraisalProcessThreeModel.Reviewer2Name = appraisalNameDetails.Reviewer2;
//            appraisalProcessThreeModel.GroupHeadName = appraisalNameDetails.GroupHead;

//            return PartialView("_promotionRecommendationForGroupHead", appraisalProcessThreeModel);
//        }

//        [HttpPost]
//        public ActionResult SaveAppraisalProcressThreeDetails(List<AppraisalEmployeeGrowthSummary> model)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                var status = dal.SavAppraisalProcressThreeDetails(model);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }

//        }

//        [HttpPost]
//        public ActionResult SavePromotionRecommendationDetails(List<Parameters> model)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                var status = dal.SavePromotionRecommendationDetails(model);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";
//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }

//        }

//        [HttpPost]
//        public ActionResult SaveSuccessionPlanningDetails(AppraisalProcessThreeModel model)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                var status = dal.SaveSuccessionPlanningDetails(model);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";
//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpGet]
//        public ActionResult AppraisalProcessIDFForm(string encryptedAppraisalId, string encryptedEmployeeId, string encryptedViewDetails, string encryptedTextLink)
//        {
//            try
//            {
//                bool isAuthorize;
//                string decryptedAppraisalId = HRMSHelper.Decrypt(encryptedAppraisalId, out isAuthorize);
//                if (!isAuthorize)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

//                bool isAuthorizeViewDetails;
//                string viewDetails = HRMSHelper.Decrypt(encryptedViewDetails, out isAuthorizeViewDetails);
//                if (!isAuthorizeViewDetails)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

//                bool isAuthorizeTextLink;
//                string textLink = HRMSHelper.Decrypt(encryptedTextLink, out isAuthorizeTextLink);
//                if (!isAuthorizeTextLink)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
//                ViewBag.EncryptedTextLink = encryptedTextLink;
//                AppraisalProcessFourModel appraisalProcessFourModel = new AppraisalProcessFourModel();

//                string EmployeeCode = Membership.GetUser().UserName;
//                string[] role = Roles.GetRolesForUser(EmployeeCode);
//                appraisalProcessFourModel.SearchedUserDetails = new SearchedUserDetails();

//                CommonMethodsDAL Commondal = new CommonMethodsDAL();
//                appraisalProcessFourModel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
//                ViewBag.LoggedInUserRole = appraisalProcessFourModel.SearchedUserDetails.UserRole;
//                appraisalProcessFourModel.SearchedUserDetails.EmployeeCode = EmployeeCode;

//                EmployeeDAL employeeDal = new EmployeeDAL();
//                appraisalProcessFourModel.EmployeeId = employeeDal.GetEmployeeID(EmployeeCode);
//                ViewBag.LoggedInEmployeeId = appraisalProcessFourModel.EmployeeId;
//                appraisalProcessFourModel.AppraisalId = Convert.ToInt32(decryptedAppraisalId);

//                AppraisalDAL dal = new AppraisalDAL();
//                appraisalProcessFourModel.Strengths = new AppraiseeStrengths();
//                appraisalProcessFourModel.Improvements = new AppraiseeImprovements();
//                appraisalProcessFourModel.TrainingProgram = new TrainingProgram();

//                tbl_Appraisal_AppraisalMaster appraisalDetails = dal.GetAppraisalDetails(Convert.ToInt32(decryptedAppraisalId));

//                //appraisalProcessFourModel.IsImprovementStrengthVisible = dal.IsReviewerStageCleared(appraisalProcessFourModel.AppraisalId);

//                //appraisalProcessFourModel.StrengthCommentsIDF = appraisalDetails.StrengthComments;
//                //appraisalProcessFourModel.ImprovementCommentsIDF = appraisalDetails.ImprovementComments;

//                appraisalProcessFourModel.TrainingProgram.TrainingProgramList = dal.GetCategoryList();
//                ViewBag.StageId = appraisalDetails.AppraisalStageID;
//                appraisalProcessFourModel.StageId = Convert.ToInt16(appraisalDetails.AppraisalStageID);
//                appraisalProcessFourModel.IsAgree = appraisalDetails.IDFISAppraiseAgree;
//                ConfigurationAppraisalDAL configAppraisalDal = new ConfigurationAppraisalDAL();
//                tbl_Appraisal_StageEvents _idfLatestEntry = dal.getStageEventlatestEntry(appraisalProcessFourModel.AppraisalId);
//                ViewBag.FromStageID = _idfLatestEntry.FromStageId;
//                ViewBag.ToStageID = _idfLatestEntry.ToStageId;
//                tbl_Appraisal_YearMaster performancePeriod = configAppraisalDal.getAppraisalYearDetails(appraisalDetails.AppraisalYearID);
//                if (performancePeriod != null)
//                {
//                    if (DateTime.Now > performancePeriod.AppraisalYearFrozenOn)
//                        ViewBag.IsPerformanceYearFrozen = true;
//                }
//                string hrAdmin = UserInRole.HRAdmin.ToString().Insert(2, " ");
//                int employeeId = employeeDal.GetEmployeeID(EmployeeCode);

//                string encryptedEmployeeID = Commondal.Encrypt(this.Session["SecurityKey"].ToString() + Convert.ToString(appraisalDetails.EmployeeID), true);
//                if (appraisalDetails.Appraiser1 == employeeId)
//                {
//                    appraisalProcessFourModel.UserInRole = UserInRole.Appraiser1;
//                    ViewBag.UserInRole = "Appraiser1";
//                    ViewBag.IsManagerOrEMployee = "Appraiser1";
//                }
//                else if (appraisalDetails.EmployeeID == employeeId)
//                {
//                    appraisalProcessFourModel.UserInRole = UserInRole.Employee;
//                    ViewBag.UserInRole = "Employee";
//                    ViewBag.IsManagerOrEMployee = "Employee";
//                }
//                else if (appraisalDetails.Reviewer1 == employeeId)
//                {
//                    appraisalProcessFourModel.UserInRole = UserInRole.Reviewer1;
//                    ViewBag.UserInRole = "Reviewer1";
//                    ViewBag.IsManagerOrEMployee = "Reviewer1";
//                }
//                else if (appraisalDetails.Reviewer2 == employeeId)
//                {
//                    appraisalProcessFourModel.UserInRole = UserInRole.Reviewer2;
//                    ViewBag.UserInRole = "Reviewer2";
//                    ViewBag.IsManagerOrEMployee = "Reviewer2";
//                }
//                else if (appraisalProcessFourModel.SearchedUserDetails.UserRole == hrAdmin && appraisalDetails.Reviewer1 != employeeId && appraisalDetails.Reviewer2 != employeeId && appraisalDetails.GroupHead != employeeId)
//                {
//                    appraisalProcessFourModel.UserInRole = UserInRole.HRAdmin;
//                    ViewBag.UserInRole = "HRAdmin";
//                    ViewBag.IsManagerOrEMployee = "HRAdmin";
//                }
//                if (appraisalDetails.AppraisalStageID == 7 || appraisalDetails.AppraisalStageID == 8)
//                    appraisalProcessFourModel.AppraiseeComments = appraisalDetails.IDFAprraiseComment;

//                appraisalProcessFourModel.TrainingProgram.EmployeeId = employeeId;
//                appraisalProcessFourModel.TrainingProgram.AppraisalId = Convert.ToInt32(decryptedAppraisalId);
//                appraisalProcessFourModel.TrainingProgram.AppraisalYearId = appraisalDetails.AppraisalYearID;
//                appraisalProcessFourModel.Strengths.AppraisalId = Convert.ToInt32(decryptedAppraisalId);
//                appraisalProcessFourModel.Strengths.AppraisalYearId = appraisalDetails.AppraisalYearID;
//                appraisalProcessFourModel.Strengths.EmployeeId = employeeId;
//                appraisalProcessFourModel.Improvements.EmployeeId = employeeId;
//                appraisalProcessFourModel.Improvements.AppraisalId = Convert.ToInt32(decryptedAppraisalId);
//                appraisalProcessFourModel.Improvements.AppraisalYearId = appraisalDetails.AppraisalYearID;
//                appraisalProcessFourModel.IsViewDetails = viewDetails;
//                ViewBag.appraisalId = decryptedAppraisalId;
//                int loggedInUserEmployeeID = employeeDal.GetEmployeeID(Membership.GetUser().UserName);
//                if (appraisalDetails.Appraiser1 == loggedInUserEmployeeID)
//                {
//                    appraisalProcessFourModel.IsAppraiser1 = true;
//                    ViewBag.IsAppraiser1 = true;
//                }
//                if (appraisalDetails.EmployeeID == loggedInUserEmployeeID)
//                {
//                    appraisalProcessFourModel.IsEmployee = true;
//                    ViewBag.IsEmployee = true;
//                }
//                appraisalProcessFourModel.Mail = new EmployeeMailTemplate();
//                ViewBag.EncryptedAppraisalId = encryptedAppraisalId;
//                ViewBag.EncryptedEmployeeId = encryptedEmployeeID;
//                ViewBag.TextLink = textLink;
//                ViewBag.EncryptedViewDetails = encryptedViewDetails;
//                ViewBag.clickedViewDetails = viewDetails;
//                HRMS_tbl_PM_Employee EmpDetails = employeeDal.GetEmployeeDetails(employeeId);
//                ViewBag.AppProcessFourEmpName = EmpDetails.EmployeeName;
//                return PartialView("_AppraisalProcessIDFForm", appraisalProcessFourModel);
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        [HttpPost]
//        public ActionResult ProjectAchievementAppraisalDetailsLoadGrid(int? appraisalId, int page, int rows)
//        {
//            try
//            {
//                bool isAuthorize;

//                string decryptedAppraisalId = Convert.ToString(appraisalId);

//                AppraisalDAL dal = new AppraisalDAL();
//                ProjectAchievementAppraisal objAppraisalDetailModel = new Models.ProjectAchievementAppraisal();
//                objAppraisalDetailModel.projectAchievementAppraisalList = new List<Models.ProjectAchievementAppraisal>();
//                int totalCount = 0;
//                objAppraisalDetailModel.projectAchievementAppraisalList = dal.GetProjectAchievementAppraisalDetails(Convert.ToInt32(decryptedAppraisalId), page, rows, out totalCount);
//                if ((objAppraisalDetailModel.projectAchievementAppraisalList == null || objAppraisalDetailModel.projectAchievementAppraisalList.Count <= 0) && page - 1 > 0)
//                {
//                    page = page - 1;
//                    objAppraisalDetailModel.projectAchievementAppraisalList = dal.GetProjectAchievementAppraisalDetails(Convert.ToInt32(decryptedAppraisalId), page, rows, out totalCount);
//                }
//                var totalRecords = totalCount;
//                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
//                var jsonData = new
//                {
//                    total = totalPages,
//                    page = page,
//                    records = totalRecords,
//                    rows = objAppraisalDetailModel.projectAchievementAppraisalList,
//                };
//                return Json(jsonData);
//            }
//            catch (Exception)
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
//            }
//        }

//        public string UploadFileLocation
//        {
//            get
//            {
//                return System.Configuration.ConfigurationManager.AppSettings["UploadAppraisalAchmentFileLocation"];
//            }
//        }

//        public string UploadDesgnatinFileLocation
//        {
//            get
//            {
//                return System.Configuration.ConfigurationManager.AppSettings["UploadDesintnFileLocation"];
//            }
//        }

//        [AcceptVerbs(HttpVerbs.Post)]
//        public ActionResult SaveProjectAchievementAppraisalInfo(HttpPostedFileBase doc, ProjectAchievementAppraisal model)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();

//                bool uploadStatus = false;
//                string resultMessage = string.Empty;

//                if (doc != null && doc.ContentLength > 0)
//                {
//                    string uploadsPath = HttpContext.Server.MapPath(UploadFileLocation);
//                    string uploadpathwithId = Path.Combine(uploadsPath, model.ProjAchvmntEmpID.ToString());

//                    uploadsPath = Path.Combine(uploadpathwithId, Convert.ToString(model.AppraisalID));
//                    string fileName = Path.GetFileName(doc.FileName);
//                    model.FileName = fileName;
//                    model.FilePath = uploadsPath;
//                    string filePath = Path.Combine(uploadsPath, fileName);
//                    if (!Directory.Exists(uploadsPath))
//                        Directory.CreateDirectory(uploadsPath);
//                    doc.SaveAs(filePath);
//                }

//                var status = dal.SaveProjectAchievementAppraisalDetails(model);

//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";
//                return Json(new { results = resultMessage, status = status }, "text/html", JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, "text/html", JsonRequestBehavior.AllowGet);
//            }
//        }

//        public ActionResult DeleteProjectAchievementAppraisalDetails(int ProjectAchievementID)
//        {
//            try
//            {
//                AppraisalDAL dal = new AppraisalDAL();
//                tbl_Appraisal_ProjectAchivement projectAchievementDetails = new tbl_Appraisal_ProjectAchivement();
//                bool eq = dal.DeleteprojectAchievementAppraisalDetails(ProjectAchievementID);
//                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }
//        [HttpPost]
//        public ActionResult CorporateDetailsAppraisalLoadGrid(int appraisalId, int StageID, string IsManagerOrEMployee, string TextLink, int page, int rows)
//        {
//            try
//            {
//                bool isAuthorize;

//                string decryptedAppraisalId = Convert.ToString(appraisalId);

//                AppraisalDAL dal = new AppraisalDAL();
//                AppraisalProcessModel objCorporateDetailModel = new Models.AppraisalProcessModel();
//                objCorporateDetailModel.CorporateContributionList = new List<Models.AppraisalProcessModel>();
//                int totalCount = 0;

//                objCorporateDetailModel.CorporateContributionList = dal.GetCorporateDetails(Convert.ToInt32(decryptedAppraisalId), StageID, IsManagerOrEMployee, TextLink, page, rows, out totalCount);

//                if ((objCorporateDetailModel.CorporateContributionList == null || objCorporateDetailModel.CorporateContributionList.Count <= 0) && page - 1 > 0)
//                {
//                    page = page - 1;
//                    objCorporateDetailModel.CorporateContributionList = dal.GetCorporateDetails(Convert.ToInt32(decryptedAppraisalId), StageID, IsManagerOrEMployee, TextLink, page, rows, out totalCount);
//                }

//                var totalRecords = totalCount;
//                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
//                var jsonData = new
//                {
//                    total = totalPages,
//                    page = page,
//                    records = totalRecords,
//                    rows = objCorporateDetailModel.CorporateContributionList,
//                };
//                return Json(jsonData);
//            }
//            catch (Exception)
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
//            }
//        }

//        [HttpPost]
//        public ActionResult SaveCorporateAppraisalInfo(AppraisalProcessModel model, string IsManagerOrEMployee)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                model.IsManagerOrEmployee = IsManagerOrEMployee;
//                var status = dal.SaveCorporateDetails(model);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult AddQualificationAppraisalDetailsLoadGrid(int appraisalId, int page, int rows)
//        {
//            try
//            {
//                string decryptedAppraisalId = Convert.ToString(appraisalId);

//                AppraisalDAL dal = new AppraisalDAL();
//                AdditionalQualificationAppraisal objAddQualificationDetailModel = new Models.AdditionalQualificationAppraisal();
//                objAddQualificationDetailModel.additionalQualificationAppraisalList = new List<Models.AdditionalQualificationAppraisal>();
//                int totalCount = 0;

//                objAddQualificationDetailModel.additionalQualificationAppraisalList = dal.GetAddQualificationAppraisalDetails(Convert.ToInt32(decryptedAppraisalId), page, rows, out totalCount);

//                if ((objAddQualificationDetailModel.additionalQualificationAppraisalList == null || objAddQualificationDetailModel.additionalQualificationAppraisalList.Count <= 0) && page - 1 > 0)
//                {
//                    page = page - 1;
//                    objAddQualificationDetailModel.additionalQualificationAppraisalList = dal.GetAddQualificationAppraisalDetails(Convert.ToInt32(decryptedAppraisalId), page, rows, out totalCount);
//                }

//                var totalRecords = totalCount;
//                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
//                var jsonData = new
//                {
//                    total = totalPages,
//                    page = page,
//                    records = totalRecords,
//                    rows = objAddQualificationDetailModel.additionalQualificationAppraisalList,
//                };
//                return Json(jsonData);
//            }
//            catch (Exception)
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
//            }
//        }

//        public ActionResult DeleteAddQualificationAppraisalDetails(int AddQualificationID)
//        {
//            try
//            {
//                AppraisalDAL dal = new AppraisalDAL();
//                tbl_Appraisal_AdditionalQualification addQualificationDetails = new tbl_Appraisal_AdditionalQualification();
//                bool eq = dal.DeleteQualificationAppraisalDetails(AddQualificationID);
//                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult SaveAddQualificationAppraisalDetails(AdditionalQualificationAppraisal model, int QualifEmployeeID, int AppraisalID, int TypeID)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                model.skill = TypeID;
//                model.QualifEmployeeID = QualifEmployeeID;
//                model.AppraisalID = AppraisalID;
//                var status = dal.SaveAddQualificationAppraisalDetails(model);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult SkillAquiredAppraisalDetailsLoadGrid(int appraisalId, int page, int rows)
//        {
//            try
//            {
//                string decryptedAppraisalId = Convert.ToString(appraisalId);

//                AppraisalDAL dal = new AppraisalDAL();
//                SkillsAquiredAppraisal objSkillAquiredDetailModel = new Models.SkillsAquiredAppraisal();
//                objSkillAquiredDetailModel.skillsAquiredList = new List<Models.SkillsAquiredAppraisal>();
//                int totalCount = 0;

//                objSkillAquiredDetailModel.skillsAquiredList = dal.GetSkillAquiredAppraisalDetails(Convert.ToInt32(decryptedAppraisalId), page, rows, out totalCount);

//                if ((objSkillAquiredDetailModel.skillsAquiredList == null || objSkillAquiredDetailModel.skillsAquiredList.Count <= 0) && page - 1 > 0)
//                {
//                    page = page - 1;
//                    objSkillAquiredDetailModel.skillsAquiredList = dal.GetSkillAquiredAppraisalDetails(Convert.ToInt32(decryptedAppraisalId), page, rows, out totalCount);
//                }

//                var totalRecords = totalCount;
//                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
//                var jsonData = new
//                {
//                    total = totalPages,
//                    page = page,
//                    records = totalRecords,
//                    rows = objSkillAquiredDetailModel.skillsAquiredList,
//                };
//                return Json(jsonData);
//            }
//            catch (Exception)
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
//            }
//        }

//        public ActionResult DeleteSkillAquiredAppraisalDetails(int SkillAquiredID)
//        {
//            try
//            {
//                AppraisalDAL dal = new AppraisalDAL();
//                tbl_Appraisal_SkillAquired skillAquiredDetails = new tbl_Appraisal_SkillAquired();
//                bool eq = dal.DeleteskillAquiredAppraisalDetails(SkillAquiredID);
//                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult SaveSkillAquiredAppraisalInfo(SkillsAquiredAppraisal model, int SkillEmployeeID, int AppraisalID)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                model.SkillEmployeeID = SkillEmployeeID;
//                model.AppraisalID = AppraisalID;
//                var status = dal.SaveSkillAquiredAppraisalDetails(model);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        /// <summary>
//        /// Gets qualification type.
//        /// </summary>
//        /// <param name="model"></param>
//        private void LoadQualificationDropDown(AdditionalQualificationAppraisal model)
//        {
//            QualificationDetailsDAL dal = new QualificationDetailsDAL();
//            model.QualificationList = new List<AddQualificationListClassAppraisal>();
//            List<tbl_PM_QualificationType> qualifDetailsList = dal.GetQualificationTypeList();
//            if (qualifDetailsList != null)
//            {
//                foreach (tbl_PM_QualificationType eachqualificationDetail in qualifDetailsList)
//                {
//                    model.QualificationList.Add(new AddQualificationListClassAppraisal()
//                    {
//                        AddQualificationID = eachqualificationDetail.QualificationTypeID,
//                        AddQualification = eachqualificationDetail.QualificationTypeName.Trim(),
//                    });
//                }
//            }
//        }
//        [HttpPost]
//        public ActionResult SavePerformanceHinderAppraisalInfo(PerformanceHinderAppraisal model)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                var status = dal.SavePerformanceHinderAppraisalDetails(model);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult SaveGoalAspireAppraisal(GoalAquireAppraisal model)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                var status = dal.SaveGoalAspireAppraisalDetails(model);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult AppraiseeStrengthDetailsLoadGrid(string employeeId, int appraisalID, int appraisalYearId, int page, int rows)
//        {
//            try
//            {
//                bool isAuthorize;

//                string decryptedEmployeeId = employeeId;

//                AppraisalDAL dal = new AppraisalDAL();
//                AppraiseeStrengths objAppraisalDetailModel = new Models.AppraiseeStrengths();
//                objAppraisalDetailModel.AppraiseeStrengthsList = new List<Models.AppraiseeStrengths>();
//                int totalCount = 0;
//                objAppraisalDetailModel.AppraiseeStrengthsList = dal.GetAppraiseeStrengthsDetails(Convert.ToInt32(decryptedEmployeeId), appraisalID, appraisalYearId, page, rows, out totalCount);
//                if ((objAppraisalDetailModel.AppraiseeStrengthsList == null || objAppraisalDetailModel.AppraiseeStrengthsList.Count <= 0) && page - 1 > 0)
//                {
//                    page = page - 1;
//                    objAppraisalDetailModel.AppraiseeStrengthsList = dal.GetAppraiseeStrengthsDetails(Convert.ToInt32(decryptedEmployeeId), appraisalYearId, appraisalID, page, rows, out totalCount);
//                }
//                var totalRecords = totalCount;
//                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
//                var jsonData = new
//                {
//                    total = totalPages,
//                    page = page,
//                    records = totalRecords,
//                    rows = objAppraisalDetailModel.AppraiseeStrengthsList,
//                };
//                return Json(jsonData);
//            }
//            catch (Exception)
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
//            }
//        }

//        [HttpPost]
//        public ActionResult SaveStrengthsDetails(AppraiseeStrengths model)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                var status = dal.SaveStrengthsDetails(model);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        public ActionResult DeleteStrengthDetails(int strengthId)
//        {
//            try
//            {
//                AppraisalDAL dal = new AppraisalDAL();
//                tbl_Appraisal_IDFAppraiserAddStrength strengthDetails = new tbl_Appraisal_IDFAppraiserAddStrength();
//                bool eq = dal.DeleteAppraiseeStrengthDetails(strengthId);
//                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult AppraiseeImprovementDetailsLoadGrid(string employeeId, int appraisalID, int appraisalYearId, int page, int rows)
//        {
//            try
//            {
//                string decryptedEmployeeId = employeeId;

//                AppraisalDAL dal = new AppraisalDAL();
//                AppraiseeImprovements objAppraisalDetailModel = new Models.AppraiseeImprovements();
//                objAppraisalDetailModel.AppraiseeImprovementsList = new List<Models.AppraiseeImprovements>();
//                int totalCount = 0;
//                objAppraisalDetailModel.AppraiseeImprovementsList = dal.GetAppraiseeImprovementsDetails(Convert.ToInt32(decryptedEmployeeId), appraisalID, appraisalYearId, page, rows, out totalCount);
//                if ((objAppraisalDetailModel.AppraiseeImprovementsList == null || objAppraisalDetailModel.AppraiseeImprovementsList.Count <= 0) && page - 1 > 0)
//                {
//                    page = page - 1;
//                    objAppraisalDetailModel.AppraiseeImprovementsList = dal.GetAppraiseeImprovementsDetails(Convert.ToInt32(decryptedEmployeeId), appraisalID, appraisalYearId, page, rows, out totalCount);
//                }
//                var totalRecords = totalCount;
//                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
//                var jsonData = new
//                {
//                    total = totalPages,
//                    page = page,
//                    records = totalRecords,
//                    rows = objAppraisalDetailModel.AppraiseeImprovementsList,
//                };
//                return Json(jsonData);
//            }
//            catch (Exception)
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
//            }
//        }

//        [HttpPost]
//        public ActionResult SaveImprovementDetails(AppraiseeImprovements model)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                var status = dal.SaveImprovementsDetails(model);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        public ActionResult DeleteImprovementDetails(int improvementId)
//        {
//            try
//            {
//                AppraisalDAL dal = new AppraisalDAL();
//                tbl_Appraisal_IDFAppraiserAddImprovement strengthDetails = new tbl_Appraisal_IDFAppraiserAddImprovement();
//                bool eq = dal.DeleteAppraiseeImprovementDetails(improvementId);
//                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult AppraiseeTrainingProgramDetailsLoadGrid(string employeeId, int appraisalID, int page, int rows)
//        {
//            try
//            {
//                bool isAuthorize;

//                string decryptedEmployeeId = employeeId;

//                AppraisalDAL dal = new AppraisalDAL();
//                TrainingProgram objAppraisalDetailModel = new Models.TrainingProgram();
//                objAppraisalDetailModel.TrainingProgramList = new List<Models.TrainingProgram>();
//                int totalCount = 0;
//                objAppraisalDetailModel.TrainingProgramList = dal.GetAppraiseeTrainingProgramDetails(Convert.ToInt32(decryptedEmployeeId), appraisalID, page, rows, out totalCount);
//                if ((objAppraisalDetailModel.TrainingProgramList == null || objAppraisalDetailModel.TrainingProgramList.Count <= 0) && page - 1 > 0)
//                {
//                    page = page - 1;
//                    objAppraisalDetailModel.TrainingProgramList = dal.GetAppraiseeTrainingProgramDetails(Convert.ToInt32(decryptedEmployeeId), appraisalID, page, rows, out totalCount);
//                }
//                var totalRecords = totalCount;
//                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
//                var jsonData = new
//                {
//                    total = totalPages,
//                    page = page,
//                    records = totalRecords,
//                    rows = objAppraisalDetailModel.TrainingProgramList,
//                };
//                return Json(jsonData);
//            }
//            catch (Exception)
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
//            }
//        }

//        [HttpPost]
//        public ActionResult SaveTrainingProgramDetails(TrainingProgram model, int AppraisalYearId, int AppraisalId, int EmployeeId)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                model.AppraisalYearId = AppraisalYearId;
//                model.AppraisalId = AppraisalId;
//                model.EmployeeId = EmployeeId;
//                var status = dal.SaveTrainingProgramDetails(model);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        public ActionResult DeleteTrainingDetails(int programId)
//        {
//            try
//            {
//                AppraisalDAL dal = new AppraisalDAL();
//                tbl_Appraisal_IDFAppraiserAddImprovement strengthDetails = new tbl_Appraisal_IDFAppraiserAddImprovement();
//                bool eq = dal.DeleteAppraiseeTrainingProgramDetails(programId);
//                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult SubmitAppraisalStageDetails(int appraisalId, string isMngrOrEmpElement)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                var status = dal.SubmitAppraisalStageDetail(appraisalId, isMngrOrEmpElement);
//                if (status)
//                {
//                    resultMessage = "Saved";
//                }
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }

//        }

//        [HttpPost]
//        public ActionResult SubmitEmployeeStageDetails(AppraisalProcessFourModel model)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                var status = dal.SubmitEmployeeStageDetail(model);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }

//        }

//        public List<FieldChildDetails> GetFieldChildDetailsList(string Field)
//        {
//            try
//            {
//                List<FieldChildDetails> childs = dal.GetFieldChildDetailsList(Field);
//                childs.ToList();
//                return childs;
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        [HttpPost]
//        public ActionResult LoadInboxListGridAppraisal(string term, string Field, string FieldChild, int page, int rows, string employeeId, string TextLink)
//        {
//            List<AppraisalProcessStatus> FinalInboxListDetails = new List<AppraisalProcessStatus>();
//            try
//            {
//                int totalCount;
//                bool isAuthorize;
//                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
//                bool isAuthorizeTextLink;
//                string decryptedtextLink = HRMSHelper.Decrypt(TextLink, out isAuthorizeTextLink);
//                if (!isAuthorizeTextLink)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

//                AppraisalDAL dal = new AppraisalDAL();
//                CommonMethodsDAL commondal = new CommonMethodsDAL();

//                List<AppraisalProcessStatus> InboxListDetails = dal.GetInboxListDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), decryptedtextLink, out totalCount);

//                if ((InboxListDetails == null || InboxListDetails.Count <= 0) && page - 1 > 0)
//                {
//                    page = page - 1;
//                    InboxListDetails = dal.GetInboxListDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), decryptedtextLink, out totalCount);
//                }

//                foreach (var item in InboxListDetails)
//                {
//                    string ExitId = commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.AppraisalId), true);
//                    item.EncryptedAppraisalId = ExitId;
//                    string encryptedEmployeeId = commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.EmployeeId), true);
//                    item.EncryptedEmployeeId = encryptedEmployeeId;
//                    FinalInboxListDetails.Add(item);
//                }

//                var totalRecords = totalCount;
//                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
//                var jsonData = new
//                {
//                    total = totalPages,
//                    page = page,
//                    records = totalRecords,
//                    rows = FinalInboxListDetails
//                };

//                return Json(jsonData);
//            }
//            catch (Exception)
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
//            }
//        }

//        [HttpPost]
//        public ActionResult LoadWatchListGridAppraisal(string term, string Field, string FieldChild, int page, int rows, string employeeId, string TextLink)
//        {
//            List<AppraisalProcessStatus> FinalWatchListDetails = new List<AppraisalProcessStatus>();
//            try
//            {
//                int totalCount;
//                bool isAuthorize;
//                CommonMethodsDAL commondal = new CommonMethodsDAL();
//                AppraisalDAL dal = new AppraisalDAL();

//                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
//                bool isAuthorizeTextLink;
//                string decryptedtextLink = HRMSHelper.Decrypt(TextLink, out isAuthorizeTextLink);
//                if (!isAuthorizeTextLink)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

//                List<AppraisalProcessStatus> WatchListDetails = dal.GetWatchListDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), decryptedtextLink, out totalCount);
//                if ((WatchListDetails == null || WatchListDetails.Count <= 0) && page - 1 > 0)
//                {
//                    page = page - 1;
//                    WatchListDetails = dal.GetWatchListDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), decryptedtextLink, out totalCount);
//                }
//                foreach (var item in WatchListDetails)
//                {
//                    string ExpenseId = commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.AppraisalId), true);
//                    item.EncryptedAppraisalId = ExpenseId;
//                    string encryptedEmployeeId = commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.EmployeeId), true);
//                    item.EncryptedEmployeeId = encryptedEmployeeId;
//                    FinalWatchListDetails.Add(item);
//                }
//                var totalRecords = totalCount;
//                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
//                var jsonData = new
//                {
//                    total = totalPages,
//                    page = page,
//                    records = totalRecords,
//                    rows = FinalWatchListDetails
//                };

//                return Json(jsonData);
//            }
//            catch (Exception)
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
//            }

//        }
//        [HttpPost]
//        public ActionResult SaveValueDriverInfoAppraisal(List<AppraisalParameter> item)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                var status = dal.SaveParameterDetailsAppraisal(item);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }

//        }

//        [HttpPost]
//        public ActionResult ShowStatusDetails(string AppraisalID, int page, int rows)
//        {
//            try
//            {
//                AppraisalDAL appraisalDAL = new AppraisalDAL();

//                int totalCount;
//                bool isAuthorize = true;
//                string decryptedAppraisalID = HRMSHelper.Decrypt(AppraisalID, out isAuthorize);
//                if (!isAuthorize)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

//                List<AppraisalProccessShowStatus> ShowStatusResult = new List<AppraisalProccessShowStatus>();
//                ShowStatusResult = appraisalDAL.GetShowStatusResult(page, rows, Convert.ToInt32(decryptedAppraisalID), out totalCount);

//                if ((ShowStatusResult == null || ShowStatusResult.Count <= 0) && page - 1 > 0)
//                {
//                    page = page - 1;
//                    ShowStatusResult = appraisalDAL.GetShowStatusResult(page, rows, Convert.ToInt32(decryptedAppraisalID), out totalCount);
//                }
//                var totalRecords = totalCount;
//                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
//                var jsonData = new
//                {
//                    total = totalPages,
//                    page = page,
//                    records = totalRecords,
//                    rows = ShowStatusResult,
//                };
//                return Json(jsonData);
//            }
//            catch (Exception)
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
//            }
//        }

//        public ActionResult DeleteCorporateDetailsAppraisal(int corporateId)
//        {
//            try
//            {
//                AppraisalDAL dal = new AppraisalDAL();
//                tbl_Appraisal_CorporateContribution projectAchievementDetails = new tbl_Appraisal_CorporateContribution();
//                bool eq = dal.DeleteCorporateAppraisalDetails(corporateId);
//                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        public ActionResult DownloadAddApprisalUploadedFile(int AppraisalID, int RequirementID)
//        {
//            try
//            {
//                AppraisalDAL DAL = new AppraisalDAL();
//                ProjectAchievementAppraisal Details = DAL.GetAddApprisalDetailsShowHistory(AppraisalID, RequirementID);
//                string FileName = Details.FileName;
//                string[] FileExtention = FileName.Split('.');
//                string contentType = "application/" + FileExtention[1];
//                string Filepath = Path.Combine(Details.FilePath, FileName);
//                return File(Filepath, contentType, FileName);
//            }
//            catch (Exception)
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
//            }
//        }
//        /// <summary>
//        /// Updated the stage and other employee details of the appraiser form section 1.
//        /// </summary>
//        /// <param name="appraiserId"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public ActionResult ApproveAppraiserForm(int appraiserId, int EmployeeId, string StausApprover, string isMngrOrEmpElement, string appraiseeComments, bool? isAppraiseeAgree, string strengthComments, string improvementComments)
//        {
//            try
//            {
//                AppraisalDAL appraisalDal = new AppraisalDAL();
//                EmployeeDAL employeeDAL = new EmployeeDAL();
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                var status = dal.ApproveAppraisal(Convert.ToInt32(appraiserId), EmployeeId, StausApprover, isMngrOrEmpElement, appraiseeComments, strengthComments, improvementComments);
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                AppraisalProcessModel _AppDetails = appraisalDal.getAppriasalDetails(appraiserId);
//                int? stageID = _AppDetails.StageID.HasValue ? _AppDetails.StageID.Value : 0;
//                int? Apparisal1 = _AppDetails.Appriser1Id.HasValue ? _AppDetails.Appriser1Id.Value : 0;
//                int? Apparisal2 = _AppDetails.Appriser2Id.HasValue ? _AppDetails.Appriser2Id.Value : 0;
//                int? Reviwer1 = _AppDetails.Reviwer1Id.HasValue ? _AppDetails.Reviwer1Id.Value : 0;
//                int? Reviwer2 = _AppDetails.Reviwer2Id.HasValue ? _AppDetails.Reviwer2Id.Value : 0;
//                int? GroupHead = _AppDetails.GroupHeadId.HasValue ? _AppDetails.GroupHeadId.Value : 0;
//                int? EmpEmployeeId = _AppDetails.EmployeeID.HasValue ? _AppDetails.EmployeeID.Value : 0;

//                HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetails(Apparisal1.HasValue ? Apparisal1.Value : 0);
//                var appraiser1Name = employee.EmployeeName;
//                tbl_Appraisal_StageEvents _latestEntry = appraisalDal.getStageEventlatestEntry(appraiserId);

//                int? fromStageID = _latestEntry.FromStageId;
//                int? toStageID = _latestEntry.ToStageId;
//                int? userID = _latestEntry.USerId;

//                string EmpApprisalId = appraiserId.ToString();

//                return Json(new { results = resultMessage, status = status, ApprisalId = EmpApprisalId, employeeID = EmpEmployeeId, stageID = stageID, Apparisal1 = Apparisal1, Apparisal2 = Apparisal2, Reviwer1 = Reviwer1, Reviwer2 = Reviwer2, GroupHead = GroupHead, fromStageID = fromStageID, toStageID = toStageID, userID = userID, appraiser1Name = appraiser1Name }, JsonRequestBehavior.AllowGet);

//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult SubmitIdfEmployeeStageDetails(int appraisalId, string appraiseeComments, bool isAppraiseeAgree, int nextStageId)
//        {
//            try
//            {
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string result = string.Empty;
//                var status = dal.SubmitIdfEmployeeStageDetails(appraisalId, appraiseeComments, isAppraiseeAgree, nextStageId);
//                return Json(new { results = result, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult GetModelListRating(int confrmationId, int empelement)
//        {
//            AppraisalProcessModel model = new AppraisalProcessModel { rating = new RatingApprMinMax() };
//            AppraisalDAL appraisalDal = new AppraisalDAL();
//            int AppraisalYearID = 0;
//            List<AppraisalParameter> paramListAppraisal = appraisalDal.GetParametersDetails(confrmationId, empelement, AppraisalYearID);
//            return Json(new { AppID = paramListAppraisal }, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public ActionResult SaveComments(string ApprisalId, string comments, string commenttype)
//        {
//            AppraisalDAL appraisalDal = new AppraisalDAL();
//            try
//            {
//                bool isAuthorize = true;
//                string decryptedAppraisalID = HRMSHelper.Decrypt(ApprisalId, out isAuthorize);

//                string resultMessage = string.Empty;

//                var status = appraisalDal.SaveCommentDetails(Convert.ToInt32(decryptedAppraisalID), comments, commenttype);

//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        public ActionResult DeleteAllAppraisalDetails(string ApprisalId, string EmployeeId, string comments)
//        {
//            try
//            {
//                AppraisalDAL appraisalDal = new AppraisalDAL();
//                bool isAuthorize = true;
//                string decryptedAppraisalID = HRMSHelper.Decrypt(ApprisalId, out isAuthorize);
//                string decryptedEmployeeID = HRMSHelper.Decrypt(EmployeeId, out isAuthorize);
//                AppraisalProcessModel _AppDetails = appraisalDal.getAppriasalDetails(Convert.ToInt32(decryptedAppraisalID));
//                int? stageID = _AppDetails.StageID;
//                int? Apparisal1 = _AppDetails.Appriser1Id;
//                int? Apparisal2 = _AppDetails.Appriser2Id;
//                int? Reviwer1 = _AppDetails.Reviwer1Id;
//                int? Reviwer2 = _AppDetails.Reviwer2Id;
//                int? GroupHead = _AppDetails.GroupHeadId;
//                int? EmpEmployeeId = _AppDetails.EmployeeID;

//                bool status = appraisalDal.DeletedAllAppriasalDetails(Convert.ToInt32(decryptedAppraisalID), decryptedEmployeeID, comments);
//                string EmpApprisalId = Commondal.Encrypt(Session["SecurityKey"].ToString() + ApprisalId, true);
//                return Json(new { status = status, ApprisalId = EmpApprisalId, employeeID = EmpEmployeeId, stageID = stageID, Apparisal1 = Apparisal1, Apparisal2 = Apparisal2, Reviwer1 = Reviwer1, Reviwer2 = Reviwer2, GroupHead = GroupHead, comments = comments }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult CheckAppraisalCancledStatus(string EmpApprisalId)
//        {
//            try
//            {
//                bool isAuthorize = true;
//                string decryptedAppraisalID = HRMSHelper.Decrypt(EmpApprisalId, out isAuthorize);
//                if (!isAuthorize)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
//                DAL.AppraisalDAL dal = new DAL.AppraisalDAL();
//                string resultMessage = string.Empty;
//                var status = dal.CheckCancleStatus(Convert.ToInt32(decryptedAppraisalID));
//                if (status)
//                    resultMessage = "Saved";
//                else
//                    resultMessage = "Error";

//                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult UnfreezeFreezeAppraisalProcess(string encryptedAppraisalId, string encryptedEmployeeId, bool isUnfreezedOrFreezed)
//        {
//            try
//            {
//                AppraisalDAL appraisalDal = new AppraisalDAL();
//                AppraisalProcessResponse response = new AppraisalProcessResponse();
//                response.isAdded = false;
//                bool isAuthorize;
//                string decryptedAppraisalId = HRMSHelper.Decrypt(encryptedAppraisalId, out isAuthorize);
//                if (!isAuthorize)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
//                bool isAuthorizeEmployee;
//                string decryptedEmployeeId = HRMSHelper.Decrypt(encryptedEmployeeId, out isAuthorizeEmployee);
//                if (!isAuthorizeEmployee)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
//                int appraisalId = Convert.ToInt16(decryptedAppraisalId);
//                int employeeId = Convert.ToInt16(decryptedEmployeeId);

//                response = appraisalDal.AppraisalProcessUnfreezeFreeze(appraisalId, employeeId, isUnfreezedOrFreezed);
//                return Json(new { isAdded = response.isAdded, isUnfreezed = response.isUnfreezed, isFreezed = response.isFreezed }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpGet]
//        public ActionResult GetGroupHeadRatingsHistory(string encryptedAppraisalId, string encryptedEmployeeId)
//        {
//            try
//            {
//                bool isAuthorize;
//                string decryptedAppraisalId = HRMSHelper.Decrypt(encryptedAppraisalId, out isAuthorize);
//                if (!isAuthorize)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
//                bool isAuthorizeEmployee;
//                string decryptedEmployeeId = HRMSHelper.Decrypt(encryptedEmployeeId, out isAuthorizeEmployee);
//                if (!isAuthorizeEmployee)
//                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
//                int appraisalId = Convert.ToInt16(decryptedAppraisalId);

//                AppraisalProcessModel appraisalModel = new AppraisalProcessModel();
//                AppraisalDAL appraisalDal = new AppraisalDAL();
//                appraisalModel.GroupHeadHistoryDetailsList = new List<ViewGroupHeadHistoryModel>();
//                appraisalModel.GroupHeadHistoryDetailsList = appraisalDal.GetGroupHeadRatingsHistoryList(appraisalId);
//                return PartialView("_ViewGroupHeadRatingHistory", appraisalModel);
//            }
//            catch (Exception)
//            {
//                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
//            }
//        }

//        [HttpPost]
//        public ActionResult IsFormSumitedorNot(string isMngrOrEmpElement, int appraisalID)
//        {
//            try
//            {
//                AppraisalDAL appraisalDal = new AppraisalDAL();
//                EmployeeDAL DAL = new EmployeeDAL();
//                bool status;
//                string EmployeeCode = Membership.GetUser().UserName;
//                int LoggedInEmployeeId = DAL.GetEmployeeID(EmployeeCode);
//                status = appraisalDal.CheckFormSumitedStatus(isMngrOrEmpElement, appraisalID, LoggedInEmployeeId);
//                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception)
//            {
//                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [AcceptVerbs(HttpVerbs.Post)]
//        public ActionResult UploadDesiganationDetails(HttpPostedFileBase doc, AppraiseeUploadDestnModel model)
//        {
//            bool uploadStatus = false;
//            string connectionString = "";
//            if (doc.ContentLength > 0)
//            {
//                string extension = System.IO.Path.GetExtension(doc.FileName);
//                string uploadsPath = HttpContext.Server.MapPath(UploadDesgnatinFileLocation);

//                if (!Directory.Exists(uploadsPath))
//                    Directory.CreateDirectory(uploadsPath);

//                string filePath = Path.Combine(uploadsPath, System.IO.Path.GetFileName(doc.FileName));
//                doc.SaveAs(filePath);

//                if (extension == ".xls")
//                {
//                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
//                }
//                else if (extension == ".xlsx")
//                {
//                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
//                }

//                OleDbConnection excelConnection = new OleDbConnection(connectionString);
//                OleDbCommand cmd = new OleDbCommand("Select [Employee Code],[Year],[Month],[Grade],[Level],[Designation],[Role Description],[Joining Designation] from [Designation details$]", excelConnection);
//                //OleDbCommand cmd = new OleDbCommand("Select [Employee Code],[Reporting Manager Code],[Competency Manager Code],[Confirmation / Exit Process Manager Code] from [Sheet2$]", excelConnection);

//                excelConnection.Open();

//                OleDbDataReader dReader;
//                dReader = cmd.ExecuteReader();

//                DataSet ds = new DataSet();
//                DataTable dt = new DataTable();

//                dt.Load(dReader);
//                ds.Tables.Add(dt);
//                string EmployeeCode = model.LoggedInEmplyeeId;
//                AppraisalDAL appraisalDal = new AppraisalDAL();

//                appraisalDal.GetExcelData(ds, EmployeeCode);
//                uploadStatus = true;

//                excelConnection.Close();
//            }

//            return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
//        }

//        public ActionResult UploadDesiganations()
//        {
//            AppraiseeUploadDestnModel model = new AppraiseeUploadDestnModel();
//            model.SearchedUserDetails = new SearchedUserDetails();
//            string EmployeeCode = Membership.GetUser().UserName;
//            ViewBag.LoggedinEmpCode = EmployeeCode;
//            return View("ApprisalDesignationUpload", model);
//        }

//    }
//}