using HRMS.DAL;
using HRMS.Models;
using System;
using System.Web.Mvc;

namespace HRMS.Controllers
{
    public class EmployeeNodeController : Controller
    {
        //
        // GET: /EmployeeNode/

        public ActionResult Index()
        {
            try
            {
                Session["SearchEmpFullName"] = null;  // to hide emp search
                Session["SearchEmpCode"] = null;
                Session["SearchEmpID"] = null;

                ConfigurationDAL configDAL = new ConfigurationDAL();
                EmployeeNodeViewModel model = new EmployeeNodeViewModel();
                model.NodesList = configDAL.GetNodeList();
                model.EmployeeList = configDAL.GetAllActiveEmployeesList();
                model.EmployeeNodeMappingList = configDAL.GetEmployeeNodeMapping();
                model.SearchedUserDetails = new SearchedUserDetails();
                return View("Index", model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult UpdateEmployeeNodeMapping(string[] allEmployeeNodeList)
        {
            try
            {
                bool status = false;
                string resultMessage = string.Empty;
                if (allEmployeeNodeList != null)
                {
                    ConfigurationDAL configDAL = new ConfigurationDAL();
                    status = configDAL.UpdateEmployeeNodeMapping(allEmployeeNodeList);
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