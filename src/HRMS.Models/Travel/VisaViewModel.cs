using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class VisaViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Required]
        public int? ID { get; set; }

        public int VisaTravelID { get; set; }

        [Required]
        [Display(Name = "Visa Type")]
        public int VisaTypeID { get; set; }

        [Required]
        [Display(Name = "Country Name")]
        public int CountryID { get; set; }

        [Required]
        [Display(Name = "From Date")]
        public DateTime? FromDate { get; set; }

        [Required]
        [Display(Name = "To Date")]
        public DateTime? ToDate { get; set; }

        [StringLength(500, ErrorMessage = "Maxium 500 characters are allowed")]
        public string Decription { get; set; }

        [StringLength(500, ErrorMessage = "Maxium 500 characters are allowed")]
        public string AdditionalInfo { get; set; }

        public string CountryName { get; set; }

        public string VisaType { get; set; }

        public List<VisaType> visatypeList { get; set; }
        public List<Country> countryList { get; set; }

        public List<VisaViewModel> viewmodellist { get; set; }

        public List<visa> VisaDetail { get; set; }

        public int? EmployeeId { get; set; }

        public int EmployeeVisaID { get; set; }

        public DateTime? ValidTill { get; set; }

        public int SelectedCountryId { get; set; }

        public int? VisaAddedStatus { get; set; }

        public string VisaTypeName { get; set; }

        public bool IsVisaExpired { get; set; }

        public string VisaFileName { get; set; }

        public string VisaFilePath { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? StageID { get; set; }

        public string isAdminRecord { get; set; }

        public string VisaFileNameUpload { get; set; }

        public int userSelectedCountryId { get; set; }
        public int selectedVisaTypeId { get; set; }
    }

    public class VisaType
    {
        public int VisaTypeID { get; set; }
        public string VisaTypeName { get; set; }
    }

    public class Country
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }

    public class visa
    {
        public int ContryId { get; set; }

        public int EmployeeId { get; set; }
    }
}