using HRMS.DAL;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class OrganizationStructureController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                Session["SearchEmpFullName"] = null;  // to hide emp search
                Session["SearchEmpCode"] = null;
                Session["SearchEmpID"] = null;

                OrganizationStructure organizationStrucure = GetOragnizationStructureInstance();
                return View(organizationStrucure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static OrganizationStructure GetOragnizationStructureInstance()
        {
            OrganizationStructure organizationStrucure = new OrganizationStructure();
            organizationStrucure.SearchedUserDetails = new SearchedUserDetails();
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            EmployeeDAL employeeDal = new EmployeeDAL();

            string employeeCode = Membership.GetUser().UserName;
            int employeeID = employeeDal.GetEmployeeID(employeeCode);
            string[] role = Roles.GetRolesForUser(employeeCode);
            if (employeeCode != null)
            {
                organizationStrucure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                organizationStrucure.SearchedUserDetails.EmployeeId = employeeID;
                organizationStrucure.SearchedUserDetails.EmployeeCode = employeeCode;
            }
            ConfigurationDAL configDAL = new ConfigurationDAL();
            List<BusinessGroup> businessGroups = configDAL.getBusinessGroups(0);
            List<BusinessGroup> businessGroupswithManagers = configDAL.getManagersListForBusinessGroup(businessGroups);
            List<Object> ExtremFinallist = new List<Object>();
            ExtremFinallist.AddRange(businessGroupswithManagers);

            foreach (var item in businessGroups)
            {
                List<Object> finalList = new List<Object>();
                List<OrganizationUnit> organizationUnits = configDAL.getOrganizationUnit(item.BusinessGroupID);
                List<OrganizationUnit> organizationUnitswithManager = configDAL.getManagersForOrganizationUnit(organizationUnits);
                finalList.AddRange(organizationUnitswithManager);
                List<DeliveryUnit> deliveryUnits = configDAL.getDeliveryUnit(item.BusinessGroupID);

                List<DeliveryUnit> deliveryUnitswithManagers = configDAL.getManagersForDeliveryUnit(deliveryUnits);
                finalList.AddRange(deliveryUnitswithManagers);
                List<DeliveryTeam> deliveryTeams = configDAL.getDeliveryTeam(item.BusinessGroupID);
                finalList.AddRange(deliveryTeams);
                ExtremFinallist.AddRange(finalList);
            }
            List<BusinessGroup> BusinessGroup = new List<BusinessGroup>();
            List<OrganizationUnit> OrganizationUnit = new List<OrganizationUnit>();
            List<DeliveryUnit> DeliveryUnit = new List<DeliveryUnit>();
            List<DeliveryTeam> DeliveryTeam = new List<DeliveryTeam>();
            foreach (var item in ExtremFinallist)
            {
                if (item.GetType() == typeof(BusinessGroup))
                {
                    BusinessGroup.Add((BusinessGroup)item);
                }
                else if (item.GetType() == typeof(OrganizationUnit))
                {
                    OrganizationUnit.Add((OrganizationUnit)item);
                }
                else if (item.GetType() == typeof(DeliveryUnit))
                {
                    DeliveryUnit.Add((DeliveryUnit)item);
                }
                else if (item.GetType() == typeof(DeliveryTeam))
                {
                    DeliveryTeam.Add((DeliveryTeam)item);
                }
            }
            organizationStrucure.BusinessGroups = BusinessGroup;
            organizationStrucure.OrganizationUnits = OrganizationUnit;
            organizationStrucure.DeliveryUnits = DeliveryUnit;
            organizationStrucure.DeliveryTeams = DeliveryTeam;
            return organizationStrucure;
        }

        public ActionResult ConfigureOrganizationStructure()
        {
            try
            {
                OrganizationStructure organizationStrucure = GetOragnizationStructureInstance();
                ConfigurationDAL configDAL = new ConfigurationDAL();
                organizationStrucure.InActiveBusinessGroups = configDAL.getInactiveBusinessGroup();
                organizationStrucure.InActiveOrganizationUnits = configDAL.getInactiveOraganizationUnits();
                organizationStrucure.InActiveDeliveryUnits = configDAL.getInactiveDeliveryUnit();
                organizationStrucure.InActiveDeliveryTeams = configDAL.getInactiveDeliveryTeams();

                return PartialView("_ConfigureOrganizationStructure", organizationStrucure);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ConfigureBusinessGroups(int BusinessGroupID)
        {
            try
            {
                OrganizationStructure ConfigureOrganizationstructure = new OrganizationStructure();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                ConfigureOrganizationstructure.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                ConfigureOrganizationstructure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                string employeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeId = employeeID;
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeCode = employeeCode;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                List<BusinessGroup> businessGroups = configDAL.getBusinessGroups(BusinessGroupID);
                List<BusinessGroup> businessGroupswithManagers = configDAL.getManagersListForBusinessGroup(businessGroups);
                List<MiddleLevelResources> MiddleLevelResources = configDAL.getMiddleLevelResources(BusinessGroupID);
                List<OrganizationUnit> organizationUnits = configDAL.getOrganizationUnit(BusinessGroupID);
                List<OrganizationUnit> organizationUnitswithManager = configDAL.getManagersForOrganizationUnit(organizationUnits);
                List<DeliveryUnit> deliveryUnits = configDAL.getDeliveryUnit(BusinessGroupID);
                List<DeliveryUnit> deliveryUnitswithManagers = configDAL.getManagersForDeliveryUnit(deliveryUnits);
                List<DeliveryTeam> deliveryTeams = configDAL.getDeliveryTeam(BusinessGroupID);
                tbl_CNF_BusinessGroups _BusinessGroup = configDAL.getBusinessGroupDetails(BusinessGroupID);
                ConfigureOrganizationstructure.businessgroup = _BusinessGroup.BusinessGroup;
                ConfigureOrganizationstructure.BusinessGroupCode = _BusinessGroup.BusinessGroupCode;
                ConfigureOrganizationstructure.Active = _BusinessGroup.Active.HasValue ? _BusinessGroup.Active.Value : false;
                ConfigureOrganizationstructure.BusinessGroupID = BusinessGroupID;
                ConfigureOrganizationstructure.BusinessGroups = businessGroupswithManagers;
                ConfigureOrganizationstructure.OrganizationUnits = organizationUnitswithManager;
                ConfigureOrganizationstructure.DeliveryUnits = deliveryUnitswithManagers;
                ConfigureOrganizationstructure.DeliveryTeams = deliveryTeams;
                ConfigureOrganizationstructure.MiddleLevelResourcesList = MiddleLevelResources;
                ConfigureOrganizationstructure.TotalOrganizationUnits = organizationUnitswithManager.Count;
                ConfigureOrganizationstructure.TotalBusinessGroupManagers = businessGroupswithManagers.SelectMany(x => x.EmployeeList).Count();
                ConfigureOrganizationstructure.TotalMiddleLevelResources = MiddleLevelResources.Count;
                return PartialView("_ConfigureBusinessGroup", ConfigureOrganizationstructure);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult LoadExistingOU(int BusinessGroupID)
        {
            try
            {
                OrganizationStructure ConfigureOrganizationstructure = new OrganizationStructure();
                if (BusinessGroupID != 0)
                {
                    ConfigurationDAL configDAL = new ConfigurationDAL();
                    List<OrganizationUnit> organizationUnits = configDAL.getOrganizationUnit(BusinessGroupID);
                    List<ExistingOrganizationUnit> ExistingOrganizationUnits = configDAL.getExistingOU(BusinessGroupID, organizationUnits);
                    ConfigureOrganizationstructure.BusinessGroupID = BusinessGroupID;
                    ConfigureOrganizationstructure.ExistingOrganizationUnits = ExistingOrganizationUnits;
                }
                return PartialView("_AddExistingOU", ConfigureOrganizationstructure);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteBusinessGroupManagers(List<int> collection, int BusinessGroupID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    success = configDAL.DeleteBusinessManager(collection, BusinessGroupID);
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

        [HttpPost]
        public ActionResult DeleteMiddleLevelResources(List<int> collection, int BusinessGroupID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    success = configDAL.DeleteMiddleLevelResource(collection, BusinessGroupID);
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

        [HttpPost]
        public ActionResult SaveBusinessGroup(OrganizationStructure model)
        {
            OrganizationStructureResponse Response = new OrganizationStructureResponse();
            try
            {
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (model != null)
                {
                    Response = configDAL.SaveBusinessGroups(model);
                }
                return Json(new { isAdded = Response.Isadded, isExisted = Response.IsExisted }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { isAdded = false, isExisted = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddBusinessGroupManager(string EmployeeID, int BusinessGroupID, int empID)
        {
            try
            {
                OrganizationStructure _OrgStructure = new OrganizationStructure();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                _OrgStructure.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                _OrgStructure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL dal = new EmployeeDAL();
                _OrgStructure.SearchedUserDetails.EmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (empID != 0)
                {
                    string EmployeeIDWithcomma = EmployeeID.TrimEnd(',');
                    string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                    int[] myIntEmployeeID = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                    int[] NewEmployeeID = new int[myIntEmployeeID.Count() - 1];
                    tbl_CNF_BusinessGroup_Managers BusinessGroupManagers = configDAL.EditBusinessGroupManager(empID);
                    _OrgStructure.Manager = Convert.ToString(BusinessGroupManagers.ManagerID);
                    _OrgStructure.IsPrimaryResponsible = BusinessGroupManagers.IsPrimaryResponsible;
                    int i = 0;
                    foreach (var item in myIntEmployeeID)
                    {
                        if (item != empID)
                        {
                            NewEmployeeID[i] = item;
                            i++;
                        }
                    }
                    _OrgStructure.BusinessGroupManagerList = configDAL.getallemployee(NewEmployeeID);
                }
                else
                {
                    if (EmployeeID != "")
                    {
                        string EmployeeIDWithcomma = EmployeeID.TrimEnd(',');
                        string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                        int[] myIntEmployeeID = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                        _OrgStructure.BusinessGroupManagerList = configDAL.getallemployee(myIntEmployeeID);
                    }
                    else
                    {
                        int[] EmptyEmployeeID = new int[0];
                        _OrgStructure.BusinessGroupManagerList = configDAL.getallemployee(EmptyEmployeeID);
                    }
                }
                _OrgStructure.Old_Manager = empID;
                _OrgStructure.BusinessGroupID = BusinessGroupID;
                return PartialView("_AddBusinessGroupManager", _OrgStructure);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddBusinessGroupManager(OrganizationStructure model)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (model.Manager != null)
                {
                    success = configDAL.SaveBusinessGroupManagers(model);
                    if (success)
                        result = "Saved";
                    else
                        result = "Error";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = "ErrorInSave";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveOrganizationUnit(OrganizationStructure model)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                success = configDAL.saveOrganizationUnits(model);
                if (success)
                    result = "Saved";
                else
                    result = "Error";
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteOrganizationUnit(string LocationID, int BusinessGroupID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                string LocationIDWithcomma = LocationID.TrimEnd(',');
                string[] LocationIDArray = LocationIDWithcomma.Split(',');
                int[] collection = Array.ConvertAll(LocationIDArray, s => int.Parse(s));
                success = configDAL.deleteOrganizationUnits(collection, BusinessGroupID);
                if (success)
                    result = "Deleted";
                else
                    result = "Error";
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult SelectResourcesForBusinessGroup(string EmployeeID, int BusinessGroupID)
        {
            try
            {
                OrganizationStructure _OrgStructure = new OrganizationStructure();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                _OrgStructure.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                _OrgStructure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                EmployeeDAL dal = new EmployeeDAL();
                _OrgStructure.SearchedUserDetails.EmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);
                _OrgStructure.BusinessGroupID = BusinessGroupID;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                int[] collection = new int[1000];
                if (EmployeeID != "")
                {
                    string EmployeeIDWithcomma = EmployeeID.TrimEnd(',');
                    string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                    collection = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                }
                else
                {
                    collection = new int[0];
                }
                List<MiddleLevelResources> selectResources = configDAL.selectNewResouce(collection, BusinessGroupID);
                _OrgStructure.MiddleLevelResourcesList = selectResources;
                _OrgStructure.TotalMiddleLevelResources = selectResources.Count;
                return PartialView("_selectResourcesForBusinessGroup", _OrgStructure);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveNewResouceForMiddleLevelForBusinessGroup(string EmployeeID, int BusinessGroupID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                string EmployeeIDWithcomma = EmployeeID.TrimEnd(',');
                string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                int[] collection = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                success = configDAL.saveMiddleLevelResouceForBusinessGroup(collection, BusinessGroupID);
                if (success)
                    result = "Saved";
                else
                    result = "Error";
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        #region MyRegion

        [HttpPost]
        public ActionResult ConfigureDeleveryTeams(int BusinessGroupID, int GroupID)
        {
            try
            {
                OrganizationStructure ConfigureOrganizationstructure = new OrganizationStructure();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                ConfigureOrganizationstructure.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                ConfigureOrganizationstructure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                string employeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeId = employeeID;
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeCode = employeeCode;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                tbl_PM_GroupMaster GroupMaster = configDAL.getDeleveryDetails(GroupID);
                ConfigureOrganizationstructure.BusinessGroupID = BusinessGroupID;
                ConfigureOrganizationstructure.GroupCode = GroupMaster.GroupCode;
                ConfigureOrganizationstructure.GroupID = GroupMaster.GroupID;
                ConfigureOrganizationstructure.GroupName = GroupMaster.GroupName;
                ConfigureOrganizationstructure.Manager = Convert.ToString(GroupMaster.ResourceHeadID.HasValue ? GroupMaster.ResourceHeadID.Value : 0);
                ConfigureOrganizationstructure.Active = GroupMaster.Active.HasValue ? GroupMaster.Active.Value : false;
                int[] employeeIds = new int[0];
                ConfigureOrganizationstructure.DeleveryTeamHeadList = configDAL.getallemployee(employeeIds);
                List<MiddleLevelResources> MiddleLevelResources = configDAL.getMiddleLevelResourcesForDeleveryTeam(BusinessGroupID, GroupID);
                ConfigureOrganizationstructure.MiddleLevelResourcesList = MiddleLevelResources;
                ConfigureOrganizationstructure.TotalMiddleLevelResources = MiddleLevelResources.Count;
                return PartialView("_ConfigureDeleveryTeam", ConfigureOrganizationstructure);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult SelectResourcesForDeleveryTeams(string EmployeeID, int BusinessGroupID, int GroupID)
        {
            OrganizationStructure _OrgStructure = new OrganizationStructure();
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            _OrgStructure.SearchedUserDetails = new SearchedUserDetails();
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            _OrgStructure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
            EmployeeDAL dal = new EmployeeDAL();
            _OrgStructure.SearchedUserDetails.EmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);
            _OrgStructure.BusinessGroupID = BusinessGroupID;
            ConfigurationDAL configDAL = new ConfigurationDAL();
            int[] collection = new int[1000];
            if (EmployeeID != "")
            {
                string EmployeeIDWithcomma = EmployeeID.TrimEnd(',');
                string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                collection = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
            }
            else
            {
                collection = new int[0];
            }
            List<MiddleLevelResources> selectResources = configDAL.selectNewResouce(collection, BusinessGroupID);
            _OrgStructure.GroupID = GroupID;
            _OrgStructure.MiddleLevelResourcesList = selectResources;
            _OrgStructure.TotalMiddleLevelResources = selectResources.Count;
            return PartialView("_selectResourcesForDeleveryTeam", _OrgStructure);
        }

        [HttpPost]
        public ActionResult SaveNewResouceForMiddleLevelForDeleveryTeam(string EmployeeID, int GroupID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                int[] collection = new int[1000];
                if (EmployeeID != "")
                {
                    string EmployeeIDWithcomma = EmployeeID.TrimEnd(',');
                    string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                    collection = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                }
                else
                {
                    collection = new int[0];
                }
                success = configDAL.saveMiddleLevelResouceForDeleveryTeam(collection, GroupID);
                if (success)
                    result = "Saved";
                else
                    result = "Error";
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteMiddleLevelResourcesForDeleveryTeam(List<int> collection, int GroupID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    success = configDAL.DeleteMiddleLevelResourceForDeleveryTeam(collection, GroupID);
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

        [HttpPost]
        public ActionResult SaveDeleveryTeams(OrganizationStructure model)
        {
            try
            {
                ConfigurationDAL configDAL = new ConfigurationDAL();
                OrganizationStructureResponse response = new OrganizationStructureResponse();
                response = configDAL.SaveDeleveryTeam(model);
                return Json(new { isAdded = response.Isadded, isActive = response.IsActive }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion MyRegion

        #region MyRegion

        [HttpPost]
        public ActionResult ConfigureDeleveryUnits(int BusinessGroupID, int ResourcePoolID)
        {
            try
            {
                OrganizationStructure ConfigureOrganizationstructure = new OrganizationStructure();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                ConfigureOrganizationstructure.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                ConfigureOrganizationstructure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                string employeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeId = employeeID;
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeCode = employeeCode;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                HRMS_tbl_PM_ResourcePool _resourcePool = configDAL.getDeleveryUnitDetails(ResourcePoolID);
                ConfigureOrganizationstructure.ResourcePoolCode = _resourcePool.ResourcePoolCode;
                ConfigureOrganizationstructure.ResourcePoolName = _resourcePool.ResourcePoolName;
                ConfigureOrganizationstructure.Active = _resourcePool.Active.HasValue ? _resourcePool.Active.Value : false;
                ConfigureOrganizationstructure.TimesheetRequired = _resourcePool.TimesheetRequired.HasValue ? _resourcePool.TimesheetRequired.Value : false;

                List<DeliveryTeam> deliveryTeams = configDAL.getDeliveryTeamForDeleveryUnit(BusinessGroupID, ResourcePoolID);
                List<ManagerList> _deliveryUnitManagers = configDAL.getdeliveryUnitManagers(BusinessGroupID, ResourcePoolID);
                List<MiddleLevelResources> MiddleLevelResourcesList = configDAL.getMiddleLevelResourcesForDeleveryUnits(BusinessGroupID, ResourcePoolID);
                ConfigureOrganizationstructure.BusinessGroupID = BusinessGroupID;
                ConfigureOrganizationstructure.ResourcePoolID = ResourcePoolID;
                ConfigureOrganizationstructure.DeliveryTeams = deliveryTeams;
                ConfigureOrganizationstructure.DeleveryUnitManagerList = _deliveryUnitManagers;
                ConfigureOrganizationstructure.TotalDeliveryTeams = deliveryTeams.Count;
                ConfigureOrganizationstructure.TotalDeliveryUnitsManagers = _deliveryUnitManagers.Count;
                ConfigureOrganizationstructure.MiddleLevelResourcesList = MiddleLevelResourcesList;
                ConfigureOrganizationstructure.TotalMiddleLevelResources = MiddleLevelResourcesList.Count;
                return PartialView("_ConfigureDeliveryUnit", ConfigureOrganizationstructure);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteDeliveryTeam(string GroupID, int ResourcePoolID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                string GroupIDWithcomma = GroupID.TrimEnd(',');
                string[] GroupIDArray = GroupIDWithcomma.Split(',');
                int[] collection = Array.ConvertAll(GroupIDArray, s => int.Parse(s));
                success = configDAL.deleteDeleteDeliveryTeams(collection, ResourcePoolID);
                if (success)
                    result = "Deleted";
                else
                    result = "Error";
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult AddDeliveryUnitManager(string EmployeeID, int ResourcePoolID, int empID, int BusinessGroupID)
        {
            try
            {
                OrganizationStructure _OrgStructure = new OrganizationStructure();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                _OrgStructure.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                _OrgStructure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL dal = new EmployeeDAL();
                _OrgStructure.SearchedUserDetails.EmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (empID != 0)
                {
                    string EmployeeIDWithcomma = EmployeeID.TrimEnd(',');
                    string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                    int[] myIntEmployeeID = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                    int[] NewEmployeeID = new int[myIntEmployeeID.Count() - 1];

                    tbl_PM_ResourcePool_Managers _PM_ResourcePool_Managers = configDAL.EditDeliveryUnitManager(empID);
                    _OrgStructure.Manager = Convert.ToString(_PM_ResourcePool_Managers.ManagerID);
                    _OrgStructure.IsPrimaryResponsible = _PM_ResourcePool_Managers.IsPrimaryResponsible.HasValue ? _PM_ResourcePool_Managers.IsPrimaryResponsible.Value : false;
                    int i = 0;
                    foreach (var item in myIntEmployeeID)
                    {
                        if (item != empID)
                        {
                            NewEmployeeID[i] = item;
                            i++;
                        }
                    }
                    _OrgStructure.DeleveryUnitManagerList = configDAL.getallemployee(NewEmployeeID);
                }
                else
                {
                    if (EmployeeID != "")
                    {
                        string EmployeeIDWithcomma = EmployeeID.TrimEnd(',');
                        string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                        int[] myIntEmployeeID = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                        _OrgStructure.DeleveryUnitManagerList = configDAL.getallemployee(myIntEmployeeID);
                    }
                    else
                    {
                        int[] EmptyEmployeeID = new int[0];
                        _OrgStructure.DeleveryUnitManagerList = configDAL.getallemployee(EmptyEmployeeID);
                    }
                }
                _OrgStructure.Old_Manager = empID;
                _OrgStructure.ResourcePoolID = ResourcePoolID;
                _OrgStructure.BusinessGroupID = BusinessGroupID;
                return PartialView("_AddDeliveryUnitManager", _OrgStructure);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddDeliveryUnitManager(OrganizationStructure model)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (model.Manager != null)
                {
                    success = configDAL.SaveDeliveryUnitManagers(model);
                    if (success)
                        result = "Saved";
                    else
                        result = "Error";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = "ErrorInSave";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteMiddleLevelResourcesDeliveryUnit(List<int> collection, int ResourcePoolID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    success = configDAL.DeleteMiddleLevelResourceForDeliveryUnit(collection, ResourcePoolID);
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
        public ActionResult SelectResourcesForDeliveryUnits(string _EmployeeID, int BusinessGroupID, int ResourcePoolID)
        {
            try
            {
                OrganizationStructure _OrgStructure = new OrganizationStructure();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                _OrgStructure.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                _OrgStructure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                EmployeeDAL dal = new EmployeeDAL();
                _OrgStructure.SearchedUserDetails.EmployeeId = dal.GetEmployeeID(Membership.GetUser().UserName);
                _OrgStructure.BusinessGroupID = BusinessGroupID;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                int[] collection = new int[1000];
                if (_EmployeeID != "")
                {
                    string EmployeeIDWithcomma = _EmployeeID.TrimEnd(',');
                    string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                    collection = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                }
                else
                {
                    collection = new int[0];
                }
                List<MiddleLevelResources> selectResources = configDAL.selectNewResouce(collection, BusinessGroupID);
                _OrgStructure.ResourcePoolID = ResourcePoolID;
                _OrgStructure.MiddleLevelResourcesList = selectResources;
                _OrgStructure.TotalMiddleLevelResources = selectResources.Count;
                return PartialView("_selectResourceForDeliveryUnit", _OrgStructure);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveNewResouceForMiddleLevelForDeleveryUnit(string EmployeeID, int ResourcePoolID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                int[] collection = new int[1000];
                if (EmployeeID != "")
                {
                    string EmployeeIDWithcomma = EmployeeID.TrimEnd(',');
                    string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                    collection = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                }
                else
                {
                    collection = new int[0];
                }
                success = configDAL.saveMiddleLevelResouceForDeleveryUnits(collection, ResourcePoolID);
                if (success)
                    result = "Saved";
                else
                    result = "Error";
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveDeliveryUnit(OrganizationStructure model)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                success = configDAL.SaveDeliveryUnits(model);
                if (success)
                    result = "Saved";
                else
                    result = "Error";
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteManagerDeliveryUnit(List<int> collection, int ResourcePoolID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    success = configDAL.DeleteDeliveryUnitManager(collection, ResourcePoolID);
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

        [HttpPost]
        public ActionResult AddExistingOU(OrganizationStructure model)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                success = configDAL.saveExistingOrganizationUnits(model);
                if (success)
                    result = "Saved";
                else
                    result = "Error";
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteBusinessGroup(int BusinessGroupID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                success = configDAL.DeleteBusinessGroups(BusinessGroupID);
                if (success)
                    result = "Saved";
                else
                    result = "Error";
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        #endregion MyRegion

        #region Configure Organization Units

        [HttpPost]
        public ActionResult ConfigureOrganizationUnit(int locationId)
        {
            try
            {
                OrganizationStructure ConfigureOrganizationstructure = new OrganizationStructure();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                ConfigureOrganizationstructure.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                ConfigureOrganizationstructure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                string employeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeId = employeeID;
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeCode = employeeCode;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                tbl_PM_Location _pm_location = configDAL.getOrganizationUnitDetails(locationId);
                ConfigureOrganizationstructure.LocationID = _pm_location.LocationID;
                ConfigureOrganizationstructure.Country = Convert.ToString(_pm_location.CountryID);
                ConfigureOrganizationstructure.CountryList = configDAL.getCountryList();
                ConfigureOrganizationstructure.Location = _pm_location.Location;
                ConfigureOrganizationstructure.Active = _pm_location.Active.HasValue ? _pm_location.Active.Value : false;
                ConfigureOrganizationstructure.LocationCode = _pm_location.LocationCode;
                ConfigureOrganizationstructure.ShortCode = _pm_location.ShortCode;
                ConfigureOrganizationstructure.Address = _pm_location.Address;
                ConfigureOrganizationstructure.Address1 = _pm_location.Address1;
                ConfigureOrganizationstructure.City = _pm_location.City;
                ConfigureOrganizationstructure.Zip = _pm_location.Zip;
                ConfigureOrganizationstructure.State = _pm_location.State;
                ConfigureOrganizationstructure.Phone1 = _pm_location.Phone1;
                ConfigureOrganizationstructure.Phone2 = _pm_location.Phone2;
                ConfigureOrganizationstructure.Fax = _pm_location.Fax;
                ConfigureOrganizationstructure.Email = _pm_location.Email;
                ConfigureOrganizationstructure.WorkingHours = _pm_location.WorkingHours;
                ConfigureOrganizationstructure.WorkingDays = _pm_location.WorkingDays;
                List<DocumentCategory> OrganizationUnitDocuments = configDAL.getOrganizationUnitDocuments(locationId);
                ConfigureOrganizationstructure.OrganizationUnitDocumentCategory = OrganizationUnitDocuments;
                ConfigureOrganizationstructure.TotalDocumentCategories = OrganizationUnitDocuments.Count;
                List<DeliveryUnit> OUDeliveryUnitList = configDAL.getOUDeliveryUnits(locationId);
                ConfigureOrganizationstructure.DeliveryUnits = OUDeliveryUnitList;
                ConfigureOrganizationstructure.TotalDeliveryUnits = OUDeliveryUnitList.Count;
                List<ManagerList> OUManagerLists = configDAL.getOUManagers(locationId);
                ConfigureOrganizationstructure.OUManagerList = OUManagerLists;
                ConfigureOrganizationstructure.TotalOUManagers = OUManagerLists.Count;
                List<MiddleLevelResources> OuMiddleLevelResource = configDAL.getOUMiddleLevelResource(locationId);
                ConfigureOrganizationstructure.MiddleLevelResourcesList = OuMiddleLevelResource;
                ConfigureOrganizationstructure.TotalMiddleLevelResources = OuMiddleLevelResource.Count;
                return PartialView("_ConfigureOrganizationUnit", ConfigureOrganizationstructure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult LoadExistingDU(int LocationID)
        {
            try
            {
                OrganizationStructure ConfigureOrganizationstructure = new OrganizationStructure();
                if (LocationID != 0)
                {
                    ConfigurationDAL configDAL = new ConfigurationDAL();
                    List<DeliveryUnit> organizationUnits = configDAL.getDeliveryUnitByLocationId(LocationID);
                    List<ExistingDeliveryUnit> ExistingDeliveryUnits = configDAL.getExistingDU(LocationID, organizationUnits);
                    ConfigureOrganizationstructure.LocationID = LocationID;
                    ConfigureOrganizationstructure.ExistingDeliveryUnits = ExistingDeliveryUnits;
                }
                return PartialView("_AddExistingDU", ConfigureOrganizationstructure);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddExistingDU(OrganizationStructure model)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                success = configDAL.saveExistingDeliveryUnits(model);
                if (success)
                    result = "Saved";
                else
                    result = "Error";
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult LoadExistingDT(int BusinessGroupID, int ResourcePoolID)
        {
            try
            {
                OrganizationStructure ConfigureOrganizationstructure = new OrganizationStructure();
                if (ResourcePoolID != 0)
                {
                    ConfigurationDAL configDAL = new ConfigurationDAL();
                    List<DeliveryTeam> currentDU = configDAL.getCurrentDTs(ResourcePoolID);
                    List<ExistingDeliveryTeam> ExistingDeliveryTeam = configDAL.getExistingDT(ResourcePoolID, currentDU);
                    ConfigureOrganizationstructure.ResourcePoolID = ResourcePoolID;
                    ConfigureOrganizationstructure.BusinessGroupID = BusinessGroupID;
                    ConfigureOrganizationstructure.ExistingDeliveryTeams = ExistingDeliveryTeam;
                }
                return PartialView("_AddExistingDT", ConfigureOrganizationstructure);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddExistingDT(OrganizationStructure model)
        {
            try
            {
                bool success = false;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                success = configDAL.saveExistingDeliveryTeams(model);
                if (success)
                    return Json(new { status = success }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveConfigureOrganizationUnit(OrganizationStructure model)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDal = new ConfigurationDAL();
                if (model.LocationID != 0)
                {
                    success = configDal.SaveOrganizationUnitDetails(model);
                    if (success)
                        result = "Saved";
                    else
                        result = "Error";

                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = "ErrorInSave";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult AddOrganizationUnitDocumentCategory(int categoryId, int locationId, string addedCategoryId)
        {
            try
            {
                OrganizationStructure ConfigureOrganizationstructure = new OrganizationStructure();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                ConfigureOrganizationstructure.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                ConfigureOrganizationstructure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                string employeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeId = employeeID;
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeCode = employeeCode;
                ConfigurationDAL configDAL = new ConfigurationDAL();

                if (categoryId != 0)
                {
                    string CategoryIDWithcomma = addedCategoryId.TrimEnd(',');
                    string[] CategoryIDArray = CategoryIDWithcomma.Split(',');
                    int[] myIntCategoryID = Array.ConvertAll(CategoryIDArray, s => int.Parse(s));
                    int[] NewCategoryID = new int[myIntCategoryID.Count() - 1];
                    tbl_PM_DocumentCategory _pm_document = configDAL.getDocumentCategoryDetails(categoryId);
                    ConfigureOrganizationstructure.ddlCategory = Convert.ToString(_pm_document.CategoryID);
                    int i = 0;
                    foreach (var item in myIntCategoryID)
                    {
                        if (item != categoryId)
                        {
                            NewCategoryID[i] = item;
                            i++;
                        }
                    }
                    ConfigureOrganizationstructure.CategoryList = configDAL.getDocumentCategoryList(NewCategoryID);
                }
                else
                {
                    if (addedCategoryId != "")
                    {
                        string CategoryIDWithcomma = addedCategoryId.TrimEnd(',');
                        string[] CategoryIDArray = CategoryIDWithcomma.Split(',');
                        int[] myIntCategoryID = Array.ConvertAll(CategoryIDArray, s => int.Parse(s));
                        ConfigureOrganizationstructure.CategoryList = configDAL.getDocumentCategoryList(myIntCategoryID);
                    }
                    else
                    {
                        int[] EmptyCategoryID = new int[0];
                        ConfigureOrganizationstructure.CategoryList = configDAL.getDocumentCategoryList(EmptyCategoryID);
                    }
                }

                ConfigureOrganizationstructure.LocationID = locationId;
                ConfigureOrganizationstructure.CategoryID = categoryId;
                return PartialView("_AddOrganizationUnitDocumentCategory", ConfigureOrganizationstructure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddOrganizationUnitDocumentCategory(OrganizationStructure model)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDal = new ConfigurationDAL();
                if (model.LocationID != 0)
                {
                    success = configDal.SaveOrganizationDocumentCategory(model);
                    if (success)
                        result = "Saved";
                    else
                        result = "Error";

                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = "ErrorInSave";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteDocumentCategory(string categoryId, int locationId)
        {
            bool success = false;
            string result = null;
            ConfigurationDAL configDAL = new ConfigurationDAL();
            string categoryIdWithcomma = categoryId.TrimEnd(',');
            string[] categoryIdArray = categoryIdWithcomma.Split(',');
            int[] collection = Array.ConvertAll(categoryIdArray, s => int.Parse(s));
            success = configDAL.deleteDocumentCategories(collection, locationId);
            if (success)
                result = "Deleted";
            else
                result = "Error";
            return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOUDeliveryUnit(int resourcePoolId, int locationId)
        {
            try
            {
                OrganizationStructure ConfigureOrganizationstructure = new OrganizationStructure();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                ConfigureOrganizationstructure.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                ConfigureOrganizationstructure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                string employeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeId = employeeID;
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeCode = employeeCode;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                HRMS_tbl_PM_ResourcePool _resourcePool = configDAL.getExistingOUDeliveryUnits(resourcePoolId);
                if (_resourcePool != null)
                {
                    ConfigureOrganizationstructure.newResourcePoolCode = _resourcePool.ResourcePoolCode;
                    ConfigureOrganizationstructure.newresourcePoolName = _resourcePool.ResourcePoolName;
                }
                ConfigureOrganizationstructure.ResourcePoolID = resourcePoolId;
                ConfigureOrganizationstructure.LocationID = locationId;

                return PartialView("_AddOUDeliveryUnit", ConfigureOrganizationstructure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddOUDeliveryUnit(OrganizationStructure model)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDal = new ConfigurationDAL();
                if (model.LocationID != 0)
                {
                    success = configDal.SaveOUDeliveryUnit(model);
                    if (success)
                        result = "Saved";
                    else
                        result = "Error";

                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = "ErrorInSave";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteOUDeliveryUnit(List<int> collection)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    success = configDAL.DeleteOUDeliveryUnit(collection);
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
        public ActionResult AddOrganizationUnitManager(int employeeId, int locationId, string addedEmployeeID)
        {
            try
            {
                OrganizationStructure ConfigureOrganizationstructure = new OrganizationStructure();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                ConfigureOrganizationstructure.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                ConfigureOrganizationstructure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                string employeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeId = employeeID;
                ConfigureOrganizationstructure.SearchedUserDetails.EmployeeCode = employeeCode;
                ConfigurationDAL configDAL = new ConfigurationDAL();

                if (employeeId != 0)
                {
                    string EmployeeIDWithcomma = addedEmployeeID.TrimEnd(',');
                    string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                    int[] myIntEmployeeID = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                    int[] NewEmployeeID = new int[myIntEmployeeID.Count() - 1];
                    tbl_PM_OUPool_Managers _oupoolManager = configDAL.getManagersDetails(employeeId);
                    ConfigureOrganizationstructure.Manager = Convert.ToString(_oupoolManager.ManagerID);
                    ConfigureOrganizationstructure.IsPrimaryResponsible = _oupoolManager.IsPrimaryResponsible.HasValue ? _oupoolManager.IsPrimaryResponsible.Value : false;
                    int i = 0;
                    foreach (var item in myIntEmployeeID)
                    {
                        if (item != employeeId)
                        {
                            NewEmployeeID[i] = item;
                            i++;
                        }
                    }
                    ConfigureOrganizationstructure.BusinessGroupManagerList = configDAL.getOUMangersList(NewEmployeeID);
                }
                else
                {
                    if (addedEmployeeID != "")
                    {
                        string EmployeeIDWithcomma = addedEmployeeID.TrimEnd(',');
                        string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                        int[] myIntEmployeeID = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                        ConfigureOrganizationstructure.BusinessGroupManagerList = configDAL.getOUMangersList(myIntEmployeeID);
                    }
                    else
                    {
                        int[] EmptyEmployeeID = new int[0];
                        ConfigureOrganizationstructure.BusinessGroupManagerList = configDAL.getOUMangersList(EmptyEmployeeID);
                    }
                }

                ConfigureOrganizationstructure.LocationID = locationId;
                ConfigureOrganizationstructure.OldEmployeeID = employeeId;
                return PartialView("_AddOrganizationUnitManager", ConfigureOrganizationstructure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddOrganizationUnitManager(OrganizationStructure model)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDal = new ConfigurationDAL();
                if (model.LocationID != 0)
                {
                    success = configDal.SaveOrganizationUnitManager(model);
                    if (success)
                        result = "Saved";
                    else
                        result = "Error";

                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = "ErrorInSave";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteOUManagers(string employeeId, int locationId)
        {
            bool success = false;
            string result = null;
            ConfigurationDAL configDAL = new ConfigurationDAL();
            string employeeIdWithcomma = employeeId.TrimEnd(',');
            string[] employeeIdArray = employeeIdWithcomma.Split(',');
            int[] collection = Array.ConvertAll(employeeIdArray, s => int.Parse(s));
            success = configDAL.deleteOUManagers(collection, locationId);
            if (success)
                result = "Deleted";
            else
                result = "Error";
            return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectOUResouces(string EmployeeID, int LocationID)
        {
            OrganizationStructure ConfigureOrganizationstructure = new OrganizationStructure();
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            ConfigureOrganizationstructure.SearchedUserDetails = new SearchedUserDetails();
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            ConfigureOrganizationstructure.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
            PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
            EmployeeDAL dal = new EmployeeDAL();
            int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
            string employeeCode = personalDAL.getEmployeeCode(employeeID);
            ConfigureOrganizationstructure.SearchedUserDetails.EmployeeId = employeeID;
            ConfigureOrganizationstructure.SearchedUserDetails.EmployeeCode = employeeCode;
            ConfigurationDAL configDAL = new ConfigurationDAL();

            int[] collection = new int[1000];
            if (EmployeeID != "")
            {
                string EmployeeIDWithcomma = EmployeeID.TrimEnd(',');
                string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                collection = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
            }
            else
            {
                collection = new int[0];
            }
            List<MiddleLevelResources> selectOUResouces = configDAL.selectNewOUResouce(collection, LocationID);
            ConfigureOrganizationstructure.MiddleLevelResourcesList = selectOUResouces;
            ConfigureOrganizationstructure.TotalMiddleLevelResources = selectOUResouces.Count;
            ConfigureOrganizationstructure.LocationID = LocationID;
            return PartialView("_selectOUResouces", ConfigureOrganizationstructure);
        }

        [HttpPost]
        public ActionResult SaveNewOUResouceForMiddleLevel(string EmployeeID, int LocationID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                string EmployeeIDWithcomma = EmployeeID.TrimEnd(',');
                string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                int[] collection = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                if (LocationID != 0)
                {
                    success = configDAL.saveOUMiddleLevelResouce(collection, LocationID);
                    if (success)
                        result = "Saved";
                    else
                        result = "Error";
                    return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
                }
                else
                    result = "ErrorInSave";
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteOUMiddleLevelResources(List<int> collection, int LocationID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    success = configDAL.DeleteOUMiddleLevelResource(collection, LocationID);
                    result = "Deleted";
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        #endregion Configure Organization Units
    }
}