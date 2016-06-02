using HRMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HRMS.DAL
{
    public class ConfigurationAppraisalDAL
    {
        private HRMSDBEntities dbContext = new HRMSDBEntities();

        public AppraisalProcessResponse DeleteParameter(List<int> collection, int? AppraisalYearID)
        {
            try
            {
                AppraisalProcessResponse response = new AppraisalProcessResponse();
                response.ParamterwithDesignation = new List<string>();
                response.isDeleted = false;
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_Appraisal_ParameterMaster parameterMaster = dbContext.tbl_Appraisal_ParameterMaster.Where(x => x.OrderNo == item && x.AppraisalYearID == AppraisalYearID).FirstOrDefault();
                    if (parameterMaster != null)
                    {
                        tbl_Appraisal_ParameterDesignationMapping _ParameterDesignationMapping = dbContext.tbl_Appraisal_ParameterDesignationMapping.Where(x => x.ParameterID == parameterMaster.ParameterID).FirstOrDefault();
                        if (_ParameterDesignationMapping == null)
                        {
                            dbContext.DeleteObject(parameterMaster);
                            dbContext.SaveChanges();
                            response.isDeleted = true;
                        }
                        else
                        {
                            response.ParamterwithDesignation.Add(parameterMaster.Parameter);
                            response.isDeleted = false;
                        }
                    }
                }
                return response;
            }
            catch
            {
                throw;
            }
        }

        public List<tbl_Appraisal_AppraisalMaster> GetAppraisalInitiationDetails(int? AppraisalYearID)
        {
            List<tbl_Appraisal_AppraisalMaster> appraisalList =
                dbContext.tbl_Appraisal_AppraisalMaster.Where(
                    x => x.AppraisalYearID == AppraisalYearID && x.AppraisalStageID >= 0).ToList();

            return appraisalList;
        }

        public List<AppraisalParameterMaster> GetParameterMaster(int? AppraisalYearID)
        {
            dbContext = new HRMSDBEntities();
            List<AppraisalParameterMaster> parameterlist = new List<AppraisalParameterMaster>();
            try
            {
                parameterlist = (from parameter in dbContext.tbl_Appraisal_ParameterMaster
                                 where parameter.AppraisalYearID == AppraisalYearID
                                 select new AppraisalParameterMaster
                                 {
                                     ParameterCategoryID = parameter.ParameterCategoryID,
                                     Parameter = parameter.Parameter,
                                     ParameterDescription = parameter.ParameterDescription,
                                     ParameterID = parameter.ParameterID,
                                     OrderNo = parameter.OrderNo,
                                     AppraisalYearID = parameter.AppraisalYearID,
                                     AppraisalParameterChecked = false
                                 }).ToList();
                return parameterlist;
            }
            catch (Exception E)
            {
                throw E;
            }
            //if (parameterlist.Count == 0)
            //{
            //    parameterlist = null;
            //    return parameterlist;
            //}
            //else
            //{
            //}
        }

        public tbl_Appraisal_ParameterMaster getParameter(int? ordernumber, int? AppraisalYearID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_Appraisal_ParameterMaster parameterMaster = dbContext.tbl_Appraisal_ParameterMaster.Where(x => x.OrderNo == ordernumber && x.AppraisalYearID == AppraisalYearID).FirstOrDefault();
                return parameterMaster;
            }
            catch
            {
                throw;
            }
        }

        public List<ParameterCategoryList> getCategoryList()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<ParameterCategoryList> parameterCategoryList = new List<ParameterCategoryList>();
                parameterCategoryList = (from e in dbContext.tbl_PA_CompetencyCategories
                                         select new ParameterCategoryList
                                  {
                                      ParameterCategoryID = e.CategoryID,
                                      ParameterCategory = e.CategoryType
                                  }).ToList();
                return parameterCategoryList;
            }
            catch
            {
                throw;
            }
        }

        public StatusForOrderNoAndParameter SaveParameter(AddAppraisalParaModel model)
        {
            try
            {
                StatusForOrderNoAndParameter status = new StatusForOrderNoAndParameter();
                dbContext = new HRMSDBEntities();

                tbl_Appraisal_ParameterMaster parameterIDmaster = dbContext.tbl_Appraisal_ParameterMaster.Where(x => x.ParameterID == model.ParameterID).FirstOrDefault();
                int parameter = dbContext.tbl_Appraisal_ParameterMaster.Where(x => x.Parameter.ToLower().Trim() == model.Parameter.ToLower().Trim() && x.AppraisalYearID == model.AppraisalYearID).Count();
                tbl_Appraisal_ParameterMaster OrderNo = dbContext.tbl_Appraisal_ParameterMaster.Where(x => x.OrderNo == model.OrderNo && x.AppraisalYearID == model.AppraisalYearID).FirstOrDefault();
                if (OrderNo == null || OrderNo.OrderNo == model.SelectedOrderNo)
                {
                    if (parameterIDmaster == null)
                    {
                        if (parameter > 0)
                        {
                            status.IsParameter = false;
                            status.IsOrderNumber = true;
                        }
                        else
                        {
                            tbl_Appraisal_ParameterMaster parameterMaster = new tbl_Appraisal_ParameterMaster();

                            parameterMaster.Parameter = model.Parameter;
                            parameterMaster.OrderNo = model.OrderNo;
                            parameterMaster.ParameterCategoryID = Convert.ToInt32(model.category);
                            if (!string.IsNullOrEmpty(model.BehavioralIndicators))
                                parameterMaster.BehavioralIndicators = model.BehavioralIndicators.Trim();
                            else
                                parameterMaster.BehavioralIndicators = model.BehavioralIndicators;
                            if (!string.IsNullOrEmpty(model.ParameterDescription))
                                parameterMaster.ParameterDescription = model.ParameterDescription.Trim();
                            else
                                parameterMaster.ParameterDescription = model.ParameterDescription;
                            parameterMaster.AppraisalYearID = model.AppraisalYearID;

                            dbContext.tbl_Appraisal_ParameterMaster.AddObject(parameterMaster);
                            dbContext.SaveChanges();
                            status.IsOrderNumber = true;
                            status.IsParameter = true;
                        }
                    }
                    else
                    {
                        if (parameterIDmaster.Parameter != model.Parameter)
                        {
                            if (parameter > 0)
                            {
                                status.IsParameter = false;
                                status.IsOrderNumber = true;
                            }
                            else
                            {
                                parameterIDmaster.Parameter = model.Parameter;
                                parameterIDmaster.OrderNo = model.OrderNo;
                                parameterIDmaster.ParameterCategoryID = Convert.ToInt32(model.category);
                                if (!string.IsNullOrEmpty(model.BehavioralIndicators))
                                    parameterIDmaster.BehavioralIndicators = model.BehavioralIndicators.Trim();
                                else
                                    parameterIDmaster.BehavioralIndicators = model.BehavioralIndicators;
                                if (!string.IsNullOrEmpty(model.ParameterDescription))
                                    parameterIDmaster.ParameterDescription = model.ParameterDescription.Trim();
                                else
                                    parameterIDmaster.ParameterDescription = model.ParameterDescription;
                                parameterIDmaster.AppraisalYearID = model.AppraisalYearID;
                                dbContext.SaveChanges();
                                status.IsOrderNumber = true;
                                status.IsParameter = true;
                            }
                        }
                        else
                        {
                            parameterIDmaster.Parameter = model.Parameter;
                            parameterIDmaster.OrderNo = model.OrderNo;
                            parameterIDmaster.ParameterCategoryID = Convert.ToInt32(model.category);
                            if (!string.IsNullOrEmpty(model.BehavioralIndicators))
                                parameterIDmaster.BehavioralIndicators = model.BehavioralIndicators.Trim();
                            else
                                parameterIDmaster.BehavioralIndicators = model.BehavioralIndicators;
                            if (!string.IsNullOrEmpty(model.ParameterDescription))
                                parameterIDmaster.ParameterDescription = model.ParameterDescription.Trim();
                            else
                                parameterIDmaster.ParameterDescription = model.ParameterDescription;
                            parameterIDmaster.AppraisalYearID = model.AppraisalYearID;
                            dbContext.SaveChanges();
                            status.IsOrderNumber = true;
                            status.IsParameter = true;
                        }
                    }
                }
                else
                {
                    status.IsOrderNumber = false;
                    status.IsParameter = true;
                }
                return status;
            }
            catch
            {
                throw;
            }
        }

        public List<AppraisalDesignation> getAppraisalDesignation(int? ParameterID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<AppraisalDesignation> parameterDesignation = new List<AppraisalDesignation>();
                parameterDesignation = (from m in dbContext.tbl_Appraisal_ParameterDesignationMapping
                                        join d in dbContext.tbl_PM_DesignationMaster on m.DesignationID equals d.DesignationID
                                        where m.ParameterID == ParameterID
                                        select new AppraisalDesignation
                                        {
                                            ParameterID = m.ParameterID,
                                            DesignationID = m.DesignationID,
                                            Designation = d.DesignationName,
                                            Checked = false
                                        }).ToList();
                return parameterDesignation;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteDesignations(List<int> collection, int parameterID)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_Appraisal_ParameterDesignationMapping parameterDesignationMapping = dbContext.tbl_Appraisal_ParameterDesignationMapping.Where(x => x.DesignationID == item && x.ParameterID == parameterID).FirstOrDefault();
                    if (parameterDesignationMapping != null)
                    {
                        dbContext.DeleteObject(parameterDesignationMapping);
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

        //rating

        public List<AppraisalRatingScales> GetAppraisalRatingsMaster(int? AppYearID)
        {
            dbContext = new HRMSDBEntities();
            List<AppraisalRatingScales> ratinglist = new List<AppraisalRatingScales>();
            try
            {
                ratinglist = (from rating in dbContext.tbl_Appraisal_RatingMaster
                              where rating.AppraisalYearID == AppYearID
                              orderby rating.Percentage descending
                              select new AppraisalRatingScales
                              {
                                  RatingID = rating.RatingID,
                                  Rating = rating.Rating,
                                  AdjustmentFactor = rating.AdjustmentFactor,
                                  AppraisalYearID = rating.AppraisalYearID,
                                  //SetAsMinimumLimit = rating.SetAsMinimumLimit,
                                  Percentage = rating.Percentage,
                                  Description = rating.Description,
                                  Checked = false
                              }).ToList();
            }
            catch (Exception E)
            {
                throw E;
            }
            //if (ratinglist.Count == 0)
            //{
            //    ratinglist = null;
            //    return ratinglist;
            //}
            //else
            //{
            return ratinglist;
            // }
        }

        public AppraisalRatingResponse SaveAppraisalRatingScales(AddAppraisalRatingScale model)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_Appraisal_RatingMaster ratingScale = dbContext.tbl_Appraisal_RatingMaster.Where(x => x.RatingID == model.RatingID).FirstOrDefault();
                tbl_Appraisal_RatingMaster _Rating_Percent = dbContext.tbl_Appraisal_RatingMaster.Where(x => x.Percentage == model.Percentage && x.AppraisalYearID == model.AppraisalYearID).FirstOrDefault();
                int _Rating = dbContext.tbl_Appraisal_RatingMaster.Where(x => x.Rating.ToLower().Trim() == model.Rating.ToLower().Trim() && x.AppraisalYearID == model.AppraisalYearID).Count();
                AppraisalRatingResponse AppRatingResponse = new AppraisalRatingResponse();
                AppRatingResponse.isRatingScalePresent = false;
                AppRatingResponse.isRatingPresent = false;
                AppRatingResponse.isRatingAdded = false;

                if (_Rating_Percent == null || _Rating_Percent.Percentage == model.SelectedPercentage)
                {
                    if (ratingScale == null) // to add new record
                    {
                        if (_Rating > 0)
                        {
                            AppRatingResponse.isRatingScalePresent = false;
                            AppRatingResponse.isRatingPresent = true;
                            AppRatingResponse.isRatingAdded = false;
                        }
                        else
                        {
                            tbl_Appraisal_RatingMaster _ratingscales = new tbl_Appraisal_RatingMaster();
                            _ratingscales.Rating = model.Rating;
                            _ratingscales.Description = model.Description;
                            _ratingscales.Percentage = model.Percentage;
                            _ratingscales.AdjustmentFactor = model.AdjustmentFactor;
                            _ratingscales.AppraisalYearID = model.AppraisalYearID;
                            _ratingscales.CreatedOn = DateTime.Now;

                            dbContext.tbl_Appraisal_RatingMaster.AddObject(_ratingscales);
                            dbContext.SaveChanges();
                            AppRatingResponse.isRatingScalePresent = false;
                            AppRatingResponse.isRatingPresent = false;
                            AppRatingResponse.isRatingAdded = true;
                        }
                    }
                    else //edit existing record
                    {
                        if (ratingScale.Rating != model.Rating)
                        {
                            if (_Rating > 0)
                            {
                                AppRatingResponse.isRatingScalePresent = false;
                                AppRatingResponse.isRatingPresent = true;
                                AppRatingResponse.isRatingAdded = false;
                            }
                            else
                            {
                                ratingScale.Rating = model.Rating;
                                ratingScale.Description = model.Description;
                                ratingScale.Percentage = model.Percentage;
                                ratingScale.AdjustmentFactor = model.AdjustmentFactor;
                                ratingScale.AppraisalYearID = model.AppraisalYearID;
                                ratingScale.ModifiedOn = DateTime.Now;
                                AppRatingResponse.isRatingScalePresent = false;
                                AppRatingResponse.isRatingPresent = false;
                                AppRatingResponse.isRatingAdded = true;
                                dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            ratingScale.Rating = model.Rating;
                            ratingScale.Description = model.Description;
                            ratingScale.Percentage = model.Percentage;
                            ratingScale.AdjustmentFactor = model.AdjustmentFactor;
                            ratingScale.AppraisalYearID = model.AppraisalYearID;
                            ratingScale.ModifiedOn = DateTime.Now;
                            AppRatingResponse.isRatingScalePresent = false;
                            AppRatingResponse.isRatingPresent = false;
                            AppRatingResponse.isRatingAdded = true;
                            dbContext.SaveChanges();
                        }
                    }
                }
                else
                {
                    AppRatingResponse.isRatingScalePresent = true;
                    AppRatingResponse.isRatingPresent = false;
                    AppRatingResponse.isRatingAdded = false;
                }
                return AppRatingResponse;
            }
            catch
            {
                throw;
            }
        }

        public List<AppraisalDesignation> getNewSelectDesignation(int[] designationID, int parameterID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<AppraisalDesignation> NewSelectDesignation = new List<AppraisalDesignation>();
                NewSelectDesignation = (from d in dbContext.tbl_PM_DesignationMaster
                                        orderby d.DesignationName ascending
                                        select new AppraisalDesignation
                                        {
                                            ParameterID = parameterID,
                                            DesignationID = d.DesignationID,
                                            Designation = d.DesignationName
                                        }).ToList();

                List<AppraisalDesignation> NewSelectDesignationfinal = new List<AppraisalDesignation>();

                foreach (var item in NewSelectDesignation)
                {
                    if (designationID.Any(designation => designation == item.DesignationID))
                        continue;
                    else
                        NewSelectDesignationfinal.Add(item);
                }

                return NewSelectDesignationfinal;
            }
            catch
            {
                throw;
            }
        }

        public bool SaveNewDesignation(List<int> designationID, int parameterID)
        {
            try
            {
                bool isSaved = false;
                dbContext = new HRMSDBEntities();
                foreach (var item in designationID)
                {
                    tbl_Appraisal_ParameterDesignationMapping parameterDesignationMapping = new tbl_Appraisal_ParameterDesignationMapping();
                    parameterDesignationMapping.ParameterID = parameterID;
                    parameterDesignationMapping.DesignationID = item;
                    dbContext.tbl_Appraisal_ParameterDesignationMapping.AddObject(parameterDesignationMapping);
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

        public List<AppraisalYear> GetAppraisalYearList()
        {
            try
            {
                List<AppraisalYear> yearList = new List<AppraisalYear>();
                tbl_Appraisal_YearMaster isCurrentYearExist = dbContext.tbl_Appraisal_YearMaster.Where(current => current.AppraisalYearStatus == 0).FirstOrDefault();
                if (isCurrentYearExist != null)
                {
                    yearList = (from year in dbContext.tbl_Appraisal_YearMaster
                                where (year.AppraisalYearStatus == 0)
                                select new AppraisalYear
                                {
                                    AppraisalYearID = year.AppraisalYearID,
                                    AppraisalYearName = year.AppraisalYear
                                }).ToList();
                }
                else
                {
                    yearList = (from year in dbContext.tbl_Appraisal_YearMaster
                                where (year.AppraisalYearStatus == null)
                                select new AppraisalYear
                                {
                                    AppraisalYearID = year.AppraisalYearID,
                                    AppraisalYearName = year.AppraisalYear
                                }).ToList();
                }
                return yearList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AppraisalYear> GetAllAppraisalYearList()
        {
            try
            {
                List<AppraisalYear> yearList = new List<AppraisalYear>();

                yearList = (from year in dbContext.tbl_Appraisal_YearMaster
                            orderby year.AppraisalYear descending
                            select new AppraisalYear
                            {
                                AppraisalYearID = year.AppraisalYearID,
                                AppraisalYearName = year.AppraisalYear
                            }).ToList();
                return yearList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AppraisalCategories> GetAllAppraisalCategoryList()
        {
            try
            {
                List<AppraisalCategories> categoryList = new List<AppraisalCategories>();

                categoryList = (from category in dbContext.Tbl_Appraisal_CategoryMaster
                                orderby category.CategoryID ascending
                                select new AppraisalCategories
                                {
                                    CategoryID = category.CategoryID,
                                    Category = category.Category,
                                    CategoryDescription = category.Description
                                }).ToList();
                return categoryList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateAppraisalYearDetails(ConfigureYearModel model)
        {
            try
            {
                bool isUpdated = false;
                int selectedYearID = Convert.ToInt32(model.Year);
                tbl_Appraisal_YearMaster _selectedYear = dbContext.tbl_Appraisal_YearMaster.Where(y => y.AppraisalYearID == selectedYearID).FirstOrDefault();
                tbl_Appraisal_YearMaster _appYearStatus = dbContext.tbl_Appraisal_YearMaster.Where(y => y.AppraisalYearStatus == 0).FirstOrDefault();
                if (_selectedYear != null)
                {
                    if (_appYearStatus != null)
                    {
                        if (_appYearStatus.AppraisalYearID != _selectedYear.AppraisalYearID)
                        {
                            _appYearStatus.AppraisalYearStatus = 1;
                            _selectedYear.AppraisalYearStatus = 0;
                        }
                        else
                        {
                            //No need to Updated Status as Both Selected and Previously set Current Year are same.
                        }
                    }
                    else
                    {
                        _selectedYear.AppraisalYearStatus = 0;
                    }
                    isUpdated = true;
                }
                dbContext.SaveChanges();

                List<tbl_PA_Competency_Master> _tbl_PA_Competency_Master = (from e in dbContext.tbl_PA_Competency_Master
                                                                            select e).ToList();
                if (_selectedYear.AppYearParametersFlag == null)
                {
                    if (_tbl_PA_Competency_Master != null)
                    {
                        foreach (tbl_PA_Competency_Master i in _tbl_PA_Competency_Master)
                        {
                            tbl_Appraisal_ParameterMaster Obj_tbl_Appraisal_ParameterMaster = new tbl_Appraisal_ParameterMaster();
                            Obj_tbl_Appraisal_ParameterMaster.Parameter = i.Competency;
                            Obj_tbl_Appraisal_ParameterMaster.ParameterDescription = i.Description;
                            Obj_tbl_Appraisal_ParameterMaster.BehavioralIndicators = i.BehavioralIndicators;
                            Obj_tbl_Appraisal_ParameterMaster.OrderNo = i.OrderNo;
                            Obj_tbl_Appraisal_ParameterMaster.ParameterCategoryID = i.CategoryID;
                            Obj_tbl_Appraisal_ParameterMaster.AppraisalYearID = selectedYearID;
                            dbContext.tbl_Appraisal_ParameterMaster.AddObject(Obj_tbl_Appraisal_ParameterMaster);
                            dbContext.SaveChanges();
                        }

                        _selectedYear.AppYearParametersFlag = 1;
                    }
                    dbContext.SaveChanges();
                }
                return isUpdated;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Appraisal_YearMaster GetCurrentYearDetails()
        {
            try
            {
                tbl_Appraisal_YearMaster currentYearDetails = dbContext.tbl_Appraisal_YearMaster.Where(year => year.AppraisalYearStatus == 0).FirstOrDefault();
                return currentYearDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AppraisalPastYears> GetPastYearDetailsList()
        {
            try
            {
                List<AppraisalPastYears> pastYearsList = (from y in dbContext.tbl_Appraisal_YearMaster
                                                          where y.AppraisalYearStatus == 1
                                                          orderby y.AppraisalYear descending
                                                          select new AppraisalPastYears
                                                          {
                                                              PastYearID = y.AppraisalYearID,
                                                              PastYearName = y.AppraisalYear
                                                          }).ToList();
                return pastYearsList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Appraisal_RatingMaster getAppraisalRatingScaleDetails(int? RatingID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_Appraisal_RatingMaster ratingscale = dbContext.tbl_Appraisal_RatingMaster.Where(r => r.RatingID == RatingID).FirstOrDefault();
                return ratingscale;
            }
            catch
            {
                throw;
            }
        }

        public AppraisalProcessResponse AddEditNewAppraisalYears(AppraisalYearModel model)
        {
            try
            {
                AppraisalProcessResponse appResponse = new AppraisalProcessResponse();
                appResponse.isAdded = false;
                appResponse.isExisted = false;
                tbl_Appraisal_YearMaster addYear = dbContext.tbl_Appraisal_YearMaster.Where(y => y.AppraisalYear == model.NewAppraisalYear).FirstOrDefault();
                tbl_Appraisal_YearMaster yearDetails = dbContext.tbl_Appraisal_YearMaster.Where(year => year.AppraisalYearID == model.AppraisalYearID).FirstOrDefault();
                if (addYear == null)
                {
                    if (model.AppraisalYearID == 0)
                    {
                        tbl_Appraisal_YearMaster addNewYear = new tbl_Appraisal_YearMaster();
                        if (model.NewAppraisalYear != null && model.NewAppraisalYear != "")
                            addNewYear.AppraisalYear = model.NewAppraisalYear.Trim();
                        else
                            addNewYear.AppraisalYear = model.NewAppraisalYear;
                        addNewYear.CreatedBy = model.SearchedUserDetails.EmployeeId;
                        addNewYear.CreatedDate = DateTime.Now;
                        dbContext.tbl_Appraisal_YearMaster.AddObject(addNewYear);
                        dbContext.SaveChanges();
                        appResponse.isAdded = true;
                    }
                    else if (yearDetails != null && model.AppraisalYearID > 0)
                    {
                        if (model.NewAppraisalYear != null && model.NewAppraisalYear != "")
                            yearDetails.AppraisalYear = model.NewAppraisalYear.Trim();
                        else
                            yearDetails.AppraisalYear = model.NewAppraisalYear;
                        yearDetails.ModifiedBy = model.SearchedUserDetails.EmployeeId;
                        yearDetails.ModifiedDate = DateTime.Now;
                        dbContext.SaveChanges();
                        appResponse.isAdded = true;
                    }
                }
                else
                {
                    appResponse.isExisted = true;
                }
                return appResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AppraisalProcessResponse AddEditNewAppraisalCategory(AppraisalCategoriesModel model)
        {
            try
            {
                AppraisalProcessResponse appResponse = new AppraisalProcessResponse();
                appResponse.isAdded = false;
                appResponse.isExisted = false;
                Tbl_Appraisal_CategoryMaster addCategory = dbContext.Tbl_Appraisal_CategoryMaster.Where(c => c.Category == model.NewAppraisalCategory).FirstOrDefault();
                Tbl_Appraisal_CategoryMaster categoryDetails = dbContext.Tbl_Appraisal_CategoryMaster.Where(category => category.CategoryID == model.CategoryID).FirstOrDefault();
                if ((addCategory == null) || (addCategory.Category == model.ExistingAppraisalCategory))
                {
                    if (model.CategoryID == 0)
                    {
                        Tbl_Appraisal_CategoryMaster addNewCategory = new Tbl_Appraisal_CategoryMaster();
                        if (model.NewAppraisalCategory != null && model.NewAppraisalCategory != "")
                            addNewCategory.Category = model.NewAppraisalCategory.Trim();
                        else
                            addNewCategory.Category = model.NewAppraisalCategory;
                        if (model.NewAppCategoryDescription != null && model.NewAppCategoryDescription != "")
                            addNewCategory.Description = model.NewAppCategoryDescription.Trim();
                        else
                            addNewCategory.Description = model.NewAppCategoryDescription;
                        addNewCategory.CreatedBy = model.SearchedUserDetails.EmployeeId;
                        addNewCategory.CreatedDate = DateTime.Now;
                        dbContext.Tbl_Appraisal_CategoryMaster.AddObject(addNewCategory);
                        dbContext.SaveChanges();
                        appResponse.isAdded = true;
                    }
                    else if (categoryDetails != null && model.CategoryID > 0)
                    {
                        if (model.NewAppraisalCategory != null && model.NewAppraisalCategory != "")
                            categoryDetails.Category = model.NewAppraisalCategory.Trim();
                        else
                            categoryDetails.Category = model.NewAppraisalCategory;
                        if (model.NewAppCategoryDescription != null && model.NewAppCategoryDescription != "")
                            categoryDetails.Description = model.NewAppCategoryDescription.Trim();
                        else
                            categoryDetails.Description = model.NewAppCategoryDescription;
                        categoryDetails.ModifiedBy = model.SearchedUserDetails.EmployeeId;
                        categoryDetails.ModifiedDate = DateTime.Now;
                        dbContext.SaveChanges();
                        appResponse.isAdded = true;
                    }
                }
                else
                {
                    appResponse.isExisted = true;
                }
                return appResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AppraisalProcessResponse DeleteAppraisalYear(int appraisalYearId)
        {
            try
            {
                AppraisalProcessResponse response = new AppraisalProcessResponse();
                response.isDeleted = false;
                response.isExisted = false;

                tbl_Appraisal_YearMaster _yearDetails = dbContext.tbl_Appraisal_YearMaster.Where(year => year.AppraisalYearID == appraisalYearId).FirstOrDefault();
                if (_yearDetails != null)
                {
                    if (_yearDetails.AppraisalYearStatus == null)
                    {
                        dbContext.DeleteObject(_yearDetails);
                        dbContext.SaveChanges();
                        response.isDeleted = true;
                    }
                    else
                        response.isExisted = true;
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AppraisalProcessResponse DeleteAppraisalCategory(int appraisalCategoryId)
        {
            try
            {
                AppraisalProcessResponse response = new AppraisalProcessResponse();
                response.isDeleted = false;

                Tbl_Appraisal_CategoryMaster _categoryDetails = dbContext.Tbl_Appraisal_CategoryMaster.Where(category => category.CategoryID == appraisalCategoryId).FirstOrDefault();
                if (_categoryDetails != null)
                {
                    dbContext.DeleteObject(_categoryDetails);
                    dbContext.SaveChanges();
                    response.isDeleted = true;
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AllEligibileEmployee> GetAllEligibileEmployees(int AppraisalYearID)
        {
            try
            {
                List<AllEligibileEmployee> allEligibileEmployees = new List<AllEligibileEmployee>();
                allEligibileEmployees = (from emp in dbContext.HRMS_tbl_PM_Employee
                                         join gm in dbContext.tbl_PM_GroupMaster on emp.GroupID equals gm.GroupID into gm_emp
                                         from gmList in gm_emp.DefaultIfEmpty()
                                         join dm in dbContext.tbl_PM_DesignationMaster on emp.DesignationID equals dm.DesignationID into dm_emp
                                         from dmList in dm_emp.DefaultIfEmpty()
                                         where ((emp.Status == false) && (emp.EmployeeStatusID == 1 || emp.EmployeeStatusID == 19 || emp.EmployeeStatusID == 10))
                                         orderby emp.EmployeeCode ascending
                                         select new AllEligibileEmployee
                                         {
                                             AppraisalYearID = AppraisalYearID,
                                             EmployeeID = emp.EmployeeID,
                                             EmployeeCode = emp.EmployeeCode,
                                             EmployeeName = emp.EmployeeName,
                                             ConfirmationDate = emp.ConfirmationDate,
                                             ProbationReviewDate = emp.Probation_Review_Date,
                                             DeliveryTeam = gmList.GroupName,
                                             Designation = dmList.DesignationName,
                                             DesignationID = emp.DesignationID,
                                             Checked = false
                                         }).ToList();

                List<tbl_Appraisal_AppraisalMaster> appraisalList = dbContext.tbl_Appraisal_AppraisalMaster.Where(app => app.AppraisalStageID >= 0 && app.AppraisalYearID == AppraisalYearID).ToList();
                List<AllEligibileEmployee> failedEligibileEmployees = new List<AllEligibileEmployee>();
                List<AllEligibileEmployee> passedEligibileEmployees = new List<AllEligibileEmployee>();
                List<AllEligibileEmployee> finalEligibileEmployees = new List<AllEligibileEmployee>();
                foreach (var allEmployees in allEligibileEmployees)
                {
                    if (appraisalList.Any(InitiatedEmp => InitiatedEmp.EmployeeID == allEmployees.EmployeeID))
                        continue;
                    else
                        finalEligibileEmployees.Add(allEmployees);
                }
                foreach (var employee in finalEligibileEmployees)
                {
                    tbl_Appraisal_AppraisalMaster _AppraisalMaster = (from master in dbContext.tbl_Appraisal_AppraisalMaster
                                                                      where ((master.EmployeeID == employee.EmployeeID) && (master.AppraisalYearID == AppraisalYearID) && (master.Appraiser1 != null) && (master.Reviewer1 != null) && (master.GroupHead != null) && (master.AppraisalStageID == null))
                                                                      select master).FirstOrDefault();
                    if (_AppraisalMaster == null)
                    {
                        failedEligibileEmployees.Add(employee);
                    }
                    else
                    {
                        passedEligibileEmployees.Add(employee);
                    }
                }
                finalEligibileEmployees = passedEligibileEmployees.Union(failedEligibileEmployees).ToList();
                if (finalEligibileEmployees.Count > 0)
                    return finalEligibileEmployees;
                else
                    return allEligibileEmployees;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AllEligibileEmployee> GetAllEligibileConfirmDateEmployees(int AppraisalYearID, DateTime ConfirmationDate)
        {
            try
            {
                List<AllEligibileEmployee> allEligibileEmployees = new List<AllEligibileEmployee>();
                allEligibileEmployees = (from emp in dbContext.HRMS_tbl_PM_Employee
                                         join gm in dbContext.tbl_PM_GroupMaster on emp.GroupID equals gm.GroupID into gm_emp
                                         from gmList in gm_emp.DefaultIfEmpty()
                                         join dm in dbContext.tbl_PM_DesignationMaster on emp.DesignationID equals dm.DesignationID into dm_emp
                                         from dmList in dm_emp.DefaultIfEmpty()
                                         where (((emp.Status == false) && (emp.EmployeeStatusID == 1 || emp.EmployeeStatusID == 19 || emp.EmployeeStatusID == 10)) && emp.Probation_Review_Date <= ConfirmationDate)
                                         orderby emp.EmployeeCode ascending
                                         select new AllEligibileEmployee
                                         {
                                             AppraisalYearID = AppraisalYearID,
                                             EmployeeID = emp.EmployeeID,
                                             EmployeeCode = emp.EmployeeCode,
                                             EmployeeName = emp.EmployeeName,
                                             ConfirmationDate = emp.ConfirmationDate,
                                             ProbationReviewDate = emp.Probation_Review_Date,
                                             DeliveryTeam = gmList.GroupName,
                                             Designation = dmList.DesignationName,
                                             Checked = false
                                         }).ToList();

                List<tbl_Appraisal_AppraisalMaster> appraisalList = dbContext.tbl_Appraisal_AppraisalMaster.Where(app => app.AppraisalStageID >= 0 && app.AppraisalYearID == AppraisalYearID).ToList();
                List<AllEligibileEmployee> failedEligibileEmployees = new List<AllEligibileEmployee>();
                List<AllEligibileEmployee> passedEligibileEmployees = new List<AllEligibileEmployee>();
                List<AllEligibileEmployee> finalEligibileEmployees = new List<AllEligibileEmployee>();
                foreach (var allEmployees in allEligibileEmployees)
                {
                    if (appraisalList.Any(InitiatedEmp => InitiatedEmp.EmployeeID == allEmployees.EmployeeID))
                        continue;
                    else
                        finalEligibileEmployees.Add(allEmployees);
                }
                foreach (var employee in finalEligibileEmployees)
                {
                    tbl_Appraisal_AppraisalMaster _AppraisalMaster = (from master in dbContext.tbl_Appraisal_AppraisalMaster
                                                                      where ((master.EmployeeID == employee.EmployeeID) && (master.AppraisalYearID == AppraisalYearID) && (master.Appraiser1 != null) && (master.Reviewer1 != null) && (master.GroupHead != null) && (master.AppraisalStageID == null))
                                                                      select master).FirstOrDefault();
                    if (_AppraisalMaster == null)
                    {
                        failedEligibileEmployees.Add(employee);
                    }
                    else
                    {
                        passedEligibileEmployees.Add(employee);
                    }
                }
                finalEligibileEmployees = passedEligibileEmployees.Union(failedEligibileEmployees).ToList();
                if (finalEligibileEmployees.Count > 0)
                    return finalEligibileEmployees;
                else
                    return allEligibileEmployees;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AllEligibileEmployee> GetAllSuccessEmployees(int[] successEmployeeID, int AppraisalYearID)
        {
            try
            {
                AllEligibileEmployee allEligibileEmployees = new AllEligibileEmployee();
                List<AllEligibileEmployee> finalEligibileEmployees = new List<AllEligibileEmployee>();
                foreach (var item in successEmployeeID)
                {
                    allEligibileEmployees = (from emp in dbContext.HRMS_tbl_PM_Employee
                                             join gm in dbContext.tbl_PM_GroupMaster on emp.GroupID equals gm.GroupID into gm_emp
                                             from gmList in gm_emp.DefaultIfEmpty()
                                             join dm in dbContext.tbl_PM_DesignationMaster on emp.DesignationID equals dm.DesignationID into dm_emp
                                             from dmList in dm_emp.DefaultIfEmpty()
                                             where emp.EmployeeID == item
                                             orderby emp.EmployeeCode ascending
                                             select new AllEligibileEmployee
                                             {
                                                 AppraisalYearID = AppraisalYearID,
                                                 EmployeeID = emp.EmployeeID,
                                                 EmployeeCode = emp.EmployeeCode,
                                                 EmployeeName = emp.EmployeeName,
                                                 ProbationReviewDate = emp.Probation_Review_Date,
                                                 ConfirmationDate = emp.ConfirmationDate,
                                                 DeliveryTeam = gmList.GroupName,
                                                 Designation = dmList.DesignationName,
                                                 //Checked = false
                                             }).FirstOrDefault();

                    finalEligibileEmployees.Add(allEligibileEmployees);
                }

                return finalEligibileEmployees;
            }
            catch (Exception)
            {
                throw;
            }
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

        public AppraisalProcessResponse InitiateAllEmpAppPro(int[] EmployeeIDs, int AppraisalYearID)
        {
            try
            {
                AppraisalProcessResponse response = new AppraisalProcessResponse();
                response.isAdded = false;
                response.failedEmployeeID = new List<int>();
                response.successEmployeeID = new List<int>();

                foreach (var item in EmployeeIDs)
                {
                    tbl_Appraisal_AppraisalMaster _AppraisalMaster = (from master in dbContext.tbl_Appraisal_AppraisalMaster
                                                                      where ((master.EmployeeID == item) && (master.AppraisalYearID == AppraisalYearID) && (master.Appraiser1 != null) && (master.Reviewer1 != null) && (master.GroupHead != null) && (master.AppraisalStageID == null))
                                                                      select master).FirstOrDefault();
                    if (_AppraisalMaster != null)
                    {
                        _AppraisalMaster.AppraisalStageID = 0;
                        dbContext.SaveChanges();
                        response.successEmployeeID.Add(item);
                        response.isAdded = true;
                    }
                    else
                    {
                        response.failedEmployeeID.Add(item);
                        response.isAdded = false;
                    }
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AppraisalProcessResponse GetIneligibileEmployeesList(List<int> EmployeeIDs, int AppraisalYearID)
        {
            try
            {
                AppraisalProcessResponse response = new AppraisalProcessResponse();
                response.failedEmployeeID = new List<int>();

                foreach (var item in EmployeeIDs)
                {
                    tbl_Appraisal_AppraisalMaster _AppraisalMaster = (from master in dbContext.tbl_Appraisal_AppraisalMaster
                                                                      where ((master.EmployeeID == item) && (master.AppraisalYearID == AppraisalYearID) && (master.Appraiser1 != null) && (master.Reviewer1 != null) && (master.GroupHead != null) && (master.AppraisalStageID == null))
                                                                      select master).FirstOrDefault();
                    if (_AppraisalMaster == null)
                    {
                        response.failedEmployeeID.Add(item);
                    }
                    else
                    {
                        //no need to return any successful employeeID list as only want failed employeeID.
                    }
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteAppraisalRatingScales(List<int> collection)
        {
            bool isDeleted = false;
            try
            {
                dbContext = new HRMSDBEntities();
                foreach (var item in collection)
                {
                    tbl_Appraisal_RatingMaster ratingScale = dbContext.tbl_Appraisal_RatingMaster.Where(x => x.RatingID == item).FirstOrDefault();
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

        public tbl_Appraisal_SrengthImprovement_Limit getSrengthImprovementLimit(int? AppraisalYearID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_Appraisal_SrengthImprovement_Limit SrengthImprovement_Limit = dbContext.tbl_Appraisal_SrengthImprovement_Limit.Where(x => x.AppraisalYearID == AppraisalYearID).FirstOrDefault();
                return SrengthImprovement_Limit;
            }
            catch
            {
                throw;
            }
        }

        public bool setStrengthImprovementLimit(AppraisalStrengthImproveModel addStrengthImproveLimit)
        {
            bool isAdded = false;
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_Appraisal_SrengthImprovement_Limit _SrengthImprovement_Limit = dbContext.tbl_Appraisal_SrengthImprovement_Limit.Where(x => x.AppraisalYearID == addStrengthImproveLimit.AppraisalYearID).FirstOrDefault();
                if (_SrengthImprovement_Limit == null)
                {
                    tbl_Appraisal_SrengthImprovement_Limit _SrengthImprovement = new tbl_Appraisal_SrengthImprovement_Limit()
                    {
                        AppraisalYearID = addStrengthImproveLimit.AppraisalYearID,
                        StrengthLimit = addStrengthImproveLimit.StrengthLimit,
                        ImprovementLimit = addStrengthImproveLimit.ImprovementLimit
                    };
                    dbContext.tbl_Appraisal_SrengthImprovement_Limit.AddObject(_SrengthImprovement);
                    isAdded = true;
                }
                else
                {
                    _SrengthImprovement_Limit.AppraisalYearID = addStrengthImproveLimit.AppraisalYearID;
                    _SrengthImprovement_Limit.StrengthLimit = addStrengthImproveLimit.StrengthLimit;
                    _SrengthImprovement_Limit.ImprovementLimit = addStrengthImproveLimit.ImprovementLimit;
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

        public decimal? getMaxStrengthLimit(int? AppraisalYear)
        {
            decimal? maxlimit = (from x in dbContext.tbl_Appraisal_RatingMaster
                                 where x.AppraisalYearID == AppraisalYear
                                 orderby x.Percentage descending
                                 select x.Percentage).FirstOrDefault();

            return maxlimit;
        }

        public decimal? getMinStrengthLimit(int? AppraisalYear)
        {
            decimal? minlimit = (from x in dbContext.tbl_Appraisal_RatingMaster
                                 where x.AppraisalYearID == AppraisalYear
                                 orderby x.Percentage
                                 select x.Percentage).FirstOrDefault();

            return minlimit;
        }

        public List<tbl_ApprisalDocuments> GetAppraisalDocuments(int appraisalYearID)
        {
            try
            {
                List<tbl_ApprisalDocuments> hRDocument = (from hrdoc in dbContext.tbl_ApprisalDocuments
                                                          where hrdoc.AppraisalYearID == appraisalYearID
                                                          orderby hrdoc.DocumentId descending
                                                          select hrdoc).ToList<tbl_ApprisalDocuments>();
                return hRDocument;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_ApprisalDocuments GetAppraisalDocument(int documentID, int appraisalYearID)
        {
            try
            {
                tbl_ApprisalDocuments hRDocument = dbContext.tbl_ApprisalDocuments.Where(ed => ed.DocumentId == documentID && ed.AppraisalYearID == appraisalYearID).FirstOrDefault();
                return hRDocument;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsAppraisalDocumentExists(string documentName, int uploadTypeID, int appraisalYearID)
        {
            var document = dbContext.tbl_ApprisalDocuments.Where(doc => doc.FileName == documentName && doc.UploadTypeId == 1 && doc.AppraisalYearID == appraisalYearID).FirstOrDefault();
            if (document == null)
                return false;

            return true;
        }

        public string GetNewNameForApprisalDocument(string documentName, int uploadTypeID, out int documentId)  //ab_c_d.xls
        {
            documentId = 0;
            var document = dbContext.tbl_ApprisalDocuments.Where(doc => doc.FileName == documentName && doc.UploadTypeId == 1).FirstOrDefault();

            if (document == null)
                return string.Empty;

            documentId = document.DocumentId;
            var latestDocument = dbContext.tbl_ApprisalDocumentDetail.Where(dd => dd.DocumentId == document.DocumentId).OrderByDescending(dd => dd.UploadedDate).FirstOrDefault();
            if (latestDocument == null) // If no subversion found in child table
                return Path.GetFileNameWithoutExtension(documentName) + "_1" + Path.GetExtension(documentName);

            string latestDocumentFileName = System.IO.Path.GetFileNameWithoutExtension(latestDocument.FileName);    //ab_c_d_23.xls
            int maxNumberAlloted = Convert.ToInt32(latestDocumentFileName.Substring(latestDocumentFileName.LastIndexOf('_') + 1));  //23

            //Path.GetFileNameWithoutExtension(documentName) = ab_c_d     //++maxNumberAlloted = 24     //Path.GetExtension(documentName) = .xls
            return Path.GetFileNameWithoutExtension(documentName) + "_" + (++maxNumberAlloted) + Path.GetExtension(documentName);
        }

        public bool UploadAppraisalDocument(IAppraisalDocuments hRDocument)
        {
            try
            {
                if (hRDocument.GetType() == typeof(tbl_ApprisalDocuments))
                {
                    dbContext.tbl_ApprisalDocuments.AddObject(hRDocument as tbl_ApprisalDocuments);
                    dbContext.SaveChanges();
                }
                else if (hRDocument.GetType() == typeof(tbl_ApprisalDocumentDetail))
                {
                    dbContext.tbl_ApprisalDocumentDetail.AddObject(hRDocument as tbl_ApprisalDocumentDetail);
                    dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AppraiserReviewerMappingModel> GetAppraisalDocumentForDispay(int page, int rows, int appraisalYearID)
        {
            List<AppraiserReviewerMappingModel> model = new List<AppraiserReviewerMappingModel>();
            AppraiserReviewerMappingModel objhrDocDetails = new AppraiserReviewerMappingModel();
            try
            {
                List<tbl_ApprisalDocuments> hRDocument = GetAppraisalDocuments(appraisalYearID);
                foreach (tbl_ApprisalDocuments eachhrDoc in hRDocument)
                {
                    var uploadTypeText = (from document in dbContext.tbl_ApprisalDocuments
                                          join uploadType in dbContext.Tbl_HR_UploadType
                                          on document.UploadTypeId equals uploadType.UploadTypeId
                                          where document.DocumentId == eachhrDoc.DocumentId && document.AppraisalYearID == appraisalYearID
                                          select uploadType.UploadType).FirstOrDefault();

                    var fileDescription = (from documentId in dbContext.tbl_ApprisalDocuments
                                           where documentId.DocumentId == eachhrDoc.DocumentId && documentId.AppraisalYearID == appraisalYearID
                                           select documentId.FileDescription).FirstOrDefault();

                    var id = (from documentId in dbContext.tbl_ApprisalDocumentDetail
                              where documentId.DocumentId == eachhrDoc.DocumentId && documentId.AppraisalYearID == appraisalYearID
                              select documentId.DocumentId).FirstOrDefault();

                    if (id != 0)
                    {
                        var hRDocumentDetails = (from hrDocDetails in dbContext.tbl_ApprisalDocumentDetail
                                                 where hrDocDetails.DocumentId == eachhrDoc.DocumentId && hrDocDetails.AppraisalYearID == appraisalYearID
                                                 orderby hrDocDetails.DocumentDetailId descending
                                                 select hrDocDetails
                                                 ).First();

                        if (fileDescription != null)
                        {
                            objhrDocDetails = new AppraiserReviewerMappingModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                //Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = hRDocumentDetails.FileDescription,
                                FileName = hRDocumentDetails.FileName,
                                AppraisalYearID = hRDocumentDetails.AppraisalYearID
                            };
                        }
                        else
                        {
                            objhrDocDetails = new AppraiserReviewerMappingModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                //Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = hRDocumentDetails.FileName,
                                AppraisalYearID = hRDocumentDetails.AppraisalYearID
                            };
                        }
                        model.Add(objhrDocDetails);
                    }
                    else
                    {
                        var hRDocumentDetails = (from hrDocDetails in dbContext.tbl_ApprisalDocuments
                                                 where hrDocDetails.DocumentId == eachhrDoc.DocumentId && hrDocDetails.AppraisalYearID == appraisalYearID
                                                 orderby hrDocDetails.DocumentId descending
                                                 select hrDocDetails
                                                    ).First();

                        if (fileDescription != null)
                        {
                            objhrDocDetails = new AppraiserReviewerMappingModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                //Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = hRDocumentDetails.FileDescription,
                                FileName = hRDocumentDetails.FileName,
                                AppraisalYearID = hRDocumentDetails.AppraisalYearID
                            };
                        }
                        else
                        {
                            objhrDocDetails = new AppraiserReviewerMappingModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                //Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = hRDocumentDetails.FileName,
                                AppraisalYearID = hRDocumentDetails.AppraisalYearID
                            };
                        }
                        model.Add(objhrDocDetails);
                    }
                }
                return (model).Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAppraisalDocumentForDispayTotalCount(int appraisalYearID)
        {
            int totalCount = 0;
            List<AppraiserReviewerMappingModel> model = new List<AppraiserReviewerMappingModel>();
            AppraiserReviewerMappingModel objhrDocDetails = new AppraiserReviewerMappingModel();
            try
            {
                List<tbl_ApprisalDocuments> hRDocument = GetAppraisalDocuments(appraisalYearID);
                foreach (tbl_ApprisalDocuments eachhrDoc in hRDocument)
                {
                    var uploadTypeText = (from document in dbContext.tbl_ApprisalDocuments
                                          join uploadType in dbContext.Tbl_HR_UploadType
                                          on document.UploadTypeId equals uploadType.UploadTypeId
                                          where document.DocumentId == eachhrDoc.DocumentId && document.AppraisalYearID == appraisalYearID
                                          select uploadType.UploadType).FirstOrDefault();

                    var fileDescription = (from documentId in dbContext.tbl_ApprisalDocuments
                                           where documentId.DocumentId == eachhrDoc.DocumentId && documentId.AppraisalYearID == appraisalYearID
                                           select documentId.FileDescription).FirstOrDefault();

                    var id = (from documentId in dbContext.tbl_ApprisalDocumentDetail
                              where documentId.DocumentId == eachhrDoc.DocumentId && documentId.AppraisalYearID == appraisalYearID
                              select documentId.DocumentId).FirstOrDefault();

                    if (id != 0)
                    {
                        var hRDocumentDetails = (from hrDocDetails in dbContext.tbl_ApprisalDocumentDetail
                                                 where hrDocDetails.DocumentId == eachhrDoc.DocumentId && hrDocDetails.AppraisalYearID == appraisalYearID
                                                 orderby hrDocDetails.DocumentDetailId descending
                                                 select hrDocDetails
                                                 ).First();

                        if (fileDescription != null)
                        {
                            objhrDocDetails = new AppraiserReviewerMappingModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                //Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = hRDocumentDetails.FileDescription,
                                FileName = hRDocumentDetails.FileName,
                                AppraisalYearID = hRDocumentDetails.AppraisalYearID
                            };
                        }
                        else
                        {
                            objhrDocDetails = new AppraiserReviewerMappingModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                //Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = hRDocumentDetails.FileName,
                                AppraisalYearID = hRDocumentDetails.AppraisalYearID
                            };
                        }
                        model.Add(objhrDocDetails);
                    }
                    else
                    {
                        var hRDocumentDetails = (from hrDocDetails in dbContext.tbl_ApprisalDocuments
                                                 where hrDocDetails.DocumentId == eachhrDoc.DocumentId && hrDocDetails.AppraisalYearID == appraisalYearID
                                                 orderby hrDocDetails.DocumentId descending
                                                 select hrDocDetails
                                                    ).First();

                        if (fileDescription != null)
                        {
                            objhrDocDetails = new AppraiserReviewerMappingModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                //Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = hRDocumentDetails.FileDescription,
                                FileName = hRDocumentDetails.FileName,
                                AppraisalYearID = hRDocumentDetails.AppraisalYearID
                            };
                        }
                        else
                        {
                            objhrDocDetails = new AppraiserReviewerMappingModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                //Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = hRDocumentDetails.FileName,
                                AppraisalYearID = hRDocumentDetails.AppraisalYearID
                            };
                        }
                        model.Add(objhrDocDetails);
                    }
                }
                totalCount = model.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return totalCount;
        }

        public void GetExcelData(System.Data.DataSet ds, AppraiserReviewerMappingModel model)
        {
            HRMS_tbl_PM_Employee employeeTable = new HRMS_tbl_PM_Employee();
            int appraisalYearID = model.AppraisalYearID;

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
                            var EmpID = (from c in dbContext.HRMS_tbl_PM_Employee
                                         where c.EmployeeCode == xlsemployeeode
                                         select c.EmployeeID).FirstOrDefault();

                            var Appraiser1_Value = (from c in dbContext.tbl_Appraisal_AppraisalMaster
                                                    where c.EmployeeID == EmpID && c.AppraisalYearID == appraisalYearID
                                                    select c.Appraiser1).FirstOrDefault();

                            string xlsAppraiser1_Value = ds.Tables[0].Rows[i]["Appraiser1"].ToString();

                            var Appraiser1_ValueID = (from c in dbContext.HRMS_tbl_PM_Employee
                                                      where c.EmployeeCode == xlsAppraiser1_Value
                                                      select c.EmployeeID).FirstOrDefault();

                            if (Convert.ToInt32(Appraiser1_ValueID) != Appraiser1_Value)
                            {
                                Appraiser1_Value = Convert.ToInt32(Appraiser1_ValueID);
                            }

                            var Appraiser2_Value = (from c in dbContext.tbl_Appraisal_AppraisalMaster
                                                    where c.EmployeeID == EmpID && c.AppraisalYearID == appraisalYearID
                                                    select c.Appraiser2).FirstOrDefault();

                            string xlsAppraiser2_Value = ds.Tables[0].Rows[i]["Appraiser2"].ToString();

                            var Appraiser2_ValueID = (from c in dbContext.HRMS_tbl_PM_Employee
                                                      where c.EmployeeCode == xlsAppraiser2_Value
                                                      select c.EmployeeID).FirstOrDefault();

                            if (Convert.ToInt32(Appraiser2_ValueID) != Appraiser2_Value)
                            {
                                Appraiser2_Value = Convert.ToInt32(Appraiser2_ValueID);
                            }

                            var Reviewer1_Value = (from c in dbContext.tbl_Appraisal_AppraisalMaster
                                                   where c.EmployeeID == EmpID && c.AppraisalYearID == appraisalYearID
                                                   select c.Reviewer1).FirstOrDefault();

                            string xlsReviewer1_Value = ds.Tables[0].Rows[i]["Reviewer1"].ToString();

                            var Reviewer1_ValueID = (from c in dbContext.HRMS_tbl_PM_Employee
                                                     where c.EmployeeCode == xlsReviewer1_Value
                                                     select c.EmployeeID).FirstOrDefault();

                            if (Convert.ToInt32(Reviewer1_ValueID) != Reviewer1_Value)
                            {
                                Reviewer1_Value = Convert.ToInt32(Reviewer1_ValueID);
                            }

                            var Reviewer2_Value = (from c in dbContext.tbl_Appraisal_AppraisalMaster
                                                   where c.EmployeeID == EmpID && c.AppraisalYearID == appraisalYearID
                                                   select c.Reviewer2).FirstOrDefault();

                            string xlsReviewer2_Value = ds.Tables[0].Rows[i]["Reviewer2"].ToString();

                            var Reviewer2_ValueID = (from c in dbContext.HRMS_tbl_PM_Employee
                                                     where c.EmployeeCode == xlsReviewer2_Value
                                                     select c.EmployeeID).FirstOrDefault();

                            if (Convert.ToInt32(Reviewer2_ValueID) != Reviewer2_Value)
                            {
                                Reviewer2_Value = Convert.ToInt32(Reviewer2_ValueID);
                            }

                            var GroupHead_Value = (from c in dbContext.tbl_Appraisal_AppraisalMaster
                                                   where c.EmployeeID == EmpID && c.AppraisalYearID == appraisalYearID
                                                   select c.GroupHead).FirstOrDefault();

                            string xlsGroupHead_Value = ds.Tables[0].Rows[i]["GroupHead"].ToString();

                            var GroupHead_ValueID = (from c in dbContext.HRMS_tbl_PM_Employee
                                                     where c.EmployeeCode == xlsGroupHead_Value
                                                     select c.EmployeeID).FirstOrDefault();

                            if (Convert.ToInt32(GroupHead_ValueID) != GroupHead_Value)
                            {
                                GroupHead_Value = Convert.ToInt32(GroupHead_ValueID);
                            }

                            var EntireRow1 = (from c in dbContext.tbl_Appraisal_AppraisalMaster
                                              where c.EmployeeID == EmpID && c.AppraisalYearID == appraisalYearID
                                              select c);

                            if (EntireRow1.Count() == 0)
                            {
                                tbl_Appraisal_AppraisalMaster newRow = new tbl_Appraisal_AppraisalMaster();
                                newRow.EmployeeID = EmpID;
                                newRow.AppraisalYearID = appraisalYearID;
                                if (Appraiser1_Value != 0)
                                {
                                    newRow.Appraiser1 = Appraiser1_Value;
                                }
                                if (Appraiser2_Value != 0)
                                {
                                    newRow.Appraiser2 = Appraiser2_Value;
                                }
                                if (Reviewer1_Value != 0)
                                {
                                    newRow.Reviewer1 = Reviewer1_Value;
                                }
                                if (Reviewer2_Value != 0)
                                {
                                    newRow.Reviewer2 = Reviewer2_Value;
                                }
                                if (GroupHead_Value != 0)
                                {
                                    newRow.GroupHead = GroupHead_Value;
                                }
                                dbContext.tbl_Appraisal_AppraisalMaster.AddObject(newRow);
                            }
                            else
                            {
                                foreach (var record1 in EntireRow1)
                                {
                                    if (Appraiser1_Value != 0)
                                    {
                                        record1.Appraiser1 = Appraiser1_Value;
                                    }
                                    if (Appraiser2_Value != 0)
                                    {
                                        record1.Appraiser2 = Appraiser2_Value;
                                    }
                                    if (Reviewer1_Value != 0)
                                    {
                                        record1.Reviewer1 = Reviewer1_Value;
                                    }
                                    if (Reviewer2_Value != 0)
                                    {
                                        record1.Reviewer2 = Reviewer2_Value;
                                    }
                                    if (GroupHead_Value != 0)
                                    {
                                        record1.GroupHead = GroupHead_Value;
                                    }
                                }
                            }

                            dbContext.SaveChanges();
                        }
                    }
                }
            }
        }

        public List<HRMS_tbl_PM_Employee> GetEmployeeDetailsList(int[] EmployeeIDs)
        {
            try
            {
                HRMS_tbl_PM_Employee _EmployeeDetails = new HRMS_tbl_PM_Employee();
                List<HRMS_tbl_PM_Employee> _EmployeeDetailsList = new List<HRMS_tbl_PM_Employee>();

                foreach (var item in EmployeeIDs)
                {
                    _EmployeeDetails = dbContext.HRMS_tbl_PM_Employee.Where(emp => emp.EmployeeID == item).FirstOrDefault();
                    _EmployeeDetailsList.Add(_EmployeeDetails);
                }
                return _EmployeeDetailsList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_Appraisal_YearMaster getAppraisalYearDetails(int appraisalYearId)
        {
            try
            {
                tbl_Appraisal_YearMaster appraisalYearDetails = dbContext.tbl_Appraisal_YearMaster.Where(year => year.AppraisalYearID == appraisalYearId).FirstOrDefault();
                return appraisalYearDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tbl_Appraisal_CategoryMaster getAppraisalCategoryDetails(int appraisalCategoryId)
        {
            try
            {
                Tbl_Appraisal_CategoryMaster appraisalCategoryDetails = dbContext.Tbl_Appraisal_CategoryMaster.Where(category => category.CategoryID == appraisalCategoryId).FirstOrDefault();
                return appraisalCategoryDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<tbl_ApprisalDocumentDetail> GetAppraisalDocumentHistoryForDisplay(int documentID, int appraisalYearID)
        {
            try
            {
                var documentHistory = (from document in dbContext.tbl_ApprisalDocuments
                                       join documentDetails in dbContext.tbl_ApprisalDocumentDetail
                                       on document.DocumentId equals documentDetails.DocumentId
                                       where document.DocumentId == documentID && document.AppraisalYearID == appraisalYearID
                                       orderby documentDetails.DocumentDetailId descending
                                       select documentDetails).ToList();
                return documentHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetUploadNameFromUploadById(string uploadByEmpId)
        {
            string UploadbyName = string.Empty;
            try
            {
                UploadbyName = (from MName in dbContext.HRMS_tbl_PM_Employee
                                where MName.EmployeeCode == uploadByEmpId
                                select MName.EmployeeName).FirstOrDefault();
                return (UploadbyName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<Tbl_HR_UploadType> GetHRUploadTypes()
        {
            return dbContext.Tbl_HR_UploadType.OrderBy(x => x.Description);
        }

        public bool DeleteAppraisalUploadDetails(int documentId, int appraisalYearID)
        {
            bool isDeleted = false;
            try
            {
                List<tbl_ApprisalDocumentDetail> docDetails = dbContext.tbl_ApprisalDocumentDetail.Where(d => d.DocumentId == documentId && d.AppraisalYearID == appraisalYearID).ToList();
                tbl_ApprisalDocuments doc = dbContext.tbl_ApprisalDocuments.Where(d => d.DocumentId == documentId && d.AppraisalYearID == appraisalYearID).FirstOrDefault();

                if (docDetails != null)
                {
                    foreach (tbl_ApprisalDocumentDetail eachdocDetail in docDetails)
                    {
                        dbContext.tbl_ApprisalDocumentDetail.DeleteObject(eachdocDetail);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }

                dbContext.tbl_ApprisalDocuments.DeleteObject(doc);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDeleted;
        }

        public bool DeleteAppraisalDocsSelected(string filename, int appraisalYearID)
        {
            bool isDeleted = false;
            try
            {
                var documentformchild = (from document in dbContext.tbl_ApprisalDocuments
                                         join documentDetails in dbContext.tbl_ApprisalDocumentDetail
                                         on document.DocumentId equals documentDetails.DocumentId
                                         where documentDetails.FileName == filename && documentDetails.AppraisalYearID == appraisalYearID
                                         select documentDetails).FirstOrDefault();

                if (documentformchild != null)
                {
                    dbContext.tbl_ApprisalDocumentDetail.DeleteObject(documentformchild);
                    dbContext.SaveChanges(); isDeleted = true;
                }

                return isDeleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppraisalStatusReportViewModel> GetAppraisalRatingsReportDetails(int YearID)
        {
            dbContext = new HRMSDBEntities();

            //List<AppraisalStatusReportViewModel> appraisalReportList = (from a in dbContext.tbl_Appraisal_AppraisalMaster
            //                                                            join e in dbContext.HRMS_tbl_PM_Employee on a.EmployeeID equals e.EmployeeID into emp
            //                                                            from empList in emp.DefaultIfEmpty()
            //                                                            join a1 in dbContext.HRMS_tbl_PM_Employee on a.Appraiser1 equals a1.EmployeeID into appraiser1
            //                                                            from appraiser1List in appraiser1.DefaultIfEmpty()
            //                                                            join a2 in dbContext.HRMS_tbl_PM_Employee on a.Appraiser2 equals a2.EmployeeID into appraiser2
            //                                                            from appraiser2List in appraiser2.DefaultIfEmpty()
            //                                                            join r1 in dbContext.HRMS_tbl_PM_Employee on a.Reviewer1 equals r1.EmployeeID into reviewer1
            //                                                            from reviewer1List in reviewer1.DefaultIfEmpty()
            //                                                            join r2 in dbContext.HRMS_tbl_PM_Employee on a.Reviewer2 equals r2.EmployeeID into reviewer2
            //                                                            from reviewer2List in reviewer2.DefaultIfEmpty()
            //                                                            join gh in dbContext.HRMS_tbl_PM_Employee on a.GroupHead equals gh.EmployeeID into grouphead
            //                                                            from grouplist in grouphead.DefaultIfEmpty()
            //                                                            join d in dbContext.tbl_PM_DesignationMaster on empList.DesignationID equals d.DesignationID into designation
            //                                                            from designationList in designation.DefaultIfEmpty()
            //                                                            join gm in dbContext.tbl_PM_GroupMaster on empList.GroupID equals gm.GroupID into dt
            //                                                            from deliveryTeamList in dt.DefaultIfEmpty()
            //                                                            join parentDt in dbContext.tbl_PM_ResourcePool on empList.ResourcePoolID equals parentDt.ResourcePoolID into pDt
            //                                                            from parentDtList in pDt.DefaultIfEmpty()
            //                                                            join currentDt in dbContext.tbl_PM_ResourcePool on empList.Current_DU equals currentDt.ResourcePoolID into cDt
            //                                                            from currentDtList in cDt.DefaultIfEmpty()
            //                                                            join ratingComments in dbContext.tbl_Appraisal_RatingComments on a.AppraisalID equals ratingComments.AppraisalID into rc
            //                                                            from ratingCommentList in rc.DefaultIfEmpty()
            //                                                            join param in dbContext.tbl_Appraisal_ParameterMaster on ratingCommentList.ParameterID equals param.ParameterID into pr
            //                                                            from paramName in pr.DefaultIfEmpty()
            //                                                            join PromotionRecommendation in dbContext.tbl_Appraisal_PromotionRecommendation on a.AppraisalID equals PromotionRecommendation.AppraisalID into promo
            //                                                            from promoRec in promo.DefaultIfEmpty()
            //                                                            join desigRev1 in dbContext.tbl_PM_DesignationMaster on promoRec.NextDesignationIDFromReviewer1 equals desigRev1.DesignationID into desigReviewer1
            //                                                            from desig_Reviewer1 in desigReviewer1.DefaultIfEmpty()
            //                                                            join desigRev2 in dbContext.tbl_PM_DesignationMaster on promoRec.NextDesignationIDfromReviewer2 equals desigRev2.DesignationID into desigReviewer2
            //                                                            from desig_Reviewer2 in desigReviewer2.DefaultIfEmpty()
            //                                                            join desigGrpHead in dbContext.tbl_PM_DesignationMaster on promoRec.NextDesignationIDfromGroupHead equals desigGrpHead.DesignationID into desigGroupHead
            //                                                            from desig_GroupHead in desigGroupHead.DefaultIfEmpty()
            //                                                            where a.AppraisalYearID == YearID
            //                                                            orderby empList.EmployeeCode
            //                                                            select new AppraisalStatusReportViewModel
            //                                                            {
            //                                                                EmployeeID = empList.EmployeeID,
            //                                                                EmployeeName = empList.EmployeeName,
            //                                                                Employeecode = empList.EmployeeCode,
            //                                                                ParentDu = parentDtList.ResourcePoolName,
            //                                                                CurrentDu = currentDtList.ResourcePoolName,
            //                                                                DeliveryTeamName = deliveryTeamList.GroupName,
            //                                                                ConfirmationDate = empList.ConfirmationDate,
            //                                                                DesignationName = designationList.DesignationName,
            //                                                                JoiningDate = empList.JoiningDate,
            //                                                                ProbationReviewDate = empList.Probation_Review_Date,
            //                                                                ParameterName = paramName.Parameter,
            //                                                                ParameterId = paramName.Parameter == "" ? 0 : paramName.ParameterID,
            //                                                                Appraiser1Name = appraiser1List.EmployeeName,
            //                                                                //Appraiser1Rating = ratingCommentList.Appraiser1Ratings,
            //                                                                //Appraiser1Comments = ratingCommentList.Appraiser1Comments,

            //                                                                Appraiser2Name = appraiser2List.EmployeeName,
            //                                                                //Appraiser2Rating = ratingCommentList.Appraiser2Ratings,
            //                                                                // Appraiser2Comments = ratingCommentList.Appraiser2Comments,

            //                                                                Reviewer1Name = reviewer1List.EmployeeName,
            //                                                                //Reviewer1Rating = ratingCommentList.Reviewer1Ratings,
            //                                                                // Reviewer1Comments = ratingCommentList.Reviewer1Comments,

            //                                                                Reviewer2Name = reviewer2List.EmployeeName,
            //                                                                Reviewer2Rating = ratingCommentList.Reviewer2Ratings,
            //                                                                Reviewer2Comments = ratingCommentList.Reviewer2Comments,

            //                                                                GroupHeadName = grouplist.EmployeeName,
            //                                                                // GroupHeadRating = ratingCommentList.GroupHeadRatings,
            //                                                                // GroupHeadComments = ratingCommentList.GroupHeadComments,

            //                                                                Reviewer1OverAllRating = ratingCommentList.OverallReviewerRatings.HasValue ? ratingCommentList.OverallReviewerRatings.Value : (int?)null,
            //                                                                Reviewer2OverAllRating = ratingCommentList.OverallReviewer2Ratings.HasValue ? ratingCommentList.OverallReviewer2Ratings.Value : (int?)null,
            //                                                                GroupHeadOverAllRating = ratingCommentList.OverallGroupHeadRatings.HasValue ? ratingCommentList.OverallGroupHeadRatings.Value : (int?)null,

            //                                                                Reviewer1OverAllComment = ratingCommentList.OverallReviewerComments,
            //                                                                Reviewer2OverAllComment = ratingCommentList.OverallReviewer2Comments,
            //                                                                GroupHeadOverAllComment = ratingCommentList.OverallGroupHeadComments,

            //                                                                PromotionRecommentationReviewer1 = promoRec.PromoRecombyReviewer1 == true ? "Yes" : "No",
            //                                                                PromotionRecommentationReviewer2 = promoRec.PromoRecombyReviewer2 == true ? "Yes" : "No",
            //                                                                PromotionRecommentationGroupHead = promoRec.PromoRecombyGroupHead == true ? "Yes" : "No",

            //                                                                NextDesignationReviewer1 = desig_Reviewer1.DesignationName,
            //                                                                NextDesignationReviewer2 = desig_Reviewer2.DesignationName,
            //                                                                NextDesignationGroupHead = desig_GroupHead.DesignationName
            //                                                            }).ToList();
            //return appraisalReportList.GroupBy(x => x.EmployeeID).Select(y => y.First()).ToList();
            int AppraisalYearID = YearID;
            var ratingreport = dbContext.AppraisalRatingcommentsdetail(AppraisalYearID);

            List<AppraisalStatusReportViewModel> appraisalReportList = (from s in ratingreport
                                                                        select new AppraisalStatusReportViewModel
                                                                        {
                                                                            //EmployeeID = s.emp,
                                                                            EmployeeName = s.Employeename,
                                                                            Employeecode = s.employeecode,
                                                                            ParentDu = s.Parent_DU,
                                                                            CurrentDu = s.Current_DU,
                                                                            DeliveryTeamName = s.Groupname,
                                                                            ConfirmationDate = s.ConfirmationDate,
                                                                            DesignationName = s.DesignationName,
                                                                            JoiningDate = s.JoiningDate,
                                                                            ProbationReviewDate = s.Probation_Review_Date,
                                                                            Appraiser1Name = s.appraiser1,
                                                                            ratingOneAppraiserOne = s.Appraiser1Parm1Ratings,
                                                                            ratingTwoAppraiserOne = s.Appraiser1Parm2Ratings,
                                                                            ratingThreeAppraiserOne = s.Appraiser1Parm3Ratings,
                                                                            ratingFourAppraiserOne = s.Appraiser1Parm4Ratings,
                                                                            ratingFiveAppraiserOne = s.Appraiser1Parm5Ratings,
                                                                            ratingSixAppraiserOne = s.Appraiser1Parm6Ratings,
                                                                            CommentOneAppraiserOne = s.Appraiser1Parm1comment,
                                                                            CommentTwoAppraiserOne = s.Appraiser1Parm2comment,
                                                                            CommentThreeAppraiserOne = s.Appraiser1Parm3comment,
                                                                            CommentFourAppraiserOne = s.Appraiser1Parm4comment,
                                                                            CommentFiveAppraiserOne = s.Appraiser1Parm5comment,
                                                                            CommentSixAppraiserOne = s.Appraiser1Parm6comment,
                                                                            Appraiser2Name = s.appraiser2,
                                                                            ratingOneAppraiserTwo = s.Appraiser2Parm1Ratings,
                                                                            ratingTwoAppraiserTwo = s.Appraiser2Parm2Ratings,
                                                                            ratingThreeAppraiserTwo = s.Appraiser2Parm3Ratings,
                                                                            ratingFourAppraiserTwo = s.Appraiser2Parm4Ratings,
                                                                            ratingFiveAppraiserTwo = s.Appraiser2Parm5Ratings,
                                                                            ratingSixAppraiserTwo = s.Appraiser2Parm6Ratings,
                                                                            CommentOneAppraiserTwo = s.Appraiser2Parm1comment,
                                                                            CommentTwoAppraiserTwo = s.Appraiser2Parm2comment,
                                                                            CommentThreeAppraiserTwo = s.Appraiser2Parm3comment,
                                                                            CommentFourAppraiserTwo = s.Appraiser2Parm4comment,
                                                                            CommentFiveAppraiserTwo = s.Appraiser2Parm5comment,
                                                                            CommentSixAppraiserTwo = s.Appraiser2Parm6comment,
                                                                            Reviewer1Name = s.reviewr1,
                                                                            ratingOneReviewerOne = s.Reviewer1Parm1Ratings,
                                                                            ratingTwoReviewerOne = s.Reviewer1Parm2Ratings,
                                                                            ratingThreeReviewerOne = s.Reviewer1Parm3Ratings,
                                                                            ratingFourReviewerOne = s.Reviewer1Parm4Ratings,
                                                                            ratingFiveReviewerOne = s.Reviewer1Parm5Ratings,
                                                                            ratingSixReviewerOne = s.Reviewer1Parm6Ratings,
                                                                            CommentOneReviewerOne = s.Reviewer1Parm1comment,
                                                                            CommentTwoReviewerOne = s.Reviewer1Parm2comment,
                                                                            CommentThreeReviewerOne = s.Reviewer1Parm3comment,
                                                                            CommentFourReviewerOne = s.Reviewer1Parm4comment,
                                                                            CommentFiveReviewerOne = s.Reviewer1Parm5comment,
                                                                            CommentSixReviewerOne = s.Reviewer1Parm6comment,
                                                                            Reviewer2Name = s.reviewr2,
                                                                            ratingOneReviewerTwo = s.Reviewer2Parm1Ratings,
                                                                            ratingTwoReviewerTwo = s.Reviewer2Parm2Ratings,
                                                                            ratingThreeReviewerTwo = s.Reviewer2Parm3Ratings,
                                                                            ratingFourReviewerTwo = s.Reviewer2Parm4Ratings,
                                                                            ratingFiveReviewerTwo = s.Reviewer2Parm5Ratings,
                                                                            ratingSixReviewerTwo = s.Reviewer2Parm6Ratings,
                                                                            CommentOneReviewerTwo = s.Reviewer2Parm1comment,
                                                                            CommentTwoReviewerTwo = s.Reviewer2Parm2comment,
                                                                            CommentThreeReviewerTwo = s.Reviewer2Parm3comment,
                                                                            CommentFourReviewerTwo = s.Reviewer2Parm4comment,
                                                                            CommentFiveReviewerTwo = s.Reviewer2Parm5comment,
                                                                            CommentSixReviewerTwo = s.Reviewer2Parm6comment,
                                                                            GroupHeadName = s.grouphead,
                                                                            ratingOneGroupHead = s.GroupHeadParm1Ratings,
                                                                            ratingTwoGroupHead = s.GroupHeadParm2Ratings,
                                                                            ratingThreeGroupHead = s.GroupHeadParm3Ratings,
                                                                            ratingFourGroupHead = s.GroupHeadParm4Ratings,
                                                                            ratingFiveGroupHead = s.GroupHeadParm5Ratings,
                                                                            ratingSixGroupHead = s.GroupHeadParm6Ratings,
                                                                            CommentOneGroupHead = s.GroupHeadParm1comment,
                                                                            CommentTwoGroupHead = s.GroupHeadParm2comment,
                                                                            CommentThreeGroupHead = s.GroupHeadParm3comment,
                                                                            CommentFourGroupHead = s.GroupHeadParm4comment,
                                                                            CommentFiveGroupHead = s.GroupHeadParm5comment,
                                                                            CommentSixGroupHead = s.GroupHeadParm6comment,
                                                                            Reviewer1OverAllRating = s.OverallReviewerRatings.HasValue ? s.OverallReviewerRatings.Value : (int?)null,
                                                                            Reviewer2OverAllRating = s.OverallReviewer2Ratings.HasValue ? s.OverallReviewer2Ratings.Value : (int?)null,
                                                                            GroupHeadOverAllRating = s.OverallGroupHeadRatings.HasValue ? s.OverallGroupHeadRatings.Value : (int?)null,
                                                                            Reviewer1OverAllComment = s.OverallReviewerComments,
                                                                            Reviewer2OverAllComment = s.OverallReviewer2Comments,
                                                                            GroupHeadOverAllComment = s.OverallGroupHeadComments,
                                                                            PromotionRecommentationReviewer1 = s.PromoRecombyReviewer1 == true ? "Yes" : s.PromoRecombyReviewer1 == false ? "No" : "NA",
                                                                            PromotionRecommentationReviewer2 = s.PromoRecombyReviewer2 == true ? "Yes" : s.PromoRecombyReviewer2 == false ? "No" : "NA",
                                                                            PromotionRecommentationGroupHead = s.PromoRecombyGroupHead == true ? "Yes" : s.PromoRecombyGroupHead == false ? "No" : "NA",
                                                                            NextDesignationReviewer1 = s.rev1Designation,
                                                                            NextDesignationReviewer2 = s.rev2Designation,
                                                                            NextDesignationGroupHead = s.groupheadDesignation
                                                                        }).ToList();
            return appraisalReportList;
        }

        public List<AppraisalStatusReportViewModel> getAppraisalRatingAndComments(int YearID)
        {
            try
            {
                List<AppraisalStatusReportViewModel> appraisalReportList = (from RatingComments in dbContext.tbl_Appraisal_RatingComments
                                                                            join Appraisal in dbContext.tbl_Appraisal_AppraisalMaster on RatingComments.AppraisalID equals Appraisal.AppraisalID
                                                                            join parametermaster in dbContext.tbl_Appraisal_ParameterMaster on RatingComments.ParameterID equals parametermaster.ParameterID
                                                                            where Appraisal.AppraisalYearID == YearID && RatingComments.AppraisalID == Appraisal.AppraisalID
                                                                            select new AppraisalStatusReportViewModel
                                                                            {
                                                                                EmployeeID = Appraisal.EmployeeID,
                                                                                Appraiser1Rating = RatingComments.Appraiser1Ratings,
                                                                                Appraiser1Comments = RatingComments.Appraiser1Comments,
                                                                                Appraiser2Rating = RatingComments.Appraiser2Ratings,
                                                                                Appraiser2Comments = RatingComments.Appraiser2Comments,
                                                                                Reviewer1Rating = RatingComments.Reviewer1Ratings,
                                                                                Reviewer1Comments = RatingComments.Reviewer1Comments,
                                                                                Reviewer2Rating = RatingComments.Reviewer2Ratings,
                                                                                Reviewer2Comments = RatingComments.Reviewer2Comments,
                                                                                GroupHeadRating = RatingComments.GroupHeadRatings,
                                                                                GroupHeadComments = RatingComments.GroupHeadComments,
                                                                                ParameterId = RatingComments.ParameterID
                                                                            }).ToList();
                return appraisalReportList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ParametersForCurrentUear> getParamterList(int YearID)
        {
            try
            {
                List<ParametersForCurrentUear> paramterList = (from paramter in dbContext.tbl_Appraisal_ParameterMaster
                                                               where paramter.AppraisalYearID == YearID
                                                               select new ParametersForCurrentUear
                                                               {
                                                                   ParameterId = paramter.ParameterID,
                                                                   ParameterName = paramter.Parameter
                                                               }).ToList();
                return paramterList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<int?> GetAppraiserList(int YearID)
        {
            try
            {
                var appraiserList = (from r in dbContext.tbl_Appraisal_AppraisalMaster
                                     where r.AppraisalYearID == YearID
                                     select r.Appraiser1).ToList();

                return appraiserList.Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}