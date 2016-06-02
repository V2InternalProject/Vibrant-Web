using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMS.Models.Appraisal
{
    public class AppraisalProcessParameterViewModel
    {
        public int employeeID { get; set; }

        public int confirmationID { get; set; }

        public int competencyID { get; set; }

        public int parameterID { get; set; }
        public string EmpComments { get; set; }
        public int? SelfRating { get; set; }
        public string MngrComments1 { get; set; }
        public int? ManagerRating1 { get; set; }
        public string MngrComments2 { get; set; }
        public int? ManagerRating2 { get; set; }
        public string ReviewerComments { get; set; }
        public int? ReviewerRating { get; set; }
        public string HrComments { get; set; }
        public int? HRrRating { get; set; }
        public string ParameterDescription { get; set; }

        public int OverallReviewRating { get; set; }
        public string OverallReviewRatingComments { get; set; }
        public int OverallReviewHRRating { get; set; }
        public string OverallReviewHRComments { get; set; }

        public string MgrName { get; set; }
        public string MgrNameSecond { get; set; }
        public string RevName { get; set; }
        public string HRName { get; set; }

        public string IsManagerOrEmployee { get; set; }
    }
}
