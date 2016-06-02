using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{
    public class AbsenteeismReportModel
    {
        #region Declare Variable
        private int userid;
        private int shiftID;
        private DateTime fromDate;
        private DateTime toDate;
        private string period;
        private string month;
        private string year;        
        #endregion

        #region Public properites
        
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
        #endregion
    }
}
