using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class ManageModulesModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<ProjectAppList> ProjectList { get; set; }
        public int ProjectID { get; set; }
        public List<ModuleComplexityList> ComplexityLists { get; set; }
        public int ComplexityID { get; set; }
        public string UserName { get; set; }
        public string SelectedModuleName { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
    }

    public class AddManageModules
    {
        public int ModuleID { get; set; }
        public int? ProjectID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public DateTime? ModuleStartDate { get; set; }
        public DateTime? ModuleEndDate { get; set; }
        public string Complexity { get; set; }
        public int? HiddenComplexityID { get; set; }
        public double? WorkHours { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
    }

    public class ModuleComplexityList
    {
        public int ComplexityID { get; set; }
        public string ComplexityName { get; set; }
    }

    public class SEMResponse
    {
        public bool status { get; set; }
        public bool isModuleNameExist { get; set; }
        public int nextContractID { get; set; }
        public bool isTagNameExist { get; set; }
    }
}