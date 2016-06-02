using System.Collections.Generic;

namespace HRMS.Models
{
    public class ViewYearModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<AppraisalPastYears> PastYearsList { get; set; }
        public int CurrentYearID { get; set; }
        public string CurrentYearName { get; set; }
    }

    public class AppraisalPastYears
    {
        public int PastYearID { get; set; }
        public string PastYearName { get; set; }
    }
}