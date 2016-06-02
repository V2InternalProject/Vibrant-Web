using HRMS.DAL;
using HRMS.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class HelpDeskAdminController : Controller
    {
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();

        public ActionResult Index()
        {
            try
            {
                ConfigurationViewModel helpdeskAdmin = new ConfigurationViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                helpdeskAdmin.SearchedUserDetails = new SearchedUserDetails();
                ViewBag.AsciiKey = Session["SecurityKey"].ToString();

                string employeeCode = Membership.GetUser().UserName;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                string[] role = Roles.GetRolesForUser(employeeCode);

                if (employeeCode != null)
                {
                    helpdeskAdmin.SearchedUserDetails.EmployeeId = employeeId;
                    helpdeskAdmin.SearchedUserDetails.EmployeeCode = employeeCode;
                    helpdeskAdmin.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }
                return View(helpdeskAdmin);
            }
            catch
            {
                throw;
            }
        }
    }
}