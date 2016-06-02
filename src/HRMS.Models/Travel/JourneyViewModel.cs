using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class JourneyViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<JourneyList> JourneyDetailsList { get; set; }
        public JourneyList JourneyDetail { get; set; }
        public JourneyViewModel JourneyDetails { get; set; }
        public List<JourneyModeList> JourneyModeList { get; set; }
    }

    public class JourneyList
    {
        public int? TravelID { get; set; }
        public int JourneyID { get; set; }
        public int EmployeeID { get; set; }
        public int StageID { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Maxium 1000 characters are allowed")]
        public string FromPlace { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Maxium 1000 characters are allowed")]
        public string ToPlace { get; set; }

        [Required]
        public DateTime? JourneyDate { get; set; }

        [Required]
        public string JourneyMode { get; set; }

        public int JourneyModeID { get; set; }

        //[Required]
        [StringLength(1000, ErrorMessage = "Maxium 1000 characters are allowed")]
        public string AdditionalInformation { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Maxium 1000 characters are allowed")]
        public string JourneyModeDetails { get; set; }

        [StringLength(1000, ErrorMessage = "Maxium 1000 characters are allowed")]
        public string JourneyFeedback { get; set; }

        [Required]
        public string TicketName { get; set; }

        public string JourneyFilePath { get; set; }

        public string TRFNo { get; set; }

        public string TicketNameUpload { get; set; }

        public List<JourneyModeList> JourneyModeList { get; set; }

        public string JourneyModeHidden { get; set; }
    }

    public class JourneyModeList
    {
        public int JourneyModeId { get; set; }
        public string JourneyModeDescription { get; set; }
    }
}