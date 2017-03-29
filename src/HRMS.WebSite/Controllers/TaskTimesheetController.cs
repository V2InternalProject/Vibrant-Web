using HRMS.DAL;
using HRMS.Models;
using MvcApplication3.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS.Controllers
{
    public class TaskTimesheetController : Controller
    {
        //
        // GET: /TaskTimesheet/

        public string DownLoadTemplateForTaskCreations
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DownLoadTemplateForTaskCreation"];
            }
        }

        private TaskTimesheetDAL dal = new TaskTimesheetDAL();

        public ActionResult Index()
        {
            return View();
        }

        #region TaskCreation

        [PageAccess(PageName = "Tasks")]
        public ActionResult CreateTask()
        {
            try
            {
                TaskCreationModel model = new TaskCreationModel();
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                SemDAL semDal = new SemDAL();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
                int EmpCode = Convert.ToInt32(employeeDetails.EmployeeCode);
                string EmpID = dal.GetEmployeeIdFromEmployeeCodeSEM(EmpCode);
                model.LoggedInEmployeeId = Convert.ToInt32(EmpID);
                model.TaskStatusList = new List<MasterDataModel>();
                model.TaskTypeList = new List<MasterDataModel>();
                model.AssignedToList = new List<EmployeeList>();
                model.TaskProjectList = dal.GetProjectList(Convert.ToInt32(Membership.GetUser().UserName));
                model.TaskStatusList = dal.GetTaskStatusRecord();
                model.TaskTypeList = dal.GetTaskTypeRecord();
                model.StatusID = dal.GetDefaultStatusId();
                model.TaskTypeID = null;
                model.ProjectTaskTypeID = 0;
                model.ProjectID = 0;
                model.TaskName = "";
                model.StartDate = null;
                model.EndDate = null;
                model.PlannedHours = null;
                model.AvgUnitTime = null;
                model.Description = "";
                model.MileStoneId = null;
                model.PlannedUnits = null;
                model.ProjectTaskType = false;
                model.TagList = dal.GetTagRecord(model.ProjectID);
                //model.AssignedEmployeeName = "Hardik Mukesh Vejani";
                return PartialView("TaskCreation", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult LoadProjectTaskGrid(int? ProjectID, int? MileStoneId, int? SelectedStatusID, int? SelectedAssignedEmployeeId, int page, int rows)
        {
            try
            {
                AddProjectTask model = new AddProjectTask();
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                int totalCount;
                List<AddProjectTask> taskDetails = new List<AddProjectTask>();
                taskDetails = dal.ProjectTaskDetailRecord(ProjectID, MileStoneId, SelectedStatusID, SelectedAssignedEmployeeId, page, rows, out totalCount);

                if ((taskDetails == null || taskDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    taskDetails = dal.ProjectTaskDetailRecord(ProjectID, MileStoneId, SelectedStatusID, SelectedAssignedEmployeeId, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = taskDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult SaveProjectTaskDetails(TaskCreationModel model)
        {
            try
            {
                ProjectTaskRespose response = new ProjectTaskRespose();
                response.status = false;
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                var mailResult = "";
                response = dal.SaveProjectTaskRecord(model);

                if (response.status == true && model.ProjectTaskTypeID == 0)
                {
                    string trimEmployeeList = model.SelectedEmployeeList.TrimEnd(',');
                    string[] stringEmployeeArray = trimEmployeeList.Split(',');
                    int[] AssignedEmployeeArray = Array.ConvertAll(stringEmployeeArray, s => int.Parse(s));
                    string AssignedEmployeeNames = "";
                    for (int i = 0; i < AssignedEmployeeArray.Length; i++)
                    {
                        EmployeeList employeeDetails = dal.GetLoggedInEmployeeDetailsByEmployeeId(AssignedEmployeeArray[i]);
                        AssignedEmployeeNames += employeeDetails.EmployeeName + ",";
                    }
                    AssignedEmployeeNames = AssignedEmployeeNames.TrimEnd(',');
                    List<TemplateHandling> th = new List<TemplateHandling>();
                    th.Add(new TemplateHandling("##Employee Name##", AssignedEmployeeNames));
                    th.Add(new TemplateHandling("##Task Name##", model.TaskName));
                    EmployeeList loggedEmployeeDetails = dal.GetLoggedInEmployeeDetailsByEmployeeId(model.LoggedInEmployeeId);
                    th.Add(new TemplateHandling("##logged in user##", loggedEmployeeDetails.EmployeeName));
                    ProjectDetails projectDetails = dal.GetProjectDetails(model.ProjectID);
                    th.Add(new TemplateHandling("##Project Name##", projectDetails.ProjectName));

                    List<TemplateHandling> SubjectReplacemnt = new List<TemplateHandling>();
                    SubjectReplacemnt.Add(new TemplateHandling("##Project Name##", projectDetails.ProjectName));

                    mailResult = TimeSheetSendMail(model.LoggedInEmployeeId, AssignedEmployeeArray, new int[] { }, th, SubjectReplacemnt, "", "", 89);// "2", "Hi Hello", "Testing Mail");
                }
                return Json(new { status = response.status, mailStatus = mailResult }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetMileStoneList(int? ProjectId)
        {
            try
            {
                TaskCreationModel model = new TaskCreationModel();
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                model.MileStoneList = new List<MileStoneListClass>();
                model.MileStoneList = dal.GetMileStoneList(ProjectId);
                model.AssignedToList = dal.GetActiveEmployeesRecord1(ProjectId);
                model.TagList = dal.GetTagRecord(ProjectId.HasValue ? ProjectId.Value : 0);
                return Json(new { MileStoneList = model.MileStoneList, AssignedToList = model.AssignedToList, TagList = model.TagList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteTaskDetails(int? ProjectTaskTypeId)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                bool result = dal.DeleteTaskRecord(ProjectTaskTypeId);
                return Json(new { status = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion TaskCreation

        [PageAccess(PageName = "Timesheet Entry")]
        public ActionResult TimeSheetEntry()
        {
            TimesheetModel model = new TimesheetModel();
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            SemDAL semDAL = new SemDAL();
            model.SearchedUserDetails = new SearchedUserDetails();
            model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
            SearchedUserDetails employeeDetails = semDAL.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
            int EmpCode = Convert.ToInt32(employeeDetails.EmployeeCode);
            string fromEmployeeID = dal.GetEmployeeIdFromEmployeeCodeSEM(EmpCode);
            //model.ProjectListdata = dal.getProjectName(Convert.ToInt32(Membership.GetUser().UserName));
            model.ProjectListdata = dal.getProjectName();
            model.StatusListdata = dal.getTimeSheetStatusNames();
            model.EmployeeId = Convert.ToInt32(fromEmployeeID);
            return PartialView("TimeSheetEntry", model);
        }

        public ActionResult ProjectNameAutoSuggestForTimesheet(string term)
        {
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            List<EmployeeDetails> searchResult = new List<EmployeeDetails>();
            searchResult = dal.ProjectNameForTimesheet(term, 1, 20);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TimesheetEntryLoadGrid(int? SelectedProjectID, int? SelectedTaskID, DateTime? SelectedFromDate, DateTime? SelectedToDate, int? SelectedStatusID, int? EmployeeId, int page, int rows)
        {
            try
            {
                TimesheetModel model = new TimesheetModel();
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                int totalCount;
                List<TimesheetModel> Entrydetails = new List<TimesheetModel>();
                Entrydetails = dal.TimesheetEntryRecords(SelectedProjectID, SelectedTaskID, SelectedFromDate, SelectedToDate, SelectedStatusID, EmployeeId, page, rows, out totalCount);

                if ((Entrydetails == null || Entrydetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    Entrydetails = dal.TimesheetEntryRecords(SelectedProjectID, SelectedTaskID, SelectedFromDate, SelectedToDate, SelectedStatusID, EmployeeId, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = 0,
                    rows = Entrydetails
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [ValidateInput(false)]
        public ActionResult SaveTimeSheetEntryDetails(TimesheetModel model)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                TimeSheetApprovalModel model1 = new TimeSheetApprovalModel();
                model1.SearchedUserDetails = new SearchedUserDetails();
                model1.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                bool status = false;
                bool IsApproverExist = false;
                bool CanUserCreateTask = false;
                int? projectId = model.ProjectID;
                bool IsEmailSent = false;
                var data = dal.GetTimeSheetApproverDetails(projectId, Convert.ToInt32(Membership.GetUser().UserName));
                var CanUserCreateTaskValue = dal.GetDetailForCreatingTask(projectId);

                if (data != "0")
                {
                    if (model.NewTaskCheckbox == true)
                    {
                        if (CanUserCreateTaskValue == "Yes")
                        {
                            SemDAL semDal = new SemDAL();
                            SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(Convert.ToInt32(model1.SearchedUserDetails.EmployeeId));
                            string LoggedInEmploeeName = employeeDetails.EmployeeFullName;
                            status = dal.SaveTimeSheetEntryRecord(model);
                            int[] EmailToEmployeeIds = new int[] { Convert.ToInt32(data) };
                            List<TemplateHandling> th = new List<TemplateHandling>();
                            List<TemplateHandling> th1 = new List<TemplateHandling>();
                            th.Add(new TemplateHandling("##loggedinuser##", LoggedInEmploeeName));
                            TimeSheetSendMail(Convert.ToInt32(model.EmployeeId.HasValue ? model.EmployeeId.Value : 0), EmailToEmployeeIds, new int[] { }, th, th1, "", "", 91);
                            CanUserCreateTask = true;
                            IsApproverExist = true;
                        }
                        else
                        {
                            CanUserCreateTask = false;
                            IsApproverExist = true;
                        }
                    }
                    else
                    {
                        SemDAL semDal = new SemDAL();
                        SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(Convert.ToInt32(model1.SearchedUserDetails.EmployeeId));
                        string LoggedInEmploeeName = employeeDetails.EmployeeFullName;
                        status = dal.SaveTimeSheetEntryRecord(model);
                        int[] EmailToEmployeeIds = new int[] { Convert.ToInt32(data) };
                        List<TemplateHandling> th = new List<TemplateHandling>();
                        List<TemplateHandling> th1 = new List<TemplateHandling>();
                        th.Add(new TemplateHandling("##loggedinuser##", LoggedInEmploeeName));
                        string emailStatus = TimeSheetSendMail(Convert.ToInt32(model.EmployeeId.HasValue ? model.EmployeeId.Value : 0), EmailToEmployeeIds, new int[] { }, th, th1, "", "", 91);
                        if (emailStatus == "EmailSend")
                            IsEmailSent = true;
                        CanUserCreateTask = true;
                        IsApproverExist = true;
                    }
                }
                else
                {
                    IsApproverExist = false;
                }

                return Json(new { status = status, IsApproverExist = IsApproverExist, CanUserCreateTask = CanUserCreateTask, IsEmailSent = IsEmailSent }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        #region TaskTimesheet Tag Configuration

        [PageAccess(PageName = "Configure Tag")]
        public ActionResult TagConfiguration(int? EmpCode, int? PageID)
        {
            TimesheetTagModel model = new TimesheetTagModel();
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            SemDAL semDAL = new SemDAL();
            model.SearchedUserDetails = new SearchedUserDetails();
            string EmployeeCode = Membership.GetUser().UserName;
            string[] role = Roles.GetRolesForUser(EmployeeCode);
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
            ViewBag.LoggedInUserRole = model.SearchedUserDetails.UserRole;
            model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
            SearchedUserDetails employeeDetails = semDAL.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
            model.UserName = employeeDetails.UserName;
            //model.TagProjectList = semDAL.GetLoggedUserProjectList(employeeDetails.UserName, "", Convert.ToInt32(employeeDetails.EmployeeCode));
            model.TagProjectList = dal.GetActiveProjectList(Convert.ToInt32(Membership.GetUser().UserName));
            //EmployeeDAL empDAL = new EmployeeDAL();
            //Session["NodeLevelAccess"] = empDAL.GetPageLevelAccessForEmployee(Convert.ToInt32(EmpCode), Convert.ToInt32(PageID));

            return PartialView("_TagConfiguration", model);
        }

        [HttpPost]
        public ActionResult TimesheetTagDetailLoadGrid(int? ProjectID, int page, int rows)
        {
            try
            {
                TimesheetTagModel model = new TimesheetTagModel();
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                int totalCount;
                List<TimesheetTagModel> tagDetails = new List<TimesheetTagModel>();
                tagDetails = dal.TimesheetTagDetailRecord(ProjectID, page, rows, out totalCount);

                if ((tagDetails == null || tagDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    tagDetails = dal.TimesheetTagDetailRecord(ProjectID, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = tagDetails
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveTagConfigurationDetails(TimesheetTagModel model, string LoggedUserName, int? ProjectID, string SelectedTagName)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                SEMResponse response = new SEMResponse();
                response = dal.SaveTagConfigurationRecord(model, LoggedUserName, ProjectID, SelectedTagName);
                return Json(new { status = response.status, isTagNameExist = response.isTagNameExist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteTimesheetTagDetails(string[] SelectedTagId)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                bool status = dal.DeleteTimesheetTagRecord(SelectedTagId);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion TaskTimesheet Tag Configuration

        #region Pmconfig

        [PageAccess(PageName = "PMS Fields Configuration")]
        public ActionResult PmsConfiguration()
        {
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            PmsConfiguration model = new PmsConfiguration();

            SemDAL semDAL = new SemDAL();
            model.SearchedUserDetails = new SearchedUserDetails();
            string EmployeeCode = Membership.GetUser().UserName;
            string[] role = Roles.GetRolesForUser(EmployeeCode);
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
            ViewBag.LoggedInUserRole = model.SearchedUserDetails.UserRole;
            var LoggedInUserRole = ViewBag.LoggedInUserRole;
            model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
            SearchedUserDetails employeeDetails = semDAL.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
            model.UserName = employeeDetails.UserName;
            model.EmployeeId = employeeDetails.EmployeeId;
            model.TimesheetProjectList = dal.GetProjectListForTimesheet(LoggedInUserRole, Convert.ToInt32(employeeDetails.EmployeeCode));
            //model.TimesheetProjectList = dal.GetActiveProjectList(Convert.ToInt32(Membership.GetUser().UserName));
            //model.TimesheetSettingList = dal.GetTimesheetSettingsList(LoggedInUserRole);
            // model.TimesheetApproverList = dal.GetTimesheetApproverList();

            return PartialView("_PMSConfigurationSettings", model);
        }

        public JsonResult PmsfilllMainDropDown()
        {
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            string InsertStatus = "";
            try
            {
                var DropDownData = dal.DTimeSheetConfig();
                return Json(new { DropList = DropDownData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception obj)
            {
                InsertStatus = obj.InnerException.ToString();
            }
            return Json(new { status = InsertStatus }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult pmsDeleteFunction(string UniqueId)
        {
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            var InsertStatus = "Done";
            if (!dal.DeleteConfigurationData(UniqueId))
            {
                InsertStatus = "NotDone";
            }
            return Json(new { status = InsertStatus }, "text/html", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PmsSaveFunction(PmsConfiguration pm, string MainDropDown, string DbStatus, string SUniqueId)
        {
            string InsertStatus = "";
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            string MainTyepGrid = pm.MainType;
            if (pm.LevelType == "Global")
            {
                pm.ProjectName = null;
            };
            if (pm.ProjectName == "undefined" || pm.ProjectName == "")
            {
                pm.ProjectName = null;
            };
            if (SUniqueId == "" || SUniqueId == "undefined")
            {
                SUniqueId = null;
            }

            string MainTodo = (MainTyepGrid != null && MainTyepGrid != "") ? "MainTypeInsert" : "SubTypeInsertOrUpdate";
            if (MainDropDown.Contains("Select"))
            {
                MainTodo = "MainTypeInsert";
            }
            if (MainTodo == "MainTypeInsert")
            {
                if ((pm.MainType == "" || pm.MainType == null) && (pm.TypeValue == "" || pm.TypeValue == null))
                {
                    InsertStatus = "Missing";
                }
                else
                {
                    if (dal.SaveConfigurationData(pm.MainType, pm.TypeValue, pm.LevelType, pm.ProjectName, MainDropDown, "MainTypeInsert", SUniqueId) == "Done")
                    {
                        InsertStatus = "DoneAdding";
                    }
                    else if (dal.SaveConfigurationData(pm.MainType, pm.TypeValue, pm.LevelType, pm.ProjectName, MainDropDown, "MainTypeInsert", SUniqueId) == "AlreadyExist")
                    {
                        InsertStatus = "AlreadyExist";
                    }
                    else
                    {
                        InsertStatus = "FailedAdding";
                    }
                }
            }
            if (MainTodo == "SubTypeInsertOrUpdate")
            {
                if ((MainDropDown != "" || MainDropDown != null) && (pm.TypeValue != "" || pm.TypeValue != null) && (pm.LevelType != "" || pm.LevelType != null))
                {
                    if (DbStatus == "Insert")
                    {
                        if (dal.SaveConfigurationData(MainDropDown, pm.TypeValue, pm.LevelType, pm.ProjectName, MainDropDown, "SubTypeInsert", SUniqueId) == "Done")
                        {
                            InsertStatus = "DoneAdding";
                        }
                        else if (dal.SaveConfigurationData(MainDropDown, pm.TypeValue, pm.LevelType, pm.ProjectName, MainDropDown, "SubTypeInsert", SUniqueId) == "NoChanges")
                        {
                            InsertStatus = "NoChanges";
                        }
                        else
                        {
                            InsertStatus = "FailedAdding";
                        }
                    }
                    if (DbStatus == "Update")
                    {
                        if (dal.SaveConfigurationData(MainDropDown, pm.TypeValue, pm.LevelType, pm.ProjectName, MainDropDown, "SubTypeUpdate", SUniqueId) == "Done")
                        {
                            InsertStatus = "DoneAdding";
                        }
                        else if (dal.SaveConfigurationData(MainDropDown, pm.TypeValue, pm.LevelType, pm.ProjectName, MainDropDown, "SubTypeUpdate", SUniqueId) == "AlreadyExist")
                        {
                            InsertStatus = "AlreadyExist";
                        }
                        else
                        {
                            InsertStatus = "FailedAdding";
                        }
                    }
                }
                else
                {
                    InsertStatus = "Missing";
                }
            }

            return Json(new { status = InsertStatus }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PmsConfigurationLoadGrid(string DropDownType)
        {
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            List<PmsConfiguration> pcm = new List<PmsConfiguration>();
            pcm = dal.getRecordsConfiguration(DropDownType);
            int total = pcm.Count();
            var jsonData = new
            {
                total,
                page = 1,
                rows = pcm,
            };
            return Json(jsonData);
        }

        #endregion Pmconfig

        [PageAccess(PageName = "Timesheet Approvals")]
        public ActionResult TimeSheetApproval()
        {
            TimeSheetApprovalModel model = new TimeSheetApprovalModel();
            TaskTimesheetDAL taskTimeSheetDAL = new TaskTimesheetDAL();
            SemDAL semDal = new SemDAL();
            model.SearchedUserDetails = new SearchedUserDetails();
            model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
            SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
            model.ProjectList = taskTimeSheetDAL.GetProjectListAsAprover(Convert.ToInt32(employeeDetails.EmployeeCode));
            string EmployeeID = taskTimeSheetDAL.GetEmployeeIdFromEmployeeCodeSEM(Convert.ToInt32(employeeDetails.EmployeeCode));
            ViewBag.EmployeeId = Convert.ToInt32(employeeDetails.EmployeeCode);
            model.ResourceList = taskTimeSheetDAL.GetResourceList(Convert.ToInt32(EmployeeID));
            ViewBag.EmployeeId1 = Convert.ToInt32(EmployeeID);
            model.StatusList = taskTimeSheetDAL.GetStatusList();
            return PartialView("_TimeSheetApproval", model);
        }

        public ActionResult ProjectNameAutoSuggestForTimesheetApproval(string term, int EmployeeCode)
        {
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            List<EmployeeDetails> searchResult = new List<EmployeeDetails>();

            searchResult = dal.ProjectNameForTimesheetApproval(EmployeeCode, term.ToUpper(), 1, 20);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProjectNameAutoSuggestForTimesheetApproval1(string term, int EmployeeID)
        {
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            List<EmployeeDetails> searchResult = new List<EmployeeDetails>();

            searchResult = dal.GetResourceList1(EmployeeID, term.ToUpper(), 1, 20);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TimeSheetApprovalLoadGrid(int? ProjectID, int? ResourceID, int? StatusID, DateTime? StartDate, DateTime? EndDate, int page, int rows)
        {
            try
            {
                InvoiceDAL invoiceDal = new InvoiceDAL();
                TaskTimesheetDAL taskTimeSheetDAL = new TaskTimesheetDAL();
                List<TimeSheetApprovalDetailsModel> timesheetApprovalDetails = new List<TimeSheetApprovalDetailsModel>();

                timesheetApprovalDetails = taskTimeSheetDAL.GetTimeSheetAprrovalGridDetails(ProjectID, ResourceID, StatusID, StartDate, EndDate, page, rows);
                if ((timesheetApprovalDetails == null || timesheetApprovalDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    timesheetApprovalDetails = taskTimeSheetDAL.GetTimeSheetAprrovalGridDetails(ProjectID, ResourceID, StatusID, StartDate, EndDate, page, rows);
                }
                var totalRecords = timesheetApprovalDetails.Count;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = timesheetApprovalDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult UpdateTimeSheetApprovalStatus(List<ApproverData> AppData, string ButtonClicked)
        {
            try
            {
                TimeSheetApprovalModel model = new TimeSheetApprovalModel();
                TaskTimesheetDAL taskTimeSheetDAL = new TaskTimesheetDAL();
                SemDAL semDal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                SearchedUserDetails employeeDetails = semDal.GetEmployeeDetails(model.SearchedUserDetails.EmployeeId);
                string LoggedInEmploeeName = employeeDetails.EmployeeFullName;
                int EmpCode = Convert.ToInt32(employeeDetails.EmployeeCode);
                string fromEmployeeID = taskTimeSheetDAL.GetEmployeeIdFromEmployeeCodeSEM(EmpCode);
                int[] EmailToEmployeeIds = new int[] { };
                ArrayList Toempids = new ArrayList();
                bool response = false;
                if (AppData != null)
                {
                    response = taskTimeSheetDAL.UpdateTimeSheetApprovalStatus(AppData, ButtonClicked, EmpCode);
                }
                if (response)
                {
                    foreach (var item in AppData)
                    {
                        if (!Toempids.Contains((item.ResourceID)))
                            Toempids.Add(item.ResourceID);
                    }
                    EmailToEmployeeIds = Toempids.ToArray(typeof(int)) as int[];
                    List<TemplateHandling> th = new List<TemplateHandling>();
                    List<TemplateHandling> th1 = new List<TemplateHandling>();
                    th.Add(new TemplateHandling("##loggedinuser##", LoggedInEmploeeName));
                    if (ButtonClicked == "Approve")
                        TimeSheetSendMail(Convert.ToInt32(fromEmployeeID), EmailToEmployeeIds, new int[] { }, th, th1, "", "", 88);
                    else if (ButtonClicked == "Reject")
                        TimeSheetSendMail(Convert.ToInt32(fromEmployeeID), EmailToEmployeeIds, new int[] { }, th, th1, "", "", 90);
                }

                return Json(new { isUpdated = true, isApproved = ButtonClicked }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        private GridView bindGrid = new GridView();

        public void ExportToExcelTimeSheetApprovalData(int? ProjectID, int? ResourceID, int? StatusID, DateTime? StartDate, DateTime? EndDate, string TimeSheetIds, int page, int rows)
        {
            WSEMDBEntities dbContext = new WSEMDBEntities();
            DataSet dsCurrentDetails = new DataSet();
            SemDAL dal = new SemDAL();
            TaskTimesheetDAL taskTimeSheetDAL = new TaskTimesheetDAL();
            List<TimeSheetApprovalDetailsModel> timesheetApprovalDetails = new List<TimeSheetApprovalDetailsModel>();
            timesheetApprovalDetails = taskTimeSheetDAL.GetTimeSheetAprrovalGridDetails(ProjectID, ResourceID, StatusID, StartDate, EndDate, page, rows);

            if (TimeSheetIds != string.Empty)
            {
                string[] ids = { };
                ids = TimeSheetIds.Split(',');

                timesheetApprovalDetails = timesheetApprovalDetails.Where(s => ids.Contains(Convert.ToString(s.TimeSheetID))).ToList();
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Project", typeof(string));
            dt.Columns.Add("Resource", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Task", typeof(string));
            dt.Columns.Add("Start Date", typeof(string));
            dt.Columns.Add("End Date", typeof(string));
            dt.Columns.Add("Hours", typeof(string));
            dt.Columns.Add("Units", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Comments", typeof(string));
            dt.Columns.Add("Approver Comments", typeof(string));

            if (timesheetApprovalDetails != null)
            {
                foreach (var item in timesheetApprovalDetails)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = item.ProjectName;
                    dr[1] = item.ResourceName;
                    dr[2] = item.Date;
                    dr[3] = item.Task;
                    dr[4] = item.StartDate;
                    dr[5] = item.EndDate;
                    dr[6] = item.Hours;
                    dr[7] = item.Units;
                    dr[8] = item.Status;
                    dr[9] = item.Comments;
                    dr[10] = item.ApproverComments;
                    dt.Rows.Add(dr);
                }
                dsCurrentDetails.Tables.Add(dt);
                bindGrid.DataSource = dsCurrentDetails;
                bindGrid.DataBind();
            }

            //using (StreamWriter writer = new StreamWriter("C:\\Temp\\dump.csv"))
            //{
            //    WriteDataTable(dt, writer, true);
            //}

            string strFileNameTo = Convert.ToString(System.DateTime.Now.ToShortDateString().ToString());
            string strFileName = "V2_TimeSheetAprrovalData_" + "_" + strFileNameTo.ToString().Replace("/", "-");
            strFileName = strFileName + ".xls";
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename = " + strFileName);
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            bindGrid.SetRenderMethodDelegate(new RenderMethod(RenderTitleCurrent));
            bindGrid.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());
            Response.End();
        }

        //added by Rahul Ramachandran for implementing Export to excel functionality
        public void ExportToExcelTimeSheetData(int? ProjectID, int? TaskID, int? StatusID, int? EmployeeID, DateTime? StartDate, DateTime? EndDate, int page, int rows)
        {
            WSEMDBEntities dbContext = new WSEMDBEntities();
            DataSet dsCurrentDetails = new DataSet();
            SemDAL dal = new SemDAL();
            TaskTimesheetDAL taskTimeSheetDAL = new TaskTimesheetDAL();

            int totalcount = 0;
            List<TimesheetModel> Entrydetails = new List<TimesheetModel>();
            Entrydetails = taskTimeSheetDAL.TimesheetEntryRecords(ProjectID, TaskID, StartDate, EndDate, StatusID, EmployeeID, page, rows, out totalcount);

            DataTable dt = new DataTable();
            dt.Columns.Add("Project", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("TaskName", typeof(string));
            dt.Columns.Add("Hours", typeof(string));
            dt.Columns.Add("Units", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Comments", typeof(string));
            dt.Columns.Add("Approver Comments", typeof(string));

            if (Entrydetails != null)
            {
                foreach (var item in Entrydetails)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = item.ProjectName;
                    dr[1] = item.Date;
                    dr[2] = item.TaskName;
                    dr[3] = item.Hours;
                    dr[4] = item.Units;
                    dr[5] = item.Status;
                    dr[6] = item.Comments;
                    dr[7] = item.ApproverComments;
                    dt.Rows.Add(dr);
                }
                dsCurrentDetails.Tables.Add(dt);
                bindGrid.DataSource = dsCurrentDetails;
                bindGrid.DataBind();
            }

            string strFileNameTo = Convert.ToString(System.DateTime.Now.ToShortDateString().ToString());
            string strFileName = "V2_TimeSheetData_" + "_" + strFileNameTo.ToString().Replace("/", "-");
            strFileName = strFileName + ".xls";
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename = " + strFileName);
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            bindGrid.SetRenderMethodDelegate(new RenderMethod(RenderTitleCurrent));
            bindGrid.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());
            Response.End();
        }

        public virtual void RenderTitleCurrent(HtmlTextWriter writer, Control ctl)
        {
            writer.AddAttribute("colspan", bindGrid.Columns.Count.ToString(System.Globalization.CultureInfo.InvariantCulture));
            writer.AddAttribute("align", "center");
            writer.RenderBeginTag("TD");
            writer.Write(" Timesheet Approval Data Details");
            writer.RenderEndTag();
            bindGrid.HeaderStyle.AddAttributesToRender(writer);
            writer.RenderBeginTag("TR");
            writer.RenderEndTag();
            foreach (Control control in ctl.Controls)
            {
                control.RenderControl(writer);
            }
        }

        #region UploadDataToDbFromExcel

        public static int CurrentEmployeeInfo(string Todo)
        {
            SemDAL semDal = new SemDAL();
            int EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
            if (Todo == "EmployeeId")
            {
                return EmployeeId;
            }
            else
            {
                var Data = semDal.GetEmployeeDetails(EmployeeId);
                return int.Parse(Data.EmployeeCode);
            }
        }

        public string UploadFileLocationPMS
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadTemplateForTaskCreation"];
                // return "Wrong Path";
            }
        }

        public string Generate_Unique_Character()
        {
            string mchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var chars = mchars;
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 4)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result.ToString();
        }

        public string Get_Unique_Name(string filename)
        {
            string[] holdval = filename.Split('.');
            string fistname = holdval[0] + "_" + Generate_Unique_Character();
            string extension = holdval[1];
            string Newname = fistname + "." + extension;
            return Newname;
        }

        public ArrayList GetEmployeeId_List(ArrayList TempEmpName)
        {
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            ArrayList ar = new ArrayList();
            foreach (var val in TempEmpName)
            {
                ar.Add(dal.GetEmployeeId(val.ToString()));
            }
            return ar;
        }

        public ArrayList GetTagId_List(ArrayList TempTagId, string ProjectId)
        {
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            ArrayList ar = new ArrayList();
            foreach (var val in TempTagId)
            {
                ar.Add(dal.GetTagsId(val.ToString(), ProjectId));
            }
            return ar;
        }

        public bool CheckIfEmpty(bool checkStatus, string filePath, string ext)
        {
            string name_query; bool status = true;
            try
            {
                string connectionString = "";
                if (checkStatus)
                {
                    if (ext == ".xls")
                    {
                        connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (ext == ".xlsx")
                    {
                        connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                    }
                    OleDbConnection excelConnection = new OleDbConnection(connectionString);
                    if (excelConnection.State == ConnectionState.Closed)
                    {
                        excelConnection.Open();
                    }

                    var tableschema = excelConnection.GetSchema("Tables");
                    DataTable activityDataTable = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    var itemsOfWorksheet = new List<SelectListItem>(); ;
                    if (activityDataTable != null)
                    {
                        string worksheetName;
                        for (int cnt = 0; cnt < activityDataTable.Rows.Count; cnt++)
                        {
                            worksheetName = activityDataTable.Rows[cnt]["TABLE_NAME"].ToString();

                            if (worksheetName.Contains('\''))
                            {
                                worksheetName = worksheetName.Replace('\'', ' ').Trim();
                            }
                            if (worksheetName.Trim().EndsWith("$"))
                                itemsOfWorksheet.Add(new SelectListItem { Text = worksheetName.TrimEnd('$'), Value = worksheetName });
                        }
                    }
                    string sheetname = "";
                    int[] count = { };
                    for (int i = 0; i < 1; i++)
                    {
                        sheetname = itemsOfWorksheet[i].Value.ToString();
                        name_query = "Select * FROM [" + sheetname + "]";
                        OleDbCommand cmd = new OleDbCommand(name_query, excelConnection);
                        OleDbDataReader dReader;
                        dReader = cmd.ExecuteReader();
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        dt.Load(dReader);
                        ds.Tables.Add(dt);
                        OleDbCommand cmd1 = new OleDbCommand(name_query, excelConnection);
                        OleDbDataReader dReader1;
                        dReader1 = cmd.ExecuteReader();
                        while (dReader1.Read())
                        {
                            if (dReader1.GetValue(1).ToString() == "" || dReader1.GetValue(2).ToString() == "" || dReader1.GetValue(3).ToString() == "" || dReader1.GetValue(4).ToString() == "")
                            {
                                return false;
                            }
                            else
                                return true;
                        }
                    }
                    excelConnection.Close();
                }
            }
            catch (Exception e)
            {
                status = false;
                throw;
            }

            return status;
        }

        public static string[] Join_DataToStringArray(ArrayList ar)
        {
            string[] data = new string[ar.Count];
            int i = 0;
            foreach (var s in ar)
            {
                data[i] = s.ToString();
                i++;
            }
            return data;
        }

        public string FileDataIsValid(string ext, string filePath)
        {
            string connectionString = "";
            if (true)
            {
                if (ext == ".xls")
                {
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                else if (ext == ".xlsx")
                {
                    connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                }
                OleDbConnection excelConnection = new OleDbConnection(connectionString);
                if (excelConnection.State == ConnectionState.Closed)
                {
                    excelConnection.Open();
                }
                var tableschema = excelConnection.GetSchema("Tables");
                DataTable activityDataTable = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                var itemsOfWorksheet = new List<SelectListItem>(); ;
                if (activityDataTable != null)
                {
                    string worksheetName;
                    for (int cnt = 0; cnt < activityDataTable.Rows.Count; cnt++)
                    {
                        worksheetName = activityDataTable.Rows[cnt]["TABLE_NAME"].ToString();

                        if (worksheetName.Contains('\''))
                        {
                            worksheetName = worksheetName.Replace('\'', ' ').Trim();
                        }
                        if (worksheetName.Trim().EndsWith("$"))
                            itemsOfWorksheet.Add(new SelectListItem { Text = worksheetName.TrimEnd('$'), Value = worksheetName });
                    }
                }

                try
                {
                    //Mail LOoop Begin from here
                    TaskTimesheetDAL dal = new TaskTimesheetDAL();
                    string sheetname = "";
                    for (int i = 0; i < 1; i++)
                    {
                        sheetname = itemsOfWorksheet[i].Value.ToString();
                        string name_query = "";
                        name_query = "Select * FROM [" + sheetname + "]";
                        OleDbCommand cmd = new OleDbCommand(name_query, excelConnection);
                        OleDbDataReader dReader;
                        dReader = cmd.ExecuteReader();
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        dt.Load(dReader);
                        ds.Tables.Add(dt);
                        OleDbCommand cmd1 = new OleDbCommand(name_query, excelConnection);
                        OleDbDataReader dReader1;
                        dReader1 = cmd.ExecuteReader();
                        int line = 0;
                        while (dReader1.Read())
                        {
                            if (dReader1.GetValue(1).ToString() == "")
                            {
                                //Session["BlankLine"] = "BlankLine";
                                return "Ok";
                            }
                            else
                            {
                                DateTime stDate;
                                DateTime endDate;
                                string TempEmpList = dReader1.GetValue(5).ToString();
                                string TempTags = dReader1.GetValue(6).ToString();
                                ArrayList TempEmployeeName = GetCommaSeperatedValues(TempEmpList);
                                ArrayList TempTagsArray = GetCommaSeperatedValues(TempTags);
                                string[] TempEmpStringArray = Join_DataToStringArray(TempEmployeeName);
                                string[] TempTagStringArray = Join_DataToStringArray(TempTagsArray);
                                string ProjectName = dReader1.GetValue(1).ToString();
                                string TaskName = dReader1.GetValue(0).ToString();
                                string Status = dReader1.GetValue(7).ToString();
                                DateTime StartDate = Convert.ToDateTime(dReader1.GetValue(2));
                                DateTime EndDate = Convert.ToDateTime(dReader1.GetValue(3));
                                string checkData = CheckIfDataIsValid(TaskName, ProjectName, StartDate, EndDate, TempEmpStringArray, TempTagStringArray, Status);
                                string checkTaskType = dal.GetLookUpStatusId(dReader1.GetValue(9).ToString(), null);
                                if (checkData == "TAsk Name Exist")
                                {
                                    //return "Task name " + TaskName + " exist in DataBase.";
                                    return "Task name " + TaskName + " exist in DataBase from " + (Convert.ToDateTime(StartDate)).ToShortDateString() + " to " + (Convert.ToDateTime(EndDate)).ToShortDateString();
                                }
                                if (checkData == "Invalid Tags")
                                {
                                    return "Invalid Tag " + TempData["InvalidTag"].ToString();
                                }
                                if (checkData == "Invalid Employee")
                                {
                                    return "Please verify assigned to " + TempData["InvalidEmployee"].ToString();
                                }
                                if (checkData == "Invalid Project")
                                {
                                    return "Please verify project name " + TempData["InvalidProject"].ToString();
                                }
                                if (checkData == "Invalid Status")
                                {
                                    return "Invalid status " + TempData["InvalidStatus"].ToString();
                                }
                                if (checkTaskType == "" || checkTaskType == null)
                                {
                                    checkData = "Invalid TaskType";
                                    return "Invalid Task Type " + dReader1.GetValue(9).ToString();
                                }
                                if (!DateTime.TryParse(dReader1.GetValue(2).ToString(), out stDate))
                                {
                                    checkData = "DateNotValid";
                                    return "Start date " + dReader1.GetValue(2).ToString() + " not valid";
                                }
                                if (!DateTime.TryParse(dReader1.GetValue(3).ToString(), out endDate))
                                {
                                    checkData = "DateNotValid";
                                    return "End date " + dReader1.GetValue(3).ToString() + " not valid";
                                }
                                if (stDate > endDate)
                                {
                                    checkData = "DateNotValid";
                                    return "Start date should be less than end date";
                                }
                                if (checkData == "Error")
                                {
                                    return "Error On Line No: " + (int.Parse(line.ToString()) + 1).ToString();
                                }
                                line++;
                            }
                        }
                    }
                    excelConnection.Close();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            return "Ok";
        }

        [HttpPost]
        public JsonResult UploadandSaveFile(HttpPostedFileBase fileIdUpload)
        {
            TaskTimesheetController tc = new TaskTimesheetController();
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            string uploadStatus = "";
            try
            {
                if (fileIdUpload != null)
                {
                    FileInfo Finfo = new FileInfo(fileIdUpload.FileName);
                    string extension = Finfo.Extension.ToLower();
                    if (extension == ".xlsx" || extension == ".xls")
                    {
                        string uploadsPath = (UploadFileLocationPMS);//
                        string fileName = Get_Unique_Name(Path.GetFileName(fileIdUpload.FileName));
                        string filePath = Path.Combine(uploadsPath, fileName).Replace("\\", "/");
                        try
                        {
                            if (!Directory.Exists(uploadsPath))
                                Directory.CreateDirectory(uploadsPath);

                            fileIdUpload.SaveAs(filePath);
                        }
                        catch (Exception e)
                        {
                            uploadStatus = "Error :" + e.Message;
                            return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
                        }
                        //To Check If File Is Valid
                        bool checkvalid = CheckIfFileIsValid(true, filePath, extension);
                        if (checkvalid)
                        {
                            //To Check If File Contains Rows.
                            bool checkifblank = CheckIfEmpty(true, filePath, extension);
                            if (checkifblank)
                            {
                                string CheckData = tc.FileDataIsValid(extension, filePath);
                                if (CheckData == "Ok")
                                {
                                    bool checks = SaveData(true, 0, extension, filePath);
                                    if (checks)
                                    {
                                        uploadStatus = "Done";
                                    }
                                    else
                                    {
                                        uploadStatus = "Unknown Error Occured";
                                    }
                                }
                                else
                                {
                                    if (CheckData == "Error")
                                        uploadStatus = CheckData;
                                    else
                                        uploadStatus = CheckData;

                                    if (CheckData == "Ok")
                                        uploadStatus = "Done";
                                }
                            }
                            else
                            {
                                uploadStatus = "File Is Blank";
                            }
                        }
                        else
                        {
                            uploadStatus = "Template Invalid";
                        }
                    }
                    else
                    {
                        uploadStatus = "NOt_Valid_File";
                    }
                }
                else
                {
                    uploadStatus = "File Not Selected";
                    return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                uploadStatus = e.StackTrace;
            }
            finally
            {
                System.GC.Collect();
            }
            return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
        }

        public bool CheckIfFileIsValid(bool checkStatus, string filePath, string ext)
        {
            string name_query; bool status = false;
            int check_counter = 0;
            try
            {
                string connectionString = "";
                if (checkStatus)
                {
                    if (ext == ".xls")
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (ext == ".xlsx")
                    {
                        connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                    }
                    OleDbConnection excelConnection = new OleDbConnection(connectionString);
                    if (excelConnection.State == ConnectionState.Closed)
                    {
                        excelConnection.Open();
                    }
                    var tableschema = excelConnection.GetSchema("Tables");
                    DataTable activityDataTable = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    var itemsOfWorksheet = new List<SelectListItem>(); ;
                    if (activityDataTable != null)
                    {
                        string worksheetName;
                        for (int cnt = 0; cnt < activityDataTable.Rows.Count; cnt++)
                        {
                            worksheetName = activityDataTable.Rows[cnt]["TABLE_NAME"].ToString();

                            if (worksheetName.Contains('\''))
                            {
                                worksheetName = worksheetName.Replace('\'', ' ').Trim();
                            }
                            if (worksheetName.Trim().EndsWith("$"))
                                itemsOfWorksheet.Add(new SelectListItem { Text = worksheetName.TrimEnd('$'), Value = worksheetName });
                        }
                    }
                    string sheetname = "";
                    int[] count = { };
                    for (int i = 0; i < 1; i++)
                    {
                        sheetname = itemsOfWorksheet[i].Value.ToString();
                        name_query = "Select * FROM [" + sheetname + "]";
                        OleDbCommand cmd = new OleDbCommand(name_query, excelConnection);
                        OleDbDataReader dReader;
                        dReader = cmd.ExecuteReader();
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        dt.Load(dReader);
                        ds.Tables.Add(dt);
                        ArrayList columnName = new ArrayList();
                        foreach (DataTable table in ds.Tables)
                        {
                            foreach (DataColumn column in table.Columns)
                            {
                                columnName.Add(column.ColumnName);
                            }
                        }
                        if (columnName[0].ToString() == "Task Name" && columnName[1].ToString() == "Project Name" &&
                            columnName[2].ToString() == "Start Date" && columnName[3].ToString() == "End Date" && columnName[4].ToString() == "Planned Hours" && columnName[5].ToString() == "Assigned To" && columnName[6].ToString() == "Tags" && columnName[7].ToString() == "Status"
                            && columnName[8].ToString() == "Avg Unit Time" && columnName[9].ToString() == "Task Type" && columnName[10].ToString() == "Description")
                        {
                            check_counter++;
                        }
                    }
                    if (check_counter == 1)
                    {
                        status = true;
                    }
                    excelConnection.Close();
                }
            }
            catch (Exception e)
            {
                status = false;
                throw;
            }
            return status;
        }

        public static ArrayList GetCommaSeperatedValues(string temp)
        {
            string[] TempList = temp.Split(',');
            ArrayList TempArray = new ArrayList();
            if (TempList.Count() > 0)
            {
                for (int j = 0; j < TempList.Count(); j++)
                {
                    TempArray.Add(TempList[j].ToString());
                }
            }
            return TempArray;
        }

        public string CheckIfDataIsValid(string TaskName, string ProjectName, DateTime StartDate, DateTime EndDate, string[] EmployeeName, string[] Tags, string Status)
        {
            //Check If TaskName Is Valid
            bool checkEmployeeName = true;
            bool checktags = true;
            bool checkTaskname = dal.GetTaskExist(TaskName, StartDate, EndDate);
            if (checkTaskname)
            {
                return "TAsk Name Exist";
            }
            bool checkProjectExist = dal.GetProjectExist(ProjectName);
            if (!checkProjectExist)
            {
                TempData["InvalidProject"] = ProjectName.ToString();
                return "Invalid Project";
            }
            if (EmployeeName.Count() > 1)
            {
                for (int i = 0; i < EmployeeName.Count(); i++)
                {
                    bool TempCheckemployeeName = dal.GetEmployeeExist(EmployeeName[i].ToString());
                    if (TempCheckemployeeName == false)
                    {
                        TempData["InvalidEmployee"] = EmployeeName[i].ToString();
                        checkEmployeeName = false;
                        break;
                    }
                }
            }
            else
            {
                checkEmployeeName = dal.GetEmployeeExist(EmployeeName[0].ToString());
                if (!checkEmployeeName)
                    TempData["InvalidEmployee"] = EmployeeName[0].ToString();
            }
            bool checkStatus = dal.GetStatusExist(Status);
            if (!checkStatus)
            {
                TempData["InvalidStatus"] = Status.ToString();
                return "Invalid Status";
            }
            if (Tags.Count() > 1)
            {
                for (int i = 0; i < Tags.Count(); i++)
                {
                    bool TempCheckTags = dal.GetTagsExist(Tags[i].ToString());
                    if (TempCheckTags == false)
                    {
                        TempData["InvalidTag"] = Tags[i].ToString();
                        checktags = false;
                        break;
                    }
                }
            }
            else
            {
                checktags = dal.GetTagsExist(Tags[0].ToString());
                if (!checktags)
                    TempData["InvalidTag"] = Tags[0].ToString();
            }

            bool DbStatus = false;
            if (checkEmployeeName && checktags && checkTaskname == false && checkProjectExist && checkStatus)
            // if (checkEmployeeName && checktags && checkProjectExist && checkStatus)
            {
                DbStatus = true;
            }
            if (DbStatus)
            {
                return "ok";
            }
            if (!checktags)
            {
                return "Invalid Tags";
            }
            if (!checkEmployeeName)
            {
                return "Invalid Employee";
            }
            else
            {
                return "Error";
            }
        }

        public string join_Array(ArrayList ar)
        {
            string val = "";
            foreach (var s in ar)
            {
                val += s + ",";
            }
            return val.TrimEnd(',');
        }

        public bool SaveData(bool checkStatus, int ProjId, string ext, string filePath)
        {
            bool result = true;
            string connectionString = "";
            if (checkStatus)
            {
                if (ext == ".xls")
                {
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                else if (ext == ".xlsx")
                {
                    connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                }
                OleDbConnection excelConnection = new OleDbConnection(connectionString);
                if (excelConnection.State == ConnectionState.Closed)
                {
                    excelConnection.Open();
                }
                var tableschema = excelConnection.GetSchema("Tables");
                DataTable activityDataTable = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                var itemsOfWorksheet = new List<SelectListItem>(); ;
                if (activityDataTable != null)
                {
                    string worksheetName;
                    for (int cnt = 0; cnt < activityDataTable.Rows.Count; cnt++)
                    {
                        worksheetName = activityDataTable.Rows[cnt]["TABLE_NAME"].ToString();

                        if (worksheetName.Contains('\''))
                        {
                            worksheetName = worksheetName.Replace('\'', ' ').Trim();
                        }
                        if (worksheetName.Trim().EndsWith("$"))
                            itemsOfWorksheet.Add(new SelectListItem { Text = worksheetName.TrimEnd('$'), Value = worksheetName });
                    }
                }

                try
                {
                    //Mail LOoop Begin from here
                    TaskTimesheetDAL dal = new TaskTimesheetDAL();
                    string sheetname = "";
                    for (int i = 0; i < 1; i++)
                    {
                        sheetname = itemsOfWorksheet[i].Value.ToString();
                        string name_query = "";
                        name_query = "Select * FROM [" + sheetname + "]";
                        OleDbCommand cmd = new OleDbCommand(name_query, excelConnection);
                        OleDbDataReader dReader;
                        dReader = cmd.ExecuteReader();
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        dt.Load(dReader);
                        ds.Tables.Add(dt);
                        OleDbCommand cmd1 = new OleDbCommand(name_query, excelConnection);
                        OleDbDataReader dReader1;
                        dReader1 = cmd.ExecuteReader();
                        while (dReader1.Read())
                        {
                            string TempEmpList = dReader1.GetValue(5).ToString();
                            if (TempEmpList == "")
                            {
                                return true;
                            }
                            string TempTags = dReader1.GetValue(6).ToString();
                            ArrayList TempEmployeeName = GetCommaSeperatedValues(TempEmpList);
                            ArrayList TempTagsArray = GetCommaSeperatedValues(TempTags);
                            int? ProjectId = int.Parse(dal.GetProjectId(dReader1.GetValue(1).ToString()));
                            ArrayList TempEmployeeIDArray = GetEmployeeId_List(TempEmployeeName);
                            ArrayList TempTagIds = GetTagId_List(TempTagsArray, ProjectId.ToString());
                            string TaskName = dReader1.GetValue(0).ToString();
                            DateTime createdate = DateTime.Parse(dReader1.GetValue(2).ToString());
                            DateTime Enddate = DateTime.Parse(dReader1.GetValue(3).ToString());
                            decimal PlannedHours = (decimal.Parse(dReader1.GetValue(4).ToString()) * 60) / TempEmployeeIDArray.Count;
                            int Status = int.Parse(dal.GetLookUpStatusId(dReader1.GetValue(7).ToString(), ProjectId.ToString()));
                            int avgTime = int.Parse(dReader1.GetValue(8).ToString());
                            string TaskType = dal.GetLookUpStatusId(dReader1.GetValue(9).ToString(), null);
                            string Description = dReader1.GetValue(10).ToString();
                            foreach (var EmpId in TempEmployeeIDArray)
                            {
                                bool checkingInsert = dal.insert_to_db(CurrentEmployeeInfo("EmployeeCode").ToString(), TaskName, ProjectId, createdate, Enddate, PlannedHours, int.Parse(EmpId.ToString()), join_Array(TempTagIds), Status, avgTime, int.Parse(TaskType), 000, DateTime.Now, 000, DateTime.Now, Description, 0);
                            }
                            foreach (var Empname in TempEmployeeName)
                            {
                                //************ For Future Use *****************//
                                //    List<TemplateHandling> SubjectReplacemnt = new List<TemplateHandling>();
                                //    SubjectReplacemnt.Add(new TemplateHandling("##Project Name##", dReader1.GetValue(1).ToString()));
                                //    EmployeeList loggedEmployeeDetails = dal.GetLoggedInEmployeeDetailsByEmployeeId(TaskTimesheetDAL.GetCurrentUserLoggedOn());
                                //    List<TemplateHandling> MessageReplacemnt = new List<TemplateHandling>();
                                //    MessageReplacemnt.Add(new TemplateHandling("##Employee Name##", Empname.ToString()));
                                //    MessageReplacemnt.Add(new TemplateHandling("##Task Name##", TaskName));
                                //    MessageReplacemnt.Add(new TemplateHandling("##Project Name##", dReader1.GetValue(1).ToString()));
                                //    MessageReplacemnt.Add(new TemplateHandling("##logged in user##", loggedEmployeeDetails.EmployeeName));
                                //    string Res = TimeSheetSendMail(13853, new int[] { 14312 }, new int[] { }, MessageReplacemnt, SubjectReplacemnt, "", "", 89);
                            }
                        }
                    }
                    excelConnection.Close();
                }
                catch (Exception e)
                {
                    result = false;
                    throw;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        #endregion UploadDataToDbFromExcel

        public string ValidateEmail(ArrayList TempEmail)
        {
            string ErrorEmailAddress = "EmailsIdOk";
            foreach (string s in TempEmail)
            {
                if (!s.Contains("@v2solutions.com"))
                {
                    ErrorEmailAddress = s;
                    break;
                }
            }
            return ErrorEmailAddress;
        }

        public EmployeeMailTemplate Generate_Template_Structure(int TemplateNo, List<TemplateHandling> SendTemp, List<TemplateHandling> SubTemp)
        {
            EmployeeMailTemplate sendIt = new EmployeeMailTemplate();
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            var TemplateData = Commondal.GetEmailTemplate(TemplateNo);
            List<EmployeeMailTemplate> emt = TemplateData;
            foreach (var Te in emt)
            {
                sendIt.Subject = Te.Subject;
                sendIt.Message = Te.Message;
                break;
            }
            string WorkOnMessage = sendIt.Message;
            string WorkOnSubject = sendIt.Subject;

            //For Message
            foreach (TemplateHandling s in SendTemp)
            {
                WorkOnMessage = WorkOnMessage.Replace(s.Key, s.Value);
            }
            //For Subject
            foreach (TemplateHandling s in SubTemp)
            {
                WorkOnSubject = WorkOnSubject.Replace(s.Key, s.Value);
            }
            sendIt.Message = WorkOnMessage;
            sendIt.Subject = WorkOnSubject;
            return sendIt;
        }

        public string TimeSheetSendMail(int fromEmpId, int[] ToEmpID, int[] ToCc, List<TemplateHandling> Temp, List<TemplateHandling> SubTempRep, string Body, string Subject, int TemplateNo)
        {
            TaskTimesheetDAL Td = new TaskTimesheetDAL();
            string Status = "";
            try
            {
                //****** Future Use *********//
                //System.Net.NetworkCredential NTLMAuthentication;
                //NTLMAuthentication = new System.Net.NetworkCredential("smtp-relay@v2solutions.com", "test1234@");
                string FormEmployeeEmail = Td.Get_Employee_Emails(fromEmpId);
                SmtpClient smtpClient = new SmtpClient(ConfigurationSettings.AppSettings["SMTPServerName"].ToString());
                smtpClient.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["PortNumber"].ToString());
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //****** Future Use *********//
                //On CryptoGraphi Error
                ////ServicePointManager.ServerCertificateValidationCallback =
                ////delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                ////{ return true; };
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), ConfigurationSettings.AppSettings["SMTPPassword"].ToString());//("v2system", "mail_123");
                //smtpClient.Credentials = NTLMAuthentication;
                MailMessage objMailMessage = new MailMessage();
                //Changed by Rahul for issue ID #142667129.
                //objMailMessage.From = new MailAddress(FormEmployeeEmail, FormEmployeeEmail);
                objMailMessage.From = new MailAddress(ConfigurationSettings.AppSettings["SMTPUserName"].ToString(), FormEmployeeEmail);
                ArrayList ToListEmail = new ArrayList();
                ArrayList ToCCEmails = new ArrayList();
                //****** Future Use *********//

                #region cm1

                //if (ValidateEmail(ToListEmail) == "EmailsIdOk")
                //{
                //if (ToEmpID.Count() > 0)
                //{
                //foreach (var s in ToEmpID)
                //    ToListEmail.Add(Td.Get_Employee_Emails(s));

                //    //if (ValidateEmail(ToCCEmails) == "EmailsIdOk")
                //    //{
                //        string GetCollectionCCEmailToSend = join_Array(ToCCEmails);
                //        objMailMessage.CC.Add(GetCollectionCCEmailToSend);
                //    //}else
                //    //{
                //    //    Status = "Some Emails Are Invalid";
                //    //}
                //}

                #endregion cm1

                if (ToEmpID.Count() > 0)
                {
                    foreach (var s in ToEmpID)
                        ToListEmail.Add(Td.Get_Employee_Emails(s));

                    string GetCollectionEmailToSend = join_Array(ToListEmail);
                    objMailMessage.To.Add(GetCollectionEmailToSend);
                }
                if (ToCc.Count() > 0)
                {
                    foreach (var s in ToEmpID)
                        ToCCEmails.Add(Td.Get_Employee_Emails(s));

                    string GetCollectionCCEmailToSend = join_Array(ToCCEmails);
                    objMailMessage.CC.Add(GetCollectionCCEmailToSend);
                }

                EmployeeMailTemplate et = new EmployeeMailTemplate();
                et = Generate_Template_Structure(TemplateNo, Temp, SubTempRep);

                objMailMessage.IsBodyHtml = true;

                if (Subject == "" || Subject == null)
                    objMailMessage.Subject = et.Subject;
                else
                    objMailMessage.Subject = Subject;

                //objMailMessage.IsBodyHtml = true;
                if (Body == null || Body == "")
                    objMailMessage.Body = et.Message;
                else
                    objMailMessage.Body = Body;

                smtpClient.Send(objMailMessage);
                Status = "EmailSend";

                #region cm2

                //}
                //else
                //{
                //    Status = "Some Emails Are Invalid";
                //}

                #endregion cm2
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
            return Status;
        }

        [HttpPost]
        public ActionResult GetSelectedProjectTasks(int? ProjectID)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                TimesheetModel model = new TimesheetModel();
                SemDAL semDAL = new SemDAL();
                bool CanUserCreateTask = false;
                int EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                SearchedUserDetails employeeDetails = semDAL.GetEmployeeDetails(EmployeeId);
                int EmpCode = Convert.ToInt32(employeeDetails.EmployeeCode);
                string EmpId = dal.GetEmployeeIdFromEmployeeCodeSEM(EmpCode);
                int EmployeeIDSEM = Convert.ToInt32(EmpId);
                ViewBag.TaskList = dal.getTaskName(ProjectID, EmployeeIDSEM);
                var CanUserCreateTaskValue = dal.GetDetailForCreatingTask(ProjectID);
                if (CanUserCreateTaskValue == "Yes")
                    CanUserCreateTask = true;

                return Json(new { ListData = ViewBag.TaskList, CanUserCreateTask = CanUserCreateTask }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetSelectedProjectSettings(int ProjectID)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                ViewBag.data = dal.getSettingName(ProjectID);
                return Json(new { ListData = ViewBag.data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetSelectedProjectTags(int ProjectID)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                TimesheetModel model = new TimesheetModel();

                ViewBag.TagList = dal.getTagsName(ProjectID);

                return Json(new { ListData = ViewBag.TagList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetSelectedProjectTaskDescription(int TaskId)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                TimesheetModel model = new TimesheetModel();

                ViewBag.TaskDescription = dal.getTaskDescription(TaskId);

                return Json(new { TaskDescription = ViewBag.TaskDescription }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveTimeSheetEntryDetailsGridData(TimesheetModel model, string LoggedUserName, int? SelectedEntryId)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                SEMResponse response = new SEMResponse();
                response = dal.SaveTimeSheetEntryRecordGridData(model, LoggedUserName, SelectedEntryId);
                return Json(new { status = response.status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteTimesheetEntryDetails(string[] SelectedEntryId)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                bool status = dal.DeleteTimesheetEntryRecord(SelectedEntryId);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetTypeOfSelectedPMSSettings(string SettingType)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                TimesheetModel model = new TimesheetModel();

                ViewBag.listdata = dal.GetPMSSettingType(SettingType);

                return Json(new { ListData = ViewBag.listdata }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SavePMSConfigurationSettingDetails(PmsConfiguration model)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                bool status = false;
                status = dal.SavePMSConfigurationSettingRecord(model);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownLoadTemplateForTaskCreation()
        {
            try
            {
                var template = "Template.xlsx";

                string[] FileExtention = template.Split('.');
                string contentType = "application/" + FileExtention[1];
                string uploadsPath = (DownLoadTemplateForTaskCreations);
                //uploadsPath = Path.Combine(uploadsPath, (ProjectId).ToString());
                string Filepath = Path.Combine(uploadsPath);
                if (System.IO.File.Exists(Filepath))
                    return File(Filepath, contentType, template);
                else
                    throw new Exception();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        //public void ExportToExcelTaskCreationData(int? ProjectID, int? MileStoneId, int? SelectedStatusID, int SelectedAssignedEmployeeId, int page, int rows)
        //{
        //    AddProjectTask model = new AddProjectTask();
        //    TaskTimesheetDAL dal = new TaskTimesheetDAL();
        //    DataSet dsCurrentDetails = new DataSet();
        //    int totalCount;
        //    List<AddProjectTask> taskDetails = new List<AddProjectTask>();
        //    taskDetails = dal.ProjectTaskDetailRecord(ProjectID, MileStoneId, SelectedStatusID, SelectedAssignedEmployeeId, page, rows, out totalCount);
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("TaskName", typeof(string));
        //    dt.Columns.Add("StartDate", typeof(string));
        //    dt.Columns.Add("EndDate", typeof(string));
        //    dt.Columns.Add("PlannedHours", typeof(string));
        //    dt.Columns.Add("AssignedToName", typeof(string));
        //    dt.Columns.Add("TagID", typeof(string));
        //    dt.Columns.Add("StatusValue", typeof(string));
        //    dt.Columns.Add("AvgUnitTime", typeof(string));
        //    dt.Columns.Add("TaskTypeName", typeof(string));
        //    dt.Columns.Add("Description", typeof(string));
        //    if (taskDetails != null)
        //    {
        //        foreach (var item in taskDetails)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = item.TaskName;
        //            dr[1] = item.StartDate;
        //            dr[2] = item.EndDate;
        //            dr[3] = item.PlannedHours;
        //            dr[4] = item.AssignedToName;
        //            dr[5] = item.TagID;
        //            dr[6] = item.StatusValue;
        //            dr[7] = item.AvgUnitTime;
        //            dr[8] = item.TaskTypeName;
        //            dr[9] = item.Description;
        //            dt.Rows.Add(dr);
        //        }
        //        dsCurrentDetails.Tables.Add(dt);
        //        bindGrid.DataSource = dsCurrentDetails;
        //        bindGrid.DataBind();
        //    }
        //    string strFileNameTo = Convert.ToString(System.DateTime.Now.ToShortDateString().ToString());
        //    string strFileName = "V2_TimeSheetAprrovalData_" + "_" + strFileNameTo.ToString().Replace("/", "-");
        //    strFileName = strFileName + ".xls";
        //    Response.Clear();
        //    Response.AddHeader("content-disposition", "attachment;filename = " + strFileName);
        //    Response.Buffer = true;
        //    Response.ContentType = "application/ms-excel";
        //    Response.Charset = "";
        //    System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        //    System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
        //    bindGrid.SetRenderMethodDelegate(new RenderMethod(RenderTitleCurrent));
        //    bindGrid.RenderControl(oHtmlTextWriter);
        //    Response.Write(oStringWriter.ToString());
        //    Response.End();
        //}

        [HttpPost]
        public ActionResult GetSelectedSettingsTimesheetApprover(string settings, int employeeID)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                ViewBag.data = dal.GetTimesheetApproverList(settings, employeeID);
                return Json(new { ListData = ViewBag.data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetSeperatePMSSettings(string SettingType, int ProjectID)
        {
            try
            {
                TaskTimesheetDAL dal = new TaskTimesheetDAL();
                TimesheetModel model = new TimesheetModel();

                ViewBag.listdata = dal.GetSeperatePMSSettingType(SettingType, ProjectID);

                return Json(new { ListData = ViewBag.listdata }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}