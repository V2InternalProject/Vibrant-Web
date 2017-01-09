﻿using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Security;

namespace HRMS.DAL
{
    public class ConfirmationDAL
    {
        private HRMSDBEntities dbContext = new HRMSDBEntities();
        private V2toolsDBEntities dbContextV2tools = new V2toolsDBEntities();

        #region Confirmation Intiated Details

        public List<tbl_PA_Competency_Master> GetEmployeeSkillDetails(int employeeId, int page, int rows)
        {
            List<tbl_PA_Competency_Master> comepetencylist = new List<tbl_PA_Competency_Master>();
            try
            {
                dbContext = new HRMSDBEntities();
                comepetencylist = (from competency in dbContext.tbl_PA_Competency_Master
                                   orderby competency.Description
                                   select competency).Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception E)
            {
                throw E;
            }
            if (comepetencylist.Count == 0)
            {
                comepetencylist = null;
                return comepetencylist;
            }
            else
            {
                return comepetencylist;
            }
        }

        public List<InitiatConfirmationProcess> GetHRReviewerList_Emp()
        {
            List<InitiatConfirmationProcess> resourcepool = new List<InitiatConfirmationProcess>();
            try
            {
                resourcepool = (from rolename in dbContextV2tools.aspnet_Roles
                                join roleid in dbContextV2tools.aspnet_UsersInRoles on rolename.RoleId equals roleid.RoleId
                                join userid in dbContextV2tools.aspnet_Users on roleid.UserId equals userid.UserId
                                where rolename.RoleName == "HR Admin"
                                select new InitiatConfirmationProcess
                                {
                                    EmployeeId = Convert.ToInt32(userid.UserId),
                                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return resourcepool;
        }

        public List<ShowStatus> GetShowStatusResult(string EmployeeId, string ConfirmationId, int page, int rows)
        {
            List<ShowStatus> Result = new List<ShowStatus>();
            List<ShowStatus> NextStageProcess = new List<ShowStatus>();
            List<ShowStatus> ConfirmationResult = new List<ShowStatus>();
            List<ShowStatus> InitiateProcess = new List<ShowStatus>();
            int empID = Convert.ToInt32(EmployeeId);
            int selectedConfirmationId = Convert.ToInt32(ConfirmationId);
            EmployeeDAL employeeDAL = new EmployeeDAL();
            try
            {
                tbl_CF_Confirmation empinfo = dbContext.tbl_CF_Confirmation.Where(ed => ed.ConfirmationID == selectedConfirmationId && ed.EmployeeID == empID).OrderByDescending(ed => ed.CreatedDate).FirstOrDefault();

                var confirmationId = empinfo.ConfirmationID;
                InitiateProcess = (from emp in dbContext.Tbl_HR_ConfirmationStageEvent
                                   join actor in dbContext.HRMS_tbl_PM_Employee on emp.UserId equals actor.EmployeeID into actors
                                   from actorlist in actors.DefaultIfEmpty()
                                   join confirmation in dbContext.tbl_CF_Confirmation on emp.ConfirmationID equals confirmation.ConfirmationID into confirmationlist
                                   from confirm in confirmationlist.DefaultIfEmpty()
                                   join employee in dbContext.HRMS_tbl_PM_Employee on confirm.EmployeeID equals employee.EmployeeID into employees
                                   from emplist in employees.DefaultIfEmpty()
                                   join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.ToStageId equals stagename.ConfirmationStageID into stages
                                   from stagedesc in stages.DefaultIfEmpty()
                                   where emp.ConfirmationID == confirmationId && emp.FromStageId == 0
                                   select new ShowStatus
                                   {
                                       ShowstatusEmployeeCode = emplist.EmployeeCode,
                                       ShowstatusEmployeeId = emplist.EmployeeID,
                                       ShowstatusEmployeeName = emplist.EmployeeName,
                                       ShowstatusCurrentStage = stagedesc.ConfirmationStage,
                                       ShowstatusStageID = emp.ToStageId,
                                       ShowstatusTime = emp.EventDatatime,
                                       ShowstatusActor = actorlist.EmployeeName,
                                       ShowstatusAction = emp.Action
                                   }).ToList();

                ConfirmationResult = (from emp in dbContext.Tbl_HR_ConfirmationStageEvent
                                      join actor in dbContext.HRMS_tbl_PM_Employee on emp.UserId equals actor.EmployeeID into actors
                                      from actorlist in actors.DefaultIfEmpty()
                                      join confirmation in dbContext.tbl_CF_Confirmation on emp.ConfirmationID equals confirmation.ConfirmationID into confirmationlist
                                      from confirm in confirmationlist.DefaultIfEmpty()
                                      join employee in dbContext.HRMS_tbl_PM_Employee on confirm.EmployeeID equals employee.EmployeeID into employees
                                      from emplist in employees.DefaultIfEmpty()
                                      join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.ToStageId equals stagename.ConfirmationStageID into stages
                                      from stagedesc in stages.DefaultIfEmpty()
                                      where emp.ConfirmationID == confirmationId && emp.ToStageId != 1
                                      select new ShowStatus
                                      {
                                          ShowstatusEmployeeCode = emplist.EmployeeCode,
                                          ShowstatusEmployeeId = emplist.EmployeeID,
                                          ShowstatusEmployeeName = emplist.EmployeeName,
                                          ShowstatusCurrentStage = stagedesc.ConfirmationStage,
                                          ShowstatusStageID = emp.ToStageId,
                                          ShowstatusTime = emp.EventDatatime,
                                          ShowstatusActor = actorlist.EmployeeName,
                                          ShowstatusAction = emp.Action
                                      }).ToList();
                Tbl_HR_ConfirmationStageEvent LatestEntry = new Tbl_HR_ConfirmationStageEvent();
                if (InitiateProcess.Count != 0)
                {
                    if (InitiateProcess[0].ShowstatusStageID != 4)
                    {
                        LatestEntry = (from emp in dbContext.Tbl_HR_ConfirmationStageEvent
                                       join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.FromStageId + 1 equals stagename.ConfirmationStageID into stages
                                       from stagedesc in stages.DefaultIfEmpty()
                                       where emp.ConfirmationID == confirmationId && emp.ToStageId != 1 && emp.FromStageId == empinfo.stageID - 1
                                       orderby emp.EventDatatime descending
                                       select emp).FirstOrDefault();
                    }
                }

                //for selecting total no. of entries of From stage id 4
                if (LatestEntry != null)
                {
                    NextStageProcess = (from emp in dbContext.Tbl_HR_ConfirmationStageEvent
                                        join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.ToStageId + 1 equals stagename.ConfirmationStageID into stages
                                        from stagedesc in stages.DefaultIfEmpty()
                                        where emp.ConfirmationID == confirmationId && emp.FromStageId == LatestEntry.FromStageId && emp.EventDatatime == LatestEntry.EventDatatime
                                        select new ShowStatus
                                        {
                                            ShowstatusCurrentStage = stagedesc.ConfirmationStage,
                                        }).ToList();
                }
                else
                {
                    Tbl_HR_ConfirmationStageEvent LatestEntryNextStageProcess = (from emp in dbContext.Tbl_HR_ConfirmationStageEvent
                                                                                 join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.ToStageId equals stagename.ConfirmationStageID into stages
                                                                                 from stagedesc in stages.DefaultIfEmpty()
                                                                                 where emp.ConfirmationID == confirmationId
                                                                                 orderby emp.EventDatatime descending
                                                                                 select emp).FirstOrDefault();
                    if (LatestEntryNextStageProcess != null && LatestEntryNextStageProcess.ToStageId != 4)
                    {
                        NextStageProcess = (from emp in dbContext.Tbl_HR_ConfirmationStageEvent
                                            join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.ToStageId + 1 equals stagename.ConfirmationStageID into stages
                                            from stagedesc in stages.DefaultIfEmpty()
                                            where emp.ConfirmationID == confirmationId && emp.FromStageId == LatestEntryNextStageProcess.FromStageId && emp.EventDatatime == LatestEntryNextStageProcess.EventDatatime
                                            select new ShowStatus
                                            {
                                                ShowstatusCurrentStage = stagedesc.ConfirmationStage,
                                            }).ToList();
                    }
                }
                if (ConfirmationResult.Count() >= 0)
                {
                    HRMS_tbl_PM_Employee hrDetails = new HRMS_tbl_PM_Employee();
                    HRMS_tbl_PM_Employee employeeDetails = new HRMS_tbl_PM_Employee();
                    HRMS_tbl_PM_Employee managerDetails = new HRMS_tbl_PM_Employee();
                    HRMS_tbl_PM_Employee manager2Details = new HRMS_tbl_PM_Employee();
                    HRMS_tbl_PM_Employee reviewerDetails = new HRMS_tbl_PM_Employee();
                    HRMS_tbl_PM_Employee HRreviewerDetails = new HRMS_tbl_PM_Employee();
                    for (int i = 0; i < NextStageProcess.Count(); i++)
                    {
                        if (empinfo.stageID != 7)
                        {
                            if (empinfo.stageID == 1 && empinfo.ConfirmationStatus == 3)
                            {
                                NextStageProcess[i].showStatus = "Probation Extended";
                            }
                            else if (empinfo.stageID == 1 && empinfo.ConfirmationStatus == 2)
                            {
                                NextStageProcess[i].showStatus = "Send for PIP";
                            }
                            else if (empinfo.stageID == 1 && empinfo.ConfirmationStatus == 4)
                            {
                                employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(empinfo.HRReviewer));
                                NextStageProcess[i].showStatus = "Pending for " + employeeDetails.EmployeeName + " to take action";
                            }
                            else if (empinfo.stageID == 2 && empinfo.ConfirmationStatus == 4)
                            {
                                employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(empinfo.FurtherApproverId));
                                NextStageProcess[i].showStatus = "Pending for " + employeeDetails.EmployeeName + " to take action";
                            }
                            else if (NextStageProcess[i].ShowstatusCurrentStage == "Confirmed")
                            {
                            }
                        }
                    }
                }
                Result = InitiateProcess.Union(ConfirmationResult).Union(NextStageProcess).ToList();
                return Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<InitiatConfirmationProcess> SearchEmployee(string searchText, int pageNo = 1, int pageSize = 10)
        {
            List<InitiatConfirmationProcess> employeeDetails = new List<InitiatConfirmationProcess>();
            try
            {
                employeeDetails = (from employee in dbContext.HRMS_tbl_PM_Employee
                                   where (employee.EmployeeName.Contains(searchText) || employee.EmployeeCode.Contains(searchText)) && employee.Status == false
                                   orderby employee.EmployeeName
                                   select new InitiatConfirmationProcess
                                   {
                                       EmployeeCode = employee.EmployeeCode,
                                       EmployeeId = employee.EmployeeID,
                                       UserId = employee.UserID,
                                       EmployeeName = employee.EmployeeName,
                                   }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employeeDetails;
        }

        public List<InitiatConfirmationProcess> SearchEmployeeForLoadGrid(string searchText, int page, int rows, out int totalCount)
        {
            List<InitiatConfirmationProcess> employeeDetails = new List<InitiatConfirmationProcess>();
            try
            {
                DateTime date = DateTime.Now.Date;
                int monthno = date.Month;
                int year = date.Year;
                DateTime fromdate, todate;
                double result = Convert.ToDouble((date.Day).ToString()) / 15;
                if (result > 1)
                {
                    if (monthno == 12)
                    {
                        fromdate = Convert.ToDateTime(monthno + "/16/" + year);
                        todate = Convert.ToDateTime("1/15/" + (year + 1));
                    }
                    else
                    {
                        fromdate = Convert.ToDateTime(monthno + "/16/" + year);
                        todate = Convert.ToDateTime((monthno + 1) + "/15/" + year);
                    }
                }
                else
                {
                    if (monthno == 1)
                    {
                        fromdate = Convert.ToDateTime("12/16/" + (year - 1));
                        todate = Convert.ToDateTime(monthno + "/15/" + year);
                    }
                    else
                    {
                        fromdate = Convert.ToDateTime((monthno - 1) + "/16/" + year);
                        todate = Convert.ToDateTime(monthno + "/15/" + year);
                    }
                }
                employeeDetails = (from employee in dbContext.HRMS_tbl_PM_Employee
                                   join gradelist in dbContext.tbl_PM_GradeMaster on employee.GradeID equals gradelist.GradeID into grade
                                   from grades in grade.DefaultIfEmpty()
                                   join buisnessgrp in dbContext.tbl_CNF_BusinessGroups on employee.BusinessGroupID equals buisnessgrp.BusinessGroupID into bu
                                   from bugrp in bu.DefaultIfEmpty()
                                   join stageid in dbContext.tbl_CF_Confirmation on employee.EmployeeID equals stageid.EmployeeID into stageids
                                   from stagesid in stageids.DefaultIfEmpty()
                                   join stagename in dbContext.tbl_CF_ConfirmationStages on stagesid.stageID equals stagename.ConfirmationStageID into stages
                                   from stagedesc in stages.DefaultIfEmpty()
                                   join reportingMgr in dbContext.HRMS_tbl_PM_Employee on stagesid.ReportingManager equals reportingMgr.EmployeeID into Rm
                                   from reportingManager in Rm.DefaultIfEmpty()
                                   join reportingMgr2 in dbContext.HRMS_tbl_PM_Employee on stagesid.ReportingManager2 equals reportingMgr2.EmployeeID into Rm2
                                   from reportingManager2 in Rm2.DefaultIfEmpty()
                                   join reviewer in dbContext.HRMS_tbl_PM_Employee on stagesid.Reviewer equals reviewer.EmployeeID into rW
                                   from Reviewer in rW.DefaultIfEmpty()
                                   join HrReviewer in dbContext.HRMS_tbl_PM_Employee on stagesid.HRReviewer equals HrReviewer.EmployeeID into HrrW
                                   from HRReviewr in HrrW.DefaultIfEmpty()
                                   join organizationUnit in dbContext.tbl_PM_Location on employee.LocationID equals organizationUnit.LocationID into ou
                                   from OUs in ou.DefaultIfEmpty()
                                   join role in dbContext.HRMS_tbl_PM_Role on employee.PostID equals role.RoleID into roles
                                   from empRole in roles.DefaultIfEmpty()
                                   where (employee.EmployeeName.Contains(searchText) || employee.EmployeeCode.Contains(searchText))
                                   && ((employee.Probation_Review_Date >= fromdate && employee.Probation_Review_Date <= todate) || (employee.Probation_Review_Date < fromdate && employee.EmployeeStatusID == 5 && employee.Status == false && employee.EmployeeStatusMasterID == 1))
                                   // || (stagesid.ConfirmationStatus==2 || stagesid.ConfirmationStatus==3)
                                   orderby employee.Probation_Review_Date
                                   select new InitiatConfirmationProcess
                                   {
                                       EmployeeCode = employee.EmployeeCode,
                                       EmployeeId = employee.EmployeeID,
                                       UserId = employee.UserID,
                                       EmployeeName = employee.EmployeeName,
                                       ProbationReviewDate = employee.Probation_Review_Date,
                                       JoiningDate = employee.JoiningDate,
                                       Grade = grades.Grade,
                                       ConfirmationStatus = employee.ConfirmationStatus,
                                       BusinessGroup = bugrp.BusinessGroup,
                                       OrganizationUnit = OUs.Location,
                                       RoleInitiate = empRole.RoleDescription,
                                       CurrentStage = stagedesc.ConfirmationStage,
                                       ReportingManager = reportingManager.EmployeeName,
                                       ReportingManager2 = reportingManager2.EmployeeName,
                                       Reviewer = Reviewer.EmployeeName,
                                       HRReviewer = HRReviewr.EmployeeName,
                                   }).ToList();

                totalCount = employeeDetails.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employeeDetails.Skip((page - 1) * rows).Take(rows).ToList();
        }

        public List<ConfirmationDetailsViewModel> InboxSearchEmployeeForConfirmationLoadGrid(string searchText, string field, string fieldchild, int page, int rows, int loginUserId, out int totalCount)
        {
            List<ConfirmationDetailsViewModel> mainResult = new List<ConfirmationDetailsViewModel>();
            List<ConfirmationDetailsViewModel> DetailsForAdmin = new List<ConfirmationDetailsViewModel>();
            List<ConfirmationDetailsViewModel> DetailsForEmployee = new List<ConfirmationDetailsViewModel>();
            List<ConfirmationDetailsViewModel> detailsForManager = new List<ConfirmationDetailsViewModel>();
            List<ConfirmationDetailsViewModel> detailsReviewer = new List<ConfirmationDetailsViewModel>();
            try
            {
                int FieldChild = 0;
                if (fieldchild != "")
                {
                    FieldChild = Convert.ToInt32(fieldchild);
                }
                string Loginemployeecode = string.Empty;
                string[] loginemployeerole = { };
                EmployeeDAL empdal = new EmployeeDAL();
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);

                HRMS_tbl_PM_Employee loginrolescheck = empdal.GetEmployeeDetails(employeeID);

                if (loginrolescheck != null || loginrolescheck.EmployeeID > 0)
                {
                    Loginemployeecode = loginrolescheck.EmployeeCode;
                    loginemployeerole = Roles.GetRolesForUser(Loginemployeecode);
                }

                //HR Admin record
                if (loginemployeerole.Contains("HR Admin"))
                {
                    DetailsForAdmin = (from emp in dbContext.tbl_CF_Confirmation
                                       join employee in dbContext.HRMS_tbl_PM_Employee on emp.EmployeeID equals employee.EmployeeID into emplst
                                       from emplist in emplst.DefaultIfEmpty()
                                       join stagename in dbContext.tbl_CF_ConfirmationStages on emp.stageID equals stagename.ConfirmationStageID into stages
                                       from stagedesc in stages.DefaultIfEmpty()
                                       where (emplist.EmployeeName.Contains(searchText) || emplist.EmployeeCode.Contains(searchText))
                                             && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emplist.BusinessGroupID == FieldChild : field == "Organization Unit" ? emplist.LocationID == FieldChild : field == "Stage Name" ? emp.stageID == FieldChild : FieldChild == 0))) //field search
                                             && ((emp.ConfirmationCoordinator == employeeID && (emp.stageID == 2 || emp.stageID == 6)) || (emp.HRReviewer == employeeID && (emp.stageID == 6 || emp.stageID == 5)))
                                       join ese in dbContext.Tbl_HR_ConfirmationStageEvent on emp.ConfirmationID equals ese.ConfirmationID into eventStageRecord  // Fix to add red Image support
                                       select new ConfirmationDetailsViewModel
                                       {
                                           Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                           ConfirmationID = emp.ConfirmationID,
                                           EmployeeId = emp.EmployeeID,
                                           EmployeeCode = emplist.EmployeeCode,
                                           EmployeeName = emplist.EmployeeName,
                                           JoiningDate = emplist.JoiningDate,
                                           ProbationReviewDate = emplist.Probation_Review_Date,
                                           InitiatedDate = emp.ConfirmationInitiationDate,
                                           StageID = emp.stageID,
                                           Stage = stagedesc.ConfirmationStage,
                                       }).ToList();
                }

                //Self record
                DetailsForEmployee = (from employee in dbContext.tbl_CF_Confirmation
                                      join emp in dbContext.HRMS_tbl_PM_Employee on employee.EmployeeID equals emp.EmployeeID into emplst
                                      from emplist in emplst.DefaultIfEmpty()
                                      join Stagename in dbContext.tbl_CF_ConfirmationStages on employee.stageID equals Stagename.ConfirmationStageID
                                      where (emplist.EmployeeName.Contains(searchText) || emplist.EmployeeCode.Contains(searchText))
                                            && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emplist.BusinessGroupID == FieldChild : field == "Organization Unit" ? emplist.LocationID == FieldChild : field == "Stage Name" ? employee.stageID == FieldChild : FieldChild == 0))) //field search
                                            && (employee.EmployeeID == employeeID && employee.stageID == 3)
                                      join ese in dbContext.Tbl_HR_ConfirmationStageEvent on employee.ConfirmationID equals ese.ConfirmationID into eventStageRecord  // Fix to add red Image support
                                      select new ConfirmationDetailsViewModel
                                      {
                                          Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                          ConfirmationID = employee.ConfirmationID,
                                          EmployeeId = employee.EmployeeID,
                                          EmployeeCode = emplist.EmployeeCode,
                                          EmployeeName = emplist.EmployeeName,
                                          JoiningDate = emplist.JoiningDate,
                                          ProbationReviewDate = emplist.Probation_Review_Date,
                                          InitiatedDate = employee.ConfirmationInitiationDate,
                                          StageID = employee.stageID,
                                          Stage = Stagename.ConfirmationStage
                                      }).ToList();

                //manager record
                detailsForManager = (from emp in dbContext.HRMS_tbl_PM_Employee
                                     join employee in dbContext.tbl_CF_Confirmation on emp.EmployeeID equals employee.EmployeeID into emplst
                                     from emplist in emplst.DefaultIfEmpty()
                                     join stagename in dbContext.tbl_CF_ConfirmationStages on emplist.stageID equals stagename.ConfirmationStageID into stages
                                     from stagedesc in stages.DefaultIfEmpty()
                                     where (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                            && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? emplist.stageID == FieldChild : FieldChild == 0))) //field search
                                            && ((emplist.ReportingManager == employeeID || emplist.ReportingManager2 == employeeID) && emplist.stageID == 4)
                                     join ese in dbContext.Tbl_HR_ConfirmationStageEvent on emplist.ConfirmationID equals ese.ConfirmationID into eventStageRecord  // Fix to add red Image support
                                     select new ConfirmationDetailsViewModel
                                     {
                                         Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                         ConfirmationID = emplist.ConfirmationID,
                                         EmployeeId = emplist.EmployeeID,
                                         EmployeeCode = emp.EmployeeCode,
                                         EmployeeName = emp.EmployeeName,
                                         JoiningDate = emp.JoiningDate,
                                         ProbationReviewDate = emp.Probation_Review_Date,
                                         InitiatedDate = emplist.ConfirmationInitiationDate,
                                         StageID = emplist.stageID,
                                         Stage = stagedesc.ConfirmationStage
                                     }).ToList();

