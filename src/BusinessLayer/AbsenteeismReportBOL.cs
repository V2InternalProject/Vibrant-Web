using System;
using System.Collections.Generic;
using System.Text;
using V2.CommonServices.FileLogger;
using V2.CommonServices.Exceptions;
using System.Data.SqlClient;
using System.Data;
using V2.Orbit.Model;
using V2.Orbit.DataLayer;

namespace V2.Orbit.BusinessLayer
{
    public class AbsenteeismReportBOL
    {
        AbsenteeismReportDAL objAbsenteeismReportDAL = new AbsenteeismReportDAL();

        #region AbsenteeismReport
        public DataSet AbsenteeismReport(AbsenteeismReportModel objAbsenteeismReportModel, bool IsAdmin, bool AllTeammembers)
        {
            try
            {
                return objAbsenteeismReportDAL.AbsenteeismReport(objAbsenteeismReportModel, IsAdmin, AllTeammembers);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AbsentismReportBOL", "AbsentismReport", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        } 
        #endregion

        #region GetEmployeeNameRpt
        public DataSet GetEmployeeNameRpt(AbsenteeismReportModel objAbsenteeismReportModel)
        {
            try
            {
                return objAbsenteeismReportDAL.GetEmployeeNameRpt(objAbsenteeismReportModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "AbsenteeismReportBOL", "GetEmployeeNameRpt", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion
    }
    
     
}
