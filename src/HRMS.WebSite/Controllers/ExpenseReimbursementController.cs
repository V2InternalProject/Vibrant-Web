using HRMS.DAL;
using HRMS.Helper;
using HRMS.Models;
using HRMS.Notification;
using MvcApplication3.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class ExpenseReimbursementController : Controller
    {
        //
        // GET: /ExpenseReimbursement/
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();

        public ActionResult Index()
        {
            try
            {
                ExpenseReimbursementViewModel model = new ExpenseReimbursementViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                ViewBag.AsciiKey = Session["SecurityKey"].ToString();
                string employeeCode = Membership.GetUser().UserName;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                string encyptedEmploeeId = Commondal.Encrypt(this.Session["SecurityKey"].ToString() + Convert.ToString(employeeId), true);
                ViewBag.encryptedEmployeeId = encyptedEmploeeId;
                int expenseId = 0;
                string encyptedExpenseId = Commondal.Encrypt(this.Session["SecurityKey"].ToString() + Convert.ToString(expenseId), true);
                ViewBag.EncryptedExpenseId = encyptedExpenseId;
                string[] role = Roles.GetRolesForUser(employeeCode);

                if (employeeCode != null)
                {
                    model.SearchedUserDetails.EmployeeId = employeeId;
                    model.SearchedUserDetails.EmployeeCode = employeeCode;
                    model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }

                int page = 1;
                int rows = 5;
                string term = "";
                string Field = "";
                string FieldChild = "";
                int totalCount;
                List<ExpenseReimbursementStatus> searchResultInbox = new List<ExpenseReimbursementStatus>();

                ExpenseReimbursementDAL expDAL = new ExpenseReimbursementDAL();
                searchResultInbox = expDAL.GetInboxListDetails(term, Field, FieldChild, page, rows, employeeId, out totalCount);
                ViewBag.InboxCount = totalCount;

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        [PageAccess(PageName = "Expense")]
        public ActionResult GetExpenseReimbursementFormDetails(string expenseEmployeeId, string encryptedExpenseId, string viewDetails)
        {
            try
            {
                Session["SearchEmpFullName"] = null;  // to hide emp search
                Session["SearchEmpCode"] = null;
                Session["SearchEmpID"] = null;

                ExpenseReimbursementViewModel model = new ExpenseReimbursementViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
                tbl_HR_Expense Records = new tbl_HR_Expense();
                string employeeCode = Membership.GetUser().UserName;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                string decryptedExpenseEmployeeId = string.Empty;
                string decryptedExpenseId = string.Empty;
                if (expenseEmployeeId == null)
                {
                    expenseEmployeeId = Commondal.Encrypt(this.Session["SecurityKey"].ToString() + Convert.ToString(employeeId), true);
                    ViewBag.encryptedEmployeeId = expenseEmployeeId;
                }
                else
                {
                    if (expenseEmployeeId == employeeId.ToString())
                    {
                        expenseEmployeeId = Commondal.Encrypt(this.Session["SecurityKey"].ToString() + Convert.ToString(expenseEmployeeId), true);
                        ViewBag.encryptedEmployeeId = expenseEmployeeId;
                    }
                    else
                    {
                        ViewBag.encryptedEmployeeId = expenseEmployeeId;
                    }
                }

                if (expenseEmployeeId != null)
                {
                    if (expenseEmployeeId == employeeId.ToString())
                    {
                        decryptedExpenseEmployeeId = expenseEmployeeId;
                    }
                    else
                    {
                        bool isAuthorize;
                        decryptedExpenseEmployeeId = HRMSHelper.Decrypt(expenseEmployeeId, out isAuthorize);
                        if (!isAuthorize)
                            return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                    }
                }
                else
                    decryptedExpenseEmployeeId = Convert.ToString(employeeId);
                if (encryptedExpenseId != null)
                {
                    bool isAuthorizeExpense;
                    decryptedExpenseId = HRMSHelper.Decrypt(encryptedExpenseId, out isAuthorizeExpense);
                    if (!isAuthorizeExpense)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedExpenseId = "0";

                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = employeeId;
                ViewBag.loginUserId = employeeId;
                model.ReimbursementEmployeeId = Convert.ToInt32(decryptedExpenseEmployeeId);
                ViewBag.reimbursementHRMSEmployeeId = model.ReimbursementEmployeeId;
                ViewBag.loginReimbursementEmployeeId = Commondal.Encrypt(Convert.ToString(Session["SecurityKey"].ToString() + employeeId), true);
                ViewBag.encryptedReimbursementEmployeeId = expenseEmployeeId;
                ViewBag.encryptedReimbursementExpenseId = encryptedExpenseId;
                ViewBag.ReimbursementEmployeeId = decryptedExpenseEmployeeId;
                ViewBag.viewDetailClick = viewDetails;
                if (model.ReimbursementEmployeeId != null)
                {
                    SemDAL semdal = new SemDAL();
                    int ReimbursementEmpCode = dal.GetEmployeeID(model.ReimbursementEmployeeId);
                    int Reimbursementemployeeid = semdal.geteEmployeeIDFromSEMDatabase(ReimbursementEmpCode.ToString());
                    ViewBag.Reimbursementemployeeid = Reimbursementemployeeid;
                }
                if (Convert.ToInt32(decryptedExpenseId) == 0)
                    dal.DeletedExpenseDetailsForNewForm(Convert.ToInt32(decryptedExpenseId), Convert.ToInt32(decryptedExpenseEmployeeId));
                model.FormDataList = new List<ExpenseReimbursementDetails>();
                if (decryptedExpenseId == "0")
                    model.ReimbursementEmployeeCode = Convert.ToInt32(employeeCode);
                else
                {
                    Records = dal.GetExpenseDetailsfromExpenseId(Convert.ToInt32(decryptedExpenseId));
                    int ExpEmpCode = dal.GetEmployeeID(Convert.ToInt32(Records.EmployeeID));
                    model.ReimbursementEmployeeCode = ExpEmpCode;
                }
                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(decryptedExpenseEmployeeId));
                model.ReimbursementEmployeeName = employeeDetails.EmployeeName;

                model.FormCode = dal.GetFormCode();

                tbl_PM_OfficeLocation employeeLocation = dal.GetLoacation(Convert.ToInt16(employeeDetails.OfficeLocation));
                if (employeeLocation != null)
                    model.Location = employeeLocation.OfficeLocation;
                model.ClientReimbursementList = dal.clientReimbursementList();

                model.CostCentreList = dal.costCentreList();
                model.CurrencyList = dal.currencyList();
                model.ProjectNameList = dal.projectNameList();
                if (Convert.ToInt32(decryptedExpenseId) == 0)
                    model.DateOfSubmission = DateTime.Now.Date;
                else
                    model.DateOfSubmission = Records.DateOfSubmission;
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string[] requestorRole = Roles.GetRolesForUser(model.ReimbursementEmployeeCode.ToString());
                if (requestorRole.Contains(UserRoles.Management))
                    ViewBag.RequestorRole = UserRoles.Management;
                else if (requestorRole.Contains(UserRoles.GroupHead))
                    ViewBag.RequestorRole = UserRoles.GroupHead;
                else if (requestorRole.Contains(UserRoles.AccountOwner))
                    ViewBag.RequestorRole = UserRoles.AccountOwner;
                else if (requestorRole.Contains(UserRoles.DeliveryManager))
                    ViewBag.RequestorRole = UserRoles.DeliveryManager;
                else if (requestorRole.Contains(UserRoles.Manager))
                    ViewBag.RequestorRole = UserRoles.Manager;

                if (Records.ExpenseID != 0)
                {
                    if (role.Contains(UserRoles.ExpenseAdmin))
                    {
                        ViewBag.IsExpenseAdmin = UserRoles.ExpenseAdmin;
                    }
                    if (role.Contains(UserRoles.ExpenseAdmin) && Records.StageID == 3)
                    {
                        ViewBag.UserRole = UserRoles.ExpenseAdmin;
                        model.SearchedUserDetails.UserRole = UserRoles.ExpenseAdmin;
                        ViewBag.IsExpenseAdmin = UserRoles.ExpenseAdmin;
                    }
                    else if (role.Contains(UserRoles.ExpenseApprover) && (Records.StageID == 1 || Records.StageID == 2))
                    {
                        ViewBag.UserRole = UserRoles.ExpenseApprover;
                        model.SearchedUserDetails.UserRole = UserRoles.ExpenseApprover;
                    }
                }
                else
                {
                    string user = Commondal.GetMaxRoleForUser(role);
                    ViewBag.UserRole = user;
                    model.SearchedUserDetails.UserRole = user;
                    if (role.Contains(UserRoles.ExpenseAdmin))
                        ViewBag.IsExpenseAdmin = UserRoles.ExpenseAdmin;
                }
                int expenseId = 0;
                model.NatureOfExpenseList = dal.GetNatureOfExpense();
                tbl_HR_Expense expenseFormDetails = dal.GetExpenseDetails(Convert.ToInt32(decryptedExpenseEmployeeId), Convert.ToInt32(decryptedExpenseId));

                if (expenseFormDetails != null)
                {
                    expenseId = expenseFormDetails.ExpenseID;
                    model.ExpenseId = expenseFormDetails.ExpenseID;
                    model.ReimbursementFormName = expenseFormDetails.ReimbursementFormName;
                    model.ProjectName = expenseFormDetails.ProjectName;
                    model.ReimbursementFormCode = expenseFormDetails.ReimbursementFormCode;
                    model.PrimaryApprover = expenseFormDetails.PrimaryApprover;
                    model.SecondaryApprover = expenseFormDetails.SecondaryApprover;
                    model.FinanceApprover = expenseFormDetails.FinanceApprover;
                    model.ClientReimbursement = expenseFormDetails.IsClientReimbursement;
                    model.clientName = expenseFormDetails.ClientName;
                    model.StageID = expenseFormDetails.StageID;
                    model.Advances = expenseFormDetails.Advance;
                    model.AmountInWords = expenseFormDetails.AmountInWords;
                    model.IsBalanceApproved = Convert.ToBoolean(expenseFormDetails.IsBalanceApprove);
                    model.IsAdvanceApproved = Convert.ToBoolean(expenseFormDetails.IsAdvanceApprove);
                    model.IsTotalApprove = Convert.ToBoolean(expenseFormDetails.IsTotalApprove);
                    model.CostCentre = expenseFormDetails.TravelCostCenterID;
                    model.Currency = expenseFormDetails.CurrencyID;
                    model.FormCode = expenseFormDetails.FormCode;
                    model.ChequeDetails = expenseFormDetails.ChequeDetails;
                }
                else
                    model.Advances = 0;
                if (model.StageID == null)
                    model.StageID = 0;

                //to calculate total expense
                //int page = 1;
                //int rows = 5;
                //int totalCount;
                decimal? total = 0;
                //List<ExpenseReimbursementDetails> expenseDetails = new List<ExpenseReimbursementDetails>();
                total = dal.CalculateTotalExpense(Convert.ToInt32(decryptedExpenseEmployeeId), expenseId);
                //foreach (var item in expenseDetails)
                //{
                //    total = total + item.Amount;
                //}
                model.Total = total;
                if (model.PrimaryApprover != null)
                {
                    HRMS_tbl_PM_Employee PrimarAppName = employeeDAL.GetEmployeeDetails(Convert.ToInt32(model.PrimaryApprover));
                    ViewBag.PrimaryApproverName = PrimarAppName.EmployeeName;
                }

                if (model.SecondaryApprover != null)
                {
                    HRMS_tbl_PM_Employee SecondaryAppName = employeeDAL.GetEmployeeDetails(Convert.ToInt32(model.SecondaryApprover));
                    ViewBag.SecondaryApproverName = SecondaryAppName.EmployeeName;
                }
                //List of Expense_Approver employees
                Dictionary<int, string> ExpenseApprovers = new Dictionary<int, string>();
                string[] expenseApproverUsers = Roles.GetUsersInRole(UserRoles.ExpenseApprover);
                foreach (string ExpenseApproverlist in expenseApproverUsers)
                {
                    HRMS_tbl_PM_Employee expenseEmployee = employeeDAL.GetEmployeeDetailsByEmployeeCode(ExpenseApproverlist);
                    if (expenseEmployee != null)
                    {
                        if (expenseEmployee.EmployeeID != Convert.ToInt32(decryptedExpenseEmployeeId))
                        {
                            if (expenseEmployee != null)
                            {
                                if (expenseFormDetails != null)
                                {
                                    if ((expenseEmployee.EmployeeID == employeeDetails.CostCenterID) &&
                                        (model.PrimaryApprover == null || model.PrimaryApprover == 0))
                                    {
                                        model.PrimaryApprover = expenseEmployee.EmployeeID;
                                    }
                                    if ((expenseEmployee.EmployeeID == employeeDetails.CompetencyManager) &&
                                        (model.SecondaryApprover == null || model.SecondaryApprover == 0))
                                    {
                                        model.SecondaryApprover = expenseEmployee.EmployeeID;
                                    }
                                }
                                ExpenseApprovers.Add(expenseEmployee.EmployeeID, expenseEmployee.EmployeeName);
                                ExpenseApprovers = ExpenseApprovers.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                            }
                        }
                    }
                }

                ViewBag.primaryApprovers = new SelectList(ExpenseApprovers, "Key", "Value");
                ViewBag.secondaryApprovers = new SelectList(ExpenseApprovers, "Key", "Value");

                //List of Expense_Admin employees
                Dictionary<int, string> FinanceApprovers = new Dictionary<int, string>();
                string[] financeApproverUsers = Roles.GetUsersInRole(UserRoles.ExpenseAdmin);
                foreach (string financeApproverUserList in financeApproverUsers)
                {
                    HRMS_tbl_PM_Employee financeEmployee = employeeDAL.GetEmployeeDetailsByEmployeeCode(financeApproverUserList);
                    if (financeEmployee != null)
                        FinanceApprovers.Add(financeEmployee.EmployeeID, financeEmployee.EmployeeName);
                    FinanceApprovers = FinanceApprovers.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                }
                ViewBag.financeApprovers = new SelectList(FinanceApprovers, "Key", "Value");
                ViewBag.ExpenseModule = ModuleNames.ExpenseModule;
                return View("ExpenseReimbursementForm", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        //to calculate total expense
        [HttpGet]
        public ActionResult CalcuateTotalExpense(string expenseEmployeeId, string encryptedExpenseId)
        {
            try
            {
                bool isAuthorize;
                int expenseId = 0;
                string decryptedExpenseEmployeeId;
                if (expenseEmployeeId.Length <= 5)
                {
                    decryptedExpenseEmployeeId = expenseEmployeeId;
                }
                else
                {
                    decryptedExpenseEmployeeId = HRMSHelper.Decrypt(expenseEmployeeId, out isAuthorize);
                }
                ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();

                bool isAuthorizeExpense;
                string decryptedExpenseId = HRMSHelper.Decrypt(encryptedExpenseId, out isAuthorizeExpense);
                if (decryptedExpenseId == "")
                    decryptedExpenseId = "0";
                tbl_HR_Expense expenseFormDetails = dal.GetExpenseDetails(Convert.ToInt32(decryptedExpenseEmployeeId), Convert.ToInt32(decryptedExpenseId));
                if (expenseFormDetails != null)
                    expenseId = expenseFormDetails.ExpenseID;

                //int page = 1;
                //int rows = 5;
                //int totalCount;
                decimal? total = 0;
                List<ExpenseReimbursementDetails> expenseDetails = new List<ExpenseReimbursementDetails>();

                //new function to calculate total amount
                total = dal.CalculateTotalExpense(Convert.ToInt32(decryptedExpenseEmployeeId), expenseId);
                //foreach (var item in expenseDetails)
                //{
                //    total = total + item.Amount;
                //}
                return Json(new { status = true, total = total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors while caluclating TotalExpense" });
            }
        }

        //to calculate amount in words
        [HttpPost]
        public ActionResult NumberToStringConvertor(decimal balance)
        {
            try
            {
                int number = Convert.ToInt32(balance);
                string result = null;
                if (number == 0)
                {
                    result = "Zero";
                }
                //if (number == -2147483648)
                //    result = "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";
                int[] num = new int[4];
                int first = 0;
                int u, h, t;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (number < 0)
                {
                    sb.Append("Minus ");
                    number = -number;
                }
                string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
                string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
                string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
                string[] words3 = { "Thousand ", "Lakh ", "Crore " };
                num[0] = number % 1000; // units
                num[1] = number / 1000;
                num[2] = number / 100000;
                num[1] = num[1] - 100 * num[2]; // thousands
                num[3] = number / 10000000; // crores
                num[2] = num[2] - 100 * num[3]; // lakhs
                for (int i = 3; i > 0; i--)
                {
                    if (num[i] != 0)
                    {
                        first = i;
                        break;
                    }
                }
                for (int i = first; i >= 0; i--)
                {
                    if (num[i] == 0) continue;
                    u = num[i] % 10; // ones
                    t = num[i] / 10;
                    h = num[i] / 100; // hundreds
                    t = t - 10 * h; // tens
                    if (h > 0) sb.Append(words0[h] + "Hundred ");
                    if (u > 0 || t > 0)
                    {
                        if (h > 0 || i == 0) sb.Append(" ");
                        if (t == 0)
                            sb.Append(words0[u]);
                        else if (t == 1)
                            sb.Append(words1[u]);
                        else
                            sb.Append(words2[t - 2] + words0[u]);
                    }
                    if (i != 0) sb.Append(words3[i - 1]);
                }
                result = sb.ToString().TrimEnd();
                if (result == "" || result == null)
                    result = "Zero";
                return Json(new { balance = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action" });
            }
        }

        [HttpPost]
        public ActionResult ExpenseDetailLoadGrid(string encryptedEmployeeId, int expenseId, int page, int rows)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(encryptedEmployeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                ExpenseReimbursementStatus model = new ExpenseReimbursementStatus();
                ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<ExpenseReimbursementDetails> expenseDetails = new List<ExpenseReimbursementDetails>();
                expenseDetails = dal.ExpenseDetailRecord(Convert.ToInt32(decryptedEmployeeId), expenseId, page, rows, out totalCount, encryptedEmployeeId);
                if ((expenseDetails == null || expenseDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    expenseDetails = dal.ExpenseDetailRecord(Convert.ToInt32(decryptedEmployeeId), expenseId, page, rows, out totalCount, encryptedEmployeeId);
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

        public ActionResult DeleteExpenseDetails(int expenseId, string expenseEmployeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(expenseEmployeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
            //   bool status = dal.DeletedExpenseDetails(expenseId, Convert.ToInt32(decryptedEmployeeId));
                 bool status = true;
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// To move process back to Requestor when any Approver Rejects Expense Reimbursement Form.
        /// </summary>
        /// <param name="model">Object of ExpenseReimbursementViewModel class</param>
        /// <returns>Boolean status and Encrypted expenseID</returns>
        [HttpPost]
        public ActionResult RejectExpenseDetails(ExpenseReimbursementViewModel model)
        {
            ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
            try
            {
                int expenseId = model.ExpenseId;
                string expenseID = Commondal.Encrypt(Session["SecurityKey"].ToString() + expenseId, true);
                int loginEmpId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                var status = dal.RejectExpenseApprovalForm(model, loginEmpId);

                ExpenseReimbursementStatus _expeseDetails = dal.getExpenseDetails(expenseId);
                int? stageID = _expeseDetails.StageId;
                int? primaryApprover = _expeseDetails.PrimaryApprover;
                int? SecondaryApprover = _expeseDetails.SecondaryApprover;
                int? FinanceApprover = _expeseDetails.FinanceApprover;
                int? expenseEmployeeId = Convert.ToInt32(model.ReimbursementEmployeeId);
                string formname = _expeseDetails.FormCode.ToString();

                return Json(new { status = status, expenseID = expenseID, employeeID = expenseEmployeeId, stageID = stageID, primaryApprover = primaryApprover, secondaryApprover = SecondaryApprover, FinanceApprover = FinanceApprover, Formname = formname, ExpenseCode = model.ReimbursementEmployeeCode }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveExpenseApprovalForm(ExpenseReimbursementViewModel model)
        {
            try
            {
                ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
                ExpenseReimbProcessResponse response = new ExpenseReimbProcessResponse();
                CommonMethodsDAL commondal = new CommonMethodsDAL();
                response.isAdded = false;
                Dictionary<int, bool> details = new Dictionary<int, bool>();
                if (model.ExpenseDetails != null)
                {
                    string expenseDetailsWithComma = model.ExpenseDetails.TrimEnd(',');
                    string[] expenseDetailsArrray = expenseDetailsWithComma.Split(',');
                    for (int i = 0; i < expenseDetailsArrray.Length; i++)
                    {
                        ExpenseDetail x = new ExpenseDetail();
                        x.ExpenseDetailID = Convert.ToInt32(expenseDetailsArrray[i]);
                        i++;
                        if (expenseDetailsArrray[i] == "True")
                            x.Verify = true;
                        else
                            x.Verify = false;
                        details.Add(x.ExpenseDetailID, x.Verify);
                    }
                }

                response = dal.SaveExpenseApprovalForm(model, details);
                string EncryptedExpenseId = commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(response.latestExpenseID), true);

                return Json(new { status = response.isAdded, EncryptedExpenseId = EncryptedExpenseId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SubmitExpenseApprovalForm(ExpenseReimbursementViewModel model)
        {
            try
            {
                ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
                ExpenseReimbProcessResponse response = new ExpenseReimbProcessResponse();
                response.isAdded = false;
                Dictionary<int, bool> details = new Dictionary<int, bool>();
                string resultMessage = string.Empty;
                var status = false;
                string expenseID = string.Empty;
                int loginEmpId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                if (model.StageID == 0)
                    response = dal.SaveExpenseApprovalForm(model, details);
                if (model.StageID == 3)
                    response.isAdded = dal.SaveExpenseDetailsFormAdmin(model);
                else
                    response.isAdded = true;

                if (response.isAdded == true)
                {
                    string loggedInUserRole = "";
                    string[] role = Roles.GetRolesForUser(model.ReimbursementEmployeeCode.ToString());
                    if (role.Contains(UserRoles.Management))
                        loggedInUserRole = UserRoles.Management;
                    else if (role.Contains(UserRoles.GroupHead))
                        loggedInUserRole = UserRoles.GroupHead;
                    else if (role.Contains(UserRoles.AccountOwner))
                        loggedInUserRole = UserRoles.AccountOwner;
                    else if (role.Contains(UserRoles.DeliveryManager))
                        loggedInUserRole = UserRoles.DeliveryManager;
                    else if (role.Contains(UserRoles.Manager))
                        loggedInUserRole = UserRoles.Manager;

                    status = dal.SubmitExpenseApprovalForm(model, loginEmpId, loggedInUserRole);
                    if (status)
                        resultMessage = "Saved";
                    else
                        resultMessage = "Error";
                }
                else
                    resultMessage = "Error";

                int ExpenseId = 0;
                if (model.FormCode != 0)
                {
                    ExpenseId = dal.getExpenseIDfromFormCode(model.FormCode);
                    expenseID = Commondal.Encrypt(Session["SecurityKey"].ToString() + ExpenseId, true);
                }

                ExpenseReimbursementStatus _expeseDetails = dal.getExpenseDetails(ExpenseId);
                int? stageID = _expeseDetails.StageId;
                int? primaryApprover = _expeseDetails.PrimaryApprover;
                int? SecondaryApprover = _expeseDetails.SecondaryApprover;
                int? FinanceApprover = _expeseDetails.FinanceApprover;
                int? expenseEmployeeId = Convert.ToInt32(model.ReimbursementEmployeeId);
                string formname = _expeseDetails.FormCode.ToString();

                return Json(new { status = status, expenseID = expenseID, employeeID = expenseEmployeeId, stageID = stageID, primaryApprover = primaryApprover, secondaryApprover = SecondaryApprover, FinanceApprover = FinanceApprover, Formname = formname, ExpenseCode = model.ReimbursementEmployeeCode }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveExpenseDetails(ExpenseReimbursementViewModel model, int ExpenseId, string EncryptedEmployeeId, string EncryptedExpenseId, int ReimbursementEmployeeId, string UploadedFileName, string UploadedFilePath)
        {
            ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
            try
            {
                string resultMessage = string.Empty;
                Tbl_HR_ExpenseDetail expenseDetails = new Tbl_HR_ExpenseDetail();
                if (UploadedFileName != null && UploadedFilePath != null)
                {
                    expenseDetails = dal.GetExpenseDetailsRecord(model.ExpenseDetailsId);
                    if (expenseDetails != null)
                    {
                        if (System.IO.File.Exists(expenseDetails.FilePath))
                        {
                            System.IO.File.Delete(expenseDetails.FilePath);
                        }
                    }
                }
                var status = dal.SaveExpenseDetails(model, ExpenseId, EncryptedEmployeeId, EncryptedExpenseId, ReimbursementEmployeeId, UploadedFileName, UploadedFilePath);

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

        public ActionResult DownloadExpenseFile(int ExpenseDetailsID)
        {
            try
            {
                ExpenseReimbursementDAL ExpDal = new ExpenseReimbursementDAL();
                Tbl_HR_ExpenseDetail _expenseDetails = new Tbl_HR_ExpenseDetail();
                _expenseDetails = ExpDal.GetExpenseDetailsRecord(ExpenseDetailsID);
                string[] FileExtention = _expenseDetails.FileName.Split('.');
                string contentType = "application/" + FileExtention[1];
                string Filepath = Path.Combine(_expenseDetails.FilePath, _expenseDetails.FileName);
                return File(Filepath, contentType, _expenseDetails.FileName);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult GetExpenseReimbursementStatus()
        {
            try
            {
                ExpenseReimbursementStatus model = new ExpenseReimbursementStatus();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.FieldChildList = new List<ReimbursementFieldChildDetails>();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                ViewBag.EncryptedEmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                ViewBag.FieldChildListBG = new SelectList(GetFieldChildDetailsListForExpense("Business Group"), "Id", "Description");
                ViewBag.FieldChildListOU = new SelectList(GetFieldChildDetailsListForExpense("Organization Unit"), "Id", "Description");
                ViewBag.FieldChildListSN = new SelectList(GetFieldChildDetailsListForExpense("Stage Name"), "Id", "Description");
                model.SearchedUserDetails.UserRole = user;
                return View("ExpenseReimbursementStatus", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult ShowStatusDetails(string ExpenseId, int page, int rows)
        {
            try
            {
                ExpenseReimbursementDAL ExpDal = new ExpenseReimbursementDAL();

                int totalCount;
                bool isAuthorize = true;
                string decryptedExpenseID = HRMSHelper.Decrypt(ExpenseId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                List<ExpenseReimbursementShowStatus> ShowStatusResult = new List<ExpenseReimbursementShowStatus>();
                ShowStatusResult = ExpDal.GetShowStatusResult(page, rows, Convert.ToInt32(decryptedExpenseID), out totalCount);

                if ((ShowStatusResult == null || ShowStatusResult.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    ShowStatusResult = ExpDal.GetShowStatusResult(page, rows, Convert.ToInt32(decryptedExpenseID), out totalCount);
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

        public List<FieldChildDetails> GetFieldChildDetailsListForExpense(string ExpenseField)
        {
            try
            {
                ExpenseReimbursementDAL objExpenseDal = new ExpenseReimbursementDAL();
                List<FieldChildDetails> childs = objExpenseDal.GetFieldChildDetailsList(ExpenseField);
                childs.ToList();
                return childs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetFieldChildDetailsList(string ExpenseField)
        {
            var objStatus = new JsonResult();
            try
            {
                ExpenseReimbursementDAL objExpenseDal = new ExpenseReimbursementDAL();
                List<FieldChildDetails> childs = objExpenseDal.GetFieldChildDetailsList(ExpenseField);

                objStatus.Data = childs;
                objStatus.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return objStatus;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult LoadInboxListGrid(string term, string Field, string FieldChild, int page, int rows, string employeeId)
        {
            List<ExpenseReimbursementStatus> FinalInboxListDetails = new List<ExpenseReimbursementStatus>();
            try
            {
                int totalCount;
                bool isAuthorize;

                string decryptedEmployeeId = Convert.ToString(employeeId);
                ExpenseReimbursementDAL expenseReimbursementdal = new ExpenseReimbursementDAL();
                CommonMethodsDAL commondal = new CommonMethodsDAL();

                List<ExpenseReimbursementStatus> InboxListDetails = expenseReimbursementdal.GetInboxListDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);

                if ((InboxListDetails == null || InboxListDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    InboxListDetails = expenseReimbursementdal.GetInboxListDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                }

                foreach (var item in InboxListDetails)
                {
                    string ExitId = commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.ExpenseId), true);
                    item.EncryptedExpenseId = ExitId;
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
            List<ExpenseReimbursementStatus> FinalWatchListDetails = new List<ExpenseReimbursementStatus>();
            try
            {
                int totalCount;
                bool isAuthorize;
                CommonMethodsDAL commondal = new CommonMethodsDAL();
                ExpenseReimbursementDAL expenseReimbursementdal = new ExpenseReimbursementDAL();

                string decryptedEmployeeId = employeeId;

                List<ExpenseReimbursementStatus> WatchListDetails = expenseReimbursementdal.GetWatchListDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                if ((WatchListDetails == null || WatchListDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    WatchListDetails = expenseReimbursementdal.GetWatchListDetails(term, Field, FieldChild, page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                }
                foreach (var item in WatchListDetails)
                {
                    string ExpenseId = commondal.Encrypt(Session["SecurityKey"].ToString() + Convert.ToString(item.ExpenseId), true);
                    item.EncryptedExpenseId = ExpenseId;
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

        [HttpPost]
        public ActionResult ExpenseReimbursementMail(string expenseId, bool isApproveCall, bool IsRejectCall, bool IsCancelled, string employeeID, int? stageID, int? primaryApprover, int? secondaryApprover, int? FinanceApprover, string Formname, string comments, int ExpenseCode)
        {
            try
            {
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                bool result = false;
                bool isAuthorize;
                string decryptedExpenseID = HRMSHelper.Decrypt(expenseId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                string loggedInUserRole = "";
                string[] role = Roles.GetRolesForUser(ExpenseCode.ToString());
                if (role.Contains(UserRoles.Management))
                    loggedInUserRole = UserRoles.Management;
                else if (role.Contains(UserRoles.GroupHead))
                    loggedInUserRole = UserRoles.GroupHead;
                else if (role.Contains(UserRoles.AccountOwner))
                    loggedInUserRole = UserRoles.AccountOwner;
                else if (role.Contains(UserRoles.DeliveryManager))
                    loggedInUserRole = UserRoles.DeliveryManager;
                else if (role.Contains(UserRoles.Manager))
                    loggedInUserRole = UserRoles.Manager;

                string[] roles = new[] { "" };
                roles = new[] { UserRoles.ExpenseAdmin };
                string financeAdmin = string.Empty;
                foreach (string r in roles)
                {
                    string[] users = Roles.GetUsersInRole(r);

                    foreach (string user in users)
                    {
                        HRMS_tbl_PM_Employee employee = dal.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(user));
                        if (employee == null)
                            financeAdmin = financeAdmin + string.Empty;
                        else
                            financeAdmin = financeAdmin + employee.EmailID + ";";
                    }
                }
                ExpenseReimbursementDAL DAL = new ExpenseReimbursementDAL();
                ExpenseReimbursementStatus expenseDetails = new ExpenseReimbursementStatus();
                EmployeeDetailsViewModel employeeDetails = new EmployeeDetailsViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                EmployeeDetailsViewModel LoggedInUserDetails = DAL.getEmployeeDetailsForExpense(employeeDAL.GetEmployeeID(Membership.GetUser().UserName));

                expenseDetails.StageId = stageID;
                employeeDetails = DAL.getEmployeeDetailsForExpense(Convert.ToInt32(employeeID));
                expenseDetails.ExpenseId = Convert.ToInt32(decryptedExpenseID);
                expenseDetails.SecondaryApprover = secondaryApprover;
                expenseDetails.FinanceApprover = FinanceApprover;
                expenseDetails.PrimaryApprover = primaryApprover;
                expenseDetails.ExpenseFormName = Formname;
                //}
                ExpenseReimbursementStatus model = new ExpenseReimbursementStatus();
                model.Mail = new EmployeeMailTemplate();
                int templateId = 0;
                string mailBody = null;
                if (isApproveCall == true)
                {
                    if (expenseDetails.StageId == 1) //1st requester submits
                    {
                        HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetails(expenseDetails.PrimaryApprover.HasValue ? expenseDetails.PrimaryApprover.Value : 0);
                        model.Mail.To = employee.EmailID;
                        model.Mail.Cc = LoggedInUserDetails.EmailID;
                        model.Mail.From = employeeDetails.EmailID;
                        templateId = 35;
                        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                        foreach (var emailTemplate in template)
                        {
                            model.Mail.Subject = emailTemplate.Subject;
                            mailBody = emailTemplate.Message;
                        }
                        model.Mail.Subject = model.Mail.Subject.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);
                        mailBody = mailBody.Replace("##name of primary approver##", employee.EmployeeName);
                        mailBody = mailBody.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);
                        mailBody = mailBody.Replace("##logged in user##", LoggedInUserDetails.EmployeeName);
                        model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                    }
                    else if (expenseDetails.StageId == 2) //1st aprover aprrove
                    {
                        HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetails(expenseDetails.SecondaryApprover.HasValue ? expenseDetails.SecondaryApprover.Value : 0);
                        model.Mail.To = employee.EmailID;
                        model.Mail.Cc = employeeDetails.EmailID + ";" + LoggedInUserDetails.EmailID;
                        model.Mail.From = LoggedInUserDetails.EmailID;
                        templateId = 36;

                        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                        foreach (var emailTemplate in template)
                        {
                            model.Mail.Subject = emailTemplate.Subject;
                            mailBody = emailTemplate.Message;
                        }
                        model.Mail.Subject = model.Mail.Subject.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);
                        mailBody = mailBody.Replace("##name of secondary approver##", employee.EmployeeName);
                        mailBody = mailBody.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);
                        mailBody = mailBody.Replace("##logged in user##", LoggedInUserDetails.EmployeeName);
                        model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                    }
                    else if (expenseDetails.StageId == 3)  //2nd approver aprove
                    {
                        HRMS_tbl_PM_Employee SecondoryApprover = employeeDAL.GetEmployeeDetails(expenseDetails.SecondaryApprover.HasValue ? expenseDetails.SecondaryApprover.Value : 0);

                        if (loggedInUserRole == UserRoles.DeliveryManager || loggedInUserRole == UserRoles.AccountOwner || loggedInUserRole == UserRoles.GroupHead || loggedInUserRole == UserRoles.Manager)
                        {
                            model.Mail.From = LoggedInUserDetails.EmailID;
                            model.Mail.To = financeAdmin;
                            model.Mail.Cc = employeeDetails.EmailID + ";" + LoggedInUserDetails.EmailID;
                        }
                        else if (loggedInUserRole == UserRoles.Management)
                        {
                            model.Mail.To = financeAdmin;
                            model.Mail.Cc = employeeDetails.EmailID;
                            model.Mail.From = employeeDetails.EmailID;
                        }
                        else
                        {
                            model.Mail.From = SecondoryApprover.EmailID;
                            model.Mail.To = financeAdmin;
                            model.Mail.Cc = employeeDetails.EmailID + ";" + LoggedInUserDetails.EmailID;
                        }

                        templateId = 38;
                        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                        foreach (var emailTemplate in template)
                        {
                            model.Mail.Subject = emailTemplate.Subject;
                            mailBody = emailTemplate.Message;
                        }
                        model.Mail.Subject = model.Mail.Subject.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);

                        mailBody = mailBody.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);
                        mailBody = mailBody.Replace("##requestor name##", employeeDetails.EmployeeName);
                        mailBody = mailBody.Replace("##logged in user##", LoggedInUserDetails.EmployeeName);
                        model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                    }
                    else if (expenseDetails.StageId == 4) //finance aprover aproove
                    {
                        if (loggedInUserRole == UserRoles.DeliveryManager || loggedInUserRole == UserRoles.AccountOwner || loggedInUserRole == UserRoles.GroupHead || loggedInUserRole == UserRoles.Manager)
                        {
                            HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetails(expenseDetails.PrimaryApprover.HasValue ? expenseDetails.PrimaryApprover.Value : 0);
                            model.Mail.To = employeeDetails.EmailID;
                            model.Mail.From = LoggedInUserDetails.EmailID;
                            model.Mail.Cc = financeAdmin + ";" + employee.EmailID;
                        }
                        else if (loggedInUserRole == UserRoles.Management)
                        {
                            model.Mail.To = employeeDetails.EmailID;
                            model.Mail.From = LoggedInUserDetails.EmailID;
                            model.Mail.Cc = financeAdmin;
                        }
                        else
                        {
                            model.Mail.To = employeeDetails.EmailID; //req
                            model.Mail.From = LoggedInUserDetails.EmailID;//finance
                            model.Mail.Cc = financeAdmin; // all finance admins
                        }
                        templateId = 39;
                        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                        foreach (var emailTemplate in template)
                        {
                            model.Mail.Subject = emailTemplate.Subject;
                            mailBody = emailTemplate.Message;
                        }
                        mailBody = mailBody.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);
                        mailBody = mailBody.Replace("##logged in user##", LoggedInUserDetails.EmployeeName);
                        model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                    }
                }
                else if (IsRejectCall == true)
                {
                    if (expenseDetails.StageId == 0) // 1st aprover reject
                    {
                        HRMS_tbl_PM_Employee PrimaryApprover = employeeDAL.GetEmployeeDetails(expenseDetails.PrimaryApprover.HasValue ? expenseDetails.PrimaryApprover.Value : 0);
                        model.Mail.To = employeeDetails.EmailID;
                        model.Mail.Cc = PrimaryApprover.EmailID;
                        model.Mail.From = PrimaryApprover.EmailID;
                        templateId = 37;
                        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                        foreach (var emailTemplate in template)
                        {
                            model.Mail.Subject = emailTemplate.Subject;
                            mailBody = emailTemplate.Message;
                        }
                        model.Mail.Subject = model.Mail.Subject.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);
                        mailBody = mailBody.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);
                        mailBody = mailBody.Replace("##Rejection comments entered by approver/finance/admin##", comments);
                        mailBody = mailBody.Replace("##logged in user##", LoggedInUserDetails.EmployeeName);
                        model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                    }
                    else if (expenseDetails.StageId == 1) //2nd aprover rejects
                    {
                        HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetails(expenseDetails.PrimaryApprover.HasValue ? expenseDetails.PrimaryApprover.Value : 0);
                        HRMS_tbl_PM_Employee SecondoryApprover = employeeDAL.GetEmployeeDetails(expenseDetails.SecondaryApprover.HasValue ? expenseDetails.SecondaryApprover.Value : 0);
                        model.Mail.To = employeeDetails.EmailID;
                        model.Mail.Cc = employee.EmailID + ";" + SecondoryApprover.EmailID;
                        model.Mail.From = LoggedInUserDetails.EmailID;
                        templateId = 37;
                        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                        foreach (var emailTemplate in template)
                        {
                            model.Mail.Subject = emailTemplate.Subject;
                            mailBody = emailTemplate.Message;
                        }
                        model.Mail.Subject = model.Mail.Subject.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);
                        mailBody = mailBody.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);
                        mailBody = mailBody.Replace("##Rejection comments entered by approver/finance/admin##", null);
                        mailBody = mailBody.Replace("##logged in user##", LoggedInUserDetails.EmployeeName);
                        model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                    }
                    else if (expenseDetails.StageId == 2) //finace aprover rejecct
                    {
                        if (loggedInUserRole == UserRoles.Management)
                        {
                            model.Mail.To = employeeDetails.EmailID;
                            model.Mail.Cc = financeAdmin;
                            model.Mail.From = LoggedInUserDetails.EmailID;
                        }
                        else
                        {
                            HRMS_tbl_PM_Employee PriamaryApprover = employeeDAL.GetEmployeeDetails(expenseDetails.PrimaryApprover.HasValue ? expenseDetails.PrimaryApprover.Value : 0);
                            HRMS_tbl_PM_Employee SecondoryApprover = employeeDAL.GetEmployeeDetails(expenseDetails.SecondaryApprover.HasValue ? expenseDetails.SecondaryApprover.Value : 0);
                            model.Mail.To = employeeDetails.EmailID;
                            model.Mail.Cc = PriamaryApprover.EmailID + ";" + SecondoryApprover.EmailID + ";" + financeAdmin;
                            model.Mail.From = LoggedInUserDetails.EmailID;
                        }
                        templateId = 37;

                        List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                        foreach (var emailTemplate in template)
                        {
                            model.Mail.Subject = emailTemplate.Subject;
                            mailBody = emailTemplate.Message;
                        }
                        model.Mail.Subject = model.Mail.Subject.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);
                        mailBody = mailBody.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);
                        mailBody = mailBody.Replace("##Rejection comments entered by approver/finance/admin##", null);
                        mailBody = mailBody.Replace("##logged in user##", LoggedInUserDetails.EmployeeName);
                        model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                    }
                }
                if (IsCancelled == true)
                {
                    HRMS_tbl_PM_Employee primaryApproverDetails = employeeDAL.GetEmployeeDetails(expenseDetails.PrimaryApprover.HasValue ? expenseDetails.PrimaryApprover.Value : 0);
                    HRMS_tbl_PM_Employee SecondaryApproverDetails = employeeDAL.GetEmployeeDetails(expenseDetails.SecondaryApprover.HasValue ? expenseDetails.SecondaryApprover.Value : 0);
                    if (primaryApproverDetails != null && SecondaryApproverDetails != null)
                        model.Mail.To = primaryApproverDetails.EmailID + ";" + SecondaryApproverDetails.EmailID + ";" + financeAdmin + ";" + LoggedInUserDetails.EmailID + ";" + employeeDetails.EmailID;
                    else if (primaryApproverDetails == null && SecondaryApproverDetails != null)
                        model.Mail.To = SecondaryApproverDetails.EmailID + ";" + financeAdmin + ";" + LoggedInUserDetails.EmailID + ";" + employeeDetails.EmailID;
                    else if (SecondaryApproverDetails == null && primaryApproverDetails != null)
                        model.Mail.To = primaryApproverDetails.EmailID + ";" + financeAdmin + ";" + LoggedInUserDetails.EmailID + ";" + employeeDetails.EmailID;
                    else
                        model.Mail.To = financeAdmin + ";" + LoggedInUserDetails.EmailID + ";" + employeeDetails.EmailID;

                    if (expenseDetails.StageId != 3)
                        model.Mail.From = employeeDetails.EmailID;
                    else
                        model.Mail.From = LoggedInUserDetails.EmailID;
                    templateId = 40;
                    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                    foreach (var emailTemplate in template)
                    {
                        model.Mail.Subject = emailTemplate.Subject;
                        mailBody = emailTemplate.Message;
                    }
                    model.Mail.Subject = model.Mail.Subject.Replace("##Reimbursement Form Name##", expenseDetails.ExpenseFormName);
                    mailBody = mailBody.Replace("##Reimbursement Form Name##", Formname);
                    mailBody = mailBody.Replace("##Cancellation comments entered by user/approver/finance/admin##", comments);
                    mailBody = mailBody.Replace("##logged in user##", LoggedInUserDetails.EmployeeName);
                    model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                    model.Mail.Cc = LoggedInUserDetails.EmailID;
                }

                result = SendMail(model.Mail);
                //result = true;
                return Json(new { status = result, validCcId = true, validtoId = true });
            }
            catch (SmtpFailedRecipientException e)
            {
                string email = null;
                char[] symbols = { '<', '>' };
                if (e.Message != null)
                {
                    email = e.Message;
                    email = email.Trim(symbols);
                }
                return Json(new { status = "ErrorRecipient", failedRecipient = email }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool SendMail(EmployeeMailTemplate model)
        {
            SMTPHelper smtpHelper = new SMTPHelper();
            char[] symbols = new char[] { ';', ' ', ',', '\r', '\n' };
            if (model != null)
            {
                string[] ToEmailId = model.To.Split(symbols);
                string[] CCEmailIds = null;
                if (model.Cc != "" && model.Cc != null)
                    CCEmailIds = model.Cc.Split(symbols);
                string[] CCEmailId = CCEmailIds.Where(s => !String.IsNullOrEmpty(s)).ToArray();
                return smtpHelper.SendMail(ToEmailId, null, CCEmailId, null, null, null, model.From, null, model.Subject, model.Message, null, null);
            }
            else
                return false;
        }

        [HttpPost]
        public ActionResult ValidateFormName(string Formname)
        {
            try
            {
                ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
                bool status = dal.GetFromNameStatus(Formname);
                return Json(status);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult ValidateFormCode(string FormCode)
        {
            try
            {
                ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
                bool status = dal.GetFromCodeStatus(FormCode);
                int formCode = dal.GetFormCode();
                if (status == true)
                    return Json(new { status = true, newFormCode = formCode });
                else
                    return Json(new { status = false, newFormCode = formCode });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult DeleteAllReimbursementDetailsByExpenseId(int expenseId, string expenseEmployeeId, string comments)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(expenseEmployeeId, out isAuthorize);

                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();

                PersonalDetailsDAL personalDetails = new PersonalDetailsDAL();
                int expenseCode = Convert.ToInt32(personalDetails.getEmployeeCode(Convert.ToInt32(decryptedEmployeeId)));

                ExpenseReimbursementStatus _expeseDetails = dal.getExpenseDetails(expenseId);
                int? stageID = _expeseDetails.StageId;
                int? primaryApprover = _expeseDetails.PrimaryApprover;
                int? SecondaryApprover = _expeseDetails.SecondaryApprover;
                int? FinanceApprover = _expeseDetails.FinanceApprover;
                string Formname = Convert.ToString(_expeseDetails.FormCode);
                bool status = dal.DeletedAllExpenseDetails(expenseId, decryptedEmployeeId);
                string expenseID = Commondal.Encrypt(Session["SecurityKey"].ToString() + expenseId, true);
                return Json(new { status = status, expenseID = expenseID, employeeID = decryptedEmployeeId, stageID = stageID, primaryApprover = primaryApprover, secondaryApprover = SecondaryApprover, FinanceApprover = FinanceApprover, Formname = Formname, comments = comments, ExpenseCode = expenseCode }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveComments(int expenseId, string comments, string commenttype)
        {
            ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
            try
            {
                string resultMessage = string.Empty;

                var status = dal.SaveCommentDetails(expenseId, comments, commenttype);

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

        [HttpGet]
        public ActionResult ConfigureExpenseReimbProjectNames()
        {
            try
            {
                ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
                ExpenseProjectNamesModel model = new ExpenseProjectNamesModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                string EmployeeCode = Membership.GetUser().UserName;
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeCode = EmployeeCode;
                model.SearchedUserDetails.EmployeeId = employeeDAL.GetEmployeeID(EmployeeCode);
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                model.ExpProjectNamesList = dal.GetAllExpenseReimbProjectNamesList();
                model.TotalExpProjects = model.ExpProjectNamesList.Count;
                return PartialView("_ConfigureExpenseReimbProjectNames", model);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult AddEditNewProject(int expProjectId, string expProjectName, int employeeId)
        {
            try
            {
                ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
                ExpenseProjectNamesModel model = new ExpenseProjectNamesModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                tbl_Client_ProjectNamesMaster ProjectDetails = dal.getExpProjectDetails(expProjectId);
                if (ProjectDetails != null)
                {
                    model.ProjectNameID = ProjectDetails.ProjectNameID;
                    model.NewProjectName = ProjectDetails.ProjectName;
                    model.NewExpProjectDescription = ProjectDetails.ProjectDescription;
                    model.ExistingExpProjectName = ProjectDetails.ProjectName;
                }
                else
                {
                    model.ProjectNameID = expProjectId;
                    model.NewProjectName = expProjectName;
                    model.NewExpProjectDescription = "";
                    model.ExistingExpProjectName = "";
                }
                model.SearchedUserDetails.EmployeeId = employeeId;
                return PartialView("_AddEditConfigureExpProjects", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult DeleteProject(int expProjectId)
        {
            try
            {
                ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
                ExpenseReimbProcessResponse response = new ExpenseReimbProcessResponse();
                response.isDeleted = false;
                if (expProjectId != 0)
                {
                    response = dal.DeleteProject(expProjectId);
                }
                return Json(new { isDeleted = response.isDeleted }, JsonRequestBehavior.AllowGet);
            }
            catch (UpdateException)
            {
                return Json(new { isDeleted = "UpdateException" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddEditProject(ExpenseProjectNamesModel model)
        {
            try
            {
                ExpenseReimbursementDAL dal = new ExpenseReimbursementDAL();
                ExpenseReimbProcessResponse Response = new ExpenseReimbProcessResponse();
                Response.isAdded = false;
                if (model.NewProjectName != null)
                {
                    Response = dal.AddEditNewProject(model);
                }
                return Json(new { isAdded = Response.isAdded, isExisted = Response.isExisted }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ConfigureExpenseReimbDetails()
        {
            try
            {
                ExpenseReimbursementViewModel model = new ExpenseReimbursementViewModel();
                string EmployeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                model.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                model.SearchedUserDetails.EmployeeCode = EmployeeCode;
                EmployeeDAL DAL = new EmployeeDAL();
                model.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
                return View(model);
            }
            catch
            {
                throw;
            }
        }
    }
}