using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class GenerateInvoiceModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public CheckListPopUp CheckListPopUp { get; set; }
        public List<ProjectAppList> IRGeneratorProjectList { get; set; }
        public List<InvoiceTypeList> TypeList { get; set; }
        public List<InvoiceStageList> StageList { get; set; }
        public List<InvoiceNameList> NameList { get; set; }
        public string StageNameFilter { get; set; }
        public string InvoiceNameFilter { get; set; }
        public string UserName { get; set; }
        public int InvoiceProjectID { get; set; }
        public int TypeID { get; set; }
        public AddSalesPeriodPopUp AddSalesPeriodPopUp { get; set; }

        // public SalesPeriod SalesPeriod { get; set; }
        public InvoiceCustomerAddressPopUp InvoiceCustomerAddressPopUp { get; set; }

        public int SelectedProjectID { get; set; }
    }

    public class InvoiceTypeList
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }
    }

    public class InvoiceStageList
    {
        public string StageName { get; set; }
    }

    public class InvoiceNameList
    {
        public string InvoiceName { get; set; }
    }

    public class IRApproverClass
    {
        public int? IRApproverID { get; set; }
    }

    public class InvoiceProjectDetails
    {
        public int ProjectID { get; set; }
        public int? ProjectCurrencyID { get; set; }
        public int IRApproverCount { get; set; }
        public int? IRApproverID { get; set; }
        public int IRFinanceApproverCount { get; set; }
        public int? IRFinanceApproverID { get; set; }
    }

    public class InvoiceDetailsModel
    {
        public int? RFIID { get; set; }
        public string InvoiceName { get; set; }
        public string InvoiceStage { get; set; }
        public DateTime? RFIRaisedOn { get; set; }
        public double? Amount { get; set; }
        public double EquivalentCurrencyAmount { get; set; }
        public double? CorporateBaseAmount { get; set; }
        public int? ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string RFIType { get; set; }
        public string CurrencyName { get; set; }
    }

    public class InvoiceShowStatusModel
    {
        public string ShowstatusEmployeeName { get; set; }

        public string ShowstatusCurrentStage { get; set; }

        public DateTime? ShowstatusTime { get; set; }

        public string ShowstatusActor { get; set; }

        public string ShowstatusAction { get; set; }

        public string ShowstatusComments { get; set; }

        public string showStatus { get; set; }
    }
}