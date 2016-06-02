using HRMS.Models.SkillMatrix;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web.Security;

namespace HRMS.DAL
{
    public class SkillMatrixDal
    {
        private WSEMDBEntities dbContext = new WSEMDBEntities();
        //Rahul
        //Rahul
        //public List<DetailsModel> SearchEmployeeForEmp(int? id, int page, int rows, out int totalCount, string Todo)
        //{
        //    try
        //    {
        //        var dbContextt = new WSEMDBEntities();
        //        List<DetailsModel> employeeDetails = new List<DetailsModel>();

        //        if (Todo == "OtherSkill")
        //        {
        //           // var Details = dbContextt.SkillData_SP(id, "OtherSkill");
        //            var Details = dbContextt.SkillData_SP(id);
        //            employeeDetails = (from d in Details
        //                               select new DetailsModel
        //                               {
        //                                   Id = d.ID,
        //                                   ResourcePoolName = d.resourcepoolname,
        //                                   SkillName = d.description,
        //                                   Rating = d.ratings,
        //                                   Remark = d.Remark

        //                               }).ToList();
        //        }
        //        else if (Todo == "Particular")
        //        {
        //            //var Details = dbContextt.SkillData_SP(id, "Particular");
        //            var Details = dbContextt.SkillData_SP(id);
        //            employeeDetails = (from d in Details
        //                               select new DetailsModel
        //                               {
        //                                   Id = d.ID,
        //                                   ResourcePoolName = d.resourcepoolname,
        //                                   SkillName = d.description,
        //                                   Rating = d.ratings,
        //                                   Remark = d.Remark

        //                               }).ToList();
        //        }

