using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class DependantDetails
    {
        public int? DependandsId { get; set; }

        public int? EmployeeId { get; set; }

        [Required]
        public int? uniqueID { get; set; }

        [Required]
        public string DependandsRelation { get; set; }

        [Required]
        public string DependandsName { get; set; }

        //[Required]
        //[Display(Name = "Relation")]
        //public string DependandsRelation { get; set; }

        [Required]
        public DateTime? DependandsBirthDate { get; set; }

        [Required]
        public int? DependandsAge { get; set; }
    }
}