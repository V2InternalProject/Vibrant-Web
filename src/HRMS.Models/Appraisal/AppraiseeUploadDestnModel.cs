using System;

namespace HRMS.Models
{
    public class AppraiseeUploadDestnModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public string LoggedInEmplyeeId { get; set; }

        public int? EmployeeCode { get; set; }

        public int? EmployeeID { get; set; }

        public int? Year { get; set; }

        public string Month { get; set; }

        public int? CurrentGradeID { get; set; }

        public string Level { get; set; }

        public string PreDesignationName { get; set; }

        public int? PreDesignationID { get; set; }

        public string RoleDescription { get; set; }

        public string Comments { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public int? ApproverId { get; set; }

        public string NewDesinationChangedTo { get; set; }

        public int? NewDesiganationId { get; set; }

        public bool IsEmployeeUpdated { get; set; }
    }
}