        //        totalCount = employeeDetails.Count();
        //        return employeeDetails.Skip((page - 1) * rows).Take(rows).ToList();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<DetailsModel> SearchEmployeeForEmp(int? id, int page, int rows, out int totalCount, string Todo)
        {
            try
            {
                var dbContextt = new WSEMDBEntities();
                List<DetailsModel> employeeDetails = new List<DetailsModel>();

                if (Todo == "OtherSkill")
                {
                    var Details = dbContextt.SkillData_SP(id, "OtherSkill");
                    employeeDetails = (from d in Details
                                       select new DetailsModel
                                       {
                                           Id = d.ID,
                                           ResourcePoolName = d.resourcepoolname,
                                           SkillName = d.description,
                                           Rating = d.ratings,
                                           Remark = d.Remark
                                       }).ToList();
                }
                else if (Todo == "Particular")
                {
                    var Details = dbContextt.SkillData_SP(id, "Particular");
                    employeeDetails = (from d in Details
                                       select new DetailsModel
                                       {
                                           Id = d.ID,
                                           ResourcePoolName = d.resourcepoolname,
                                           SkillName = d.description,
                                           Rating = d.ratings,
                                           Remark = d.Remark
                                       }).ToList();
                }

                totalCount = employeeDetails.Count();
                return employeeDetails.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DetailsModel> getResourcePoolNamesList()
        {
            var dbContextt = new WSEMDBEntities();
            List<DetailsModel> resourcePoolName = new List<DetailsModel>();
            var resourcePools = dbContextt.GetResoucePoolType_SP();
            resourcePoolName = (from r in resourcePools
                                orderby r.ResourcePoolName
                                select new DetailsModel
                                {
                                    ResourcePoolID = r.ResourcePoolID,
                                    ResourcePoolName = r.ResourcePoolName
                                }).ToList();
            return resourcePoolName;
        }

        public List<DetailsModel> getSkillNames(int id)
        {
            var dbcontext = new WSEMDBEntities();
            List<DetailsModel> skillName = new List<DetailsModel>();
            var skill = dbcontext.getSkillname_sp(id);
            skillName = (from s in skill
                         orderby s.description
                         select new DetailsModel

                         {
                             ToolId = s.ToolId,
                             SkillName = s.description
                         }).ToList();
            return skillName;
        }

        public List<DetailsModel> getRatings()
        {
            var dbcontext = new WSEMDBEntities();
            List<DetailsModel> ratings = new List<DetailsModel>();
            var rating = dbcontext.GetEmployeeRatings_SP();
            ratings = (from r in rating
                       orderby r.ProficiencyId
                       select new DetailsModel
                       {
                           Rating = r.Description,
                           RatingId = r.ProficiencyId
                       }).ToList();
            return ratings;
        }

        public bool SaveRating(DetailsModel model, string skillID, string rating, int? loggedInUserEmployeeCode, string UpdatedBy)
        {
            var dbcontext = new WSEMDBEntities();
            ObjectParameter Output = new ObjectParameter("Output", typeof(int));
            int? SkillID = Convert.ToInt32(skillID);
            string Rating = rating;
            dbcontext.AddUpdateSearchEmployeeByName_SP(0, loggedInUserEmployeeCode, "INSERT", 0, SkillID, Rating, model.Remark, UpdatedBy, DateTime.Now, Output);
            //dbcontext.AddUpdateSearchEmployeeByName_SP(0, loggedInUserEmployeeCode, "INSERT", 0, SkillID, Rating, UpdatedBy, DateTime.Now, Output);
            bool status = Convert.ToBoolean(Output.Value);
            return status;
        }

        public bool DeleteSkill(int id)
        {
            try
            {
                WSEMDBEntities dbContext = new WSEMDBEntities();
                bool status = false;
                ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                SkillMatrixDal dal = new SkillMatrixDal();
                dbContext.DeleteSearchByEmployeeName(id, dal.GetCurrentUserLoggedOn(), Result);
                status = Convert.ToBoolean(Result.Value);

                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SubmitSkillDetails(string ProjectEmployeeRoleID)
        {
            try
            {
                WSEMDBEntities dbContext = new WSEMDBEntities();
                bool status = false;
                ObjectParameter Result = new ObjectParameter("Result", typeof(int));

                dbContext.SubmitSkillMatrixRatingDetails_SP(ProjectEmployeeRoleID, Result);

                status = Convert.ToBoolean(Result.Value);
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public HRMS_tbl_PM_Employee GetEmployeeDetailsByEmployeeCode(string EmployeeCode)
        {
            var employeeDetails = dbContext.GetEmployeeDetailsByEmployeeCode_sp(EmployeeCode);
            HRMS_tbl_PM_Employee empDetails = new HRMS_tbl_PM_Employee();
            foreach (var item in employeeDetails)
            {
                empDetails.EmployeeName = item.EmployeeName;
                empDetails.EmployeeID = item.EmployeeID;
            }
            return empDetails;
        }

        //Krishal//
        public List<skillmatrix_history> getSkillMatrix_NewHistory(int ResourcePoolId, int EmployeeId, string ToolId)
        {
            List<skillmatrix_history> HistoryRecords = new List<skillmatrix_history>();
            if (ToolId == null)
            {
                ToolId = "";
            }
            try
            {
                var historyList = dbContext.getHistoryNewVersion_sp(ResourcePoolId, EmployeeId, ToolId);
                var test = historyList.AsEnumerable().ToList();
                HistoryRecords = (from item in test
                                  select new skillmatrix_history
                                  {
                                      ROWS = item.ROWss,
                                      EmployeeId = item.EmployeeId,
                                      Rank = item.Rank,
                                      Resourcepoolname = item.Resourcepoolname,
                                      Description = item.description,
                                      Ratings = item.ratings,
                                      Remark = item.remark,
                                      UpdatedBy = item.UpdatedBy,
                                      UpdatedOn = item.UpdatedOn
                                  }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return HistoryRecords;
        }

        public List<SkillMatrixShowHistoryModel> getSkillMatrix(int ResourcePoolId, int EmployeeId, string ToolId)
        {
            List<SkillMatrixShowHistoryModel> HistoryRecords = new List<SkillMatrixShowHistoryModel>();

            try
            {
                var historyList = dbContext.getHistory_sp(ResourcePoolId, EmployeeId, ToolId);
                var test = historyList.AsEnumerable().ToList();
                HistoryRecords = (from item in test
                                  select new SkillMatrixShowHistoryModel
                                  {
                                      EmployeeId = item.EmployeeID,
                                      ResourcePoolID = item.resourcepoolid,
                                      ResourcePoolName = item.resourcepoolname,
                                      ToolId = item.ToolId,
                                      //toolId = item.ToolId1,
                                      Description = item.description,
                                      Ratings = item.ratings,
                                      UpdatedBy = item.UpdatedBy,
                                      UpdatedOn = item.UpdatedOn
                                  }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return HistoryRecords;
        }

        public List<ResourcePoolList> getResoucePoolNames()
        {
            List<ResourcePoolList> resourcePoolName = new List<ResourcePoolList>();
            var resourcePools = dbContext.GetResoucePoolType_SP();
            resourcePoolName = (from r in resourcePools
                                select new ResourcePoolList
                                {
                                    ResourcePoolId = r.ResourcePoolID,
                                    ResourcePoolName = r.ResourcePoolName
                                }).ToList();
            return resourcePoolName;
        }

        public List<GetSkillName> GetSkillname(int ResourcePoolId)
        {
            List<GetSkillName> getskilllist = new List<GetSkillName>();
            var Skillname = dbContext.getSkillname_sp(ResourcePoolId);//getHistory_sp(ResourcePoolId, 13853);
            var skilladd = (from sub in Skillname
                            select new GetSkillName
                            {
                                toolId = sub.ToolId,
                                Description = sub.description
                            }).ToList();

            return skilladd;
        }

        ////////////////ganesh,prince n nikita/////////////

        public List<ResourcePoolSkillListDetails> getResourcePoolNames()
        {
            List<ResourcePoolSkillListDetails> resourcePoolName = new List<ResourcePoolSkillListDetails>();
            var resourcePools = dbContext.GetResoucePoolType_SP();
            resourcePoolName = (from r in resourcePools
                                select new ResourcePoolSkillListDetails
                                {
                                    ResourcePoolId = r.ResourcePoolID,
                                    ResourcePoolName = r.ResourcePoolName
                                }).ToList();
            return resourcePoolName;
        }

        public List<SkillList> getSkillNamesList(int? ResourcePoolID)
        {
            List<SkillList> skillName = new List<SkillList>();
            var skillNames = dbContext.SearchSkillDropDown_SP(ResourcePoolID);
            skillName = (from s in skillNames
                         select new SkillList
                         {
                             SkillID = s.toolid,
                             SkillName = s.description
                         }).ToList();
            return skillName;
        }

        //Prince JOhnson

        public ObjectResult DGetSkillMum()
        {
            return dbContext.Get_SkillMUM_Sp();
        }

        public void InsertSkillMatrixUploadInfo(string fileName, int id, string UpdDescription, string GetCurrentUSer, string dt)
        {
            dbContext.Insert_SkillMatrix_Upload_Info_Sp(fileName, id, UpdDescription, GetCurrentUSer, dt);
        }

        public void SkillmatrixUploadInfoDelete(int documentId)
        {
            dbContext.Skillmatrix_UploadInfo_Delete_Sp(documentId);
        }

        public string GetCurrentUserLoggedOn()
        {
            EmployeeDAL empdal = new EmployeeDAL();
            HRMS_tbl_PM_Employee employeeDetails = empdal.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(Membership.GetUser().UserName));
            return employeeDetails.EmployeeName;
        }

        public void InsertSkillRates(int? eid, int projID, int? SkillID, string rate, string UpdatedBy, DateTime UpdatedOn)
        {
            dbContext.Insert_Skill_Rate_Sp(eid, projID, SkillID, rate, UpdatedBy, UpdatedOn);
        }

        public int? GetInfoSkillMatrixs(string todo, string data, int? Rid)
        {
            return dbContext.Get_Info_SkillMatrix_Sp(todo, data, Rid).FirstOrDefault();
        }

        public int? GetEmpID_SkillMatrixs(int id)
        {
            return dbContext.Get_EmpID_SkillMatrix_Sp(id).SingleOrDefault();
        }

        //////////////nikita///////

        public List<Details> Searchemployeebyname(int id, string searchText, int? Id, int? page, int? rows, out int total)
        {
            List<Details> employeeDetails = new List<Details>();
            try
            {
                var skill = dbContext.getSkillData_SP(id);
                employeeDetails = (from type in skill
                                   select new Details

                                   {
                                       ID = type.ID,
                                       ResourcePoolName = type.ResourcePoolName,
                                       Description = type.SkillName,
                                       Rating = type.Rating
                                   }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            total = employeeDetails.Count();
            return employeeDetails;
        }

        public List<Details> GetResourcePoolNameDetails()
        {
            List<Details> resourcePoolName = new List<Details>();
            var resourcePools = dbContext.GetResoucePoolType_SP();
            resourcePoolName = (from r in resourcePools
                                select new Details
                                {
                                    ResourcePoolID = r.ResourcePoolID,
                                    ResourcePoolName = r.ResourcePoolName
                                }).ToList();
            return resourcePoolName;
        }

        public List<Details> GetSkillNameDetails(int resourcePoolID)
        {
            List<Details> skillName = new List<Details>();
            var skillNames = dbContext.getSkillname_sp(resourcePoolID);
            skillName = (from r in skillNames
                         select new Details
                         {
                             SkillID = r.ToolId,
                             Description = r.description
                         }).ToList();
            return skillName;
        }

        public List<Ratings> GetRatingsDetails()
        {
            List<Ratings> rating1 = new List<Ratings>();
            var ratings = dbContext.GetEmployeeSkillRatings_SP();
            rating1 = (from r in ratings
                       select new Ratings
                       {
                           ProficiencyID = r.ProficiencyId,
                           Rating = r.Description
                       }).ToList();
            return rating1;
        }

        public bool SaveSearchEmployeeByName(Details model, int? SkillID, string Rating, int? loggedInUserEmployeeCode, string UpdatedBy)
        {
            ObjectParameter Output = new ObjectParameter("Output", typeof(int));
            //if (model.ID == null)
            //{
            //    dbContext.AddUpdateSearchEmployeeByName_SP(0, loggedInUserEmployeeCode, "INSERT", 0, SkillID, Rating, UpdatedBy, DateTime.Now, Output);
            //}
            //else
            //{
            //    dbContext.AddUpdateSearchEmployeeByName_SP(model.ID, loggedInUserEmployeeCode, "UPDATE", 0, 0, Rating, UpdatedBy, DateTime.Now, Output);
            //}
            dbContext.AddUpdateSearchEmployeeByName_SP(0, loggedInUserEmployeeCode, "INSERT", 0, SkillID, Rating, "", UpdatedBy, DateTime.Now, Output);
            bool status = Convert.ToBoolean(Output.Value);
            Output.Value = 0;
            return status;
        }

        //public bool SaveSearchEmployeeByName(Details model, int? SkillID, string Rating, int? loggedInUserEmployeeCode, string UpdatedBy)
        //{
        //    ObjectParameter Output = new ObjectParameter("Output", typeof(int));
        //    //if (model.ID == null)
        //    //{
        //    //    dbContext.AddUpdateSearchEmployeeByName_SP(0, loggedInUserEmployeeCode, "INSERT", 0, SkillID, Rating, UpdatedBy, DateTime.Now, Output);
        //    //}
        //    //else
        //    //{
        //    //    dbContext.AddUpdateSearchEmployeeByName_SP(model.ID, loggedInUserEmployeeCode, "UPDATE", 0, 0, Rating, UpdatedBy, DateTime.Now, Output);
        //    //}
        //    dbContext.AddUpdateSearchEmployeeByName_SP(0, loggedInUserEmployeeCode, "INSERT", 0, SkillID, Rating, UpdatedBy, DateTime.Now, Output);
        //    //dbContext.AddUpdateSearchEmployeeByName_SP(0, loggedInUserEmployeeCode, "INSERT", 0, SkillID, "", UpdatedBy, DateTime.Now, Output);
        //    bool status = Convert.ToBoolean(Output.Value);
        //    Output.Value = 0;
        //    return status;

        //}
        public bool DeleteSearchByEmployeeName(IList<string> id)
        {
            SkillMatrixDal dal = new SkillMatrixDal();
            try
            {
                bool status = false;
                foreach (string InDId in id)
                {
                    if (InDId != "")
                    {
                        ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                        int Id = Convert.ToInt32(InDId);
                        dbContext.DeleteSearchByEmployeeName(Id, dal.GetCurrentUserLoggedOn(), Result);
                        status = Convert.ToBoolean(Result.Value);
                    }
                }

                return status;
            }
            catch (Exception)
            {
                throw;
            }
            //return true;
        }

        //kalindi
        public List<ConfigureResourcePoolGridModel> GetResourcePoolNameDetailsConfiguration()
        {
            WSEMDBEntities dbContext = new WSEMDBEntities();
            List<ConfigureResourcePoolGridModel> resourcePoolName = new List<ConfigureResourcePoolGridModel>();
            var resourcePools = dbContext.GetResoucePoolType_SP();
            resourcePoolName = (from r in resourcePools
                                select new ConfigureResourcePoolGridModel
                                {
                                    ResourcePoolId = r.ResourcePoolID,
                                    ResourcePoolName = r.ResourcePoolName
                                }).ToList();
            return resourcePoolName;
        }

        public List<ConfigurationSkillMatrix> GetSkillName(string id)
        {
            //int? id = ResorcePoolID;
            WSEMDBEntities dbContext = new WSEMDBEntities();
            List<ConfigurationSkillMatrix> skillname = new List<ConfigurationSkillMatrix>();
            var resourcePools = dbContext.FillSkill_SP(int.Parse(id));
            skillname = (from r in resourcePools
                         select new ConfigurationSkillMatrix
                         {
                             ToolId = r.toolid,
                             Description = r.description,
                         }).ToList();
            return skillname;
        }

        public List<ConfigureResourcePoolGridModel> GetResourceSkilldetails(int page, int rows, out int totalCount)
        {
            WSEMDBEntities dbContext = new WSEMDBEntities();
            List<ConfigureResourcePoolGridModel> ResourceskillDetails = new List<ConfigureResourcePoolGridModel>();
            try
            {
                var skill = dbContext.GetSkillResourcePoolDetails_SP();
                ResourceskillDetails = (from type in skill
                                        select new ConfigureResourcePoolGridModel
                                        {
                                            ToolId = type.ToolID,

                                            ResourcePoolId = type.ResourcePoolID,
                                            ResourcePoolName = type.ResourcePoolName,
                                            Description = type.Description,
                                            CreatedDate = type.CreatedDate
                                        }).ToList();
                totalCount = ResourceskillDetails.Count();
                //  return ResourceskillDetails.Skip((page - 1) * rows).Take(rows).ToList(); //------change
                return ResourceskillDetails.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveSkillResourceDetail(ConfigurationSkillMatrix model)
        {
            WSEMDBEntities dbContext = new WSEMDBEntities();
            SkillMatrixDal sd = new SkillMatrixDal();
            int ResourcePoolID = model.ResourcePoolId;
            string description = model.Description;

            ObjectParameter ToolSkillID = new ObjectParameter("ToolSkillID", typeof(int));
            ObjectParameter Output = new ObjectParameter("Result", typeof(int));
            bool status = false;

            if (model.ToolId != null)
            {
                var Temp = dbContext.AddUpdateSkillToolDetails_SP(model.ToolId, ResourcePoolID, sd.GetCurrentUserLoggedOn(), description.ToLower().Trim(), "EDIT", Output, ToolSkillID);
                foreach (var Te in Temp)
                {
                    status = Convert.ToBoolean(Te.Column1);
                    break;
                }
            }
            else
            {
                var Temp = dbContext.AddUpdateSkillToolDetails_SP(model.ToolId, ResourcePoolID, sd.GetCurrentUserLoggedOn(), description.ToLower().Trim(), "INSERT", Output, ToolSkillID);
                foreach (var Te in Temp)
                {
                    status = Convert.ToBoolean(Te.Column1);
                    break;
                }
            }

            //status = Convert.ToBoolean(Output.Value);
            return status;
        }

        public bool DeleteSkillToolsData(List<int> ToolId)
        {
            WSEMDBEntities dbContext = new WSEMDBEntities();
            try
            {
                bool status = false;
                foreach (var InDId in ToolId)
                {
                    if (InDId != 0)
                    {
                        ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                        int Tool = Convert.ToInt32(InDId);
                        dbContext.DeleteBySkill_SP(Tool, Result);
                        status = Convert.ToBoolean(Result.Value);
                    }
                }

                return status;
            }
            catch (Exception)
            {
                throw;
            }
            //return true;
        }

        public bool DeleteCheckList(int QueationnaireID)
        {
            bool isDeleted = false;
            try
            {
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }
    }
}