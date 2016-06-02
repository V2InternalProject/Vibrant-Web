using HRMS.DAL;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class AppraisalSectionController : Controller
    {
        private HRMSPageLevelAccess objpagelevel = new HRMSPageLevelAccess();

        //
        // GET: /AppraisalSection/
        private EmployeeDAL employeeDAL = new EmployeeDAL();

        public ActionResult AppraisalMainForm()
        {
            return View();
        }

        public ActionResult AppraisalEmployee()
        {
            string PageName = "some";
            objpagelevel.PageLevelAccess(PageName);
            return View();
        }

        [HttpGet]
        public int GetLogEmployeeId()
        {
            string employeeCode = Membership.GetUser().UserName;
            int EmployeeCode = Convert.ToInt16(employeeCode);
            int employeeId = employeeDAL.GetEmployeeID(employeeCode);
            return (EmployeeCode);
        }
    }
}