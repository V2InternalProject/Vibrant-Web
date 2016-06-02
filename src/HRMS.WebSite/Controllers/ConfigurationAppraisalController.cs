using HRMS.DAL;
using HRMS.Models;
using HRMS.Notification;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS.Controllers
{
    public class ConfigurationAppraisalController : Controller
    {
        //
        // GET: /ConfigurationAppraisal/
        private ConfigurationAppraisalDAL appraisalDAL = new ConfigurationAppraisalDAL();

        public ActionResult Index()
        {
            Session["SearchEmpFullName"] = null;  // to hide emp search
            Session["SearchEmpCode"] = null;
            Session["SearchEmpID"] = null;
            return View();
        }

        [HttpGet]
        public ActionResult ConfigureAppraisalYear()
        {
            try
            {
                Session["SearchEmpFullName"] = null;  // to hide emp search
                Session["SearchEmpCode"] = null;
                Session["SearchEmpID"] = null;

                AppraisalYearModel confirmationmodel = new AppraisalYearModel();
                string EmployeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                confirmationmodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                confirmationmodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                confirmationmodel.SearchedUserDetails.EmployeeCode = EmployeeCode;
                EmployeeDAL DAL = new EmployeeDAL();
                confirmationmodel.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
                ConfigurationDAL configDAL = new ConfigurationDAL();
                List<CompetencyMaster> competencyMaster = configDAL.GetCompetencyMaster();
                return PartialView("_ConfigureAppraisalYear", confirmationmodel);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult AppraisalStatus()
        {
            try
            {
                AppraisalStatusReport model = new AppraisalStatusReport();

                string EmployeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                model.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                model.SearchedUserDetails.EmployeeCode = EmployeeCode;

                EmployeeDAL DAL = new EmployeeDAL();
                model.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
                ViewBag.LoggedInEmployeeId = model.SearchedUserDetails.EmployeeId;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                List<CompetencyMaster> competencyMaster = configDAL.GetCompetencyMaster();
                model.AppraisalYear = new List<AppraisalYearReport>();
                model.FreezeAppraisalYear = new List<AppraisalYearReport>();
                model.IndividualDevelopmentYear = new List<AppraisalYearReport>();
                model.InitiateIndividualDevelopmentYear = new List<AppraisalYearReport>();
                model.FreezeIndividualDevelopmentYear = new List<AppraisalYearReport>();
                List<tbl_Appraisal_YearMaster> yearList = configDAL.GetYearList();

                foreach (tbl_Appraisal_YearMaster year in yearList.Where(x => x.AppraisalYearStatus == 0))
                {
                    model.FreezeYearID = year.AppraisalYearID;
                    model.FreezeIndividualDevelopmentYearID = year.AppraisalYearID;
                }
                foreach (tbl_Appraisal_YearMaster year in yearList)
                {
                    model.AppraisalYear.Add(new AppraisalYearReport()
                    {
                        AppraisalYearID = year.AppraisalYearID,
                        AppraisalYearDesc = year.AppraisalYear
                    });

                    model.FreezeAppraisalYear.Add(new AppraisalYearReport()
                    {
                        AppraisalYearID = year.AppraisalYearID,
                        AppraisalYearDesc = year.AppraisalYear
                    });

                    model.IndividualDevelopmentYear.Add(new AppraisalYearReport()
                    {
                        AppraisalYearID = year.AppraisalYearID,
                        AppraisalYearDesc = year.AppraisalYear
                    });

                    model.FreezeIndividualDevelopmentYear.Add(new AppraisalYearReport()
                    {
                        AppraisalYearID = year.AppraisalYearID,
                        AppraisalYearDesc = year.AppraisalYear
                    });

                    if (year.AppraisalYearFrozenOn != null && year.AppraisalYearFrozenBy != null && (year.IDFInitiatedOn == null || year.IDFInitiatedOn > DateTime.Now))
                        model.InitiateIndividualDevelopmentYear.Add(new AppraisalYearReport()
                        {
                            AppraisalYearID = year.AppraisalYearID,
                            AppraisalYearDesc = year.AppraisalYear
                        });
                }
                model.Mail = new EmployeeMailTemplate();
                return PartialView("_AppraisalStatus", model);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult InitiateIndividualDevelopment(int YearID, string InitiateDate)
        {
            try
            {
                AppraisalProcessResponse initiateResponse = new AppraisalProcessResponse();
                ConfigurationDAL configDAL = new ConfigurationDAL();
                EmployeeDAL DAL = new EmployeeDAL();
                ConfigurationAppraisalDAL _configurationAppraisalDAL = new ConfigurationAppraisalDAL();
                DateTime initiateDate = Convert.ToDateTime(InitiateDate);
                string EmployeeCode = Membership.GetUser().UserName;
                int EmployeeId = DAL.GetEmployeeID(EmployeeCode);
                initiateResponse = configDAL.InitiateIndividualDevelopment(YearID, initiateDate, EmployeeId);

                List<int?> appraiserList = _configurationAppraisalDAL.GetAppraiserList(YearID);

                var appraiserNames = "";

                foreach (var user in appraiserList)
                {
                    appraiserNames = appraiserNames + user + ",";
                }

                return Json(new
                {
                    appraiserListData = appraiserNames,
                    status = initiateResponse.isAdded,
                    InitiateIDF_LessThan_FreezePerformanceAppraisal = initiateResponse.InitiateIDF_LessThan_FreezePerformanceAppraisal,
                    InitiateIDF_GreaterThan_FreezeIDF = initiateResponse.InitiateIDF_GreaterThan_FreezeIDF
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult FreezeIndividualDevelopment(int YearID, string FroozenDate)
        {
            ConfigurationDAL configDAL = new ConfigurationDAL();
            EmployeeDAL DAL = new EmployeeDAL();
            DateTime froozenDate = Convert.ToDateTime(FroozenDate);
            string EmployeeCode = Membership.GetUser().UserName;
            int EmployeeId = DAL.GetEmployeeID(EmployeeCode);
            string result = configDAL.FreezeIndividualDevelopment(YearID, froozenDate, EmployeeId);
            if (result == "true")
                return Json(new { status = "true" }, JsonRequestBehavior.AllowGet);
            else if (result == "Initiate IDF first")
                return Json(new { status = "Initiate IDF first" }, JsonRequestBehavior.AllowGet);
            else if (result == "Appraisal FreezeDate Greater")
                return Json(new { status = "Appraisal FreezeDate Greater" }, JsonRequestBehavior.AllowGet);
            else if (result == "IDFInitiate Date Greater")
                return Json(new { status = "IDFInitiate Date Greater" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { status = "false" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FreezeAppraisalYear(int YearID, string FroozenDate)
        {
            ConfigurationDAL configDAL = new ConfigurationDAL();
            EmployeeDAL DAL = new EmployeeDAL();
            DateTime froozenDate = Convert.ToDateTime(FroozenDate);
            string EmployeeCode = Membership.GetUser().UserName;
            int EmployeeId = DAL.GetEmployeeID(EmployeeCode);
            string result = configDAL.FreezeAppraisalYear(YearID, froozenDate, EmployeeId);
            if (result == "true")
                return Json(new { status = "true" }, JsonRequestBehavior.AllowGet);
            else if (result == "Cannot Change")
                return Json(new { status = "Cannot Change" }, JsonRequestBehavior.AllowGet);
            else if (result == "Greater Date")
                return Json(new { status = "Greater Date" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { status = "false" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitiateIndividualDevelopStage()
        {
            AppraisalStatusReport model = new AppraisalStatusReport();
            ConfigurationDAL configDAL = new ConfigurationDAL();
            List<tbl_Appraisal_YearMaster> yearList = configDAL.GetYearList();
            model.InitiateIndividualDevelopmentYear = new List<AppraisalYearReport>();
            foreach (tbl_Appraisal_YearMaster year in yearList.Where(x => x.AppraisalYearStatus == 0))
            {
                model.FreezeIndividualDevelopmentYearID = year.AppraisalYearID;
            }
            foreach (tbl_Appraisal_YearMaster year in yearList)
            {
                if (year.AppraisalYearFrozenOn != null && year.AppraisalYearFrozenBy != null && year.IDFFrozenOn == null && year.IDFFrozenBy == null)
                    model.InitiateIndividualDevelopmentYear.Add(new AppraisalYearReport()
                    {
                        AppraisalYearID = year.AppraisalYearID,
                        AppraisalYearDesc = year.AppraisalYear
                    });
            }
            return PartialView("_InitiateIndividualDevelopment", model);
        }

        [HttpPost]
        public ActionResult FreezeIndividualDevelopmentLoadGrid(int page, int rows)
        {
            try
            {
                AppraisalStatusReport model = new AppraisalStatusReport();
                ConfigurationDAL configDAL = new ConfigurationDAL();
                int totalCount;
                model.FreezeIndividualDevelopmentStageList = configDAL.GetFreezeIndividualDevelopmentStageList(page, rows, out totalCount);

                if ((model.FreezeIndividualDevelopmentStageList == null || model.FreezeIndividualDevelopmentStageList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model.FreezeIndividualDevelopmentStageList = configDAL.GetFreezeIndividualDevelopmentStageList(page, rows, out totalCount);
                }

                var jsonData = new
               {
                   total = (int)Math.Ceiling((double)totalCount / (double)rows),
                   page = page,
                   records = totalCount,
                   rows = model.FreezeIndividualDevelopmentStageList
               };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult InitiateIndividualDevelopmentLoadGrid(int page, int rows)
        {
            try
            {
                AppraisalStatusReport model = new AppraisalStatusReport();
                ConfigurationDAL configDAL = new ConfigurationDAL();
                int totalCount;
                model.InitiateIndividualDevelopmentStageList = configDAL.GetInitiateIndividualDevelopList(page, rows, out totalCount);

                if ((model.InitiateIndividualDevelopmentStageList == null || model.InitiateIndividualDevelopmentStageList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model.InitiateIndividualDevelopmentStageList = configDAL.GetInitiateIndividualDevelopList(page, rows, out totalCount);
                }

                var jsonData = new
               {
                   total = (int)Math.Ceiling((double)totalCount / (double)rows),
                   page = page,
                   records = totalCount,
                   rows = model.InitiateIndividualDevelopmentStageList
               };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult FreezeAppraisalLoadGrid(int page, int rows)
        {
            try
            {
                AppraisalStatusReport model = new AppraisalStatusReport();
                ConfigurationDAL configDAL = new ConfigurationDAL();
                int totalCount;
                model.FreezeAppraisalPeriodList = configDAL.GetFreezeAppraisalPeriodList(page, rows, out totalCount);

                if ((model.FreezeAppraisalPeriodList == null || model.FreezeAppraisalPeriodList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model.FreezeAppraisalPeriodList = configDAL.GetFreezeAppraisalPeriodList(page, rows, out totalCount);
                }

                var jsonData = new
               {
                   total = (int)Math.Ceiling((double)totalCount / (double)rows),
                   page = page,
                   records = totalCount,
                   rows = model.FreezeAppraisalPeriodList
               };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult IndividualDevelopmentStatusReportByYear(int YearID)
        {
            AppraisalStatusReport ReportModel = new AppraisalStatusReport();
            ConfigurationDAL configDAL = new ConfigurationDAL();
            ReportModel.IndividualDevelopmentStatusReportList = configDAL.GetIndividualDevelopmentStatusReportForTheYear(YearID);
            ReportModel.IndividualDevelopmentYearID = YearID;
            return PartialView("IndividualDevelopmentStatusReportByYear", ReportModel);
        }

        [HttpGet]
        public ActionResult AppraisalStatusReportByYear(int YearID)
        {
            AppraisalStatusReport ReportModel = new AppraisalStatusReport();
            ConfigurationDAL configDAL = new ConfigurationDAL();
            ReportModel.AppraisalReportViewModelList = configDAL.GetAppraisalReportForTheYear(YearID);
            ReportModel.YearID = YearID;
            return PartialView("AppraisalStatusReportByYear", ReportModel);
        }

        [HttpGet]
        public ActionResult AppraisalRatingsReportByYear(int YearID)
        {
            try
            {
                AppraisalStatusReport RatingModel = new AppraisalStatusReport();
                ConfigurationAppraisalDAL configAppraisalDal = new ConfigurationAppraisalDAL();
                RatingModel.AppraisalRatingsReportList = new List<AppraisalStatusReportViewModel>();
                RatingModel.AppraisalRatingsReportList = configAppraisalDal.GetAppraisalRatingsReportDetails(YearID);
                RatingModel.AppraisalRatingsYearID = YearID;
                RatingModel.ParameterList = configAppraisalDal.getParamterList(YearID);
                Session["YearID"] = YearID;
                //RatingModel.AppraisalStatusReportRatingAndComments = configAppraisalDal.getAppraisalRatingAndComments(YearID);
                return PartialView("_AppraisalRatingCommentsReportByYear", RatingModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public void ExportToExcel(AppraisalStatusReport app)
        {
            DataSet ds = new DataSet();
            GridView gv = new GridView();
            if (app.AppraisalReportViewModelList != null)
            {
                List<PrintAppraisalStatusReport> report = new List<PrintAppraisalStatusReport>();
                app.AppraisalReportViewModelList.ForEach(x =>
                {
                    PrintAppraisalStatusReport rep = new PrintAppraisalStatusReport();
                    rep.EmployeeName = x.EmployeeName;
                    rep.EmployeeCode = x.Employeecode;
                    rep.EmployeeID = x.EmployeeID;
                    rep.DeliveryTeam = x.DeliveryTeamName;
                    rep.Designation = x.DesignationName;
                    rep.ConfirmationDate = x.ConfirmationDate;
                    rep.ProbationDate = x.ProbationReviewDate;
                    rep.Appraiser1 = x.Appraiser1Name;
                    rep.Appraiser2 = x.Appraiser2Name;
                    rep.Reviewer1 = x.Reviewer1Name;
                    rep.Reviewer2 = x.Reviewer2Name;
                    rep.GroupHead = x.GroupHeadName;
                    rep.Stage = x.AppraisalStageDesc;
                    rep.EmployeeEmail = x.EmployeeEmail;
                    rep.Appraiser1Email = x.Appraiser1Email;
                    rep.Appraiser2Email = x.Appraiser2Email;
                    rep.Reviewer1Email = x.Reviewer1Email;
                    rep.Reviewer2Email = x.Reviewer2Email;
                    rep.GroupHeadEmail = x.GroupHeadEmail;
                    report.Add(rep);
                });
                DataTable dt = new DataTable();
                dt.Columns.Add("Employee Code", typeof(string));
                dt.Columns.Add("Employee ID", typeof(string));
                dt.Columns.Add("Employee Name", typeof(string));
                dt.Columns.Add("Delivery Team", typeof(string));
                dt.Columns.Add("Designation", typeof(string));
                dt.Columns.Add("Probation Review Date", typeof(string));
                dt.Columns.Add("Confirmation Date", typeof(string));
                dt.Columns.Add("Appraiser1", typeof(string));
                dt.Columns.Add("Appraiser2", typeof(string));
                dt.Columns.Add("Reviewer1", typeof(string));
                dt.Columns.Add("Reviewer2", typeof(string));
                dt.Columns.Add("GroupHead", typeof(string));
                dt.Columns.Add("Stage", typeof(string));
                dt.Columns.Add("EmployeeEmail", typeof(string));
                dt.Columns.Add("Appraiser1Email", typeof(string));
                dt.Columns.Add("Appraiser2Email", typeof(string));
                dt.Columns.Add("Reviewer1Email", typeof(string));
                dt.Columns.Add("Reviewer2Email", typeof(string));
                dt.Columns.Add("GroupHeadEmail", typeof(string));

                foreach (var array in report)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = array.EmployeeCode;
                    dr[1] = array.EmployeeID;
                    dr[2] = array.EmployeeName;
                    dr[3] = array.DeliveryTeam;
                    dr[4] = array.Designation;
                    dr[5] = string.Format("{0:dd/MM/yy}", array.ProbationDate);
                    dr[6] = string.Format("{0:dd/MM/yyyy}", array.ConfirmationDate);
                    dr[7] = array.Appraiser1;
                    dr[8] = array.Appraiser2;
                    dr[9] = array.Reviewer1;
                    dr[10] = array.Reviewer2;
                    dr[11] = array.GroupHead;
                    dr[12] = array.Stage;
                    dr[13] = array.EmployeeEmail;
                    dr[14] = array.Appraiser1Email;
                    dr[15] = array.Appraiser2Email;
                    dr[16] = array.Reviewer1Email;
                    dr[17] = array.Reviewer2Email;
                    dr[18] = array.GroupHeadEmail;
                    dt.Rows.Add(dr);
                }
                int? AppraisalYear = app.AppraisalReportViewModelList.Select(x => x.AppraisalYearID).FirstOrDefault();
                var totalRecords = "Total Employees :" + app.AppraisalReportViewModelList.Where(x => x.AppraisalYearID == AppraisalYear).Count();
                var Completed = "Completed:" + app.AppraisalReportViewModelList.Where(x => x.AppraisalStageID == 7 && x.AppraisalYearID == AppraisalYear).Count();
                var pending = "Pending:" + app.AppraisalReportViewModelList.Where(x => x.AppraisalStageID != 7 && x.AppraisalYearID == AppraisalYear).Count();
                dt.Rows.Add(new[] { totalRecords, Completed, pending });
                gv.DataSource = dt;
            }
            else
                gv.DataSource = app.IndividualDevelopmentStatusReportList;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            if (app.AppraisalReportViewModelList != null)
                Response.AddHeader("content-disposition", "attachment; filename=AppraisalReport.xls");
            else
                Response.AddHeader("content-disposition", "attachment; filename=IndividualDevelopmentReport.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        [HttpGet]
        public ActionResult ConfigureNewYear()
        {
            try
            {
                ConfigureYearModel configureYearModel = new ConfigureYearModel();
                string EmployeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                configureYearModel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                configureYearModel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                configureYearModel.SearchedUserDetails.EmployeeCode = EmployeeCode;
                EmployeeDAL DAL = new EmployeeDAL();
                configureYearModel.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
                tbl_Appraisal_YearMaster currentYearDetails = appraisalDAL.GetCurrentYearDetails();
                if (currentYearDetails != null)
                {
                    configureYearModel.Year = currentYearDetails.AppraisalYearID.ToString();
                }
                configureYearModel.AppraisalYearList = appraisalDAL.GetAppraisalYearList();
                return PartialView("_ConfigureNewYear", configureYearModel);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult UpdateAppraisalYearDetails(ConfigureYearModel model)
        {
            try
            {
                bool status = false;
                if (model.Year != null)
                {
                    status = appraisalDAL.UpdateAppraisalYearDetails(model);
                }
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ViewYearRecords()
        {
            try
            {
                ViewYearModel confirmationmodel = new ViewYearModel();
                string EmployeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                confirmationmodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                confirmationmodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                confirmationmodel.SearchedUserDetails.EmployeeCode = EmployeeCode;
                EmployeeDAL DAL = new EmployeeDAL();
                confirmationmodel.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
                tbl_Appraisal_YearMaster currentYearDetails = appraisalDAL.GetCurrentYearDetails();
                if (currentYearDetails != null)
                {
                    confirmationmodel.CurrentYearID = currentYearDetails.AppraisalYearID;
                    confirmationmodel.CurrentYearName = currentYearDetails.AppraisalYear;
                }
                else
                {
                    confirmationmodel.CurrentYearID = 0;
                    confirmationmodel.CurrentYearName = "";
                }

                confirmationmodel.PastYearsList = appraisalDAL.GetPastYearDetailsList();
                return PartialView("_ViewAppraisalRecords", confirmationmodel);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult ConfigureAppraisalDetails()
        {
            try
            {
                AppraisalYearModel configModel = new AppraisalYearModel();
                string EmployeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                configModel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                configModel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                configModel.SearchedUserDetails.EmployeeCode = EmployeeCode;
                EmployeeDAL DAL = new EmployeeDAL();
                configModel.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
                return PartialView("_ConfigureAppraisalDetails", configModel);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult NewAppraisalYears()
        {
            try
            {
                AppraisalYearModel addAppYearModel = new AppraisalYearModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                string EmployeeCode = Membership.GetUser().UserName;
                addAppYearModel.SearchedUserDetails = new SearchedUserDetails();
                addAppYearModel.SearchedUserDetails.EmployeeCode = EmployeeCode;
                addAppYearModel.SearchedUserDetails.EmployeeId = employeeDAL.GetEmployeeID(EmployeeCode);
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                addAppYearModel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                addAppYearModel.AppraisalYearList = appraisalDAL.GetAllAppraisalYearList();
                addAppYearModel.TotalAppraisalYear = addAppYearModel.AppraisalYearList.Count;
                return PartialView("_AddAppraisalYears", addAppYearModel);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult ConfigureAppraisalCategory()
        {
            try
            {
                AppraisalCategoriesModel categoryModel = new AppraisalCategoriesModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                string EmployeeCode = Membership.GetUser().UserName;
                categoryModel.SearchedUserDetails = new SearchedUserDetails();
                categoryModel.SearchedUserDetails.EmployeeCode = EmployeeCode;
                categoryModel.SearchedUserDetails.EmployeeId = employeeDAL.GetEmployeeID(EmployeeCode);
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                categoryModel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                categoryModel.AppraisalCategoryList = appraisalDAL.GetAllAppraisalCategoryList();
                categoryModel.TotalAppraisalCategory = categoryModel.AppraisalCategoryList.Count;
                return PartialView("_ConfigureAppraisalCategories", categoryModel);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult AddEditAppraisalYear(AppraisalYearModel model)
        {
            try
            {
                AppraisalProcessResponse appraisalResponse = new AppraisalProcessResponse();
                appraisalResponse.isAdded = false;
                if (model.NewAppraisalYear != null)
                {
                    appraisalResponse = appraisalDAL.AddEditNewAppraisalYears(model);
                }
                return Json(new { isAdded = appraisalResponse.isAdded, isExisted = appraisalResponse.isExisted }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddEditAppraisalCategory(AppraisalCategoriesModel model)
        {
            try
            {
                AppraisalProcessResponse appraisalResponse = new AppraisalProcessResponse();
                appraisalResponse.isAdded = false;
                if (model.NewAppraisalCategory != null)
                {
                    appraisalResponse = appraisalDAL.AddEditNewAppraisalCategory(model);
                }
                return Json(new { isAdded = appraisalResponse.isAdded, isExisted = appraisalResponse.isExisted }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddEditNewAppraisalYear(int appraisalYearId, string appraisalYearName, int employeeId)
        {
            try
            {
                AppraisalYearModel editAppYearModel = new AppraisalYearModel();
                editAppYearModel.SearchedUserDetails = new SearchedUserDetails();
                tbl_Appraisal_YearMaster appraisalYearDetails = appraisalDAL.getAppraisalYearDetails(appraisalYearId);
                if (appraisalYearDetails != null)
                {
                    editAppYearModel.NewAppraisalYear = appraisalYearDetails.AppraisalYear;
                    editAppYearModel.AppraisalYearID = appraisalYearDetails.AppraisalYearID;
                }
                else
                {
                    editAppYearModel.NewAppraisalYear = appraisalYearName;
                    editAppYearModel.AppraisalYearID = appraisalYearId;
                }
                editAppYearModel.SearchedUserDetails.EmployeeId = employeeId;
                return PartialView("_EditAppraisalYear", editAppYearModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult AddEditNewAppraisalCategory(int appraisalCategoryId, string appraisalCategoryName, int employeeId)
        {
            try
            {
                AppraisalCategoriesModel editAppCategoryModel = new AppraisalCategoriesModel();
                editAppCategoryModel.SearchedUserDetails = new SearchedUserDetails();
                Tbl_Appraisal_CategoryMaster appraisalCategoryDetails = appraisalDAL.getAppraisalCategoryDetails(appraisalCategoryId);
                if (appraisalCategoryDetails != null)
                {
                    editAppCategoryModel.CategoryID = appraisalCategoryDetails.CategoryID;
                    editAppCategoryModel.NewAppraisalCategory = appraisalCategoryDetails.Category;
                    editAppCategoryModel.NewAppCategoryDescription = appraisalCategoryDetails.Description;
                    editAppCategoryModel.ExistingAppraisalCategory = appraisalCategoryDetails.Category;
                }
                else
                {
                    editAppCategoryModel.CategoryID = appraisalCategoryId;
                    editAppCategoryModel.NewAppraisalCategory = appraisalCategoryName;
                    editAppCategoryModel.NewAppCategoryDescription = "";
                    editAppCategoryModel.ExistingAppraisalCategory = "";
                }
                editAppCategoryModel.SearchedUserDetails.EmployeeId = employeeId;
                return PartialView("_AddEditConfigureAppraisalCategories", editAppCategoryModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult DeleteAppraisalYear(int appraisalYearId)
        {
            try
            {
                AppraisalProcessResponse response = new AppraisalProcessResponse();
                response.isDeleted = false;
                if (appraisalYearId != 0)
                {
                    response = appraisalDAL.DeleteAppraisalYear(appraisalYearId);
                }
                return Json(new { isDeleted = response.isDeleted, isExisted = response.isExisted }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// To Delete Appraisal Categories
        /// </summary>
        /// <param name="appraisalCategoryId">Primary Key of Appraisal Category</param>
        /// <returns>Returns Bool value or String value as per specific Exception</returns>
        [HttpPost]
        public ActionResult DeleteAppraisalCategory(int appraisalCategoryId)
        {
            try
            {
                AppraisalProcessResponse response = new AppraisalProcessResponse();
                response.isDeleted = false;
                if (appraisalCategoryId != 0)
                {
                    response = appraisalDAL.DeleteAppraisalCategory(appraisalCategoryId);
                }
                return Json(new { isDeleted = response.isDeleted }, JsonRequestBehavior.AllowGet);
            }
            catch (UpdateException)
            {
                return Json(new { isDeleted = "UpdateException" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AppraisalParameters(int? AppraisalYearID)
        {
            try
            {
                AppraisalParametersModel model = new AppraisalParametersModel();
                string EmployeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                model.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                model.SearchedUserDetails.EmployeeCode = EmployeeCode;
                EmployeeDAL DAL = new EmployeeDAL();
                model.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                List<AppraisalParameterMaster> appraisalParameterMaster = configAppDAL.GetParameterMaster(AppraisalYearID);
                if (appraisalParameterMaster != null)
                {
                    model.ParameterRecordsCount = appraisalParameterMaster.Count;
                    model.AppraisalParameterMaster = appraisalParameterMaster;
                }
                model.AppraisalYearID = AppraisalYearID.HasValue ? AppraisalYearID.Value : 0;

                List<tbl_Appraisal_AppraisalMaster> appraisalDetails = configAppDAL.GetAppraisalInitiationDetails(AppraisalYearID);
                if (appraisalDetails.Count > 0)
                    ViewBag.IsInitiated = true;

                return PartialView("_AppraisalParameters", model);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult AppraisalRatingScale(int? AppraisalYearID)
        {
            try
            {
                AppraisalRatingModel appraisalmodel = new AppraisalRatingModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                appraisalmodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                appraisalmodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);

                appraisalmodel.SearchedUserDetails.EmployeeId = employeeID;
                appraisalmodel.SearchedUserDetails.EmployeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();

                HRMSDBEntities dbContext = new HRMSDBEntities();

                var statuscount = dbContext.tbl_Appraisal_RatingMaster.Where(x => x.AppraisalYearID == AppraisalYearID).FirstOrDefault();

                List<tbl_PA_Rating_Master> _tbl_PA_Rating_Master = (from e in dbContext.tbl_PA_Rating_Master
                                                                    select e).ToList();
                tbl_Appraisal_YearMaster _selectedYear = dbContext.tbl_Appraisal_YearMaster.Where(y => y.AppraisalYearID == AppraisalYearID).FirstOrDefault();

                if (_selectedYear.AppYearRatingsFlag == null)
                {
                    if (_tbl_PA_Rating_Master != null)
                    {
                        foreach (tbl_PA_Rating_Master i in _tbl_PA_Rating_Master)
                        {
                            tbl_Appraisal_RatingMaster Obj_tbl_Appraisal_RatingMaster = new tbl_Appraisal_RatingMaster();
                            Obj_tbl_Appraisal_RatingMaster.Rating = i.Rating;
                            Obj_tbl_Appraisal_RatingMaster.Description = i.Description;
                            Obj_tbl_Appraisal_RatingMaster.Percentage = i.Percentage;
                            Obj_tbl_Appraisal_RatingMaster.AdjustmentFactor = i.AdjustmentFactor;
                            Obj_tbl_Appraisal_RatingMaster.SetAsMinimumLimit = i.SetAsMinimumLimit;
                            Obj_tbl_Appraisal_RatingMaster.AppraisalYearID = Convert.ToInt32(AppraisalYearID);

                            dbContext.tbl_Appraisal_RatingMaster.AddObject(Obj_tbl_Appraisal_RatingMaster);
                            dbContext.SaveChanges();
                        }

                        _selectedYear.AppYearRatingsFlag = 1;
                        dbContext.SaveChanges();
                    }
                }

                List<AppraisalRatingScales> appraisalRatingMaster = configAppDAL.GetAppraisalRatingsMaster(AppraisalYearID);
                if (appraisalRatingMaster != null)
                {
                    appraisalmodel.RecordsCount = appraisalRatingMaster.Count;
                    appraisalmodel.AppraisalRatingScale = appraisalRatingMaster;
                }
                appraisalmodel.AppraisalYearID = AppraisalYearID.HasValue ? AppraisalYearID.Value : 0;

                List<tbl_Appraisal_AppraisalMaster> appraisalDetails = configAppDAL.GetAppraisalInitiationDetails(AppraisalYearID);
                if (appraisalDetails.Count > 0)
                    ViewBag.IsInitiated = true;

                return PartialView("_AppraisalRatingScale", appraisalmodel);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult AppraisalStrengthImprove(int? AppraisalYearID)
        {
            try
            {
                AppraisalStrengthImproveModel model = new AppraisalStrengthImproveModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                model.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                model.SearchedUserDetails.EmployeeId = employeeID;
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                model.SearchedUserDetails.EmployeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                ViewBag.maxStrength = Convert.ToInt32(configAppDAL.getMaxStrengthLimit(AppraisalYearID));
                ViewBag.minStrength = Convert.ToInt32(configAppDAL.getMinStrengthLimit(AppraisalYearID));
                ViewBag.minStrengthPlusOne = ViewBag.minStrength + 1;
                if (AppraisalYearID != null)
                {
                    tbl_Appraisal_SrengthImprovement_Limit _SrengthImprovement = configAppDAL.getSrengthImprovementLimit(AppraisalYearID);
                    if (_SrengthImprovement != null)
                    {
                        if ((_SrengthImprovement.StrengthLimit.HasValue ? _SrengthImprovement.StrengthLimit.Value : 0) <= ViewBag.maxStrength)
                        {
                            model.StrengthLimit = _SrengthImprovement.StrengthLimit.Value;
                            model.ImprovementLimit = _SrengthImprovement.ImprovementLimit.Value;
                        }
                    }
                }
                model.AppraisalYearID = AppraisalYearID.HasValue ? AppraisalYearID.Value : 0;
                List<tbl_Appraisal_AppraisalMaster> appraisalDetails = configAppDAL.GetAppraisalInitiationDetails(AppraisalYearID);
                if (appraisalDetails.Count > 0)
                    ViewBag.IsInitiated = true;
                return PartialView("_AppraisalStrengthImprove", model);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult AppraiserReviewerMapping(int? AppraisalYearID)
        {
            try
            {
                AppraiserReviewerMappingModel model = new AppraiserReviewerMappingModel();
                string EmployeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                model.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                model.SearchedUserDetails.EmployeeCode = EmployeeCode;
                EmployeeDAL DAL = new EmployeeDAL();
                model.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
                ConfigurationDAL configDAL = new ConfigurationDAL();
                model.AppraisalYearID = AppraisalYearID.HasValue ? AppraisalYearID.Value : 0;
                return PartialView("_AppraiserReviewerMapping", model);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// It is used to Open view for Adding/Editing Appraisal Parameter for current Appraisal Year
        /// </summary>
        /// <param name="orderID">orderID to check its uniqueness and AppraisalYearID to check current year</param>
        /// <param name="AppraisalYearID"></param>
        /// <returns>returns Partial view _AddAppraisalParameter</returns>
        [HttpGet]
        public ActionResult AddAppraisalParameter(int? orderID, int? AppraisalYearID)
        {
            try
            {
                AddAppraisalParaModel model = new AddAppraisalParaModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                model.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                model.SearchedUserDetails.EmployeeCode = Membership.GetUser().UserName;
                EmployeeDAL DAL = new EmployeeDAL();
                model.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(Membership.GetUser().UserName);
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                model.AppraisalYearID = AppraisalYearID.HasValue ? AppraisalYearID.Value : 0;
                if (orderID != null)
                {
                    tbl_Appraisal_ParameterMaster parameterMaster = configAppDAL.getParameter(orderID, AppraisalYearID);
                    model.Parameter = parameterMaster.Parameter;
                    model.OrderNo = parameterMaster.OrderNo;
                    model.SelectedOrderNo = parameterMaster.OrderNo;
                    model.category = Convert.ToString(parameterMaster.ParameterCategoryID);
                    model.BehavioralIndicators = parameterMaster.BehavioralIndicators;
                    model.ParameterDescription = parameterMaster.ParameterDescription;
                    model.ParameterID = parameterMaster.ParameterID;
                    model.AppraisalYearID = parameterMaster.AppraisalYearID;
                }
                model.ParameterCategoryList = configAppDAL.getCategoryList();

                List<tbl_Appraisal_AppraisalMaster> appraisalDetails = configAppDAL.GetAppraisalInitiationDetails(AppraisalYearID);
                if (appraisalDetails.Count > 0)
                    ViewBag.IsInitiated = true;
                return PartialView("_AddAppraisalParameter", model);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors.." });
            }
        }

        [HttpGet]
        public ActionResult EligibilityCriteria(int AppraisalYearID)
        {
            try
            {
                EligibilityCriteriaModel eligibileModel = new EligibilityCriteriaModel();
                string EmployeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                eligibileModel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                eligibileModel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                eligibileModel.SearchedUserDetails.EmployeeCode = EmployeeCode;
                EmployeeDAL DAL = new EmployeeDAL();
                eligibileModel.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
                eligibileModel.allSuccessEmployeeList = new List<AllEligibileEmployee>();
                eligibileModel.Mail = new EmployeeMailTemplate();
                eligibileModel.ApprasialYearID = AppraisalYearID;
                eligibileModel.allEmployeeList = appraisalDAL.GetAllEligibileEmployees(AppraisalYearID);
                eligibileModel.allEmployeeListCount = eligibileModel.allEmployeeList.Count;
                eligibileModel.allConfirmationDateEmployeeList = new List<AllEligibileEmployee>();
                eligibileModel.DesignationList = appraisalDAL.GetDesignationList();
                tbl_Appraisal_YearMaster performancePeriod = appraisalDAL.getAppraisalYearDetails(AppraisalYearID);
                if (performancePeriod != null)
                {
                    if (DateTime.Now > performancePeriod.AppraisalYearFrozenOn)
                        ViewBag.IsPerformanceYearFrozen = true;
                }
                return PartialView("_EligibilityCriteria", eligibileModel);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult GetIneligibileEmployees(List<int> AppraisalEmployeeId, int AppraisalYearID)
        {
            try
            {
                AppraisalProcessResponse response = new AppraisalProcessResponse();
                if (AppraisalYearID != 0)
                {
                    response = appraisalDAL.GetIneligibileEmployeesList(AppraisalEmployeeId, AppraisalYearID);
                }
                return Json(new { failedEmployeeID = response.failedEmployeeID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult ConfirmationDateEligibilityCriteria(int AppraisalYearID, DateTime ConfirmationDate)
        {
            try
            {
                EligibilityCriteriaModel eligibileModel = new EligibilityCriteriaModel();
                string EmployeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                eligibileModel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                eligibileModel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                eligibileModel.SearchedUserDetails.EmployeeCode = EmployeeCode;
                EmployeeDAL DAL = new EmployeeDAL();
                eligibileModel.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);
                eligibileModel.allSuccessEmployeeList = new List<AllEligibileEmployee>();
                eligibileModel.Mail = new EmployeeMailTemplate();
                eligibileModel.ApprasialYearID = AppraisalYearID;
                eligibileModel.allConfirmationDateEmployeeList = appraisalDAL.GetAllEligibileConfirmDateEmployees(AppraisalYearID, ConfirmationDate);
                eligibileModel.allConfirmationDateEmployeeListCount = eligibileModel.allConfirmationDateEmployeeList.Count;
                return PartialView("_AllConfirmDateEmployees", eligibileModel);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult InitiateAllEmpApp(string EmployeeId, int AppraisalYearID)
        {
            try
            {
                AppraisalProcessResponse response = new AppraisalProcessResponse();
                response.isAdded = false;
                if (AppraisalYearID != null)
                {
                    string EmployeeIDWithcomma = EmployeeId.TrimEnd(',');
                    string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                    int[] myIntEmployeeID = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                    response = appraisalDAL.InitiateAllEmpAppPro(myIntEmployeeID, AppraisalYearID);
                }
                return Json(new { isAdded = response.isAdded, failedEmployeeID = response.failedEmployeeID, successEmployeeID = response.successEmployeeID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ShowSuccessfulIniEmployee(string successEmployeeID, int AppraisalYearID)
        {
            try
            {
                EligibilityCriteriaModel succesEmpModel = new EligibilityCriteriaModel();
                string EmployeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(EmployeeCode);
                succesEmpModel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                succesEmpModel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                succesEmpModel.SearchedUserDetails.EmployeeCode = EmployeeCode;
                EmployeeDAL DAL = new EmployeeDAL();
                succesEmpModel.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(EmployeeCode);

                if (successEmployeeID != null)
                {
                    string EmployeeIDWithcomma = successEmployeeID.TrimEnd(',');
                    string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                    int[] myIntEmployeeID = Array.ConvertAll(EmployeeIDArray, s => int.Parse(s));
                    succesEmpModel.ApprasialYearID = AppraisalYearID;

                    succesEmpModel.allSuccessEmployeeList = appraisalDAL.GetAllSuccessEmployees(myIntEmployeeID, AppraisalYearID);
                }
                return PartialView("_AllSuccesfulIniEmployee", succesEmpModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult AppraisalDesignations(int? parameterID)
        {
            try
            {
                Session["parameterID"] = parameterID;
                var sessionParameterID = (int)Session["parameterID"];
                AppraisalDesignationsModel designationmodel = new AppraisalDesignationsModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                designationmodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                designationmodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                if (parameterID != null)
                {
                    List<AppraisalDesignation> parameterDesignationMapping = configAppDAL.getAppraisalDesignation(parameterID);
                    designationmodel.AppraisalDesignations = parameterDesignationMapping;
                }
                return PartialView("_AppraisalDesignations", designationmodel);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteParameter(List<int> collection, int? AppraisalYearID)
        {
            try
            {
                AppraisalProcessResponse response = new AppraisalProcessResponse();
                response.isDeleted = false;
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                if (collection.Count != 0)
                {
                    response = configAppDAL.DeleteParameter(collection, AppraisalYearID);
                }
                return
                    Json(new { status = response.isDeleted, ParamterwithDesignation = response.ParamterwithDesignation },
                        JsonRequestBehavior.AllowGet);
            }
            catch (UpdateException)
            {
                return Json(new { status = "UpdateException" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// It is used to Add/Edit new Appraisal Parameter for Current Year.
        /// </summary>
        /// <param name="model">Object of AddAppraisalParaModel class</param>
        /// <returns>Json objects containing resultMesssage,status,orderNumber and AppraisalYearID</returns>
        [HttpPost]
        public ActionResult AddAppraisalParameter(AddAppraisalParaModel model)
        {
            try
            {
                bool success = false;
                StatusForOrderNoAndParameter status = new StatusForOrderNoAndParameter();
                string result = null;
                int OrderNumber = 0;
                int? AppraisalYearID = 0;
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                status = configAppDAL.SaveParameter(model);
                if (model.IsAddnew)
                {
                    if (status.IsOrderNumber == true && status.IsParameter == true)
                    {
                        result = "addnew";
                        return Json(new { resultMesssage = result, status = true }, JsonRequestBehavior.AllowGet);
                    }
                    else if (status.IsParameter == false)
                    {
                        result = "ErrorInParamter";
                    }
                    else if (status.IsOrderNumber == false)
                    {
                        result = "ErrorInOrderNumber";
                    }
                    return Json(new { resultMesssage = result, status = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (status.IsOrderNumber == true && status.IsParameter == true)
                    {
                        result = "Saved";
                        OrderNumber = model.OrderNo.HasValue ? model.OrderNo.Value : 0;
                        AppraisalYearID = model.AppraisalYearID;
                        success = true;
                    }
                    else if (status.IsParameter == false)
                    {
                        result = "ErrorInParamter";
                        success = false;
                    }
                    else if (status.IsOrderNumber == false)
                    {
                        result = "ErrorInOrderNumber";
                        success = false;
                    }

                    return Json(new { resultMesssage = result, status = success, orderNumber = OrderNumber, AppraisalYearID = model.AppraisalYearID }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteDesignation(List<int> collection, int parameterID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                if (collection.Count != 0)
                {
                    success = configAppDAL.DeleteDesignations(collection, parameterID);
                    result = "Deleted";
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SelectDesignations(string designationID)
        {
            try
            {
                int parameterID = (int)Session["parameterID"];

                AppraisalDesignationsModel appraisaldesignation = new AppraisalDesignationsModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                appraisaldesignation.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                appraisaldesignation.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                List<AppraisalDesignation> NewSelectDesignation = new List<AppraisalDesignation>();
                if (designationID != "")
                {
                    string designationIDWithcomma = designationID.TrimEnd(',');
                    string[] roleidArray = designationIDWithcomma.Split(',');
                    int[] myInts = Array.ConvertAll(roleidArray, s => int.Parse(s));
                    NewSelectDesignation = configAppDAL.getNewSelectDesignation(myInts, parameterID);
                }
                else
                {
                    HRMSDBEntities dbContext = new HRMSDBEntities();
                    NewSelectDesignation = (from d in dbContext.tbl_PM_DesignationMaster
                                            orderby d.DesignationName ascending
                                            select new AppraisalDesignation
                                     {
                                         ParameterID = parameterID,
                                         DesignationID = d.DesignationID,
                                         Designation = d.DesignationName
                                     }).ToList();
                }
                appraisaldesignation.AppraisalDesignations = NewSelectDesignation;
                return PartialView("_SelectDesignations", appraisaldesignation);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveNewDesignation(List<int> designationID, int parameterID)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                if (designationID.Count != 0)
                {
                    success = configAppDAL.SaveNewDesignation(designationID, parameterID);
                    result = "Saved";
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult setStrengthImprovementLimit(AppraisalStrengthImproveModel addStrengthImproveLimit)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                success = configAppDAL.setStrengthImprovementLimit(addStrengthImproveLimit);

                if (success)
                    result = "Saved";
                else
                    result = "Error";
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public string UploadFileLocationAppraisal
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadAppraisalFileLocation"];
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadAppraisalDocument(HttpPostedFileBase doc, AppraiserReviewerMappingModel model)
        {
            ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();

            bool uploadStatus = false;

            if (doc.ContentLength > 0)
            {
                string uploadsPath = HttpContext.Server.MapPath(UploadFileLocationAppraisal);
                uploadsPath = Path.Combine(uploadsPath, GetUploadTypeSelectedText(1));
                model.DocumentID = null;
                string fileName = Path.GetFileName(doc.FileName);
                try
                {
                    string connectionString = "";
                    string extension = System.IO.Path.GetExtension(Request.Files["doc"].FileName);

                    if (!Directory.Exists(uploadsPath))
                        Directory.CreateDirectory(uploadsPath);

                    string filePath = Path.Combine(uploadsPath, System.IO.Path.GetFileName(Request.Files["doc"].FileName));
                    Request.Files["doc"].SaveAs(filePath);

                    if (extension == ".xls")
                    {
                        connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (extension == ".xlsx")
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }

                    OleDbConnection excelConnection = new OleDbConnection(connectionString);
                    OleDbCommand cmd = new OleDbCommand("Select [Employee Code],[Appraiser1],[Appraiser2],[Reviewer1],[Reviewer2],[GroupHead] from [Sheet2$]", excelConnection);

                    excelConnection.Open();

                    OleDbDataReader dReader;
                    dReader = cmd.ExecuteReader();

                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();

                    dt.Load(dReader);
                    ds.Tables.Add(dt);

                    ConfigurationAppraisalDAL uploadDAL = new ConfigurationAppraisalDAL();

                    int flag = 0;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["Employee Code"].ToString() != "")
                        {
                            if ((ds.Tables[0].Rows[i]["Appraiser1"].ToString() != "") &&
                                (ds.Tables[0].Rows[i]["Reviewer1"].ToString() != "") &&
                                (ds.Tables[0].Rows[i]["GroupHead"].ToString() != ""))
                            {
                                flag = 0;
                            }
                            else
                            {
                                flag = 1;
                                break;
                            }
                        }
                    }
                    if (flag == 0)
                    {
                        uploadDAL.GetExcelData(ds, model);
                        uploadStatus = true;

                        excelConnection.Close();

                        IAppraisalDocuments document = null;

                        if (
                            !configAppDAL.IsAppraisalDocumentExists(Path.GetFileName(doc.FileName), model.UploadTypeId,
                                model.AppraisalYearID))
                        {
                            // Insert new record to parent
                            document = new tbl_ApprisalDocuments();
                            document.FileName = Path.GetFileName(doc.FileName);
                            ((tbl_ApprisalDocuments)document).FileDescription = model.FileDescription;
                            ((tbl_ApprisalDocuments)document).UploadTypeId = 1;
                        }
                        else
                        {
                            // Insert new record to child

                            document = new tbl_ApprisalDocumentDetail();
                            int documentID = 0;
                            string newNameForDocument =
                                configAppDAL.GetNewNameForApprisalDocument(Path.GetFileName(doc.FileName), 1,
                                    out documentID);
                            fileName = newNameForDocument;
                            document.DocumentId = documentID;
                            document.FileName = newNameForDocument;
                            ((tbl_ApprisalDocumentDetail)document).UploadTypeId = 1;
                        }
                        document.AppraisalYearID = model.AppraisalYearID;
                        document.FilePath = uploadsPath;

                        document.FileDescription = model.FileDescription;
                        document.UploadedBy = int.Parse(HttpContext.User.Identity.Name);
                        document.UploadedDate = DateTime.Now;
                        string filePath1 = Path.Combine(uploadsPath, document.FileName);
                        Request.Files["doc"].SaveAs(filePath1);
                        configAppDAL.UploadAppraisalDocument(document);

                        uploadStatus = true;
                    }
                }
                catch (Exception)
                {
                    //throw;
                }
            }
            return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadAppraisalUploadDetails(int page, int rows, int appraisalYearID)
        {
            ConfigurationAppraisalDAL uploads = new ConfigurationAppraisalDAL();
            try
            {
                List<AppraiserReviewerMappingModel> Result = uploads.GetAppraisalDocumentForDispay(page, rows, appraisalYearID);
                int totalCount = uploads.GetAppraisalDocumentForDispayTotalCount(appraisalYearID);
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = Result
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Rating

        [HttpPost]
        public ActionResult AddAppraisalRatingScales(AddAppraisalRatingScale model)
        {
            try
            {
                string result = null;
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                string UserRole = Commondal.GetMaxRoleForUser(role);
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                AppraisalRatingResponse appRatingResponse = new AppraisalRatingResponse();
                appRatingResponse = configAppDAL.SaveAppraisalRatingScales(model);
                return Json(new { status = appRatingResponse.isRatingAdded, isRatingScalePresent = appRatingResponse.isRatingScalePresent, isRatingPresent = appRatingResponse.isRatingPresent }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddAppraisalRatingScales(int? RatingID, int? AppraisalYearID)
        {
            try
            {
                AddAppraisalRatingScale addNewRating = new AddAppraisalRatingScale();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                addNewRating.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                addNewRating.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                addNewRating.SearchedUserDetails.EmployeeId = employeeID;
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                addNewRating.SearchedUserDetails.EmployeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                addNewRating.AppraisalYearID = AppraisalYearID.HasValue ? AppraisalYearID.Value : 0;
                if (RatingID != null)
                {
                    tbl_Appraisal_RatingMaster ratingScale = configAppDAL.getAppraisalRatingScaleDetails(RatingID);
                    addNewRating.Percentage = ratingScale.Percentage;
                    addNewRating.SelectedPercentage = ratingScale.Percentage;
                    addNewRating.Rating = ratingScale.Rating;
                    addNewRating.RatingID = ratingScale.RatingID;
                    addNewRating.Description = ratingScale.Description;
                    addNewRating.AdjustmentFactor = ratingScale.AdjustmentFactor;
                    addNewRating.AppraisalYearID = ratingScale.AppraisalYearID;
                }
                List<tbl_Appraisal_AppraisalMaster> appraisalDetails = configAppDAL.GetAppraisalInitiationDetails(AppraisalYearID);
                if (appraisalDetails.Count > 0)
                    ViewBag.IsInitiated = true;
                return PartialView("_AddAppraisalRatingScale", addNewRating);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        public ActionResult ConfigureAppraisalRatingScales(int? AppraisalYearID)
        {
            try
            {
                AppraisalRatingModel configratingApprisalRatingSale = new AppraisalRatingModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                configratingApprisalRatingSale.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                configratingApprisalRatingSale.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL dal = new EmployeeDAL();
                int employeeID = dal.GetEmployeeID(Membership.GetUser().UserName);
                string employeeCode = personalDAL.getEmployeeCode(employeeID);
                configratingApprisalRatingSale.SearchedUserDetails.EmployeeId = employeeID;
                configratingApprisalRatingSale.SearchedUserDetails.EmployeeCode = personalDAL.getEmployeeCode(employeeID);
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                List<AppraisalRatingScales> appraisalRatingScale = configAppDAL.GetAppraisalRatingsMaster(AppraisalYearID);
                if (appraisalRatingScale != null)
                {
                    configratingApprisalRatingSale.RecordsCount = appraisalRatingScale.Count;
                    configratingApprisalRatingSale.AppraisalRatingScale = appraisalRatingScale;
                }
                configratingApprisalRatingSale.AppraisalYearID = AppraisalYearID.HasValue ? AppraisalYearID.Value : 0;
                return PartialView("_AppraisalRatingScale", configratingApprisalRatingSale);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteAppraisalRatingScales(List<int> collection)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationAppraisalDAL configAppDAL = new ConfigurationAppraisalDAL();
                if (collection.Count != 0)
                {
                    success = configAppDAL.DeleteAppraisalRatingScales(collection);
                    result = "Deleted";
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AppraisalSendMail(string successEmpIDs, int loggedinEmpID, string status, string comments, int appraisalID)
        {
            try
            {
                EligibilityCriteriaModel model = new EligibilityCriteriaModel();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                AppraisalDAL appDAL = new AppraisalDAL();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                string EmployeeIDWithcomma = successEmpIDs.TrimEnd(',');
                string[] EmployeeIDArray = EmployeeIDWithcomma.Split(',');
                string[] EmployeeIDArrays = new string[EmployeeIDArray.Length];
                for (int i = 0; i < EmployeeIDArray.Length; i++)
                {
                    if (EmployeeIDArray[i] != "0")
                        EmployeeIDArrays[i] = EmployeeIDArray[i];
                }
                string[] EmployeeIDs = EmployeeIDArrays.Where(s => !String.IsNullOrEmpty(s)).ToArray();
                int[] myIntEmployeeID = Array.ConvertAll(EmployeeIDs, s => int.Parse(s));
                int templateId;
                model.Mail = new EmployeeMailTemplate();
                List<HRMS_tbl_PM_Employee> toEmployeeDetailsList = appraisalDAL.GetEmployeeDetailsList(myIntEmployeeID);

                if (status == "" || status == null || status == "EscalateToHRByAppraiser")
                {
                    if (status != "EscalateToHRByAppraiser")
                    {
                        string[] users = Roles.GetUsersInRole("HR Admin");
                        foreach (string user in users)
                        {
                            HRMS_tbl_PM_Employee employee = dal.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(user));
                            if (employee == null)
                                model.Mail.To = model.Mail.To + string.Empty;
                            else
                                model.Mail.To = model.Mail.To + employee.EmailID + ";";
                        }
                    }
                    else
                    {
                        model.Mail.To = "v2appraisal@v2solutions.com";
                    }
                }
                int appraiser1ID = 0;
                int appraiser2ID = 0;
                int reviewer1ID = 0;
                int reviewer2ID = 0;
                int employeeIdOfAppraisee;
                tbl_Appraisal_AppraisalMaster appraiseeDetails = new tbl_Appraisal_AppraisalMaster();
                HRMS_tbl_PM_Employee empDetailsOfAppraisee = new HRMS_tbl_PM_Employee();
                HRMS_tbl_PM_Employee empDetailsOfAppraiser1 = new HRMS_tbl_PM_Employee();
                HRMS_tbl_PM_Employee empDetailsOfAppraiser2 = new HRMS_tbl_PM_Employee();
                HRMS_tbl_PM_Employee empDetailsOfReviewer1 = new HRMS_tbl_PM_Employee();
                HRMS_tbl_PM_Employee empDetailsOfReviewer2 = new HRMS_tbl_PM_Employee();
                if (appraisalID != 0)
                {
                    appraiseeDetails = appDAL.GetAppraisalDetails(appraisalID);
                    appraiser1ID = appraiseeDetails.Appraiser1.HasValue ? appraiseeDetails.Appraiser1.Value : 0;
                    appraiser2ID = appraiseeDetails.Appraiser2.HasValue ? appraiseeDetails.Appraiser2.Value : 0;
                    reviewer1ID = appraiseeDetails.Reviewer1.HasValue ? appraiseeDetails.Reviewer1.Value : 0;
                    reviewer2ID = appraiseeDetails.Reviewer2.HasValue ? appraiseeDetails.Reviewer2.Value : 0;
                    employeeIdOfAppraisee = appraiseeDetails.EmployeeID;
                    empDetailsOfAppraisee = employeeDAL.GetEmployeeDetails(employeeIdOfAppraisee);
                    empDetailsOfAppraiser1 = employeeDAL.GetEmployeeDetails(appraiser1ID);
                    empDetailsOfAppraiser2 = employeeDAL.GetEmployeeDetails(appraiser2ID);
                    empDetailsOfReviewer1 = employeeDAL.GetEmployeeDetails(reviewer1ID);
                    empDetailsOfReviewer2 = employeeDAL.GetEmployeeDetails(reviewer2ID);
                }
                HRMS_tbl_PM_Employee fromEmployeeDetails = employeeDAL.GetEmployeeDetails(loggedinEmpID);
                if (fromEmployeeDetails != null)
                {
                    model.Mail.From = fromEmployeeDetails.EmailID;

                    foreach (var item in toEmployeeDetailsList)
                    {
                        if (status == "" || status == null)
                        {
                            model.Mail.Bcc = model.Mail.Bcc + item.EmailID + ";";
                        }
                        else if (status == "EscalateToHRByAppraiser")
                        {
                            model.Mail.Cc = model.Mail.Cc + item.EmailID + ";";
                        }
                        else
                        {
                            model.Mail.To = model.Mail.To + item.EmailID + ";";
                        }
                    }
                    if (status == "canceled")
                    {
                        templateId = 43;
                    }
                    else if (status == "Submitted")
                    {
                        templateId = 44;
                    }
                    else if (status == "AppraiserApproved")
                    {
                        templateId = 45;
                    }
                    else if (status == "ReviewerApproved")
                    {
                        templateId = 46;
                    }
                    else if (status == "IDFInitiate")
                    {
                        templateId = 49;
                    }
                    else if (status == "rejectedByAppraiser")
                    {
                        templateId = 47;
                    }
                    else if (status == "rejectedByReviewer")
                    {
                        templateId = 48;
                    }
                    else if (status == "GroupHeadApproved")
                    {
                        templateId = 50;
                    }
                    else if (status == "IDFAppraiserSubmitted")
                    {
                        templateId = 51;
                    }
                    else if (status == "EscalateToHRByAppraiser")
                    {
                        templateId = 52;
                    }
                    else if (status == "AppraisalProcessCompletion")
                    {
                        templateId = 53;
                    }
                    else if (status == "disagreedByEmployee")
                    {
                        templateId = 54;
                    }
                    else
                        templateId = 41;

                    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                    foreach (var emailTemplate in template)
                    {
                        model.Mail.Subject = emailTemplate.Subject;
                        model.Mail.Message = emailTemplate.Message;
                    }

                    if (status == "ReviewerApproved" || status == "AppraisalProcessCompletion")
                    {
                        string[] users = Roles.GetUsersInRole("HR Admin");
                        foreach (string user in users)
                        {
                            HRMS_tbl_PM_Employee employee = dal.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(user));
                            if (employee == null)
                                model.Mail.Cc = model.Mail.Cc + string.Empty;
                            else
                                model.Mail.Cc = model.Mail.Cc + employee.EmailID + ";";
                        }
                    }
                    if (status == "rejectedByReviewer")
                    {
                        if (reviewer1ID != loggedinEmpID)
                        {
                            model.Mail.Cc = empDetailsOfReviewer1.EmailID;
                        }
                        else
                        {
                            if (empDetailsOfReviewer2 != null)
                            {
                                model.Mail.Cc = empDetailsOfReviewer2.EmailID;
                            }
                        }
                    }
                    if (status == "rejectedByAppraiser")
                    {
                        if (appraiser1ID != loggedinEmpID)
                        {
                            model.Mail.Cc = empDetailsOfAppraiser1.EmailID;
                        }
                        else
                        {
                            if (empDetailsOfAppraiser2 != null)
                            {
                                model.Mail.Cc = empDetailsOfAppraiser2.EmailID;
                            }
                        }
                    }

                    if (status == "canceled")
                    {
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Name##", empDetailsOfAppraisee.EmployeeName);
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Code##", empDetailsOfAppraisee.EmployeeCode);
                        model.Mail.Message = model.Mail.Message.Replace("<br>", Environment.NewLine);
                        model.Mail.Message = model.Mail.Message.Replace("##logged in HR Admin##", fromEmployeeDetails.EmployeeName);
                        model.Mail.Message = model.Mail.Message.Replace("##HR Comments##", comments);
                    }
                    if (status == "rejectedByAppraiser")
                    {
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Name##", empDetailsOfAppraisee.EmployeeName);
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Code##", empDetailsOfAppraisee.EmployeeCode);
                        model.Mail.Message = model.Mail.Message.Replace("##Employee Name##", empDetailsOfAppraisee.EmployeeName);
                        model.Mail.Message = model.Mail.Message.Replace("<br>", Environment.NewLine);
                        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromEmployeeDetails.EmployeeName);
                        model.Mail.Message = model.Mail.Message.Replace("##Rejection comments entered by appraiser##", comments);
                    }
                    if (status == "rejectedByReviewer")
                    {
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Name##", empDetailsOfAppraisee.EmployeeName);
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Code##", empDetailsOfAppraisee.EmployeeCode);
                        model.Mail.Message = model.Mail.Message.Replace("##Employee Name##", empDetailsOfAppraisee.EmployeeName);
                        model.Mail.Message = model.Mail.Message.Replace("##Employee Code##", empDetailsOfAppraisee.EmployeeCode);
                        model.Mail.Message = model.Mail.Message.Replace("<br>", Environment.NewLine);
                        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromEmployeeDetails.EmployeeName);
                        model.Mail.Message = model.Mail.Message.Replace("##Rejection comments entered by reviewer##", comments);
                    }
                    if (status == "" || status == null || status == "GroupHeadApproved" || status == "IDFInitiate")
                    {
                        model.Mail.Message = model.Mail.Message.Replace("<br>", Environment.NewLine);
                        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromEmployeeDetails.EmployeeName);
                    }
                    if (status == "Submitted" || status == "AppraiserApproved" || status == "ReviewerApproved" || status == "IDFAppraiserSubmitted" || status == "EscalateToHRByAppraiser" || status == "AppraisalProcessCompletion")
                    {
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Name##", empDetailsOfAppraisee.EmployeeName);
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Code##", empDetailsOfAppraisee.EmployeeCode);
                        model.Mail.Message = model.Mail.Message.Replace("##Employee Name##", empDetailsOfAppraisee.EmployeeName);
                        model.Mail.Message = model.Mail.Message.Replace("##Employee Code##", empDetailsOfAppraisee.EmployeeCode);
                        model.Mail.Message = model.Mail.Message.Replace("<br>", Environment.NewLine);
                        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromEmployeeDetails.EmployeeName);
                    }
                    if (status == "disagreedByEmployee")
                    {
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Name##", empDetailsOfAppraisee.EmployeeName);
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Code##", empDetailsOfAppraisee.EmployeeCode);
                        model.Mail.Message = model.Mail.Message.Replace("##Appraiser1 Name##", empDetailsOfAppraiser1.EmployeeName);
                        model.Mail.Message = model.Mail.Message.Replace("##Employee Name##", empDetailsOfAppraisee.EmployeeName);
                        model.Mail.Message = model.Mail.Message.Replace("##Employee Code##", empDetailsOfAppraisee.EmployeeCode);
                        model.Mail.Message = model.Mail.Message.Replace("<br>", Environment.NewLine);
                        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromEmployeeDetails.EmployeeName);
                    }
                    if (status == "IDFAppraiserSubmitted")
                    {
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Name##", empDetailsOfAppraisee.EmployeeName);
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Code##", empDetailsOfAppraisee.EmployeeCode);
                        model.Mail.Message = model.Mail.Message.Replace("##Appraiser1 Name##", empDetailsOfAppraiser1.EmployeeName);
                        model.Mail.Message = model.Mail.Message.Replace("##Employee Name##", empDetailsOfAppraisee.EmployeeName);
                        model.Mail.Message = model.Mail.Message.Replace("<br>", Environment.NewLine);
                        model.Mail.Message = model.Mail.Message.Replace("##logged in user##", fromEmployeeDetails.EmployeeName);
                    }
                    if (status == "IDFInitiate")
                    {
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Name##", empDetailsOfAppraisee.EmployeeName);
                        model.Mail.Subject = model.Mail.Subject.Replace("##Employee Code##", empDetailsOfAppraisee.EmployeeCode);
                    }
                }

                return PartialView("_MailTemplateConfigureAppraisalProcess", model.Mail);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult SendEmail(EmployeeMailTemplate model)
        {
            try
            {
                bool result = false;
                EmployeeDAL employeeDAL = new EmployeeDAL();

                char[] symbols = new char[] { ';', ' ', ',', '\r', '\n' };
                int CcCounter = 0;
                int ToCounter = 0;

                if (model.Cc != "" && model.Cc != null)
                {
                    string CcMailIds = model.Cc.TrimEnd(symbols);
                    model.Cc = CcMailIds;
                    string[] EmailIds = CcMailIds.Split(symbols);

                    string[] CCEmailId = EmailIds.Where(s => !String.IsNullOrEmpty(s)).ToArray();

                    foreach (string id in CCEmailId)
                    {
                        HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetailsFromEmailId(id);

                        if (employeeDetails != null)
                            CcCounter = 1;
                        else
                        {
                            CcCounter = 0;
                            break;
                        }
                    }

                    string[] EmailToId = model.To.Split(symbols);
                    string[] EmailToIds = EmailToId.Where(s => !String.IsNullOrEmpty(s)).ToArray();
                    foreach (string email in EmailToIds)
                    {
                        if (email != "v2appraisal@v2solutions.com")
                        {
                            HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetailsFromEmailId(email);
                            if (employeeDetails != null)
                            {
                                ToCounter = 1;
                            }
                            else
                            {
                                ToCounter = 0;
                                break;
                            }
                        }
                        else
                        {
                            ToCounter = 1;
                        }
                    }
                }
                else
                {
                    CcCounter = 1;
                    string[] EmailToId = model.To.Split(symbols);
                    string[] EmailToIds = EmailToId.Where(s => !String.IsNullOrEmpty(s)).ToArray();
                    foreach (string email in EmailToIds)
                    {
                        if (email != "v2appraisal@v2solutions.com")
                        {
                            HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetailsFromEmailId(email);
                            if (employeeDetails != null)
                            {
                                ToCounter = 1;
                            }
                            else
                            {
                                ToCounter = 0;
                                break;
                            }
                        }
                        else
                        {
                            ToCounter = 1;
                        }
                    }
                }

                if (CcCounter == 1 && ToCounter == 1)
                {
                    result = SendMail(model);
                    if (result == true)
                        return Json(new { status = true, validCcId = true, validtoId = true });
                    else
                        return Json(new { status = false, validCcId = true, validtoId = true });
                }
                else
                {
                    if (CcCounter == 1 && ToCounter == 0)
                        return Json(new { status = false, validCcId = true, validtoId = false });
                    else
                    {
                        if (CcCounter == 0 && ToCounter == 1)
                            return Json(new { status = false, validCcId = false, validtoId = true });
                        else
                        {
                            return Json(new { status = false, validCcId = false, validtoId = false });
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool SendMail(EmployeeMailTemplate model)
        {
            try
            {
                SMTPHelper smtpHelper = new SMTPHelper();
                char[] symbols = new char[] { ';', ' ', ',', '\r', '\n' };
                if (model != null)
                {
                    string[] ToEmailId = model.To.Split(symbols);

                    //Loop to seperate email id's of CC peoples
                    string[] CCEmailId = null;
                    if (model.Cc != "" && model.Cc != null)
                    {
                        CCEmailId = model.Cc.Split(symbols);
                        string[] CCEmailIds = CCEmailId.Where(s => !String.IsNullOrEmpty(s)).ToArray();
                        return smtpHelper.SendMail(ToEmailId, null, CCEmailIds, null, null, null, model.From, null, model.Subject, model.Message, null, null);
                    }
                    else
                        return smtpHelper.SendMail(ToEmailId, null, null, null, null, null, model.From, null, model.Subject, model.Message, null, null);
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult ShowHistoryAppraisalDocUploads(int documentId, string uploadType, int appraisalYearID)
        {
            HRMSDBEntities dbContext = new HRMSDBEntities();
            ConfigurationAppraisalDAL uploads = new ConfigurationAppraisalDAL();
            List<tbl_ApprisalDocumentDetail> objHRDocDetails = new List<tbl_ApprisalDocumentDetail>();
            tbl_ApprisalDocuments objhRDoc = new tbl_ApprisalDocuments();
            List<AppraiserReviewerMappingModel> objDocList = new List<AppraiserReviewerMappingModel>();
            int uploadTypeId = dbContext.Tbl_HR_UploadType.Where(x => x.UploadType == uploadType).FirstOrDefault().UploadTypeId;
            try
            {
                objhRDoc = uploads.GetAppraisalDocument(documentId, appraisalYearID);
                objHRDocDetails = uploads.GetAppraisalDocumentHistoryForDisplay(documentId, appraisalYearID);

                foreach (tbl_ApprisalDocumentDetail eachDocDetail in objHRDocDetails)
                {
                    AppraiserReviewerMappingModel dd = new AppraiserReviewerMappingModel()
                    {
                        DocumentID = eachDocDetail.DocumentId,

                        FileDescription = eachDocDetail.FileDescription,
                        FileName = eachDocDetail.FileName,
                        UploadedBy = uploads.GetUploadNameFromUploadById((eachDocDetail.UploadedBy).ToString()),
                        UploadedDate = (eachDocDetail.UploadedDate).Value,
                        FilePath = eachDocDetail.FilePath,
                        UploadTypeId = uploadTypeId,
                        AppraisalYearID = eachDocDetail.AppraisalYearID
                    };
                    objDocList.Add(dd);
                }

                AppraiserReviewerMappingModel dd1 = new AppraiserReviewerMappingModel()
                {
                    DocumentID = objhRDoc.DocumentId,

                    FileDescription = objhRDoc.FileDescription,
                    FileName = objhRDoc.FileName,
                    UploadedBy = uploads.GetUploadNameFromUploadById((objhRDoc.UploadedBy).ToString()),
                    UploadedDate = (objhRDoc.UploadedDate).Value,
                    FilePath = objhRDoc.FilePath,
                    UploadTypeId = uploadTypeId,
                    AppraisalYearID = objhRDoc.AppraisalYearID
                };

                objDocList.Add(dd1);
                return PartialView("_ShowAppraisalDocHistory", objDocList);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public ActionResult DownloadAppraisalFile(string filename, int uploadTypeId, int appraisalYearID)
        {
            HRMSDBEntities dbContext = new HRMSDBEntities();
            ConfigurationAppraisalDAL RMGupload = new ConfigurationAppraisalDAL();
            string Loginemployeecode = string.Empty;
            string[] loginemployeerole = { };
            EmployeeDAL empdal = new EmployeeDAL();
            int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);
            HRMS_tbl_PM_Employee loginrolescheck = empdal.GetEmployeeDetails(employeeID);
            Loginemployeecode = loginrolescheck.EmployeeCode;
            loginemployeerole = Roles.GetRolesForUser(Loginemployeecode);

            try
            {
                var documentformchild = (from document in dbContext.tbl_ApprisalDocuments
                                         join documentDetails in dbContext.tbl_ApprisalDocumentDetail
                                         on document.DocumentId equals documentDetails.DocumentId
                                         where document.UploadTypeId == uploadTypeId && documentDetails.FileName == filename && documentDetails.AppraisalYearID == appraisalYearID
                                         select documentDetails).FirstOrDefault();

                var documentfromparent = (from document in dbContext.tbl_ApprisalDocuments
                                          where document.UploadTypeId == uploadTypeId && document.FileName == filename && document.AppraisalYearID == appraisalYearID
                                          select document).FirstOrDefault();

                string rootFolder = HttpContext.Server.MapPath(UploadFileLocationAppraisal);
                string[] FileExtention = filename.Split('.');
                string contentType = "application/" + FileExtention[1];

                if (documentformchild != null)
                {
                    string subfolderpath = Path.Combine(rootFolder, GetUploadTypeTextFromDocIdAppraisalChild(documentformchild.DocumentId, appraisalYearID));
                    string Filepath = Path.Combine(subfolderpath, filename);
                    if (!System.IO.File.Exists(Filepath))
                    {
                        throw new Exception();
                    }
                    return File(Filepath, contentType, filename);
                }
                else
                {
                    string subfolderpath = Path.Combine(rootFolder, GetUploadTypeTextFromDocIdAppraisal(documentfromparent.DocumentId, appraisalYearID));
                    string Filepath = Path.Combine(subfolderpath, filename);
                    if (!System.IO.File.Exists(Filepath))
                    {
                        throw new Exception();
                    }
                    return File(Filepath, contentType, filename);
                }
            }
            catch (Exception)
            {
                ConfigurationViewModel model = new ConfigurationViewModel();
                model.SearchedUserDetails = new SearchedUserDetails();
                string employeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(employeeCode);
                if (employeeCode != null)
                {
                    CommonMethodsDAL Commondal = new CommonMethodsDAL();
                    model.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                }
                return PartialView("_FileNotFound", model);
            }
        }

        public string GetUploadTypeTextFromDocIdAppraisalChild(int documentId, int appraisalYearID)
        {
            string uploadTypeText = string.Empty;
            HRMSDBEntities dbContext = new HRMSDBEntities();
            try
            {
                var uploadTypeId = (from ut in dbContext.tbl_ApprisalDocumentDetail
                                    where ut.DocumentId == documentId && ut.AppraisalYearID == appraisalYearID
                                    select ut.UploadTypeId).FirstOrDefault();

                var uploadTypeId1 = uploadTypeId.HasValue ? uploadTypeId.Value : 0;
                uploadTypeText = GetUploadTypeSelectedText(uploadTypeId1);

                return uploadTypeText;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetUploadTypeTextFromDocIdAppraisal(int documentId, int appraisalYearID)
        {
            string uploadTypeText = string.Empty;
            HRMSDBEntities dbContext = new HRMSDBEntities();
            try
            {
                var uploadTypeId = (from ut in dbContext.tbl_ApprisalDocuments
                                    where ut.DocumentId == documentId && ut.AppraisalYearID == appraisalYearID
                                    select ut.UploadTypeId).FirstOrDefault();

                var uploadTypeId1 = uploadTypeId.HasValue ? uploadTypeId.Value : 0;
                uploadTypeText = GetUploadTypeSelectedText(uploadTypeId1);

                return uploadTypeText;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetUploadTypeSelectedText(int UploadTypeId)
        {
            ConfigurationAppraisalDAL uploads = new ConfigurationAppraisalDAL();
            var uploadTypes = uploads.GetHRUploadTypes();

            return uploadTypes.Where(u => u.UploadTypeId == UploadTypeId).FirstOrDefault().UploadType;
        }

        public ActionResult DeleteAppraisalUploadDetails(int documentId, int appraisalYearID)
        {
            ConfigurationAppraisalDAL uploads = new ConfigurationAppraisalDAL();
            HRMSDBEntities dbContext = new HRMSDBEntities();
            bool udd = false;
            try
            {
                var parentDoc = dbContext.tbl_ApprisalDocuments.Where(x => x.DocumentId == documentId && x.AppraisalYearID == appraisalYearID).FirstOrDefault();
                var versionDocs = dbContext.tbl_ApprisalDocumentDetail.Where(x => x.DocumentId == documentId && x.AppraisalYearID == appraisalYearID).ToList();
                string rootFolder = HttpContext.Server.MapPath(UploadFileLocationAppraisal);
                string subfolderpath = Path.Combine(rootFolder, GetUploadTypeTextFromDocIdAppraisal(parentDoc.DocumentId, appraisalYearID));
                if (versionDocs != null)
                {
                    foreach (var d in versionDocs)
                    {
                        string versionDocFilepath = Path.Combine(subfolderpath, d.FileName);
                        if (System.IO.File.Exists(versionDocFilepath))
                            System.IO.File.Delete(versionDocFilepath);
                    }
                }

                string Filepath = Path.Combine(subfolderpath, parentDoc.FileName);
                if (System.IO.File.Exists(Filepath))
                    System.IO.File.Delete(Filepath);

                udd = uploads.DeleteAppraisalUploadDetails(documentId, appraisalYearID);
                return Json(udd, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                udd = false;
                return Json(udd, JsonRequestBehavior.AllowGet);
            }
        }

        public void ExportIniEmployeeToExcel(EligibilityCriteriaModel AllIniEmp)
        {
            List<AllIniEmployees> emp = new List<AllIniEmployees>();
            AllIniEmp.allSuccessEmployeeList.ForEach(x =>
                {
                    AllIniEmployees allEmp = new AllIniEmployees();
                    allEmp.EmployeeCode = x.EmployeeCode;
                    allEmp.EmployeeName = x.EmployeeName;
                    allEmp.Designation = x.Designation;
                    allEmp.DeliveryTeam = x.DeliveryTeam;
                    allEmp.ConfirmationDate = x.ConfirmationDate;
                    allEmp.ProbationReviewDate = x.ProbationReviewDate;
                    emp.Add(allEmp);
                });
            GridView gv = new GridView();
            gv.DataSource = emp;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=InitiatedEmployees.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        public ActionResult DeleteAppraisalDocsSelected(List<string> filenames, int appraisalYearID)
        {
            ConfigurationAppraisalDAL uploads = new ConfigurationAppraisalDAL();
            HRMSDBEntities dbContext = new HRMSDBEntities();

            string Loginemployeecode = string.Empty;
            string[] loginemployeerole = { };
            EmployeeDAL empdal = new EmployeeDAL();
            int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);
            HRMS_tbl_PM_Employee loginrolescheck = empdal.GetEmployeeDetails(employeeID);
            Loginemployeecode = loginrolescheck.EmployeeCode;
            loginemployeerole = Roles.GetRolesForUser(Loginemployeecode);

            bool result = false;
            try
            {
                if (filenames != null)
                {
                    foreach (string filename in filenames)
                    {
                        var documentformchild = (from document in dbContext.tbl_ApprisalDocuments
                                                 join documentDetails in dbContext.tbl_ApprisalDocumentDetail
                                                 on document.DocumentId equals documentDetails.DocumentId
                                                 where documentDetails.FileName == filename && documentDetails.AppraisalYearID == appraisalYearID
                                                 select documentDetails).FirstOrDefault();

                        string rootFolder = HttpContext.Server.MapPath(UploadFileLocationAppraisal);
                        string subfolderpath = Path.Combine(rootFolder, GetUploadTypeTextFromDocIdAppraisal(documentformchild.DocumentId, appraisalYearID));
                        string Filepath = Path.Combine(subfolderpath, filename);

                        if (System.IO.File.Exists(Filepath))
                            System.IO.File.Delete(Filepath);
                        result = uploads.DeleteAppraisalDocsSelected(filename, appraisalYearID);
                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public void ExportAppraisalRatingReportToExcel(AppraisalStatusReport ratingReportList)
        {
            DataSet ds = new DataSet();
            GridView gv = new GridView();
            List<AppraisalStatusReportViewModel> report = new List<AppraisalStatusReportViewModel>();
            ConfigurationAppraisalDAL configAppraisalDal = new ConfigurationAppraisalDAL();
            ratingReportList.ParameterList = configAppraisalDal.getParamterList(ratingReportList.YearID);

            ratingReportList.AppraisalRatingsReportList.ForEach(x =>
            {
                AppraisalStatusReportViewModel reportModel = new AppraisalStatusReportViewModel();
                reportModel.EmployeeName = x.EmployeeName;
                reportModel.Employeecode = x.Employeecode;
                reportModel.ParentDu = x.ParentDu;
                reportModel.CurrentDu = x.CurrentDu;
                reportModel.DeliveryTeamName = x.DeliveryTeamName;
                reportModel.DesignationName = x.DesignationName;
                reportModel.JoiningDate = x.JoiningDate;
                reportModel.ConfirmationDate = x.ConfirmationDate;
                reportModel.ProbationReviewDate = x.ProbationReviewDate;
                reportModel.Appraiser1Name = x.Appraiser1Name;

                //for appraiser 1

                reportModel.ratingOneAppraiserOne = x.ratingOneAppraiserOne;
                reportModel.ratingTwoAppraiserOne = x.ratingTwoAppraiserOne;
                reportModel.ratingThreeAppraiserOne = x.ratingThreeAppraiserOne;
                reportModel.ratingFourAppraiserOne = x.ratingFourAppraiserOne;
                reportModel.ratingFiveAppraiserOne = x.ratingFiveAppraiserOne;
                reportModel.ratingSixAppraiserOne = x.ratingSixAppraiserOne;

                reportModel.CommentOneAppraiserOne = x.CommentOneAppraiserOne;
                reportModel.CommentTwoAppraiserOne = x.CommentTwoAppraiserOne;
                reportModel.CommentThreeAppraiserOne = x.CommentThreeAppraiserOne;
                reportModel.CommentFourAppraiserOne = x.CommentFourAppraiserOne;
                reportModel.CommentFiveAppraiserOne = x.CommentFiveAppraiserOne;
                reportModel.CommentSixAppraiserOne = x.CommentSixAppraiserOne;

                //for appraiser2
                reportModel.Appraiser2Name = x.Appraiser2Name;

                reportModel.ratingOneAppraiserTwo = x.ratingOneAppraiserTwo;
                reportModel.ratingTwoAppraiserTwo = x.ratingTwoAppraiserTwo;
                reportModel.ratingThreeAppraiserTwo = x.ratingThreeAppraiserTwo;
                reportModel.ratingFourAppraiserTwo = x.ratingFourAppraiserTwo;
                reportModel.ratingFiveAppraiserTwo = x.ratingFiveAppraiserTwo;
                reportModel.ratingSixAppraiserTwo = x.ratingSixAppraiserTwo;

                reportModel.CommentOneAppraiserTwo = x.CommentOneAppraiserTwo;
                reportModel.CommentTwoAppraiserTwo = x.CommentTwoAppraiserTwo;
                reportModel.CommentThreeAppraiserTwo = x.CommentThreeAppraiserTwo;
                reportModel.CommentFourAppraiserTwo = x.CommentFourAppraiserTwo;
                reportModel.CommentFiveAppraiserTwo = x.CommentFiveAppraiserTwo;
                reportModel.CommentSixAppraiserTwo = x.CommentSixAppraiserTwo;

                //for reviewer1
                reportModel.Reviewer1Name = x.Reviewer1Name;
                reportModel.ratingOneReviewerOne = x.ratingOneReviewerOne;
                reportModel.ratingTwoReviewerOne = x.ratingTwoReviewerOne;
                reportModel.ratingThreeReviewerOne = x.ratingThreeReviewerOne;
                reportModel.ratingFourReviewerOne = x.ratingFourReviewerOne;
                reportModel.ratingFiveReviewerOne = x.ratingFiveReviewerOne;
                reportModel.ratingSixReviewerOne = x.ratingSixReviewerOne;

                reportModel.CommentOneReviewerOne = x.CommentOneReviewerOne;
                reportModel.CommentTwoReviewerOne = x.CommentTwoReviewerOne;
                reportModel.CommentThreeReviewerOne = x.CommentThreeReviewerOne;
                reportModel.CommentFourReviewerOne = x.CommentFourReviewerOne;
                reportModel.CommentFiveReviewerOne = x.CommentFiveReviewerOne;
                reportModel.CommentSixReviewerOne = x.CommentSixReviewerOne;

                reportModel.Reviewer1OverAllRating = x.Reviewer1OverAllRating;
                reportModel.Reviewer1OverAllComment = x.Reviewer1OverAllComment;
                reportModel.PromotionRecommentationReviewer1 = x.PromotionRecommentationReviewer1;
                reportModel.NextDesignationReviewer1 = x.NextDesignationReviewer1;

                //for reviewer2

                reportModel.ratingOneReviewerTwo = x.ratingOneReviewerTwo;
                reportModel.ratingTwoReviewerTwo = x.ratingTwoReviewerTwo;
                reportModel.ratingThreeReviewerTwo = x.ratingThreeReviewerTwo;
                reportModel.ratingFourReviewerTwo = x.ratingFourReviewerTwo;
                reportModel.ratingFiveReviewerTwo = x.ratingFiveReviewerTwo;
                reportModel.ratingSixReviewerTwo = x.ratingSixReviewerTwo;

                reportModel.CommentOneReviewerTwo = x.CommentOneReviewerTwo;
                reportModel.CommentTwoReviewerTwo = x.CommentTwoReviewerTwo;
                reportModel.CommentThreeReviewerTwo = x.CommentThreeReviewerTwo;
                reportModel.CommentFourReviewerTwo = x.CommentFourReviewerTwo;
                reportModel.CommentFiveReviewerTwo = x.CommentFiveReviewerTwo;
                reportModel.CommentSixReviewerTwo = x.CommentSixReviewerTwo;

                reportModel.Reviewer2OverAllRating = x.Reviewer2OverAllRating;
                reportModel.Reviewer2OverAllComment = x.Reviewer2OverAllComment;
                reportModel.PromotionRecommentationReviewer2 = x.PromotionRecommentationReviewer2;
                reportModel.NextDesignationReviewer2 = x.NextDesignationReviewer2;
                //for groupHead

                reportModel.ratingOneGroupHead = x.ratingOneGroupHead;
                reportModel.ratingTwoGroupHead = x.ratingTwoGroupHead;
                reportModel.ratingThreeGroupHead = x.ratingThreeGroupHead;
                reportModel.ratingFourGroupHead = x.ratingFourGroupHead;
                reportModel.ratingFiveGroupHead = x.ratingFiveGroupHead;
                reportModel.ratingSixGroupHead = x.ratingSixGroupHead;

                reportModel.CommentOneReviewerTwo = x.CommentOneReviewerTwo;
                reportModel.CommentTwoGroupHead = x.CommentTwoGroupHead;
                reportModel.CommentThreeGroupHead = x.CommentThreeGroupHead;
                reportModel.CommentFourGroupHead = x.CommentFourGroupHead;
                reportModel.CommentFiveGroupHead = x.CommentFiveGroupHead;
                reportModel.CommentSixGroupHead = x.CommentSixGroupHead;

                reportModel.Reviewer2Name = x.Reviewer2Name;
                reportModel.GroupHeadName = x.GroupHeadName;
                reportModel.GroupHeadOverAllRating = x.GroupHeadOverAllRating;
                reportModel.GroupHeadOverAllComment = x.GroupHeadOverAllComment;
                reportModel.PromotionRecommentationGroupHead = x.PromotionRecommentationGroupHead;
                reportModel.NextDesignationGroupHead = x.NextDesignationGroupHead;
                report.Add(reportModel);
            });
            DataTable dt = new DataTable();

            for (int i = 0; i < 86; i++)
            {
                dt.Columns.Add("", typeof(string));
            }

            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = "Employee Name";
                dr[1] = "Employee Code";
                dr[2] = "Parent DU";
                dr[3] = "Current DU";
                dr[4] = "Delivery Team";
                dr[5] = "Designation";
                dr[6] = "Joining Date";
                dr[7] = "Confirmation Date";
                dr[8] = "Probation Review Date";
                dr[9] = "Appraiser 1 Name";
                dr[10] = ratingReportList.ParameterList[0].ParameterName;
                dr[11] = ratingReportList.ParameterList[1].ParameterName;
                dr[12] = ratingReportList.ParameterList[2].ParameterName;
                dr[13] = ratingReportList.ParameterList[3].ParameterName;
                dr[14] = ratingReportList.ParameterList[4].ParameterName;
                dr[15] = ratingReportList.ParameterList[5].ParameterName;
                dr[16] = ratingReportList.ParameterList[0].ParameterName;
                dr[17] = ratingReportList.ParameterList[1].ParameterName;
                dr[18] = ratingReportList.ParameterList[2].ParameterName;
                dr[19] = ratingReportList.ParameterList[3].ParameterName;
                dr[20] = ratingReportList.ParameterList[4].ParameterName;
                dr[21] = ratingReportList.ParameterList[5].ParameterName;
                dr[22] = "Appraiser 2 Name";
                dr[23] = ratingReportList.ParameterList[0].ParameterName;
                dr[24] = ratingReportList.ParameterList[1].ParameterName;
                dr[25] = ratingReportList.ParameterList[2].ParameterName;
                dr[26] = ratingReportList.ParameterList[3].ParameterName;
                dr[27] = ratingReportList.ParameterList[4].ParameterName;
                dr[28] = ratingReportList.ParameterList[5].ParameterName;
                dr[29] = ratingReportList.ParameterList[0].ParameterName;
                dr[30] = ratingReportList.ParameterList[1].ParameterName;
                dr[31] = ratingReportList.ParameterList[2].ParameterName;
                dr[32] = ratingReportList.ParameterList[3].ParameterName;
                dr[33] = ratingReportList.ParameterList[4].ParameterName;
                dr[34] = ratingReportList.ParameterList[5].ParameterName;
                dr[35] = "Reviewer 1 Name";
                dr[36] = ratingReportList.ParameterList[0].ParameterName;
                dr[37] = ratingReportList.ParameterList[1].ParameterName;
                dr[38] = ratingReportList.ParameterList[2].ParameterName;
                dr[39] = ratingReportList.ParameterList[3].ParameterName;
                dr[40] = ratingReportList.ParameterList[4].ParameterName;
                dr[41] = ratingReportList.ParameterList[5].ParameterName;
                dr[42] = "Reviewer1 OverAll Rating";
                dr[43] = ratingReportList.ParameterList[0].ParameterName;
                dr[44] = ratingReportList.ParameterList[1].ParameterName;
                dr[45] = ratingReportList.ParameterList[2].ParameterName;
                dr[46] = ratingReportList.ParameterList[3].ParameterName;
                dr[47] = ratingReportList.ParameterList[4].ParameterName;
                dr[48] = ratingReportList.ParameterList[5].ParameterName;
                dr[49] = "Reviewer1 OverAll Comment";
                dr[50] = "Promotion Recommendation";
                dr[51] = "Next Designation";
                dr[52] = "Reviewer 2 Name";
                dr[53] = ratingReportList.ParameterList[0].ParameterName;
                dr[54] = ratingReportList.ParameterList[1].ParameterName;
                dr[55] = ratingReportList.ParameterList[2].ParameterName;
                dr[56] = ratingReportList.ParameterList[3].ParameterName;
                dr[57] = ratingReportList.ParameterList[4].ParameterName;
                dr[58] = ratingReportList.ParameterList[5].ParameterName;
                dr[59] = "Reviewer2 OverAll Rating";
                dr[60] = ratingReportList.ParameterList[0].ParameterName;
                dr[61] = ratingReportList.ParameterList[1].ParameterName;
                dr[62] = ratingReportList.ParameterList[2].ParameterName;
                dr[63] = ratingReportList.ParameterList[3].ParameterName;
                dr[64] = ratingReportList.ParameterList[4].ParameterName;
                dr[65] = ratingReportList.ParameterList[5].ParameterName;
                dr[66] = "Reviewer2 OverAll Comment";
                dr[67] = "Promotion Recommendation";
                dr[68] = "Next Designation";
                dr[69] = "Group Head Name";
                dr[70] = ratingReportList.ParameterList[0].ParameterName;
                dr[71] = ratingReportList.ParameterList[1].ParameterName;
                dr[72] = ratingReportList.ParameterList[2].ParameterName;
                dr[73] = ratingReportList.ParameterList[3].ParameterName;
                dr[74] = ratingReportList.ParameterList[4].ParameterName;
                dr[75] = ratingReportList.ParameterList[5].ParameterName;
                dr[76] = "GroupHead OverAll Rating";
                dr[77] = ratingReportList.ParameterList[0].ParameterName;
                dr[78] = ratingReportList.ParameterList[1].ParameterName;
                dr[79] = ratingReportList.ParameterList[2].ParameterName;
                dr[80] = ratingReportList.ParameterList[3].ParameterName;
                dr[81] = ratingReportList.ParameterList[4].ParameterName;
                dr[82] = ratingReportList.ParameterList[5].ParameterName;
                dr[83] = "GroupHead OverAll Comment";
                dr[84] = "Promotion Recommendation";
                dr[85] = "Next Designation";

                dt.Rows.Add(dr);
            }
            foreach (var array in report)
            {
                DataRow dr = dt.NewRow();
                dr[0] = array.EmployeeName;
                dr[1] = array.Employeecode;
                dr[2] = array.ParentDu;
                dr[3] = array.CurrentDu;
                dr[4] = array.DeliveryTeamName;
                dr[5] = array.DesignationName;
                dr[6] = array.JoiningDate.HasValue ? Convert.ToDateTime(array.JoiningDate).ToShortDateString() : null;
                dr[7] = array.ConfirmationDate.HasValue ? Convert.ToDateTime(array.ConfirmationDate).ToShortDateString() : null;
                dr[8] = array.ProbationReviewDate.HasValue ? Convert.ToDateTime(array.ProbationReviewDate).ToShortDateString() : null;
                dr[9] = array.Appraiser1Name;
                dr[10] = array.ratingOneAppraiserOne;
                dr[11] = array.ratingTwoAppraiserOne;
                dr[12] = array.ratingThreeAppraiserOne;
                dr[13] = array.ratingFourAppraiserOne;
                dr[14] = array.ratingFiveAppraiserOne;
                dr[15] = array.ratingSixAppraiserOne;
                dr[16] = array.CommentOneAppraiserOne;
                dr[17] = array.CommentTwoAppraiserOne;
                dr[18] = array.CommentThreeAppraiserOne;
                dr[19] = array.CommentFourAppraiserOne;
                dr[20] = array.CommentFiveAppraiserOne;
                dr[21] = array.CommentSixAppraiserOne;
                dr[22] = array.Appraiser2Name;
                dr[23] = array.ratingOneAppraiserTwo;
                dr[24] = array.ratingTwoAppraiserTwo;
                dr[25] = array.ratingThreeAppraiserTwo;
                dr[26] = array.ratingFourAppraiserTwo;
                dr[27] = array.ratingFiveAppraiserTwo;
                dr[28] = array.ratingSixAppraiserTwo;
                dr[29] = array.CommentOneAppraiserTwo;
                dr[30] = array.CommentTwoAppraiserTwo;
                dr[31] = array.CommentThreeAppraiserTwo;
                dr[32] = array.CommentFourAppraiserTwo;
                dr[33] = array.CommentFiveAppraiserTwo;
                dr[34] = array.CommentSixAppraiserTwo;
                dr[35] = array.Reviewer1Name;
                dr[36] = array.ratingOneReviewerOne;
                dr[37] = array.ratingTwoReviewerOne;
                dr[38] = array.ratingThreeReviewerOne;
                dr[39] = array.ratingFourReviewerOne;
                dr[40] = array.ratingFiveReviewerOne;
                dr[41] = array.ratingSixReviewerOne;
                dr[42] = array.Reviewer1OverAllRating;
                dr[43] = array.CommentOneReviewerOne;
                dr[44] = array.CommentTwoReviewerOne;
                dr[45] = array.CommentThreeReviewerOne;
                dr[46] = array.CommentFourReviewerOne;
                dr[47] = array.CommentFiveReviewerOne;
                dr[48] = array.CommentSixReviewerOne;
                dr[49] = array.Reviewer1OverAllComment;
                dr[50] = array.PromotionRecommentationReviewer1;
                dr[51] = array.NextDesignationReviewer1;
                dr[52] = array.Reviewer2Name;
                dr[53] = array.ratingOneReviewerTwo;
                dr[54] = array.ratingTwoReviewerTwo;
                dr[55] = array.ratingThreeReviewerTwo;
                dr[56] = array.ratingFourReviewerTwo;
                dr[57] = array.ratingFiveReviewerTwo;
                dr[58] = array.ratingSixReviewerTwo;
                dr[59] = array.Reviewer2OverAllRating;
                dr[60] = array.CommentOneReviewerTwo;
                dr[61] = array.CommentTwoReviewerTwo;
                dr[62] = array.CommentThreeReviewerTwo;
                dr[63] = array.CommentFourReviewerTwo;
                dr[64] = array.CommentFiveReviewerTwo;
                dr[65] = array.CommentSixReviewerTwo;
                dr[66] = array.Reviewer2OverAllComment;
                dr[67] = array.PromotionRecommentationReviewer2;
                dr[68] = array.NextDesignationReviewer2;
                dr[69] = array.GroupHeadName;
                dr[70] = array.ratingOneGroupHead;
                dr[71] = array.ratingTwoGroupHead;
                dr[72] = array.ratingThreeGroupHead;
                dr[73] = array.ratingFourGroupHead;
                dr[74] = array.ratingFiveGroupHead;
                dr[75] = array.ratingSixGroupHead;
                dr[76] = array.GroupHeadOverAllRating;
                dr[77] = array.CommentOneGroupHead;
                dr[78] = array.CommentTwoGroupHead;
                dr[79] = array.CommentThreeGroupHead;
                dr[80] = array.CommentFourGroupHead;
                dr[81] = array.CommentFiveGroupHead;
                dr[82] = array.CommentSixGroupHead;
                dr[83] = array.GroupHeadOverAllComment;
                dr[84] = array.PromotionRecommentationGroupHead;
                dr[85] = array.NextDesignationGroupHead;

                dt.Rows.Add(dr);
            }

            gv.ShowHeader = false;

            gv.DataSource = dt;
            gv.DataBind();

            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = " ";
            cell.ColumnSpan = 1;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = " ";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = " ";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = " ";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = " ";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = " ";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = " ";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = " ";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = " ";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = " ";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 6;
            cell.Text = "Appraiser 1 Rating";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 6;
            cell.Text = "Appraiser 1 Comment";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = " ";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 6;
            cell.Text = "Appraiser 2 Rating";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 6;
            cell.Text = "Appraiser 2 Comments";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = " ";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 7;
            cell.Text = "Reviewer 1 Rating";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 7;
            cell.Text = "Reviewer 1 Comment";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 3;
            cell.Text = " ";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 7;
            cell.Text = "Reviewer 2 Rating";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 7;
            cell.Text = "Reviewer 2 Comment";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 3;
            cell.Text = " ";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 7;
            cell.Text = "Group Head Rating";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 7;
            cell.Text = "Group Head Comment";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = " ";
            row.Controls.Add(cell);

            gv.HeaderRow.Parent.Controls.AddAt(0, row);

            foreach (GridViewRow rr in gv.Rows)
            {
                int p = 0;
                if (rr.RowType == DataControlRowType.DataRow)
                {
                    rr.Attributes.Add("class", "DateClass");
                }
                break;
            }

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=AppraisalRatingReport.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);

            string styleDate = @"<style> .DateClass { font-weight:bold;} </style> ";

            Response.Output.Write(styleDate);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
}