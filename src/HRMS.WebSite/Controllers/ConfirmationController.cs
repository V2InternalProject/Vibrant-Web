using HRMS.DAL;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class ConfirmationController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                Session["SearchEmpFullName"] = null;  // to hide emp search
                Session["SearchEmpCode"] = null;
                Session["SearchEmpID"] = null;

                ConfirmationViewModel confirmationmodel = new ConfirmationViewModel();
                string EmployeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                confirmationmodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                confirmationmodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                confirmationmodel.SearchedUserDetails.EmployeeCode = EmployeeCode;
                EmployeeDAL DAL = new EmployeeDAL();
                confirmationmodel.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
                return View(confirmationmodel);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ConfigureParameter()
        {
            try
            {
                Session["SearchEmpFullName"] = null;  // to hide emp search
                Session["SearchEmpCode"] = null;
                Session["SearchEmpID"] = null;

                ConfirmationViewModel confirmationmodel = new ConfirmationViewModel();
                string EmployeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                confirmationmodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                confirmationmodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                confirmationmodel.SearchedUserDetails.EmployeeCode = EmployeeCode;
                EmployeeDAL DAL = new EmployeeDAL();
                confirmationmodel.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
                ConfigurationDAL configDAL = new ConfigurationDAL();
                List<CompetencyMaster> competencyMaster = configDAL.GetCompetencyMaster();
                confirmationmodel.RecordsCount = competencyMaster.Count;
                confirmationmodel.CompetencyMasters = competencyMaster;
                return PartialView("_ConfigureParameter", confirmationmodel);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult AddParameter(int? orderID)
        {
            addParameter addparameter = new addParameter();
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            addparameter.SearchedUserDetails = new SearchedUserDetails();
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            addparameter.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
            addparameter.SearchedUserDetails.EmployeeCode = Membership.GetUser().UserName;
            EmployeeDAL DAL = new EmployeeDAL();
            addparameter.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(Membership.GetUser().UserName);
            ConfigurationDAL configDAL = new ConfigurationDAL();
            if (orderID != null)
            {
                tbl_PA_Competency_Master competencyMaster = configDAL.getParameter(orderID);
                addparameter.Parameter = competencyMaster.Competency;
                addparameter.OrderNo = competencyMaster.OrderNo;
                addparameter.category = Convert.ToString(competencyMaster.CategoryID);
                addparameter.BehavioralIndicators = competencyMaster.BehavioralIndicators;
                addparameter.Description = competencyMaster.Description;
                addparameter.CompetencyID = competencyMaster.CompetencyID;
            }
            addparameter.CategoryList = configDAL.getCategoryList();
            return PartialView("_addParameter", addparameter);
        }

        [HttpPost]
        public ActionResult AddParameter(addParameter model)
        {
            try
            {
                bool success = false;
                string result = null;
                int OrderNumber = 0;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                success = configDAL.SaveParameter(model);
                if (model.IsAddnew)
                {
                    result = "addnew";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (success)
                    {
                        result = "Saved";
                        OrderNumber = model.OrderNo.HasValue ? model.OrderNo.Value : 0;
                    }
                    else
                    {
                        result = "Error";
                    }
                    return Json(new { resultMesssage = result, status = success, orderNumber = OrderNumber }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteParameter(List<int> collection)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    success = configDAL.DeleteParameter(collection);
                    if (success)
                        result = "Deleted";
                    else
                        result = "Error";
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ApplicableRoles(int? competencyID)
        {
            try
            {
                Session["competencyID"] = competencyID;
                var sessionCompetencyID = (int)Session["competencyID"];
                ApplicableRolesViewModel applicablerole = new ApplicableRolesViewModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                applicablerole.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                applicablerole.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (competencyID != null)
                {
                    List<ApplicableRole> competencyRoleApplicability = configDAL.getApplicableRoles(competencyID);
                    applicablerole.ApplicableRoles = competencyRoleApplicability;
                }
                return PartialView("_ApplicableRoles", applicablerole);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteRole(List<int> collection, int competencyID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    success = configDAL.DeleteRoles(collection, competencyID);
                    result = "Deleted";
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SelectRoles(string roleID)
        {
            try
            {
                int competencyID = (int)Session["competencyID"];
                ApplicableRolesViewModel applicablerole = new ApplicableRolesViewModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                applicablerole.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                applicablerole.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                ConfigurationDAL configDAL = new ConfigurationDAL();
                List<ApplicableRole> NewSelectRole = new List<ApplicableRole>();
                if (roleID != "")
                {
                    string roleIDWithcomma = roleID.TrimEnd(',');
                    string[] roleidArray = roleIDWithcomma.Split(',');
                    int[] myInts = Array.ConvertAll(roleidArray, s => int.Parse(s));
                    NewSelectRole = configDAL.getNewSelectRole(myInts, competencyID);
                }
                else
                {
                    HRMSDBEntities dbContext = new HRMSDBEntities();
                    NewSelectRole = (from e in dbContext.HRMS_tbl_PM_Role
                                     orderby e.RoleDescription ascending
                                     select new ApplicableRole
                                     {
                                         CompetencyID = competencyID,
                                         RoleID = e.RoleID,
                                         Role = e.RoleDescription
                                     }).ToList();
                }
                applicablerole.ApplicableRoles = NewSelectRole;
                return PartialView("_SelectRoles", applicablerole);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveNewRole(List<int> roleID, int competencyID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (roleID.Count != 0)
                {
                    success = configDAL.SaveNewRole(roleID, competencyID);
                    result = "Saved";
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        #region MyRegion

        [HttpGet]
        public ActionResult ConfigureParameterCategories()
        {
            try
            {
                ConfigureParameterCategory configParametercategory = new ConfigureParameterCategory();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                configParametercategory.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                configParametercategory.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);

                configParametercategory.SearchedUserDetails.EmployeeId = employeeID;
                configParametercategory.SearchedUserDetails.EmployeeCode = personalDAL.getEmployeeCode(employeeID);

                ConfigurationDAL configDAL = new ConfigurationDAL();
                List<Parametercompetency> parametercompetency = configDAL.GetParameterCategories();
                configParametercategory.RecordsCount = parametercompetency.Count;
                configParametercategory.Parametercompetencys = parametercompetency;
                return PartialView("_ConfigureParameterCompetency", configParametercategory);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult AddParameterCompetency(int? categoryID)
        {
            try
            {
                AddNewCategory addnewcategory = new AddNewCategory();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                addnewcategory.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                addnewcategory.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                addnewcategory.SearchedUserDetails.EmployeeId = employeeID;
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                addnewcategory.SearchedUserDetails.EmployeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigurationDAL configDAL = new ConfigurationDAL();

                if (categoryID != null)
                {
                    tbl_PA_CompetencyCategories parameterCategory = configDAL.getParametercategory(categoryID);
                    addnewcategory.CategoryID = parameterCategory.CategoryID;
                    addnewcategory.Category = parameterCategory.CategoryType;
                    addnewcategory.Description = parameterCategory.CategoryDescription;
                }
                return PartialView("_addParameterCategory", addnewcategory);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddParameterCompetency(AddNewCategory addnewcategory)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                success = configDAL.SaveParameterCategory(addnewcategory);
                if (addnewcategory.IsAddnew)
                {
                    result = "addnew";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (success)
                        result = "Saved";
                    else
                        result = "Error";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteParameterCompetency(List<int> collection)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    success = configDAL.DeleteParameterCompetency(collection);
                    result = "Deleted";
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        #endregion MyRegion

        #region MyRegion

        [HttpGet]
        public ActionResult ConfigureRatingScales()
        {
            ConfigureRatingScales configratingSale = new ConfigureRatingScales();
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            configratingSale.SearchedUserDetails = new SearchedUserDetails();
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            configratingSale.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
            PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
            EmployeeDAL dal = new EmployeeDAL();
            int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);

            configratingSale.SearchedUserDetails.EmployeeId = employeeID;
            configratingSale.SearchedUserDetails.EmployeeCode = personalDAL.getEmployeeCode(employeeID);
            ConfigurationDAL configDAL = new ConfigurationDAL();
            List<RatingScales> ratingScale = configDAL.GetRatingScales();
            configratingSale.RecordsCount = ratingScale.Count;
            configratingSale.RatingScale = ratingScale;
            return PartialView("_ConfigureRatingScales", configratingSale);
        }

        [HttpGet]
        public ActionResult AddRatingScales(int? RatingID)
        {
            try
            {
                AddRatingScale addNewRating = new AddRatingScale();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                addNewRating.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                addNewRating.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                addNewRating.SearchedUserDetails.EmployeeId = employeeID;
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                addNewRating.SearchedUserDetails.EmployeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigurationDAL configDAL = new ConfigurationDAL();

                if (RatingID != null)
                {
                    tbl_PA_Rating_Master ratingScale = configDAL.getRatingScaleDetails(RatingID);
                    addNewRating.Percentage = ratingScale.Percentage;
                    addNewRating.Rating = ratingScale.Rating;
                    addNewRating.RatingID = ratingScale.RatingID;
                    addNewRating.Description = ratingScale.Description;
                    addNewRating.AdjustmentFactor = ratingScale.AdjustmentFactor;
                    addNewRating.SetAsMinimumLimit = ratingScale.SetAsMinimumLimit.HasValue ? ratingScale.SetAsMinimumLimit.Value : false;
                }
                return PartialView("_addRatingScale", addNewRating);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddRatingScales(AddRatingScale model)
        {
            try
            {
                bool success = false;
                string result = null;
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                string UserRole = Commondal.GetMaxRoleForUser(role);
                ConfigurationDAL configDAL = new ConfigurationDAL();
                success = configDAL.SaveRatingScales(model, UserRole);
                if (model.IsAddnew)
                {
                    result = "addnew";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (success)
                        result = "Saved";
                    else
                        result = "Error";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteRatingScales(List<int> collection)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    success = configDAL.DeleteRatingScales(collection);
                    result = "Deleted";
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        #endregion MyRegion

        #region MyRegion

        public ActionResult ConfigureParametersRoles(string searchstring)
        {
            try
            {
                ConfigureParametersforRoles parameterRoles = new ConfigureParametersforRoles();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                parameterRoles.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                parameterRoles.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                parameterRoles.SearchedUserDetails.EmployeeId = employeeID;
                parameterRoles.SearchedUserDetails.EmployeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigurationDAL configDAL = new ConfigurationDAL();
                List<RoleLists> roleList = configDAL.getAllRoles();
                if (searchstring != null)
                {
                    List<RoleLists> roleListwithsearch = roleList.FindAll(x => x.RoleDescription.ToLower().Contains(searchstring.ToLower()));
                    parameterRoles.RoleList = roleListwithsearch;
                    parameterRoles.RecordsCount = roleList.Count;
                }
                else
                {
                    parameterRoles.RoleList = roleList;
                    parameterRoles.RecordsCount = roleList.Count;
                }

                return PartialView("_ConfigureParameterRole", parameterRoles);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult EditParameterRole(int RoleID)
        {
            try
            {
                Session["RoleID"] = RoleID;
                AssocieteDriverAndComepetency driverAndComeptency = new AssocieteDriverAndComepetency();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                driverAndComeptency.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                driverAndComeptency.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                driverAndComeptency.SearchedUserDetails.EmployeeId = employeeID;
                driverAndComeptency.SearchedUserDetails.EmployeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigurationDAL configDAL = new ConfigurationDAL();
                List<Competencies> competency = configDAL.getCompetenciesForRole(RoleID);
                driverAndComeptency.CompetencyList = competency;
                driverAndComeptency.RecordsCount = competency.Count;
                driverAndComeptency.RoleID = RoleID;
                return PartialView("_CompetenciesForRole", driverAndComeptency);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SelectNewCompetency(string CompetencyID, string searchstring)
        {
            try
            {
                int RoleID = (int)Session["RoleID"];
                AssocieteDriverAndComepetency selectCompetency = new AssocieteDriverAndComepetency();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                selectCompetency.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                selectCompetency.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                ConfigurationDAL configDAL = new ConfigurationDAL();
                List<Competencies> NewSelectCompetency = new List<Competencies>();
                if (CompetencyID != "")
                {
                    string CompetencyIDWithcomma = CompetencyID.TrimEnd(',');
                    string[] CompetencyIDArray = CompetencyIDWithcomma.Split(',');
                    int[] myInts = Array.ConvertAll(CompetencyIDArray, s => int.Parse(s));
                    NewSelectCompetency = configDAL.getNewSelectCompetency(myInts, RoleID);
                }
                else
                {
                    HRMSDBEntities dbContext = new HRMSDBEntities();
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
                }
                if (searchstring != "")
                {
                    List<Competencies> competencyListwithsearch = NewSelectCompetency.FindAll(x => x.Parameter.ToLower().Contains(searchstring.ToLower()));
                    selectCompetency.CompetencyList = competencyListwithsearch;
                    selectCompetency.RecordsCount = competencyListwithsearch.Count;
                }
                else
                {
                    selectCompetency.CompetencyList = NewSelectCompetency;
                    selectCompetency.RecordsCount = NewSelectCompetency.Count;
                }

                return PartialView("_SelectCompetencies", selectCompetency);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveNewCompetency(List<int> CompetencyID, int RoleID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (CompetencyID.Count != 0)
                {
                    success = configDAL.SaveNewCompetency(CompetencyID, RoleID);
                    result = "Saved";
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteNewCompetency(List<int> collection, int RoleID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    success = configDAL.DeletenewCompetencies(collection, RoleID);
                    result = "Deleted";
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        #endregion MyRegion
    }
}