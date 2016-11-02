using HRMS.DAL;
using HRMS.Helper;
using HRMS.Models;
using HRMS.Models.Enums;
using log4net;
using MvcApplication3.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    /// <summary>
    /// This controller contains actions like Search Employee
    /// </summary>
    ///

    public class EmployeeDetailsController : Controller
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();
        private Random incident = new Random();

        public string UploadFileLocation
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadEmployeeFileLocation"];
            }
        }

        public string UploadPassportLocation
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadPassportFileLocation"];
            }
        }

        public string UploadVisaLocation
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadVisaFileLocation"];
            }
        }

        private int pageSize = 10;
        private HRMSDBEntities dbContext = new HRMSDBEntities();

        /// <summary>
        /// Redirect to Index View of Search Employee.
        /// </summary>
        /// <returns>Returns the EmployeeSearchResulViewtModel model to Index view of Employee Controller</returns>
        [HttpGet]
        [Authorize]
        public ActionResult Index(string employeeId)
        {
            EmployeeDetails model = new EmployeeDetails();
            try
            {
                string decryptedEmployeeId = string.Empty;
                EmployeeDAL employeeDAL = new EmployeeDAL();
                string employeeCode = Membership.GetUser().UserName;
                int employeeID = employeeDAL.GetEmployeeID(employeeCode);
                Session["SearchEmpID"] = employeeId;
                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedEmployeeId = Convert.ToString(employeeID);
                model.SearchedUserDetails = new SearchedUserDetails();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                model.SearchedUserDetails.UserRole = user;
                string userRole = user;

                string encryptedLoggedinEmployeeID = Convert.ToString(Session["encryptedLoggedinEmployeeID"]);
                if (encryptedLoggedinEmployeeID == "")
                    return RedirectToAction("LogOn", "Account");
                string decryptedLoggedinEmployeeID = Commondal.Decrypt(Convert.ToString(encryptedLoggedinEmployeeID), true);
                decryptedLoggedinEmployeeID = decryptedLoggedinEmployeeID.Replace(Session["SecurityKey"].ToString(), "");
                if (decryptedEmployeeId == "0")
                    decryptedEmployeeId = decryptedLoggedinEmployeeID;

                ViewBag.EmployeeId = employeeId;

                int? decryptedemployeeId = Convert.ToInt32(decryptedEmployeeId);

                if (decryptedemployeeId == null || decryptedemployeeId <= 0)
                    return RedirectToAction("Index", "PersonalDetails");
                else
                {
                    model.EmployeeId = decryptedemployeeId.HasValue ? decryptedemployeeId.Value : 0;

                    //EmployeeDAL employeeDAL = new EmployeeDAL();
                    HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(decryptedemployeeId.HasValue ? decryptedemployeeId.Value : 0);
                    if (employeeDetails != null && employeeDetails.EmployeeID > 0)
                    {
                        model.SearchedUserDetails.EmployeeId = decryptedemployeeId.HasValue ? decryptedemployeeId.Value : 0;
                        model.SearchedUserDetails.EmployeeFullName = employeeDetails.EmployeeName;
                        model.SearchedUserDetails.EmployeeCode = employeeDetails.EmployeeCode;
                        Session["SearchEmpFullName"] = employeeDetails.EmployeeName;
                        Session["SearchEmpCode"] = employeeDetails.EmployeeCode;
                    }
                    //return View(model);
                    return RedirectToAction("EmployeeDetails", "EmployeeDetails", new { employeeId = employeeId });
                }
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        /// <summary>
        /// This action takes EmployeeSearchResulViewtModel as input and gives Employee list which satisfies the search criteria.
        /// </summary>
        /// <param name="model">Takes form values for searching particular Employee</param>
        /// <returns>Returns list of Employee(Name and Employee Code) which matches with the search criteria</returns>
        [HttpPost]
        public ActionResult Index(EmployeeDetails model)
        {
            return View(model);
        }

        [HttpGet]
        public ActionResult SearchEmployee()
        {
            try
            {
                string searchText = string.Empty;
                EmployeeSearchResulViewtModel model = new EmployeeSearchResulViewtModel();
                model.SearchText = searchText;
                model.PageSize = pageSize;
                model.PageNo = 1;
                model.TotalPages = 0;
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.EmployeeDetailsList = new List<EmployeeDetails>();
                model.SearchedUserDetails = new SearchedUserDetails();
                return View(model);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        [HttpPost]
        public ActionResult SearchEmployee(EmployeeSearchResulViewtModel model)
        {
            try
            {
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                string userRole = user;
                model.UserRole = userRole;
                if (userRole == UserRoles.RMG)
                    model.UserRole = UserRoles.RMG;

                if (model.PageNo <= 0)
                {
                    model.PageNo = 1;
                }
                if (model.PageSize <= 0)
                {
                    model.PageSize = pageSize;
                }
                if (model.SearchText == null)
                    model.SearchText = "";
                EmployeeDAL employeeDAL = new EmployeeDAL();
                int totalCount = employeeDAL.SearchEmployeeTotalCount(model.SearchText);
                model.TotalPages = (int)Math.Ceiling((double)totalCount / (double)model.PageSize);
                model.EmployeeDetailsList = GetEncryptedlist(employeeDAL.SearchEmployee(model.SearchText, model.PageNo, model.PageSize));
                model.SearchText = model.SearchText;
                SemDAL semdal = new SemDAL();
                Session["SearchEmpIDForSkill"] = semdal.geteEmployeeIDFromSEMDatabase(model.EmployeeDetailsList.FirstOrDefault().EmployeeCode.ToString());

                #region Get Logged In User Details

                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.UserRole = model.UserRole;
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(model.EmployeeId);
                if (employeeDetails != null)
                {
                    model.SearchedUserDetails.EmployeeId = model.EmployeeId;
                    model.SearchedUserDetails.EncryptedEmployeeId = model.EncryptedEmployeeId;
                    model.SearchedUserDetails.EmployeeFullName = employeeDetails.EmployeeName;
                    model.SearchedUserDetails.EmployeeCode = employeeDetails.EmployeeCode;
                    Session["SearchEmpFullName"] = employeeDetails.EmployeeName;
                    Session["SearchEmpCode"] = employeeDetails.EmployeeCode;
                }

                #endregion Get Logged In User Details

                return View(model);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        public List<EmployeeDetails> GetEncryptedlist(List<EmployeeDetails> SearchedEmpList)
        {
            foreach (EmployeeDetails emp in SearchedEmpList)
            {
                emp.EncryptedEmployeeId = Commondal.Encrypt(Convert.ToString(Session["SecurityKey"].ToString() + emp.EmployeeId), true);
            }
            return SearchedEmpList;
        }

        /// <summary>
        /// Returns list of Employee whose name or employee code matches with the input parameter. This action method used to get autosuggest result from jQuery
        /// </summary>
        /// <param name="term">The string that we have to search with Employee Name or Employee Code</param>
        /// <returns>Gives list of Emplyee Name along iwth Employee Code that we found in search result</returns>
        public ActionResult SearchEmployeeAutoSuggest(string term)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();
            List<EmployeeDetails> searchResult = new List<EmployeeDetails>();
            searchResult = employeeDAL.SearchEmployee(term, 1, 20);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchEmployeeAutoSuggestFoeSEM(string term)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();
            List<EmployeeDetails> searchResult = new List<EmployeeDetails>();
            searchResult = employeeDAL.SearchEmployeeForSEM(term, 1, 20);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchEmployeeForRecruiter(string term)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();
            List<EmployeeDetails> searchResult = new List<EmployeeDetails>();
            searchResult = employeeDAL.SearchEmployeeForRecruiter(term, 1, 20);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// returns list of designations
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult SearchDesignation(string term)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();
            List<Designations> searchResult = new List<Designations>();
            searchResult = employeeDAL.GetDesignations(term);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to retrieve Employee's Designation details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PageAccess(PageName = "Designation")]
        public ActionResult DesignationDetails(string employeeId)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                string employeeCode = Membership.GetUser().UserName;
                int employeeID = employeeDAL.GetEmployeeID(employeeCode);
                string decryptedEmployeeId = string.Empty;
                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                {
                    ViewBag.EmployeeId = employeeId;
                    decryptedEmployeeId = Convert.ToString(employeeID);
                }
                DesignationDetailsViewModel model = new DesignationDetailsViewModel();
                EmployeeDAL dal = new EmployeeDAL();
                int joiningdt = Convert.ToDateTime(dal.GetEmployeeJoiningDate(Convert.ToInt32(decryptedEmployeeId))).Year;
                int currentyear = DateTime.Now.Year;
                int joinMonth = Convert.ToDateTime(dal.GetEmployeeJoiningDate(Convert.ToInt32(decryptedEmployeeId))).Month;
                List<int> listOfYears = new List<int>();

                //for filling dropdownlist of year rom joinning date to current year
                for (int i = joiningdt; i <= currentyear; i++)
                {
                    listOfYears.Add(joiningdt);
                    joiningdt++;
                }
                ViewBag.ListOfYears = listOfYears;
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                model.UserRole = user;
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                model.EmpStatusMasterID = personalDAL.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                model.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                model.NewDesignation = new DesignationDetails();
                model.NewDesignation.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                ViewBag.designationEmployeeId = employeeId;
                List<SelectListItem> months = new List<SelectListItem>();
                months = model.NewDesignation.GetMonths();
                List<SelectListItem> joinMonths = new List<SelectListItem>();
                for (int i = 0; i < months.Count; i++)
                {
                    joinMonths.Add(months[i]);
                }
                model.NewDesignation.JoiningMonth = joinMonths;
                ViewBag.ListofMonths = joinMonths;
                ViewBag.GradeList = new SelectList(dal.GetGradeList(), "GradeId", "GradeName");

                //Need to change for level
                ViewBag.LevelList = new SelectList(dal.GetGradeList(), "GradeId", "GradeId");
                return PartialView("_DesignationDetails", model);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult LoadDesignationDetails(int page, int rows, string employeeId)
        {
            try
            {
                EmployeeDAL dal = new EmployeeDAL();

                DesignationDetailsViewModel model = new DesignationDetailsViewModel();
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                model.DesignationDetailsList = dal.GetEmployeePreviousDesignationDetails(page, rows, Convert.ToInt32(decryptedEmployeeId));

                if ((model.DesignationDetailsList == null || model.DesignationDetailsList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model.DesignationDetailsList = dal.GetEmployeePreviousDesignationDetails(page, rows, Convert.ToInt32(decryptedEmployeeId));
                }

                int totalCount = model.DesignationDetailsList.Count;

                var totalRecords = totalCount;//totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = model.DesignationDetailsList
                };

                return Json(jsonData);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult DesignationDetails(DesignationDetails model, int EmployeeId, int? YearId, string Month, string Level, int? GradeId)
        {
            try
            {
                Designations response = new Designations();
                model.EmployeeId = EmployeeId;

                if (ModelState.IsValid == false)
                {
                    return Json(new { status = false }, JsonRequestBehavior.AllowGet);
                }

                if (EmployeeId != null)
                {
                    model.EmployeeId = EmployeeId;
                    model.Year = YearId.HasValue ? YearId.Value : 0;
                    model.Month = Month;
                    model.Level = Level;
                    model.GradeId = GradeId.HasValue ? GradeId.Value : 0;
                }
                EmployeeDAL dal = new EmployeeDAL();
               // response = dal.SaveDesignationDetails(model);
                response.isAdded = true;
                response.isValidMonth = true;
                response.isValidEntry = true;
                return Json(new { status = response.isAdded, isValidMonth = response.isValidMonth, isValidEntry = response.isValidEntry }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckDesignation(string Designation)
        {
            EmployeeDAL dal = new EmployeeDAL();
            var isExists = dal.Checkdesignation(Designation);
            return Json(isExists, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckJoiningDesignation(string JoiningDesignation)
        {
            EmployeeDAL dal = new EmployeeDAL();
            var isExists = dal.Checkdesignation(JoiningDesignation);
            return Json(new { isExists = isExists }, JsonRequestBehavior.AllowGet);
            //return Json(isExistsz, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [PageAccess(PageName = "Passport")]
        public ActionResult TravelDetails(string employeeId)
        {
            try
            {
                string decryptedEmployeeId = string.Empty;
                EmployeeDAL employeeDAL = new EmployeeDAL();
                string employeeCode = Membership.GetUser().UserName;
                int employeeID = employeeDAL.GetEmployeeID(employeeCode);
                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                {
                    decryptedEmployeeId = Convert.ToString(employeeID);
                }

                TravelDetailsViewModel travelDetailsViewModel = new TravelDetailsViewModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                travelDetailsViewModel.UserRole = user;
                ViewBag.EmployeeId = employeeId;
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                travelDetailsViewModel.EmpStatusMasterID = personalDAL.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                travelDetailsViewModel.checkSpouseDetails = employeeDAL.IsSpouseDetailsPresent(Convert.ToInt32(decryptedEmployeeId));
                travelDetailsViewModel.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                //return RedirectToAction("PersonalTravelDetails", "EmployeeDetails", new { employeeId = employeeId });
                return PartialView("_TravelDetails", travelDetailsViewModel);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        [HttpPost]
        public ActionResult DeletePassportDocument(int DocumentID, TravelDetailsPerson PersonType)
        {
            try
            {
                bool status = false;
                EmployeeDAL DAL = new EmployeeDAL();
                status = DAL.DeletePassportDetails(DocumentID, PersonType);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TravelDetails(HttpPostedFileBase doc, TravelDetailsViewModel model)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                bool result = false;
                if (doc != null)
                {
                    if (doc.ContentLength > 0)
                    {
                        string uploadsPath = (UploadPassportLocation);
                        string uploadpathwithId = Path.Combine(uploadsPath, (model.EmployeeId).ToString());
                        uploadsPath = Path.Combine(uploadpathwithId, Convert.ToString(model.PassportNumber));
                        string fileName = Path.GetFileName(doc.FileName);
                        model.PassportFileName = fileName;
                        model.PassportFilePath = uploadsPath;
                        TravelDAL DAL = new TravelDAL();
                        result = employeeDAL.SavePassportDetails(model, model.PersonType);

                        string filePath = Path.Combine(uploadsPath, fileName);
                        if (!Directory.Exists(uploadsPath))
                            Directory.CreateDirectory(uploadsPath);

                        doc.SaveAs(filePath);
                    }
                }
                else
                {
                    result = true;
                }
                if (employeeDAL.AddUpdatePassportDetails(model, model.PersonType) && result == true)
                {
                    return Json(new { status = true }, "text/html", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false }, "text/html", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return Json(new { status = "Error" }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EmployeePassportLoadGrid(int page, int rows, int EmployeeID)
        {
            try
            {
                EmployeeDAL empdal = new EmployeeDAL();
                TravelDetailsViewModel Passport = new TravelDetailsViewModel();
                int totalCount;
                int LoggedInEmployeeId = empdal.GetEmployeeID(Membership.GetUser().UserName);

                List<TravelDetailsViewModel> passportDetails = empdal.GetEmployeePassportDetails(page, rows, EmployeeID, out totalCount);
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);

                if ((passportDetails == null) && page - 1 > 0)
                {
                    page = page - 1;
                    passportDetails = empdal.GetEmployeePassportDetails(page, rows, EmployeeID, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = passportDetails,
                };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SpousePassportLoadGrid(int page, int rows, int EmployeeID)
        {
            try
            {
                EmployeeDAL empdal = new EmployeeDAL();
                TravelDetailsViewModel Passport = new TravelDetailsViewModel();
                int totalCount;
                int LoggedInEmployeeId = empdal.GetEmployeeID(Membership.GetUser().UserName);

                List<TravelDetailsViewModel> passportDetails = empdal.GetSpousePassportDetails(page, rows, EmployeeID, out totalCount);
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);

                if ((passportDetails == null) && page - 1 > 0)
                {
                    page = page - 1;
                    passportDetails = empdal.GetSpousePassportDetails(page, rows, EmployeeID, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = passportDetails,
                };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult DownloadPassportFile(string PassportFileName, int DocumentID, int EmployeeID, TravelDetailsPerson PersonType)
        {
            try
            {
                EmployeeDAL DAL = new EmployeeDAL();
                TravelDetailsViewModel Details = DAL.GetPassportShowHistory(EmployeeID, DocumentID, PersonType);
                string[] FileExtention = PassportFileName.Split('.');
                string contentType = "application/" + FileExtention[1];
                string Filepath = Path.Combine(Details.PassportFilePath, PassportFileName);
                return File(Filepath, contentType, PassportFileName);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult DownloadVisaFile(string VisaFileName, int EmployeeVisaId, int EmployeeID, TravelDetailsPerson PersonType)
        {
            try
            {
                EmployeeDAL DAL = new EmployeeDAL();
                VisaDetailsViewModel Details = DAL.GetVisaShowHistory(EmployeeID, EmployeeVisaId, PersonType);
                string[] FileExtention = VisaFileName.Split('.');
                string contentType = "application/" + FileExtention[1];
                string Filepath = Path.Combine(Details.VisaFilePath, VisaFileName);
                return File(Filepath, contentType, VisaFileName);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult showPassportDetails(int EmployeeID, int DocumentID, TravelDetailsPerson PersonType)
        {
            try
            {
                TravelDetailsViewModel model = new TravelDetailsViewModel();
                EmployeeDAL DAL = new EmployeeDAL();

                TravelDetailsViewModel employeeDetails = DAL.GetPassportShowHistory(EmployeeID, DocumentID, PersonType);
                model.PassportFileName = employeeDetails.PassportFileName;
                model.CreatedDate = employeeDetails.CreatedDate;
                model.DocumentID = employeeDetails.DocumentID;
                model.EmployeeId = EmployeeID;
                model.PersonType = PersonType;
                return PartialView("_EmployeePassportDetails", model);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult ShowVisaDetails(int EmployeeID, int EmployeeVisaId, TravelDetailsPerson PersonType)
        {
            try
            {
                VisaDetailsViewModel model = new VisaDetailsViewModel();
                EmployeeDAL DAL = new EmployeeDAL();

                VisaDetailsViewModel employeeDetails = DAL.GetVisaShowHistory(EmployeeID, EmployeeVisaId, PersonType);
                model.VisaFileName = employeeDetails.VisaFileName;
                model.CreatedDate = employeeDetails.CreatedDate;
                model.EmployeeVisaId = EmployeeVisaId;
                model.EmployeeId = EmployeeID;
                model.PersonType = PersonType;
                return PartialView("_EmployeeVisaDetails", model);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult PersonalTravelDetails(string employeeId, TravelDetailsPerson? type)
        {
            try
            {
                string decryptedEmployeeId = string.Empty;
                EmployeeDAL employeeDAL = new EmployeeDAL();
                string employeeCode = Membership.GetUser().UserName;
                int employeeID = employeeDAL.GetEmployeeID(employeeCode);
                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                {
                    decryptedEmployeeId = Convert.ToString(employeeID);
                }

                ViewBag.EmployeeId = employeeId;
                ViewBag.decryptedSpouseEmployeeId = decryptedEmployeeId;

                ViewBag.loggedInEmployeeId = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                TravelDetailsViewModel travelDetailsViewModel = new TravelDetailsViewModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                travelDetailsViewModel.UserRole = user;
                ViewBag.UserRole = user;
                travelDetailsViewModel.EmployeeId = Convert.ToInt32(decryptedEmployeeId);

                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();

                int EmpStatusMasterID = personalDAL.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                travelDetailsViewModel = employeeDAL.GetPassportDetails(Convert.ToInt32(decryptedEmployeeId), type.HasValue ? type.Value : TravelDetailsPerson.Own);

                if (travelDetailsViewModel == null)
                    travelDetailsViewModel = new TravelDetailsViewModel();
                travelDetailsViewModel.PersonType = type.HasValue ? type.Value : TravelDetailsPerson.Own;

                travelDetailsViewModel.VisaDetailsModel = new VisaDetailsViewModel();
                travelDetailsViewModel.VisaDetailsModel.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                travelDetailsViewModel.VisaDetailsModel.CountryList = employeeDAL.GetCountryList().ToDictionary(m => m.CountryId, m => m.CountryName);
                travelDetailsViewModel.VisaDetailsModel.EmpStatusMasterID = EmpStatusMasterID;
                travelDetailsViewModel.EmpStatusMasterID = EmpStatusMasterID;
                travelDetailsViewModel.VisaDetailsModel.visatypeList = employeeDAL.GetVisaTypeDetails();

                travelDetailsViewModel.EmpStatusMasterID = personalDAL.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                travelDetailsViewModel.checkSpouseDetails = employeeDAL.IsSpouseDetailsPresent(Convert.ToInt32(decryptedEmployeeId));

                if (travelDetailsViewModel.PersonType == TravelDetailsPerson.Own)
                    return PartialView("_PersonalTravelDetails", travelDetailsViewModel);
                else
                {
                    travelDetailsViewModel.VisaDetailsModel.EmpStatusMasterID = EmpStatusMasterID;
                    return PartialView("_SpousePersonalTravelDetails", travelDetailsViewModel);
                }
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult AddUpdateVisaDetails(HttpPostedFileBase doc, VisaDetailsViewModel model)
        public ActionResult AddUpdateVisaDetails(HttpPostedFileBase doc, TravelDetailsViewModel model)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                bool result = false;
                if (doc != null)
                {
                    if (doc.ContentLength > 0)
                    {
                        string uploadsPath = (UploadVisaLocation);
                        uploadsPath = Path.Combine(uploadsPath, (model.VisaDetailsModel.EmployeeId).ToString());
                        string fileName = Path.GetFileName(doc.FileName);
                        model.VisaDetailsModel.VisaFileName = fileName;
                        model.VisaDetailsModel.VisaFilePath = uploadsPath;
                        //model.VisaDetailsModel.PersonType = model.PersonType;
                        TravelDAL DAL = new TravelDAL();
                        result = employeeDAL.AddUpdateVisaDetails(model.VisaDetailsModel);

                        string filePath = Path.Combine(uploadsPath, fileName);
                        if (!Directory.Exists(uploadsPath))
                            Directory.CreateDirectory(uploadsPath);

                        doc.SaveAs(filePath);
                    }
                }
                return Json(new { status = result }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return Json(new { status = "Error" }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult checkValidEmployeeVisaDetails(int employeeId, int countryId)
        {
            try
            {
                EmployeeDAL dal = new EmployeeDAL();
                CheckPassportValid CheckPassportValid = new CheckPassportValid();
                CheckPassportValid = dal.checkValidEmployeeVisaDetail(employeeId, countryId);
                if (CheckPassportValid.IsVisaValid == true && CheckPassportValid.IsVisaExist == true)
                    return Json(new { IsVisaValid = true, IsVisaExist = true }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { IsVisaValid = false, IsVisaExist = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult GetVisaDetailsLoadGrid(int page, int rows, string employeeId, TravelDetailsPerson type)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                VisaDetailsViewModel model = new VisaDetailsViewModel();
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                List<VisaDetailsViewModel> visaDetailsList = employeeDAL.GetVisaDetails(page, rows, Convert.ToInt32(decryptedEmployeeId), type);
                if ((visaDetailsList == null || visaDetailsList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    visaDetailsList = employeeDAL.GetVisaDetails(page, rows, Convert.ToInt32(decryptedEmployeeId), type);
                }

                int totalCount = employeeDAL.GetVisaDetailsTotalCount(Convert.ToInt32(decryptedEmployeeId), type);

                var totalRecords = totalCount;//totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = visaDetailsList
                };

                return Json(jsonData);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult DeleteVisaDetails(int employeeVisaId)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();
            if (employeeDAL.DeleteEmployeeVisaDetails(employeeVisaId))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DeleteSpouseVisaDetails(int dependantsVisaDetailsId)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();
            if (employeeDAL.DeleteEmployeeDependantVisaDetails(dependantsVisaDetailsId))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// This action create new employee or edit exiting employee details
        /// </summary>
        /// <param name="term">Search Employee by Employee Id</param>
        /// <returns>Gives Employee Details</returns>
        [HttpGet]
        public ActionResult DisciplinaryDetailsForView()
        {
            EmployeeDisciplinaryDetailsViewModel Disciplines = new EmployeeDisciplinaryDetailsViewModel();

            return PartialView("_DisplayDisciplinaryDetails");
        }

        [HttpGet]
        [PageAccess(PageName = "Disciplinary")]
        public ActionResult DisciplinaryDetails(string employeeId)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                string employeeCode = Membership.GetUser().UserName;
                int employeeID = employeeDAL.GetEmployeeID(employeeCode);
                string decryptedEmployeeId = string.Empty;
                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                {
                    decryptedEmployeeId = Convert.ToString(employeeID);
                    decryptedEmployeeId = employeeId;
                }
                ViewBag.EmployeeId = employeeId;
                EmployeeDisciplinaryDetailsViewModel model = new EmployeeDisciplinaryDetailsViewModel();
                List<EmployeeDisciplinaryDetailsViewModel> DisciplinaryDetails = employeeDAL.GetEmployeeDisciplinaryDetailsTabclick(Convert.ToInt32(decryptedEmployeeId));

                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                model.EmpStatusMasterID = personalDAL.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                model.EmployeeManagerList = employeeDAL.GetEmployeeListForManager();
                model.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                string loggedInEmployeeID = (employeeDAL.GetEmployeeID(Membership.GetUser().UserName)).ToString();
                model.LoginUserId = Commondal.Encrypt(Convert.ToString(Session["SecurityKey"].ToString() + loggedInEmployeeID), true);
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                model.UserRole = user;
                return PartialView("_DisciplinaryDetails", model);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult AddDisciplinaryDetails(int employeeId)
        {
            try
            {
                EmployeeDisciplinaryDetailsViewModel model = new EmployeeDisciplinaryDetailsViewModel();

                EmployeeDAL employeeDAL = new EmployeeDAL();
                List<EmployeeDisciplinaryDetailsViewModel> DisciplinaryDetails = employeeDAL.GetEmployeeDisciplinaryDetailsTabclick(employeeId);

                if (DisciplinaryDetails.Count == 0)
                {
                    model.EmployeeId = 0;
                }
                else
                {
                    model.EmployeeId = employeeId;
                }
                return PartialView("_AddDisciplinaryDetails", model);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult LoadDisciplinaryDetailsData(int page, int rows, string employeeId)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();

                EmployeeDisciplinaryDetailsViewModel ddviewModel = new EmployeeDisciplinaryDetailsViewModel();

                List<EmployeeDisciplinaryDetailsViewModel> DisciplinaryDetailsList1 = new List<EmployeeDisciplinaryDetailsViewModel>();

                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                int totalCount = employeeDAL.GetEmployeeDisciplinaryDetailsTotalCount(Convert.ToInt32(decryptedEmployeeId));

                List<EmployeeDisciplinaryDetailsViewModel> DisciplinaryDetailsList = employeeDAL.GetEmployeeDisciplinaryDetails(page, rows, Convert.ToInt32(decryptedEmployeeId));

                if ((DisciplinaryDetailsList == null || DisciplinaryDetailsList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    DisciplinaryDetailsList = employeeDAL.GetEmployeeDisciplinaryDetails(page, rows, Convert.ToInt32(decryptedEmployeeId));
                }

                foreach (EmployeeDisciplinaryDetailsViewModel details in DisciplinaryDetailsList)
                {
                    ddviewModel = new EmployeeDisciplinaryDetailsViewModel()
                    {
                        CreatedByUserId = details.CreatedByUserId,
                        CreatedByUserName = details.CreatedByUserName,
                        DisciplineSubject = details.DisciplineSubject,
                        Manager = employeeDAL.GetManagerNameFromManagerId(details.ManagerId),
                        AddedDate = DateTime.Parse(details.AddedDate.ToString()),
                        DisciplineMessage = details.DisciplineMessage,
                        ManagerId = details.ManagerId,
                        EmployeeId = details.EmployeeId,
                        DisciplineId = details.DisciplineId
                    };

                    DisciplinaryDetailsList1.Add(ddviewModel);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = DisciplinaryDetailsList1
                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disciplineId"></param>
        /// <returns></returns>
        public ActionResult GetDisciplineDetailsFromDisciplineId(int disciplineId)
        {
            EmployeeDisciplinaryDetailsViewModel model = new EmployeeDisciplinaryDetailsViewModel();
            EmployeeDAL employeeDAL = new EmployeeDAL();
            try
            {
                ViewBag.loginUserId = employeeDAL.GetLoginUserId(disciplineId);
                model = employeeDAL.GetDisciplineDetailsFromDisciplineId(disciplineId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult AddUpdateEmployeeDisciplines(EmployeeDisciplinaryDetailsViewModel model)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();
            bool resultMessage = true;
            try
            {
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                model.LoginUserId = loginUserId;
                Tbl_PM_Disciplinary employeeDisciplines = new Tbl_PM_Disciplinary()
                {
                    CreatedBy = model.LoginUserId,
                    EmployeeId = model.EmployeeId,
                    Date = DateTime.Parse(model.AddedDate.ToString()),
                    Message = model.DisciplineMessage.Trim(),
                    subject = model.DisciplineSubject.Trim(),

                    ManagerId = Convert.ToInt32(model.Manager),
                    DisciplinaryId = model.DisciplineId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                //resultMessage = employeeDAL.AddUpdateEmployeeDisciplines(employeeDisciplines);
                return Json(resultMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult DeleteDesignationDetails(int? designationId, bool? isDefaultRecord, int? employeeId)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();
            bool eq = employeeDAL.DeleteDesignationaryDetails(designationId, isDefaultRecord, employeeId);
            return Json(eq, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disciplineId"></param>
        /// <returns></returns>
        public ActionResult DeleteDisciplineDetails(int disciplineId)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();

            bool eq = employeeDAL.DeleteDisciplinaryDetails(disciplineId);
            return Json(eq, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet]
        [PageAccess(PageName = "Employee")]
        public ActionResult EmployeeDetails(string employeeId)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                string decryptedEmployeeId = string.Empty;
                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedEmployeeId = Convert.ToString(employeeID);
                string encyptedEmploeeId = Commondal.Encrypt(this.Session["SecurityKey"].ToString() + Convert.ToString(decryptedEmployeeId), true);
                ViewBag.EmployeeId = encyptedEmploeeId;
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                EmployeeDetailsViewModel model = new EmployeeDetailsViewModel();
                ViewBag.Role = new SelectList(employeeDAL.GetEmployeeRole(), "RoleID", "RoleDescription");
                ViewBag.ReportingTo = new SelectList(employeeDAL.GetReportingToList_Emp(), "EmployeeId", "EmployeeName");
                ViewBag.CompetencyManager = new SelectList(employeeDAL.GetReportingToList_Emp(), "EmployeeId", "EmployeeName");
                ViewBag.ExitConfirmationManager = new SelectList(employeeDAL.GetReportingToList_Emp(), "EmployeeId", "EmployeeName");
                ViewBag.Location = new SelectList(employeeDAL.GetLocationList(), "LocationId", "LocationName");
                //DT:GroupId
                ViewBag.GroupName = new SelectList(employeeDAL.GetGroupList(), "GroupId", "GroupName");
                // Resource Name: Resource Detail
                ViewBag.Resource = new SelectList(employeeDAL.GetResourceMaster(), "ResourcePoolId", "ResourcePoolName");
                ViewBag.OfficeLocationName = new SelectList(employeeDAL.GetOfficeLocationList(), "OfficeLocationID", "OfficeLocation");
                //Respouce Pool:Current Du,Parent DU
                ViewBag.DU = new SelectList(employeeDAL.GetResourcePool(), "ResourcePoolID", "ResourcePoolName");
                ViewBag.Calender = new SelectList(employeeDAL.GetCalenderLocationList(), "CalenderId", "CalenderLocationName");
                ViewBag.ShiftName = new SelectList(employeeDAL.GetshiftDetailsList(), "ShiftId", "ShiftDescription");
                ViewBag.Roles = new SelectList(Roles.GetAllRoles(), "roleName").OrderBy(x => x.Text).ToList();
                ViewBag.BusinessGroups = new SelectList(employeeDAL.getBusinessGroupNames(), "BusinessGroupID", "BusinessGroup");
                ViewBag.Recruiter = new SelectList(employeeDAL.GetSearchEmployeeForRecruiter(), "EmployeeName", "EmployeeName");
                ViewBag.ConfirmProcess = employeeDAL.CheckConfirmStatus(Convert.ToInt32(decryptedEmployeeId));

                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                model.EmpStatusMasterID = personalDAL.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));

                HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetails(Convert.ToInt32(decryptedEmployeeId));
                model.EmployeeStatusMasterList = employeeDAL.GetEmployeeStatusMaster();

                //if statusMasterid is set then only status dropdown will fill else it will be blank.
                if (employee.EmployeeStatusMasterID != 0 || employee.EmployeeStatusMasterID != null)
                    model.EmployeeStatusList = employeeDAL.GetEmployeeStatus(employee.EmployeeStatusMasterID);

                List<LoginRolesDetails> ObjLoginDetails = new List<LoginRolesDetails>();
                ObjLoginDetails = employeeDAL.GetLoginRolesDetails();

                HRMS_tbl_PM_Employee objReportingToName = employeeDAL.GetEmployeeReportingToName_Emp(Convert.ToInt32(decryptedEmployeeId));
                HRMS_tbl_PM_Employee ObjCompetencyManagerName = employeeDAL.GetCompetencyManagerName_Emp(Convert.ToInt32(decryptedEmployeeId));
                HRMS_tbl_PM_Employee ObjExitConfirmationManagerName = employeeDAL.GetExitConfirmationManagerName_Emp(Convert.ToInt32(decryptedEmployeeId));
                tbl_HR_ExitInstance objExitInstance = employeeDAL.GetSeperationDetails(Convert.ToInt32(decryptedEmployeeId));
                model.UserRole = user;

                if (employee != null)
                {
                    model.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                    model.EmployeeCode = employee.EmployeeCode;

                    if (employee.EmployeeStatusMasterID != null)
                        model.EmployeeStatusMaster = employee.EmployeeStatusMasterID.ToString();

                    if (employee.EmployeeStatusID != null)
                        model.EmployeeStatus = employee.EmployeeStatusID.ToString();
                    if (employee.IsBillable == null)
                        employee.IsBillable = true;
                    model.BillableStatus = employee.IsBillable.HasValue ? employee.IsBillable.Value : false;
                    // Modified By Mahesh F For Issue Id:18556
                    if (employee.Commitments_Made == null)
                    {
                        model.CommitmentsMade = "NA";
                    }
                    else
                    {
                        model.CommitmentsMade = employee.Commitments_Made;
                    }
                    model.ConfirmationDate = employee.ConfirmationDate;
                    model.CurrentDU = employee.Current_DU;

                    model.DT = employee.GroupID.ToString();
                    if (employee.GroupID.ToString() != null)
                        model.DT = employee.GroupID.ToString();

                    model.ExitDate = employee.LeavingDate;
                    model.JoiningDate = employee.JoiningDate;
                    model.RejoinedWithingOneYear = employee.RejoinedWithinYear.HasValue ? employee.RejoinedWithinYear.Value : false;
                    model.OrganizationUnit = employee.LocationID.ToString();
                    model.LastYearAppraisal = employee.L_Y_Appraisal_Score;

                    if (model.LastYearAppraisal == null)
                    {
                        model.LastYearAppraisal = "NA";
                    }
                    model.LastYearIncrement = employee.L_Y_Increment;
                    if (model.LastYearIncrement == null)
                    {
                        model.LastYearIncrement = "NA";
                    }
                    model.LastYearPromotion = employee.L_Y_Promotion_Status;
                    if (model.LastYearPromotion == null)
                    {
                        model.LastYearPromotion = "NA";
                    }
                    model.OfficeLocation = employee.OfficeLocation.ToString();
                    model.ParentDU = employee.ResourcePoolID.ToString();
                    model.Months = (Convert.ToInt32(employee.ProbationPeriod));
                    int mon = model.Months.HasValue ? model.Months.Value : 0;
                    DateTime? mon1 = employee.JoiningDate;
                    mon1 = DateTime.Now.AddMonths(mon);
                    if (employee.Probation_Review_Date == null)
                        model.ProbationReviewDate = mon1;
                    else
                        model.ProbationReviewDate = employee.Probation_Review_Date;
                    model.RecruiterName = employee.Recruiter_Name;
                    model.Region = employee.Region;
                    model.ESICNo = employee.ESICNo;
                    if (model.ESICNo == null)
                    {
                        model.ESICNo = "NA";
                    }
                    model.PFNo = employee.PFNo;
                    if (model.PFNo == null)
                    {
                        model.PFNo = "NA";
                    }
                    model.IncomeTaxNo = employee.IncomeTaxNo;
                    if (model.IncomeTaxNo == null)
                    {
                        model.IncomeTaxNo = "NA";
                    }
                    model.CalenderName = employee.CalendarLocationId.ToString();
                    model.Shift = employee.ShiftID.ToString();
                    string[] Loginrole = Roles.GetRolesForUser(employee.EmployeeCode); //for Employee Role
                    model.LoginRolesList = new List<LoginRolesDetails>();
                    model.HdLoginRolesList = new List<LoginRolesDetails>();
                    model.Group = employee.BusinessGroupID.ToString();

                    model.OrgRoleID = employee.PostID;
                    model.Months = null;
                    if (objReportingToName != null)
                    {
                        model.ReportingToName_Emp = objReportingToName.EmployeeName;
                        model.ReportingToId_Emp = objReportingToName.EmployeeID;
                    }

                    if (ObjExitConfirmationManagerName != null)
                    {
                        model.ExitConfirmationManagerName_Emp = ObjExitConfirmationManagerName.EmployeeName;
                        model.ExitConfirmationManagerId_Emp = ObjExitConfirmationManagerName.EmployeeID;
                    }

                    if (ObjCompetencyManagerName != null)
                    {
                        model.CompetencyManagerName_Emp = ObjCompetencyManagerName.EmployeeName;
                        model.CompetencyManagerId_Emp = ObjCompetencyManagerName.EmployeeID;
                    }

                    if (objExitInstance != null)
                    {
                        model.TentativeReleaseDate = objExitInstance.TentativeReleavingDate;
                        model.ResignedDate = objExitInstance.ResignedDate;
                        model.AgreedReleaseDate = objExitInstance.AgreedReleaseDate;
                    }

                    if (employee.DesignationID != null && employee.DesignationID != 0)
                    {
                        tbl_PM_DesignationMaster designation = employeeDAL.GetDesignation(employee.DesignationID);
                        model.Designation = designation.DesignationName;
                    }
                    else
                        model.Designation = "Designation is not set";

                    if (Loginrole.Count() != 0)
                    {
                        int i = 0;
                        foreach (LoginRolesDetails Lrole in ObjLoginDetails)
                        {
                            if (i < Loginrole.Count())
                            {
                                if (Lrole.RoleName == Loginrole[i].ToString())
                                {
                                    model.HdLoginRolesList.Add(Lrole);
                                    i++;
                                    model.LoginRole = model.LoginRole + "," + Lrole.RoleName;
                                }
                            }
                        }
                        model.LoginRole.TrimStart(',');
                    }
                    model.LoginRolesList = ObjLoginDetails;
                    int EmpId = Convert.ToInt32(decryptedEmployeeId);
                    var res = (from r1 in dbContext.tbl_PM_ResourcePoolMaster
                               from r2 in dbContext.tbl_PM_ResourcePoolDetail
                               where r1.ResourcePoolID == r2.ResourcePoolID && r2.EmployeeID == EmpId
                               select r1.ResourcePoolID).FirstOrDefault();
                    model.ResourcePoolName = res.ToString();
                }
                return PartialView("_EmployeeDetails", model);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult EmployeeDetails(EmployeeDetailsViewModel model)
        {
            string result = string.Empty;
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();

                var success = employeeDAL.SaveEmployeeDetails(model);
                if (success)
                    result = HRMS.Resources.Success.ResourceManager.GetString("SaveEmployeeDetailsSuccess");
                else
                    result = HRMS.Resources.Errors.ResourceManager.GetString("SaveEmployeeDetailsError");

                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetemployeeStatusController(string MatserId)
        {
            var objStatus = new JsonResult();
            int? SMid = 0;
            if (MatserId != "")
                SMid = Convert.ToInt32(MatserId);
            else
                SMid = 0;

            List<EmployeeStatusListDetails> employeestatus = new List<EmployeeStatusListDetails>();
            try
            {
                employeestatus = (from status in dbContext.tbl_PM_EmployeeStatus
                                  where status.EmployeeStatusMasterID == SMid
                                  orderby status.EmployeeStatus
                                  select new EmployeeStatusListDetails
                                  {
                                      EmployeeStatusId = status.EmployeeStatusID,
                                      EmployeeStatus = status.EmployeeStatus
                                  }).ToList();
                objStatus.Data = employeestatus;
                objStatus.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return objStatus;
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                throw;
            }
        }

        [HttpGet]
        [PageAccess(PageName = "Project")]
        public ActionResult ProjectDetails(string employeeId)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                string employeeCode = Membership.GetUser().UserName;
                int employeeID = employeeDAL.GetEmployeeID(employeeCode);
                string decryptedEmployeeId = string.Empty;
                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                {
                    ViewBag.EmployeeId = employeeId;
                    decryptedEmployeeId = Convert.ToString(employeeID);
                }

                ViewBag.employeeId = employeeId;
                ProjectDetailsViewModel model = new ProjectDetailsViewModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                model.UserRole = user;

                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                model.EmpStatusMasterID = personalDAL.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                model.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                ViewBag.ProjectList = new SelectList(employeeDAL.GetProjectList(), "ProjectId", "ProjectName");
                ViewBag.EmployeeList = new SelectList(employeeDAL.GetEmployeeList(), "EmployeeId", "EmployeeName");
                ViewBag.RoleList = new SelectList(employeeDAL.GetProjectRole(), "ProjectRoleId", "ProjectRoleDesc");
                ViewBag.ResourceManager = new SelectList(employeeDAL.GetResourcePoolEmployee(), "EmployeeId", "EmployeeName");
                ViewBag.DeliveryHead = new SelectList(employeeDAL.GetDeliveyHead(), "EmployeeId", "EmployeeName");
                ViewBag.DelieveryUnitList = new SelectList(employeeDAL.GetResourcePool(), "ResourcePoolID", "ResourcePoolName");
                return PartialView("_ProjectDetails", model);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult LoadProjectDetails(int page, int rows, string employeeId)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                ProjectDetailsViewModel model = new ProjectDetailsViewModel();
                int totalCount;
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                List<ProjectDetailsViewModel> projectDetailsList = employeeDAL.GetProjectDetailsByEmployeeId(page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);

                if ((projectDetailsList == null || projectDetailsList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    projectDetailsList = employeeDAL.GetProjectDetailsByEmployeeId(page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = projectDetailsList
                };

                return Json(jsonData);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        [PageAccess(PageName = "Bond Details")]
        public ActionResult BondDetails(string employeeId)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                string employeeCode = Membership.GetUser().UserName;
                int employeeID = employeeDAL.GetEmployeeID(employeeCode);
                string decryptedEmployeeId = string.Empty;
                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                {
                    decryptedEmployeeId = Convert.ToString(employeeID);
                    ViewBag.EmployeeId = employeeId;
                }
                ViewBag.bondEmployeeId = employeeId;
                BondDetailsViewModel model = new BondDetailsViewModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);

                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                model.EmpStatusMasterID = personalDAL.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                ViewBag.BondTypeList = employeeDAL.GetBondTypeList();
                model.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                model.UserRole = user;
                model.BondStatusList = new Dictionary<string, string>();
                model.BondStatusList.Add(YesNoCondition.Yes.ToLower(), YesNoCondition.Yes);
                model.BondStatusList.Add(YesNoCondition.No.ToLower(), YesNoCondition.No);
                ViewBag.BondStatusList = model.BondStatusList;
                return PartialView("_BondDetails", model);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult BondDetails(BondDetailsViewModel model, string EmployeeId, int? BondId, string BondStatus, int BondTypeId)
        {
            bool status = false;
            string resultMessage = HRMS.Resources.Errors.BondDetailsAddError;
            try
            {
                string decryptedEmployeeId = string.Empty;
                if (EmployeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(EmployeeId, out isAuthorize);
                    model.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                    model.BondId = BondId.HasValue ? BondId.Value : 0;
                    model.BondStatus = BondStatus;
                    model.BondTypeID = BondTypeId;
                }
                if (model.BondStatus == "no")
                {
                    model.BondAmount = null;
                    model.BondOverDate = null;
                }
                EmployeeDAL employeeDAL = new EmployeeDAL();
                if (status==false)
                {
                    resultMessage = HRMS.Resources.Success.BondDetailsAddSuccess;
                    status = true;
                }
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                resultMessage = HRMS.Resources.Errors.ExceptionOccurred;
            }
            //  return Json(resultMessage , JsonRequestBehavior.AllowGet);
            return Json(new { resultMesssage = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadBondDetailsGrid(int page, int rows, string employeeId)
        {
            try
            {
                EmployeeDAL dal = new EmployeeDAL();
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                List<BondDetailsViewModel> model = new List<BondDetailsViewModel>();
                model = dal.LoadBondDetailsGrid(page, rows, Convert.ToInt32(decryptedEmployeeId));

                if ((model == null || model.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model = dal.LoadBondDetailsGrid(page, rows, Convert.ToInt32(decryptedEmployeeId));
                }

                int totalCount = dal.LoadBondDetailsGridTotalCount(page, rows, Convert.ToInt32(decryptedEmployeeId));

                var totalRecords = totalCount;//totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = model
                };

                return Json(jsonData);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult DeleteBondDetails(int employeeBondID)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();

            bool eq = employeeDAL.DeleteBondDetails(employeeBondID);
            return Json(eq, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [PageAccess(PageName = "Experience")]
        public ActionResult ExperienceDetails(string employeeId)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                string employeeCode = Membership.GetUser().UserName;
                int employeeID = employeeDAL.GetEmployeeID(employeeCode);
                string decryptedEmployeeId = string.Empty;
                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                {
                    ViewBag.EmployeeId = employeeId;
                    decryptedEmployeeId = Convert.ToString(employeeID);
                }
                EmployeeDAL empdal = new EmployeeDAL();
                EmployeeExperienceDetails _employeeExperienceDetails = empdal.GetEmployeeExperienceDetails(Convert.ToInt32(decryptedEmployeeId));

                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                _employeeExperienceDetails.UserRole = user;
                _employeeExperienceDetails.UserRole = user;
                ViewBag.EmployeeId = employeeId;
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                int employeeMasterStatus = personalDAL.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                _employeeExperienceDetails.EmpStatusMasterID = employeeMasterStatus;
                _employeeExperienceDetails.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                _employeeExperienceDetails.PastExperienceDetails.NewExperience = new PastEmployeeExperienceDetails();
                _employeeExperienceDetails.PastExperienceDetails.NewExperience.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                _employeeExperienceDetails.PastExperienceDetails.NewExperience.EmployeeWorkingTypeList = new List<EmployeeWorkingType>();
                _employeeExperienceDetails.PastExperienceDetails.NewExperience.EmployeeWorkingTypeList = empdal.GetEmpoyeeWorkingTypeList();
                ViewBag.EmployeeWorkingTypeList = empdal.GetEmpoyeeWorkingTypeList();
                _employeeExperienceDetails.PastExperienceDetails.EmpStatusMasterID = employeeMasterStatus;
                return PartialView("_ExperienceDetails", _employeeExperienceDetails);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult ExperienceDetails(PastEmployeeExperienceDetails model, string EmployeeId, int? EmpHistroyId, int EmpTypeId)
        {
            bool resultStatus = true;
            try
            {
                string decryptedEmployeeId = string.Empty;
                if (EmployeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(EmployeeId, out isAuthorize);
                    model.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                    model.EmployeeHistoryId = EmpHistroyId;
                    model.EmployeeTypeId = EmpTypeId;
                }

                EmployeeDAL employeeDAL = new EmployeeDAL();
                //resultStatus = employeeDAL.AddUpdateEmployeePastExperience(model);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                throw;
            }
            return Json(resultStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GapExperienceDetails(EmployeeExperienceDetails model, string EmployeeId, int? EmployeeGapExpId)
        {
            bool resultStatus = false;
            try
            {
                string decryptedEmployeeId = string.Empty;
                if (EmployeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(EmployeeId, out isAuthorize);
                    model.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                    model.EmployeeGapExpId = EmployeeGapExpId.HasValue ? EmployeeGapExpId.Value : 0;
                }
                EmployeeDAL employeeDAL = new EmployeeDAL();
              //  resultStatus = employeeDAL.AddUpdateEmployeeGapExperience(model);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                throw;
            }
            return Json(resultStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPastExperienceDetailsLoadGrid(int page, int rows, string employeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                EmployeeDAL empdal = new EmployeeDAL();
                List<PastEmployeeExperienceDetails> _employeeExperienceDetails = empdal.GetEmployeePastExperienceDetails(page, rows, Convert.ToInt32(decryptedEmployeeId));

                if ((_employeeExperienceDetails == null || _employeeExperienceDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    _employeeExperienceDetails = empdal.GetEmployeePastExperienceDetails(page, rows, Convert.ToInt32(decryptedEmployeeId));
                }

                int totalCount = empdal.GetEmployeePastExperienceDetailsTotalCount(Convert.ToInt32(decryptedEmployeeId));

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = _employeeExperienceDetails
                };

                return Json(jsonData);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult GetGapExperienceDetailsLoadGrid(int page, int rows, string employeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                EmployeeDAL empdal = new EmployeeDAL();
                List<EmployeeExperienceDetails> _employeeExperienceDetails = empdal.GetEmployeeGapExperienceDetails(page, rows, Convert.ToInt32(decryptedEmployeeId));

                if ((_employeeExperienceDetails == null || _employeeExperienceDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    _employeeExperienceDetails = empdal.GetEmployeeGapExperienceDetails(page, rows, Convert.ToInt32(decryptedEmployeeId));
                }

                int totalCount = empdal.GetEmployeeGapExperienceDetailsTotalCount(Convert.ToInt32(decryptedEmployeeId));

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = _employeeExperienceDetails
                };

                return Json(jsonData);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// by vijay
        /// <param name="UploadTypeId"></param>
        /// <returns></returns>
        public string GetUploadTypeSelectedText(int UploadTypeId)
        {
            UploadsDAL uploads = new UploadsDAL();
            var uploadTypes = uploads.GetEmployeeUploadTypes();

            return uploadTypes.Where(u => u.UploadTypeId == UploadTypeId).FirstOrDefault().UploadType;
        }

        /// <summary>
        /// Method to get uploadTypeText from DocId
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public string GetEmpUploadTypeTextFromDocId(int documentId)
        {
            string uploadTypeText = string.Empty;
            HRMSDBEntities dbContext = new HRMSDBEntities();
            try
            {
                var uploadTypeId = (from ut in dbContext.Tbl_Employee_Documents
                                    where ut.DocumentId == documentId
                                    select ut.UploadTypeId).FirstOrDefault();

                uploadTypeText = GetUploadTypeSelectedText(uploadTypeId);
                return uploadTypeText;
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// by vijay
        /// <returns></returns>
        public List<SelectListItem> GetUploadTypes()
        {
            UploadsDAL uploads = new UploadsDAL();
            var uploadTypes = uploads.GetEmployeeUploadTypes();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var ut in uploadTypes)
            {
                list.Add(new SelectListItem { Selected = true, Text = ut.UploadType, Value = ut.UploadTypeId.ToString() });
            }

            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public ActionResult UploadDocuments(string employeeId)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                string employeeCode = Membership.GetUser().UserName;
                int employeeID = employeeDAL.GetEmployeeID(employeeCode);
                string decryptedEmployeeId = string.Empty;
                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedEmployeeId = Convert.ToString(employeeID);

                ViewBag.employeeId = employeeId;
                UploadEmployeeDocumentsViewModel uploademployeedocumentmodel = new UploadEmployeeDocumentsViewModel();
                uploademployeedocumentmodel.UploadTypeValues = this.GetUploadTypes();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                uploademployeedocumentmodel.UserRole = user;

                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                uploademployeedocumentmodel.EmpStatusMasterID = personalDAL.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                uploademployeedocumentmodel.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                return PartialView("_UploadEmployeeDocs", uploademployeedocumentmodel);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult LoadEmployeeUploadDetails(int page, int rows, string employeeId)
        {
            UploadsDAL uploads = new UploadsDAL();
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                List<UploadEmployeeDocumentsViewModel> Result = uploads.GetEmpDocumentForDispay(page, rows, Convert.ToInt32(decryptedEmployeeId));

                if ((Result == null || Result.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    Result = uploads.GetEmpDocumentForDispay(page, rows, Convert.ToInt32(decryptedEmployeeId));
                }

                int totalCount = uploads.GetEmpDocumentForDispayTotalCount(Convert.ToInt32(decryptedEmployeeId));

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = Result
                };

                return Json(jsonData);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadEmployeeDocument(HttpPostedFileBase doc, UploadEmployeeDocumentsViewModel model)
        {
            UploadsDAL uploads = new UploadsDAL();
            bool uploadStatus = false;

            if (doc.ContentLength > 0)
            {
                string uploadsPath = (UploadFileLocation);
                string uploadpathwithId = Path.Combine(uploadsPath, (model.EmployeeId).ToString());
                uploadsPath = Path.Combine(uploadpathwithId, GetUploadTypeSelectedText(model.UploadTypeId));
                string fileName = Path.GetFileName(doc.FileName);
                try
                {
                    IDocuments document = null;

                    if (!uploads.IsEmployeeDocumentExists(Path.GetFileName(doc.FileName), model.EmployeeId, model.UploadTypeId))
                    {
                        // Insert new record to parent
                        document = new Tbl_Employee_Documents();
                        document.FileName = Path.GetFileName(doc.FileName);
                        ((Tbl_Employee_Documents)document).FileDescription = model.FileDescription.Trim();
                        ((Tbl_Employee_Documents)document).UploadTypeId = model.UploadTypeId;
                        ((Tbl_Employee_Documents)document).EmployeeId = model.EmployeeId;
                    }
                    else
                    {
                        // Insert new record to child
                        document = new Tbl_Employee_DocumentDetail();
                        int documentID = 0;
                        string newNameForDocument = uploads.GetNewNameForEmployeeDocument(Path.GetFileName(doc.FileName), model.UploadTypeId, model.EmployeeId, out documentID);
                        fileName = newNameForDocument;
                        document.DocumentId = documentID;
                        document.FileName = newNameForDocument;
                    }

                    document.FilePath = uploadsPath;
                    if (model.Comments != null && model.Comments != "")
                        document.Comments = model.Comments.Trim();
                    else
                        document.Comments = model.Comments;
                    document.FileDescription = model.FileDescription.Trim();
                    //ToDO: need to bind the Uploaded userId from logged in user
                    document.UploadedBy = int.Parse(HttpContext.User.Identity.Name);
                    document.UploadedDate = DateTime.Now;
                    bool result = uploads.UploadEmployeeDocument(document);

                    string filePath = Path.Combine(uploadsPath, fileName);
                    if (!Directory.Exists(uploadsPath))
                        Directory.CreateDirectory(uploadsPath);

                    doc.SaveAs(filePath);

                    if (result)
                        uploadStatus = true;
                    else
                    {
                        uploadStatus = false;
                    }
                }
                catch (Exception ex)
                {
                    var num = incident.Next(10000, 99999).ToString();
                    log.Error("Error : " + num, ex);
                    return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
                }
            }

            return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disciplineId"></param>
        /// <returns></returns>
        public ActionResult DeleteEmployeeUploadDetails(int documentId, int employeeId)
        {
            UploadsDAL uploads = new UploadsDAL();
            bool udd = false;
            try
            {
                var parentDocument = dbContext.Tbl_Employee_Documents.Where(x => x.DocumentId == documentId)
                                       .FirstOrDefault();

                var versionDocs = dbContext.Tbl_Employee_DocumentDetail.Where(x => x.DocumentId == documentId).ToList();

                string rootFolder = (UploadFileLocation);
                string subpath = Path.Combine(rootFolder, (employeeId).ToString());
                string subfolderpath = Path.Combine(subpath, GetEmpUploadTypeTextFromDocId(parentDocument.DocumentId));

                if (versionDocs != null)
                {
                    foreach (var d in versionDocs)
                    {
                        string versionDocFilepath = Path.Combine(subfolderpath, d.FileName);

                        if (System.IO.File.Exists(versionDocFilepath))
                        {
                            System.IO.File.Delete(versionDocFilepath);
                        }
                    }
                }

                string Filepath = Path.Combine(subfolderpath, parentDocument.FileName);

                if (System.IO.File.Exists(Filepath))
                {
                    System.IO.File.Delete(Filepath);
                }

                udd = uploads.DeleteEmployeeUploadDetails(documentId);
                return Json(udd, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                udd = false;
                return Json(udd, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public ActionResult DeleteEmpDocsSelected(List<string> filenames)
        {
            UploadsDAL uploads = new UploadsDAL();
            HRMSDBEntities dbContext = new HRMSDBEntities();

            bool result = false;
            try
            {
                if (filenames != null)
                {
                    foreach (string filename in filenames)
                    {
                        var documentformchild = (from document in dbContext.Tbl_Employee_Documents
                                                 join documentDetails in dbContext.Tbl_Employee_DocumentDetail
                                                 on document.DocumentId equals documentDetails.DocumentId
                                                 where documentDetails.FileName == filename
                                                 select documentDetails).FirstOrDefault();

                        var parentid = (from document in dbContext.Tbl_Employee_Documents
                                        join documentDetails in dbContext.Tbl_Employee_DocumentDetail
                                        on document.DocumentId equals documentDetails.DocumentId
                                        where documentDetails.FileName == filename
                                        select document).FirstOrDefault();

                        string rootFolder = (UploadFileLocation);
                        string subpath = Path.Combine(rootFolder, (parentid.EmployeeId).ToString());
                        string subfolderpath = Path.Combine(subpath, GetEmpUploadTypeTextFromDocId(documentformchild.DocumentId));
                        string Filepath = Path.Combine(subfolderpath, filename);

                        if (System.IO.File.Exists(Filepath))
                        {
                            System.IO.File.Delete(Filepath);
                        }
                        result = uploads.DeleteEmpDocsSelected(filename);
                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Action methd will hit when user will click on the Details link in gridview,
        /// to view the Employee Doc History of the files
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public ActionResult ShowHistoryEmpDocUploads(int documentId, int employeeId)
        {
            UploadsDAL uploads = new UploadsDAL();
            List<Tbl_Employee_DocumentDetail> objHRDocDetails = new List<Tbl_Employee_DocumentDetail>();
            Tbl_Employee_Documents objhRDoc = new Tbl_Employee_Documents();
            List<UploadEmployeeDocumentsViewModel> objDocList = new List<UploadEmployeeDocumentsViewModel>();

            try
            {
                objhRDoc = uploads.GetEmployeeDocument(documentId);
                objHRDocDetails = uploads.GetEmpDocumentHistoryForDisplay(documentId);

                foreach (Tbl_Employee_DocumentDetail eachDocDetail in objHRDocDetails)
                {
                    UploadEmployeeDocumentsViewModel dd = new UploadEmployeeDocumentsViewModel()
                    {
                        DocumentID = eachDocDetail.DocumentId,
                        Comments = eachDocDetail.Comments,
                        FileDescription = eachDocDetail.FileDescription,
                        FileName = eachDocDetail.FileName,
                        UploadedBy = uploads.GetUploadNameFromUploadById(HttpContext.User.Identity.Name),
                        UploadedDate = (eachDocDetail.UploadedDate).Value,
                        FilePath = eachDocDetail.FilePath,
                        EmployeeId = employeeId
                    };
                    objDocList.Add(dd);
                }

                UploadEmployeeDocumentsViewModel dd1 = new UploadEmployeeDocumentsViewModel()
                {
                    DocumentID = objhRDoc.DocumentId,
                    Comments = objhRDoc.Comments,
                    FileDescription = objhRDoc.FileDescription,
                    FileName = objhRDoc.FileName,
                    UploadedBy = uploads.GetUploadNameFromUploadById(HttpContext.User.Identity.Name),
                    UploadedDate = (objhRDoc.UploadedDate).Value,
                    FilePath = objhRDoc.FilePath,
                    EmployeeId = employeeId
                };

                objDocList.Add(dd1);
                return PartialView("_ShowEmpDocHistroy", objDocList);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Action will fire when user clicks on the Filename,to download the file,
        /// when viewing the history/Details view of files
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ActionResult DownloadEmployeeFile(string filename, int employeeId)
        {
            int empId = 0;
            HRMSDBEntities dbContext = new HRMSDBEntities();
            UploadsDAL uploads = new UploadsDAL();
            try
            {
                var documentformchild = (from document in dbContext.Tbl_Employee_Documents
                                         join documentDetails in dbContext.Tbl_Employee_DocumentDetail
                                         on document.DocumentId equals documentDetails.DocumentId
                                         where document.EmployeeId == employeeId && documentDetails.FileName == filename
                                         select documentDetails).FirstOrDefault();

                var documentfromparent = (from document in dbContext.Tbl_Employee_Documents
                                          where document.EmployeeId == employeeId && document.FileName == filename
                                          select document).FirstOrDefault();

                if (documentformchild != null)
                {
                    empId = uploads.GetEmpIdFromDocId(documentformchild.DocumentId);
                }

                string rootFolder = (UploadFileLocation);
                string[] FileExtention = filename.Split('.');
                string contentType = "application/" + FileExtention[1];

                if (documentformchild != null)
                {
                    string uploadpathwithId = Path.Combine(rootFolder, (empId).ToString());
                    string subfolderpath = Path.Combine(uploadpathwithId, GetEmpUploadTypeTextFromDocId(documentformchild.DocumentId));
                    string Filepath = Path.Combine(subfolderpath, filename);

                    if (System.IO.File.Exists(Filepath))
                        return File(Filepath, contentType, filename);
                    else
                    {
                        var Result = "File does not exist on the server";
                        return Json(Result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    string uploadpathwithId = Path.Combine(rootFolder, (documentfromparent.EmployeeId).ToString());
                    string subfolderpath = Path.Combine(uploadpathwithId, GetEmpUploadTypeTextFromDocId(documentfromparent.DocumentId));
                    string Filepath = Path.Combine(subfolderpath, filename);

                    if (System.IO.File.Exists(Filepath))
                        return File(Filepath, contentType, filename);
                    else
                    {
                        var Result = "File does not exist on the server";
                        return Json(Result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetTotalExperienceDetails(string employeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                EmployeeDAL employeeDAL = new EmployeeDAL();
                EmployeeExperienceDetails model = employeeDAL.GetEmployeeExperienceDetails(Convert.ToInt32(decryptedEmployeeId));
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                model.UserRole = user;

                model.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                return PartialView("_TotalExperienceDetails", model);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult AddEmployeeExperienceDetails(int employeeId)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();
            PastEmployeeExperienceDetails model = new PastEmployeeExperienceDetails();
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            string user = Commondal.GetMaxRoleForUser(role);
            model.UserRole = user;

            model.EmployeeId = employeeId;
            model.EmployeeWorkingTypeList = new List<EmployeeWorkingType>();
            model.EmployeeWorkingTypeList = employeeDAL.GetEmpoyeeWorkingTypeList();
            return PartialView("_AddPastExperienceDetails", model);
        }

        [HttpGet]
        public ActionResult DeletePastExperienceDetails(int empHistoryId)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                bool status = true;
                if (true)//employeeDAL.DeleteEmployeePastExperienceDetails(empHistoryId))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult DeleteGapExperienceDetails(int empGapExpId)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                bool status=true;
                if (status)//employeeDAL.DeleteEmployeeGapExperienceDetails(empGapExpId))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult UpdateTotalExperienceDetails(EmployeeExperienceDetails model)
        {
            bool isUpdated = false;
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
             //employeeDAL.UpdateTotalExperienceDetails(model))
                    isUpdated = true;
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
            return Json(isUpdated, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetExpDateList(int EmployeeId)
        {
            try
            {
                EmployeeDAL Empdal = new EmployeeDAL();
                List<PastEmployeeExperienceDetails> list = new List<PastEmployeeExperienceDetails>();
                list = Empdal.GetExperceDateList(EmployeeId);

                return Json(new { Count = list.Count, DateList = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var num = incident.Next(10000, 99999).ToString();
                log.Error("Error : " + num, ex);
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult CheckIfFileExistVisa(string VisaFileName, int? EmployeeVisaId, int? EmployeeID, TravelDetailsPerson PersonType)
        {
            try
            {
                EmployeeDAL DAL = new EmployeeDAL();
                VisaDetailsViewModel Details = DAL.GetVisaShowHistory(Convert.ToInt32(EmployeeID), Convert.ToInt32(EmployeeVisaId), PersonType);
                string[] FileExtention = VisaFileName.Split('.');
                string contentType = "application/" + FileExtention[1];
                string Filepath = Path.Combine(Details.VisaFilePath, VisaFileName);

                if (System.IO.File.Exists(Filepath))
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public ActionResult CheckIfFileExistPassport(string PassportFileName, int? DocumentID, int? EmployeeID, TravelDetailsPerson PersonType)
        {
            try
            {
                EmployeeDAL DAL = new EmployeeDAL();
                TravelDetailsViewModel Details = DAL.GetPassportShowHistory(Convert.ToInt32(EmployeeID), Convert.ToInt32(DocumentID), PersonType);
                string[] FileExtention = PassportFileName.Split('.');
                string contentType = "application/" + FileExtention[1];
                string Filepath = Path.Combine(Details.PassportFilePath, PassportFileName);

                if (System.IO.File.Exists(Filepath))
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public ActionResult InitiateConfimation(int? EmployeeID)
        {
            try
            {
                bool check;
                EmployeeDAL DAL = new EmployeeDAL();

                //TravelDetailsViewModel Details = DAL.GetPassportShowHistory(Convert.ToInt32(EmployeeID), Convert.ToInt32(DocumentID), PersonType);
                check = DAL.InitiateConfirmation(Convert.ToInt32(EmployeeID));

                //if (System.IO.File.Exists(Filepath))
                //    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                //else
                return Json(new { status = check }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}