using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class PastEmployeeExperienceViewModel
    {
        public int? EmployeeId { get; set; }

        public List<PastEmployeeExperienceDetails> PastExperienceList { get; set; }

        public PastEmployeeExperienceDetails NewExperience { get; set; }

        public string UserRole { get; set; }

        public int EmpStatusMasterID { get; set; }

        public int? EmployeeHistoryId { get; set; }

        [Required]
        public int EmployeeTypeId { get; set; }
    }
}