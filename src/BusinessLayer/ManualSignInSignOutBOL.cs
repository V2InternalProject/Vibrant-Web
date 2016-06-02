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
    public class ManualSignInSignOutBOL
    {

        //DataSet dsManualSignInSignOutBOL = new DataSet();
        ManualSignInSignOutDAL objManualSignInSignOutDAL = new ManualSignInSignOutDAL();
        //get the Sign In Sign Out Data 
        public DataSet GetOldData(SignInSignOutModel objSignInSignOutModel)
        {
            /*dsManualSignInSignOutBOL = objManualSignInSignOutDAL.GetOldData(objSignInSignOutModel.SignInSignOutID);
            return dsManualSignInSignOutBOL;*/

            return objManualSignInSignOutDAL.GetOldData(objSignInSignOutModel);
        }
        public DataSet CheckAndGetDateData(SignInSignOutModel objSignInSignOutModel)
        {
          return objManualSignInSignOutDAL.CheckAndGetDateData(objSignInSignOutModel); 

        }
        // code added by Anushree Tirwadkar on 19_4_2010
        public DataSet GetEmployeeJoiningData(SignInSignOutModel objSignInSignOutModel)
        {
            return objManualSignInSignOutDAL.GetEmployeeJoiningData(objSignInSignOutModel);

        }
        //update existing record
        public SqlDataReader ModifyAddRecordsInsignInSignOut(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                return objManualSignInSignOutDAL.ModifyAddRecordsInsignInSignOut(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOutBOL.cs", "ModifyAddRecordsInsignInSignOut", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #region not in use
        public int FetchID(SignInSignOutModel objSignInSignOutModel)
        {
            int intSignInSignOutID = objManualSignInSignOutDAL.FetchID(objSignInSignOutModel);
            return intSignInSignOutID;
        } 
        #endregion

        //add a new record
        public SqlDataReader SignIn(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                return objManualSignInSignOutDAL.SignIn(objSignInSignOutModel);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOutBOL.cs", "SignIn", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        public SqlDataReader ModifyInTime(SignInSignOutModel objSignInSignOutModel)
        {
            return objManualSignInSignOutDAL.ModifyInTime(objSignInSignOutModel);
        }

    }
}
