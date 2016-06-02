using HRMS.DAL;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace HRMS.Controllers
{
    public class ExitConfigurationController : Controller
    {
        public ActionResult Index()
        {
            Session["SearchEmpFullName"] = null;  // to hide emp search
            Session["SearchEmpCode"] = null;
            Session["SearchEmpID"] = null;

            ExitViewModel Exitmodel = new ExitViewModel();
            string employeeCode = Membership.GetUser().UserName;
            string[] role = Roles.GetRolesForUser(employeeCode);
            Exitmodel.SearchedUserDetails = new SearchedUserDetails();
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            Exitmodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
            PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
            EmployeeDAL DAL = new EmployeeDAL();
            Exitmodel.SearchedUserDetails.EmployeeId = DAL.GetEmployeeID(employeeCode);
            Exitmodel.SearchedUserDetails.EmployeeCode = employeeCode;
            return View(Exitmodel);
        }

        //Configure Separation Reasons
        public ActionResult ConfigureSeperationReason()
        {
            try
            {
                ExitViewModel Exitmodel = new ExitViewModel();
                string employeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(employeeCode);
                Exitmodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                Exitmodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL emmployeeDAL = new EmployeeDAL();
                Exitmodel.SearchedUserDetails.EmployeeId = emmployeeDAL.GetEmployeeID(employeeCode);
                Exitmodel.SearchedUserDetails.EmployeeCode = employeeCode;
                ConfigurationDAL dal = new ConfigurationDAL();
                List<SeperationReasons> seperationReason = dal.getSeperationReason();
                Exitmodel.CountRecord = seperationReason.Count;
                Exitmodel.seperationReason = seperationReason;
                return PartialView("_SeperationReasonDetails", Exitmodel);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ConfigureEditReason(int? reasonId)
        {
            try
            {
                ExitViewModel Exitmodel = new ExitViewModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                Exitmodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                Exitmodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                EmployeeDAL EmployeeDAL = new EmployeeDAL();
                Exitmodel.SearchedUserDetails.EmployeeId = EmployeeDAL.GetEmployeeID(Membership.GetUser().UserName);
                ConfigurationDAL dal = new ConfigurationDAL();
                if (reasonId != null)
                {
                    v_tbl_HR_Reasons reasonRecord = dal.getReason(reasonId);
                    Exitmodel.Reason = reasonRecord.Reason;

                    Exitmodel.tag = Convert.ToString(reasonRecord.TagID);
                    Exitmodel.TagID = reasonRecord.TagID;
                    Exitmodel.ReasonID = reasonRecord.ReasonID;
                }
                Exitmodel.ReasonID = reasonId.HasValue ? reasonId.Value : 0;
                return PartialView("_ReasonDetails", Exitmodel);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditReason(ExitViewModel model)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDal = new ConfigurationDAL();

                ExitViewModel returnModel = new ExitViewModel();
                returnModel = configDal.SaveEditedReason(model);
                //if record  does not exist
                if (returnModel.IsExisted == false)
                {
                    if (returnModel.IsEdited == true)
                    {
                        success = true;
                        result = "Edited";
                    }
                    else
                    {
                        success = true;
                        result = "Added";
                    }
                }
                else
                {
                    success = true;
                    result = "Exists";
                }

                return Json(new { resultReason = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteReason(List<int> collection)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    foreach (var item in collection)
                    {
                        success = configDAL.DeleteReason(item);
                        result = "Deleted";
                    }
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        //Configure Separation Checklist
        public ActionResult ConfigureSeperationChecklist()
        {
            try
            {
                ExitViewModel Exitmodel = new ExitViewModel();
                string employeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(employeeCode);
                Exitmodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                Exitmodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                Exitmodel.SearchedUserDetails.EmployeeId = employeeDAL.GetEmployeeID(employeeCode);
                Exitmodel.SearchedUserDetails.EmployeeCode = employeeCode;
                ConfigurationDAL dal = new ConfigurationDAL();
                List<SeperationForCheckList> seperationCheckList = dal.getConfigSeperationCheckList();
                Exitmodel.CountRecord = seperationCheckList.Count;
                Exitmodel.seperationCheckList = seperationCheckList;
                return PartialView("_ConfigurationSeperationChecklistDetails", Exitmodel);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ConfigureSeparationEditCheckList(int? QuestionnaireID)
        {
            try
            {
                ExitViewModel Exitmodel = new ExitViewModel();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                Exitmodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                Exitmodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                ConfigurationDAL dal = new ConfigurationDAL();
                if (QuestionnaireID != null)
                {
                    ExitViewModel SeperationCheckList = dal.getCheckList(QuestionnaireID);
                    Exitmodel.QuestionnaireID = SeperationCheckList.QuestionnaireID;
                    Exitmodel.QuestionnaireName = SeperationCheckList.QuestionnaireName;
                    Exitmodel.QuestionnaireDescription = SeperationCheckList.QuestionnaireDescription;
                    Exitmodel.TagID = SeperationCheckList.TagID;
                    Exitmodel.RevisionID = SeperationCheckList.RevisionID;
                    Exitmodel.RevisionNo = SeperationCheckList.RevisionNo;
                    Exitmodel.Reason = SeperationCheckList.Reason;
                }
                List<ReasonDetail> Q_Questionnaire_Revision = dal.getReasonList();
                Exitmodel.ReasonList = Q_Questionnaire_Revision;
                return PartialView("_CheckListDetails", Exitmodel);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditCheckList(ExitViewModel model)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDal = new ConfigurationDAL();

                success = configDal.SaveEditedCheckList(model);
                if (success == true)
                    result = "Edited";
                else
                    result = "Error";
                return Json(new { resultReason = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteCheckList(List<int> collection)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection.Count != 0)
                {
                    foreach (var item in collection)
                    {
                        success = configDAL.DeleteCheckList(item);
                        result = "Deleted";
                    }
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        // Configure Separation Process Stackholder
        public ActionResult ConfigureSeparationStackholder()
        {
            try
            {
                StackHolderVM stackHoldermodel = new StackHolderVM();
                string employeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(employeeCode);
                stackHoldermodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                stackHoldermodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                stackHoldermodel.SearchedUserDetails.EmployeeId = employeeDAL.GetEmployeeID(employeeCode);
                stackHoldermodel.SearchedUserDetails.EmployeeCode = employeeCode;
                ConfigurationDAL dal = new ConfigurationDAL();
                List<StackHolderList> stackHolder = dal.getStackHolderDetails();
                stackHoldermodel.CountRecord = stackHolder.Count;
                stackHoldermodel.stackHolder = stackHolder;
                return PartialView("_SeparationStackholder", stackHoldermodel);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteStackholder(string collection)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDAL = new ConfigurationDAL();
                if (collection != "")
                {
                    string roleIDWithcomma = collection.TrimEnd(',');
                    string[] roleidArray = roleIDWithcomma.Split(',');
                    int[] myInts = Array.ConvertAll(roleidArray, s => int.Parse(s));
                    foreach (var item in myInts)
                    {
                        success = configDAL.DeleteStackHolderRecord(item);
                        result = "Deleted";
                    }
                }
                return Json(new { resultMesssage = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult SelectStakeholder(string collection, string searchstring)
        {
            try
            {
                string roleIDWithcomma = collection.TrimEnd(',');
                string[] roleidArray = roleIDWithcomma.Split(',');
                int[] myInts = Array.ConvertAll(roleidArray, s => int.Parse(s));

                StackHolderVM stackHoldermodel = new StackHolderVM();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                stackHoldermodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                stackHoldermodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                ConfigurationDAL dal = new ConfigurationDAL();
                List<StackHolderList> stackholder = dal.getSelectedStackHolder(myInts);

                if (searchstring != "")
                {
                    List<StackHolderList> searchStakeHolder = stackholder.FindAll(x => x.Employee.ToLower().Replace(" ", "").Contains(searchstring.ToLower().Replace(" ", "")));
                    stackHoldermodel.stackHolder = searchStakeHolder;
                    stackHoldermodel.CountRecord = searchStakeHolder.Count;
                }
                else
                {
                    stackHoldermodel.stackHolder = stackholder;
                    stackHoldermodel.CountRecord = stackholder.Count;
                }

                return PartialView("_StakeHolder", stackHoldermodel);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ShowSelectedStakeholder(string collection)
        {
            try
            {
                StackHolderVM stackHoldermodel = new StackHolderVM();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                stackHoldermodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                stackHoldermodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                ConfigurationDAL dal = new ConfigurationDAL();
                if (collection != "")
                {
                    string roleIDWithcomma = collection.TrimEnd(',');
                    string[] roleidArray = roleIDWithcomma.Split(',');
                    int[] myInts = Array.ConvertAll(roleidArray, s => int.Parse(s));
                    List<StackHolderList> stackholder = dal.getSelectedStackHolder(myInts);
                    List<StackHolderList> stackholderSelected = new List<StackHolderList>();
                    foreach (var item in stackholder)
                    {
                        if (myInts.Any(emp => emp == item.EmployeeID))
                        {
                            stackholderSelected.Add(item);
                        }
                        else
                            continue;
                    }
                    stackHoldermodel.stackHolder = stackholderSelected;
                    stackHoldermodel.CountRecord = stackholderSelected.Count;
                }
                else
                {
                    List<StackHolderList> stackholder = new List<StackHolderList>();
                }
                ViewBag.IsShowButtonHide = true;
                return PartialView("_StakeHolder", stackHoldermodel);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveStackHolder(string collection)
        {
            try
            {
                bool success = false;
                string result = null;
                ConfigurationDAL configDal = new ConfigurationDAL();
                string roleIDWithcomma = collection.TrimEnd(',');
                string[] roleidArray = roleIDWithcomma.Split(',');
                int[] myInts = Array.ConvertAll(roleidArray, s => int.Parse(s));
                success = configDal.SaveStakeHolderSelected(myInts);
                if (success == true)
                    result = "Edited";
                else
                    result = "Added";
                return Json(new { resultReason = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        //Set Separation Stage Approvers
        public ActionResult SetSeparationStageApprovers()
        {
            try
            {
                SeparationChecklist Separationmodel = new SeparationChecklist();
                string employeeCode = Membership.GetUser().UserName;
                string[] role = Roles.GetRolesForUser(employeeCode);
                Separationmodel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                Separationmodel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
                PersonalDetailsDAL personalDAL = new PersonalDetailsDAL();
                EmployeeDAL employeeDAL = new EmployeeDAL();
                Separationmodel.SearchedUserDetails.EmployeeId = employeeDAL.GetEmployeeID(employeeCode);
                Separationmodel.SearchedUserDetails.EmployeeCode = employeeCode;
                ConfigurationDAL dal = new ConfigurationDAL();
                List<SeperationChecklistRecord> seperationChecklist = dal.getSeperationChecklist();
                Separationmodel.seperationChecklist = seperationChecklist;
                return PartialView("_SetSeparationStageApproversDetails", Separationmodel);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult FeedbackChecklist(int OrderNumber)
        {
            exitFeedbackChecklistVM checklistModel = new exitFeedbackChecklistVM();
            string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
            checklistModel.SearchedUserDetails = new SearchedUserDetails();
            CommonMethodsDAL Commondal = new CommonMethodsDAL();
            checklistModel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);
            ConfigurationDAL dal = new ConfigurationDAL();
            if (OrderNumber != 0)
            {
                List<FeedbackChk> feedbackChk = dal.getFeedbackChklist(OrderNumber);
                checklistModel.feedbackChk = feedbackChk;
                checklistModel.CountRecord = feedbackChk.Count;
                foreach (var item in feedbackChk)
                {
                    checklistModel.Checklist = item.Checklist;
                    checklistModel.Name = item.Name;
                    checklistModel.Role = item.Role;
                    checklistModel.HiddenChecklistID = item.Checklist;
                    checklistModel.HiddenNameID = item.EmployeeID;
                }
            }
            checklistModel.StageID = OrderNumber;
            checklistModel.checkListFor = dal.getChecklistDetails();
            ViewBag.checkListFor = dal.getChecklistDetails();
            List<CheckListNames> checkList = dal.getcheckListNames();
            checklistModel.checkListNames = checkList;

            return PartialView("_ExitFeedbackChkList", checklistModel);
        }

        [HttpPost]
        public ActionResult SaveCheckList(List<FeedbackCheckList> model)
        {
            try
            {
                bool success = false;
                bool successDelete = false;
                string result = null;
                ConfigurationDAL configDal = new ConfigurationDAL();
                if (model != null)
                {
                    List<FeedbackChk> FeedBackCheckListForStage = configDal.getFeedBackCheckListForstage(model.FirstOrDefault().stageID);
                    List<FeedbackChk> FinalDeleteList = new List<FeedbackChk>();
                    foreach (var item in FeedBackCheckListForStage)
                    {
                        if (model.Any(m => m.EmployeeId == item.EmployeeID))
                            continue;
                        else
                            FinalDeleteList.Add(item);
                    }
                    if (FinalDeleteList.Count != 0)
                        successDelete = configDal.deleteApprover(FinalDeleteList);

                    foreach (var item in model)
                    {
                        success = configDal.Save_HR_ExitProcess_StageApprovers(item);
                        if (success)
                            result = "Added";
                        else
                        {
                            result = "Error";
                            return Json(new { resultReason = result, status = success }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { resultReason = result, status = success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult LoadExitFeedbackCheckListGrid(int OrderNumber, int page, int rows)
        {
            try
            {
                ConfigurationDAL dal = new ConfigurationDAL();

                exitFeedbackChecklistVM checklistModel = new exitFeedbackChecklistVM();
                string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                checklistModel.SearchedUserDetails = new SearchedUserDetails();
                CommonMethodsDAL Commondal = new CommonMethodsDAL();
                checklistModel.SearchedUserDetails.UserRole = Commondal.GetMaxRoleForUser(role);

                if (OrderNumber != 0)
                {
                    List<FeedbackChk> feedbackChk = dal.getFeedbackChklistData(OrderNumber);
                    checklistModel.feedbackChk = feedbackChk;
                    checklistModel.CountRecord = feedbackChk.Count;
                    foreach (var item in feedbackChk)
                    {
                        checklistModel.Checklist = item.Checklist;
                        checklistModel.ChecklistName = item.ChecklistName;
                        checklistModel.Name = item.Name;
                        checklistModel.Role = item.Role;
                    }
                }
                checklistModel.StageID = OrderNumber;
                checklistModel.checkListFor = dal.getChecklistDetails();
                ViewBag.checkListFor = dal.getChecklistDetails();
                List<CheckListNames> checkList = dal.getcheckListNames();
                checklistModel.checkListNames = checkList;

                var jsonData = new
                {
                    rows = checklistModel.feedbackChk,
                };
                return Json(jsonData);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { errorCode = "There are some errors" });
            }
        }

        [HttpPost]
        public ActionResult GetRoleForSelectedEmployeeName(int EmployeeID)
        {
            try
            {
                ConfigurationDAL dal = new ConfigurationDAL();
                string data = dal.GetRoleDetails(EmployeeID);

                return Json(new { Role = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveExitFeedbackCheckListDetails(exitFeedbackChecklistVM model, int? CheckListID, int? NameID, string LoggedUserName)
        {
            try
            {
                ConfigurationDAL dal = new ConfigurationDAL();
                SEMResponse response = new SEMResponse();

                response = dal.SaveExitFeedbackCheckListDetails(model, CheckListID, NameID, LoggedUserName);

                return Json(new { status = response.status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}