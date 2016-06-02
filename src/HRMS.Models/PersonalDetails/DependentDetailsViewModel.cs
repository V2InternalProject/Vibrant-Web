using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class DependentDetailsViewModel
    {
        public int? DependandsId { get; set; }

        public int? EmployeeId { get; set; }

        public List<DependantDetails> DependantDetailsList { get; set; }

        public DependantDetails DependantDetail { get; set; }

        [Required]
        public int? uniqueID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Dependent Name cannot be greater than 100 characters.")]
        //[RegularExpression(@"^[a-zA-Z\\\-\[\]\""._^%$#!~@,&*()+={}|:;'<>/`? ]+$", ErrorMessage = "Dependands name cannot contain numbers.")]
        public string DependandsName { get; set; }

        [Required]
        public string DependandsRelation { get; set; }

        public string UserRole { get; set; }

        [Required]
        public DateTime DependandsBirthDate { get; set; }

        [Required]
        public int? DependandsAge { get; set; }

        public List<DependantDetails> RelationList { get; set; }

        public int EmpStatusMasterID { get; set; }
    }
}