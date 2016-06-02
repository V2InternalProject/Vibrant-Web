using System.Collections.Generic;

namespace HRMS.Models
{
    public class StageApproversVM
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<StageApprover> stageApprover { get; set; }
        public bool isChecked { get; set; }
    }

    public class StageApprover
    {
        public string EmpName { get; set; }

        public string BusinessGroup { get; set; }

        public string OrgUnit { get; set; }

        public string Roles { get; set; }

        public string Designation { get; set; }

        public string Department { get; set; }

        public string EmpStatus { get; set; }

        public string Location { get; set; }

        public string TotalExp { get; set; }

        public string ExpUs { get; set; }
    }
}