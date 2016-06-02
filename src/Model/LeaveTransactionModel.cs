using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{
    [Serializable]
    public class LeaveTransactionModel
    {
        #region Veriable Declaration

        private int leaveDetailsID;
        private int userID;
        private int shiftID;
        private string description;
        private decimal quantity;
        private DateTime transactionDate;
        private int leaveTransactionID;
        private int compensationID;
        private Boolean leaveType;
        private DateTime fromDate;
        private DateTime toDate;
        private string period;
        private string month;
        private string year;
        private int leaveTypeID;
        private Boolean transactionMode;
        private string employeeName;
        #endregion

        #region Public properties

        public int LeaveDetailsID
        {
            get { return leaveDetailsID; }
            set { leaveDetailsID = value; }
        }

        public int ShiftID
        {
            get { return shiftID; }
            set { shiftID = value; }
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public DateTime TransactionDate
        {
            get { return transactionDate; }
            set { transactionDate = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public int LeaveTransactionID
        {
            get { return leaveTransactionID; }
            set { leaveTransactionID = value; }
        }

        public int CompensationID
        {
            get { return compensationID; }
            set { compensationID = value; }
        }

        public Boolean LeaveType
        {
            get { return leaveType; }
            set { leaveType = value; }
        }
        public DateTime FromDate
        {
            get { return fromDate; }
            set { fromDate = value; }
        }
        public DateTime ToDate
        {
            get { return toDate; }
            set { toDate = value; }
        }
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

        public int LeaveTypeID
        {
            get { return leaveTypeID; }
            set { leaveTypeID = value; }

        }
        public Boolean TransactionMode
        {
            get { return transactionMode; }
            set { transactionMode = value; }
        }
        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; }
        }
      


        #endregion
    }
}
