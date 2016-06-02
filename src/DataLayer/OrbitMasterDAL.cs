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
    public class OrbitMasterDAL : DBBaseClass
    {
        #region GetEmployeeNames
        //public DataSet WFGetLeaveDetails(int leaveDetailsId)
        //{
        //    try
        //    {
        //        DataSet GetLeaveDetails;
        //        SqlParameter[] objParam = new SqlParameter[1];
        //        objParam[0] = new SqlParameter("@LeaveDetailID", leaveDetailsId);

        //        GetLeaveDetails = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "WFGetLeaveDetails", objParam);
        //        return GetLeaveDetails;
        //    }
        //    catch (V2Exceptions ex)
        //    {
        //        throw;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "LeaveDeatilsDAL.cs", "WFGetLeaveDetails", ex.StackTrace);
        //        throw new V2Exceptions(ex.ToString(),ex);
        //    }

        //}

        #endregion
    }
}
