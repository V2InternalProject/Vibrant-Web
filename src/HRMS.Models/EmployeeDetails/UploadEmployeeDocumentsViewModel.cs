using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HRMS.Models
{
    public class UploadEmployeeDocumentsViewModel
    {
        [Required]
        [Display(Name = "Document Id")]
        public int DocumentID { get; set; }

        [Required(ErrorMessage = "Please select Upload Type.")]
        [Display(Name = "Upload Type Id")]
        public int UploadTypeId { get; set; }

        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "File Name is Required")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "File Description is Required")]
        [StringLength(25, ErrorMessage = "File Description can not be greater than 25 characters.")]
        public string FileDescription { get; set; }

        [Required(ErrorMessage = "Please select UploadType")]
        public string UploadType { get; set; }

        public string FilePath { get; set; }

        [StringLength(200, ErrorMessage = "Comment can not be greater than 200 characters.")]
        public string Comments { get; set; }

        [Display(Name = "Uploaded Date")]
        public DateTime UploadedDate { get; set; }

        [Display(Name = "Uploaded By")]
        public string UploadedBy { get; set; }

        public List<SelectListItem> UploadTypeValues { get; set; }

        public bool isChecked { get; set; }

        public string UserRole { get; set; }

        public int EmpStatusMasterID { get; set; }
    }

    public class EmployeeDocumentDetailsViewModel
    {
        [Required]
        [Display(Name = "Document Detail Id")]
        public int DocumentDetailId { get; set; }

        [Required]
        [Display(Name = "Document Id")]
        public int DocumentID { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        public string FileDescription { get; set; }

        public string FilePath { get; set; }

        public string Comments { get; set; }

        [Display(Name = "Uploaded Date")]
        public DateTime UploadedDate { get; set; }

        [Display(Name = "Uploaded By")]
        public string UploadedBy { get; set; }
    }

    public class EmployeeDocumentUploadTypeViewModel
    {
        [Required]
        [Display(Name = "Employee Id")]
        public int UploadTypeId { get; set; }

        [Display(Name = "Upload Type")]
        public string UploadType { get; set; }

        public string Description { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }
    }
}