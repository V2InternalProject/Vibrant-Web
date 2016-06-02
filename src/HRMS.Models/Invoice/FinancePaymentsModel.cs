namespace HRMS.Models
{
    public class FinancePaymentsModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public PaymentTrackingHistory PaymentTrackingHistory { get; set; }
        public TrackingSummaryHistory TrackingSummaryHistory { get; set; }
    }
}