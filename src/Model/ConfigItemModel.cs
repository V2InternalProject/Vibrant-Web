using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{
    /// <summary>
    /// Summary description for DepartmentMaster.
    /// </summary>
    [Serializable]
    public class ConfigItemModel
    {
        #region private variable declaration

        private int configItemIdID;
        private string configItemName;
        private string configItemValue;
        private string configItemDescription;
        #endregion

        #region Public Property
        public int ConfigItemIdID
        {
            get { return configItemIdID; }
            set { configItemIdID = value; }
        }
        public string ConfigItemName
        {
            get { return configItemName; }
            set { configItemName = value; }
        }
        public string ConfigItemValue
        {
            get { return configItemValue; }
            set { configItemValue = value; }
        }
        public string ConfigItemDescription
        {
            get { return configItemDescription; }
            set { configItemDescription = value; }
        }
        
        #endregion 
    }
}
