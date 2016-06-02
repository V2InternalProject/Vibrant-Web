namespace HRMS.Models
{
    public class Common
    {
    }

    public static class YesNoCondition
    {
        public static string Yes = "Yes";
        public static string No = "No";
    }

    public static class UserRoles
    {
        public static string HRAdmin = "HR Admin";
        public static string HRExecutive = "HR Executive";
        public static string Finance = "Finance";
        public static string RMG = "RMG";
        public static string Admin = "Admin";
        public static string PMS_DT = "PMS_DT";
        public static string PMS_DU = "PMS_DU";
        public static string Manager = "Manager";
        public static string Developers = "Developers";
        public static string TravelAdmin = "Travel_Admin";
        public static string TravelApprover = "Travel Approver";
        public static string ExpenseAdmin = "Expense_Admin";
        public static string ExpenseApprover = "Expense_Approver";
        public static string DeliveryManager = "Delivery Manager";
        public static string AccountOwner = "Account Owners";
        public static string GroupHead = "Group Head";
        public static string Management = "Management";
        public static string ProjectApprover = "Project Approver";
    }

    public static class ApprovalStatusMessages
    {
        public static string NoAction_0 = "Field change pending HR approval";
        public static string OnHold_1 = "Field change approval is On Hold";
        public static string Approved_2 = "Field change is Approved";
        public static string Rejected_3 = "Field change approval is Rejected";

        public static string NoAction_4 = "Field change pending approval";
    }

    public static class GridHRApprovalStatusMessages
    {
        public static string GNoAction_Edit0 = "Changes Pending HR Approval";
        public static string GNoAction_Add0 = "Addition Pending HR Approval";
        public static string GOnHold_1 = "Changes are On Hold";
        public static string GApproved_2 = "Changes are Approved";
        public static string GRejected_3 = "Changes are Rejected";
    }

    public static class GridRMGApprovalStatusMessages
    {
        public static string GNoAction_Edit_RMG0 = "Changes Pending RMG Approval";
        public static string GNoAction_Add_RMG0 = "Addition Pending RMG Approval";
        public static string GOnHold_RMG_1 = "Changes are On Hold";
        public static string GApproved_RMG_2 = "Changes are Approved";
        public static string GRejected_RMG_3 = "Changes are Rejected";
    }

    public enum EmployeeRolesOrderSem
    {
        Admin = 1,
        RMG = 1,
        PMS_DU = 2,
        PMS_DT = 3,
        Manager = 4
    }

    public enum EmployeeRolesOrder
    {
        HRAdmin = 1,
        RMG = 2,
        HRExecutive = 3,
        Manager = 4,
        Developers = 5
        //Admin = 1,
        //RMG = 1,
        //PMS_DU = 2,
        //PMS_DT = 3,
        //Manager = 4
    }

    public static class MinDate
    {
        public static string MinValue = "1/1/1900 12:00:00 AM";
    }

    public static class SessionFilter
    {
        public static string OnActionExecuting = "OnActionExecuting";
        public static string OnActionExecuted = "OnActionExecuted";
        public static string EncryptedLoggedinEmployeeId = "encryptedLoggedinEmployeeID";
    }

    public static class RevisionQuestions
    {
        public static string QuestionOne = "From what value to what value ?";

        public static string QuestionTwo = "What are the root causes for this revision ?";

        public static string QuestionThree = "How are you dealing with these root causes ?";

        public static string QuestionFour = "How can we avoid these problems/revisions in future ?";

        public static string QuestionFive = "What is your degree of confidence that further revisions to this project will not be required ?";
    }

    public static class ApprovalStatus
    {
        public static string RevisionPendingApproval = "Revisions Pending Approval";

        public static string RevisionApproved = "Revisions Approved";

        public static string RevisionRejected = "Revisions Rejected";
    }

    public static class InvoiceStages
    {
        public static string DraftStage = "Draft";
        public static string ApproverStage = "Approver";
        public static string FinanceApproverStage = "Finance Approver";
        public static string ApprovedStage = "Approved";
        public static string CancelledStage = "Cancelled";
    }

    public static class TagLevelName
    {
        public static string GlobalTag = "Global";
        public static string ProjectTag = "Project";
    }

    public static class ModuleNames
    {
        public static string ExpenseModule = "ExpenseModule";
    }
}