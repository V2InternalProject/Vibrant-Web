using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AddRatingScale
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Required]
        [Display(Name = "Rating Scale")]
        public decimal? Percentage { get; set; }

        [Required]
        [Display(Name = "Rating")]
        public string Rating { get; set; }

        public int RatingID { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Adjustment Factor")]
        [RegularExpression("^[0-9]([.,][0-9]{2,3})?$", ErrorMessage = "Enter only numeric values.")]
        public double? AdjustmentFactor { get; set; }

        [Display(Name = "Set as Minimum Limit [For Confirmation Process]")]
        public bool SetAsMinimumLimit { get; set; }

        public bool IsAddnew { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //public List<ModifiedvalueList> modifiedValues { get; set; }
    }

    //public class ModifiedvalueList
    // {
    //    public int RatingID { get; set; }
    // }
}