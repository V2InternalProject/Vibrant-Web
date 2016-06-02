using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AddNewCategory
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public bool IsAddnew { get; set; }

        public int CategoryID { get; set; }
    }
}