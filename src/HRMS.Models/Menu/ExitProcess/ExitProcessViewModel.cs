using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ExitProcessViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public string UserRole { get; set; }

        public int? EmployeeId { get; set; }

        public int SeparationId { get; set; }

        public int TerminatedEmpId { get; set; }

        [Required(ErrorMessage = "Please enter your comment")]
        public string EmpComment { get; set; }

        public string EmpName { get; set; }

        [Required(ErrorMessage = "Resigned Date is Required")]
        [DataType(DataType.Date)]
        public DateTime? ResignedDate { get; set; }

        [Required]
        public int? NoticePeriod { get; set; }

        [Required(ErrorMessage = "Please select Reason for Exit")]
        public string ReasonForSeparartion { get; set; }

        [Required(ErrorMessage = "Tentative Release Date is Required")]
        [DataType(DataType.Date)]
        public DateTime? TentativeReleaseDate { get; set; }

        [Required(ErrorMessage = "Agreed Release Date is Required")]
        [DataType(DataType.Date)]
        public DateTime? AgreedReleaseDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SystemReleavingDate { get; set; }

        public List<SeparationReason> SeparationReasonList { get; set; }

        public List<ModeOfSeperation> ModeOfSeperationList { get; set; }

        public SeparationMailTemplate Mail { get; set; }

        [Required(ErrorMessage = "Please select Mode for Exit")]
        public string ModeOfSeparation { get; set; }

        public string Isterminate { get; set; }

        public int? StageId { get; set; }

        public string stageName { get; set; }

        [Required(ErrorMessage = "Please enter Initiator Comment.")]
        public string InitiatorComment { get; set; }

        public string ManagerComment { get; set; }

        public string HRComment { get; set; }

        public string RMGComment { get; set; }

        public string WaiveOff { get; set; }

        public bool? IsWithdraw { get; set; }
    }

    public class ModeOfSeperation
    {
        public int SeperationId { get; set; }

        public string SeperationName { get; set; }
    }

    public class SeparationReason
    {
        public int ReasonId { get; set; }

        public string Reason { get; set; }
    }
}