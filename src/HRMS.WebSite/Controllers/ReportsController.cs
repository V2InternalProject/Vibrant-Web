using HRMS.DAL;
using HRMS.Models;
using MvcApplication3.Filters;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class ReportsController : Controller
    {
        private EmployeeDAL employeeDAL = new EmployeeDAL();
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();
        private ReportDAL reportDAL = new ReportDAL();

        [PageAccess(PageName = "Report")]
        public ActionResult Index()
        {
            ReportModel reportModel = new ReportModel();
            List<Report> reportList = new List<Report>();
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            string EmpID = dal.GetEmployeeIdFromEmployeeCodeSEM(Convert.ToInt32(Membership.GetUser().UserName));
            int employeeID = Convert.ToInt32(EmpID);
            reportList = reportDAL.GetReportList(employeeID);
            reportModel.reportList = reportList;
            return View(reportModel);
        }

        [HttpGet]
        public ActionResult GetSelectedReportData(string reportID)
        {
            TaskTimesheetDAL dal = new TaskTimesheetDAL();
            string EmpID = dal.GetEmployeeIdFromEmployeeCodeSEM(Convert.ToInt32(Membership.GetUser().UserName));
            int employeeID = Convert.ToInt32(EmpID);
            try
            {
                return Json(reportDAL.GetReportData(Convert.ToInt32(reportID), employeeID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { });
            }
        }

        [HttpPost]
        public ActionResult SaveFormData(List<ControlsList> Controls)
        {
            try
            {
                Tuple<bool, Guid> isUpdated = reportDAL.SaveFormData(Controls);
                return Json(new { IsUpdated = isUpdated.Item1, newGuid = isUpdated.Item2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { });
            }
        }
    }
}