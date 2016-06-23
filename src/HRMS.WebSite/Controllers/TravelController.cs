using HRMS.DAL;
using HRMS.Helper;
using HRMS.Models;
using HRMS.Notification;
using MvcApplication3.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS.Controllers
{
    public class TravelController : Controller
    {
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();
        private EmployeeDAL empdal = new EmployeeDAL();
        private HRMSDBEntities dbContext = new HRMSDBEntities();
        private QualificationDetailsDAL Qual = new QualificationDetailsDAL();

        public string UploadFileLocation
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadPassportFileLocation"];
            }
        }

        public string AccommodationFileLocation
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadAccFileLocation"];
            }
        }

        public string UploadClientInviteLetterFileLocation
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadClientInviteLetterLocation"];
            }
        }

        public string UploadJourneyFileLocation
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadJourneyFileLocation"];
            }
        }

        public string UploadAdminVisaLocation
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadAdminVisaFileLocation"];
            }
        }

        private TravelDAL travelDAL = new TravelDAL();

        public ActionResult Index(string Appchk)
        {
            try
            {
                TravelViewModel model = new TravelViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                ViewBag.AsciiKey = Session["SecurityKey"].ToString();
                string employeeCode = Membership.GetUser().UserName;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                string encyptedEmploeeId = Commondal.Encrypt(this.Session["SecurityKey"].ToString() + Convert.ToString(employeeId), true);
                ViewBag.encryptedEmployeeId = encyptedEmploeeId;
                int travelId = 0;
                string encyptedTravelId = Commondal.Encrypt(Convert.ToString(travelId), true);
                ViewBag.EncryptedTravelId = encyptedTravelId;
                string[] role = Roles.GetRolesForUser(employeeCode);

                if (employeeCode != null)
                {
                    model.SearchedUserDetails.EmployeeId = employeeId;
                    model.SearchedUserDetails.EmployeeCode = employeeCode;
                    model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }

                Session["Conter"] = Convert.ToInt32(0);
                ViewBag.Counter = Session["Conter"].ToString();
                Session["Travelid"] = null;
                Session["ViewExt"] = "No";
                Session["Stageid"] = null;
                Session["Btnsubmitstatus"] = null;
                Session["Extensiton"] = "No";
                int page = 1;
                int rows = 5;
                int totalCount;
                string term = "";
                string Field = "";
                string FieldChild = "";
                List<TravelStatus> searchResultInbox = new List<TravelStatus>();

                TravelDAL travelDAL = new TravelDAL();
                searchResultInbox = travelDAL.GetInboxListTravelDetails(term, Field, FieldChild, page, rows, employeeId, out totalCount);
                ViewBag.InboxCount = totalCount;
                string encryptedTravelID = Commondal.Encrypt(this.Session["SecurityKey"].ToString() + "0", true);
                ViewBag.EncryptedTravelId = encryptedTravelID;
                //if ((role.Contains("Travel Approver") || role.Contains("Group Head") || role.Contains("Travel_Admin")) && (Appchk == "Yes"))
                //{
                //    return RedirectToAction("GetTravelStatus", "Travel");
                //}
                //else
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        # region TravelContact

        [HttpPost]
        public ActionResult TravelEmergencyContactLoadGrid(int travelId, int page, int rows)
        {
            try
            {
                TravelDAL dal = new TravelDAL();
                List<TravelEmergencyContactViewModel> model = new List<TravelEmergencyContactViewModel>();
                int totalCount;
                model = dal.GetTravelEmergencyContactDetails(travelId, page, rows, out totalCount);
                if ((model == null || model.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model = dal.GetTravelEmergencyContactDetails(travelId, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = model,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult DeleteTravelContactDetails(int contactId, int travelId)
        {
            try
            {
                TravelDAL dal = new TravelDAL();
                bool status = dal.DeleteTravelContactDetails(contactId, travelId);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        private ContactViewModel ContactDetails(bool loggedinUser)
        {
            bool isSaved;
            string decryptedTravelId = Session["Travelid"].ToString();
            ContactViewModel model = new ContactViewModel();
            TravelDAL dal = new TravelDAL();
            Tbl_HR_Travel employeerecord = dal.GetTravelDetailsfromTravelID(Convert.ToInt32(decryptedTravelId));
            if (Convert.ToInt32(decryptedTravelId) != 0)
            {
                int travelID = Convert.ToInt32(decryptedTravelId);

                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                EmployeeDAL EmployeeDAL = new EmployeeDAL();
                int employeeID = EmployeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                ViewBag.loggedinEmployeeID = employeeID;

                model.TravelEmergencyContactModel = new TravelEmergencyContactViewModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                if (Session["Stageid"] != null)
                {
                    ViewBag.StageID = Session["Stageid"].ToString();
                }
                model.TravelId = travelID;
                model.TravelEmergencyContactModel.TravelId = travelID;
                ViewBag.ContactEmployeeId = employeerecord.EmployeeId;
                ViewBag.IsLoggedInUser = loggedinUser;

                if (User.IsInRole("Travel Approver"))
                {
                    ViewBag.UserRole = "TravelApprover";
                }
                if (User.IsInRole("Group Head"))
                {
                    ViewBag.UserRole = "GroupHead";
                }
                if (User.IsInRole("Travel_Admin"))
                {
                    ViewBag.UserRole = "Travel_Admin";
                }
                List<tbl_PM_EmployeeEmergencyContact> objEmergencyContactDetails = dal.GetEmployeeEmergencyContactDetails(Convert.ToInt32(employeerecord.EmployeeId));
                if (objEmergencyContactDetails != null)
                {
                    foreach (var i in objEmergencyContactDetails)
                    {
                        int emergncyContactId = i.EmployeeEmergencyContactID;
                        Tbl_HR_TravelContactDetails objTravelContactDetails = dal.GetTravelContactDetailsForEmergncyContactId(emergncyContactId, travelID);
                        if (objTravelContactDetails == null)
                            dal.SaveTravelContactDetails(objEmergencyContactDetails, travelID);
                    }
                }
            }

            ViewBag.RelationTypeList = dal.GetRelationList();
            return model;
        }

        [HttpGet]
        public ActionResult GetContactNewDetails(string TravelEmployeeId, string encryptedTravelId)
        {
            try
            {
                bool isTravelAuthorize;
                string decryptedTravelId = HRMSHelper.Decrypt(encryptedTravelId, out isTravelAuthorize);
                ContactViewModel model = new ContactViewModel();
                TravelDAL dal = new TravelDAL();
                Tbl_HR_Travel employeerecord = dal.GetTravelDetailsfromTravelID(Convert.ToInt32(decryptedTravelId));
                if (Convert.ToInt32(decryptedTravelId) != 0)
                {
                    if (!isTravelAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                    int travelID = Convert.ToInt32(decryptedTravelId);
                    bool isAuthorize;
                    string decryptedEmployeeId = HRMSHelper.Decrypt(TravelEmployeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                    string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                    string user = Commondal.GetMaxRoleForUser(role);
                    EmployeeDAL EmployeeDAL = new EmployeeDAL();
                    int employeeID = EmployeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                    ViewBag.loggedinEmployeeID = employeeID;
                    model.TravelEmergencyContactModel = new TravelEmergencyContactViewModel();
                    model.SearchedUserDetails = new SearchedUserDetails();
                    model.TravelId = travelID;
                    model.TravelEmergencyContactModel.TravelId = travelID;
                    ViewBag.ContactEmployeeId = employeerecord.EmployeeId;
                    bool loggedinUser = dal.CheckForTheLogged(Convert.ToInt32(decryptedEmployeeId), Convert.ToString(travelID));
                    ViewBag.IsLoggedInUser = loggedinUser;
                    HRMS_tbl_PM_Employee objContactDetails = dal.GetEmployeePersonalDetails(Convert.ToInt32(employeerecord.EmployeeId));
                    Tbl_HR_TravelContact objtravelContact = dal.GetTravelContact(travelID);
                    if (Session["Stageid"] != null)
                    {
                        ViewBag.StageID = Session["Stageid"].ToString();
                    }
                    if (objtravelContact != null)
                    {
                        model.PersonalEmailId = objtravelContact.PersonalEmailId;
                        model.ContactNoIndia = objtravelContact.ContactNoIndia;
                        model.ContactNoAbroad = objtravelContact.ContactNoAbroad;
                    }
                    else if (objContactDetails != null)
                    {
                        model.userPersonalEmailId = objContactDetails.EmailID1;
                        model.ContactNoIndia = objContactDetails.MobileNumber;
                    }
                    if (User.IsInRole("Travel Approver"))
                    {
                        ViewBag.UserRole = "TravelApprover";
                    }
                    if (User.IsInRole("Group Head"))
                    {
                        ViewBag.UserRole = "GroupHead";
                    }
                    if (User.IsInRole("Travel_Admin"))
                    {
                        ViewBag.UserRole = "Travel_Admin";
                    }
                    List<tbl_PM_EmployeeEmergencyContact> objEmergencyContactDetails = dal.GetEmployeeEmergencyContactDetails(Convert.ToInt32(employeerecord.EmployeeId));
                    if (objEmergencyContactDetails != null)
                    {
                        foreach (var i in objEmergencyContactDetails)
                        {
                            int emergncyContactId = i.EmployeeEmergencyContactID;
                            Tbl_HR_TravelContactDetails objTravelContactDetails = dal.GetTravelContactDetailsForEmergncyContactId(emergncyContactId, travelID);
                            if (objTravelContactDetails == null)
                                dal.SaveTravelContactDetails(objEmergencyContactDetails, travelID);
                        }
                    }
                }

                ViewBag.RelationTypeList = dal.GetRelationList();
                return PartialView("_ContactDetailsForm", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SaveTravelContact(ContactViewModel model)
        {
            try
            {
                string result = string.Empty;
                TravelDAL dal = new TravelDAL();
                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);
                Tbl_HR_Travel stageid = dal.GetTravelDetailsfromTravelID(Convert.ToInt32(decryptedTravelId));
                int ContStageid = Convert.ToInt32(stageid.StageID);
                var success = dal.SaveTravelContact(model, travelid, ContStageid);
                if (success)
                    result = HRMS.Resources.Success.ResourceManager.GetString("SaveContactDetailsSuccess");
                else
                    result = HRMS.Resources.Errors.ResourceManager.GetString("SaveContactDetailsError");

                string TravelID = Commondal.Encrypt(Convert.ToString(Session["SecurityKey"].ToString() + (model.TravelId.HasValue ? travelid : 0).ToString()), true);
                return Json(new { resultMesssage = result, status = success, travelId = TravelID }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddTravelEmergencyContactDetails(TravelEmergencyContactViewModel model, int UniqueID, int TravelId)
        {
            try
            {
                TravelDAL dal = new TravelDAL();
                string result = string.Empty;
                string decryptedTravelId = Session["Travelid"].ToString();
                var success = dal.AddTravelEmergencyContactDetails(model, UniqueID, TravelId, decryptedTravelId);
                return Json(new { status = success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        [HttpGet]
        public ActionResult GetTravelFormDetailsIndex(bool IsNewForm, string EncryptedTravelID, string viewDetails)
        {
            try
            {
                TravelViewModel model = new TravelViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                ViewBag.AsciiKey = Session["SecurityKey"].ToString();
                string employeeCode = Membership.GetUser().UserName;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);

                Session["Stageid"] = null;
                string encyptedEmploeeId = Commondal.Encrypt(this.Session["SecurityKey"].ToString() + Convert.ToString(employeeId), true);

                if (EncryptedTravelID != null)
                {
                    string DecryptedTravelID = Commondal.Decrypt(Convert.ToString(EncryptedTravelID), true);
                    string travlid = DecryptedTravelID.Replace(Session["SecurityKey"].ToString(), "");
                    Tbl_HR_Travel travelDetails = travelDAL.GetTravelDetailsfromTravelID(Convert.ToInt32(travlid));
                    if (travelDetails.TravelExtensionEndDate != null)
                    {
                        ViewBag.Extensionstatus = "Yes";
                    }
                    ViewBag.AdminApproverID = travelDetails.AdminApproverId;
                    ViewBag.EncryptedTravelEmployeeID = Convert.ToString(travelDetails.EmployeeId);
                    ViewBag.RequestorID = travelDetails.RequestorId;
                    ViewBag.Details = viewDetails;
                    ViewBag.StageID = travelDetails.StageID;
                    Session["ViewExt"] = viewDetails;
                    TravelDAL dal = new TravelDAL();
                    bool loggedinUser = dal.CheckForTheLogged(employeeId, travlid);
                    ViewBag.IsLoggedInUser = loggedinUser;
                    if (IsNewForm == false)
                    {
                        string viewdet = Session["ViewExt"].ToString();
                        ViewBag.Viewstats = viewdet;
                    }
                }
                else
                    EncryptedTravelID = Commondal.Encrypt(this.Session["SecurityKey"].ToString() + "0", true);
                ViewBag.encryptedEmployeeId = encyptedEmploeeId;
                ViewBag.EncryptedTravelEmployeeID = encyptedEmploeeId;
                ViewBag.EncryptedTravelId = EncryptedTravelID;
                model.IsNewForm = IsNewForm;
                string[] role = Roles.GetRolesForUser(employeeCode);
                Session["Btnsubmitstatus"] = null;
                if (IsNewForm == true || IsNewForm == false)
                {
                    Session["Conter"] = Convert.ToInt32(0);
                    ViewBag.Counter = Session["Conter"].ToString();
                    Session["Travelid"] = null;
                }
                if (employeeCode != null)
                {
                    model.SearchedUserDetails.EmployeeId = employeeId;
                    model.SearchedUserDetails.EmployeeCode = employeeCode;
                    model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                    if (User.IsInRole("Travel_Admin"))
                    {
                        ViewBag.UserRole = "Travel_Admin";
                    }
                }

                Session["Extensiton"] = viewDetails;
                ViewBag.Extension = viewDetails;
                return View("TravelDetailsFormIndex", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        [PageAccess(PageName = "Travel")]
        public ActionResult GetTravelFormDetails(bool IsNewForm, string encryptedTravelId, string viewDetails, string isExtensionOfOriginalForm)
        {
            try
            {
                Session["SearchEmpFullName"] = null;  // to hide emp search
                Session["SearchEmpCode"] = null;
                Session["SearchEmpID"] = null;

                bool isAuthorizeExpense;
                Session["isTravelViewDetails"] = viewDetails;
                string decryptedTravelId = string.Empty;
                if (IsNewForm == true)
                    encryptedTravelId = null;
                if (encryptedTravelId != null)
                {
                    decryptedTravelId = HRMSHelper.Decrypt(encryptedTravelId, out isAuthorizeExpense);
                    if (!isAuthorizeExpense)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedTravelId = "0";
                if (Convert.ToInt16(decryptedTravelId) == 0)
                {
                    Session["Conter"] = Convert.ToInt32(0);
                    Session["Travelid"] = null;
                    Session["ViewExt"] = "No";
                    Session["Stageid"] = null;
                    Session["Btnsubmitstatus"] = null;
                    Session["Extensiton"] = "No";
                }
                int Travelid;
                CountryDetailsList objDefault = new CountryDetailsList();
                if (viewDetails == "Ext")
                {
                    ViewBag.IsExt = viewDetails;
                }
                if (Session["Travelid"] == null && viewDetails != "yes")
                {
                    Travelid = 0;
                    if (viewDetails == "Ext" && decryptedTravelId != null)
                    {
                        Session["Travelid"] = decryptedTravelId;
                        string decryptedTravelIdNew = Session["Travelid"].ToString();
                        Travelid = Convert.ToInt32(decryptedTravelIdNew);
                        Session["Extensiton"] = "Ext";
                    }
                }
                else
                {
                    Session["Travelid"] = decryptedTravelId;
                    string decryptedTravelIdNew = Session["Travelid"].ToString();
                    Travelid = Convert.ToInt32(decryptedTravelIdNew);
                }
                if (IsNewForm == false)
                {
                    ViewBag.Viewstats = Session["ViewExt"].ToString();
                }
                TravelViewModel model = new TravelViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();

                TravelDAL dal = new TravelDAL();
                Tbl_HR_Travel TravelDetails = new Tbl_HR_Travel();
                Tbl_HR_Travel TravelDetailsExtension = new Tbl_HR_Travel();
                string employeeCode = Membership.GetUser().UserName;
                int locationid = 0;
                tbl_PM_Location location = null;
                model.IsNewForm = IsNewForm;
                ViewBag.IsNewForm = IsNewForm;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                model.TravelEmployeeCode = Convert.ToInt32(employeeCode);
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = employeeId;
                model.ProjectNameList = dal.GetProjectList();
                tbl_PM_GroupMaster employeeDeliveryTeam = null;
                ViewBag.loginUserId = employeeId;
                ViewBag.loginReimbursementEmployeeId = Commondal.Encrypt(Convert.ToString(Session["SecurityKey"].ToString() + employeeId), true);

                if (IsNewForm == false && decryptedTravelId != null)
                {
                    if (viewDetails == "NoExt" && decryptedTravelId != null && Travelid == 0)
                        TravelDetailsExtension = dal.GetTravelDetails(Convert.ToInt32(decryptedTravelId));
                    else
                        TravelDetailsExtension = dal.GetTravelDetails(Convert.ToInt32(Travelid));
                }
                else
                {
                    TravelDetails = dal.GetTravelDetails(Convert.ToInt32(Travelid));
                }
                HRMS_tbl_PM_Employee employeeDetails = new HRMS_tbl_PM_Employee();
                employeeDetails = employeeDAL.GetEmployeeDetails(employeeId);
                if (TravelDetails != null && Session["Travelid"] != null && IsNewForm == true)
                {
                    model.TravelEmployeeId = TravelDetails.EmployeeId.HasValue ? TravelDetails.EmployeeId.Value : 0;
                    ViewBag.encryptedTravelEmployeeId = Commondal.Encrypt(Convert.ToString(Session["SecurityKey"].ToString() + (TravelDetails.EmployeeId.HasValue ? TravelDetails.EmployeeId.Value : 0)), true);
                    model.TravelTRFNo = TravelDetails.TRFNo;
                    model.TravelId = TravelDetails.TravelId;
                    HRMS_tbl_PM_Employee employeeDeltravel = employeeDAL.GetEmployeeDetails(Convert.ToInt32(TravelDetails.EmployeeId));
                    ViewBag.TravelEmp = TravelDetails.EmployeeId;
                    model.TravelEmployeeCode = Convert.ToInt32(employeeDeltravel.EmployeeCode);
                    model.TravelEmployeeName = employeeDeltravel.EmployeeName;
                    model.RequestDate = TravelDetails.RequestDate;
                    model.ProjectName = TravelDetails.ProjectName;
                    model.RequesterId = TravelDetails.RequestorId;
                    //model.GroupheadApprover = TravelDetails.GroupHeadId;
                    model.ProjectManagerApprover = TravelDetails.ProjectManagerId;
                    model.AdminApprover = TravelDetails.AdminApproverId;
                    if (TravelDetails.TravelToCountry == 0)
                    {
                        objDefault.CountryId = 0;
                        objDefault.CountryName = "Select";
                        model.CurrentCountryList = dal.GetTravelCountryDetails();
                        model.CurrentCountryList.Insert(0, objDefault);
                    }
                    else
                    {
                        model.TravelToCountry = TravelDetails.TravelToCountry;
                        model.CurrentCountryList = dal.GetTravelCountryDetails();
                    }

                    model.TravelStartDate = TravelDetails.TravelStartDate;
                    model.TravelEndDate = TravelDetails.TravelEndDate;
                    //model.POCDetail = TravelDetails.POCDetail;
                    model.AdditionalInfo = TravelDetails.AdditionalInfo;
                    model.TravelType = TravelDetails.TravelTypeId;
                    //model.IsValidVisa = Convert.ToInt32(TravelDetails.IsValidVisa);
                    // model.TravellingAbroadForFirstTime = Convert.ToInt32(TravelDetails.TravellingAbroadForFirstTime);
                    model.ExpenseReimbursedByClient = Convert.ToInt32(TravelDetails.ExpenseReimbursedByClient);
                    //model.IsPointOfContact = Convert.ToInt32(TravelDetails.IsPointOfContact);
                    model.ContactNoAbroad = TravelDetails.ContactNoAbroad;

                    employeeDeliveryTeam = dal.GetDeliveryTeamName(Convert.ToInt32(employeeDeltravel.GroupID));
                    if (employeeDeliveryTeam != null)
                        model.DeliveryTeam = employeeDeliveryTeam.GroupName;

                    locationid = Convert.ToInt32(employeeDeltravel.LocationID);
                    location = dal.getOrganizationUnitDetails(locationid);
                    if (location != null)
                        model.OrganizationUnit = Convert.ToString(location.Location);

                    model.StageID = TravelDetails.StageID;
                    ViewBag.StageID = TravelDetails.StageID;
                    model.TravelEmployeeId = Convert.ToInt32(TravelDetails.EmployeeId);
                    Session["Travelid"] = TravelDetails.TravelId;
                }
                else
                {
                    model.TravelEmployeeId = employeeId;
                    ViewBag.encryptedTravelEmployeeId = Commondal.Encrypt(Convert.ToString(Session["SecurityKey"].ToString() + Convert.ToString(0)), true);
                }

                if (TravelDetailsExtension != null && IsNewForm == false)
                {
                    model.TravelEmployeeId = TravelDetailsExtension.EmployeeId.HasValue ? TravelDetailsExtension.EmployeeId.Value : 0;
                    ViewBag.encryptedTravelEmployeeId = Commondal.Encrypt(Convert.ToString(Session["SecurityKey"].ToString() + (TravelDetailsExtension.EmployeeId.HasValue ? TravelDetailsExtension.EmployeeId.Value : 0)), true);
                    model.TravelId = TravelDetailsExtension.TravelId;
                    model.TravelTRFNo = TravelDetailsExtension.TRFNo;
                    HRMS_tbl_PM_Employee employeeDel = employeeDAL.GetEmployeeDetails(Convert.ToInt32(TravelDetailsExtension.EmployeeId));
                    ViewBag.TravelEmp = TravelDetailsExtension.EmployeeId;
                    model.TravelEmployeeCode = Convert.ToInt32(employeeDel.EmployeeCode);
                    model.TravelEmployeeName = employeeDel.EmployeeName;
                    model.RequestDate = TravelDetailsExtension.RequestDate;
                    model.ProjectName = TravelDetailsExtension.ProjectName;
                    model.RequesterId = TravelDetailsExtension.RequestorId;
                    //model.GroupheadApprover = TravelDetailsExtension.GroupHeadId;
                    model.ProjectManagerApprover = TravelDetailsExtension.ProjectManagerId;
                    model.AdminApprover = TravelDetailsExtension.AdminApproverId;
                    if (TravelDetailsExtension.TravelToCountry == 0)
                    {
                        objDefault.CountryId = 0;
                        objDefault.CountryName = "Select";
                        model.CurrentCountryList = dal.GetTravelCountryDetails();
                        model.CurrentCountryList.Insert(0, objDefault);
                    }
                    else
                    {
                        model.TravelToCountry = Convert.ToInt32(TravelDetailsExtension.TravelToCountry);
                        model.CurrentCountryList = dal.GetTravelCountryDetails();
                    }

                    model.TravelStartDate = TravelDetailsExtension.TravelStartDate;
                    model.TravelEndDate = TravelDetailsExtension.TravelEndDate;
                    //model.POCDetail = TravelDetailsExtension.POCDetail;
                    model.AdditionalInfo = TravelDetailsExtension.AdditionalInfo;
                    model.TravelType = TravelDetailsExtension.TravelTypeId;
                    // model.IsValidVisa = Convert.ToInt32(TravelDetailsExtension.IsValidVisa);
                    // model.TravellingAbroadForFirstTime = Convert.ToInt32(TravelDetailsExtension.TravellingAbroadForFirstTime);
                    model.ExpenseReimbursedByClient = Convert.ToInt32(TravelDetailsExtension.ExpenseReimbursedByClient);
                    //model.IsPointOfContact = Convert.ToInt32(TravelDetailsExtension.IsPointOfContact);
                    model.ContactNoAbroad = TravelDetailsExtension.ContactNoAbroad;
                    employeeDeliveryTeam = dal.GetDeliveryTeamName(Convert.ToInt32(employeeDel.GroupID));
                    if (employeeDeliveryTeam != null)
                        model.DeliveryTeam = employeeDeliveryTeam.GroupName;
                    model.TravelExtexsionStartDate = TravelDetailsExtension.TravelExtexsionStartDate;
                    model.TravelExtensionEndDate = TravelDetailsExtension.TravelExtensionEndDate;
                    model.ImmigrationDate = TravelDetailsExtension.ImmigrationDate;
                    locationid = Convert.ToInt32(employeeDel.LocationID);
                    location = dal.getOrganizationUnitDetails(locationid);
                    if (location != null)
                        model.OrganizationUnit = Convert.ToString(location.Location);

                    if (model.TravelExtensionEndDate != null)
                    {
                        ViewBag.Viewstats = "NoExt";
                        Session["ViewExt"] = "NoExt";
                        ViewBag.GetExtStatus = "True";
                        model.TravelExtDetails = TravelDetailsExtension.TravelExtDetails;
                    }

                    model.StageID = TravelDetailsExtension.StageID;
                    Session["Stageid"] = TravelDetailsExtension.StageID;
                    Session["Travelid"] = TravelDetailsExtension.TravelId;
                    if (TravelDetailsExtension.StageID >= 0)
                        Session["Btnsubmitstatus"] = "Yes";
                }

                if (employeeDetails != null && IsNewForm == true)
                {
                    model.TravelEmployeeName = employeeDetails.EmployeeName;
                }

                bool loggedinUser = dal.CheckForTheLogged(employeeId, decryptedTravelId);
                ViewBag.IsLoggedInUser = loggedinUser;
                ViewBag.encryptedId = encryptedTravelId;
                ViewBag.viewDetailClick = viewDetails;

                model.DateOfSubmission = DateTime.Now.Date;
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                if (Session["Travelid"] != null)
                {
                    if (User.IsInRole("Travel_Admin") && (TravelDetails.StageID == 0 || TravelDetailsExtension.StageID == 0))
                    {
                        ViewBag.UserRole = "Travel_Admin";
                        model.SearchedUserDetails.UserRole = "Travel_Admin";
                    }
                    else if (User.IsInRole("Travel_Admin") && ((TravelDetails.StageID == 3 || TravelDetailsExtension.StageID == 3) || (TravelDetails.StageID == 4 || TravelDetailsExtension.StageID == 4)))
                    {
                        ViewBag.UserRole = "Travel_Admin";
                        model.SearchedUserDetails.UserRole = "Travel_Admin";
                    }
                    if (User.IsInRole("Travel Approver") && (TravelDetails.ProjectManagerId == employeeId || TravelDetailsExtension.ProjectManagerId == employeeId) && ((TravelDetails.StageID == 1 || TravelDetailsExtension.StageID == 1) || (TravelDetails.StageID == 2 || TravelDetailsExtension.StageID == 2) || (TravelDetails.StageID == 3 || TravelDetailsExtension.StageID == 3)))
                    {
                        ViewBag.UserRole = "TravelApprover";
                        model.SearchedUserDetails.UserRole = "TravelApprover";
                    }
                    if (User.IsInRole("Group Head") && (TravelDetails.GroupHeadId == null || TravelDetailsExtension.GroupHeadId == null) && (TravelDetails.StageID == 0 || TravelDetailsExtension.StageID == 0))
                    {
                        ViewBag.UserRole = "GroupHead";
                        model.SearchedUserDetails.UserRole = "GroupHead";
                    }
                    else if (User.IsInRole("Group Head") && (TravelDetails.GroupHeadId == employeeId || TravelDetailsExtension.GroupHeadId == employeeId) && (TravelDetails.StageID == 2 || TravelDetailsExtension.StageID == 2))
                    {
                        ViewBag.UserRole = "GroupHead";
                        model.SearchedUserDetails.UserRole = "GroupHead";
                    }
                    if (User.IsInRole("Management") && (TravelDetails.StageID == 0 || TravelDetailsExtension.StageID == 0))
                    {
                        ViewBag.UserRole = "Management";
                        model.SearchedUserDetails.UserRole = "Management";
                    }
                    else if (User.IsInRole("Account Owners") && (TravelDetails.StageID == 0 || TravelDetailsExtension.StageID == 0))
                    {
                        ViewBag.UserRole = "Account Owners";
                        model.SearchedUserDetails.UserRole = "Account Owners";
                    }
                    else if (User.IsInRole("Delivery Manager") && (TravelDetails.StageID == 0 || TravelDetailsExtension.StageID == 0))
                    {
                        ViewBag.UserRole = "Delivery Manager";
                        model.SearchedUserDetails.UserRole = "Delivery Manager";
                    }
                }
                else
                {
                    if (Roles.GetRolesForUser().Contains("Travel_Admin"))
                    {
                        ViewBag.UserRole = "Travel_Admin";
                        model.SearchedUserDetails.UserRole = "Travel_Admin";
                    }
                    else if (User.IsInRole("Management"))
                    {
                        ViewBag.UserRole = "Management";
                        model.SearchedUserDetails.UserRole = "Management";
                    }
                    else if (User.IsInRole("Group Head"))
                    {
                        ViewBag.UserRole = "GroupHead";
                        model.SearchedUserDetails.UserRole = "GroupHead";
                    }
                    else if (User.IsInRole("Account Owners"))
                    {
                        ViewBag.UserRole = "Account Owners";
                        model.SearchedUserDetails.UserRole = "Account Owners";
                    }
                    else if (User.IsInRole("Delivery Manager"))
                    {
                        ViewBag.UserRole = "Delivery Manager";
                        model.SearchedUserDetails.UserRole = "Delivery Manager";
                    }
                    else if (User.IsInRole("Travel_Admin"))
                    {
                        ViewBag.UserRole = "Travel_Admin";
                        model.SearchedUserDetails.UserRole = "Travel_Admin";
                    }
                    else if (User.IsInRole("Travel Approver"))
                    {
                        ViewBag.UserRole = "TravelApprover";
                        model.SearchedUserDetails.UserRole = "TravelApprover";
                    }
                }

                if (IsNewForm == false && decryptedTravelId != null)
                {
                    string viewdet = Session["ViewExt"].ToString();
                    ViewBag.Viewstats = viewdet;
                    string trfno = dal.GetEmployeeTrfNo(decryptedTravelId);
                    if (Session["Extensiton"].ToString() == "Ext" && isExtensionOfOriginalForm == "Yes")
                    {
                        var TRFNoAuto = dal.GetNewExtensionTRFNo(trfno);
                        bool statustrfno = dal.GetTRFNoIsValide(TRFNoAuto);
                        string NewExtTRFNo = string.Empty;

                        while (statustrfno == true)
                        {
                            TRFNoAuto = dal.GetNewExtensionTRFNo(TRFNoAuto);
                            bool finalstatus = dal.GetTRFNoIsValide(TRFNoAuto);
                            if (finalstatus == false)
                            {
                                statustrfno = false;
                            }
                        }
                        model.TravelTRFNo = Convert.ToString(TRFNoAuto);
                        model.IsNewForm = false;
                    }
                    else
                    {
                        if (trfno == "0")
                        {
                            trfno = "1";
                            model.TravelTRFNo = Convert.ToString(trfno);
                        }
                        else
                            model.TravelTRFNo = Convert.ToString(trfno);
                    }
                    if (Session["Extensiton"].ToString() == "Ext")
                    {
                        if (TravelDetailsExtension.StageID == 4)
                            model.StageID = 0;
                        ViewBag.Extension = Session["Extensiton"].ToString();
                    }
                    if (viewdet == "No")
                    {
                        Tbl_HR_Travel Fillformtravelid = dal.GetTravelidExtensionform(Convert.ToString(model.TravelTRFNo));
                        if (Fillformtravelid != null)
                            Session["Travelid"] = Fillformtravelid.TravelId;
                    }
                }
                if (IsNewForm == true && Session["Travelid"] == null)
                {
                    var TRFNoAuto = dal.GetNextTRFNo();
                    if (TRFNoAuto != "")
                    {
                        decimal d = Convert.ToDecimal(TRFNoAuto);
                        decimal num = Math.Round(d, 0);
                        int IncreMentTRFNo = Convert.ToInt32(num) + 1;
                        model.TravelTRFNo = Convert.ToString(IncreMentTRFNo);
                    }
                    else
                    {
                        model.TravelTRFNo = "1";
                    }
                }

                model.RequestDate = DateTime.Now.Date;

                if (Travelid == 0 && employeeDeliveryTeam == null && location == null)
                {
                    employeeDeliveryTeam = dal.GetDeliveryTeamName(Convert.ToInt32(employeeDetails.GroupID));
                    if (employeeDeliveryTeam != null)
                        model.DeliveryTeam = employeeDeliveryTeam.GroupName;

                    locationid = Convert.ToInt32(employeeDetails.LocationID);
                    location = dal.getOrganizationUnitDetails(locationid);
                    if (location != null)
                        model.OrganizationUnit = Convert.ToString(location.Location);

                    objDefault.CountryId = 0;
                    objDefault.CountryName = "Select";
                    model.CurrentCountryList = dal.GetTravelCountryDetails();
                    model.CurrentCountryList.Insert(0, objDefault);
                }

                model.TravelTypeList = dal.GetTravelTypes();
                model.ClientReimbursementList = dal.clientYesNoList();

                model.Mail = new TravelMailTemplate();

                //List of Travel_Approver employees
                Dictionary<int, string> ProjectManagerApprover = new Dictionary<int, string>();
                string[] TravelApproverUsers = Roles.GetUsersInRole("Travel Approver");
                foreach (string ExpenseApproverlist in TravelApproverUsers)
                {
                    HRMS_tbl_PM_Employee expenseEmployee = employeeDAL.GetEmployeeDetailsByEmployeeCode(ExpenseApproverlist);
                    if (expenseEmployee != null)
                        ProjectManagerApprover.Add(expenseEmployee.EmployeeID, expenseEmployee.EmployeeName);
                    ProjectManagerApprover = ProjectManagerApprover.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                }
                ViewBag.ProjectManagerApprovers = new SelectList(ProjectManagerApprover, "Key", "Value");

                //List of Travel_Approver employees
                //Dictionary<int, string> GroupHeadApprovers = new Dictionary<int, string>();
                //string[] GroupHeadApproverUsers = Roles.GetUsersInRole("Group Head");
                //foreach (string ExpenseApproverlist in GroupHeadApproverUsers)
                //{
                //    HRMS_tbl_PM_Employee expenseEmployee = employeeDAL.GetEmployeeDetailsByEmployeeCode(ExpenseApproverlist);
                //    if (expenseEmployee != null)
                //        GroupHeadApprovers.Add(expenseEmployee.EmployeeID, expenseEmployee.EmployeeName);
                //    GroupHeadApprovers = GroupHeadApprovers.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                //}
                //ViewBag.GroupHeadApprovers = new SelectList(GroupHeadApprovers, "Key", "Value");

                //List of Travel_Admin employees
                Dictionary<int, string> FinanceApprovers = new Dictionary<int, string>();
                string[] financeApproverUsers = Roles.GetUsersInRole("Travel_Admin");
                foreach (string financeApproverUserList in financeApproverUsers)
                {
                    HRMS_tbl_PM_Employee financeEmployee = employeeDAL.GetEmployeeDetailsByEmployeeCode(financeApproverUserList);
                    if (financeEmployee != null)
                        FinanceApprovers.Add(financeEmployee.EmployeeID, financeEmployee.EmployeeName);
                    FinanceApprovers = FinanceApprovers.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                }

                Session["Conter"] = Convert.ToInt32(Session["Conter"]) + 1;
                ViewBag.Counter = Session["Conter"].ToString();
                //model.ContactDeatails = new ContactViewModel();
                //model.PassportDetails = new PassportViewModel();
                //model.VisaDetails = new VisaViewModel();
                //model.JourneyDetails = new JourneyViewModel();
                //model.ClientDetails = new ClientViewModel();
                //model.AccomodationDetails = new AccomodationViewModel();
                //model.VisaDetails.countryList = new List<Country>();
                Session["CountryId"] = 0;
                model.OtherAdminDetails = new OtherAdminViewModel();
                model.OtherAdminDetails.RequirementID = 0;
                ViewBag.RelationTypeList = dal.GetRelationList();
                ViewBag.financeApprovers = new SelectList(FinanceApprovers, "Key", "Value");
                if (Session["Travelid"] != null)
                {
                    model.ContactDeatails = ContactDetails(loggedinUser);
                    model.PassportDetails = PassportDetails(encryptedTravelId);
                    model.VisaDetails = VisaDetails(encryptedTravelId, model.TravelEmployeeId);
                    model.JourneyDetails = JourneyDetails(encryptedTravelId);
                    model.ClientDetails = ClientDetails();
                    model.AccomodationDetails = AccomodationDetails(ViewBag.loginReimbursementEmployeeId, encryptedTravelId);
                    if ((model.StageID >= 3 && ViewBag.UserRole == "Travel_Admin" && ViewBag.IsLoggedInUser != true) || (model.SearchedUserDetails.EmployeeId == ViewBag.RequestorID) || (model.StageID == 4 && (ViewBag.Viewstats != "NoExt" || (ViewBag.viewDetailClick == "yes" && model.StageID == 4)) && (ViewBag.Extension != "Ext" || (ViewBag.viewDetailClick == "yes" && model.StageID == 4))))
                    {
                        model.AccomodationAdminDetails = AccomodationAdminDetails();
                        model.ConveyanceDetails = ConveyanceDetails();
                        model.OtherAdminDetails = OtherAdminDetails(model.TravelEmployeeId);
                    }
                }

                /// For Contact Details
                bool isSaved = false;
                var travelContact = dbContext.Tbl_HR_TravelContact.Where(x => x.TravelId == model.TravelId).FirstOrDefault();

                if (travelContact == null)
                {
                    Tbl_HR_Travel UserEmployeeId = dal.GetTravelDetailsfromTravelID(model.TravelId);
                    HRMS_tbl_PM_Employee objContactDetails = dal.GetEmployeePersonalDetails(employeeId);
                    if (UserEmployeeId != null)
                    {
                        isSaved = dal.SaveTravelContact(objContactDetails, model.TravelId);
                    }
                    Tbl_HR_TravelContact objtravelContact = dal.GetTravelContact(model.TravelId);

                    if (objtravelContact != null)
                    {
                        model.PersonalEmailId = objtravelContact.PersonalEmailId;
                        model.userPersonalEmailId = objtravelContact.PersonalEmailId;
                        model.ContactNoIndia = objtravelContact.ContactNoIndia;
                        model.ContactNoAbroad = objtravelContact.ContactNoAbroad;
                    }
                    else if (objContactDetails != null)
                    {
                        model.userPersonalEmailId = objContactDetails.EmailID1;
                        model.ContactNoIndia = objContactDetails.MobileNumber;
                    }
                    ///
                }
                else
                {
                    model.userPersonalEmailId = travelContact.PersonalEmailId;
                    model.ContactNoIndia = travelContact.ContactNoIndia;
                }

                ViewBag.TRFNo = model.TravelTRFNo;

                return PartialView("_TravelDetailsForm", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        //[HttpGet]
        //public ActionResult GetPassportFormDetails(string encryptedTravelId)
        //{
        //try
        //{
        private PassportViewModel PassportDetails(string encryptedTravelId)
        {
            string decryptedTravelId = Session["Travelid"].ToString();
            if (Session["Stageid"] != null)
            {
                ViewBag.StageID = Session["Stageid"].ToString();
            }
            PassportViewModel model = new PassportViewModel();
            TravelDAL DAL = new TravelDAL();
            Tbl_HR_Travel TravelDetails = DAL.GetTravelDetails(Convert.ToInt32(decryptedTravelId));
            if (TravelDetails != null)
            {
                model = DAL.getPassportDetails(TravelDetails.EmployeeId.HasValue ? TravelDetails.EmployeeId.Value : 0);
                model.EmployeeID = TravelDetails.EmployeeId.HasValue ? TravelDetails.EmployeeId.Value : 0;
            }
            else
            {
                EmployeeDAL EmployeeDAL = new EmployeeDAL();
                model = DAL.getPassportDetails(EmployeeDAL.GetEmployeeID(Membership.GetUser().UserName));
            }
            ViewBag.encryptedTravelId = encryptedTravelId;
            model.TravelID = Convert.ToInt32(decryptedTravelId);

            string employeeCode = Membership.GetUser().UserName;
            EmployeeDAL employeeDAL = new EmployeeDAL();
            int employeeId = employeeDAL.GetEmployeeID(employeeCode);
            bool loggedinUser = DAL.CheckForTheLogged(employeeId, decryptedTravelId);
            ViewBag.IsLoggedInUser = loggedinUser;
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);

            if (User.IsInRole("Travel Approver"))
            {
                ViewBag.UserRole = "TravelApprover";
            }
            if (User.IsInRole("Group Head"))
            {
                ViewBag.UserRole = "GroupHead";
            }
            if (User.IsInRole("Travel_Admin"))
            {
                ViewBag.UserRole = "Travel_Admin";
            }

            model.SearchedUserDetails = new SearchedUserDetails();
            return model;
            //}
            //catch (Exception)
            //{
            //    return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            //}
        }

        [HttpPost]
        public ActionResult PassportLoadGrid(int page, int rows, int EmployeeID)
        {
            try
            {
                TravelDAL dal = new TravelDAL();
                EmployeeDAL empdal = new EmployeeDAL();
                PassportViewModel Passport = new PassportViewModel();
                int totalCount;
                int LoggedInEmployeeId = empdal.GetEmployeeID(Membership.GetUser().UserName);

                List<PassportViewModel> passportDetails = dal.GetPassportDetails(page, rows, EmployeeID, out totalCount);
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);

                if ((passportDetails == null) && page - 1 > 0)
                {
                    page = page - 1;
                    passportDetails = dal.GetPassportDetails(page, rows, EmployeeID, out totalCount);
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
            catch
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadPassportDetails(HttpPostedFileBase doc, PassportViewModel model)
        {
            try
            {
                if (doc.ContentLength > 0)
                {
                    string uploadsPath = (UploadFileLocation);
                    string uploadpathwithId = Path.Combine(uploadsPath, (model.EmployeeID).ToString());
                    uploadsPath = Path.Combine(uploadpathwithId, Convert.ToString(model.TravelID));
                    string fileName = Path.GetFileName(doc.FileName);
                    model.PassportFileName = fileName;
                    model.PassportFilePath = uploadsPath;
                    TravelDAL DAL = new TravelDAL();
                    bool result = DAL.SavePassportDetails(model);

                    string filePath = Path.Combine(uploadsPath, fileName);
                    if (!Directory.Exists(uploadsPath))
                        Directory.CreateDirectory(uploadsPath);

                    doc.SaveAs(filePath);

                    if (result)
                        return Json(new { status = true }, "text/html", JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { status = false }, "text/html", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { status = false }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult showPassportDetails(int EmployeeID, int DocumentID)
        {
            try
            {
                PassportViewModel model = new PassportViewModel();
                TravelDAL DAL = new TravelDAL();
                //PassportViewModel employeeDetails = DAL.getEmployeeDetailsFromtravelID(TravelID);
                model.SearchedUserDetails = new SearchedUserDetails();
                PassportViewModel employeeDetails = DAL.GetPassportShowHistory(EmployeeID, DocumentID);
                model.PassportFileName = employeeDetails.PassportFileName;
                model.CreatedDate = employeeDetails.CreatedDate;
                model.DocumentID = employeeDetails.DocumentID;
                model.EmployeeID = EmployeeID;
                //model.TravelID = TravelID;
                return PartialView("_PassportDetails", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult DownloadPassportFile(string PassportFileName, int DocumentID, int EmployeeID)
        {
            try
            {
                TravelDAL DAL = new TravelDAL();
                PassportViewModel Details = DAL.GetPassportShowHistory(EmployeeID, DocumentID);
                string[] FileExtention = PassportFileName.Split('.');
                string contentType = "application/" + FileExtention[1];
                string Filepath = Path.Combine(Details.PassportFilePath, PassportFileName);
                if (!System.IO.File.Exists(Filepath))
                {
                    throw new Exception();
                }
                return File(Filepath, contentType, PassportFileName);
            }
            catch (Exception)
            {
                ConfigurationViewModel model = new ConfigurationViewModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                string employeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(employeeCode);
                if (employeeCode != null)
                {
                    CommonMethodsDAL Commondal = new CommonMethodsDAL();
                    model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }
                return PartialView("_FileNotFound", model);
            }
        }

        public ActionResult DownloadTravelVisaFile(string VisaFileName, int VisaTravelID)
        {
            try
            {
                TravelDAL DAL = new TravelDAL();
                VisaViewModel Details = DAL.GetTravelVisaShowHistory(VisaTravelID);
                string[] FileExtention = VisaFileName.Split('.');
                string contentType = "application/" + FileExtention[1];
                string Filepath = Path.Combine(Details.VisaFilePath, VisaFileName);
                if (!System.IO.File.Exists(Filepath))
                {
                    throw new Exception();
                }
                return File(Filepath, contentType, VisaFileName);
            }
            catch (Exception)
            {
                ConfigurationViewModel model = new ConfigurationViewModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                string employeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(employeeCode);
                if (employeeCode != null)
                {
                    CommonMethodsDAL Commondal = new CommonMethodsDAL();
                    model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }
                return PartialView("_FileNotFound", model);
            }
        }

        public ActionResult DownloadOtherRequireUploadedFile(int TravelID, int RequirementID)
        {
            try
            {
                TravelDAL DAL = new TravelDAL();
                OtherAdminViewModel Details = DAL.GetOtherRequireDetailsShowHistory(TravelID, RequirementID);
                string FileName = Details.FileName;
                string[] FileExtention = FileName.Split('.');
                string contentType = "application/" + FileExtention[1];
                string Filepath = Path.Combine(Details.FilePath, FileName);
                if (!System.IO.File.Exists(Filepath))
                {
                    throw new Exception();
                }
                return File(Filepath, contentType, FileName);
            }
            catch (Exception)
            {
                ConfigurationViewModel model = new ConfigurationViewModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                string employeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(employeeCode);
                if (employeeCode != null)
                {
                    CommonMethodsDAL Commondal = new CommonMethodsDAL();
                    model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }
                return PartialView("_FileNotFound", model);
            }
        }

        [HttpPost]
        public ActionResult DeletePassportDocument(int DocumentID)
        {
            try
            {
                bool status = false;
                TravelDAL DAL = new TravelDAL();
                status = DAL.DeletePassportDetails(DocumentID);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        private VisaViewModel VisaDetails(string encryptedTravelId, int travelEmployeeId)
        {
            string decryptedTravelID = Session["Travelid"].ToString();

            DAL.TravelDAL dal = new DAL.TravelDAL();
            VisaViewModel model = new VisaViewModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            if (Session["Stageid"] != null)
            {
                ViewBag.StageID = Session["Stageid"].ToString();
            }
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            string user = Commondal.GetMaxRoleForUser(role);
            model.SearchedUserDetails.UserRole = user;
            ViewBag.TravelEmployeeId = travelEmployeeId;
            ViewBag.LoggedInTravelempId = empdal.GetEmployeeID(Membership.GetUser().UserName);
            ViewBag.EncryptedTravelID = encryptedTravelId;
            model.VisaTravelID = Convert.ToInt32(decryptedTravelID);
            string employeeCode = Membership.GetUser().UserName;
            EmployeeDAL employeeDAL = new EmployeeDAL();
            int employeeId = employeeDAL.GetEmployeeID(employeeCode);
            bool loggedinUser = dal.CheckForTheLogged(employeeId, decryptedTravelID);
            ViewBag.IsLoggedInUser = loggedinUser;

            List<Country> countryList = dal.GetCountryDetails();
            int travelIdVisa = Convert.ToInt32(decryptedTravelID);
            List<Country> countryListVisa = new List<Country>();
            Tbl_HR_Travel travelDetails = dbContext.Tbl_HR_Travel.Where(x => x.TravelId == travelIdVisa).FirstOrDefault();
            dal.saveTravelVisaDetail(travelDetails);
            foreach (var visaitem in countryList)
            {
                if (travelDetails.TravelToCountry == visaitem.CountryID)
                    countryListVisa.Add(visaitem);
            }
            model.countryList = countryListVisa;

            List<VisaType> visatypeList = dal.GetVisaTypeDetails();
            model.visatypeList = visatypeList;
            model.EmployeeId = travelDetails.EmployeeId;
            return model;
        }

        [HttpPost]
        public ActionResult TravelVisaDetailLoadGrid(string TravelId, int page, int rows)
        {
            try
            {
                string decryptedTravelId = Session["Travelid"].ToString();
                string CountryidTravel = Session["CountryId"].ToString();
                TravelDAL dal = new TravelDAL();
                VisaViewModel objTravelVisadetails = new VisaViewModel();
                objTravelVisadetails.viewmodellist = new List<VisaViewModel>();
                int totalCount = 0;

                objTravelVisadetails.viewmodellist = dal.GetTravelVisaDetails(Convert.ToInt32(decryptedTravelId), page, rows, out totalCount, CountryidTravel);

                if ((objTravelVisadetails.viewmodellist == null || objTravelVisadetails.viewmodellist.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    objTravelVisadetails.viewmodellist = dal.GetTravelVisaDetails(Convert.ToInt32(decryptedTravelId), page, rows, out totalCount, CountryidTravel);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = objTravelVisadetails.viewmodellist,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult ShowTravelVisaDetails(int VisaTravelID)
        {
            try
            {
                VisaViewModel model = new VisaViewModel();
                TravelDAL DAL = new TravelDAL();

                VisaViewModel employeeDetails = DAL.GetTravelVisaShowHistory(VisaTravelID);
                model.VisaFileName = employeeDetails.VisaFileName;
                model.CreatedDate = employeeDetails.CreatedDate;
                model.VisaTravelID = employeeDetails.VisaTravelID;
                return PartialView("_VisaDetails", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult TravelConveynceLoadGrid(string TravelId, int page, int rows)
        {
            try
            {
                string decryptedTravelId = Session["Travelid"].ToString();

                TravelDAL dal = new TravelDAL();
                ConveyanceAdminViewModel objTravelVisadetails = new ConveyanceAdminViewModel();
                objTravelVisadetails.viewmodellist = new List<ConveyanceAdminViewModel>();
                int totalCount = 0;

                objTravelVisadetails.viewmodellist = dal.GetConvaynancesDetails(Convert.ToInt32(decryptedTravelId), page, rows, out totalCount);

                if ((objTravelVisadetails.viewmodellist == null || objTravelVisadetails.viewmodellist.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    objTravelVisadetails.viewmodellist = dal.GetConvaynancesDetails(Convert.ToInt32(decryptedTravelId), page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = objTravelVisadetails.viewmodellist,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveTravelVisaDetailsInfo(VisaViewModel model, int? SelectedCountryID, int? SelectedVisaTypeID)
        {
            try
            {
                DAL.TravelDAL dal = new DAL.TravelDAL();
                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);
                bool result = false;
                //bool isNoFileUploaded = false;
                //if (doc != null && doc.ContentLength > 0)
                //{
                string destinationPath = (UploadAdminVisaLocation);
                destinationPath = Path.Combine(destinationPath, (model.EmployeeId).ToString());
                //string fileName = Path.GetFileName(doc.FileName);
                tbl_HR_VisaUploadDetailsTravel uploadDetails = travelDAL.GetVisaUploadDetails(travelid, model.VisaTravelID);
                if (uploadDetails != null)
                {
                    model.VisaFileName = uploadDetails.FileName;
                    model.VisaFilePath = destinationPath;
                    string sourceFilePath = Path.Combine(uploadDetails.FilePath, uploadDetails.FileName);
                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        if (!Directory.Exists(destinationPath))
                            Directory.CreateDirectory(destinationPath);
                        System.IO.File.Copy(sourceFilePath, Path.Combine(destinationPath, uploadDetails.FileName), true);
                        System.IO.File.Delete(sourceFilePath);
                        bool isDeleted = travelDAL.DeleteVisaUploadRecord(travelid, model.VisaTravelID);
                    }
                }
                TravelDAL DAL = new TravelDAL();
                result = dal.SaveTravelVisaDetails(model, decryptedTravelId, SelectedCountryID, SelectedVisaTypeID);

                //string filePath = Path.Combine(uploadsPath, fileName);
                //if (!Directory.Exists(uploadsPath))
                //    Directory.CreateDirectory(uploadsPath);

                //doc.SaveAs(filePath);
                //}
                //else
                //{
                //    isNoFileUploaded = true;
                //    result = dal.SaveTravelVisaDetails(model, decryptedTravelId, isNoFileUploaded, SelectedCountryID, SelectedVisaTypeID);
                //}
                return Json(new { status = result }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult SaveTravelVisaUploadsDetailsInfo(HttpPostedFileBase doc, VisaViewModel model)
        {
            try
            {
                bool success = false;
                if (model != null)
                {
                    string decryptedTravelId = Session["Travelid"].ToString();
                    int TravelId = Convert.ToInt32(decryptedTravelId);
                    if (doc != null && doc.ContentLength > 0)
                    {
                        string uploadsPath = (System.Configuration.ConfigurationManager.AppSettings["UploadAdminVisaTempFileLocation"]);
                        uploadsPath = Path.Combine(uploadsPath, Convert.ToString(decryptedTravelId));
                        string fileName = Path.GetFileName(doc.FileName);
                        model.VisaFileName = fileName;
                        model.VisaFilePath = uploadsPath;
                        string filePath = Path.Combine(uploadsPath, fileName);
                        if (!Directory.Exists(uploadsPath))
                            Directory.CreateDirectory(uploadsPath);
                        doc.SaveAs(filePath);
                        success = travelDAL.SaveVisaUploadDetails(model, TravelId);
                    }
                }
                return Json(new { status = success, FileName = model.VisaFileName, VisaId = model.VisaTravelID }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveConvaynanceDetailsInfo(ConveyanceAdminViewModel model, int? ConveyanceTypeId, string HotelToAirport)
        {
            DAL.TravelDAL dal = new DAL.TravelDAL();
            string decryptedTravelId = Session["Travelid"].ToString();
            int travelid = Convert.ToInt32(decryptedTravelId);
            int ConveyanceId = Convert.ToInt32(ConveyanceTypeId);
            try
            {
                string resultMessage = string.Empty;

                var status = dal.SaveConavanaceDetails(model, travelid, ConveyanceId, HotelToAirport);
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

        public ActionResult DeleteTravelVisaDetails(string TravelId, string IDs)
        {
            try
            {
                TravelDAL dal = new TravelDAL();
                bool eq = dal.DeleteTravelVisaDetailsInfo(Convert.ToInt32(TravelId), Convert.ToInt32(IDs));
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteConvaynaceDetails(string ConvaytravelId, string travelId)
        {
            try
            {
                TravelDAL dal = new TravelDAL();
                bool eq = dal.DeleteConvaynanceDetailsInfo(Convert.ToInt32(ConvaytravelId), Convert.ToInt32(travelId));
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpGet]
        //public ActionResult GetOtherAdminFormDetails(string TravelEmployeeId, string TravelID)
        //{
        //    try
        //    {
        private OtherAdminViewModel OtherAdminDetails(int TravelEmployeeId)
        {
            //bool isAuthorize;
            //string decryptedempTravelID = HRMSHelper.Decrypt(TravelEmployeeId, out isAuthorize);
            //if (!isAuthorize)
            //    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

            string decryptedTravelId = Session["Travelid"].ToString();
            int travelid = Convert.ToInt32(decryptedTravelId);
            ViewBag.Travelid = travelid;
            DAL.TravelDAL dal = new DAL.TravelDAL();
            OtherAdminViewModel model = new OtherAdminViewModel();
            model.SearchedUserDetails = new SearchedUserDetails();

            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            if (User.IsInRole("Travel_Admin"))
            {
                ViewBag.UserRole = "Travel_Admin";
                model.SearchedUserDetails.UserRole = "Travel_Admin";
            }
            if (Session["Stageid"] != null)
            {
                ViewBag.StageID = Session["Stageid"].ToString();
            }
            string employeeCode = Membership.GetUser().UserName;
            EmployeeDAL employeeDAL = new EmployeeDAL();
            int employeeId = employeeDAL.GetEmployeeID(employeeCode);
            bool loggedinUser = dal.CheckForTheLogged(employeeId, decryptedTravelId);
            ViewBag.IsLoggedInUser = loggedinUser;
            model.otheradminViewmodelList = new List<OtherAdminViewModel>();
            List<tbl_HR_TravelOtherRequirement> otherDetails = dal.GetTravelOtherRequirementDetails(travelid);

            if (otherDetails != null)
            {
                foreach (tbl_HR_TravelOtherRequirement eachTravelOtherdetails in otherDetails)
                {
                    model.otheradminViewmodelList.Add(new OtherAdminViewModel()
                    {
                        ID = Convert.ToInt32(eachTravelOtherdetails.ID),
                        RequirementID = Convert.ToInt32(eachTravelOtherdetails.RequirementID),
                        Description = dal.GetRequirementTypeName(Convert.ToInt32(eachTravelOtherdetails.TypeID)),
                        ReceivedByEmployee = eachTravelOtherdetails.ReceivedByEmployee,
                        FilePath = eachTravelOtherdetails.FilePath,
                        FileName = eachTravelOtherdetails.FileName,
                        Miscdetails = eachTravelOtherdetails.Details
                    });
                }
            }

            model.CurrencyList = dal.GetCurrencyList();
            List<RequirementType> TypeList = dal.GetRequiremetTypeDetails();
            model.requirementTypeList = TypeList;

            List<EmployeeAcceptance> acclist = dal.GetAcceptanceStatus();
            model.employeeAceptionsList = acclist;
            return model;
            //}
            //catch (Exception)
            //{
            //    return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            //}
        }

        [HttpPost]
        public ActionResult TravelOtherRequiementDetailLoadGrid(string TravelId, int page, int rows)
        {
            try
            {
                string decryptedTravelId = Session["Travelid"].ToString();

                TravelDAL dal = new TravelDAL();
                OtherAdminViewModel objTravelOtherReqdetails = new OtherAdminViewModel();
                objTravelOtherReqdetails.otheradminViewmodelList = new List<OtherAdminViewModel>();
                int totalCount = 0;

                objTravelOtherReqdetails.otheradminViewmodelList = dal.GetTravelOtherRequirementDetails(Convert.ToInt32(decryptedTravelId), page, rows, out totalCount);

                if ((objTravelOtherReqdetails.otheradminViewmodelList == null || objTravelOtherReqdetails.otheradminViewmodelList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    objTravelOtherReqdetails.otheradminViewmodelList = dal.GetTravelOtherRequirementDetails(Convert.ToInt32(decryptedTravelId), page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = objTravelOtherReqdetails.otheradminViewmodelList,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveTravelOtherRequiementDetailsInfo(HttpPostedFileBase doc, OtherAdminViewModel model)
        {
            DAL.TravelDAL dal = new DAL.TravelDAL();
            string decryptedTravelId = Session["Travelid"].ToString();
            int travelid = Convert.ToInt32(decryptedTravelId);
            string resultMessage = string.Empty;
            var status = false;
            try
            {
                if (model.isFormValid == true)
                {
                    if (doc != null)
                    {
                        if (doc.ContentLength > 0)
                        {
                            string uploadsPath = (UploadFileLocation);
                            string uploadpathwithId = Path.Combine(uploadsPath, (travelid).ToString());

                            uploadsPath = Path.Combine(uploadpathwithId, Convert.ToString(model.ID));
                            string fileName = Path.GetFileName(doc.FileName);
                            model.FileName = fileName;
                            model.FilePath = uploadsPath;
                            TravelDAL DAL = new TravelDAL();
                            string filePath = Path.Combine(uploadsPath, fileName);
                            if (!Directory.Exists(uploadsPath))
                                Directory.CreateDirectory(uploadsPath);

                            doc.SaveAs(filePath);
                        }
                    }

                    status = dal.SaveTravelOtherRequirementDetails(model, travelid);
                }
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status, isFormValid = model.isFormValid }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteTravelOtherRequiementDetails(string TravelId, string IDs)
        {
            try
            {
                TravelDAL dal = new TravelDAL();
                bool eq = dal.DeleteTravelOtherRequirementDetailsInfo(Convert.ToInt32(TravelId), Convert.ToInt32(IDs));
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpGet]
        //public ActionResult GetClientFormDetails()
        //{
        //    try
        //    {
        private ClientViewModel ClientDetails()
        {
            ClientViewModel model = new ClientViewModel();
            PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
            EmployeeDAL employeeDAL = new EmployeeDAL();
            model.SearchedUserDetails = new SearchedUserDetails();
            model.SearchedUserDetails.EmployeeId = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
            string decryptedTravelId = Session["Travelid"].ToString();
            int travelid = Convert.ToInt32(decryptedTravelId);
            Tbl_HR_Travel travelDetails = travelDAL.GetTravelDetailsfromTravelID(travelid);
            TravelDAL dal = new TravelDAL();
            string employeeCode = Membership.GetUser().UserName;
            int employeeId = employeeDAL.GetEmployeeID(employeeCode);
            bool loggedinUser = dal.CheckForTheLogged(employeeId, decryptedTravelId);
            ViewBag.IsLoggedInUser = loggedinUser;
            if (travelDetails != null)
            {
                model.AdditionalInfo = travelDetails.ClientAdditionalInformation;
            }
            if (Session["Stageid"] != null)
            {
                ViewBag.StageID = Session["Stageid"].ToString();
            }
            model.TravellingLocationList = new List<TravellingLocationList>();
            //model.TravellingLocationList = dal.GetTravellingLocation();
            ViewBag.clientList = dal.GetTravellingLocation();
            ViewBag.ProjectNameList = dal.GetProjectList();
            return model;
            //}
            //catch (Exception)
            //{
            //    return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            //}
        }

        //[HttpGet]
        //public ActionResult GetAccomodationFormDetails(string TravelEmployeeId, string encryptedTravelId)
        //{
        //    try
        //    {
        private AccomodationViewModel AccomodationDetails(string TravelEmployeeId, string encryptedTravelId)
        {
            AccomodationViewModel model = new AccomodationViewModel();
            EmployeeDAL employeeDAL = new EmployeeDAL();
            TravelDAL dal = new TravelDAL();
            string employeeCode = Membership.GetUser().UserName;
            int employeeId = employeeDAL.GetEmployeeID(employeeCode);
            model.SearchedUserDetails = new SearchedUserDetails();
            model.SearchedUserDetails.EmployeeId = employeeId;
            ViewBag.loginUserId = employeeId;
            string decryptedTravelId = Session["Travelid"].ToString();
            model.ClientTravelYesNoList = dal.clientTravelYesNoList();
            Tbl_HR_Travel stageid = dal.GetTravelDetailsfromTravelID(Convert.ToInt32(decryptedTravelId));

            int? TravelAppRoverId = stageid.ProjectManagerId;
            int? GroupHeadId = stageid.GroupHeadId;
            int? TravelAdminId = stageid.AdminApproverId;

            if (stageid.TRFNo.Contains("."))
            {
                ViewBag.Extension = "Ext";
            }

            if (Session["Stageid"] != null)
            {
                ViewBag.StageID = stageid.StageID.ToString();
            }
            if (Session["Btnsubmitstatus"] != null)
            {
                string status = Session["Btnsubmitstatus"].ToString();
                ViewBag.submitBtnstatus = status;
            }
            //To check if the user has logged in himself
            bool loggedinUser = dal.CheckForTheLogged(employeeId, decryptedTravelId);
            ViewBag.IsLoggedInUser = loggedinUser;

            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            if (stageid.StageID == 0)
            {
                if (User.IsInRole("Travel_Admin"))
                {
                    ViewBag.UserRole = "Travel_Admin";
                    model.SearchedUserDetails.UserRole = "Travel_Admin";
                }
                else if (User.IsInRole("Travel Approver"))
                {
                    ViewBag.UserRole = "TravelApprover";
                    model.SearchedUserDetails.UserRole = "TravelApprover";
                }
                else if (User.IsInRole(UserRoles.GroupHead))
                {
                    ViewBag.UserRole = "Group Head";
                    model.SearchedUserDetails.UserRole = "GroupHead";
                }
                else if (User.IsInRole("Delivery Manager"))
                {
                    ViewBag.UserRole = "Delivery Manager";
                    model.SearchedUserDetails.UserRole = "Delivery Manager";
                }
                else if (User.IsInRole("Account Owners"))
                {
                    ViewBag.UserRole = "Account Owners";
                    model.SearchedUserDetails.UserRole = "Account Owners";
                }
                else if (User.IsInRole("Management"))
                {
                    ViewBag.UserRole = "Management";
                    model.SearchedUserDetails.UserRole = "Management";
                }
            }
            else
            {
                if (User.IsInRole("Travel_Admin") && stageid.StageID == 3)
                {
                    ViewBag.UserRole = "Travel_Admin";
                    model.SearchedUserDetails.UserRole = "Travel_Admin";
                }
                else if (User.IsInRole("Travel Approver") && TravelAppRoverId == employeeId && stageid.StageID == 1)
                {
                    ViewBag.UserRole = "TravelApprover";
                    model.SearchedUserDetails.UserRole = "TravelApprover";
                }
                else if (User.IsInRole("Group Head") && GroupHeadId == employeeId && stageid.StageID == 2)
                {
                    ViewBag.UserRole = "Group Head";
                    model.SearchedUserDetails.UserRole = "GroupHead";
                }
                else if (User.IsInRole("Travel_Admin") && stageid.StageID == 4)
                {
                    ViewBag.UserRole = "Travel_Admin";
                    model.SearchedUserDetails.UserRole = "Travel_Admin";
                }
                else if (User.IsInRole("Travel Approver") && TravelAppRoverId == employeeId && stageid.StageID == 2)
                {
                    ViewBag.UserRole = "TravelApprover";
                    model.SearchedUserDetails.UserRole = "TravelApprover";
                }
                else if (User.IsInRole("Group Head") && GroupHeadId == employeeId && stageid.StageID == 3)
                {
                    ViewBag.UserRole = "Group Head";
                    model.SearchedUserDetails.UserRole = "Group Head";
                }
                else if (User.IsInRole("Delivery Manager"))
                {
                    ViewBag.UserRole = "Delivery Manager";
                    model.SearchedUserDetails.UserRole = "Delivery Manager";
                }
                else if (User.IsInRole("Account Owners"))
                {
                    ViewBag.UserRole = "Account Owners";
                    model.SearchedUserDetails.UserRole = "Account Owners";
                }
                else if (User.IsInRole("Management"))
                {
                    ViewBag.UserRole = "Management";
                    model.SearchedUserDetails.UserRole = "Management";
                }
            }

            Tbl_HR_Travel TravelDetails = dal.GetTravelDetails(Convert.ToInt32(decryptedTravelId));
            int SumitedUserEmpId = Convert.ToInt32(TravelDetails.EmployeeId);
            ViewBag.SumitedUserEmpId = SumitedUserEmpId;

            string UserRolesOrganSubmited = dal.GetEmployeeCode(SumitedUserEmpId);
            string[] usersInRoles = Roles.GetRolesForUser(UserRolesOrganSubmited);

            if (usersInRoles.Contains("Delivery Manager"))
            {
                ViewBag.SumitedUserEmpRole = "Delivery Manager";
            }
            if (usersInRoles.Contains("Account Owners"))
            {
                ViewBag.SumitedUserEmpRole = "Account Owners";
            }
            if (usersInRoles.Contains("Group Head"))
            {
                ViewBag.SumitedUserEmpRole = "Group Head";
            }
            if (usersInRoles.Contains("Management"))
            {
                ViewBag.SumitedUserEmpRole = "Management";
            }
            int TravelRequirementsID = 0;
            Tbl_HR_Travel TravelFormDetails = dal.GetTravelDetails(Convert.ToInt32(decryptedTravelId));
            tbl_HR_Travel_EmployeeTravelRequirement AccomodationFormDetails = dal.GetAccomodationDetails(Convert.ToInt32(decryptedTravelId));
            if (AccomodationFormDetails != null)
            {
                model.TravelId = Convert.ToInt32(decryptedTravelId);
                model.TravelEmployeeId = Convert.ToInt32(TravelFormDetails.EmployeeId);
                TravelRequirementsID = AccomodationFormDetails.TravelRequirementsID;
                model.TravelRequirementsID = AccomodationFormDetails.TravelRequirementsID;
                model.AccomodationNeeded = Convert.ToInt32(AccomodationFormDetails.AccomodationMasterID);
                //model.ShuttelNeeded = Convert.ToInt32(AccomodationFormDetails.ShuttleMasterID);
                model.HardDriveNeeded = Convert.ToInt32(AccomodationFormDetails.HardDriveMasterID);
                model.LaptopNeeded = Convert.ToInt32(AccomodationFormDetails.LaptopMasterID);
                model.USBDriveNeeded = Convert.ToInt32(AccomodationFormDetails.UsbDriveMasterID);
                model.AirportPickUpNeeded = Convert.ToInt32(AccomodationFormDetails.AirportPickupMasterID);
                model.StageID = TravelFormDetails.StageID;
                model.SoftwaresNeeded = AccomodationFormDetails.EmployeeSoftwaresRequirement;
                model.AdditionalInformation = AccomodationFormDetails.RequirementAdditionalInformation;
            }
            return model;
            //}
            //catch (Exception)
            //{
            //    return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            //}
        }

        [HttpPost]
        public ActionResult SaveAccomodationFormDetails(AccomodationViewModel model)
        {
            TravelDAL dal = new TravelDAL();
            try
            {
                string resultMessage = string.Empty;
                string decryptedTravelId = Session["Travelid"].ToString();
                var status = dal.SaveAccomodationFormDetails(model, decryptedTravelId);
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                if (User.IsInRole("Travel Approver"))
                {
                    ViewBag.UserRole = "TravelApprover";
                }
                if (User.IsInRole("Group Head"))
                {
                    ViewBag.UserRole = "GroupHead";
                }
                if (User.IsInRole("Travel_Admin"))
                {
                    ViewBag.UserRole = "Travel_Admin";
                }

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
        public ActionResult SubmitTravelApprovalForm(AccomodationViewModel model)
        {
            TravelDAL dal = new TravelDAL();
            try
            {
                string resultMessage = string.Empty;
                var status = false;
                int loginEmpId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                ViewBag.loginEmpId = loginEmpId;

                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);
                string userrole = string.Empty;
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                Tbl_HR_Travel TravelDetails = dal.GetTravelDetails(travelid);
                if (User.IsInRole("Travel Approver") && loginEmpId == TravelDetails.ProjectManagerId)
                {
                    userrole = "TravelApprover";
                }
                //if (User.IsInRole("Group Head") && loginEmpId == TravelDetails.GroupHeadId)
                //{
                //    userrole = "GroupHead";
                //}

                int SumitedUserEmpId = Convert.ToInt32(TravelDetails.EmployeeId);
                ViewBag.SumitedUserEmpId = SumitedUserEmpId;

                string UserRolesOrganSubmited = dal.GetEmployeeCode(SumitedUserEmpId);
                string[] usersInRoles = Roles.GetRolesForUser(UserRolesOrganSubmited);

                if (usersInRoles.Contains("Delivery Manager"))
                {
                    ViewBag.SumitedUserEmpRole = "Delivery Manager";
                }
                if (usersInRoles.Contains("Account Owners"))
                {
                    ViewBag.SumitedUserEmpRole = "Account Owners";
                }
                if (usersInRoles.Contains("Group Head"))
                {
                    ViewBag.SumitedUserEmpRole = "Group Head";
                }
                if (usersInRoles.Contains("Management"))
                {
                    ViewBag.SumitedUserEmpRole = "Management";
                }
                string UserRolesOrgan = dal.GetEmployeeCode(loginEmpId);
                string[] usersInRolesorg = Roles.GetRolesForUser(UserRolesOrgan);

                if (usersInRolesorg.Contains("Delivery Manager"))
                {
                    ViewBag.UserRolesOrgan = "Delivery Manager";
                }
                if (usersInRolesorg.Contains("Account Owners"))
                {
                    ViewBag.UserRolesOrgan = "Account Owners";
                }
                if (usersInRolesorg.Contains("Group Head"))
                {
                    ViewBag.UserRolesOrgan = "Group Head";
                }
                if (usersInRolesorg.Contains("Management"))
                {
                    ViewBag.UserRolesOrgan = "Management";
                }

                if (SumitedUserEmpId != loginEmpId && (ViewBag.SumitedUserEmpRole == "Delivery Manager" || ViewBag.SumitedUserEmpRole == "Account Owners" || ViewBag.SumitedUserEmpRole == "Group Head" || ViewBag.SumitedUserEmpRole == "Management"))
                {
                    status = dal.SubmitTravelApprovalFormOrganizationHeads(model, loginEmpId, travelid, userrole, null, ViewBag.SumitedUserEmpRole);
                }
                else if (SumitedUserEmpId == loginEmpId && (ViewBag.UserRolesOrgan == "Delivery Manager" || ViewBag.UserRolesOrgan == "Account Owners" || ViewBag.UserRolesOrgan == "Group Head" || ViewBag.UserRolesOrgan == "Management"))
                {
                    status = dal.SubmitTravelApprovalFormOrganizationHeads(model, loginEmpId, travelid, userrole, ViewBag.UserRolesOrgan, null);
                }
                else
                {
                    status = dal.SubmitTravelApprovalForm(model, loginEmpId, travelid, userrole);
                }
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                int TravelId = dal.getTravelID(model.TravelEmployeeId);
                string travelID = Commondal.Encrypt(Session["SecurityKey"].ToString() + TravelId, true);

                return Json(new { results = resultMessage, status = status, travelID = travelID, isApproved = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpGet]
        //public ActionResult GetAccomodationAdminFormDetails()
        //{
        //    try
        //    {
        private AccomodationAdminViewModel AccomodationAdminDetails()
        {
            AccomodationAdminViewModel model = new AccomodationAdminViewModel();
            PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
            EmployeeDAL employeeDAL = new EmployeeDAL();
            model.SearchedUserDetails = new SearchedUserDetails();
            model.SearchedUserDetails.EmployeeId = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
            string decryptedTravelId = Session["Travelid"].ToString();
            int travelid = Convert.ToInt32(decryptedTravelId);
            tbl_HR_TravelAccomodationDetails travelAccoDetails = travelDAL.GetAccomodationAdminDetails(travelid);
            Tbl_HR_Travel travelDetails = travelDAL.GetTravelDetailsfromTravelID(travelid);
            int? TravelAppRoverId = travelDetails.ProjectManagerId;
            int? GroupHeadId = travelDetails.GroupHeadId;
            int? TravelAdminId = travelDetails.AdminApproverId;
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            if (travelDetails.StageID == 0)
            {
                if (role.Contains(UserRoles.TravelAdmin))
                {
                    ViewBag.UserRole = UserRoles.TravelAdmin;
                    model.SearchedUserDetails.UserRole = UserRoles.TravelAdmin;
                }
                else if (role.Contains(UserRoles.TravelApprover))
                {
                    ViewBag.UserRole = UserRoles.TravelApprover;
                    model.SearchedUserDetails.UserRole = UserRoles.TravelApprover;
                }
                else if (role.Contains(UserRoles.GroupHead))
                {
                    ViewBag.UserRole = UserRoles.GroupHead;
                    model.SearchedUserDetails.UserRole = UserRoles.GroupHead;
                }
            }
            else
            {
                if (role.Contains(UserRoles.TravelAdmin) && travelDetails.StageID == 3)
                {
                    ViewBag.UserRole = UserRoles.TravelAdmin;
                    model.SearchedUserDetails.UserRole = UserRoles.TravelAdmin;
                }
                else if (role.Contains(UserRoles.TravelApprover) && TravelAppRoverId == model.SearchedUserDetails.EmployeeId && travelDetails.StageID == 1)
                {
                    ViewBag.UserRole = UserRoles.TravelApprover;
                    model.SearchedUserDetails.UserRole = UserRoles.TravelApprover;
                }
                else if (role.Contains(UserRoles.GroupHead) && GroupHeadId == model.SearchedUserDetails.EmployeeId && travelDetails.StageID == 2)
                {
                    ViewBag.UserRole = UserRoles.GroupHead;
                    model.SearchedUserDetails.UserRole = UserRoles.GroupHead;
                }
                else if (role.Contains(UserRoles.TravelAdmin) && travelDetails.StageID == 4)
                {
                    ViewBag.UserRole = UserRoles.TravelAdmin;
                    model.SearchedUserDetails.UserRole = UserRoles.TravelAdmin;
                }
                else if (role.Contains(UserRoles.TravelApprover) && TravelAppRoverId == model.SearchedUserDetails.EmployeeId && travelDetails.StageID == 2)
                {
                    ViewBag.UserRole = UserRoles.TravelApprover;
                    model.SearchedUserDetails.UserRole = UserRoles.TravelApprover;
                }
                else if (role.Contains(UserRoles.GroupHead) && GroupHeadId == model.SearchedUserDetails.EmployeeId && travelDetails.StageID == 3)
                {
                    ViewBag.UserRole = UserRoles.GroupHead;
                    model.SearchedUserDetails.UserRole = UserRoles.GroupHead;
                }
            }

            //ViewBag.AdminApproverId = travelDetails.AdminApproverId;
            model.newAccomodationAdmin = new AccomodationAdmin();
            model.newAccomodationAdmin.LoggedInEmployeeId = model.SearchedUserDetails.EmployeeId;
            model.newAccomodationAdmin.TravelId = travelid;
            model.newAccomodationAdmin.AdditionalInformation = travelDAL.GetAccoDetailsAdditionalInformation(travelid);
            if (Session["Stageid"] != null)
            {
                ViewBag.StageID = Session["Stageid"].ToString();
            }
            TravelDAL dal = new TravelDAL();
            bool loggedinUser = dal.CheckForTheLogged(Convert.ToInt32(model.newAccomodationAdmin.LoggedInEmployeeId), decryptedTravelId);
            ViewBag.IsLoggedInUser = loggedinUser;
            return model;
            //}
            //catch (Exception)
            //{
            //    return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            //}
        }

        [HttpPost]
        public ActionResult LoadAccomodationAdminDetails(int page, int rows, int TravelID)
        {
            try
            {
                AccomodationAdminViewModel model = new AccomodationAdminViewModel();
                int totalCount;
                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);

                model.AccomodationDetailsList = travelDAL.GetAccomodationAdminGrid(page, rows, travelid, out totalCount);

                if ((model.AccomodationDetailsList == null || model.AccomodationDetailsList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model.AccomodationDetailsList = travelDAL.GetAccomodationAdminGrid(page, rows, travelid, out totalCount);
                }

                var jsonData = new
                {
                    total = (int)Math.Ceiling((double)totalCount / (double)rows),
                    page = page,
                    records = totalCount,
                    rows = model.AccomodationDetailsList
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult LoadJourneyGridTravel(int page, int rows, string travelID, string TRFNo)
        {
            try
            {
                //JourneyViewModel model = new JourneyViewModel();
                List<JourneyList> model = new List<JourneyList>();
                //model.TravelID = Convert.ToInt32(travelID);
                int totalCount;

                model = travelDAL.GetJourneyList(page, rows, Convert.ToInt32(Session["Travelid"].ToString()), TRFNo, out totalCount);

                if ((model == null || model.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model = travelDAL.GetJourneyList(page, rows, Convert.ToInt16(travelID), TRFNo, out totalCount);
                }

                //List<JourneyModeList> journeyList = travelDAL.GetJourneyModeList();
                //model.JourneyModeList = journeyList;

                var jsonData = new
                {
                    total = (int)Math.Ceiling((double)totalCount / (double)rows),
                    page = page,
                    records = totalCount,
                    rows = model
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        //[HttpPost]
        //public ActionResult SaveAdminAccomodationDetails(AccomodationAdmin model)

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAdminAccomodationDetails(AccomodationAdmin model)
        {
            try
            {
                bool success = false;
                if (model != null)
                {
                    string decryptedTravelId = Session["Travelid"].ToString();
                    int travelid = Convert.ToInt32(decryptedTravelId);

                    //if (doc != null)
                    //{
                    //    if (doc.ContentLength > 0)
                    //    {
                    string destinationPath = (AccommodationFileLocation);
                    //string uploadpathwithId = Path.Combine(destinationPath, (travelid).ToString());
                    destinationPath = Path.Combine(destinationPath, (travelid).ToString());
                    //destinationPath = Path.Combine(uploadpathwithId, Convert.ToString(model.TravelId));
                    //string fileName = Path.GetFileName(doc.FileName);
                    tbl_HR_TravelAccomodationUploadDetails uploadDetails = travelDAL.GetAccomodationAdminUploadDetails(travelid, model.AccomodationID);
                    if (uploadDetails != null)
                    {
                        model.FileName = uploadDetails.FileName;
                        model.FilePath = destinationPath;
                        string sourceFilePath = Path.Combine(uploadDetails.FilePath, uploadDetails.FileName);
                        if (System.IO.File.Exists(sourceFilePath))
                        {
                            //System.IO.File.Copy(filePath, Server.MapPath("~/Uploads"));
                            //var file = Directory.GetFiles(uploadDetails.FilePath);
                            if (!Directory.Exists(destinationPath))
                                Directory.CreateDirectory(destinationPath);
                            System.IO.File.Copy(sourceFilePath, Path.Combine(destinationPath, uploadDetails.FileName), true);
                            System.IO.File.Delete(sourceFilePath);
                            bool isDeleted = travelDAL.DeleteAdminAccomodationUploadRecord(model.AccomodationID, travelid);
                        }
                        //string filePath = Path.Combine(uploadDetails.FilePath, uploadDetails.FileName);
                        //if (!Directory.Exists(uploadDetails.FilePath))
                        //    Directory.CreateDirectory(uploadDetails.FilePath);

                        //doc.SaveAs(filePath);
                    }
                    //TravelDAL DAL = new TravelDAL();

                    //    }
                    //}
                    success = travelDAL.AddAdminAccomodationDetails(model, travelid);
                    return Json(new { status = success }, "text/html", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = success }, "text/html", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult SaveAdminAccomodationUploadDetails(HttpPostedFileBase doc, AccomodationAdmin model)
        {
            try
            {
                bool success = false;
                if (model != null)
                {
                    string decryptedTravelId = Session["Travelid"].ToString();
                    int travelid = Convert.ToInt32(decryptedTravelId);
                    if (doc != null && doc.ContentLength > 0)
                    {
                        string uploadsPath = (System.Configuration.ConfigurationManager.AppSettings["UploadAccTempFileLocation"]);
                        //string uploadpathwithId = Path.Combine(uploadsPath, (travelid).ToString());
                        uploadsPath = Path.Combine(uploadsPath, (travelid).ToString());
                        //uploadsPath = Path.Combine(uploadpathwithId, Convert.ToString(model.TravelId));
                        string fileName = Path.GetFileName(doc.FileName);
                        model.FileName = fileName;
                        model.FilePath = uploadsPath;
                        string filePath = Path.Combine(uploadsPath, fileName);
                        if (!Directory.Exists(uploadsPath))
                            Directory.CreateDirectory(uploadsPath);
                        doc.SaveAs(filePath);
                        success = travelDAL.AddAdminAccomodationUploadDetails(model);
                    }
                }
                return Json(new { status = success, FileName = model.FileName, AccomodationId = model.AccomodationID }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteAdminAccomodationDetails(int AccomodationID, int TravelID)
        {
            try
            {
                bool success = false;
                if (AccomodationID != 0)
                {
                    success = travelDAL.DeleteAdminAccomodationDetails(AccomodationID, TravelID);
                    return Json(new { status = success }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { status = success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteAdminAccomodationUploadDetails(int AccomodationID)
        {
            try
            {
                bool success = false;
                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);

                tbl_HR_TravelAccomodationUploadDetails uploadDetails = travelDAL.GetAccomodationAdminUploadDetails(travelid, AccomodationID);
                if (uploadDetails != null)
                {
                    string sourceFilePath = Path.Combine(uploadDetails.FilePath, uploadDetails.FileName);
                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        System.IO.File.Delete(sourceFilePath);
                        success = travelDAL.DeleteAdminAccomodationUploadRecord(AccomodationID, travelid);
                    }
                }
                return Json(new { status = success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteJourneyUploadDetails(int JourneyId)
        {
            try
            {
                bool success = false;
                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);

                Tbl_HR_TravelJourneyUploadDetails uploadDetails = travelDAL.GetJourneyUploadDetails(travelid, JourneyId);
                if (uploadDetails != null)
                {
                    string sourceFilePath = Path.Combine(uploadDetails.FilePath, uploadDetails.FileName);
                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        System.IO.File.Delete(sourceFilePath);
                        success = travelDAL.DeleteJourneyUploadRecord(travelid, JourneyId);
                    }
                }
                return Json(new { status = success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteClientUploadDetails(int ClientId)
        {
            try
            {
                bool success = false;
                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);

                tbl_HR_Travel_ClientUploadInformation uploadDetails = travelDAL.GetClientUploadDetails(travelid, ClientId);
                if (uploadDetails != null)
                {
                    string sourceFilePath = Path.Combine(uploadDetails.FilePath, uploadDetails.FileName);
                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        System.IO.File.Delete(sourceFilePath);
                        success = travelDAL.DeleteClientUploadRecord(travelid, ClientId);
                    }
                }
                return Json(new { status = success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteVisaUploadDetails(int VisaId)
        {
            try
            {
                bool success = false;
                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);

                tbl_HR_VisaUploadDetailsTravel uploadDetails = travelDAL.GetVisaUploadDetails(travelid, VisaId);
                if (uploadDetails != null)
                {
                    string sourceFilePath = Path.Combine(uploadDetails.FilePath, uploadDetails.FileName);
                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        System.IO.File.Delete(sourceFilePath);
                        success = travelDAL.DeleteVisaUploadRecord(travelid, VisaId);
                    }
                }
                return Json(new { status = success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveAdminAccoAdditionalInfo(int TravelID, string AdditionalInformation)
        {
            try
            {
                bool success = false;
                if (TravelID != 0)
                {
                    string decryptedTravelId = Session["Travelid"].ToString();
                    int travelid = Convert.ToInt32(decryptedTravelId);
                    success = travelDAL.SaveAdminAccoAdditionalInfo(travelid, AdditionalInformation);
                    return Json(new { status = success }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { status = success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpGet]
        //public ActionResult GetConveyanceAdminFormDetails()
        //{
        //    try
        //    {
        private ConveyanceAdminViewModel ConveyanceDetails()
        {
            string decryptedTravelId = Session["Travelid"].ToString();
            int travelid = Convert.ToInt32(decryptedTravelId);

            DAL.TravelDAL dal = new DAL.TravelDAL();
            ConveyanceAdminViewModel model = new ConveyanceAdminViewModel();
            model.SearchedUserDetails = new SearchedUserDetails();

            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            string user = Commondal.GetMaxRoleForUser(role);
            model.SearchedUserDetails.UserRole = user;
            if (Session["Stageid"] != null)
            {
                ViewBag.StageID = Session["Stageid"].ToString();
            }
            ViewBag.LoggedInTravelempId = empdal.GetEmployeeID(Membership.GetUser().UserName);
            bool loggedinUser = dal.CheckForTheLogged(Convert.ToInt32(ViewBag.LoggedInTravelempId), decryptedTravelId);
            ViewBag.IsLoggedInUser = loggedinUser;

            model.viewmodellist = new List<ConveyanceAdminViewModel>();
            List<tbl_HR_TravelLocalConveyanceDetails> VisaDetails = dal.GetTravelConyanacesDetails(travelid);
            Tbl_HR_Travel ConvayadditionalInfo = dbContext.Tbl_HR_Travel.Where(travel => travel.TravelId == travelid).FirstOrDefault();
            if (VisaDetails != null)
            {
                foreach (tbl_HR_TravelLocalConveyanceDetails eachTravelVisadetails in VisaDetails)
                {
                    model.viewmodellist.Add(new ConveyanceAdminViewModel()
                    {
                        LocalConveyanceID = Convert.ToInt32(eachTravelVisadetails.LocalConveyanceID),
                        TravelID = Convert.ToInt32(eachTravelVisadetails.TravelID),
                        City = Convert.ToString(eachTravelVisadetails.City),
                        ConveyanceType = dal.GetConvayancedType(eachTravelVisadetails.ConveyanceType),
                        FromDate = Convert.ToDateTime(eachTravelVisadetails.FromDate),
                        //ToDate = Convert.ToDateTime(eachTravelVisadetails.ToDate),
                        //InsuranceDetails = eachTravelVisadetails.InsuranceDetails,
                    });
                }
            }

            List<CityT> countryList = dal.GetCityDetails();
            model.CityList = countryList;
            model.AdditionalInformation = ConvayadditionalInfo.ConvaynaceAdditionalInfo;
            List<ConveyType> visatypeList = dal.GetConaveyanceTypeDetails();
            model.ConavaytypeList = visatypeList;
            return model;
            //}
            //catch (Exception)
            //{
            //    return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            //}
        }

        [HttpGet]
        public ActionResult GetTravelExtensionFormDetails()
        {
            try
            {
                TravelExtensionViewModel model = new TravelExtensionViewModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                if (Session["Stageid"] != null)
                {
                    ViewBag.StageID = Session["Stageid"].ToString();
                }
                return View("TravelExtensionDetailsForm", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult GetTravelStatus()
        {
            try
            {
                TravelStatus model = new TravelStatus();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.FieldChildList = new List<TravelFieldChildDetails>();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                ViewBag.EncryptedEmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                if (role.Contains("Travel_Admin"))
                {
                    ViewBag.IsTravelAdmin = "yes";
                }
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                model.SearchedUserDetails.UserRole = user;
                ViewBag.FieldChildListBG = new SelectList(GetFieldChildDetailsList("Business Group"), "Id", "Description");
                ViewBag.FieldChildListOU = new SelectList(GetFieldChildDetailsList("Organization Unit"), "Id", "Description");
                ViewBag.FieldChildListSN = new SelectList(GetFieldChildDetailsList("Stage Name"), "Id", "Description");
                return View("TravelStatus", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        //[HttpPost]
        //public ActionResult SaveClientDetails(ClientViewModel model)
        //{
        //    TravelDAL dal = new TravelDAL();
        //    try
        //    {
        //        string decryptedTravelId = Session["Travelid"].ToString();
        //        string resultMessage = string.Empty;

        //        var status = dal.SaveClientDetailForm(model, decryptedTravelId);
        //        if (status)
        //            resultMessage = "Saved";
        //        else
        //            resultMessage = "Error";

        //        return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public ActionResult SaveClientForm(ClientViewModel model)
        {
            TravelDAL dal = new TravelDAL();
            try
            {
                string resultMessage = string.Empty;
                string decryptedTravelId = Session["Travelid"].ToString();
                var status = dal.SaveClientForm(model, decryptedTravelId);
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
        public ActionResult ClientDetailLoadGrid(string encryptedEmployeeId, int travelId, int page, int rows)
        {
            try
            {
                string decryptedTravelId = Session["Travelid"].ToString();
                ClientViewModel model = new ClientViewModel();
                TravelDAL dal = new TravelDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<ClientViewModel> expenseDetails = new List<ClientViewModel>();
                expenseDetails = dal.TravelDetailRecord(Convert.ToInt32(decryptedTravelId), page, rows, out totalCount);
                if ((expenseDetails == null || expenseDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    expenseDetails = dal.TravelDetailRecord(Convert.ToInt32(decryptedTravelId), page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = expenseDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult DeleteClientDetails(int clientId)
        {
            try
            {
                TravelDAL dal = new TravelDAL();
                bool status = dal.DeletedClientDetails(clientId);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveTravelRequestForm(TravelViewModel model)
        {
            TravelDAL dal = new TravelDAL();
            try
            {
                string resultMessage = string.Empty;
                RetriveTravelID RetriveTravelID = new RetriveTravelID();
                bool statusExtension = false;
                string TravelID = null;
                String EmployeeID = null;
                string employeeCode = Membership.GetUser().UserName;
                EmployeeDAL employeeDAL = new EmployeeDAL();
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                if (model.IsNewForm == true)
                {
                    EmployeeDAL EmployeeDAL = new EmployeeDAL();
                    model.TravelEmployeeId = EmployeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                    RetriveTravelID = dal.SaveRquestDeatilsForm(model, employeeId);
                    TravelID = Convert.ToString(RetriveTravelID.TravelID);
                    EmployeeID = Convert.ToString(RetriveTravelID.EmployeeID);
                    Session["Travelid"] = TravelID;
                }
                else
                {
                    String TrfNo = Convert.ToString(model.TravelTRFNo);
                    if (TrfNo.Contains("."))
                    {
                        statusExtension = dal.SaveExtFormWithAllFormDeatils(model.TravelTRFNo, model.TravelId, model.TravelExtexsionStartDate, model.TravelExtensionEndDate, model.TravelExtDetails, model);
                        Tbl_HR_Travel newExtDeatils = dal.GetTravelidExtensionform(model.TravelTRFNo);
                        TravelID = Convert.ToString(newExtDeatils.TravelId);
                        EmployeeID = Convert.ToString(newExtDeatils.EmployeeId);
                        ViewBag.EncryptedTravelId = newExtDeatils.TravelId;
                        ViewBag.EncryptedTravelEmployeeID = EmployeeID;
                        Session["Travelid"] = TravelID;
                        model.StageID = newExtDeatils.StageID;
                        ViewBag.StageID = newExtDeatils.StageID;
                        Session["Stageid"] = newExtDeatils.StageID;
                    }
                    else
                    {
                        EmployeeDAL EmployeeDAL = new EmployeeDAL();
                        model.TravelEmployeeId = EmployeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                        RetriveTravelID = dal.SaveRquestDeatilsForm(model, employeeId);
                        TravelID = Convert.ToString(model.TravelId);
                        EmployeeID = Convert.ToString(RetriveTravelID.EmployeeID);
                        Session["Travelid"] = TravelID;
                    }
                }
                if ((RetriveTravelID.TravelID != 0 && RetriveTravelID.IsAdded == true) || statusExtension == true)
                {
                    TravelID = Commondal.Encrypt(Convert.ToString(Session["SecurityKey"].ToString() + TravelID), true);
                    EmployeeID = Commondal.Encrypt(Convert.ToString(Session["SecurityKey"].ToString() + EmployeeID), true);
                    return Json(new { status = true, TravelID = TravelID, EmployeeID = EmployeeID }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { status = true, TravelID = TravelID, EmployeeID = EmployeeID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult TravelShowStatusDetails(string TravelId, int page, int rows)
        {
            try
            {
                TravelDAL dal = new TravelDAL();

                int totalCount;
                bool isAuthorize = true;
                string decryptedExpenseID = HRMSHelper.Decrypt(TravelId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                List<TravelShowStatus> ShowStatusResult = new List<TravelShowStatus>();
                ShowStatusResult = dal.GetShowStatusResult(page, rows, Convert.ToInt32(decryptedExpenseID), out totalCount);
                totalCount = ShowStatusResult.Count();
                if ((ShowStatusResult == null || ShowStatusResult.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    ShowStatusResult = dal.GetShowStatusResult(page, rows, Convert.ToInt32(decryptedExpenseID), out totalCount);
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
        public ActionResult SaveComments(int travelId, string comments, string commenttype)
        {
            TravelDAL dal = new TravelDAL();
            try
            {
                string resultMessage = string.Empty;

                var status = dal.SaveCommentDetails(travelId, comments, commenttype);

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
        public ActionResult RejectTravelDetails(AccomodationViewModel model)
        {
            TravelDAL dal = new TravelDAL();
            try
            {
                int travelId = model.TravelId;
                string travelID = Commondal.Encrypt(Session["SecurityKey"].ToString() + travelId, true);
                string resultMessage = string.Empty;
                int loginEmpId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                var status = dal.RejectTravelApprovalForm(model, loginEmpId);

                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status, travelID = travelID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteAllTravelDetailsByTravelId(int travelId, string TravelEmployeeId, string comments)
        {
            try
            {
                TravelDAL dal = new TravelDAL();
                string Travelid = Session["Travelid"].ToString();
                TravelStatus _travelDetails = dal.getTravelDetails(Convert.ToInt32(Travelid));
                int? stageID = _travelDetails.StageId;
                int? ProjectManagerApprover = _travelDetails.ProjectManagerApprover;
                int? GroupHeadApprover = _travelDetails.GroupHeadApprover;
                int? AdminApprover = _travelDetails.AdminApprover;
                bool status = dal.DeletedAllTravelDetails(Convert.ToInt32(Travelid), TravelEmployeeId);
                string travelID = Commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToInt32(Travelid), true);
                return Json(new { status = status, travelID = travelID, employeeID = TravelEmployeeId, stageID = stageID, ProjectManagerApprover = ProjectManagerApprover, GroupHeadApprover = GroupHeadApprover, AdminApprover = AdminApprover, comments = comments }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult LoadInboxListGrid(string term, string Field, string FieldChild, int page, int rows, string employeeId)
        {
            List<TravelStatus> FinalInboxListDetails = new List<TravelStatus>();
            try
            {
                int totalCount;

                string decryptedEmployeeId = Convert.ToString(employeeId);
                TravelDAL traveldal = new TravelDAL();
                CommonMethodsDAL commondal = new CommonMethodsDAL();

                List<TravelStatus> InboxListDetails = traveldal.GetInboxListTravelDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);

                if ((InboxListDetails == null || InboxListDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    InboxListDetails = traveldal.GetInboxListTravelDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                }

                foreach (var item in InboxListDetails)
                {
                    string ExitId = commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.TravelId), true);
                    item.EncryptedTravelId = ExitId;
                    string encryptedEmployeeId = commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.EmployeeId), true);
                    item.EncryptedEmployeeId = encryptedEmployeeId;
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
        public ActionResult LoadWatchListGrid(string term, string Field, string FieldChild, int page, int rows, string employeeId)
        {
            List<TravelStatus> FinalWatchListDetails = new List<TravelStatus>();
            try
            {
                int totalCount;

                CommonMethodsDAL commondal = new CommonMethodsDAL();
                TravelDAL traveldal = new TravelDAL();
                string decryptedEmployeeId = employeeId;
                List<TravelStatus> WatchListDetails = traveldal.GetWatchListTravelDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                if ((WatchListDetails == null || WatchListDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    WatchListDetails = traveldal.GetWatchListTravelDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                }
                foreach (var item in WatchListDetails)
                {
                    string TravelId = commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.TravelId), true);
                    item.EncryptedTravelId = TravelId;
                    string encryptedEmployeeId = commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.EmployeeId), true);
                    item.EncryptedEmployeeId = encryptedEmployeeId;
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

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult UploadJouneyDetails(JourneyList model, int TravelId, string TRFNO, string JourneyModeIds)
        {
            try
            {
                string decryptedTravelId = Session["Travelid"].ToString();
                bool uploadStatus = false;
                string resultMessage = string.Empty;
                bool status = false;

                //if (doc != null && doc.ContentLength > 0)
                //{
                string destinationPath = (UploadFileLocation);
                //string uploadpathwithId = Path.Combine(uploadsPath, model.EmployeeID.ToString());
                destinationPath = Path.Combine(destinationPath, Convert.ToString(TravelId));
                Tbl_HR_TravelJourneyUploadDetails uploadDetails = travelDAL.GetJourneyUploadDetails(TravelId, model.JourneyID);
                if (uploadDetails != null)
                {
                    model.TicketName = uploadDetails.FileName;
                    model.JourneyFilePath = destinationPath;
                    string sourceFilePath = Path.Combine(uploadDetails.FilePath, uploadDetails.FileName);
                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        if (!Directory.Exists(destinationPath))
                            Directory.CreateDirectory(destinationPath);
                        System.IO.File.Copy(sourceFilePath, Path.Combine(destinationPath, uploadDetails.FileName), true);
                        System.IO.File.Delete(sourceFilePath);
                        bool isDeleted = travelDAL.DeleteJourneyUploadRecord(TravelId, model.JourneyID);
                    }
                }
                //string fileName = Path.GetFileName(doc.FileName);
                //model.TicketName = fileName;
                //model.JourneyFilePath = uploadsPath;
                //string filePath = Path.Combine(uploadsPath, fileName);
                //if (!Directory.Exists(uploadsPath))
                //    Directory.CreateDirectory(uploadsPath);
                //doc.SaveAs(filePath);
                //}
                TravelDAL DAL = new TravelDAL();
                status = DAL.SaveJourneyDetails(model, TravelId, TRFNO, JourneyModeIds);

                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadJouneyUploadDetails(HttpPostedFileBase doc, JourneyViewModel model)
        {
            try
            {
                bool success = false;
                if (model != null)
                {
                    string decryptedTravelId = Session["Travelid"].ToString();
                    int TravelId = Convert.ToInt32(decryptedTravelId);
                    if (doc != null && doc.ContentLength > 0)
                    {
                        string uploadsPath = (System.Configuration.ConfigurationManager.AppSettings["UploadPassportTempFileLocation"]);
                        //string uploadpathwithId = Path.Combine(uploadsPath, (travelid).ToString());
                        //uploadsPath = Path.Combine(uploadsPath, (travelid).ToString());
                        //uploadsPath = Path.Combine(uploadpathwithId, Convert.ToString(model.TravelId));
                        //string fileName = Path.GetFileName(doc.FileName);
                        //model.FileName = fileName;
                        //model.FilePath = uploadsPath;
                        //string filePath = Path.Combine(uploadsPath, fileName);
                        //if (!Directory.Exists(uploadsPath))
                        //    Directory.CreateDirectory(uploadsPath);
                        //doc.SaveAs(filePath);
                        //string uploadsPath = HttpContext.Server.MapPath(UploadFileLocation);
                        //////////
                        uploadsPath = Path.Combine(uploadsPath, Convert.ToString(decryptedTravelId));
                        string fileName = Path.GetFileName(doc.FileName);
                        model.JourneyDetail.TicketName = fileName;
                        model.JourneyDetail.JourneyFilePath = uploadsPath;
                        string filePath = Path.Combine(uploadsPath, fileName);
                        if (!Directory.Exists(uploadsPath))
                            Directory.CreateDirectory(uploadsPath);
                        doc.SaveAs(filePath);
                        success = travelDAL.SaveJourneyUploadDetails(model, TravelId);
                    }
                }
                return Json(new { status = success, FileName = model.JourneyDetail.TicketName, JourneyId = model.JourneyDetail.JourneyID }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult UploadClientDetails(ClientViewModel model, int? LocationId, int? SelectedClientId)
        {
            try
            {
                string decryptedTravelId = Session["Travelid"].ToString();
                int TravelId = Convert.ToInt32(decryptedTravelId);
                string resultMessage = string.Empty;
                bool status = false;
                int clientLocationId = Convert.ToInt32(LocationId);
                int selClientId = Convert.ToInt32(SelectedClientId);

                //if (doc != null && model.TravellingLocId == 4)
                //{
                string destinationPath = (UploadClientInviteLetterFileLocation);
                //string uploadpathwithId = Path.Combine(uploadsPath, decryptedTravelId.ToString());
                destinationPath = Path.Combine(destinationPath, Convert.ToString(TravelId));
                //string fileName = Path.GetFileName(doc.FileName);
                tbl_HR_Travel_ClientUploadInformation uploadDetails = travelDAL.GetClientUploadDetails(TravelId, model.ClientId);
                if (uploadDetails != null)
                {
                    model.ClientInviteLetterName = uploadDetails.FileName;
                    model.ClientIviteLetterFilePath = destinationPath;
                    string sourceFilePath = Path.Combine(uploadDetails.FilePath, uploadDetails.FileName);
                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        if (!Directory.Exists(destinationPath))
                            Directory.CreateDirectory(destinationPath);
                        System.IO.File.Copy(sourceFilePath, Path.Combine(destinationPath, uploadDetails.FileName), true);
                        System.IO.File.Delete(sourceFilePath);
                        bool isDeleted = travelDAL.DeleteClientUploadRecord(TravelId, model.ClientId);
                    }
                }

                //string filePath = Path.Combine(uploadsPath, fileName);
                //if (!Directory.Exists(uploadsPath))
                //    Directory.CreateDirectory(uploadsPath);
                //doc.SaveAs(filePath);
                //}

                TravelDAL DAL = new TravelDAL();
                status = DAL.SaveClientDetailForm(model, decryptedTravelId, clientLocationId, selClientId);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadClientFileDetails(HttpPostedFileBase doc, ClientViewModel model)
        {
            try
            {
                bool success = false;
                if (model != null)
                {
                    string decryptedTravelId = Session["Travelid"].ToString();
                    int TravelId = Convert.ToInt32(decryptedTravelId);
                    if (doc != null && doc.ContentLength > 0)
                    {
                        string uploadsPath = (System.Configuration.ConfigurationManager.AppSettings["UploadClientInviteLetterTempLocation"]);

                        uploadsPath = Path.Combine(uploadsPath, Convert.ToString(decryptedTravelId));
                        string fileName = Path.GetFileName(doc.FileName);
                        model.ClientInviteLetterName = fileName;
                        model.ClientIviteLetterFilePath = uploadsPath;
                        string filePath = Path.Combine(uploadsPath, fileName);
                        if (!Directory.Exists(uploadsPath))
                            Directory.CreateDirectory(uploadsPath);
                        doc.SaveAs(filePath);
                        success = travelDAL.SaveClientUploadDetails(model, TravelId);
                    }
                }
                return Json(new { status = success, FileName = model.ClientInviteLetterName, ClientId = model.ClientId }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpGet]
        //public ActionResult GetJourneyFormDetails(string encryptedTravelId)
        //{
        //    try
        //    {
        private JourneyViewModel JourneyDetails(string encryptedTravelId)
        {
            string decryptedTravelID = Session["Travelid"].ToString();
            JourneyViewModel model = new JourneyViewModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            model.JourneyDetailsList = new List<JourneyList>();
            model.JourneyDetail = new JourneyList();
            model.JourneyModeList = travelDAL.GetJourneyModeList();

            model.JourneyDetail.TravelID = Convert.ToInt32(decryptedTravelID);
            TravelDAL dal = new TravelDAL();
            string employeeCode = Membership.GetUser().UserName;
            EmployeeDAL employeeDAL = new EmployeeDAL();
            int employeeId = employeeDAL.GetEmployeeID(employeeCode);
            bool loggedinUser = dal.CheckForTheLogged(employeeId, decryptedTravelID);
            ViewBag.IsLoggedInUser = loggedinUser;
            if (Session["Stageid"] != null)
            {
                ViewBag.StageID = Session["Stageid"].ToString();
                model.JourneyDetail.StageID = Convert.ToInt32(Session["Stageid"].ToString());
            }
            ViewBag.TravelID = Session["Travelid"].ToString();
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);

            if (User.IsInRole("Travel Approver"))
            {
                ViewBag.UserRole = "TravelApprover";
                model.SearchedUserDetails.UserRole = "TravelApprover";
            }
            if (User.IsInRole("Group Head"))
            {
                ViewBag.UserRole = "GroupHead";
                model.SearchedUserDetails.UserRole = "GroupHead";
            }
            if (User.IsInRole("Travel_Admin"))
            {
                ViewBag.UserRole = "Travel_Admin";
                model.SearchedUserDetails.UserRole = "Travel_Admin";
            }
            return model;
            //}
            //catch (Exception)
            //{
            //    return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            //}
        }

        [HttpPost]
        public ActionResult SaveConvayinfodetails(int TravelID, string addiconveyInfo)
        {
            try
            {
                bool success = false;
                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);

                if (travelid != 0)
                {
                    success = travelDAL.SaveConveyAdditionalInfo(travelid, addiconveyInfo);

                    return Json(new { status = success }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { status = success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteJourneyDetails(int journeyId)
        {
            try
            {
                TravelDAL dal = new TravelDAL();
                bool status = dal.DeleteJourneyDetails(journeyId);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public ActionResult SaveJourneyDetails(JourneyViewModel model)
        //{
        //    try
        //    {
        //        string result = string.Empty;
        //        TravelDAL dal = new TravelDAL();
        //        model.JourneyDetail.TravelID = Convert.ToInt32(Session["Travelid"].ToString());

        //        var success = dal.SaveJourneyDetails(model);
        //        if (success)
        //            result = HRMS.Resources.Success.ResourceManager.GetString("SaveJourneyDetailsSuccess");
        //        else
        //            result = HRMS.Resources.Errors.ResourceManager.GetString("SaveJourneyDetailsError");

        //        return Json(new { resultMesssage = result, status = success, travelId = model.JourneyDetail.TravelID }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch
        //    {
        //        return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        //    }

        //}

        [HttpGet]
        public ActionResult GetExtensionStatus()
        {
            try
            {
                TravelStatus model = new TravelStatus();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.FieldChildList = new List<TravelFieldChildDetails>();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                ViewBag.EncryptedEmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                model.SearchedUserDetails.UserRole = user;
                ViewBag.FieldChildListBG = new SelectList(GetFieldChildDetailsList("Business Group"), "Id", "Description");
                ViewBag.FieldChildListOU = new SelectList(GetFieldChildDetailsList("Organization Unit"), "Id", "Description");
                ViewBag.FieldChildListSN = new SelectList(GetFieldChildDetailsList("Stage Name"), "Id", "Description");
                return View("ExtensionStatus", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult LoadExtensionListGrid(string term, string Field, string FieldChild, int page, int rows, string employeeId)
        {
            List<TravelStatus> FinalInboxListDetails = new List<TravelStatus>();
            try
            {
                int totalCount;

                string decryptedEmployeeId = Convert.ToString(employeeId);
                TravelDAL traveldal = new TravelDAL();
                CommonMethodsDAL commondal = new CommonMethodsDAL();

                List<TravelStatus> InboxListDetails = traveldal.GetExtensionListTravelDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);

                if ((InboxListDetails == null || InboxListDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    InboxListDetails = traveldal.GetExtensionListTravelDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                }

                foreach (var item in InboxListDetails)
                {
                    string ExitId = commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.TravelId), true);
                    item.EncryptedTravelId = ExitId;
                    string encryptedEmployeeId = commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.EmployeeId), true);
                    item.EncryptedEmployeeId = encryptedEmployeeId;
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

        [HttpGet]
        public ActionResult showJourneyDetails(int TravelID, int DocumentID)
        {
            try
            {
                JourneyViewModel model = new JourneyViewModel();
                TravelDAL DAL = new TravelDAL();
                PassportViewModel employeeDetails = DAL.getEmployeeDetailsFromtravelID(TravelID);
                model.SearchedUserDetails = new SearchedUserDetails();

                return PartialView("_PassportDetails", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult DownloadJourneyFile(string DocumentID, int TravelID)
        {
            try
            {
                TravelDAL DAL = new TravelDAL();
                JourneyViewModel model = new JourneyViewModel();
                model.JourneyDetailsList = new List<JourneyList>();
                model.JourneyDetailsList = DAL.GetJourneyShowHistory(TravelID, Convert.ToInt32(DocumentID));
                string[] FileExtention = model.JourneyDetailsList[0].TicketName.Split('.');
                string contentType = "application/" + FileExtention[1];
                string Filepath = Path.Combine(model.JourneyDetailsList[0].JourneyFilePath, model.JourneyDetailsList[0].TicketName);
                return File(Filepath, contentType, model.JourneyDetailsList[0].TicketName);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult DownloadClientLetteFile(string DocumentID, int TravelID)
        {
            try
            {
                TravelDAL DAL = new TravelDAL();
                ClientViewModel model = new ClientViewModel();
                model.ClientDetailsList = new List<ClientViewModel>();
                model.ClientDetailsList = DAL.GetClientShowHistory(TravelID, Convert.ToInt32(DocumentID));
                string[] FileExtension = model.ClientDetailsList[0].ClientInviteLetterName.Split('.');
                string contentType = "application/" + FileExtension[1];
                string Filepath = Path.Combine(model.ClientDetailsList[0].ClientIviteLetterFilePath, model.ClientDetailsList[0].ClientInviteLetterName);
                if (!System.IO.File.Exists(Filepath))
                {
                    throw new Exception();
                }
                return File(Filepath, contentType, model.ClientDetailsList[0].ClientInviteLetterName);
            }
            catch (Exception)
            {
                ConfigurationViewModel model = new ConfigurationViewModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                string employeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(employeeCode);
                if (employeeCode != null)
                {
                    CommonMethodsDAL Commondal = new CommonMethodsDAL();
                    model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }
                return PartialView("_FileNotFound", model);
            }
        }

        [HttpPost]
        public ActionResult RejectTravelDetailsAdmin(AccomodationViewModel model)
        {
            TravelDAL dal = new TravelDAL();
            try
            {
                bool status;
                string travelID = Commondal.Encrypt(Session["SecurityKey"].ToString() + model.TravelId, true);
                string resultMessage = string.Empty;
                int loginEmpId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();

                string userrole = string.Empty;
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                if (User.IsInRole("Travel Approver"))
                {
                    userrole = "TravelApprover";
                }
                if (User.IsInRole("Group Head"))
                {
                    userrole = "GroupHead";
                }

                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);
                Tbl_HR_Travel TravelDetails = dal.GetTravelDetails(travelid);
                int SumitedUserEmpId = Convert.ToInt32(TravelDetails.EmployeeId);
                ViewBag.SumitedUserEmpId = SumitedUserEmpId;
                string UserRolesOrganSubmited = dal.GetEmployeeCode(SumitedUserEmpId);
                string[] usersInRoles = Roles.GetRolesForUser(UserRolesOrganSubmited);

                if (usersInRoles.Contains("Delivery Manager"))
                {
                    ViewBag.SumitedUserEmpRole = "Delivery Manager";
                }
                if (usersInRoles.Contains("Account Owners"))
                {
                    ViewBag.SumitedUserEmpRole = "Account Owners";
                }
                if (usersInRoles.Contains("Group Head"))
                {
                    ViewBag.SumitedUserEmpRole = "Group Head";
                }
                if (usersInRoles.Contains("Management"))
                {
                    ViewBag.SumitedUserEmpRole = "Management";
                }

                if (SumitedUserEmpId != loginEmpId && (ViewBag.SumitedUserEmpRole == "Delivery Manager" || ViewBag.SumitedUserEmpRole == "Account Owners" || ViewBag.SumitedUserEmpRole == "Group Head" || ViewBag.SumitedUserEmpRole == "Management"))
                {
                    status = dal.RejectTravelApprovalFormAdminForOrgMem(model, loginEmpId, travelid, userrole, ViewBag.SumitedUserEmpRole);
                }
                else
                {
                    status = dal.RejectTravelApprovalFormAdmin(model, loginEmpId, travelid);
                }
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status, travelID = travelID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SearchEmployeeAutoSuggestTravel(string term)
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

        [HttpPost]
        public ActionResult TravelSendMail(string successEmpIDs, int loggedinEmpID, int templateID)
        {
            try
            {
                TravelViewModel model = new TravelViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                model.Mail = new TravelMailTemplate();
                HRMS_tbl_PM_Employee fromEmployeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(successEmpIDs));
                HRMS_tbl_PM_Employee ToEmployeeDetails = employeeDAL.GetEmployeeDetails(loggedinEmpID);

                if (fromEmployeeDetails != null)
                {
                    model.Mail.From = fromEmployeeDetails.EmailID;
                    model.Mail.To = ToEmployeeDetails.EmailID;

                    //int mailtemplateId = templateID;
                    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateID);
                    foreach (var emailTemplate in template)
                    {
                        model.Mail.Subject = emailTemplate.Subject;
                        model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromEmployeeDetails.EmployeeName);
                    }
                }

                return PartialView("_TravelSendEmail", model.Mail);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult SendEmail(EmployeeMailTemplate model)
        {
            bool result = false;
            EmployeeDAL employeeDAL = new EmployeeDAL();
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

        [HttpPost]
        public ActionResult ChkAllGridStatus(int TravelID, int CountryId, int TravelType, string TravelTRFNo)
        {
            try
            {
                List<bool> status = new List<bool>();

                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);
                status = travelDAL.GetAllGridStatus(travelid, CountryId, TravelType, TravelTRFNo);
                bool ClientG, PassG, ContactG, Contact1G, VisaG, JourneyG;

                ClientG = status[0];
                PassG = status[1];
                ContactG = status[2];
                Contact1G = status[3];
                VisaG = status[4];
                JourneyG = status[5];
                return Json(new { ClientG = ClientG, PassG = PassG, ContactG = ContactG, Contact1G = Contact1G, VisaG = VisaG, JourneyG = JourneyG }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetStatusForNewExtensionForm(string TravelID)
        {
            try
            {
                bool status;
                TravelDAL dal = new TravelDAL();
                bool isAuthorizeExpense;
                string decryptedTravelId = HRMSHelper.Decrypt(TravelID, out isAuthorizeExpense);
                int travelid = Convert.ToInt32(decryptedTravelId);
                Tbl_HR_Travel emp = dbContext.Tbl_HR_Travel.Where(ed => ed.TravelId == travelid).FirstOrDefault();
                status = travelDAL.CheckExtensionStageId(emp.TRFNo);

                var TRFNoAuto = dal.GetNewExtensionTRFNo(emp.TRFNo);
                bool statustrfno = dal.GetTRFNoIsValide(TRFNoAuto);
                string NewExtTRFNo = string.Empty;

                while (statustrfno == true)
                {
                    TRFNoAuto = dal.GetNewExtensionTRFNo(TRFNoAuto);
                    bool finalstatus = dal.GetTRFNoIsValide(TRFNoAuto);
                    if (finalstatus == false)
                    {
                        statustrfno = false;
                    }
                }
                decimal Trfno = Convert.ToDecimal(TRFNoAuto);
                decimal newTrf = decimal.Subtract(Trfno, 0.1M);
                return Json(new { status = status, newTrf = newTrf }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult FinalApproveValidationAdmin(int TravelID)
        {
            try
            {
                bool status;
                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);
                status = travelDAL.CheckAdminConveyanceUploadStatus(travelid);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CheckValidationAccTab(int TravelID)
        {
            try
            {
                bool status;
                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);
                status = travelDAL.CheckValidationAccTabDeatails(travelid);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ChkAllStatusForAdminGrids(int TravelID)
        {
            try
            {
                List<bool> status = new List<bool>();
                string decryptedTravelId = Session["Travelid"].ToString();
                int travelid = Convert.ToInt32(decryptedTravelId);
                status = travelDAL.GetAllAdminStatusGrid(travelid);
                bool AccmG, LocalConG, MisceG;

                AccmG = status[0];
                LocalConG = status[1];
                MisceG = status[2];

                return Json(new { AccmG = AccmG, LocalConG = LocalConG, MisceG = MisceG }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public List<FieldChildDetails> GetFieldChildDetailsList(string travelField)
        {
            try
            {
                TravelDAL dal = new TravelDAL();

                List<FieldChildDetails> childs = dal.GetFieldChildDetailsList(travelField);
                childs.ToList();
                return childs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult TravelStatusReportEmpolyee(int TravelId)
        {
            TravelReport ReportModel = new TravelReport();
            TravelDAL dal = new TravelDAL();
            Tbl_HR_TravelJourneyDetails Jourdetails = dbContext.Tbl_HR_TravelJourneyDetails.Where(cd => cd.TravelId == TravelId).FirstOrDefault();
            int DocumentId = Jourdetails.Id;
            ViewBag.DocId = DocumentId;
            ViewBag.TrId = TravelId;
            ReportModel.TravelReportList = dal.GetTravelReportForEmployee(TravelId);
            ReportModel.TravelAirportToHotel = dal.GetTravelToHotelList(TravelId);
            ReportModel.IndEmergencyContactDetails = dal.GetAllEmergencyContacts(TravelId);
            return PartialView("EmployeeTravelStatusReport", ReportModel);
        }

        [HttpPost]
        public void ExportToExcelTravel(TravelReport app)
        {
            DataSet ds = new DataSet();
            GridView gv = new GridView();
            HyperLink hplink = new HyperLink();
            DataTable dt = new DataTable();
            if (app.TravelReportList != null)
            {
                List<TravelReportViewModel> report = new List<TravelReportViewModel>();
                app.TravelReportList.ForEach(x =>
                {
                    TravelReportViewModel rep = new TravelReportViewModel();
                    rep.travelid = x.travelid;
                    rep.TRFNO = x.TRFNO;
                    rep.EmployeeCode = x.EmployeeCode;
                    rep.EmployeeName = x.EmployeeName;
                    rep.Group = x.Group;
                    rep.ReportingManager = x.ReportingManager;
                    rep.DepartFromBaseLocation = x.DepartFromBaseLocation;
                    rep.ArrivalDestination = x.ArrivalDestination;
                    rep.DepartFromDestination = x.DepartFromDestination;
                    rep.ArrivalBaseDestination = x.ArrivalBaseDestination;
                    rep.TavelContactNo = x.TavelContactNo;
                    rep.HotelAddress = x.HotelAddress;
                    rep.HotelRoomNo = x.HotelRoomNo;
                    rep.ClientName = x.ClientName;
                    rep.clientAddress = x.clientAddress;
                    rep.ClientContactPerson = x.ClientContactPerson;
                    rep.ClientContactNo = x.ClientContactNo;
                    rep.VisaValiditydate = x.VisaValiditydate;
                    rep.InsurenceDetails = x.InsurenceDetails;
                    rep.TravelStatus = x.TravelStatus;
                    rep.Comments = x.Comments;
                    rep.TicketAttachment = x.TicketAttachment;
                    hplink.Text = x.TicketAttachment;
                    rep.InsurenceAttachment = x.InsurenceAttachment;
                    rep.TicketName = x.TicketName;
                    report.Add(rep);
                });

                List<ConveyanceAdminViewModel> report2 = new List<ConveyanceAdminViewModel>();
                if (app.TravelAirportToHotel != null)
                {
                    app.TravelAirportToHotel.ForEach(x =>
                    {
                        ConveyanceAdminViewModel rep2 = new ConveyanceAdminViewModel();
                        rep2.ConveyplusTravelDetails = x.ConvayName + "," + x.TravelDetails;
                        rep2.TravelID = x.TravelID;
                        report2.Add(rep2);
                    });
                }

                List<EmergencyContactViewModel> report3 = new List<EmergencyContactViewModel>();
                if (app.IndEmergencyContactDetails != null)
                {
                    app.IndEmergencyContactDetails.ForEach(x =>
                    {
                        EmergencyContactViewModel rep3 = new EmergencyContactViewModel();
                        //rep3.ConcateReportField = x.Name + "\n" + x.Relation + "\n" + x.EmgAddress + "\n" + x.ContactNo;
                        rep3.ConcateReportField = "Name:" + x.Name + "\r\n" + "Relation:" + x.Relation + "\r\n" +
                                                  "Location:" + x.EmgAddress + "\r\n" + "Conatct No:" + x.ContactNo;
                        rep3.TravelID = x.TravelID;
                        report3.Add(rep3);
                    });
                }
                List<AccomodationAdmin> report4 = new List<AccomodationAdmin>();
                if (app.GetAccomodationDetailsList != null)
                {
                    app.GetAccomodationDetailsList.ForEach(x =>
                        {
                            AccomodationAdmin rep4 = new AccomodationAdmin();
                            rep4.HotelAddress = x.HotelAddress;
                            rep4.HotelContactNumber = x.HotelContactNumber;
                            rep4.TravelId = x.TravelId;
                            report4.Add(rep4);
                        });
                }
                List<ClientViewModel> report5 = new List<ClientViewModel>();
                if (app.GetClientDetailsList != null)
                {
                    app.GetClientDetailsList.ForEach(x =>
                    {
                        ClientViewModel rep5 = new ClientViewModel();
                        rep5.ClientName = x.ClientName;
                        rep5.ClientAddress = x.ClientAddress;
                        rep5.ClientContact = x.ClientContact;
                        rep5.ClientContactNumber = x.ClientContactNumber;
                        rep5.TravelId = x.TravelId;
                        report5.Add(rep5);
                    });
                }
                List<OtherAdminViewModel> report6 = new List<OtherAdminViewModel>();
                if (app.GetOtherRequirementDetailsList != null)
                {
                    app.GetOtherRequirementDetailsList.ForEach(x =>
                    {
                        OtherAdminViewModel rep6 = new OtherAdminViewModel();
                        rep6.InsurenceDetails = x.InsurenceDetails;
                        rep6.FileName = x.FileName;
                        rep6.TravelId = x.TravelId;
                        report6.Add(rep6);
                    });
                }
                List<JourneyList> report7 = new List<JourneyList>();
                if (app.GetJourneyDetailsList123 != null)
                {
                    app.GetJourneyDetailsList123.ForEach(x =>
                    {
                        JourneyList rep7 = new JourneyList();
                        rep7.TicketName = x.TicketName;
                        rep7.TravelID = x.TravelID;
                        report7.Add(rep7);
                    });
                }

                dt.Columns.Add("TRF No.", typeof(string));
                dt.Columns.Add("Employee ID", typeof(string));
                dt.Columns.Add("Employee Name", typeof(string));
                dt.Columns.Add("Group", typeof(string));
                dt.Columns.Add("Reporting Manager", typeof(string));
                dt.Columns.Add("Departure from base location", typeof(string));
                dt.Columns.Add("Arrival at destination", typeof(string));
                dt.Columns.Add("Departure from destination", typeof(string));
                dt.Columns.Add("Arrival at base location", typeof(string));
                dt.Columns.Add("Mobile Travelling Destination", typeof(string));

                dt.Columns.Add("Hotel Address", typeof(string));
                dt.Columns.Add("Hotel Room No.", typeof(string));
                dt.Columns.Add("Client Name", typeof(string));
                dt.Columns.Add("Client Address", typeof(string));
                dt.Columns.Add("Client Contact Person", typeof(string));
                dt.Columns.Add("Client Contact", typeof(string));
                dt.Columns.Add("Visa Validity /  Expiry Date", typeof(string));
                dt.Columns.Add("Insurance Policy Details", typeof(string));
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("Comments", typeof(string));
                dt.Columns.Add("Ticket Attachment", typeof(string));
                dt.Columns.Add("Insurance", typeof(string));
                dt.Columns.Add("Travel from Airport to Hotel", typeof(string));
                dt.Columns.Add("India Emergency Contact", typeof(string));

                foreach (var array in report)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = array.TRFNO;
                    dr[1] = array.EmployeeCode;
                    dr[2] = array.EmployeeName;
                    dr[3] = array.Group;
                    dr[4] = array.ReportingManager;
                    dr[5] = array.DepartFromBaseLocation;
                    dr[6] = array.ArrivalDestination;
                    dr[7] = array.DepartFromDestination;
                    dr[8] = array.ArrivalBaseDestination;
                    dr[9] = array.TavelContactNo;

                    for (int iter = 0; iter < report4.Count; iter++)
                    {
                        if (array.travelid == report4[iter].TravelId)
                        {
                            if (report4[iter].HotelAddress != null)
                            {
                                dr[10] = dr[10] + "," + report4[iter].HotelAddress;

                                string str = dr[10].ToString();
                                if (str[0] == ',')
                                {
                                    str = str.Remove(0, 1);
                                    dr[10] = str;
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                    for (int iter = 0; iter < report4.Count; iter++)
                    {
                        if (array.travelid == report4[iter].TravelId)
                        {
                            if (report4[iter].HotelContactNumber != null)
                            {
                                dr[11] = dr[11] + "," + report4[iter].HotelContactNumber;

                                string str = dr[11].ToString();
                                if (str[0] == ',')
                                {
                                    str = str.Remove(0, 1);
                                    dr[11] = str;
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                    for (int iter = 0; iter < report5.Count; iter++)
                    {
                        if (array.travelid == report5[iter].TravelId)
                        {
                            if (report5[iter].ClientName != null)
                            {
                                dr[12] = dr[12] + "," + report5[iter].ClientName;

                                string str = dr[12].ToString();
                                if (str[0] == ',')
                                {
                                    str = str.Remove(0, 1);
                                    dr[12] = str;
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                    for (int iter = 0; iter < report5.Count; iter++)
                    {
                        if (array.travelid == report5[iter].TravelId)
                        {
                            if (report5[iter].ClientAddress != null)
                            {
                                dr[13] = dr[13] + "," + report5[iter].ClientAddress;

                                string str = dr[13].ToString();
                                if (str[0] == ',')
                                {
                                    str = str.Remove(0, 1);
                                    dr[13] = str;
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                    for (int iter = 0; iter < report5.Count; iter++)
                    {
                        if (array.travelid == report5[iter].TravelId)
                        {
                            if (report5[iter].ClientContact != null)
                            {
                                dr[14] = dr[14] + "," + report5[iter].ClientContact;

                                string str = dr[14].ToString();
                                if (str[0] == ',')
                                {
                                    str = str.Remove(0, 1);
                                    dr[14] = str;
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                    for (int iter = 0; iter < report5.Count; iter++)
                    {
                        if (array.travelid == report5[iter].TravelId)
                        {
                            if (report5[iter].ClientContactNumber != null)
                            {
                                dr[15] = dr[15] + "," + report5[iter].ClientContactNumber;

                                string str = dr[15].ToString();
                                if (str[0] == ',')
                                {
                                    str = str.Remove(0, 1);
                                    dr[15] = str;
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                    dr[16] = array.VisaValiditydate;

                    for (int iter = 0; iter < report6.Count; iter++)
                    {
                        if (array.travelid == report6[iter].TravelId)
                        {
                            if (report6[iter].InsurenceDetails != null)
                            {
                                dr[17] = dr[17] + "," + report6[iter].InsurenceDetails;

                                string str = dr[17].ToString();
                                if (str[0] == ',')
                                {
                                    str = str.Remove(0, 1);
                                    dr[17] = str;
                                }
                            }
                        }
                        else
                        {
                        }
                    }

                    dr[18] = array.TravelStatus;
                    dr[19] = array.Comments;

                    for (int iter = 0; iter < report7.Count; iter++)
                    {
                        if (array.travelid == report7[iter].TravelID)
                        {
                            if (report7[iter].TicketName != null)
                            {
                                dr[20] = dr[20] + "," + report7[iter].TicketName;

                                string str = dr[20].ToString();
                                if (str[0] == ',')
                                {
                                    str = str.Remove(0, 1);
                                    dr[20] = str;
                                }
                            }
                        }
                        else
                        {
                        }
                    }

                    for (int iter = 0; iter < report6.Count; iter++)
                    {
                        if (array.travelid == report6[iter].TravelId)
                        {
                            if (report6[iter].FileName != null)
                            {
                                dr[21] = dr[21] + "," + report6[iter].FileName;

                                string str = dr[21].ToString();
                                if (str[0] == ',')
                                {
                                    str = str.Remove(0, 1);
                                    dr[21] = str;
                                }
                            }
                        }
                        else
                        {
                        }
                    }

                    for (int iter = 0; iter < report2.Count; iter++)
                    {
                        if (array.travelid == report2[iter].TravelID)
                        {
                            if (report2[iter].ConveyplusTravelDetails != null)
                            {
                                dr[22] = dr[22] + "," + report2[iter].ConveyplusTravelDetails;

                                string str = dr[22].ToString();
                                if (str[0] == ',')
                                {
                                    str = str.Remove(0, 1);
                                    dr[22] = str;
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                    for (int iter = 0; iter < report3.Count; iter++)
                    {
                        if (array.travelid == report3[iter].TravelID)
                        {
                            if (report3[iter].ConcateReportField != null)
                            {
                                dr[23] = dr[23] + "," + report3[iter].ConcateReportField;

                                string str = dr[23].ToString();
                                if (str[0] == ',')
                                {
                                    str = str.Remove(0, 1);
                                    dr[23] = str;
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                    dt.Rows.Add(dr);
                }

                var totalRecords = "Total Records :" + app.TravelReportList.Count();
                dt.Rows.Add(new[] { totalRecords });
                gv.DataSource = dt;
            }

            gv.DataBind();

            Response.AddHeader("content-disposition", "attachment; filename=TravelReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            DataGrid g = new DataGrid();

            g.DataSource = dt;
            g.DataBind();
            foreach (DataGridItem i in g.Items)
            {
                int p = 0;
                foreach (TableCell tc in i.Cells)
                {
                    if (p >= 5 && p <= 8)
                    {
                        tc.Attributes.Add("class", "DateClass");
                        p++;
                    }
                    else
                    {
                        p++;
                        tc.Attributes.Add("class", "text");
                    }
                }
            }

            g.RenderControl(htmlWrite);
            string style = @"<style> .text { mso-number-format:\@; } </style> ";
            string styleDate = @"<style> .DateClass { mso-number-format:d\-mmm\-yyyy; } </style> ";

            Response.Write(style);
            Response.Write(styleDate);
            Response.Write(stringWrite.ToString());
            Response.End();
        }

        [HttpGet]
        public ActionResult TravelStatusReportForAllEmployee()
        {
            TravelReport ReportModel = new TravelReport();
            TravelDAL dal = new TravelDAL();

            List<TravelReportViewModel> finalReports = new List<TravelReportViewModel>();
            List<ConveyanceAdminViewModel> finalairportToHotelReports = new List<ConveyanceAdminViewModel>();
            List<EmergencyContactViewModel> finalemergencyContactDetails = new List<EmergencyContactViewModel>();
            List<ClientViewModel> finalGetClientContacts = new List<ClientViewModel>();
            List<AccomodationAdmin> finalAccomodationDetails = new List<AccomodationAdmin>();
            List<JourneyList> finalJourneyDetail = new List<JourneyList>();
            List<int> travelJourneyDetails = new List<int>();
            List<OtherAdminViewModel> finalOtherDetails = new List<OtherAdminViewModel>();

            List<GetTravelID> travelIDListCount = dal.GetTravelIDs();

            foreach (var item in travelIDListCount)
            {
                Tbl_HR_TravelJourneyDetails jourDetails = dbContext.Tbl_HR_TravelJourneyDetails.Where(cd => cd.TravelId == item.TravelId).FirstOrDefault();
                if (jourDetails != null)
                {
                    travelJourneyDetails.Add(jourDetails.Id);
                }

                List<TravelReportViewModel> reports = new List<TravelReportViewModel>();
                List<ConveyanceAdminViewModel> airportToHotelReports = new List<ConveyanceAdminViewModel>();
                List<EmergencyContactViewModel> emergencyContactDetails = new List<EmergencyContactViewModel>();
                List<ClientViewModel> GetClientContacts = new List<ClientViewModel>();
                List<AccomodationAdmin> AccomodationDetails = new List<AccomodationAdmin>();
                List<JourneyList> journeyDetail = new List<JourneyList>();
                List<OtherAdminViewModel> otherDetails = new List<OtherAdminViewModel>();

                otherDetails = dal.GetOtherRequirementDetailsList(item.TravelId.HasValue ? item.TravelId.Value : 0);
                journeyDetail = dal.GetJourneyDetailsList(item.TravelId.HasValue ? item.TravelId.Value : 0);
                AccomodationDetails = dal.GetAccomodationDetailsList(item.TravelId.HasValue ? item.TravelId.Value : 0);
                GetClientContacts = dal.GetClientContactList(item.TravelId.HasValue ? item.TravelId.Value : 0);
                reports = dal.GetTravelReportForEmployee(item.TravelId.HasValue ? item.TravelId.Value : 0);
                airportToHotelReports = dal.GetTravelToHotelList(item.TravelId.HasValue ? item.TravelId.Value : 0);
                emergencyContactDetails = dal.GetAllEmergencyContacts(item.TravelId.HasValue ? item.TravelId.Value : 0);

                foreach (var other in otherDetails)
                {
                    finalOtherDetails.Add(other);
                }

                foreach (var clientViewModel in GetClientContacts)
                {
                    finalGetClientContacts.Add(clientViewModel);
                }
                foreach (var Accomodation in AccomodationDetails)
                {
                    finalAccomodationDetails.Add(Accomodation);
                }
                foreach (var journey in journeyDetail)
                {
                    finalJourneyDetail.Add(journey);
                }

                foreach (var travelReportViewModel in reports)
                {
                    finalReports.Add(travelReportViewModel);
                }
                foreach (var conveyanceAdminViewModel in airportToHotelReports)
                {
                    finalairportToHotelReports.Add(conveyanceAdminViewModel);
                }
                foreach (var emergencyContactViewModel in emergencyContactDetails)
                {
                    finalemergencyContactDetails.Add(emergencyContactViewModel);
                }
            }
            ReportModel.JourneyDetailsId = travelJourneyDetails;
            ReportModel.TravelReportList = finalReports;
            ReportModel.TravelAirportToHotel = finalairportToHotelReports;
            ReportModel.IndEmergencyContactDetails = finalemergencyContactDetails;
            ReportModel.GetClientDetailsList = finalGetClientContacts;
            ReportModel.GetAccomodationDetailsList = finalAccomodationDetails;
            ReportModel.GetJourneyDetailsList123 = finalJourneyDetail;
            ReportModel.GetOtherRequirementDetailsList = finalOtherDetails;
            ReportModel.GetTravelIDList = travelIDListCount;

            return PartialView("EmployeeTravelStatusReport", ReportModel);
        }

        public ActionResult DownloadAdminAccomUploadedFile(int TravelID, int AccomodationID)
        {
            try
            {
                TravelDAL DAL = new TravelDAL();
                AccomodationAdmin Details = DAL.GetAdminAccomodationShowHistory(TravelID, AccomodationID);
                string FileName = Details.FileName;
                string[] FileExtention = FileName.Split('.');
                string contentType = "application/" + FileExtention[1];
                string Filepath = Path.Combine(Details.FilePath, FileName);
                if (!System.IO.File.Exists(Filepath))
                {
                    throw new Exception();
                }
                return File(Filepath, contentType, FileName);
            }
            catch (Exception)
            {
                ConfigurationViewModel model = new ConfigurationViewModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                string employeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(employeeCode);
                if (employeeCode != null)
                {
                    CommonMethodsDAL Commondal = new CommonMethodsDAL();
                    model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }
                return PartialView("_FileNotFound", model);
            }
        }

        [HttpPost]
        public ActionResult checkValidEmployeeVisaDetails(int employeeId, int countryId)
        {
            try
            {
                TravelDAL dal = new TravelDAL();
                CheckPassportValid CheckPassportValid = new CheckPassportValid();
                CheckPassportValid = dal.checkValidEmployeeVisaDetail(employeeId, countryId);
                ViewBag.CountryId = countryId;
                TravelViewModel travel = new TravelViewModel();
                travel.VisaDetails = new VisaViewModel();
                travel.VisaDetails.CountryID = countryId;

                Session["CountryId"] = countryId;
                if (CheckPassportValid.IsVisaValid == true && CheckPassportValid.IsVisaExist == true && CheckPassportValid.IsVisaRequired == true)
                    return Json(new { IsVisaValid = true, IsVisaExist = true, IsVisaRequired = true }, JsonRequestBehavior.AllowGet);
                if (CheckPassportValid.IsVisaValid == false && CheckPassportValid.IsVisaExist == false && CheckPassportValid.IsVisaRequired == false)
                    return Json(new { IsVisaValid = false, IsVisaExist = false, IsVisaRequired = false }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { IsVisaValid = false, IsVisaExist = false, IsVisaRequired = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public System.Threading.Timer myTimer;

        public void SetTimerValue()
        {
            // trigger the event at 9 AM. For 7 PM use 21 i.e. 24 hour format
            DateTime requiredTime = DateTime.Today.AddHours(9).AddMinutes(00);
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
            sendMail();
            // now, call the set timer method to reset its next call time
            SetTimerValue();
        }

        private string GetADOConnectionString()
        {
            HRMSDBEntities ctx = new HRMSDBEntities(); //create your entity object here
            EntityConnection ec = (EntityConnection)ctx.Connection;
            SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
            string adoConnStr = sc.ConnectionString;
            return adoConnStr;
        }

        public void sendMail()
        {
            string constring = GetADOConnectionString();
            SqlConnection con = new SqlConnection(constring);
            DateTime today = DateTime.Today;
            DateTime sevenDaysEarlier = today.AddDays(7);
            DateTime TwoDaysEarlier = today.AddDays(2);

            DateTime TwoDaysAfter = today.AddDays(-2);

            string records = "Select * from Tbl_HR_Travel where TravelExtensionEndDate = '" + sevenDaysEarlier + "' OR TravelEndDate = '" + sevenDaysEarlier + "'";
            string Twodaysrecords = "Select * from Tbl_HR_Travel where TravelExtensionEndDate = '" + TwoDaysEarlier + "' OR TravelEndDate = '" + TwoDaysEarlier + "'";

            string JourneyQry = "Select * from Tbl_HR_Travel where TravelExtensionEndDate = '" + TwoDaysAfter + "' OR TravelEndDate = '" + TwoDaysAfter + "'";

            con.Open();
            SqlDataAdapter da3 = new SqlDataAdapter(Twodaysrecords, con);
            DataSet ds3 = new DataSet();
            da3.Fill(ds3);

            SqlDataAdapter da = new SqlDataAdapter(records, con);
            DataSet ds = new DataSet();
            da.Fill(ds);

            SqlDataAdapter Journeyda = new SqlDataAdapter(JourneyQry, con);
            DataSet Journeyds = new DataSet();
            Journeyda.Fill(Journeyds);

            List<int> TravelJourneyEmpolyeeid = new List<int>();
            foreach (DataTable t in Journeyds.Tables)
            {
                foreach (DataRow row in t.Rows)
                {
                    string EmployeeIds = row["EmployeeId"].ToString();
                    int EmpidJouy = Convert.ToInt32(EmployeeIds);
                    TravelJourneyEmpolyeeid.Add(EmpidJouy);
                }
            }

            SqlDataAdapter daJouEmail = new SqlDataAdapter();
            DataSet dsJouEmail = new DataSet();
            foreach (var item in TravelJourneyEmpolyeeid)
            {
                string EmployeeRecords = "Select EmailID from HRMS_tbl_PM_Employee where EmployeeID = '" + item + "'";
                daJouEmail = new SqlDataAdapter(EmployeeRecords, con);
                daJouEmail.Fill(dsJouEmail);
            }

            List<string> JourneyFinalEmailds = new List<string>();

            foreach (DataTable t in dsJouEmail.Tables)
            {
                foreach (DataRow row in t.Rows)
                {
                    string EmailId = row["EmailID"].ToString();
                    JourneyFinalEmailds.Add(EmailId);
                }
            }

            var values = new List<Tuple<int, DateTime>>();

            foreach (DataTable t in ds.Tables)
            {
                foreach (DataRow row in t.Rows)
                {
                    string EmployeeIds = row["EmployeeId"].ToString();
                    string EndDate = string.Empty;
                    string ExtDate = string.Empty;
                    if (row["TravelEndDate"].ToString() != null)
                    {
                        EndDate = row["TravelEndDate"].ToString();
                    }
                    else
                    {
                        ExtDate = row["TravelExtensionEndDate"].ToString();
                    }
                    DateTime ActualExtDate;
                    if (EndDate != "")
                    {
                        ActualExtDate = Convert.ToDateTime(EndDate);
                    }
                    else
                    {
                        ActualExtDate = Convert.ToDateTime(ExtDate);
                    }
                    int empid = Convert.ToInt32(EmployeeIds);
                    values.Add(new Tuple<int, DateTime>(empid, ActualExtDate));
                }
            }

            foreach (DataTable t in ds3.Tables)
            {
                foreach (DataRow row in t.Rows)
                {
                    string EmployeeIds = row["EmployeeId"].ToString();
                    string EndDate = string.Empty;
                    string ExtDate = string.Empty;
                    if (row["TravelEndDate"].ToString() != null)
                    {
                        EndDate = row["TravelEndDate"].ToString();
                    }
                    else
                    {
                        ExtDate = row["TravelExtensionEndDate"].ToString();
                    }
                    DateTime ActualExtDate;
                    if (EndDate != "")
                    {
                        ActualExtDate = Convert.ToDateTime(EndDate);
                    }
                    else
                    {
                        ActualExtDate = Convert.ToDateTime(ExtDate);
                    }
                    int empid = Convert.ToInt32(EmployeeIds);
                    values.Add(new Tuple<int, DateTime>(empid, ActualExtDate));
                }
            }

            List<string> EmaildsList = new List<string>();

            SqlDataAdapter da2 = new SqlDataAdapter();
            DataSet ds2 = new DataSet();
            foreach (var item in values)
            {
                string EmployeeRecords = "Select EmailID from HRMS_tbl_PM_Employee where EmployeeID = '" + item.Item1 + "'";
                da2 = new SqlDataAdapter(EmployeeRecords, con);
                da2.Fill(ds2);
            }
            foreach (DataTable t in ds2.Tables)
            {
                foreach (DataRow row in t.Rows)
                {
                    string EmailId = row["EmailID"].ToString();
                    EmaildsList.Add(EmailId);
                }
            }

            string[] EmaildsList2;
            EmaildsList2 = System.Web.Security.Roles.GetUsersInRole("Travel_Admin");
            List<string> EmaildsCCList = new List<string>();
            SqlDataAdapter da4 = new SqlDataAdapter();
            DataSet ds4 = new DataSet();
            foreach (var item in EmaildsList2)
            {
                string EmployeeRecords = "Select EmailID from HRMS_tbl_PM_Employee where EmployeeCode = '" + item + "'";
                da4 = new SqlDataAdapter(EmployeeRecords, con);
                da4.Fill(ds4);
            }
            foreach (DataTable t in ds4.Tables)
            {
                foreach (DataRow row in t.Rows)
                {
                    string EmailId = row["EmailID"].ToString();
                    EmaildsCCList.Add(EmailId);
                }
            }

            for (int i = 0; i < EmaildsList.Count; i++)
            {
                MailMessage mail = new MailMessage();

                mail.To.Add(EmaildsList[i]);

                foreach (var item1 in EmaildsCCList)
                {
                    mail.CC.Add(item1);
                }
                string strEmailAddrFrom = "TravelAdmin@v2solutions.com";
                mail.From = new MailAddress(strEmailAddrFrom, "TravelAdmin");
                mail.Subject = "Travel Extension";
                mail.Body = "Hi,</br></br> Your travel end date is " + values[i].Item2 + ". In case you need to extend your travel, please fill the travel extension form in Vibrant Web. </br>Go to Departments > Admin > Travel Process > New Extension Request. </br></br></br></br> Regards,</br>Travel Team.";
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                SmtpClient myMailClient = new SmtpClient();
                myMailClient.Host = "smtp.gmail.com";
                myMailClient.Port = 587;
                myMailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                myMailClient.Send(mail);
                mail.Dispose();
            }

            for (int i = 0; i < JourneyFinalEmailds.Count; i++)
            {
                MailMessage mail = new MailMessage();

                mail.To.Add(EmaildsList[i]);

                foreach (var item1 in EmaildsCCList)
                {
                    mail.CC.Add(item1);
                }
                string strEmailAddrFrom = "TravelAdmin@v2solutions.com";
                mail.From = new MailAddress(strEmailAddrFrom, "TravelAdmin");
                mail.Subject = "Travel Extension";
                mail.Body = "Hi,</br></br> please fill your Journey feedback details in Vibrant Web.</br>Go to Departments > Admin > Travel Process > Saved/Submitted Requests. </br></br></br></br> Regards,</br>Travel Team.";
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                SmtpClient myMailClient = new SmtpClient();
                myMailClient.Host = "webmail.in.v2solutions.com";
                myMailClient.Port = 587;
                myMailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                myMailClient.Send(mail);
                mail.Dispose();
            }
            con.Close();
        }
    }
}