using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ConfigureYearModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Required]
        public string Year { get; set; }

        public List<AppraisalYear> AppraisalYearList { get; set; }
        public int AppraisalYearStatus { get; set; }
    }

    public class AppraisalYear
    {
        public int AppraisalYearID { get; set; }
        public string AppraisalYearName { get; set; }
    }
}