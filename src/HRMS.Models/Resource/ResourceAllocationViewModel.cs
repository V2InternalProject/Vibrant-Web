using System.Collections.Generic;

namespace HRMS.Models
{
    public class ResourceAllocationViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public string EmployeeCode { get; set; }

        public int Employeeid { get; set; }

        public string EmployeeName { get; set; }

        public List<ResourceAllocationDetailsList> ResourceAllocationDetailsList { get; set; }
    }

    public class ResourceAllocationDetailsList
    {
        public string EmployeeCode { get; set; }

        public int Employeeid { get; set; }

        public string EmployeeName { get; set; }
    }
}