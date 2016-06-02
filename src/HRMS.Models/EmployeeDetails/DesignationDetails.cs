using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HRMS.Models
{
    public class DesignationDetails
    {
        public int? EmployeeId { get; set; }

        [Required]
        //[Range(typeof(string), "1900", "2099")]
        public int? Year { get; set; }

        [Required]
        [StringLength(9, ErrorMessage = "Month can not be greater than 9 digits")]
        public string Month { get; set; }

        public string Grade { get; set; }

        public int? GradeId { get; set; }

        public bool? isDefaultRecord { get; set; }

        public string Level { get; set; }

        [Required]
        [Remote("CheckDesignation", "EmployeeDetails", ErrorMessage = "Please enter correct designation.")]
        //[StringLength(25, ErrorMessage = "Designation can not be greater than 25 digits")]
        public string Designation { get; set; }

        [Display(Name = "Role Description")]
        [StringLength(100, ErrorMessage = "Role Description can not be greater than 100 characters.")]
        public string RoleDescription { get; set; }

        [Required]
        [Remote("CheckJoiningDesignation", "EmployeeDetails", ErrorMessage = "Please enter correct designation.")]
        //[StringLength(25, ErrorMessage = "Joining Designation can not be greater than 25 digits")]
        [Display(Name = "Joining Designation")]
        public string JoiningDesignation { get; set; }

        //required for edit functionality
        public int? UniqueId { get; set; }

        public List<SelectListItem> JoiningMonth;

        public List<SelectListItem> GetMonths()
        {
            List<SelectListItem> list = new List<SelectListItem>
				{
					new SelectListItem { Selected = true, Text = "January", Value = "1" },
					new SelectListItem { Selected = true, Text = "February", Value = "2" },
					new SelectListItem { Selected = true, Text = "March", Value = "3" },
					new SelectListItem { Selected = true, Text = "April", Value = "4" },
					new SelectListItem { Selected = true, Text = "May", Value = "5" },
					new SelectListItem { Selected = true, Text = "June", Value = "6" },
					new SelectListItem { Selected = true, Text = "July", Value = "7" },
					new SelectListItem { Selected = true, Text = "August", Value = "8" },
					new SelectListItem { Selected = true, Text = "September", Value = "9" },
					new SelectListItem { Selected = true, Text = "October", Value = "10" },
					new SelectListItem { Selected = true, Text = "November", Value = "11" },
					new SelectListItem { Selected = true, Text = "December", Value = "12" },
				};
            return list;
        }

        //public List<SelectListItem> GetYears()
        //{
        //    List<SelectListItem> list_years = new List<SelectListItem>
        //        {
        //            new SelectListItem { Selected = true, Text = DateTime.Now.Year.ToString() , Value = "1" },

        //        };
        //    return list_years;
        //}
    }
}