using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ManageDocumentsModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public DocumentFileDetails DocumentFileDetailsModel { get; set; }

        public List<DocumentCategoryDetails> DocumentCategoryDetailsList { get; set; }

        public List<DocumentSubCategoryDetails> DocumentSubCategoryDetailsList { get; set; }

        public List<ProjectDetails> ProjectDetailsList { get; set; }

        public int CategoryId { get; set; }

        public string ProjectId { get; set; }

        public int Count { get; set; }
        public string searchtext { get; set; }

        public int? DocumentCategoryId { get; set; }
    }

    public class ProjectDetails
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }
    }

    public class DocumentCategoryDetails
    {
        public int DocumentCategoryId { get; set; }

        public string DocumentCategory { get; set; }
    }

    public class DocumentSubCategoryDetails
    {
        public int DocumentSubCategoryId { get; set; }

        public string DocumentSubCategory { get; set; }
    }

    public class DocumentFileDetails
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<DocumentCategoryDetails> DocumentCategoryDetailsList { get; set; }

        public List<DocumentSubCategoryDetails> DocumentSubCategoryDetailsList { get; set; }

        public int ProjectId { get; set; }
        public int DocumentAttachmentID { get; set; }
        public float FileSize { get; set; }
        public string DocName { get; set; }

        [Required(ErrorMessage = "Please select Document Category")]
        public int? CategoryId { get; set; }

        public int? SubCategoryId { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public string DocPath { get; set; }
        public string Details { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedOn { get; set; }
        public string EmployeeName { get; set; }
        public string LoggedInUser { get; set; }
    }

    public class DirectoryName
    {
        public string DirectoryNames { get; set; }

        public string FileName { get; set; }
    }
}