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
    public class BulkEntriesBOL
    {
        BulkEntriesDAL objBulkDAL = new BulkEntriesDAL();

        #region GetBulkEntries
        public DataSet GetBulkEntries(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                return objBulkDAL.GetBulkEntries(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntriesBOL.cs", "GetBulkEntries", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region AddBulkEntriesDetails
        //public SqlDataReader AddBulkEntriesDetails(SignInSignOutModel objSignInSignOutModel)
        //{
        //    try
        //    {
        //        return objBulkDAL.AddBulkEntriesDetails(objSignInSignOutModel);
        //    }
        //    catch (V2Exceptions ex)
        //    {
        //        throw;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        if (ex.Message.CompareTo("Already leave applied for this dates.") != 0)
        //        {
        //            FileLog objFileLog = FileLog.GetLogger();
        //            objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntriesBOL.cs", "AddBulkEntriesDetails", ex.StackTrace);
        //            throw new V2Exceptions(ex.ToString(),ex);
        //        }
        //        else
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        public int AddBulkEntriesDetails(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                return objBulkDAL.AddBulkEntriesDetails(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex.Message.CompareTo("Already leave applied for this dates.") != 0)
                {
                    FileLog objFileLog = FileLog.GetLogger();
                    objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntriesBOL.cs", "AddBulkEntriesDetails", ex.StackTrace);
                    throw new V2Exceptions(ex.ToString(),ex);
                }
                else
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region GetBulkEntriesForEmployees
        public DataSet GetBulkEntriesForEmployees(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                return objBulkDAL.GetBulkEntriesForEmployees(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntriesBOL.cs", "GetBulkEntriesForEmployees", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region DeleteSignInSignOutDetails
        public int DeleteSignInSignOutDetails(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                return objBulkDAL.DeleteSignInSignOutDetails(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntriesBOL.cs", "DeleteSignInSignOutDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region SearchBulkDetails
        public DataSet SearchBulkDetails(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                return objBulkDAL.SearchBulkDetails(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "BulkEntriesBOL.cs", "SearchBulkDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion
    }
}