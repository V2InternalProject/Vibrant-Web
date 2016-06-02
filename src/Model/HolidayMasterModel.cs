using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{	
    /// <summary>
    /// Summary description for DepartmentMaster.
    /// </summary>
    [Serializable]
    public class HolidayMasterModel
    {
        #region private variable declaration

        private int userID;
        private int holidayID;
        private DateTime holidayDate;
        private string holidayDescription;
        private int year;
        private bool IsHolidayForShift;
        //Added by Rahul Ramachandran
        private int officeLocation;

        private DateTime startdate;
        private DateTime enddate;
        #endregion 

        #region public properpties

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public int HolidayID
        {
            get { return holidayID; }
            set { holidayID = value; }
        }
        public DateTime HolidayDate
        {
            get { return holidayDate; }
            set { holidayDate = value; }
        }
        public string HolidayDescription
        {
            get { return holidayDescription; }
            set { holidayDescription = value; }
        }
        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public bool isHolidayForShift
        {
            get { return IsHolidayForShift; }
            set { IsHolidayForShift = value; }
        }

        //Added by Rahul Ramachandran
        public int OfficeLocation
        {
            get { return officeLocation; }
            set { officeLocation = value; }
        }

        public DateTime StartDate
        {
            get { return startdate; }
            set { startdate = value; }
        }
        public DateTime EndDate
        {
            get { return enddate; }
            set { enddate = value; }
        }
        #endregion
    }
}
