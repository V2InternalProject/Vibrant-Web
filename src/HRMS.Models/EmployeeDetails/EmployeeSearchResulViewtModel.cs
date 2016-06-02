using HRMS.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class EmployeeSearchResulViewtModel
    {
        public List<EmployeeDetails> EmployeeDetailsList { get; set; }

        [Display(ResourceType = typeof(EmployeeMessages), Name = "SearchEmployeeLabel")]
        public string SearchText { get; set; }

        public int PageNo { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public string EncryptedEmployeeId { get; set; }
        public int EmployeeId { get; set; }

        public string UserRole { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }
    }
}