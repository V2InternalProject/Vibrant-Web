using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class EmployeeQualifications
    {
        public int EmployeeQualificationID { get; set; }

        public int EmployeeQualificationHistoryId { get; set; }

        public int? EmployeeID { get; set; }

        [Required]
        public int? QualificationID { get; set; }

        [Required]
        public string Qualification { get; set; }

        [Required]
        public int? DegreeID { get; set; }

        [Required]
        public string Degree { get; set; }

        //[Required]
        //[RegularExpression("^[a-zA-Z .]+$", ErrorMessage = "Use letters and . only please")]
        [StringLength(200, ErrorMessage = "Specialization can not be more than 200 characters.")]
        public string Specialization { get; set; }

        [Required]
        //[RegularExpression("^[a-zA-Z .]+$", ErrorMessage = "Use letters and . only please")]
        [StringLength(100, ErrorMessage = "Institute can not be more than 100 characters.")]
        public string Institute { get; set; }

        [Required]
        //[RegularExpression("^[a-zA-Z .]+$", ErrorMessage = "Use letters and . only please")]
        [StringLength(200, ErrorMessage = "University can not be more than 200 characters.")]
        public string University { get; set; }

        //[Required]
        //[RegularExpression("^[0-9a-zA-Z .]+$", ErrorMessage = "Use letters and . only please")]
        //public string Course { get; set; }

        [Required]
        //[Range(typeof(int), "1900", "2099")]
        public int? Year { get; set; }

        [Required]
        public int? TypeID { get; set; }

        [Required]
        public string Type { get; set; }

        public string ApprovedValue { get; set; }

        public string ApprovedType { get; set; }

        public string ApprovedComments { get; set; }

        public int? ApprovalStatusMasterID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Percentage/Grade can not be more than 50 characters.")]
        //[RegularExpression(@"^[a-zA-Z 0-9\.\+\-]*$", ErrorMessage = "Please enter valid percentage or grade.")]
        public string Percentage { get; set; }

        public int? Status { get; set; }

        public string ApprovalOrRejectionStatus { get; set; }

        public string ActionType { get; set; }

        public string ApprovalStatusFlag { get; set; }

        public string CanSendMail { get; set; }
    }
}