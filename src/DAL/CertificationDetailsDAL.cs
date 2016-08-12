using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRMS.DAL
{
    public class CertificationDetailsDAL
    {
        private HRMSDBEntities dbContext = new HRMSDBEntities();
        private QualificationDetailsDAL objQual = new QualificationDetailsDAL();

        public List<HRMS_tbl_PM_Certifications> LoadCertificationDDL()
        {
            List<HRMS_tbl_PM_Certifications> certList = (from certificationDrp in dbContext.HRMS_tbl_PM_Certifications
                                                         orderby certificationDrp.CertificationName
                                                         select certificationDrp).ToList();

            return certList;
        }

        //GetCertificationDetails
        public List<CertificationDetails> GetEmployeeCertificationDetails(int page, int rows, int EmployeeID, out int totalCount)
        {
            List<CertificationDetails> finalObj = new List<CertificationDetails>();

            try
            {
                var AllDetails = (from certificate in dbContext.tbl_PM_EmployeeCertificationMatrix
                                  where certificate.EmployeeID == EmployeeID
                                  orderby certificate.CertificationDate descending
                                  select certificate).ToList();

                if (AllDetails != null)
                {
                    foreach (var obj in AllDetails)
                    {
                        var histroy = (from his in dbContext.tbl_PM_EmployeeCertificationMatrixHistory
                                       where his.EmployeeCertificationID == obj.EmployeeCertificationID
                                       orderby his.EmployeeCertificationHistoryId descending
                                       select his).FirstOrDefault();

                        if (obj != null)
                        {
                            CertificationDetails current = new CertificationDetails();

                            current.EmployeeCertificationID = obj.EmployeeCertificationID;
                            current.CertificationID = obj.CertificationID;
                            current.EmployeeID = obj.EmployeeID;
                            if (obj.CertificationID != null)
                            {
                                current.CertificationName = obj.HRMS_tbl_PM_Certifications.CertificationName;
                                current.CertificationNameID = obj.HRMS_tbl_PM_Certifications.CertificationID;
                            }
                            current.CertificationNo = obj.CertificationNo;
                            current.CertificationDate = obj.CertificationDate;
                            current.CertificationGrade = obj.Grade;
                            current.CertificationScore = obj.TotalScore;
                            current.Institution = obj.InstituteName;
                            if (histroy != null)
                            {
                                current.Status = objQual.GetStatusMessage(histroy.ActionType, histroy.Status, false);
                                if (histroy.Status == 2 || histroy.Status == 3)
                                {
                                    if ((DateTime.Now - Convert.ToDateTime(histroy.ModifiedDate)).TotalHours < 72)
                                    {
                                        //show message
                                        current.ApprovalStatusFlag = "1";
                                    }
                                    else
                                    {
                                        //hide message
                                        current.ApprovalStatusFlag = "0";
                                    }
                                }
                                current.ActionType = histroy.ActionType;
                            }
                            finalObj.Add(current);
                        }
                    }
                    finalObj.Skip((page - 1) * rows).Take(rows).ToList();
                }

                totalCount = dbContext.tbl_PM_EmployeeCertificationMatrix.Where(x => x.EmployeeID == EmployeeID).Count();

                return finalObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanSendMail(int EmployeeID)
        {
            List<CertificationDetails> finalObj = new List<CertificationDetails>();

            try
            {
                var AllDetails = (from certificate in dbContext.tbl_PM_EmployeeCertificationMatrix
                                  where certificate.EmployeeID == EmployeeID
                                  orderby certificate.CertificationDate descending
                                  select certificate).ToList();

                if (AllDetails != null)
                {
                    foreach (var obj in AllDetails)
                    {
                        var histroy = (from his in dbContext.tbl_PM_EmployeeCertificationMatrixHistory
                                       where his.EmployeeCertificationID == obj.EmployeeCertificationID
                                       orderby his.EmployeeCertificationHistoryId descending
                                       select his).FirstOrDefault();

                        if (obj != null)
                        {
                            if (histroy != null)
                                if (histroy.Status != 2 && histroy.Status != 3)
                                {
                                    CertificationDetails current = new CertificationDetails();
                                    current.CanSendMail = Convert.ToString(histroy.SendMail);
                                    finalObj.Add(current);
                                }
                        }
                    }
                }
                if (finalObj.Any(x => x.CanSendMail == "True"))
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool MailSent(int EmployeeID)
        {
            List<CertificationDetails> finalObj = new List<CertificationDetails>();
            try
            {
                var AllDetails = (from certificate in dbContext.tbl_PM_EmployeeCertificationMatrix
                                  where certificate.EmployeeID == EmployeeID
                                  orderby certificate.CertificationDate descending
                                  select certificate).ToList();

                if (AllDetails != null)
                {
                    foreach (var obj in AllDetails)
                    {
                        var histroy = (from his in dbContext.tbl_PM_EmployeeCertificationMatrixHistory
                                       where his.EmployeeCertificationID == obj.EmployeeCertificationID
                                       orderby his.EmployeeCertificationHistoryId descending
                                       select his).FirstOrDefault();

                        if (obj != null)
                        {
                            if (histroy != null)
                                if (histroy.Status != 2 && histroy.Status != 3)
                                {
                                    histroy.SendMail = false;
                                    dbContext.SaveChanges();
                                }
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveCertificationDetails(CertificationDetails model, bool IsLoggedInEmployee, int? SelectedCertificationID, int? EmployeeId)
        {
            bool isAdded = false;

            tbl_PM_EmployeeCertificationMatrix emp = dbContext.tbl_PM_EmployeeCertificationMatrix.Where(ed => ed.EmployeeCertificationID == model.EmployeeCertificationID).FirstOrDefault();
            if (emp == null || emp.EmployeeCertificationID <= 0)
            {
                tbl_PM_EmployeeCertificationMatrix certificationDetails = new tbl_PM_EmployeeCertificationMatrix();

                certificationDetails.EmployeeCertificationID = model.EmployeeCertificationID;
                certificationDetails.EmployeeID = EmployeeId;
                certificationDetails.CertificationID = SelectedCertificationID;
                if (model.CertificationNo != null && model.CertificationNo != "")
                    certificationDetails.CertificationNo = model.CertificationNo.Trim();
                else
                    certificationDetails.CertificationNo = model.CertificationNo;
                certificationDetails.CertificationDate = model.CertificationDate;
                if (model.Institution != null && model.Institution != "")
                    certificationDetails.InstituteName = model.Institution.Trim();
                else
                    certificationDetails.InstituteName = model.Institution;
                if (model.CertificationScore != null && model.CertificationScore != "")
                    certificationDetails.TotalScore = model.CertificationScore.Trim();
                else
                    certificationDetails.TotalScore = model.CertificationScore;
                if (model.CertificationGrade != null && model.CertificationGrade != "")
                    certificationDetails.Grade = model.CertificationGrade.Trim();
                else
                    certificationDetails.Grade = model.CertificationGrade;

                dbContext.tbl_PM_EmployeeCertificationMatrix.AddObject(certificationDetails);
                dbContext.SaveChanges();

                if (IsLoggedInEmployee == true)
                {
                    tbl_PM_EmployeeCertificationMatrixHistory certificationHistory = new tbl_PM_EmployeeCertificationMatrixHistory();
                    certificationHistory.EmployeeCertificationID = certificationDetails.EmployeeCertificationID;
                    certificationHistory.EmployeeID = EmployeeId;
                    certificationHistory.CertificationID = SelectedCertificationID;
                    if (model.CertificationNo != null && model.CertificationNo != "")
                        certificationHistory.CertificationNo = model.CertificationNo.Trim();
                    else
                        certificationHistory.CertificationNo = model.CertificationNo;
                    certificationHistory.CertificationDate = model.CertificationDate;
                    if (model.Institution != null && model.Institution != "")
                        certificationHistory.InstituteName = model.Institution.Trim();
                    else
                        certificationHistory.InstituteName = model.Institution;
                    if (model.CertificationScore != null && model.CertificationScore != "")
                        certificationHistory.TotalScore = model.CertificationScore.Trim();
                    else
                        certificationHistory.TotalScore = model.CertificationScore;
                    if (model.CertificationGrade != null && model.CertificationGrade != "")
                        certificationHistory.Grade = model.CertificationGrade.Trim();
                    else
                        certificationHistory.Grade = model.CertificationGrade;
                    certificationHistory.ActionType = "Add";
                    certificationHistory.CreatedBy = EmployeeId.ToString();
                    certificationHistory.CreatedDate = DateTime.Now;
                    certificationHistory.SendMail = true;

                    dbContext.tbl_PM_EmployeeCertificationMatrixHistory.AddObject(certificationHistory);
                    dbContext.SaveChanges();
                }
            }
            else
            {
                if (emp.EmployeeID != EmployeeId ||
                emp.CertificationID != SelectedCertificationID ||
                emp.CertificationNo != model.CertificationNo ||
                emp.CertificationDate != model.CertificationDate ||
                emp.Grade != model.CertificationGrade ||
                emp.InstituteName != model.Institution ||
                emp.TotalScore != model.CertificationScore)
                {
                    if (IsLoggedInEmployee == true)
                    {
                        tbl_PM_EmployeeCertificationMatrixHistory Certifications = new tbl_PM_EmployeeCertificationMatrixHistory();
                        Certifications.EmployeeCertificationID = model.EmployeeCertificationID;
                        Certifications.EmployeeID = emp.EmployeeID;
                        Certifications.CertificationID = emp.CertificationID;
                        if (emp.CertificationNo != null && emp.CertificationNo != "")
                            Certifications.CertificationNo = emp.CertificationNo.Trim();
                        else
                            Certifications.CertificationNo = emp.CertificationNo;
                        Certifications.CertificationDate = emp.CertificationDate;
                        if (emp.InstituteName != null && emp.InstituteName != "")
                            Certifications.InstituteName = emp.InstituteName.Trim();
                        else
                            Certifications.InstituteName = emp.InstituteName;
                        if (emp.TotalScore != null && emp.TotalScore != "")
                            Certifications.TotalScore = emp.TotalScore.Trim();
                        else
                            Certifications.TotalScore = emp.TotalScore;
                        if (emp.Grade != null && emp.Grade != "")
                            Certifications.Grade = emp.Grade.Trim();
                        else
                            Certifications.Grade = emp.Grade;
                        Certifications.ActionType = "Edit";
                        Certifications.ModifiedBy = EmployeeId.ToString();
                        Certifications.ModifiedDate = DateTime.Now;
                        Certifications.SendMail = true;

                        dbContext.tbl_PM_EmployeeCertificationMatrixHistory.AddObject(Certifications);
                        dbContext.SaveChanges();
                    }

                    emp.EmployeeID = EmployeeId;
                    emp.CertificationID = SelectedCertificationID;
                    if (model.CertificationNo != null && model.CertificationNo != "")
                        emp.CertificationNo = model.CertificationNo.Trim();
                    else
                        emp.CertificationNo = model.CertificationNo;
                    emp.CertificationDate = model.CertificationDate;
                    if (model.CertificationGrade != null && model.CertificationGrade != "")
                        emp.Grade = model.CertificationGrade.Trim();
                    else
                        emp.Grade = model.CertificationGrade;
                    if (model.Institution != null && model.Institution != "")
                        emp.InstituteName = model.Institution.Trim();
                    else
                        emp.InstituteName = model.Institution;
                    if (model.CertificationScore != null && model.CertificationScore != "")
                        emp.TotalScore = model.CertificationScore.Trim();
                    else
                        emp.TotalScore = model.CertificationScore;
                }
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool DeleteCertificationDetails(int certId, int employeeId)
        {
            bool isDeleted = false;
            tbl_PM_EmployeeCertificationMatrix cert = dbContext.tbl_PM_EmployeeCertificationMatrix.Where(cd => cd.EmployeeCertificationID == certId && cd.EmployeeID == employeeId).FirstOrDefault();
            if (cert != null && cert.EmployeeCertificationID > 0)
            {
                dbContext.DeleteObject(cert);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        //GetCertificationDetails
        public List<CertificationDetails> GetEmployeeCertificationHistoryDetails(int page, int rows, int EmployeeID, out int totalCount)
        {
            var AllDetails = (from cert in dbContext.tbl_PM_EmployeeCertificationMatrixHistory
                              where cert.EmployeeID == EmployeeID
                              orderby cert.EmployeeCertificationID descending
                              select new CertificationDetails
                              {
                                  EmployeeCertificationID = cert.EmployeeCertificationID,
                                  CertificationID = cert.CertificationID,
                                  EmployeeID = cert.EmployeeID,
                                  CertificationName = "",
                                  CertificationNo = cert.CertificationNo,
                                  CertificationDate = cert.CertificationDate,
                                  CertificationGrade = cert.Grade,
                                  CertificationScore = cert.TotalScore,
                                  Institution = cert.InstituteName
                              }).Skip((page - 1) * rows).Take(rows).ToList();

            totalCount = this.dbContext.tbl_PM_EmployeeCertificationMatrixHistory.Count(x => x.EmployeeID == EmployeeID);

            return AllDetails;
        }

        public List<CertificationDetails> GetEmpCertificateDetailAndHistory(int page, int rows, int EmployeeID, out int totalCount, string module)
        {
            List<CertificationDetails> finalCertification = new List<CertificationDetails>();
            List<CertificationDetails> cerificationHistory = new List<CertificationDetails>();

            var certificationDetails = (from cert in dbContext.tbl_PM_EmployeeCertificationMatrix
                                        where cert.EmployeeID == EmployeeID
                                        orderby cert.EmployeeCertificationID descending
                                        select new CertificationDetails
                                        {
                                            Type = "",
                                            Value = "New",
                                            ApproveStatus = null,
                                            Comments = string.Empty,
                                            EmployeeCertificationID = cert.EmployeeCertificationID,
                                            EmployeeCertificationHistoryID = 0,

                                            CertificationID = cert.CertificationID,
                                            EmployeeID = cert.EmployeeID,
                                            CertificationName = cert.HRMS_tbl_PM_Certifications.CertificationName,
                                            CertificationNo = cert.CertificationNo,
                                            CertificationDate = cert.CertificationDate,
                                            CertificationGrade = cert.Grade,
                                            CertificationScore = cert.TotalScore,
                                            Institution = cert.InstituteName
                                        }).Skip((page - 1) * rows).Take(rows).ToList();

            if (module == "New Certification Details")
            {
                cerificationHistory = (from cert in dbContext.tbl_PM_EmployeeCertificationMatrixHistory
                                       where cert.EmployeeID == EmployeeID && (cert.Status == null)
                                       orderby cert.EmployeeCertificationHistoryId descending
                                       select new CertificationDetails
                                       {
                                           Type = cert.ActionType,
                                           Value = "Old",
                                           ApprovalStatusMasterID = cert.Status,
                                           Comments = cert.Comments,
                                           EmployeeCertificationID = cert.EmployeeCertificationID,
                                           EmployeeCertificationHistoryID = cert.EmployeeCertificationHistoryId,
                                           CertificationID = cert.CertificationID,
                                           EmployeeID = cert.EmployeeID,
                                           CertificationName = cert.HRMS_tbl_PM_Certifications.CertificationName,
                                           CertificationNo = cert.CertificationNo,
                                           CertificationDate = cert.CertificationDate,
                                           CertificationGrade = cert.Grade,
                                           CertificationScore = cert.TotalScore,
                                           Institution = cert.InstituteName,
                                       }).Skip((page - 1) * rows).Take(rows).ToList();
            }
            else
            {
                cerificationHistory = (from cert in dbContext.tbl_PM_EmployeeCertificationMatrixHistory
                                       where cert.EmployeeID == EmployeeID && (cert.Status == 1)
                                       orderby cert.EmployeeCertificationHistoryId descending
                                       select new CertificationDetails
                                       {
                                           Type = cert.ActionType,
                                           Value = "Old",
                                           ApprovalStatusMasterID = cert.Status,
                                           Comments = cert.Comments,
                                           EmployeeCertificationID = cert.EmployeeCertificationID,
                                           EmployeeCertificationHistoryID = cert.EmployeeCertificationHistoryId,
                                           CertificationID = cert.CertificationID,
                                           EmployeeID = cert.EmployeeID,
                                           CertificationName = cert.HRMS_tbl_PM_Certifications.CertificationName,
                                           CertificationNo = cert.CertificationNo,
                                           CertificationDate = cert.CertificationDate,
                                           CertificationGrade = cert.Grade,
                                           CertificationScore = cert.TotalScore,
                                           Institution = cert.InstituteName,
                                       }).Skip((page - 1) * rows).Take(rows).ToList();
            }

            // Remove unwanted records
            foreach (var empCertHistory in cerificationHistory)
            {
                foreach (var empCertDetail in certificationDetails)
                {
                    if (empCertDetail.EmployeeCertificationID == empCertHistory.EmployeeCertificationID)
                    {
                        if (empCertHistory.Type.Equals("Edit"))
                        {
                            empCertDetail.EmployeeCertificationHistoryID = empCertHistory.EmployeeCertificationHistoryID;
                            empCertDetail.Type = empCertHistory.Type;
                            empCertDetail.ApproveStatus = empCertHistory.ApproveStatus;
                            finalCertification.Add(empCertDetail);
                        }
                        else
                        {
                            empCertHistory.Value = "New";
                        }
                        finalCertification.Add(empCertHistory);
                    }
                }
            }

            totalCount = finalCertification.Count(x => x.EmployeeID == EmployeeID);
            return finalCertification;
        }

        public bool SaveCertificationMatrixHistory(List<CertificationDetails> model, string CertHrComment)
        {
            try
            {
                if (model.Any(x => x.EmployeeCertificationHistoryID == 0)) return false;

                var newData = model.Where(cer => cer.Value == "New").ToList();
                foreach (var data in newData)
                {
                    var certificateHistory = dbContext.tbl_PM_EmployeeCertificationMatrixHistory.FirstOrDefault(cmh => cmh.EmployeeID == data.EmployeeID && cmh.EmployeeCertificationHistoryId == data.EmployeeCertificationHistoryID);

                    certificateHistory.Comments = CertHrComment;
                    certificateHistory.Status = data.ApprovalStatusMasterID == 0 ? null : data.ApprovalStatusMasterID;
                    certificateHistory.ModifiedDate = DateTime.Now;

                    if (data.ApprovalStatusMasterID == 3)
                    {
                        var certificationMatrix = dbContext.tbl_PM_EmployeeCertificationMatrix.FirstOrDefault(empQuality => empQuality.EmployeeID == data.EmployeeID
                            && empQuality.EmployeeCertificationID == data.EmployeeCertificationID);

                        if (data.Type.Equals("Add"))
                        {
                            //dbContext.tbl_PM_EmployeeCertificationMatrix.DeleteObject(certificationMatrix);
                        }
                        else
                        {
                            certificationMatrix.CertificationID = certificateHistory.CertificationID;
                            certificationMatrix.CertificationNo = certificateHistory.CertificationNo;
                            certificationMatrix.InstituteName = certificateHistory.InstituteName;
                            certificationMatrix.CertificationDate = certificateHistory.CertificationDate;
                            certificationMatrix.TotalScore = certificateHistory.TotalScore;
                            certificationMatrix.Grade = certificateHistory.Grade;
                        }
                    }
                }

                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}