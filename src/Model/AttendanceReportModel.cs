using System;
using System.Collections.Generic;
using System.Text;

namespace V2.Orbit.Model
{
   public class AttendanceReportModel
   {
       #region Variable Declaration  
       private int userId;
       private int shiftId;
       private string month;
       private string year;
       #endregion 

       #region Public Property
       public int UserId
       {
           get { return userId; }
           set { userId = value; }
       }
       public int ShiftID
       {
           get { return shiftId; }
           set { shiftId = value; }
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
