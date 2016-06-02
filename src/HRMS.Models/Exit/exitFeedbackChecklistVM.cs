using System.Collections.Generic;

namespace HRMS.Models
{
    public class exitFeedbackChecklistVM
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<FeedbackChk> feedbackChk { get; set; }
        public int CountRecord { get; set; }
        public List<CheckList> checkListFor { get; set; }
        public int StageID { get; set; }
        public List<CheckListNames> checkListNames { get; set; }

        public string Name { get; set; }
        public string Role { get; set; }
        public int Checklist { get; set; }
        public string ChecklistName { get; set; }
        public int CheckListNameID { get; set; }
        public int QuestionnaireID { get; set; }

        public int ExitFeedbackCheckListID { get; set; }
        public int? HiddenChecklistID { get; set; }
        public int? HiddenNameID { get; set; }
    }

    public class FeedbackChk
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int RoleID { get; set; }
        public bool isChecked { get; set; }
        public int Checklist { get; set; }
        public string ChecklistName { get; set; }
        public int RevisionID { get; set; }
        public int stageApproverID { get; set; }
        public int StageID { get; set; }
    }

    public class FeedbackCheckList
    {
        public int CheckListValue { get; set; }
        public int EmployeeId { get; set; }
        public int RoleID { get; set; }
        public int stageID { get; set; }
        public int RevisionID { get; set; }
        public int StageApproverID { get; set; }
    }

    public class CheckList
    {
        public int QuestionnaireID { get; set; }
        public string QuestionnaireName { get; set; }
        public int RevisionID { get; set; }

        //public int ReasonID { get; set; }
        //public string Reason { get; set; }
        //public string Role { get; set; }
        //public string Name { get; set; }
        //public string DeptStage { get; set; }
        //public bool isChecked { get; set; }
    }

    public class CheckListNames
    {
        public int CheckListNameID { get; set; }
        public string CheckListName { get; set; }

        public string Role { get; set; }

        public string BusinessGroup { get; set; }

        public string OrganizationUnit { get; set; }

        public string Employee { get; set; }

        public int EmployeeID { get; set; }
    }
}