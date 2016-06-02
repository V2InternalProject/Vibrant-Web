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
    public class ConfigItemDAL : DBBaseClass
    {
        DataSet dsConfigItem= new DataSet();

        public DataSet bindData()
        {
            {

                try
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetConfigList");

                }
                catch (V2Exceptions ex)
                {
                    throw;
                }
                catch (System.Exception ex)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItemDAL.cs", "bindData", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
            }
        }

        #region AddNewConfigItems
        //public int AddNewConfigItems(ConfigItemModel objConfigItemModel)
        //{

        //    SqlParameter[] paramAdd = new SqlParameter[2];

        //    paramAdd[0] = new SqlParameter("@ConfigItemName", SqlDbType.VarChar, 100);
        //    paramAdd[0].Value = objConfigItemModel.ConfigItemName;
        //    paramAdd[1] = new SqlParameter("@ConfigItemValue", SqlDbType.VarChar, 100);
        //    paramAdd[1].Value = objConfigItemModel.ConfigItemValue;

        //    try
        //    {
        //        return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "AddNewConfigItems", paramAdd);
        //    }
        //    catch (V2Exceptions ex)
        //    {
        //        throw;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItemDAL.cs", "AddNewConfigItems", ex.StackTrace);
        //        throw new V2Exceptions(ex.ToString(),ex);
        //    }

        //}
        #endregion

        #region UpdateConfigItems
        public int UpdateConfigItems(ConfigItemModel objConfigItemModel)
        {
            try
            {
                SqlParameter[] paramAdd = new SqlParameter[4];
                paramAdd[0] = new SqlParameter("@ConfigItemId", SqlDbType.Int);
                paramAdd[0].Value = objConfigItemModel.ConfigItemIdID;
                paramAdd[1] = new SqlParameter("@ConfigItemName", SqlDbType.VarChar, 100);
                paramAdd[1].Value = objConfigItemModel.ConfigItemName;
                paramAdd[2] = new SqlParameter("@ConfigItemValue", SqlDbType.VarChar, 100);
                paramAdd[2].Value = objConfigItemModel.ConfigItemValue;
                paramAdd[3] = new SqlParameter("@ConfigItemDescription", SqlDbType.VarChar, 100);
                paramAdd[3].Value = objConfigItemModel.ConfigItemDescription;
                
            
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateConfigItems", paramAdd);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItemDAL.cs", "UpdateConfigItems", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        public int DeleteWorkflowInstanceManually(Guid WorkflowId)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@WorkflowInstanceInternalId", SqlDbType.UniqueIdentifier);
                param[0].Value = WorkflowId;

                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeleteWorkflowInstanceManually", param);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItemDAL.cs", "DeleteWorkflowInstanceManually", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet GetEmployeeListForAdminApproval()
        {
            try
            {
                DataSet dsEmployeeList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetEmployeeListForAdminApproval");
                return dsEmployeeList;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItemDAL.cs", "GetEmployeeListForAdminApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

    }
}
