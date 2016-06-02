using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace HRMS.DAL
{
    public class ExpenseReimbursementDAL
    {
        private HRMSDBEntities dbContext = new HRMSDBEntities();
        private WSEMDBEntities dbSEMContext = new WSEMDBEntities();

        //Get Delivery team name for an employee
        public tbl_PM_GroupMaster GetDeliveryTeamName(int groupId)
        {
            try
            {
                tbl_PM_GroupMaster empDetails = dbContext.tbl_PM_GroupMaster.Where(ed => ed.GroupID == groupId).FirstOrDefault();
                return empDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ClientReimbursementList> clientReimbursementList()
        {
            List<ClientReimbursementList> clientReimbursement = new List<ClientReimbursementList>();
            try
            {
                clientReimbursement = (from client in dbContext.Tbl_HR_Client_ReimbursementStatusMaster
                                       select new ClientReimbursementList
                                       {
                                           ClientReimbursementId = client.ID,
                                           ClientReimbursementValue = client.Description
                                       }).ToList();
                return clientReimbursement;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CostCentreList> costCentreList()
        {
            List<CostCentreList> costcentre = new List<CostCentreList>();
            try
            {
                costcentre = (from c in dbContext.tbl_PM_TravelCostCenter
                              orderby c.TravelCostCenterName
                              select new CostCentreList
                              {
                                  CostCentreID = c.TravelCostCenterID,
                                  CostCentreName = c.TravelCostCenterName
                              }).ToList();
                return costcentre;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CurrencyList> currencyList()
        {
            List<CurrencyList> currency = new List<CurrencyList>();
            try
            {
                currency = (from c in dbContext.tbl_HR_Currency
                            select new CurrencyList
                            {
                                CurrencyID = c.CurrencyID,
                                CurrencyName = c.CurrencyName
                            }).ToList();
                return currency;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_HR_Expense GetExpenseDetails(int employeeId, int expenseId)
        {
            try
            {
                tbl_HR_Expense empDetails = dbContext.tbl_HR_Expense.Where(ed => ed.EmployeeID == employeeId && ed.ExpenseID == expenseId).FirstOrDefault();
                return empDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_PM_OfficeLocation GetLoacation(int locationId)
        {
            try
            {
                tbl_PM_OfficeLocation empDetails = dbContext.tbl_PM_OfficeLocation.Where(ed => ed.OfficeLocationID == locationId).FirstOrDefault();
                return empDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NatureOfExpenseList> GetNatureOfExpense()
        {
            List<NatureOfExpenseList> clientReimbursement = new List<NatureOfExpenseList>();
            try
            {
                clientReimbursement = (from expense in dbContext.Tbl_HR_NatureOfExpense
                                       select new NatureOfExpenseList
                                       {
                                           NatureOfExpenseId = expense.ID,
                                           NatureOfExpensevalue = expense.NatureOfExpense
                                       }).ToList();
                return clientReimbursement;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ExpenseReimbursementDetails> ExpenseDetailRecord(int employeeId, int expenseId, int page, int rows, out int totalCount, string encryptedEmployeeId)
        {
            List<ExpenseReimbursementDetails> expenseRecords = new List<ExpenseReimbursementDetails>();

            try
            {
                expenseRecords = (from expenses in dbContext.Tbl_HR_ExpenseDetail
                                  join natureOfExpense in dbContext.Tbl_HR_NatureOfExpense on expenses.NatureOfExpenseID equals natureOfExpense.ID
                                  where expenses.EmployeeId == employeeId && expenses.ExpenseID == expenseId
                                  select new ExpenseReimbursementDetails
                                  {
                                      ExpenseDetailsId = expenses.ID,
                                      ReceiptNo = expenses.ReceiptNo,
                                      DateOfExpense = expenses.DateOfExpense,
                                      NatureOfExpense = natureOfExpense.NatureOfExpense,
                                      Details = expenses.Details,
                                      Amount = expenses.Ammount,
                                      Verify = expenses.IsChecked.HasValue ? expenses.IsChecked.Value : false,
                                      Comments = expenses.Comments,
                                      EncryptedEmployeeId = encryptedEmployeeId,
                                      FileName = expenses.FileName,
                                      FilePath = expenses.FilePath
                                  }).ToList();

                //expenseRecords = (from expenses in dbContext.Tbl_HR_ExpenseDetail
                //                  where expenses.ExpenseID == expenseId
                //                  select new ExpenseReimbursementDetails
                //                  {
                //                      ExpenseId = expenses.ID
                //                  }).ToList();

                //amountRecords[1].Details = "Total";

                totalCount = expenseRecords.Count();
                return expenseRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal? CalculateTotalExpense(int employeeId, int expenseId)
        {
            List<ExpenseReimbursementDetails> expenseRecords = new List<ExpenseReimbursementDetails>();
            try
            {
                decimal? totalAmount = (from expenses in dbContext.Tbl_HR_ExpenseDetail
                                        join natureOfExpense in dbContext.Tbl_HR_NatureOfExpense on expenses.NatureOfExpenseID equals natureOfExpense.ID
                                        where expenses.EmployeeId == employeeId && expenses.ExpenseID == expenseId
                                        select expenses.Ammount).Sum();

                // totalCount = expenseRecords.Count();
                return totalAmount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeletedExpenseDetailsForNewForm(int ExpenseID, int employeeId)
        {
            bool isDeleted = false;
            Tbl_HR_ExpenseDetail expenseInfo = dbContext.Tbl_HR_ExpenseDetail.Where(cd => cd.ExpenseID == ExpenseID && cd.EmployeeId == employeeId).FirstOrDefault();
            if (expenseInfo != null)
            {
                dbContext.DeleteObject(expenseInfo);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public bool DeletedExpenseDetails(int ExpenseID, int employeeId)
        {
            bool isDeleted = false;
            Tbl_HR_ExpenseDetail expenseInfo = dbContext.Tbl_HR_ExpenseDetail.Where(cd => cd.ID == ExpenseID).FirstOrDefault();
            if (ExpenseID != null && ExpenseID > 0)
            {
                dbContext.DeleteObject(expenseInfo);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public bool RejectExpenseApprovalForm(ExpenseReimbursementViewModel empExpense, int employeeId)
        {
            bool isDeleted = false;
            tbl_HR_Expense expenseInfo = dbContext.tbl_HR_Expense.Where(cd => cd.ExpenseID == empExpense.ExpenseId).FirstOrDefault();
            if (expenseInfo != null && expenseInfo.ExpenseID > 0)
            {
                expenseInfo.StageID = 0;
                Tbl_HR_ExpenseStageEvent emp = new Tbl_HR_ExpenseStageEvent();
                emp.ExpenseID = empExpense.ExpenseId;
                emp.EventDatatime = DateTime.Now;
                emp.Action = "Rejected";
                emp.FromStageId = empExpense.StageID;
                emp.ToStageId = 0;
                emp.UserId = employeeId;
                emp.Comments = empExpense.RejectComments;
                dbContext.Tbl_HR_ExpenseStageEvent.AddObject(emp);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public bool SaveExpenseDetailsFormAdmin(ExpenseReimbursementViewModel empExpense)
        {
            bool isAdded = false;
            try
            {
                tbl_HR_Expense emp = dbContext.tbl_HR_Expense.Where(ed => ed.ExpenseID == empExpense.ExpenseId).FirstOrDefault();
                tbl_HR_Expense expense = new tbl_HR_Expense();
                if (emp != null)
                {
                    emp.ChequeDetails = empExpense.ChequeDetails;
                    emp.ReimbursementFormCode = empExpense.ReimbursementFormCode;
                    emp.Balance = empExpense.Balance;
                    emp.Advance = empExpense.Advances;
                    emp.AmountInWords = empExpense.AmountInWords;
                }
                dbContext.SaveChanges();
                isAdded = true;
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ExpenseReimbProcessResponse SaveExpenseApprovalForm(ExpenseReimbursementViewModel empExpense, Dictionary<int, bool> details)
        {
            try
            {
                ExpenseReimbProcessResponse response = new ExpenseReimbProcessResponse();
                response.isAdded = false;
                tbl_HR_Expense emp = dbContext.tbl_HR_Expense.Where(ed => ed.ExpenseID == empExpense.ExpenseId).FirstOrDefault();
                if (emp == null || emp.ExpenseID <= 0)
                {
                    tbl_HR_Expense expense = new tbl_HR_Expense();
                    expense.EmployeeID = empExpense.ReimbursementEmployeeId;
                    expense.StageID = empExpense.StageID;
                    expense.ReimbursementFormName = empExpense.ReimbursementFormName;
                    expense.ReimbursementFormCode = empExpense.ReimbursementFormCode;
                    expense.ProjectName = empExpense.ProjectName;
                    expense.IsClientReimbursement = empExpense.ClientReimbursement;
                    expense.ClientName = empExpense.clientName;
                    expense.DateOfSubmission = DateTime.Now;
                    expense.CreatedDate = DateTime.Now;
                    expense.Total = empExpense.Total;
                    expense.Balance = empExpense.Balance;
                    expense.Advance = empExpense.Advances;
                    expense.AmountInWords = empExpense.AmountInWords;
                    expense.PrimaryApprover = empExpense.PrimaryApprover;
                    expense.SecondaryApprover = empExpense.SecondaryApprover;
                    expense.FinanceApprover = empExpense.FinanceApprover;
                    expense.IsAdvanceApprove = empExpense.IsAdvanceApproved;
                    expense.IsTotalApprove = empExpense.IsTotalApprove;
                    expense.IsBalanceApprove = empExpense.IsBalanceApproved;
                    expense.TravelCostCenterID = empExpense.CostCentre;
                    expense.CurrencyID = empExpense.Currency;
                    expense.FormCode = empExpense.FormCode;
                    if (empExpense.ChequeDetails != null && empExpense.ChequeDetails != "")
                        expense.ChequeDetails = empExpense.ChequeDetails.Trim();
                    else
                        expense.ChequeDetails = empExpense.ChequeDetails;
                    dbContext.tbl_HR_Expense.AddObject(expense);
                }
                else
                {
                    emp.EmployeeID = empExpense.ReimbursementEmployeeId;
                    emp.StageID = empExpense.StageID;
                    emp.ReimbursementFormName = empExpense.ReimbursementFormName;
                    emp.ReimbursementFormCode = empExpense.ReimbursementFormCode;
                    emp.ProjectName = empExpense.ProjectName;
                    emp.IsClientReimbursement = empExpense.ClientReimbursement;
                    emp.ClientName = empExpense.clientName;
                    emp.DateOfSubmission = DateTime.Now;
                    emp.Total = empExpense.Total;
                    emp.Balance = empExpense.Balance;
                    emp.Advance = empExpense.Advances;
                    emp.AmountInWords = empExpense.AmountInWords;
                    emp.PrimaryApprover = empExpense.PrimaryApprover;
                    emp.SecondaryApprover = empExpense.SecondaryApprover;
                    emp.FinanceApprover = empExpense.FinanceApprover;
                    emp.IsAdvanceApprove = empExpense.IsAdvanceApproved;
                    emp.IsTotalApprove = empExpense.IsTotalApprove;
                    emp.IsBalanceApprove = empExpense.IsBalanceApproved;
                    emp.TravelCostCenterID = empExpense.CostCentre;
                    emp.CurrencyID = empExpense.Currency;
                    emp.FormCode = empExpense.FormCode;
                    if (empExpense.ChequeDetails != null && empExpense.ChequeDetails != "")
                        emp.ChequeDetails = empExpense.ChequeDetails.Trim();
                    else
                        emp.ChequeDetails = empExpense.ChequeDetails;
                }
                dbContext.SaveChanges();
                response.isAdded = true;
                tbl_HR_Expense latestExpenseDetail;
                if (empExpense.ExpenseId == 0)
                {
                    latestExpenseDetail = (from expense in dbContext.tbl_HR_Expense
                                           where expense.EmployeeID == empExpense.ReimbursementEmployeeId
                                           orderby expense.CreatedDate descending
                                           select expense).FirstOrDefault();
                }
                else
                {
                    latestExpenseDetail = (from expense in dbContext.tbl_HR_Expense
                                           where expense.EmployeeID == empExpense.ReimbursementEmployeeId && expense.ExpenseID == empExpense.ExpenseId
                                           orderby expense.CreatedDate descending
                                           select expense).FirstOrDefault();
                }
                response.latestExpenseID = latestExpenseDetail.ExpenseID;
                List<Tbl_HR_ExpenseDetail> expenseDetails = dbContext.Tbl_HR_ExpenseDetail.Where(ed => ed.EmployeeId == empExpense.ReimbursementEmployeeId && ed.ExpenseID == 0).ToList();
                foreach (var item in expenseDetails)
                {
                    item.ExpenseID = latestExpenseDetail.ExpenseID;
                    dbContext.SaveChanges();
                    response.isAdded = true;
                }

                foreach (var expenseDetail in details)
                {
                    Tbl_HR_ExpenseDetail newExpenseDetails = dbContext.Tbl_HR_ExpenseDetail.Where(x => x.ID == expenseDetail.Key).FirstOrDefault();
                    if (newExpenseDetails != null)
                    {
                        newExpenseDetails.IsChecked = expenseDetail.Value;
                        dbContext.SaveChanges();
                        response.isAdded = true;
                    }
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SubmitExpenseApprovalForm(ExpenseReimbursementViewModel empExpense, int loginEmpId, string loggedInUserRole)
        {
            bool isAdded = false;
            try
            {
                tbl_HR_Expense latestExpenseDetail = (from expense in dbContext.tbl_HR_Expense
                                                      where expense.EmployeeID == empExpense.ReimbursementEmployeeId
                                                      && expense.FormCode == empExpense.FormCode
                                                      select expense).FirstOrDefault();

                Tbl_HR_ExpenseStageEvent emp = new Tbl_HR_ExpenseStageEvent();
                emp.ExpenseID = latestExpenseDetail.ExpenseID;
                emp.EventDatatime = DateTime.Now;
                emp.Action = "Approved";
                emp.UserId = loginEmpId;
                emp.FromStageId = latestExpenseDetail.StageID;

                if ((loggedInUserRole == UserRoles.DeliveryManager || loggedInUserRole == UserRoles.AccountOwner ||
                     loggedInUserRole == UserRoles.GroupHead || loggedInUserRole == UserRoles.Manager) && empExpense.StageID == 0)
                    emp.ToStageId = latestExpenseDetail.StageID + 1;
                else if ((loggedInUserRole == UserRoles.DeliveryManager || loggedInUserRole == UserRoles.AccountOwner ||
                          loggedInUserRole == UserRoles.GroupHead || loggedInUserRole == UserRoles.Manager) && empExpense.StageID == 1)
                    emp.ToStageId = 3;
                else if (loggedInUserRole == UserRoles.Management && empExpense.StageID == 0)
                    emp.ToStageId = 3;
                else
                    emp.ToStageId = latestExpenseDetail.StageID + 1;

                dbContext.Tbl_HR_ExpenseStageEvent.AddObject(emp);
                dbContext.SaveChanges();

                if ((loggedInUserRole == UserRoles.DeliveryManager || loggedInUserRole == UserRoles.AccountOwner ||
                     loggedInUserRole == UserRoles.GroupHead || loggedInUserRole == UserRoles.Manager) && empExpense.StageID == 0)
                    latestExpenseDetail.StageID = latestExpenseDetail.StageID + 1;
                else if ((loggedInUserRole == UserRoles.DeliveryManager || loggedInUserRole == UserRoles.AccountOwner ||
                          loggedInUserRole == UserRoles.GroupHead || loggedInUserRole == UserRoles.Manager) && empExpense.StageID == 1)
                    latestExpenseDetail.StageID = 3;
                else if (loggedInUserRole == UserRoles.Management && empExpense.StageID == 0)
                    latestExpenseDetail.StageID = 3;
                else
                    latestExpenseDetail.StageID = latestExpenseDetail.StageID + 1;

                //latestExpenseDetail.StageID = latestExpenseDetail.StageID + 1;
                latestExpenseDetail.IsTotalApprove = false;
                latestExpenseDetail.IsBalanceApprove = false;
                latestExpenseDetail.IsAdvanceApprove = false;
                dbContext.SaveChanges();

                List<Tbl_HR_ExpenseDetail> expenses = dbContext.Tbl_HR_ExpenseDetail.Where(x => x.ExpenseID == latestExpenseDetail.ExpenseID).ToList();
                foreach (var item in expenses)
                {
                    item.IsChecked = false;
                    dbContext.SaveChanges();
                }
                isAdded = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        public bool SaveExpenseDetails(ExpenseReimbursementViewModel empExpense, int ExpenseId, string EncryptedEmployeeId, string EncryptedExpenseId, int ReimbursementEmployeeId, string UploadedFileName, string UploadedFilePath)
        {
            bool isAdded = false;

            Tbl_HR_ExpenseDetail emp = dbContext.Tbl_HR_ExpenseDetail.Where(ed => ed.ID == empExpense.ExpenseDetailsId).FirstOrDefault();
            if (emp == null || emp.ID <= 0)
            {
                Tbl_HR_ExpenseDetail expense = new Tbl_HR_ExpenseDetail();
                expense.ExpenseID = empExpense.ExpenseId;
                //expense.EmployeeId = empExpense.ReimbursementEmployeeId;
                expense.EmployeeId = ReimbursementEmployeeId;
                expense.ReceiptNo = empExpense.ReceiptNo;
                expense.DateOfExpense = empExpense.DateOfExpense;
                expense.Details = empExpense.Details;
                expense.Ammount = empExpense.Amount;
                expense.Comments = empExpense.Comments;
                expense.NatureOfExpenseID = empExpense.NatureOfExpense;
                expense.FileName = UploadedFileName;
                expense.FilePath = UploadedFilePath;

                dbContext.Tbl_HR_ExpenseDetail.AddObject(expense);
            }
            else
            {
                emp.ID = empExpense.ExpenseDetailsId;
                //emp.EmployeeId = empExpense.ReimbursementEmployeeId;
                emp.EmployeeId = ReimbursementEmployeeId;
                emp.ReceiptNo = empExpense.ReceiptNo;
                emp.DateOfExpense = empExpense.DateOfExpense;
                emp.Details = empExpense.Details;
                emp.Ammount = empExpense.Amount;
                emp.Comments = empExpense.Comments;
                emp.NatureOfExpenseID = empExpense.NatureOfExpense;
                if (!string.IsNullOrEmpty(UploadedFileName))
                    emp.FileName = UploadedFileName.Trim();
                if (!string.IsNullOrEmpty(UploadedFilePath))
                    emp.FilePath = UploadedFilePath.Trim();
                //emp.NatureOfExpenseID = (from nature in dbContext.Tbl_HR_NatureOfExpense
                //                         where nature.NatureOfExpense == empExpense.NatureOfExpense
                //                    select nature.ID).FirstOrDefault();
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        //public List<PrimaryApproverList> primaryApproversList()
        //{
        //    List<PrimaryApproverList> primaryApprovers = new List<PrimaryApproverList>();
        //     EmployeeDAL employeeDAL = new EmployeeDAL();
        //    try
        //    {
        //        string[] users = Roles.GetUsersInRole("Expense_Approver");
        //            foreach (string userlist in users)
        //            {
        //                HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsByEmployeeCode(userlist);
        //                if (employee != null)
        //                    primaryApprovers.Add(new PrimaryApproverList
        //                    {
        //                        PrimaryApproverId = e
        //                    }
        //            }
        //        primaryApprovers = (from primary in tbl_)
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public List<ExpenseReimbursementStatus> GetInboxListDetails(string searchText, string field, string fieldChild, int page, int rows, int employeeId, out int totalCount)
        {
            List<ExpenseReimbursementStatus> mainResult = new List<ExpenseReimbursementStatus>();
            List<ExpenseReimbursementStatus> employeeresult = new List<ExpenseReimbursementStatus>();
            List<ExpenseReimbursementStatus> PriApproverCheck = new List<ExpenseReimbursementStatus>();
            List<ExpenseReimbursementStatus> SecApproverCheck = new List<ExpenseReimbursementStatus>();
            List<ExpenseReimbursementStatus> FinApproverCheck = new List<ExpenseReimbursementStatus>();

            try
            {
                int FieldChild = 0;
                if (fieldChild != "" && fieldChild != "undefined")
                {
                    FieldChild = Convert.ToInt32(fieldChild) - 1;
                }
                string LogeedInEmCode = string.Empty;
                string[] LogeedInEmRoles = { };
                EmployeeDAL empdal = new EmployeeDAL();

                HRMS_tbl_PM_Employee employeeDetails = empdal.GetEmployeeDetails(employeeId);
                if (employeeDetails != null && employeeDetails.EmployeeID > 0)
                {
                    LogeedInEmCode = employeeDetails.EmployeeCode;
                    LogeedInEmRoles = Roles.GetRolesForUser(LogeedInEmCode);
                }

                //this logic is for employee himself logins what falls under his Inbox bucket.ie. his own record.

                #region Employee Inbox Section

                employeeresult = (from E in dbContext.tbl_HR_Expense
                                  join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                  from ex in exp.DefaultIfEmpty()
                                  join s in dbContext.tbl_HR_ExpenseStages on E.StageID + 1 equals s.StageID into st
                                  from extstage in st.DefaultIfEmpty()
                                  where E.EmployeeID == employeeId && E.StageID == 0 && (E.IsCancelled == false || E.IsCancelled == null)
                                       && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                       && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                  join ese in dbContext.Tbl_HR_ExpenseStageEvent on E.ExpenseID equals ese.ExpenseID into eventStageRecord  // Fix to add red Image support

                                  select new ExpenseReimbursementStatus
                                  {
                                      Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                      ReportingTo = ex.ReportingTo,
                                      ExpenseId = E.ExpenseID,
                                      StageId = E.StageID,
                                      ExpenseStageOrder = E.StageID,
                                      stageName = extstage.ExpenseStage,
                                      EmployeeId = E.EmployeeID,
                                      Employeename = ex.EmployeeName,
                                      ExpenseFormName = E.ReimbursementFormName,
                                      ExpenseFormCode = E.ReimbursementFormCode,
                                      FormCode = E.FormCode
                                  }).Distinct().OrderByDescending(exid => exid.ExpenseId).ToList();

                #endregion Employee Inbox Section

                // following logic for checking manager login & any entries fall under managers watchlist bucket

                #region For Primary Approver Inbox Section

                PriApproverCheck = (from E in dbContext.tbl_HR_Expense
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                    join s in dbContext.tbl_HR_ExpenseStages on E.StageID + 1 equals s.StageID
                                    where E.PrimaryApprover == employeeId && E.StageID == 1 && (E.IsCancelled == false || E.IsCancelled == null)
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                         && (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.Tbl_HR_ExpenseStageEvent on E.ExpenseID equals ese.ExpenseID into eventStageRecord  // Fix to add red Image support

                                    select new ExpenseReimbursementStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                        ReportingTo = emp.ReportingTo,
                                        ExpenseId = E.ExpenseID,
                                        StageId = E.StageID,
                                        ExpenseStageOrder = E.StageID,
                                        stageName = s.ExpenseStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = emp.EmployeeName,
                                        ExpenseFormName = E.ReimbursementFormName,
                                        ExpenseFormCode = E.ReimbursementFormCode,
                                        FormCode = E.FormCode
                                    }).Distinct().OrderByDescending(exid => exid.ExpenseId).ToList();

                #endregion For Primary Approver Inbox Section

                // following logic is to check what falls under HR Admins watchlist ie.
                // HR Admin will handle HR Approval,Exit Interview, Hr Closure, Exit stages.

                #region Secondary approver Inbox Section

                SecApproverCheck = (from E in dbContext.tbl_HR_Expense
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                    join s in dbContext.tbl_HR_ExpenseStages on E.StageID + 1 equals s.StageID
                                    where E.SecondaryApprover == employeeId && E.StageID == 2 && (E.IsCancelled == false || E.IsCancelled == null)
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                         && (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.Tbl_HR_ExpenseStageEvent on E.ExpenseID equals ese.ExpenseID into eventStageRecord  // Fix to add red Image support

                                    select new ExpenseReimbursementStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                        ReportingTo = emp.ReportingTo,
                                        ExpenseId = E.ExpenseID,
                                        StageId = E.StageID,
                                        ExpenseStageOrder = E.StageID,
                                        stageName = s.ExpenseStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = emp.EmployeeName,
                                        ExpenseFormName = E.ReimbursementFormName,
                                        ExpenseFormCode = E.ReimbursementFormCode,
                                        FormCode = E.FormCode
                                    }).Distinct().OrderByDescending(exid => exid.ExpenseId).ToList();

                #endregion Secondary approver Inbox Section

                #region Finance Approval stage Inbox Section

                FinApproverCheck = (from E in dbContext.tbl_HR_Expense
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                    join s in dbContext.tbl_HR_ExpenseStages on E.StageID + 1 equals s.StageID
                                    where LogeedInEmRoles.Contains(UserRoles.ExpenseAdmin) && E.StageID == 3 && (E.IsCancelled == false || E.IsCancelled == null)
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                         && (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.Tbl_HR_ExpenseStageEvent on E.ExpenseID equals ese.ExpenseID into eventStageRecord  // Fix to add red Image support

                                    select new ExpenseReimbursementStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                        ReportingTo = emp.ReportingTo,
                                        ExpenseId = E.ExpenseID,
                                        StageId = E.StageID,
                                        ExpenseStageOrder = E.StageID,
                                        stageName = s.ExpenseStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = emp.EmployeeName,
                                        ExpenseFormName = E.ReimbursementFormName,
                                        ExpenseFormCode = E.ReimbursementFormCode,
                                        FormCode = E.FormCode
                                    }).Distinct().OrderByDescending(exid => exid.ExpenseId).ToList();

                #endregion Finance Approval stage Inbox Section

                mainResult = employeeresult.Union(employeeresult).Union(PriApproverCheck).Union(SecApproverCheck).Union(FinApproverCheck).ToList();
                totalCount = mainResult.Count;
                //return mainResult.Skip((page - 1) * rows).Take(rows).ToList();
                return mainResult.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ExpenseReimbursementStatus> GetWatchListDetails(string searchText, string field, string fieldChild, int page, int rows, int employeeId, out int totalCount)
        {
            List<ExpenseReimbursementStatus> mainResult = new List<ExpenseReimbursementStatus>();
            List<ExpenseReimbursementStatus> employeeresult = new List<ExpenseReimbursementStatus>();
            List<ExpenseReimbursementStatus> PriApproverCheck = new List<ExpenseReimbursementStatus>();
            List<ExpenseReimbursementStatus> SecApproverCheck = new List<ExpenseReimbursementStatus>();
            List<ExpenseReimbursementStatus> FinApproverCheck = new List<ExpenseReimbursementStatus>();

            try
            {
                int FieldChild = 0;
                if (fieldChild != "")
                {
                    FieldChild = Convert.ToInt32(fieldChild) - 1;
                }
                string LogeedInEmCode = string.Empty;
                string[] LogeedInEmRoles = { };
                EmployeeDAL empdal = new EmployeeDAL();

                HRMS_tbl_PM_Employee employeeDetails = empdal.GetEmployeeDetails(employeeId);
                if (employeeDetails != null && employeeDetails.EmployeeID > 0)
                {
                    LogeedInEmCode = employeeDetails.EmployeeCode;
                    LogeedInEmRoles = Roles.GetRolesForUser(LogeedInEmCode);
                }

                //this logic is for employee himself logins what falls under his watchlist bucket.ie. his own record.

                #region Employee Watchlist Section

                employeeresult = (from E in dbContext.tbl_HR_Expense
                                  join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                  join s in dbContext.tbl_HR_ExpenseStages on E.StageID + 1 equals s.StageID
                                  where E.EmployeeID == employeeId && E.StageID != 0 && (E.IsCancelled == false || E.IsCancelled == null) && (((E.StageID == 1) && (E.PrimaryApprover != E.EmployeeID)) || ((E.StageID == 2) && (E.SecondaryApprover != E.EmployeeID)) || ((E.StageID == 3) && (E.FinanceApprover == null)) || (E.StageID == 4)) && (!LogeedInEmRoles.Contains(UserRoles.ExpenseAdmin))
                                       && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                       && (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                  join ese in dbContext.Tbl_HR_ExpenseStageEvent on E.ExpenseID equals ese.ExpenseID into eventStageRecord  // Fix to add red Image support

                                  select new ExpenseReimbursementStatus
                                  {
                                      Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                      ReportingTo = emp.ReportingTo,
                                      ExpenseId = E.ExpenseID,
                                      StageId = E.StageID,
                                      ExpenseStageOrder = E.StageID,
                                      stageName = s.ExpenseStage,
                                      EmployeeId = E.EmployeeID,
                                      Employeename = emp.EmployeeName,
                                      ExpenseFormName = E.ReimbursementFormName,
                                      ExpenseFormCode = E.ReimbursementFormCode,
                                      FormCode = E.FormCode
                                  }).Distinct().OrderByDescending(exid => exid.ExpenseId).ToList();

                #endregion Employee Watchlist Section

                // following logic for checking manager login & any entries fall under managers watchlist bucket

                #region For Primary Approver Watchlist Section

                PriApproverCheck = (from E in dbContext.tbl_HR_Expense
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_HR_ExpenseStages on E.StageID + 1 equals s.StageID into st
                                    from extstage in st.DefaultIfEmpty()
                                    where E.PrimaryApprover == employeeId && (E.StageID != 1) && (E.IsCancelled == false || E.IsCancelled == null) && (LogeedInEmRoles.Contains(UserRoles.ExpenseAdmin) && (E.StageID != 3) || !(LogeedInEmRoles.Contains(UserRoles.ExpenseAdmin)) && (E.StageID == 4 || E.StageID == 3 || E.StageID == 2))
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                         && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.Tbl_HR_ExpenseStageEvent on E.ExpenseID equals ese.ExpenseID into eventStageRecord  // Fix to add red Image support

                                    select new ExpenseReimbursementStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support

                                        ReportingTo = ex.ReportingTo,
                                        ExpenseId = E.ExpenseID,
                                        StageId = E.StageID,
                                        ExpenseStageOrder = E.StageID,
                                        stageName = extstage.ExpenseStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = ex.EmployeeName,
                                        ExpenseFormName = E.ReimbursementFormName,
                                        ExpenseFormCode = E.ReimbursementFormCode,
                                        FormCode = E.FormCode
                                    }).Distinct().OrderByDescending(exid => exid.ExpenseId).ToList();

                #endregion For Primary Approver Watchlist Section

                // following logic is to check what falls under HR Admins watchlist ie.
                // HR Admin will handle HR Approval,Exit Interview, Hr Closure, Exit stages.

                #region Secondary approver Watchlist Section

                SecApproverCheck = (from E in dbContext.tbl_HR_Expense
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_HR_ExpenseStages on E.StageID + 1 equals s.StageID into st
                                    from extstage in st.DefaultIfEmpty()
                                    where E.SecondaryApprover == employeeId && (E.StageID != 2) && (E.IsCancelled == false || E.IsCancelled == null) && (LogeedInEmRoles.Contains(UserRoles.ExpenseAdmin) && (E.StageID != 3) || !(LogeedInEmRoles.Contains(UserRoles.ExpenseAdmin)) && (E.StageID == 3 || E.StageID == 4 || E.StageID == 1))
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                         && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.Tbl_HR_ExpenseStageEvent on E.ExpenseID equals ese.ExpenseID into eventStageRecord  // Fix to add red Image support

                                    select new ExpenseReimbursementStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                        ReportingTo = ex.ReportingTo,
                                        ExpenseId = E.ExpenseID,
                                        StageId = E.StageID,
                                        ExpenseStageOrder = E.StageID,
                                        stageName = extstage.ExpenseStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = ex.EmployeeName,
                                        ExpenseFormName = E.ReimbursementFormName,
                                        ExpenseFormCode = E.ReimbursementFormCode,
                                        FormCode = E.FormCode
                                    }).Distinct().OrderByDescending(exid => exid.ExpenseId).ToList();

                #endregion Secondary approver Watchlist Section

                #region Finance Approval stage Watchlist Section

                FinApproverCheck = (from E in dbContext.tbl_HR_Expense
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_HR_ExpenseStages on E.StageID + 1 equals s.StageID into st
                                    from extstage in st.DefaultIfEmpty()
                                    where LogeedInEmRoles.Contains(UserRoles.ExpenseAdmin) && (E.StageID != 3) && (E.IsCancelled == false || E.IsCancelled == null) && (E.PrimaryApprover != employeeId || E.PrimaryApprover == null) && (E.SecondaryApprover != employeeId || E.SecondaryApprover == null)
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.StageID == FieldChild : FieldChild == 0))) //field search
                                         && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.Tbl_HR_ExpenseStageEvent on E.ExpenseID equals ese.ExpenseID into eventStageRecord  // Fix to add red Image support

                                    select new ExpenseReimbursementStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                        ReportingTo = ex.ReportingTo,
                                        ExpenseId = E.ExpenseID,
                                        StageId = E.StageID,
                                        ExpenseStageOrder = E.StageID,
                                        stageName = extstage.ExpenseStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = ex.EmployeeName,
                                        ExpenseFormName = E.ReimbursementFormName,
                                        ExpenseFormCode = E.ReimbursementFormCode,
                                        FormCode = E.FormCode
                                    }).Distinct().OrderByDescending(exid => exid.ExpenseId).ToList();

                #endregion Finance Approval stage Watchlist Section

                mainResult = employeeresult.Union(employeeresult).Union(PriApproverCheck).Union(SecApproverCheck).Union(FinApproverCheck).ToList();

                var distinctItems = mainResult.GroupBy(x => x.ExpenseId).Select(y => y.First()).ToList();

                totalCount = distinctItems.Count;

                //return distinctItems.Skip((page - 1) * rows).Take(rows).ToList();
                return distinctItems.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<FieldChildDetails> GetFieldChildDetailsList(string field)
        {
            try
            {
                List<FieldChildDetails> childDetails = new List<FieldChildDetails>();
                if (field == "Business Group")
                {
                    List<FieldChildDetails> child = (from l in dbContext.tbl_CNF_BusinessGroups
                                                     select new FieldChildDetails
                                                     {
                                                         Id = l.BusinessGroupID,
                                                         Description = l.BusinessGroup
                                                     }).ToList();

                    return child;
                }
                else
                {
                    if (field == "Organization Unit")
                    {
                        List<FieldChildDetails> child = (from l in dbContext.tbl_PM_Location
                                                         select new FieldChildDetails
                                                         {
                                                             Id = l.LocationID,
                                                             Description = l.Location
                                                         }).ToList();

                        return child;
                    }
                    else
                    {
                        if (field == "Stage Name")
                        {
                            List<FieldChildDetails> child = (from expenseStage in dbContext.tbl_HR_ExpenseStages
                                                             select new FieldChildDetails
                                                             {
                                                                 Id = expenseStage.StageID,
                                                                 Description = expenseStage.ExpenseStage
                                                             }).ToList();

                            return child;
                        }
                        else
                        {
                            return childDetails;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ExpenseReimbursementShowStatus> GetShowStatusResult(int page, int rows, int expenseID, out int totalCount)
        {
            try
            {
                List<ExpenseReimbursementShowStatus> FinalResult = new List<ExpenseReimbursementShowStatus>();
                List<ExpenseReimbursementShowStatus> result = new List<ExpenseReimbursementShowStatus>();
                ExpenseReimbursementShowStatus secondresult = new ExpenseReimbursementShowStatus();
                string ApproverName = string.Empty;
                string ApproverNameFinal = string.Empty;
                var expensedetails = (from e in dbContext.tbl_HR_Expense where e.ExpenseID == expenseID select e).FirstOrDefault();
                var expenseStageDetails = (from e in dbContext.Tbl_HR_ExpenseStageEvent where e.ExpenseID == expenseID select e).FirstOrDefault();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();

                string employeeCode = personalDAL.getEmployeeCode(expensedetails.EmployeeID.HasValue ? expensedetails.EmployeeID.Value : 0);
                string loggedInUserRole = "";
                string[] role = Roles.GetRolesForUser(employeeCode);
                if (role.Contains(UserRoles.DeliveryManager))
                    loggedInUserRole = UserRoles.DeliveryManager;
                else if (role.Contains(UserRoles.AccountOwner))
                    loggedInUserRole = UserRoles.AccountOwner;
                else if (role.Contains(UserRoles.GroupHead))
                    loggedInUserRole = UserRoles.GroupHead;
                else if (role.Contains(UserRoles.Management))
                    loggedInUserRole = UserRoles.Management;
                else if (role.Contains(UserRoles.Manager))
                    loggedInUserRole = UserRoles.Manager;

                if (expensedetails.StageID == 0)
                {
                    HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(expensedetails.EmployeeID.HasValue ? expensedetails.EmployeeID.Value : 0);
                    ApproverNameFinal = EmpDetails.EmployeeName;
                }
                if (expensedetails.StageID == 1)
                {
                    HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(expensedetails.PrimaryApprover.HasValue ? expensedetails.PrimaryApprover.Value : 0);
                    ApproverNameFinal = EmpDetails.EmployeeName;
                }
                else if (expensedetails.StageID == 2)
                {
                    HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(expensedetails.SecondaryApprover.HasValue ? expensedetails.SecondaryApprover.Value : 0);
                    ApproverNameFinal = EmpDetails.EmployeeName;
                }

                result = (from events in dbContext.Tbl_HR_ExpenseStageEvent
                          join employee in dbContext.HRMS_tbl_PM_Employee on events.UserId equals employee.EmployeeID into expenseemployee
                          from exevent in expenseemployee.DefaultIfEmpty()
                          join expense in dbContext.tbl_HR_Expense on events.ExpenseID equals expense.ExpenseID into expenseEvent
                          from exStageEvent in expenseEvent.DefaultIfEmpty()
                          join stages in dbContext.tbl_HR_ExpenseStages on (loggedInUserRole == UserRoles.Management && events.FromStageId == 0 && events.ToStageId == 3) ? (events.FromStageId + 1) :
                          ((loggedInUserRole == UserRoles.DeliveryManager || loggedInUserRole == UserRoles.AccountOwner || loggedInUserRole == UserRoles.GroupHead || loggedInUserRole == UserRoles.Manager) && events.FromStageId == 1 && events.ToStageId == 3) ? events.FromStageId + 1 :
                          (events.Action == "Approved" ? events.ToStageId : (events.FromStageId + 1))
                          equals stages.StageID into stage
                          from eventstage in stage.DefaultIfEmpty()
                          join employee in dbContext.HRMS_tbl_PM_Employee on exStageEvent.EmployeeID equals employee.EmployeeID into employeeexpenseevent
                          from employeeexpense in employeeexpenseevent.DefaultIfEmpty()
                          where exStageEvent.ExpenseID == expenseID
                          orderby events.Id ascending
                          select new ExpenseReimbursementShowStatus
                          {
                              ShowstatusAction = events.Action,
                              ShowstatusActor = exevent.EmployeeName,
                              ShowstatusCurrentStage = eventstage.ExpenseStage,
                              ShowstatusStageID = events.FromStageId,
                              ShowstatusEmployeeCode = employeeexpense.EmployeeCode,
                              ShowstatusEmployeeId = exevent.EmployeeID,
                              ShowstatusEmployeeName = employeeexpense.EmployeeName,
                              ShowstatusTime = events.EventDatatime,
                              ShowstatusComments = events.Action == "Rejected" ? events.Comments : ""
                          }).ToList();

                if (result.Any())
                    FinalResult.AddRange(result);

                if (expensedetails.StageID != 4)
                {
                    string messageToDisplay = "";
                    if (expensedetails.StageID == 3)
                        messageToDisplay = "Pending for Finance Admin to take action";
                    else
                        messageToDisplay = "Waiting for " + ApproverNameFinal + " to take Action";

                    secondresult = (from ex in dbContext.tbl_HR_Expense
                                    join s in dbContext.tbl_HR_ExpenseStages on (ex.StageID + 1) equals s.StageID into stage
                                    from EStage in stage.DefaultIfEmpty()
                                    where ex.ExpenseID == expenseID
                                    select new ExpenseReimbursementShowStatus
                                    {
                                        ShowstatusCurrentStage = EStage.ExpenseStage,
                                        showStatus = messageToDisplay
                                    }).FirstOrDefault();

                    FinalResult.Add(secondresult);
                }
                else if (expensedetails.StageID == 4)
                {
                    secondresult = (from ex in dbContext.tbl_HR_Expense
                                    join s in dbContext.tbl_HR_ExpenseStages on (ex.StageID + 1) equals s.StageID into stage
                                    from EStage in stage.DefaultIfEmpty()
                                    where ex.ExpenseID == expenseID
                                    select new ExpenseReimbursementShowStatus
                                    {
                                        ShowstatusCurrentStage = EStage.ExpenseStage
                                    }).FirstOrDefault();
                    FinalResult.Add(secondresult);
                }
                totalCount = FinalResult.Count;
                return FinalResult.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetFormCode()
        {
            var code = (from c in dbContext.tbl_HR_Expense
                        orderby c.FormCode descending
                        select c.FormCode).FirstOrDefault();

            if (code == null)
            {
                return 1;
            }
            else
            {
                return (Convert.ToInt32(code) + 1);
            }
        }

        public bool GetFromNameStatus(string Formname)
        {
            try
            {
                bool status;
                var expensedetails = (from e in dbContext.tbl_HR_Expense where e.ReimbursementFormName == Formname select e).FirstOrDefault();
                if (expensedetails != null)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GetFromCodeStatus(string FormCode)
        {
            int formCode = Convert.ToInt32(FormCode);
            try
            {
                bool status;
                var formCodeExist = (from e in dbContext.tbl_HR_Expense where e.FormCode == formCode select e).FirstOrDefault();
                if (formCodeExist != null)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ExpenseReimbursementStatus getExpenseDetails(int expernseID)
        {
            try
            {
                ExpenseReimbursementStatus expenseDetails = new ExpenseReimbursementStatus();
                expenseDetails = (from expense in dbContext.tbl_HR_Expense
                                  join employee in dbContext.HRMS_tbl_PM_Employee on expense.EmployeeID equals employee.EmployeeID into exemployee
                                  from expeseeployee in exemployee.DefaultIfEmpty()
                                  where expense.ExpenseID == expernseID
                                  select new ExpenseReimbursementStatus
                                  {
                                      ExpenseId = expense.ExpenseID,
                                      EmployeeId = expense.EmployeeID,
                                      Employeename = expeseeployee.EmployeeName,
                                      StageId = expense.StageID,
                                      ExpenseFormName = expense.ReimbursementFormName,
                                      ExpenseFormCode = expense.ReimbursementFormCode,
                                      ProjectName = expense.ProjectName,
                                      IsClientReimbursement = expense.IsClientReimbursement.HasValue ? expense.IsClientReimbursement.Value : 0,
                                      ClientName = expense.ClientName,
                                      DateOfSubmission = expense.DateOfSubmission,
                                      PrimaryApprover = expense.PrimaryApprover,
                                      SecondaryApprover = expense.SecondaryApprover,
                                      FinanceApprover = expense.FinanceApprover,
                                      ToalAmount = expense.Total,
                                      BalanceAmount = expense.Balance,
                                      AdvanceAmount = expense.Advance,
                                      FormCode = expense.FormCode
                                  }).FirstOrDefault();
                return expenseDetails;
            }
            catch
            {
                throw;
            }
        }

        public bool DeletedAllExpenseDetails(int ExpenseID, string employeeId)
        {
            bool isDeleted = false;
            tbl_HR_Expense DelFromExpentbl = dbContext.tbl_HR_Expense.Where(cd => cd.ExpenseID == ExpenseID).FirstOrDefault();
            if (ExpenseID != null && ExpenseID > 0)
            {
                DelFromExpentbl.IsCancelled = true;
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public EmployeeDetailsViewModel getEmployeeDetailsForExpense(int? employeeID)
        {
            try
            {
                EmployeeDetailsViewModel employeeDetails = new EmployeeDetailsViewModel();
                employeeDetails = (from e in dbContext.HRMS_tbl_PM_Employee
                                   join r in dbContext.HRMS_tbl_PM_Role on e.PostID equals r.RoleID into role
                                   from ERole in role.DefaultIfEmpty()
                                   where e.EmployeeID == employeeID
                                   select new EmployeeDetailsViewModel
                                   {
                                       OrgRoleDescription = ERole.RoleDescription,
                                       EmployeeId = e.EmployeeID,
                                       EmployeeCode = e.EmployeeCode,
                                       EmployeeName = e.EmployeeName,
                                       EmailID = e.EmailID
                                   }).FirstOrDefault();
                return employeeDetails;
            }
            catch
            {
                throw;
            }
        }

        public int getExpenseID(int employeeID, int expenseId)
        {
            try
            {
                int expenseID = 0;
                tbl_HR_Expense _HRExpense = dbContext.tbl_HR_Expense.Where(x => x.EmployeeID == employeeID && x.ExpenseID == expenseId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                return expenseID = _HRExpense.ExpenseID;
            }
            catch
            {
                throw;
            }
        }

        public int getExpenseIDfromFormCode(int? FormCode)
        {
            try
            {
                int expenseID = 0;
                tbl_HR_Expense _HRExpense = dbContext.tbl_HR_Expense.Where(x => x.FormCode == FormCode).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                return expenseID = _HRExpense.ExpenseID;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveCommentDetails(int expenseid, string comments, string CommentType)
        {
            bool status = false;
            try
            {
                tbl_HR_Expense emp = dbContext.tbl_HR_Expense.Where(ed => ed.ExpenseID == expenseid).FirstOrDefault();
                if (emp != null)
                {
                    if (CommentType == "rejected")
                    {
                        emp.RejectComment = comments;
                        dbContext.SaveChanges();
                    }

                    if (CommentType == "canceled")
                    {
                        emp.CancelComment = comments;
                        dbContext.SaveChanges();
                    }
                }

                status = true;
            }
            catch
            {
                throw;
            }
            return status;
        }

        public tbl_HR_Expense GetExpenseDetailsfromExpenseId(int expenseId)
        {
            try
            {
                tbl_HR_Expense empDetails = dbContext.tbl_HR_Expense.Where(ed => ed.ExpenseID == expenseId).FirstOrDefault();
                return empDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetEmployeeID(int employeeid)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                string employeeID = (from e in dbContext.HRMS_tbl_PM_Employee
                                     where e.EmployeeID == employeeid
                                     select e.EmployeeCode).FirstOrDefault();

                int Employyeid = Convert.ToInt32(employeeID);

                return Employyeid;
            }
            catch
            {
                throw;
            }
        }

        public List<ExpenseReimbProjects> GetAllExpenseReimbProjectNamesList()
        {
            try
            {
                List<ExpenseReimbProjects> ProjectsList = new List<ExpenseReimbProjects>();

                ProjectsList = (from project in dbContext.tbl_Client_ProjectNamesMaster
                                orderby project.ProjectNameID ascending
                                select new ExpenseReimbProjects
                                {
                                    ProjectNameID = project.ProjectNameID,
                                    ProjectName = project.ProjectName,
                                    ProjectDescription = project.ProjectDescription
                                }).ToList();
                return ProjectsList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Client_ProjectNamesMaster getExpProjectDetails(int expProjectId)
        {
            try
            {
                tbl_Client_ProjectNamesMaster expProjectDetails = dbContext.tbl_Client_ProjectNamesMaster.Where(category => category.ProjectNameID == expProjectId).FirstOrDefault();
                return expProjectDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ExpenseReimbProcessResponse DeleteProject(int expProjectId)
        {
            try
            {
                ExpenseReimbProcessResponse response = new ExpenseReimbProcessResponse();
                response.isDeleted = false;

                tbl_Client_ProjectNamesMaster _projectDetails = dbContext.tbl_Client_ProjectNamesMaster.Where(category => category.ProjectNameID == expProjectId).FirstOrDefault();
                if (_projectDetails != null)
                {
                    dbContext.DeleteObject(_projectDetails);
                    dbContext.SaveChanges();
                    response.isDeleted = true;
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ExpenseReimbProcessResponse AddEditNewProject(ExpenseProjectNamesModel model)
        {
            try
            {
                ExpenseReimbProcessResponse Response = new ExpenseReimbProcessResponse();
                Response.isAdded = false;
                Response.isExisted = false;
                tbl_Client_ProjectNamesMaster addProject = dbContext.tbl_Client_ProjectNamesMaster.Where(c => c.ProjectName == model.NewProjectName).FirstOrDefault();
                tbl_Client_ProjectNamesMaster categoryDetails = dbContext.tbl_Client_ProjectNamesMaster.Where(category => category.ProjectNameID == model.ProjectNameID).FirstOrDefault();
                if ((addProject == null) || (addProject.ProjectName == model.ExistingExpProjectName))
                {
                    if (model.ProjectNameID == 0)
                    {
                        tbl_Client_ProjectNamesMaster addNewProject = new tbl_Client_ProjectNamesMaster();
                        if (model.NewProjectName != null && model.NewProjectName != "")
                            addNewProject.ProjectName = model.NewProjectName.Trim();
                        else
                            addNewProject.ProjectName = model.NewProjectName;
                        if (model.NewExpProjectDescription != null && model.NewExpProjectDescription != "")
                            addNewProject.ProjectDescription = model.NewExpProjectDescription.Trim();
                        else
                            addNewProject.ProjectDescription = model.NewExpProjectDescription;
                        addNewProject.CreatedBy = model.SearchedUserDetails.EmployeeId;
                        addNewProject.CreatedDate = DateTime.Now;
                        dbContext.tbl_Client_ProjectNamesMaster.AddObject(addNewProject);
                        dbContext.SaveChanges();
                        Response.isAdded = true;
                    }
                    else if (categoryDetails != null && model.ProjectNameID > 0)
                    {
                        if (model.NewProjectName != null && model.NewProjectName != "")
                            categoryDetails.ProjectName = model.NewProjectName.Trim();
                        else
                            categoryDetails.ProjectName = model.NewProjectName;
                        if (model.NewExpProjectDescription != null && model.NewExpProjectDescription != "")
                            categoryDetails.ProjectDescription = model.NewExpProjectDescription.Trim();
                        else
                            categoryDetails.ProjectDescription = model.NewExpProjectDescription;
                        categoryDetails.ModifiedBy = model.SearchedUserDetails.EmployeeId;
                        categoryDetails.ModifiedDate = DateTime.Now;
                        dbContext.SaveChanges();
                        Response.isAdded = true;
                    }
                }
                else
                {
                    Response.isExisted = true;
                }
                return Response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProjectNameList> projectNameList()
        {
            List<ProjectNameList> projectName = new List<ProjectNameList>();
            try
            {
                //projectName = (from c in dbContext.tbl_Client_ProjectNamesMaster
                //               orderby c.ProjectName
                //               select new ProjectNameList
                //               {
                //                   ProjectNameID = c.ProjectNameID,
                //                   ProjectName = c.ProjectName
                //               }).ToList();

                projectName = (from types in dbSEMContext.tbl_PM_Customer
                               orderby types.CustomerName
                               select new ProjectNameList
                               {
                                   ProjectNameID = types.Customer,
                                   ProjectName = types.CustomerName
                               }).ToList();

                return projectName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tbl_HR_ExpenseDetail GetExpenseDetailsRecord(int ExpenseDetailsId)
        {
            try
            {
                Tbl_HR_ExpenseDetail detailsRecord = dbContext.Tbl_HR_ExpenseDetail.Where(e => e.ID == ExpenseDetailsId).FirstOrDefault();
                return detailsRecord;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}