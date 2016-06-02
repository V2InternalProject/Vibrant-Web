using HRMS.DAL;
using HRMS.Models;
using HRMS.Models.SkillMatrix;
using MvcApplication3.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS.Controllers
{
    public class SkillMatrixController : Controller
    {
        // GET: /SkillMatrix/
        private SkillMatrixDal dal = new SkillMatrixDal();

        //global variable to check for submit(counter)
        private static int count;

        private static bool statusData;

        public ActionResult Index_Skillmatrix(int? EmployeeCode, int? ProjectEmployeeRoleID, int? ProjectSkillMatrixStatusID, string LoginEmployeeID)
        {
            ModelState.Clear();
            DetailsModel model = new DetailsModel();
            WSEMDBEntities wsem = new WSEMDBEntities();
            //int employeeCode = Convert.ToInt32(EmployeeCode);
            string employeeCode = Convert.ToString(EmployeeCode);
            tbl_PM_Employee_SEM employeeDetails = wsem.tbl_PM_Employee_SEM.Where(x => x.EmployeeCode == employeeCode).FirstOrDefault();
            model.EmployeeCode = EmployeeCode;
            model.EmployeeId = employeeDetails.EmployeeID;
            model.ProjectEmployeeRoleID = ProjectEmployeeRoleID;
            model.ProjectSkillMatrixFormStatus = ProjectSkillMatrixStatusID; //added by sat
            model.EmployeeName = employeeDetails.EmployeeName;
            ViewBag.ResourcePoolList = dal.getResourcePoolNamesList();
            ViewBag.Ratings = dal.getRatings();
            ViewBag.loggedInEmployeeId = LoginEmployeeID;
            return PartialView("Index_Skillmatrix", model);
        }

        public ActionResult SpecificSkill(int? id, int page, int rows)
        {
            try
            {
                DetailsModel model = new DetailsModel();
                int totalCount;
                List<DetailsModel> list = new List<DetailsModel>();
                list = dal.SearchEmployeeForEmp(id, page, rows, out totalCount, "Particular");
                if ((list == null || list.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    list = dal.SearchEmployeeForEmp(id, page, rows, out totalCount, "Particular");
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = list
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult AdditionalSkill(int? id, int page, int rows)
        {
            try
            {
                DetailsModel model = new DetailsModel();
                int totalCount;
                List<DetailsModel> list = new List<DetailsModel>();
                list = dal.SearchEmployeeForEmp(id, page, rows, out totalCount, "OtherSkill");
                if ((list == null || list.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    list = dal.SearchEmployeeForEmp(id, page, rows, out totalCount, "OtherSkill");
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = list
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult ShowSkillDetails(int resourcePoolID)
        {
            DetailsModel model = new DetailsModel();

            List<DetailsModel> skillName = dal.getSkillNames(resourcePoolID);
            return Json(new { results = skillName }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveRatingDetails(DetailsModel model, string skillID, int? resourcePoolID, string rating, string EmployeeCode, int loggedInEmployeeId)
        {
            string resultMessage = string.Empty;
            bool status = false;
            try
            {
                if (rating.Trim().Length != 0 && skillID.Trim().Length != 0 && resourcePoolID != null)
                {
                    WSEMDBEntities wsem = new WSEMDBEntities();
                    tbl_PM_Employee_SEM loggedEmployeeDetails = wsem.tbl_PM_Employee_SEM.Where(x => x.EmployeeID == loggedInEmployeeId).FirstOrDefault();
                    HRMS_tbl_PM_Employee employeeDetails = new HRMS_tbl_PM_Employee();
                    employeeDetails = dal.GetEmployeeDetailsByEmployeeCode(EmployeeCode);
                    status = dal.SaveRating(model, skillID, rating, employeeDetails.EmployeeID, loggedEmployeeDetails.EmployeeName);
                    if (status)
                    {
                        resultMessage = "Saved";
                        count = 1;
                    }
                    else
                        resultMessage = "Error";
                }
                else
                {
                    status = false;
                }
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteRatingDetails(int Id)
        {
            try
            {
                bool status = dal.DeleteSkill(Id);
                count = 1;
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FinalSubmitSkillMatrix(int projectEmployeeRoleID)
        {
            try
            {
                //if (stat == false)
                //{
                //if (count == 1)
                //{
                statusData = dal.SubmitSkillDetails(projectEmployeeRoleID.ToString());
                //count = 0;
                //}
                //else
                //{
                //    statusData = false;
                //}
                //}
                //else if (stat == true)
                //{
                //    statusData = dal.SubmitSkillDetails(projectEmployeeRoleID.ToString());
                //}
                return Json(new { status = statusData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ShowHistory(int EmployeeId)
        {
            skillmatrix_history model = new skillmatrix_history();
            ViewBag.ResourcePoolType = dal.getResoucePoolNames().ToList();
            model.resourcepoolListdata = dal.getResoucePoolNames().ToList();
            model.EmployeeIdInt = EmployeeId;
            model.getskilllist = new List<GetSkillName>();
            return PartialView("ShowHistory", model);
        }

        [HttpGet]
        public ActionResult ShowSkillData(int ResourcePoolID)
        {
            try
            {
                skillmatrix_history model = new skillmatrix_history();
                ViewBag.SkillListDetails = dal.GetSkillname(ResourcePoolID);
                model.resourcepoolListdata = dal.getResoucePoolNames().ToList();
                model.getskilllist = dal.GetSkillname(ResourcePoolID);
                return Json(new { results = model.getskilllist }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        public JsonResult History(int? employeeId, int? resourcePoolID, string ToolId)
        {
            List<skillmatrix_history> list = new List<skillmatrix_history>();
            int ResourcePoolID = Convert.ToInt32(resourcePoolID);
            int EmployeeId = Convert.ToInt32(employeeId);
            list = dal.getSkillMatrix_NewHistory(ResourcePoolID, EmployeeId, ToolId);
            int total = list.Count();
            var jsonData = new
            {
                total,
                page = 1,
                rows = list,
            };
            return Json(jsonData);
        }

        [HttpPost]
        public ActionResult SkillCount(int employeeId)
        {
            bool status = false;
            List<skillmatrix_history> list = new List<skillmatrix_history>();
            list = dal.getSkillMatrix_NewHistory(0, employeeId, null);
            int count = list.Count();
            if (count <= 0)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return Json(new { status = status });
        }

        [HttpGet]
        [PageAccess(PageName = "Skill Matrix")]
        public ActionResult Index()
        {
            Session["SearchEmpFullName"] = null;
            Session["SearchEmpCode"] = null;
            Session["SearchEmpID"] = null;

            SkillMatrixSearchAll model = new SkillMatrixSearchAll();
            SkillMatrixShowHistoryModel showhistory = new SkillMatrixShowHistoryModel();
            SearchByEmployeeNameModel searchbynamemodel = new SearchByEmployeeNameModel();
            model.ShowSkill = new SkillMatrixShowHistoryModel();
            searchbynamemodel.ShowSkill = new SkillMatrixShowHistoryModel();
            ViewBag.ResourcePoolType = dal.getResoucePoolNames().ToList();
            showhistory.resourcepoolListdata = dal.getResoucePoolNames().ToList();
            showhistory.getskilllist = new List<GetSkillName>();
            WSEMDBEntities wsem = new WSEMDBEntities();
            model.SearchedUserDetails = new SearchedUserDetails();
            model.ResourcePoolListDetails = dal.getResourcePoolNames();
            List<SkillList> SkillDetails = new List<SkillList>();
            model.SkillListDetails = SkillDetails;
            ViewBag.dataForDropDown = dal.DGetSkillMum();
            model.NewSearchEmp = new SearchByEmployeeNameModel();
            model.NewSearchEmp.ShowSkill = new SkillMatrixShowHistoryModel();
            model.NewSearchEmp.ShowSkill.resourcepoolListdata = dal.getResoucePoolNames().ToList();
            model.NewSearchEmp.ShowSkill.getskilllist = dal.GetSkillname(0);
            model.deatailsModel = new Details();
            string employeeCode = Membership.GetUser().UserName;
            int EmployeeCode = Convert.ToInt32(employeeCode);
            string empcode = EmployeeCode.ToString();
            tbl_PM_Employee_SEM employeeDetails = wsem.tbl_PM_Employee_SEM.Where(x => x.EmployeeCode == empcode).FirstOrDefault();
            model.EmployeeId = employeeDetails.EmployeeID;
            searchbynamemodel.EmployeeId = employeeDetails.EmployeeID;
            ViewBag.LogInUserId = EmployeeCode;
            ViewBag.IsResource = dal.GetResourcePoolNameDetails();
            ViewBag.IsRating = dal.GetRatingsDetails();
            return View("Index", model);
        }

        private GridView bindGridDemo = new GridView();

        public ActionResult ScreenExportToExcel(string FinalCol, string ProjectName, int? ResourcePoolID, int? SkillId)
        {
            DataSet newds = new DataSet();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dtCloned = new DataTable();
            string connectionString = ConfigurationManager.AppSettings["ConnectionStringForSkillMatrix"].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand sqlComm = new SqlCommand("SearchSkills_SP", conn);
                sqlComm.Parameters.AddWithValue("@Resourcepoolid", ResourcePoolID);
                sqlComm.Parameters.AddWithValue("@Skillid", SkillId);
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                da.Fill(ds);
                dt = ds.Tables[0];
                int cnt = dt.Columns.Count;
                dt1 = dt.Copy();
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.ColumnName != FinalCol && col.ColumnName != "Employee Code" && col.ColumnName != "Employee Name" && FinalCol != "" && FinalCol != "Select")
                        dt1.Columns.Remove(col.ColumnName);
                }
                dtCloned = dt1.Clone();
                foreach (DataColumn col in dt1.Columns)
                {
                    if (col.ColumnName != "Employee Code" && col.ColumnName != "Employee Name")
                    {
                        dtCloned.Columns[col.ColumnName].DataType = typeof(string);
                    }
                }
                foreach (DataRow row in dt1.Rows)
                {
                    dtCloned.ImportRow(row);
                }
                DataRow dr = dtCloned.NewRow();
                foreach (DataColumn col in dt1.Columns)
                {
                    if (col.ColumnName != "Employee Code" && col.ColumnName != "Employee Name")
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            dr = dtCloned.Rows[i];
                            if (dr[col.ColumnName].ToString() == "-99")
                            {
                                dr[col.ColumnName] = "NA";
                            }
                            if (dr[col.ColumnName].ToString() == "-999")
                            {
                                dr[col.ColumnName] = "Not Rated";
                            }
                        }
                    }
                }
            }
            bindGridDemo.DataSource = dtCloned;
            bindGridDemo.DataBind();
            Response.Clear();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Skill_Matrix.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            bindGridDemo.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return Json(new { Error = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadSkillMatrixGrid(int? ResourceIdValue, int? SkillId)
        {
            int page = 1;
            int rows = 10;
            DataSet ds = new DataSet("TimeRanges");
            string connectionString = ConfigurationManager.AppSettings["ConnectionStringForSkillMatrix"].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand sqlComm = new SqlCommand("SearchSkills_SP", conn);
                sqlComm.Parameters.AddWithValue("@Resourcepoolid", ResourceIdValue);
                if (SkillId == null)
                {
                    sqlComm.Parameters.AddWithValue("@Skillid", 0);
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@Skillid", SkillId);
                }
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                if (ResourceIdValue == null)
                {
                    return Json(new { Error = true }, JsonRequestBehavior.AllowGet);
                }

                da.Fill(ds);
                int Count = ds.Tables.Count;
                if (Count == 0)
                {
                    return Json(new { Error = true }, JsonRequestBehavior.AllowGet);
                }
                DataTable firstTable = ds.Tables[0];
                if (firstTable.Rows.Count == 0)
                {
                    return Json(new { Error = true }, JsonRequestBehavior.AllowGet);
                }

                DataTable dtCloned = firstTable.Clone();
                foreach (DataColumn col in firstTable.Columns)
                {
                    if (col.ColumnName != "Employee Code" && col.ColumnName != "Employee Name")
                    {
                        dtCloned.Columns[col.ColumnName].DataType = typeof(string);
                    }
                }
                foreach (DataRow row in firstTable.Rows)
                {
                    dtCloned.ImportRow(row);
                }
                DataRow dr = dtCloned.NewRow();
                foreach (DataColumn col in firstTable.Columns)
                {
                    if (col.ColumnName != "Employee Code" && col.ColumnName != "Employee Name")
                    {
                        for (int i = 0; i < firstTable.Rows.Count; i++)
                        {
                            dr = dtCloned.Rows[i];
                            if (dr[col.ColumnName].ToString() == "-99")
                            {
                                dr[col.ColumnName] = "NA";
                            }
                            if (dr[col.ColumnName].ToString() == "-999")
                            {
                                dr[col.ColumnName] = "Not Rated";
                            }
                        }
                    }
                }
                int recordsCount = dtCloned.Rows.Count;
                List<string> columnName = new List<string>();

                int Rowscount = Convert.ToInt32(rows);
                JqGridData objJqGrid = new JqGridData();
                objJqGrid.page = page;
                objJqGrid.total = ((recordsCount + Rowscount - 1) / Rowscount);
                objJqGrid.records = recordsCount;
                objJqGrid.rows = GetJson(dtCloned);
                int count = dtCloned.Columns.Count;
                foreach (DataColumn column in dtCloned.Columns)
                {
                    columnName.Add(column.Caption);
                }
                objJqGrid.rowsHead = columnName;
                List<object> colcontetn = new List<object>();
                foreach (var item in columnName)
                {
                    JqGridDataHeading obj = new JqGridDataHeading();
                    obj.name = item.ToString();
                    obj.index = item.ToString();
                    colcontetn.Add(obj);
                }
                objJqGrid.rowsM = colcontetn;
                JavaScriptSerializer jser = new JavaScriptSerializer();
                return Json(new { results = objJqGrid, st = "success" }, JsonRequestBehavior.AllowGet);
            }
        }

        public static string GetJson(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [HttpPost]
        public ActionResult GetSkillListBasedOnResourcePoolID(int? ResourcePoolID)
        {
            try
            {
                SkillMatrixSearchAll model = new SkillMatrixSearchAll();
                ArrayList columnName = new ArrayList();
                ArrayList rowData = new ArrayList();
                model.SkillListDetails = dal.getSkillNamesList(ResourcePoolID);
                DataSet ds = new DataSet();
                string connectionString = ConfigurationManager.AppSettings["ConnectionStringForSkillMatrix"].ToString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("SearchSkills_SP", conn);
                    sqlComm.Parameters.AddWithValue("@Resourcepoolid", ResourcePoolID);
                    sqlComm.Parameters.AddWithValue("@Skillid", 0);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    if (ResourcePoolID == null)
                    {
                        return Json(new { Error = true }, JsonRequestBehavior.AllowGet);
                    }

                    da.Fill(ds);
                    int Count = ds.Tables.Count;
                    if (Count == 0)
                    {
                        return Json(new { Error = true }, JsonRequestBehavior.AllowGet);
                    }
                    DataTable firstTable = ds.Tables[0];
                    int count = firstTable.Columns.Count;
                    foreach (DataColumn cl in firstTable.Columns)
                    {
                        columnName.Add(cl.Caption);
                    }

                    foreach (DataRow row in firstTable.Rows)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            rowData.Add(row[i]);
                        }
                    }
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json1 = js.Serialize(columnName);
                    JavaScriptSerializer js1 = new JavaScriptSerializer();
                    string json2 = js1.Serialize(rowData);
                    JavaScriptSerializer js2 = new JavaScriptSerializer();
                    string json3 = js2.Serialize(model.SkillListDetails);

                    return Json(new { results = json3, columnName = json1, Data = json2 }, JsonRequestBehavior.AllowGet);
                    // return (JsonConvert.SerializeObject(firstTable));
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult LoadGrid(int ResourcePoolId, int SkillId)
        {
            SkillMatrixSearchAll model = new SkillMatrixSearchAll();
            List<SkillMatrixSearchAll> FixBidDetails = new List<SkillMatrixSearchAll>();
            return View();
        }

        ///////////////////Prince////////////////////

        public string UploadFileLocationRMG
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadRMGFileLocation"];
                // return "Wrong Path";
            }
        }

        private SkillMatrixSearchAll model = new SkillMatrixSearchAll();

        public string Generate_Unique_Character()
        {
            string mchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var chars = mchars;
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 4)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result.ToString();
        }

        public string Get_Unique_Name(string filename)
        {
            string[] holdval = filename.Split('.');
            string fistname = holdval[0] + "_" + Generate_Unique_Character();
            string extension = holdval[1];
            string Newname = fistname + "." + extension;
            return Newname;
        }

        [HttpPost]
        public ActionResult UploadEmployeeSkill(string dropDownName, HttpPostedFileBase file, string UpdDescription)
        {
            string uploadStatus = "";
            int id = 0;
            try
            {
                id = int.Parse(dropDownName);
            }
            catch (Exception)
            {
                uploadStatus = "Select Drop Down";
                return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
            }
            int ProjID = 0;
            model.UploadStatus = false;
            bool mainResult = model.FullDoneStatus = true;
            try
            {
                if (file != null)
                {
                    FileInfo Finfo = new FileInfo(file.FileName);
                    string extension = Finfo.Extension.ToLower();
                    if (extension == ".xlsx" || extension == ".xls")
                    {
                        string uploadsPath = (UploadFileLocationRMG);//
                        string fileName = Get_Unique_Name(Path.GetFileName(file.FileName));
                        string filePath = Path.Combine(uploadsPath, fileName);
                        try
                        {
                            if (!Directory.Exists(uploadsPath))
                                Directory.CreateDirectory(uploadsPath);

                            file.SaveAs(filePath);
                        }
                        catch (Exception e)
                        {
                            uploadStatus = "Error :" + e.Message;
                            return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
                        }
                        bool checkvalid = CheckIfFileIsValid(true, filePath, extension);//To Check If File Is Valid
                        if (!checkvalid)
                        {
                            uploadStatus = "Template Invalid";
                        }
                        else
                        {
                            bool check = CheckIfEmpty(true, filePath, extension);//To Check If Uploaded File Has Rows
                            model.UploadStatus = true;
                            ViewBag.status = model.UploadStatus;
                            if (check && id == 1)
                            {
                                mainResult = true;
                            }
                            else if (check == false && id == 2)
                            {
                                mainResult = SaveData(model.UploadStatus, ProjID, extension, filePath);
                            }
                            else
                            {
                                uploadStatus = "MissMatch";
                                mainResult = false;
                            }

                            if (mainResult)
                            {
                                dal.InsertSkillMatrixUploadInfo(fileName, id, UpdDescription, dal.GetCurrentUserLoggedOn(), DateTime.Now.ToString());
                                uploadStatus = "Done";
                            }
                        }
                    }
                    else
                    {
                        uploadStatus = "NOt_Valid_File";
                    }
                }
                else
                {
                    ViewBag.status = "File Not Selected";
                    uploadStatus = "File Not Selected";
                    return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                ViewBag.status = "File Not Selected";
                uploadStatus = e.StackTrace;
            }
            finally
            {
                System.GC.Collect();
            }
            return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveData()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult DeleteFile(int documentId, string filename)
        {
            bool udd = false;
            string rootFolder = (UploadFileLocationRMG);//HardCoded
            string fullPath = rootFolder + "/" + filename;
            try
            {
                System.IO.File.Delete(fullPath);
                udd = true;
                dal.SkillmatrixUploadInfoDelete(documentId);
            }
            catch (IOException)
            {
                dal.SkillmatrixUploadInfoDelete(documentId);
                udd = true; ;
            }
            catch (Exception)
            {
                if (!System.IO.File.Exists(fullPath))
                {
                    dal.SkillmatrixUploadInfoDelete(documentId);
                    udd = true;
                }
                else
                {
                    udd = false;
                }
            }

            return Json(udd, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult LoadRMGUploadDetailsBlank(string sidx, string sord, int page, int rows, string id)
        {
            var context = new WSEMDBEntities();
            var query = from e in context.tbl_PM_SkillMatrixUploadInfo
                        select e;
            var count = query.Count();
            var jsonData = new
            {
                total = 1,
                page = page,
                records = count,
                rows = query.Select(x => new { x.ID, x.FileName, x.Type, x.Description, x.UploadedBy, x.UploadedOn }).Where(x => x.Type == 1)
                       .ToList()
                       .Select(x => new
                       {
                           ID = x.ID,
                           cell = new string[] { x.ID.ToString(), x.FileName, x.Type.ToString(), x.Description, x.UploadedBy, x.UploadedOn.ToString() }
                       }).ToArray(),
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult LoadRMGUploadDetails(string sidx, string sord, int page, int rows, string id)
        {
            var context = new WSEMDBEntities();
            var query = from e in context.tbl_PM_SkillMatrixUploadInfo
                        select e;
            var count = query.Count();
            var jsonData = new
            {
                total = 1,
                page = page,
                records = count,
                rows = query.Select(x => new { x.ID, x.FileName, x.Type, x.Description, x.UploadedBy, x.UploadedOn }).Where(x => x.Type == 2)
                        .ToList()
                     .Select(x => new
                     {
                         ID = x.ID,
                         cell = new string[] { x.ID.ToString(), x.FileName, x.Type.ToString(), x.Description, x.UploadedBy, x.UploadedOn.ToString() }
                     }).ToArray(),
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult Download(string filename)
        {
            string uploadsPath = (UploadFileLocationRMG);
            string fileName = Path.GetFileName(filename);
            string filePath = Path.Combine(uploadsPath, filename);
            string[] FileExtention = filename.Split('.');
            Response.ContentType = "application:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            System.GC.Collect();//Release File From IIS
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.TransmitFile(filePath);
            Response.End();
            return View();
        }

        public ActionResult CheckFileFound(string filename)
        {
            string uploadsPath = (UploadFileLocationRMG);
            string fileName = Path.GetFileName(filename);
            string filePath = Path.Combine(uploadsPath, filename);
            string uploadStatus = "false";
            System.GC.Collect();//Release File From IIS
            if (System.IO.File.Exists(filePath))
            {
                uploadStatus = "true";
                return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        public bool CheckIfFileIsValid(bool checkStatus, string filePath, string ext)
        {
            string name_query; bool status = false;
            int check_counter = 0;
            try
            {
                string connectionString = "";
                if (checkStatus)
                {
                    if (ext == ".xls")
                    {
                        connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (ext == ".xlsx")
                    {
                        connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                    }
                    OleDbConnection excelConnection = new OleDbConnection(connectionString);
                    excelConnection.Open();
                    var tableschema = excelConnection.GetSchema("Tables");
                    DataTable activityDataTable = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    var itemsOfWorksheet = new List<SelectListItem>(); ;
                    if (activityDataTable != null)
                    {
                        string worksheetName;
                        for (int cnt = 0; cnt < activityDataTable.Rows.Count; cnt++)
                        {
                            worksheetName = activityDataTable.Rows[cnt]["TABLE_NAME"].ToString();

                            if (worksheetName.Contains('\''))
                            {
                                worksheetName = worksheetName.Replace('\'', ' ').Trim();
                            }
                            if (worksheetName.Trim().EndsWith("$"))
                                itemsOfWorksheet.Add(new SelectListItem { Text = worksheetName.TrimEnd('$'), Value = worksheetName });
                        }
                    }
                    string sheetname = "";
                    int[] count = { };
                    for (int i = 0; i < itemsOfWorksheet.Count; i++)
                    {
                        sheetname = itemsOfWorksheet[i].Value.ToString();
                        name_query = "Select * FROM [" + sheetname + "]";
                        OleDbCommand cmd = new OleDbCommand(name_query, excelConnection);
                        OleDbDataReader dReader;
                        dReader = cmd.ExecuteReader();
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        dt.Load(dReader);
                        ds.Tables.Add(dt);
                        ArrayList columnName = new ArrayList();
                        foreach (DataTable table in ds.Tables)
                        {
                            foreach (DataColumn column in table.Columns)
                            {
                                columnName.Add(column.ColumnName);
                            }
                        }
                        if (columnName.Contains("Sr#No") && columnName.Contains("Emp Code"))
                        {
                            check_counter++;
                        }
                    }
                    if (check_counter == itemsOfWorksheet.Count)
                    {
                        status = true;
                    }
                }
            }
            catch (Exception e)
            {
                status = false;
                throw;
            }
            return status;
        }

        public bool CheckIfEmpty(bool checkStatus, string filePath, string ext)
        {
            string name_query; bool status = true;
            try
            {
                string connectionString = "";
                if (checkStatus)
                {
                    if (ext == ".xls")
                    {
                        connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (ext == ".xlsx")
                    {
                        connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                    }
                    OleDbConnection excelConnection = new OleDbConnection(connectionString);
                    excelConnection.Open();
                    var tableschema = excelConnection.GetSchema("Tables");
                    DataTable activityDataTable = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    var itemsOfWorksheet = new List<SelectListItem>(); ;
                    if (activityDataTable != null)
                    {
                        string worksheetName;
                        for (int cnt = 0; cnt < activityDataTable.Rows.Count; cnt++)
                        {
                            worksheetName = activityDataTable.Rows[cnt]["TABLE_NAME"].ToString();

                            if (worksheetName.Contains('\''))
                            {
                                worksheetName = worksheetName.Replace('\'', ' ').Trim();
                            }
                            if (worksheetName.Trim().EndsWith("$"))
                                itemsOfWorksheet.Add(new SelectListItem { Text = worksheetName.TrimEnd('$'), Value = worksheetName });
                        }
                    }
                    string sheetname = "";
                    int[] count = { };
                    for (int i = 0; i < itemsOfWorksheet.Count; i++)
                    {
                        sheetname = itemsOfWorksheet[i].Value.ToString();
                        name_query = "Select * FROM [" + sheetname + "]";
                        OleDbCommand cmd = new OleDbCommand(name_query, excelConnection);
                        OleDbDataReader dReader;
                        dReader = cmd.ExecuteReader();
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        dt.Load(dReader);
                        ds.Tables.Add(dt);
                        int r = ds.Tables[0].Rows.Count;
                        if (r != 0)
                        {
                            status = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                status = false;
                throw;
            }
            return status;
        }

        public bool SaveData(bool checkStatus, int ProjId, string ext, string filePath)
        {
            bool result = true;
            string connectionString = "";
            if (checkStatus)
            {
                if (ext == ".xls")
                {
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                else if (ext == ".xlsx")
                {
                    connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                }
                OleDbConnection excelConnection = new OleDbConnection(connectionString);
                excelConnection.Open();
                var tableschema = excelConnection.GetSchema("Tables");
                DataTable activityDataTable = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                var itemsOfWorksheet = new List<SelectListItem>(); ;
                if (activityDataTable != null)
                {
                    string worksheetName;
                    for (int cnt = 0; cnt < activityDataTable.Rows.Count; cnt++)
                    {
                        worksheetName = activityDataTable.Rows[cnt]["TABLE_NAME"].ToString();

                        if (worksheetName.Contains('\''))
                        {
                            worksheetName = worksheetName.Replace('\'', ' ').Trim();
                        }
                        if (worksheetName.Trim().EndsWith("$"))
                            itemsOfWorksheet.Add(new SelectListItem { Text = worksheetName.TrimEnd('$'), Value = worksheetName });
                    }
                }

                try
                {
                    //Mail LOoop Begin from here
                    string sheetname = "";
                    for (int i = 0; i < itemsOfWorksheet.Count; i++)
                    {
                        sheetname = itemsOfWorksheet[i].Value.ToString();
                        string name_query = "";
                        name_query = "Select * FROM [" + sheetname + "]";
                        OleDbCommand cmd = new OleDbCommand(name_query, excelConnection);
                        OleDbDataReader dReader;
                        dReader = cmd.ExecuteReader();
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        dt.Load(dReader);
                        ds.Tables.Add(dt);
                        OleDbCommand cmd1 = new OleDbCommand(name_query, excelConnection);
                        OleDbDataReader dReader1;
                        dReader1 = cmd.ExecuteReader(); int? ResourcePoolID;
                        while (dReader1.Read())
                        {
                            int? EmployeeID = dal.GetEmpID_SkillMatrixs(int.Parse(dReader1.GetValue(1).ToString()));
                            int? eid = EmployeeID.HasValue ? EmployeeID : null;
                            //================================================================================================
                            ResourcePoolID = dal.GetInfoSkillMatrixs("ResourceID", sheetname.Replace('$', ' ').Trim(), 0);
                            var columnName = new ArrayList();
                            foreach (DataTable table in ds.Tables)
                            {
                                foreach (DataColumn column in table.Columns)
                                {
                                    columnName.Add(column.ColumnName);
                                }
                            }
                            int mcolumn = columnName.Count;
                            for (int count = 3; count <= mcolumn - 1; count++)
                            {
                                int? data = dal.GetInfoSkillMatrixs("SkillId", columnName[count].ToString(), ResourcePoolID);
                                int? SkillID = data.HasValue ? data : null;
                                string rate = "Not Rated";
                                if ((dReader1.GetValue(count).ToString() != null && dReader1.GetValue(count).ToString() != ""))
                                {
                                    rate = dReader1.GetValue(count).ToString();
                                }
                                else
                                {
                                    rate = "Not Rated";
                                }
                                string UpdatedBy = dal.GetCurrentUserLoggedOn();
                                DateTime UpdatedOn = DateTime.Now;
                                dal.InsertSkillRates(eid, 0, SkillID, rate, UpdatedBy, UpdatedOn);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    result = false;
                    throw;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        //Nikita
        public ActionResult SearchByEmployeeName()
        {
            SkillMatrixSearchAll model = new SkillMatrixSearchAll();
            model.SearchedUserDetails = new SearchedUserDetails();
            string employeeCode = Membership.GetUser().UserName;
            int EmployeeCode = Convert.ToInt32(employeeCode);
            ViewBag.LogInUserId = EmployeeCode;
            ViewBag.IsResource = dal.GetResourcePoolNameDetails();
            // ViewBag.IsSkillName = dal.GetSkillNameDetails(5);
            ViewBag.IsRating = dal.GetRatingsDetails();
            return PartialView("_SearchByEmployeeName", model);
        }

        [HttpPost]
        public ActionResult LoadSearchByEmployeeName(string userEmployeecode, string searchText, int? Id, int? page, int? rows)
        {
            try
            {
                SkillMatrixDal dal = new SkillMatrixDal();
                CommonMethodsDAL commondal = new CommonMethodsDAL();
                WSEMDBEntities dbContext = new WSEMDBEntities();
                int totalCount;
                tbl_PM_Employee_SEM EmpDetails = dbContext.tbl_PM_Employee_SEM.Where(ed => ed.EmployeeCode == userEmployeecode).FirstOrDefault();
                List<Details> EmployeeSearchDetails = dal.Searchemployeebyname(Convert.ToInt32(EmpDetails.EmployeeID), searchText, Id, page, rows, out totalCount);
                int total = EmployeeSearchDetails.Count();
                if ((EmployeeSearchDetails == null || EmployeeSearchDetails.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    EmployeeSearchDetails = dal.Searchemployeebyname(Convert.ToInt32(EmpDetails.EmployeeID), searchText, Id, page, rows, out totalCount);
                }
                var totalRecords = total;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = EmployeeSearchDetails,
                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SaveSearchEmployeeByName(Details model, int? SkillID, int? ResourcePoolID, string Rating, string loggedInUserEmployeeCode)
        {
            string resultMessage = string.Empty;
            bool status = false;
            try
            {
                string ratingg = model.Rating;
                if ((Rating == null || SkillID == null) && model.ID == null)
                {
                    resultMessage = "Error";
                    status = false;
                }
                else if (ratingg == "undefined")
                {
                    resultMessage = "Errorr";
                    status = false;
                }
                else
                {
                    EmployeeDAL employeeDAL = new EmployeeDAL();
                    WSEMDBEntities wsem = new WSEMDBEntities();
                    SkillMatrixDal dal = new SkillMatrixDal();
                    tbl_PM_Employee_SEM employeeDetails = wsem.tbl_PM_Employee_SEM.Where(x => x.EmployeeCode == loggedInUserEmployeeCode).FirstOrDefault();
                    status = dal.SaveSearchEmployeeByName(model, SkillID, Rating, employeeDetails.EmployeeID, dal.GetCurrentUserLoggedOn());
                    if (status)
                        resultMessage = "Saved";
                    else
                        resultMessage = "Error";
                }
                return Json(new { results = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { results = "Error", status = status }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteSearchByEmployeeName(IList<string> Id)
        {
            try
            {
                SkillMatrixDal dal = new SkillMatrixDal();
                bool status = dal.DeleteSearchByEmployeeName(Id);
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Show_Skill(int resourcePoolID)
        {
            SkillMatrixDal dal = new SkillMatrixDal();
            Details model = new Details();
            List<Details> skillName = dal.GetSkillNameDetails(resourcePoolID);
            return Json(new { results = skillName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportToExcelOfBulkAllocationon(int id)
        {
            DataSet newds = new DataSet();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            string connectionString = ConfigurationManager.AppSettings["ConnectionStringForSkillMatrix"].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand sqlComm = new SqlCommand("getSkillData_sp", conn);
                sqlComm.Parameters.AddWithValue("@ID", id);
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                da.Fill(ds);

                dt = ds.Tables[0];
                int cnt = dt.Columns.Count;
                dt1 = dt.Copy();

                foreach (DataColumn col in dt.Columns)
                {
                    if (col.ColumnName == "ID")
                        dt1.Columns.Remove(col.ColumnName);
                }
            }
            bindGridDemo.DataSource = dt1;
            bindGridDemo.DataBind();
            Response.Clear();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Skill_Matrix.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            bindGridDemo.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return Json(new { Error = false }, JsonRequestBehavior.AllowGet);
        }

        ///////////kalindi////////////
        [HttpPost]
        public ActionResult AddSkill(int page, int rows)
        {
            try
            {
                int totalCount;
                ConfigurationSkillMatrix model = new ConfigurationSkillMatrix();
                model.SearchedUserDetails = new SearchedUserDetails();
                ViewBag.ResourcePoolName = dal.GetResourcePoolNameDetailsConfiguration();
                var test = dal.GetResourceSkilldetails(page, rows, out totalCount);
                if ((test == null || test.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    test = dal.GetResourceSkilldetails(page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = test,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        public ActionResult GetSkillname(string resourcePoolID)
        {
            ConfigurationSkillMatrix model = new ConfigurationSkillMatrix();
            List<ConfigurationSkillMatrix> alldata = dal.GetSkillName(resourcePoolID);
            return Json(alldata, JsonRequestBehavior.AllowGet);
        }

        [PageAccess(PageName = "Skill Matrix Configurations")]
        public ActionResult ConfigureSkillMatrixView()
        {
            try
            {
                Session["SearchEmpFullName"] = null;  // to hide emp search
                Session["SearchEmpCode"] = null;
                Session["SearchEmpID"] = null;

                ConfigurationSkillMatrix model = new ConfigurationSkillMatrix();
                model.SearchedUserDetails = new SearchedUserDetails();
                ViewBag.ResourcePoolName = dal.GetResourcePoolNameDetails();
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult AddSkillToolsData(ConfigurationSkillMatrix model, string ResourcePoolID, string SkillID)
        {
            try
            {
                int pool = model.ResourcePoolId;
                string resultMessage = string.Empty;
                bool status = false;
                if (pool != null)
                {
                    status = dal.SaveSkillResourceDetail(model);
                    if (status == true)
                    {
                        if (model.ToolId != null)
                        {
                            resultMessage = "Updated";
                        }
                        else
                        {
                            resultMessage = "Saved";
                        }
                    }
                    else
                    {
                        resultMessage = "Error";
                    }
                    return Json(new { result = resultMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // resultMessage = "Errorr";
                    return Json(new { result = "Errorr" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteSkillToolsData(List<int> ID)
        {
            try
            {
                string resultMessage = string.Empty;
                bool status = dal.DeleteSkillToolsData(ID);
                if (status == true)
                {
                    resultMessage = "Deleted";
                }
                else
                {
                    resultMessage = "Error";
                }
                return Json(new { result = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}