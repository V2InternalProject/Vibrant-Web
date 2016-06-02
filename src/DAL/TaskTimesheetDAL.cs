using HRMS.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Security;

namespace HRMS.DAL
{
    public class TaskTimesheetDAL
    {
        private WSEMDBEntities dbContext = new WSEMDBEntities();
        private HRMSDBEntities dbHRMSContext = new HRMSDBEntities();
        private WSEMDBEntities dbSEMContext = new WSEMDBEntities();

        private readonly String ConnectionString =
            ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        public List<ProjectList> GetProjectList(int employeeCode)
        {
            List<ProjectList> projectNames = new List<ProjectList>();
            var projectList = dbContext.GetActiveProjectList_SP(employeeCode);
            projectNames = (from s in projectList
                            orderby s.projectName ascending
                            select new ProjectList
                            {
                                ProjectID = s.projectId,
                                ProjectName = s.projectName
                            }).ToList();
            return projectNames.ToList();
        }

        public List<ProjectList> GetProjectListAsAprover(int employeeCode)
        {
            List<ProjectList> projectNames = new List<ProjectList>();
            var projectList = dbContext.GetProjectListOfUserAsApprover_SP(employeeCode);
            projectNames = (from s in projectList
                            orderby s.ProjectName ascending
                            select new ProjectList
                            {
                                ProjectID = s.ProjectID,
                                ProjectName = s.ProjectName
                            }).ToList();
            return projectNames.ToList();
        }

        public List<EmployeeDetails> ProjectNameForTimesheetApproval(int EmployeeCode, string searchText, int pageNo = 1, int pageSize = 20)
        {
            List<EmployeeDetails> projectNames = new List<EmployeeDetails>();
            var projectList = dbContext.GetProjectListOfUserAsApprover_SP(EmployeeCode);
            projectNames = (from s in projectList
                            where s.ProjectName.ToLower().Contains(searchText.ToLower())
                            orderby s.ProjectName ascending
                            select new EmployeeDetails
                            {
                                ProjectID = s.ProjectID,
                                ProjectName = s.ProjectName
                            }).ToList();
            return projectNames.ToList();
        }

