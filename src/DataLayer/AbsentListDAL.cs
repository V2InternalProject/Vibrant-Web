using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;

namespace V2.Orbit.DataLayer
{
   [Serializable]
    public class AbsentListDAL: DBBaseClass
    {

       //WFGetAbsentLeaveList
         #region GetAbsentList
        public DataSet GetAbsentList()
        {
            DataSet GetAbsentList;
            try
            {

                return GetAbsentList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "WFGetAbsentList");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AbsentListDAL", "WFGetAbsentList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

       public DataSet GetAbsentLeaveList()
        {
            DataSet GetAbsentList;
            try
            {

                return GetAbsentList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "WFGetAbsentLeaveList");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "GetAbsentListDAL.cs", "GetAbsentLeaveList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }


        #endregion


        #region Upload Leaves
       public DataSet GetLeaveUploadEmployeeList()
        {
            DataSet dsGetLeaveUploadEmployeeList;
            try
            {

                return dsGetLeaveUploadEmployeeList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "WFLeaveUploadMonthly");
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AbsentListDAL", "GetLeaveUploadEmployeeList", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion


    }
}
