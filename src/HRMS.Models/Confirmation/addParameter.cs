using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class addParameter
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Required]
        [Display(Name = "Parameter")]
        public string Parameter { get; set; }

        [Required]
        [Display(Name = "Order Number")]
        public int? OrderNo { get; set; }

        public List<CategoryList> CategoryList { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string category { get; set; }

        [Display(Name = "Behavioral Indicators")]
        public string BehavioralIndicators { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public bool IsAddnew { get; set; }

        public int CompetencyID { get; set; }
    }

    public class CategoryList
    {
        public int CategoryID { get; set; }

        public string CategoryType { get; set; }
    }
}