using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class CertificationDetails
    {
        public string Type { get; set; }

        public string Value { get; set; }

        //  public int Status { get; set; }

        public string Comments { get; set; }

        public int? ApprovalStatusMasterID { get; set; }

        public int EmployeeCertificationID { get; set; }

        public int EmployeeCertificationHistoryID { get; set; }

        public int? EmployeeID { get; set; }

        public int? CertificationID { get; set; }

        public int? CertificationNameID { get; set; }

        [Required]
        [DisplayName("Certification Name")]
        public string CertificationName { get; set; }

        [Required]
        [DisplayName("Certification Number")]
        [StringLength(50, ErrorMessage = "Certification Number can not be greater that 50 character.")]
        public string CertificationNo { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Institution name can not contain more than 500 characters.")]
        public string Institution { get; set; }

        [Required]
        [DisplayName("Certification Date")]
        public DateTime? CertificationDate { get; set; }

        [Required]
        [DisplayName("Certification Score")]
        [StringLength(30, ErrorMessage = "Certification score can not contain more than 30 characters.")]
        public string CertificationScore { get; set; }

        [Required]
        [DisplayName("Certification Grade")]
        [StringLength(30, ErrorMessage = "Certification grade can not contain more than 30 characters.")]
        public string CertificationGrade { get; set; }

        public int? ApproveStatus { get; set; }

        public string Status { get; set; }

        public string ActionType { get; set; }

        public string ApprovalStatusFlag { get; set; }

        public string CanSendMail { get; set; }
    }
}