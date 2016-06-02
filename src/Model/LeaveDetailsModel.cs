using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{
    [Serializable]    
    public class LeaveDetailsModel
    {
        #region Veriable Declaration

        private int leaveDetailsID;
        private int leaveType;
        private int userID;
        private int shiftID;
        private DateTime leaveDateFrom;
        private DateTime leavedateTo;
        private float totalLeaveDays;
        private float leaveCorrectionDays;
        private string leaveReason;
        private int statusID;
        private int approverID;
        private string approverComments;
        private DateTime requestedOn;
        private Guid workFlowID;
        private string period;
        private string month;
        private string year;
        #endregion

        #region Public properties

        public int ShiftID
        {
            get { return shiftID; }
            set { shiftID = value; }
        }

        public int LeaveDetailsID
        {
            get { return leaveDetailsID; }
            set { leaveDetailsID = value; }
        }
        public int LeaveType
        {
            get { return leaveType; }
            set { leaveType = value; }
        }
        public int UserID
        {
            get { return userID;}
            set { userID = value; }      
        }

        public DateTime LeaveDateFrom
        {
            get { return leaveDateFrom; }
            set { leaveDateFrom = value; }
        }

        public DateTime LeaveDateTo
        {
            get { return leavedateTo; }
            set { leavedateTo = value; }
        }

        public float TotalLeaveDays
        {
            get { return totalLeaveDays; }
            set { totalLeaveDays = value; }
        }

        public float LeaveCorrectionDays
        {
            get { return leaveCorrectionDays; }
            set { leaveCorrectionDays = value; }
        }

        public string LeaveResason
        {
            get { return leaveReason; }
            set { leaveReason = value; }
        }

        public int StatusID
        {
            get { return statusID; }
            set { statusID = value; }
        }

        public int ApproverID
        {
            get { return approverID; }
            set { approverID = value; }
        }

        public string ApproverComments
        {
            get { return approverComments; }
            set { approverComments = value; }
        }

        public DateTime RequestedOn
        {
            get { return requestedOn; }
            set { requestedOn = value; }
        }

        public Guid WorkFlowID
        {
            get { return workFlowID; }
            set { workFlowID = value; }
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
        #endregion
    }
}
