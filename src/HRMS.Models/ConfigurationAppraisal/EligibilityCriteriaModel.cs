using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class EligibilityCriteriaModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<AllEligibileEmployee> allEmployeeList { get; set; }
        public int allEmployeeListCount { get; set; }
        public List<AllEligibileEmployee> allConfirmationDateEmployeeList { get; set; }
        public int allConfirmationDateEmployeeListCount { get; set; }
        public List<AllEligibileEmployee> allSuccessEmployeeList { get; set; }
        public int ApprasialYearID { get; set; }
        public EmployeeMailTemplate Mail { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public List<AllIniEmployees> allIniEmployees { get; set; }
        public List<Designation> DesignationList { get; set; }
    }

    public class AllEligibileEmployee
    {
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DeliveryTeam { get; set; }
        public string Designation { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? ProbationReviewDate { get; set; }
        public bool Checked { get; set; }
        public int AppraisalYearID { get; set; }
        public int? DesignationID { get; set; }
    }

    public class AllIniEmployees
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DeliveryTeam { get; set; }
        public string Designation { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? ProbationReviewDate { get; set; }
    }
}