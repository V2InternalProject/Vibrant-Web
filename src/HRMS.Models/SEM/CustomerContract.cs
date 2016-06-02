using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class CustomerContract
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public int ContractID { get; set; }

        [Required]
        [StringLength(3500, ErrorMessage = "Contract Summary can not be greater that 3500 characters.")]
        public string ContractSummary { get; set; }

        [Required]
        [StringLength(3500, ErrorMessage = "Contract Details can not be greater that 3500 characters.")]
        public string ContractDetails { get; set; }

        [Required]
        public DateTime? CommencementDate { get; set; }

        [Required(ErrorMessage = "The ContractEffectiveDate field is required.")]
        public DateTime? ContractSigningDate { get; set; }

        [Required]
        public DateTime? ContractValidityDate { get; set; }

        public DateTime? CustomerContractSigningDate { get; set; }

        public DateTime? CustomerContractValidityDate { get; set; }

        public int? CustomerID { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public int? ContractType { get; set; }

        public string ContractTypeName { get; set; }

        public List<ContractTypes> ContractTypeList { get; set; }

        public ContractFileDetails ContractFileDetailsModel { get; set; }

        public string UserName { get; set; }
    }

    public class ContractTypes
    {
        public int ContractTypeID { get; set; }
        public string ContractTypeName { get; set; }
    }

    public class ContractFileDetails
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public int ContractID { get; set; }
        public int ContractAttachmentID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileUpload { get; set; }

        [StringLength(500, ErrorMessage = "Description can not be more than 500 characters.")]
        public string Description { get; set; }

        public string AttachedBy { get; set; }
        public DateTime? AttachedDate { get; set; }
        public string EmployeeName { get; set; }
        public bool? IsFileExists { get; set; }
    }
}