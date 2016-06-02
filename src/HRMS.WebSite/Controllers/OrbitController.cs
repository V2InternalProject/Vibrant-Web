using HRMS.DAL;
using HRMS.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class OrbitController : Controller
    {
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();

        public ActionResult Index()
        {
            try
            {
                ConfigurationViewModel orbit = new ConfigurationViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                orbit.SearchedUserDetails = new SearchedUserDetails();
                ViewBag.AsciiKey = Session["SecurityKey"].ToString();

                string employeeCode = Membership.GetUser().UserName;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                string[] role = Roles.GetRolesForUser(employeeCode);

                if (employeeCode != null)
                {
                    orbit.SearchedUserDetails.EmployeeId = employeeId;
                    SemDAL semdal = new SemDAL();
                    int employeeid = semdal.geteEmployeeIDFromSEMDatabase(employeeCode);
                    orbit.SearchedUserDetails.IsProjectReviewer = semdal.CheckIfEmployeeisReviewer(employeeid);
                    orbit.SearchedUserDetails.EmployeeCode = employeeCode;
                    orbit.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }
                return View(orbit);
            }
            catch
            {
                throw;
            }
        }
    }
}