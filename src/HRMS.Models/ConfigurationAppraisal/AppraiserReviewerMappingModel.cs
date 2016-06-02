using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AppraiserReviewerMappingModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Required]
        public int? DocumentID { get; set; }

        public int AppraisalYearID { get; set; }

        [Required(ErrorMessage = "File Name is Required")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "File Description is Required")]
        public string FileDescription { get; set; }

        [Required(ErrorMessage = "Please select UploadType")]
        public string UploadType { get; set; }

        public string FilePath { get; set; }

        //public string Comments { get; set; }

        public DateTime UploadedDate { get; set; }

        public string UploadedBy { get; set; }

        public bool isChecked { get; set; }

        public int UploadTypeId { get; set; }
    }
}