using HRMS.Models;
using HRMS.Models.SkillMatrix;
using HRMS.Notification;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web.Security;

namespace HRMS.DAL
{
    public class PersonalDetailsDAL
    {
        private HRMSDBEntities dbContext = new HRMSDBEntities();
        private PMS3_HRMSDBEntities dbpms3Context = new PMS3_HRMSDBEntities();
        private WSEMDBEntities dbSEMContext = new WSEMDBEntities();
        private List<string> columnNameList = new List<string>();
        private List<string> FieldLabelList = new List<string>();
        private List<string> NewValueList = new List<string>();
        private V2toolsDBEntities dbv2toolsContext = new V2toolsDBEntities();
        private bool MemberInactive;

        public string GetNewEmployeecode(bool checkboxStatus)
        {
            string newEmpCode = string.Empty;
            int i = 0;

            try
            {
                if (checkboxStatus == false)
                {
                    long? employee = (from emp in dbContext.v_tbl_pm_Employee_v2
                                      select emp.EmployeeCode
                                     ).Max();

                    newEmpCode = employee.ToString();
                }
                else
                {
                    newEmpCode = (from emp in dbContext.HRMS_tbl_PM_Employee
                                  where emp.EmployeeCode.Length == 3
                                  select emp.EmployeeCode
                                     ).Max();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ((Convert.ToInt32(newEmpCode) + 1).ToString());
        }

        //check if the 'OLD_' entry is there in the system for a username or emailid
        // if exists update it with again 'OLD_'
        public int CheckUNameAndEmailExists(string Uname, string EmailId)
        {
            try
            {
                int i = 0;

                List<HRMS_tbl_PM_Employee> emp = (from e in dbContext.HRMS_tbl_PM_Employee
                                                  where e.UserName.Contains(Uname) && e.EmailID.Contains(EmailId)
                                                  orderby e.EmployeeID ascending
                                                  select e).ToList();

                if (emp.Count > 0)
                {
                    foreach (HRMS_tbl_PM_Employee e in emp)
                    {
                        e.UserName = "OLD_" + e.UserName;
                        e.EmailID = "OLD_" + e.EmailID;
                        dbContext.SaveChanges();
                    }
                    i = 1;
                    return i;
                }
                else
                    return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SwapEmpToPermanentAndContract(int empId, bool IsMoveToContract)
        {
            try
            {
                bool status = false;
                int count = 0;
                string Username = string.Empty;
                string FrshUsername = string.Empty;
                string FreshemailId = string.Empty;
                string ParentEmployeecode = string.Empty;
                string emailid = string.Empty;
                if (IsMoveToContract == true)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }

                int EmpId, IsNewEmployeeCreated = 0;

                #region Personal_Details Move

                HRMS_tbl_PM_Employee EmpDetails = (from e in dbContext.HRMS_tbl_PM_Employee
                                                   where e.EmployeeID == empId
                                                   select e).FirstOrDefault();

                if (EmpDetails != null)
                {
                    FrshUsername = EmpDetails.UserName;
                    FreshemailId = EmpDetails.EmailID;
                    ParentEmployeecode = EmpDetails.EmployeeCode;
                    HRMS_tbl_PM_Employee ContractEmpNewCopy = new HRMS_tbl_PM_Employee();
                    ContractEmpNewCopy = EmpDetails;

                    count = CheckUNameAndEmailExists(EmpDetails.UserName, EmpDetails.EmailID);

                    Username = FrshUsername;
                    emailid = FreshemailId;
                    EmpDetails.Status = Convert.ToBoolean(1);
                    EmpDetails.EmployeeStatusMasterID = 2;
                    EmpDetails.EmployeeStatusID = 6;
                    dbContext.SaveChanges();
                }

                HRMS_tbl_PM_Employee _tbl_Pm_Employee = new HRMS_tbl_PM_Employee();

                //new 3 digit code
                string EmpCode = GetNewEmployeecode(status);

                _tbl_Pm_Employee.EmployeeCode = EmpCode;
                _tbl_Pm_Employee.Status = Convert.ToBoolean(0);
                _tbl_Pm_Employee.UserName = Username;
                _tbl_Pm_Employee.EmailID = emailid;
                _tbl_Pm_Employee.Contract_Employee = EmpDetails.Contract_Employee;
                _tbl_Pm_Employee.Prefix = EmpDetails.Prefix;
                _tbl_Pm_Employee.FirstName = EmpDetails.FirstName;
                _tbl_Pm_Employee.MiddleName = EmpDetails.MiddleName;
                _tbl_Pm_Employee.LastName = EmpDetails.LastName;
                _tbl_Pm_Employee.EmployeeName = EmpDetails.FirstName + " " + EmpDetails.MiddleName + " " + EmpDetails.LastName;
                _tbl_Pm_Employee.Gender = EmpDetails.Gender;
                _tbl_Pm_Employee.BirthDate = EmpDetails.BirthDate;
                _tbl_Pm_Employee.MaritalStatus = EmpDetails.MaritalStatus;
                _tbl_Pm_Employee.WeddingDate = EmpDetails.WeddingDate;
                _tbl_Pm_Employee.Recognition = EmpDetails.Recognition;
                _tbl_Pm_Employee.ProfileImageName = EmpDetails.ProfileImageName;
                _tbl_Pm_Employee.ProfileImagePath = EmpDetails.ProfileImagePath;
                _tbl_Pm_Employee.Remarks = EmpDetails.Remarks;
                _tbl_Pm_Employee.JoiningCompanyID = 1;
                _tbl_Pm_Employee.NoOfChildren = EmpDetails.NoOfChildren.HasValue ? EmpDetails.NoOfChildren.Value : 0;
                _tbl_Pm_Employee.SpouseName = EmpDetails.SpouseName;
                _tbl_Pm_Employee.SpouseBirthdate = EmpDetails.SpouseBirthdate;

                _tbl_Pm_Employee.Child1Name = EmpDetails.Child1Name;
                _tbl_Pm_Employee.Child1Birthdate = EmpDetails.Child1Birthdate;

                _tbl_Pm_Employee.Child2Name = EmpDetails.Child2Name;
                _tbl_Pm_Employee.Child2Birthdate = EmpDetails.Child2Birthdate;

                _tbl_Pm_Employee.Child3Name = EmpDetails.Child3Name;
                _tbl_Pm_Employee.Child3Birthdate = EmpDetails.Child3Birthdate;

                _tbl_Pm_Employee.Child4Name = EmpDetails.Child4Name;
                _tbl_Pm_Employee.Child4Birthdate = EmpDetails.Child4Birthdate;

                _tbl_Pm_Employee.Child5Name = EmpDetails.Child5Name;
                _tbl_Pm_Employee.Child5Birthdate = EmpDetails.Child5Birthdate;

                _tbl_Pm_Employee.ReportingTo = EmpDetails.ReportingTo;
                _tbl_Pm_Employee.CostCenterID = EmpDetails.CostCenterID;
                _tbl_Pm_Employee.CompetencyManager = EmpDetails.CompetencyManager;
                _tbl_Pm_Employee.CustomFieldNumeric2 = 1;
                _tbl_Pm_Employee.IsLDAPAuthentication = false;
                //here cost centerid represents competency Manager Id
                _tbl_Pm_Employee.ShiftID = EmpDetails.ShiftID;
                _tbl_Pm_Employee.MaidenName = EmpDetails.MaidenName;
                _tbl_Pm_Employee.SeatingLocation = EmpDetails.SeatingLocation;

                _tbl_Pm_Employee.BloodGroup = EmpDetails.BloodGroup;

                _tbl_Pm_Employee.CurrentAddress = EmpDetails.CurrentAddress;
                _tbl_Pm_Employee.CurrentState = EmpDetails.CurrentState;
                _tbl_Pm_Employee.CurrentCity = EmpDetails.CurrentCity;
                _tbl_Pm_Employee.CurrentPinCode = EmpDetails.CurrentPinCode;
                _tbl_Pm_Employee.Address = EmpDetails.Address;
                _tbl_Pm_Employee.State = EmpDetails.State;
                _tbl_Pm_Employee.City = EmpDetails.City;
                _tbl_Pm_Employee.PinCode = EmpDetails.PinCode;
                _tbl_Pm_Employee.CountryID = EmpDetails.CountryID;
                _tbl_Pm_Employee.CurrentCountryID = EmpDetails.CurrentCountryID;

                //added by prasad
                _tbl_Pm_Employee.YIM = EmpDetails.YIM;
                _tbl_Pm_Employee.ResidenceVoIP = EmpDetails.ResidenceVoIP;
                _tbl_Pm_Employee.ExtensionNo = EmpDetails.ExtensionNo;
                _tbl_Pm_Employee.EmailID1 = EmpDetails.EmailID1;
                _tbl_Pm_Employee.VoIP = EmpDetails.VoIP;
                _tbl_Pm_Employee.RejoinedWithinYear = EmpDetails.RejoinedWithinYear;
                if (_tbl_Pm_Employee.RejoinedWithinYear == true)
                {
                    _tbl_Pm_Employee.ConfirmationDate = EmpDetails.JoiningDate;
                    _tbl_Pm_Employee.EmployeeStatusID = 1;
                }
                else
                    _tbl_Pm_Employee.ConfirmationDate = EmpDetails.ConfirmationDate;

                _tbl_Pm_Employee.EmailID3 = EmpDetails.EmailID3;
                _tbl_Pm_Employee.GTailkID = EmpDetails.GTailkID;
                _tbl_Pm_Employee.PersonalEmailID = EmpDetails.PersonalEmailID; // PersonalEmailID column is mapped as GTalkID in VW for HCM mapping.
                _tbl_Pm_Employee.CurrentPhone = EmpDetails.CurrentPhone;
                _tbl_Pm_Employee.MobileNumber = EmpDetails.MobileNumber;
                _tbl_Pm_Employee.AlternateContactNumber = EmpDetails.AlternateContactNumber;

                _tbl_Pm_Employee.PP_ExpiryDate = EmpDetails.PP_ExpiryDate;
                _tbl_Pm_Employee.PP_DateOfIssue = EmpDetails.PP_DateOfIssue;
                _tbl_Pm_Employee.PP_FullName = EmpDetails.PP_FullName;
                _tbl_Pm_Employee.PP_IsValid = EmpDetails.PP_IsValid.HasValue ? EmpDetails.PP_IsValid.Value : false;
                _tbl_Pm_Employee.NoofPagesLeft = EmpDetails.NoofPagesLeft.HasValue ? EmpDetails.NoofPagesLeft.Value : 0;
                _tbl_Pm_Employee.PassportNumber = EmpDetails.PassportNumber;
                _tbl_Pm_Employee.PP_PlaceOfIssue = EmpDetails.PP_PlaceOfIssue;
                _tbl_Pm_Employee.PP_RelativeName = EmpDetails.PP_RelativeName;

                _tbl_Pm_Employee.RelevantExperienceInMonths = EmpDetails.RelevantExperienceInMonths;
                _tbl_Pm_Employee.RelevantExperienceInYears = EmpDetails.RelevantExperienceInYears;

                //Following is forEmployeeDetails
                if (IsMoveToContract == true)
                {
                    _tbl_Pm_Employee.ContractFrom = DateTime.Now;
                    _tbl_Pm_Employee.EmployeeStatusMasterID = 1;
                    if (_tbl_Pm_Employee.RejoinedWithinYear == false)
                        _tbl_Pm_Employee.EmployeeStatusID = 20;
                    EmpDetails.LeavingDate = DateTime.Now;
                    MemberInactive = true;
                    aspnet_Membership _Membership = (from m in dbv2toolsContext.aspnet_Users
                                                     join roleID in dbv2toolsContext.aspnet_Membership on m.UserId equals roleID.UserId
                                                     where m.UserName == ParentEmployeecode
                                                     select roleID).FirstOrDefault();
                    _Membership.IsLockedOut = MemberInactive;
                    dbv2toolsContext.SaveChanges();
                    _tbl_Pm_Employee.EmployeeType = "Contract";
                }
                else
                {
                    _tbl_Pm_Employee.EmployeeStatusMasterID = 1;
                    if (_tbl_Pm_Employee.RejoinedWithinYear == false)
                        _tbl_Pm_Employee.EmployeeStatusID = 5;
                    EmpDetails.LeavingDate = DateTime.Now;
                    MemberInactive = true;
                    aspnet_Membership _Membership = (from m in dbv2toolsContext.aspnet_Users
                                                     join roleID in dbv2toolsContext.aspnet_Membership on m.UserId equals roleID.UserId
                                                     where m.UserName == ParentEmployeecode
                                                     select roleID).FirstOrDefault();
                    _Membership.IsLockedOut = MemberInactive;
                    dbv2toolsContext.SaveChanges();
                    _tbl_Pm_Employee.EmployeeType = "Regular";
                }
                _tbl_Pm_Employee.IsBillable = EmpDetails.IsBillable;
                _tbl_Pm_Employee.Commitments_Made = EmpDetails.Commitments_Made;
                _tbl_Pm_Employee.JoiningDate = DateTime.Now;

                _tbl_Pm_Employee.Probation_Review_Date = EmpDetails.Probation_Review_Date;

                _tbl_Pm_Employee.BusinessGroupID = EmpDetails.BusinessGroupID;
                _tbl_Pm_Employee.Current_DU = EmpDetails.Current_DU;
                _tbl_Pm_Employee.ResourcePoolID = EmpDetails.ResourcePoolID;
                _tbl_Pm_Employee.L_Y_Appraisal_Score = EmpDetails.L_Y_Appraisal_Score;
                _tbl_Pm_Employee.L_Y_Promotion_Status = EmpDetails.L_Y_Promotion_Status;
                _tbl_Pm_Employee.L_Y_Increment = EmpDetails.L_Y_Increment;
                _tbl_Pm_Employee.LocationID = EmpDetails.LocationID;
                _tbl_Pm_Employee.OfficeLocation = EmpDetails.OfficeLocation;
                _tbl_Pm_Employee.GroupID = EmpDetails.GroupID;
                _tbl_Pm_Employee.Recruiter_Name = EmpDetails.Recruiter_Name;
                _tbl_Pm_Employee.Region = EmpDetails.Region;
                _tbl_Pm_Employee.ESICNo = EmpDetails.ESICNo;
                _tbl_Pm_Employee.PFNo = EmpDetails.PFNo;
                _tbl_Pm_Employee.IncomeTaxNo = EmpDetails.IncomeTaxNo;
                _tbl_Pm_Employee.ReportingTo = EmpDetails.ReportingTo;
                _tbl_Pm_Employee.CompetencyManager = EmpDetails.CompetencyManager;
                _tbl_Pm_Employee.CostCenterID = EmpDetails.CostCenterID;
                _tbl_Pm_Employee.CalendarLocationId = EmpDetails.CalendarLocationId;
                _tbl_Pm_Employee.ShiftID = EmpDetails.ShiftID;
                _tbl_Pm_Employee.ReportingTime = EmpDetails.ReportingTime;
                _tbl_Pm_Employee.PostID = EmpDetails.PostID;
                _tbl_Pm_Employee.CreatedDate = DateTime.Now;

                dbContext.HRMS_tbl_PM_Employee.AddObject(_tbl_Pm_Employee);
                dbContext.SaveChanges();
                EmpId = _tbl_Pm_Employee.EmployeeID;
                string password = "mail_123";
                Membership.CreateUser(EmpCode, password);
                IsNewEmployeeCreated = 1;
                string[] ParentEmpRoles = Roles.GetRolesForUser(ParentEmployeecode);
                if (ParentEmpRoles.Length > 0)
                {
                    foreach (string Role in ParentEmpRoles)
                    {
                        Roles.AddUserToRole(EmpCode, Role);
                    }
                }

                #endregion Personal_Details Move

                #region Hobbies Move

                tbl_HR_Hobbies _tbl_HR_Hobbies = (from e in dbContext.tbl_HR_Hobbies
                                                  where e.EmployeeID == empId
                                                  select e).FirstOrDefault();

                if (_tbl_HR_Hobbies != null)
                {
                    tbl_HR_Hobbies Obj_tbl_HR_Hobbies = new tbl_HR_Hobbies();
                    Obj_tbl_HR_Hobbies = _tbl_HR_Hobbies;
                    Obj_tbl_HR_Hobbies.EmployeeID = EmpId;
                    dbContext.ObjectStateManager.ChangeObjectState(Obj_tbl_HR_Hobbies, System.Data.EntityState.Added);
                    dbContext.tbl_HR_Hobbies.AddObject(Obj_tbl_HR_Hobbies);
                }

                #endregion Hobbies Move

                #region ResourcePoolDetail Move

                tbl_PM_ResourcePoolDetail _tbl_PM_ResourcePoolDetail = (from e in dbContext.tbl_PM_ResourcePoolDetail
                                                                        where e.EmployeeID == empId
                                                                        select e).FirstOrDefault();

                if (_tbl_PM_ResourcePoolDetail != null)
                {
                    tbl_PM_ResourcePoolDetail Obj_tbl_PM_ResourcePoolDetail = new tbl_PM_ResourcePoolDetail();
                    Obj_tbl_PM_ResourcePoolDetail = _tbl_PM_ResourcePoolDetail;
                    Obj_tbl_PM_ResourcePoolDetail.EmployeeID = EmpId;
                    dbContext.ObjectStateManager.ChangeObjectState(Obj_tbl_PM_ResourcePoolDetail, System.Data.EntityState.Added);
                    dbContext.tbl_PM_ResourcePoolDetail.AddObject(Obj_tbl_PM_ResourcePoolDetail);
                }

                #endregion ResourcePoolDetail Move

                #region Achievement Move

                tbl_HR_Achievement _tbl_HR_Achievement = (from e in dbContext.tbl_HR_Achievement
                                                          where e.EmployeeID == empId
                                                          select e).FirstOrDefault();

                if (_tbl_HR_Achievement != null)
                {
                    tbl_HR_Achievement Obj_tbl_HR_Achievement = new tbl_HR_Achievement();
                    Obj_tbl_HR_Achievement = _tbl_HR_Achievement;
                    Obj_tbl_HR_Achievement.EmployeeID = EmpId;
                    dbContext.ObjectStateManager.ChangeObjectState(Obj_tbl_HR_Achievement, System.Data.EntityState.Added);
                    dbContext.tbl_HR_Achievement.AddObject(Obj_tbl_HR_Achievement);
                }

                #endregion Achievement Move

                #region Emp_Dependands Move

                List<tbl_PM_Employee_Dependands> empSpouseAndChildDetails = (from dependantans in dbContext.tbl_PM_Employee_Dependands
                                                                             where dependantans.EmployeeID == empId
                                                                             select dependantans).ToList();

                if (empSpouseAndChildDetails.Count > 0)
                {
                    foreach (tbl_PM_Employee_Dependands dep in empSpouseAndChildDetails)
                    {
                        if (dep != null)
                        {
                            tbl_PM_Employee_Dependands ObjnewDep = new tbl_PM_Employee_Dependands();
                            ObjnewDep = dep;
                            ObjnewDep.EmployeeID = EmpId;
                            dbContext.ObjectStateManager.ChangeObjectState(ObjnewDep, System.Data.EntityState.Added);
                            dbContext.tbl_PM_Employee_Dependands.AddObject(ObjnewDep);
                        }
                    }
                }

                #endregion Emp_Dependands Move

                #region Emp_ContactMove

                List<tbl_PM_EmployeeEmergencyContact> empEmergencyDetails = (from dependantans in dbContext.tbl_PM_EmployeeEmergencyContact
                                                                             where dependantans.EmployeeID == empId
                                                                             select dependantans).ToList();

                if (empEmergencyDetails.Count > 0)
                {
                    foreach (tbl_PM_EmployeeEmergencyContact EmgCont in empEmergencyDetails)
                    {
                        if (EmgCont != null)
                        {
                            tbl_PM_EmployeeEmergencyContact ObjnewEmgCont = new tbl_PM_EmployeeEmergencyContact();
                            ObjnewEmgCont = EmgCont;
                            ObjnewEmgCont.EmployeeID = EmpId;
                            dbContext.ObjectStateManager.ChangeObjectState(ObjnewEmgCont, System.Data.EntityState.Added);
                            dbContext.tbl_PM_EmployeeEmergencyContact.AddObject(ObjnewEmgCont);
                        }
                    }
                }

                #endregion Emp_ContactMove

                #region EmpQualification Move

                List<tbl_PM_EmployeeQualificationMatrix> EmpQualification = (from dependantans in dbContext.tbl_PM_EmployeeQualificationMatrix
                                                                             where dependantans.EmployeeID == empId
                                                                             select dependantans).ToList();

                if (EmpQualification.Count > 0)
                {
                    foreach (tbl_PM_EmployeeQualificationMatrix Empqual in EmpQualification)
                    {
                        if (Empqual != null)
                        {
                            tbl_PM_EmployeeQualificationMatrix ObjnewEmpqual = new tbl_PM_EmployeeQualificationMatrix();
                            ObjnewEmpqual = Empqual;
                            ObjnewEmpqual.EmployeeID = EmpId;
                            dbContext.ObjectStateManager.ChangeObjectState(ObjnewEmpqual, System.Data.EntityState.Added);
                            dbContext.tbl_PM_EmployeeQualificationMatrix.AddObject(ObjnewEmpqual);
                        }
                    }
                }

                #endregion EmpQualification Move

                #region EmployeeCertification Move

                List<tbl_PM_EmployeeCertificationMatrix> EmpCertification = (from dependantans in dbContext.tbl_PM_EmployeeCertificationMatrix
                                                                             where dependantans.EmployeeID == empId
                                                                             select dependantans).ToList();

                if (EmpCertification.Count > 0)
                {
                    foreach (tbl_PM_EmployeeCertificationMatrix EmpCertD in EmpCertification)
                    {
                        if (EmpCertD != null)
                        {
                            tbl_PM_EmployeeCertificationMatrix ObjnewEmpCertD = new tbl_PM_EmployeeCertificationMatrix();
                            ObjnewEmpCertD = EmpCertD;
                            ObjnewEmpCertD.EmployeeID = EmpId;
                            dbContext.ObjectStateManager.ChangeObjectState(ObjnewEmpCertD, System.Data.EntityState.Added);
                            dbContext.tbl_PM_EmployeeCertificationMatrix.AddObject(ObjnewEmpCertD);
                        }
                    }
                }

                #endregion EmployeeCertification Move

                #region SkillDetails Move

                List<tbl_PM_EmployeeSkillMatrix> employeeSkillDetailsList = (dbContext.tbl_PM_EmployeeSkillMatrix.
                                                                            Where(ed => ed.EmployeeID == empId)).ToList();

                if (employeeSkillDetailsList.Count > 0)
                {
                    foreach (tbl_PM_EmployeeSkillMatrix Skill in employeeSkillDetailsList)
                    {
                        if (Skill != null)
                        {
                            tbl_PM_EmployeeSkillMatrix ObjSkill = new tbl_PM_EmployeeSkillMatrix();
                            ObjSkill = Skill;
                            ObjSkill.EmployeeID = EmpId;
                            dbContext.ObjectStateManager.ChangeObjectState(ObjSkill, System.Data.EntityState.Added);
                            dbContext.tbl_PM_EmployeeSkillMatrix.AddObject(ObjSkill);
                        }
                    }
                }

                #endregion SkillDetails Move

                #region Medical D Move

                List<tbl_PM_MedicalDescription> MedicalDesc = (from medicalHistory in dbContext.tbl_PM_MedicalDescription
                                                               where medicalHistory.Employee_Id == empId
                                                               select medicalHistory).ToList();

                if (MedicalDesc.Count > 0)
                {
                    foreach (tbl_PM_MedicalDescription Med in MedicalDesc)
                    {
                        if (Med != null)
                        {
                            tbl_PM_MedicalDescription ObjMed = new tbl_PM_MedicalDescription();
                            ObjMed = Med;
                            ObjMed.Employee_Id = EmpId;
                            dbContext.ObjectStateManager.ChangeObjectState(ObjMed, System.Data.EntityState.Added);
                            dbContext.tbl_PM_MedicalDescription.AddObject(ObjMed);
                        }
                    }
                }

                #endregion Medical D Move

                #region Designation Change

                List<tbl_PM_EmployeeDesignation_Change> Designation = (from Desig in dbContext.tbl_PM_EmployeeDesignation_Change
                                                                       where Desig.EmployeeID == empId
                                                                       select Desig).ToList();

                if (Designation.Count > 0)
                {
                    foreach (tbl_PM_EmployeeDesignation_Change EmpDesignation in Designation)
                    {
                        if (EmpDesignation != null)
                        {
                            tbl_PM_EmployeeDesignation_Change ObjEmpDesignation = new tbl_PM_EmployeeDesignation_Change();
                            ObjEmpDesignation = EmpDesignation;
                            ObjEmpDesignation.EmployeeID = EmpId;
                            dbContext.ObjectStateManager.ChangeObjectState(ObjEmpDesignation, System.Data.EntityState.Added);
                            dbContext.tbl_PM_EmployeeDesignation_Change.AddObject(ObjEmpDesignation);
                        }
                    }
                }

                #endregion Designation Change

                #region EmployeeVisaDetails

                List<tbl_PM_EmployeeVisaDetails> EmpVisa = (from Desig in dbContext.tbl_PM_EmployeeVisaDetails
                                                            where Desig.EmployeeID == empId
                                                            select Desig).ToList();

                if (EmpVisa.Count > 0)
                {
                    foreach (tbl_PM_EmployeeVisaDetails Visa in EmpVisa)
                    {
                        if (Visa != null)
                        {
                            tbl_PM_EmployeeVisaDetails ObjVisa = new tbl_PM_EmployeeVisaDetails();
                            ObjVisa = Visa;
                            ObjVisa.EmployeeID = EmpId;
                            dbContext.ObjectStateManager.ChangeObjectState(ObjVisa, System.Data.EntityState.Added);
                            dbContext.tbl_PM_EmployeeVisaDetails.AddObject(ObjVisa);
                        }
                    }
                }

                #endregion EmployeeVisaDetails

                #region EmployeeDependandsVisaDetails

                List<tbl_PM_DependandsVisaDetails> DepVisa = ((from visaDetails in dbContext.tbl_PM_DependandsVisaDetails
                                                               join dependentDetails in dbContext.tbl_PM_Employee_Dependands on visaDetails.DependandsID equals dependentDetails.DependandsID
                                                               where dependentDetails.EmployeeID == empId
                                                               select visaDetails).ToList());

                if (DepVisa.Count > 0)
                {
                    foreach (tbl_PM_DependandsVisaDetails DVisa in DepVisa)
                    {
                        if (DVisa != null)
                        {
                            int parenDepId = (from d in dbContext.tbl_PM_Employee_Dependands
                                              where d.DependandsID == DVisa.DependandsID
                                              select d.DependandsID).FirstOrDefault();

                            tbl_PM_DependandsVisaDetails ObjDVisa = new tbl_PM_DependandsVisaDetails();

                            ObjDVisa = DVisa;
                            ObjDVisa.DependandsID = parenDepId;
                            dbContext.ObjectStateManager.ChangeObjectState(ObjDVisa, System.Data.EntityState.Added);
                            dbContext.tbl_PM_DependandsVisaDetails.AddObject(ObjDVisa);
                        }
                    }
                }

                #endregion EmployeeDependandsVisaDetails

                #region Project_Resource_Mapping Move

                long ParenteCode = (long)Convert.ToDouble(ParentEmployeecode);
                List<Project_Resource_Mapping> Project = (from Desig in dbpms3Context.Project_Resource_Mapping
                                                          where Desig.UserID == ParenteCode
                                                          select Desig).ToList();

                if (Project.Count > 0)
                {
                    foreach (Project_Resource_Mapping EmpProject in Project)
                    {
                        if (EmpProject != null)
                        {
                            Project_Resource_Mapping ObjEmpProject = new Project_Resource_Mapping();
                            long ContractProjectCode = (long)Convert.ToDouble(EmpCode);
                            ObjEmpProject = EmpProject;
                            ObjEmpProject.UserID = ContractProjectCode;
                            dbpms3Context.ObjectStateManager.ChangeObjectState(ObjEmpProject, System.Data.EntityState.Added);
                            dbpms3Context.Project_Resource_Mapping.AddObject(ObjEmpProject);
                        }
                    }
                }

                #endregion Project_Resource_Mapping Move

                #region ProjectEmployeeRole Move

                List<tbl_PM_ProjectEmployeeRole> EmpProjectRole = (from Desig in dbSEMContext.tbl_PM_ProjectEmployeeRole
                                                                   where Desig.EmployeeID == empId
                                                                   select Desig).ToList();

                if (EmpProjectRole.Count > 0)
                {
                    foreach (tbl_PM_ProjectEmployeeRole ProjectRole in EmpProjectRole)
                    {
                        if (ProjectRole != null)
                        {
                            tbl_PM_ProjectEmployeeRole ObjProjectRole = new tbl_PM_ProjectEmployeeRole();
                            ObjProjectRole = ProjectRole;
                            ObjProjectRole.EmployeeID = EmpId;
                            dbSEMContext.ObjectStateManager.ChangeObjectState(ObjProjectRole, System.Data.EntityState.Added);
                            dbSEMContext.tbl_PM_ProjectEmployeeRole.AddObject(ObjProjectRole);
                        }
                    }
                }

                #endregion ProjectEmployeeRole Move

                #region Emp_PastExp Details Move

                List<HRMS_tbl_PM_EmployeeHistory> EmpPastExp = (from Desig in dbContext.HRMS_tbl_PM_EmployeeHistory
                                                                where Desig.EmployeeID == empId
                                                                select Desig).ToList();

                if (EmpPastExp.Count > 0)
                {
                    foreach (HRMS_tbl_PM_EmployeeHistory EmpHis in EmpPastExp)
                    {
                        if (EmpHis != null)
                        {
                            HRMS_tbl_PM_EmployeeHistory ObjEmpHis = new HRMS_tbl_PM_EmployeeHistory();
                            ObjEmpHis = EmpHis;
                            ObjEmpHis.EmployeeID = EmpId;
                            dbContext.ObjectStateManager.ChangeObjectState(ObjEmpHis, System.Data.EntityState.Added);
                            dbContext.HRMS_tbl_PM_EmployeeHistory.AddObject(ObjEmpHis);
                        }
                    }
                }

                #endregion Emp_PastExp Details Move

                #region EmployeeQualificationGap Move

                List<tbl_PM_EmployeeQualificationGap> EmpGapDetails = (from Desig in dbContext.tbl_PM_EmployeeQualificationGap
                                                                       where Desig.EmployeeID == empId
                                                                       select Desig).ToList();

                if (EmpGapDetails.Count > 0)
                {
                    foreach (tbl_PM_EmployeeQualificationGap EmpGap in EmpGapDetails)
                    {
                        if (EmpGap != null)
                        {
                            tbl_PM_EmployeeQualificationGap ObjEmpGap = new tbl_PM_EmployeeQualificationGap();
                            ObjEmpGap = EmpGap;
                            ObjEmpGap.EmployeeID = EmpId;
                            dbContext.ObjectStateManager.ChangeObjectState(ObjEmpGap, System.Data.EntityState.Added);
                            dbContext.tbl_PM_EmployeeQualificationGap.AddObject(ObjEmpGap);
                        }
                    }
                }

                #endregion EmployeeQualificationGap Move

                #region EmployeeUpoloadDocs Move

                List<Tbl_Employee_Documents> EmpUploadDocs = (from dependantans in dbContext.Tbl_Employee_Documents
                                                              where dependantans.EmployeeId == empId
                                                              select dependantans).ToList();

                if (EmpUploadDocs.Count > 0)
                {
                    foreach (Tbl_Employee_Documents EmpUploadD in EmpUploadDocs)
                    {
                        if (EmpUploadD != null)
                        {
                            Tbl_Employee_Documents ObjnewEmpUploadD = new Tbl_Employee_Documents();
                            ObjnewEmpUploadD = EmpUploadD;
                            ObjnewEmpUploadD.EmployeeId = EmpId;
                            dbContext.ObjectStateManager.ChangeObjectState(ObjnewEmpUploadD, System.Data.EntityState.Added);
                            dbContext.Tbl_Employee_Documents.AddObject(ObjnewEmpUploadD);
                        }
                    }
                }

                #endregion EmployeeUpoloadDocs Move

                #region Emp_Bond move (hardik)

                List<tbl_BondDetails> BondDetail = (from bond in dbContext.tbl_BondDetails
                                                    where bond.Employee_Id == empId
                                                    select bond).ToList();

                if (BondDetail.Count > 0)
                {
                    foreach (tbl_BondDetails bond in BondDetail)
                    {
                        if (bond != null)
                        {
                            tbl_BondDetails ObjBond = new tbl_BondDetails();
                            ObjBond = bond;
                            ObjBond.Employee_Id = EmpId;
                            dbContext.ObjectStateManager.ChangeObjectState(ObjBond, System.Data.EntityState.Added);
                            dbContext.tbl_BondDetails.AddObject(ObjBond);
                        }
                    }
                }

                #endregion Emp_Bond move (hardik)

                #region Emp_Discipline move (hardik)

                List<Tbl_PM_Disciplinary> DisciplineDetail = (from discipline in dbContext.Tbl_PM_Disciplinary
                                                              where discipline.EmployeeId == empId
                                                              select discipline).ToList();

                if (DisciplineDetail.Count > 0)
                {
                    foreach (Tbl_PM_Disciplinary discipline in DisciplineDetail)
                    {
                        if (discipline != null)
                        {
                            Tbl_PM_Disciplinary Objdiscipline = new Tbl_PM_Disciplinary();
                            Objdiscipline = discipline;
                            Objdiscipline.EmployeeId = EmpId;
                            dbContext.ObjectStateManager.ChangeObjectState(Objdiscipline, System.Data.EntityState.Added);
                            dbContext.Tbl_PM_Disciplinary.AddObject(Objdiscipline);
                        }
                    }
                }

                #endregion Emp_Discipline move (hardik)

                int result = dbContext.SaveChanges(SaveOptions.DetectChangesBeforeSave);

                if (IsNewEmployeeCreated == 1)
                    return Convert.ToInt32(EmpId);
                else
                    return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ContractPermanentDetails> GetEmpContractPermanentHistory(int page, int rows, int employeeId, out int totalCount)
        {
            try
            {
                string UserNameToPass = string.Empty;
                List<ContractPermanentDetails> obj = new List<ContractPermanentDetails>();

                HRMS_tbl_PM_Employee empDetails = (from e in dbContext.HRMS_tbl_PM_Employee
                                                   where e.EmployeeID == employeeId
                                                   select e).FirstOrDefault();

                if (empDetails != null)
                {
                    if (empDetails.UserName.Contains("OLD_"))
                    {
                        int index = empDetails.UserName.LastIndexOf("_");
                        UserNameToPass = empDetails.UserName.Substring(index + 1);
                    }
                    else
                    {
                        UserNameToPass = empDetails.UserName;
                    }

                    List<HRMS_tbl_PM_Employee> reocordsToFetch = (from e in dbContext.HRMS_tbl_PM_Employee
                                                                  where e.UserName.Contains(UserNameToPass) && e.BirthDate == empDetails.BirthDate
                                                                  select e
                                                           ).Distinct().ToList();

                    foreach (HRMS_tbl_PM_Employee v in reocordsToFetch)
                    {
                        ContractPermanentDetails contractobj = new ContractPermanentDetails();

                        if (v.EmployeeType == "Regular")
                        {
                            v.EmployeeType = "Permanent";
                        }
                        else if (v.EmployeeType == null || v.EmployeeType == "Contract")
                        {
                            v.EmployeeType = "Contract";
                        }
                        if (v.Status == Convert.ToBoolean(0))
                        {
                            contractobj.EmployeeCodeStatus = "Active";
                        }
                        else if (v.Status == Convert.ToBoolean(1))
                        {
                            contractobj.EmployeeCodeStatus = "Inactive";
                        }

                        contractobj.OldEmployeecode = v.EmployeeCode;
                        contractobj.EmployeeType = v.EmployeeType;
                        contractobj.CreatedDate = v.CreatedDate;

                        obj.Add(contractobj);
                    }

                    totalCount = obj.Count;
                    return obj.Skip((page - 1) * rows).Take(rows).ToList();
                }
                else
                {
                    // obj = null;
                    totalCount = 0;
                    return obj.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CountryDetails> GetCountryDetails()
        {
            List<CountryDetails> conutries = new List<CountryDetails>();
            try
            {
                conutries = (from country in dbContext.tbl_PM_CountryMaster
                             orderby country.CountryName
                             select new CountryDetails
                             {
                                 CountryId = country.CountryID,
                                 CountryName = country.CountryName
                             }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return conutries;
        }

        /// <summary>
        /// To retrieve residential details of Employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public HRMS_tbl_PM_Employee GetEmployeePersonalDetails(int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                HRMS_tbl_PM_Employee PersonalDetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();
                return PersonalDetails;
            }
            catch (Exception ex)
            {
                SMTPHelper objFileLog = new SMTPHelper();
                objFileLog.AddAnEntryToLogFile("GetEmployeePersonalDetails", ex.Message);
                throw;
            }
        }

        public tbl_PM_CountryMaster GetCountryID(string countryname)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PM_CountryMaster country = dbContext.tbl_PM_CountryMaster.Where(ed => ed.CountryName == countryname).FirstOrDefault();
                return country;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public HRMS_tbl_PM_Employee GetEmployeeDetailsFromEmpCode(int employeeCode)
        {
            try
            {
                string empcode = employeeCode.ToString();
                dbContext = new HRMSDBEntities();
                HRMS_tbl_PM_Employee EmpDetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeCode == empcode && ed.Status == false).FirstOrDefault();
                return EmpDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public HRMS_tbl_PM_Employee GetEmployeeDetailsFromEmailId(string Email)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                HRMS_tbl_PM_Employee EmpDetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmailID == Email).FirstOrDefault();
                return EmpDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DateTime GetEmployeeBirthDate(int employeeId)
        {
            DateTime birthDate;
            try
            {
                birthDate = Convert.ToDateTime(dbContext.HRMS_tbl_PM_Employee.Where(em => em.EmployeeID == employeeId).Select(em => em.BirthDate).SingleOrDefault());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return birthDate;
        }

        //public List<ShiftList> GetShiftList()
        //{
        //    List<ShiftList> Shifts = new List<ShiftList>();
        //    try
        //    {
        //        Shifts = (from shift in dborbit.ShiftMasters
        //                  where shift.ISActive == true
        //                  orderby shift.ShiftID ascending
        //                  select new ShiftList
        //                  {
        //                      ShiftId = shift.ShiftID,
        //                      ShiftName = shift.ShiftName

        //                  }).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Shifts;
        //}

        //public List<int> GetHrs()
        //{
        //    List<int> Hrs = new List<int>();
        //    try
        //    {
        //        for (int i = 0; i <= 24; i++)
        //        {
        //            Hrs.Add(i);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Hrs;
        //}

        //public List<int> GetMins()
        //{
        //    List<int> Mins = new List<int>();
        //    try
        //    {
        //        for (int i = 0; i <= 60; i++)
        //        {
        //            Mins.Add(i);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Mins;
        //}

        //public int? GetShiftId(int employeeId)
        //{
        //    try
        //    {
        //        dbContext = new HRMSDBEntities();
        //        int? ShiftDetail = (from shift in dbContext.HRMS_tbl_PM_Employee
        //                             where shift.EmployeeID == employeeId
        //                             select shift.ShiftID).FirstOrDefault();
        //        return ShiftDetail;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        //public ShiftMaster GetShiftName(int shiftId)
        //{
        //    try
        //    {
        //    ShiftMaster shiftName= (from  Sname in dborbit.ShiftMasters
        //                            where Sname.ShiftID==shiftId
        //                            select Sname.ShiftName ).FirstOrDefault();

        //       return shiftName ;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public List<RepotingToList> GetReportingToList()
        //{
        //    List<RepotingToList> resourcepool = new List<RepotingToList>();
        //    try
        //    {
        //        resourcepool = (from resource in dbContext.HRMS_tbl_PM_Employee
        //                        where resource.Status == false
        //                        orderby resource.EmployeeName ascending
        //                        select new RepotingToList
        //                        {
        //                            EmployeeId = resource.EmployeeID,
        //                            EmployeeName = resource.EmployeeName
        //                        }).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return resourcepool;
        //}

        //public HRMS_tbl_PM_Employee GetEmployeeReportingToName(int employeeId)
        //{
        //    try
        //    {
        //        dbContext = new HRMSDBEntities();
        //        int ReportingToId = (from id in dbContext.HRMS_tbl_PM_Employee
        //                             where id.EmployeeID == employeeId

        //                             select id.CostCenterID.HasValue ? id.CostCenterID.Value : 0).FirstOrDefault();

        //        if (ReportingToId > 0)
        //        {
        //            HRMS_tbl_PM_Employee ReportingTo = (from name in dbContext.HRMS_tbl_PM_Employee
        //                                           where name.EmployeeID == ReportingToId
        //                                           select name).FirstOrDefault();

        //            return ReportingTo;
        //        }
        //        else
        //            return null;

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public HRMS_tbl_PM_Employee GetExitConfirmationManagerName(int employeeId)
        //{
        //    try
        //    {
        //        //CostCenter represents competency Manager Id
        //        dbContext = new HRMSDBEntities();
        //        int ExitConfirmationManagerId = (from id in dbContext.HRMS_tbl_PM_Employee
        //                                         where id.EmployeeID == employeeId

        //                                         select id.ReportingTo.HasValue ? id.ReportingTo.Value : 0).FirstOrDefault();

        //        if (ExitConfirmationManagerId > 0)
        //        {
        //            HRMS_tbl_PM_Employee ExitConfirmationManager = (from name in dbContext.HRMS_tbl_PM_Employee
        //                                                       where name.EmployeeID == ExitConfirmationManagerId
        //                                                       select name).FirstOrDefault();

        //            return ExitConfirmationManager;
        //        }
        //        else
        //            return null;

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public HRMS_tbl_PM_Employee GetCompetencyManagerName(int employeeId)
        //{
        //    try
        //    {
        //        //CostCenter represents competency Manager Id
        //        dbContext = new HRMSDBEntities();
        //        int CompetencyManagerId = (from id in dbContext.HRMS_tbl_PM_Employee
        //                                   where id.EmployeeID == employeeId

        //                                   select id.CompetencyManager.HasValue ? id.CompetencyManager.Value : 0).FirstOrDefault();

        //        if (CompetencyManagerId > 0)
        //        {
        //            HRMS_tbl_PM_Employee CompetencyManager = (from name in dbContext.HRMS_tbl_PM_Employee
        //                                                 where name.EmployeeID == CompetencyManagerId
        //                                                 select name).FirstOrDefault();

        //            return CompetencyManager;
        //        }
        //        else
        //            return null;

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public List<tbl_PM_Employee_Dependands> GetEmpDependantsDetails(int employeeId)
        {
            try
            {
                List<tbl_PM_Employee_Dependands> empSpouseAndChildDetails = (from dependantans in dbContext.tbl_PM_Employee_Dependands
                                                                             where (dependantans.RelationType == "Wife"
                                                                              || dependantans.RelationType == "Husband"
                                                                              || dependantans.RelationType == null)
                                                                              && dependantans.EmployeeID == employeeId
                                                                             select dependantans).ToList();

                return empSpouseAndChildDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To save blood group of employee in "HRMS_tbl_PM_Employee" table
        /// </summary>
        /// <param name="bloodGroup"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public bool SaveBloodGroup(string bloodGroup, int employeeId)
        {
            bool isAdded = false;
            dbContext = new HRMSDBEntities();
            //string blood_groupName = dbContext.tbl_BloodGroup.Where(ed => ed.BloodGroup_Id == bg_Id).FirstOrDefault();
            //string blood_groupName = (from i in dbContext.tbl_BloodGroup where i.BloodGroup_Id==bg_Id select i.BloodGroup_Name ).FirstOrDefault();
            HRMS_tbl_PM_Employee details = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();
            if (details == null || details.EmployeeID <= 0)
            {
                dbContext.HRMS_tbl_PM_Employee.AddObject(details);
                dbContext.SaveChanges();
            }
            else
            {
                details.BloodGroup = bloodGroup;
                dbContext.SaveChanges();
            }
            isAdded = true;
            return isAdded;
        }

        public tbl_HR_Hobbies GetEmployeeID(int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_HR_Hobbies PersonalDetails = dbContext.tbl_HR_Hobbies.Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();

                return PersonalDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public HRMS_tbl_PM_Employee GetEmployeeCode()
        //{
        //    try
        //    {
        //        dbContext = new HRMSDBEntities();
        //        //HRMS_tbl_PM_Employee EmployeeCode = dbContext.HRMS_tbl_PM_Employee..Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();
        //        HRMS_tbl_PM_Employee EmployeeCode = from x in dbContext.HRMS_tbl_PM_Employee
        //                    group by x.EmployeeCode by x.id2 into values
        //                    select values.Max();

        //        return EmployeeCode;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public tbl_HR_Achievement GetEmployeeID_Achvment(int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_HR_Achievement ObjAchievement = dbContext.tbl_HR_Achievement.Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();

                return ObjAchievement;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To add Residential Details into DataBase
        /// </summary>
        /// <param name="ResidentialDetails"></param>
        /// <returns></returns>
        public bool AddResidentialDetails(HRMS_tbl_PM_Employee ResidentialDetails)
        {
            try
            {
                bool isAdded = false;

                HRMS_tbl_PM_Employee details = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == ResidentialDetails.EmployeeID).FirstOrDefault();
                if (details == null || details.EmployeeID <= 0)
                {
                    dbContext.HRMS_tbl_PM_Employee.AddObject(ResidentialDetails);
                    dbContext.SaveChanges();
                }
                else
                {
                    details.CurrentAddress = ResidentialDetails.CurrentAddress.Trim();
                    details.CurrentState = ResidentialDetails.CurrentState.Trim();
                    details.CurrentCity = ResidentialDetails.CurrentCity.Trim();
                    details.CurrentPinCode = ResidentialDetails.CurrentPinCode.Trim();
                    details.Address = ResidentialDetails.Address.Trim();
                    details.State = ResidentialDetails.State.Trim();
                    details.City = ResidentialDetails.City.Trim();
                    details.PinCode = ResidentialDetails.PinCode.Trim();
                    details.CountryID = ResidentialDetails.CountryID;
                    details.CurrentCountryID = ResidentialDetails.CurrentCountryID;

                    dbContext.SaveChanges();
                }
                isAdded = true;
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Getting UserName from the Employee table based on Employee Id value
        /// </summary>
        /// <param name="employeeId">The Id of the employee whose User name we have to get</param>
        /// <returns>Gives user name of the employee</returns>
        public string GetUserNameByEmployeeId(int employeeId)
        {
            string userName = string.Empty;
            try
            {
                userName = dbContext.HRMS_tbl_PM_Employee.Where(em => em.EmployeeID == employeeId).Select(em => em.UserName).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userName;
        }

        //public DateTime GetEmployeeJoiningDate(int employeeId)
        //{
        //    DateTime joiningDate;
        //    try
        //    {
        //        joiningDate =Convert.ToDateTime(dbContext.HRMS_tbl_PM_Employee.Where(em => em.EmployeeID == employeeId).Select(em => em.JoiningDate).SingleOrDefault());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return joiningDate;
        //}

        /// <summary>
        /// To add contact details to database
        /// </summary>
        /// <param name="model"></param>
        public bool SaveContactDetails(ContactDetailsViewModel model)
        {
            try
            {
                bool isAdded = false;
                string userName = GetUserNameByEmployeeId(model.EmployeeId.HasValue ? model.EmployeeId.Value : 0);

                var empContactDetails = dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeID == model.EmployeeId).SingleOrDefault();
                var empEmergencyDetails = dbContext.tbl_PM_EmployeeEmergencyContact.Where(x => x.EmployeeID == model.EmployeeId).FirstOrDefault();

                if (empContactDetails == null && empEmergencyDetails == null)
                {
                    HRMS_tbl_PM_Employee empDetails = new HRMS_tbl_PM_Employee();
                    empDetails.EmployeeID = model.EmployeeId.Value;
                    empDetails.UserName = userName;
                    if (model.AlternateEmailId != null && model.AlternateEmailId != "")
                        empDetails.EmailID3 = model.AlternateEmailId.Trim();
                    else
                        empDetails.EmailID3 = model.AlternateEmailId;

                    empDetails.MobileNumber = model.MobileNumber.Trim();
                    empDetails.CurrentPhone = model.ResidenceNumber.Trim();
                    if (model.SkypeId != null && model.SkypeId != "")
                        empDetails.GTailkID = model.SkypeId.Trim();
                    else
                        empDetails.GTailkID = model.SkypeId;

                    if (model.GtalkId != null && model.GtalkId != "")
                        empDetails.PersonalEmailID = model.GtalkId.Trim(); // PersonalEmailID column is mapped as GTalkID in VW for HCM mapping.
                    else
                        empDetails.PersonalEmailID = model.GtalkId;

                    empDetails.EmailID = model.OfficeEmailId;
                    if (model.AlternateContactNumber != null && model.AlternateContactNumber != "")
                        empDetails.AlternateContactNumber = model.AlternateContactNumber.Trim();
                    else
                        empDetails.AlternateContactNumber = model.AlternateContactNumber;

                    if (model.SeatingLocation != null && model.SeatingLocation != "")
                        empDetails.SeatingLocation = model.SeatingLocation.Trim();
                    else
                        empDetails.SeatingLocation = model.SeatingLocation;

                    if (model.OfficeVoip != null && model.OfficeVoip != "")
                        empDetails.VoIP = model.OfficeVoip.Trim();
                    else
                        empDetails.VoIP = model.OfficeVoip;

                    if (model.ResidenceVoip != null && model.ResidenceVoip != "")
                        empDetails.ResidenceVoIP = model.ResidenceVoip.Trim();
                    else
                        empDetails.ResidenceVoIP = model.ResidenceVoip;

                    empDetails.EmailID1 = model.PersonalEmailId.Trim();

                    if (model.YIMId != null && model.YIMId != "")
                        empDetails.YIM = model.YIMId.Trim();
                    else
                        empDetails.YIM = model.YIMId;

                    dbContext.HRMS_tbl_PM_Employee.AddObject(empDetails);
                }
                else
                {
                    empContactDetails.EmployeeID = model.EmployeeId.Value;
                    empContactDetails.UserName = userName;
                    if (model.AlternateEmailId != null && model.AlternateEmailId != "")
                        empContactDetails.EmailID3 = model.AlternateEmailId.Trim();
                    else
                        empContactDetails.EmailID3 = model.AlternateEmailId;

                    empContactDetails.EmailID = model.OfficeEmailId;
                    if (model.SkypeId != null && model.SkypeId != "")
                        empContactDetails.GTailkID = model.SkypeId.Trim();
                    else
                        empContactDetails.GTailkID = model.SkypeId;

                    if (model.GtalkId != null && model.GtalkId != "")
                        empContactDetails.PersonalEmailID = model.GtalkId.Trim(); // PersonalEmailID column is mapped as GTalkID in VW for HCM mapping.
                    else
                        empContactDetails.PersonalEmailID = model.GtalkId;

                    empContactDetails.CurrentPhone = model.ResidenceNumber.Trim();
                    empContactDetails.MobileNumber = model.MobileNumber.Trim();
                    if (model.AlternateContactNumber != null && model.AlternateContactNumber != "")
                        empContactDetails.AlternateContactNumber = model.AlternateContactNumber.Trim();
                    else
                        empContactDetails.AlternateContactNumber = model.AlternateContactNumber;

                    if (model.SeatingLocation != null && model.SeatingLocation != "")
                        empContactDetails.SeatingLocation = model.SeatingLocation.Trim();
                    else
                        empContactDetails.SeatingLocation = model.SeatingLocation;

                    if (model.OfficeVoip != null && model.OfficeVoip != "")
                        empContactDetails.VoIP = model.OfficeVoip.Trim();
                    else
                        empContactDetails.VoIP = model.OfficeVoip;

                    if (model.ResidenceVoip != null && model.ResidenceVoip != "")
                        empContactDetails.ResidenceVoIP = model.ResidenceVoip.Trim();
                    else
                        empContactDetails.ResidenceVoIP = model.ResidenceVoip;

                    empContactDetails.EmailID1 = model.PersonalEmailId.Trim();
                    if (model.YIMId != null && model.YIMId != "")
                        empContactDetails.YIM = model.YIMId.Trim();
                    else
                        empContactDetails.YIM = model.YIMId;
                }
                dbContext.SaveChanges();
                isAdded = true;
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<tbl_PM_EmployeeEmergencyContact> GetEmployeeEmergencyContactDetails(int employeeId)
        {
            //  List<tbl_PM_EmployeeEmergencyContact> EmployeeContactDetailsList = new List<tbl_PM_EmployeeEmergencyContact>();

            try
            {
                dbContext = new HRMSDBEntities();
                List<tbl_PM_EmployeeEmergencyContact> EmployeeContactDetailsList = (from contacts in dbContext.tbl_PM_EmployeeEmergencyContact
                                                                                    where contacts.EmployeeID == employeeId
                                                                                    select contacts).ToList();
                return EmployeeContactDetailsList.OrderBy(x => x.EmployeeEmergencyContactID).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmergencyContactViewModel> GetEmployeeEmergencyContactDetails(int employeeId, int page, int rows, out int totalCount)
        {
            try
            {
                // List<EmergencyContactViewModel> EmergencyContactList = new List<EmergencyContactViewModel>();// EmergencyContactList
                //relatiship = (from empcontact in dbContext.tbl_PM_EmployeeRelationType
                //                    where empcontact.UniqueID = contact.RelationTypeID
                //                    select RelationType)
                dbContext = new HRMSDBEntities();
                var EmergencyContactList = (from contact in dbContext.tbl_PM_EmployeeEmergencyContact
                                            where contact.EmployeeID == employeeId
                                            orderby contact.EmployeeID descending
                                            select new EmergencyContactViewModel
                                            {
                                                EmployeeId = contact.EmployeeID,
                                                EmployeeEmergencyContactId = contact.EmployeeEmergencyContactID,
                                                Name = contact.Name,
                                                EmailId = contact.EmailID,
                                                ContactNo = contact.ContactNo,
                                                EmgAddress = contact.Address,
                                                Relation = (from relation in dbContext.tbl_PM_EmployeeRelationType
                                                            where relation.UniqueID == contact.RelationTypeID
                                                            select relation.RelationType).FirstOrDefault(),
                                                uniqueID = contact.RelationTypeID,
                                                //Relation = (from empcontact in dbContext.tbl_PM_EmployeeRelationType
                                                //            where empcontact.UniqueID = contact.RelationTypeID
                                                //            select empcontact.RelationType)
                                            }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from contact in dbContext.tbl_PM_EmployeeEmergencyContact
                              where contact.EmployeeID == employeeId
                              select contact.EmployeeID).Count();

                return EmergencyContactList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmergencyContactViewModel> GetRelationList()
        {
            //     public List<tbl_PM_EmployeeRelationType> GetRelation()
            //{
            //    List<tbl_PM_EmployeeRelationType> RelationList = dbContext.tbl_PM_EmployeeRelationType.ToList();
            //    return RelationList;

            //    }
            List<EmergencyContactViewModel> model = new List<EmergencyContactViewModel>();
            try
            {
                model = (from bt in dbContext.tbl_PM_EmployeeRelationType
                         orderby bt.RelationType ascending
                         select new EmergencyContactViewModel
                         {
                             uniqueID = bt.UniqueID,
                             Relation = bt.RelationType
                         }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return model.OrderBy(x => x.Relation).ToList();
        }

        /// <summary>
        /// Method will return the Employee skill details
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<HRMS_tbl_PM_Tools> GetSkillDetails()
        {
            List<HRMS_tbl_PM_Tools> skillDetailsList = new List<HRMS_tbl_PM_Tools>();

            try
            {
                dbContext = new HRMSDBEntities();
                skillDetailsList = (from sd in dbContext.HRMS_tbl_PM_Tools select sd).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return skillDetailsList.OrderBy(x => x.Description).ToList();
        }

        public List<SkillLevelList> GetSkillLevel()
        {
            List<SkillLevelList> skillLevelList = new List<SkillLevelList>();

            try
            {
                dbContext = new HRMSDBEntities();
                skillLevelList = (from sl in dbContext.Tbl_pm_Proficiencymaster
                                  select

                                      new SkillLevelList
                                      {
                                          Text = sl.Description,
                                          Value = sl.ProficiencyId
                                      }).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return skillLevelList.OrderBy(x => x.Text).ToList();
        }

        public string GetParticularSkillLevel(int level)
        {
            string skillLevelList = string.Empty;

            try
            {
                dbContext = new HRMSDBEntities();
                skillLevelList = (from sl in dbContext.Tbl_pm_Proficiencymaster
                                  where sl.ProficiencyId == level
                                  select sl.Description).SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }

            return skillLevelList;
        }

        /// <summary>
        /// Method will give us Employees skill details based on passed employeeId.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<tbl_PM_EmployeeSkillMatrix> GetEmployeeSkillDetails(int employeeId)
        {
            List<tbl_PM_EmployeeSkillMatrix> employeeSkillDetailsList = new List<tbl_PM_EmployeeSkillMatrix>();
            try
            {
                dbContext = new HRMSDBEntities();
                employeeSkillDetailsList = (dbContext.tbl_PM_EmployeeSkillMatrix.Where(ed => ed.EmployeeID == employeeId)).ToList();
            }
            catch (Exception E)
            {
                throw E;
            }
            if (employeeSkillDetailsList.Count == 0)
            {
                employeeSkillDetailsList = null;
                return employeeSkillDetailsList;
            }
            else
            {
                return employeeSkillDetailsList;
            }
        }

        public bool CanSendMail(int EmployeeID)
        {
            List<SkillDetails> finalObj = new List<SkillDetails>();
            dbContext = new HRMSDBEntities();
            var AllDetails = (dbContext.tbl_PM_EmployeeSkillMatrix.Where(ed => ed.EmployeeID == EmployeeID)).ToList();

            if (AllDetails != null)
            {
                foreach (var obj in AllDetails)
                {
                    var histroy = (from his in dbContext.tbl_PM_EmployeeSkillMatrix_History
                                   where his.EmployeeSkillID == obj.EmployeeSkillID
                                   orderby his.EmployeeSkillHistoryID descending
                                   select his).FirstOrDefault();

                    if (obj != null)
                    {
                        if (histroy != null)
                            if (histroy.Status != 2 && histroy.Status != 3)
                            {
                                SkillDetails current = new SkillDetails();
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

        public bool MailSent(int EmployeeID)
        {
            List<SkillDetails> finalObj = new List<SkillDetails>();
            dbContext = new HRMSDBEntities();
            var AllDetails = (dbContext.tbl_PM_EmployeeSkillMatrix.Where(ed => ed.EmployeeID == EmployeeID)).ToList();

            if (AllDetails != null)
            {
                foreach (var obj in AllDetails)
                {
                    var histroy = (from his in dbContext.tbl_PM_EmployeeSkillMatrix_History
                                   where his.EmployeeSkillID == obj.EmployeeSkillID
                                   orderby his.EmployeeSkillHistoryID descending
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

        /// <summary>
        /// Method will give us skill Description Based on Provided skillID
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public string EmployeeSkillDescription(int skillId)
        {
            string description = string.Empty;
            try
            {
                var skilldescription = dbContext.HRMS_tbl_PM_Tools.Where(desc => desc.ToolID == skillId).FirstOrDefault();
                description = skilldescription.Description;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return description;
        }

        /// <summary>
        /// Method will add skill details to database
        /// </summary>
        /// <param name="enteredskillDetails"></param>
        /// <returns></returns>
        public bool AddEmployeeSkillDetails(SkillDetails model, bool IsLoggedInEmployee)
        {
            bool isAdded = false;
            int skill = Convert.ToInt32(model.Skill);
            try
            {
                tbl_PM_EmployeeSkillMatrix Skilldetails = dbContext.tbl_PM_EmployeeSkillMatrix.Where(ed => ed.EmployeeSkillID == model.EmployeeSkillID).FirstOrDefault();
                tbl_PM_EmployeeSkillMatrix Skilldetail = dbContext.tbl_PM_EmployeeSkillMatrix.Where(e => e.ToolID == skill && e.EmployeeID == model.EmployeeID).FirstOrDefault();
                int sLevel = 0;
                if (model.SkillLevel != "")
                {
                    sLevel = Convert.ToInt32(model.SkillLevel);
                }
                else
                {
                    sLevel = 0;
                }

                if (Skilldetails == null)
                {
                    if (Skilldetail == null)
                    {
                        tbl_PM_EmployeeSkillMatrix skillDetails = new tbl_PM_EmployeeSkillMatrix()
                        {
                            EmployeeID = model.EmployeeID,
                            ToolID = int.Parse(model.Skill),
                            EmployeeskillLevel = sLevel,
                            EmployeeSkillID = model.EmployeeSkillID,
                            Proficiency = sLevel
                        };
                        dbContext.tbl_PM_EmployeeSkillMatrix.AddObject(skillDetails);
                        dbContext.SaveChanges();

                        if (IsLoggedInEmployee == true)
                        {
                            tbl_PM_EmployeeSkillMatrix_History Skills = new tbl_PM_EmployeeSkillMatrix_History()
                            {
                                EmployeeID = model.EmployeeID,
                                ToolID = int.Parse(model.Skill),
                                EmployeeskillLevel = sLevel,
                                EmployeeSkillID = skillDetails.EmployeeSkillID,
                                Proficiency = sLevel,
                                ActionType = "Add",
                                CreatedBy = model.EmployeeID.ToString(),
                                CreatedDate = DateTime.Now,
                                SendMail = true
                            };

                            dbContext.tbl_PM_EmployeeSkillMatrix_History.AddObject(Skills);
                            dbContext.SaveChanges();
                        }

                        isAdded = true;
                    }
                    else
                    {
                        isAdded = false;
                    }
                }
                else
                {
                    if (Skilldetail.EmployeeSkillID == null)
                    {
                        isAdded = false;
                    }
                    else
                    {
                        if (Skilldetails.EmployeeID != model.EmployeeID ||
                                     Skilldetails.ToolID != int.Parse(model.Skill) ||
                                     Skilldetails.EmployeeskillLevel != sLevel ||
                                     Skilldetails.EmployeeSkillID != model.EmployeeSkillID ||
                                     Skilldetails.Proficiency != sLevel)
                        {
                            if (IsLoggedInEmployee == true)
                            {
                                tbl_PM_EmployeeSkillMatrix_History Skills = new tbl_PM_EmployeeSkillMatrix_History()
                                {
                                    EmployeeID = Skilldetails.EmployeeID,
                                    ToolID = Skilldetails.ToolID,
                                    EmployeeskillLevel = Skilldetails.EmployeeskillLevel,
                                    EmployeeSkillID = model.EmployeeSkillID,
                                    Proficiency = Skilldetails.Proficiency,
                                    ActionType = "Edit",
                                    ModifiedBy = model.EmployeeID.ToString(),
                                    ModifiedDate = DateTime.Now,
                                    SendMail = true
                                };

                                dbContext.tbl_PM_EmployeeSkillMatrix_History.AddObject(Skills);
                                dbContext.SaveChanges();
                            }

                            Skilldetails.ToolID = int.Parse(model.Skill);
                            Skilldetails.EmployeeskillLevel = sLevel;
                            Skilldetails.Proficiency = sLevel;
                            dbContext.SaveChanges();
                            isAdded = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        public tbl_ApprovalChanges GetChangedFields(int? employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                //string defaultDiscription = "User Name";
                //if (fieldDiscription != "")
                //{
                //    defaultDiscription = fieldDiscription;
                //}
                tbl_ApprovalChanges approvalchanges = (from ac in dbContext.tbl_ApprovalChanges
                                                       where ac.EmployeeID == employeeId
                                                       orderby ac.Id descending
                                                       select ac).FirstOrDefault();
                //dbContext.tbl_ApprovalChanges.Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();

                return approvalchanges;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public IList<List<EmployeeChangeDetails>> GetChangedFieldsList()
        //{
        //    try
        //    {
        //        dbContext = new HRMSDBEntities();
        //        List<EmployeeChangeDetails> approvalchanges = new List<EmployeeChangeDetails>();

        //        IList<List<EmployeeChangeDetails>> totalValues = new List<List<EmployeeChangeDetails>>();

        //        List<int?> employeeIdList = (from e in dbContext.tbl_ApprovalChanges
        //                                    orderby e.EmployeeID
        //                                    select e.EmployeeID).Distinct().ToList();

        //        foreach (var item in employeeIdList)
        //        {
        //            var employeeid = item;

        //            approvalchanges = (from ac in dbContext.tbl_ApprovalChanges
        //                               where ac.EmployeeID == employeeid
        //                               orderby ac.Id descending
        //                               select new EmployeeChangeDetails
        //                               {
        //                                   EmployeeID = ac.EmployeeID,
        //                                   FieldDiscription = ac.FieldDiscription,
        //                                   Module = ac.Module,
        //                                   FieldDbColumnName = ac.FieldDbColumnName,
        //                                   OldValue = ac.OldValue,
        //                                   NewValue = ac.NewValue,
        //                                   CreatedBy = ac.CreatedBy,
        //                                   CreatedDateTime = ac.CreatedDateTime,
        //                                   ApprovalStatusMasterID = ac.ApprovalStatusMasterID,
        //                                   Comments = ac.Comments
        //                               }).ToList();

        //            totalValues.Add(approvalchanges);
        //        }

        //        return totalValues;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public List<EmployeeChangeDetails> SelectedModuleDetailsList(int employeeId, string module, int page, int rows, out int totalCount)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<EmployeeChangeDetails> approvalchanges = new List<EmployeeChangeDetails>();
                DateTime dateOld = DateTime.MinValue;
                DateTime dateNew = DateTime.MinValue;

                if (module == "New Personal Details" || module == "New Residential Details")
                {
                    var Newmodule = module.Replace("New", "").Trim();
                    approvalchanges = (from ac in dbContext.tbl_ApprovalChanges
                                       where ac.EmployeeID == employeeId && ac.Module == Newmodule && (ac.ApprovalStatusMasterID == null)
                                       orderby ac.Id
                                       select new EmployeeChangeDetails
                                       {
                                           ChildEmployeeID = ac.EmployeeID,
                                           ChildFieldDiscription = ac.FieldDiscription,
                                           //ChildModule = ac.Module,
                                           //ChildFieldDbColumnName = ac.FieldDbColumnName,
                                           ChildOldValue = ac.OldValue,
                                           ChildNewValue = ac.NewValue,
                                           ChildNewValueAdmin = ac.NewValue,
                                           //ChildCreatedBy = ac.CreatedBy,
                                           //ChildCreatedDateTime = ac.CreatedDateTime,
                                           ChildApprovalStatusMasterID = ac.ApprovalStatusMasterID,
                                           ChildComments = ac.Comments,
                                           ChildFieldDbColumnName = ac.FieldDbColumnName
                                       }).ToList();
                }
                if (module == "OnHold Personal Details" || module == "OnHold Residential Details")
                {
                    var NewMoule = module.Replace("OnHold", "").Trim();
                    approvalchanges = (from ac in dbContext.tbl_ApprovalChanges
                                       where ac.EmployeeID == employeeId && ac.Module == NewMoule && (ac.ApprovalStatusMasterID == 1)
                                       orderby ac.Id
                                       select new EmployeeChangeDetails
                                       {
                                           ChildEmployeeID = ac.EmployeeID,
                                           ChildFieldDiscription = ac.FieldDiscription,
                                           //ChildModule = ac.Module,
                                           //ChildFieldDbColumnName = ac.FieldDbColumnName,
                                           ChildOldValue = ac.OldValue,
                                           ChildNewValue = ac.NewValue,
                                           ChildNewValueAdmin = ac.NewValue,
                                           //ChildCreatedBy = ac.CreatedBy,
                                           //ChildCreatedDateTime = ac.CreatedDateTime,
                                           ChildApprovalStatusMasterID = ac.ApprovalStatusMasterID,
                                           ChildComments = ac.Comments,
                                           ChildFieldDbColumnName = ac.FieldDbColumnName
                                       }).ToList();
                }

                foreach (var item in approvalchanges)
                {
                    if (DateTime.TryParse(item.ChildOldValue, out dateOld))
                    {
                        if (dateOld == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                            item.ChildOldValue = null;
                        else
                            item.ChildOldValue = dateOld.ToShortDateString();
                    }
                    if (DateTime.TryParse(item.ChildNewValue, out dateNew))
                    {
                        if (dateNew == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                            item.ChildNewValue = null;
                        else
                            item.ChildNewValue = dateNew.ToShortDateString();
                    }
                    if (item.ChildOldValue == null)
                    {
                        item.ChildOldValue = "";
                    }
                    if (item.ChildNewValue == null)
                    {
                        item.ChildNewValue = "";
                    }
                    if (item.ChildNewValueAdmin == null)
                    {
                        item.ChildNewValueAdmin = "";
                    }
                }
                totalCount = approvalchanges.Count();

                return approvalchanges.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SavePersonalDetailsResponse SaveChangedField(EmployeeChangesApprovalViewModel changes)
        {
            SavePersonalDetailsResponse response = new SavePersonalDetailsResponse();
            try
            {
                var empPersonalDetails = dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeID == changes.EmployeeID).SingleOrDefault();
                tbl_ApprovalChanges _tbl_ApprovalChanges = new tbl_ApprovalChanges();
                //tbl_ApprovalChanges oldrow = GetChangedFields(changes.EmployeeID, changes.FieldDiscription);

                //if (oldrow != null)
                //{
                //    if (oldrow.NewValue == changes.OldValue && oldrow.OldValue != changes.NewValue)
                //    {
                //        _tbl_ApprovalChanges.OldValue = oldrow.OldValue;
                //        _tbl_ApprovalChanges.NewValue = changes.NewValue;
                //    }
                //    else
                //    {
                if (empPersonalDetails != null)
                {
                    HRMS_tbl_PM_Employee _tbl_Pm_Employee = new HRMS_tbl_PM_Employee();

                    _tbl_ApprovalChanges.EmployeeID = changes.EmployeeID;
                    _tbl_ApprovalChanges.CreatedBy = changes.CreatedBy;
                    _tbl_ApprovalChanges.CreatedDateTime = changes.CreatedDateTime;
                    if (changes.NewValue != null && changes.NewValue != "")
                        _tbl_ApprovalChanges.NewValue = changes.NewValue.Trim();
                    else
                        _tbl_ApprovalChanges.NewValue = changes.NewValue;
                    if (changes.OldValue != null && changes.OldValue != "")
                        _tbl_ApprovalChanges.OldValue = changes.OldValue.Trim();
                    else
                        _tbl_ApprovalChanges.OldValue = changes.OldValue;
                    _tbl_ApprovalChanges.FieldDiscription = changes.FieldDiscription;
                    _tbl_ApprovalChanges.Module = changes.Module;
                    _tbl_ApprovalChanges.ApprovalStatusMasterID = changes.ApprovalStatusMasterID;
                    _tbl_ApprovalChanges.Comments = changes.Comments;
                    _tbl_ApprovalChanges.FieldDbColumnName = changes.FieldDbColumnName;

                    dbContext.tbl_ApprovalChanges.AddObject(_tbl_ApprovalChanges);
                    dbContext.SaveChanges();

                    columnNameList.Add(changes.FieldDbColumnName);
                    FieldLabelList.Add(changes.FieldDiscription);
                    NewValueList.Add(changes.NewValue);

                    response.NewValues = NewValueList;
                    response.FieldLabels = FieldLabelList;
                    response.ColumnNames = columnNameList;
                    response.EmployeeId = (int)changes.EmployeeID;
                    response.IsAdded = true;
                }
                //}
                // }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public bool CheckUserNameApprovalExist(int employeeId)
        {
            try
            {
                bool IsUserNameExist = false;

                var userNameExist = (from username in dbContext.tbl_ApprovalChanges
                                     where username.EmployeeID == employeeId && username.FieldDiscription == "User Name"
                                     && (username.ApprovalStatusMasterID == null || username.ApprovalStatusMasterID == 1)
                                     orderby username.Id descending
                                     select username).FirstOrDefault();

                if (userNameExist != null)
                {
                    IsUserNameExist = true;
                }

                return IsUserNameExist;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// To add Personal details to database
        /// </summary>
        /// <param name="model"></param>
        public SavePersonalDetailsResponse SavePersonalDetails(PersonalDetailsViewModel model)
        {
            int EmpId = 0;
            SavePersonalDetailsResponse response = new SavePersonalDetailsResponse();
            try
            {
                var empPersonalDetails = dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeID == model.EmployeeId).SingleOrDefault();
                var empHobbies = dbContext.tbl_HR_Hobbies.Where(x => x.EmployeeID == model.EmployeeId).FirstOrDefault();
                var empAchievement = dbContext.tbl_HR_Achievement.Where(x => x.EmployeeID == model.EmployeeId).FirstOrDefault();
                var empSpouseAndChildDetails = (from dependantans in dbContext.tbl_PM_Employee_Dependands
                                                where (dependantans.RelationType == "Wife"
                                                 || dependantans.RelationType == "Husband"
                                                 || dependantans.RelationType == null)
                                                 && dependantans.EmployeeID == model.EmployeeId
                                                select dependantans).ToList();

                if (empPersonalDetails == null && empHobbies == null && empAchievement == null)
                {
                    HRMS_tbl_PM_Employee _tbl_Pm_Employee = new HRMS_tbl_PM_Employee();

                    _tbl_Pm_Employee.Contract_Employee = model.ContractEmployee;
                    _tbl_Pm_Employee.Prefix = model.Prefix;
                    _tbl_Pm_Employee.FirstName = model.FirstName.Trim();

                    if (model.MiddleName != null && model.MiddleName != "")
                        _tbl_Pm_Employee.MiddleName = model.MiddleName.Trim();
                    else
                        _tbl_Pm_Employee.MiddleName = model.MiddleName;

                    _tbl_Pm_Employee.LastName = model.LastName.Trim();
                    _tbl_Pm_Employee.UserName = model.UserName.Trim();
                    _tbl_Pm_Employee.EmailID = model.UserName + "@v2solutions.com";
                    // _tbl_Pm_Employee.EmployeeName = _tbl_Pm_Employee.FirstName + " " + _tbl_Pm_Employee.MiddleName + " " + _tbl_Pm_Employee.LastName;
                    _tbl_Pm_Employee.Gender = model.Gender;
                    _tbl_Pm_Employee.BirthDate = model.BirthDate;
                    _tbl_Pm_Employee.MaritalStatus = model.MaritalStatus;
                    _tbl_Pm_Employee.WeddingDate = model.WeddingDate;
                    if (model.Recognition != null && model.Recognition != "")
                        _tbl_Pm_Employee.Recognition = model.Recognition.Trim();
                    else
                        _tbl_Pm_Employee.Recognition = model.Recognition;

                    _tbl_Pm_Employee.EmployeeCode = model.EmployeeCode;
                    _tbl_Pm_Employee.ProfileImageName = model.ProfileImageName;
                    _tbl_Pm_Employee.ProfileImagePath = model.ProfileImagePath;
                    if (model.Remarks != null && model.Remarks != "")
                        _tbl_Pm_Employee.Remarks = model.Remarks.Trim();
                    else
                        _tbl_Pm_Employee.Remarks = model.Remarks;

                    _tbl_Pm_Employee.Status = Convert.ToBoolean(0);
                    _tbl_Pm_Employee.JoiningCompanyID = 1;
                    _tbl_Pm_Employee.Agreement_Signed_Date = model.AgreementDate;
                    _tbl_Pm_Employee.NoOfChildren = model.NoOfchildren.HasValue ? model.NoOfchildren.Value : 0;
                    if (model.SpouseName != null && model.SpouseName != "")
                        _tbl_Pm_Employee.SpouseName = model.SpouseName.Trim();
                    else
                        _tbl_Pm_Employee.SpouseName = model.SpouseName;

                    _tbl_Pm_Employee.SpouseBirthdate = model.SpouseBirthDate;

                    if (model.Child1Name != null && model.Child1Name != "")
                        _tbl_Pm_Employee.Child1Name = model.Child1Name.Trim();
                    else
                        _tbl_Pm_Employee.Child1Name = model.Child1Name;

                    _tbl_Pm_Employee.Child1Birthdate = model.Child1BirthDate;

                    if (model.Child2Name != null && model.Child2Name != "")
                        _tbl_Pm_Employee.Child2Name = model.Child2Name.Trim();
                    else
                        _tbl_Pm_Employee.Child2Name = model.Child2Name;

                    _tbl_Pm_Employee.Child2Birthdate = model.Child2BirthDate;

                    if (model.Child3Name != null && model.Child3Name != "")
                        _tbl_Pm_Employee.Child3Name = model.Child3Name.Trim();
                    else
                        _tbl_Pm_Employee.Child3Name = model.Child3Name;

                    _tbl_Pm_Employee.Child3Birthdate = model.Child3BirthDate;

                    if (model.Child4Name != null && model.Child4Name != "")
                        _tbl_Pm_Employee.Child4Name = model.Child4Name.Trim();
                    else
                        _tbl_Pm_Employee.Child4Name = model.Child4Name;

                    _tbl_Pm_Employee.Child4Birthdate = model.Child4BirthDate;

                    if (model.Child5Name != null && model.Child5Name != "")
                        _tbl_Pm_Employee.Child5Name = model.Child5Name.Trim();
                    else
                        _tbl_Pm_Employee.Child5Name = model.Child5Name;

                    _tbl_Pm_Employee.Child5Birthdate = model.Child5BirthDate;

                    _tbl_Pm_Employee.CustomFieldNumeric2 = 1;
                    _tbl_Pm_Employee.IsLDAPAuthentication = false;
                    _tbl_Pm_Employee.CreatedDate = DateTime.Now;
                    _tbl_Pm_Employee.ContractFrom = model.ContractFrom;
                    _tbl_Pm_Employee.ContractTo = model.ContractTo;
                    //here cost centerid represents competency Manager Id
                    _tbl_Pm_Employee.ShiftID = model.ShiftId;

                    if (model.MaidanName != null && model.MaidanName != "")
                        _tbl_Pm_Employee.MaidenName = model.MaidanName.Trim();
                    else
                        _tbl_Pm_Employee.MaidenName = model.MaidanName;

                    if (model.EmployeeCode.Length == 3)
                        _tbl_Pm_Employee.EmployeeType = "Contract";
                    else
                    {
                        if (model.EmployeeCode.Length > 3)
                            _tbl_Pm_Employee.EmployeeType = "Regular";
                    }
                    dbContext.HRMS_tbl_PM_Employee.AddObject(_tbl_Pm_Employee);
                    dbContext.SaveChanges();

                    EmpId = _tbl_Pm_Employee.EmployeeID;

                    //for new user default password will  be mail_123
                    string password = "mail_123";

                    Membership.CreateUser(model.EmployeeCode, password);

                    tbl_HR_Hobbies _tbl_HR_Hobbies = new tbl_HR_Hobbies();
                    _tbl_HR_Hobbies.EmployeeID = _tbl_Pm_Employee.EmployeeID;

                    if (model.Hobbies != null && model.Hobbies != "")
                        _tbl_HR_Hobbies.Decription = model.Hobbies.Trim();
                    else
                        _tbl_HR_Hobbies.Decription = model.Hobbies;

                    tbl_HR_Achievement _tbl_HR_Achievement = new tbl_HR_Achievement();
                    _tbl_HR_Achievement.EmployeeID = _tbl_Pm_Employee.EmployeeID;

                    if (model.Achievement != null && model.Achievement != "")
                        _tbl_HR_Achievement.Decription = model.Achievement.Trim();
                    else
                        _tbl_HR_Achievement.Decription = model.Achievement;

                    dbContext.tbl_HR_Hobbies.AddObject(_tbl_HR_Hobbies);
                    dbContext.tbl_HR_Achievement.AddObject(_tbl_HR_Achievement);

                    dbContext.SaveChanges();
                }
                else
                {
                    //string userName = GetUserNameByEmployeeId(model.EmployeeId.HasValue ? model.EmployeeId.Value : 0);
                    empPersonalDetails.EmployeeID = model.EmployeeId.Value;
                    empPersonalDetails.UserName = model.UserName.Trim();
                    empPersonalDetails.EmailID = model.UserName + "@v2solutions.com";
                    empPersonalDetails.Prefix = model.Prefix;

                    empPersonalDetails.FirstName = model.FirstName.Trim();
                    if (model.MiddleName != null && model.MiddleName != "")
                        empPersonalDetails.MiddleName = model.MiddleName.Trim();
                    else
                        empPersonalDetails.MiddleName = model.MiddleName;

                    if (model.SpouseName != null && model.SpouseName != "")
                        empPersonalDetails.SpouseName = model.SpouseName.Trim();
                    else
                        empPersonalDetails.SpouseName = model.SpouseName;

                    empPersonalDetails.SpouseBirthdate = model.SpouseBirthDate;

                    if (model.Child1Name != null && model.Child1Name != "")
                        empPersonalDetails.Child1Name = model.Child1Name.Trim();
                    else
                        empPersonalDetails.Child1Name = model.Child1Name;

                    empPersonalDetails.Child1Birthdate = model.Child1BirthDate;

                    if (model.Child2Name != null && model.Child2Name != "")
                        empPersonalDetails.Child2Name = model.Child2Name.Trim();
                    else
                        empPersonalDetails.Child2Name = model.Child2Name;

                    empPersonalDetails.Child2Birthdate = model.Child2BirthDate;

                    if (model.Child3Name != null && model.Child3Name != "")
                        empPersonalDetails.Child3Name = model.Child3Name.Trim();
                    else
                        empPersonalDetails.Child3Name = model.Child3Name;
                    empPersonalDetails.Child3Birthdate = model.Child3BirthDate;

                    if (model.Child4Name != null && model.Child4Name != "")
                        empPersonalDetails.Child4Name = model.Child4Name.Trim();
                    else
                        empPersonalDetails.Child4Name = model.Child4Name;
                    empPersonalDetails.Child4Birthdate = model.Child4BirthDate;

                    if (model.Child5Name != null && model.Child5Name != "")
                        empPersonalDetails.Child5Name = model.Child5Name.Trim();
                    else
                        empPersonalDetails.Child5Name = model.Child5Name;

                    empPersonalDetails.Child5Birthdate = model.Child5BirthDate;
                    string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                    CommonMethodsDAL Commondal = new CommonMethodsDAL();
                    string user = Commondal.GetMaxRoleForUser(role);
                    if (user == Models.UserRoles.HRAdmin)
                    {
                        empPersonalDetails.EmployeeName = model.FirstName + " " + model.MiddleName + " " + model.LastName;
                    }
                    empPersonalDetails.Contract_Employee = model.ContractEmployee;
                    empPersonalDetails.LastName = model.LastName.Trim();
                    //empPersonalDetails.EmployeeName = empPersonalDetails.FirstName + " " + empPersonalDetails.MiddleName + " " + empPersonalDetails.LastName;
                    if (model.Remarks != null && model.Remarks != "")
                        empPersonalDetails.Remarks = model.Remarks.Trim();
                    else
                        empPersonalDetails.Remarks = model.Remarks;

                    if (model.MaidanName != null && model.MaidanName != "")
                        empPersonalDetails.MaidenName = model.MaidanName.Trim();
                    else
                        empPersonalDetails.MaidenName = model.MaidanName;

                    empPersonalDetails.ContractFrom = model.ContractFrom;
                    empPersonalDetails.ContractTo = model.ContractTo;
                    if (model.ProfileImageName != null && model.ProfileImagePath != null)
                    {
                        empPersonalDetails.ProfileImageName = model.ProfileImageName;
                        empPersonalDetails.ProfileImagePath = model.ProfileImagePath;
                    }

                    empPersonalDetails.MaritalStatus = model.MaritalStatus;
                    empPersonalDetails.Gender = model.Gender;
                    empPersonalDetails.BirthDate = model.BirthDate;
                    if (model.MaritalStatus == "Single")
                    {
                        empPersonalDetails.WeddingDate = null;
                    }
                    else
                    {
                        empPersonalDetails.WeddingDate = model.WeddingDate;
                    }

                    if (model.Recognition != null && model.Recognition != "")
                        empPersonalDetails.Recognition = model.Recognition.Trim();
                    else
                        empPersonalDetails.Recognition = model.Recognition;

                    empPersonalDetails.Agreement_Signed_Date = model.AgreementDate;

                    empPersonalDetails.NoOfChildren = model.NoOfchildren.HasValue ? model.NoOfchildren.Value : 0;
                    if (empHobbies == null)
                    {
                        tbl_HR_Hobbies _tbl_HR_Hobbies = new tbl_HR_Hobbies();
                        _tbl_HR_Hobbies.EmployeeID = model.EmployeeId;

                        if (model.Hobbies != null && model.Hobbies != "")
                            _tbl_HR_Hobbies.Decription = model.Hobbies.Trim();
                        else
                            _tbl_HR_Hobbies.Decription = model.Hobbies;

                        dbContext.tbl_HR_Hobbies.AddObject(_tbl_HR_Hobbies);
                    }
                    else
                    {
                        if (model.Hobbies != null && model.Hobbies != "")
                            empHobbies.Decription = model.Hobbies.Trim();
                        else
                            empHobbies.Decription = model.Hobbies;
                    }

                    if (empAchievement == null)
                    {
                        tbl_HR_Achievement _tbl_HR_Achievement = new tbl_HR_Achievement();

                        _tbl_HR_Achievement.EmployeeID = model.EmployeeId;
                        if (model.Achievement != null && model.Achievement != "")
                            _tbl_HR_Achievement.Decription = model.Achievement.Trim();
                        else
                            _tbl_HR_Achievement.Decription = model.Achievement;

                        dbContext.tbl_HR_Achievement.AddObject(_tbl_HR_Achievement);
                    }
                    else
                    {
                        if (model.Achievement != null && model.Achievement != "")
                            empAchievement.Decription = model.Achievement.Trim();
                        else
                            empAchievement.Decription = model.Achievement;
                    }

                    EmpId = model.EmployeeId.Value;
                }

                dbContext.SaveChanges();
                response.IsAdded = true;
                response.EmployeeId = EmpId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public List<EmployeeCodeList> GetEmployeeCode()
        {
            List<EmployeeCodeList> employeecode = new List<EmployeeCodeList>();
            try
            {
                employeecode = (from emp in dbContext.HRMS_tbl_PM_Employee
                                select new EmployeeCodeList { EmployeeCode = emp.EmployeeCode }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return employeecode;
        }

        /// <summary>
        /// Checkes the provided EmployeeCode in the database and returns Boolean result specifying specific employee code is already exist in the database or not
        /// </summary>
        /// <param name="employeeCode">Need to check with the EmployeeCode field of the Employee table</param>
        /// <returns>Returns boolean(true,false) to indicate employee code is exist in db or not</returns>
        public bool IsEmployeeCodeExist(string employeeCode)
        {
            bool isExist = false;
            try
            {
                isExist = (from employee in dbContext.HRMS_tbl_PM_Employee
                           where employee.EmployeeCode == employeeCode
                           select employee.EmployeeCode).Count() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isExist;
        }

        //Medical History
        public IList<tbl_BloodGroup> GetBloodGroupList()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                IList<tbl_BloodGroup> BloodGroup = (IList<tbl_BloodGroup>)dbContext.tbl_BloodGroup.OrderBy(b => b.BloodGroup_Name).ToList();
                return BloodGroup;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int EmployeeBg(int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                //var bloodGroup = (from bg in  dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == employeeId)  select bg.BloodGroup).FirstOrDefault();
                var bloodGroup = (from bg_id in dbContext.tbl_BloodGroup
                                  join bg in dbContext.HRMS_tbl_PM_Employee
                                  on bg_id.BloodGroup_Name equals bg.BloodGroup
                                  where bg.EmployeeID == employeeId
                                  select bg_id.BloodGroup_Id).FirstOrDefault();
                return bloodGroup;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteMedical_Desc(int medicalDescId, int employeeId)
        {
            try
            {
                bool isDeleted = false;
                tbl_PM_MedicalDescription medical = dbContext.tbl_PM_MedicalDescription.Where(ed => ed.MedicalDesc_Id == medicalDescId && ed.Employee_Id == employeeId).FirstOrDefault();
                if (medical != null && medical.MedicalDesc_Id > 0)
                {
                    dbContext.tbl_PM_MedicalDescription.DeleteObject(medical);
                    dbContext.SaveChanges();
                    isDeleted = true;
                }
                return isDeleted;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MedicalHistoryDetails> GetAllMedicalDesc(int employeeId, int page, int rows, out int totalCount)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<MedicalHistoryDetails> MedicalDesc = (from medicalHistory in dbContext.tbl_PM_MedicalDescription
                                                           where medicalHistory.Employee_Id == employeeId
                                                           orderby medicalHistory.Employee_Id descending
                                                           select new MedicalHistoryDetails
                                                           {
                                                               EmployeeId = medicalHistory.Employee_Id,
                                                               MedicalDescription = medicalHistory.Medical_Description,
                                                               MedicalDescId = medicalHistory.MedicalDesc_Id,
                                                               Year = medicalHistory.Year
                                                           }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from medicalHistory in dbContext.tbl_PM_MedicalDescription
                              where medicalHistory.Employee_Id == employeeId
                              select medicalHistory.MedicalDesc_Id).Count();
                return MedicalDesc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int MedicalDesc_TotalCount()
        {
            int totalCount = 0;
            try
            {
                dbContext = new HRMSDBEntities();
                totalCount = (from medicalDesc in dbContext.tbl_PM_MedicalDescription
                              select medicalDesc.Employee_Id).Count();
            }
            catch (Exception)
            {
                throw;
            }
            return totalCount;
        }

        public tbl_PM_MedicalDescription GetEmployeeMedicalHistory(int MedicalDesc_Id)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PM_MedicalDescription MedicalDesc = dbContext.tbl_PM_MedicalDescription.Where(ed => ed.MedicalDesc_Id == MedicalDesc_Id).FirstOrDefault();
                //var MedicalDesc = (from i in HRMS_tbl_PM_Employee
                //                                         join j in tbl_PM_MedicalDescription
                //                                         on  i.EmployeeID equals j.EmployeeID select(i.EmployeeID,Year,) );

                return MedicalDesc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteMedicalDesc(int id)
        {
            try
            {
                bool isDeleted = false;
                tbl_PM_MedicalDescription details = dbContext.tbl_PM_MedicalDescription.Where(ed => ed.Employee_Id == id).FirstOrDefault();
                if (details != null && details.Employee_Id > 0)
                {
                    dbContext.DeleteObject(details);
                    dbContext.SaveChanges();
                    isDeleted = true;
                }
                return isDeleted;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddEmployeeMedicalHistory(tbl_PM_MedicalDescription medicalDesc)
        {
            try
            {
                bool isAdded = false;
                tbl_PM_MedicalDescription medical = dbContext.tbl_PM_MedicalDescription.Where(ed => ed.MedicalDesc_Id == medicalDesc.MedicalDesc_Id).FirstOrDefault();
                if (medical == null || medical.MedicalDesc_Id <= 0)
                {
                    dbContext.tbl_PM_MedicalDescription.AddObject(medicalDesc);
                    dbContext.SaveChanges();
                }
                else
                {
                    medical.Employee_Id = medicalDesc.Employee_Id;
                    medical.MedicalDesc_Id = medicalDesc.MedicalDesc_Id;
                    medical.Medical_Description = medicalDesc.Medical_Description.Trim();
                    medical.Year = medicalDesc.Year;
                    dbContext.SaveChanges();
                }
                isAdded = true;
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddEmployeeEmergencyContact(EmergencyContactViewModel emergencyDetails, int? RelationId, int? EmployeeID)
        {
            try
            {
                bool isAdded = false;
                tbl_PM_EmployeeEmergencyContact emp = dbContext.tbl_PM_EmployeeEmergencyContact.Where(ed => ed.EmployeeEmergencyContactID == emergencyDetails.EmployeeEmergencyContactId).FirstOrDefault();
                if (emp == null || emp.EmployeeEmergencyContactID <= 0)
                {
                    tbl_PM_EmployeeEmergencyContact emergencyContact = new tbl_PM_EmployeeEmergencyContact();

                    emergencyContact.EmployeeID = EmployeeID;
                    emergencyContact.EmployeeEmergencyContactID = emergencyDetails.EmployeeEmergencyContactId;
                    emergencyContact.Name = emergencyDetails.Name.Trim();
                    if (emergencyDetails.EmgAddress != null && emergencyDetails.EmgAddress != "")
                        emergencyContact.Address = emergencyDetails.EmgAddress.Trim();
                    else
                        emergencyContact.Address = emergencyDetails.EmgAddress;

                    if (emergencyDetails.EmailId != null && emergencyDetails.EmailId != "")
                        emergencyContact.EmailID = emergencyDetails.EmailId.Trim();
                    else
                        emergencyContact.EmailID = emergencyDetails.EmailId;

                    emergencyContact.ContactNo = emergencyDetails.ContactNo.Trim();
                    emergencyContact.RelationTypeID = RelationId;
                    dbContext.tbl_PM_EmployeeEmergencyContact.AddObject(emergencyContact);
                    dbContext.SaveChanges();
                }
                else
                {
                    emp.EmployeeID = EmployeeID;
                    emp.EmployeeEmergencyContactID = emergencyDetails.EmployeeEmergencyContactId;
                    emp.Name = emergencyDetails.Name.Trim();
                    if (emergencyDetails.EmgAddress != null && emergencyDetails.EmgAddress != "")
                        emp.Address = emergencyDetails.EmgAddress.Trim();
                    else
                        emp.Address = emergencyDetails.EmgAddress;

                    if (emergencyDetails.EmailId != null && emergencyDetails.EmailId != "")
                        emp.EmailID = emergencyDetails.EmailId.Trim();
                    else
                        emp.EmailID = emergencyDetails.EmailId;
                    emp.ContactNo = emergencyDetails.ContactNo.Trim();
                    emp.RelationTypeID = RelationId;
                    dbContext.SaveChanges();
                }
                isAdded = true;
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteEmployeeEmergencyContact(int employeeEmergencyContactId, int employeeId)
        {
            try
            {
                bool isDeleted = false;
                tbl_PM_EmployeeEmergencyContact emergencyContactdlt = dbContext.tbl_PM_EmployeeEmergencyContact.Where(ed => ed.EmployeeEmergencyContactID == employeeEmergencyContactId && ed.EmployeeID == employeeId).FirstOrDefault();
                if (emergencyContactdlt != null && emergencyContactdlt.EmployeeEmergencyContactID > 0)
                {
                    dbContext.tbl_PM_EmployeeEmergencyContact.DeleteObject(emergencyContactdlt);
                    dbContext.SaveChanges();
                    isDeleted = true;
                }
                return isDeleted;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<tbl_PM_Employee_Dependands> GetDependantsDetails(int employeeId)
        {
            List<tbl_PM_Employee_Dependands> DependantsDetailsList = new List<tbl_PM_Employee_Dependands>();

            try
            {
                dbContext = new HRMSDBEntities();
                DependantsDetailsList = (from dependantans in dbContext.tbl_PM_Employee_Dependands
                                         where dependantans.EmployeeID == employeeId
                                         select dependantans).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return DependantsDetailsList.OrderBy(x => x.DependandsID).ToList();
        }

        public List<DependantDetails> GetDependantsDetails(int employeeId, int page, int rows, out int totalCount)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<DependantDetails> empSpouseAndChildDetails = (from dependantans in dbContext.tbl_PM_Employee_Dependands
                                                                   join relate in dbContext.tbl_PM_EmployeeRelationType
                                                                   on dependantans.RelationType equals relate.RelationType
                                                                   where dependantans.EmployeeID == employeeId

                                                                   orderby dependantans.EmployeeID descending
                                                                   select new DependantDetails
                                                                   {
                                                                       EmployeeId = dependantans.EmployeeID,
                                                                       DependandsId = dependantans.DependandsID,
                                                                       DependandsName = dependantans.Name,
                                                                       uniqueID = relate.UniqueID,
                                                                       DependandsRelation = dependantans.RelationType,
                                                                       DependandsBirthDate = dependantans.BirthDate,
                                                                       DependandsAge = dependantans.Age
                                                                   }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from dependantans in dbContext.tbl_PM_Employee_Dependands
                              where dependantans.EmployeeID == employeeId
                              select dependantans.EmployeeID).Count();

                return empSpouseAndChildDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // code added by Jitu-----Start

        public List<tbl_PM_Employee_Declarations> GetDeclarationsDetails(int employeeId)
        {
            List<tbl_PM_Employee_Declarations> DeclarationDetailsList = new List<tbl_PM_Employee_Declarations>();

            try
            {
                dbContext = new HRMSDBEntities();
                DeclarationDetailsList = (from decleration in dbContext.tbl_PM_Employee_Declarations
                                          where decleration.EmployeeID == employeeId
                                          select decleration).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return DeclarationDetailsList.OrderBy(x => x.DeclarationId).ToList();
        }

        public List<DeclarationsDetails> GetDeclarationsDetails(int employeeId, int page, int rows, out int totalCount)
        {
            try
            {
                List<EpmType> addEmptype = new List<EpmType>
               {
                   new EpmType   { EmpTypeIds=1,  EmpTypeNames="Active"},
                   new EpmType{ EmpTypeIds=2,EmpTypeNames="Inactive"}
               };

                dbContext = new HRMSDBEntities();
                List<DeclarationsDetails> empDeclarationDetails = (from declaration in dbContext.tbl_PM_Employee_Declarations.ToList()
                                                                   join emp in addEmptype on declaration.V2EmployeeName equals emp.EmpTypeNames
                                                                   join relate in dbContext.tbl_PM_EmployeeRelationType.ToList() on declaration.RelationshipName equals relate.RelationType
                                                                   where declaration.EmployeeID == employeeId
                                                                   orderby declaration.EmployeeID descending
                                                                   select new DeclarationsDetails
                                                                   {
                                                                       EmployeeID = declaration.EmployeeID.HasValue ? declaration.EmployeeID.Value : 0,
                                                                       Name = declaration.Name,
                                                                       V2EmployeeID = emp.EmpTypeIds,
                                                                       V2EmployeeName = declaration.V2EmployeeName,
                                                                       EmployeeCode = Convert.ToInt32(declaration.EmployeeCode),
                                                                       uniqueID = relate.UniqueID,
                                                                       RelationshipName = declaration.RelationshipName,
                                                                       DeclarationId = declaration.DeclarationId
                                                                   }).Skip((page - 1) * rows).Take(rows).ToList();
                totalCount = (from declaration in dbContext.tbl_PM_Employee_Declarations
                              where declaration.EmployeeID == employeeId
                              select declaration.EmployeeID).Count();

                return empDeclarationDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveDeclarationDetails(DeclarationDetailsViewModel empDeclaration, int? RelationId, int? StatusID, int? EmployeeId)
        {
            bool isAdded = false;

            tbl_PM_Employee_Declarations emp = dbContext.tbl_PM_Employee_Declarations.Where(ed => ed.DeclarationId == empDeclaration.DeclarationId).FirstOrDefault();
            if (emp == null || emp.DeclarationId <= 0)
            {
                tbl_PM_Employee_Declarations declaration = new tbl_PM_Employee_Declarations();
                declaration.EmployeeID = EmployeeId;
                declaration.DeclarationId = Convert.ToInt32(empDeclaration.DeclarationId);
                declaration.Name = empDeclaration.Name;
                declaration.EmployeeCode = Convert.ToInt32(empDeclaration.EmployeeCode);
                declaration.RelationshipName = (from relation in dbContext.tbl_PM_EmployeeRelationType
                                                where relation.UniqueID == RelationId
                                                select relation.RelationType).FirstOrDefault();
                if (StatusID == 2)
                {
                    declaration.V2EmployeeName = "Inactive";
                }
                else declaration.V2EmployeeName = "Active";

                dbContext.tbl_PM_Employee_Declarations.AddObject(declaration);
            }
            else
            {
                emp.EmployeeID = EmployeeId;
                emp.DeclarationId = Convert.ToInt32(empDeclaration.DeclarationId);
                emp.Name = empDeclaration.Name;
                emp.EmployeeCode = Convert.ToInt32(empDeclaration.EmployeeCode);

                emp.RelationshipName = (from relation in dbContext.tbl_PM_EmployeeRelationType
                                        where relation.UniqueID == RelationId
                                        select relation.RelationType).FirstOrDefault();

                if (StatusID == 2)
                {
                    emp.V2EmployeeName = "Inactive";
                }
                else emp.V2EmployeeName = "Active";
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool DeleteDeclarationDetails(int DeclarationID, int employeeId)
        {
            bool isDeleted = false;
            tbl_PM_Employee_Declarations declarationID = dbContext.tbl_PM_Employee_Declarations.Where(cd => cd.DeclarationId == DeclarationID && cd.EmployeeID == employeeId).FirstOrDefault();
            if (declarationID != null)
            {
                dbContext.DeleteObject(declarationID);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public List<HRMS_tbl_PM_Employee> GetActive_InActive()
        {
            List<HRMS_tbl_PM_Employee> EmployeeList = dbContext.HRMS_tbl_PM_Employee.ToList();
            return EmployeeList;
        }

        // --End----

        public List<EmployeeChangesApprovalViewModel> GetEmployeeChangeDetails(int page, int rows, string searchText, string Module, int LoggedInEmployeeId, out int totalCount)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<EmployeeChangesApprovalViewModel> finalempChangeDetails = new List<EmployeeChangesApprovalViewModel>();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                string user = Commondal.GetMaxRoleForUser(role);
                if (user == Models.UserRoles.HRAdmin)
                {
                    if (Module == "NewApproval")
                    {
                        List<EmployeeChangesApprovalViewModel> empChangeDetails = (from changes in dbContext.tbl_ApprovalChanges
                                                                                   where (changes.CreatedBy.Contains(searchText) || changes.Module.Contains(searchText)) && (changes.ApprovalStatusMasterID == null) && (changes.EmployeeID != LoggedInEmployeeId)
                                                                                   orderby changes.EmployeeID
                                                                                   group changes by new { changes.EmployeeID, changes.Module, changes.CreatedBy } into grp
                                                                                   select new EmployeeChangesApprovalViewModel
                                                                                   {
                                                                                       EmployeeID = grp.Key.EmployeeID,
                                                                                       Module = grp.Key.Module,
                                                                                       CreatedBy = grp.Key.CreatedBy
                                                                                   }).ToList();

                        List<EmployeeChangesApprovalViewModel> empQualDetails = (from emp in dbContext.HRMS_tbl_PM_Employee
                                                                                 join qual in dbContext.tbl_PM_EmployeeQualificationMatrix_History on emp.EmployeeID equals qual.EmployeeID
                                                                                 where ((qual.Status == null) && (emp.EmployeeName.Contains(searchText) || searchText.Contains("Qualification Details")) && (qual.EmployeeID != LoggedInEmployeeId))
                                                                                 orderby qual.EmployeeID
                                                                                 group qual by new { qual.EmployeeID, emp.EmployeeName } into grp
                                                                                 select new EmployeeChangesApprovalViewModel
                                                                                 {
                                                                                     EmployeeID = grp.Key.EmployeeID,
                                                                                     Module = "Qualification Details",
                                                                                     CreatedBy = grp.Key.EmployeeName,
                                                                                 }).ToList();

                        List<EmployeeChangesApprovalViewModel> certificationDetails = (from emp in dbContext.HRMS_tbl_PM_Employee
                                                                                       join matrixHistory in dbContext.tbl_PM_EmployeeCertificationMatrixHistory on emp.EmployeeID equals matrixHistory.EmployeeID
                                                                                       where ((matrixHistory.Status == null) && (emp.EmployeeName.Contains(searchText) || searchText.Contains("Certification Details")) && (matrixHistory.EmployeeID != LoggedInEmployeeId))
                                                                                       orderby matrixHistory.EmployeeID
                                                                                       group matrixHistory by new { matrixHistory.EmployeeID, emp.EmployeeName } into grp
                                                                                       select new EmployeeChangesApprovalViewModel
                                                                                       {
                                                                                           EmployeeID = grp.Key.EmployeeID,
                                                                                           Module = "Certification Details",
                                                                                           CreatedBy = grp.Key.EmployeeName,
                                                                                       }).ToList();

                        finalempChangeDetails = empChangeDetails.Union(empQualDetails).Union(certificationDetails).ToList();
                    }
                    else
                    {
                        List<EmployeeChangesApprovalViewModel> empChangeDetails = (from changes in dbContext.tbl_ApprovalChanges
                                                                                   where (changes.CreatedBy.Contains(searchText) || changes.Module.Contains(searchText)) && (changes.ApprovalStatusMasterID == 1) && (changes.EmployeeID != LoggedInEmployeeId)
                                                                                   orderby changes.EmployeeID
                                                                                   group changes by new { changes.EmployeeID, changes.Module, changes.CreatedBy } into grp
                                                                                   select new EmployeeChangesApprovalViewModel
                                                                                   {
                                                                                       EmployeeID = grp.Key.EmployeeID,
                                                                                       Module = grp.Key.Module,
                                                                                       CreatedBy = grp.Key.CreatedBy
                                                                                   }).ToList();

                        List<EmployeeChangesApprovalViewModel> empQualDetails = (from emp in dbContext.HRMS_tbl_PM_Employee
                                                                                 join qual in dbContext.tbl_PM_EmployeeQualificationMatrix_History on emp.EmployeeID equals qual.EmployeeID
                                                                                 where ((qual.Status == 1) && (emp.EmployeeName.Contains(searchText) || searchText.Contains("Qualification Details")) && qual.EmployeeID != LoggedInEmployeeId)
                                                                                 orderby qual.EmployeeID
                                                                                 group qual by new { qual.EmployeeID, emp.EmployeeName } into grp
                                                                                 select new EmployeeChangesApprovalViewModel
                                                                                 {
                                                                                     EmployeeID = grp.Key.EmployeeID,
                                                                                     Module = "Qualification Details",
                                                                                     CreatedBy = grp.Key.EmployeeName,
                                                                                 }).ToList();

                        List<EmployeeChangesApprovalViewModel> certificationDetails = (from emp in dbContext.HRMS_tbl_PM_Employee
                                                                                       join matrixHistory in dbContext.tbl_PM_EmployeeCertificationMatrixHistory on emp.EmployeeID equals matrixHistory.EmployeeID
                                                                                       where ((matrixHistory.Status == 1) && (emp.EmployeeName.Contains(searchText) || searchText.Contains("Certification Details")) && matrixHistory.EmployeeID != LoggedInEmployeeId)
                                                                                       orderby matrixHistory.EmployeeID
                                                                                       group matrixHistory by new { matrixHistory.EmployeeID, emp.EmployeeName } into grp
                                                                                       select new EmployeeChangesApprovalViewModel
                                                                                       {
                                                                                           EmployeeID = grp.Key.EmployeeID,
                                                                                           Module = "Certification Details",
                                                                                           CreatedBy = grp.Key.EmployeeName,
                                                                                       }).ToList();

                        finalempChangeDetails = empChangeDetails.Union(empQualDetails).Union(certificationDetails).ToList();
                    }
                }
                else if (user == Models.UserRoles.RMG)
                {
                    if (Module == "NewApproval")
                    {
                        List<EmployeeChangesApprovalViewModel> empSkillDetails = (from emp in dbContext.HRMS_tbl_PM_Employee
                                                                                  join qual in dbContext.tbl_PM_EmployeeSkillMatrix_History on emp.EmployeeID equals qual.EmployeeID
                                                                                  where ((qual.Status == null) && (emp.EmployeeName.Contains(searchText) || searchText.Contains("Skill Details")))
                                                                                  orderby qual.EmployeeID
                                                                                  group qual by new { qual.EmployeeID, emp.EmployeeName } into grp
                                                                                  select new EmployeeChangesApprovalViewModel
                                                                                  {
                                                                                      EmployeeID = grp.Key.EmployeeID,
                                                                                      Module = "Skill Details",
                                                                                      CreatedBy = grp.Key.EmployeeName,
                                                                                  }).ToList();
                        finalempChangeDetails = empSkillDetails;
                    }
                    else
                    {
                        List<EmployeeChangesApprovalViewModel> empSkillDetails = (from emp in dbContext.HRMS_tbl_PM_Employee
                                                                                  join qual in dbContext.tbl_PM_EmployeeSkillMatrix_History on emp.EmployeeID equals qual.EmployeeID
                                                                                  where ((qual.Status == 1) && (emp.EmployeeName.Contains(searchText) || searchText.Contains("Skill Details")))
                                                                                  orderby qual.EmployeeID
                                                                                  group qual by new { qual.EmployeeID, emp.EmployeeName } into grp
                                                                                  select new EmployeeChangesApprovalViewModel
                                                                                  {
                                                                                      EmployeeID = grp.Key.EmployeeID,
                                                                                      Module = "Skill Details",
                                                                                      CreatedBy = grp.Key.EmployeeName,
                                                                                  }).ToList();
                        finalempChangeDetails = empSkillDetails;
                    }
                }

                totalCount = finalempChangeDetails.Count();

                return finalempChangeDetails.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmployeeChangesApprovalViewModel> GetEmployeeChangeDetailsView(int employeeId, int page, int rows, out int totalCount)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<EmployeeChangesApprovalViewModel> empChangeDetails = (from changes in dbContext.tbl_ApprovalChanges
                                                                           where changes.EmployeeID == employeeId
                                                                           orderby changes.FieldDiscription

                                                                           select new EmployeeChangesApprovalViewModel
                                                                           {
                                                                               EmployeeID = changes.EmployeeID,
                                                                               FieldDiscription = changes.FieldDiscription,
                                                                               //Module = grp.Key.Module,
                                                                               // FieldDbColumnName = changes.FieldDbColumnName,
                                                                               OldValue = changes.OldValue,
                                                                               NewValue = changes.NewValue,
                                                                               ApprovalStatusMasterID = changes.ApprovalStatusMasterID,
                                                                               Comments = changes.Comments,
                                                                               //CreatedBy = grp.Key.CreatedBy,
                                                                               //CreatedDateTime = changes.CreatedDateTime
                                                                           }).ToList();
                totalCount = empChangeDetails.Count();

                return empChangeDetails.Skip((page - 1) * rows).Take(rows).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public List<ApprovalStatusListDetails> GetApprovalStatusList()
        //{
        //    List<ApprovalStatusListDetails> statuslist = new List<ApprovalStatusListDetails>();
        //    try
        //    {
        //        statuslist = (from s in dbContext.tbl_ApprovalStatusMaster
        //                      orderby s.ApprovalStatusId
        //                      select new ApprovalStatusListDetails
        //                      {
        //                          ApprovalStatusID = s.ApprovalStatusId,
        //                          ApprovalStatus = s.ApprovalStatus
        //                      }).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return statuslist;
        //}

        public List<ApprovalEmployeeDetails> SearchEmployeeNameModule(string searchText, int pageNo = 1, int pageSize = 10)
        {
            List<ApprovalEmployeeDetails> employeeDetails = new List<ApprovalEmployeeDetails>();
            try
            {
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                string user = Commondal.GetMaxRoleForUser(role);
                if (user == Models.UserRoles.HRAdmin)
                {
                    employeeDetails = (from employee in dbContext.tbl_ApprovalChanges
                                       where (employee.CreatedBy.Contains(searchText) || employee.Module.Contains(searchText)) && (employee.ApprovalStatusMasterID == null || employee.ApprovalStatusMasterID == 1)
                                       orderby employee.CreatedBy
                                       group employee by new { employee.EmployeeID, employee.Module, employee.CreatedBy } into grp
                                       select new ApprovalEmployeeDetails
                                       {
                                           EmployeeId = grp.Key.EmployeeID,
                                           CreatedBy = grp.Key.CreatedBy,
                                           Module = grp.Key.Module
                                       }).ToList();

                    List<ApprovalEmployeeDetails> qualificationDetails = (from q in dbContext.tbl_PM_EmployeeQualificationMatrix_History
                                                                          join e in dbContext.HRMS_tbl_PM_Employee on q.EmployeeID equals e.EmployeeID
                                                                          where q.Status == null &&
                                                                          (e.EmployeeName.Contains(searchText) || "Qualification Details".Contains(searchText))
                                                                          orderby e.EmployeeName
                                                                          group e by new { e.EmployeeID, e.EmployeeName } into grp
                                                                          select new ApprovalEmployeeDetails
                                                                          {
                                                                              EmployeeId = grp.Key.EmployeeID,
                                                                              CreatedBy = grp.Key.EmployeeName,
                                                                              Module = "Qualification Details"
                                                                          }).ToList();

                    List<ApprovalEmployeeDetails> certificationDetails = (from q in dbContext.tbl_PM_EmployeeCertificationMatrixHistory
                                                                          join e in dbContext.HRMS_tbl_PM_Employee on q.EmployeeID equals e.EmployeeID
                                                                          where q.Status == null &&
                                                                          (e.EmployeeName.Contains(searchText) || "Certification Details".Contains(searchText))
                                                                          orderby e.EmployeeName
                                                                          group e by new { e.EmployeeID, e.EmployeeName } into grp
                                                                          select new ApprovalEmployeeDetails
                                                                          {
                                                                              EmployeeId = grp.Key.EmployeeID,
                                                                              CreatedBy = grp.Key.EmployeeName,
                                                                              Module = "Certification Details"
                                                                          }).ToList();
                    employeeDetails = employeeDetails.Union(qualificationDetails).Union(certificationDetails).ToList();
                }
                else if (user == Models.UserRoles.RMG)
                {
                    List<ApprovalEmployeeDetails> skillDetails = (from q in dbContext.tbl_PM_EmployeeSkillMatrix_History
                                                                  join e in dbContext.HRMS_tbl_PM_Employee on q.EmployeeID equals e.EmployeeID
                                                                  where q.Status == null &&
                                                                  (e.EmployeeName.Contains(searchText) || "Skill Details".Contains(searchText))
                                                                  orderby e.EmployeeName
                                                                  group e by new { e.EmployeeID, e.EmployeeName } into grp
                                                                  select new ApprovalEmployeeDetails
                                                                  {
                                                                      EmployeeId = grp.Key.EmployeeID,
                                                                      CreatedBy = grp.Key.EmployeeName,
                                                                      Module = "Skill Details"
                                                                  }).ToList();

                    employeeDetails = skillDetails.ToList();
                }
                return employeeDetails.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SavedependantDetails(DependentDetailsViewModel empDependant, int? RelationId, int? EmployeeID)
        {
            bool isAdded = false;

            tbl_PM_Employee_Dependands emp = dbContext.tbl_PM_Employee_Dependands.Where(ed => ed.DependandsID == empDependant.DependandsId).FirstOrDefault();
            if (emp == null || emp.DependandsID <= 0)
            {
                tbl_PM_Employee_Dependands dependant = new tbl_PM_Employee_Dependands();
                dependant.EmployeeID = EmployeeID;
                dependant.DependandsID = Convert.ToInt32(empDependant.DependandsId);
                dependant.Name = empDependant.DependandsName.Trim();
                dependant.BirthDate = empDependant.DependandsBirthDate;
                dependant.Age = empDependant.DependandsAge;
                dependant.RelationType = (from relation in dbContext.tbl_PM_EmployeeRelationType
                                          where relation.UniqueID == RelationId
                                          select relation.RelationType).FirstOrDefault();

                dbContext.tbl_PM_Employee_Dependands.AddObject(dependant);
            }
            else
            {
                emp.EmployeeID = EmployeeID;
                emp.DependandsID = Convert.ToInt32(empDependant.DependandsId);
                emp.Name = empDependant.DependandsName.Trim();
                emp.BirthDate = empDependant.DependandsBirthDate;
                emp.Age = empDependant.DependandsAge;
                emp.RelationType = (from relation in dbContext.tbl_PM_EmployeeRelationType
                                    where relation.UniqueID == RelationId
                                    select relation.RelationType).FirstOrDefault();
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool DeletedependantDetails(int DependantID, int employeeId)
        {
            bool isDeleted = false;
            tbl_PM_Employee_Dependands dependantID = dbContext.tbl_PM_Employee_Dependands.Where(cd => cd.DependandsID == DependantID && cd.EmployeeID == employeeId).FirstOrDefault();
            if (DependantID != null && DependantID > 0)
            {
                dbContext.DeleteObject(dependantID);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public List<tbl_HR_Years> GetYearList()                                                        // Added Year Drop Down List
        {
            var empList = dbContext.tbl_HR_Years.ToList();
            return empList;
        }

        public List<tbl_PM_EmployeeRelationType> GetRelation()
        {
            List<tbl_PM_EmployeeRelationType> RelationList = dbContext.tbl_PM_EmployeeRelationType.OrderBy(r => r.RelationType).ToList();
            return RelationList;
        }

        /// <summary>
        /// Method will delete the skill details from database of respective employeeskillId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteSkillDetails(int employeeskillId, int employeeId)
        {
            bool isDeleted = false;
            tbl_PM_EmployeeSkillMatrix details = dbContext.tbl_PM_EmployeeSkillMatrix.Where(ed => ed.EmployeeSkillID == employeeskillId && ed.EmployeeID == employeeId).FirstOrDefault();
            if (details != null && details.EmployeeID > 0)
            {
                dbContext.DeleteObject(details);
                dbContext.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        /// <summary>
        /// To Populate the dropdown with preselected skill details while editing.
        /// </summary>
        /// <param name="employeeSkillId"></param>
        /// <returns></returns>
        public tbl_PM_EmployeeSkillMatrix GetSkillIdFromEmployeeSkillId(int employeeSkillId)
        {
            tbl_PM_EmployeeSkillMatrix employeeSkillDetails = new tbl_PM_EmployeeSkillMatrix();
            try
            {
                dbContext = new HRMSDBEntities();
                employeeSkillDetails = dbContext.tbl_PM_EmployeeSkillMatrix.Where(ed => ed.EmployeeSkillID == employeeSkillId).SingleOrDefault();
            }
            catch (Exception E)
            {
                throw E;
            }
            return employeeSkillDetails;
        }

        /// <summary>
        /// Method to update the Employee Skill details.
        /// </summary>
        /// <param name="enteredskillDetails"></param>
        /// <returns></returns>
        public bool EditEmpployeeSkillDetails(tbl_PM_EmployeeSkillMatrix enteredskillDetails)
        {
            bool isAdded = false;
            try
            {
                tbl_PM_EmployeeSkillMatrix updatingSkilldetails = dbContext.tbl_PM_EmployeeSkillMatrix.Where(ed => ed.EmployeeSkillID == enteredskillDetails.EmployeeSkillID).FirstOrDefault();
                if (updatingSkilldetails == null && updatingSkilldetails.EmployeeID <= 0)
                {
                    dbContext.tbl_PM_EmployeeSkillMatrix.AddObject(updatingSkilldetails);
                    dbContext.SaveChanges();
                }
                else
                {
                    updatingSkilldetails.ToolID = enteredskillDetails.ToolID;
                    updatingSkilldetails.EmployeeskillLevel = enteredskillDetails.EmployeeskillLevel;
                    dbContext.SaveChanges();
                }
                isAdded = true;
            }
            catch (Exception)
            {
                throw;
            }
            return isAdded;
        }

        public bool CheckUserName(string userName)
        {
            bool isExists = false;

            var emp = dbContext.HRMS_tbl_PM_Employee.Where(x => x.UserName == userName).FirstOrDefault();

            if (emp == null)
            {
                return isExists;
            }
            else
            {
                isExists = true;
                return isExists;
            }
        }

        public string GetEmployeeUserName(int employeeId)
        {
            string UserName = string.Empty;
            try
            {
                dbContext = new HRMSDBEntities();
                var username = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();
                UserName = username.UserName;
            }
            catch (Exception)
            {
                throw;
            }
            return UserName;
        }

        public int GetEmployeeStatusMasterID(int EmployeeID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                var employeeStatus = dbContext.HRMS_tbl_PM_Employee.Where(e => e.EmployeeID == EmployeeID).FirstOrDefault();
                if (employeeStatus != null)
                {
                    int EmployeeStatusMasterID = employeeStatus.EmployeeStatusMasterID.HasValue ? employeeStatus.EmployeeStatusMasterID.Value : 0;
                    return EmployeeStatusMasterID;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                throw;
            }
        }

        public tbl_ApprovalChanges getEmployeeApprovalDetails(int? employeeId, string fieldDiscription)
        {
            try
            {
                tbl_ApprovalChanges employeeDetails = (from r in dbContext.tbl_ApprovalChanges
                                                       where r.EmployeeID == employeeId && r.FieldDiscription == fieldDiscription
                                                       select r).FirstOrDefault();
                return employeeDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string getEmployeeCode(int employeeID)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                var employeeStatus = dbContext.HRMS_tbl_PM_Employee.Where(e => e.EmployeeID == employeeID).FirstOrDefault();
                if (employeeStatus != null)
                {
                    return employeeStatus.EmployeeCode;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public bool saveApprovalStatus(EmployeeChangesApprovalViewModel model)
        {
            try
            {
                bool isAdded = false;
                int? employeeId = 0;
                string fieldLabel = string.Empty;
                string oldvalue = string.Empty;
                string newvalue = string.Empty;
                string columnName = string.Empty;
                HRMS_tbl_PM_Employee _PM_Employee = new HRMS_tbl_PM_Employee();

                foreach (var item in model.ChangeDetailsList.ToList())
                {
                    employeeId = item.ChildEmployeeID;
                    fieldLabel = item.ChildFieldDiscription;
                    oldvalue = item.ChildOldValue;
                    if (fieldLabel == "User Name")
                    {
                        newvalue = item.ChildNewValueAdmin;
                    }
                    else
                    {
                        newvalue = item.ChildNewValue;
                    }

                    columnName = item.ChildFieldDbColumnName;
                    tbl_ApprovalChanges employeeDetails = new tbl_ApprovalChanges();

                    employeeDetails = (from r in dbContext.tbl_ApprovalChanges
                                       where (r.EmployeeID == employeeId && r.FieldDiscription == fieldLabel)
                                       orderby r.CreatedDateTime descending
                                       select r).FirstOrDefault();

                    _PM_Employee = (from emp in dbContext.HRMS_tbl_PM_Employee
                                    where emp.EmployeeID == employeeId
                                    select emp).FirstOrDefault();

                    employeeDetails.ApprovalStatusMasterID = item.ChildApprovalStatusMasterID;
                    employeeDetails.Comments = model.Comments;

                    if (item.ChildApprovalStatusMasterID == 2)
                    {
                        employeeDetails.ApprovedDateTime = DateTime.Now;
                    }
                    if (item.ChildApprovalStatusMasterID == 3)
                    {
                        employeeDetails.RejectedDateTime = DateTime.Now;
                    }
                    if (fieldLabel == "User Name" && item.ChildApprovalStatusMasterID != 3)
                    {
                        employeeDetails.NewValue = newvalue;
                        _PM_Employee.UserName = newvalue;
                        _PM_Employee.EmailID = _PM_Employee.UserName + "@v2solutions.com";
                    }
                }
                dbContext.SaveChanges();

                foreach (var item in model.ChangeDetailsList.ToList())
                {
                    employeeId = item.ChildEmployeeID;
                    fieldLabel = item.ChildFieldDiscription;
                    oldvalue = item.ChildOldValue;
                    if (fieldLabel == "User Name")
                    {
                        newvalue = item.ChildNewValueAdmin;
                    }
                    else
                    {
                        newvalue = item.ChildNewValue;
                    }
                    columnName = item.ChildFieldDbColumnName;

                    tbl_ApprovalChanges employeeDetails = new tbl_ApprovalChanges();

                    employeeDetails = (from r in dbContext.tbl_ApprovalChanges
                                       where (r.EmployeeID == employeeId && r.FieldDiscription == fieldLabel)
                                       orderby r.CreatedDateTime descending
                                       select r).FirstOrDefault();

                    if (item.ChildApprovalStatusMasterID == 3)
                    {
                        employeeDetails.RejectedDateTime = DateTime.Now;
                        if (columnName == "Country" || columnName == "CurrentCountry")
                        {
                            tbl_PM_CountryMaster country = GetCountryID(oldvalue);
                            tbl_PM_CountryMaster currentcountry = GetCountryID(oldvalue);
                            if (currentcountry != null)
                            {
                                _PM_Employee = dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeID == employeeId).FirstOrDefault();

                                if (columnName == "Country")
                                {
                                    _PM_Employee.CountryID = country.CountryID;
                                    _PM_Employee.Country = country.CountryName;
                                }
                                if (columnName == "CurrentCountry")
                                {
                                    _PM_Employee.CurrentCountryID = currentcountry.CountryID;
                                    _PM_Employee.CurrentCountry = currentcountry.CountryName;
                                }
                                dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            dbContext.RevertAdminApprovalChanges(columnName, oldvalue, employeeId);
                        }
                    }
                }
                isAdded = updateEmployeeName(employeeId.HasValue ? employeeId.Value : 0);
                //isAdded = true;
                return isAdded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool updateEmployeeName(int EmployeeID)
        {
            try
            {
                bool isUpdated = false;
                string employeeName = string.Empty;
                employeeName = GetDisplayName(EmployeeID);
                HRMS_tbl_PM_Employee _employee = dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeID == EmployeeID).FirstOrDefault();
                _employee.EmployeeName = employeeName;
                dbContext.Connection.Close();
                dbContext.SaveChanges();
                isUpdated = true;
                return isUpdated;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<EmployeeQualifications> GetAdminEmployeeQualificationsApprovalDetails(int page, int rows, int EmployeeID, out int totalCount, string module)
        {
            List<EmployeeQualifications> finalQualification = new List<EmployeeQualifications>();
            List<EmployeeQualifications> QualHistory = new List<EmployeeQualifications>();

            // This table always gives new data
            var QualDetails = (from qual in dbContext.tbl_PM_EmployeeQualificationMatrix
                               where qual.EmployeeID == EmployeeID
                               orderby qual.EmployeeQualificationID descending
                               select new EmployeeQualifications
                               {
                                   EmployeeQualificationID = qual.EmployeeQualificationID,
                                   EmployeeID = qual.EmployeeID,
                                   EmployeeQualificationHistoryId = 0,
                                   Specialization = qual.Specialization,
                                   Institute = qual.Institute,
                                   University = qual.University,
                                   //Course = qual.Courses,
                                   Year = qual.PassoutYear,
                                   Percentage = qual.Class,
                                   Degree = qual.HRMS_tbl_PM_QualificationGroupMaster.QualificationGroupName,
                                   DegreeID = qual.QualificationGroupID,
                                   Qualification = qual.HRMS_tbl_PM_Qualifications.QualificationName,
                                   QualificationID = qual.QualificationID,
                                   Type = qual.tbl_PM_QualificationType.QualificationTypeName,
                                   TypeID = qual.QualificationTypeID,
                                   ApprovedComments = string.Empty,
                                   ApprovedType = string.Empty,
                                   ApprovedValue = "New"
                               }).Skip((page - 1) * rows).Take(rows).ToList();

            // This is backup table aka history matrix table
            if (module == "New Qualification Details")
            {
                QualHistory = (from qualhistory in dbContext.tbl_PM_EmployeeQualificationMatrix_History
                               where qualhistory.EmployeeID == EmployeeID && (qualhistory.Status == null)
                               orderby qualhistory.EmployeeQualificationID descending
                               select new EmployeeQualifications
                               {
                                   EmployeeQualificationID = qualhistory.EmployeeQualificationID,
                                   EmployeeID = qualhistory.EmployeeID,
                                   EmployeeQualificationHistoryId = qualhistory.EmployeeQualificationHistoryId,
                                   Specialization = qualhistory.Specialization,
                                   Institute = qualhistory.Institute,
                                   University = qualhistory.University,
                                   //Course = qualhistory.Courses,
                                   Year = qualhistory.PassoutYear,
                                   Percentage = qualhistory.Class,
                                   Degree = qualhistory.HRMS_tbl_PM_QualificationGroupMaster.QualificationGroupName,
                                   DegreeID = qualhistory.QualificationGroupID,
                                   Qualification = qualhistory.HRMS_tbl_PM_Qualifications.QualificationName,
                                   QualificationID = qualhistory.QualificationID,
                                   Type = qualhistory.tbl_PM_QualificationType.QualificationTypeName,
                                   TypeID = qualhistory.QualificationTypeID,
                                   ApprovedComments = qualhistory.Comments,
                                   //ApprovalStatusMasterID=qualhistory.Status,
                                   ApprovedType = qualhistory.ActionType,
                                   ApprovedValue = "Old",
                                   Status = qualhistory.Status
                               }).Skip((page - 1) * rows).Take(rows).ToList();
            }
            else
            {
                QualHistory = (from qualhistory in dbContext.tbl_PM_EmployeeQualificationMatrix_History
                               where qualhistory.EmployeeID == EmployeeID && (qualhistory.Status == 1)
                               orderby qualhistory.EmployeeQualificationID descending
                               select new EmployeeQualifications
                               {
                                   EmployeeQualificationID = qualhistory.EmployeeQualificationID,
                                   EmployeeID = qualhistory.EmployeeID,
                                   EmployeeQualificationHistoryId = qualhistory.EmployeeQualificationHistoryId,
                                   Specialization = qualhistory.Specialization,
                                   Institute = qualhistory.Institute,
                                   University = qualhistory.University,
                                   //Course = qualhistory.Courses,
                                   Year = qualhistory.PassoutYear,
                                   Percentage = qualhistory.Class,
                                   Degree = qualhistory.HRMS_tbl_PM_QualificationGroupMaster.QualificationGroupName,
                                   DegreeID = qualhistory.QualificationGroupID,
                                   Qualification = qualhistory.HRMS_tbl_PM_Qualifications.QualificationName,
                                   QualificationID = qualhistory.QualificationID,
                                   Type = qualhistory.tbl_PM_QualificationType.QualificationTypeName,
                                   TypeID = qualhistory.QualificationTypeID,
                                   ApprovedComments = qualhistory.Comments,
                                   ApprovalStatusMasterID = qualhistory.Status,
                                   ApprovedType = qualhistory.ActionType,
                                   ApprovedValue = "Old",
                                   Status = qualhistory.Status
                               }).Skip((page - 1) * rows).Take(rows).ToList();
            }

            // Remove unwanted records
            foreach (var empQualHistory in QualHistory)
            {
                foreach (var empQualDetail in QualDetails)
                {
                    if (empQualDetail.EmployeeQualificationID == empQualHistory.EmployeeQualificationID)
                    {
                        if (empQualHistory.ApprovedType.Equals("Edit"))
                        {
                            empQualDetail.EmployeeQualificationHistoryId = empQualHistory.EmployeeQualificationHistoryId;
                            empQualDetail.ApprovedType = empQualHistory.ApprovedType;
                            finalQualification.Add(empQualDetail);
                        }
                        else
                        {
                            empQualHistory.ApprovedValue = "New";
                        }
                        finalQualification.Add(empQualHistory);
                    }
                }
            }

            totalCount = finalQualification.Count(x => x.EmployeeID == EmployeeID);
            return finalQualification;
        }

        //GetMainSkillDetails
        public List<SkillDetails> GetEmployeeSkillDetails(int page, int rows, int EmployeeID, out int totalCount)
        {
            List<SkillDetails> empSkillDetails = (from emp in dbContext.tbl_PM_EmployeeSkillMatrix
                                                  join tools in dbContext.HRMS_tbl_PM_Tools on emp.ToolID equals tools.ToolID
                                                  where (emp.EmployeeID == EmployeeID)
                                                  orderby emp.EmployeeID
                                                  select new SkillDetails
                                                  {
                                                      Skill = tools.Description,
                                                      SkillLevel = Convert.ToString(emp.EmployeeskillLevel),
                                                  }).Skip((page - 1) * rows).Take(rows).ToList();

            totalCount = this.dbContext.tbl_PM_EmployeeSkillMatrix.Count(x => x.EmployeeID == EmployeeID);

            return empSkillDetails;
        }

        //GetHistory&MainSkillDetails
        public List<SkillDetails> GetEmpSkillDetailsAndHistory(int page, int rows, int EmployeeID, out int totalCount, string module)
        {
            List<SkillDetails> finalskillList = new List<SkillDetails>();
            List<SkillDetails> skillHistory = new List<SkillDetails>();
            List<SkillDetails> skillDetails = (from emp in dbContext.tbl_PM_EmployeeSkillMatrix
                                               join tools in dbContext.HRMS_tbl_PM_Tools on emp.ToolID equals tools.ToolID
                                               join level in dbContext.Tbl_pm_Proficiencymaster on emp.EmployeeskillLevel equals level.ProficiencyId into skillNew
                                               from skill in skillNew.DefaultIfEmpty()
                                               where (emp.EmployeeID == EmployeeID)
                                               orderby emp.EmployeeID descending
                                               select new SkillDetails
                                               {
                                                   ActionType = string.Empty,
                                                   Value = "New",
                                                   ApproveStatus = 0,
                                                   Comments = string.Empty,

                                                   EmployeeSkillID = emp.EmployeeSkillID,
                                                   EmployeeSkillHistoryID = 0,
                                                   EmployeeID = EmployeeID,
                                                   Skill = tools.Description,
                                                   SkillLevel = skill.Description
                                               }).ToList();

            if (module == "New Skill Details")
            {
                skillHistory = (from emp in dbContext.tbl_PM_EmployeeSkillMatrix_History
                                join tools in dbContext.HRMS_tbl_PM_Tools on emp.ToolID equals tools.ToolID
                                join level in dbContext.Tbl_pm_Proficiencymaster on emp.EmployeeskillLevel equals level.ProficiencyId into skillOld
                                from skill in skillOld.DefaultIfEmpty()
                                where emp.EmployeeID == EmployeeID && (emp.Status == null)
                                orderby emp.EmployeeSkillID descending
                                select new SkillDetails
                                {
                                    ActionType = emp.ActionType,
                                    Value = "Old",
                                    ApprovalStatusMasterID = emp.Status,
                                    Comments = emp.Comments,
                                    EmployeeSkillID = emp.EmployeeSkillID,
                                    EmployeeSkillHistoryID = emp.EmployeeSkillHistoryID,
                                    EmployeeID = EmployeeID,
                                    Skill = tools.Description,
                                    SkillLevel = skill.Description
                                }).ToList();
            }
            else
            {
                skillHistory = (from emp in dbContext.tbl_PM_EmployeeSkillMatrix_History
                                join tools in dbContext.HRMS_tbl_PM_Tools on emp.ToolID equals tools.ToolID
                                join level in dbContext.Tbl_pm_Proficiencymaster on emp.EmployeeskillLevel equals level.ProficiencyId into skillOld
                                from skill in skillOld.DefaultIfEmpty()
                                where emp.EmployeeID == EmployeeID && (emp.Status == 1)
                                orderby emp.EmployeeSkillID descending
                                select new SkillDetails
                                {
                                    ActionType = emp.ActionType,
                                    Value = "Old",
                                    ApprovalStatusMasterID = emp.Status,
                                    Comments = emp.Comments,
                                    EmployeeSkillID = emp.EmployeeSkillID,
                                    EmployeeSkillHistoryID = emp.EmployeeSkillHistoryID,
                                    EmployeeID = EmployeeID,
                                    Skill = tools.Description,
                                    SkillLevel = skill.Description
                                }).ToList();
            }

            // Remove unwanted records
            foreach (var empSkillHistory in skillHistory)
            {
                foreach (var empSkillDetail in skillDetails)
                {
                    if (empSkillDetail.EmployeeSkillID == empSkillHistory.EmployeeSkillID)
                    {
                        if (empSkillHistory.ActionType.Equals("Edit"))
                        {
                            empSkillDetail.EmployeeSkillHistoryID = empSkillHistory.EmployeeSkillHistoryID;
                            empSkillDetail.ActionType = empSkillHistory.ActionType;
                            finalskillList.Add(empSkillDetail);
                        }
                        else
                        {
                            empSkillHistory.Value = "New";
                        }
                        finalskillList.Add(empSkillHistory);
                    }
                }
            }

            totalCount = finalskillList.Count(x => x.EmployeeID == EmployeeID);
            return finalskillList.Skip((page - 1) * rows).Take(rows).ToList();
        }

        //GetHistorySkillDetails
        public List<SkillDetails> GetEmployeeSkillHistoryDetails(int page, int rows, int EmployeeID, out int totalCount)
        {
            var AllDetails = (from cert in dbContext.tbl_PM_EmployeeSkillMatrix_History
                              where cert.EmployeeID == EmployeeID
                              orderby cert.EmployeeSkillID descending
                              select new SkillDetails
                              {
                                  EmployeeSkillID = cert.EmployeeSkillID,
                                  EmployeeSkillHistoryID = cert.EmployeeSkillID,
                                  EmployeeID = EmployeeID
                              }).Skip((page - 1) * rows).Take(rows).ToList();

            totalCount = this.dbContext.tbl_PM_EmployeeCertificationMatrixHistory.Count(x => x.EmployeeID == EmployeeID);

            return AllDetails;
        }

        public bool SaveSkillDetails(tbl_PM_EmployeeSkillMatrix empCert)
        {
            bool isAdded = false;

            tbl_PM_EmployeeSkillMatrix emp = this.dbContext.tbl_PM_EmployeeSkillMatrix.FirstOrDefault(ed => ed.EmployeeSkillID == empCert.EmployeeSkillID);
            if (emp == null || emp.EmployeeSkillID <= 0)
            {
                dbContext.tbl_PM_EmployeeSkillMatrix.AddObject(empCert);
            }
            else
            {
                emp.EmployeeID = empCert.EmployeeID;
                emp.EmployeeSkillID = empCert.EmployeeSkillID;
            }
            dbContext.SaveChanges();
            isAdded = true;
            return isAdded;
        }

        public bool SaveQualificationMatrixHistory(List<EmployeeQualifications> model, string Comments)
        {
            try
            {
                if (model.Any(x => x.EmployeeQualificationHistoryId == 0)) return false;
                var newData = model.Where(cer => cer.ApprovedValue == "New").ToList();
                foreach (var data in newData)
                {
                    var qualificationHistory = dbContext.tbl_PM_EmployeeQualificationMatrix_History.FirstOrDefault(empQuality => empQuality.EmployeeID == data.EmployeeID && empQuality.EmployeeQualificationHistoryId == data.EmployeeQualificationHistoryId);

                    qualificationHistory.Comments = Comments;
                    qualificationHistory.Status = data.ApprovalStatusMasterID == 0 ? null : data.ApprovalStatusMasterID;
                    qualificationHistory.ActionType = data.ApprovedType;
                    qualificationHistory.ModifiedDate = DateTime.Now;

                    if (data.ApprovalStatusMasterID == 3)
                    {
                        var qualificationMatrix = dbContext.tbl_PM_EmployeeQualificationMatrix.FirstOrDefault(empQuality => empQuality.EmployeeID == data.EmployeeID
                            && empQuality.EmployeeQualificationID == data.EmployeeQualificationID);

                        if (data.ApprovedType.Equals("Add"))
                        {
                            dbContext.tbl_PM_EmployeeQualificationMatrix.DeleteObject(qualificationMatrix);
                        }
                        else
                        {
                            qualificationMatrix.QualificationID = qualificationHistory.QualificationID.GetValueOrDefault(0);
                            qualificationMatrix.University = qualificationHistory.University;
                            qualificationMatrix.PassoutYear = qualificationHistory.PassoutYear;
                            qualificationMatrix.QualificationTypeID = qualificationHistory.QualificationTypeID;
                            qualificationMatrix.Specialization = qualificationHistory.Specialization;
                            qualificationMatrix.Class = qualificationHistory.Class;
                            qualificationMatrix.Courses = qualificationHistory.Courses;
                            qualificationMatrix.Institute = qualificationHistory.Institute;
                            qualificationMatrix.QualificationGroupID = qualificationHistory.QualificationGroupID.GetValueOrDefault(1);
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

        public bool SaveEmployeeSkillMatrixHistory(List<SkillDetails> model, string SkillHrComment)
        {
            try
            {
                if (model.Any(x => x.EmployeeSkillHistoryID == 0)) return false;
                var newData = model.Where(cer => cer.Value == "New").ToList();
                foreach (var data in newData)
                {
                    var skillHistory = dbContext.tbl_PM_EmployeeSkillMatrix_History.FirstOrDefault(empQuality => empQuality.EmployeeID == data.EmployeeID && empQuality.EmployeeSkillHistoryID == data.EmployeeSkillHistoryID);

                    skillHistory.Comments = SkillHrComment;
                    skillHistory.Status = data.ApprovalStatusMasterID == 0 ? null : data.ApprovalStatusMasterID;
                    skillHistory.ActionType = data.ActionType;
                    skillHistory.ModifiedDate = DateTime.Now;

                    if (data.ApprovalStatusMasterID == 3)
                    {
                        var skillMatrix =
                            dbContext.tbl_PM_EmployeeSkillMatrix.FirstOrDefault(empskill => empskill.EmployeeID == data.EmployeeID && empskill.EmployeeSkillID == data.EmployeeSkillID);

                        if (data.ActionType.Equals("Add"))
                        {
                            dbContext.tbl_PM_EmployeeSkillMatrix.DeleteObject(skillMatrix);
                        }
                        else
                        {
                            skillMatrix.EmployeeSkillID = skillHistory.EmployeeSkillID;
                            skillMatrix.EmployeeskillLevel = skillHistory.EmployeeskillLevel;
                            skillMatrix.ToolID = skillHistory.ToolID;
                            skillHistory.Proficiency = skillHistory.Proficiency;
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

        [Obsolete("This method is not in use")]
        public bool SaveQualificationMatrixHistoryOld(List<EmployeeQualifications> model)
        {
            try
            {
                var newData = model.Where(cer => cer.ApprovedValue == "New").ToList();
                foreach (var data in newData)
                {
                    tbl_PM_EmployeeQualificationMatrix_History qualificationHistory = new tbl_PM_EmployeeQualificationMatrix_History();
                    if (data.EmployeeQualificationHistoryId == 0)
                    {
                        qualificationHistory = dbContext.tbl_PM_EmployeeQualificationMatrix_History.FirstOrDefault(empQuality => empQuality.EmployeeID == data.EmployeeID);
                    }
                    else
                    {
                        qualificationHistory = dbContext.tbl_PM_EmployeeQualificationMatrix_History.FirstOrDefault(empQuality => empQuality.EmployeeID == data.EmployeeID && empQuality.EmployeeQualificationHistoryId == data.EmployeeQualificationHistoryId);
                    }
                    qualificationHistory.Comments = data.ApprovedComments;
                    qualificationHistory.Status = data.ApprovalStatusMasterID;
                    qualificationHistory.ActionType = data.ApprovedType;

                    if (data.ApprovalStatusMasterID == 3)
                    {
                        tbl_PM_EmployeeQualificationMatrix qualificationMatrix =
                            dbContext.tbl_PM_EmployeeQualificationMatrix.FirstOrDefault(
                                empQuality =>
                                empQuality.EmployeeID == data.EmployeeID
                                && empQuality.EmployeeQualificationID == data.EmployeeQualificationID);

                        if (data.ApprovedType.Equals("Add"))
                        {
                            dbContext.tbl_PM_EmployeeQualificationMatrix.DeleteObject(qualificationMatrix);
                        }
                        else
                        {
                            qualificationMatrix.QualificationID = qualificationHistory.QualificationID.GetValueOrDefault(0);
                            qualificationMatrix.University = qualificationHistory.University;
                            qualificationMatrix.PassoutYear = qualificationHistory.PassoutYear;
                            qualificationMatrix.QualificationTypeID = qualificationHistory.QualificationTypeID;
                            qualificationMatrix.Specialization = qualificationHistory.Specialization;
                            qualificationMatrix.Class = qualificationHistory.Class;
                            qualificationMatrix.Courses = qualificationHistory.Courses;
                            qualificationMatrix.Institute = qualificationHistory.Institute;
                            //qualificationMatrix.tbl_PM_QualificationGroupMaster.QualificationGroupName = qualificationHistory.tbl_PM_QualificationGroupMaster.QualificationGroupName;
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

        [Obsolete("This method is not in use")]
        public bool SaveEmployeeSkillMatrixHistoryOld(List<SkillDetails> model)
        {
            try
            {
                var newData = model.Where(cer => cer.Value == "New").ToList();
                var Old_Data = model.Where(x => x.Value == "Old").ToList();
                foreach (var data in newData)
                {
                    tbl_PM_EmployeeSkillMatrix_History skillHistory = new tbl_PM_EmployeeSkillMatrix_History();

                    if (data.EmployeeSkillHistoryID == 0)
                    {
                        skillHistory = dbContext.tbl_PM_EmployeeSkillMatrix_History.FirstOrDefault(empQuality => empQuality.EmployeeID == data.EmployeeID);
                    }
                    else
                    {
                        skillHistory = dbContext.tbl_PM_EmployeeSkillMatrix_History.FirstOrDefault(empQuality => empQuality.EmployeeID == data.EmployeeID && empQuality.EmployeeSkillHistoryID == data.EmployeeSkillHistoryID);
                    }
                    skillHistory.Comments = data.Comments;
                    skillHistory.Status = data.ApprovalStatusMasterID;
                    skillHistory.ActionType = data.ActionType;

                    if (data.ApprovalStatusMasterID == 3)
                    {
                        tbl_PM_EmployeeSkillMatrix skillMatrix =
                        dbContext.tbl_PM_EmployeeSkillMatrix.FirstOrDefault(
                            empskill =>
                            empskill.EmployeeID == data.EmployeeID
                            && empskill.EmployeeSkillID == data.EmployeeSkillID);

                        if (data.ActionType.Equals("Add"))
                        {
                            dbContext.tbl_PM_EmployeeSkillMatrix.DeleteObject(skillMatrix);
                        }
                        else
                        {
                            foreach (var item in Old_Data)
                            {
                                if (data.EmployeeSkillHistoryID == item.EmployeeSkillHistoryID)
                                {
                                    HRMS_tbl_PM_Tools _Tools = dbContext.HRMS_tbl_PM_Tools.Where(x => x.Description == item.Skill).FirstOrDefault();
                                    Tbl_pm_Proficiencymaster _Proficiencymaster = dbContext.Tbl_pm_Proficiencymaster.Where(x => x.Description == item.SkillLevel).FirstOrDefault();
                                    skillMatrix.EmployeeSkillID = skillHistory.EmployeeSkillID;
                                    skillMatrix.EmployeeskillLevel = _Proficiencymaster.ProficiencyId;
                                    skillMatrix.ToolID = _Tools.ToolID;
                                }
                            }
                        }
                    }
                    dbContext.SaveChanges();
                }
                //dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetDisplayName(int employeeID)
        {
            string displayName = "";
            EmployeeDAL employeeDAL = new EmployeeDAL();
            HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(employeeID);
            string firstName = "";
            string lastname = "";
            string middleName = "";
            bool statusf = false;
            bool statusl = false;
            bool statusm = false;
            IQueryable<tbl_ApprovalChanges> employeeAppdetails = employeeDAL.GetEmployeeFirstName(employeeID);
            if (employeeAppdetails != null && employeeAppdetails.Count() > 0 && (employeeAppdetails.Where(ed => ed.FieldDbColumnName == "FirstName").Count() > 0) || (employeeAppdetails.Where(ed => ed.FieldDbColumnName == "LastName").Count() > 0) || (employeeAppdetails.Where(ed => ed.FieldDbColumnName == "MiddleName").Count() > 0))
            {
                foreach (var item in employeeAppdetails)
                {
                    if (item.FieldDbColumnName == "FirstName" && (item.ApprovalStatusMasterID == 3 || item.ApprovalStatusMasterID == 2))
                    {
                        if (statusf == false)
                        {
                            firstName = employeeDetails.FirstName;
                            statusf = true;
                        }
                    }
                    else if (item.FieldDbColumnName == "FirstName" && statusf == false)
                    {
                        firstName = item.OldValue;
                        statusf = true;
                    }
                    else if (statusf == false)
                    {
                        firstName = employeeDetails.FirstName;
                    }
                    if ((item.ApprovalStatusMasterID == 3 || item.ApprovalStatusMasterID == 2) && item.FieldDbColumnName == "LastName")
                    {
                        if (statusl == false)
                        {
                            lastname = employeeDetails.LastName;
                            statusl = true;
                        }
                    }
                    else if (item.FieldDbColumnName == "LastName" && statusl == false)
                    {
                        lastname = item.OldValue;
                        statusl = true;
                    }
                    else if (statusl == false)
                    {
                        lastname = employeeDetails.LastName;
                    }

                    if ((item.ApprovalStatusMasterID == 3 || item.ApprovalStatusMasterID == 2) && item.FieldDbColumnName == "MiddleName")
                    {
                        if (statusm == false)
                        {
                            middleName = employeeDetails.MiddleName;
                            statusm = true;
                        }
                    }
                    else if (item.FieldDbColumnName == "MiddleName" && statusm == false)
                    {
                        middleName = item.OldValue;
                        statusm = true;
                    }
                    else if (statusm == false)
                    {
                        middleName = employeeDetails.MiddleName;
                    }
                }
                displayName = firstName + " " + middleName + " " + lastname;
            }
            else
                displayName = employeeDetails.EmployeeName;
            return displayName;
        }

        public Tuple<string, string> GetEmployeeProfileImagePath(int EmployeeID)
        {
            try
            {
                HRMS_tbl_PM_Employee _employeeDetails = dbContext.HRMS_tbl_PM_Employee.Where(e => e.EmployeeID == EmployeeID).FirstOrDefault();
                string ImagePath = "";
                string FirstName = "";
                FirstName = _employeeDetails.FirstName;
                if (System.IO.File.Exists(_employeeDetails.ProfileImagePath) == true)
                {
                    ImagePath = _employeeDetails.ProfileImagePath;
                }
                return new Tuple<string, string>(ImagePath, FirstName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SkillDetailsViewModel> GetSkillDeeetails(int employeeId, int page, int rows, out int totalCount)
        {
            try
            {
                List<SkillDetailsViewModel> Records = new List<SkillDetailsViewModel>();

                var Details = dbSEMContext.Get_DataSkill_SP(employeeId);
                //var test = Details.AsEnumerable().ToList();
                Records = (from d in Details
                           select new SkillDetailsViewModel
                           {
                               ToolId = d.toolID,

                               Description = d.Description,

                               ResourcePoolId = d.ResourcePoolID,

                               ResourcePoolName = d.ResourcePoolName,

                               Rating = d.ratings,

                               SkillId = d.toolID
                           }).ToList();
                totalCount = Records.Count();
                return Records.Skip((page - 1) * rows).Take(rows).ToList();
                //return Records.ToList();
                var test = Records.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SkillDetailsViewModel> GetDevelopmentPlanSkills(int EmployeeId, int ResPoolId, int? id, int page, int rows, out int totalCount)
        {
            try
            {
                List<SkillDetailsViewModel> Records = new List<SkillDetailsViewModel>();
                var Details = dbSEMContext.GetDevelopmentPlanSkillsDetails_SP(EmployeeId, ResPoolId);

                Records = (from d in Details
                           select new SkillDetailsViewModel
                           {
                               Description = d.Description,
                               Rating = d.ratings,
                               ToolId = d.toolID,
                               ID = d.ID
                           }).ToList();
                totalCount = Records.Count();
                return Records.Skip((page - 1) * rows).Take(rows).ToList();

                var test = Records.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SkillDetailsViewModel> getSkillMatrixDevelopmentPlan(int EmployeeId, int ResId)
        {
            List<SkillDetailsViewModel> employeeDetails = new List<SkillDetailsViewModel>();

            try
            {
                var skill = dbSEMContext.getSkillDevelopmentPlan_SP(EmployeeId, ResId);
                var test = skill.AsEnumerable().ToList();
                employeeDetails = (from type in test
                                   select new SkillDetailsViewModel

                                   {
                                       ID = type.ID,
                                       ResourcePoolName = type.ResourcePoolName,
                                       Description = type.SkillName,
                                       Rating = type.Rating,
                                       ExpectedRating = type.ExpectedRating,
                                       TargetDate = type.TargetDate
                                   }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return employeeDetails;
        }

        public bool ActiveSkillDevelopmentPlan(List<string> id)
        {
            string respid = id[0].Replace("abc", "").Trim();

            PersonalDetailsDAL dal = new PersonalDetailsDAL();
            if (id == null)
            {
                id = new List<string>();
            }
            try
            {
                bool status = false;
                foreach (string InDId in id)
                {
                    if (InDId != "")
                    {
                        ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                        int Id = Convert.ToInt32(respid);
                        dbSEMContext.ActiveSkillDevelopmentPlan_SP(Id, Result);
                        status = Convert.ToBoolean(Result.Value);
                    }
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
            //return true;
        }

        public List<Ratings> GetRatingsDetails()
        {
            List<Ratings> rating1 = new List<Ratings>();
            var ratings = dbSEMContext.GetEmployeeSkillRatings_SP();
            rating1 = (from r in ratings
                       select new Ratings
                       {
                           ProficiencyID = r.ProficiencyId,
                           Rating = r.Description
                       }).ToList();
            return rating1;
        }

        public bool SaveRatingDetails(string ID1, string ExpectedRating, DateTime? Targetdate, int loggedInEmployeeId, string UpdatedBy)
        {
            int ID = Convert.ToInt32(ID1);
            WSEMDBEntities dbContext = new WSEMDBEntities();
            bool status = false;
            ObjectParameter Result = new ObjectParameter("Result", typeof(int));
            dbContext.SubmitSkillDevelopmentPlan_SP(ID, ExpectedRating, Targetdate, Result);
            status = Convert.ToBoolean(Result.Value);
            return status;
        }

        public bool SubmitSkillDevelopmentPlan(int? ID, string ExpectedRating, DateTime? Targetdate)
        {
            try
            {
                WSEMDBEntities dbContext = new WSEMDBEntities();
                bool status = false;
                ObjectParameter Result = new ObjectParameter("Result", typeof(int));
                dbContext.SubmitSkillDevelopmentPlan_SP(ID, ExpectedRating, Targetdate, Result);
                status = Convert.ToBoolean(Result.Value);
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //-
        public List<SkillDetailsViewModel> GetSubSkillDetails(int EmployeeId, int ResPoolId, int page, int rows, out int totalCount)
        {
            try
            {
                List<SkillDetailsViewModel> Records = new List<SkillDetailsViewModel>();

                var Details = dbSEMContext.Get_SubSkill_SP(EmployeeId, ResPoolId);

                Records = (from d in Details
                           select new SkillDetailsViewModel
                           {
                               Description = d.Description,
                               Rating = d.ratings,
                               ToolId = d.toolID,
                               ID = d.ID
                           }).ToList();
                totalCount = Records.Count();
                return Records.Skip((page - 1) * rows).Take(rows).ToList();

                //var test = Records.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveSubSkillRatings(int searchedEmployeeId, int? SkillID, string Rating)
        {
            ObjectParameter Output = new ObjectParameter("Output", typeof(int));

            dbSEMContext.UpdateSubSkillByRatings_SP(searchedEmployeeId, SkillID, Rating, Output);
            bool status = Convert.ToBoolean(Output.Value);
            Output.Value = 0;
            return status;
        }

        public List<SkillDetailsViewModel> GetSkillDetails(int employeeId, int page, int rows, out int totalCount)
        {
            try
            {
                List<SkillDetailsViewModel> Records = new List<SkillDetailsViewModel>();

                var Details = dbSEMContext.Get_DataSkill_SP(employeeId);
                //  var test = Details.AsEnumerable().ToList();
                Records = (from d in Details
                           select new SkillDetailsViewModel
                           {
                               ToolId = d.toolID,

                               Description = d.Description,

                               ResourcePoolId = d.ResourcePoolID,

                               ResourcePoolName = d.ResourcePoolName,

                               Rating = d.ratings,

                               SkillId = d.toolID
                           }).ToList();
                totalCount = Records.Count();
                return Records.Skip((page - 1) * rows).Take(rows).ToList();
                //return Records.ToList();
                var test = Records.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SubmitSkillManagementDetails(int ID, string Rating)
        {
            ObjectParameter Result = new ObjectParameter("result", typeof(int));
            dbSEMContext.SubmitSkillManagementDetails_SP(ID, Rating, Result);
            bool status = Convert.ToBoolean(Result.Value);
            Result.Value = 0;
            return status;
        }

        public List<SkillDetailsViewModel> SearchSkillForSkillMgmt(string searchText, int pageNo = 1, int pageSize = 10)
        {
            List<SkillDetailsViewModel> skillDetails = new List<SkillDetailsViewModel>();

            try
            {
                skillDetails = (from skill in dbContext.tbl_PM_ResourcePoolMaster
                                // where (employee.EmployeeName.Contains(searchText) || employee.EmployeeCode.Contains(searchText))
                                where (skill.ResourcePoolCode.Contains(searchText))
                                orderby skill.ResourcePoolCode
                                select new SkillDetailsViewModel
                                {
                                    ResourcePoolNames = skill.ResourcePoolCode,
                                    ResourcePoolId = skill.ResourcePoolID
                                }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return skillDetails;
        }

        public string Get_ResourcePoolName(int? EmployeeID)
        {
            try
            {
                var resourcePool = (from poolMaster in dbContext.tbl_PM_ResourcePoolMaster
                                    from poolDetail in dbContext.tbl_PM_ResourcePoolDetail
                                    where poolMaster.ResourcePoolID == poolDetail.ResourcePoolID && poolDetail.EmployeeID == EmployeeID
                                    select poolMaster.ResourcePoolName).FirstOrDefault();

                return resourcePool;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Get_EmployeeName(string EmployeeId)
        {
            WSEMDBEntities dbcon = new WSEMDBEntities();
            dbcon.Connection.Open();
            int EmpId = int.Parse(EmployeeId);
            var result = from m in dbcon.tbl_PM_Employee_SEM
                         where m.EmployeeID == EmpId
                         select m.EmployeeName;
            var s = "";
            foreach (var Temp in result)
            {
                s = Temp;
            }
            dbcon.Connection.Close();
            return s;
        }

        public bool UpdatePassword(string UserId, string Password)
        {
            int UserID = Convert.ToInt32(UserId);
            ObjectParameter Result = new ObjectParameter("result", typeof(int));
            dbSEMContext.UpdatePassword_SP(UserID, Password, Result);
            bool status = Convert.ToBoolean(Result.Value);
            Result.Value = 0;
            return status;
        }
    }
}