using System;
using System.Collections.Generic;

namespace HRMS.Models.SkillMatrix
{
    public class ConfigurationSkillMatrix
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        private List<ConfigureResourcePoolGridModel> resourcePoolModel { get; set; }
        public int? ToolId { get; set; }
        public int ResourcePoolId { get; set; }
        public string ResourcePoolName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SkillId { get; set; }
    }

    public class ConfigureResourcePoolGridModel
    {
        public int? ToolId { get; set; }
        public int ResourcePoolId { get; set; }
        public string ResourcePoolName { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int SkillId { get; set; }
    }
}