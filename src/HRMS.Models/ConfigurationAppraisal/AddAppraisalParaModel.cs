using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AddAppraisalParaModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        [Required]
        [Display(Name = "Parameter")]
        public string Parameter { get; set; }

        [Required]
        [Display(Name = "Order Number")]
        public int? OrderNo { get; set; }

        public List<ParameterCategoryList> ParameterCategoryList { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string category { get; set; }

        [Display(Name = "Behavioral Indicators")]
        [StringLength(1000, ErrorMessage = "Behavioral Indicators cannot be greater than 1000 characters.")]
        public string BehavioralIndicators { get; set; }

        [Display(Name = "Description")]
        [StringLength(1000, ErrorMessage = "Description cannot be greater than 1000 characters.")]
        public string ParameterDescription { get; set; }

        public bool IsAddnew { get; set; }

        public int ParameterID { get; set; }

        public int AppraisalYearID { get; set; }

        public int? SelectedOrderNo { get; set; }

        public StatusForOrderNoAndParameter Status { get; set; }
    }

    public class ParameterCategoryList
    {
        public int ParameterCategoryID { get; set; }

        public string ParameterCategory { get; set; }
    }

    public class StatusForOrderNoAndParameter
    {
        public bool IsOrderNumber { get; set; }

        public bool IsParameter { get; set; }
    }
}