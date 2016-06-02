namespace HRMS.common
{
    public class HRMSResponse
    {
        public long IncidentID { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}