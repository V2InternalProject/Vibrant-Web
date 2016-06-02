using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class FinancePaymentTrackingModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public string EmployeeCode { get; set; }

        public int? EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int? RFIItemID { get; set; }

        public int? RFIID { get; set; }

        public int ClientNameID { get; set; }

        public List<ClientListInvoiceApproved> ClientListdata = new List<ClientListInvoiceApproved>();

        public int DeliveryUnitID { get; set; }

        public List<InvoiceDeliveryUnitList> DeliveryUnitListdata = new List<InvoiceDeliveryUnitList>();

        public List<FinancePaymentDataHistoryList> PaymentDataHistoryList = new List<FinancePaymentDataHistoryList>();

        public int FinancePaymentTrackingDataID { get; set; }

        public string ClientName { get; set; }

        public string DeliveryUnit { get; set; }

        public string hdnDeliveryUnit { get; set; }

        public DateTime? hdnExpectedPaymentDate { get; set; }

        public double? hdnPendingAmount { get; set; }

        public string hdnDays { get; set; }

        public int? SystemInvoiceNumber { get; set; }

        public string QuickBooksInvoiceNumber { get; set; }

        public DateTime? InvoiceMonthDate { get; set; }

        public DateTime? InvoiceSentDate { get; set; }

        public DateTime? ExpectedPaymentDate { get; set; }

        public DateTime? ReportDate { get; set; }

        public int? PaymentTerm { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public string Days { get; set; }

        public double? Amount { get; set; }

        public string Status { get; set; }

        public DateTime? CollectionDate { get; set; }

        public string CollectedAmount { get; set; }

        public string hdnCollectedAmount { get; set; }

        public double? PendingAmount { get; set; }

        public class ClientListInvoiceApproved
        {
            public int? ClientId { get; set; }
            public string ClientName { get; set; }
            public int clientIdList { get; set; }
        }

        public class InvoiceDeliveryUnitList
        {
            public int? DeliveryUnitId { get; set; }
            public string DeliveryUnitName { get; set; }
            public int deliveryUnitIdList { get; set; }
        }

        public class FinancePaymentDataHistoryList
        {
            public DateTime? UpdatedOn { get; set; }

            public string UpdatedBy { get; set; }

            public DateTime? CollectionDate { get; set; }

            public string CollectedAmount { get; set; }

            public string PendingAmount { get; set; }

            public string Status { get; set; }
        }
    }
}