using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{
    [Serializable]
    public class CompensationDetailsModel
    {
        #region Veriable Declaration

        private int compensationID;
        private int shiftId;
        private int userID;
        private DateTime appliedFor;        
        private float totalLeaveDays;        
        private string reason;
        private int statusID;
        private int approverID;
        private string approverComments;
        private DateTime requestedOn;
        private Guid workFlowID;
        private DateTime compensationFrom;
        private DateTime compensationTo;
        private string period;
        private string month;
        private string year;

        #endregion

        #region Public properties

        public int ShiftId
        {
            get { return shiftId; }
            set { shiftId = value; }
        }

        public int CompensationID
        {
            get { return compensationID; }
            set { compensationID = value; }
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public DateTime AppliedFor
        {
            get { return appliedFor; }
            set { appliedFor = value; }
        }       

        public float TotalLeaveDays
        {
            get { return totalLeaveDays; }
            set { totalLeaveDays = value; }
        }
               
        public string Resason
        {
            get { return reason; }
            set { reason = value; }
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

        public DateTime CompensationFrom
        {
            get { return compensationFrom; }
            set { compensationFrom = value; }
        }

        public DateTime CompensationTo
        {
            get { return compensationTo; }
            set { compensationTo = value; }
        }
        public string Period
        {
            get { return period; }
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
