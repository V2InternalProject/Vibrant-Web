using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class AppraisalProcessFourModel
    {
        [Required(ErrorMessage = "Appraisee Comments required")]
        [StringLength(1000, ErrorMessage = "Appraisee Comments can not be greater than 1000 characters.")]
        public string AppraiseeComments { get; set; }

        //public string ImprovementCommentsIDF { get; set; }

        //public string StrengthCommentsIDF { get; set; }

        //public bool IsImprovementStrengthVisible { get; set; }

        public int AppraisalId { get; set; }

        public int AppraisalYearId { get; set; }

        public int EmployeeId { get; set; }

        public bool? IsAgree { get; set; }

        public bool IsAppraiser1 { get; set; }

        public bool IsEmployee { get; set; }

        public int StageId { get; set; }

        public string IsViewDetails { get; set; }

        public UserInRole UserInRole { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }

        public AppraiseeStrengths Strengths { get; set; }

        public AppraiseeImprovements Improvements { get; set; }

        public TrainingProgram TrainingProgram { get; set; }

        public List<Category> CatrgoryList { get; set; }

        public Category Category { get; set; }

        public List<TrainingProgram> TrainingProgramList { get; set; }

        public EmployeeMailTemplate Mail { get; set; }
    }

    public class AppraisalMasterDetails
    {
        public int AppraisalId { get; set; }

        public int EmployeeId { get; set; }

        public int AppraisalYearId { get; set; }

        public int? ReviewerRating { get; set; }

        public string ParameterName { get; set; }
    }

    public class AppraiseeStrengths
    {
        public int StrengthId { get; set; }

        public int AppraisalId { get; set; }

        public int EmployeeId { get; set; }

        public int AppraisalYearId { get; set; }

        [Required]
        public string Strength { get; set; }

        [Required]
        public string StrengthComments { get; set; }

        public List<AppraiseeStrengths> AppraiseeStrengthsList { get; set; }
    }

    public class AppraiseeImprovements
    {
        public int ImprovementId { get; set; }

        public int AppraisalId { get; set; }

        public int EmployeeId { get; set; }

        public int AppraisalYearId { get; set; }

        [Required]
        public string Improvement { get; set; }

        [Required]
        public string ImprovementComments { get; set; }

        public List<AppraiseeImprovements> AppraiseeImprovementsList { get; set; }
    }

    public class TrainingProgram
    {
        public int ProgramId { get; set; }

        public int AppraisalId { get; set; }

        public int EmployeeId { get; set; }

        public int AppraisalYearId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Category field is required.")]
        public int CategoryId { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Reason { get; set; }

        public List<TrainingProgram> TrainingProgramList { get; set; }

        public List<Category> CatrgoryList { get; set; }

        public Category Categories { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}