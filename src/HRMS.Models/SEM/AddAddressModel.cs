using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AddCustomerAddress
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int CutomerIds { get; set; }

        public int CustID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Abbreviated Name can not be greater than 50 characters.")]
        public string Address { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Abbreviated Name can not be greater than 50 characters.")]
        public string Country { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Abbreviated Name can not be greater than 50 characters.")]
        public string State { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Abbreviated Name can not be greater than 50 characters.")]
        public string City { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Abbreviated Name can not be greater than 50 characters.")]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Abbreviated Name can not be greater than 50 characters.")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Abbreviated Name can not be greater than 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_.]*@[a-zA-Z0-9_.]*\.([a-zA-Z]{2,4})$", ErrorMessage = "Please enter valid email id.")]
        public string EmailId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Abbreviated Name can not be greater than 50 characters.")]
        public string Details { get; set; }

        public bool? SameAddess { get; set; }
    }
}