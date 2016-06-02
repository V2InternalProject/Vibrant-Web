using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using V2.Orbit.DataLayer;
using V2.Orbit.Model;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Orbit.BusinessLayer
{
    [Serializable]
    public class RosterPlanningBOL
    {
        RosterPlanningDAL objRosterPlanningDAL = new RosterPlanningDAL();
        DataSet dsRosterPlanningBOL = new DataSet();

        #region GetShift
        public DataSet GetShift(RosterPlanningModel objRosterPlanningModel)
        {


            try
            {
                return objRosterPlanningDAL.GetShift(objRosterPlanningModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningBOL.cs", "sp_FillShiftData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region GetEmployeeShiftDetail
        public DataSet GetEmployeeShiftDetail(RosterPlanningModel objRosterPlanningModel)
        {


            try
            {
                return objRosterPlanningDAL.GetEmployeeShiftDetail(objRosterPlanningModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningBOL.cs", "sp_FillShiftData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region FillEmployeeList
        public DataSet GetEmployeeList(RosterPlanningModel objRosterPlanningModel)
        {


            try
            {
                dsRosterPlanningBOL = objRosterPlanningDAL.GetEmployeeList(objRosterPlanningModel);
                return dsRosterPlanningBOL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningBOL.cs", "sp_FillEmployeeData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion


        #region InsertEmployeeShiftdetails

        public void InsertEmployeeeShiftDetails(RosterPlanningModel objRosterPlanningModel)
        {
            try
            {
                objRosterPlanningDAL.InsertEmployeeeShiftDetails(objRosterPlanningModel);

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningBOL.cs", "sp_Insert_Update_EmployeeShiftDetail", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion


        #region GetRosterData
        public DataSet GetRosterData(RosterPlanningModel objRosterPlanningModel)
        {


            try
            {
                dsRosterPlanningBOL = objRosterPlanningDAL.GetRosterData(objRosterPlanningModel);
                return dsRosterPlanningBOL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningBOL.cs", "GetRosterData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region Get Employee Roles

        public DataSet GetEmployeeRole(RosterPlanningModel objRosterPlanningModel)
        {


            try
            {
                dsRosterPlanningBOL = objRosterPlanningDAL.GetEmployeeRole(objRosterPlanningModel);
                return dsRosterPlanningBOL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningBOL.cs", "GetEmployeeRole", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion



	#region Report For RosterData       

        public DataSet GetRosterDataReport(RosterPlanningModel objRosterPlanningModel)
        {


            try
            {
                dsRosterPlanningBOL = objRosterPlanningDAL.GetRosterDataReport(objRosterPlanningModel);
                return dsRosterPlanningBOL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningBOL.cs", "GetRosterData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion



    }
}
