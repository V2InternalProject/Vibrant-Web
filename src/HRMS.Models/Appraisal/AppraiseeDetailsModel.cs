using System.Collections.Generic;

namespace HRMS.Models
{
    public class AppraiseeDetailsModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        public List<AppraiserDetails> AppraiserDetailsList { get; set; }

        public List<ReviewerDetails> ReviewerDetailsList { get; set; }

        public List<GroupHeadDetails> GroupHeadDetailsList { get; set; }

        public List<AppraisalYearDetails> AppraisalYearList { get; set; }

        public int SelectedAppraisalYear { get; set; }
    }

    public class AppraisalYearDetails
    {
        public int AppraisalYearId { get; set; }

        public string CurrentAppraisalYear { get; set; }
    }

    public class AppraiserDetails
    {
        public string AppraiserEmployeeCode { get; set; }

        public string Appraiser { get; set; }

        public string AppraiserDeliveryTeam { get; set; }

        public string AppraiserDesignation { get; set; }

        public string AppraiserStage { get; set; }
    }

    public class ReviewerDetails
    {
        public string ReviewerEmployeeCode { get; set; }

        public string Reviewer { get; set; }

        public string ReviewerDeliveryTeam { get; set; }

        public string ReviewerDesignation { get; set; }

        public string ReviewerStage { get; set; }
    }

    public class GroupHeadDetails
    {
        public string GroupHeadEmployeeCode { get; set; }

        public string GroupHead { get; set; }

        public string GroupHeadDeliveryTeam { get; set; }

        public string GroupHeadDesignation { get; set; }

        public string GroupHeadStage { get; set; }
    }
}