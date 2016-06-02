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
    public class SignInSignOutBOL
    {
        SignInSignOutDAL objSignInSignOutDAL = new SignInSignOutDAL();
        DataSet dsSignInSignOutBOL = new DataSet();
        #region Sign In
        //check if the employee is on leave -- Employee will not be allowed to sign-in if he has an approved leave 
        public DataSet CheckLeaveDetails(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                dsSignInSignOutBOL = objSignInSignOutDAL.CheckLeaveDetails(objSignInSignOutModel);
                return dsSignInSignOutBOL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL.cs", "CheckLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            } 
        }


         //check for the missing Sign out entries. Employee will not be allowed to sign in unless he has singed out for the corr sign-ins.
        public DataSet CheckMissingSignOut(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
            dsSignInSignOutBOL = objSignInSignOutDAL.CheckMissingSignOut(objSignInSignOutModel);
            return dsSignInSignOutBOL;
                 }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL.cs", "CheckMissingSignOut", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            } 
        }
        

        // Check if the Employee has already Signed_in for the day
        public DataSet CheckForMultipleSignIn(SignInSignOutModel objSignInSignOutModel)
        {
            try
             {
            dsSignInSignOutBOL = objSignInSignOutDAL.CheckForMultipleSignIn(objSignInSignOutModel);
            return dsSignInSignOutBOL;
             }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL.cs", "CheckForMultipleSignIn", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            } 
        
        }

        // after the above 3 functions(conditions) are satisfied(return false) the employee is allowed to sign-in.
        public SqlDataReader SignIn(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
             return objSignInSignOutDAL.SignIn(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL.cs", "SignIn", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            } 
        }
        
        #endregion

        #region BindGrid
        public DataSet BindSignInSignOut(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                dsSignInSignOutBOL = objSignInSignOutDAL.BindSignInSignOut(objSignInSignOutModel);
                return dsSignInSignOutBOL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL.cs", "BindSignInSignOut", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            } 
        }

        public DataSet GetWeeklyOff(SignInSignOutModel objWeeklyOffModel)
        {
            try
            {
                dsSignInSignOutBOL = objSignInSignOutDAL.GetWeeklyOff(objWeeklyOffModel);
                return dsSignInSignOutBOL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL.cs", "GetWeeklyOff", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion
        
        #region SignOut

        // //Check if the Employee has already signed out for the day
        public string MultipleSignOuts(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                string SignInSignOutID = objSignInSignOutDAL.MultipleSignOuts(objSignInSignOutModel);
                return SignInSignOutID;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL.cs", "MultipleSignOuts", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            } 
        }

         //Auto Sign Out
        public SqlDataReader SignOut(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                return objSignInSignOutDAL.SignOut(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL.cs", "SignOut", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            } 
        }

        #endregion
        
        #region Search
        public DataSet Search(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
               dsSignInSignOutBOL = objSignInSignOutDAL.Search(objSignInSignOutModel);
               return dsSignInSignOutBOL;
            }
            
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL", "Search", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            } 
        }
        #endregion

        #region GetBulkEntries
        public DataSet GetBulkEntries(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                dsSignInSignOutBOL = objSignInSignOutDAL.GetBulkEntries(objSignInSignOutModel);
                return dsSignInSignOutBOL;
            }

            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL", "Search", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion
        
        #region Approval

        //display data in the grid
        public DataSet BindApprovalData(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                return objSignInSignOutDAL.BindApprovalData(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL", "BindApprovalData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }
#endregion
        
        //Get Status
        #region GetStatus
        public DataSet GetStatus()
        {
            

            try
            {
                return objSignInSignOutDAL.GetStatus();
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL.cs", "GetStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region UpdateStatus
        public void UpdateStatus(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                objSignInSignOutDAL.UpdateStatus(objSignInSignOutModel);

            }

            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL", "UpdateStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        } 
        #endregion

        // Search Signin Signout Report
        #region SearchSigninSignOutRpt
        public DataSet SearchSigninSignOutRpt(SignInSignOutModel objSignInSignOutModel, bool IsAdmin, bool AllTeammembers)
        {
            try
            {
                return objSignInSignOutDAL.SearchSigninSignOutRpt(objSignInSignOutModel, IsAdmin, AllTeammembers);

            }

            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL", "SearchSigninSignOutRpt", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        } 
        #endregion

        #region SearchApproval
        public DataSet SearchApproval(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                dsSignInSignOutBOL = objSignInSignOutDAL.SearchApproval(objSignInSignOutModel);
                return dsSignInSignOutBOL;
            }

            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL", "SearchApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        } 
        #endregion

        //workflow
        #region WFGetSignInSignOutDetails
        public DataSet WFGetSignInSignOutDetails(int SignInSignOutID)
        {
            try
            {
                return objSignInSignOutDAL.WFGetSignInSignOutDetails(SignInSignOutID);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL", "WFGetSignInSignOutDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        } 
        #endregion

        #region UpdateEmployeeLeaveAndComp
        public int UpdateEmployeeLeaveAndComp(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                return objSignInSignOutDAL.UpdateEmployeeLeaveAndComp(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {

                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutModelBOL.cs", "UpdateEmployeeLeaveAndComp", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);

            }
        }
        #endregion

        #region Admin Approval

        public DataSet GetSISOForAdminApproval(int StatusID, string FromDate, string ToDate, int UserID)
        {
            try
            {
                return objSignInSignOutDAL.GetSISOForAdminApproval(StatusID, FromDate, ToDate,UserID );
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutBOL", "GetSISOForAdminApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }

        #endregion

    }
}
