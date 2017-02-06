using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ConfirmationFormViewModel
    {
        public int? EmployeeID { get; set; }
        public int? CorporateId { get; set; }
        public int? CorporateEmployeeID { get; set; }
        public int confirmationID { get; set; }
        public int SrNo { get; set; }

        [StringLength(1000, ErrorMessage = "Area Of Contribution cannot be more than 1000 characters.")]
        public string AreaOfContribution { get; set; }

        [StringLength(1000, ErrorMessage = "Contribution Description cannot be more than 1000 characters.")]
        public string ContributionDesc { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string ManagerComments { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string ManagerCommentsSecond { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string ReviewerComments { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string HRReviewerComments { get; set; }  // dbField is SecondReviewerComments

        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmpRole { get; set; }
        public string ProbationReviewDate { get; set; }
        public string ReportingManagerName { get; set; }
        public string ManagerName { get; set; }
        public string ManagerNameSecond { get; set; }
        public string ReviewerName { get; set; }
        public string HRReviewerName { get; set; }
        public string UserRole { get; set; }
        public int StageID { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DateTime ExtendProbationDate { get; set; }

        public int NumberOfDaysProbation { get; set; }
        public int NumberOfDaysPIP { get; set; }

        public DateTime PIPDate { get; set; }
        public DateTime PIPstartDate { get; set; }
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
        public string ReportingManagerComments { get; set; }
        public string HRComments { get; set; }
        public string DUManagerComments { get; set; }
        public string ProbationComments { get; set; }
        public string PIPComments { get; set; }
        public string IsAcceptedOrExtended { get; set; }
        public List<ConfirmationFormViewModel> CorporateDetailsList { get; set; }
        public List<ConfirmationFormViewModel> CorporateContributionList { get; set; }
        public List<PerformanceHinder> PerformanceHinderList { get; set; }
        public PerformanceHinder NewPerformanceHinderDetail { get; set; }
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public PerformanceHinderTable PerfHinderListTable { get; set; }
        public PerformanceHinderTable PerfHinderTable { get; set; }
        public ProjectAchievement projAchievement { get; set; }
        public List<ProjectAchievement> projAchievementList { get; set; }
        public SkillsAquired skillAquired { get; set; }
        public AdditionalQualification additionalQualification { get; set; }
        public List<ConfirmationParameter> confParameterList { get; set; }
        public ConfirmationParameter confirmationParameter { get; set; }
        public GoalAquire goalAquire { get; set; }
        public string IsManagerOrEmployee { get; set; }
        public string ViewButtonClicked { get; set; }
        public List<MailTemplateViewModel> MailDetailsList { get; set; }
        public RatingMinMax rating { get; set; }
        public MailTemplateViewModel MailDetail { get; set; }

        //[StringLength(2000, ErrorMessage = "Area Of Contribution Comments can not be greater than 2000 characters.")]
        public string AreaOfContributionComments { get; set; }

        //[StringLength(2000, ErrorMessage = "Training Program Comments can not be greater than 2000 characters.")]
        public string TrainingProgramComments { get; set; }

        //[StringLength(2000, ErrorMessage = "Behaviour Comments can not be greater than 2000 characters.")]
        public string BehaviourComments { get; set; }

        public int QuestionId { get; set; }
        public int EmployeeIdConfirmation { get; set; }
        public int ApproverDetailId { get; set; }
        public bool IsFurtherApproverNeeded { get; set; }
        public string FurtherApproverName { get; set; }
        public int FurtherApproverId { get; set; }
        public bool IsFurtherApproverStageCleared { get; set; }
        public List<GuideLines> GuideLinesList { get; set; }
    }

    public class RatingMinMax
    {
        public int min { get; set; }
        public int max { get; set; }
    }

    public class PerformanceHinder
    {
        public int? PerformanceID { get; set; }
        public string PerformEmployeeComments { get; set; }
        public string PerformManagerComments { get; set; }
        public string PerformReviewerComments { get; set; }
        public string PerformHRComments { get; set; }
        public string IsManagerOrEmployee { get; set; }
    }

    public class PerformanceHinderTable
    {
        public int? empID { get; set; }
        public int? confID { get; set; }
        public int? perfHinderID { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string EmpCommentsFFSelf { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string MngrCommentsFFSelf { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string ReviewerCommentsFFSelf { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string HrCommentsFFSelf { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string EmpCommentsFFEnvi { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string MngrCommentsFFEnvi { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string ReviewerCommentsFFEnvi { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string HrCommentsFFEnvi { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string EmpCommentsIFSelf { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string MngrCommentsIFSelf { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string ReviewerCommentsIFSelf { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string HrCommentsIFSelf { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string EmpCommentsIFEnvi { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string MngrCommentsIFEnvi { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string ReviewerCommentsIFEnvi { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string HrCommentsIFEnvi { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string EmpCommentsSupport { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string MngrCommentsSupport { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string ReviewerCommentsSupport { get; set; }

        public string HrCommentsSupport { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string MngrCommentsFFSelfSecond { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string MngrCommentsFFEnviSecond { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string MngrCommentsIFSelfSecond { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string MngrCommentsIFEnviSecond { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot be more than 1000 characters.")]
        public string MngrCommentsSupportSecond { get; set; }

        public string MgrName { get; set; }
        public string MgrNameSecond { get; set; }
        public string RevName { get; set; }
        public string HRName { get; set; }

        public string IsManagerOrEmployee { get; set; }
    }

    public class ProjectAchievement
    {
        public int ProjectID { get; set; }
        public int? EmpID { get; set; }
        public int? ConfirmationID { get; set; }
        public int? ProjAchieveID { get; set; }

        [StringLength(1000, ErrorMessage = "Project Description cannot be more than 1000 characters.")]
        public string ProjectDesc { get; set; }

        [StringLength(1000, ErrorMessage = "Project achievements cannot be more than 1000 characters.")]
        public string ProjectAchievements { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [StringLength(100, ErrorMessage = "Project Manager name cannot be more than 100 characters.")]
        public string NameOfManager { get; set; }

        public string IsManagerOrEmployee { get; set; }
        public List<ProjectAchievement> projectAchievementList { get; set; }
        public string ProjectName { get; set; }
        public string SystemDate { get; set; }
        public string AllocationEndDate { get; set; }
        public string EmployeeCode { get; set; }
        public int ProjectEmployeeRoleID { get; set; }
        public int ProjectEndAppraisalStausID { get; set; }
    }

    public class SkillsAquired
    {
        public int? SkillEmployeeID { get; set; }
        public int? ConfirmationID { get; set; }
        public int? SkillsAquiredID { get; set; }

        [StringLength(100, ErrorMessage = "Skill Name name cannot be more than 100 characters.")]
        public string SkillName { get; set; }

        [StringLength(1000, ErrorMessage = "Aquired Through name cannot be more than 1000 characters.")]
        public string AquiredThrough { get; set; }

        [StringLength(1000, ErrorMessage = "Project Usefulness name cannot be more than 1000 characters.")]
        public string ProjectUsefulness { get; set; }

        public string IsManagerOrEmployee { get; set; }
        public List<SkillsAquired> skillsAquiredList { get; set; }
    }

    public class AdditionalQualification
    {
        public int? QualifEmployeeID { get; set; }
        public int? ConfirmationID { get; set; }
        public int? AddQualificationID { get; set; }

        [StringLength(100, ErrorMessage = "Title cannot be more than 100 characters.")]
        public string Title { get; set; }

        public DateTime? FromDuration { get; set; }
        public DateTime? ToDuration { get; set; }

        // to create DDL of skills
        public List<SkillTypeList> Type { get; set; }

        public string skill { get; set; }

        [StringLength(1000, ErrorMessage = "Skills Aquired cannot be more than 1000 characters.")]
        public string AddSkillAquired { get; set; }

        [StringLength(1000, ErrorMessage = "Skills Used cannot be more than 1000 characters.")]
        public string AddSkillUsed { get; set; }

        public string IsManagerOrEmployee { get; set; }
        public List<AddQualificationListClass> QualificationList { get; set; }
        public List<AdditionalQualification> additionalQualificationList { get; set; }
    }

    public class SkillTypeList
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class GoalAquire
    {
        public int? EmployeID { get; set; }
        public int? GoalID { get; set; }
        public int ConfirmID { get; set; }

        [StringLength(1000, ErrorMessage = "Long Term goals cannot be more than 1000 characters.")]
        public string LongTerm { get; set; }

        [StringLength(1000, ErrorMessage = "Short Term goals cannot be more than 1000 characters.")]
        public string ShortTerm { get; set; }

        [StringLength(1000, ErrorMessage = "Skill Development Programmes cannot be more than 1000 characters.")]
        public string SkillDevPrgm { get; set; }

        public string IsManagerOrEmployee { get; set; }
    }

    public class EmpRole
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class EmployeeStatusList
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class EmployeementTypeList
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class GradeList
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class AddQualificationListClass
    {
        public int AddQualificationID { get; set; }
        public string AddQualification { get; set; }
    }
}