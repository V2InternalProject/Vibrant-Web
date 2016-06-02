using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class CustomerContact
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int CustomerIds { get; set; }

        public int? CustID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Abbreviated Name can not be greater than 50 characters.")]
        public string ContactPerson { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Customer Name can not be greater than 50 characters.")]
        public string TypeofContact { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.]*@[a-zA-Z0-9_.]*\.([a-zA-Z]{2,4})$", ErrorMessage = "Please enter valid email id.")]
        [StringLength(200, ErrorMessage = "Email ID can not be greater that 200 characters.")]
        public string EmailID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Customer Name can not be greater than 50 characters.")]
        public string MobileNumber { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Customer Name can not be greater than 50 characters.")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Customer Name can not be greater than 50 characters.")]
        public string FaxNumber { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Customer Name can not be greater than 50 characters.")]
        public string OnlineContact { get; set; }

        public bool SameAddess { get; set; }
    }
}