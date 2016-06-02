using System;
using System.Collections.Generic;
using System.Text;
using V2.CommonServices.FileLogger;
using V2.CommonServices.Exceptions;
using V2.Orbit.DataLayer;
using V2.Orbit.Model;
using System.Data;

namespace V2.Orbit.BusinessLayer
{
    [Serializable]
   public class StatusMasterBOL
    {
       StatusMasterDAL objStatusMasterDAL = new StatusMasterDAL();
        #region getSBU
       public DataSet BindData()
        {
            try
            {
                DataSet dsStatus = objStatusMasterDAL.BindData();
                return dsStatus;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SBUMasterBOL", "BindData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }
        #endregion

        #region AddSBU
       public int AddStatus(StatusMasterModel objStatusMasterModel)
        {
            try
            {
                return objStatusMasterDAL.AddStatus(objStatusMasterModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SBUMasterBOL", "AddStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }
        #endregion

        #region UpdateStatus
       public int UpdateStatus(StatusMasterModel objStatusMasterModel)
        {
            try
            {
                return objStatusMasterDAL.UpdateStatus(objStatusMasterModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SBUMasterBOL", "UpdateStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }
        #endregion

        #region DeleteSBU
        //public void DeleteSBU(SBUMasterModel objSBUMasterModel)
        //{
        //    try
        //    {
        //        objSBUMasterDAL.DeleteSBU(objSBUMasterModel);
        //    }
        //    catch (V2Exceptions ex)
        //    {
        //        throw;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "SBUMasterBOL", "DeleteSBU", ex.StackTrace);
        //        throw new V2Exceptions(ex.ToString(),ex);
        //    }
        //}
       #endregion
       
    }
}
