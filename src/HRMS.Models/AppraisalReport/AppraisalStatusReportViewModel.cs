using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AppraisalStatusReportViewModel
    {
        public int AppraisalID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Employeecode { get; set; }
        public int AppraisalYearID { get; set; }
        public string AppraisalYear { get; set; }
        public int? AppraisalStageID { get; set; }
        public string AppraisalStageDesc { get; set; }
        public int? Appraiser1ID { get; set; }
        public string Appraiser1Name { get; set; }
        public int? Appraiser2ID { get; set; }
        public string Appraiser2Name { get; set; }
        public int? Reviewer1ID { get; set; }
        public string Reviewer1Name { get; set; }
        public int? Reviewer2ID { get; set; }
        public string Reviewer2Name { get; set; }
        public int? GroupHeadID { get; set; }
        public string GroupHeadName { get; set; }
        public int DeliveryTeamID { get; set; }
        public string DeliveryTeamName { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yy}")]
        public DateTime? ConfirmationDate { get; set; }

        public int? DesignationID { get; set; }
        public string DesignationName { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yy}")]
        public DateTime? JoiningDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yy}")]
        public DateTime? ProbationReviewDate { get; set; }

        public string ParameterName { get; set; }

        public int? ParameterId { get; set; }

        public string ParentDu { get; set; }
        public string CurrentDu { get; set; }
        public int? Appraiser1Rating { get; set; }
        public string Appraiser1Comments { get; set; }
        public int? Appraiser2Rating { get; set; }
        public string Appraiser2Comments { get; set; }
        public int? Reviewer1Rating { get; set; }
        public string Reviewer1Comments { get; set; }
        public int? Reviewer2Rating { get; set; }
        public string Reviewer2Comments { get; set; }
        public int? GroupHeadRating { get; set; }
        public string GroupHeadComments { get; set; }

        public string EmployeeEmail { get; set; }
        public string Appraiser1Email { get; set; }
        public string Appraiser2Email { get; set; }
        public string Reviewer1Email { get; set; }
        public string Reviewer2Email { get; set; }
        public string GroupHeadEmail { get; set; }

        public int? Reviewer1OverAllRating { get; set; }

        public int? Reviewer2OverAllRating { get; set; }

        public int? GroupHeadOverAllRating { get; set; }

        public string Reviewer1OverAllComment { get; set; }

        public string Reviewer2OverAllComment { get; set; }

        public string GroupHeadOverAllComment { get; set; }

        public List<int?> RatingsForAppraiser1 { get; set; }

        public List<int?> RatingsForAppraiser2 { get; set; }

        public List<int?> RatingsForReviewer1 { get; set; }

        public List<int?> RatingsForReviewer2 { get; set; }

        public List<int?> RatingsForGroupHead { get; set; }

        public List<string> CommentsForAppraiser1 { get; set; }

        public List<string> CommentsForAppraiser2 { get; set; }

        public List<string> CommentsForReviewer1 { get; set; }

        public List<string> CommentsForReviewer2 { get; set; }

        public List<string> CommentsForGroupHead { get; set; }

        public int? ratingOneAppraiserOne { get; set; }

        public int? ratingTwoAppraiserOne { get; set; }

        public int? ratingThreeAppraiserOne { get; set; }

        public int? ratingFourAppraiserOne { get; set; }

        public int? ratingFiveAppraiserOne { get; set; }

        public int? ratingSixAppraiserOne { get; set; }

        public int? ratingOneAppraiserTwo { get; set; }

        public int? ratingTwoAppraiserTwo { get; set; }

        public int? ratingThreeAppraiserTwo { get; set; }

        public int? ratingFourAppraiserTwo { get; set; }

        public int? ratingFiveAppraiserTwo { get; set; }

        public int? ratingSixAppraiserTwo { get; set; }

        public int? ratingOneReviewerOne { get; set; }

        public int? ratingTwoReviewerOne { get; set; }

        public int? ratingThreeReviewerOne { get; set; }

        public int? ratingFourReviewerOne { get; set; }

        public int? ratingFiveReviewerOne { get; set; }

        public int? ratingSixReviewerOne { get; set; }

        public int? ratingOneReviewerTwo { get; set; }

        public int? ratingTwoReviewerTwo { get; set; }

        public int? ratingThreeReviewerTwo { get; set; }

        public int? ratingFourReviewerTwo { get; set; }

        public int? ratingFiveReviewerTwo { get; set; }

        public int? ratingSixReviewerTwo { get; set; }

        public int? ratingOneGroupHead { get; set; }

        public int? ratingTwoGroupHead { get; set; }

        public int? ratingThreeGroupHead { get; set; }

        public int? ratingFourGroupHead { get; set; }

        public int? ratingFiveGroupHead { get; set; }

        public int? ratingSixGroupHead { get; set; }

        //for comments

        public string CommentOneAppraiserOne { get; set; }

        public string CommentTwoAppraiserOne { get; set; }

        public string CommentThreeAppraiserOne { get; set; }

        public string CommentFourAppraiserOne { get; set; }

        public string CommentFiveAppraiserOne { get; set; }

        public string CommentSixAppraiserOne { get; set; }

        public string CommentOneAppraiserTwo { get; set; }

        public string CommentTwoAppraiserTwo { get; set; }

        public string CommentThreeAppraiserTwo { get; set; }

        public string CommentFourAppraiserTwo { get; set; }

        public string CommentFiveAppraiserTwo { get; set; }

        public string CommentSixAppraiserTwo { get; set; }

        public string CommentOneReviewerOne { get; set; }

        public string CommentTwoReviewerOne { get; set; }

        public string CommentThreeReviewerOne { get; set; }

        public string CommentFourReviewerOne { get; set; }

        public string CommentFiveReviewerOne { get; set; }

        public string CommentSixReviewerOne { get; set; }

        public string CommentOneReviewerTwo { get; set; }

        public string CommentTwoReviewerTwo { get; set; }

        public string CommentThreeReviewerTwo { get; set; }

        public string CommentFourReviewerTwo { get; set; }

        public string CommentFiveReviewerTwo { get; set; }

        public string CommentSixReviewerTwo { get; set; }

        public string CommentOneGroupHead { get; set; }

        public string CommentTwoGroupHead { get; set; }

        public string CommentThreeGroupHead { get; set; }

        public string CommentFourGroupHead { get; set; }

        public string CommentFiveGroupHead { get; set; }

        public string CommentSixGroupHead { get; set; }

        public string PromotionRecommentationReviewer1 { get; set; }

        public string PromotionRecommentationReviewer2 { get; set; }

        public string PromotionRecommentationGroupHead { get; set; }

        public string NextDesignationReviewer1 { get; set; }

        public string NextDesignationReviewer2 { get; set; }

        public string NextDesignationGroupHead { get; set; }
    }

    public class PrintAppraisalStatusReport
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int EmployeeID { get; set; }
        public string DeliveryTeam { get; set; }
        public string Designation { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yy}")]
        public DateTime? ConfirmationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yy}")]
        public DateTime? ProbationDate { get; set; }

        public string Appraiser1 { get; set; }
        public string Appraiser2 { get; set; }
        public string Reviewer1 { get; set; }
        public string Reviewer2 { get; set; }
        public string GroupHead { get; set; }
        public string Stage { get; set; }

        public string EmployeeEmail { get; set; }
        public string Appraiser1Email { get; set; }
        public string Appraiser2Email { get; set; }
        public string Reviewer1Email { get; set; }
        public string Reviewer2Email { get; set; }
        public string GroupHeadEmail { get; set; }
    }
}