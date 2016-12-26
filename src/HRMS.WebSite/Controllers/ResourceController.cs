using HRMS.DAL;
using HRMS.Models;
using MvcApplication3.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS.Controllers
{
    public class ResourceController : Controller
    {
        //
        // GET: /Resourse/
        [HttpGet]
        public ActionResult AddEditResourse(string HelpdeskTicketIDs, string ProjectIds, string ProjectRoles, string WorkHourss, string FromDates, string ToDates, string Mode, string ReportedByIds)
        {
            //int HelpdeskTicketID, int? ProjectId, string ProjectRole, int? WorkHours, string FromDate, string ToDate, string Mode, int? ReportedById
            CommonMethodsDAL dalc = new CommonMethodsDAL();
            var decryptedHelpdeskTicketIDs = dalc.Decrypt(HelpdeskTicketIDs.Replace("\"", ""), true);
            var decryptedProjectIds = dalc.Decrypt(ProjectIds.Replace("\"", ""), true);

            var decryptedProjectRoles = dalc.Decrypt(ProjectRoles.Replace("\"", ""), true);
            var decryptedWorkHourss = dalc.Decrypt(WorkHourss.Replace("\"", ""), true);

            var decryptedFromDates = dalc.Decrypt(FromDates.Replace("\"", ""), true);
            var decryptedToDates = dalc.Decrypt(ToDates.Replace("\"", ""), true);

            var decryptedReportedByIds = dalc.Decrypt(ReportedByIds.Replace("\"", ""), true);
            int HelpdeskTicketID = int.Parse(decryptedHelpdeskTicketIDs);
            int? ProjectId = int.Parse(decryptedProjectIds);
            int? WorkHours = int.Parse(decryptedWorkHourss);
            int? ReportedById = int.Parse(decryptedReportedByIds);
            AddEdirResourseModel model = new AddEdirResourseModel();
            SemDAL dal = new SemDAL();
            model.SearchedUserDetails = new SearchedUserDetails();
            model.EmployeeList = dal.GetEmployeeListForReportingTo(ProjectId);
            model.EmployeeRole = dal.getEmployeeRoles();
            model.ResourseTypes = dal.GetResourceTypes();
            model.ProjectID = ProjectId;
            model.RequesterId = ReportedById;
            tbl_PM_Project projectDetails = dal.GetProjectDetails(ProjectId);
            if (projectDetails != null)
            {
                model.ProjectID = projectDetails.ProjectID;
                model.ProjectStartDate = projectDetails.ActualStartDate;
                model.ProjectName = projectDetails.ProjectName;
                if (projectDetails.ActualEndDate != null)
                    model.ProjectEndDate = projectDetails.ActualEndDate.Value;
                else
                    model.ProjectEndDate = projectDetails.ExpectedEndDate;
            }
            model.ButtonClick = Mode;
            if (model.ButtonClick == "UpdateAllocation")
            {
                model.ProjectRole = decryptedProjectRoles;
                model.AllocationStartDate = Convert.ToDateTime(decryptedFromDates);
                model.AllocationEndDate = Convert.ToDateTime(decryptedToDates);
                model.AllocatedPercentage = double.Parse(WorkHours.ToString());
            }
            model.HelpdeskTicketID = HelpdeskTicketID;
            //DataSet dsResourceDetails = dal.GetHelpDeskDetailsFromHelpDeskIssueID(HelpdeskTicketID);
            //var values = new List<Tuple<DateTime, DateTime>>();
            //foreach (DataTable t in dsResourceDetails.Tables)
            //{
            //    foreach (DataRow row in t.Rows)
            //    {
            //        string StartDate = row["FromDate"].ToString();
            //        string EndDate = row["ToDate"].ToString();
            //        values.Add(new Tuple<DateTime, DateTime>(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)));
            //    }
            //}
            //for (int i = 0; i < values.Count; i++)
            //{
            //    model.ProjectStartDate = values[i].Item1;
            //    model.ProjectEndDate = values[i].Item1;
            //}

            return View("AddEditResourceDetails", model);
        }

        [PageAccess(PageName = "My Allocations")]
        public ActionResult ResourceIndex()
        {
            Session["SearchEmpFullName"] = null;  // to hide emp search
            Session["SearchEmpCode"] = null;
            Session["SearchEmpID"] = null;
            ResourceIndexModel model = new ResourceIndexModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            string employeeCode = Membership.GetUser().UserName;
            int EmployeeCode = Convert.ToInt32(employeeCode);
            ViewBag.LogInUserId = EmployeeCode;
            EmployeeDAL employeeDAL = new EmployeeDAL();
            //Session["ResourceNodeLevelAccess"] = employeeDAL.GetPageLevelAccessForEmployee(Convert.ToInt32(EmpCode), Convert.ToInt32(PageID));
            return View(model);
        }

        public ActionResult SearchResource()
        {
            SearchResourceModel model = new SearchResourceModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult ResourceAllocationDashboard()
        {
            ResourceAllocationDashboardModel model = new ResourceAllocationDashboardModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        public ActionResult ReportView()
        {
            ResourceAllocationDashboardModel model = new ResourceAllocationDashboardModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            //return View(model);

            //string AYearID = "1";
            var reportParameters = new Dictionary<string, string>();

            //if (form["Category"] == 1)
            //  form["Category"] = "%";

            string UserID = "61";

            string LoginType = "E";

            string UserName = "Admin";

            string LoginId = "";

            string FinancialPeriod = "";

            string monthPara = "";

            string yearPara = "";

            string businessGroupID = "";

            string LocationID = "";

            string RoleId = "";

            string GradeId = "";

            string EmployeeName = "";

            string Deployable = "";

            string AllocationPercentage = "";

            reportParameters.Add("UserID", UserID);
            reportParameters.Add("LoginType", LoginType);
            reportParameters.Add("UserName", UserName);
            reportParameters.Add("LoginId", LoginId);
            reportParameters.Add("FinancialPeriod", FinancialPeriod);
            reportParameters.Add("monthPara", monthPara);
            reportParameters.Add("yearPara", yearPara);
            reportParameters.Add("businessGroupID", businessGroupID);
            reportParameters.Add("LocationID", LocationID);
            reportParameters.Add("RoleId", RoleId);
            reportParameters.Add("GradeId", GradeId);
            reportParameters.Add("EmployeeName", EmployeeName);
            reportParameters.Add("Deployable", Deployable);
            reportParameters.Add("AllocationPercentage", AllocationPercentage);
            Session["reportParameters"] = reportParameters;
            Session["reportPath"] = "/ResourceAllocation/ResourceAllocationView";
            return Redirect("../Reports/ResourceAllocationView.aspx");
            //return PartialView("ReportView",model);
        }

        public ActionResult UtilizationReport()
        {
            UtilizationReportModel model = new UtilizationReportModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        [PageAccess(PageName = "Manage Resource")]
        public ActionResult RMGViewPost()
        {
            RMGViewPostModel model = new RMGViewPostModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            SemDAL Dal = new SemDAL();
            EmployeeDAL employeeDAL = new EmployeeDAL();
            string employeeCode = Membership.GetUser().UserName;
            string[] role = Roles.GetRolesForUser(employeeCode);
            model.AsOnDate = DateTime.Now.Date;
            //Session["ResourceNodeLevelAccess"] = employeeDAL.GetPageLevelAccessForEmployee(Convert.ToInt32(EmpCode), Convert.ToInt32(PageID));

            int employeeId = Dal.geteEmployeeIDFromSEMDatabase(employeeCode);
            ViewBag.loginUserId = employeeId;
            string roleToPass = null;
            string userName = employeeId.ToString();
            int employeeCodeEmp = Convert.ToInt32(employeeCode);
            roleToPass = Dal.GetMaxRoleForUser(employeeCode);
            string empRole = string.Empty;

            int ProjectStatus = 2;
            model.ProjectApprovedListdata = Dal.GetResourceAllocationProjectList(ProjectStatus, employeeCodeEmp, "Admin", empRole);
            model.ProjectRolesList = Dal.getEmployeeRoles();

            //  ViewBag.IsProjectManager = role.Contains("Manager");
            ConfigurationViewModel orbit = new ConfigurationViewModel();
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            orbit.SearchedUserDetails = new SearchedUserDetails();
            orbit.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
            model.resourcePoolList = Dal.GetResourcePoolList();

            //if (orbit.SearchedUserDetails.UserRole == "RMG")
            //{
            //    ViewBag.Heading = "RMG View Post Allocation";
            //}
            //if (orbit.SearchedUserDetails.UserRole == "Manager")
            //{
            //    ViewBag.Heading = "Manager View Post Allocation";
            //}

            return View(model);
        }

        public ActionResult ManagerViewPost()
        {
            ManagerViewPostModel model = new ManagerViewPostModel();
            model.SearchedUserDetails = new SearchedUserDetails();
            return View(model);
        }

        [HttpPost]
        public ActionResult RMGCurrentResourceLoadGrid(string EmployeeId, int projectID, string searchText, DateTime? AsOnDate, int page, int rows, string GridName, int? ResourcePoolId, int? EmployeeForProject)
        {
            try
            {
                int SemEmployeeId = 0;
                SEMViewModel model = new SEMViewModel();
                SemDAL dal = new SemDAL();
                if (EmployeeId != null && EmployeeId != "0")
                {
                    int EmpID = Convert.ToInt32(EmployeeId);
                    SemEmployeeId = dal.GetSemEmployeeId(EmpID);
                    model.SearchedUserDetails = new SearchedUserDetails();
                    model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                }

                int totalCount;
                List<RMGViewPostModel> expenseDetails = new List<RMGViewPostModel>();
                expenseDetails = dal.GetResouceAllocGridsDetails(SemEmployeeId, Convert.ToInt32(projectID), searchText, AsOnDate, page, rows, out totalCount, GridName, ResourcePoolId, EmployeeForProject);
                if ((expenseDetails == null || expenseDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    expenseDetails = dal.GetResouceAllocGridsDetails(SemEmployeeId, Convert.ToInt32(projectID), searchText, AsOnDate, page, rows, out totalCount, GridName, ResourcePoolId, EmployeeForProject);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = expenseDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult GetResourceListForProject(int projectID)
        {
            try
            {
                SemDAL dal = new SemDAL();
                List<EmployeeListDetails> EmployeeList = new List<EmployeeListDetails>();
                var employees = dal.GetEmployeeListAllocatedToProject(projectID);
                ViewBag.ProjectResourceList = employees;
                return Json(employees);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult GetResourceListForProjectTypeAhead(string term)
        {
            try
            {
                SemDAL dal = new SemDAL();
                List<EmployeeListDetails> EmployeeList = new List<EmployeeListDetails>();
                var employees = dal.GetEmployeeListAllocatedToProject1(term, 1, 20);
                // ViewBag.ProjectResourceList = employees;
                return Json(employees, JsonRequestBehavior.AllowGet);
                //return Json(employees);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult AddEditResourse(AddEdirResourseModel model)
        {
            try
            {
                string resultMessage = string.Empty;
                bool finalStatus;
                string HelpDeskTicketID = string.Empty;
                SaveAddEditResources status = new SaveAddEditResources();
                SemDAL dal = new SemDAL();
                status = dal.SaveAddEditResource(model);
                if (status.isAllocationDone == true && status.CanAllocationDone == true)
                {
                    resultMessage = status.ErrorMessage;
                    finalStatus = true;
                }
                else
                {
                    resultMessage = status.ErrorMessage;
                    finalStatus = false;
                }
                return Json(new { results = resultMessage, status = finalStatus, HelpDeskTicketID = model.HelpdeskTicketID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult ReallocationAndBulkReallocation(string HelpdeskTicketIDS, string ProjectIdS, string ReportedByIdS)
        {
            try
            {
                CommonMethodsDAL dalc = new CommonMethodsDAL();
                var decryptedHelpdeskTicketIDS = dalc.Decrypt(HelpdeskTicketIDS.Replace("\"", ""), true);
                var decryptedProjectIdS = dalc.Decrypt(ProjectIdS.Replace("\"", ""), true);
                var decryptedReportedByIdS = dalc.Decrypt(ReportedByIdS.Replace("\"", ""), true);
                int HelpdeskTicketID = int.Parse(decryptedHelpdeskTicketIDS);
                int? ProjectId = int.Parse(decryptedProjectIdS);
                int? ReportedById = int.Parse(decryptedReportedByIdS);
                AddEdirResourseModel model = new AddEdirResourseModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                string employeeCode = Membership.GetUser().UserName;
                SemDAL dal = new SemDAL();
                //model.ProjectID = 395;
                int employeeId = dal.geteEmployeeIDFromSEMDatabase(employeeCode);
                ViewBag.loginUserId = employeeId;
                model.ProjectID = ProjectId;
                model.RequesterId = ReportedById;
                tbl_PM_Project projectDetails = dal.GetProjectDetails(ProjectId);
                if (projectDetails != null)
                {
                    model.ProjectStartDate = projectDetails.ActualStartDate;
                    model.ProjectEndDate = projectDetails.ActualEndDate;
                    model.ProjectName = projectDetails.ProjectName;
                }
                model.HelpdeskTicketID = HelpdeskTicketID;
                return View("ReallocationAndBulkReallocation", model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public void ExportToExcelOfBulkAllocation(int? ProjectID)
        {
            SemDAL dal = new SemDAL();
            DataSet dsCurrentDetails = new DataSet();
            DataSet dsHistoryDetails = new DataSet();
            int ProID = Convert.ToInt32(ProjectID);
            List<AddEdirResourseModel> CurrentDetails = dal.LoadReallocationDetails(ProID);

            DataTable dt = new DataTable();
            dt.Columns.Add("Project Code", typeof(string));
            dt.Columns.Add("Project Name", typeof(string));
            dt.Columns.Add("EmployeeID", typeof(string));
            dt.Columns.Add("EmployeeName", typeof(string));
            dt.Columns.Add("Skills", typeof(string));
            dt.Columns.Add("Designation Name", typeof(string));
            dt.Columns.Add("Resource Type", typeof(string));
            dt.Columns.Add("Employment Status", typeof(string));
            dt.Columns.Add("Start Date", typeof(string));
            dt.Columns.Add("End Date", typeof(string));
            dt.Columns.Add("Allocation Percentage", typeof(string));
            dt.Columns.Add("Project Start Date", typeof(string));
            dt.Columns.Add("Project End Date", typeof(string));
            dt.Columns.Add("Comments", typeof(string));

            if (CurrentDetails != null)
            {
                foreach (var item in CurrentDetails)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = item.ProjectCode;
                    dr[1] = item.ProjectName;
                    dr[2] = item.EmployeeCode;
                    dr[3] = item.EmployeeName;
                    dr[4] = item.Skills;
                    dr[5] = item.DesignationName;
                    dr[6] = item.ResourceType;
                    dr[7] = item.EmployementStatus;

                    var StartDate = Convert.ToDateTime(item.AllocationStartDate);
                    var AllocationStartDate = StartDate.ToString("MM/dd/yyyy");
                    dr[8] = AllocationStartDate;

                    var EndDate = Convert.ToDateTime(item.AllocationEndDate);
                    var AllocationEndDate = EndDate.ToString("MM/dd/yyyy");
                    dr[9] = AllocationEndDate;

                    dr[10] = item.AllocatedPercentage;

                    var PsDate = Convert.ToDateTime(item.ProjectStartDate);
                    var ProjectStartDate = PsDate.ToString("MM/dd/yyyy");
                    dr[11] = ProjectStartDate;

                    var PEDate = Convert.ToDateTime(item.ProjectEndDate);
                    var ProjectEndDate = PEDate.ToString("MM/dd/yyyy");
                    dr[12] = ProjectEndDate;

                    dr[13] = item.Comments;
                    dt.Rows.Add(dr);
                }
                dsCurrentDetails.Tables.Add(dt);
                bindGrid.DataSource = dsCurrentDetails;
                bindGrid.DataBind();
            }
            string ProjectName = string.Empty;
            foreach (var item in CurrentDetails)
            {
                if (item.ProjectName != null)
                {
                    ProjectName = item.ProjectName;
                    break;
                }
            }

            string strFileName = "V2_RA_" + ProjectName.ToString().Replace("/", "-");
            strFileName = strFileName + ".xls";
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename = " + strFileName);
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            bindGrid.SetRenderMethodDelegate(new RenderMethod(RenderTitleCurrent));
            bindGrid.RenderControl(oHtmlTextWriter);
            // Export to Excel grid title
            bindGrid1.SetRenderMethodDelegate(new RenderMethod(RenderTitleHistory));
            bindGrid1.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());
            Response.End();
        }

        [HttpPost]
        public ActionResult LoadReallocationGrid(int? ProjectID, int page, int rows)
        {
            try
            {
                if (ProjectID == null)
                    ProjectID = 0;
                AddEdirResourseModel model = new AddEdirResourseModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<AddEdirResourseModel> ResourceListDetails = new List<AddEdirResourseModel>();
                ResourceListDetails = dal.LoadReallocationeGridFromProjectID(ProjectID, page, rows, out totalCount);

                if ((ResourceListDetails == null || ResourceListDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    ResourceListDetails = dal.LoadReallocationeGridFromProjectID(ProjectID, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = ResourceListDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult CurrentAllocationLoadGrid(int EmployeeId, int page, int rows)
        {
            try
            {
                AddEdirResourseModel model = new AddEdirResourseModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<AddEdirResourseModel> ResourceListDetails = new List<AddEdirResourseModel>();
                ResourceListDetails = dal.LoadCurrentReallocationeGridFromEmployeeID(EmployeeId, page, rows, out totalCount);

                if ((ResourceListDetails == null || ResourceListDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    ResourceListDetails = dal.LoadCurrentReallocationeGridFromEmployeeID(EmployeeId, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = ResourceListDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult AllocationHistoryLoadGrid(int EmployeeId, int page, int rows)
        {
            try
            {
                AddEdirResourseModel model = new AddEdirResourseModel();
                SemDAL dal = new SemDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                model.SearchedUserDetails.EmployeeId = HRMS.Helper.HRMSHelper.LoggedInEmployeeId();
                int totalCount;
                List<AddEdirResourseModel> ResourceListDetails = new List<AddEdirResourseModel>();
                ResourceListDetails = dal.LoadCurrentReallocationeHistoryGridFromEmployeeID(EmployeeId, page, rows, out totalCount);

                if ((ResourceListDetails == null || ResourceListDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    ResourceListDetails = dal.LoadCurrentReallocationeGridFromEmployeeID(EmployeeId, page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = ResourceListDetails,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult SaveReallocationForResource(AddEdirResourseModel model, string OldDate)
        {
            try
            {
                bool uploadStatus = false;
                string resultMessage = string.Empty;
                string EmployeeID = string.Empty;
                bool status = false;
                DateTime date = Convert.ToDateTime(OldDate);
                model.AllocationOldEndDate = date;
                SemDAL dal = new SemDAL();
                status = dal.UpdateReallocationOfResource(model);

                if (status == true)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                EmployeeID = Convert.ToString(model.EmployeeId);
                return Json(new { results = resultMessage, status = status, EmployeeID = EmployeeID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult SaveBulkReallocation(List<AddEdirResourseModel> model)
        {
            try
            {
                bool uploadStatus = false;
                string resultRedMessage = string.Empty;
                string resultGreenMessage = string.Empty;
                SemDAL dal = new SemDAL();
                List<int> List = dal.SaveBulkAllocationRecords(model);

                foreach (var item in model)
                {
                    resultGreenMessage = resultGreenMessage + "," + item.EmployeeId;
                }

                if (List.Count > 0)
                {
                    foreach (var item in model)
                    {
                        foreach (var employee in List)
                        {
                            if (item.EmployeeId == employee)
                            {
                                resultRedMessage = resultRedMessage + "," + item.EmployeeId;
                            }
                        }
                    }
                }

                if (resultRedMessage != string.Empty)
                {
                    string[] redIds = resultRedMessage.Split(',');
                    for (int i = 0; i < redIds.Length; i++)
                    {
                        if (redIds[i].Length > 0)
                        {
                            string ids = "," + redIds[i] + ",";

                            resultGreenMessage = "," + resultGreenMessage + ",";
                            resultGreenMessage = resultGreenMessage.Replace(ids, ", ,");
                        }
                    }
                }

                string[] greenIds = resultGreenMessage.Split(',');
                string newGreenMessage = string.Empty;

                for (int i = 0; i < greenIds.Length; i++)
                {
                    if (greenIds[i].Length > 0)
                    {
                        int n;
                        bool isNumeric = int.TryParse(greenIds[i], out n);
                        if (isNumeric) newGreenMessage = newGreenMessage + "," + greenIds[i];
                    }
                }

                resultGreenMessage = newGreenMessage;
                resultRedMessage = resultRedMessage.TrimStart(new char[] { ',' });
                resultGreenMessage = resultGreenMessage.TrimStart(new char[] { ',' });

                return Json(new { resultsRed = resultRedMessage, resultsGreen = resultGreenMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult ResourceSendMail(string btnClick, int? RequesterId, string EmployeeName, int? ReportingTo, DateTime? FromDate, int? projectRole, int? ResorceType, int? AllocatedPercentage, string Comments, int? projectId, int? loggedinEmpID, string EmployeeIDs, DateTime? Todate)
        {
            try
            {
                TravelViewModel model = new TravelViewModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                SemDAL dal = new SemDAL();
                model.Mail = new TravelMailTemplate();

                tbl_PM_Project projectDetails = dal.GetProjectDetails(Convert.ToInt32(projectId));

                SearchedUserDetails fromAndCCEmployeeDetails = dal.GetEmployeeDetailsByName(EmployeeName);

                ResourseType ResourseTypeList = dal.GetResourceTypesByResourceId(Convert.ToInt32(ResorceType));
                Role RoleDetails = dal.GetEmployeeRolesByRoleId(Convert.ToInt32(projectRole));

                int templateId = 0;

                if (fromAndCCEmployeeDetails != null)
                    model.Mail.To = model.Mail.To + fromAndCCEmployeeDetails.EmployeeEmailId + ";";
                // }

                if (loggedinEmpID == null || loggedinEmpID == 0)
                {
                    int EmployeeID = dal.geteEmployeeIDFromSEMDatabase(Membership.GetUser().UserName);
                    loggedinEmpID = EmployeeID;
                }

                tbl_PM_Employee_SEM EmpDetailsForCCAndFrom = dal.GetEmployeeDetailsFromEmployeeID(Convert.ToInt32(loggedinEmpID));
                if (EmpDetailsForCCAndFrom != null)
                {
                    model.Mail.Cc = EmpDetailsForCCAndFrom.EmailID + ";";
                    model.Mail.From = EmpDetailsForCCAndFrom.EmailID;
                }
                if (btnClick == "Add/Edit Resource")
                {
                    int EmployeeID = dal.geteEmployeeIDFromSEMDatabase(Membership.GetUser().UserName);
                    tbl_PM_Employee_SEM employeeDetails = dal.GetEmployeeDetailsFromEmployeeID(EmployeeID);
                    tbl_PM_Employee_SEM reportingToDetails = dal.GetEmployeeDetailsFromEmployeeID(Convert.ToInt32(ReportingTo));
                    templateId = 63;
                    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                    //tbl_PM_ProjectEmployeeRole projectDetails = dal.getEmployeeRoleDetails(Convert.ToInt32(HelpDeskTicketID));
                    //string EmployeeName = string.Empty;
                    //string ProjectName = string.Empty;
                    foreach (var emailTemplate in template)
                    {
                        model.Mail.Subject = emailTemplate.Subject;
                        //model.Mail.Subject = model.Mail.Subject.Replace("##project name##", projectDeatils.ProjectName);
                        model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                        model.Mail.Message = model.Mail.Message.Replace("##Employeename##", Convert.ToString(EmployeeName));
                        model.Mail.Message = model.Mail.Message.Replace("##project name##", Convert.ToString(projectDetails.ProjectName));
                        if (reportingToDetails != null)
                            model.Mail.Message = model.Mail.Message.Replace("##reporting to##", Convert.ToString(reportingToDetails.EmployeeName));
                        else
                            model.Mail.Message = model.Mail.Message.Replace("Reporting To: ##reporting to##", null);
                        model.Mail.Message = model.Mail.Message.Replace("##project role##", RoleDetails.RoleDescription);
                        model.Mail.Message = model.Mail.Message.Replace("##resource type##", ResourseTypeList.ResourseTypeDescription);
                        model.Mail.Message = model.Mail.Message.Replace("##Allocation Start Date##", FromDate.Value.ToString("MM/dd/yyyy"));
                        model.Mail.Message = model.Mail.Message.Replace("##Allocation End Date##", Todate.Value.ToString("MM/dd/yyyy"));
                        model.Mail.Message = model.Mail.Message.Replace("##allocated (%)##", Convert.ToString(AllocatedPercentage));
                        model.Mail.Message = model.Mail.Message.Replace("##comments##", Convert.ToString(Comments));
                        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", employeeDetails.EmployeeName);
                    }
                }

                //if (btnClick == "btnReleaseUpdate")
                //{
                //    templateId = 64;
                //    tbl_PM_ProjectEmployeeRole ResouceDeatils = new tbl_PM_ProjectEmployeeRole();
                //    if (fromAndCCEmployeeDetails != null)
                //        ResouceDeatils = dal.GetResorcetDetails(Convert.ToInt32(fromAndCCEmployeeDetails.EmployeeId));
                //    tbl_PM_Project project = dal.GetResorcetProjectDetails(projectId);
                //    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                //    tbl_PM_Employee_SEM EmpDetails = dal.GetEmployeeDetailsFromEmployeeID(loggedinEmpID.HasValue ? loggedinEmpID.Value : 0);
                //    foreach (var emailTemplate in template)
                //    {
                //        model.Mail.Subject = emailTemplate.Subject;
                //        model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                //        model.Mail.Message = model.Mail.Message.Replace("##project name##", project.ProjectName);
                //        model.Mail.Message = model.Mail.Message.Replace("##employee name##", EmployeeName);
                //        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", EmpDetailsForCCAndFrom.EmployeeName);
                //        DateTime ActualEndate = Convert.ToDateTime(ResouceDeatils.ActualEndDate);
                //        model.Mail.Message = model.Mail.Message.Replace("##release date##", ActualEndate.ToString("MM/dd/yyyy"));
                //    }
                //}
                if (btnClick == "btnReleaseUpdate")
                {
                    templateId = 64;

                    tbl_PM_ProjectEmployeeRole ResouceDeatils = new tbl_PM_ProjectEmployeeRole();
                    tbl_PM_Project project = dal.GetResorcetProjectDetails(projectId);
                    PersonalDetailsDAL personalDal = new PersonalDetailsDAL();

                    string[] users = Roles.GetUsersInRole("RMG");

                    foreach (string user in users)
                    {
                        HRMS_tbl_PM_Employee employee = personalDal.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(user));
                        if (employee == null)
                            model.Mail.Cc = model.Mail.Cc + string.Empty;
                        else
                            model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                    }

                    if (fromAndCCEmployeeDetails != null)

                        ResouceDeatils = dal.GetResorcetDetails(Convert.ToInt32(fromAndCCEmployeeDetails.EmployeeId), Convert.ToInt32(projectId));

                    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);

                    tbl_PM_Employee_SEM EmpDetails = dal.GetEmployeeDetailsFromEmployeeID(loggedinEmpID.HasValue ? loggedinEmpID.Value : 0);

                    foreach (var emailTemplate in template)
                    {
                        model.Mail.Subject = emailTemplate.Subject;

                        model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);

                        model.Mail.Message = model.Mail.Message.Replace("##project name##", project.ProjectName);

                        model.Mail.Message = model.Mail.Message.Replace("##employee name##", EmployeeName);

                        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", EmpDetailsForCCAndFrom.EmployeeName);

                        // DateTime ActualEndate = Convert.ToDateTime(ResouceDeatils.ActualEndDate);

                        // model.Mail.Message = model.Mail.Message.Replace("##release date##", Convert.ToDateTime(ResouceDeatils.ActualEndDate).ToString("MM/dd/yyyy"));
                    }
                }

                //if (btnClick == "btnCancelRelease")
                //{
                //    templateId = 65;
                //    tbl_PM_ProjectEmployeeRole ResouceDeatils = new tbl_PM_ProjectEmployeeRole();
                //    if (fromAndCCEmployeeDetails != null)
                //        ResouceDeatils = dal.GetResorcetDetails(Convert.ToInt32(fromAndCCEmployeeDetails.EmployeeId));
                //    tbl_PM_Project project = dal.GetResorcetProjectDetails(projectId);
                //    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                //    tbl_PM_Employee_SEM EmpDetails = dal.GetEmployeeDetailsFromEmployeeID(loggedinEmpID.HasValue ? loggedinEmpID.Value : 0);

                //    foreach (var emailTemplate in template)
                //    {
                //        model.Mail.Subject = emailTemplate.Subject;
                //        model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                //        model.Mail.Message = model.Mail.Message.Replace("##project name##", project.ProjectName);
                //        model.Mail.Message = model.Mail.Message.Replace("##employeename##", EmployeeName);
                //        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", EmpDetailsForCCAndFrom.EmployeeName);
                //        DateTime ActualEndate = Convert.ToDateTime(ResouceDeatils.ActualEndDate);
                //        model.Mail.Message = model.Mail.Message.Replace("##release date##", ActualEndate.ToString("MM/dd/yyyy"));
                //    }
                //}

                if (btnClick == "btnCancelRelease")
                {
                    templateId = 65;

                    tbl_PM_ProjectEmployeeRole ResouceDeatils = new tbl_PM_ProjectEmployeeRole();

                    if (fromAndCCEmployeeDetails != null)

                        ResouceDeatils = dal.GetResorcetDetails(Convert.ToInt32(fromAndCCEmployeeDetails.EmployeeId), Convert.ToInt32(projectId));

                    tbl_PM_Project project = dal.GetResorcetProjectDetails(projectId);

                    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);

                    tbl_PM_Employee_SEM EmpDetails = dal.GetEmployeeDetailsFromEmployeeID(loggedinEmpID.HasValue ? loggedinEmpID.Value : 0);

                    foreach (var emailTemplate in template)
                    {
                        model.Mail.Subject = emailTemplate.Subject;

                        model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);

                        model.Mail.Message = model.Mail.Message.Replace("##project name##", project.ProjectName);

                        model.Mail.Message = model.Mail.Message.Replace("##employeename##", EmployeeName);

                        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", EmpDetailsForCCAndFrom.EmployeeName);

                        // DateTime ActualEndate = Convert.ToDateTime(ResouceDeatils.ActualEndDate);

                        model.Mail.Message = model.Mail.Message.Replace("##release date##", Convert.ToDateTime(ResouceDeatils.ActualEndDate).ToString("MM/dd/yyyy"));
                    }
                }
                if (btnClick == "BulkReallocation")
                {
                    string EmpName = string.Empty;
                    if (EmployeeIDs != "")
                    {
                        string EmployeeIDWithcomma = EmployeeIDs.TrimEnd(',');
                        string[] EmployeeArray = EmployeeIDWithcomma.Split(',');
                        int[] EmployeeId = Array.ConvertAll(EmployeeArray, s => int.Parse(s));
                        foreach (var item in EmployeeId)
                        {
                            tbl_PM_Employee_SEM EmpDetails = dal.GetEmployeeDetailsFromEmployeeID(item);
                            if (EmpDetails != null)
                                EmpName = EmpName + EmpDetails.EmployeeName + ",";
                            model.Mail.To = model.Mail.To + EmpDetails.EmailID + ";";
                        }
                    }
                    templateId = 67;
                    //tbl_PM_ProjectEmployeeRole ResouceDeatils = new tbl_PM_ProjectEmployeeRole();
                    //if (fromAndCCEmployeeDetails != null)
                    //    ResouceDeatils = dal.GetResorcetDetails(Convert.ToInt32(fromAndCCEmployeeDetails.EmployeeId));
                    //int ProjectId = 0;
                    //if (ResouceDeatils != null)
                    //    ProjectId = ResouceDeatils.ProjectID;
                    //tbl_PM_Project project = dal.GetResorcetProjectDetails(ProjectId);
                    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                    tbl_PM_Employee_SEM EmpDetail = dal.GetEmployeeDetailsFromEmployeeID(loggedinEmpID.HasValue ? loggedinEmpID.Value : 0);

                    foreach (var emailTemplate in template)
                    {
                        model.Mail.Subject = emailTemplate.Subject;
                        model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                        model.Mail.Message = model.Mail.Message.Replace("##project name##", projectDetails.ProjectName);
                        model.Mail.Message = model.Mail.Message.Replace("##employeename##", Convert.ToString(EmpName));
                        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", EmpDetail.EmployeeName);
                        model.Mail.Message = model.Mail.Message.Replace("##to date##", Todate.Value.ToString("MM/dd/yyyy"));
                    }
                }
                //}
                return PartialView("_CustomerCreationEmail", model.Mail);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult ProjectEndAppraisalFormView(string EmployeeCode, int? ProjectID, int ProjectEmployeeRoleID, int? ProjectEndAppraisalStatusID)
        {
            try
            {
                ProjectEndAppraisalFormModel model = new ProjectEndAppraisalFormModel();
                SemDAL dal = new SemDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.SearchedUserDetails = new SearchedUserDetails();
                ConfigurationDAL configDAL = new ConfigurationDAL();
                ViewBag.AsciiKey = Session["SecurityKey"].ToString();

                string LoggedinUserEmployeeCode = Membership.GetUser().UserName;
                int LoggedinUserEmployeeId = dal.GetEmployeeID(LoggedinUserEmployeeCode);
                ViewBag.loginUserId = LoggedinUserEmployeeId;

                int EmployeeID = dal.GetEmployeeID(EmployeeCode);

                tbl_PM_Employee_SEM details = dal.GetEmployeeDetailsFromEmployeeID(EmployeeID);
                model.EmployeeName = details.EmployeeName;

                model.LoggedinUserEmployeeCode = LoggedinUserEmployeeCode;
                model.ProjectEmployeeRoleID = ProjectEmployeeRoleID;
                int proID = 0;
                tbl_PM_ProjectEmployeeRole EmpRoleDetails = dal.GetProjectEmployeeRoleAllocationDetails(ProjectEmployeeRoleID);
                if (EmpRoleDetails != null)
                {
                    model.AllocationStartDate = EmpRoleDetails.ExpectedStartDate;
                    if (EmpRoleDetails.ActualEndDate == null)
                    {
                        model.ReleaseDate = EmpRoleDetails.ExpectedEndDate;
                    }
                    else
                    {
                        model.ReleaseDate = EmpRoleDetails.ActualEndDate;
                    }
                    proID = EmpRoleDetails.ProjectID;
                }
                tbl_PM_Employee_SEM loggedinUserEmpDetails = dal.GetEmployeeDetailsFromEmployeeID(Convert.ToInt32(details.ReportingTo));
                model.ProjectManager = loggedinUserEmpDetails.EmployeeName;

                tbl_PM_Project projDetails = dal.GetProjectDetails(proID);
                if (projDetails != null)
                {
                    model.ProjectName = projDetails.ProjectName;
                }

                List<RatingScales> ratingScale = configDAL.GetRatingScales();
                model.RatingScale = ratingScale;
                model.JoiningDate = EmpRoleDetails.ExpectedStartDate;
                for (int i = 0; i < model.RatingScale.Count; i++)
                {
                    model.RatingScale[i].Percentage = Convert.ToInt32(model.RatingScale[i].Percentage);
                }

                List<ProjectEndAppraisalParameters> parameterlist = dal.GetProjectEndAppraisalParameters(EmployeeID, ProjectID, ProjectEndAppraisalStatusID);
                if (parameterlist.Count > 0)
                {
                    model.ProjectEndAppraisalParameterList = parameterlist;
                }

                ParameterRating Rating = dal.GetMinMaxRating();
                ViewBag.minRating = Rating.MinValue;
                ViewBag.maxRating = Rating.MaxValue;
                List<int> ratingList = new List<int>();
                for (int i = Rating.MinValue; i <= Rating.MaxValue; i++)
                {
                    ratingList.Add(i);
                }
                model.RatingsList = ratingList;

                model.EmployeeID = EmployeeID;
                model.ProjectID = ProjectID;
                model.ProjectEndAppraisalFormStatus = ProjectEndAppraisalStatusID;
                foreach (var l in parameterlist)
                {
                    if (l.ProjectLead != null)
                    {
                        model.ProjectLead = l.ProjectLead;
                        break;
                    }
                }

                return View("ProjectEndAppraisalForm", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SaveProjectEndAppraisalFormDetails(List<ProjectEndAppraisalParameters> ProjectEndAppraisalParameters)
        {
            try
            {
                DAL.SemDAL dal = new DAL.SemDAL();
                string resultMessage = string.Empty;

                var status = dal.SaveProjectEndAppraisalFormDetails(ProjectEndAppraisalParameters);

                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        private GridView bindGrid = new GridView();
        private GridView bindGrid1 = new GridView();
        private GridView gv = new GridView();
        private GridView gv2 = new GridView();

        public void ExportToExcelResourceData(string ProjectID, string ProjectName, string EmployeeId)
        {
            WSEMDBEntities dbContext = new WSEMDBEntities();

            // This Below two Dataset for Manageresource grids and My allocation Grid
            DataSet dsCurrentDetails = new DataSet();
            DataSet dsHistoryDetails = new DataSet();

            int SemEmployeeId = 0;
            SemDAL dal = new SemDAL();
            if (EmployeeId != null)
            {
                SemEmployeeId = dal.GetSemEmployeeId(Convert.ToInt32(EmployeeId));
            }

            if (EmployeeId == null)
            {
                int ProID = Convert.ToInt32(ProjectID);
                var CurrentDetails = dbContext.GetCurrentUsersForProject(ProID, null, null);
                var HistoryDetails = dbContext.GetHistoryForProject(ProID, null, null);
                DataTable dt = new DataTable();
                dt.Columns.Add("Employee ID", typeof(string));
                dt.Columns.Add("EmployeeName", typeof(string));
                dt.Columns.Add("ResourcePoolName", typeof(string));
                dt.Columns.Add("DesignationName", typeof(string));
                dt.Columns.Add("Resource Type", typeof(string));
                dt.Columns.Add("EmploymentStatus", typeof(string));
                dt.Columns.Add("StartDate", typeof(string));
                dt.Columns.Add("EndDate", typeof(string));
                dt.Columns.Add("Allocation Percentage", typeof(string));
                dt.Columns.Add("Comments", typeof(string));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("Employee ID", typeof(string));
                dt2.Columns.Add("EmployeeName", typeof(string));
                dt2.Columns.Add("ResourcePoolName", typeof(string));
                dt2.Columns.Add("DesignationName", typeof(string));
                dt2.Columns.Add("Resource Type", typeof(string));
                dt2.Columns.Add("EmploymentStatus", typeof(string));
                dt2.Columns.Add("StartDate", typeof(string));
                dt2.Columns.Add("EndDate", typeof(string));
                dt2.Columns.Add("Allocation Percentage", typeof(string));
                dt2.Columns.Add("Comments", typeof(string));

                if (CurrentDetails != null)
                {
                    foreach (var item in CurrentDetails)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = item.employeecode;
                        dr[1] = item.EmployeeName;
                        dr[2] = item.ResourcePoolName;
                        dr[3] = item.DesignationName;
                        dr[4] = item.ResourceStatus;
                        dr[5] = item.EmploymentStatus;
                        dr[6] = item.StartDate;
                        dr[7] = item.EndDate;
                        dr[8] = item.AllocatedPercentage;
                        dr[9] = item.Comments;
                        dt.Rows.Add(dr);
                    }
                    dsCurrentDetails.Tables.Add(dt);
                    bindGrid.DataSource = dsCurrentDetails;
                    bindGrid.DataBind();
                }
                if (HistoryDetails != null)
                {
                    foreach (var item in HistoryDetails)
                    {
                        DataRow dr2 = dt2.NewRow();
                        dr2[0] = item.employeecode;
                        dr2[1] = item.EmployeeName;
                        dr2[2] = item.ResourcePoolName;
                        dr2[3] = item.DesignationName;
                        dr2[4] = item.ResourceStatus;
                        dr2[5] = item.EmploymentStatus;
                        dr2[6] = item.StartDate;
                        dr2[7] = item.EndDate;
                        dr2[8] = item.AllocatedPercentage;
                        dr2[9] = item.Comments;
                        dt2.Rows.Add(dr2);
                    }
                    dsHistoryDetails.Tables.Add(dt2);
                    bindGrid1.DataSource = dsHistoryDetails;
                    bindGrid1.DataBind();
                }
            }
            if (EmployeeId != null)
            {
                var CurrentDetails = dbContext.SearchCurrentUsersForProject_SP(SemEmployeeId);
                var HistoryDetails = dbContext.SearchResourceHis_SP(SemEmployeeId);
                DataTable dt = new DataTable();
                dt.Columns.Add("Employee ID", typeof(string));
                dt.Columns.Add("EmployeeName", typeof(string));
                dt.Columns.Add("ResourcePoolName", typeof(string));
                dt.Columns.Add("DesignationName", typeof(string));
                dt.Columns.Add("Resource Type", typeof(string));
                dt.Columns.Add("EmploymentStatus", typeof(string));
                dt.Columns.Add("StartDate", typeof(string));
                dt.Columns.Add("EndDate", typeof(string));
                dt.Columns.Add("Allocation Percentage", typeof(string));
                dt.Columns.Add("Comments", typeof(string));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("Employee ID", typeof(string));
                dt2.Columns.Add("EmployeeName", typeof(string));
                dt2.Columns.Add("ResourcePoolName", typeof(string));
                dt2.Columns.Add("DesignationName", typeof(string));
                dt2.Columns.Add("Resource Type", typeof(string));
                dt2.Columns.Add("EmploymentStatus", typeof(string));
                dt2.Columns.Add("StartDate", typeof(string));
                dt2.Columns.Add("EndDate", typeof(string));
                dt2.Columns.Add("Allocation Percentage", typeof(string));
                dt2.Columns.Add("Comments", typeof(string));

                if (CurrentDetails != null)
                {
                    foreach (var item in CurrentDetails)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = item.employeecode;
                        dr[1] = item.EmployeeName;
                        dr[2] = item.ResourcePoolName;
                        dr[3] = item.DesignationName;
                        dr[4] = item.ResourceStatus;
                        dr[5] = item.EmploymentStatus;
                        dr[6] = item.StartDate;
                        dr[7] = item.EndDate;
                        dr[8] = item.AllocatedPercentage;
                        dr[9] = item.Comments;
                        dt.Rows.Add(dr);
                    }
                    dsCurrentDetails.Tables.Add(dt);
                    bindGrid.DataSource = dsCurrentDetails;
                    bindGrid.DataBind();
                }
                if (HistoryDetails != null)
                {
                    foreach (var item in HistoryDetails)
                    {
                        DataRow dr2 = dt2.NewRow();
                        dr2[0] = item.EmployeeID;
                        dr2[1] = item.EmployeeName;
                        dr2[2] = item.ResourcePoolName;
                        dr2[3] = item.DesignationName;
                        dr2[4] = item.ResourceStatus;
                        dr2[5] = item.EmploymentStatus;
                        dr2[6] = item.StartDate;
                        dr2[7] = item.EndDate;
                        dr2[8] = item.AllocatedPercentage;
                        dr2[9] = item.Comments;
                        dt2.Rows.Add(dr2);
                    }
                    dsHistoryDetails.Tables.Add(dt2);
                    bindGrid1.DataSource = dsHistoryDetails;
                    bindGrid1.DataBind();
                }
            }
            string strFileNameFrom = Convert.ToString(System.DateTime.Now.ToShortDateString().ToString());
            string strProjname;

            if (ProjectName == null)
                strProjname = "MyAllocation";
            else
                strProjname = Convert.ToString(ProjectName);

            string strFileNameTo = Convert.ToString(System.DateTime.Now.ToShortDateString().ToString());
            string strFileName = "V2_RA_" + strProjname.ToString() + "_" + strFileNameTo.ToString().Replace("/", "-");
            strFileName = strFileName + ".xls";
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename = " + strFileName);
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);

            bindGrid.SetRenderMethodDelegate(new RenderMethod(RenderTitleCurrent));
            bindGrid.RenderControl(oHtmlTextWriter);
            // Export to Excel grid title
            bindGrid1.SetRenderMethodDelegate(new RenderMethod(RenderTitleHistory));
            bindGrid1.RenderControl(oHtmlTextWriter);

            Response.Write(oStringWriter.ToString());
            Response.End();
        }

        public virtual void RenderTitleCurrent(HtmlTextWriter writer, Control ctl)
        {
            writer.AddAttribute("colspan", bindGrid.Columns.Count.ToString(System.Globalization.CultureInfo.InvariantCulture));
            writer.AddAttribute("align", "center");
            writer.RenderBeginTag("TD");
            writer.Write(" Current Resource Details");
            writer.RenderEndTag();
            bindGrid.HeaderStyle.AddAttributesToRender(writer);
            writer.RenderBeginTag("TR");
            writer.RenderEndTag();
            foreach (Control control in ctl.Controls)
            {
                control.RenderControl(writer);
            }
        }

        protected virtual void RenderTitleHistory(HtmlTextWriter writer, Control ctl)
        {
            writer.AddAttribute("colspan", bindGrid1.Columns.Count.ToString(System.Globalization.CultureInfo.InvariantCulture));
            writer.AddAttribute("align", "center");
            writer.RenderBeginTag("TD");
            writer.Write("Resource History Details");
            writer.RenderEndTag();
            bindGrid1.HeaderStyle.AddAttributesToRender(writer);
            writer.RenderBeginTag("TR");
            writer.RenderEndTag();
            foreach (Control control in ctl.Controls)
            {
                control.RenderControl(writer);
            }
        }

        [HttpPost]
        public ActionResult SaveProjectRoleForemployee(RMGViewPostModel model, string RoleId, int reportTo)
        {
            try
            {
                string resultMessage = string.Empty;
                bool status = false;
                if (RoleId == "undefined")
                    RoleId = "0";
                SemDAL dal = new SemDAL();
                status = dal.SaveEmployeeRole(model, Convert.ToInt32(RoleId), Convert.ToInt32(reportTo));
                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveReportingTo(string ProjectEmployeeRoleId, int ReportingTo)
        {
            try
            {
                bool status = false;
                SemDAL dal = new SemDAL();
                status = dal.SaveEmployeeReportingTo(ProjectEmployeeRoleId, ReportingTo);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public System.Threading.Timer myTimer;

        public void SetTimerValue()
        {
            // trigger the event at 9 AM. For 7 PM use 21 i.e. 24 hour format
            DateTime requiredTime = DateTime.Today.AddHours(10).AddMinutes(30);
            if (DateTime.Now > requiredTime)
            {
                requiredTime = requiredTime.AddDays(1);
            }
            // initialize timer only, do not specify the start time or the interval
            myTimer = new System.Threading.Timer(new TimerCallback(TimerAction));
            // first parameter is the start time and the second parameter is the interval
            // Timeout.Infinite means do not repeat the interval, only start the timer
            myTimer.Change((int)(requiredTime - DateTime.Now).TotalMilliseconds, Timeout.Infinite);
        }

        public void TimerAction(object e)
        {
            // do some work
            sendMail();
            // now, call the set timer method to reset its next call time
            SetTimerValue();
        }

        public void sendMail()
        {
            SemDAL dal = new SemDAL();
            DataSet dsResourceDetails = dal.GetAutoTriggerMailDetailsForResource();
            var values = new List<Tuple<int, string, string, string, string, string>>();

            foreach (DataTable t in dsResourceDetails.Tables)
            {
                foreach (DataRow row in t.Rows)
                {
                    string projectIds = row["projectId"].ToString();
                    string ProjName = row["projectName"].ToString();
                    string EndDate = string.Empty;
                    if (row["AllocationEndDate"].ToString() != null)
                    {
                        EndDate = row["AllocationEndDate"].ToString();
                    }
                    string EmpEmailId = row["EmpEMailId"].ToString();
                    string ManagerEmailId = row["ManagerEmailId"].ToString();
                    string EmpName = row["EmpName"].ToString();
                    int ProjectId = Convert.ToInt32(projectIds);
                    values.Add(new Tuple<int, string, string, string, string, string>(ProjectId, ProjName, EndDate, EmpEmailId, ManagerEmailId, EmpName));
                }
            }

            for (int i = 0; i < values.Count; i++)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(values[i].Item4);
                mail.To.Add(values[i].Item5);
                PersonalDetailsDAL personalDal = new PersonalDetailsDAL();
                string[] users = Roles.GetUsersInRole("RMG");
                foreach (string user in users)
                {
                    HRMS_tbl_PM_Employee employee = personalDal.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(user));
                    if (employee == null)
                    { }
                    else
                        mail.CC.Add(employee.EmailID);
                }

                string RMGEmail = System.Configuration.ConfigurationManager.AppSettings["RMGEmailId"].ToString();
                string Email = string.Empty;
                Email = RMGEmail;
                mail.CC.Add(RMGEmail);
                mail.From = new MailAddress(Email, "RMG");

                TravelViewModel model = new TravelViewModel();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                model.Mail = new TravelMailTemplate();
                int templateId = 66;
                List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                foreach (var emailTemplate in template)
                {
                    model.Mail.Subject = emailTemplate.Subject;
                    model.Mail.Message = emailTemplate.Message.Replace("<br>", Environment.NewLine);
                    model.Mail.Message = model.Mail.Message.Replace("##employee name##", values[i].Item6);
                    model.Mail.Message = model.Mail.Message.Replace("##project name##", values[i].Item2);
                    model.Mail.Message = model.Mail.Message.Replace("##allocation end date##", values[i].Item3);
                    model.Mail.Message = model.Mail.Message.Replace("##logged in user##", System.Configuration.ConfigurationManager.AppSettings["RMGName"].ToString());
                }
                SmtpClient smtpClient = new SmtpClient();
                mail.Subject = model.Mail.Subject;
                mail.Body = model.Mail.Message;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.Host = System.Configuration.ConfigurationManager.AppSettings["SMTPServerName"].ToString();
                //smtpClient.Host = "v2mailserver.in.v2solutions.com";
                string UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"].ToString();
                string Password = System.Configuration.ConfigurationManager.AppSettings["Password"].ToString();
                smtpClient.Credentials = new System.Net.NetworkCredential(UserName, Password);
                smtpClient.Port = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["PortNumber"].ToString());
                smtpClient.Send(mail);
            }
        }

        [HttpPost]
        public ActionResult CheckTimeSheetDetails(string releaseDate, string EmployeeID)
        {
            try
            {
                string resultMessage = string.Empty;
                bool status = false;
                DateTime? ReleaseDate = null;
                if (releaseDate != "null")
                {
                    ReleaseDate = Convert.ToDateTime(releaseDate);
                }
                WSEMDBEntities dbContext = new WSEMDBEntities();
                tbl_PM_DailyActivity DatailActivityDetails = dbContext.tbl_PM_DailyActivity.Where(x => x.EntryDate == ReleaseDate && x.EmployeeID == EmployeeID).FirstOrDefault();
                if (DatailActivityDetails == null)
                    status = false;
                else
                    status = true;

                if (status)
                    resultMessage = "Saved";
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public void ExportToExcelBenchResourceData(int projectID, string ProjectName, DateTime? AsOnDate, string EmployeeId)
        {
            WSEMDBEntities dbContext = new WSEMDBEntities();

            // This Below two Dataset for Manageresource grids and My allocation Grid
            DataSet dsCurrentDetails = new DataSet();

            int SemEmployeeId = 0;
            SemDAL dal = new SemDAL();
            if (EmployeeId != null && EmployeeId != "0")
            {
                SemEmployeeId = dal.GetSemEmployeeId(Convert.ToInt32(EmployeeId));
            }
            //if (EmployeeId != null && EmployeeId != "0")
            //{
            var CurrentDetails = dbContext.GetUnallocatedResource_sp(AsOnDate, null);
            if (SemEmployeeId != 0)
                CurrentDetails = dbContext.GetUnallocatedResource_sp(AsOnDate, SemEmployeeId);
            DataTable dt = new DataTable();
            dt.Columns.Add("Employee ID", typeof(string));
            dt.Columns.Add("EmployeeName", typeof(string));
            dt.Columns.Add("UnallocatedFrom", typeof(string));
            dt.Columns.Add("PrimarySkills", typeof(string));
            dt.Columns.Add("Designationname", typeof(string));
            dt.Columns.Add("ReportingTo", typeof(string));
            dt.Columns.Add("Allocation Percentage", typeof(string));
            dt.Columns.Add("Present/Absent", typeof(string));

            if (CurrentDetails != null)
            {
                foreach (var item in CurrentDetails)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = item.EmployeeCode;
                    dr[1] = item.EmployeeName;
                    dr[2] = item.unallocatedfrom;
                    dr[3] = item.Resource_Pool;
                    dr[4] = item.designationname;
                    dr[5] = item.ReportingTo;
                    dr[6] = item.percentage;
                    dr[7] = item.Present_Absent;
                    dt.Rows.Add(dr);
                }
                dsCurrentDetails.Tables.Add(dt);
                bindGrid.DataSource = dsCurrentDetails;
                bindGrid.DataBind();
            }
            //}
            string strFileNameFrom = Convert.ToString(System.DateTime.Now.ToShortDateString().ToString());
            string strProjname;

            if (ProjectName == null)
                strProjname = "MyAllocation";
            else
                strProjname = Convert.ToString(ProjectName);

            string strFileNameTo = Convert.ToString(System.DateTime.Now.ToShortDateString().ToString());
            string strFileName = "V2_RA_" + strProjname.ToString() + "_" + strFileNameTo.ToString().Replace("/", "-");
            strFileName = strFileName + ".xls";
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename = " + strFileName);
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);

            bindGrid.SetRenderMethodDelegate(new RenderMethod(RenderTitleBench));
            bindGrid.RenderControl(oHtmlTextWriter);
            // Export to Excel grid title

            Response.Write(oStringWriter.ToString());
            Response.End();
        }

        public virtual void RenderTitleBench(HtmlTextWriter writer, Control ctl)
        {
            writer.AddAttribute("colspan", bindGrid.Columns.Count.ToString(System.Globalization.CultureInfo.InvariantCulture));
            writer.AddAttribute("align", "center");
            writer.RenderBeginTag("TD");
            writer.Write(" Bench Resource Details");
            writer.RenderEndTag();
            bindGrid.HeaderStyle.AddAttributesToRender(writer);
            writer.RenderBeginTag("TR");
            writer.RenderEndTag();
            foreach (Control control in ctl.Controls)
            {
                control.RenderControl(writer);
            }
        }

        public ActionResult ManualRelease(string EmployeeId, string ProjectEmployeeRoleID, string ProjectID)
        {
            SemDAL dal = new SemDAL();

            bool result = dal.ReleaseResource(Convert.ToInt32(EmployeeId), Convert.ToInt32(ProjectEmployeeRoleID), Convert.ToInt32(ProjectID));

            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
            //RMGCurrentResourceLoadGrid(string EmployeeId, int projectID, string searchText, DateTime? AsOnDate, int page, int rows, string GridName, int? ResourcePoolId, int? EmployeeForProject)
            // RMGCurrentResourceLoadGrid("", Convert.ToInt32(ProjectID), "", null, 0, 0, "", 0, 0);
        }
    }
}