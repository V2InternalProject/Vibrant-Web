using HRMS.DAL;
using HRMS.Models;
using HRMS.Notification;
using MvcApplication3.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS.Controllers
{
    public class InvoiceController : Controller
    {
        //
        // GET: /Invoice/

        public ActionResult SalesPeriod()
        {
            try
            {
                SalesPeriodModel model = new SalesPeriodModel();
                SemDAL semDal = new SemDAL();
                InvoiceDAL invoiceDal = new InvoiceDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
                model.UserName = employeeDetails.UserName;

                model.SalesPeriodMonthLists = invoiceDal.GetSalesPeriodMonthList();
                model.SalesPeriodYearLists = invoiceDal.GetSalesPeriodYearList();
                model.SalesPeriodIsOpenLists = invoiceDal.GetSalesPeriodIsOpenList();

                return PartialView("_SalesPeriod", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult LoadSalesPeriodGrid(int page, int rows)
        {
            try
            {
                AddSalesPeriods model = new AddSalesPeriods();

                InvoiceDAL dal = new InvoiceDAL();
                int totalCount;
                List<AddSalesPeriods> Details = new List<AddSalesPeriods>();
                Details = dal.SalesPeriodDetailRecord(page, rows, out totalCount);

                if ((Details == null || Details.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    Details = dal.SalesPeriodDetailRecord(page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = Details,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        #region GenerateInvoice

        [PageAccess(PageName = "Generate Invoice")]
        public ActionResult GenerateInvoice(string TextLink)
        {
            try
            {
                Session["SearchEmpFullName"] = null;  // to hide emp search
                Session["SearchEmpCode"] = null;
                Session["SearchEmpID"] = null;

                GenerateInvoiceModel model = new GenerateInvoiceModel();
                InvoiceDAL invoiceDal = new InvoiceDAL();
                SemDAL semDal = new SemDAL();
                TaskTimesheetDAL taskDal = new TaskTimesheetDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
                model.IRGeneratorProjectList = semDal.GetLoggedUserProjectList(employeeDetails.UserName, TextLink, Convert.ToInt32(employeeDetails.EmployeeCode));
                model.UserName = employeeDetails.UserName;
                //model.UserName = "chetan.jain"; //hardcoded for testing
                model.TypeList = invoiceDal.GetInvoiceTypeList();
                model.StageList = invoiceDal.GetInvoiceStageFilterList();
                model.NameList = invoiceDal.GetInvoiceNameFilterList();

                int EmpCode = Convert.ToInt32(employeeDetails.EmployeeCode);
                string EmpID = taskDal.GetEmployeeIdFromEmployeeCodeSEM(EmpCode);

                ViewBag.IsIRApprover = invoiceDal.CheckLoggedUserIsIRApprover(Convert.ToInt32(EmpID));
                ViewBag.IsIRFinanceApprover = invoiceDal.CheckLoggedUserIsIRFinanceApprover(Convert.ToInt32(EmpID));
                ViewBag.TextLink = TextLink;
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult AddInvoice(int? RFIID, int? ProjectID, string textLink, string viewDetails)
        {
            try
            {
                AddInvoiceModel model = new AddInvoiceModel();
                InvoiceDAL dal = new InvoiceDAL();
                SemDAL semDal = new SemDAL();
                TaskTimesheetDAL taskDal = new TaskTimesheetDAL();
                model.Mail = new TravelMailTemplate();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.LstCheckList = new List<InvoiceCheckList>();
                // model.LstCheckList = dal.LoadInvoiceCheckList(RFIID);
                model.IsViewDetails = viewDetails;
                ViewBag.IsNewFrom = viewDetails;
                model.TypeList = dal.GetInvoiceTypeList();
                ViewBag.IsDiscount = dal.GetSalesPeriodIsOpenList();
                ViewBag.ResourcePoolType = dal.getResoucePoolNames();
                ViewBag.BillingType = dal.getBillingTypes();
                ViewBag.TypeOfIR = dal.getTypeOfIR();
                model.CustAddress = dal.GetCustomerNameForInvoice(ProjectID.HasValue ? ProjectID.Value : 0);
                model.CustomerName = model.CustAddress.CustomerName;
                model.CustomerID = model.CustAddress.CustomerID;
                model.CustomerContactPerson = dal.GetCustomerContactPerson(ProjectID.HasValue ? ProjectID.Value : 0);
                model.Currency = dal.GetCustomerProjectCurrency(ProjectID.HasValue ? ProjectID.Value : 0);
                model.CurrencyName = model.Currency.CurrencyName;
                model.CurrencyID = model.Currency.CuurencyID;
                model.IrPirID = dal.GetNextRFIID();
                model.ProjectID = (ProjectID.HasValue ? ProjectID.Value : 0);
                ViewBag.ProjectId = model.ProjectID;
                ViewBag.TextLink = textLink;
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                ViewBag.loginUserId = model.SearchedUserDetails.EmployeeId;
                SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
                model.RFIRaisedBy = employeeDetails.UserName;
                ViewBag.RFIID = RFIID;

                int EmpCode = Convert.ToInt32(employeeDetails.EmployeeCode);
                string EmpID = taskDal.GetEmployeeIdFromEmployeeCodeSEM(EmpCode);
                ViewBag.IsIRApprover = dal.CheckLoggedUserIsIRApprover(Convert.ToInt32(EmpID));
                ViewBag.IsIRFinanceApprover = dal.CheckLoggedUserIsIRFinanceApprover(Convert.ToInt32(EmpID));
                if (RFIID > 0)
                {
                    var rfidtls = dal.GetRFIIDDetails(RFIID.HasValue ? RFIID.Value : 0);
                    model.IrPirID = rfidtls.IrPirID;
                    model.IsProforma = rfidtls.IsProforma;
                    model.TypeID = rfidtls.TypeID;
                    model.CreditDays = rfidtls.CreditDays;
                    model.ContractID = rfidtls.ContractID;
                    model.SalesPeriodID = rfidtls.SalesPeriodID;
                    model.CustomerContactID = rfidtls.CustomerContactID;
                    model.CustomerEmail = rfidtls.ConfirmEmailID;
                    model.CurrentStatus = rfidtls.CurrentStatus;
                    Session["CurrentStatus"] = rfidtls.CurrentStatus;
                    if (model.IsProforma == true)
                        ViewBag.IsPIRRequest = "True";
                    else
                        ViewBag.IsPIRRequest = "False";
                    var invoiceDetails = dal.GetDetailsForInvoiceByRFIID(RFIID.HasValue ? RFIID.Value : 0);
                    model.CustomerAddress = invoiceDetails.CustomerAddress;
                    model.Contract = invoiceDetails.ContractSummary;
                    model.SalesPeriod = invoiceDetails.SalesPeriod;
                    model.CustomerAddressID = Convert.ToInt32(invoiceDetails.CustomerAddressID);
                    model.ContractID = Convert.ToInt32(invoiceDetails.ContractID);
                    model.SalesPeriodID = Convert.ToInt32(invoiceDetails.SalesPeriodID);
                }
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult SaveInvoiceDetails(string textLink, AddInvoiceModel model)
        {
            try
            {
                InvoiceDAL dal = new InvoiceDAL();
                string resultMessage = string.Empty;
                int Rfiid = dal.GetNextRFIID();
                if (Rfiid > model.IrPirID && model.IsViewDetails == "NewForm")
                {
                    resultMessage = "IDIncremented";
                    return Json(new { results = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var status = false;
                    var submitstatus = false;
                    var updstatus = false;
                    AddInvoiceResponse historyStatus = new AddInvoiceResponse();
                    var ApproverStatus = false;
                    if (model.ButtonClicked == "Cancelled")
                    {
                        submitstatus = dal.SubmitInvoiceDetails(model);
                        return Json(new { results = resultMessage, submitstatus = submitstatus, }, JsonRequestBehavior.AllowGet);
                    }

                    if (textLink == "IRApproval" || textLink == "FinanceApproval" || (textLink == "IRGenerator" && model.ButtonClicked == "Submit"))
                    {
                        if (textLink != "IRApproval" || textLink != "FinanceApproval")
                            updstatus = dal.SaveInvoiceDetails(model);
                        else
                            ApproverStatus = true;
                        if (model.IsProforma == true)
                            historyStatus = dal.SavePIRHistoryDetails(model.IrPirID, model.TypeID);
                        if (historyStatus.Status == false && model.IsProforma == true)
                            return Json(new { results = resultMessage, submitstatus = historyStatus.Status, }, JsonRequestBehavior.AllowGet);
                        submitstatus = dal.SubmitInvoiceDetails(model);
                        if (submitstatus)
                            resultMessage = "Saved";
                        else
                            resultMessage = "Error";
                        return Json(new { results = resultMessage, submitstatus = submitstatus, }, JsonRequestBehavior.AllowGet);
                    }
                    status = dal.SaveInvoiceDetails(model);
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

        [HttpGet]
        public ActionResult GetPIRHistoryDetails(int? RFIID, int? RFITypeID)
        {
            try
            {
                AddInvoiceModel model = new AddInvoiceModel();
                InvoiceDAL dal = new InvoiceDAL();
                model.HistoryIDList = dal.GetHistoryIdList(RFIID);
                model.PIRHistoryDetailsList = dal.GetPIRHistoryRecords(RFIID, RFITypeID);
                model.InvoiceTypeHeadersList = dal.GetInvoiceTypeHeaders(RFITypeID);
                return PartialView("_ShowPIRHistory", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpGet]
        public ActionResult LoadInvoiceCheckList(int? RFIID)
        {
            try
            {
                AddInvoiceModel model = new AddInvoiceModel();
                InvoiceDAL dal = new InvoiceDAL();
                model.LstCheckList = new List<InvoiceCheckList>();
                model.LstCheckList = dal.LoadInvoiceCheckList(RFIID);
                return PartialView("_CheckList", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpGet]
        public ActionResult GetCheckListDetails(int? ProjectID, int RFIID, string IsProforma, int? TypeID, string TextLink)
        {
            try
            {
                AddInvoiceModel model = new AddInvoiceModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                InvoiceDAL dal = new InvoiceDAL();
                model.LstCheckList = new List<InvoiceCheckList>();
                model.LstCheckList = dal.GetCheckListDetails();
                model.ProjectID = ProjectID.HasValue ? ProjectID.Value : 0;
                model.RFIID = RFIID;
                if (IsProforma == "true")
                    model.IsProforma = true;
                else
                    model.IsProforma = false;
                model.TypeID = TypeID.HasValue ? TypeID.Value : 0;
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                ViewBag.loginUserId = model.SearchedUserDetails.EmployeeId;
                ViewBag.TextLink = TextLink;
                return PartialView("_AddCheckListDetails", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult SaveCheckList(List<InvoiceCheckList> model)
        {
            try
            {
                InvoiceDAL dal = new InvoiceDAL();
                string resultMessage = string.Empty;
                var status = dal.SaveCheckListDetails(model);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult GetSelectedProjectDetails(int ProjectID)
        {
            try
            {
                bool isCurrencyExist = false;
                bool isIRApproverExist = false;
                bool isIRFinanceApproverExist = false;
                InvoiceDAL dal = new InvoiceDAL();
                InvoiceProjectDetails response = new InvoiceProjectDetails();
                response = dal.GetSelectedProjectRecord(ProjectID);
                if (response.ProjectCurrencyID == null)
                    isCurrencyExist = false;
                else
                    isCurrencyExist = true;
                if (response.IRApproverCount == 0)
                    isIRApproverExist = false;
                else
                    isIRApproverExist = true;
                if (response.IRFinanceApproverCount == 0)
                    isIRFinanceApproverExist = false;
                else
                    isIRFinanceApproverExist = true;
                return Json(new { isCurrencyExist = isCurrencyExist, isIRApproverExist = isIRApproverExist, isIRFinanceApproverExist = isIRFinanceApproverExist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult InvoiceInboxWatchListLoadGrid(string TextLink, int? ProjectID, string LoggedUserName, int? InvoiceTypeID, string InvoiceName, string InvoiceStage, string GridName, int page, int rows)
        {
            try
            {
                InvoiceDAL invoiceDal = new InvoiceDAL();
                int totalCount;
                List<InvoiceDetailsModel> invoiceDetails = new List<InvoiceDetailsModel>();
                invoiceDetails = invoiceDal.GetInvoiceInboxWatchListDetails(TextLink, ProjectID, LoggedUserName, InvoiceTypeID, InvoiceName, InvoiceStage, GridName, page, rows, out totalCount);
                if ((invoiceDetails == null || invoiceDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    invoiceDetails = invoiceDal.GetInvoiceInboxWatchListDetails(TextLink, ProjectID, LoggedUserName, InvoiceTypeID, InvoiceName, InvoiceStage, GridName, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = invoiceDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult FixBidLoadGrid(int RFIID, int page, int rows)
        {
            try
            {
                InvoiceDAL invoiceDal = new InvoiceDAL();
                int totalCount;
                List<InvoiceTypeItemModel> FixBidDetails = new List<InvoiceTypeItemModel>();
                FixBidDetails = invoiceDal.GetFixBidDetails(RFIID, page, rows, out totalCount);
                if ((FixBidDetails == null || FixBidDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    FixBidDetails = invoiceDal.GetFixBidDetails(RFIID, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = FixBidDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult SaveFixBidDetails(InvoiceTypeItemModel model, int? TypeID, int? RFIID, int? SelectedResourcepool)
        {
            try
            {
                string resultMessage = string.Empty;
                string resultMessageRate = string.Empty;
                bool status = false;
                InvoiceDAL dal = new InvoiceDAL();
                status = dal.SaveFixBidDetails(model, TypeID, RFIID, SelectedResourcepool);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ActionResult DeleteInvoiceTypeDetails(string[] SelectedRFIItemID)
        {
            try
            {
                InvoiceDAL dal = new InvoiceDAL();
                bool status = false;
                status = dal.DeleteInvoiceTypeDetails(SelectedRFIItemID);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult ShowStatusInvoice(int RFIID, int ProjectID, int page, int rows)
        {
            try
            {
                InvoiceDAL invoiceDal = new InvoiceDAL();

                int totalCount;
                List<InvoiceShowStatusModel> ShowStatusResult = new List<InvoiceShowStatusModel>();
                //RFIID = 2498; //passed hardcoded for testing
                //ProjectID = 390; //passed hardcoded for testing
                ShowStatusResult = invoiceDal.GetInvoiceShowStatusResult(page, rows, RFIID, ProjectID, out totalCount);

                if ((ShowStatusResult == null || ShowStatusResult.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    ShowStatusResult = invoiceDal.GetInvoiceShowStatusResult(page, rows, RFIID, ProjectID, out totalCount);
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

        #endregion GenerateInvoice

        [HttpPost]
        public ActionResult DeleteSalesPeriodDetails(string[] SelectedModuleId)
        {
            try
            {
                InvoiceDAL dal = new InvoiceDAL();
                bool status = dal.DeleteSalesPeriodRecord(SelectedModuleId);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveSalesPeriodDetails(AddSalesPeriods model, int SalesPeriodMonthID, int SalesPeriodYearID, int SalesPeriodIsOpenID, string LoggedUserName, string SalesPeriodStartDate, string SalesPeriodEndDate)
        {
            try
            {
                bool status = false;
                InvoiceDAL dal = new InvoiceDAL();
                SEMResponse response = new SEMResponse();
                response = dal.SaveSalesPeriodRecord(model, SalesPeriodMonthID, SalesPeriodYearID, SalesPeriodIsOpenID, LoggedUserName, SalesPeriodStartDate, SalesPeriodEndDate);

                return Json(new { status = response.status, isModuleNameExist = response.isModuleNameExist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetContractDetails(int? CustomerID)
        {
            try
            {
                ContractDetails model = new ContractDetails();
                InvoiceDAL dal = new InvoiceDAL();
                model.ContractTypeList = dal.getContractTypeList();
                model.CustomerID = CustomerID.HasValue ? CustomerID.Value : 0;
                return PartialView("_AddContractDetails", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpGet]
        public ActionResult GetSalesPeriodDetails()
        {
            try
            {
                SalesPeriod model = new SalesPeriod();
                InvoiceDAL dal = new InvoiceDAL();
                //model.cust
                return PartialView("_AddSalesPeriod", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpGet]
        public ActionResult GetCustomerAddressDetails(int? CustomerID)
        {
            try
            {
                InvoiceCustomerAddress model = new InvoiceCustomerAddress();
                InvoiceDAL dal = new InvoiceDAL();
                model.CustomerID = CustomerID.HasValue ? CustomerID.Value : 0;
                return PartialView("_InvoiceCustomerAddress", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult GetCustomerAddressLoadGrid(int? CustomerID, int page, int rows)
        {
            try
            {
                if (CustomerID == null)
                    CustomerID = 0;
                InvoiceCustomerAddress model = new InvoiceCustomerAddress();
                InvoiceDAL dal = new InvoiceDAL();
                int totalCount;
                List<InvoiceCustomerAddress> CustomerAddressDetails = new List<InvoiceCustomerAddress>();
                CustomerAddressDetails = dal.CustomerAddressDetails(CustomerID, page, rows, out totalCount);

                if ((CustomerAddressDetails == null || CustomerAddressDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    CustomerAddressDetails = dal.CustomerAddressDetails(CustomerID, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = CustomerAddressDetails
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult LoadContractGridGrid(int? CustomerID, int page, int rows, int? ContractTypeID)
        {
            try
            {
                if (CustomerID == null)
                    CustomerID = 0;
                ContractDetails model = new ContractDetails();
                InvoiceDAL dal = new InvoiceDAL();
                int totalCount;
                List<ContractDetails> ContractDetails = new List<ContractDetails>();
                ContractDetails = dal.LoadContractDetails(CustomerID, ContractTypeID, page, rows, out totalCount);

                if ((ContractDetails == null || ContractDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    ContractDetails = dal.LoadContractDetails(CustomerID, ContractTypeID, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = ContractDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult LoadSalesPeriodGridGrid(int page, int rows)
        {
            try
            {
                SalesPeriod model = new SalesPeriod();
                InvoiceDAL dal = new InvoiceDAL();
                int totalCount;
                List<SalesPeriod> SalesPeriodDetails = new List<SalesPeriod>();
                SalesPeriodDetails = dal.LoadSalesPeriodDetails(page, rows, out totalCount);

                if ((SalesPeriodDetails == null || SalesPeriodDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    SalesPeriodDetails = dal.LoadSalesPeriodDetails(page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = SalesPeriodDetails
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult ConfigurationRole()
        {
            ConfigurationRoleModel model = new ConfigurationRoleModel();
            InvoiceDAL dal = new InvoiceDAL();
            SemDAL semDal = new SemDAL();
            model.SearchedUserDetails = new SearchedUserDetails();
            model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
            SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
            model.SearchedUserDetails.UserName = employeeDetails.UserName;
            model.MainRoleLists = dal.GetRoleDetails();
            var category = dal.GetDocumentCategoryList();
            model.CategoryList = category
                        .ToList()
                        .Select(x => new SelectListItem
                        {
                            Value = x.DocumentCategoryID.ToString(),
                            Text = x.DocumentCategory
                        });
            return PartialView("_ConfigurationRole", model);
        }

        public ActionResult LoadConfigurationRoleDetailsMapping(int RoleID)
        {
            InvoiceDAL dal = new InvoiceDAL();
            SemDAL semDal = new SemDAL();
            ConfigurationRoleModel model = new ConfigurationRoleModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
            SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
            model.SearchedUserDetails.UserName = employeeDetails.UserName;
            var MainList = dal.LoadConfigurationRoleDetailsMapping(RoleID);

            foreach (var item in MainList)
            {
                model.IRApprover = item.IRApprover;
                model.IRFinanceApprover = item.IRFinanceApprover;
                model.IRGenerator = item.IRGenerator;
                model.ProjectCreator = item.ProjectCreator;
                model.ResourceAllocator = item.ResourceAllocator;
                model.RoleDescription = item.RoleDescription;
                model.TimesheetToBeFilled = item.TimesheetToBeFilled;
                if (item.DocumentCategoryAccess != null)
                {
                    model.SelectedItemId = item.DocumentCategoryAccess.Select(c => c.ToString());
                }
            }
            model.RoleID = RoleID;
            var category = dal.GetDocumentCategoryList();
            model.CategoryList = category
                        .ToList()
                        .Select(x => new SelectListItem
                        {
                            Value = x.DocumentCategoryID.ToString(),
                            Text = x.DocumentCategory
                        });
            return PartialView("_ConfigurationRoleDetails", model);
        }

        [HttpPost]
        public ActionResult SaveDocumentCategory(ConfigurationRoleModel model)
        {
            bool status = false;
            InvoiceDAL dal = new InvoiceDAL();
            SEMResponse response = new SEMResponse();
            response = dal.SaveDocumentCategory(model);

            return Json(new { status = response.status }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinancePayments()
        {
            FinancePaymentsModel model = new FinancePaymentsModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        [PageAccess(PageName = "Payment Data")]
        public ActionResult FinancePaymentTracking()
        {
            FinancePaymentTrackingModel model = new FinancePaymentTrackingModel();
            InvoiceDAL dal = new InvoiceDAL();
            SemDAL semDal = new SemDAL();
            model.SearchedUserDetails = new SearchedUserDetails();
            model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
            SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
            model.SearchedUserDetails.UserName = employeeDetails.UserName;

            model.ClientListdata = dal.GetClientNamesForFinancePaymentTracking();
            model.DeliveryUnitListdata = dal.GetDeliveryUnitListdata();
            ViewBag.DeliveryUnitListdata = model.DeliveryUnitListdata;

            return View(model);
        }

        [PageAccess(PageName = "Outstanding data")]
        public ActionResult FinanceTrackingSummary()
        {
            FinanceTrackingSummaryModel model = new FinanceTrackingSummaryModel();
            InvoiceDAL dal = new InvoiceDAL();
            SemDAL semDal = new SemDAL();
            model.SearchedUserDetails = new SearchedUserDetails();
            model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
            SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
            model.SearchedUserDetails.UserName = employeeDetails.UserName;

            model.ClientListdata = dal.GetClientNamesForFinancePaymentTracking();
            model.DeliveryUnitListdata = dal.GetDeliveryUnitdata();
            ViewBag.DeliveryUnitListdata = model.DeliveryUnitListdata;
            model.StatusListdata = dal.GetStatusListDataForFinancePaymentOutstanding();
            ViewBag.StatusListdata = model.StatusListdata;

            return View(model);
        }

        [HttpPost]
        public ActionResult FinancePaymentTrackingLoadGrid(int? selectedClientID, int? selectedDeliveryUnitID, int page, int rows)
        {
            try
            {
                FinancePaymentTrackingModel model = new FinancePaymentTrackingModel();
                InvoiceDAL dal = new InvoiceDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<FinancePaymentTrackingModel> Details = new List<FinancePaymentTrackingModel>();
                Details = dal.LoadFinancePaymentTracking(selectedClientID, selectedDeliveryUnitID, page, rows, out totalCount);

                if ((Details == null || Details.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    Details = dal.LoadFinancePaymentTracking(selectedClientID, selectedDeliveryUnitID, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = Details,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult SaveFinanceTrackingDetails(FinancePaymentTrackingModel model, DateTime? ExpectedPaymentDate, int? Days, double? PendingAmount, int? DeliveryUnit)
        {
            try
            {
                string resultMessage = string.Empty;
                bool status = false;
                InvoiceDAL dal = new InvoiceDAL();
                status = dal.SaveFinanceTrackingDetails(model, ExpectedPaymentDate, Days, PendingAmount, DeliveryUnit);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SendMailInvoiceModule(string successEmpIDs, string RFIID, string BtnClick, string IsPIRRequest, string TextLink, int projectId)
        {
            try
            {
                TravelViewModel model = new TravelViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                InvoiceDAL Invoicedal = new InvoiceDAL();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                SemDAL dal = new SemDAL();

                model.Mail = new TravelMailTemplate();
                HRMS_tbl_PM_Employee IRFinanceApproval = new HRMS_tbl_PM_Employee();
                HRMS_tbl_PM_Employee fromEmployeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(successEmpIDs));
                var rfidtls = Invoicedal.GetRFIIDDetails(Convert.ToInt32(RFIID));
                SearchedUserDetails ProjectEmpDetails = Invoicedal.GetEmployeeDetails(rfidtls.RFIRaisedBy);
                if (fromEmployeeDetails != null)
                {
                    model.Mail.From = fromEmployeeDetails.EmailID;
                    model.Mail.Cc = fromEmployeeDetails.EmailID;
                    model.Mail.To = ProjectEmpDetails.EmployeeEmailId;
                    int templateId = 0;
                    if (TextLink == "IRApproval")
                    {
                        if (rfidtls.IsProforma == false)
                        {
                            if (BtnClick == "ApproveIRSuccessMsg")
                            {
                                var a = Invoicedal.GetProjectIRFinanceApprovalList(projectId);
                                model.Mail.To = null;
                                foreach (var ad in a)
                                {
                                    model.Mail.To = model.Mail.To + ad.EmployeeEmailId + ";";
                                }
                                templateId = 68;
                            }
                            if (BtnClick == "InvoiceRejectedSuccess")
                            {
                                templateId = 69;
                            }
                            if (BtnClick == "InvoiceCancelledSuccess")
                            {
                                templateId = 70;
                            }
                        }
                        else
                        {
                            if (BtnClick == "ApproveIRSuccessMsg")
                            {
                                templateId = 71;
                            }
                            if (BtnClick == "InvoiceRejectedSuccess")
                            {
                                templateId = 72;
                            }
                            if (BtnClick == "InvoiceCancelledSuccess")
                            {
                                templateId = 73;
                            }
                        }
                    }
                    else if (TextLink == "FinanceApproval")
                    {
                        List<InvoiceIR_PIRDetails> EmailIDs = Invoicedal.getProjectApproverList(projectId);
                        model.Mail.Cc = "";
                        foreach (var item in EmailIDs)
                        {
                            model.Mail.Cc = model.Mail.Cc + item.EmailID + ";";
                        }
                        if (BtnClick == "ApproveIRSuccessMsg")
                        {
                            templateId = 74;
                        }
                        if (BtnClick == "InvoiceRejectedSuccess")
                        {
                            templateId = 75;
                        }
                        if (BtnClick == "InvoiceCancelledSuccess")
                        {
                            templateId = 76;
                        }
                    }
                    else
                    {
                        string prevStatus = Convert.ToString(Session["CurrentStatus"]);
                        if (prevStatus == InvoiceStages.ApproverStage)
                        {
                            var a = Invoicedal.getProjectApproverList(projectId);
                            model.Mail.To = null;
                            foreach (var ad in a)
                            {
                                model.Mail.To = model.Mail.To + ad.EmailID + ";";
                            }
                        }
                        if (prevStatus == InvoiceStages.FinanceApproverStage || prevStatus == InvoiceStages.ApprovedStage)
                        {
                            //to finance approvers
                            var a = Invoicedal.GetProjectIRFinanceApprovalList(projectId);
                            model.Mail.To = null;
                            foreach (var ad in a)
                            {
                                model.Mail.To = model.Mail.To + ad.EmployeeEmailId + ";";
                            }

                            //cc to projectapprovers
                            var b = Invoicedal.getProjectApproverList(projectId);
                            model.Mail.Cc = null;
                            foreach (var cc in b)
                            {
                                model.Mail.Cc = model.Mail.Cc + cc.EmailID + ";";
                            }
                        }
                        if (BtnClick == "InvoiceCancelledSuccess")
                        {
                            templateId = 92;
                        }
                    }
                    if (BtnClick == "Submit")
                    {
                        if (rfidtls.IsProforma == true)
                            templateId = 78;
                        else
                            templateId = 77;
                        model.Mail.To = "";
                        List<InvoiceIR_PIRDetails> EmailIDs = Invoicedal.getProjectApproverList(projectId);
                        foreach (var item in EmailIDs)
                        {
                            model.Mail.To = model.Mail.To + item.EmailID + ";";
                        }
                    }

                    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                    foreach (var emailTemplate in template)
                    {
                        model.Mail.Subject = emailTemplate.Subject.Replace("##IRID##", RFIID);
                        model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                        model.Mail.Message = model.Mail.Message.Replace("##IRID##", RFIID);
                        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromEmployeeDetails.EmployeeName);
                    }
                }

                return PartialView("_InvoiceCreationEmail", model.Mail);
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
        public ActionResult RejectInvoiceDetails(string textLink, AddInvoiceModel model)
        {
            try
            {
                InvoiceDAL dal = new InvoiceDAL();
                string resultMessage = string.Empty;

                var status = false;
                var isHistoryDetailsDeleted = false;
                status = dal.RejectInvoiceDetails(model);
                if (status)
                {
                    resultMessage = "Saved";
                    if (model.IsProforma == true)
                        isHistoryDetailsDeleted = dal.DeletePIRHistroyDetails(model.IrPirID);
                }
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
        public ActionResult FinanceOutStandingDataLoadGrid(int? selectedClientID, int? selectedDeliveryUnitID, int? selectedStatusID, int page, int rows)
        {
            try
            {
                FinanceTrackingSummaryModel model = new FinanceTrackingSummaryModel();
                InvoiceDAL dal = new InvoiceDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<FinanceTrackingSummaryModel> Details = new List<FinanceTrackingSummaryModel>();
                Details = dal.LoadFinanceOutStandingData(selectedClientID, selectedDeliveryUnitID, selectedStatusID, page, rows, out totalCount);

                if ((Details == null || Details.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    Details = dal.LoadFinanceOutStandingData(selectedClientID, selectedDeliveryUnitID, selectedStatusID, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = Details,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult SaveFinanceOutstandingDataDetails(FinanceTrackingSummaryModel model, int? DeliveryUnit, int? Status)
        {
            try
            {
                string resultMessage = string.Empty;
                bool status = false;
                InvoiceDAL dal = new InvoiceDAL();
                status = dal.SaveFinanceOutstandingDataDetails(model, DeliveryUnit, Status);
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ActionResult FinancePaymentOutstandingDataViewHistory(int RFIID)
        {
            try
            {
                FinanceTrackingSummaryModel model = new FinanceTrackingSummaryModel();
                InvoiceDAL dal = new InvoiceDAL();
                SemDAL semDal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
                model.SearchedUserDetails.UserName = employeeDetails.UserName;

                List<HRMS.Models.FinanceTrackingSummaryModel.FinanceOutStandingDataHistoryList> Details = new List<HRMS.Models.FinanceTrackingSummaryModel.FinanceOutStandingDataHistoryList>();
                Details = dal.GetFinanceOutStandingTrackingDataViewHistory(RFIID);

                if (Details != null)
                {
                    model.OutStandingDataHistoryList = Details;
                }

                return PartialView("_TrackingSummaryShowHistory", model);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ActionResult FinancePaymentTrackingDataViewHistory(int RFIItemID)
        {
            try
            {
                FinancePaymentTrackingModel model = new FinancePaymentTrackingModel();
                InvoiceDAL dal = new InvoiceDAL();
                SemDAL semDal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
                model.SearchedUserDetails.UserName = employeeDetails.UserName;

                List<HRMS.Models.FinancePaymentTrackingModel.FinancePaymentDataHistoryList> Details = new List<HRMS.Models.FinancePaymentTrackingModel.FinancePaymentDataHistoryList>();
                Details = dal.GetFinancePaymentTrackingDataViewHistory(RFIItemID);

                if (Details != null)
                {
                    model.PaymentDataHistoryList = Details;
                }
                return PartialView("_PaymentTrackingShowHistory", model);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private GridView bindGrid = new GridView();

        public void ExportToExcelFinancePaymentData(int? ClientID, int? DeliveryUnitID)
        {
            WSEMDBEntities dbContext = new WSEMDBEntities();
            DataSet dsCurrentDetails = new DataSet();
            SemDAL dal = new SemDAL();

            var Details = dbContext.GetFinancePaymentTrackingDetails_SP(ClientID, DeliveryUnitID);

            DataTable dt = new DataTable();
            dt.Columns.Add("Client", typeof(string));
            dt.Columns.Add("Delivery Unit", typeof(string));
            dt.Columns.Add("System Invoice Number", typeof(string));
            dt.Columns.Add("Quick Books Invoice Number", typeof(string));
            dt.Columns.Add("Invoice Month & Date", typeof(string));
            dt.Columns.Add("Invoice Sent Date", typeof(string));
            dt.Columns.Add("Expected Payment Date", typeof(string));
            dt.Columns.Add("Report Date", typeof(string));
            dt.Columns.Add("Payment Term", typeof(string));
            dt.Columns.Add("Days", typeof(string));
            dt.Columns.Add("Amount", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Collection Date", typeof(string));
            dt.Columns.Add("Collected Amount", typeof(string));
            dt.Columns.Add("Pending Amount", typeof(string));

            if (Details != null)
            {
                foreach (var item in Details.ToList())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = item.ClientName;
                    dr[1] = item.DeliveryUnit;
                    dr[2] = item.SystemInvoiceNumber;
                    dr[3] = item.QuickBooksInvoiceNumber;
                    dr[4] = item.InvoiceMonthDate;
                    dr[5] = item.InvoiceSentDate;
                    dr[6] = item.ExpectedPaymentDate;
                    dr[7] = item.ReportDate;
                    dr[8] = item.PaymentTerm;
                    dr[9] = item.Days;
                    dr[10] = item.Amount;
                    dr[11] = item.Status;
                    dr[12] = item.CollectionDate;
                    dr[13] = item.CollectedAmount;
                    dr[14] = item.PendingAmount;
                    dt.Rows.Add(dr);
                }
                dsCurrentDetails.Tables.Add(dt);
                bindGrid.DataSource = dsCurrentDetails;
                bindGrid.DataBind();
            }

            string strFileNameTo = Convert.ToString(System.DateTime.Now.ToShortDateString().ToString());
            string strFileName = "V2_FinancePaymentData_" + "_" + strFileNameTo.ToString().Replace("/", "-");
            strFileName = strFileName + ".xls";
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename = " + strFileName);
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);

            bindGrid.SetRenderMethodDelegate(new RenderMethod(RenderTitleCurrent));
            bindGrid.RenderControl(oHtmlTextWriter);

            Response.Write(oStringWriter.ToString());
            Response.End();
        }

        public virtual void RenderTitleCurrent(HtmlTextWriter writer, Control ctl)
        {
            writer.AddAttribute("colspan", bindGrid.Columns.Count.ToString(System.Globalization.CultureInfo.InvariantCulture));
            writer.AddAttribute("align", "center");
            writer.RenderBeginTag("TD");
            writer.Write(" Finance Payment Data Details");
            writer.RenderEndTag();
            bindGrid.HeaderStyle.AddAttributesToRender(writer);
            writer.RenderBeginTag("TR");
            writer.RenderEndTag();
            foreach (Control control in ctl.Controls)
            {
                control.RenderControl(writer);
            }
        }

        private GridView bindGrid1 = new GridView();

        public void ExportToExcelFinanceOutStandingData(int? ClientID, int? DeliveryUnitID, int? StatusID)
        {
            WSEMDBEntities dbContext = new WSEMDBEntities();
            DataSet dsCurrentDetails = new DataSet();
            SemDAL dal = new SemDAL();

            var Details = dbContext.GetFinancePaymentOutStandingDataDetails_SP(ClientID, DeliveryUnitID, StatusID);

            DataTable dt = new DataTable();
            dt.Columns.Add("Client Name", typeof(string));
            dt.Columns.Add("Delivery Unit", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Current", typeof(string));
            dt.Columns.Add("Days1To30", typeof(string));
            dt.Columns.Add("Days31To60", typeof(string));
            dt.Columns.Add("Days61To90", typeof(string));
            dt.Columns.Add("DaysAbove90", typeof(string));
            dt.Columns.Add("Total", typeof(string));
            dt.Columns.Add("Payment Term", typeof(string));
            dt.Columns.Add("Collection Amount", typeof(string));
            dt.Columns.Add("Last Payment Date", typeof(string));

            if (Details != null)
            {
                foreach (var item in Details.ToList())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = item.clientName;
                    dr[1] = item.DeliveryUnitName;
                    dr[2] = item.StatusName;
                    dr[3] = item.CurrentAmount;
                    dr[4] = item.Days1To30;
                    dr[5] = item.Days31To60;
                    dr[6] = item.Days61To90;
                    dr[7] = item.DaysAbove90;
                    dr[8] = item.Total;
                    dr[9] = item.PaymentTerms;
                    dr[10] = item.CollectionAmount;
                    dr[11] = item.LastPaymentDate;

                    dt.Rows.Add(dr);
                }
                dsCurrentDetails.Tables.Add(dt);
                bindGrid1.DataSource = dsCurrentDetails;
                bindGrid1.DataBind();
            }

            string strFileNameTo = Convert.ToString(System.DateTime.Now.ToShortDateString().ToString());
            string strFileName = "V2_FinanceOutStandingData_" + "_" + strFileNameTo.ToString().Replace("/", "-");
            strFileName = strFileName + ".xls";
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename = " + strFileName);
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);

            bindGrid1.SetRenderMethodDelegate(new RenderMethod(RenderTitleCurrentOutStanding));
            bindGrid1.RenderControl(oHtmlTextWriter);

            Response.Write(oStringWriter.ToString());
            Response.End();
        }

        public virtual void RenderTitleCurrentOutStanding(HtmlTextWriter writer, Control ctl)
        {
            writer.AddAttribute("colspan", bindGrid1.Columns.Count.ToString(System.Globalization.CultureInfo.InvariantCulture));
            writer.AddAttribute("align", "center");
            writer.RenderBeginTag("TD");
            writer.Write(" Finance OutStanding Data Details");
            writer.RenderEndTag();
            bindGrid1.HeaderStyle.AddAttributesToRender(writer);
            writer.RenderBeginTag("TR");
            writer.RenderEndTag();
            foreach (Control control in ctl.Controls)
            {
                control.RenderControl(writer);
            }
        }

        public ActionResult CheckInvoiceGridDetailsExistorNot(int RFIID)
        {
            try
            {
                WSEMDBEntities dbcontext = new WSEMDBEntities();
                var data = dbcontext.tbl_PM_RFI_Items.Where(x => x.RFIID == RFIID).ToList();

                int datacount = data.Count();

                return Json(new { count = datacount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ActionResult GetAlternateId(int RFIID, int LoggedInEmployeeId)
        {
            int? UniqueId = 0;
            try
            {
                WSEMDBEntities dbcontext = new WSEMDBEntities();
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                var s = dbcontext.CopyRFIIDDetails(RFIID, LoggedInEmployeeId, Output);
                foreach (var Res in s)
                {
                    UniqueId = Res;
                    break;
                }
            }
            catch (Exception e)
            {
            }
            return Json(new { NewRFid = UniqueId }, JsonRequestBehavior.AllowGet);
        }
    }
}