using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ConfigureRatingScales
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<RatingScales> RatingScale { get; set; }

        [Display(Name = "Total Records : ")]
        public int RecordsCount { get; set; }
    }

    public class RatingScales
    {
        public int RatingID { get; set; }

        public string Rating { get; set; }

        public string Description { get; set; }

        public decimal? Percentage { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public double? AdjustmentFactor { get; set; }

        public bool? SetAsMinimumLimit { get; set; }

        public bool Checked { get; set; }
    }
}