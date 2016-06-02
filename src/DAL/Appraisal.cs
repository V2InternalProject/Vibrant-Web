using HRMS.Models;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using V2.CommonServices.Exceptions;

namespace HRMS.DAL
{
    [Serializable]
    public class Appraisal
    {
        private readonly String ConnectionString =
            ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAppraisalYear()
        {
            var dAppraisalYear = new Dictionary<int, string>();
            try
            {
                var Year = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "getAppraisalYear");

                if (Year != null && Year.Tables.Count > 0)
                {
                    if (Year.Tables[0] != null && Year.Tables[0].Rows.Count > 0)
                    {
                        int id = Convert.ToInt16(Year.Tables[0].Rows[0]["AppraisalYearID"]);
                        var appYear = Convert.ToString(Year.Tables[0].Rows[0]["AppraisalYear"]);

                        dAppraisalYear.Add(id, appYear);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
            return dAppraisalYear;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="empID"></param>
        /// <param name="loggedUser"></param>
        /// <returns></returns>
        public AppraisalSection Section(int sectionId, int empID, int loggedUser)
        {
            var appSection = new AppraisalSection();
            var yearId = GetAppraisalYear().Keys.FirstOrDefault();
            if (yearId == null)
            {
                appSection.errorMessage = "No Appraisal Year is active.";
                return appSection;
            }

            appSection = GetSection(sectionId, yearId, empID, loggedUser);

            return appSection;
        }

        /// <summary>
        ///     get Section Configuration for appraisal form
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="yearId"></param>
        /// <param name="empId"></param>
        /// <returns></returns>
        private AppraisalSection GetSection(int sectionId, int yearId, int empId, int loggedUser)
        {
            var objParam = new SqlParameter[4];
            objParam[0] = new SqlParameter("@yearId", yearId);
            objParam[1] = new SqlParameter("@secitonId", sectionId);
            objParam[2] = new SqlParameter("@empId", empId);
            objParam[3] = new SqlParameter("@loggedUser", loggedUser);
            var questionList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                "GetAppraisalSectionConfiguration", objParam);

            var appraisalSection = new AppraisalSection();

            appraisalSection.sectionId = sectionId;
            appraisalSection.loggedUser = loggedUser;
            appraisalSection.empId = empId;
            appraisalSection.yearId = yearId;

            if (questionList.Tables.Count > 0)
            {
                if (questionList.Tables[0].Rows.Count > 0)
                {
                    appraisalSection.sectionName = questionList.Tables[0].Rows[0]["sectionName"].ToString();
                    appraisalSection.sectionType = questionList.Tables[0].Rows[0]["sectionType"].ToString();
                    appraisalSection.commandType = questionList.Tables[0].Rows[0]["commandType"].ToString();
                    appraisalSection.isSubmit = questionList.Tables[0].Rows[0]["isSubmit"].ToString(); ;
                    appraisalSection.sectionTypeParser = questionList.Tables[0].Rows[0]["sectionTypeAbbr"].ToString();

                    var questions = new Dictionary<string, AppraisalQuestions>();
                    var param = new Dictionary<string, Dictionary<string, string>>();

                    foreach (DataRow dr in questionList.Tables[0].Rows)
                    {
                        var appraisalQuestions = new AppraisalQuestions();
                        appraisalQuestions.controlType = dr["controlType"].ToString();
                        appraisalQuestions.dataType = dr["dataType"].ToString();
                        appraisalQuestions.isRequired = Convert.ToInt16(dr["isRequired"]) == 1 ? true : false;
                        appraisalQuestions.questionId = Convert.ToInt16(dr["questionid"]);
                        appraisalQuestions.questionText = dr["questionText"].ToString();
                        appraisalQuestions.seq = Convert.ToInt16(dr["seq"]);
                        appraisalQuestions.validation = new QuestionValidation();
                        if (string.IsNullOrEmpty(dr["questionAbbr"].ToString()))
                        {
                            questions.Add(dr["questionText"].ToString().Trim(), appraisalQuestions);
                        }
                        else
                        {
                            questions.Add(dr["questionAbbr"].ToString(), appraisalQuestions);
                        }

                        if (!string.IsNullOrEmpty(dr["questionParam"].ToString()))
                        {
                            var p = dr["questionParam"].ToString().Split(',');
                            var d = new Dictionary<string, string>();
                            for (var i = 0; i < p.Count(); i++)
                            {
                                d.Add("r" + i, p[i]);
                            }
                            param.Add(dr["questionAbbr"].ToString(), d);
                        }
                    }
                    appraisalSection.param = param;
                    appraisalSection.questions = questions;
                    appraisalSection.protocol = "post";
                    appraisalSection.saveURL = "#save/sectionid/for/empid";

                    // appraisalSection = AddModQuestionsforStage(appraisalSection);
                }
                else
                {
                    appraisalSection.commandType = "1";
                    appraisalSection.isSubmit = "no";
                    appraisalSection.errorMessage = "No data available.";
                }
            }
            else
            {
                appraisalSection.commandType = "1";
                appraisalSection.isSubmit = "no";
                appraisalSection.errorMessage = "No data available.";
            }
            return appraisalSection;
        }

        /// <summary>
        ///     Modifying section config for display purpose
        /// </summary>
        /// <param name="appraisalSection"></param>
        /// <returns></returns>
        private AppraisalSection AddModQuestionsforStage(AppraisalSection appraisalSection)
        {
            var stageid = GetAppraisalStage(appraisalSection.empId, appraisalSection.loggedUser, appraisalSection.yearId);

            if (appraisalSection.sectionType == "V")
            {
                if (stageid > 1 && stageid != 6) //6 for IDF stage and 1 for appraisee
                {
                    foreach (var q in appraisalSection.questions.ToList())
                    {
                        q.Value.controlType = "Label";
                    }
                }
            }
            else if (appraisalSection.sectionType == "RC")
            {
                var i = appraisalSection.questions.Count() - 2;
                foreach (var q in appraisalSection.questions.ToList())
                {
                    i--;
                    q.Value.controlType = "Label";
                    if (i == 0)
                        break;
                }
            }
            else
            {
                var i = appraisalSection.questions.Count() - 1;
                foreach (var q in appraisalSection.questions.ToList())
                {
                    i--;
                    q.Value.controlType = "Label";
                    if (i == 0)
                        break;
                }
            }
            return appraisalSection;
        }

        /// <summary>
        ///     get current stage of employee
        /// </summary>
        /// <param name="empID"></param>
        /// <param name="loggedUser"></param>
        /// <returns></returns>
        private int GetAppraisalStage(int empID, int loggedUser, int yearId)
        {
            var objParam = new SqlParameter[3];
            objParam[0] = new SqlParameter("@empId", empID);
            objParam[1] = new SqlParameter("@LoggedUser", loggedUser);
            objParam[2] = new SqlParameter("@yearId", yearId);

            var stageId = SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetAppraisalStage",
                objParam);
            if (!string.IsNullOrEmpty(Convert.ToString(stageId)))
                return Convert.ToInt16(stageId);
            else
                return 0;
        }

        public List<EmployeeList<int>> GetActiveEmployeeList()
        {
            var employeeList = new List<EmployeeList<int>>();
            try
            {
                DataSet empList;

                empList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                    "GetActiveEmployeeList");

                foreach (DataRow drw in empList.Tables[0].Rows)
                {
                    var eItemList = new EmployeeList<int>();
                    eItemList.Value = Convert.ToInt32(drw["employeeid"]);
                    eItemList.Text = Convert.ToString(drw["employeename"]);
                    employeeList.Add(eItemList);
                }

                return employeeList;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public MenuItem GetMenuList(int empId, int loggedUser)
        {
            var menuItem = new MenuItem();
            try
            {
                DataSet menu;
                var menuList = new List<Menu>();
                var yearId = GetAppraisalYear().Keys.FirstOrDefault();
                if (yearId == null)
                {
                    menuItem.errorMessage = "No Appraisal Year is active.";
                    return menuItem;
                }

                var objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@yearId", yearId);
                objParam[1] = new SqlParameter("@empId", empId);
                objParam[2] = new SqlParameter("@loggedUser", loggedUser);

                menu = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                    "GetAppraisalMenu", objParam);
                menuList.Add(new Menu { SectionID = 99, Text = "Details", URL = "#/Candidate/Details/For/" + empId.ToString() });
                foreach (DataRow drw in menu.Tables[0].Rows)
                {
                    var oItem = new Menu();
                    oItem.SectionID = Convert.ToInt32(drw["SectionID"]);
                    oItem.Text = Convert.ToString(drw["Text"]);
                    oItem.URL = Convert.ToString(drw["URL"]);
                    menuList.Add(oItem);
                }
                menuItem.MenuList = menuList;
                return menuItem;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dYearId"></param>
        /// <returns></returns>
        public DataSet GetSectionList(int? dYearId)
        {
            DataSet dsSection;
            try
            {
                var objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@yearId", dYearId);
                dsSection = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                    "getAppraisalSectionList", objParam);
                return dsSection;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="yearId"></param>
        /// <param name="sectionID"></param>
        /// <returns></returns>
        public DataSet GetQuestionList(int? yearId, int? sectionID)
        {
            if (yearId == null) throw new ArgumentNullException("yearId");
            DataSet dsQuestion;
            try
            {
                var objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@yearId", yearId);
                objParam[1] = new SqlParameter("@sectionId", sectionID);
                dsQuestion = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                    "getAppraisalQuestionList", objParam);
                return dsQuestion;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<AppraiseeList> GetAppraisalList()
        {
            DataSet dsAppraisalList;
            var lstApr = new List<AppraiseeList>();
            try
            {
                var yearId = GetAppraisalYear().Keys.FirstOrDefault();
                if (yearId == null)
                {
                    throw new Exception("No Appraisal Year is active.");
                }

                var objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@yearID", yearId);

                dsAppraisalList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                    "GetProbableAppraiseeList", objParam);

                GetListGiveDs(dsAppraisalList, lstApr);

                return lstApr;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public AppraiseeDetails GetAppraiseeDetails(int empId)
        {
            DataSet dsAppraisee;
            var Appraisee = new AppraiseeDetails();
            try
            {
                var yearId = GetAppraisalYear().Keys.FirstOrDefault();
                if (yearId == null)
                {
                    throw new Exception("No Appraisal Year is active.");
                }

                var objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@yearID", yearId);
                objParam[1] = new SqlParameter("@empid", empId);

                dsAppraisee = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                    "GetApraiseeDisplayDetails", objParam);

                if (dsAppraisee.Tables.Count > 0)
                {
                    if (dsAppraisee.Tables[0].Rows.Count > 0)
                    {
                        Appraisee.EmployeeCode = Convert.ToString(dsAppraisee.Tables[0].Rows[0]["EmployeeCode"]);
                        Appraisee.Employeename = Convert.ToString(dsAppraisee.Tables[0].Rows[0]["EmployeeName"]);
                        Appraisee.Appraiser1 = Convert.ToString(dsAppraisee.Tables[0].Rows[0]["Appraiser1"]);
                        Appraisee.Appraiser2 = Convert.ToString(dsAppraisee.Tables[0].Rows[0]["Appraiser2"]);
                        Appraisee.Reviewer1 = Convert.ToString(dsAppraisee.Tables[0].Rows[0]["reviewer1"]);
                        Appraisee.Reviewer2 = Convert.ToString(dsAppraisee.Tables[0].Rows[0]["reviewer2"]);
                        Appraisee.GroupHead = Convert.ToString(dsAppraisee.Tables[0].Rows[0]["GroupHead"]);
                        Appraisee.DateOfJoining = Convert.ToString(dsAppraisee.Tables[0].Rows[0]["DateOfJoining"]);
                        Appraisee.DeliveryUnit = Convert.ToString(dsAppraisee.Tables[0].Rows[0]["DeliveryUnit"]);
                        Appraisee.Designation = Convert.ToString(dsAppraisee.Tables[0].Rows[0]["Designation"]);
                        Appraisee.Location = Convert.ToString(dsAppraisee.Tables[0].Rows[0]["Location"]);
                        Appraisee.PeriodFrom = Convert.ToString(dsAppraisee.Tables[0].Rows[0]["PeriodFrom"]);
                        Appraisee.PeriodTo = Convert.ToString(dsAppraisee.Tables[0].Rows[0]["PeriodTo"]);
                    }
                }

                return Appraisee;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dsAppraisalList"></param>
        /// <param name="lstApr"></param>
        private static void GetListGiveDs(DataSet dsAppraisalList, List<AppraiseeList> lstApr)
        {
            if (dsAppraisalList != null && dsAppraisalList.Tables.Count > 0)
            {
                if (dsAppraisalList.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drw in dsAppraisalList.Tables[0].Rows)
                    {
                        var oApr = new AppraiseeList();
                        oApr.App1id = Convert.ToInt16(drw["App1id"]);
                        oApr.App2id = Convert.ToInt16(drw["App2id"]);
                        oApr.EmployeeCode = Convert.ToInt16(drw["EmployeeCode"]);
                        oApr.EmployeeID = Convert.ToInt16(drw["EmployeeID"]);
                        oApr.EmployeeName = Convert.ToString(drw["EmployeeName"]);
                        oApr.GroupHeadID = Convert.ToInt16(drw["GroupHeadID"]);
                        oApr.GroupID = Convert.ToInt16(drw["GroupID"]);
                        oApr.IDFEsc1 = Convert.ToInt16(drw["IDFEscalation1"]);
                        oApr.IDFEsc2 = Convert.ToInt16(drw["IDFEscalation2"]);
                        oApr.IDFId = Convert.ToInt16(drw["IDFPersonID"]);
                        oApr.Rv1ID = Convert.ToInt16(drw["RV1ID"]);
                        oApr.RV2id = Convert.ToInt16(drw["RV2ID"]);
                        oApr.DU = Convert.ToInt16(drw["DUID"]);
                        oApr.RPool = Convert.ToInt16(drw["RPoolID"]);
                        oApr.DUName = Convert.ToString(drw["DUName"]);
                        oApr.RPoolName = Convert.ToString(drw["RPoolName"]);

                        lstApr.Add(oApr);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="objAppr"></param>
        /// <returns></returns>
        public List<AppraiseeList> GetSavedAppraisalList(SearchAppraisal objAppr)
        {
            DataSet dsSavedAppraisalList;
            var lstSavedApr = new List<AppraiseeList>();
            try
            {
                var yearId = GetAppraisalYear().Keys.FirstOrDefault();
                if (yearId == null)
                {
                    throw new Exception("No Appraisal Year is active.");
                }

                var objParam = new SqlParameter[8];

                objParam[0] = new SqlParameter("@AppraisalYearID", yearId);

                if (objAppr != null)
                {
                    objParam[1] = new SqlParameter("@EmployeeID", objAppr.EmployeeID);
                    objParam[2] = new SqlParameter("@AppraisalStageID", objAppr.AppraisalStageID);
                    objParam[3] = new SqlParameter("@Appraiser1", objAppr.Appraiser1);
                    objParam[4] = new SqlParameter("@Appraiser2", objAppr.Appraiser2);
                    objParam[5] = new SqlParameter("@Reviewer1", objAppr.Reviewer1);
                    objParam[6] = new SqlParameter("@Reviewer2", objAppr.Reviewer2);
                    objParam[7] = new SqlParameter("@GroupHead", objAppr.GroupHead);
                }
                else
                {
                    objParam[1] = new SqlParameter("@EmployeeID", DBNull.Value);
                    objParam[2] = new SqlParameter("@AppraisalStageID", DBNull.Value);
                    objParam[3] = new SqlParameter("@Appraiser1", DBNull.Value);
                    objParam[4] = new SqlParameter("@Appraiser2", DBNull.Value);
                    objParam[5] = new SqlParameter("@Reviewer1", DBNull.Value);
                    objParam[6] = new SqlParameter("@Reviewer2", DBNull.Value);
                    objParam[7] = new SqlParameter("@GroupHead", DBNull.Value);
                }

                dsSavedAppraisalList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                    "getsavedappraisal", objParam);

                GetListGiveDs(dsSavedAppraisalList, lstSavedApr);
                return lstSavedApr;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objAppr"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool SaveAppraisalList(AppraiseeList objAppr, int loggedUser, string action)
        {
            try
            {
                var objParam = new SqlParameter[20];

                var yearId = GetAppraisalYear().Keys.FirstOrDefault();
                if (yearId == null)
                {
                    throw new Exception("No Appraisal Year is active.");
                }

                objParam[0] = new SqlParameter("@Action", action);
                objParam[1] = new SqlParameter("@EmployeeID", objAppr.EmployeeID);
                objParam[2] = new SqlParameter("@AppraisalYearID", yearId);
                objParam[3] = new SqlParameter("@AppraisalStageID", DBNull.Value);
                objParam[4] = new SqlParameter("@AppraisalInitiatedOn", DBNull.Value);
                objParam[5] = new SqlParameter("@Appraiser1", objAppr.App1id);
                objParam[6] = new SqlParameter("@Appraiser2", objAppr.App2id);
                objParam[7] = new SqlParameter("@Reviewer1", objAppr.Rv1ID);
                objParam[8] = new SqlParameter("@Reviewer2", objAppr.RV2id);
                objParam[9] = new SqlParameter("@GroupHead", objAppr.GroupHeadID);
                objParam[10] = new SqlParameter("@IDFPerson", objAppr.IDFId);
                objParam[11] = new SqlParameter("@IDFEscalation1", objAppr.IDFEsc1);
                objParam[12] = new SqlParameter("@IDFEscalation2", objAppr.IDFEsc2);
                objParam[13] = new SqlParameter("@IDFAprraiseComment", DBNull.Value);
                objParam[14] = new SqlParameter("@IDFISAppraiseAgree", DBNull.Value);
                objParam[15] = new SqlParameter("@CancelComment", DBNull.Value);
                objParam[16] = new SqlParameter("@IsCancelled", DBNull.Value);
                objParam[17] = new SqlParameter("@UnFreezedByAdmin", DBNull.Value);
                objParam[18] = new SqlParameter("@FreezeComment", objAppr.FreezeComment);
                objParam[19] = new SqlParameter("@LoggedinUserID", loggedUser);

                var response = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "saveappraisal",
                    objParam);

                //try {
                //    if(action.ToLower() == "initiate")
                //    {
                //        SendMail(objAppr.EmployeeID.ToString(), "HRAdmin","system");
                //    }
                //}
                //catch (Exception ex) { }

                return (response > 0);
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dYearId"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public DataSet GetAppraisalForm(int dYearId, int? employeeID)
        {
            DataSet dsForm;
            try
            {
                var objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@yearId", dYearId);
                objParam[1] = new SqlParameter("@employeeID", employeeID);

                dsForm = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "getAppraisalForm",
                    objParam);
                return dsForm;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// Returns all details ie.e from, to email address, subject and body of mail
        /// </summary>
        /// <param name="fromUser"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public DataSet GetMailDetails(int fromUser, int employeeID, string msg)
        {
            DataSet dsMailDetails;
            try
            {
                var yearId = GetAppraisalYear().Keys.FirstOrDefault();
                if (yearId == null)
                {
                    throw new Exception("No Appraisal Year is active.");
                }

                var objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@FromID", fromUser);
                objParam[1] = new SqlParameter("@empID", employeeID);
                objParam[2] = new SqlParameter("@yearId", yearId);
                objParam[3] = new SqlParameter("@IDFStatus", msg);

                dsMailDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetSubmitMailInfo", objParam);
                return dsMailDetails;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objSection"></param>
        /// <returns></returns>
        public bool SaveSection(Section objSection)
        {
            try
            {
                var objParam = new SqlParameter[3];

                objParam[0] = new SqlParameter("@sectionName", objSection.sectionName);
                objParam[1] = new SqlParameter("@sectionType", objSection.sectionType);
                objParam[2] = new SqlParameter("@loggedinUserID", objSection.lastUpdatedBy);

                var response = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "SaveSection",
                    objParam);
                return Convert.ToBoolean(response);
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objQuestion"></param>
        /// <returns></returns>
        public bool SaveAppraisalQuestion(Question objQuestion)
        {
            try
            {
                var objParam = new SqlParameter[8];

                objParam[0] = new SqlParameter("@questionName", objQuestion.questionName);
                objParam[1] = new SqlParameter("@questionType", objQuestion.questionType);
                objParam[2] = new SqlParameter("@loggedinUserID", objQuestion.lastUpdatedBy);

                if (objQuestion.parentQuestionId != null && objQuestion.parentQuestionId > 0)
                    objParam[3] = new SqlParameter("@parentQuestionId", objQuestion.parentQuestionId);
                else
                    objParam[3] = new SqlParameter("@parentQuestionId", DBNull.Value);

                objParam[4] = new SqlParameter("@seq", objQuestion.seq);
                objParam[5] = new SqlParameter("@dType", objQuestion.dType);
                objParam[6] = new SqlParameter("@mandatory", objQuestion.mandatory);
                objParam[7] = new SqlParameter("@stageId", objQuestion.stageId);

                var response = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                    "SaveAppraisalQuestion", objParam);

                return Convert.ToBoolean(response);
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="appraisalSectionPost"></param>
        /// <returns></returns>
        public VibrantHttpResponse saveAppraisalSection(AppraisalSectionPost appraisalSectionPost)
        {
            VibrantHttpResponse vResponse = new VibrantHttpResponse();
            try
            {
                var objParam = new SqlParameter[6];

                var yearId = GetAppraisalYear().Keys.FirstOrDefault();
                if (yearId == null)
                {
                    throw new Exception("No Appraisal Year is active.");
                }

                appraisalSectionPost.YearId = yearId;
                int appStage = GetAppraisalStage(appraisalSectionPost.EmpId, appraisalSectionPost.LoggedUser, yearId);

                if (appStage > 0)
                {
                    objParam[0] = new SqlParameter("@EmpID", appraisalSectionPost.EmpId);
                    objParam[1] = new SqlParameter("@Loggeduser", appraisalSectionPost.LoggedUser);
                    objParam[2] = new SqlParameter("@Section", appraisalSectionPost.SectionId);
                    objParam[3] = new SqlParameter("@StageID", appStage);
                    objParam[4] = new SqlParameter("@YearID", yearId);
                    objParam[5] = new SqlParameter("@data", System.Data.DbType.String);       //appraisalSectionPost.Data
                    objParam[5].Value = appraisalSectionPost.Data.ToString();
                    var response = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "SaveAnswer",
                        objParam);

                    vResponse.Data = Convert.ToString(response);
                    vResponse.Message = "Saved Successfully";
                }
            }
            catch (Exception ex)
            {
                vResponse.Exception = ex;
                vResponse.Message = "Save operation failed. Error : " + ex.Message.ToString();
            }
            return vResponse;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="appraisalSectionPost"></param>
        /// <returns></returns>
        public VibrantHttpResponse Submit(AppraisalSectionPost appraisalSectionPost)
        {
            VibrantHttpResponse vResponse = new VibrantHttpResponse();
            try
            {
                var objParam = new SqlParameter[6];

                var yearId = GetAppraisalYear().Keys.FirstOrDefault();
                if (yearId == null)
                {
                    throw new Exception("No Appraisal Year is active.");
                }

                appraisalSectionPost.YearId = yearId;
                int appStage = GetAppraisalStage(appraisalSectionPost.EmpId, appraisalSectionPost.LoggedUser, appraisalSectionPost.YearId);

                if (appStage > 0)
                {
                    objParam[0] = new SqlParameter("@EmpID", appraisalSectionPost.EmpId);
                    objParam[1] = new SqlParameter("@Loggeduser", appraisalSectionPost.LoggedUser);
                    objParam[2] = new SqlParameter("@StageID", appStage);
                    objParam[3] = new SqlParameter("@NewStageID", appStage + 1);
                    objParam[4] = new SqlParameter("@YearID", appraisalSectionPost.YearId);
                    objParam[5] = new SqlParameter("@errorMsg", SqlDbType.VarChar);
                    objParam[5].Size = 250;
                    objParam[5].Direction = ParameterDirection.Output;

                    var response = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "SubmitAppraisal",
                        objParam);

                    if (!string.IsNullOrEmpty(Convert.ToString(objParam[5].Value)))
                    {
                        if (Convert.ToString(objParam[5].Value).ToLower().Contains("appraisal submitted to closure") || Convert.ToString(objParam[5].Value).ToLower().Contains("appraisal escalated"))
                        {
                            vResponse.Data = Convert.ToString(response);
                            vResponse.Message = Convert.ToString(objParam[5].Value);
                        }
                        else
                            throw new Exception(Convert.ToString(objParam[5].Value));
                    }
                    else
                    {
                        vResponse.Data = Convert.ToString(response);
                        vResponse.Message = "Appraisal Submitted Successfully to next stage.";
                    }
                }
            }
            catch (Exception ex)
            {
                vResponse.Exception = ex;
                vResponse.Message = "Failed to submit. Error : " + ex.Message.ToString();
            }
            return vResponse;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="appraisalSection"></param>
        /// <returns></returns>
        public AppraisalSectionPost GetAppraisalData(AppraisalSectionPost appraisalSection)
        {
            try
            {
                var yearId = GetAppraisalYear().Keys.FirstOrDefault();
                if (yearId == null)
                {
                    throw new Exception("No Appraisal Year is active.");
                }

                appraisalSection.YearId = yearId;
                var objParam = new SqlParameter[5];
                int appStage = GetAppraisalStage(appraisalSection.EmpId, appraisalSection.LoggedUser, appraisalSection.YearId);
                objParam[0] = new SqlParameter("@EmpID", appraisalSection.EmpId);
                objParam[1] = new SqlParameter("@Loggeduser", appraisalSection.LoggedUser);
                objParam[2] = new SqlParameter("@Section", appraisalSection.SectionId);
                objParam[3] = new SqlParameter("@StageID", appStage);
                objParam[4] = new SqlParameter("@YearID", yearId);

                DataSet appData = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetAppraisalData",
                    objParam);
                if (appData.Tables[0].Rows.Count > 0)
                {
                    if (appraisalSection.SectionTypeParser == "JA")
                        appraisalSection.Data = JArray.Parse(appData.Tables[0].Rows[0]["jdata"].ToString());
                    else if (appraisalSection.SectionTypeParser == "JO")
                        appraisalSection.Data = JObject.Parse(appData.Tables[0].Rows[0]["jdata"].ToString());
                }
                else
                {
                    appraisalSection = new AppraisalSectionPost();
                }
            }
            catch (Exception ex)
            {
                appraisalSection = new AppraisalSectionPost();
                //Log
            }

            return appraisalSection;
        }

        // Krishal

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<SectionsList> GetAllSectionList()
        {
            DataSet sectionList;
            List<SectionsList> lstSection = new List<SectionsList>();
            try
            {
                sectionList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetSectionList");

                if (sectionList != null && sectionList.Tables.Count > 0)
                {
                    if (sectionList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drw in sectionList.Tables[0].Rows)
                        {
                            SectionsList oApr = new SectionsList();
                            oApr.sectionId = Convert.ToInt16(drw["sectionId"]);
                            oApr.sectionName = drw["sectionName"].ToString();
                            oApr.sectionType = Convert.ToInt16(drw["sectionType"]);

                            lstSection.Add(oApr);
                        }
                    }
                }

                return lstSection;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<QuestionsList> GetAllQuestionList()
        {
            DataSet questionList;
            List<QuestionsList> lstQuestion = new List<QuestionsList>();
            try
            {
                questionList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetQuestionList");

                if (questionList != null && questionList.Tables.Count > 0)
                {
                    if (questionList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drw in questionList.Tables[0].Rows)
                        {
                            QuestionsList oApr = new QuestionsList();
                            oApr.questionId = Convert.ToInt16(drw["questionId"]);
                            oApr.questionText = drw["questionText"].ToString();
                            oApr.dataType = drw["dataType"].ToString();
                            oApr.questionParam = drw["questionParam"].ToString();
                            oApr.questionAbbr = drw["questionAbbr"].ToString();
                            oApr.controlType = drw["controlType"].ToString();
                            oApr.validation = drw["validation"].ToString();
                            oApr.editStageId = Convert.ToInt16(drw["editStageId"]);

                            lstQuestion.Add(oApr);
                        }
                    }
                }

                return lstQuestion;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<yearLists> GetAllYear()
        {
            DataSet yearList;
            List<yearLists> lstSection = new List<yearLists>();
            try
            {
                yearList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "getAppraisalYear");

                if (yearList != null && yearList.Tables.Count > 0)
                {
                    if (yearList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drw in yearList.Tables[0].Rows)
                        {
                            yearLists oApr = new yearLists();
                            oApr.AppraisalYearID = Convert.ToInt16(drw["AppraisalYearID"]);
                            oApr.AppraisalYear = drw["AppraisalYear"].ToString();

                            lstSection.Add(oApr);
                        }
                    }
                }

                return lstSection;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<yearSectionsMapping> GetYearSectionList(int? ID)
        {
            DataSet dsYearSectionList;
            List<yearSectionsMapping> lstyear = new List<yearSectionsMapping>();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@yearID", ID);

                dsYearSectionList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetYearSectionList", objParam);

                if (dsYearSectionList != null && dsYearSectionList.Tables.Count > 0)
                {
                    if (dsYearSectionList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drw in dsYearSectionList.Tables[0].Rows)
                        {
                            yearSectionsMapping oYear = new yearSectionsMapping();
                            oYear.MappingId = Convert.ToInt16(drw["MappingId"]);
                            oYear.YearID = Convert.ToInt16(drw["YearId"]);
                            oYear.YearName = drw["YearName"].ToString();
                            oYear.SectionId = Convert.ToInt16(drw["SectionId"]);
                            oYear.SectionName = drw["SectionName"].ToString();
                            oYear.Order = Convert.ToInt16(drw["OrderOfSection"]);
                            oYear.isRequired = Convert.ToInt16(drw["isRequired"]);
                            oYear.Stages = drw["Stages"].ToString();

                            lstyear.Add(oYear);
                        }
                    }
                }

                return lstyear;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="yearId"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public List<Meppings> GetMappingId(int? yearId, int sectionId)
        {
            DataSet dsMappingId;
            List<Meppings> IdMapping = new List<Meppings>();
            try
            {
                var MappingType = "s";
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@yearID", yearId);
                objParam[1] = new SqlParameter("@sectionID", sectionId);
                objParam[2] = new SqlParameter("@MappingType", MappingType);

                dsMappingId = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetMappingId", objParam);

                if (dsMappingId != null && dsMappingId.Tables.Count > 0)
                {
                    if (dsMappingId.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drw in dsMappingId.Tables[0].Rows)
                        {
                            Meppings oYear = new Meppings();
                            oYear.MappingId = Convert.ToInt16(drw["MappingId"]);

                            IdMapping.Add(oYear);
                        }
                    }
                }

                return IdMapping;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<SectionsList> GetAllSectionOfYear(int? Id)
        {
            DataSet sectionList;
            List<SectionsList> lstSection = new List<SectionsList>();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@yearID", Id);

                sectionList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetSectionOfYear", objParam);

                if (sectionList != null && sectionList.Tables.Count > 0)
                {
                    if (sectionList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drw in sectionList.Tables[0].Rows)
                        {
                            SectionsList oApr = new SectionsList();
                            oApr.sectionId = Convert.ToInt16(drw["sectionId"]);
                            oApr.sectionName = drw["sectionName"].ToString();

                            lstSection.Add(oApr);
                        }
                    }
                }

                return lstSection;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// Returns list of questions based on mapping
        /// </summary>
        /// <param name="yearId"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public List<finalMappings> GetYearQuestionList(int? yearId, int? sectionId)
        {
            DataSet dsMappingList;
            List<finalMappings> lstyear = new List<finalMappings>();
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@yearID", yearId);
                objParam[1] = new SqlParameter("@sectionID", sectionId);

                dsMappingList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetYearQuestionList", objParam);

                if (dsMappingList != null && dsMappingList.Tables.Count > 0)
                {
                    if (dsMappingList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drw in dsMappingList.Tables[0].Rows)
                        {
                            finalMappings oYear = new finalMappings();
                            oYear.MappingId = Convert.ToInt16(drw["MappingId"]);
                            oYear.QMappingId = Convert.ToInt16(drw["QMappingId"]);
                            oYear.YearID = Convert.ToInt16(drw["YearID"]);
                            oYear.YearName = drw["YearName"].ToString();
                            oYear.SectionId = Convert.ToInt16(drw["SectionId"]);
                            oYear.SectionName = drw["SectionName"].ToString();
                            oYear.QuestionId = Convert.ToInt16(drw["QuestionId"]);
                            oYear.QuestionName = drw["QuestionName"].ToString();
                            oYear.Order = Convert.ToInt16(drw["OrderOfSection"]);
                            oYear.isRequired = Convert.ToInt16(drw["isRequired"]);
                            oYear.Stages = drw["Stages"].ToString();
                            lstyear.Add(oYear);
                        }
                    }
                }

                return lstyear;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// Returns list of employees base don year ID provided
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<Employee> EmployeeList(int? Id)
        {
            DataSet dsMappingList;
            List<Employee> lstyear = new List<Employee>();
            try
            {
                var yearId = GetAppraisalYear().Keys.FirstOrDefault();
                if (yearId == null)
                {
                    throw new Exception("No Appraisal Year is active.");
                }
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@yearID", yearId);
                objParam[1] = new SqlParameter("@Loggeduser", Id);

                dsMappingList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetAppraisalEmployeeList", objParam);

                if (dsMappingList != null && dsMappingList.Tables.Count > 0)
                {
                    if (dsMappingList.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drw in dsMappingList.Tables[0].Rows)
                        {
                            Employee oYear = new Employee();
                            oYear.EmployeeId = Convert.ToInt16(drw["EmployeeId"]);
                            oYear.EmployeeCode = Convert.ToInt16(drw["EmployeeCode"]);
                            oYear.EmployeeName = drw["EmployeeName"].ToString();
                            // oYear.RelationAbbr = drw["RelationAbbr"].ToString();
                            oYear.RelationShip = drw["AppraisalRole"].ToString();
                            oYear.StageComplete = drw["StageComplete"].ToString();
                            oYear.IsLinkActive = drw["IsLinkActive"].ToString();
                            oYear.reviewlink = drw["reviewlink"].ToString();

                            lstyear.Add(oYear);
                        }
                    }
                }

                return lstyear;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// Submits modifications done to sections to database
        /// </summary>
        /// <param name="objSec"></param>
        /// <returns></returns>
        public string SaveUpdateSections(SectionsList objSec)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[5];

                objParam[0] = new SqlParameter("@SectionId", objSec.sectionId);
                objParam[1] = new SqlParameter("@SectionName", objSec.sectionName);
                objParam[2] = new SqlParameter("@SectionType", objSec.sectionType);
                objParam[3] = new SqlParameter("@ERROR", SqlDbType.VarChar);
                objParam[3].Size = 200;
                objParam[3].Direction = ParameterDirection.Output;

                int response = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "SaveUpdateSections", objParam);
                string abc = Convert.ToString(objParam[3].Value);

                return "";
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// Submits modifications done to questions to database
        /// </summary>
        /// <param name="objQue"></param>
        /// <returns></returns>
        public bool SaveUpdateQuestions(QuestionsList objQue)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[10];

                objParam[0] = new SqlParameter("@QuestionId", objQue.questionId);
                objParam[1] = new SqlParameter("@QuestionName", objQue.questionText);
                objParam[2] = new SqlParameter("@DataType", objQue.dataType);
                objParam[3] = new SqlParameter("@QuestionParam", objQue.questionParam);
                objParam[4] = new SqlParameter("@QuestionAbbr", objQue.questionAbbr);
                objParam[5] = new SqlParameter("@ControlType", objQue.controlType);
                objParam[6] = new SqlParameter("@Validation", objQue.validation);
                objParam[7] = new SqlParameter("@editStageId", objQue.editStageId);

                int response = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "SaveUpdateQuestions", objParam);

                return (response > 0);
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// Submits Year secrion mapping to database
        /// </summary>
        /// <param name="objYSMaping"></param>
        /// <returns></returns>
        public bool SaveYearSectionMapping(yearSectionsMapping objYSMaping)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[10];

                objParam[0] = new SqlParameter("@MappingType", objYSMaping.MappingType);
                objParam[1] = new SqlParameter("@yearId", objYSMaping.YearID);
                objParam[2] = new SqlParameter("@SectionId", objYSMaping.SectionId);
                objParam[3] = new SqlParameter("@Order", objYSMaping.Order);
                objParam[4] = new SqlParameter("@isRequired", objYSMaping.isRequired);
                objParam[5] = new SqlParameter("@Stages", objYSMaping.Stages);

                int response = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "SaveYearSectionMapping", objParam);

                return (response > 0);
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// Submits the Section Question mapping to database
        /// </summary>
        /// <param name="objYSMaping"></param>
        /// <returns></returns>
        public bool SaveMapping(finalMappings objYSMaping)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[10];

                objParam[0] = new SqlParameter("@MappingType", objYSMaping.MappingType);
                objParam[1] = new SqlParameter("@QuestionId", objYSMaping.QuestionId);
                objParam[2] = new SqlParameter("@MappingId", objYSMaping.MappingId);
                objParam[3] = new SqlParameter("@Order", objYSMaping.Order);
                objParam[4] = new SqlParameter("@isRequired", objYSMaping.isRequired);
                objParam[5] = new SqlParameter("@Stages", objYSMaping.Stages);

                int response = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "SaveMapping", objParam);

                return (response > 0);
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// Updates the order of sections as per the modification done on UI
        /// </summary>
        /// <param name="objAppr"></param>
        /// <returns></returns>
        public bool UpdateSectionOrder(yearSectionsMapping objAppr)
        {
            try
            {
                var objParam = new SqlParameter[5];

                objParam[0] = new SqlParameter("@MappingId", objAppr.MappingId);
                objParam[1] = new SqlParameter("@yearId", objAppr.YearID);
                objParam[2] = new SqlParameter("@SectionId", objAppr.SectionId);
                objParam[3] = new SqlParameter("@Order", objAppr.Order);

                var response = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "updateSectionOrder",
                    objParam);

                return (response > 0);
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// Updates the order of questions as per the modification done on UI
        /// </summary>
        /// <param name="objAppr"></param>
        /// <returns></returns>
        public bool UpdateQuestionOrder(finalMappings objAppr)
        {
            try
            {
                var objParam = new SqlParameter[5];

                //int appYear = Convert.ToInt16(GetAppraisalYear().FirstOrDefault().Key);

                objParam[0] = new SqlParameter("@MappingId", objAppr.MappingId);
                objParam[1] = new SqlParameter("@QMappingId", objAppr.QMappingId);
                objParam[2] = new SqlParameter("@QuestionId", objAppr.QuestionId);
                objParam[3] = new SqlParameter("@Order", objAppr.Order);

                var response = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "updateQuestionOrder",
                    objParam);

                return (response > 0);
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public bool UpdateSectionMapping(yearSectionsMapping objAppr)
        {
            try
            {
                var objParam = new SqlParameter[5];

                objParam[0] = new SqlParameter("@MappingId", objAppr.MappingId);
                objParam[1] = new SqlParameter("@isRequired", objAppr.isRequired);
                objParam[2] = new SqlParameter("@Stages", objAppr.Stages);

                var response = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "updateSectionMapping",
                    objParam);

                return (response > 0);
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public bool UpdateQuestionMapping(finalMappings objAppr)
        {
            try
            {
                var objParam = new SqlParameter[5];

                objParam[0] = new SqlParameter("@MappingId", objAppr.MappingId);
                objParam[1] = new SqlParameter("@isRequired", objAppr.isRequired);
                objParam[2] = new SqlParameter("@Stages", objAppr.Stages);

                var response = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "updateQuestionMapping",
                    objParam);

                return (response > 0);
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }
    }
}