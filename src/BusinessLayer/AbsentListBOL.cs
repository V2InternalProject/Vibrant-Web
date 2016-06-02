using System;
using System.Collections.Generic;
using System.Text;
using V2.Orbit.DataLayer;
using V2.Orbit.Model;
using System.Data;
using System.Data.SqlClient;
using V2.CommonServices.FileLogger;
using V2.CommonServices.Exceptions;

namespace V2.Orbit.BusinessLayer
{
    [Serializable]
    public class AbsentListBOL
    {
        AbsentListDAL objAbsentListDAL = new AbsentListDAL();
        //GetAbsentLeaveList()
        #region GetAbsentList
        public DataSet GetAbsentList()
        {
            try
            {
                return objAbsentListDAL.GetAbsentList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AbsentListBOL.cs", "GetAbsentList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public DataSet GetAbsentLeaveList()
        {
            try
            {
                return objAbsentListDAL.GetAbsentLeaveList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AbsentListBOL.cs", "GetAbsentLeaveList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion
        
        
        #region Upload Leaves
       public DataSet GetLeaveUploadEmployeeList()
        {
            
            try
            {

                return objAbsentListDAL.GetLeaveUploadEmployeeList();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AbsentListBOL.cs", "GetLeaveUploadEmployeeList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

    }
}
