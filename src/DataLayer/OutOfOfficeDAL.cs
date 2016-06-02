using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using V2.Orbit.Model;
using Microsoft.ApplicationBlocks.Data;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Orbit.DataLayer
{

    [Serializable]
   public class OutOfOfficeDAL:DBBaseClass
    {
        #region AddOutOfOffice
        public SqlDataReader AddOutOfOffice(OutOfOfficeModel objOutOfOfficeModel)
        {
            //int rowsReturned = 0;
            SqlParameter[] objSqlParam = new SqlParameter[6];

            objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.BigInt);
            objSqlParam[0].Value = objOutOfOfficeModel.UserId;

            objSqlParam[1] = new SqlParameter("@InTime", SqlDbType.DateTime);
            objSqlParam[1].Value = objOutOfOfficeModel.InTime;

            objSqlParam[2] = new SqlParameter("@OutTime", SqlDbType.DateTime);
            objSqlParam[2].Value = objOutOfOfficeModel.OutTime;

            objSqlParam[3] = new SqlParameter("@TypeID", SqlDbType.Int);
            objSqlParam[3].Value = objOutOfOfficeModel.Type;

            objSqlParam[4] = new SqlParameter("@Comment", SqlDbType.VarChar);
            objSqlParam[4].Value = objOutOfOfficeModel.Comments;  

            objSqlParam[5] = new SqlParameter("@ApproverID", SqlDbType.BigInt);
            objSqlParam[5].Value = objOutOfOfficeModel.ApproverId;  
         

            try
            {
                SqlDataReader dr = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "AddOutOfOffice", objSqlParam);
                return dr;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("An entry for the selected Date and Time already exists") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "AddOutOfOffice", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else {
                    throw ex;
                }

            }

        } 
        #endregion

         public DataSet GetReportingTo(OutOfOfficeModel objOutOfOfficeModel)
         {
            DataSet GetReportingTo;
            SqlParameter[] objParam = new SqlParameter[1];
            objParam[0] = new SqlParameter("@UserID", objOutOfOfficeModel.UserId);
           
            try
            {

                return GetReportingTo = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetReportingToProc", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "GetReportingToProc", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #region Update OutOfOffice
        public int UpdateOutOffice(OutOfOfficeModel objOutOfOfficeModel)
        {
            int rowsReturned = 0;
            SqlParameter[] objSqlParam = new SqlParameter[6];


            objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.BigInt);
            objSqlParam[0].Value = objOutOfOfficeModel.UserId;

            objSqlParam[1] = new SqlParameter("@InTime", SqlDbType.DateTime);
            objSqlParam[1].Value = objOutOfOfficeModel.InTime;

            objSqlParam[2] = new SqlParameter("@OutTime", SqlDbType.DateTime);
            objSqlParam[2].Value = objOutOfOfficeModel.OutTime;

            objSqlParam[3] = new SqlParameter("@TypeID", SqlDbType.Int);
            objSqlParam[3].Value = objOutOfOfficeModel.Type;

            objSqlParam[4] = new SqlParameter("@Comment", SqlDbType.VarChar);
            objSqlParam[4].Value = objOutOfOfficeModel.Comments;

            objSqlParam[5] = new SqlParameter("@OutOfOfficeID", SqlDbType.BigInt);
            objSqlParam[5].Value = objOutOfOfficeModel.OutOfOfficeID;



            try
            {
                rowsReturned = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateOutOffice", objSqlParam);
                return rowsReturned;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("An entry for the selected Date and Time already exists") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "UpdateOutOfOffice", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else {
                    throw ex;
                }
            }

        }  
        #endregion

        #region Delete OutOFOffice
       public int DeleteOutOfOffice(OutOfOfficeModel objOutOfOfficeModel)
        {
            int rowsReturned = 0;
            SqlParameter[] objSqlParam = new SqlParameter[2];

            objSqlParam[0] = new SqlParameter("@OutOfOfficeID", SqlDbType.BigInt);
            objSqlParam[0].Value = objOutOfOfficeModel.OutOfOfficeID;
            objSqlParam[1] = new SqlParameter("@UserId", SqlDbType.Int);
            objSqlParam[1].Value = objOutOfOfficeModel.UserId;
           //objSqlParam[2] = new SqlParameter("@statusId",SqlDbType.Int);
           //objSqlParam[2].Value=objOutOfOfficeModel.StatusId ;


            try
            {
                rowsReturned = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeleteOutOfOffice", objSqlParam);
                return rowsReturned;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "UpdateOutOfOffice", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }  
        #endregion

        #region CancelOutOfOffice
        public DataSet CancelOutOfOffice(OutOfOfficeModel objOutOfOfficeModel)
        {
            try
            {
                DataSet dsCancelOutOfOffice;
                SqlParameter[] objSqlparam = new SqlParameter[2];

                objSqlparam[0] = new SqlParameter("@OutOfOfficeID", SqlDbType.Int);
                objSqlparam[0].Value = objOutOfOfficeModel.OutOfOfficeID;

                objSqlparam[1] = new SqlParameter("@UserID", SqlDbType.Int);
                objSqlparam[1].Value = objOutOfOfficeModel.UserId;

                dsCancelOutOfOffice = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "CancelOutOfOffice", objSqlparam);
                return dsCancelOutOfOffice;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "CancelOutOfOffice", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion

        #region GetReasonName
        public DataSet GetReasonName()
        {
            DataSet dsGetReasonName;
            try
            {
                dsGetReasonName = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetReasonName");
                return dsGetReasonName;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "GetReasonName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion

        #region GetOutOfOffice
        public DataSet GetOutOfOffice(OutOfOfficeModel objOutOfOfficeModel)
        {
            DataSet dsGetOutOfOffice;

            SqlParameter[] objSqlParam = new SqlParameter[1];
            objSqlParam[0] = new SqlParameter("@UserId", SqlDbType.Int);
            objSqlParam[0].Value = objOutOfOfficeModel.UserId;
            try
            {
                dsGetOutOfOffice = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetOutofOffice", objSqlParam);
                return dsGetOutOfOffice;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "GetReasonName", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        } 
        #endregion

        #region GetStatus
        public DataSet GetStatus()
        {
            DataSet dsGetStatus;
            
            try
            {
                dsGetStatus = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetStatus");
                return dsGetStatus;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "GetStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        } 
        #endregion

        #region SearchOutOfOffice
       public DataSet SearchOutOfOffice(OutOfOfficeModel objOutOfOfficeModel)
       {
         
            DataSet dsSearchOutOfOffice;
            SqlParameter[] objSqlParam = new SqlParameter[4];

            objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
            objSqlParam[0].Value = objOutOfOfficeModel.UserId;

            if (objOutOfOfficeModel.FromDate.ToString() != "1/1/0001 12:00:00 AM")
            {

                objSqlParam[1] = new SqlParameter("@Fromdate", SqlDbType.DateTime);
                objSqlParam[1].Value = objOutOfOfficeModel.FromDate;
            }
            else
            {
                objSqlParam[1] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                objSqlParam[1].Value = null;
            }

            if (objOutOfOfficeModel.ToDate.ToString() != "1/1/0001 12:00:00 AM")
            {
                objSqlParam[2] = new SqlParameter("@Todate", SqlDbType.DateTime);
                objSqlParam[2].Value = objOutOfOfficeModel.ToDate;
            }
            else
            {
                objSqlParam[2] = new SqlParameter("@Todate", SqlDbType.DateTime);
                objSqlParam[2].Value = null;
            }            

            objSqlParam[3] = new SqlParameter("@StatusId", SqlDbType.Int);
            objSqlParam[3].Value = objOutOfOfficeModel.StatusId;
           
           try
           {
               dsSearchOutOfOffice = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchOutOfOffice", objSqlParam);
               return dsSearchOutOfOffice;
           }
           catch (V2Exceptions ex)
           {
               throw;
           }
           catch (System.Exception ex)
           {
               FileLog objFileLog = FileLog.GetLogger();
               objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "SearchOutOfOffice", ex.StackTrace);
               throw new V2Exceptions(ex.ToString(),ex);
           }
       } 
        #endregion

       #region SearchOutOfOfficeDatewise
       public DataSet SearchOutOfOfficeDatewise(OutOfOfficeModel objOutOfOfficeModel)
       {
           DataSet dsSearchOutOfOfficeDatewise;
           SqlParameter[] objSqlParam = new SqlParameter[4];

           objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
           objSqlParam[0].Value = objOutOfOfficeModel.UserId;

           if (objOutOfOfficeModel.FromDate.ToString() != "1/1/0001 12:00:00 AM")
           {

               objSqlParam[1] = new SqlParameter("@Fromdate", SqlDbType.DateTime);
               objSqlParam[1].Value = objOutOfOfficeModel.FromDate;
           }
           else
           {
               objSqlParam[1] = new SqlParameter("@FromDate", SqlDbType.DateTime);
               objSqlParam[1].Value = null;
           }

           if (objOutOfOfficeModel.ToDate.ToString() != "1/1/0001 12:00:00 AM")
           {
               objSqlParam[2] = new SqlParameter("@Todate", SqlDbType.DateTime);
               objSqlParam[2].Value = objOutOfOfficeModel.ToDate;
           }
           else
           {
               objSqlParam[2] = new SqlParameter("@Todate", SqlDbType.DateTime);
               objSqlParam[2].Value = null;
           }

           objSqlParam[3] = new SqlParameter("@StatusId", SqlDbType.Int);
           objSqlParam[3].Value = objOutOfOfficeModel.StatusId;

           try
           {
               dsSearchOutOfOfficeDatewise = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchOutOfOfficeDatewise", objSqlParam);
               return dsSearchOutOfOfficeDatewise;
           }
           catch (V2Exceptions ex)
           {
               throw;
           }
           catch (System.Exception ex)
           {
               FileLog objFileLog = FileLog.GetLogger();
               objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "SearchOutOfOffice", ex.StackTrace);
               throw new V2Exceptions(ex.ToString(),ex);
           }

       } 
       #endregion

       // Out Of Office Approval
       #region GetOutOfOfficeApproval
       public DataSet GetOutOfOfficeApproval(OutOfOfficeModel objOutOfOfficeModel)
       {
           DataSet dsGetOutOfOfficeApproval;
           SqlParameter[] objSqlParam = new SqlParameter[2];

           objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
           objSqlParam[0].Value = objOutOfOfficeModel.UserId;

           objSqlParam[1] = new SqlParameter("@StatusID", SqlDbType.Int);
           objSqlParam[1].Value = objOutOfOfficeModel.StatusId;


           try
           {
               dsGetOutOfOfficeApproval = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetOutOfOfficeApproval", objSqlParam);
               return dsGetOutOfOfficeApproval;
           }
           catch (V2Exceptions ex)
           {
               throw;
           }
           catch (System.Exception ex)
           {
               FileLog objFileLog = FileLog.GetLogger();
               objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "SearchOutOfOffice", ex.StackTrace);
               throw new V2Exceptions(ex.ToString(),ex);
           }

       } 
       #endregion

       #region UpdateOutOfficeApproval
       public int UpdateOutOfficeApproval(OutOfOfficeModel objOutOfOfficeModel)
       {
           int rowsReturned = 0;
           SqlParameter[] objSqlParam = new SqlParameter[7];


           objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.BigInt);
           objSqlParam[0].Value = objOutOfOfficeModel.UserId;

           objSqlParam[1] = new SqlParameter("@InTime", SqlDbType.DateTime);
           objSqlParam[1].Value = objOutOfOfficeModel.InTime;

           objSqlParam[2] = new SqlParameter("@OutTime", SqlDbType.DateTime);
           objSqlParam[2].Value = objOutOfOfficeModel.OutTime;

           //objSqlParam[3] = new SqlParameter("@TypeID", SqlDbType.Int);
           //objSqlParam[3].Value = objOutOfOfficeModel.Type;

           //objSqlParam[4] = new SqlParameter("@Comment", SqlDbType.VarChar);
           //objSqlParam[4].Value = objOutOfOfficeModel.Comments;

           objSqlParam[3] = new SqlParameter("@OutOfOfficeID", SqlDbType.BigInt);
           objSqlParam[3].Value = objOutOfOfficeModel.OutOfOfficeID;

           objSqlParam[4] = new SqlParameter("@ApproverID", SqlDbType.BigInt);
           objSqlParam[4].Value = objOutOfOfficeModel.ApproverId;

           objSqlParam[5] = new SqlParameter("@ApproverComments", SqlDbType.VarChar);
           objSqlParam[5].Value = objOutOfOfficeModel.ApproverComments;

           objSqlParam[6] = new SqlParameter("@StatusID", SqlDbType.Int);
           objSqlParam[6].Value = objOutOfOfficeModel.StatusId;


           try
           {
               rowsReturned = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateOutOfficeApproval", objSqlParam);
               return rowsReturned;
           }
           catch (V2Exceptions ex)
           {
               throw;
           }
           catch (System.Exception ex)
           {
               if (ex.Message.CompareTo("An entry for the selected Date and Time already exists") != 0)
               {
                   FileLog objFileLog = FileLog.GetLogger();
                   objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "UpdateOutOfficeApproval", ex.StackTrace);
                   throw new V2Exceptions(ex.ToString(),ex);
               }
               else {
                   throw ex;
               }
           }

       } 
       #endregion

       #region SearchOutOfOfficeApproval
       public DataSet SearchOutOfOfficeApproval(OutOfOfficeModel objOutOfOfficeModel)
       {
           DataSet dsSearchOutOfOfficeApproval;
           SqlParameter[] objSqlParam = new SqlParameter[4];

           objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
           objSqlParam[0].Value = objOutOfOfficeModel.UserId;


           if (objOutOfOfficeModel.FromDate.ToString() != "1/1/0001 12:00:00 AM")
           {

               objSqlParam[1] = new SqlParameter("@Fromdate", SqlDbType.DateTime);
               objSqlParam[1].Value = objOutOfOfficeModel.FromDate;
           }
           else
           {
               objSqlParam[1] = new SqlParameter("@FromDate", SqlDbType.DateTime);
               objSqlParam[1].Value = null;
           }
           if (objOutOfOfficeModel.ToDate.ToString() != "1/1/0001 12:00:00 AM")
           {
               objSqlParam[2] = new SqlParameter("@Todate", SqlDbType.DateTime);
               objSqlParam[2].Value = objOutOfOfficeModel.ToDate;
           }
           else
           {
               objSqlParam[2] = new SqlParameter("@Todate", SqlDbType.DateTime);
               objSqlParam[2].Value = null;
           }
           objSqlParam[3] = new SqlParameter("@StatusId", SqlDbType.Int);
           objSqlParam[3].Value = objOutOfOfficeModel.StatusId;

           try
           {
               dsSearchOutOfOfficeApproval = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchOutOfOfficeApproval", objSqlParam);
               return dsSearchOutOfOfficeApproval;
           }
           catch (V2Exceptions ex)
           {
               throw;
           }
           catch (System.Exception ex)
           {
               FileLog objFileLog = FileLog.GetLogger();
               objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "SearchOutOfOfficeApproval", ex.StackTrace);
               throw new V2Exceptions(ex.ToString(),ex);
           }
       }
       
       #endregion

       #region SearchOutOfOfficeApprovalDateWise
       public DataSet SearchOutOfOfficeApprovalDateWise(OutOfOfficeModel objOutOfOfficeModel)
       {
           DataSet dsSearchOutOfOfficeApprovalDateWise;
           SqlParameter[] objSqlParam = new SqlParameter[4];

           objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
           objSqlParam[0].Value = objOutOfOfficeModel.UserId;


           if (objOutOfOfficeModel.FromDate.ToString() != "1/1/0001 12:00:00 AM")
           {

               objSqlParam[1] = new SqlParameter("@Fromdate", SqlDbType.DateTime);
               objSqlParam[1].Value = objOutOfOfficeModel.FromDate;
           }
           else
           {
               objSqlParam[1] = new SqlParameter("@FromDate", SqlDbType.DateTime);
               objSqlParam[1].Value = null;
           }
           if (objOutOfOfficeModel.ToDate.ToString() != "1/1/0001 12:00:00 AM")
           {
               objSqlParam[2] = new SqlParameter("@Todate", SqlDbType.DateTime);
               objSqlParam[2].Value = objOutOfOfficeModel.ToDate;
           }
           else
           {
               objSqlParam[2] = new SqlParameter("@Todate", SqlDbType.DateTime);
               objSqlParam[2].Value = null;
           }

           objSqlParam[3] = new SqlParameter("@StatusId", SqlDbType.Int);
           objSqlParam[3].Value = objOutOfOfficeModel.StatusId;

           try
           {
               dsSearchOutOfOfficeApprovalDateWise = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchOutOfOfficeApprovalDateWise", objSqlParam);
               return dsSearchOutOfOfficeApprovalDateWise;
           }
           catch (V2Exceptions ex)
           {
               throw;
           }
           catch (System.Exception ex)
           {
               FileLog objFileLog = FileLog.GetLogger();
               objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "SearchOutOfOfficeApproval", ex.StackTrace);
               throw new V2Exceptions(ex.ToString(),ex);
           }
       }


       
       #endregion



       // Out of OFfice Report
       #region GetEmployeeNameRpt
       public DataSet GetEmployeeNameRpt(OutOfOfficeModel objOutOfOfficeModel)
       {
           DataSet dsGetEmployeeNameRpt;
           SqlParameter[] objSqlParam = new SqlParameter[1];

           objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
           objSqlParam[0].Value = objOutOfOfficeModel.UserId;

           try
           {
               dsGetEmployeeNameRpt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetEmployeeNameRpt", objSqlParam);
               return dsGetEmployeeNameRpt;
           }
           catch (V2Exceptions ex)
           {
               throw;
           }
           catch (System.Exception ex)
           {
               FileLog objFileLog = FileLog.GetLogger();
               objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "GetEmployeeNameRpt", ex.StackTrace);
               throw new V2Exceptions(ex.ToString(),ex);
           }

       } 
       #endregion

       #region GetEmployeeNameRptShift
       public DataSet GetEmployeeNameRptShift(OutOfOfficeModel objOutOfOfficeModel)
       {
           DataSet dsGetEmployeeNameRpt;
           SqlParameter[] objSqlParam = new SqlParameter[2];

           objSqlParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
           objSqlParam[0].Value = objOutOfOfficeModel.UserId;
           objSqlParam[1] = new SqlParameter("@ShiftID", SqlDbType.Int);
           objSqlParam[1].Value = objOutOfOfficeModel.ShiftID;


           try
           {
               dsGetEmployeeNameRpt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetEmployeeNameRptShift", objSqlParam);
               return dsGetEmployeeNameRpt;
           }
           catch (V2Exceptions ex)
           {
               throw;
           }
           catch (System.Exception ex)
           {
               FileLog objFileLog = FileLog.GetLogger();
               objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "GetEmployeeNameRptShift", ex.StackTrace);
               throw new V2Exceptions(ex.ToString(),ex);
           }

       } 
       #endregion


       #region SearchOutOfOfficeRpt
       public DataSet SearchOutOfOfficeRpt(OutOfOfficeModel objOutOfOfficeModel, bool IsAdmin, bool AllTeammembers)
       {
           DataSet dsSearchOutOfOfficeRpt;
           SqlParameter[] objSqlParam = new SqlParameter[9];

           objSqlParam[0] = new SqlParameter("@UserId", SqlDbType.Int);
           objSqlParam[0].Value = objOutOfOfficeModel.UserId;

           objSqlParam[1] = new SqlParameter("@period", SqlDbType.NVarChar);
           objSqlParam[1].Value = objOutOfOfficeModel.Period;


           objSqlParam[2] = new SqlParameter("@StatusId", SqlDbType.Int);
           objSqlParam[2].Value = objOutOfOfficeModel.StatusId;

           if (objOutOfOfficeModel.FromDate.ToString() != "1/1/0001 12:00:00 AM")
           {

               objSqlParam[3] = new SqlParameter("@FromDate", SqlDbType.DateTime);
               objSqlParam[3].Value = objOutOfOfficeModel.FromDate;
           }
           else
           {
               objSqlParam[3] = new SqlParameter("@FromDate", SqlDbType.DateTime);
               objSqlParam[3].Value = null;
           }
           if (objOutOfOfficeModel.ToDate.ToString() != "1/1/0001 12:00:00 AM")
           {
               objSqlParam[4] = new SqlParameter("@Todate", SqlDbType.DateTime);
               objSqlParam[4].Value = objOutOfOfficeModel.ToDate;
           }
           else
           {
               objSqlParam[4] = new SqlParameter("@Todate", SqlDbType.DateTime);
               objSqlParam[4].Value = null;
           }

           objSqlParam[5] = new SqlParameter("@Month", SqlDbType.NVarChar);
           objSqlParam[5].Value = objOutOfOfficeModel.Month;

           objSqlParam[6] = new SqlParameter("@Year", SqlDbType.NVarChar);
           objSqlParam[6].Value = objOutOfOfficeModel.Year;

           objSqlParam[7] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
           objSqlParam[7].Value = IsAdmin;

           objSqlParam[8] = new SqlParameter("@AllTeammembers", SqlDbType.Bit);
           objSqlParam[8].Value = AllTeammembers;

           try
           {
               dsSearchOutOfOfficeRpt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchOutOfOfficeRpt", objSqlParam);
               return dsSearchOutOfOfficeRpt;
           }
           catch (V2Exceptions ex)
           {
               throw;
           }
           catch (System.Exception ex)
           {
               FileLog objFileLog = FileLog.GetLogger();
               objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "SearchOutOfOfficeRpt", ex.StackTrace);
               throw new V2Exceptions(ex.ToString(),ex);
           }
       } 
       #endregion

        #region WFGetOutOfOfficeDetails
        public DataSet WFGetOutOfOfficeDetails(int outOfOfficeID)
         {
             try
            {
                  DataSet GetLeaveDetails;
            SqlParameter[] objParam = new SqlParameter[1];
            objParam[0] = new SqlParameter("@OutOfOfficeID",outOfOfficeID );

               GetLeaveDetails=SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "WFGetOutOfOfficeDetails",objParam);
                 return GetLeaveDetails;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "OutOfOfficeDAL.cs", "WFGetOutOfOfficeDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

         }
        #endregion
   }
}
