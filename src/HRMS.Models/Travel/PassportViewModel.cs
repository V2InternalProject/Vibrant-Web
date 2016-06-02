using System;

namespace HRMS.Models
{
    public class PassportViewModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public int PassportID { get; set; }

        public int EmployeeID { get; set; }

        public string UserRole { get; set; }

        public string EmployeeCode { get; set; }

        public int TravelID { get; set; }

        public int DocumentID { get; set; }

        public string PassportNumber { get; set; }

        public string SonofWifeOfDaughterof { get; set; }

        public DateTime? DateOfIssue { get; set; }

        public string PlaceOfIssue { get; set; }

        public DateTime? DateOfExpiry { get; set; }

        public int? NumberOfPagesLeft { get; set; }

        public string FullNameAsInPassport { get; set; }

        public string PassportFileName { get; set; }

        public string PassportFilePath { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}