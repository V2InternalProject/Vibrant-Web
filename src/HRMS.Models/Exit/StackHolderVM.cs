using System.Collections.Generic;

namespace HRMS.Models
{
    public class StackHolderVM
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<StackHolderList> stackHolder { get; set; }
        public List<StackHolderList> searchStakeHolder { get; set; }
        public int? CountRecord { get; set; }

        //public string Employee { get; set; }
        //public string Role { get; set; }
        //public string BusinessGroup { get; set; }
        //public string OrganizationUnit { get; set; }
    }

    public class StackHolderList
    {
        public int? EmployeeID { get; set; }
        public string Employee { get; set; }
        public string BusinessGroup { get; set; }
        public string OrganizationUnit { get; set; }
        public string Role { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string EmpStatus { get; set; }
        public string Location { get; set; }
        public string TotalExperiance { get; set; }
        public string V2Experiance { get; set; }
        public bool isChecked { get; set; }
        public string StackHolderId { get; set; }
        public int CollectedEmployeeID { get; set; }
    }
}