using HRMS.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class TravelDetailsViewModel
    {
        [Display(Name = "Valid Passport")]
        public bool IsValidPassport { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Passport number can not be more than 50 characters.")]
        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }

        [Required]
        [Display(Name = "Son of/Wife of/Daughter of")]
        [RegularExpression(@"^[a-zA-Z\s._]+$", ErrorMessage = "Special characters and Numbers are not allowed")]
        [StringLength(100, ErrorMessage = "Son of/Wife of/Daughter of cannot be greater than 100 characters.")]
        //[RegularExpression("^[a-zA-Z .]+$", ErrorMessage = "Son of/Wife of/Daughter of can not contain numbers or special characters.")]
        //[RegularExpression(@"^[a-zA-Z\\\-\[\]\""._^%$#!~@,&*()+={}|:;'<>/`? ]+$", ErrorMessage = "Son Of/Wife Of/Daugher Of cannot contain numbers.")]
        public string SonOfWifeOfDaugherOf { get; set; }

        [Required]
        [Display(Name = "Date Of Issue")]
        public DateTime? DateOfIssue { get; set; }

        [Required]
        [Display(Name = "Place Of Issue")]
        //[StringLength(50, ErrorMessage = "Place Of Issue cannot be greater than 50 characters.")]
        //[RegularExpression("^[a-zA-Z .]+$", ErrorMessage = "Place Of Issuse can not contain numbers or special characters.")]
        //[RegularExpression(@"^[a-zA-Z\\\-\[\]\""._^%$#!~@,&*()+={}|:;'<>/`? ]+$", ErrorMessage = "Place Of Issue cannot contain numbers.")]
        [RegularExpression(@"^[a-zA-Z\s._]+$", ErrorMessage = "Special characters and Numbers are not allowed")]
        public string PlaceOfIssue { get; set; }

        [Required]
        [Display(Name = "Date Of Expiry")]
        public DateTime? DateOfExpiry { get; set; }

        [Required]
        [Display(Name = "No. Of Pages Left")]
        [Range(0, 999)]
        [RegularExpression("^([0-9]+)$", ErrorMessage = "Enter Whole Number only.")]
        public int? NoOfPagesLeft { get; set; }

        [Required]
        [Display(Name = "Full Name As In Passport")]
        [RegularExpression(@"^[a-zA-Z\s._]+$", ErrorMessage = "Special characters and Numbers are not allowed")]
        [StringLength(100, ErrorMessage = "Full Name As In Passport cannot be greater than 100 characters.")]
        //[RegularExpression("^[a-zA-Z .]+$", ErrorMessage = "Full Name As In Passport can not contain numbers or special characters.")]
        //[RegularExpression(@"^[a-zA-Z\\\-\[\]\""._^%$#!~@,&*()+={}|:;'<>/`? ]+$", ErrorMessage = "FullName AsIn Passport cannot contain numbers.")]
        public string FullNameAsInPassport { get; set; }

        public List<VisaDetailsViewModel> VisaDetailsList { get; set; }

        public int EmployeeId { get; set; }

        public TravelDetailsPerson PersonType { get; set; }

        public EmployeeDependentTravelDetails DependantDetails { get; set; }

        public VisaDetailsViewModel VisaDetailsModel { get; set; }

        public bool IsSpouseDetailsAvailable { get; set; }

        public CheckSpouseDetails checkSpouseDetails { get; set; }

        public int DependantId { get; set; }

        public TravelDetailsViewModel()
        {
            PersonType = TravelDetailsPerson.Own;
            IsValidPassport = true;
            NoOfPagesLeft = 0;
        }

        public string UserRole { get; set; }

        public int EmpStatusMasterID { get; set; }

        public int DocumentID { get; set; }

        public string PassportFileName { get; set; }

        public string PassportFilePath { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }

    public class VisaDetailsViewModel
    {
        public int EmployeeVisaId { get; set; }

        public int EmployeeId { get; set; }

        [Display(Name = "Valid Visa")]
        public bool IsValidVisa { get; set; }

        public Dictionary<int, string> CountryList { get; set; }

        //Date of Expiry of Visa
        [Required]
        [Display(Name = "Valid Till")]
        public DateTime? ValidTill { get; set; }

        public bool IsVisaExpired { get; set; }

        [Required(ErrorMessage = "Visa type is required")]
        [Display(Name = "Visa Type")]
        public int VisaTypeID { get; set; }

        public string VisaTypeName { get; set; }

        public string Country { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [Display(Name = "Country")]
        public int SelectedCountryId { get; set; }

        public int DependantVisaDetailsId { get; set; }

        public int DependantId { get; set; }

        public TravelDetailsPerson PersonType { get; set; }

        public string UserRole { get; set; }

        public int EmpStatusMasterID { get; set; }

        public List<VisaTypeForEmployeeDetails> visatypeList { get; set; }

        public string VisaFileName { get; set; }

        public string VisaFilePath { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

    public class CheckSpouseDetails
    {
        public bool isPresent { get; set; }
        public bool isApproved { get; set; }
        public string spouseMaritalStatusName { get; set; }
        public int? approvalStatusId { get; set; }
    }

    public class VisaTypeForEmployeeDetails
    {
        public int VisaTypeID { get; set; }
        public string VisaTypeName { get; set; }
    }

    public class CheckPassportValid
    {
        public bool IsVisaExist { get; set; }

        public bool IsVisaValid { get; set; }
    }
}