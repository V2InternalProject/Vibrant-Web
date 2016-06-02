using HRMS.DAL;
using HRMS.Models;
using System;
using System.Web.Mvc;

namespace HRMS.Controllers
{
    public class AccessRightsController : Controller
    {
        //
        // GET: /AccessRights/

        public ActionResult Index()
        {
            try
            {
                Session["SearchEmpFullName"] = null;  // to hide emp search
                Session["SearchEmpCode"] = null;
                Session["SearchEmpID"] = null;

                ConfigurationDAL configDAL = new ConfigurationDAL();
                AccessRightsNodeDetails model = new AccessRightsNodeDetails();
                model.AccessRightsList = configDAL.GetAccessRightsNodeMapping();
                model.NodesList = configDAL.GetNodeList();
                model.AccessList = configDAL.GetAccessRightsList();
                model.SearchedUserDetails = new SearchedUserDetails();
                return View("Index", model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult UpdateRoleNodeMapping(string[] allRoleNodeList)
        {
            try
            {
                bool status = false;
                string resultMessage = string.Empty;
                if (allRoleNodeList != null)
                {
                    ConfigurationDAL configDAL = new ConfigurationDAL();
                    AccessRightsNodeDetails model = new AccessRightsNodeDetails();
                    status = configDAL.UpdateRoleNodeMapping(allRoleNodeList);
                    if (status)
                        resultMessage = "Saved";
                    else
                        resultMessage = "Error";
                }
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}