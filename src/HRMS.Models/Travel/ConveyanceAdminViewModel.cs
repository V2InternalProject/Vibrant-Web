using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ConveyanceAdminViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int LocalConveyanceID { get; set; }

        public int TravelID { get; set; }

        [Required]
        public int? ConveyanceType { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string TravelDetails { get; set; }

        [Required]
        [StringLength(1000)]
        public string FromAddress { get; set; }

        [Required]
        [StringLength(1000)]
        public string ToAddress { get; set; }

        [Required]
        public DateTime? FromDate { get; set; }

        //[Required]
        public DateTime? ToDate { get; set; }

        [StringLength(100)]
        public string ReservationNumber { get; set; }

        [StringLength(100)]
        public string InsuranceDetails { get; set; }

        [StringLength(500, ErrorMessage = "Maxium 500 characters are allowed")]
        public string AdditionalInformation { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string CityName { get; set; }

        public string ConvayName { get; set; }

        public string ConvayNameHidden { get; set; }

        public List<ConveyType> ConavaytypeList { get; set; }

        public List<CityT> CityList { get; set; }

        public string ConveyplusTravelDetails { get; set; }

        public int? AirporttoHotel { get; set; }

        public bool HoteltoAirport { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string AirportName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maxium 100 characters are allowed")]
        public string HotelName { get; set; }

        public string TravelingFrom { get; set; }

        public List<ConveyanceAdminViewModel> viewmodellist { get; set; }

        public int ConvayanceListID { get; set; }

        public string SelectedHoteltoAirport { get; set; }
    }

    public class ConveyType
    {
        public int ConvayListID { get; set; }
        public string ConvayListName { get; set; }
    }

    public class CityT
    {
        public int CityID { get; set; }
        public string CityName { get; set; }
    }
}