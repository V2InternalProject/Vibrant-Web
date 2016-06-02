using HRMS.DAL;
using HRMS.Models;
using log4net;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        private ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index(string errorCode)
        {
            try
            {
                ErrorViewModel errormodel = new ErrorViewModel();
                errormodel.SearchedUserDetails = new SearchedUserDetails();
                string employeeCode = Membership.GetUser().UserName;
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(employeeCode);
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                errormodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                if (employeeCode != null)
                {
                    CommonMethodsDAL common = new CommonMethodsDAL();
                    errormodel.SearchedUserDetails.EmployeeId = employeeID;
                    errormodel.SearchedUserDetails.EmployeeCode = employeeCode;
                }
                ViewBag.ErrorCode = errorCode;
                log.Error(errorCode);
                return View(errormodel);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString(), ex);
                return RedirectToAction("LogOn", "Account");
            }
        }

        public ActionResult ErrorPage()
        {
            try
            {
                return View("ErrorMessage");
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString(), ex);
                return RedirectToAction("LogOn", "Account");
            }
        }

        public ActionResult JavascriptNotSupported()
        {
            return View();
        }
    }
}