using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HRMS.Models
{
    public class PersonalDetailsViewModel
    {
        public int? EmployeeId { get; set; }

        public int? SearchUserID { get; set; }

        [Required]
        public string Prefix { get; set; }

        [Required]
        [Remote("IsEmployeeCodeExist", "PersonalDetails", ErrorMessage = "Employee code already exist!")]
        [RegularExpression("^(0|[1-9][0-9]*)$", ErrorMessage = "Employee code can contain numbers only.")]
        public string EmployeeCode { get; set; }

        public string NewEmployeeCode { get; set; }

        public string UserRole { get; set; }

        public string ProfileImageName { get; set; }

        public string ProfileImagePath { get; set; }

        public string lblMaritalStatus { get; set; }

        public string lblWeddingDate { get; set; }

        public string lblSpouseName { get; set; }

        public string lblSpouseBirthDate { get; set; }

        public string lblNoOfchildren { get; set; }

        public string lblChild1Name { get; set; }

        public string lblChild1BirthDate { get; set; }

        public string lblChild2Name { get; set; }

        public string lblChild2BirthDate { get; set; }

        public string lblChild3Name { get; set; }

        public string lblChild3BirthDate { get; set; }

        public string lblChild4Name { get; set; }

        public string lblChild4BirthDate { get; set; }

        public string lblChild5Name { get; set; }

        public string lblChild5BirthDate { get; set; }

        public string lblUserName { get; set; }

        public string lblMaidanName { get; set; }

        public string lblSalutation { get; set; }

        public string lblFirstName { get; set; }

        public string lblLastName { get; set; }

        public string lblMiddleName { get; set; }

        [Required]
        // [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only and only single space in between two words please")]
        [StringLength(50, ErrorMessage = "First Name can not be greater that 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        // [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only and only single space in between two words please")]
        [StringLength(50, ErrorMessage = "Last Name can not be greater that 50 characters.")]
        public string LastName { get; set; }

        //  [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        [StringLength(50, ErrorMessage = "Middle Name can not be greater that 50 characters.")]
        public string MiddleName { get; set; }

        //public string EmployeeName { get; set; }

        [Required]
        public string Gender { get; set; }

        //  [StatusCheckAge]
        [Required]
        // [DataType(DataType.Date)]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Required]
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        /// <summary>
        /// Maidan Name
        /// </summary>
        [StringLength(50, ErrorMessage = "Maxium 50 characters are allowed")]
        public string MaidanName { get; set; }

        // [Required]
        // [RequiredIf("Select", true)]
        // [Required]
        // [#if equals ("Select")
        // [ValidateInput if()
        public string ReportingToName { get; set; }

        [Required]
        public int ReportingToId { get; set; }

        // [Required]
        public string ExitConfirmationManagerName { get; set; }

        [Required]
        public int ExitConfirmationManagerId { get; set; }

        public string CompetencyManagerName { get; set; }

        [Required]
        public int CompetencyManagerId { get; set; }

        [Display(Name = "Contract Employee")]
        public bool ContractEmployee { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ContractFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ContractTo { get; set; }

        public int ShiftId { get; set; }

        public string ShiftName { get; set; }

        public string ReportingTimeHrs { get; set; }

        public string ReportingTimeMins { get; set; }

        public class DDLHelper
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }

        public List<SelectListItem> GetGender()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Female", Value = "Female" },
                new SelectListItem { Selected = true, Text = "Male", Value = "Male" },
                new SelectListItem { Selected = true, Text = "Other", Value = "Other" }
            };
            return list;
        }

        public List<SelectListItem> GetSalutation()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Mr.", Value = "Mr." },
                new SelectListItem { Selected = true, Text = "Mrs.", Value = "Mrs." },
                new SelectListItem { Selected = true, Text = "Ms.", Value = "Ms." }
            };
            return list;
        }

        public List<SelectListItem> GetMarritalStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Divorced", Value = "Divorced" },
                new SelectListItem { Selected = true, Text = "Married", Value = "Married" },
                new SelectListItem { Selected = true, Text = "Single", Value = "Single" },
                new SelectListItem { Selected = true, Text = "Widowed", Value = "Widowed" }
            };
            return list;
        }

        //  [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        //  [Required]
        //  [DateGreaterThanAttribute("BirthDate")]
        public DateTime? WeddingDate { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Maxium 500 characters are allowed")]
        //[RegularExpression(@"^[a-zA-Z\s+,&;]+$", ErrorMessage = "Special characters and Numbers are not allowed")]
        public string Hobbies { get; set; }

        [StringLength(500, ErrorMessage = "Maxium 500 characters are allowed")]
        //[RegularExpression(@"^[a-zA-Z\s+,&;]+$", ErrorMessage = "Special characters and Numbers are not allowed")]
        public string Achievement { get; set; }

        public string Age { get; set; }

        [StringLength(400, ErrorMessage = "Maxium 400 characters are allowed")]
        //[RegularExpression(@"^[a-zA-Z\s+,&;]+$", ErrorMessage = "Special characters and Numbers are not allowed")]
        public string Recognition { get; set; }

        [Required]
        [Display(Name = "No. Of children")]
        [Range(00, 10)]
        public int? NoOfchildren { get; set; }

        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        //[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string SpouseName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SpouseBirthDate { get; set; }

        //[Display(Name = "Child 1 Name")]
        // [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string Child1Name { get; set; }

        //[Display(Name = "Child 2 Name")]
        //[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string Child2Name { get; set; }

        //[Display(Name = "Child 3 Name")]
        //[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string Child3Name { get; set; }

        //[Display(Name = "Child 4 Name")]
        //[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string Child4Name { get; set; }

        //[Display(Name = "Child 5 Name")]
        // [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string Child5Name { get; set; }

        // [Display(Name = "Child 1 BirthDate")]
        [DataType(DataType.Date)]
        public DateTime? Child1BirthDate { get; set; }

        //  [Display(Name = "Child 2 BirthDate")]
        [DataType(DataType.Date)]
        public DateTime? Child2BirthDate { get; set; }

        //  [Display(Name = "Child 3 BirthDate")]
        [DataType(DataType.Date)]
        public DateTime? Child3BirthDate { get; set; }

        //  [Display(Name = "Child 4 BirthDate")]
        [DataType(DataType.Date)]
        public DateTime? Child4BirthDate { get; set; }

        //  [Display(Name = "Child 5 BirthDate")]
        [DataType(DataType.Date)]
        public DateTime? Child5BirthDate { get; set; }

        //[Required]
        public DateTime? AgreementDate { get; set; }

        public bool IsAllowEditing { get; set; }

        public bool IsAllowViewing { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Required]
        [Remote("IsEmployeeUserNameExist", "PersonalDetails", ErrorMessage = "User name already exist.Please select another user name.")]
        [RegularExpression(@"^[a-zA-Z\s._]+$", ErrorMessage = "Special characters and Numbers are not allowed")]
        [StringLength(500, ErrorMessage = "User Name can not be greater that 500 characters.")]
        public string UserName { get; set; }

        [StringLength(2000, ErrorMessage = "Maxium 2000 characters are allowed")]
        //[RegularExpression(@"^[a-zA-Z\s+,&;]+$", ErrorMessage = "Special characters and Numbers are not allowed")]
        public string Remarks { get; set; }

        //[Display(Name = "Designation Details")]
        public List<RepotingToList> ReportingToList { get; set; }

        public int EmpStatusMasterID { get; set; }

        public List<ContractPermanentDetails> ContractPermanentList { get; set; }

        public EmployeeMailTemplate Mail { get; set; }
    }

    public class EmployeeCodeList
    {
        public string EmployeeCode { get; set; }
    }

    public class RepotingToList
    {
        public int? EmployeeId { get; set; }

        public string EmployeeName { get; set; }
    }

    public class ShiftList
    {
        public int? ShiftId { get; set; }

        public string ShiftName { get; set; }
    }

    public class ContractPermanentDetails
    {
        public string OldEmployeecode { get; set; }

        public string EmployeeType { get; set; }

        public string EmployeeCodeStatus { get; set; }

        public string NewEmployeecode { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

    public class UploadModel
    {
        public string ModuleName { get; set; }
        public string FormName { get; set; }
        public string FileNameProp { get; set; }
        public string FilePathProp { get; set; }
    }
}