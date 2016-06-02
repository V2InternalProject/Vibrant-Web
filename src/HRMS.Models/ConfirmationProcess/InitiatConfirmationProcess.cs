using HRMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class InitiatConfirmationProcess
    {
        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "User Id")]
        public int? UserId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        public string encryptedInitiatedEmployeeId { get; set; }

        public DateTime? JoiningDate { get; set; }

        public DateTime? ProbationReviewDate { get; set; }

        public DateTime? EventDate { get; set; }

        public string Grade { get; set; }

        public int confirmationid { get; set; }

        public int? ConfirmationStatus { get; set; }

        public string RoleInitiate { get; set; }

        public string BusinessGroup { get; set; }

        public string StageName { get; set; }

        public string OrganizationUnit { get; set; }

        public string encryptedEmployeeId { get; set; }

        [Required]
        public string ReportingManager { get; set; }

        public string ReportingManager2 { get; set; }

        [Required]
        public string Reviewer { get; set; }

        [Required]
        public string HRReviewer { get; set; }

        [Required]
        public string ConfirmationCoordinator { get; set; }

        [Required]
        public DateTime InitiationDate { get; set; }

        public string CurrentStage { get; set; }

        [Required]
        public string Comments { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }

        private List<InitiatConfirmationProcess> emplist { get; set; }

        public List<MailTemplateViewModel> MailDetailsList { get; set; }

        public MailTemplateViewModel MailDetail { get; set; }

        [Display(ResourceType = typeof(EmployeeMessages), Name = "SearchEmployeeLabel")]
        public string SearchText { get; set; }
    }
}