namespace HRMS.Models
{
    public class SearchedUserDetails
    {
        public int EmployeeId { get; set; }

        public string EmployeeFullName { get; set; }
        public string EncryptedEmployeeId { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeEmailId { get; set; }

        public string UserRole { get; set; }

        public string UserName { get; set; }

        public bool IsProjectReviewer { get; set; }

        public string Describtion { get; set; }

        public string createdBy { get; set; }

        public int? repotingTo { get; set; }
    }
}