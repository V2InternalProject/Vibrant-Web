using HRMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class EmpSeperationTerminationViewModel
    {
        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "User Id")]
        public int? UserId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        public DateTime? JoiningDate { get; set; }

        public DateTime? ProbationReviewDate { get; set; }

        public DateTime? EventDate { get; set; }

        public string Grade { get; set; }

        public string hiddenEmpId { get; set; }

        public int ExitProcessID { get; set; }

        public string RoleInitiate { get; set; }

        public string BusinessGroup { get; set; }

        public string StageName { get; set; }

        public string OrganizationGroup { get; set; }

        public string ddlOrganizationGroup { get; set; }

        [Required]
        public string ReportingManager { get; set; }

        //public string ReportingManager2 { get; set; }

        //[Required]
        //public string Reviewer { get; set; }

        //[Required]
        //public string HRReviewer { get; set; }

        //[Required]
        //public string ConfirmationCoordinator { get; set; }

        [Required]
        public DateTime InitiationDate { get; set; }

        public string IsExit { get; set; }

        [Required]
        public string Comments { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }

        private List<EmpSeperationTerminationViewModel> emplist { get; set; }

        public List<MailTemplateViewModel> MailDetailsList { get; set; }

        public MailTemplateViewModel MailDetail { get; set; }

        //public InitiatConfirmationProcess InitiatConfirmationlist { get; set; }

        //List<string> searchlist { get; set; }

        [Display(ResourceType = typeof(EmployeeMessages), Name = "SearchEmployeeLabel")]
        public string SearchText { get; set; }
    }
}