using HRMS.DAL;
using HRMS.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class SmartTrackController : Controller
    {
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();
        private PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();

        public ActionResult Index()
        {
            try
            {
                ConfigurationViewModel smartTrack = new ConfigurationViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                smartTrack.SearchedUserDetails = new SearchedUserDetails();
                ViewBag.AsciiKey = Session["SecurityKey"].ToString();

                string employeeCode = Membership.GetUser().UserName;
                int employeeId = employeeDAL.GetEmployeeID(employeeCode);
                string[] role = Roles.GetRolesForUser(employeeCode);

                if (employeeCode != null)
                {
                    smartTrack.SearchedUserDetails.EmployeeId = employeeId;
                    smartTrack.SearchedUserDetails.EmployeeCode = employeeCode;
                    smartTrack.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }
                return View(smartTrack);
            }
            catch
            {
                throw;
            }
        }
    }
}