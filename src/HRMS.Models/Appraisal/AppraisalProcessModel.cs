using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AppraisalProcessModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }

        //confirmation code
        public int? EmployeeID { get; set; }

        public int? CorporateId { get; set; }

        //public int? CorporateEmployeeID { get; set; }
        public int appraisalId { get; set; }

        public int SrNo { get; set; }

        public EmployeeMailTemplate Mail { get; set; }
        public string Employeename { get; set; }
        public int? Appriser1Id { get; set; }
        public int? Appriser2Id { get; set; }
        public int? Reviwer1Id { get; set; }
        public int? Reviwer2Id { get; set; }
        public int? GroupHeadId { get; set; }
        public bool IsPerformanceYearFrozen { get; set; }
        public bool IsIDFFrozen { get; set; }
        public bool IsUnfreezedByAdmin { get; set; }
        public string NoReviewer2 { get; set; }
        public string NoAppraiser2 { get; set; }
        public string LinkClicked { get; set; }
        public int? FromStageID { get; set; }
        public int? ToStageID { get; set; }

        public List<ViewGroupHeadHistoryModel> GroupHeadHistoryDetailsList { get; set; }

        [StringLength(1000, ErrorMessage = "Area Of Contribution can not be greater than 1000 characters.")]
        public string AreaOfContribution { get; set; }

        [StringLength(1000, ErrorMessage = "Contribution Description can not be greater than 1000 characters.")]
        public string ContributionDesc { get; set; }

        [StringLength(1000, ErrorMessage = "Appraiser 1 Comments can not be greater than 1000 characters.")]
        public string Appraiser1Comments { get; set; }

        [StringLength(1000, ErrorMessage = "Appraiser 2 Comments can not be greater than 1000 characters.")]
        public string Appraiser2Comments { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer 1 Comments can not be greater than 1000 characters.")]
        public string Reviewer1Comments { get; set; }

        [StringLength(1000, ErrorMessage = "Reviewer 2 Comments can not be greater than 1000 characters.")]
        public string Reviewer2Comments { get; set; }

        [StringLength(1000, ErrorMessage = "Group Head Comments can not be greater than 1000 characters.")]
        public string GrpHeadComments { get; set; }

        public string Appraiser1Name { get; set; }
        public string Appraiser2Name { get; set; }
        public string Reviewer1Name { get; set; }
        public string Reviewer2Name { get; set; }
        public string GroupHeadName { get; set; }
        public string UserRole { get; set; }
        public int? StageID { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DateTime ExtendProbationDate { get; set; }
        public DateTime PIPDate { get; set; }
        public string ProbationTemplate { get; set; }
        public string ConfirmationTemplate { get; set; }
        public List<EmployeementTypeList> EmployeeType { get; set; }
        public List<EmployeeStatusList> EmployeeStatus { get; set; }
        public List<GradeList> Grade { get; set; }
        public string gradeName { get; set; }
        public string roleName { get; set; }
        public string empStatus { get; set; }
        public string empType { get; set; }
        public List<EmpRole> Role { get; set; }
        public string ConfirmationComments { get; set; }
        public string ProbationComments { get; set; }
        public string PIPComments { get; set; }
        public string IsAcceptedOrExtended { get; set; }
        public List<AppraisalProcessModel> CorporateDetailsList { get; set; }
        public List<AppraisalProcessModel> CorporateContributionList { get; set; }
        public List<PerformanceHinder> PerformanceHinderList { get; set; }
        public PerformanceHinder NewPerformanceHinderDetail { get; set; }
        public PerformanceHinderAppraisal PerfHinderListAppraisal { get; set; }
        public List<CertificationDetails> certifications { get; set; }

        // public PerformanceHinderAppraisal PerfHinderAppraisal { get; set; }
        public ProjectAchievement projAchievement { get; set; }

        public List<ProjectAchievement> projAchievementList { get; set; }
        public SkillsAquiredAppraisal skillAquiredAppraisal { get; set; }
        public AdditionalQualificationAppraisal additionalQualificationAppraisal { get; set; }
        public List<AppraisalParameter> AppraisalParameterList { get; set; }
        public ConfirmationParameter confirmationParameter { get; set; }
        public GoalAquireAppraisal goalAquireAppraisal { get; set; }
        public string IsManagerOrEmployee { get; set; }
        public string ViewButtonClicked { get; set; }
        public List<MailTemplateViewModel> MailDetailsList { get; set; }
        public RatingApprMinMax rating { get; set; }
        public MailTemplateViewModel MailDetail { get; set; }
        public ProjectAchievementAppraisal projAchievementtAppraisal { get; set; }
        public List<ProjectAchievementAppraisal> projAchievementtAppraisalList { get; set; }
        public ApraiserName AppraiserName { get; set; }
        public string IsViewDetails { get; set; }

        public List<GuideLines> GuideLinesList { get; set; }
    }

    public class ApraiserName
    {
        public string EmployeeName { get; set; }
        public string Appraiser1 { get; set; }
        public string Appraiser2 { get; set; }
        public string Reviewer1 { get; set; }
        public string Reviewer2 { get; set; }
        public string GroupHead { get; set; }
    }

    public class ProjectAchievementAppraisal
    {
        public int? ProjAchvmntEmpID { get; set; }
        public int? AppraisalID { get; set; }
        public int? ProjAchieveID { get; set; }

        [StringLength(1000, ErrorMessage = "Project Description/ Assignments Handled can not be greater than 1000 characters.")]
        public string ProjectDesc { get; set; }

        [StringLength(1000, ErrorMessage = "Achievements on the project if any can not be greater than 1000 characters.")]
        public string ProjectAchievements { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [StringLength(1000, ErrorMessage = "Name of the Project Manager can not be greater than 1000 characters.")]
        public string NameOfManager { get; set; }

        public string IsManagerOrEmployee { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }
        public List<ProjectAchievementAppraisal> projectAchievementAppraisalList { get; set; }
    }

    public class PerformanceHinderAppraisal
    {
        public int? EmpID { get; set; }
        public int? AppraisalID { get; set; }
        public int? PerfHinderID { get; set; }

        //[StringLength(1000, ErrorMessage = "Facilitating Factors (Environment) can not be greater than 1000 characters.")]
        public string EmployeeCommentsFFEnvi { get; set; }

        //[StringLength(1000, ErrorMessage = "Facilitating Factors (Self) can not be greater than 1000 characters.")]
        public string EmployeeCommentsFFSelf { get; set; }

        //[StringLength(1000, ErrorMessage = "Inhibiting Factors (Self Related) can not be greater than 1000 characters.")]
        public string EmployeeCommentsIFSelf { get; set; }

        //[StringLength(1000, ErrorMessage = "Inhibiting Factors (Environment Related) can not be greater than 1000 characters.")]
        public string EmployeeCommentsIFEnvi { get; set; }

        //[StringLength(1000, ErrorMessage = "Support Expected/ Required from Organization in Future can not be greater than 1000 characters.")]
        public string EmployeeCommentsSupport { get; set; }

        //[StringLength(1000, ErrorMessage = "Facilitating Factors (Environment) can not be greater than 1000 characters.")]
        public string Appraiser1CommentsFFEnvi { get; set; }

        //[StringLength(1000, ErrorMessage = "Inhibiting Factors (Environment Related) can not be greater than 1000 characters.")]
        public string Appraiser1CommentsIFEnvi { get; set; }

        //[StringLength(1000, ErrorMessage = "Inhibiting Factors (Self Related) can not be greater than 1000 characters.")]
        public string Appraiser1CommentsIFSelf { get; set; }

        //[StringLength(1000, ErrorMessage = "Facilitating Factors (Self) can not be greater than 1000 characters.")]
        public string Appraiser1CommentsFFSelf { get; set; }

        //[StringLength(1000, ErrorMessage = "Support Expected/ Required from Organization in Future can not be greater than 1000 characters.")]
        public string Appraiser1CommentsSupport { get; set; }

        //[StringLength(1000, ErrorMessage = "Facilitating Factors (Environment) can not be greater than 1000 characters.")]
        public string Appraiser2CommentsFFEnvi { get; set; }

        //[StringLength(1000, ErrorMessage = "Inhibiting Factors (Environment Related) can not be greater than 1000 characters.")]
        public string Appraiser2CommentsIFEnvi { get; set; }

        //[StringLength(1000, ErrorMessage = "Inhibiting Factors (Self Related) can not be greater than 1000 characters.")]
        public string Appraiser2CommentsIFSelf { get; set; }

        //[StringLength(1000, ErrorMessage = "Facilitating Factors (Self) can not be greater than 1000 characters.")]
        public string Appraiser2CommentsFFSelf { get; set; }

        //[StringLength(1000, ErrorMessage = "Support Expected/ Required from Organization in Future can not be greater than 1000 characters.")]
        public string Appraiser2CommentsSupport { get; set; }

        //[StringLength(1000, ErrorMessage = "Facilitating Factors (Environment) can not be greater than 1000 characters.")]
        public string Reviewer1CommentsFFEnvi { get; set; }

        //[StringLength(1000, ErrorMessage = "Inhibiting Factors (Environment Related) can not be greater than 1000 characters.")]
        public string Reviewer1CommentsIFEnvi { get; set; }

        //[StringLength(1000, ErrorMessage = "Inhibiting Factors (Self Related) can not be greater than 1000 characters.")]
        public string Reviewer1CommentsIFSelf { get; set; }

        //[StringLength(1000, ErrorMessage = "Facilitating Factors (Self) can not be greater than 1000 characters.")]
        public string Reviewer1CommentsFFSelf { get; set; }

        //[StringLength(1000, ErrorMessage = "Support Expected/ Required from Organization in Future can not be greater than 1000 characters.")]
        public string Reviewer1CommentsSupport { get; set; }

        //[StringLength(1000, ErrorMessage = "Facilitating Factors (Environment) can not be greater than 1000 characters.")]
        public string Reviewer2CommentsFFEnvi { get; set; }

        //[StringLength(1000, ErrorMessage = "Inhibiting Factors (Environment Related) can not be greater than 1000 characters.")]
        public string Reviewer2CommentsIFEnvi { get; set; }

        //[StringLength(1000, ErrorMessage = "Inhibiting Factors (Self Related) can not be greater than 1000 characters.")]
        public string Reviewer2CommentsIFSelf { get; set; }

        //[StringLength(1000, ErrorMessage = "Facilitating Factors (Self) can not be greater than 1000 characters.")]
        public string Reviewer2CommentsFFSelf { get; set; }

        //[StringLength(1000, ErrorMessage = "Support Expected/ Required from Organization in Future can not be greater than 1000 characters.")]
        public string Reviewer2CommentsSupport { get; set; }

        //[StringLength(1000, ErrorMessage = "Facilitating Factors (Environment) can not be greater than 1000 characters.")]
        public string GroupHeadCommentsFFEnvi { get; set; }

        //[StringLength(1000, ErrorMessage = "Inhibiting Factors (Environment Related) can not be greater than 1000 characters.")]
        public string GroupHeadCommentsIFEnvi { get; set; }

        //[StringLength(1000, ErrorMessage = "Inhibiting Factors (Self Related) can not be greater than 1000 characters.")]
        public string GroupHeadCommentsIFSelf { get; set; }

        //[StringLength(1000, ErrorMessage = "Facilitating Factors (Self) can not be greater than 1000 characters.")]
        public string GroupHeadCommentsFFSelf { get; set; }

        //[StringLength(1000, ErrorMessage = "Support Expected/ Required from Organization in Future can not be greater than 1000 characters.")]
        public string GroupHeadCommentsSupport { get; set; }

        public string EmployeeName { get; set; }
        public string Appraisal1Name { get; set; }
        public string Appraisal2Name { get; set; }
        public string Reviewer1Name { get; set; }
        public string Reviewer2Name { get; set; }
        public string GroupHeadName { get; set; }

        public string IsManagerOrEmployee { get; set; }
    }

    public class GoalAquireAppraisal
    {
        public int? EmployeIDGoal { get; set; }
        public int? GoalID { get; set; }
        public int AppraisalIDGoal { get; set; }

        public string LongTerm { get; set; }
        public string ShortTerm { get; set; }
        public string SkillDevPrgm { get; set; }

        public string IsManagerOrEmployee { get; set; }
    }

    public class SkillsAquiredAppraisal
    {
        public int? SkillEmployeeID { get; set; }
        public int? AppraisalID { get; set; }
        public int? SkillsAquiredID { get; set; }

        [StringLength(100, ErrorMessage = "Skill Name can not be greater than 100 characters.")]
        public string SkillName { get; set; }

        [StringLength(1000, ErrorMessage = "Acquired Through can not be greater than 1000 characters.")]
        public string AquiredThrough { get; set; }

        [StringLength(1000, ErrorMessage = "Usefulness to the project can not be greater than 1000 characters.")]
        public string ProjectUsefulness { get; set; }

        public string IsManagerOrEmployee { get; set; }
        public List<SkillsAquiredAppraisal> skillsAquiredList { get; set; }
    }

    public class AdditionalQualificationAppraisal
    {
        public int? QualifEmployeeID { get; set; }
        public int? AppraisalID { get; set; }
        public int? AddQualificationID { get; set; }

        //public int? QualificationID { get; set; }

        [StringLength(100, ErrorMessage = "Title can not be greater than 100 characters.")]
        public string Title { get; set; }

        public DateTime? FromDuration { get; set; }
        public DateTime? ToDuration { get; set; }

        // to create DDL of skills
        public List<SkillTypeList> Type { get; set; }

        public int? skill { get; set; }
        public string typeName { get; set; }
        //[StringLength(100, ErrorMessage = "Skills Acquired can not be greater than 100 characters.")]
        //public string AddSkillAquired { get; set; }

        //[StringLength(1000, ErrorMessage = "Extent to which the skill was used can not be greater than 1000 characters.")]
        //public string AddSkillUsed { get; set; }

        public string IsManagerOrEmployee { get; set; }
        public List<AddQualificationListClassAppraisal> QualificationList { get; set; }
        public List<AdditionalQualificationAppraisal> additionalQualificationAppraisalList { get; set; }
    }

    public class AddQualificationListClassAppraisal
    {
        public int AddQualificationID { get; set; }
        public string AddQualification { get; set; }
    }

    public class RatingApprMinMax
    {
        public int min { get; set; }
        public int max { get; set; }
    }

    public class AppraisalParameter
    {
        public int employeeID { get; set; }
        public string emplyeeName { get; set; }
        public int appraisalID { get; set; }

        //public int competencyID { get; set; }

        public int? parameterID { get; set; }
        public string EmpComments { get; set; }
        public int? SelfRating { get; set; }
        public string AppraiserComments1 { get; set; }
        public int? AppraiserRating1 { get; set; }
        public string AppraiserComments2 { get; set; }
        public int? AppraiserRating2 { get; set; }
        public string ReviewerComments1 { get; set; }
        public int? ReviewerRating1 { get; set; }
        public string ReviewerComments2 { get; set; }
        public int? ReviewerRating2 { get; set; }
        public string GrpHeadComments { get; set; }
        public int? GrpHeadRating { get; set; }
        public string ParameterDescription { get; set; }
        public string Parameter { get; set; }
        public int? OverallReviewRating { get; set; }
        public string OverallReviewRatingComments { get; set; }
        public int? OverallReview2Rating { get; set; }
        public string OverallReview2RatingComments { get; set; }
        public int? OverallGrpHeadRating { get; set; }
        public string OverallGrpHeadComments { get; set; }

        public string App1Name { get; set; }
        public string App2Name { get; set; }
        public string Rev1Name { get; set; }
        public string Rev2Name { get; set; }
        public string GrpHeadName { get; set; }

        public string IsManagerOrEmployee { get; set; }
        public int LoggedInEmployeeId { get; set; }
        public int StageID { get; set; }
        public bool IsIDFFrozen { get; set; }
        public bool isUnfreezedByAdmin { get; set; }
    }

    public class ViewGroupHeadHistoryModel
    {
        public int AppraisalID { get; set; }
        public int? OldOverallGroupHeadRating { get; set; }
        public int? NewOverallGroupHeadRating { get; set; }
        public string OldOverallGroupHeadComments { get; set; }
        public string NewOverallGroupHeadComments { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class GuideLines
    {
        public decimal? Percentage { get; set; }

        public string Rating { get; set; }

        public string Description { get; set; }
    }
}