using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ConfigureParameterCategory
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<Parametercompetency> Parametercompetencys { get; set; }

        [Display(Name = "Total Records : ")]
        public int RecordsCount { get; set; }
    }

    public class Parametercompetency
    {
        public int CategoryID { get; set; }

        public string CategoryType { get; set; }

        public string CategoryDescription { get; set; }

        public bool Checked { get; set; }
    }
}