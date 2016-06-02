using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ProjectDetailsViewModel
    {
        [Display(Name = "Project Id")]
        public int ProjectDetailID { get; set; }

        [Display(Name = "Project resource Id")]
        public int ProjectResourceID { get; set; }

        [Required]
        [Display(Name = "Employee Id")]
        public int? EmployeeId { get; set; }

        //[Required]
        //[Display(Name = "Project Name")]
        //public string projectName { get; set; }

        [Required]
        [Display(Name = "Project Start Date")]
        public DateTime? FromDate { get; set; }

        [Required]
        [Display(Name = "Project End Date")]
        public DateTime? ToDate { get; set; }

        [Required]
        [Display(Name = "Current Role")]
        public string CurrentRole { get; set; }

        [Required]
        [Display(Name = "Current Project")]
        public string CurrentProject { get; set; }

        [Required]
        [Display(Name = "Current Reporting Manager")]
        public string CurrentReportingManager { get; set; }

        [Required]
        [Display(Name = "Current Delivery Head / GroupHead")]
        public string CurrentDeliveryHead { get; set; }

        [Required]
        public string Appraiser { get; set; }

        [Required]
        public string Reviewer { get; set; }

        [Required]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [Required]
        [Display(Name = "Delivery Unit")]
        public string DeliveryUnit { get; set; }

        [Required]
        [Display(Name = "Resource Pool Name")]
        public string ResourcePoolName { get; set; }

        [Required]
        [Display(Name = "Resource Pool Manager")]
        public string ResourcePoolManager { get; set; }

        public string UserRole { get; set; }

        public int EmpStatusMasterID { get; set; }
    }
}