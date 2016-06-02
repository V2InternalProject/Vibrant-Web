using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AppraisalYearModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Required(ErrorMessage = "Please Enter New Year.")]
        [RegularExpression(@"^([0-9]{4,4})*-([0-9]{4,4})$", ErrorMessage = "Please enter valid Year Format.")]
        [StringLength(100, ErrorMessage = "Appraisal Year can not be greater that 100 characters.")]
        public string NewAppraisalYear { get; set; }

        public List<AppraisalYear> AppraisalYearList { get; set; }

        [Display(Name = "Total Appraisal Year/s :")]
        public int TotalAppraisalYear { get; set; }

        public int AppraisalYearID { get; set; }

        public AppraisalRatingResponse RatingResponse { get; set; }
    }

    public class AppraisalProcessResponse
    {
        public bool isAdded { get; set; }
        public bool isExisted { get; set; }
        public List<int> failedEmployeeID { get; set; }
        public List<int> successEmployeeID { get; set; }
        public bool isDeleted { get; set; }
        public List<string> ParamterwithDesignation { get; set; }
        public bool InitiateIDF_LessThan_FreezePerformanceAppraisal { get; set; }
        public bool InitiateIDF_GreaterThan_FreezeIDF { get; set; }
        public bool isUnfreezed { get; set; }
        public bool isFreezed { get; set; }
    }

    public class AppraisalCategoriesModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Required(ErrorMessage = "Please Enter Category.")]
        [StringLength(50, ErrorMessage = "Appraisal Category can not be greater that 50 characters.")]
        public string NewAppraisalCategory { get; set; }

        [Required(ErrorMessage = "Please Enter Category Description.")]
        [StringLength(200, ErrorMessage = "Appraisal Category Description can not be greater that 200 characters.")]
        public string NewAppCategoryDescription { get; set; }

        public List<AppraisalCategories> AppraisalCategoryList { get; set; }

        [Display(Name = "Total Appraisal Category :")]
        public int TotalAppraisalCategory { get; set; }

        public int CategoryID { get; set; }

        public string ExistingAppraisalCategory { get; set; }
    }

    public class AppraisalCategories
    {
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public string CategoryDescription { get; set; }
    }

    public class AppraisalRatingResponse
    {
        public bool isRatingScalePresent { get; set; }
        public bool isRatingPresent { get; set; }
        public bool isRatingDescriptionPresent { get; set; }
        public bool isRatingAdded { get; set; }
    }
}