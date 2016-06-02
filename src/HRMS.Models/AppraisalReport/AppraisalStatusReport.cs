using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class AppraisalStatusReport
    {
        //public string YearDesc { get; set; }
        public int YearID { get; set; }

        public int FreezeYearID { get; set; }
        public int IndividualDevelopmentYearID { get; set; }
        public int InitiateIndividualDevelopmentYearID { get; set; }
        public int FreezeIndividualDevelopmentYearID { get; set; }
        public int AppraisalRatingsYearID { get; set; }
        public int AppraisalYearFroozenBy { get; set; }
        public List<AppraisalYearReport> AppraisalYear { get; set; }
        public List<AppraisalYearReport> FreezeAppraisalYear { get; set; }
        public List<AppraisalYearReport> IndividualDevelopmentYear { get; set; }
        public List<AppraisalYearReport> InitiateIndividualDevelopmentYear { get; set; }
        public List<AppraisalYearReport> FreezeIndividualDevelopmentYear { get; set; }
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<AppraisalStatusReportViewModel> AppraisalReportViewModelList { get; set; }
        public List<IndividualDevelopmentStatusReport> IndividualDevelopmentStatusReportList { get; set; }
        public List<AppraisalStatusReportViewModel> AppraisalRatingsReportList { get; set; }
        public DateTime AppraisalYearFroozenOn { get; set; }
        public DateTime IndividualDevelopmentFrozenOn { get; set; }
        public DateTime IndividualDevelopmentInitiatedOn { get; set; }
        public List<FreezeAppraisalPeriod> FreezeAppraisalPeriodList { get; set; }
        public List<InitiateIndividualDevelopmentStage> InitiateIndividualDevelopmentStageList { get; set; }
        public List<FreezeIndividualDevelopmentStage> FreezeIndividualDevelopmentStageList { get; set; }
        public EmployeeMailTemplate Mail { get; set; }
        public List<ParametersForCurrentUear> ParameterList { get; set; }

        public List<AppraisalStatusReportViewModel> AppraisalStatusReportRatingAndComments { get; set; }
    }

    public class AppraisalYearReport
    {
        public int AppraisalYearID { get; set; }
        public string AppraisalYearDesc { get; set; }
    }

    public class ParametersForCurrentUear
    {
        public int ParameterId { get; set; }
        public string ParameterName { get; set; }
    }

    public class FreezeAppraisalPeriod
    {
        public string AppraisalYearDesc { get; set; }
        public string AppraisalYearFroozenByEmpName { get; set; }
        public DateTime? AppraisalYearFroozenOn { get; set; }
    }

    public class InitiateIndividualDevelopmentStage
    {
        public string InitiatedYear { get; set; }
        public string AppraisalInitiatedBy { get; set; }
        public DateTime? InitiatedOn { get; set; }
    }

    public class FreezeIndividualDevelopmentStage
    {
        public string FrozenYear { get; set; }
        public string FrozenBy { get; set; }
        public DateTime? FrozenOn { get; set; }
    }
}