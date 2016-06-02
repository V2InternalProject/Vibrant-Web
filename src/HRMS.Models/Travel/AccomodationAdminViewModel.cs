using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AccomodationAdminViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public AccomodationAdmin newAccomodationAdmin { get; set; }
        public List<AccomodationAdmin> AccomodationDetailsList { get; set; }
    }

    public class AccomodationAdmin
    {
        public int AccomodationID { get; set; }

        public int TravelId { get; set; }

        public int LoggedInEmployeeId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string HotelName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string HotelAddress { get; set; }

        [Required]
        [RegularExpression(@"^[0-9.\+\(\)\-\s]+$", ErrorMessage = "Hotel Contact Number can not contain alphabets.")]
        [StringLength(100, ErrorMessage = "Hotel Contact Number India can not be greater than 100 characters.")]
        public string HotelContactNumber { get; set; }

        [Required]
        public DateTime? BookingFromDate { get; set; }

        [Required]
        public DateTime? BookingToDate { get; set; }

        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string RoomDetails { get; set; }

        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string CheckinDetails { get; set; }

        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string CheckoutDetails { get; set; }

        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string AdditionalDetails { get; set; }

        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string AdditionalInformation { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [StringLength(50, ErrorMessage = "Maxium 50 characters are allowed")]
        public string FileName { get; set; }

        [StringLength(500, ErrorMessage = "Maxium 500 characters are allowed")]
        public string FilePath { get; set; }

        public string FileUpload { get; set; }
    }
}