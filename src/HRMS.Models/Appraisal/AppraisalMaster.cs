using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class EmployeeList<T>
    {
        public T Value { get; set; }
        public string Text { get; set; }
    }

    public class Menu
    {
        public int SectionID { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }
    }

    public class MenuItem
    {
        public List<Menu> MenuList { get; set; }
        public string errorMessage { get; set; }
    }

    public class AppraiseeList
    {
        public int EmployeeCode { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        // public int ProjectID { get; set; }
        public int App1id { get; set; }

        public int App2id { get; set; }
        public int Rv1ID { get; set; }
        public int RV2id { get; set; }
        public int GroupID { get; set; }
        public int GroupHeadID { get; set; }
        public int IDFId { get; set; }
        public int IDFEsc1 { get; set; }
        public int IDFEsc2 { get; set; }
        public int DU { get; set; }
        public int RPool { get; set; }
        public string DUName { get; set; }
        public string RPoolName { get; set; }
        public string FreezeComment { get; set; }
    }

    public class AppraisalMaster
    {
        public int AppraisalID { get; set; }
        public int EmployeeID { get; set; }
        public int AppraisalYearID { get; set; }
        public int AppraisalStageID { get; set; }
        public DateTime AppraisalInitiatedOn { get; set; }
        public int ApprisalCordinator { get; set; }
        public int Appraiser1 { get; set; }
        public int Appraiser2 { get; set; }
        public int Reviewer1 { get; set; }
        public int Reviewer2 { get; set; }
        public int GroupHead { get; set; }
        public string IDFAprraiseComment { get; set; }
        public bool IDFISAppraiseAgree { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public string CancelComment { get; set; }
        public bool IsCancelled { get; set; }
        public bool UnFreezedByAdmin { get; set; }
        public int DU { get; set; }
        public int RPool { get; set; }
        public string DUName { get; set; }
        public string RPoolName { get; set; }
    }

    public class AppraiseeDetails
    {
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public string Employeename { get; set; }
        public string EmployeeCode { get; set; }
        public string Appraiser1 { get; set; }
        public string Appraiser2 { get; set; }
        public string Reviewer1 { get; set; }
        public string Reviewer2 { get; set; }
        public string GroupHead { get; set; }
        public string DeliveryUnit { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }
        public string DateOfJoining { get; set; }
    }

    public class SearchAppraisal
    {
        public int? EmployeeID { get; set; }
        public int? AppraisalYearID { get; set; }
        public int? AppraisalStageID { get; set; }
        public int? Appraiser1 { get; set; }
        public int? Appraiser2 { get; set; }
        public int? Reviewer1 { get; set; }
        public int? Reviewer2 { get; set; }
        public int? GroupHead { get; set; }
        public int DU { get; set; }
        public int RPool { get; set; }
        public string DUName { get; set; }
        public string RPoolName { get; set; }
    }

    public class Section
    {
        public int? sectionId { get; set; }
        public string sectionName { get; set; }
        public int sectionType { get; set; }
        public DateTime lastUpdated { get; set; }
        public int lastUpdatedBy { get; set; }
    }

    public class Question
    {
        public int? questionId { get; set; }
        public string questionName { get; set; }
        public int questionType { get; set; }
        public DateTime lastUpdated { get; set; }
        public int? lastUpdatedBy { get; set; }
        public int? parentQuestionId { get; set; }
        public int seq { get; set; }
        public string dType { get; set; }
        public bool mandatory { get; set; }
        public int stageId { get; set; }
    }

    //public class Param
    //{ }

    //krishal

    public class SectionsList
    {
        public int sectionId { get; set; }
        public string sectionName { get; set; }
        public int sectionType { get; set; }
    }

    public class QuestionsList
    {
        public int questionId { get; set; }
        public string questionText { get; set; }
        public string dataType { get; set; }
        public string questionParam { get; set; }
        public string questionAbbr { get; set; }
        public string controlType { get; set; }
        public string validation { get; set; }
        public int? editStageId { get; set; }
    }

    public class yearLists
    {
        public int AppraisalYearID { get; set; }
        public string AppraisalYear { get; set; }
    }

    public class yearSectionsMapping
    {
        public int MappingId { get; set; }
        public int YearID { get; set; }
        public string YearName { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int Order { get; set; }
        public string MappingType { get; set; }
        public int isRequired { get; set; }
        public string Stages { get; set; }
    }

    public class Meppings
    {
        public int MappingId { get; set; }
    }

    public class finalMappings
    {
        public int MappingId { get; set; }
        public int QMappingId { get; set; }
        public int YearID { get; set; }
        public string YearName { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int Order { get; set; }
        public string MappingType { get; set; }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public int isRequired { get; set; }
        public string Stages { get; set; }
    }

    public class Employee
    {
        public int EmployeeId { get; set; }
        public int EmployeeCode { get; set; }
        public string EmployeeName { get; set; }

        //public string RelationAbbr { get; set; }
        public string RelationShip { get; set; }

        public string StageComplete { get; set; }
        public string IsLinkActive { get; set; }
        public string reviewlink { get; set; }
    }
}