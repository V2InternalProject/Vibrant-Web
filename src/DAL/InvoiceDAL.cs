using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web.Security;

namespace HRMS.DAL
{
    public class InvoiceDAL
    {
        private WSEMDBEntities dbContext = new WSEMDBEntities();

        #region Generate Invoice

        public List<InvoiceTypeList> GetInvoiceTypeList()
        {
            try
            {
                var invoiceTypes = dbContext.GetInvoiceTypes_SP();
                List<InvoiceTypeList> invoiceTypeLists = (from type in invoiceTypes
                                                          select new InvoiceTypeList
                                                          {
                                                              TypeID = type.RFITypeID,
                                                              TypeName = type.RFITypeName
                                                          }).ToList();
                return invoiceTypeLists;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public InvoiceProjectDetails GetSelectedProjectRecord(int ProjectID)
        {
            try
            {
                var projectDetails = dbContext.GetApprovedProjectList_SP();
                InvoiceProjectDetails projectRecord = new InvoiceProjectDetails();
                projectRecord = (from p in projectDetails
                                 where p.ProjectID == ProjectID
                                 select new InvoiceProjectDetails
                                 {
                                     ProjectID = ProjectID,
                                     ProjectCurrencyID = p.BillingCurrencyID
                                 }).FirstOrDefault();

                var irApproversList = dbContext.GetProjectIRApproverDetails_sp(ProjectID);
                List<InvoiceProjectDetails> IRApproverIDList = (from ir in irApproversList
                                                                select new InvoiceProjectDetails
                                                                {
                                                                    IRApproverID = ir.ApproverID
                                                                }).ToList();
                projectRecord.IRApproverCount = IRApproverIDList.Count;

                var irFinanceApproversList = dbContext.GetProjectIRFinanceApproverDetails_sp(ProjectID);
                List<InvoiceProjectDetails> IRFinanceApproverIDList = (from ir in irFinanceApproversList
                                                                       select new InvoiceProjectDetails
                                                                       {
                                                                           IRFinanceApproverID = ir.FinanceApproverID
                                                                       }).ToList();
                projectRecord.IRFinanceApproverCount = IRApproverIDList.Count;
                return projectRecord;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<InvoiceStageList> GetInvoiceStageFilterList()
        {
            try
            {
                var stageFilters = dbContext.GetInvoiceStageFilters_SP();
                List<InvoiceStageList> stageFilterList = (from stage in stageFilters
                                                          select new InvoiceStageList
                                                          {
                                                              StageName = stage.RFIStageFilters
                                                          }).ToList();
                return stageFilterList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<InvoiceNameList> GetInvoiceNameFilterList()
        {
            try
            {
                var nameFilters = dbContext.GetInvoiceNameFilters_SP();
                List<InvoiceNameList> nameFilterList = (from name in nameFilters
                                                        select new InvoiceNameList
                                                         {
                                                             InvoiceName = name.InvoiceNameFilter
                                                         }).ToList();
                return nameFilterList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckLoggedUserIsIRApprover(int EmployeeId)
        {
            try
            {
                bool isIRApprover = false;
                var irApproverDetails = dbContext.GetProjectIRApproverDetailsFromEmployeeID_SP(EmployeeId);
                List<IRApproverClass> irApproverList = (from name in irApproverDetails
                                                        select new IRApproverClass
                                                        {
                                                            IRApproverID = name.ApproverID
                                                        }).ToList();
                if (irApproverList.Count > 0)
                    isIRApprover = true;
                return isIRApprover;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckLoggedUserIsIRFinanceApprover(int EmployeeId)
        {
            try
            {
                bool isIRFinanceApprover = false;
                var irFinanceApproverDetails = dbContext.GetProjectIRFinanceApproverDetailsFromEmployeeID_SP(EmployeeId);
                List<IRApproverClass> irFinanceApproverList = (from name in irFinanceApproverDetails
                                                               select new IRApproverClass
                                                               {
                                                                   IRApproverID = name.FinanceApproverID
                                                               }).ToList();
                if (irFinanceApproverList.Count > 0)
                    isIRFinanceApprover = true;
                return isIRFinanceApprover;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<InvoiceDetailsModel> GetInvoiceInboxWatchListDetails(string TextLink, int? ProjectID, string LoggedUserName, int? InvoiceTypeID, string InvoiceName, string InvoiceStage, string GridName, int page, int rows, out int totalCount)
        {
            try
            {
                bool? isProforma = null;
                if (InvoiceName == "IR")
                    isProforma = false;
                else if (InvoiceName == "PIR")
                    isProforma = true;
                if (string.IsNullOrEmpty(InvoiceStage))
                {
                    InvoiceStage = null;
                }

                var inboxWLDetails = dbContext.GetInvoiceInboxWatchListDetails_sp(TextLink, ProjectID, LoggedUserName, isProforma, InvoiceTypeID, InvoiceStage, GridName);
                List<InvoiceDetailsModel> IRGeneratorResults = (from i in inboxWLDetails
                                                                select new InvoiceDetailsModel
                                                                {
                                                                    ProjectID = i.ProjectID,
                                                                    Amount = Math.Round(Convert.ToDouble(i.BillingCurrencyAmount), 2),
                                                                    EquivalentCurrencyAmount = Math.Round(Convert.ToDouble(i.BaseCurrencyAmount), 2),
                                                                    CorporateBaseAmount = Math.Round(Convert.ToDouble(i.CompanyBaseCurrencyAmount), 2),
                                                                    CurrencyName = i.CurrencyName,
                                                                    CustomerName = i.CustomerName,
                                                                    InvoiceName = i.IsProforma.ToString(),
                                                                    InvoiceStage = i.CurrentStatus,
                                                                    ProjectName = i.ProjectName,
                                                                    RFIID = i.RFIID,
                                                                    RFIRaisedOn = i.RFIRaisedOn,
                                                                    RFIType = i.RFITypeName
                                                                }).ToList();
                totalCount = IRGeneratorResults.Count();
                return IRGeneratorResults.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<InvoiceShowStatusModel> GetInvoiceShowStatusResult(int page, int rows, int RFIID, int ProjectID, out int totalCount)
        {
            try
            {
                List<InvoiceShowStatusModel> FinalResult = new List<InvoiceShowStatusModel>();
                List<InvoiceShowStatusModel> result = new List<InvoiceShowStatusModel>();
                InvoiceShowStatusModel secondresult = new InvoiceShowStatusModel();
                string ApproverNameFinal = string.Empty;
                var invoicedetails = (from e in dbContext.v_tbl_PM_RFIs where e.RFIID == RFIID select e).FirstOrDefault();

                if (invoicedetails.CurrentStatus == InvoiceStages.DraftStage)
                {
                    var EmpDetails = (from e in dbContext.v_tbl_PM_RFIs where e.RFIID == RFIID select e).FirstOrDefault();
                    ApproverNameFinal = EmpDetails.EmployeeName;
                }

                var stageDetails = dbContext.GetInvoiceShowStatusDetails_sp(RFIID);
                result = (from e in stageDetails
                          select new InvoiceShowStatusModel
                          {
                              ShowstatusAction = e.Action,
                              ShowstatusActor = e.EmployeeName,
                              ShowstatusCurrentStage = e.CurrentRFIStatus,
                              ShowstatusEmployeeName = e.EmployeeName,
                              ShowstatusTime = e.ChangedOn,
                              ShowstatusComments = (e.Action == "Rejected" || e.Action == "Cancelled") ? e.Comments : ""
                          }).ToList();

                if (result.Any())
                    FinalResult.AddRange(result);

                if (invoicedetails.CurrentStatus == InvoiceStages.DraftStage)
                {
                    string messageToDisplay = "";
                    messageToDisplay = "Pending for " + ApproverNameFinal + " to take action";
                    secondresult = (from i in dbContext.v_tbl_PM_RFIs
                                    where i.RFIID == RFIID
                                    select new InvoiceShowStatusModel
                                    {
                                        ShowstatusCurrentStage = i.CurrentStatus,
                                        showStatus = messageToDisplay
                                    }).FirstOrDefault();

                    FinalResult.Add(secondresult);
                }
                else if (invoicedetails.CurrentStatus == InvoiceStages.ApproverStage)
                {
                    string messageToDisplay = "";
                    messageToDisplay = "Pending for Approver to take action";
                    secondresult = (from i in dbContext.v_tbl_PM_RFIs
                                    where i.RFIID == RFIID
                                    select new InvoiceShowStatusModel
                                    {
                                        ShowstatusCurrentStage = i.CurrentStatus,
                                        showStatus = messageToDisplay
                                    }).FirstOrDefault();

                    FinalResult.Add(secondresult);
                }
                else if (invoicedetails.CurrentStatus == InvoiceStages.FinanceApproverStage)
                {
                    string messageToDisplay = "";
                    messageToDisplay = "Pending for Finance Approver to take action";
                    secondresult = (from f in dbContext.v_tbl_PM_RFIs
                                    where f.RFIID == RFIID
                                    select new InvoiceShowStatusModel
                                    {
                                        ShowstatusCurrentStage = f.CurrentStatus,
                                        showStatus = messageToDisplay
                                    }).FirstOrDefault();

                    FinalResult.Add(secondresult);
                }
                else if (invoicedetails.CurrentStatus == InvoiceStages.ApprovedStage)
                {
                    secondresult = (from a in dbContext.v_tbl_PM_RFIs
                                    where a.RFIID == RFIID
                                    select new InvoiceShowStatusModel
                                    {
                                        ShowstatusCurrentStage = a.CurrentStatus
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

        public List<int?> GetHistoryIdList(int? RFIID)
        {
            try
            {
                var HistoryDetails = dbContext.GetInvoiceTypeHistoryDetails_SP();
                List<int?> historyIDList = (from h in HistoryDetails
                                            orderby h.HistoryID descending
                                            where h.RFIID == RFIID
                                            select h.HistoryID).ToList();

                return historyIDList.GroupBy(x => x.Value).Select(y => y.First()).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<InvoiceTypeItemModel> GetPIRHistoryRecords(int? RFIID, int? RFITypeID)
        {
            try
            {
                var invoiceTypeLatestDetails = dbContext.GetInvoiceTypeHistoryDetails_SP();
                List<InvoiceTypeItemModel> HistoryDetailsList = new List<InvoiceTypeItemModel>();
                if (RFITypeID == 4 || RFITypeID == 5 || RFITypeID == 1)
                {
                    HistoryDetailsList = (from typeDetails in invoiceTypeLatestDetails
                                          orderby typeDetails.HistoryID descending
                                          where typeDetails.RFIID == RFIID
                                          select new InvoiceTypeItemModel
                                          {
                                              ItemDescription = typeDetails.ItemDescription,
                                              IsDiscountItem = Convert.ToString(typeDetails.IsDiscountItem),
                                              Quantity = typeDetails.Quantity,
                                              Rate = typeDetails.Rate,
                                              Amount = typeDetails.Amount,
                                              BillableResources = typeDetails.CustomField1,
                                              Billing_Type_Adv_1_Post_2 = Convert.ToString(typeDetails.CustomField2),
                                              ResourcePoolType = typeDetails.CustomField4,
                                              RFIID = RFIID,
                                              UpdatedBy = typeDetails.UpdatedBy,
                                              UpdatedByEmployeeName = typeDetails.UpdatedByEmployeeName,
                                              UpdatedDate = typeDetails.UpdatedDate,
                                              ApprovedBy = typeDetails.ApprovedBy,
                                              ApprovedByEmployeeName = typeDetails.ApprovedByEmployeeName,
                                              ApprovedDate = typeDetails.ApprovedDate,
                                              HistoryID = typeDetails.HistoryID,
                                              RFIItemID = typeDetails.RFIItemID
                                          }).ToList();
                }
                else if (RFITypeID == 6)
                {
                    HistoryDetailsList = (from typeDetails in invoiceTypeLatestDetails
                                          orderby typeDetails.HistoryID descending
                                          where typeDetails.RFIID == RFIID
                                          select new InvoiceTypeItemModel
                                          {
                                              ItemDescription = typeDetails.ItemDescription,
                                              IsDiscountItem = Convert.ToString(typeDetails.IsDiscountItem),
                                              Quantity = typeDetails.Quantity,
                                              Rate = typeDetails.Rate,
                                              Amount = typeDetails.Amount,
                                              BillableResources = typeDetails.CustomField1,
                                              Billing_Type_Adv_1_Post_2 = Convert.ToString(typeDetails.CustomField2),
                                              Type_FB_1_FF_2_Trans_3 = Convert.ToString(typeDetails.CustomField3),
                                              ResourcePoolType = typeDetails.CustomField4,
                                              RFIID = RFIID,
                                              UpdatedBy = typeDetails.UpdatedBy,
                                              UpdatedByEmployeeName = typeDetails.UpdatedByEmployeeName,
                                              UpdatedDate = typeDetails.UpdatedDate,
                                              ApprovedBy = typeDetails.ApprovedBy,
                                              ApprovedByEmployeeName = typeDetails.ApprovedByEmployeeName,
                                              ApprovedDate = typeDetails.ApprovedDate,
                                              HistoryID = typeDetails.HistoryID,
                                              RFIItemID = typeDetails.RFIItemID
                                          }).ToList();
                }

                return HistoryDetailsList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<InvoiceTypeHeaders> GetInvoiceTypeHeaders(int? RFITypeID)
        {
            try
            {
                var invoiceTypeHeaders = dbContext.GetInvoiceTypeHeaders_SP(RFITypeID);
                List<InvoiceTypeHeaders> headersList = (from h in invoiceTypeHeaders
                                                        select new InvoiceTypeHeaders
                                                        {
                                                            RFITypeID = h.RFITypeID,
                                                            HeaderName = h.UserFriendlyCaption
                                                        }).ToList();

                return headersList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AddInvoiceResponse SavePIRHistoryDetails(int RFIID, int RFITypeID)
        {
            try
            {
                AddInvoiceResponse response = new AddInvoiceResponse();
                SemDAL semDal = new SemDAL();
                response.Status = false;
                int? incrementedHistoryID = 0;
                int rfiidCount = 0;

                string UpdatedBy = string.Empty;
                DateTime? UpdatedDate = null;
                string ApprovedBy = string.Empty;
                DateTime? ApprovedDate = null;
                string Operation = "";
                double? Type_FB_1_FF_2_Trans_3;
                double? Billing_Type_Adv_1_Post_2;
                bool? IsDiscountItem;

                EmployeeDAL EmployeeDAL = new EmployeeDAL();
                int EmployeeId = EmployeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                //int EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(EmployeeId);

                var rfiidDetails = dbContext.GetRfiDetails_sp(RFIID);
                string currentStage = rfiidDetails.Select(x => x.CurrentStatus).FirstOrDefault();
                //To get Current Invoice Type records based on RFIID
                var currentInvoiceTypeDetails = dbContext.GetGridloadForInvoice_SP(RFIID);
                List<InvoiceTypeItemModel> currentInvoiceTypeList = (from typeDetails in currentInvoiceTypeDetails
                                                                     select new InvoiceTypeItemModel
                                                                     {
                                                                         Amount = typeDetails.Amount,
                                                                         BaseCurrencyAmount = typeDetails.BaseCurrencyAmount,
                                                                         BillableResources = typeDetails.CustomField1,
                                                                         Billing_Type_Adv_1_Post_2 = Convert.ToString(typeDetails.CustomField2),
                                                                         CompanyBaseCurrencyAmount = typeDetails.CompanyBaseCurrencyAmount,
                                                                         //CorporateBaseAmount = typeDetails.CompanyBaseCurrencyAmount,
                                                                         IsDiscountItem = Convert.ToString(typeDetails.IsDiscountItem),
                                                                         ItemDescription = typeDetails.ItemDescription,
                                                                         Quantity = typeDetails.Quantity,
                                                                         Rate = typeDetails.Rate,
                                                                         ResourcePoolType = typeDetails.CustomField4,
                                                                         RFIID = RFIID,
                                                                         RFIItemID = typeDetails.RFIItemID,
                                                                         Type_FB_1_FF_2_Trans_3 = Convert.ToString(typeDetails.CustomField3),
                                                                         InvoiceGenerated = typeDetails.InvoiceGenerated,
                                                                         LocalCurrencyAmount = typeDetails.LocalCurrencyAmount,
                                                                         BillingToBaseConversionRate = typeDetails.BillingToBaseConversionRate,
                                                                         LocalToBaseConversionRate = typeDetails.LocalToBaseConversionRate,
                                                                         OrderNumber = typeDetails.OrderNumber,
                                                                         CreatedBy = typeDetails.CreatedBy,
                                                                         CreatedDate = typeDetails.CreatedDate,
                                                                         ModifiedBy = typeDetails.ModifiedBy,
                                                                         ModifiedDate = typeDetails.ModifiedDate,
                                                                         CompanyBaseCurrencyConversionRate = typeDetails.CompanyBaseCurrencyConversionRate,
                                                                         CompanyReportingCurrencyAmount1 = typeDetails.CompanyReportingCurrencyAmount1,
                                                                         CompanyReportingCurrencyAmount2 = typeDetails.CompanyReportingCurrencyAmount2,
                                                                         CompanyReportingCurrencyAmount3 = typeDetails.CompanyReportingCurrencyAmount3
                                                                     }).ToList();

                //get highest HistoryID to increment for adding new records
                var invoiceTypeHistoryDetails = dbContext.GetInvoiceTypeHistoryDetails_SP();
                int? highestHistoryID = (from history in invoiceTypeHistoryDetails
                                         orderby history.HistoryID descending
                                         where history.RFIID == RFIID
                                         select history.HistoryID).FirstOrDefault();

                if (highestHistoryID == null)
                    incrementedHistoryID = 1;
                else
                    incrementedHistoryID = highestHistoryID + 1;

                var invoiceTypeHistoryCount = dbContext.GetInvoiceTypeHistoryDetails_SP();
                rfiidCount = invoiceTypeHistoryCount.Where(x => x.RFIID == RFIID).Count();

                if (currentStage == InvoiceStages.DraftStage)
                {
                    UpdatedBy = employeeDetails.UserName;
                    UpdatedDate = DateTime.Now;
                    Operation = "INSERT";
                }
                else if (currentStage == InvoiceStages.ApproverStage)
                {
                    ApprovedBy = employeeDetails.UserName;
                    ApprovedDate = DateTime.Now;
                    Operation = "UPDATE";
                }

                if (currentInvoiceTypeList.Count == 0)
                    response.Status = true;
                else if (rfiidCount == 0 && currentStage == InvoiceStages.DraftStage)
                {
                    //add into history table
                    foreach (var item in currentInvoiceTypeList)
                    {
                        if (string.IsNullOrEmpty(item.Billing_Type_Adv_1_Post_2))
                            Billing_Type_Adv_1_Post_2 = null;
                        else
                            Billing_Type_Adv_1_Post_2 = Convert.ToDouble(item.Billing_Type_Adv_1_Post_2);

                        if (string.IsNullOrEmpty(item.Type_FB_1_FF_2_Trans_3))
                            Type_FB_1_FF_2_Trans_3 = null;
                        else
                            Type_FB_1_FF_2_Trans_3 = Convert.ToDouble(item.Type_FB_1_FF_2_Trans_3);

                        if (string.IsNullOrEmpty(item.IsDiscountItem))
                            IsDiscountItem = null;
                        else
                            IsDiscountItem = Convert.ToBoolean(item.IsDiscountItem);

                        ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                        dbContext.AddUpdateInvoiceTypeHistoryDetails_SP(item.Amount, item.BaseCurrencyAmount, item.BillableResources, Billing_Type_Adv_1_Post_2, IsDiscountItem,
                            item.ItemDescription, item.Quantity, item.Rate, item.ResourcePoolType, item.RFIID, Type_FB_1_FF_2_Trans_3, item.InvoiceGenerated, item.LocalCurrencyAmount,
                            item.BillingToBaseConversionRate, item.LocalToBaseConversionRate, item.OrderNumber, item.CreatedBy, item.CreatedDate, item.ModifiedBy, item.ModifiedDate,
                            item.CompanyBaseCurrencyConversionRate, item.CompanyBaseCurrencyAmount, item.CompanyReportingCurrencyAmount1, item.CompanyReportingCurrencyAmount2,
                            item.CompanyReportingCurrencyAmount3, RFITypeID, incrementedHistoryID, UpdatedBy, UpdatedDate, ApprovedBy, ApprovedDate, Operation, Output);
                        response.Status = Convert.ToBoolean(Output.Value);
                    }
                }
                else if (rfiidCount > 0)
                {
                    //compare main table values with history table then add if difference
                    bool isValueChanged = false;

                    var invoiceTypeLatestDetails = dbContext.GetInvoiceTypeHistoryDetails_SP();
                    List<InvoiceTypeItemModel> latestHistoryDetails = (from typeDetails in invoiceTypeLatestDetails
                                                                       where typeDetails.RFIID == RFIID && typeDetails.HistoryID == highestHistoryID
                                                                       select new InvoiceTypeItemModel
                                                                       {
                                                                           Amount = typeDetails.Amount,
                                                                           BaseCurrencyAmount = typeDetails.BaseCurrencyAmount,
                                                                           BillableResources = typeDetails.CustomField1,
                                                                           Billing_Type_Adv_1_Post_2 = Convert.ToString(typeDetails.CustomField2),
                                                                           CompanyBaseCurrencyAmount = typeDetails.CompanyBaseCurrencyAmount,
                                                                           //CorporateBaseAmount = typeDetails.CompanyBaseCurrencyAmount,
                                                                           IsDiscountItem = Convert.ToString(typeDetails.IsDiscountItem),
                                                                           ItemDescription = typeDetails.ItemDescription,
                                                                           Quantity = typeDetails.Quantity,
                                                                           Rate = typeDetails.Rate,
                                                                           ResourcePoolType = typeDetails.CustomField4,
                                                                           RFIID = RFIID,
                                                                           RFIItemID = typeDetails.RFIItemID,
                                                                           Type_FB_1_FF_2_Trans_3 = Convert.ToString(typeDetails.CustomField3),
                                                                           InvoiceGenerated = typeDetails.InvoiceGenerated,
                                                                           LocalCurrencyAmount = typeDetails.LocalCurrencyAmount,
                                                                           BillingToBaseConversionRate = typeDetails.BillingToBaseConversionRate,
                                                                           LocalToBaseConversionRate = typeDetails.LocalToBaseConversionRate,
                                                                           OrderNumber = typeDetails.OrderNumber,
                                                                           CreatedBy = typeDetails.CreatedBy,
                                                                           CreatedDate = typeDetails.CreatedDate,
                                                                           ModifiedBy = typeDetails.ModifiedBy,
                                                                           ModifiedDate = typeDetails.ModifiedDate,
                                                                           CompanyBaseCurrencyConversionRate = typeDetails.CompanyBaseCurrencyConversionRate,
                                                                           CompanyReportingCurrencyAmount1 = typeDetails.CompanyReportingCurrencyAmount1,
                                                                           CompanyReportingCurrencyAmount2 = typeDetails.CompanyReportingCurrencyAmount2,
                                                                           CompanyReportingCurrencyAmount3 = typeDetails.CompanyReportingCurrencyAmount3
                                                                       }).ToList();

                    if (currentStage == InvoiceStages.ApproverStage)
                    {
                        foreach (var item in latestHistoryDetails)
                        {
                            if (string.IsNullOrEmpty(item.Billing_Type_Adv_1_Post_2))
                                Billing_Type_Adv_1_Post_2 = null;
                            else
                                Billing_Type_Adv_1_Post_2 = Convert.ToDouble(item.Billing_Type_Adv_1_Post_2);

                            if (string.IsNullOrEmpty(item.Type_FB_1_FF_2_Trans_3))
                                Type_FB_1_FF_2_Trans_3 = null;
                            else
                                Type_FB_1_FF_2_Trans_3 = Convert.ToDouble(item.Type_FB_1_FF_2_Trans_3);

                            if (string.IsNullOrEmpty(item.IsDiscountItem))
                                IsDiscountItem = null;
                            else
                                IsDiscountItem = Convert.ToBoolean(item.IsDiscountItem);

                            ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                            dbContext.AddUpdateInvoiceTypeHistoryDetails_SP(item.Amount, item.BaseCurrencyAmount, item.BillableResources, Billing_Type_Adv_1_Post_2, IsDiscountItem,
                                item.ItemDescription, item.Quantity, item.Rate, item.ResourcePoolType, item.RFIID, Type_FB_1_FF_2_Trans_3, item.InvoiceGenerated, item.LocalCurrencyAmount,
                                item.BillingToBaseConversionRate, item.LocalToBaseConversionRate, item.OrderNumber, item.CreatedBy, item.CreatedDate, item.ModifiedBy, item.ModifiedDate,
                                item.CompanyBaseCurrencyConversionRate, item.CompanyBaseCurrencyAmount, item.CompanyReportingCurrencyAmount1, item.CompanyReportingCurrencyAmount2,
                                item.CompanyReportingCurrencyAmount3, RFITypeID, highestHistoryID, UpdatedBy, UpdatedDate, ApprovedBy, ApprovedDate, Operation, Output);
                            response.Status = Convert.ToBoolean(Output.Value);
                        }
                    }
                    else if (currentInvoiceTypeList.Count() != latestHistoryDetails.Count())
                    {
                        foreach (var item in currentInvoiceTypeList)
                        {
                            if (string.IsNullOrEmpty(item.Billing_Type_Adv_1_Post_2))
                                Billing_Type_Adv_1_Post_2 = null;
                            else
                                Billing_Type_Adv_1_Post_2 = Convert.ToDouble(item.Billing_Type_Adv_1_Post_2);

                            if (string.IsNullOrEmpty(item.Type_FB_1_FF_2_Trans_3))
                                Type_FB_1_FF_2_Trans_3 = null;
                            else
                                Type_FB_1_FF_2_Trans_3 = Convert.ToDouble(item.Type_FB_1_FF_2_Trans_3);

                            if (string.IsNullOrEmpty(item.IsDiscountItem))
                                IsDiscountItem = null;
                            else
                                IsDiscountItem = Convert.ToBoolean(item.IsDiscountItem);

                            ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                            dbContext.AddUpdateInvoiceTypeHistoryDetails_SP(item.Amount, item.BaseCurrencyAmount, item.BillableResources, Billing_Type_Adv_1_Post_2, IsDiscountItem,
                                item.ItemDescription, item.Quantity, item.Rate, item.ResourcePoolType, item.RFIID, Type_FB_1_FF_2_Trans_3, item.InvoiceGenerated, item.LocalCurrencyAmount,
                                item.BillingToBaseConversionRate, item.LocalToBaseConversionRate, item.OrderNumber, item.CreatedBy, item.CreatedDate, item.ModifiedBy, item.ModifiedDate,
                                item.CompanyBaseCurrencyConversionRate, item.CompanyBaseCurrencyAmount, item.CompanyReportingCurrencyAmount1, item.CompanyReportingCurrencyAmount2,
                                item.CompanyReportingCurrencyAmount3, RFITypeID, incrementedHistoryID, UpdatedBy, UpdatedDate, ApprovedBy, ApprovedDate, Operation, Output);
                            response.Status = Convert.ToBoolean(Output.Value);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < currentInvoiceTypeList.Count; i++)
                        {
                            for (int j = i; j < latestHistoryDetails.Count; j++)
                            {
                                if (currentInvoiceTypeList[i].Amount != latestHistoryDetails[j].Amount)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].BaseCurrencyAmount != latestHistoryDetails[j].BaseCurrencyAmount)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].BillableResources != latestHistoryDetails[j].BillableResources)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].Billing_Type_Adv_1_Post_2 != latestHistoryDetails[j].Billing_Type_Adv_1_Post_2)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].CompanyBaseCurrencyAmount != latestHistoryDetails[j].CompanyBaseCurrencyAmount)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].IsDiscountItem != latestHistoryDetails[j].IsDiscountItem)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].ItemDescription != latestHistoryDetails[j].ItemDescription)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].Quantity != latestHistoryDetails[j].Quantity)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].Rate != latestHistoryDetails[j].Rate)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].ResourcePoolType != latestHistoryDetails[j].ResourcePoolType)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].Type_FB_1_FF_2_Trans_3 != latestHistoryDetails[j].Type_FB_1_FF_2_Trans_3)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].InvoiceGenerated != latestHistoryDetails[j].InvoiceGenerated)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].LocalCurrencyAmount != latestHistoryDetails[j].LocalCurrencyAmount)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].BillingToBaseConversionRate != latestHistoryDetails[j].BillingToBaseConversionRate)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].LocalToBaseConversionRate != latestHistoryDetails[j].LocalToBaseConversionRate)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].OrderNumber != latestHistoryDetails[j].OrderNumber)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].CompanyBaseCurrencyConversionRate != latestHistoryDetails[j].CompanyBaseCurrencyConversionRate)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].CompanyReportingCurrencyAmount1 != latestHistoryDetails[j].CompanyReportingCurrencyAmount1)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].CompanyReportingCurrencyAmount2 != latestHistoryDetails[j].CompanyReportingCurrencyAmount2)
                                    isValueChanged = true;
                                else if (currentInvoiceTypeList[i].CompanyReportingCurrencyAmount3 != latestHistoryDetails[j].CompanyReportingCurrencyAmount3)
                                    isValueChanged = true;
                                break;
                            }
                        }

                        if (isValueChanged == true)
                        {
                            foreach (var item in currentInvoiceTypeList)
                            {
                                if (string.IsNullOrEmpty(item.Billing_Type_Adv_1_Post_2))
                                    Billing_Type_Adv_1_Post_2 = null;
                                else
                                    Billing_Type_Adv_1_Post_2 = Convert.ToDouble(item.Billing_Type_Adv_1_Post_2);

                                if (string.IsNullOrEmpty(item.Type_FB_1_FF_2_Trans_3))
                                    Type_FB_1_FF_2_Trans_3 = null;
                                else
                                    Type_FB_1_FF_2_Trans_3 = Convert.ToDouble(item.Type_FB_1_FF_2_Trans_3);

                                if (string.IsNullOrEmpty(item.IsDiscountItem))
                                    IsDiscountItem = null;
                                else
                                    IsDiscountItem = Convert.ToBoolean(item.IsDiscountItem);

                                ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                                dbContext.AddUpdateInvoiceTypeHistoryDetails_SP(item.Amount, item.BaseCurrencyAmount, item.BillableResources, Billing_Type_Adv_1_Post_2, IsDiscountItem,
                                    item.ItemDescription, item.Quantity, item.Rate, item.ResourcePoolType, item.RFIID, Type_FB_1_FF_2_Trans_3, item.InvoiceGenerated, item.LocalCurrencyAmount,
                                    item.BillingToBaseConversionRate, item.LocalToBaseConversionRate, item.OrderNumber, item.CreatedBy, item.CreatedDate, item.ModifiedBy, item.ModifiedDate,
                                    item.CompanyBaseCurrencyConversionRate, item.CompanyBaseCurrencyAmount, item.CompanyReportingCurrencyAmount1, item.CompanyReportingCurrencyAmount2,
                                    item.CompanyReportingCurrencyAmount3, RFITypeID, incrementedHistoryID, UpdatedBy, UpdatedDate, ApprovedBy, ApprovedDate, Operation, Output);
                                response.Status = Convert.ToBoolean(Output.Value);
                            }
                        }
                        else
                            response.Status = true;
                    }
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeletePIRHistroyDetails(int RFIID)
        {
            try
            {
                bool status = false;
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                dbContext.DeletePIRHistoryDetails_sp(RFIID, Output);
                status = Convert.ToBoolean(Output.Value);
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CustomerAddressInvoice GetCustomerNameForInvoice(int ProjectID)
        {
            try
            {
                var CustomerAddress = dbContext.usp_Sel_CustomerName(ProjectID);
                CustomerAddressInvoice customerAddress = (from cust in CustomerAddress
                                                          select new CustomerAddressInvoice
                                                        {
                                                            CustomerID = cust.Customer,
                                                            CustomerName = cust.CustomerName
                                                        }).FirstOrDefault();
                return customerAddress;
            }
            catch
            {
                throw;
            }
        }

        public List<CustomerContactPerson> GetCustomerContactPerson(int ProjectID)
        {
            try
            {
                var customerContact = dbContext.usp_sel_CustomerContactByProjectID(ProjectID);
                List<CustomerContactPerson> customerContactList = (from cust in customerContact
                                                                   select new CustomerContactPerson
                                                                   {
                                                                       CustomerContactID = cust.CustomerContactID,
                                                                       ContactPerson = cust.ContactPerson,
                                                                       EmailID = cust.EMailID
                                                                   }).ToList();
                return customerContactList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ProjectCurrency GetCustomerProjectCurrency(int ProjectID)
        {
            try
            {
                var customerCurrency = dbContext.usp_sel_CustomerProjectCurrency(ProjectID);
                ProjectCurrency customerCurrencyList = (from cust in customerCurrency
                                                        select new ProjectCurrency
                                                        {
                                                            CuurencyID = cust.CurrencyID,
                                                            CurrencyName = cust.CurrencyName
                                                        }).FirstOrDefault();
                return customerCurrencyList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetNextRFIID()
        {
            try
            {
                ObjectParameter Output = new ObjectParameter("NextID", typeof(int));
                dbContext.usp_sel_NextRFIID(Output);
                int nextrfiid = Convert.ToInt32((Output.Value)) + 1;
                return nextrfiid;
            }
            catch
            {
                throw;
            }
        }

        public AddInvoiceModel GetRFIIDDetails(int Rfiid)
        {
            try
            {
                AddInvoiceModel rfidetails = new AddInvoiceModel();
                var rfidtls = dbContext.GetRfiDetails_sp(Rfiid);
                rfidetails = (from r in rfidtls
                              select new AddInvoiceModel
                              {
                                  IrPirID = r.RFIID,
                                  IsProforma = r.IsProforma,
                                  CreditDays = r.CreditDays.HasValue ? r.CreditDays.Value : 0,
                                  TypeID = r.RFITypeID.HasValue ? r.RFITypeID.Value : 0,
                                  CustomerContactID = r.CustomerContactID.HasValue ? r.CustomerContactID.Value : 0,
                                  ConfirmEmailID = r.ConfirmEmailID,
                                  RFIRaisedBy = r.RFIRaisedBy,
                                  CurrentStatus = r.CurrentStatus
                              }).FirstOrDefault();
                return rfidetails;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveInvoiceDetails(AddInvoiceModel model)
        {
            try
            {
                bool status = false;
                string CurrentStatus = "";
                int? rfiid = 0;
                AddInvoiceModel rfidetails = new AddInvoiceModel();
                var frmTable = dbContext.GetRfiDetails_sp(model.IrPirID);
                rfidetails = (from r in frmTable
                              select new AddInvoiceModel
                              {
                                  IsProforma = r.IsProforma,
                                  CurrentStatus = r.CurrentStatus
                              }).FirstOrDefault();
                if (rfidetails != null)
                {
                    if (rfidetails.IsProforma == true)
                    {
                        if (rfidetails.CurrentStatus == InvoiceStages.DraftStage)
                            CurrentStatus = InvoiceStages.DraftStage;
                        if (rfidetails.CurrentStatus == InvoiceStages.ApproverStage)
                            CurrentStatus = InvoiceStages.ApproverStage;
                    }
                    if (rfidetails.IsProforma == false)
                    {
                        if (rfidetails.CurrentStatus == InvoiceStages.DraftStage)
                            CurrentStatus = InvoiceStages.DraftStage;
                        if (rfidetails.CurrentStatus == InvoiceStages.ApproverStage)
                            CurrentStatus = InvoiceStages.ApproverStage;
                        if (rfidetails.CurrentStatus == InvoiceStages.FinanceApproverStage)
                            CurrentStatus = InvoiceStages.FinanceApproverStage;
                        if (rfidetails.CurrentStatus == InvoiceStages.ApprovedStage)
                            CurrentStatus = InvoiceStages.ApprovedStage;
                    }
                }
                else
                {
                    CurrentStatus = "Draft";
                }
                if (model.IsViewDetails == "NewForm")
                    rfiid = null;
                else
                    rfiid = model.IrPirID;
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                dbContext.AddUpdateRFIs_SP(rfiid, model.ProjectID, model.IsProforma, model.TypeID, model.CustomerID, model.CustomerContactID, model.CustomerAddressID, model.CurrencyID, model.CreditDays, null, model.CustomerEmail, null, model.ContractID, "Header", CurrentStatus, model.SalesPeriodID, model.RFIRaisedBy, Output);
                status = Convert.ToBoolean(Output.Value);
                return status;
            }
            catch
            {
                throw;
            }
        }

        public bool SubmitInvoiceDetails(AddInvoiceModel model)
        {
            try
            {
                bool status = false;
                string CurrentStatus = "";
                CurrentStatus = model.CurrentStatus;
                AddInvoiceModel rfidetails = new AddInvoiceModel();
                var frmTable = dbContext.GetRfiDetails_sp(model.IrPirID);
                rfidetails = (from r in frmTable
                              select new AddInvoiceModel
                              {
                                  IsProforma = r.IsProforma,
                                  CurrentStatus = r.CurrentStatus
                              }).FirstOrDefault();
                if (rfidetails != null)
                {
                    if (rfidetails.IsProforma == true)
                    {
                        if (rfidetails.CurrentStatus == InvoiceStages.DraftStage)
                            CurrentStatus = InvoiceStages.ApproverStage;
                        if (rfidetails.CurrentStatus == InvoiceStages.ApproverStage)
                            CurrentStatus = InvoiceStages.DraftStage;
                        if (model.ButtonClicked == "Cancelled")
                            CurrentStatus = InvoiceStages.CancelledStage;
                    }
                    if (rfidetails.IsProforma == false)
                    {
                        if (rfidetails.CurrentStatus == InvoiceStages.DraftStage)
                            CurrentStatus = InvoiceStages.ApproverStage;
                        if (rfidetails.CurrentStatus == InvoiceStages.ApproverStage)
                            CurrentStatus = InvoiceStages.FinanceApproverStage;
                        if (rfidetails.CurrentStatus == InvoiceStages.FinanceApproverStage)
                            CurrentStatus = InvoiceStages.ApprovedStage;
                        if (rfidetails.CurrentStatus == InvoiceStages.ApprovedStage)
                            CurrentStatus = InvoiceStages.ApprovedStage;
                        if (model.ButtonClicked == "Cancelled")
                            CurrentStatus = InvoiceStages.CancelledStage;
                    }
                }
                else
                {
                    CurrentStatus = "Draft";
                }
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                dbContext.AddUpdateRFIStatus_sp(model.IrPirID, model.ProjectID, CurrentStatus, model.Comments, model.RFIRaisedBy, model.Action, Output);
                status = Convert.ToBoolean(Output.Value);
                return status;
            }
            catch
            {
                throw;
            }
        }

        public InvoiceDetails GetDetailsForInvoiceByRFIID(int RFIID)
        {
            InvoiceDetails invoiceDetails = new InvoiceDetails();
            try
            {
                var details = dbContext.GetDetailsForInvoiceByRFIID(RFIID);
                invoiceDetails = (from r in details
                                  select new InvoiceDetails
                                   {
                                       ContractID = r.ContractID,
                                       ContractSummary = r.ContractSummary,
                                       CustomerAddressID = r.CustomerId,
                                       CustomerAddress = r.Address,
                                       SalesPeriodID = r.SalesPeriodID,
                                       SalesPeriod = r.SalesPeriod
                                   }).FirstOrDefault();
                return invoiceDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InvoiceIR_PIRDetails> getProjectApproverList(int ProjectID)
        {
            List<InvoiceIR_PIRDetails> invoiceDetails = new List<InvoiceIR_PIRDetails>();
            try
            {
                var details = dbContext.GetProjectIRApprovalList_SP(ProjectID);
                invoiceDetails = (from r in details
                                  select new InvoiceIR_PIRDetails
                                   {
                                       ApproverID = r.ApproverId,
                                       ProjedtID = r.ProjectId,
                                       EmailID = r.emailId
                                   }).ToList();
                return invoiceDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InvoiceCheckList> LoadInvoiceCheckList(int? RFIID)
        {
            // RFIID = 2401;
            try
            {
                List<InvoiceCheckList> chkList = new List<InvoiceCheckList>();

                var Details = dbContext.GetInvoiceCheckListDetails_SP();

                chkList = (from d in Details
                           where d.RFIID == RFIID
                           select new InvoiceCheckList
                           {
                               RFIID = d.RFIID,
                               RFIChecklistInstanceID = d.RFIChecklistInstanceID,
                               RFIChecklistInstanceItemID = d.RFIChecklistInstanceID,
                               RFIChecklistItem = d.RFIChecklistItem,
                               RFIChecklistItemID = d.RFIChecklistItemID,
                               Comments = d.Comments,
                               RFIChecklistItemResponse = d.RFIChecklistItemResponse
                           }).ToList();

                //totalCount = chkList.Count();

                return chkList.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InvoiceCheckList> GetCheckListDetails()
        {
            try
            {
                List<InvoiceCheckList> chkList = new List<InvoiceCheckList>();

                var Details = dbContext.GetRFICheckListItems_SP();

                chkList = (from d in Details
                           select new InvoiceCheckList
                           {
                               RFIChecklistItemID = d.RFIChecklistItemID,
                               RFIChecklistItem = d.RFIChecklistItem,
                               RFIChecklistID = d.RFIChecklistID
                           }).ToList();
                return chkList.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Generate Invoice

        public List<SalesPeriodMonthList> GetSalesPeriodMonthList()
        {
            var dataList = dbContext.GetInvoiceSalesPeriodMonthList_SP();
            List<SalesPeriodMonthList> data = (from d in dataList
                                               select new SalesPeriodMonthList
                                             {
                                                 SalesPeriodMonthID = d.SalesPeriodMonthID,
                                                 SalesPeriodMonth = d.SalesPeriodMonth
                                             }).ToList();
            return data;
        }

        public List<SalesPeriodYearList> GetSalesPeriodYearList()
        {
            var dataList = dbContext.GetInvoiceSalesPeriodYearList_SP();
            List<SalesPeriodYearList> data = (from d in dataList
                                              select new SalesPeriodYearList
                                            {
                                                SalesPeriodYearID = d.YearID,
                                                SalesPeriodYear = d.YearName
                                            }).ToList();
            return data;
        }

        public List<SalesPeriodIsOpenList> GetSalesPeriodIsOpenList()
        {
            var dataList = dbContext.GetInvoiceSalesPeriodIsOpenList_SP();
            List<SalesPeriodIsOpenList> data = (from d in dataList
                                                select new SalesPeriodIsOpenList
                                              {
                                                  SalesPeriodIsOpenID = d.SalesPeriodIsOpenID,
                                                  SalesPeriodIsOpen = d.SalesPeriodIsOpen
                                              }).ToList();
            return data;
        }

        public List<AddSalesPeriods> SalesPeriodDetailRecord(int page, int rows, out int totalCount)
        {
            try
            {
                List<AddSalesPeriods> Records = new List<AddSalesPeriods>();

                var Details = dbContext.GetInvoiceSalesPeriodDetails_sp();
                Records = (from d in Details
                           select new AddSalesPeriods
                           {
                               SalesPeriodID = d.SalesPeriodID,
                               SalesPeriodMonth = d.SalesPeriodMonth,
                               SalesPeriodYear = d.SalesPeriodYear,
                               SalesPeriodStartDate = d.SalesPeriodStartDate,
                               SalesPeriodEndDate = d.SalesPeriodEndDate,
                               SalesPeriodIsOpen = d.SalesPeriodIsOpen,
                               HiddenSalesPeriodMonthID = d.SalesPeriodMonthID,
                               HiddenSalesPeriodYearID = d.SalesPeriodYearID,
                               HiddenSalesPeriodIsOpenID = d.SalesPeriodIsOpenID
                           }).ToList();

                totalCount = Records.Count();

                return Records.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveCheckListDetails(List<InvoiceCheckList> model)
        {
            try
            {
                bool status = false;
                ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                AddInvoiceModel InvoiceDetails = new AddInvoiceModel();
                string EmployeeCode = Membership.GetUser().UserName;
                string UserName = dbContext.tbl_PM_Employee_SEM.Where(x => x.EmployeeCode == EmployeeCode).FirstOrDefault().UserName;
                if (model.Count > 0)
                {
                    decimal ChecklistInstanceID = 0;
                    int? RFIID = 0;
                    int? ProjecID = 0;
                    bool IsProforma = false;
                    int? TypeID = 0;
                    for (int i = 0; i < 1; i++)
                    {
                        ProjecID = model[0].ProjectID;
                        RFIID = model[0].RFIID;
                        IsProforma = model[0].IsProforma;
                        TypeID = model[0].TypeID;
                        var SaveCheckList = dbContext.usp_Ins_tbl_PM_RFIChecklistInstances_SP(null, ProjecID, 1, RFIID, null);
                        foreach (var item in SaveCheckList)
                        {
                            ChecklistInstanceID = Convert.ToDecimal(item.ChecklistInstanceID);
                        }
                    }
                    int count = model.Count;
                    int ChecklistInstanceId = 0;
                    for (int m = 0; m < count; m++)
                    {
                        int? ProjecId = model[m].ProjectID;
                        //int? RFIId = model[m].RFIID;
                        int? RFIChecklistItemID = model[m].RFIChecklistItemID;
                        bool isCheckListChecked = model[m].isCheckListChecked;
                        string Comments = model[m].Comments;
                        int RFIChecklistID = 1;
                        string CreatedBy = UserName;
                        ChecklistInstanceId = Convert.ToInt32(ChecklistInstanceID);
                        var ChecklistInstanceItemID = dbContext.usp_Ins_tbl_PM_RFIChecklistInstance_Items(null, ChecklistInstanceId, ProjecId, RFIChecklistID, RFIChecklistItemID, isCheckListChecked, Comments, CreatedBy);
                    }
                    dbContext.UpdateChecklistInstanceID_SP(ChecklistInstanceId, RFIID, Result);
                    InvoiceDetails.RFIID = RFIID.HasValue ? RFIID.Value : 0;
                    InvoiceDetails.ProjectID = ProjecID.HasValue ? ProjecID.Value : 0; ;
                    InvoiceDetails.Comments = null;
                    InvoiceDetails.Action = "Move Ahead";
                    InvoiceDetails.IrPirID = RFIID.HasValue ? RFIID.Value : 0;
                    InvoiceDetails.RFIRaisedBy = UserName;
                    InvoiceDetails.IsProforma = IsProforma;
                    InvoiceDetails.TypeID = TypeID.HasValue ? TypeID.Value : 0;
                }
                status = Convert.ToBoolean(Result.Value);
                bool submitStatus = false;
                if (status == true)
                {
                    AddInvoiceResponse response = this.SavePIRHistoryDetails(InvoiceDetails.RFIID, InvoiceDetails.TypeID);
                    submitStatus = this.SubmitInvoiceDetails(InvoiceDetails);
                }
                if (submitStatus == true)
                    status = true;
                else
                    status = false;
                return status;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ResourcePoolListDetails> getResoucePoolNames()
        {
            List<ResourcePoolListDetails> resourcePoolName = new List<ResourcePoolListDetails>();
            var resourcePools = dbContext.GetResoucePoolType_SP();
            resourcePoolName = (from r in resourcePools
                                select new ResourcePoolListDetails
                                {
                                    ResourcePoolId = r.ResourcePoolID,
                                    ResourcePoolName = r.ResourcePoolName
                                }).ToList();
            return resourcePoolName;
        }

        public List<BillingTypes> getBillingTypes()
        {
            List<BillingTypes> BillingTypes = new List<BillingTypes>();
            var BillingTypess = dbContext.GetBillingType_SP();
            BillingTypes = (from r in BillingTypess
                            select new BillingTypes
                                {
                                    BillingID = r.Id,
                                    BillingType = r.BillingType
                                }).ToList();
            return BillingTypes;
        }

        public List<TypeOfIR> getTypeOfIR()
        {
            List<TypeOfIR> IRTypes = new List<TypeOfIR>();
            var IRType = dbContext.GetTypeOfIR_SP();
            IRTypes = (from r in IRType
                       select new TypeOfIR
                           {
                               IRID = r.typeIRID,
                               IRDescription = r.typeIRDescription
                           }).ToList();
            return IRTypes;
        }

        public List<InvoiceTypeItemModel> GetFixBidDetails(int RFIID, int page, int rows, out int totalCount)
        {
            try
            {
                List<InvoiceTypeItemModel> Records = new List<InvoiceTypeItemModel>();

                var Details = dbContext.GetGridloadForInvoice_SP(RFIID);

                Records = (from d in Details
                           select new InvoiceTypeItemModel
                           {
                               RFIItemID = d.RFIItemID,
                               RFIID = d.RFIID,
                               ItemDescription = d.ItemDescription,
                               IsDiscountItem = d.IsDiscountItem.HasValue ? (d.IsDiscountItem.Value == true ? "Yes" : "No") : "No",
                               Quantity = d.Quantity,
                               Rate = d.Rate,
                               Amount = d.Amount,
                               BillableResources = d.CustomField1,
                               Billing_Type_Adv_1_Post_2 = Convert.ToString(d.CustomField2) == "1" ? "Adv" : "Post",
                               Type_FB_1_FF_2_Trans_3 = Convert.ToString(d.CustomField3) == "1" ? "Fix Bid" : Convert.ToString(d.CustomField3) == "2" ? "Fix Fee" : Convert.ToString(d.CustomField3) == "3" ? "Trans" : "Select",
                               ResourcePoolType = d.CustomField4,
                               CompanyBaseCurrencyAmount = d.CompanyBaseCurrencyAmount,
                               CorporateBaseAmount = d.CompanyBaseCurrencyAmount
                           }).ToList();
                totalCount = Records.Count();
                return Records.Skip((page - 1) * rows).Take(rows).ToList();
                //return Records.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveFixBidDetails(InvoiceTypeItemModel model, int? TypeID, int? RFIID, int? SelectedResourcepoolType)
        {
            try
            {
                bool status = false;
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                string mode = string.Empty;
                if (model.RFIItemID == 0)
                    mode = "Add";
                else
                    mode = "Update";

                string EmployeeCode = Membership.GetUser().UserName;
                string UserName = dbContext.tbl_PM_Employee_SEM.Where(x => x.EmployeeCode == EmployeeCode).FirstOrDefault().UserName;
                if (model.Quantity == null)
                    model.Quantity = 1;
                double? amount = Math.Round(Convert.ToDouble(model.Rate * model.Quantity), 2);
                int resoucrPoolID = Convert.ToInt32(model.ResourcePoolType);
                var ResourcePools = dbContext.GetResouceNameFromResourceID_SP(SelectedResourcepoolType);
                string resourcePoolName = string.Empty;

                foreach (var item in ResourcePools)
                {
                    resourcePoolName = item.ResourcePoolName;
                }
                double BillingType = Convert.ToDouble(model.Billing_Type_Adv_1_Post_2);
                double IRType = 0.0;
                if (TypeID == 6)
                    IRType = Convert.ToDouble(model.Type_FB_1_FF_2_Trans_3);
                bool isdiscount = model.IsDiscountItem == "0" ? false : true;
                dbContext.AddUpdateInvoiceTypeDetails_SP(TypeID, model.RFIItemID, RFIID, model.ItemDescription, isdiscount, model.Quantity, model.Rate, amount, model.BillableResources, BillingType, IRType, resourcePoolName, mode, UserName, Output);
                status = Convert.ToBoolean(Output.Value);
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteInvoiceTypeDetails(string[] SelectedRFIItemID)
        {
            try
            {
                bool status = false;
                for (int i = 0; i < SelectedRFIItemID.Length; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(string));
                    string[] RFIIItemID = SelectedRFIItemID[i].Split('_');
                    int RFIItemID = Convert.ToInt32(RFIIItemID[1]);
                    dbContext.DeleteInvoiceTypeDetails_sp(RFIItemID, Output);
                    if (Output.Value.ToString() == "Success")
                        status = true;
                    else
                        status = false;
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteSalesPeriodRecord(string[] SelectedModuleId)
        {
            try
            {
                bool status = false;
                for (int i = 0; i < SelectedModuleId.Length; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                    int SalesPeriodID = Convert.ToInt32(SelectedModuleId[i]);
                    dbContext.DeleteSalesPeriodDetails_sp(SalesPeriodID, Output);
                    status = Convert.ToBoolean(Output.Value);
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SEMResponse SaveSalesPeriodRecord(AddSalesPeriods model, int SalesPeriodMonthID, int SalesPeriodYearID, int SalesPeriodIsOpenID, string LoggedUserName, string SalesStartDate, string SalesEndDate)
        {
            try
            {
                SEMResponse response = new SEMResponse();
                response.isModuleNameExist = false;
                response.status = false;
                int? SalesPeriodMonth = SalesPeriodMonthID;
                int? SalesPeriodYear = SalesPeriodYearID;
                int? IsOpen = SalesPeriodIsOpenID;
                int SalesPeriodID = model.SalesPeriodID;
                DateTime? SalesPeriodStartDate = null;
                DateTime? SalesPeriodEndDate = null;

                var Records = dbContext.GetSalesPeriodRecord_sp(SalesPeriodMonth, SalesPeriodYear);

                var Details = (from m in Records
                               select new AddSalesPeriods
                               {
                                   SalesPeriodID = m.SalesPeriodID,
                                   isOpenID = m.IsOpen
                               }).ToList();
                int? isOpenData = 0;
                foreach (var item in Details)
                {
                    isOpenData = Convert.ToInt32(item.isOpenID);
                }

                if (Details.Count == 1 && isOpenData != SalesPeriodIsOpenID)
                {
                    IsOpen = SalesPeriodIsOpenID;
                }

                if (Details.Count == 0 || (Details.Count == 1 && isOpenData != SalesPeriodIsOpenID))
                {
                    string Operation = "";
                    if (model.SalesPeriodID == 0)
                    {
                        Operation = "INSERT";
                        SalesPeriodStartDate = Convert.ToDateTime(SalesStartDate);
                        SalesPeriodEndDate = Convert.ToDateTime(SalesEndDate);
                    }
                    else
                    {
                        Operation = "UPDATE";
                        SalesPeriodStartDate = null;
                        SalesPeriodEndDate = null;
                    }

                    ObjectParameter Output = new ObjectParameter("Result", typeof(int));

                    dbContext.AddUpdateInvoiceSalesPeriodDetails_sp(SalesPeriodID, SalesPeriodMonth, SalesPeriodYear, SalesPeriodStartDate, SalesPeriodEndDate, IsOpen, LoggedUserName, Operation, Output);

                    response.status = Convert.ToBoolean(Output.Value);
                    return response;
                }
                else
                {
                    response.isModuleNameExist = true;
                    return response;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ContractTypes> getContractTypeList()
        {
            try
            {
                var contractTypes = dbContext.GetContractTypes_SP();
                List<ContractTypes> contracttypelist = (from e in contractTypes
                                                        select new ContractTypes
                                                          {
                                                              ContractTypeID = e.ContractTypeID,
                                                              ContractTypeName = e.ContractTypeName
                                                          }).ToList();
                return contracttypelist;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ContractDetails> LoadContractDetails(int? customerID, int? ContractTypeID, int page, int rows, out int totalCount)
        {
            List<ContractDetails> reallocationRecords = new List<ContractDetails>();
            try
            {
                int ContractTypeId = ContractTypeID.HasValue ? ContractTypeID.Value : 0;
                var reallocationEmployeeList = dbContext.GetContractDetailsFromCustomerID_SP(customerID, ContractTypeId);

                reallocationRecords = (from r in reallocationEmployeeList
                                       select new ContractDetails
                                       {
                                           customerName = r.CustomerName,
                                           ContractID = r.ContractID,
                                           ContractTypeName = r.ContractTypeName,
                                           ContractSummary = r.ContractSummary,
                                           ContractSigningDate = r.ContractStartDate,
                                           ContractExpiryDate = r.ContractEndDate,
                                           CommencementDate = r.ContractSummaryDate
                                       }).ToList();

                totalCount = reallocationRecords.Count();
                return reallocationRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesPeriod> LoadSalesPeriodDetails(int page, int rows, out int totalCount)
        {
            List<SalesPeriod> SalesPeriodRecords = new List<SalesPeriod>();
            try
            {
                var SalesPeriodRecordsList = dbContext.GetSalesPeriod_SP();

                SalesPeriodRecords = (from r in SalesPeriodRecordsList
                                      select new SalesPeriod
                                      {
                                          SalesPeriodID = r.SalesPeriodID,
                                          SalesPeriodMonth = r.SalesPeriodMonth,
                                          SalesPeriodYear = r.SalesPeriodYear,
                                          SalesPeriodStartDate = r.SalesPeriodStartDate,
                                          SalesPeriodEndDate = r.SalesPeriodEndDate,
                                          IsOpen = r.IsOpen == true ? "Yes" : "No"
                                      }).ToList();

                totalCount = SalesPeriodRecords.Count();
                return SalesPeriodRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InvoiceCustomerAddress> CustomerAddressDetails(int? customerID, int page, int rows, out int totalCount)
        {
            List<InvoiceCustomerAddress> CustomerAddressRecords = new List<InvoiceCustomerAddress>();
            try
            {
                var CustomerAddressRecordsList = dbContext.GetCustomerAddress_SP(customerID);

                CustomerAddressRecords = (from r in CustomerAddressRecordsList
                                          select new InvoiceCustomerAddress
                                          {
                                              CustomerAddressID = r.CustomerId,
                                              Address = r.Address,
                                              City = r.City,
                                              PhoneNumber = r.PhoneNumber,
                                              Country = r.CountryName,
                                              Details = r.Details,
                                              EmailID = r.EmailId,
                                              state = r.State,
                                          }).ToList();

                totalCount = CustomerAddressRecords.Count();
                return CustomerAddressRecords.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DocumentCategoryList> GetDocumentCategoryList()
        {
            var dataList = dbContext.GetDocumentCategoryList_SP();
            List<DocumentCategoryList> data = (from d in dataList
                                               select new DocumentCategoryList
                                               {
                                                   DocumentCategoryID = d.CategoryID,
                                                   DocumentCategory = d.Category
                                               }).ToList();
            return data;
        }

        public List<MainRoleDetails> GetRoleDetails()
        {
            var dataList = dbContext.GetRoleDetails_SP();
            List<MainRoleDetails> data = (from d in dataList
                                          select new MainRoleDetails
                                               {
                                                   RoleID = d.RoleID,
                                                   RoleDescription = d.RoleDescription,
                                               }).ToList();
            return data;
        }

        public List<MainRoleDetails> LoadConfigurationRoleDetailsMapping(int RoleID)
        {
            List<MainRoleDetails> Records = new List<MainRoleDetails>();
            try
            {
                var data = dbContext.GetConfigurationRoleDetailsMappingByRoleID_sp(RoleID);
                Records = (from d in data
                           select new MainRoleDetails
                           {
                               RoleDescription = d.RoleDescription,
                               RoleID = d.RoleID,
                               ProjectCreator = d.ProjectCreator.HasValue ? d.ProjectCreator.Value : false,
                               ResourceAllocator = d.ResourceAllocator.HasValue ? d.ResourceAllocator.Value : false,
                               IRGenerator = d.IsAuthorisedToGenerateInvoice,
                               IRApprover = d.AuthorizedForRFIApproval.HasValue ? d.AuthorizedForRFIApproval.Value : false,
                               IRFinanceApprover = d.IRFinanceApprover.HasValue ? d.IRFinanceApprover.Value : false,
                               TimesheetToBeFilled = d.TimesheetToBeFilled.HasValue ? d.TimesheetToBeFilled.Value : false,
                               DocumentCategoryAccess = d.DocumentCategoryAccess
                           }).ToList();
                return Records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SEMResponse SaveDocumentCategory(ConfigurationRoleModel model)
        {
            try
            {
                SEMResponse response = new SEMResponse();
                response.status = false;
                int RoleID = model.RoleID;
                bool ProjectCreator = model.ProjectCreator;
                bool ResourceAllocator = model.ResourceAllocator;
                bool IRGenerator = model.IRGenerator;
                bool IRApprover = model.IRApprover;
                bool IRFinanceApprover = model.IRFinanceApprover;
                bool TimesheetToBeFilled = model.TimesheetToBeFilled;
                var test = model.SelectedItemId.AsEnumerable().ToList();
                string DocumentCategoryAccess = "";

                for (int i = 0; i < test.Count; i++)
                {
                    DocumentCategoryAccess += test[i] + ",";
                }
                DocumentCategoryAccess = DocumentCategoryAccess.TrimEnd(',');

                ObjectParameter Result = new ObjectParameter("Result", typeof(int));

                dbContext.SaveConfigurationRoleDetails_sp(RoleID, ProjectCreator, ResourceAllocator, IRGenerator, IRApprover, IRFinanceApprover, TimesheetToBeFilled, DocumentCategoryAccess, Result);

                response.status = Convert.ToBoolean(Result.Value);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SearchedUserDetails GetEmployeeDetails(string UserName)
        {
            HRMSDBEntities dbContextHRMS = new HRMSDBEntities();
            int EmployeeID = 0;
            var employeeDetailsList = dbContextHRMS.GetEmployeeDetailsHRMS_sp(EmployeeID, UserName);
            SearchedUserDetails employeeDetail = (from e in employeeDetailsList
                                                  where e.USERNAME == UserName
                                                  select new SearchedUserDetails
                                                  {
                                                      EmployeeFullName = e.EmployeeName,
                                                      EmployeeId = e.EmployeeID,
                                                      EmployeeCode = e.EmployeeCode,
                                                      UserName = e.USERNAME,
                                                      EmployeeEmailId = e.EmailID,
                                                      Describtion = (from relation in dbContextHRMS.HRMS_tbl_PM_Role
                                                                     where relation.RoleID == e.PostID
                                                                     select relation.RoleDescription).FirstOrDefault(),
                                                      repotingTo = e.ReportingTo
                                                  }).FirstOrDefault();
            return employeeDetail;
        }

        public bool RejectInvoiceDetails(AddInvoiceModel model)
        {
            try
            {
                bool status = false;
                string CurrentStatus = "";
                CurrentStatus = model.CurrentStatus;
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                dbContext.RejectRFIStatus_sp(model.IrPirID, model.ProjectID, CurrentStatus, model.Comments, model.RFIRaisedBy, model.Action, Output);
                status = Convert.ToBoolean(Output.Value);
                return status;
            }
            catch
            {
                throw;
            }
        }

        public List<ProjectIRFinanceApprovers> GetProjectIRFinanceApprovalList(int ProjectID)
        {
            var dataList = dbContext.GetProjectIRFinanceApprovalList_SP(ProjectID);
            List<ProjectIRFinanceApprovers> data = (from d in dataList
                                                    select new ProjectIRFinanceApprovers
                                                      {
                                                          EmployeeId = d.FinanceApproverId,
                                                          EmployeeEmailId = d.emailId
                                                      }).ToList();
            return data;
        }

        public List<HRMS.Models.FinancePaymentTrackingModel.ClientListInvoiceApproved> GetClientNamesForFinancePaymentTracking()
        {
            var dataList = dbContext.GetClientNamesForFinancePaymentTracking_SP();
            List<HRMS.Models.FinancePaymentTrackingModel.ClientListInvoiceApproved> data = (from d in dataList
                                                                                            select new HRMS.Models.FinancePaymentTrackingModel.ClientListInvoiceApproved
                                                                                              {
                                                                                                  ClientId = d.ClientID,
                                                                                                  ClientName = d.ClientName
                                                                                              }).ToList();
            return data;
        }

        public List<HRMS.Models.FinancePaymentTrackingModel.InvoiceDeliveryUnitList> GetDeliveryUnitListdata()
        {
            var dataList = dbContext.GetInvoiceDeliveryUnit_SP();
            List<HRMS.Models.FinancePaymentTrackingModel.InvoiceDeliveryUnitList> data = (from d in dataList
                                                                                          select new HRMS.Models.FinancePaymentTrackingModel.InvoiceDeliveryUnitList
                                                                                            {
                                                                                                DeliveryUnitId = d.InvoiceDeliveryUnitID,
                                                                                                DeliveryUnitName = d.InvoiceDeliveryUnitName
                                                                                            }).ToList();
            return data;
        }

        public List<FinancePaymentTrackingModel> LoadFinancePaymentTracking(int? ClientID, int? DeliveryUnitID, int page, int rows, out int totalCount)
        {
            List<FinancePaymentTrackingModel> Records = new List<FinancePaymentTrackingModel>();
            try
            {
                if (ClientID == 0 || ClientID == null)
                {
                    ClientID = 0;
                }
                if (DeliveryUnitID == 0)
                {
                    DeliveryUnitID = null;
                }

                var data = dbContext.GetFinancePaymentTrackingDetails_SP(ClientID, DeliveryUnitID);

                Records = (from r in data
                           select new FinancePaymentTrackingModel
                           {
                               RFIItemID = r.RFIItemID,
                               RFIID = r.RFIID,
                               ClientName = r.ClientName,
                               //ClientNameID=r.Customer,
                               SystemInvoiceNumber = r.RFIItemID,
                               InvoiceMonthDate = r.InvoiceMonthDate,
                               PaymentTerm = r.PaymentTerm,
                               ReportDate = DateTime.Now,
                               PendingAmount = r.PendingAmount,
                               Amount = r.Amount,
                               Status = r.Status,
                               Days = r.Days,
                               DeliveryUnit = r.DeliveryUnitName,
                               hdnDeliveryUnit = r.DeliveryUnit,
                               QuickBooksInvoiceNumber = r.QuickBooksInvoiceNumber,
                               InvoiceSentDate = r.InvoiceSentDate,
                               ExpectedPaymentDate = r.ExpectedPaymentDate,
                               CollectionDate = r.CollectionDate,
                               CollectedAmount = r.CollectedAmount,
                               hdnCollectedAmount = r.CollectedAmount,
                               hdnExpectedPaymentDate = r.ExpectedPaymentDate,
                               hdnDays = r.Days,
                               hdnPendingAmount = r.PendingAmount
                           }).ToList();

                totalCount = Records.Count();
                return Records.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveFinanceTrackingDetails(FinancePaymentTrackingModel model, DateTime? ExpectedPaymentDate, int? days, double? pendingAmount, int? deliveryUnit)
        {
            try
            {
                bool status = false;
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));

                string Days = Convert.ToString(days);
                string DeliveryUnit = Convert.ToString(deliveryUnit);

                Decimal? Amount = Convert.ToDecimal(model.Amount);
                Decimal? PendingAmount = Convert.ToDecimal(pendingAmount);

                string mode = string.Empty;

                mode = "Add";

                string EmployeeCode = Membership.GetUser().UserName;
                string UserName = dbContext.tbl_PM_Employee_SEM.Where(x => x.EmployeeCode == EmployeeCode).FirstOrDefault().UserName;

                dbContext.AddUpdateFinancePaymentDataTrackingDetails_SP(model.RFIItemID, model.RFIID, model.ClientName, DeliveryUnit, model.SystemInvoiceNumber, model.QuickBooksInvoiceNumber, model.InvoiceMonthDate,
                    model.InvoiceSentDate, model.ExpectedPaymentDate, model.ReportDate, model.PaymentTerm, Days, Amount, model.Status, model.CollectionDate, model.CollectedAmount,
                    PendingAmount, mode, UserName, Output);
                status = Convert.ToBoolean(Output.Value);

                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<HRMS.Models.FinanceTrackingSummaryModel.DeliveryUnitList> GetDeliveryUnitdata()
        {
            var dataList = dbContext.GetPMSDeliveryUnit_SP();
            List<HRMS.Models.FinanceTrackingSummaryModel.DeliveryUnitList> data = (from d in dataList
                                                                                   select new HRMS.Models.FinanceTrackingSummaryModel.DeliveryUnitList
                                                                                    {
                                                                                        DeliveryUnitId = d.ResourcePoolID,
                                                                                        DeliveryUnitName = d.ResourcePoolName
                                                                                    }).ToList();
            return data;
        }

        public List<HRMS.Models.FinanceTrackingSummaryModel.InvoiceStatusList> GetStatusListDataForFinancePaymentOutstanding()
        {
            var dataList = dbContext.GetStatusListDataForFinancePaymentOutstanding_SP();
            List<HRMS.Models.FinanceTrackingSummaryModel.InvoiceStatusList> data = (from d in dataList
                                                                                    select new HRMS.Models.FinanceTrackingSummaryModel.InvoiceStatusList
                                                                                    {
                                                                                        StatusID = d.StatusID,
                                                                                        StatusName = d.StatusName
                                                                                    }).ToList();
            return data;
        }

        public List<FinanceTrackingSummaryModel> LoadFinanceOutStandingData(int? ClientID, int? DeliveryUnitID, int? StatusID, int page, int rows, out int totalCount)
        {
            List<FinanceTrackingSummaryModel> Records = new List<FinanceTrackingSummaryModel>();
            try
            {
                var data = dbContext.GetFinancePaymentOutStandingDataDetails_SP(ClientID, DeliveryUnitID, StatusID);

                Records = (from r in data
                           select new FinanceTrackingSummaryModel
                           {
                               //RFIID=1810,
                               ClientName = r.clientName,
                               Current = r.CurrentAmount.ToString(),
                               Days1To30 = r.Days1To30.ToString(),
                               Days31To60 = r.Days31To60.ToString(),
                               Days61To90 = r.Days61To90.ToString(),
                               DaysAbove90 = r.DaysAbove90.ToString(),
                               CollectionAmount = r.CollectionAmount.ToString(),
                               LastPaymentDate = r.LastPaymentDate,
                               PaymentTerms = r.PaymentTerms,
                               Total = r.Total.ToString(),
                               Status = r.StatusName,
                               DeliveryUnit = r.DeliveryUnitName
                           }).ToList();

                totalCount = Records.Count();
                return Records.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveFinanceOutstandingDataDetails(FinanceTrackingSummaryModel model, int? deliveryUnit, int? statusval)
        {
            try
            {
                bool status = false;
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));

                string DeliveryUnit = Convert.ToString(deliveryUnit);
                string Status = Convert.ToString(statusval);

                string mode = string.Empty;

                mode = "Add";

                string EmployeeCode = Membership.GetUser().UserName;
                string UserName = dbContext.tbl_PM_Employee_SEM.Where(x => x.EmployeeCode == EmployeeCode).FirstOrDefault().UserName;

                dbContext.AddUpdateFinanceOutstandingDataDetails_SP(model.RFIItemID, model.RFIID, model.ClientName, DeliveryUnit, Status, model.Current, model.Days1To30,
                    model.Days31To60, model.Days61To90, model.DaysAbove90, model.Total, model.PaymentTerms, model.CollectionAmount, model.LastPaymentDate, mode, UserName, Output);
                status = Convert.ToBoolean(Output.Value);

                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<HRMS.Models.FinancePaymentTrackingModel.FinancePaymentDataHistoryList> GetFinancePaymentTrackingDataViewHistory(int RFIItemID)
        {
            List<HRMS.Models.FinancePaymentTrackingModel.FinancePaymentDataHistoryList> Records = new List<HRMS.Models.FinancePaymentTrackingModel.FinancePaymentDataHistoryList>();
            try
            {
                var data = dbContext.GetFinancePaymentTrackingDataViewHistory_SP(RFIItemID);

                Records = (from r in data
                           select new HRMS.Models.FinancePaymentTrackingModel.FinancePaymentDataHistoryList
                           {
                               CollectedAmount = r.CollectedAmount.ToString(),
                               CollectionDate = r.CollectionDate,
                               PendingAmount = r.PendingAmount,
                               Status = r.Status,
                               UpdatedBy = r.ModifiedBy,
                               UpdatedOn = r.ModifiedOn
                           }).ToList();

                return Records.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<HRMS.Models.FinanceTrackingSummaryModel.FinanceOutStandingDataHistoryList> GetFinanceOutStandingTrackingDataViewHistory(int RFIID)
        {
            List<HRMS.Models.FinanceTrackingSummaryModel.FinanceOutStandingDataHistoryList> Records = new List<HRMS.Models.FinanceTrackingSummaryModel.FinanceOutStandingDataHistoryList>();
            try
            {
                var data = dbContext.GetFinanceOutStandingTrackingDataViewHistory_SP(RFIID);

                Records = (from r in data
                           select new HRMS.Models.FinanceTrackingSummaryModel.FinanceOutStandingDataHistoryList
                           {
                               Total = r.Total,
                               CollectionAmount = r.CollectionAmount,
                               LastPaymentDate = r.LastPaymentDate
                           }).ToList();

                return Records.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}