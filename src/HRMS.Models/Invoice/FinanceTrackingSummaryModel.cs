using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class FinanceTrackingSummaryModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public string EmployeeCode { get; set; }

        public int? EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int? RFIItemID { get; set; }

        public int? RFIID { get; set; }

        public int ClientNameID { get; set; }

        public List<HRMS.Models.FinancePaymentTrackingModel.ClientListInvoiceApproved> ClientListdata = new List<HRMS.Models.FinancePaymentTrackingModel.ClientListInvoiceApproved>();

        public int DeliveryUnitID { get; set; }

        public List<DeliveryUnitList> DeliveryUnitListdata = new List<DeliveryUnitList>();

        public int StatusID { get; set; }

        public List<InvoiceStatusList> StatusListdata = new List<InvoiceStatusList>();

        public int FinancePaymentOutstandingDataID { get; set; }

        public int FinancePaymentTrackingDataID { get; set; }

        public string ClientName { get; set; }

        public string DeliveryUnit { get; set; }

        public string Status { get; set; }

        public string Current { get; set; }

        public string Days1To30 { get; set; }

        public string Days31To60 { get; set; }

        public string Days61To90 { get; set; }

        public string DaysAbove90 { get; set; }

        public string Total { get; set; }

        public int? PaymentTerms { get; set; }

        public string CollectionAmount { get; set; }

        public DateTime? LastPaymentDate { get; set; }

        public List<FinanceOutStandingDataHistoryList> OutStandingDataHistoryList = new List<FinanceOutStandingDataHistoryList>();

        public class DeliveryUnitList
        {
            public int? DeliveryUnitId { get; set; }
            public string DeliveryUnitName { get; set; }
            public int deliveryUnitIdList { get; set; }
        }

        public class InvoiceStatusList
        {
            public int? StatusID { get; set; }
            public string StatusName { get; set; }
        }

        public class FinanceOutStandingDataHistoryList
        {
            public string Total { get; set; }

            public string CollectionAmount { get; set; }

            public DateTime? LastPaymentDate { get; set; }
        }
    }
}