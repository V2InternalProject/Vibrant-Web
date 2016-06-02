using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class DeclarationDetailsViewModel
    {
        // public int? DependandsId { get; set; }

        public int? EmployeeID { get; set; }
        public int? DeclarationId { get; set; }

        public List<DeclarationsDetails> DeclarationDetailsList { get; set; }

        public DeclarationsDetails DeclarationDetail { get; set; }

        [Required]
        public int? RelationshipID { get; set; }

        [Required]
        public int? V2EmployeeID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int EmployeeCode { get; set; }

        public string UserRole { get; set; }

        public List<DependantDetails> RelationList { get; set; }

        public int EmpStatusMasterID { get; set; }

        [Required]
        public string DependandsRelation { get; set; }

        [Required]
        public int? uniqueID { get; set; }

        public List<DeclarationsDetails> EmployeeList { get; set; }
        public List<EpmType> EmpTypeList { get; set; }
    }

    public class EpmType
    {
        [Required]
        public int EmpTypeIds { get; set; }

        [Required]
        public string EmpTypeNames { get; set; }
    }
}