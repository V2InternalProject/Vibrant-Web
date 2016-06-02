using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using V2.Orbit.Model;
using V2.Orbit.DataLayer;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Orbit.BusinessLayer
{
    [Serializable]
    public class HolidayMasterBOL
    {
        DataSet DsHolidays = new DataSet();
        HolidayMasterDAL objHolidayMasterDAL = new HolidayMasterDAL();

        public DataSet bindData()
        {
            DsHolidays = objHolidayMasterDAL.bindData();
            return DsHolidays;
        }

        #region AddNewDepartmentDetails
        public int AddNewDepartmentDetails(HolidayMasterModel objHolidayMasterModel)
        {

            try
            {
                return objHolidayMasterDAL.AddNewDepartmentDetails(objHolidayMasterModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "DepartmentMasterBOL.cs", "AddNewDepartmentDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region UpdateDepartmentDetails
        public int UpdateDepartmentDetails(HolidayMasterModel objHolidayMasterModel)
        {
            try
            {
                return objHolidayMasterDAL.UpdateDepartmentDetails(objHolidayMasterModel);

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayMasterBOL.cs", "UpdateDepartmentDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion


        public DataSet searchHolidayList(HolidayMasterModel objHolidayMasterModel)
        {
            DsHolidays = objHolidayMasterDAL.SearchHolidayList(objHolidayMasterModel);
            return DsHolidays;
        }
        public DataSet bindHolidaysForLeaveApprovals(HolidayMasterModel objHolidayMasterModel)
        {
            DsHolidays = objHolidayMasterDAL.bindHolidaysForLeaveApprovals(objHolidayMasterModel);
            return DsHolidays;
        }
    }
   
}
