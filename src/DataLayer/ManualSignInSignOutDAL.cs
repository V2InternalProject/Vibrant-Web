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
    public class ManualSignInSignOutDAL : DBBaseClass
    {
        DataSet dsManualSignInSignOutDAL = new DataSet();
        //get the Sign In Sign Out Data 
        public DataSet GetOldData(SignInSignOutModel objSignInSignOutModel)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@SignInSignOutID", SqlDbType.Int);
            param[0].Value = objSignInSignOutModel.SignInSignOutID;
            dsManualSignInSignOutDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetSignInSignOutDataFromID", param);
            for (int i = 0; i < dsManualSignInSignOutDAL.Tables[0].Rows.Count; i++)
            {
                if (dsManualSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString() != "")
                {
                    string str = dsManualSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString();
                    char[] chrSplit = { ':' };
                    string[] strArray = new string[2];
                    strArray = str.Split(chrSplit);
                    if (strArray[0] == "##Leave")
                    {
                        str = str.Replace(strArray[0], "");
                        str = str.Replace(":", "");
                        dsManualSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"] = str;
                    }

                }
            }
            return dsManualSignInSignOutDAL;
        }

        public DataSet GetEmployeeJoiningData(SignInSignOutModel objSignInSignOutModel)
        {

            SqlParameter[] param = new SqlParameter[1];
           
            param[0] = new SqlParameter("@UserID", SqlDbType.BigInt);
            param[0].Value = objSignInSignOutModel.EmployeeID;  //@UserID

            dsManualSignInSignOutDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetEmployeeJoiningData", param);
           
            return dsManualSignInSignOutDAL;

        }

         public DataSet CheckAndGetDateData(SignInSignOutModel objSignInSignOutModel)
        {

           SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@SignInTime", SqlDbType.DateTime);
            param[0].Value = objSignInSignOutModel.SignInTime;  //@UserID

             param[1] = new SqlParameter("@UserID", SqlDbType.BigInt);
             param[1].Value = objSignInSignOutModel.EmployeeID;  //@UserID

            dsManualSignInSignOutDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetSignInSignOutData", param);
            for (int i = 0; i < dsManualSignInSignOutDAL.Tables[0].Rows.Count; i++)
            {
                if (dsManualSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString() != "")
                {
                    string str = dsManualSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString();
                    char[] chrSplit = { ':' };
                    string[] strArray = new string[2];
                    strArray = str.Split(chrSplit);
                    if (strArray[0] == "##Leave")
                    {
                        str = str.Replace(strArray[0], "");
                        str = str.Replace(":", "");
                        dsManualSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"] = str;
                    }

                }
            }
            return dsManualSignInSignOutDAL;

        }

        #region not in use
        public int FetchID(SignInSignOutModel objSignInSignOutModel)
        {
            SqlParameter[] param = new SqlParameter[2];

            DateTime NewDate = new DateTime();
            if (objSignInSignOutModel.SignInTime.Date == NewDate)
                param[0] = new SqlParameter("@SignInTime", objSignInSignOutModel.SignOutTime);
            else
                param[0] = new SqlParameter("@SignInTime", objSignInSignOutModel.SignInTime);

            param[1] = new SqlParameter("@EmployeeID", SqlDbType.Int);
            param[1].Value = objSignInSignOutModel.EmployeeID;
            int intSignInSignOutID = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "FindIdOfExistingRecord", param));
            return intSignInSignOutID;

        }
        

        #endregion

        //enter the manual Sign In Sign Out Record 
        public SqlDataReader ModifyAddRecordsInsignInSignOut(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@SignInSignOutID", SqlDbType.Int);
                param[0].Value = objSignInSignOutModel.SignInSignOutID;              
                param[1] = new SqlParameter("@SignInTime", objSignInSignOutModel.SignInTime);
                param[2] = new SqlParameter("@SignOutTime", objSignInSignOutModel.SignOutTime);
                param[3] = new SqlParameter("@IsSignInManual", SqlDbType.Bit);
                param[3].Value = objSignInSignOutModel.IsSignInManual;
                param[4] = new SqlParameter("@IsSignOutManual", SqlDbType.Bit);
                param[4].Value = objSignInSignOutModel.IsSignOutManual;
                param[5] = new SqlParameter("@SignInComment", SqlDbType.NVarChar);
                param[5].Value = objSignInSignOutModel.SignInComment;
                param[6] = new SqlParameter("@SignOutComment", SqlDbType.NVarChar);
                param[6].Value = objSignInSignOutModel.SignOutComment;
                param[7] = new SqlParameter("@EmployeeID", SqlDbType.Int);
                param[7].Value = objSignInSignOutModel.EmployeeID;
               SqlDataReader sdrUpdate =  SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "ModifyAddRecordsInsignInSignOut1", param);
               return sdrUpdate;
            }
            
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOutDAL.cs", "ModifyAddRecordsInsignInSignOut", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }



        }

        #region SignIn

        public SqlDataReader SignIn(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
                param[0].Value = objSignInSignOutModel.EmployeeID;
                param[1] = new SqlParameter("@IsSignInManual", SqlDbType.Bit);
                param[1].Value = objSignInSignOutModel.IsSignInManual;
                param[2] = new SqlParameter("@SignInComments", SqlDbType.NVarChar);
                param[2].Value = objSignInSignOutModel.SignInComment;
                param[3] = new SqlParameter("@SignInTime1", SqlDbType.DateTime);
                param[3].Value = objSignInSignOutModel.SignInTime;
                param[4] = new SqlParameter("@SignOutComments", SqlDbType.NVarChar);
                param[4].Value = objSignInSignOutModel.SignOutComment;
                param[5] = new SqlParameter("@SignOutTime", SqlDbType.DateTime);
                param[5].Value = objSignInSignOutModel.SignOutTime;

                
                SqlDataReader sdrMSignIn =  SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "ManualSignIn", param);
                return sdrMSignIn;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOutDAL", "SignIn", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }


        #endregion

        public SqlDataReader ModifyInTime(SignInSignOutModel objSignInSignOutModel)
        {

            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@SignInSignOutID", SqlDbType.Int);
                param[0].Value = objSignInSignOutModel.SignInSignOutID;               
                param[1] = new SqlParameter("@SignInTime", objSignInSignOutModel.SignInTime);         
                param[2] = new SqlParameter("@IsSignInManual", SqlDbType.Bit);
                param[2].Value = objSignInSignOutModel.IsSignInManual;
                param[3] = new SqlParameter("@SignInComment", SqlDbType.NVarChar);
                param[3].Value = objSignInSignOutModel.SignInComment;
                SqlDataReader sdrModifyInTime = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "ModifyInTime", param);

                return sdrModifyInTime;
             
            }
            
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "ManualSignInSignOutDAL.cs", "ModifyInTime", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }



        }
        
    }
}
