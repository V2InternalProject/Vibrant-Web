using System;
using System.Collections.Generic;
using System.Text;
using V2.Orbit.Model;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Orbit.DataLayer
{
    [Serializable]
    public class HolidayMasterDAL:DBBaseClass
    {
        DataSet dsHolidays = new DataSet();

        public DataSet bindData()
        {
            {

                try
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetHolidaysList");

                }
                catch (V2Exceptions ex)
                {
                    throw;
                }
                catch (System.Exception ex)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayMasterDAL .cs", "bindData", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
            }
        }

        #region AddNewDepartmentDetails
        public int AddNewDepartmentDetails(HolidayMasterModel objHolidayMasterModel)
        {

            SqlParameter[] paramAdd = new SqlParameter[4];

            paramAdd[0] = new SqlParameter("@HolidaysDate", SqlDbType.DateTime);
            paramAdd[0].Value = objHolidayMasterModel.HolidayDate;
            paramAdd[1] = new SqlParameter("@HolidaysDescription", SqlDbType.VarChar, 100);
            paramAdd[1].Value = objHolidayMasterModel.HolidayDescription;
            paramAdd[2] = new SqlParameter("@IsHolidayForShift", SqlDbType.Bit);
            paramAdd[2].Value = objHolidayMasterModel.isHolidayForShift;
            paramAdd[3] = new SqlParameter("@OfficeLocation", SqlDbType.TinyInt);
            paramAdd[3].Value = objHolidayMasterModel.OfficeLocation;

            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "AddNewHolidays", paramAdd);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "holidaysList.cs", "AddNewDepartmentDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }
        #endregion

        #region UpdateDepartmentDetails
        public int UpdateDepartmentDetails(HolidayMasterModel objHolidayMasterModel)
        {
            SqlParameter[] paramAdd = new SqlParameter[5];
            paramAdd[0] = new SqlParameter("@HolidaysId", SqlDbType.Int);
            paramAdd[0].Value = objHolidayMasterModel.HolidayID;
            paramAdd[1] = new SqlParameter("@HolidaysDate", SqlDbType.DateTime);
            paramAdd[1].Value = objHolidayMasterModel.HolidayDate;
            paramAdd[2] = new SqlParameter("@HolidaysDescription", SqlDbType.VarChar, 100);
            paramAdd[2].Value = objHolidayMasterModel.HolidayDescription;
            paramAdd[3] = new SqlParameter("@IsHolidayForShift", SqlDbType.Bit);
            paramAdd[3].Value = objHolidayMasterModel.isHolidayForShift;
            paramAdd[4] = new SqlParameter("@OfficeLocation", SqlDbType.TinyInt);
            paramAdd[4].Value = objHolidayMasterModel.OfficeLocation;
            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateHolidaysList", paramAdd);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "Holidayslist.cs", "UpdateDepartmentDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion



        public DataSet SearchHolidayList(HolidayMasterModel objHolidayMasterModel)
        {
            SqlParameter[] paramAdd = new SqlParameter[2];

            paramAdd[0] = new SqlParameter("@Year", SqlDbType.Int);
            paramAdd[0].Value = objHolidayMasterModel.Year;
            paramAdd[1] = new SqlParameter("@UserID", SqlDbType.Int);
            paramAdd[1].Value = objHolidayMasterModel.UserID;
            try
            {

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchHolidaysList", paramAdd);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayMasterDAL .cs", "SearchHolidaysList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        public DataSet bindHolidaysForLeaveApprovals(HolidayMasterModel objHolidayMasterModel)
        {
            SqlParameter[] paramAdd = new SqlParameter[3];

            paramAdd[0] = new SqlParameter("@StartDate", SqlDbType.DateTime);
            paramAdd[0].Value = objHolidayMasterModel.StartDate;
            paramAdd[1] = new SqlParameter("@EndDate", SqlDbType.DateTime);
            paramAdd[1].Value = objHolidayMasterModel.EndDate;
            paramAdd[2] = new SqlParameter("@UserID", SqlDbType.Int);
            paramAdd[2].Value = objHolidayMasterModel.UserID;

            //paramAdd[0] = new SqlParameter("@StartDate", SqlDbType.DateTime);
            //paramAdd[0].Value ="5/30/2016";
            //paramAdd[1] = new SqlParameter("@EndDate", SqlDbType.DateTime);
            //paramAdd[1].Value = "5/30/2016";
            //paramAdd[2] = new SqlParameter("@UserID", SqlDbType.Int);
            //paramAdd[2].Value = objHolidayMasterModel.UserID;
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "sp_GetHolidaysListforLeaveApprovals", paramAdd);

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayMasterDAL .cs", "bindHolidaysForLeaveApprovals", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }

        }

        

    }
   
}
