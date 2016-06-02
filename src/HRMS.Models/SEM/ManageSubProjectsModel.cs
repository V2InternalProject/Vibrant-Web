using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ManageSubProjectsModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        //public List<PMSProjectDetailsViewModel> ProjectList { get; set; }

        public List<ProjectAppList> ProjectList { get; set; }

        public string Prj { get; set; }

        public string loggedInUserEmployeeCode { get; set; }

        public int? HiddenResponsiblePerson { get; set; }

        public int? SubProjectId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Sub Project Name can not be greater than 100 characters.")]
        public string SubProjectName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Sub Project Description can not be greater than 100 characters.")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        public Double? WorkHours { get; set; }

        public string ResponsiblePerson { get; set; }

        public int? ProjectID { get; set; }
    }
}