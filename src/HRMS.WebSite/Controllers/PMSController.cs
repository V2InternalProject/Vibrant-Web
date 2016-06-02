using HRMS.DAL;
using HRMS.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class PMSController : Controller
    {
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();

        public ActionResult Index()
        {
            try
            {
                Session["SearchEmpFullName"] = null;  // to hide emp search
                Session["SearchEmpCode"] = null;
                Session["SearchEmpID"] = null;

                ConfigurationViewModel pms = new ConfigurationViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                pms.SearchedUserDetails = new SearchedUserDetails();
                ViewBag.AsciiKey = Session["SecurityKey"].ToString();

                string employeeCode = Membership.GetUser().UserName;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                string[] role = Roles.GetRolesForUser(employeeCode);

                if (employeeCode != null)
                {
                    pms.SearchedUserDetails.EmployeeId = employeeId;
                    pms.SearchedUserDetails.EmployeeCode = employeeCode;
                    pms.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }
                return View(pms);
            }
            catch
            {
                throw;
            }
        }
    }
}