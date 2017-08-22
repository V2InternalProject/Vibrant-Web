using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace HRMS.DAL
{
    public class ExitProcessDAL
    {
        private PMSDbEntities dbPmsContext = new PMSDbEntities();
        private HRMSDBEntities dbContext = new HRMSDBEntities();
        private PMS3_HRMSDBEntities dbpms3Context = new PMS3_HRMSDBEntities();
        private WSEMDBEntities dbSEMContext = new WSEMDBEntities();
        private EmployeeDAL employeeDAL = new EmployeeDAL();
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();
        private V2toolsDBEntities dbv2toolsContext = new V2toolsDBEntities();

        public int GetApproverStageIdFromEmpId(int? employeeId)
        {
            try
            {
                var approverDetailsStageId = (from ex in dbContext.tbl_HR_ExitProcess_StageApprovers
                                              where ex.ExitInstanceID == 0 && ex.ApproverID == employeeId
                                              && (ex.stageID == 3 || ex.stageID == 8) //change to solve rmg approval stage issuse
                                              select ex.stageID).SingleOrDefault();

                if (approverDetailsStageId != null)
                    return approverDetailsStageId.Value;
                else
                    return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string[] GetToFieldEmailsRMG_HRAdmin(int exitInstanceId)
        {
            string ApproverEmail = "";
            string ApproverName = "";

            string[] ApproverEmailFinal = new string[2];

            try
            {
                var approverlist = (from e in dbContext.tbl_HR_ExitInstance
                                    join ex in dbContext.tbl_HR_ExitProcess_StageApprovers on e.stageID equals ex.stageID
                                    join h in dbContext.HRMS_tbl_PM_Employee on ex.ApproverID equals h.EmployeeID
                                    where ex.ExitInstanceID == 0 && e.ExitInstanceID == exitInstanceId && h.Status == false
                                    select ex).ToList();

                if (approverlist.Count > 0)
                {
                    foreach (var obj in approverlist)
                    {
                        HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(obj.ApproverID.Value);
                        ApproverEmail = ApproverEmail + EmpDetails.EmailID + ";";
                        ApproverName = ApproverName + EmpDetails.EmployeeName + ",";
                    }

                    char[] symbols = new char[] { ';' };
                    char[] symbols1 = new char[] { ',' };
                    ApproverEmailFinal[0] = ApproverEmail.TrimEnd(symbols);
                    ApproverEmailFinal[1] = ApproverName.TrimEnd(symbols1);
                }
                return ApproverEmailFinal;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SeparationReason> GetSeparationReasonList()
        {
            List<SeparationReason> objReason = new List<SeparationReason>();

            try
            {
                objReason = (from r in dbContext.tbl_HR_Reasons
                             where r.TagID == 2678
                             orderby r.Reason
                             select new SeparationReason
                             {
                                 Reason = r.Reason,
                                 ReasonId = r.ReasonID
                             }
                                  ).ToList();

                return objReason;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<WaiveOff> GetWaiveOffList()
        {
            List<WaiveOff> objWaive = new List<WaiveOff>();

            try
            {
                objWaive = (from r in dbContext.tbl_HR_ExitWaiveOff
                            orderby r.WaiveOffDescription
                            select new WaiveOff
                            {
                                WaiveId = r.WaiveOffId,
                                Description = r.WaiveOffDescription
                            }
                                   ).ToList();

                return objWaive;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_HR_ExitInstance GetExitfromEmpIdForResignLinkHiding(int employeeId)
        {
            try
            {
                tbl_HR_ExitInstance exit = (from e in dbContext.tbl_HR_ExitInstance
                                            where e.EmployeeID == employeeId && e.IsWithdrawn != true
                                            orderby e.CreatedDate descending
                                            select e).FirstOrDefault();

                return exit;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<tbl_HR_ExitInstance> GetExitDetailsfromEmpId(int employeeId)
        {
            try
            {
                List<tbl_HR_ExitInstance> exit = (from e in dbContext.tbl_HR_ExitInstance
                                                  where e.EmployeeID == employeeId
                                                  select e).ToList();

                return exit;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ExitProcessViewModel GetEmpSeparationDetails(int exitInstanceId)
        {
            ExitProcessViewModel model = new ExitProcessViewModel();

            try
            {
                model = (from e in dbContext.tbl_HR_ExitInstance
                         join s in dbContext.tbl_HR_ExitStage on e.stageID equals s.ExitStageID into stage
                         from EStage in stage.DefaultIfEmpty()
                         join se in dbContext.Tbl_HR_ExitModeOfSeparation on e.ResignedType equals se.Id into mode
                         from EMode in mode.DefaultIfEmpty()
                         join r in dbContext.tbl_HR_Reasons on e.ReasonID equals r.ReasonID into reason
                         from EReason in reason.DefaultIfEmpty()
                         join w in dbContext.tbl_HR_ExitWaiveOff on e.waiveof equals w.WaiveOffId into waive
                         from EWaive in waive.DefaultIfEmpty()
                         where e.ExitInstanceID == exitInstanceId
                         select new ExitProcessViewModel
                         {
                             InitiatorComment = e.EmployeeComment,
                             ManagerComment = e.ManagerComment,
                             HRComment = e.HRComment,
                             RMGComment = e.RMGComment,
                             ModeOfSeparation = EMode.ModeOfSeparation,
                             ResignedDate = e.ResignedDate,
                             TentativeReleaseDate = e.TentativeReleavingDate,
                             AgreedReleaseDate = e.AgreedReleaseDate,
                             ReasonForSeparartion = EReason.Reason,
                             SeparationId = e.ExitInstanceID,
                             StageId = e.stageID,
                             stageName = EStage.ExitStage,
                             SystemReleavingDate = e.SystemReleavingDate,
                             WaiveOff = EWaive.WaiveOffDescription,
                             NoticePeriod = e.NoticePeriod
                         }).SingleOrDefault();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EmployeeDetailsViewModel GetEmpSeparationShowDetails(int exitInstanceId)
        {
            EmployeeDetailsViewModel model = new EmployeeDetailsViewModel();
            try
            {
                int? EmpId = 0;
                ExitProcessViewModel exit = GetSeparationDetails(exitInstanceId);

                if (exit != null)
                {
                    EmpId = exit.EmployeeId;

                    model = (from e in dbContext.HRMS_tbl_PM_Employee
                             join r in dbContext.HRMS_tbl_PM_Role on e.PostID equals r.RoleID into role
                             from ERole in role.DefaultIfEmpty()
                             join l in dbContext.tbl_PM_Location on e.LocationID equals l.LocationID into location
                             from ELocation in location.DefaultIfEmpty()
                             join ol in dbContext.tbl_PM_OfficeLocation on e.OfficeLocation equals ol.OfficeLocationID into officeLocation
                             from OLocation in officeLocation.DefaultIfEmpty()
                             where e.EmployeeID == EmpId
                             select new EmployeeDetailsViewModel
                             {
                                 OrgRoleDescription = ERole.RoleDescription,
                                 Group = "Offshore Development Center",
                                 OrganizationUnit = ELocation.Location,
                                 EmployeeId = e.EmployeeID,
                                 EmployeeCode = e.EmployeeCode,
                                 EmployeeName = e.EmployeeName,
                                 JoiningDate = e.JoiningDate,
                                 OfficeLocation = OLocation.OfficeLocation
                             }).SingleOrDefault();
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_HR_ExitInstance GetEmpExitTermination(int exitInstanceId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_HR_ExitInstance EmpDetails = dbContext.tbl_HR_ExitInstance.Where(ed => ed.ExitInstanceID == exitInstanceId).FirstOrDefault();
                return EmpDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DesignationDetails GetEmpSeparationDesignationDetails(int exitInstanceId)
        {
            DesignationDetails model = new DesignationDetails();
            try
            {
                int? EmpId = 0;
                ExitProcessViewModel exit = GetSeparationDetails(exitInstanceId);
                if (exit != null)
                {
                    EmpId = exit.EmployeeId;

                    model = (
                             from d in dbContext.tbl_PM_EmployeeDesignation_Change
                             join dm in dbContext.tbl_PM_DesignationMaster on d.DesignationID equals dm.DesignationID into designation
                             from EDesigMaster in designation.DefaultIfEmpty()
                             join g in dbContext.tbl_PM_GradeMaster on d.CurrentGradeID equals g.GradeID into grade
                             from EGrade in grade.DefaultIfEmpty()
                             join e in dbContext.HRMS_tbl_PM_Employee on d.DesignationID equals e.DesignationID into data
                             from EData in data.DefaultIfEmpty()
                             orderby d.UniqueID descending
                             where EData.EmployeeID == EmpId
                             select new DesignationDetails
                             {
                                 Designation = EDesigMaster.DesignationName,
                                 Grade = EGrade.Grade
                             }).FirstOrDefault();
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tbl_HR_ExitStageEvent GetEmpTerminationDetails(int exitInstanceId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                Tbl_HR_ExitStageEvent EmpDetails = dbContext.Tbl_HR_ExitStageEvent.Where(ed => ed.ExitInstanceId == exitInstanceId).FirstOrDefault();
                return EmpDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveSeparationFormDetails(ExitProcessViewModel model, out int exitInstanceId)
        {
            bool isSuccess = false;
            int? statusMasterId = 0, statusId = 0;

            try
            {
                string loginName = System.Web.HttpContext.Current.User.Identity.Name;
                HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginName);

                if (model.Isterminate == "yes")
                {
                    tbl_HR_ExitInstance tblEmployee = new tbl_HR_ExitInstance()
                    {
                        EmployeeID = model.TerminatedEmpId,
                        HRComment = model.EmpComment,
                        ResignedDate = model.ResignedDate,
                        //ResignedDate = DateTime.Now,
                        ReasonID = int.Parse(model.ReasonForSeparartion),
                        TentativeReleavingDate = model.TentativeReleaseDate,
                        AgreedReleaseDate = model.AgreedReleaseDate,
                        CreatedDate = DateTime.Now,
                        stageID = 7,
                        ResignedType = 1,
                        NoticePeriod = model.NoticePeriod
                    };
                    dbContext.tbl_HR_ExitInstance.AddObject(tblEmployee);
                    dbContext.SaveChanges();
                    exitInstanceId = tblEmployee.ExitInstanceID;

                    Tbl_HR_ExitStageEvent ExitEvent = new Tbl_HR_ExitStageEvent();
                    tbl_HR_ExitInstance resignDetails = (from r in dbContext.tbl_HR_ExitInstance
                                                         where r.ExitInstanceID == tblEmployee.ExitInstanceID
                                                         select r).SingleOrDefault();

                    if (resignDetails != null)
                    {
                        ExitEvent.ExitInstanceId = resignDetails.ExitInstanceID;
                        ExitEvent.EventDateTime = DateTime.Now;
                        ExitEvent.Action = "Submit";
                        ExitEvent.FromStageId = 1;
                        ExitEvent.ToStageId = resignDetails.stageID;
                        ExitEvent.StageActorEmployeeId = loginuser.EmployeeID;
                        ExitEvent.Comments = resignDetails.EmployeeComment;
                        dbContext.Tbl_HR_ExitStageEvent.AddObject(ExitEvent);
                        dbContext.SaveChanges();
                    }

                    HRMS_tbl_PM_Employee empdetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == model.TerminatedEmpId).FirstOrDefault();
                    if (empdetails != null)
                    {
                        empdetails.EmployeeStatusMasterID = 2;
                        empdetails.EmployeeStatusID = 13;
                        empdetails.LeavingDate = model.TentativeReleaseDate;
                        empdetails.Status = true;
                        dbContext.SaveChanges();
                        bool MemberInactive = true;
                        aspnet_Membership _Membership = (from m in dbv2toolsContext.aspnet_Users
                                                         join roleID in dbv2toolsContext.aspnet_Membership on m.UserId equals roleID.UserId
                                                         where m.UserName == empdetails.EmployeeCode
                                                         select roleID).FirstOrDefault();
                        _Membership.IsLockedOut = MemberInactive;
                        dbv2toolsContext.SaveChanges();
                        isSuccess = true;
                    };
                }
                else
                {
                    HRMS_tbl_PM_Employee empdetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == model.EmployeeId).FirstOrDefault();
                    if (empdetails != null)
                    {
                        statusMasterId = empdetails.EmployeeStatusMasterID;
                        statusId = empdetails.EmployeeStatusID;
                    };

                    tbl_HR_ExitInstance tblEmployee = new tbl_HR_ExitInstance()
                    {
                        EmployeeID = model.EmployeeId,
                        EmployeeComment = model.EmpComment,
                        ResignedDate = model.ResignedDate,
                        //ResignedDate = DateTime.Now,
                        ReasonID = int.Parse(model.ReasonForSeparartion),
                        TentativeReleavingDate = model.TentativeReleaseDate,
                        AgreedReleaseDate = model.AgreedReleaseDate,
                        CreatedDate = DateTime.Now,
                        stageID = 2,
                        ResignedType = 1,
                        waiveof = 3,
                        IsWithdrawn = false,
                        NoticePeriod = model.NoticePeriod,
                        PrevoiusEmpStatusMatserId = statusMasterId,
                        PrevoiusEmpStatusId = statusId,
                        SystemReleavingDate = model.SystemReleavingDate
                    };
                    dbContext.tbl_HR_ExitInstance.AddObject(tblEmployee);
                    dbContext.SaveChanges();
                    exitInstanceId = tblEmployee.ExitInstanceID;

                    Tbl_HR_ExitStageEvent ExitEvent = new Tbl_HR_ExitStageEvent();
                    tbl_HR_ExitInstance resignDetails = (from r in dbContext.tbl_HR_ExitInstance
                                                         where r.ExitInstanceID == tblEmployee.ExitInstanceID
                                                         select r).SingleOrDefault();

                    if (resignDetails != null)
                    {
                        ExitEvent.ExitInstanceId = resignDetails.ExitInstanceID;
                        ExitEvent.EventDateTime = DateTime.Now;
                        ExitEvent.Action = "Submit";
                        ExitEvent.FromStageId = 1;
                        ExitEvent.ToStageId = resignDetails.stageID;
                        ExitEvent.StageActorEmployeeId = resignDetails.EmployeeID;
                        ExitEvent.Comments = resignDetails.EmployeeComment;
                        dbContext.Tbl_HR_ExitStageEvent.AddObject(ExitEvent);
                        dbContext.SaveChanges();
                    }

                    if (empdetails != null)
                    {
                        empdetails.EmployeeStatusMasterID = 1;
                        empdetails.EmployeeStatusID = 18;
                        dbContext.SaveChanges();
                        isSuccess = true;
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        public ExitProcessViewModel GetSeparationDetails(int exitInstanceId)
        {
            try
            {
                ExitProcessViewModel Resign = (from r in dbContext.tbl_HR_ExitInstance
                                               join o in dbContext.tbl_HR_Reasons on r.ReasonID equals o.ReasonID
                                               where r.ExitInstanceID == exitInstanceId
                                               select new ExitProcessViewModel
                                               {
                                                   TentativeReleaseDate = r.TentativeReleavingDate,
                                                   AgreedReleaseDate = r.AgreedReleaseDate,
                                                   EmpComment = r.EmployeeComment,
                                                   IsWithdraw = r.IsWithdrawn,
                                                   EmployeeId = r.EmployeeID,
                                                   StageId = r.stageID,
                                                   ResignedDate = r.ResignedDate,
                                                   ReasonForSeparartion = o.Reason,
                                                   NoticePeriod = r.NoticePeriod,
                                                   SystemReleavingDate = r.SystemReleavingDate,
                                                   HRComment = r.HRComment
                                               }
                                               ).SingleOrDefault();

                return Resign;
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
                            List<FieldChildDetails> child = (from l in dbContext.tbl_HR_ExitStage
                                                             select new FieldChildDetails
                                                             {
                                                                 Id = l.ExitStageID,
                                                                 Description = l.ExitStage
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

        public List<EmpSeparationApprovals> GetWatchListDetails(string searchText, string field, string fieldChild, int page, int rows, int employeeId, out int totalCount)
        {
            List<EmpSeparationApprovals> mainResult = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> employeeresult = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> EmpManagerCheck = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> HRAdminCheck = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> HRAdminHRApprovalStageCheck = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> StakeHoldersCheck = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> RMGCheck = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> RMGStageCLearanceCheck = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> DepartmentList = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> ProjectDepartmentList = new List<EmpSeparationApprovals>();
            CommonMethodsDAL dal = new CommonMethodsDAL();

            try
            {
                int FieldChild = 0;
                if (fieldChild != "")
                {
                    FieldChild = Convert.ToInt32(fieldChild);
                }
                string LogeedInEmCode = string.Empty;
                string[] LogeedInEmRoles = { };

                EmployeeDAL empdal = new EmployeeDAL();
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);

                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(employeeId);
                if (employeeDetails != null && employeeDetails.EmployeeID > 0)
                {
                    LogeedInEmCode = employeeDetails.EmployeeCode;
                    LogeedInEmRoles = Roles.GetRolesForUser(LogeedInEmCode);
                }

                var stageapprovers = (from ex in dbContext.tbl_HR_ExitProcess_StageApprovers
                                      where ex.ExitInstanceID == 0
                                      select ex).ToList();

                List<int?> stageapproversArray = new List<int?>();

                if (stageapprovers.Count > 0)
                    foreach (var item in stageapprovers)
                        stageapproversArray.Add(item.ApproverID);

                //query for stakeholders
                tbl_HR_ExitProcess_StageApprovers ApproverDetails = (from ap in dbContext.tbl_HR_ExitProcess_StageApprovers
                                                                     where ap.ApproverID == employeeId && ap.ExitInstanceID == 0 && ap.stageID == 4
                                                                     select ap).FirstOrDefault();

                // this logic is for employee himself logins what falls under his watchlist bucket.ie. his own record.

                #region Employee WatchList Section

                employeeresult = (from E in dbContext.tbl_HR_ExitInstance
                                  join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                  join s in dbContext.tbl_HR_ExitStage on E.stageID equals s.ExitStageID
                                  where E.EmployeeID == employeeId && (E.IsWithdrawn == true || E.stageID != 1) &&
                                  (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                   && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.stageID == FieldChild : FieldChild == 0))) //field search
                                  join ese in dbContext.Tbl_HR_ExitStageEvent on E.ExitInstanceID equals ese.ExitInstanceId into eventStageRecord  // Fix to add red Image support

                                  select new EmpSeparationApprovals
                                  {
                                      Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                      ExitStageOrder = s.ExitStageOrder,
                                      ReportingTo = emp.ReportingTo,
                                      ExitInstanceId = E.ExitInstanceID,
                                      StageId = E.stageID,
                                      ResignedDate = E.ResignedDate,
                                      stageName = s.ExitStage,
                                      EmployeeId = E.EmployeeID,
                                      WatchListEmployeeName = emp.EmployeeName,
                                      IsWithdrawn = E.IsWithdrawn
                                  }).Distinct().OrderByDescending(exid => exid.ResignedDate).ToList();

                #endregion Employee WatchList Section

                // following logic for checking manager login & any entries fall under managers watchlist bucket

                #region For Manager WatchList Section

                if (stageapproversArray.Contains(employeeId) == false && ApproverDetails == null)
                {
                    EmpManagerCheck = (from e in dbContext.HRMS_tbl_PM_Employee
                                       join ex in dbContext.tbl_HR_ExitInstance on e.EmployeeID equals ex.EmployeeID
                                       join s in dbContext.tbl_HR_ExitStage on ex.stageID equals s.ExitStageID
                                       where e.ReportingTo == employeeId && e.EmployeeID == ex.EmployeeID && ex.stageID != 2 &&
                                       (e.EmployeeName.Contains(searchText) || e.EmployeeCode.Contains(searchText))
                                        && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? e.BusinessGroupID == FieldChild : field == "Organization Unit" ? e.LocationID == FieldChild : field == "Stage Name" ? ex.stageID == FieldChild : FieldChild == 0))) //field search
                                       join ese in dbContext.Tbl_HR_ExitStageEvent on ex.ExitInstanceID equals ese.ExitInstanceId into eventStageRecord  // Fix to add red Image support

                                       select new EmpSeparationApprovals
                                       {
                                           Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                           ExitStageOrder = s.ExitStageOrder,
                                           ReportingTo = e.ReportingTo,
                                           ExitInstanceId = ex.ExitInstanceID,
                                           StageId = ex.stageID,
                                           ResignedDate = ex.ResignedDate,
                                           stageName = s.ExitStage,
                                           EmployeeId = ex.EmployeeID,
                                           WatchListEmployeeName = e.EmployeeName,
                                           IsWithdrawn = ex.IsWithdrawn
                                       }).Distinct().OrderByDescending(exid => exid.ResignedDate).ToList();

                    foreach (var item in EmpManagerCheck)
                    {
                        if (item.StageId == 4)
                        {
                            Tbl_HR_ExitStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ExitStageEvent
                                                                 where empInfo.ExitInstanceId == item.ExitInstanceId
                                                                 && (empInfo.FromStageId == 3 && empInfo.ToStageId == 4)
                                                                 orderby empInfo.EventDateTime descending
                                                                 select empInfo).FirstOrDefault();
                            Tbl_HR_ExitStageEvent LatestEntryProjectDepartments = new Tbl_HR_ExitStageEvent();
                            if (LatestEntry != null)
                            {
                                LatestEntryProjectDepartments = (from empInfo in dbContext.Tbl_HR_ExitStageEvent
                                                                 where empInfo.ExitInstanceId == item.ExitInstanceId
                                                                       && (empInfo.FromStageId == 4 && empInfo.ToStageId == 4)
                                                                       && empInfo.EventDateTime > LatestEntry.EventDateTime
                                                                       && empInfo.StageActorEmployeeId == employeeID
                                                                 orderby empInfo.EventDateTime descending
                                                                 select empInfo).Take(1).FirstOrDefault();
                            }
                            if (LatestEntryProjectDepartments != null)
                                ProjectDepartmentList.Add(item);
                        }
                        else
                            ProjectDepartmentList.Add(item);
                    }
                }

                #endregion For Manager WatchList Section

                // following logic is to check what falls under HR Admins watchlist ie.
                // HR Admin will handle HR Approval,Exit Interview, Hr Closure, Exit stages.

                #region HRAdmin WatchList Section

                if (LogeedInEmRoles.Length > 0)
                {
                    if (LogeedInEmRoles.Contains("HR Admin"))
                    {
                        List<int?> approversSatageID = new List<int?>();

                        var approverDetails = (from ex in dbContext.tbl_HR_ExitProcess_StageApprovers
                                               where ex.ExitInstanceID == 0 && ex.ApproverID == employeeId
                                               select ex).ToList();

                        if (approverDetails.Count > 0)
                        {
                            foreach (var stage in approverDetails)
                            {
                                approversSatageID.Add(stage.stageID);
                            }
                        }
                        HRAdminCheck = (from E in dbContext.tbl_HR_ExitInstance
                                        join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                        join s in dbContext.tbl_HR_ExitStage on E.stageID equals s.ExitStageID
                                        where (E.stageID == 1 || E.stageID == 2 || E.stageID == 8 || (E.stageID == 7 && stageapproversArray.Contains(employeeId) == false) ||
                                        (E.stageID == 3 && approversSatageID.Contains(3) == false) || (E.stageID == 4 && approversSatageID.Contains(4) == false))
                                         && emp.ReportingTo != employeeId &&
                                        (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.stageID == FieldChild : FieldChild == 0))) //field search
                                        join ese in dbContext.Tbl_HR_ExitStageEvent on E.ExitInstanceID equals ese.ExitInstanceId into eventStageRecord  // Fix to add red Image support

                                        select new EmpSeparationApprovals
                                        {
                                            Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                            ExitStageOrder = s.ExitStageOrder,
                                            ReportingTo = emp.ReportingTo,
                                            ExitInstanceId = E.ExitInstanceID,
                                            StageId = E.stageID,
                                            ResignedDate = E.ResignedDate,
                                            stageName = s.ExitStage,
                                            EmployeeId = E.EmployeeID,
                                            WatchListEmployeeName = emp.EmployeeName,
                                            IsWithdrawn = E.IsWithdrawn
                                        }).Distinct().OrderByDescending(exid => exid.ResignedDate).ToList();
                    }
                }

                #endregion HRAdmin WatchList Section

                #region HRAdmin HR Approval WatchList Section

                if (LogeedInEmRoles.Length > 0)
                {
                    if (stageapproversArray.Contains(employeeId))
                    {
                        List<int?> approversSatageID = new List<int?>();

                        var approverDetails = (from ex in dbContext.tbl_HR_ExitProcess_StageApprovers
                                               where ex.ExitInstanceID == 0 && ex.ApproverID == employeeId
                                               select ex).ToList();

                        if (approverDetails.Count > 0)
                        {
                            foreach (var stage in approverDetails)
                            {
                                approversSatageID.Add(stage.stageID);
                            }
                        }
                        HRAdminHRApprovalStageCheck = (from E in dbContext.tbl_HR_ExitInstance
                                                       join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                                       join s in dbContext.tbl_HR_ExitStage on E.stageID equals s.ExitStageID
                                                       join exitevents in dbContext.Tbl_HR_ExitStageEvent on E.ExitInstanceID equals exitevents.ExitInstanceId
                                                       where (((E.stageID == 4 && approversSatageID.Contains(4) == false && (LogeedInEmRoles.Contains("HR Admin") == false)) && emp.ReportingTo != employeeId && approversSatageID.Contains(3) == true &&
                                                       (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))) ||

                                                        (E.stageID > 3 && E.stageID != 8 && E.stageID != 4 && ((E.stageID != 5 || E.stageID != 6) && (LogeedInEmRoles.Contains("HR Admin") == true)) && emp.ReportingTo != employeeId && approversSatageID.Contains(3) == true &&
                                                       (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))))
                                                        && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.stageID == FieldChild : FieldChild == 0))) //field search

                                                       join ese in dbContext.Tbl_HR_ExitStageEvent on E.ExitInstanceID equals ese.ExitInstanceId into eventStageRecord  // Fix to add red Image support

                                                       select new EmpSeparationApprovals
                                                       {
                                                           Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                                           ExitStageOrder = s.ExitStageOrder,
                                                           ReportingTo = emp.ReportingTo,
                                                           ExitInstanceId = E.ExitInstanceID,
                                                           StageId = E.stageID,
                                                           ResignedDate = E.ResignedDate,
                                                           stageName = s.ExitStage,
                                                           EmployeeId = E.EmployeeID,
                                                           WatchListEmployeeName = emp.EmployeeName,
                                                           IsWithdrawn = E.IsWithdrawn
                                                       }).Distinct().OrderByDescending(exid => exid.ResignedDate).ToList();
                    }
                }

                #endregion HRAdmin HR Approval WatchList Section

                #region RMG WatchList Section

                if (LogeedInEmRoles.Length > 0)
                {
                    if (LogeedInEmRoles.Contains("RMG"))
                    {
                        List<int?> approversSatageID = new List<int?>();

                        var approverDetails = (from ex in dbContext.tbl_HR_ExitProcess_StageApprovers
                                               where ex.ExitInstanceID == 0 && ex.ApproverID == employeeId
                                               select ex).ToList();

                        if (approverDetails.Count > 0)
                        {
                            foreach (var stage in approverDetails)
                            {
                                approversSatageID.Add(stage.stageID);
                            }
                        }

                        RMGCheck = (from E in dbContext.tbl_HR_ExitInstance
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                    join s in dbContext.tbl_HR_ExitStage on E.stageID equals s.ExitStageID
                                    where (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                     && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.stageID == FieldChild : FieldChild == 0))) //field search
                                    && (((E.stageID == 3 || E.stageID == 4 || E.stageID == 5 || E.stageID == 6 || E.stageID == 7 || (E.stageID == 8 && approversSatageID.Contains(8) == false)) && stageapproversArray.Contains(employeeId) == false) || E.stageID == 1 || E.stageID == 2)

                                    && emp.ReportingTo != employeeId
                                    join ese in dbContext.Tbl_HR_ExitStageEvent on E.ExitInstanceID equals ese.ExitInstanceId into eventStageRecord  // Fix to add red Image support

                                    select new EmpSeparationApprovals
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                        ExitStageOrder = s.ExitStageOrder,
                                        ReportingTo = emp.ReportingTo,
                                        ExitInstanceId = E.ExitInstanceID,
                                        StageId = E.stageID,
                                        ResignedDate = E.ResignedDate,
                                        stageName = s.ExitStage,
                                        EmployeeId = E.EmployeeID,
                                        WatchListEmployeeName = emp.EmployeeName,
                                        IsWithdrawn = E.IsWithdrawn
                                    }).Distinct().OrderByDescending(exid => exid.ResignedDate).ToList();
                    }
                }

                #endregion RMG WatchList Section

                #region RMG StageCLearance WatchList Section

                if (LogeedInEmRoles.Length > 0)
                {
                    if (stageapproversArray.Contains(employeeId))
                    {
                        int? approversSatageID = 0;

                        var approverDetails = (from ex in dbContext.tbl_HR_ExitProcess_StageApprovers
                                               where ex.ExitInstanceID == 0 && ex.ApproverID == employeeId && ex.stageID == 8
                                               select ex).SingleOrDefault();

                        if (approverDetails != null)
                            approversSatageID = approverDetails.stageID;

                        RMGStageCLearanceCheck = (from E in dbContext.tbl_HR_ExitInstance
                                                  join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                                  join s in dbContext.tbl_HR_ExitStage on E.stageID equals s.ExitStageID
                                                  where (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                                   && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.stageID == FieldChild : FieldChild == 0))) //field search
                                                  && E.stageID != 8 && E.stageID > 2 && emp.ReportingTo != employeeId && approversSatageID == 8

                                                  join ese in dbContext.Tbl_HR_ExitStageEvent on E.ExitInstanceID equals ese.ExitInstanceId into eventStageRecord  // Fix to add red Image support

                                                  select new EmpSeparationApprovals
                                                  {
                                                      Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                                      ExitStageOrder = s.ExitStageOrder,
                                                      ReportingTo = emp.ReportingTo,
                                                      ExitInstanceId = E.ExitInstanceID,
                                                      StageId = E.stageID,
                                                      ResignedDate = E.ResignedDate,
                                                      stageName = s.ExitStage,
                                                      EmployeeId = E.EmployeeID,
                                                      WatchListEmployeeName = emp.EmployeeName,
                                                      IsWithdrawn = E.IsWithdrawn
                                                  }).Distinct().OrderByDescending(exid => exid.ResignedDate).ToList();
                    }
                }

                #endregion RMG StageCLearance WatchList Section

                #region StakeHoldders WatchList Section

                if (ApproverDetails != null)
                {
                    // Watch list records
                    StakeHoldersCheck = (from E in dbContext.tbl_HR_ExitInstance
                                         join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                         join s in dbContext.tbl_HR_ExitStage on E.stageID equals s.ExitStageID
                                         where E.stageID >= 4
                                                    && (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                                    && emp.ReportingTo != employeeId
                                                     && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.stageID == FieldChild : FieldChild == 0))) //field search
                                         join ese in dbContext.Tbl_HR_ExitStageEvent on E.ExitInstanceID equals ese.ExitInstanceId into eventStageRecord  // Fix to add red Image support

                                         select new EmpSeparationApprovals
                                         {
                                             Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                             ExitStageOrder = s.ExitStageOrder,
                                             ReportingTo = emp.ReportingTo,
                                             ExitInstanceId = E.ExitInstanceID,
                                             StageId = E.stageID,
                                             ResignedDate = E.ResignedDate,
                                             stageName = s.ExitStage,
                                             EmployeeId = E.EmployeeID,
                                             WatchListEmployeeName = emp.EmployeeName,
                                             IsWithdrawn = E.IsWithdrawn
                                         }).Distinct().OrderByDescending(exid => exid.ResignedDate).ToList();

                    foreach (var item in StakeHoldersCheck)
                    {
                        if (item.StageId == 4)
                        {
                            Tbl_HR_ExitStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ExitStageEvent
                                                                 where empInfo.ExitInstanceId == item.ExitInstanceId
                                                                 && (empInfo.FromStageId == 3 && empInfo.ToStageId == 4)
                                                                 orderby empInfo.EventDateTime descending
                                                                 select empInfo).FirstOrDefault();
                            List<Tbl_HR_ExitStageEvent> LatestEntryDepartments = new List<Tbl_HR_ExitStageEvent>();
                            if (LatestEntry != null)
                            {
                                LatestEntryDepartments = (from empInfo in dbContext.Tbl_HR_ExitStageEvent
                                                          where empInfo.ExitInstanceId == item.ExitInstanceId
                                                                && (empInfo.FromStageId == 4 && empInfo.ToStageId == 4)
                                                                && empInfo.EventDateTime > LatestEntry.EventDateTime
                                                                && empInfo.StageActorEmployeeId == employeeID
                                                          orderby empInfo.EventDateTime descending
                                                          select empInfo).ToList();
                            }

                            if (item.ReportingTo == employeeID && LatestEntryDepartments.Count(r => r.StageActorEmployeeId == employeeID) == 2)
                            {
                                DepartmentList.Add(item);
                            }
                            else if (item.ReportingTo == employeeID && LatestEntryDepartments.Count(r => r.StageActorEmployeeId == employeeID) == 1)
                            {
                                continue;
                            }
                            else if (LatestEntryDepartments.Count != 0)
                            {
                                DepartmentList.Add(item);
                            }
                        }
                        else
                        {
                            if (!LogeedInEmRoles.Contains("HR Admin"))
                                DepartmentList.Add(item);
                        }
                    }
                }

                #endregion StakeHoldders WatchList Section

                mainResult = employeeresult.Union(ProjectDepartmentList).Union(HRAdminCheck).Union(HRAdminHRApprovalStageCheck).Union(DepartmentList).Union(RMGCheck).Union(RMGStageCLearanceCheck).ToList();

                mainResult.GroupBy(x => x.ExitInstanceId).Select(group => group.First());

                var grouped = mainResult.GroupBy(item => item.ExitInstanceId);
                var finalList = grouped.Select(grp => grp.OrderBy(item => item.ExitInstanceId).First());

                mainResult = finalList.ToList();
                totalCount = mainResult.Count;
                //return mainResult.Skip((page - 1) * rows).Take(rows).ToList();
                return mainResult.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmpSeparationApprovals> GetInboxListDetails(string searchText, string field, string fieldChild, int page, int rows, int employeeId, out int totalCount)
        {
            List<EmpSeparationApprovals> mainResult = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> employeeresult = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> EmpManagerCheck = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> HRAdminCheck = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> HRAdminHRApprovalStageCheck = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> StakeHoldersCheck = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> RMGCheck = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> DepartmentList = new List<EmpSeparationApprovals>();
            List<EmpSeparationApprovals> ProjectDepartmentList = new List<EmpSeparationApprovals>();

            try
            {
                int FieldChild = 0;
                if (fieldChild != "")
                {
                    FieldChild = Convert.ToInt32(fieldChild);
                }
                string LogeedInEmCode = string.Empty;
                string[] LogeedInEmRoles = { };
                EmployeeDAL empdal = new EmployeeDAL();
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);

                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(employeeId);
                if (employeeDetails != null && employeeDetails.EmployeeID > 0)
                {
                    LogeedInEmCode = employeeDetails.EmployeeCode;
                    LogeedInEmRoles = Roles.GetRolesForUser(LogeedInEmCode);
                }

                var stageapprovers = (from ex in dbContext.tbl_HR_ExitProcess_StageApprovers
                                      where ex.ExitInstanceID == 0
                                      select ex).ToList();

                List<int?> stageapproversArray = new List<int?>();
                if (stageapprovers.Count > 0)
                    foreach (var item in stageapprovers)
                        stageapproversArray.Add(item.ApproverID);

                //this logic is for employee himself logins what falls under his watchlist bucket.ie. his own record.

                #region Employee Inbox Section

                employeeresult = (from E in dbContext.tbl_HR_ExitInstance
                                  join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                  join s in dbContext.tbl_HR_ExitStage on E.stageID equals s.ExitStageID
                                  where E.EmployeeID == employeeId && E.stageID == 1
                                       && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.stageID == FieldChild : FieldChild == 0))) //field search
                                       && E.IsWithdrawn == false
                                  join ese in dbContext.Tbl_HR_ExitStageEvent on E.ExitInstanceID equals ese.ExitInstanceId into eventStageRecord  // Fix to add red Image support

                                  select new EmpSeparationApprovals
                                  {
                                      Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                      ExitStageOrder = s.ExitStageOrder,
                                      ReportingTo = emp.ReportingTo,
                                      ExitInstanceId = E.ExitInstanceID,
                                      StageId = E.stageID,
                                      ResignedDate = E.ResignedDate,
                                      stageName = s.ExitStage,
                                      EmployeeId = E.EmployeeID,
                                      WatchListEmployeeName = emp.EmployeeName,
                                      IsWithdrawn = E.IsWithdrawn
                                  }).Distinct().OrderByDescending(exid => exid.ResignedDate).ToList();

                #endregion Employee Inbox Section

                // following logic for checking manager login & any entries fall under managers watchlist bucket

                #region For Manager Inbox Section

                EmpManagerCheck = (from e in dbContext.HRMS_tbl_PM_Employee
                                   join ex in dbContext.tbl_HR_ExitInstance on e.EmployeeID equals ex.EmployeeID
                                   join s in dbContext.tbl_HR_ExitStage on ex.stageID equals s.ExitStageID
                                   where e.ReportingTo == employeeId && e.EmployeeID == ex.EmployeeID && (ex.stageID == 2 || ex.stageID == 4)
                                   && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? e.BusinessGroupID == FieldChild : field == "Organization Unit" ? e.LocationID == FieldChild : field == "Stage Name" ? ex.stageID == FieldChild : FieldChild == 0))) //field search
                                   && (e.EmployeeName.Contains(searchText) || e.EmployeeCode.Contains(searchText))
                                   join ese in dbContext.Tbl_HR_ExitStageEvent on ex.ExitInstanceID equals ese.ExitInstanceId into eventStageRecord  // Fix to add red Image support

                                   select new EmpSeparationApprovals
                                   {
                                       Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                       ExitStageOrder = s.ExitStageOrder,
                                       ReportingTo = e.ReportingTo,
                                       ExitInstanceId = ex.ExitInstanceID,
                                       StageId = ex.stageID,
                                       ResignedDate = ex.ResignedDate,
                                       stageName = s.ExitStage,
                                       EmployeeId = ex.EmployeeID,
                                       WatchListEmployeeName = e.EmployeeName,
                                       IsWithdrawn = ex.IsWithdrawn
                                   }).Distinct().OrderByDescending(exid => exid.ResignedDate).ToList();

                foreach (var item in EmpManagerCheck)
                {
                    int i = 0;
                    Tbl_HR_ExitStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ExitStageEvent
                                                         where empInfo.ExitInstanceId == item.ExitInstanceId
                                                         && (empInfo.FromStageId == 3 && empInfo.ToStageId == 4)
                                                         orderby empInfo.EventDateTime descending
                                                         select empInfo).FirstOrDefault();
                    List<Tbl_HR_ExitStageEvent> LatestEntryProjectDepartments = new List<Tbl_HR_ExitStageEvent>();
                    if (LatestEntry != null)
                    {
                        LatestEntryProjectDepartments = (from empInfo in dbContext.Tbl_HR_ExitStageEvent
                                                         where empInfo.ExitInstanceId == item.ExitInstanceId
                                                         && (empInfo.FromStageId == 4 && empInfo.ToStageId == 4)
                                                          && empInfo.StageActorEmployeeId == employeeID
                                                         && empInfo.EventDateTime > LatestEntry.EventDateTime
                                                         orderby empInfo.EventDateTime descending
                                                         select empInfo).ToList();
                    }

                    if (LatestEntryProjectDepartments.Count != 0)
                    {
                        foreach (var items in LatestEntryProjectDepartments)
                        {
                            if (items.StageActorEmployeeId != employeeID)
                            {
                                ProjectDepartmentList.Add(item);
                            }
                            else
                                continue;
                        }
                    }
                    else
                    {
                        ProjectDepartmentList.Add(item);
                    }
                    i++;
                }

                #endregion For Manager Inbox Section

                // following logic is to check what falls under HR Admins watchlist ie.
                // HR Admin will handle HR Approval,Exit Interview, Hr Closure, Exit stages.

                #region HRAdmin Inbox Section

                if (LogeedInEmRoles.Length > 0)
                {
                    if (LogeedInEmRoles.Contains("HR Admin"))
                    {
                        HRAdminCheck = (from E in dbContext.tbl_HR_ExitInstance
                                        join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                        join s in dbContext.tbl_HR_ExitStage on E.stageID equals s.ExitStageID
                                        where (E.stageID == 5 || E.stageID == 6) &&
                                        (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.stageID == FieldChild : FieldChild == 0))) //field search
                                        join ese in dbContext.Tbl_HR_ExitStageEvent on E.ExitInstanceID equals ese.ExitInstanceId into eventStageRecord  // Fix to add red Image support

                                        select new EmpSeparationApprovals
                                        {
                                            Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                            ExitStageOrder = s.ExitStageOrder,
                                            ReportingTo = emp.ReportingTo,
                                            ExitInstanceId = E.ExitInstanceID,
                                            StageId = E.stageID,
                                            ResignedDate = E.ResignedDate,
                                            stageName = s.ExitStage,
                                            EmployeeId = E.EmployeeID,
                                            WatchListEmployeeName = emp.EmployeeName,
                                            IsWithdrawn = E.IsWithdrawn
                                        }).Distinct().OrderByDescending(exid => exid.ResignedDate).ToList();
                    }
                }

                #endregion HRAdmin Inbox Section

                #region HRAdmin HR Approval stage Inbox Section

                if (LogeedInEmRoles.Length > 0)
                {
                    if (stageapproversArray.Contains(employeeId))
                    {
                        var approverDetails = (from ex in dbContext.tbl_HR_ExitProcess_StageApprovers
                                               where ex.ExitInstanceID == 0 && ex.ApproverID == employeeId && ex.stageID == 3
                                               select ex).SingleOrDefault();

                        if (approverDetails != null)
                        {
                            HRAdminHRApprovalStageCheck = (from E in dbContext.tbl_HR_ExitInstance
                                                           join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                                           join s in dbContext.tbl_HR_ExitStage on E.stageID equals s.ExitStageID

                                                           where E.stageID == 3 && approverDetails.stageID == 3 &&
                                                           (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                                            && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.stageID == FieldChild : FieldChild == 0))) //field search
                                                           join ese in dbContext.Tbl_HR_ExitStageEvent on E.ExitInstanceID equals ese.ExitInstanceId into eventStageRecord  // Fix to add red Image support

                                                           orderby E.ExitInstanceID descending
                                                           select new EmpSeparationApprovals
                                                           {
                                                               Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                                               ExitStageOrder = s.ExitStageOrder,
                                                               ReportingTo = emp.ReportingTo,
                                                               ExitInstanceId = E.ExitInstanceID,
                                                               StageId = E.stageID,
                                                               ResignedDate = E.ResignedDate,
                                                               stageName = s.ExitStage,
                                                               EmployeeId = E.EmployeeID,
                                                               WatchListEmployeeName = emp.EmployeeName,
                                                               IsWithdrawn = E.IsWithdrawn
                                                           }).Distinct().OrderByDescending(exid => exid.ResignedDate).ToList();
                        }
                    }
                }

                #endregion HRAdmin HR Approval stage Inbox Section

                #region StakeHoldders Inbox Section

                tbl_HR_ExitProcess_StageApprovers ApproverDetails = (from ap in dbContext.tbl_HR_ExitProcess_StageApprovers
                                                                     where ap.ApproverID == employeeId && ap.ExitInstanceID == 0 && ap.stageID == 4
                                                                     select ap).FirstOrDefault();
                if (ApproverDetails != null)
                {
                    StakeHoldersCheck = (from E in dbContext.tbl_HR_ExitInstance
                                         join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                         join s in dbContext.tbl_HR_ExitStage on E.stageID equals s.ExitStageID
                                         where E.stageID == 4 && (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                          && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.stageID == FieldChild : FieldChild == 0))) //field search
                                         join ese in dbContext.Tbl_HR_ExitStageEvent on E.ExitInstanceID equals ese.ExitInstanceId into eventStageRecord  // Fix to add red Image support

                                         select new EmpSeparationApprovals
                                         {
                                             Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                             ExitStageOrder = s.ExitStageOrder,
                                             ReportingTo = emp.ReportingTo,
                                             ExitInstanceId = E.ExitInstanceID,
                                             StageId = E.stageID,
                                             ResignedDate = E.ResignedDate,
                                             stageName = s.ExitStage,
                                             EmployeeId = E.EmployeeID,
                                             WatchListEmployeeName = emp.EmployeeName,
                                             IsWithdrawn = E.IsWithdrawn
                                         }).Distinct().OrderByDescending(exid => exid.ResignedDate).ToList();

                    foreach (var item in StakeHoldersCheck)
                    {
                        //if (item.ReportingTo != employeeID)
                        //{
                        Tbl_HR_ExitStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ExitStageEvent
                                                             where empInfo.ExitInstanceId == item.ExitInstanceId
                                                             && (empInfo.FromStageId == 3 && empInfo.ToStageId == 4)
                                                             orderby empInfo.EventDateTime descending
                                                             select empInfo).FirstOrDefault();
                        List<Tbl_HR_ExitStageEvent> LatestEntryDepartments = new List<Tbl_HR_ExitStageEvent>();
                        if (LatestEntry != null)
                        {
                            LatestEntryDepartments = (from empInfo in dbContext.Tbl_HR_ExitStageEvent
                                                      where empInfo.ExitInstanceId == item.ExitInstanceId
                                                      && (empInfo.FromStageId == 4 && empInfo.ToStageId == 4)
                                                       && empInfo.StageActorEmployeeId == employeeID
                                                      && empInfo.EventDateTime > LatestEntry.EventDateTime
                                                      orderby empInfo.EventDateTime descending
                                                      select empInfo).ToList();
                        }
                        if (item.ReportingTo == employeeID && LatestEntryDepartments.Count(r => r.StageActorEmployeeId == employeeID) == 2)
                        {
                            continue;
                        }
                        else if (item.ReportingTo == employeeID && LatestEntryDepartments.Count(r => r.StageActorEmployeeId == employeeID) == 1)
                        {
                            DepartmentList.Add(item);
                        }
                        else if (LatestEntryDepartments.Count == 0)
                        {
                            DepartmentList.Add(item);
                        }
                        else if (LatestEntryDepartments.Count != 0)
                        {
                            continue;
                        }

                        //}
                    }
                }

                #endregion StakeHoldders Inbox Section

                #region RMG Inbox Section

                if (LogeedInEmRoles.Length > 0)
                {
                    if (stageapproversArray.Contains(employeeId))
                    {
                        var approverDetails = (from ex in dbContext.tbl_HR_ExitProcess_StageApprovers
                                               where ex.ExitInstanceID == 0 && ex.ApproverID == employeeId && ex.stageID == 8
                                               select ex).SingleOrDefault();

                        if (approverDetails != null)
                        {
                            RMGCheck = (from E in dbContext.tbl_HR_ExitInstance
                                        join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID
                                        join s in dbContext.tbl_HR_ExitStage on E.stageID equals s.ExitStageID
                                        where (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText))
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? emp.BusinessGroupID == FieldChild : field == "Organization Unit" ? emp.LocationID == FieldChild : field == "Stage Name" ? E.stageID == FieldChild : FieldChild == 0))) //field search
                                         && E.stageID == 8 && approverDetails.stageID == 8
                                        join ese in dbContext.Tbl_HR_ExitStageEvent on E.ExitInstanceID equals ese.ExitInstanceId into eventStageRecord  // Fix to add red Image support

                                        select new EmpSeparationApprovals
                                        {
                                            Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDateTime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                            ExitStageOrder = s.ExitStageOrder,
                                            ReportingTo = emp.ReportingTo,
                                            ExitInstanceId = E.ExitInstanceID,
                                            StageId = E.stageID,
                                            ResignedDate = E.ResignedDate,
                                            stageName = s.ExitStage,
                                            EmployeeId = E.EmployeeID,
                                            WatchListEmployeeName = emp.EmployeeName,
                                            IsWithdrawn = E.IsWithdrawn
                                        }).Distinct().OrderByDescending(exid => exid.ResignedDate).ToList();
                        }
                    }
                }

                #endregion RMG Inbox Section

                mainResult = employeeresult.Union(ProjectDepartmentList).Union(HRAdminCheck).Union(HRAdminHRApprovalStageCheck).Union(DepartmentList).Union(RMGCheck).OrderByDescending(m => m.ResignedDate).ToList();
                totalCount = mainResult.Count;
                //return mainResult.Skip((page - 1) * rows).Take(rows).ToList();
                return mainResult.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckLoginUserIsEmpOrManager(int employeeId)
        {
            try
            {
                var EmpManagerCheck = (from e in dbContext.HRMS_tbl_PM_Employee
                                       where e.ReportingTo == employeeId
                                       orderby e.EmployeeID descending
                                       select e).ToList();

                if (EmpManagerCheck.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SeparationShowStatus> GetSeparationStatusDetails(int page, int rows, int exitInstanceId, out int totalCount)
        {
            try
            {
                List<SeparationShowStatus> FinalResult = new List<SeparationShowStatus>();
                List<SeparationShowStatus> result = new List<SeparationShowStatus>();
                SeparationShowStatus secondresult = new SeparationShowStatus();
                string ApproverName = string.Empty;
                string ApproverNameFinal = string.Empty;
                var Resigndetails = (from s in dbContext.tbl_HR_ExitInstance where s.ExitInstanceID == exitInstanceId select s).FirstOrDefault();
                if (Resigndetails.stageID == 2)
                {
                    var empname = (from e in dbContext.HRMS_tbl_PM_Employee
                                   join ex in dbContext.tbl_HR_ExitInstance on e.EmployeeID equals ex.EmployeeID
                                   where ex.ExitInstanceID == exitInstanceId
                                   select e).FirstOrDefault();
                    if (empname.ReportingTo != null && empname.ReportingTo != 0)
                    {
                        HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(empname.ReportingTo.Value);
                        ApproverNameFinal = EmpDetails.EmployeeName;
                    }
                }
                else
                {
                    if (Resigndetails.stageID == 1)
                    {
                        var empname = (from e in dbContext.HRMS_tbl_PM_Employee
                                       join ex in dbContext.tbl_HR_ExitInstance on e.EmployeeID equals ex.EmployeeID
                                       where ex.ExitInstanceID == exitInstanceId
                                       select e).FirstOrDefault();

                        ApproverNameFinal = empname.EmployeeName;
                    }
                    else
                    {
                        var approverlist = (from e in dbContext.tbl_HR_ExitInstance
                                            join ex in dbContext.tbl_HR_ExitProcess_StageApprovers on e.stageID equals ex.stageID
                                            where ex.ExitInstanceID == 0 && e.ExitInstanceID == exitInstanceId
                                            select ex).ToList();

                        if (approverlist.Count > 0)
                        {
                            Tbl_HR_ExitStageEvent LatestEntryFromExit = new Tbl_HR_ExitStageEvent();
                            if (Resigndetails.stageID == 4)
                            {
                                var empname = (from e in dbContext.HRMS_tbl_PM_Employee
                                               join ex in dbContext.tbl_HR_ExitInstance on e.EmployeeID equals ex.EmployeeID
                                               where ex.ExitInstanceID == exitInstanceId
                                               select e).FirstOrDefault();
                                HRMS_tbl_PM_Employee EmpDetailsmgr = new HRMS_tbl_PM_Employee();
                                if (empname.ReportingTo != null && empname.ReportingTo != 0)
                                {
                                    EmpDetailsmgr = employeeDAL.GetEmployeeDetails(empname.ReportingTo.Value);
                                }
                                LatestEntryFromExit = (from emp in dbContext.Tbl_HR_ExitStageEvent
                                                       where emp.ExitInstanceId == exitInstanceId && emp.FromStageId == 3 && emp.ToStageId == 4
                                                       orderby emp.EventDateTime descending
                                                       select emp).FirstOrDefault();
                                List<Tbl_HR_ExitStageEvent> EntryFromDepartmentsList = new List<Tbl_HR_ExitStageEvent>();
                                if (LatestEntryFromExit != null)
                                {
                                    EntryFromDepartmentsList = (from emp in dbContext.Tbl_HR_ExitStageEvent
                                                                where emp.ExitInstanceId == exitInstanceId && emp.FromStageId == 4 && emp.ToStageId == 4 && emp.EventDateTime > LatestEntryFromExit.EventDateTime
                                                                select emp).ToList();
                                    foreach (var obj in approverlist)
                                    {
                                        if (EntryFromDepartmentsList.Count(r => r.StageActorEmployeeId == obj.ApproverID) >= 1)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetailsExit(obj.ApproverID.Value);
                                            if (EmpDetails == null)
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                ApproverName = ApproverName + EmpDetails.EmployeeName + ",";
                                            }
                                        }
                                    }
                                    if (EntryFromDepartmentsList.Count(r => r.StageActorEmployeeId == EmpDetailsmgr.EmployeeID) == 0)
                                    {
                                        //HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(EmpDetailsmgr.EmployeeID);
                                        ApproverName = ApproverName + EmpDetailsmgr.EmployeeName + ",";
                                    }
                                }
                                if (ApproverName == "" && Resigndetails.stageID == 4)
                                    ApproverName = EmpDetailsmgr.EmployeeName;
                            }
                            else
                            {
                                foreach (var obj in approverlist)
                                {
                                    HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetailsExit(obj.ApproverID.Value);
                                    if (EmpDetails == null)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        ApproverName = ApproverName + EmpDetails.EmployeeName + ",";
                                    }
                                }
                            }
                            char[] symbols = new char[] { ',' };
                            ApproverNameFinal = ApproverName.TrimEnd(symbols);
                        }
                    }
                }

                result = (from se in dbContext.Tbl_HR_ExitStageEvent
                          join e in dbContext.HRMS_tbl_PM_Employee on se.StageActorEmployeeId equals e.EmployeeID into stageEvent
                          from EEvent in stageEvent.DefaultIfEmpty()
                          join ex in dbContext.tbl_HR_ExitInstance on se.ExitInstanceId equals ex.ExitInstanceID into Exit
                          from EExit in Exit.DefaultIfEmpty()
                          join s in dbContext.tbl_HR_ExitStage on se.FromStageId equals s.ExitStageID into stage
                          from EStage in stage.DefaultIfEmpty()
                          join e in dbContext.HRMS_tbl_PM_Employee on EExit.EmployeeID equals e.EmployeeID into stageEvent1
                          from EEvent1 in stageEvent1.DefaultIfEmpty()
                          where EExit.ExitInstanceID == exitInstanceId
                          orderby se.Id ascending

                          select new SeparationShowStatus
                          {
                              ShowstatusAction = se.Action,
                              ShowstatusActor = EEvent.EmployeeName,
                              ShowstatusComments = se.Comments,
                              ShowstatusCurrentStage = EStage.ExitStage,
                              ShowstatusStageID = se.FromStageId,
                              ShowstatusEmployeeCode = EEvent1.EmployeeCode,
                              ShowstatusEmployeeId = EExit.EmployeeID,
                              ShowstatusEmployeeName = EEvent1.EmployeeName,
                              ShowstatusTime = se.EventDateTime
                          }).ToList();

                if (result.Any())
                    FinalResult.AddRange(result);

                tbl_HR_ExitInstance iswithdrawn = (from s in dbContext.tbl_HR_ExitInstance where s.ExitInstanceID == exitInstanceId select s).FirstOrDefault();

                if (iswithdrawn.IsWithdrawn == false && iswithdrawn.stageID != 7)
                {
                    secondresult = (from ex in dbContext.tbl_HR_ExitInstance
                                    join s in dbContext.tbl_HR_ExitStage on ex.stageID equals s.ExitStageID into stage
                                    from EStage in stage.DefaultIfEmpty()
                                    where ex.ExitInstanceID == exitInstanceId
                                    select new SeparationShowStatus
                                    {
                                        ShowstatusCurrentStage = EStage.ExitStage,
                                        showStatus = "Waiting for " + ApproverNameFinal + " to take Action"
                                    }).FirstOrDefault();

                    FinalResult.Add(secondresult);
                }
                else if (iswithdrawn.IsWithdrawn == false && iswithdrawn.stageID == 7)
                {
                    secondresult = (from ex in dbContext.tbl_HR_ExitInstance
                                    join s in dbContext.tbl_HR_ExitStage on ex.stageID equals s.ExitStageID into stage
                                    from EStage in stage.DefaultIfEmpty()
                                    where ex.ExitInstanceID == exitInstanceId
                                    select new SeparationShowStatus
                                    {
                                        ShowstatusCurrentStage = EStage.ExitStage
                                    }).FirstOrDefault();

                    FinalResult.Add(secondresult);
                }

                totalCount = FinalResult.Count;

                return FinalResult.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool WithdrawEmployeeResignation(int exitInstanceId)
        {
            try
            {
                bool status = false;
                Tbl_HR_ExitStageEvent obj = new Tbl_HR_ExitStageEvent();
                tbl_HR_ExitInstance resignDetails = (from r in dbContext.tbl_HR_ExitInstance
                                                     where r.ExitInstanceID == exitInstanceId
                                                     select r).SingleOrDefault();

                if (resignDetails != null)
                {
                    if (resignDetails.IsWithdrawn != true)
                    {
                        resignDetails.IsWithdrawn = true;
                        resignDetails.stageID = 1;
                        dbContext.SaveChanges();

                        obj.ExitInstanceId = resignDetails.ExitInstanceID;
                        obj.EventDateTime = DateTime.Now;
                        obj.Action = "Withdraw";
                        obj.FromStageId = resignDetails.stageID;
                        obj.ToStageId = 1;
                        obj.StageActorEmployeeId = resignDetails.EmployeeID;
                        dbContext.Tbl_HR_ExitStageEvent.AddObject(obj);
                    }
                }

                HRMS_tbl_PM_Employee empdetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == resignDetails.EmployeeID).FirstOrDefault();
                if (empdetails != null)
                {
                    empdetails.EmployeeStatusMasterID = resignDetails.PrevoiusEmpStatusMatserId;
                    empdetails.EmployeeStatusID = resignDetails.PrevoiusEmpStatusId;
                    dbContext.SaveChanges();
                    status = true;
                };

                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveShowDetailsData(SeparationShowDetails model)
        {
            try
            {
                bool status = false;

                tbl_HR_ExitInstance exit = (from e in dbContext.tbl_HR_ExitInstance
                                            where e.ExitInstanceID == model.SeparationFormDetails.SeparationId
                                            select e).SingleOrDefault();

                if (exit != null)
                {
                    exit.EmployeeComment = model.SeparationFormDetails.InitiatorComment;
                    exit.ManagerComment = model.SeparationFormDetails.ManagerComment;
                    exit.HRComment = model.SeparationFormDetails.HRComment;
                    exit.ReasonID = Convert.ToInt32(model.SeparationFormDetails.ReasonForSeparartion);
                    exit.ResignedDate = model.SeparationFormDetails.ResignedDate;
                    exit.RMGComment = model.SeparationFormDetails.RMGComment;
                    exit.TentativeReleavingDate = model.SeparationFormDetails.TentativeReleaseDate;
                    exit.AgreedReleaseDate = model.SeparationFormDetails.AgreedReleaseDate;
                    exit.waiveof = Convert.ToInt32(model.SeparationFormDetails.WaiveOff);
                    dbContext.SaveChanges();
                    status = true;
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ApproveShowDetailsData(SeparationShowDetails model)
        {
            try
            {
                bool datasaved = SaveShowDetailsData(model);
                int? OldStageId = 0;
                bool status = false;

                if (datasaved == true)
                {
                    tbl_HR_ExitInstance exi = (from e in dbContext.tbl_HR_ExitInstance
                                               where e.ExitInstanceID == model.SeparationFormDetails.SeparationId
                                               select e
                      ).SingleOrDefault();

                    if (exi != null)
                    {
                        OldStageId = exi.stageID;
                        if (exi.stageID == (from a in dbContext.tbl_HR_ExitStage where a.Description == "Line Manager Approval" select a.ExitStageID).SingleOrDefault())
                            exi.stageID = (from a in dbContext.tbl_HR_ExitStage where a.Description == "HR Approval Stage" select a.ExitStageID).SingleOrDefault();
                        else
                        {
                            if (exi.stageID == (from a in dbContext.tbl_HR_ExitStage where a.Description == "Exit" select a.ExitStageID).SingleOrDefault())
                                exi.stageID = exi.stageID;
                            else
                            {
                                exi.stageID = exi.stageID + 1;
                            }
                        }
                    }
                    dbContext.SaveChanges();

                    Tbl_HR_ExitStageEvent obj = new Tbl_HR_ExitStageEvent();
                    tbl_HR_ExitInstance resignDetails = (from r in dbContext.tbl_HR_ExitInstance
                                                         where r.ExitInstanceID == model.SeparationFormDetails.SeparationId
                                                         select r).SingleOrDefault();

                    if (resignDetails != null)
                    {
                        int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);

                        obj.ExitInstanceId = resignDetails.ExitInstanceID;
                        obj.EventDateTime = DateTime.Now;
                        obj.Action = "Approve";
                        obj.FromStageId = OldStageId;
                        obj.ToStageId = resignDetails.stageID;
                        obj.StageActorEmployeeId = employeeID;

                        string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                        string user = Commondal.GetMaxRoleForUser(role);
                        int empID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                        //bool result = CheckLoginUserIsEmpOrManager(empID);
                        if (user == "RMG")
                            obj.Comments = model.SeparationFormDetails.RMGComment;
                        else
                        {
                            //if (user == "HR Admin" && result != true)
                            if (user == "HR Admin")
                            {
                                obj.Comments = model.SeparationFormDetails.HRComment;
                            }
                            else
                            {
                                //if (result == true)
                                obj.Comments = model.SeparationFormDetails.ManagerComment;
                            }
                        }

                        dbContext.Tbl_HR_ExitStageEvent.AddObject(obj);
                        dbContext.SaveChanges();

                        status = true;
                    }
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool RejectShowDetailsData(SeparationShowDetails model)
        {
            try
            {
                bool datasaved = SaveShowDetailsData(model);
                int? OldStageId = 0;
                bool status = false;

                if (datasaved == true)
                {
                    tbl_HR_ExitInstance exi = (from e in dbContext.tbl_HR_ExitInstance
                                               where e.ExitInstanceID == model.SeparationFormDetails.SeparationId
                                               select e
                      ).SingleOrDefault();

                    if (exi != null)
                    {
                        OldStageId = exi.stageID;


                        if (exi.stageID == (from a in dbContext.tbl_HR_ExitStage where a.Description == "HR Approval Stage" select a.ExitStageID).SingleOrDefault())
                            exi.stageID = (from a in dbContext.tbl_HR_ExitStage where a.Description == "Line Manager Approval" select a.ExitStageID).SingleOrDefault();
                        else if (exi.stageID == (from a in dbContext.tbl_HR_ExitStage where a.Description == "RMG Approval Stage" select a.ExitStageID).SingleOrDefault())
                            exi.stageID = (from a in dbContext.tbl_HR_ExitStage where a.Description == "Line Manager Approval" select a.ExitStageID).SingleOrDefault();
                        else
                        {
                            exi.stageID = exi.stageID - 1;
                        }

                    }

                    dbContext.SaveChanges();

                    Tbl_HR_ExitStageEvent obj = new Tbl_HR_ExitStageEvent();
                    tbl_HR_ExitInstance resignDetails = (from r in dbContext.tbl_HR_ExitInstance
                                                         where r.ExitInstanceID == model.SeparationFormDetails.SeparationId
                                                         select r).SingleOrDefault();

                    if (resignDetails != null)
                    {
                        int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);

                        obj.ExitInstanceId = resignDetails.ExitInstanceID;
                        obj.EventDateTime = DateTime.Now;
                        obj.Action = "Reject";
                        obj.FromStageId = OldStageId;
                        obj.ToStageId = resignDetails.stageID;
                        obj.StageActorEmployeeId = employeeID;

                        string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                        string user = Commondal.GetMaxRoleForUser(role);
                        bool result = CheckLoginUserIsEmpOrManager(model.EmployeeId.Value);
                        if (user == "RMG")
                            obj.Comments = model.SeparationFormDetails.RMGComment;
                        else
                        {
                            if (user == "HR Admin")
                            {
                                obj.Comments = model.SeparationFormDetails.HRComment;
                            }
                            else
                            {
                                if (result == true)
                                    obj.Comments = model.SeparationFormDetails.ManagerComment;
                            }
                        }

                        dbContext.Tbl_HR_ExitStageEvent.AddObject(obj);
                        dbContext.SaveChanges();

                        status = true;
                    }
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SubmitShowDetailsData(SeparationShowDetails model)
        {
            try
            {
                bool datasaved = SaveShowDetailsData(model);
                int? OldStageId = 0;
                bool status = false;

                if (datasaved == true)
                {
                    tbl_HR_ExitInstance exi = (from e in dbContext.tbl_HR_ExitInstance
                                               where e.ExitInstanceID == model.SeparationFormDetails.SeparationId
                                               select e
                      ).SingleOrDefault();

                    if (exi != null)
                    {
                        OldStageId = exi.stageID;
                        exi.stageID = 2;
                    }
                    dbContext.SaveChanges();

                    Tbl_HR_ExitStageEvent obj = new Tbl_HR_ExitStageEvent();
                    tbl_HR_ExitInstance resignDetails = (from r in dbContext.tbl_HR_ExitInstance
                                                         where r.ExitInstanceID == model.SeparationFormDetails.SeparationId
                                                         select r).SingleOrDefault();

                    if (resignDetails != null)
                    {
                        int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);

                        obj.ExitInstanceId = resignDetails.ExitInstanceID;
                        obj.EventDateTime = DateTime.Now;
                        obj.Action = "Submit";
                        obj.FromStageId = OldStageId;
                        obj.ToStageId = resignDetails.stageID;
                        obj.StageActorEmployeeId = employeeID;

                        string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                        string user = Commondal.GetMaxRoleForUser(role);
                        bool result = CheckLoginUserIsEmpOrManager(model.EmployeeId.Value);

                        obj.Comments = model.SeparationFormDetails.InitiatorComment;

                        dbContext.Tbl_HR_ExitStageEvent.AddObject(obj);
                        dbContext.SaveChanges();

                        status = true;
                    }
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionnaireQuestion> GetFinanceQuestionnaireQuestionDetails()
        {
            try
            {
                List<QuestionnaireQuestion> finance = (from e in dbContext.tbl_Q_QuestionnaireQuestion
                                                       where e.QuestionnaireID == 4
                                                       select new QuestionnaireQuestion
                                                       {
                                                           QuetionRevisionID = e.RevisionId,
                                                           QuestionnaireQuestionID = e.QuestionnaireQuestionID,
                                                           QuestionDescription = e.QuestionDescription,
                                                           wattage = e.wattage
                                                       }).ToList();

                return finance;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Q_QuestionnaireQuestion GetFinanceRevisionId()
        {
            try
            {
                tbl_Q_QuestionnaireQuestion finance = (from e in dbContext.tbl_Q_QuestionnaireQuestion
                                                       where e.QuestionnaireID == 4
                                                       select e).FirstOrDefault();

                return finance;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DepartmentResponce> GetResponceList(int ExitInstanceId)
        {
            try
            {
                List<DepartmentResponce> Responce = (from e in dbContext.tbl_PM_ChecklistResponses
                                                     where e.ContextId == ExitInstanceId
                                                     select new DepartmentResponce
                                                       {
                                                           exitinstanceid = e.ContextId,
                                                           checklistitem = e.CheckListItemId,
                                                           checklistresponce = e.ResponseId,
                                                           ResponceComments = e.Comments,
                                                           RevisionIDDepartment = e.RevisionId
                                                       }).ToList();

                return Responce;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionnaireOption> GetAdminQuestionnaireOptionDetails()
        {
            try
            {
                List<QuestionnaireOption> finance = GetFinanceQuestionnaireOptionDetails();
                List<QuestionnaireOption> IT = GetITQuestionnaireOptionDetails();
                List<QuestionnaireOption> HR = GetHRQuestionnaireOptionDetails();
                List<QuestionnaireOption> Admin = GetADMINQuestionnaireOptionDetails();
                List<QuestionnaireOption> Project = GetProjectQuestionnaireOptionDetails();
                List<QuestionnaireOption> Asset = GetAssetQuestionnaireOptionDetails();
                List<QuestionnaireOption> Result = new List<QuestionnaireOption>();
                Result = finance.Union(IT).Union(HR).Union(Admin).Union(Project).Union(Asset).ToList();
                return Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionnaireQuestion> GetAdminQuestionnaireQuestionDetails()
        {
            try
            {
                List<QuestionnaireQuestion> finance = GetFinanceQuestionnaireQuestionDetails();
                List<QuestionnaireQuestion> IT = GetITQuestionnaireQuestionDetails();
                List<QuestionnaireQuestion> HR = GetHRQuestionnaireQuestionDetails();
                List<QuestionnaireQuestion> ADMIN = GetADMINQuestionnaireQuestionDetails();
                List<QuestionnaireQuestion> Project = GetProjectQuestionnaireQuestionDetails();
                List<QuestionnaireQuestion> Asset = GetAssetQuestionnaireQuestionDetails();
                List<QuestionnaireQuestion> Result = new List<QuestionnaireQuestion>();
                Result = finance.Union(IT).Union(HR).Union(ADMIN).Union(Project).Union(Asset).ToList();
                return Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionnaireOption> GetFinanceQuestionnaireOptionDetails()
        {
            try
            {
                List<QuestionnaireOption> finance = (from e in dbContext.tbl_Q_QuestionnaireOption
                                                     where e.QuestionnaireID == 4
                                                     select new QuestionnaireOption
                                                     {
                                                         QuestionnaireQuestionID = e.QuestionnaireQuestionID,
                                                         OrderInWhichToAppear = e.QuestionnaireOptionID,
                                                         QuestionnaireOptionID = e.QuestionnaireOptionID,
                                                         OptionDescription = e.OptionDescription
                                                     }).ToList();

                return finance;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool savefinanceseparationDetails(FinanceClearance model)
        {
            bool isSuccess = false;
            ExitProcessDAL dal = new ExitProcessDAL();
            try
            {
                for (int i = 0; i < model.QuestionnaireQuestions.Count; i++)
                {
                    //var checklistResponces = dbContext.tbl_PM_ChecklistResponses.Where(x => x.ContextId == model.ExitInstanceId && (x.CheckListItemId==model.QuestionnaireQuestions[i].QuestionnaireQuestionID)).ToList();
                    int QuestionnaireQuestionID = model.QuestionnaireQuestions[i].QuestionnaireQuestionID;
                    var checklistResponces = (from c in dbContext.tbl_PM_ChecklistResponses
                                              where c.ContextId == model.ExitInstanceId && c.CheckListItemId == QuestionnaireQuestionID
                                              select c).ToList();

                    if (checklistResponces.Count() != 0)
                    {
                        var item = model.QuestionnaireQuestions[i];
                        tbl_PM_ChecklistResponses tblcommentsdetails1 =
                                this.dbContext.tbl_PM_ChecklistResponses.FirstOrDefault(com => com.ContextId == model.ExitInstanceId && (com.CheckListItemId == item.QuestionnaireQuestionID));
                        tblcommentsdetails1.Comments = item.Comments;
                        tblcommentsdetails1.ResponseId = model.QuestionnaireOptions[i].QuestionnaireOptionID;

                        dbContext.SaveChanges();
                    }
                    else
                    {
                        var item = model.QuestionnaireQuestions[i];
                        tbl_PM_ChecklistResponses tblcommentsdetails = new tbl_PM_ChecklistResponses
                        {
                            ContextId = model.ExitInstanceId,
                            CheckListItemId = item.QuestionnaireQuestionID,
                            Comments = item.Comments,

                            ResponseId = model.QuestionnaireOptions[i].QuestionnaireOptionID,

                            CheckListID = (from q in dbContext.tbl_Q_QuestionnaireQuestion
                                           where q.QuestionnaireQuestionID == item.QuestionnaireQuestionID
                                           select q.QuestionnaireQuestionID).FirstOrDefault(),

                            RevisionId = (from q in dbContext.tbl_Q_QuestionnaireQuestion
                                          where q.QuestionnaireQuestionID == item.QuestionnaireQuestionID
                                          select q.RevisionId).FirstOrDefault(),
                        };
                        dbContext.tbl_PM_ChecklistResponses.AddObject(tblcommentsdetails);

                        dbContext.SaveChanges();
                    }
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }

        //save Asset department
        public bool saveAssetseparationDetails(FinanceClearance model)
        {
            bool isSuccess = false;
            ExitProcessDAL dal = new ExitProcessDAL();
            try
            {
                for (int i = 0; i < model.QuestionnaireQuestions.Count; i++)
                {
                    //var checklistResponces = dbContext.tbl_PM_ChecklistResponses.Where(x => x.ContextId == model.ExitInstanceId && (x.CheckListItemId==model.QuestionnaireQuestions[i].QuestionnaireQuestionID)).ToList();
                    int QuestionnaireQuestionID = model.QuestionnaireQuestions[i].QuestionnaireQuestionID;
                    var checklistResponces = (from c in dbContext.tbl_PM_ChecklistResponses
                                              where c.ContextId == model.ExitInstanceId && c.CheckListItemId == QuestionnaireQuestionID
                                              select c).ToList();

                    if (checklistResponces.Count() != 0)
                    {
                        var item = model.QuestionnaireQuestions[i];
                        tbl_PM_ChecklistResponses tblcommentsdetails1 =
                                this.dbContext.tbl_PM_ChecklistResponses.FirstOrDefault(com => com.ContextId == model.ExitInstanceId && (com.CheckListItemId == item.QuestionnaireQuestionID));
                        tblcommentsdetails1.Comments = item.Comments;
                        tblcommentsdetails1.ResponseId = model.QuestionnaireOptions[i].QuestionnaireOptionID;

                        dbContext.SaveChanges();
                    }
                    else
                    {
                        var item = model.QuestionnaireQuestions[i];
                        tbl_PM_ChecklistResponses tblcommentsdetails = new tbl_PM_ChecklistResponses
                        {
                            ContextId = model.ExitInstanceId,
                            CheckListItemId = item.QuestionnaireQuestionID,
                            Comments = item.Comments,

                            ResponseId = model.QuestionnaireOptions[i].QuestionnaireOptionID,

                            CheckListID = (from q in dbContext.tbl_Q_QuestionnaireQuestion
                                           where q.QuestionnaireQuestionID == item.QuestionnaireQuestionID
                                           select q.QuestionnaireQuestionID).FirstOrDefault(),

                            RevisionId = (from q in dbContext.tbl_Q_QuestionnaireQuestion
                                          where q.QuestionnaireQuestionID == item.QuestionnaireQuestionID
                                          select q.RevisionId).FirstOrDefault(),
                        };
                        dbContext.tbl_PM_ChecklistResponses.AddObject(tblcommentsdetails);

                        dbContext.SaveChanges();
                    }
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }

        public List<QuestionnaireQuestion> GetITQuestionnaireQuestionDetails()
        {
            try
            {
                List<QuestionnaireQuestion> IT = (from e in dbContext.tbl_Q_QuestionnaireQuestion
                                                  where e.QuestionnaireID == 2
                                                  orderby e.QuestionnaireQuestionID
                                                  select new QuestionnaireQuestion
                                                  {
                                                      QuetionRevisionID = e.RevisionId,
                                                      QuestionnaireQuestionID = e.QuestionnaireQuestionID,
                                                      QuestionDescription = e.QuestionDescription,
                                                      wattage = e.wattage
                                                  }).ToList();

                return IT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Asset Department

        public List<QuestionnaireQuestion> GetAssetQuestionnaireQuestionDetails()
        {
            try
            {
                List<QuestionnaireQuestion> Asset = (from e in dbContext.tbl_Q_QuestionnaireQuestion
                                                     where e.QuestionnaireID == 20
                                                     orderby e.QuestionnaireQuestionID
                                                     select new QuestionnaireQuestion
                                                     {
                                                         QuetionRevisionID = e.RevisionId,
                                                         QuestionnaireQuestionID = e.QuestionnaireQuestionID,
                                                         QuestionDescription = e.QuestionDescription,
                                                         wattage = e.wattage
                                                     }).ToList();

                return Asset;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Q_QuestionnaireQuestion GetITRevisionId()
        {
            try
            {
                tbl_Q_QuestionnaireQuestion finance = (from e in dbContext.tbl_Q_QuestionnaireQuestion
                                                       where e.QuestionnaireID == 2
                                                       select e).FirstOrDefault();

                return finance;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionnaireOption> GetITQuestionnaireOptionDetails()
        {
            try
            {
                List<QuestionnaireOption> IT = (from e in dbContext.tbl_Q_QuestionnaireOption
                                                where e.QuestionnaireID == 2
                                                orderby e.QuestionnaireQuestionID
                                                select new QuestionnaireOption
                                                {
                                                    QuestionnaireQuestionID = e.QuestionnaireQuestionID,
                                                    OrderInWhichToAppear = e.QuestionnaireOptionID,
                                                    QuestionnaireOptionID = e.QuestionnaireOptionID,
                                                    OptionDescription = e.OptionDescription
                                                }).ToList();

                return IT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Asset Department
        public List<QuestionnaireOption> GetAssetQuestionnaireOptionDetails()
        {
            try
            {
                List<QuestionnaireOption> Asset = (from e in dbContext.tbl_Q_QuestionnaireOption
                                                   where e.QuestionnaireID == 20
                                                   orderby e.QuestionnaireQuestionID
                                                   select new QuestionnaireOption
                                                   {
                                                       QuestionnaireQuestionID = e.QuestionnaireQuestionID,
                                                       OrderInWhichToAppear = e.QuestionnaireOptionID,
                                                       QuestionnaireOptionID = e.QuestionnaireOptionID,
                                                       OptionDescription = e.OptionDescription
                                                   }).ToList();

                return Asset;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionnaireQuestion> GetHRQuestionnaireQuestionDetails()
        {
            try
            {
                List<QuestionnaireQuestion> HR = (from e in dbContext.tbl_Q_QuestionnaireQuestion
                                                  where e.QuestionnaireID == 5
                                                  orderby e.QuestionnaireQuestionID
                                                  select new QuestionnaireQuestion
                                                  {
                                                      QuetionRevisionID = e.RevisionId,
                                                      QuestionnaireQuestionID = e.QuestionnaireQuestionID,
                                                      QuestionDescription = e.QuestionDescription,
                                                      wattage = e.wattage
                                                  }).ToList();

                return HR;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionnaireQuestion> GetProjectQuestionnaireQuestionDetails()
        {
            try
            {
                List<QuestionnaireQuestion> Project = (from e in dbContext.tbl_Q_QuestionnaireQuestion
                                                       where e.QuestionnaireID == 9
                                                       orderby e.QuestionnaireQuestionID
                                                       select new QuestionnaireQuestion
                                                       {
                                                           QuetionRevisionID = e.RevisionId,
                                                           QuestionnaireQuestionID = e.QuestionnaireQuestionID,
                                                           QuestionDescription = e.QuestionDescription,
                                                           wattage = e.wattage
                                                       }).ToList();

                return Project;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Q_QuestionnaireQuestion GetHRRevisionId()
        {
            try
            {
                tbl_Q_QuestionnaireQuestion finance = (from e in dbContext.tbl_Q_QuestionnaireQuestion
                                                       where e.QuestionnaireID == 5
                                                       select e).FirstOrDefault();

                return finance;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Q_QuestionnaireQuestion GetProjectRevisionId()
        {
            try
            {
                tbl_Q_QuestionnaireQuestion Project = (from e in dbContext.tbl_Q_QuestionnaireQuestion
                                                       where e.QuestionnaireID == 9
                                                       select e).FirstOrDefault();

                return Project;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionnaireOption> GetHRQuestionnaireOptionDetails()
        {
            try
            {
                List<QuestionnaireOption> HR = (from e in dbContext.tbl_Q_QuestionnaireOption
                                                where e.QuestionnaireID == 5
                                                orderby e.QuestionnaireQuestionID
                                                select new QuestionnaireOption
                                                {
                                                    QuestionnaireQuestionID = e.QuestionnaireQuestionID,
                                                    OrderInWhichToAppear = e.QuestionnaireOptionID,
                                                    QuestionnaireOptionID = e.QuestionnaireOptionID,
                                                    OptionDescription = e.OptionDescription
                                                }).ToList();

                return HR;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionnaireOption> GetProjectQuestionnaireOptionDetails()
        {
            try
            {
                List<QuestionnaireOption> Project = (from e in dbContext.tbl_Q_QuestionnaireOption
                                                     where e.QuestionnaireID == 9
                                                     orderby e.QuestionnaireQuestionID
                                                     select new QuestionnaireOption
                                                     {
                                                         QuestionnaireQuestionID = e.QuestionnaireQuestionID,
                                                         OrderInWhichToAppear = e.QuestionnaireOptionID,
                                                         QuestionnaireOptionID = e.QuestionnaireOptionID,
                                                         OptionDescription = e.OptionDescription
                                                     }).ToList();

                return Project;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionnaireQuestion> GetADMINQuestionnaireQuestionDetails()
        {
            try
            {
                List<QuestionnaireQuestion> ADMIN = (from e in dbContext.tbl_Q_QuestionnaireQuestion
                                                     where e.QuestionnaireID == 3
                                                     orderby e.QuestionnaireQuestionID
                                                     select new QuestionnaireQuestion
                                                     {
                                                         QuetionRevisionID = e.RevisionId,
                                                         QuestionnaireQuestionID = e.QuestionnaireQuestionID,
                                                         QuestionDescription = e.QuestionDescription,
                                                         wattage = e.wattage
                                                     }).ToList();

                return ADMIN;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Q_QuestionnaireQuestion GetAdminRevisionId()
        {
            try
            {
                tbl_Q_QuestionnaireQuestion finance = (from e in dbContext.tbl_Q_QuestionnaireQuestion
                                                       where e.QuestionnaireID == 3
                                                       select e).FirstOrDefault();

                return finance;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Asset Department
        public tbl_Q_QuestionnaireQuestion GetAssetRevisionId()
        {
            try
            {
                tbl_Q_QuestionnaireQuestion Asset = (from e in dbContext.tbl_Q_QuestionnaireQuestion
                                                     where e.QuestionnaireID == 20
                                                     select e).FirstOrDefault();

                return Asset;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionnaireOption> GetADMINQuestionnaireOptionDetails()
        {
            try
            {
                List<QuestionnaireOption> ADMIN = (from e in dbContext.tbl_Q_QuestionnaireOption
                                                   where e.QuestionnaireID == 3
                                                   orderby e.QuestionnaireQuestionID
                                                   select new QuestionnaireOption
                                                   {
                                                       QuestionnaireQuestionID = e.QuestionnaireQuestionID,
                                                       OrderInWhichToAppear = e.QuestionnaireOptionID,
                                                       QuestionnaireOptionID = e.QuestionnaireOptionID,
                                                       OptionDescription = e.OptionDescription
                                                   }).ToList();

                return ADMIN;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public FinanceClearance GettentativeDetails(int ExitInstanceId)
        {
            try
            {
                FinanceClearance Resign = (from r in dbContext.tbl_HR_ExitInstance
                                           join e in dbContext.HRMS_tbl_PM_Employee on r.EmployeeID equals e.EmployeeID
                                           join c in dbContext.tbl_PM_OfficeLocation on e.OfficeLocation equals c.OfficeLocationID into x
                                           from location in x.DefaultIfEmpty()
                                           where r.ExitInstanceID == ExitInstanceId
                                           orderby r.ExitChecklistID descending
                                           select new FinanceClearance
                                           {
                                               TentativeReleaseDate = r.TentativeReleavingDate,
                                               EmployeeId = r.EmployeeID,
                                               EmployeeName = e.EmployeeName,
                                               location = location.OfficeLocation,
                                               ExitInstanceId = r.ExitInstanceID
                                           }
                                               ).FirstOrDefault();

                return Resign;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetjqinboxEmployeeID(int ExitInstanceId)
        {
            try
            {
                int? employeeID = (from e in dbContext.tbl_HR_ExitInstance
                                   where e.ExitInstanceID == ExitInstanceId
                                   select e.EmployeeID).FirstOrDefault();

                return employeeID.HasValue ? employeeID.Value : 0;
            }
            catch
            {
                throw;
            }
        }

        public bool PushBackDetailsData(int ExitInstanceId)
        {
            try
            {
                int? OldStageId = 0;
                bool status = false;

                tbl_HR_ExitInstance exi = (from e in dbContext.tbl_HR_ExitInstance
                                           where e.ExitInstanceID == ExitInstanceId
                                           select e
                  ).SingleOrDefault();

                if (exi != null)
                {
                    OldStageId = exi.stageID;

                    if (exi.stageID == (from a in dbContext.tbl_HR_ExitStage where a.Description == "Department Clearance" select a.ExitStageID).SingleOrDefault())
                    {
                        exi.stageID = exi.stageID - 1;
                    }
                }
                dbContext.SaveChanges();

                Tbl_HR_ExitStageEvent obj = new Tbl_HR_ExitStageEvent();
                tbl_HR_ExitInstance resignDetails = (from r in dbContext.tbl_HR_ExitInstance
                                                     where r.ExitInstanceID == ExitInstanceId
                                                     select r).SingleOrDefault();

                if (resignDetails != null)
                {
                    int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);

                    obj.ExitInstanceId = resignDetails.ExitInstanceID;
                    obj.EventDateTime = DateTime.Now;
                    obj.Action = "Push Back";
                    obj.FromStageId = OldStageId;
                    obj.ToStageId = resignDetails.stageID;
                    obj.StageActorEmployeeId = employeeID;

                    dbContext.Tbl_HR_ExitStageEvent.AddObject(obj);
                    dbContext.SaveChanges();

                    status = true;
                }

                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Tbl_HR_ExitStageEvent> LatestDepartmentEntry(int ExitInstanceId)
        {
            try
            {
                //for selecting latest entry with From stage id is 3
                Tbl_HR_ExitStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ExitStageEvent
                                                     where empInfo.FromStageId == 3 && empInfo.ToStageId == 4 && empInfo.ExitInstanceId == ExitInstanceId
                                                     orderby empInfo.EventDateTime descending
                                                     select empInfo).FirstOrDefault();

                //for selecting total no. of entries of From stage id 4\
                List<Tbl_HR_ExitStageEvent> TotalRecords = new List<Tbl_HR_ExitStageEvent>();
                if (LatestEntry != null)
                {
                    TotalRecords = (from total in dbContext.Tbl_HR_ExitStageEvent
                                    where total.ExitInstanceId == ExitInstanceId && total.FromStageId == 4 && (total.ToStageId == 4 || total.ToStageId == 5) && total.EventDateTime > LatestEntry.EventDateTime
                                    select total).ToList();
                }
                return TotalRecords;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool MoveAheadDetailsData(FinanceClearance model)
        {
            try
            {
                int? OldStageId = 0;
                bool status = false;
                Tbl_HR_ExitStageEvent flag = (from e in dbContext.Tbl_HR_ExitStageEvent
                                              where e.FromStageId == 4 && e.ExitInstanceId == model.ExitInstanceId && e.QuestionnaireID == model.QuestionnaireID
                                              orderby e.EventDateTime descending
                                              select e).FirstOrDefault();

                if (flag != null)
                    return status;
                status = savefinanceseparationDetails(model);
                if (status)
                {
                    tbl_HR_ExitInstance exi = (from e in dbContext.tbl_HR_ExitInstance
                                               where e.ExitInstanceID == model.ExitInstanceId
                                               orderby e.CreatedDate descending
                                               select e
                      ).FirstOrDefault();

                    //for selecting latest entry with From stage id is 3
                    Tbl_HR_ExitStageEvent LatestEntry = (from empInfo in dbContext.Tbl_HR_ExitStageEvent
                                                         where empInfo.FromStageId == 3 && empInfo.ExitInstanceId == model.ExitInstanceId
                                                         orderby empInfo.EventDateTime descending
                                                         select empInfo).FirstOrDefault();

                    //for selecting total no. of entries of From stage id 4
                    List<Tbl_HR_ExitStageEvent> TotalRecords = new List<Tbl_HR_ExitStageEvent>();
                    if (LatestEntry != null)
                    {
                        TotalRecords = (from total in dbContext.Tbl_HR_ExitStageEvent
                                        where total.ExitInstanceId == model.ExitInstanceId && total.Action == "Move Ahead" && total.EventDateTime > LatestEntry.EventDateTime
                                        select total).ToList();
                    }
                    if (exi != null)
                    {
                        OldStageId = exi.stageID;

                        if (exi.stageID == (from a in dbContext.tbl_HR_ExitStage where a.Description == "Department Clearance" select a.ExitStageID).SingleOrDefault())
                        {
                            if (TotalRecords.Count() == 5)
                            {
                                exi.stageID = exi.stageID + 1;
                            }
                            else
                            {
                                exi.stageID = exi.stageID;
                            }
                        }
                    }
                    dbContext.SaveChanges();

                    Tbl_HR_ExitStageEvent obj = new Tbl_HR_ExitStageEvent();
                    tbl_HR_ExitInstance resignDetails = (from r in dbContext.tbl_HR_ExitInstance
                                                         where r.ExitInstanceID == model.ExitInstanceId
                                                         select r).SingleOrDefault();

                    if (resignDetails != null)
                    {
                        int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);

                        obj.ExitInstanceId = resignDetails.ExitInstanceID;
                        obj.EventDateTime = DateTime.Now;
                        obj.Action = "Move Ahead";
                        obj.FromStageId = OldStageId;
                        obj.ToStageId = resignDetails.stageID;
                        obj.StageActorEmployeeId = employeeID;
                        obj.QuestionnaireID = model.QuestionnaireID;
                        dbContext.Tbl_HR_ExitStageEvent.AddObject(obj);
                        dbContext.SaveChanges();

                        status = true;
                    }
                    return status;
                }
                else
                {
                    return status;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Q_Questionnaire GetLoginUserDepartment(int employeeid)
        {
            try
            {
                tbl_Q_Questionnaire EmpDetails = (from departments in dbContext.tbl_Q_Questionnaire
                                                  join approvers in dbContext.tbl_HR_ExitProcess_StageApprovers on departments.QuestionnaireID equals approvers.QuestionnaireID
                                                  where approvers.ExitInstanceID == 0 && approvers.ApproverID == employeeid
                                                  orderby departments.QuestionnaireID ascending
                                                  select departments).FirstOrDefault();

                return EmpDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ApproverList> GetDepartmentList(int ExitInstanceId)
        {
            try
            {
                List<ApproverList> ApproverDetails = new List<ApproverList>();
                ApproverDetails = (from departments in dbContext.tbl_HR_ExitProcess_StageApprovers
                                   join employee in dbContext.HRMS_tbl_PM_Employee on departments.ApproverID equals employee.EmployeeID
                                   where departments.ExitInstanceID == 0 && departments.stageID == 4 && employee.Status == false
                                   select new ApproverList
                                   {
                                       ApproverID = departments.QuestionnaireID,
                                       ApproverName = employee.EmployeeName,
                                       RevisionID = departments.RevisionID,
                                       QuestionnaireID = departments.QuestionnaireID
                                   }).ToList();

                List<ApproverList> ReportingTo = new List<ApproverList>();
                ReportingTo = (from exit in dbContext.tbl_HR_ExitInstance
                               join employee in dbContext.HRMS_tbl_PM_Employee on exit.EmployeeID equals employee.EmployeeID
                               join reporting in dbContext.HRMS_tbl_PM_Employee on employee.ReportingTo equals reporting.EmployeeID
                               where exit.ExitInstanceID == ExitInstanceId
                               select new ApproverList
                               {
                                   ApproverID = employee.ReportingTo,
                                   ApproverName = reporting.EmployeeName
                               }).ToList();

                return ApproverDetails.Union(ReportingTo).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ExitInterview> GetExitInterviewForm(int exitInstanceId, int approverId, out string hrclosurecomments)
        {
            List<ExitInterview> formDetails = new List<ExitInterview>();
            List<ExitInterview> finalObject = new List<ExitInterview>();
            tbl_PM_ChecklistResponses hrcomments = new tbl_PM_ChecklistResponses();

            try
            {
                dbContext.CommandTimeout = 240;
                var List = dbContext.usp_sel_tbl_Q_Questionnaire_Published(6, 22, false, exitInstanceId, "E", 5, approverId);

                foreach (var result in List)
                {
                    ExitInterview Exit = new ExitInterview();
                    Exit.AnswerSetId = result.AnswerSetID;
                    Exit.Comments = result.Comments;
                    Exit.FormName = result.CheckListName;
                    Exit.ItemId = result.ItemID;
                    Exit.ItemName = result.ItemName;
                    Exit.QuestionnaireCategoryId = result.QuestionnaireCategoryID;
                    Exit.QuestionnaireId = result.QuestionnaireID;
                    Exit.ResponseDate = Convert.ToDateTime(result.ResponseDate);
                    Exit.ResponseId = result.ResponseID;
                    Exit.RevisionId = result.RevisionId;
                    Exit.SectionName = result.SectionName;
                    Exit.ExitInstanceId = exitInstanceId;
                    Exit.TotalAnswer = (from a in dbContext.tbl_Q_Answer where a.AnswerSetID == result.AnswerSetID select a.AnswerID).Count();
                    formDetails.Add(Exit);
                }

                if (formDetails.Count > 0)
                {
                    foreach (var item in formDetails)
                    {
                        var options = (from o in dbContext.tbl_Q_QuestionnaireOption_Published
                                       where o.QuestionnaireQuestionID == item.ItemId && o.RevisionId == item.RevisionId
                                       select new QuestionaryOption
                                       {
                                           OptionDescription = o.OptionDescription,
                                           QuestionaryOptionId = o.QuestionnaireOptionID
                                       }).ToList();
                        item.OptionList = options;
                        finalObject.Add(item);
                    }
                    hrcomments = (from results in dbContext.tbl_PM_ChecklistResponses
                                  where results.ContextId == exitInstanceId && results.UniqueId == 5
                                  orderby results.ChecklistResponseId
                                  select results).FirstOrDefault();
                }
                if (hrcomments != null)
                {
                    hrclosurecomments = hrcomments.HRClosureComments;
                }
                else
                {
                    hrclosurecomments = "";
                }
                return finalObject;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveExitInterviewFormData(List<ExitInterview> model, int employeeId)
        {
            try
            {
                bool status = false;
                ExitInterview obj = model.FirstOrDefault();
                var exit = (from e in dbContext.tbl_PM_ChecklistResponses
                            where e.ContextId == obj.ExitInstanceId && e.UniqueId == 5
                            select e).ToList();

                if (exit.Count > 0)
                {
                    foreach (var item in model)
                    {
                        var tempExit = (from e in dbContext.tbl_PM_ChecklistResponses
                                        where e.ContextId == item.ExitInstanceId && e.CheckListItemId == item.ItemId
                                        select e).SingleOrDefault();

                        if (tempExit != null)
                        {
                            tempExit.CheckListItemId = item.ItemId;
                            tempExit.ResponseId = item.ResponseId;
                            tempExit.Comments = item.Comments;
                            tempExit.HRClosureComments = item.HRClosureComments;
                            tempExit.ResponseDate = DateTime.Now;
                            tempExit.ApproverID = employeeId;
                            dbContext.SaveChanges();
                        }
                    }

                    status = true;
                }
                else
                {
                    foreach (var item in model)
                    {
                        tbl_PM_ChecklistResponses response = new tbl_PM_ChecklistResponses();

                        response.ContextId = item.ExitInstanceId;
                        response.ContextType = "E";
                        response.CheckListItemId = item.ItemId;
                        response.ResponseId = item.ResponseId;
                        response.Comments = item.Comments;
                        response.UniqueId = 5;
                        response.ResponseDate = DateTime.Now;
                        response.RevisionId = item.RevisionId;
                        response.ApproverID = employeeId;
                        response.CheckListID = 6;
                        response.HRClosureComments = item.HRClosureComments;
                        dbContext.tbl_PM_ChecklistResponses.AddObject(response);
                    }

                    dbContext.SaveChanges();
                    status = true;
                }

                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ApproveExitInterViewFormData(List<ExitInterview> model)
        {
            try
            {
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                ExitInterview exit = model.FirstOrDefault();
                bool datasaved = SaveExitInterviewFormData(model, employeeID);
                int? OldStageId = 0;
                bool status = false;

                if (datasaved == true)
                {
                    tbl_HR_ExitInstance exi = (from e in dbContext.tbl_HR_ExitInstance
                                               where e.ExitInstanceID == exit.ExitInstanceId
                                               select e).SingleOrDefault();

                    if (exi != null)
                    {
                        OldStageId = exi.stageID;

                        if (exi.stageID == (from a in dbContext.tbl_HR_ExitStage where a.Description == "Line Manager Approval" select a.ExitStageID).SingleOrDefault())
                            exi.stageID = (from a in dbContext.tbl_HR_ExitStage where a.Description == "RMG Approval Stage" select a.ExitStageID).SingleOrDefault();
                        else
                        {
                            if (exi.stageID == (from a in dbContext.tbl_HR_ExitStage where a.Description == "RMG Approval Stage" select a.ExitStageID).SingleOrDefault())
                                exi.stageID = (from a in dbContext.tbl_HR_ExitStage where a.Description == "HR Approval Stage" select a.ExitStageID).SingleOrDefault();
                            else
                            {
                                if (exi.stageID == (from a in dbContext.tbl_HR_ExitStage where a.Description == "Exit" select a.ExitStageID).SingleOrDefault())
                                    exi.stageID = exi.stageID;
                                else
                                {
                                    exi.stageID = exi.stageID + 1;
                                }
                            }
                        }
                    }
                    dbContext.SaveChanges();

                    Tbl_HR_ExitStageEvent obj = new Tbl_HR_ExitStageEvent();
                    tbl_HR_ExitInstance resignDetails = (from r in dbContext.tbl_HR_ExitInstance
                                                         where r.ExitInstanceID == exit.ExitInstanceId
                                                         select r).SingleOrDefault();

                    if (resignDetails != null)
                    {
                        obj.ExitInstanceId = resignDetails.ExitInstanceID;
                        obj.EventDateTime = DateTime.Now;
                        obj.Action = "Approve";
                        obj.FromStageId = OldStageId;
                        obj.ToStageId = resignDetails.stageID;
                        obj.StageActorEmployeeId = employeeID;

                        dbContext.Tbl_HR_ExitStageEvent.AddObject(obj);
                        dbContext.SaveChanges();
                        if (resignDetails.stageID != 7)
                        {
                            status = true;
                        }
                    }
                    //added by nikhil for hr closure
                    if (resignDetails.stageID == 7)
                    {
                        HRMS_tbl_PM_Employee empdetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == resignDetails.EmployeeID).FirstOrDefault();
                        if (empdetails != null)
                        {
                            empdetails.LeavingDate = exi.AgreedReleaseDate;
                            dbContext.SaveChanges();
                            bool MemberInactive = true;
                            aspnet_Membership _Membership = (from m in dbv2toolsContext.aspnet_Users
                                                             join roleID in dbv2toolsContext.aspnet_Membership on m.UserId equals roleID.UserId
                                                             where m.UserName == empdetails.EmployeeCode
                                                             select roleID).FirstOrDefault();
                            _Membership.IsLockedOut = MemberInactive;
                            dbv2toolsContext.SaveChanges();
                            status = true;
                        };
                    }
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool PushBackHRClosure(int exitInstanceId)
        {
            try
            {
                bool status = false;
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                tbl_HR_ExitInstance exitinfo = (from e in dbContext.tbl_HR_ExitInstance
                                                where e.ExitInstanceID == exitInstanceId
                                                select e).SingleOrDefault();

                if (exitinfo != null)
                {
                    exitinfo.CreatedDate = DateTime.Now;
                    exitinfo.stageID = exitinfo.stageID - 1;
                    dbContext.SaveChanges();
                }
                Tbl_HR_ExitStageEvent obj = new Tbl_HR_ExitStageEvent();
                tbl_HR_ExitInstance resignDetails = (from r in dbContext.tbl_HR_ExitInstance
                                                     where r.ExitInstanceID == exitinfo.ExitInstanceID
                                                     select r).SingleOrDefault();
                if (resignDetails != null)
                {
                    obj.EventDateTime = DateTime.Now;
                    obj.Action = "Push Back";
                    obj.FromStageId = resignDetails.stageID;
                    obj.ToStageId = resignDetails.stageID - 1;
                    obj.StageActorEmployeeId = employeeID;
                    dbContext.SaveChanges();
                    status = true;
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmpSeperationTerminationViewModel> SearchEmployeeForTerminationLoadGrid(string searchText, string OrganizationUnit, int page, int rows, out int totalCount)
        {
            List<EmpSeperationTerminationViewModel> ExitemployeeDetails = new List<EmpSeperationTerminationViewModel>();
            List<EmpSeperationTerminationViewModel> employeeDetails = new List<EmpSeperationTerminationViewModel>();
            List<EmpSeperationTerminationViewModel> result = new List<EmpSeperationTerminationViewModel>();
            int organization = 0;
            if (OrganizationUnit != "")
            {
                organization = Convert.ToInt32(OrganizationUnit);
            }
            try
            {
                if (OrganizationUnit == "")
                {
                    ExitemployeeDetails = (from employee in dbContext.HRMS_tbl_PM_Employee
                                           join buisnessgrp in dbContext.tbl_CNF_BusinessGroups on employee.BusinessGroupID equals buisnessgrp.BusinessGroupID into BIgrp
                                           from BuisnessGrp in BIgrp.DefaultIfEmpty()
                                           join reportingMgr in dbContext.HRMS_tbl_PM_Employee on employee.CostCenterID equals reportingMgr.EmployeeID into Rm
                                           from reportingManager in Rm.DefaultIfEmpty()
                                           join organizationUnit in dbContext.tbl_PM_Location on employee.LocationID equals organizationUnit.LocationID into OU
                                           from Organization in OU.DefaultIfEmpty()
                                           where (employee.EmployeeName.Contains(searchText) || employee.EmployeeCode.Contains(searchText)) && (employee.EmployeeStatusID == 13 || employee.EmployeeStatusMasterID == 2)
                                           orderby employee.EmployeeName
                                           select new EmpSeperationTerminationViewModel
                                           {
                                               EmployeeCode = employee.EmployeeCode,
                                               EmployeeId = employee.EmployeeID,
                                               EmployeeName = employee.EmployeeName,
                                               JoiningDate = employee.JoiningDate,
                                               BusinessGroup = BuisnessGrp.BusinessGroup,
                                               OrganizationGroup = Organization.Location,
                                               IsExit = "yes",
                                               ReportingManager = reportingManager.EmployeeName
                                           }).ToList();

                    employeeDetails = (from employee in dbContext.HRMS_tbl_PM_Employee
                                       join buisnessgrp in dbContext.tbl_CNF_BusinessGroups on employee.BusinessGroupID equals buisnessgrp.BusinessGroupID into BIgrp
                                       from BuisnessGrp in BIgrp.DefaultIfEmpty()
                                       join reportingMgr in dbContext.HRMS_tbl_PM_Employee on employee.CostCenterID equals reportingMgr.EmployeeID into Rm
                                       from reportingManager in Rm.DefaultIfEmpty()
                                       join organizationUnit in dbContext.tbl_PM_Location on employee.LocationID equals organizationUnit.LocationID into OU
                                       from Organization in OU.DefaultIfEmpty()
                                       where (employee.EmployeeName.Contains(searchText) || employee.EmployeeCode.Contains(searchText)) && (employee.EmployeeStatusMasterID == 1)
                                       orderby employee.EmployeeName
                                       select new EmpSeperationTerminationViewModel
                                       {
                                           EmployeeCode = employee.EmployeeCode,
                                           EmployeeId = employee.EmployeeID,
                                           EmployeeName = employee.EmployeeName,
                                           JoiningDate = employee.JoiningDate,
                                           BusinessGroup = BuisnessGrp.BusinessGroup,
                                           OrganizationGroup = Organization.Location,
                                           IsExit = "no",
                                           ReportingManager = reportingManager.EmployeeName
                                       }).ToList();
                }
                else
                {
                    ExitemployeeDetails = (from employee in dbContext.HRMS_tbl_PM_Employee
                                           join buisnessgrp in dbContext.tbl_CNF_BusinessGroups on employee.BusinessGroupID equals buisnessgrp.BusinessGroupID into BIgrp
                                           from BuisnessGrp in BIgrp.DefaultIfEmpty()
                                           join reportingMgr in dbContext.HRMS_tbl_PM_Employee on employee.CostCenterID equals reportingMgr.EmployeeID into Rm
                                           from reportingManager in Rm.DefaultIfEmpty()
                                           join organizationUnit in dbContext.tbl_PM_Location on employee.LocationID equals organizationUnit.LocationID into OU
                                           from Organization in OU.DefaultIfEmpty()
                                           where (employee.EmployeeName.Contains(searchText) || employee.EmployeeCode.Contains(searchText)) && (employee.EmployeeStatusID == 13 || employee.EmployeeStatusMasterID == 2) && (employee.LocationID == organization)
                                           orderby employee.EmployeeName
                                           select new EmpSeperationTerminationViewModel
                                           {
                                               EmployeeCode = employee.EmployeeCode,
                                               EmployeeId = employee.EmployeeID,
                                               EmployeeName = employee.EmployeeName,
                                               JoiningDate = employee.JoiningDate,
                                               BusinessGroup = BuisnessGrp.BusinessGroup,
                                               OrganizationGroup = Organization.Location,
                                               IsExit = "yes",
                                               ReportingManager = reportingManager.EmployeeName
                                           }).ToList();

                    employeeDetails = (from employee in dbContext.HRMS_tbl_PM_Employee
                                       join buisnessgrp in dbContext.tbl_CNF_BusinessGroups on employee.BusinessGroupID equals buisnessgrp.BusinessGroupID into BIgrp
                                       from BuisnessGrp in BIgrp.DefaultIfEmpty()
                                       join reportingMgr in dbContext.HRMS_tbl_PM_Employee on employee.CostCenterID equals reportingMgr.EmployeeID into Rm
                                       from reportingManager in Rm.DefaultIfEmpty()
                                       join organizationUnit in dbContext.tbl_PM_Location on employee.LocationID equals organizationUnit.LocationID into OU
                                       from Organization in OU.DefaultIfEmpty()
                                       where (employee.EmployeeName.Contains(searchText) || employee.EmployeeCode.Contains(searchText)) && (employee.EmployeeStatusMasterID == 1) && (employee.LocationID == organization)
                                       orderby employee.EmployeeName
                                       select new EmpSeperationTerminationViewModel
                                       {
                                           EmployeeCode = employee.EmployeeCode,
                                           EmployeeId = employee.EmployeeID,
                                           EmployeeName = employee.EmployeeName,
                                           JoiningDate = employee.JoiningDate,
                                           BusinessGroup = BuisnessGrp.BusinessGroup,
                                           OrganizationGroup = Organization.Location,
                                           IsExit = "no",
                                           ReportingManager = reportingManager.EmployeeName
                                       }).ToList();
                }

                result = employeeDetails.Union(ExitemployeeDetails).ToList();
                totalCount = result.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result.Skip((page - 1) * rows).Take(rows).ToList();
        }

        public List<ModeOfSeperation> GetModeOfSeperationList()
        {
            List<ModeOfSeperation> locationlist = new List<ModeOfSeperation>();
            try
            {
                locationlist = (from mode in dbContext.Tbl_HR_ExitModeOfSeparation
                                select new ModeOfSeperation
                                {
                                    SeperationId = mode.Id,
                                    SeperationName = mode.ModeOfSeparation
                                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return locationlist.OrderBy(x => x.SeperationName).ToList();
        }
    }
}