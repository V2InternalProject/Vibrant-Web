using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{
    public class OrbitMasterModel
    {
        #region Veriable Declaration
        private int userID;
        private string userName;
        #endregion

        #region Public properties

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; } 
        }
        #endregion

    }
}
