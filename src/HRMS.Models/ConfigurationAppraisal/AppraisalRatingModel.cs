using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AppraisalRatingModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<AppraisalRatingScales> AppraisalRatingScale { get; set; }

        [Display(Name = "Total Records : ")]
        public int RecordsCount { get; set; }

        public int AppraisalYearID { get; set; }
    }

    public class AppraisalRatingScales
    {
        public int RatingID { get; set; }

        public string Rating { get; set; }

        public string Description { get; set; }

        public decimal? Percentage { get; set; }

        public int AppraisalYearID { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public double? AdjustmentFactor { get; set; }

        //public bool? SetAsMinimumLimit { get; set; }

        public bool Checked { get; set; }
    }
}