using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class IndividualDevelopmentStatusReport
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DeliveryTeam { get; set; }
        public string Designation { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ConfirmationDate { get; set; }

        public string Status { get; set; }
        public string ApraiseeComment { get; set; }
        public string Appraiseeagreedisagree { get; set; }
        public string EmployeeEmail { get; set; }
        public string Appraiser1Email { get; set; }
    }
}