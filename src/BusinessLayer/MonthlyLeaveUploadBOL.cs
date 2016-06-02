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

   public class MonthlyLeaveUploadBOL
    {
        MonthlyLeaveUploadDAL objMonthlyLeaveUploadDAL = new MonthlyLeaveUploadDAL();
       public DataSet BindData(MonthlyLeaveUploadModel objMonthlyLeaveUploadModel)
        {
            return objMonthlyLeaveUploadDAL.BindData(objMonthlyLeaveUploadModel);

        }
        #region AddNewMonthlyLeaveDetails
       public int AddNewMonthlyLeaveDetails(MonthlyLeaveUploadModel objMonthlyLeaveUploadModel)
        {

            try
            {
                return objMonthlyLeaveUploadDAL.AddNewMonthlyLeaveDetails(objMonthlyLeaveUploadModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUploadBOL.cs", "AddNewDepartmentDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region UpdateNewMonthlyLeaveDetails
       public int UpdateNewMonthlyLeaveDetails(MonthlyLeaveUploadModel objMonthlyLeaveUploadModel)
        {
            try
            {
                return objMonthlyLeaveUploadDAL.UpdateNewMonthlyLeaveDetails(objMonthlyLeaveUploadModel);

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "MonthlyLeaveUploadBOL.cs", "UpdateDepartmentDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion
    }
}
