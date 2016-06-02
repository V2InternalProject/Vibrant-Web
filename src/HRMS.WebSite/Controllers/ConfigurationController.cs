using HRMS.DAL;
using HRMS.Models;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class ConfigurationController : Controller
    {
        //
        // GET: /Configuration/

        public ActionResult Index()
        {
            try
            {
                ConfigurationViewModel configviewmodel = new ConfigurationViewModel();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                configviewmodel.SearchedUserDetails = new SearchedUserDetails();
                ViewBag.AsciiKey = Session["SecurityKey"].ToString();
                string employeeCode = Membership.GetUser().UserName;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                string[] role = Roles.GetRolesForUser(employeeCode);

                if (employeeCode != null)
                {
                    configviewmodel.SearchedUserDetails.EmployeeId = employeeId;
                    configviewmodel.SearchedUserDetails.EmployeeCode = employeeCode;
                    configviewmodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }

                return View(configviewmodel);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}