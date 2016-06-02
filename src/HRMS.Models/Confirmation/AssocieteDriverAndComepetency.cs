using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AssocieteDriverAndComepetency
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<Competencies> CompetencyList { get; set; }

        [Display(Name = "Total Records : ")]
        public int RecordsCount { get; set; }

        public int RoleID { get; set; }
    }

    public class Competencies
    {
        public int? OrderNo { get; set; }

        public int CompetencyID { get; set; }

        public string Parameter { get; set; }

        public string Description { get; set; }

        public bool Checked { get; set; }

        public int RoleID { get; set; }
    }
}