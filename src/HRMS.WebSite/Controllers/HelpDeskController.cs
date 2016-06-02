using HRMS.DAL;
using HRMS.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class HelpDeskController : Controller
    {
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();

        public ActionResult Index()
        {
            try
            {
                ConfigurationViewModel helpdesk = new ConfigurationViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                helpdesk.SearchedUserDetails = new SearchedUserDetails();
                ViewBag.AsciiKey = Session["SecurityKey"].ToString();

                string employeeCode = Membership.GetUser().UserName;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                string[] role = Roles.GetRolesForUser(employeeCode);

                if (employeeCode != null)
                {
                    helpdesk.SearchedUserDetails.EmployeeId = employeeId;
                    helpdesk.SearchedUserDetails.EmployeeCode = employeeCode;
                    helpdesk.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }
                return View(helpdesk);
            }
            catch
            {
                throw;
            }
        }
    }
}