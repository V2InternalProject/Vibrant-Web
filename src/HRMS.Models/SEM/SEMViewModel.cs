using System;

namespace HRMS.Models
{
    public class SEMViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public DateTime? ContractSigningDate { get; set; }

        public DateTime? ContractValidityDate { get; set; }

        public string Region { get; set; }
    }
}