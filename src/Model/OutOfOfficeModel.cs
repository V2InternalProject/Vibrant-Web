using System;
using System.Collections.Generic;
using System.Text;


namespace V2.Orbit.Model
{
    [Serializable]
   public class OutOfOfficeModel
    {
        #region Declare Variable
        private int userid;
        private int shiftID;
        private DateTime outTime;
        private DateTime inTime;
        private int type;
        private string comments;
        private int approverId;
        private string approverComments;
        private int statusId;
        private Guid workflowId;
        private int outOfOfficeID;
        private DateTime fromDate;
        private DateTime toDate;
        private string period;
        private string month;
        private string year;

      
        #endregion


        #region Public Properties

       public int OutOfOfficeID
       {
           get { return outOfOfficeID; }
           set { outOfOfficeID = value; }

       }

       public int ShiftID
       {
           get { return shiftID; }
           set { shiftID = value; }
       }
        public int UserId
        {
            get { return userid; }
            set { userid = value; }

        }
        public DateTime OutTime
        {
            get { return outTime; }
            set { outTime = value; }
        }
        public DateTime InTime
        {
            get { return inTime; }
            set { inTime = value; }
        }
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public int ApproverId
        {
            get { return approverId; }
            set { approverId = value; }
        }
        public string ApproverComments
        {
            get { return approverComments; }
            set { approverComments = value; }
        }
        public int StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Guid WorkflowId
        {
            get { return workflowId; }
            set { workflowId = value; }
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
           get {
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
