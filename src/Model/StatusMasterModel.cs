using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{
    [Serializable]
    public class StatusMasterModel
    {
     #region Private Variable 
        private int statusID;
        private string statusName;
        private int isActive;
     #endregion 
  
        #region Public Property
        public int StatusId
        {
            get { return statusID; }
            set {  statusID=value ; }
        }

        public string StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }
        public int IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        #endregion 
    }
}
