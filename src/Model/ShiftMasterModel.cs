using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{
    [Serializable]
    public class ShiftMasterModel
    {
        #region private variable declaration

        
        private int ShiftID;
        private string ShiftName;
        //private string Description;
        private DateTime ShiftInTime;
        private DateTime ShiftOutTime;
        private bool ISActive;

        #endregion

        #region public properpties


        

        public int shiftID
        {
            get { return ShiftID; }
            set { ShiftID = value; }
        }

        public string shiftName
        {
            get { return ShiftName; }
            set { ShiftName = value; }
        }

        //public string description
        //{
        //    get { return Description; }
        //    set { Description = value; }
        //}

        public DateTime shiftInTime
        {
            get { return ShiftInTime; }
            set { ShiftInTime = value; }
        }

        public DateTime shiftOutTime
        {
            get { return ShiftOutTime; }
            set { ShiftOutTime = value; }
        }

        public bool isActive
        {
            get { return ISActive; }
            set { ISActive = value; }
        }
        #endregion

    }
}
