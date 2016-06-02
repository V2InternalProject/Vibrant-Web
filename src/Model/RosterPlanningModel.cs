using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{
    [Serializable]
    public class RosterPlanningModel
    {
        #region Declare Variable
        private int shiftid;
        private int userid;
        private DateTime fromdate;
        private DateTime todate;
        private DateTime weekoff1;
        private DateTime weekoff2;
        private int loggedUserId;
        #endregion


        #region Public Properties
        public int ShiftID
        {
            get { return shiftid; }
            set { shiftid = value; }
        }


        
        public int UserId
        {
            get { return userid; }
            set { userid = value; }
        }


        
        public DateTime FromDate
        {
            get { return fromdate; }
            set { fromdate = value; }
        }


       
        public DateTime ToDate
        {
            get { return todate; }
            set { todate = value; }
        }

        
        public DateTime WeekOffDate1
        {
            get { return weekoff1; }
            set { weekoff1 = value; }
        }

        
        public DateTime WeekOffDate2
        {
            get { return weekoff2; }
            set { weekoff2 = value; }
        }

        public int LoggedUserId
        {
            get { return loggedUserId; }
            set { loggedUserId = value; }
        }
        #endregion

    }
}
