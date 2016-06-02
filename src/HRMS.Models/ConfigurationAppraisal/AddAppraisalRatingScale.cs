using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AddAppraisalRatingScale
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Required]
        [Display(Name = "Rating Scale")]
        public decimal? Percentage { get; set; }

        public decimal? SelectedPercentage { get; set; }

        [Required]
        [Display(Name = "Rating")]
        public string Rating { get; set; }

        public int RatingID { get; set; }

        [Display(Name = "Description")]
        [StringLength(1000, ErrorMessage = "Description cannot be greater than 1000 characters.")]
        public string Description { get; set; }

        [Display(Name = "Adjustment Factor")]
        [RegularExpression("^[0-9]([.,][0-9]{2,3})?$", ErrorMessage = "Enter only numeric values.")]
        public double? AdjustmentFactor { get; set; }

        [Display(Name = "Set as Minimum Limit [For Appraisal Process]")]

        //public bool SetAsMinimumLimit { get; set; }

        public int AppraisalYearID { get; set; }

        public bool IsAddnew { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}