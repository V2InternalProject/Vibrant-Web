using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace HRMS.DAL
{
    public class ConfigurationDAL
    {
        private HRMSDBEntities dbContext = new HRMSDBEntities();
        private WSEMDBEntities WSEMdbContext = new WSEMDBEntities();

        public List<BusinessGroup> getBusinessGroups(int BusinessGroupID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<BusinessGroup> businessGroups = new List<BusinessGroup>();
                if (BusinessGroupID != 0)
                {
                    businessGroups = (from b in dbContext.tbl_CNF_BusinessGroups
                                      where b.Active == true && b.BusinessGroupID == BusinessGroupID
                                      select new BusinessGroup
                                      {
                                          BusinessGroupID = b.BusinessGroupID,
                                          businessgroup = b.BusinessGroup,
                                          BusinessGroupCode = b.BusinessGroupCode,
                                          ModifiedBy = b.ModifiedBy,
                                          ModifiedDate = b.ModifiedDate,
                                          Active = b.Active.HasValue ? b.Active.Value : false,
                                          LastSequence = b.LastSequence,
                                          Checked = false
                                      }).ToList();
                }
                else
                {
                    businessGroups = (from businessGroup in dbContext.tbl_CNF_BusinessGroups
                                      orderby businessGroup.BusinessGroup ascending
                                      where businessGroup.Active == true
                                      select new BusinessGroup
                                      {
                                          BusinessGroupID = businessGroup.BusinessGroupID,
                                          businessgroup = businessGroup.BusinessGroup,
                                          BusinessGroupCode = businessGroup.BusinessGroupCode,
                                          ModifiedBy = businessGroup.ModifiedBy,
                                          ModifiedDate = businessGroup.ModifiedDate,
                                          Active = businessGroup.Active.HasValue ? businessGroup.Active.Value : false,
                                          LastSequence = businessGroup.LastSequence,
                                          Checked = false
                                      }).ToList();
                }
                return businessGroups;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_CNF_BusinessGroups getBusinessGroupDetails(int BusinessGroupID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_CNF_BusinessGroups BusinessGroup = dbContext.tbl_CNF_BusinessGroups.Where(x => x.BusinessGroupID == BusinessGroupID).FirstOrDefault();
                return BusinessGroup;
            }
            catch
            {
                throw;
            }
        }

        public List<OrganizationUnit> getOrganizationUnit(int BusinessGroupID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<string> empty = new List<string>();
                List<OrganizationUnit> OrganizationUnits = new List<OrganizationUnit>();
                OrganizationUnits = (from PM_Location in dbContext.tbl_PM_Location
                                     join CNF_BusinessGroup in dbContext.tbl_CNF_BusinessGroup_OUPools on PM_Location.LocationID equals CNF_BusinessGroup.OUPoolID
                                     orderby PM_Location.Location ascending
                                     where PM_Location.Active == true && CNF_BusinessGroup.BusinessGroupID == BusinessGroupID
                                     select new OrganizationUnit
                                     {
                                         LocationID = PM_Location.LocationID,
                                         Location = PM_Location.Location,
                                         LocationCode = PM_Location.LocationCode,
                                         CreatedDate = PM_Location.CreatedDate,
                                         ModifiedBy = PM_Location.ModifiedBy,
                                         ModifiedDate = PM_Location.ModifiedDate,
                                         Active = PM_Location.Active.HasValue ? PM_Location.Active.Value : false,
                                         UniqueID = CNF_BusinessGroup.UniqueID,
                                         BusinessGroupID = CNF_BusinessGroup.BusinessGroupID,
                                         OUPoolID = CNF_BusinessGroup.OUPoolID,
                                         Checked = false,
                                         EmployeeName = empty
                                     }).ToList();
                return OrganizationUnits;
            }
            catch
            {
                throw;
            }
        }

        public List<DeliveryUnit> getDeliveryUnit(int BusinessGroupID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<string> empty = new List<string>();
                List<DeliveryUnit> deliveryUnits = new List<DeliveryUnit>();
                deliveryUnits = (from bu in dbContext.tbl_CNF_BusinessGroups
                                 join ou in dbContext.tbl_CNF_BusinessGroup_OUPools on bu.BusinessGroupID equals ou.BusinessGroupID
                                 join l in dbContext.tbl_PM_Location on ou.OUPoolID equals l.LocationID
                                 join du in dbContext.tbl_PM_OUPool_ResourcePools on ou.OUPoolID equals du.OUPoolID
                                 join r in dbContext.HRMS_tbl_PM_ResourcePool on du.ResourcePoolID equals r.ResourcePoolID
                                 orderby r.ResourcePoolName ascending
                                 where bu.BusinessGroupID == BusinessGroupID && r.Active == true
                                 select new DeliveryUnit
                                 {
                                     LocationID = l.LocationID,
                                     Location = l.Location,
                                     ResourcePoolID = r.ResourcePoolID,
                                     ResourcePoolCode = r.ResourcePoolCode,
                                     ResourcePoolName = r.ResourcePoolName,
                                     CreatedBy = r.CreatedBy,
                                     CreatedDate = r.CreatedDate,
                                     ModifiedBy = r.ModifiedBy,
                                     ModifiedDate = r.ModifiedDate,
                                     Active = r.Active.HasValue ? r.Active.Value : false,
                                     UniqueID = du.UniqueID,
                                     OUPoolID = du.OUPoolID,
                                     BusinessGroupID = BusinessGroupID,
                                     EmployeeName = empty
                                 }).ToList();
                return deliveryUnits;
            }
            catch
            {
                throw;
            }
        }

        public List<DeliveryTeam> getDeliveryTeam(int BusinessGroupID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<DeliveryTeam> deliveryTeams = new List<DeliveryTeam>();
                deliveryTeams = (from bu in dbContext.tbl_CNF_BusinessGroups
                                 join ou in dbContext.tbl_CNF_BusinessGroup_OUPools on bu.BusinessGroupID equals ou.BusinessGroupID
                                 join du in dbContext.tbl_PM_OUPool_ResourcePools on ou.OUPoolID equals du.OUPoolID
                                 join r in dbContext.HRMS_tbl_PM_ResourcePool on du.ResourcePoolID equals r.ResourcePoolID
                                 join t in dbContext.tbl_PM_ResourcePool_Teams on r.ResourcePoolID equals t.ResourcePoolID
                                 join g in dbContext.tbl_PM_GroupMaster on t.GroupID equals g.GroupID
                                 join e in dbContext.HRMS_tbl_PM_Employee on g.ResourceHeadID equals e.EmployeeID into xy
                                 from x in xy.DefaultIfEmpty()
                                 orderby g.GroupName ascending
                                 where g.Active == true && bu.BusinessGroupID == BusinessGroupID
                                 select new DeliveryTeam
                                 {
                                     GroupName = g.GroupName,
                                     GroupID = g.GroupID,
                                     ResourcePoolID = t.ResourcePoolID,
                                     ResourcePoolName = r.ResourcePoolName,
                                     ResourceHeadID = g.ResourceHeadID,
                                     EmployeeName = x.EmployeeName,
                                     EmployeeID = g.ResourceHeadID.HasValue ? g.ResourceHeadID.Value : 0,
                                     BusinessGroupID = BusinessGroupID
                                 }).ToList();

                HRMS_tbl_PM_Employee _employee = new HRMS_tbl_PM_Employee();
                foreach (var item in deliveryTeams)
                {
                    _employee = dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeID == item.EmployeeID).FirstOrDefault();
                    if (_employee != null)
                    {
                        if (_employee.Status == false)
                            item.EmployeeName = item.EmployeeName;
                        else
                            item.EmployeeName = null;
                    }
                }
                return deliveryTeams;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BusinessGroup> getManagersListForBusinessGroup(List<BusinessGroup> businessGroup)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ManagerList> managerlist = new List<ManagerList>();
                foreach (BusinessGroup item in businessGroup)
                {
                    managerlist = (from m in dbContext.tbl_CNF_BusinessGroup_Managers
                                   join b in dbContext.tbl_CNF_BusinessGroups on m.BusinessGroupID equals b.BusinessGroupID
                                   join e in dbContext.HRMS_tbl_PM_Employee on m.ManagerID equals e.EmployeeID
                                   where b.Active == true && e.Status == false && m.BusinessGroupID == item.BusinessGroupID
                                   select new ManagerList
                                   {
                                       UserName = e.UserName,
                                       EmployeeID = e.EmployeeID,
                                       EmployeeName = e.EmployeeName,
                                       BusinessGroupID = m.BusinessGroupID.HasValue ? m.BusinessGroupID.Value : 0,
                                       IsPrimaryResponsible = m.IsPrimaryResponsible
                                   }).ToList();
                    for (int i = 0; i < managerlist.Count; i++)
                    {
                        item.EmployeeList.Add(managerlist[i]);
                    }
                }
                return businessGroup;
            }
            catch
            {
                throw;
            }
        }

        public List<OrganizationUnit> getManagersForOrganizationUnit(List<OrganizationUnit> organizationUnit)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<string> managerlist = new List<string>();
                foreach (OrganizationUnit item in organizationUnit)
                {
                    managerlist = (from poolmanagers in dbContext.tbl_PM_OUPool_Managers
                                   join l in dbContext.tbl_PM_Location on poolmanagers.OUPoolID equals l.LocationID
                                   join e in dbContext.HRMS_tbl_PM_Employee on poolmanagers.ManagerID equals e.EmployeeID
                                   where l.Active == true && e.Status == false && l.LocationID == item.LocationID
                                   select e.EmployeeName).ToList();
                    for (int i = 0; i < managerlist.Count; i++)
                    {
                        item.EmployeeName.Add(managerlist[i]);
                    }
                }
                return organizationUnit;
            }
            catch
            {
                throw;
            }
        }

        public List<DeliveryUnit> getManagersForDeliveryUnit(List<DeliveryUnit> deliveryUnit)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<string> ManagerList = new List<string>();
                foreach (DeliveryUnit item in deliveryUnit)
                {
                    ManagerList = (from poolmanagers in dbContext.tbl_PM_ResourcePool_Managers
                                   join resourcepool in dbContext.HRMS_tbl_PM_ResourcePool on poolmanagers.ResourcePoolID equals resourcepool.ResourcePoolID
                                   join e in dbContext.HRMS_tbl_PM_Employee on poolmanagers.ManagerID equals e.EmployeeID
                                   where resourcepool.Active == true && e.Status == false && poolmanagers.ResourcePoolID == item.ResourcePoolID
                                   select e.EmployeeName).ToList();
                    for (int i = 0; i < ManagerList.Count; i++)
                    {
                        item.EmployeeName.Add(ManagerList[i]);
                    }
                }
                return deliveryUnit;
            }
            catch
            {
                throw;
            }
        }

        public List<MiddleLevelResources> getMiddleLevelResources(int BusinessGroupID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<MiddleLevelResources> middleLevelResources = (from b in dbContext.tbl_CNF_BusinessGroups_MiddleLevelResources
                                                                   join e in dbContext.HRMS_tbl_PM_Employee on b.EmployeeID equals e.EmployeeID
                                                                   join r in dbContext.HRMS_tbl_PM_Role on e.PostID equals r.RoleID into xy
                                                                   from x in xy.DefaultIfEmpty()
                                                                   where b.BusinessGroupID == BusinessGroupID
                                                                   select new MiddleLevelResources
                                                                   {
                                                                       EmpoloyeeID = e.EmployeeID,
                                                                       EmployeeName = e.EmployeeName,
                                                                       Role = x.RoleDescription,
                                                                       EmailID = e.EmailID,
                                                                       BusinessGroupID = BusinessGroupID,
                                                                       Checked = false
                                                                   }).ToList();
                return middleLevelResources;
            }
            catch
            {
                throw;
            }
        }

        public List<tbl_Appraisal_YearMaster> GetYearList()
        {
            var yearList = dbContext.tbl_Appraisal_YearMaster.ToList();
            return yearList.OrderByDescending(v => v.AppraisalYear).ToList();
        }

        public string FreezeAppraisalYear(int YearID, DateTime froozenDate, int EmployeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                var AppraisalYearDetails = (from y in dbContext.tbl_Appraisal_YearMaster
                                            where y.AppraisalYearID == YearID
                                            select y).FirstOrDefault();
                if (AppraisalYearDetails.AppraisalYearFrozenOn != null && AppraisalYearDetails.AppraisalYearFrozenBy != null)
                    if (AppraisalYearDetails.AppraisalYearFrozenOn.Value.Date < DateTime.Today)
                    {
                        return "Cannot Change";
                    }
                if (AppraisalYearDetails.IDFInitiatedBy != null && AppraisalYearDetails.IDFInitiatedOn != null)
                {
                    if (AppraisalYearDetails.IDFInitiatedOn.Value.Date <= froozenDate)
                    {
                        return "Greater Date";
                    }
                }
                if (AppraisalYearDetails != null)
                {
                    AppraisalYearDetails.AppraisalYearFrozenOn = froozenDate;
                    AppraisalYearDetails.AppraisalYearFrozenBy = EmployeeId;
                    dbContext.SaveChanges();
                    return "true";
                }

                return "false";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AppraisalProcessResponse InitiateIndividualDevelopment(int YearID, DateTime initiateDate, int EmployeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                AppraisalProcessResponse response = new AppraisalProcessResponse();
                response.isAdded = false;
                response.InitiateIDF_LessThan_FreezePerformanceAppraisal = false;
                response.InitiateIDF_GreaterThan_FreezeIDF = false;

                var AppraisalYearDetails = (from y in dbContext.tbl_Appraisal_YearMaster
                                            where y.AppraisalYearID == YearID
                                            select y).FirstOrDefault();
                if (AppraisalYearDetails.AppraisalYearFrozenOn != null && AppraisalYearDetails.AppraisalYearFrozenBy != null)
                    if (AppraisalYearDetails.AppraisalYearFrozenOn.Value.Date >= initiateDate)
                    {
                        response.InitiateIDF_LessThan_FreezePerformanceAppraisal = true;
                        return response;
                    }

                if (AppraisalYearDetails.IDFFrozenOn != null && AppraisalYearDetails.IDFFrozenBy != null)
                {
                    if (AppraisalYearDetails.IDFFrozenOn.Value.Date <= initiateDate)
                    {
                        response.InitiateIDF_GreaterThan_FreezeIDF = true;
                        return response;
                    }
                }
                if (AppraisalYearDetails != null)
                {
                    AppraisalYearDetails.IDFInitiatedOn = initiateDate;
                    AppraisalYearDetails.IDFInitiatedBy = EmployeeId;
                    tbl_Appraisal_SrengthImprovement_Limit limit = dbContext.tbl_Appraisal_SrengthImprovement_Limit.Where(ed => ed.AppraisalYearID == YearID).FirstOrDefault();
                    List<AppraisalMasterDetails> appraisalMaster = (from ratingComments in dbContext.tbl_Appraisal_RatingComments
                                                                    join master in dbContext.tbl_Appraisal_AppraisalMaster on ratingComments.AppraisalID equals master.AppraisalID
                                                                    join param in dbContext.tbl_Appraisal_ParameterMaster on ratingComments.ParameterID equals param.ParameterID
                                                                    where master.AppraisalYearID == YearID
                                                                    select new AppraisalMasterDetails
                                                                    {
                                                                        AppraisalId = master.AppraisalID,
                                                                        EmployeeId = master.EmployeeID,
                                                                        AppraisalYearId = master.AppraisalYearID,
                                                                        ReviewerRating = ratingComments.Reviewer1Ratings,
                                                                        ParameterName = param.Parameter
                                                                    }).ToList();
                    foreach (var item in appraisalMaster)
                    {
                        if (item.ReviewerRating >= limit.StrengthLimit)
                        {
                            tbl_Appraisal_IDFAppraiserAddStrength strength = new tbl_Appraisal_IDFAppraiserAddStrength();
                            strength.AppraisalID = item.AppraisalId;
                            strength.EmployeeID = item.EmployeeId;
                            strength.AppraisalYearID = item.AppraisalYearId;
                            strength.IDFAppraiserStrength = item.ParameterName;
                            strength.CreatedOn = DateTime.Now;
                            dbContext.tbl_Appraisal_IDFAppraiserAddStrength.AddObject(strength);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            tbl_Appraisal_IDFAppraiserAddImprovement improvement = new tbl_Appraisal_IDFAppraiserAddImprovement();
                            improvement.AppraisalID = item.AppraisalId;
                            improvement.EmployeeID = item.EmployeeId;
                            improvement.AppraisalYearID = item.AppraisalYearId;
                            improvement.IDFAppraiserImprovement = item.ParameterName;
                            improvement.CreatedOn = DateTime.Now;
                            dbContext.tbl_Appraisal_IDFAppraiserAddImprovement.AddObject(improvement);
                            dbContext.SaveChanges();
                        }
                    }
                    dbContext.SaveChanges();
                    response.isAdded = true;
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string FreezeIndividualDevelopment(int YearID, DateTime froozenDate, int EmployeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                var AppraisalYearDetails = (from y in dbContext.tbl_Appraisal_YearMaster
                                            where y.AppraisalYearID == YearID
                                            select y).FirstOrDefault();
                if (AppraisalYearDetails.IDFInitiatedOn != null && AppraisalYearDetails.IDFInitiatedBy != null)
                {
                    if (AppraisalYearDetails.IDFInitiatedOn.Value.Date >= froozenDate)
                    {
                        return "IDFInitiate Date Greater";
                    }
                }
                if (AppraisalYearDetails.IDFInitiatedOn == null && AppraisalYearDetails.IDFInitiatedBy == null)
                {
                    return "Initiate IDF first";
                }
                if (AppraisalYearDetails.AppraisalYearFrozenOn != null && AppraisalYearDetails.AppraisalYearFrozenBy != null)
                {
                    if (AppraisalYearDetails.AppraisalYearFrozenOn.Value.Date >= froozenDate)
                        return "Appraisal FreezeDate Greater";
                }
                if (AppraisalYearDetails != null)
                {
                    AppraisalYearDetails.IDFFrozenOn = froozenDate;
                    AppraisalYearDetails.IDFFrozenBy = EmployeeId;
                    dbContext.SaveChanges();
                    return "true";
                }

                return "false";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<InitiateIndividualDevelopmentStage> GetInitiateIndividualDevelopList(int page, int rows, out int totalCount)
        {
            List<InitiateIndividualDevelopmentStage> finalObj = new List<InitiateIndividualDevelopmentStage>();
            try
            {
                dbContext = new HRMSDBEntities();
                List<InitiateIndividualDevelopmentStage> AllDetails = (from appr in dbContext.tbl_Appraisal_YearMaster
                                                                       join e in dbContext.HRMS_tbl_PM_Employee on appr.IDFInitiatedBy equals e.EmployeeID into emp
                                                                       from empList in emp.DefaultIfEmpty()
                                                                       where appr.IDFInitiatedBy != null &&
                                                                       appr.IDFInitiatedOn != null
                                                                       select new InitiateIndividualDevelopmentStage
                                                                       {
                                                                           AppraisalInitiatedBy = empList.EmployeeName,
                                                                           InitiatedOn = appr.IDFInitiatedOn,
                                                                           InitiatedYear = appr.AppraisalYear
                                                                       }
                                                         ).ToList();
                totalCount = AllDetails.Count;
                return AllDetails.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<FreezeIndividualDevelopmentStage> GetFreezeIndividualDevelopmentStageList(int page, int rows, out int totalCount)
        {
            List<FreezeIndividualDevelopmentStage> finalObj = new List<FreezeIndividualDevelopmentStage>();
            try
            {
                dbContext = new HRMSDBEntities();
                List<FreezeIndividualDevelopmentStage> AllDetails = (from appr in dbContext.tbl_Appraisal_YearMaster
                                                                     join e in dbContext.HRMS_tbl_PM_Employee on appr.IDFFrozenBy equals e.EmployeeID into emp
                                                                     from empList in emp.DefaultIfEmpty()
                                                                     where appr.IDFFrozenBy != null &&
                                                                     appr.IDFFrozenOn != null
                                                                     select new FreezeIndividualDevelopmentStage
                                                                    {
                                                                        FrozenBy = empList.EmployeeName,
                                                                        FrozenOn = appr.IDFFrozenOn,
                                                                        FrozenYear = appr.AppraisalYear
                                                                    }
                                                         ).ToList();
                totalCount = AllDetails.Count;
                return AllDetails.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<FreezeAppraisalPeriod> GetFreezeAppraisalPeriodList(int page, int rows, out int totalCount)
        {
            List<FreezeAppraisalPeriod> finalObj = new List<FreezeAppraisalPeriod>();
            try
            {
                dbContext = new HRMSDBEntities();
                List<FreezeAppraisalPeriod> AllDetails = (from appr in dbContext.tbl_Appraisal_YearMaster
                                                          join e in dbContext.HRMS_tbl_PM_Employee on appr.AppraisalYearFrozenBy equals e.EmployeeID into emp
                                                          from empList in emp.DefaultIfEmpty()
                                                          where appr.AppraisalYearFrozenBy != null &&
                                                          appr.AppraisalYearFrozenOn != null
                                                          select new FreezeAppraisalPeriod
                                                          {
                                                              AppraisalYearDesc = appr.AppraisalYear,
                                                              AppraisalYearFroozenByEmpName = empList.EmployeeName,
                                                              AppraisalYearFroozenOn = appr.AppraisalYearFrozenOn
                                                          }
                                                         ).ToList();
                totalCount = AllDetails.Count;
                return AllDetails.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IndividualDevelopmentStatusReport> GetIndividualDevelopmentStatusReportForTheYear(int YearID)
        {
            dbContext = new HRMSDBEntities();
            List<IndividualDevelopmentStatusReport> developmentList = (from a in dbContext.tbl_Appraisal_AppraisalMaster
                                                                       join e in dbContext.HRMS_tbl_PM_Employee on a.EmployeeID equals e.EmployeeID into emp
                                                                       from empList in emp.DefaultIfEmpty()
                                                                       join a1 in dbContext.HRMS_tbl_PM_Employee on a.Appraiser1 equals a1.EmployeeID into appraiser1
                                                                       from appraiser1List in appraiser1.DefaultIfEmpty()
                                                                       join s in dbContext.tbl_Appraisal_Stages on a.AppraisalStageID equals s.AppraisalStageID into stage
                                                                       from stageList in stage.DefaultIfEmpty()
                                                                       join d in dbContext.tbl_PM_DesignationMaster on empList.DesignationID equals d.DesignationID into designation
                                                                       from designationList in designation.DefaultIfEmpty()
                                                                       join g in dbContext.tbl_PM_GroupMaster on empList.GroupID equals g.GroupID into empGroup
                                                                       from groupList in empGroup.DefaultIfEmpty()
                                                                       where a.AppraisalYearID == YearID
                                                                       // && ((a.AppraisalStageID == 7) || (a.AppraisalStageID == 8))
                                                                       select new IndividualDevelopmentStatusReport
                                                                       {
                                                                           ConfirmationDate = empList.ConfirmationDate,
                                                                           DeliveryTeam = groupList.GroupName,
                                                                           Designation = designationList.DesignationName,
                                                                           EmployeeCode = empList.EmployeeCode,
                                                                           EmployeeName = empList.EmployeeName,
                                                                           Status = stageList.AppraisalStage,
                                                                           ApraiseeComment = a.IDFAprraiseComment,
                                                                           Appraiseeagreedisagree = a.IDFISAppraiseAgree == true ? "Yes" : a.IDFISAppraiseAgree == false ? "No" : "",
                                                                           EmployeeEmail = empList.EmailID,
                                                                           Appraiser1Email = appraiser1List.EmailID
                                                                       }).ToList();
            return developmentList.ToList();
        }

        public List<AppraisalStatusReportViewModel> GetAppraisalReportForTheYear(int YearID)
        {
            dbContext = new HRMSDBEntities();
            List<AppraisalStatusReportViewModel> appraisalReportList = (from a in dbContext.tbl_Appraisal_AppraisalMaster
                                                                        join e in dbContext.HRMS_tbl_PM_Employee on a.EmployeeID equals e.EmployeeID into emp
                                                                        from empList in emp.DefaultIfEmpty()
                                                                        join a1 in dbContext.HRMS_tbl_PM_Employee on a.Appraiser1 equals a1.EmployeeID into appraiser1
                                                                        from appraiser1List in appraiser1.DefaultIfEmpty()
                                                                        join a2 in dbContext.HRMS_tbl_PM_Employee on a.Appraiser2 equals a2.EmployeeID into appraiser2
                                                                        from appraiser2List in appraiser2.DefaultIfEmpty()
                                                                        join r1 in dbContext.HRMS_tbl_PM_Employee on a.Reviewer1 equals r1.EmployeeID into reviewer1
                                                                        from reviewer1List in reviewer1.DefaultIfEmpty()
                                                                        join r2 in dbContext.HRMS_tbl_PM_Employee on a.Reviewer2 equals r2.EmployeeID into reviewer2
                                                                        from reviewer2List in reviewer2.DefaultIfEmpty()
                                                                        join gh in dbContext.HRMS_tbl_PM_Employee on a.GroupHead equals gh.EmployeeID into grouphead
                                                                        from grouplist in grouphead.DefaultIfEmpty()
                                                                        join s in dbContext.tbl_Appraisal_Stages on a.AppraisalStageID equals s.AppraisalStageID into stage
                                                                        from stageList in stage.DefaultIfEmpty()
                                                                        join d in dbContext.tbl_PM_DesignationMaster on empList.DesignationID equals d.DesignationID into designation
                                                                        from designationList in designation.DefaultIfEmpty()
                                                                        join g in dbContext.tbl_PM_GroupMaster on empList.GroupID equals g.GroupID into empGroup
                                                                        from groupList in empGroup.DefaultIfEmpty()
                                                                        join y in dbContext.tbl_Appraisal_YearMaster on a.AppraisalYearID equals y.AppraisalYearID into year
                                                                        from yearList in year.DefaultIfEmpty()
                                                                        where a.AppraisalYearID == YearID
                                                                        select new AppraisalStatusReportViewModel
                                                                        {
                                                                            AppraisalID = a.AppraisalID,
                                                                            AppraisalStageID = a.AppraisalStageID,
                                                                            AppraisalStageDesc = stageList.AppraisalStage,
                                                                            AppraisalYear = yearList.AppraisalYear,
                                                                            AppraisalYearID = a.AppraisalYearID,
                                                                            Appraiser1ID = a.Appraiser1,
                                                                            Appraiser1Name = appraiser1List.EmployeeName,
                                                                            Appraiser2ID = a.Appraiser2,
                                                                            Appraiser2Name = appraiser2List.EmployeeName,
                                                                            ConfirmationDate = empList.ConfirmationDate,
                                                                            ProbationReviewDate = empList.Probation_Review_Date,
                                                                            DeliveryTeamID = empList.EmployeeID,
                                                                            DeliveryTeamName = groupList.GroupName,
                                                                            Employeecode = empList.EmployeeCode,
                                                                            EmployeeID = a.EmployeeID,
                                                                            EmployeeName = empList.EmployeeName,
                                                                            GroupHeadID = a.GroupHead,
                                                                            GroupHeadName = grouplist.EmployeeName,
                                                                            Reviewer1ID = a.Reviewer1,
                                                                            Reviewer1Name = reviewer1List.EmployeeName,
                                                                            Reviewer2ID = a.Reviewer2,
                                                                            Reviewer2Name = reviewer2List.EmployeeName,
                                                                            DesignationID = empList.DesignationID,
                                                                            DesignationName = designationList.DesignationName,

                                                                            EmployeeEmail = empList.EmailID,
                                                                            Appraiser1Email = appraiser1List.EmailID,
                                                                            Appraiser2Email = appraiser2List.EmailID,
                                                                            Reviewer1Email = reviewer1List.EmailID,
                                                                            Reviewer2Email = reviewer2List.EmailID,
                                                                            GroupHeadEmail = grouplist.EmailID
                                                                        }).ToList();
            return appraisalReportList.ToList();
        }

        #region MyRegion

        public List<CompetencyMaster> GetCompetencyMaster()
        {
            dbContext = new HRMSDBEntities();
            List<CompetencyMaster> comepetencylist = new List<CompetencyMaster>();
            try
            {
                comepetencylist = (from competency in dbContext.tbl_PA_Competency_Master
                                   select new CompetencyMaster
                                   {
                                       categoryID = competency.CategoryID,
                                       Compentancy = competency.Competency,
                                       Description = competency.Description,
                                       CompentancyID = competency.CompetencyID,
                                       OrderNo = competency.OrderNo,
                                       Checked = false
                                   }).ToList();
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

        public List<CategoryList> getCategoryList()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<CategoryList> categegoryList = new List<CategoryList>();
                categegoryList = (from e in dbContext.tbl_PA_CompetencyCategories
                                  select new CategoryList
                                  {
                                      CategoryID = e.CategoryID,
                                      CategoryType = e.CategoryType
                                  }).ToList();
                return categegoryList;
            }
            catch
            {
                throw;
            }
        }

        public tbl_PA_Competency_Master getParameter(int? ordernumber)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PA_Competency_Master competencyMaster = dbContext.tbl_PA_Competency_Master.Where(x => x.OrderNo == ordernumber).FirstOrDefault();
                return competencyMaster;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveParameter(addParameter addparameter)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PA_Competency_Master competencyIDmaster = dbContext.tbl_PA_Competency_Master.Where(x => x.CompetencyID == addparameter.CompetencyID).FirstOrDefault();
                if (competencyIDmaster == null)
                {
                    int OrderNo = dbContext.tbl_PA_Competency_Master.Where(x => x.OrderNo == addparameter.OrderNo).Count();
                    if (OrderNo == 0)
                    {
                        tbl_PA_Competency_Master competencyMaster = new tbl_PA_Competency_Master()
                        {
                            //CompetencyID=addparameter.CompetencyID,
                            Competency = addparameter.Parameter,
                            OrderNo = addparameter.OrderNo,
                            CategoryID = Convert.ToInt32(addparameter.category),
                            BehavioralIndicators = addparameter.BehavioralIndicators,
                            Description = addparameter.Description
                        };
                        dbContext.tbl_PA_Competency_Master.AddObject(competencyMaster);
                        isAdded = true;
                    }
                    else
                    {
                        isAdded = false;
                    }
                }
                else
                {
                    competencyIDmaster.Competency = addparameter.Parameter;
                    competencyIDmaster.OrderNo = addparameter.OrderNo;
                    competencyIDmaster.CategoryID = Convert.ToInt32(addparameter.category);
                    competencyIDmaster.BehavioralIndicators = addparameter.BehavioralIndicators;
                    competencyIDmaster.Description = addparameter.Description;
                    dbContext.SaveChanges();
                    isAdded = true;
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteParameter(List<int> collection)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PA_Competency_Master competencyMaster = dbContext.tbl_PA_Competency_Master.Where(x => x.OrderNo == item).FirstOrDefault();
                    if (competencyMaster != null)
                    {
                        tbl_PA_CompetencyRoleApplicability _PA_CompetencyRoleApplicability = dbContext.tbl_PA_CompetencyRoleApplicability.Where(x => x.CompetencyID == competencyMaster.CompetencyID).FirstOrDefault();
                        if (_PA_CompetencyRoleApplicability == null)
                        {
                            dbContext.DeleteObject(competencyMaster);
                            dbContext.SaveChanges();
                            isDeleted = true;
                        }
                        else
                            isDeleted = false;
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        public List<ApplicableRole> getApplicableRoles(int? CompetencyID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ApplicableRole> competencyRoleApplicability = new List<ApplicableRole>();
                competencyRoleApplicability = (from c in dbContext.tbl_PA_CompetencyRoleApplicability
                                               join r in dbContext.HRMS_tbl_PM_Role on c.RoleID equals r.RoleID
                                               where c.CompetencyID == CompetencyID
                                               select new ApplicableRole
                                               {
                                                   CompetencyID = c.CompetencyID,
                                                   RoleID = c.RoleID,
                                                   Role = r.RoleDescription,
                                                   Checked = false
                                               }).ToList();
                return competencyRoleApplicability;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteRoles(List<int> collection, int competencyID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PA_CompetencyRoleApplicability competencyRoleApplicability = dbContext.tbl_PA_CompetencyRoleApplicability.Where(x => x.RoleID == item && x.CompetencyID == competencyID).FirstOrDefault();
                    if (competencyRoleApplicability != null)
                    {
                        dbContext.DeleteObject(competencyRoleApplicability);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }

                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        public List<ApplicableRole> getNewSelectRole(int[] roleID, int competencyID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ApplicableRole> NewSelectRole = new List<ApplicableRole>();
                NewSelectRole = (from e in dbContext.HRMS_tbl_PM_Role
                                 orderby e.RoleDescription ascending
                                 select new ApplicableRole
                                 {
                                     CompetencyID = competencyID,
                                     RoleID = e.RoleID,
                                     Role = e.RoleDescription
                                 }).ToList();

                List<ApplicableRole> NewSelectRolefinal = new List<ApplicableRole>();

                foreach (var item in NewSelectRole)
                {
                    if (roleID.Any(role => role == item.RoleID))
                        continue;
                    else
                        NewSelectRolefinal.Add(item);
                }

                return NewSelectRolefinal;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveNewRole(List<int> roleID, int competencyID)
        {
            try
            {
                bool isSaved = false;
                dbContext = new HRMSDBEntities();
                foreach (var item in roleID)
                {
                    tbl_PA_CompetencyRoleApplicability competencyRoleApplicability = new tbl_PA_CompetencyRoleApplicability();
                    competencyRoleApplicability.CompetencyID = competencyID;
                    competencyRoleApplicability.RoleID = item;
                    dbContext.tbl_PA_CompetencyRoleApplicability.AddObject(competencyRoleApplicability);
                    isSaved = true;
                }
                dbContext.SaveChanges();
                return isSaved;
            }
            catch
            {
                throw;
            }
        }

        #endregion MyRegion

        #region MyRegion

        public List<Parametercompetency> GetParameterCategories()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<Parametercompetency> parametercompetency = (from e in dbContext.tbl_PA_CompetencyCategories
                                                                 select new Parametercompetency
                                                                 {
                                                                     CategoryID = e.CategoryID,
                                                                     CategoryType = e.CategoryType,
                                                                     CategoryDescription = e.CategoryDescription,
                                                                     Checked = false
                                                                 }).ToList();
                return parametercompetency;
            }
            catch
            {
                throw;
            }
        }

        public tbl_PA_CompetencyCategories getParametercategory(int? CategoryID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PA_CompetencyCategories CompetencyCategories = dbContext.tbl_PA_CompetencyCategories.Where(x => x.CategoryID == CategoryID).FirstOrDefault();
                return CompetencyCategories;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveParameterCategory(AddNewCategory addnewcategory)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PA_CompetencyCategories CompetencyCategories = dbContext.tbl_PA_CompetencyCategories.Where(x => x.CategoryID == addnewcategory.CategoryID).FirstOrDefault();
                if (CompetencyCategories == null)
                {
                    tbl_PA_CompetencyCategories CompetencyCategory = new tbl_PA_CompetencyCategories()
                        {
                            CategoryID = addnewcategory.CategoryID,
                            CategoryType = addnewcategory.Category,
                            CategoryDescription = addnewcategory.Description
                        };
                    dbContext.tbl_PA_CompetencyCategories.AddObject(CompetencyCategory);
                    isAdded = true;
                }
                else
                {
                    CompetencyCategories.CategoryID = addnewcategory.CategoryID;
                    CompetencyCategories.CategoryType = addnewcategory.Category;
                    CompetencyCategories.CategoryDescription = addnewcategory.Description;
                    isAdded = true;
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteParameterCompetency(List<int> collection)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PA_CompetencyCategories CompetencyCategories = dbContext.tbl_PA_CompetencyCategories.Where(x => x.CategoryID == item).FirstOrDefault();
                    if (CompetencyCategories != null)
                    {
                        dbContext.DeleteObject(CompetencyCategories);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        #endregion MyRegion

        #region MyRegion

        public List<RatingScales> GetRatingScales()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<RatingScales> ratingScale = (from r in dbContext.tbl_PA_Rating_Master
                                                  select new RatingScales
                                                  {
                                                      RatingID = r.RatingID,
                                                      Rating = r.Rating,
                                                      Description = r.Description,
                                                      Percentage = r.Percentage,
                                                      ModifiedBy = r.ModifiedBy,
                                                      ModifiedDate = r.ModifiedDate,
                                                      AdjustmentFactor = r.AdjustmentFactor,
                                                      SetAsMinimumLimit = r.SetAsMinimumLimit,
                                                      Checked = false
                                                  }).ToList();
                return ratingScale;
            }
            catch
            {
                throw;
            }
        }

        public tbl_PA_Rating_Master getRatingScaleDetails(int? RatingID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PA_Rating_Master ratingscale = dbContext.tbl_PA_Rating_Master.Where(r => r.RatingID == RatingID).FirstOrDefault();
                return ratingscale;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveRatingScales(AddRatingScale model, string userRole)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PA_Rating_Master ratingScale = dbContext.tbl_PA_Rating_Master.Where(x => x.RatingID == model.RatingID).FirstOrDefault();
                if (ratingScale != null)
                {
                    ratingScale.Rating = model.Rating;
                    ratingScale.Description = model.Description;
                    ratingScale.Percentage = model.Percentage;
                    ratingScale.AdjustmentFactor = model.AdjustmentFactor;
                    ratingScale.SetAsMinimumLimit = model.SetAsMinimumLimit;
                    ratingScale.CreatedBy = ratingScale.CreatedBy;
                    ratingScale.CreatedDate = ratingScale.CreatedDate;
                    ratingScale.ModifiedDate = DateTime.Now;
                    ratingScale.ModifiedBy = userRole;
                    isAdded = true;
                }
                else
                {
                    tbl_PA_Rating_Master _Rating_Master = dbContext.tbl_PA_Rating_Master.Where(x => x.Percentage == model.Percentage).FirstOrDefault();
                    if (_Rating_Master == null)
                    {
                        tbl_PA_Rating_Master ratingscales = new tbl_PA_Rating_Master()
                        {
                            Rating = model.Rating,
                            Description = model.Description,
                            Percentage = model.Percentage,
                            AdjustmentFactor = model.AdjustmentFactor,
                            CreatedDate = DateTime.Now,
                            CreatedBy = userRole,
                            SetAsMinimumLimit = model.SetAsMinimumLimit
                        };
                        dbContext.tbl_PA_Rating_Master.AddObject(ratingscales);
                        isAdded = true;
                    }
                    else
                        isAdded = false;
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteRatingScales(List<int> collection)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PA_Rating_Master ratingScale = dbContext.tbl_PA_Rating_Master.Where(x => x.RatingID == item).FirstOrDefault();
                    if (ratingScale != null)
                    {
                        dbContext.DeleteObject(ratingScale);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        #endregion MyRegion

        #region MyRegion

        public List<RoleLists> getAllRoles()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<RoleLists> roleList = (from e in dbContext.HRMS_tbl_PM_Role
                                            select new RoleLists
                                            {
                                                RoleID = e.RoleID,
                                                RoleDescription = e.RoleDescription
                                            }).ToList();
                return roleList;
            }
            catch
            {
                throw;
            }
        }

        public List<Competencies> getCompetenciesForRole(int RoleID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<Competencies> competency = (from e in dbContext.tbl_PA_Competency_Master
                                                 join c in dbContext.tbl_PA_CompetencyRoleApplicability on e.CompetencyID equals c.CompetencyID
                                                 join r in dbContext.HRMS_tbl_PM_Role on c.RoleID equals r.RoleID
                                                 where c.RoleID == RoleID
                                                 select new Competencies
                                                 {
                                                     OrderNo = e.OrderNo,
                                                     Parameter = e.Competency,
                                                     Description = e.Description,
                                                     CompetencyID = e.CompetencyID
                                                 }).ToList();
                return competency;
            }
            catch
            {
                throw;
            }
        }

        public List<Competencies> getNewSelectCompetency(int[] CompetencyID, int RoleID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<Competencies> NewSelectCompetency = new List<Competencies>();
                NewSelectCompetency = (from c in dbContext.tbl_PA_Competency_Master
                                       select new Competencies
                                       {
                                           OrderNo = c.OrderNo,
                                           CompetencyID = c.CompetencyID,
                                           Parameter = c.Competency,
                                           Description = c.Description,
                                           RoleID = RoleID,
                                           Checked = false
                                       }).Distinct().ToList();

                List<Competencies> NewSelectCompetencyfinal = new List<Competencies>();

                foreach (var item in NewSelectCompetency)
                {
                    if (CompetencyID.Any(role => role == item.CompetencyID))
                        continue;
                    else
                        NewSelectCompetencyfinal.Add(item);
                }
                return NewSelectCompetencyfinal;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveNewCompetency(List<int> CompetencyID, int RoleID)
        {
            try
            {
                bool isSaved = false;
                dbContext = new HRMSDBEntities();
                foreach (var item in CompetencyID)
                {
                    tbl_PA_CompetencyRoleApplicability CompetencyRoleApplicability = new tbl_PA_CompetencyRoleApplicability();
                    CompetencyRoleApplicability.CompetencyID = item;
                    CompetencyRoleApplicability.RoleID = RoleID;
                    dbContext.tbl_PA_CompetencyRoleApplicability.AddObject(CompetencyRoleApplicability);
                    isSaved = true;
                }
                dbContext.SaveChanges();
                return isSaved;
            }
            catch
            {
                throw;
            }
        }

        public bool DeletenewCompetencies(List<int> collection, int RoleID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PA_CompetencyRoleApplicability CompetencyRoleApplicability = dbContext.tbl_PA_CompetencyRoleApplicability.Where(x => x.CompetencyID == item && x.RoleID == RoleID).FirstOrDefault();
                    if (CompetencyRoleApplicability != null)
                    {
                        dbContext.tbl_PA_CompetencyRoleApplicability.DeleteObject(CompetencyRoleApplicability);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        #endregion MyRegion

        #region MyRegion

        public bool DeleteBusinessManager(List<int> collection, int BusinessGroupID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_CNF_BusinessGroup_Managers BusinessManagers = dbContext.tbl_CNF_BusinessGroup_Managers.Where(x => x.ManagerID == item && x.BusinessGroupID == BusinessGroupID).FirstOrDefault();
                    if (BusinessManagers != null)
                    {
                        dbContext.DeleteObject(BusinessManagers);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteMiddleLevelResource(List<int> collection, int BusinessGroupID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_CNF_BusinessGroups_MiddleLevelResources MiddleLevelResources = dbContext.tbl_CNF_BusinessGroups_MiddleLevelResources.Where(x => x.EmployeeID == item && x.BusinessGroupID == BusinessGroupID).FirstOrDefault();
                    if (MiddleLevelResources != null)
                    {
                        dbContext.DeleteObject(MiddleLevelResources);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        public OrganizationStructureResponse SaveBusinessGroups(OrganizationStructure model)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                OrganizationStructureResponse Response = new OrganizationStructureResponse();
                Response.Isadded = false;
                Response.IsExisted = false;
                if (model.BusinessGroupID != 0)
                {
                    tbl_CNF_BusinessGroups _CNF_BusinessGroups = dbContext.tbl_CNF_BusinessGroups.Where(x => x.BusinessGroupID == model.BusinessGroupID).FirstOrDefault();
                    if (_CNF_BusinessGroups != null)
                    {
                        if (model.Active == false)
                        {
                            tbl_CNF_BusinessGroup_OUPools BusinessGroup_OUPools = (from OU in dbContext.tbl_CNF_BusinessGroup_OUPools
                                                                                   join l in dbContext.tbl_PM_Location on OU.OUPoolID equals l.LocationID
                                                                                   where OU.BusinessGroupID == model.BusinessGroupID && l.Active == true
                                                                                   select OU).FirstOrDefault();
                            //dbContext.tbl_CNF_BusinessGroup_OUPools.Where(x => x.BusinessGroupID == model.BusinessGroupID).FirstOrDefault();
                            if (BusinessGroup_OUPools != null)
                            {
                                Response.Isadded = false;
                            }
                            else
                            {
                                _CNF_BusinessGroups.BusinessGroupID = model.BusinessGroupID;
                                _CNF_BusinessGroups.BusinessGroup = model.businessgroup;
                                _CNF_BusinessGroups.BusinessGroupCode = model.BusinessGroupCode;
                                _CNF_BusinessGroups.Active = model.Active;
                                Response.Isadded = true;
                            }
                        }
                        else
                        {
                            _CNF_BusinessGroups.BusinessGroupID = model.BusinessGroupID;
                            _CNF_BusinessGroups.BusinessGroup = model.businessgroup;
                            _CNF_BusinessGroups.BusinessGroupCode = model.BusinessGroupCode;
                            _CNF_BusinessGroups.Active = model.Active;
                            Response.Isadded = true;
                        }
                    }
                }
                else
                {
                    tbl_CNF_BusinessGroups BusinessGroup = dbContext.tbl_CNF_BusinessGroups.Where(x => x.BusinessGroupCode == model.BusinessGroupCode || x.BusinessGroup == model.businessgroup).FirstOrDefault();
                    if (BusinessGroup == null)
                    {
                        tbl_CNF_BusinessGroups _BusinessGroups = new tbl_CNF_BusinessGroups()
                        {
                            BusinessGroupID = model.BusinessGroupID,
                            BusinessGroup = model.businessgroup,
                            BusinessGroupCode = model.BusinessGroupCode,
                            Active = true
                        };
                        dbContext.tbl_CNF_BusinessGroups.AddObject(_BusinessGroups);
                        Response.Isadded = true;
                    }
                    else
                    {
                        Response.IsExisted = true;
                        Response.Isadded = false;
                    }
                }
                dbContext.SaveChanges();
                return Response;
            }
            catch
            {
                throw;
            }
        }

        #endregion MyRegion

        public List<ManagerList> getallemployee(int[] EmployeeID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ManagerList> managerList = (from e in dbContext.HRMS_tbl_PM_Employee
                                                 where e.Status == false
                                                 select new ManagerList
                                                           {
                                                               BusinessGroupID = e.BusinessGroupID.HasValue ? e.BusinessGroupID.Value : 0,
                                                               EmployeeID = e.EmployeeID,
                                                               EmployeeName = e.EmployeeName.Trim(),
                                                               UserName = e.UserName,
                                                               IsPrimaryResponsible = false
                                                           }).Distinct().OrderBy(v => v.EmployeeName).ToList();

                List<ManagerList> managerListWithoutExistingManagers = new List<ManagerList>();

                foreach (var manager in managerList)
                {
                    if (EmployeeID.Any(_empID => _empID == manager.EmployeeID))
                        continue;
                    else
                        managerListWithoutExistingManagers.Add(manager);
                }

                return managerListWithoutExistingManagers;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveBusinessGroupManagers(OrganizationStructure model)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                int Managerid = Convert.ToInt32(model.Manager);
                tbl_CNF_BusinessGroup_Managers _businessGroupManagers = dbContext.tbl_CNF_BusinessGroup_Managers.Where(x => x.BusinessGroupID == model.BusinessGroupID && x.ManagerID == model.Old_Manager).FirstOrDefault();
                if (_businessGroupManagers == null)
                {
                    tbl_CNF_BusinessGroup_Managers _businessGroupManager = new tbl_CNF_BusinessGroup_Managers();
                    _businessGroupManager.ManagerID = Convert.ToInt32(model.Manager);
                    _businessGroupManager.BusinessGroupID = model.BusinessGroupID;
                    _businessGroupManager.IsPrimaryResponsible = model.IsPrimaryResponsible;
                    _businessGroupManager.CreatedDate = DateTime.Now;
                    dbContext.tbl_CNF_BusinessGroup_Managers.AddObject(_businessGroupManager);
                    isAdded = true;
                }
                else
                {
                    _businessGroupManagers.ManagerID = Managerid;
                    _businessGroupManagers.BusinessGroupID = model.BusinessGroupID;
                    _businessGroupManagers.IsPrimaryResponsible = model.IsPrimaryResponsible;
                    _businessGroupManagers.ModifiedDate = DateTime.Now;
                    isAdded = true;
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public bool saveOrganizationUnits(OrganizationStructure model)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PM_Location _pmLocation = dbContext.tbl_PM_Location.Where(x => x.LocationCode == model.LocationCode || x.Location == model.Location).FirstOrDefault();
                if (_pmLocation == null)
                {
                    tbl_PM_Location _tbl_pmLocation = new tbl_PM_Location();
                    _tbl_pmLocation.LocationID = model.LocationID;
                    _tbl_pmLocation.Location = model.Location;
                    _tbl_pmLocation.CreatedDate = DateTime.Now;
                    _tbl_pmLocation.LocationCode = model.LocationCode;
                    _tbl_pmLocation.Active = true;
                    dbContext.tbl_PM_Location.AddObject(_tbl_pmLocation);
                    dbContext.SaveChanges();

                    tbl_PM_Location _locationDetails = dbContext.tbl_PM_Location.Where(x => x.Location == model.Location).FirstOrDefault();

                    tbl_CNF_BusinessGroup_OUPools _businessGroupOUPool = new tbl_CNF_BusinessGroup_OUPools();
                    _businessGroupOUPool.UniqueID = model.UniqueID;
                    _businessGroupOUPool.BusinessGroupID = model.BusinessGroupID;
                    _businessGroupOUPool.OUPoolID = _locationDetails.LocationID;
                    dbContext.tbl_CNF_BusinessGroup_OUPools.AddObject(_businessGroupOUPool);
                    isAdded = true;
                    dbContext.SaveChanges();
                }
                else
                {
                    isAdded = false;
                }

                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public bool deleteOrganizationUnits(int[] collection, int BusinessGroupID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();

                foreach (var item in collection)
                {
                    //tbl_PM_OUPool_ResourcePools _OUPool_ResourcePools = dbContext.tbl_PM_OUPool_ResourcePools.Where(x => x.OUPoolID == item).FirstOrDefault();
                    tbl_PM_OUPool_ResourcePools _OUPool_ResourcePools = (from ou in dbContext.tbl_PM_OUPool_ResourcePools
                                                                         join r in dbContext.HRMS_tbl_PM_ResourcePool on ou.ResourcePoolID equals r.ResourcePoolID
                                                                         where ou.OUPoolID == item && r.Active == true
                                                                         select ou).FirstOrDefault();
                    if (_OUPool_ResourcePools != null)
                    {
                        isDeleted = false;
                    }
                    else
                    {
                        tbl_PM_Location _pmLocation = dbContext.tbl_PM_Location.Where(x => x.LocationID == item).FirstOrDefault();
                        if (_pmLocation != null)
                        {
                            _pmLocation.Active = false;
                        }
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        public List<MiddleLevelResources> selectNewResouce(int[] collection, int BusinessGroupID)
        {
            try
            {
                dbContext = new HRMSDBEntities();

                List<MiddleLevelResources> selectNewResouce = (from e in dbContext.HRMS_tbl_PM_Employee
                                                               join r in dbContext.HRMS_tbl_PM_Role on e.PostID equals r.RoleID into xy
                                                               from x in xy.DefaultIfEmpty()
                                                               where e.Status == false
                                                               select new MiddleLevelResources
                                                               {
                                                                   EmpoloyeeID = e.EmployeeID,
                                                                   EmployeeName = e.EmployeeName,
                                                                   Role = x.RoleDescription,
                                                                   EmailID = e.EmailID,
                                                                   BusinessGroupID = BusinessGroupID,
                                                                   Checked = false
                                                               }).ToList();

                List<MiddleLevelResources> selectNewResouceWithoutExistingResource = new List<MiddleLevelResources>();

                foreach (var Resouce in selectNewResouce)
                {
                    if (collection.Any(_empID => _empID == Resouce.EmpoloyeeID))
                        continue;
                    else
                        selectNewResouceWithoutExistingResource.Add(Resouce);
                }
                return selectNewResouceWithoutExistingResource;
            }
            catch
            {
                throw;
            }
        }

        public bool saveMiddleLevelResouceForBusinessGroup(int[] collection, int BuainessGroupID)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_CNF_BusinessGroups_MiddleLevelResources BusinessGroups_MiddleLevelResources = new tbl_CNF_BusinessGroups_MiddleLevelResources();
                    BusinessGroups_MiddleLevelResources.BusinessGroupID = BuainessGroupID;
                    BusinessGroups_MiddleLevelResources.EmployeeID = item;
                    dbContext.tbl_CNF_BusinessGroups_MiddleLevelResources.AddObject(BusinessGroups_MiddleLevelResources);
                    dbContext.SaveChanges();
                    isAdded = true;
                }
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public tbl_CNF_BusinessGroup_Managers EditBusinessGroupManager(int EmpID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_CNF_BusinessGroup_Managers BusinessGroup_Manager = dbContext.tbl_CNF_BusinessGroup_Managers.Where(x => x.ManagerID == EmpID).FirstOrDefault();
                return BusinessGroup_Manager;
            }
            catch
            {
                throw;
            }
        }

        #region MyRegion

        public List<MiddleLevelResources> getMiddleLevelResourcesForDeleveryTeam(int BusinessGroupID, int groupID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<MiddleLevelResources> middlelevelresources = (from g in dbContext.tbl_PM_GroupMaster_MiddleLevelResources
                                                                   join e in dbContext.HRMS_tbl_PM_Employee on g.EmployeeID equals e.EmployeeID
                                                                   join r in dbContext.HRMS_tbl_PM_Role on e.PostID equals r.RoleID into xy
                                                                   from x in xy.DefaultIfEmpty()
                                                                   where g.GroupID == groupID && e.Status == false
                                                                   select new MiddleLevelResources
                                                                   {
                                                                       EmpoloyeeID = e.EmployeeID,
                                                                       EmployeeName = e.EmployeeName,
                                                                       BusinessGroupID = BusinessGroupID,
                                                                       EmailID = e.EmailID,
                                                                       Role = x.RoleDescription,
                                                                       Checked = false
                                                                   }).ToList();
                return middlelevelresources;
            }
            catch
            {
                throw;
            }
        }

        public tbl_PM_GroupMaster getDeleveryDetails(int GroupID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PM_GroupMaster _GroupMaster = dbContext.tbl_PM_GroupMaster.Where(x => x.GroupID == GroupID).FirstOrDefault();
                return _GroupMaster;
            }
            catch
            {
                throw;
            }
        }

        public bool saveMiddleLevelResouceForDeleveryTeam(int[] collection, int GroupID)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PM_GroupMaster_MiddleLevelResources GroupMaster_MiddleLevelResources = new tbl_PM_GroupMaster_MiddleLevelResources();
                    GroupMaster_MiddleLevelResources.GroupID = GroupID;
                    GroupMaster_MiddleLevelResources.EmployeeID = item;
                    dbContext.tbl_PM_GroupMaster_MiddleLevelResources.AddObject(GroupMaster_MiddleLevelResources);
                    dbContext.SaveChanges();
                    isAdded = true;
                }
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteMiddleLevelResourceForDeleveryTeam(List<int> collection, int GroupID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PM_GroupMaster_MiddleLevelResources GroupMaster_MiddleLevelResources = dbContext.tbl_PM_GroupMaster_MiddleLevelResources.Where(x => x.GroupID == GroupID && x.EmployeeID == item).FirstOrDefault();
                    if (GroupMaster_MiddleLevelResources != null)
                    {
                        dbContext.tbl_PM_GroupMaster_MiddleLevelResources.DeleteObject(GroupMaster_MiddleLevelResources);
                        dbContext.SaveChanges();
                    }
                    isDeleted = true;
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        public OrganizationStructureResponse SaveDeleveryTeam(OrganizationStructure model)
        {
            try
            {
                OrganizationStructureResponse response = new OrganizationStructureResponse();
                response.Isadded = false;
                response.IsActive = true;
                dbContext = new HRMSDBEntities();
                if (model.GroupID != 0)
                {
                    tbl_PM_GroupMaster _GroupMaster = dbContext.tbl_PM_GroupMaster.Where(x => x.GroupID == model.GroupID).FirstOrDefault();
                    if (_GroupMaster != null)
                    {
                        _GroupMaster.GroupID = model.GroupID;
                        _GroupMaster.GroupCode = model.GroupCode;
                        _GroupMaster.GroupName = model.GroupName;
                        _GroupMaster.ModifiedDate = DateTime.Now;
                        _GroupMaster.ResourceHeadID = Convert.ToInt32(model.Manager);
                        _GroupMaster.Active = model.Active;
                        response.Isadded = true;
                    }
                    else
                    {
                        tbl_PM_GroupMaster groupMaster = new tbl_PM_GroupMaster();
                        groupMaster.GroupID = model.GroupID;
                        groupMaster.GroupCode = model.GroupCode;
                        groupMaster.GroupName = model.GroupName;
                        groupMaster.CreatedDate = DateTime.Now;
                        groupMaster.ResourceHeadID = Convert.ToInt32(model.Manager);
                        groupMaster.Active = model.Active;
                        dbContext.tbl_PM_GroupMaster.AddObject(groupMaster);
                        response.Isadded = true;
                    }
                }
                else
                {
                    tbl_PM_GroupMaster _GroupMasters = dbContext.tbl_PM_GroupMaster.Where(x => x.GroupCode == model.GroupCode || x.GroupName == model.GroupName).FirstOrDefault();
                    if (_GroupMasters == null)
                    {
                        tbl_PM_GroupMaster groupMaster = new tbl_PM_GroupMaster();
                        groupMaster.GroupID = model.GroupID;
                        groupMaster.GroupCode = model.GroupCode;
                        groupMaster.GroupName = model.GroupName;
                        groupMaster.CreatedDate = DateTime.Now;
                        groupMaster.ResourceHeadID = Convert.ToInt32(model.Manager);
                        groupMaster.Active = true;
                        dbContext.tbl_PM_GroupMaster.AddObject(groupMaster);
                        dbContext.SaveChanges();

                        tbl_PM_GroupMaster groupmasters = dbContext.tbl_PM_GroupMaster.Where(x => x.GroupName == model.GroupName).FirstOrDefault();

                        tbl_PM_ResourcePool_Teams resoucePool = new tbl_PM_ResourcePool_Teams();
                        resoucePool.ResourcePoolID = model.ResourcePoolID;
                        resoucePool.GroupID = groupmasters.GroupID;
                        dbContext.tbl_PM_ResourcePool_Teams.AddObject(resoucePool);
                        response.Isadded = true;
                    }
                    else if (_GroupMasters.Active == false)
                    {
                        response.IsActive = false;
                    }
                    else
                    {
                        response.Isadded = false;
                    }
                }
                dbContext.SaveChanges();
                return response;
            }
            catch
            {
                throw;
            }
        }

        #endregion MyRegion

        #region MyRegion

        public HRMS_tbl_PM_ResourcePool getDeleveryUnitDetails(int ResourcePoolID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                HRMS_tbl_PM_ResourcePool _resoucePool = dbContext.HRMS_tbl_PM_ResourcePool.Where(x => x.ResourcePoolID == ResourcePoolID).FirstOrDefault();
                return _resoucePool;
            }
            catch
            {
                throw;
            }
        }

        public List<DeliveryTeam> getDeliveryTeamForDeleveryUnit(int BusinessGroupID, int ResourcePoolID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<DeliveryTeam> deliveryTeam = (from r in dbContext.HRMS_tbl_PM_ResourcePool
                                                   join t in dbContext.tbl_PM_ResourcePool_Teams on r.ResourcePoolID equals t.ResourcePoolID
                                                   join g in dbContext.tbl_PM_GroupMaster on t.GroupID equals g.GroupID
                                                   join e in dbContext.HRMS_tbl_PM_Employee on g.ResourceHeadID equals e.EmployeeID into xy
                                                   from x in xy.DefaultIfEmpty()
                                                   where r.ResourcePoolID == ResourcePoolID && g.Active == true
                                                   select new DeliveryTeam
                                                   {
                                                       GroupName = g.GroupName,
                                                       GroupCode = g.GroupCode,
                                                       GroupID = g.GroupID,
                                                       ResourcePoolID = t.ResourcePoolID,
                                                       ResourceHeadID = g.ResourceHeadID,
                                                       EmployeeName = x.EmployeeName,
                                                       EmployeeID = g.ResourceHeadID.HasValue ? g.ResourceHeadID.Value : 0,
                                                       BusinessGroupID = BusinessGroupID,
                                                       Checked = false
                                                   }).ToList();
                HRMS_tbl_PM_Employee _employee = new HRMS_tbl_PM_Employee();
                foreach (var item in deliveryTeam)
                {
                    _employee = dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeID == item.EmployeeID).FirstOrDefault();
                    if (_employee != null)
                    {
                        if (_employee.Status == false)
                            item.EmployeeName = item.EmployeeName;
                        else
                            item.EmployeeName = null;
                    }
                }
                return deliveryTeam;
            }
            catch
            {
                throw;
            }
        }

        public List<ManagerList> getdeliveryUnitManagers(int BusinessGroupID, int ResourcePoolID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ManagerList> _managerList = (from poolmanagers in dbContext.tbl_PM_ResourcePool_Managers
                                                  join resourcepool in dbContext.HRMS_tbl_PM_ResourcePool on poolmanagers.ResourcePoolID equals resourcepool.ResourcePoolID
                                                  join e in dbContext.HRMS_tbl_PM_Employee on poolmanagers.ManagerID equals e.EmployeeID
                                                  where resourcepool.Active == true && poolmanagers.ResourcePoolID == ResourcePoolID
                                                  select new ManagerList
                                                  {
                                                      BusinessGroupID = BusinessGroupID,
                                                      EmployeeID = e.EmployeeID,
                                                      EmployeeName = e.EmployeeName,
                                                      ResourcePoolID = poolmanagers.ResourcePoolID.HasValue ? poolmanagers.ResourcePoolID.Value : 0,
                                                      UserName = e.UserName,
                                                      IsPrimaryResponsible = poolmanagers.IsPrimaryResponsible.HasValue ? poolmanagers.IsPrimaryResponsible.Value : false,
                                                      Checked = false
                                                  }).ToList();
                return _managerList;
            }
            catch
            {
                throw;
            }
        }

        public List<MiddleLevelResources> getMiddleLevelResourcesForDeleveryUnits(int BusinessGroupID, int ResourcePoolID)
        {
            try
            {
                List<MiddleLevelResources> MiddleLevelResourcesList = (from g in dbContext.tbl_PM_ResourcePool_MiddleLevelResources
                                                                       join e in dbContext.HRMS_tbl_PM_Employee on g.EmployeeID equals e.EmployeeID
                                                                       join r in dbContext.HRMS_tbl_PM_Role on e.PostID equals r.RoleID into xy
                                                                       from x in xy.DefaultIfEmpty()
                                                                       where g.ResourcePoolID == ResourcePoolID && e.Status == false
                                                                       select new MiddleLevelResources
                                                                       {
                                                                           EmpoloyeeID = e.EmployeeID,
                                                                           EmployeeName = e.EmployeeName,
                                                                           BusinessGroupID = BusinessGroupID,
                                                                           EmailID = e.EmailID,
                                                                           Role = x.RoleDescription,
                                                                           Checked = false
                                                                       }).ToList();
                return MiddleLevelResourcesList;
            }
            catch
            {
                throw;
            }
        }

        public bool deleteDeleteDeliveryTeams(int[] collection, int ResourcePoolID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();

                foreach (var item in collection)
                {
                    tbl_PM_GroupMaster _PM_GroupMaster = dbContext.tbl_PM_GroupMaster.Where(x => x.GroupID == item).FirstOrDefault();
                    if (_PM_GroupMaster != null)
                    {
                        _PM_GroupMaster.Active = false;
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveDeliveryUnitManagers(OrganizationStructure model)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                int Managerid = Convert.ToInt32(model.Manager);
                tbl_PM_ResourcePool_Managers _ResourcePool_Managers = dbContext.tbl_PM_ResourcePool_Managers.Where(x => x.ResourcePoolID == model.ResourcePoolID && x.ManagerID == model.Old_Manager).FirstOrDefault();
                if (_ResourcePool_Managers == null)
                {
                    tbl_PM_ResourcePool_Managers _ResourcePool_Manager = new tbl_PM_ResourcePool_Managers();
                    _ResourcePool_Manager.ManagerID = Convert.ToInt32(model.Manager);
                    _ResourcePool_Manager.ResourcePoolID = model.ResourcePoolID;
                    _ResourcePool_Manager.IsPrimaryResponsible = model.IsPrimaryResponsible;
                    _ResourcePool_Manager.CreatedDate = DateTime.Now;
                    dbContext.tbl_PM_ResourcePool_Managers.AddObject(_ResourcePool_Manager);
                    isAdded = true;
                }
                else
                {
                    _ResourcePool_Managers.ManagerID = Managerid;
                    _ResourcePool_Managers.ResourcePoolID = model.ResourcePoolID;
                    _ResourcePool_Managers.IsPrimaryResponsible = model.IsPrimaryResponsible;
                    _ResourcePool_Managers.ModifiedDate = DateTime.Now;
                    isAdded = true;
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public tbl_PM_ResourcePool_Managers EditDeliveryUnitManager(int EmpID)
        {
            dbContext = new HRMSDBEntities();
            tbl_PM_ResourcePool_Managers _ResourcePool_Managers = dbContext.tbl_PM_ResourcePool_Managers.Where(x => x.ManagerID == EmpID).FirstOrDefault();
            return _ResourcePool_Managers;
        }

        public bool DeleteMiddleLevelResourceForDeliveryUnit(List<int> collection, int ResourcePoolID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PM_ResourcePool_MiddleLevelResources _ResourcePool_MiddleLevelResources = dbContext.tbl_PM_ResourcePool_MiddleLevelResources.Where(x => x.ResourcePoolID == ResourcePoolID && x.EmployeeID == item).FirstOrDefault();
                    if (_ResourcePool_MiddleLevelResources != null)
                    {
                        dbContext.tbl_PM_ResourcePool_MiddleLevelResources.DeleteObject(_ResourcePool_MiddleLevelResources);
                        dbContext.SaveChanges();
                    }
                    isDeleted = true;
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        public bool saveMiddleLevelResouceForDeleveryUnits(int[] collection, int ResourcePoolID)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PM_ResourcePool_MiddleLevelResources _ResourcePool_MiddleLevelResources = new tbl_PM_ResourcePool_MiddleLevelResources();
                    _ResourcePool_MiddleLevelResources.ResourcePoolID = ResourcePoolID;
                    _ResourcePool_MiddleLevelResources.EmployeeID = item;
                    dbContext.tbl_PM_ResourcePool_MiddleLevelResources.AddObject(_ResourcePool_MiddleLevelResources);
                    dbContext.SaveChanges();
                    isAdded = true;
                }
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveDeliveryUnits(OrganizationStructure model)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                HRMS_tbl_PM_ResourcePool _PM_ResourcePool = dbContext.HRMS_tbl_PM_ResourcePool.Where(x => x.ResourcePoolID == model.ResourcePoolID).FirstOrDefault();
                if (_PM_ResourcePool == null)
                {
                    HRMS_tbl_PM_ResourcePool PM_ResourcePool = new HRMS_tbl_PM_ResourcePool()
                        {
                            ResourcePoolCode = model.ResourcePoolCode,
                            ResourcePoolName = model.ResourcePoolName,
                            Active = model.Active,
                            TimesheetRequired = model.TimesheetRequired
                        };
                    dbContext.HRMS_tbl_PM_ResourcePool.AddObject(PM_ResourcePool);
                    isAdded = true;
                }
                else
                {
                    if (model.Active == false)
                    {
                        //tbl_PM_ResourcePool_Teams _ResourcePool_Teams = dbContext.tbl_PM_ResourcePool_Teams.Where(x => x.ResourcePoolID == model.ResourcePoolID).FirstOrDefault();
                        tbl_PM_ResourcePool_Teams _ResourcePool_Teams = (from r in dbContext.tbl_PM_ResourcePool_Teams
                                                                         join rp in dbContext.tbl_PM_GroupMaster on r.GroupID equals rp.GroupID
                                                                         where r.ResourcePoolID == model.ResourcePoolID && rp.Active == true
                                                                         select r).FirstOrDefault();
                        if (_ResourcePool_Teams != null)
                        {
                            isAdded = false;
                        }
                        else
                        {
                            _PM_ResourcePool.ResourcePoolID = model.ResourcePoolID;
                            _PM_ResourcePool.ResourcePoolCode = model.ResourcePoolCode;
                            _PM_ResourcePool.ResourcePoolName = model.ResourcePoolName;
                            _PM_ResourcePool.Active = model.Active;
                            _PM_ResourcePool.TimesheetRequired = model.TimesheetRequired;
                            isAdded = true;
                        }
                    }
                    else
                    {
                        _PM_ResourcePool.ResourcePoolID = model.ResourcePoolID;
                        _PM_ResourcePool.ResourcePoolCode = model.ResourcePoolCode;
                        _PM_ResourcePool.ResourcePoolName = model.ResourcePoolName;
                        _PM_ResourcePool.Active = model.Active;
                        _PM_ResourcePool.TimesheetRequired = model.TimesheetRequired;
                        isAdded = true;
                    }
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteDeliveryUnitManager(List<int> collection, int resourcePoolID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PM_ResourcePool_Managers _PM_ResourcePool_Managers = dbContext.tbl_PM_ResourcePool_Managers.Where(x => x.ResourcePoolID == resourcePoolID && x.ManagerID == item).FirstOrDefault();
                    if (_PM_ResourcePool_Managers != null)
                    {
                        dbContext.DeleteObject(_PM_ResourcePool_Managers);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        #endregion MyRegion

        #region MyRegion

        public List<BusinessGroup> getInactiveBusinessGroup()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<BusinessGroup> businessGroups = new List<BusinessGroup>();
                businessGroups = (from businessGroup in dbContext.tbl_CNF_BusinessGroups
                                  orderby businessGroup.BusinessGroup ascending
                                  where businessGroup.Active == false
                                  select new BusinessGroup
                                  {
                                      BusinessGroupID = businessGroup.BusinessGroupID,
                                      businessgroup = businessGroup.BusinessGroup,
                                      BusinessGroupCode = businessGroup.BusinessGroupCode,
                                      ModifiedBy = businessGroup.ModifiedBy,
                                      ModifiedDate = businessGroup.ModifiedDate,
                                      Active = businessGroup.Active.HasValue ? businessGroup.Active.Value : false,
                                      LastSequence = businessGroup.LastSequence,
                                      Checked = false
                                  }).ToList();
                return businessGroups;
            }
            catch
            {
                throw;
            }
        }

        public List<OrganizationUnit> getInactiveOraganizationUnits()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<OrganizationUnit> _organizationUnits = new List<OrganizationUnit>();
                _organizationUnits = (from l in dbContext.tbl_PM_Location
                                      orderby l.Location ascending
                                      where l.Active == false
                                      select new OrganizationUnit
                                      {
                                          LocationID = l.LocationID,
                                          Location = l.Location,
                                          LocationCode = l.LocationCode,
                                          CreatedDate = l.CreatedDate,
                                          ModifiedBy = l.ModifiedBy,
                                          ModifiedDate = l.ModifiedDate,
                                          Active = l.Active.HasValue ? l.Active.Value : false,
                                          Checked = false
                                      }).ToList();
                return _organizationUnits;
            }
            catch
            {
                throw;
            }
        }

        public List<DeliveryUnit> getInactiveDeliveryUnit()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<DeliveryUnit> _deliveryUnit = new List<DeliveryUnit>();
                _deliveryUnit = (from r in dbContext.HRMS_tbl_PM_ResourcePool
                                 orderby r.ResourcePoolName ascending
                                 where r.Active == false
                                 select new DeliveryUnit
                                 {
                                     ResourcePoolID = r.ResourcePoolID,
                                     ResourcePoolCode = r.ResourcePoolCode,
                                     ResourcePoolName = r.ResourcePoolName,
                                     CreatedBy = r.CreatedBy,
                                     CreatedDate = r.CreatedDate,
                                     ModifiedBy = r.ModifiedBy,
                                     ModifiedDate = r.ModifiedDate,
                                     Active = r.Active.HasValue ? r.Active.Value : false
                                 }).ToList();
                return _deliveryUnit;
            }
            catch
            {
                throw;
            }
        }

        public List<DeliveryTeam> getInactiveDeliveryTeams()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<DeliveryTeam> _deliveryTeam = new List<DeliveryTeam>();
                _deliveryTeam = (from g in dbContext.tbl_PM_GroupMaster
                                 orderby g.GroupName ascending
                                 where g.Active == false
                                 select new DeliveryTeam
                                 {
                                     GroupName = g.GroupName,
                                     GroupCode = g.GroupCode,
                                     GroupID = g.GroupID,
                                     ResourceHeadID = g.ResourceHeadID
                                 }).ToList();
                return _deliveryTeam;
            }
            catch
            {
                throw;
            }
        }

        public List<ExistingOrganizationUnit> getExistingOU(int BusinessGroupID, List<OrganizationUnit> organizationUnits)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ExistingOrganizationUnit> _ExistingOU = new List<ExistingOrganizationUnit>();
                _ExistingOU = (from l in dbContext.tbl_PM_Location
                               join b in dbContext.tbl_CNF_BusinessGroup_OUPools on l.LocationID equals b.OUPoolID into xy
                               from x in xy.DefaultIfEmpty()
                               where (x.BusinessGroupID != BusinessGroupID || x.BusinessGroupID == null) && l.Active == true
                               select new ExistingOrganizationUnit
                               {
                                   Location = l.Location,
                                   LocationID = l.LocationID,
                                   BusinessGroupID = x.BusinessGroupID.HasValue ? x.BusinessGroupID.Value : 0
                               }).ToList();
                List<ExistingOrganizationUnit> _ExistingOUFiltered = new List<ExistingOrganizationUnit>();
                foreach (var item in _ExistingOU)
                {
                    if (organizationUnits.Any(org => org.LocationID == item.LocationID))
                        continue;
                    else
                        _ExistingOUFiltered.Add(item);
                }
                return _ExistingOUFiltered;
            }
            catch
            {
                throw;
            }
        }

        public bool saveExistingOrganizationUnits(OrganizationStructure model)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                int ExistingOU = Convert.ToInt32(model.ExistingOU);
                tbl_CNF_BusinessGroup_OUPools BusinessGroup_OUPools = dbContext.tbl_CNF_BusinessGroup_OUPools.Where(x => x.BusinessGroupID == model.BusinessGroupID && x.OUPoolID == ExistingOU).FirstOrDefault();
                if (BusinessGroup_OUPools == null)
                {
                    tbl_CNF_BusinessGroup_OUPools _BusinessGroup_OUPools = new tbl_CNF_BusinessGroup_OUPools();
                    _BusinessGroup_OUPools.BusinessGroupID = model.BusinessGroupID;
                    _BusinessGroup_OUPools.OUPoolID = ExistingOU;
                    dbContext.tbl_CNF_BusinessGroup_OUPools.AddObject(_BusinessGroup_OUPools);
                    isAdded = true;

                    tbl_PM_Location _PM_Location = dbContext.tbl_PM_Location.Where(x => x.LocationID == ExistingOU).FirstOrDefault();
                    if (_PM_Location != null && _PM_Location.Active == false)
                    {
                        _PM_Location.Active = true;
                    }
                    dbContext.SaveChanges();
                }
                else
                    isAdded = false;
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteBusinessGroups(int BusinessGroupID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                //tbl_CNF_BusinessGroup_OUPools _BusinessGroup_OUPools = dbContext.tbl_CNF_BusinessGroup_OUPools.Where(x => x.BusinessGroupID == BusinessGroupID).FirstOrDefault();
                tbl_CNF_BusinessGroup_OUPools _BusinessGroup_OUPools = (from OU in dbContext.tbl_CNF_BusinessGroup_OUPools
                                                                        join l in dbContext.tbl_PM_Location on OU.OUPoolID equals l.LocationID
                                                                        where OU.BusinessGroupID == BusinessGroupID && l.Active == true
                                                                        select OU).FirstOrDefault();
                if (_BusinessGroup_OUPools != null)
                {
                    isDeleted = false;
                }
                else
                {
                    tbl_CNF_BusinessGroups _BusinessGroups = dbContext.tbl_CNF_BusinessGroups.Where(x => x.BusinessGroupID == BusinessGroupID).FirstOrDefault();
                    if (_BusinessGroups != null)
                    {
                        _BusinessGroups.Active = false;
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        #endregion MyRegion

        #region ExitProccessConfig

        public List<SeperationReasons> getSeperationReason()
        {
            int TagID = 2678;
            try
            {
                dbContext = new HRMSDBEntities();
                List<SeperationReasons> seperationReason = new List<SeperationReasons>();
                seperationReason = (from e in dbContext.tbl_HR_Reasons
                                    where e.TagID == TagID
                                    orderby e.Reason
                                    select new SeperationReasons
                                    {
                                        ReasonID = e.ReasonID,
                                        Reason = e.Reason,
                                        TagID = e.TagID,

                                        isChecked = false
                                    }).ToList();
                return seperationReason;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// To get Employee Details by employee id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<SeperationReasons> getReasonDetails(int? reasonId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<SeperationReasons> reasonDetails = new List<SeperationReasons>();

                if (reasonId != null)
                {
                    reasonDetails = (from e in dbContext.tbl_HR_Reasons
                                     join v in dbContext.v_tbl_HR_Reasons on e.ReasonID equals v.ReasonID
                                     where v.ReasonID == reasonId
                                     select new SeperationReasons
                                     {
                                         ReasonID = e.ReasonID,
                                         Reason = e.Reason,
                                         tag = v.Tag
                                     }).ToList();
                }
                return reasonDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public v_tbl_HR_Reasons getReason(int? reasonId)
        {
            dbContext = new HRMSDBEntities();
            v_tbl_HR_Reasons reasonRecord = dbContext.v_tbl_HR_Reasons.Where(x => x.ReasonID == reasonId).FirstOrDefault();
            return reasonRecord;
        }

        public bool DeleteReason(int reasonId)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_HR_Reasons reasonMaster = dbContext.tbl_HR_Reasons.Where(x => x.ReasonID == reasonId).FirstOrDefault();
                if (reasonMaster != null)
                {
                    dbContext.DeleteObject(reasonMaster);
                    isDeleted = true;
                    dbContext.SaveChanges();
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// To edit or add configuration reason for exit process
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExitViewModel SaveEditedReason(ExitViewModel model)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                // 2678 Tagid is for exit process
                int TagID = 2678;
                tbl_HR_Reasons reasonChange = dbContext.tbl_HR_Reasons.Where(c => c.ReasonID == model.ReasonID).FirstOrDefault();
                model.IsExisted = false;
                model.IsEdited = false;
                tbl_HR_Reasons addreason = new tbl_HR_Reasons();
                //if new record is added
                if (reasonChange == null)
                    addreason = dbContext.tbl_HR_Reasons.Where(c => c.Reason == model.Reason).FirstOrDefault();
                else // if record edited
                    addreason = dbContext.tbl_HR_Reasons.Where(c => c.Reason == model.Reason && c.ReasonID != model.ReasonID).FirstOrDefault();

                ExitViewModel objExit = new ExitViewModel();

                //if record already does not exists
                if ((addreason == null))
                {
                    //edit
                    if (reasonChange != null)
                    {
                        reasonChange.Reason = model.Reason;
                        reasonChange.ReasonID = model.ReasonID;
                        reasonChange.TagID = TagID;
                        model.IsEdited = true;
                    }
                    //new record
                    else
                    {
                        dbContext = new HRMSDBEntities();
                        tbl_HR_Reasons newRecord = new tbl_HR_Reasons();
                        {
                            newRecord.Reason = model.Reason;
                            newRecord.TagID = TagID;
                        }

                        dbContext.tbl_HR_Reasons.AddObject(newRecord);
                        model.IsEdited = true;
                    }
                    dbContext.SaveChanges();
                }
                //if record already  exists
                else
                {
                    model.IsExisted = true;
                }
                return model;
            }
            catch
            {
                throw;
            }
        }

        public List<SeperationForCheckList> getConfigSeperationCheckList()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<SeperationForCheckList> seperationReason = new List<SeperationForCheckList>();
                seperationReason = (from e in dbContext.tbl_Q_Questionnaire
                                    orderby e.QuestionnaireName
                                    select new SeperationForCheckList
                                    {
                                        QuestionnaireID = e.QuestionnaireID,
                                        QuestionnaireName = e.QuestionnaireName,
                                        RevisionID = e.RevisionId.HasValue ? e.RevisionId.Value : 0,
                                        QuestionnaireDescription = e.Description,
                                        isChecked = false
                                    }).ToList();
                return seperationReason;
            }
            catch
            {
                throw;
            }
        }

        public List<SeperationReasons> getConfigSeparationCheckListDetails(int? reasonId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<SeperationReasons> reasonDetails = new List<SeperationReasons>();

                if (reasonId != null)
                {
                    reasonDetails = (from e in dbContext.tbl_Q_Questionnaire
                                     join v in dbContext.v_tbl_Q_Questionnaire on e.QuestionnaireID equals v.QuestionnaireID
                                     where v.QuestionnaireID == reasonId
                                     select new SeperationReasons
                                     {
                                         ReasonID = e.QuestionnaireID,
                                         Reason = e.QuestionnaireName,
                                         QuestionnaireDescription = e.Description
                                     }).ToList();
                }
                return reasonDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ExitViewModel getCheckList(int? QuestionnaireID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                ExitViewModel reasonRecord = (from q in dbContext.tbl_Q_Questionnaire
                                              join r in dbContext.tbl_Q_Questionnaire_Revision on q.RevisionId equals r.RevisionId
                                              where q.QuestionnaireID == QuestionnaireID
                                              select new ExitViewModel
                                              {
                                                  QuestionnaireID = q.QuestionnaireID,
                                                  QuestionnaireName = q.QuestionnaireName,
                                                  QuestionnaireDescription = q.Description,
                                                  TagID = q.TagID,
                                                  RevisionID = q.RevisionId.HasValue ? q.RevisionId.Value : 0,
                                                  RevisionNo = q.RevisionNo.HasValue ? q.RevisionNo.Value : 0,
                                                  Reason = r.Reason
                                              }).FirstOrDefault();
                return reasonRecord;
            }
            catch
            {
                throw;
            }
        }

        public List<ReasonDetail> getReasonList()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ReasonDetail> ReasonDetails = (from r in dbContext.tbl_Q_Questionnaire_Revision
                                                    select new ReasonDetail
                                                    {
                                                        QuestionnaireID = r.QuestionnaireId.HasValue ? r.QuestionnaireId.Value : 0,
                                                        RevisionID = r.RevisionId,
                                                        RevisionNo = r.RevisionNo.HasValue ? r.RevisionNo.Value : 0,
                                                        Reason = r.Reason
                                                    }).ToList();
                return ReasonDetails;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteCheckList(int QueationnaireID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_Q_Questionnaire reasonMaster = dbContext.tbl_Q_Questionnaire.Where(x => x.QuestionnaireID == QueationnaireID).FirstOrDefault();
                if (reasonMaster != null)
                {
                    dbContext.DeleteObject(reasonMaster);
                    isDeleted = true;
                    dbContext.SaveChanges();
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveEditedCheckList(ExitViewModel model)
        {
            bool isEdited = false;
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_Q_Questionnaire CheckListChange = dbContext.tbl_Q_Questionnaire.Where(c => c.QuestionnaireID == model.QuestionnaireID).FirstOrDefault();
                tbl_Q_Questionnaire_Revision RevisionNo = dbContext.tbl_Q_Questionnaire_Revision.Where(x => x.RevisionId == model.RevisionID).FirstOrDefault();
                if (CheckListChange != null)
                {
                    CheckListChange.QuestionnaireName = model.QuestionnaireName;
                    CheckListChange.Description = model.QuestionnaireDescription;
                    CheckListChange.RevisionId = model.RevisionID;
                    CheckListChange.RevisionNo = RevisionNo.RevisionNo.HasValue ? RevisionNo.RevisionNo.Value : 0;
                    isEdited = true;
                }
                else
                {
                    dbContext = new HRMSDBEntities();
                    tbl_Q_Questionnaire newRecord = new tbl_Q_Questionnaire();
                    {
                        newRecord.QuestionnaireName = model.QuestionnaireName;
                        newRecord.Description = model.QuestionnaireDescription;
                        newRecord.RevisionId = model.RevisionID;
                        newRecord.RevisionNo = RevisionNo.RevisionNo.HasValue ? RevisionNo.RevisionNo.Value : 0;
                    };
                    dbContext.tbl_Q_Questionnaire.AddObject(newRecord);
                    isEdited = true;
                }
                dbContext.SaveChanges();
                return isEdited;
            }
            catch
            {
                throw;
            }
        }

        public List<SeperationChecklistRecord> getSeperationChecklist()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<SeperationChecklistRecord> separationChecklist = new List<SeperationChecklistRecord>();
                separationChecklist = (from e in dbContext.v_tbl_HR_ExitProcess_StageApprovers
                                       orderby e.ExitStageID
                                       select new SeperationChecklistRecord
                                       {
                                           OrderNo = e.OrderNumber,
                                           Stage = e.ExitStage,
                                           Approver = e.ApproverNames
                                       }).ToList();
                return separationChecklist;
            }
            catch
            {
                throw;
            }
        }

        public List<CheckList> getChecklistDetails()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<CheckList> checkListFor = new List<CheckList>();
                checkListFor = (from q in dbContext.tbl_Q_Questionnaire
                                where q.ShowRating == true
                                select new CheckList
                                {
                                    QuestionnaireID = q.QuestionnaireID,
                                    QuestionnaireName = q.QuestionnaireName,
                                    RevisionID = q.RevisionId.HasValue ? q.RevisionId.Value : 0
                                }).ToList();
                return checkListFor;
            }
            catch
            {
                throw;
            }
        }

        public List<FeedbackChk> getFeedbackChklist(int orderNumber)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<FeedbackChk> feedbackChk = new List<FeedbackChk>();
                feedbackChk = (from s in dbContext.tbl_HR_ExitProcess_StakeHolder
                               join e in dbContext.HRMS_tbl_PM_Employee on (s.EmployeeID ?? 0) equals e.EmployeeID
                               into ee
                               from x in ee.DefaultIfEmpty()
                               join r in dbContext.HRMS_tbl_PM_Role on (x.PostID ?? 0) equals r.RoleID
                               into rr
                               from y in rr.DefaultIfEmpty()
                               join HR in dbContext.tbl_HR_ExitProcess_StageApprovers on (s.EmployeeID ?? 1) equals (HR.ApproverID ?? 0)
                               into sa
                               from xy in
                                   (from h in sa where (h.stageID ?? 0) == orderNumber && (h.ExitInstanceID ?? 1) == 0 select h).DefaultIfEmpty()
                               where x.Status == false
                               select new FeedbackChk
                               {
                                   EmployeeID = x.EmployeeID == null ? 0 : x.EmployeeID,
                                   Name = x.EmployeeName ?? string.Empty,
                                   Role = y.RoleDescription ?? string.Empty,
                                   RoleID = y.RoleID,
                                   StageID = orderNumber,
                                   RevisionID = xy.RevisionID ?? 0,
                                   stageApproverID = xy.stageApproverID == null ? 0 : xy.stageApproverID,
                                   isChecked = false
                               }).ToList();

                List<FeedbackChk> checkedFeedBackList = new List<FeedbackChk>();
                checkedFeedBackList = (from a in dbContext.tbl_HR_ExitProcess_StageApprovers
                                       join e in dbContext.HRMS_tbl_PM_Employee on a.ApproverID equals e.EmployeeID into zy
                                       from x in zy.DefaultIfEmpty()
                                       join r in dbContext.HRMS_tbl_PM_Role on x.PostID equals r.RoleID into xx
                                       from z in xx.DefaultIfEmpty()
                                       join q in dbContext.tbl_Q_Questionnaire on a.QuestionnaireID equals q.QuestionnaireID into xy
                                       from y in xy.DefaultIfEmpty()
                                       where a.ExitInstanceID == 0 && a.stageID == orderNumber
                                       select new FeedbackChk
                                       {
                                           EmployeeID = x.EmployeeID == null ? 0 : x.EmployeeID,
                                           Name = x.EmployeeName ?? string.Empty,
                                           Role = z.RoleDescription ?? string.Empty,
                                           RoleID = z.RoleID == null ? 0 : z.RoleID,
                                           Checklist = a.QuestionnaireID ?? 0,
                                           ChecklistName = y.QuestionnaireName,
                                           RevisionID = a.RevisionID ?? 0,
                                           stageApproverID = a.stageApproverID == null ? 0 : a.stageApproverID,
                                           isChecked = true
                                       }).ToList();
                foreach (var item in feedbackChk)
                {
                    if (checkedFeedBackList.Any(feed => feed.EmployeeID == item.EmployeeID))
                    {
                        item.Role = checkedFeedBackList.Where(x => x.EmployeeID == item.EmployeeID).FirstOrDefault().Role;
                        item.Checklist = checkedFeedBackList.Where(x => x.EmployeeID == item.EmployeeID).FirstOrDefault().Checklist;
                        item.ChecklistName = checkedFeedBackList.Where(x => x.EmployeeID == item.EmployeeID).FirstOrDefault().ChecklistName;
                        item.isChecked = true;
                    }
                }
                return feedbackChk;
            }
            catch
            {
                throw;
            }
        }

        public List<StackHolderList> getStackHolderDetails()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<StackHolderList> stackHolderRecords = new List<StackHolderList>();
                stackHolderRecords = (from e in dbContext.HRMS_tbl_PM_Employee
                                      join s in dbContext.tbl_HR_ExitProcess_StakeHolder on e.EmployeeID equals s.EmployeeID
                                      join r in dbContext.HRMS_tbl_PM_Role on e.PostID equals r.RoleID
                                      join b in dbContext.tbl_CNF_BusinessGroups on e.BusinessGroupID equals b.BusinessGroupID
                                      join l in dbContext.tbl_PM_Location on e.LocationID equals l.LocationID
                                      orderby e.EmployeeName ascending
                                      select new StackHolderList
                                      {
                                          Employee = e.EmployeeName,
                                          EmployeeID = e.EmployeeID,
                                          Role = r.RoleDescription,
                                          BusinessGroup = b.BusinessGroup,
                                          OrganizationUnit = l.Location
                                      }).ToList();
                return stackHolderRecords;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteStackHolderRecord(int employeeID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_HR_ExitProcess_StakeHolder stackHolderMaster = dbContext.tbl_HR_ExitProcess_StakeHolder.Where(x => x.EmployeeID == employeeID).FirstOrDefault();
                if (stackHolderMaster != null)
                {
                    dbContext.DeleteObject(stackHolderMaster);
                    isDeleted = true;
                    dbContext.SaveChanges();
                }
                else
                {
                    isDeleted = false;
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        public List<StackHolderList> getSelectedStackHolder(int[] employeeID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<StackHolderList> selectedRecord = (from e in dbContext.v_HRMS_tbl_PM_Employee_Details
                                                        orderby e.EmployeeName ascending
                                                        select new StackHolderList
                                                        {
                                                            EmployeeID = e.EmployeeID,
                                                            Employee = e.EmployeeName,
                                                            BusinessGroup = e.BusinessGroup,
                                                            OrganizationUnit = e.OrganizationUnit,
                                                            Role = e.RoleDescription,
                                                            Designation = e.Designation,
                                                            Department = e.Department,
                                                            EmpStatus = e.EmployeeStatus,
                                                            Location = e.Location,
                                                            TotalExperiance = e.TotalExperience,
                                                            V2Experiance = e.CurrentExp,
                                                            isChecked = false
                                                        }).ToList();
                foreach (var item in selectedRecord)
                {
                    if (employeeID.Any(empID => empID == item.EmployeeID))
                        item.isChecked = true;
                    else
                        continue;
                }
                return selectedRecord;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveStakeHolderSelected(int[] collection)
        {
            bool isEdited = false;
            try
            {
                dbContext = new HRMSDBEntities();

                foreach (var item in collection)
                {
                    tbl_HR_ExitProcess_StakeHolder stackHolderAdd = new tbl_HR_ExitProcess_StakeHolder();
                    stackHolderAdd.StakeHolderID = 0;
                    stackHolderAdd.EmployeeID = item;
                    dbContext.tbl_HR_ExitProcess_StakeHolder.AddObject(stackHolderAdd);
                    dbContext.SaveChanges();
                    isEdited = true;
                }
                return isEdited;
            }
            catch
            {
                throw;
            }
        }

        public bool Save_HR_ExitProcess_StageApprovers(FeedbackCheckList item)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                if (item.stageID == 3 || item.stageID == 6 || item.stageID == 8)
                {
                    if (item.EmployeeId != 0 || item.RoleID != 0)
                    {
                        tbl_HR_ExitProcess_StageApprovers _ExitProcess_StageApprovers = dbContext.tbl_HR_ExitProcess_StageApprovers.Where(x => x.stageApproverID == item.StageApproverID && x.stageID == item.stageID && x.ApproverID == item.EmployeeId && x.RevisionID == item.RevisionID).FirstOrDefault();
                        tbl_Q_Questionnaire _Q_Questionnaire = dbContext.tbl_Q_Questionnaire.Where(x => x.QuestionnaireID == item.CheckListValue).FirstOrDefault();
                        int RevisionID = 0;
                        if (_Q_Questionnaire != null)
                            RevisionID = _Q_Questionnaire.RevisionId.HasValue ? _Q_Questionnaire.RevisionId.Value : 0;
                        else
                            RevisionID = 0;
                        if (_ExitProcess_StageApprovers != null)
                        {
                            _ExitProcess_StageApprovers.stageID = item.stageID;
                            _ExitProcess_StageApprovers.ApproverID = item.EmployeeId;
                            _ExitProcess_StageApprovers.QuestionnaireID = item.CheckListValue;
                            _ExitProcess_StageApprovers.RevisionID = RevisionID;
                            _ExitProcess_StageApprovers.ExitInstanceID = 0;
                            isAdded = true;
                        }
                        else
                        {
                            tbl_HR_ExitProcess_StageApprovers ExitProcess_StageApprovers = new tbl_HR_ExitProcess_StageApprovers();
                            ExitProcess_StageApprovers.stageID = item.stageID;
                            ExitProcess_StageApprovers.ApproverID = item.EmployeeId;
                            ExitProcess_StageApprovers.QuestionnaireID = item.CheckListValue;
                            ExitProcess_StageApprovers.RevisionID = RevisionID;
                            ExitProcess_StageApprovers.ExitInstanceID = 0;
                            dbContext.tbl_HR_ExitProcess_StageApprovers.AddObject(ExitProcess_StageApprovers);
                            isAdded = true;
                        }
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        isAdded = false;
                    }
                }
                else
                {
                    if (item.EmployeeId == 0 || item.CheckListValue == 0 || item.RoleID == 0 || item.stageID == 0)
                    {
                        isAdded = false;
                    }
                    else
                    {
                        tbl_HR_ExitProcess_StageApprovers _ExitProcess_StageApprovers = dbContext.tbl_HR_ExitProcess_StageApprovers.Where(x => x.stageApproverID == item.StageApproverID && x.stageID == item.stageID && x.ApproverID == item.EmployeeId && x.RevisionID == item.RevisionID).FirstOrDefault();
                        tbl_Q_Questionnaire _Q_Questionnaire = dbContext.tbl_Q_Questionnaire.Where(x => x.QuestionnaireID == item.CheckListValue).FirstOrDefault();
                        int RevisionID = 0;
                        if (_Q_Questionnaire != null)
                            RevisionID = _Q_Questionnaire.RevisionId.HasValue ? _Q_Questionnaire.RevisionId.Value : 0;
                        else
                            RevisionID = 0;
                        if (_ExitProcess_StageApprovers != null)
                        {
                            _ExitProcess_StageApprovers.stageID = item.stageID;
                            _ExitProcess_StageApprovers.ApproverID = item.EmployeeId;
                            _ExitProcess_StageApprovers.QuestionnaireID = item.CheckListValue;
                            _ExitProcess_StageApprovers.RevisionID = RevisionID;
                            _ExitProcess_StageApprovers.ExitInstanceID = 0;
                            isAdded = true;
                        }
                        else
                        {
                            tbl_HR_ExitProcess_StageApprovers ExitProcess_StageApprovers = new tbl_HR_ExitProcess_StageApprovers();
                            ExitProcess_StageApprovers.stageID = item.stageID;
                            ExitProcess_StageApprovers.ApproverID = item.EmployeeId;
                            ExitProcess_StageApprovers.QuestionnaireID = item.CheckListValue;
                            ExitProcess_StageApprovers.RevisionID = RevisionID;
                            ExitProcess_StageApprovers.ExitInstanceID = 0;
                            dbContext.tbl_HR_ExitProcess_StageApprovers.AddObject(ExitProcess_StageApprovers);
                            isAdded = true;
                        }
                        dbContext.SaveChanges();
                    }
                }
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public List<FeedbackChk> getFeedBackCheckListForstage(int stageID)
        {
            List<FeedbackChk> checkedFeedBackList = (from a in dbContext.tbl_HR_ExitProcess_StageApprovers
                                                     join e in dbContext.HRMS_tbl_PM_Employee on a.ApproverID equals e.EmployeeID into zy
                                                     from x in zy.DefaultIfEmpty()
                                                     join r in dbContext.HRMS_tbl_PM_Role on x.PostID equals r.RoleID into xx
                                                     from z in xx.DefaultIfEmpty()
                                                     join q in dbContext.tbl_Q_Questionnaire on a.QuestionnaireID equals q.QuestionnaireID into xy
                                                     from y in xy.DefaultIfEmpty()
                                                     where a.ExitInstanceID == 0 && a.stageID == stageID
                                                     select new FeedbackChk
                                                     {
                                                         EmployeeID = x.EmployeeID == null ? 0 : x.EmployeeID,
                                                         Name = x.EmployeeName ?? string.Empty,
                                                         Role = z.RoleDescription ?? string.Empty,
                                                         RoleID = z.RoleID == null ? 0 : z.RoleID,
                                                         Checklist = a.QuestionnaireID ?? 0,
                                                         RevisionID = a.RevisionID ?? 0,
                                                         stageApproverID = a.stageApproverID == null ? 0 : a.stageApproverID,
                                                         StageID = a.stageID.HasValue ? a.stageID.Value : 0,
                                                         isChecked = true
                                                     }).ToList();
            return checkedFeedBackList;
        }

        public bool deleteApprover(List<FeedbackChk> FinalDeleteList)
        {
            bool isDeleted = false;
            try
            {
                foreach (var item in FinalDeleteList)
                {
                    tbl_HR_ExitProcess_StageApprovers _HR_ExitProcess_StageApprovers = dbContext.tbl_HR_ExitProcess_StageApprovers.Where(x => x.ApproverID == item.EmployeeID && x.stageID == item.StageID && x.ExitInstanceID == 0).FirstOrDefault();
                    if (_HR_ExitProcess_StageApprovers != null)
                    {
                        dbContext.tbl_HR_ExitProcess_StageApprovers.DeleteObject(_HR_ExitProcess_StageApprovers);
                    }
                    dbContext.SaveChanges();
                    isDeleted = true;
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        #endregion ExitProccessConfig

        #region Configure Organization Unit

        public tbl_PM_Location getOrganizationUnitDetails(int locationId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PM_Location pm_location = dbContext.tbl_PM_Location.Where(x => x.LocationID == locationId).FirstOrDefault();
                return pm_location;
            }
            catch
            {
                throw;
            }
        }

        public List<DeliveryUnit> getDeliveryUnitByLocationId(int LocationID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<string> empty = new List<string>();
                List<DeliveryUnit> deliveryUnits = new List<DeliveryUnit>();
                deliveryUnits = (from bu in dbContext.tbl_CNF_BusinessGroups
                                 join ou in dbContext.tbl_CNF_BusinessGroup_OUPools on bu.BusinessGroupID equals ou.BusinessGroupID
                                 join l in dbContext.tbl_PM_Location on ou.OUPoolID equals l.LocationID
                                 join du in dbContext.tbl_PM_OUPool_ResourcePools on ou.OUPoolID equals du.OUPoolID
                                 join r in dbContext.HRMS_tbl_PM_ResourcePool on du.ResourcePoolID equals r.ResourcePoolID
                                 orderby r.ResourcePoolName ascending
                                 where l.LocationID == LocationID && r.Active == true
                                 select new DeliveryUnit
                                 {
                                     LocationID = l.LocationID,
                                     Location = l.Location,
                                     ResourcePoolID = r.ResourcePoolID,
                                     ResourcePoolCode = r.ResourcePoolCode,
                                     ResourcePoolName = r.ResourcePoolName,
                                     CreatedBy = r.CreatedBy,
                                     CreatedDate = r.CreatedDate,
                                     ModifiedBy = r.ModifiedBy,
                                     ModifiedDate = r.ModifiedDate,
                                     Active = r.Active.HasValue ? r.Active.Value : false,
                                     UniqueID = du.UniqueID,
                                     OUPoolID = du.OUPoolID,
                                     EmployeeName = empty
                                 }).ToList();
                return deliveryUnits;
            }
            catch
            {
                throw;
            }
        }

        public List<DeliveryTeam> getCurrentDTs(int ResourcePoolID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<string> empty = new List<string>();
                List<DeliveryTeam> deliveryTeams = new List<DeliveryTeam>();
                deliveryTeams = (from OU in dbContext.HRMS_tbl_PM_ResourcePool
                                 join OUsDT in dbContext.tbl_PM_ResourcePool_Teams on OU.ResourcePoolID equals OUsDT.ResourcePoolID
                                 join DT in dbContext.tbl_PM_GroupMaster on OUsDT.GroupID equals DT.GroupID
                                 orderby DT.GroupName ascending
                                 where OU.ResourcePoolID == ResourcePoolID && DT.Active == true
                                 select new DeliveryTeam
                                 {
                                     ResourcePoolID = OU.ResourcePoolID,
                                     ResourcePoolName = OU.ResourcePoolName,
                                     GroupID = DT.GroupID,
                                     GroupCode = DT.GroupCode,
                                     GroupName = DT.GroupName,
                                     CreatedBy = DT.CreatedBy,
                                     CreatedDate = DT.CreatedDate,
                                     ModifiedBy = DT.ModifiedBy,
                                     ModifiedDate = DT.ModifiedDate,
                                     ResourceHeadID = DT.ResourceHeadID,
                                     Active = DT.Active.HasValue ? DT.Active.Value : false
                                 }).ToList();
                return deliveryTeams;
            }
            catch
            {
                throw;
            }
        }

        public List<DocumentCategory> getOrganizationUnitDocuments(int locationId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<DocumentCategory> organizationUnitDocuments = new List<DocumentCategory>();

                organizationUnitDocuments = (from _location in dbContext.tbl_PM_Location
                                             join _locationDocument in dbContext.tbl_PM_Location_DocumentCategory
                                                 on _location.LocationID equals _locationDocument.LocationID
                                             join _documentCategory in dbContext.tbl_PM_DocumentCategory
                                                 on _locationDocument.CategoryID equals _documentCategory.CategoryID
                                             where _locationDocument.LocationID == locationId
                                             select new DocumentCategory
                                             {
                                                 LocationID = locationId,
                                                 CategoryID = _locationDocument.CategoryID.HasValue ? _locationDocument.CategoryID.Value : 0,
                                                 Category = _documentCategory.Category,
                                                 CreatedBy = _locationDocument.CreatedBy,
                                                 CreatedDate = _locationDocument.CreatedDate.Value,
                                                 ModifiedBy = _locationDocument.ModifiedBy,
                                                 ModifiedDate = _locationDocument.ModifiedDate,
                                                 Checked = false
                                             }).ToList();

                return organizationUnitDocuments;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DeliveryUnit> getOUDeliveryUnits(int locationId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<DeliveryUnit> OUdeliveryunitlist = (from _location in dbContext.tbl_PM_Location
                                                         join _oupool in dbContext.tbl_PM_OUPool_ResourcePools on _location.LocationID equals _oupool.OUPoolID
                                                         join _resourcepool in dbContext.HRMS_tbl_PM_ResourcePool on _oupool.ResourcePoolID equals _resourcepool.ResourcePoolID
                                                         where _oupool.OUPoolID == locationId && _resourcepool.Active == true
                                                         select new DeliveryUnit
                                                         {
                                                             ResourcePoolName = _resourcepool.ResourcePoolName,
                                                             ResourcePoolCode = _resourcepool.ResourcePoolCode,
                                                             ResourcePoolID = _oupool.ResourcePoolID.HasValue ? _oupool.ResourcePoolID.Value : 0,
                                                             OUPoolID = _oupool.OUPoolID.HasValue ? _oupool.OUPoolID.Value : 0,
                                                             LocationID = locationId
                                                         }).ToList();
                return OUdeliveryunitlist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool saveExistingDeliveryUnits(OrganizationStructure model)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                int ExistingDU = Convert.ToInt32(model.ExistingDU);
                tbl_PM_OUPool_ResourcePools BusinessGroup_OUPools = dbContext.tbl_PM_OUPool_ResourcePools.Where(x => x.ResourcePoolID == ExistingDU && x.OUPoolID == model.LocationID).FirstOrDefault();
                if (BusinessGroup_OUPools == null)
                {
                    tbl_PM_OUPool_ResourcePools _BusinessGroup_OUPools = new tbl_PM_OUPool_ResourcePools();
                    _BusinessGroup_OUPools.ResourcePoolID = ExistingDU;
                    _BusinessGroup_OUPools.OUPoolID = model.LocationID;
                    dbContext.tbl_PM_OUPool_ResourcePools.AddObject(_BusinessGroup_OUPools);
                    isAdded = true;

                    //tbl_PM_ResourcePool _PM_Location = dbContext.tbl_PM_ResourcePool.Where(x => x.ResourcePoolID == ExistingDU).FirstOrDefault();
                    //if (_PM_Location != null && _PM_Location.Active == false)
                    //{
                    //    _PM_Location.Active = true;
                    //}
                    dbContext.SaveChanges();
                }
                else
                    isAdded = false;
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public bool saveExistingDeliveryTeams(OrganizationStructure model)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                int ExistingDT = Convert.ToInt32(model.ExistingDT);
                tbl_PM_ResourcePool_Teams _ResourcePool_Teams = dbContext.tbl_PM_ResourcePool_Teams.Where(x => x.ResourcePoolID == model.ResourcePoolID && x.GroupID == ExistingDT).FirstOrDefault();
                if (_ResourcePool_Teams == null)
                {
                    tbl_PM_ResourcePool_Teams _ResourcePool_Team = new tbl_PM_ResourcePool_Teams();
                    _ResourcePool_Team.ResourcePoolID = model.ResourcePoolID;
                    _ResourcePool_Team.GroupID = ExistingDT;
                    dbContext.tbl_PM_ResourcePool_Teams.AddObject(_ResourcePool_Team);
                    isAdded = true;

                    //tbl_PM_GroupMaster _GroupMaster = dbContext.tbl_PM_GroupMaster.Where(x => x.GroupID == ExistingDT).FirstOrDefault();
                    //if (_GroupMaster != null && _GroupMaster.Active == false)
                    //{
                    //    _GroupMaster.Active = true;
                    //}
                    dbContext.SaveChanges();
                }
                else
                    isAdded = false;
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public List<ManagerList> getOUManagers(int locationId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ManagerList> OUManagerList = new List<ManagerList>();
                OUManagerList = (from _location in dbContext.tbl_PM_Location
                                 join _oupoolManager in dbContext.tbl_PM_OUPool_Managers
                                     on _location.LocationID equals _oupoolManager.OUPoolID
                                 join _employee in dbContext.HRMS_tbl_PM_Employee
                                     on _oupoolManager.ManagerID equals _employee.EmployeeID
                                 where _oupoolManager.OUPoolID == locationId
                                 select new ManagerList
                                 {
                                     EmployeeID = _employee.EmployeeID,
                                     UserName = _employee.UserName,
                                     EmployeeName = _employee.EmployeeName,
                                     LocationID = locationId,
                                     IsPrimaryResponsible = _oupoolManager.IsPrimaryResponsible.HasValue ? _oupoolManager.IsPrimaryResponsible.Value : false,
                                     Checked = false
                                 }).ToList();
                return OUManagerList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MiddleLevelResources> getOUMiddleLevelResource(int locationId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<MiddleLevelResources> middleLevelResources = (from _location in dbContext.tbl_PM_Location_MiddleLevelResources
                                                                   join _employee in dbContext.HRMS_tbl_PM_Employee on _location.EmployeeID equals _employee.EmployeeID
                                                                   join _role in dbContext.HRMS_tbl_PM_Role on _employee.PostID equals _role.RoleID into er
                                                                   from x in er.DefaultIfEmpty()
                                                                   where _location.LocationID == locationId && _employee.Status == false
                                                                   select new MiddleLevelResources
                                                                   {
                                                                       EmpoloyeeID = _employee.EmployeeID,
                                                                       EmployeeName = _employee.EmployeeName,
                                                                       Role = x.RoleDescription,
                                                                       EmailID = _employee.EmailID,
                                                                       LocationID = locationId,
                                                                       Checked = false
                                                                   }).ToList();
                return middleLevelResources;
            }
            catch
            {
                throw;
            }
        }

        public List<OrganizationCountryDetails> getCountryList()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<OrganizationCountryDetails> contryList = (from t in dbContext.tbl_PM_CountryMaster
                                                               select new OrganizationCountryDetails
                                                               {
                                                                   CountryId = t.CountryID,
                                                                   CountryName = t.CountryName
                                                               }).ToList();
                return contryList;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveOrganizationUnitDetails(OrganizationStructure model)
        {
            bool isAdded = false;
            try
            {
                tbl_PM_Location _PM_Location = dbContext.tbl_PM_Location.Where(x => x.LocationID == model.LocationID).FirstOrDefault();
                if (_PM_Location == null)
                {
                    tbl_PM_Location pm_location = new tbl_PM_Location()
                    {
                        Location = model.Location,
                        LocationCode = model.LocationCode,
                        Active = model.Active,
                        ShortCode = model.ShortCode,
                        Address = model.Address,
                        Address1 = model.Address1,
                        City = model.City,
                        Zip = model.Zip,
                        State = model.State,
                        CountryID = Convert.ToInt32(model.Country),
                        Phone1 = model.Phone1,
                        Phone2 = model.Phone2,
                        Fax = model.Fax,
                        Email = model.Email,
                        WorkingHours = model.WorkingHours,
                        WorkingDays = model.WorkingDays
                    };
                    dbContext.tbl_PM_Location.AddObject(pm_location);
                    isAdded = true;
                }
                else
                {
                    if (model.Active == false)
                    {
                        tbl_PM_OUPool_ResourcePools OUPool_ResourcePools = (from ou in dbContext.tbl_PM_OUPool_ResourcePools
                                                                            join r in dbContext.HRMS_tbl_PM_ResourcePool on ou.ResourcePoolID equals r.ResourcePoolID
                                                                            where ou.OUPoolID == model.LocationID && r.Active == true
                                                                            select ou).FirstOrDefault();
                        //dbContext.tbl_PM_OUPool_ResourcePools.Where(x => x.OUPoolID == model.LocationID).FirstOrDefault();
                        if (OUPool_ResourcePools != null)
                        {
                            isAdded = false;
                        }
                        else
                        {
                            _PM_Location.Location = model.Location;
                            _PM_Location.LocationCode = model.LocationCode;
                            _PM_Location.Active = model.Active;
                            _PM_Location.ShortCode = model.ShortCode;
                            _PM_Location.Address = model.Address;
                            _PM_Location.Address1 = model.Address1;
                            _PM_Location.City = model.City;
                            _PM_Location.Zip = model.Zip;
                            _PM_Location.State = model.State;
                            _PM_Location.CountryID = Convert.ToInt32(model.Country);
                            _PM_Location.Phone1 = model.Phone1;
                            _PM_Location.Phone2 = model.Phone2;
                            _PM_Location.Fax = model.Fax;
                            _PM_Location.Email = model.Email;
                            _PM_Location.WorkingHours = model.WorkingHours;
                            _PM_Location.WorkingDays = model.WorkingDays;
                            isAdded = true;
                        }
                    }
                    else
                    {
                        _PM_Location.Location = model.Location;
                        _PM_Location.LocationCode = model.LocationCode;
                        _PM_Location.Active = model.Active;
                        _PM_Location.ShortCode = model.ShortCode;
                        _PM_Location.Address = model.Address;
                        _PM_Location.Address1 = model.Address1;
                        _PM_Location.City = model.City;
                        _PM_Location.Zip = model.Zip;
                        _PM_Location.State = model.State;
                        _PM_Location.CountryID = Convert.ToInt32(model.Country);
                        _PM_Location.Phone1 = model.Phone1;
                        _PM_Location.Phone2 = model.Phone2;
                        _PM_Location.Fax = model.Fax;
                        _PM_Location.Email = model.Email;
                        _PM_Location.WorkingHours = model.WorkingHours;
                        _PM_Location.WorkingDays = model.WorkingDays;
                        isAdded = true;
                    }
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public tbl_PM_DocumentCategory getDocumentCategoryDetails(int categoryId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PM_DocumentCategory documentCategory = dbContext.tbl_PM_DocumentCategory.Where(d => d.CategoryID == categoryId).FirstOrDefault();
                return documentCategory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<OrganizationUnitDocumentList> getDocumentCategoryList(int[] categoryId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<OrganizationUnitDocumentList> documentList = (from d in dbContext.tbl_PM_DocumentCategory
                                                                   select new OrganizationUnitDocumentList
                                                                   {
                                                                       CategoryID = d.CategoryID,
                                                                       Category = d.Category
                                                                   }).ToList();

                List<OrganizationUnitDocumentList> documentListWithoutExistingDocuments = new List<OrganizationUnitDocumentList>();

                foreach (var document in documentList)
                {
                    if (categoryId.Any(_categoryID => _categoryID == document.CategoryID))
                        continue;
                    else
                        documentListWithoutExistingDocuments.Add(document);
                }
                return documentListWithoutExistingDocuments;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveOrganizationDocumentCategory(OrganizationStructure model)
        {
            bool isAdded = false;
            try
            {
                var categoryID = Convert.ToInt32(model.ddlCategory);
                tbl_PM_Location_DocumentCategory _tbl_LocationDocument = dbContext.tbl_PM_Location_DocumentCategory.Where(d => d.LocationID == model.LocationID && d.CategoryID == model.CategoryID).FirstOrDefault();
                if (_tbl_LocationDocument == null)
                {
                    tbl_PM_Location_DocumentCategory _tbl_locationdocument = new tbl_PM_Location_DocumentCategory
                    {
                        LocationID = model.LocationID,
                        CategoryID = categoryID,
                        CreatedDate = DateTime.Now,
                    };

                    dbContext.tbl_PM_Location_DocumentCategory.AddObject(_tbl_locationdocument);
                    isAdded = true;
                }
                else
                {
                    _tbl_LocationDocument.LocationID = model.LocationID;
                    _tbl_LocationDocument.CategoryID = categoryID;
                    _tbl_LocationDocument.ModifiedDate = DateTime.Now;
                    isAdded = true;
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool deleteDocumentCategories(int[] collection, int locationId)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PM_Location_DocumentCategory _pm_documentCategory = dbContext.tbl_PM_Location_DocumentCategory.Where(x => x.CategoryID == item && x.LocationID == locationId).FirstOrDefault();
                    if (_pm_documentCategory != null)
                    {
                        dbContext.DeleteObject(_pm_documentCategory);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        public HRMS_tbl_PM_ResourcePool getExistingOUDeliveryUnits(int resourcePoolId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                HRMS_tbl_PM_ResourcePool _resourcePool = dbContext.HRMS_tbl_PM_ResourcePool.Where(r => r.ResourcePoolID == resourcePoolId).FirstOrDefault();
                return _resourcePool;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveOUDeliveryUnit(OrganizationStructure model)
        {
            bool isAdded = false;
            try
            {
                HRMS_tbl_PM_ResourcePool _ResourcePool = new HRMS_tbl_PM_ResourcePool();
                if (model.ResourcePoolID == 0)
                {
                    _ResourcePool = dbContext.HRMS_tbl_PM_ResourcePool.Where(d => d.ResourcePoolCode == model.newResourcePoolCode || d.ResourcePoolName == model.newresourcePoolName).FirstOrDefault();
                    if (_ResourcePool == null)
                    {
                        HRMS_tbl_PM_ResourcePool _resourcepool = new HRMS_tbl_PM_ResourcePool
                        {
                            ResourcePoolCode = model.newResourcePoolCode,
                            ResourcePoolName = model.newresourcePoolName,
                            CreatedDate = DateTime.Now,
                            Active = true
                        };

                        dbContext.HRMS_tbl_PM_ResourcePool.AddObject(_resourcepool);
                        isAdded = true;
                        dbContext.SaveChanges();

                        int latestResourcePoolID = (from e in dbContext.HRMS_tbl_PM_ResourcePool
                                                    orderby e.ResourcePoolID descending
                                                    select e.ResourcePoolID).FirstOrDefault();
                        tbl_PM_OUPool_ResourcePools _oupoolResorcepool = dbContext.tbl_PM_OUPool_ResourcePools.Where(r => r.OUPoolID == model.LocationID && r.ResourcePoolID == latestResourcePoolID).FirstOrDefault();
                        if (_oupoolResorcepool == null)
                        {
                            tbl_PM_OUPool_ResourcePools _oupool = new tbl_PM_OUPool_ResourcePools
                            {
                                OUPoolID = model.LocationID,
                                ResourcePoolID = latestResourcePoolID
                            };
                            dbContext.tbl_PM_OUPool_ResourcePools.AddObject(_oupool);
                            isAdded = true;
                            dbContext.SaveChanges();
                        }
                    }
                    else
                        isAdded = false;
                }
                else
                {
                    _ResourcePool = dbContext.HRMS_tbl_PM_ResourcePool.Where(d => d.ResourcePoolID == model.ResourcePoolID).FirstOrDefault();
                    _ResourcePool.ResourcePoolCode = model.newResourcePoolCode;
                    _ResourcePool.ResourcePoolName = model.newresourcePoolName;
                    _ResourcePool.ModifiedDate = DateTime.Now;
                    isAdded = true;
                    dbContext.SaveChanges();
                }
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteOUDeliveryUnit(List<int> collection)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PM_ResourcePool_Teams _ResourcePool_Teams = (from r in dbContext.tbl_PM_ResourcePool_Teams
                                                                     join rp in dbContext.tbl_PM_GroupMaster on r.GroupID equals rp.GroupID
                                                                     where r.ResourcePoolID == item && rp.Active == true
                                                                     select r).FirstOrDefault();
                    //dbContext.tbl_PM_ResourcePool_Teams.Where(x => x.ResourcePoolID == item).FirstOrDefault();
                    if (_ResourcePool_Teams != null)
                    {
                        isDeleted = false;
                    }
                    else
                    {
                        HRMS_tbl_PM_ResourcePool _resourcePool = dbContext.HRMS_tbl_PM_ResourcePool.Where(x => x.ResourcePoolID == item).FirstOrDefault();
                        if (_resourcePool != null)
                        {
                            _resourcePool.Active = false;
                            dbContext.SaveChanges();
                            isDeleted = true;
                        }
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        public tbl_PM_OUPool_Managers getManagersDetails(int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PM_OUPool_Managers managerDetails = dbContext.tbl_PM_OUPool_Managers.Where(m => m.ManagerID == employeeId).FirstOrDefault();
                return managerDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ManagerList> getOUMangersList(int[] employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ManagerList> managersList = (from e in dbContext.HRMS_tbl_PM_Employee
                                                  where e.Status == false
                                                  select new ManagerList
                                                  {
                                                      EmployeeID = e.EmployeeID,
                                                      EmployeeName = e.EmployeeName
                                                  }).ToList();

                List<ManagerList> documentListWithoutExistingManager = new List<ManagerList>();
                foreach (var employee in managersList)
                {
                    if (employeeId.Any(_employeeID => _employeeID == employee.EmployeeID))
                        continue;
                    else
                        documentListWithoutExistingManager.Add(employee);
                }
                return documentListWithoutExistingManager;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveOrganizationUnitManager(OrganizationStructure model)
        {
            bool isAdded = false;
            try
            {
                var newManagerID = Convert.ToInt32(model.Manager);
                tbl_PM_OUPool_Managers _oupoolManagers = dbContext.tbl_PM_OUPool_Managers.Where(d => d.OUPoolID == model.LocationID && d.ManagerID == model.OldEmployeeID).FirstOrDefault();
                if (_oupoolManagers == null)
                {
                    tbl_PM_OUPool_Managers _OUPoolManagers = new tbl_PM_OUPool_Managers
                    {
                        OUPoolID = model.LocationID,
                        ManagerID = newManagerID,
                        IsPrimaryResponsible = model.IsPrimaryResponsible,
                        CreatedDate = DateTime.Now,
                    };
                    dbContext.tbl_PM_OUPool_Managers.AddObject(_OUPoolManagers);
                    isAdded = true;
                }
                else
                {
                    _oupoolManagers.OUPoolID = model.LocationID;
                    _oupoolManagers.ManagerID = newManagerID;
                    _oupoolManagers.IsPrimaryResponsible = model.IsPrimaryResponsible;
                    _oupoolManagers.ModifiedDate = DateTime.Now;
                    isAdded = true;
                }
                dbContext.SaveChanges();
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool deleteOUManagers(int[] collection, int locationId)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PM_OUPool_Managers _ouPoolManagers = dbContext.tbl_PM_OUPool_Managers.Where(x => x.ManagerID == item && x.OUPoolID == locationId).FirstOrDefault();
                    if (_ouPoolManagers != null)
                    {
                        dbContext.DeleteObject(_ouPoolManagers);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        public List<MiddleLevelResources> selectNewOUResouce(int[] collection, int LocationID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<MiddleLevelResources> selectNewOUResouce = (from e in dbContext.HRMS_tbl_PM_Employee
                                                                 join r in dbContext.HRMS_tbl_PM_Role on e.PostID equals r.RoleID into er
                                                                 from x in er.DefaultIfEmpty()
                                                                 where e.Status == false
                                                                 select new MiddleLevelResources
                                                                 {
                                                                     EmpoloyeeID = e.EmployeeID,
                                                                     EmployeeName = e.EmployeeName,
                                                                     Role = x.RoleDescription,
                                                                     EmailID = e.EmailID,
                                                                     LocationID = LocationID,
                                                                     Checked = false
                                                                 }).ToList();

                List<MiddleLevelResources> selectNewOUResouceWithoutExistingResource = new List<MiddleLevelResources>();
                foreach (var Resouce in selectNewOUResouce)
                {
                    if (collection.Any(_empID => _empID == Resouce.EmpoloyeeID))
                        continue;
                    else
                        selectNewOUResouceWithoutExistingResource.Add(Resouce);
                }
                return selectNewOUResouceWithoutExistingResource;
            }
            catch
            {
                throw;
            }
        }

        public List<ExistingDeliveryUnit> getExistingDU(int LocationID, List<DeliveryUnit> deliveryUnits)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ExistingDeliveryUnit> _ExistingOU = new List<ExistingDeliveryUnit>();
                _ExistingOU = (from l in dbContext.HRMS_tbl_PM_ResourcePool
                               join b in dbContext.tbl_PM_OUPool_ResourcePools on l.ResourcePoolID equals b.ResourcePoolID into xy
                               from x in xy.DefaultIfEmpty()
                               where (x.OUPoolID != LocationID || x.OUPoolID == null) && l.Active == true
                               select new ExistingDeliveryUnit
                               {
                                   ResourcePoolID = l.ResourcePoolID,
                                   ResourcePoolName = l.ResourcePoolName,
                                   LocationID = x.OUPoolID.HasValue ? x.OUPoolID.Value : 0
                               }).ToList();
                List<ExistingDeliveryUnit> _ExistingOUFiltered = new List<ExistingDeliveryUnit>();
                foreach (var item in _ExistingOU)
                {
                    if (deliveryUnits.Any(org => org.ResourcePoolID == item.ResourcePoolID))
                        continue;
                    else
                        _ExistingOUFiltered.Add(item);
                }
                return _ExistingOUFiltered.GroupBy(x => x.ResourcePoolID).Select(y => y.First()).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<ExistingDeliveryTeam> getExistingDT(int ResourcePoolID, List<DeliveryTeam> currentDU)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ExistingDeliveryTeam> _ExistingDT = new List<ExistingDeliveryTeam>();
                _ExistingDT = (from l in dbContext.tbl_PM_GroupMaster
                               join b in dbContext.tbl_PM_ResourcePool_Teams on l.GroupID equals b.GroupID into xy
                               from x in xy.DefaultIfEmpty()
                               where (x.ResourcePoolID != ResourcePoolID || x.ResourcePoolID == null) && l.Active == true
                               select new ExistingDeliveryTeam
                               {
                                   ResourcePoolID = x.ResourcePoolID.HasValue ? x.ResourcePoolID.Value : 0,
                                   GroupID = l.GroupID,
                                   GroupName = l.GroupName
                               }).ToList();

                List<ExistingDeliveryTeam> _ExistingDTFiltered = new List<ExistingDeliveryTeam>();
                foreach (var item in _ExistingDT)
                {
                    if (currentDU.Any(org => org.GroupID == item.GroupID))
                        continue;
                    else
                        _ExistingDTFiltered.Add(item);
                }
                return _ExistingDTFiltered.GroupBy(x => x.GroupID).Select(y => y.First()).ToList();
            }
            catch
            {
                throw;
            }
        }

        public bool saveOUMiddleLevelResouce(int[] collection, int LocationID)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PM_Location_MiddleLevelResources _LocationMiddleLevelResources = new tbl_PM_Location_MiddleLevelResources();
                    _LocationMiddleLevelResources.LocationID = LocationID;
                    _LocationMiddleLevelResources.EmployeeID = item;
                    dbContext.tbl_PM_Location_MiddleLevelResources.AddObject(_LocationMiddleLevelResources);
                    dbContext.SaveChanges();
                    isAdded = true;
                }
                return isAdded;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteOUMiddleLevelResource(List<int> collection, int LocationID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_PM_Location_MiddleLevelResources _locationMiddleLevelResource = dbContext.tbl_PM_Location_MiddleLevelResources.Where(x => x.EmployeeID == item && x.LocationID == LocationID).FirstOrDefault();
                    if (_locationMiddleLevelResource != null)
                    {
                        dbContext.DeleteObject(_locationMiddleLevelResource);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }

        #endregion Configure Organization Unit

        #region Access Rights Node Mapping

        public List<AccessRightsNodeMapping> GetAccessRightsNodeMapping()
        {
            List<AccessRightsNodeMapping> _accessRightsNode = new List<AccessRightsNodeMapping>();
            var _accessRightsNodeList = WSEMdbContext.GetAccessRightsNodeMapping_sp();
            _accessRightsNode = (from s in _accessRightsNodeList
                                 select new AccessRightsNodeMapping
                                 {
                                     //ID = s.ID,
                                     AccessRightID = s.AccessRightID,
                                     AccessRightName = s.AccessRightName,
                                     NodeID = s.NodeID,
                                     NodeName = s.NodeName,
                                     CanAdd = s.CanAdd,
                                     CanEdit = s.CanEdit,
                                     CanDelete = s.CanDelete,
                                     CanView = s.CanView
                                 }).ToList();
            return _accessRightsNode.ToList();
        }

        public List<Nodes> GetNodeList()
        {
            List<Nodes> _node = new List<Nodes>();
            var _accessNodeList = WSEMdbContext.GetNodeList_sp();
            _node = (from n in _accessNodeList
                     select new Nodes
                     {
                         NodeID = n.NodeID,
                         NodeName = n.NodeName
                     }).ToList();
            return _node.ToList();
        }

        public List<AccessRights> GetAccessRightsList()
        {
            List<AccessRights> _accessRights = new List<AccessRights>();
            var _accessRightsList = WSEMdbContext.GetAccessRightList_sp();
            _accessRights = (from a in _accessRightsList
                             select new AccessRights
                             {
                                 AccessRightID = a.AccessRightID,
                                 AccessRightName = a.AccessRightName
                             }).ToList();
            return _accessRights.ToList();
        }

        public bool UpdateRoleNodeMapping(string[] allRoleNodeList)
        {
            try
            {
                bool status = false;
                int counter = 0;
                for (int i = 0; i < allRoleNodeList.Length / 6; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                    WSEMdbContext.AddUpdateAccessRightsNodesMapping_sp(Convert.ToInt32(allRoleNodeList[counter]), Convert.ToInt32(allRoleNodeList[counter + 1]), Convert.ToBoolean(allRoleNodeList[counter + 2]), Convert.ToBoolean(allRoleNodeList[counter + 3]), Convert.ToBoolean(allRoleNodeList[counter + 4]), Convert.ToBoolean(allRoleNodeList[counter + 5]), Output);
                    status = Convert.ToBoolean(Output.Value);
                    counter = counter + 6;
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Access Rights Node Mapping

        #region Employee Node Mapping

        public List<Employees> GetAllActiveEmployeesList()
        {
            List<Employees> _employees = new List<Employees>();
            var _employeesList = WSEMdbContext.usp_Sel_EmployeeList();
            _employees = (from e in _employeesList
                          select new Employees
                          {
                              EmployeeCode = e.EmployeeCode,
                              EmployeeName = e.EmployeeName
                          }).ToList();
            return _employees.ToList();
        }

        public List<EmployeeNodeMapping> GetEmployeeNodeMapping()
        {
            List<EmployeeNodeMapping> _employeeNodeMapping = new List<EmployeeNodeMapping>();
            var _employeeNodeMappingList = WSEMdbContext.usp_Sel_EmployeeNodeMapping();
            _employeeNodeMapping = (from s in _employeeNodeMappingList
                                    select new EmployeeNodeMapping
                                    {
                                        EmployeeCode = s.EmployeeCode,
                                        EmployeeName = s.EmployeeName,
                                        NodeID = s.NodeID,
                                        NodeName = s.NodeName,
                                        CanAdd = s.CanAdd,
                                        CanEdit = s.CanEdit,
                                        CanDelete = s.CanDelete,
                                        CanView = s.CanView
                                    }).ToList();
            return _employeeNodeMapping.ToList();
        }

        public bool UpdateEmployeeNodeMapping(string[] allEmployeeNodeList)
        {
            try
            {
                bool status = false;
                int counter = 0;
                for (int i = 0; i < allEmployeeNodeList.Length / 4; i++)
                {
                    ObjectParameter Output = new ObjectParameter("Result", typeof(int));
                    WSEMdbContext.usp_Upd_EmployeeNodesMapping(Convert.ToInt32(allEmployeeNodeList[counter]), Convert.ToInt32(allEmployeeNodeList[counter + 1]), Convert.ToInt32(allEmployeeNodeList[counter + 2]), Convert.ToBoolean(allEmployeeNodeList[counter + 3]), Output);
                    status = Convert.ToBoolean(Output.Value);
                    counter = counter + 4;
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Employee Node Mapping

        public List<CheckListNames> getcheckListNames()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<CheckListNames> stackHolderRecords = new List<CheckListNames>();
                stackHolderRecords = (from e in dbContext.HRMS_tbl_PM_Employee
                                      join s in dbContext.tbl_HR_ExitProcess_StakeHolder on e.EmployeeID equals s.EmployeeID
                                      join r in dbContext.HRMS_tbl_PM_Role on e.PostID equals r.RoleID
                                      join b in dbContext.tbl_CNF_BusinessGroups on e.BusinessGroupID equals b.BusinessGroupID
                                      join l in dbContext.tbl_PM_Location on e.LocationID equals l.LocationID
                                      orderby e.EmployeeName ascending
                                      select new CheckListNames
                                      {
                                          Employee = e.EmployeeName,
                                          EmployeeID = e.EmployeeID,
                                          Role = r.RoleDescription,
                                          BusinessGroup = b.BusinessGroup,
                                          OrganizationUnit = l.Location
                                      }).ToList();
                return stackHolderRecords;
            }
            catch
            {
                throw;
            }
        }

        public List<FeedbackChk> getFeedbackChklistData(int orderNumber)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<FeedbackChk> checkedFeedBackList = new List<FeedbackChk>();
                checkedFeedBackList = (from a in dbContext.tbl_HR_ExitProcess_StageApprovers
                                       join e in dbContext.HRMS_tbl_PM_Employee on a.ApproverID equals e.EmployeeID into zy
                                       from x in zy.DefaultIfEmpty()
                                       join r in dbContext.HRMS_tbl_PM_Role on x.PostID equals r.RoleID into xx
                                       from z in xx.DefaultIfEmpty()
                                       join q in dbContext.tbl_Q_Questionnaire on a.QuestionnaireID equals q.QuestionnaireID into xy
                                       from y in xy.DefaultIfEmpty()
                                       where a.ExitInstanceID == 0 && a.stageID == orderNumber
                                       select new FeedbackChk
                                       {
                                           EmployeeID = x.EmployeeID == null ? 0 : x.EmployeeID,
                                           Name = x.EmployeeName ?? string.Empty,
                                           Role = z.RoleDescription ?? string.Empty,
                                           RoleID = z.RoleID == null ? 0 : z.RoleID,
                                           Checklist = a.QuestionnaireID ?? 0,
                                           ChecklistName = y.QuestionnaireName,
                                           RevisionID = a.RevisionID ?? 0,
                                           stageApproverID = a.stageApproverID == null ? 0 : a.stageApproverID,
                                           isChecked = true
                                       }).ToList();

                return checkedFeedBackList;
            }
            catch
            {
                throw;
            }
        }

        public string GetRoleDetails(int EmployeeID)
        {
            try
            {
                string details = (from e in dbContext.HRMS_tbl_PM_Employee
                                  join r in dbContext.HRMS_tbl_PM_Role on e.PostID equals r.RoleID
                                  where e.EmployeeID == EmployeeID
                                  select r.RoleDescription).FirstOrDefault();

                return details;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public SEMResponse SaveExitFeedbackCheckListDetails(exitFeedbackChecklistVM model, int? CheckListID, int? NameID, string LoggedUserName)
        {
            try
            {
                SEMResponse response = new SEMResponse();
                response.status = false;

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}