using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{  
    /// <summary>
    /// Summary description for DepartmentMaster.
    /// </summary>
    [Serializable]
    public class MonthlyLeaveUploadModel
    {
        #region Declare Private Variable 
        private int monthlyLeaveUploadId;
        private string leaveMonth;
        private int leaveYear;
        private double leaveDays;
        #endregion 

        #region Public Property
        public int MonthlyLeaveUploadId
        {
            get { return monthlyLeaveUploadId; }
            set { monthlyLeaveUploadId = value; }
        }
        public string LeaveMonth
        {
            get { return leaveMonth; }
            set { leaveMonth = value; }
        }
        public int LeaveYear
        {
            get { return leaveYear; }
            set { leaveYear = value; }
        }
        public double LeaveDays
        {
            get { return leaveDays; }
            set { leaveDays = value; }
        } 

        #endregion
    }
}
