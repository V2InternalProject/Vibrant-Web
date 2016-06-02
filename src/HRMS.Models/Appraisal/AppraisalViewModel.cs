using System;

namespace HRMS.Models
{
    public class AppraisalViewModel
    {
        public int EmployeeId { get; set; }

        //[Required]
        //public string Quality1Comments { get; set; }

        //[Required]
        //[Range(1, 5, ErrorMessage = "Ratings should be in between 1 and 5")]
        //public int Quality1Rating { get; set; }

        //[Required]
        //public string Quality2Comments { get; set; }

        //[Required]
        //[Range(1, 5, ErrorMessage = "Ratings should be in between 1 and 5")]
        //public int Quality2Rating { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }

        public DateTime ReviewFromDate { get; set; }

        public DateTime ReviewToDate { get; set; }

        public int EmployeeCode { get; set; }

        public string EmployeeName { get; set; }

        public string DeliveryUnit { get; set; }

        public string Location { get; set; }

        public DateTime DateOfJoining { get; set; }

        public string Appraisal1 { get; set; }

        public string Appraisal2 { get; set; }

        public string Reviewer { get; set; }

        // Project Assignment

        public DateTime ProjectFromDate { get; set; }

        public DateTime ProjectToDate { get; set; }

        public string ProjectDescription { get; set; }

        public string ProjectAchievment { get; set; }

        public int SatisfactionId { get; set; }

        public int ProjectManagerId { get; set; }
    }
}