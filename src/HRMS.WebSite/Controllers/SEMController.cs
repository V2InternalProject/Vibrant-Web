using HRMS.DAL;
using HRMS.Models;
using HRMS.Notification;
using MvcApplication3.Filters;
using System;
using System.Collections;
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
using V2.CommonServices.FileLogger;

namespace HRMS.Controllers
{
    public class SEMController : Controller
    {
        //
        // GET: /SEM/

        public string UploadContractLocation
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadContractFileLocation"];
            }
        }

        public string UploadManagerDocumentFileLocation
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadManagerDocumentFileLocation"];
            }
        }

        [PageAccess(PageName = "Customer")]
        public ActionResult Index(int? EmpCode, int? PageID)
        {
            Session["SearchEmpFullName"] = null;  // to hide emp search
            Session["SearchEmpCode"] = null;
            Session["SearchEmpID"] = null;

            SEMViewModel model = new SEMViewModel();
            EmployeeDAL DAL = new EmployeeDAL();
            model.SearchedUserDetails = new SearchedUserDetails();
            Session["NodeLevelAccess"] = DAL.GetPageLevelAccessForEmployee(Convert.ToInt32(EmpCode), Convert.ToInt32(PageID));
            return View(model);
        }

        public ActionResult AddCustomer()
        {
            AddCustomerModel model = new AddCustomerModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        [PageAccess(PageName = "Manage Project")]
        public ActionResult ProjectIndex()
        {
            Session["SearchEmpFullName"] = null;  // to hide emp search
            Session["SearchEmpCode"] = null;
            Session["SearchEmpID"] = null;
            SemDAL dal = new SemDAL();
            PMSProjectDetailsViewModel model = new PMSProjectDetailsViewModel();
            EmployeeDAL employeeDAL = new EmployeeDAL();
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            ViewBag.AddProjectID = Commondal.Encrypt(Convert.ToString(0), true);
            int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);

            var roles = (List<AccessRightMapping>)HttpContext.Session["AccessRights"];
            if (roles.Where(t => t.CanAdd == 1).Any())
            {
                ViewBag.ProjectCreator = "Project _Creator";
            }
            string user = Commondal.GetMaxRoleForUser(role);
            ViewBag.UserRole = user;
            model.SearchedUserDetails = new SearchedUserDetails();
            model.SearchedUserDetails.UserRole = user;
            model.SearchedUserDetails.EmployeeId = employeeID;
            int employeeid = dal.geteEmployeeIDFromSEMDatabase(Membership.GetUser().UserName);
            model.SearchedUserDetails.IsProjectReviewer = dal.CheckIfEmployeeisReviewer(employeeid);
            model.ProjectNamesList = dal.GetProjectNamesList();
            model.PMSApprovalStatusList = dal.GetPMSApprovalStatusList();
            model.PMSProjectStatusList = dal.GetProjectStatusList();
            return View(model);
        }

        public ActionResult GetPMSProjectDetails(string ProjectIDs, string ApprovalStatusIds)
        {
            //int? ProjectID, int? ApprovalStatusId
            CommonMethodsDAL dalc = new CommonMethodsDAL();
            var decryptedApprovalStatusId = "";
            int? ApprovalStatusId;
            int tempVal;
            var decryptedProjectId = dalc.Decrypt(ProjectIDs.Replace("\"", ""), true);
            if (ApprovalStatusIds != null)
                decryptedApprovalStatusId = dalc.Decrypt(ApprovalStatusIds.Replace("\"", ""), true);
            else
                decryptedApprovalStatusId = "";
            ApprovalStatusId = Int32.TryParse(decryptedApprovalStatusId, out tempVal) ? tempVal : (int?)null;

            int? ProjectID = int.Parse(decryptedProjectId);
            SemDAL dal = new SemDAL();
            PMSProjectDetailsViewModel model = new PMSProjectDetailsViewModel();
            EmployeeDAL employeeDAL = new EmployeeDAL();
            string[] LogeedInEmRoles = { };
            string[] ProjectApprovers = { };
            string[] HRAdmin = { };
            ProjectApprovers = Roles.GetUsersInRole("Project_Approver");
            if (ProjectApprovers.Length != 0)
                model.IsProjectApproverPresent = true;
            HRAdmin = Roles.GetUsersInRole("HR Admin");
            foreach (string user in HRAdmin)
            {
                HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(user));
                if (employee == null)
                    model.HrAdminEMailIds = model.HrAdminEMailIds + string.Empty;
                else
                    model.HrAdminEMailIds = model.HrAdminEMailIds + employee.EmailID + ";" + "</br>";
            }
            model.HrAdminEMailIds = model.HrAdminEMailIds.Replace("</br>", Environment.NewLine);
            model.SearchedUserDetails = new SearchedUserDetails();
            ViewBag.AsciiKey = Session["SecurityKey"].ToString();
            string employeeCode = Membership.GetUser().UserName;
            model.loggedInUserEmployeeCode = employeeCode;
            int employeeId = employeeDAL.GetEmployeeID(employeeCode);
            model.userId = employeeId;
            ViewBag.loginUserId = employeeId;
            ViewBag.ProjectId = ProjectID;
            Session["ProjectId"] = ProjectID;
            tbl_PM_Project projectDetails = new tbl_PM_Project();
            model.Mail = new TravelMailTemplate();
            LogeedInEmRoles = Roles.GetRolesForUser(employeeCode);
            if (LogeedInEmRoles.Contains("Project_Approver"))
                model.IsProjectApprover = true;
            if (LogeedInEmRoles.Contains("RMG"))
                ViewBag.user = "RMG";
            projectDetails = dal.GetPMSProjectDetails(ProjectID);

            List<DateTime> HolidayDates = dal.getHolidayDateList();
            ViewBag.Holidaydates = HolidayDates;
            if (projectDetails != null)
            {
                model.ProjectCode = projectDetails.ProjectCode;
                model.ProjectName = projectDetails.ProjectName;
                model.ProjectID = projectDetails.ProjectID;
                model.AbbreviatedName = projectDetails.ShortJobTitle;
                model.PMSProjectGroup = projectDetails.ProjectGroupID;
                model.PMSProjectDescription = projectDetails.Description;
                model.PMSProjectStatus = projectDetails.ProjectStatusID.ToString();
                model.PMSProjectCurrency = projectDetails.BillingCurrencyID;
                model.PMSBusinessGroup = projectDetails.BusinessGroupID;
                model.PMSCommercialDetailsType = projectDetails.ContractType;
                model.PMSPractice = projectDetails.ProjectTypeID;
                model.PMSProjectStartDate = projectDetails.ActualStartDate;
                model.PMSProjectEndDate = projectDetails.ActualEndDate;
                model.OriginalDateTime = projectDetails.ActualEndDate;
                model.PMSProjectWorkHours = projectDetails.EstimatedEfforts.ToString();
                model.PMSProjectDurationDays = projectDetails.ExpectedDuration;
                model.PMSCustomer = projectDetails.CustomerID;
                model.PMSProjectBillableStatus = projectDetails.Billable;
                model.PMSOrganizationUnit = projectDetails.LocationID;
                model.PMSDeliveryUnit = projectDetails.ResourcePoolID;
                model.PMSDeliveryTeam = projectDetails.ResourceGroupID;
                model.PMSLifeCycle = projectDetails.LifeCycleID;
                model.ApprovalStaus = projectDetails.ApprovalStatusID.HasValue ? projectDetails.ApprovalStatusID.Value : 0;
                model.ApprovalStatusID = projectDetails.ApprovalStatusID.HasValue ? projectDetails.ApprovalStatusID.Value : 0;
                model.RevisionStaus = projectDetails.RevisionStatusID.HasValue ? projectDetails.RevisionStatusID.Value : 0;
                v_tbl_PM_Customer customerDetails = dal.GetCustomerDetails(projectDetails.CustomerID.HasValue ? projectDetails.CustomerID.Value : 0);
                if (customerDetails != null)
                {
                    if (customerDetails.DateSigned != null)
                        model.CustomerStartDate = customerDetails.DateSigned.Value.ToShortDateString();
                    if (customerDetails.ContractValidityDate != null)
                        model.CustomerEndDate = customerDetails.ContractValidityDate.Value.ToShortDateString();
                }
            }
            model.ShowHistory = new ShowHistoryPMS();
            model.ShowHistory.ProjectId = Convert.ToInt16(ProjectID);
            model.PMSOrganizationUnitList = dal.GetPMSOrganizationUnitList();
            model.PMSDeliveryUnitList = dal.GetPMSDeliveryUnitList();
            model.PMSDeliveryTeamList = dal.GetPMSDeliveryTeamList();
            model.PMSCustomerList = dal.GetPMSCustomerList();
            model.PMSProjectGroupList = dal.GetPMSProjectGroupList();
            model.PMSProjectStatusList = dal.GetPMSProjectStatusList();
            model.PMSProjectCurrencyList = dal.GetPMSProjectCurrencyList();
            model.PMSPracticeList = dal.GetPMSPracticeList();
            model.PMSLifeCycleList = dal.GetPMSLifeCycleList();
            model.PMSCommercialDetailsTypeList = dal.GetPMSCommercialDetailsTypeList();
            model.PMSBusinessGroupList = dal.GetPMSBusinessGroupList();
            ViewBag.EndDate = projectDetails.ActualEndDate.HasValue ? projectDetails.ActualEndDate.Value.ToShortDateString() : null;
            WSEMDBEntities dbContext = new WSEMDBEntities();
            List<PMSProjectDetailsViewModel> fieldlabellist = null;
            List<PMSProjectDetailsViewModel> approvalStatusIdList = null;
            if (ProjectID != 0 && ProjectID != null)
            {
                fieldlabellist = dal.getfieldlabellist(ProjectID);
                approvalStatusIdList = dal.approvalStatusIdList(ProjectID);
            }

            string constantMessage;
            List<string> approvalMessageList = new List<string>();
            if (approvalStatusIdList != null)
            {
                foreach (var item in approvalStatusIdList)
                {
                    if (item.RevisionStaus == 4 || item.RevisionStaus == null)
                    {
                        constantMessage = ApprovalStatusMessages.NoAction_4;
                        approvalMessageList.Add(constantMessage);
                    }
                    else if (item.RevisionStaus == 5)
                    {
                        constantMessage = ApprovalStatusMessages.Approved_2;
                        approvalMessageList.Add(constantMessage);
                    }
                    else if (item.RevisionStaus == 6)
                    {
                        constantMessage = ApprovalStatusMessages.Rejected_3;
                        approvalMessageList.Add(constantMessage);
                    }
                }
            }
            List<string> FeildList = new List<string>();
            if (fieldlabellist != null)
            {
                foreach (var item in fieldlabellist)
                {
                    FeildList.Add(item.FeildName);
                }
            }
            model.fieldlabellist = FeildList;
            model.approvalMessageList = approvalMessageList;
            ViewBag.ReviewersName = dal.GetProjectReviewersName();
            ViewBag.ProjectIRApproverName = dal.GetProjectIRApproverName();
            ViewBag.ProjectIRFinanceApproverName = dal.GetProjectIRFinanceApproverName();
            int employeeid = dal.geteEmployeeIDFromSEMDatabase(Membership.GetUser().UserName);
            model.SearchedUserDetails.IsProjectReviewer = dal.CheckIfEmployeeisReviewerForParticularProject(ProjectID, employeeid);

            return PartialView("_AddPMSProjectDetails", model);
        }

        public ActionResult TaskTypeManage()
        {
            TaskTypeManageModel model = new TaskTypeManageModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult TaskPracticeMap()
        {
            TaskPracticeMapModel model = new TaskPracticeMapModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult GeneralTaskManage()
        {
            GeneralTaskManageModel model = new GeneralTaskManageModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult CreateAssignTasks()
        {
            CreateAssignTasksModel model = new CreateAssignTasksModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult AssignTask()
        {
            AssignTaskModel model = new AssignTaskModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult GeneralTasks()
        {
            GeneralTasksModel model = new GeneralTasksModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult UploadTasks()
        {
            UploadTasksModel model = new UploadTasksModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult UploadData()
        {
            UploadDataModel model = new UploadDataModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult TimesheetEntry()
        {
            TimesheetEntryModel model = new TimesheetEntryModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult TimesheetView()
        {
            TimesheetViewModel model = new TimesheetViewModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult TimesheetApprovals()
        {
            TimesheetApprovalsModel model = new TimesheetApprovalsModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult ApproveTimesheetDetails()
        {
            ApproveTSDetailsModel model = new ApproveTSDetailsModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult ManageSubProjects()
        {
            try
            {
                ManageSubProjectsModel model = new ManageSubProjectsModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                SemDAL dal = new SemDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                string employeeCode = Membership.GetUser().UserName;
                model.loggedInUserEmployeeCode = employeeCode;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                SearchedUserDetails employeeDetails = dal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
                model.ProjectList = new List<ProjectAppList>();
                model.ProjectList = dal.GetLoggedUserProjectList(employeeDetails.UserName, "ActiveProjects", Convert.ToInt32(employeeDetails.EmployeeCode));
                //ViewBag.EmployeeLists = dal.GetEmployeeList();
                model.Prj = Convert.ToString(Session["ProjectId"]);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        #region Manage Modules

        public ActionResult ManageModules(int? ProjectId)
        {
            try
            {
                ManageModulesModel model = new ManageModulesModel();
                SemDAL semDal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
                model.ProjectList = semDal.GetLoggedUserProjectList(employeeDetails.UserName, "ActiveProjects", Convert.ToInt32(employeeDetails.EmployeeCode));
                model.ComplexityLists = semDal.GetModuleComplexityList();
                model.UserName = employeeDetails.UserName;
                model.ProjectID = ProjectId.HasValue ? ProjectId.Value : 0;
                if (Session["ProjectId"] != null)
                    model.ProjectID = Convert.ToInt32(Session["ProjectId"]);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult LoadProjectModuleGrid(int ProjectID, int page, int rows)
        {
            try
            {
                AddManageModules model = new AddManageModules();
                SemDAL dal = new SemDAL();
                int totalCount;
                List<AddManageModules> moduleDetails = new List<AddManageModules>();
                moduleDetails = dal.ProjectModuleDetailRecord(ProjectID, page, rows, out totalCount);

                if ((moduleDetails == null || moduleDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    moduleDetails = dal.ProjectModuleDetailRecord(ProjectID, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = moduleDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveProjectModuleDetails(AddManageModules model, int ComplexityID, string LoggedUserName, int ProjectID, string SelectedModuleName)
        {
            try
            {
                bool status = false;
                SemDAL dal = new SemDAL();
                SEMResponse response = new SEMResponse();
                response = dal.SaveProjectModuleRecord(model, ComplexityID, LoggedUserName, ProjectID, SelectedModuleName);

                return Json(new { status = response.status, isModuleNameExist = response.isModuleNameExist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetSelectedProjectDetails(int ProjectID)
        {
            try
            {
                bool status = false;
                SemDAL dal = new SemDAL();
                AddManageModules response = new AddManageModules();
                response = dal.GetSelectedProjectRecord(ProjectID);
                //return Json(new { ProjectStartDate = response.ProjectStartDate, ProjectEndDate = response.ProjectEndDate }, JsonRequestBehavior.AllowGet);

                ViewBag.EmployeeLists = dal.GetEmployeeList(ProjectID);
                return Json(new { ProjectStartDate = response.ProjectStartDate, ProjectEndDate = response.ProjectEndDate, ResponsilePersnList = ViewBag.EmployeeLists }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteModuleDetails(string[] SelectedModuleId)
        {
            try
            {
                SemDAL dal = new SemDAL();
                bool status = dal.DeleteModuleRecord(SelectedModuleId);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Manage Modules

        public ActionResult ManagePhases()
        {
            ManagePhasesModel model = new ManagePhasesModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            SemDAL dal = new SemDAL();
            model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
            SearchedUserDetails employeeDetails = dal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
            model.ProjectApprovedList = dal.GetLoggedUserProjectList(employeeDetails.UserName, "ActiveProjects", Convert.ToInt32(employeeDetails.EmployeeCode));

            //if (Session["ProjectId"] != null)
            //{
            //    model.projectnameId = Convert.ToInt32(Session["ProjectId"]);
            //    model.ProjectId = Convert.ToInt32(Session["ProjectId"]);
            //}
            model.projectnameId = Convert.ToInt32(Session["ProjectId"]);
            return View(model);
        }

        [HttpPost]
        public ActionResult CustomerDetailLoadGrid(string searchText, int page, int rows)
        {
            try
            {
                SEMViewModel model = new SEMViewModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<SEMViewModel> expenseDetails = new List<SEMViewModel>();
                expenseDetails = dal.CustomerDetailRecord(searchText, page, rows, out totalCount);
                if ((expenseDetails == null || expenseDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    expenseDetails = dal.CustomerDetailRecord(searchText, page, rows, out totalCount);
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

        [HttpGet]
        public ActionResult GetCustomerFormDetails(int? Customerid)
        {
            try
            {
                AddCustomerModel model = new AddCustomerModel();
                SemDAL Dal = new SemDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                ViewBag.AsciiKey = Session["SecurityKey"].ToString();
                string employeeCode = Membership.GetUser().UserName;

                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                ViewBag.loginUserId = employeeId;
                v_tbl_PM_Customer CustDetails = new v_tbl_PM_Customer();

                CustDetails = Dal.GetCustomerDetails(Customerid);
                if (CustDetails != null)
                {
                    model.CurrentCountryListSEM = Dal.GetTravelCountryDetails();
                    model.Countrynames = CustDetails.Country;
                    model.CutomerIds = CustDetails.Customer;
                    ViewBag.CustomerId = CustDetails.Customer;
                    model.ExtMaktSegName = CustDetails.BusinessType;
                    model.AbbreviatedName = CustDetails.CustomerID;
                    model.RegionName = CustDetails.RegionID;
                    model.CustomerName = CustDetails.CustomerName;
                    model.ContractSigningDate = CustDetails.DateSigned;
                    model.ContractValidityDate = CustDetails.ContractValidityDate;
                    model.Address = CustDetails.Address;
                    model.PhoneNumber = CustDetails.MobileNumber;
                    model.AlternatePhoneNumber = CustDetails.Phone;
                    model.State = CustDetails.State;
                    model.City = CustDetails.City;
                    model.FaxNumber = CustDetails.FaxNumber;
                    model.ZipCode = CustDetails.PinCode;
                    model.EmailAddress = CustDetails.EmailID;
                    model.CreditPeriod = CustDetails.CreditPeriod;
                }

                //CountryDetailsListSEM objDefault = new CountryDetailsListSEM();
                //objDefault.CountryId = 0;
                //objDefault.CountryName = "Select";
                //model.CurrentCountryListSEM.Insert(0, objDefault);

                model.CurrentCountryListSEM = Dal.GetTravelCountryDetails();
                model.RegionTypeList = Dal.GetRegionTypeList();
                ViewBag.CountryLists = Dal.GetTravelCountryDetails();
                ViewBag.TypeOfContacts = Dal.GetTypeOfContactList();
                model.ExtMaktSegList = Dal.GetExtMktSegmentList();
                model.CustomerContract = new CustomerContract();
                model.CustomerAddress = new CustomerAddress();
                model.CustomerContact = new CustomerContact();
                TravelMailTemplate CustMail = new TravelMailTemplate();
                model.Mail = new TravelMailTemplate();

                return View("AddCustomer", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SaveCustomerDetails(AddCustomerModel model)
        {
            try
            {
                bool uploadStatus = false;
                string resultMessage = string.Empty;
                bool status = false;
                Tuple<bool, int, bool> statusVal;
                SemDAL dal = new SemDAL();
                statusVal = dal.SaveCustomerDetail(model);
                status = statusVal.Item1;
                ViewBag.CustomerId = statusVal.Item2;
                int? Customerid = statusVal.Item2;
                bool isAbbreviatedNameExist = statusVal.Item3;

                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status, CutomerId = Customerid, AbbreviatedNameExist = isAbbreviatedNameExist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InvoiceApprovals()
        {
            InvoiceApprovalsModel model = new InvoiceApprovalsModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult InvoiceFinance()
        {
            InvoiceFinanceModel model = new InvoiceFinanceModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveInvoiceAddressDetails(AddCustomerAddress model, string CustomerId, int CountryID, int SameAddess)
        {
            try
            {
                string resultMessage = string.Empty;
                bool status = false;

                SemDAL dal = new SemDAL();
                int CustInvoiceIdEmp = Convert.ToInt32(CustomerId);
                bool SameAdd = Convert.ToBoolean(SameAddess);
                status = dal.SaveInvoiceAddressDetail(model, CustInvoiceIdEmp, CountryID, SameAdd);

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
        public ActionResult LoadAddressInvoiceGrid(string CustomerID, int page, int rows)
        {
            try
            {
                AddCustomerAddress model = new AddCustomerAddress();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<AddCustomerAddress> expenseDetails = new List<AddCustomerAddress>();
                expenseDetails = dal.AddreessInvoiceDetailRecord(Convert.ToInt32(CustomerID), page, rows, out totalCount);

                if ((expenseDetails == null || expenseDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    expenseDetails = dal.AddreessInvoiceDetailRecord(Convert.ToInt32(CustomerID), page, rows, out totalCount);
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

        [HttpPost]
        public ActionResult LoadCustomerContactGrid(string CustomerID, int page, int rows)
        {
            try
            {
                CustomerContact model = new CustomerContact();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<CustomerContact> expenseDetails = new List<CustomerContact>();
                expenseDetails = dal.CustomerContactsRecord(Convert.ToInt32(CustomerID), page, rows, out totalCount);

                if ((expenseDetails == null || expenseDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    expenseDetails = dal.CustomerContactsRecord(Convert.ToInt32(CustomerID), page, rows, out totalCount);
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

        [HttpPost]
        public ActionResult SaveCustomerContactsDetails(CustomerContact model, string CustomerId)
        {
            try
            {
                string resultMessage = string.Empty;
                bool status = false;

                SemDAL dal = new SemDAL();
                int CustInvoiceIdEmp = Convert.ToInt32(CustomerId);
                status = dal.SaveCustomeContactDetail(model, CustInvoiceIdEmp);

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
        public ActionResult PMSProjectDetailsLoadGrid(string encryptedEmployeeId, int ddlApprovalStatusID, int ddlProjectlStatusID, string searchText, int page, int rows)
        {
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                PMSProjectDetailsViewModel model = new PMSProjectDetailsViewModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                string[] LogeedInEmRoles = { };
                string employeeCode = Membership.GetUser().UserName;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                int loggedInUserID = employeeId;
                string loggedInUserEmployeeCode = employeeCode;
                bool IsProjectApprover = false;
                string role = string.Empty;
                LogeedInEmRoles = Roles.GetRolesForUser(employeeCode);
                if (LogeedInEmRoles.Contains("Project_Approver"))
                    IsProjectApprover = true;
                if (LogeedInEmRoles.Contains("RMG"))
                    role = "RMG";
                if (LogeedInEmRoles.Contains("PMO"))
                    role = "PMO";

                List<PMSProjectDetailsViewModel> projectDetails = new List<PMSProjectDetailsViewModel>();
                projectDetails = dal.ProjectDetailRecord(ddlApprovalStatusID, ddlProjectlStatusID, searchText, loggedInUserEmployeeCode, IsProjectApprover, role, page, rows, out totalCount);
                if ((projectDetails == null || projectDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    projectDetails = dal.ProjectDetailRecord(ddlApprovalStatusID, ddlProjectlStatusID, searchText, loggedInUserEmployeeCode, IsProjectApprover, role, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = projectDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult ProjectReviewerDetailsLoadGrid(int ProjectID, int page, int rows)
        {
            try
            {
                PMSProjectDetailsViewModel model = new PMSProjectDetailsViewModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<PMSProjectDetailsViewModel> projectDetails = new List<PMSProjectDetailsViewModel>();
                projectDetails = dal.ProjectReviewerDetailsRecord(ProjectID, page, rows, out totalCount);
                if ((projectDetails == null || projectDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    projectDetails = dal.ProjectReviewerDetailsRecord(ProjectID, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = projectDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult GetReviewerRole(int EmployeeId)
        {
            try
            {
                SemDAL dal = new SemDAL();
                string Role = dal.GetRoleByEmployeeID(EmployeeId);
                return Json(new { results = Role }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult DeleteProjectReviewerDetails(string[] SelectedProjectReviewerId)
        {
            try
            {
                SemDAL dal = new SemDAL();
                bool status = false;
                status = dal.DeleteProjectReviewerDetails(SelectedProjectReviewerId);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpGet]
        public ActionResult ApprooveRevisionProjectDetails(int? ProjectID)
        {
            try
            {
                PMSProjectDetailsViewModel model = new PMSProjectDetailsViewModel();
                model.ProjectID = ProjectID;
                SemDAL dal = new SemDAL();
                List<RevisionList> fieldList = dal.GetApprooveRevisionProjectDetails(model.ProjectID);

                if (fieldList.Count > 0)
                {
                    foreach (var item in fieldList)
                    {
                        if (item.FeildName == "End Date")
                        {
                            model.IsEndDateChanged = true;
                            //item.OldValue = Convert.ToDateTime(item.OldValue).ToShortDateString();
                            if (item.OldValue == "" || item.OldValue == null)
                                item.OldValue = "";
                            else
                                item.OldValue = Convert.ToDateTime(item.OldValue).ToShortDateString();
                            item.NewValue = Convert.ToDateTime(item.NewValue).ToShortDateString();
                        }
                        if (item.FeildName == "Work(Hours)")
                            model.IsWorkHourChanged = true;
                    }
                }
                model.RevisionFeilds = fieldList;
                List<PMSProjectDetailsViewModel> ManagerRevisionComment = dal.getManagerRevisionComment(model.ProjectID);
                if (ManagerRevisionComment.Count > 0)
                {
                    foreach (var item in ManagerRevisionComment)
                    {
                        model.ManagerRevisionComment = item.ManagerRevisionComment.Replace("<br>", Environment.NewLine);
                    }
                }
                EmployeeDAL employeeDAL = new EmployeeDAL();
                ViewBag.loginUserId = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                return PartialView("_ApproveRevision", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult SaveProjectReviewersDetails(PMSProjectDetailsViewModel model, string ProjectID, int EmployeeId, string RoleDescription, DateTime ToDate, int CurrentId)
        {
            try
            {
                string resultMessage = string.Empty;
                bool status = false;

                SemDAL dal = new SemDAL();
                model.EmployeeId = EmployeeId;
                model.RoleDescription = RoleDescription;
                model.RoleId = dal.getRoleIdByDesription(RoleDescription);
                model.ProjectID = Convert.ToInt32(ProjectID);
                if (model.EmployeeId == CurrentId)
                {
                    return Json(new { results = "Saved", status = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    status = dal.SaveProjectReviewersDetail(model, ToDate);

                    if (status)
                        resultMessage = "Saved";
                    else
                        resultMessage = "Error";

                    return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SavePMSProjectDetailsForRevision(PMSProjectDetailsViewModel model)
        {
            try
            {
                bool uploadStatus = false;
                string resultMessage = string.Empty;
                bool status = false;
                Tuple<bool, int?, bool> statusVal;
                SemDAL dal = new SemDAL();

                List<PMSProjectDetailsViewModel> PMSProjectDetailList = dal.GetPMSProjectDetailForApproval(model.ProjectID.HasValue ? model.ProjectID.Value : 0);

                PMSProjectDetailsViewModel PMSProjectDetails = new PMSProjectDetailsViewModel();

                foreach (var item in PMSProjectDetailList)
                {
                    PMSProjectDetails.PMSProjectWorkHours = item.PMSProjectWorkHours;
                    PMSProjectDetails.PMSProjectEndDate = item.PMSProjectEndDate;

                    PMSProjectDetails.ApprovalStaus = item.ApprovalStaus;
                    PMSProjectDetails.PMSCommercialDetailsType = item.PMSCommercialDetailsType;
                    PMSProjectDetails.PMSBusinessGroup = item.PMSBusinessGroup;
                    PMSProjectDetails.PMSOrganizationUnit = item.PMSOrganizationUnit;
                    PMSProjectDetails.PMSDeliveryUnit = item.PMSDeliveryUnit;
                    PMSProjectDetails.PMSDeliveryTeam = item.PMSDeliveryTeam;
                    PMSProjectDetails.PMSProjectGroup = item.PMSProjectGroup;
                    PMSProjectDetails.PMSProjectStatusID = item.PMSProjectStatusID;
                    PMSProjectDetails.PMSProjectCurrency = item.PMSProjectCurrency;
                    PMSProjectDetails.PMSPractice = item.PMSPractice;
                    PMSProjectDetails.PMSProjectDurationDays = item.PMSProjectDurationDays;
                    PMSProjectDetails.PMSCustomer = item.PMSCustomer;
                    PMSProjectDetails.PMSLifeCycle = item.PMSLifeCycle;
                    PMSProjectDetails.PMSProjectBillableStatus = item.PMSProjectBillableStatus;
                    PMSProjectDetails.ApprovalStatusID = item.ApprovalStatusID;
                }
                bool statusValForRevision;
                var IsHoursChanges = false;
                if (PMSProjectDetails.PMSProjectWorkHours != model.PMSProjectWorkHours)
                {
                    if ((PMSProjectDetails.PMSProjectWorkHours == null || PMSProjectDetails.PMSProjectWorkHours == "")
                    && model.PMSProjectWorkHours == null || model.PMSProjectWorkHours == "")
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        IsHoursChanges = true;
                        model.PMSProjectWorkHours = PMSProjectDetails.PMSProjectWorkHours;
                    }
                }

                var IsEndDateChanges = false;
                if (PMSProjectDetails.PMSProjectEndDate != model.PMSProjectEndDate)
                {
                    if ((PMSProjectDetails.PMSProjectEndDate == null || PMSProjectDetails.PMSProjectEndDate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                        && model.PMSProjectEndDate == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        IsEndDateChanges = true;
                        model.PMSProjectEndDate = PMSProjectDetails.PMSProjectEndDate;
                    }
                }

                //for all fields those gor changed
                if (PMSProjectDetails.PMSCommercialDetailsType != model.PMSCommercialDetailsType)
                {
                    if ((PMSProjectDetails.PMSCommercialDetailsType == null)
                    && model.PMSCommercialDetailsType == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        model.FeildName = "Commercial Details";
                        model.PMSCommercialDetailsTypeList = dal.GetPMSCommercialDetailsTypeList();
                        var oldValue = model.PMSCommercialDetailsTypeList.Where(x => x.PMSCommercialDetailsTypeID == PMSProjectDetails.PMSCommercialDetailsType).FirstOrDefault();
                        var newValue = model.PMSCommercialDetailsTypeList.Where(x => x.PMSCommercialDetailsTypeID == model.PMSCommercialDetailsType).FirstOrDefault();
                        if (oldValue != null)
                            model.OldValue = Convert.ToString(oldValue.PMSCommercialDetailsType);
                        model.NewValue = Convert.ToString(newValue.PMSCommercialDetailsType);
                        model.RevisionStaus = 0;
                        statusValForRevision = dal.savePMSProjectDetailsForApproval(model);
                    }
                }

                if (PMSProjectDetails.PMSBusinessGroup != model.PMSBusinessGroup)
                {
                    if ((PMSProjectDetails.PMSBusinessGroup == null)
                    && model.PMSBusinessGroup == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        model.FeildName = "Business Group";
                        model.PMSBusinessGroupList = dal.GetPMSBusinessGroupList();
                        var oldValue = model.PMSBusinessGroupList.Where(x => x.PMSBusinessGroupID == PMSProjectDetails.PMSBusinessGroup).FirstOrDefault();
                        var newValue = model.PMSBusinessGroupList.Where(x => x.PMSBusinessGroupID == model.PMSBusinessGroup).FirstOrDefault();
                        if (oldValue != null)
                            model.OldValue = Convert.ToString(oldValue.PMSBusinessGroup);
                        model.NewValue = Convert.ToString(newValue.PMSBusinessGroup);
                        model.RevisionStaus = 0;
                        statusValForRevision = dal.savePMSProjectDetailsForApproval(model);
                    }
                }

                if (PMSProjectDetails.PMSOrganizationUnit != model.PMSOrganizationUnit)
                {
                    if ((PMSProjectDetails.PMSOrganizationUnit == null)
                    && model.PMSOrganizationUnit == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        model.FeildName = "Organization Unit";
                        model.PMSOrganizationUnitList = dal.GetPMSOrganizationUnitList();
                        var oldValue = model.PMSOrganizationUnitList.Where(x => x.PMSOrganizationUnitID == PMSProjectDetails.PMSOrganizationUnit).FirstOrDefault();
                        var newValue = model.PMSOrganizationUnitList.Where(x => x.PMSOrganizationUnitID == model.PMSOrganizationUnit).FirstOrDefault();
                        if (oldValue != null)
                            model.OldValue = Convert.ToString(oldValue.PMSOrganizationUnit);
                        model.NewValue = Convert.ToString(newValue.PMSOrganizationUnit);
                        model.RevisionStaus = 0;
                        statusValForRevision = dal.savePMSProjectDetailsForApproval(model);
                    }
                }

                if (PMSProjectDetails.PMSDeliveryUnit != model.PMSDeliveryUnit)
                {
                    if ((PMSProjectDetails.PMSDeliveryUnit == null)
                    && model.PMSDeliveryUnit == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        model.FeildName = "Delivery Unit";
                        model.PMSDeliveryUnitList = dal.GetPMSDeliveryUnitList();
                        var oldValue = model.PMSDeliveryUnitList.Where(x => x.PMSDeliveryUnitID == PMSProjectDetails.PMSDeliveryUnit).FirstOrDefault();
                        var newValue = model.PMSDeliveryUnitList.Where(x => x.PMSDeliveryUnitID == model.PMSDeliveryUnit).FirstOrDefault();
                        if (oldValue != null)
                            model.OldValue = Convert.ToString(oldValue.PMSDeliveryUnit);
                        model.NewValue = Convert.ToString(newValue.PMSDeliveryUnit);
                        model.RevisionStaus = 0;
                        statusValForRevision = dal.savePMSProjectDetailsForApproval(model);
                    }
                }

                if (PMSProjectDetails.PMSDeliveryTeam != model.PMSDeliveryTeam)
                {
                    if ((PMSProjectDetails.PMSDeliveryTeam == null)
                    && model.PMSDeliveryTeam == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        model.FeildName = "Delivery Team";
                        model.PMSDeliveryTeamList = dal.GetPMSDeliveryTeamList();
                        var oldValue = model.PMSDeliveryTeamList.Where(x => x.PMSDeliveryTeamID == PMSProjectDetails.PMSDeliveryTeam).FirstOrDefault();
                        var newValue = model.PMSDeliveryTeamList.Where(x => x.PMSDeliveryTeamID == model.PMSDeliveryTeam).FirstOrDefault();
                        if (oldValue != null)
                            model.OldValue = Convert.ToString(oldValue.PMSDeliveryTeam);
                        model.NewValue = Convert.ToString(newValue.PMSDeliveryTeam);
                        model.RevisionStaus = 0;
                        statusValForRevision = dal.savePMSProjectDetailsForApproval(model);
                    }
                }

                if (PMSProjectDetails.PMSProjectGroup != model.PMSProjectGroup)
                {
                    if ((PMSProjectDetails.PMSProjectGroup == null)
                    && model.PMSProjectGroup == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        model.FeildName = "Project Group";
                        model.PMSProjectGroupList = dal.GetPMSProjectGroupList();
                        var oldValue = model.PMSProjectGroupList.Where(x => x.PMSProjectGroupID == PMSProjectDetails.PMSProjectGroup).FirstOrDefault();
                        var newValue = model.PMSProjectGroupList.Where(x => x.PMSProjectGroupID == model.PMSProjectGroup).FirstOrDefault();
                        if (oldValue != null)
                            model.OldValue = Convert.ToString(oldValue.PMSProjectGroup);
                        model.NewValue = Convert.ToString(newValue.PMSProjectGroup);
                        model.RevisionStaus = 0;
                        statusValForRevision = dal.savePMSProjectDetailsForApproval(model);
                    }
                }

                if (Convert.ToString(PMSProjectDetails.PMSProjectStatusID) != model.PMSProjectStatus)
                {
                    if ((PMSProjectDetails.PMSProjectStatusID == null)
                    && model.PMSProjectStatus == null || model.PMSProjectStatus == "")
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        model.FeildName = "Project Status";
                        model.PMSProjectStatusList = dal.GetPMSProjectStatusList();
                        int newStatusId = Convert.ToInt32(model.PMSProjectStatus);
                        var oldValue = model.PMSProjectStatusList.Where(x => x.PMSProjectStatusID == PMSProjectDetails.PMSProjectStatusID).FirstOrDefault();
                        var newValue = model.PMSProjectStatusList.Where(x => x.PMSProjectStatusID == newStatusId).FirstOrDefault();
                        if (oldValue != null)
                            model.OldValue = Convert.ToString(oldValue.PMSProjectStatus);
                        model.NewValue = Convert.ToString(newValue.PMSProjectStatus);
                        model.RevisionStaus = 0;
                        statusValForRevision = dal.savePMSProjectDetailsForApproval(model);
                    }
                }

                if (PMSProjectDetails.PMSProjectCurrency != model.PMSProjectCurrency)
                {
                    if ((PMSProjectDetails.PMSProjectCurrency == null)
                    && model.PMSProjectCurrency == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        model.FeildName = "Project Currency";
                        model.PMSProjectCurrencyList = dal.GetPMSProjectCurrencyList();
                        var oldValue = model.PMSProjectCurrencyList.Where(x => x.PMSProjectCurrencyID == PMSProjectDetails.PMSProjectCurrency).FirstOrDefault();
                        var newValue = model.PMSProjectCurrencyList.Where(x => x.PMSProjectCurrencyID == model.PMSProjectCurrency).FirstOrDefault();
                        if (oldValue != null)
                            model.OldValue = Convert.ToString(oldValue.PMSProjectCurrency);
                        model.NewValue = Convert.ToString(newValue.PMSProjectCurrency);
                        model.RevisionStaus = 0;
                        statusValForRevision = dal.savePMSProjectDetailsForApproval(model);
                    }
                }

                if (PMSProjectDetails.PMSPractice != model.PMSPractice)
                {
                    if ((PMSProjectDetails.PMSPractice == null)
                    && model.PMSPractice == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        model.FeildName = "Practice";
                        model.PMSPracticeList = dal.GetPMSPracticeList();
                        var oldValue = model.PMSPracticeList.Where(x => x.PMSPracticeID == PMSProjectDetails.PMSPractice).FirstOrDefault();
                        var newValue = model.PMSPracticeList.Where(x => x.PMSPracticeID == model.PMSPractice).FirstOrDefault();
                        if (oldValue != null)
                            model.OldValue = Convert.ToString(oldValue.PMSPractice);
                        model.NewValue = Convert.ToString(newValue.PMSPractice);
                        model.RevisionStaus = 0;
                        statusValForRevision = dal.savePMSProjectDetailsForApproval(model);
                    }
                }

                if (PMSProjectDetails.PMSLifeCycle != model.PMSLifeCycle)
                {
                    if ((PMSProjectDetails.PMSLifeCycle == null)
                    && model.PMSLifeCycle == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        model.FeildName = "Life Cycle";
                        model.PMSLifeCycleList = dal.GetPMSLifeCycleList();
                        var oldValue = model.PMSLifeCycleList.Where(x => x.PMSLifeCycleID == PMSProjectDetails.PMSLifeCycle).FirstOrDefault();
                        var newValue = model.PMSLifeCycleList.Where(x => x.PMSLifeCycleID == model.PMSLifeCycle).FirstOrDefault();
                        if (oldValue != null)
                            model.OldValue = Convert.ToString(oldValue.PMSLifeCycle);
                        model.NewValue = Convert.ToString(newValue.PMSLifeCycle);
                        model.RevisionStaus = 0;
                        statusValForRevision = dal.savePMSProjectDetailsForApproval(model);
                    }
                }

                if (PMSProjectDetails.PMSProjectBillableStatus != model.PMSProjectBillableStatus)
                {
                    if ((PMSProjectDetails.PMSProjectBillableStatus == null)
                    && model.PMSProjectBillableStatus == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        model.FeildName = "Billable";
                        model.OldValue = PMSProjectDetails.PMSProjectBillableStatus == true ? "true" : "false";
                        model.NewValue = model.PMSProjectBillableStatus == true ? "true" : "false";
                        model.RevisionStaus = 0;
                        statusValForRevision = dal.savePMSProjectDetailsForApproval(model);
                    }
                }
                statusVal = dal.SavePMSProjectDetailForRevision(model, PMSProjectDetails.ApprovalStatusID);
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                status = statusVal.Item1;
                ViewBag.ProjectID = Commondal.Encrypt(Convert.ToString(statusVal.Item2), true);
                int? ProjectID = statusVal.Item2;
                bool IsProjectNameExist = statusVal.Item3;

                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status, ProjectID = ViewBag.ProjectID, IsHoursChanges = IsHoursChanges, IsEndDateChanges = IsEndDateChanges, ProjectNameExist = IsProjectNameExist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetRevisionDetails(RevisionDetailsModel RevisionDetailsModel)
        {
            try
            {
                PMSProjectDetailsViewModel PMSProjectDetails = new PMSProjectDetailsViewModel();
                PMSProjectDetails.PMSProjectEndDate = RevisionDetailsModel.EndDate;
                PMSProjectDetails.PMSProjectWorkHours = RevisionDetailsModel.WorkHours;
                PMSProjectDetails.ProjectID = RevisionDetailsModel.ProjectID;
                EmployeeDAL employeeDAL = new EmployeeDAL();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                ViewBag.loginUserId = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                ViewBag.encryptedProjectID = Commondal.Encrypt(Convert.ToString(RevisionDetailsModel.ProjectID), true);
                return PartialView("_RevisionDetails", PMSProjectDetails);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ApproveRejectRevisionApproval(int? ProjectID, string btnClick, string ApprovalComment, int EmployeeId, bool IsEndDateChanged, bool IsWorkHourChanged)
        {
            try
            {
                string resultMessage = string.Empty;
                bool status = false;
                bool resetStatus = false;
                Tuple<bool, bool> savedStatus;
                SemDAL dal = new SemDAL();
                int? ProjID = ProjectID;
                savedStatus = dal.saveApproveDetailsIntrail(ProjID, btnClick, ApprovalComment, EmployeeId, IsEndDateChanged, IsWorkHourChanged);
                status = savedStatus.Item1;
                resetStatus = savedStatus.Item2;
                if (status == true)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";

                return Json(new { results = resultMessage, status = status, resetStatus = resetStatus }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SendForRevisionApproval(RevisionDetailsModel RevisionDetailsModel)
        {
            try
            {
                SemDAL dal = new SemDAL();
                List<PMSProjectDetailsViewModel> PMSProjectDetailList = dal.GetPMSProjectDetailForApproval(RevisionDetailsModel.ProjectID.HasValue ? RevisionDetailsModel.ProjectID.Value : 0);

                PMSProjectDetailsViewModel PMSProjectDetails = new PMSProjectDetailsViewModel();

                foreach (var item in PMSProjectDetailList)
                {
                    PMSProjectDetails.PMSProjectWorkHours = item.PMSProjectWorkHours;
                    PMSProjectDetails.PMSProjectEndDate = item.PMSProjectEndDate;
                }
                PMSProjectDetailsViewModel model = new PMSProjectDetailsViewModel();

                bool statusValForRevision;
                model.ProjectID = RevisionDetailsModel.ProjectID;
                model.QuestionOne = RevisionDetailsModel.QuestionOne;
                model.QuestionTwo = RevisionDetailsModel.QuestionTwo;
                model.QuestionThree = RevisionDetailsModel.QuestionThree;
                model.QuestionFour = RevisionDetailsModel.QuestionFour;
                model.QuestionFive = RevisionDetailsModel.QuestionFive;

                if (PMSProjectDetails.PMSProjectWorkHours != RevisionDetailsModel.WorkHours)
                {
                    if ((PMSProjectDetails.PMSProjectWorkHours == null || PMSProjectDetails.PMSProjectWorkHours == "")
                    && RevisionDetailsModel.WorkHours == null || RevisionDetailsModel.WorkHours == "")
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        model.FeildName = "Work(Hours)";
                        model.OldValue = PMSProjectDetails.PMSProjectWorkHours;
                        model.NewValue = RevisionDetailsModel.WorkHours;
                        model.RevisionStaus = 4;
                        statusValForRevision = dal.savePMSProjectDetailsForApproval(model);
                    }
                }
                if (PMSProjectDetails.PMSProjectEndDate != RevisionDetailsModel.EndDate)
                {
                    if ((PMSProjectDetails.PMSProjectEndDate == null || PMSProjectDetails.PMSProjectEndDate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                        && RevisionDetailsModel.EndDate == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        model.FeildName = "End Date";
                        model.OldValue = PMSProjectDetails.PMSProjectEndDate.ToString();
                        model.NewValue = RevisionDetailsModel.EndDate.ToString();
                        model.RevisionStaus = 4;
                        statusValForRevision = dal.savePMSProjectDetailsForApproval(model);
                    }
                }
                string stringTobeSaved = RevisionQuestions.QuestionOne + "<br>" + model.QuestionOne + "<br><br>" + RevisionQuestions.QuestionTwo + "<br>" + model.QuestionTwo + "<br><br>" + RevisionQuestions.QuestionThree + "<br>" + model.QuestionThree + "<br><br>" + RevisionQuestions.QuestionFour + "<br>" + model.QuestionFour + "<br><br>" + RevisionQuestions.QuestionFive + "<br>" + model.QuestionFive;
                EmployeeDAL employeeDal = new EmployeeDAL();
                int employeeId = employeeDal.GetEmployeeID(Membership.GetUser().UserName);
                SearchedUserDetails employeedetails = dal.GetEmployeeDetails(employeeId);
                string sentBy = string.Empty;
                if (employeedetails != null)
                    sentBy = employeedetails.UserName;
                bool statusForRevision = dal.SaveQuestionsDetailsForRevision(model.ProjectID, stringTobeSaved, sentBy);
                model.PMSProjectWorkHours = RevisionDetailsModel.WorkHours;
                model.PMSProjectEndDate = RevisionDetailsModel.EndDate;
                model.RevisionStaus = 4;
                bool status = dal.SaveUpdatedRevisionValues(model);

                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SavePMSProjectDetails(PMSProjectDetailsViewModel model)
        {
            try
            {
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                bool uploadStatus = false;
                string resultMessage = string.Empty;
                bool status = false;
                bool resetStatus = false;
                Tuple<bool, int?, bool, bool> statusVal;
                SemDAL dal = new SemDAL();
                statusVal = dal.SavePMSProjectDetail(model);
                status = statusVal.Item1;
                ViewBag.ProjectID = statusVal.Item2;
                resetStatus = statusVal.Item4;
                string ProjectID = Commondal.Encrypt(Convert.ToString(statusVal.Item2), true);
                bool IsProjectNameExist = statusVal.Item3;

                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status, ProjectID = ProjectID, ProjectNameExist = IsProjectNameExist, DecryptedProjectID = statusVal.Item2, resetStatus = resetStatus }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteCustomerDetails(int CustomerID)
        {
            try
            {
                SemDAL dal = new SemDAL();
                bool status = dal.DeleteCustomerDetails(CustomerID);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteCustomerContactDetails(int CustomerID)
        {
            try
            {
                SemDAL dal = new SemDAL();
                bool status = dal.DeleteCustomerConDetails(CustomerID);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CustomerSendMail(string successEmpIDs, string CustomerId)
        {
            try
            {
                TravelViewModel model = new TravelViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                SemDAL dal = new SemDAL();

                model.Mail = new TravelMailTemplate();
                HRMS_tbl_PM_Employee fromEmployeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(successEmpIDs));
                string SendMailToPerson1 = System.Configuration.ConfigurationManager.AppSettings["CustomerSendMailToPerson1"];
                string SendMailCCPerson1 = System.Configuration.ConfigurationManager.AppSettings["CustomerSendMailCCPerson1"];
                string SendMailCCPerson2 = System.Configuration.ConfigurationManager.AppSettings["CustomerSendMailCCPerson2"];
                CustomerContract contractDetails = dal.GetLatestContractDetails(Convert.ToInt32(CustomerId));
                if (fromEmployeeDetails != null)
                {
                    model.Mail.From = fromEmployeeDetails.EmailID;
                    model.Mail.To = SendMailToPerson1;
                    model.Mail.Cc = fromEmployeeDetails.EmailID + ";" + SendMailCCPerson1 + ";" + SendMailCCPerson2;

                    int templateId = 55;
                    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                    foreach (var emailTemplate in template)
                    {
                        model.Mail.Subject = emailTemplate.Subject;
                        model.Mail.Subject = model.Mail.Subject.Replace("##<Customer name>##", contractDetails.CustomerName);
                        model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromEmployeeDetails.EmployeeName);

                        model.Mail.Message = model.Mail.Message.Replace("##<Customer name>##", contractDetails.CustomerName);
                        model.Mail.Message = model.Mail.Message.Replace("##<Contract Type>##", contractDetails.ContractTypeName);
                        model.Mail.Message = model.Mail.Message.Replace("##<Contract Signing Date>##", contractDetails.ContractSigningDate.Value.ToShortDateString());
                        model.Mail.Message = model.Mail.Message.Replace("##<Contract Validity Date>##", contractDetails.ContractValidityDate.Value.ToShortDateString());
                    }
                }

                return PartialView("_CustomerCreationEmail", model.Mail);
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
                    if (model.To != "" && model.To != null)
                    {
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
                        ToCounter = 0;
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
        public ActionResult SendForApprovalPMSProjectDetails(PMSProjectDetailsViewModel model)
        {
            try
            {
                bool uploadStatus = false;
                string resultMessage = string.Empty;
                bool status = false;
                Tuple<bool, int?, bool, bool> savedStatus;
                bool approvalStatus = false;
                bool resetStatus = false;
                SemDAL dal = new SemDAL();
                EmployeeDAL empdal = new EmployeeDAL();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                int LoggedInEmployeeId = empdal.GetEmployeeID(Membership.GetUser().UserName);
                savedStatus = dal.SavePMSProjectDetail(model);
                status = savedStatus.Item1;
                ViewBag.ProjectID = Commondal.Encrypt(Convert.ToString(savedStatus.Item2), true);
                int? ProjectID = savedStatus.Item2;
                resetStatus = savedStatus.Item4;
                if (status)
                {
                    approvalStatus = dal.SendForApprovalPMSProjectDetails(Convert.ToInt16(ProjectID), LoggedInEmployeeId);
                    if (approvalStatus)
                        resultMessage = "Saved";
                    else
                        resultMessage = "Error";
                }
                else
                    resultMessage = "Error";

                return Json(new { Results = resultMessage, saveStatus = status, approveStatus = approvalStatus, ProjectID = ViewBag.ProjectID, DecryptedProjectID = ProjectID, resetStatus = resetStatus }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ApproveRejectPMSProjectDetails(int? projectId, string btnClick, string RejectedComments)
        {
            try
            {
                bool uploadStatus = false;
                string resultMessage = string.Empty;
                bool status = false;
                EmployeeDAL empdal = new EmployeeDAL();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                int LoggedInEmployeeId = empdal.GetEmployeeID(Membership.GetUser().UserName);
                HRMS_tbl_PM_Employee empDetails = empdal.GetEmployeeDetails(LoggedInEmployeeId);
                SemDAL dal = new SemDAL();
                status = dal.ApproveRejectPMSProjectDetail(Convert.ToInt16(projectId), btnClick, empDetails.UserName, LoggedInEmployeeId, RejectedComments);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                string encryptedProjectID = Commondal.Encrypt(Convert.ToString(projectId), true);
                return Json(new { results = resultMessage, status = status, ProjectID = projectId, encryptedProjectID = encryptedProjectID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [PageAccess(PageName = "Manage Documents")]
        public ActionResult ManageDocuments()
        {
            ManageDocumentsModel model = new ManageDocumentsModel();
            SemDAL dal = new SemDAL();
            EmployeeDAL empdal = new EmployeeDAL();
            //HRMS_tbl_PM_Employee empDetails= empdal.GetEmployeeDetails(Membership.GetUser().UserName);
            string loginName = System.Web.HttpContext.Current.User.Identity.Name;
            HRMS_tbl_PM_Employee empDetails = empdal.GetEmployeeDetailsByEmployeeCode(loginName);
            model.SearchedUserDetails = new SearchedUserDetails();
            model.DocumentFileDetailsModel = new DocumentFileDetails();
            model.DocumentCategoryDetailsList = new List<DocumentCategoryDetails>();
            model.DocumentFileDetailsModel.DocumentCategoryDetailsList = dal.GetCategoryList();
            model.ProjectDetailsList = new List<ProjectDetails>();
            model.ProjectDetailsList = dal.GetProjectAllocationDetails(loginName);
            model.DocumentFileDetailsModel.UploadedBy = loginName;
            model.DocumentFileDetailsModel.EmployeeName = empDetails.EmployeeName;
            model.DocumentFileDetailsModel.UploadedOn = DateTime.Now.Date.ToShortDateString();
            //List<DocumentSubCategoryDetails> subcategory = new List<DocumentSubCategoryDetails>();
            model.DocumentFileDetailsModel.DocumentSubCategoryDetailsList = new List<DocumentSubCategoryDetails>();
            model.ProjectId = Convert.ToString(Session["ProjectId"]);
            return View(model);
        }

        #region CustomerContractDetails

        [HttpGet]
        public ActionResult GetCustomerContractFormDetails(int? CustomerID, int ContractID)
        {
            try
            {
                CustomerContract model = new CustomerContract();
                CustomerContract ContractDetails = new CustomerContract();
                SemDAL Dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                SearchedUserDetails employeeDetails = Dal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
                v_tbl_PM_Customer customerDetails = Dal.GetCustomerDetails(CustomerID);
                ContractDetails = Dal.GetCustomerContractDetails(ContractID);
                if (ContractDetails != null)
                    model = ContractDetails;
                model.ContractTypeList = Dal.GetContractTypeList();
                model.UserName = employeeDetails.UserName;
                if (customerDetails != null)
                {
                    model.CustomerName = customerDetails.CustomerName;
                    model.CustomerID = customerDetails.Customer;
                    model.CustomerContractSigningDate = customerDetails.DateSigned;
                    model.CustomerContractValidityDate = customerDetails.ContractValidityDate;
                    if (ContractID == 0)
                    {
                        model.ContractSigningDate = customerDetails.DateSigned;
                        model.ContractValidityDate = customerDetails.ContractValidityDate;
                        ViewBag.ContractValdate = customerDetails.ContractValidityDate;
                    }
                }
                model.ContractFileDetailsModel = new ContractFileDetails();
                model.ContractFileDetailsModel.EmployeeName = employeeDetails.EmployeeFullName;
                ViewBag.ContractID = ContractID;

                return PartialView("_AddContract", model);
            }
            catch (Exception)
            {
                throw;
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult CustomerContractFileLoadGrid(string encryptedEmployeeId, int ContractID, int page, int rows)
        {
            try
            {
                ContractFileDetails model = new ContractFileDetails();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<ContractFileDetails> contractFileDetails = new List<ContractFileDetails>();
                contractFileDetails = dal.CustomerContractFileRecord(ContractID, page, rows, out totalCount);
                if ((contractFileDetails == null || contractFileDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    contractFileDetails = dal.CustomerContractFileRecord(ContractID, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = contractFileDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult SaveCustomerContractFormDetails(CustomerContract model)
        {
            try
            {
                SemDAL dal = new SemDAL();
                SEMResponse response = new SEMResponse();
                if (model.ContractID != null)
                    response = dal.SaveCustomerContractDetails(model);
                return Json(new { status = response.status, newcontractid = response.nextContractID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CustomerContractDetailLoadGrid(int? CustomerID, int page, int rows)
        {
            try
            {
                CustomerContract model = new CustomerContract();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<CustomerContract> contractDetails = new List<CustomerContract>();
                contractDetails = dal.CustomerContractDetailRecord(CustomerID, page, rows, out totalCount);
                if ((contractDetails == null || contractDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    contractDetails = dal.CustomerContractDetailRecord(CustomerID, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = contractDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult DeleteCustomerContractDetails(int ContractID)
        {
            try
            {
                SemDAL dal = new SemDAL();
                bool status = false;
                if (ContractID != 0)
                    status = dal.DeleteCustomerContractRecord(ContractID);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteCustomerContractFileDetails(int ContractAttachmentID)
        {
            try
            {
                SemDAL dal = new SemDAL();
                bool status = false;
                if (ContractAttachmentID != 0)
                    status = dal.DeleteCustomerContractFileRecord(ContractAttachmentID);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadContractFileDetails(HttpPostedFileBase doc, ContractFileDetails model)
        {
            try
            {
                SemDAL semDAL = new SemDAL();
                bool result = false;
                if (doc != null && doc.ContentLength > 0)
                {
                    string uploadsPath = (UploadContractLocation);
                    uploadsPath = Path.Combine(uploadsPath, (model.ContractID).ToString());
                    string fileName = Path.GetFileName(doc.FileName);
                    model.FileName = fileName;
                    model.FilePath = uploadsPath;
                    result = semDAL.SaveContractFileDetails(model);

                    string filePath = Path.Combine(uploadsPath, fileName);
                    if (!Directory.Exists(uploadsPath))
                        Directory.CreateDirectory(uploadsPath);

                    doc.SaveAs(filePath);
                }
                else if (model.ContractAttachmentID > 0)
                    result = semDAL.SaveContractFileDetails(model);

                return Json(new { status = result }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadContractFile(int ContractID, int ContractAttachmentID, string FileName)
        {
            try
            {
                string[] FileExtention = FileName.Split('.');
                string contentType = "application/" + FileExtention[1];
                string uploadsPath = (UploadContractLocation);
                uploadsPath = Path.Combine(uploadsPath, (ContractID).ToString());
                string Filepath = Path.Combine(uploadsPath, FileName);
                return File(Filepath, contentType, FileName);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        #endregion CustomerContractDetails

        [HttpPost]
        public ActionResult ManagePhaseLoadGrid(string encryptedEmployeeId, int projectID, string searchText, int page, int rows)
        {
            try
            {
                SEMViewModel model = new SEMViewModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<ManagePhasesModel> expenseDetails = new List<ManagePhasesModel>();
                expenseDetails = dal.GetManagePhaseSelectedProJectDetails(Convert.ToInt32(projectID), searchText, page, rows, out totalCount);
                if ((expenseDetails == null || expenseDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    expenseDetails = dal.GetManagePhaseSelectedProJectDetails(Convert.ToInt32(projectID), searchText, page, rows, out totalCount);
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

        public ActionResult PhasesView(int? ProjectPhaseId, int? PhaseID)
        {
            PhasesViewModel model = new PhasesViewModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            ManagePhasesModel expenseDetails = new ManagePhasesModel();
            SemDAL dal = new SemDAL();

            if (ProjectPhaseId != 0)
            {
                expenseDetails = dal.GetManagePhaseProJectDetails(Convert.ToInt32(PhaseID), Convert.ToInt32(ProjectPhaseId));
                ViewBag.IBManagePhasesList = 0;
            }
            else
            {
                model.IBManagePhasesList = dal.IBManagesPhaseList();
                ViewBag.IBManagePhasesList = model.IBManagePhasesList.Count;
            }
            ViewBag.ProjectPhaseId = ProjectPhaseId;
            ViewBag.ProjectIds = PhaseID;
            ViewBag.Dates = dal.IBManagesPhaseDateList(ViewBag.ProjectIds);
            ViewBag.OrderNumbers = dal.IBManagesPhaseOrderNumersList(ViewBag.ProjectIds);

            if (expenseDetails != null)
            {
                //model.Phases = expenseDetails.Phases;
                model.Phases = expenseDetails.Phases;
                model.ProjectPhaseId = expenseDetails.ProjectPhaseId;
                model.ProjectId = expenseDetails.ProjectId;
                model.OrderNumber = expenseDetails.OrderNumber;
                model.StartDate = expenseDetails.StartDate;
                model.EndDate = expenseDetails.EndDate;
                model.WorkHours = expenseDetails.WorkHours;
                model.PeakTeamSize = expenseDetails.PeakTeamSize;
                model.ResponsiblePerId = expenseDetails.ResponsiblePerson;
                model.Currentphase = expenseDetails.Currentphase;
                model.PercentageEfforts = Convert.ToSingle(expenseDetails.PercentageEfforts);
            }
            model.ResposiblePersoneList = dal.ResponsiblrPersonsList(Convert.ToInt32(PhaseID), Convert.ToInt32(ProjectPhaseId));

            return View(model);
        }

        [HttpPost]
        public ActionResult ProjectSendMail(int loggedinEmpID, int projectId, string btnClick)
        {
            try
            {
                TravelViewModel model = new TravelViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                SemDAL dal = new SemDAL();
                model.Mail = new TravelMailTemplate();
                // string[] projectApprovers = Roles.GetUsersInRole("Project_Approver");
                //HRMS_tbl_PM_Employee fromEmployeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(successEmpIDs));
                HRMS_tbl_PM_Employee fromAndCCEmployeeDetails = employeeDAL.GetEmployeeDetails(loggedinEmpID);
                tbl_PM_Project projectDeatils = dal.GetProjectDetails(Convert.ToInt32(projectId));
                CustomerDetails custDetails = new CustomerDetails();
                if (projectDeatils != null)
                    custDetails = dal.GetCustomerDetailsForProjectMail(Convert.ToInt16(projectDeatils.CustomerID));
                if (fromAndCCEmployeeDetails != null)
                {
                    model.Mail.From = fromAndCCEmployeeDetails.EmailID;
                    model.Mail.Cc = fromAndCCEmployeeDetails.EmailID;
                    int templateId = 0;
                    string[] projectApprovers = Roles.GetUsersInRole("Project_Approver");
                    foreach (string user in projectApprovers)
                    {
                        HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(user));
                        if (employee == null)
                            model.Mail.To = model.Mail.To + string.Empty;
                        else
                            model.Mail.To = model.Mail.To + employee.EmailID + ";";
                    }

                    var CommersialList = dal.GetPMSCommercialDetailsTypeList();
                    if (btnClick == "SendForApproval")
                    {
                        templateId = 56;
                        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                        foreach (var emailTemplate in template)
                        {
                            model.Mail.Subject = emailTemplate.Subject;
                            model.Mail.Subject = model.Mail.Subject.Replace("##project name##", projectDeatils.ProjectName);
                            model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                            model.Mail.Message = model.Mail.Message.Replace("##start date##", projectDeatils.ActualStartDate.Value.ToShortDateString());
                            model.Mail.Message = model.Mail.Message.Replace("##project name##", projectDeatils.ProjectName);
                            model.Mail.Message = model.Mail.Message.Replace("##end date##", projectDeatils.ActualEndDate.Value.ToShortDateString());
                            model.Mail.Message = model.Mail.Message.Replace("##commercial details##", (from m in CommersialList where projectDeatils.ContractType == m.PMSCommercialDetailsTypeID select m.PMSCommercialDetailsType).FirstOrDefault());
                            model.Mail.Message = model.Mail.Message.Replace("##work hours##", Convert.ToString(projectDeatils.EstimatedEfforts));
                            model.Mail.Message = model.Mail.Message.Replace("##duration##", Convert.ToString(projectDeatils.ExpectedDuration));
                            model.Mail.Message = model.Mail.Message.Replace("##customer##", Convert.ToString(custDetails.CustomerName));
                            model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromAndCCEmployeeDetails.EmployeeName);
                        }
                    }

                    if (btnClick == "Rivision Approval")
                    {
                        templateId = 59;
                        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                        foreach (var emailTemplate in template)
                        {
                            model.Mail.Subject = emailTemplate.Subject;
                            model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                            model.Mail.Message = model.Mail.Message.Replace("##project name##", projectDeatils.ProjectName);
                            model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromAndCCEmployeeDetails.EmployeeName);
                        }
                    }
                    if (btnClick == "Approve Rivision Approval")
                    {
                        templateId = 60;
                        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                        int RevisionStatusID = 5;
                        tbl_PM_AuditTrail TrailDetails = dal.getAudioTrailDetails(Convert.ToInt32(projectId), RevisionStatusID);
                        string ApprovalComment = null;
                        if (TrailDetails != null)
                            ApprovalComment = TrailDetails.Comments;
                        foreach (var emailTemplate in template)
                        {
                            model.Mail.Subject = emailTemplate.Subject;
                            model.Mail.Subject = model.Mail.Subject.Replace("##project name##", projectDeatils.ProjectName);
                            model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                            model.Mail.Message = model.Mail.Message.Replace("##project name##", projectDeatils.ProjectName);
                            model.Mail.Message = model.Mail.Message.Replace("##approver decision comments##", ApprovalComment);
                            model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromAndCCEmployeeDetails.EmployeeName);
                        }
                    }
                    if (btnClick == "Reject Rivision Approval")
                    {
                        templateId = 61;
                        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                        int RevisionStatusID = 6;
                        tbl_PM_AuditTrail TrailDetails = dal.getAudioTrailDetails(Convert.ToInt32(projectId), RevisionStatusID);
                        string ApprovalComment = null;
                        if (TrailDetails != null)
                            ApprovalComment = TrailDetails.Comments;
                        foreach (var emailTemplate in template)
                        {
                            model.Mail.Subject = emailTemplate.Subject;
                            model.Mail.Subject = model.Mail.Subject.Replace("##project name##", projectDeatils.ProjectName);
                            model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                            model.Mail.Message = model.Mail.Message.Replace("##project name##", projectDeatils.ProjectName);
                            model.Mail.Message = model.Mail.Message.Replace("##approver decision comments##", ApprovalComment);
                            model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromAndCCEmployeeDetails.EmployeeName);
                        }
                    }
                    if (btnClick != "Rivision Approval" && btnClick != "SendForApproval")
                    {
                        HRMS_tbl_PM_Employee employeeApprove = dal.GetEmployeeDetailsByUserName(projectDeatils.CreatedBy);
                        if (employeeApprove != null)
                            model.Mail.To = employeeApprove.EmailID;
                    }
                    if (btnClick == "Approve")
                    {
                        templateId = 57;
                        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                        foreach (var emailTemplate in template)
                        {
                            model.Mail.Subject = emailTemplate.Subject;
                            model.Mail.Subject = model.Mail.Subject.Replace("##project name##", projectDeatils.ProjectName);
                            model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                            model.Mail.Message = model.Mail.Message.Replace("##project name##", projectDeatils.ProjectName);
                            model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromAndCCEmployeeDetails.EmployeeName);
                        }
                    }
                    if (btnClick == "Reject")
                    {
                        templateId = 58;
                        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                        foreach (var emailTemplate in template)
                        {
                            model.Mail.Subject = emailTemplate.Subject;
                            model.Mail.Subject = model.Mail.Subject.Replace("##project name##", projectDeatils.ProjectName);
                            model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                            model.Mail.Message = model.Mail.Message.Replace("##project name##", projectDeatils.ProjectName);
                            model.Mail.Message = model.Mail.Message.Replace("##rejection comments##", projectDeatils.RejectedComment);
                            model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromAndCCEmployeeDetails.EmployeeName);
                        }
                    }
                }

                return PartialView("_CustomerCreationEmail", model.Mail);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult PhaseManagement()
        {
            Session["SearchEmpFullName"] = null;  // to hide emp search
            Session["SearchEmpCode"] = null;
            Session["SearchEmpID"] = null;

            PhaseManagementModel model = new PhaseManagementModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult PhasesPracticeMapping()
        {
            SemDAL dal = new SemDAL();
            PhasesPracticeMappingModel model = new PhasesPracticeMappingModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            model.PracticeDetailsList = dal.GetPracticeList();
            model.PhasePracticeMapping = new List<PhasesPracticeMapping>();
            ViewBag.PracticeList = model.PracticeDetailsList;
            return View(model);
        }

        [HttpPost]
        public ActionResult GetBusinessDays(DateTime pmsStartDate, DateTime pmsEndDate)
        {
            try
            {
                SemDAL dal = new SemDAL();
                int days;
                int businessDays = dal.GetBusinessDays(pmsStartDate, pmsEndDate);

                return Json(new { days = businessDays }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        [HttpPost]
        public ActionResult ResourceDetailsLoadGrid(string ResourceDetailsLoadGrid, int PhaseID, int page, int rows)
        {
            try
            {
                SEMViewModel model = new SEMViewModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<PhasesViewModel> expenseDetails = new List<PhasesViewModel>();
                expenseDetails = dal.GetResourceDetailsProJectDetails(Convert.ToInt32(PhaseID), page, rows, out totalCount);
                if ((expenseDetails == null || expenseDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    expenseDetails = dal.GetResourceDetailsProJectDetails(Convert.ToInt32(PhaseID), page, rows, out totalCount);
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

        public ActionResult ApproveTimesheet()
        {
            ApproveTimesheetModel model = new ApproveTimesheetModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult CompleteTimesheet()
        {
            CompleteTimesheetModel model = new CompleteTimesheetModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        [HttpPost]
        public ActionResult LoadPhaseManagementData(int page, int rows)
        {
            try
            {
                PhaseManagementModel model = new PhaseManagementModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<PhaseManagementDetails> phasedescription = new List<PhaseManagementDetails>();
                phasedescription = dal.PhaseManagementDetails(page, rows, out totalCount);
                if ((phasedescription == null || phasedescription.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    phasedescription = dal.PhaseManagementDetails(page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = phasedescription
                };
                return Json(jsonData);
                //return View("PhaseManagement", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult DeletePhaseDescription(string[] PhaseID)
        {
            try
            {
                SemDAL dal = new SemDAL();
                //bool status = dal.DeletePhaseDescription(PhaseID);
                bool status = dal.DeletePhaseDescription(PhaseID);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ManageSubProjectLoadGrid(int? ProjectID, int page, int rows)
        {
            try
            {
                if (ProjectID == null)
                    ProjectID = 0;
                ManageSubProjectsModel model = new ManageSubProjectsModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                string employeeCode = Membership.GetUser().UserName;
                model.loggedInUserEmployeeCode = employeeCode;
                int totalCount;
                List<ManageSubProjectsModel> subProjectDetails = new List<ManageSubProjectsModel>();
                subProjectDetails = dal.LoadSubProjectGrid(ProjectID, page, rows, employeeCode, out totalCount);

                if ((subProjectDetails == null || subProjectDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    subProjectDetails = dal.LoadSubProjectGrid(ProjectID, page, rows, employeeCode, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = subProjectDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult SaveManageSubProjectDetails(ManageSubProjectsModel model, int? ProjectId, string ResponsiblePerson, string loggedInUserEmployeeCode)
        {
            try
            {
                int ProjectID = Convert.ToInt32(ProjectId);
                string resultMessage = string.Empty;
                bool status = false;
                bool isSubProjectNameExist;
                Tuple<bool, int, bool> statusVal;
                SemDAL dal = new SemDAL();
                statusVal = dal.SaveSubProjectDetails(model, ProjectID, ResponsiblePerson, loggedInUserEmployeeCode);
                status = statusVal.Item1;
                isSubProjectNameExist = statusVal.Item3;
                ViewBag.subProjectID = statusVal.Item2;

                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status, isSubProjectNameExist = isSubProjectNameExist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteManageSubProjectDetails(string[] SubProjectID)
        {
            try
            {
                SemDAL dal = new SemDAL();
                bool status = dal.DeleteSubProjectDetails(SubProjectID);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdatePhaseManagementData(PhaseManagementDetails model, string oldPhaseDescription)
        {
            try
            {
                string resultMessage = string.Empty;
                bool status = false;
                bool IsExists = false;
                Tuple<bool, bool> statusval;
                SemDAL dal = new SemDAL();
                statusval = dal.UpdatePhaseManagementData(model, oldPhaseDescription);
                status = statusval.Item1;
                IsExists = statusval.Item2;
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status, IsExists = IsExists }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveIBPhaseManageDetails(PhasesViewModel model)
        {
            try
            {
                //bool uploadStatus = false;
                string resultMessage = string.Empty;
                bool status = false;
                Tuple<bool, int, bool> statusVal;
                SemDAL dal = new SemDAL();
                statusVal = dal.SavePhaseIBMangDetail(model);
                status = statusVal.Item1;
                ViewBag.ProjectPhaseID = statusVal.Item2;
                int? ProjectPhaseID = statusVal.Item2;
                bool isPhaseExist = statusVal.Item3;
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status, ProjectPhaseID = ProjectPhaseID, isPhaseExist = isPhaseExist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteIBGridPhaseDetails(string[] SelectedPhaseIds, string ProjectID)
        {
            try
            {
                SemDAL dal = new SemDAL();
                bool status = dal.DeleteIDGridPhaseDetails(SelectedPhaseIds, Convert.ToInt32(ProjectID));
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ShowHistoryProjectLoadGrid(int projectId, int page, int rows)
        {
            try
            {
                SEMViewModel model = new SEMViewModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                string btnClick = "showHistory";
                List<PMSProjectDetailsViewModel> projectHistoryDetails = new List<PMSProjectDetailsViewModel>();
                projectHistoryDetails = dal.GetProjectHistoryDetails(Convert.ToInt32(projectId), btnClick, page, rows, out totalCount);
                if ((projectHistoryDetails == null || projectHistoryDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    projectHistoryDetails = dal.GetProjectHistoryDetails(Convert.ToInt32(projectId), btnClick, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = projectHistoryDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult ViewRevisionHistoryProjectLoadGrid(int projectId, int page, int rows)
        {
            try
            {
                SEMViewModel model = new SEMViewModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                string btnClick = "revisionHistory";
                List<PMSProjectDetailsViewModel> projectHistoryDetails = new List<PMSProjectDetailsViewModel>();
                projectHistoryDetails = dal.GetProjectHistoryDetails(Convert.ToInt32(projectId), btnClick, page, rows, out totalCount);
                if ((projectHistoryDetails == null || projectHistoryDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    projectHistoryDetails = dal.GetProjectHistoryDetails(Convert.ToInt32(projectId), btnClick, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = projectHistoryDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult ShowProjectApprovalLoadGrid(int projectId, int page, int rows)
        {
            try
            {
                SEMViewModel model = new SEMViewModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<PMSProjectDetailsViewModel> projectApprovalDetails = new List<PMSProjectDetailsViewModel>();
                projectApprovalDetails = dal.GetProjectApprovalDetails(Convert.ToInt32(projectId), page, rows, out totalCount);
                if ((projectApprovalDetails == null || projectApprovalDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    projectApprovalDetails = dal.GetProjectApprovalDetails(Convert.ToInt32(projectId), page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = projectApprovalDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult TimesheetEntryView()
        {
            TimesheetEntryViewModel model = new TimesheetEntryViewModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        #region Milestone

        [HttpGet]
        public ActionResult LoadMilestoneTaskForCompletion(int ProjectID, int MilestoneID)
        {
            try
            {
                //ProjectID = 390;
                //MilestoneID = 4892;
                ManageMilestonesModel model = new ManageMilestonesModel();
                SemDAL dal = new SemDAL();
                //List<TaskClosure> taskClosure = new List<TaskClosure>();
                model.TaskClosureComplitionList = new List<TaskClosureComplition>();
                model.TaskClosureComplitionList = dal.LoadTaskClosureComplition(ProjectID, MilestoneID);
                //model.TaskClosureList = taskClosure;

                model.TaskClosureVoidList = new List<TaskClosureVoid>();
                model.TaskClosureVoidList = dal.LoadTaskClosureVoid(ProjectID, MilestoneID);
                model.ProjectID = ProjectID;
                model.MilestoneID = MilestoneID;
                return PartialView("_TaskClosure", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        //[HttpGet]
        //public ActionResult LoadMilestoneTaskForVoid(int ProjectID, int MilestoneID)
        //{
        //    try
        //    {
        //        //ProjectID = 390;
        //        //MilestoneID = 4892;
        //        ManageMilestonesModel model = new ManageMilestonesModel();
        //        SemDAL dal = new SemDAL();
        //        model.TaskClosureVoidList = new List<TaskClosureVoid>();
        //        model.TaskClosureVoidList = dal.LoadTaskClosureVoid(ProjectID, MilestoneID);
        //        //model.TaskClosureList = taskClosure;

        //        return PartialView("_TaskClosure", model);
        //    }
        //    catch (Exception)
        //    {
        //        return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
        //    }
        //}

        public ActionResult DeleteMilestoneDetails(string[] MilestoneID)
        {
            try
            {
                SemDAL dal = new SemDAL();
                bool status = dal.DeleteMilestoneDetails(MilestoneID);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveMilestoneDetails(ManageMilestonesModel model, int ProjectID, string ResponsiblePerson, string MilestoneStatus)
        {
            try
            {
                bool uploadStatus = false;
                string resultMessage = string.Empty;
                bool status = false;
                bool isMilestoneNameExist;
                bool isValidMilestoneStatus = true;
                int? MilestoneID = 0;
                Tuple<bool, int?, bool, bool> statusVal;
                SemDAL dal = new SemDAL();
                statusVal = dal.SaveMilestoneDetails(model, ProjectID, ResponsiblePerson, MilestoneStatus);
                status = statusVal.Item1;
                MilestoneID = statusVal.Item2;
                isValidMilestoneStatus = statusVal.Item4;
                isMilestoneNameExist = statusVal.Item3;

                //ViewBag.MilestoneID = statusVal.;
                //var slist = statusVal.Item4;
                //model.TaskClosureComplitionList = slist;
                //ViewBag.TaskClosureComplitionList = slist;
                // int? Customerid = statusVal.Item2;
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status, MilestoneID, isMilestoneNameExist = isMilestoneNameExist, isValidMilestoneStatus = isValidMilestoneStatus }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [PageAccess(PageName = "Manage Milestones")]
        public ActionResult ManageMilestones()
        {
            ManageMilestonesModel model = new ManageMilestonesModel();
            SemDAL dal = new SemDAL();
            EmployeeDAL employeeDAL = new EmployeeDAL();
            //TaskClosure taskClosure = new TaskClosure();
            model.TaskClosureComplition = new TaskClosureComplition();
            model.TaskClosureVoid = new TaskClosureVoid();
            model.SearchedUserDetails = new SearchedUserDetails();

            ViewBag.AsciiKey = Session["SecurityKey"].ToString();
            string employeeCode = Membership.GetUser().UserName;
            int employeeId = employeeDAL.GetEmployeeID(employeeCode);
            // model.userId = employeeId;
            model.TaskClosureComplitionList = new List<TaskClosureComplition>();
            model.TaskClosureVoidList = new List<TaskClosureVoid>();

            model.SearchedUserDetails = new SearchedUserDetails();
            model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
            SearchedUserDetails employeeDetails = dal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
            model.ProjectList = dal.GetLoggedUserProjectList(employeeDetails.UserName, "ActiveProjects", Convert.ToInt32(employeeDetails.EmployeeCode));
            ViewBag.MilestoneStausList = dal.GetMilestoneStatusList();
            model.Prj = Convert.ToString(Session["ProjectId"]);
            //var approvalStatusID = 0;
            //model.ProjectList = dal.GetProjectNamesListByApprovalStatus(approvalStatusID);

            //ViewBag.EmployeeLists = employeeDAL.GetEmployeeList();
            //ViewBag.EmployeeLists = dal.GetEmployeeList();

            //if (Session["ProjectId"] != null)
            //    model.ProjectID = Convert.ToInt32(Session["ProjectId"]);

            return View(model);
        }

        [HttpPost]
        public ActionResult LoadMilestoneGrid(int? ProjectID, int page, int rows)
        {
            try
            {
                if (ProjectID == null)
                    ProjectID = 0;
                ManageMilestonesModel model = new ManageMilestonesModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<ManageMilestonesModel> milestoneDetails = new List<ManageMilestonesModel>();
                milestoneDetails = dal.LoadMilestoneGrid(ProjectID, page, rows, out totalCount);

                if ((milestoneDetails == null || milestoneDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    milestoneDetails = dal.LoadMilestoneGrid(ProjectID, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = milestoneDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult CloseTask(string[] TaskID, int Flag, int ProjectID, int? MilestoneID)
        {
            try
            {
                bool IsMilestoneClosed;
                bool status;
                SemDAL dal = new SemDAL();
                Tuple<bool, bool> statusVal;
                statusVal = dal.CloseTasks(TaskID, Flag, ProjectID, MilestoneID);

                status = statusVal.Item1;
                IsMilestoneClosed = statusVal.Item2;

                return Json(new { status = status, IsMilestoneClosed = IsMilestoneClosed }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Milestone

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadProjectDocumentFileDetails(HttpPostedFileBase doc, DocumentFileDetails model)
        {
            try
            {
                SemDAL semDAL = new SemDAL();
                bool result = false;
                if (doc.ContentLength > 0)
                {
                    string uploadsPath = (UploadManagerDocumentFileLocation);
                    //uploadsPath = Path.Combine(uploadsPath, (model.ProjectId).ToString());

                    tbl_PM_Project projectDetails = semDAL.GetProjectDetails(model.ProjectId);
                    string projectCode = null;
                    if (projectDetails != null)
                        projectCode = projectDetails.ProjectCode;
                    uploadsPath = Path.Combine(uploadsPath, projectCode);
                    string mode = "Category";
                    var directiveName = semDAL.GetDirectiveName(model.CategoryId, model.SubCategoryId, mode);
                    var pathtostore = Path.Combine(projectCode, directiveName.DirectoryNames);

                    uploadsPath = Path.Combine(uploadsPath, directiveName.DirectoryNames);
                    if (model.SubCategoryId != null)
                    {
                        mode = "SubCategory";
                        var directiveNameSubCategory = semDAL.GetDirectiveName(model.CategoryId, model.SubCategoryId, mode);
                        uploadsPath = Path.Combine(uploadsPath, directiveNameSubCategory.DirectoryNames);
                        pathtostore = Path.Combine(pathtostore, directiveNameSubCategory.DirectoryNames);
                    }

                    string fileName = Path.GetFileName(doc.FileName);
                    model.DocName = fileName;
                    model.DocPath = pathtostore;
                    float size = (float)doc.ContentLength;
                    model.FileSize = (float)((size) / 1024);
                    result = semDAL.SaveDocumentFileDetails(model);

                    string filePath = Path.Combine(uploadsPath, fileName);
                    if (!Directory.Exists(uploadsPath))
                        Directory.CreateDirectory(uploadsPath);

                    doc.SaveAs(filePath);
                }

                return Json(new { status = result }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, "", "SEMController.cs", "UploadProjectDocumentFileDetails :", ex.Message);
                return Json(new { status = "Error" }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetProjectDocumentLoadGrid(int? projectId, string searchtext, int? documentCategoryId, int page, int rows)
        {
            try
            {
                DocumentFileDetails model = new DocumentFileDetails();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount = 0;

                List<DocumentFileDetails> documentFileDetails = new List<DocumentFileDetails>();
                int projectID = Convert.ToInt32(projectId);
                if (projectID != 0)
                {
                    documentFileDetails = dal.ProjectDocumentFileRecord(projectID, searchtext, documentCategoryId, page, rows, out totalCount);
                    if ((documentFileDetails == null || documentFileDetails.Count <= 0) && page - 1 > 0)
                    {
                        page = page - 1;
                        documentFileDetails = dal.ProjectDocumentFileRecord(projectID, searchtext, documentCategoryId, page, rows, out totalCount);
                    }
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = documentFileDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public System.Threading.Timer myTimer;

        public void SetTimerValue()
        {

            // trigger the event at 9 AM. For 7 PM use 21 i.e. 24 hour format
            DateTime requiredTime = DateTime.Today.AddHours(10).AddMinutes(00);
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

            //sendmailforCustomerEnd();
            // now, call the set timer method to reset its next call time
            SetTimerValue();

        }

        private string GetADOConnectionString()
        {
            WSEMDBEntities ctx = new WSEMDBEntities(); //create your entity object here
            EntityConnection ec = (EntityConnection)ctx.Connection;
            SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
            string adoConnStr = sc.ConnectionString;
            return adoConnStr;
        }

        //public void sendmailforCustomerEnd()
        //{
        //    string constring = GetADOConnectionString();
        //    SqlConnection con = new SqlConnection(constring);
        //    string[] RMGs = { };
        //    string[] ProjectApprovers = { };
        //    RMGs = Roles.GetUsersInRole("RMG");
        //    ProjectApprovers = Roles.GetUsersInRole("Project_Approver");
        //    SqlDataAdapter ProjectApproversEmail = new SqlDataAdapter();
        //    DataSet dsProjectApproversEmail = new DataSet();
        //    foreach (var item in ProjectApprovers)
        //    {
        //        string EmployeeRecords = "Select EmailID from HRMS_tbl_PM_Employee where EmployeeCode = '" + item + "' AND Status = '" + 0 + "'";
        //        ProjectApproversEmail = new SqlDataAdapter(EmployeeRecords, con);
        //        ProjectApproversEmail.Fill(dsProjectApproversEmail);
        //    }
        //    List<string> ApproverEmaildsList = new List<string>();

        //    foreach (DataTable t in dsProjectApproversEmail.Tables)
        //    {
        //        foreach (DataRow row in t.Rows)
        //        {
        //            string EmailId = row["EmailID"].ToString();
        //            ApproverEmaildsList.Add(EmailId);
        //        }
        //    }

        //    SqlDataAdapter RMGsEmail = new SqlDataAdapter();
        //    DataSet dsRMGsEmail = new DataSet();
        //    foreach (var item in RMGs)
        //    {
        //        string EmployeeRecords = "Select EmailID from HRMS_tbl_PM_Employee where EmployeeCode = '" + item + "' AND Status = '" + 0 + "'";
        //        RMGsEmail = new SqlDataAdapter(EmployeeRecords, con);
        //        RMGsEmail.Fill(dsRMGsEmail);
        //    }
        //    List<string> RMGEmaildsList = new List<string>();

        //    foreach (DataTable t in dsRMGsEmail.Tables)
        //    {
        //        foreach (DataRow row in t.Rows)
        //        {
        //            string EmailId = row["EmailID"].ToString();
        //            RMGEmaildsList.Add(EmailId);
        //        }
        //    }
        //    DateTime today = DateTime.Today;
        //    DateTime sevenDaysbefore = today.AddDays(7);
        //    string records = "Select * from tbl_PM_Customer where ContractValidityDate = '" + sevenDaysbefore + "'";
        //    con.Open();
        //    SqlDataAdapter dataap = new SqlDataAdapter(records, con);
        //    DataSet cust = new DataSet();
        //    dataap.Fill(cust);
        //    var custvalues = new List<Tuple<int, string, string>>();
        //    foreach (DataTable t in cust.Tables)
        //    {
        //        foreach (DataRow row in t.Rows)
        //        {
        //            int CustomerID = Convert.ToInt32(row["Customer"]);
        //            string CustomerName = row["CustomerName"].ToString();
        //            string ContractValidityDate = string.Empty;
        //            if (row["ContractValidityDate"].ToString() != null)
        //            {
        //                ContractValidityDate = row["ContractValidityDate"].ToString();
        //            }
        //            custvalues.Add(new Tuple<int, string, string>(CustomerID, CustomerName, ContractValidityDate));
        //        }
        //    }
        //    for (int i = 0; i < custvalues.Count; i++)
        //    {
        //        TravelViewModel model = new TravelViewModel();
        //        CommonMethodsDAL Commondal = new CommonMethodsDAL();
        //        MailMessage mail = new MailMessage();
        //        SqlCommand cmd = new SqlCommand();
        //        EmployeeDAL employeeDAL = new EmployeeDAL();
        //        SemDAL dal = new SemDAL();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "GetProjectIRFinanceApprovers_SP";//write sp name here
        //        cmd.Connection = con;
        //        SqlDataAdapter fin = new SqlDataAdapter(cmd);
        //        DataSet FinanceApprover = new DataSet();
        //        fin.Fill(FinanceApprover);
        //        foreach (DataTable t in FinanceApprover.Tables)
        //        {
        //            foreach (DataRow row in t.Rows)
        //            {
        //                tbl_PM_Employee_SEM EmployeeDetailsemail = employeeDAL.GetEmployeeDetailsEmail(Convert.ToInt32(row["employeeid"]));
        //                HRMS_tbl_PM_Employee fromEmployeeDetailsEmail = employeeDAL.GetEmployeeDetailsByEmployeeCode(EmployeeDetailsemail.EmployeeCode);
        //                if (fromEmployeeDetailsEmail == null)
        //                {

        //                }
        //                else
        //                {
        //                    mail.To.Add(fromEmployeeDetailsEmail.EmailID);
        //                }

        //            }
        //        }

        //        SqlCommand cmd2 = new SqlCommand();
        //        cmd2.CommandType = CommandType.StoredProcedure;
        //        cmd2.CommandText = "GetCustomerwithPMemail";//write sp name here
        //        cmd2.Connection = con;
        //        cmd2.Parameters.Add("@customerId", Convert.ToInt32(custvalues[i].Item1));
        //        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
        //        DataSet dsPMMail = new DataSet();
        //        da2.Fill(dsPMMail);
        //        foreach (DataTable t in dsPMMail.Tables)
        //        {
        //            foreach (DataRow row in t.Rows)
        //            {
        //                tbl_PM_Employee_SEM EmployeeDetailsemail = employeeDAL.GetEmployeeDetailsEmail(Convert.ToInt32(row["EmployeeID"]));
        //                HRMS_tbl_PM_Employee fromEmployeeDetailsEmailPM = employeeDAL.GetEmployeeDetailsByEmployeeCode(EmployeeDetailsemail.EmployeeCode);
        //                if (fromEmployeeDetailsEmailPM == null)
        //                {

        //                }
        //                else
        //                {
        //                    mail.To.Add(fromEmployeeDetailsEmailPM.EmailID);
        //                }

        //            }
        //        }

        //        SqlCommand cmd3 = new SqlCommand();
        //        cmd3.CommandType = CommandType.StoredProcedure;
        //        cmd3.CommandText = "GetCustomerwithProjectIRapprovermail";//write sp name here
        //        cmd3.Connection = con;
        //        cmd3.Parameters.Add("@customerId", Convert.ToInt32(custvalues[i].Item1));
        //        SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
        //        DataSet dsProjectIRMail = new DataSet();
        //        da3.Fill(dsProjectIRMail);
        //        foreach (DataTable t in dsProjectIRMail.Tables)
        //        {
        //            foreach (DataRow row in t.Rows)
        //            {
        //                if (Convert.ToString(row["emailId"]) == null)
        //                {
        //                }
        //                else
        //                {
        //                    mail.To.Add(Convert.ToString(row["emailId"]));
        //                }
        //            }
        //        }
        //        foreach (var item in RMGEmaildsList)
        //        {
        //            mail.CC.Add(item);
        //        }
        //        string Email = string.Empty;
        //        foreach (var item in ApproverEmaildsList)
        //        {
        //            Email = item;
        //            // mail.CC.Add(item);
        //        }
        //        foreach (var item in RMGEmaildsList)
        //        {
        //            mail.CC.Add(item);
        //        }

        //        mail.From = new MailAddress(Email, "TestEmail");
        //        model.Mail = new TravelMailTemplate();
        //        int templateId = 97;
        //        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
        //        foreach (var emailTemplate in template)
        //        {
        //            model.Mail.Subject = emailTemplate.Subject;
        //            model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
        //            model.Mail.Message = model.Mail.Message.Replace("##project name##", custvalues[i].Item2);
        //            model.Mail.Message = model.Mail.Message.Replace("##project End Date##", (Convert.ToDateTime(custvalues[i].Item3).ToShortDateString()).ToString());
        //            model.Mail.Message = model.Mail.Message.Replace("##logged in user##", "RMG");
        //        }
        //        SmtpClient smtpClient = new SmtpClient();
        //        mail.Subject = model.Mail.Subject;
        //        mail.Body = model.Mail.Message;
        //        smtpClient.UseDefaultCredentials = false;
        //        smtpClient.EnableSsl = true;
        //        smtpClient.Host = System.Configuration.ConfigurationManager.AppSettings["SMTPServerName"].ToString();
        //        //smtpClient.Host = "v2mailserver.in.v2solutions.com";
        //        string UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"].ToString();
        //        string Password = System.Configuration.ConfigurationManager.AppSettings["Password"].ToString();
        //        smtpClient.Credentials = new System.Net.NetworkCredential(UserName, Password);
        //        smtpClient.Port = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["PortNumber"].ToString());
        //        smtpClient.Send(mail);
        //    }
        //    con.Close();

        //}

        public void sendMail()
        {
            string constring = GetADOConnectionString();
            SqlConnection con = new SqlConnection(constring);
            DateTime today = DateTime.Today;
            SqlCommand pcmd = new SqlCommand();
            pcmd.CommandType = CommandType.StoredProcedure;
            pcmd.CommandText = "GetProjectEndRecords";//write sp name here
            pcmd.Connection = con;
            SqlDataAdapter Pend = new SqlDataAdapter(pcmd);
            DataSet ds = new DataSet();
            Pend.Fill(ds);

            string[] ProjectApprovers = { };
            string[] RMGs = { };
            string[] Managers = { };
            ProjectApprovers = Roles.GetUsersInRole("Project_Approver");
            RMGs = Roles.GetUsersInRole("RMG");
            Managers = Roles.GetUsersInRole("Manager");

            SqlDataAdapter ProjectApproversEmail = new SqlDataAdapter();
            DataSet dsProjectApproversEmail = new DataSet();
            foreach (var item in ProjectApprovers)
            {
                string EmployeeRecords = "Select EmailID from HRMS_tbl_PM_Employee where EmployeeCode = '" + item + "' AND Status = '" + 0 + "'";
                ProjectApproversEmail = new SqlDataAdapter(EmployeeRecords, con);
                ProjectApproversEmail.Fill(dsProjectApproversEmail);
            }

            SqlDataAdapter RMGsEmail = new SqlDataAdapter();
            DataSet dsRMGsEmail = new DataSet();
            foreach (var item in RMGs)
            {
                string EmployeeRecords = "Select EmailID from HRMS_tbl_PM_Employee where EmployeeCode = '" + item + "' AND Status = '" + 0 + "'";
                RMGsEmail = new SqlDataAdapter(EmployeeRecords, con);
                RMGsEmail.Fill(dsRMGsEmail);
            }

            SqlDataAdapter ManagersEmail = new SqlDataAdapter();
            DataSet dsManagersEmail = new DataSet();
            var values = new List<Tuple<int, string, string>>();

            foreach (DataTable t in ds.Tables)
            {
                foreach (DataRow row in t.Rows)
                {
                    string projectIds = row["projectID"].ToString();
                    string ProjName = row["ProjectName"].ToString();
                    string EndDate = string.Empty;
                    if (row["ActualEndDate"].ToString() != null)
                    {
                        EndDate = row["ActualEndDate"].ToString();
                    }
                    int ProjectId = Convert.ToInt32(projectIds);
                    values.Add(new Tuple<int, string, string>(ProjectId, ProjName, EndDate));
                }
            }

            List<string> ApproverEmaildsList = new List<string>();

            foreach (DataTable t in dsProjectApproversEmail.Tables)
            {
                foreach (DataRow row in t.Rows)
                {
                    string EmailId = row["EmailID"].ToString();
                    ApproverEmaildsList.Add(EmailId);
                }
            }

            List<string> RMGEmaildsList = new List<string>();

            foreach (DataTable t in dsRMGsEmail.Tables)
            {
                foreach (DataRow row in t.Rows)
                {
                    string EmailId = row["EmailID"].ToString();
                    RMGEmaildsList.Add(EmailId);
                }
            }

            List<string> ManagerEmaildsList = new List<string>();

            foreach (DataTable t in dsManagersEmail.Tables)
            {
                foreach (DataRow row in t.Rows)
                {
                    string EmailId = row["EmailID"].ToString();
                    ManagerEmaildsList.Add(EmailId);
                }
            }

            for (int i = 0; i < values.Count; i++)
            {
                MailMessage mail = new MailMessage();
                foreach (var item in ManagerEmaildsList)
                {
                    // mail.To.Add(item);
                }
                string Email = string.Empty;
                foreach (var item in ApproverEmaildsList)
                {
                    Email = item;
                    mail.CC.Add(item);
                }
                foreach (var item in RMGEmaildsList)
                {
                    mail.CC.Add(item);
                }

                mail.From = new MailAddress(Email, "Project Approver");

                TravelViewModel model = new TravelViewModel();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                SemDAL dal = new SemDAL();
                List<PMSProjectDetailsViewModel> projectDetails = new List<PMSProjectDetailsViewModel>();
                projectDetails = dal.ProjectReviewerDetailsforEmail(values[i].Item1);
                if (projectDetails == null)
                {
                }
                foreach (var k in projectDetails)
                {
                    tbl_PM_Employee_SEM EmployeeDetailsemail = employeeDAL.GetEmployeeDetailsEmail(k.EmployeeId);
                    HRMS_tbl_PM_Employee fromEmployeeDetailsEmail = employeeDAL.GetEmployeeDetailsByEmployeeCode(EmployeeDetailsemail.EmployeeCode);
                    if (fromEmployeeDetailsEmail == null)
                    {

                    }
                    else
                    {
                        mail.To.Add(fromEmployeeDetailsEmail.EmailID);
                    }
                }

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetPM_DMforMailalert";//write sp name here
                cmd.Connection = con;
                cmd.Parameters.Add("@ProjectID", Convert.ToInt32(values[i].Item1));
                SqlDataAdapter da2 = new SqlDataAdapter(cmd);
                DataSet dsPMDMEMail = new DataSet();
                da2.Fill(dsPMDMEMail);
                foreach (DataTable t in dsPMDMEMail.Tables)
                {
                    foreach (DataRow row in t.Rows)
                    {
                        tbl_PM_Employee_SEM EmployeeDetailsemail = employeeDAL.GetEmployeeDetailsEmail(Convert.ToInt32(row["EmployeeID"]));
                        HRMS_tbl_PM_Employee fromEmployeeDetailsEmail = employeeDAL.GetEmployeeDetailsByEmployeeCode(EmployeeDetailsemail.EmployeeCode);
                        if (fromEmployeeDetailsEmail == null)
                        {

                        }
                        else
                        {
                            mail.To.Add(fromEmployeeDetailsEmail.EmailID);
                        }

                    }
                }
                model.Mail = new TravelMailTemplate();
                int templateId = 62;
                List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                foreach (var emailTemplate in template)
                {
                    model.Mail.Subject = emailTemplate.Subject;
                    model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                    model.Mail.Message = model.Mail.Message.Replace("##project name##", values[i].Item2);
                    model.Mail.Message = model.Mail.Message.Replace("##project End Date##", (Convert.ToDateTime(values[i].Item3).ToShortDateString()).ToString());
                    model.Mail.Message = model.Mail.Message.Replace("##logged in user##", "RMG");
                }
                SmtpClient smtpClient = new SmtpClient();
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
                mail.Dispose();
            }
            con.Close();
        }

        public ActionResult DeleteProjectDocumnetFileDetails(int DocumentAttachmentID)
        {
            try
            {
                SemDAL dal = new SemDAL();
                bool status = false;
                if (DocumentAttachmentID != 0)
                    status = dal.DeleteProjectDocumentFileRecord(DocumentAttachmentID);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CheckIfFileExist(int? ProjectId, int? DocumentAttachmentID)
        {
            try
            {
                SemDAL dal = new SemDAL();
                //string DocName = "";
                DirectoryName DocName = new DirectoryName();
                //List<DocumentFileDetails> documentFileDetails = new List<DocumentFileDetails>();
                DocName = dal.GetProjectDocumentsFileDetailsById(Convert.ToInt32(DocumentAttachmentID));

                //string[] FileExtention = DocName.Split('.');
                //string contentType = "application/" + FileExtention[1];
                string uploadsPath = (UploadManagerDocumentFileLocation);
                //uploadsPath = Path.Combine(uploadsPath, (ProjectId).ToString());
                string Filepath = Path.Combine(uploadsPath, DocName.DirectoryNames, DocName.FileName);
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

        public ActionResult DownloadDocumentFile(int ProjectId, int DocumentAttachmentID)
        {
            //string DocName = "";
            try
            {
                SemDAL dal = new SemDAL();
                DirectoryName DocName = new DirectoryName();
                //List<DocumentFileDetails> documentFileDetails = new List<DocumentFileDetails>();
                DocName = dal.GetProjectDocumentsFileDetailsById(DocumentAttachmentID);

                string[] FileExtention = DocName.FileName.Split('.');
                string contentType = "application/" + DocName.FileName[1];
                string uploadsPath = (UploadManagerDocumentFileLocation);
                //uploadsPath = Path.Combine(uploadsPath, (ProjectId).ToString());
                string Filepath = Path.Combine(uploadsPath, DocName.DirectoryNames, DocName.FileName);
                if (System.IO.File.Exists(Filepath))
                    return File(Filepath, contentType, DocName.FileName);
                else
                //return RedirectToAction("Index", "Error", new { errorCode = "File path doesnot exist." });
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult LoadPhasesPracticeMapping(int practiceID)
        {
            SemDAL dal = new SemDAL();
            PhasesPracticeMappingModel model = new PhasesPracticeMappingModel();
            model.PhasePracticeMapping = dal.LoadPhasesPracticeMapping(practiceID);
            return PartialView("_PhasePracticeMapping", model);
        }

        public ActionResult AddUpdateProjectTypePhaseData(string[] allFinalValues)
        {
            try
            {
                bool status = false;
                string resultMessage = string.Empty;
                if (allFinalValues != null)
                {
                    SemDAL dal = new SemDAL();
                    status = dal.AddUpdateProjectTypePhaseData(allFinalValues);
                    if (status)
                        resultMessage = "Saved";
                    else
                        resultMessage = "Error";
                }
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ProjectIRApproverDetailsLoadGrid(int ProjectID, int page, int rows)
        {
            try
            {
                PMSProjectDetailsViewModel model = new PMSProjectDetailsViewModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<PMSProjectDetailsViewModel> projectIRApproverDetails = new List<PMSProjectDetailsViewModel>();
                projectIRApproverDetails = dal.ProjectIRApproverDetailsRecord(ProjectID, page, rows, out totalCount);
                if ((projectIRApproverDetails == null || projectIRApproverDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    projectIRApproverDetails = dal.ProjectIRApproverDetailsRecord(ProjectID, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = projectIRApproverDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult SaveProjectIRApproverDetails(PMSProjectDetailsViewModel model, string ProjectID, int EmployeeId, string RoleDescription)
        {
            try
            {
                string resultMessage = string.Empty;
                bool status = false;

                SemDAL dal = new SemDAL();
                model.IRApproverEmployeeId = EmployeeId;
                model.IRApproverRoleDescription = RoleDescription;
                model.IRApproverRoleId = dal.getRoleIdByDesription(RoleDescription);
                model.IRApproverProjectID = Convert.ToInt32(ProjectID);
                status = dal.SaveProjectIRApproverDetail(model);

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

        public ActionResult DeleteProjectIRApproverDetails(string[] SelectedProjectIRApproverId)
        {
            try
            {
                SemDAL dal = new SemDAL();
                bool status = false;
                status = dal.DeleteProjectIRApproverDetails(SelectedProjectIRApproverId);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult ProjectIRFinanceApproverDetailsLoadGrid(int ProjectID, int page, int rows)
        {
            try
            {
                PMSProjectDetailsViewModel model = new PMSProjectDetailsViewModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<PMSProjectDetailsViewModel> projectIRFinanceApproverDetails = new List<PMSProjectDetailsViewModel>();
                projectIRFinanceApproverDetails = dal.projectIRFinanceApproverDetails(ProjectID, page, rows, out totalCount);
                if ((projectIRFinanceApproverDetails == null || projectIRFinanceApproverDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    projectIRFinanceApproverDetails = dal.projectIRFinanceApproverDetails(ProjectID, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = projectIRFinanceApproverDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult SaveProjectIRFinanceApproverDetails(PMSProjectDetailsViewModel model, string ProjectID, int EmployeeId, string RoleDescription)
        {
            try
            {
                string resultMessage = string.Empty;
                bool status = false;

                SemDAL dal = new SemDAL();
                model.IRFinanceApproverEmployeeId = EmployeeId;
                model.IRFinanceApproverRoleDescription = RoleDescription;
                model.IRFinanceApproverRoleId = dal.getRoleIdByDesription(RoleDescription);
                model.IRFinanceApproverProjectID = Convert.ToInt32(ProjectID);
                status = dal.SaveProjectIRFinanceApproverDetail(model);

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

        public ActionResult DeleteProjectIRFinanceApproverDetails(string[] SelectedProjectIRFinanceApproverId)
        {
            try
            {
                SemDAL dal = new SemDAL();
                bool status = false;
                status = dal.DeleteProjectIRFinanceApproverDetails(SelectedProjectIRFinanceApproverId);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult AllocateResourceWhileProjectApprove(PMSProjectDetailsViewModel model)
        {
            try
            {
                //bool uploadStatus = false;
                string resultMessage = string.Empty;
                //bool status = false;
                bool statusVal;
                SemDAL dal = new SemDAL();
                statusVal = dal.AllocatedResourceOnProject(model);

                if (statusVal)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = statusVal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetSelectedCustomerDetails(int CustomerID)
        {
            try
            {
                bool status = false;
                SemDAL dal = new SemDAL();
                v_tbl_PM_Customer customer = new v_tbl_PM_Customer();
                customer = dal.GetCustomerDetails(CustomerID);
                if (customer != null)
                    return Json(new { status = true, CustomerStartDate = customer.DateSigned, CustomerEndDate = customer.ContractValidityDate }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetProjectURL(string projectId, string approveStatus)
        {
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            ArrayList list = new ArrayList();
            list.Add(projectId);
            list.Add(approveStatus);

            ArrayList list1 = new ArrayList();
            foreach (string value in list)
            {
                list1.Add(Commondal.Encrypt(Convert.ToString(value), true));
            }
            return Json(list1, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSubCategory(string categoryId)
        {
            try
            {
                SemDAL dal = new SemDAL();
                if (categoryId == "")
                    categoryId = null;
                List<DocumentSubCategoryDetails> DocumentSubCategoryDetailsList = new List<DocumentSubCategoryDetails>();
                DocumentSubCategoryDetailsList = dal.GetSubCategory(Convert.ToInt32(categoryId));
                return Json(new { List = DocumentSubCategoryDetailsList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { List = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ProjectNameAutoSuggestForManageProject(string term)
        {
            SemDAL dal = new SemDAL();
            List<ProjectAppList> searchResult = new List<ProjectAppList>();
            int EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
            SearchedUserDetails employeeDetails = dal.GetEmployeeDetails(EmployeeId);
            searchResult = dal.ProjectNameForProject(term, Convert.ToInt32(employeeDetails.EmployeeCode), 1, 20);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSelectedProjectTasksForManageProject(int ProjectID)
        {
            try
            {
                PMSProjectDetailsViewModel model = new PMSProjectDetailsViewModel();
                SemDAL semDAL = new SemDAL();
                int EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                SearchedUserDetails employeeDetails = semDAL.GetEmployeeDetails(EmployeeId);
                int EmpCode = Convert.ToInt32(employeeDetails.EmployeeCode);
                string EmpId = semDAL.GetEmployeeIdFromEmployeeCodeSEM(EmpCode);
                int EmployeeIDSEM = Convert.ToInt32(EmpId);
                ViewBag.TaskList = semDAL.getTaskName(ProjectID, EmployeeIDSEM);
                return Json(new { ListData = ViewBag.TaskList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}