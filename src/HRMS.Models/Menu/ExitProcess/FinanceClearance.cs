using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class FinanceClearance
    {
        public ExitProcessViewModel SeparationFormDetails { get; set; }

        public int? EmployeeId { get; set; }

        public string EmployeeCode { get; set; }

        public int? UserId { get; set; }

        public int? FinanceRevisionID { get; set; }

        public int? ITRevisionID { get; set; }

        public int? AssetRevisionID { get; set; }

        public int? HRRevisionID { get; set; }

        public int? AdminRevisionID { get; set; }

        public int? ProjectRevisionID { get; set; }

        public int? FinanceQuestionnaireID { get; set; }

        public int? ITQuestionnaireID { get; set; }

        public int? HRQuestionnaireID { get; set; }

        public int? AdminQuestionnaireID { get; set; }

        public int? AssetQuestionnaireID { get; set; }

        public int? ProjectQuestionnaireID { get; set; }

        public int? QuestionnaireID { get; set; }

        public string UserRole { get; set; }

        public int ExitInstanceId { get; set; }

        public string EmployeeName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? TentativeReleaseDate { get; set; }

        public string location { get; set; }

        public bool checkboxcommon { get; set; }

        public string Respondent { get; set; }

        public string QuestionDescription { get; set; }

        public double? wattage { get; set; }

        public string IsAdminStageCleared { get; set; }

        public string IsHRStageCleared { get; set; }

        public string IsITStageCleared { get; set; }

        public string IsFinanceStageCleared { get; set; }

        public string IsProjectStageCleared { get; set; }

        public string IsAssetStageCleared { get; set; }

        public List<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }

        public List<QuestionnaireOption> QuestionnaireOptions { get; set; }

        public List<ApproverList> Approvers { get; set; }

        public List<DepartmentResponce> ResponceList { get; set; }

        public SeparationMailTemplate Mail { get; set; }
    }

    public class QuestionnaireQuestion
    {
        public int QuestionnaireQuestionID { get; set; }

        public int? QuetionRevisionID { get; set; }

        public string QuestionDescription { get; set; }

        public double? wattage { get; set; }

        //  [Required]
        [StringLength(1000, ErrorMessage = "Maxium 1000 characters are allowed")]
        public string Comments { get; set; }
    }

    public class QuestionnaireOption
    {
        public decimal? QuestionnaireQuestionID { get; set; }
        public int? OrderInWhichToAppear { get; set; }
        public int QuestionnaireOptionID { get; set; }
        public string OptionDescription { get; set; }
    }

    public class ApproverList
    {
        public int? ApproverID { get; set; }
        public int? RevisionID { get; set; }
        public string ApproverName { get; set; }
        public int? QuestionnaireID { get; set; }
    }

    public class DepartmentResponce
    {
        public int? exitinstanceid { get; set; }

        public string ResponceComments { get; set; }

        public int? RevisionIDDepartment { get; set; }

        public int? checklistitem { get; set; }

        public int? checklistresponce { get; set; }

        public int? AdminRevisionID { get; set; }
    }
}