using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class MailTemplateViewModel
    {
        public int employeeId { get; set; }

        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        public string Cc { get; set; }

        [Required]
        public string Subject { get; set; }

        public string Note { get; set; }

        [Required]
        public string Message { get; set; }
    }
}