                List<ConfirmationDetailsViewModel> ManagerList = new List<ConfirmationDetailsViewModel>();
                foreach (var item in detailsForManager)
                {
                    Tbl_HR_ConfirmationStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ConfirmationStageEvent
                                                                 where empInfo.ConfirmationID == item.ConfirmationID
                                                                 orderby empInfo.EventDatatime descending
                                                                 select empInfo).FirstOrDefault();
                    Tbl_HR_ConfirmationStageEvent LatestEntryManager = new Tbl_HR_ConfirmationStageEvent();
                    if (LatestEntry != null)
                    {
                        LatestEntryManager = (from empInfo in dbContext.Tbl_HR_ConfirmationStageEvent
                                              where empInfo.ConfirmationID == item.ConfirmationID && (empInfo.FromStageId == 4 && empInfo.ToStageId == 4) && empInfo.EventDatatime >= LatestEntry.EventDatatime
                                              orderby empInfo.EventDatatime descending
                                              select empInfo).FirstOrDefault();
                    }
                    if (LatestEntryManager != null)
                    {
                        if (LatestEntryManager.UserId != employeeID)
                        {
                            ManagerList.Add(item);
                        }
                        else
                            continue;
                    }
                    else
                    {
                        ManagerList.Add(item);
                    }
                }

                //reviewer record
                detailsReviewer = (from emp in dbContext.tbl_CF_Confirmation
                                   join employee in dbContext.HRMS_tbl_PM_Employee on emp.EmployeeID equals employee.EmployeeID into emplst
                                   from emplist in emplst.DefaultIfEmpty()
                                   join stagename in dbContext.tbl_CF_ConfirmationStages on emp.stageID equals stagename.ConfirmationStageID into stages
                                   from stagedesc in stages.DefaultIfEmpty()
                                   where (emplist.EmployeeName.Contains(searchText) || emplist.EmployeeCode.Contains(searchText))
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emplist.BusinessGroupID == FieldChild : field == "Organization Unit" ? emplist.LocationID == FieldChild : field == "Stage Name" ? emp.stageID == FieldChild : FieldChild == 0))) //field search
                                         && (emp.Reviewer == employeeID && emp.stageID == 5)
                                   orderby emplist.EmployeeName
                                   select new ConfirmationDetailsViewModel
                                   {
                                       ConfirmationID = emp.ConfirmationID,
                                       EmployeeId = emp.EmployeeID,
                                       EmployeeCode = emplist.EmployeeCode,
                                       EmployeeName = emplist.EmployeeName,
                                       JoiningDate = emplist.JoiningDate,
                                       ProbationReviewDate = emplist.Probation_Review_Date,
                                       InitiatedDate = emp.ConfirmationInitiationDate,
                                       StageID = emp.stageID,
                                       Stage = stagedesc.ConfirmationStage
                                   }).ToList();

                List<ConfirmationDetailsViewModel> ReviewerList = new List<ConfirmationDetailsViewModel>();
                foreach (var item in detailsReviewer)
                {
                    Tbl_HR_ConfirmationStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ConfirmationStageEvent
                                                                 where empInfo.ConfirmationID == item.ConfirmationID
                                                                 orderby empInfo.EventDatatime descending
                                                                 select empInfo).FirstOrDefault();

                    Tbl_HR_ConfirmationStageEvent LatestEntryReviewer = new Tbl_HR_ConfirmationStageEvent();

                    if (LatestEntry != null)
                    {
                        LatestEntryReviewer = (from empInfo in dbContext.Tbl_HR_ConfirmationStageEvent
                                               where empInfo.ConfirmationID == item.ConfirmationID && (empInfo.FromStageId == 5 && empInfo.ToStageId == 5) && empInfo.EventDatatime >= LatestEntry.EventDatatime
                                               orderby empInfo.EventDatatime descending
                                               select empInfo).FirstOrDefault();
                    }
                    if (LatestEntryReviewer != null)
                    {
                        if (LatestEntryReviewer.UserId != employeeID)
                        {
                            ReviewerList.Add(item);
                        }
                        else
                            continue;
                    }
                    else
                    {
                        ReviewerList.Add(item);
                    }
                }
                mainResult = DetailsForAdmin.Union(DetailsForEmployee).Union(ManagerList).Union(ReviewerList).ToList();
                totalCount = mainResult.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mainResult.Skip((page - 1) * rows).Take(rows).ToList();
        }

        public List<ConfirmationDetailsViewModel> WatchListSearchEmployeeForConfirmationLoadGrid(string searchText, string field, string fieldchild, int page, int rows, int loginUserId, out int totalCount)
        {
            List<ConfirmationDetailsViewModel> mainResult = new List<ConfirmationDetailsViewModel>();
            List<ConfirmationDetailsViewModel> DetailsForAdmin = new List<ConfirmationDetailsViewModel>();
            List<ConfirmationDetailsViewModel> DetailsForEmployee = new List<ConfirmationDetailsViewModel>();
            List<ConfirmationDetailsViewModel> detailsForManager = new List<ConfirmationDetailsViewModel>();
            List<ConfirmationDetailsViewModel> detailsReviewer = new List<ConfirmationDetailsViewModel>();
            try
            {
                int FieldChild = 0;
                if (fieldchild != "")
                {
                    FieldChild = Convert.ToInt32(fieldchild);
                }
                string Loginemployeecode = string.Empty;
                String[] loginemployeerole = { };
                EmployeeDAL empdal = new EmployeeDAL();
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);
                HRMS_tbl_PM_Employee loginrolescheck = empdal.GetEmployeeDetails(employeeID);
                tbl_CF_Confirmation empinfo = new tbl_CF_Confirmation();
                if (loginrolescheck != null || loginrolescheck.EmployeeID > 0)
                {
                    Loginemployeecode = loginrolescheck.EmployeeCode;
                    loginemployeerole = Roles.GetRolesForUser(Loginemployeecode);
                }
                if (loginemployeerole.Contains("HR Admin"))
                {
                    DetailsForAdmin = (from emp in dbContext.tbl_CF_Confirmation
                                       join employee in dbContext.HRMS_tbl_PM_Employee on emp.EmployeeID equals employee.EmployeeID into emplst
                                       from emplist in emplst.DefaultIfEmpty()
                                       join stagename in dbContext.tbl_CF_ConfirmationStages on emp.stageID equals stagename.ConfirmationStageID into stages
                                       from stagedesc in stages.DefaultIfEmpty()
                                       where (emplist.EmployeeName.Contains(searchText) || emplist.EmployeeCode.Contains(searchText))
                                             && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emplist.BusinessGroupID == FieldChild : field == "Organization Unit" ? emplist.LocationID == FieldChild : field == "Stage Name" ? emp.stageID == FieldChild : FieldChild == 0))) //field search
                                             && ((emp.stageID == 3 || emp.stageID == 4 || emp.stageID == 7 || (emp.stageID == 1 && (emp.ConfirmationStatus == 3 || emp.ConfirmationStatus == 2))) || (emp.stageID == 5 && (emp.HRReviewer != employeeID)) || (emp.stageID == 6 && ((emp.HRReviewer != employeeID) && (emp.ConfirmationCoordinator != employeeID))))
                                       join ese in dbContext.Tbl_HR_ConfirmationStageEvent on emp.ConfirmationID equals ese.ConfirmationID into eventStageRecord  // Fix to add red Image support
                                       select new ConfirmationDetailsViewModel
                                       {
                                           Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                           ConfirmationID = emp.ConfirmationID,
                                           EmployeeId = emp.EmployeeID,
                                           EmployeeCode = emplist.EmployeeCode,
                                           EmployeeName = emplist.EmployeeName,
                                           JoiningDate = emplist.JoiningDate,
                                           ProbationReviewDate = emplist.Probation_Review_Date,
                                           InitiatedDate = emp.ConfirmationInitiationDate,
                                           StageID = emp.stageID,
                                           Stage = stagedesc.ConfirmationStage
                                       }).Distinct().ToList();
                }

                detailsReviewer = (from emp in dbContext.tbl_CF_Confirmation
                                   join employee in dbContext.HRMS_tbl_PM_Employee on emp.EmployeeID equals employee.EmployeeID into emplst
                                   from emplist in emplst.DefaultIfEmpty()
                                   join stagename in dbContext.tbl_CF_ConfirmationStages on emp.stageID equals stagename.ConfirmationStageID into stages
                                   from stagedesc in stages.DefaultIfEmpty()
                                   where (emplist.EmployeeName.Contains(searchText) || emplist.EmployeeCode.Contains(searchText))
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emplist.BusinessGroupID == FieldChild : field == "Organization Unit" ? emplist.LocationID == FieldChild : field == "Stage Name" ? emp.stageID == FieldChild : FieldChild == 0))) //field search
                                         && (emp.Reviewer == employeeID && (emp.stageID != 5 || emp.stageID == 5))
                                   orderby emplist.EmployeeName
                                   join ese in dbContext.Tbl_HR_ConfirmationStageEvent on emp.ConfirmationID equals ese.ConfirmationID into eventStageRecord  // Fix to add red Image support
                                   select new ConfirmationDetailsViewModel
                                   {
                                       Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                       ConfirmationID = emp.ConfirmationID,
                                       EmployeeId = emp.EmployeeID,
                                       EmployeeCode = emplist.EmployeeCode,
                                       EmployeeName = emplist.EmployeeName,
                                       JoiningDate = emplist.JoiningDate,
                                       ProbationReviewDate = emplist.Probation_Review_Date,
                                       InitiatedDate = emp.ConfirmationInitiationDate,
                                       StageID = emp.stageID,
                                       Stage = stagedesc.ConfirmationStage
                                   }).ToList();

                List<ConfirmationDetailsViewModel> ReviewerList = new List<ConfirmationDetailsViewModel>();
                foreach (var item in detailsReviewer)
                {
                    Tbl_HR_ConfirmationStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ConfirmationStageEvent
                                                                 where empInfo.ConfirmationID == item.ConfirmationID
                                                                 orderby empInfo.EventDatatime descending
                                                                 select empInfo).FirstOrDefault();
                    Tbl_HR_ConfirmationStageEvent LatestEntryReviewer = new Tbl_HR_ConfirmationStageEvent();
                    if (LatestEntry != null)
                    {
                        LatestEntryReviewer = (from empInfo in dbContext.Tbl_HR_ConfirmationStageEvent
                                               where empInfo.ConfirmationID == item.ConfirmationID && (empInfo.FromStageId == 5 && (empInfo.ToStageId == 5 || empInfo.ToStageId == 6)) && empInfo.EventDatatime <= LatestEntry.EventDatatime
                                               orderby empInfo.EventDatatime descending
                                               select empInfo).FirstOrDefault();
                    }
                    if (LatestEntryReviewer != null)
                    {
                        if (LatestEntryReviewer.UserId == employeeID)
                        {
                            ReviewerList.Add(item);
                        }
                        else
                            continue;
                    }
                    else
                    {
                        ReviewerList.Add(item);
                    }
                }
                DetailsForEmployee = (from employee in dbContext.tbl_CF_Confirmation
                                      join emp in dbContext.HRMS_tbl_PM_Employee on employee.EmployeeID equals emp.EmployeeID into emplst
                                      from emplist in emplst.DefaultIfEmpty()
                                      join Stagename in dbContext.tbl_CF_ConfirmationStages on employee.stageID equals Stagename.ConfirmationStageID
                                      where (emplist.EmployeeName.Contains(searchText) || emplist.EmployeeCode.Contains(searchText)) &&
                                            (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emplist.BusinessGroupID == FieldChild : field == "Organization Unit" ? emplist.LocationID == FieldChild : field == "Stage Name" ? employee.stageID == FieldChild : FieldChild == 0))) &&
                                            (employee.EmployeeID == employeeID && (employee.stageID != 3))
                                      select new ConfirmationDetailsViewModel
                                      {
                                          ConfirmationID = employee.ConfirmationID,
                                          EmployeeId = employee.EmployeeID,
                                          EmployeeCode = emplist.EmployeeCode,
                                          EmployeeName = emplist.EmployeeName,
                                          JoiningDate = emplist.JoiningDate,
                                          ProbationReviewDate = emplist.Probation_Review_Date,
                                          InitiatedDate = employee.ConfirmationInitiationDate,
                                          StageID = employee.stageID,
                                          Stage = Stagename.ConfirmationStage
                                      }).ToList();

                detailsForManager = (from emp in dbContext.HRMS_tbl_PM_Employee
                                     join employee in dbContext.tbl_CF_Confirmation on emp.EmployeeID equals employee.EmployeeID into emplst
                                     from emplist in emplst.DefaultIfEmpty()
                                     join stagename in dbContext.tbl_CF_ConfirmationStages on emplist.stageID equals stagename.ConfirmationStageID into stages
                                     from stagedesc in stages.DefaultIfEmpty()
                                     where (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                      && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? emplist.stageID == FieldChild : FieldChild == 0))) //field search
                                      && ((emplist.ReportingManager == employeeID || emplist.ReportingManager2 == employeeID)
                                      && (emplist.stageID != 4 || emplist.stageID == 4))
                                     join ese in dbContext.Tbl_HR_ConfirmationStageEvent on emplist.ConfirmationID equals ese.ConfirmationID into eventStageRecord
                                     select new ConfirmationDetailsViewModel
                                     {
                                         Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty,
                                         ConfirmationID = emplist.ConfirmationID,
                                         EmployeeId = emplist.EmployeeID,
                                         EmployeeCode = emp.EmployeeCode,
                                         EmployeeName = emp.EmployeeName,
                                         JoiningDate = emp.JoiningDate,
                                         ProbationReviewDate = emp.Probation_Review_Date,
                                         InitiatedDate = emplist.ConfirmationInitiationDate,
                                         StageID = emplist.stageID,
                                         Stage = stagedesc.ConfirmationStage,
                                     }).ToList();
                List<ConfirmationDetailsViewModel> ManagerList = new List<ConfirmationDetailsViewModel>();
                foreach (var item in detailsForManager)
                {
                    Tbl_HR_ConfirmationStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ConfirmationStageEvent
                                                                 where empInfo.ConfirmationID == item.ConfirmationID
                                                                 orderby empInfo.EventDatatime descending
                                                                 select empInfo).FirstOrDefault();
                    Tbl_HR_ConfirmationStageEvent LatestEntryManager = new Tbl_HR_ConfirmationStageEvent();
                    if (LatestEntry != null)
                    {
                        LatestEntryManager = (from empInfo in dbContext.Tbl_HR_ConfirmationStageEvent
                                              where empInfo.ConfirmationID == item.ConfirmationID && (empInfo.FromStageId == 4 && (empInfo.ToStageId == 4 || empInfo.ToStageId == 5))
                                              && empInfo.EventDatatime <= LatestEntry.EventDatatime && empInfo.UserId == employeeID
                                              orderby empInfo.EventDatatime descending
                                              select empInfo).FirstOrDefault();
                    }
                    if (LatestEntryManager != null)
                    {
                        if (LatestEntry.FromStageId == 5 && LatestEntry.ToStageId == 4)
                        {
                            continue;
                        }
                        else
                        {
                            if (LatestEntryManager.UserId == employeeID)
                            {
                                ManagerList.Add(item);
                            }
                            else
                                continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                mainResult = DetailsForAdmin.Union(DetailsForEmployee).Union(ManagerList).Union(ReviewerList).ToList();
                totalCount = mainResult.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mainResult.Skip((page - 1) * rows).Take(rows).ToList();
        }

        private string GetStageMessage(int? stageId, string employeeName, string manager1Name, string reviewer, string cordinator)
        {
            string strRV = string.Empty;
            switch (stageId)
            {
                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    strRV = "Waiting for " + employeeName + " to take action";
                    break;

                case 4:
                    strRV = "Waiting for " + manager1Name + " to take action";
                    break;

                case 5:
                    strRV = "Waiting for " + reviewer + " to take action";
                    break;

                case 6:
                    strRV = "Waiting for " + cordinator + " to take action";
                    break;

                case 7:
                    break;
            }
            return strRV;
        }

        public List<FieldChildList> GetFieldChildDetails(string FieldName)
        {
            List<FieldChildList> childDetails = new List<FieldChildList>();
            try
            {
                if (FieldName == "Buisness Group")
                {
                    childDetails = (from status in dbContext.tbl_CNF_BusinessGroups
                                    select new FieldChildList
                                    {
                                        ID = status.BusinessGroupID,
                                        Discription = status.BusinessGroup
                                    }).ToList();

                    return childDetails;
                }
                else
                {
                    if (FieldName == "Organization Unit")
                    {
                        childDetails = (from status in dbContext.tbl_PM_Location
                                        select new FieldChildList
                                        {
                                            ID = status.LocationID,
                                            Discription = status.Location
                                        }).ToList();
                        return childDetails;
                    }
                    else
                    {
                        childDetails = (from status in dbContext.tbl_CF_ConfirmationStages
                                        where status.ConfirmationStageID != 2
                                        select new FieldChildList
                                        {
                                            ID = status.ConfirmationStageID,
                                            Discription = status.ConfirmationStage
                                        }).ToList();
                        return childDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InitiatConfirmationProcess> GetManagersList()
        {
            List<InitiatConfirmationProcess> managers = new List<InitiatConfirmationProcess>();
            try
            {
                managers = (from employee in dbContext.HRMS_tbl_PM_Employee
                            select new InitiatConfirmationProcess
                            {
                                ReportingManager = employee.EmployeeName
                            }).ToList();
                return managers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public tbl_CF_Confirmation GetSeparationDetails(int employeeid)
        {
            tbl_CF_Confirmation employeeDetails = new tbl_CF_Confirmation();
            try
            {
                employeeDetails = dbContext.tbl_CF_Confirmation.Where(ed => ed.EmployeeID == employeeid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employeeDetails;
        }

        public bool InitiateConfirmationDetail(tbl_CF_Confirmation initiateProcess, int employeeid)
        {
            bool isAdded = false;
            try
            {
                tbl_CF_Confirmation emp = dbContext.tbl_CF_Confirmation.Where(ed => ed.ConfirmationID == initiateProcess.ConfirmationID).FirstOrDefault();
                if (emp == null || emp.ConfirmationID <= 0)
                {
                    dbContext.tbl_CF_Confirmation.AddObject(initiateProcess);
                    dbContext.SaveChanges();
                    tbl_CF_Confirmation confirmationid = (from cnf in dbContext.tbl_CF_Confirmation
                                                          where cnf.EmployeeID == initiateProcess.EmployeeID
                                                          orderby cnf.ConfirmationInitiationDate descending
                                                          select cnf).FirstOrDefault();
                    Tbl_HR_ConfirmationStageEvent initiateProcessStages = new Tbl_HR_ConfirmationStageEvent();
                    {
                        initiateProcessStages.ConfirmationID = confirmationid.ConfirmationID;
                        initiateProcessStages.EventDatatime = confirmationid.ConfirmationInitiationDate;
                        initiateProcessStages.Action = "Approved";
                        initiateProcessStages.FromStageId = 1;
                        initiateProcessStages.ToStageId = 3;
                        initiateProcessStages.UserId = employeeid;
                        dbContext.Tbl_HR_ConfirmationStageEvent.AddObject(initiateProcessStages);
                        dbContext.SaveChanges();
                    };
                }
                else
                {
                    emp.EmployeeID = initiateProcess.EmployeeID;
                    emp.ConfirmationCoordinator = initiateProcess.ConfirmationCoordinator;
                    emp.ReportingManager = initiateProcess.ReportingManager;
                    emp.ReportingManager2 = initiateProcess.ReportingManager2;
                    emp.Reviewer = initiateProcess.Reviewer;
                    emp.HRReviewer = initiateProcess.HRReviewer;
                    emp.Comments = initiateProcess.Comments;
                    emp.ConfirmationInitiationDate = initiateProcess.ConfirmationInitiationDate;
                    dbContext.SaveChanges();
                }
                HRMS_tbl_PM_Employee empInfo = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == initiateProcess.EmployeeID).FirstOrDefault();
                if (empInfo != null)
                {
                    empInfo.ConfirmationStatus = 1;
                    dbContext.SaveChanges();
                }

                isAdded = true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return isAdded;
        }

        #endregion Confirmation Intiated Details

        #region Confirmation Form Save Records.

        public bool SaveCorporateDetails(ConfirmationFormViewModel empCorporate)
        {
            bool isAdded = false;
            int? ConfirmID = (int?)empCorporate.confirmationID;
            tbl_CF_CorporateContribution emp = dbContext.tbl_CF_CorporateContribution.Where(ed => ed.CorporateID == empCorporate.CorporateId && ed.ConfirmationID == ConfirmID).FirstOrDefault();
            if (emp == null)
            {
                tbl_CF_CorporateContribution corporate = new tbl_CF_CorporateContribution();
                corporate.EmployeeID = empCorporate.CorporateEmployeeID.Value;
                corporate.ConfirmationID = empCorporate.confirmationID;
                corporate.AreaOfContribution = empCorporate.AreaOfContribution;
                corporate.ContributionDesc = empCorporate.ContributionDesc;
                if (empCorporate.IsManagerOrEmployee == "Manager")
                    corporate.ManagerComments = empCorporate.ManagerComments.Trim();
                if (empCorporate.IsManagerOrEmployee == "Manager2")
                    corporate.ManagerCommentSecond = empCorporate.ManagerCommentsSecond.Trim();
                if (empCorporate.IsManagerOrEmployee == "Reviewer")
                    corporate.ReviewerComments = empCorporate.ReviewerComments.Trim();
                if (empCorporate.IsManagerOrEmployee == "HR")
                    corporate.SecondReviewerComments = empCorporate.HRReviewerComments.Trim();
                dbContext.tbl_CF_CorporateContribution.AddObject(corporate);
            }
            else
            {
                if (empCorporate.IsManagerOrEmployee == "Employee")
                {
                    emp.AreaOfContribution = empCorporate.AreaOfContribution.Trim();
                    emp.ContributionDesc = empCorporate.ContributionDesc.Trim();
                }
                if (empCorporate.IsManagerOrEmployee == "Manager")
                    emp.ManagerComments = empCorporate.ManagerComments.Trim();
                if (empCorporate.IsManagerOrEmployee == "Manager2")
                    emp.ManagerCommentSecond = empCorporate.ManagerCommentsSecond.Trim();
                if (empCorporate.IsManagerOrEmployee == "Reviewer")
                    emp.ReviewerComments = empCorporate.ReviewerComments.Trim();
                if (empCorporate.IsManagerOrEmployee == "HR")
                    emp.SecondReviewerComments = empCorporate.HRReviewerComments.Trim();
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool SaveProjectAchievementDetails(ProjectAchievement empCorporate)
        {
            bool isAdded = false;

            tbl_CF_ProjectAchievement emp = dbContext.tbl_CF_ProjectAchievement.Where(ed => ed.ProjectID == empCorporate.ProjAchieveID && ed.ConfirmationID == empCorporate.ConfirmationID).FirstOrDefault();
            if (emp == null)
            {
                tbl_CF_ProjectAchievement corporate = new tbl_CF_ProjectAchievement();
                corporate.EmployeeID = empCorporate.EmpID.Value;
                corporate.ConfirmationID = Convert.ToInt32(empCorporate.ConfirmationID);
                corporate.ProjectDescription = empCorporate.ProjectDesc.Trim();
                corporate.ProjectAchievement = empCorporate.ProjectAchievements.Trim();
                corporate.StartDate = empCorporate.StartDate;
                corporate.EndDate = empCorporate.EndDate;
                corporate.NameOfProjManager = empCorporate.NameOfManager.Trim();
                dbContext.tbl_CF_ProjectAchievement.AddObject(corporate);
            }
            else
            {
                emp.ProjectDescription = empCorporate.ProjectDesc.Trim();
                emp.ProjectAchievement = empCorporate.ProjectAchievements.Trim();
                emp.StartDate = empCorporate.StartDate;
                emp.EndDate = empCorporate.EndDate;
                emp.NameOfProjManager = empCorporate.NameOfManager.Trim();
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool SaveAddQualificationDetails(AdditionalQualification empCorporate)
        {
            bool isAdded = false;
            tbl_CF_AdditionalQualification emp = dbContext.tbl_CF_AdditionalQualification.Where(ed => ed.AddQualificationID == empCorporate.AddQualificationID && ed.ConfirmationID == empCorporate.ConfirmationID).FirstOrDefault();
            if (emp == null)
            {
                //if (empCorporate.AddQualificationID == 0 || empCorporate.AddQualificationID == null)
                //    empCorporate.AddQualificationID = 0;
                //else
                //{
                //    int projID = (from c in dbContext.tbl_CF_AdditionalQualification select c.AddQualificationID).Max();
                //    empCorporate.AddQualificationID = projID;
                //}
                tbl_CF_AdditionalQualification corporate = new tbl_CF_AdditionalQualification();
                corporate.EmployeeID = empCorporate.QualifEmployeeID.Value;
                corporate.ConfirmationID = Convert.ToInt32(empCorporate.ConfirmationID);
                //  corporate.AddQualificationID = Convert.ToInt32(empCorporate.AddQualificationID) + 1;
                corporate.Title = empCorporate.Title.Trim();
                corporate.Type = GetSkillDescription(Convert.ToInt32(empCorporate.skill));
                corporate.FromDuration = empCorporate.FromDuration;
                corporate.ToDuration = empCorporate.ToDuration;
                corporate.SkillAquired = empCorporate.AddSkillAquired.Trim();
                corporate.SkillUsefulness = empCorporate.AddSkillUsed.Trim();

                dbContext.tbl_CF_AdditionalQualification.AddObject(corporate);
                dbContext.SaveChanges();
            }
            else
            {
                emp.Title = empCorporate.Title.Trim();
                emp.FromDuration = empCorporate.FromDuration;
                emp.ToDuration = empCorporate.ToDuration;

                string qualifDescription;
                int val = Convert.ToInt32(empCorporate.skill);
                HRMS_tbl_PM_Tools pm = dbContext.HRMS_tbl_PM_Tools.Where(ed => ed.ToolID == val).FirstOrDefault();
                tbl_PM_QualificationType qualification = dbContext.tbl_PM_QualificationType.Where(qualificationType => qualificationType.QualificationTypeID == val).FirstOrDefault();
                qualifDescription = qualification.QualificationTypeName.Trim();
                emp.Type = qualifDescription;
                emp.SkillAquired = empCorporate.AddSkillAquired.Trim();
                emp.SkillUsefulness = empCorporate.AddSkillUsed.Trim();
                dbContext.SaveChanges();
            }

            isAdded = true;
            return isAdded;
        }

        public bool SaveGoalAspireDetails(GoalAquire empCorporate)
        {
            bool isAdded = false;
            tbl_CF_GoalAspire emp = dbContext.tbl_CF_GoalAspire.Where(ed => ed.EmployeeID == empCorporate.EmployeID && ed.ConfirmationID == empCorporate.ConfirmID).FirstOrDefault();
            if (emp == null)
            {
                tbl_CF_GoalAspire corporate = new tbl_CF_GoalAspire();
                corporate.EmployeeID = Convert.ToInt32(empCorporate.EmployeID);
                corporate.ConfirmationID = Convert.ToInt32(empCorporate.ConfirmID);
                corporate.ShortTermGoal = empCorporate.ShortTerm.Trim();
                corporate.LongTermGoal = empCorporate.LongTerm.Trim();
                corporate.SkillDevPrgm = empCorporate.SkillDevPrgm.Trim();

                dbContext.tbl_CF_GoalAspire.AddObject(corporate);
            }
            else
            {
                emp.ShortTermGoal = empCorporate.ShortTerm.Trim();
                emp.LongTermGoal = empCorporate.LongTerm.Trim();
                emp.SkillDevPrgm = empCorporate.SkillDevPrgm.Trim();
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool SaveSkillAquiredDetails(SkillsAquired empCorporate)
        {
            bool isAdded = false;
            tbl_CF_SkillAquired emp = dbContext.tbl_CF_SkillAquired.Where(ed => ed.SkillAquiredID == empCorporate.SkillsAquiredID && ed.ConfirmationID == empCorporate.ConfirmationID).FirstOrDefault();
            if (emp == null)
            {
                tbl_CF_SkillAquired corporate = new tbl_CF_SkillAquired();
                corporate.EmployeeID = empCorporate.SkillEmployeeID.Value;
                corporate.ConfirmationID = Convert.ToInt32(empCorporate.ConfirmationID);
                corporate.SkillName = empCorporate.SkillName.Trim();
                corporate.SkillAquiredThrough = empCorporate.AquiredThrough.Trim();
                corporate.SkillUsefulness = empCorporate.ProjectUsefulness.Trim();
                dbContext.tbl_CF_SkillAquired.AddObject(corporate);
            }
            else
            {
                emp.SkillName = empCorporate.SkillName.Trim();
                emp.SkillAquiredThrough = empCorporate.AquiredThrough.Trim();
                emp.SkillUsefulness = empCorporate.ProjectUsefulness.Trim();
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool SaveParameterDetails(List<ConfirmationParameter> empCorporate)
        {
            bool isAdded = false;
            dbContext = new HRMSDBEntities();
            if (empCorporate != null)
            {
                int count = empCorporate.Count();
                int comptncyID = 0;
                int confID = 0;
                string str;
                int emp = 0;
                for (int i = 0; i < count; i++)
                {
                    emp = empCorporate[0].employeeID;
                    comptncyID = empCorporate[i].competencyID;
                    confID = empCorporate[i].confirmationID;
                    tbl_CF_ValueDrivers objValueDrivers = dbContext.tbl_CF_ValueDrivers.Where(ed => ed.CompetencyID == comptncyID && ed.EmployeeID == emp && ed.ConfirmationID == confID).FirstOrDefault();
                    if (objValueDrivers != null)
                    {
                        if (empCorporate[0].IsManagerOrEmployee == "Employee")
                        {
                            objValueDrivers.SelfRating = empCorporate[i].SelfRating == null ? 0 : empCorporate[i].SelfRating;
                            objValueDrivers.EmployeeComments = empCorporate[i].EmpComments == null ? "" : empCorporate[i].EmpComments.Trim();
                        }
                        if (empCorporate[0].IsManagerOrEmployee == "Manager")
                        {
                            objValueDrivers.ManagerRating1 = empCorporate[i].ManagerRating1 == null ? 0 : empCorporate[i].ManagerRating1;
                            objValueDrivers.ManagerComments1 = empCorporate[i].MngrComments1 == null ? "" : empCorporate[i].MngrComments1.Trim();
                        }
                        if (empCorporate[0].IsManagerOrEmployee == "Manager2")
                        {
                            objValueDrivers.ManagerRating2 = empCorporate[i].ManagerRating2 == null ? 0 : empCorporate[i].ManagerRating2;
                            objValueDrivers.ManagerComments2 = empCorporate[i].MngrComments2 == null ? "" : empCorporate[i].MngrComments2.Trim();
                        }
                        if (empCorporate[0].IsManagerOrEmployee == "Reviewer")
                        {
                            objValueDrivers.ReviewerRating = empCorporate[i].ReviewerRating == null ? 0 : empCorporate[i].ReviewerRating;
                            objValueDrivers.ReviewerComments = empCorporate[i].ReviewerComments == null ? "" : empCorporate[i].ReviewerComments.Trim();
                        }
                        if (empCorporate[0].IsManagerOrEmployee == "HR")
                        {
                            objValueDrivers.HRrRating = empCorporate[i].HRrRating == null ? 0 : empCorporate[i].HRrRating;
                            objValueDrivers.HRComments = empCorporate[i].HrComments == null ? "" : empCorporate[i].HrComments.Trim();
                        }
                        dbContext.SaveChanges();
                    }
                }
                tbl_CF_ValueDrivers objValueDriver = dbContext.tbl_CF_ValueDrivers.Where(ed => ed.EmployeeID == emp).FirstOrDefault();
                if (objValueDriver != null)
                {
                    if (empCorporate[0].IsManagerOrEmployee == "HR")
                    {
                        objValueDriver.OverallHRReview = empCorporate[count - 1].OverallReviewHRRating == null ? 0 : empCorporate[count - 1].OverallReviewHRRating;
                        objValueDriver.OverallHRComments = empCorporate[count - 1].OverallReviewHRComments == null ? "" : empCorporate[count - 1].OverallReviewHRComments.Trim();
                        dbContext.SaveChanges();
                    }
                    if (empCorporate[0].IsManagerOrEmployee == "Reviewer")
                    {
                        objValueDriver.OverallReviewRating = empCorporate[count - 1].OverallReviewRating == null ? 0 : empCorporate[count - 1].OverallReviewRating;
                        objValueDriver.OverallReviewComments = empCorporate[count - 1].OverallReviewRatingComments == null ? "" : empCorporate[count - 1].OverallReviewRatingComments.Trim();
                        dbContext.SaveChanges();
                    }
                }
            }
            isAdded = true;
            return isAdded;
        }

        public bool SavePerformanceHinderDetails(PerformanceHinderTable empCorporate)
        {
            bool isAdded = false;
            tbl_CF_PerformanceHinders emp = dbContext.tbl_CF_PerformanceHinders.Where(ed => ed.PerformanceHinderID == empCorporate.perfHinderID).FirstOrDefault();
            tbl_CF_Confirmation conf = dbContext.tbl_CF_Confirmation.Where(ed => ed.EmployeeID == empCorporate.empID && ed.ConfirmationID == empCorporate.confID).FirstOrDefault();
            if (emp == null || emp.EmployeeID <= 0 || emp.PerformanceHinderID < 0)
            {
                tbl_CF_PerformanceHinders corporate = new tbl_CF_PerformanceHinders();
                corporate.EmployeeID = Convert.ToInt32(empCorporate.empID);
                corporate.ConfirmationID = Convert.ToInt32(empCorporate.confID);

                corporate.EmployeeCommentsFFEnvi = empCorporate.EmpCommentsFFEnvi;
                corporate.EmployeeCommentsFFSelf = empCorporate.EmpCommentsFFSelf;
                corporate.EmployeeCommentsIFEnvi = empCorporate.EmpCommentsIFEnvi;
                corporate.EmployeeCommentsIFSelf = empCorporate.EmpCommentsIFSelf;
                corporate.EmployeeCommentsSupport = empCorporate.EmpCommentsSupport;

                dbContext.tbl_CF_PerformanceHinders.AddObject(corporate);
            }
            else
            {
                if (empCorporate.IsManagerOrEmployee == "Employee")
                {
                    emp.EmployeeCommentsFFEnvi = empCorporate.EmpCommentsFFEnvi;
                    emp.EmployeeCommentsFFSelf = empCorporate.EmpCommentsFFSelf;
                    emp.EmployeeCommentsIFEnvi = empCorporate.EmpCommentsIFEnvi;
                    emp.EmployeeCommentsIFSelf = empCorporate.EmpCommentsIFSelf;
                    emp.EmployeeCommentsSupport = empCorporate.EmpCommentsSupport;
                }
                if (empCorporate.IsManagerOrEmployee == "Manager")
                {
                    emp.ManagerCommentsFFEnvi = empCorporate.MngrCommentsFFEnvi;
                    emp.ManagerCommentsFFSelf = empCorporate.MngrCommentsFFSelf;
                    emp.ManagerCommentsIFEnvi = empCorporate.MngrCommentsIFEnvi;
                    emp.ManagerCommentsIFSelf = empCorporate.MngrCommentsIFSelf;
                    emp.ManagerCommentsSupport = empCorporate.MngrCommentsSupport;
                }
                if (empCorporate.IsManagerOrEmployee == "Manager2")
                {
                    emp.ManagerCommentsFFEnviSecond = empCorporate.MngrCommentsFFEnviSecond;
                    emp.ManagerCommentsFFSelfSecond = empCorporate.MngrCommentsFFSelfSecond;
                    emp.ManagerCommentsIFEnviSecond = empCorporate.MngrCommentsIFEnviSecond;
                    emp.ManagerCommentsIFSelfSecond = empCorporate.MngrCommentsIFSelfSecond;
                    emp.ManagerCommentsSupportSecond = empCorporate.MngrCommentsSupportSecond;
                }
                if (empCorporate.IsManagerOrEmployee == "Reviewer")
                {
                    emp.ReviewerCommentsFFEnvi = empCorporate.ReviewerCommentsFFEnvi;
                    emp.ReviewerCommentsFFSelf = empCorporate.ReviewerCommentsFFSelf;
                    emp.ReviewerCommentsIFEnvi = empCorporate.ReviewerCommentsIFEnvi;
                    emp.ReviewerCommentsIFSelf = empCorporate.ReviewerCommentsIFSelf;
                    emp.ReviewerCommentsSupport = empCorporate.ReviewerCommentsSupport;
                }
                if (empCorporate.IsManagerOrEmployee == "HR" && conf.stageID != 6)
                {
                    emp.HrCommentsFFEnvi = empCorporate.HrCommentsFFEnvi;
                    emp.HrCommentsFFSelf = empCorporate.HrCommentsFFSelf;
                    emp.HrCommentsIFEnvi = empCorporate.HrCommentsIFEnvi;
                    emp.HrCommentsIFSelf = empCorporate.HrCommentsIFSelf;
                    emp.HrCommentsSupport = empCorporate.HrCommentsSupport;
                }
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool saveParametersDescInValueDriver(List<int> ParameterList, List<string> paramdesc, int? empID, int? confID)
        {
            bool isAdded = false;
            int EmployeeID = Convert.ToInt32(empID);
            int ConfirmationID = Convert.ToInt32(confID);
            int CompetencyID = 0;
            tbl_CF_ValueDrivers addParamDriver;
            for (int i = 0; i < ParameterList.Count; i++)
            {
                CompetencyID = ParameterList[i];
                tbl_CF_ValueDrivers paramDriver = dbContext.tbl_CF_ValueDrivers.Where(ed => ed.CompetencyID == CompetencyID && ed.EmployeeID == EmployeeID && ed.ConfirmationID == confID).FirstOrDefault();
                if (paramDriver == null)
                {
                    addParamDriver = new tbl_CF_ValueDrivers();
                    addParamDriver.CompetencyID = CompetencyID;
                    addParamDriver.ParameterDescription = paramdesc[i].Trim();
                    addParamDriver.EmployeeID = EmployeeID;
                    addParamDriver.ConfirmationID = ConfirmationID;
                    dbContext.tbl_CF_ValueDrivers.AddObject(addParamDriver);
                }
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool saveHrConfiramtionDetails(ConfirmationFormViewModel empCorporate)
        {
            bool isAdded = false;
            tbl_CF_TempConfirmation tempConf = dbContext.tbl_CF_TempConfirmation.Where(ed => ed.ConfirmationID == empCorporate.confirmationID).FirstOrDefault();
            tbl_CF_Confirmation ConfDetails = dbContext.tbl_CF_Confirmation.Where(ed => ed.ConfirmationID == empCorporate.confirmationID).FirstOrDefault();
            HRMS_tbl_PM_Employee EmpDetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == empCorporate.EmployeeID).FirstOrDefault();

            if (tempConf == null)
            {
                tbl_CF_TempConfirmation corporate = new tbl_CF_TempConfirmation();

                if (empCorporate.IsAcceptedOrExtended == "accept")
                {
                    corporate.EmployeeStatusID = Convert.ToInt32(empCorporate.empStatus);
                    //corporate.EmployeeType = empCorporate.empType;
                    corporate.ConfirmationID = Convert.ToInt32(empCorporate.confirmationID);
                    corporate.RoleID = Convert.ToInt32(empCorporate.roleName);
                    corporate.GradeID = Convert.ToInt32(empCorporate.gradeName);
                    corporate.ConfirmationDate = empCorporate.ConfirmationDate;
                    corporate.ConfirmationComments = empCorporate.ConfirmationComments == null ? " " : empCorporate.ConfirmationComments.Trim();
                    corporate.ConfirmationStatus = 4;
                    ConfDetails.ConfirmationStatus = 4;     // status =4 (it means employee is confirmed)
                }
                else if (empCorporate.IsAcceptedOrExtended == "extend")
                {
                    corporate.ExtendedProbationDate = empCorporate.ExtendProbationDate;
                    corporate.ExtensionComments = empCorporate.ProbationComments;
                    corporate.ConfirmationID = Convert.ToInt32(empCorporate.confirmationID);
                    corporate.ConfirmationStatus = 3;
                    ConfDetails.ConfirmationStatus = 3; //status = 3 (it means employee is probation is extended)
                }
                else if (empCorporate.IsAcceptedOrExtended == "sendPIP")
                {
                    corporate.PIPDate = empCorporate.PIPDate;
                    corporate.PIPComments = empCorporate.PIPComments == null ? " " : empCorporate.PIPComments.Trim();
                    corporate.ConfirmationID = Convert.ToInt32(empCorporate.confirmationID);
                    corporate.ConfirmationStatus = 2;
                    ConfDetails.ConfirmationStatus = 2;//status =2 (it means employee is send for PIP)
                }
                dbContext.tbl_CF_TempConfirmation.AddObject(corporate);
            }
            else
            {
                if (empCorporate.IsAcceptedOrExtended == "accept")
                {
                    tempConf.EmployeeStatusID = Convert.ToInt32(empCorporate.empStatus);
                    //tempConf.EmployeeType = empCorporate.empType;
                    tempConf.RoleID = Convert.ToInt32(empCorporate.roleName);
                    tempConf.GradeID = Convert.ToInt32(empCorporate.gradeName);
                    tempConf.ConfirmationDate = empCorporate.ConfirmationDate;
                    tempConf.ConfirmationComments = empCorporate.ConfirmationComments == null ? " " : empCorporate.ConfirmationComments.Trim();
                    tempConf.ConfirmationStatus = 4;
                }
                else if (empCorporate.IsAcceptedOrExtended == "extend")
                {
                    tempConf.ExtendedProbationDate = empCorporate.ExtendProbationDate;
                    tempConf.ExtensionComments = empCorporate.ProbationComments == null ? " " : empCorporate.ProbationComments.Trim();
                    tempConf.ConfirmationStatus = 3;
                }
                else if (empCorporate.IsAcceptedOrExtended == "sendPIP")
                {
                    tempConf.PIPComments = empCorporate.PIPComments == null ? " " : empCorporate.PIPComments.Trim();
                    tempConf.PIPDate = empCorporate.PIPDate;
                    tempConf.ConfirmationStatus = 2;
                }
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool ApproveConfirmation(int? empID, string approveReject, int confirmationId)
        {
            bool isAdded = false;
            dbContext = new HRMSDBEntities();
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);

                int UserID = 0;
                // update tbl_CF_Confirmation table
                tbl_CF_Confirmation confTable = (from data in dbContext.tbl_CF_Confirmation
                                                 where data.EmployeeID == empID && data.ConfirmationID == confirmationId
                                                 select data).FirstOrDefault();

                // get ToStageId
                int ToStage = (from id in dbContext.Tbl_HR_ConfirmationStageEvent
                               where id.ConfirmationID == confTable.ConfirmationID
                               orderby id.EventDatatime descending
                               select id.ToStageId.HasValue ? id.ToStageId.Value : 0).FirstOrDefault();

                Tbl_HR_ConfirmationStageEvent confEvent = (from data in dbContext.Tbl_HR_ConfirmationStageEvent
                                                           where data.ConfirmationID == confTable.ConfirmationID && data.ToStageId == ToStage
                                                           select data).FirstOrDefault();
                UserID = loginuser.EmployeeID;

                if (confTable == null)
                {
                    isAdded = false;
                    return isAdded;
                }
                else
                {
                    if (approveReject == "Reject")
                    {
                        Tbl_HR_ConfirmationStageEvent corporate = new Tbl_HR_ConfirmationStageEvent();
                        corporate.ConfirmationID = confTable.ConfirmationID;
                        //need to change as the 1 who approve that person ID is store
                        corporate.UserId = UserID;
                        corporate.FromStageId = ToStage;
                        corporate.ToStageId = ToStage - 1;
                        corporate.Action = approveReject.Trim();
                        corporate.EventDatatime = DateTime.Now;
                        dbContext.Tbl_HR_ConfirmationStageEvent.AddObject(corporate);
                        confTable.stageID = ToStage - 1;
                        confTable.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        if ((confTable.ReportingManager2 != null && confTable.ReportingManager2 != 0) && confTable.stageID == 4)
                        {
                            //for selecting latest entry with From stage id is 3
                            Tbl_HR_ConfirmationStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ConfirmationStageEvent
                                                                         where ((empInfo.FromStageId == 3 && empInfo.ToStageId == 4) || (empInfo.FromStageId == 5 && empInfo.ToStageId == 4)) && empInfo.ConfirmationID == confTable.ConfirmationID
                                                                         orderby empInfo.EventDatatime descending
                                                                         select empInfo).FirstOrDefault();
                            //for selecting total no. of entries of From stage id 4
                            List<Tbl_HR_ConfirmationStageEvent> TotalRecords = new List<Tbl_HR_ConfirmationStageEvent>();
                            if (LatestEntry != null)
                            {
                                TotalRecords = (from total in dbContext.Tbl_HR_ConfirmationStageEvent
                                                where total.ConfirmationID == confTable.ConfirmationID && total.Action == "Move Ahead" && total.EventDatatime > LatestEntry.EventDatatime
                                                select total).ToList();
                            }
                            Tbl_HR_ConfirmationStageEvent corporate = new Tbl_HR_ConfirmationStageEvent();
                            corporate.ConfirmationID = confTable.ConfirmationID;
                            corporate.UserId = UserID;
                            corporate.FromStageId = ToStage;
                            corporate.Action = "Move Ahead";
                            corporate.EventDatatime = DateTime.Now;
                            confTable.CreatedDate = DateTime.Now;
                            if (TotalRecords.Count == 1)
                            {
                                confTable.stageID = ToStage + 1;
                                corporate.ToStageId = ToStage + 1;
                            }
                            else
                                corporate.ToStageId = ToStage;

                            dbContext.Tbl_HR_ConfirmationStageEvent.AddObject(corporate);
                        }
                        else if (confTable.stageID == 5)
                        {
                            //for selecting latest entry with From stage id is 3
                            Tbl_HR_ConfirmationStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ConfirmationStageEvent
                                                                         where ((empInfo.FromStageId == 4 && empInfo.ToStageId == 5) || (empInfo.FromStageId == 6 && empInfo.ToStageId == 5)) && empInfo.ConfirmationID == confTable.ConfirmationID
                                                                         orderby empInfo.EventDatatime descending
                                                                         select empInfo).FirstOrDefault();
                            //for selecting total no. of entries of From stage id 4
                            List<Tbl_HR_ConfirmationStageEvent> TotalRecords = new List<Tbl_HR_ConfirmationStageEvent>();
                            if (LatestEntry != null)
                            {
                                TotalRecords = (from total in dbContext.Tbl_HR_ConfirmationStageEvent
                                                where total.ConfirmationID == confTable.ConfirmationID && total.Action == "Move Ahead" && total.EventDatatime > LatestEntry.EventDatatime
                                                select total).ToList();
                            }
                            Tbl_HR_ConfirmationStageEvent corporate = new Tbl_HR_ConfirmationStageEvent();
                            corporate.ConfirmationID = confTable.ConfirmationID;
                            corporate.UserId = UserID;
                            corporate.FromStageId = ToStage;
                            corporate.Action = "Move Ahead";
                            corporate.EventDatatime = DateTime.Now;
                            confTable.CreatedDate = DateTime.Now;
                            if (TotalRecords.Count == 1)
                            {
                                confTable.stageID = ToStage + 1;
                                corporate.ToStageId = ToStage + 1;
                            }
                            else
                                corporate.ToStageId = ToStage;
                            dbContext.Tbl_HR_ConfirmationStageEvent.AddObject(corporate);
                        }
                        else
                        {
                            Tbl_HR_ConfirmationStageEvent corporate = new Tbl_HR_ConfirmationStageEvent();
                            corporate.ConfirmationID = confTable.ConfirmationID;
                            corporate.UserId = UserID;
                            corporate.FromStageId = ToStage;
                            corporate.ToStageId = ToStage + 1;
                            corporate.Action = approveReject.Trim();
                            corporate.EventDatatime = DateTime.Now;

                            if (confTable.stageID == 6)
                            {
                                tbl_CF_TempConfirmation tempObj = (from data in dbContext.tbl_CF_TempConfirmation
                                                                   where (data.ConfirmationID == confTable.ConfirmationID)
                                                                   select data).FirstOrDefault();
                                if (tempObj != null)
                                {
                                    //int empType = Convert.ToInt32(tempObj.EmployeeType);
                                    HRMS_tbl_PM_Employee EmpDetails = (from data in dbContext.HRMS_tbl_PM_Employee
                                                                       where (data.EmployeeID == confTable.EmployeeID)
                                                                       select data).FirstOrDefault();
                                    if (tempObj.ConfirmationStatus == 4)  // confirmed
                                    {
                                        EmpDetails.EmployeeStatusID = tempObj.EmployeeStatusID;

                                        EmpDetails.PostID = tempObj.RoleID;
                                        //EmpDetails.EmployeeStatusMasterID = empType;
                                        EmpDetails.ConfirmationDate = tempObj.ConfirmationDate;
                                        confTable.ConfirmationDate = tempObj.ConfirmationDate;
                                        confTable.stageID = ToStage + 1;
                                        confTable.ConfirmationStatus = 4;
                                        confTable.CreatedDate = DateTime.Now;
                                    }
                                    else if (tempObj.ConfirmationStatus == 3)  //  extendProbation
                                    {
                                        confTable.stageID = 1;
                                        corporate.ToStageId = 1;
                                        confTable.ExtendedProbationDate = tempObj.ExtendedProbationDate;
                                        EmpDetails.Probation_Review_Date = tempObj.ExtendedProbationDate;
                                        EmpDetails.ConfirmationStatus = 0;
                                        confTable.ConfirmationStatus = 3;
                                        confTable.CreatedDate = DateTime.Now;
                                    }
                                    else if (tempObj.ConfirmationStatus == 2)  // sendToPIP
                                    {
                                        confTable.stageID = 1;
                                        corporate.ToStageId = 1;
                                        EmpDetails.Probation_Review_Date = tempObj.PIPDate;
                                        EmpDetails.ConfirmationStatus = 0;
                                        confTable.PIPDate = tempObj.PIPDate;
                                        confTable.ConfirmationStatus = 2;
                                        confTable.CreatedDate = DateTime.Now;
                                    }
                                }
                            }
                            else
                            {
                                confTable.stageID = ToStage + 1;
                                confTable.CreatedDate = DateTime.Now;
                            }
                            dbContext.Tbl_HR_ConfirmationStageEvent.AddObject(corporate);
                        }
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
            isAdded = true;
            return isAdded;
        }

        #endregion Confirmation Form Save Records.

        #region Confirmation Form Delete Records.

        public bool DeletecorporateDetails(int CorporateID)
        {
            bool isDeleted = false;
            tbl_CF_CorporateContribution corporateID = dbContext.tbl_CF_CorporateContribution.Where(cd => cd.CorporateID == CorporateID).FirstOrDefault();
            if (CorporateID != null && CorporateID > 0)
            {
                dbContext.DeleteObject(corporateID);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public bool DeleteprojectAchievementDetails(int ProjectAchievementID)
        {
            bool isDeleted = false;
            tbl_CF_ProjectAchievement projectAchievementID = dbContext.tbl_CF_ProjectAchievement.Where(cd => cd.ProjectID == ProjectAchievementID).FirstOrDefault();
            if (projectAchievementID != null)
            {
                dbContext.DeleteObject(projectAchievementID);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public bool DeleteskillAquiredDetails(int SkillAquiredID)
        {
            bool isDeleted = false;
            tbl_CF_SkillAquired skillAquiredID = dbContext.tbl_CF_SkillAquired.Where(cd => cd.SkillAquiredID == SkillAquiredID).FirstOrDefault();
            if (skillAquiredID != null)
            {
                dbContext.DeleteObject(skillAquiredID);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public bool DeleteQualificationDetails(int AddQualificationID)
        {
            bool isDeleted = false;
            tbl_CF_AdditionalQualification addQualificationID = dbContext.tbl_CF_AdditionalQualification.Where(cd => cd.AddQualificationID == AddQualificationID).FirstOrDefault();
            if (addQualificationID != null)
            {
                dbContext.DeleteObject(addQualificationID);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        #endregion Confirmation Form Delete Records.

        #region Confirmation Form Get Records.

        public List<ConfirmationFormViewModel> GetCorporateDetails(int employeeId, int confirmationId, int page, int rows, out int totalCount)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ConfirmationFormViewModel> corporateContribution = (from corporates in dbContext.tbl_CF_CorporateContribution
                                                                         where corporates.EmployeeID == employeeId && corporates.ConfirmationID == confirmationId
                                                                         orderby corporates.EmployeeID descending
                                                                         select new ConfirmationFormViewModel
                                                                         {
                                                                             EmployeeID = corporates.EmployeeID,
                                                                             CorporateId = corporates.CorporateID,
                                                                             AreaOfContribution = corporates.AreaOfContribution.Trim(),
                                                                             ContributionDesc = corporates.ContributionDesc.Trim(),
                                                                             ManagerComments = corporates.ManagerComments.Trim(),
                                                                             ManagerCommentsSecond = corporates.ManagerCommentSecond.Trim(),
                                                                             ReviewerComments = corporates.ReviewerComments.Trim(),
                                                                             HRReviewerComments = corporates.SecondReviewerComments.Trim(),
                                                                         }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from corporates in dbContext.tbl_CF_CorporateContribution
                              where corporates.EmployeeID == employeeId
                              select corporates.EmployeeID).Count();

                return corporateContribution;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProjectAchievement> GetProjectAchievementDetails(int employeeId, int? confirmationId, int page, int rows, out int totalCount)
        {
            try
            {
                int confID = Convert.ToInt32(confirmationId);
                dbContext = new HRMSDBEntities();
                List<ProjectAchievement> projectAchievement = (from corporates in dbContext.tbl_CF_ProjectAchievement
                                                               where corporates.EmployeeID == employeeId && corporates.ConfirmationID == confID

                                                               orderby corporates.EmployeeID descending
                                                               select new ProjectAchievement
                                                               {
                                                                   EmpID = corporates.EmployeeID,
                                                                   ProjAchieveID = corporates.ProjectID,
                                                                   ConfirmationID = corporates.ConfirmationID,
                                                                   ProjectDesc = corporates.ProjectDescription.Trim(),
                                                                   ProjectAchievements = corporates.ProjectAchievement.Trim(),
                                                                   StartDate = corporates.StartDate,
                                                                   EndDate = corporates.EndDate,
                                                                   NameOfManager = corporates.NameOfProjManager.Trim(),
                                                               }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from corporates in dbContext.tbl_CF_ProjectAchievement
                              where corporates.EmployeeID == employeeId
                              select corporates.EmployeeID).Count();

                return projectAchievement;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SkillsAquired> GetSkillAquiredDetails(int employeeId, int confirmationId, int page, int rows, out int totalCount)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<SkillsAquired> skillAquired = (from corporates in dbContext.tbl_CF_SkillAquired
                                                    where corporates.EmployeeID == employeeId && corporates.ConfirmationID == confirmationId
                                                    orderby corporates.EmployeeID descending
                                                    select new SkillsAquired
                                                    {
                                                        SkillEmployeeID = corporates.EmployeeID,
                                                        SkillsAquiredID = corporates.SkillAquiredID,
                                                        ConfirmationID = corporates.ConfirmationID,
                                                        SkillName = corporates.SkillName.Trim(),
                                                        AquiredThrough = corporates.SkillAquiredThrough.Trim(),
                                                        ProjectUsefulness = corporates.SkillUsefulness.Trim(),
                                                    }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from corporates in dbContext.tbl_CF_SkillAquired
                              where corporates.EmployeeID == employeeId
                              select corporates.EmployeeID).Count();

                return skillAquired;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AdditionalQualification> GetAddQualificationDetails(int employeeId, int ConfirmationId, int page, int rows, out int totalCount)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<AdditionalQualification> skillAquired = (from corporates in dbContext.tbl_CF_AdditionalQualification
                                                              where corporates.EmployeeID == employeeId && corporates.ConfirmationID == ConfirmationId

                                                              orderby corporates.EmployeeID descending
                                                              select new AdditionalQualification
                                                              {
                                                                  QualifEmployeeID = corporates.EmployeeID,
                                                                  AddQualificationID = corporates.AddQualificationID,
                                                                  ConfirmationID = corporates.ConfirmationID,
                                                                  Title = corporates.Title.Trim(),
                                                                  FromDuration = corporates.FromDuration,
                                                                  ToDuration = corporates.ToDuration,
                                                                  skill = corporates.Type,
                                                                  AddSkillAquired = corporates.SkillAquired.Trim(),
                                                                  AddSkillUsed = corporates.SkillUsefulness.Trim(),
                                                              }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from corporates in dbContext.tbl_CF_SkillAquired
                              where corporates.EmployeeID == employeeId
                              select corporates.EmployeeID).Count();
                return skillAquired;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetSkillDescription(int desc)
        {
            string qualifDescription;
            try
            {
                dbContext = new HRMSDBEntities();
                HRMS_tbl_PM_Tools emp = dbContext.HRMS_tbl_PM_Tools.Where(ed => ed.ToolID == desc).FirstOrDefault();
                tbl_PM_QualificationType qualification = dbContext.tbl_PM_QualificationType.Where(qualificationType => qualificationType.QualificationTypeID == desc).FirstOrDefault();
                qualifDescription = qualification.QualificationTypeName.Trim();
            }
            catch (Exception)
            {
                throw;
            }
            return qualifDescription;
        }

        public List<tbl_CF_PerformanceHinder> GetPerformanceHinder(int? employeeId)
        {
            List<tbl_CF_PerformanceHinder> performanceHinder = new List<tbl_CF_PerformanceHinder>();
            try
            {
                dbContext = new HRMSDBEntities();
                performanceHinder = (dbContext.tbl_CF_PerformanceHinder.Where(ed => ed.EmployeeID == employeeId)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            if (performanceHinder.Count == 0)
            {
                performanceHinder = null;
                return performanceHinder;
            }
            return performanceHinder;
        }

        public tbl_CF_PerformanceHinders GetPerformanceHinderTable(int? employeeId, int? confirmationID)
        {
            tbl_CF_PerformanceHinders performanceHinder = new tbl_CF_PerformanceHinders();
            try
            {
                dbContext = new HRMSDBEntities();
                performanceHinder = (dbContext.tbl_CF_PerformanceHinders.Where(ed => ed.EmployeeID == employeeId && ed.ConfirmationID == confirmationID)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return performanceHinder;
        }

        public int? GetConfirmationID(int? employeeId)
        {
            tbl_CF_Confirmation confirmation = new tbl_CF_Confirmation();
            try
            {
                dbContext = new HRMSDBEntities();
                confirmation = (dbContext.tbl_CF_Confirmation.Where(ed => ed.EmployeeID == employeeId)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            if (confirmation == null)
                return 0;
            else
                return confirmation.ConfirmationID;
        }

        public List<tbl_CF_ValueDrivers> GetParameters(int? employeeId, int? ConfirmationID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                int RoleId = (from id in dbContext.HRMS_tbl_PM_Employee

                              where id.EmployeeID == employeeId
                              select id.PostID.HasValue ? id.PostID.Value : 0).FirstOrDefault();

                if (RoleId > 0)
                {
                    HRMS_tbl_PM_Role LoginRole = (from name in dbContext.HRMS_tbl_PM_Role
                                                  where name.RoleID == RoleId
                                                  select name).FirstOrDefault();
                    List<int?> parameter = (from name in dbContext.tbl_PA_CompetencyRoleApplicability
                                            where name.RoleID == RoleId
                                            select name.CompetencyID).ToList();
                    List<Int32> parameterList = new List<int>();
                    foreach (var param in parameter)
                        parameterList.Add(Convert.ToInt32(param));
                    int empID = Convert.ToInt32(employeeId);  // need to convert
                    List<string> paramdesc = new List<string>();
                    foreach (var par in parameterList)
                    {
                        string ParamDesc = (from name in dbContext.tbl_PA_Competency_Master
                                            where name.CompetencyID == par
                                            select name.Competency).FirstOrDefault();
                        paramdesc.Add(ParamDesc);
                    }
                    bool ret = saveParametersDescInValueDriver(parameterList, paramdesc, employeeId, ConfirmationID);
                    if (ret)
                    { }
                    else
                    {
                        return null;
                    }
                    tbl_CF_ValueDrivers check = (from chk in dbContext.tbl_CF_ValueDrivers
                                                 where chk.EmployeeID == empID
                                                 select chk).FirstOrDefault();
                    List<tbl_CF_ValueDrivers> valueDriver = new List<tbl_CF_ValueDrivers>();
                    List<tbl_CF_ValueDrivers> finalValueDriver = new List<tbl_CF_ValueDrivers>();
                    tbl_CF_ValueDrivers objValueDriver;
                    if (check != null)
                    {
                        foreach (var val in paramdesc)
                        {
                            valueDriver = (from vals in dbContext.tbl_CF_ValueDrivers
                                           where vals.ConfirmationID == ConfirmationID && vals.EmployeeID == empID && vals.ParameterDescription == val
                                           orderby vals.EmployeeID
                                           select vals).ToList();
                            foreach (var vals in valueDriver)
                            {
                                objValueDriver = new tbl_CF_ValueDrivers();
                                objValueDriver.ParameterID = vals.ParameterID;
                                objValueDriver.ParameterDescription = vals.ParameterDescription.Trim();
                                objValueDriver.EmployeeID = vals.EmployeeID;
                                objValueDriver.ConfirmationID = vals.ConfirmationID;
                                objValueDriver.CompetencyID = vals.CompetencyID;
                                objValueDriver.EmployeeComments = vals.EmployeeComments == null ? "" : vals.EmployeeComments.Trim();
                                objValueDriver.SelfRating = vals.SelfRating == null ? 0 : vals.SelfRating;
                                objValueDriver.ManagerRating1 = vals.ManagerRating1 == null ? 0 : vals.ManagerRating1;
                                objValueDriver.ManagerComments1 = vals.ManagerComments1 == null ? "" : vals.ManagerComments1.Trim();
                                objValueDriver.OverallManagerComments = vals.OverallManagerComments == null ? "" : vals.OverallManagerComments.Trim();
                                objValueDriver.OverallManagerRating = vals.OverallManagerRating == null ? 0 : vals.OverallManagerRating;
                                finalValueDriver.Add(objValueDriver);
                            }
                        }
                    }
                    else
                    {
                        foreach (var val in paramdesc)
                        {
                            objValueDriver = new tbl_CF_ValueDrivers();
                            objValueDriver.EmployeeID = empID;
                            objValueDriver.ConfirmationID = Convert.ToInt32(ConfirmationID);
                            objValueDriver.ParameterDescription = val.Trim();
                            finalValueDriver.Add(objValueDriver);
                        }
                    }
                    return finalValueDriver;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_CF_GoalAspire GetGoalAspire(int? employeeId, int? ConfirmationID)
        {
            tbl_CF_GoalAspire goalAspire = new tbl_CF_GoalAspire();
            try
            {
                dbContext = new HRMSDBEntities();
                goalAspire = (dbContext.tbl_CF_GoalAspire.Where(ed => ed.EmployeeID == employeeId && ed.ConfirmationID == ConfirmationID)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return goalAspire;
        }

        public List<Int32> GetParametersID(int? employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                int RoleId = (from id in dbContext.HRMS_tbl_PM_Employee

                              where id.EmployeeID == employeeId
                              // where id.RoleID == roleId
                              select id.PostID.HasValue ? id.PostID.Value : 0).FirstOrDefault();
                if (RoleId > 0)
                {
                    HRMS_tbl_PM_Role LoginRole = (from name in dbContext.HRMS_tbl_PM_Role
                                                  where name.RoleID == RoleId
                                                  select name).FirstOrDefault();

                    List<Int32> parameterList = (from name in dbContext.tbl_CF_ParameterRoleMapping
                                                 where name.RoleID == RoleId
                                                 select name.ParameterID).ToList();

                    tbl_CF_ValueDrivers valDriver = (from name in dbContext.tbl_CF_ValueDrivers
                                                     where name.EmployeeID == employeeId
                                                     select name).FirstOrDefault();
                    if (valDriver == null)
                    {
                    }
                    return parameterList;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string CheckLoginUserIsEmpOrManager(int employeeId)
        {
            try
            {
                string str = "";
                var mngr = (from manger in dbContext.tbl_CF_Confirmation
                            where manger.ReportingManager == employeeId
                            select manger.ReportingManager).FirstOrDefault();
                if (mngr != null)
                {
                    str = "Manager";
                }

                var mngr2 = (from manger in dbContext.tbl_CF_Confirmation
                             where manger.ReportingManager2 == employeeId
                             select manger.ReportingManager2).FirstOrDefault();
                if (mngr != null)
                {
                    str = "Manager2";
                }

                var reviewer = (from manger in dbContext.tbl_CF_Confirmation
                                where manger.Reviewer == employeeId
                                select manger.Reviewer).FirstOrDefault();
                if (mngr != null)
                {
                    str = "Reviewer";
                }

                var hr = (from manger in dbContext.tbl_CF_Confirmation
                          where manger.HRReviewer == employeeId
                          select manger.HRReviewer).FirstOrDefault();
                if (mngr != null)
                {
                    str = "HR";
                }
                return str;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<GradeList> getGradeList()
        {
            List<GradeList> grades = new List<GradeList>();
            try
            {
                grades = (from grade in dbContext.tbl_PM_GradeMaster
                          orderby grade.GradeID ascending
                          select new GradeList
                          {
                              Value = grade.GradeID,
                              Text = grade.Grade
                          }).ToList();
            }
            catch (Exception)
            {
            }
            return grades;
        }

        public List<EmployeeStatusList> getEmployeeStatusList()
        {
            List<EmployeeStatusList> employeestatus = new List<EmployeeStatusList>();
            try
            {
                employeestatus = (from status in dbContext.tbl_PM_EmployeeStatus
                                  where status.EmployeeStatusID == 1 || status.EmployeeStatusID == 2
                                  orderby status.EmployeeStatusID
                                  select new EmployeeStatusList
                                  {
                                      Value = status.EmployeeStatusID,
                                      Text = status.EmployeeStatus
                                  }).ToList();
            }
            catch (Exception)
            {
            }
            return employeestatus;
        }

        public List<EmpRole> getRoleList()
        {
            List<EmpRole> roleList = new List<EmpRole>();
            try
            {
                roleList = (from resource in dbContext.HRMS_tbl_PM_Role

                            select new EmpRole
                            {
                                Value = resource.RoleID,
                                Text = resource.RoleDescription
                            }).ToList();
            }
            catch (Exception)
            {
            }
            return roleList;
        }

        public List<EmployeementTypeList> getEmployeementType()
        {
            List<EmployeementTypeList> employeementType = new List<EmployeementTypeList>();
            try
            {
                employeementType = (from status in dbContext.tbl_PM_EmployeeStatusMaster
                                    where status.EmployeementStatusId == 1 || status.EmployeementStatusId == 2
                                    select new EmployeementTypeList
                                    {
                                        Value = status.EmployeementStatusId,
                                        Text = status.EmployeementStatus
                                    }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return employeementType;
        }

        public tbl_CF_Confirmation getConfirmationId(int? employeeID)
        {
            tbl_CF_Confirmation confTable = new tbl_CF_Confirmation();
            try
            {
                confTable = (from data in dbContext.tbl_CF_Confirmation
                             where data.EmployeeID == employeeID
                             orderby data.ConfirmationID descending
                             select data).FirstOrDefault();
            }
            catch (Exception)
            {
            }
            return confTable;
        }

        public tbl_CF_TempConfirmation GetTempConfirmation(int confirmationID)
        {
            tbl_CF_TempConfirmation tempConfirmation = new tbl_CF_TempConfirmation();
            try
            {
                dbContext = new HRMSDBEntities();
                tempConfirmation = (dbContext.tbl_CF_TempConfirmation.Where(ed => ed.ConfirmationID == confirmationID)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return tempConfirmation;
        }
        public DataSet GetTempConfirmationNew(int confirmationID)
        {
            ConfirmationFormViewModel tempConfirmation = new ConfirmationFormViewModel();
            DataSet dset = new DataSet();
            try
            {
                string constring = GetADOConnectionString();
                SqlConnection con = new SqlConnection(constring);
                con.Open();

                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter dadapter = new SqlDataAdapter();
                cmd.CommandText = "Select * from tbl_CF_TempConfirmation where ConfirmationID ='" + confirmationID + "' ";//write sp name here
                cmd.Connection = con;

                dadapter = new SqlDataAdapter(cmd.CommandText, con);
                dadapter.Fill(dset);
                //using (SqlDataReader reader = cmd.ExecuteReader())
                //{
                //    tempConfirmation = ;

                //}
                //tempConfirmation= cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            return dset;
        }
        public RatingMinMax GetRating()
        {
            RatingMinMax ratingMinMax = new RatingMinMax();

            try
            {
                Decimal? ratingMin = (from c in dbContext.tbl_PA_Rating_Master select c.Percentage).Min();
                ratingMinMax.min = Convert.ToInt32(ratingMin);
                Decimal? ratingMax = (from c in dbContext.tbl_PA_Rating_Master select c.Percentage).Max();
                ratingMinMax.max = Convert.ToInt32(ratingMax);
            }
            catch (Exception)
            {
                throw;
            }
            return ratingMinMax;
        }

        public List<ReportingToList_Emp> GetManagerList_Emp(int empID)
        {
            List<ReportingToList_Emp> resourcepool = new List<ReportingToList_Emp>();
            try
            {
                resourcepool = (from resource in dbContext.HRMS_tbl_PM_Employee
                                where resource.Status == false
                                orderby resource.EmployeeName ascending
                                select new ReportingToList_Emp
                                {
                                    EmployeeId = resource.EmployeeID,
                                    EmployeeName = resource.EmployeeName
                                }).ToList();

                List<ReportingToList_Emp> Managers = new List<ReportingToList_Emp>();
                foreach (var item in resourcepool)
                {
                    if (item.EmployeeId != empID)
                    {
                        Managers.Add(item);
                    }
                    else
                        continue;
                }
                return Managers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ValidateConfirmationDetailForm(int? empID)
        {
            //On Approve btn need to check whether employee has fill al the details
            int countCorporateContribution = dbContext.tbl_CF_CorporateContribution.Where(ed => ed.EmployeeID == ed.EmployeeID).Count();
            if (countCorporateContribution > 0)
            {
                int countProjectAchievement = dbContext.tbl_CF_ProjectAchievement.Where(ed => ed.EmployeeID == ed.EmployeeID).Count();
                if (countProjectAchievement > 0)
                {
                    int countAdditionalQualification = dbContext.tbl_CF_AdditionalQualification.Where(ed => ed.EmployeeID == ed.EmployeeID).Count();
                    if (countAdditionalQualification > 0)
                    {
                        int countValueDriver = dbContext.tbl_CF_ValueDrivers.Where(ed => ed.EmployeeID == ed.EmployeeID).Count();
                        if (countValueDriver > 0)
                        {
                            int countSkillAquired = dbContext.tbl_CF_SkillAquired.Where(ed => ed.EmployeeID == ed.EmployeeID).Count();
                            if (countSkillAquired > 0)
                            {
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        #endregion Confirmation Form Get Records.

        #region ConfirmationProcessChanges

        public DataSet GetAutoTriggerMailDetailsForConfirmation()
        {
            try
            {
                string constring = GetADOConnectionString();
                SqlConnection con = new SqlConnection(constring);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetEmployeeDetailsForSendMailConfirmation_SP";//write sp name here
                cmd.Connection = con;
                // string records = "Select PRE.projectId,proj.projectName,PRE.employeeId,convert(varchar(12), PRE.expectedEndDate, 101) AllocationEndDate,empEmployee.employeename EmpName,empEmployee.emailId EmpEMailId,empManager.employeename ManagerName,empManager.emailId ManagerEmailId from tbl_PM_ProjectEmployeeRole PRE left join Tbl_PM_Project proj on proj.projectId = PRE.projectId left join HRMS_tbl_PM_Employee empEmployee on empEmployee.employeeid = PRE.employeeId left join HRMS_tbl_PM_Employee empManager on empManager.employeeid = empEmployee.reportingTo where PRE.expectedEndDate = '" + sevenDaysBefore + "'";

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dsConfirmationDetails = new DataSet();
                da.Fill(dsConfirmationDetails);
                return dsConfirmationDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetAutoTriggerMailDetailsForConfirmationBeforProbation()
        {
            try
            {
                string constring = GetADOConnectionString();
                SqlConnection con = new SqlConnection(constring);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetEmployeeDetailsForSendMailConfirmationBeforeProbationDate_SP";//write sp name here
                cmd.Connection = con;
                // string records = "Select PRE.projectId,proj.projectName,PRE.employeeId,convert(varchar(12), PRE.expectedEndDate, 101) AllocationEndDate,empEmployee.employeename EmpName,empEmployee.emailId EmpEMailId,empManager.employeename ManagerName,empManager.emailId ManagerEmailId from tbl_PM_ProjectEmployeeRole PRE left join Tbl_PM_Project proj on proj.projectId = PRE.projectId left join HRMS_tbl_PM_Employee empEmployee on empEmployee.employeeid = PRE.employeeId left join HRMS_tbl_PM_Employee empManager on empManager.employeeid = empEmployee.reportingTo where PRE.expectedEndDate = '" + sevenDaysBefore + "'";

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dsConfirmationDetails = new DataSet();
                da.Fill(dsConfirmationDetails);
                return dsConfirmationDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetADOConnectionString()
        {
            HRMSDBEntities ctx = new HRMSDBEntities(); //create your entity object here
            EntityConnection ec = (EntityConnection)ctx.Connection;
            SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
            string adoConnStr = sc.ConnectionString;
            return adoConnStr;
        }

        public List<ConfirmationDetailsViewModel> LoadGridConfirmationDetailsList(string searchText, string field, string fieldchild, int page, int rows, int loginUserId, out int totalCount)
        {
            try
            {
                List<ConfirmationDetailsViewModel> ConfirmationList = new List<ConfirmationDetailsViewModel>();
                List<ConfirmationDetailsViewModel> DetailsForAdmin = new List<ConfirmationDetailsViewModel>();
                List<ConfirmationDetailsViewModel> DetailsForEmployee = new List<ConfirmationDetailsViewModel>();

                int FieldChild = 0;
                if (fieldchild != "")
                {
                    FieldChild = Convert.ToInt32(fieldchild);
                }
                string Loginemployeecode = string.Empty;
                string[] loginemployeerole = { };
                EmployeeDAL empdal = new EmployeeDAL();
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);

                HRMS_tbl_PM_Employee loginrolescheck = empdal.GetEmployeeDetails(employeeID);

                if (loginrolescheck != null || loginrolescheck.EmployeeID > 0)
                {
                    Loginemployeecode = loginrolescheck.EmployeeCode;
                    loginemployeerole = Roles.GetRolesForUser(Loginemployeecode);
                }

                //HR Admin record
                if (loginemployeerole.Contains("HR Admin"))
                {
                    DetailsForAdmin = (from emp in dbContext.tbl_CF_Confirmation
                                       join employee in dbContext.HRMS_tbl_PM_Employee on emp.EmployeeID equals employee.EmployeeID into emplst
                                       from emplist in emplst.DefaultIfEmpty()
                                       join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.stageID + 1 equals stagename.ConfirmationStageID into stages
                                       //join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.stageID equals stagename.ConfirmationStageID into stages
                                       from stagedesc in stages.DefaultIfEmpty()
                                       where (emplist.EmployeeName.Contains(searchText) || emplist.EmployeeCode.Contains(searchText))
                                             && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emplist.BusinessGroupID == FieldChild : field == "Organization Unit" ? emplist.LocationID == FieldChild : field == "Stage Name" ? emp.stageID == (FieldChild - 1) : FieldChild == 0))) //field search
                                             && emp.IsNewProcess == true
                                       join ese in dbContext.Tbl_HR_ConfirmationStageEvent on emp.ConfirmationID equals ese.ConfirmationID into eventStageRecord  // Fix to add red Image support
                                       select new ConfirmationDetailsViewModel
                                       {
                                           Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                           ConfirmationID = emp.ConfirmationID,
                                           EmployeeId = emp.EmployeeID,
                                           EmployeeCode = emplist.EmployeeCode,
                                           EmployeeName = emplist.EmployeeName,
                                           JoiningDate = emplist.JoiningDate,
                                           ProbationReviewDate = emplist.Probation_Review_Date,
                                           InitiatedDate = emp.ConfirmationInitiationDate,
                                           StageID = emp.stageID,
                                           Stage = emp.stageID == 4 ? "Confirmation Completed" : stagedesc.ConfirmationStage,
                                           IsAdmin = true,
                                           IsManager = emp.ReportingManager == employeeID ? true : false,
                                           IsFurtherApprover = emp.FurtherApproverId == employeeID ? true : false,
                                           IsFurtherApproverPresent = emp.IsFurtherApprovalStagePresent,
                                           IsFurtherApproverCleared = emp.IsFurtherApprovalStageCleared
                                       }).ToList();
                }
                else
                {
                    DetailsForEmployee = (from emp in dbContext.tbl_CF_Confirmation
                                          join employee in dbContext.HRMS_tbl_PM_Employee on emp.EmployeeID equals employee.EmployeeID into emplst
                                          from emplist in emplst.DefaultIfEmpty()
                                          join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.stageID + 1 equals stagename.ConfirmationStageID into stages
                                          //join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.stageID equals stagename.ConfirmationStageID into stages
                                          from stagedesc in stages.DefaultIfEmpty()
                                          where (emplist.EmployeeName.Contains(searchText) || emplist.EmployeeCode.Contains(searchText))
                                                && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emplist.BusinessGroupID == FieldChild : field == "Organization Unit" ? emplist.LocationID == FieldChild : field == "Stage Name" ? emp.stageID == FieldChild : FieldChild == 0))) //field search
                                                && (emp.ReportingManager == employeeID || emp.FurtherApproverId == employeeID)
                                                && emp.IsNewProcess == true //&& emplist.Probation_Review_Date <= DateTime.Now
                                          join ese in dbContext.Tbl_HR_ConfirmationStageEvent on emp.ConfirmationID equals ese.ConfirmationID into eventStageRecord  // Fix to add red Image support
                                          select new ConfirmationDetailsViewModel
                                          {
                                              Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatatime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                              ConfirmationID = emp.ConfirmationID,
                                              EmployeeId = emp.EmployeeID,
                                              EmployeeCode = emplist.EmployeeCode,
                                              EmployeeName = emplist.EmployeeName,
                                              JoiningDate = emplist.JoiningDate,
                                              ProbationReviewDate = emplist.Probation_Review_Date,
                                              InitiatedDate = emp.ConfirmationInitiationDate,
                                              StageID = emp.stageID,
                                              Stage = emp.stageID == 4 ? "Confirmation Completed" : stagedesc.ConfirmationStage,
                                              IsManager = emp.ReportingManager == employeeID ? true : false,
                                              IsFurtherApprover = emp.FurtherApproverId == employeeID ? true : false,
                                              IsFurtherApproverPresent = emp.IsFurtherApprovalStagePresent,
                                              IsFurtherApproverCleared = emp.IsFurtherApprovalStageCleared
                                          }).ToList();
                }
                ConfirmationList = DetailsForAdmin.Union(DetailsForEmployee).ToList();
                totalCount = ConfirmationList.Count();
                //return ConfirmationList.Skip((page - 1) * rows).Take(rows).ToList();
                return ConfirmationList.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProjectAchievement> GetProjectDetailsConfirmation(int EmployeeId, int page, int rows, out int totalCount)
        {
            try
            {
                List<ProjectAchievement> ProjectDetailsConfirmation = new List<ProjectAchievement>();

                var ProjectDetails = dbContext.ProjectDetailsForConfirmation_sp(EmployeeId);
                //if (ProjectReviewerDetail.Count() > 0)
                //{
                ProjectDetailsConfirmation = (from reviewerdetails in ProjectDetails
                                              select new ProjectAchievement
                                              {
                                                  EmpID = EmployeeId,
                                                  ProjectEndAppraisalStausID = reviewerdetails.ProjectEndAppraisalStausID.HasValue ? reviewerdetails.ProjectEndAppraisalStausID.Value : 0,
                                                  ProjectEmployeeRoleID = reviewerdetails.ProjectEmployeeRoleID,
                                                  ProjectID = reviewerdetails.ProjectId.HasValue ? reviewerdetails.ProjectId.Value : 0,
                                                  ProjectName = reviewerdetails.projectname,
                                                  NameOfManager = reviewerdetails.ManagerName,
                                                  AllocationEndDate = reviewerdetails.AllocationEndDate,
                                                  SystemDate = reviewerdetails.currentDate
                                              }).ToList();
                //}
                totalCount = ProjectDetailsConfirmation.Count();
                //return ProjectDetailsConfirmation.Skip((page - 1) * rows).Take(rows).ToList();
                return ProjectDetailsConfirmation.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveParameterDetailsConfirmation(List<ConfirmationParameter> empCorporate)
        {
            bool isAdded = false;
            if (empCorporate != null)
            {
                dbContext = new HRMSDBEntities();
                if (empCorporate != null)
                {
                    int count = empCorporate.Count();
                    int comptncyID = 0;
                    int confID = 0;
                    string str;
                    int emp = 0;
                    for (int i = 0; i < count; i++)
                    {
                        emp = empCorporate[0].employeeID;
                        comptncyID = empCorporate[i].competencyID;
                        confID = empCorporate[i].confirmationID;
                        tbl_CF_ValueDrivers objValueDrivers = dbContext.tbl_CF_ValueDrivers.Where(ed => ed.CompetencyID == comptncyID && ed.EmployeeID == emp && ed.ConfirmationID == confID).FirstOrDefault();
                        if (objValueDrivers != null)
                        {
                            if (empCorporate[0].IsManagerOrEmployee == "Manager")
                            {
                                objValueDrivers.ManagerRating1 = empCorporate[i].ManagerRating1 == null ? 0 : empCorporate[i].ManagerRating1;
                                objValueDrivers.ManagerComments1 = empCorporate[i].MngrComments1 == null ? "" : empCorporate[i].MngrComments1.Trim();
                            }
                            dbContext.SaveChanges();
                        }
                    }
                    List<tbl_CF_ValueDrivers> objValueDriver = dbContext.tbl_CF_ValueDrivers.Where(ed => ed.EmployeeID == emp).ToList();
                    foreach (var item in objValueDriver)
                    {
                        if (item != null)
                        {
                            if (empCorporate[0].IsManagerOrEmployee == "Manager")
                            {
                                item.OverallManagerRating = empCorporate[count - 1].OverallManagerRating == null ? 0 : empCorporate[count - 1].OverallManagerRating;
                                item.OverallManagerComments = empCorporate[count - 1].OverallManagerRatingComments == null ? "" : empCorporate[count - 1].OverallManagerRatingComments.Trim();
                                dbContext.SaveChanges();
                            }
                        }
                    }
                }
                isAdded = true;
            }
            else
                isAdded = true;
            return isAdded;
        }

        public bool SaveConfirmationFormQuetionsDetails(ConfirmationFormViewModel empCorporate)
        {
            bool isAdded = false;
            tbl_cf_ConfirmationFormQuetionsComments emp = dbContext.tbl_cf_ConfirmationFormQuetionsComments.Where(ed => ed.QuetionId == empCorporate.QuestionId).FirstOrDefault();
            if (emp == null || emp.EmployeeID <= 0 || emp.QuetionId < 0)
            {
                tbl_cf_ConfirmationFormQuetionsComments corporate = new tbl_cf_ConfirmationFormQuetionsComments();
                corporate.EmployeeID = Convert.ToInt32(empCorporate.EmployeeIdConfirmation);
                corporate.ConfirmationID = Convert.ToInt32(empCorporate.confirmationID);

                corporate.AreaOfContributionComments = empCorporate.AreaOfContributionComments;
                corporate.BehaviorComments = empCorporate.BehaviourComments;
                corporate.TrainingProgramComments = empCorporate.TrainingProgramComments;

                dbContext.tbl_cf_ConfirmationFormQuetionsComments.AddObject(corporate);
            }
            else
            {
                emp.AreaOfContributionComments = empCorporate.AreaOfContributionComments;
                emp.BehaviorComments = empCorporate.BehaviourComments;
                emp.TrainingProgramComments = empCorporate.TrainingProgramComments;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool SaveApproverCommentsDetails(ConfirmationFormViewModel empCorporate)
        {
            bool isAdded = false;
            tbl_cf_ApproverComments emp = dbContext.tbl_cf_ApproverComments.Where(ed => ed.EmployeeID == empCorporate.EmployeeIdConfirmation && ed.ConfirmationID == empCorporate.confirmationID).FirstOrDefault();
            tbl_CF_Confirmation cnf = dbContext.tbl_CF_Confirmation.Where(ed => ed.EmployeeID == empCorporate.EmployeeIdConfirmation && ed.ConfirmationID == empCorporate.confirmationID).FirstOrDefault();
            if (emp == null)
            {
                tbl_cf_ApproverComments corporate = new tbl_cf_ApproverComments();
                corporate.EmployeeID = Convert.ToInt32(empCorporate.EmployeeIdConfirmation);
                corporate.ConfirmationID = Convert.ToInt32(empCorporate.confirmationID);
                corporate.ReportingManagerComments = empCorporate.ReportingManagerComments.Trim();
                if (empCorporate.HRComments != null)
                    corporate.HRComments = empCorporate.HRComments.Trim();
                if (empCorporate.DUManagerComments != null)
                    corporate.AdditionalApproverComments = empCorporate.DUManagerComments.Trim();
                dbContext.tbl_cf_ApproverComments.AddObject(corporate);
            }
            else
            {
                emp.ReportingManagerComments = empCorporate.ReportingManagerComments.Trim();
                if (empCorporate.HRComments != null)
                    emp.HRComments = empCorporate.HRComments.Trim();
                if (empCorporate.DUManagerComments != null)
                    emp.AdditionalApproverComments = empCorporate.DUManagerComments.Trim();
                if (empCorporate.IsFurtherApproverNeeded == true && empCorporate.IsManagerOrEmployee == "HR")
                {
                    cnf.IsFurtherApprovalStagePresent = true;
                    cnf.FurtherApproverId = empCorporate.FurtherApproverId;
                }
                else if (empCorporate.IsManagerOrEmployee == "HR")
                    cnf.IsFurtherApprovalStagePresent = null;
                if (empCorporate.IsManagerOrEmployee == "FurtherApprover")
                    cnf.IsFurtherApprovalStageCleared = true;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool saveHrClosureFormDetails(ConfirmationFormViewModel empCorporate)
        {
            bool isAdded = false;
            tbl_CF_TempConfirmation tempConf = dbContext.tbl_CF_TempConfirmation.Where(ed => ed.ConfirmationID == empCorporate.confirmationID).FirstOrDefault();
            tbl_CF_Confirmation ConfDetails = dbContext.tbl_CF_Confirmation.Where(ed => ed.ConfirmationID == empCorporate.confirmationID).FirstOrDefault();
            HRMS_tbl_PM_Employee EmpDetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == empCorporate.EmployeeIdConfirmation).FirstOrDefault();
            if (empCorporate.IsManagerOrEmployee == "Manager")
            {
                if (tempConf == null)
                {
                    tbl_CF_TempConfirmation corporate = new tbl_CF_TempConfirmation();
                    corporate.ConfirmationID = Convert.ToInt32(empCorporate.confirmationID);
                    dbContext.tbl_CF_TempConfirmation.AddObject(corporate);

                    if (empCorporate.IsAcceptedOrExtended == "accept")
                    {
                        corporate.ConfirmationStatus = 4;
                        ConfDetails.ConfirmationStatus = 4;
                    }
                    else if (empCorporate.IsAcceptedOrExtended == "extend")
                    {
                        corporate.ConfirmationStatus = 3;
                        ConfDetails.ConfirmationStatus = 3;
                    }
                    else if (empCorporate.IsAcceptedOrExtended == "sendPIP")
                    {
                        corporate.ConfirmationStatus = 2;
                        ConfDetails.ConfirmationStatus = 2;
                    }
                }
                else
                {
                    if (empCorporate.IsAcceptedOrExtended == "accept")
                    {
                        tempConf.ConfirmationStatus = 4;
                        ConfDetails.ConfirmationStatus = 4;
                    }
                    else if (empCorporate.IsAcceptedOrExtended == "extend")
                    {
                        tempConf.ConfirmationStatus = 3;
                        ConfDetails.ConfirmationStatus = 3;
                    }
                    else if (empCorporate.IsAcceptedOrExtended == "sendPIP")
                    {
                        tempConf.ConfirmationStatus = 2;
                        ConfDetails.ConfirmationStatus = 2;
                    }
                }
            }
            else
            {
                if (tempConf != null)
                {
                    if (empCorporate.IsAcceptedOrExtended == "accept")
                    {
                        tempConf.EmployeeStatusID = Convert.ToInt32(empCorporate.empStatus);
                        //tempConf.EmployeeType = empCorporate.empType;
                        // tempConf.RoleID = Convert.ToInt32(empCorporate.roleName);
                        // tempConf.GradeID = Convert.ToInt32(empCorporate.gradeName);
                        if (empCorporate.ConfirmationDate == DateTime.MinValue)
                            tempConf.ConfirmationDate = null;
                        else
                            tempConf.ConfirmationDate = empCorporate.ConfirmationDate;
                        tempConf.ConfirmationComments = empCorporate.ConfirmationComments == null ? " " : empCorporate.ConfirmationComments.Trim();
                        tempConf.ConfirmationStatus = 4;
                        ConfDetails.ConfirmationStatus = 4;
                    }
                    else if (empCorporate.IsAcceptedOrExtended == "extend")
                    {
                        if (empCorporate.ExtendProbationDate == DateTime.MinValue)
                            tempConf.ExtendedProbationDate = null;
                        else
                            tempConf.ExtendedProbationDate = empCorporate.ExtendProbationDate;
                        tempConf.ExtensionComments = empCorporate.ProbationComments == null ? " " : empCorporate.ProbationComments.Trim();
                        tempConf.ConfirmationStatus = 3;
                        ConfDetails.ConfirmationStatus = 3;
                        tempConf.NumberOfDaysExtension = empCorporate.NumberOfDaysProbation;
                    }
                    else if (empCorporate.IsAcceptedOrExtended == "sendPIP")
                    {
                        tempConf.PIPComments = empCorporate.PIPComments == null ? " " : empCorporate.PIPComments.Trim();
                        if (empCorporate.PIPDate == DateTime.MinValue)
                            tempConf.PIPDate = null;
                        else
                        {
                            tempConf.PIPDate = empCorporate.PIPDate;
                            try
                            {
                                string constring = GetADOConnectionString();
                                SqlConnection con = new SqlConnection(constring);
                                con.Open();
                                SqlCommand cmd = new SqlCommand();

                                cmd.CommandText = "update tbl_CF_TempConfirmation set PIPstartDate= '" + empCorporate.PIPstartDate + "' where ConfirmationID ='" + empCorporate.confirmationID + "' ";//write sp name here
                                cmd.Connection = con;
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception)
                            {
                                throw;
                            }

                        }
                        tempConf.NumberOfDaysExtension = empCorporate.NumberOfDaysPIP;
                        tempConf.ConfirmationStatus = 2;
                        ConfDetails.ConfirmationStatus = 2;
                    }
                }
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public tbl_cf_ConfirmationFormQuetionsComments GetQuetionList(int? employeeId, int? confirmationID)
        {
            tbl_cf_ConfirmationFormQuetionsComments quetionList = new tbl_cf_ConfirmationFormQuetionsComments();
            try
            {
                dbContext = new HRMSDBEntities();
                quetionList = (dbContext.tbl_cf_ConfirmationFormQuetionsComments.Where(ed => ed.EmployeeID == employeeId && ed.ConfirmationID == confirmationID)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return quetionList;
        }

        public tbl_cf_ApproverComments GetApproverComments(int? employeeId, int? confirmationID)
        {
            tbl_cf_ApproverComments quetionList = new tbl_cf_ApproverComments();
            try
            {
                dbContext = new HRMSDBEntities();
                quetionList = (dbContext.tbl_cf_ApproverComments.Where(ed => ed.EmployeeID == employeeId && ed.ConfirmationID == confirmationID)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return quetionList;
        }

        public bool ApproveConfirmationFormDetails(int? empID, int confirmationId, string IsManagerOrEmployee, string ReportingMangerComment, string HrComments)
        {
            bool isAdded = false;
            dbContext = new HRMSDBEntities();
            try
            {
                EmployeeDAL employeeDAL = new EmployeeDAL();
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);

                int UserID = 0;
                // update tbl_CF_Confirmation table
                tbl_CF_Confirmation confTable = (from data in dbContext.tbl_CF_Confirmation
                                                 where data.EmployeeID == empID && data.ConfirmationID == confirmationId
                                                 select data).FirstOrDefault();

                // get ToStageId
                int ToStage = (from id in dbContext.Tbl_HR_ConfirmationStageEvent
                               where id.ConfirmationID == confTable.ConfirmationID
                               orderby id.EventDatatime descending
                               select id.ToStageId.HasValue ? id.ToStageId.Value : 0).FirstOrDefault();

                Tbl_HR_ConfirmationStageEvent confEvent = (from data in dbContext.Tbl_HR_ConfirmationStageEvent
                                                           where data.ConfirmationID == confTable.ConfirmationID && data.ToStageId == ToStage
                                                           select data).FirstOrDefault();
                UserID = loginuser.EmployeeID;

                if (confTable == null)
                {
                    isAdded = false;
                    return isAdded;
                }
                else
                {
                    //if (confTable.stageID == 4)
                    //{
                    //    //for selecting latest entry with From stage id is 3
                    //    Tbl_HR_ConfirmationStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ConfirmationStageEvent
                    //                                                 where ((empInfo.FromStageId == 3 && empInfo.ToStageId == 4) || (empInfo.FromStageId == 5 && empInfo.ToStageId == 4)) && empInfo.ConfirmationID == confTable.ConfirmationID
                    //                                                 orderby empInfo.EventDatatime descending
                    //                                                 select empInfo).FirstOrDefault();
                    //    //for selecting total no. of entries of From stage id 4
                    //    List<Tbl_HR_ConfirmationStageEvent> TotalRecords = new List<Tbl_HR_ConfirmationStageEvent>();
                    //    if (LatestEntry != null)
                    //    {
                    //        TotalRecords = (from total in dbContext.Tbl_HR_ConfirmationStageEvent
                    //                        where total.ConfirmationID == confTable.ConfirmationID && total.Action == "Move Ahead" && total.EventDatatime > LatestEntry.EventDatatime
                    //                        select total).ToList();
                    //    }
                    //    Tbl_HR_ConfirmationStageEvent corporate = new Tbl_HR_ConfirmationStageEvent();
                    //    corporate.ConfirmationID = confTable.ConfirmationID;
                    //    corporate.UserId = UserID;
                    //    corporate.FromStageId = ToStage;
                    //    corporate.Action = "Move Ahead";
                    //    corporate.EventDatatime = DateTime.Now;
                    //    confTable.CreatedDate = DateTime.Now;
                    //    if (TotalRecords.Count == 1)
                    //    {
                    //        confTable.stageID = ToStage + 1;
                    //        corporate.ToStageId = ToStage + 1;
                    //    }
                    //    else
                    //        corporate.ToStageId = ToStage;

                    //    dbContext.Tbl_HR_ConfirmationStageEvent.AddObject(corporate);
                    //}
                    //else if (confTable.stageID == 5)
                    //{
                    //    //for selecting latest entry with From stage id is 3
                    //    Tbl_HR_ConfirmationStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ConfirmationStageEvent
                    //                                                 where ((empInfo.FromStageId == 4 && empInfo.ToStageId == 5) || (empInfo.FromStageId == 6 && empInfo.ToStageId == 5)) && empInfo.ConfirmationID == confTable.ConfirmationID
                    //                                                 orderby empInfo.EventDatatime descending
                    //                                                 select empInfo).FirstOrDefault();
                    //    //for selecting total no. of entries of From stage id 4
                    //    List<Tbl_HR_ConfirmationStageEvent> TotalRecords = new List<Tbl_HR_ConfirmationStageEvent>();
                    //    if (LatestEntry != null)
                    //    {
                    //        TotalRecords = (from total in dbContext.Tbl_HR_ConfirmationStageEvent
                    //                        where total.ConfirmationID == confTable.ConfirmationID && total.Action == "Move Ahead" && total.EventDatatime > LatestEntry.EventDatatime
                    //                        select total).ToList();
                    //    }
                    //    Tbl_HR_ConfirmationStageEvent corporate = new Tbl_HR_ConfirmationStageEvent();
                    //    corporate.ConfirmationID = confTable.ConfirmationID;
                    //    corporate.UserId = UserID;
                    //    corporate.FromStageId = ToStage;
                    //    corporate.Action = "Move Ahead";
                    //    corporate.EventDatatime = DateTime.Now;
                    //    confTable.CreatedDate = DateTime.Now;
                    //    if (TotalRecords.Count == 1)
                    //    {
                    //        confTable.stageID = ToStage + 1;
                    //        corporate.ToStageId = ToStage + 1;
                    //    }
                    //    else
                    //        corporate.ToStageId = ToStage;
                    //    dbContext.Tbl_HR_ConfirmationStageEvent.AddObject(corporate);
                    //}
                    //else
                    //{
                    Tbl_HR_ConfirmationStageEvent corporate = new Tbl_HR_ConfirmationStageEvent();
                    corporate.ConfirmationID = confTable.ConfirmationID;
                    corporate.UserId = UserID;
                    corporate.FromStageId = ToStage;
                    corporate.Action = "Approved";
                    if (IsManagerOrEmployee == "FurtherApprover")
                    {
                        corporate.ToStageId = ToStage - 1;
                    }
                    else if (ToStage == 0 || (confTable.IsFurtherApprovalStagePresent == true && ToStage == 1))
                    {
                        corporate.Comments = ReportingMangerComment;
                        corporate.ToStageId = ToStage + 1;
                    }
                    else
                        corporate.ToStageId = 4;
                    if (confTable.IsFurtherApprovalStageCleared == true && ToStage == 1)
                        corporate.ToStageId = 4;
                    corporate.EventDatatime = DateTime.Now;

                    if (ToStage == 1 && confTable.IsFurtherApprovalStagePresent == true)
                    {
                        corporate.Comments = HrComments;
                    }

                    confTable.stageID = corporate.ToStageId;

                    tbl_CF_TempConfirmation tempObj = (from data in dbContext.tbl_CF_TempConfirmation
                                                       where (data.ConfirmationID == confTable.ConfirmationID)
                                                       select data).FirstOrDefault();
                    if (tempObj != null)
                    {
                        //int empType = Convert.ToInt32(tempObj.EmployeeType);
                        HRMS_tbl_PM_Employee EmpDetails = (from data in dbContext.HRMS_tbl_PM_Employee
                                                           where (data.EmployeeID == confTable.EmployeeID)
                                                           select data).FirstOrDefault();
                        if (tempObj.ConfirmationStatus == 4)  // confirmed
                        {
                            EmpDetails.EmployeeStatusID = tempObj.EmployeeStatusID;
                            if (tempObj.RoleID != null && tempObj.RoleID != 0)
                                EmpDetails.PostID = tempObj.RoleID;
                            //EmpDetails.EmployeeStatusMasterID = empType;
                            EmpDetails.ConfirmationDate = tempObj.ConfirmationDate;
                            confTable.ConfirmationDate = tempObj.ConfirmationDate;
                            //if (confTable.FurtherApproverId != null)
                            //{
                            //    confTable.stageID = ToStage + 1;
                            //}
                            if (confTable.IsFurtherApprovalStageCleared == true && ToStage == 1)
                                confTable.stageID = 4;
                            confTable.ConfirmationStatus = 4;
                            confTable.CreatedDate = DateTime.Now;
                        }
                        else if (tempObj.ConfirmationStatus == 3)  //  extendProbation
                        {
                            //confTable.stageID = ToStage + 1;
                            confTable.CreatedDate = DateTime.Now;
                            confTable.ExtendedProbationDate = tempObj.ExtendedProbationDate;
                            //if (confTable.IsFurtherApprovalStageCleared != null && IsManagerOrEmployee != "FurtherApprover")
                            if (IsManagerOrEmployee == "HR")
                            {
                                if (confTable.IsFurtherApprovalStagePresent == true)
                                {
                                    if (confTable.IsFurtherApprovalStageCleared == true)
                                    {
                                        confTable.stageID = 0;
                                        corporate.ToStageId = 0;
                                        EmpDetails.Probation_Review_Date = tempObj.ExtendedProbationDate;
                                        EmpDetails.ConfirmationDate = null;
                                        EmpDetails.ConfirmationStatus = 0;
                                        EmpDetails.EmployeeStatusID = 5;
                                    }
                                }
                                else
                                {
                                    confTable.stageID = 0;
                                    corporate.ToStageId = 0;
                                    EmpDetails.Probation_Review_Date = tempObj.ExtendedProbationDate;
                                    EmpDetails.ConfirmationDate = null;
                                    EmpDetails.ConfirmationStatus = 0;
                                    EmpDetails.EmployeeStatusID = 5;
                                }
                                confTable.ConfirmationStatus = 3;
                                confTable.CreatedDate = DateTime.Now;
                            }
                        }
                        else if (tempObj.ConfirmationStatus == 2)  // sendToPIP
                        {
                            //confTable.stageID = ToStage + 1;
                            confTable.CreatedDate = DateTime.Now;
                            //if (confTable.IsFurtherApprovalStageCleared != null && IsManagerOrEmployee != "FurtherApprover")

                            if (IsManagerOrEmployee == "HR")
                            {
                                if (confTable.IsFurtherApprovalStagePresent == true)
                                {
                                    if (confTable.IsFurtherApprovalStageCleared == true)
                                    {
                                        confTable.stageID = 0;
                                        corporate.ToStageId = 0;
                                        EmpDetails.Probation_Review_Date = tempObj.PIPDate;
                                        EmpDetails.ConfirmationDate = null;
                                        EmpDetails.ConfirmationStatus = 0;
                                        EmpDetails.EmployeeStatusID = 5;
                                    }
                                }
                                else
                                {
                                    confTable.stageID = 0;
                                    corporate.ToStageId = 0;
                                    EmpDetails.Probation_Review_Date = tempObj.PIPDate;
                                    EmpDetails.ConfirmationDate = null;
                                    EmpDetails.ConfirmationStatus = 0;
                                    EmpDetails.EmployeeStatusID = 5;
                                }
                                confTable.PIPDate = tempObj.PIPDate;
                                confTable.ConfirmationStatus = 2;
                                confTable.CreatedDate = DateTime.Now;
                            }

                            //if (IsManagerOrEmployee != "FurtherApprover")
                            //{
                            //    confTable.stageID = 0;
                            //    corporate.ToStageId = 0;
                            //    EmpDetails.Probation_Review_Date = tempObj.PIPDate;
                            //    EmpDetails.ConfirmationStatus = 0;
                            //}
                            //confTable.PIPDate = tempObj.PIPDate;
                            //confTable.ConfirmationStatus = 2;
                            //confTable.CreatedDate = DateTime.Now;
                        }
                    }

                    dbContext.Tbl_HR_ConfirmationStageEvent.AddObject(corporate);
                    //}

                    dbContext.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
            isAdded = true;
            return isAdded;
        }

        public bool RejectConfirmationFormDetails(int? empID, int confirmationId, string IsManagerOrEmployee, string RejectComments)
        {
            bool isAdded = false;
            dbContext = new HRMSDBEntities();
            try
            {
                int UserID = 0;
                EmployeeDAL employeeDAL = new EmployeeDAL();
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                string loginUserId = loginName;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
                UserID = loginuser.EmployeeID;
                tbl_CF_Confirmation confTable = (from data in dbContext.tbl_CF_Confirmation
                                                 where data.EmployeeID == empID && data.ConfirmationID == confirmationId
                                                 select data).FirstOrDefault();

                // get ToStageId
                int ToStage = (from id in dbContext.Tbl_HR_ConfirmationStageEvent
                               where id.ConfirmationID == confTable.ConfirmationID
                               orderby id.EventDatatime descending
                               select id.ToStageId.HasValue ? id.ToStageId.Value : 0).FirstOrDefault();

                Tbl_HR_ConfirmationStageEvent corporate = new Tbl_HR_ConfirmationStageEvent();
                corporate.ConfirmationID = confTable.ConfirmationID;
                corporate.UserId = UserID;
                corporate.FromStageId = ToStage;
                corporate.Action = "Rejected";
                if (IsManagerOrEmployee == "FurtherApprover")
                {
                    corporate.ToStageId = ToStage - 2;
                    confTable.IsFurtherApprovalStageCleared = true;
                }
                else
                    corporate.ToStageId = ToStage - 1;
                corporate.EventDatatime = DateTime.Now;
                corporate.Comments = RejectComments;
                dbContext.Tbl_HR_ConfirmationStageEvent.AddObject(corporate);
                confTable.stageID = corporate.ToStageId;
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
            }
            isAdded = true;
            return isAdded;
        }

        public Tbl_HR_ConfirmationStageEvent getStageDetails(int confirmationId)
        {
            try
            {
                Tbl_HR_ConfirmationStageEvent corporate = dbContext.Tbl_HR_ConfirmationStageEvent.Where(x => x.ConfirmationID == confirmationId).OrderByDescending(x => x.EventDatatime).FirstOrDefault();
                return corporate;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public tbl_CF_Confirmation getConfirmationDetails(int employeeId)
        {
            try
            {
                tbl_CF_Confirmation confirmationDetails = dbContext.tbl_CF_Confirmation.Where(x => x.EmployeeID == employeeId).FirstOrDefault();
                return confirmationDetails;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public tbl_PM_ResourcePool_Managers GetFurtherApproverName(int resourcePoolId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PM_ResourcePool_Managers furtherApprover = dbContext.tbl_PM_ResourcePool_Managers.Where(ed => ed.ResourcePoolID == resourcePoolId && ed.IsPrimaryResponsible == true).FirstOrDefault();
                return furtherApprover;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<FieldChildDetails> GetFieldChildDetailsList(string field)
        {
            try
            {
                List<FieldChildDetails> childDetails = new List<FieldChildDetails>();
                if (field == "Business Group")
                {
                    List<FieldChildDetails> child = (from l in dbContext.tbl_CNF_BusinessGroups
                                                     select new FieldChildDetails
                                                     {
                                                         Id = l.BusinessGroupID,
                                                         Description = l.BusinessGroup
                                                     }).ToList();

                    return child;
                }
                else
                {
                    if (field == "Organization Unit")
                    {
                        List<FieldChildDetails> child = (from l in dbContext.tbl_PM_Location
                                                         select new FieldChildDetails
                                                         {
                                                             Id = l.LocationID,
                                                             Description = l.Location
                                                         }).ToList();

                        return child;
                    }
                    else
                    {
                        if (field == "Stage Name")
                        {
                            List<FieldChildDetails> child = (from expenseStage in dbContext.tbl_CF_ConfirmationStagesNew
                                                             select new FieldChildDetails
                                                             {
                                                                 Id = expenseStage.ConfirmationStageID,
                                                                 Description = expenseStage.ConfirmationStage
                                                             }).ToList();

                            return child;
                        }
                        else
                        {
                            return childDetails;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ShowStatus> GetShowStatusResultConfirmation(string EmployeeId, string ConfirmationId, int page, int rows)
        {
            List<ShowStatus> Result = new List<ShowStatus>();
            List<ShowStatus> NextStageProcess = new List<ShowStatus>();
            List<ShowStatus> ConfirmationResult = new List<ShowStatus>();
            List<ShowStatus> InitiateProcess = new List<ShowStatus>();
            int empID = Convert.ToInt32(EmployeeId);
            int selectedConfirmationId = Convert.ToInt32(ConfirmationId);
            int ToStageID = 0;
            EmployeeDAL employeeDAL = new EmployeeDAL();
            try
            {
                tbl_CF_Confirmation empinfo = dbContext.tbl_CF_Confirmation.Where(ed => ed.ConfirmationID == selectedConfirmationId && ed.EmployeeID == empID).OrderByDescending(ed => ed.CreatedDate).FirstOrDefault();

                var confirmationId = empinfo.ConfirmationID;
                InitiateProcess = (from emp in dbContext.Tbl_HR_ConfirmationStageEvent
                                   join actor in dbContext.HRMS_tbl_PM_Employee on emp.UserId equals actor.EmployeeID into actors
                                   from actorlist in actors.DefaultIfEmpty()
                                   join confirmation in dbContext.tbl_CF_Confirmation on emp.ConfirmationID equals confirmation.ConfirmationID into confirmationlist
                                   from confirm in confirmationlist.DefaultIfEmpty()
                                   join employee in dbContext.HRMS_tbl_PM_Employee on confirm.EmployeeID equals employee.EmployeeID into employees
                                   from emplist in employees.DefaultIfEmpty()
                                   join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.ToStageId equals stagename.ConfirmationStageID into stages
                                   from stagedesc in stages.DefaultIfEmpty()
                                   where emp.ConfirmationID == confirmationId
                                   select new ShowStatus
                                   {
                                       ShowstatusEmployeeCode = emplist.EmployeeCode,
                                       ShowstatusEmployeeId = emplist.EmployeeID,
                                       ShowstatusEmployeeName = emplist.EmployeeName,
                                       ShowstatusCurrentStage = stagedesc.ConfirmationStage == null ? (emp.UserId == confirm.FurtherApproverId ? "Further Approval Stage" : "HR Stage") : ((emp.FromStageId == 2 && emp.ToStageId == 1) ? "Further Approval Stage" : stagedesc.ConfirmationStage),
                                       ShowstatusStageID = emp.ToStageId,
                                       ShowstatusTime = emp.EventDatatime,
                                       ShowstatusActor = actorlist.EmployeeName,
                                       ShowstatusAction = emp.Action,
                                       ShowstatusComments = emp.Comments
                                   }).ToList();

                //ConfirmationResult = (from emp in dbContext.Tbl_HR_ConfirmationStageEvent
                //                      join actor in dbContext.HRMS_tbl_PM_Employee on emp.UserId equals actor.EmployeeID into actors
                //                      from actorlist in actors.DefaultIfEmpty()
                //                      join confirmation in dbContext.tbl_CF_Confirmation on emp.ConfirmationID equals confirmation.ConfirmationID into confirmationlist
                //                      from confirm in confirmationlist.DefaultIfEmpty()
                //                      join employee in dbContext.HRMS_tbl_PM_Employee on confirm.EmployeeID equals employee.EmployeeID into employees
                //                      from emplist in employees.DefaultIfEmpty()
                //                      join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.ToStageId equals stagename.ConfirmationStageID into stages
                //                      from stagedesc in stages.DefaultIfEmpty()
                //                      where emp.ConfirmationID == confirmationId && emp.ToStageId != 1
                //                      select new ShowStatus
                //                      {
                //                          ShowstatusEmployeeCode = emplist.EmployeeCode,
                //                          ShowstatusEmployeeId = emplist.EmployeeID,
                //                          ShowstatusEmployeeName = emplist.EmployeeName,
                //                          ShowstatusCurrentStage = stagedesc.ConfirmationStage,
                //                          ShowstatusStageID = emp.ToStageId,
                //                          ShowstatusTime = emp.EventDatatime,
                //                          ShowstatusActor = actorlist.EmployeeName,
                //                          ShowstatusAction = emp.Action

                //                      }).ToList();
                Tbl_HR_ConfirmationStageEvent LatestEntry = new Tbl_HR_ConfirmationStageEvent();
                if (InitiateProcess.Count != 0)
                {
                    if (InitiateProcess[0].ShowstatusStageID != 4)
                    {
                        LatestEntry = (from emp in dbContext.Tbl_HR_ConfirmationStageEvent
                                       join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.FromStageId + 1 equals stagename.ConfirmationStageID into stages
                                       from stagedesc in stages.DefaultIfEmpty()
                                       where emp.ConfirmationID == confirmationId && emp.FromStageId == empinfo.stageID - 1
                                       orderby emp.EventDatatime descending
                                       select emp).FirstOrDefault();
                    }
                }

                //for selecting total no. of entries of From stage id 4
                if (LatestEntry != null)
                {
                    NextStageProcess = (from emp in dbContext.Tbl_HR_ConfirmationStageEvent
                                        join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.ToStageId + 1 equals stagename.ConfirmationStageID into stages
                                        from stagedesc in stages.DefaultIfEmpty()
                                        where emp.ConfirmationID == confirmationId && emp.FromStageId == LatestEntry.FromStageId && emp.EventDatatime == LatestEntry.EventDatatime
                                        select new ShowStatus
                                        {
                                            ShowstatusCurrentStage = stagedesc.ConfirmationStage,
                                        }).ToList();
                }
                else
                {
                    Tbl_HR_ConfirmationStageEvent LatestEntryNextStageProcess = (from emp in dbContext.Tbl_HR_ConfirmationStageEvent
                                                                                 join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.ToStageId equals stagename.ConfirmationStageID into stages
                                                                                 from stagedesc in stages.DefaultIfEmpty()
                                                                                 where emp.ConfirmationID == confirmationId
                                                                                 orderby emp.EventDatatime descending
                                                                                 select emp).FirstOrDefault();
                    if (LatestEntryNextStageProcess != null)
                        ToStageID = LatestEntryNextStageProcess.ToStageId.HasValue ? LatestEntryNextStageProcess.ToStageId.Value : 0;
                    if (LatestEntryNextStageProcess != null && LatestEntryNextStageProcess.ToStageId != 4)
                    {
                        NextStageProcess = (from emp in dbContext.Tbl_HR_ConfirmationStageEvent
                                            join stagename in dbContext.tbl_CF_ConfirmationStagesNew on emp.ToStageId + 1 equals stagename.ConfirmationStageID into stages
                                            from stagedesc in stages.DefaultIfEmpty()
                                            where emp.ConfirmationID == confirmationId && emp.FromStageId == LatestEntryNextStageProcess.FromStageId && emp.EventDatatime == LatestEntryNextStageProcess.EventDatatime
                                            select new ShowStatus
                                            {
                                                ShowstatusCurrentStage = stagedesc.ConfirmationStage,
                                            }).ToList();
                    }
                }
                if (InitiateProcess.Count() >= 0)
                {
                    HRMS_tbl_PM_Employee employeeDetails = new HRMS_tbl_PM_Employee();
                    for (int i = 0; i < NextStageProcess.Count(); i++)
                    {
                        if (empinfo.stageID == 0 && empinfo.IsFurtherApprovalStagePresent != true)
                        {
                            employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(empinfo.ReportingManager));
                            NextStageProcess[i].showStatus = "Pending for " + employeeDetails.EmployeeName + " to take action";
                        }
                        else if (empinfo.stageID == 1)
                        {
                            NextStageProcess[i].showStatus = "Pending for HR Admin to take action";
                        }
                        else if (empinfo.stageID == 2)
                        {
                            employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(empinfo.FurtherApproverId));
                            NextStageProcess[i].showStatus = "Pending for " + employeeDetails.EmployeeName + " to take action";
                        }
                        else if (empinfo.stageID == 0 && empinfo.IsFurtherApprovalStagePresent == true && empinfo.ConfirmationStatus == 3)
                        {
                            NextStageProcess[i].showStatus = "Probation Extended";
                        }
                        else if (empinfo.stageID == 0 && empinfo.IsFurtherApprovalStagePresent == true && empinfo.ConfirmationStatus == 2)
                        {
                            NextStageProcess[i].showStatus = "Send for PIP";
                        }
                        else if (empinfo.stageID == 0 && empinfo.IsFurtherApprovalStagePresent == true && empinfo.ConfirmationStatus == 4)
                        {
                            employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(empinfo.ReportingManager));
                            NextStageProcess[i].showStatus = "Pending for " + employeeDetails.EmployeeName + " to take action";
                        }
                    }

                    if (NextStageProcess.Count() == 0 && ToStageID != 4)
                    {
                        employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(empinfo.ReportingManager));
                        ShowStatus status = new ShowStatus();
                        status.ShowstatusCurrentStage = "Reporting Manager Stage";
                        status.showStatus = "Pending for " + employeeDetails.EmployeeName + " to take action";
                        NextStageProcess.Add(status);
                    }
                    //}
                    //}
                }
                //Result = InitiateProcess.Union(ConfirmationResult).Union(NextStageProcess).ToList();
                Result = InitiateProcess.Union(NextStageProcess).ToList();
                return Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<GuideLines> getGuileLines()
        {
            List<GuideLines> guidlines = new List<GuideLines>();
            guidlines = (from RatingMaster in dbContext.tbl_PA_Rating_Master
                         select new GuideLines
                         {
                             Percentage = RatingMaster.Percentage,
                             Rating = RatingMaster.Rating,
                             Description = RatingMaster.Description
                         }).ToList();

            return guidlines;
        }

        #endregion ConfirmationProcessChanges
    }
}