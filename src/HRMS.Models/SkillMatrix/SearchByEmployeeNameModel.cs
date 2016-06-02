using System.Collections.Generic;

namespace HRMS.Models.SkillMatrix
{
    public class SearchByEmployeeNameModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public SkillMatrixShowHistoryModel ShowSkill { get; set; }
        public string Field { get; set; }
        public string FieldChild { get; set; }
        public List<ResourcePoolName> ResourceField { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }

    public class ResourcePoolName
    {
        public int ResourcePoolId { get; set; }
        public string Description { get; set; }
    }
}