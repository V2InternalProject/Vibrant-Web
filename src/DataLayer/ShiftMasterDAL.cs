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
    public class ShiftMasterDAL:DBBaseClass
    {

        #region GetShiftDetails

        public DataSet bindData()
        {
            {

                try
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetShiftDetails");

                }
                catch (V2Exceptions ex)
                {
                    throw;
                }
                catch (System.Exception ex)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "ShiftMasterDAL.cs .cs", "GetShiftDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
            }
        }

        #endregion


        #region AddShiftMaster
        public int AddShiftMaster(ShiftMasterModel objShiftMasterModel)
        {

            SqlParameter[] paramAdd = new SqlParameter[4];

            paramAdd[0] = new SqlParameter("@ShiftName", SqlDbType.VarChar,100);
            paramAdd[0].Value = objShiftMasterModel.shiftName;
            //paramAdd[1] = new SqlParameter("@Description", SqlDbType.VarChar,100);
            //paramAdd[1].Value = objShiftMasterModel.description;
            paramAdd[1] = new SqlParameter("@ShiftInTime", SqlDbType.DateTime);
            paramAdd[1].Value = objShiftMasterModel.shiftInTime;
            paramAdd[2] = new SqlParameter("@ShiftOutTime", SqlDbType.DateTime);
            paramAdd[2].Value = objShiftMasterModel.shiftOutTime;
            paramAdd[3] = new SqlParameter("@ISActive", SqlDbType.Bit);
            paramAdd[3].Value = objShiftMasterModel.isActive;
            
            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "AddShiftMaster", paramAdd);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ShiftMasterDAL.cs", "AddShiftMaster", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }
        #endregion


        #region UpdateShiftMaster
        public int UpdateShiftMaster(ShiftMasterModel objShiftMasterModel)
        {

            SqlParameter[] paramAdd = new SqlParameter[5];

            paramAdd[0] = new SqlParameter("@ShiftName", SqlDbType.VarChar, 100);
            paramAdd[0].Value = objShiftMasterModel.shiftName;
            //paramAdd[1] = new SqlParameter("@Description", SqlDbType.VarChar, 100);
            //paramAdd[1].Value = objShiftMasterModel.description;
            paramAdd[1] = new SqlParameter("@ShiftInTime", SqlDbType.DateTime);
            paramAdd[1].Value = objShiftMasterModel.shiftInTime;
            paramAdd[2] = new SqlParameter("@ShiftOutTime", SqlDbType.DateTime);
            paramAdd[2].Value = objShiftMasterModel.shiftOutTime;
            paramAdd[3] = new SqlParameter("@ISActive", SqlDbType.Bit);
            paramAdd[3].Value = objShiftMasterModel.isActive;
            paramAdd[4] = new SqlParameter("@ShiftID", SqlDbType.Int);
            paramAdd[4].Value = objShiftMasterModel.shiftID;

            try
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateShiftMaster", paramAdd);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ShiftMasterDAL.cs", "UpdateShiftMaster", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }
        #endregion


    }
}
