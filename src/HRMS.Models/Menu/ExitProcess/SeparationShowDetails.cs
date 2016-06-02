using System.Collections.Generic;

namespace HRMS.Models
{
    public class SeparationShowDetails
    {
        public ExitProcessViewModel SeparationFormDetails { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }

        public EmployeeDetailsViewModel EmployeeDetails { get; set; }

        public DesignationDetails EmpdesignationDetails { get; set; }

        public Role OrgRoleDetails { get; set; }

        public int? EmployeeId { get; set; }

        public int EmployeeCode { get; set; }

        public List<WaiveOff> WaiveOffList { get; set; }

        public string WaiveOff { get; set; }

        public string GridClick { get; set; }
    }

    public class WaiveOff
    {
        public int WaiveId { get; set; }

        public string Description { get; set; }
    }
}