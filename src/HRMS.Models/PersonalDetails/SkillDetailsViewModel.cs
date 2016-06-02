using HRMS.Models.SkillMatrix;
using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    /// <summary>
    ///
    /// </summary>
    public class SkillDetailsViewModel
    {
        public List<SkillDetails> EmployeeSkillDetails { get; set; }
        public SkillDetails NewSkillDetail { get; set; }
        public string UserRole { get; set; }
        public int EmpStatusMasterID { get; set; }
        public string Description { get; set; }
        public int ToolId { get; set; }
        public int ResourcePoolId { get; set; }
        public string ResourcePoolName { get; set; }
        public string EmployeeId { get; set; }
        public int SkillId { get; set; }
        public string Rating { get; set; }
        public int? ID { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public int SearchUserID { get; set; }

        //Testing New Code

        public SearchedUserDetails SearchedUserDetails { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string Designation { get; set; }
        public string ResourcePoolNames { get; set; }   // Used in Skill Management screen
        public string LoggedInUserRole { get; set; }
        public string CompetencyManagerName_Emp { get; set; }
        public string ConfirmationManager { get; set; }
        public string ReportingTo { get; set; }

        //For DevelopmentPlan
        public string ExpectedRating { get; set; }

        public DateTime? TargetDate { get; set; }
        public List<SkillMatrixShowHistoryModel> DevelopmentPlan { get; set; }
        private List<Ratings> RatingList = new List<Ratings>();
    }
}