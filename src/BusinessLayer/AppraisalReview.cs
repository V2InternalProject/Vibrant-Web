using System;
using System.Collections.Generic;
using System.Linq;
using HRMS.DAL;
using HRMS.Models;
using BOL;
using System.Data;
using BLL;
using MailActivity;

namespace V2.Orbit.BusinessLayer
{
    public class AppraisalReview
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appraisalSectionPost"></param>
        /// <returns></returns>
        public VibrantHttpResponse saveAppraisalSection(AppraisalSectionPost appraisalSectionPost)
        {
            var objApr = new Appraisal();
            try
            {
                var response = objApr.saveAppraisalSection(appraisalSectionPost);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public VibrantHttpResponse Submit(AppraisalSectionPost appraisalSectionPost)
        {
            var objApr = new Appraisal();
            try
            {
                var response = objApr.Submit(appraisalSectionPost);


                if (response.Message.ToLower().Contains("error"))
                    throw new Exception(response.Message);
                else
                    return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="appraisalSectionPost"></param>
        /// <returns></returns>
        public AppraisalSectionPost GetAppraisalData(AppraisalSectionPost appraisalSection)
        {
            var objApr = new Appraisal();

            try
            {
                return objApr.GetAppraisalData(appraisalSection);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        ///     Communicates to DAL to retrieve list of candidates list
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<ProbableList> GetAppraisalList()
        {
            var lstProbableApraisee = new List<ProbableList>();
            var objApr = new Appraisal();
            try
            {
                var AppraisalList = objApr.GetAppraisalList().ToList();

                ConvertApraiseeToProbable(lstProbableApraisee, AppraisalList);
                return lstProbableApraisee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public AppraiseeDetails GetAppraiseeDetails(int empID)
        {
            var lstProbableApraisee = new AppraiseeDetails();
            var objApr = new Appraisal();
            try
            {
                var AppraisalList = objApr.GetAppraiseeDetails(empID);


                return AppraisalList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Communicates to DAL to retrieve list of candidates list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProbableList> GetInitiationList()
        {
            var lstSaved = new List<ProbableList>();
            var objApr = new Appraisal();
            try
            {
                var SavedAppraisalList = objApr.GetSavedAppraisalList(null);

                ConvertApraiseeToProbable(lstSaved, SavedAppraisalList);
                return lstSaved;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Communicates to DAL to retrieve list of candidates list
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<ProbableList> GetInitiated()
        {
            var lstSaved = new List<ProbableList>();
            var objApr = new Appraisal();
            var oSearch = new SearchAppraisal();
            try
            {
                oSearch.AppraisalStageID = 1;
                var SavedAppraisalList = objApr.GetSavedAppraisalList(oSearch);

                ConvertApraiseeToProbable(lstSaved, SavedAppraisalList);
                return lstSaved;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Communicates to DAL to retrieve list of candidates list
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<ProbableList> GetFreezed()
        {
            var lstSaved = new List<ProbableList>();
            var objApr = new Appraisal();
            var oSearch = new SearchAppraisal();
            try
            {
                oSearch.AppraisalStageID = 99;
                var SavedAppraisalList = objApr.GetSavedAppraisalList(oSearch);

                ConvertApraiseeToProbable(lstSaved, SavedAppraisalList);
                return lstSaved;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Converts Appraisee into Probable candidate
        /// </summary>
        /// <param name="lstProbableApraisee"></param>
        /// <param name="AppraisalList"></param>
        private static void ConvertApraiseeToProbable(List<ProbableList> lstProbableApraisee,
            List<AppraiseeList> AppraisalList)
        {
            foreach (var itm in AppraisalList.ToList())
            {
                var oItem = new ProbableList();
                oItem.App1 = itm.App1id;
                oItem.App2 = itm.App2id;
                oItem.EC = itm.EmployeeCode;
                oItem.EID = itm.EmployeeID;
                oItem.EName = itm.EmployeeName;
                oItem.GHID = itm.GroupHeadID;
                oItem.GID = itm.GroupID;
                oItem.IDF = itm.IDFId;
                oItem.IDFE1 = itm.IDFEsc1;
                oItem.IDFE2 = itm.IDFEsc2;
                oItem.Rv1 = itm.Rv1ID;
                oItem.RV2 = itm.RV2id;
                oItem.DU = itm.DU;
                oItem.DUName = itm.DUName;
                oItem.RPool = itm.RPool;
                oItem.RPoolName = itm.RPoolName;

                lstProbableApraisee.Add(oItem);
            }
        }

        /// <summary>
        ///     Returns Active Employee list for appraisal hierarchy setup
        /// </summary>
        /// <returns></returns>
        public SetupList GetSetupList()
        {
            var objSetup = new SetupList();
            var setupItem = new List<DropDownList<int>>();

            try
            {
                var objApr = new Appraisal();

                var lstEmp = objApr.GetActiveEmployeeList();

                foreach (var itm in lstEmp)
                {
                    var oItem = new DropDownList<int>();

                    oItem.Value = itm.Value;
                    oItem.Text = itm.Text;

                    setupItem.Add(oItem);
                }

                objSetup.Appr1 = setupItem;
                objSetup.Appr2 = "Appr1";
                objSetup.GroupHeadID = "Appr1";
                objSetup.IDFEsc1 = "Appr1";
                objSetup.IDFEsc2 = "Appr1";
                objSetup.IDFId = "Appr1";
                objSetup.Revr1 = "Appr1";
                objSetup.Revr2 = "Appr1";
                //var a = lstEmp.Select(l => new DropDownList<int>() { l.Value, l.Text }).ToList();

                return objSetup;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MenuItem Menu(int empId, int loggedUser)
        {
            try
            {
                var objApr = new Appraisal();

                var lstMenu = objApr.GetMenuList(empId, loggedUser);
                return lstMenu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Communicates with DAL to Submit Candidates for Appraisal
        /// </summary>
        /// <param name="lstAppraisal"></param>
        /// <returns></returns>
        public bool SaveAppraisalList(List<ProbableList> lstAppraisal, string freezeComment, int loggedUser, string action)
        {
            var objApr = new Appraisal();
            foreach (var oItem in lstAppraisal)
            {
                var objAppr = new AppraiseeList();

                objAppr.App1id = oItem.App1;
                objAppr.App2id = oItem.App2;
                objAppr.EmployeeCode = oItem.EC;
                objAppr.EmployeeCode = oItem.EC;
                objAppr.EmployeeID = oItem.EID;
                objAppr.EmployeeName = oItem.EName;
                objAppr.GroupHeadID = oItem.GHID;
                objAppr.GroupID = oItem.GID;
                objAppr.IDFId = oItem.IDF;
                objAppr.IDFEsc1 = oItem.IDFE1;
                objAppr.IDFEsc2 = oItem.IDFE2;
                objAppr.Rv1ID = oItem.Rv1;
                objAppr.RV2id = oItem.RV2;
                objAppr.FreezeComment = freezeComment;

                objApr.SaveAppraisalList(objAppr, loggedUser, action);
            }
            return false;
        }

        /// <summary>
        ///     Returns current active appraisal year. There should be always only single appraisal year active.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAppraisalYear()
        {
            try
            {
                var objApr = new Appraisal();
                var apprYear = objApr.GetAppraisalYear();
                return apprYear;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AppraisalSection Section(int sectionId, int empID, int loggedUser)
        {
            var objApr = new Appraisal();
            var appraisalSection = new AppraisalSection();

            appraisalSection = objApr.Section(sectionId, empID, loggedUser);

            return appraisalSection;
        }

        //krishal
        public List<SectionList> GetAllSectionList()
        {
            List<SectionList> sectionlists = new List<SectionList>();
            Appraisal objApr = new Appraisal();
            try
            {
                var SectionsLists = objApr.GetAllSectionList().ToList();
                foreach (SectionsList itm in SectionsLists.ToList())
                {
                    SectionList oItem = new SectionList();
                    oItem.sectionId = itm.sectionId;
                    oItem.sectionName = itm.sectionName;
                    oItem.sectionType = itm.sectionType;
                    sectionlists.Add(oItem);
                }
                return sectionlists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<QuestionList> GetAllQuestionList()
        {
            List<QuestionList> QuestionLists = new List<QuestionList>();
            Appraisal objApr = new Appraisal();
            try
            {
                var QuestionsLists = objApr.GetAllQuestionList().ToList();
                foreach (QuestionsList itm in QuestionsLists.ToList())
                {
                    QuestionList oItem = new QuestionList();
                    oItem.questionId = itm.questionId;
                    oItem.questionText = itm.questionText;
                    oItem.dataType = itm.dataType;
                    oItem.questionParam = itm.questionParam;
                    oItem.questionAbbr = itm.questionAbbr;
                    oItem.controlType = itm.controlType;
                    oItem.validation = itm.validation;
                    oItem.editStageId = itm.editStageId;
                    QuestionLists.Add(oItem);
                }
                return QuestionLists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<yearList> GetAllYear()
        {
            List<yearList> YearLists = new List<yearList>();
            Appraisal objApr = new Appraisal();
            try
            {
                var YearList = objApr.GetAllYear().ToList();
                foreach (yearLists itm in YearList.ToList())
                {
                    yearList oItem = new yearList();
                    oItem.AppraisalYearID = itm.AppraisalYearID;
                    oItem.AppraisalYear = itm.AppraisalYear;
                    YearLists.Add(oItem);
                }
                return YearLists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<yearSectionMapping> GetYearSectionList(int? ID)
        {
            List<yearSectionMapping> lstYearSectin = new List<yearSectionMapping>();
            Appraisal objApr = new Appraisal();
            try
            {
                var YearSectionList = objApr.GetYearSectionList(ID).ToList();

                foreach (yearSectionsMapping itm in YearSectionList.ToList())
                {
                    yearSectionMapping oItem = new yearSectionMapping();
                    oItem.MappingId = itm.MappingId;
                    oItem.YearID = itm.YearID;
                    oItem.YearName = itm.YearName;
                    oItem.SectionId = itm.SectionId;
                    oItem.SectionName = itm.SectionName;
                    oItem.Order = itm.Order;
                    oItem.isRequired = itm.isRequired;
                    oItem.Stages = itm.Stages;

                    lstYearSectin.Add(oItem);

                }

                return lstYearSectin;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Mepping> GetMappingId(int? yearId, int sectionId)
        {
            List<Mepping> mappingId = new List<Mepping>();
            Appraisal objApr = new Appraisal();
            try
            {
                var maingId = objApr.GetMappingId(yearId, sectionId).ToList();

                foreach (Meppings itm in maingId.ToList())
                {
                    Mepping oItem = new Mepping();
                    oItem.MappingId = itm.MappingId;

                    mappingId.Add(oItem);

                }

                return mappingId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SectionList> GetAllSectionOfYear(int? Id)
        {
            List<SectionList> sectionlists = new List<SectionList>();
            Appraisal objApr = new Appraisal();
            try
            {
                var SectionsLists = objApr.GetAllSectionOfYear(Id).ToList();
                foreach (SectionsList itm in SectionsLists.ToList())
                {
                    SectionList oItem = new SectionList();
                    oItem.sectionId = itm.sectionId;
                    oItem.sectionName = itm.sectionName;
                    sectionlists.Add(oItem);
                }
                return sectionlists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<finalMapping> GetYearQuestionList(int? yearId, int? sectionId)
        {
            List<finalMapping> lstYearSectionmapping = new List<finalMapping>();
            Appraisal objApr = new Appraisal();
            try
            {
                var YearSectionList = objApr.GetYearQuestionList(yearId, sectionId).ToList();

                foreach (finalMappings itm in YearSectionList.ToList())
                {
                    finalMapping oItem = new finalMapping();
                    oItem.MappingId = itm.MappingId;
                    oItem.QMappingId = itm.QMappingId;
                    oItem.YearID = itm.YearID;
                    oItem.YearName = itm.YearName;
                    oItem.SectionId = itm.SectionId;
                    oItem.SectionName = itm.SectionName;
                    oItem.QuestionId = itm.QuestionId;
                    oItem.QuestionName = itm.QuestionName;
                    oItem.Order = itm.Order;
                    oItem.isRequired = itm.isRequired;
                    oItem.Stages = itm.Stages;

                    lstYearSectionmapping.Add(oItem);

                }

                return lstYearSectionmapping;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<employee> EmployeeList(int? Id)
        {
            List<employee> lstEmployee = new List<employee>();
            Appraisal objApr = new Appraisal();
            try
            {
                var YearSectionList = objApr.EmployeeList(Id).ToList();

                foreach (Employee itm in YearSectionList.ToList())
                {
                    employee oItem = new employee();
                    oItem.EmployeeId = itm.EmployeeId;
                    oItem.EmployeeCode = itm.EmployeeCode;
                    oItem.EmployeeName = itm.EmployeeName;
                    // oItem.RelationAbbr = itm.RelationAbbr;
                    oItem.RelationShip = itm.RelationShip;
                    oItem.StageComplete = itm.StageComplete;
                    oItem.IsLinkActive = itm.IsLinkActive;
                    oItem.reviewlink = itm.reviewlink;
                    lstEmployee.Add(oItem);

                }

                return lstEmployee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveUpdateSections(List<SectionList> sections)
        {
            Appraisal objApr = new Appraisal();
            foreach (SectionList oItem in sections)
            {
                SectionsList objSec = new SectionsList();

                objSec.sectionId = oItem.sectionId;
                objSec.sectionName = oItem.sectionName;
                objSec.sectionType = oItem.sectionType;

                objApr.SaveUpdateSections(objSec);
            }
            return false;
        }

        public bool SaveUpdateQuestions(List<QuestionList> questions)
        {
            Appraisal objApr = new Appraisal();
            foreach (QuestionList oItem in questions)
            {
                QuestionsList objQue = new QuestionsList();

                objQue.questionId = oItem.questionId;
                objQue.questionText = oItem.questionText;
                objQue.dataType = oItem.dataType;
                objQue.questionParam = oItem.questionParam;
                objQue.questionAbbr = oItem.questionAbbr;
                objQue.controlType = oItem.controlType;
                objQue.validation = oItem.validation;
                objQue.editStageId = oItem.editStageId;

                objApr.SaveUpdateQuestions(objQue);
            }
            return false;
        }

        public bool SaveYearSectionMapping(List<yearSectionMapping> mappingData)
        {
            Appraisal objApr = new Appraisal();
            foreach (yearSectionMapping oItem in mappingData)
            {
                yearSectionsMapping objYSMaping = new yearSectionsMapping();

                objYSMaping.YearID = oItem.YearID;
                objYSMaping.SectionId = oItem.SectionId;
                objYSMaping.Order = oItem.Order;
                objYSMaping.MappingType = oItem.MappingType;
                objYSMaping.isRequired = oItem.isRequired;
                objYSMaping.Stages = oItem.Stages;

                objApr.SaveYearSectionMapping(objYSMaping);
            }
            return false;
        }

        public bool SaveMapping(List<finalMapping> mappingData)
        {
            Appraisal objApr = new Appraisal();
            foreach (finalMapping oItem in mappingData)
            {
                finalMappings objYSMaping = new finalMappings();

                objYSMaping.QuestionId = oItem.QuestionId;
                objYSMaping.MappingId = oItem.MappingId;
                objYSMaping.Order = oItem.Order;
                objYSMaping.MappingType = oItem.MappingType;
                objYSMaping.isRequired = oItem.isRequired;
                objYSMaping.Stages = oItem.Stages;

                objApr.SaveMapping(objYSMaping);
            }
            return false;
        }

        public bool UpdateSectionOrder(List<yearSectionMapping> lstSectionOrder)
        {
            var objApr = new Appraisal();
            foreach (var oItem in lstSectionOrder)
            {
                var objAppr = new yearSectionsMapping();

                objAppr.MappingId = oItem.MappingId;
                objAppr.YearID = oItem.YearID;
                objAppr.SectionId = oItem.SectionId;
                objAppr.Order = oItem.Order;

                objApr.UpdateSectionOrder(objAppr);
            }
            return false;
        }

        public bool UpdateQuestionOrder(List<finalMapping> lstQuestionOrder)
        {
            var objApr = new Appraisal();
            foreach (var oItem in lstQuestionOrder)
            {
                var objAppr = new finalMappings();

                objAppr.MappingId = oItem.MappingId;
                objAppr.QMappingId = oItem.QMappingId;
                objAppr.YearID = oItem.YearID;
                objAppr.SectionId = oItem.SectionId;
                objAppr.QuestionId = oItem.QuestionId;
                objAppr.Order = oItem.Order;

                objApr.UpdateQuestionOrder(objAppr);
            }
            return false;
        }

        public bool UpdateSectionMapping(List<yearSectionMapping> lstSectionOrder)
        {
            var objApr = new Appraisal();
            foreach (var oItem in lstSectionOrder)
            {
                var objAppr = new yearSectionsMapping();

                objAppr.MappingId = oItem.MappingId;
                objAppr.isRequired = oItem.isRequired;
                objAppr.Stages = oItem.Stages;

                objApr.UpdateSectionMapping(objAppr);
            }
            return false;
        }

        public bool UpdateQuestionMapping(List<finalMapping> lstSectionOrder)
        {
            var objApr = new Appraisal();
            foreach (var oItem in lstSectionOrder)
            {
                var objAppr = new finalMappings();

                objAppr.MappingId = oItem.MappingId;
                objAppr.isRequired = oItem.isRequired;
                objAppr.Stages = oItem.Stages;

                objApr.UpdateQuestionMapping(objAppr);
            }
            return false;
        }

    }

    public class SetupList
    {
        public List<DropDownList<int>> Appr1 { get; set; }
        public string Appr2 { get; set; }
        public string Revr1 { get; set; }
        public string Revr2 { get; set; }
        public string GroupHeadID { get; set; }
        public string IDFId { get; set; }
        public string IDFEsc1 { get; set; }
        public string IDFEsc2 { get; set; }
    }

    public class DropDownList<T>
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

    public class MenuList
    {
        public List<Menu> MenuItem { get; set; }
        public string errorMessage { get; set; }
    }

    public class ProbableList
    {
        public int EC { get; set; }
        public int EID { get; set; }
        public string EName { get; set; }
        // public int ProjectID { get; set; }
        public int App1 { get; set; }
        public int App2 { get; set; }
        public int Rv1 { get; set; }
        public int RV2 { get; set; }
        public int GID { get; set; }
        public int GHID { get; set; }
        public int IDF { get; set; }
        public int IDFE1 { get; set; }
        public int IDFE2 { get; set; }
        public int DU { get; set; }
        public int RPool { get; set; }
        public string DUName { get; set; }
        public string RPoolName { get; set; }
    }

    //krishal
    public class SectionList
    {
        public int sectionId { get; set; }
        public string sectionName { get; set; }
        public int sectionType { get; set; }
    }

    public class QuestionList
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

    public class yearList
    {
        public int AppraisalYearID { get; set; }
        public string AppraisalYear { get; set; }
    }

    public class yearSectionMapping
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

    public class finalMapping
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

    public class employee
    {
        public int EmployeeId { get; set; }
        public int EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        // public string RelationAbbr { get; set; }
        public string RelationShip { get; set; }
        public string StageComplete { get; set; }
        public string IsLinkActive { get; set; }
        public string reviewlink { get; set; }
    }

    public class Mepping
    {
        public int MappingId { get; set; }
    }

}