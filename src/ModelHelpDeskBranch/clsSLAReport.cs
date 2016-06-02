using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Helpdesk.Model
{

    public class clsSLAReport
    {
        #region private variable declaration
        private int reportIssueID;
        private string description;
        private int subCategoryId;
        private string name;
        private DateTime reportIssueDate;
        private string problemSeverity;
        private string employeeName;
        private DateTime issueResolvedDate;
        private string statusDesc;
        private int issueHealth;

        private int issueAssignmentID;
        private int employeeID;
        private int categoryID;

        private int statusID;
        private string cause;
        private string fix;
        private string reportedOn;
        private string reportedBy;
        private int problemSeverityID;
        private int problemPriorityID;

        private string connectionstring;
        private int selectedStatus;
        private string selectedEmployee;
        private int loginId;
        private int category;
        private int issueMoveTo;
        private int issueMoveForm;
        private string reportIssueIDStr;


        private DateTime reportCloseDate;
        #endregion


        public clsSLAReport()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        # region public properties


        public int ReportIssueID
        {
            get { return reportIssueID; }
            set { reportIssueID = value; }
        }

        public int IssueHealth
        {
            get { return issueHealth; }
            set { issueHealth = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int SubCategoryId
        {
            get { return subCategoryId; }
            set { subCategoryId = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public DateTime ReportIssueDate
        {
            get { return reportIssueDate; }
            set { reportIssueDate = value; }
        }

        public string ProblemSeverity
        {
            get { return problemSeverity; }
            set { problemSeverity = value; }
        }

        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; }
        }

        public DateTime IssueResolvedDate
        {
            get { return issueResolvedDate; }
            set { issueResolvedDate = value; }
        }

        public string StatusDesc
        {
            get { return statusDesc; }
            set { statusDesc = value; }
        }



        public int IssueAssignmentID
        {
            get { return issueAssignmentID; }
            set { issueAssignmentID = value; }
        }

        public int EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }


        public int StatusID
        {
            get { return statusID; }
            set { statusID = value; }
        }


        public int ProblemSeverityID
        {
            get { return problemSeverityID; }
            set { problemSeverityID = value; }
        }

        public int ProblemPriorityID
        {
            get { return problemPriorityID; }
            set { problemPriorityID = value; }
        }

        public string Cause
        {
            get { return cause; }
            set { cause = value; }
        }
        public string Fix
        {
            get { return fix; }
            set { fix = value; }
        }

        public string ReportedOn
        {
            get { return reportedOn; }
            set { reportedOn = value; }
        }

        public string ReportedBy
        {
            get { return reportedBy; }
            set { reportedBy = value; }
        }



        public string Connectionstring
        {
            get { return connectionstring; }
            set { connectionstring = value; }
        }
        public int SelectedStatus
        {
            get { return selectedStatus; }
            set { selectedStatus = value; }
        }
        public string SelectedEmployee
        {
            get { return selectedEmployee; }
            set { selectedEmployee = value; }
        }
        public int LoginId
        {
            get { return loginId; }
            set { loginId = value; }
        }
        public int Category
        {
            get { return category; }
            set { category = value; }
        }
        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }
        public int IssueMoveFrom
        {
            get { return issueMoveForm; }
            set { issueMoveForm = value; }
        }
        public int IssueMoveTo
        {
            get { return issueMoveTo; }
            set { issueMoveTo = value; }
        }

        public string ReportIssueIDStr
        {
            get { return reportIssueIDStr; }
            set { reportIssueIDStr = value; }
        }


        public DateTime ReportCloseDate
        {
            get { return reportCloseDate; }
            set { reportCloseDate = value; }
        }

        #endregion
    }
}
