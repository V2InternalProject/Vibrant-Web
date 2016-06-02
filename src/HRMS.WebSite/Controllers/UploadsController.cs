using HRMS.DAL;
using HRMS.Models;
using MvcApplication3.Filters;
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

namespace HRMS.Controllers
{
    public class UploadsController : Controller
    {
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();
        private EmployeeDAL dal = new EmployeeDAL();

        public string UploadFileLocation
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadHRFileLocation"];
            }
        }

        public string UploadFileLocationRMG
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadRMGFileLocation"];
            }
        }

        //
        // GET: /Uploads/
        [HttpGet]
        public ActionResult Index(int? documentID)
        {
            ViewBag.DocumentID = documentID;
            return View();
        }

        [PageAccess(PageName = "Uploads")]
        public ActionResult Uploads(int? documentID)
        {
            Session["SearchEmpID"] = null; // hide emp search
            Session["SearchEmpFullName"] = null;
            Session["SearchEmpCode"] = null;

            UploadHRDocumentsViewModel vm = new UploadHRDocumentsViewModel();
            string employeeCode = Membership.GetUser().UserName;
            int employeeID = dal.GetEmployeeID(employeeCode);
            vm.UploadTypeValues = this.GetUploadTypes();
            ViewBag.DocumentID = documentID;
            vm.SearchedUserDetails = new SearchedUserDetails();
            vm.SearchedUserDetails.EmployeeId = employeeID;
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            string user = Commondal.GetMaxRoleForUser(role);
            vm.SearchedUserDetails.UserRole = user;
            return View(vm);
        }

        public List<SelectListItem> GetUploadTypes()
        {
            UploadsDAL uploads = new UploadsDAL();
            var uploadTypes = uploads.GetHRUploadTypes();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var uploadType in uploadTypes)
                list.Add(new SelectListItem { Selected = true, Text = uploadType.UploadType, Value = uploadType.UploadTypeId.ToString() });

            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Uploads(UploadHRDocumentsViewModel vm)
        {
            Session["SearchEmpID"] = null;  // to hide employee search
            Session["SearchEmpFullName"] = null;
            Session["SearchEmpCode"] = null;
            return View(vm);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadHRDocument(HttpPostedFileBase doc, UploadHRDocumentsViewModel model)
        {
            UploadsDAL uploads = new UploadsDAL();
            bool uploadStatus = false;

            if (doc.ContentLength > 0)
            {
                string uploadsPath = (UploadFileLocation);
                uploadsPath = Path.Combine(uploadsPath, GetUploadTypeSelectedText(model.UploadTypeId));
                string fileName = Path.GetFileName(doc.FileName);
                try
                {
                    IDocuments document = null;

                    if (!uploads.IsHRDocumentExists(Path.GetFileName(doc.FileName), model.UploadTypeId))
                    {
                        // Insert new record to parent
                        document = new Tbl_HR_Documents();
                        document.FileName = Path.GetFileName(doc.FileName);
                        ((Tbl_HR_Documents)document).FileDescription = model.FileDescription;
                        ((Tbl_HR_Documents)document).UploadTypeId = model.UploadTypeId;
                    }
                    else
                    {
                        // Insert new record to child

                        document = new Tbl_HR_DocumentDetail();
                        int documentID = 0;
                        string newNameForDocument = uploads.GetNewNameForHRDocument(Path.GetFileName(doc.FileName), model.UploadTypeId, out documentID);
                        fileName = newNameForDocument;
                        document.DocumentId = documentID;
                        document.FileName = newNameForDocument;
                    }

                    document.FilePath = uploadsPath;
                    document.Comments = model.Comments;
                    document.FileDescription = model.FileDescription;
                    document.UploadedBy = int.Parse(HttpContext.User.Identity.Name);
                    document.UploadedDate = DateTime.Now;
                    uploads.UploadHRDocument(document);

                    string filePath = Path.Combine(uploadsPath, fileName);
                    if (!Directory.Exists(uploadsPath))
                        Directory.CreateDirectory(uploadsPath);

                    doc.SaveAs(filePath);
                    uploadStatus = true;
                }
                catch (Exception)
                {
                    //throw;
                }
            }
            return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadRMGDocument(HttpPostedFileBase doc, UploadHRDocumentsViewModel model)
        {
            UploadsDAL uploads = new UploadsDAL();
            bool uploadStatus = false;

            if (doc.ContentLength > 0)
            {
                string uploadsPath = (UploadFileLocationRMG);
                uploadsPath = Path.Combine(uploadsPath, GetUploadTypeSelectedText(1));
                string fileName = Path.GetFileName(doc.FileName);
                try
                {
                    IDocuments document = null;

                    if (!uploads.IsRMGDocumentExists(Path.GetFileName(doc.FileName), model.UploadTypeId))
                    {
                        // Insert new record to parent
                        document = new Tbl_RMG_Documents();
                        document.FileName = Path.GetFileName(doc.FileName);
                        ((Tbl_RMG_Documents)document).FileDescription = model.FileDescription;
                        ((Tbl_RMG_Documents)document).UploadTypeId = 1;
                    }
                    else
                    {
                        // Insert new record to child

                        document = new Tbl_RMG_DocumentDetail();
                        int documentID = 0;
                        string newNameForDocument = uploads.GetNewNameForRMGDocument(Path.GetFileName(doc.FileName), 1, out documentID);
                        fileName = newNameForDocument;
                        document.DocumentId = documentID;
                        document.FileName = newNameForDocument;
                    }

                    document.FilePath = uploadsPath;
                    document.Comments = model.Comments;
                    document.FileDescription = model.FileDescription;
                    document.UploadedBy = int.Parse(HttpContext.User.Identity.Name);
                    document.UploadedDate = DateTime.Now;
                    uploads.UploadRMGDocument(document);

                    string filePath = Path.Combine(uploadsPath, fileName);
                    if (!Directory.Exists(uploadsPath))
                        Directory.CreateDirectory(uploadsPath);

                    doc.SaveAs(filePath);
                    uploadStatus = true;
                }
                catch (Exception)
                {
                    //throw;
                }
            }
            return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadHRUploadDetails(int page, int rows)
        {
            UploadsDAL uploads = new UploadsDAL();
            try
            {
                List<UploadHRDocumentsViewModel> Result = uploads.GetHRDocumentForDispay(page, rows);
                if ((Result == null || Result.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    Result = uploads.GetHRDocumentForDispay(page, rows);
                }

                int totalCount = uploads.GetHRDocumentForDispayTotalCount();
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

        [HttpPost]
        public ActionResult LoadRMGUploadDetails(int page, int rows)
        {
            UploadsDAL uploads = new UploadsDAL();
            try
            {
                List<UploadHRDocumentsViewModel> Result = uploads.GetRMGDocumentForDispay(page, rows);
                if ((Result == null || Result.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    Result = uploads.GetHRDocumentForDispay(page, rows);
                }

                int totalCount = uploads.GetRMGDocumentForDispayTotalCount();
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="disciplineId"></param>
        /// <returns></returns>
        public ActionResult DeleteHRUploadDetails(int documentId)
        {
            UploadsDAL uploads = new UploadsDAL();
            HRMSDBEntities dbContext = new HRMSDBEntities();
            bool udd = false;
            try
            {
                var parentDoc = dbContext.Tbl_HR_Documents.Where(x => x.DocumentId == documentId).FirstOrDefault();
                var versionDocs = dbContext.Tbl_HR_DocumentDetail.Where(x => x.DocumentId == documentId).ToList();
                string rootFolder = (UploadFileLocation);
                string subfolderpath = Path.Combine(rootFolder, GetUploadTypeTextFromDocId(parentDoc.DocumentId));
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

                udd = uploads.DeleteHRUploadDetails(documentId);
                return Json(udd, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                udd = false;
                return Json(udd, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteRMGUploadDetails(int documentId)
        {
            UploadsDAL uploads = new UploadsDAL();
            HRMSDBEntities dbContext = new HRMSDBEntities();
            bool udd = false;
            try
            {
                var parentDoc = dbContext.Tbl_RMG_Documents.Where(x => x.DocumentId == documentId).FirstOrDefault();
                var versionDocs = dbContext.Tbl_RMG_DocumentDetail.Where(x => x.DocumentId == documentId).ToList();
                string rootFolder = (UploadFileLocationRMG);
                string subfolderpath = Path.Combine(rootFolder, GetUploadTypeTextFromDocIdRMG(parentDoc.DocumentId));
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

                udd = uploads.DeleteRMGUploadDetails(documentId);
                return Json(udd, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                udd = false;
                return Json(udd, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public ActionResult DeleteHRDocsSelected(List<string> filenames)
        {
            UploadsDAL uploads = new UploadsDAL();
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
                if (loginemployeerole.Contains("RMG"))
                {
                    if (filenames != null)
                    {
                        foreach (string filename in filenames)
                        {
                            var documentformchild = (from document in dbContext.Tbl_RMG_Documents
                                                     join documentDetails in dbContext.Tbl_RMG_DocumentDetail
                                                     on document.DocumentId equals documentDetails.DocumentId
                                                     where documentDetails.FileName == filename
                                                     select documentDetails).FirstOrDefault();

                            string rootFolder = (UploadFileLocationRMG);
                            string subfolderpath = Path.Combine(rootFolder, GetUploadTypeTextFromDocIdRMG(documentformchild.DocumentId));
                            string Filepath = Path.Combine(subfolderpath, filename);

                            if (System.IO.File.Exists(Filepath))
                                System.IO.File.Delete(Filepath);
                            result = uploads.DeleteRMGDocsSelected(filename);
                        }
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (filenames != null)
                    {
                        foreach (string filename in filenames)
                        {
                            var documentformchild = (from document in dbContext.Tbl_HR_Documents
                                                     join documentDetails in dbContext.Tbl_HR_DocumentDetail
                                                     on document.DocumentId equals documentDetails.DocumentId
                                                     where documentDetails.FileName == filename
                                                     select documentDetails).FirstOrDefault();

                            string rootFolder = (UploadFileLocation);
                            string subfolderpath = Path.Combine(rootFolder, GetUploadTypeTextFromDocId(documentformchild.DocumentId));
                            string Filepath = Path.Combine(subfolderpath, filename);

                            if (System.IO.File.Exists(Filepath))
                                System.IO.File.Delete(Filepath);
                            result = uploads.DeleteHRDocsSelected(filename);
                        }
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Action will fire when user clicks on the Filename,to download the file,
        /// when viewing the history/Details view of files
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ActionResult DownloadHRFile(string filename, int uploadTypeId)
        {
            HRMSDBEntities dbContext = new HRMSDBEntities();
            UploadsDAL RMGupload = new UploadsDAL();
            string Loginemployeecode = string.Empty;
            string[] loginemployeerole = { };
            EmployeeDAL empdal = new EmployeeDAL();
            int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);
            HRMS_tbl_PM_Employee loginrolescheck = empdal.GetEmployeeDetails(employeeID);
            Loginemployeecode = loginrolescheck.EmployeeCode;
            loginemployeerole = Roles.GetRolesForUser(Loginemployeecode);

            try
            {
                if (loginemployeerole.Contains("RMG"))
                {
                    var documentformchild = (from document in dbContext.Tbl_RMG_Documents
                                             join documentDetails in dbContext.Tbl_RMG_DocumentDetail
                                             on document.DocumentId equals documentDetails.DocumentId
                                             where document.UploadTypeId == uploadTypeId && documentDetails.FileName == filename
                                             select documentDetails).FirstOrDefault();

                    var documentfromparent = (from document in dbContext.Tbl_RMG_Documents
                                              where document.UploadTypeId == uploadTypeId && document.FileName == filename
                                              select document).FirstOrDefault();

                    string rootFolder = (UploadFileLocationRMG);
                    string[] FileExtention = filename.Split('.');
                    string contentType = "application/" + FileExtention[1];

                    if (documentformchild != null)
                    {
                        string subfolderpath = Path.Combine(rootFolder, GetUploadTypeTextFromDocIdRMG(documentformchild.DocumentId));
                        string Filepath = Path.Combine(subfolderpath, filename);
                        if (!System.IO.File.Exists(Filepath))
                        {
                            throw new Exception();
                        }
                        return File(Filepath, contentType, filename);
                    }
                    else
                    {
                        string subfolderpath = Path.Combine(rootFolder, GetUploadTypeTextFromDocIdRMG(documentfromparent.DocumentId));
                        string Filepath = Path.Combine(subfolderpath, filename);
                        if (!System.IO.File.Exists(Filepath))
                        {
                            throw new Exception();
                        }
                        return File(Filepath, contentType, filename);
                    }
                }
                else
                {
                    var documentformchild = (from document in dbContext.Tbl_HR_Documents
                                             join documentDetails in dbContext.Tbl_HR_DocumentDetail
                                             on document.DocumentId equals documentDetails.DocumentId
                                             where document.UploadTypeId == uploadTypeId && documentDetails.FileName == filename
                                             select documentDetails).FirstOrDefault();

                    var documentfromparent = (from document in dbContext.Tbl_HR_Documents
                                              where document.UploadTypeId == uploadTypeId && document.FileName == filename
                                              select document).FirstOrDefault();

                    string rootFolder = (UploadFileLocation);
                    string[] FileExtention = filename.Split('.');
                    string contentType = "application/" + FileExtention[1];

                    if (documentformchild != null)
                    {
                        string subfolderpath = Path.Combine(rootFolder, GetUploadTypeTextFromDocId(documentformchild.DocumentId));
                        string Filepath = Path.Combine(subfolderpath, filename);
                        if (!System.IO.File.Exists(Filepath))
                        {
                            throw new Exception();
                        }
                        return File(Filepath, contentType, filename);
                    }
                    else
                    {
                        string subfolderpath = Path.Combine(rootFolder, GetUploadTypeTextFromDocId(documentfromparent.DocumentId));
                        string Filepath = Path.Combine(subfolderpath, filename);
                        if (!System.IO.File.Exists(Filepath))
                        {
                            throw new Exception();
                        }
                        return File(Filepath, contentType, filename);
                    }
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

        /// <summary>
        /// Action methd will hit when user will click on the Details link in gridview,
        /// to view the History of the files
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult ShowHistoryHrDocUploads(int documentId, string uploadType)
        {
            HRMSDBEntities dbContext = new HRMSDBEntities();
            UploadsDAL uploads = new UploadsDAL();
            List<Tbl_HR_DocumentDetail> objHRDocDetails = new List<Tbl_HR_DocumentDetail>();
            Tbl_HR_Documents objhRDoc = new Tbl_HR_Documents();
            List<UploadHRDocumentsViewModel> objDocList = new List<UploadHRDocumentsViewModel>();
            int uploadTypeId = dbContext.Tbl_HR_UploadType.Where(x => x.UploadType == uploadType).FirstOrDefault().UploadTypeId;
            try
            {
                objhRDoc = uploads.GetHRDocument(documentId);
                objHRDocDetails = uploads.GetHRDocumentHistoryForDisplay(documentId);

                foreach (Tbl_HR_DocumentDetail eachDocDetail in objHRDocDetails)
                {
                    UploadHRDocumentsViewModel dd = new UploadHRDocumentsViewModel()
                    {
                        DocumentID = eachDocDetail.DocumentId,
                        Comments = eachDocDetail.Comments,
                        FileDescription = eachDocDetail.FileDescription,
                        FileName = eachDocDetail.FileName,
                        UploadedBy = uploads.GetUploadNameFromUploadById(HttpContext.User.Identity.Name),
                        UploadedDate = (eachDocDetail.UploadedDate).Value,
                        FilePath = eachDocDetail.FilePath,
                        UploadTypeId = uploadTypeId
                    };
                    objDocList.Add(dd);
                }

                UploadHRDocumentsViewModel dd1 = new UploadHRDocumentsViewModel()
                {
                    DocumentID = objhRDoc.DocumentId,
                    Comments = objhRDoc.Comments,
                    FileDescription = objhRDoc.FileDescription,
                    FileName = objhRDoc.FileName,
                    UploadedBy = uploads.GetUploadNameFromUploadById(HttpContext.User.Identity.Name),
                    UploadedDate = (objhRDoc.UploadedDate).Value,
                    FilePath = objhRDoc.FilePath,
                    UploadTypeId = uploadTypeId
                };

                objDocList.Add(dd1);
                return PartialView("_ShowHRDocHistory", objDocList);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult ShowHistoryRMGDocUploads(int documentId, string uploadType)
        {
            HRMSDBEntities dbContext = new HRMSDBEntities();
            UploadsDAL uploads = new UploadsDAL();
            List<Tbl_RMG_DocumentDetail> objHRDocDetails = new List<Tbl_RMG_DocumentDetail>();
            Tbl_RMG_Documents objhRDoc = new Tbl_RMG_Documents();
            List<UploadHRDocumentsViewModel> objDocList = new List<UploadHRDocumentsViewModel>();
            int uploadTypeId = dbContext.Tbl_HR_UploadType.Where(x => x.UploadType == uploadType).FirstOrDefault().UploadTypeId;
            try
            {
                objhRDoc = uploads.GetRMGDocument(documentId);
                objHRDocDetails = uploads.GetRMGDocumentHistoryForDisplay(documentId);

                foreach (Tbl_RMG_DocumentDetail eachDocDetail in objHRDocDetails)
                {
                    UploadHRDocumentsViewModel dd = new UploadHRDocumentsViewModel()
                    {
                        DocumentID = eachDocDetail.DocumentId,
                        Comments = eachDocDetail.Comments,
                        FileDescription = eachDocDetail.FileDescription,
                        FileName = eachDocDetail.FileName,
                        UploadedBy = uploads.GetUploadNameFromUploadById(HttpContext.User.Identity.Name),
                        UploadedDate = (eachDocDetail.UploadedDate).Value,
                        FilePath = eachDocDetail.FilePath,
                        UploadTypeId = uploadTypeId
                    };
                    objDocList.Add(dd);
                }

                UploadHRDocumentsViewModel dd1 = new UploadHRDocumentsViewModel()
                {
                    DocumentID = objhRDoc.DocumentId,
                    Comments = objhRDoc.Comments,
                    FileDescription = objhRDoc.FileDescription,
                    FileName = objhRDoc.FileName,
                    UploadedBy = uploads.GetUploadNameFromUploadById(HttpContext.User.Identity.Name),
                    UploadedDate = (objhRDoc.UploadedDate).Value,
                    FilePath = objhRDoc.FilePath,
                    UploadTypeId = uploadTypeId
                };

                objDocList.Add(dd1);
                return PartialView("_ShowHRDocHistory", objDocList);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        /// <summary>
        /// Method to get uploadTypeText from DocId
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public string GetUploadTypeTextFromDocId(int documentId)
        {
            string uploadTypeText = string.Empty;
            HRMSDBEntities dbContext = new HRMSDBEntities();
            try
            {
                var uploadTypeId = (from ut in dbContext.Tbl_HR_Documents
                                    where ut.DocumentId == documentId
                                    select ut.UploadTypeId).FirstOrDefault();

                uploadTypeText = GetUploadTypeSelectedText(uploadTypeId);

                return uploadTypeText;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetUploadTypeTextFromDocIdRMG(int documentId)
        {
            string uploadTypeText = string.Empty;
            HRMSDBEntities dbContext = new HRMSDBEntities();
            try
            {
                var uploadTypeId = (from ut in dbContext.Tbl_RMG_Documents
                                    where ut.DocumentId == documentId
                                    select ut.UploadTypeId).FirstOrDefault();

                uploadTypeText = GetUploadTypeSelectedText(uploadTypeId);

                return uploadTypeText;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetUploadTypeSelectedText(int UploadTypeId)
        {
            UploadsDAL uploads = new UploadsDAL();
            var uploadTypes = uploads.GetHRUploadTypes();

            return uploadTypes.Where(u => u.UploadTypeId == UploadTypeId).FirstOrDefault().UploadType;
        }

        public ActionResult OrbitStart()
        {
            UploadHRDocumentsViewModel vm = new UploadHRDocumentsViewModel();
            vm.SearchedUserDetails = new SearchedUserDetails();
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            //added
            string user = Commondal.GetMaxRoleForUser(role);
            vm.SearchedUserDetails.UserRole = user;

            return View("Demo", vm);
        }

        public ActionResult Importexcel()
        {
            bool uploadStatus = false;
            string connectionString = "";
            if (Request.Files["FileUpload1"].ContentLength > 0)
            {
                string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName);
                string uploadsPath = (UploadFileLocation);

                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                string filePath = Path.Combine(uploadsPath, System.IO.Path.GetFileName(Request.Files["FileUpload1"].FileName));
                Request.Files["FileUpload1"].SaveAs(filePath);

                if (extension == ".xls")
                {
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                else if (extension == ".xlsx")
                {
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }

                OleDbConnection excelConnection = new OleDbConnection(connectionString);
                OleDbCommand cmd = new OleDbCommand("Select [Employee Code],[Reporting Manager Code],[Competency Manager Code],[Confirmation / Exit Process Manager Code],[Business Group],[BusinessGroupID],[Organization Unit],[OrganizationUnitID],[Parent DU],[ParentDUID],[Current DU],[CurrentDUID],[Delivery Team],[DT ID],[Resource Pool Name],[Resource Pool ID],[Skill Name],[Skill ID],[Skill Level],[Proficiency ID],[BusinessGroup],[BusinessGroupID],[Location],[Locationid],[DU],[DUId],[DT],[DTId],[Resourcepoolname],[Resourcepoolid],[Skill],[ToolId],[Description],[ProficiencyId] from [Sheet2$]", excelConnection);
                //OleDbCommand cmd = new OleDbCommand("Select [Employee Code],[Reporting Manager Code],[Competency Manager Code],[Confirmation / Exit Process Manager Code] from [Sheet2$]", excelConnection);

                excelConnection.Open();

                OleDbDataReader dReader;
                dReader = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                dt.Load(dReader);
                ds.Tables.Add(dt);

                UploadsDAL uploadDAL = new UploadsDAL();

                uploadDAL.GetExcelData(ds);
                uploadStatus = true;

                excelConnection.Close();
            }

            return Json(new { status = uploadStatus }, "text/html", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UploadFile(string ModuleName, string FormName, string FileNameProp, string FilePathProp)
        {
            try
            {
                UploadModel model = new UploadModel();
                model.ModuleName = ModuleName;
                model.FormName = FormName;
                model.FileNameProp = FileNameProp;
                model.FilePathProp = FilePathProp;
                return PartialView("_UploadFile", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }
    }
}