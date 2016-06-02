using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{
    [Serializable]
    public class SignInSignOutModel
    {
        //private variables
        private DateTime date1;
        private int shiftID;
        private int signInSignOutID;
        private DateTime signInTime;
        private DateTime signOutTime;
        private string totalHoursWorked;
        private int isSignInManual;
        private int isSigOutManual;
        private string signInComment;
        private string signOutComment;
        private DateTime reportingTime;
        private int statusID;
        private int employeeID;
        private Guid workflowID;
        private DateTime todate;
        private DateTime fromDate;
        private string employeeName;
        private string approverComments;
        private int approverID;
        private string period;
        private string month;
        private string year;
        private bool isBulk;
        private int type;
        private int mode;
        private string columnName;
        private string sortOrder;

        #region Public variables

        public DateTime Date1
        {
            get { return date1; }
            set { date1 = value; }
        }

        public int ShiftID
        {
            get { return shiftID; }
            set { shiftID = value; }
        }
        public int SignInSignOutID
        {
            get { return signInSignOutID; }
            set { signInSignOutID = value; }
        }

        public DateTime SignInTime
        {
            get { return signInTime; }
            set { signInTime = value; }
        }

        public DateTime SignOutTime
        {
            get { return signOutTime; }
            set { signOutTime = value; }
        }

        public string TotalHoursWorked
        {
            get { return totalHoursWorked; }
            set { totalHoursWorked = value; }
        }

        public int IsSignInManual
        {
            get { return isSignInManual; }
            set { isSignInManual = value; }
        }

        public int IsSignOutManual
        {
            get { return isSigOutManual; }
            set { isSigOutManual = value; }
        }

        public string SignInComment
        {
            get { return signInComment; }
            set { signInComment = value; }
        }

        public string SignOutComment
        {
            get { return signOutComment; }
            set { signOutComment = value; }
        }

        public DateTime ReportingTime
        {
            get { return reportingTime; }
            set { reportingTime = value; }
        }

        public int StatusID
        {
            get { return statusID; }
            set { statusID = value; }
        }

        public int EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        public Guid WorkflowID
        {
            get { return workflowID; }
            set { workflowID = value; }
        }
        public DateTime Todate
        {
            get { return todate; }
            set { todate = value; }
        }

        public DateTime FromDate
        {
            get { return fromDate; }
            set { fromDate = value; }
        }
        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; }
        }
        public string ApproverComments
        {
            get { return approverComments; }
            set { approverComments = value; }
        }

        public int ApproverID
        {
            get { return approverID; }
            set { approverID = value; }
        }
        // Reports
        public string Period
        {
            get
            {
                return period;
            }
            set { period = value; }
        }
        public string Month
        {
            get { return month; }
            set { month = value; }

        }
        public string Year
        {
            get { return year; }
            set { year = value; }

        }
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        public int Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public bool IsBulk
        {
            get { return isBulk; }
            set { isBulk = value; }
        }

        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        public string SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }


        #endregion


    }
}
