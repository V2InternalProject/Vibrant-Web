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
    public class ShiftMasterBOL
    {
        DataSet DsShift = new DataSet();
        ShiftMasterDAL objShiftMasterDAL = new ShiftMasterDAL();

        public DataSet bindData()
        {
            DsShift = objShiftMasterDAL.bindData();
            return DsShift;
        }

        #region AddShiftMaster
        public int AddShiftMaster(ShiftMasterModel objShiftMasterModel)
        {

            try
            {
                return objShiftMasterDAL.AddShiftMaster(objShiftMasterModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ShiftMasterBOL.cs", "AddShiftDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region UpdateShiftMaster
        public int UpdateShiftMaster(ShiftMasterModel objShiftMasterModel)
        {
            try
            {
                return objShiftMasterDAL.UpdateShiftMaster(objShiftMasterModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ShiftMasterBOL.cs", "UpdateShiftDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }

        #endregion 
    }
}
