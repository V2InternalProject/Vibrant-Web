using HRMS.DAL;
using HRMS.Helper;
using HRMS.Models;
using HRMS.Notification;
using MvcApplication3.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace HRMS.Controllers
{
    //[CustomAuthorize(Roles = "HR Admin, HR Executive, RMG")]
    public class PersonalDetailsController : Controller
    {
        private CommonMethodsDAL Commondal = new CommonMethodsDAL();
        private PersonalDetailsDAL dal = new PersonalDetailsDAL();
        private EmployeeDAL empdal = new EmployeeDAL();
        private HRMSDBEntities dbContext = new HRMSDBEntities();
        private QualificationDetailsDAL Qual = new QualificationDetailsDAL();
        private CertificationDetailsDAL Cert = new CertificationDetailsDAL();

        public string UploadProfileImageLocation
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadEmployeeFileLocation"];
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ///
        [HttpGet]
        public ActionResult Index(string employeeId)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();
            PersonalDetailsDAL dal = new PersonalDetailsDAL();
            PersonalDetailsViewModel model = new PersonalDetailsViewModel();
            try
            {
                model.SearchedUserDetails = new SearchedUserDetails();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.UserRole = user;
                model.SearchedUserDetails.UserRole = user;

                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                ViewBag.loggedinEmployeeID = employeeID;

                if (string.IsNullOrEmpty(employeeId) && user != UserRoles.HRAdmin)
                {
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                {
                    string decryptedEmployeeId = string.Empty;
                    string employeeCode = Membership.GetUser().UserName;
                    int employeeid = employeeDAL.GetEmployeeID(employeeCode);
                    Session["SearchEmpID"] = employeeId;
                    if (employeeId != null)
                    {
                        bool isAuthorize;
                        decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                        if (!isAuthorize)
                            return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                    }
                    else
                        decryptedEmployeeId = Convert.ToString(employeeid);

                    int? decryptedemployeeId = 0;
                    if (string.IsNullOrEmpty(decryptedEmployeeId))
                        model.SearchUserID = 0;
                    else
                    {
                        decryptedemployeeId = Convert.ToInt32(decryptedEmployeeId);
                        model.SearchUserID = Convert.ToInt32(decryptedEmployeeId);
                    }

                    if (!HttpContext.User.Identity.IsAuthenticated)
                        return RedirectToAction("LogOn", "Account");  //replace the null with a viewModel as needed.

                    model.EmployeeId = decryptedemployeeId.HasValue ? decryptedemployeeId.Value : 0;
                    ViewBag.EmployeeId = employeeId;

                    HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(decryptedemployeeId.HasValue ? decryptedemployeeId.Value : 0);

                    string displayName;
                    displayName = dal.GetDisplayName(decryptedemployeeId.HasValue ? decryptedemployeeId.Value : 0);

                    if (employeeDetails != null && employeeDetails.EmployeeID > 0)
                    {
                        model.SearchedUserDetails.EmployeeId = decryptedemployeeId.HasValue ? decryptedemployeeId.Value : 0;
                        model.SearchedUserDetails.EmployeeFullName = displayName;
                        model.SearchedUserDetails.EmployeeCode = employeeDetails.EmployeeCode;
                        Session["SearchEmpFullName"] = displayName;
                        Session["SearchEmpCode"] = employeeDetails.EmployeeCode;
                    }
                    //return View(model);
                    return RedirectToAction("PersonalDetails", "PersonalDetails", new { employeeId = employeeId });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors.." });
            }
        }

        /// <summary>
        ///  To retrieve Personal details of Employee
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PageAccess(PageName = "Personal")]
        public ActionResult PersonalDetails(string employeeId)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PersonalDetails");  //replace the null with a viewModel as needed.

            try
            {
                string decryptedEmployeeId = string.Empty;
                string encryptedEmployeeId = string.Empty;
                CommonMethodsDAL commonDal = new CommonMethodsDAL();
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);
                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                    ViewBag.EmployeeId = employeeId;
                }
                else
                {
                    decryptedEmployeeId = Convert.ToString(employeeID);
                    encryptedEmployeeId = commonDal.Encrypt(this.Session["SecurityKey"].ToString() + Convert.ToString(employeeID), true);
                    ViewBag.EmployeeId = encryptedEmployeeId;
                }
                //ViewBag.EmployeeId = employeeId;
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                List<tbl_PM_Employee_Dependands> objDependandantsList = new List<tbl_PM_Employee_Dependands>();

                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                PersonalDetailsViewModel objPersonalDetailModel = new PersonalDetailsViewModel();

                ViewBag.LoggedInEmployeeId = empdal.GetEmployeeID(Membership.GetUser().UserName);
                objPersonalDetailModel.SearchUserID = Convert.ToInt32(decryptedEmployeeId);

                HRMS_tbl_PM_Employee objPersonal = dal.GetEmployeePersonalDetails(Convert.ToInt32(decryptedEmployeeId));
                tbl_HR_Hobbies objHRHobbies = dal.GetEmployeeID(Convert.ToInt32(decryptedEmployeeId));
                tbl_HR_Achievement objHRAchievement = dal.GetEmployeeID_Achvment(Convert.ToInt32(decryptedEmployeeId));

                string user = Commondal.GetMaxRoleForUser(role);
                objPersonalDetailModel.Mail = new EmployeeMailTemplate();

                objPersonalDetailModel.UserRole = user;
                ViewBag.UserRole = user;
                objPersonalDetailModel.EmpStatusMasterID = dal.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                if (objPersonal != null)
                {
                    if (objPersonal.WeddingDate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                        objPersonal.WeddingDate = null;
                    if (objPersonal.SpouseBirthdate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                        objPersonal.SpouseBirthdate = null;
                    if (objPersonal.Child1Birthdate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                        objPersonal.Child1Birthdate = null;
                    if (objPersonal.Child2Birthdate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                        objPersonal.Child2Birthdate = null;
                    if (objPersonal.Child3Birthdate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                        objPersonal.Child3Birthdate = null;
                    if (objPersonal.Child4Birthdate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                        objPersonal.Child4Birthdate = null;
                    if (objPersonal.Child5Birthdate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                        objPersonal.Child5Birthdate = null;

                    if (objPersonal.ContractFrom == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                        objPersonal.ContractFrom = null;
                    if (objPersonal.ContractTo == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                        objPersonal.ContractTo = null;

                    objPersonalDetailModel.ContractEmployee = objPersonal.Contract_Employee.HasValue ? objPersonal.Contract_Employee.Value : false;
                    objPersonalDetailModel.ContractFrom = objPersonal.ContractFrom;
                    objPersonalDetailModel.ContractTo = objPersonal.ContractTo;

                    objPersonalDetailModel.EmployeeId = objPersonal.EmployeeID;
                    objPersonalDetailModel.UserName = objPersonal.UserName;
                    objPersonalDetailModel.EmployeeCode = objPersonal.EmployeeCode;
                    objPersonalDetailModel.ContractFrom = objPersonal.ContractFrom;
                    objPersonalDetailModel.ContractTo = objPersonal.ContractTo;
                    objPersonalDetailModel.Prefix = objPersonal.Prefix;
                    objPersonalDetailModel.FirstName = objPersonal.FirstName;
                    objPersonalDetailModel.MiddleName = objPersonal.MiddleName;
                    objPersonalDetailModel.LastName = objPersonal.LastName;
                    objPersonalDetailModel.MaritalStatus = objPersonal.MaritalStatus;
                    objPersonalDetailModel.BirthDate = objPersonal.BirthDate;
                    objPersonalDetailModel.Gender = objPersonal.Gender;
                    objPersonalDetailModel.WeddingDate = objPersonal.WeddingDate;
                    objPersonalDetailModel.Recognition = objPersonal.Recognition;
                    objPersonalDetailModel.NoOfchildren = objPersonal.NoOfChildren;
                    objPersonalDetailModel.AgreementDate = objPersonal.Agreement_Signed_Date;
                    objPersonalDetailModel.MaidanName = objPersonal.MaidenName;
                    objPersonalDetailModel.Remarks = objPersonal.Remarks;
                    objPersonalDetailModel.SpouseName = objPersonal.SpouseName;
                    objPersonalDetailModel.SpouseBirthDate = objPersonal.SpouseBirthdate;
                    objPersonalDetailModel.Child1Name = objPersonal.Child1Name;
                    objPersonalDetailModel.Child1BirthDate = objPersonal.Child1Birthdate;
                    objPersonalDetailModel.Child2Name = objPersonal.Child2Name;
                    objPersonalDetailModel.Child2BirthDate = objPersonal.Child2Birthdate;
                    objPersonalDetailModel.Child3Name = objPersonal.Child3Name;
                    objPersonalDetailModel.Child3BirthDate = objPersonal.Child3Birthdate;
                    objPersonalDetailModel.Child4Name = objPersonal.Child4Name;
                    objPersonalDetailModel.Child4BirthDate = objPersonal.Child4Birthdate;
                    objPersonalDetailModel.Child5Name = objPersonal.Child5Name;
                    objPersonalDetailModel.Child5BirthDate = objPersonal.Child5Birthdate;

                    if (System.IO.File.Exists(objPersonal.ProfileImagePath) == true)
                    {
                        objPersonalDetailModel.ProfileImageName = objPersonal.ProfileImageName;
                        objPersonalDetailModel.ProfileImagePath = objPersonal.ProfileImagePath;
                    }
                    else
                    {
                        objPersonalDetailModel.ProfileImageName = "browse-person-img.png";
                        objPersonalDetailModel.ProfileImagePath = Server.MapPath(Url.Content("/Images/New Design/browse-person-img.png"));
                    }

                    ViewBag.UserName = objPersonalDetailModel.UserName;
                    var currentyear = DateTime.Now.Year;
                    var currentMnth = DateTime.Now.Month;
                    var bithyear = DateTime.Parse(objPersonal.BirthDate.ToString()).Year;
                    var brthMonth = DateTime.Parse(objPersonal.BirthDate.ToString()).Month;
                    var ageYrs = currentyear - bithyear;
                    var ageMnths = currentMnth - brthMonth;

                    if (ageMnths < 0 || (ageMnths == 0 && DateTime.Now.Date < DateTime.Parse(objPersonal.BirthDate.ToString()).Date))
                        ageYrs--;

                    objPersonalDetailModel.Age = ageYrs.ToString() + " Yrs";
                }
                else
                {
                    bool checkboxStatus = false;
                    objPersonalDetailModel.EmployeeCode = dal.GetNewEmployeecode(checkboxStatus);
                    objPersonalDetailModel.EmployeeId = 0;
                }

                if (objHRHobbies != null)
                    objPersonalDetailModel.Hobbies = objHRHobbies.Decription;
                else
                    objPersonalDetailModel.Hobbies = "";

                if (objHRAchievement != null)
                    objPersonalDetailModel.Achievement = objHRAchievement.Decription;
                else
                    objPersonalDetailModel.Achievement = "";

                return PartialView("_PersonalDetails", objPersonalDetailModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors.." });
            }
        }

        [HttpGet]
        public ActionResult PersonalChanges(string employeeId)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "PersonalDetails");
            }
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                EmployeeChangesApprovalViewModel objEmployeeChangesApprovalModel = new EmployeeChangesApprovalViewModel();
                tbl_ApprovalChanges approvalchanges = dal.GetChangedFields(Convert.ToInt32(decryptedEmployeeId));
                HRMSDBEntities dbContext = new HRMSDBEntities();

                if (approvalchanges != null)
                {
                    objEmployeeChangesApprovalModel.OldValue = approvalchanges.OldValue;
                    objEmployeeChangesApprovalModel.NewValue = approvalchanges.NewValue;
                }
                int EmployeeID = Convert.ToInt32(decryptedEmployeeId);
                List<string> fieldlabellist = (from change in dbContext.tbl_ApprovalChanges
                                               where change.EmployeeID == EmployeeID && change.Module.Contains("Personal Details")
                                               orderby change.Id ascending
                                               select change.FieldDiscription).ToList();

                List<int?> approvalStatusIdList = (from change in dbContext.tbl_ApprovalChanges
                                                   where change.EmployeeID == EmployeeID && change.Module.Contains("Personal Details")
                                                   orderby change.Id ascending
                                                   select change.ApprovalStatusMasterID).ToList();

                List<DateTime?> approvedDateTimeList = (from change in dbContext.tbl_ApprovalChanges
                                                        where change.EmployeeID == EmployeeID && change.Module.Contains("Personal Details")
                                                        orderby change.Id ascending
                                                        select change.ApprovedDateTime).ToList();

                List<DateTime?> rejectedDateTimeList = (from change in dbContext.tbl_ApprovalChanges
                                                        where change.EmployeeID == EmployeeID && change.Module.Contains("Personal Details")
                                                        orderby change.Id ascending
                                                        select change.RejectedDateTime).ToList();

                List<string> approvalMessageList = new List<string>();
                string constantMessage;
                List<int> approvalFlagList = new List<int>();
                List<int> rejectFlagList = new List<int>();

                if (approvedDateTimeList != null)
                {
                    foreach (var item in approvedDateTimeList)
                    {
                        if (item != null)
                        {
                            if ((DateTime.Now - Convert.ToDateTime(item)).TotalHours < 72)
                            {
                                // show  message
                                approvalFlagList.Add(1);
                            }
                            else
                            {
                                // hide message
                                approvalFlagList.Add(0);
                            }
                        }
                        else
                        {
                            // new message
                            approvalFlagList.Add(2);
                        }
                    }
                }

                if (rejectedDateTimeList != null)
                {
                    foreach (var item in rejectedDateTimeList)
                    {
                        if (item != null)
                        {
                            if ((DateTime.Now - Convert.ToDateTime(item)).TotalHours < 72)
                            {
                                // show  message
                                rejectFlagList.Add(1);
                            }
                            else
                            {
                                // hide message
                                rejectFlagList.Add(0);
                            }
                        }
                        else
                        {
                            // new message
                            rejectFlagList.Add(2);
                        }
                    }
                }

                if (approvalStatusIdList != null)
                {
                    foreach (var item in approvalStatusIdList)
                    {
                        if (item == 0 || item == null)
                        {
                            constantMessage = ApprovalStatusMessages.NoAction_0;
                            approvalMessageList.Add(constantMessage);
                        }

                        if (item == 1)
                        {
                            constantMessage = ApprovalStatusMessages.OnHold_1;
                            approvalMessageList.Add(constantMessage);
                        }

                        if (item == 2)
                        {
                            constantMessage = ApprovalStatusMessages.Approved_2;
                            approvalMessageList.Add(constantMessage);
                        }

                        if (item == 3)
                        {
                            constantMessage = ApprovalStatusMessages.Rejected_3;
                            approvalMessageList.Add(constantMessage);
                        }
                    }
                }

                if (Request.IsAjaxRequest())
                {
                    return Json(new { status = true, label = fieldlabellist, approvalMessage = approvalMessageList, approveddatetime = approvedDateTimeList, rejecteddatetime = rejectedDateTimeList, approvalFlag = approvalFlagList, rejectFlag = rejectFlagList }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return PartialView("_PersonalDetails");
                }
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonalChanges(PersonalDetailsViewModel model)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogOn", "Account");
            }
            try
            {
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                SavePersonalDetailsResponse response = new SavePersonalDetailsResponse();
                HRMSDBEntities dbContext = new HRMSDBEntities();
                ViewBag.Roles = new SelectList(Roles.GetAllRoles(), "roleName");
                EmployeeChangesApprovalViewModel changes = new EmployeeChangesApprovalViewModel();

                int empID = (int)model.EmployeeId;
                tbl_ApprovalChanges _tbl_ApprovalChanges = new tbl_ApprovalChanges();
                var empPersonalDetails = dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeID == empID).SingleOrDefault();

                var createdby = (from creator in dbContext.HRMS_tbl_PM_Employee
                                 where creator.EmployeeID == empID
                                 select creator.EmployeeName).FirstOrDefault();

                bool IsUserNameExist = false;
                IsUserNameExist = dal.CheckUserNameApprovalExist(empID);

                if ((model.Gender == "Female" && (model.MaritalStatus != "Single") && (empPersonalDetails.MaritalStatus != model.MaritalStatus)) ||
                    (empPersonalDetails.FirstName.Trim() != model.FirstName.Trim() || empPersonalDetails.LastName.Trim() != model.LastName.Trim()))
                {
                    if ((empPersonalDetails.MaritalStatus == null || empPersonalDetails.MaritalStatus == "")
                  && (model.MaritalStatus == null || model.MaritalStatus == ""))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else if (((empPersonalDetails.FirstName == null || empPersonalDetails.FirstName == "")
                            && (model.FirstName == null || model.FirstName == "")) || ((empPersonalDetails.LastName == null || empPersonalDetails.LastName == "")
                            && (model.LastName == null || model.LastName == "")))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else if (IsUserNameExist == true)
                    {
                        //no changes made. This condition is added to identify whether the UserName field is already sent for Approval
                        //and is On Hold or No Action taken for same.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblUserName;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.UserName;
                        changes.NewValue = model.UserName;
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "UserName";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.MaritalStatus != model.MaritalStatus)
                {
                    if ((empPersonalDetails.MaritalStatus == null || empPersonalDetails.MaritalStatus == "")
                   && (model.MaritalStatus == null || model.MaritalStatus == ""))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblMaritalStatus;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.MaritalStatus;
                        changes.NewValue = model.MaritalStatus;
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "MaritalStatus";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.MaidenName != null && empPersonalDetails.MaidenName != "")
                    empPersonalDetails.MaidenName = empPersonalDetails.MaidenName.Trim();
                else
                    empPersonalDetails.MaidenName = empPersonalDetails.MaidenName;
                if (model.MaidanName != null && model.MaidanName != "")
                    model.MaidanName = model.MaidanName.Trim();
                else
                    model.MaidanName = model.MaidanName;

                if (empPersonalDetails.MaidenName != model.MaidanName)
                {
                    if ((empPersonalDetails.MaidenName == null || empPersonalDetails.MaidenName == "")
                    && (model.MaidanName == null || model.MaidanName == ""))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblMaidanName;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.MaidenName;
                        changes.NewValue = model.MaidanName;
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "MaidenName";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.WeddingDate != model.WeddingDate)
                {
                    if ((empPersonalDetails.WeddingDate == null || empPersonalDetails.WeddingDate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                        && model.WeddingDate == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblWeddingDate;
                        changes.Module = "Personal Details";
                        changes.OldValue = Convert.ToString(empPersonalDetails.WeddingDate);
                        changes.NewValue = Convert.ToString(model.WeddingDate);
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "WeddingDate";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.NoOfChildren != model.NoOfchildren)
                {
                    if ((empPersonalDetails.NoOfChildren == null || empPersonalDetails.NoOfChildren == 0)
                        && model.NoOfchildren == 0)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                        //The first condition will return true if the database is having null value in NoOfChildren and model returned 0 as the default value.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblNoOfchildren;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.NoOfChildren.ToString();
                        changes.NewValue = model.NoOfchildren.ToString();
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "NoOfChildren";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.SpouseName != null && empPersonalDetails.SpouseName != "")
                    empPersonalDetails.SpouseName = empPersonalDetails.SpouseName.Trim();
                else
                    empPersonalDetails.SpouseName = empPersonalDetails.SpouseName;
                if (model.SpouseName != null && model.SpouseName != "")
                    model.SpouseName = model.SpouseName.Trim();
                else
                    model.SpouseName = model.SpouseName;
                if (empPersonalDetails.SpouseName != model.SpouseName)
                {
                    if ((empPersonalDetails.SpouseName == null || empPersonalDetails.SpouseName == "")
                   && (model.SpouseName == null || model.SpouseName == ""))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblSpouseName;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.SpouseName;
                        changes.NewValue = model.SpouseName;
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "SpouseName";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.SpouseBirthdate != model.SpouseBirthDate)
                {
                    if ((empPersonalDetails.SpouseBirthdate == null || empPersonalDetails.SpouseBirthdate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                    && model.SpouseBirthDate == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblSpouseBirthDate;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.SpouseBirthdate.ToString();
                        changes.NewValue = model.SpouseBirthDate.ToString();
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "SpouseBirthDate";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.Child1Name != null && empPersonalDetails.Child1Name != "")
                    empPersonalDetails.Child1Name = empPersonalDetails.Child1Name.Trim();
                else
                    empPersonalDetails.Child1Name = empPersonalDetails.Child1Name;
                if (model.Child1Name != null && model.Child1Name != "")
                    model.Child1Name = model.Child1Name.Trim();
                else
                    model.Child1Name = model.Child1Name;
                if (empPersonalDetails.Child1Name != model.Child1Name)
                {
                    if ((empPersonalDetails.Child1Name == null || empPersonalDetails.Child1Name == "")
                        && (model.Child1Name == null || model.Child1Name == ""))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblChild1Name;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.Child1Name;
                        changes.NewValue = model.Child1Name;
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "Child1Name";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.Child1Birthdate != model.Child1BirthDate)
                {
                    if ((empPersonalDetails.Child1Birthdate == null || empPersonalDetails.Child1Birthdate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                    && model.Child1BirthDate == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblChild1BirthDate;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.Child1Birthdate.ToString();
                        changes.NewValue = model.Child1BirthDate.ToString();
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "Child1Birthdate";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.Child2Name != null && empPersonalDetails.Child2Name != "")
                    empPersonalDetails.Child2Name = empPersonalDetails.Child2Name.Trim();
                else
                    empPersonalDetails.Child2Name = empPersonalDetails.Child2Name;
                if (model.Child2Name != null && model.Child2Name != "")
                    model.Child2Name = model.Child2Name.Trim();
                else
                    model.Child2Name = model.Child2Name;
                if (empPersonalDetails.Child2Name != model.Child2Name)
                {
                    if ((empPersonalDetails.Child2Name == null || empPersonalDetails.Child2Name == "")
                        && (model.Child2Name == null || model.Child2Name == ""))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblChild2Name;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.Child2Name;
                        changes.NewValue = model.Child2Name;
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "Child2Name";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.Child2Birthdate != model.Child2BirthDate)
                {
                    if ((empPersonalDetails.Child2Birthdate == null || empPersonalDetails.Child2Birthdate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                    && model.Child2BirthDate == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblChild2BirthDate;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.Child2Birthdate.ToString();
                        changes.NewValue = model.Child2BirthDate.ToString();
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "Child2Birthdate";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.Child3Name != null && empPersonalDetails.Child3Name != "")
                    empPersonalDetails.Child3Name = empPersonalDetails.Child3Name.Trim();
                else
                    empPersonalDetails.Child3Name = empPersonalDetails.Child3Name;
                if (model.Child3Name != null && model.Child3Name != "")
                    model.Child3Name = model.Child3Name.Trim();
                else
                    model.Child3Name = model.Child3Name;

                if (empPersonalDetails.Child3Name != model.Child3Name)
                {
                    if ((empPersonalDetails.Child3Name == null || empPersonalDetails.Child3Name == "")
                        && (model.Child3Name == null || model.Child3Name == ""))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblChild3Name;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.Child3Name;
                        changes.NewValue = model.Child3Name;
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "Child3Name";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.Child3Birthdate != model.Child3BirthDate)
                {
                    if ((empPersonalDetails.Child3Birthdate == null || empPersonalDetails.Child3Birthdate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                    && model.Child3BirthDate == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblChild3BirthDate;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.Child3Birthdate.ToString();
                        changes.NewValue = model.Child3BirthDate.ToString();
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "Child3Birthdate";
                        response = dal.SaveChangedField(changes);
                    }
                }

                if (empPersonalDetails.Child4Name != null && empPersonalDetails.Child4Name != "")
                    empPersonalDetails.Child4Name = empPersonalDetails.Child4Name.Trim();
                else
                    empPersonalDetails.Child4Name = empPersonalDetails.Child4Name;
                if (model.Child4Name != null && model.Child4Name != "")
                    model.Child4Name = model.Child4Name.Trim();
                else
                    model.Child4Name = model.Child4Name;
                if (empPersonalDetails.Child4Name != model.Child4Name)
                {
                    if ((empPersonalDetails.Child4Name == null || empPersonalDetails.Child4Name == "")
                        && (model.Child4Name == null || model.Child4Name == ""))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblChild4Name;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.Child4Name;
                        changes.NewValue = model.Child4Name;
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "Child4Name";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.Child4Birthdate != model.Child4BirthDate)
                {
                    if ((empPersonalDetails.Child4Birthdate == null || empPersonalDetails.Child4Birthdate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                    && model.Child4BirthDate == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblChild4BirthDate;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.Child4Birthdate.ToString();
                        changes.NewValue = model.Child4BirthDate.ToString();
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "Child4Birthdate";
                        response = dal.SaveChangedField(changes);
                    }
                }

                if (empPersonalDetails.Child5Name != null && empPersonalDetails.Child5Name != "")
                    empPersonalDetails.Child5Name = empPersonalDetails.Child5Name.Trim();
                else
                    empPersonalDetails.Child5Name = empPersonalDetails.Child5Name;
                if (model.Child5Name != null && model.Child5Name != "")
                    model.Child5Name = model.Child5Name.Trim();
                else
                    model.Child5Name = model.Child5Name;
                if (empPersonalDetails.Child5Name != model.Child5Name)
                {
                    if ((empPersonalDetails.Child5Name == null || empPersonalDetails.Child5Name == "")
                        && (model.Child5Name == null || model.Child5Name == ""))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblChild5Name;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.Child5Name;
                        changes.NewValue = model.Child5Name;
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "Child5Name";
                        response = dal.SaveChangedField(changes);
                    }
                }
                if (empPersonalDetails.Child5Birthdate != model.Child5BirthDate)
                {
                    if ((empPersonalDetails.Child5Birthdate == null || empPersonalDetails.Child5Birthdate == Convert.ToDateTime(HRMS.Models.MinDate.MinValue))
                    && model.Child5BirthDate == null)
                    {
                        // no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblChild5BirthDate;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.Child5Birthdate.ToString();
                        changes.NewValue = model.Child5BirthDate.ToString();
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "Child5Birthdate";
                        response = dal.SaveChangedField(changes);
                    }
                }

                if (empPersonalDetails.Prefix != model.Prefix)
                {
                    if ((empPersonalDetails.Prefix == null || empPersonalDetails.Prefix == "")
                   && (model.Prefix == null || model.Prefix == ""))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblSalutation;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.Prefix;
                        changes.NewValue = model.Prefix;
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "Prefix";
                        response = dal.SaveChangedField(changes);
                    }
                }

                if (empPersonalDetails.FirstName.Trim() != model.FirstName.Trim())
                {
                    if ((empPersonalDetails.FirstName == null || empPersonalDetails.FirstName == "")
                   && (model.FirstName == null || model.FirstName == ""))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblFirstName;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.FirstName;
                        changes.NewValue = model.FirstName;
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "FirstName";
                        response = dal.SaveChangedField(changes);
                    }
                }

                if (empPersonalDetails.MiddleName != null && empPersonalDetails.MiddleName != "")
                    empPersonalDetails.MiddleName = empPersonalDetails.MiddleName.Trim();
                else
                    empPersonalDetails.MiddleName = empPersonalDetails.MiddleName;
                if (model.MiddleName != null && model.MiddleName != "")
                    model.MiddleName = model.MiddleName.Trim();
                else
                    model.MiddleName = model.MiddleName;
                if (empPersonalDetails.MiddleName != model.MiddleName)
                {
                    if ((empPersonalDetails.MiddleName == null || empPersonalDetails.MiddleName == "")
                   && (model.MiddleName == null || model.MiddleName == ""))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblMiddleName;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.MiddleName;
                        changes.NewValue = model.MiddleName;
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "MiddleName";
                        response = dal.SaveChangedField(changes);
                    }
                }

                if (empPersonalDetails.LastName.Trim() != model.LastName.Trim())
                {
                    if ((empPersonalDetails.LastName == null || empPersonalDetails.LastName == "")
                   && (model.LastName == null || model.LastName == ""))
                    {
                        //no changes made. This condition is added to identify the whether the changes are made by user or not.
                    }
                    else
                    {
                        changes.EmployeeID = empID;
                        changes.FieldDiscription = model.lblLastName;
                        changes.Module = "Personal Details";
                        changes.OldValue = empPersonalDetails.LastName;
                        changes.NewValue = model.LastName;
                        changes.CreatedBy = createdby;
                        changes.CreatedDateTime = DateTime.Now;
                        changes.FieldDbColumnName = "LastName";
                        response = dal.SaveChangedField(changes);
                    }
                }

                if (response.IsAdded != false)
                {
                    if (empPersonalDetails != null)
                    {
                        empPersonalDetails.MaritalStatus = model.MaritalStatus;
                        empPersonalDetails.WeddingDate = model.WeddingDate;
                        if (model.SpouseName != null && model.SpouseName != "")
                            empPersonalDetails.SpouseName = model.SpouseName.Trim();
                        else
                            empPersonalDetails.SpouseName = model.SpouseName;

                        empPersonalDetails.SpouseBirthdate = model.SpouseBirthDate;
                        empPersonalDetails.NoOfChildren = model.NoOfchildren;
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

                        empPersonalDetails.Prefix = model.Prefix;
                        empPersonalDetails.FirstName = model.FirstName.Trim();

                        if (model.MiddleName != null && model.MiddleName != "")
                            empPersonalDetails.MiddleName = model.MiddleName.Trim();
                        else
                            empPersonalDetails.MiddleName = model.MiddleName;
                        empPersonalDetails.LastName = model.LastName.Trim();

                        if (model.MaidanName != null && model.MaidanName != "")
                            empPersonalDetails.MaidenName = model.MaidanName.Trim();
                        else
                            empPersonalDetails.MaidenName = model.MaidanName;

                        dbContext.SaveChanges();
                    }
                }

                return Json(new { employeeId = response.EmployeeId, status = response.IsAdded, label = response.FieldLabels, approvalMessage = ApprovalStatusMessages.NoAction_0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// To Edit Personal details of Employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        //[ValidateInput(false)]
        public ActionResult PersonalDetails(HttpPostedFileBase blogpic, string tempProfileImagePath, PersonalDetailsViewModel model)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogOn", "Account");  //replace the null with a viewModel as needed.
            }
            bool isNewEmployeeStatus = true;
            try
            {
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                SavePersonalDetailsResponse response = new SavePersonalDetailsResponse();
                model.Mail = new EmployeeMailTemplate();
                string result = string.Empty;
                ViewBag.Roles = new SelectList(Roles.GetAllRoles(), "roleName");

                if (ModelState.IsValid == false)
                {
                    return Json(new { status = "Error" }, "text/html", JsonRequestBehavior.AllowGet);
                }

                if (blogpic != null && blogpic.ContentLength > 0 && !string.IsNullOrWhiteSpace(blogpic.FileName))
                {
                    string uploadsPath = (UploadProfileImageLocation);
                    string uploadpathwithId = Path.Combine(uploadsPath, (model.EmployeeId).ToString()).Replace("\\", "/");
                    string newuploadsPath = Path.Combine(uploadpathwithId, "Images").Replace("\\", "/");
                    string fileName = Path.GetFileName(blogpic.FileName).Replace("\\", "/");
                    string filePath = Path.Combine(newuploadsPath, fileName).Replace("\\", "/");
                    if (!Directory.Exists(newuploadsPath))
                        Directory.CreateDirectory(newuploadsPath);

                    blogpic.SaveAs(filePath);

                    model.ProfileImageName = fileName;
                    model.ProfileImagePath = filePath;
                }
                else if (!string.IsNullOrWhiteSpace(tempProfileImagePath))
                {
                    string uploadsPath = (UploadProfileImageLocation);
                    string uploadpathwithId = Path.Combine(uploadsPath, (model.EmployeeId).ToString()).Replace("\\", "/");
                    string newuploadsPath = Path.Combine(uploadpathwithId, "Images").Replace("\\", "/");
                    string fileName = Path.GetFileName(tempProfileImagePath).Replace("\\", "/");
                    string filePath = Path.Combine(newuploadsPath, fileName).Replace("\\", "/");
                    if (!Directory.Exists(newuploadsPath))
                        Directory.CreateDirectory(newuploadsPath);

                    string strTempProfileImahePath = Server.MapPath(string.Format("~/{0}", tempProfileImagePath));
                    System.IO.File.Copy(strTempProfileImahePath, filePath, true);

                    model.ProfileImageName = fileName;
                    model.ProfileImagePath = filePath;
                }

                if (model.EmployeeId > 0)
                    isNewEmployeeStatus = false;

                response = dal.SavePersonalDetails(model);
                string EmployeeId = Commondal.Encrypt(Session["SecurityKey"].ToString() + response.EmployeeId, true);
                if (response.EmployeeId > 0)
                    result = HRMS.Resources.Success.ResourceManager.GetString("SavePersonalDetailsSuccess");
                else
                    result = HRMS.Resources.Errors.ResourceManager.GetString("SavePersonalDetailsError");

                return Json(new { resultMesssage = result, employeeId = EmployeeId, status = response.IsAdded, isNewEmployee = isNewEmployeeStatus }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult MailSend(string employeeId, string Module)
        {
            bool isAuthorize;
            string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
            try
            {
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                PersonalDetailsViewModel model = new PersonalDetailsViewModel();
                EmployeeChangesApprovalViewModel change = new EmployeeChangesApprovalViewModel();
                model.Mail = new EmployeeMailTemplate();
                HRMS_tbl_PM_Employee employeeDetails = dal.GetEmployeePersonalDetails(Convert.ToInt32(decryptedEmployeeId));
                tbl_ApprovalChanges approvalchanges = dal.GetChangedFields(Convert.ToInt32(decryptedEmployeeId));
                if (approvalchanges != null)
                    change.Module = approvalchanges.Module;

                if (employeeDetails != null)
                {
                    model.Mail.From = employeeDetails.EmailID;
                    model.Mail.Cc = employeeDetails.EmailID;

                    int templateId = 0;
                    if (Module == "Personal Details")
                        templateId = 6;
                    else if (Module == "Residential Details")
                        templateId = 7;
                    else if (Module == "Qualification Details")
                    {
                        templateId = 8;
                    }
                    else if (Module == "Certification Details")
                        templateId = 9;
                    else if (Module == "Skill Details")
                        templateId = 10;

                    string mailBody = null;
                    List<EmployeeMailTemplate> template = Commondal.GetEmailTemplate(templateId);
                    foreach (var emailTemplate in template)
                    {
                        model.Mail.Subject = emailTemplate.Subject;
                        mailBody = emailTemplate.Message;
                    }
                    mailBody = mailBody.Replace("##employee name##", employeeDetails.EmployeeName);
                    mailBody = mailBody.Replace("##employee code##", employeeDetails.EmployeeCode);
                    model.Mail.Message = mailBody.Replace("<br>", Environment.NewLine);
                    ViewBag.Body = mailBody;
                    string[] roles = new[] { "" };

                    if (templateId != 10)
                        roles = new[] { "HR Admin" };
                    else
                        roles = new[] { "RMG" };

                    foreach (string r in roles)
                    {
                        string[] users = Roles.GetUsersInRole(r);

                        foreach (string user in users)
                        {
                            HRMS_tbl_PM_Employee employee = dal.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(user));
                            if (employee == null)
                                model.Mail.To = model.Mail.To + string.Empty;
                            else
                                model.Mail.To = model.Mail.To + employee.EmailID + ";";
                        }
                    }
                }
                bool result = false;
                char[] symbols = new char[] { ';', ' ', ',', '\r', '\n' };
                int CcCounter = 0;
                int ToCounter = 0;

                if (model.Mail.Cc != null)
                {
                    string CcMailIds = model.Mail.Cc.TrimEnd(symbols);
                    model.Mail.Cc = CcMailIds;
                    string[] EmailIds = CcMailIds.Split(symbols);
                    string[] EmailId = EmailIds.Where(s => !String.IsNullOrEmpty(s)).ToArray();
                    foreach (string id in EmailId)
                    {
                        HRMS_tbl_PM_Employee details = dal.GetEmployeeDetailsFromEmailId(id);

                        if (details != null)
                            CcCounter = 1;
                        else
                        {
                            CcCounter = 0;
                            break;
                        }
                    }

                    string[] EmailToId = model.Mail.To.Split(symbols);
                    string[] EmailToIds = EmailToId.Where(s => !String.IsNullOrEmpty(s)).ToArray();
                    foreach (string email in EmailToIds)
                    {
                        HRMS_tbl_PM_Employee details = dal.GetEmployeeDetailsFromEmailId(email);
                        if (details != null)

                            ToCounter = 1;
                        else
                        {
                            ToCounter = 0;
                            break;
                        }
                    }
                }
                else
                {
                    CcCounter = 1;
                    string[] EmailToIds = model.Mail.To.Split(symbols);
                    foreach (string email in EmailToIds)
                    {
                        HRMS_tbl_PM_Employee Details = dal.GetEmployeeDetailsFromEmailId(email);
                        if (Details != null)
                            ToCounter = 1;
                        else
                        {
                            ToCounter = 0;
                            break;
                        }
                    }
                }
                if (CcCounter == 1 && ToCounter == 1)
                {
                    result = SendMail(model.Mail);
                    if (Module == "Qualification Details")
                    {
                        bool send = Qual.MailSent(Convert.ToInt32(decryptedEmployeeId));
                    }
                    if (Module == "Certification Details")
                    {
                        bool send = Cert.MailSent(Convert.ToInt32(decryptedEmployeeId));
                    }
                    if (Module == "Skill Details")
                    {
                        bool send = dal.MailSent(Convert.ToInt32(decryptedEmployeeId));
                    }
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
            catch (SmtpFailedRecipientException e)
            {
                if (Module == "Qualification Details")
                {
                    bool send = Qual.MailSent(Convert.ToInt32(decryptedEmployeeId));
                }
                if (Module == "Certification Details")
                {
                    bool send = Cert.MailSent(Convert.ToInt32(decryptedEmployeeId));
                }
                if (Module == "Skill Details")
                {
                    bool send = dal.MailSent(Convert.ToInt32(decryptedEmployeeId));
                }
                string email = null;
                char[] symbols = { '<', '>' };
                if (e.Message != null)
                {
                    email = e.Message;
                    email = email.Trim(symbols);
                }
                return Json(new { status = "ErrorRecipient", failedRecipient = email }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool SendMail(EmployeeMailTemplate model)
        {
            SMTPHelper smtpHelper = new SMTPHelper();
            char[] symbols = new char[] { ';', ' ', ',', '\r', '\n' };
            if (model != null)
            {
                string[] ToEmailId = model.To.Split(symbols);

                //Loop to seperate email id's of CC peoples
                string[] CCEmailIds = null;
                if (model.Cc != "" && model.Cc != null)
                    CCEmailIds = model.Cc.Split(symbols);
                string[] CCEmailId = CCEmailIds.Where(s => !String.IsNullOrEmpty(s)).ToArray();
                return smtpHelper.SendMail(ToEmailId, null, CCEmailId, null, null, null, model.From, null, model.Subject, model.Message, null, null);
            }
            else
                return false;
        }

        public ActionResult GetNewEmployeecode(bool checkboxStatus)
        {
            PersonalDetailsDAL dal = new PersonalDetailsDAL();
            string NewEmpCode = string.Empty;
            try
            {
                NewEmpCode = dal.GetNewEmployeecode(checkboxStatus);
                return Json(new { NewEmpCode }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SwapEmpToPermanentAndContract(int empId, bool IsMoveToContract)
        {
            try
            {
                int Cotractid = dal.SwapEmpToPermanentAndContract(empId, IsMoveToContract);
                CommonMethodsDAL DAL = new CommonMethodsDAL();
                string encryptedEmployeeid = DAL.Encrypt(Convert.ToString(Session["SecurityKey"].ToString() + Cotractid), true);
                if (encryptedEmployeeid != null)
                    return Json(new { Status = true, NewContractEmpId = encryptedEmployeeid }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Status = false, NewContractEmpId = encryptedEmployeeid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EmpContractPermanentHistoryLoadGrid(int page, int rows, string employeeId)
        {
            PersonalDetailsViewModel model = new PersonalDetailsViewModel();
            int totalCount;
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                model.ContractPermanentList = dal.GetEmpContractPermanentHistory(page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);

                if ((model.ContractPermanentList == null || model.ContractPermanentList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model.ContractPermanentList = dal.GetEmpContractPermanentHistory(page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                }

                var jsonData = new
                {
                    total = (int)Math.Ceiling((double)totalCount / (double)rows),
                    page = page,
                    records = totalCount,
                    rows = model.ContractPermanentList
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        [PageAccess(PageName = "Dependent")]
        public ActionResult DependentDetails(string employeeId)
        {
            try
            {
                string decryptedEmployeeId = string.Empty;
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);

                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedEmployeeId = Convert.ToString(employeeID);

                DependentDetailsViewModel model = new DependentDetailsViewModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                model.UserRole = user;

                PersonalDetailsDAL personaldal = new PersonalDetailsDAL();

                model.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                model.DependantDetail = new DependantDetails();

                ViewBag.LoggedInEmployeeId = empdal.GetEmployeeID(Membership.GetUser().UserName);
                ViewBag.DependentEmployeeId = employeeId;

                // Added Year Drop Down List
                DAL.PersonalDetailsDAL dal = new DAL.PersonalDetailsDAL();
                model.DependantDetail = new Models.DependantDetails();
                model.DependantDetailsList = new List<DependantDetails>();
                List<tbl_PM_Employee_Dependands> DependantsDetails = dal.GetDependantsDetails(Convert.ToInt32(decryptedEmployeeId));

                model.EmpStatusMasterID = dal.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                if (DependantsDetails != null)
                {
                    foreach (tbl_PM_Employee_Dependands eachSkillDetails in DependantsDetails)
                    {
                        model.DependantDetailsList.Add(new DependantDetails()
                        {
                            EmployeeId = Convert.ToInt32(eachSkillDetails.EmployeeID),
                            DependandsId = Convert.ToInt32(eachSkillDetails.DependandsID),
                            DependandsName = eachSkillDetails.Name,
                            DependandsBirthDate = Convert.ToDateTime(eachSkillDetails.BirthDate),
                            DependandsAge = eachSkillDetails.Age,
                        });
                    }
                }

                List<tbl_PM_EmployeeRelationType> RelationList = dal.GetRelation();
                model.RelationList = new List<DependantDetails>();

                if (RelationList != null)
                {
                    foreach (tbl_PM_EmployeeRelationType relation in RelationList)
                    {
                        model.RelationList.Add(new DependantDetails()
                        {
                            uniqueID = relation.UniqueID,
                            DependandsRelation = relation.RelationType
                        });
                    }
                }

                return PartialView("_DependentDetails", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult DependentDetailsLoadGrid(string employeeId, int page, int rows)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                DependentDetailsViewModel objDependentDetailModel = new Models.DependentDetailsViewModel();
                objDependentDetailModel.DependantDetailsList = new List<Models.DependantDetails>();
                int totalCount;

                objDependentDetailModel.DependantDetailsList = dal.GetDependantsDetails(Convert.ToInt32(decryptedEmployeeId), page, rows, out totalCount);

                if ((objDependentDetailModel.DependantDetailsList == null || objDependentDetailModel.DependantDetailsList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    objDependentDetailModel.DependantDetailsList = dal.GetDependantsDetails(Convert.ToInt32(decryptedEmployeeId), page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = objDependentDetailModel.DependantDetailsList,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SaveDependantInfo(DependentDetailsViewModel model, int? RelationId, int? EmployeeID)
        {
            try
            {
                DAL.PersonalDetailsDAL dal = new DAL.PersonalDetailsDAL();
                string resultMessage = string.Empty;
                //var status = dal.SavedependantDetails(model, RelationId, EmployeeID);
                var status = true;
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteDependantDetails(int DependantID, string dependentEmployeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(dependentEmployeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                tbl_PM_Employee_Dependands dependantDetails = new tbl_PM_Employee_Dependands();
                bool eq = dal.DeletedependantDetails(DependantID, Convert.ToInt32(decryptedEmployeeId));
                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        // code added By Jitendra start
        [HttpGet]
        [PageAccess(PageName = "Declaration")]
        public ActionResult DeclarationMethodDetails(string employeeId)
        {
            try
            {
                string decryptedEmployeeId = string.Empty;
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);

                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedEmployeeId = Convert.ToString(employeeID);

                DeclarationDetailsViewModel model = new DeclarationDetailsViewModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                model.UserRole = user;

                PersonalDetailsDAL personaldal = new PersonalDetailsDAL();

                model.EmployeeID = Convert.ToInt32(decryptedEmployeeId);
                model.DeclarationDetail = new DeclarationsDetails();

                ViewBag.LoggedInEmployeeId = empdal.GetEmployeeID(Membership.GetUser().UserName);
                ViewBag.declarationEmployeeId = employeeId;

                // Added Year Drop Down List
                DAL.PersonalDetailsDAL dal = new DAL.PersonalDetailsDAL();
                model.DeclarationDetail = new Models.DeclarationsDetails();
                model.DeclarationDetailsList = new List<DeclarationsDetails>();

                List<tbl_PM_Employee_Declarations> declaratioDetails = dal.GetDeclarationsDetails(Convert.ToInt32(decryptedEmployeeId));

                model.EmpStatusMasterID = dal.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                if (declaratioDetails != null)
                {
                    foreach (tbl_PM_Employee_Declarations eachDeclarationDetails in declaratioDetails)
                    {
                        model.DeclarationDetailsList.Add(new DeclarationsDetails()
                        {
                            EmployeeID = Convert.ToInt32(eachDeclarationDetails.EmployeeID),
                            DeclarationId = Convert.ToInt32(eachDeclarationDetails.DeclarationId),
                            Name = eachDeclarationDetails.Name,
                            EmployeeCode = Convert.ToInt32(eachDeclarationDetails.EmployeeCode),
                            V2EmployeeName = eachDeclarationDetails.V2EmployeeName,
                            RelationshipName = eachDeclarationDetails.RelationshipName,
                        });
                    }
                }

                List<tbl_PM_EmployeeRelationType> RelationList = dal.GetRelation();
                model.RelationList = new List<DependantDetails>();

                if (RelationList != null)
                {
                    foreach (tbl_PM_EmployeeRelationType relation in RelationList)
                    {
                        model.RelationList.Add(new DependantDetails()
                        {
                            uniqueID = relation.UniqueID,
                            DependandsRelation = relation.RelationType
                        });
                    }
                }

                List<EpmType> addEmptype = new List<EpmType>
               {
                   new EpmType   { EmpTypeIds=1,  EmpTypeNames="Active"},
                   new EpmType{ EmpTypeIds=2,EmpTypeNames="Inactive"}
               };
                model.EmployeeList = new List<DeclarationsDetails>();

                if (addEmptype != null)
                {
                    foreach (EpmType emplist in addEmptype)
                    {
                        model.EmployeeList.Add(new DeclarationsDetails()
                        {
                            V2EmployeeID = emplist.EmpTypeIds,
                            V2EmployeeName = emplist.EmpTypeNames
                        });
                    }
                }

                return PartialView("_DeclarationDetails", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult DeclarationDetailsLoadGrid(string employeeId, int page, int rows)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                DeclarationDetailsViewModel objDeclarationDetailModel = new DeclarationDetailsViewModel();
                objDeclarationDetailModel.DeclarationDetailsList = new List<DeclarationsDetails>();
                int totalCount = 0;

                objDeclarationDetailModel.DeclarationDetailsList = dal.GetDeclarationsDetails(Convert.ToInt32(decryptedEmployeeId), page, rows, out totalCount);

                if ((objDeclarationDetailModel.DeclarationDetailsList == null || objDeclarationDetailModel.DeclarationDetailsList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    objDeclarationDetailModel.DeclarationDetailsList = dal.GetDeclarationsDetails(Convert.ToInt32(decryptedEmployeeId), page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = objDeclarationDetailModel.DeclarationDetailsList,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SaveDecalrationInfo(DeclarationDetailsViewModel model, int? RelationId, int? StatusID, int? EmployeeId)
        {
            DAL.PersonalDetailsDAL dal = new DAL.PersonalDetailsDAL();
            try
            {
                string resultMessage = string.Empty;

                var status = dal.SaveDeclarationDetails(model, RelationId, StatusID, EmployeeId);
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

        public ActionResult DeleteDeclarationDetails(int DeclarationID, string DeclarationEmployeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(DeclarationEmployeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                bool eq = dal.DeleteDeclarationDetails(DeclarationID, Convert.ToInt32(decryptedEmployeeId));
                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        //END

        public ActionResult CheckEmployee()
        {
            PersonalDetailsDAL dal = new PersonalDetailsDAL();
            List<EmployeeCodeList> employeecode = new List<EmployeeCodeList>();
            employeecode = dal.GetEmployeeCode();
            return Json(employeecode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Checks whether provided employee code is already exist in the database or not
        /// </summary>
        /// <param name="EmployeeCode">The string that need to check in the database for duplication</param>
        /// <returns>Returns Json Result(true, false). If EmployeeCodeAlreadyExist then returns false else returns true.</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult IsEmployeeCodeExist(string EmployeeCode)
        {
            PersonalDetailsDAL dal = new PersonalDetailsDAL();
            if (dal.IsEmployeeCodeExist(EmployeeCode))
                return Json(false, JsonRequestBehavior.AllowGet);//Sending false to show message as Employee Code Already Exist
            else
                return Json(true, JsonRequestBehavior.AllowGet);//Sending true so that MVC Validation will not show "Already Exist" message
        }

        /// <summary>
        /// Checks whether provided user name is already exist in the database or not
        /// </summary>
        /// <param name="username">The string that need to check in the database for duplication</param>
        /// <returns>Returns Json Result(true, false). If user name not exists then returns false else returns true.</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult IsEmployeeUserNameExist(string UserName)
        {
            PersonalDetailsDAL dal = new PersonalDetailsDAL();
            if (dal.CheckUserName(UserName))
                return Json(false, JsonRequestBehavior.AllowGet);//Sending false to show message as Employee Code Already Exist
            else
                return Json(true, JsonRequestBehavior.AllowGet);//Sending true so that MVC Validation will not show "Already Exist" message
        }

        /// <summary>
        ///  To retrieve Residential detail of Employee
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet]
        [PageAccess(PageName = "Residence")]
        public ActionResult ResidentialDetails(string employeeId)
        {
            try
            {
                string decryptedEmployeeId = string.Empty;
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);

                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedEmployeeId = Convert.ToString(employeeID);

                ViewBag.residentialEmployeeID = employeeId;
                ViewBag.residentialDecryptedEmployeeID = decryptedEmployeeId;

                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                ResidentialDetailsViewModel objResidentModel = new ResidentialDetailsViewModel();
                HRMS_tbl_PM_Employee objResident = dal.GetEmployeePersonalDetails(Convert.ToInt32(decryptedEmployeeId));
                objResidentModel.EmpStatusMasterID = dal.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                if (objResident != null)
                {
                    string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                    string user = Commondal.GetMaxRoleForUser(role);
                    objResidentModel.UserRole = user;

                    EmployeeDAL EmployeeDAL = new EmployeeDAL();

                    ViewBag.loggedinEmployeeID = employeeID;

                    objResidentModel.SearchedUserID = Convert.ToInt32(decryptedEmployeeId);
                    if (objResident.CountryID != null)
                        objResidentModel.country = objResident.CountryID.ToString();
                    else
                        objResidentModel.country = "Select";

                    if (objResident.CurrentCountryID != null)
                        objResidentModel.CurrentCountry = objResident.CurrentCountryID.ToString();
                    else
                        objResidentModel.CurrentCountry = "Select";

                    CountryDetails objDefault = new CountryDetails();
                    objDefault.CountryId = 0;
                    objDefault.CountryName = "Select";

                    CountryDetails objDefault1 = new CountryDetails();
                    objDefault1.CountryId = 0;
                    objDefault1.CountryName = "Select";

                    objResidentModel.CountryList = dal.GetCountryDetails();
                    objResidentModel.CountryList.Insert(0, objDefault);

                    objResidentModel.CurrentCountryList = dal.GetCountryDetails();
                    objResidentModel.CurrentCountryList.Insert(0, objDefault);

                    objResidentModel.CurrentAddress = objResident.CurrentAddress;
                    objResidentModel.CurrentCity = objResident.CurrentCity;
                    objResidentModel.CurrentState = objResident.CurrentState;
                    objResidentModel.CurrentZipCode = objResident.CurrentPinCode;
                    objResidentModel.Address = objResident.Address;
                    objResidentModel.City = objResident.City;
                    objResidentModel.State = objResident.State;
                    objResidentModel.ZipCode = objResident.PinCode;
                    objResidentModel.EmployeeId = objResident.EmployeeID;
                    return PartialView("_ResidentialDetails", objResidentModel);
                }
                else
                    return PartialView("_ResidentialDetails");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        /// <summary>
        ///  To Add Residential Details of employee
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResidentialDetails(ResidentialDetailsViewModel model)
        {
            try
            {
                string result = string.Empty;

                if (ModelState.IsValid == false)
                {
                    return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
                }
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                HRMS_tbl_PM_Employee residentialDetails = new HRMS_tbl_PM_Employee()
                {
                    CurrentAddress = model.CurrentAddress.Trim(),
                    CurrentCity = model.CurrentCity.Trim(),
                    CurrentState = model.CurrentState.Trim(),
                    CurrentPinCode = model.CurrentZipCode.Trim(),
                    Address = model.Address.Trim(),
                    City = model.City.Trim(),
                    State = model.State.Trim(),
                    PinCode = model.ZipCode.Trim(),
                    UserName = "test_UserName",
                    EmployeeID = model.EmployeeId,
                    CountryID = int.Parse(model.country),
                    CurrentCountryID = int.Parse(model.CurrentCountry)
                };

                var status = dal.AddResidentialDetails(residentialDetails);

                if (status)
                    result = HRMS.Resources.Success.ResourceManager.GetString("AddResidentialDetailsSuccess");
                else
                    result = HRMS.Resources.Errors.ResourceManager.GetString("AddResidentialDetailsError");

                return Json(new { resultMesssage = result, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ResidentialChanges(ResidentialDetailsViewModel model)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogOn", "Account");  //replace the null with a viewModel as needed.
            }
            try
            {
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                SavePersonalDetailsResponse response = new SavePersonalDetailsResponse();
                HRMSDBEntities dbContext = new HRMSDBEntities();
                ViewBag.Roles = new SelectList(Roles.GetAllRoles(), "roleName");
                EmployeeChangesApprovalViewModel changes = new EmployeeChangesApprovalViewModel();

                int empID = (int)model.EmployeeId;
                tbl_ApprovalChanges _tbl_ApprovalChanges = new tbl_ApprovalChanges();
                var empPersonalDetails = dbContext.HRMS_tbl_PM_Employee.Where(x => x.EmployeeID == empID).SingleOrDefault();

                var createdby = (from creator in dbContext.HRMS_tbl_PM_Employee
                                 where creator.EmployeeID == empID
                                 select creator.EmployeeName).FirstOrDefault();

                int currentcountryid = Convert.ToInt32(model.CurrentCountry);
                var currentCountryName = (from currentcountry in dbContext.tbl_PM_CountryMaster
                                          where currentcountry.CountryID == currentcountryid
                                          select currentcountry.CountryName).FirstOrDefault();

                int countryid = Convert.ToInt32(model.country);
                var CountryName = (from country in dbContext.tbl_PM_CountryMaster
                                   where country.CountryID == countryid
                                   select country.CountryName).FirstOrDefault();

                if (empPersonalDetails.CurrentAddress != null && empPersonalDetails.CurrentAddress != "")
                    empPersonalDetails.CurrentAddress = empPersonalDetails.CurrentAddress.Trim().Replace("\r", "");
                else
                    empPersonalDetails.CurrentAddress = empPersonalDetails.CurrentAddress;
                if (model.CurrentAddress != null && model.CurrentAddress != "")
                    model.CurrentAddress = model.CurrentAddress.Trim().Replace("\r", "");
                else
                    model.CurrentAddress = model.CurrentAddress;
                if (empPersonalDetails.CurrentAddress != model.CurrentAddress)
                {
                    changes.EmployeeID = empID;
                    changes.FieldDiscription = model.lblCurrentAddress;
                    changes.Module = "Residential Details";
                    changes.OldValue = empPersonalDetails.CurrentAddress;
                    changes.NewValue = model.CurrentAddress;
                    changes.CreatedBy = createdby;
                    changes.CreatedDateTime = DateTime.Now;
                    changes.FieldDbColumnName = "CurrentAddress";
                    response = dal.SaveChangedField(changes);
                }
                if (empPersonalDetails.CurrentCountryID != currentcountryid)
                {
                    changes.EmployeeID = empID;
                    changes.FieldDiscription = model.lblCurrentCountry;
                    changes.Module = "Residential Details";
                    changes.OldValue = empPersonalDetails.CurrentCountry;
                    changes.NewValue = currentCountryName;
                    changes.CreatedBy = createdby;
                    changes.CreatedDateTime = DateTime.Now;
                    changes.FieldDbColumnName = "CurrentCountry";
                    response = dal.SaveChangedField(changes);
                }
                if (empPersonalDetails.CurrentState != null && empPersonalDetails.CurrentState != "")
                    empPersonalDetails.CurrentState = empPersonalDetails.CurrentState.Trim();
                else
                    empPersonalDetails.CurrentState = empPersonalDetails.CurrentState;
                if (model.CurrentState != null && model.CurrentState != "")
                    model.CurrentState = model.CurrentState.Trim();
                else
                    model.CurrentState = model.CurrentState;
                if (empPersonalDetails.CurrentState != model.CurrentState)
                {
                    changes.EmployeeID = empID;
                    changes.FieldDiscription = model.lblCurrentState;
                    changes.Module = "Residential Details";
                    changes.OldValue = empPersonalDetails.CurrentState;
                    changes.NewValue = model.CurrentState;
                    changes.CreatedBy = createdby;
                    changes.CreatedDateTime = DateTime.Now;
                    changes.FieldDbColumnName = "CurrentState";
                    response = dal.SaveChangedField(changes);
                }
                if (empPersonalDetails.CurrentCity != null && empPersonalDetails.CurrentCity != "")
                    empPersonalDetails.CurrentCity = empPersonalDetails.CurrentCity.Trim();
                else
                    empPersonalDetails.CurrentCity = empPersonalDetails.CurrentCity;
                if (model.CurrentCity != null && model.CurrentCity != "")
                    model.CurrentCity = model.CurrentCity.Trim();
                else
                    model.CurrentCity = model.CurrentCity;
                if (empPersonalDetails.CurrentCity != model.CurrentCity)
                {
                    changes.EmployeeID = empID;
                    changes.FieldDiscription = model.lblCurrentCity;
                    changes.Module = "Residential Details";
                    changes.OldValue = empPersonalDetails.CurrentCity;
                    changes.NewValue = model.CurrentCity;
                    changes.CreatedBy = createdby;
                    changes.CreatedDateTime = DateTime.Now;
                    changes.FieldDbColumnName = "CurrentCity";
                    response = dal.SaveChangedField(changes);
                }
                if (empPersonalDetails.CurrentPinCode != null && empPersonalDetails.CurrentPinCode != "")
                    empPersonalDetails.CurrentPinCode = empPersonalDetails.CurrentPinCode.Trim();
                else
                    empPersonalDetails.CurrentPinCode = empPersonalDetails.CurrentPinCode;
                if (model.CurrentZipCode != null && model.CurrentZipCode != "")
                    model.CurrentZipCode = model.CurrentZipCode.Trim();
                else
                    model.CurrentZipCode = model.CurrentZipCode;
                if (empPersonalDetails.CurrentPinCode != model.CurrentZipCode)
                {
                    changes.EmployeeID = empID;
                    changes.FieldDiscription = model.lblCurrentZipCode;
                    changes.Module = "Residential Details";
                    changes.OldValue = empPersonalDetails.CurrentPinCode;
                    changes.NewValue = model.CurrentZipCode;
                    changes.CreatedBy = createdby;
                    changes.CreatedDateTime = DateTime.Now;
                    changes.FieldDbColumnName = "CurrentPinCode";
                    response = dal.SaveChangedField(changes);
                }
                if (empPersonalDetails.Address != null && empPersonalDetails.Address != "")
                    empPersonalDetails.Address = empPersonalDetails.Address.Trim().Replace("\r", "");
                else
                    empPersonalDetails.Address = empPersonalDetails.Address;
                if (model.Address != null && model.Address != "")
                    model.Address = model.Address.Trim().Replace("\r", "");
                else
                    model.Address = model.Address;
                if (empPersonalDetails.Address != model.Address)
                {
                    changes.EmployeeID = empID;
                    changes.FieldDiscription = model.lblAddress;
                    changes.Module = "Residential Details";
                    changes.OldValue = empPersonalDetails.Address;
                    changes.NewValue = model.Address;
                    changes.CreatedBy = createdby;
                    changes.CreatedDateTime = DateTime.Now;
                    changes.FieldDbColumnName = "Address";
                    response = dal.SaveChangedField(changes);
                }
                if (empPersonalDetails.CountryID != countryid)
                {
                    changes.EmployeeID = empID;
                    changes.FieldDiscription = model.lblCountry;
                    changes.Module = "Residential Details";
                    changes.OldValue = empPersonalDetails.Country;
                    changes.NewValue = CountryName;
                    changes.CreatedBy = createdby;
                    changes.CreatedDateTime = DateTime.Now;
                    changes.FieldDbColumnName = "Country";
                    response = dal.SaveChangedField(changes);
                }
                if (empPersonalDetails.State != null && empPersonalDetails.State != "")
                    empPersonalDetails.State = empPersonalDetails.State.Trim();
                else
                    empPersonalDetails.State = empPersonalDetails.State;
                if (model.State != null && model.State != "")
                    model.State = model.State.Trim();
                else
                    model.State = model.State;
                if (empPersonalDetails.State != model.State)
                {
                    changes.EmployeeID = empID;
                    changes.FieldDiscription = model.lblState;
                    changes.Module = "Residential Details";
                    changes.OldValue = empPersonalDetails.State;
                    changes.NewValue = model.State;
                    changes.CreatedBy = createdby;
                    changes.CreatedDateTime = DateTime.Now;
                    changes.FieldDbColumnName = "State";
                    response = dal.SaveChangedField(changes);
                }
                if (empPersonalDetails.City != null && empPersonalDetails.City != "")
                    empPersonalDetails.City = empPersonalDetails.City.Trim();
                else
                    empPersonalDetails.City = empPersonalDetails.City;
                if (model.City != null && model.City != "")
                    model.City = model.City.Trim();
                else
                    model.City = model.City;
                if (empPersonalDetails.City != model.City)
                {
                    changes.EmployeeID = empID;
                    changes.FieldDiscription = model.lblCity;
                    changes.Module = "Residential Details";
                    changes.OldValue = empPersonalDetails.City;
                    changes.NewValue = model.City;
                    changes.CreatedBy = createdby;
                    changes.CreatedDateTime = DateTime.Now;
                    changes.FieldDbColumnName = "City";
                    response = dal.SaveChangedField(changes);
                }
                if (empPersonalDetails.PinCode != null && empPersonalDetails.PinCode != "")
                    empPersonalDetails.PinCode = empPersonalDetails.PinCode.Trim();
                else
                    empPersonalDetails.PinCode = empPersonalDetails.PinCode;
                if (model.ZipCode != null && model.ZipCode != "")
                    model.ZipCode = model.ZipCode.Trim();
                else
                    model.ZipCode = model.ZipCode;
                if (empPersonalDetails.PinCode != model.ZipCode)
                {
                    changes.EmployeeID = empID;
                    changes.FieldDiscription = model.lblZipCode;
                    changes.Module = "Residential Details";
                    changes.OldValue = empPersonalDetails.PinCode;
                    changes.NewValue = model.ZipCode;
                    changes.CreatedBy = createdby;
                    changes.CreatedDateTime = DateTime.Now;
                    changes.FieldDbColumnName = "PinCode";
                    response = dal.SaveChangedField(changes);
                }

                if (response.IsAdded != false)
                {
                    if (empPersonalDetails != null)
                    {
                        empPersonalDetails.CurrentAddress = model.CurrentAddress.Trim();
                        if (currentCountryName != null && currentCountryName != "")
                            currentCountryName = currentCountryName.Trim();
                        empPersonalDetails.CurrentCountry = currentCountryName;
                        empPersonalDetails.CurrentCountryID = int.Parse(model.CurrentCountry);
                        empPersonalDetails.CurrentState = model.CurrentState.Trim();
                        empPersonalDetails.CurrentCity = model.CurrentCity.Trim();
                        empPersonalDetails.CurrentPinCode = model.CurrentZipCode.Trim();
                        empPersonalDetails.Address = model.Address.Trim();
                        if (CountryName != null && CountryName != "")
                            CountryName = CountryName.Trim();
                        empPersonalDetails.Country = CountryName;
                        empPersonalDetails.CountryID = int.Parse(model.country);
                        empPersonalDetails.State = model.State.Trim();
                        empPersonalDetails.City = model.City.Trim();
                        empPersonalDetails.PinCode = model.ZipCode.Trim();
                        dbContext.SaveChanges();
                    }
                }
                return Json(new { employeeId = response.EmployeeId, status = response.IsAdded, label = response.FieldLabels, approvalMessage = ApprovalStatusMessages.NoAction_0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ResidentialChanges(string employeeId)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "PersonalDetails");
            }
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                EmployeeChangesApprovalViewModel objEmployeeChangesApprovalModel = new EmployeeChangesApprovalViewModel();
                tbl_ApprovalChanges approvalchanges = dal.GetChangedFields(Convert.ToInt32(decryptedEmployeeId));
                HRMSDBEntities dbContext = new HRMSDBEntities();
                int EmployeeID = Convert.ToInt32(decryptedEmployeeId);
                List<string> fieldlabellist = (from change in dbContext.tbl_ApprovalChanges
                                               where change.EmployeeID == EmployeeID && change.Module.Contains("Residential Details")
                                               orderby change.Id ascending
                                               select change.FieldDiscription).ToList();

                List<int?> approvalStatusIdList = (from change in dbContext.tbl_ApprovalChanges
                                                   where change.EmployeeID == EmployeeID && change.Module.Contains("Residential Details")
                                                   orderby change.Id ascending
                                                   select change.ApprovalStatusMasterID).ToList();

                List<DateTime?> approvedDateTimeList = (from change in dbContext.tbl_ApprovalChanges
                                                        where change.EmployeeID == EmployeeID && change.Module.Contains("Residential Details")
                                                        orderby change.Id ascending
                                                        select change.ApprovedDateTime).ToList();

                List<DateTime?> rejectedDateTimeList = (from change in dbContext.tbl_ApprovalChanges
                                                        where change.EmployeeID == EmployeeID && change.Module.Contains("Residential Details")
                                                        orderby change.Id ascending
                                                        select change.RejectedDateTime).ToList();

                List<string> approvalMessageList = new List<string>();
                List<int> approvalFlagList = new List<int>();
                List<int> rejectFlagList = new List<int>();

                if (approvedDateTimeList != null)
                {
                    foreach (var item in approvedDateTimeList)
                    {
                        if (item != null)
                        {
                            if ((DateTime.Now - Convert.ToDateTime(item)).TotalHours < 72)
                            {
                                // show  message
                                approvalFlagList.Add(1);
                            }
                            else
                            {
                                // hide message
                                approvalFlagList.Add(0);
                            }
                        }
                        else
                        {
                            // hide message
                            approvalFlagList.Add(2);
                        }
                    }
                }

                if (rejectedDateTimeList != null)
                {
                    foreach (var item in rejectedDateTimeList)
                    {
                        if (item != null)
                        {
                            if ((DateTime.Now - Convert.ToDateTime(item)).TotalHours < 72)
                            {
                                // show  message
                                rejectFlagList.Add(1);
                            }
                            else
                            {
                                // hide message
                                rejectFlagList.Add(0);
                            }
                        }
                        else
                        {
                            // hide message
                            rejectFlagList.Add(2);
                        }
                    }
                }

                string constantMessage;

                if (approvalStatusIdList != null)
                {
                    foreach (var item in approvalStatusIdList)
                    {
                        if (item == 0 || item == null)
                        {
                            constantMessage = ApprovalStatusMessages.NoAction_0;
                            approvalMessageList.Add(constantMessage);
                        }

                        if (item == 1)
                        {
                            constantMessage = ApprovalStatusMessages.OnHold_1;
                            approvalMessageList.Add(constantMessage);
                        }

                        if (item == 2)
                        {
                            constantMessage = ApprovalStatusMessages.Approved_2;
                            approvalMessageList.Add(constantMessage);
                        }

                        if (item == 3)
                        {
                            constantMessage = ApprovalStatusMessages.Rejected_3;
                            approvalMessageList.Add(constantMessage);
                        }
                    }
                }

                if (Request.IsAjaxRequest())
                {
                    return Json(new { status = true, label = fieldlabellist, approvalMessage = approvalMessageList, approveddatetime = approvedDateTimeList, rejecteddatetime = rejectedDateTimeList, approvalFlag = approvalFlagList, rejectFlag = rejectFlagList }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return PartialView("_ResidentialDetails");
                }
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// To retrieve Contact Details of ane employee
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PageAccess(PageName = "Contact")]
        public ActionResult ContactDetails(string employeeId)
        {
            try
            {
                string decryptedEmployeeId = string.Empty;
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);

                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedEmployeeId = Convert.ToString(employeeID);

                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                ViewBag.loggedinEmployeeID = employeeID;

                ContactDetailsViewModel model = new ContactDetailsViewModel();
                model.EmergencyContactModel = new EmergencyContactViewModel();
                HRMS_tbl_PM_Employee objContactDetails = dal.GetEmployeePersonalDetails(Convert.ToInt32(decryptedEmployeeId));
                ViewBag.UserName = dal.GetEmployeeUserName(Convert.ToInt32(decryptedEmployeeId));
                model.UserRole = user;
                model.EmergencyContactModel.UserRole = user;
                ViewBag.SearchedUserID = Convert.ToInt32(decryptedEmployeeId);
                ViewBag.ContactEmployeeId = employeeId;
                model.EmpStatusMasterID = dal.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                model.EmergencyContactModel.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                List<tbl_PM_EmployeeEmergencyContact> objEmergencyContactDetails = dal.GetEmployeeEmergencyContactDetails(Convert.ToInt32(decryptedEmployeeId));

                if (objContactDetails != null)
                {
                    model.SeatingLocation = objContactDetails.SeatingLocation;
                    model.AlternateEmailId = objContactDetails.EmailID3;
                    model.EmployeeId = objContactDetails.EmployeeID;
                    model.GtalkId = objContactDetails.PersonalEmailID; // PersonalEmailID column is mapped as GTalkID in VW for HCM mapping.
                    model.MobileNumber = objContactDetails.MobileNumber;
                    model.OfficeEmailId = objContactDetails.EmailID;
                    model.ResidenceNumber = objContactDetails.CurrentPhone;
                    model.SkypeId = objContactDetails.GTailkID;
                    model.AlternateContactNumber = objContactDetails.AlternateContactNumber;
                    model.OfficeVoip = objContactDetails.VoIP;
                    model.PersonalEmailId = objContactDetails.EmailID1;
                    model.ResidenceVoip = objContactDetails.ResidenceVoIP;
                    model.YIMId = objContactDetails.YIM;
                }

                if (objEmergencyContactDetails != null)
                {
                    foreach (tbl_PM_EmployeeEmergencyContact eachContactDetails in objEmergencyContactDetails)
                    {
                        model.EmergencyContactModel.EmployeeId = eachContactDetails.EmployeeID;
                        model.EmergencyContactModel.EmployeeEmergencyContactId = eachContactDetails.EmployeeEmergencyContactID;
                        model.EmergencyContactModel.Name = eachContactDetails.Name;
                        model.EmergencyContactModel.EmgAddress = eachContactDetails.Address;
                        model.EmergencyContactModel.ContactNo = eachContactDetails.ContactNo;
                        model.EmergencyContactModel.EmailId = eachContactDetails.EmailID;
                        model.EmergencyContactModel.uniqueID = eachContactDetails.RelationTypeID;
                    }
                }

                ViewBag.RelationTypeList = dal.GetRelationList();
                return PartialView("_ContactDetails", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult ContactDetails(ContactDetailsViewModel model)
        {
            try
            {
                string result = string.Empty;
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                var success = dal.SaveContactDetails(model);
                if (success)
                    result = HRMS.Resources.Success.ResourceManager.GetString("SaveContactDetailsSuccess");
                else
                    result = HRMS.Resources.Errors.ResourceManager.GetString("SaveContactDetailsError");

                return Json(new { resultMesssage = result, status = success, employeeId = model.EmployeeId }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EmergencyContactLoadGrid(string employeeId, int page, int rows)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                //ContactDetailsViewModel model = new ContactDetailsViewModel();
                List<EmergencyContactViewModel> model = new List<EmergencyContactViewModel>();

                int totalCount;
                model = dal.GetEmployeeEmergencyContactDetails(Convert.ToInt32(decryptedEmployeeId), page, rows, out totalCount);
                if ((model == null || model.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model = dal.GetEmployeeEmergencyContactDetails(Convert.ToInt32(decryptedEmployeeId), page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = model,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult DeleteEmployeeEmergencyContact(int EmployeeEmergencyContactId, string contactEmployeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(contactEmployeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                DAL.PersonalDetailsDAL dal = new DAL.PersonalDetailsDAL();
                bool status = dal.DeleteEmployeeEmergencyContact(EmployeeEmergencyContactId, Convert.ToInt32(decryptedEmployeeId));
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddEmployeeEmergencyContact(EmergencyContactViewModel model, int? RelationId, int? EmployeeID)
        {
            try
            {
                DAL.PersonalDetailsDAL dal = new DAL.PersonalDetailsDAL();
                string result = string.Empty;
                //var success = dal.AddEmployeeEmergencyContact(model, RelationId, EmployeeID);
                var success = true;
                if (success)
                    result = HRMS.Resources.Success.ResourceManager.GetString("SaveContactDetailsSuccess");
                else
                    result = HRMS.Resources.Errors.ResourceManager.GetString("SaveContactDetailsError");
                return Json(new { result = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Action will fire on click of 'Ok' button on a popup for saving/adding new skill
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveEmployeeskillDetails(SkillDetailsViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                bool skillError = false;
                bool skillLevelError = false;

                if (ViewData.ModelState["NewSkillDetail.Skill"].Errors.Count > 0)
                {
                    skillError = true;
                }

                return Json(new { skill = skillError, skillLevel = skillLevelError, status = false });
            }

            string result = string.Empty;
            DAL.PersonalDetailsDAL dal = new PersonalDetailsDAL();
            try
            {
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);

                string user = Commondal.GetMaxRoleForUser(role);
                bool IsLoggedInEmployee = false;

                if (user != "RMG")
                    IsLoggedInEmployee = true;
                else
                    IsLoggedInEmployee = false;

                var status = dal.AddEmployeeSkillDetails(model.NewSkillDetail, IsLoggedInEmployee);
                if (status)
                    result = "Skill Added Successfully";
                else
                    result = "Error in adding Skill";

                return Json(new { resultMessage = result, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [PageAccess(PageName = "Skill")]
        public ActionResult EmployeeSkillDetails(string employeeId)
        {
            try
            {
                SkillDetailsViewModel model = new SkillDetailsViewModel();
                string decryptedEmployeeId = string.Empty;
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);
                ViewBag.IsRating = dal.GetRatingsDetails();
                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedEmployeeId = Convert.ToString(employeeID);

                model.SearchUserID = Convert.ToInt32(decryptedEmployeeId);//search user id
                ViewBag.LoggedInUserId = employeeID;// loggedin employeeid
                ViewBag.skillEmployeeId = employeeId;//encrypted employee id
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                model.UserRole = user;
                SemDAL semDAL = new SemDAL();
                int SEMEmployeeId = semDAL.geteEmployeeIDFromSEMDatabase(Membership.GetUser().UserName);
                ViewBag.loggedinEmployeeID = SEMEmployeeId;
                model.EmployeeId = SEMEmployeeId.ToString();
                return PartialView("SkillMatrix", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult LoadSkillDetailRecords(string employeeId, int page, int rows)
        {
            try
            {
                ViewBag.IsRating = dal.GetRatingsDetails();
                int EmployeeId = Convert.ToInt32(employeeId);
                PersonalDetailsDAL objPDetail = new PersonalDetailsDAL();
                int totalCount;
                List<SkillDetailsViewModel> skill_details = new List<SkillDetailsViewModel>();
                skill_details = objPDetail.GetSkillDeeetails(EmployeeId, page, rows, out totalCount);
                if ((skill_details == null || skill_details.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    skill_details = objPDetail.GetSkillDeeetails(EmployeeId, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = skill_details,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        /// <summary>
        /// Action will fire to delete the skill details from database of respective employeeskillId,
        /// On a click of OK button Of Popup asking for Deletion.
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public ActionResult DeleteEmployeeSkillDetails(int employeeSkillId, string employeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                if (dal.DeleteSkillDetails(employeeSkillId, Convert.ToInt32(decryptedEmployeeId)))
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// returns the View for Employee Qualifications
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet]
        [PageAccess(PageName = "Education")]
        public ActionResult EmployeeQualifications(string employeeId)
        {
            try
            {
                string decryptedEmployeeId = string.Empty;
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);

                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedEmployeeId = Convert.ToString(employeeID);

                EmployeeQualificationsViewModel model = new EmployeeQualificationsViewModel();
                model.EmployeeID = Convert.ToInt32(decryptedEmployeeId);
                ViewBag.QualificationEmployeeID = employeeId;
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                model.UserRole = user;
                ViewBag.loggedinEmployeeID = employeeID;

                PersonalDetailsDAL personaldal = new PersonalDetailsDAL();
                int birthDateYear = Convert.ToDateTime(personaldal.GetEmployeeBirthDate(Convert.ToInt32(decryptedEmployeeId))).Year;
                model.bithDate = birthDateYear;

                List<YearListClass> listOfYears = new List<YearListClass>();
                for (int i = birthDateYear; i <= DateTime.Now.Year; i++)
                {
                    listOfYears.Add(new YearListClass { Year = birthDateYear.ToString(), YearID = birthDateYear });
                    birthDateYear++;
                }

                model.YearList = listOfYears;
                model.EmpStatusMasterID = personaldal.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));

                model.DegreeList = new List<DegreeListClass>();
                model.NewEmployeeQualification = new EmployeeQualifications();
                model.QualificationList = new List<QualificationListClass>();
                model.TypeList = new List<TypeListClass>();

                QualificationDetailsDAL dal = new QualificationDetailsDAL();
                List<HRMS_tbl_PM_Qualifications> qualifications = dal.GetEmployeeQualificationList();
                List<tbl_PM_QualificationType> qualificationTypes = dal.GetQualificationTypeList();
                List<HRMS_tbl_PM_QualificationGroupMaster> qualificationGroupMasters = dal.GetDegreeList();

                foreach (HRMS_tbl_PM_QualificationGroupMaster qualificationGroupMaster in qualificationGroupMasters)
                {
                    model.DegreeList.Add(new DegreeListClass()
                    {
                        DegreeID = qualificationGroupMaster.QualificationGroupID,
                        Degree = qualificationGroupMaster.QualificationGroupName
                    });
                }

                foreach (HRMS_tbl_PM_Qualifications qualification in qualifications)
                {
                    model.QualificationList.Add(new QualificationListClass()
                    {
                        QualificationID = qualification.QualificationID,
                        Qualification = qualification.QualificationName
                    });
                }

                foreach (tbl_PM_QualificationType qualificationType in qualificationTypes)
                {
                    model.TypeList.Add(new TypeListClass()
                    {
                        TypeID = qualificationType.QualificationTypeID,
                        Type = qualificationType.QualificationTypeName
                    });
                }

                bool send = dal.CanSendMail(Convert.ToInt32(decryptedEmployeeId));
                if (send)
                    ViewBag.SendMail = "CanSend";
                else
                    ViewBag.SendMail = "CanNotSend";

                ViewBag.qualificationEmployeeID = employeeId;
                return PartialView("_EmployeeQualifications", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        /// <summary>
        /// Loads the grid view with the employees Qualification Details
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EmployeeQualificationsLoadGrid(int page, int rows, string employeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                QualificationDetailsDAL dal = new QualificationDetailsDAL();
                //EmployeeQualificationsViewModel model = new EmployeeQualificationsViewModel();
                List<EmployeeQualifications> model = new List<Models.EmployeeQualifications>();

                int totalCount;
                model = dal.GetEmployeeQualificationsOtherDetails(page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);

                if ((model == null || model.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model = dal.GetEmployeeQualificationsOtherDetails(page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                }

                var jsonData = new
                {
                    total = (int)Math.Ceiling((double)totalCount / (double)rows),
                    page = page,
                    records = totalCount,
                    rows = model
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        /// <summary>
        /// Adds Details of the employee like Specilization, Institute and University
        /// </summary>
        /// <param name="model">Takes all the values of the Employee Qualification as input to add into database</param>
        /// <returns>Returns success or failure message of Employee Qualification addition</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEmployeeQualifications(EmployeeQualifications model, int? SelectedQualificationID, int? SelectedDegreeID, int? SelectedYearID, int? SelectedTypeID, int? EmployeeId)
        {
            try
            {
                //DAL.QualificationDetailsDAL dal = new DAL.QualificationDetailsDAL();
                //int? empid = EmployeeId;
                string resultMessage = string.Empty;
                //string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                //int LoggedinEmployeeId = empdal.GetEmployeeID(Membership.GetUser().UserName);
                //model.EmployeeID = LoggedinEmployeeId;
                //string user = Commondal.GetMaxRoleForUser(role);
                //bool IsLoggedInEmployee = false;

                //if (empid == model.EmployeeID)
                //    IsLoggedInEmployee = true;
                //else
                //    IsLoggedInEmployee = false;
                var status = true;
                //var status = dal.SaveEmployeeQualification(model, IsLoggedInEmployee, SelectedQualificationID, SelectedDegreeID, SelectedYearID, SelectedTypeID, EmployeeId);
                if (status)
                {
                    resultMessage = "Saved";
                }
                else
                    resultMessage = "Error";
                return Json(new { results = resultMessage, status = "saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///
        /// To delete qualification details of employee by id
        /// </summary>
        /// <param name="employeeQualificationID"></param>
        /// <returns></returns>
        public ActionResult DeleteQualificationDetails(int employeeQualificationID, string employeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                QualificationDetailsDAL dal = new QualificationDetailsDAL();
                bool status = dal.DeleteEmployeeQualification(employeeQualificationID, Convert.ToInt32(decryptedEmployeeId));
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddEmployeeMedicalHistory(MedicalHistoryDetailsViewModel model, int? YearID, int? EmployeeID, int? MedicalDescId)
        {
            try
            {
                int Mesicaldescid = Convert.ToInt32(MedicalDescId);
                DAL.PersonalDetailsDAL dal = new DAL.PersonalDetailsDAL();
                tbl_PM_MedicalDescription employeeMedicalDetails = new tbl_PM_MedicalDescription()
                {
                    Employee_Id = EmployeeID,
                    MedicalDesc_Id = Mesicaldescid,
                    Medical_Description = model.MedicalDescription.Trim(),
                    Year = YearID.ToString()
                };

                //string resultMessage = string.Empty;
               // bool status = dal.AddEmployeeMedicalHistory(employeeMedicalDetails);
                bool status = false;
                //if (status)
                //    resultMessage = HRMS.Resources.Success.ResourceManager.GetString("AddMedicalDetailsSuccess");
                //else
                //    resultMessage = HRMS.Resources.Errors.ResourceManager.GetString("AddMedicalDetailsError");
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// To get blood group of employee from model and save in database
        /// </summary>
        /// <param name="bloodGroup"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveBloodGroup(string bgId, string employeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                PersonalDetailsDAL dal = new DAL.PersonalDetailsDAL();
                string resultMessage = string.Empty;
                bool status = dal.SaveBloodGroup(bgId, Convert.ToInt32(decryptedEmployeeId));
                if (status)
                    resultMessage = HRMS.Resources.Success.ResourceManager.GetString("AddMedicalDetailsSuccess");
                else
                    resultMessage = HRMS.Resources.Errors.ResourceManager.GetString("AddMedicalDetailsError");
                return Json(new { result = resultMessage, status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// To retrieve Employee Medical History
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        ///
        [HttpGet]
        [PageAccess(PageName = "Medical History")]
        public ActionResult MedicalHistory(string employeeId)
        {
            try
            {
                string decryptedEmployeeId = string.Empty;
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);

                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedEmployeeId = Convert.ToString(employeeID);

                MedicalHistoryDetailsViewModel ObjMedicalDescModel = new MedicalHistoryDetailsViewModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                ObjMedicalDescModel.UserRole = user;

                ViewBag.LoggedInEmployeeId = employeeID;
                ViewBag.MedicalEmployeeId = employeeId;
                //ObjMedicalDescModel.EmployeeId = employeeID;
                PersonalDetailsDAL personaldal = new PersonalDetailsDAL();
                ObjMedicalDescModel.EmpStatusMasterID = personaldal.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                int birthdt = Convert.ToDateTime(personaldal.GetEmployeeBirthDate(Convert.ToInt32(decryptedEmployeeId))).Year;
                int currentyear = DateTime.Now.Year;
                ObjMedicalDescModel.birthDate = birthdt;
                List<YearListClass1> listOfYears = new List<YearListClass1>();

                var counter = 1;
                for (int i = birthdt; i <= currentyear; i++)
                {
                    listOfYears.Add(new YearListClass1 { Year = birthdt, YearID = counter });
                    birthdt++;
                    counter++;
                }
                ObjMedicalDescModel.YearList = listOfYears;
                ViewBag.YearList = listOfYears;
                ObjMedicalDescModel.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                ObjMedicalDescModel.MedicalHistory = new Models.MedicalHistoryDetails();
                ObjMedicalDescModel.MedicalHistory.EmployeeId = Convert.ToInt32(decryptedEmployeeId);
                DAL.PersonalDetailsDAL dal = new DAL.PersonalDetailsDAL();
                IList<tbl_BloodGroup> objBloodGroup = dal.GetBloodGroupList();
                List<BloodGroupModel> objBloodGroupList = new List<BloodGroupModel>();

                foreach (tbl_BloodGroup bloodGroup in objBloodGroup)
                {
                    BloodGroupModel bg = new BloodGroupModel();
                    bg.BloodGroupId = bloodGroup.BloodGroup_Id;
                    bg.BloodGroupName = bloodGroup.BloodGroup_Name;
                    objBloodGroupList.Add(bg);
                }
                ObjMedicalDescModel.BloodGroupList = objBloodGroupList;

                int BloodGroup = dal.EmployeeBg(Convert.ToInt32(decryptedEmployeeId));
                ObjMedicalDescModel.SelectedBloodGroup = BloodGroup;
                return PartialView("_MedicalHistory", ObjMedicalDescModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        public ActionResult DeleteMedicalHistory(int MedicalDesc_ID, string employeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                DAL.PersonalDetailsDAL dal = new DAL.PersonalDetailsDAL();
                tbl_PM_MedicalDescription employeeQualifications = new tbl_PM_MedicalDescription();
                bool eq = dal.DeleteMedical_Desc(MedicalDesc_ID, Convert.ToInt32(decryptedEmployeeId));
                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// To create gridview of Employee Medical History
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        ///
        [HttpPost]
        public ActionResult MedicalHistoryLoadGrid(string employeeId, int page, int rows)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                MedicalHistoryDetailsViewModel model = new Models.MedicalHistoryDetailsViewModel();

                model.MedicalHistoryList = new List<Models.MedicalHistoryDetails>();
                int totalCount;
                model.MedicalHistoryList = dal.GetAllMedicalDesc(Convert.ToInt32(decryptedEmployeeId), page, rows, out totalCount);

                if ((model.MedicalHistoryList == null || model.MedicalHistoryList.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model.MedicalHistoryList = dal.GetAllMedicalDesc(Convert.ToInt32(decryptedEmployeeId), page, rows, out totalCount);
                }

                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = model.MedicalHistoryList,
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpGet]
        [PageAccess(PageName = "Certification")]
        public ActionResult CertificationDetails(string employeeId)
        {
            try
            {
                string decryptedEmployeeId = string.Empty;
                int employeeID = empdal.GetEmployeeID(Membership.GetUser().UserName);

                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedEmployeeId = Convert.ToString(employeeID);

                CertificationDetailsViewModel model = new CertificationDetailsViewModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string user = Commondal.GetMaxRoleForUser(role);
                model.UserRole = user;

                ViewBag.loggedinEmployeeID = employeeID;
                ViewBag.CertificationEmployeeID = employeeId;

                PersonalDetailsDAL personaldal = new PersonalDetailsDAL();
                model.EmpStatusMasterID = personaldal.GetEmployeeStatusMasterID(Convert.ToInt32(decryptedEmployeeId));
                model.EmployeeID = Convert.ToInt32(decryptedEmployeeId);
                model.CertificationNameList = new List<Certification>();
                model.NewCertification = new CertificationDetails();
                model.NewCertification.EmployeeID = Convert.ToInt32(decryptedEmployeeId);

                CertificationDetailsDAL dal = new CertificationDetailsDAL();
                List<HRMS_tbl_PM_Certifications> certificate = dal.LoadCertificationDDL();

                bool send = dal.CanSendMail(Convert.ToInt32(decryptedEmployeeId));
                if (send)
                    ViewBag.SendMail = "CanSend";
                else
                    ViewBag.SendMail = "CanNotSend";

                foreach (HRMS_tbl_PM_Certifications c in certificate)
                {
                    model.CertificationNameList.Add(new Certification()
                    {
                        CertificationID = c.CertificationID,
                        CertificationName = c.CertificationName
                    });
                }

                return PartialView("_CertificationDetails", model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult LoadCertificationGrid(int page, int rows, string employeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                CertificationDetailsDAL dal = new CertificationDetailsDAL();
                //CertificationDetailsViewModel model = new CertificationDetailsViewModel();
                List<CertificationDetails> model = new List<Models.CertificationDetails>();

                int totalCount;
                model = dal.GetEmployeeCertificationDetails(page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);

                if ((model == null || model.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    model = dal.GetEmployeeCertificationDetails(page, rows, Convert.ToInt32(decryptedEmployeeId), out totalCount);
                }

                var jsonData = new
                {
                    total = (int)Math.Ceiling((double)totalCount / (double)rows),
                    page = page,
                    records = totalCount,
                    rows = model
                };

                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors." });
            }
        }

        [HttpPost]
        public ActionResult SaveCertificationDetails(CertificationDetails model, int? SelectedCertificationID, int? EmployeeId)
        {
            try
            {
                CertificationDetailsDAL dal = new CertificationDetailsDAL();
                int? eppid = EmployeeId;

                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                int LoggedinEmployeeId = empdal.GetEmployeeID(Membership.GetUser().UserName);
                model.EmployeeID = LoggedinEmployeeId;

                string user = Commondal.GetMaxRoleForUser(role);

                bool IsLoggedInEmployee = false;

                if (eppid == LoggedinEmployeeId)
                    IsLoggedInEmployee = true;
                else
                    IsLoggedInEmployee = false;

                string resultMessage = string.Empty;

              //  var status = dal.SaveCertificationDetails(model, IsLoggedInEmployee, SelectedCertificationID, EmployeeId);
                  var status = false;

                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteCertificationDetails(int employeeCertificationID, string certificationEmployeeId)
        {
            try
            {
                bool isAuthorize;
                string decryptedEmployeeId = HRMSHelper.Decrypt(certificationEmployeeId, out isAuthorize);
                if (!isAuthorize)
                    return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });

                CertificationDetailsDAL dal = new CertificationDetailsDAL();
                tbl_PM_EmployeeCertificationMatrix employeeQualifications = new tbl_PM_EmployeeCertificationMatrix();
                bool eq = dal.DeleteCertificationDetails(employeeCertificationID, Convert.ToInt32(decryptedEmployeeId));
                return Json(new { status = eq }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// method to get file from filepath
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public ActionResult GetFileFromPath(string filePath)
        {
            if (filePath == null)
            {
                filePath = Server.MapPath(Url.Content("/Images/New Design/browse-person-img.png"));
            }
            if (Request.IsAjaxRequest())
            {
                byte[] fileData = System.IO.File.ReadAllBytes(Path.GetFullPath(filePath));

                string imageBase64Data = System.Convert.ToBase64String(fileData);

                return Json(imageBase64Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string filename = Path.GetFileName(filePath);

                string[] FileExtention = filename.Split('.');
                string contentType = "application/" + FileExtention[1];
                filePath = Path.GetFullPath(filePath);
                return File(filePath, contentType);
            }
        }

        //to create drop down for skills
        private void LoadSkillsDropDown(SkillDetails model)
        {
            PersonalDetailsDAL dal = new PersonalDetailsDAL();
            model.SkillsDDL = new List<SkillDetailsList>();
            List<HRMS_tbl_PM_Tools> skillDetailsList = dal.GetSkillDetails();

            if (skillDetailsList != null)
            {
                foreach (HRMS_tbl_PM_Tools eachSkillDetail in skillDetailsList)
                {
                    model.SkillsDDL.Add(new SkillDetailsList()
                    {
                        Value = eachSkillDetail.ToolID.ToString(),
                        Text = eachSkillDetail.Description
                    });
                }
            }
        }

        [HttpPost]
        public ActionResult LoadSkillManagementDetails(string skillInformation, int page, int rows)
        {
            try
            {
                //skillInformation data: -- "Java 1 13853"
                string employeeId = ""; string resPoolId = "";
                if (skillInformation != "undefined" && skillInformation.Length < 6)
                {
                    employeeId = skillInformation;
                }

                if (skillInformation != "undefined" && skillInformation.Length > 7)
                {
                    string[] Temp = skillInformation.Split(' ');
                    employeeId = Temp[2];
                    resPoolId = Temp[1].Trim();
                }
                else
                {
                    //employeeId = "0";
                    resPoolId = "0";
                }
                if (employeeId == "undefined" || employeeId == "") { employeeId = "0"; }

                // employeeId = "13853";
                int EmployeeId = Convert.ToInt32(employeeId);
                int ResPoolId = 0;
                PersonalDetailsDAL objPDetail = new PersonalDetailsDAL();
                int totalCount;
                List<SkillDetailsViewModel> subskill_details = new List<SkillDetailsViewModel>();
                subskill_details = objPDetail.GetSubSkillDetails(EmployeeId, ResPoolId, page, rows, out totalCount);
                if ((subskill_details == null || subskill_details.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    subskill_details = objPDetail.GetSubSkillDetails(EmployeeId, ResPoolId, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = subskill_details,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult SaveSubSkillRatings(SkillDetailsViewModel model, string UniqueID, string Rating)
        {
            string resultMessage = string.Empty;
            bool status = false;
            try
            {
                string ratingg = model.Rating;
                if (Rating == null)//|| ToolId == null)
                {
                    resultMessage = "Error";
                    status = false;
                }
                else if (ratingg == "undefined")
                {
                    resultMessage = "Error";
                    status = false;
                }
                else
                {
                    //EmployeeDAL employeeDAL = new EmployeeDAL();
                    //WSEMDBEntities wsem = new WSEMDBEntities();
                    PersonalDetailsDAL dal = new PersonalDetailsDAL();
                    status = dal.SubmitSkillManagementDetails(int.Parse(UniqueID), Rating);
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

        [HttpPost]
        public ActionResult SaveSkillManagementDetails(SkillDetailsViewModel model, string UniqueID, string Rating)
        {
            string resultMessage = string.Empty;
            bool status = false;
            try
            {
                string ratingg = model.Rating;
                if (Rating == null)
                {
                    resultMessage = "Error";
                    status = false;
                }
                else if (ratingg == "undefined")
                {
                    resultMessage = "Error";
                    status = false;
                }
                else
                {
                    //EmployeeDAL employeeDAL = new EmployeeDAL();
                    //WSEMDBEntities wsem = new WSEMDBEntities();
                    PersonalDetailsDAL dal = new PersonalDetailsDAL();

                    status = dal.SubmitSkillManagementDetails(int.Parse(UniqueID), Rating);
                    if (status)
                        resultMessage = "Saved";
                    else
                        resultMessage = "Error";
                }
                return Json(new { status = resultMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = "Error", status = status }, JsonRequestBehavior.AllowGet);
            }
        }

        //--------------------------------------Nikita---------------------------------//
        [HttpGet]
        public ActionResult DevelopmentPlan(string employeeId)
        {
            SkillDetailsViewModel model = new SkillDetailsViewModel();
            //string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            //string user = Commondal.GetMaxRoleForUser(role);
            //model.UserRole = user;
            SemDAL semDAL = new SemDAL();
            int SEMEmployeeId = semDAL.geteEmployeeIDFromSEMDatabase(Membership.GetUser().UserName);
            ViewBag.loggedinEmployeeID = SEMEmployeeId;
            ViewBag.IsRating = dal.GetRatingsDetails();
            //model.NewSkillDetail = new SkillDetails();
            //model.EmployeeSkillDetails = new List<SkillDetails>();
            //model.NewSkillDetail.EmployeeID = Convert.ToInt32(SEMEmployeeId);
            //model.NewSkillDetail.UserRole = user;
            model.EmployeeId = SEMEmployeeId.ToString();

            return PartialView("_DevelopmentPlanMain", model);
        }

        public ActionResult LoadDevelopmentPlanSkillDetails(string skillInformation, int? id, int page, int rows)
        {
            try
            {
                //skillInformation data: -- "Java 1 13853"
                //string[] Temp = skillInformation.Split(' ');
                //string employeeId = Temp[2];
                //string resPoolId = Temp[1].Trim();

                if (skillInformation == "undefined") { skillInformation = "0"; }
                // employeeId = "13853";
                int EmployeeId = Convert.ToInt32(skillInformation);
                int ResPoolId = 0;

                PersonalDetailsDAL objPDetail = new PersonalDetailsDAL();
                int totalCount;
                List<SkillDetailsViewModel> subskill_details = new List<SkillDetailsViewModel>();
                subskill_details = objPDetail.GetDevelopmentPlanSkills(EmployeeId, ResPoolId, id, page, rows, out totalCount);
                if ((subskill_details == null || subskill_details.Count <= 0) && page - 1 > 0)
                {
                    page = page - 1;
                    subskill_details = objPDetail.GetDevelopmentPlanSkills(EmployeeId, ResPoolId, id, page, rows, out totalCount);
                }
                var totalRecords = totalCount;
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = subskill_details,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpGet]
        public ActionResult DevelopmentPlanSkill(string EmployeeId, string EmployeeName, string EmployeeCode, string PrmSkillId)
        {
            PersonalDetailsDAL dal = new PersonalDetailsDAL();
            string[] Temp = EmployeeName.Split(' ');

            Temp[0] = Temp[0].Replace(" ", " ");
            string EmployeeName1 = Temp[0];

            var s = dal.Get_EmployeeName(EmployeeId);

            SkillDetailsViewModel model = new SkillDetailsViewModel();
            model.SearchedUserDetails = new SearchedUserDetails();

            ViewBag.IsRating = dal.GetRatingsDetails();
            ViewBag.EmpCode = EmployeeCode;
            ViewBag.EmpName = s;
            model.EmployeeId = EmployeeId;
            model.EmployeeName = s;

            if (PrmSkillId != null && PrmSkillId != "undefined")
                ViewBag.PrmResourcePoolId = 0;

            return PartialView("_DevelopmentPlan", model);
        }

        public ActionResult LoadDevelopmentPlan(int id, int ResourcePoolId)
        {
            PersonalDetailsDAL dal = new PersonalDetailsDAL();
            List<SkillDetailsViewModel> list = new List<SkillDetailsViewModel>();
            List<SkillDetailsViewModel> resultToSend = new List<SkillDetailsViewModel>();
            list = dal.getSkillMatrixDevelopmentPlan(id, ResourcePoolId);
            int PrmId = Convert.ToInt32(Session["SelectedPrimaryList"]);
            if (PrmId != null)
            {
                for (int i = 0; i < PrmId; i++)
                {
                    foreach (var s in list)
                    {
                        if (s.ID == PrmId)
                        {
                            resultToSend.Add(s);
                        }
                    }
                }
            }
            int total = list.Count();
            var jsonData = new
            {
                total,
                page = 1,
                rows = resultToSend
            };
            return Json(jsonData);
        }

        [HttpPost]
        public ActionResult DevelopmentPlan_LoadPop(List<string> Id, string PrmSkillId, int EmployeeId, string EmployeeName, string Employeecode)
        {
            bool status = false;
            string respoId = Id[0].Replace("abc", "").Trim();
            int resId = Convert.ToInt32(respoId);
            List<SkillDetailsViewModel> list = new List<SkillDetailsViewModel>();
            list = dal.getSkillMatrixDevelopmentPlan(EmployeeId, resId);
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

        [HttpPost]
        public ActionResult ActiveSkillDevelopmentPlan(List<string> Id)
        {
            try
            {
                PersonalDetailsDAL dal = new PersonalDetailsDAL();
                bool status = dal.ActiveSkillDevelopmentPlan(Id);
                string resid = Id[0].Replace("abc", "").Trim();

                Session["SelectedPrimaryList"] = resid;
                //Session["SelectedSecondaryList"] = SecId;
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveRatingsSkills(string ID1, string ExpectedRating, string TargetDate, int? loggedInEmployeeId)
        {
            string resultMessage = string.Empty;
            bool status = false;
            SemDAL Dal = new SemDAL();
            loggedInEmployeeId = Dal.geteEmployeeIDFromSEMDatabase(Membership.GetUser().UserName);
            try
            {
                //string ratingg = ExpectedRating;
                if (ExpectedRating.Trim().Length != 0 && ID1.Trim().Length != 0)

                // {
                //    resultMessage = "Error";
                //    status = false;
                //}
                //else if (ratingg == "undefined")
                //{
                //    resultMessage = "Errorr";
                //    status = false;
                //}

                // else
                {
                    EmployeeDAL employeeDAL = new EmployeeDAL();
                    WSEMDBEntities wsem = new WSEMDBEntities();
                    tbl_PM_Employee_SEM loggedEmployeeDetails = wsem.tbl_PM_Employee_SEM.Where(x => x.EmployeeID == loggedInEmployeeId).FirstOrDefault();
                    //HRMS_tbl_PM_Employee employeeDetails = new HRMS_tbl_PM_Employee();
                    //employeeDetails = dal.GetEmployeeDetailsFromEmpCode(Convert.ToInt32(EmployeeCode));
                    status = dal.SaveRatingDetails(ID1, ExpectedRating, Convert.ToDateTime(TargetDate), loggedEmployeeDetails.EmployeeID, loggedEmployeeDetails.EmployeeName);

                    if (status)
                    {
                        resultMessage = "Saved";
                        //  count=1;
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
                return Json(new { results = "Error", status = status }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FinalSubmitDevelopmentPlan(int? Id, string ExpectedRating, string Targetdate)
        {
            string resultMessage = string.Empty;
            bool status = false;
            try
            {
                string ratingg = ExpectedRating;
                if (ExpectedRating == null)
                {
                    resultMessage = "Error";
                    status = false;
                }
                else if (ratingg == "undefined")
                {
                    resultMessage = "Error";
                    status = false;
                }
                else
                {
                    status = dal.SubmitSkillDevelopmentPlan(Id, ExpectedRating, Convert.ToDateTime(Targetdate));

                    if (status)
                        resultMessage = "Saved";
                    else
                        resultMessage = "Error";
                }
                // bool status = false;
                return Json(new { status = resultMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ActionResult SearchEmployeeAutoSuggestForSEM(string term)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();
            List<EmployeeDetails> searchResult = new List<EmployeeDetails>();
            searchResult = employeeDAL.SearchEmployeeForSEM(term, 1, 20);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        //-------------------------End-----------------------//
        public ActionResult SearchSkillAutoSuggestForSkillMgmt(string term)
        {
            PersonalDetailsDAL objPDetailDAL = new PersonalDetailsDAL();
            List<SkillDetailsViewModel> searchResult = new List<SkillDetailsViewModel>();
            searchResult = objPDetailDAL.SearchSkillForSkillMgmt(term, 1, 20);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeDetails(string EmpId)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();
            var model = new SkillDetailsViewModel();
            HRMS_tbl_PM_Employee employee = employeeDAL.GetEmployeeDetailsByCode(EmpId);
            HRMS_tbl_PM_Employee objReportingToName = employeeDAL.GetEmployeeReportingToName_Emp(Convert.ToInt32(employee.EmployeeID));
            HRMS_tbl_PM_Employee ObjCompetencyManagerName = employeeDAL.GetCompetencyManagerName_Emp(Convert.ToInt32(employee.EmployeeID));
            HRMS_tbl_PM_Employee ObjExitConfirmationManagerName = employeeDAL.GetExitConfirmationManagerName_Emp(Convert.ToInt32(employee.EmployeeID));

            if (objReportingToName != null)
            {
                model.ReportingTo = objReportingToName.EmployeeName;
            }
            if (ObjExitConfirmationManagerName != null)
            {
                model.ConfirmationManager = ObjExitConfirmationManagerName.EmployeeName;
            }

            if (ObjCompetencyManagerName != null)
            {
                model.CompetencyManagerName_Emp = ObjCompetencyManagerName.EmployeeName;
            }
            if (employee.DesignationID != null || employee.DesignationID != 0)
            {
                tbl_PM_DesignationMaster designation = employeeDAL.GetDesignation(employee.DesignationID);
                model.Designation = designation.DesignationName;
            }
            else
                model.Designation = "Designation is not set";

            model.EmployeeName = employee.EmployeeName;
            PersonalDetailsDAL dpersonal = new PersonalDetailsDAL();
            var Res = dpersonal.Get_ResourcePoolName(employee.EmployeeID);
            model.ResourcePoolNames = Res;
            return Json(new { status = model }, "text/html", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveSubSkillRatingsPersonal(SkillDetailsViewModel model, string Rating, int? ToolId, int searchedEmployeeId)
        {
            string resultMessage = string.Empty;
            bool status = false;
            try
            {
                string ratingg = model.Rating;
                if (Rating == null || ToolId == null)
                {
                    resultMessage = "Error";
                    status = false;
                }
                else if (ratingg == "undefined")
                {
                    resultMessage = "Error";
                    status = false;
                }
                else
                {
                    //EmployeeDAL employeeDAL = new EmployeeDAL();
                    //WSEMDBEntities wsem = new WSEMDBEntities();
                    PersonalDetailsDAL dal = new PersonalDetailsDAL();

                    status = dal.SaveSubSkillRatings(searchedEmployeeId, ToolId, Rating);
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

        /// <summary>
        /// Reset Password Functionality
        /// </summary>
        /// <param name="employeeId">employeeId</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult ResetPassword(string employeeId)
        {
            EmployeeDAL employeeDAL = new EmployeeDAL();
            PersonalDetailsDAL dal = new PersonalDetailsDAL();
            PersonalDetailsViewModel model = new PersonalDetailsViewModel();
            string strPassword = "";
            string decryptedEmployeeId = string.Empty;
            try
            {
                model.SearchedUserDetails = new SearchedUserDetails();
                int employeeID = employeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                if (employeeId != null)
                {
                    bool isAuthorize;
                    decryptedEmployeeId = HRMSHelper.Decrypt(employeeId, out isAuthorize);
                    if (!isAuthorize)
                        return RedirectToAction("Index", "Error", new { errorCode = "You are not authorised to do this action." });
                }
                else
                    decryptedEmployeeId = Convert.ToString(employeeID);

                HRMS_tbl_PM_Employee employeeDetails = employeeDAL.GetEmployeeDetails(Convert.ToInt32(decryptedEmployeeId));
                MembershipUser m = Membership.GetUser(employeeDetails.EmployeeCode);
                if (m.IsLockedOut == true)
                {
                    m.UnlockUser();
                }
                strPassword = m.ResetPassword();
                SemDAL semDal = new SemDAL();
                HRMS_tbl_PM_Employee details = semDal.GetEmployeeDetailsByEmployeeCode(Membership.GetUser().UserName);
                HRMS_tbl_PM_Employee loggedindetails = semDal.GetEmployeeDetailsByEmployeeCode(employeeDetails.EmployeeCode);
                bool status;
                status = dal.UpdatePassword(employeeDetails.EmployeeCode, strPassword);

                string LoggedInEmploeeName = details.EmployeeName;
                int[] EmailToEmployeeIds = new int[] { loggedindetails.EmployeeID };
                int[] EmailCcEmployeeIds = new int[] { };
                List<TemplateHandling> th = new List<TemplateHandling>();
                List<TemplateHandling> th1 = new List<TemplateHandling>();
                th.Add(new TemplateHandling("##password##", strPassword));
                th.Add(new TemplateHandling("##loggedinuser##", LoggedInEmploeeName));

                int templateId = 93;
                string emptyvalue = "";
                string emptyvalue1 = "";
                var data = new TaskTimesheetController().TimeSheetSendMail(details.EmployeeID, EmailToEmployeeIds, EmailCcEmployeeIds, th, th1, emptyvalue, emptyvalue1, templateId);

                var data2 = "";

                return RedirectToAction("PersonalDetails", "PersonalDetails", new { employeeId = employeeId });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}