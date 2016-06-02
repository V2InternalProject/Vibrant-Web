using System;
using System.Collections.Generic;

namespace HRMS.Models.SkillMatrix
{
    public class skillmatrix_history
    {
        public int? ToolId { get; set; }
        public int? ResourcePoolID { get; set; }
        public List<ResourcePoolList> resourcepoolListdata { get; set; }
        public List<GetSkillName> getskilllist { get; set; }
        public string ROWS { get; set; }
        public string EmployeeId { get; set; }
        public string Rank { get; set; }
        public string Resourcepoolname { get; set; }
        public string Description { get; set; }
        public string Ratings { get; set; }
        public string Remark { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public int? EmployeeIdInt { get; set; }
        public string ResourcePoolName { get; set; }
        //public description

        //[description] varchar(200),
        //[ratings] nvarchar(200),
        //[remark] nvarchar(200),
        //[UpdatedBy] nvarchar(200),
        //[UpdatedOn] DateTime
    }

    public class SkillMatrixShowHistoryModel
    {
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public List<ResourcePoolList> resourcepoolListdata { get; set; }
        public List<GetSkillName> getskilllist { get; set; }
        public int? ResourcePoolID { get; set; }
        public string ResourcePoolName { get; set; }
        public int? ProjecyID { get; set; }
        public int? ToolId { get; set; }
        public int? toolId { get; set; }
        public string Description { get; set; }
        public string Ratings { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    public class ResourcePoolList
    {
        public int ResourcePoolId { get; set; }
        public string ResourcePoolName { get; set; }
    }

    public class GetSkillName
    {
        public int toolId { get; set; }
        public string Description { get; set; }
        public int ResourcePoolId { get; set; }
    }
}