using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    /// <summary>
    /// Class will represent the fields to map skill details
    /// </summary>
    public class SkillDetails
    {
        public int EmployeeID { get; set; }

        public int EmployeeSkillID { get; set; }

        [Required]
        public string Skill { get; set; }

        public int? SkillId { get; set; }

        public string SkillLevel { get; set; }

        public string SkillLevelText { get; set; }

        public string Status { get; set; }

        public string ApprovalStatusFlag { get; set; }

        public int? ApproveStatus { get; set; }

        public string ActionType { get; set; }

        public string Comments { get; set; }

        public string UserRole { get; set; }

        public string HrComment { get; set; }

        public string Value { get; set; }

        public int EmployeeSkillHistoryID { get; set; }

        public string Module { get; set; }

        public int? ApprovalStatusMasterID { get; set; }

        // to create DDL of skills
        public List<SkillDetailsList> SkillsDDL { get; set; }

        public List<SkillLevelList> SkillsLevelDDL { get; set; }

        public string CanSendMail { get; set; }
    }

    public class SkillDetailsList
    {
        public string Value { get; set; }

        public string Text { get; set; }
    }

    public class SkillLevelList
    {
        public int Value { get; set; }

        public string Text { get; set; }
    }
}