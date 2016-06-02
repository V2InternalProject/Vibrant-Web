namespace HRMS.Models
{
    public class PhaseManagementModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public int PhaseID { get; set; }
        public string PhaseDescription { get; set; }
    }

    public class PhaseManagementDetails
    {
        public int PhaseID { get; set; }
        public string PhaseDescription { get; set; }
        public string CreatedBy { get; set; }
    }
}