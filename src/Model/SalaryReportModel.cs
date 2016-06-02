using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{
    public class SalaryReportModel
    {
        #region Variable Declaration
      
        private string month;
        private string year;
        #endregion

        #region Public Property
      
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
