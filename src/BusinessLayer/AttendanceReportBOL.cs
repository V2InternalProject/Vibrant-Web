using System;
using System.Collections.Generic;
using System.Text;
using V2.Orbit.DataLayer;
using V2.Orbit.Model;
using System.Data.SqlClient;
using System.Data;
namespace V2.Orbit.BusinessLayer
{
    public class AttendanceReportBOL
    {
        AttendanceReportDAL objAttendanceReportDAL = new AttendanceReportDAL();
        public System.Data.DataSet BindEmployee(AttendanceReportModel objAttendanceReportModel)
        {
            return objAttendanceReportDAL.BindEmployee(objAttendanceReportModel);
        }

        public DataSet GetAttendanceReport(AttendanceReportModel objAttendanceReportModel,Boolean IsAdmin,Boolean AllTeammembers)
        {
            return objAttendanceReportDAL.GetAttendanceReport(objAttendanceReportModel, IsAdmin, AllTeammembers);
        }
    }
}
