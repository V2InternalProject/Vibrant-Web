using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class AddInvoiceModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public CheckListPopUp CheckListPopUp { get; set; }
        public List<InvoiceCheckList> LstCheckList { get; set; }
        public string IsViewDetails { get; set; }
        public List<InvoiceTypeList> TypeList { get; set; }
        public int TypeID { get; set; }
        public string CustomerName { get; set; }
        public List<CustomerContactPerson> CustomerContactPerson { get; set; }
        public int CustomerContactID { get; set; }
        public string CustomerAddress { get; set; }
        public int CustomerID { get; set; }
        public string Contract { get; set; }
        public string SalesPeriod { get; set; }
        public ProjectCurrency Currency { get; set; }
        public string CustomerEmail { get; set; }
        public int CreditDays { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public int IrPirID { get; set; }
        public bool IsProforma { get; set; }
        public CustomerAddressInvoice CustAddress { get; set; }
        public int ProjectID { get; set; }
        public string RFIRaisedBy { get; set; }
        public DateTime RFIRaisedOn { get; set; }
        public string CurrentStatus { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public DateTime DueDate { get; set; }
        public string ConfirmEmailID { get; set; }
        public int ChecklistID { get; set; }
        public int ChecklistInstanceID { get; set; }
        public int SalesPeriodID { get; set; }
        public int ContractID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int MilestoneID { get; set; }
        public int CustomerAddressID { get; set; }
        public string Action { get; set; }
        public string Comments { get; set; }
        public bool IsNewForm { get; set; }
        public string ButtonClicked { get; set; }
        public List<int?> HistoryIDList { get; set; }
        public List<InvoiceTypeItemModel> PIRHistoryDetailsList { get; set; }
        public List<InvoiceTypeHeaders> InvoiceTypeHeadersList { get; set; }
        public TravelMailTemplate Mail { get; set; }
        public int? SelectedResourcePoolType { get; set; }
        public int RFIID { get; set; }
        public string ContractSummary { get; set; }
        //public string SalesPeriod { get; set; }
    }

    public class InvoiceCheckList
    {
        public int? RFIChecklistID { get; set; }
        public int? RFIChecklistItemID { get; set; }
        public int? RFIChecklistInstanceItemID { get; set; }
        public int? RFIChecklistInstanceID { get; set; }
        public string RFIChecklistItem { get; set; }
        public bool? RFIChecklistItemResponse { get; set; }
        public string Comments { get; set; }
        public int? RFIID { get; set; }
        public bool isCheckListChecked { get; set; }
        public int? ProjectID { get; set; }
        public bool IsProforma { get; set; }
        public int TypeID { get; set; }
    }

    public class InvoiceTypeItemModel
    {
        public int RFIItemID { get; set; }

        public int? RFIID { get; set; }

        public string ItemDescription { get; set; }

        public double? Quantity { get; set; }

        public double? Rate { get; set; }

        public double? Amount { get; set; }

        public double BaseCurrencyAmount { get; set; }

        public double? CompanyBaseCurrencyAmount { get; set; }

        public double? BillableResources { get; set; }

        public string IsDiscountItem { get; set; }

        public string Billing_Type_Adv_1_Post_2 { get; set; }

        public string Type_FB_1_FF_2_Trans_3 { get; set; }

        public string ResourcePoolType { get; set; }

        public double? CorporateBaseAmount { get; set; }

        public int? OrderNumber { get; set; }
        public bool InvoiceGenerated { get; set; }
        public double LocalCurrencyAmount { get; set; }
        public double BillingToBaseConversionRate { get; set; }
        public double LocalToBaseConversionRate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public double? CompanyBaseCurrencyConversionRate { get; set; }
        public double? CompanyReportingCurrencyAmount1 { get; set; }
        public double? CompanyReportingCurrencyAmount2 { get; set; }
        public double? CompanyReportingCurrencyAmount3 { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? HistoryID { get; set; }
        public string UpdatedByEmployeeName { get; set; }
        public string ApprovedByEmployeeName { get; set; }
    }

    public class ContractDetails
    {
        public int CustomerID { get; set; }

        public string ContractType { get; set; }

        public List<ContractTypes> ContractTypeList { get; set; }

        public string customerName { get; set; }

        public string ContractSummary { get; set; }

        public string ContractTypeName { get; set; }

        public DateTime? CommencementDate { get; set; }

        public DateTime? ContractSigningDate { get; set; }

        public DateTime? ContractExpiryDate { get; set; }

        public int ContractID { get; set; }
    }

    public class SalesPeriod
    {
        public int SalesPeriodID { get; set; }

        public int? SalesPeriodYear { get; set; }

        public string SalesPeriodMonth { get; set; }

        public DateTime? SalesPeriodStartDate { get; set; }

        public DateTime? SalesPeriodEndDate { get; set; }

        public string IsOpen { get; set; }
    }

    public class InvoiceCustomerAddress
    {
        public int CustomerAddressID { get; set; }

        public int CustomerID { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public string state { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailID { get; set; }

        public string Details { get; set; }
    }

    public class CustomerContactPerson
    {
        public int CustomerContactID { get; set; }
        public string ContactPerson { get; set; }
        public string EmailID { get; set; }
    }

    public class BillingTypes
    {
        public int BillingID { get; set; }

        public string BillingType { get; set; }
    }

    public class TypeOfIR
    {
        public int IRID { get; set; }

        public string IRDescription { get; set; }
    }

    public class NewInvoice
    {
        public int IrPirID { get; set; }
        public int ProjectID { get; set; }
        public bool IsProforma { get; set; }
        public int RfiTypeID { get; set; }
        public int RfiRaisedBy { get; set; }
        public string CurrentStatus { get; set; }
        public int CustomerID { get; set; }
        public int CustomerContactID { get; set; }
        public int CustomerAddressID { get; set; }
        public int BillingCurrencyID { get; set; }
        public int SalesPeriodID { get; set; }
    }

    public class ProjectCurrency
    {
        public int CuurencyID { get; set; }
        public string CurrencyName { get; set; }
    }

    public class CustomerAddressInvoice
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }

    public class AddInvoiceResponse
    {
        public bool Status { get; set; }
    }

    public class InvoiceTypeHeaders
    {
        public int? RFITypeID { get; set; }
        public string HeaderName { get; set; }
    }

    public class ProjectIRFinanceApprovers
    {
        public int? EmployeeId { get; set; }
        public string EmployeeEmailId { get; set; }
    }

    public class InvoiceDetails
    {
        public int? ContractID { get; set; }
        public string ContractSummary { get; set; }
        public int? CustomerAddressID { get; set; }
        public string CustomerAddress { get; set; }
        public int? SalesPeriodID { get; set; }
        public string SalesPeriod { get; set; }
    }

    public class InvoiceIR_PIRDetails
    {
        public int? ProjedtID { get; set; }

        public int? ApproverID { get; set; }

        public string EmailID { get; set; }
    }
}