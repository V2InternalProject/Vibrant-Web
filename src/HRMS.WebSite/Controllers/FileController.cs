using HRMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;

namespace HRMS.Controllers
{
    public class FileController : ApiController
    {
        [HttpPost]
        public UploadFileResult Post()
        {
            UploadFileResult result = new UploadFileResult();
            try
            {
                result.errorOccured = false;
                var httpRequest = HttpContext.Current.Request;
                var filePath = "";
                var fileName = "";
                var uploadPath = "";
                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var moduleName = httpRequest.Form["ModuleName"];
                        if (moduleName == ModuleNames.ExpenseModule)
                        {
                            uploadPath = System.Configuration.ConfigurationManager.AppSettings["File.ExpenseUploadLocation"];
                        }
                        Guid globalID1 = Guid.NewGuid();
                        Guid globalID2 = Guid.NewGuid();
                        fileName = globalID1 + "_" + globalID2 + Path.GetExtension(postedFile.FileName);
                        filePath = Path.Combine(uploadPath, fileName);
                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);
                        postedFile.SaveAs(filePath);

                        docfiles.Add(uploadPath);
                        docfiles.Add(fileName);
                    }
                    result.FileName = fileName;
                    result.FilePath = uploadPath;
                    result.isUploaded = true;
                }
                else
                {
                    result.isUploaded = false;
                }
                return result;
            }
            catch (Exception)
            {
                result.errorOccured = true;
                return result;
            }
        }
    }

    public class UploadFileResult
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool isUploaded { get; set; }
        public bool errorOccured { get; set; }
    }
}