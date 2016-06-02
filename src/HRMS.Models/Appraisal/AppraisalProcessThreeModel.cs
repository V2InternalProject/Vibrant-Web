using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AppraisalProcessThreeModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public int EmployeeID { get; set; }
        public int AppraisalID { get; set; }
        public int AppraisalYearID { get; set; }
        public int StageID { get; set; }
        public string IsManagerOrEmployee { get; set; }
        public string Designation { get; set; }
        public string Reviewer1Name { get; set; }
        public string Reviewer2Name { get; set; }
        public string GroupHeadName { get; set; }

        // public List<AppraisalEmployeeGrowthSummary> AppraisalEmpGrowthSummaryList { get; set; }
        public AppraisalEmployeeGrowthSummary AppraisalEmpGrowthSummary { get; set; }

        public List<Parameters> parameterListForReviewerOne { get; set; }
        public List<Parameters> parameterListForReviewerTwo { get; set; }
        public List<Parameters> parameterListForGroupHead { get; set; }
        public int? Reviewer1OverallRating { get; set; }
        public int? Reviewer2OverallRating { get; set; }
        public int? GroupHeadOverallRating { get; set; }
        public int DesignationID { get; set; }

        // public int? NextDesignationID { get; set; }
        public int? NextDesignationIDFromReviewer1 { get; set; }

        public int? NextDesignationIDFromReviewer2 { get; set; }
        public int? NextDesignationIDFromGropHead { get; set; }
        public List<Designation> DesignationList { get; set; }

        public bool Reviewer1Recomendation { get; set; }
        public bool Reviewer2Recomendation { get; set; }
        public bool GroupHeadRecomendation { get; set; }

        [StringLength(2000, ErrorMessage = "Reviewer1 RecomendationComments can not be greater than 2000 characters.")]
        public string Reviewer1RecomendationComments { get; set; }

        [StringLength(2000, ErrorMessage = "Reviewer2 RecomendationComments can not be greater than 2000 characters.")]
        public string Reviewer2RecomendationComments { get; set; }

        [StringLength(2000, ErrorMessage = "GroupHead RecomendationComments can not be greater than 2000 characters.")]
        public string GroupHeadRecomendationComments { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer1 ReadyNow Comments can not be greater than 1000 characters.")]
        public string ReadyCommentsByReviewer1 { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer2 ReadyNow Comments can not be greater than 1000 characters.")]
        public string ReadyCommentsByReviewer2 { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer1 1to2years Comments can not be greater than 1000 characters.")]
        public string ReadyComments1to2YearsByReviewer1 { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer1 3to5years Comments can not be greater than 1000 characters.")]
        public string ReadyComments3to5YearsByReviewer1 { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer2 1to2years Comments can not be greater than 1000 characters.")]
        public string ReadyComments1to2YearsByReviewer2 { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer2 3to5years Comments can not be greater than 1000 characters.")]
        public string ReadyComments3to5YearsByReviewer2 { get; set; }

        public UserInRole UserInRole { get; set; }
        public TimeFrameID TimeFrameID { get; set; }

        [StringLength(2000, ErrorMessage = "Reviewer1 OverallRatingComments can not be greater than 2000 characters.")]
        public string Reviewer1OverallRatingComments { get; set; }

        [StringLength(2000, ErrorMessage = "Reviewer2 OverallRatingComments can not be greater than 2000 characters.")]
        public string Reviewer2OverallRatingComments { get; set; }

        [StringLength(2000, ErrorMessage = "GroupHead OverallRatingComments can not be greater than 2000 characters.")]
        public string GroupHeadOverallRatingComments { get; set; }

        public string IsViewDetails { get; set; }
        public UpraisalRating UprasaisaRating { get; set; }
        public EmployeeMailTemplate Mail { get; set; }
        public List<int> RatingsList { get; set; }
    }

    public class UpraisalRating
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }

    public class Designation
    {
        public int DesignationID { get; set; }
        public string DesignationDesc { get; set; }
    }

    public enum UserInRole
    {
        Reviewer1,
        Reviewer2,
        GroupHead,
        Appraiser1,
        Employee,
        HRAdmin
    }

    public enum TimeFrameID
    {
        ReadyNow = 1,
        OneToTwoYears,
        ThreeToFiveYears
    }

    public class Parameters
    {
        public int EmployeeID { get; set; }
        public int? AppraisalID { get; set; }
        public int parmID { get; set; }
        public string ParameterDesc { get; set; }
        public int? Reviewer1Ratings { get; set; }
        public string Reviewer1Comments { get; set; }
        public int? Reviewer2Raitings { get; set; }
        public string Reviewer2Comments { get; set; }

        public int? GroupHeadRaitings { get; set; }
        public string GroupHeadComments { get; set; }

        public UserInRole UserInRole { get; set; }
        public int? Reviewer1OverallRating { get; set; }
        public int? Reviewer2OverallRating { get; set; }

        public int? GroupHeadOverallRating { get; set; }

        public int DesignationID { get; set; }
        public int NextDesignationIDFromReviewer1 { get; set; }
        public int NextDesignationIDFromReviewer2 { get; set; }
        public int NextDesignationIDFromGroupHead { get; set; }

        public bool? Reviewer1Recomendation { get; set; }
        public bool? Reviewer2Recomendation { get; set; }
        public bool? GroupHeadRecomendation { get; set; }

        public string Reviewer1RecomendationComments { get; set; }
        public string Reviewer2RecomendationComments { get; set; }
        public string GroupHeadRecomendationComments { get; set; }
        public string Reviewer1OverallRatingComments { get; set; }
        public string Reviewer2OverallRatingComments { get; set; }
        public string GroupHeadOverallRatingComments { get; set; }
    }

    public class AppraisalEmployeeGrowthSummary
    {
        public int? EmpGrowthID { get; set; }
        public int EmployeeID { get; set; }
        public int AppraisalID { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer1 ReadyNow Comments can not be greater than 1000 characters.")]
        public string Reviewer1CommentsReadyNow { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer2 ReadyNow Comments can not be greater than 1000 characters.")]
        public string Reviewer2CommentsReadyNow { get; set; }

        [StringLength(1000, ErrorMessage = "GroupHead ReadyNow Comments can not be greater than 1000 characters.")]
        public string GroupHeadCommentsReadyNow { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer1 1to2years Comments can not be greater than 1000 characters.")]
        public string Reviewer1Comments1to2years { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer2 1to2years Comments can not be greater than 1000 characters.")]
        public string Reviewer2Comments1to2years { get; set; }

        [StringLength(1000, ErrorMessage = "GroupHead 1to2years Comments can not be greater than 1000 characters.")]
        public string GroupHeadComments1to2years { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer1 3to5years Comments can not be greater than 1000 characters.")]
        public string Reviewer1Comments3to5years { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer2 3to5years Comments can not be greater than 1000 characters.")]
        public string Reviewer2Comments3to5years { get; set; }

        [StringLength(1000, ErrorMessage = "GroupHead 3to5years Comments can not be greater than 1000 characters.")]
        public string GroupHeadComments3to5years { get; set; }

        public string Reviewer1Name { get; set; }
        public string Reviewer2Name { get; set; }
        public string GroupHeadName { get; set; }

        public string IsManagerOrEmployee { get; set; }

        public UserInRole UserInRole { get; set; }
    }
}