using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ConfirmationParameter
    {
        public int employeeID { get; set; }

        public int confirmationID { get; set; }

        public int competencyID { get; set; }

        public int parameterID { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string EmpComments { get; set; }

        public int? SelfRating { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string MngrComments1 { get; set; }

        public int? ManagerRating1 { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string MngrComments2 { get; set; }

        public int? ManagerRating2 { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string ReviewerComments { get; set; }

        public int? ReviewerRating { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string HrComments { get; set; }

        public int? HRrRating { get; set; }
        public string ParameterDescription { get; set; }
        public int OverallReviewRating { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string OverallReviewRatingComments { get; set; }

        public int OverallReviewHRRating { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string OverallReviewHRComments { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string OverallManagerRatingComments { get; set; }

        public int OverallManagerRating { get; set; }

        public string MgrName { get; set; }
        public string MgrNameSecond { get; set; }
        public string RevName { get; set; }
        public string HRName { get; set; }

        public string IsManagerOrEmployee { get; set; }
    }
}