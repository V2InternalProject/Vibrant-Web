using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using V2.Orbit.Model;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;
using System.Configuration;


namespace V2.Orbit.DataLayer
{
    [Serializable]
    public class RosterPlanningDAL : DBBaseClass
    {
        DataSet dsRosterPlanningDAL = new DataSet();

        #region GetRosterData
        public DataSet GetRosterData(RosterPlanningModel objRosterPlanningModel)
        {


            try
            {
                SqlParameter[] objParam = new SqlParameter[5];
                objParam[0] = new SqlParameter("@UserID", objRosterPlanningModel.UserId);
                objParam[1] = new SqlParameter("@ShiftID", objRosterPlanningModel.ShiftID);
                objParam[2] = new SqlParameter("@StartDate", objRosterPlanningModel.FromDate);
                objParam[3] = new SqlParameter("@EndDate", objRosterPlanningModel.ToDate);
                objParam[4] = new SqlParameter("@LoggedUserId", objRosterPlanningModel.LoggedUserId);

                dsRosterPlanningDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetRosterData", objParam);
                return dsRosterPlanningDAL;

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningDAL.cs", "GetRosterData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region GetShift
        public DataSet GetShift(RosterPlanningModel objRosterPlanningModel)
        {


            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                //objParam[0] = new SqlParameter("@EmployeeID", DbType.Int32);

                objParam[0] = new SqlParameter("@UserID", DbType.Int32);
                objParam[0].Value = objRosterPlanningModel.UserId;

                dsRosterPlanningDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "sp_FillShiftData", objParam);
                return dsRosterPlanningDAL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningDAL.cs", "sp_FillShiftData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region GetEmployeeShiftDetail
        public DataSet GetEmployeeShiftDetail(RosterPlanningModel objRosterPlanningModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                //objParam[0] = new SqlParameter("@EmployeeID", DbType.Int32);

                objParam[0] = new SqlParameter("@UserID", DbType.Int32);
                objParam[0].Value = objRosterPlanningModel.UserId;


                objParam[1] = new SqlParameter("@FromDate", DbType.DateTime);
                objParam[1].Value = objRosterPlanningModel.FromDate;


                objParam[2] = new SqlParameter("@ToDate", DbType.DateTime);
                objParam[2].Value = objRosterPlanningModel.ToDate;

                dsRosterPlanningDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetEmployeeShiftDetail", objParam);
                return dsRosterPlanningDAL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningDAL.cs", "GetEmployeeShiftDetail", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region FillEmployeeList
        public DataSet GetEmployeeList(RosterPlanningModel objRosterPlanningModel)
        {


            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@ShiftID", DbType.Int32);
                objParam[0].Value = objRosterPlanningModel.ShiftID;

                objParam[1] = new SqlParameter("@UserID", DbType.Int32);
                objParam[1].Value = objRosterPlanningModel.UserId;

                //objParam[0] = new SqlParameter("@ShiftID", objRosterPlanningModel.ShiftID);
                //objParam[1] = new SqlParameter("@UserID", objRosterPlanningModel.UserId);


                dsRosterPlanningDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "sp_FillEmployeeData", objParam);
                return dsRosterPlanningDAL;

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningDAL.cs", "sp_FillEmployeeData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region InsertEmployeeShiftdetails

        public void InsertEmployeeeShiftDetails(RosterPlanningModel objRosterPlanningModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[6];
                objParam[0] = new SqlParameter("@UserID", objRosterPlanningModel.UserId);
                objParam[1] = new SqlParameter("@ShiftId", objRosterPlanningModel.ShiftID);
                objParam[2] = new SqlParameter("@WeekOff1", objRosterPlanningModel.WeekOffDate1);
                objParam[3] = new SqlParameter("@WeekOff2", objRosterPlanningModel.WeekOffDate2);
                objParam[4] = new SqlParameter("@StartDate", objRosterPlanningModel.FromDate);
                objParam[5] = new SqlParameter("@EndDate", objRosterPlanningModel.ToDate);

                SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "sp_Insert_Update_EmployeeShiftDetail", objParam);


            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningDAL.cs", "sp_Insert_Update_EmployeeShiftDetail", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }

        #endregion

        #region Get Employee Role

        public DataSet GetEmployeeRole(RosterPlanningModel objRosterPlanningModel)
        {
             string V2toolsDBEntity = "";
             V2toolsDBEntity = ConfigurationManager.AppSettings["V2toolsDBEntity"].ToString();

            try
            {
                SqlParameter[] objParam = new SqlParameter[2];

                objParam[0] = new SqlParameter("@ApplicationName", DbType.String);
                objParam[0].Value = "V2ToolsApp";

                objParam[1] = new SqlParameter("@UserName", DbType.String);
                objParam[1].Value = objRosterPlanningModel.UserId;


                dsRosterPlanningDAL = SqlHelper.ExecuteDataset(V2toolsDBEntity, CommandType.StoredProcedure, "aspnet_UsersInRoles_GetRolesForUser", objParam);
                return dsRosterPlanningDAL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningDAL.cs", "GetEmployeeRole", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }


        #endregion

        #region Report For RosterData
        public DataSet GetRosterDataReport(RosterPlanningModel objRosterPlanningModel)
        {


            try
            {
                SqlParameter[] objParam = new SqlParameter[5];
                objParam[0] = new SqlParameter("@UserID", objRosterPlanningModel.UserId);
                objParam[1] = new SqlParameter("@ShiftID", objRosterPlanningModel.ShiftID);
                objParam[2] = new SqlParameter("@StartDate", objRosterPlanningModel.FromDate);
                objParam[3] = new SqlParameter("@EndDate", objRosterPlanningModel.ToDate);
                objParam[4] = new SqlParameter("@LoggedUserId", objRosterPlanningModel.LoggedUserId);

                dsRosterPlanningDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetRosterDataReport", objParam);
                return dsRosterPlanningDAL;

            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "RosterPlanningDAL.cs", "GetRosterDataReport", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }


        #endregion





    }
}
