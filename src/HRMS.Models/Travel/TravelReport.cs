using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class TravelReport
    {
        public List<TravelReportViewModel> TravelReportList { get; set; }
        public List<ConveyanceAdminViewModel> TravelAirportToHotel { get; set; }
        public List<EmergencyContactViewModel> IndEmergencyContactDetails { get; set; }
        public List<AccomodationAdmin> GetAccomodationDetailsList { get; set; }
        public List<ClientViewModel> GetClientDetailsList { get; set; }
        public List<JourneyList> GetJourneyDetailsList123 { get; set; }
        public List<OtherAdminViewModel> GetOtherRequirementDetailsList { get; set; }
        public List<GetTravelID> GetTravelIDList { get; set; }
        public List<int> JourneyDetailsId { get; set; }
    }

    public class TravelReportViewModel
    {
        public int travelid { get; set; }
        public String TRFNO { get; set; }
        public int? EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Group { get; set; }
        public string ReportingManager { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DepartFromBaseLocation { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ArrivalDestination { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DepartFromDestination { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ArrivalBaseDestination { get; set; }

        public string TavelContactNo { get; set; }

        public string TravelAirportToHotel { get; set; }

        public string HotelAddress { get; set; }
        public string HotelRoomNo { get; set; }

        public int? ClientName { get; set; }
        public string clientAddress { get; set; }
        public string ClientContactPerson { get; set; }
        public string ClientContactNo { get; set; }

        public string VisaValiditydate { get; set; }
        public string InsurenceDetails { get; set; }

        public string IndEmergencyContactDetails { get; set; }
        public string TravelStatus { get; set; }
        public string Comments { get; set; }

        public string TicketAttachment { get; set; }
        public string TicketName { get; set; }
        public string InsurenceAttachment { get; set; }
        public int Counter { get; set; }
    }

    public class GetTravelID
    {
        public int? TravelId { get; set; }

        public int? StageId { get; set; }
    }
}