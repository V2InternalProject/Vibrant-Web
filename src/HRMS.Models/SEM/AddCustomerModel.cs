using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AddCustomerModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public CustomerAddress CustomerAddress { get; set; }
        public CustomerContact CustomerContact { get; set; }
        public CustomerContract CustomerContract { get; set; }

        public AddContract AddContract { get; set; }
        public TravelMailTemplate Mail { get; set; }

        public int CutomerIds { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Abbreviated Name can not be greater than 50 characters.")]
        public string AbbreviatedName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Customer Name can not be greater than 50 characters.")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "The ContractEffectiveDate field is required.")]
        public DateTime? ContractSigningDate { get; set; }

        [Required]
        public DateTime? ContractValidityDate { get; set; }

        public List<CountryDetailsListSEM> CurrentCountryListSEM { get; set; }

        public int? CountryIds { get; set; }

        [Required]
        public string Countrynames { get; set; }

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter number only")]
        //[StringLength(50, ErrorMessage = "Credit Period can not be greater than 50 characters.")]
        public int? CreditPeriod { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Address Name can not be greater than 150 characters.")]
        public string Address { get; set; }

        public string Country { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Phone Number can not be greater than 50 characters.")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "State Name can not be greater than 50 characters.")]
        public string State { get; set; }

        [StringLength(50, ErrorMessage = "Alternate Phone Number can not be greater than 50 characters.")]
        public string AlternatePhoneNumber { get; set; }

        [StringLength(50, ErrorMessage = "Fax Number can not be greater than 50 characters.")]
        public string FaxNumber { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "City Name can not be greater than 50 characters.")]
        public string City { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.]*@[a-zA-Z0-9_.]*\.([a-zA-Z]{2,4})$", ErrorMessage = "Please enter valid email id.")]
        [StringLength(50, ErrorMessage = "Email ID can not be greater that 50 characters.")]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Zip Code can not be greater than 50 characters.")]
        public string ZipCode { get; set; }

        public List<Region> RegionTypeList { get; set; }

        public List<ExternalMarketSegmentation> ExtMaktSegList { get; set; }

        [Required]
        public int? ExtMaktSegName { get; set; }

        public int? RegionName { get; set; }
    }

    public class Region
    {
        public int RegionID { get; set; }
        public string RegionNames { get; set; }
    }

    public class ExternalMarketSegmentation
    {
        public int ExtMaktSegID { get; set; }
        public string ExtMaktSeg { get; set; }
    }

    public class CountryDetailsListSEM
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CC_FIPS { get; set; }
    }

    public class TypeOfContactListSEM
    {
        public int ContactTypeID { get; set; }
        public string ContactTypeName { get; set; }
    }
}