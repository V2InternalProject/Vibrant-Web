using System;
using System.Collections.Generic;

namespace HRMS.Models.SkillMatrix
{
    public class Details
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public int? ID { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeCode { get; set; }
        public string ResourcePoolName { get; set; }
        public int? ResourcePoolID { get; set; }
        public int? SkillID { get; set; }
        public string Description { get; set; }
        public string Rating { get; set; }
        public int? ProjectID { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int EmployeeId { get; set; }
        private List<Ratings> RatingList = new List<Ratings>();
    }

    public class Ratings
    {
        public int ProficiencyID { get; set; }
        public string Rating { get; set; }
    }
}