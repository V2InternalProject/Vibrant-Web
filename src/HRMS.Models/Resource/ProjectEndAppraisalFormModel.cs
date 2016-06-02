using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class ProjectEndAppraisalFormModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<RatingScales> RatingScale = new List<RatingScales>();

        public List<ProjectEndAppraisalParameters> ProjectEndAppraisalParameterList { get; set; }

        public string LoggedinUserEmployeeCode { get; set; }

        public string EmployeeCode { get; set; }

        public int? EmployeeID { get; set; }

        public int? ProjectID { get; set; }

        public string EmployeeName { get; set; }

        public string ProjectName { get; set; }

        public string ProjectLead { get; set; }

        public string ProjectManager { get; set; }

        public int? ProjectEndAppraisalFormStatus { get; set; }

        public DateTime? JoiningDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public DateTime? AllocationStartDate { get; set; }

        public string Remarks { get; set; }

        public List<int> RatingsList { get; set; }

        public int MainRatingID { get; set; }

        public decimal? MainPercentage { get; set; }

        public ParameterRating parameterRating { get; set; }

        public List<RatingList> ratingList { get; set; }

        public int ProjectEmployeeRoleID { get; set; }
    }

    public class ProjectEndAppraisalParameters
    {
        public int ProjectEndAppraisalParameterID { get; set; }

        public string ProjectEndAppraisalParameter { get; set; }

        public decimal? ProjectEndAppraisalParameterRating { get; set; }

        public string ProjectEndAppraisalParameterRemarks { get; set; }

        public int? EmployeeID { get; set; }

        public int? ProjectID { get; set; }

        public string LoggedinUserEmployeeCode { get; set; }

        public int? ProjectEndAppraisalFormStatus { get; set; }

        public int ProjectEndAppraisalFormID { get; set; }

        public string ProjectLead { get; set; }

        public string State { get; set; }

        public DateTime? Releasedate { get; set; }

        public int ProjectEmployeeRoleID { get; set; }
    }

    public class ParameterRating
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }

    public class RatingList
    {
        public int RatingID { get; set; }
        public decimal? Percentage { get; set; }
    }
}