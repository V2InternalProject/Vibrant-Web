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
    public class StatusMasterDAL : DBBaseClass
    {
        StatusMasterModel objStatusMasterModel = new StatusMasterModel();
                
        #region BindData
        public DataSet BindData()
        {
            try
            {
                DataSet dsGetSBU = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetStatus");
                return dsGetSBU;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "StatusMaster.cs", "bindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        } 
        #endregion

        #region Add Satus
        public int AddStatus(StatusMasterModel objStatusMasterModel)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@StatusName", SqlDbType.VarChar);
            param[0].Value = objStatusMasterModel.StatusName;
            param[1] = new SqlParameter("@IsActive", SqlDbType.Bit);
            param[1].Value = objStatusMasterModel.IsActive;

            try
            {
                int rowsffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "AddStatusMaster", param);
               return rowsffected;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SBUMasterDAL.cs", "AddSBU", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }


        } 
        #endregion

        #region UpdateStatus
        public int UpdateStatus(StatusMasterModel objStatusMasterModel)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@StatusName", SqlDbType.VarChar);
            param[0].Value = objStatusMasterModel.StatusName;
            param[1] = new SqlParameter("@IsActive",SqlDbType.Int);
            param[1].Value = objStatusMasterModel.IsActive;
            param[2] = new SqlParameter("@StatusId", SqlDbType.BigInt);
            param[2].Value = objStatusMasterModel.StatusId;


            try
            {
                int rowsffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateStatus", param);
               return rowsffected;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SBUMasterDAL.cs", "UpdateStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        
        #endregion

        #region DeleteSBU
        public void DeleteSBU(StatusMasterModel objStatusMasterModel)
        {
            //SqlParameter[] param = new SqlParameter[1];
            //param[0] = new SqlParameter("@SBUId", SqlDbType.BigInt);
            //param[0].Value = objStatusMasterModel.StatusId;


            //try
            //{
            //    SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeleteSBUMaster", param);

            //}
            //catch (V2Exceptions ex)
            //{
            //    throw;
            //}
            //catch (System.Exception ex)
            //{
            //    FileLog objFileLog = FileLog.GetLogger();
            //    objFileLog.WriteLine(LogType.Error, ex.Message, "SBUMasterDAL.cs", "DeleteSBU", ex.StackTrace);
            //    throw new V2Exceptions(ex.ToString(),ex);
            //}

        } 
        #endregion
        
        
    }
   
}


