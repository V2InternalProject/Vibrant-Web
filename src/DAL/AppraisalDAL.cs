using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace HRMS.DAL
{
    public class AppraisalDAL
    {
        private HRMSDBEntities dbContext = new HRMSDBEntities();
        private EmployeeDAL dal = new EmployeeDAL();

        public List<AppraisalViewModel> ProjectAssignmentLoadGrid(int page, int rows, int employeeId, out int totalCount)
        {
            var projectAssignment = (from proj in dbContext.Tbl_Appraisal_Project_Assignment
                                     where proj.EmployeeId == employeeId
                                     orderby proj.FromDate descending
                                     select new AppraisalViewModel
                                     {
                                         ProjectFromDate = proj.FromDate.Value,
                                         ProjectToDate = proj.ToDate.Value,
                                         ProjectDescription = proj.ProjectDescription_Assignment,
                                         ProjectAchievment = proj.Achievements,
                                         SatisfactionId = proj.SatisfactionID.Value,
                                         ProjectManagerId = proj.ProjectMangerId.Value
                                     }).Skip((page - 1) * rows).Take(rows).ToList();
            totalCount = dbContext.Tbl_Appraisal_Project_Assignment.Where(x => x.EmployeeId == employeeId).Count();
            return projectAssignment;
        }

        public List<ProjectAchievementAppraisal> GetProjectAchievementAppraisalDetails(int? appraisalId, int page, int rows, out int totalCount)
        {
            try
            {
                List<ProjectAchievementAppraisal> projectAchievement = (from projachieve in dbContext.tbl_Appraisal_ProjectAchivement
                                                                        where projachieve.AppraisalID == appraisalId

                                                                        orderby projachieve.EmployeeID descending
                                                                        select new ProjectAchievementAppraisal
                                                                        {
                                                                            ProjAchvmntEmpID = projachieve.EmployeeID,
                                                                            ProjAchieveID = projachieve.ProjectID,
                                                                            AppraisalID = projachieve.AppraisalID,
                                                                            ProjectDesc = projachieve.ProjectDescription.Trim(),
                                                                            ProjectAchievements = projachieve.ProjectAchivement.Trim(),
                                                                            StartDate = projachieve.StartDate,
                                                                            EndDate = projachieve.EndDate,
                                                                            NameOfManager = projachieve.NameOfProjManager.Trim(),
                                                                            FileName = projachieve.FileName,
                                                                            FilePath = projachieve.FilePath
                                                                        }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from projachieve in dbContext.tbl_Appraisal_ProjectAchivement
                              where projachieve.AppraisalID == appraisalId
                              select projachieve.EmployeeID).Count();

                return projectAchievement;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveProjectAchievementAppraisalDetails(ProjectAchievementAppraisal empCorporate)
        {
            bool isAdded = false;
            int loggedInEmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);
            tbl_Appraisal_ProjectAchivement emp = dbContext.tbl_Appraisal_ProjectAchivement.Where(ed => ed.ProjectID == empCorporate.ProjAchieveID && ed.AppraisalID == empCorporate.AppraisalID).FirstOrDefault();
            if (emp == null)
            {
                tbl_Appraisal_ProjectAchivement corporate = new tbl_Appraisal_ProjectAchivement();
                corporate.EmployeeID = empCorporate.ProjAchvmntEmpID.Value;
                corporate.AppraisalID = Convert.ToInt32(empCorporate.AppraisalID);
                corporate.ProjectDescription = empCorporate.ProjectDesc.Trim();
                corporate.ProjectAchivement = empCorporate.ProjectAchievements.Trim();
                corporate.StartDate = empCorporate.StartDate;
                corporate.EndDate = empCorporate.EndDate;
                corporate.NameOfProjManager = empCorporate.NameOfManager.Trim();
                corporate.FileName = empCorporate.FileName;
                corporate.FilePath = empCorporate.FilePath;
                corporate.CreatedBy = Convert.ToInt32(empCorporate.ProjAchvmntEmpID);
                corporate.ModifiedBy = loggedInEmployeeId;
                corporate.CreatedOn = DateTime.Now;
                corporate.ModifiedOn = DateTime.Now;
                dbContext.tbl_Appraisal_ProjectAchivement.AddObject(corporate);
            }
            else
            {
                emp.ProjectDescription = empCorporate.ProjectDesc.Trim();
                emp.ProjectAchivement = empCorporate.ProjectAchievements.Trim();
                emp.StartDate = empCorporate.StartDate;
                emp.EndDate = empCorporate.EndDate;
                emp.NameOfProjManager = empCorporate.NameOfManager.Trim();
                if (empCorporate.FileName != null && empCorporate.FilePath != null)
                {
                    emp.FileName = empCorporate.FileName;
                    emp.FilePath = empCorporate.FilePath;
                }
                emp.ModifiedBy = loggedInEmployeeId;
                emp.ModifiedOn = DateTime.Now;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool DeleteprojectAchievementAppraisalDetails(int ProjectAchievementID)
        {
            bool isDeleted = false;
            tbl_Appraisal_ProjectAchivement projectAchievementID = dbContext.tbl_Appraisal_ProjectAchivement.Where(cd => cd.ProjectID == ProjectAchievementID).FirstOrDefault();
            if (projectAchievementID != null)
            {
                dbContext.DeleteObject(projectAchievementID);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public List<AppraisalProcessModel> GetCorporateDetails(int appraisalId, int StageID, string IsManagerOrEMployee, string TextLink, int page, int rows, out int totalCount)
        {
            try
            {
                var CorporateDetails = dbContext.GetCorporateContribution_SP(appraisalId, StageID, IsManagerOrEMployee, TextLink);
                List<AppraisalProcessModel> corporateContribution = (from corporates in CorporateDetails
                                                                     select new AppraisalProcessModel
                                                                     {
                                                                         EmployeeID = corporates.EmployeeID,
                                                                         CorporateId = corporates.CorporateID,
                                                                         AreaOfContribution = corporates.AreaofContribution,
                                                                         ContributionDesc = corporates.ContributionDescription,
                                                                         Appraiser1Comments = corporates.Appraiser1Comments,
                                                                         Appraiser2Comments = corporates.Appraiser2Comments,
                                                                         Reviewer1Comments = corporates.Reviewer1Comments,
                                                                         Reviewer2Comments = corporates.Reviewer2Comments,
                                                                         GrpHeadComments = corporates.GroupHeadComments
                                                                     }).Skip((page - 1) * rows).Take(rows).ToList();

                //List<AppraisalProcessModel> corporateContribution = (from corporates in dbContext.tbl_Appraisal_CorporateContribution
                //                                                     where corporates.AppraisalID == appraisalId
                //                                                     orderby corporates.EmployeeID descending
                //                                                     select new AppraisalProcessModel
                //                                                     {
                //                                                         EmployeeID = corporates.EmployeeID,
                //                                                         CorporateId = corporates.CorporateID,
                //                                                         AreaOfContribution = corporates.AreaofContribution.Trim(),
                //                                                         ContributionDesc = corporates.ContributionDescription.Trim(),
                //                                                         Appraiser1Comments = corporates.Appraiser1Comments.Trim(),
                //                                                         Appraiser2Comments = corporates.Appraiser2Comments.Trim(),
                //                                                         Reviewer1Comments = corporates.Reviewer1Comments.Trim(),
                //                                                         Reviewer2Comments = corporates.Reviewer2Comments.Trim(),
                //                                                         GrpHeadComments = corporates.GroupHeadComments.Trim()
                //                                                     }).Skip((page - 1) * rows).Take(rows).ToList();

                totalCount = (from corporates in dbContext.tbl_Appraisal_CorporateContribution
                              where corporates.AppraisalID == appraisalId
                              select corporates.EmployeeID).Count();
                return corporateContribution;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveCorporateDetails(AppraisalProcessModel empCorporate)
        {
            bool isAdded = false;
            int loggedInEmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);
            int? appraisalId = (int?)empCorporate.appraisalId;
            tbl_Appraisal_CorporateContribution emp = dbContext.tbl_Appraisal_CorporateContribution.Where(ed => ed.CorporateID == empCorporate.CorporateId && ed.AppraisalID == empCorporate.appraisalId).FirstOrDefault();
            if (emp == null)
            {
                tbl_Appraisal_CorporateContribution corporate = new tbl_Appraisal_CorporateContribution();
                corporate.EmployeeID = empCorporate.EmployeeID.Value;
                corporate.AppraisalID = empCorporate.appraisalId;
                corporate.AreaofContribution = empCorporate.AreaOfContribution;
                corporate.ContributionDescription = empCorporate.ContributionDesc;
                corporate.CreatedBy = Convert.ToInt32(empCorporate.EmployeeID);
                corporate.ModifiedBy = loggedInEmployeeId;
                corporate.CreatedOn = DateTime.Now;
                corporate.ModifiedOn = DateTime.Now;
                if (empCorporate.IsManagerOrEmployee == "Employee")
                {
                    corporate.AreaofContribution = empCorporate.AreaOfContribution.Trim();
                    corporate.ContributionDescription = empCorporate.ContributionDesc.Trim();
                }
                if (empCorporate.IsManagerOrEmployee == "Appraiser1" && empCorporate.Appraiser1Comments != null)
                    corporate.Appraiser1Comments = empCorporate.Appraiser1Comments.Trim();
                if (empCorporate.IsManagerOrEmployee == "Appraiser2" && empCorporate.Appraiser2Comments != null)
                    corporate.Appraiser2Comments = empCorporate.Appraiser2Comments.Trim();
                if (empCorporate.IsManagerOrEmployee == "Reviewer1" && empCorporate.Reviewer1Comments != null)
                    corporate.Reviewer1Comments = empCorporate.Reviewer1Comments.Trim();
                if (empCorporate.IsManagerOrEmployee == "Reviewer2" && empCorporate.Reviewer2Comments != null)
                    corporate.Reviewer1Comments = empCorporate.Reviewer2Comments.Trim();
                if (empCorporate.IsManagerOrEmployee == "GroupHead" && empCorporate.GrpHeadComments != null)
                    corporate.GroupHeadComments = empCorporate.GrpHeadComments.Trim();
                dbContext.tbl_Appraisal_CorporateContribution.AddObject(corporate);
            }
            else
            {
                if (empCorporate.IsManagerOrEmployee == "Employee")
                {
                    emp.AreaofContribution = empCorporate.AreaOfContribution.Trim();
                    emp.ContributionDescription = empCorporate.ContributionDesc.Trim();
                }
                if (empCorporate.IsManagerOrEmployee == "Appraiser1" && empCorporate.Appraiser1Comments != null)
                    emp.Appraiser1Comments = empCorporate.Appraiser1Comments.Trim();
                if (empCorporate.IsManagerOrEmployee == "Appraiser2" && empCorporate.Appraiser2Comments != null)
                    emp.Appraiser2Comments = empCorporate.Appraiser2Comments.Trim();
                if (empCorporate.IsManagerOrEmployee == "Reviewer1" && empCorporate.Reviewer1Comments != null)
                    emp.Reviewer1Comments = empCorporate.Reviewer1Comments.Trim();
                if (empCorporate.IsManagerOrEmployee == "Reviewer2" && empCorporate.Reviewer2Comments != null)
                    emp.Reviewer2Comments = empCorporate.Reviewer2Comments.Trim();
                if (empCorporate.IsManagerOrEmployee == "GroupHead" && empCorporate.GrpHeadComments != null)
                    emp.GroupHeadComments = empCorporate.GrpHeadComments.Trim();
                else
                    emp.GroupHeadComments = empCorporate.GrpHeadComments;
                emp.ModifiedBy = loggedInEmployeeId;
                emp.ModifiedOn = DateTime.Now;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public tbl_Appraisal_AppraisalMaster GetAppraisalDetails(int appraisalId)
        {
            tbl_Appraisal_AppraisalMaster appraisalTable = new tbl_Appraisal_AppraisalMaster();
            try
            {
                appraisalTable = (from data in dbContext.tbl_Appraisal_AppraisalMaster
                                  where data.AppraisalID == appraisalId
                                  orderby data.AppraisalID descending
                                  select data).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return appraisalTable;
        }

        public ApraiserName GetAprraiserName(int? employeeID, int? AppraisalID)
        {
            ApraiserName appraisalTable = new ApraiserName();
            try
            {
                appraisalTable = (from data in dbContext.tbl_Appraisal_AppraisalMaster
                                  join employee in dbContext.HRMS_tbl_PM_Employee on data.EmployeeID equals employee.EmployeeID into emp
                                  from empName in emp.DefaultIfEmpty()
                                  join appraiser1 in dbContext.HRMS_tbl_PM_Employee on data.Appraiser1 equals appraiser1.EmployeeID into app1
                                  from app1Name in app1.DefaultIfEmpty()
                                  join appraiser2 in dbContext.HRMS_tbl_PM_Employee on data.Appraiser2 equals appraiser2.EmployeeID into app2
                                  from app2Name in app2.DefaultIfEmpty()
                                  join reviewer1 in dbContext.HRMS_tbl_PM_Employee on data.Reviewer1 equals reviewer1.EmployeeID into rev1
                                  from rev1Name in rev1.DefaultIfEmpty()
                                  join reviewer2 in dbContext.HRMS_tbl_PM_Employee on data.Reviewer2 equals reviewer2.EmployeeID into rev2
                                  from rev2Name in rev2.DefaultIfEmpty()
                                  join grpHead in dbContext.HRMS_tbl_PM_Employee on data.GroupHead equals grpHead.EmployeeID into grpHd
                                  from groupHead in grpHd.DefaultIfEmpty()
                                  where data.AppraisalID == AppraisalID
                                  orderby data.AppraisalID descending
                                  select new ApraiserName
                                  {
                                      EmployeeName = empName.EmployeeName,
                                      Appraiser1 = app1Name.EmployeeName,
                                      Appraiser2 = app2Name.EmployeeName,
                                      Reviewer1 = rev1Name.EmployeeName,
                                      Reviewer2 = rev2Name.EmployeeName,
                                      GroupHead = groupHead.EmployeeName
                                  }).FirstOrDefault();
            }
            catch (Exception)
            {
            }
            return appraisalTable;
        }

        public tbl_Appraisal_PerformanceHinders GetAppraisalPerformanceHinder(int appraisalId)
        {
            tbl_Appraisal_PerformanceHinders performHinder = new tbl_Appraisal_PerformanceHinders();
            try
            {
                performHinder = dbContext.tbl_Appraisal_PerformanceHinders.OrderByDescending(ph => ph.CreatedOn).Where(ph => ph.AppraisalID == appraisalId).FirstOrDefault();
            }
            catch (Exception)
            {
            }
            return performHinder;
        }

        public List<Parameters> GetParameters(int? designationID, int AppID)
        {
            List<Parameters> ListParm = (
                                         from pm in dbContext.tbl_Appraisal_ParameterMaster
                                         join p in dbContext.tbl_Appraisal_ParameterDesignationMapping on pm.ParameterID equals p.ParameterID
                                         join y in dbContext.tbl_Appraisal_YearMaster on pm.AppraisalYearID equals y.AppraisalYearID
                                         where p.DesignationID == designationID && y.AppraisalYearStatus == 0

                                         select new Parameters
                                         {
                                             parmID = pm.ParameterID,
                                             ParameterDesc = pm.Parameter
                                         }).ToList();
            return ListParm.ToList();
        }

        public List<Parameters> GetParametersByEmployeeDesignation(string employeeCode)
        {
            List<Parameters> ListParm = (from pm in dbContext.tbl_Appraisal_ParameterMaster
                                         join p in dbContext.tbl_Appraisal_ParameterDesignationMapping on pm.ParameterID equals p.ParameterID
                                         join e in dbContext.HRMS_tbl_PM_Employee on p.DesignationID equals e.DesignationID
                                         where e.EmployeeCode == employeeCode
                                         select new Parameters
                                         {
                                             ParameterDesc = pm.Parameter
                                         }).ToList();
            return ListParm.ToList();
        }

        public List<Designation> GetDesignationList()
        {
            List<Designation> desigList = (from d in dbContext.tbl_PM_DesignationMaster
                                           orderby d.DesignationName
                                           select new Designation
                                           {
                                               DesignationID = d.DesignationID,
                                               DesignationDesc = d.DesignationName
                                           }).ToList();
            return desigList.ToList();
        }

        public tbl_Appraisal_EmpGrowthSummary getEmpGrowthSummary(int? employeeId, int appraisalID)
        {
            tbl_Appraisal_EmpGrowthSummary empGrowthSummary = new tbl_Appraisal_EmpGrowthSummary();
            try
            {
                empGrowthSummary = (dbContext.tbl_Appraisal_EmpGrowthSummary.Where(ed => ed.EmployeeID == employeeId && ed.AppraisalID == appraisalID)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return empGrowthSummary;
        }

        public List<AdditionalQualificationAppraisal> GetAddQualificationAppraisalDetails(int appraisalId, int page, int rows, out int totalCount)
        {
            try
            {
                List<AdditionalQualificationAppraisal> skillAquired = (from qualification in dbContext.tbl_Appraisal_AdditionalQualification
                                                                       join typeMaster in dbContext.tbl_PM_QualificationType on qualification.Type equals typeMaster.QualificationTypeID into type
                                                                       from _typeMaster in type.DefaultIfEmpty()
                                                                       where qualification.AppraisalID == appraisalId
                                                                       orderby qualification.EmployeeID descending
                                                                       select new AdditionalQualificationAppraisal
                                                                       {
                                                                           QualifEmployeeID = qualification.EmployeeID,
                                                                           AddQualificationID = qualification.AddQualificationID,
                                                                           AppraisalID = qualification.AppraisalID,
                                                                           Title = qualification.Title.Trim(),
                                                                           FromDuration = qualification.FromDuration,
                                                                           ToDuration = qualification.ToDuration,
                                                                           skill = qualification.Type,
                                                                           typeName = _typeMaster.QualificationTypeName
                                                                       }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from qualification in dbContext.tbl_Appraisal_AdditionalQualification
                              where qualification.AppraisalID == appraisalId
                              select qualification.EmployeeID).Count();
                return skillAquired;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteQualificationAppraisalDetails(int AddQualificationID)
        {
            bool isDeleted = false;
            tbl_Appraisal_AdditionalQualification addQualificationID = dbContext.tbl_Appraisal_AdditionalQualification.Where(cd => cd.AddQualificationID == AddQualificationID).FirstOrDefault();
            if (addQualificationID != null)
            {
                dbContext.DeleteObject(addQualificationID);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public bool SaveAddQualificationAppraisalDetails(AdditionalQualificationAppraisal empCorporate)
        {
            bool isAdded = false;
            int loggedInEmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);
            tbl_Appraisal_AdditionalQualification emp = dbContext.tbl_Appraisal_AdditionalQualification.Where(ed => ed.AddQualificationID == empCorporate.AddQualificationID && ed.AppraisalID == empCorporate.AppraisalID).FirstOrDefault();
            if (emp == null)
            {
                if (empCorporate.skill == 7)
                {
                    tbl_Appraisal_AdditionalQualification corporate = new tbl_Appraisal_AdditionalQualification();
                    corporate.EmployeeID = empCorporate.QualifEmployeeID.Value;
                    corporate.AppraisalID = Convert.ToInt32(empCorporate.AppraisalID);
                    corporate.Type = empCorporate.skill;
                    corporate.CreatedBy = Convert.ToInt32(empCorporate.QualifEmployeeID);
                    corporate.ModifiedBy = loggedInEmployeeId;
                    corporate.CreatedOn = DateTime.Now;
                    corporate.ModifiedOn = DateTime.Now;
                    dbContext.tbl_Appraisal_AdditionalQualification.AddObject(corporate);
                    dbContext.SaveChanges();
                }
                else
                {
                    tbl_Appraisal_AdditionalQualification corporate = new tbl_Appraisal_AdditionalQualification();
                    corporate.EmployeeID = empCorporate.QualifEmployeeID.Value;
                    corporate.AppraisalID = Convert.ToInt32(empCorporate.AppraisalID);
                    corporate.Title = empCorporate.Title.Trim();
                    corporate.Type = empCorporate.skill;
                    corporate.FromDuration = empCorporate.FromDuration;
                    corporate.ToDuration = empCorporate.ToDuration;
                    corporate.CreatedBy = Convert.ToInt32(empCorporate.QualifEmployeeID);
                    corporate.ModifiedBy = loggedInEmployeeId;
                    corporate.CreatedOn = DateTime.Now;
                    corporate.ModifiedOn = DateTime.Now;
                    dbContext.tbl_Appraisal_AdditionalQualification.AddObject(corporate);
                    dbContext.SaveChanges();
                }
                //if (Convert.ToInt32(empCorporate.typeName) == 7)
                //{
                //    tbl_Appraisal_AdditionalQualification corporate = new tbl_Appraisal_AdditionalQualification();
                //    corporate.EmployeeID = empCorporate.QualifEmployeeID.Value;
                //    corporate.AppraisalID = Convert.ToInt32(empCorporate.AppraisalID);
                //    corporate.Type = Convert.ToInt32(empCorporate.typeName);
                //    corporate.CreatedBy = Convert.ToInt32(empCorporate.QualifEmployeeID);
                //    corporate.ModifiedBy = loggedInEmployeeId;
                //    corporate.CreatedOn = DateTime.Now;
                //    corporate.ModifiedOn = DateTime.Now;
                //    dbContext.tbl_Appraisal_AdditionalQualification.AddObject(corporate);
                //    dbContext.SaveChanges();
                //}
                //else
                //{
                //    tbl_Appraisal_AdditionalQualification corporate = new tbl_Appraisal_AdditionalQualification();
                //    corporate.EmployeeID = empCorporate.QualifEmployeeID.Value;
                //    corporate.AppraisalID = Convert.ToInt32(empCorporate.AppraisalID);
                //    corporate.Title = empCorporate.Title.Trim();
                //    corporate.Type = Convert.ToInt32(empCorporate.typeName);
                //    corporate.FromDuration = empCorporate.FromDuration;
                //    corporate.ToDuration = empCorporate.ToDuration;
                //    corporate.CreatedBy = Convert.ToInt32(empCorporate.QualifEmployeeID);
                //    corporate.ModifiedBy = loggedInEmployeeId;
                //    corporate.CreatedOn = DateTime.Now;
                //    corporate.ModifiedOn = DateTime.Now;
                //    dbContext.tbl_Appraisal_AdditionalQualification.AddObject(corporate);
                //    dbContext.SaveChanges();
                //}
            }
            else
            {
                if (empCorporate.skill == 7)
                {
                    emp.Title = null;
                    emp.FromDuration = null;
                    emp.ToDuration = null;
                    emp.Type = empCorporate.skill;
                    emp.ModifiedBy = loggedInEmployeeId;
                    emp.ModifiedOn = DateTime.Now;
                    dbContext.SaveChanges();
                }
                else
                {
                    emp.Title = empCorporate.Title.Trim();
                    emp.FromDuration = empCorporate.FromDuration;
                    emp.ToDuration = empCorporate.ToDuration;
                    emp.Type = empCorporate.skill;
                    emp.ModifiedBy = loggedInEmployeeId;
                    emp.ModifiedOn = DateTime.Now;
                    dbContext.SaveChanges();
                }

                //if (Convert.ToInt32(empCorporate.typeName) == 7)
                //{
                //    emp.Title = null;
                //    emp.FromDuration = null;
                //    emp.ToDuration = null;
                //    emp.Type = Convert.ToInt32(empCorporate.typeName);
                //    emp.ModifiedBy = loggedInEmployeeId;
                //    emp.ModifiedOn = DateTime.Now;
                //    dbContext.SaveChanges();
                //}
                //else
                //{
                //    emp.Title = empCorporate.Title.Trim();
                //    emp.FromDuration = empCorporate.FromDuration;
                //    emp.ToDuration = empCorporate.ToDuration;
                //    emp.Type = Convert.ToInt32(empCorporate.typeName);
                //    emp.ModifiedBy = loggedInEmployeeId;
                //    emp.ModifiedOn = DateTime.Now;
                //    dbContext.SaveChanges();
                //}
            }
            isAdded = true;
            return isAdded;
        }

        public string GetSkillDescription(int desc)
        {
            string qualifDescription;
            try
            {
                tbl_PM_QualificationType qualification = dbContext.tbl_PM_QualificationType.Where(qualificationType => qualificationType.QualificationTypeID == desc).FirstOrDefault();
                qualifDescription = qualification.QualificationTypeName.Trim();
            }
            catch (Exception)
            {
                throw;
            }
            return qualifDescription;
        }

        public List<SkillsAquiredAppraisal> GetSkillAquiredAppraisalDetails(int appraisalId, int page, int rows, out int totalCount)
        {
            try
            {
                List<SkillsAquiredAppraisal> skillAquired = (from skills in dbContext.tbl_Appraisal_SkillAquired
                                                             where skills.AppraisalID == appraisalId
                                                             orderby skills.EmployeeID descending
                                                             select new SkillsAquiredAppraisal
                                                             {
                                                                 SkillEmployeeID = skills.EmployeeID,
                                                                 SkillsAquiredID = skills.SkillAquiredID,
                                                                 AppraisalID = skills.AppraisalID,
                                                                 SkillName = skills.SkillName.Trim(),
                                                                 AquiredThrough = skills.SkillAquiredThrough.Trim(),
                                                                 ProjectUsefulness = skills.SkillUsefullness.Trim(),
                                                             }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from skills in dbContext.tbl_Appraisal_SkillAquired
                              where skills.AppraisalID == appraisalId
                              select skills.EmployeeID).Count();

                return skillAquired;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteskillAquiredAppraisalDetails(int SkillAquiredID)
        {
            bool isDeleted = false;
            tbl_Appraisal_SkillAquired skillAquiredID = dbContext.tbl_Appraisal_SkillAquired.Where(cd => cd.SkillAquiredID == SkillAquiredID).FirstOrDefault();
            if (skillAquiredID != null)
            {
                dbContext.DeleteObject(skillAquiredID);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public bool SaveSkillAquiredAppraisalDetails(SkillsAquiredAppraisal empCorporate)
        {
            bool isAdded = false;
            int loggedInEmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);
            tbl_Appraisal_SkillAquired emp = dbContext.tbl_Appraisal_SkillAquired.Where(ed => ed.SkillAquiredID == empCorporate.SkillsAquiredID && ed.AppraisalID == empCorporate.AppraisalID).FirstOrDefault();
            if (emp == null)
            {
                tbl_Appraisal_SkillAquired corporate = new tbl_Appraisal_SkillAquired();
                corporate.EmployeeID = empCorporate.SkillEmployeeID.Value;
                corporate.AppraisalID = Convert.ToInt32(empCorporate.AppraisalID);
                corporate.SkillName = empCorporate.SkillName.Trim();
                corporate.SkillAquiredThrough = empCorporate.AquiredThrough.Trim();
                corporate.SkillUsefullness = empCorporate.ProjectUsefulness.Trim();
                corporate.CreatedBy = Convert.ToInt32(empCorporate.SkillEmployeeID);
                corporate.ModifiedBy = loggedInEmployeeId;
                corporate.CreatedOn = DateTime.Now;
                corporate.ModifiedOn = DateTime.Now;
                dbContext.tbl_Appraisal_SkillAquired.AddObject(corporate);
            }
            else
            {
                emp.SkillName = empCorporate.SkillName.Trim();
                emp.SkillAquiredThrough = empCorporate.AquiredThrough.Trim();
                emp.SkillUsefullness = empCorporate.ProjectUsefulness.Trim();
                emp.ModifiedBy = loggedInEmployeeId;
                emp.ModifiedOn = DateTime.Now;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool SavePerformanceHinderAppraisalDetails(PerformanceHinderAppraisal performanceHinder)
        {
            bool isAdded = false;
            int loggedInEmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);
            tbl_Appraisal_PerformanceHinders empPerformanceHinderDetails = dbContext.tbl_Appraisal_PerformanceHinders.Where(ed => ed.AppraisalID == performanceHinder.AppraisalID).FirstOrDefault();
            //tbl_CF_Confirmation conf = dbContext.tbl_CF_Confirmation.Where(ed => ed.EmployeeID == performanceHinder.empID && ed.ConfirmationID == performanceHinder.confID).FirstOrDefault();
            if (empPerformanceHinderDetails == null || empPerformanceHinderDetails.EmployeeID <= 0 || empPerformanceHinderDetails.PerformanceHinderID < 0)
            {
                tbl_Appraisal_PerformanceHinders performanceHinderDetails = new tbl_Appraisal_PerformanceHinders();
                performanceHinderDetails.EmployeeID = Convert.ToInt32(performanceHinder.EmpID);
                performanceHinderDetails.AppraisalID = Convert.ToInt32(performanceHinder.AppraisalID);
                performanceHinderDetails.CreatedBy = Convert.ToInt32(performanceHinder.EmpID);
                performanceHinderDetails.ModifiedBy = loggedInEmployeeId;
                performanceHinderDetails.CreatedOn = DateTime.Now;
                performanceHinderDetails.ModifiedOn = DateTime.Now;
                if (!string.IsNullOrEmpty(performanceHinder.EmployeeCommentsFFEnvi))
                    performanceHinderDetails.EmployeeCommentsFFEnvi = performanceHinder.EmployeeCommentsFFEnvi.Trim();
                else
                    performanceHinderDetails.EmployeeCommentsFFEnvi = performanceHinder.EmployeeCommentsFFEnvi;

                if (!string.IsNullOrEmpty(performanceHinder.EmployeeCommentsFFSelf))
                    performanceHinderDetails.EmployeeCommentsFFSelf = performanceHinder.EmployeeCommentsFFSelf.Trim();
                else
                    performanceHinderDetails.EmployeeCommentsFFSelf = performanceHinder.EmployeeCommentsFFSelf;

                if (!string.IsNullOrEmpty(performanceHinder.EmployeeCommentsIFEnvi))
                    performanceHinderDetails.EmployeeCommentsIFEnvi = performanceHinder.EmployeeCommentsIFEnvi.Trim();
                else
                    performanceHinderDetails.EmployeeCommentsIFEnvi = performanceHinder.EmployeeCommentsIFEnvi;

                if (!string.IsNullOrEmpty(performanceHinder.EmployeeCommentsIFSelf))
                    performanceHinderDetails.EmployeeCommentsIFSelf = performanceHinder.EmployeeCommentsIFSelf.Trim();
                else
                    performanceHinderDetails.EmployeeCommentsIFSelf = performanceHinder.EmployeeCommentsIFSelf;

                if (!string.IsNullOrEmpty(performanceHinder.EmployeeCommentsSupport))
                    performanceHinderDetails.EmployeeCommentsSupport = performanceHinder.EmployeeCommentsSupport.Trim();
                else
                    performanceHinderDetails.EmployeeCommentsSupport = performanceHinder.EmployeeCommentsSupport;

                dbContext.tbl_Appraisal_PerformanceHinders.AddObject(performanceHinderDetails);
            }
            else
            {
                if (performanceHinder.IsManagerOrEmployee == "Employee")
                {
                    if (!string.IsNullOrEmpty(performanceHinder.EmployeeCommentsFFEnvi))
                        empPerformanceHinderDetails.EmployeeCommentsFFEnvi = performanceHinder.EmployeeCommentsFFEnvi.Trim();
                    else
                        empPerformanceHinderDetails.EmployeeCommentsFFEnvi = performanceHinder.EmployeeCommentsFFEnvi;

                    if (!string.IsNullOrEmpty(performanceHinder.EmployeeCommentsFFSelf))
                        empPerformanceHinderDetails.EmployeeCommentsFFSelf = performanceHinder.EmployeeCommentsFFSelf.Trim();
                    else
                        empPerformanceHinderDetails.EmployeeCommentsFFSelf = performanceHinder.EmployeeCommentsFFSelf;

                    if (!string.IsNullOrEmpty(performanceHinder.EmployeeCommentsIFEnvi))
                        empPerformanceHinderDetails.EmployeeCommentsIFEnvi = performanceHinder.EmployeeCommentsIFEnvi.Trim();
                    else
                        empPerformanceHinderDetails.EmployeeCommentsIFEnvi = performanceHinder.EmployeeCommentsIFEnvi;

                    if (!string.IsNullOrEmpty(performanceHinder.EmployeeCommentsIFSelf))
                        empPerformanceHinderDetails.EmployeeCommentsIFSelf = performanceHinder.EmployeeCommentsIFSelf.Trim();
                    else
                        empPerformanceHinderDetails.EmployeeCommentsIFSelf = performanceHinder.EmployeeCommentsIFSelf;

                    if (!string.IsNullOrEmpty(performanceHinder.EmployeeCommentsSupport))
                        empPerformanceHinderDetails.EmployeeCommentsSupport = performanceHinder.EmployeeCommentsSupport.Trim();
                    else
                        empPerformanceHinderDetails.EmployeeCommentsSupport = performanceHinder.EmployeeCommentsSupport;
                }
                if (performanceHinder.IsManagerOrEmployee == "Appraiser1")
                {
                    if (!string.IsNullOrEmpty(performanceHinder.Appraiser1CommentsFFEnvi))
                        empPerformanceHinderDetails.Appraiser1CommentsFFEnvi = performanceHinder.Appraiser1CommentsFFEnvi.Trim();
                    else
                        empPerformanceHinderDetails.Appraiser1CommentsFFEnvi = performanceHinder.Appraiser1CommentsFFEnvi;

                    if (!string.IsNullOrEmpty(performanceHinder.Appraiser1CommentsFFSelf))
                        empPerformanceHinderDetails.Appraiser1CommentsFFSelf = performanceHinder.Appraiser1CommentsFFSelf.Trim();
                    else
                        empPerformanceHinderDetails.Appraiser1CommentsFFSelf = performanceHinder.Appraiser1CommentsFFSelf;

                    if (!string.IsNullOrEmpty(performanceHinder.Appraiser1CommentsIFEnvi))
                        empPerformanceHinderDetails.Appraiser1CommentsIFEnvi = performanceHinder.Appraiser1CommentsIFEnvi.Trim();
                    else
                        empPerformanceHinderDetails.Appraiser1CommentsIFEnvi = performanceHinder.Appraiser1CommentsIFEnvi;

                    if (!string.IsNullOrEmpty(performanceHinder.Appraiser1CommentsIFSelf))
                        empPerformanceHinderDetails.Appraiser1CommentsIFSelf = performanceHinder.Appraiser1CommentsIFSelf.Trim();
                    else
                        empPerformanceHinderDetails.Appraiser1CommentsIFSelf = performanceHinder.Appraiser1CommentsIFSelf;

                    if (!string.IsNullOrEmpty(performanceHinder.Appraiser1CommentsSupport))
                        empPerformanceHinderDetails.Appraiser1CommentsSupport = performanceHinder.Appraiser1CommentsSupport.Trim();
                    else
                        empPerformanceHinderDetails.Appraiser1CommentsSupport = performanceHinder.Appraiser1CommentsSupport;
                }
                if (performanceHinder.IsManagerOrEmployee == "Appraiser2")
                {
                    if (!string.IsNullOrEmpty(performanceHinder.Appraiser2CommentsFFEnvi))
                        empPerformanceHinderDetails.Appraiser2CommentsFFEnvi = performanceHinder.Appraiser2CommentsFFEnvi.Trim();
                    else
                        empPerformanceHinderDetails.Appraiser2CommentsFFEnvi = performanceHinder.Appraiser2CommentsFFEnvi;

                    if (!string.IsNullOrEmpty(performanceHinder.Appraiser2CommentsFFSelf))
                        empPerformanceHinderDetails.Appraiser2CommentsFFSelf = performanceHinder.Appraiser2CommentsFFSelf.Trim();
                    else
                        empPerformanceHinderDetails.Appraiser2CommentsFFSelf = performanceHinder.Appraiser2CommentsFFSelf;

                    if (!string.IsNullOrEmpty(performanceHinder.Appraiser2CommentsIFEnvi))
                        empPerformanceHinderDetails.Appraiser2CommentsIFEnvi = performanceHinder.Appraiser2CommentsIFEnvi.Trim();
                    else
                        empPerformanceHinderDetails.Appraiser2CommentsIFEnvi = performanceHinder.Appraiser2CommentsIFEnvi;

                    if (!string.IsNullOrEmpty(performanceHinder.Appraiser2CommentsIFSelf))
                        empPerformanceHinderDetails.Appraiser2CommentsIFSelf = performanceHinder.Appraiser2CommentsIFSelf.Trim();
                    else
                        empPerformanceHinderDetails.Appraiser2CommentsIFSelf = performanceHinder.Appraiser2CommentsIFSelf;

                    if (!string.IsNullOrEmpty(performanceHinder.Appraiser2CommentsSupport))
                        empPerformanceHinderDetails.Appraiser2CommentsSupport = performanceHinder.Appraiser2CommentsSupport.Trim();
                    else
                        empPerformanceHinderDetails.Appraiser2CommentsSupport = performanceHinder.Appraiser2CommentsSupport;
                }
                if (performanceHinder.IsManagerOrEmployee == "Reviewer1")
                {
                    if (!string.IsNullOrEmpty(performanceHinder.Reviewer1CommentsFFEnvi))
                        empPerformanceHinderDetails.Reviewer1CommentsFFEnvi = performanceHinder.Reviewer1CommentsFFEnvi.Trim();
                    else
                        empPerformanceHinderDetails.Reviewer1CommentsFFEnvi = performanceHinder.Reviewer1CommentsFFEnvi;

                    if (!string.IsNullOrEmpty(performanceHinder.Reviewer1CommentsFFSelf))
                        empPerformanceHinderDetails.Reviewer1CommentsFFSelf = performanceHinder.Reviewer1CommentsFFSelf.Trim();
                    else
                        empPerformanceHinderDetails.Reviewer1CommentsFFSelf = performanceHinder.Reviewer1CommentsFFSelf;

                    if (!string.IsNullOrEmpty(performanceHinder.Reviewer1CommentsIFEnvi))
                        empPerformanceHinderDetails.Reviewer1CommentsIFEnvi = performanceHinder.Reviewer1CommentsIFEnvi.Trim();
                    else
                        empPerformanceHinderDetails.Reviewer1CommentsIFEnvi = performanceHinder.Reviewer1CommentsIFEnvi;

                    if (!string.IsNullOrEmpty(performanceHinder.Reviewer1CommentsIFSelf))
                        empPerformanceHinderDetails.Reviewer1CommentsIFSelf = performanceHinder.Reviewer1CommentsIFSelf.Trim();
                    else
                        empPerformanceHinderDetails.Reviewer1CommentsIFSelf = performanceHinder.Reviewer1CommentsIFSelf;

                    if (!string.IsNullOrEmpty(performanceHinder.Reviewer1CommentsSupport))
                        empPerformanceHinderDetails.Reviewer1CommentsSupport = performanceHinder.Reviewer1CommentsSupport.Trim();
                    else
                        empPerformanceHinderDetails.Reviewer1CommentsSupport = performanceHinder.Reviewer1CommentsSupport;
                }
                if (performanceHinder.IsManagerOrEmployee == "Reviewer2")
                {
                    if (!string.IsNullOrEmpty(performanceHinder.Reviewer2CommentsFFEnvi))
                        empPerformanceHinderDetails.Reviewer2CommentsFFEnvi = performanceHinder.Reviewer2CommentsFFEnvi.Trim();
                    else
                        empPerformanceHinderDetails.Reviewer2CommentsFFEnvi = performanceHinder.Reviewer2CommentsFFEnvi;

                    if (!string.IsNullOrEmpty(performanceHinder.Reviewer2CommentsFFSelf))
                        empPerformanceHinderDetails.Reviewer2CommentsFFSelf = performanceHinder.Reviewer2CommentsFFSelf.Trim();
                    else
                        empPerformanceHinderDetails.Reviewer2CommentsFFSelf = performanceHinder.Reviewer2CommentsFFSelf;

                    if (!string.IsNullOrEmpty(performanceHinder.Reviewer2CommentsIFEnvi))
                        empPerformanceHinderDetails.Reviewer2CommentsIFEnvi = performanceHinder.Reviewer2CommentsIFEnvi.Trim();
                    else
                        empPerformanceHinderDetails.Reviewer2CommentsIFEnvi = performanceHinder.Reviewer2CommentsIFEnvi;

                    if (!string.IsNullOrEmpty(performanceHinder.Reviewer2CommentsIFSelf))
                        empPerformanceHinderDetails.Reviewer2CommentsIFSelf = performanceHinder.Reviewer2CommentsIFSelf.Trim();
                    else
                        empPerformanceHinderDetails.Reviewer2CommentsIFSelf = performanceHinder.Reviewer2CommentsIFSelf;

                    if (!string.IsNullOrEmpty(performanceHinder.Reviewer2CommentsSupport))
                        empPerformanceHinderDetails.Reviewer2CommentsSupport = performanceHinder.Reviewer2CommentsSupport.Trim();
                    else
                        empPerformanceHinderDetails.Reviewer2CommentsSupport = performanceHinder.Reviewer2CommentsSupport;
                }
                //if (performanceHinder.IsManagerOrEmployee == "HR" && conf.stageID != 6)
                if (performanceHinder.IsManagerOrEmployee == "GroupHead")
                {
                    if (!string.IsNullOrEmpty(performanceHinder.GroupHeadCommentsFFEnvi))
                        empPerformanceHinderDetails.GroupHeadCommentsFFEnvi = performanceHinder.GroupHeadCommentsFFEnvi.Trim();
                    else
                        empPerformanceHinderDetails.GroupHeadCommentsFFEnvi = performanceHinder.GroupHeadCommentsFFEnvi;

                    if (!string.IsNullOrEmpty(performanceHinder.GroupHeadCommentsFFSelf))
                        empPerformanceHinderDetails.GroupHeadCommentsFFSelf = performanceHinder.GroupHeadCommentsFFSelf.Trim();
                    else
                        empPerformanceHinderDetails.GroupHeadCommentsFFSelf = performanceHinder.GroupHeadCommentsFFSelf;

                    if (!string.IsNullOrEmpty(performanceHinder.GroupHeadCommentsIFEnvi))
                        empPerformanceHinderDetails.GroupHeadCommentsIFEnvi = performanceHinder.GroupHeadCommentsIFEnvi.Trim();
                    else
                        empPerformanceHinderDetails.GroupHeadCommentsIFEnvi = performanceHinder.GroupHeadCommentsIFEnvi;

                    if (!string.IsNullOrEmpty(performanceHinder.GroupHeadCommentsIFSelf))
                        empPerformanceHinderDetails.GroupHeadCommentsIFSelf = performanceHinder.GroupHeadCommentsIFSelf.Trim();
                    else
                        empPerformanceHinderDetails.GroupHeadCommentsIFSelf = performanceHinder.GroupHeadCommentsIFSelf;

                    if (!string.IsNullOrEmpty(performanceHinder.GroupHeadCommentsSupport))
                        empPerformanceHinderDetails.GroupHeadCommentsSupport = performanceHinder.GroupHeadCommentsSupport.Trim();
                    else
                        empPerformanceHinderDetails.GroupHeadCommentsSupport = performanceHinder.GroupHeadCommentsSupport;
                }
                empPerformanceHinderDetails.ModifiedBy = loggedInEmployeeId;
                empPerformanceHinderDetails.ModifiedOn = DateTime.Now;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool SaveGoalAspireAppraisalDetails(GoalAquireAppraisal empCorporate)
        {
            bool isAdded = false;
            int loggedInEmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);
            tbl_Appraisal_GoalAspire emp = dbContext.tbl_Appraisal_GoalAspire.Where(ed => ed.EmployeeID == empCorporate.EmployeIDGoal && ed.AppraisalID == empCorporate.AppraisalIDGoal).FirstOrDefault();
            if (emp == null)
            {
                tbl_Appraisal_GoalAspire corporate = new tbl_Appraisal_GoalAspire();
                corporate.EmployeeID = Convert.ToInt32(empCorporate.EmployeIDGoal);
                corporate.AppraisalID = Convert.ToInt32(empCorporate.AppraisalIDGoal);
                if (!string.IsNullOrEmpty(empCorporate.ShortTerm))
                    corporate.ShortTermGoal = empCorporate.ShortTerm.Trim();
                else
                    corporate.ShortTermGoal = empCorporate.ShortTerm;

                if (!string.IsNullOrEmpty(empCorporate.LongTerm))
                    corporate.LongTermGoal = empCorporate.LongTerm.Trim();
                else
                    corporate.LongTermGoal = empCorporate.LongTerm;

                if (!string.IsNullOrEmpty(empCorporate.SkillDevPrgm))
                    corporate.SkillDevPrgm = empCorporate.SkillDevPrgm.Trim();
                else
                    corporate.SkillDevPrgm = empCorporate.SkillDevPrgm;
                corporate.CreatedBy = Convert.ToInt32(empCorporate.EmployeIDGoal);
                corporate.ModifiedBy = loggedInEmployeeId;
                corporate.CreatedOn = DateTime.Now;
                corporate.ModifiedOn = DateTime.Now;
                dbContext.tbl_Appraisal_GoalAspire.AddObject(corporate);
            }
            else
            {
                if (!string.IsNullOrEmpty(empCorporate.ShortTerm))
                    emp.ShortTermGoal = empCorporate.ShortTerm.Trim();
                else
                    emp.ShortTermGoal = empCorporate.ShortTerm;

                if (!string.IsNullOrEmpty(empCorporate.LongTerm))
                    emp.LongTermGoal = empCorporate.LongTerm.Trim();
                else
                    emp.LongTermGoal = empCorporate.LongTerm;

                if (!string.IsNullOrEmpty(empCorporate.SkillDevPrgm))
                    emp.SkillDevPrgm = empCorporate.SkillDevPrgm.Trim();
                else
                    emp.SkillDevPrgm = empCorporate.SkillDevPrgm;
                emp.ModifiedBy = loggedInEmployeeId;
                emp.ModifiedOn = DateTime.Now;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public List<ReviewerDetails> GetReviewerDetails(int loggedInEmployeeId, int appraisalYearId)
        {
            List<ReviewerDetails> reviewerDetails = (from appraiser in dbContext.tbl_Appraisal_AppraisalMaster
                                                     join stage in dbContext.tbl_Appraisal_Stages on appraiser.AppraisalStageID equals stage.AppraisalStageID into stagelist
                                                     from stages in stagelist.DefaultIfEmpty()
                                                     join employee in dbContext.HRMS_tbl_PM_Employee on appraiser.EmployeeID equals employee.EmployeeID into emplist
                                                     from emp in emplist.DefaultIfEmpty()
                                                     join delivery in dbContext.tbl_PM_GroupMaster on emp.GroupID equals delivery.GroupID into deiliverylist
                                                     from deliveryTeam in deiliverylist.DefaultIfEmpty()
                                                     join designation in dbContext.tbl_PM_DesignationMaster on emp.DesignationID equals designation.DesignationID into designationlist
                                                     from empDesignation in designationlist.DefaultIfEmpty()
                                                     where (appraiser.Reviewer2 == loggedInEmployeeId || appraiser.Reviewer1 == loggedInEmployeeId) && appraiser.AppraisalYearID == appraisalYearId
                                                     orderby appraiser.EmployeeID
                                                     select new ReviewerDetails
                                                     {
                                                         Reviewer = emp.EmployeeName,
                                                         ReviewerEmployeeCode = emp.EmployeeCode,
                                                         ReviewerDeliveryTeam = deliveryTeam.GroupName,
                                                         ReviewerDesignation = empDesignation.DesignationName,
                                                         ReviewerStage = stages.AppraisalStage
                                                     }).ToList();
            return reviewerDetails;
        }

        public List<GroupHeadDetails> GetGroupHeadDetails(int loggedInEmployeeId, int appraisalYearId)
        {
            List<GroupHeadDetails> groupHeadDetails = (from appraiser in dbContext.tbl_Appraisal_AppraisalMaster
                                                       join stage in dbContext.tbl_Appraisal_Stages on appraiser.AppraisalStageID equals stage.AppraisalStageID into stagelist
                                                       from stages in stagelist.DefaultIfEmpty()
                                                       join employee in dbContext.HRMS_tbl_PM_Employee on appraiser.EmployeeID equals employee.EmployeeID into emplist
                                                       from emp in emplist.DefaultIfEmpty()
                                                       join delivery in dbContext.tbl_PM_GroupMaster on emp.GroupID equals delivery.GroupID into deiliverylist
                                                       from deliveryTeam in deiliverylist.DefaultIfEmpty()
                                                       join designation in dbContext.tbl_PM_DesignationMaster on emp.DesignationID equals designation.DesignationID into designationlist
                                                       from empDesignation in designationlist.DefaultIfEmpty()
                                                       where appraiser.GroupHead == loggedInEmployeeId && appraiser.AppraisalYearID == appraisalYearId
                                                       orderby appraiser.EmployeeID
                                                       select new GroupHeadDetails
                                                       {
                                                           GroupHead = emp.EmployeeName,
                                                           GroupHeadEmployeeCode = emp.EmployeeCode,
                                                           GroupHeadDeliveryTeam = deliveryTeam.GroupName,
                                                           GroupHeadDesignation = empDesignation.DesignationName,
                                                           GroupHeadStage = stages.AppraisalStage
                                                       }).ToList();
            return groupHeadDetails;
        }

        public List<AppraiserDetails> GetAppraiserDetails(int loggedInEmployeeId, int appraisalYearId)
        {
            List<AppraiserDetails> appraiserDetails = (from appraiser in dbContext.tbl_Appraisal_AppraisalMaster
                                                       join stage in dbContext.tbl_Appraisal_Stages on appraiser.AppraisalStageID equals stage.AppraisalStageID into stagelist
                                                       from stages in stagelist.DefaultIfEmpty()
                                                       join employee in dbContext.HRMS_tbl_PM_Employee on appraiser.EmployeeID equals employee.EmployeeID into emplist
                                                       from emp in emplist.DefaultIfEmpty()
                                                       join delivery in dbContext.tbl_PM_GroupMaster on emp.GroupID equals delivery.GroupID into deiliverylist
                                                       from deliveryTeam in deiliverylist.DefaultIfEmpty()
                                                       join designation in dbContext.tbl_PM_DesignationMaster on emp.DesignationID equals designation.DesignationID into designationlist
                                                       from empDesignation in designationlist.DefaultIfEmpty()
                                                       where (appraiser.Appraiser1 == loggedInEmployeeId || appraiser.Appraiser2 == loggedInEmployeeId) && appraiser.AppraisalYearID == appraisalYearId
                                                       orderby appraiser.EmployeeID
                                                       select new AppraiserDetails
                                                       {
                                                           Appraiser = emp.EmployeeName,
                                                           AppraiserEmployeeCode = emp.EmployeeCode,
                                                           AppraiserDeliveryTeam = deliveryTeam.GroupName,
                                                           AppraiserDesignation = empDesignation.DesignationName,
                                                           AppraiserStage = stages.AppraisalStage
                                                       }).ToList();
            return appraiserDetails;
        }

        public List<AppraisalYearDetails> GetAppraisalYears()
        {
            List<AppraisalYearDetails> appraisalYears = (from appraisal in dbContext.tbl_Appraisal_YearMaster
                                                         orderby appraisal.AppraisalYear
                                                         select new AppraisalYearDetails
                                                         {
                                                             AppraisalYearId = appraisal.AppraisalYearID,
                                                             CurrentAppraisalYear = appraisal.AppraisalYear
                                                         }).ToList();

            return appraisalYears;
        }

        public int GetCurrentYear()
        {
            tbl_Appraisal_YearMaster currentYear = dbContext.tbl_Appraisal_YearMaster.Where(ed => ed.AppraisalYearStatus == 0).FirstOrDefault();
            if (currentYear != null)
                return currentYear.AppraisalYearID;
            else return 0;
        }

        public bool SavAppraisalProcressThreeDetails(List<AppraisalEmployeeGrowthSummary> app)
        {
            AppraisalEmployeeGrowthSummary model = app.FirstOrDefault();
            bool isAdded = false;
            int AppID = model.AppraisalID;
            int empID = model.EmployeeID;
            tbl_Appraisal_EmpGrowthSummary emp = dbContext.tbl_Appraisal_EmpGrowthSummary.Where(e => e.AppraisalID == AppID).FirstOrDefault();

            if (emp != null)
            {
                if (model.UserInRole == UserInRole.Reviewer1)
                {
                    if (!string.IsNullOrEmpty(model.Reviewer1CommentsReadyNow))
                        emp.Reviewer1CommentsReadyNow = model.Reviewer1CommentsReadyNow.Trim();
                    else
                        emp.Reviewer1CommentsReadyNow = model.Reviewer1CommentsReadyNow;

                    if (!string.IsNullOrEmpty(model.Reviewer1Comments1to2years))
                        emp.Reviewer1Comments1to2years = model.Reviewer1Comments1to2years.Trim();
                    else
                        emp.Reviewer1Comments1to2years = model.Reviewer1Comments1to2years;

                    if (!string.IsNullOrEmpty(model.Reviewer1Comments3to5years))
                        emp.Reviewer1Comments3to5years = model.Reviewer1Comments3to5years.Trim();
                    else
                        emp.Reviewer1Comments3to5years = model.Reviewer1Comments3to5years;
                }
                if (model.UserInRole == UserInRole.Reviewer2)
                {
                    if (!string.IsNullOrEmpty(model.Reviewer2CommentsReadyNow))
                        emp.Reviewer2CommentsReadyNow = model.Reviewer2CommentsReadyNow.Trim();
                    else
                        emp.Reviewer2CommentsReadyNow = model.Reviewer2CommentsReadyNow;

                    if (!string.IsNullOrEmpty(model.Reviewer2Comments1to2years))
                        emp.Reviewer2Comments1to2years = model.Reviewer2Comments1to2years.Trim();
                    else
                        emp.Reviewer2Comments1to2years = model.Reviewer2Comments1to2years;

                    if (!string.IsNullOrEmpty(model.Reviewer2Comments3to5years))
                        emp.Reviewer2Comments3to5years = model.Reviewer2Comments3to5years.Trim();
                    else
                        emp.Reviewer2Comments3to5years = model.Reviewer2Comments3to5years;
                }
                if (model.UserInRole == UserInRole.GroupHead)
                {
                    if (!string.IsNullOrEmpty(model.GroupHeadCommentsReadyNow))
                        emp.GroupHeadCommentsReadyNow = model.GroupHeadCommentsReadyNow.Trim();
                    else
                        emp.GroupHeadCommentsReadyNow = model.GroupHeadCommentsReadyNow;

                    if (!string.IsNullOrEmpty(model.GroupHeadComments1to2years))
                        emp.GroupHeadComments1to2years = model.GroupHeadComments1to2years.Trim();
                    else
                        emp.GroupHeadComments1to2years = model.GroupHeadComments1to2years;

                    if (!string.IsNullOrEmpty(model.GroupHeadComments3to5years))
                        emp.GroupHeadComments3to5years = model.GroupHeadComments3to5years.Trim();
                    else
                        emp.GroupHeadComments3to5years = model.GroupHeadComments3to5years;
                }
            }
            else
            {
                tbl_Appraisal_EmpGrowthSummary growth = new tbl_Appraisal_EmpGrowthSummary();
                growth.AppraisalID = model.AppraisalID;
                growth.EmployeeID = model.EmployeeID;

                if (model.UserInRole == UserInRole.Reviewer1)
                {
                    if (!string.IsNullOrEmpty(model.Reviewer1CommentsReadyNow))
                        growth.Reviewer1CommentsReadyNow = model.Reviewer1CommentsReadyNow.Trim();
                    else
                        growth.Reviewer1CommentsReadyNow = model.Reviewer1CommentsReadyNow;
                    if (!string.IsNullOrEmpty(model.Reviewer1Comments1to2years))
                        growth.Reviewer1Comments1to2years = model.Reviewer1Comments1to2years.Trim();
                    else
                        growth.Reviewer1Comments1to2years = model.Reviewer1Comments1to2years;
                    if (!string.IsNullOrEmpty(model.Reviewer1Comments3to5years))
                        growth.Reviewer1Comments3to5years = model.Reviewer1Comments3to5years.Trim();
                    else
                        growth.Reviewer1Comments3to5years = model.Reviewer1Comments3to5years;
                }
                if (model.UserInRole == UserInRole.Reviewer2)
                {
                    if (!string.IsNullOrEmpty(model.Reviewer2CommentsReadyNow))
                        growth.Reviewer2CommentsReadyNow = model.Reviewer2CommentsReadyNow.Trim();
                    else
                        growth.Reviewer2CommentsReadyNow = model.Reviewer2CommentsReadyNow;
                    if (!string.IsNullOrEmpty(model.Reviewer2Comments1to2years))
                        growth.Reviewer2Comments1to2years = model.Reviewer2Comments1to2years.Trim();
                    else
                        growth.Reviewer2Comments1to2years = model.Reviewer2Comments1to2years;
                    if (!string.IsNullOrEmpty(model.Reviewer2Comments3to5years))
                        growth.Reviewer2Comments3to5years = model.Reviewer2Comments3to5years.Trim();
                    else
                        growth.Reviewer2Comments3to5years = model.Reviewer2Comments3to5years;
                }

                if (model.UserInRole == UserInRole.GroupHead)
                {
                    if (!string.IsNullOrEmpty(model.GroupHeadCommentsReadyNow))
                        growth.GroupHeadCommentsReadyNow = model.GroupHeadCommentsReadyNow.Trim();
                    else
                        growth.GroupHeadCommentsReadyNow = model.GroupHeadCommentsReadyNow;
                    if (!string.IsNullOrEmpty(model.GroupHeadComments1to2years))
                        growth.GroupHeadComments1to2years = model.GroupHeadComments1to2years.Trim();
                    else
                        growth.GroupHeadComments1to2years = model.GroupHeadComments1to2years;
                    if (!string.IsNullOrEmpty(model.GroupHeadComments3to5years))
                        growth.GroupHeadComments3to5years = model.GroupHeadComments3to5years.Trim();
                    else
                        growth.GroupHeadComments3to5years = model.GroupHeadComments3to5years;
                }

                dbContext.tbl_Appraisal_EmpGrowthSummary.AddObject(growth);
            }

            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool SavePromotionRecommendationDetails(List<Parameters> model)
        {
            try
            {
                bool isAdded = false;
                if (model != null)
                {
                    int count = model.Count;
                    int? AppID = model[0].AppraisalID;
                    int empID = model[0].EmployeeID;
                    string UserRole = (model[0].UserInRole).ToString();

                    tbl_Appraisal_PromotionRecommendation recom = dbContext.tbl_Appraisal_PromotionRecommendation.Where(e => e.AppraisalID == AppID && e.EmployeeID == empID).FirstOrDefault();
                    //does not exist in db
                    if (recom == null)
                    {
                        tbl_Appraisal_PromotionRecommendation obj = new tbl_Appraisal_PromotionRecommendation();
                        obj.AppraisalID = model[0].AppraisalID;
                        obj.CurrentDesignationID = model[0].DesignationID;
                        obj.NextDesignationIDFromReviewer1 = model[0].NextDesignationIDFromReviewer1;
                        obj.NextDesignationIDfromReviewer2 = model[0].NextDesignationIDFromReviewer2;
                        obj.NextDesignationIDfromGroupHead = model[0].NextDesignationIDFromGroupHead;
                        obj.EmployeeID = model[0].EmployeeID;
                        if (UserRole == UserInRole.Reviewer1.ToString())
                        {
                            obj.OverallReviewer1Ratings = model[0].Reviewer1OverallRating;
                            obj.PromoRecombyReviewer1 = model[0].Reviewer1Recomendation;
                            if (!string.IsNullOrEmpty(model[0].Reviewer1RecomendationComments))
                                obj.PromoRecombyReviewer1Comments = model[0].Reviewer1RecomendationComments.Trim();
                            else
                                obj.PromoRecombyReviewer1Comments = model[0].Reviewer1RecomendationComments;
                            if (!string.IsNullOrEmpty(model[0].Reviewer1OverallRatingComments))
                                obj.OverallReviewer1Comments = model[0].Reviewer1OverallRatingComments.Trim();
                            else
                                obj.OverallReviewer1Comments = model[0].Reviewer1OverallRatingComments;
                        }
                        if (UserRole == UserInRole.Reviewer2.ToString())
                        {
                            obj.OverallReviewer2Ratings = model[0].Reviewer2OverallRating;
                            obj.PromoRecombyReviewer2 = model[0].Reviewer2Recomendation;
                            if (!string.IsNullOrEmpty(model[0].Reviewer2RecomendationComments))
                                obj.PromoRecombyReviewer2Comments = model[0].Reviewer2RecomendationComments.Trim();
                            else
                                obj.PromoRecombyReviewer2Comments = model[0].Reviewer2RecomendationComments;
                            if (!string.IsNullOrEmpty(model[0].Reviewer2OverallRatingComments))
                                obj.OverallReviewer2Comments = model[0].Reviewer2OverallRatingComments.Trim();
                            else
                                obj.OverallReviewer2Comments = model[0].Reviewer2OverallRatingComments;
                        }
                        if (UserRole == UserInRole.GroupHead.ToString())
                        {
                            obj.OverallGroupHeadRatings = model[0].GroupHeadOverallRating;
                            obj.PromoRecombyGroupHead = model[0].GroupHeadRecomendation;
                            if (!string.IsNullOrEmpty(model[0].GroupHeadRecomendationComments))
                                obj.PromoRecombyGroupHeadComments = model[0].GroupHeadRecomendationComments.Trim();
                            else
                                obj.PromoRecombyGroupHeadComments = model[0].GroupHeadRecomendationComments;
                            if (!string.IsNullOrEmpty(model[0].GroupHeadOverallRatingComments))
                                obj.OverallGroupHeadComments = model[0].GroupHeadOverallRatingComments.Trim();
                            else
                                obj.OverallGroupHeadComments = model[0].GroupHeadOverallRatingComments;
                        }
                        dbContext.tbl_Appraisal_PromotionRecommendation.AddObject(obj);
                        dbContext.SaveChanges();
                        int PromoRecomendationID = dbContext.tbl_Appraisal_PromotionRecommendation.Where(x => x.AppraisalID == AppID && x.EmployeeID == empID).Select(s => s.PromoRecomID).FirstOrDefault();

                        for (int m = 0; m < count - 1; m++)
                        {
                            tbl_Appraisal_PromotionRecommendationParameters parm = new tbl_Appraisal_PromotionRecommendationParameters();
                            parm.ParameterID = model[m].parmID;
                            parm.PromoRecomID = PromoRecomendationID;
                            if (UserRole == UserInRole.Reviewer1.ToString())
                            {
                                if (!string.IsNullOrEmpty(model[m].Reviewer1Comments))
                                    parm.Reviewer1Comments = model[m].Reviewer1Comments.Trim();
                                else
                                    parm.Reviewer1Comments = model[m].Reviewer1Comments;
                                parm.Reviewer1Ratings = model[m].Reviewer1Ratings;
                            }
                            if (UserRole == UserInRole.Reviewer2.ToString())
                            {
                                if (!string.IsNullOrEmpty(model[m].Reviewer2Comments))
                                    parm.Reviewer2Comments = model[m].Reviewer2Comments.Trim();
                                else
                                    parm.Reviewer2Comments = model[m].Reviewer2Comments;
                                parm.Reviewer2Ratings = model[m].Reviewer2Raitings;
                            }
                            if (UserRole == UserInRole.GroupHead.ToString())
                            {
                                if (!string.IsNullOrEmpty(model[m].GroupHeadComments))
                                    parm.GroupHeadComments = model[m].GroupHeadComments.Trim();
                                else
                                    parm.GroupHeadComments = model[m].GroupHeadComments;
                                parm.GroupHeadRatings = model[m].GroupHeadRaitings;
                            }
                            dbContext.tbl_Appraisal_PromotionRecommendationParameters.AddObject(parm);
                        }
                    }
                    // already existing record
                    else
                    {
                        //update tbl_Appraisal_PromotionRecommendationParameters
                        int PromoRecomID = dbContext.tbl_Appraisal_PromotionRecommendation.Where(x => x.AppraisalID == AppID && x.EmployeeID == empID).Select(s => s.PromoRecomID).FirstOrDefault();
                        //int parmcount = parm.Count;
                        recom.AppraisalID = model[0].AppraisalID;
                        recom.CurrentDesignationID = model[0].DesignationID;
                        recom.EmployeeID = model[0].EmployeeID;
                        recom.NextDesignationIDFromReviewer1 = model[0].NextDesignationIDFromReviewer1;
                        recom.NextDesignationIDfromReviewer2 = model[0].NextDesignationIDFromReviewer2;
                        recom.NextDesignationIDfromGroupHead = model[0].NextDesignationIDFromGroupHead;
                        if (UserRole == UserInRole.Reviewer1.ToString())
                        {
                            recom.OverallReviewer1Ratings = model[0].Reviewer1OverallRating;
                            recom.PromoRecombyReviewer1 = model[0].Reviewer1Recomendation;
                            if (!string.IsNullOrEmpty(model[0].Reviewer1RecomendationComments))
                                recom.PromoRecombyReviewer1Comments = model[0].Reviewer1RecomendationComments.Trim();
                            else
                                recom.PromoRecombyReviewer1Comments = model[0].Reviewer1RecomendationComments;
                            if (!string.IsNullOrEmpty(model[0].Reviewer1OverallRatingComments))
                                recom.OverallReviewer1Comments = model[0].Reviewer1OverallRatingComments.Trim();
                            else
                                recom.OverallReviewer1Comments = model[0].Reviewer1OverallRatingComments;
                        }
                        if (UserRole == UserInRole.Reviewer2.ToString())
                        {
                            recom.OverallReviewer2Ratings = model[0].Reviewer2OverallRating;
                            recom.PromoRecombyReviewer2 = model[0].Reviewer2Recomendation;
                            if (!string.IsNullOrEmpty(model[0].Reviewer2RecomendationComments))
                                recom.PromoRecombyReviewer2Comments = model[0].Reviewer2RecomendationComments.Trim();
                            else
                                recom.PromoRecombyReviewer2Comments = model[0].Reviewer2RecomendationComments;
                            if (!string.IsNullOrEmpty(model[0].Reviewer2OverallRatingComments))
                                recom.OverallReviewer2Comments = model[0].Reviewer2OverallRatingComments.Trim();
                            else
                                recom.OverallReviewer2Comments = model[0].Reviewer2OverallRatingComments;
                        }
                        if (UserRole == UserInRole.GroupHead.ToString())
                        {
                            recom.OverallGroupHeadRatings = model[0].GroupHeadOverallRating;
                            recom.PromoRecombyGroupHead = model[0].GroupHeadRecomendation;
                            if (!string.IsNullOrEmpty(model[0].GroupHeadRecomendationComments))
                                recom.PromoRecombyGroupHeadComments = model[0].GroupHeadRecomendationComments.Trim();
                            else
                                recom.PromoRecombyGroupHeadComments = model[0].GroupHeadRecomendationComments;
                            if (!string.IsNullOrEmpty(model[0].GroupHeadOverallRatingComments))
                                recom.OverallGroupHeadComments = model[0].GroupHeadOverallRatingComments.Trim();
                            else
                                recom.OverallGroupHeadComments = model[0].GroupHeadOverallRatingComments;
                        }
                        List<tbl_Appraisal_PromotionRecommendationParameters> parm = dbContext.tbl_Appraisal_PromotionRecommendationParameters.Where(e => e.PromoRecomID == PromoRecomID).ToList();
                        foreach (var tblAppraisalPromotionRecommendationParameterse in parm)
                        {
                            if (UserRole == UserInRole.Reviewer1.ToString())
                            {
                                tblAppraisalPromotionRecommendationParameterse.Reviewer1Ratings = null;
                                tblAppraisalPromotionRecommendationParameterse.Reviewer1Comments = null;
                            }
                            else if (UserRole == UserInRole.Reviewer2.ToString())
                            {
                                tblAppraisalPromotionRecommendationParameterse.Reviewer2Ratings = null;
                                tblAppraisalPromotionRecommendationParameterse.Reviewer2Comments = null;
                            }
                            else if (UserRole == UserInRole.GroupHead.ToString())
                            {
                                tblAppraisalPromotionRecommendationParameterse.GroupHeadRatings = null;
                                tblAppraisalPromotionRecommendationParameterse.GroupHeadComments = null;
                            }
                        }
                        for (int i = 0; i < count - 1; i++)
                        {
                            int parmID = model[i].parmID;
                            tbl_Appraisal_PromotionRecommendationParameters promoParam = dbContext.tbl_Appraisal_PromotionRecommendationParameters.Where(p => p.ParameterID == parmID && p.PromoRecomID == PromoRecomID).FirstOrDefault();
                            if (promoParam != null)
                            {
                                if (UserRole == UserInRole.Reviewer1.ToString())
                                {
                                    promoParam.Reviewer1Ratings = model[i].Reviewer1Ratings;
                                    if (!string.IsNullOrEmpty(model[i].Reviewer1Comments))
                                        promoParam.Reviewer1Comments = model[i].Reviewer1Comments.Trim();
                                    else
                                        promoParam.Reviewer1Comments = model[i].Reviewer1Comments;
                                }
                                if (UserRole == UserInRole.Reviewer2.ToString())
                                {
                                    promoParam.Reviewer2Ratings = model[i].Reviewer2Raitings;
                                    if (!string.IsNullOrEmpty(model[i].Reviewer2Comments))
                                        promoParam.Reviewer2Comments = model[i].Reviewer2Comments.Trim();
                                    else
                                        promoParam.Reviewer2Comments = model[i].Reviewer2Comments;
                                }
                                if (UserRole == UserInRole.GroupHead.ToString())
                                {
                                    promoParam.GroupHeadRatings = model[i].GroupHeadRaitings;
                                    if (!string.IsNullOrEmpty(model[i].GroupHeadComments))
                                        promoParam.GroupHeadComments = model[i].GroupHeadComments.Trim();
                                    else
                                        promoParam.GroupHeadComments = model[i].GroupHeadComments;
                                }
                            }
                            else
                            {
                                if (model[i].parmID != 0)
                                {
                                    tbl_Appraisal_PromotionRecommendationParameters Promotionparm = new tbl_Appraisal_PromotionRecommendationParameters();
                                    Promotionparm.ParameterID = model[i].parmID;
                                    Promotionparm.PromoRecomID = PromoRecomID;
                                    if (UserRole == UserInRole.Reviewer1.ToString())
                                    {
                                        if (!string.IsNullOrEmpty(model[i].Reviewer1Comments))
                                            Promotionparm.Reviewer1Comments = model[i].Reviewer1Comments.Trim();
                                        else
                                            Promotionparm.Reviewer1Comments = model[i].Reviewer1Comments;
                                        Promotionparm.Reviewer1Ratings = model[i].Reviewer1Ratings;
                                    }
                                    if (UserRole == UserInRole.Reviewer2.ToString())
                                    {
                                        if (!string.IsNullOrEmpty(model[i].Reviewer2Comments))
                                            Promotionparm.Reviewer2Comments = model[i].Reviewer2Comments.Trim();
                                        else
                                            Promotionparm.Reviewer2Comments = model[i].Reviewer2Comments;
                                        Promotionparm.Reviewer2Ratings = model[i].Reviewer2Raitings;
                                    }
                                    if (UserRole == UserInRole.GroupHead.ToString())
                                    {
                                        Promotionparm.GroupHeadRatings = model[i].GroupHeadRaitings;
                                        if (!string.IsNullOrEmpty(model[i].GroupHeadComments))
                                            Promotionparm.GroupHeadComments = model[i].GroupHeadComments.Trim();
                                        else
                                            Promotionparm.GroupHeadComments = model[i].GroupHeadComments;
                                    }
                                    dbContext.tbl_Appraisal_PromotionRecommendationParameters.AddObject(Promotionparm);
                                }
                            }
                        }
                    }

                    dbContext.SaveChanges();
                    isAdded = true;
                }
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpraisalRating GetMinMaxRating(int upraisalYearId)
        {
            try
            {
                List<tbl_Appraisal_RatingMaster> upAppraisalRatingMasters =
                    dbContext.tbl_Appraisal_RatingMaster.Where(x => x.AppraisalYearID == upraisalYearId).OrderByDescending(x => x.Percentage).ToList();
                UpraisalRating upraisalRating = new UpraisalRating();
                if (upAppraisalRatingMasters.Count > 0)
                {
                    upraisalRating.MaxValue = Convert.ToInt32(upAppraisalRatingMasters[0].Percentage);
                    upraisalRating.MinValue = Convert.ToInt32(upAppraisalRatingMasters[upAppraisalRatingMasters.Count - 1].Percentage);
                }
                return upraisalRating;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public AppraisalProcessThreeModel GetSuccessionPlanningDetails(int EmployeeID, int AppraisalID, string UserRole)
        {
            AppraisalProcessThreeModel model = new AppraisalProcessThreeModel();
            tbl_Appraisal_EmpSuccessionPlanning plan = dbContext.tbl_Appraisal_EmpSuccessionPlanning.Where(s => s.AppraisalID == AppraisalID && s.EmployeeID == EmployeeID).FirstOrDefault();

            if (plan != null)
            {
                if (UserRole == UserInRole.Reviewer1.ToString())
                {
                    model.ReadyComments1to2YearsByReviewer1 = plan.Reviewer1Comments1to2years;
                    model.ReadyComments3to5YearsByReviewer1 = plan.Reviewer1Comments3to5years;
                    model.ReadyCommentsByReviewer1 = plan.Reviewer1CommentsReadyNow;
                }
                if (UserRole == UserInRole.Reviewer2.ToString())
                {
                    model.ReadyComments1to2YearsByReviewer2 = plan.Reviewer2Comments1to2years;
                    model.ReadyComments3to5YearsByReviewer2 = plan.Reviewer2Comments3to5years;
                    model.ReadyCommentsByReviewer2 = plan.Reviewer2CommentsReadyNow;
                }
            }
            return model;
        }

        public bool SaveSuccessionPlanningDetails(AppraisalProcessThreeModel model)
        {
            bool isAdded = false;
            int appID = model.AppraisalID;
            int EmpId = model.EmployeeID;
            //string UserRole = model.UserInRole;
            tbl_Appraisal_EmpSuccessionPlanning plan = dbContext.tbl_Appraisal_EmpSuccessionPlanning.Where(s => s.AppraisalID == appID && s.EmployeeID == EmpId).FirstOrDefault();
            if (plan != null)
            {
                if (model.UserInRole == UserInRole.Reviewer1)
                {
                    if (!string.IsNullOrEmpty(model.ReadyComments1to2YearsByReviewer1))
                        plan.Reviewer1Comments1to2years = model.ReadyComments1to2YearsByReviewer1.Trim();
                    else
                        plan.Reviewer1Comments1to2years = model.ReadyComments1to2YearsByReviewer1;
                    if (!string.IsNullOrEmpty(model.ReadyComments3to5YearsByReviewer1))
                        plan.Reviewer1Comments3to5years = model.ReadyComments3to5YearsByReviewer1.Trim();
                    else
                        plan.Reviewer1Comments3to5years = model.ReadyComments3to5YearsByReviewer1;
                    if (!string.IsNullOrEmpty(model.ReadyCommentsByReviewer1))
                        plan.Reviewer1CommentsReadyNow = model.ReadyCommentsByReviewer1.Trim();
                    else
                        plan.Reviewer1CommentsReadyNow = model.ReadyCommentsByReviewer1;
                }
                if (model.UserInRole == UserInRole.Reviewer2)
                {
                    if (!string.IsNullOrEmpty(model.ReadyComments1to2YearsByReviewer2))
                        plan.Reviewer2Comments1to2years = model.ReadyComments1to2YearsByReviewer2.Trim();
                    else
                        plan.Reviewer2Comments1to2years = model.ReadyComments1to2YearsByReviewer2;
                    if (!string.IsNullOrEmpty(model.ReadyComments3to5YearsByReviewer2))
                        plan.Reviewer2Comments3to5years = model.ReadyComments3to5YearsByReviewer2.Trim();
                    else
                        plan.Reviewer2Comments3to5years = model.ReadyComments3to5YearsByReviewer2;
                    if (!string.IsNullOrEmpty(model.ReadyCommentsByReviewer2))
                        plan.Reviewer2CommentsReadyNow = model.ReadyCommentsByReviewer2.Trim();
                    else
                        plan.Reviewer2CommentsReadyNow = model.ReadyCommentsByReviewer2;
                }
            }
            else
            {
                tbl_Appraisal_EmpSuccessionPlanning EmpPlan = new tbl_Appraisal_EmpSuccessionPlanning();
                EmpPlan.AppraisalID = model.AppraisalID;
                EmpPlan.EmployeeID = model.EmployeeID;
                if (model.UserInRole == UserInRole.Reviewer1)
                {
                    if (!string.IsNullOrEmpty(model.ReadyComments1to2YearsByReviewer1))
                        EmpPlan.Reviewer1Comments1to2years = model.ReadyComments1to2YearsByReviewer1.Trim();
                    else
                        EmpPlan.Reviewer1Comments1to2years = model.ReadyComments1to2YearsByReviewer1;
                    if (!string.IsNullOrEmpty(model.ReadyComments3to5YearsByReviewer1))
                        EmpPlan.Reviewer1Comments3to5years = model.ReadyComments3to5YearsByReviewer1.Trim();
                    else
                        EmpPlan.Reviewer1Comments3to5years = model.ReadyComments3to5YearsByReviewer1;
                    if (!string.IsNullOrEmpty(model.ReadyCommentsByReviewer1))
                        EmpPlan.Reviewer1CommentsReadyNow = model.ReadyCommentsByReviewer1.Trim();
                    else
                        EmpPlan.Reviewer1CommentsReadyNow = model.ReadyCommentsByReviewer1;
                }
                if (model.UserInRole == UserInRole.Reviewer2)
                {
                    if (!string.IsNullOrEmpty(model.ReadyComments1to2YearsByReviewer2))
                        EmpPlan.Reviewer2Comments1to2years = model.ReadyComments1to2YearsByReviewer2.Trim();
                    else
                        EmpPlan.Reviewer2Comments1to2years = model.ReadyComments1to2YearsByReviewer2;
                    if (!string.IsNullOrEmpty(model.ReadyComments3to5YearsByReviewer2))
                        EmpPlan.Reviewer2Comments3to5years = model.ReadyComments3to5YearsByReviewer2.Trim();
                    else
                        EmpPlan.Reviewer2Comments3to5years = model.ReadyComments3to5YearsByReviewer2;
                    if (!string.IsNullOrEmpty(model.ReadyCommentsByReviewer2))
                        EmpPlan.Reviewer2CommentsReadyNow = model.ReadyCommentsByReviewer2.Trim();
                    else
                        EmpPlan.Reviewer2CommentsReadyNow = model.ReadyCommentsByReviewer2;
                }
                dbContext.tbl_Appraisal_EmpSuccessionPlanning.AddObject(EmpPlan);
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public AppraisalEmployeeGrowthSummary GetAppProcessDetails(int EmpID, int AppID)
        {
            AppraisalEmployeeGrowthSummary model = new AppraisalEmployeeGrowthSummary();

            tbl_Appraisal_EmpGrowthSummary emp = dbContext.tbl_Appraisal_EmpGrowthSummary.Where(s => s.AppraisalID == AppID && s.EmployeeID == EmpID).FirstOrDefault();

            if (emp != null)
            {
                model.Reviewer1Comments1to2years = emp.Reviewer1Comments1to2years;
                model.Reviewer1Comments3to5years = emp.Reviewer1Comments3to5years;
                model.Reviewer1CommentsReadyNow = emp.Reviewer1CommentsReadyNow;
                model.Reviewer2CommentsReadyNow = emp.Reviewer2CommentsReadyNow;
                model.Reviewer2Comments1to2years = emp.Reviewer2Comments1to2years;
                model.Reviewer2Comments3to5years = emp.Reviewer2Comments3to5years;
            }
            return model;
        }

        public List<Parameters> GetPromotionRecommendationDetails(int EmpID, int AppID, string UserRole, int? DesignationID)
        {
            List<Parameters> ParameterList = GetParameters(DesignationID, AppID);
            List<Parameters> modelList = new List<Parameters>();

            tbl_Appraisal_PromotionRecommendation recom = (from r in dbContext.tbl_Appraisal_PromotionRecommendation
                                                           where r.AppraisalID == AppID && r.EmployeeID == EmpID
                                                           select r).FirstOrDefault();

            if (recom != null)
            {
                if (ParameterList.Count > 0)
                {
                    foreach (var item in ParameterList)
                    {
                        Parameters parm = new Parameters();
                        tbl_Appraisal_PromotionRecommendationParameters recommendation = (from r in dbContext.tbl_Appraisal_PromotionRecommendationParameters
                                                                                          where r.PromoRecomID == recom.PromoRecomID && r.ParameterID == item.parmID
                                                                                          select r).FirstOrDefault();
                        if (recommendation != null)
                        {
                            parm.AppraisalID = AppID;
                            parm.parmID = recommendation.ParameterID;
                            parm.ParameterDesc = item.ParameterDesc;
                            if (UserRole == UserInRole.Reviewer1.ToString())
                            {
                                parm.Reviewer1Comments = recommendation.Reviewer1Comments;
                                parm.Reviewer1OverallRating = recom.OverallReviewer1Ratings;
                                parm.Reviewer1OverallRatingComments = recom.OverallReviewer1Comments;
                                parm.Reviewer1Ratings = recommendation.Reviewer1Ratings;
                                parm.Reviewer1Recomendation = recom.PromoRecombyReviewer1;
                                parm.Reviewer1RecomendationComments = recom.PromoRecombyReviewer1Comments;
                            }
                            if (UserRole == UserInRole.Reviewer2.ToString())
                            {
                                parm.Reviewer2Comments = recommendation.Reviewer2Comments;
                                parm.Reviewer2OverallRating = recom.OverallReviewer2Ratings;
                                parm.Reviewer2OverallRatingComments = recom.OverallReviewer2Comments;
                                parm.Reviewer2Raitings = recommendation.Reviewer2Ratings;
                                parm.Reviewer2Recomendation = recom.PromoRecombyReviewer2;
                                parm.Reviewer2RecomendationComments = recom.PromoRecombyReviewer2Comments;
                            }
                            if (UserRole == UserInRole.GroupHead.ToString())
                            {
                                parm.GroupHeadComments = recommendation.GroupHeadComments;
                                parm.GroupHeadOverallRating = recom.OverallGroupHeadRatings;
                                parm.GroupHeadOverallRatingComments = recom.OverallGroupHeadComments;
                                parm.GroupHeadRaitings = recommendation.GroupHeadRatings;
                                parm.GroupHeadRecomendation = recom.PromoRecombyGroupHead;
                                parm.GroupHeadRecomendationComments = recom.PromoRecombyGroupHeadComments;
                            }
                        }
                        else
                        {
                            parm.parmID = item.parmID;
                            parm.ParameterDesc = item.ParameterDesc;
                        }
                        modelList.Add(parm);
                    }
                }
                else
                {
                    Parameters parm = new Parameters();
                    parm.AppraisalID = AppID;
                    if (UserRole == UserInRole.Reviewer1.ToString())
                    {
                        parm.Reviewer1OverallRating = recom.OverallReviewer1Ratings;
                        parm.Reviewer1OverallRatingComments = recom.OverallReviewer1Comments;
                        parm.Reviewer1Recomendation = recom.PromoRecombyReviewer1;
                        parm.Reviewer1RecomendationComments = recom.PromoRecombyReviewer1Comments;
                    }
                    if (UserRole == UserInRole.Reviewer2.ToString())
                    {
                        parm.Reviewer2OverallRating = recom.OverallReviewer2Ratings;
                        parm.Reviewer2OverallRatingComments = recom.OverallReviewer2Comments;
                        parm.Reviewer2Recomendation = recom.PromoRecombyReviewer2;
                        parm.Reviewer2RecomendationComments = recom.PromoRecombyReviewer2Comments;
                    }
                    if (UserRole == UserInRole.GroupHead.ToString())
                    {
                        parm.GroupHeadOverallRating = recom.OverallGroupHeadRatings;
                        parm.GroupHeadOverallRatingComments = recom.OverallGroupHeadComments;
                        parm.GroupHeadRecomendation = recom.PromoRecombyGroupHead;
                        parm.GroupHeadRecomendationComments = recom.PromoRecombyGroupHeadComments;
                    }
                    modelList.Add(parm);
                }
            }
            else
            {
                foreach (var item in ParameterList)
                {
                    modelList.Add(item);
                }
            }
            return modelList.ToList();
        }

        public tbl_Appraisal_PromotionRecommendation getAppraisalPromotionDetails(int appraisalId)
        {
            tbl_Appraisal_PromotionRecommendation promotionRecommendation = dbContext.tbl_Appraisal_PromotionRecommendation.Where(x => x.AppraisalID == appraisalId).FirstOrDefault();
            return promotionRecommendation;
        }

        //public void AddAppraiseeDefaultStrength( int appraisalId)
        //{
        //    try
        //    {
        //        tbl_Appraisal_AppraisalMaster appraisal = dbContext.tbl_Appraisal_AppraisalMaster.Where(ed => ed.AppraisalID == appraisalId).FirstOrDefault();
        //        int appraisalYearId = 0;
        //        if (appraisal != null)
        //        {
        //            appraisalYearId = appraisal.AppraisalYearID;

        //            List<AppraiseeStrengths> strengthsList = (from strength in dbContext.tbl_Appraisal_RatingComments
        //                join parameter in dbContext.tbl_Appraisal_ParameterMaster on strength.ParameterID equals
        //                    parameter.ParameterID
        //                join strengthLimit in dbContext.tbl_Appraisal_SrengthImprovement_Limit on
        //                    parameter.AppraisalYearID equals strengthLimit.AppraisalYearID
        //                where
        //                    strengthLimit.AppraisalYearID == appraisalYearId && strength.AppraisalID == appraisalId &&
        //                    strength.Appraiser1Ratings >= strengthLimit.StrengthLimit
        //                orderby strength.AppraisalID descending
        //                select new AppraiseeStrengths
        //                {
        //                    Strength = parameter.Parameter,
        //                    StrengthId = parameter.ParameterID,
        //                    AppraisalId = strength.AppraisalID
        //                }).ToList();
        //            if (strengthsList != null)
        //            {
        //                tbl_Appraisal_IDFAppraiserAddStrength strengths = new tbl_Appraisal_IDFAppraiserAddStrength();
        //                foreach (var strength in strengthsList)
        //                {
        //                    strengths.EmployeeID = strength.EmployeeId;
        //                    strengths.AppraisalID = strength.AppraisalId;
        //                    strengths.AppraisalYearID = strength.AppraisalYearId;
        //                    strengths.IDFAppraiserStrength = strength.Strength;
        //                    strengths.IDFAppraiserStrengthComment = strength.StrengthComments;
        //                    dbContext.tbl_Appraisal_IDFAppraiserAddStrength.AddObject(strengths);
        //                    dbContext.SaveChanges();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public List<AppraiseeStrengths> GetAppraiseeStrengthsDetails(int employeeId, int appraisalId, int appraisalYearId, int page, int rows, out int totalCount)
        {
            //try
            //{
            //    List<AppraiseeStrengths> strengthsList = (from strength in dbContext.tbl_Appraisal_IDFAppraiserAddStrength
            //                                              where strength.AppraisalID == appraisalId
            //                                              orderby strength.AppraisalID descending
            //                                              select new AppraiseeStrengths
            //                                              {
            //                                                  AppraisalId = strength.AppraisalID,
            //                                                  AppraisalYearId = strength.AppraisalYearID,
            //                                                  StrengthComments = strength.IDFAppraiserStrengthComment,
            //                                                  Strength = strength.IDFAppraiserStrength,
            //                                                  StrengthId = strength.IDFAppraiserStrengthID
            //                                              }).Skip((page - 1) * rows).Take(rows).ToList();
            //    totalCount = (from strength in dbContext.tbl_Appraisal_IDFAppraiserAddStrength
            //                  where strength.AppraisalID == appraisalId
            //                  select strength.AppraisalID).Count();
            //    return strengthsList;
            //}
            try
            {
                List<AppraiseeStrengths> strengthsList = (from strength in dbContext.tbl_Appraisal_RatingComments
                                                          join db in dbContext.tbl_Appraisal_ParameterMaster on strength.ParameterID equals db.ParameterID
                                                          join strimp in dbContext.tbl_Appraisal_SrengthImprovement_Limit on db.AppraisalYearID equals strimp.AppraisalYearID
                                                          where strength.AppraisalID == appraisalId && strength.Reviewer1Ratings >= strimp.StrengthLimit
                                                          orderby strength.AppraisalID descending
                                                          select new AppraiseeStrengths
                                                          {
                                                              AppraisalId = strength.AppraisalID,
                                                              Strength = db.Parameter,
                                                              StrengthComments = strength.Reviewer1Comments
                                                          }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from strength in dbContext.tbl_Appraisal_RatingComments
                              where strength.AppraisalID == appraisalId
                              select strength.AppraisalID).Count();
                return strengthsList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveStrengthsDetails(AppraiseeStrengths strength)
        {
            bool isAdded = false;
            int? apraisalID = (int?)strength.AppraisalId;
            tbl_Appraisal_IDFAppraiserAddStrength emp = dbContext.tbl_Appraisal_IDFAppraiserAddStrength.Where(ed => ed.IDFAppraiserStrengthID == strength.StrengthId && ed.AppraisalID == apraisalID).FirstOrDefault();
            if (emp == null)
            {
                tbl_Appraisal_IDFAppraiserAddStrength strengths = new tbl_Appraisal_IDFAppraiserAddStrength();
                strengths.EmployeeID = strength.EmployeeId;
                strengths.AppraisalID = strength.AppraisalId;
                strengths.AppraisalYearID = strength.AppraisalYearId;
                strengths.IDFAppraiserStrength = strength.Strength;
                strengths.IDFAppraiserStrengthComment = strength.StrengthComments;
                dbContext.tbl_Appraisal_IDFAppraiserAddStrength.AddObject(strengths);
            }
            else
            {
                emp.IDFAppraiserStrengthID = strength.StrengthId;
                emp.EmployeeID = strength.EmployeeId;
                emp.AppraisalID = strength.AppraisalId;
                emp.AppraisalYearID = strength.AppraisalYearId;
                emp.IDFAppraiserStrength = strength.Strength;
                emp.IDFAppraiserStrengthComment = strength.StrengthComments;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool DeleteAppraiseeStrengthDetails(int strengthID)
        {
            bool isDeleted = false;
            tbl_Appraisal_IDFAppraiserAddStrength strengthDetails = dbContext.tbl_Appraisal_IDFAppraiserAddStrength.Where(cd => cd.IDFAppraiserStrengthID == strengthID).FirstOrDefault();
            if (strengthDetails != null)
            {
                dbContext.DeleteObject(strengthDetails);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        //public void AddAppraiseeDefaultImprovement(int employeeId, int appraisalId, int appraisalYearId)
        //{
        //    try
        //    {
        //        List<AppraiseeImprovements> improvementList = (from strength in dbContext.tbl_Appraisal_RatingComments
        //                                                       join parameter in dbContext.tbl_Appraisal_ParameterMaster on strength.ParameterID equals parameter.ParameterID
        //                                                       join strengthLimit in dbContext.tbl_Appraisal_SrengthImprovement_Limit on parameter.AppraisalYearID equals strengthLimit.AppraisalYearID
        //                                                       where strengthLimit.AppraisalYearID == appraisalYearId && strength.AppraisalID == appraisalId && strength.Appraiser1Ratings <= strengthLimit.ImprovementLimit
        //                                                       orderby strength.AppraisalID descending
        //                                                       select new AppraiseeImprovements
        //                                                       {
        //                                                           Improvement = parameter.Parameter,
        //                                                           ImprovementId = parameter.ParameterID
        //                                                       }).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public List<AppraiseeImprovements> GetAppraiseeImprovementsDetails(int employeeId, int appraisalId, int appraisalYearId, int page, int rows, out int totalCount)
        {
            //try
            //{
            //    List<AppraiseeImprovements> improvementList = (from strength in dbContext.tbl_Appraisal_IDFAppraiserAddImprovement
            //                                                   where strength.AppraisalID == appraisalId
            //                                                   orderby strength.AppraisalID descending
            //                                                   select new AppraiseeImprovements
            //                                                   {
            //                                                       AppraisalId = strength.AppraisalID,
            //                                                       AppraisalYearId = strength.AppraisalYearID,
            //                                                       ImprovementComments = strength.IDFAppraiserImprovementComment,
            //                                                       Improvement = strength.IDFAppraiserImprovement,
            //                                                       ImprovementId = strength.IDFAppraiserImprovementID
            //                                                   }).Skip((page - 1) * rows).Take(rows).ToList();

            //    totalCount = (from strength in dbContext.tbl_Appraisal_IDFAppraiserAddImprovement
            //                  where strength.AppraisalID == appraisalId
            //                  select strength.AppraisalID).Count();
            //    return improvementList;
            //}
            try
            {
                List<AppraiseeImprovements> improvementList = (from strength in dbContext.tbl_Appraisal_RatingComments
                                                               join db in dbContext.tbl_Appraisal_ParameterMaster on strength.ParameterID equals db.ParameterID
                                                               join strimp in dbContext.tbl_Appraisal_SrengthImprovement_Limit on db.AppraisalYearID equals strimp.AppraisalYearID
                                                               where strength.AppraisalID == appraisalId && strength.Reviewer1Ratings < strimp.StrengthLimit
                                                               orderby strength.AppraisalID descending
                                                               select new AppraiseeImprovements
                                                               {
                                                                   AppraisalId = strength.AppraisalID,
                                                                   Improvement = db.Parameter,
                                                                   ImprovementComments = strength.Reviewer1Comments
                                                               }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from strength in dbContext.tbl_Appraisal_RatingComments
                              where strength.AppraisalID == appraisalId
                              select strength.AppraisalID).Count();
                return improvementList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveImprovementsDetails(AppraiseeImprovements improvement)
        {
            bool isAdded = false;
            int? apraisalID = (int?)improvement.AppraisalId;
            tbl_Appraisal_IDFAppraiserAddImprovement emp = dbContext.tbl_Appraisal_IDFAppraiserAddImprovement.Where(ed => ed.IDFAppraiserImprovementID == improvement.ImprovementId && ed.AppraisalID == apraisalID).FirstOrDefault();
            if (emp == null)
            {
                tbl_Appraisal_IDFAppraiserAddImprovement strengths = new tbl_Appraisal_IDFAppraiserAddImprovement();
                strengths.EmployeeID = improvement.EmployeeId;
                strengths.AppraisalID = improvement.AppraisalId;
                strengths.AppraisalYearID = improvement.AppraisalYearId;
                strengths.IDFAppraiserImprovement = improvement.Improvement;
                strengths.IDFAppraiserImprovementComment = improvement.ImprovementComments;
                dbContext.tbl_Appraisal_IDFAppraiserAddImprovement.AddObject(strengths);
            }
            else
            {
                emp.IDFAppraiserImprovementID = improvement.ImprovementId;
                emp.EmployeeID = improvement.EmployeeId;
                emp.AppraisalID = improvement.AppraisalId;
                emp.AppraisalYearID = improvement.AppraisalYearId;
                emp.IDFAppraiserImprovement = improvement.Improvement;
                emp.IDFAppraiserImprovementComment = improvement.ImprovementComments;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool DeleteAppraiseeImprovementDetails(int improvementID)
        {
            bool isDeleted = false;
            tbl_Appraisal_IDFAppraiserAddImprovement improvementDetails = dbContext.tbl_Appraisal_IDFAppraiserAddImprovement.Where(cd => cd.IDFAppraiserImprovementID == improvementID).FirstOrDefault();
            if (improvementDetails != null)
            {
                dbContext.DeleteObject(improvementDetails);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public List<TrainingProgram> GetAppraiseeTrainingProgramDetails(int employeeId, int appraisalId, int page, int rows, out int totalCount)
        {
            try
            {
                List<TrainingProgram> strengthsList = (from strength in dbContext.tbl_Appraisal_IDFTrainingProgram
                                                       join category in dbContext.Tbl_Appraisal_CategoryMaster on strength.CategoryID equals category.CategoryID
                                                       where strength.AppraisalID == appraisalId
                                                       orderby strength.AppraisalID descending
                                                       select new TrainingProgram
                                                       {
                                                           AppraisalId = strength.AppraisalID,
                                                           AppraisalYearId = strength.AppraisalYearID,
                                                           ProgramId = strength.IDFTrainingProgramID,
                                                           Name = strength.NameofTrainingProgram,
                                                           CategoryId = category.CategoryID,
                                                           Category = category.Category,
                                                           Reason = strength.ReasonForTraining
                                                       }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from strength in dbContext.tbl_Appraisal_IDFTrainingProgram
                              where strength.AppraisalID == appraisalId
                              select strength.AppraisalID).Count();

                return strengthsList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveTrainingProgramDetails(TrainingProgram program)
        {
            bool isAdded = false;
            int? apraisalID = (int?)program.AppraisalId;
            tbl_Appraisal_IDFTrainingProgram emp = dbContext.tbl_Appraisal_IDFTrainingProgram.Where(ed => ed.IDFTrainingProgramID == program.ProgramId && ed.AppraisalID == apraisalID).FirstOrDefault();
            if (emp == null)
            {
                tbl_Appraisal_IDFTrainingProgram strengths = new tbl_Appraisal_IDFTrainingProgram();
                strengths.EmployeeID = program.EmployeeId;
                strengths.AppraisalID = program.AppraisalId;
                strengths.AppraisalYearID = program.AppraisalYearId;
                strengths.NameofTrainingProgram = program.Name;
                strengths.ReasonForTraining = program.Reason;
                strengths.CategoryID = program.CategoryId;
                dbContext.tbl_Appraisal_IDFTrainingProgram.AddObject(strengths);
            }
            else
            {
                emp.IDFTrainingProgramID = program.ProgramId;
                emp.EmployeeID = program.EmployeeId;
                emp.AppraisalID = program.AppraisalId;
                emp.AppraisalYearID = program.AppraisalYearId;
                emp.NameofTrainingProgram = program.Name;
                emp.ReasonForTraining = program.Reason;
                emp.CategoryID = program.CategoryId;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool DeleteAppraiseeTrainingProgramDetails(int programId)
        {
            bool isDeleted = false;
            tbl_Appraisal_IDFTrainingProgram improvementDetails = dbContext.tbl_Appraisal_IDFTrainingProgram.Where(cd => cd.IDFTrainingProgramID == programId).FirstOrDefault();
            if (improvementDetails != null)
            {
                dbContext.DeleteObject(improvementDetails);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public List<TrainingProgram> GetCategoryList()
        {
            List<TrainingProgram> category = (from appraisal in dbContext.Tbl_Appraisal_CategoryMaster
                                              orderby appraisal.CategoryID
                                              select new TrainingProgram
                                              {
                                                  CategoryId = appraisal.CategoryID,
                                                  Category = appraisal.Category
                                              }).ToList();

            return category;
        }

        public bool SubmitAppraisalStageDetail(int appraisalId, string isMngrOrEmpElement)
        {
            bool isAdded = false;
            int loggedInEmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);

            tbl_Appraisal_AppraisalMaster emp = dbContext.tbl_Appraisal_AppraisalMaster.Where(ed => ed.AppraisalID == appraisalId).FirstOrDefault();
            if (emp != null)
            {
                if (isMngrOrEmpElement == "Appraiser1" || isMngrOrEmpElement == "Appraiser2" || isMngrOrEmpElement == "Reviewer1" || isMngrOrEmpElement == "Reviewer2")
                {
                    tbl_Appraisal_StageEvents LatestEntry = new tbl_Appraisal_StageEvents();
                    if ((emp.Appraiser2 != null && emp.Appraiser2 != 0) || (emp.Reviewer2 != null && emp.Reviewer2 != 0))
                    {
                        if (isMngrOrEmpElement == "Appraiser2" || isMngrOrEmpElement == "Appraiser1")
                        {
                            LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                           where (empInfo.FromStageId == 1 && empInfo.ToStageId == 1) && empInfo.AppraisalID == emp.AppraisalID && (emp.Appraiser1 == empInfo.USerId || emp.Appraiser2 == empInfo.USerId)
                                           orderby empInfo.EventDatetime descending
                                           select empInfo).FirstOrDefault();
                        }
                        if (isMngrOrEmpElement == "Reviewer2" || isMngrOrEmpElement == "Reviewer1")
                        {
                            LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                           where (empInfo.FromStageId == 2 && empInfo.ToStageId == 2) && empInfo.AppraisalID == emp.AppraisalID && (emp.Reviewer1 == empInfo.USerId || emp.Reviewer2 == empInfo.USerId)
                                           orderby empInfo.EventDatetime descending
                                           select empInfo).FirstOrDefault();
                        }
                        if (LatestEntry != null)
                        {
                            emp.AppraisalStageID = emp.AppraisalStageID + 1;
                        }
                        else
                        {
                            emp.AppraisalStageID = emp.AppraisalStageID;
                        }
                    }
                    else
                    {
                        emp.AppraisalStageID = emp.AppraisalStageID + 1;
                        dbContext.SaveChanges();
                        isAdded = true;
                    }
                }
                else
                {
                    emp.AppraisalStageID = emp.AppraisalStageID + 1;
                    dbContext.SaveChanges();
                    //isAdded = true;
                }
                isAdded = true;
            }
            dbContext.SaveChanges();
            return isAdded;
        }

        public bool SubmitEmployeeStageDetail(AppraisalProcessFourModel model)
        {
            bool isAdded = false;
            tbl_Appraisal_AppraisalMaster emp = dbContext.tbl_Appraisal_AppraisalMaster.Where(ed => ed.AppraisalID == model.AppraisalId).FirstOrDefault();
            if (model.IsAgree == true)
                emp.AppraisalStageID = emp.AppraisalStageID + 1;
            else
                emp.AppraisalStageID = emp.AppraisalStageID - 1;
            if (emp != null)
            {
                if (model.IsAgree == true)
                    emp.AppraisalStageID = emp.AppraisalStageID + 1;
                else
                    emp.AppraisalStageID = emp.AppraisalStageID - 1;
                emp.IDFISAppraiseAgree = model.IsAgree;
                emp.IDFAprraiseComment = model.AppraiseeComments;
                dbContext.SaveChanges();
                isAdded = true;
            }
            return isAdded;
        }

        public tbl_Appraisal_AppraisalMaster GetAppraisalDetailsByAppraisalID(int appraisalId)
        {
            tbl_Appraisal_AppraisalMaster appraisalTable = new tbl_Appraisal_AppraisalMaster();
            try
            {
                appraisalTable = dbContext.tbl_Appraisal_AppraisalMaster.Where(e => e.AppraisalID == appraisalId).FirstOrDefault();
            }
            catch (Exception)
            {
            }
            return appraisalTable;
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
                            List<FieldChildDetails> child = (from appraisalStage in dbContext.tbl_Appraisal_Stages
                                                             select new FieldChildDetails
                                                             {
                                                                 Id = appraisalStage.AppraisalStageID,
                                                                 Description = appraisalStage.AppraisalStage
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

        public List<AppraisalProcessStatus> GetInboxListDetails(string searchText, string field, string fieldChild, int page, int rows, int employeeId, string TextLink, out int totalCount)
        {
            List<AppraisalProcessStatus> mainResult = new List<AppraisalProcessStatus>();
            List<AppraisalProcessStatus> CommonResult = new List<AppraisalProcessStatus>();

            try
            {
                int FieldChild = 0;
                if (fieldChild != "")
                {
                    FieldChild = Convert.ToInt32(fieldChild) - 1;
                }

                if (TextLink == "SelfAppraisal")
                {
                    CommonResult = (from E in dbContext.tbl_Appraisal_AppraisalMaster
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_Appraisal_Stages on E.AppraisalStageID + 1 equals s.AppraisalStageID into st
                                    join y in dbContext.tbl_Appraisal_YearMaster on E.AppraisalYearID equals y.AppraisalYearID
                                    from extstage in st.DefaultIfEmpty()
                                    where E.EmployeeID == employeeId && (E.AppraisalStageID == 0 || (E.AppraisalStageID == 5 && (y.IDFFrozenOn == null || y.IDFFrozenOn >= DateTime.Now))) && E.AppraisalYearID == E.AppraisalYearID
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.AppraisalStageID == FieldChild : FieldChild == 0))) //field search
                                         && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.tbl_Appraisal_StageEvents on E.AppraisalID equals ese.AppraisalID into eventStageRecord  // Fix to add red Image support

                                    select new AppraisalProcessStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatetime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support

                                        ReportingTo = ex.ReportingTo,
                                        AppraisalId = E.AppraisalID,
                                        StageId = E.AppraisalStageID,
                                        AppraisalStageOrder = E.AppraisalStageID,
                                        stageName = extstage.AppraisalStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = ex.EmployeeName,
                                        AppraisalYearId = E.AppraisalYearID
                                    }).Distinct().OrderByDescending(exid => exid.AppraisalId).ToList();
                }
                if (TextLink == "Appraiser")
                {
                    CommonResult = (from E in dbContext.tbl_Appraisal_AppraisalMaster
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_Appraisal_Stages on E.AppraisalStageID + 1 equals s.AppraisalStageID into st
                                    join y in dbContext.tbl_Appraisal_YearMaster on E.AppraisalYearID equals y.AppraisalYearID
                                    from extstage in st.DefaultIfEmpty()
                                    where (E.Appraiser1 == employeeId || E.Appraiser2 == employeeId) && (E.AppraisalStageID == 1 || ((E.AppraisalStageID == 4 && y.IDFInitiatedOn <= DateTime.Now && (y.IDFFrozenOn == null || y.IDFFrozenOn >= DateTime.Now)) && E.Appraiser1 == employeeId)) || E.IsCancelled != true
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.AppraisalStageID == FieldChild : FieldChild == 0))) //field search
                                         && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.tbl_Appraisal_StageEvents on E.AppraisalID equals ese.AppraisalID into eventStageRecord  // Fix to add red Image support

                                    select new AppraisalProcessStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatetime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support

                                        ReportingTo = ex.ReportingTo,
                                        AppraisalId = E.AppraisalID,
                                        StageId = E.AppraisalStageID,
                                        AppraisalStageOrder = E.AppraisalStageID,
                                        stageName = extstage.AppraisalStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = ex.EmployeeName,
                                        AppraisalYearId = E.AppraisalYearID
                                    }).Distinct().OrderByDescending(exid => exid.AppraisalId).ToList();

                    List<AppraisalProcessStatus> ManagerList = new List<AppraisalProcessStatus>();
                    foreach (var item in CommonResult)
                    {
                        tbl_Appraisal_StageEvents LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                                 where empInfo.AppraisalID == item.AppraisalId
                                                                 orderby empInfo.EventDatetime descending
                                                                 select empInfo).FirstOrDefault();
                        tbl_Appraisal_StageEvents LatestEntryManager = new tbl_Appraisal_StageEvents();
                        if (LatestEntry != null)
                        {
                            LatestEntryManager = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                  where empInfo.AppraisalID == item.AppraisalId && (empInfo.FromStageId == 1 && empInfo.ToStageId == 1) && empInfo.EventDatetime >= LatestEntry.EventDatetime
                                                  orderby empInfo.EventDatetime descending
                                                  select empInfo).FirstOrDefault();
                        }
                        if (LatestEntryManager != null)
                        {
                            if (LatestEntryManager.USerId != employeeId)
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

                    CommonResult.Clear();
                    CommonResult = ManagerList;
                }
                if (TextLink == "Reviewer")
                {
                    CommonResult = (from E in dbContext.tbl_Appraisal_AppraisalMaster
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_Appraisal_Stages on E.AppraisalStageID + 1 equals s.AppraisalStageID into st
                                    from extstage in st.DefaultIfEmpty()
                                    where (E.Reviewer1 == employeeId || E.Reviewer2 == employeeId) && (E.AppraisalStageID == 2) || E.IsCancelled != true
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.AppraisalStageID == FieldChild : FieldChild == 0))) //field search
                                         && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.tbl_Appraisal_StageEvents on E.AppraisalID equals ese.AppraisalID into eventStageRecord  // Fix to add red Image support

                                    select new AppraisalProcessStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatetime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support

                                        ReportingTo = ex.ReportingTo,
                                        AppraisalId = E.AppraisalID,
                                        StageId = E.AppraisalStageID,
                                        AppraisalStageOrder = E.AppraisalStageID,
                                        stageName = extstage.AppraisalStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = ex.EmployeeName,
                                        AppraisalYearId = E.AppraisalYearID
                                    }).Distinct().OrderByDescending(exid => exid.AppraisalId).ToList();

                    List<AppraisalProcessStatus> ReviewerList = new List<AppraisalProcessStatus>();
                    foreach (var item in CommonResult)
                    {
                        tbl_Appraisal_StageEvents LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                                 where empInfo.AppraisalID == item.AppraisalId
                                                                 orderby empInfo.EventDatetime descending
                                                                 select empInfo).FirstOrDefault();
                        tbl_Appraisal_StageEvents LatestEntryManager = new tbl_Appraisal_StageEvents();
                        if (LatestEntry != null)
                        {
                            LatestEntryManager = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                  where empInfo.AppraisalID == item.AppraisalId && (empInfo.FromStageId == 2 && empInfo.ToStageId == 2) && empInfo.EventDatetime >= LatestEntry.EventDatetime
                                                  orderby empInfo.EventDatetime descending
                                                  select empInfo).FirstOrDefault();
                        }
                        if (LatestEntryManager != null)
                        {
                            if (LatestEntryManager.USerId != employeeId)
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

                    CommonResult.Clear();
                    CommonResult = ReviewerList;
                }
                if (TextLink == "GroupHead")
                {
                    CommonResult = (from E in dbContext.tbl_Appraisal_AppraisalMaster
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_Appraisal_Stages on E.AppraisalStageID + 1 equals s.AppraisalStageID into st
                                    join y in dbContext.tbl_Appraisal_YearMaster on E.AppraisalYearID equals y.AppraisalYearID
                                    from extstage in st.DefaultIfEmpty()
                                    where E.GroupHead == employeeId && (E.AppraisalStageID == 3 && y.AppraisalYearFrozenOn <= DateTime.Now) || E.IsCancelled != true
                                         && (FieldChild == 0 || (FieldChild != 0 && (field == "Buisness Group" ? ex.BusinessGroupID == FieldChild : field == "Organization Unit" ? ex.LocationID == FieldChild : field == "Stage Name" ? E.AppraisalStageID == FieldChild : FieldChild == 0))) //field search
                                         && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.tbl_Appraisal_StageEvents on E.AppraisalID equals ese.AppraisalID into eventStageRecord  // Fix to add red Image support

                                    select new AppraisalProcessStatus
                                    {
                                        Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatetime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support

                                        ReportingTo = ex.ReportingTo,
                                        AppraisalId = E.AppraisalID,
                                        StageId = E.AppraisalStageID,
                                        AppraisalStageOrder = E.AppraisalStageID,
                                        stageName = extstage.AppraisalStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = ex.EmployeeName,
                                        AppraisalYearId = E.AppraisalYearID
                                    }).Distinct().OrderByDescending(exid => exid.AppraisalId).ToList();
                }
                mainResult = CommonResult.ToList();
                totalCount = mainResult.Count;
                return mainResult.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AppraisalProcessStatus> GetWatchListDetails(string searchText, string field, string fieldChild, int page, int rows, int employeeId, string TextLink, out int totalCount)
        {
            List<AppraisalProcessStatus> mainResult = new List<AppraisalProcessStatus>();
            List<AppraisalProcessStatus> appraisalCoordinaorResult = new List<AppraisalProcessStatus>();
            List<AppraisalProcessStatus> CommonResult = new List<AppraisalProcessStatus>();

            try
            {
                int FieldChild = 0;
                if (fieldChild != "")
                {
                    FieldChild = Convert.ToInt32(fieldChild) - 1;
                }

                //this logic is for employee himself logins what falls under his Inbox bucket.ie. his own record.

                #region Employee Inbox Section

                if (TextLink == "SelfAppraisal")
                {
                    CommonResult = (from E in dbContext.tbl_Appraisal_AppraisalMaster
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_Appraisal_Stages on (E.AppraisalStageID == 7 || E.AppraisalStageID == 8 || E.AppraisalStageID == 9 ? E.AppraisalStageID : E.AppraisalStageID + 1) equals s.AppraisalStageID
                                        into st
                                    from extstage in st.DefaultIfEmpty()
                                    join y in dbContext.tbl_Appraisal_YearMaster on E.AppraisalYearID equals y.AppraisalYearID into ym
                                    from ya in ym.DefaultIfEmpty()
                                    where E.EmployeeID == employeeId && (E.AppraisalStageID != 0 && E.AppraisalStageID != 5) || (E.EmployeeID == employeeId && E.AppraisalStageID >= 0 && ya.IDFFrozenOn <= DateTime.Now) && E.AppraisalYearID == E.AppraisalYearID || E.AppraisalYearID != E.AppraisalYearID
                                          &&
                                          (FieldChild == 0 ||
                                           (FieldChild != 0 &&
                                            (field == "Buisness Group"
                                                ? ex.BusinessGroupID == FieldChild
                                                : field == "Organization Unit"
                                                    ? ex.LocationID == FieldChild
                                                    : field == "Stage Name" ? E.AppraisalStageID == FieldChild : FieldChild == 0)))
                                        //field search
                                          && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.tbl_Appraisal_StageEvents on E.AppraisalID equals ese.AppraisalID into
                                        eventStageRecord
                                    // Fix to add red Image support

                                    select new AppraisalProcessStatus
                                    {
                                        Field =
                                            eventStageRecord.Any()
                                                ? eventStageRecord.OrderByDescending(x => x.EventDatetime).FirstOrDefault().Action
                                                : string.Empty, // Fix to add red Image support

                                        ReportingTo = ex.ReportingTo,
                                        AppraisalId = E.AppraisalID,
                                        StageId = E.AppraisalStageID,
                                        AppraisalStageOrder = E.AppraisalStageID,
                                        stageName = extstage.AppraisalStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = ex.EmployeeName,
                                        AppraisalYearId = E.AppraisalYearID
                                    }).Distinct().OrderByDescending(exid => exid.AppraisalId).ToList();
                }

                #endregion Employee Inbox Section

                // following logic for checking manager login & any entries fall under managers watchlist bucket

                #region For Primary Approver Inbox Section

                if (TextLink == "Appraiser")
                {
                    CommonResult = (from E in dbContext.tbl_Appraisal_AppraisalMaster
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_Appraisal_Stages on (E.AppraisalStageID == 7 || E.AppraisalStageID == 8 || E.AppraisalStageID == 9 ? E.AppraisalStageID : E.AppraisalStageID + 1) equals s.AppraisalStageID
                                        into st
                                    from extstage in st.DefaultIfEmpty()
                                    join y in dbContext.tbl_Appraisal_YearMaster on E.AppraisalYearID equals y.AppraisalYearID into ym
                                    from ya in ym.DefaultIfEmpty()
                                    where
                                        (E.Appraiser1 == employeeId || E.Appraiser2 == employeeId) &&
                                        E.AppraisalStageID != null && ((E.AppraisalStageID != 4 && E.Appraiser1 == employeeId) || (ya.IDFInitiatedOn < DateTime.Now && E.IDFISAppraiseAgree == false && E.AppraisalStageID == 4 && E.Appraiser2 == employeeId) || (ya.IDFInitiatedOn == null && E.AppraisalStageID == 4 && E.Appraiser1 == employeeId) || (E.Appraiser2 == employeeId)) || E.IsCancelled == true || ((ya.IDFFrozenOn != null || ya.IDFFrozenOn <= DateTime.Now) && E.AppraisalStageID == 4)
                                        &&
                                        (FieldChild == 0 ||
                                         (FieldChild != 0 &&
                                          (field == "Buisness Group"
                                              ? ex.BusinessGroupID == FieldChild
                                              : field == "Organization Unit"
                                                  ? ex.LocationID == FieldChild
                                                  : field == "Stage Name" ? E.AppraisalStageID == FieldChild : FieldChild == 0)))
                                        //field search
                                        && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.tbl_Appraisal_StageEvents on E.AppraisalID equals ese.AppraisalID into
                                        eventStageRecord
                                    // Fix to add red Image support

                                    select new AppraisalProcessStatus
                                    {
                                        Field =
                                            eventStageRecord.Any()
                                                ? eventStageRecord.OrderByDescending(x => x.EventDatetime).FirstOrDefault().Action
                                                : string.Empty, // Fix to add red Image support

                                        ReportingTo = ex.ReportingTo,
                                        AppraisalId = E.AppraisalID,
                                        StageId = E.AppraisalStageID,
                                        AppraisalStageOrder = E.AppraisalStageID,
                                        stageName = extstage.AppraisalStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = ex.EmployeeName,
                                        AppraisalYearId = E.AppraisalYearID,
                                        Appriser2Id = E.Appraiser2,
                                        Appriser1Id = E.Appraiser1
                                    }).Distinct().OrderByDescending(exid => exid.AppraisalId).ToList();

                    List<AppraisalProcessStatus> ManagerList = new List<AppraisalProcessStatus>();
                    foreach (var item in CommonResult)
                    {
                        tbl_Appraisal_StageEvents LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                                 where empInfo.AppraisalID == item.AppraisalId
                                                                 orderby empInfo.EventDatetime descending
                                                                 select empInfo).FirstOrDefault();
                        tbl_Appraisal_StageEvents LatestEntryManager = new tbl_Appraisal_StageEvents();
                        if (LatestEntry != null)
                        {
                            LatestEntryManager = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                  where empInfo.AppraisalID == item.AppraisalId && ((empInfo.FromStageId == 0 || empInfo.FromStageId == 1) && (empInfo.ToStageId == 1 || empInfo.ToStageId == 2))
                                                  && empInfo.EventDatetime <= LatestEntry.EventDatetime && empInfo.USerId == employeeId
                                                  orderby empInfo.EventDatetime descending
                                                  select empInfo).FirstOrDefault();

                            if (LatestEntryManager != null)
                            {
                                if (LatestEntry.Action == "Move Ahead" || LatestEntry.Action == "Approved" || LatestEntry.Action == "" || (LatestEntry.Action == "Reject" && LatestEntry.ToStageId != 1))
                                {
                                    if (LatestEntry.Action == "Reject")
                                    {
                                        if ((LatestEntry.FromStageId == 1 && LatestEntry.ToStageId == 0) || (LatestEntry.FromStageId == 5 && LatestEntry.ToStageId == 4))
                                        {
                                            ManagerList.Add(item);
                                        }
                                        else
                                            continue;
                                    }
                                    else
                                    {
                                        if (LatestEntry.FromStageId == 3 && LatestEntry.ToStageId == 2)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (LatestEntryManager.USerId == employeeId)
                                            {
                                                //to handle reject case
                                                tbl_Appraisal_StageEvents LatestEntryReject = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                                                               where empInfo.AppraisalID == item.AppraisalId && empInfo.Action == "Reject"
                                                                                               orderby empInfo.EventDatetime descending
                                                                                               select empInfo).FirstOrDefault();

                                                tbl_Appraisal_YearMaster currentYear = dbContext.tbl_Appraisal_YearMaster.Where(ed => ed.AppraisalYearID == item.AppraisalYearId).FirstOrDefault();

                                                if (LatestEntryReject != null)
                                                {
                                                    if (LatestEntryManager.EventDatetime > LatestEntryReject.EventDatetime)
                                                        ManagerList.Add(item);
                                                    else
                                                    {
                                                        if (item.Field == "Approved" && (((LatestEntryReject.FromStageId != 1 && LatestEntryReject.ToStageId != 0 && LatestEntry.FromStageId == 0 && LatestEntry.ToStageId == 1)) || ((LatestEntryReject.FromStageId != 2 && LatestEntryReject.ToStageId != 1 && LatestEntry.FromStageId == 1 && LatestEntry.ToStageId == 2)) || ((LatestEntryReject.FromStageId != 3 && LatestEntryReject.ToStageId != 2 && LatestEntry.FromStageId == 2 && LatestEntry.ToStageId == 3)) || ((LatestEntryReject.FromStageId != 4 && LatestEntryReject.ToStageId != 3 && LatestEntry.FromStageId == 3 && LatestEntry.ToStageId == 4)) || ((LatestEntryReject.FromStageId == 5 && LatestEntryReject.ToStageId == 4)) || (LatestEntry.FromStageId == 5 && LatestEntry.ToStageId == 7)))
                                                        {
                                                            ManagerList.Add(item);
                                                        }
                                                        else
                                                        {
                                                            continue;
                                                        }
                                                    }
                                                }
                                                else if (((LatestEntry.FromStageId == 1 && LatestEntry.ToStageId == 2) || (LatestEntry.FromStageId == 5 && LatestEntry.ToStageId == 7) || (LatestEntry.FromStageId == 1 && LatestEntry.ToStageId == 1) || (LatestEntry.FromStageId == 4 && LatestEntry.ToStageId == 5) || (LatestEntry.FromStageId == 2 && LatestEntry.ToStageId == 2) || (LatestEntry.FromStageId == 2 && LatestEntry.ToStageId == 3) || (LatestEntry.FromStageId == 3 && LatestEntry.ToStageId == 4 && (currentYear.IDFInitiatedOn == null || (currentYear.IDFInitiatedOn < DateTime.Now && item.Appriser2Id == employeeId))) || (LatestEntry.FromStageId == 3 && LatestEntry.ToStageId == 4 && (currentYear.IDFFrozenOn <= DateTime.Now))) && LatestEntryManager.USerId == employeeId)
                                                {
                                                    ManagerList.Add(item);
                                                }
                                                else
                                                    continue;
                                            }
                                            else
                                                continue;
                                        }
                                    }
                                }
                                else
                                    if (item.Field != null)
                                    {
                                        if ((item.Field == "Canceled") || (item.Field == "Reject" && ((LatestEntry.FromStageId != 2 && LatestEntry.ToStageId != 1))))
                                        {
                                            ManagerList.Add(item);
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                            }
                            else
                            {
                                if (item.Field != null)
                                {
                                    if (item.Field == "Canceled" || item.Field == "Reject")
                                    {
                                        ManagerList.Add(item);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }

                    CommonResult.Clear();
                    CommonResult = ManagerList;
                }

                #endregion For Primary Approver Inbox Section

                // following logic is to check what falls under HR Admins watchlist ie.
                // HR Admin will handle HR Approval,Exit Interview, Hr Closure, Exit stages.

                #region Secondary approver Inbox Section

                if (TextLink == "Reviewer")
                {
                    CommonResult = (from E in dbContext.tbl_Appraisal_AppraisalMaster
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_Appraisal_Stages on (E.AppraisalStageID == 7 || E.AppraisalStageID == 8 || E.AppraisalStageID == 9 ? E.AppraisalStageID : E.AppraisalStageID + 1) equals s.AppraisalStageID
                                        into st
                                    from extstage in st.DefaultIfEmpty()
                                    where (E.Reviewer1 == employeeId || E.Reviewer2 == employeeId) && E.AppraisalStageID != null || E.IsCancelled == true
                                          &&
                                          (FieldChild == 0 ||
                                           (FieldChild != 0 &&
                                            (field == "Buisness Group"
                                                ? ex.BusinessGroupID == FieldChild
                                                : field == "Organization Unit"
                                                    ? ex.LocationID == FieldChild
                                                    : field == "Stage Name" ? E.AppraisalStageID == FieldChild : FieldChild == 0)))
                                        //field search
                                          && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.tbl_Appraisal_StageEvents on E.AppraisalID equals ese.AppraisalID into
                                        eventStageRecord
                                    // Fix to add red Image support

                                    select new AppraisalProcessStatus
                                    {
                                        Field =
                                            eventStageRecord.Any()
                                                ? eventStageRecord.OrderByDescending(x => x.EventDatetime).FirstOrDefault().Action
                                                : string.Empty, // Fix to add red Image support

                                        ReportingTo = ex.ReportingTo,
                                        AppraisalId = E.AppraisalID,
                                        StageId = E.AppraisalStageID,
                                        AppraisalStageOrder = E.AppraisalStageID,
                                        stageName = extstage.AppraisalStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = ex.EmployeeName,
                                        AppraisalYearId = E.AppraisalYearID
                                    }).Distinct().OrderByDescending(exid => exid.AppraisalId).ToList();

                    List<AppraisalProcessStatus> ReviwerList = new List<AppraisalProcessStatus>();
                    foreach (var item in CommonResult)
                    {
                        tbl_Appraisal_StageEvents LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                                 where empInfo.AppraisalID == item.AppraisalId
                                                                 orderby empInfo.EventDatetime descending
                                                                 select empInfo).FirstOrDefault();
                        tbl_Appraisal_StageEvents LatestEntryManager = new tbl_Appraisal_StageEvents();
                        if (LatestEntry != null)
                        {
                            LatestEntryManager = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                  where empInfo.AppraisalID == item.AppraisalId && (empInfo.FromStageId == 2 && (empInfo.ToStageId == 2 || empInfo.ToStageId == 3 || empInfo.ToStageId == 1))
                                                  && empInfo.EventDatetime <= LatestEntry.EventDatetime && empInfo.USerId == employeeId
                                                  orderby empInfo.EventDatetime descending
                                                  select empInfo).FirstOrDefault();

                            if (LatestEntryManager != null)
                            {
                                if (LatestEntry.Action == "Move Ahead" || LatestEntry.Action == "Approved" || LatestEntry.Action == "" || (LatestEntry.Action == "Reject" && LatestEntry.ToStageId != 2))
                                {
                                    if (LatestEntry.Action == "Reject")
                                    {
                                        if (LatestEntry.FromStageId == 2 && LatestEntry.ToStageId == 1)
                                        {
                                            ReviwerList.Add(item);
                                        }
                                        else if (LatestEntry.FromStageId == 5 && LatestEntry.ToStageId == 4)
                                        {
                                            ReviwerList.Add(item);
                                        }
                                        else
                                            continue;
                                    }
                                    else
                                    {
                                        if (LatestEntry.FromStageId == 3 && LatestEntry.ToStageId == 4)
                                        {
                                            ReviwerList.Add(item);
                                            continue;
                                        }
                                        else
                                        {
                                            if (LatestEntryManager.USerId == employeeId)
                                            {
                                                //to handle reject case
                                                tbl_Appraisal_StageEvents LatestEntryReject = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                                                               where empInfo.AppraisalID == item.AppraisalId && empInfo.Action == "Reject"
                                                                                               orderby empInfo.EventDatetime descending
                                                                                               select empInfo).FirstOrDefault();
                                                if (LatestEntryReject != null)
                                                {
                                                    if (LatestEntryManager.EventDatetime > LatestEntryReject.EventDatetime)
                                                        ReviwerList.Add(item);
                                                    else
                                                    {
                                                        if (item.Field == "Approved" && !(LatestEntry.FromStageId == 1 && LatestEntry.ToStageId == 2))
                                                        {
                                                            ReviwerList.Add(item);
                                                        }
                                                        else
                                                        {
                                                            continue;
                                                        }
                                                    }
                                                }
                                                else if (((LatestEntry.FromStageId == 2 && LatestEntry.ToStageId == 3) || (LatestEntry.FromStageId == 2 && LatestEntry.ToStageId == 2) || (LatestEntry.FromStageId == 4 && LatestEntry.ToStageId == 5) || (LatestEntry.FromStageId == 5 && LatestEntry.ToStageId == 7)) && LatestEntryManager.USerId == employeeId)
                                                {
                                                    ReviwerList.Add(item);
                                                }
                                                else
                                                    continue;
                                            }
                                            else
                                                continue;
                                        }
                                    }
                                }
                                else
                                    if (item.Field != null)
                                    {
                                        if (item.Field == "Canceled")
                                        {
                                            ReviwerList.Add(item);
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                            }
                            else
                            {
                                if (item.Field != null)
                                {
                                    if (item.Field == "Canceled")
                                    {
                                        ReviwerList.Add(item);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }

                    CommonResult.Clear();
                    CommonResult = ReviwerList;
                }

                #endregion Secondary approver Inbox Section

                #region Employee Inbox Section

                if (TextLink == "GroupHead")
                {
                    CommonResult = (from E in dbContext.tbl_Appraisal_AppraisalMaster
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_Appraisal_Stages on (E.AppraisalStageID == 7 || E.AppraisalStageID == 8 || E.AppraisalStageID == 9 ? E.AppraisalStageID : E.AppraisalStageID + 1) equals s.AppraisalStageID
                                        into st
                                    from extstage in st.DefaultIfEmpty()
                                    where E.GroupHead == employeeId && (E.AppraisalStageID != 3) || E.IsCancelled == true
                                          &&
                                          (FieldChild == 0 ||
                                           (FieldChild != 0 &&
                                            (field == "Buisness Group"
                                                ? ex.BusinessGroupID == FieldChild
                                                : field == "Organization Unit"
                                                    ? ex.LocationID == FieldChild
                                                    : field == "Stage Name" ? E.AppraisalStageID == FieldChild : FieldChild == 0)))
                                        //field search
                                          && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.tbl_Appraisal_StageEvents on E.AppraisalID equals ese.AppraisalID into
                                        eventStageRecord
                                    // Fix to add red Image support

                                    select new AppraisalProcessStatus
                                    {
                                        Field =
                                            eventStageRecord.Any()
                                                ? eventStageRecord.OrderByDescending(x => x.EventDatetime).FirstOrDefault().Action
                                                : string.Empty, // Fix to add red Image support

                                        ReportingTo = ex.ReportingTo,
                                        AppraisalId = E.AppraisalID,
                                        StageId = E.AppraisalStageID,
                                        AppraisalStageOrder = E.AppraisalStageID,
                                        stageName = extstage.AppraisalStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = ex.EmployeeName,
                                        AppraisalYearId = E.AppraisalYearID
                                    }).Distinct().OrderByDescending(exid => exid.AppraisalId).ToList();
                }

                #endregion Employee Inbox Section

                #region Employee Inbox Section

                if (TextLink == "AppraisalCoordinator")
                {
                    CommonResult = (from E in dbContext.tbl_Appraisal_AppraisalMaster
                                    join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                    from ex in exp.DefaultIfEmpty()
                                    join s in dbContext.tbl_Appraisal_Stages on (E.AppraisalStageID == 7 || E.AppraisalStageID == 8 || E.AppraisalStageID == 9 ? E.AppraisalStageID : E.AppraisalStageID + 1) equals s.AppraisalStageID
                                        into st
                                    from extstage in st.DefaultIfEmpty()
                                    join Y in dbContext.tbl_Appraisal_YearMaster on E.AppraisalYearID equals Y.AppraisalYearID
                                    into yr
                                    from yrstage in yr.DefaultIfEmpty()
                                    where (E.AppraisalStageID != null && yrstage.AppraisalYearStatus == 0)
                                          &&
                                          (FieldChild == 0 ||
                                           (FieldChild != 0 &&
                                            (field == "Buisness Group"
                                                ? ex.BusinessGroupID == FieldChild
                                                : field == "Organization Unit"
                                                    ? ex.LocationID == FieldChild
                                                    : field == "Stage Name" ? E.AppraisalStageID == FieldChild : FieldChild == 0)))
                                        //field search
                                          && (ex.EmployeeName.Contains(searchText) || ex.EmployeeCode.Contains(searchText))
                                    join ese in dbContext.tbl_Appraisal_StageEvents on E.AppraisalID equals ese.AppraisalID into
                                        eventStageRecord
                                    // Fix to add red Image support

                                    select new AppraisalProcessStatus
                                    {
                                        Field =
                                            eventStageRecord.Any()
                                                ? eventStageRecord.OrderByDescending(x => x.EventDatetime).FirstOrDefault().Action
                                                : string.Empty, // Fix to add red Image support

                                        ReportingTo = ex.ReportingTo,
                                        AppraisalId = E.AppraisalID,
                                        StageId = E.AppraisalStageID,
                                        AppraisalStageOrder = E.AppraisalStageID,
                                        stageName = extstage.AppraisalStage,
                                        EmployeeId = E.EmployeeID,
                                        Employeename = ex.EmployeeName,
                                        AppraisalYearId = E.AppraisalYearID,
                                        IDFFrozenOnDate = yrstage.IDFFrozenOn,
                                        UnFreezedByAdmin = E.UnFreezedByAdmin
                                    }).Distinct().OrderByDescending(exid => exid.AppraisalId).ToList();
                }

                #endregion Employee Inbox Section

                mainResult = CommonResult.ToList();
                var distinctItems = mainResult.GroupBy(x => x.AppraisalId).Select(y => y.First()).ToList();
                totalCount = distinctItems.Count;
                return distinctItems.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Appraisal_GoalAspire GetGoalAspire(int appraisalId)
        {
            tbl_Appraisal_GoalAspire goalAspire = new tbl_Appraisal_GoalAspire();
            try
            {
                dbContext = new HRMSDBEntities();
                goalAspire = (dbContext.tbl_Appraisal_GoalAspire.Where(ed => ed.AppraisalID == appraisalId)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return goalAspire;
        }

        public List<AppraisalParameter> GetParametersDetails(int appraisalId, int employeeId, int AppraisalYearID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<AppraisalParameter> finalValueDriver = new List<AppraisalParameter>();
                List<Parameters> ListParm = (from pm in dbContext.tbl_Appraisal_ParameterMaster
                                             join p in dbContext.tbl_Appraisal_ParameterDesignationMapping on pm.ParameterID equals p.ParameterID
                                             join e in dbContext.HRMS_tbl_PM_Employee on p.DesignationID equals e.DesignationID
                                             where e.EmployeeID == employeeId && pm.AppraisalYearID == AppraisalYearID
                                             select new Parameters
                                             {
                                                 ParameterDesc = pm.Parameter,
                                                 parmID = pm.ParameterID
                                             }).ToList();

                if (ListParm != null)
                {
                    bool ret = saveParametersDescInValueDriver(ListParm, employeeId, appraisalId);
                    if (ret)
                    { }
                    else
                    {
                        return null;
                    }
                    finalValueDriver = (from pm in dbContext.tbl_Appraisal_RatingComments
                                        join p in dbContext.tbl_Appraisal_ParameterMaster on pm.ParameterID equals p.ParameterID
                                        where pm.AppraisalID == appraisalId && pm.EmployeeID == employeeId && p.AppraisalYearID == AppraisalYearID
                                        select new AppraisalParameter
                                        {
                                            appraisalID = pm.AppraisalID,
                                            parameterID = pm.ParameterID,
                                            Parameter = p.Parameter,
                                            employeeID = pm.EmployeeID,
                                            EmpComments = pm.EmployeeComments,
                                            SelfRating = pm.SelfRating,
                                            AppraiserRating1 = pm.Appraiser1Ratings,
                                            AppraiserComments1 = pm.Appraiser1Comments,
                                            AppraiserRating2 = pm.Appraiser2Ratings,
                                            AppraiserComments2 = pm.Appraiser2Comments,
                                            ReviewerRating1 = pm.Reviewer1Ratings,
                                            ReviewerComments1 = pm.Reviewer1Comments,
                                            ReviewerRating2 = pm.Reviewer2Ratings,
                                            ReviewerComments2 = pm.Reviewer2Comments,
                                            GrpHeadRating = pm.GroupHeadRatings,
                                            GrpHeadComments = pm.GroupHeadComments,
                                            OverallReviewRating = pm.OverallReviewerRatings,
                                            OverallReviewRatingComments = pm.OverallReviewerComments,
                                            OverallReview2Rating = pm.OverallReviewer2Ratings,
                                            OverallReview2RatingComments = pm.OverallReviewer2Comments,
                                            OverallGrpHeadRating = pm.OverallGroupHeadRatings,
                                            OverallGrpHeadComments = pm.OverallGroupHeadComments
                                        }).ToList();
                }
                return finalValueDriver;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool saveParametersDescInValueDriver(List<Parameters> ListParm, int empID, int appraisalID)
        {
            bool isAdded = false;
            int parameterId = 0;
            int loggedInEmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);
            tbl_Appraisal_RatingComments addParamDriver;
            foreach (var item in ListParm)
            {
                parameterId = Convert.ToInt16(item.parmID);
                tbl_Appraisal_RatingComments paramDriver = dbContext.tbl_Appraisal_RatingComments.Where(ed => ed.ParameterID == parameterId && ed.EmployeeID == empID && ed.AppraisalID == appraisalID).FirstOrDefault();
                if (paramDriver == null)
                {
                    addParamDriver = new tbl_Appraisal_RatingComments();
                    addParamDriver.ParameterID = parameterId;
                    addParamDriver.EmployeeID = empID;
                    addParamDriver.AppraisalID = appraisalID;
                    addParamDriver.ModifiedBy = loggedInEmployeeId;
                    addParamDriver.ModifiedOn = DateTime.Now;
                    addParamDriver.CreatedBy = loggedInEmployeeId;
                    addParamDriver.CreatedOn = DateTime.Now;
                    dbContext.tbl_Appraisal_RatingComments.AddObject(addParamDriver);
                    dbContext.SaveChanges();
                }
            }

            isAdded = true;
            return isAdded;
        }

        public bool SaveParameterDetailsAppraisal(List<AppraisalParameter> empParameter)
        {
            bool isAdded = false;
            int loggedInEmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);
            dbContext = new HRMSDBEntities();
            if (empParameter != null)
            {
                int count = empParameter.Count();
                int? parameterId = 0;
                int appraiserId = 0;
                string str;
                int emp = 0;
                for (int i = 0; i < count; i++)
                {
                    emp = empParameter[0].employeeID;
                    int stageId = empParameter[count - 1].StageID;
                    parameterId = empParameter[i].parameterID;
                    appraiserId = empParameter[i].appraisalID;
                    tbl_Appraisal_RatingComments objValueDrivers = dbContext.tbl_Appraisal_RatingComments.Where(ed => ed.ParameterID == parameterId && ed.EmployeeID == emp && ed.AppraisalID == appraiserId).FirstOrDefault();

                    if (objValueDrivers != null)
                    {
                        if (empParameter[0].IsManagerOrEmployee == "Employee")
                        {
                            objValueDrivers.SelfRating = empParameter[i].SelfRating == null ? 0 : empParameter[i].SelfRating;
                            objValueDrivers.EmployeeComments = empParameter[i].EmpComments == null ? "" : empParameter[i].EmpComments.Trim();
                        }
                        if (empParameter[0].IsManagerOrEmployee == "Appraiser1")
                        {
                            objValueDrivers.Appraiser1Ratings = empParameter[i].AppraiserRating1 == null ? 0 : empParameter[i].AppraiserRating1;
                            objValueDrivers.Appraiser1Comments = empParameter[i].AppraiserComments1 == null ? "" : empParameter[i].AppraiserComments1.Trim();
                        }
                        if (empParameter[0].IsManagerOrEmployee == "Appraiser2")
                        {
                            objValueDrivers.Appraiser2Ratings = empParameter[i].AppraiserRating2 == null ? 0 : empParameter[i].AppraiserRating2;
                            objValueDrivers.Appraiser2Comments = empParameter[i].AppraiserComments2 == null ? "" : empParameter[i].AppraiserComments2.Trim();
                        }
                        if (empParameter[0].IsManagerOrEmployee == "Reviewer1")
                        {
                            objValueDrivers.Reviewer1Ratings = empParameter[i].ReviewerRating1 == null ? 0 : empParameter[i].ReviewerRating1;
                            objValueDrivers.Reviewer1Comments = empParameter[i].ReviewerComments1 == null ? "" : empParameter[i].ReviewerComments1.Trim();
                        }
                        if (empParameter[0].IsManagerOrEmployee == "Reviewer2")
                        {
                            objValueDrivers.Reviewer2Ratings = empParameter[i].ReviewerRating2 == null ? 0 : empParameter[i].ReviewerRating2;
                            objValueDrivers.Reviewer2Comments = empParameter[i].ReviewerComments2 == null ? "" : empParameter[i].ReviewerComments2.Trim();
                        }
                        if (empParameter[0].IsManagerOrEmployee == "GroupHead" && (stageId != 7 && stageId != 8))
                        {
                            objValueDrivers.GroupHeadRatings = empParameter[i].GrpHeadRating == null ? 0 : empParameter[i].GrpHeadRating;
                            objValueDrivers.GroupHeadComments = empParameter[i].GrpHeadComments == null ? "" : empParameter[i].GrpHeadComments.Trim();
                        }
                        objValueDrivers.ModifiedBy = loggedInEmployeeId;
                        objValueDrivers.ModifiedOn = DateTime.Now;
                        dbContext.SaveChanges();
                    }
                }
                List<tbl_Appraisal_RatingComments> objValueDriver = dbContext.tbl_Appraisal_RatingComments.Where(ed => ed.EmployeeID == emp && ed.AppraisalID == appraiserId).ToList();
                tbl_Appraisal_OverallGroupHeadRatingHistory ratingHistory = dbContext.tbl_Appraisal_OverallGroupHeadRatingHistory.Where(h => h.AppraisalID == appraiserId).FirstOrDefault();
                int stageIdGrpHd = empParameter[count - 1].StageID;
                bool IsIDFFrozen = empParameter[count - 1].IsIDFFrozen;
                bool isUnfreezedByAdmin = empParameter[count - 1].isUnfreezedByAdmin;

                if (empParameter[0].IsManagerOrEmployee == "GroupHead" && (stageIdGrpHd == 7 || stageIdGrpHd == 8 || IsIDFFrozen == true) && isUnfreezedByAdmin == true)
                {
                    if (!string.IsNullOrEmpty(objValueDriver[0].OverallGroupHeadComments))
                        objValueDriver[0].OverallGroupHeadComments = objValueDriver[0].OverallGroupHeadComments.Trim().Replace("\r", "");
                    if (!string.IsNullOrEmpty(empParameter[count - 1].OverallGrpHeadComments))
                        empParameter[count - 1].OverallGrpHeadComments = empParameter[count - 1].OverallGrpHeadComments.Trim().Replace("\r", "");

                    if ((objValueDriver[0].OverallGroupHeadComments != empParameter[count - 1].OverallGrpHeadComments) || (objValueDriver[0].OverallGroupHeadRatings != empParameter[count - 1].OverallGrpHeadRating))
                    {
                        tbl_Appraisal_OverallGroupHeadRatingHistory _ratingHistory = new tbl_Appraisal_OverallGroupHeadRatingHistory();
                        if (ratingHistory == null)
                        {
                            _ratingHistory.AppraisalID = empParameter[count - 1].appraisalID;
                            _ratingHistory.OldOverallGroupHeadRating = objValueDriver[0].OverallGroupHeadRatings;
                            _ratingHistory.OldOverallGroupHeadComments = objValueDriver[0].OverallGroupHeadComments;
                            _ratingHistory.NewOverallGroupHeadRating = empParameter[count - 1].OverallGrpHeadRating;
                            _ratingHistory.NewOverallGroupHeadComments = empParameter[count - 1].OverallGrpHeadComments;
                            _ratingHistory.CreatedBy = loggedInEmployeeId;
                            _ratingHistory.CreatedOn = DateTime.Now;
                        }
                        else
                        {
                            _ratingHistory.AppraisalID = empParameter[count - 1].appraisalID;
                            _ratingHistory.OldOverallGroupHeadRating = objValueDriver[0].OverallGroupHeadRatings;
                            _ratingHistory.OldOverallGroupHeadComments = objValueDriver[0].OverallGroupHeadComments;
                            _ratingHistory.NewOverallGroupHeadRating = empParameter[count - 1].OverallGrpHeadRating;
                            _ratingHistory.NewOverallGroupHeadComments = empParameter[count - 1].OverallGrpHeadComments;
                            _ratingHistory.ModifiedBy = loggedInEmployeeId;
                            _ratingHistory.ModifiedOn = DateTime.Now;
                        }
                        dbContext.tbl_Appraisal_OverallGroupHeadRatingHistory.AddObject(_ratingHistory);
                        dbContext.SaveChanges();
                    }
                }
                foreach (var item in objValueDriver)
                {
                    if (item != null)
                    {
                        if (empParameter[0].IsManagerOrEmployee == "Reviewer1")
                        {
                            item.OverallReviewerRatings = empParameter[count - 1].OverallReviewRating == null ? 0 : empParameter[count - 1].OverallReviewRating;
                            item.OverallReviewerComments = empParameter[count - 1].OverallReviewRatingComments == null ? "" : empParameter[count - 1].OverallReviewRatingComments.Trim();
                            item.ModifiedBy = loggedInEmployeeId;
                            item.ModifiedOn = DateTime.Now;
                            dbContext.SaveChanges();
                        }
                        if (empParameter[0].IsManagerOrEmployee == "Reviewer2")
                        {
                            item.OverallReviewer2Ratings = empParameter[count - 1].OverallReview2Rating == null ? 0 : empParameter[count - 1].OverallReview2Rating;
                            item.OverallReviewer2Comments = empParameter[count - 1].OverallReview2RatingComments == null ? "" : empParameter[count - 1].OverallReview2RatingComments.Trim();
                            item.ModifiedBy = loggedInEmployeeId;
                            item.ModifiedOn = DateTime.Now;
                            dbContext.SaveChanges();
                        }
                        if (empParameter[0].IsManagerOrEmployee == "GroupHead")
                        {
                            item.OverallGroupHeadRatings = empParameter[count - 1].OverallGrpHeadRating == null ? 0 : empParameter[count - 1].OverallGrpHeadRating;
                            item.OverallGroupHeadComments = empParameter[count - 1].OverallGrpHeadComments == null ? "" : empParameter[count - 1].OverallGrpHeadComments.Trim();
                            item.ModifiedBy = loggedInEmployeeId;
                            item.ModifiedOn = DateTime.Now;
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            isAdded = true;
            return isAdded;
        }

        public List<AppraisalProccessShowStatus> GetShowStatusResult(int page, int rows, int AppraisalID, out int totalCount)
        {
            try
            {
                List<AppraisalProccessShowStatus> FinalResult = new List<AppraisalProccessShowStatus>();
                List<AppraisalProccessShowStatus> result = new List<AppraisalProccessShowStatus>();
                AppraisalProccessShowStatus secondresult = new AppraisalProccessShowStatus();
                string ApproverName = string.Empty;
                string ApproverNameFinal = string.Empty;
                string ApproverNameFinal2 = string.Empty;
                int AppraiserApproved = 0;
                int ReviewerApproved = 0;
                var appraisaldetails = (from appraisal in dbContext.tbl_Appraisal_AppraisalMaster where appraisal.AppraisalID == AppraisalID select appraisal).FirstOrDefault();
                //var expenseStageDetails = (from stageEvent in dbContext.tbl_Appraisal_StageEvents where stageEvent.AppraisalID == AppraisalID select stageEvent).FirstOrDefault();
                var LastEnrtyForStatus = (from stageEvent in dbContext.tbl_Appraisal_StageEvents where stageEvent.AppraisalID == AppraisalID select stageEvent).OrderByDescending(x => x.EventDatetime).FirstOrDefault();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                if (LastEnrtyForStatus != null)
                {
                    if (LastEnrtyForStatus.FromStageId == LastEnrtyForStatus.ToStageId)
                    {
                        if (LastEnrtyForStatus.USerId == appraisaldetails.Appraiser1)
                            AppraiserApproved = Convert.ToInt32(LastEnrtyForStatus.USerId);
                        if (LastEnrtyForStatus.USerId == appraisaldetails.Appraiser2)
                            AppraiserApproved = Convert.ToInt32(LastEnrtyForStatus.USerId);
                        if (LastEnrtyForStatus.USerId == appraisaldetails.Reviewer1)
                            ReviewerApproved = Convert.ToInt32(LastEnrtyForStatus.USerId);
                        if (LastEnrtyForStatus.USerId == appraisaldetails.Reviewer2)
                            ReviewerApproved = Convert.ToInt32(appraisaldetails.Reviewer2);
                    }
                }
                if (appraisaldetails.AppraisalStageID == 0)
                {
                    HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(appraisaldetails.EmployeeID);
                    ApproverNameFinal = EmpDetails.EmployeeName;
                }
                if (appraisaldetails.AppraisalStageID == 1 || appraisaldetails.AppraisalStageID == 6)
                {
                    if (appraisaldetails.Appraiser2 != null && AppraiserApproved == 0)
                    {
                        HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(appraisaldetails.Appraiser1.HasValue ? appraisaldetails.Appraiser1.Value : 0);
                        HRMS_tbl_PM_Employee EmpDetails1 = employeeDAL.GetEmployeeDetails(appraisaldetails.Appraiser2.HasValue ? appraisaldetails.Appraiser2.Value : 0);
                        ApproverNameFinal = EmpDetails.EmployeeName;
                        ApproverNameFinal2 = EmpDetails1.EmployeeName;
                    }
                    else
                    {
                        if (AppraiserApproved == appraisaldetails.Appraiser1)
                        {
                            HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(appraisaldetails.Appraiser2.HasValue ? appraisaldetails.Appraiser2.Value : 0);
                            ApproverNameFinal = EmpDetails.EmployeeName;
                        }
                        else
                        {
                            HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(appraisaldetails.Appraiser1.HasValue ? appraisaldetails.Appraiser1.Value : 0);
                            ApproverNameFinal = EmpDetails.EmployeeName;
                        }
                    }
                }
                else if (appraisaldetails.AppraisalStageID == 2)
                {
                    //if (LastEnrtyForStatus.FromStageId == 2 && LastEnrtyForStatus.ToStageId == 2)
                    //{
                    //    HRMS_tbl_PM_Employee EmpDetails1 = employeeDAL.GetEmployeeDetails(appraisaldetails.Reviewer2.HasValue ? appraisaldetails.Reviewer2.Value : 0);
                    //    ApproverNameFinal = EmpDetails1.EmployeeName;
                    //}
                    //else
                    //{
                    //    HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(appraisaldetails.Reviewer1.HasValue ? appraisaldetails.Reviewer1.Value : 0);
                    //    ApproverNameFinal = EmpDetails.EmployeeName;
                    //}
                    if (appraisaldetails.Reviewer2 != null && ReviewerApproved == 0)
                    {
                        HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(appraisaldetails.Reviewer1.HasValue ? appraisaldetails.Reviewer1.Value : 0);
                        HRMS_tbl_PM_Employee EmpDetails1 = employeeDAL.GetEmployeeDetails(appraisaldetails.Reviewer2.HasValue ? appraisaldetails.Reviewer2.Value : 0);
                        ApproverNameFinal = EmpDetails.EmployeeName;
                        ApproverNameFinal2 = EmpDetails1.EmployeeName;
                    }
                    else
                    {
                        if (ReviewerApproved == appraisaldetails.Reviewer1)
                        {
                            HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(appraisaldetails.Reviewer2.HasValue ? appraisaldetails.Reviewer2.Value : 0);
                            ApproverNameFinal = EmpDetails.EmployeeName;
                        }
                        else
                        {
                            HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(appraisaldetails.Reviewer1.HasValue ? appraisaldetails.Reviewer1.Value : 0);
                            ApproverNameFinal = EmpDetails.EmployeeName;
                        }
                    }
                }
                else if (appraisaldetails.AppraisalStageID == 3)
                {
                    HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(appraisaldetails.GroupHead.HasValue ? appraisaldetails.GroupHead.Value : 0);
                    ApproverNameFinal = EmpDetails.EmployeeName;
                }
                else if (appraisaldetails.AppraisalStageID == 4)
                {
                    HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(appraisaldetails.Appraiser1.HasValue ? appraisaldetails.Appraiser1.Value : 0);
                    ApproverNameFinal = EmpDetails.EmployeeName;
                }
                else if (appraisaldetails.AppraisalStageID == 5)
                {
                    HRMS_tbl_PM_Employee EmpDetails = employeeDAL.GetEmployeeDetails(appraisaldetails.EmployeeID);
                    ApproverNameFinal = EmpDetails.EmployeeName;
                }

                result = (from events in dbContext.tbl_Appraisal_StageEvents
                          join employee in dbContext.HRMS_tbl_PM_Employee on events.USerId equals employee.EmployeeID into appraisalemployee
                          from appraisalStageEvent in appraisalemployee.DefaultIfEmpty()
                          join appraisal in dbContext.tbl_Appraisal_AppraisalMaster on events.AppraisalID equals appraisal.AppraisalID into appraisalEvent
                          from appStageEvent in appraisalEvent.DefaultIfEmpty()
                          join stages in dbContext.tbl_Appraisal_Stages on events.Action == "Approved" ? events.ToStageId : (events.FromStageId + 1) equals stages.AppraisalStageID into stage
                          from eventstage in stage.DefaultIfEmpty()
                          join employee in dbContext.HRMS_tbl_PM_Employee on appStageEvent.EmployeeID equals employee.EmployeeID into employeeappraisalevent
                          from employeeappraisal in employeeappraisalevent.DefaultIfEmpty()
                          where appStageEvent.AppraisalID == AppraisalID
                          orderby events.ID ascending
                          select new AppraisalProccessShowStatus
                          {
                              ShowstatusAction = events.Action,
                              ShowstatusActor = appraisalStageEvent.EmployeeName,
                              ShowstatusCurrentStage = eventstage.AppraisalStage,
                              ShowstatusStageID = events.FromStageId,
                              ShowstatusEmployeeCode = employeeappraisal.EmployeeCode,
                              ShowstatusEmployeeId = appraisalStageEvent.EmployeeID,
                              ShowstatusEmployeeName = employeeappraisal.EmployeeName,
                              ShowstatusTime = events.EventDatetime,
                              ShowstatusComments = events.Action == "Reject" || events.Action == "Canceled" ? events.Comments : ""
                          }).ToList();

                if (result.Any())
                    FinalResult.AddRange(result);
                if (appraisaldetails.IsCancelled != true)
                {
                    if (appraisaldetails.AppraisalStageID != 8 && appraisaldetails.AppraisalStageID != 7)
                    {
                        if (ApproverNameFinal2 != "")
                        {
                            secondresult = (from app in dbContext.tbl_Appraisal_AppraisalMaster
                                            join s in dbContext.tbl_Appraisal_Stages on (app.AppraisalStageID + 1) equals s.AppraisalStageID into stage
                                            from AppStage in stage.DefaultIfEmpty()
                                            where app.AppraisalID == AppraisalID
                                            select new AppraisalProccessShowStatus
                                            {
                                                ShowstatusCurrentStage = AppStage.AppraisalStage,
                                                showStatus = "Waiting for " + ApproverNameFinal + "," + ApproverNameFinal2 + "\n to take Action"
                                            }).FirstOrDefault();
                        }
                        else
                        {
                            secondresult = (from app in dbContext.tbl_Appraisal_AppraisalMaster
                                            join s in dbContext.tbl_Appraisal_Stages on (app.AppraisalStageID + 1) equals s.AppraisalStageID into stage
                                            from AppStage in stage.DefaultIfEmpty()
                                            where app.AppraisalID == AppraisalID
                                            select new AppraisalProccessShowStatus
                                            {
                                                ShowstatusCurrentStage = AppStage.AppraisalStage,
                                                showStatus = "Waiting for " + ApproverNameFinal + "\n to take Action"
                                            }).FirstOrDefault();
                        }

                        FinalResult.Add(secondresult);
                    }
                }
                else if (appraisaldetails.AppraisalStageID == 8)
                {
                    secondresult = (from app in dbContext.tbl_Appraisal_AppraisalMaster
                                    join s in dbContext.tbl_Appraisal_Stages on (app.AppraisalStageID + 1) equals s.AppraisalStageID into stage
                                    from AppStage in stage.DefaultIfEmpty()
                                    where app.AppraisalID == AppraisalID
                                    select new AppraisalProccessShowStatus
                                    {
                                        ShowstatusCurrentStage = AppStage.AppraisalStage
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

        public bool DeleteCorporateAppraisalDetails(int corporateId)
        {
            bool isDeleted = false;
            tbl_Appraisal_CorporateContribution corporateDetails = dbContext.tbl_Appraisal_CorporateContribution.Where(cd => cd.CorporateID == corporateId).FirstOrDefault();
            if (corporateDetails != null)
            {
                dbContext.DeleteObject(corporateDetails);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public RatingApprMinMax GetRating(int appraisalYearId)
        {
            RatingApprMinMax ratingMinMax = new RatingApprMinMax();

            try
            {
                Decimal? ratingMin = (from c in dbContext.tbl_Appraisal_RatingMaster
                                      where c.AppraisalYearID == appraisalYearId
                                      select c.Percentage).Min();
                ratingMinMax.min = Convert.ToInt32(ratingMin);
                Decimal? ratingMax = (from c in dbContext.tbl_Appraisal_RatingMaster
                                      where c.AppraisalYearID == appraisalYearId
                                      select c.Percentage).Max();
                ratingMinMax.max = Convert.ToInt32(ratingMax);
            }
            catch (Exception)
            {
                throw;
            }
            return ratingMinMax;
        }

        public List<ActionCount> GetCounts(int employeeId)
        {
            try
            {
                List<ActionCount> employeeresult = (from E in dbContext.tbl_Appraisal_AppraisalMaster
                                                    join y in dbContext.tbl_Appraisal_YearMaster on E.AppraisalYearID equals y.AppraisalYearID
                                                    where (E.EmployeeID == employeeId || (E.Appraiser1 == employeeId) ||
                                                          (E.Appraiser2 == employeeId) || (E.Reviewer1 == employeeId) || (E.Reviewer2 == employeeId) ||
                                                          (E.GroupHead == employeeId)) && (y.IDFFrozenOn >= DateTime.Now || y.IDFFrozenOn == null)
                                                    select new ActionCount
                                                    {
                                                        EmployeeId = E.EmployeeID,
                                                        Appraiser1 = E.Appraiser1,
                                                        Appraiser2 = E.Appraiser2,
                                                        Reviewer1 = E.Reviewer1,
                                                        Reviewer2 = E.Reviewer2,
                                                        GroupHead = E.GroupHead,
                                                        StageId = E.AppraisalStageID,
                                                        IDFInitiatedOn = y.IDFInitiatedOn,
                                                        IDfFrozenOn = y.IDFFrozenOn,
                                                        AppraisalYearFrozenOn = y.AppraisalYearFrozenOn
                                                    }).ToList();

                //(E.AppraisalStageID == 4 && y.IDFInitiatedOn <= DateTime.Now) && E.Appraiser1 == employeeId)
                //dbContext.tbl_Appraisal_AppraisalMaster.Where(E => E.EmployeeID == employeeId || (E.Appraiser1 == employeeId) ||
                //         (E.Appraiser2 == employeeId) || (E.Reviewer1 == employeeId) || (E.Reviewer2 == employeeId) || (E.GroupHead == employeeId)).ToList();

                return employeeresult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<GuideLines> getGuileLines(int appraisalYearId)
        {
            List<GuideLines> guidlines = new List<GuideLines>();
            guidlines = (from RatingMaster in dbContext.tbl_Appraisal_RatingMaster
                         where RatingMaster.AppraisalYearID == appraisalYearId
                         select new GuideLines
                         {
                             Percentage = RatingMaster.Percentage,
                             Rating = RatingMaster.Rating,
                             Description = RatingMaster.Description
                         }).ToList();

            return guidlines;
        }

        public ProjectAchievementAppraisal GetAddApprisalDetailsShowHistory(int? AppraisalID, int RequirementID)
        {
            try
            {
                ProjectAchievementAppraisal AddAppdetailshistory = new ProjectAchievementAppraisal();
                AddAppdetailshistory = (from otherdetails in dbContext.tbl_Appraisal_ProjectAchivement
                                        where otherdetails.AppraisalID == AppraisalID && otherdetails.ProjectID == RequirementID
                                        select new ProjectAchievementAppraisal
                                        {
                                            FileName = otherdetails.FileName,
                                            FilePath = otherdetails.FilePath,
                                            AppraisalID = otherdetails.AppraisalID,
                                            ProjAchieveID = otherdetails.ProjectID
                                        }).FirstOrDefault();
                return AddAppdetailshistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //logic is not added yet
        public bool ApproveAppraisal(int appraiserId, int EmployeeId, string StausApprover, string isMngrOrEmpElement, string appraiseeComments, string strengthComments, string improvementComments)
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

                tbl_Appraisal_AppraisalMaster confTable = (from data in dbContext.tbl_Appraisal_AppraisalMaster
                                                           where data.AppraisalID == appraiserId
                                                           select data).FirstOrDefault();

                // get ToStageId
                int ToStage = (from id in dbContext.tbl_Appraisal_StageEvents
                               where id.AppraisalID == confTable.AppraisalID
                               orderby id.EventDatetime descending
                               select id.ToStageId.HasValue ? id.ToStageId.Value : 0).FirstOrDefault();

                tbl_Appraisal_StageEvents confEvent = (from data in dbContext.tbl_Appraisal_StageEvents
                                                       where data.AppraisalID == confTable.AppraisalID && data.Action == "Reject"
                                                       orderby data.EventDatetime descending
                                                       select data).FirstOrDefault();

                UserID = loginuser.EmployeeID;

                if (confTable == null)
                {
                    isAdded = false;
                    return isAdded;
                }
                else
                {
                    tbl_Appraisal_AppraisalMaster appMaster = new tbl_Appraisal_AppraisalMaster();
                    //if(strengthComments != null)
                    //confTable.StrengthComments = strengthComments.Trim();
                    //if (improvementComments != null)
                    //confTable.ImprovementComments = improvementComments.Trim();

                    //rejected by employee
                    if (StausApprover == "Reject")
                    {
                        tbl_Appraisal_StageEvents corporate = new tbl_Appraisal_StageEvents();
                        corporate.AppraisalID = confTable.AppraisalID;
                        corporate.USerId = UserID;
                        corporate.FromStageId = ToStage;
                        corporate.ToStageId = ToStage - 1;
                        corporate.Action = StausApprover.Trim();
                        corporate.EventDatetime = DateTime.Now;
                        corporate.Comments = appraiseeComments;
                        if (isMngrOrEmpElement == "Employee" && confTable.AppraisalStageID == 5)
                        {
                            confTable.IDFISAppraiseAgree = false;
                            confTable.IDFAprraiseComment = appraiseeComments;
                            corporate.Comments = appraiseeComments;
                        }
                        dbContext.tbl_Appraisal_StageEvents.AddObject(corporate);

                        confTable.AppraisalStageID = ToStage - 1;
                        confTable.CreatedOn = DateTime.Now;
                    }
                    //escalate to HR by Appraiser1
                    else if (StausApprover == "EscalateToHR" && isMngrOrEmpElement == "Appraiser1")
                    {
                        tbl_Appraisal_StageEvents corporate = new tbl_Appraisal_StageEvents();
                        corporate.AppraisalID = confTable.AppraisalID;
                        corporate.USerId = UserID;
                        corporate.FromStageId = ToStage;
                        corporate.ToStageId = 8;
                        corporate.Action = StausApprover.Trim();
                        corporate.EventDatetime = DateTime.Now;
                        dbContext.tbl_Appraisal_StageEvents.AddObject(corporate);
                        confTable.AppraisalStageID = 8;
                    }
                    else
                    {
                        if (isMngrOrEmpElement == "Appraiser1" || isMngrOrEmpElement == "Appraiser2" || isMngrOrEmpElement == "Reviewer1" || isMngrOrEmpElement == "Reviewer2")
                        {
                            if (((confTable.Appraiser2 != null && confTable.Appraiser2 != 0) && confTable.AppraisalStageID == 1) || ((confTable.Reviewer2 != null && confTable.Reviewer2 != 0) && confTable.AppraisalStageID == 2))
                            {
                                tbl_Appraisal_StageEvents LatestEntry = new tbl_Appraisal_StageEvents();
                                if (confEvent != null)
                                {
                                    if (isMngrOrEmpElement == "Appraiser2" || isMngrOrEmpElement == "Appraiser1")
                                    {
                                        LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                       where (empInfo.FromStageId == 1 && empInfo.ToStageId == 1) && empInfo.AppraisalID == confTable.AppraisalID
                                                            && (confTable.Appraiser1 == empInfo.USerId || confTable.Appraiser2 == empInfo.USerId)
                                                            && empInfo.EventDatetime > confEvent.EventDatetime
                                                       orderby empInfo.EventDatetime descending
                                                       select empInfo).FirstOrDefault();
                                    }
                                    else if (isMngrOrEmpElement == "Reviewer2" || isMngrOrEmpElement == "Reviewer1")
                                    {
                                        LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                       where (empInfo.FromStageId == 2 && empInfo.ToStageId == 2) && empInfo.AppraisalID == confTable.AppraisalID
                                                       && (confTable.Reviewer1 == empInfo.USerId || confTable.Reviewer2 == empInfo.USerId)
                                                       && empInfo.EventDatetime > confEvent.EventDatetime
                                                       orderby empInfo.EventDatetime descending
                                                       select empInfo).FirstOrDefault();
                                    }
                                }
                                else
                                {
                                    if (isMngrOrEmpElement == "Appraiser2" || isMngrOrEmpElement == "Appraiser1")
                                    {
                                        LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                       where (empInfo.FromStageId == 1 && empInfo.ToStageId == 1) && empInfo.AppraisalID == confTable.AppraisalID
                                                            && (confTable.Appraiser1 == empInfo.USerId || confTable.Appraiser2 == empInfo.USerId)
                                                       orderby empInfo.EventDatetime descending
                                                       select empInfo).FirstOrDefault();
                                    }
                                    else if (isMngrOrEmpElement == "Reviewer2" || isMngrOrEmpElement == "Reviewer1")
                                    {
                                        LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                       where (empInfo.FromStageId == 2 && empInfo.ToStageId == 2) && empInfo.AppraisalID == confTable.AppraisalID
                                                       && (confTable.Reviewer1 == empInfo.USerId || confTable.Reviewer2 == empInfo.USerId)
                                                       orderby empInfo.EventDatetime descending
                                                       select empInfo).FirstOrDefault();
                                    }
                                }

                                tbl_Appraisal_StageEvents TotalRecords = new tbl_Appraisal_StageEvents();
                                if (LatestEntry != null)
                                {
                                    TotalRecords = (from total in dbContext.tbl_Appraisal_StageEvents
                                                    where total.AppraisalID == confTable.AppraisalID && total.Action == "Move Ahead"
                                                    orderby total.EventDatetime descending
                                                    select total).FirstOrDefault();
                                    confTable.AppraisalStageID = confTable.AppraisalStageID + 1;
                                }
                                else
                                    confTable.AppraisalStageID = confTable.AppraisalStageID;
                                tbl_Appraisal_StageEvents corporate = new tbl_Appraisal_StageEvents();
                                corporate.AppraisalID = confTable.AppraisalID;
                                corporate.USerId = UserID;
                                corporate.FromStageId = ToStage;
                                corporate.Action = "Move Ahead";
                                corporate.EventDatetime = DateTime.Now;
                                confTable.CreatedOn = DateTime.Now;
                                if (TotalRecords.AppraisalID != 0)
                                {
                                    //confTable.AppraisalStageID = ToStage + 1;
                                    corporate.ToStageId = ToStage + 1;
                                }
                                else
                                    corporate.ToStageId = ToStage;

                                dbContext.tbl_Appraisal_StageEvents.AddObject(corporate);
                            }
                            else
                            {
                                tbl_Appraisal_StageEvents corporate = new tbl_Appraisal_StageEvents();
                                corporate.AppraisalID = confTable.AppraisalID;
                                corporate.USerId = UserID;
                                corporate.FromStageId = ToStage;
                                corporate.ToStageId = ToStage + 1;
                                corporate.Action = StausApprover.Trim();
                                corporate.EventDatetime = DateTime.Now;
                                dbContext.tbl_Appraisal_StageEvents.AddObject(corporate);
                                confTable.AppraisalStageID = confTable.AppraisalStageID + 1;
                            }
                        }
                        else
                        {
                            if (isMngrOrEmpElement == "Employee" && confTable.AppraisalStageID == 5)   //employee accept- appraisal completed
                            {
                                tbl_Appraisal_StageEvents corporate = new tbl_Appraisal_StageEvents();
                                corporate.AppraisalID = confTable.AppraisalID;
                                corporate.USerId = UserID;
                                corporate.FromStageId = ToStage;
                                corporate.ToStageId = 7;
                                corporate.Action = StausApprover.Trim();
                                corporate.EventDatetime = DateTime.Now;
                                dbContext.tbl_Appraisal_StageEvents.AddObject(corporate);
                                confTable.AppraisalStageID = 7;
                                confTable.IDFISAppraiseAgree = true;
                                confTable.IDFAprraiseComment = appraiseeComments;
                            }
                            else
                            {
                                tbl_Appraisal_StageEvents corporate = new tbl_Appraisal_StageEvents();
                                corporate.AppraisalID = confTable.AppraisalID;
                                corporate.USerId = UserID;
                                corporate.FromStageId = ToStage;
                                corporate.ToStageId = ToStage + 1;
                                corporate.Action = StausApprover.Trim();
                                corporate.EventDatetime = DateTime.Now;
                                dbContext.tbl_Appraisal_StageEvents.AddObject(corporate);
                                confTable.AppraisalStageID = confTable.AppraisalStageID + 1;
                            }
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

        public bool SubmitIdfEmployeeStageDetails(int appraisalId, string appraiseeComments, bool isAppraiseeAgree, int nextStageId)
        {
            bool isAdded = false;
            //int? apraisalID = (int?) appraisalId;
            tbl_Appraisal_AppraisalMaster appraisal = dbContext.tbl_Appraisal_AppraisalMaster.Where(ap => ap.AppraisalID == appraisalId).FirstOrDefault();
            if (appraisal != null)
            {
                appraisal.IDFISAppraiseAgree = isAppraiseeAgree;
                appraisal.IDFAprraiseComment = appraiseeComments;
                appraisal.AppraisalID = appraisalId;
                appraisal.AppraisalStageID = nextStageId;

                dbContext.SaveChanges();
                isAdded = true;
            }
            return isAdded;
        }

        public bool SaveCommentDetails(int ApprisalId, string comments, string CommentType)
        {
            bool status = false;
            try
            {
                tbl_Appraisal_AppraisalMaster emp = dbContext.tbl_Appraisal_AppraisalMaster.Where(ed => ed.AppraisalID == ApprisalId).FirstOrDefault();
                if (emp != null)
                {
                    //if (CommentType == "rejected")
                    //{
                    //    emp.RejectComment = comments;
                    //    dbContext.SaveChanges();
                    //}

                    if (CommentType == "canceled")
                    {
                        emp.CancelComment = comments;
                        dbContext.SaveChanges();
                    }
                }

                status = true;
            }
            catch
            {
                throw;
            }
            return status;
        }

        public AppraisalProcessModel getAppriasalDetails(int ApprisalId)
        {
            try
            {
                AppraisalProcessModel AppriasalDetails = new AppraisalProcessModel();
                AppriasalDetails = (from App in dbContext.tbl_Appraisal_AppraisalMaster
                                    join employee in dbContext.HRMS_tbl_PM_Employee on App.EmployeeID equals employee.EmployeeID into exemployee
                                    from employeeDel in exemployee.DefaultIfEmpty()
                                    where App.AppraisalID == ApprisalId
                                    select new AppraisalProcessModel
                                    {
                                        appraisalId = App.AppraisalID,
                                        EmployeeID = App.EmployeeID,
                                        Employeename = employeeDel.EmployeeName,
                                        StageID = App.AppraisalStageID,
                                        Appriser1Id = App.Appraiser1,
                                        Appriser2Id = App.Appraiser2,
                                        Reviwer1Id = App.Reviewer1,
                                        Reviwer2Id = App.Reviewer2,
                                        GroupHeadId = App.GroupHead
                                    }).FirstOrDefault();
                return AppriasalDetails;
            }
            catch
            {
                throw;
            }
        }

        public bool DeletedAllAppriasalDetails(int ApprisalId, string employeeId, string comments)
        {
            bool isDeleted = false;

            List<tbl_Appraisal_AdditionalQualification> _Appraisal_AdditionalQualification = (from qualification in dbContext.tbl_Appraisal_AdditionalQualification
                                                                                              where qualification.AppraisalID == ApprisalId
                                                                                              select qualification).ToList();

            List<tbl_Appraisal_CorporateContribution> _Appraisal_CorporateContribution = (from CorporateContribution in dbContext.tbl_Appraisal_CorporateContribution
                                                                                          where CorporateContribution.AppraisalID == ApprisalId
                                                                                          select CorporateContribution).ToList();

            List<tbl_Appraisal_EmpGrowthSummary> _Appraisal_EmpGrowthSummary = (from EmpGrowthSummary in dbContext.tbl_Appraisal_EmpGrowthSummary
                                                                                where EmpGrowthSummary.AppraisalID == ApprisalId
                                                                                select EmpGrowthSummary).ToList();

            List<tbl_Appraisal_EmployeeGrowth> _Appraisal_EmployeeGrowth = (from EmployeeGrowth in dbContext.tbl_Appraisal_EmployeeGrowth
                                                                            where EmployeeGrowth.AppraisalID == ApprisalId
                                                                            select EmployeeGrowth).ToList();

            List<tbl_Appraisal_EmpSuccessionPlanning> _Appraisal_EmpSuccessionPlanning = (from EmpSuccessionPlanning in dbContext.tbl_Appraisal_EmpSuccessionPlanning
                                                                                          where EmpSuccessionPlanning.AppraisalID == ApprisalId
                                                                                          select EmpSuccessionPlanning).ToList();

            List<tbl_Appraisal_GoalAspire> _Appraisal_GoalAspire = (from GoalAspire in dbContext.tbl_Appraisal_GoalAspire
                                                                    where GoalAspire.AppraisalID == ApprisalId
                                                                    select GoalAspire).ToList();

            List<tbl_Appraisal_IDFAppraiserAddImprovement> _Appraisal_IDFAppraiserAddImprovement = (from IDFAppraiserAddImprovement in dbContext.tbl_Appraisal_IDFAppraiserAddImprovement
                                                                                                    where IDFAppraiserAddImprovement.AppraisalID == ApprisalId
                                                                                                    select IDFAppraiserAddImprovement).ToList();

            List<tbl_Appraisal_IDFAppraiserAddStrength> _Appraisal_IDFAppraiserAddStrength = (from IDFAppraiserAddStrength in dbContext.tbl_Appraisal_IDFAppraiserAddStrength
                                                                                              where IDFAppraiserAddStrength.AppraisalID == ApprisalId
                                                                                              select IDFAppraiserAddStrength).ToList();

            List<tbl_Appraisal_IDFTrainingProgram> _Appraisal_IDFTrainingProgram = (from IDFTrainingProgram in dbContext.tbl_Appraisal_IDFTrainingProgram
                                                                                    where IDFTrainingProgram.AppraisalID == ApprisalId
                                                                                    select IDFTrainingProgram).ToList();

            List<tbl_Appraisal_OverallGroupHeadRatingHistory> _Appraisal_OverallGroupHeadRatingHistory = (from OverallGroupHeadRatingHistory in dbContext.tbl_Appraisal_OverallGroupHeadRatingHistory
                                                                                                          where OverallGroupHeadRatingHistory.AppraisalID == ApprisalId
                                                                                                          select OverallGroupHeadRatingHistory).ToList();

            List<tbl_Appraisal_PerformanceHinders> _Appraisal_PerformanceHinders = (from PerformanceHinders in dbContext.tbl_Appraisal_PerformanceHinders
                                                                                    where PerformanceHinders.AppraisalID == ApprisalId
                                                                                    select PerformanceHinders).ToList();

            List<tbl_Appraisal_ProjectAchivement> _Appraisal_ProjectAchivement = (from ProjectAchivement in dbContext.tbl_Appraisal_ProjectAchivement
                                                                                  where ProjectAchivement.AppraisalID == ApprisalId
                                                                                  select ProjectAchivement).ToList();

            List<tbl_Appraisal_SkillAquired> _Appraisal_SkillAquired = (from SkillAquired in dbContext.tbl_Appraisal_SkillAquired
                                                                        where SkillAquired.AppraisalID == ApprisalId
                                                                        select SkillAquired).ToList();

            List<tbl_Appraisal_SuccessionPlanning> _Appraisal_SuccessionPlanning = (from SuccessionPlanning in dbContext.tbl_Appraisal_SuccessionPlanning
                                                                                    where SuccessionPlanning.AppraisalID == ApprisalId
                                                                                    select SuccessionPlanning).ToList();

            List<tbl_Appraisal_RatingComments> _Appraisal_RatingComments = (from RatingComments in dbContext.tbl_Appraisal_RatingComments
                                                                            where RatingComments.AppraisalID == ApprisalId
                                                                            select RatingComments).ToList();

            List<tbl_Appraisal_PromotionRecommendation> _PromotionRecommendation = (from StageEvents in dbContext.tbl_Appraisal_PromotionRecommendation
                                                                                    where StageEvents.AppraisalID == ApprisalId
                                                                                    select StageEvents).ToList();
            List<tbl_Appraisal_PromotionRecommendationParameters> _PromotionRecommendationParameters = new List<tbl_Appraisal_PromotionRecommendationParameters>();
            foreach (var param in _PromotionRecommendation)
            {
                _PromotionRecommendationParameters = (from StageEvents in dbContext.tbl_Appraisal_PromotionRecommendationParameters
                                                      where StageEvents.PromoRecomID == param.PromoRecomID
                                                      select StageEvents).ToList();
            }

            List<tbl_Appraisal_AppraisalMaster> _Appraisal_AppraisalMaster = (from AppraisalMaster in dbContext.tbl_Appraisal_AppraisalMaster
                                                                              where AppraisalMaster.AppraisalID == ApprisalId
                                                                              select AppraisalMaster).ToList();

            List<tbl_Appraisal_StageEvents> _Appraisal_StageEvents = (from StageEvents in dbContext.tbl_Appraisal_StageEvents
                                                                      where StageEvents.AppraisalID == ApprisalId
                                                                      select StageEvents).ToList();

            if (_PromotionRecommendationParameters != null)
            {
                foreach (var item in _PromotionRecommendationParameters)
                {
                    dbContext.tbl_Appraisal_PromotionRecommendationParameters.DeleteObject(item);
                }
            }
            if (_PromotionRecommendation != null)
            {
                foreach (var item in _PromotionRecommendation)
                {
                    dbContext.tbl_Appraisal_PromotionRecommendation.DeleteObject(item);
                }
            }
            if (_Appraisal_AdditionalQualification != null)
            {
                foreach (var item in _Appraisal_AdditionalQualification)
                {
                    dbContext.tbl_Appraisal_AdditionalQualification.DeleteObject(item);
                }
            }
            if (_Appraisal_CorporateContribution != null)
            {
                foreach (var item in _Appraisal_CorporateContribution)
                {
                    dbContext.tbl_Appraisal_CorporateContribution.DeleteObject(item);
                }
            }
            if (_Appraisal_EmpGrowthSummary != null)
            {
                foreach (var item in _Appraisal_EmpGrowthSummary)
                {
                    dbContext.tbl_Appraisal_EmpGrowthSummary.DeleteObject(item);
                }
            }
            if (_Appraisal_EmployeeGrowth != null)
            {
                foreach (var item in _Appraisal_EmployeeGrowth)
                {
                    dbContext.tbl_Appraisal_EmployeeGrowth.DeleteObject(item);
                }
            }
            if (_Appraisal_EmpSuccessionPlanning != null)
            {
                foreach (var item in _Appraisal_EmpSuccessionPlanning)
                {
                    dbContext.tbl_Appraisal_EmpSuccessionPlanning.DeleteObject(item);
                }
            }
            if (_Appraisal_GoalAspire != null)
            {
                foreach (var item in _Appraisal_GoalAspire)
                {
                    dbContext.tbl_Appraisal_GoalAspire.DeleteObject(item);
                }
            }
            if (_Appraisal_IDFAppraiserAddImprovement != null)
            {
                foreach (var item in _Appraisal_IDFAppraiserAddImprovement)
                {
                    dbContext.tbl_Appraisal_IDFAppraiserAddImprovement.DeleteObject(item);
                }
            } if (_Appraisal_IDFAppraiserAddStrength != null)
            {
                foreach (var item in _Appraisal_IDFAppraiserAddStrength)
                {
                    dbContext.tbl_Appraisal_IDFAppraiserAddStrength.DeleteObject(item);
                }
            }
            if (_Appraisal_IDFTrainingProgram != null)
            {
                foreach (var item in _Appraisal_IDFTrainingProgram)
                {
                    dbContext.tbl_Appraisal_IDFTrainingProgram.DeleteObject(item);
                }
            }
            if (_Appraisal_OverallGroupHeadRatingHistory != null)
            {
                foreach (var item in _Appraisal_OverallGroupHeadRatingHistory)
                {
                    dbContext.tbl_Appraisal_OverallGroupHeadRatingHistory.DeleteObject(item);
                }
            } if (_Appraisal_PerformanceHinders != null)
            {
                foreach (var item in _Appraisal_PerformanceHinders)
                {
                    dbContext.tbl_Appraisal_PerformanceHinders.DeleteObject(item);
                }
            } if (_Appraisal_ProjectAchivement != null)
            {
                foreach (var item in _Appraisal_ProjectAchivement)
                {
                    dbContext.tbl_Appraisal_ProjectAchivement.DeleteObject(item);
                }
            } if (_Appraisal_SkillAquired != null)
            {
                foreach (var item in _Appraisal_SkillAquired)
                {
                    dbContext.tbl_Appraisal_SkillAquired.DeleteObject(item);
                }
            } if (_Appraisal_SuccessionPlanning != null)
            {
                foreach (var item in _Appraisal_SuccessionPlanning)
                {
                    dbContext.tbl_Appraisal_SuccessionPlanning.DeleteObject(item);
                }
            } if (_Appraisal_RatingComments != null)
            {
                foreach (var item in _Appraisal_RatingComments)
                {
                    dbContext.tbl_Appraisal_RatingComments.DeleteObject(item);
                }
            } if (_Appraisal_StageEvents != null)
            {
                foreach (var item in _Appraisal_StageEvents)
                {
                    dbContext.tbl_Appraisal_StageEvents.DeleteObject(item);
                }
            } if (_Appraisal_AppraisalMaster != null)
            {
                foreach (var item in _Appraisal_AppraisalMaster)
                {
                    dbContext.tbl_Appraisal_AppraisalMaster.DeleteObject(item);
                }
            }
            dbContext.SaveChanges();
            isDeleted = true;
            //EmployeeDAL employeeDAL = new EmployeeDAL();
            //string loginName = System.Web.HttpContext.Current.User.Identity.Name;
            //string loginUserId = loginName;
            //HRMS_tbl_PM_Employee loginuser = employeeDAL.GetEmployeeDetailsByEmployeeCode(loginUserId);
            //int UserID = loginuser.EmployeeID;

            //tbl_Appraisal_AppraisalMaster confTable = (from data in dbContext.tbl_Appraisal_AppraisalMaster
            //                                           where data.AppraisalID == ApprisalId
            //                                           select data).FirstOrDefault();
            //int ToStage = (from id in dbContext.tbl_Appraisal_StageEvents
            //               where id.AppraisalID == confTable.AppraisalID
            //               orderby id.EventDatetime descending
            //               select id.ToStageId.HasValue ? id.ToStageId.Value : 0).FirstOrDefault();
            //tbl_Appraisal_AppraisalMaster CancleFromAppMastertbl = dbContext.tbl_Appraisal_AppraisalMaster.Where(cd => cd.AppraisalID == ApprisalId).FirstOrDefault();
            //if (ApprisalId > 0)
            //{
            //    if (CancleFromAppMastertbl != null)
            //    {
            //        CancleFromAppMastertbl.IsCancelled = true;
            //        tbl_Appraisal_StageEvents corporate = new tbl_Appraisal_StageEvents();
            //        corporate.AppraisalID = confTable.AppraisalID;
            //        corporate.USerId = UserID;
            //        corporate.FromStageId = ToStage;
            //        corporate.ToStageId = ToStage;
            //        corporate.Action = "Canceled";
            //        corporate.Comments = comments;
            //        corporate.EventDatetime = DateTime.Now;
            //        dbContext.tbl_Appraisal_StageEvents.AddObject(corporate);
            //    }

            //    dbContext.SaveChanges();
            //    isDeleted = true;
            //}
            return isDeleted;
        }

        public AppraisalProcessResponse AppraisalProcessUnfreezeFreeze(int appraisalId, int employeeId, bool isUnfreezedOrFreezed)
        {
            try
            {
                AppraisalProcessResponse response = new AppraisalProcessResponse();
                response.isFreezed = false;
                response.isUnfreezed = false;
                response.isAdded = false;

                tbl_Appraisal_AppraisalMaster _AppraisalMaster = dbContext.tbl_Appraisal_AppraisalMaster.Where(appraisal => appraisal.AppraisalID == appraisalId).FirstOrDefault();
                if (_AppraisalMaster != null)
                {
                    if (isUnfreezedOrFreezed == true)
                    {
                        _AppraisalMaster.UnFreezedByAdmin = isUnfreezedOrFreezed;
                        response.isUnfreezed = true;
                    }
                    else
                    {
                        _AppraisalMaster.UnFreezedByAdmin = isUnfreezedOrFreezed;
                        response.isFreezed = true;
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

        public List<ViewGroupHeadHistoryModel> GetGroupHeadRatingsHistoryList(int appraisalId)
        {
            try
            {
                List<ViewGroupHeadHistoryModel> groupHeadHistoryList = new List<ViewGroupHeadHistoryModel>();
                groupHeadHistoryList = (from history in dbContext.tbl_Appraisal_OverallGroupHeadRatingHistory
                                        join appraisalMaster in dbContext.tbl_Appraisal_AppraisalMaster on history.AppraisalID equals appraisalMaster.AppraisalID into master
                                        from masterList in master.DefaultIfEmpty()
                                        join createdBy in dbContext.HRMS_tbl_PM_Employee on history.CreatedBy equals createdBy.EmployeeID into c
                                        from createdByList in c.DefaultIfEmpty()
                                        join modifiedBy in dbContext.HRMS_tbl_PM_Employee on history.ModifiedBy equals modifiedBy.EmployeeID into m
                                        from modifiedByList in m.DefaultIfEmpty()
                                        where history.AppraisalID == appraisalId
                                        orderby history.GroupHeadHistoryID ascending
                                        select new ViewGroupHeadHistoryModel
                                        {
                                            OldOverallGroupHeadRating = history.OldOverallGroupHeadRating,
                                            OldOverallGroupHeadComments = history.OldOverallGroupHeadComments,
                                            NewOverallGroupHeadRating = history.NewOverallGroupHeadRating,
                                            NewOverallGroupHeadComments = history.NewOverallGroupHeadComments,
                                            CreatedByName = createdByList.EmployeeName,
                                            CreatedOn = history.CreatedOn,
                                            ModifiedByName = modifiedByList.EmployeeName,
                                            ModifiedOn = history.ModifiedOn
                                        }).ToList();

                return groupHeadHistoryList.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CertificationDetails> GetEmployeeCertificationDetails(int employeeId, int appraisalId)
        {
            List<CertificationDetails> pendingApprover = new List<CertificationDetails>();
            List<CertificationDetails> approved = new List<CertificationDetails>();

            var appraisalYear = (from app in dbContext.tbl_Appraisal_AppraisalMaster
                                 join year in dbContext.tbl_Appraisal_YearMaster on app.AppraisalYearID equals year.AppraisalYearID
                                 where app.AppraisalID == appraisalId
                                 select year.AppraisalYear).FirstOrDefault();

            int startYear = Convert.ToInt32(appraisalYear.Substring(0, appraisalYear.IndexOf("-")));
            DateTime startDate, endDate;
            startDate = Convert.ToDateTime("4/1/" + startYear);
            endDate = Convert.ToDateTime("3/31/" + (startYear + 1));

            //var query = from category in mycatg
            //            where category.IsPublic == 1
            //               || category.FirstName == "XXX"
            //            group 1 by category.Catg into grouped
            //            select new
            //            {
            //                Catg = grouped.Key,
            //                Count = grouped.Count()
            //            };

            pendingApprover = (from certificationHistory in dbContext.tbl_PM_EmployeeCertificationMatrixHistory
                               join certification in dbContext.tbl_PM_Certifications on certificationHistory.CertificationID equals certification.CertificationID
                               where (certificationHistory.Status == null || certificationHistory.Status == 0) && certificationHistory.CertificationDate >= startDate
                                     && certificationHistory.CertificationDate <= endDate && certificationHistory.EmployeeID == employeeId
                               select new CertificationDetails
                               {
                                   EmployeeCertificationID = certificationHistory.EmployeeCertificationID,
                                   CertificationName = certification.CertificationName,
                                   CertificationNo = certificationHistory.CertificationNo,
                                   Institution = certificationHistory.InstituteName,
                                   CertificationDate = certificationHistory.CertificationDate,
                                   CertificationScore = certificationHistory.TotalScore,
                                   CertificationGrade = certificationHistory.Grade
                               }).ToList();

            if (pendingApprover.Count != 0)
            {
                foreach (var item in pendingApprover)
                {
                    approved = (from certificationMaster in dbContext.tbl_PM_EmployeeCertificationMatrix
                                join certification in dbContext.tbl_PM_Certifications on certificationMaster.CertificationID equals certification.CertificationID
                                where certificationMaster.CertificationDate >= startDate && certificationMaster.CertificationDate <= endDate
                                && certificationMaster.EmployeeID == employeeId && certificationMaster.EmployeeCertificationID != item.EmployeeCertificationID
                                select new CertificationDetails
                                {
                                    CertificationName = certification.CertificationName,
                                    CertificationNo = certificationMaster.CertificationNo,
                                    Institution = certificationMaster.InstituteName,
                                    CertificationDate = certificationMaster.CertificationDate,
                                    CertificationScore = certificationMaster.TotalScore,
                                    CertificationGrade = certificationMaster.Grade
                                }).ToList();
                }
            }
            else
            {
                approved = (from certificationMaster in dbContext.tbl_PM_EmployeeCertificationMatrix
                            join certification in dbContext.tbl_PM_Certifications on certificationMaster.CertificationID equals certification.CertificationID
                            where certificationMaster.CertificationDate >= startDate && certificationMaster.CertificationDate <= endDate
                            && certificationMaster.EmployeeID == employeeId
                            select new CertificationDetails
                            {
                                CertificationName = certification.CertificationName,
                                CertificationNo = certificationMaster.CertificationNo,
                                Institution = certificationMaster.InstituteName,
                                CertificationDate = certificationMaster.CertificationDate,
                                CertificationScore = certificationMaster.TotalScore,
                                CertificationGrade = certificationMaster.Grade
                            }).ToList();
            }

            return pendingApprover.Union(approved).ToList();
        }

        public bool CheckCancleStatus(int ApprisalId)
        {
            bool status = false;
            try
            {
                tbl_Appraisal_AppraisalMaster emp = dbContext.tbl_Appraisal_AppraisalMaster.Where(ed => ed.AppraisalID == ApprisalId).FirstOrDefault();
                if (emp.IsCancelled == true)
                {
                    status = true;
                }
            }
            catch
            {
                throw;
            }
            return status;
        }

        public int GetAsAnAppraiserCount(int employeeId, out int totalCount)
        {
            List<AppraisalProcessStatus> CommonResult = new List<AppraisalProcessStatus>();

            try
            {
                CommonResult = (from E in dbContext.tbl_Appraisal_AppraisalMaster
                                join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                from ex in exp.DefaultIfEmpty()
                                join s in dbContext.tbl_Appraisal_Stages on E.AppraisalStageID + 1 equals s.AppraisalStageID into st
                                join y in dbContext.tbl_Appraisal_YearMaster on E.AppraisalYearID equals y.AppraisalYearID
                                from extstage in st.DefaultIfEmpty()
                                where (E.Appraiser1 == employeeId || E.Appraiser2 == employeeId) && (E.AppraisalStageID == 1 || ((E.AppraisalStageID == 4 && y.IDFInitiatedOn <= DateTime.Now && (y.IDFFrozenOn == null || y.IDFFrozenOn >= DateTime.Now)) && E.Appraiser1 == employeeId)) || E.IsCancelled != true

                                join ese in dbContext.tbl_Appraisal_StageEvents on E.AppraisalID equals ese.AppraisalID into eventStageRecord  // Fix to add red Image support

                                select new AppraisalProcessStatus
                                {
                                    Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatetime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                    ReportingTo = ex.ReportingTo,
                                    AppraisalId = E.AppraisalID,
                                    StageId = E.AppraisalStageID,
                                    AppraisalStageOrder = E.AppraisalStageID,
                                    stageName = extstage.AppraisalStage,
                                    EmployeeId = E.EmployeeID,
                                    Employeename = ex.EmployeeName,
                                    AppraisalYearId = E.AppraisalYearID
                                }).Distinct().OrderByDescending(exid => exid.AppraisalId).ToList();

                List<AppraisalProcessStatus> ManagerList = new List<AppraisalProcessStatus>();
                foreach (var item in CommonResult)
                {
                    tbl_Appraisal_StageEvents LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                             where empInfo.AppraisalID == item.AppraisalId
                                                             orderby empInfo.EventDatetime descending
                                                             select empInfo).FirstOrDefault();
                    tbl_Appraisal_StageEvents LatestEntryManager = new tbl_Appraisal_StageEvents();
                    if (LatestEntry != null)
                    {
                        LatestEntryManager = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                              where empInfo.AppraisalID == item.AppraisalId && (empInfo.FromStageId == 1 && empInfo.ToStageId == 1) && empInfo.EventDatetime >= LatestEntry.EventDatetime
                                              orderby empInfo.EventDatetime descending
                                              select empInfo).FirstOrDefault();
                    }
                    if (LatestEntryManager != null)
                    {
                        if (LatestEntryManager.USerId != employeeId)
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

                CommonResult.Clear();
                CommonResult = ManagerList;

                totalCount = CommonResult.Count;
                return totalCount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAsAnReviewerCount(int employeeId, out int totalCount)
        {
            List<AppraisalProcessStatus> CommonResult = new List<AppraisalProcessStatus>();

            try
            {
                CommonResult = (from E in dbContext.tbl_Appraisal_AppraisalMaster
                                join emp in dbContext.HRMS_tbl_PM_Employee on E.EmployeeID equals emp.EmployeeID into exp
                                from ex in exp.DefaultIfEmpty()
                                join s in dbContext.tbl_Appraisal_Stages on E.AppraisalStageID + 1 equals s.AppraisalStageID into st
                                from extstage in st.DefaultIfEmpty()
                                where (E.Reviewer1 == employeeId || E.Reviewer2 == employeeId) && (E.AppraisalStageID == 2) || E.IsCancelled != true

                                join ese in dbContext.tbl_Appraisal_StageEvents on E.AppraisalID equals ese.AppraisalID into eventStageRecord  // Fix to add red Image support

                                select new AppraisalProcessStatus
                                {
                                    Field = eventStageRecord.Any() ? eventStageRecord.OrderByDescending(x => x.EventDatetime).FirstOrDefault().Action : string.Empty, // Fix to add red Image support
                                    ReportingTo = ex.ReportingTo,
                                    AppraisalId = E.AppraisalID,
                                    StageId = E.AppraisalStageID,
                                    AppraisalStageOrder = E.AppraisalStageID,
                                    stageName = extstage.AppraisalStage,
                                    EmployeeId = E.EmployeeID,
                                    Employeename = ex.EmployeeName,
                                    AppraisalYearId = E.AppraisalYearID
                                }).Distinct().OrderByDescending(exid => exid.AppraisalId).ToList();

                List<AppraisalProcessStatus> ReviewerList = new List<AppraisalProcessStatus>();
                foreach (var item in CommonResult)
                {
                    tbl_Appraisal_StageEvents LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                             where empInfo.AppraisalID == item.AppraisalId
                                                             orderby empInfo.EventDatetime descending
                                                             select empInfo).FirstOrDefault();
                    tbl_Appraisal_StageEvents LatestEntryManager = new tbl_Appraisal_StageEvents();
                    if (LatestEntry != null)
                    {
                        LatestEntryManager = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                              where empInfo.AppraisalID == item.AppraisalId && (empInfo.FromStageId == 2 && empInfo.ToStageId == 2) && empInfo.EventDatetime >= LatestEntry.EventDatetime
                                              orderby empInfo.EventDatetime descending
                                              select empInfo).FirstOrDefault();
                    }
                    if (LatestEntryManager != null)
                    {
                        if (LatestEntryManager.USerId != employeeId)
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

                CommonResult.Clear();
                CommonResult = ReviewerList;

                totalCount = CommonResult.Count;
                return totalCount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Appraisal_StageEvents getStageEventlatestEntry(int ApprisalId)
        {
            try
            {
                tbl_Appraisal_StageEvents LatestEntry = new tbl_Appraisal_StageEvents();
                LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                               where empInfo.AppraisalID == ApprisalId
                               orderby empInfo.EventDatetime descending
                               select empInfo).FirstOrDefault();

                return LatestEntry;
            }
            catch
            {
                throw;
            }
        }

        public bool CheckFormSumitedStatus(string isMngrOrEmpElement, int appraisalID, int LoggedInEmployeeId)
        {
            try
            {
                bool Status = false;

                tbl_Appraisal_StageEvents LatestEntry = (from empInfo in dbContext.tbl_Appraisal_StageEvents
                                                         where empInfo.AppraisalID == appraisalID
                                                         orderby empInfo.EventDatetime descending
                                                         select empInfo).FirstOrDefault();
                if (LatestEntry != null)
                {
                    if (isMngrOrEmpElement == "Employee")
                    {
                        if (LatestEntry.USerId == LoggedInEmployeeId && LatestEntry.FromStageId == 0 && LatestEntry.ToStageId == 1 || LatestEntry.FromStageId == 5 && LatestEntry.ToStageId == 7 || LatestEntry.FromStageId == 5 && LatestEntry.ToStageId == 4)
                        {
                            Status = true;
                        }
                    }
                    else if (isMngrOrEmpElement == "Appraiser1" || isMngrOrEmpElement == "Appraiser2")
                    {
                        if (LatestEntry.USerId == LoggedInEmployeeId && (LatestEntry.FromStageId == 1 && LatestEntry.ToStageId == 1 || LatestEntry.FromStageId == 1 && LatestEntry.ToStageId == 2 || LatestEntry.FromStageId == 4 && LatestEntry.ToStageId == 5 || LatestEntry.FromStageId == 1 && LatestEntry.ToStageId == 0 || LatestEntry.FromStageId == 4 && LatestEntry.ToStageId == 3 || LatestEntry.FromStageId == 4 && LatestEntry.ToStageId == 8))
                        {
                            Status = true;
                        }
                    }
                    else if (isMngrOrEmpElement == "Reviewer2" || isMngrOrEmpElement == "Reviewer1")
                    {
                        if (LatestEntry.USerId == LoggedInEmployeeId && (LatestEntry.FromStageId == 2 && LatestEntry.ToStageId == 2 || LatestEntry.FromStageId == 2 && LatestEntry.ToStageId == 3 || LatestEntry.FromStageId == 2 && LatestEntry.ToStageId == 1))
                        {
                            Status = true;
                        }
                    }
                    else if (isMngrOrEmpElement == "GroupHead")
                    {
                        if (LatestEntry.USerId == LoggedInEmployeeId && (LatestEntry.FromStageId == 3 && LatestEntry.ToStageId == 4 || LatestEntry.FromStageId == 3 && LatestEntry.ToStageId == 2))
                        {
                            Status = true;
                        }
                    }
                }
                return Status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public bool IsReviewerStageCleared(int appraiserId)
        //{
        //    bool isReviewerStageCleared=false;
        //    tbl_Appraisal_RatingComments appRatingComments = (from data in dbContext.tbl_Appraisal_RatingComments
        //                                                      where data.AppraisalID == appraiserId
        //                                                      select data).FirstOrDefault();
        //    if (appRatingComments.Reviewer1Comments != null)
        //        isReviewerStageCleared = true;

        //    return isReviewerStageCleared;

        //}

        public void GetExcelData(System.Data.DataSet ds, string EmployeeCodes)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["Employee Code"].ToString() != "")
                {
                    string xlsemployeeode = Convert.ToInt32(ds.Tables[0].Rows[i]["Employee Code"]).ToString();

                    if (xlsemployeeode != null)
                    {
                        var code = (from c in dbContext.HRMS_tbl_PM_Employee
                                    where c.EmployeeCode == xlsemployeeode
                                    select c.EmployeeCode).FirstOrDefault();

                        if (xlsemployeeode == code)
                        {
                            string EmployeeCode = ds.Tables[0].Rows[i]["Employee Code"].ToString();

                            var EmployeeId = (from c in dbContext.HRMS_tbl_PM_Employee
                                              where c.EmployeeCode == EmployeeCode
                                              select c.EmployeeID).FirstOrDefault();

                            string xlsYears = ds.Tables[0].Rows[i]["Year"].ToString();
                            int? xlsYear = 0;
                            if (xlsYears != "")
                            {
                                xlsYear = Convert.ToInt32(ds.Tables[0].Rows[i]["Year"].ToString());
                            }
                            string xlsmonth = ds.Tables[0].Rows[i]["Month"].ToString();

                            string xlsGrade = ds.Tables[0].Rows[i]["Grade"].ToString();
                            var CurrentDesgId = (from c in dbContext.tbl_PM_GradeMaster
                                                 where c.Grade == xlsGrade
                                                 select c.GradeID).FirstOrDefault();

                            string xlsLevel = ds.Tables[0].Rows[i]["Level"].ToString();

                            string xlsDesiganationCurrent = ds.Tables[0].Rows[i]["Designation"].ToString();

                            int? GradeID = 0;

                            if (xlsDesiganationCurrent != "")
                            {
                                var CurrentGradeId = (from c in dbContext.tbl_PM_DesignationMaster
                                                      where c.DesignationName == xlsDesiganationCurrent
                                                      select c.DesignationID).FirstOrDefault();
                                GradeID = Convert.ToInt32(CurrentGradeId);
                            }

                            if (CurrentDesgId != null)
                            {
                                CurrentDesgId = Convert.ToInt32(CurrentDesgId);
                            }

                            string xlsRoleDescription = ds.Tables[0].Rows[i]["Role Description"].ToString();

                            string xlsJoingDestion = ds.Tables[0].Rows[i]["Joining Designation"].ToString();

                            int? NewDesgIds = 0;
                            if (xlsJoingDestion != "")
                            {
                                var NewDesgId = (from c in dbContext.tbl_PM_DesignationMaster
                                                 where c.DesignationName == xlsJoingDestion
                                                 select c.DesignationID).FirstOrDefault();
                                NewDesgIds = Convert.ToInt32(NewDesgId);
                            }

                            tbl_PM_EmployeeDesignation_Change record2 = new tbl_PM_EmployeeDesignation_Change();
                            record2.EmployeeID = EmployeeId;
                            record2.DesignationID = NewDesgIds;
                            record2.ChangeTo = xlsDesiganationCurrent + " - " + xlsJoingDestion;
                            record2.ModifiedDate = DateTime.Now;
                            record2.NewDesignationID = GradeID;
                            record2.ApproverID = Convert.ToInt32(EmployeeCodes);
                            record2.IsEmployeeUpdated = 1;
                            record2.Year = xlsYear;
                            record2.Month = xlsmonth;
                            record2.Level = xlsLevel;
                            record2.CurrentGradeID = CurrentDesgId;
                            record2.RoleDescription = xlsRoleDescription;
                            dbContext.tbl_PM_EmployeeDesignation_Change.AddObject(record2);
                            HRMS_tbl_PM_Employee employee = (from c in dbContext.HRMS_tbl_PM_Employee
                                                             where c.EmployeeCode == EmployeeCode
                                                             select c).FirstOrDefault();
                            employee.DesignationID = NewDesgIds;
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}