using System;
using System.Collections.Generic;
using System.Text;
using V2.Orbit.Model;
using V2.Orbit.DataLayer;
using System.Data;
using V2.CommonServices.FileLogger;
using V2.CommonServices.Exceptions;

namespace V2.Orbit.BusinessLayer
{
    [Serializable]
    public class ConfigItemBOL
    {
        DataSet DsConfigItem = new DataSet();
        ConfigItemDAL objConfigItemDAL = new ConfigItemDAL();

        public DataSet bindData()
        {
            try
            {
                DsConfigItem = objConfigItemDAL.bindData();
                return DsConfigItem;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "HolidayMasterBOL.cs", "bindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
            
        }  

     
        #region    AddNewConfigItems
        //public int AddNewConfigItems(ConfigItemModel objConfigItemModel)
        //{

        //    try
        //    {
        //        return objConfigItemDAL.AddNewConfigItems(objConfigItemModel);
        //    }
        //    catch (V2Exceptions ex)
        //    {
        //        throw;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItemBOL.cs", "AddNewConfigItems", ex.StackTrace);
        //        throw new V2Exceptions(ex.ToString(),ex);
        //    }
        //}
        #endregion

        #region UpdateConfigItems
        public int UpdateConfigItems(ConfigItemModel objConfigItemModel)
        {
            try
            {
                return objConfigItemDAL.UpdateConfigItems(objConfigItemModel);

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItemBOL.cs", "UpdateConfigItems", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        public int DeleteWorkflowInstanceManually(Guid WorkflowId)
        {
            try
            {
                return objConfigItemDAL.DeleteWorkflowInstanceManually(WorkflowId);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItemBOL.cs", "DeleteWorkflowInstanceManually", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }

        public DataSet GetEmployeeListForAdminApproval()
        {
            try
            {
                DataSet dsEmployeeList = objConfigItemDAL.GetEmployeeListForAdminApproval();
                return dsEmployeeList;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ConfigItemBOL.cs", "GetEmployeeListForAdminApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }

    }
}
