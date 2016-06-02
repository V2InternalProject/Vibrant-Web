using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ConfirmationViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<CompetencyMaster> CompetencyMasters { get; set; }

        [Display(Name = "Total Records : ")]
        public int RecordsCount { get; set; }
    }

    public class CompetencyMaster
    {
        public int CompentancyID { get; set; }

        public string Compentancy { get; set; }

        public string Description { get; set; }

        public int? OrderNo { get; set; }

        public int? categoryID { get; set; }

        public bool Checked { get; set; }
    }
}