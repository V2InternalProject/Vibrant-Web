using HRMS.Models;
using HRMS.Models.Enums;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace HRMS.DAL
{
    public class EmployeeDAL
    {
        //Initialized the DB Context of HRMS entity model
        private HRMSDBEntities dbContext = new HRMSDBEntities();

        private WSEMDBEntities dbSEMContext = new WSEMDBEntities();
        private PMSDbEntities dbPmsContext = new PMSDbEntities();
        private PMS3_HRMSDBEntities dbpms3Context = new PMS3_HRMSDBEntities();
        private V2toolsDBEntities dbv2toolsContext = new V2toolsDBEntities();
        private bool MemberInactive;

        public List<LoginRolesDetails> GetLoginRolesDetails()
        {
            List<LoginRolesDetails> LRolesDetails = new List<LoginRolesDetails>();
            try
            {
                LRolesDetails = (from LRDetails in dbv2toolsContext.aspnet_Roles
                                 orderby LRDetails.RoleName
                                 select new LoginRolesDetails
                                 {
                                     RoleDescription = LRDetails.Description,
                                     RoleId = LRDetails.RoleId,
                                     RoleName = LRDetails.RoleName
                                 }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LRolesDetails;
        }

        public int GetFormCode()
        {
            var code = (from c in dbContext.tbl_HR_Expense
                        orderby c.FormCode descending
                        select c.FormCode).FirstOrDefault();

            if (code == null)
            {
                return 1;
            }
            else
            {
                return (Convert.ToInt32(code) + 1);
            }
        }

        public string GetSpecificLoginRolesDetails(string roleId)
        {
            Guid roleID = Guid.Parse(roleId);
            string LRolesDetails = "";
            try
            {
                LRolesDetails = (from LRDetails in dbv2toolsContext.aspnet_Roles
                                 where LRDetails.RoleId == roleID
                                 orderby LRDetails.RoleName
                                 select LRDetails.RoleName).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LRolesDetails;
        }

        public List<ReportingToList_Emp> GetReportingToList_Emp()
        {
            List<ReportingToList_Emp> resourcepool = new List<ReportingToList_Emp>();
            try
            {
                resourcepool = (from resource in dbContext.HRMS_tbl_PM_Employee
                                where resource.Status == false
                                orderby resource.EmployeeName ascending
                                select new ReportingToList_Emp
                                {
                                    EmployeeId = resource.EmployeeID,
                                    EmployeeName = resource.EmployeeName
                                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return resourcepool;
        }

        public HRMS_tbl_PM_Employee GetEmployeeReportingToName_Emp(int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                int ReportingToId_Emp = (from id in dbContext.HRMS_tbl_PM_Employee
                                         where id.EmployeeID == employeeId

                                         select id.CostCenterID.HasValue ? id.CostCenterID.Value : 0).FirstOrDefault();

                if (ReportingToId_Emp > 0)
                {
                    HRMS_tbl_PM_Employee ReportingTo = (from name in dbContext.HRMS_tbl_PM_Employee
                                                        where name.EmployeeID == ReportingToId_Emp
                                                        select name).FirstOrDefault();

                    return ReportingTo;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public HRMS_tbl_PM_Employee GetExitConfirmationManagerName_Emp(int employeeId)
        {
            try
            {
                //CostCenter represents competency Manager Id
                dbContext = new HRMSDBEntities();
                int ExitConfirmationManagerId_Emp = (from id in dbContext.HRMS_tbl_PM_Employee
                                                     where id.EmployeeID == employeeId

                                                     select id.ReportingTo.HasValue ? id.ReportingTo.Value : 0).FirstOrDefault();

                if (ExitConfirmationManagerId_Emp > 0)
                {
                    HRMS_tbl_PM_Employee ExitConfirmationManager = (from name in dbContext.HRMS_tbl_PM_Employee
                                                                    where name.EmployeeID == ExitConfirmationManagerId_Emp
                                                                    select name).FirstOrDefault();

                    return ExitConfirmationManager;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tbl_HR_ExitInstance GetSeperationDetails(int employeeId)
        {
            try
            {
                tbl_HR_ExitInstance exitEnstance = (from sd in dbContext.tbl_HR_ExitInstance
                                                    where sd.EmployeeID == employeeId
                                                    orderby sd.CreatedDate descending
                                                    select sd).FirstOrDefault();

                return exitEnstance;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public HRMS_tbl_PM_Employee GetCompetencyManagerName_Emp(int employeeId)
        {
            try
            {
                //CostCenter represents competency Manager Id
                dbContext = new HRMSDBEntities();
                int CompetencyManagerId_Emp = (from id in dbContext.HRMS_tbl_PM_Employee
                                               where id.EmployeeID == employeeId

                                               select id.CompetencyManager.HasValue ? id.CompetencyManager.Value : 0).FirstOrDefault();

                if (CompetencyManagerId_Emp > 0)
                {
                    HRMS_tbl_PM_Employee CompetencyManager = (from name in dbContext.HRMS_tbl_PM_Employee
                                                              where name.EmployeeID == CompetencyManagerId_Emp
                                                              select name).FirstOrDefault();

                    return CompetencyManager;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new System.Configuration.AppSettingsReader();
            // Get the key from config file

            string key = (string)settingsReader.GetValue("SecurityKey",
                                                             typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public List<EmployeeDetails> SearchEmployee(string searchText, int pageNo = 1, int pageSize = 10)
        {
            List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();
            try
            {
                employeeDetails = (from employee in dbContext.HRMS_tbl_PM_Employee
                                   where (employee.EmployeeName.Contains(searchText) || employee.EmployeeCode.Contains(searchText))
                                   orderby employee.EmployeeName
                                   select new EmployeeDetails
                                   {
                                       EmployeeCode = employee.EmployeeCode,
                                       EmployeeId = employee.EmployeeID,
                                       UserId = employee.UserID,
                                       EmployeeName = employee.EmployeeName,
                                       Address = employee.Address,
                                       PostID = employee.PostID
                                   }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employeeDetails;
        }

        public List<EmployeeDetails> SearchEmployeeForSEM(string searchText, int pageNo = 1, int pageSize = 10)
        {
            List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();
            List<EmployeeDetails> result = new List<EmployeeDetails>();
            try
            {
                employeeDetails = (from employee in dbSEMContext.tbl_PM_Employee_SEM
                                   where (employee.EmployeeName.Contains(searchText) || employee.EmployeeCode.Contains(searchText)) && employee.Status == false
                                   orderby employee.EmployeeName
                                   select new EmployeeDetails
                                   {
                                       EmployeeCode = employee.EmployeeCode,
                                       EmployeeId = employee.EmployeeID,
                                       UserId = employee.UserID,
                                       EmployeeName = employee.EmployeeName,
                                       Address = employee.Address,
                                       PostID = employee.PostID
                                   }).ToList();//Skip((pageNo - 1) * pageSize).Take(pageSize).
                //Added by Rahul Ramachandran: Issue ID 3890
                var empTerminateList = (from emp in dbContext.HRMS_tbl_PM_Employee
                                        where (emp.EmployeeName.Contains(searchText) || emp.EmployeeCode.Contains(searchText)) && emp.Status != false && emp.EmployeeStatusID == 13
                                        orderby emp.EmployeeName
                                        select new EmployeeDetails
                                        {
                                            EmployeeCode = emp.EmployeeCode,
                                            EmployeeId = emp.EmployeeID,
                                            UserId = emp.UserID,
                                            EmployeeName = emp.EmployeeName,
                                            Address = emp.Address,
                                            PostID = emp.PostID
                                        }).ToList();
                result = employeeDetails.Where(p => !empTerminateList.Any(p2 => p2.EmployeeCode == p.EmployeeCode)).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        //public List<EmployeeDetails> SearchEmployeeForSEM(string searchText, int pageNo = 1, int pageSize = 10)
        //{
        //    List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();

        //    try
        //    {
        //        employeeDetails = (from employee in dbSEMContext.tbl_PM_Employee_SEM
        //                           where (employee.EmployeeName.Contains(searchText) || employee.EmployeeCode.Contains(searchText)) && employee.Status == false
        //                           orderby employee.EmployeeName
        //                           select new EmployeeDetails
        //                           {
        //                               EmployeeCode = employee.EmployeeCode,
        //                               EmployeeId = employee.EmployeeID,
        //                               UserId = employee.UserID,
        //                               EmployeeName = employee.EmployeeName,
        //                               Address = employee.Address,
        //                               PostID = employee.PostID
        //                           }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return employeeDetails;
        //}

        public List<EmployeeDetails> SearchEmployeeForRecruiter(string searchText, int pageNo = 1, int pageSize = 10)
        {
            List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();
            try
            {
                employeeDetails = (from employee in dbContext.HRMS_tbl_PM_Employee
                                   where (employee.EmployeeName.Contains(searchText) || employee.EmployeeCode.Contains(searchText)) && employee.Status == false
                                   orderby employee.EmployeeName
                                   select new EmployeeDetails
                                   {
                                       EmployeeCode = employee.EmployeeCode,
                                       EmployeeId = employee.EmployeeID,
                                       UserId = employee.UserID,
                                       EmployeeName = employee.EmployeeName,
                                       Address = employee.Address,
                                       PostID = employee.PostID
                                   }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employeeDetails;
        }

        public List<EmployeeDetails> GetSearchEmployeeForRecruiter()
        {
            List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();
            try
            {
                employeeDetails = (from employee in dbContext.HRMS_tbl_PM_Employee
                                   where employee.Status == false                                // modified to resovle UAT issue
                                   orderby employee.EmployeeName ascending
                                   select new EmployeeDetails
                                   {
                                       EmployeeCode = employee.EmployeeCode,
                                       EmployeeId = employee.EmployeeID,
                                       UserId = employee.UserID,
                                       EmployeeName = employee.EmployeeName,
                                       Address = employee.Address,
                                       PostID = employee.PostID
                                   }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employeeDetails;
        }

        /// <summary>
        /// Get Total Count of the Employee List whose Name or Employee Code mataches with the searchText
        /// </summary>
        /// <param name="searchText">Input string which contains either Employee Name or Employee Code</param>
        /// <returns>Total Count of the employees whose name or employee code matches with the input string</returns>
        public int SearchEmployeeTotalCount(string searchText)
        {
            int totalCount = 0;
            try
            {
                totalCount = (from employee in dbContext.HRMS_tbl_PM_Employee
                              where employee.EmployeeName.Contains(searchText) || employee.EmployeeCode.Contains(searchText)
                              select employee.EmployeeID).Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return totalCount;
        }

        /// <summary>
        /// To get Employee Details by employee id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public HRMS_tbl_PM_Employee GetEmployeeDetails(int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                HRMS_tbl_PM_Employee EmpDetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();
                return EmpDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public HRMS_tbl_PM_Employee GetEmployeeDetailsExit(int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                HRMS_tbl_PM_Employee EmpDetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == employeeId && ed.Status == false).FirstOrDefault();
                return EmpDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public tbl_PM_Employee_SEM GetEmployeeDetailsForConfirmation(int employeeId)
        {
            try
            {
                dbSEMContext = new WSEMDBEntities();
                tbl_PM_Employee_SEM EmpDetails = dbSEMContext.tbl_PM_Employee_SEM.Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();
                return EmpDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<tbl_ApprovalChanges> GetEmployeeFirstName(int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                IQueryable<tbl_ApprovalChanges> EmpFirstName = (dbContext.tbl_ApprovalChanges.Where(ed => ed.EmployeeID == employeeId).OrderByDescending(ed => ed.CreatedDateTime)).Take(3);
                return EmpFirstName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public HRMS_tbl_PM_Employee GetEmployeeDetailsFromEmailId(string id)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                HRMS_tbl_PM_Employee EmpDetailsbyid = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmailID == id && ed.Status == false).FirstOrDefault();
                return EmpDetailsbyid;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public HRMS_tbl_PM_Employee GetEmployeeDetailsByEmployeeCode(string employeeCode)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                HRMS_tbl_PM_Employee EmpDetailsbyCode = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeCode == employeeCode && ed.Status == false).FirstOrDefault();
                return EmpDetailsbyCode;
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

        //public HRMS_tbl_PM_Employee GetEmployeeDetailsFromEmailId(string Email)
        //{
        //    try
        //    {
        //        dbContext = new HRMSDBEntities();
        //        HRMS_tbl_PM_Employee EmpDetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmailID == Email).FirstOrDefault();
        //        return EmpDetails;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public HRMS_tbl_PM_Employee GetUserDetailByEmployeeCode(string employeeCode)
        {
            return dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeCode == employeeCode).SingleOrDefault();
        }

        /// <summary>
        /// To retrieve emergency contact details of an employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public tbl_PM_EmployeeEmergencyContact GetEmployeeEmergencyContactDetails(int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                tbl_PM_EmployeeEmergencyContact emergencyContactDetails = dbContext.tbl_PM_EmployeeEmergencyContact.Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();
                return emergencyContactDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<GradeListDetails> GetGradeList()
        {
            List<GradeListDetails> gradelist = new List<GradeListDetails>();
            try
            {
                gradelist = (from grade in dbContext.tbl_PM_GradeMaster
                             orderby grade.GradeID ascending
                             select new GradeListDetails
                                    {
                                        GradeId = grade.GradeID,
                                        GradeName = grade.Grade
                                    }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return gradelist;
        }

        /// <summary>
        /// To retrieve employee's last 3 designation details
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<DesignationDetails> GetEmployeePreviousDesignationDetails(int page, int rows, int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();

                var designationDetails = (from preDesignation in dbContext.tbl_PM_EmployeeDesignation_Change
                                          join designationMaster in dbContext.tbl_PM_DesignationMaster on preDesignation.NewDesignationID equals designationMaster.DesignationID
                                          join designationMaster1 in dbContext.tbl_PM_DesignationMaster on preDesignation.DesignationID equals designationMaster1.DesignationID
                                          where preDesignation.EmployeeID == employeeId
                                          select new
                                          {
                                              isDefaultRecord = false,
                                              ModifiedDate = preDesignation.ModifiedDate,
                                              UniqueId = preDesignation.UniqueID,
                                              Designation = designationMaster.DesignationName,
                                              Year = preDesignation.Year,
                                              Month = preDesignation.Month,
                                              RoleDesription = preDesignation.RoleDescription,
                                              Level = preDesignation.Level,
                                              Grade = preDesignation.CurrentGradeID,
                                              JoiningDesignation = designationMaster1.DesignationName
                                          }).ToList().OrderByDescending(x => x.Year).ThenBy(x => x.Month);

                var count = designationDetails.Count();

                if (count > 0)
                {
                    DesignationDetailsViewModel model = new DesignationDetailsViewModel();
                    model.DesignationDetailsList = new List<DesignationDetails>();

                    //first element of list will always be current designation details
                    foreach (var item in designationDetails)
                    {
                        var grade = (from g in dbContext.tbl_PM_GradeMaster
                                     where g.GradeID == item.Grade
                                     select g.Grade).FirstOrDefault();
                        model.DesignationDetailsList.Add(new DesignationDetails()
                        {
                            isDefaultRecord = item.isDefaultRecord,
                            UniqueId = item.UniqueId,
                            Designation = item.Designation.ToString(),
                            Year = item.Year,
                            Month = item.Month,
                            Grade = grade,
                            GradeId = item.Grade,
                            Level = item.Level,
                            RoleDescription = item.RoleDesription
                        });
                    }

                    //last element of list will be joining designation
                    foreach (var item in model.DesignationDetailsList)
                    {
                        item.JoiningDesignation = (from ab in designationDetails orderby ab.ModifiedDate descending select ab.JoiningDesignation).FirstOrDefault();
                        //designationDetails.Last().JoiningDesignation;
                    }

                    return (model.DesignationDetailsList).Skip((page - 1) * rows).Take(rows).ToList();
                }
                else
                {
                    var joiningYear = (from e in dbContext.HRMS_tbl_PM_Employee where e.EmployeeID == employeeId select e.JoiningDate.Value.Year).FirstOrDefault();
                    var joiningMonth1 = (from e in dbContext.HRMS_tbl_PM_Employee where e.EmployeeID == employeeId select e.JoiningDate.Value.Month).FirstOrDefault();
                    var joiningMonth = new DateTime(2010, joiningMonth1, 1).ToString("MMMM");
                    var empdesignationDetails = (from preDesignation in dbContext.HRMS_tbl_PM_Employee
                                                 join designationMaster in dbContext.tbl_PM_DesignationMaster on preDesignation.DesignationID equals designationMaster.DesignationID
                                                 where preDesignation.EmployeeID == employeeId && preDesignation.DesignationID > 0
                                                 select new
                                                 {
                                                     isDefaultRecord = true,
                                                     ModifiedDate = preDesignation.ModifiedDate,
                                                     //UniqueId = preDesignation.UniqueID,
                                                     Designation = designationMaster.DesignationName,
                                                     Year = joiningYear,
                                                     Month = joiningMonth,
                                                     RoleDesription = preDesignation.RoleDescription,
                                                     Level = preDesignation.Level,
                                                     Grade = preDesignation.GradeID,
                                                     JoiningDesignation = designationMaster.DesignationName
                                                 }).ToList();

                    var empCount = empdesignationDetails.Count();
                    DesignationDetailsViewModel model = new DesignationDetailsViewModel();
                    model.DesignationDetailsList = new List<DesignationDetails>();

                    //first element of list will always be current designation details
                    foreach (var item in empdesignationDetails)
                    {
                        var grade = (from g in dbContext.tbl_PM_GradeMaster
                                     where g.GradeID == item.Grade
                                     select g.Grade).FirstOrDefault();
                        model.DesignationDetailsList.Add(new DesignationDetails()
                        {
                            isDefaultRecord = item.isDefaultRecord,
                            //UniqueId = item.UniqueId,
                            Designation = item.Designation.ToString(),
                            Year = item.Year,
                            Month = item.Month.ToString(),
                            Grade = grade,
                            GradeId = item.Grade,
                            //Level = item.Level,
                            RoleDescription = item.RoleDesription,
                            JoiningDesignation = item.JoiningDesignation
                        });
                    }

                    return (model.DesignationDetailsList).Skip((page - 1) * rows).Take(rows).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteDesignationaryDetails(int? designationaryId, bool? isDefaultRecord, int? employeeId)
        {
            bool isDeleted = false;
            try
            {
                if (isDefaultRecord == false || isDefaultRecord == null)
                {
                    tbl_PM_EmployeeDesignation_Change details = dbContext.tbl_PM_EmployeeDesignation_Change.Where(ed => ed.UniqueID == designationaryId).FirstOrDefault();
                    if (details != null && details.EmployeeID > 0)
                    {
                        dbContext.DeleteObject(details);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                else
                {
                    HRMS_tbl_PM_Employee defaultdetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == employeeId).FirstOrDefault();
                    if (defaultdetails != null && defaultdetails.EmployeeID > 0)
                    {
                        defaultdetails.DesignationID = 0;
                        dbContext.SaveChanges();
                        isDeleted = true;
                        isDefaultRecord = false;
                    }
                }
                return isDeleted;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Role> GetEmployeeRole()
        {
            //ist<RepotingToList> resourcepool = new List<RepotingToList>();
            List<Role> resource1 = new List<Role>();
            try
            {
                resource1 = (from resource in dbContext.HRMS_tbl_PM_Role

                             select new Role
                             {
                                 RoleID = resource.RoleID,
                                 RoleDescription = resource.RoleDescription
                             }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return resource1.OrderBy(x => x.RoleDescription).ToList();
        }

        public HRMS_tbl_PM_Role GetRoleUser(int? employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                int RoleId = (from id in dbContext.HRMS_tbl_PM_Employee

                              where id.EmployeeID == employeeId
                              // where id.RoleID == roleId
                              select id.PostID.HasValue ? id.PostID.Value : 0).FirstOrDefault();

                if (RoleId > 0)
                {
                    HRMS_tbl_PM_Role LoginRole = (from name in dbContext.HRMS_tbl_PM_Role
                                                  where name.RoleID == RoleId
                                                  select name).FirstOrDefault();

                    return LoginRole;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetEmployeeID(string employeeCode)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                int employeeID = (from e in dbContext.HRMS_tbl_PM_Employee
                                  where e.EmployeeCode == employeeCode
                                  select e.EmployeeID).FirstOrDefault();
                return employeeID;
            }
            catch
            {
                throw;
            }
        }

        public int GetSelectedMonthNumber(DesignationDetails _model)
        {
            int selectedYear = Convert.ToInt32(_model.Year);
            string selectedMonth = _model.Month;
            DateTime joiningdate = GetEmployeeJoiningDate(_model.EmployeeId.HasValue ? _model.EmployeeId.Value : 0);
            int joinYear = joiningdate.Year;
            int joinMonth = joiningdate.Month;
            int currentYear = DateTime.Now.Year;

            if (selectedYear == joinYear || selectedYear == currentYear)
            {
                switch (selectedMonth.ToLower())
                {
                    case "january":
                        return 1;

                    case "february":
                        return 2;

                    case "march":
                        return 3;

                    case "april":
                        return 4;

                    case "may":
                        return 5;

                    case "june":
                        return 6;

                    case "july":
                        return 7;

                    case "august":
                        return 8;

                    case "september":
                        return 9;

                    case "october":
                        return 10;

                    case "november":
                        return 11;

                    case "december":
                        return 12;

                    default:
                        return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// to add new designation of employee or to current modify designation details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Designations SaveDesignationDetails(DesignationDetails model)
        {
            try
            {
                Designations response = new Designations();
                response.isAdded = false;
                response.isValidMonth = true;
                response.isValidEntry = true;
                var newDesignationId = dbContext.tbl_PM_DesignationMaster.Where(x => x.DesignationName == model.Designation).SingleOrDefault().DesignationID;
                var joiningDesignationId = dbContext.tbl_PM_DesignationMaster.Where(x => x.DesignationName == model.JoiningDesignation).SingleOrDefault().DesignationID;
                var currentDesignationDetails = dbContext.tbl_PM_EmployeeDesignation_Change.Where(x => x.EmployeeID == model.EmployeeId && x.UniqueID == model.UniqueId)
                                                        .OrderByDescending(x => x.Year).ThenBy(x => x.Month).FirstOrDefault();

                var DesignationId = dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeID == model.EmployeeId).FirstOrDefault();

                DateTime joiningdate = GetEmployeeJoiningDate(model.EmployeeId.HasValue ? model.EmployeeId.Value : 0);
                int joinYear = joiningdate.Year;
                int joinMonth = joiningdate.Month;
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                int selectedYear = Convert.ToInt32(model.Year);
                string selectedMonth = model.Month;

                int selectedMonthnumber = GetSelectedMonthNumber(model);
                if (selectedYear == joinYear && selectedMonthnumber < joinMonth && selectedMonthnumber != 0)
                {
                    response.isValidMonth = false;
                }
                else if (selectedYear == currentYear && selectedMonthnumber > currentMonth && selectedMonthnumber != 0)
                {
                    response.isValidMonth = false;
                }
                else
                {
                    List<tbl_PM_EmployeeDesignation_Change> designationList = dbContext.tbl_PM_EmployeeDesignation_Change.Where(x => x.EmployeeID == model.EmployeeId).ToList();
                    if (designationList.Any(emp => emp.EmployeeID == model.EmployeeId && emp.UniqueID != model.UniqueId && emp.Year == model.Year && emp.Month == model.Month))
                    {
                        response.isValidEntry = false;
                    }
                    else
                    {
                        if (currentDesignationDetails != null)
                        {
                            currentDesignationDetails.EmployeeID = model.EmployeeId;
                            currentDesignationDetails.Year = model.Year;
                            currentDesignationDetails.Month = model.Month;
                            if (model.Grade != null)
                                currentDesignationDetails.CurrentGradeID = int.Parse(model.Grade);
                            else
                                currentDesignationDetails.CurrentGradeID = null;

                            currentDesignationDetails.DesignationID = currentDesignationDetails.DesignationID;
                            currentDesignationDetails.Level = model.Level;
                            currentDesignationDetails.NewDesignationID = newDesignationId;
                            if (model.RoleDescription == null)
                            {
                                currentDesignationDetails.RoleDescription = " ";
                            }
                            else
                            {
                                currentDesignationDetails.RoleDescription = model.RoleDescription.Trim();
                            }
                            DesignationId.DesignationID = newDesignationId;
                            currentDesignationDetails.ModifiedDate = DateTime.Now;
                            //to update joining designation
                            var joiningDesignationDetails = dbContext.tbl_PM_EmployeeDesignation_Change.Where(x => x.EmployeeID == model.EmployeeId)
                                                                .OrderByDescending(x => x.Year).ThenBy(x => x.Month).ToList().Last();

                            var modifiedJoiningDesignation = dbContext.tbl_PM_DesignationMaster.Where(x => x.DesignationName == model.JoiningDesignation).SingleOrDefault().DesignationID;

                            if (joiningDesignationDetails.DesignationID != modifiedJoiningDesignation)
                            {
                                joiningDesignationDetails.DesignationID = modifiedJoiningDesignation;
                            }
                        }
                        else
                        {
                            tbl_PM_EmployeeDesignation_Change newDesignationDetails = new tbl_PM_EmployeeDesignation_Change();
                            HRMS_tbl_PM_Employee newDesignationEmpDetails = new HRMS_tbl_PM_Employee();
                            newDesignationDetails.EmployeeID = model.EmployeeId;
                            newDesignationDetails.Year = model.Year;
                            newDesignationDetails.Month = model.Month;
                            if (model.Grade != null)
                                newDesignationDetails.CurrentGradeID = int.Parse(model.Grade);
                            newDesignationDetails.ModifiedDate = DateTime.Now;
                            newDesignationDetails.DesignationID = joiningDesignationId;
                            newDesignationDetails.Level = model.Level;
                            newDesignationDetails.NewDesignationID = newDesignationId;
                            if (model.RoleDescription == null)
                            {
                                newDesignationDetails.RoleDescription = " ";
                            }
                            else
                            {
                                newDesignationDetails.RoleDescription = model.RoleDescription.Trim();
                            }
                            DesignationId.DesignationID = newDesignationId;
                            //newDesignationEmpDetails.DesignationID = newDesignationId;
                            dbContext.tbl_PM_EmployeeDesignation_Change.AddObject(newDesignationDetails);
                            //dbContext.HRMS_tbl_PM_Employee.AddObject(newDesignationEmpDetails);
                        }
                        dbContext.SaveChanges();
                        response.isAdded = true;
                        tbl_PM_EmployeeDesignation_Change designationListAfterSave = (from designationChange in dbContext.tbl_PM_EmployeeDesignation_Change
                                                                                      join months in dbContext.tbl_HR_Months on designationChange.Month equals months.MOnth into month
                                                                                      from monthList in month.DefaultIfEmpty()
                                                                                      orderby designationChange.Year descending, monthList.MonthID descending
                                                                                      where designationChange.EmployeeID == model.EmployeeId
                                                                                      select designationChange).FirstOrDefault();

                        HRMS_tbl_PM_Employee employeeDetails = dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeID == model.EmployeeId).FirstOrDefault();
                        if (employeeDetails != null)
                        {
                            employeeDetails.DesignationID = designationListAfterSave.NewDesignationID;
                            dbContext.SaveChanges();
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Checkdesignation(string designationName)
        {
            var designation = dbContext.tbl_PM_DesignationMaster.Where(x => x.DesignationName == designationName).FirstOrDefault();
            if (designation != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int? GetDesignationIdOfEmployee(int employeeId)
        {
            int? designationId =
                dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeID == employeeId)
                    .Select(x => x.DesignationID)
                    .FirstOrDefault();
            return designationId;
        }

        /// <summary>
        /// To Save Employee Details
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public bool SaveEmployeeDetails(EmployeeDetailsViewModel employee)
        {
            string trimRList = null;
            bool isAdded = false;
            bool empStatus = false;
            bool noStatus = false;
            try
            {
                var details = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == employee.EmployeeId).SingleOrDefault();
                var resourceName = dbContext.tbl_PM_ResourcePoolDetail.Where(x => x.EmployeeID == employee.EmployeeId).SingleOrDefault();

                var user = Membership.GetUser(details.EmployeeCode);
                if (user == null)
                {
                    string password = "Mail_098";
                    Membership.CreateUser(details.EmployeeCode, password);
                }
                string rlist = employee.SelectedRolesList;
                if (rlist != null)
                {
                    trimRList = rlist.TrimEnd(',');
                }
                //added to to make status of employee active if his EmployeeStatusMaster is ACTIVE else INACTIVE
                if (!string.IsNullOrEmpty(employee.EmployeeStatusMaster))
                {
                    if (int.Parse(employee.EmployeeStatusMaster) == 1)
                        empStatus = Convert.ToBoolean(0);
                    else
                        empStatus = Convert.ToBoolean(1);

                    noStatus = false;
                }
                else
                    noStatus = true;

                if (employee.RejoinedWithingOneYear == true)
                {
                    employee.ConfirmationDate = employee.JoiningDate;
                    employee.EmployeeStatus = "1";
                }

                if (details == null && resourceName == null)
                {
                    DateTime rtime = DateTime.MinValue;
                    int sid = 0;
                    if (!string.IsNullOrEmpty(employee.Shift))
                    {
                        sid = int.Parse(employee.Shift);
                        var reportingTime = dbPmsContext.ShiftMasters.Where(ed => ed.ShiftID == sid).SingleOrDefault();
                        rtime = reportingTime.ShiftInTime.Value;
                    }
                    else
                    {
                        sid = 1;
                        var reportingTime = dbPmsContext.ShiftMasters.Where(ed => ed.ShiftID == sid).SingleOrDefault();
                        rtime = reportingTime.ShiftInTime.Value;
                    }

                    HRMS_tbl_PM_Employee employeeDetails = new HRMS_tbl_PM_Employee();
                    employeeDetails.EmployeeCode = employee.EmployeeCode;
                    if (!string.IsNullOrEmpty(employee.EmployeeStatusMaster))
                        employeeDetails.EmployeeStatusMasterID = int.Parse(employee.EmployeeStatusMaster);
                    else
                        employeeDetails.EmployeeStatusMasterID = null;
                    if (!string.IsNullOrEmpty(employee.EmployeeStatus))
                        employeeDetails.EmployeeStatusID = int.Parse(employee.EmployeeStatus);
                    else
                        employeeDetails.EmployeeStatusID = null;
                    employeeDetails.IsBillable = employee.BillableStatus;
                    if (noStatus == false)
                        employeeDetails.Status = empStatus;
                    else
                        employeeDetails.Status = null;
                    if (!string.IsNullOrEmpty(employee.CommitmentsMade))
                        employeeDetails.Commitments_Made = employee.CommitmentsMade.Trim();
                    else
                        employeeDetails.Commitments_Made = employee.CommitmentsMade;
                    employeeDetails.JoiningDate = employee.JoiningDate;
                    employeeDetails.LeavingDate = employee.ExitDate;
                    employeeDetails.ProbationPeriod = employee.Months;
                    employeeDetails.Probation_Review_Date = employee.ProbationReviewDate;
                    employeeDetails.ConfirmationDate = employee.ConfirmationDate;
                    if (!string.IsNullOrEmpty(employee.Group))
                        employeeDetails.BusinessGroupID = Convert.ToInt32(employee.Group);
                    else
                        employeeDetails.BusinessGroupID = null;

                    employeeDetails.RejoinedWithinYear = employee.RejoinedWithingOneYear;
                    employeeDetails.Current_DU = employee.CurrentDU;

                    if (!string.IsNullOrEmpty(employee.ParentDU))
                        employeeDetails.ResourcePoolID = int.Parse(employee.ParentDU);
                    else
                        employeeDetails.ResourcePoolID = null;
                    if (employee.LastYearAppraisal != null && employee.LastYearAppraisal != "")
                        employeeDetails.L_Y_Appraisal_Score = employee.LastYearAppraisal.Trim();
                    else
                        employeeDetails.L_Y_Appraisal_Score = employee.LastYearAppraisal;

                    if (employee.LastYearPromotion != null && employee.LastYearPromotion != "")
                        employeeDetails.L_Y_Promotion_Status = employee.LastYearPromotion.Trim();
                    else
                        employeeDetails.L_Y_Promotion_Status = employee.LastYearPromotion;

                    if (employee.LastYearIncrement != null && employee.LastYearIncrement != "")
                        employeeDetails.L_Y_Increment = employee.LastYearIncrement.Trim();
                    else
                        employeeDetails.L_Y_Increment = employee.LastYearIncrement;

                    if (!string.IsNullOrEmpty(employee.OrganizationUnit))
                        employeeDetails.LocationID = int.Parse(employee.OrganizationUnit);
                    else
                        employeeDetails.LocationID = null;
                    if (!string.IsNullOrEmpty(employee.OfficeLocation))
                        employeeDetails.OfficeLocation = int.Parse(employee.OfficeLocation);
                    else
                        employeeDetails.OfficeLocation = null;
                    if (!string.IsNullOrEmpty(employee.DT))
                        employeeDetails.GroupID = int.Parse(employee.DT);
                    else
                        employeeDetails.GroupID = null;
                    if (!string.IsNullOrEmpty(employee.RecruiterName))
                        employeeDetails.Recruiter_Name = employee.RecruiterName.Trim();
                    else
                        employeeDetails.Recruiter_Name = employee.RecruiterName;
                    if (employee.Region != null && employee.Region != "")
                        employeeDetails.Region = employee.Region.Trim();
                    else
                        employeeDetails.Region = employee.Region;

                    if (employee.ESICNo != null && employee.ESICNo != "")
                        employeeDetails.ESICNo = employee.ESICNo.Trim();
                    else
                        employeeDetails.ESICNo = employee.ESICNo;

                    if (employee.PFNo != null && employee.PFNo != "")
                        employeeDetails.PFNo = employee.PFNo.Trim();
                    else
                        employeeDetails.PFNo = employee.PFNo;

                    employeeDetails.ReportingTo = employee.ExitConfirmationManagerId_Emp;
                    employeeDetails.CompetencyManager = employee.CompetencyManagerId_Emp;
                    employeeDetails.CostCenterID = employee.ReportingToId_Emp;
                    if (employee.IncomeTaxNo != null && employee.IncomeTaxNo != "")
                        employeeDetails.IncomeTaxNo = employee.IncomeTaxNo.Trim();
                    else
                        employeeDetails.IncomeTaxNo = employee.IncomeTaxNo;
                    if (!string.IsNullOrEmpty(employee.CalenderName))
                        employeeDetails.CalendarLocationId = int.Parse(employee.CalenderName);
                    else
                        employeeDetails.CalendarLocationId = null;
                    if (!string.IsNullOrEmpty(employee.Shift))
                        employeeDetails.ShiftID = int.Parse(employee.Shift);
                    else
                        employeeDetails.ShiftID = 0;
                    employeeDetails.ReportingTime = Convert.ToDateTime(rtime.TimeOfDay.ToString());
                    employeeDetails.PostID = employee.OrgRoleID;

                    tbl_PM_ResourcePoolDetail resource = new tbl_PM_ResourcePoolDetail();
                    resource.EmployeeID = employee.EmployeeId;
                    if (!string.IsNullOrEmpty(employee.ResourcePoolName))
                        resource.ResourcePoolID = int.Parse(employee.ResourcePoolName);
                    else
                        resource.ResourcePoolID = null;

                    if (!string.IsNullOrEmpty(trimRList))
                    {
                        string[] eachRole = trimRList.Split(',');
                        string currentRole = "";
                        foreach (string str in eachRole)
                        {
                            currentRole = GetSpecificLoginRolesDetails(str);
                            Roles.AddUserToRole(employee.EmployeeCode, currentRole);
                        }
                    }
                    dbContext.HRMS_tbl_PM_Employee.AddObject(employeeDetails);
                    dbContext.tbl_PM_ResourcePoolDetail.AddObject(resource);
                }
                else
                {
                    DateTime rtime = DateTime.MinValue;
                    int sid = 0;
                    if (!string.IsNullOrEmpty(employee.Shift))
                    {
                        sid = int.Parse(employee.Shift);
                        var reportingTime = dbPmsContext.ShiftMasters.Where(ed => ed.ShiftID == sid).SingleOrDefault();
                        rtime = reportingTime.ShiftInTime.Value;
                    }
                    else
                    {
                        sid = 1;
                        var reportingTime = dbPmsContext.ShiftMasters.Where(ed => ed.ShiftID == sid).SingleOrDefault();
                        rtime = reportingTime.ShiftInTime.Value;
                    }
                    details.IsLDAPAuthentication = false;
                    details.EmployeeCode = employee.EmployeeCode;
                    if (!string.IsNullOrEmpty(employee.EmployeeStatusMaster))
                        details.EmployeeStatusMasterID = int.Parse(employee.EmployeeStatusMaster);
                    else
                        details.EmployeeStatusMasterID = null;
                    if (!string.IsNullOrEmpty(employee.EmployeeStatus))
                        details.EmployeeStatusID = int.Parse(employee.EmployeeStatus);
                    else
                        details.EmployeeStatusID = null;
                    if (noStatus == false)
                        details.Status = empStatus;
                    else
                        details.Status = null;
                    details.IsBillable = employee.BillableStatus;
                    if (!string.IsNullOrEmpty(employee.CommitmentsMade))
                        details.Commitments_Made = employee.CommitmentsMade.Trim();
                    else
                        details.Commitments_Made = employee.CommitmentsMade;
                    details.JoiningDate = employee.JoiningDate;
                    details.LeavingDate = employee.ExitDate;
                    details.ProbationPeriod = employee.Months;
                    details.Probation_Review_Date = employee.ProbationReviewDate;
                    details.ConfirmationDate = employee.ConfirmationDate;
                    details.RejoinedWithinYear = employee.RejoinedWithingOneYear;

                    if (!string.IsNullOrEmpty(employee.Group))
                        details.BusinessGroupID = Convert.ToInt32(employee.Group);
                    else
                        details.BusinessGroupID = null;
                    details.Current_DU = employee.CurrentDU;
                    if (employee.ParentDU != null)
                        details.ResourcePoolID = int.Parse(employee.ParentDU);
                    else
                        details.ResourcePoolID = null;

                    if (employee.LastYearAppraisal != null && employee.LastYearAppraisal != "")
                        details.L_Y_Appraisal_Score = employee.LastYearAppraisal.Trim();
                    else
                        details.L_Y_Appraisal_Score = employee.LastYearAppraisal;

                    if (employee.LastYearPromotion != null && employee.LastYearPromotion != "")
                        details.L_Y_Promotion_Status = employee.LastYearPromotion.Trim();
                    else
                        details.L_Y_Promotion_Status = employee.LastYearPromotion;

                    if (employee.LastYearIncrement != null && employee.LastYearIncrement != "")
                        details.L_Y_Increment = employee.LastYearIncrement.Trim();
                    else
                        details.L_Y_Increment = employee.LastYearIncrement;

                    if (!string.IsNullOrEmpty(employee.OrganizationUnit))
                        details.LocationID = int.Parse(employee.OrganizationUnit);
                    else
                        details.LocationID = null;
                    if (!string.IsNullOrEmpty(employee.OfficeLocation))
                        details.OfficeLocation = int.Parse(employee.OfficeLocation);
                    else
                        details.OfficeLocation = null;
                    if (!string.IsNullOrEmpty(employee.DT))
                        details.GroupID = int.Parse(employee.DT);
                    else
                        details.GroupID = null;
                    if (!string.IsNullOrEmpty(employee.RecruiterName))
                        details.Recruiter_Name = employee.RecruiterName.Trim();
                    else
                        details.Recruiter_Name = employee.RecruiterName;
                    if (employee.Region != null && employee.Region != "")
                        details.Region = employee.Region.Trim();
                    else
                        details.Region = employee.Region;

                    if (employee.ESICNo != null && employee.ESICNo != "")
                        details.ESICNo = employee.ESICNo.Trim();
                    else
                        details.ESICNo = employee.ESICNo;

                    if (employee.PFNo != null && employee.PFNo != "")
                        details.PFNo = employee.PFNo.Trim();
                    else
                        details.PFNo = employee.PFNo;

                    if (employee.IncomeTaxNo != null && employee.IncomeTaxNo != "")
                        details.IncomeTaxNo = employee.IncomeTaxNo.Trim();
                    else
                        details.IncomeTaxNo = employee.IncomeTaxNo;

                    if (!string.IsNullOrEmpty(employee.CalenderName))
                        details.CalendarLocationId = int.Parse(employee.CalenderName);
                    else
                        details.CalendarLocationId = null;
                    if (!string.IsNullOrEmpty(employee.Shift))
                        details.ShiftID = int.Parse(employee.Shift);
                    else
                        details.ShiftID = 0;

                    details.ReportingTime = Convert.ToDateTime(rtime.TimeOfDay.ToString());
                    details.PostID = employee.OrgRoleID;
                    details.ReportingTo = employee.ExitConfirmationManagerId_Emp;
                    details.CompetencyManager = employee.CompetencyManagerId_Emp;
                    details.CostCenterID = employee.ReportingToId_Emp;

                    if (!string.IsNullOrEmpty(employee.EmployeeStatusMaster))
                    {
                        if (int.Parse(employee.EmployeeStatusMaster) == 2)
                        {
                            MemberInactive = true;
                            aspnet_Membership _Membership = (from m in dbv2toolsContext.aspnet_Users
                                                             join roleID in dbv2toolsContext.aspnet_Membership on m.UserId equals roleID.UserId
                                                             where m.UserName == employee.EmployeeCode
                                                             select roleID).FirstOrDefault();
                            _Membership.IsLockedOut = MemberInactive;
                            dbv2toolsContext.SaveChanges();
                        }
                        if (int.Parse(employee.EmployeeStatusMaster) == 1)
                        {
                            MemberInactive = false;
                            aspnet_Membership _Membership = (from m in dbv2toolsContext.aspnet_Users
                                                             join roleID in dbv2toolsContext.aspnet_Membership on m.UserId equals roleID.UserId
                                                             where m.UserName == employee.EmployeeCode
                                                             select roleID).FirstOrDefault();
                            _Membership.IsLockedOut = MemberInactive;
                            dbv2toolsContext.SaveChanges();
                        }
                    }

                    if (resourceName == null)
                    {
                        tbl_PM_ResourcePoolDetail resource = new tbl_PM_ResourcePoolDetail();
                        resource.EmployeeID = employee.EmployeeId;
                        if (!string.IsNullOrEmpty(employee.ResourcePoolName))
                            resource.ResourcePoolID = int.Parse(employee.ResourcePoolName);
                        else
                            resource.ResourcePoolID = null;

                        dbContext.tbl_PM_ResourcePoolDetail.AddObject(resource);
                    }
                    else
                    {
                        var empidSEM = (from empSEM in dbSEMContext.tbl_PM_Employee_SEM
                                        where empSEM.EmployeeCode == employee.EmployeeCode
                                        select empSEM.EmployeeID).FirstOrDefault();

                        List<tbl_PM_ResourcePoolDetail_SEM> resourceNameSEM =
                            dbSEMContext.tbl_PM_ResourcePoolDetail_SEM.Where(x => x.EmployeeID == empidSEM).ToList();

                        foreach (var tblPmResourcePoolDetailSem in resourceNameSEM)
                        {
                            dbSEMContext.tbl_PM_ResourcePoolDetail_SEM.DeleteObject(tblPmResourcePoolDetailSem);
                        }
                        dbSEMContext.SaveChanges();
                        if (!string.IsNullOrEmpty(employee.ResourcePoolName))
                            resourceName.ResourcePoolID = int.Parse(employee.ResourcePoolName);
                        else
                            resourceName.ResourcePoolID = null;
                    }
                    if (!string.IsNullOrEmpty(trimRList))
                    {
                        string[] role = Roles.GetRolesForUser(employee.EmployeeCode);
                        string[] eachRole = trimRList.Split(',');
                        string currentRole = "";
                        if (role.Count() != 0)
                        {
                            foreach (string str in role)
                            {
                                Roles.RemoveUserFromRole(employee.EmployeeCode, str);
                            }

                            foreach (string str in eachRole)
                            {
                                currentRole = GetSpecificLoginRolesDetails(str);
                                Roles.AddUserToRole(employee.EmployeeCode, currentRole);
                            }
                        }
                        else
                        {
                            foreach (string str in eachRole)
                            {
                                currentRole = GetSpecificLoginRolesDetails(str);
                                Roles.AddUserToRole(employee.EmployeeCode, currentRole);
                            }
                        }
                    }
                }
                dbContext.SaveChanges();
                isAdded = true;
            }
            catch (Exception)
            {
                throw;
            }
            return isAdded;
        }

        /// <summary>
        /// to get the all  details of disciplinary on disciplinaryDetails tab click
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<EmployeeDisciplinaryDetailsViewModel> GetEmployeeDisciplinaryDetailsTabclick(int employeeId)
        {
            List<EmployeeDisciplinaryDetailsViewModel> DisciplinaryDetails = new List<EmployeeDisciplinaryDetailsViewModel>();
            try
            {
                DisciplinaryDetails = (from disciplinaryDetails in dbContext.Tbl_PM_Disciplinary

                                       join CreatedByUserName in dbContext.HRMS_tbl_PM_Employee
                                       on disciplinaryDetails.CreatedBy equals CreatedByUserName.EmployeeCode
                                       where disciplinaryDetails.EmployeeId == employeeId
                                       select new EmployeeDisciplinaryDetailsViewModel
                                       {
                                           CreatedByUserId = disciplinaryDetails.CreatedBy,
                                           CreatedByUserName = CreatedByUserName.EmployeeName,
                                           AddedDate = disciplinaryDetails.Date,
                                           DisciplineId = disciplinaryDetails.DisciplinaryId,
                                           DisciplineMessage = disciplinaryDetails.Message,
                                           DisciplineSubject = disciplinaryDetails.subject,
                                           EmployeeId = disciplinaryDetails.EmployeeId,
                                           ManagerId = disciplinaryDetails.ManagerId
                                       }).ToList();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return DisciplinaryDetails;
        }

        //get logged in user ID for Created by field in Tbl_PM_Disciplinary table
        public string GetLoginUserId(int desciplinaryId)
        {
            string LoginId;
            try
            {
                LoginId = (from loginid in dbContext.Tbl_PM_Disciplinary
                           where loginid.DisciplinaryId == desciplinaryId
                           select loginid.CreatedBy).SingleOrDefault();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return LoginId;
        }

        /// <summary>
        /// method will retrive the disciplinary details total count based on employeeId passed.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public int GetEmployeeDisciplinaryDetailsTotalCount(int employeeId)
        {
            List<EmployeeDisciplinaryDetailsViewModel> DisciplinaryDetails = new List<EmployeeDisciplinaryDetailsViewModel>();
            try
            {
                DisciplinaryDetails = (from disciplinaryDetails in dbContext.Tbl_PM_Disciplinary

                                       where disciplinaryDetails.EmployeeId == employeeId
                                       select new EmployeeDisciplinaryDetailsViewModel
                                       {
                                           AddedDate = disciplinaryDetails.Date,
                                           DisciplineId = disciplinaryDetails.DisciplinaryId,
                                           DisciplineMessage = disciplinaryDetails.Message,
                                           DisciplineSubject = disciplinaryDetails.subject,
                                           EmployeeId = disciplinaryDetails.EmployeeId,
                                           ManagerId = disciplinaryDetails.ManagerId
                                       }).ToList();

                return DisciplinaryDetails.Count;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        /// <summary>
        /// method will retrive the disciplinary details based on employeeId passed.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<EmployeeDisciplinaryDetailsViewModel> GetEmployeeDisciplinaryDetails(int page, int rows, int employeeId)
        {
            List<EmployeeDisciplinaryDetailsViewModel> DisciplinaryDetails = new List<EmployeeDisciplinaryDetailsViewModel>();
            try
            {
                DisciplinaryDetails = (from disciplinaryDetails in dbContext.Tbl_PM_Disciplinary

                                       orderby disciplinaryDetails.DisciplinaryId descending
                                       where disciplinaryDetails.EmployeeId == employeeId
                                       select new EmployeeDisciplinaryDetailsViewModel
                                   {
                                       CreatedByUserId = disciplinaryDetails.CreatedBy,
                                       //for getting created by name of disciplinaryDetails
                                       CreatedByUserName = (from e in dbContext.HRMS_tbl_PM_Employee
                                                            where e.EmployeeCode == disciplinaryDetails.CreatedBy
                                                            select e.EmployeeName).FirstOrDefault(),

                                       AddedDate = disciplinaryDetails.Date,
                                       DisciplineId = disciplinaryDetails.DisciplinaryId,
                                       DisciplineMessage = disciplinaryDetails.Message,
                                       DisciplineSubject = disciplinaryDetails.subject,
                                       EmployeeId = disciplinaryDetails.EmployeeId,
                                       ManagerId = disciplinaryDetails.ManagerId
                                   }).ToList();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return ((DisciplinaryDetails).Skip((page - 1) * rows).Take(rows).ToList());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<Tbl_PM_Disciplinary> GetAllDisciplinaryDetails(int page, int rows)
        {
            List<Tbl_PM_Disciplinary> DisciplinaryDetailsList = (from educationDetails in dbContext.Tbl_PM_Disciplinary orderby educationDetails.EmployeeId select educationDetails).Skip((page - 1) * rows).Take(rows).ToList();
            return DisciplinaryDetailsList;
        }

        public int GetAllDisciplinaryDetailsTotalCount()
        {
            int totalCount = 0;
            try
            {
                totalCount = (from DisciplinaryDetails in dbContext.Tbl_PM_Disciplinary select DisciplinaryDetails.DisciplinaryId).Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return totalCount;
        }

        public DateTime GetEmployeeJoiningDate(int employeeId)
        {
            DateTime joiningDate;
            try
            {
                joiningDate = Convert.ToDateTime(dbContext.HRMS_tbl_PM_Employee.Where(em => em.EmployeeID == employeeId).Select(em => em.JoiningDate).SingleOrDefault());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return joiningDate;
        }

        public bool DeleteDisciplinaryDetails(int desciplinaryId)
        {
            bool isDeleted = false;
            try
            {
                Tbl_PM_Disciplinary details = dbContext.Tbl_PM_Disciplinary.Where(ed => ed.DisciplinaryId == desciplinaryId).FirstOrDefault();
                if (details != null && details.EmployeeId > 0)
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

        /// <summary>
        /// Method will give us ManagerId from Managername
        /// </summary>
        /// <param name="ManagerId"></param>
        /// <returns></returns>
        public int GetManagerIdFromManagerName(string managaerName)
        {
            int ManagerId;
            try
            {
                ManagerId = (from Mid in dbContext.HRMS_tbl_PM_Employee
                             where Mid.EmployeeName == managaerName
                             select Mid.EmployeeID).FirstOrDefault();
                return (ManagerId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to retrive manager name from managerId
        /// </summary>
        /// <param name="managaerId"></param>
        /// <returns></returns>
        public string GetManagerNameFromManagerId(int managaerId)
        {
            string ManagerName = string.Empty;
            try
            {
                ManagerName = (from Mname in dbContext.HRMS_tbl_PM_Employee
                               where Mname.EmployeeID == managaerId
                               select Mname.EmployeeName).FirstOrDefault();
                return (ManagerName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method will return model containing Disciplinary details of respective disciplineId
        /// </summary>
        /// <param name="disciplineId"></param>
        /// <returns></returns>
        public EmployeeDisciplinaryDetailsViewModel GetDisciplineDetailsFromDisciplineId(int disciplineId)
        {
            EmployeeDisciplinaryDetailsViewModel DisciplineDetails = new EmployeeDisciplinaryDetailsViewModel();
            try
            {
                DisciplineDetails = (from ddetails in dbContext.Tbl_PM_Disciplinary
                                     where ddetails.DisciplinaryId == disciplineId
                                     select new EmployeeDisciplinaryDetailsViewModel
                                     {
                                         AddedDate = ddetails.Date,
                                         DisciplineId = ddetails.DisciplinaryId,
                                         DisciplineMessage = ddetails.Message,
                                         DisciplineSubject = ddetails.subject,
                                         EmployeeId = ddetails.EmployeeId,
                                         ManagerId = ddetails.ManagerId
                                     }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return (DisciplineDetails);
        }

        /// <summary>
        /// will add & update the discipline details to database
        /// </summary>
        /// <param name="empDiscplines"></param>
        /// <returns></returns>
        public bool AddUpdateEmployeeDisciplines(Tbl_PM_Disciplinary empDiscplines)
        {
            bool isAdded = false;
            try
            {
                Tbl_PM_Disciplinary emp = dbContext.Tbl_PM_Disciplinary.Where(ed => ed.DisciplinaryId == empDiscplines.DisciplinaryId).FirstOrDefault();
                if (emp == null || emp.DisciplinaryId <= 0)
                {
                    dbContext.Tbl_PM_Disciplinary.AddObject(empDiscplines);
                    dbContext.SaveChanges();
                }
                else
                {
                    emp.CreatedBy = empDiscplines.CreatedBy;
                    emp.EmployeeId = empDiscplines.EmployeeId;
                    emp.Date = empDiscplines.Date;
                    emp.Message = empDiscplines.Message.Trim();
                    emp.subject = empDiscplines.subject.Trim();
                    emp.ManagerId = empDiscplines.ManagerId;
                    dbContext.SaveChanges();
                }

                isAdded = true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return isAdded;
        }

        public bool AddEmployeeDetails(HRMS_tbl_PM_Employee employee)
        {
            try
            {
                bool isAdded = false;
                HRMS_tbl_PM_Employee details = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeID == employee.EmployeeID).FirstOrDefault();
                if (details == null || details.EmployeeID <= 0)
                {
                    dbContext.HRMS_tbl_PM_Employee.AddObject(employee);
                    dbContext.SaveChanges();
                }
                else
                {
                    details.IsLDAPAuthentication = true;
                    details.EmployeeCode = employee.EmployeeCode;
                    details.EmployeeStatusID = employee.EmployeeStatusID;
                    //Status
                    details.Commitments_Made = employee.Commitments_Made;
                    details.JoiningDate = employee.JoiningDate;
                    details.LeavingDate = employee.LeavingDate;
                    details.ProbationPeriod = employee.ProbationPeriod;
                    details.Probation_Review_Date = employee.Probation_Review_Date;
                    details.ConfirmationDate = employee.ConfirmationDate;
                    //ContractEmployee
                    //Group
                    details.Current_DU = employee.Current_DU;
                    details.ResourcePoolID = employee.ResourcePoolID;
                    details.L_Y_Appraisal_Score = employee.L_Y_Appraisal_Score;
                    details.L_Y_Promotion_Status = employee.L_Y_Promotion_Status;
                    details.L_Y_Increment = employee.L_Y_Increment;
                    details.LocationID = employee.LocationID;
                    details.GroupID = employee.GroupID;
                    details.Recruiter_Name = employee.Recruiter_Name;
                    details.Region = employee.Region;
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
        /// method to retrieve designation names
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public List<Designations> GetDesignations(string searchText)
        {
            List<Designations> designations = new List<Designations>();
            try
            {
                designations = (from designation in dbContext.tbl_PM_DesignationMaster
                                where designation.DesignationName.Contains(searchText)
                                orderby designation.DesignationName
                                select new Designations
                                {
                                    DesignationId = designation.DesignationID,
                                    DesignationName = designation.DesignationName
                                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return designations;
        }

        /// <summary>
        /// method to retrieve country names
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public List<CountryDetails> GetCountryList()
        {
            List<CountryDetails> countryDetailsList = new List<CountryDetails>();
            try
            {
                countryDetailsList = (from country in dbContext.tbl_PM_CountryMaster
                                      orderby country.CountryName
                                      select new CountryDetails
                                      {
                                          CountryId = country.CountryID,
                                          CountryName = country.CountryName
                                      }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return countryDetailsList;
        }

        public List<VisaTypeForEmployeeDetails> GetVisaTypeDetails()
        {
            List<VisaTypeForEmployeeDetails> VisaType = new List<VisaTypeForEmployeeDetails>();
            try
            {
                VisaType = (from visa in dbContext.tbl_PM_VisaTypeMaster
                            orderby visa.VisaType
                            select new VisaTypeForEmployeeDetails
                            {
                                VisaTypeID = visa.VisaTypeID,
                                VisaTypeName = visa.VisaType
                            }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return VisaType;
        }

        public List<VisaDetailsViewModel> GetVisaDetails(int page, int rows, int employeeId, TravelDetailsPerson type)
        {
            List<VisaDetailsViewModel> visaDetailsList = new List<VisaDetailsViewModel>();
            try
            {
                if (type == TravelDetailsPerson.Own)
                {
                    visaDetailsList = (from visaDetails in dbContext.tbl_PM_EmployeeVisaDetails
                                       where visaDetails.EmployeeID == employeeId
                                       select new VisaDetailsViewModel
                                       {
                                           EmployeeId = visaDetails.EmployeeID.HasValue ? visaDetails.EmployeeID.Value : 0,
                                           EmployeeVisaId = visaDetails.EmployeeVisaID,
                                           IsValidVisa = visaDetails.IsValid.HasValue ? visaDetails.IsValid.Value : false,
                                           ValidTill = visaDetails.ValidUpto,
                                           SelectedCountryId = visaDetails.CountryID.HasValue ? visaDetails.CountryID.Value : 0,
                                           Country = dbContext.tbl_PM_CountryMaster.Where(m => m.CountryID == visaDetails.CountryID).Select(m => m.CountryName).FirstOrDefault(),
                                           VisaTypeID = visaDetails.VisaTypeID.HasValue ? visaDetails.VisaTypeID.Value : 0,
                                           VisaTypeName = dbContext.tbl_PM_VisaTypeMaster.Where(m => m.VisaTypeID == visaDetails.VisaTypeID).Select(m => m.VisaType).FirstOrDefault(),
                                           IsVisaExpired = visaDetails.ValidUpto < DateTime.Now ? true : false,
                                           VisaFileName = visaDetails.VisaFileName,
                                           VisaFilePath = visaDetails.VisaFilePath
                                       }).ToList();
                }
                if (type == TravelDetailsPerson.Spouse)
                {
                    string spouseMaritalStatus = (from spouseStatus in dbContext.HRMS_tbl_PM_Employee
                                                  where spouseStatus.EmployeeID == employeeId
                                                  select spouseStatus.MaritalStatus).FirstOrDefault();

                    visaDetailsList = (from visaDetails in dbContext.tbl_PM_DependandsVisaDetails
                                       where visaDetails.EmployeeID == employeeId && spouseMaritalStatus == "Married"
                                       select new VisaDetailsViewModel
                                       {
                                           DependantVisaDetailsId = visaDetails.DependandsVisaDetailsID,
                                           EmployeeId = visaDetails.EmployeeID.HasValue ? visaDetails.EmployeeID.Value : 0,
                                           IsValidVisa = visaDetails.IsValid.HasValue ? visaDetails.IsValid.Value : false,
                                           ValidTill = visaDetails.ValidUpto,
                                           SelectedCountryId = visaDetails.CountryID.HasValue ? visaDetails.CountryID.Value : 0,
                                           Country = dbContext.tbl_PM_CountryMaster.Where(m => m.CountryID == visaDetails.CountryID).Select(m => m.CountryName).FirstOrDefault(),
                                           VisaTypeID = visaDetails.VisaTypeID.HasValue ? visaDetails.VisaTypeID.Value : 0,
                                           VisaTypeName = dbContext.tbl_PM_VisaTypeMaster.Where(m => m.VisaTypeID == visaDetails.VisaTypeID).Select(m => m.VisaType).FirstOrDefault(),
                                           IsVisaExpired = visaDetails.ValidUpto < DateTime.Now ? true : false,
                                           VisaFileName = visaDetails.VisaFileName,
                                           VisaFilePath = visaDetails.VisaFilePath
                                       }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return visaDetailsList.Skip((page - 1) * rows).Take(rows).ToList();
        }

        public int GetVisaDetailsTotalCount(int employeeId, TravelDetailsPerson type)
        {
            int totalCount = 0;
            try
            {
                if (type == TravelDetailsPerson.Own)
                {
                    totalCount = (from visaDetails in dbContext.tbl_PM_EmployeeVisaDetails
                                  where visaDetails.EmployeeID == employeeId
                                  select visaDetails.EmployeeVisaID).Count();
                }
                if (type == TravelDetailsPerson.Spouse)
                {
                    totalCount = (from visaDetails in dbContext.tbl_PM_DependandsVisaDetails
                                  join dependentDetails in dbContext.tbl_PM_Employee_Dependands on visaDetails.DependandsID equals dependentDetails.DependandsID
                                  where dependentDetails.EmployeeID == employeeId && (dependentDetails.RelationType == "Wife" || dependentDetails.RelationType == "Husband")
                                  select visaDetails.DependandsVisaDetailsID).Count();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return totalCount;
        }

        public List<ProjectDetailsViewModel> GetProjectDetailsByEmployeeId(int page, int rows, int employeeId, out int totalCount)
        {
            HRMS_tbl_PM_Employee employee = GetEmployeeDetails(employeeId);
            int employeecode = Convert.ToInt32(employee.EmployeeCode);

            DateTime System_dt = DateTime.Now.Date;

            List<ProjectDetailsViewModel> ProjectsList = new List<ProjectDetailsViewModel>();
            try
            {
                //var pmslist = (from EmployeeDetail in dbpms3Context.Project_Resource_Mapping
                //               where EmployeeDetail.UserID == employeecode && EmployeeDetail.StartDate <= System_dt && (EmployeeDetail.EndDate >= System_dt || EmployeeDetail.EndDate == null)
                //               select new
                //               {
                //                   EmployeeDetail.Project_ResourceID,
                //                   EmployeeDetail.ProjectID,
                //                   EmployeeDetail.UserID,
                //                   EmployeeDetail.StartDate,
                //                   EmployeeDetail.EndDate,
                //                   EmployeeDetail.RoleID
                //               }).ToList();

                var SEMEmpId = (from SEMEmpDtls in dbSEMContext.tbl_PM_Employee_SEM
                                where SEMEmpDtls.EmployeeCode == employee.EmployeeCode
                                select SEMEmpDtls.EmployeeID).FirstOrDefault();

                var semlist = (from projectDetails in dbSEMContext.tbl_PM_ProjectEmployeeRole
                               join prj in dbSEMContext.tbl_PM_Project on projectDetails.ProjectID equals prj.ProjectID
                               where projectDetails.EmployeeID == SEMEmpId && projectDetails.ExpectedStartDate <= System_dt && prj.GlobalProject != true && (projectDetails.ActualEndDate >= System_dt ||
                               (projectDetails.ActualEndDate == null && projectDetails.ExpectedEndDate >= System_dt) || (projectDetails.ActualEndDate == null && projectDetails.ExpectedEndDate == null))
                               select new
                                   {
                                       projectDetails.ProjectEmployeeRoleID,
                                       projectDetails.ProjectID,
                                       //UserID = (from emp in dbContext.HRMS_tbl_PM_Employee
                                       //          where emp.EmployeeID == projectDetails.EmployeeID
                                       //          select emp.EmployeeCode),
                                       projectDetails.ExpectedStartDate,
                                       projectDetails.ActualEndDate,
                                       projectDetails.ExpectedEndDate,
                                       //EndDate = projectDetails.ActualEndDate.HasValue ? projectDetails.ActualEndDate : projectDetails.ExpectedEndDate,
                                       projectDetails.Role
                                   }).ToList();

                //if (pmslist.Count != 0)
                //{
                //    totalCount = dbpms3Context.Project_Resource_Mapping.Where(x => x.UserID == employeecode).Count();
                //}
                //else
                {
                    //totalCount = dbSEMContext.tbl_PM_ProjectEmployeeRole.Where(x => x.EmployeeID == employeeId).Count();
                    totalCount = semlist.Count();
                }

                //foreach (var project in pmslist)
                //{
                //    ProjectDetailsViewModel projectdetail = new ProjectDetailsViewModel();

                //    projectdetail.ProjectDetailID = Convert.ToInt32(project.ProjectID);

                //    //projectdetail.EmployeeId = (from emp in dbContext.HRMS_tbl_PM_Employee
                //    //                            where emp.EmployeeCode == project.UserID
                //    //                            select emp.EmployeeID).FirstOrDefault();

                //    projectdetail.ProjectResourceID = Convert.ToInt32(project.Project_ResourceID);

                //    projectdetail.CurrentProject = (from proj in dbpms3Context.ProjectMasters
                //                                    where proj.ProjectID == project.ProjectID
                //                                    select proj.ProjectName).FirstOrDefault();

                //    var reportingto = (from proj in dbContext.HRMS_tbl_PM_Employee
                //                       where proj.EmployeeID == employeeId
                //                       select proj.CostCenterID).FirstOrDefault();

                //    projectdetail.CurrentReportingManager = (from emp in dbContext.HRMS_tbl_PM_Employee
                //                                             where emp.EmployeeID == reportingto
                //                                             select emp.EmployeeName).FirstOrDefault();
                //    //changed here

                //    projectdetail.CurrentRole = (from proj in dbContext.tbl_PM_Role
                //                                 where proj.RoleID == project.RoleID
                //                                 select proj.RoleDescription).FirstOrDefault();

                //    var resourceId = (from proj in dbContext.HRMS_tbl_PM_Employee
                //                      where proj.EmployeeID == employeeId
                //                      select proj.ResourcePoolID).FirstOrDefault();

                //    projectdetail.ResourcePoolName = (from pool in dbContext.tbl_PM_ResourcePoolMaster
                //                                      where pool.ResourcePoolID == resourceId
                //                                      select pool.ResourcePoolName).FirstOrDefault();

                //    var DeliveryUnit = (from proj in dbContext.tbl_PM_ResourcePoolDetail
                //                        where proj.EmployeeID == employeeId
                //                        select proj.ResourcePoolID).FirstOrDefault();

                //    projectdetail.DeliveryUnit = (from proj in dbContext.tbl_PM_ResourcePool
                //                                  where proj.ResourcePoolID == DeliveryUnit
                //                                  select proj.ResourcePoolName).FirstOrDefault();

                //    // add manager table
                //    var employeeid = (from manager in dbContext.tbl_PM_ResourcePoolManagers
                //                      where manager.ResourcePoolID == resourceId
                //                      select manager.EmployeeID).FirstOrDefault();

                //    projectdetail.ResourcePoolManager = (from emp in dbContext.HRMS_tbl_PM_Employee
                //                                         where emp.EmployeeID == employeeid
                //                                         select emp.EmployeeName).FirstOrDefault();

                //    var FromDate = project.StartDate;

                //    if (FromDate != null)
                //    {
                //        projectdetail.FromDate = DateTime.Parse(FromDate.ToString());
                //    }
                //    else
                //    {
                //        projectdetail.FromDate = null;
                //    }

                //    var todate = project.EndDate;

                //    if (todate != null)
                //    {
                //        projectdetail.ToDate = DateTime.Parse(todate.ToString());
                //    }
                //    else
                //    {
                //        projectdetail.ToDate = null;
                //    }
                //    ProjectsList.Add(projectdetail);
                //}

                foreach (var project in semlist)
                {
                    ProjectDetailsViewModel projectdetail = new ProjectDetailsViewModel();

                    projectdetail.ProjectDetailID = Convert.ToInt32(project.ProjectID);

                    projectdetail.ProjectResourceID = Convert.ToInt32(project.ProjectEmployeeRoleID);

                    projectdetail.CurrentProject = (from proj in dbSEMContext.tbl_PM_Project
                                                    where proj.ProjectID == project.ProjectID
                                                    select proj.ProjectName).FirstOrDefault();

                    var reportingto = (from proj in dbContext.HRMS_tbl_PM_Employee
                                       where proj.EmployeeID == employeeId
                                       select proj.CostCenterID).FirstOrDefault();

                    projectdetail.CurrentReportingManager = (from emp in dbContext.HRMS_tbl_PM_Employee
                                                             where emp.EmployeeID == reportingto
                                                             select emp.EmployeeName).FirstOrDefault();
                    //changed here

                    projectdetail.CurrentRole = (from projsem in dbSEMContext.tbl_PM_RoleSem
                                                 where projsem.RoleID == project.Role
                                                 select projsem.RoleDescription).FirstOrDefault();

                    var resourceId = (from proj in dbContext.tbl_PM_ResourcePoolDetail
                                      where proj.EmployeeID == employeeId
                                      select proj.ResourcePoolID).FirstOrDefault();

                    projectdetail.ResourcePoolName = (from pool in dbContext.tbl_PM_ResourcePoolMaster
                                                      where pool.ResourcePoolID == resourceId
                                                      select pool.ResourcePoolName).FirstOrDefault();

                    var DeliveryUnit = (from proj in dbContext.HRMS_tbl_PM_Employee
                                        where proj.EmployeeID == employeeId
                                        select proj.ResourcePoolID).FirstOrDefault();

                    projectdetail.DeliveryUnit = (from proj in dbContext.HRMS_tbl_PM_ResourcePool
                                                  where proj.ResourcePoolID == DeliveryUnit
                                                  select proj.ResourcePoolName).FirstOrDefault();

                    // add manager table
                    var employeeid = (from manager in dbContext.tbl_PM_ResourcePoolManagers
                                      where manager.ResourcePoolID == resourceId
                                      select manager.EmployeeID).FirstOrDefault();

                    projectdetail.ResourcePoolManager = (from emp in dbContext.HRMS_tbl_PM_Employee
                                                         where emp.EmployeeID == employeeid
                                                         select emp.EmployeeName).FirstOrDefault();

                    var FromDate = project.ExpectedStartDate;

                    if (FromDate != null)
                    {
                        projectdetail.FromDate = DateTime.Parse(FromDate.ToString());
                    }
                    else
                    {
                        projectdetail.FromDate = null;
                    }

                    var todateActual = project.ActualEndDate;
                    var todateexpected = project.ExpectedEndDate;

                    if (todateActual != null)
                    {
                        projectdetail.ToDate = DateTime.Parse(todateActual.ToString());
                    }
                    else
                    {
                        if (todateexpected != null)
                        {
                            projectdetail.ToDate = DateTime.Parse(todateexpected.ToString());
                        }
                        else
                        {
                            projectdetail.ToDate = null;
                        }
                    }

                    ProjectsList.Add(projectdetail);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ProjectsList.Skip((page - 1) * rows).Take(rows).ToList();
        }

        //public int GetProjectDetailsByEmployeeIdTotalCount(int employeeId)
        //{
        //    int totalCount = 0;
        //    try
        //    {
        //        totalCount = (from projectdetails in dbContext.Tbl_PM_EmployeeProject
        //                      where projectdetails.EmployeeId == employeeId
        //                      select projectdetails).Count();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return totalCount;
        //}

        public List<LocationListDetails> GetLocationList()
        {
            List<LocationListDetails> locationlist = new List<LocationListDetails>();
            try
            {
                locationlist = (from loction in dbContext.tbl_PM_Location
                                where loction.Active == true
                                orderby loction.Location
                                select new LocationListDetails
                                {
                                    LocationId = loction.LocationID,
                                    LocationName = loction.Location
                                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return locationlist.OrderBy(x => x.LocationName).ToList();
        }

        public List<CalenderLocationDetails> GetCalenderLocationList()
        {
            List<CalenderLocationDetails> CalenderLocationList = new List<CalenderLocationDetails>();
            try
            {
                CalenderLocationList = (from calender in dbPmsContext.CalendarLists
                                        select new CalenderLocationDetails
                                {
                                    CalenderId = calender.CalendarId,
                                    CalenderLocationName = calender.CalendarLocation
                                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return CalenderLocationList.OrderBy(x => x.CalenderLocationName).ToList();
        }

        public List<ShiftDetails> GetshiftDetailsList()
        {
            List<ShiftDetails> ShiftDetailsList = new List<ShiftDetails>();
            try
            {
                ShiftDetailsList = (from shift in dbPmsContext.ShiftMasters
                                    where shift.ISActive == true
                                    select new ShiftDetails
                                    {
                                        ShiftId = shift.ShiftID,
                                        ShiftDescription = shift.Description
                                    }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return ShiftDetailsList.OrderBy(x => x.ShiftDescription).ToList();
        }

        public List<GroupListDetails> GetGroupList()
        {
            List<GroupListDetails> grouplist = new List<GroupListDetails>();
            try
            {
                grouplist = (from groupl in dbContext.tbl_PM_GroupMaster
                             where groupl.Active == true
                             orderby groupl.GroupName
                             select new GroupListDetails
                             {
                                 GroupId = groupl.GroupID,
                                 GroupName = groupl.GroupName
                             }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return grouplist.OrderBy(x => x.GroupName).ToList();
        }

        public List<ProjectListDetails> GetProjectList()
        {
            List<ProjectListDetails> projectlist = new List<ProjectListDetails>();
            try
            {
                projectlist = (from project in dbSEMContext.tbl_PM_Project
                               select new ProjectListDetails
                               {
                                   ProjectId = project.ProjectID,
                                   ProjectName = project.ProjectName
                               }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return projectlist;
        }

        public List<ProjectRoleList> GetProjectRole()
        {
            List<ProjectRoleList> rolelist = new List<ProjectRoleList>();
            try
            {
                //change here
                rolelist = (from role in dbContext.HRMS_tbl_PM_Role
                            // rolelist = (from role in dbSEMContext.tbl_PM_Role
                            select new ProjectRoleList
                            {
                                ProjectRoleId = role.RoleID,
                                ProjectRoleDesc = role.RoleDescription
                            }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return rolelist;
        }

        public List<ClientListDetails> GetClientList()
        {
            List<ClientListDetails> clientlist = new List<ClientListDetails>();
            try
            {
                clientlist = (from client in dbSEMContext.tbl_PM_Customer
                              select new ClientListDetails
                              {
                                  ClientId = client.Customer,
                                  ClientName = client.CustomerName
                              }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return clientlist;
        }

        public List<EmployeeListDetails> GetEmployeeList()
        {
            List<EmployeeListDetails> employeelist = new List<EmployeeListDetails>();
            try
            {
                employeelist = (from employee in dbContext.HRMS_tbl_PM_Employee
                                select new EmployeeListDetails
                                {
                                    EmployeeId = employee.EmployeeID,
                                    EmployeeName = employee.EmployeeName.Trim()
                                }).OrderBy(x => x.EmployeeName).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return employeelist;
        }

        public List<EmployeeListDetails> GetEmployeeListForManager()
        {
            List<EmployeeListDetails> employeelist = new List<EmployeeListDetails>();
            try
            {
                employeelist = (from employee in dbContext.HRMS_tbl_PM_Employee
                                where employee.Status == false
                                orderby employee.EmployeeName ascending
                                select new EmployeeListDetails
                                {
                                    EmployeeId = employee.EmployeeID,
                                    EmployeeName = employee.EmployeeName.Trim()
                                }).OrderBy(x => x.EmployeeName).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return employeelist;
        }

        public List<EmployeeListDetails> GetDeliveyHead()
        {
            List<EmployeeListDetails> employeelist = new List<EmployeeListDetails>();
            try
            {
                employeelist = (from employee in dbContext.HRMS_tbl_PM_Employee
                                from grouphead in dbContext.tbl_PM_GroupMaster
                                where employee.EmployeeID == grouphead.ResourceHeadID
                                select new EmployeeListDetails
                                {
                                    EmployeeId = employee.EmployeeID,
                                    EmployeeName = employee.EmployeeName
                                }).Distinct().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return employeelist;
        }

        public List<ResourcePoolListDetails> GetResourcePool()
        {
            List<ResourcePoolListDetails> resourcepool = new List<ResourcePoolListDetails>();
            try
            {
                resourcepool = (from resource in dbContext.HRMS_tbl_PM_ResourcePool
                                where resource.Active == true
                                orderby resource.ResourcePoolName ascending
                                select new ResourcePoolListDetails
                                {
                                    ResourcePoolId = resource.ResourcePoolID,
                                    ResourcePoolName = resource.ResourcePoolName
                                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return resourcepool.OrderBy(x => x.ResourcePoolName).ToList();
        }

        public List<Business_Group> getBusinessGroupNames()
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<Business_Group> BusinessGroups = new List<Business_Group>();
                BusinessGroups = (from b in dbContext.tbl_CNF_BusinessGroups
                                  orderby b.BusinessGroup ascending
                                  where b.Active == true
                                  select new Business_Group
                                  {
                                      BusinessGroupID = b.BusinessGroupID,
                                      BusinessGroup = b.BusinessGroup
                                  }).ToList();
                return BusinessGroups;
            }
            catch
            {
                throw;
            }
        }

        public List<OfficeLocationListDetails> GetOfficeLocationList()
        {
            List<OfficeLocationListDetails> OfficeLocation = new List<OfficeLocationListDetails>();
            try
            {
                OfficeLocation = (from OfficeLocation1 in dbContext.tbl_PM_OfficeLocation
                                  orderby OfficeLocation1.OfficeLocation ascending
                                  select new OfficeLocationListDetails
                                  {
                                      OfficeLocationID = OfficeLocation1.OfficeLocationID,
                                      OfficeLocation = OfficeLocation1.OfficeLocation
                                  }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return OfficeLocation;
        }

        public List<EmployeeStatusListDetails> GetEmployeeStatus(int? StatusMasterId)
        {
            List<EmployeeStatusListDetails> employeestatus = new List<EmployeeStatusListDetails>();
            try
            {
                employeestatus = (from status in dbContext.tbl_PM_EmployeeStatus
                                  where status.EmployeeStatusMasterID == StatusMasterId
                                  select new EmployeeStatusListDetails
                                  {
                                      EmployeeStatusId = status.EmployeeStatusID,
                                      EmployeeStatus = status.EmployeeStatus
                                  }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return employeestatus.OrderBy(x => x.EmployeeStatus).ToList();
        }

        public List<EmployeeStatusMsterListDetails> GetEmployeeStatusMaster()
        {
            List<EmployeeStatusMsterListDetails> employeestatus = new List<EmployeeStatusMsterListDetails>();
            try
            {
                employeestatus = (from status in dbContext.tbl_PM_EmployeeStatusMaster
                                  select new EmployeeStatusMsterListDetails
                                  {
                                      EmployeeStatusMasterId = status.EmployeementStatusId,
                                      EmployeeStatusMaster = status.EmployeementStatus
                                  }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return employeestatus.OrderBy(x => x.EmployeeStatusMaster).ToList();
        }

        public List<ResourcePoolMasterListDetails> GetResourceMaster()
        {
            List<ResourcePoolMasterListDetails> resourcemaster = new List<ResourcePoolMasterListDetails>();
            try
            {
                resourcemaster = (from resource in dbContext.tbl_PM_ResourcePoolMaster
                                  select new ResourcePoolMasterListDetails
                                  {
                                      ResourcePoolId = resource.ResourcePoolID,
                                      ResourcePoolName = resource.ResourcePoolName
                                  }).ToList();
            }
            catch
            {
                throw;
            }
            return resourcemaster.OrderBy(x => x.ResourcePoolName).ToList();
        }

        public List<EmployeeListDetails> GetResourceManager(int ResourcePoolId)
        {
            List<EmployeeListDetails> resourcemaster = new List<EmployeeListDetails>();
            try
            {
                resourcemaster = (from resource in dbContext.tbl_PM_ResourcePoolManagers
                                  from employee in dbContext.HRMS_tbl_PM_Employee
                                  where employee.EmployeeID == resource.EmployeeID && resource.ResourcePoolID == ResourcePoolId
                                  select new EmployeeListDetails
                                  {
                                      EmployeeId = employee.EmployeeID,
                                      EmployeeName = employee.EmployeeName
                                  }).Distinct().ToList();
            }
            catch
            {
                throw;
            }
            return resourcemaster;
        }

        public List<EmployeeListDetails> GetResourcePoolEmployee()
        {
            List<EmployeeListDetails> resourcemaster = new List<EmployeeListDetails>();
            try
            {
                resourcemaster = (from resource in dbContext.tbl_PM_ResourcePoolManagers
                                  from employee in dbContext.HRMS_tbl_PM_Employee
                                  where employee.EmployeeID == resource.EmployeeID
                                  select new EmployeeListDetails
                                  {
                                      EmployeeId = employee.EmployeeID,
                                      EmployeeName = employee.EmployeeName
                                  }).Distinct().ToList();
            }
            catch
            {
                throw;
            }
            return resourcemaster;
        }

        public bool SavePassportDetails(TravelDetailsViewModel model, TravelDetailsPerson type)
        {
            try
            {
                bool status = false;
                if (type == TravelDetailsPerson.Own)
                {
                    Tbl_PM_EmployeePassport _Tbl_PM_EmployeePassport = new Tbl_PM_EmployeePassport();
                    _Tbl_PM_EmployeePassport.EmployeeID = model.EmployeeId;
                    _Tbl_PM_EmployeePassport.PassportFileName = model.PassportFileName;
                    _Tbl_PM_EmployeePassport.PassportFilePath = model.PassportFilePath;
                    _Tbl_PM_EmployeePassport.CreatedDate = DateTime.Now;
                    dbContext.Tbl_PM_EmployeePassport.AddObject(_Tbl_PM_EmployeePassport);
                    dbContext.SaveChanges();
                    status = true;
                }
                else if (type == TravelDetailsPerson.Spouse)
                {
                    Tbl_PM_SpousePassportDocuments _SpousePassport = new Tbl_PM_SpousePassportDocuments();
                    _SpousePassport.EmployeeID = model.EmployeeId;
                    _SpousePassport.PassportFileName = model.PassportFileName;
                    _SpousePassport.PassportFilePath = model.PassportFilePath;
                    _SpousePassport.CreatedDate = DateTime.Now;
                    dbContext.Tbl_PM_SpousePassportDocuments.AddObject(_SpousePassport);
                    dbContext.SaveChanges();
                    status = true;
                }
                return status;
            }
            catch
            {
                throw;
            }
        }

        public CheckPassportValid checkValidEmployeeVisaDetail(int employeeId, int countryId)
        {
            tbl_PM_EmployeeVisaDetails tbl_PM_EmployeeVisaDetails = new tbl_PM_EmployeeVisaDetails();
            CheckPassportValid CheckPassportValid = new CheckPassportValid();
            tbl_PM_EmployeeVisaDetails = dbContext.tbl_PM_EmployeeVisaDetails.Where(x => x.EmployeeID == employeeId && x.CountryID == countryId).FirstOrDefault();
            if (tbl_PM_EmployeeVisaDetails != null)
            {
                CheckPassportValid.IsVisaValid = tbl_PM_EmployeeVisaDetails.ValidUpto < DateTime.Now ? false : true;
                CheckPassportValid.IsVisaExist = true;
                //CheckPassportValid.IsVisaExist = false;
                //CheckPassportValid.IsVisaValid = false;
            }
            else
            {
                CheckPassportValid.IsVisaExist = false;
                CheckPassportValid.IsVisaValid = false;
            }
            return CheckPassportValid;
        }

        public bool AddUpdateVisaDetails(VisaDetailsViewModel visaDetails)
        {
            bool isSuccess = false;
            try
            {
                if (visaDetails.PersonType == TravelDetailsPerson.Own)
                {
                    tbl_PM_EmployeeVisaDetails tblVisaDetails = dbContext.tbl_PM_EmployeeVisaDetails.Where(m => m.EmployeeVisaID == visaDetails.EmployeeVisaId).FirstOrDefault();
                    if (tblVisaDetails != null && tblVisaDetails.EmployeeVisaID > 0)
                    {
                        tblVisaDetails.CountryID = visaDetails.SelectedCountryId;
                        tblVisaDetails.ValidUpto = visaDetails.ValidTill;
                        tblVisaDetails.IsValid = visaDetails.IsValidVisa;
                        tblVisaDetails.VisaTypeID = visaDetails.VisaTypeID;
                        tblVisaDetails.VisaFileName = visaDetails.VisaFileName;
                        tblVisaDetails.VisaFilePath = visaDetails.VisaFilePath;
                        tblVisaDetails.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        if (visaDetails.ValidTill == null)
                            return false;
                        tbl_PM_EmployeeVisaDetails tblVisa = new tbl_PM_EmployeeVisaDetails()
                        {
                            CountryID = visaDetails.SelectedCountryId,
                            EmployeeID = visaDetails.EmployeeId,
                            ValidUpto = visaDetails.ValidTill,
                            VisaTypeID = visaDetails.VisaTypeID,
                            VisaFileName = visaDetails.VisaFileName,
                            VisaFilePath = visaDetails.VisaFilePath,
                            CreatedDate = DateTime.Now,
                            IsValid = true
                        };
                        dbContext.tbl_PM_EmployeeVisaDetails.AddObject(tblVisa);
                    }
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
                if (visaDetails.PersonType == TravelDetailsPerson.Spouse)
                {
                    tbl_PM_DependandsVisaDetails tblVisaDetails = dbContext.tbl_PM_DependandsVisaDetails.Where(m => m.EmployeeID == visaDetails.EmployeeId && m.DependandsVisaDetailsID == visaDetails.DependantVisaDetailsId).FirstOrDefault();
                    HRMS_tbl_PM_Employee spouseMaritalStatus = dbContext.HRMS_tbl_PM_Employee.Where(ms => ms.EmployeeID == visaDetails.EmployeeId).FirstOrDefault();
                    if (spouseMaritalStatus != null && spouseMaritalStatus.MaritalStatus == "Married")
                    {
                        if (tblVisaDetails != null && tblVisaDetails.DependandsVisaDetailsID > 0 && tblVisaDetails.EmployeeID > 0)
                        {
                            tblVisaDetails.CountryID = visaDetails.SelectedCountryId;
                            tblVisaDetails.ValidUpto = visaDetails.ValidTill;
                            tblVisaDetails.IsValid = visaDetails.IsValidVisa;
                            tblVisaDetails.VisaTypeID = visaDetails.VisaTypeID;
                            tblVisaDetails.VisaFileName = visaDetails.VisaFileName;
                            tblVisaDetails.VisaFilePath = visaDetails.VisaFilePath;
                            tblVisaDetails.CreatedDate = DateTime.Now;
                        }
                        else
                        {
                            if (visaDetails.ValidTill == null)
                                return false;
                            tbl_PM_DependandsVisaDetails tblVisa = new tbl_PM_DependandsVisaDetails()
                            {
                                CountryID = visaDetails.SelectedCountryId,
                                EmployeeID = visaDetails.EmployeeId,
                                ValidUpto = visaDetails.ValidTill,
                                VisaTypeID = visaDetails.VisaTypeID,
                                VisaFileName = visaDetails.VisaFileName,
                                VisaFilePath = visaDetails.VisaFilePath,
                                CreatedDate = DateTime.Now,
                                IsValid = true
                            };
                            dbContext.tbl_PM_DependandsVisaDetails.AddObject(tblVisa);
                        }
                        dbContext.SaveChanges();
                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        public bool DeletePassportDetails(int DocumentID, TravelDetailsPerson PersonType)
        {
            try
            {
                bool isDeleted = false;
                if (PersonType == TravelDetailsPerson.Own)
                {
                    Tbl_PM_EmployeePassport _PassportDocument = dbContext.Tbl_PM_EmployeePassport.Where(p => p.DocumentID == DocumentID).FirstOrDefault();
                    if (_PassportDocument != null)
                    {
                        dbContext.DeleteObject(_PassportDocument);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                else if (PersonType == TravelDetailsPerson.Spouse)
                {
                    Tbl_PM_SpousePassportDocuments _SpousePassportDocument = dbContext.Tbl_PM_SpousePassportDocuments.Where(p => p.DocumentID == DocumentID).FirstOrDefault();
                    if (_SpousePassportDocument != null)
                    {
                        dbContext.DeleteObject(_SpousePassportDocument);
                        dbContext.SaveChanges();
                        isDeleted = true;
                    }
                }
                return isDeleted;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TravelDetailsViewModel GetPassportDetails(int employeeId, TravelDetailsPerson type)
        {
            TravelDetailsViewModel travelDetails = new TravelDetailsViewModel();
            string spouseMaritalStatus = "";
            try
            {
                if (type == TravelDetailsPerson.Own)
                {
                    travelDetails = (from empDetails in dbContext.HRMS_tbl_PM_Employee
                                     where empDetails.EmployeeID == employeeId
                                     select new TravelDetailsViewModel
                                     {
                                         DateOfExpiry = empDetails.PP_ExpiryDate,
                                         DateOfIssue = empDetails.PP_DateOfIssue,
                                         EmployeeId = empDetails.EmployeeID,
                                         FullNameAsInPassport = empDetails.PP_FullName,
                                         IsValidPassport = empDetails.PP_IsValid.HasValue ? empDetails.PP_IsValid.Value : false,
                                         NoOfPagesLeft = empDetails.NoofPagesLeft.HasValue ? empDetails.NoofPagesLeft.Value : 0,
                                         PassportNumber = empDetails.PassportNumber,
                                         PlaceOfIssue = empDetails.PP_PlaceOfIssue,
                                         SonOfWifeOfDaugherOf = empDetails.PP_RelativeName
                                     }).FirstOrDefault();
                }
                if (type == TravelDetailsPerson.Spouse)
                {
                    spouseMaritalStatus = (from spouseStatus in dbContext.HRMS_tbl_PM_Employee
                                           where spouseStatus.EmployeeID == employeeId
                                           select spouseStatus.MaritalStatus).FirstOrDefault();

                    travelDetails = (from spousePassportDetails in dbContext.tbl_PM_SpousePassportDetails
                                     where spousePassportDetails.EmployeeId == employeeId && spouseMaritalStatus == "Married"
                                     select new TravelDetailsViewModel
                                     {
                                         EmployeeId = spousePassportDetails.EmployeeId.HasValue ? spousePassportDetails.EmployeeId.Value : 0,
                                         DateOfExpiry = spousePassportDetails.PP_ExpiryDate,
                                         DateOfIssue = spousePassportDetails.PP_DateOfIssue,
                                         FullNameAsInPassport = spousePassportDetails.PP_FullName,
                                         NoOfPagesLeft = spousePassportDetails.NoofPagesLeft,
                                         PassportNumber = spousePassportDetails.PassportNumber,
                                         PlaceOfIssue = spousePassportDetails.PP_PlaceOfIssue,
                                         SonOfWifeOfDaugherOf = spousePassportDetails.PP_SonOfWifeOfDaughterOf
                                     }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return travelDetails;
        }

        public TravelDetailsViewModel GetPassportShowHistory(int EmployeeID, int DocumentID, TravelDetailsPerson PersonType)
        {
            try
            {
                TravelDetailsViewModel showHistory = new TravelDetailsViewModel();
                if (PersonType == TravelDetailsPerson.Own)
                {
                    showHistory = (from passport in dbContext.Tbl_PM_EmployeePassport
                                   where passport.DocumentID == DocumentID && passport.EmployeeID == EmployeeID
                                   select new TravelDetailsViewModel
                                   {
                                       PassportFileName = passport.PassportFileName,
                                       CreatedDate = passport.CreatedDate,
                                       PassportFilePath = passport.PassportFilePath,
                                       DocumentID = passport.DocumentID,
                                       EmployeeId = passport.EmployeeID
                                   }).FirstOrDefault();
                }
                else if (PersonType == TravelDetailsPerson.Spouse)
                {
                    showHistory = (from passport in dbContext.Tbl_PM_SpousePassportDocuments
                                   where passport.DocumentID == DocumentID && passport.EmployeeID == EmployeeID
                                   select new TravelDetailsViewModel
                                   {
                                       PassportFileName = passport.PassportFileName,
                                       CreatedDate = passport.CreatedDate,
                                       PassportFilePath = passport.PassportFilePath,
                                       DocumentID = passport.DocumentID,
                                       EmployeeId = passport.EmployeeID
                                   }).FirstOrDefault();
                }
                return showHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VisaDetailsViewModel GetVisaShowHistory(int EmployeeID, int EmployeeVisaId, TravelDetailsPerson PersonType)
        {
            try
            {
                VisaDetailsViewModel showHistory = new VisaDetailsViewModel();
                if (PersonType == TravelDetailsPerson.Own)
                {
                    showHistory = (from empVisa in dbContext.tbl_PM_EmployeeVisaDetails
                                   where empVisa.EmployeeVisaID == EmployeeVisaId && empVisa.EmployeeID == EmployeeID
                                   select new VisaDetailsViewModel
                                   {
                                       VisaFileName = empVisa.VisaFileName,
                                       VisaFilePath = empVisa.VisaFilePath,
                                       CreatedDate = empVisa.CreatedDate,
                                       EmployeeVisaId = empVisa.EmployeeVisaID,
                                       EmployeeId = empVisa.EmployeeID.HasValue ? empVisa.EmployeeID.Value : 0
                                   }).FirstOrDefault();
                }
                else if (PersonType == TravelDetailsPerson.Spouse)
                {
                    showHistory = (from spouseVisa in dbContext.tbl_PM_DependandsVisaDetails
                                   where spouseVisa.DependandsVisaDetailsID == EmployeeVisaId && spouseVisa.EmployeeID == EmployeeID
                                   select new VisaDetailsViewModel
                                   {
                                       VisaFileName = spouseVisa.VisaFileName,
                                       VisaFilePath = spouseVisa.VisaFilePath,
                                       CreatedDate = spouseVisa.CreatedDate,
                                       DependantVisaDetailsId = spouseVisa.DependandsVisaDetailsID,
                                       EmployeeId = spouseVisa.EmployeeID.HasValue ? spouseVisa.EmployeeID.Value : 0
                                   }).FirstOrDefault();
                }
                return showHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddUpdatePassportDetails(TravelDetailsViewModel travelDetails, TravelDetailsPerson type)
        {
            bool isAdded = false;
            try
            {
                if (type == TravelDetailsPerson.Own)
                {
                    HRMS_tbl_PM_Employee employeeDetails = dbContext.HRMS_tbl_PM_Employee.Where(m => m.EmployeeID == travelDetails.EmployeeId).FirstOrDefault();
                    if (employeeDetails != null && employeeDetails.EmployeeID > 0)
                    {
                        employeeDetails.PP_IsValid = travelDetails.IsValidPassport;
                        employeeDetails.PassportNumber = travelDetails.PassportNumber.Trim();
                        employeeDetails.PP_RelativeName = travelDetails.SonOfWifeOfDaugherOf.Trim();
                        employeeDetails.PP_DateOfIssue = travelDetails.DateOfIssue;
                        employeeDetails.PP_PlaceOfIssue = travelDetails.PlaceOfIssue.Trim();
                        employeeDetails.PP_ExpiryDate = travelDetails.DateOfExpiry;
                        employeeDetails.NoofPagesLeft = travelDetails.NoOfPagesLeft;
                        employeeDetails.PP_FullName = travelDetails.FullNameAsInPassport.Trim();
                        dbContext.SaveChanges();
                        isAdded = true;
                    }
                }
                if (type == TravelDetailsPerson.Spouse)
                {
                    HRMS_tbl_PM_Employee spouseMaritalStatus = dbContext.HRMS_tbl_PM_Employee.Where(m => m.EmployeeID == travelDetails.EmployeeId).FirstOrDefault();
                    tbl_PM_SpousePassportDetails spousePassport = dbContext.tbl_PM_SpousePassportDetails.Where(e => e.EmployeeId == travelDetails.EmployeeId).FirstOrDefault();
                    if (spouseMaritalStatus != null && spouseMaritalStatus.MaritalStatus == "Married")
                    {
                        if (spousePassport != null && spousePassport.EmployeeId > 0)
                        {
                            spousePassport.PassportNumber = travelDetails.PassportNumber.Trim();
                            spousePassport.PP_SonOfWifeOfDaughterOf = travelDetails.SonOfWifeOfDaugherOf.Trim();
                            spousePassport.PP_DateOfIssue = travelDetails.DateOfIssue;
                            spousePassport.PP_PlaceOfIssue = travelDetails.PlaceOfIssue.Trim();
                            spousePassport.PP_ExpiryDate = travelDetails.DateOfExpiry;
                            spousePassport.NoofPagesLeft = travelDetails.NoOfPagesLeft;
                            spousePassport.PP_FullName = travelDetails.FullNameAsInPassport.Trim();
                            dbContext.SaveChanges();
                            isAdded = true;
                        }
                        else if (spousePassport == null)
                        {
                            tbl_PM_SpousePassportDetails spousePassportDetails = new tbl_PM_SpousePassportDetails();
                            spousePassportDetails.EmployeeId = travelDetails.EmployeeId;
                            spousePassportDetails.PassportNumber = travelDetails.PassportNumber.Trim();
                            spousePassportDetails.PP_SonOfWifeOfDaughterOf = travelDetails.SonOfWifeOfDaugherOf.Trim();
                            spousePassportDetails.PP_DateOfIssue = travelDetails.DateOfIssue;
                            spousePassportDetails.PP_PlaceOfIssue = travelDetails.PlaceOfIssue.Trim();
                            spousePassportDetails.PP_ExpiryDate = travelDetails.DateOfExpiry;
                            spousePassportDetails.NoofPagesLeft = travelDetails.NoOfPagesLeft;
                            spousePassportDetails.PP_FullName = travelDetails.FullNameAsInPassport.Trim();
                            dbContext.tbl_PM_SpousePassportDetails.AddObject(spousePassportDetails);
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

        public List<TravelDetailsViewModel> GetEmployeePassportDetails(int page, int rows, int EmployeeID, out int totalCount)
        {
            List<TravelDetailsViewModel> passportDetails = (from p in dbContext.Tbl_PM_EmployeePassport
                                                            where p.EmployeeID == EmployeeID
                                                            select new TravelDetailsViewModel
                                                            {
                                                                PassportFileName = p.PassportFileName,
                                                                EmployeeId = p.EmployeeID,
                                                                DocumentID = p.DocumentID
                                                            }).ToList();
            totalCount = passportDetails.Count();
            return passportDetails.Skip((page - 1) * rows).Take(rows).ToList();
        }

        public List<TravelDetailsViewModel> GetSpousePassportDetails(int page, int rows, int EmployeeID, out int totalCount)
        {
            List<TravelDetailsViewModel> passportDetails = (from p in dbContext.Tbl_PM_SpousePassportDocuments
                                                            where p.EmployeeID == EmployeeID
                                                            select new TravelDetailsViewModel
                                                            {
                                                                PassportFileName = p.PassportFileName,
                                                                EmployeeId = p.EmployeeID,
                                                                DocumentID = p.DocumentID
                                                            }).ToList();
            totalCount = passportDetails.Count();
            return passportDetails.Skip((page - 1) * rows).Take(rows).ToList();
        }

        public int GetDependentId(int employeeId, TravelDetailsPerson type)
        {
            int dependentId = 0;
            try
            {
                if (type == TravelDetailsPerson.Spouse)
                {
                    dependentId = dbContext.tbl_PM_Employee_Dependands.Where(m => m.EmployeeID == employeeId && (m.RelationType == "Wife" || m.RelationType == "Husband")).Select(m => m.DependandsID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dependentId;
        }

        public CheckSpouseDetails IsSpouseDetailsPresent(int employeeId)
        {
            CheckSpouseDetails IsSpousedetailsValid = new CheckSpouseDetails();
            IsSpousedetailsValid.isPresent = false;
            IsSpousedetailsValid.isApproved = false;
            try
            {
                var employeeAprovaldata =
                    this.dbContext.tbl_ApprovalChanges.Where(
                        ap => ap.EmployeeID == employeeId && ap.FieldDiscription == "Marital Status")
                        .OrderByDescending(apc => apc.CreatedDateTime)
                        .FirstOrDefault();

                if (employeeAprovaldata != null)
                {
                    IsSpousedetailsValid.isApproved = (employeeAprovaldata.ApprovalStatusMasterID.HasValue
                                  && employeeAprovaldata.ApprovalStatusMasterID.Value == 2);
                }
                else
                {
                    IsSpousedetailsValid.isApproved = true;
                }

                IsSpousedetailsValid.spouseMaritalStatusName =
                    this.dbContext.HRMS_tbl_PM_Employee.Where(
                        m => m.EmployeeID == employeeId).Select(m => m.MaritalStatus).FirstOrDefault();

                if (IsSpousedetailsValid.spouseMaritalStatusName == "Married")
                {
                    IsSpousedetailsValid.isPresent = true;
                }

                IsSpousedetailsValid.approvalStatusId = (from ap in dbContext.tbl_ApprovalChanges
                                                         where ap.EmployeeID == employeeId && ap.FieldDiscription == "Marital Status"
                                                         orderby ap.CreatedDateTime descending
                                                         select ap.ApprovalStatusMasterID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return IsSpousedetailsValid;
        }

        public bool DeleteEmployeeVisaDetails(int employeeVisaId)
        {
            bool isDeleted = false;
            try
            {
                tbl_PM_EmployeeVisaDetails visaDetails = dbContext.tbl_PM_EmployeeVisaDetails.Where(m => m.EmployeeVisaID == employeeVisaId).FirstOrDefault();
                List<tbl_HR_VisaDetailsTravel> travelVisaDetails = (from travelMaster in dbContext.Tbl_HR_Travel
                                                                    join travelVisa in dbContext.tbl_HR_VisaDetailsTravel on travelMaster.TravelId equals travelVisa.ID
                                                                    join employeeVisa in dbContext.tbl_PM_EmployeeVisaDetails on travelVisa.EmployeeVisaID equals employeeVisa.EmployeeVisaID
                                                                    where travelVisa.EmployeeVisaID == employeeVisaId && travelMaster.StageID == 0
                                                                    select travelVisa).ToList();

                dbContext.tbl_PM_EmployeeVisaDetails.DeleteObject(visaDetails);
                foreach (var item in travelVisaDetails)
                {
                    if (item != null)
                    {
                        dbContext.tbl_HR_VisaDetailsTravel.DeleteObject(item);
                    }
                }

                dbContext.SaveChanges();
                isDeleted = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDeleted;
        }

        public bool DeleteEmployeeDependantVisaDetails(int dependantsVisaDetailsId)
        {
            bool isDeleted = false;
            try
            {
                tbl_PM_DependandsVisaDetails visaDetails = dbContext.tbl_PM_DependandsVisaDetails.Where(m => m.DependandsVisaDetailsID == dependantsVisaDetailsId).FirstOrDefault();
                dbContext.tbl_PM_DependandsVisaDetails.DeleteObject(visaDetails);
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
        /// Getting bond detials of the employee from the database
        /// </summary>
        /// <param name="employeeId">Need to get bond information of the particular employee</param>
        /// <returns>Returns the Bond details information of the provided employee. It includes information such as Bond Amount, Bond Over Date, Bond Status(Yes, No)</returns>
        public BondDetailsViewModel GetBondDetails(int employeeId)
        {
            BondDetailsViewModel bondDetails = new BondDetailsViewModel();
            try
            {
                bondDetails = (from tblBondDetails in dbContext.tbl_BondDetails
                               where tblBondDetails.Employee_Id == employeeId
                               select new BondDetailsViewModel
                               {
                                   BondId = tblBondDetails.Bond_id,
                                   BondAmount = tblBondDetails.BondAmount,
                                   BondOverDate = tblBondDetails.BondOverDate,
                                   EmployeeId = tblBondDetails.Employee_Id,
                                   BondStatus = tblBondDetails.BontStatus
                               }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bondDetails;
        }

        public List<BondDetailsViewModel> LoadBondDetailsGrid(int pageNo, int pageSize, int employeeId)
        {
            List<BondDetailsViewModel> bondDetails = new List<BondDetailsViewModel>();
            try
            {
                bondDetails = (from tblBondDetails in dbContext.tbl_BondDetails
                               where tblBondDetails.Employee_Id == employeeId
                               orderby tblBondDetails.BondType
                               select new BondDetailsViewModel
                               {
                                   BondId = tblBondDetails.Bond_id,
                                   BondTypeID = tblBondDetails.BondType,
                                   BondType = (from b in dbContext.tbl_BondTypeMaster where b.BondTypeID == tblBondDetails.BondType select b.Description).FirstOrDefault(),
                                   BondAmount = tblBondDetails.BondAmount,
                                   BondOverDate = tblBondDetails.BondOverDate,
                                   EmployeeId = tblBondDetails.Employee_Id,
                                   BondStatus = tblBondDetails.BontStatus,
                                   BondStatusHidden = tblBondDetails.BontStatus
                               }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bondDetails;
        }

        public int LoadBondDetailsGridTotalCount(int pageNo, int pageSize, int employeeId)
        {
            List<BondDetailsViewModel> bondDetails = new List<BondDetailsViewModel>();
            int count = 0;
            try
            {
                bondDetails = (from tblBondDetails in dbContext.tbl_BondDetails
                               where tblBondDetails.Employee_Id == employeeId
                               orderby tblBondDetails.BondType
                               select new BondDetailsViewModel
                               {
                                   BondId = tblBondDetails.Bond_id,
                                   BondTypeID = tblBondDetails.BondType,
                                   BondType = (from b in dbContext.tbl_BondTypeMaster where b.BondTypeID == tblBondDetails.BondType select b.Description).FirstOrDefault(),
                                   BondAmount = tblBondDetails.BondAmount,
                                   BondOverDate = tblBondDetails.BondOverDate,
                                   EmployeeId = tblBondDetails.Employee_Id,
                                   BondStatus = tblBondDetails.BontStatus
                               }).ToList();

                count = bondDetails.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return count;
        }

        public bool DeleteBondDetails(int employeeBondID)
        {
            bool isDeleted = false;
            try
            {
                tbl_BondDetails details = dbContext.tbl_BondDetails.Where(b => b.Bond_id == employeeBondID).FirstOrDefault();
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

        public List<BondDetailsViewModel> GetBondTypeList()
        {
            List<BondDetailsViewModel> model = new List<BondDetailsViewModel>();
            try
            {
                model = (from bt in dbContext.tbl_BondTypeMaster
                         select
                             new BondDetailsViewModel
                             {
                                 BondTypeID = bt.BondTypeID,
                                 BondType = bt.Description
                             }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return model.OrderBy(x => x.BondType).ToList();
        }

        /// <summary>
        /// Add or Update Bond Details in the database based on the data provided
        /// </summary>
        /// <param name="bondDetails">Contains detials of the bond details like Bond Amount, Bond Over Date, Bond Status etc.</param>
        /// <returns>Returns True/False to indicate the Add/Update operation is Succeeded or Failed</returns>
        public bool AddUpdateBondDetails(BondDetailsViewModel bondDetails)
        {
            bool isSuccess = false;
            try
            {
                tbl_BondDetails tblBondDetails = (from bd in dbContext.tbl_BondDetails
                                                  where bd.Bond_id == bondDetails.BondId
                                                  orderby bd.Bond_id descending
                                                  select bd).FirstOrDefault();
                if (tblBondDetails != null && tblBondDetails.Bond_id > 0)
                {
                    tblBondDetails.BondType = Convert.ToInt32(bondDetails.BondType);
                    if (bondDetails.BondAmount != null && bondDetails.BondAmount != "")
                        tblBondDetails.BondAmount = bondDetails.BondAmount.Trim();
                    else
                        tblBondDetails.BondAmount = bondDetails.BondAmount;
                    tblBondDetails.BondOverDate = bondDetails.BondOverDate;
                    tblBondDetails.BontStatus = bondDetails.BondStatus;
                }
                else
                {
                    tblBondDetails = new tbl_BondDetails();
                    tblBondDetails.Employee_Id = bondDetails.EmployeeId;
                    if (bondDetails.BondAmount != null && bondDetails.BondAmount != "")
                        tblBondDetails.BondAmount = bondDetails.BondAmount.Trim();
                    else
                        tblBondDetails.BondAmount = bondDetails.BondAmount;
                    tblBondDetails.BondOverDate = bondDetails.BondOverDate;
                    tblBondDetails.BontStatus = bondDetails.BondStatus;
                    tblBondDetails.BondType = Convert.ToInt32(bondDetails.BondType);

                    dbContext.tbl_BondDetails.AddObject(tblBondDetails);
                }
                dbContext.SaveChanges();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        /// <summary>
        /// To retrieve experience details of an employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public EmployeeExperienceDetails GetEmployeeExperienceDetails(int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                EmployeeExperienceDetails _employeeExperienceDetails = new EmployeeExperienceDetails();

                var experienceListResult = dbContext.GetExperienceDetails(employeeId);

                foreach (var result in experienceListResult)
                {
                    _employeeExperienceDetails.EmployeeId = employeeId;
                    _employeeExperienceDetails.PastExperienceYears = result.PastExperienceYears.Value;
                    _employeeExperienceDetails.PastExperienceMonths = result.PastExperienceMonths.Value;
                    _employeeExperienceDetails.V2ExperienceYears = result.V2ExperienceYears.Value;
                    _employeeExperienceDetails.V2ExperienceMonths = result.V2ExperienceMonths.Value;
                    _employeeExperienceDetails.TotalExperienceYears = result.TotalExperienceYears.Value;
                    _employeeExperienceDetails.TotalExperienceMonths = result.TotalExperienceMonths.Value;
                    _employeeExperienceDetails.RelevantExperienceYears = result.RelevantExperienceYears;
                    _employeeExperienceDetails.RelevantExperienceMonths = result.RelevantExperienceMonths;
                }

                _employeeExperienceDetails.PastExperienceDetails = new PastEmployeeExperienceViewModel();
                _employeeExperienceDetails.PastExperienceDetails.EmployeeId = employeeId;
                return _employeeExperienceDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To get Total Count of Employee's Past Experiences
        /// </summary>
        /// <param name="employeeId">For getting count based on Employee Id</param>
        /// <returns>Returns total count of employees past experiences</returns>
        public int GetEmployeePastExperienceDetailsTotalCount(int employeeId)
        {
            int totalCount = 0;
            try
            {
                dbContext = new HRMSDBEntities();
                List<PastEmployeeExperienceDetails> _employeePastExperienceDetails = new List<PastEmployeeExperienceDetails>();

                totalCount = (from _experience in dbContext.HRMS_tbl_PM_EmployeeHistory
                              where _experience.EmployeeID == employeeId
                              select _experience.EmployeeHistoryID).Count();
            }
            catch (Exception)
            {
                throw;
            }
            return totalCount;
        }

        public int GetEmployeeGapExperienceDetailsTotalCount(int employeeId)
        {
            int totalCount = 0;
            try
            {
                dbContext = new HRMSDBEntities();
                List<EmployeeExperienceDetails> _employeeGapExperienceDetails = new List<EmployeeExperienceDetails>();

                totalCount = (from _experience in dbContext.tbl_PM_EmployeeQualificationGap
                              where _experience.EmployeeID == employeeId
                              select _experience.GapID).Count();
            }
            catch (Exception)
            {
                throw;
            }
            return totalCount;
        }

        /// <summary>
        /// To retrieve experience details of an employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<PastEmployeeExperienceDetails> GetEmployeePastExperienceDetails(int pageNo, int pageSize, int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<PastEmployeeExperienceDetails> _employeePastExperienceDetails = new List<PastEmployeeExperienceDetails>();

                _employeePastExperienceDetails = (from _experience in dbContext.HRMS_tbl_PM_EmployeeHistory
                                                  where _experience.EmployeeID == employeeId
                                                  orderby _experience.WorkedFrom descending
                                                  select new PastEmployeeExperienceDetails
                                                  {
                                                      EmployeeHistoryId = _experience.EmployeeHistoryID,
                                                      EmployeeId = _experience.EmployeeID,
                                                      EmployeeTypeId = _experience.EmployeeTypeID.Value,
                                                      OrganizationName = _experience.OrganizationName,
                                                      WorkedFrom = _experience.WorkedFrom.Value,
                                                      WorkedTill = _experience.WorkedTill.Value,
                                                      Location = _experience.Location,
                                                      EmployeeWorkingType = (from workingType in dbContext.tbl_PM_EmployeeType where workingType.UniqueID == _experience.EmployeeTypeID select workingType.EmployeeType).FirstOrDefault(),
                                                      LastDesignation = _experience.LastDesignation,
                                                      ReportingManager = _experience.ReportingManager,
                                                      LastSalaryDrawn = _experience.LastSalaryDrawn
                                                  }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return _employeePastExperienceDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmployeeExperienceDetails> GetEmployeeGapExperienceDetails(int pageNo, int pageSize, int employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                List<EmployeeExperienceDetails> _employeeGapExperienceDetails = new List<EmployeeExperienceDetails>();

                _employeeGapExperienceDetails = (from _experience in dbContext.tbl_PM_EmployeeQualificationGap
                                                 where _experience.EmployeeID == employeeId
                                                 orderby _experience.FromDate descending
                                                 select new EmployeeExperienceDetails
                                                  {
                                                      EmployeeGapExpId = _experience.GapID,
                                                      EmployeeId = _experience.EmployeeID,
                                                      FromDate = _experience.FromDate,
                                                      ToDate = _experience.ToDate,
                                                      Reason = _experience.Reason,
                                                      GapDuration = _experience.GapDuration,
                                                      Description = _experience.Decription
                                                  }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return _employeeGapExperienceDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmployeeWorkingType> GetEmpoyeeWorkingTypeList()
        {
            List<EmployeeWorkingType> workingTypeList = new List<EmployeeWorkingType>();
            try
            {
                workingTypeList = (from workingType in dbContext.tbl_PM_EmployeeType
                                   select new EmployeeWorkingType
                                   {
                                       EmployeeTypeId = workingType.UniqueID,
                                       WorkingTypeName = workingType.EmployeeType
                                   }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return workingTypeList;
        }

        public bool AddUpdateEmployeePastExperience(PastEmployeeExperienceDetails pastExperienceDetails)
        {
            bool isSuccess = false;
            try
            {
                if (pastExperienceDetails != null && pastExperienceDetails.EmployeeHistoryId > 0)
                {
                    HRMS_tbl_PM_EmployeeHistory tblEmployeehostory = dbContext.HRMS_tbl_PM_EmployeeHistory.Where(m => m.EmployeeHistoryID == pastExperienceDetails.EmployeeHistoryId).FirstOrDefault();
                    if (tblEmployeehostory != null && tblEmployeehostory.EmployeeHistoryID > 0)
                    {
                        tblEmployeehostory.OrganizationName = pastExperienceDetails.OrganizationName.Trim();
                        tblEmployeehostory.WorkedFrom = pastExperienceDetails.WorkedFrom;
                        tblEmployeehostory.WorkedTill = pastExperienceDetails.WorkedTill;
                        tblEmployeehostory.Location = pastExperienceDetails.Location.Trim();
                        tblEmployeehostory.EmployeeTypeID = pastExperienceDetails.EmployeeTypeId;
                        if (pastExperienceDetails.LastDesignation != null && pastExperienceDetails.LastDesignation != "")
                            tblEmployeehostory.LastDesignation = pastExperienceDetails.LastDesignation.Trim();
                        else
                            tblEmployeehostory.LastDesignation = pastExperienceDetails.LastDesignation;

                        if (pastExperienceDetails.ReportingManager != null && pastExperienceDetails.ReportingManager != "")
                            tblEmployeehostory.ReportingManager = pastExperienceDetails.ReportingManager.Trim();
                        else
                            tblEmployeehostory.ReportingManager = pastExperienceDetails.ReportingManager;

                        if (pastExperienceDetails.LastSalaryDrawn != null && pastExperienceDetails.LastSalaryDrawn != "")
                            tblEmployeehostory.LastSalaryDrawn = pastExperienceDetails.LastSalaryDrawn.Trim();
                        else
                            tblEmployeehostory.LastSalaryDrawn = pastExperienceDetails.LastSalaryDrawn;
                        tblEmployeehostory.ModifiedDate = DateTime.Now;
                    }
                }
                else
                {
                    if (pastExperienceDetails != null)
                    {
                        HRMS_tbl_PM_EmployeeHistory tblEmployeeHistory = new HRMS_tbl_PM_EmployeeHistory();
                        tblEmployeeHistory.EmployeeID = pastExperienceDetails.EmployeeId;
                        tblEmployeeHistory.OrganizationName = pastExperienceDetails.OrganizationName.Trim();
                        tblEmployeeHistory.WorkedFrom = pastExperienceDetails.WorkedFrom;
                        tblEmployeeHistory.WorkedTill = pastExperienceDetails.WorkedTill;
                        tblEmployeeHistory.Location = pastExperienceDetails.Location.Trim();
                        tblEmployeeHistory.ModifiedDate = DateTime.Now;
                        tblEmployeeHistory.IsCurrentCompanyExperience = false;
                        tblEmployeeHistory.CreatedDate = DateTime.Now;
                        tblEmployeeHistory.EmployeeTypeID = pastExperienceDetails.EmployeeTypeId;
                        if (pastExperienceDetails.LastDesignation != null && pastExperienceDetails.LastDesignation != "")
                            tblEmployeeHistory.LastDesignation = pastExperienceDetails.LastDesignation.Trim();
                        else
                            tblEmployeeHistory.LastDesignation = pastExperienceDetails.LastDesignation;
                        if (pastExperienceDetails.ReportingManager != null && pastExperienceDetails.ReportingManager != "")
                            tblEmployeeHistory.ReportingManager = pastExperienceDetails.ReportingManager.Trim();
                        else
                            tblEmployeeHistory.ReportingManager = pastExperienceDetails.ReportingManager;
                        if (pastExperienceDetails.LastSalaryDrawn != null && pastExperienceDetails.LastSalaryDrawn != "")
                            tblEmployeeHistory.LastSalaryDrawn = pastExperienceDetails.LastSalaryDrawn.Trim();
                        else
                            tblEmployeeHistory.LastSalaryDrawn = pastExperienceDetails.LastSalaryDrawn;

                        dbContext.HRMS_tbl_PM_EmployeeHistory.AddObject(tblEmployeeHistory);
                    }
                }
                dbContext.SaveChanges();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        public bool AddUpdateEmployeeGapExperience(EmployeeExperienceDetails gapExperienceDetails)
        {
            bool isSuccess = false;
            try
            {
                if (gapExperienceDetails != null && gapExperienceDetails.EmployeeGapExpId > 0)
                {
                    tbl_PM_EmployeeQualificationGap tblEmployeegap = dbContext.tbl_PM_EmployeeQualificationGap.Where(m => m.GapID == gapExperienceDetails.EmployeeGapExpId).FirstOrDefault();
                    if (tblEmployeegap != null && tblEmployeegap.GapID > 0)
                    {
                        tblEmployeegap.FromDate = gapExperienceDetails.FromDate;
                        tblEmployeegap.ToDate = gapExperienceDetails.ToDate;
                        tblEmployeegap.Reason = gapExperienceDetails.Reason.Trim();
                        tblEmployeegap.GapDuration = gapExperienceDetails.GapDuration.ToString();
                        if (gapExperienceDetails.Description != null && gapExperienceDetails.Description != "")
                            tblEmployeegap.Decription = gapExperienceDetails.Description.Trim();
                        else
                            tblEmployeegap.Decription = gapExperienceDetails.Description;
                        tblEmployeegap.ModifiedDate = DateTime.Now;
                    }
                }
                else
                {
                    if (gapExperienceDetails != null)
                    {
                        tbl_PM_EmployeeQualificationGap tblEmployeegap = new tbl_PM_EmployeeQualificationGap();
                        tblEmployeegap.EmployeeID = gapExperienceDetails.EmployeeId;
                        tblEmployeegap.ModifiedDate = DateTime.Now;
                        tblEmployeegap.CreatedDate = DateTime.Now;
                        tblEmployeegap.FromDate = gapExperienceDetails.FromDate;
                        tblEmployeegap.ToDate = gapExperienceDetails.ToDate;
                        tblEmployeegap.Reason = gapExperienceDetails.Reason.Trim();
                        tblEmployeegap.GapDuration = gapExperienceDetails.GapDuration.ToString();
                        if (gapExperienceDetails.Description != null && gapExperienceDetails.Description != "")
                            tblEmployeegap.Decription = gapExperienceDetails.Description.Trim();
                        else
                            tblEmployeegap.Decription = gapExperienceDetails.Description;

                        dbContext.tbl_PM_EmployeeQualificationGap.AddObject(tblEmployeegap);
                    }
                }
                dbContext.SaveChanges();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        public bool DeleteEmployeePastExperienceDetails(int empHistoryId)
        {
            bool isDeleted = false;
            try
            {
                HRMS_tbl_PM_EmployeeHistory tblEmployeeHistory = dbContext.HRMS_tbl_PM_EmployeeHistory.Where(m => m.EmployeeHistoryID == empHistoryId).FirstOrDefault();
                if (tblEmployeeHistory != null && tblEmployeeHistory.EmployeeHistoryID > 0)
                {
                    dbContext.HRMS_tbl_PM_EmployeeHistory.DeleteObject(tblEmployeeHistory);
                    dbContext.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDeleted;
        }

        public bool DeleteEmployeeGapExperienceDetails(int empGapExpId)
        {
            bool isDeleted = false;
            try
            {
                tbl_PM_EmployeeQualificationGap tblEmployeeHistory = dbContext.tbl_PM_EmployeeQualificationGap.Where(m => m.GapID == empGapExpId).FirstOrDefault();
                if (tblEmployeeHistory != null && tblEmployeeHistory.GapID > 0)
                {
                    dbContext.tbl_PM_EmployeeQualificationGap.DeleteObject(tblEmployeeHistory);
                    dbContext.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDeleted;
        }

        public bool UpdateTotalExperienceDetails(EmployeeExperienceDetails model)
        {
            bool isUpdated = false;
            try
            {
                HRMS_tbl_PM_Employee tblEmployeeDetails = dbContext.HRMS_tbl_PM_Employee.Where(m => m.EmployeeID == model.EmployeeId).FirstOrDefault();
                if (tblEmployeeDetails != null && tblEmployeeDetails.EmployeeID > 0)
                {
                    tblEmployeeDetails.RelevantExperienceInMonths = model.RelevantExperienceMonths;
                    tblEmployeeDetails.RelevantExperienceInYears = model.RelevantExperienceYears;
                    dbContext.SaveChanges();
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        public string getCurrentDU(string EmployeeCode)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                int? EmployeeCurrentDU = dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeCode == EmployeeCode).FirstOrDefault().Current_DU;
                return Convert.ToString(EmployeeCurrentDU);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<PastEmployeeExperienceDetails> GetExperceDateList(int EmployeeId)
        {
            List<PastEmployeeExperienceDetails> DateList = new List<PastEmployeeExperienceDetails>();
            try
            {
                DateList = (from Date in dbContext.HRMS_tbl_PM_EmployeeHistory
                            where Date.EmployeeID == EmployeeId
                            select new PastEmployeeExperienceDetails
                            {
                                WorkedFrom = Date.WorkedFrom,
                                WorkedTill = Date.WorkedTill
                            }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DateList;
        }

        public tbl_PM_DesignationMaster GetDesignation(int? designationId)
        {
            tbl_PM_DesignationMaster designation = new tbl_PM_DesignationMaster();
            try
            {
                designation = dbContext.tbl_PM_DesignationMaster.Where(d => d.DesignationID == designationId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return designation;
        }

        public HRMS_tbl_PM_Role GetEmployeeOrganizationRole(int? roleId)
        {
            //ist<RepotingToList> resourcepool = new List<RepotingToList>();
            HRMS_tbl_PM_Role OrganizationRole = new HRMS_tbl_PM_Role();
            try
            {
                OrganizationRole = (from roleName in dbContext.HRMS_tbl_PM_Role
                                    where roleName.RoleID == roleId
                                    select roleName).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return OrganizationRole;
        }

        //public bool ActiveStatusDetails(int Status)
        //{
        //    EmployeeDAL employeeDAL = new EmployeeDAL();
        //    bool? activeValue = false ;
        //    try
        //    {
        //        activeValue = (from AV in dbContext.tbl_PM_GroupMaster
        //                        where AV.Active.Equals(1)
        //                       select AV.Active).FirstOrDefault();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return activeValue.Value;
        //}

        //public List<EmployeeManagerList> GetEmployeeManager()
        //{
        //    List<EmployeeManagerList> employeeDetails = new List<EmployeeManagerList>();
        //    try
        //    {
        //        employeeDetails = (from employee in dbContext.HRMS_tbl_PM_Employee
        //                           orderby employee.EmployeeName
        //                           select new EmployeeManagerList
        //                           {
        //                             ManagerId=employee.EmployeeID,
        //                             ManagerName=employee.EmployeeName
        //                           }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return employeeDetails;
        //}

        public List<ViewableNodesForEmployee> GetViewableNodesForEmployee(int EmpCode)
        {
            List<ViewableNodesForEmployee> viewablenodes = new List<ViewableNodesForEmployee>();
            WSEMDBEntities WSEMdbContext = new WSEMDBEntities();
            try
            {
                var viewablenodesList = WSEMdbContext.usp_Sel_ViewableNodes(EmpCode);
                viewablenodes = (from v in viewablenodesList
                                 select new ViewableNodesForEmployee
                                 {
                                     NodeID = v.NodeID,
                                     NodeName = v.NodeName,
                                     CanView = v.CanView
                                 }).ToList();
                return viewablenodes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PageRights GetPageLevelAccessForEmployee(int EmpCode, int NodeID)
        {
            PageRights NodeLevelAccess = new PageRights();
            WSEMDBEntities WSEMdbContext = new WSEMDBEntities();
            try
            {
                var NodeLevelAccessList = WSEMdbContext.usp_Sel_NodeLevelAccess(EmpCode, NodeID);
                NodeLevelAccess = (from n in NodeLevelAccessList
                                   select new PageRights
                                {
                                    PageId = n.NodeID,
                                    CanAdd = Convert.ToBoolean(n.CanAdd),
                                    CanEdit = Convert.ToBoolean(n.CanEdit),
                                    CanDelete = Convert.ToBoolean(n.CanDelete)
                                }).FirstOrDefault();
                return NodeLevelAccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //-----------------------------------------------//
        public HRMS_tbl_PM_Employee GetEmployeeDetailsByCode(string employeeId)
        {
            try
            {
                dbContext = new HRMSDBEntities();
                int EmpId = int.Parse(employeeId);
                HRMS_tbl_PM_Employee EmpDetails = dbContext.HRMS_tbl_PM_Employee.Where(ed => ed.EmployeeCode == employeeId).FirstOrDefault();
                return EmpDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AccessRightMapping> GetPageAccessMapping(string employeeCode)
        {
            List<AccessRightMapping> userAccessDetails = new List<AccessRightMapping>();
            var accessDetails = dbSEMContext.v2_getUserAccess(Convert.ToInt32(employeeCode));
            userAccessDetails = (from aD in accessDetails
                                 orderby aD.UserName
                                 select new AccessRightMapping
                                {
                                    UserName = aD.UserName,
                                    RoleName = aD.RoleName,
                                    ActionKey = aD.ActionKey,
                                    MenuId = aD.MenuId,
                                    Action = aD.Action,
                                    ControllerName = aD.ControllerName,
                                    Area = aD.Area,
                                    Section = aD.Section,
                                    CanAdd = aD.CanAdd
                                }).ToList();
            return userAccessDetails;
        }

        public string GetPageAccessMapping_xmlData(string employeeCode)
        {
            ObjectResult<string> data = dbSEMContext.v2_getUserAccess_xml(Convert.ToInt32(employeeCode));
            StringBuilder builder = new StringBuilder();

            foreach (var item in data)
            {
                builder.Append(item);
            }

            return builder.ToString();
        }

        public bool InitiateConfirmation(int EmployeeID)
        {
            bool isAdded = false;
            ObjectParameter Output = new ObjectParameter("Result", typeof(int));
            dbContext.InitiateConfirmation_sp(EmployeeID, Output);
            isAdded = Convert.ToBoolean(Output.Value);
            return isAdded;
        }

        public bool CheckConfirmStatus(int EmployeeID)
        {
            bool isAdded = false;
            ObjectParameter Output = new ObjectParameter("Result", typeof(int));
            dbContext.InitiateConfirmationCheck_sp(EmployeeID, Output);
            isAdded = Convert.ToBoolean(Output.Value);
            return isAdded;
        }
    }
}