using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRMS.DAL
{
    public class QualificationDetailsDAL
    {
        private HRMSDBEntities dbContext = new HRMSDBEntities();

        public string GetStatusMessage(string actionType, int? status, bool IsSkillCall)
        {
            try
            {
                if (IsSkillCall != true)
                {
                    if (status == null || status == 0)
                    {
                        if (actionType == "Add")
                            return GridHRApprovalStatusMessages.GNoAction_Add0;
                        else
                            return GridHRApprovalStatusMessages.GNoAction_Edit0;
                    }
                    else
                    {
                        if (status == 1)
                            return GridHRApprovalStatusMessages.GOnHold_1;

                        if (status == 2)
                            return GridHRApprovalStatusMessages.GApproved_2;

                        if (status == 3)
                            return GridHRApprovalStatusMessages.GRejected_3;
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
                else
                {
                    if (status == null || status == 0)
                    {
                        if (actionType == "Add")
                            return GridRMGApprovalStatusMessages.GNoAction_Add_RMG0;
                        else
                            return GridRMGApprovalStatusMessages.GNoAction_Edit_RMG0;
                    }
                    else
                    {
                        if (status == 1)
                            return GridRMGApprovalStatusMessages.GOnHold_RMG_1;

                        if (status == 2)
                            return GridRMGApprovalStatusMessages.GApproved_RMG_2;

                        if (status == 3)
                            return GridRMGApprovalStatusMessages.GRejected_RMG_3;
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanSendMail(int EmployeeID)
        {
            List<EmployeeQualifications> finalObj = new List<EmployeeQualifications>();

            var AllDetails = (from qual in dbContext.tbl_PM_EmployeeQualificationMatrix
                              where qual.EmployeeID == EmployeeID
                              orderby qual.PassoutYear descending
                              select qual).ToList();

            if (AllDetails != null)
            {
                foreach (var obj in AllDetails)
                {
                    var histroy = (from his in dbContext.tbl_PM_EmployeeQualificationMatrix_History
                                   where his.EmployeeQualificationID == obj.EmployeeQualificationID
                                   orderby his.EmployeeQualificationHistoryId descending
                                   select his).FirstOrDefault();

                    if (obj != null)
                    {
                        if (histroy != null)
                            if (histroy.Status != 2 && histroy.Status != 3)
                            {
                                EmployeeQualifications current = new EmployeeQualifications();
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

        public bool MailSent(int employeeId)
        {
            List<EmployeeQualifications> finalObj = new List<EmployeeQualifications>();
            try
            {
                var AllDetails = (from qual in dbContext.tbl_PM_EmployeeQualificationMatrix
                                  where qual.EmployeeID == employeeId
                                  orderby qual.PassoutYear descending
                                  select qual).ToList();

                if (AllDetails != null)
                {
                    foreach (var obj in AllDetails)
                    {
                        var histroy = (from his in dbContext.tbl_PM_EmployeeQualificationMatrix_History
                                       where his.EmployeeQualificationID == obj.EmployeeQualificationID
                                       orderby his.EmployeeQualificationHistoryId descending
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

        public List<EmployeeQualifications> GetEmployeeQualificationsOtherDetails(int page, int rows, int EmployeeID, out int totalCount)
        {
            List<EmployeeQualifications> finalObj = new List<EmployeeQualifications>();

            try
            {
                var AllDetails = (from qual in dbContext.tbl_PM_EmployeeQualificationMatrix
                                  where qual.EmployeeID == EmployeeID
                                  orderby qual.PassoutYear descending
                                  select qual).ToList();

                if (AllDetails != null)
                {
                    foreach (var obj in AllDetails)
                    {
                        var histroy = (from his in dbContext.tbl_PM_EmployeeQualificationMatrix_History
                                       where his.EmployeeQualificationID == obj.EmployeeQualificationID
                                       orderby his.EmployeeQualificationHistoryId descending
                                       select his).FirstOrDefault();

                        if (obj != null)
                        {
                            EmployeeQualifications current = new EmployeeQualifications();

                            current.EmployeeQualificationID = obj.EmployeeQualificationID;
                            current.EmployeeID = obj.EmployeeID;
                            current.Specialization = obj.Specialization;
                            current.Institute = obj.Institute;
                            current.University = obj.University;
                            //current.Course = obj.Courses;
                            current.Year = obj.PassoutYear;
                            current.Percentage = obj.Class;
                            if (obj.QualificationGroupID != null)
                                current.Degree = obj.HRMS_tbl_PM_QualificationGroupMaster.QualificationGroupName;
                            current.DegreeID = obj.QualificationGroupID;
                            if (obj.QualificationID != null)
                                current.Qualification = obj.HRMS_tbl_PM_Qualifications.QualificationName;
                            current.QualificationID = obj.QualificationID;
                            if (obj.QualificationTypeID != null)
                                current.Type = obj.tbl_PM_QualificationType.QualificationTypeName;
                            current.TypeID = obj.QualificationTypeID;

                            if (histroy != null)
                            {
                                current.ApprovalOrRejectionStatus = GetStatusMessage(histroy.ActionType, histroy.Status, false);
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
                }
                totalCount = dbContext.tbl_PM_EmployeeQualificationMatrix.Where(x => x.EmployeeID == EmployeeID).Count();

                return finalObj.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<HRMS_tbl_PM_Qualifications> GetEmployeeQualificationList()
        {
            List<HRMS_tbl_PM_Qualifications> empList = dbContext.HRMS_tbl_PM_Qualifications.ToList();
            return empList.OrderBy(x => x.QualificationName).ToList();
        }

        public List<HRMS_tbl_PM_QualificationGroupMaster> GetDegreeList()
        {
            var empList = dbContext.HRMS_tbl_PM_QualificationGroupMaster.ToList();
            return empList.OrderBy(x => x.QualificationGroupName).ToList();
        }

        public List<tbl_HR_Years> GetYearList()                                                        // Added Year Drop Down List
        {
            var empList = dbContext.tbl_HR_Years.ToList();
            return empList;
        }

        public List<tbl_PM_QualificationType> GetQualificationTypeList()
        {
            var empList = dbContext.tbl_PM_QualificationType.ToList();
            return empList.OrderBy(x => x.QualificationTypeName).ToList();
        }

        public bool DeleteEmployeeQualification(int qualId, int enployeeId)
        {
            bool isDeleted = false;
            tbl_PM_EmployeeQualificationMatrix qual = dbContext.tbl_PM_EmployeeQualificationMatrix.Where(qd => qd.EmployeeQualificationID == qualId && qd.EmployeeID == enployeeId).FirstOrDefault();
            if (qual != null && qual.EmployeeQualificationID > 0)
            {
                dbContext.DeleteObject(qual);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public bool SaveEmployeeQualification(EmployeeQualifications model, bool IsLoggedInEmployee, int? SelectedQualificationID, int? SelectedDegreeID, int? SelectedYearID, int? SelectedTypeID, int? EmployeeId)
        {
            bool isAdded = false;

            tbl_PM_EmployeeQualificationMatrix emp = dbContext.tbl_PM_EmployeeQualificationMatrix.Where(ed => ed.EmployeeQualificationID == model.EmployeeQualificationID).FirstOrDefault();

            if (emp == null || emp.EmployeeQualificationID <= 0)
            {
                tbl_PM_EmployeeQualificationMatrix employeeQualifications = new tbl_PM_EmployeeQualificationMatrix();
                employeeQualifications.EmployeeQualificationID = model.EmployeeQualificationID;
                employeeQualifications.EmployeeID = EmployeeId;
                employeeQualifications.QualificationID = SelectedQualificationID;
                employeeQualifications.QualificationGroupID = SelectedDegreeID;
                if (model.Specialization != null && model.Specialization != "")
                    employeeQualifications.Specialization = model.Specialization.Trim();
                else
                    employeeQualifications.Specialization = model.Specialization;
                if (model.Institute != null && model.Institute != "")
                    employeeQualifications.Institute = model.Institute.Trim();
                else
                    employeeQualifications.Institute = model.Institute;
                if (model.University != null && model.University != "")
                    employeeQualifications.University = model.University.Trim();
                else
                    employeeQualifications.University = model.University;
                //Courses = model.NewEmployeeQualification.Course;
                employeeQualifications.PassoutYear = SelectedYearID;
                employeeQualifications.QualificationTypeID = SelectedTypeID;
                if (model.Percentage != null && model.Percentage != "")
                    employeeQualifications.Class = model.Percentage.Trim();
                else
                    employeeQualifications.Class = model.Percentage;
                dbContext.tbl_PM_EmployeeQualificationMatrix.AddObject(employeeQualifications);
                dbContext.SaveChanges();

                if (IsLoggedInEmployee == true)
                {
                    tbl_PM_EmployeeQualificationMatrix_History Qualifications = new tbl_PM_EmployeeQualificationMatrix_History();

                    Qualifications.EmployeeQualificationID = employeeQualifications.EmployeeQualificationID;
                    Qualifications.EmployeeID = EmployeeId;
                    Qualifications.QualificationID = SelectedQualificationID;
                    Qualifications.QualificationGroupID = SelectedDegreeID;
                    if (model.Specialization != null && model.Specialization != "")
                        Qualifications.Specialization = model.Specialization.Trim();
                    else
                        Qualifications.Specialization = model.Specialization;
                    if (model.Institute != null && model.Institute != "")
                        Qualifications.Institute = model.Institute.Trim();
                    else
                        Qualifications.Institute = model.Institute;
                    if (model.University != null && model.University != "")
                        Qualifications.University = model.University.Trim();
                    else
                        Qualifications.University = model.University;
                    //Courses = model.NewEmployeeQualification.Course;
                    Qualifications.PassoutYear = SelectedYearID;
                    Qualifications.QualificationTypeID = SelectedTypeID;
                    if (model.Percentage != null && model.Percentage != "")
                        Qualifications.Class = model.Percentage.Trim();
                    else
                        Qualifications.Class = model.Percentage;
                    Qualifications.ActionType = "Add";
                    Qualifications.CreatedBy = EmployeeId.ToString();
                    Qualifications.CreatedDate = DateTime.Now;
                    Qualifications.SendMail = true;

                    dbContext.tbl_PM_EmployeeQualificationMatrix_History.AddObject(Qualifications);
                    dbContext.SaveChanges();
                }
            }
            //for edit
            else
            {
                if (
                emp.EmployeeID != EmployeeId ||
                    //emp.Courses != model.NewEmployeeQualification.Course ||
                emp.Institute != model.Institute ||
                emp.PassoutYear != SelectedYearID ||

                //column percentage from db was not allowing to edit nvarchar values properly so using class column to store percentage as well grades
                emp.Class != model.Percentage ||

                emp.Specialization != model.Specialization ||
                emp.QualificationTypeID != SelectedTypeID ||
                emp.QualificationID != SelectedQualificationID ||
                emp.QualificationGroupID != SelectedDegreeID ||
                emp.University != model.University
                )
                {
                    if (IsLoggedInEmployee == true)
                    {
                        tbl_PM_EmployeeQualificationMatrix_History Qualifications = new tbl_PM_EmployeeQualificationMatrix_History();
                        Qualifications.EmployeeQualificationID = model.EmployeeQualificationID;
                        Qualifications.EmployeeID = emp.EmployeeID;
                        Qualifications.QualificationID = emp.QualificationID;
                        Qualifications.QualificationGroupID = emp.QualificationGroupID;
                        if (emp.Specialization != null && emp.Specialization != "")
                            Qualifications.Specialization = emp.Specialization.Trim();
                        else
                            Qualifications.Specialization = emp.Specialization;
                        if (emp.Institute != null && emp.Institute != "")
                            Qualifications.Institute = emp.Institute.Trim();
                        else
                            Qualifications.Institute = emp.Institute;
                        if (emp.University != null && emp.University != "")
                            Qualifications.University = emp.University.Trim();
                        else
                            Qualifications.University = emp.University;
                        Qualifications.Courses = emp.Courses;
                        Qualifications.PassoutYear = emp.PassoutYear;
                        Qualifications.QualificationTypeID = emp.QualificationTypeID;
                        if (emp.Class != null && emp.Class != "")
                            Qualifications.Class = emp.Class.Trim();
                        else
                            Qualifications.Class = emp.Class;
                        Qualifications.ActionType = "Edit";
                        Qualifications.ModifiedBy = EmployeeId.ToString();
                        Qualifications.ModifiedDate = DateTime.Now;
                        Qualifications.SendMail = true;

                        dbContext.tbl_PM_EmployeeQualificationMatrix_History.AddObject(Qualifications);
                        dbContext.SaveChanges();
                    }

                    emp.EmployeeID = EmployeeId;
                    //emp.Courses = model.NewEmployeeQualification.Course;
                    if (model.Institute != null && model.Institute != "")
                        emp.Institute = model.Institute.Trim();
                    else
                        emp.Institute = model.Institute;
                    emp.PassoutYear = SelectedYearID;

                    //column percentage from db was not allowing to edit nvarchar values properly so using class column to store percentage as well grades
                    if (model.Percentage != null && model.Percentage != "")
                        emp.Class = model.Percentage.Trim();
                    else
                        emp.Class = model.Percentage;
                    if (model.Specialization != null && model.Specialization != "")
                        emp.Specialization = model.Specialization.Trim();
                    else
                        emp.Specialization = model.Specialization;
                    emp.QualificationTypeID = SelectedTypeID;
                    emp.QualificationID = SelectedQualificationID;
                    emp.QualificationGroupID = SelectedDegreeID;
                    if (model.University != null && model.University != "")
                        emp.University = model.University.Trim();
                    else
                        emp.University = model.University;
                }
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }
    }
}