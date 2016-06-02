using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AccomodationViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int TravelRequirementsID { get; set; }

        public int TravelEmployeeId { get; set; }

        public int TravelId { get; set; }

        public int? StageID { get; set; }

        public string EncryptedTravelId { get; set; }

        public string EncryptedEmployeeId { get; set; }

        public string Comments { get; set; }

        [Required(ErrorMessage = "Accomodation Needed is required")]
        public int? AccomodationNeeded { get; set; }

        [Required(ErrorMessage = "Airport PickUp Needed is required")]
        public int? AirportPickUpNeeded { get; set; }

        //[Required]
        //public int? ShuttelNeeded { get; set; }

        [Required(ErrorMessage = "Laptop Needed is required")]
        public int? LaptopNeeded { get; set; }

        [Required(ErrorMessage = "Hard Drive Needed is required")]
        public int? HardDriveNeeded { get; set; }

        [Required(ErrorMessage = "USB Drive Needed is required")]
        public int? USBDriveNeeded { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Softwares Needed field can not be greater than 1000 characters.")]
        public String SoftwaresNeeded { get; set; }

        [StringLength(1000, ErrorMessage = "Maxium 1000 characters are allowed")]
        public string AdditionalInformation { get; set; }

        public List<ClientTravelAccomodationList> ClientTravelYesNoList { get; set; }
    }

    public class ClientTravelAccomodationList
    {
        public int ClientTraveAccomodationlsId { get; set; }

        public string ClientTraveAccomodationlsValue { get; set; }
    }
}