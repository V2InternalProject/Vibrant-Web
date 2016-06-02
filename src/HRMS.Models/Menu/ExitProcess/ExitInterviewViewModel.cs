using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ExitInterviewViewModel
    {
        public ExitProcessViewModel SeparationFormDetails { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }

        public EmployeeDetailsViewModel EmployeeDetails { get; set; }

        public int? EmployeeId { get; set; }

        public int HiddenRadioId { get; set; }

        public int? HiddenStageId { get; set; }

        [Required(ErrorMessage = "HR Comments are required")]
        public string HRClosureComments { get; set; }

        public List<ExitInterview> ListExitInterviewItems { get; set; }
    }

    public class ExitInterview
    {
        public int QuestionnaireId { get; set; }

        public string FormName { get; set; }

        public int? RevisionId { get; set; }

        public int QuestionnaireCategoryId { get; set; }

        public string SectionName { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public decimal? AnswerSetId { get; set; }

        public int? ResponseId { get; set; }

        public DateTime? ResponseDate { get; set; }

        public string Comments { get; set; }

        [Required(ErrorMessage = "HR Comments are required")]
        public string HRClosureComments { get; set; }

        public int TotalAnswer { get; set; }

        public int? ExitInstanceId { get; set; }

        public List<QuestionaryOption> OptionList { get; set; }
    }

    public class QuestionaryOption
    {
        public int? QuestionaryOptionId { get; set; }

        public string OptionDescription { get; set; }
    }
}