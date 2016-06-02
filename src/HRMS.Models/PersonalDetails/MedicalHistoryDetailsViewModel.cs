using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class MedicalHistoryDetailsViewModel
    {
        public int? EmployeeId { get; set; }

        public string UserRole { get; set; }

        public List<MedicalHistoryDetails> MedicalHistoryList { get; set; }

        public MedicalHistoryDetails MedicalHistory { get; set; }

        [Required]
        [Display(Name = "Blood Group")]
        public int SelectedBloodGroup { get; set; }

        public IList<BloodGroupModel> BloodGroupList { get; set; }

        public YearListClass1 YearListClass { get; set; }

        public List<YearListClass1> YearList { get; set; }                                  // Added Year Drop Down List

        public int EmpStatusMasterID { get; set; }

        public int birthDate { get; set; }

        [Display(Name = "Medical Description Id")]
        public int MedicalDescId { get; set; }

        [Required]
        [Display(Name = "Medical Description")]
        [StringLength(500, ErrorMessage = "Medical Description can not be more than 500 characters.")]
        public string MedicalDescription { get; set; }

        [Required]
        //[Range(typeof(string), "1900", "2099")]
        public string Year { get; set; }
    }

    public class YearListClass1
    {
        public int YearID { get; set; }
        public int Year { get; set; }
    }
}