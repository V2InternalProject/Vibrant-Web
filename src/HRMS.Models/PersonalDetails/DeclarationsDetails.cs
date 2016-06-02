using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class DeclarationsDetails
    {
        [Required]
        public int? EmployeeID { get; set; }

        [Required]
        public int? DeclarationId { get; set; }

        [Required]
        public int? RelationshipID { get; set; }

        [Required]
        public int? V2EmployeeID { get; set; }

        [Required]
        public string V2EmployeeName { get; set; }

        [Required]
        public string RelationshipName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int? uniqueID { get; set; }

        [Required]
        [DisplayName("Employee Code")]
        public int EmployeeCode { get; set; }

        [Required]
        public int? EmpTypeIds { get; set; }

        [Required]
        public string EmpTypeNames { get; set; }
    }

    //public class EpmType
    //{
    //    [Required]
    //    public int? EmpTypeIds { get; set; }
    //    [Required]
    //    public string EmpTypeNames { get; set; }
    //}
}