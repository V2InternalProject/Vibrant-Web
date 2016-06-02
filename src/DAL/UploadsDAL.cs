using HRMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HRMS.DAL
{
    public class UploadsDAL
    {
        private HRMSDBEntities dbContext = new HRMSDBEntities();

        /// <summary>
        /// To retrieve HR Document detail
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns></returns>
        public List<Tbl_HR_Documents> GetHRDocuments()
        {
            try
            {
                List<Tbl_HR_Documents> hRDocument = (from hrdoc in dbContext.Tbl_HR_Documents
                                                     orderby hrdoc.DocumentId descending
                                                     select hrdoc).ToList<Tbl_HR_Documents>();
                return hRDocument;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Tbl_RMG_Documents> GetRMGDocuments()
        {
            try
            {
                List<Tbl_RMG_Documents> hRDocument = (from hrdoc in dbContext.Tbl_RMG_Documents
                                                      orderby hrdoc.DocumentId descending
                                                      select hrdoc).ToList<Tbl_RMG_Documents>();
                return hRDocument;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To retrieve employee document detail
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns></returns>
        public List<Tbl_Employee_Documents> GetEmployeeDocuments(int employeeId)
        {
            try
            {
                List<Tbl_Employee_Documents> EmpDocument = (from empdoc in dbContext.Tbl_Employee_Documents
                                                            orderby empdoc.DocumentId descending
                                                            where empdoc.EmployeeId == employeeId
                                                            select empdoc)
                                                           .ToList<Tbl_Employee_Documents>();
                return EmpDocument;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To retrieve HR Document detail
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns></returns>
        public Tbl_HR_Documents GetHRDocument(int documentID)
        {
            try
            {
                Tbl_HR_Documents hRDocument = dbContext.Tbl_HR_Documents.Where(ed => ed.DocumentId == documentID).FirstOrDefault();
                return hRDocument;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tbl_RMG_Documents GetRMGDocument(int documentID)
        {
            try
            {
                Tbl_RMG_Documents hRDocument = dbContext.Tbl_RMG_Documents.Where(ed => ed.DocumentId == documentID).FirstOrDefault();
                return hRDocument;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To retrieve Employee Document detail
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns></returns>
        public Tbl_Employee_Documents GetEmployeeDocument(int documentID)
        {
            try
            {
                Tbl_Employee_Documents EmpDocument = dbContext.Tbl_Employee_Documents.Where(ed => ed.DocumentId == documentID).FirstOrDefault();
                return EmpDocument;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// For HR, method to count the no of records, available
        /// </summary>
        /// <returns></returns>
        public int GetHRDocumentForDispayTotalCount()
        {
            int totalCount = 0;
            List<UploadHRDocumentsViewModel> model = new List<UploadHRDocumentsViewModel>();
            UploadHRDocumentsViewModel objhrDocDetails = new UploadHRDocumentsViewModel();
            try
            {
                List<Tbl_HR_Documents> hRDocument = GetHRDocuments();
                foreach (Tbl_HR_Documents eachhrDoc in hRDocument)
                {
                    var uploadTypeText = (from document in dbContext.Tbl_HR_Documents
                                          join uploadType in dbContext.Tbl_HR_UploadType
                                          on document.UploadTypeId equals uploadType.UploadTypeId
                                          where document.DocumentId == eachhrDoc.DocumentId
                                          select uploadType.UploadType).FirstOrDefault();

                    var fileDescription = (from documentId in dbContext.Tbl_HR_Documents where documentId.DocumentId == eachhrDoc.DocumentId select documentId.FileDescription).FirstOrDefault();

                    var id = (from documentId in dbContext.Tbl_HR_DocumentDetail where documentId.DocumentId == eachhrDoc.DocumentId select documentId.DocumentId).FirstOrDefault();

                    if (id != 0)
                    {
                        var hRDocumentDetails = (from hrDocDetails in dbContext.Tbl_HR_DocumentDetail
                                                 where hrDocDetails.DocumentId == eachhrDoc.DocumentId
                                                 orderby hrDocDetails.DocumentDetailId descending
                                                 select hrDocDetails
                                                 ).First();

                        if (fileDescription != null)
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = hRDocumentDetails.FileDescription,
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        else
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        model.Add(objhrDocDetails);
                    }
                    else
                    {
                        var hRDocumentDetails = (from hrDocDetails in dbContext.Tbl_HR_Documents
                                                 where hrDocDetails.DocumentId == eachhrDoc.DocumentId
                                                 orderby hrDocDetails.DocumentId descending
                                                 select hrDocDetails
                                                    ).First();

                        if (fileDescription != null)
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = hRDocumentDetails.FileDescription,
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        else
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        model.Add(objhrDocDetails);
                    }
                }
                totalCount = model.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return totalCount;
        }

        public int GetRMGDocumentForDispayTotalCount()
        {
            int totalCount = 0;
            List<UploadHRDocumentsViewModel> model = new List<UploadHRDocumentsViewModel>();
            UploadHRDocumentsViewModel objhrDocDetails = new UploadHRDocumentsViewModel();
            try
            {
                List<Tbl_RMG_Documents> hRDocument = GetRMGDocuments();
                foreach (Tbl_RMG_Documents eachhrDoc in hRDocument)
                {
                    var uploadTypeText = (from document in dbContext.Tbl_RMG_Documents
                                          join uploadType in dbContext.Tbl_HR_UploadType
                                          on document.UploadTypeId equals uploadType.UploadTypeId
                                          where document.DocumentId == eachhrDoc.DocumentId
                                          select uploadType.UploadType).FirstOrDefault();

                    var fileDescription = (from documentId in dbContext.Tbl_RMG_Documents where documentId.DocumentId == eachhrDoc.DocumentId select documentId.FileDescription).FirstOrDefault();

                    var id = (from documentId in dbContext.Tbl_RMG_DocumentDetail where documentId.DocumentId == eachhrDoc.DocumentId select documentId.DocumentId).FirstOrDefault();

                    if (id != 0)
                    {
                        var hRDocumentDetails = (from hrDocDetails in dbContext.Tbl_RMG_DocumentDetail
                                                 where hrDocDetails.DocumentId == eachhrDoc.DocumentId
                                                 orderby hrDocDetails.DocumentDetailId descending
                                                 select hrDocDetails
                                                 ).First();

                        if (fileDescription != null)
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = hRDocumentDetails.FileDescription,
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        else
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        model.Add(objhrDocDetails);
                    }
                    else
                    {
                        var hRDocumentDetails = (from hrDocDetails in dbContext.Tbl_RMG_Documents
                                                 where hrDocDetails.DocumentId == eachhrDoc.DocumentId
                                                 orderby hrDocDetails.DocumentId descending
                                                 select hrDocDetails
                                                    ).First();

                        if (fileDescription != null)
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = hRDocumentDetails.FileDescription,
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        else
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        model.Add(objhrDocDetails);
                    }
                }
                totalCount = model.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return totalCount;
        }

        /// <summary>
        /// Method to load the gridview with UploadDocument details
        /// </summary>
        /// <returns></returns>
        public List<UploadHRDocumentsViewModel> GetHRDocumentForDispay(int page, int rows)
        {
            List<UploadHRDocumentsViewModel> model = new List<UploadHRDocumentsViewModel>();
            UploadHRDocumentsViewModel objhrDocDetails = new UploadHRDocumentsViewModel();
            try
            {
                List<Tbl_HR_Documents> hRDocument = GetHRDocuments();
                foreach (Tbl_HR_Documents eachhrDoc in hRDocument)
                {
                    var uploadTypeText = (from document in dbContext.Tbl_HR_Documents
                                          join uploadType in dbContext.Tbl_HR_UploadType
                                          on document.UploadTypeId equals uploadType.UploadTypeId
                                          where document.DocumentId == eachhrDoc.DocumentId
                                          select uploadType.UploadType).FirstOrDefault();

                    var fileDescription = (from documentId in dbContext.Tbl_HR_Documents where documentId.DocumentId == eachhrDoc.DocumentId select documentId.FileDescription).FirstOrDefault();

                    var id = (from documentId in dbContext.Tbl_HR_DocumentDetail where documentId.DocumentId == eachhrDoc.DocumentId select documentId.DocumentId).FirstOrDefault();

                    if (id != 0)
                    {
                        var hRDocumentDetails = (from hrDocDetails in dbContext.Tbl_HR_DocumentDetail
                                                 where hrDocDetails.DocumentId == eachhrDoc.DocumentId
                                                 orderby hrDocDetails.DocumentDetailId descending
                                                 select hrDocDetails
                                                 ).First();

                        if (fileDescription != null)
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = hRDocumentDetails.FileDescription,
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        else
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        model.Add(objhrDocDetails);
                    }
                    else
                    {
                        var hRDocumentDetails = (from hrDocDetails in dbContext.Tbl_HR_Documents
                                                 where hrDocDetails.DocumentId == eachhrDoc.DocumentId
                                                 orderby hrDocDetails.DocumentId descending
                                                 select hrDocDetails
                                                    ).First();

                        if (fileDescription != null)
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = hRDocumentDetails.FileDescription,
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        else
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        model.Add(objhrDocDetails);
                    }
                }
                return (model).Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UploadHRDocumentsViewModel> GetRMGDocumentForDispay(int page, int rows)
        {
            List<UploadHRDocumentsViewModel> model = new List<UploadHRDocumentsViewModel>();
            UploadHRDocumentsViewModel objhrDocDetails = new UploadHRDocumentsViewModel();
            try
            {
                List<Tbl_RMG_Documents> hRDocument = GetRMGDocuments();
                foreach (Tbl_RMG_Documents eachhrDoc in hRDocument)
                {
                    var uploadTypeText = (from document in dbContext.Tbl_RMG_Documents
                                          join uploadType in dbContext.Tbl_HR_UploadType
                                          on document.UploadTypeId equals uploadType.UploadTypeId
                                          where document.DocumentId == eachhrDoc.DocumentId
                                          select uploadType.UploadType).FirstOrDefault();

                    var fileDescription = (from documentId in dbContext.Tbl_RMG_Documents where documentId.DocumentId == eachhrDoc.DocumentId select documentId.FileDescription).FirstOrDefault();

                    var id = (from documentId in dbContext.Tbl_RMG_DocumentDetail where documentId.DocumentId == eachhrDoc.DocumentId select documentId.DocumentId).FirstOrDefault();

                    if (id != 0)
                    {
                        var hRDocumentDetails = (from hrDocDetails in dbContext.Tbl_RMG_DocumentDetail
                                                 where hrDocDetails.DocumentId == eachhrDoc.DocumentId
                                                 orderby hrDocDetails.DocumentDetailId descending
                                                 select hrDocDetails
                                                 ).First();

                        if (fileDescription != null)
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = hRDocumentDetails.FileDescription,
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        else
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        model.Add(objhrDocDetails);
                    }
                    else
                    {
                        var hRDocumentDetails = (from hrDocDetails in dbContext.Tbl_RMG_Documents
                                                 where hrDocDetails.DocumentId == eachhrDoc.DocumentId
                                                 orderby hrDocDetails.DocumentId descending
                                                 select hrDocDetails
                                                    ).First();

                        if (fileDescription != null)
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = hRDocumentDetails.FileDescription,
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        else
                        {
                            objhrDocDetails = new UploadHRDocumentsViewModel()
                            {
                                FilePath = hRDocumentDetails.FilePath,
                                DocumentID = hRDocumentDetails.DocumentId,
                                Comments = hRDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = hRDocumentDetails.FileName
                            };
                        }
                        model.Add(objhrDocDetails);
                    }
                }
                return (model).Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// method to count the no of records, available against that employeeId
        /// </summary>
        /// by vijay
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public int GetEmpDocumentForDispayTotalCount(int employeeId)
        {
            List<UploadEmployeeDocumentsViewModel> model = new List<UploadEmployeeDocumentsViewModel>();
            UploadEmployeeDocumentsViewModel objEmpDocDetails = new UploadEmployeeDocumentsViewModel();
            try
            {
                List<Tbl_Employee_Documents> EmpDocument = GetEmployeeDocuments(employeeId);
                foreach (Tbl_Employee_Documents eachEmpDoc in EmpDocument)
                {
                    var uploadTypeText = (from document in dbContext.Tbl_Employee_Documents
                                          join uploadType in dbContext.Tbl_Employee_UploadType
                                          on document.UploadTypeId equals uploadType.UploadTypeId
                                          where document.DocumentId == eachEmpDoc.DocumentId
                                          select uploadType.UploadType).FirstOrDefault();

                    var fileDescription = (from documentId in dbContext.Tbl_Employee_Documents where documentId.DocumentId == eachEmpDoc.DocumentId select documentId.FileDescription).FirstOrDefault();

                    var id = (from documentId in dbContext.Tbl_Employee_DocumentDetail where documentId.DocumentId == eachEmpDoc.DocumentId select documentId.DocumentId).FirstOrDefault();

                    if (id != 0)
                    {
                        var EmpDocumentDetails = (from EmpDocDetails in dbContext.Tbl_Employee_DocumentDetail
                                                  where EmpDocDetails.DocumentId == eachEmpDoc.DocumentId
                                                  orderby EmpDocDetails.DocumentDetailId descending
                                                  select EmpDocDetails
                                                 ).First();

                        if (fileDescription != null)
                        {
                            objEmpDocDetails = new UploadEmployeeDocumentsViewModel()
                            {
                                FilePath = EmpDocumentDetails.FilePath,
                                DocumentID = EmpDocumentDetails.DocumentId,
                                Comments = EmpDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = EmpDocumentDetails.FileDescription,
                                FileName = EmpDocumentDetails.FileName,
                                EmployeeId = employeeId
                            };
                        }
                        else
                        {
                            objEmpDocDetails = new UploadEmployeeDocumentsViewModel()
                            {
                                FilePath = EmpDocumentDetails.FilePath,
                                DocumentID = EmpDocumentDetails.DocumentId,
                                Comments = EmpDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = EmpDocumentDetails.FileName,
                                EmployeeId = employeeId
                            };
                        }
                        model.Add(objEmpDocDetails);
                    }
                    else
                    {
                        var EmpDocumentDetails = (from EmpDocDetails in dbContext.Tbl_Employee_Documents
                                                  where EmpDocDetails.DocumentId == eachEmpDoc.DocumentId
                                                  orderby EmpDocDetails.DocumentId descending
                                                  select EmpDocDetails
                                                    ).First();

                        if (fileDescription != null)
                        {
                            objEmpDocDetails = new UploadEmployeeDocumentsViewModel()
                            {
                                FilePath = EmpDocumentDetails.FilePath,
                                DocumentID = EmpDocumentDetails.DocumentId,
                                Comments = EmpDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = EmpDocumentDetails.FileDescription,
                                FileName = EmpDocumentDetails.FileName,
                                EmployeeId = employeeId
                            };
                        }
                        else
                        {
                            objEmpDocDetails = new UploadEmployeeDocumentsViewModel()
                            {
                                FilePath = EmpDocumentDetails.FilePath,
                                DocumentID = EmpDocumentDetails.DocumentId,
                                Comments = EmpDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = EmpDocumentDetails.FileName,
                                EmployeeId = employeeId
                            };
                        }
                        model.Add(objEmpDocDetails);
                    }
                }
                return (model).Count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to load the gridview with Employee UploadDocument details
        /// </summary>
        /// by vijay
        /// <returns></returns>
        public List<UploadEmployeeDocumentsViewModel> GetEmpDocumentForDispay(int page, int rows, int employeeId)
        {
            List<UploadEmployeeDocumentsViewModel> model = new List<UploadEmployeeDocumentsViewModel>();
            UploadEmployeeDocumentsViewModel objEmpDocDetails = new UploadEmployeeDocumentsViewModel();
            try
            {
                List<Tbl_Employee_Documents> EmpDocument = GetEmployeeDocuments(employeeId);
                foreach (Tbl_Employee_Documents eachEmpDoc in EmpDocument)
                {
                    var uploadTypeText = (from document in dbContext.Tbl_Employee_Documents
                                          join uploadType in dbContext.Tbl_Employee_UploadType
                                          on document.UploadTypeId equals uploadType.UploadTypeId
                                          where document.DocumentId == eachEmpDoc.DocumentId
                                          select uploadType.UploadType).FirstOrDefault();

                    var fileDescription = (from documentId in dbContext.Tbl_Employee_Documents where documentId.DocumentId == eachEmpDoc.DocumentId select documentId.FileDescription).FirstOrDefault();

                    var id = (from documentId in dbContext.Tbl_Employee_DocumentDetail where documentId.DocumentId == eachEmpDoc.DocumentId select documentId.DocumentId).FirstOrDefault();

                    if (id != 0)
                    {
                        var EmpDocumentDetails = (from EmpDocDetails in dbContext.Tbl_Employee_DocumentDetail
                                                  where EmpDocDetails.DocumentId == eachEmpDoc.DocumentId
                                                  orderby EmpDocDetails.DocumentDetailId descending
                                                  select EmpDocDetails
                                                 ).First();

                        if (fileDescription != null)
                        {
                            objEmpDocDetails = new UploadEmployeeDocumentsViewModel()
                            {
                                FilePath = EmpDocumentDetails.FilePath,
                                DocumentID = EmpDocumentDetails.DocumentId,
                                Comments = EmpDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = EmpDocumentDetails.FileDescription,
                                FileName = EmpDocumentDetails.FileName,
                                EmployeeId = employeeId
                            };
                        }
                        else
                        {
                            objEmpDocDetails = new UploadEmployeeDocumentsViewModel()
                            {
                                FilePath = EmpDocumentDetails.FilePath,
                                DocumentID = EmpDocumentDetails.DocumentId,
                                Comments = EmpDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = EmpDocumentDetails.FileName,
                                EmployeeId = employeeId
                            };
                        }
                        model.Add(objEmpDocDetails);
                    }
                    else
                    {
                        var EmpDocumentDetails = (from EmpDocDetails in dbContext.Tbl_Employee_Documents
                                                  where EmpDocDetails.DocumentId == eachEmpDoc.DocumentId
                                                  orderby EmpDocDetails.DocumentId descending
                                                  select EmpDocDetails
                                                    ).First();

                        if (fileDescription != null)
                        {
                            objEmpDocDetails = new UploadEmployeeDocumentsViewModel()
                            {
                                FilePath = EmpDocumentDetails.FilePath,
                                DocumentID = EmpDocumentDetails.DocumentId,
                                Comments = EmpDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileDescription = EmpDocumentDetails.FileDescription,
                                FileName = EmpDocumentDetails.FileName,
                                EmployeeId = employeeId
                            };
                        }
                        else
                        {
                            objEmpDocDetails = new UploadEmployeeDocumentsViewModel()
                            {
                                FilePath = EmpDocumentDetails.FilePath,
                                DocumentID = EmpDocumentDetails.DocumentId,
                                Comments = EmpDocumentDetails.Comments,
                                UploadType = uploadTypeText.ToString(),
                                FileName = EmpDocumentDetails.FileName,
                                EmployeeId = employeeId
                            };
                        }
                        model.Add(objEmpDocDetails);
                    }
                }
                return (model).Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// method to retrive HRDocDetailsHistroy for display in gridview Details
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns></returns>
        public List<Tbl_Employee_DocumentDetail> GetEmpDocumentHistoryForDisplay(int documentID)
        {
            try
            {
                var documentHistory = (from document in dbContext.Tbl_Employee_Documents
                                       join documentDetails in dbContext.Tbl_Employee_DocumentDetail
                                       on document.DocumentId equals documentDetails.DocumentId
                                       where document.DocumentId == documentID
                                       orderby documentDetails.DocumentDetailId descending
                                       select documentDetails).ToList();
                return documentHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method will delete employeeDoc entire Histroy from gridview delete
        /// </summary>
        /// by vijay
        /// <param name="documentId"></param>
        /// <returns></returns>
        public bool DeleteEmployeeUploadDetails(int documentId)
        {
            bool isDeleted = false;
            try
            {
                List<Tbl_Employee_DocumentDetail> docDetails = dbContext.Tbl_Employee_DocumentDetail.Where(d => d.DocumentId == documentId).ToList();
                Tbl_Employee_Documents doc = dbContext.Tbl_Employee_Documents.Where(d => d.DocumentId == documentId).FirstOrDefault();

                if (docDetails != null)
                {
                    foreach (Tbl_Employee_DocumentDetail eachdocDetail in docDetails)
                    {
                        dbContext.Tbl_Employee_DocumentDetail.DeleteObject(eachdocDetail);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }

                dbContext.Tbl_Employee_Documents.DeleteObject(doc);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDeleted;
        }

        /// <summary>
        ///  Method will delete HRDoc entire Histroy from gridview delete
        /// </summary>
        /// by vijay
        /// <param name="documentId"></param>
        /// <returns></returns>
        public bool DeleteHRUploadDetails(int documentId)
        {
            bool isDeleted = false;
            try
            {
                List<Tbl_HR_DocumentDetail> docDetails = dbContext.Tbl_HR_DocumentDetail.Where(d => d.DocumentId == documentId).ToList();
                Tbl_HR_Documents doc = dbContext.Tbl_HR_Documents.Where(d => d.DocumentId == documentId).FirstOrDefault();

                if (docDetails != null)
                {
                    foreach (Tbl_HR_DocumentDetail eachdocDetail in docDetails)
                    {
                        dbContext.Tbl_HR_DocumentDetail.DeleteObject(eachdocDetail);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }

                dbContext.Tbl_HR_Documents.DeleteObject(doc);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDeleted;
        }

        public bool DeleteRMGUploadDetails(int documentId)
        {
            bool isDeleted = false;
            try
            {
                List<Tbl_RMG_DocumentDetail> docDetails = dbContext.Tbl_RMG_DocumentDetail.Where(d => d.DocumentId == documentId).ToList();
                Tbl_RMG_Documents doc = dbContext.Tbl_RMG_Documents.Where(d => d.DocumentId == documentId).FirstOrDefault();

                if (docDetails != null)
                {
                    foreach (Tbl_RMG_DocumentDetail eachdocDetail in docDetails)
                    {
                        dbContext.Tbl_RMG_DocumentDetail.DeleteObject(eachdocDetail);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }

                dbContext.Tbl_RMG_Documents.DeleteObject(doc);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDeleted;
        }

        /// <summary>
        /// Method to retrive File Description from DocumentId
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns></returns>
        public string GetFileDescriptionFromDocId(int documentID)
        {
            try
            {
                string GetFileDescription = (from fdesc in dbContext.Tbl_HR_Documents where fdesc.DocumentId == documentID select fdesc.FileDescription).FirstOrDefault();
                return GetFileDescription;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to retrive Emp File Description from DocumentId
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns></returns>
        public string GetEmpFileDescriptionFromDocId(int documentID)
        {
            try
            {
                string GetFileDescription = (from fdesc in dbContext.Tbl_Employee_Documents where fdesc.DocumentId == documentID select fdesc.FileDescription).FirstOrDefault();
                return GetFileDescription;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetEmpIdFromDocId(int documentID)
        {
            try
            {
                var empIdforFolderIdentification = (from document in dbContext.Tbl_Employee_Documents
                                                    join documentDetails in dbContext.Tbl_Employee_DocumentDetail
                                                    on document.DocumentId equals documentDetails.DocumentId
                                                    where documentDetails.DocumentId == documentID
                                                    select document.EmployeeId).FirstOrDefault();

                return empIdforFolderIdentification;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to find out name of employee who uploaded the file
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns></returns>
        public string GetUploadNameFromUploadById(string uploadByEmpId)
        {
            string UploadbyName = string.Empty;
            try
            {
                UploadbyName = (from MName in dbContext.HRMS_tbl_PM_Employee
                                where MName.EmployeeCode == uploadByEmpId
                                select MName.EmployeeName).FirstOrDefault();
                return (UploadbyName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetUploadUserId(string uploadByEmpName)
        {
            try
            {
                string employeeID = (from Empid in dbContext.HRMS_tbl_PM_Employee
                                     where Empid.EmployeeName == uploadByEmpName
                                     select Empid.EmployeeCode).FirstOrDefault();
                return employeeID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool DeleteHRDocsSelected(string filename)
        {
            bool isDeleted = false;
            try
            {
                var documentformchild = (from document in dbContext.Tbl_HR_Documents
                                         join documentDetails in dbContext.Tbl_HR_DocumentDetail
                                         on document.DocumentId equals documentDetails.DocumentId
                                         where documentDetails.FileName == filename
                                         select documentDetails).FirstOrDefault();

                if (documentformchild != null)
                {
                    dbContext.Tbl_HR_DocumentDetail.DeleteObject(documentformchild);
                    dbContext.SaveChanges(); isDeleted = true;
                }

                return isDeleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteRMGDocsSelected(string filename)
        {
            bool isDeleted = false;
            try
            {
                var documentformchild = (from document in dbContext.Tbl_RMG_Documents
                                         join documentDetails in dbContext.Tbl_RMG_DocumentDetail
                                         on document.DocumentId equals documentDetails.DocumentId
                                         where documentDetails.FileName == filename
                                         select documentDetails).FirstOrDefault();

                if (documentformchild != null)
                {
                    dbContext.Tbl_RMG_DocumentDetail.DeleteObject(documentformchild);
                    dbContext.SaveChanges(); isDeleted = true;
                }

                return isDeleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool DeleteEmpDocsSelected(string filename)
        {
            bool isDeleted = false;
            try
            {
                var documentformchild = (from document in dbContext.Tbl_Employee_Documents
                                         join documentDetails in dbContext.Tbl_Employee_DocumentDetail
                                         on document.DocumentId equals documentDetails.DocumentId
                                         where documentDetails.FileName == filename
                                         select documentDetails).FirstOrDefault();

                if (documentformchild != null)
                {
                    dbContext.Tbl_Employee_DocumentDetail.DeleteObject(documentformchild);
                    dbContext.SaveChanges(); isDeleted = true;
                }

                return isDeleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to retrive HRDocDetailsHistroy for display in gridview Details
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns></returns>
        public List<Tbl_HR_DocumentDetail> GetHRDocumentHistoryForDisplay(int documentID)
        {
            try
            {
                var documentHistory = (from document in dbContext.Tbl_HR_Documents
                                       join documentDetails in dbContext.Tbl_HR_DocumentDetail
                                       on document.DocumentId equals documentDetails.DocumentId
                                       where document.DocumentId == documentID
                                       orderby documentDetails.DocumentDetailId descending
                                       select documentDetails).ToList();
                return documentHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Tbl_RMG_DocumentDetail> GetRMGDocumentHistoryForDisplay(int documentID)
        {
            try
            {
                var documentHistory = (from document in dbContext.Tbl_RMG_Documents
                                       join documentDetails in dbContext.Tbl_RMG_DocumentDetail
                                       on document.DocumentId equals documentDetails.DocumentId
                                       where document.DocumentId == documentID
                                       orderby documentDetails.DocumentDetailId descending
                                       select documentDetails).ToList();
                return documentHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To retrieve HR document history
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns></returns>
        public IQueryable<Tbl_HR_DocumentDetail> GetHRDocumentHistory(int documentID)
        {
            try
            {
                var documentHistory = (from document in dbContext.Tbl_HR_Documents
                                       join documentDetails in dbContext.Tbl_HR_DocumentDetail
                                       on document.DocumentId equals documentDetails.DocumentId
                                       where document.DocumentId == documentID
                                       select documentDetails);
                return documentHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To retrieve Employee document history
        /// </summary>
        /// <param name="documentID"></param>
        /// <returns></returns>
        public IQueryable<Tbl_Employee_DocumentDetail> GetEmployeeDocumentHistory(int documentID)
        {
            try
            {
                var documentHistory = (from document in dbContext.Tbl_Employee_Documents
                                       join documentDetails in dbContext.Tbl_Employee_DocumentDetail
                                       on document.DocumentId equals documentDetails.DocumentId
                                       where document.DocumentId == documentID
                                       select documentDetails);
                return documentHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Check if the hr document is already exists
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="uploadTypeID"></param>
        /// <returns></returns>
        public bool IsHRDocumentExists(string documentName, int uploadTypeID)
        {
            var document = dbContext.Tbl_HR_Documents.Where(doc => doc.FileName == documentName && doc.UploadTypeId == uploadTypeID).FirstOrDefault();
            if (document == null)
                return false;

            return true;
        }

        public bool IsRMGDocumentExists(string documentName, int uploadTypeID)
        {
            var document = dbContext.Tbl_RMG_Documents.Where(doc => doc.FileName == documentName && doc.UploadTypeId == 1).FirstOrDefault();
            if (document == null)
                return false;

            return true;
        }

        /// <summary>
        /// Check if the employee document is already exists
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="uploadTypeID"></param>
        /// <returns></returns>
        public bool IsEmployeeDocumentExists(string documentName, int employeeId, int uploadTypeID)
        {
            var document = dbContext.Tbl_Employee_Documents.Where(doc => doc.FileName == documentName && doc.UploadTypeId == uploadTypeID && doc.EmployeeId == employeeId).FirstOrDefault();
            if (document == null)
                return false;

            return true;
        }

        /// <summary>
        /// Generate the new name for hr document
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="uploadTypeID"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public string GetNewNameForHRDocument(string documentName, int uploadTypeID, out int documentId)  //ab_c_d.xls
        {
            documentId = 0;
            var document = dbContext.Tbl_HR_Documents.Where(doc => doc.FileName == documentName && doc.UploadTypeId == uploadTypeID).FirstOrDefault();

            if (document == null)
                return string.Empty;

            documentId = document.DocumentId;
            var latestDocument = dbContext.Tbl_HR_DocumentDetail.Where(dd => dd.DocumentId == document.DocumentId).OrderByDescending(dd => dd.UploadedDate).FirstOrDefault();
            if (latestDocument == null) // If no subversion found in child table
                return Path.GetFileNameWithoutExtension(documentName) + "_1" + Path.GetExtension(documentName);

            string latestDocumentFileName = System.IO.Path.GetFileNameWithoutExtension(latestDocument.FileName);    //ab_c_d_23.xls
            int maxNumberAlloted = Convert.ToInt32(latestDocumentFileName.Substring(latestDocumentFileName.LastIndexOf('_') + 1));  //23

            //Path.GetFileNameWithoutExtension(documentName) = ab_c_d     //++maxNumberAlloted = 24     //Path.GetExtension(documentName) = .xls
            return Path.GetFileNameWithoutExtension(documentName) + "_" + (++maxNumberAlloted) + Path.GetExtension(documentName);
        }

        public string GetNewNameForRMGDocument(string documentName, int uploadTypeID, out int documentId)  //ab_c_d.xls
        {
            documentId = 0;
            var document = dbContext.Tbl_RMG_Documents.Where(doc => doc.FileName == documentName && doc.UploadTypeId == 1).FirstOrDefault();

            if (document == null)
                return string.Empty;

            documentId = document.DocumentId;
            var latestDocument = dbContext.Tbl_RMG_DocumentDetail.Where(dd => dd.DocumentId == document.DocumentId).OrderByDescending(dd => dd.UploadedDate).FirstOrDefault();
            if (latestDocument == null) // If no subversion found in child table
                return Path.GetFileNameWithoutExtension(documentName) + "_1" + Path.GetExtension(documentName);

            string latestDocumentFileName = System.IO.Path.GetFileNameWithoutExtension(latestDocument.FileName);    //ab_c_d_23.xls
            int maxNumberAlloted = Convert.ToInt32(latestDocumentFileName.Substring(latestDocumentFileName.LastIndexOf('_') + 1));  //23

            //Path.GetFileNameWithoutExtension(documentName) = ab_c_d     //++maxNumberAlloted = 24     //Path.GetExtension(documentName) = .xls
            return Path.GetFileNameWithoutExtension(documentName) + "_" + (++maxNumberAlloted) + Path.GetExtension(documentName);
        }

        /// <summary>
        /// Generate the new name for employee document
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="uploadTypeID"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public string GetNewNameForEmployeeDocument(string documentName, int uploadTypeID, int employeeId, out int documentId)  //ab_c_d.xls
        {
            documentId = 0;
            var document = dbContext.Tbl_Employee_Documents.Where(doc => doc.FileName == documentName && doc.UploadTypeId == uploadTypeID && doc.EmployeeId == employeeId).FirstOrDefault();

            if (document == null)
                return string.Empty;

            documentId = document.DocumentId;
            var latestDocument = dbContext.Tbl_Employee_DocumentDetail.Where(dd => dd.DocumentId == document.DocumentId).OrderByDescending(dd => dd.UploadedDate).FirstOrDefault();
            if (latestDocument == null) // If no subversion found in child table
                return Path.GetFileNameWithoutExtension(documentName) + "_1" + Path.GetExtension(documentName);

            string latestDocumentFileName = System.IO.Path.GetFileNameWithoutExtension(latestDocument.FileName);    //ab_c_d_23.xls
            int maxNumberAlloted = Convert.ToInt32(latestDocumentFileName.Substring(latestDocumentFileName.LastIndexOf('_') + 1));  //23

            //Path.GetFileNameWithoutExtension(documentName) = ab_c_d     //++maxNumberAlloted = 24     //Path.GetExtension(documentName) = .xls
            return Path.GetFileNameWithoutExtension(documentName) + "_" + (++maxNumberAlloted) + Path.GetExtension(documentName);
        }

        /// <summary>
        /// upload hr document
        /// </summary>
        /// <param name="hRDocument"></param>
        /// <returns></returns>
        public bool UploadHRDocument(IDocuments hRDocument)
        {
            try
            {
                if (hRDocument.GetType() == typeof(Tbl_HR_Documents))
                {
                    dbContext.Tbl_HR_Documents.AddObject(hRDocument as Tbl_HR_Documents);
                    dbContext.SaveChanges();
                }
                else if (hRDocument.GetType() == typeof(Tbl_HR_DocumentDetail))
                {
                    dbContext.Tbl_HR_DocumentDetail.AddObject(hRDocument as Tbl_HR_DocumentDetail);
                    dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UploadRMGDocument(IDocuments hRDocument)
        {
            try
            {
                if (hRDocument.GetType() == typeof(Tbl_RMG_Documents))
                {
                    dbContext.Tbl_RMG_Documents.AddObject(hRDocument as Tbl_RMG_Documents);
                    dbContext.SaveChanges();
                }
                else if (hRDocument.GetType() == typeof(Tbl_RMG_DocumentDetail))
                {
                    dbContext.Tbl_RMG_DocumentDetail.AddObject(hRDocument as Tbl_RMG_DocumentDetail);
                    dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// upload employee document
        /// </summary>
        /// <param name="EmpDocument"></param>
        /// <returns></returns>
        public bool UploadEmployeeDocument(IDocuments EmpDocument)
        {
            try
            {
                if (EmpDocument.GetType() == typeof(Tbl_Employee_Documents))
                {
                    dbContext.Tbl_Employee_Documents.AddObject(EmpDocument as Tbl_Employee_Documents);
                    dbContext.SaveChanges();
                }
                else if (EmpDocument.GetType() == typeof(Tbl_Employee_DocumentDetail))
                {
                    dbContext.Tbl_Employee_DocumentDetail.AddObject(EmpDocument as Tbl_Employee_DocumentDetail);
                    dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get hr upload types
        /// </summary>
        /// <returns></returns>
        public IQueryable<Tbl_HR_UploadType> GetHRUploadTypes()
        {
            return dbContext.Tbl_HR_UploadType.OrderBy(x => x.Description);
        }

        /// <summary>
        /// Get employee upload types
        /// </summary>
        /// <returns></returns>
        public IQueryable<Tbl_Employee_UploadType> GetEmployeeUploadTypes()
        {
            return dbContext.Tbl_Employee_UploadType.OrderBy(x => x.Description);
        }

        public void GetExcelData(System.Data.DataSet ds)
        {
            HRMS_tbl_PM_Employee employeeTable = new HRMS_tbl_PM_Employee();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["Employee Code"].ToString() != "")
                {
                    string xlsemployeeode = Convert.ToInt32(ds.Tables[0].Rows[i]["Employee Code"]).ToString();

                    if (xlsemployeeode != null)
                    {
                        var code = (from c in dbContext.HRMS_tbl_PM_Employee
                                    where c.EmployeeCode == xlsemployeeode
                                    select c.EmployeeCode).FirstOrDefault();

                        if (xlsemployeeode == code)
                        {
                            string xlsReportingManagerCode = ds.Tables[0].Rows[i]["Reporting Manager Code"].ToString();

                            var ReportingManagerCode_Empid_Value = (from c in dbContext.HRMS_tbl_PM_Employee
                                                                    where c.EmployeeCode == xlsReportingManagerCode
                                                                    select c.EmployeeID).FirstOrDefault();

                            string xlsCompetencyManagerCode = ds.Tables[0].Rows[i]["Competency Manager Code"].ToString();

                            var CompetencyManagerCode_Empid_Value = (from c in dbContext.HRMS_tbl_PM_Employee
                                                                     where c.EmployeeCode.Equals(xlsCompetencyManagerCode)
                                                                     select c.EmployeeID).FirstOrDefault();

                            string xlsConfirmationExitProcessManagerCode = ds.Tables[0].Rows[i]["Confirmation / Exit Process Manager Code"].ToString();

                            var ConfirmationExitProcessManagerCode_Empid_Value = (from c in dbContext.HRMS_tbl_PM_Employee
                                                                                  where c.EmployeeCode.Equals(xlsConfirmationExitProcessManagerCode)
                                                                                  select c.EmployeeID).FirstOrDefault();

                            var BusinessGroupID_Value = (from c in dbContext.HRMS_tbl_PM_Employee
                                                         where c.EmployeeCode == xlsemployeeode
                                                         select c.BusinessGroupID).FirstOrDefault();

                            var xlsBusinessGroupID = ds.Tables[0].Rows[i]["BusinessGroupID"].ToString();

                            if (xlsBusinessGroupID != "")
                            {
                                if (Convert.ToInt32(xlsBusinessGroupID) != BusinessGroupID_Value)
                                {
                                    BusinessGroupID_Value = Convert.ToInt32(xlsBusinessGroupID);
                                }
                            }

                            var LocationID_Value = (from c in dbContext.HRMS_tbl_PM_Employee
                                                    where c.EmployeeCode == xlsemployeeode
                                                    select c.LocationID).FirstOrDefault();

                            var xlsLocationid = ds.Tables[0].Rows[i]["Locationid"].ToString();

                            if (xlsLocationid != "")
                            {
                                if (Convert.ToInt32(xlsLocationid) != LocationID_Value)
                                {
                                    LocationID_Value = Convert.ToInt32(xlsLocationid);
                                }
                            }

                            var Parent_DU_ID_Value = (from c in dbContext.HRMS_tbl_PM_Employee
                                                      where c.EmployeeCode == xlsemployeeode
                                                      select c.ResourcePoolID).FirstOrDefault();

                            var xlsParent_DU_ID = ds.Tables[0].Rows[i]["ParentDUID"].ToString();

                            if (xlsParent_DU_ID != "")
                            {
                                if (Convert.ToInt32(xlsParent_DU_ID) != Parent_DU_ID_Value)
                                {
                                    Parent_DU_ID_Value = Convert.ToInt32(xlsParent_DU_ID);
                                }
                            }

                            var Current_DU_ID_Value = (from c in dbContext.HRMS_tbl_PM_Employee
                                                       where c.EmployeeCode == xlsemployeeode
                                                       select c.Current_DU).FirstOrDefault();

                            int? xlsCurrentDUID = Convert.ToInt32(ds.Tables[0].Rows[i]["CurrentDUID"]);

                            if (xlsCurrentDUID != null)
                            {
                                if (xlsCurrentDUID != Current_DU_ID_Value)
                                {
                                    Current_DU_ID_Value = xlsCurrentDUID;
                                }
                            }

                            var DT_ID_Value = (from c in dbContext.HRMS_tbl_PM_Employee
                                               where c.EmployeeCode == xlsemployeeode
                                               select c.GroupID).FirstOrDefault();

                            var xlsDT_ID = ds.Tables[0].Rows[i]["DT ID"].ToString();

                            if (xlsDT_ID != "")
                            {
                                if (Convert.ToInt32(xlsDT_ID) != DT_ID_Value)
                                {
                                    DT_ID_Value = Convert.ToInt32(xlsDT_ID);
                                }
                            }

                            var EmpID = (from c in dbContext.HRMS_tbl_PM_Employee
                                         where c.EmployeeCode == xlsemployeeode
                                         select c.EmployeeID).FirstOrDefault();

                            var Resource_Pool_ID_Value = (from c in dbContext.tbl_PM_ResourcePoolDetail
                                                          where c.EmployeeID == EmpID
                                                          select c.ResourcePoolID).FirstOrDefault();

                            var xlsResource_Pool_ID = ds.Tables[0].Rows[i]["Resource Pool ID"].ToString();

                            if (xlsResource_Pool_ID != "")
                            {
                                if (Convert.ToInt32(xlsResource_Pool_ID) != Resource_Pool_ID_Value)
                                {
                                    Resource_Pool_ID_Value = Convert.ToInt32(xlsResource_Pool_ID);
                                }
                            }

                            var Tool_ID_Value = (from c in dbContext.tbl_PM_EmployeeSkillMatrix
                                                 where c.EmployeeID == EmpID
                                                 select c.ToolID).FirstOrDefault();

                            var xlsTool_ID = ds.Tables[0].Rows[i]["ToolId"].ToString();

                            if (xlsTool_ID != "")
                            {
                                if (Convert.ToInt32(xlsTool_ID) != Tool_ID_Value)
                                {
                                    Tool_ID_Value = Convert.ToInt32(xlsTool_ID);
                                }
                            }

                            var Proficiency_ID_Value = (from c in dbContext.tbl_PM_EmployeeSkillMatrix
                                                        where c.EmployeeID == EmpID
                                                        select c.Proficiency).FirstOrDefault();

                            var xlsProficiency_ID = ds.Tables[0].Rows[i]["Proficiency ID"].ToString();

                            if (xlsProficiency_ID != "")
                            {
                                if (Convert.ToInt32(xlsProficiency_ID) != Proficiency_ID_Value)
                                {
                                    Proficiency_ID_Value = Convert.ToInt32(xlsProficiency_ID);
                                }
                            }

                            var EntireRow = (from c in dbContext.HRMS_tbl_PM_Employee
                                             where c.EmployeeCode == xlsemployeeode
                                             select c);

                            foreach (var record in EntireRow)
                            {
                                if (xlsLocationid != "")
                                {
                                    record.LocationID = LocationID_Value;
                                }
                                if (xlsBusinessGroupID != "")
                                {
                                    record.BusinessGroupID = BusinessGroupID_Value;
                                }
                                if (xlsCurrentDUID != null)
                                {
                                    record.Current_DU = Current_DU_ID_Value;
                                }
                                if (xlsParent_DU_ID != "")
                                {
                                    record.ResourcePoolID = Parent_DU_ID_Value;
                                }
                                if (xlsDT_ID != "")
                                {
                                    record.GroupID = DT_ID_Value;
                                }
                                if (ReportingManagerCode_Empid_Value != 0)
                                {
                                    record.CostCenterID = ReportingManagerCode_Empid_Value;
                                }
                                if (CompetencyManagerCode_Empid_Value != 0)
                                {
                                    record.CompetencyManager = CompetencyManagerCode_Empid_Value;
                                }
                                if (ConfirmationExitProcessManagerCode_Empid_Value != 0)
                                {
                                    record.ReportingTo = ConfirmationExitProcessManagerCode_Empid_Value;
                                }
                            }

                            var EntireRow1 = (from c in dbContext.tbl_PM_ResourcePoolDetail
                                              where c.EmployeeID == EmpID
                                              select c);
                            foreach (var record1 in EntireRow1)
                            {
                                if (xlsResource_Pool_ID != "")
                                {
                                    record1.ResourcePoolID = Resource_Pool_ID_Value;
                                }
                            }

                            var EntireRow2 = (from c in dbContext.tbl_PM_EmployeeSkillMatrix
                                              where c.EmployeeID == EmpID
                                              select c);

                            //foreach (var record2 in EntireRow2)
                            //{
                            if (EntireRow2.Count() == 0)
                            {
                                tbl_PM_EmployeeSkillMatrix newRow = new tbl_PM_EmployeeSkillMatrix();
                                newRow.EmployeeID = EmpID;
                                if (xlsTool_ID != "")
                                {
                                    newRow.ToolID = Tool_ID_Value;
                                }
                                if (xlsProficiency_ID != "")
                                {
                                    newRow.Proficiency = Proficiency_ID_Value;
                                }
                                if (xlsProficiency_ID != "")
                                {
                                    newRow.EmployeeskillLevel = Proficiency_ID_Value.HasValue ? Proficiency_ID_Value.Value : 0;
                                }

                                dbContext.tbl_PM_EmployeeSkillMatrix.AddObject(newRow);
                                // dbContext.SaveChanges();
                            }
                            else
                            {
                                var isUpdated = false;
                                foreach (var item in EntireRow2)
                                {
                                    if (item.EmployeeID == EmpID && item.ToolID == Tool_ID_Value)
                                    {
                                        item.EmployeeID = EmpID;
                                        if (xlsTool_ID != "")
                                        {
                                            item.ToolID = Tool_ID_Value;
                                        }
                                        if (xlsProficiency_ID != "")
                                        {
                                            item.Proficiency = Proficiency_ID_Value;
                                            item.EmployeeskillLevel = Proficiency_ID_Value.HasValue ? Proficiency_ID_Value.Value : 0;
                                        }
                                        isUpdated = true;
                                    }
                                }
                                if (isUpdated == false)
                                {
                                    tbl_PM_EmployeeSkillMatrix record2 = new tbl_PM_EmployeeSkillMatrix();
                                    record2.EmployeeID = EmpID;
                                    if (xlsTool_ID != "")
                                    {
                                        record2.ToolID = Tool_ID_Value;
                                    }
                                    if (xlsProficiency_ID != "")
                                    {
                                        record2.Proficiency = Proficiency_ID_Value;
                                        record2.EmployeeskillLevel = Proficiency_ID_Value.HasValue ? Proficiency_ID_Value.Value : 0;
                                    }

                                    dbContext.tbl_PM_EmployeeSkillMatrix.AddObject(record2);
                                }
                            }
                            //}

                            dbContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}