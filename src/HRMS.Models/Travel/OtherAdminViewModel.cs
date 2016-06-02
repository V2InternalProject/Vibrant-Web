using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class OtherAdminViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int RequirementID { get; set; }

        public int ID { get; set; }

        public int StageID { get; set; }

        public int TravelId { get; set; }

        public int? TypeID { get; set; }

        public string Description { get; set; }

        public int? CurrencyID { get; set; }

        public string InsurenceDetails { get; set; }

        [Required]
        [Display(Name = "Requrement Type")]
        public int RequrementTypeID { get; set; }

        [StringLength(500, ErrorMessage = "Maxium 500 characters are allowed")]
        public string Miscdetails { get; set; }

        [StringLength(50, ErrorMessage = "Maxium 50 characters are allowed")]
        public string FileName { get; set; }

        [StringLength(500, ErrorMessage = "Maxium 500 characters are allowed")]
        public string FilePath { get; set; }

        [Display(Name = "Received By Employee")]
        public string AcceptanceID { get; set; }

        public string ReceivedByEmployee { get; set; }

        public string Comments { get; set; }

        public string CurrnyName { get; set; }

        public List<TCurrencyList> CurrencyList { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter numbers only.")]
        public int? Advacesamount { get; set; }

        public List<RequirementType> requirementTypeList { get; set; }

        public List<EmployeeAcceptance> employeeAceptionsList { get; set; }

        public List<OtherAdminViewModel> otheradminViewmodelList { get; set; }

        public bool cash { get; set; }

        public bool card { get; set; }

        public string CardDetails { get; set; }

        public DateTime? InsuranceFromDate { get; set; }
        public DateTime? InsuranceToDate { get; set; }

        public string PaymentMode { get; set; }

        public List<PaymentmodeList> PaymodeList { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter numbers only.")]
        public int? AmountOnCard { get; set; }

        public bool isFormValid { get; set; }
    }

    public class RequirementType
    {
        public int RequrementTypeID { get; set; }
        public string Description { get; set; }
    }

    public class EmployeeAcceptance
    {
        public string AcceptanceID { get; set; }
        public string ReceivedByEmployee { get; set; }
    }

    public class TCurrencyList
    {
        public int? CurrencyID { get; set; }
        public string CurrencyName { get; set; }
    }

    public class PaymentmodeList
    {
        public int? PaymentModeid { get; set; }
        public string PaymentmodeName { get; set; }
    }
}