        public List<ResourceList> GetResourceList(int EmployeeID)
        {
            List<ResourceList> resourceNames = new List<ResourceList>();
            try
            {
                var employeeList = dbContext.GetEmployeeListForTimeSheet_SP(EmployeeID);
                resourceNames = (from m in employeeList
                                 select new ResourceList
                                 {
                                     ResourceID = m.Employeeid,
                                     ResourceName = m.EmployeeName
                                 }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return resourceNames;
        }

        public List<EmployeeDetails> GetResourceList1(int EmployeeID, string searchText, int pageNo = 1, int pageSize = 20)
        {
            List<EmployeeDetails> resourceNames = new List<EmployeeDetails>();
            try
            {
                var employeeList = dbContext.GetEmployeeListForTimeSheet_SP(EmployeeID);
                resourceNames = (from m in employeeList
                                 where m.EmployeeName.ToLower().Contains(searchText.ToLower())
                                 select new EmployeeDetails
                                 {
                                     ResourceID = m.Employeeid,
                                     ResourceName = m.EmployeeName
                                 }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return resourceNames;
        }

        public List<StatusList> GetStatusList()
        {
            try
            {
                List<StatusList> statusNames = new List<StatusList>();
                List<MasterDataModel> taskTypeRecords = new List<MasterDataModel>();
                var typeList = dbContext.GetMasterDataDetails_SP();
                statusNames = (from m in typeList
                               orderby m.LookUpTypeId ascending
                               where m.dataTypeValue == "TimeSheet Approval"
                               select new StatusList
                               {
                                   StatusID = m.LookUpTypeId,
                                   Status = m.Value
                               }).ToList();
                return statusNames.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetEmployeeIdFromEmployeeCodeSEM(int EmpCode)
        {
            try
            {
                var EmpCode1 = Convert.ToString(EmpCode);
                var SEMEmpId = (from SEMEmpDtls in dbContext.tbl_PM_Employee_SEM
                                where SEMEmpDtls.EmployeeCode == EmpCode1
                                select SEMEmpDtls.EmployeeID).FirstOrDefault();
                return SEMEmpId.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmployeeDetails> ProjectNameForTimesheet(string searchText, int pageNo = 1, int pageSize = 10)
        {
            List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();
            List<TimesheetModel.ProjectList> projectname = new List<TimesheetModel.ProjectList>();

            DateTime System_dt = DateTime.Now.Date;

            EmployeeDAL employeeDAL = new EmployeeDAL();
            string employeeCode = Membership.GetUser().UserName;

            var employeeID = (from SEMEmpDtls in dbSEMContext.tbl_PM_Employee_SEM
                              where SEMEmpDtls.EmployeeCode == employeeCode
                              select SEMEmpDtls.EmployeeID).FirstOrDefault();

            employeeDetails = (from prj in dbSEMContext.tbl_PM_Project
                               where (prj.GlobalProject != true && prj.ProjectName.ToLower().Contains(searchText.ToLower()))
                               select new EmployeeDetails
                               {
                                   EmployeeName = prj.ProjectName,
                                   EmployeeId = prj.ProjectID
                               }).ToList();
            return employeeDetails.ToList();
        }

        public List<TimeSheetApprovalDetailsModel> GetTimeSheetAprrovalGridDetails(int? ProjectID, int? ResourceID, int? StatusID, DateTime? StartDate, DateTime? EndDate, int page, int rows)
        {
            try
            {
                var EmployeeID = Convert.ToInt32(GetEmployeeIdFromEmployeeCodeSEM(Convert.ToInt32(Membership.GetUser().UserName)));
                var projectApprovalDetails = dbContext.GetTimeSheetApprovalGridData_SP(Convert.ToInt32(Membership.GetUser().UserName), ProjectID, ResourceID, StatusID, StartDate, EndDate);
                List<TimeSheetApprovalDetailsModel> TimeSheetApprovalResults = (from i in projectApprovalDetails
                                                                                where i.ResourceID != EmployeeID
                                                                                select new TimeSheetApprovalDetailsModel
                                                                                {
                                                                                    TimeSheetID = i.TimeSheetId,
                                                                                    ProjectID = i.ProjectID,
                                                                                    ProjectName = i.Project,
                                                                                    ResourceID = i.ResourceID,
                                                                                    ResourceName = i.Resource,
                                                                                    Date = i.Date,
                                                                                    Task = i.Task,
                                                                                    StartDate = i.StartDate,
                                                                                    EndDate = i.EndDate,
                                                                                    Hours = i.Hours,
                                                                                    Units = i.Units,
                                                                                    IsApproved = i.Is_Approved,
                                                                                    Status = i.Status,
                                                                                    Comments = i.Comment,
                                                                                    ApproverComments = i.ApproverComments
                                                                                }).ToList();
                for (int i = 0; i < TimeSheetApprovalResults.Count; i++)
                {
                    TimeSheetApprovalResults[i].Hours = ConvertMinutesToHours(TimeSheetApprovalResults[i].Hours);
                }
                var totalCount = TimeSheetApprovalResults.Count();
                return TimeSheetApprovalResults.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateTimeSheetApprovalStatus(List<ApproverData> AppData, string ButtonClicked, int EmpCode)
        {
            try
            {
                bool status = false;
                int TimeSheetID = 0;
                string ApproverComments = "";
                for (int i = 0; i < AppData.Count; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                    TimeSheetID = Convert.ToInt32(AppData[i].TimeSheetID);
                    ApproverComments = AppData[i].ApproverComments;
                    if (ApproverComments == null)
                        ApproverComments = "-";
                    dbContext.UpdateTimeSheetApprovalStatus_SP(EmpCode, TimeSheetID, ButtonClicked, ApproverComments, Output);
                    status = Convert.ToBoolean(Output.Value);
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TimesheetTagModel> TimesheetTagDetailRecord(int? projectID, int page, int rows, out int totalCount)
        {
            try
            {
                List<TimesheetTagModel> tagRecords = new List<TimesheetTagModel>();
                var tagDetails = dbContext.GetTimesheetTagDetails_sp(projectID);

                tagRecords = (from tag in tagDetails
                              select new TimesheetTagModel
                              {
                                  TagNameId = tag.TagId,
                                  ProjectID = tag.RefId,
                                  TagName = tag.TagName,
                                  TagStartDate = tag.StartDate,
                                  TagEndDate = tag.EndDate,
                                  TagLevel = tag.LevelName,
                                  HiddenTagLevel = tag.LevelName
                              }).ToList();

                totalCount = tagRecords.Count();
                return tagRecords.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SEMResponse SaveTagConfigurationRecord(TimesheetTagModel model, string LoggedUserName, int? ProjectId, string SelectedTagName)
        {
            try
            {
                SEMResponse response = new SEMResponse();
                response.isTagNameExist = false;
                response.status = false;
                var tagRecords = dbContext.GetTimesheetTagDetails_sp(ProjectId);
                var tagDetails = (from m in tagRecords
                                  where m.TagName.ToLower() == model.TagName.ToLower()
                                  select new TimesheetTagModel
                                  {
                                      TagName = m.TagName,
                                      ProjectID = m.RefId
                                  }).ToList();
                if (tagDetails.Count == 0 || (model.TagName == SelectedTagName && model.TagNameId > 0))
                {
                    int TagId = model.TagNameId;
                    int? ProjectID = ProjectId;
                    string TagName = model.TagName;
                    string LevelName = model.TagLevel;
                    DateTime? StartDate = model.TagStartDate;
                    DateTime? EndDate = model.TagEndDate;

                    DateTime CreatedDate = DateTime.Now;

                    string Mode = "";
                    if (model.TagNameId == 0)
                        Mode = "INSERT";
                    else
                        Mode = "UPDATE";

                    ObjectParameter Result = new ObjectParameter("Result", typeof(int));

                    dbContext.AddUpdateTimesheetTagDetails_sp(TagId, ProjectID, TagName, StartDate, EndDate, LevelName, CreatedDate, LoggedUserName, Mode, Result);
                    response.status = Convert.ToBoolean(Result.Value);
                    return response;
                }
                else
                {
                    response.isTagNameExist = true;
                    return response;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteTimesheetTagRecord(string[] SelectedTagId)
        {
            try
            {
                bool status = false;
                for (int i = 0; i < SelectedTagId.Length; i++)
                {
                    ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                    int TagId = Convert.ToInt32(SelectedTagId[i]);
                    dbContext.DeleteTimesheetTagDetails_sp(TagId, Result);
                    status = Convert.ToBoolean(Result.Value);
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //WSEMDBEntities dbContext = new WSEMDBEntities();

        public ObjectResult DTimeSheetConfig()
        {
            return dbContext.sp_TimeSheetFDConfiguration();
        }

        /// <summary>
        /// Fill Grid Master Configurations
        /// </summary>
        /// <param name="type">Data From DropDown</param>
        /// <returns></returns>
        public List<PmsConfiguration> getRecordsConfiguration(string Type)
        {
            List<PmsConfiguration> Records = new List<PmsConfiguration>();
            try
            {
                var historyList = dbContext.sp_FillConfigurationGrid(Type);
                var test = historyList.AsEnumerable().ToList();
                Records = (from item in test
                           select new PmsConfiguration
                           {
                               ID = item.LookUpTypeId,
                               TypeValue = item.Value,
                               LevelType = item.LevelName,
                               Rfid = item.Ref_Id,
                               CreatedBy = item.CreatedBy,
                               CreatedDate = item.CreatedDate,
                               ModifiedBy = item.ModifiedBy,
                               ModifiedDate = item.ModifiedDate
                           }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return Records;
        }

        public ObjectResult DGetProjectname()
        {
            return dbContext.sp_FillprojectInfo();
        }

        public bool GetTaskExist(string TaskName, DateTime StartDate, DateTime EndDate)
        {
            ObjectParameter Output = new ObjectParameter("Status", typeof(string));
            dbContext.sp_SaveFromExcel_Validator("CheckIfTaskExist", "", "", TaskName, StartDate, EndDate, "", "", Output);
            string DataFrmDb = Output.Value.ToString();
            if (DataFrmDb == "StatusExist")
            {
                return true;
            }
            return false;
        }

        public bool GetProjectExist(string Projectname)
        {
            ObjectParameter Output = new ObjectParameter("Status", typeof(string));
            dbContext.sp_SaveFromExcel_Validator("CheckIfProjectExist", "", Projectname, "", null, null, "", "", Output);
            string DataFrmDb = Output.Value.ToString();
            if (DataFrmDb == "ProjectExist")
            {
                return true;
            }
            return false;
        }

        public bool GetEmployeeExist(string EmployeeName)
        {
            ObjectParameter Output = new ObjectParameter("Status", typeof(string));
            dbContext.sp_SaveFromExcel_Validator("CheckIfEmployeeExist", EmployeeName, "", "", null, null, "", "", Output);
            string DataFrmDb = Output.Value.ToString();
            if (DataFrmDb == "EmployeeExist")
            {
                return true;
            }
            return false;
        }

        public bool GetTagsExist(string tagName)
        {
            ObjectParameter Output = new ObjectParameter("Status", typeof(string));
            dbContext.sp_SaveFromExcel_Validator("CheckIfTagExist", "", "", "", null, null, tagName, "", Output);
            string DataFrmDb = Output.Value.ToString();
            if (DataFrmDb == "TagExist")
            {
                return true;
            }
            return false;
        }

        public bool GetStatusExist(string StatusName)
        {
            ObjectParameter Output = new ObjectParameter("Status", typeof(string));
            dbContext.sp_SaveFromExcel_Validator("CheckIfStatusExist", "", "", "", null, null, "", StatusName, Output);
            string DataFrmDb = Output.Value.ToString();
            if (DataFrmDb == "LookUpStatusExist")
            {
                return true;
            }
            return false;
        }

        public string GetProjectId(string Projectname)
        {
            ObjectParameter Output = new ObjectParameter("Status", typeof(string));
            dbContext.sp_SaveFromExcel_Validator("GetProjectId", "", Projectname, "", null, null, "", "", Output);
            string DataFrmDb = Output.Value.ToString();
            return DataFrmDb;
        }

        public string GetEmployeeId(string Employeename)
        {
            ObjectParameter Output = new ObjectParameter("Status", typeof(string));
            dbContext.sp_SaveFromExcel_Validator("GetEmployeeId", Employeename, "", "", null, null, "", "", Output);
            string DataFrmDb = Output.Value.ToString();
            return DataFrmDb;
        }

        //GetTagId
        public string GetLookUpStatusId(string StatusName, string ProjectId)
        {
            ObjectParameter Output = new ObjectParameter("Status", typeof(string));
            dbContext.sp_SaveFromExcel_Validator("GetLookUpStatusId", "", ProjectId, "", null, null, "", StatusName, Output);
            string DataFrmDb = Output.Value.ToString();
            return DataFrmDb;
        }

        public string GetTagsId(string TagName, string ProjectId)
        {
            ObjectParameter Output = new ObjectParameter("Status", typeof(string));
            dbContext.sp_SaveFromExcel_Validator("GetTagId", "", ProjectId, "", null, null, TagName, "", Output);
            string DataFrmDb = Output.Value.ToString();
            return DataFrmDb;
        }

        public bool insert_to_db(string EmployeeCode, string TaskName, int? ProjectId, DateTime? StartDate, DateTime? EndDate, decimal? PlannedHours, int? AssignedTo, string TagID, int? Status, int? AvgUnitTime, int tasktypeId, int? CreatedBy, DateTime CreatedDate, int? ModifiedBy, DateTime ModifiedDAte, string Description, int? MileStoneID)
        {
            ObjectParameter Output = new ObjectParameter("DbDone", typeof(int));
            dbContext.sp_InsertFromExcelToDb(TaskName, ProjectId, StartDate, EndDate, PlannedHours, AssignedTo, TagID, Status, AvgUnitTime, tasktypeId, GetLoggedInEmployeeDetails(EmployeeCode).EmployeeId, CreatedDate, null, null, Description, null, Output);
            int Exist = Convert.ToInt32(Output.Value);
            if (Exist == 0)
            {
                return true;
            }

            return false;
        }

        public static int GetCurrentUserLoggedOn()
        {
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            return dal.GetLoggedInEmployeeDetails(Membership.GetUser().UserName.ToString()).EmployeeId;
        }

        public string SaveConfigurationData(string MainType, string TypeValue, string LevelTypeDrop, string ProjectId, string MainDropDown, string Todo, string SUniqueId)
        {
            int? TempProjectId = Convert.ToInt32(ProjectId);
            int? TempUniqueId = Convert.ToInt32(SUniqueId);
            string StatusFromDb = "";
            ObjectParameter Output = new ObjectParameter("Status", typeof(int));
            if (Todo == "MainTypeInsert")
            {
                dbContext.Sp_SaveConfigurationStatusData("MainStatus", MainType, TypeValue, LevelTypeDrop, TempProjectId, MainDropDown, GetCurrentUserLoggedOn(), 0, Output);
                int Exist = Convert.ToInt32(Output.Value);
                if (Exist == 0)
                {
                    Output.Value = null;
                    dbContext.Sp_SaveConfigurationStatusData("MainAdd", MainType, TypeValue, LevelTypeDrop, TempProjectId, MainDropDown, GetCurrentUserLoggedOn(), 0, Output);
                    if (Convert.ToInt32(Output.Value) == 0)
                    {
                        StatusFromDb = "Done";
                    }
                    else
                    {
                        StatusFromDb = "Error";
                    }
                }
                else if (Exist == 2)
                    StatusFromDb = "AlreadyExist";
                else
                    StatusFromDb = "Error";

                Output.Value = null;
            }
            else if (Todo == "SubTypeInsert")
            {
                dbContext.Sp_SaveConfigurationStatusData("CheckSubStatus", MainType, TypeValue, LevelTypeDrop, TempProjectId, MainDropDown, GetCurrentUserLoggedOn(), 0, Output);
                int Exist = Convert.ToInt32(Output.Value);
                if (Exist == 0)
                {
                    Output.Value = null;
                    dbContext.Sp_SaveConfigurationStatusData("SubAdd", MainType, TypeValue, LevelTypeDrop, TempProjectId, MainDropDown, GetCurrentUserLoggedOn(), 0, Output);
                    if (Convert.ToInt32(Output.Value) == 0)
                    {
                        StatusFromDb = "Done";
                    }
                    else
                    {
                        StatusFromDb = "Error";
                    }
                }
                else if (Exist == 2)
                    StatusFromDb = "AlreadyExist";
                else
                    StatusFromDb = "Error";

                Output.Value = null;
            }
            else
            {
                dbContext.Sp_SaveConfigurationStatusData("CheckSubStatus", MainType, TypeValue, LevelTypeDrop, TempProjectId, MainDropDown, GetCurrentUserLoggedOn(), 0, Output);
                int Exist = Convert.ToInt32(Output.Value);
                if (Exist == 0)
                {
                    Output.Value = null;
                    dbContext.Sp_SaveConfigurationStatusData("SubEdit", MainType, TypeValue, LevelTypeDrop, TempProjectId, MainDropDown, GetCurrentUserLoggedOn(), TempUniqueId, Output);
                    if (Convert.ToInt32(Output.Value) == 0)
                    {
                        StatusFromDb = "Done";
                    }
                    else
                    {
                        StatusFromDb = "Error";
                    }
                }
                else if (Exist == 2)
                    StatusFromDb = "NoChanges";
                else
                    StatusFromDb = "Error";

                Output.Value = null;
            }

            return StatusFromDb;
        }

        public bool DeleteConfigurationData(string id)
        {
            int cnv_Id = Convert.ToInt32(id);
            ObjectParameter Output = new ObjectParameter("Status", typeof(int));
            dbContext.Sp_SaveConfigurationStatusData("Delete", "", "", "", 0, "", GetCurrentUserLoggedOn(), cnv_Id, Output);

            if (Convert.ToInt32(Output.Value) == 1)
            {
                return false;
            }
            return true;
        }

        public List<TimesheetModel> TimesheetEntryRecords(int? ProjectID, int? TaskID, DateTime? SelectedFromDate, DateTime? SelectedToDate, int? StatusID, int? EmployeeId, int page, int rows, out int totalCount)
        {
            try
            {
                List<TimesheetModel> Entryrecords = new List<TimesheetModel>();

                //Changed By Rahul R to avoid the anonymous functioning.
                var Entrydetails = dbContext.GetTimesheetDetails_sp(ProjectID, TaskID, SelectedFromDate, SelectedToDate, StatusID);
                Entryrecords = Entrydetails.Where(x => x.EmployeeId == EmployeeId).Select(e => new TimesheetModel
                                {
                                    TimeSheetId = e.TimeSheetId,
                                    ProjectName = e.ProjectName,
                                    ProjectID = e.projectId,
                                    TaskName = e.TaskName,
                                    Hours = e.Hours,
                                    Comments = e.Comment,
                                    Units = e.Units,
                                    Date = e.Date,
                                    ProjectTaskTypeId = e.ProjectTaskTypeId,
                                    EmployeeId = e.EmployeeId,
                                    Status = e.Value,
                                    ApproverComments = e.ApproverComments
                                }).ToList<TimesheetModel>();
                //Entryrecords = (from e in Entrydetails
                //                where e.EmployeeId == EmployeeId
                //                select new TimesheetModel
                //                {
                //                    TimeSheetId = e.TimeSheetId,
                //                    ProjectName = e.ProjectName,
                //                    ProjectID = e.projectId,
                //                    TaskName = e.TaskName,
                //                    Hours = e.Hours,
                //                    Comments = e.Comment,
                //                    Units = e.Units,
                //                    Date = e.Date,
                //                    ProjectTaskTypeId = e.ProjectTaskTypeId,
                //                    EmployeeId = e.EmployeeId,
                //                    Status = e.Value,
                //                    ApproverComments = e.ApproverComments

                //                }).ToList();

                for (int i = 0; i < Entryrecords.Count; i++)
                {
                    Entryrecords[i].Hours = ConvertMinutesToHours(Entryrecords[i].Hours);
                }

                totalCount = Entryrecords.Count();

                return Entryrecords.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TimesheetModel.ProjectList> getProjectName()
        {
            List<TimesheetModel.ProjectList> projectname = new List<TimesheetModel.ProjectList>();
            var projectnames = dbContext.GetProjectName_SP();
            projectname = (from r in projectnames
                           select new TimesheetModel.ProjectList
                           {
                               ProjectID = r.ProjectId,
                               ProjectName = r.ProjectName
                           }).ToList();
            return projectname;
            //List<TimesheetModel.ProjectList> projectNames = new List<TimesheetModel.ProjectList>();
            //var projectList = dbContext.GetAllocatedProjectListOfUser_SP(employeeCode);
            //projectNames = (from s in projectList
            //                orderby s.ProjectName ascending
            //                select new TimesheetModel.ProjectList
            //                {
            //                    ProjectID = s.ProjectID,
            //                    ProjectName = s.ProjectName
            //                }).ToList();
            //return projectNames.ToList();
        }

        public List<TimesheetModel.TimesheetStatusList> getTimeSheetStatusNames()
        {
            List<TimesheetModel.TimesheetStatusList> statusname = new List<TimesheetModel.TimesheetStatusList>();
            var statusnames = dbContext.GetTimesheetStatusNames_SP();
            statusname = (from r in statusnames
                          select new TimesheetModel.TimesheetStatusList
                          {
                              StatusID = r.LookUpTypeId,
                              StatusName = r.value
                          }).ToList();
            return statusname;
        }

        #region TaskCreation

        public List<AddProjectTask> ProjectTaskDetailRecord(int? projectID, int? MileStoneId, int? SelectedStatusID, int? SelectedAssignedEmployeeId, int page, int rows, out int totalCount)
        {
            try
            {
                List<AddProjectTask> taskRecords = new List<AddProjectTask>();
                //var customerList = dbContext.GetManagePhaseDetails_SP(projectID);
                var taskDetails = dbContext.GetProjectTaskDetails_SP(projectID, MileStoneId, SelectedStatusID, SelectedAssignedEmployeeId);

                taskRecords = (from task in taskDetails
                               select new AddProjectTask
                               {
                                   ProjectTaskTypeID = task.ProjectTaskTypeID,
                                   TaskName = task.TaskName,
                                   ProjectId = task.ProjectId,
                                   StartDate = task.StartDate,
                                   EndDate = task.EndDate,
                                   PlannedHours = task.PlannedHours,
                                   AssignedTo = task.AssignedTo,
                                   AssignedToName = task.AssignedToName,
                                   TagID = task.TagID,
                                   StatusID = task.Status,
                                   StatusValue = task.StatusValue,
                                   AvgUnitTime = task.AvgUnitTime,
                                   TaskTypeID = task.TaskTypeID,
                                   TaskTypeName = task.TaskType,
                                   Description = task.Description,
                                   ActualHours = task.ActualHrs,
                                   PlannedUnits = task.PlannedUnits,
                                   ProjectTaskType = task.ProjectTaskType,
                                   ProjectTaskTypeValue = task.ProjectTaskType == true ? "Yes" : "No"
                               }).ToList();

                for (int i = 0; i < taskRecords.Count; i++)
                {
                    taskRecords[i].PlannedHours = Convert.ToDecimal((Math.Round(Convert.ToDouble(taskRecords[i].PlannedHours / 60), 2)));
                    taskRecords[i].ActualHours = Convert.ToDecimal((Math.Round(Convert.ToDouble(taskRecords[i].ActualHours / 60), 2)));
                    string[] stringTagArray;
                    string SelectedTagName = "";
                    if (taskRecords[i].TagID != null)
                    {
                        stringTagArray = taskRecords[i].TagID.Split(',');
                        int[] selectedTagArray = Array.ConvertAll(stringTagArray, s => int.Parse(s));
                        TagListClass tagRecords = new TagListClass();

                        for (int j = 0; j < selectedTagArray.Length; j++)
                        {
                            var tagList = dbContext.GetTimesheetTagDetails_sp(projectID);
                            tagRecords = (from m in tagList
                                          where m.TagId == selectedTagArray[j]
                                          select new TagListClass
                                          {
                                              TagId = m.TagId,
                                              TagName = m.TagName
                                          }).FirstOrDefault();
                            if (tagRecords != null)
                            {
                                SelectedTagName += tagRecords.TagName + ',';
                            }
                            //SelectedTagName += tagRecords.TagName + ',';
                        }
                        if (SelectedTagName != "")
                        {
                            taskRecords[i].TagName = SelectedTagName.TrimEnd(',');
                        }
                    }
                    else
                    {
                        taskRecords[i].TagName = string.Empty;
                    }
                }

                totalCount = taskRecords.Count();
                //return moduleRecords.Skip((page - 1) * rows).Take(rows).ToList();
                return taskRecords.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MasterDataModel> GetTaskStatusRecord()
        {
            try
            {
                List<MasterDataModel> taskStatusRecords = new List<MasterDataModel>();
                var taskList = dbContext.GetTaskStatusDetails_SP();
                taskStatusRecords = (from m in taskList
                                     select new MasterDataModel
                                     {
                                         LookUpTypeId = m.LookUpTypeId,
                                         Type = m.Type,
                                         Value = m.Value
                                     }).ToList();
                return taskStatusRecords.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MasterDataModel> GetTaskTypeRecord()
        {
            try
            {
                List<MasterDataModel> taskTypeRecords = new List<MasterDataModel>();
                var typeList = dbContext.GetTaskTypeDetails_SP();
                taskTypeRecords = (from m in typeList
                                   select new MasterDataModel
                                   {
                                       LookUpTypeId = m.LookUpTypeId,
                                       Type = m.Type,
                                       Value = m.Value
                                   }).ToList();
                return taskTypeRecords.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? GetDefaultStatusId()
        {
            try
            {
                int? defaultStatusId = null;
                MasterDataModel statusRecords = new MasterDataModel();
                var typeList = dbContext.GetDefaultTaskStatusDetails_SP();
                statusRecords = (from m in typeList
                                 select new MasterDataModel
                                 {
                                     LookUpTypeId = m.LookUpTypeId,
                                     Type = m.Type,
                                     Value = m.Value
                                 }).FirstOrDefault();
                if (statusRecords != null)
                    defaultStatusId = statusRecords.LookUpTypeId;
                return defaultStatusId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmployeeList> GetActiveEmployeesRecord(int? ProjectId)
        {
            try
            {
                List<EmployeeList> employeeRecords = new List<EmployeeList>();

                var employeeList = dbContext.GetActiveEmployeeForPerticularProject_SP(ProjectId);
                employeeRecords = (from m in employeeList
                                   select new EmployeeList
                                   {
                                       EmployeeId = m.EmployeeID,
                                       EmployeeCode = m.Employeecode,
                                       EmployeeName = m.EmployeeName
                                   }).ToList();
                return employeeRecords.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmployeeList> GetActiveEmployeesRecord1(int? ProjectId)
        {
            //try
            //{
            //    List<EmployeeList> employeeRecords = new List<EmployeeList>();

            //    var employeeList = dbContext.GetActiveEmployeeForPerticularProject_SP(ProjectId);
            //    employeeRecords = (from m in employeeList
            //                       select new EmployeeList
            //                       {
            //                           EmployeeId = m.EmployeeID,
            //                           EmployeeCode = m.Employeecode,
            //                           EmployeeName = m.EmployeeName
            //                       }).ToList();
            //    return employeeRecords.ToList();
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

            var employeeList = new List<EmployeeList>();
            try
            {
                var objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ProjectId", ProjectId);
                DataSet empList;

                empList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                    "GetActiveEmployeeForPerticularProject_SP", objParam);

                foreach (DataRow drw in empList.Tables[0].Rows)
                {
                    var eItemList = new EmployeeList();
                    eItemList.EmployeeId = Convert.ToInt32(drw["EmployeeId"]);
                    eItemList.EmployeeCode = (drw["EmployeeCode"]).ToString();
                    eItemList.EmployeeName = drw["FirstName"].ToString() + '.' + drw["LastName"].ToString();
                    employeeList.Add(eItemList);
                }

                return employeeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmployeeList GetLoggedInEmployeeDetails(string EmployeeCode)
        {
            try
            {
                EmployeeList employeeRecords = new EmployeeList();
                var employeeList = dbContext.GetEmployeeList_SP();
                employeeRecords = (from m in employeeList
                                   where m.EmployeeCode == EmployeeCode
                                   select new EmployeeList
                                   {
                                       EmployeeId = m.Employeeid,
                                       EmployeeCode = m.EmployeeCode,
                                       EmployeeName = m.EmployeeName
                                   }).FirstOrDefault();
                return employeeRecords;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EmployeeList GetLoggedInEmployeeDetailsByEmployeeId(int EmployeeId)
        {
            try
            {
                EmployeeList employeeRecords = new EmployeeList();
                var employeeList = dbContext.GetEmployeeList_SP();
                employeeRecords = (from m in employeeList
                                   where m.Employeeid == EmployeeId
                                   select new EmployeeList
                                   {
                                       EmployeeId = m.Employeeid,
                                       EmployeeCode = m.EmployeeCode,
                                       EmployeeName = m.EmployeeName
                                   }).FirstOrDefault();
                return employeeRecords;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProjectDetails GetProjectDetails(int ProjectId)
        {
            try
            {
                ProjectDetails projectRecord = new ProjectDetails();
                var ProjectDetails = dbContext.GetPMSProjectDetails_SP(ProjectId);
                projectRecord = (from p in ProjectDetails
                                 select new ProjectDetails
                                 {
                                     ProjectId = p.ProjectID,
                                     ProjectName = p.ProjectName
                                 }).FirstOrDefault();
                return projectRecord;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TagListClass> GetTagRecord(int ProjectID)
        {
            try
            {
                List<TagListClass> tagRecords = new List<TagListClass>();
                var tagList = dbContext.GetTimesheetTagDetails_sp(ProjectID);
                tagRecords = (from m in tagList
                              //where m.LevelName == TagLevelName.ProjectTag ? m.StartDate <= DateTime.Now && m.EndDate >= DateTime.Now : m.LevelName == TagLevelName.GlobalTag
                              where m.LevelName == TagLevelName.ProjectTag || m.LevelName == TagLevelName.GlobalTag
                              orderby m.TagName ascending
                              select new TagListClass
                              {
                                  TagId = m.TagId,
                                  TagName = m.TagName
                              }).ToList();
                return tagRecords.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProjectTaskRespose SaveProjectTaskRecord(TaskCreationModel model)
        {
            try
            {
                ProjectTaskRespose response = new ProjectTaskRespose();
                response.status = false;
                //response.isTaskNameExist = false;
                //var taskRecords = dbContext.GetProjectTaskDetails_SP(model.ProjectID);
                //var taskDetails = (from m in taskRecords
                //                   where m.TaskName.ToLower() == model.TaskName.ToLower()
                //                   select new AddProjectTask
                //                     {
                //                         TaskName = m.TaskName,
                //                         ProjectTaskTypeID = m.ProjectTaskTypeID
                //                     }).ToList();
                //if (taskDetails.Count == 0 || (model.TaskName == model.SelectedTaskName && model.ProjectTaskTypeID > 0))
                //{
                string[] stringEmployeeArray;
                string trimEmployeeList;
                string trimTagIdList = "";
                int assignedEmployeeCount = 0;
                trimEmployeeList = model.SelectedEmployeeList.TrimEnd(',');
                stringEmployeeArray = trimEmployeeList.Split(',');
                assignedEmployeeCount = stringEmployeeArray.Count();
                int[] AssignedEmployeeArray = Array.ConvertAll(stringEmployeeArray, s => int.Parse(s));
                //int totalMinutes = ConvertHoursToMinutes(model.PlannedHours);
                double? totalMinutes = model.PlannedHours * 60;
                double? PlannedHoursPerEmployee = totalMinutes / assignedEmployeeCount;
                if (!string.IsNullOrEmpty(model.SelectedTagList))
                    trimTagIdList = model.SelectedTagList.TrimEnd(',');

                int ProjectTaskTypeID = model.ProjectTaskTypeID;
                string TaskName = model.TaskName;
                int ProjectID = model.ProjectID;
                DateTime? StartDate = model.StartDate;
                DateTime? EndDate = model.EndDate;
                int? StatusID = model.StatusID;
                int? AvgUnitTime = model.AvgUnitTime;
                int? TaskTypeID = model.TaskTypeID;
                int CreatedBy = model.LoggedInEmployeeId;
                DateTime CreatedDate = DateTime.Now;
                string Description = model.Description;
                int? MileStoneId = model.MileStoneId;
                int? PlannedUnits = model.PlannedUnits;
                bool ProjectTaskType = model.ProjectTaskType;
                string Operation = "";
                if (model.ProjectTaskTypeID == 0)
                    Operation = "INSERT";
                else
                    Operation = "UPDATE";

                for (int i = 0; i < assignedEmployeeCount; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                    dbContext.AddUpdateProjectTaskDetails_sp(ProjectTaskTypeID, TaskName, ProjectID, StartDate, EndDate, Convert.ToDecimal(PlannedHoursPerEmployee), Convert.ToInt32(AssignedEmployeeArray[i]), trimTagIdList, StatusID, AvgUnitTime,
                        TaskTypeID, CreatedBy, CreatedDate, Description, MileStoneId, PlannedUnits, ProjectTaskType, Operation, Output);
                    response.status = Convert.ToBoolean(Output.Value);
                }
                //}
                //else
                //    response.isTaskNameExist = true;
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public double? ConvertMinutesToHours(double? savedMinutes)
        {
            double? ActualHours;
            double totalHours = (Math.Round(Convert.ToDouble(savedMinutes / 60), 2));
            double convertedHours = Math.Floor(totalHours);
            double Minutes = totalHours - convertedHours;
            double convertedMinutes = (Math.Round(Minutes * 60) / 100);
            return ActualHours = convertedHours + convertedMinutes;
        }

        public List<MileStoneListClass> GetMileStoneList(int? ProjectId)
        {
            List<MileStoneListClass> mileStoneList = new List<MileStoneListClass>();
            var mileStoneRecords = dbContext.GeMilestoneDetails_SP();
            mileStoneList = (from m in mileStoneRecords
                             where m.ProjectID == ProjectId
                             select new MileStoneListClass()
                             {
                                 MileStoneId = m.MileStoneID,
                                 MileStoneName = m.MileStone,
                                 ProjStartDate = m.ProjStartDate,
                                 ProjEndDate = m.ProjEndDate
                             }).ToList();
            return mileStoneList;
        }

        public bool DeleteTaskRecord(int? ProjectTaskTypeId)
        {
            ObjectParameter Output = new ObjectParameter("Result", typeof(int));
            dbContext.DeleteProjectTaskDetails_sp(ProjectTaskTypeId, Output);
            bool status = Convert.ToBoolean(Output.Value);
            return status;
        }

        #endregion TaskCreation

        public string Get_Employee_Emails(int EmployeeId)
        {
            ObjectParameter Output = new ObjectParameter("Status", typeof(string));
            dbContext.sp_SaveFromExcel_Validator("GetEmpEmail", EmployeeId.ToString(), "", "", null, null, "", "", Output);
            string DataFrmDb = Output.Value.ToString();
            return DataFrmDb;
        }

        public List<TimesheetModel.TaskList> getTaskName(int? ProjectID, int? EmployeeId)
        {
            List<TimesheetModel.TaskList> taskname = new List<TimesheetModel.TaskList>();
            var tasknames = dbContext.GetTaskName_SP(ProjectID, EmployeeId);
            taskname = (from r in tasknames
                        select new TimesheetModel.TaskList
                        {
                            ProjectTaskTypeId = r.ProjectTaskTypeId,
                            TaskName = r.TaskName,
                            ProjectID = r.ProjectId,
                            AssignedTo = r.AssignedTo,
                            AvgUnitTime = r.AvgUnitTime,
                            Description = r.Description
                        }).ToList();
            return taskname;
        }

        public List<TimesheetSettingList> getSettingName(int ProjectID)
        {
            List<TimesheetSettingList> name = new List<TimesheetSettingList>();
            var names = dbContext.GetPMSConfigurationSettingsName_SP(ProjectID);
            name = (from r in names
                    select new TimesheetSettingList
                    {
                        Settingid = r.Type,
                        SettingName = r.Type
                    }).ToList();
            return name;
        }

        public List<TimesheetModel.TaskList> getTagsName(int? ProjectID)
        {
            List<TimesheetModel.TaskList> tagname = new List<TimesheetModel.TaskList>();
            var tagnames = dbContext.GetTagName_SP(ProjectID);
            tagname = (from r in tagnames
                       where r.LevelName == TagLevelName.ProjectTag ? r.StartDate <= DateTime.Now && r.EndDate >= DateTime.Now : r.LevelName == TagLevelName.GlobalTag
                       orderby r.TagName ascending
                       select new TimesheetModel.TaskList
                       {
                           TagId = r.TagId,
                           TagName = r.TagName,
                           TagType = r.LevelName,
                           ProjectID = r.RefId
                       }).ToList();
            return tagname;
        }

        public bool SaveTimeSheetEntryRecord(TimesheetModel model)
        {
            try
            {
                int status = 0;
                string Mode = "";
                Mode = "INSERT";
                int convertedHours = Convert.ToInt32(model.Hours * 60);
                int totalhours = convertedHours + (model.Minutes.HasValue ? model.Minutes.Value : 0);

                ObjectParameter Output = new ObjectParameter("Result", typeof(int));

                if (model.NewTaskCheckbox != true)
                {
                    dbContext.AddUpdateTimesheetEntryDetails_sp(model.TimeSheetId, model.Units, model.ProjectTaskTypeId, model.EmployeeId, model.Date, totalhours, model.Comments, Mode, Output);

                    status = Convert.ToInt32(Output.Value);
                }
                else
                {
                    dbContext.AddNewTimesheetTaskDetails_sp(model.NewTask, model.ProjectID, model.TagID, model.EmployeeId, model.Date, model.Date, 1153, model.Description, Output);
                    status = Convert.ToInt32(Output.Value);
                    dbContext.AddUpdateTimesheetEntryDetails_sp(model.TimeSheetId, model.Units, status, model.EmployeeId, model.Date, totalhours, model.Comments, Mode, Output);
                }
                if (status != 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TimesheetModel> getTaskDescription(int? TaskId)
        {
            List<TimesheetModel> data = new List<TimesheetModel>();
            var details = dbContext.getTaskDescription_SP(TaskId);
            data = (from r in details
                    select new TimesheetModel
                    {
                        AvgUnitTime = r.AvgUnitTime,
                        Description = r.Description,
                        HrUnit = r.Value,
                        TaskStartDate = r.StartDate,
                        TaskEndDate = r.EndDate,
                        ActualHours = Convert.ToDouble(r.ActualHrs),
                        PlannedHours = Convert.ToDouble(r.PlannedHours)
                    }).ToList();
            return data;
        }

        public SEMResponse SaveTimeSheetEntryRecordGridData(TimesheetModel model, string LoggedUserName, int? SelectedEntryId)
        {
            try
            {
                SEMResponse response = new SEMResponse();
                response.status = false;
                int? TimeSheetId = SelectedEntryId;

                ObjectParameter Result = new ObjectParameter("Result", typeof(Int32));

                double ActualHours = Math.Floor(Convert.ToDouble(model.Hours));
                double Minutes = (Convert.ToDouble(model.Hours) % 1 * 100);
                double convertedHours = ActualHours * 60;
                int? hours = Convert.ToInt32(convertedHours + Minutes);

                dbContext.UpdateTimesheetDetails_sp(TimeSheetId, model.Units, hours, model.Comments, LoggedUserName, Result);
                response.status = Convert.ToBoolean(Result.Value);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteTimesheetEntryRecord(string[] SelectedEntryId)
        {
            try
            {
                bool status = false;
                for (int i = 0; i < SelectedEntryId.Length; i++)
                {
                    ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                    int TimeSheetId = Convert.ToInt32(SelectedEntryId[i]);
                    dbContext.DeleteTimesheetEntryDetails_sp(TimeSheetId, Result);
                    status = Convert.ToBoolean(Result.Value);
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProjectAppList> GetProjectListForTimesheet(string LoggedInUserRole, int employeeCode)
        {
            if (LoggedInUserRole == "HR Admin")
            {
                var dataList = dbContext.GetProjectName_SP();
                List<ProjectAppList> list = (from project in dataList
                                             select new ProjectAppList
                                             {
                                                 Projectids = project.ProjectId,
                                                 ProjectName = project.ProjectName
                                             }).ToList();

                ProjectAppList l1 = new ProjectAppList()
                {
                    Projectids = 0,
                    ProjectName = "Global"
                };
                list.Insert(0, l1);
                return list;
            }
            else
            {
                var datalist = dbContext.GetAllocatedProjectListOfUser_SP(employeeCode);
                List<ProjectAppList> loggedUserProjectList = (from project in datalist
                                                              orderby project.projectName ascending
                                                              select new ProjectAppList
                                                              {
                                                                  Projectids = project.projectId,
                                                                  ProjectName = project.projectName
                                                              }).ToList();

                return loggedUserProjectList;
            }
        }

        //public List<TimesheetSettingList> GetTimesheetSettingsList(string LoggedInUserRole)
        //{
        //    List<TimesheetSettingList> list = new List<TimesheetSettingList>();
        //    //if (LoggedInUserRole == "HR Admin")
        //    //{
        //    var dataList = dbContext.GetPMSConfigurationSettingsName_SP(LoggedInUserRole);
        //    list = (from project in dataList
        //            select new TimesheetSettingList
        //      {
        //          Settingid = project.Type,
        //          SettingName = project.Type
        //      }).ToList();
        //    //}
        //    return list;

        //}

        public List<PmsConfiguration> GetPMSSettingType(string SettingType)
        {
            List<PmsConfiguration> typeData = new List<PmsConfiguration>();
            var data = dbContext.GetPMSSettingTypeData_SP(SettingType);
            typeData = (from r in data
                        select new PmsConfiguration
                        {
                            LookUpTypeId = r.LookUpTypeId,
                            Type = r.Type,
                            Value = r.Value,
                            ProjectID = r.Ref_Id,
                            dataType = r.dataType,
                            dataTypeValue = r.dataTypeValue,
                            LevelName = r.LevelName
                        }).ToList();
            return typeData;
        }

        public PmsConfiguration GetSeperatePMSSettingType(string SettingType, int ProjectID)
        {
            PmsConfiguration typeData = new PmsConfiguration();
            var data = dbContext.GetSeperatePMSSettingTypeData_SP(SettingType, ProjectID);
            typeData = (from r in data
                        select new PmsConfiguration
                        {
                            LookUpTypeId = r.LookUpTypeId,
                            Type = r.Type,
                            Value = r.Value,
                            ProjectID = r.Ref_Id,
                            dataType = r.dataType,
                            dataTypeValue = r.dataTypeValue,
                            LevelName = r.LevelName
                        }).FirstOrDefault();
            return typeData;
        }

        public List<TimesheetApproverList> GetTimesheetApproverList(string settings, int employeeID)
        {
            List<TimesheetApproverList> list = new List<TimesheetApproverList>();
            //if (LoggedInUserRole == "HR Admin")
            //{
            var dataList = dbContext.GetPMSTimesheetApproversName_SP(settings, employeeID);
            list = (from project in dataList
                    select new TimesheetApproverList
                    {
                        TimesheetApproverID = project.EmployeeID,
                        TimesheetApproverName = project.employeename
                    }).ToList();
            //}
            return list;
        }

        public bool SavePMSConfigurationSettingRecord(PmsConfiguration model)
        {
            try
            {
                bool status = false;
                var Textvalue = "";
                ObjectParameter Output = new ObjectParameter("Result", typeof(int));

                if (model.dataType == "DDValue")
                {
                    Textvalue = model.DropDownValue;
                    dbContext.AddPMSTimesheetSettingDetails_sp(model.Type, Textvalue, model.LevelName, model.ProjectID, model.EmployeeId, model.dataType, Output);
                }
                else if (model.dataType == "Email")
                {
                    Textvalue = model.EmailIDValue;
                    dbContext.AddPMSTimesheetSettingDetails_sp(model.Type, Textvalue, model.LevelName, model.ProjectID, model.EmployeeId, model.dataType, Output);
                }
                else if (model.dataType == "User")
                {
                    Textvalue = model.UserValue;
                    dbContext.AddPMSTimesheetSettingDetails_sp(model.Type, Textvalue, model.LevelName, model.ProjectID, model.EmployeeId, model.dataType, Output);
                }
                else if (model.dataType == "SelectDD")
                {
                    Textvalue = model.SelectedDDValue;
                    dbContext.AddPMSTimesheetSettingDetails_sp(model.Type, Textvalue, model.LevelName, model.ProjectID, model.EmployeeId, model.dataType, Output);
                }
                else if (model.dataType == "CheckBox")
                {
                    if (model.CheckBoxValue == true)
                    {
                        Textvalue = "Yes";
                    }
                    else
                    {
                        Textvalue = "No";
                    }
                    dbContext.AddPMSTimesheetSettingDetails_sp(model.Type, Textvalue, model.LevelName, model.ProjectID, model.EmployeeId, model.dataType, Output);
                }
                else
                {
                    model.LevelName = "Global";
                    model.ProjectID = null;
                    dbContext.AddPMSTimesheetSettingDetails_sp(model.Type, Textvalue, model.LevelName, model.ProjectID, model.EmployeeId, model.dataType, Output);
                }

                status = Convert.ToBoolean(Output.Value);

                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetTimeSheetApproverDetails(int? projectId, int? EmployeeCode)
        {
            var dataValue = "";
            var data = dbContext.GetTimesheetApproverDetails_SP(projectId, EmployeeCode);
            foreach (var d in data)
            {
                dataValue = Convert.ToString(d.Value);
            }
            return dataValue;
        }

        public string GetDetailForCreatingTask(int? projectId)
        {
            var dataValue = "";
            var data = dbContext.GetDetailForCreatingTask_SP(projectId);
            foreach (var d in data)
            {
                dataValue = d.Value;
            }
            return dataValue;
        }

        public List<ActiveProjectList> GetActiveProjectList(int employeeCode)
        {
            List<ActiveProjectList> projectNames = new List<ActiveProjectList>();
            var projectList = dbContext.GetAllocatedProjectListOfUser_SP(employeeCode);
            projectNames = (from s in projectList
                            orderby s.projectName ascending
                            select new ActiveProjectList
                            {
                                ProjectID = s.projectId,
                                ProjectName = s.projectName
                            }).ToList();
            return projectNames.ToList();
        }
    }
}