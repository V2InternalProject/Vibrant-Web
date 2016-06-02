using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AppraisalStrengthImproveModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Required]
        public int? StrengthLimit { get; set; }

        public int? ImprovementLimit { get; set; }

        public int AppraisalYearID { get; set; }
    }
}