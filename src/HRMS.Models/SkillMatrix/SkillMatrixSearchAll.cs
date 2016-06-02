using System.Collections.Generic;

namespace HRMS.Models.SkillMatrix
{
    public class SkillMatrixSearchAll
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public SkillMatrixShowHistoryModel ShowSkill { get; set; }
        public List<ResourcePoolSkillListDetails> ResourcePoolListDetails { get; set; }
        public List<SkillList> SkillListDetails { get; set; }

        //public List<SkillMatrix_Grid> getrecords { get; set; }
        public string dynamicTablesGeneration { get; set; }

        public string ResourcePoolName { get; set; }
        public string SkillName { get; set; }
        public bool UploadStatus { get; set; }
        public bool FullDoneStatus { get; set; }
        public string dropDownName { get; set; }
        public string SelectedId { get; set; }
        public SearchByEmployeeNameModel NewSearchEmp { get; set; }
        public Details deatailsModel { get; set; }
        public string Field { get; set; }
        public string FieldChild { get; set; }
        public List<ResourcePoolName> ResourceField { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }

    public class ResourcePoolSkillListDetails
    {
        public int ResourcePoolId { get; set; }
        public string ResourcePoolName { get; set; }
    }

    public class SkillList
    {
        public int SkillID { get; set; }
        public string SkillName { get; set; }
    }

    public class JqGridData
    {
        public int page { get; set; }
        public int total { get; set; }
        public int records { get; set; }
        public int ResourcePoolId { get; set; }
        public string ResourcePoolName { get; set; }
        public List<object> rowsM { get; set; }
        public string rows { get; set; }
        public List<string> rowsHead { get; set; }
    }

    public class JqGridDataHeading
    {
        public string name { get; set; }
        public string index { get; set; }
    }

    public class SkillSaveAllModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; } //Need To remove
        public bool UploadStatus { get; set; }
        public bool FullDoneStatus { get; set; }
        public string dropDownName { get; set; }
        public string SelectedId { get; set; }
    }
}