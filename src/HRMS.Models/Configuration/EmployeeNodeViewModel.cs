using System.Collections.Generic;

namespace HRMS.Models
{
    public class EmployeeNodeViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<EmployeeNodeMapping> EmployeeNodeMappingList { get; set; }
        public List<Nodes> NodesList { get; set; }
        public List<Employees> EmployeeList { get; set; }
    }

    //public class Nodes
    //{
    //    public int NodeID { get; set; }
    //    public string NodeName { get; set; }
    //}
    public class Employees
    {
        public int EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
    }

    public class EmployeeNodeMapping
    {
        public int EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }
    }
}