using HRMS.DAL;
using Newtonsoft.Json;
using System;
using System.Web.Security;

namespace HRMS
{
    public class Executer
    {
        public static string GetExpenseIds(string param)
        {
            try
            {
                ReportDAL dal = new ReportDAL();
                WSEMDBEntities dbContext = new WSEMDBEntities();
                return JsonConvert.SerializeObject(dal.GetDropDownDataIfAvailable(dbContext, "ExpenseList", int.Parse(param), null, null, null));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetEmployeeIds(string param)
        {
            try
            {
                ReportDAL dal = new ReportDAL();
                WSEMDBEntities dbContext = new WSEMDBEntities();
                return JsonConvert.SerializeObject(dal.GetDropDownDataIfAvailable(dbContext, "EmployeeListByProjectID", null, param, null, null));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetEmployeeIdsForTimeSheet(string param)
        {
            try
            {
                ReportDAL dal = new ReportDAL();
                WSEMDBEntities dbContext = new WSEMDBEntities();
                //string Employeecode
                string Employeecode = Membership.GetUser().UserName;
                TaskTimesheetDAL Timesheetdal = new TaskTimesheetDAL();
                int? employeid = Convert.ToInt32(Timesheetdal.GetEmployeeIdFromEmployeeCodeSEM(Convert.ToInt32(Employeecode)));
                return JsonConvert.SerializeObject(dal.GetDropDownDataIfAvailable(dbContext, "EmployeeListByProjectIDForTimeSheet", employeid, param, null, null));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetEmployeeCodes(string param, string ControlName, int? Reportid)
        {
            try
            {
                ReportDAL dal = new ReportDAL();
                WSEMDBEntities dbContext = new WSEMDBEntities();
                string Employeecode = Membership.GetUser().UserName;
                TaskTimesheetDAL Timesheetdal = new TaskTimesheetDAL();
                int? employeid = Convert.ToInt32(Timesheetdal.GetEmployeeIdFromEmployeeCodeSEM(Convert.ToInt32(Employeecode)));
                return JsonConvert.SerializeObject(dal.GetDropDownDataIfAvailable(dbContext, "EmployeeCodeListByProjectID", employeid, param, Reportid, ControlName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetEmployeeIdsForPlannedTask(string param, string ControlName, int? Reportid)
        {
            try
            {
                ReportDAL dal = new ReportDAL();
                WSEMDBEntities dbContext = new WSEMDBEntities();
                string Employeecode = Membership.GetUser().UserName;
                TaskTimesheetDAL Timesheetdal = new TaskTimesheetDAL();
                int? employeid = Convert.ToInt32(Timesheetdal.GetEmployeeIdFromEmployeeCodeSEM(Convert.ToInt32(Employeecode)));
                return JsonConvert.SerializeObject(dal.GetDropDownDataIfAvailable(dbContext, "EmployeeListByProjectIDForPlannedTask", employeid, param, Reportid, ControlName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}