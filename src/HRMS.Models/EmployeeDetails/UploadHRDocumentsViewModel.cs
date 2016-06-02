using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HRMS.Models
{
    public class UploadHRDocumentsViewModel
    {
        [Required]
        public int DocumentID { get; set; }

        public int UploadTypeId { get; set; }

        [Required(ErrorMessage = "File Name is Required")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "File Description is Required")]
        public string FileDescription { get; set; }

        [Required(ErrorMessage = "Please select UploadType")]
        public string UploadType { get; set; }

        public string FilePath { get; set; }

        public string Comments { get; set; }

        public DateTime UploadedDate { get; set; }

        public string UploadedBy { get; set; }

        public List<SelectListItem> UploadTypeValues { get; set; }

        public bool isChecked { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }
    }

    public class HRDocumentDetailsViewModel
    {
        [Required]
        public int DocumentDetailId { get; set; }

        [Required]
        public int DocumentID { get; set; }

        public int UploadTypeId { get; set; }

        public string FileName { get; set; }

        public string FileDescription { get; set; }

        public string FilePath { get; set; }

        public string Comments { get; set; }

        public DateTime UploadedDate { get; set; }

        public string UploadedBy { get; set; }
    }

    public class HRDocumentUploadTypeViewModel
    {
        [Required]
        public int UploadTypeId { get; set; }

        public string UploadType { get; set; }

        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}